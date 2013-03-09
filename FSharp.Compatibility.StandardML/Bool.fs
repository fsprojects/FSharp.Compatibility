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
// http://www.standardml.org/Basis/bool.html

/// The StringCvt structure provides types and functions for handling the
/// conversion between strings and values of various basic types. 
module FSharp.Compatibility.StandardML.Bool

type bool = System.Boolean

/// returns the logical negation of the boolean value b.
let inline not b =
    not b

/// returns the string representation of b, either "true" or "false".
let inline toString b =
    if b then "true" else "false"

//
let scan (getc : (char, 'a) StringCvt.reader) strm =
    raise <| System.NotImplementedException "Bool.scan"

//
let fromString (s : string) : bool option =
    raise <| System.NotImplementedException "Bool.fromString"


(*

val scan       : (char, 'a) StringCvt.reader
                   -> (bool, 'a) StringCvt.reader

*)
