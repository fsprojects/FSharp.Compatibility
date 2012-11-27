(*  OCaml Compatibility Library for F#
    (FSharp.Compatibility.OCaml)

    Copyright (c) Microsoft Corporation 2005-2009
    Copyright (c) Jack Pappas 2012
        http://github.com/jack-pappas

    This code is distributed under the terms of the Apache 2.0 license.
    See the LICENSE file for details. *)

module internal AssemblyInfo

open System
open System.Diagnostics.CodeAnalysis
open System.Reflection
open System.Runtime.CompilerServices
open System.Runtime.InteropServices
open System.Security.Permissions


//[<assembly: AutoOpen("FSharp.Compatibility.OCaml")>]
[<assembly: CLSCompliant(true)>]

#if FX_NO_SECURITY_PERMISSIONS
#else
#if FX_SIMPLE_SECURITY_PERMISSIONS
[<assembly: SecurityPermission(SecurityAction.RequestMinimum)>]
#else
#endif
#endif

#if FX_NO_DEFAULT_DEPENDENCY_TYPE
#else
[<assembly: Dependency("FSharp.Core", LoadHint.Always)>] 
#endif

[<assembly: SuppressMessage(
    "Microsoft.Globalization",
    "CA1305:SpecifyIFormatProvider",
    Scope = "member",
    Target = "Internal.Utilities.Pervasives+OutChannelImpl.#.ctor(Internal.Utilities.Pervasives+writer)",
    MessageId = "System.IO.TextWriter.#ctor")>]

do ()