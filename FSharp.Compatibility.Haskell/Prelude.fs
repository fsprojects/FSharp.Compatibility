﻿(**
Haskell Compatibility
=====================
 - This module helps to run Haskell fragments or to port code from/to Haskell.
 - By opening this module many F# and F#+ operators get shadowed.
Here's a table listing some common operators/functions:
```
+-----------------------+-----------------------+--------------------+
| Operation             | Haskell-Compatibility | Haskell            |
+=======================+=======================+====================+
| List.append           | ++                    | ++                 |
+-----------------------+-----------------------+--------------------+
| Function composition  | f . (g)               | f . g              |
+-----------------------+-----------------------+--------------------+
| Equality              | ==                    | ==                 |
+-----------------------+-----------------------+--------------------+
| Avoid Parentheses     | $                     | $                  |
+-----------------------+-----------------------+--------------------+
| Inequality            | =/                    | /=                 |
+-----------------------+-----------------------+--------------------+
| Flip                  | flip                  | flip               |
+-----------------------+-----------------------+--------------------+
| Apply function        | not available         | not available      | 
| to a value            | in Haskell            | in Haskell         |
+-----------------------+-----------------------+--------------------+
|                       |                       |                    |
+-----------------------+-----------------------+--------------------+
| Functors                                                           |
+-----------------------+-----------------------+--------------------+
| Map                   | <!> or fmap           | <$> or fmap        |
+-----------------------+-----------------------+--------------------+
| Map with              |                       |                    |
| interchanged          |                       |                    |
| arguments             |                       |                    |
+-----------------------+-----------------------+--------------------+
|                       |                       |                    |
+-----------------------+-----------------------+--------------------+
| Monoids                                                            |
+-----------------------+-----------------------+--------------------+
| Zero element          | mempty                | mempty             |
+-----------------------+-----------------------+--------------------+
| Append                | mappend               | mappend            |
+-----------------------+-----------------------+--------------------+
|                       |                       |                    |
+-----------------------+-----------------------+--------------------+
| Applicative functors                                               |
+-----------------------+-----------------------+--------------------+
| Apply (combine)       | <*>                   | <*>                |
+-----------------------+-----------------------+--------------------+
| Sequence actions      |                       |                    |
| and discard values    | *>                    | *>                 |
| of first argument     |                       |                    |
+-----------------------+-----------------------+--------------------+
| Sequence actions      |                       |                    |
| and discard values    | <*                    | <*                 |
| of second argument    |                       |                    |
+-----------------------+-----------------------+--------------------+
| A variant of <*>      |                       |                    |
| with the arguments    | <**>                  | <**>               |
| reversed.             |                       |                    |
+-----------------------+-----------------------+--------------------+
| Lift a value.         | pure'                 | pure               |
+-----------------------+-----------------------+--------------------+
|                       |                       |                    |
+-----------------------+-----------------------+--------------------+
| Alternatives                                                       |
+-----------------------+-----------------------+--------------------+
| Binary operation      | <|>                   | <|>                |
+-----------------------+-----------------------+--------------------+
|                       |                       |                    |
+-----------------------+-----------------------+--------------------+
| Monads                                                             |
+-----------------------+-----------------------+--------------------+
| Bind sequentially and |                       |                    |
| compose two actions,  |                       |                    |
| passing any value     | >>=                   | >>=                |
| produced by the       |                       |                    |
| first as an argument  |                       |                    |
| to the second.        |                       |                    |
+-----------------------+-----------------------+--------------------+
| Bind with             |                       |                    |
| interchanged          | =<<                   | =<<                |
| arguments.            |                       |                    |
+-----------------------+-----------------------+--------------------+
```
*)
module FSharp.Compatibility.Haskell.Prelude

open FSharpPlus
open FSharpPlus.Data
open FSharpPlus.Operators

let getDual (Dual x) = x
let getAll (All x) = x
let getAny (Any x) = x
let getFirst (First x) = x
let getLast (Last x) = x
let runKleisli (Kleisli f) = f

