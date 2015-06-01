(*

Copyright 2005-2009 Microsoft Corporation
Copyright 2012 Jack Pappas

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

// References:
// http://caml.inria.fr/pub/docs/manual-ocaml/libref/Ratio.html

/// <summary>Operation on rational numbers.</summary>
/// <remarks>This module is used to support the implementation of Num and should not be called directly.</remarks>
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharp.Compatibility.OCaml.Ratio

open System.Numerics
open MathNet.Numerics
open FSharp.Compatibility.OCaml.Big_int

/// Infinite-precision rational number.
type ratio = BigRational

//
let numerator_ratio (r : ratio) : big_int =
    r.Numerator

//
let denominator_ratio (r : ratio) : big_int =
    r.Denominator

////
//let normalize_ratio (r : ratio) =
//    raise <| System.NotImplementedException "Ratio.normalize_ratio"

(*

val null_denominator : ratio -> bool
val sign_ratio : ratio -> int
val normalize_ratio : ratio -> ratio
val cautious_normalize_ratio : ratio -> ratio
val cautious_normalize_ratio_when_printing : ratio -> ratio
val create_ratio : big_int -> big_int -> ratio
val create_normalized_ratio : big_int -> big_int -> ratio
val is_normalized_ratio : ratio -> bool
val report_sign_ratio : ratio -> big_int -> big_int
val abs_ratio : ratio -> ratio
val is_integer_ratio : ratio -> bool
val add_ratio : ratio -> ratio -> ratio
val minus_ratio : ratio -> ratio
val add_int_ratio : int -> ratio -> ratio
val add_big_int_ratio : big_int -> ratio -> ratio
val sub_ratio : ratio -> ratio -> ratio
val mult_ratio : ratio -> ratio -> ratio
val mult_int_ratio : int -> ratio -> ratio
val mult_big_int_ratio : big_int -> ratio -> ratio
val square_ratio : ratio -> ratio
val inverse_ratio : ratio -> ratio
val div_ratio : ratio -> ratio -> ratio
val integer_ratio : ratio -> big_int
val floor_ratio : ratio -> big_int
val round_ratio : ratio -> big_int
val ceiling_ratio : ratio -> big_int
val eq_ratio : ratio -> ratio -> bool
val compare_ratio : ratio -> ratio -> int
val lt_ratio : ratio -> ratio -> bool
val le_ratio : ratio -> ratio -> bool
val gt_ratio : ratio -> ratio -> bool
val ge_ratio : ratio -> ratio -> bool
val max_ratio : ratio -> ratio -> ratio
val min_ratio : ratio -> ratio -> ratio
val eq_big_int_ratio : big_int -> ratio -> bool
val compare_big_int_ratio : big_int -> ratio -> int
val lt_big_int_ratio : big_int -> ratio -> bool
val le_big_int_ratio : big_int -> ratio -> bool
val gt_big_int_ratio : big_int -> ratio -> bool
val ge_big_int_ratio : big_int -> ratio -> bool
val int_of_ratio : ratio -> int
val ratio_of_int : int -> ratio
val ratio_of_nat : nat -> ratio
val nat_of_ratio : ratio -> nat
val ratio_of_big_int : big_int -> ratio
val big_int_of_ratio : ratio -> big_int
val div_int_ratio : int -> ratio -> ratio
val div_ratio_int : ratio -> int -> ratio
val div_big_int_ratio : big_int -> ratio -> ratio
val div_ratio_big_int : ratio -> big_int -> ratio
val approx_ratio_fix : int -> ratio -> string
val approx_ratio_exp : int -> ratio -> string
val float_of_rational_string : ratio -> string
val string_of_ratio : ratio -> string
val ratio_of_string : string -> ratio
val float_of_ratio : ratio -> float
val power_ratio_positive_int : ratio -> int -> ratio
val power_ratio_positive_big_int : ratio -> big_int -> ratio

*)

