(*  OCaml Compatibility Library for F#
    (FSharpx.Compatibility.OCaml)

    Copyright (c) Microsoft Corporation 2005-2009
    Copyright (c) Jack Pappas 2012
        http://github.com/jack-pappas

    This code is distributed under the terms of the Apache 2.0 license.
    See the LICENSE file for details. *)

/// List operations.
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharpx.Compatibility.OCaml.List

open System
open System.Collections.Generic
open System.Diagnostics
open Microsoft.FSharp.Core.OptimizedClosures


let private indexNotFound () =
    raise <| KeyNotFoundException "An index satisfying the predicate was not found in the collection"

//
[<CompilerMessage("This construct is for ML compatibility. This F# library function has been renamed. Use 'fold' instead. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let fold_left f z xs = List.fold f z xs

//
[<CompilerMessage("This construct is for ML compatibility. This F# library function has been renamed. Use 'fold2' instead. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let fold_left2 f z xs1 xs2 = List.fold2 f z xs1 xs2

//
[<CompilerMessage("This construct is for ML compatibility. This F# library function has been renamed. Use 'foldBack' instead. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let fold_right f xs z = List.foldBack f xs z

//
[<CompilerMessage("This construct is for ML compatibility. This F# library function has been renamed. Use 'foldBack2' instead. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let fold_right2 f xs1 xs2 z = List.foldBack2 f xs1 xs2 z

//
[<CompilerMessage("This construct is for ML compatibility. This F# library function has been renamed. Use 'forall' instead. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let for_all f xs = List.forall f xs

//
[<CompilerMessage("This construct is for ML compatibility. This F# library function has been renamed. Use 'sortWith' instead. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let stable_sort f xs = List.sortWith f xs

//
[<CompilerMessage("This construct is for ML compatibility. This F# library function has been renamed. Use 'unzip' instead. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let split x =  List.unzip x

//
[<CompilerMessage("This construct is for ML compatibility. This F# library function has been renamed. Use 'zip' instead. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let combine x1 x2 =  List.zip x1 x2

//
[<CompilerMessage("This construct is for ML compatibility. This F# library function has been renamed. Use 'filter' instead. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let find_all f x = List.filter f x

//
[<CompilerMessage("This construct is for ML compatibility. This F# library function has been renamed. Use 'concat' instead. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let flatten (list:seq<list<_>>) = List.concat list

//
[<CompilerMessage("This construct is for ML compatibility. The F# library name for this function is now 'List.ofArray'. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let of_array (array:'T array) = List.ofArray array

//
[<CompilerMessage("This construct is for ML compatibility. The F# library name for this function is now 'List.toArray'. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let to_array (list:'T list) = List.toArray list

//
[<CompilerMessage("This construct is for ML compatibility. The F# library name for this function is now 'List.head'. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let hd list = List.head list

//
[<CompilerMessage("This construct is for ML compatibility. The F# library name for this function is now 'List.tail'. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let tl list = List.tail list

//
let for_all2 f xs1 xs2 = List.forall2 f xs1 xs2

//
[<Obsolete("This function will be removed. Use 'not List.isEmpty' instead.")>]
let nonempty x =
    match x with
    | [] -> false
    | _ -> true

//
let rec contains x l =
    match l with
    | [] ->
        false
    | h::t ->
        x = h
        || contains x t

//
let rec rev_map2_acc (f:FSharpFunc<_,_,_>) l1 l2 acc =
    match l1,l2 with 
    | [],[] -> acc
    | h1::t1, h2::t2 -> rev_map2_acc f t1 t2 (f.Invoke(h1,h2) :: acc)
    | _ -> invalidArg "l2" "the lists have different lengths"

//
[<CompilerMessage("This construct is for ML compatibility. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let rev_map2 f l1 l2 = 
    let f = FSharpFunc<_,_,_>.Adapt(f)
    rev_map2_acc f l1 l2 []

//
[<CompilerMessage("This construct is for ML compatibility. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let rec rev_append l1 l2 =
    match l1 with 
    | [] -> l2
    | h::t -> rev_append t (h::l2)

//
[<CompilerMessage("This construct is for ML compatibility. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let rec rev_map_acc f l acc =
    match l with 
    | [] -> acc
    | h::t -> rev_map_acc f t (f h :: acc)

//
let rev_map f l = rev_map_acc f l []

//
[<CompilerMessage("This construct is for ML compatibility. The F# name for this function is 'contains'. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let mem x l =
    contains x l

//
[<CompilerMessage("This construct is for ML compatibility. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let rec memq x l = 
    match l with 
    | [] -> false 
    | h::t -> LanguagePrimitives.PhysicalEquality x h || memq x t

//
[<CompilerMessage("This construct is for ML compatibility. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let rec assoc x l = 
    match l with 
    | [] -> indexNotFound()
    | ((h,r)::t) -> if x = h then r else assoc x t

//
[<CompilerMessage("This construct is for ML compatibility. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let rec try_assoc x l = 
    match l with 
    | [] -> None
    | ((h,r)::t) -> if x = h then Some(r) else try_assoc x t

//
[<CompilerMessage("This construct is for ML compatibility. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let rec mem_assoc x l = 
    match l with 
    | [] -> false
    | ((h,_)::t) -> x = h || mem_assoc x t

//
[<CompilerMessage("This construct is for ML compatibility. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let rec remove_assoc x l = 
    match l with 
    | [] -> []
    | (((h,_) as p) ::t) -> if x = h then t else p:: remove_assoc x t

//
[<CompilerMessage("This construct is for ML compatibility. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let rec assq x l = 
    match l with 
    | [] -> indexNotFound()
    | ((h,r)::t) -> if LanguagePrimitives.PhysicalEquality x h then r else assq x t

//
[<CompilerMessage("This construct is for ML compatibility. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let rec try_assq x l = 
    match l with 
    | [] -> None
    | ((h,r)::t) -> if LanguagePrimitives.PhysicalEquality x h then Some r else try_assq x t

//
[<CompilerMessage("This construct is for ML compatibility. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let rec mem_assq x l = 
    match l with 
    | [] -> false
    | ((h,_)::t) -> LanguagePrimitives.PhysicalEquality x h || mem_assq x t

//
[<CompilerMessage("This construct is for ML compatibility. This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.", 62, IsHidden=true)>]
let rec remove_assq x l = 
    match l with 
    | [] -> []
    | (((h,_) as p) ::t) ->
        if LanguagePrimitives.PhysicalEquality x h then t else p:: remove_assq x t

//
let scanReduce f l = 
    match l with 
    | [] -> invalidArg "l" "the input list is empty"
    | (h::t) -> List.scan f h t

//
let scanArraySubRight<'T,'State> (f:FSharpFunc<'T,'State,'State>) (arr:_[]) start fin initState = 
    let mutable state = initState  
    let mutable res = [state]  
    for i = fin downto start do
        state <- f.Invoke(arr.[i], state);
        res <- state :: res
    res

//
let scanReduceBack f l = 
    match l with 
    | [] -> invalidArg "l" "the input list is empty"
    | _ -> 
        let f = FSharpFunc<_,_,_>.Adapt(f)
        let arr = Array.ofList l 
        let arrn = Array.length arr 
        scanArraySubRight f arr 0 (arrn - 2) arr.[arrn - 1]

