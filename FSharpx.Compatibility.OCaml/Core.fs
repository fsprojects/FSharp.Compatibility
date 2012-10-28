(*  OCaml Compatibility Library for F#
    (FSharpx.Compatibility.OCaml)

    Copyright (c) Microsoft Corporation 2005-2009
    Copyright (c) Jack Pappas 2012
        http://github.com/jack-pappas

    This code is distributed under the terms of the Apache 2.0 license.
    See the LICENSE file for details. *)

// References:
// http://caml.inria.fr/pub/docs/manual-ocaml/manual034.html

/// Built-in types and predefined exceptions.
[<AutoOpen>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharpx.Compatibility.OCaml.Core

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
//exception Match_failure of string * int * int
type Match_failure = MatchFailureException

/// Exception raised when an assertion fails.
/// The arguments are the location of the assert keyword in the source code (file name, line number, column number). 
exception Assert_failure of string * int * int

/// Exception raised by library functions to signal that the given arguments do not make sense.
exception Invalid_argument of string

/// Exception raised by library functions to signal that they are undefined on the given arguments.
// TODO : Should this just be an alias for System.Exception (exn) so it can be matched
// by the built-in F# Failure pattern?
exception Failure of string

/// Exception raised by search functions when the desired object could not be found.
exception Not_found

/// Exception raised by the garbage collector when there is insufficient memory to complete the computation.
// TODO : This exception can only be raised by the garbage collector,
// so any other code will only be consuming it.
// Therefore, this should probably be changed to an active pattern instead.
//exception Out_of_memory
type Out_of_memory = OutOfMemoryException

/// Exception raised by the bytecode interpreter when the evaluation stack reaches its maximal size.
/// This often indicates infinite or excessively deep recursion in the user’s program.
// TODO : This exception shouldn't ever be raised by user code, so it should
// probably be changed to an active pattern instead.
//exception Stack_overflow
type Stack_overflow = StackOverflowException

/// Exception raised by the input/output functions to report an operating system error.
exception Sys_error of string

/// Exception raised by input functions to signal that the end of file has been reached.
exception End_of_file

/// Exception raised by integer division and remainder operations when their second argument is zero.
//exception Division_by_zero
type Division_by_zero = DivideByZeroException

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


