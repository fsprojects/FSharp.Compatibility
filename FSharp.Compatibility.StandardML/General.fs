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
// http://www.standardml.org/Basis/general.html

/// <summary>
/// The structure General defines exceptions, datatypes, and functions which are used throughout
/// the SML Basis Library, and are useful in a wide range of programs.
/// </summary>
/// <remarks>
/// All of the types and values defined in General are available unqualified at the top-level.
/// </remarks>
[<AutoOpen>]
module FSharp.Compatibility.StandardML.General

/// The type containing a single value denoted (), which is typically used as a
/// trivial argument or as a return value for a side-effecting function. 
type unit = Microsoft.FSharp.Core.unit

/// The type of values transmitted when an exception is raised and handled.
/// This type is special in that it behaves like a datatype with an extensible set of
/// data constructors, where new constructors are created by exception declarations.
type exn = Microsoft.FSharp.Core.exn

(*  Exceptions indicating that pattern matching failed in a val binding or, respectively,
    in a case expression or function application. This occurs when the matched value is
    not an instance of any of the supplied patterns. *)
exception Bind
exception Match

/// The exception indicating an attempt to create a character with a code outside
/// the range supported by the underlying character type (see CHAR.chr).
exception Chr

/// The exception indicating an attempt to divide by zero. It replaces the Mod exception
/// required by the SML'90 Definition.
exception Div   // System.DivideByZeroException

/// The exception indicating that the argument of a mathematical function is outside
/// the domain of the function. It is raised by functions in structures matching the
/// MATH or INT_INF signatures. It replaces the Sqrt and Ln exceptions required by
/// the SML'90 Definition. 
exception Domain

/// A general-purpose exception used to signify the failure of an operation.
/// It is not raised by any function in the SML Basis Library, but is provided for use
/// by users and user-defined libraries. 
exception Fail of string

/// The exception indicating that the result of an arithmetic function is not representable,
/// in particular, is too large. It replaces the Abs, Exp, Neg, Prod, Quot, and Sum exceptions
/// required by the SML'90 Definition. 
exception Overflow

/// The exception indicating an attempt to create an aggregate data structure
/// (such as an array, string, or vector) whose size is too large or negative.
exception Size

/// The exception indicating an attempt to apply SUBSTRING.span to two incompatible substrings.
exception Span

/// The exception indicating that an index is out of range, typically arising when the program
/// is accessing an element in an aggregate data structure (such as a list, string, array, or vector).
exception Subscript     // System.IndexOutOfRangeException

/// Returns a name for the exception ex.
/// The name returned may be that of any exception constructor aliasing with ex.
// exn -> string
let exnName ex =
    raise <| System.NotImplementedException "General.exnName"

/// Returns a message corresponding to exception ex.
/// The precise format of the message may vary between implementations and locales,
/// but will at least contain the string 'exnName ex'.
// exn -> string
let exnMessage ex =
    raise <| System.NotImplementedException "General.exnMessage"

/// Values of type 'order' are used when comparing elements of a type that has a linear ordering. 
type order = LESS | EQUAL | GREATER

// is the function composition of f and g. Thus, (f o g) a is equivalent to f(g a).
// ('b -> 'c) * ('a -> 'b) -> 'a -> 'c 
//let inline ( o ) f g = f << g

// returns a. It provides a notational shorthand for evaluating a, then b, before returning the value of a.
// 'a * unit -> 'a
let inline before (a, b : unit) = a
    
/// Returns (). The purpose of ignore is to discard the result of a computation,
/// returning () instead. This is useful, for example, when a higher-order function,
/// such as List.app, requires a function returning unit, but the function to be
/// used returns values of some other type. 
let inline ignore a = ()
