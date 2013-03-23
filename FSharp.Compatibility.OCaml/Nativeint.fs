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

// References:
// http://caml.inria.fr/pub/docs/manual-ocaml/libref/Nativeint.html

/// Processor-native integers.
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharp.Compatibility.OCaml.Nativeint

let zero = 0n
let one = 1n
let inline add (x : nativeint) (y : nativeint) = x + y
let inline sub (x : nativeint) (y : nativeint) = x - y
let inline mul (x : nativeint) (y : nativeint) = x * y
let inline div (x : nativeint) (y : nativeint) = x / y
let inline rem (x : nativeint) (y : nativeint) = x % y
let inline succ (x : nativeint) = x + 1n
let inline pred (x : nativeint) = x - 1n
let inline abs (x : nativeint) = abs x
let size = sizeof<nativeint> * 8

// TODO : These need to be fixed so they're correct.
let max_int = 0n - 1n
let min_int = 0x0UL

let logand (x : nativeint) (y : nativeint) = x &&& y
let logor (x : nativeint) (y : nativeint) = x ||| y
let logxor (x : nativeint) (y : nativeint) = x ^^^ y
let lognot (x : nativeint) = ~~~x
let shift_left (x : nativeint) (n : int) = x <<< n
let shift_right (x : nativeint) (n : int) = x >>> n
let shift_right_logical (x : nativeint) (n : int) =
    nativeint <| (unativeint x) >>> n
let of_int (n : int) = nativeint n
let to_int (x : nativeint) = int x
let of_float (f : float) = nativeint f
let to_float (x : nativeint) = float x

//let of_string (s : string) = try nativeint s with _ -> failwith "Nativeint.of_string"
let to_string (x : nativeint) = x.ToString ()

type t = nativeint
let inline compare (x : nativeint) (y : nativeint) =
    compare x y


