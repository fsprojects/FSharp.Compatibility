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

