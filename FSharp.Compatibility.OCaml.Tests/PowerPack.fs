﻿(*

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

namespace FSharp.Compatibility.OCaml.Tests.PowerPack

#nowarn "44"
#nowarn "62"

open System
open System.Collections
open System.Collections.Generic
open System.Numerics
open Microsoft.FSharp.Math
open FSharp.Compatibility.OCaml
open FSharp.Compatibility.OCaml.Num
open MathNet.Numerics
open NUnit.Framework


exception Foo

//
[<AutoOpen>]
module private TestHelpers =
    open System

    let test msg (b:bool) = Assert.IsTrue(condition=b, message= "MiniTest '" + msg + "'")
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
module ArrayTests =
    [<Test>]
    let BasicTests () : unit =
        let test_mem () = 
          test "array.contains a" (not (Array.contains 3 [| 1; 2; 4 |]))
          test "array.contains b" (Array.contains 3 [| 1; 3; 4 |])

        let test_make_matrix () = 
          let arr = Array.createJaggedMatrix 2 3 6 in
          test "test2931: sdvjk2" (arr.[0].[0] = 6);
          test "test2931: sdvjk2" (arr.[0].[1] = 6);
          test "test2931: sdvjk2" (arr.[0].[2] = 6);
          test "test2931: sdvjk2" (arr.[1].[0] = 6);
          test "test2931: sdvjk2" (arr.[1].[1] = 6);
          test "test2931: sdvjk2" (arr.[1].[2] = 6);
          arr.[0].[0] <- 5;
          arr.[0].[1] <- 5;
          arr.[0].[2] <- 5;
          arr.[1].[0] <- 4;
          arr.[1].[1] <- 5;
          arr.[1].[2] <- 5;
          test "test2931: sdvjk2" (arr.[1].[0] = 4)

        test_make_matrix ()
        test_mem ()


[<TestFixture>]
module ByteTests =
    [<Test>]
    let BasicTests () : unit =

      test "vwknjewv0" (Byte.zero = 0uy);
      test "vwknjewv0"  (Byte.add 0uy Byte.one = Byte.one);
      test  "vwknjewv0" (Byte.add Byte.one 0uy  = Byte.one);
      test  "vwknjewv0" (Byte.sub Byte.one 0uy  = Byte.one);
      test  "vwknjewv0" (Byte.sub Byte.one Byte.one  = 0uy);
      for i = 0 to 255 do 
        test  "vwknjewv0" (int (byte i) = i);
      for i = 0 to 255 do 
        test  "vwknjewv0" (byte i = byte i);
      stdout.WriteLine "mul i 1";
      for i = 0 to 255 do 
        test  "vwknjewv0"  (Byte.mul (byte i) (byte 1) = byte i);
      stdout.WriteLine "add";
      for i = 0 to 255 do 
        for j = 0 to 255 do 
          test  "vwknjewv0"  (int (Byte.add (byte i) (byte j)) = ((i + j) % 256));
      stdout.WriteLine "mul i 1";
      for i = 0 to 49032 do 
        test  "vwknjewv0"  (int (byte i) = (i % 256));
      for i = 0 to 255 do 
        test  "vwknjewv0"  (Byte.div (byte i) (byte 1) = byte i);
      for i = 0 to 255 do 
        test  "vwknjewv0"  (Byte.rem (byte i) (byte 1) = byte 0);
      for i = 0 to 254 do 
        test  "vwknjewv0"  (Byte.succ (byte i) = Byte.add (byte i) Byte.one);
      for i = 1 to 255 do 
        test  "vwknjewv0"  (Byte.pred (byte i) = Byte.sub (byte i) Byte.one);
      for i = 1 to 255 do 
        for j = 1 to 255 do 
          test  "vwknjewv0"  (int (Byte.logand (byte i) (byte j)) = (&&&) i j);
      stdout.WriteLine "logor";
      for i = 1 to 255 do 
        for j = 1 to 255 do 
          test  "vwknjewv0"  (int (Byte.logor (byte i) (byte j)) = (|||) i j);
      stdout.WriteLine "logxor";
      for i = 1 to 255 do 
        for j = 1 to 255 do 
          test  "vwknjewv0"  (int (Byte.logxor (byte i) (byte j)) = (^^^) i j);
      stdout.WriteLine "lognot";
      for i = 1 to 255 do 
          test  "vwknjewv0"  (Byte.lognot (byte i) = byte (~~~ i))
      stdout.WriteLine "shift_left";
      for i = 0 to 255 do 
        for j = 0 to 7 do 
          test  "vwknjewv0"  (Byte.shift_left (byte i) j = byte ( i <<< j))
      stdout.WriteLine "shift_right";
      for i = 0 to 255 do 
        for j = 0 to 7 do 
          test  "vwknjewv0"  (Byte.shift_right (byte i) j = byte (i >>> j))
      stdout.WriteLine "to_string";
      for i = 0 to 255 do 
          test  "vwknjewv0"  (Byte.to_string (byte i) = sprintf "%d" i)
      stdout.WriteLine "of_string";
      for i = 0 to 255 do 
          test  "vwknjewv0"  (Byte.of_string (string i) = byte i)
      stdout.WriteLine "done";


[<TestFixture>]
module Int32Tests =
    [<Test>]
    let BasicTests () : unit =
      test "test1" (Int32.zero = Int32.zero);
      test "test2" (Int32.add Int32.zero Int32.one = Int32.one);
      test "test3" (Int32.add Int32.one Int32.zero  = Int32.one);
      test "test4" (Int32.sub Int32.one Int32.zero  = Int32.one);
      test "test5" (Int32.sub Int32.one Int32.one  = Int32.zero);
      for i = 0 to 255 do 
        test "test6" (Int32.to_int (Int32.of_int i) = i);
      done;
      for i = 0 to 255 do 
        test "test7" (Int32.of_int i = Int32.of_int i);
      done;
      stdout.WriteLine "mul i 1";
      for i = 0 to 255 do 
        test "test8" (Int32.mul (Int32.of_int i) (Int32.of_int 1) = Int32.of_int i);
      done;
      stdout.WriteLine "add";
      for i = 0 to 255 do 
        for j = 0 to 255 do 
          test "test" (Int32.to_int (Int32.add (Int32.of_int i) (Int32.of_int j)) = (i + j));
        done;
      done;
      stdout.WriteLine "constants: min_int"; stdout.Flush();
      test "testq" (Int32.min_int = -2147483648);
      test "testw" (Int32.min_int = -2147483647 - 1);

      stdout.WriteLine "constants: max_int";stdout.Flush();
      test "teste" (Int32.max_int = 2147483647);
      test "testr" (Int32.max_int = 2147483646 + 1);

      stdout.WriteLine "constants: string max_int";stdout.Flush();
      test "testt" (string Int32.max_int = "2147483647");
      test "testy" (string Int32.min_int = "-2147483648");
      test "testu" (Int32.to_string Int32.max_int = "2147483647");
      test "testi" (Int32.to_string Int32.min_int = "-2147483648");

      stdout.WriteLine "constants: max_int - 10";stdout.Flush();
      test "testa" (Int32.max_int - 10 = 2147483637);

      stdout.WriteLine "min int";stdout.Flush();
      for i = Int32.min_int to Int32.min_int + 10 do 
        test "testb" (Int32.to_int (Int32.of_int i) = i);
      done;
      stdout.WriteLine "max int";stdout.Flush();
      for i = Int32.max_int - 10 to Int32.max_int - 1 do 
        test "testc" (Int32.to_int (Int32.of_int i) = i);
      done;
      stdout.WriteLine "div";
      for i = 0 to 255 do 
        test "testd" (Int32.div (Int32.of_int i) (Int32.of_int 1) = Int32.of_int i);
      done;
      for i = 0 to 255 do 
        test "teste" (Int32.rem (Int32.of_int i) (Int32.of_int 1) = Int32.of_int 0);
      done;
      for i = 0 to 254 do 
        test "testf" (Int32.succ (Int32.of_int i) = Int32.add (Int32.of_int i) Int32.one);
      done;
      for i = 1 to 255 do 
        test "testg" (Int32.pred (Int32.of_int i) = Int32.sub (Int32.of_int i) Int32.one);
      done;
      for i = 1 to 255 do 
        for j = 1 to 255 do 
          test "testh" (Int32.to_int (Int32.logand (Int32.of_int i) (Int32.of_int j)) = (i &&& j));
        done;
      done;
      stdout.WriteLine "logor";
      for i = 1 to 255 do 
        for j = 1 to 255 do 
          test "testj" (Int32.to_int (Int32.logor (Int32.of_int i) (Int32.of_int j)) = (i ||| j));
        done;
      done;
      stdout.WriteLine "logxor";
      for i = 1 to 255 do 
        for j = 1 to 255 do 
          test "testkkh" (Int32.to_int (Int32.logxor (Int32.of_int i) (Int32.of_int j)) = (i ^^^ j));
        done;
      done;
      stdout.WriteLine "lognot";
      for i = 1 to 255 do 
          test "testqf" (Int32.lognot (Int32.of_int i) = Int32.of_int (~~~i))
      done;
      stdout.WriteLine "shift_left";
      for i = 0 to 255 do 
        for j = 0 to 7 do 
          test "testcr4" (Int32.shift_left (Int32.of_int i) j = Int32.of_int (i <<< j))
        done;
      done;
      stdout.WriteLine "shift_right";
      for i = 0 to 255 do 
        for j = 0 to 7 do 
          test "testvt3q" (Int32.shift_right (Int32.of_int i) j = Int32.of_int (i >>> j))
        done;
      done;
      test "testqvt4" (Int32.shift_right 2 1 = 1);
      test "testvq3t" (Int32.shift_right 4 2 = 1);
      stdout.WriteLine "shift_right_logical";
      for i = 0 to 255 do 
        for j = 0 to 7 do 
          test "testvq34" (Int32.shift_right_logical (Int32.of_int i) j = Int32.of_int (Pervasives.(lsr) i j))
        done;
      done;
      test "testvq3t" (Int32.shift_right_logical 0xFFFFFFFF 1 = 0x7FFFFFFF);
      stdout.WriteLine "shift_right_logical (1)";
      test "testvq3" (Int32.shift_right_logical 0xFFFFFFF2 1 = 0x7FFFFFF9);
      stdout.WriteLine "shift_right_logical (2)";
      test "testqvt4" (Int32.shift_right_logical 0x7FFFFFF2 1 = 0x3FFFFFF9);
      stdout.WriteLine "shift_right_logical (3) ";
      test "testqv3t" (Int32.shift_right_logical 0xFFFFFFFF 2 = 0x3FFFFFFF);
      stdout.WriteLine "shift_right_logical (4)";
      test "testb4y5" (Int32.shift_right_logical 0x80000004 2 = 0x20000001);
      stdout.WriteLine "to_string";
      for i = 0 to 255 do 
          test "testbsyet" (Int32.to_string (Int32.of_int i) = string i)
      done;
      stdout.WriteLine "of_string";
      for i = 0 to 255 do 
          test "testvq4" (Int32.of_string (string i) = Int32.of_int i)
      done;
      stdout.WriteLine "constants (hex)";
      test "testv4w" (Int32.of_string "0x0" = 0);
      test "testv35" (Int32.of_string "0x1" = 1);
      test "testvq3" (Int32.of_string "0x2" = 2);
      test "testv3qt" (Int32.of_string "0xa" = 10);
      test "testbwy4" (Int32.of_string "0xff" = 255);
      stdout.WriteLine "constants (octal)";
      test "testb4y5" (Int32.of_string "0o0" = 0);
      test "testb4y" (Int32.of_string "0o1" = 1);
      test "testby4" (Int32.of_string "0o2" = 2);
      test "testbw4y" (Int32.of_string "0o7" = 7);
      test "testb45" (Int32.of_string "0o10" = 8);
      test "testbw4" (Int32.of_string "0o777" = 7*64 + 7*8 + 7);
      test "test67n" (Int32.of_string "0o111" = 64 + 8 + 1);
      stdout.WriteLine "constants (binary)";
      test "test34q" (Int32.of_string "0b0" = 0);
      test "testn" (Int32.of_string "0b1" = 1);
      test "tester" (Int32.of_string "0b10" = 2);
      test "testeyn" (Int32.of_string "0b11" = 3);
      test "testynr" (Int32.of_string "0b00000000" = 0);
      test "testnea" (Int32.of_string "0b11111111" = 0xFF);
      test "testneayr" (Int32.of_string "0b1111111100000000" = 0xFF00);
      test "testne" (Int32.of_string "0b11111111000000001111111100000000" = 0xFF00FF00);
      test "testnaey" (Int32.of_string "0b11111111111111111111111111111111" = 0xFFFFFFFF);
      test "testny" (Int32.of_string "0x7fffffff" = Int32.max_int);


[<TestFixture>]
module UInt32Tests =
    [<Test>]
    let BasicTests () : unit =
      stdout.WriteLine "constants (hex, unit32)";
      test "testv4w" (UInt32.of_string "0x0" = 0u);
      test "testv35" (UInt32.of_string "0x1" = 1u);
      test "testvq3" (UInt32.of_string "0x2" = 2u);
      test "testv3qt" (UInt32.of_string "0xa" = 10u);
      test "testbwy4" (UInt32.of_string "0xff" = 255u);
      stdout.WriteLine "constants (octal, unit32)";
      test "testb4y5" (UInt32.of_string "0o0" = 0u);
      test "testb4y" (UInt32.of_string "0o1" = 1u);
      test "testby4" (UInt32.of_string "0o2" = 2u);
      test "testbw4y" (UInt32.of_string "0o7" = 7u);
      test "testb45" (UInt32.of_string "0o10" = 8u);
      test "testbw4" (UInt32.of_string "0o777" = 7u*64u + 7u*8u + 7u);
      test "test67n" (UInt32.of_string "0o111" = 64u + 8u + 1u);
      stdout.WriteLine "constants (binary, unit32)";

      test "test34q" (UInt32.of_string "0b0" = 0u);
      test "testn" (UInt32.of_string "0b1" = 1u);
      test "tester" (UInt32.of_string "0b10" = 2u);
      test "testeyn" (UInt32.of_string "0b11" = 3u);
      test "testynr" (UInt32.of_string "0b00000000" = 0u);
      test "testnea" (UInt32.of_string "0b11111111" = 0xFFu);
      test "testneayr" (UInt32.of_string "0b1111111100000000" = 0xFF00u);

      test "testne" (UInt32.of_string "0b11111111000000001111111100000000" = 0xFF00FF00u);
      test "testnaey" (UInt32.of_string "0b11111111111111111111111111111111" = 0xFFFFFFFFu);
      test "testny" (UInt32.of_string "0xffffffff" = UInt32.max_int);

      stdout.WriteLine "constants (decimal)";
      test "test" (Int32.of_string "2147483647" = Int32.max_int);
      test "test" (Int32.of_string "-0x80000000" = Int32.min_int);
      test "test" (Int32.of_string "-2147483648" = Int32.min_int);
      stdout.WriteLine "done";
      ()


[<TestFixture>]
module Int64Tests =
    [<Test>]
    let BasicTests () : unit =
          test  "vwknw4vkl"  (Int64.zero = Int64.zero);
          test  "vwknw4vkl"  (Int64.add Int64.zero Int64.one = Int64.one);
          test  "vwknw4vkl"  (Int64.add Int64.one Int64.zero  = Int64.one);
          test  "vwknw4vkl"  (Int64.sub Int64.one Int64.zero  = Int64.one);
          test  "vwknw4vkl"  (Int64.sub Int64.one Int64.one  = Int64.zero);
          for i = 0 to 255 do 
            test  "vwknw4vkl"  (Int64.to_int (Int64.of_int i) = i);
          done;
          for i = 0 to 255 do 
            test  "vwknw4vkl"  (Int64.of_int i = Int64.of_int i);
          done;
          stdout.WriteLine "mul i 1";
          for i = 0 to 255 do 
            test  "vwknw4vkl"  (Int64.mul (Int64.of_int i) (Int64.of_int 1) = Int64.of_int i);
          done;
          stdout.WriteLine "add";
          for i = 0 to 255 do 
            for j = 0 to 255 do 
              test  "vwknw4vkl"  (Int64.to_int (Int64.add (Int64.of_int i) (Int64.of_int j)) = (i + j));
            done;
          done;
          stdout.WriteLine "div";
          for i = 0 to 255 do 
            test  "vwknw4vkl"  (Int64.div (Int64.of_int i) (Int64.of_int 1) = Int64.of_int i);
          done;
          for i = 0 to 255 do 
            test  "vwknw4vkl"  (Int64.rem (Int64.of_int i) (Int64.of_int 1) = Int64.of_int 0);
          done;
          for i = 0 to 254 do 
            test  "vwknw4vkl"  (Int64.succ (Int64.of_int i) = Int64.add (Int64.of_int i) Int64.one);
          done;
          for i = 1 to 255 do 
            test  "vwknw4vkl"  (Int64.pred (Int64.of_int i) = Int64.sub (Int64.of_int i) Int64.one);
          done;
          for i = 1 to 255 do 
            for j = 1 to 255 do 
              test  "vwknw4vkl"  (Int64.to_int (Int64.logand (Int64.of_int i) (Int64.of_int j)) = Pervasives.(land) i j);
            done;
          done;
          stdout.WriteLine "logor";
          for i = 1 to 255 do 
            for j = 1 to 255 do 
              test  "vwknw4vkl"  (Int64.to_int (Int64.logor (Int64.of_int i) (Int64.of_int j)) = Pervasives.(lor) i j);
            done;
          done;
          stdout.WriteLine "logxor";
          for i = 1 to 255 do 
            for j = 1 to 255 do 
              test  "vwknw4vkl"  (Int64.to_int (Int64.logxor (Int64.of_int i) (Int64.of_int j)) = i ^^^ j);
            done;
          done;
          stdout.WriteLine "lognot";
          for i = 1 to 255 do 
              test  "vwknw4vkl"  (Int64.lognot (Int64.of_int i) = Int64.of_int (~~~ i))
          done;
        #if NOTAILCALLS // NOTAILCALLS <-> MONO
        #else
          stdout.WriteLine "shift_left";
          for i = 0 to 255 do 
            for j = 0 to 7 do 
              test  "vwknw4vkl"  (Int64.shift_left (Int64.of_int i) j = Int64.of_int (i <<< j))
            done;
          done;
          stdout.WriteLine "shift_right";
          for i = 0 to 255 do 
            for j = 0 to 7 do 
              test  "vwknw4vkl"  (Int64.shift_right (Int64.of_int i) j = Int64.of_int (i >>> j))
            done;
          done;
          stdout.WriteLine "shift_right_logical";
          for i = 0 to 255 do 
            for j = 0 to 7 do 
              test  "vwknw4vkl"  (Int64.shift_right_logical (Int64.of_int i) j = Int64.of_int (Pervasives.(lsr) i j))
            done;
          done;
        #endif
          stdout.WriteLine "to_string";
          for i = 0 to 255 do 
              test  "vwknw4vkl"  (Int64.to_string (Int64.of_int i) = string i)
          done;
          stdout.WriteLine "of_string";
          for i = 0 to 255 do 
              test  "vwknw4vkl"  (Int64.of_string (string i) = Int64.of_int i)
          done;
          stdout.WriteLine "constants (hex)";
          test  "vwknw4vkl"  (Int64.of_string "0x0" = 0L);
          test  "vwknw4vkl"  (Int64.of_string "0x1" = 1L);
          test  "vwknw4vkl"  (Int64.of_string "0x2" = 2L);
          test  "vwknw4vkl"  (Int64.of_string "0xa" = 10L);
          test  "vwknw4vkl"  (Int64.of_string "0xff" = 255L);
          stdout.WriteLine "constants (octal)";
          test  "vwknw4vkl"  (Int64.of_string "0o0" = 0L);
          test  "vwknw4vkl"  (Int64.of_string "0o1" = 1L);
          test  "vwknw4vkl"  (Int64.of_string "0o2" = 2L);
          test  "vwknw4vkl"  (Int64.of_string "0o7" = 7L);
          test  "vwknw4vkl"  (Int64.of_string "0o10" = 8L);
          test  "vwknw4vkl"  (Int64.of_string "0o777" = 7L*64L + 7L*8L + 7L);
          test  "vwknw4vkl"  (Int64.of_string "0o111" = 64L + 8L + 1L);
          stdout.WriteLine "constants (binary)";
          test  "vwknw4vkl"  (Int64.of_string "0b0" = 0L);
          test  "vwknw4vkl"  (Int64.of_string "0b1" = 1L);
          test  "vwknw4vkl"  (Int64.of_string "0b10" = 2L);
          test  "vwknw4vkl"  (Int64.of_string "0b11" = 3L);
          test  "vwknw4vkl"  (Int64.of_string "0b00000000" = 0L);
          test  "vwknw4vkl"  (Int64.of_string "0b11111111" = 0xFFL);
          test  "vwknw4vkl"  (Int64.of_string "0b1111111100000000" = 0xFF00L);
          test  "vwknw4vkl"  (Int64.of_string "0b11111111000000001111111100000000" = 0xFF00FF00L);
          test  "vwknw4vkl"  (Int64.of_string "0b11111111111111111111111111111111" = 0xFFFFFFFFL);
          test  "vwknw4vkl"  (Int64.of_string "0b1111111100000000111111110000000011111111000000001111111100000000" = 0xFF00FF00FF00FF00L);
          test  "vwknw4vkl"  (Int64.of_string "0b1111111111111111111111111111111111111111111111111111111111111111" = 0xFFFFFFFFFFFFFFFFL);

          stdout.WriteLine "of_string: min_int";
          test  "vwknw4vkl"  (Int64.of_string "-0x8000000000000000" = Int64.min_int);
          test  "vwknw4vkl"  (Int64.of_string "-9223372036854775808" = Int64.min_int);
          test  "vwknw4vkl"  (-9223372036854775808L = Int64.min_int);
          stdout.WriteLine "done";

          stdout.WriteLine "constants (hex, UInt64)";
          test  "vwknw4vkl"  (UInt64.of_string "0x0" = 0UL);
          test  "vwknw4vkl"  (UInt64.of_string "0x1" = 1UL);
          test  "vwknw4vkl"  (UInt64.of_string "0x2" = 2UL);
          test  "vwknw4vkl"  (UInt64.of_string "0xa" = 10UL);
          test  "vwknw4vkl"  (UInt64.of_string "0xff" = 255UL);
          test  "vwknw4vkl"  (UInt64.of_string "0xffffffffffffffff" = UInt64.max_int);
          
          stdout.WriteLine "constants (octal, UInt64)";
          test  "vwknw4vkl"  (UInt64.of_string "0o0" = 0UL);
          test  "vwknw4vkl"  (UInt64.of_string "0o1" = 1UL);
          test  "vwknw4vkl"  (UInt64.of_string "0o2" = 2UL);
          test  "vwknw4vkl"  (UInt64.of_string "0o7" = 7UL);
          test  "vwknw4vkl"  (UInt64.of_string "0o10" = 8UL);
          test  "vwknw4vkl"  (UInt64.of_string "0o777" = 7UL*64UL + 7UL*8UL + 7UL);
          test  "vwknw4vkl"  (UInt64.of_string "0o111" = 64UL + 8UL + 1UL);
          
          stdout.WriteLine "constants (binary, UInt64)";
          test  "vwknw4vkl"  (UInt64.of_string "0b0" = 0UL);
          test  "vwknw4vkl"  (UInt64.of_string "0b1" = 1UL);
          test  "vwknw4vkl"  (UInt64.of_string "0b10" = 2UL);
          test  "vwknw4vkl"  (UInt64.of_string "0b11" = 3UL);
          test  "vwknw4vkl"  (UInt64.of_string "0b00000000" = 0UL);
          test  "vwknw4vkl"  (UInt64.of_string "0b11111111" = 0xFFUL);
          test  "vwknw4vkl"  (UInt64.of_string "0b1111111100000000" = 0xFF00UL);
          test  "vwknw4vkl"  (UInt64.of_string "0b11111111000000001111111100000000" = 0xFF00FF00UL);
          test  "vwknw4vkl"  (UInt64.of_string "0b11111111111111111111111111111111" = 0xFFFFFFFFUL);
          test  "vwknw4vkl"  (UInt64.of_string "0b1111111100000000111111110000000011111111000000001111111100000000" = 0xFF00FF00FF00FF00UL);
          test  "vwknw4vkl"  (UInt64.of_string "0b1111111111111111111111111111111111111111111111111111111111111111" = 0xFFFFFFFFFFFFFFFFUL);

          stdout.WriteLine "done";


[<TestFixture>]
module ListCompatTests =
    [<Test>]
    let BasicTests () : unit =
        test "List.tryfind_indexi" (List.tryFindIndex ((=) 4) [1;2;3;4;4;3;2;1] = Some 3)


[<TestFixture>]
module PervasivesTests =
    [<Test>]
    let ExceptionMappings () : unit =
        check "exception mappings (int)"  true (try int_of_string "A" |> ignore; false with Failure _ -> true | _ -> false)
        check "exception mappings (float)"  true (try float_of_string "A" |> ignore; false with Failure _ -> true | _ -> false)

    [<Test>]
    let BasicTests () : unit =
        test "tefwiu32" (try raise Not_found with Not_found -> true | _ -> false)

        test "tefw38vj" (try raise Out_of_memory with Out_of_memory -> true | _ -> false)

        test "tefw93mvj" (try raise Division_by_zero with Division_by_zero -> true | _ -> false)

        test "tefwfewevj" (try raise Stack_overflow with Stack_overflow -> true | _ -> false)

        test "tefw9ifmevj" (try raise End_of_file with End_of_file -> true | _ -> false)

        test "atefwiu32" (try raise Not_found with Out_of_memory | Division_by_zero | Stack_overflow | End_of_file -> false | Not_found -> true | _ -> false)

        test "btefwiu32" (try raise Out_of_memory with Not_found | Division_by_zero | Stack_overflow | End_of_file -> false | Out_of_memory -> true | _ -> false)

        test "ctefwiu32" (try raise Division_by_zero with Not_found | Out_of_memory | Stack_overflow | End_of_file -> false | Division_by_zero -> true | _ -> false)

        test "dtefwiu32" (try raise Stack_overflow with Not_found | Out_of_memory | Division_by_zero | End_of_file -> false | Stack_overflow -> true | _ -> false)

        test "etefwiu32" (try raise End_of_file with Not_found | Out_of_memory | Division_by_zero |  Stack_overflow -> false | End_of_file -> true | _ -> false)

        test "ftefwiu32" (try raise Foo with Not_found | Out_of_memory | Division_by_zero |  Stack_overflow -> false | Foo -> true | _ -> false)
  

    let private checkFileContentsUsingVariousTechniques(filename) =
        using (open_in_bin filename) <| fun is -> 
            let buf = Array.create 5 0uy in
            check "cewjk1" (input is buf 0 5) 5;
            check "cewjk2" buf [|104uy; 101uy; 108uy; 108uy; 111uy|];
            check "cewjk3" (input is buf 0 2) 2;
            check "cewjk4" buf [|13uy; 10uy; 108uy; 108uy; 111uy|]

        using (open_in_bin filename) <| fun is2 -> 
            check "cewjk5" (is2.Peek()) 104;
            check "cewjk6" (is2.Read()) 104;
            check "cewjk7" (is2.Read()) 101;
            check "cewjk8" (is2.Read()) 108;
            check "cewjk9" (is2.Read()) 108;
            check "cewjk0" (is2.Read()) 111;
            check "cewjkq" (is2.Read()) 13;
            check "cewjkw" (is2.Read()) 10;
            check "cewjke" (is2.Read()) (-1)

        using (open_in_bin filename) <| fun is3 -> 
            check "cewjkr" (input_char is3) 'h';
            check "cewjkt" (input_char is3) 'e';
            check "cewjky" (input_char is3) 'l';
            check "cewjku" (input_char is3) 'l';
            check "cewjki" (input_char is3) 'o';
            check "cewjko" (input_char is3) '\r';
            check "cewjkp" (input_char is3) '\n';
            check "cewjka" (try input_char is3 |> ignore; false with End_of_file -> true) true

        using (open_in_bin filename) <| fun is4 -> 
            let buf4 = Array.create 5 '0' in
            check "cewjks" (input_chars is4 buf4 0 5) 5;
            check "cewjkd" (buf4) [|'h'; 'e'; 'l'; 'l'; 'o'; |];
            check "cewjkf" (input_chars is4 buf4 0 2) 2;
            check "cewjkd" (buf4) [|'\r'; '\n'; 'l'; 'l'; 'o'; |];
            check "cewjkh" (input_chars is4 buf4 0 2) 0
            
        using (open_in filename) <| fun is5 -> 
            let buf5 = Array.create 5 0uy in
            check "veswhek1" (input is5 buf5 0 5) 5;
            check "veswhek2" buf5 [|104uy; 101uy; 108uy; 108uy; 111uy|];
            check "veswhek3" (input is5 buf5 0 2)  2;
            check "veswhek4" buf5 [|13uy; 10uy; 108uy; 108uy; 111uy|];
            check "veswhek5" (input is5 buf5 0 2) 0

        using (open_in filename) <| fun is2 -> 
            check "veswhek6" (is2.Peek()) 104;
            check "veswhek7" (is2.Read()) 104;
            check "veswhek8" (is2.Read()) 101;
            check "veswhek9" (is2.Read()) 108;
            check "veswhek0" (is2.Read()) 108;
            check "veswhekq" (is2.Read()) 111;
            check "veswhekw" (is2.Read()) 13;
            check "veswheke" (is2.Read()) 10;
            check "veswhekr" (is2.Read()) (-1)

        using (open_in filename) <| fun is3 -> 
            check "veswhekt" (input_char is3) 'h';
            check "veswheky" (input_char is3) 'e';
            check "veswheku" (input_char is3) 'l';
            check "veswheko" (input_char is3) 'l';
            check "veswhekp" (input_char is3) 'o';
            check "veswheka" (input_char is3) '\r';
            check "veswheks" (input_char is3) '\n';
            check "veswhekd" (try input_char is3 |> ignore; false with End_of_file -> true) true

    [<Test>]
    let IO_EndOfLine_Translations () : unit =
        using (open_out_bin "test.txt") <| fun os ->
            fprintf os "hello\r\n"
        checkFileContentsUsingVariousTechniques("test.txt")

        using (open_out "test.txt") <| fun os ->
            fprintf os "hello\r\n"
        checkFileContentsUsingVariousTechniques("test.txt")
        
        using (open_out "test.txt") <| fun os ->
            os.Write (let s = "hello\r\n" in Array.init s.Length (fun i -> s.[i]) )
        checkFileContentsUsingVariousTechniques("test.txt")
        
        using (open_out_bin "test.txt") <| fun os ->
            os.Write (let s = "hello\r\n" in Array.init s.Length (fun i -> s.[i]) )
        checkFileContentsUsingVariousTechniques("test.txt")
        
        using (open_out "test.txt") <| fun os ->
            os.Write "hello\r\n"
        checkFileContentsUsingVariousTechniques("test.txt")
        
        using (open_out_bin "test.txt") <| fun os ->
            os.Write "hello\r\n"
        checkFileContentsUsingVariousTechniques("test.txt")

#if FX_NO_BINARY_SERIALIZATION
#else
//    [<Test>]
//    let BinarySerialization () : unit =
//
//        (* Andrez:
//           It appears to me that writing the empty list into a binary channel does not work.
//        *)   
//          
//          let file = open_out_bin "test.txt" in
//          output_value file ([]: int list);
//          close_out file;
//          let file = open_in_bin "test.txt" in
//          if (input_value file : int list) <> [] then (reportFailure "wnwve0ljkvwe");
//          close_in file;
#endif


[<TestFixture>]
module FilenameTests =
    [<Test>]
    let dirname () : unit =
        // These tests only work on Windows.
        // For now, we ignore them when running on other platforms, but it'd be nice to have some cross-platform tests too.
        if System.Environment.OSVersion.Platform <> System.PlatformID.Win32NT then
            Assert.Ignore "These tests are currently designed to run on Windows, so they will be skipped on this platform."
        else
            check "Filename.dirname1"  "C:" (Filename.dirname "C:")
            check "Filename.dirname2"  "C:\\" (Filename.dirname "C:\\")
            check "Filename.dirname3"  "c:\\" (Filename.dirname "c:\\")
            check "Filename.dirname2"  "C:/" (Filename.dirname "C:/")
            check "Filename.dirname3"  "c:/" (Filename.dirname "c:/")
            check "Filename.dirname4"  "." (Filename.dirname "")
            check "Filename.dirname5"  "\\" (Filename.dirname "\\")
            check "Filename.dirname6"  "." (Filename.dirname "a")
            // F# and OCaml do return different results for this one.
            // F# preserves the double slashes.  That seems fair enough 
            // do check "Filename.dirname2"  "\\" (Filename.dirname "\\\\")

            check "Filename.dirname1"  "" (Filename.basename "C:")
            check "Filename.dirname2"  "" (Filename.basename "C:\\")
            check "Filename.dirname2"  "" (Filename.basename "c:\\")
            check "Filename.dirname2"  "" (Filename.basename "")
            check "Filename.dirname2"  "c" (Filename.basename "\\\\c")
            check "Filename.dirname2"  "" (Filename.basename "\\\\")

    [<Test>]
    let is_relative () : unit =
        // These tests only work on Windows.
        // For now, we ignore them when running on other platforms, but it'd be nice to have some cross-platform tests too.
        if System.Environment.OSVersion.Platform <> System.PlatformID.Win32NT then
            Assert.Ignore "These tests are currently designed to run on Windows, so they will be skipped on this platform."
        else
            check "is_relative1"  false (Filename.is_relative "C:")
            check "is_relative2"  false (Filename.is_relative "C:\\")
            check "is_relative3"  false (Filename.is_relative "c:\\")
            check "is_relative4"  false (Filename.is_relative "C:/")
            check "is_relative5"  false (Filename.is_relative "c:/")
            check "is_relative6"  true (Filename.is_relative "")
            check "is_relative7"  true (Filename.is_relative ".")
            check "is_relative8"  true (Filename.is_relative "a")
            check "is_relative9"  false (Filename.is_relative "\\")
            check "is_relative10"  false (Filename.is_relative "\\\\")

            check "is_relative8"  true (Filename.is_implicit "a")
            check "is_relative8"  false (Filename.is_implicit ".\\a")
            check "is_relative8"  false (Filename.is_implicit "..\\a")

    [<Test>]
    let has_extension () : unit =
        // These tests only work on Windows.
        // For now, we ignore them when running on other platforms, but it'd be nice to have some cross-platform tests too.
        if System.Environment.OSVersion.Platform <> System.PlatformID.Win32NT then
            Assert.Ignore "These tests are currently designed to run on Windows, so they will be skipped on this platform."
        else
            let has_extension (s:string) = 
              (String.length s >= 1 && String.get s (String.length s - 1) = '.') 
              || System.IO.Path.HasExtension(s)

            check "has_extension 1"  false (has_extension "C:")
            check "has_extension 2"  false (has_extension "C:\\")
            check "has_extension 3"  false (has_extension "c:\\")
            check "has_extension 4"  false (has_extension "")
            check "has_extension 5"  true (has_extension ".")
            check "has_extension 6"  false (has_extension "a")
            check "has_extension 7"  true (has_extension "a.b")
            check "has_extension 8"  true (has_extension ".b")
            check "has_extension 9"  true (has_extension "c:\\a.b")
            check "has_extension 10"  true (has_extension "c:\\a.")

    [<Test>]
    let chop_extension () : unit =
        // These tests only work on Windows.
        // For now, we ignore them when running on other platforms, but it'd be nice to have some cross-platform tests too.
        if System.Environment.OSVersion.Platform <> System.PlatformID.Win32NT then
            Assert.Ignore "These tests are currently designed to run on Windows, so they will be skipped on this platform."
        else
            check "chop_extension1"  true (try ignore(Filename.chop_extension "C:"); false with Invalid_argument  "chop_extension" -> true)
            check "chop_extension2"  true (try ignore(Filename.chop_extension "C:\\"); false with Invalid_argument  "chop_extension" -> true)
            check "chop_extension3"  true (try ignore(Filename.chop_extension "c:\\"); false with Invalid_argument  "chop_extension" -> true)
            check "chop_extension4"  true (try ignore(Filename.chop_extension "C:/"); false with Invalid_argument  "chop_extension" -> true)
            check "chop_extension5"  true (try ignore(Filename.chop_extension "c:/"); false with Invalid_argument  "chop_extension" -> true)
            check "chop_extension6"  true (try ignore(Filename.chop_extension ""); false with Invalid_argument  "chop_extension" -> true)
            check "chop_extension7"  true (try ignore(Filename.chop_extension "a"); false with Invalid_argument  "chop_extension" -> true)
            check "chop_extension8"  true (try ignore(Filename.chop_extension "c:\\a"); false with Invalid_argument  "chop_extension" -> true)
            check "chop_extension9"  true (try ignore(Filename.chop_extension "c:\\foo.b\\a"); false with Invalid_argument  "chop_extension" -> true)
            check "chop_extension10"  "" (Filename.chop_extension ".")
            check "chop_extension11"  "a" (Filename.chop_extension "a.")
            check "chop_extension12"  "c:\\a" (Filename.chop_extension "c:\\a.")        


#if FX_NO_DOUBLE_BIT_CONVERTER
#else
[<TestFixture>]
module FloatTests =
  
    [<Test>]
    let BasicTests () : unit =
        check "FloatParse.1" (Float.to_bits (Float.of_string "0.0")) 0L
        check "FloatParse.0" (Float.to_bits (Float.of_string "-0.0"))      0x8000000000000000L // (-9223372036854775808L)
        check "FloatParse.2" (Float.to_bits (Float.of_string "-1E-127"))   0xa591544581b7dec2L // (-6516334528322609470L)
        check "FloatParse.3" (Float.to_bits (Float.of_string "-1E-323"))   0x8000000000000002L // (-9223372036854775806L)
        check "FloatParse.4" (Float.to_bits (Float.of_string "-1E-324"))   0x8000000000000000L // (-9223372036854775808L)
        check "FloatParse.5" (Float.to_bits (Float.of_string "-1E-325"))   0x8000000000000000L // (-9223372036854775808L)
        check "FloatParse.6" (Float.to_bits (Float.of_string "1E-325")) 0L
        check "FloatParse.7" (Float.to_bits (Float.of_string "1E-322")) 20L
        check "FloatParse.8" (Float.to_bits (Float.of_string "1E-323")) 2L
        check "FloatParse.9" (Float.to_bits (Float.of_string "1E-324")) 0L
        check "FloatParse.A" (Float.to_bits (Float.of_string "Infinity"))  0x7ff0000000000000L // 9218868437227405312L
        check "FloatParse.B" (Float.to_bits (Float.of_string "-Infinity")) 0xfff0000000000000L // (-4503599627370496L)
        check "FloatParse.C" (Float.to_bits (Float.of_string "NaN"))       0xfff8000000000000L  // (-2251799813685248L)
        check "FloatParse.D" (Float.to_bits (Float.of_string "-NaN"))    ( // http://en.wikipedia.org/wiki/NaN
                                                                  let bit64 = System.IntPtr.Size = 8 in
                                                                  if bit64 && System.Environment.Version.Major < 4 then
                                                                      // 64-bit (on NetFx2.0) seems to have same repr for -nan and nan
                                                                      0xfff8000000000000L // (-2251799813685248L)
                                                                  else
                                                                      // 64-bit (on NetFx4.0) and 32-bit (any NetFx) seems to flip the sign bit on negation.
                                                                      // However:
                                                                      // it seems nan has the negative-bit set from the start,
                                                                      // and -nan then has the negative-bit cleared!
                                                                      0x7ff8000000000000L // 9221120237041090560L
                                                                )
#endif

#if FX_NO_COMMAND_LINE_ARGS
#else
[<TestFixture>]
module ArgTests =
  
    [<Test>]
    let BasicTests () : unit =

      let res = System.Text.StringBuilder()
      let add (x:string) = res.Append("<"^x^">") |> ignore
      FSharp.Compatibility.OCaml.Arg.parse_argv (ref 0) [|"main.exe";"otherA";"";"otherB"|] [] add "fred"
      check "Bug3803" (res.ToString()) "<otherA><><otherB>"
#endif  


[<TestFixture>]
module FuncConvertTests =  
    [<Test>]
    let BasicTests () : unit =

        check "dwe098ce1" ((Microsoft.FSharp.Core.FuncConvert.FuncFromTupled(fun (a,b) -> a + b)) 3 4) 7
        check "dwe098ce2" ((Microsoft.FSharp.Core.FuncConvert.FuncFromTupled(fun (a,b,c) -> a + b + c)) 3 4 5) 12
        check "dwe098ce3" ((Microsoft.FSharp.Core.FuncConvert.FuncFromTupled(fun (a,b,c,d) -> a + b + c + d)) 3 4 5 5) 17
        check "dwe098ce4" ((Microsoft.FSharp.Core.FuncConvert.FuncFromTupled(fun (a,b,c,d,e) -> a + b + c + d + e)) 3 4 5 5 5) 22

        check "dwe098ce1" ((Microsoft.FSharp.Core.FuncConvert.ToFSharpFunc(System.Converter(fun a -> a + 1))) 3) 4
        check "dwe098ce1" ((Microsoft.FSharp.Core.FuncConvert.ToFSharpFunc(System.Action<_>(fun a -> ()))) 3) ()


[<TestFixture>]
module BigRationalTests =

    // BigRational Tests
    // =================

    // Notes: What cases to consider?
    //   For (p,q) cases q=0, q=1, q<>1. [UPDATE: remove (x,0)]
    //   For (p,q) when q=1 there could be 2 internal representations, either Z or Q.
    //   For (p,0) this value can be signed, corresponds to +/- infinity point. [Update: remove it]
    // Hashes on (p,1) for both representations must agree.
    // For binary operators, try for result with and without HCF (normalisation).
    // Also: 0/0 is an acceptable representation. See normalisation code. [Update: remove it].

    // Overrides to test:
    // .ToString()
    // .GetHashCode()
    // .Equals()
    // IComparable.CompareTo()

    // Misc construction.
    let natA  n   = BigRational.FromInt n       // internally Z
    let natB  n   = (natA n / natA 7) * natA 7  // internally Q
    let ratio p q = BigRational.FromInt p / BigRational.FromInt q
    let (/%)  b c = BigRational.FromBigInt b / BigRational.FromBigInt c

    // Misc test values
    let q0 = natA 0
    let q1 = natA 1
    let q2 = natA 2
    let q3 = natA 3
    let q4 = natA 4
    let q5 = natA 5
    let minIntI = bigint System.Int32.MinValue
    let maxIntI = bigint System.Int32.MaxValue
    let ran = System.Random()
    let nextZ n = bigint (ran.Next(n))

    // A selection of test points.
    let points =
        // A selection of integer and reciprical points
        let points =
            [for i in -13I .. 13I -> i,1I] @
            [for i in -13I .. 13I -> 1I,i]
        // Exclude x/0
        let points = [for p,q in points do if q <> 0I then yield p,q ] // PROPOSE: (q,0) never a valid Q value, filter them out of tests...
        // Scale by various values, including into BigInt range
        let scale (kp,kq) (p,q) = (p*kp,q*kq)
        let scales k pqs = List.map (scale k) pqs
        let points = List.concat [points;
                                  scales (10000I,1I) points;
                                  scales (1I,10000I) points;
                                  scales (maxIntI,1I) points;
                                  scales (1I,maxIntI) points;
                                 ]
        points
    let pointsNonZero = [for p,q in points do if p<>0I then yield p,q] // non zero points

    let makeQs p q =     
        if q = 1I && minIntI <= p && p <= maxIntI then
            // (p,1) where p is int32
            let p32 = int32 p
            [natA p32;natB p32;BigRational.FromBigInt p]   // two reprs for int32 
        else
            [BigRational.FromBigInt p / BigRational.FromBigInt q]
            
    let miscQs = [for p,q in points do yield! makeQs p q]

    let product xs ys = [for x in xs do for y in ys do yield x,y]
    let vector1s = [for z in points -> z]
    let vector2s = product points points

    [<Test>]
    let BasicTests1 () : unit =
        check "generic format h"  "1N" (sprintf "%A" 1N)
        check "generic format q"  "-1N" (sprintf "%A" (-1N))

        test "vliwe98"   (id -2N = - 2N)
        test "d3oc002" (LanguagePrimitives.GenericZero<bignum> = 0N)
        test "d3oc112w" (LanguagePrimitives.GenericOne<bignum> = 1N)

        check "weioj3h" (sprintf "%O" 3N) "3" 
        check "weioj3k" (sprintf "%O" (3N / 4N)) "3/4"
        check "weioj3k" (sprintf "%O" (3N / 400000000N)) "3/400000000"
        check "weioj3l" (sprintf "%O" (3N / 3N))  "1"
        check "weioj3q" (sprintf "%O" (-3N))  "-3"
        //check "weioj3w" (sprintf "%O" -3N) "-3"
        check "weioj3e" (sprintf "%O" (-3N / -3N)) "1"

        // The reason why we do not use hardcoded values is the the representation may change based on the NetFx we are targeting.
        // For example, when targeting NetFx4.0, the result is "-3E+61" instead of "-3000....0N"
        let v = -30000000000000000000000000000000000000000000000000000000000000N
        check "weioj3r" (sprintf "%O" v) ((box v).ToString())

  
    [<Test>]
    let BasicTests2 () : unit =


        // Test arithmetic ops: tests
        let test2One name f check ((p,q),(pp,qq)) =     
            // There may be several ways to construct the test rationals
            let zs        = makeQs p  q
            let zzs       = makeQs pp qq
            let results   = [for z in zs do for zz in zzs do yield f (z,zz)]    
            let refP,refQ = check (p,q) (pp,qq)    
            let refResult = BigRational.FromBigInt refP / BigRational.FromBigInt refQ
            let resOK (result:BigRational) = 
                result.Numerator * refQ = refP * result.Denominator && 
                BigRational.Equals(refResult,result)
            match List.tryFind (fun result -> not (resOK result)) results with
            | None        -> () // ok
            | Some result -> printf "Test failed. %s (%A,%A) (%A,%A). Expected %A. Observed %A\n" name p q pp qq refResult result
                             reportFailure "cejkew09"

        let test2All name f check vectors = List.iter (test2One name f check) vectors

        // Test arithmetic ops: call
        test2All "add" (BigRational.(+))  (fun (p,q) (pp,qq) -> (p*qq + q*pp,q*qq)) vector2s
        test2All "sub" (BigRational.(-))  (fun (p,q) (pp,qq) -> (p*qq - q*pp,q*qq)) vector2s
        test2All "mul" (BigRational.(*))  (fun (p,q) (pp,qq) -> (p*pp,q*qq))        vector2s // *) <-- for EMACS
        test2All "div" (BigRational.(/))  (fun (p,q) (pp,qq) -> (p*qq,q*pp))        (product points pointsNonZero)


  
    [<Test>]
    let RangeTests () : unit =
        // Test x0 .. dx .. x1
        let checkRange3 (x0:BigRational) dx x1 k =
            let f (x:BigRational) = x * BigRational.FromBigInt k |> BigRational.ToBigInt
            let rangeA = {x0 .. dx .. x1} |> Seq.map f
            let rangeB = {f x0 .. f dx .. f x1}
            //printf "Length=%d\n" (Seq.length rangeA)
            let same = Seq.forall2 (=) rangeA rangeB
            check (sprintf "Range3 %A .. %A .. %A scaled to %A" x0 dx x1 k) same true
            
        checkRange3 (0I /% 1I)  (1I /% 7I) (100I /% 1I)  (7I*1I)
        checkRange3 (0I /% 1I)  (1I /% 7I) (100I /% 11I) (7I*11I)
        checkRange3 (1I /% 13I) (1I /% 7I) (100I /% 11I) (7I*11I*13I)
        for i = 0 to 1000 do
            let m = 1000 // max steps is -m to m in steps of 1/m i.e. 2.m^2
            let p0,q0 = nextZ m     ,nextZ m + 1I
            let p1,q1 = nextZ m     ,nextZ m + 1I
            let pd,qd = nextZ m + 1I,nextZ m + 1I
            checkRange3 (p0 /% q0) (pd /% qd) (p1 /% q1) (q0 * q1 * qd)


        // Test x0 .. x1
        let checkRange2 (x0:BigRational) x1 =
            let z0  = BigRational.ToBigInt x0
            let z01 = BigRational.ToBigInt (x1 - x0)    
            let f (x:BigRational) = x |> BigRational.ToBigInt
            let rangeA = [x0 .. x1] |> List.map f       // range with each item rounded down
            let rangeB = [z0 .. z0 + z01]               // range of same length from the round down start point
            check (sprintf "Range2: %A .. %A" x0 x1) rangeA rangeB

        checkRange2 (0I /% 1I)  (100I /% 1I)  
        checkRange2 (0I /% 1I)  (100I /% 11I) 
        checkRange2 (1I /% 13I) (100I /% 11I) 
        for i = 0 to 1000 do
            let m = 10000 // max steps is -m to m in steps of 1 i.e. 2.m
            let p0,q0 = nextZ m     ,nextZ m + 1I
            let p1,q1 = nextZ m     ,nextZ m + 1I
            checkRange2 (p0 /% q0) (p1 /% q1) //(q0 * q1 * qd)

        // ToString()
        // Cases: integer, computed integer, rational<1, rational>1, +/-infinity, nan
        (natA 1).ToString()     |> check "ToString" "1" 
        (natA 0).ToString()     |> check "ToString"  "0"
        (natA (-12)).ToString() |> check "ToString" "-12"
        (natB 1).ToString()     |> check "ToString" "1"
        (natB 0).ToString()     |> check "ToString" "0"
        (natB (-12)).ToString() |> check "ToString" "-12"
        (1I /% 3I).ToString()   |> check "ToString" "1/3"
        (12I /% 5I).ToString()  |> check "ToString" "12/5"
        //(13I /% 0I).ToString()  |> check "ToString" "1/0"     // + 1/0. Plan to make this invalid value
        //(-13I /% 0I).ToString() |> check "ToString" "1/0"     // - 1/0. Plan to make this invalid value
        //(0I /% 0I).ToString()   |> check "ToString" "0/0"     //   0/0. Plan to make this invalid value

        // GetHashCode
        // Cases: zero, integer, computed integer, computed by multiple routes.
        let checkSameHashGeneric a b                      = check (sprintf "GenericHash     %A %A" a b) (a.GetHashCode()) (b.GetHashCode())
        let checkSameHash (a:BigRational) (b:BigRational) = check (sprintf "BigRationalHash %A %A" a b)  (a.GetHashCode()) (b.GetHashCode()); checkSameHashGeneric a b

        List.iter (fun n -> checkSameHash (natA n) (natB n)) [-10 .. 10]
        List.iter (fun n -> checkSameHash n ((n * q3 + n * q2) / q5)) miscQs

        // bug 3488: should non-finite values be supported?
        //let x = BigRational.FromBigInt (-1I) / BigRational.FromBigInt 0I
        //let q2,q3,q5 = BigRational.FromInt 2,BigRational.FromInt 3,BigRational.FromInt 5
        //let x2 = (x * q2 + x * q3) / q5
        //x,x2,x = x2

        // Test: Zero,One?
        check "ZeroA" BigRational.Zero (natA 0)
        check "ZeroA" BigRational.Zero (natA 0)
        check "OneA"  BigRational.One  (natB 1)
        check "OneB"  BigRational.One  (natB 1)

    [<Test>]
    let BinaryAndUnaryOperators () : unit =
        // Test: generic bop
        let testR2One name f check ((p,q),(pp,qq)) =     
            // There may be several ways to construct the test rationals
            let zs        = makeQs p  q
            let zzs       = makeQs pp qq
            let resultRef = check (p,q) (pp,qq) // : bool    
            let args      = [for z in zs do for zz in zzs do yield (z,zz)]    
            match List.tryFind (fun (z,zz) -> resultRef <> f (z,zz)) args with
            | None        -> () // ok
            | Some (z,zz) -> printf "Test failed. %s (%A,%A) (%A,%A) = %s %A %A. Expected %A.\n" name p q pp qq name z zz resultRef
                             reportFailure "cknwe9"

        // Test: generic uop
        let testR1One name f check (p,q) =     
            // There may be several ways to construct the test rationals
            let zs        = makeQs p  q    
            let resultRef = check (p,q) //: bool        
            match List.tryFind (fun z -> resultRef <> f z) zs with
            | None   -> () // ok
            | Some z -> printf "Test failed. %s (%A,%A) = %s %A. Expected %A.\n" name p q name z resultRef
                        reportFailure "vekjkrejvre0"
                             
        let testR2All name f check vectors = List.iter (testR2One name f check) vectors
        let testR1All name f check vectors = List.iter (testR1One name f check) vectors

        // Test: relations
        let sign (i:BigInteger) = BigInteger(i.Sign)
        testR2All "="  BigRational.(=)           (fun (p,q) (pp,qq) -> (p*qq =  q*pp)) vector2s
        testR2All "="  BigRational.op_Equality   (fun (p,q) (pp,qq) -> (p*qq =  q*pp)) vector2s
        testR2All "!=" BigRational.op_Inequality (fun (p,q) (pp,qq) -> (p*qq <> q*pp)) vector2s
        //     p/q < pp/qq
        // iff (p * sign q) / (q  * sign q)  < (pp * sign qq) / (qq * sign qq)
        // iff (p * sign q) * (qq * sign qq) < (pp * sign qq) * (q  * sign q)           since q*sign q is always +ve.
        testR2All "<"  BigRational.(<)  (fun (p,q) (pp,qq) -> (p * sign q) * (qq * sign qq) < (pp * sign qq) * (q * sign q)) vector2s
        testR2All ">"  BigRational.(>)  (fun (p,q) (pp,qq) -> (p * sign q) * (qq * sign qq) > (pp * sign qq) * (q * sign q)) vector2s
        testR2All "<=" BigRational.(<=) (fun (p,q) (pp,qq) -> (p * sign q) * (qq * sign qq) <= (pp * sign qq) * (q * sign q)) vector2s
        testR2All ">=" BigRational.(>=) (fun (p,q) (pp,qq) -> (p * sign q) * (qq * sign qq) >= (pp * sign qq) * (q * sign q)) vector2s

        // System.IComparable tests
        let BigRationalCompareTo (p:BigRational,q:BigRational) = (p :> System.IComparable).CompareTo(q)
        testR2All "IComparable.CompareTo" BigRationalCompareTo (fun (p,q) (pp,qq) -> compare ((p * sign q) * (qq * sign qq)) ((pp * sign qq) * (q * sign q))) vector2s

        // Test: is negative, is positive
        testR1All "IsNegative" (fun (x:BigRational) -> x.IsNegative) (fun (p,q) -> sign p * sign q = -1I) vector1s
        testR1All "IsPositive" (fun (x:BigRational) -> x.IsPositive) (fun (p,q) -> sign p * sign q =  1I) vector1s
        testR1All "IsZero"     (fun (x:BigRational) -> x = q0)       (fun (p,q) -> sign p = 0I)           vector1s


        let test1One name f check (p,q) =     
            // There may be several ways to construct the test rationals
            let zs        = makeQs p  q    
            let results   = [for z in zs -> f z]
            let refP,refQ = check (p,q)   
            let refResult = BigRational.FromBigInt refP / BigRational.FromBigInt refQ
            let resOK (result:BigRational) = 
               result.Numerator * refQ = refP * result.Denominator && 
               BigRational.Equals(refResult,result)
            match List.tryFind (fun result -> not (resOK result)) results with
            | None        -> () // ok
            | Some result -> printf "Test failed. %s (%A,%A). Expected %A. Observed %A\n" name p q refResult result
                             reportFailure "klcwe09wek"

        let test1All name f check vectors = List.iter (test1One name f check) vectors
  
        test1All "neg" (BigRational.(~-)) (fun (p,q)         -> (-p,q))             vector1s
        test1All "pos" (BigRational.(~+)) (fun (p,q)         -> (p,q))              vector1s // why have ~+ ???

        // Test: Abs,Sign
        test1All  "Abs"         (BigRational.Abs)     (fun (p,q) -> (abs p,abs q)) vector1s
        testR1All "Sign"        (fun (x:BigRational) -> x.Sign)    (fun (p,q) -> check "NonZeroDenom" (sign q <> 0I) true; (sign p * sign q) |> int32) vector1s

        // Test: PowN
        test1All  "PowN(x,2)"   (fun x -> BigRational.PowN(x,2))   (fun (p,q) -> (p*p,q*q)) vector1s
        test1All  "PowN(x,1)"   (fun x -> BigRational.PowN(x,1))   (fun (p,q) -> (p,q)) vector1s
        test1All  "PowN(x,0)"   (fun x -> BigRational.PowN(x,0))   (fun (p,q) -> (1I,1I)) vector1s

        // MatteoT: moved to numbersVS2008\test.ml
        //test1All  "PowN(x,200)" (fun x -> BigRational.PowN(x,200)) (fun (p,q) -> (BigInteger.Pow(p,200I),BigInteger.Pow(q,200I))) vector1s

        // MatteoT: moved to numbersVS2008\test.ml
        //let powers = [0I .. 100I]
        //powers |> List.iter (fun i -> test1All  "PowN(x,i)" (fun x -> BigRational.PowN(x,int i)) (fun (p,q) -> (BigInteger.Pow(p,i),BigInteger.Pow(q,i))) vector1s)

        // Test: PowN with negative powers - expect exception
        //testR1All  "PowN(x,-1)"  (fun x -> throws (fun () -> BigRational.PowN(x,-1))) (fun (p,q) -> true) vector1s
        //testR1All  "PowN(x,-4)"  (fun x -> throws (fun () -> BigRational.PowN(x,-4))) (fun (p,q) -> true) vector1s



[<TestFixture>]
module BigNumType =
    let g_positive1 = 1000000000000000000000000000000000018N
    let g_positive2 = 1000000000000000000000000000000000000N
    let g_negative1 = -1000000000000000000000000000000000018N
    let g_negative2 = -1000000000000000000000000000000000000N
    let g_negative3 = -1000000000000000000000000000000000036N
    let g_zero      = 0N
    let g_normal    = 88N
    let g_bigintpositive    = 1000000000000000000000000000000000018I
    let g_bigintnegative    = -1000000000000000000000000000000000018I
    
    // Interfaces
    [<Test>]
    let IComparable () : unit =      
        // Legit IC
        let ic = g_positive1 :> IComparable    
        Assert.AreEqual(ic.CompareTo(g_positive1),0) 
        checkThrowsArgumentException( fun () -> ic.CompareTo(g_bigintpositive) |> ignore)
    
    // Base class methods
    [<Test>]
    let ObjectToString() =
    
        // Currently the CLR 4.0 and CLR 2.0 behavior of BigInt.ToString is different, causing this test to fail.
        
        Assert.AreEqual(g_positive1.ToString(),
                        "1000000000000000000000000000000000018")
        Assert.AreEqual(g_zero.ToString(),"0") 
        Assert.AreEqual(g_normal.ToString(),"88")
        
        
    [<Test; Ignore("Bug 5286 - Differences between CLR 2.0 BigInt and CLR 4.0 BigInt")>]
    let System_Object_GetHashCode() =
        Assert.AreEqual(g_negative1.GetHashCode(),1210897093)
        Assert.AreEqual(g_normal.GetHashCode(),89)
        Assert.AreEqual(g_zero.GetHashCode(),1)
        ()
    
    // Static methods    
    [<Test>]
    let Abs() =
        Assert.AreEqual(bignum.Abs(g_negative1), g_positive1)
        Assert.AreEqual(bignum.Abs(g_negative2), g_positive2)
        Assert.AreEqual(bignum.Abs(g_positive1), g_positive1)
        Assert.AreEqual(bignum.Abs(g_normal),    g_normal)
        Assert.AreEqual(bignum.Abs(g_zero),      g_zero)
        ()
        
    [<Test>]
    let FromBigInt() =
        Assert.AreEqual(bignum.FromBigInt(g_bigintpositive),
                        g_positive1)
        Assert.AreEqual(bignum.FromBigInt(g_bigintnegative),
                        g_negative1)
        Assert.AreEqual(bignum.FromBigInt(0I),g_zero)
        Assert.AreEqual(bignum.FromBigInt(88I),g_normal)
        ()
    
    [<Test>]
    let FromInt() =
        Assert.AreEqual(bignum.FromInt(2147483647),   2147483647N)
        Assert.AreEqual(bignum.FromInt(-2147483648), -2147483648N)
        Assert.AreEqual(bignum.FromInt(0),   0N)
        Assert.AreEqual(bignum.FromInt(88), 88N)
        ()
        
    [<Test>]
    let One() =
        Assert.AreEqual(bignum.One,1N)
        ()
    
    [<Test>]
    let Parse() =
        Assert.AreEqual(bignum.Parse("100"),   100N)
        Assert.AreEqual(bignum.Parse("-100"), -100N)
        Assert.AreEqual(bignum.Parse("0"),     g_zero)
        Assert.AreEqual(bignum.Parse("88"),    g_normal)
        ()
        
    [<Test>]
    let PowN() =
        Assert.AreEqual(bignum.PowN(100N,2), 10000N)
        Assert.AreEqual(bignum.PowN(-3N,3),  -27N)
        Assert.AreEqual(bignum.PowN(g_zero,2147483647), 0N)
        Assert.AreEqual(bignum.PowN(g_normal,0),        1N)
        ()
        
        
    [<Test>]
    let Sign() =
        Assert.AreEqual(g_positive1.Sign,  1)
        Assert.AreEqual(g_negative1.Sign, -1)
        Assert.AreEqual(g_zero.Sign,   0)
        Assert.AreEqual(g_normal.Sign, 1)
        ()
        
    
        
    [<Test>]
    let ToBigInt() =
        Assert.AreEqual(bignum.ToBigInt(g_positive1), g_bigintpositive)
        Assert.AreEqual(bignum.ToBigInt(g_negative1), g_bigintnegative)
        Assert.AreEqual(bignum.ToBigInt(g_zero),   0I)
        Assert.AreEqual(bignum.ToBigInt(g_normal), 88I)
        ()
        
    
        
    [<Test>]
    let ToDouble() =
        Assert.AreEqual(bignum.ToDouble(179769N*1000000000000000N),   1.79769E+20)
        Assert.AreEqual(bignum.ToDouble(-179769N*1000000000000000N), -1.79769E+20)
        Assert.AreEqual(bignum.ToDouble(0N),0.0)
        Assert.AreEqual(bignum.ToDouble(88N),88.0)
        Assert.AreEqual(double(179769N*1000000000000000N),   1.79769E+20)
        Assert.AreEqual(double(-179769N*1000000000000000N), -1.79769E+20)
        Assert.AreEqual(double(0N),0.0)
        Assert.AreEqual(double(88N),88.0)
        ()
        
        
    [<Test>]
    let ToInt32() =
        Assert.AreEqual(bignum.ToInt32(2147483647N),   2147483647)
        Assert.AreEqual(bignum.ToInt32(-2147483648N), -2147483648)
        Assert.AreEqual(bignum.ToInt32(0N),  0)
        Assert.AreEqual(bignum.ToInt32(88N), 88)
        Assert.AreEqual(int32(2147483647N),   2147483647)
        Assert.AreEqual(int32(-2147483648N), -2147483648)
        Assert.AreEqual(int32(0N),  0)
        Assert.AreEqual(int32(88N), 88)
        
    
        
    [<Test>]
    let Zero() =
        Assert.AreEqual(bignum.Zero,0N)
        ()
       
    // operator methods  
    [<Test>]
    let test_op_Addition() =
        Assert.AreEqual(100N + 200N, 300N)
        Assert.AreEqual((-100N) + (-200N), -300N)
        Assert.AreEqual(g_positive1 + g_negative1, 0N)
        Assert.AreEqual(g_zero + g_zero,0N)
        Assert.AreEqual(g_normal + g_normal, 176N)
        Assert.AreEqual(g_normal + g_normal, 176N)
        ()
        
        
        
    [<Test>]
    let test_op_Division() =
        Assert.AreEqual(g_positive1 / g_positive1, 1N)
        Assert.AreEqual(-100N / 2N, -50N)
        Assert.AreEqual(g_zero / g_positive1, 0N)
        ()
        
    [<Test>]
    let test_op_Equality() =
        
        Assert.IsTrue((g_positive1 = g_positive1))
        Assert.IsTrue((g_negative1 = g_negative1))
        Assert.IsTrue((g_zero = g_zero))
        Assert.IsTrue((g_normal = g_normal))
        ()
        
    [<Test>]
    let test_op_GreaterThan () : unit =
        Assert.AreEqual((g_positive1 > g_positive2), true)
        Assert.AreEqual((g_negative1 > g_negative2), false)
        Assert.AreEqual((g_zero > g_zero), false)
        Assert.AreEqual((g_normal > g_normal), false)
        
        
        ()
    [<Test>]
    let test_op_GreaterThanOrEqual () : unit =
        Assert.AreEqual((g_positive1 >= g_positive2), true)
        Assert.AreEqual((g_positive2 >= g_positive1), false)                                             
        Assert.AreEqual((g_negative1 >= g_negative1), true)
        Assert.AreEqual((0N >= g_zero), true)
        
        ()
    [<Test>]  
    let test_op_LessThan () : unit =
        Assert.AreEqual((g_positive1 < g_positive2), false)
        Assert.AreEqual((g_negative1 < g_negative3), false)
        Assert.AreEqual((0N < g_zero), false)
        
        ()
    [<Test>]
    let test_op_LessThanOrEqual () : unit =
        Assert.AreEqual((g_positive1 <= g_positive2), false)
        Assert.AreEqual((g_positive2 <= g_positive1), true)                                             
        Assert.AreEqual((g_negative1 <= g_negative1), true)
        Assert.AreEqual((0N <= g_zero), true)
       
        ()
    
    [<Test>]
    let test_op_Multiply () : unit =
        Assert.AreEqual(3N * 5N, 15N)
        Assert.AreEqual((-3N) * (-5N), 15N)
        Assert.AreEqual((-3N) * 5N, -15N)
        Assert.AreEqual(0N * 5N, 0N)
        
        ()
        
    [<Test>]
    let test_op_Range () : unit =
        let resultPos = [0N .. 2N]
        let seqPos    = [0N; 1N; 2N]                                                                
        verifySeqsEqual resultPos seqPos
        
        let resultNeg = [-2N .. 0N]                                       
        let seqNeg =  [-2N; -1N; 0N]  
        verifySeqsEqual resultNeg seqNeg
        
        let resultSmall = [0N ..5N]
        let seqSmall = [0N; 1N; 2N; 3N; 4N; 5N]        
        verifySeqsEqual resultSmall seqSmall
           
        ()
        
        
    [<Test>]
    let test_op_RangeStep () : unit =
        let resultPos = [0N .. 3N .. 6N]
        let seqPos    = [0N; 3N; 6N]                                                                
        verifySeqsEqual resultPos seqPos
        
        let resultNeg = [-6N .. 3N .. 0N]                                        
        let seqNeg =  [-6N; -3N; 0N]  
        verifySeqsEqual resultNeg seqNeg
        
        let resultSmall = [0N .. 3N .. 9N]
        let seqSmall = [0N; 3N; 6N; 9N]        
        verifySeqsEqual resultSmall seqSmall
                   
        ()
        
    [<Test>]
    let test_op_Subtraction () : unit =
        Assert.AreEqual(g_positive1 - g_positive2,18N)
        Assert.AreEqual(g_negative1 - g_negative3,18N)
        Assert.AreEqual(0N-g_positive1, g_negative1)
        ()
        
    [<Test>]
    let test_op_UnaryNegation () : unit =
        Assert.AreEqual(-g_positive1, g_negative1)
        Assert.AreEqual(-g_negative1, g_positive1)
        Assert.AreEqual(-0N,0N) 
        
        ()
        
    [<Test>]
    let test_op_UnaryPlus () : unit =
        Assert.AreEqual(+g_positive1,g_positive1)
        Assert.AreEqual(+g_negative1,g_negative1)
        Assert.AreEqual(+0N, 0N)
        
        ()
    
    // instance methods
    [<Test>]
    let Denominator () : unit =
        Assert.AreEqual(g_positive1.Denominator, 1I)
        Assert.AreEqual(g_negative1.Denominator, 1I)
        Assert.AreEqual(0N.Denominator, 1I)
        
        ()       
    
    [<Test>]
    let IsNegative () : unit =
        Assert.IsFalse(g_positive1.IsNegative)
        Assert.IsTrue(g_negative1.IsNegative)

        Assert.IsFalse( 0N.IsNegative)
        Assert.IsFalse(-0N.IsNegative)
        
        () 
        
        
    [<Test>]
    let IsPositive () : unit =

        Assert.IsTrue(g_positive1.IsPositive)
        Assert.IsFalse(g_negative1.IsPositive)

        Assert.IsFalse( 0N.IsPositive)
        Assert.IsFalse(-0N.IsPositive)
        
        ()     
        
    [<Test>]
    let Numerator () : unit =
        Assert.AreEqual(g_positive1.Numerator, g_bigintpositive)
        Assert.AreEqual(g_negative1.Numerator, g_bigintnegative)
        Assert.AreEqual(0N.Numerator, 0I)
        
        ()  

