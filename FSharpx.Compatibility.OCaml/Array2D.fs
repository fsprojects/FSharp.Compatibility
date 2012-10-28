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
module FSharpx.Compatibility.OCaml.Array2D

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


