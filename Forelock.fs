module Forelock

open shelly

open System
open System.IO

(* Some dictionary *)
let ✓ s args = sprintf s args
let ✞ s args = shellxn s args
let ❂ ssargs = printfn ssargs

let mutable start  = AppDomain.CurrentDomain.BaseDirectory

(* Base .NET Framework Path, currently support v. 4.0 -> 4.5.2 *)
let ☃ = let b = ✓ @"%s\Reference Assemblies\Microsoft\Framework\.NETFramework\"
                <|if (IntPtr.Size = 4)
                      then Environment.GetEnvironmentVariable("ProgramFiles")
                      else Environment.GetEnvironmentVariable("ProgramFiles(x86)")
        if    File.Exists (✓ "%sv4.5.2\\mscorlib.dll" b) then (✓ "%sv4.5.2" b)
        elif  File.Exists (✓ "%sv4.5.1\\mscorlib.dll" b) then (✓ "%sv4.5.1" b)
        elif  File.Exists (✓ "%sv4.5\\mscorlib.dll" b)   then (✓ "%sv4.5" b)
        elif  File.Exists (✓ "%sv4.0\\mscorlib.dll" b)   then (✓ "%sv4.0" b)
        else  null

let mscorlib    = ✓ "\"%s\\mscorlib.dll\""    ☃
let system      = ✓ "\"%s\\System.dll\""      ☃
let system_core = ✓ "\"%s\\System.Core.dll\"" ☃

let ☀   = ✓ "%s\\..\\..\\Heather\\tools" start
let ★   = ✓ "\"%s\\fsc.exe\""           ☀
let ☆   = ✓ "\"%s\\FSharp.Core.dll\""   ☀
let ★★  = ✓ "\"%s\\fsi.exe\""           ☀

type Relations =
    | opens = 0
    | modules = 1

(* List of Packages automatically detected and downloaded from Nuget *)
let mutable packages =
    [|  "shelly", "tools\\net40\\shelly.dll", "Getting shelly", "shellxn", false
    |]
