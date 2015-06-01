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

// References:
// http://caml.inria.fr/pub/docs/manual-ocaml/libref/Pervasives.html

/// The initially opened module.
[<AutoOpen>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharp.Compatibility.OCaml.Pervasives

open System
open System.Collections.Generic
open System.IO
open System.Text

#nowarn "62" // compatibility warnings
#nowarn "35"  // 'deprecated' warning about redefining '<' etc.
#nowarn "86"  // 'deprecated' warning about redefining '<' etc.
#nowarn "60"  // override implementations in intrinsic extensions
#nowarn "69"  // interface implementations in intrinsic extensions


(*** Exceptions ***)

/// <summary>
/// Raise exception <c>Invalid_argument</c> with the given string.
/// </summary>
let invalid_arg (str : string) =
    raise <| Invalid_argument "str"

// exception Exit


(*** Comparisons ***)

/// e1 == e2 tests for physical equality of e1 and e2.
let inline ( == ) x y =
    LanguagePrimitives.PhysicalEquality x y

/// Negation of (==).
let inline ( != ) x y =
    not <| LanguagePrimitives.PhysicalEquality x y


(*** Boolean operations ***)

// NOTE : The 'not', (&&), and (||) operators are already
// provided in the F# Core Library (FSharp.Core).

[<Obsolete("Deprecated. The (&&) operator should be used instead.")>]
let inline (&) (x : bool) (y : bool) =
    x && y

[<Obsolete("Deprecated. The (||) operator should be used instead.")>]
let inline (or) (x : bool) (y : bool) =
    x || y


(*** Integer arithmetic ***)

let inline succ (x : int) =
    x + 1

let inline pred (x : int) =
    x - 1

/// Integer remainder. If y is not zero, the result of x mod y satisfies the following properties:
/// x = (x / y) * y + x mod y and abs(x mod y) <= abs(y) - 1.
/// If y = 0, x mod y raises Division_by_zero. Note that x mod y is negative only if x < 0.
let inline (mod) (x : int) (y : int) =
    Operators.(%) x y

/// The greatest representable integer.
let [<Literal>] max_int =
    System.Int32.MaxValue

/// The smallest representable integer.
let [<Literal>] min_int =
    System.Int32.MinValue


(* Bitwise operations *)

/// Bitwise logical and.
let inline (land) (x : int) (y : int) =
    Operators.(&&&) x y

/// Bitwise logical or.
let inline (lor) (x : int) (y : int) =
    Operators.(|||) x y

/// Bitwise logical exclusive or.
let inline (lxor) (x : int) (y : int) =
    Operators.(^^^) x y

/// Bitwise logical negation.
let inline lnot (x : int) =
    Operators.(~~~) x

/// n lsl m shifts n to the left by m bits.
/// The result is unspecified if m < 0 or m >= bitsize, where bitsize is 32 on a 32-bit platform and 64 on a 64-bit platform.
let inline (lsl) (x : int) (y : int) =
    Operators.(<<<) x y

/// n lsr m shifts n to the right by m bits.
/// This is a logical shift: zeroes are inserted regardless of the sign of n. The result is unspecified if m < 0 or m >= bitsize.
let inline (lsr) (x : int) (y : int) =
    int32 (uint32 x >>> y)

/// n asr m shifts n to the right by m bits.
/// This is an arithmetic shift: the sign bit of n is replicated. The result is unspecified if m < 0 or m >= bitsize.
let inline (asr) (x : int) (y : int) =
    Operators.(>>>) x y


(*** Floating-point arithmetic ***)

/// Unary negation.
let inline ( ~-. ) (x : float) = -x

/// Unary addition.
let inline ( ~+. ) (x : float) = x

/// Floating-point addition.
let inline ( +. ) (x : float) (y : float) = x + y

/// Floating-point subtraction.
let inline ( -. ) (x : float) (y : float) = x - y

/// Floating-point multiplication.
let inline ( *. ) (x : float) (y : float) = x * y

/// Floating-point division.
let inline ( /. ) (x : float) (y : float) = x / y

/// Exponentiation.
let inline ( ** ) (x : float) (y : float) =
    Math.Pow (x, y)

// NOTE : The following functions are already
// provided in the F# Core Library (FSharp.Core):
//  sqrt
//  exp
//  log
//  log10
//  cos
//  sin
//  tan
//  acos
//  asin
//  atan
//  atan2
//  cosh
//  sinh
//  tanh
//  ceil
//  floor

/// hypot x y returns sqrt(x * x + y * y), that is, the length of the hypotenuse
/// of a right-angled triangle with sides of length x and y, or, equivalently,
/// the distance of the point (x,y) to origin.
let hypot (x : float) (y : float) =
    raise <| System.NotImplementedException "hypot"

//
let expm1 (x : float) =
    raise <| System.NotImplementedException "expm1"

//
let log1p (x : float) =
    raise <| System.NotImplementedException "log1p"

/// abs_float f returns the absolute value of f.
let inline abs_float (x : float) =
    Operators.abs x

/// copysign x y returns a float whose absolute value is that of x and whose sign is that of y.
/// If x is nan, returns nan. If y is nan, returns either x or -. x, but it is not specified which.
let copysign (x : float) (y : float) : float =
    raise <| System.NotImplementedException "copysign"

/// mod_float a b returns the remainder of a with respect to b.
/// The returned value is a -. n *. b, where n is the quotient a /. b rounded towards zero to an integer.
let inline mod_float (a : float) (b : float) : float =
    a - b * truncate (a / b)

/// frexp f returns the pair of the significant and the exponent of f.
/// When f is zero, the significant x and the exponent n of f are equal to zero.
/// When f is non-zero, they are defined by f = x *. 2 ** n and 0.5 <= x < 1.0.
let frexp (f : float) : float * int =
    raise <| System.NotImplementedException "frexp"

/// ldexp x n returns x *. 2 ** n.
let inline ldexp (x : float) (n : int) : float =
    x * (2.0 ** float n)

/// modf f returns the pair of the fractional and integral part of f.
let inline modf (f : float) : float * float =
    let integral = Operators.floor f
    integral, f - integral

/// Convert an integer to floating-point.
let inline float_of_int (value : int) : float =
    float value

/// Truncate the given floating-point number to an integer.
/// The result is unspecified if the argument is nan or falls outside the range of representable integers.
let inline int_of_float (value : float) : int =
    int32 value

/// Positive infinity.
let [<Literal>] infinity =
    System.Double.PositiveInfinity

/// Negative infinity.
let [<Literal>] neg_infinity =
    System.Double.NegativeInfinity

//
let [<Literal>] nan =
    System.Double.NaN

/// The largest positive finite value of type float.
let [<Literal>] max_float =
    System.Double.MaxValue

/// The smallest positive, non-zero, non-denormalized value of type float.
let [<Literal>] min_float =
    System.Double.MinValue

/// The difference between 1.0 and the smallest exactly representable floating-point number greater than 1.0.
let [<Literal>] epsilon_float = 0x3CB0000000000000LF // Int64.float_of_bits 4372995238176751616L

/// The five classes of floating-point numbers, as determined by
/// the <see cref="classify_float"/> function.
type fpclass =
    /// Normal number.
    | FP_normal
    /// Number very close to 0.0, has reduced precision.
    | FP_subnormal
    /// Number is 0.0 or -0.0.
    | FP_zero
    /// Number is positive or negative infinity.
    | FP_infinite
    /// Not a number: result of an undefined operation.
    | FP_nan

/// Determines if the given floating-point number is a subnormal value.
let private isSubnormal (value : float) =
    let rawValue = uint64 <| BitConverter.DoubleToInt64Bits value

    let exponent = (rawValue &&& 0x7FF0000000000000UL) >>> 52
    let significand = rawValue &&& 0x000FFFFFFFFFFFFFUL

    exponent = 0UL
    && significand <> 0UL

/// Return the class of the given floating-point number:
/// normal, subnormal, zero, infinite, or not a number.
let classify_float (value : float) : fpclass =
    if value = 0.0 || value = -0.0 then
        FP_zero
    elif Double.IsNaN value then
        FP_nan
    elif Double.IsInfinity value then
        FP_infinite
    elif isSubnormal value then
        FP_subnormal
    else
        FP_normal


(*** String operations ***)

//
let inline (^) (x : string) (y : string) =
    System.String.Concat (x, y)


(*** Character operations ***)

/// Return the ASCII code of the argument.
let inline int_of_char (c : char) : int =
    int c

/// Return the character with the given ASCII code.
let char_of_int (value : int) : char =
    // Preconditions
    if value < int Byte.MinValue ||
        value > int Byte.MaxValue then
        raise <| Invalid_argument "char_of_int"
    
    char value


(*** Unit operations ***)

// NOTE : The 'ignore' function is already provided
// in the F# Core Library (FSharp.Core).


(*** String conversion functions ***)

/// Return the string representation of a boolean.
let inline string_of_bool (value : bool) : string =
    if value then "true" else "false"

/// Convert the given string to a boolean.
let bool_of_string (str : string) : bool =
    match str with
    | "true" -> true
    | "false" -> false
    | _ ->
        raise <| Invalid_argument "bool_of_string"

/// Return the string representation of an integer, in decimal.
let inline string_of_int (value : int) : string =
    value.ToString ()

/// Convert the given string to an integer.
/// The string is read in decimal (by default) or in hexadecimal (if it begins with 0x or 0X),
/// octal (if it begins with 0o or 0O), or binary (if it begins with 0b or 0B).
let int_of_string (str : string) : int =
    // TODO : This function should also check the parsed value -- if it's
    // outside the range of a 31-bit integer, then fail in that case too.
    if str.StartsWith ("0x", StringComparison.OrdinalIgnoreCase) then
        try Convert.ToInt32 (str.[2..], 16)
        with _ -> failwith "int_of_string"
    elif str.StartsWith ("0o", StringComparison.OrdinalIgnoreCase) then
        try Convert.ToInt32 (str.[2..], 8)
        with _ -> failwith "int_of_string"
    elif str.StartsWith ("0b", StringComparison.OrdinalIgnoreCase) then
        try Convert.ToInt32 (str.[2..], 2)
        with _ -> failwith "int_of_string"
    else
        match Int32.TryParse str with
        | true, value ->
            value
        | false, _ ->
            failwith "int_of_string"

/// Return the string representation of a floating-point number.
let string_of_float (value : float) : string =
    value.ToString ()

/// Convert the given string to a float.
let float_of_string (str : string) : float =
    try float str
    with _ ->
        failwith "float_of_string"


(*** Pair operations ***)

/// Return the first component of a pair.
let inline fst p = fst p

/// Return the second component of a pair.
let inline snd p = snd p


(*** List operations ***)

// NOTE : The '@' operator is already provided
// in the F# Core Library (FSharp.Core).


(*** Input/output ***)
(* Note: all input/output functions can raise Sys_error when the system calls they invoke fail. *)

/// The type of input channel.
type in_channel = System.IO.TextReader

/// The type of output channel.
type out_channel = TextWriter

/// The standard input for the process.
let stdin : in_channel = stdin

/// The standard output for the process.
let stdout : out_channel = stdout

/// The standard error output for the process.
let stderr : out_channel = stderr

//
type open_flag =
    | Open_rdonly | Open_wronly | Open_append
    | Open_creat | Open_trunc | Open_excl
    | Open_binary | Open_text
#if FX_NO_NONBLOCK_IO
#else
    | Open_nonblock
#endif
    | Open_encoding of Encoding

//--------------------------------------------------------------------------
// I/O
//
// OCaml-compatible channels conflate binary and text IO. It is very inconvenient to introduce
// out_channel as a new abstract type, as this means functions like fprintf can't be used in 
// conjunction with TextWriter values.  Hence we pretend that OCaml channels are TextWriters, and 
// implement TextWriters in such a way the the implementation contains an optional binary stream
// which is utilized by the OCaml binary I/O methods.
//
// Notes on the implementation: We discriminate between three kinds of 
// readers/writers since various operations are possible on each kind.
// StreamReaders/StreamWriters inherit from text readers/writers and
// thus support more functionality.  We could just support two 
// constructors (Binary and Text) and use dynamic type tests on the underlying .NET
// objects to detect the cases where we have StreamWriters.
//--------------------------------------------------------------------------

type writer =
    | StreamW of StreamWriter
    | TextW of (unit -> TextWriter)
    | BinaryW of BinaryWriter

//
let private defaultEncoding =
#if FX_NO_DEFAULT_ENCODING
    // default encoding on Silverlight is UTF8 (to aling with e.g. System.IO.StreamReader)
    Encoding.UTF8
#else
    Encoding.Default
#endif

//
type OutChannelImpl internal (w : writer) =
    inherit TextWriter ()
    //
    let mutable writer = w

    //
    override __.Encoding
        with get () =
            raise <| System.NotImplementedException "OutChannelImpl.get_Encoding"

    //
    member x.Writer
        with get () = writer
        and set w = writer <- w

    override x.Write(c:char[]) = x.Write(c, 0, c.Length)
    override x.Write(s:string) = x.Write(s.ToCharArray())
    override x.Write(c:char) = x.Write([| c |])
    override x.Write((c:char[]),(index:int),(count:int)) = 
        match writer with
        | StreamW sw ->
            (sw :> TextWriter).Write(c,index,count)
        | TextW tw ->
            (tw ()).Write(c,index,count)
        | BinaryW br ->
            br.Write(c,index,count)
    //
    member x.Stream =
        match writer with
        | TextW _ -> failwith "cannot access a stream for this channel"
        | BinaryW bw -> bw.BaseStream
        | StreamW sw -> sw.BaseStream

    //
    member x.TextWriter =
        match writer with
        | StreamW sw ->
            sw :> TextWriter
        | TextW tw ->
            tw ()
        | BinaryW _ ->
            failwith "Binary channels created using the OCaml-compatible Pervasives.open_out_bin cannot be used as TextWriters. \
                        Consider using 'System.IO.BinaryWriter' in preference to creating channels using open_out_bin."
        
    //
    member x.StreamWriter = 
        match writer with 
        | StreamW sw -> sw
        | _ ->
            failwith "cannot access a stream writer for this channel"

    //
    member x.BinaryWriter =
        match writer with
        | BinaryW w -> w
        | _ ->
            failwith "cannot access a binary writer for this channel"

    interface System.IDisposable with 
        member x.Dispose() = 
            match writer with
            | TextW tw -> 
                (tw() :> IDisposable).Dispose() 
            | BinaryW bw -> 
                (bw :> IDisposable).Dispose()
            | StreamW sw -> 
                (sw :> IDisposable).Dispose()

//
type reader =
    | StreamR of StreamReader
    | TextR of (unit -> TextReader)
    | BinaryR of BinaryReader

/// See OutChannelImpl
type InChannelImpl internal (r : reader) =
    inherit TextReader ()
    //
    let mutable reader = r

    //
    member x.Reader
        with get () = reader
        and set r = reader <- r
    
    //
    member x.Stream =
        match reader with
        | TextR _ -> failwith "cannot access a stream for this channel"
        | BinaryR bw -> bw.BaseStream
        | StreamR sw -> sw.BaseStream

    //
    member x.TextReader =
        match reader with
        | StreamR sw ->
            sw :> TextReader
        | TextR tw ->
            tw ()
        | _ ->
            failwith "Binary channels created using the OCaml-compatible Pervasives.open_in_bin cannot be used as TextReaders. \
                        If necessary use the OCaml compatible binary input methods Pervasvies.input etc. to read from this channel. \
                        Consider using 'System.IO.BinaryReader' in preference to channels created using open_in_bin."
        
    //
    member x.StreamReader =
        match reader with
        | StreamR sw -> sw
        | _ -> failwith "cannot access a stream writer for this channel"

    //
    member x.BinaryReader =
        match reader with
        | BinaryR w -> w
        | _ -> failwith "cannot access a binary writer for this channel"

    override x.Peek() = 
        match reader with
        | BinaryR bw ->  bw.PeekChar()
        | TextR tr -> (tr()).Peek()
        | StreamR sr -> sr.Peek()

    //
    override __.Read () =
        match reader with
        | StreamR sr ->
            sr.Read ()
        | BinaryR br ->
            br.Read ()
        | TextR tr ->
            (tr ()).Read ()

    //
    override __.Read (buffer, index, count) =
        match reader with
        | StreamR sr ->
            sr.Read (buffer, index, count)
        | BinaryR br ->
            br.Read (buffer, index, count)
        | TextR tr ->
            (tr ()).Read (buffer, index, count)

    interface System.IDisposable with 
        member x.Dispose() = 
            match reader with
            | TextR tr -> 
                (tr() :> IDisposable).Dispose() 
            | BinaryR br -> 
                (br :> IDisposable).Dispose()
            | StreamR sr -> 
                (sr :> IDisposable).Dispose()

//
let private (!!) (os : out_channel) =
    match os with
    | :? OutChannelImpl as os ->
        os.Writer
    | :? StreamWriter as sw ->
        StreamW sw
    | _ ->
        TextW (fun () -> os)

//
let private (<--) (os: out_channel) os' =
    match os with
    | :? OutChannelImpl as os ->
        os.Writer <- os'
    | _ ->
        failwith "the mode may not be adjusted on a writer not created with one of the Pervasives.open_* functions"
    
//
let private stream_to_BinaryWriter s =
    BinaryW (new BinaryWriter (s))

//
let private stream_to_StreamWriter (encoding : Encoding) (s : Stream) =
    // on mono, the StreamWriter constructor will emit a UTF-8 BOM if you pass the encoding to the constructor (even
    // if it is the default encoding).  For consistency with windows, which does not do this, don't pass the encoding 
    // unless it differs from the default.  (NOTE: if we don't do this, some tests fail because they don't expect a BOM.
    // could modify them to watch for the BOM, since it is technically acceptable.)
    if encoding = System.Text.Encoding.Default then
        StreamW (new StreamWriter (s)) 
    else
        StreamW (new StreamWriter (s, encoding))

//
module private OutChannel =
    let to_Stream (os : out_channel) =
        match !!os with
        | BinaryW bw ->
            bw.BaseStream
        | StreamW sw ->
            sw.BaseStream
        | TextW _ ->
            failwith "to_Stream: cannot access a stream for this channel"
      
    let to_StreamWriter (os : out_channel) =
        match !!os with
        | StreamW sw -> sw
        | _ -> failwith "to_StreamWriter: cannot access a stream writer for this channel"
      
    let to_TextWriter (os : out_channel) =
        match !!os with
        | StreamW sw ->
            sw :> TextWriter
        | TextW tw ->
            tw ()
        | _ -> os
      
    let of_StreamWriter w =
        new OutChannelImpl (StreamW w)
        :> out_channel

    let to_BinaryWriter (os : out_channel) =
        match !!os with
        | BinaryW bw -> bw
        | _ -> failwith "to_BinaryWriter: cannot access a binary writer for this channel"
      
    let of_BinaryWriter w =
        new OutChannelImpl (BinaryW w)
        :> out_channel
      
    let of_TextWriter (w : TextWriter) =
        let absw =
            match w with
            | :? StreamWriter as sw ->
                StreamW sw
            | tw ->
                TextW (fun () -> tw)

        new OutChannelImpl (absw)
        :> out_channel
        
    let of_Stream encoding (s : Stream) =
        new OutChannelImpl (stream_to_StreamWriter encoding s)
        :> out_channel
      
let private listContains x l =
    List.exists ((=) x) l

let open_out_gen flags (_perm : int) (s : string) =
    // permissions are ignored
    let app = listContains Open_append flags

    let access =
        match listContains Open_rdonly flags, listContains Open_wronly flags with
        | true, true ->
            invalidArg "flags" "invalid access for reading"
        | true, false ->
            invalidArg "flags" "invalid access for writing" // FileAccess.Read 
        | false, true ->
            FileAccess.Write
        | false, false ->
            if app then FileAccess.Write
            else FileAccess.ReadWrite

    let mode =
        match listContains Open_excl flags, app, listContains Open_creat flags, listContains Open_trunc flags with
        | true,false,false,false ->
            FileMode.CreateNew
        | false,false,true,false ->
            FileMode.Create
        | false,false,false,false ->
            FileMode.OpenOrCreate
        | false,false,false,true ->
            FileMode.Truncate
        | false,false,true,true ->
            FileMode.OpenOrCreate
        | false,true,false,false ->
            FileMode.Append
        | _ ->
            invalidArg "flags" "invalid mode"

    let share = FileShare.Read
    let bufferSize = 0x1000
#if FX_NO_NONBLOCK_IO
    let stream = new FileStream (s, mode, access, share, bufferSize)
#else
    let stream =
        let allowAsync = listContains Open_nonblock flags
        new FileStream (s, mode, access, share, bufferSize, allowAsync)
#endif

    match listContains Open_binary flags, listContains Open_text flags with
    | true,true ->
        invalidArg "flags" "mixed text/binary flags"
    | true,false ->
        new OutChannelImpl (stream_to_BinaryWriter stream )
        :> out_channel
    | false,_ ->
        let encoding =
            let encoding = List.tryPick (function Open_encoding e -> Some e | _ -> None) flags
            defaultArg encoding defaultEncoding
        OutChannel.of_Stream encoding (stream :> Stream)
        
let open_out (s : string) =
    open_out_gen [Open_text; Open_wronly; Open_creat] 777 s

// NOTE: equiv to
//       new BinaryWriter(new FileStream(s,FileMode.OpenOrCreate,FileAccess.Write,FileShare.Read ,0x1000,false)) 
let open_out_bin (s : string) =
    open_out_gen [Open_binary; Open_wronly; Open_creat] 777 s


let flush (os : out_channel) =
    match !!os with
    | TextW tw ->
        // the default method does not flush, is it overriden for the console?
        (tw()).Flush ()
    | BinaryW bw ->
        bw.Flush ()
    | StreamW sw ->
        sw.Flush ()

let close_out (os : out_channel) =
    match !!os with
    | TextW tw ->
        (tw ()).Close ()
    | BinaryW bw ->
        bw.Close ()
    | StreamW sw ->
        sw.Close ()

let prim_output_newline (os : out_channel) =
    match !!os with
    | TextW tw ->
        (tw()).WriteLine ()
    | BinaryW _ ->
        invalidArg "os" "the channel is a binary channel"
    | StreamW sw ->
        sw.WriteLine()

let output_string (os : out_channel) (s : string) =
    match !!os with
    | TextW tw ->
        (tw()).Write s
    | BinaryW bw ->
         // Write using a char array - writing a string writes it length-prefixed!
         Array.init s.Length (fun i -> s.[i])
         |> bw.Write
    | StreamW sw ->
        sw.Write s

let prim_output_int (os : out_channel) (s : int) =
    match !!os with
    | TextW tw ->
        (tw()).Write s
    | BinaryW _ ->
        invalidArg "os" "the channel is a binary channel"
    | StreamW sw ->
        sw.Write s

let prim_output_float (os : out_channel) (s : float) =
    match !!os with
    | TextW tw ->
        (tw()).Write s
    | BinaryW _ ->
        invalidArg "os" "the channel is a binary channel"
    | StreamW sw ->
        sw.Write s

let output_char (os : out_channel) (c : char) =
    match !!os with
    | TextW tw ->
        (tw()).Write c
    | BinaryW bw ->
        if int c > 255 then
            invalidArg "c" "unicode characters of value > 255 may not be written to binary channels"
        bw.Write (byte c)
    | StreamW sw ->
        sw.Write c

let output_chars (os : out_channel) (c : char[]) start len  = 
    match !!os with 
    | TextW tw ->
        (tw()).Write (c, start, len)
    | BinaryW bw ->
        bw.Write (c, start, len)
    | StreamW sw ->
        sw.Write (c, start, len)

let seek_out (os : out_channel) (n : int) =
    match !!os with
    | StreamW sw -> 
        sw.Flush ()
        (OutChannel.to_Stream os).Seek (int64 n, SeekOrigin.Begin)
        |> ignore
    | TextW _ ->
        (OutChannel.to_Stream os).Seek (int64 n, SeekOrigin.Begin)
        |> ignore
    | BinaryW bw ->
        bw.Flush ()
        bw.Seek (n, SeekOrigin.Begin) |> ignore
//
let pos_out (os : out_channel) =
    flush os
    int32 (OutChannel.to_Stream os).Position
//
let out_channel_length (os : out_channel) =
    flush os
    int32 (OutChannel.to_Stream os).Length

//
let output (os : out_channel) (buf : byte[]) (x : int) (len : int) = 
    match !!os with
    | BinaryW bw ->
        bw.Write (buf, x, len)
    | TextW _
    | StreamW _ ->
        output_string os (defaultEncoding.GetString (buf, x, len))

//
let output_byte (os : out_channel) (x : int) =
    match !!os with
    | BinaryW bw ->
        bw.Write(byte (x % 256))
    | TextW _
    | StreamW _ ->
        output_char os (char (x % 256))

//
let output_binary_int (os : out_channel) (x : int) =
    match !!os with
    | BinaryW bw ->
        bw.Write x
    | _ -> failwith "output_binary_int: not a binary stream"

//
let set_binary_mode_out (os : out_channel) b =
    match !!os with
    | TextW _ when b ->
        failwith "cannot set this stream to binary mode"
    | TextW _ -> ()
    | StreamW _ when not b -> ()
    | BinaryW _ when b -> ()
    | BinaryW bw ->
        os <--  stream_to_StreamWriter defaultEncoding (OutChannel.to_Stream os)
    | StreamW bw ->
        os <-- stream_to_BinaryWriter (OutChannel.to_Stream os)
    

#if FX_NO_BINARY_SERIALIZATION
#else
//
let output_value (os : out_channel) (x : 'a) =
    let formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter()
    formatter.Serialize (OutChannel.to_Stream os, box [x])
    flush os
#endif

//
let private (!!!) (c : in_channel) =
    match c with
    | :? InChannelImpl as c ->
        c.Reader
    | :? StreamReader as sr ->
        StreamR sr
    | _ ->
        TextR (fun () -> c)

//
let private (<---) (c: in_channel) r =
    match c with
    | :? InChannelImpl as c ->
        c.Reader<- r
    | _ -> failwith "the mode may only be adjusted channels created with one of the Pervasives.open_* functions"

//
let private mk_BinaryReader (s: Stream) =
    BinaryR (new BinaryReader (s))
//
let private mk_StreamReader e (s: Stream) =
    StreamR (new StreamReader (s, e, false))

//
module private InChannel =
    let of_Stream (e : Encoding) (s : Stream) =
        new InChannelImpl (mk_StreamReader e s)
        :> in_channel

    let of_StreamReader w =
        new InChannelImpl (StreamR w)
        :> in_channel

    let of_BinaryReader r =
        new InChannelImpl (BinaryR r)
        :> in_channel
      
    let of_TextReader (r : TextReader) =
        let absr =
            match r with
            | :? StreamReader as sr -> StreamR sr
            | tr -> TextR (fun () -> tr)
        new InChannelImpl(absr) :> in_channel

    let to_StreamReader (c : in_channel) =
        match !!!c with
        | StreamR sr -> sr
        | _ -> failwith "to_StreamReader: cannot access a stream reader for this channel"
      
    let to_BinaryReader (is : in_channel) =
        match !!!is with
        | BinaryR sr -> sr
        | _ -> failwith "to_BinaryReader: cannot access a binary reader for this channel"
      
    let to_TextReader (is : in_channel) =
        match !!!is with
        | TextR tr -> tr ()
        | _ -> is
      
    let to_Stream (is : in_channel) =
        match !!!is with
        | BinaryR bw -> bw.BaseStream
        | StreamR sw -> sw.BaseStream
        | _ -> failwith "cannot seek, set position or calculate length of this stream"

// permissions are ignored
let open_in_gen flags (_perm : int) (s : string) =
    let access =
        match listContains Open_rdonly flags, listContains Open_wronly flags with
        | true, true ->
            invalidArg "flags" "invalid access"
        | true, false ->
            FileAccess.Read
        | false, true ->
            invalidArg "flags" "invalid access for reading"
        | false, false ->
            FileAccess.ReadWrite

    let mode =
        match listContains Open_excl flags, listContains Open_append flags, listContains Open_creat flags, listContains Open_trunc flags with
        | false, false, false, false ->
            FileMode.Open
        | _ ->
            invalidArg "flags" "invalid mode for reading"

    let share = FileShare.Read
    let bufferSize = 0x1000
#if FX_NO_NONBLOCK_IO
    let stream =
        new FileStream(s, mode, access, share, bufferSize)
        :> Stream
#else
    let stream =
        let allowAsync = listContains Open_nonblock flags
        new FileStream (s, mode, access, share, bufferSize, allowAsync)
        :> Stream
#endif
    match listContains Open_binary flags, listContains Open_text flags with
    | true, true ->
        invalidArg "flags" "mixed text/binary flags specified"
    | true, false ->
        new InChannelImpl (mk_BinaryReader stream)
        :> in_channel
    | false, _ ->
        let encoding =
            let encoding = List.tryPick (function Open_encoding e -> Some e | _ -> None) flags
            defaultArg encoding defaultEncoding
        InChannel.of_Stream encoding stream
//
let open_in_encoded (e : Encoding) (s : string) =
    open_in_gen [Open_text; Open_rdonly; Open_encoding e] 777 s
//
let open_in (s : string) =
    open_in_gen [Open_text; Open_rdonly] 777 s

// NOTE: equivalent to
//     new BinaryReader(new FileStream(s,FileMode.Open,FileAccess.Read,FileShare.Read,0x1000,false))
let open_in_bin (s : string) =
    open_in_gen [Open_binary; Open_rdonly] 777 s

//
let close_in (is : in_channel) =
    match !!!is with
    | TextR tw -> (tw()).Close()
    | BinaryR bw -> bw.Close()
    | StreamR sw -> sw.Close()

//
let input_line (is : in_channel) =
    match !!!is with
    | BinaryR _ -> failwith "input_line: binary mode"
    | TextR tw -> match tw().ReadLine() with null -> raise End_of_file | res -> res
    | StreamR sw -> match sw.ReadLine() with null -> raise End_of_file | res -> res

//
let input_byte (is : in_channel) =
    match !!!is with
    | BinaryR bw ->  int (bw.ReadByte())
    | TextR tr -> let b = (tr()).Read() in if b = -1 then raise End_of_file else int (byte b)
    | StreamR sr -> let b = sr.Read() in if b = -1 then raise End_of_file else int (byte b)

//
let prim_peek (is : in_channel) =
    match !!!is with
    | BinaryR bw ->  bw.PeekChar()
    | TextR tr -> (tr()).Peek()
    | StreamR sr -> sr.Peek()

//
let prim_input_char (is : in_channel) =
    match !!!is with
    | BinaryR bw ->  (try int(bw.ReadByte()) with End_of_file -> -1)
    | TextR tr -> (tr()).Read()
    | StreamR sr -> sr.Read()

//
let input_char (is : in_channel) = 
    match !!!is with
    | BinaryR _ ->
        try char_of_int (input_byte is) with :? System.IO.EndOfStreamException -> raise End_of_file
    | TextR tr ->
        let b = (tr()).Read ()
        if b = -1 then raise End_of_file
        else char b
    | StreamR sr ->
        let b = sr.Read ()
        if b = -1 then raise End_of_file
        else char b

//
let input_chars (is : in_channel) (buf : char[]) start len = 
    match !!!is with
    | BinaryR bw ->
        bw.Read (buf, start, len)
    | TextR trf ->
        let tr = trf ()
        tr.Read (buf, start, len)
    | StreamR sr ->
        sr.Read (buf, start, len)

//
let seek_in (is : in_channel) (n : int) =
    match !!!is with
    | StreamR sw ->
        sw.DiscardBufferedData ()
    | TextR _
    | BinaryR _ -> ()

    ignore <| (InChannel.to_Stream is).Seek (int64 n, SeekOrigin.Begin)

//
let pos_in (is : in_channel) =
    int32 (InChannel.to_Stream is).Position
//
let in_channel_length (is : in_channel) =
    int32 (InChannel.to_Stream is).Length

//
let private input_bytes_from_TextReader (tr : TextReader) (enc : Encoding) (buf : byte[]) (x : int) (len : int) = 
    /// Don't read too many characters!
    let lenc = (len * 99) / enc.GetMaxByteCount (100)
    let charbuf : char[] = Array.zeroCreate lenc
    let nRead = tr.Read (charbuf, x, lenc)
    let count = enc.GetBytes (charbuf, x, nRead, buf, x)
    count

//
let input (is : in_channel) (buf : byte[]) (x : int) (len : int) = 
    match !!!is with
    | StreamR _ ->
        (InChannel.to_Stream is).Read (buf, x, len)
    | TextR trf ->
        input_bytes_from_TextReader (trf ()) defaultEncoding buf x len
    | BinaryR br ->
        br.Read (buf, x, len)

//
let really_input (is : in_channel) (buf : byte[]) (x : int) (len : int) =
    let mutable n = 0
    let mutable i = 1
    // while i > 0 && n < len do
    while (if i > 0 then n < len else false) do
        i <- input is buf (x + n) (len - n)
        n <- n + i

//
let private unsafe_really_input (is : in_channel) buf x len =
    really_input is buf x len
//
let input_binary_int (is : in_channel) =
    match !!!is with
    | BinaryR bw ->
        bw.ReadInt32 ()
    | _ ->
        failwith "input_binary_int: not a binary stream"

//
let set_binary_mode_in (is : in_channel) b =
    match !!!is with
    | TextR _ when b ->
        failwith "set_binary_mode_in: cannot set this stream to binary mode"
    | TextR _ -> ()
    | StreamR _ when not b -> ()
    | BinaryR _ when b -> ()
    | BinaryR bw ->
        is <---  mk_StreamReader defaultEncoding (InChannel.to_Stream is)
    | StreamR bw ->
        is <--- mk_BinaryReader (InChannel.to_Stream is)

(*
#if FX_NO_BINARY_SERIALIZATION
#else
let input_value (is:in_channel) = 
    let formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter() 
    let res = formatter.Deserialize(InChannel.to_Stream is) 
    match ((unbox res) : 'a list) with 
    | [x] -> x
    | _ -> failwith "input_value: expected one item"
#endif

type InChannelImpl with 
    override x.Dispose(deep:bool) = if deep then close_in (x :> in_channel)
    override x.Peek() = prim_peek (x :> in_channel) 
    override x.Read() = prim_input_char (x :> in_channel) 
    override x.Read((buffer:char[]),(index:int),(count:int)) = input_chars (x :> in_channel) buffer index count
    

type OutChannelImpl with 
    override x.Dispose(deep:bool) = if deep then close_out (x :> out_channel)
    override x.Encoding = x.TextWriter.Encoding
    override x.FormatProvider = x.TextWriter.FormatProvider
    override x.NewLine = x.TextWriter.NewLine
    override x.Write(s:string) = output_string (x :> out_channel) s
    override x.Write(c:char) = output_char (x :> out_channel) c
    override x.Write(c:char[]) = output_chars (x :> out_channel) c 0 c.Length
    override x.Write((c:char[]),(index:int),(count:int)) = output_chars (x :> out_channel) c index count
*)

(* Output functions on standard output *)


/// Print a character on standard output.
let print_char (c : char) : unit =
    output_char stdout c

/// Print a string on standard output.
let print_string (str : string) : unit =
    output_string stdout str

/// Print an integer, in decimal, on standard output.
let print_int (value : int) : unit =
    prim_output_int stdout value

/// Print a floating-point number, in decimal, on standard output.
let print_float (value : float) : unit =
    prim_output_float stdout value

/// Print a newline character on standard output, and flush standard output.
/// This can be used to simulate line buffering of standard output.
let print_newline () : unit =
    prim_output_newline stdout

/// Print a string, followed by a newline character,
/// on standard output and flush standard output.
let print_endline (str : string) : unit =
    print_string str
    print_newline ()


(* Output functions on standard error *)

/// Print a character on standard error.
let prerr_char (c : char) : unit =
    output_char stderr c

/// Print a string on standard error.
let prerr_string (str : string) : unit =
    output_string stderr str

/// Print an integer, in decimal, on standard error.
let prerr_int (value : int) : unit =
    prim_output_int stderr value

/// Print a floating-point number, in decimal, on standard error.
let prerr_float (value : float) : unit =
    prim_output_float stderr value

/// Print a newline character on standard error, and flush standard error.
/// This can be used to simulate line buffering of standard error.
let prerr_newline () : unit =
    prim_output_newline stderr

/// Print a string, followed by a newline character,
/// on standard error and flush standard error.
let prerr_endline (str : string) : unit =
    prerr_string str
    prerr_newline ()


(* Input functions on standard input *)

/// Flush standard output, then read characters from standard input until a newline character is encountered.
/// Return the string of all characters read, without the newline character at the end.
let read_line () : string =
    stdout.Flush ()
    input_line stdin

/// Flush standard output, then read one line from standard input and convert it to an integer.
let read_int () : int =
    int_of_string (read_line ())

/// Flush standard output, then read one line from standard input and convert it to a floating-point number.
/// The result is unspecified if the line read is not a valid representation of a floating-point number.
let read_float () : float =
    float_of_string (read_line ())


(* General output functions *)
// TODO


(* General input functions *)
// TODO


(* Operations on large files *)

/// <summary>Operations on large files.</summary>
/// <remarks>This sub-module provides 64-bit variants of the channel functions that manipulate
/// file positions and file sizes. By representing positions and sizes by 64-bit integers
/// (type int64) instead of regular integers (type int), these alternate functions allow operating
/// on files whose sizes are greater than max_int.</remarks>
module LargeFile =
    //
    let seek_out (channel : out_channel) (pos : int64) : unit =
        raise <| System.NotImplementedException "LargeFile.seek_out"

    //
    let pos_out (channel : out_channel) : int64 =
        raise <| System.NotImplementedException "LargeFile.pos_out"

    //
    let out_channel_length (channel : out_channel) : int64 =
        raise <| System.NotImplementedException "LargeFile.out_channel_length"

    //
    let seek_in (channel : in_channel) (pos : int64) : unit =
        raise <| System.NotImplementedException "LargeFile.seek_in"

    //
    let pos_in (channel : in_channel) : int64 =
        raise <| System.NotImplementedException "LargeFile.pos_in"

    //
    let in_channel_length (channel : in_channel) : int64 =
        raise <| System.NotImplementedException "LargeFile.in_channel_length"


(*** References ***)

// NOTE : The types and functions in this section
// are already available in the F# Core Library (FSharp.Core).


(*** Operations on format strings ***)

type format4<'a, 'b, 'c, 'd> = Microsoft.FSharp.Core.Format<'a, 'b, 'c, 'd>
type format<'a, 'b, 'c> = format4<'a, 'b, 'c, 'c>

//
let string_of_format (fmt : format6<_,_,_,_,_,_>) : string =
    raise <| System.NotImplementedException "string_of_format"

//
let format_of_string (str : string) : format6<_,_,_,_,_,_> =
    raise <| System.NotImplementedException "format_of_string"

//
let ( ^^ ) (f1 : format6<'a, 'b, 'c, 'd, 'e, 'f>) (f2 : format6<'f, 'b, 'c, 'e, 'g, 'h>) : format6<'a, 'b, 'c, 'd, 'g, 'h> =
    raise <| System.NotImplementedException "Pervasives.(^^)"


(*** Program termination ***)

//exception Exit

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


#if FX_NO_EXIT
#else
/// <summary>Terminate the process, returning the given status code to the operating system:
/// usually 0 to indicate no errors, and a small positive integer to indicate failure.</summary>
/// <remarks><para>All open output channels are flushed with <see cref="flush_all"/>.</para>
/// <para>An implicit <c>exit 0</c> is performed each time a program terminates
/// normally. An implicit <c>exit 2</c> is performed if the program terminates
/// early because of an uncaught exception.</para></remarks>
let exit (exitCode : int) =
    // TODO : Implement implicit flushing of output channels
    // TODO : Add handlers which perform an implicit 'exit 0' or 'exit 2' depending
    // on how the program terminates.
    Operators.exit exitCode
#endif

/// <summary>Register the given function to be called at program termination time.</summary>
/// <remarks><para>The functions registered with <see cref="at_exit"/> will be called when
/// the program executes <see cref="exit"/>, or terminates, either normally or
/// because of an uncaught exception.</para>
/// <para>The functions are called in "last in, first out" order: the function most
/// recently added with <see cref="at_exit"/> is called first.</para></remarks>
let at_exit (f : unit -> unit) : unit =
    ExitCallbacks.at_exit f

