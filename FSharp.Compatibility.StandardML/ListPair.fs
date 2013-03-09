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
/// The operations fall into two categories.
/// Those in the first category, whose names do not
/// end in "Eq", do not require that the lists have the same length. When the lists are of
/// uneven lengths, the excess elements from the tail of the longer list are ignored.
/// The operations in the second category, whose names have the suffix "Eq", differ from their
/// similarly named operations in the first category only when the list arguments have
/// unequal lengths, in which case they typically raise the UnequalLengths exception.
/// </remarks>
module FSharp.Compatibility.StandardML.ListPair

/// This exception is raised by those functions that require arguments of identical length.
exception UnequalLengths

//
let zip (l1, l2) =
    raise <| System.NotImplementedException "ListPair.zip"

//
let zipEq (l1, l2) =
    raise <| System.NotImplementedException "ListPair.zipEq"

//
let unzip (l : ('a * 'b) list) : 'a list * 'b list =
    raise <| System.NotImplementedException "ListPair.unzip"

//
let app (f : 'a * 'b -> unit) (l1, l2) : unit =
    raise <| System.NotImplementedException "ListPair.app"

//
let appEq (f : 'a * 'b -> unit) (l1, l2) : unit =
    raise <| System.NotImplementedException "ListPair.appEq"

//
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

//
let all (f : 'a * 'b -> bool) (l1, l2) : bool =
    raise <| System.NotImplementedException "ListPair.all"

//
let exists (f : 'a * 'b -> bool) (l1, l2) : bool =
    raise <| System.NotImplementedException "ListPair.exists"

//
let allEq (f : 'a * 'b -> bool) (l1, l2) : bool =
    raise <| System.NotImplementedException "ListPair.allEq"