// Operatots
let ($)   x y = x y
let (.()) x y = x << y
let const' k _ = k
let (==) = (=)
let (=/) x y = not (x = y)

type DeReference_op = DeReference_op with
    static member (=>) (DeReference_op, a:'a ref        ) = !a
    static member (=>) (DeReference_op, a:string        ) = a.ToCharArray() |> Array.toList
    static member (=>) (DeReference_op, _:DeReference_op) = DeReference_op

/// converts a string to list<char> otherwise still works as dereference operator.
let inline (!) a = DeReference_op => a

let show x = '\"' :: x ++ !"\""

type Maybe<'t> = Option<'t>
let  Just x :Maybe<'t> = Some x
let  Nothing:Maybe<'t> = None
let  (|Just|Nothing|) = function Some x -> Just x | _ -> Nothing
let maybe  n f = function | Nothing -> n | Just x -> f x

/// Type abbreviation for Haskell's Either type.
// NOTE : If the Haskell Either type were directly translated to use Choice`2, then Left would become
// Choice1Of2 and Right would become Choice2Of2. However, this pattern "inverts" the translation,
// because Choice1Of2 and Right normally represent a "success" value while Choice2Of2 and Left
// represent an "error" value.
type Either<'a,'b> = Choice<'b,'a>
let  Right x :Either<'a,'b> = Choice1Of2 x
let  Left  x :Either<'a,'b> = Choice2Of2 x
let  (|Right|Left|) = function Choice1Of2 x -> Right x | Choice2Of2 x -> Left x
let either f g = function Left x -> f x | Right y -> g y

// IO
type IO<'a> = Async<'a>
let runIO (f:IO<'a>) = Async.RunSynchronously f
let getLine    = async {return System.Console.ReadLine()} :IO<string>
let putStrLn x = async {printfn "%s" x}                   :IO<unit>
let print    x = async {printfn "%A" x}                   :IO<unit>


// List

let map f x = List.map f x

let replicate  n x = List.replicate  n x

open System.Reflection
let cycle lst =
    let last = ref lst
    let rec copy = function
        | [] -> failwith "empty list"
        | [z] -> 
            let v = [z]
            last.Value <- v
            v
        | x::xs ->  x::copy xs
    let cycled = copy lst
    let strs = last.Value.GetType().GetFields(BindingFlags.NonPublic ||| BindingFlags.Instance) |> Array.map (fun field -> field.Name)
    let tailField = last.Value.GetType().GetField(Array.find(fun (s:string) -> s.ToLower().Contains("tail")) strs, BindingFlags.NonPublic ||| BindingFlags.Instance)
    tailField.SetValue(last.Value, cycled)
    cycled

let drop i list = 
    let rec loop i lst = 
        match (lst, i) with
        | ([] as x, _) | (x, 0) -> x
        | x, n -> loop (n-1) (List.tail x)
    if i > 0 then loop i list else list

let take i (list:_ list) = Seq.truncate i list |> Seq.toList


// Functors

let inline fmap f x = map f x

// Applicative functors
            
let inline pure' x   = result x
let inline empty()   = getEmpty()
let inline optional v = Just <!> v <|> pure' Nothing

// Monoids

let inline mempty() = getZero()
let inline mappend a b = plus a b
let inline mconcat s = Seq.sum s

type Ordering = LT|EQ|GT with
    static member        Empty = EQ
    static member        Append (x:Ordering, y) = 
        match x, y with
        | LT, _ -> LT
        | EQ, a -> a
        | GT, _ -> GT

let inline compare x y =
    match compare x y with
    | a when a > 0 -> GT
    | a when a < 0 -> LT
    | _            -> EQ


// Foldable
let inline foldr (f: 'a -> 'b -> 'b) (z:'b) x :'b = foldBack f x z
let inline foldl (f: 'b -> 'a -> 'b) (z:'b) x :'b = fold     f z x


// Numerics
open FSharpPlus.Math.Generic

type Integer = bigint

let inline fromInteger  (x:Integer)   :'Num    = fromBigInt x
let inline toInteger    (x:'Integral) :Integer = toBigInt   x
let inline fromIntegral (x:'Integral) :'Num = (fromInteger << toInteger) x

let inline _whenIntegral a = let _ = if false then toBigInt a else 0I in ()

let inline div (a:'Integral) b :'Integral =
    _whenIntegral a
    let (a,b) = if b < 0G then (-a,-b) else (a,b)
    (if a < 0G then (a - b + 1G) else a) / b
            
let inline quot (a:'Integral) (b:'Integral) :'Integral = _whenIntegral a; a / b
let inline rem  (a:'Integral) (b:'Integral) :'Integral = _whenIntegral a; a % b
let inline quotRem a b :'Integral * 'Integral = _whenIntegral a; divRem a b
let inline mod'   a b :'Integral = _whenIntegral a; ((a % b) + b) % b
let inline divMod D d :'Integral * 'Integral =
    let q, r = quotRem D d
    if (r < 0G) then
        if (d > 0G) then (q - 1G, r + d)
        else             (q + 1G, r - d)
    else (q, r)


let inline ( **) a (b:'Floating) :'Floating = a ** b
let inline sqrt    (x:'Floating) :'Floating = Microsoft.FSharp.Core.Operators.sqrt x
            
let inline asinh x :'Floating = log (x + sqrt (1G+x*x))
let inline acosh x :'Floating = log (x + (x+1G) * sqrt ((x-1G)/(x+1G)))
let inline atanh x :'Floating = (1G/2G) * log ((1G+x) / (1G-x))
            
let inline logBase x y  :'Floating =  log y / log x


/// Monad

let inline return' x = result x

let inline sequence ms = List.sequence ms
let inline replicateM n x = List.replicateM n x            
let inline mapM f as' = List.traverse f as'
let inline filterM p x = List.filterM p x
let inline liftM  f m1    = m1 >>= (return' << f)
let inline liftM2 f m1 m2 = m1 >>= fun x1 -> m2 >>= fun x2 -> return' (f x1 x2)
let inline ap     x y     = liftM2 id x y
            
let do' = new Builders.MonadPlusBuilder()


// Monad Plus
let inline mzero() = getEmpty()
let inline mplus (x:'a) (y:'a) : 'a = (<|>) x y
let inline guard x = if x then return' () else mzero()




// Arrow
let inline id'() = getCatId ()
let inline (<<<) f g = catComp f g
let inline (>>>) f g = catComp g f
let inline (&&&) f g = fanout f g
let inline (|||) f g = fanin  f g
let inline app() = getApp ()
let inline zeroArrow() = mzero ()
let inline (<+>)   f g = (<|>) f g


// Cont

let callCC' = Cont.callCC
let inline when'  p s = if p then s else return' ()
let inline unless p s = when' (not p) s

let runCont = Cont.run
type Cont = Cont
let Cont = Cont.Cont
            

// Reader

let ask'      = Reader.ask
let local'    = Reader.local

let runReader = Reader.run
type Reader = Reader
let Reader = Reader.Reader
            

// State

let get'      = State.get
let put'      = State.put
let execState = State.exec

let runState  = State.run
type State = State
let State = State.State

            
// Monad Transformers
type MaybeT<'T> = OptionT<'T>
let MaybeT  x = OptionT x
let runMaybeT = OptionT.run
let inline mapMaybeT f x = OptionT.map f x
let runListT  = ListT.run
let inline liftIO (x: IO<'a>) = liftAsync x

        
// ContT
let runContT  = ContT.run
type ContT = ContT
let ContT = ContT.ContT
            
// ReaderT
let runReaderT = ReaderT.run
type ReaderT = ReaderT
let ReaderT = ReaderT.ReaderT
            
// StateT
let runStateT = StateT.run
type StateT = StateT
let StateT = StateT.StateT
            
// MonadError
let inline throwError x   = throw x
let inline catchError v h = catch v h
            
// ErrorT
let runErrorT = ChoiceT.run
type ErrorT = ErrorT
let ErrorT = ErrorT.ErrorT