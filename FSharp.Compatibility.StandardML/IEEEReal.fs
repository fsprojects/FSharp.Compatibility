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
// http://www.standardml.org/Basis/ieee-float.html

/// The IEEEReal structure defines types associated with an IEEE implementation
/// of floating-point numbers. In addition, it provides control for the floating-point
/// hardware's rounding mode. Refer to the IEEE standard 754-1985 and the ANSI/IEEE
/// standard 854-1987 for additional information.
module FSharp.Compatibility.StandardML.IEEEReal

//
exception Unordered

//
type real_order = LESS | EQUAL | GREATER | UNORDERED

//
type float_class =
    | NAN
    | INF
    | ZERO
    | NORMAL
    | SUBNORMAL

//
type rounding_mode =
    | TO_NEAREST
    | TO_NEGINF
    | TO_POSINF
    | TO_ZERO

//
type decimal_approx = {
    ``class`` : float_class;
    sign : bool;
    digits : int list;
    exp : int;
}



(*

val setRoundingMode : rounding_mode -> unit
val getRoundingMode : unit -> rounding_mode

val toString : decimal_approx -> string
val scan       : (char, 'a) StringCvt.reader
                   -> (decimal_approx, 'a) StringCvt.reader
val fromString : string -> decimal_approx option

*)
