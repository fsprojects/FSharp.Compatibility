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
module FSharp.Compatibility.OCaml.Array2D

open System
open System.Runtime.InteropServices

#nowarn "9"
// The address-of operator may result in non-verifiable code.
// Its use is restricted to passing byrefs to functions that require them.
#nowarn "51"


let inline pinObjUnscoped (obj : obj) =
    GCHandle.Alloc(obj,GCHandleType.Pinned)

let inline pinObj (obj : obj) f =
    let gch = pinObjUnscoped obj 
    try f gch
    finally
        gch.Free ()

[<Unverifiable>]
[<NoDynamicInvocation>]
let inline pin (arr : 'T[,]) (f : nativeptr<'T> -> 'U) =
    pinObj (box arr) <| fun _ -> f &&arr.[0,0]
    
[<Unverifiable>]
[<NoDynamicInvocation>]
let inline pinUnscoped (arr : 'T [,]) : nativeptr<'T> * _ =
    let gch = pinObjUnscoped (box arr)
    &&arr.[0,0], gch

[<Unverifiable>]
[<NoDynamicInvocation>]
[<Obsolete("This function has been renamed to 'pinUnscoped'")>]
let inline pin_unscoped arr =
    pinUnscoped arr


