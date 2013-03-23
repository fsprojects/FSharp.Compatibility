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
// http://caml.inria.fr/pub/docs/manual-ocaml/libref/Queue.html

/// First-in first-out queues.
/// This module implements queues (FIFOs), with in-place modification.
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharp.Compatibility.OCaml.Queue

/// The type of queues containing elements of type 'a.
type t<'a> = System.Collections.Generic.Queue<'a>

/// Raised when Queue.take or Queue.peek is applied to an empty queue.
exception Empty

/// Return a new queue, initially empty.
let inline create () : t<'a> =
    System.Collections.Generic.Queue<_> ()

/// add x q adds the element x at the end of the queue q.
let inline add (x : 'a) (q : t<'a>) : unit =
    q.Enqueue x

/// push is a synonym for add.
let inline push (x : 'a) (q : t<'a>) : unit =
    add x q

/// take q removes and returns the first element in queue q, or raises Empty if the queue is empty.
let take (q : t<'a>) : 'a =
    if q.Count = 0 then
        raise Empty
    else
        q.Dequeue ()

/// pop is a synonym for take.
let inline pop (q : t<'a>) : 'a =
    take q

/// peek q returns the first element in queue q, without removing it from the queue, or raises Empty if the queue is empty.
let peek (q : t<'a>) : 'a =
    if q.Count = 0 then
        raise Empty
    else
        q.Peek ()

/// top is a synonym for peek.
let inline top (q : t<'a>) : 'a =
    peek q

/// Discard all elements from a queue.
let inline clear (q : t<'a>) : unit =
    q.Clear ()

/// Return a copy of the given queue.
let copy (q : t<'a>) : t<'a> =
    raise <| System.NotImplementedException "Queue.copy"

/// Return true if the given queue is empty, false otherwise.
let inline is_empty (q : t<'a>) : bool =
    q.Count = 0

/// Return the number of elements in a queue.
let inline length (q : t<'a>) : int =
    q.Count

/// iter f q applies f in turn to all elements of q, from the least recently entered to the most recently entered. The queue itself is unchanged.
let iter (f : 'a -> unit) (q : t<'a>) : unit =
    raise <| System.NotImplementedException "Queue.iter"

/// fold f accu q is equivalent to List.fold_left f accu l, where l is the list of q's elements. The queue remains unchanged.
let fold (f : 'b -> 'a -> 'b) (accu : 'b) (q : t<'a>) =
    raise <| System.NotImplementedException "Queue.fold"

/// transfer q1 q2 adds all of q1's elements at the end of the queue q2, then clears q1.
/// It is equivalent to the sequence iter (fun x -> add x q2) q1; clear q1, but runs in constant time.
let transfer (q1 : t<'a>) (q2 : t<'a>) : unit =
    raise <| System.NotImplementedException "Queue.transfer"

