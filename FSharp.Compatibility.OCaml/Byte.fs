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

//
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharp.Compatibility.OCaml.Byte

let compare (x:byte) y = compare x y

let zero = 0uy
let one = 1uy
let add (x:byte) (y:byte) = x + y
let sub (x:byte) (y:byte) = x - y
let mul (x:byte) (y:byte) = x * y
let div (x:byte) (y:byte) = x / y
let rem (x:byte) (y:byte) = x % y
let succ (x:byte) = x + 1uy
let pred (x:byte) = x - 1uy
let max_int = 0xFFuy
let min_int = 0uy
let logand (x:byte) (y:byte) = x &&& y
let logor (x:byte) (y:byte) = x ||| y
let logxor (x:byte) (y:byte) = x ^^^ y
let lognot (x:byte) = ~~~x
let shift_left (x:byte) (n:int) =  x <<< n
let shift_right (x:byte) (n:int) =  x >>> n
let of_int (n:int) =  byte n
let to_int (x:byte) = int x
let of_char (n:char) =  byte n
let to_char (x:byte) = char x
let of_string (s:string) = byte (int32 s)
      
let to_string (x:byte) =  (box x).ToString()

let of_int32 (n:int32) = byte n
let to_int32 (x:uint8) = int32 x

let of_uint16 (n:uint16) = byte n
let to_uint16 (x:uint8)  = uint16 x

let of_uint32 (n:uint32) = byte n
let to_uint32 (x:uint8)  = uint32 x

