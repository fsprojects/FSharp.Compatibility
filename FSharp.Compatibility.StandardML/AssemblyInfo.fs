(*
Copyright (c) 2012, Jack Pappas
All rights reserved.

This code is provided under the terms of the 2-clause ("Simplified") BSD license.
See LICENSE.TXT for licensing details.
*)

module internal AssemblyInfo

open System
open System.Diagnostics.CodeAnalysis
open System.Reflection
open System.Runtime.CompilerServices
open System.Runtime.InteropServices
open System.Security.Permissions


[<assembly: AutoOpen("FSharp.Compatibility.StandardML")>]
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

do ()