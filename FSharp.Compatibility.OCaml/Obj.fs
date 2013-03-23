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

/// Operations on internal representations of values.
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharp.Compatibility.OCaml.Obj

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

