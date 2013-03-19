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
// http://www.standardml.org/Basis/word.html

//
module FSharp.Compatibility.StandardML.Word

//
type word = unativeint

/// The number of bits in type word. wordSize need not be a power of two.
/// Note that word has a fixed, finite precision.
let wordSize : int =
    sizeof<word> * 8

(*

val toLarge      : word -> LargeWord.word
val toLargeX     : word -> LargeWord.word
val toLargeWord  : word -> LargeWord.word
val toLargeWordX : word -> LargeWord.word
val fromLarge     : LargeWord.word -> word
val fromLargeWord : LargeWord.word -> word
val toLargeInt  : word -> LargeInt.int
val toLargeIntX : word -> LargeInt.int
val fromLargeInt : LargeInt.int -> word
val toIntX : word -> int

*)

//
let inline toInt (w : word) =
    int w

//
let inline fromInt (i : int) : word =
    unativeint i

/// Bitwise AND.
let inline andb (w1 : word, w2 : word) : word =
    w1 &&& w2

/// Bitwise OR.
let inline orb (w1 : word, w2 : word) : word =
    w1 ||| w2

/// Bitwise XOR.
let inline xorb (w1 : word, w2 : word) : word =
    w1 ^^^ w2

/// returns the bit-wise complement (NOT) of i.
let inline notb (w : word) : word =
    ~~~w

/// shifts i to the left by n bit positions, filling in zeros from the right. When i and n are
/// interpreted as unsigned binary numbers, this returns (i* 2(n))(mod (2 (wordSize))).
/// In particular, shifting by greater than or equal to wordSize results in 0.
/// This operation is similar to the ``(logical) shift left'' instruction in many processors.
let inline (<<) (i : word) (n : word) : word =
    i <<< (int n)

/// shifts i to the right by n bit positions, filling in zeros from the left.
/// When i and n are interpreted as unsigned binary numbers, it returns floor((i / 2(n))).
/// In particular, shifting by greater than or equal to wordSize results in 0.
/// This operation is similar to the ``logical shift right'' instruction in many processors. 
let inline (>>) (i : word) (n : word) : word =
    i >>> (int n)

//
// let inline (~>>) (i : word) (n : word) =
//    i >>> (int n)

(*

val + : word * word -> word
val - : word * word -> word
val * : word * word -> word
val div : word * word -> word
val mod : word * word -> word

*)

//
let inline compare (i : word, j : word) : order =
    let comp = compare i j
    if comp = 0 then EQUAL
    elif comp < 0 then LESS
    else GREATER

(*

val <  : word * word -> bool
val <= : word * word -> bool
val >  : word * word -> bool
val >= : word * word -> bool

val ~ : word -> word
val min : word * word -> word
val max : word * word -> word

val fmt      : StringCvt.radix -> word -> string
val toString : word -> string
val scan       : StringCvt.radix
                   -> (char, 'a) StringCvt.reader
                     -> (word, 'a) StringCvt.reader
val fromString : string -> word option

*)
