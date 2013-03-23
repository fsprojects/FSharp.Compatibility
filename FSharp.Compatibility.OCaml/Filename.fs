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

/// Operations on file names.
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharp.Compatibility.OCaml.Filename

open System.IO

let current_dir_name = "."
let parent_dir_name =  ".."
let concat (x:string) (y:string) = Path.Combine (x,y)
let is_relative (s:string) = not (Path.IsPathRooted(s))

// Case sensitive (original behaviour preserved).
let check_suffix (x:string) (y:string) = x.EndsWith(y,System.StringComparison.Ordinal) 

let chop_suffix (x:string) (y:string) =
    if not (check_suffix x y) then 
        raise (System.ArgumentException("chop_suffix")) // message has to be precisely this, for OCaml compatibility, and no argument name can be set
    x.[0..x.Length-y.Length-1]

let has_extension (s:string) = 
    (s.Length >= 1 && s.[s.Length - 1] = '.' && s <> ".." && s <> ".") 
    || Path.HasExtension(s)

let chop_extension (s:string) =
    if s = "." then "" else // for OCaml compatibility
    if not (has_extension s) then 
        raise (System.ArgumentException("chop_extension")) // message has to be precisely this, for OCaml compatibility, and no argument name can be set
    Path.Combine (Path.GetDirectoryName s,Path.GetFileNameWithoutExtension(s))


let basename (s:string) = 
    Path.GetFileName(s)

let dirname (s:string) = 
    if s = "" then "."
    else 
      match Path.GetDirectoryName(s) with 
      | null -> if Path.IsPathRooted(s) then s else "."
      | res -> if res = "" then "." else res

let is_implicit (s:string) = 
    is_relative s &&
      match Path.GetDirectoryName(s) with 
      | null -> true
      | res -> (res <> current_dir_name && res <> parent_dir_name)


let temp_file (_p:string) (_s:string) = Path.GetTempFileName()

let quote s = "\'" ^ s ^ "\'"

