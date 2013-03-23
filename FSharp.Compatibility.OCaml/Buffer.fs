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

/// Extensible string buffers.
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharp.Compatibility.OCaml.Buffer

// use of ocaml compat functions from Pervasives
#nowarn "62"

(*

type t = System.Text.StringBuilder

let create (n:int) = new System.Text.StringBuilder(n)
    
let contents (t:t) = t.ToString()
let length (t:t) = t.Length
let clear (t:t) = ignore (t.Remove(0,length t))
let reset (t:t) = ignore (t.Remove(0,length t))
let add_char (t:t) (c:char) = ignore (t.Append(c))
let add_string (t:t) (s:string) = ignore (t.Append(s))
let add_substring (t:t) (s:string) n m = ignore (t.Append(s.[n..n+m-1]))
let add_buffer (t:t) (t2:t) = ignore (t.Append(t2.ToString()))

#if FX_NO_ASCII_ENCODING
#else
let add_channel (t:t) (c:in_channel) n = 
  let b = Array.zeroCreate n  in 
  really_input c b 0 n;
  ignore (t.Append(System.Text.Encoding.ASCII.GetString(b, 0, n)))
#endif

let output_buffer (os:out_channel) (t:t) = 
  output_string os (t.ToString())

*)

