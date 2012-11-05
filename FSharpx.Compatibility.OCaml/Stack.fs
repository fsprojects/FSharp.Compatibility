(*  OCaml Compatibility Library for F#
    (FSharpx.Compatibility.OCaml)

    Copyright (c) Microsoft Corporation 2005-2009
    Copyright (c) Jack Pappas 2012
        http://github.com/jack-pappas

    This code is distributed under the terms of the Apache 2.0 license.
    See the LICENSE file for details. *)

// References:
// http://caml.inria.fr/pub/docs/manual-ocaml/libref/Stack.html

/// Last-in first-out stacks.
/// This module implements stacks (LIFOs), with in-place modification.
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharpx.Compatibility.OCaml.Stack

/// The type of stacks containing elements of type 'a.
type t<'a> = System.Collections.Generic.Stack<'a>

/// Raised when Stack.pop or Stack.top is applied to an empty stack.
exception Empty

/// Return a new stack, initially empty.
let inline create () : t<'a> =
    System.Collections.Generic.Stack<_> ()

/// push x s adds the element x at the top of stack s.
let inline push (x : 'a) (s : t<'a>) : unit =
    s.Push x

/// pop s removes and returns the topmost element in stack s, or raises Empty if the stack is empty.
let pop (s : t<'a>) : 'a =
    if s.Count = 0 then
        raise Empty
    else
        s.Pop ()

/// top s returns the topmost element in stack s, or raises Empty if the stack is empty.
let top (s : t<'a>) : 'a =
    if s.Count = 0 then
        raise Empty
    else
        s.Peek ()

/// Discard all elements from a stack.
let inline clear (s : t<'a>) : unit =
    s.Clear ()

/// Return a copy of the given stack.
let copy (s : t<'a>) : t<'a> =
    // TODO : Make sure the items are ordered the same way in the copy
    // as they are in the original stack.
    System.Collections.Generic.Stack (s)

/// Return true if the given stack is empty, false otherwise.
let inline is_empty (s : t<'a>) : bool =
    s.Count = 0

/// Return the number of elements in a stack.
let inline length (s : t<'a>) : int =
    s.Count

/// iter f s applies f in turn to all elements of s, from the element at the top of the stack
/// to the element at the bottom of the stack. The stack itself is unchanged.
let iter (f : 'a -> unit) (s : t<'a>) : unit =
    // TODO : Check to make sure the items are processed in the expected order.
    Seq.iter f s

