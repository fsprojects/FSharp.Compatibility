(*  OCaml Compatibility Library for F#
    (FSharpx.Compatibility.OCaml)

    Copyright © 2012 Jack Pappas (github.com/jack-pappas)

    This code is available under the Apache 2.0 license.
    See the LICENSE file for the complete text of the license. *)

module FSharpx.Compatibility.OCaml.Tests.Num

open NUnit.Framework
open FsUnit

open System
open System.Numerics
open Microsoft.FSharp.Math
open FSharpx.Compatibility.OCaml.Num


[<Test>]
let ``0 does not equal 1``() =
    (Int 0) = (Int 1)
    |> should equal false





