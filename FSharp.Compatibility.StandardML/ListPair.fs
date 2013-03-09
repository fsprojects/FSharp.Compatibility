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
// http://www.standardml.org/Basis/list-pair.html

/// <summary>
/// The ListPair structure provides operations on pairs of lists.
/// </summary>
/// <remarks>
/// <para>The operations fall into two categories.
/// Those in the first category, whose names do not
/// end in "Eq", do not require that the lists have the same length. When the lists are of
/// uneven lengths, the excess elements from the tail of the longer list are ignored.
/// The operations in the second category, whose names have the suffix "Eq", differ from their
/// similarly named operations in the first category only when the list arguments have
/// unequal lengths, in which case they typically raise the UnequalLengths exception.</para>
///
/// <para>Note that a function requiring equal length arguments should determine this lazily,
/// i.e., it should act as though the lists have equal length and invoke the user-supplied
/// function argument, but raise the exception if it arrives at the end of one list before
/// the end of the other.</para>
/// </remarks>
module FSharp.Compatibility.StandardML.ListPair

/// This exception is raised by those functions that require arguments of identical length.
exception UnequalLengths

/// These functions combine the two lists l1 and l2 into a list of pairs, with the first
/// element of each list comprising the first element of the result, the second elements
/// comprising the second element of the result, and so on.
/// If the lists are of unequal lengths, zip ignores the excess elements from the tail
/// of the longer one, while zipEq raises the exception UnequalLengths.
let zip (l1, l2) =
    raise <| System.NotImplementedException "ListPair.zip"

/// These functions combine the two lists l1 and l2 into a list of pairs, with the first
/// element of each list comprising the first element of the result, the second elements
/// comprising the second element of the result, and so on.
/// If the lists are of unequal lengths, zip ignores the excess elements from the tail
/// of the longer one, while zipEq raises the exception UnequalLengths.
let zipEq (l1, l2) =
    raise <| System.NotImplementedException "ListPair.zipEq"

/// returns a pair of lists formed by splitting the elements of l.
/// This is the inverse of zip for equal length lists. 
let unzip (l : ('a * 'b) list) : 'a list * 'b list =
    raise <| System.NotImplementedException "ListPair.unzip"

/// <summary>
/// These apply the function f to the list of pairs of elements generated from left to right
/// from the lists l1 and l2. If the lists are of unequal lengths, the former ignores the excess
/// elements from the tail of the longer one, and the latter raises UnequalLengths.
/// </summary>
/// <remarks>
/// The above expressions are respectively equivalent to:
/// <c>List.app f (zip (l1, l2))</c>
/// <c>List.app f (zipEq (l1, l2))</c>
/// ignoring possible side-effects of the function f.
/// </remarks>
let app (f : 'a * 'b -> unit) (l1, l2) : unit =
    raise <| System.NotImplementedException "ListPair.app"

//
let appEq (f : 'a * 'b -> unit) (l1, l2) : unit =
    raise <| System.NotImplementedException "ListPair.appEq"

/// <summary>
/// These map the function f over the list of pairs of elements generated from left to
/// right from the lists l1 and l2, returning the list of results.
/// If the lists are of unequal lengths, the former ignores the excess elements from the
/// tail of the longer one, and the latter raises UnequalLengths.
/// </summary>
/// <remarks>
/// The above expressions are respectively equivalent to:
/// <c>List.map f (zip (l1, l2))</c>
/// <c>List.map f (zipEq (l1, l2))</c>
/// ignoring possible side-effects of the function f.
/// </remarks>
let map (f : 'a * 'b -> 'c) (l1, l2) : 'c list =
    raise <| System.NotImplementedException "ListPair.map"

//
let mapEq (f : 'a * 'b -> 'c) (l1, l2) : 'c list =
    raise <| System.NotImplementedException "ListPair.mapEq"

//
let foldl (f : 'a * 'b * 'c -> 'c) init (l1, l2) : 'c =
    raise <| System.NotImplementedException "ListPair.foldl"

//
let foldr (f : 'a * 'b * 'c -> 'c) init (l1, l2) : 'c =
    raise <| System.NotImplementedException "ListPair.foldr"

//
let foldlEq (f : 'a * 'b * 'c -> 'c) init (l1, l2) : 'c =
    raise <| System.NotImplementedException "ListPair.foldlEq"

//
let foldrEq (f : 'a * 'b * 'c -> 'c) init (l1, l2) : 'c =
    raise <| System.NotImplementedException "ListPair.foldrEq"

/// <summary>
/// These functions provide short-circuit testing of a predicate over a pair of lists.
/// </summary>
/// <remarks>
/// They are respectively equivalent to:
/// <c>List.all f (zip (l1, l2))</c>
/// <c>List.exists f (zip (l1, l2))</c>
/// </remarks>
let all (f : 'a * 'b -> bool) (l1, l2) : bool =
    raise <| System.NotImplementedException "ListPair.all"

//
let exists (f : 'a * 'b -> bool) (l1, l2) : bool =
    raise <| System.NotImplementedException "ListPair.exists"

/// <summary>
/// returns true if l1 and l2 have equal length and all pairs of elements satisfy the predicate f.
/// That is, the expression is equivalent to:
/// <c>(List.length l1 = List.length l2) andalso (List.all f (zip (l1, l2)))</c>
/// </summary>
/// <remarks>
/// This function does not appear to have any nice algebraic relation with the other functions,
/// but it is included as providing a useful notion of equality, analogous to the notion of
/// equality of lists over equality types.
/// </remarks>
let allEq (f : 'a * 'b -> bool) (l1, l2) : bool =
    raise <| System.NotImplementedException "ListPair.allEq"
