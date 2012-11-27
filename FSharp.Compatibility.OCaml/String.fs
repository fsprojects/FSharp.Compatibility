(*  OCaml Compatibility Library for F#
    (FSharp.Compatibility.OCaml)

    Copyright (c) Microsoft Corporation 2005-2009
    Copyright (c) Jack Pappas 2012
        http://github.com/jack-pappas

    This code is distributed under the terms of the Apache 2.0 license.
    See the LICENSE file for details. *)

// References:
// http://caml.inria.fr/pub/docs/manual-ocaml/libref/String.html

/// String operations.
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharp.Compatibility.OCaml.String

open System
open System.Text.RegularExpressions

//
let private test_null arg =
    match arg with
    | null ->
        raise <| System.ArgumentNullException "arg"
    | _ -> ()

let private indexNotFound () =
    raise <| System.Collections.Generic.KeyNotFoundException "An index for the character was not found in the string"

//
let private fast_get (s:string) n =
    test_null s
    s.[n]

/// Return a copy of the argument, with special characters represented by
/// escape sequences, following the lexical conventions of OCaml.
/// If there is no special character in the argument, return the original
/// string itself, not a copy.
let inline escaped (str : string) : string =
    // Insert a backslash before any special characters, using a regular expression.
    Regex.Replace (str, @"[\x00\a\b\t\n\v\f\r\x1a\x22\x27\x5c\x60]", "\\$0")

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'str.[i]' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let get (str : string) i =
    test_null str
    try str.[i]
    with :? System.ArgumentException -> invalidArg "i" "index out of bounds" 

//
let length (str : string) =
    test_null str
    str.Length

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'str.[i..j]' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let sub (s : string) (start : int) (len : int) =
    test_null s
    try s.Substring (start, len)
    with :? System.ArgumentException ->
        failwith "String.sub" 

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'Operators.compare' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let compare (x : string) (y : string) =
    compare x y

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using the overloaded 'string' operator instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let of_char (c : char) =
    System.Char.ToString c

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'String.replicate' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let make (n : int) (c : char) : string =
    System.String (c, n)

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'str.IndexOf' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let index_from (s : string) (start : int) (c : char) =  
    test_null s
    try let r = s.IndexOf (c, start)
        if r = -1 then indexNotFound() else r
    with :? ArgumentException ->
        invalidArg "start" "String.index_from" 

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'str.LastIndexOf' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let rindex_from (s : string) (start : int) (c : char) =
    test_null s
    try let r =  s.LastIndexOf (c, start)
        if r = -1 then indexNotFound() else r
    with :? ArgumentException ->
        invalidArg "start" "String.rindex_from" 

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'str.IndexOf' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let index (s : string) (c : char) =
    test_null s
    index_from s 0 c

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'str.LastIndexOf' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let rindex (s : string) (c : char) =
    test_null s
    rindex_from s (length s - 1) c

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'str.IndexOf' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let contains_between (s : string) (start : int) (stop : int) (c : char) =
    test_null s
    try s.IndexOf (c, start, stop - start + 1) <> -1
    with :? ArgumentException ->
        invalidArg "start" "String.contains_between" 

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'str.IndexOf' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let contains_from (s : string) (start : int) (c : char) =
    test_null s
    let stop = length s - 1
    try s.IndexOf (c, start, stop - start + 1) <> -1
    with :? ArgumentException ->
        invalidArg "start" "String.contains_from" 

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'str.LastIndexOf' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let rcontains_from (s : string) (stop : int) (c : char) =
    test_null s
    let start = 0
    try s.IndexOf (c, start, stop - start + 1) <> -1
    with :? ArgumentException ->
        invalidArg "stop" "String.rcontains_from" 

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'str.Contains' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let contains (s : string) (c : char) =
    contains_from s 0 c

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'str.ToUpper' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let uppercase (s : string) =
    test_null s
#if FX_NO_TO_LOWER_INVARIANT
    s.ToUpper System.Globalization.CultureInfo.InvariantCulture
#else
    s.ToUpperInvariant ()
#endif

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'str.ToLower()' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let lowercase (s : string) =
    test_null s
#if FX_NO_TO_LOWER_INVARIANT
    s.ToLower System.Globalization.CultureInfo.InvariantCulture
#else
    s.ToLowerInvariant ()
#endif

//
let capitalize (s : string) =
    test_null s
    if s.Length = 0 then ""
    else (uppercase s.[0..0]) + s.[1 .. s.Length - 1]

//
let uncapitalize (s : string) =
    test_null s
    if s.Length = 0 then ""
    else (lowercase s.[0..0]) + s.[1 .. s.Length - 1]

#if FX_NO_STRING_SPLIT_OPTIONS
#else
//
let split (c : char list) =
    let ca = Array.ofList c
    fun (s : string) ->
        test_null s
        s.Split (ca, StringSplitOptions.RemoveEmptyEntries)
        |> Array.toList
#endif

//
let trim (c : char list) =
    let ca = Array.ofList c
    fun (s : string) ->
        s.Trim ca

