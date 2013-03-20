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
// http://www.standardml.org/Basis/math.html

/// <summary>
/// The signature MATH specifies basic mathematical constants, the square root function,
/// and trigonometric, hyperbolic, exponential, and logarithmic functions based on
/// a real type. The functions defined here have roughly the same semantics as their
/// counterparts in ISO C's math.h.
/// </summary>
/// <remarks>
/// The top-level structure Math provides these functions for the default real type Real.real.
/// In the functions below, unless specified otherwise, if any argument is a NaN,
/// the return value is a NaN. In a list of rules specifying the behavior of a function
/// in special cases, the first matching rule defines the semantics. 
/// </remarks>
module FSharp.Compatibility.StandardML.Math

open Real


/// The constant pi (3.141592653...).
let [<Literal>] pi : real = System.Math.PI

/// The base e (2.718281828...) of the natural logarithm.
let [<Literal>] e : real = System.Math.E

/// returns the square root of x.
/// sqrt (~0.0) = ~0.0. If x < 0, it returns NaN. 
let inline sqrt (x : real) : real =
    System.Math.Sqrt x

(*
These return the sine, cosine, and tangent, respectively, of x, measured in radians.
If x is an infinity, these functions return NaN.
Note that tan will produce infinities at various finite values, roughly corresponding
to the singularities of the tangent function. 
*)

//
let inline sin (x : real) : real =
    System.Math.Sin x

//
let inline cos (x : real) : real =
    System.Math.Cos x

//
let inline tan (x : real) : real =
    System.Math.Tan x

(*
These return the arc sine and arc cosine, respectively, of x. asin is the inverse of sin.
Its result is guaranteed to be in the closed interval [-pi/2,pi/2]. acos is the inverse of cos.
Its result is guaranteed to be in the closed interval [0,pi].
If the magnitude of x exceeds 1.0, they return NaN. 
*)

//
let inline asin (x : real) : real =
    System.Math.Asin x

//
let inline acos (x : real) : real =
    System.Math.Acos x

/// returns the arc tangent of x. atan is the inverse of tan.
/// For finite arguments, the result is guaranteed to be in the open interval (-pi/2,pi/2).
/// If x is +infinity, it returns pi/2; if x is -infinity, it returns -pi/2. 
let inline atan (x : real) : real =
    System.Math.Atan x

//
let inline atan2 (y : real, x : real) : real =
    System.Math.Atan2 (y, x)

/// returns e(x), i.e., e raised to the x(th) power.
/// If x is +infinity, it returns +infinity; if x is -infinity, it returns 0. 
let inline exp (x : real) : real =
    System.Math.Exp x

/// returns x(y), i.e., x raised to the y(th) power.
/// For finite x and y, this is well-defined when x > 0, or when x < 0 and y is integral.
let inline pow (x : real, y : real) : real =
    System.Math.Pow (x, y)

(*
These return the natural logarithm (base e) and decimal logarithm (base 10), respectively, of x.
If x < 0, they return NaN; if x = 0, they return -infinity; if x is infinity, they return infinity.
*)

//
let inline ln (x : real) : real =
    System.Math.Log x

//
let inline log10 (x : real) : real =
    System.Math.Log10 x

(*
These return the hyperbolic sine, hyperbolic cosine, and hyperbolic tangent, respectively,
of x, that is, the values (e(x) - e(-x)) / 2, (e(x) + e(-x)) / 2, and (sinh x)/(cosh x). 
*)

//
let inline sinh (x : real) : real =
    System.Math.Sinh x

//
let inline cosh (x : real) : real =
    System.Math.Cosh x

//
let inline tanh (x : real) : real =
    System.Math.Tanh x
