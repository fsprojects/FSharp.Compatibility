(*  OCaml Compatibility Library for F#
    (FSharpx.Compatibility.OCaml)

    Copyright (c) 2012 Jack Pappas (github.com/jack-pappas)

    This code is available under the Apache 2.0 license.
    See the LICENSE file for the complete text of the license. *)

// Reference:
// http://caml.inria.fr/pub/docs/manual-ocaml/libref/Array.html

//
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharpx.Compatibility.OCaml.Array

open System


//
let make_matrix (dimx : int) (dimy : int) (e : 'a) : 'a[][] =
    // Preconditions
    // TODO (make sure they match the original OCaml preconditions)

    Array.init dimx <| fun _ ->
    Array.init dimy <| fun _ ->
        e

//
[<Obsolete("Deprecated. Use 'make_matrix' instead.")>]
let inline create_matrix (dimx : int) (dimy : int) (e : 'a) =
    make_matrix dimx dimy e

