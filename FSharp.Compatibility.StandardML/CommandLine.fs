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

// Reference:
// http://www.standardml.org/Basis/command-line.html

/// <summary>
/// The CommandLine structure provides access to the name and arguments used
/// to invoke the currently running program.
/// </summary>
/// <remarks>
/// The precise semantics of the above operations are operating system and implementation-specific.
/// For example, name might return a full pathname or just the base name.
/// </remarks>
module FSharp.Compatibility.StandardML.CommandLine

/// The name used to invoke the current program.
let name () : string =
    System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName

/// <summary>
/// The argument list used to invoke the current program.
/// </summary>
/// <remarks>
/// The arguments returned may be only a subset of the arguments actually supplied by the user,
/// since an implementation's runtime system may consume some of them.
/// </remarks>
let arguments () : string list =
    System.Environment.GetCommandLineArgs ()
    |> Array.toList
