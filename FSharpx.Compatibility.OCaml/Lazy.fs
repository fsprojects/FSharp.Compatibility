(*  OCaml Compatibility Library for F#
    (FSharpx.Compatibility.OCaml)

    Copyright (c) Microsoft Corporation 2005-2009
    Copyright (c) Jack Pappas 2012
        http://github.com/jack-pappas

    This code is distributed under the terms of the Apache 2.0 license.
    See the LICENSE file for details. *)

/// Deferred computations.
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharpx.Compatibility.OCaml.Lazy

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

