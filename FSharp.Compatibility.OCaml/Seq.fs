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

//
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharp.Compatibility.OCaml.Seq

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

