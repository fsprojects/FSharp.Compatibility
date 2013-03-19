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

/// Types, functions, and values defined in the Standard ML top-level.
[<AutoOpen>]
module FSharp.Compatibility.StandardML.TopLevel

let inline str (c : char) =
    c.ToString ()

/// returns the number of elements in the list l.
let inline length l =
    List.length l

/// returns |s|, the number of characters in string s.
let inline size (s : string) =
    String.length s

/// The type option provides a distinction between some value and no value,
/// and is often used for representing the result of partially defined functions.
/// It can be viewed as a typed version of the C convention of returning
/// a NULL pointer to indicate no value.
type 'a option = 'a Option.option


