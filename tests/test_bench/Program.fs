﻿//////////////////////////////////////////////////////////////////////////////
// 
// fscx - Expandable F# compiler project
//   Author: Kouji Matsui (@kekyo2), bleis-tift (@bleis-tift)
//   GutHub: https://github.com/fscx-projects/
//
// Creative Commons Legal Code
// 
// CC0 1.0 Universal
// 
//   CREATIVE COMMONS CORPORATION IS NOT A LAW FIRM AND DOES NOT PROVIDE
//   LEGAL SERVICES.DISTRIBUTION OF THIS DOCUMENT DOES NOT CREATE AN
//   ATTORNEY-CLIENT RELATIONSHIP.CREATIVE COMMONS PROVIDES THIS
//   INFORMATION ON AN "AS-IS" BASIS.CREATIVE COMMONS MAKES NO WARRANTIES
//   REGARDING THE USE OF THIS DOCUMENT OR THE INFORMATION OR WORKS
//   PROVIDED HEREUNDER, AND DISCLAIMS LIABILITY FOR DAMAGES RESULTING FROM
//   THE USE OF THIS DOCUMENT OR THE INFORMATION OR WORKS PROVIDED
//   HEREUNDER.
//
//////////////////////////////////////////////////////////////////////////////

open System
open System.IO
open FSharp.Expandable
open Microsoft.FSharp.Compiler.Ast.Visitors

module Program =

  let dump entry =
    Console.WriteLine(entry.ToString())

  [<EntryPoint>]
  let main argv = 
    let args = CompilerHelper.UnsafeGetPreDefinedDefaultArguments TargetRuntimes.Loaded [] (["SampleAspectLogger.fs"; "SampleCode.fs"] |> List.map Path.GetFullPath)
    args.FilterArguments <-
        [("FSharp.Expandable.Compiler.Aspect",[|"SampleCode"|])]    // Regex'd assembly name
        |> Map.ofList
    let declAspectVisitor = DeclareFscxInjectAspectVisitor("SampleAspectLogger.SampleAspect")
    CompilerHelper.RawCompileWithArguments (new Action<_>(dump)) args ([declAspectVisitor] |> Seq.cast<_>)
