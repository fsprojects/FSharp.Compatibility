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
// http://www.standardml.org/Basis/string.html

/// The STRING signature specifies the basic operations on a string type, which is
/// a vector of the underlying character type char as defined in the structure. 
module FSharp.Compatibility.StandardML.String

open Option

(*
eqtype string
eqtype char
*)

/// The longest allowed size of a string.
let maxSize = System.Int32.MaxValue

/// returns |s|, the number of characters in string s.
let inline size (s : string) =
    String.length s

/// returns the i(th) character of s, counting from zero.
/// This raises Subscript if i < 0 or |s| <= i.
let sub (s : string, i) =
    if i < 0 || String.length s <= i then
        raise Subscript
    else
        s.[i]
    
/// These return substrings of s.
/// The first returns the substring of s from the i(th) character to the end of the string, i.e., the string s[i..|s|-1].
/// This raises Subscript if i < 0 or |s| < i.
/// The second form returns the substring of size j starting at index i, i.e., the string s[i..i+j-1].
/// It raises Subscript if i < 0 or j < 0 or |s| < i + j. Note that, if defined, extract returns the empty string when i = |s|.
/// The third form returns the substring s[i..i+j-1], i.e., the substring of size j starting at index i.
/// This is equivalent to extract(s, i, SOME j).
let extract (s : string, i, j) =
    let len = String.length s
    if i < 0 || len < i then
        raise Subscript
    else
        match j with
        | NONE ->
            s.[i..]
        | SOME j ->
            if j < 0 || len < i + j then
                raise Subscript
            else
                s.[i..i+j-1]

//
let inline substring (s : string, i, j) =
    extract (s, i, SOME j)

// TODO
// val ^ : string * string -> string
// let inline (^) (s : string) (t : string) = s ^ t

/// is the concatenation of all the strings in l.
let concat (l : string list) =
    String.concat "" l

/// returns the concatenation of the strings in the list l using the string s as a separator.
let concatWith s (l : string list) =
    String.concat s l

/// is the string of size one containing the character c.
let str (c : char) =
    c.ToString ()

/// generates the string containing the characters in the list l.
/// This is equivalent to concat (List.map str l). 
let implode (l : char list) =
    // OPTIMIZATION : If the list is empty return immediately.
    match l with
    | [] -> ""
    | l ->
        let sb = System.Text.StringBuilder ()
        l |> List.iter (sb.Append >> ignore)
        sb.ToString ()

/// is the list of characters in the string s.
let explode (s : string) =
    let len = String.length s

    // OPTIMIZATION : If the string is empty return immediately.
    if len = 0 then []
    else
        // Build the list backwards so it doesn't need to be reversed.
        let rec charList chars idx =
            if idx = 0 then
                s.[0] :: chars
            else
                charList (s.[idx] :: chars) (idx - 1)

        charList [] (len - 1)

/// applies f to each element of s from left to right, returning the resulting string.
/// It is equivalent to implode(List.map f (explode s)).
let map (f : char -> char) (s : string) : string =
    let len = String.length s

    // OPTIMIZATION : If the string is empty return immediately.
    if len = 0 then ""
    else
        let sb = System.Text.StringBuilder (len)
        String.iter (f >> sb.Append >> ignore) s
        sb.ToString ()
    
/// returns the string generated from s by mapping each character in s by f.
/// It is equivalent to concat(List.map f (explode s)).
let translate (f : char -> string) (s : string) =
    let len = String.length s

    // OPTIMIZATION : If the string is empty return immediately.
    if len = 0 then ""
    else
        let sb = System.Text.StringBuilder ()
        String.iter (f >> sb.Append >> ignore) s
        sb.ToString ()

//
let tokens (f : char -> bool) (s : string) : string list =
    raise <| System.NotImplementedException "String.tokens"

//
let fields (f : char -> bool) (s : string) : string list =
    raise <| System.NotImplementedException "String.fields"

/// These functions return true if the string s1 is a prefix, substring, or suffix
/// (respectively) of the string s2. Note that the empty string is a prefix, substring,
/// and suffix of any string, and that a string is a prefix, substring, and suffix of itself. 
let isPrefix (s1 : string) (s2 : string) : bool =
    s2.StartsWith s1

//
let isSubstring (s1 : string) (s2 : string) : bool =
    s2.IndexOf s1 <> -1

//
let isSuffix (s1 : string) (s2 : string) : bool =
    s2.EndsWith s1

//
let compare (s : string, t : string) : order =
    raise <| System.NotImplementedException "String.compare"

//
let collate (f : char * char -> order) (s : string, t : string) : order =
    raise <| System.NotImplementedException "String.collate"


(*

val <  : string * string -> bool
val <= : string * string -> bool
val >  : string * string -> bool
val >= : string * string -> bool

val toString : string -> String.string
val scan       : (char, 'a) StringCvt.reader -> (string, 'a) StringCvt.reader
val fromString : String.string -> string option
val toCString : string -> String.string
val fromCString : String.string -> string option

*)
