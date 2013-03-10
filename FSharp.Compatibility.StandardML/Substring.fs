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
// http://www.standardml.org/Basis/substring.html

//
module FSharp.Compatibility.StandardML.Substring

// string
// dropl
// dropr
// full


(*

type substring
eqtype char
eqtype string

val sub : substring * int -> char
val size : substring -> int
val base : substring -> string * int * int
val extract   : string * int * int option -> substring
val substring : string * int * int -> substring
val full : string -> substring
val string : substring -> string
val isEmpty : substring -> bool
val getc : substring -> (char * substring) option
val first : substring -> char option
val triml : int -> substring -> substring
val trimr : int -> substring -> substring
val slice : substring * int * int option -> substring
val concat : substring list -> string
val concatWith : string -> substring list -> string
val explode : substring -> char list
val isPrefix    : string -> substring -> bool
val isSubstring : string -> substring -> bool
val isSuffix    : string -> substring -> bool
val compare : substring * substring -> order
val collate : (char * char -> order)
                -> substring * substring -> order
val splitl : (char -> bool)
                -> substring -> substring * substring
val splitr : (char -> bool)
                -> substring -> substring * substring
val splitAt : substring * int -> substring * substring
val dropl : (char -> bool) -> substring -> substring
val dropr : (char -> bool) -> substring -> substring
val takel : (char -> bool) -> substring -> substring
val taker : (char -> bool) -> substring -> substring
val position : string -> substring -> substring * substring
val span : substring * substring -> substring
val translate : (char -> string) -> substring -> string
val tokens : (char -> bool) -> substring -> substring list
val fields : (char -> bool) -> substring -> substring list
val app : (char -> unit) -> substring -> unit
val foldl : (char * 'a -> 'a) -> 'a -> substring -> 'a
val foldr : (char * 'a -> 'a) -> 'a -> substring -> 'a

*)
