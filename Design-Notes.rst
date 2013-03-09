####################
FSharp.Compatibility
####################
Design/Implementation Notes
***************************


FSharp.Compatibility.OCaml
==========================

- The GC and Weak modules can be implemented using the System.GC type and
  the System.Diagnostics.Process / System.Diagnostics.ProcessInfo types.
- The Lazy module can be easily re-implemented using F#'s 'lazy' keyword.
- The Thread module should be able to be re-implemented using the
  System.Threading.Thread type (or the System.Diagnostics.ProcessThread type).
- The Str module can be implemented using System.Text.RegularExpression.
- The Random module can be implemented using System.Random.
- The Digest library can be implemented using System.Security.Cryptography.MD5
  (preferably MD5Cng, but that requires .NET 3.5 or later, so if we support
  .NET 2.0 use MD5CryptoServiceProvider).
- The Complex module can be implemented using the System.Numerics.Complex type.
- It should be possible to implement the Graphics module in a fairly
  straightforward way using System.Windows.Forms and System.Drawing.
  With some extra effort, it may also be possible to implement it on top of WPF.
  - It may also be possible (and useful) to implement the Tk module in the
    same way, as it's normally used to provide a higher-level API for designing
    GUI applications. Also look at implementing the interface exposed by
    liblablgtk-ocaml.
- The Stack module could be implemented on top of the System.Collections.Generic.Stack type.
  Or, we could create a Stack type which simply contains a reference cell with an F# list inside it.
- The Queue module is simple (and has already been partially implemented). However,
  some of the operations in the module require completion in O(1) time, so we can't
  use the System.Collections.Generic.Queue<'T> type; instead, we'll implement a
  purely functional queue, then create a Queue type which holds a reference cell
  containing an instance of the functional queue.


FSharp.Compatibility.OCaml.System
=================================

- The Unix, UnixLabels, and ThreadUnix modules can be implemented in a
  separate library, and built on top of the Mono.Unix / Mono.Posix libraries.
- The Sys module can be implemented using System.Environment, System.Diagnostics.Process,
  System.IO.File, System.IO.Directory.
  - The signal-handling functions and constants are only relevant on
    Unix-based systems, so the re-implemented module should probably go in
    the separate library that'll also contain the Unix module; this way the
    signal-handling functions can be implemented using the Mono.Unix library.
