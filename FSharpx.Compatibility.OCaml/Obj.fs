(*  OCaml Compatibility Library for F#
    (FSharpx.Compatibility.OCaml)

    Copyright (c) Microsoft Corporation 2005-2009
    Copyright (c) Jack Pappas 2012
        http://github.com/jack-pappas

    This code is distributed under the terms of the Apache 2.0 license.
    See the LICENSE file for details. *)

//
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharpx.Compatibility.OCaml.Obj

//
type t = obj

//
let repr x = box x

//
let obj (x : obj) = unbox x

//
let magic x = obj (repr x)

//
let [<Literal>] nullobj : obj = null

//
let eq (x: 'T) (y: 'T) = LanguagePrimitives.PhysicalEquality x y

//
let not_eq (x : 'T) (y : 'T) = not (LanguagePrimitives.PhysicalEquality x y)

