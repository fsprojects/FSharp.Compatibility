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

/// Deferred computations.
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharp.Compatibility.OCaml.Lazy

open Microsoft.FSharp.Control


//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
type 'T t = Lazy<'T>

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'v.Force()' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let inline force_val (x : Lazy<'T>) =
    x.Force ()

/// Build a lazy (delayed) value from the given computation.
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'lazy' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let inline lazy_from_fun (f : unit -> 'T) =
    Lazy.Create f

/// Build a lazy (delayed) value from the given pre-computed value.
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'Lazy.CreateFromValue' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let inline lazy_from_val (value : 'T) =
    Lazy.CreateFromValue value

/// Check if a lazy (delayed) value has already been computed.
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using 'Lazy.IsForced' instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let inline lazy_is_val (x : Lazy<'T>) =
    x.IsValueCreated

/// Build a lazy (delayed) value from the given computation.
[<CompilerMessage(
    "This construct is for ML compatibility. \
    Consider using Lazy.Create instead. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let inline create (f : unit -> 'T) =
    Lazy.Create f

