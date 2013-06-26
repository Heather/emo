open shelly

open System
open System.IO

Console.Clear()
printfn "+---------------------------+"
printfn "+  emo. version 0.0.3  ☣    +"
printfn "+---------------------------+"

let mutable ☃ = @"C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0"
let mscorlib    = sprintf "\"%s\\mscorlib.dll\""    ☃
let system      = sprintf "\"%s\\System.dll\""      ☃
let system_core = sprintf "\"%s\\System.Core.dll\"" ☃

let mutable ☀ = "tools\\Heather\\tools\\net40"
let ★ = sprintf "\"%s\\fsc.exe\""         ☀
let ☆ = sprintf "\"%s\\FSharp.Core.dll\"" ☀

let source = 
    (new DirectoryInfo(".")).GetFiles()
    |> Array.filter /> fun f -> (  f.Extension = ".fs"
                                || f.Extension = ".fsx")

let § = "tools\\nuget\\nuget.exe"
let ☂ パッケージ =
    for (名前, ファイル, エモ) in パッケージ do
        let ☄ = sprintf "tools\\%s\\%s" 名前 ファイル
        if not <| File.Exists ☄ then
            printfn "%s" エモ
            let ☭ = 
                sprintf "\"install\" \"%s\" \"-OutputDirectory\" \"tools\" \"-ExcludeVersion\"" 名前
            shellxn § ☭
if File.Exists § then
    ☂ [
        yield "Heather", "tools\\net40\\fsc.exe", "Getting Custom F# Compiler with Unicode Support"
        if File.Exists "TODO" then
            yield "ctodo", "tools\\cctodo_100.exe", "Getting light todo list management util"
       ]

type Relations =
    | opens = 0
    | modules = 1

let mutable packages =
    [|  "shelly", "tools\\net40\shelly.dll", "Getting shelly", "shellxn", false
    |]
    
let opens, ➷ =
    printfn "-* code analyse\n"
    let ☎ =
        [for f in source do
            use 끈 = f.OpenText()
            while not 끈.EndOfStream do
                match ( 끈.ReadLine() ).TrimStart() with
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
    printfn "! cycle %d ->\n" !weirdCounter
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
                printfn " >>> compiling %A" n
                n.Split('.').[0] |> fun ☢ ->
                    let ☭ =
                        sprintf "-o:bin\\%s.dll --noframework --optimize+ -r:%s %s --target:library --warn:4 --utf8output --fullpaths %s"
                        <| ☢ <| ☆ 
                        <| String.Join(" ",
                            [for (_, _, f_n, _) in ✤ ->
                                sprintf "-r:%s.dll" <| f_n.Split('.').[0]
                                ])
                        <| f
                    shellxn ★ ☭; (f, n, v, true)
            else printfn " >>> can't compile %A" f; (f, n, v, false)
        |> Seq.toList
    match ( 悪魔|> List.filter /> fun (_, _, _, ✿) -> not ✿
                |> List.length ) with
    | 0 -> printfn "\n! all the modules compiled"
    | mx when mx = !buildTasks -> printfn "\n! can't compile modules"
    | mx -> weirdCounter := !weirdCounter + 1
            printfn "\n! compiled %d modules" (!buildTasks - mx)
            buildTasks := mx ; ♥ 悪魔

let exeFiles =
    source  |> Seq.filter /> fun f -> 
        ➷  |> Seq.filter /> fun (f_m, n_m, _, _) -> f.FullName = f_m
            |> Seq.length
            |> fun ✦ -> ✦ = 0

printfn ""
if not <| Directory.Exists "bin" then
    Directory.CreateDirectory "bin" |> ignore
if Seq.length ➷ > 0 then
    printfn "-* building modules:\n"
    ♥ ➷ ; printfn ""
if Seq.length exeFiles > 0 then
    printfn "-* building executables:\n"
    exeFiles |> Seq.iter /> fun fl ->
        printfn " >>> compiling %A" fl
        fl.Name.Split('.').[0] |> fun ☢ ->
            let ☭ =
                sprintf "-o:bin\\%s.exe --noframework --optimize+ -r:%s -r:%s -r:%s -r:%s %s %s --warn:4 --utf8output --fullpaths %s"
                <| ☢ <| ☆
                <| mscorlib <| system <| system_core
                <| String.Join(" ",
                    [for (pn, pf, _, _, need) in packages do
                        if need then 
                            let file = sprintf "tools\\%s\\%s" pn pf
                            yield sprintf "-r:%s" <| file
                        ])
                <| String.Join(" ",
                    [for (_, f_n, _, _) in ➷ ->
                        sprintf "-r:%s.dll" <| f_n.Split('.').[0]
                        ])
                <| fl.FullName
            shellxn ★ ☭
    printfn ""
    printfn "! all the executables compiled"
    printfn ""
    
if File.Exists "TODO" then
    printfn "-* Processing todo"
    if not <| File.Exists "todo.cmd" then
        printfn ""
        printfn "! Creating todo.cmd"
        File.WriteAllText("todo.cmd", "@echo off \n \"tools/ctodo/tools/cctodo_100.exe\" %*")
    if File.Exists "todo.db3" then File.Delete "todo.db3"
        
    "todo.cmd" |> fun ☠ ->
        shellxn ☠ "initdb"
        shellxn ☠ "set git 0"
        shellxn ☠ "set syncfile TODO"
        shellxn ☠ "sync"
        shellxn ☠ ""

printfn "-* press any key to close"
Console.ReadKey() |> ignore