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
// http://caml.inria.fr/pub/docs/manual-ocaml/libref/Stack.html

/// Last-in first-out stacks.
/// This module implements stacks (LIFOs), with in-place modification.
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharp.Compatibility.OCaml.Stack

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

