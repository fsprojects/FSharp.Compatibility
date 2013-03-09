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
// http://www.standardml.org/Basis/option.html

/// <summary>
/// The Option structure defines the option type, used for handling partial functions
/// and optional values, and provides a collection of common combinators.
/// </summary>
/// <remarks>
/// The type, the Option exception, and the functions <c>getOpt</c>, <c>valOf</c>,
/// and <c>isSome</c> are available in the top-level environment.
/// </remarks>
module FSharp.Compatibility.StandardML.Option

/// The type option provides a distinction between some value and no value,
/// and is often used for representing the result of partially defined functions.
/// It can be viewed as a typed version of the C convention of returning
/// a NULL pointer to indicate no value.
type 'a option = NONE | SOME of 'a

//
exception Option

/// <summary>
/// returns v if opt is <c>SOME(v)</c>; otherwise it returns a.
/// </summary>
let getOpt (opt, a) =
    match opt with
    | NONE -> a
    | SOME v -> v

/// <summary>
/// returns true if opt is <c>SOME(v)</c>; otherwise it returns false.
/// </summary>
let isSome = function
    | NONE -> false
    | SOME _ -> true

/// <summary>
/// returns v if opt is <c>SOME(v)</c>; otherwise it raises the Option exception.
/// </summary>
let valOf = function
    | SOME v -> v
    | NONE ->
        raise Option

/// <summary>
/// returns <c>SOME(a)</c> if <c>f(a)</c> is true and <c>NONE</c> otherwise.
/// </summary>
let filter (f : 'a -> bool) a =
    if f a then SOME a else NONE

/// <summary>
/// The join function maps <c>NONE</c> to <c>NONE</c> and <c>SOME(v)</c> to v.
/// </summary>
let join = function
    | NONE
    | SOME NONE ->
        NONE
    | SOME (SOME v) ->
        SOME v

/// <summary>
/// applies the function f to the value v if opt is <c>SOME(v)</c>, and otherwise does nothing.
/// </summary>
let app f opt : unit =
    match opt with
    | NONE -> ()
    | SOME v ->
        f v
    
/// <summary>
/// maps <c>NONE</c> to <c>NONE</c> and <c>SOME(v)</c> to <c>SOME(f v)</c>.
/// </summary>
let map f opt =
    match opt with
    | NONE -> NONE
    | SOME v ->
        SOME (f v)

/// <summary>
/// maps <c>NONE</c> to <c>NONE</c> and <c>SOME(v)</c> to <c>f(v)</c>.
/// </summary>
/// <remarks>
/// The expression <c>mapPartial f</c> is equivalent to <c>join &lt;&lt; (map f)</c>.
/// </remarks>
let mapPartial f opt =
    match opt with
    | NONE -> NONE
    | SOME v ->
        f v

/// <summary>
/// returns <c>NONE</c> if <c>g(a)</c> is <c>NONE</c>;
/// otherwise, if <c>g(a)</c> is <c>SOME(v)</c>, it returns <c>SOME(f v)</c>.
/// </summary>
/// <remarks>
/// Thus, the compose function composes f with the partial function g to produce another
/// partial function. The expression <c>compose (f, g)</c> is equivalent to <c>(map f) &lt;&lt; g</c>.
/// </remarks>
let compose (f, g) a =
    match g a with
    | NONE -> NONE
    | SOME v ->
        SOME (f v)

/// <summary>
/// returns <c>NONE</c> if <c>g(a)</c> is <c>NONE</c>;
/// otherwise, if <c>g(a)</c> is <c>SOME(v)</c>, it returns <c>f(v)</c>.
/// </summary>
/// <remarks>
/// Thus, the composePartial function composes the two partial functions f and g to produce another
/// partial function. The expression <c>composePartial (f, g)</c> is equivalent to <c>(mapPartial f) &lt;&lt; g</c>.
/// </remarks>
let composePartial (f, g) a =
    match g a with
    | NONE -> NONE
    | SOME v ->
        f v
