(*

Copyright 2005-2009 Microsoft Corporation
Copyright 2012 Jack Pappas

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

// References:
// http://caml.inria.fr/pub/docs/manual-ocaml/manual034.html

/// Built-in types and predefined exceptions.
[<AutoOpen>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharp.Compatibility.OCaml.Core

open System

(*** Built-in types ***)

/// <summary>The type of format strings.</summary>
/// <typeparam name="a">the type of the parameters of the format.</typeparam>
/// <typeparam name="b">the type of the first argument given to %a and %t printing functions (see module Printf).</typeparam>
/// <typeparam name="c">the result type of these functions in 'b, and also the type of the argument transmitted to the first argument of kprintf-style functions.</typeparam>
/// <typeparam name="d">the result type for the scanf-style functions (see module Scanf).</typeparam>
/// <typeparam name="e">the type of the receiver function for the scanf-style functions.</typeparam>
/// <typeparam name="f">the result type for the printf-style functions.</typeparam>
type format6<'a, 'b, 'c, 'd, 'e, 'f> (fmt : string) =
    //
    member __.RawValue
        with get () = fmt    

/// This type is used to implement the Lazy module.
/// It should not be used directly.
type lazy_t<'a> = Lazy<'a>


(*** Predefined exceptions ***)

//
[<Obsolete("Code which raises Match_failure should be changed to raise MatchFailureException instead.")>]
exception Match_failure of string * int * int

/// Exception raised when an assertion fails.
/// The arguments are the location of the assert keyword in the source code (file name, line number, column number). 
exception Assert_failure of string * int * int

//
exception Undefined

/// Exception raised by library functions to signal that the given arguments do not make sense.
[<Obsolete("Code which raises Invalid_argument should be changed to raise System.ArgumentException instead.")>]
exception Invalid_argument of string

/// Exception raised by library functions to signal that they are undefined on the given arguments.
// TODO : Should this just be an alias for System.Exception (exn) so it can be matched
// by the built-in F# Failure pattern?
exception Failure of string

[<CompiledName("FailurePattern")>]
let (|Failure|_|) (ex : exn) =
    // This allows us to match both the Failure exception defined above and System.Exception
    // (raised by functions such as 'failwith') so the exception is matched as expected no matter
    // how it was raised.
    match ex with
    | :? Failure as failure ->
        Some failure.Data0
    | _ ->
        Microsoft.FSharp.Core.Operators.(|Failure|_|) ex

/// Exception raised by search functions when the desired object could not be found.
exception Not_found

/// Exception raised by the garbage collector when there is insufficient memory to complete the computation.
// TODO : This exception can only be raised by the garbage collector,
// so any other code will only be consuming it.
// Therefore, this should probably be changed to an active pattern instead.
[<Obsolete("Code which raises Out_of_memory should be changed to raise System.OutOfMemoryException instead.")>]
exception Out_of_memory

/// Exception raised by the bytecode interpreter when the evaluation stack reaches its maximal size.
/// This often indicates infinite or excessively deep recursion in the user’s program.
// TODO : This exception shouldn't ever be raised by user code, so it should
// probably be changed to an active pattern instead.
[<Obsolete("Code which raises Stack_overflow should be changed to raise System.StackOverflowException instead.")>]
exception Stack_overflow

/// Exception raised by the input/output functions to report an operating system error.
exception Sys_error of string

/// Exception raised by input functions to signal that the end of file has been reached.
[<Obsolete("Code which raises Stack_overflow should be changed to raise System.IO.EndOfStreamException instead.")>]
exception End_of_file

/// Exception raised by integer division and remainder operations when their second argument is zero.
[<Obsolete("Code which raises Division_by_zero should be changed to raise System.DivideByZeroException instead.")>]
exception Division_by_zero

/// A special case of <see cref="Sys_error"/> raised when no I/O is possible on a non-blocking I/O channel. 
exception Sys_blocked_io

/// Exception raised when an ill-founded recursive module definition is evaluated.
/// The arguments are the location of the definition in the source code (file name, line number, column number).
exception Undefined_recursive_module of string * int * int

//
module ExnPatterns =
    //
    let (|Match_failure|_|) (e : exn) =
        match e with
        | :? MatchFailureException as mfe ->
            Some (mfe.Data0, mfe.Data1, mfe.Data2)
        | _ ->
            None



let not_found() = raise Not_found

let int_neg (x:int) = -x
let inline (.()) (arr: _[]) n = arr.[n]
let inline (.()<-) (arr: _[]) n x = arr.[n] <- x

(*
(*  mod_float x y = x - y * q where q = truncate(a/b) and truncate x removes fractional part of x *)
let truncate (x:float) : int = int32 x

#if FX_NO_TRUNCATE
let truncatef (x:float) = 
    if x >= 0.0 then 
        System.Math.Floor x
    else
        System.Math.Ceiling x
#else
let truncatef (x:float) = System.Math.Truncate x
#endif
*)

let string_of_int (x:int) = x.ToString()
let int_of_string (s:string) = try int32 s with _ -> failwith "int_of_string"
let string_to_int   x = int_of_string x

(*
module Pervasives = 
    let hash  (x: 'a) = LanguagePrimitives.GenericHash x
    let invalid_arg s = raise (System.ArgumentException(s))
*)

