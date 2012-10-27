(*  OCaml Compatibility Library for F#
    (FSharpx.Compatibility.OCaml)

    Copyright (c) 2012 Jack Pappas (github.com/jack-pappas)

    This code is available under the Apache 2.0 license.
    See the LICENSE file for the complete text of the license. *)

// References:
// http://caml.inria.fr/pub/docs/manual-ocaml/libref/Pervasives.html

//
[<AutoOpen>]
module FSharpx.Compatibility.OCaml.Pervasives

open System

(*** Floating-point arithmetic ***)

/// The five classes of floating-point numbers, as determined by
/// the <see cref="classify_float"/> function.
type fpclass =
    /// Normal number, none of the below
    | FP_normal
    /// Number very close to 0.0, has reduced precision
    | FP_subnormal
    /// Number is 0.0 or -0.0
    | FP_zero
    /// Number is positive or negative infinity
    | FP_infinite
    /// Not a number: result of an undefined operation
    | FP_nan

/// Return the class of the given floating-point number:
/// normal, subnormal, zero, infinite, or not a number.
let classify_float (value : float) : fpclass =
    raise <| System.NotImplementedException "classify_float"


(*** Operations on format strings ***)

//
type format4<'a, 'b, 'c, 'd> = format6<'a, 'b, 'c, 'c, 'c, 'd>

//
type format<'a, 'b, 'c> = format4<'a, 'b, 'c, 'c>


(*** Program termination ***)

/// Contains internal, mutable state representing actions to be performed
/// upon program termination (either normally or because of an uncaught exception).
module private ExitCallbacks =
    open System.Threading

    /// Actions to be executed when the program is exiting.
    let mutable private exitFunctions : (unit -> unit) list = List.empty

    /// When set to a non-zero value, indicates the exit functions
    /// have been executed (or are currently executing).
    let mutable private exitFunctionsExecutedFlag = 0

    /// Executes the exit functions if any have been registered and if
    /// they have not already been executed.
    let private executeExitFunctionsIfNecessary _ =
        if Interlocked.CompareExchange (&exitFunctionsExecutedFlag, 1, 0) = 0 then
            // Run the exit functions in last-in-first-out order.
            exitFunctions
            |> List.iter (fun f -> f ())

    // Register handlers for events which fire when the process exits cleanly
    // or due to an exception being thrown.
    do
        AppDomain.CurrentDomain.ProcessExit.Add executeExitFunctionsIfNecessary
        AppDomain.CurrentDomain.UnhandledException.Add executeExitFunctionsIfNecessary

    //
    let (*rec*) at_exit f =
        // TODO : Before adding the function to the list, check the value of
        // exitFunctionsExecutedFlag; if the functions are already executing,
        // we'll either raise an exception, or perhaps invoke the function
        // right here instead of adding it to the list.
        
        // TODO (IMPORTANT) : This function needs to be re-written using
        // atomic operations so it's thread-safe. When rewriting this,
        // it may be easier if we make this function recursive rather than
        // using an imperative loop.
        exitFunctions <- f :: exitFunctions


/// <summary>Terminate the process, returning the given status code to the operating system:
/// usually 0 to indicate no errors, and a small positive integer to indicate failure.</summary>
/// <remarks><para>All open output channels are flushed with <see cref="flush_all"/>.</para>
/// <para>An implicit <c>exit 0</c> is performed each time a program terminates
/// normally. An implicit <c>exit 2</c> is performed if the program terminates
/// early because of an uncaught exception.</para></remarks>
let exit (exitCode : int) =
    raise <| System.NotImplementedException ()

/// <summary>Register the given function to be called at program termination time.</summary>
/// <remarks><para>The functions registered with <see cref="at_exit"/> will be called when
/// the program executes <see cref="exit"/>, or terminates, either normally or
/// because of an uncaught exception.</para>
/// <para>The functions are called in "last in, first out" order: the function most
/// recently added with <see cref="at_exit"/> is called first.</para></remarks>
let at_exit (f : unit -> unit) : unit =
    ExitCallbacks.at_exit f

