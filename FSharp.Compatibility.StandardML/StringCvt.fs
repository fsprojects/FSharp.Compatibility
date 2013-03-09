(*

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

// Reference:
// http://www.standardml.org/Basis/string-cvt.html

/// The StringCvt structure provides types and functions for handling the
/// conversion between strings and values of various basic types.
module FSharp.Compatibility.StandardML.StringCvt

/// The values of type radix are used to specify the radix of a representation
/// of an integer, corresponding to the bases 2, 8, 10, and 16, respectively.
type radix =
    /// Base 2.
    | BIN
    /// Base 8.
    | OCT
    /// Base 10.
    | DEC
    /// Base 16.
    | HEX

/// Values of type realfmt are used to specify the format of a string
/// representation for a real or floating-point number.
type realfmt =
    //
    | SCI of int option
    //
    | FIX of int option
    //
    | GEN of int option
    //
    | EXACT

//
type ('a, 'b) reader = 'b -> ('a * 'b) option

//
let padLeft (c : char) (i : int) (s : string) : string =
    raise <| System.NotImplementedException "StringCvt.padLeft"

//
let padRight (c : char) (i : int) (s : string) : string =
    raise <| System.NotImplementedException "StringCvt.padRight"

//
let splitl (f : char -> bool) (rdr : (char, 'a) reader) (src : 'a) : string * 'a =
    raise <| System.NotImplementedException "StringCvt.splitl"

//
let takel (f : char -> bool) (rdr : (char, 'a) reader) (src : 'a) : string =
    fst (splitl f rdr src)

//
let dropl (f : char -> bool) (rdr : (char, 'a) reader) (src : 'a) : 'a =
    snd (splitl f rdr src)

//
let skipWS (rdr : (char, 'a) reader) (src : 'a) : 'a =
    raise <| System.NotImplementedException "StringCvt.skipWS"


(*

type cs
val scanString : ((char, cs) reader -> ('a, cs) reader)
                   -> string -> 'a option

*)
