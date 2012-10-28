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
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharpx.Compatibility.OCaml.Seq

open System
open System.Collections.Generic


//
[<Obsolete("This function will be removed in a future release. Use a sqeuence expression instead.")>]
let generate openf compute closef =
    seq {
    let r = openf ()
    try
        let x = ref None
        while (x := compute r; (!x).IsSome) do
            yield (!x).Value
    finally
        closef r }

