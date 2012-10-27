(*  OCaml Compatibility Library for F#
    (FSharpx.Compatibility.OCaml)

    Copyright (c) 2012 Jack Pappas (github.com/jack-pappas)

    This code is available under the Apache 2.0 license.
    See the LICENSE file for the complete text of the license. *)

// References:
// http://caml.inria.fr/pub/docs/manual-ocaml/libref/String.html

/// String operations.
module FSharpx.Compatibility.OCaml.String

open System
open System.Text.RegularExpressions

/// Return a copy of the argument, with special characters represented by
/// escape sequences, following the lexical conventions of OCaml.
/// If there is no special character in the argument, return the original
/// string itself, not a copy.
let inline escaped (str : string) : string =
    // Insert a backslash before any special characters, using a regular expression.
    Regex.Replace (str, @"[\x00\a\b\t\n\v\f\r\x1a\x22\x27\x5c\x60]", "\\$0")


