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
// http://www.standardml.org/Basis/char.html

//
module FSharp.Compatibility.StandardML.Char

(*

eqtype char
eqtype string

val minChar : char
val maxChar : char
val maxOrd : int

val ord : char -> int
val chr : int -> char
val succ : char -> char
val pred : char -> char

val compare : char * char -> order
val <  : char * char -> bool
val <= : char * char -> bool
val >  : char * char -> bool
val >= : char * char -> bool

*)

/// returns the (non-negative) integer code of the character c.
let inline ord (c : char) =
    int c

/// returns the character whose code is i; raises Chr if i < 0 or i > maxOrd.
let chr (i : int) =
    raise <| System.NotImplementedException "Char.chr"

/// returns true if character c occurs in the string s; otherwise it returns false.
let contains (s : string) (c : char) =
    s.IndexOf c <> -1

//
let notContains (s : string) (c : char) =
    s.IndexOf c = -1

/// returns true if c is a (seven-bit) ASCII character, i.e., 0 <= ord c <= 127.
/// Note that this function is independent of locale.
let isAscii (c : char) =
    int c <= 127

/// These return the lowercase (respectively, uppercase) letter corresponding to c if c is a letter; otherwise it returns c.
let inline toLower (c : char) =
    System.Char.ToLowerInvariant c

//
let inline toUpper (c : char) =
    System.Char.ToUpperInvariant c

/// returns true if c is a letter (lowercase or uppercase).
let inline isAlpha (c : char) =
    System.Char.IsLetter c

/// returns true if c is alphanumeric (a letter or a decimal digit).
let inline isAlphaNum (c : char) =
    System.Char.IsLetterOrDigit c

/// returns true if c is a control character.
let inline isCntrl (c : char) =
    System.Char.IsControl c

/// returns true if c is a decimal digit [0-9].
let inline isDigit (c : char) =
    System.Char.IsDigit c

/// returns true if c is a graphical character, that is, it is printable and not a whitespace character.
let isGraph (c : char) =
    not <| System.Char.IsWhiteSpace c

/// returns true if c is a hexadecimal digit [0-9a-fA-F].
let isHexDigit (c : char) =
    match c with
    | '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9'
    | 'a' | 'b' | 'c' | 'd' | 'e' | 'f'
    | 'A' | 'B' | 'C' | 'D' | 'E' | 'F' -> true
    | _ -> false

/// returns true if c is a lowercase letter.
let inline isLower (c : char) =
    System.Char.IsLower c

/// returns true if c is a printable character (space or visible), i.e., not a control character.
let isPrint (c : char) =
    raise <| System.NotImplementedException "Char.isPrint"

/// returns true if c is a whitespace character (space, newline, tab, carriage return, vertical tab, formfeed).
let isSpace (c : char) =
    System.Char.IsWhiteSpace c

/// returns true if c is a punctuation character: graphical but not alphanumeric.
let isPunct (c : char) =
    System.Char.IsPunctuation c

//
let inline isUpper (c : char) =
    System.Char.IsUpper c

//
let toString (c : char) =
    // TODO : The semantics defined for this method need to be handled correctly
    // -- we can't just call .ToString().
    raise <| System.NotImplementedException "Char.toString"

(*
val scan       : (Char.char, 'a) StringCvt.reader -> (char, 'a) StringCvt.reader
val fromString : String.string -> char option
val toCString : char -> String.string
val fromCString : String.string -> char option
*)
