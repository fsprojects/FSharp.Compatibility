(*

Copyright 2005-2009 Microsoft Corporation
Copyright 2013 Jack Pappas

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

*)

namespace FSharp.Compatibility.OCaml.System.Tests.PowerPack

#nowarn "44"
#nowarn "62"

open System.Collections.Generic
open FSharp.Compatibility.OCaml
open NUnit.Framework


exception Foo

//
[<AutoOpen>]
module private TestHelpers =
    open System

    let test msg b = Assert.IsTrue(b, "MiniTest '" + msg + "'")
    let logMessage msg = 
        System.Console.WriteLine("LOG:" + msg)
//        System.Diagnostics.Trace.WriteLine("LOG:" + msg)
    let check msg v1 v2 = test msg (v1 = v2)
    let reportFailure msg = Assert.Fail msg
    let numActiveEnumerators = ref 0
    let throws f = try f() |> ignore; false with e -> true

    let countEnumeratorsAndCheckedDisposedAtMostOnceAtEnd (seq: seq<'a>) =
       let enumerator() = 
                 numActiveEnumerators := !numActiveEnumerators + 1;
                 let disposed = ref false in
                 let endReached = ref false in
                 let ie = seq.GetEnumerator() in
                 { new System.Collections.Generic.IEnumerator<'a> with 
                      member x.Current =
                          test "rvlrve0" (not !endReached);
                          test "rvlrve1" (not !disposed);
                          ie.Current
                      member x.Dispose() = 
                          test "rvlrve2" !endReached;
                          test "rvlrve4" (not !disposed);
                          numActiveEnumerators := !numActiveEnumerators - 1;
                          disposed := true;
                          ie.Dispose() 
                   interface System.Collections.IEnumerator with 
                      member x.MoveNext() = 
                          test "rvlrve0" (not !endReached);
                          test "rvlrve3" (not !disposed);
                          endReached := not (ie.MoveNext());
                          not !endReached
                      member x.Current = 
                          test "qrvlrve0" (not !endReached);
                          test "qrvlrve1" (not !disposed);
                          box ie.Current
                      member x.Reset() = 
                          ie.Reset()
                   } in

       { new seq<'a> with 
             member x.GetEnumerator() =  enumerator() 
         interface System.Collections.IEnumerable with 
             member x.GetEnumerator() =  (enumerator() :> _) }

    let countEnumeratorsAndCheckedDisposedAtMostOnce (seq: seq<'a>) =
       let enumerator() = 
                 let disposed = ref false in
                 let endReached = ref false in
                 let ie = seq.GetEnumerator() in
                 numActiveEnumerators := !numActiveEnumerators + 1;
                 { new System.Collections.Generic.IEnumerator<'a> with 
                      member x.Current =
                          test "qrvlrve0" (not !endReached);
                          test "qrvlrve1" (not !disposed);
                          ie.Current
                      member x.Dispose() = 
                          test "qrvlrve4" (not !disposed);
                          numActiveEnumerators := !numActiveEnumerators - 1;
                          disposed := true;
                          ie.Dispose() 
                   interface System.Collections.IEnumerator with 
                      member x.MoveNext() = 
                          test "qrvlrve0" (not !endReached);
                          test "qrvlrve3" (not !disposed);
                          endReached := not (ie.MoveNext());
                          not !endReached
                      member x.Current = 
                          test "qrvlrve0" (not !endReached);
                          test "qrvlrve1" (not !disposed);
                          box ie.Current
                      member x.Reset() = 
                          ie.Reset()
                   } in

       { new seq<'a> with 
             member x.GetEnumerator() =  enumerator() 
         interface System.Collections.IEnumerable with 
             member x.GetEnumerator() =  (enumerator() :> _) }

    // Verifies two sequences are equal (same length, equiv elements)
    let verifySeqsEqual seq1 seq2 =
        if Seq.length seq1 <> Seq.length seq2 then Assert.Fail()
        
        let zippedElements = Seq.zip seq1 seq2
        if zippedElements |> Seq.forall (fun (a, b) -> a = b) 
        then ()
        else Assert.Fail()
        
    /// Check that the lamda throws an exception of the given type. Otherwise
    /// calls Assert.Fail()
    let private checkThrowsExn<'a when 'a :> exn> (f : unit -> unit) =
        let funcThrowsAsExpected =
            try
                let _ = f ()
                false // Did not throw!
            with
            | :? 'a
                -> true   // Thew null ref, OK
            | _ -> false  // Did now throw a null ref exception!
        if funcThrowsAsExpected
        then ()
        else Assert.Fail()

    // Illegitimate exceptions. Once we've scrubbed the library, we should add an
    // attribute to flag these exception's usage as a bug.
    let checkThrowsNullRefException      f = checkThrowsExn<NullReferenceException>   f
    let checkThrowsIndexOutRangException f = checkThrowsExn<IndexOutOfRangeException> f

    // Legit exceptions
    let checkThrowsNotSupportedException f = checkThrowsExn<NotSupportedException>    f
    let checkThrowsArgumentException     f = checkThrowsExn<ArgumentException>        f
    let checkThrowsArgumentNullException f = checkThrowsExn<ArgumentNullException>    f
    let checkThrowsKeyNotFoundException  f = checkThrowsExn<KeyNotFoundException>     f
    let checkThrowsDivideByZeroException f = checkThrowsExn<DivideByZeroException>    f
    let checkThrowsInvalidOperationExn   f = checkThrowsExn<InvalidOperationException> f


[<TestFixture>]
module SysTests =  
    [<Test>]
    let TestFileExists () : unit =

        test "dwe098" (not (Sys.file_exists "never-create-me"))

    [<Test>]
    let Test_Sys_remove () : unit =
          let os = open_out "remove-me.txt" in
          close_out os;
          test "dwe098" (Sys.file_exists "remove-me.txt" && (Sys.remove "remove-me.txt"; not (Sys.file_exists "remove-me.txt")))

    [<Test>]
    let Test_Sys_rename () : unit =
          let os = open_out "rename-me.txt" in
          close_out os;
          test "dwe098dw" (Sys.file_exists "rename-me.txt" && (Sys.rename "rename-me.txt" "remove-me.txt"; Sys.file_exists "remove-me.txt" && not (Sys.file_exists "rename-me.txt") && (Sys.remove "remove-me.txt"; not (Sys.file_exists "remove-me.txt"))))

#if FX_NO_ENVIRONMENT
#else
    [<Test>]
    let Test_Sys_getenv () : unit =
          ignore (Sys.getenv "PATH");
          test "w99ocwkm" (try ignore (Sys.getenv "VERY UNLIKELY VARIABLE"); false; with Not_found -> true)
#endif

    [<Test>]
    let Test_Sys_getcwd () : unit =
            
          let p1 = Sys.getcwd() in 
          Sys.chdir "..";
          let p2 = Sys.getcwd() in 
          test "eiojk" (p1 <> p2);
          Sys.chdir p1;
          let p3 = Sys.getcwd() in 
          test "eiojk" (p1 = p3)

#if FX_NO_PROCESS_START
#else
    [<Test>]
    let Test_Sys_command () : unit =

          test "ekj" (Sys.command "help.exe" |> ignore; true)
#endif

    [<Test>]
    let Test_Sys_word_size () : unit =

          test "ekdwq8uj" (Sys.word_size = 32 || Sys.word_size = 64)

#if FX_NO_PROCESS_DIAGNOSTICS
#else
    [<Test>]
    let Test_Sys_time () : unit =

          let t1 = ref (Sys.time()) in 
          for i = 1 to 30 do 
            let t2 = Sys.time() in 
            test "fe921lk30" (!t1 <= t2);
            t1 := t2
          done
#endif