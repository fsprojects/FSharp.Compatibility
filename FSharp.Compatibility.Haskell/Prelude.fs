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

module FSharp.Compatibility.Haskell.Prelude

(* Type extensions for built-in .NET and F# types *)

//
let inline (++) (str1 : string) (str2 : string) =
    System.String.Concat (str1, str2)

//
let inline show x =
    x.ToString ()

// Haskell              | F#
//---------------------------------
// rem                  | %
// mod                  | ?
// quot                 | /
// div                  | ?
// divMod               | ?
// quotRem              | divRem

/// Type abbreviation for Haskell's Either type.
// NOTE : If the Haskell Either type were directly translated to use Choice`2, then Left would become
// Choice1Of2 and Right would become Choice2Of2. However, this pattern "inverts" the translation,
// because Choice1Of2 and Right normally represent a "success" value while Choice2Of2 and Left
// represent an "error" value.
type Either<'a, 'b> = Choice<'b, 'a>

/// Active pattern which simulates Either using the Choice type.
let inline (|Right|Left|) (either : Either<'a, 'b>) : Either<_,_> =
    match either with
    | Choice1Of2 x ->
        Right x
    | Choice2Of2 x ->
        Left x


