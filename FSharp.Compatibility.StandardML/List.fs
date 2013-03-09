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


