[<AutoOpen>]
module emo.settings

let mutable netpath = @"C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0"
let mscorlib    = sprintf "\"%s\\mscorlib.dll\""    netpath
let system      = sprintf "\"%s\\System.dll\""      netpath
let system_core = sprintf "\"%s\\System.Core.dll\"" netpath

let mutable fspath = "tools\\Heather\\tools\\net40"
let ★ = sprintf "\"%s\\fsc.exe\""         fspath
let ☆ = sprintf "\"%s\\FSharp.Core.dll\"" fspath