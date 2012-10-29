﻿(*  OCaml Compatibility Library for F#
    (FSharpx.Compatibility.OCaml)

    Copyright (c) Microsoft Corporation 2005-2009
    Copyright (c) Jack Pappas 2012
        http://github.com/jack-pappas

    This code is distributed under the terms of the Apache 2.0 license.
    See the LICENSE file for details. *)

/// The run-time library for parsers generated by fsyacc.
[<CompilerMessage(
    "This module is for ML compatibility. \
    This message can be disabled using '--nowarn:62' or '#nowarn \"62\"'.",
    62, IsHidden = true)>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharpx.Compatibility.OCaml.Parsing

open Microsoft.FSharp.Text.Lexing
open Microsoft.FSharp.Text.Parsing


//
exception Parse_error = RecoverableParseError

//
let private dummyProvider =
    let err _  = failwith "You must generate your parser using the '--ml-compatibility' option or call 'Parsing.set_parse_state parseState' \
                            in each action before using functions from the Parsing module.  This is because the module uses global state which \
                            must be set up for use in each parsing action. Review the notes in the 'Microsoft.FSharp.Compatibility.OCaml.Parsing' \
                            module if you are using parsers on multiple threads."
    { new IParseState with 
        member x.InputRange(i) = err();
        member p.InputStartPosition(n) = err();
        member p.InputEndPosition(n) = err();
        member x.ResultRange = err();
        member x.GetInput(i) = err();
        member x.ParserLocalStore = err();
        member x.RaiseError() = err()  
      }

//
let mutable private parse_information = dummyProvider

//
let private parse_error s =
    parse_information.RaiseError () (failwith s : unit)

//
let private set_parse_state (x : IParseState) =
    parse_information <- x

//
let private enforce_nonnull_pos p : Lexing.position =
    match box p with
    | null -> Position.Empty
    | _ -> p

//
let symbol_start_pos () =
    parse_information.ResultRange
    |> fst
    |> enforce_nonnull_pos

//
let symbol_end_pos () =
    parse_information.ResultRange
    |> snd
    |> enforce_nonnull_pos

//
let rhs_start_pos (n : int) =
    parse_information.InputRange n
    |> fst
    |> enforce_nonnull_pos

//
let rhs_end_pos (n : int) =
    parse_information.InputRange n
    |> snd
    |> enforce_nonnull_pos

//
let symbol_start () =
    (symbol_start_pos()).pos_cnum
//
let symbol_end () =
    (symbol_end_pos()).pos_cnum
//
let rhs_start n =
    (rhs_start_pos n).pos_cnum
//
let rhs_end n =
    (rhs_end_pos n).pos_cnum

/// Empty the parser stack. Call it just after a parsing function has returned to remove
/// all pointers from the parser stack to structures that were built by semantic actions
/// during parsing. This is optional, but lowers the memory requirements of the programs.
let clear_parser () =
    // TEMP : Don't do anything for now -- programs should work as-is, though they won't
    // benefit from freeing the parser structures. This won't matter anyway, once fsyacc
    // modifications are released.
    ()

////
//let set_trace (flag : bool) : bool =
//    raise <| System.NotImplementedException "Parsing.set_trace"