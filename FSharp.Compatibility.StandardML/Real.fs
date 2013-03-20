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
// http://www.standardml.org/Basis/real.html

/// <summary>
/// The REAL signature specifies structures that implement floating-point numbers.
/// </summary>
/// <remarks>
/// <para>The semantics of floating-point numbers should follow the IEEE standard 754-1985
/// and the ANSI/IEEE standard 854-1987. In addition, implementations of the REAL signature
/// are required to use non-trapping semantics. Additional aspects of the design of the REAL
/// and MATH signatures were guided by the Floating-Point C Extensions developed by the
/// X3J11 ANSI committee and the lecture notes by W. Kahan on the IEEE standard 754.</para>
/// <para>Although there can be many representations for NaN values, the Library models them
/// as a single value and currently provides no explicit way to distinguish among them,
/// ignoring the sign bit. Thus, in the descriptions below and in the Math structure,
/// we just refer to the NaN value.</para>
/// </remarks>
module FSharp.Compatibility.StandardML.Real

type real = System.Double

/// The base of the representation, e.g., 2 or 10 for IEEE floating point.
let radix = 2

/// The number of digits, each between 0 and radix-1, in the mantissa.
/// Note that the precision includes the implicit (or hidden) bit used in
/// the IEEE representation (e.g., the value of Real64.precision is 53).
let precision = 53





(*

val maxFinite    : real
val minPos       : real
val minNormalPos : real

val posInf : real
val negInf : real

val + : real * real -> real
val - : real * real -> real
val * : real * real -> real
val / : real * real -> real
val rem : real * real -> real
val *+ : real * real * real -> real
val *- : real * real * real -> real
val ~ : real -> real
val abs : real -> real

val min : real * real -> real
val max : real * real -> real

val sign : real -> int
val signBit : real -> bool
val sameSign : real * real -> bool
val copySign : real * real -> real

*)

/// returns LESS, EQUAL, or GREATER according to whether its first argument is less than,
/// equal to, or greater than the second.
/// It raises IEEEReal.Unordered on unordered arguments.
// TODO : Implement functionality to check for unordered arguments and raise IEEEReal.Unordered.
let compare (x : real, y : real) =
    let comp = compare x y
    if comp = 0 then EQUAL
    elif comp < 0 then LESS
    else GREATER
    

(*

val compare     : real * real -> order
val compareReal : real * real -> IEEEReal.real_order
val <  : real * real -> bool
val <= : real * real -> bool
val >  : real * real -> bool
val >= : real * real -> bool
val == : real * real -> bool
val != : real * real -> bool
val ?= : real * real -> bool
val unordered : real * real -> bool

val isFinite : real -> bool
val isNan : real -> bool
val isNormal : real -> bool
val class : real -> IEEEReal.float_class

val toManExp : real -> {man : real, exp : int}
val fromManExp : {man : real, exp : int} -> real
val split   : real -> {whole : real, frac : real}
val realMod : real -> real

val nextAfter : real * real -> real
val checkFloat : real -> real

val realFloor : real -> real
val realCeil  : real -> real
val realTrunc : real -> real
val realRound : real -> real
val floor : real -> int
val ceil  : real -> int
val trunc : real -> int
val round : real -> int
val toInt      : IEEEReal.rounding_mode -> real -> int
val toLargeInt : IEEEReal.rounding_mode
                   -> real -> LargeInt.int
val fromInt      : int -> real
val fromLargeInt : LargeInt.int -> real
val toLarge   : real -> LargeReal.real
val fromLarge : IEEEReal.rounding_mode
                  -> LargeReal.real -> real

val fmt      : StringCvt.realfmt -> real -> string
*)

//
let toString (r : real) : string =
    raise <| System.NotImplementedException "Real.toString"

(*
val toString : real -> string
val scan       : (char, 'a) StringCvt.reader
                   -> (real, 'a) StringCvt.reader
val fromString : string -> real option

val toDecimal   : real -> IEEEReal.decimal_approx
val fromDecimal : IEEEReal.decimal_approx -> real option

*)
