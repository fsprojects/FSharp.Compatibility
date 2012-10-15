(*  OCaml Compatibility Library for F#
    (FSharpx.Compatibility.OCaml)

    Copyright (c) 2012 Jack Pappas (github.com/jack-pappas)

    This code is available under the Apache 2.0 license.
    See the LICENSE file for the complete text of the license. *)

// References:
// http://caml.inria.fr/pub/docs/manual-ocaml/manual037.html
// http://hal.inria.fr/docs/00/07/00/27/PDF/RT-0141.pdf


/// <summary>Operation on arbitrary-precision numbers.</summary>
/// <remarks>Numbers (type num) are arbitrary-precision rational numbers, plus the
/// special elements 1/0 (infinity) and 0/0 (undefined).</remarks>
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FSharpx.Compatibility.OCaml.Num

open System
open System.Globalization
open System.Numerics
open Microsoft.FSharp.Math

// TEMP : Alias for 'nat' so it can be used by the function definitions below.
// TODO : For full compatibility, 'nat' needs to be defined as in OCaml, i.e.,
// as an inductive type which can represent an arbitrary-length unsigned integer.
type nat = uint64

(* The BigRational type from the F# PowerPack *)
[<AutoOpen>]
module BigRationalLargeImpl =
    let ZeroI = BigInteger 0
    let OneI = BigInteger 1
    let ToDoubleI (x : BigInteger) = double x
    let ToInt32I (x : BigInteger) = int32 x
        
[<CustomEquality; CustomComparison>]
type BigRationalLarge = 
    | Q of BigInteger * BigInteger // invariants: (p,q) in lowest form, q >= 0 

    override n.ToString() =
        let (Q(p,q)) = n 
        if q.IsOne then p.ToString() 
        else p.ToString() + "/" + q.ToString()

    static member Hash (Q(ap,aq)) = 
        // This hash code must be identical to the hash for BigInteger when the numbers coincide.
        if aq.IsOne then ap.GetHashCode() else (ap.GetHashCode() <<< 3) + aq.GetHashCode()

    override x.GetHashCode() = BigRationalLarge.Hash(x)
        
    static member Equals(Q(ap,aq), Q(bp,bq)) = 
        BigInteger.(=)  (ap,bp) && BigInteger.(=) (aq,bq)   // normal form, so structural equality 
        
    static member LessThan(Q(ap,aq), Q(bp,bq)) = 
        BigInteger.(<)  (ap * bq,bp * aq)
        
    // note: performance improvement possible here
    static member Compare(p,q) = 
        if BigRationalLarge.LessThan(p,q) then -1 
        elif BigRationalLarge.LessThan(q,p)then  1 
        else 0 

    interface System.IComparable with 
        member this.CompareTo(obj:obj) = 
            match obj with 
            | :? BigRationalLarge as that -> BigRationalLarge.Compare(this,that)
            | _ -> invalidArg "obj" "the object does not have the correct type"

    override this.Equals(that:obj) = 
        match that with 
        | :? BigRationalLarge as that -> BigRationalLarge.Equals(this,that)
        | _ -> false

    member x.IsNegative = let (Q(ap,_)) = x in sign ap < 0
    member x.IsPositive = let (Q(ap,_)) = x in sign ap > 0

    member x.Numerator = let (Q(p,_)) = x in p
    member x.Denominator = let (Q(_,q)) = x in q
    member x.Sign = (let (Q(p,_)) = x in sign p)

    static member ToDouble (Q(p,q)) = 
        ToDoubleI p / ToDoubleI q

    static member Normalize (p:BigInteger,q:BigInteger) =
        if q.IsZero then
            raise (System.DivideByZeroException())  (* throw for any x/0 *)
        elif q.IsOne then
            Q(p,q)
        else
            let k = BigInteger.GreatestCommonDivisor(p,q)
            let p = p / k 
            let q = q / k 
            if sign q < 0 then Q(-p,-q) else Q(p,q)

    static member Rational  (p:int,q:int) = BigRationalLarge.Normalize (bigint p,bigint q)
    static member RationalZ (p,q) = BigRationalLarge.Normalize (p,q)
       
    static member Parse (str:string) =
        let len = str.Length 
        if len=0 then invalidArg "str" "empty string";
        let j = str.IndexOf '/' 
        if j >= 0 then 
            let p = BigInteger.Parse (str.Substring(0,j)) 
            let q = BigInteger.Parse (str.Substring(j+1,len-j-1)) 
            BigRationalLarge.RationalZ (p,q)
        else
            let p = BigInteger.Parse str 
            BigRationalLarge.RationalZ (p,OneI)
        
    static member (~-) (Q(bp,bq))    = Q(-bp,bq)          // still coprime, bq >= 0 
    static member (+) (Q(ap,aq),Q(bp,bq)) = BigRationalLarge.Normalize ((ap * bq) + (bp * aq),aq * bq)
    static member (-) (Q(ap,aq),Q(bp,bq)) = BigRationalLarge.Normalize ((ap * bq) - (bp * aq),aq * bq)
    static member (*) (Q(ap,aq),Q(bp,bq)) = BigRationalLarge.Normalize (ap * bp,aq * bq)
    static member (/) (Q(ap,aq),Q(bp,bq)) = BigRationalLarge.Normalize (ap * bq,aq * bp)
    static member ( ~+ )(n1:BigRationalLarge) = n1
        
 
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module BigRationalLarge = 
    open System.Numerics
    
    let inv    (Q(ap,aq)) = BigRationalLarge.Normalize(aq,ap)    

    let pown (Q(p,q)) (n:int) = Q(BigInteger.Pow(p,n),BigInteger.Pow  (q,n)) // p,q powers still coprime
        
    let equal (Q(ap,aq)) (Q(bp,bq)) = ap=bp && aq=bq   // normal form, so structural equality 
    let lt    a b = BigRationalLarge.LessThan(a,b)
    let gt    a b = BigRationalLarge.LessThan(b,a)
    let lte   (Q(ap,aq)) (Q(bp,bq)) = BigInteger.(<=) (ap * bq,bp * aq)
    let gte   (Q(ap,aq)) (Q(bp,bq)) = BigInteger.(>=) (ap * bq,bp * aq)

    let of_bigint   z = BigRationalLarge.RationalZ(z,OneI )
    let of_int n = BigRationalLarge.Rational(n,1)
       
    // integer part
    let integer (Q(p,q)) =
        let mutable r = BigInteger(0)
        let d = BigInteger.DivRem (p,q,&r)          // have p = d.q + r, |r| < |q| 
        if r < ZeroI
        then d - OneI                 // p = (d-1).q + (r+q) 
        else d                             // p =     d.q + r       
      
        
//----------------------------------------------------------------------------
// BigRational
//--------------------------------------------------------------------------

[<CustomEquality; CustomComparison>]
[<StructuredFormatDisplay("{StructuredDisplayString}N")>]
type BigRational =
    | Z of BigInteger
    | Q of BigRationalLarge

    static member ( + )(n1,n2) = 
        match n1,n2 with
        | Z z ,Z zz -> Z (z + zz)
        | Q q ,Q qq -> Q (q + qq)
        | Z z ,Q qq -> Q (BigRationalLarge.of_bigint z + qq)
        | Q q ,Z zz -> Q (q  + BigRationalLarge.of_bigint zz)

    static member ( * )(n1,n2) = 
        match n1,n2 with
        | Z z ,Z zz -> Z (z * zz)
        | Q q ,Q qq -> Q (q * qq)
        | Z z ,Q qq -> Q (BigRationalLarge.of_bigint z * qq)
        | Q q ,Z zz -> Q (q  * BigRationalLarge.of_bigint zz)

    static member ( - )(n1,n2) = 
        match n1,n2 with
        | Z z ,Z zz -> Z (z - zz)
        | Q q ,Q qq -> Q (q - qq)
        | Z z ,Q qq -> Q (BigRationalLarge.of_bigint z - qq)
        | Q q ,Z zz -> Q (q  - BigRationalLarge.of_bigint zz)

    static member ( / )(n1,n2) = 
        match n1,n2 with
        | Z z ,Z zz -> Q (BigRationalLarge.RationalZ(z,zz))
        | Q q ,Q qq -> Q (q / qq)
        | Z z ,Q qq -> Q (BigRationalLarge.of_bigint z / qq)
        | Q q ,Z zz -> Q (q  / BigRationalLarge.of_bigint zz)

    static member ( ~- )(n1) = 
        match n1 with
        | Z z -> Z (-z)
        | Q q -> Q (-q)

    static member ( ~+ )(n1:BigRational) = n1

    // nb. Q and Z hash codes must match up - see notes above
    override n.GetHashCode() = 
        match n with 
        | Z z -> z.GetHashCode()
        | Q q -> q.GetHashCode() 

    override this.Equals(obj:obj) = 
        match obj with 
        | :? BigRational as that -> BigRational.(=)(this, that)
        | _ -> false

    interface System.IComparable with 
        member n1.CompareTo(obj:obj) = 
            match obj with 
            | :? BigRational as n2 -> 
                    if BigRational.(<)(n1, n2) then -1 elif BigRational.(=)(n1, n2) then 0 else 1
            | _ -> invalidArg "obj" "the objects are not comparable"

    static member FromInt (x:int) = Z (bigint x)
    static member FromBigInt x = Z x

    static member Zero = BigRational.FromInt(0) 
    static member One = BigRational.FromInt(1) 


    static member PowN (n,i:int) =
        match n with
        | Z z -> Z (BigInteger.Pow (z,i))
        | Q q -> Q (BigRationalLarge.pown q i)

    static member op_Equality (n,nn) = 
        match n,nn with
        | Z z ,Z zz -> BigInteger.(=) (z,zz)
        | Q q ,Q qq -> (BigRationalLarge.equal q qq)
        | Z z ,Q qq -> (BigRationalLarge.equal (BigRationalLarge.of_bigint z) qq)
        | Q q ,Z zz -> (BigRationalLarge.equal q (BigRationalLarge.of_bigint zz))
    static member op_Inequality (n,nn) = not (BigRational.op_Equality(n,nn))
    
    static member op_LessThan (n,nn) = 
        match n,nn with
        | Z z ,Z zz -> BigInteger.(<) (z,zz)
        | Q q ,Q qq -> (BigRationalLarge.lt q qq)
        | Z z ,Q qq -> (BigRationalLarge.lt (BigRationalLarge.of_bigint z) qq)
        | Q q ,Z zz -> (BigRationalLarge.lt q (BigRationalLarge.of_bigint zz))
    static member op_GreaterThan (n,nn) = 
        match n,nn with
        | Z z ,Z zz -> BigInteger.(>) (z,zz)
        | Q q ,Q qq -> (BigRationalLarge.gt q qq)
        | Z z ,Q qq -> (BigRationalLarge.gt (BigRationalLarge.of_bigint z) qq)
        | Q q ,Z zz -> (BigRationalLarge.gt q (BigRationalLarge.of_bigint zz))
    static member op_LessThanOrEqual (n,nn) = 
        match n,nn with
        | Z z ,Z zz -> BigInteger.(<=) (z,zz)
        | Q q ,Q qq -> (BigRationalLarge.lte q qq)
        | Z z ,Q qq -> (BigRationalLarge.lte (BigRationalLarge.of_bigint z) qq)
        | Q q ,Z zz -> (BigRationalLarge.lte q (BigRationalLarge.of_bigint zz))
    static member op_GreaterThanOrEqual (n,nn) = 
        match n,nn with
        | Z z ,Z zz -> BigInteger.(>=) (z,zz)
        | Q q ,Q qq -> (BigRationalLarge.gte q qq)
        | Z z ,Q qq -> (BigRationalLarge.gte (BigRationalLarge.of_bigint z) qq)
        | Q q ,Z zz -> (BigRationalLarge.gte q (BigRationalLarge.of_bigint zz))

    member n.IsNegative = 
        match n with 
        | Z z -> sign z < 0 
        | Q q -> q.IsNegative

    member n.IsPositive = 
        match n with 
        | Z z -> sign z > 0
        | Q q -> q.IsPositive
            
    member n.Numerator = 
        match n with 
        | Z z -> z
        | Q q -> q.Numerator

    member n.Denominator = 
        match n with 
        | Z _ -> OneI
        | Q q -> q.Denominator

    member n.Sign = 
        if n.IsNegative then -1 
        elif n.IsPositive then  1 
        else 0

    static member Abs(n:BigRational) = 
        if n.IsNegative then -n else n

    static member ToDouble(n:BigRational) = 
        match n with
        | Z z -> ToDoubleI z
        | Q q -> BigRationalLarge.ToDouble q

    static member ToBigInt(n:BigRational) = 
        match n with 
        | Z z -> z
        | Q q -> BigRationalLarge.integer q 

    static member ToInt32(n:BigRational) = 
        match n with 
        | Z z -> ToInt32I(z)
        | Q q -> ToInt32I(BigRationalLarge.integer q )

    static member op_Explicit (n:BigRational) = BigRational.ToInt32 n
    static member op_Explicit (n:BigRational) = BigRational.ToDouble n
    static member op_Explicit (n:BigRational) = BigRational.ToBigInt n


    override n.ToString() = 
        match n with 
        | Z z -> z.ToString()
        | Q q -> q.ToString()

    member x.StructuredDisplayString = x.ToString()
                   
    static member Parse(s:string) = Q (BigRationalLarge.Parse s)

type BigNum = BigRational
type bignum = BigNum

module NumericLiteralN = 
    let FromZero () = BigRational.Zero 
    let FromOne () = BigRational.One 
    let FromInt32 i = BigRational.FromInt i
    let FromInt64 (i64:int64) = BigRational.FromBigInt (new BigInteger(i64))
    let FromString s = BigRational.Parse s





//
[<CustomEquality; CustomComparison>]
type Num =
    /// 32-bit signed integer.
    | Int of int
    /// Arbitrary-precision integer.
    | Big_int of bigint
    // Arbitrary-precision rational.
    | Ratio of BigRational

    //
    static member Zero
        with get () = Int 0

    //
    static member One
        with get () = Int 1

    //
    static member (*inline*) private FromInt64 (value : int64) : Num =
        if value > (int64 Int32.MaxValue) ||
            value < (int64 Int32.MinValue) then
            Big_int <| bigint value
        else
            Int <| int value

    //
    static member (*inline*) private FromBigInt (value : bigint) : Num =
        // OPTIMIZE : Create static (let-bound) values to hold bigint versions
        // of Int32.MinValue and Int32.MaxValue
        if value > (bigint Int32.MaxValue) ||
            value < (bigint Int32.MinValue) then
            Big_int value
        else
            Int <| int value

    //
    static member (*inline*) private FromBigRational (value : BigRational) =
        // Determine if the BigRational represents a whole (i.e., non-fractional)
        // quantity; if so, convert it to an int or bigint.
        if (value.Numerator % value.Denominator).IsZero then
            value.Numerator / value.Denominator
            |> Num.FromBigInt
        else
            Ratio value

    static member op_Addition (x : Num, y : Num) : Num =
        match x, y with
        | Int x, Int y ->
            (int64 x) + (int64 y)
            |> Num.FromInt64
        | Int x, Big_int y ->
            (bigint x) + y
            |> Num.FromBigInt
        | Int x, Ratio y ->
            Ratio <| (BigRational.FromInt x) + y
        | Big_int x, Int y ->
            x + (bigint y)
            |> Num.FromBigInt
        | Big_int x, Big_int y ->
            x + y
            |> Num.FromBigInt
        | Big_int x, Ratio y ->
            (BigRational.FromBigInt x) + y
            |> Ratio
        | Ratio x, Int y ->
            x + (BigRational.FromInt y)
            |> Ratio
        | Ratio x, Big_int y ->
            x + (BigRational.FromBigInt y)
            |> Ratio
        | Ratio x, Ratio y ->
            x + y
            |> Num.FromBigRational

    static member op_Subtraction (x : Num, y : Num) : Num =
        match x, y with
        | Int x, Int y ->
            (int64 x) - (int64 y)
            |> Num.FromInt64
        | Int x, Big_int y ->
            (bigint x) - y
            |> Num.FromBigInt
        | Int x, Ratio y ->
            Ratio <| (BigRational.FromInt x) - y
        | Big_int x, Int y ->
            x - (bigint y)
            |> Num.FromBigInt
        | Big_int x, Big_int y ->
            x - y
            |> Num.FromBigInt
        | Big_int x, Ratio y ->
            (BigRational.FromBigInt x) - y
            |> Ratio
        | Ratio x, Int y ->
            x - (BigRational.FromInt y)
            |> Ratio
        | Ratio x, Big_int y ->
            x - (BigRational.FromBigInt y)
            |> Ratio
        | Ratio x, Ratio y ->
            x - y
            |> Num.FromBigRational

    static member op_Multiply (x : Num, y : Num) : Num =
        match x, y with
        | Int x, Int y ->
            (int64 x) * (int64 y)
            |> Num.FromInt64
        | Int x, Big_int y ->
            (bigint x) * y
            |> Big_int
        | Int x, Ratio y ->
            (BigRational.FromInt x) * y
            |> Num.FromBigRational
        | Big_int x, Int y ->
            x * (bigint y)
            |> Big_int
        | Big_int x, Big_int y ->
            x * y
            |> Big_int
        | Big_int x, Ratio y ->
            (BigRational.FromBigInt x) * y
            |> Num.FromBigRational
        | Ratio x, Int y ->
            x * (BigRational.FromInt y)
            |> Num.FromBigRational
        | Ratio x, Big_int y ->
            x * (BigRational.FromBigInt y)
            |> Num.FromBigRational
        | Ratio x, Ratio y ->
            x * y
            |> Num.FromBigRational

    static member op_Division (x : Num, y : Num) : Num =
        // Preconditions
        if y.IsZero then
            Exception ("Division_by_zero",
                DivideByZeroException ())
            |> raise

        (*  Don't perform the actual division operation -- just create a Ratio
            from the inputs to avoid any possible truncation of the result. *)
        let x, y =
            match x, y with
            | Int x, Int y ->
                (BigRational.FromInt x), (BigRational.FromInt y)
            | Int x, Big_int y ->
                (BigRational.FromInt x), (BigRational.FromBigInt y)
            | Int x, Ratio y ->
                (BigRational.FromInt x), y
            | Big_int x, Int y ->
                (BigRational.FromBigInt x), (BigRational.FromInt y)
            | Big_int x, Big_int y ->
                (BigRational.FromBigInt x), (BigRational.FromBigInt y)
            | Big_int x, Ratio y ->
                (BigRational.FromBigInt x), y
            | Ratio x, Int y ->
                x, (BigRational.FromInt y)
            | Ratio x, Big_int y ->
                x, (BigRational.FromBigInt y)
            | Ratio x, Ratio y ->
                x, y

        // Divide the values and create a Ratio from the result.
        // Attempt to reduce the result before returning it.
        Num.FromBigRational (x / y)

    //
    static member Quotient (x : Num, y : Num) : Num =
        match x, y with
        (*  Check for division by zero. *)
        | _, y when y.IsZero ->
            Exception ("Division_by_zero",
                DivideByZeroException ())
            |> raise

        (* Standard cases *)
        | Int x, Int y ->
            Int (x / y)
        | Int x, Big_int y ->
            (bigint x) / y
            |> Num.FromBigInt
        | Big_int x, Int y ->
            x / (bigint y)
            |> Num.FromBigInt
        | Big_int x, Big_int y ->
            x / y
            |> Num.FromBigInt
        | Int _, Ratio _
        | Big_int _, Ratio _
        | Ratio _, Int _
        | Ratio _, Big_int _
        | Ratio _, Ratio _ ->
            Num.Floor (x / y)

    static member op_Modulus (x : Num, y : Num) : Num =
        match x, y with
        (* Check for division-by-zero. *)
        | _, y when y.IsZero ->
            Exception ("Division_by_zero",
                DivideByZeroException ())
            |> raise

        | Int x, Int y ->
            Int (x % y)
        | Int x, Big_int y ->
            (bigint x) % y
            |> Num.FromBigInt
        | Big_int x, Int y ->
            x % (bigint y)
            |> Num.FromBigInt
        | Big_int x, Big_int y ->
            x % y
            |> Num.FromBigInt
        | Int _, Ratio _
        | Big_int _, Ratio _
        | Ratio _, Int _
        | Ratio _, Big_int _
        | Ratio _, Ratio _ ->
            x - (y * Num.Quotient (x, y))

    static member op_UnaryNegation (x : Num) : Num =
        match x with
        | Int x ->
            // Handle Int32.MinValue correctly by changing it to a bigint.
            if x = Int32.MinValue then
                Big_int <| -(BigInteger Int32.MinValue)
            else
                Int -x
        | Big_int x ->
            Big_int -x
        | Ratio x ->
            Ratio -x

    //
    static member Abs (x : Num) : Num =
        match x with
        | Int x ->
            // Need to handle Int32.MinValue correctly by changing it to a bigint.
            if x = System.Int32.MinValue then
                BigInteger Int32.MinValue
                |> BigInteger.Abs
                |> Big_int
            else
                Int <| abs x
        | Big_int x ->
            BigInteger.Abs x
            |> Big_int
        | Ratio x ->
            BigRational.Abs x
            |> Ratio

    //
    static member Max (x : Num, y : Num) =
        match x, y with
        | Int a, Int b ->
            Int <| max a b
        | Big_int a, Big_int b ->
            Big_int <| max a b
        | Ratio a, Ratio b ->
            Ratio <| max a b

        | ((Int a) as x), ((Big_int b) as y)
        | ((Big_int b) as y), ((Int a) as x) ->
            if (bigint a) > b then x else y

        | ((Int a) as x), ((Ratio b) as y)
        | ((Ratio b) as y), ((Int a) as x) ->
            if (BigRational.FromInt a) > b then x else y

        | ((Big_int a) as x), ((Ratio b) as y)
        | ((Ratio b) as y), ((Big_int a) as x) ->
            if (BigRational.FromBigInt a) > b then x else y

    //
    static member Min (x : Num, y : Num) =
        match x, y with
        | Int a, Int b ->
            Int <| min a b
        | Big_int a, Big_int b ->
            Big_int <| min a b
        | Ratio a, Ratio b ->
            Ratio <| min a b

        | ((Int a) as x), ((Big_int b) as y)
        | ((Big_int b) as y), ((Int a) as x) ->
            if (bigint a) < b then x else y

        | ((Int a) as x), ((Ratio b) as y)
        | ((Ratio b) as y), ((Int a) as x) ->
            if (BigRational.FromInt a) < b then x else y

        | ((Big_int a) as x), ((Ratio b) as y)
        | ((Ratio b) as y), ((Big_int a) as x) ->
            if (BigRational.FromBigInt a) < b then x else y

    //
    static member Pow (x : Num, y : Num) : Num =
        match y with
        | Int y ->
            Num.Pow (x, y)
        | Big_int y ->
            // TODO : Implement this case -- it works in the original OCaml module.
            raise <| System.NotImplementedException "Num.Pow (Num, Num)"
        | Ratio _ ->
            // TODO : This could actually be implemented, if it would be useful.
            raise <| System.NotSupportedException "Cannot raise a Num to a fractional (Ratio) power."

    //
    static member Pow (x : Num, y : int) : Num =
        match x with
        | Int x ->
            BigInteger.Pow (bigint x, y)
            |> Num.FromBigInt
        | Big_int x ->
            BigInteger.Pow (x, y)
            |> Num.FromBigInt
        | Ratio q ->
            BigRational.PowN (q, y)
            |> Num.FromBigRational

    //
    static member Sign (x : Num) : int =
        match x with
        | Int x ->
            Math.Sign x
        | Big_int x ->
            x.Sign
        | Ratio x ->
            x.Sign

    //
    static member Ceiling (x : Num) : Num =
        match x with
        | Int _
        | Big_int _ as x -> x
        | Ratio q ->
            if (q.Numerator % q.Denominator).IsZero then x
            else
                (q.Numerator / q.Denominator) + BigInteger.One
                |> Num.FromBigInt

    //
    static member Floor (x : Num) : Num =
        match x with
        | Int _
        | Big_int _ as x -> x
        | Ratio q ->
            q.Numerator / q.Denominator
            |> Num.FromBigInt

    //
    static member Round (x : Num) : Num =
        match x with
        | Int _
        | Big_int _ as x -> x
        | Ratio q ->
            // Round to nearest integer (i.e., 1/3 rounds to 0 and 2/3 rounds to 1).
            // NOTE : 1/2 rounds to 1.
            raise <| System.NotImplementedException "Num.Round"

    //
    static member Truncate (x : Num) : Num =
        match x with
        | Int _
        | Big_int _ as x -> x
        | Ratio q ->
            // Truncate any fractional part of the value -- i.e., return a bigint
            // containing the integer part of the Ratio.            
            raise <| System.NotImplementedException "Num.Truncate"

    //
    static member Parse (str : string) : Num =
        // Preconditions
        if str = null then
            raise <| ArgumentNullException "str"
        elif String.length str < 1 then
            ArgumentException ("The string is empty.", "str")
            |> raise

        match BigInteger.TryParse str with
        | true, value ->
            Num.FromBigInt value
        | false, _ ->
            // Try parsing the string as a rational.
            BigRational.Parse str
            |> Num.FromBigRational

    override this.ToString () =
        match this with
        | Int x ->
            x.ToString ()
        | Big_int x ->
            x.ToString ()
        | Ratio q ->
            q.ToString ()

    //
    member this.IsZero
        with get () =
            match this with
            | Int x ->
                x = 0
            | Big_int x ->
                x.IsZero
            | Ratio q ->
                q.Numerator.IsZero

    //
    static member private AreEqual (x : Num, y : Num) : bool =
        match x, y with
        | Int a, Int b ->
            a = b
        | Big_int a, Big_int b ->
            a = b
        | Ratio a, Ratio b ->
            a = b

        | Int a, Big_int b
        | Big_int b, Int a ->
            (bigint a) = b

        | Int a, Ratio b
        | Ratio b, Int a ->
            (BigRational.FromInt a) = b

        | Big_int a, Ratio b
        | Ratio b, Big_int a ->
            (BigRational.FromBigInt a) = b

    static member private Compare (x : Num, y : Num) : int =
        match x, y with
        | Int a, Int b ->
            compare a b
        | Big_int a, Big_int b ->
            compare a b
        | Ratio a, Ratio b ->
            compare a b

        | Int a, Big_int b
        | Big_int b, Int a ->
            compare (bigint a) b

        | Int a, Ratio b
        | Ratio b, Int a ->
            compare (BigRational.FromInt a) b

        | Big_int a, Ratio b
        | Ratio b, Big_int a ->
            compare (BigRational.FromBigInt a) b

    override this.Equals (other : obj) =
        match other with
        | :? Num as other ->
            Num.AreEqual (this, other)
        | _ ->
            false

    override this.GetHashCode () =
        match this with
        | Int x -> x
        | Big_int x ->
            x.GetHashCode ()
        | Ratio x ->
            x.GetHashCode ()

    interface System.IEquatable<Num> with
        //
        member this.Equals (other : Num) =
            Num.AreEqual (this, other)

    interface System.IComparable with
        //
        member this.CompareTo (other : obj) =
            match other with
            | :? Num as other ->
                Num.Compare (this, other)
            | _ ->
                // Should we throw an exception or return 1 or -1?
                raise <| System.NotSupportedException ()

    interface System.IComparable<Num> with
        //
        member this.CompareTo (other : Num) =
            Num.Compare (this, other)


/// Type alias for Num, for compatibility with OCaml.
type num = Num

(* TODO :   Add [<CompilerMessage>] to the functions below so when they're used
            the F# compiler will emit a warning to let the user know they can
            use the equivalent, built-in F# generic function.
            E.g., use the generic 'abs' instead of 'abs_num'. *)

/// Addition.
let inline add_num (x : num) (y : num) : num =
    x + y

let inline ( +/ ) (x : num) (y : num) : num =
    x + y

/// Unary negation.
let inline minus_num (x : num) : num =
    -x

let inline ( -/ ) (x : num) (y : num) : num =
    x - y

/// Subtraction.
let inline sub_num (x : num) (y : num) : num =
    x - y

let inline ( */ ) (x : num) (y : num) : num =
    x * y

/// Multiplication.
let inline mult_num (x : num) (y : num) : num =
    x * y

/// Square the number.
let inline square_num (x : num) : num =
    x * x

/// Division.
let inline div_num (x : num) (y : num) : num =
    x / y

/// Euclidian division.
let inline quo_num (x : num) (y : num) : num =
    Num.Quotient (x, y)

/// Modulus division.
let inline mod_num (x : num) (y : num) : num =
    x % y

//
let inline ( **/ ) (x : num) (y : num) : num =
    num.Pow (x, y)

/// Raise a number to an exponent.
let inline power_num (x : num) (y : num) : num =
    num.Pow (x, y)

/// Absolute value.
let inline abs_num (x : num) : num =
    num.Abs x

//
let inline succ_num (n : num) : num =
    n + (Int 1)

//
let inline pred_num (n : num) : num =
    n - (Int 1)

//
let incr_num (r : num ref) : unit =
    r := succ_num !r

//
let decr_num (r : num ref) : unit =
    r := pred_num !r

/// Test if a number is an integer.
let is_integer_num (n : num) : bool =
    match n with
    | Int _
    | Big_int _ ->
        true
    | Ratio q ->
        (q.Numerator % q.Denominator).IsZero


(* The four following functions approximate a number by an integer *)

//
let inline integer_num (n : num) : num =
    Num.Truncate n

//
let inline floor_num (n : num) : num =
    num.Floor n

//
let inline round_num (n : num) : num =
    Num.Round n

//
let inline ceiling_num (n : num) : num =
    num.Ceiling n

//
let inline sign_num (n : num) : int =
    num.Sign n


(* Comparisons between numbers *)

let inline ( =/ ) (x : num) (y : num) =
    x = y
let inline ( </ ) (x : num) (y : num) =
    x < y
let inline ( >/ ) (x : num) (y : num) =
    x > y
let inline ( <=/ ) (x : num) (y : num) =
    x <= y
let inline ( >=/ ) (x : num) (y : num) =
    x >= y
let inline ( <>/ ) (x : num) (y : num) =
    x <> y
let inline eq_num (x : num) (y : num) =
    x = y
let inline lt_num (x : num) (y : num) =
    x < y
let inline le_num (x : num) (y : num) =
    x <= y
let inline gt_num (x : num) (y : num) =
    x > y
let inline ge_num (x : num) (y : num) =
    x >= y

/// Return -1, 0 or 1 if the first argument is less than, equal to, or greater than the second argument.
let inline compare_num (x : num) (y : num) =
    compare x y
/// Return the greater of the two arguments.
let inline max_num (x : num) (y : num) =
    num.Max (x, y)
/// Return the smaller of the two arguments.
let inline min_num (x : num) (y : num) =
    num.Min (x, y)


(* Coercions with strings *)

//
let inline string_of_num (n : num) : string =
    n.ToString ()

//
let approx_num_fix (precision : int) (n : num) : string =
    raise <| System.NotImplementedException "approx_num_fix"

//
let approx_num_exp (precision : int) (n : num) : string =
    raise <| System.NotImplementedException "approx_num_exp"

/// Convert a string to a number.
/// Raise Failure "num_of_string" if the given string is not a valid representation of an integer
let num_of_string (str : string) : num =
    // If the string can't be parsed (i.e., an exception was thrown),
    // catch the exception then raise an OCaml-compatible exception.
    try
        num.Parse str
    with ex ->
        Exception ("num_of_string", ex)
        |> raise

(* Coercions between numerical types *)

//
let int_of_num (n : num) : int =
    match n with
    | Int x -> x
    | Big_int x ->
        // TODO : If 'n' is too large to fit into an 'int', then fail with
        // the message "int_of_string" for compatbility with OCaml.
        raise <| System.NotImplementedException "int_of_num"
    | Ratio q ->
        // TODO : If 'q' can not be represented as an 'int', then fail with
        // the message "int_of_string" for compatbility with OCaml.
        raise <| System.NotImplementedException "int_of_num"

//
let inline num_of_int (r : int) : num =
    Int r

//
let nat_of_num (n : num) : nat =
    // TODO : Determine how to handle cases where 'n' is a Ratio or
    // is a Big_int whose value is too large for an 'int'.
    raise <| System.NotImplementedException "nat_of_num"

//
let num_of_nat (r : nat) : num =
    raise <| System.NotImplementedException "num_of_nat"

//
let inline num_of_big_int (i : bigint) : num =
    Big_int i

//
let big_int_of_num (n : num) : bigint =
    match n with
    | Int i ->
        bigint i
    | Big_int i ->
        i
    | Ratio q ->
        raise <| System.NotImplementedException "big_int_of_num"

//
let ratio_of_num (n : num) : BigRational =
    match n with
    | Int i ->
        BigRational.FromInt i
    | Big_int i ->
        BigRational.FromBigInt i
    | Ratio q ->
        q

//
let inline num_of_ratio (q : BigRational) : num =
    Ratio q

//
let float_of_num (n : num) : float =
    raise <| System.NotImplementedException "float_of_num"

