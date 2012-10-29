(*  OCaml Compatibility Library for F#
    (FSharpx.Compatibility.OCaml)

    Copyright (c) Microsoft Corporation 2005-2009
    Copyright (c) Jack Pappas 2012
        http://github.com/jack-pappas

    This code is distributed under the terms of the Apache 2.0 license.
    See the LICENSE file for details. *)

// Reference:
// http://caml.inria.fr/pub/docs/manual-ocaml/libref/Array.html

/// Array operations.
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharpx.Compatibility.OCaml.Array

open System
open System.Runtime.InteropServices

#nowarn "9"
// The address-of operator may result in non-verifiable code.
// Its use is restricted to passing byrefs to functions that require them.
#nowarn "51"


//
let make_matrix (dimx : int) (dimy : int) (e : 'a) : 'a[][] =
    // Preconditions
    // TODO (make sure they match the original OCaml preconditions)

    Array.init dimx <| fun _ ->
    Array.init dimy <| fun _ ->
        e

//
[<Obsolete("Deprecated. Use 'make_matrix' instead.")>]
[<CompilerMessage("This construct is for ML compatibility. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden = true)>]
let inline create_matrix (dimx : int) (dimy : int) (e : 'a) =
    make_matrix dimx dimy e

[<Obsolete("This function will be removed. Use 'not Array.isEmpty' instead")>]
let nonempty (arr: _[]) = (arr.Length <> 0)
    
let inline pinObjUnscoped (obj: obj) =
    GCHandle.Alloc(obj,GCHandleType.Pinned) 

let inline pinObj (obj: obj) f = 
    let gch = pinObjUnscoped obj 
    try f gch
    finally
        gch.Free()
    
[<Unverifiable>]
[<NoDynamicInvocation>]
let inline pin (arr: 'T []) (f : nativeptr<'T> -> 'U) = 
    pinObj (box arr) (fun _ -> f (&&arr.[0]))
    
[<Unverifiable>]
[<NoDynamicInvocation>]
let inline pinUnscoped (arr: 'T []) : nativeptr<'T> * _ = 
    let gch = pinObjUnscoped (box arr) 
    &&arr.[0], gch

[<Unverifiable>]
[<NoDynamicInvocation>]
[<Obsolete("This function has been renamed to 'pinUnscoped'")>]
let inline pin_unscoped arr = pinUnscoped arr 
      

let inline contains x (arr: 'T []) =
    // OPTIMIZE : Replace the implementation below with
    // a simpler call to System.Array.IndexOf
    let mutable found = false
    let mutable i = 0
    let eq = LanguagePrimitives.FastGenericEqualityComparer

    while not found && i < arr.Length do
        if eq.Equals(x,arr.[i]) then
            found <- true
        else
            i <- i + 1
    found

[<CompilerMessage("This construct is for ML compatibility. The F# name for this function is 'contains'. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden = true)>]
let inline mem x arr =
    contains x arr
        
let scanSubRight f (arr : _[]) start fin initState = 
    let mutable state = initState 
    let res = Array.create (2+fin-start) initState 
    for i = fin downto start do
        state <- f arr.[i] state;
        res.[i - start] <- state
    done;
    res

let scanSubLeft f  initState (arr : _[]) start fin = 
    let mutable state = initState 
    let res = Array.create (2+fin-start) initState 
    for i = start to fin do
        state <- f state arr.[i];
        res.[i - start+1] <- state
    done;
    res

let scanReduce f (arr : _[]) = 
    let arrn = arr.Length
    if arrn = 0 then invalidArg "arr" "the input array is empty"
    else scanSubLeft f arr.[0] arr 1 (arrn - 1)

let scanReduceBack f (arr : _[])  = 
    let arrn = arr.Length
    if arrn = 0 then invalidArg "arr" "the input array is empty"
    else scanSubRight f arr 0 (arrn - 2) arr.[arrn - 1]

let createJaggedMatrix (n:int) (m:int) (x:'T) = 
    let arr = (Array.zeroCreate n : 'T [][]) 
    for i = 0 to n - 1 do 
        let inner = (Array.zeroCreate m : 'T []) 
        for j = 0 to m - 1 do 
            inner.[j] <- x
        arr.[i] <- inner
    arr

//let create_matrix n m x = createJaggedMatrix n m x

//
let isEmpty array =
    Array.isEmpty array

//
let zero_create n =
    Array.zeroCreate n 

//
[<CompilerMessage("This construct is for ML compatibility. This F# library function has been renamed. Use 'fold' instead. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden = true)>]
let fold_left f z array =
    Array.fold f z array

//
[<CompilerMessage("This construct is for ML compatibility. This F# library function has been renamed. Use 'foldBack' instead. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden = true)>]
let fold_right f array z =
    Array.foldBack f array z

//
[<CompilerMessage("This construct is for ML compatibility. This F# library function has been renamed. Use 'forall' instead. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden = true)>]
let for_all f array =
    Array.forall f array

//
[<CompilerMessage("This construct is for ML compatibility. This F# library function has been renamed. Use 'unzip' instead. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden = true)>]
let split array =
    Array.unzip array

//
[<CompilerMessage("This construct is for ML compatibility. This F# library function has been renamed. Use 'zip' instead. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden = true)>]
let combine array1 array2 =
    Array.zip array1 array2

//
[<CompilerMessage("This construct is for ML compatibility. This F# library function has been renamed. Use 'create' instead. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden = true)>]
let make (n : int) (x : 'T) =
    Array.create n x

//
[<CompilerMessage("This construct is for ML compatibility. The F# library name for this function is now 'Array.toList'. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden = true)>]
let to_list array =
    Array.toList array

//
[<CompilerMessage("This construct is for ML compatibility. The F# library name for this function is now 'Array.ofList'. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden = true)>]
let of_list list =
    Array.ofList list

