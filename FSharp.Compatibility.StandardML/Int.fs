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
// http://www.standardml.org/Basis/integer.html

/// <summary>
/// Instances of the INTEGER signature provide a type of signed integers of either
/// a fixed or arbitrary precision, and arithmetic and conversion operations.
/// </summary>
/// <remarks>
/// For fixed precision implementations, most arithmetic operations raise the
/// exception Overflow when their result is not representable.
/// </remarks>
module FSharp.Compatibility.StandardML.Int

type int = System.Int32



(*

val toLarge   : int -> LargeInt.int
val fromLarge : LargeInt.int -> int
val toInt   : int -> Int.int
val fromInt : Int.int -> int

*)

/// If SOME(n), this denotes the number n of significant bits in type int,
/// including the sign bit. If it is NONE, int has arbitrary precision.
/// The precision need not necessarily be a power of two.
let precision : int option =
    Some 32

//
let minInt =
    Some System.Int32.MinValue

//
let maxInt =
    Some System.Int32.MaxValue

(*

val + : int * int -> int
val - : int * int -> int
val * : int * int -> int
val div : int * int -> int
val mod : int * int -> int
val quot : int * int -> int
val rem : int * int -> int

*)

//
let compare (i : int, j : int) : order =
    let comp = compare i j
    if comp = 0 then EQUAL
    elif comp < 0 then LESS
    else GREATER

/// returns the absolute value (magnitude) of i.
/// It raises Overflow when the result is not representable.
let abs (i : int) : int =
    match i with
    | 0x80000000 ->
        raise Overflow
    | i ->
        System.Math.Abs i

//
let inline min (i : int, j : int) =
    System.Math.Min (i, j)

//
let inline max (i : int, j : int) =
    System.Math.Max (i, j)

/// returns -1, 0, or 1 when i is less than, equal to, or greater than 0, respectively.
let inline sign (i : int) =
    // Need to use the fully-qualified name for the built-in F# 'compare' here
    // because it's masked by the 'compare' function defined above.
    Microsoft.FSharp.Core.Operators.compare i 0

/// returns true if i and j have the same sign. It is equivalent to (sign i = sign j).
let inline sameSign (i : int, j : int) =
    sign i = sign j

(*
val fmt      : StringCvt.radix -> int -> string
*)

//
let toString (i : int) : string =
    raise <| System.NotImplementedException "Int.toString"

(*
val scan       : StringCvt.radix -> (char, 'a) StringCvt.reader -> (int, 'a) StringCvt.reader
*)

//
let fromString (s : string) : int option =
    raise <| System.NotImplementedException "Int.fromString"

