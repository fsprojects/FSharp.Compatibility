(*  OCaml Compatibility Library for F#
    (FSharpx.Compatibility.OCaml)

    Copyright (c) Microsoft Corporation 2005-2009
    Copyright (c) Jack Pappas 2012
        http://github.com/jack-pappas

    This code is distributed under the terms of the Apache 2.0 license.
    See the LICENSE file for details. *)

// References:
// http://caml.inria.fr/pub/docs/manual-ocaml/libref/Char.html

/// Character operations.
module FSharpx.Compatibility.OCaml.Char

open System

/// Return the ASCII code of the argument.
let code (c : char) : int =
    // TODO : What do we do if 'c' is a non-ASCII (i.e., Unicode) char?
    raise <| System.NotImplementedException "Char.code"

/// Return the character with the given ASCII code.
let chr (value : int) : char =
    if value < int Byte.MinValue ||
        value > int Byte.MaxValue then
        raise <| Invalid_argument "Char.chr"

    char value

/// Return a string representing the given character, with special
/// characters escaped following the lexical conventions of OCaml.
let escaped = function
    (* Special characters are converted to a string and prefixed with a backslash. *)
    | '\u0000'
    | '\a'
    | '\b'
    | '\t'
    | '\n'
    | '\v'
    | '\f'
    | '\r'
    | '\u001a'
    | '\u0022'
    | '\u0027'
    | '\u005c'
    | '\u0060'
        as c ->
        String [| '\\'; c; |] : string

    (* Non-special characters are converted directly to a string. *)
    | c ->
        string c

/// Convert the given character to its equivalent lowercase character.
let inline lowercase (c : char) : char =
    Char.ToLowerInvariant c

/// Convert the given character to its equivalent uppercase character.
let inline uppercase (c : char) : char =
    Char.ToUpperInvariant c

/// An alias for the type of characters.
type t = char

/// The comparison function for characters, with the same specification as <see cref="compare"/>.
let inline compare (a : char) (b : char) =
    compare a b

