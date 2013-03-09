####################
FSharp.Compatibility
####################
Compatibility Libraries for F#
******************************

This repository contains libraries which help adapt existing code in other languages to F#.


OCaml
=====

- `FSharp.Compatibility.OCaml`_ *(Apache 2.0)*

  The OCaml Core library and most of the Standard Library.

- `FSharp.Compatibility.OCaml.Format`_ *(LGPL v2)*

  The Format module from the OCaml Standard Library. This is provided as a separate assembly for licensing reasons -- it contains source code from the OCaml Standard Library.

- `FSharp.Compatibility.OCaml.System`_ *(Apache 2.0)*

  Implementations of certain system-related modules from the OCaml Standard Library, such as **Sys** and **Unix**. This is provided as a separate assembly to avoid taking additional dependencies in the **FSharp.Compatibility.OCaml** project.

.. _`FSharp.Compatibility.OCaml`: https://nuget.org/packages/FSharp.Compatibility.OCaml
.. _`FSharp.Compatibility.OCaml.Format`: https://nuget.org/packages/FSharp.Compatibility.OCaml.Format
.. _`FSharp.Compatibility.OCaml.System`: https://nuget.org/packages/FSharp.Compatibility.OCaml.System


Standard ML (SML)
=================

- `FSharp.Compatibility.StandardML`_ *(BSD)*

  The Standard ML '97 Basis Library.

.. _`FSharp.Compatibility.StandardML`: https://nuget.org/packages/FSharp.Compatibility.StandardML

License
=======

The projects in this repository are individually licensed. The license for each project is listed next to the project name in the descriptions above; please see the **LICENSE** or **COPYING** files within the project folders for specific license details.
