(*
Copyright (c) 2012, Jack Pappas
All rights reserved.

This code is provided under the terms of the 2-clause ("Simplified") BSD license.
See LICENSE.TXT for licensing details.
*)

//
module FSharp.Compatibility.StandardML.Unsafe


/// This module creates and manipulates suspensions for lazy evaluation.
module Susp =
    //
    type susp<'a> = Lazy<'a>

    //
    let force (susp : susp<'a>) =
        susp.Force ()

    //
    let delay f : susp<'a> =
        System.Lazy.Create f


