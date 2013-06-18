emo
---

 - it wants to compile every F# project without project files at all
 - to build just run run.bat


``` fsharp
let source = 
    (new DirectoryInfo(".")).GetFiles()
    |> Array.filter /> fun f -> (   f.Extension = ".fs"
                                ||  f.Extension = ".fsx")

type Relations =
    | opens = 0
    | modules = 1

let opens, ➷ =
    let Analyze =
        [for f in source do
            use tx = f.OpenText()
            while not tx.EndOfStream do
                let line = ( tx.ReadLine() ).TrimStart()
                if line.StartsWith("open ") then
                    let split = line.Split([| "open " |], StringSplitOptions.None)
                    if split.Length > 1 then
                        yield (Relations.opens, f.FullName, f.Name, split.[1])
                else if line.StartsWith("module ") then
                    let split = line.Split([| "module " |], StringSplitOptions.None)
                    if split.Length > 1 then
                        yield (Relations.modules, f.FullName, f.Name, split.[1])]
    Analyze |> Seq.filter(fun (r,_,_,_) -> r = Relations.opens)   |> Seq.map (fun (_,f,n,v) -> (f, n, v))        |> Seq.toList ,
    Analyze |> Seq.filter(fun (r,_,_,_) -> r = Relations.modules) |> Seq.map (fun (_,f,n,v) -> (f, n, v, false)) |> Seq.toList

let buildTasks = ref (  ➷  |> List.filter /> fun (_, _, _, ✿) -> ✿ = false
                            |> List.length  )
let weirdCounter = ref 0
let rec ♥ modules_to_compile =
    printfn "! cycle %d ->\n" !weirdCounter
    let 悪魔 = 
        modules_to_compile
        |> List.filter /> fun (_, _, _, ✿) -> ✿ = false
        |> List.map /> fun (f, n, v, _) -> 
            let moduleOpens = opens |> List.filter /> fun (f_o, _, _) -> (f_o = f)
            let checkCompiled = 
                moduleOpens |> Seq.map /> fun (_, _, v_o) ->
                    match v_o with
                    | mx when v_o.StartsWith "System" -> (v_o, "System", "System", true)
                    | mx when v_o.StartsWith "FSharp" -> (v_o, "FSharp", "FSharp", true)
                    | _ ->  modules_to_compile |> Seq.filter /> fun (_, _, v_m, _) -> (v_m = v_o)
                                               |> Seq.map    /> fun (f_m, f_n, _, ✿) -> (v_o, f_m, f_n, ✿)
                            |> fun foundModule -> if (Seq.length foundModule > 0) then
                                                    Seq.head foundModule
                                                  else (v_o, "", "", false)
            let allComiled = checkCompiled
                               |> Seq.filter /> fun (_, _, _, ✿) -> ✿ = false
                               |> Seq.length
                               |> fun notComiled -> notComiled = 0
            if allComiled then 
                printfn " >>> compiling %A" n
                let ☭ =
                    sprintf "-o:%s.dll --noframework --optimize+ -r:%s --target:library --warn:4 --utf8output --fullpaths %s"
                    <| n.Split('.').[0] <| ☆ <| f
                shellxn ★ ☭
                (f, n, v, true)
            else printfn " >>> can't compile %A" f; (f, n, v, false)
        |> Seq.toList
    match ( 悪魔|> List.filter /> fun (_, _, _, ✿) -> ✿ = false
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
            |> fun fm -> fm = 0

printfn ""
if Seq.length ➷ > 0 then
    printfn "-* building modules:\n"
    ♥ ➷ ; printfn ""
if Seq.length exeFiles > 0 then
    printfn "-* building executables:\n"
    exeFiles |> Seq.iter /> fun fl ->
        printfn " >>> compiling %A" fl
        let ☭ =
            sprintf "-o:%s.exe --noframework --optimize+ -r:%s -r:%s -r:%s -r:%s -r:%s -r:settings.dll --warn:4 --utf8output --fullpaths %s"
            <| fl.Name.Split('.').[0] 
            <| ☆ <| mscorlib <| system <| system_core 
            <| "tools\\shelly\\tools\\net40\\shelly.dll"
            <| fl.FullName
        shellxn ★ ☭
```
