(*

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

module FSharp.Compatibility.OCaml.Num.Tests

#nowarn "62"

open System
open System.Numerics
open Microsoft.FSharp.Math
open FSharp.Compatibility.OCaml.Num
open NUnit.Framework
open FsUnit

(* TODO : Import the BigRational tests from the F# PowerPack. *)


[<Test>]
let ``0 does not equal 1``() =
    (Int 0) = (Int 1)
    |> should equal false

(* op_Division *)

[<Test>]
[<ExpectedException(typeof<exn>, ExpectedMessage = "Division_by_zero")>]
let ``division by zero raises exception``() =
    (Int 1) / (Int 0)
    |> ignore

[<Test>]
let ``division should not truncate result``() =
    let result = (Int 2) / (Int 3)
    result.IsZero
    |> should equal false

(* op_Modulus *)

[<Test>]
[<ExpectedException(typeof<exn>, ExpectedMessage = "Division_by_zero")>]
let ``mod_num zero divisor raises exception``() =
    mod_num (Int 1) (Int 0)
    |> ignore

[<Test>]
let ``mod_num test 1``() =
    let numerator = (Int 4) / (Int 9)
    mod_num numerator (Int 2)
    |> should equal numerator

[<Test>]
let ``mod_num test 2``() =
    let result = mod_num ((Int 4) / (Int 9)) ((Int 2) / (Int 9))
    result.IsZero
    |> should equal true


(* Quotient *)

[<Test>]
[<ExpectedException(typeof<exn>, ExpectedMessage = "Division_by_zero")>]
let ``quo_num zero divisor raises exception``() =
    quo_num (Int 1) (Int 0)
    |> ignore

[<Test>]
let ``quo_num should truncate result``() =
    let result = quo_num (Int 2) (Int 3)
    result.IsZero
    |> should equal true


(* Parse and num_of_string *)

[<Test>]
let ``num_of_string parse integer``() =
    let expectedValue = 1234
    num_of_string (expectedValue.ToString ())
    |> should equal (Int expectedValue)

[<Test>]
let ``num_of_string parse big integer``() =
    let expectedValue = BigInteger.Parse "28492458193028194"
    num_of_string (expectedValue.ToString ())
    |> should equal (Big_int expectedValue)

[<Test>]
[<ExpectedException(typeof<exn>, ExpectedMessage = "num_of_string")>]
let ``num_of_string invalid string``() =
    num_of_string "123.4"
    |> ignore




