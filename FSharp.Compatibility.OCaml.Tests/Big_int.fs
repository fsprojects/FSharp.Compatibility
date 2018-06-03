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

module FSharp.Compatibility.OCaml.Big_int.Tests

#nowarn "62"

open System
open System.Numerics
open Microsoft.FSharp.Math
open FSharp.Compatibility.OCaml.Big_int
open FSharp.Compatibility.OCaml
open FSharp.Compatibility.OCaml.Num
open NUnit.Framework
open MathNet.Numerics
open FsUnit


[<Test>]
let Parse() =
    Assert.AreEqual(big_int_of_string "0",      zero_big_int, "1")
    Assert.AreEqual(big_int_of_string "0xABC",  bigint 2748,  "2")
    Assert.AreEqual(big_int_of_string "2748",   bigint 2748,  "3")
    Assert.AreEqual(big_int_of_string "-0xABC", bigint -2748, "4")
    Assert.AreEqual(big_int_of_string "-2748",  bigint -2748, "5")
    Assert.AreEqual(big_int_of_string "0xabc",  bigint 2748,  "6")
    Assert.AreEqual(big_int_of_string "0xaBc",  bigint 2748,  "7")
    //Assert.AreEqual(big_int_of_string "0o123",  bigint 291,   "8")
    //Assert.AreEqual(big_int_of_string "0b101",  bigint 5,     "9")
    Assert.AreEqual(big_int_of_string "5",  bigint 5,         "10")
    ()