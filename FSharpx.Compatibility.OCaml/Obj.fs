(*  OCaml Compatibility Library for F#
    (FSharpx.Compatibility.OCaml)

    Copyright (c) Microsoft Corporation 2005-2009
    Copyright (c) Jack Pappas 2012
        http://github.com/jack-pappas

    This code is distributed under the terms of the Apache 2.0 license.
    See the LICENSE file for details. *)

/// Operations on internal representations of values.
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharpx.Compatibility.OCaml.Obj

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'null' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let [<Literal>] nullobj : obj = null

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'obj' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
type t = obj

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'box' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let inline repr x = box x

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'unbox' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let inline obj (x : obj) = unbox x

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'box' and/or 'unbox' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let inline magic x = obj (repr x)

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'Microsoft.FSharp.Core.LanguagePrimitives.PhysicalEquality' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let inline eq (x: 'T) (y: 'T) =
    LanguagePrimitives.PhysicalEquality x y

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'not(Microsoft.FSharp.Core.LanguagePrimitives.PhysicalEquality(...))' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let inline not_eq (x : 'T) (y : 'T) =
    not (LanguagePrimitives.PhysicalEquality x y)

