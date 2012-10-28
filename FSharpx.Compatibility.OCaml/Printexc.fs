(*  OCaml Compatibility Library for F#
    (FSharpx.Compatibility.OCaml)

    Copyright (c) Microsoft Corporation 2005-2009
    Copyright (c) Jack Pappas 2012
        http://github.com/jack-pappas

    This code is distributed under the terms of the Apache 2.0 license.
    See the LICENSE file for details. *)

//
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharpx.Compatibility.OCaml.Printexc

let to_string (e:exn) = 
  match e with 
  | Failure s -> s
  | :? System.ArgumentException as e -> sprintf "invalid argument: %s" e.Message
  | MatchFailureException(s,n,m) -> sprintf "match failure, file '%s', line %d, column %d" s n m
  | _ -> sprintf "%A\n" e

let print f x = try f x with e -> stderr.WriteLine (to_string e) ; raise e 

