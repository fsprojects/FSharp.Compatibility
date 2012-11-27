(*  OCaml Compatibility Library for F#
    (FSharp.Compatibility.OCaml)

    Copyright (c) Microsoft Corporation 2005-2009
    Copyright (c) Jack Pappas 2012
        http://github.com/jack-pappas

    This code is distributed under the terms of the Apache 2.0 license.
    See the LICENSE file for details. *)

/// Facilities for printing exceptions.
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharp.Compatibility.OCaml.Printexc

open System


/// Printexc.to_string e returns a string representation of the exception e.
let to_string (e : exn) =
    match e with
    | Failure s -> s
    | :? System.ArgumentException as e ->
        sprintf "invalid argument: %s" e.Message
    | MatchFailureException (s, n, m) ->
        sprintf "match failure, file '%s', line %d, column %d" s n m
    | _ ->
        sprintf "%A\n" e

/// Printexc.print fn x applies fn to x and returns the result.
/// If the evaluation of fn x raises any exception, the name of the exception is printed
/// on standard error output, and the exception is raised again. The typical use is to
/// catch and report exceptions that escape a function application.
let print fn x =
    try fn x
    with e ->
        stderr.WriteLine (to_string e)
        reraise ()  // raise e

/// Printexc.catch fn x is similar to Printexc.print, but aborts the program with exit code 2 after printing the uncaught exception.
[<Obsolete()>]
let catch fn x =
    try fn x
    with e ->
        stderr.WriteLine (to_string e)
        exit 2

