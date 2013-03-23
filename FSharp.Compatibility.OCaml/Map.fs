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

/// Association tables over ordered types.
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharp.Compatibility.OCaml.Map

open System.Collections.Generic


type TaggedMap<'Key, 'Value, 'Tag when 'Tag :> IComparer<'Key>> =
    Microsoft.FSharp.Collections.Tagged.Map<'Key, 'Value, 'Tag>
type TaggedMap<'Key, 'Value> =
    Microsoft.FSharp.Collections.Tagged.Map<'Key, 'Value>

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
type Provider<'Key, 'T, 'Tag> when 'Tag :> IComparer<'Key> =
    interface
        abstract empty: TaggedMap<'Key,'T,'Tag>;
        abstract add: 'Key -> 'T -> TaggedMap<'Key,'T,'Tag> -> TaggedMap<'Key,'T,'Tag>;
        abstract find: 'Key -> TaggedMap<'Key,'T,'Tag> -> 'T;
        abstract first: ('Key -> 'T -> 'U option) -> TaggedMap<'Key,'T,'Tag> -> 'U option;
        abstract tryfind: 'Key -> TaggedMap<'Key,'T,'Tag> -> 'T option;
        abstract remove: 'Key -> TaggedMap<'Key,'T,'Tag> -> TaggedMap<'Key,'T,'Tag>;
        abstract mem: 'Key -> TaggedMap<'Key,'T,'Tag> -> bool;
        abstract iter: ('Key -> 'T -> unit) -> TaggedMap<'Key,'T,'Tag> -> unit;
        abstract map:  ('T -> 'U) -> TaggedMap<'Key,'T,'Tag> -> TaggedMap<'Key,'U,'Tag>;
        abstract mapi: ('Key -> 'T -> 'U) -> TaggedMap<'Key,'T,'Tag> -> TaggedMap<'Key,'U,'Tag>;
        abstract fold: ('Key -> 'T -> 'State -> 'State) -> TaggedMap<'Key,'T,'Tag> -> 'State -> 'State
    end

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
type Provider<'Key, 'Value> = Provider<'Key, 'Value, IComparer<'Key>>

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let MakeTagged (cf : 'Tag) : Provider<'Key, 'Value, 'Tag> when 'Tag :> IComparer<'Key> =
    // OPTIMIZE : Re-implement this as a generalized, static, generic value
    // so only one instance will be created for each combination of ('Key, 'Value, 'Tag).
    { new Provider<_,_,_> with 
            member p.empty = TaggedMap<_,_,_>.Empty(cf);
            member p.add k v m  = m.Add(k,v);
            member p.find x m = m.[x] 
            member p.first f m = m.First(f)
            member p.tryfind k m = m.TryFind(k)
            member p.remove x m = m.Remove(x)
            member p.mem x m = m.ContainsKey(x)
            member p.iter f m = m.Iterate(f)
            member p.map f m = m.MapRange(f)
            member p.mapi f m = m.Map(f)
            member p.fold f m z = m.Fold f z }

//
[<CompilerMessage(
    "This construct is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
let Make cf  = MakeTagged (ComparisonIdentity.FromFunction cf)

