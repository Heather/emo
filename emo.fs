open shelly

open System
open System.IO

Console.Clear()

    (* Some dictionary *)
let ✓ s args = sprintf s args
let ✞ s args = shellxn s args
let ❂ ssargs = printfn ssargs

❂ "+---------------------------+"
❂ "+  emo. version 0.0.5  ☣    +"
❂ "+---------------------------+"

let mutable ☃ = @"C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0"
let mscorlib    = ✓ "\"%s\\mscorlib.dll\""    ☃
let system      = ✓ "\"%s\\System.dll\""      ☃
let system_core = ✓ "\"%s\\System.Core.dll\"" ☃

let mutable ☀ = "tools\\Heather\\tools\\net40"
let ★ = ✓ "\"%s\\fsc.exe\""         ☀
let ☆ = ✓ "\"%s\\FSharp.Core.dll\"" ☀

let source = 
    (new DirectoryInfo(".")).GetFiles()
    |> Array.filter /> fun f -> (  f.Extension = ".fs"
                                || f.Extension = ".fsx")

let § = "tools\\nuget\\nuget.exe"
let ☂ pkgs =
    for (pn, pf, pd) in pkgs do
        let ☄ = ✓ "tools\\%s\\%s" pn pf
        if not <| File.Exists ☄ then
            ❂ "%s" pd
            ✞ § (✓ "\"install\" \"%s\" \"-OutputDirectory\" \"tools\" \"-ExcludeVersion\"" pn)
if File.Exists § then
    ☂ [ yield "Heather", "tools\\net40\\fsc.exe", "Getting Custom F# Compiler with Unicode Support"
        if File.Exists "TODO" then
            yield "ctodo", "tools\\cctodo_100.exe", "Getting light todo list management util" ]
Environment.GetEnvironmentVariable("ProgramFiles") |> fun programFiles -> (* Moving from F# 3.0 to F# 3.1 is hard... *)
    ✓ @"%s\Reference Assemblies\Microsoft\FSharp\.NETFramework\v4.0\4.3.0.0\FSharp.Core.dll" programFiles
    |> fun newCore -> if File.Exists newCore then 
                        ❂ "-* Replacing 3.0 FSharp.Core.dll with 3.1 one\n"
                        File.Copy(newCore, "tools\\Heather\\tools\\net40\\FSharp.Core.dll", true)

type Relations =
    | opens = 0
    | modules = 1

let mutable packages =
    [|  "shelly", "tools\\net40\shelly.dll", "Getting shelly", "shellxn", false
    |]
    
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
let weirdCounter = ref 0
let rec ♥ modules_to_compile =
    ❂ "! cycle %d ->\n" !weirdCounter
    let 悪魔 = 
        modules_to_compile
        |> List.filter /> fun (_, _, _, ✿) -> not ✿
        |> List.map /> fun (f, n, v, _) -> 
            let ☯ = opens |> List.filter /> fun (f_o, _, _) -> (f_o = f)
            let ✤ = 
                ☯ |> Seq.map /> fun (_, _, v_o) ->
                    match v_o with
                    | ✈ when v_o.StartsWith "System" -> (v_o, "System", "System", true)
                    | ✈ when v_o.StartsWith "FSharp" -> (v_o, "FSharp", "FSharp", true)
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
                        ✓ "-o:bin\\%s.dll --noframework --optimize+ -r:%s %s --target:library --warn:4 --utf8output --fullpaths %s"
                        <| ☢ <| ☆ 
                        <| String.Join(" ",
                            [for (_, _, f_n, _) in ✤ ->
                                ✓ "-r:%s.dll" <| f_n.Split('.').[0]
                                ])
                        <| f
                    ✞ ★ ☭; (f, n, v, true)
            else ❂ " >>> can't compile %A" f; (f, n, v, false)
        |> Seq.toList
    match ( 悪魔|> List.filter /> fun (_, _, _, ✿) -> not ✿
                |> List.length ) with
    | 0 -> ❂ "\n! all the modules compiled"
    | mx when mx = !buildTasks -> ❂ "\n! can't compile modules"
    | mx -> weirdCounter := !weirdCounter + 1
            ❂ "\n! compiled %d modules" (!buildTasks - mx)
            buildTasks := mx ; ♥ 悪魔

let exeFiles =
    source  |> Seq.filter /> fun f -> 
        ➷  |> Seq.filter /> fun (f_m, n_m, _, _) -> f.FullName = f_m
            |> Seq.length
            |> fun ✦ -> ✦ = 0

❂ ""
if not <| Directory.Exists "bin" then
    Directory.CreateDirectory "bin" |> ignore
if Seq.length ➷ > 0 then ❂ "-* building modules:\n"; ♥ ➷ ; ❂ ""
if Seq.length exeFiles > 0 then
    ❂ "-* building executables:\n"
    exeFiles |> Seq.iter /> fun fl ->
        ❂ " >>> compiling %A" fl
        fl.Name.Split('.').[0] |> fun ☢ ->
            let ☭ =
                ✓ "-o:bin\\%s.exe --noframework --optimize+ -r:%s -r:%s -r:%s -r:%s %s %s --warn:4 --utf8output --fullpaths %s"
                <| ☢ <| ☆
                <| mscorlib <| system <| system_core
                <| String.Join(" ",
                    [for (pn, pf, _, _, need) in packages do
                        if need then 
                            let file = ✓ "tools\\%s\\%s" pn pf
                            yield ✓ "-r:%s" <| file
                        ])
                <| String.Join(" ",
                    [for (_, f_n, _, _) in ➷ ->
                        ✓ "-r:%s.dll" <| f_n.Split('.').[0]
                        ])
                <| fl.FullName
            ✞ ★ ☭
    ❂ ""
    ❂ "! all the executables compiled"
    ❂ ""
    
if File.Exists "TODO" then
    ❂ "-* Processing todo"
    if not <| File.Exists "todo.cmd" then
        ❂ ""
        ❂ "! Creating todo.cmd"
        File.WriteAllText("todo.cmd", "@echo off \n \"tools/ctodo/tools/cctodo_100.exe\" %*")
    if File.Exists "todo.db3" then File.Delete "todo.db3"
        
    ✞ "todo.cmd" |> fun ☠ ->
        ☠ "initdb"
        ☠ "set git 0"
        ☠ "set syncfile TODO"
        ☠ "sync"
        ☠ ""

❂ "-* press any key to close"
Console.ReadKey() |> ignore