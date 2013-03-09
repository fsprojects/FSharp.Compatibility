(*

Copyright 2013 Jack Pappas

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

// Reference:
// http://www.standardml.org/Basis/list.html

/// <summary>
/// The List structure provides a collection of utility functions for manipulating
/// polymorphic lists, traditionally an important datatype in functional programming.
/// </summary>
/// <remarks>
/// Following the concrete syntax provided by the list :: operator, the head of a list
/// appears leftmost. Thus, a traversal of a list from left to right starts with the head,
/// then recurses on the tail. In addition, as a sequence type, a list has an indexing
/// of its elements, with the head having index 0, the second element having index 1, etc.
/// </remarks>
module FSharp.Compatibility.StandardML.List

open Option


type list<'a> = Microsoft.FSharp.Collections.list<'a>

(* NOTE :   We reuse the built-in F# list type instead of re-defining it.
            For ML compatibility, a constant and an active pattern are provided
            for creating and matching the 'nil' pattern. *)

let nil = []

let (|Nil|_|) lst =
    match lst with
    | [] -> Some ()
    | _ -> None

/// This exception indicates that an empty list was given as an argument
/// to a function requiring a non-empty list.
exception Empty

/// returns true if the list l is empty.
let inline ``null`` l =
    List.isEmpty l

/// returns the number of elements in the list l.
let inline length l =
    List.length l

///// returns the list that is the concatenation of l1 and l2.
//let inline (@) (l1, l2) =
//    l1 @ l2

/// returns the first element of l. It raises Empty if l is nil.
let hd l =
    match l with
    | [] ->
        raise Empty
    | hd :: _ ->
        hd

/// returns all but the first element of l. It raises Empty if l is nil.
let tl l =
    match l with
    | [] ->
        raise Empty
    | _ :: tl ->
        tl

/// returns the last element of l. It raises Empty if l is nil.
let last l =
    match l with
    | [] ->
        raise Empty
    | hd :: tl ->
        let rec lastRec (el, l) =
            match l with
            | [] -> el
            | hd :: tl ->
                lastRec (hd, tl)

        lastRec (hd, tl)

/// <summary>
/// returns <c>NONE</c> if the list is empty, and <c>SOME(hd l,tl l)</c> otherwise.
/// This function is particularly useful for creating value readers from lists of characters.
/// </summary>
/// <remarks>
/// For example, <c>Int.scan StringCvt.DEC getItem</c> has the type
/// <c>(int,char list) StringCvt.reader</c>
/// and can be used to scan decimal integers from lists of characters.
/// </remarks>
let getItem l =
    match l with
    | [] -> NONE
    | hd :: tl ->
        SOME (hd, tl)

/// returns the i(th) element of the list l, counting from 0.
/// It raises Subscript if i < 0 or i >= length l. We have nth(l,0) = hd l, ignoring exceptions. 
let nth (l, i) =
    raise <| System.NotImplementedException "List.nth"

/// returns the first i elements of the list l. It raises Subscript if i < 0 or i > length l. We have take(l, length l) = l.
let take (l, i) =
    raise <| System.NotImplementedException "List.take"

/// returns what is left after dropping the first i elements of the list l. It raises Subscript if i < 0 or i > length l.
/// It holds that take(l, i) @ drop(l, i) = l when 0 <= i <= length l. We also have drop(l, length l) = [].
let drop (l, i) =
    raise <| System.NotImplementedException "List.drop"

/// returns a list consisting of l's elements in reverse order.
let inline rev l =
    List.rev l

/// returns the list that is the concatenation of all the lists in l in order.
/// concat[l1,l2,...ln] = l1 @ l2 @ ... @ ln
let inline concat (l : _ list list) =
    List.concat l

// returns (rev l1) @ l2.
let inline revAppend (l1, l2) =
    (rev l1) @ l2

/// applies f to the elements of l, from left to right.
let inline app f l =
    List.iter f l

/// applies f to each element of l from left to right, returning the list of results.
let inline map f l =
    List.map f l

/// <summary>
/// applies f to each element of l from left to right, returning a list of results,
/// with SOME stripped, where f was defined. f is not defined for an element of l if
/// f applied to the element returns NONE.
/// </summary>
/// <remarks>
/// The above expression is equivalent to:
/// <c>((map valOf) o (filter isSome) o (map f)) l</c>
/// </remarks>
let mapPartial f l =
    ([], l)
    ||> List.fold (fun results el ->
        match f el with
        | NONE ->
            results
        | SOME v ->
            v :: results)
    |> List.rev

/// applies f to each element x of the list l, from left to right, until f x evaluates to true.
/// It returns SOME(x) if such an x exists; otherwise it returns NONE.
let find f l =
    match List.tryFind f l with
    | None -> NONE
    | Some v -> SOME v

/// applies f to each element x of l, from left to right, and returns the list of those x
/// for which f x evaluated to true, in the same order as they occurred in the argument list.
let inline filter f l =
    List.filter f l

/// applies f to each element x of l, from left to right, and returns a pair (pos, neg)
/// where pos is the list of those x for which f x evaluated to true, and neg is the list
/// of those for which f x evaluated to false. The elements of pos and neg retain the
/// same relative order they possessed in l. 
let inline partition f l =
    List.partition f l

//
let foldl (f : 'a * 'b -> 'b) init l =
    raise <| System.NotImplementedException "List.foldl"

//
let foldr (f : 'a * 'b -> 'b) init l =
    raise <| System.NotImplementedException "List.foldr"

/// applies f to each element x of the list l, from left to right, until f x evaluates to true;
/// it returns true if such an x exists and false otherwise.
let inline exists f l =
    List.exists f l

/// applies f to each element x of the list l, from left to right, until f x evaluates to false;
/// it returns false if such an x exists and true otherwise.
/// It is equivalent to not(exists (not o f) l)).
let inline all f l =
    List.forall f l

/// returns a list of length n equal to [f(0), f(1), ..., f(n-1)], created from left to right.
/// It raises Size if n < 0. 
let tabulate (n, f : int -> 'a) =
    raise <| System.NotImplementedException "List.tabulate"

/// performs lexicographic comparison of the two lists using the given ordering f on the list elements.
let collate (f : 'a * 'a -> order) (l1, l2) : order =
    raise <| System.NotImplementedException "List.collate"
