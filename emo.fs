open shelly
open Forelock

open System
open System.IO

    (* Some dictionary *)
let ✓ s args = sprintf s args
let ✞ s args = shellxn s args
let ❂ ssargs = printfn ssargs

let mutable debug  = false
let mutable dotodo = true
let mutable start  = AppDomain.CurrentDomain.BaseDirectory

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

let version() =
    ❂ "+---------------------------+"
    ❂ "+     emo. version 0.1.6    +"
    ❂ "+---------------------------+"
let help() =
    version()
    ❂ """
    args |> Seq.iter(fun (arg:string) ->
                if arg.Contains         "=" then
                    let s = arg.Split   '='
                    match s.[0] with
                    | "--path" -> start <- s.[1]
                    | _ -> ()
                else
                    match arg with
                    | "--debug"             -> debug  <- true
                    | "--notodo"            -> dotodo <- false
                    | "--version" | "-v"    -> version();   rtn := true
                    | "--help" | "-h"       -> help();      rtn := true
                    | _ -> ())
    """

type Relations =
    | opens = 0
    | modules = 1

let mutable packages =
    [|  "shelly", "tools\\net40\\shelly.dll", "Getting shelly", "shellxn", false
    |]

[<EntryPoint>]
let Main(args) =
  if String.IsNullOrEmpty ☃ then
    version(); printfn "Failed to find .NET install"; -1
  else
    let rtn =  ref false
    if  args.Length > 0 then
        args |> Seq.iter(fun (arg:string) ->
                    if arg.Contains         "=" then
                        let s = arg.Split   '='
                        match s.[0] with
                        | "--path" -> start <- s.[1]
                        | _ -> ()
                    else
                        match arg with
                        | "--debug"             -> debug  <- true
                        | "--notodo"            -> dotodo <- false
                        | "--version" | "-v"    -> version();   rtn := true
                        | "--help" | "-h"       -> help();      rtn := true
                        | _ -> ())
    if debug then version()
    if not !rtn then
        let source =
            (new DirectoryInfo( ✓ "%s\\..\\..\\.." start )).GetFiles()
            |> Array.filter /> fun f -> (  f.Extension = ".fs"
                                        || f.Extension = ".fsx")

        let § = ✓ "\"%s..\\..\\nuget\\nuget.exe\"" start
        let ☂ pkgs =
            for (pn, pf, pd) in pkgs do
                if debug then printfn "check if %s exists" pf
                if ((not <| File.Exists ( ✓ "..\\..\\%s\\%s" pn pf )) &&
                    (not <| File.Exists ( ✓ "tools\\%s\\%s" pn pf ) ) ) then
                    ❂ "%s" pd
                    (✓ "\"install\" \"%s\" \"-OutputDirectory\" \"%s\\..\\..\" \"-ExcludeVersion\"" pn start)
                    |> fun ☄ ->
                        if debug then ❂ "> %s" ☄
                        ✞ § ☄
        if File.Exists "..\\..\\nuget\\nuget.exe" || File.Exists "tools\\nuget\\nuget.exe" then
            ☂ [ yield "Heather", "tools\\fsc.exe", "Getting Custom F# Compiler with Unicode Support"
                if File.Exists "TODO" then
                    yield "ctodo", "tools\\cctodo.exe", "Getting light todo list management util" ]
        else ❂ "No NuGet found on %s" §

        let opens, ➷ =
            ❂ "-* code analyse\n"
            let ☎ =
                [for f in source do
                    use tx = f.OpenText()
                    while not tx.EndOfStream do
                        match ( tx.ReadLine() ).TrimStart() with
                        | line when line.StartsWith("open ") ->
                            let split = line.Split([| "open " |], StringSplitOptions.None)
                            if split.Length > 1 then
                                yield (Relations.opens, f.FullName, f.Name, split.[1])
                        | line when line.StartsWith("module ") ->
                            let split = line.Split([| "module " |], StringSplitOptions.None)
                            if split.Length > 1 then
                                yield (Relations.modules, f.FullName, f.Name, split.[1])
                        | line -> (* Additional analyse *)
                            packages
                            |> Array.filter /> fun (_,_,_,_,✲) -> not ✲
                            |> Array.iteri /> fun i (pn,pf,pm,pc,_) ->
                                if line.Contains(pc) then
                                    packages.[i] <- (pn,pf,pm,pc,true)
                                    ☂ [ (pn, pf, pm) ] ]
            ☎ |> Seq.filter(fun (☤,_,_,_) -> ☤ = Relations.opens)   |> Seq.map (fun (_,f,n,v) -> (f, n, v))        |> Seq.toList ,
            ☎ |> Seq.filter(fun (☤,_,_,_) -> ☤ = Relations.modules) |> Seq.map (fun (_,f,n,v) -> (f, n, v, false)) |> Seq.toList

        let buildTasks = ref (  ➷  |> List.filter /> fun (_, _, _, ✿) -> not ✿
                                    |> List.length  )

        let Failess = ref false;
        let cycleCounter = ref 0
        let libCounter = ref 0
        let rec ♥ modules_to_compile =
            ❂ "! cycle %d ->\n" !cycleCounter
            let 悪魔 =
                modules_to_compile
                |> List.filter /> fun (_, _, _, ✿) -> not ✿
                |> List.map /> fun (f, n, v, _) ->
                    let ☯ = opens |> List.filter /> fun (f_o, _, _) -> (f_o = f)
                    let ✤ =
                        ☯ |> Seq.map /> fun (_, _, v_o) ->
                            match v_o with
                            | ✈ when v_o.StartsWith "System" ->  (v_o, "System", "System", true)
                            | ✈ when v_o.StartsWith "FSharp" ->  (v_o, "FSharp", "FSharp", true)
                            | ✈ when v_o.StartsWith "Failess" ->
                                if not !Failess then
                                    Failess := true
                                    ☂ [ ("Failess", "tools\\Failess.exe", "Failess build tool with CSS EDSL library") ]
                                (v_o, "Failess", (✓ "%s\\..\\..\\Failess\\tools\\FailessLib.dll" start), true)
                            | _ ->  modules_to_compile |> Seq.filter /> fun (_, _, v_m, _) -> (v_m = v_o)
                                                       |> Seq.map    /> fun (f_m, f_n, _, ✿) -> (v_o, f_m, f_n, ✿)
                                    |> fun foundModule -> if (Seq.length foundModule > 0) then
                                                            Seq.head foundModule
                                                          else (v_o, "", "", false)
                    let allComiled =   ✤
                                       |> Seq.filter /> fun (_, _, _, ✿) -> not ✿
                                       |> Seq.length
                                       |> fun ✖ -> ✖ = 0
                    if allComiled then
                        ❂ " >>> compiling %A" n
                        n.Split('.').[0] |> fun ☢ ->
                            let ☭ =
                                ✓ "-o:%s\\..\\..\\..\\bin\\%s.dll --noframework --optimize+ -r:%s -r:%s -r:%s -r:%s %s --target:library --warn:4 --utf8output --fullpaths %s"
                                <| start
                                <| if ☢ = ""
                                        then
                                            libCounter:= !libCounter + 1
                                            ✓ "Lib%d" !libCounter
                                        else ☢
                                <| ☆
                                <| mscorlib <| system <| system_core
                                <| String.Join(" ",
                                    [for (_, _, f_n, _) in ✤ do
                                        let dll = f_n.Split('.')
                                        if dll.Length > 1 then
                                            if dll.[dll.Length - 1] = "dll"
                                                then yield ✓ "-r:%s" f_n
                                                else yield ✓ "-r:%s.dll" dll.[0]
                                        ])
                                <| f
                            if debug then ❂ "> %s" ☭
                            ✞ ★ ☭; (f, n, v, true)
                    else ❂ " >>> can't compile %A" f; (f, n, v, false)
                |> Seq.toList
            match ( 悪魔|> List.filter /> fun (_, _, _, ✿) -> not ✿
                        |> List.length ) with
            | 0 -> ❂ "\n! all the modules compiled"
            | mx when mx = !buildTasks -> ❂ "\n! can't compile modules"
            | mx -> cycleCounter := !cycleCounter + 1
                    ❂ "\n! compiled %d modules" (!buildTasks - mx)
                    buildTasks := mx ; ♥ 悪魔

        let exeFiles =
            source  |> Seq.filter /> fun f ->
                ➷  |> Seq.filter /> fun (f_m, n_m, _, _) -> f.FullName = f_m
                    |> Seq.length
                    |> fun ✦ -> ✦ = 0

        let fakeFiles =
            exeFiles |> Seq.filter /> fun f -> f.Extension = ".fsx"
                     |> Seq.filter /> fun f ->
                        use tx = f.OpenText()
                        [while not tx.EndOfStream do
                            match ( tx.ReadLine() ).TrimStart() with
                            | line when line.StartsWith("Target ") -> yield true
                            | _ -> yield false]
                        |> Seq.length
                        |> fun ✦ -> ✦ > 0

        ❂ ""
        if not <| Directory.Exists ( ✓ "%s\\..\\..\\..\\bin" start ) then
            Directory.CreateDirectory ( ✓ "%s\\..\\..\\..\\bin" start) |> ignore
        if Seq.length ➷ > 0 then ❂ "-* building modules:\n"; ♥ ➷ ; ❂ ""

        if Seq.length fakeFiles > 0 then
            fakeFiles |> Seq.iter /> fun fl ->
            let ☭ = ✓ "FSI=%s %s" ★★ fl.FullName
            ✞ ( ✓ "%s\\..\\..\\Failess\\tools\\Failess.exe" start ) ☭
        else if Seq.length exeFiles > 0 then
            ❂ "-* building executables:\n"
            exeFiles |> Seq.iter /> fun fl ->
                ❂ " >>> compiling %A" fl
                fl.Name.Split('.').[0] |> fun ☢ ->
                    let ☭ =
                        ✓ "-o:%s\\..\\..\\..\\bin\\%s.exe --noframework --optimize+ -r:%s -r:%s -r:%s -r:%s %s %s --warn:4 --utf8output --fullpaths %s"
                        <| start
                        <| ☢ <| ☆
                        <| mscorlib <| system <| system_core
                        <| String.Join(" ",
                            [for (pn, pf, _, _, need) in packages do
                                if need then
                                    let file = ✓ "%s\\..\\..\\%s\\%s" start pn pf
                                    yield ✓ "-r:%s" <| file
                                ])
                        <| String.Join(" ",
                            [for (_, f_n, _, _) in ➷ do
                                let dll = f_n.Split('.')
                                if dll.Length > 1 then
                                    if dll.[dll.Length - 1] = "dll"
                                        then yield ✓ "-r:%s" f_n
                                        else yield ✓ "-r:%s.dll" dll.[0]
                                ])
                        <| fl.FullName
                    if debug then ❂ "> %s" ☭
                    ✞ ★ ☭
            ❂ ""
            ❂ "! all the executables compiled"
            ❂ ""

        if dotodo && File.Exists "TODO" then
            ❂ "-* Processing todo"
            if not <| File.Exists ( ✓ "%s\\..\\..\\..\\todo.cmd" start ) then
                ❂ "! Creating todo.cmd"
                File.WriteAllText( ( ✓ "%s\\..\\..\\..\\todo.cmd" start ), "@echo off \n \"tools/ctodo/tools/cctodo.exe\" %*")
            ( ✓ "%s\\..\\..\\..\\todo.db3" start ) |> fun ftd ->
                if File.Exists ftd then File.Delete ftd

            ✞ ( ✓ "%s\\..\\..\\ctodo\\tools\\cctodo.exe" start ) |> fun ☠ ->
                ☠ "initdb"
                ☠ "set git 0"
                ☠ "set syncfile TODO"
                ☠ "sync"
                ☠ ""

            ✓ "%s\\..\\..\\..\\tools\\emo\\tools\\todo.db3" start |> fun tdb ->
                if File.Exists tdb then
                    ✓ "%s\\..\\..\\..\\todo.db3" start |> fun ntdb ->
                        if File.Exists ntdb then File.Delete ntdb
                        File.Move(tdb, ntdb)

        ❂ "-* press any key to close"
        Console.ReadKey() |> ignore

    0 (* Success *)
