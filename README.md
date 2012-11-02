## FSharpx.Compatibility
### Compatibility Libraries for F# ###

This repository contains library projects which help adapt existing code in other languages to F#.


---
### OCaml

* **[FSharpx.Compatibility.OCaml](https://nuget.org/packages/FSharpx.Compatibility.OCaml)** *(Apache 2.0)*

  The OCaml Core library and most of the Standard Library.

* **[FSharpx.Compatibility.OCaml.Format](https://nuget.org/packages/FSharpx.Compatibility.OCaml.Format)** *(LGPL v2)*

  The Format module from the OCaml Standard Library. This is provided as a separate assembly due to licensing concerns; it contains source code from the OCaml Standard Library.

* **[FSharpx.Compatibility.OCaml.System](https://nuget.org/packages/FSharpx.Compatibility.OCaml.System)** *(Apache 2.0)*

  Implementations of certain system-related modules from the OCaml Standard Library, such as **Sys** and **Unix**. This is provided as a separate assembly to avoid taking additional dependencies in the **FSharpx.Compatibility.OCaml** project.


---
### License

The projects in this repository are individually licensed. The license for each project is listed next to the project name in the descriptions above; please see the **LICENSE** or **COPYING** files within the project folders for specific license details.
