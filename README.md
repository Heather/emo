emo
---

 - it wants to compile every F# project without project files at all
 - to build just run emo.exe

``` fsharp
let source = 
    (new DirectoryInfo(".")).GetFiles()
    |> Array.filter /> fun f -> (   f.Extension = ".fs"
                                ||  f.Extension = ".fsx")
let opens, X =
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
    Analyze |> Seq.filter(fun (r,_,_,_) -> r = Relations.opens)   
            |> Seq.map (fun (_,f,n,v) -> (f, n, v))        |> Seq.toList ,
    Analyze |> Seq.filter(fun (r,_,_,_) -> r = Relations.modules) 
            |> Seq.map (fun (_,f,n,v) -> (f, n, v, false)) |> Seq.toList
let buildTasks = ref (  X  |> List.filter /> fun (_, _, _, X) -> X = false
                            |> List.length  )
let weirdCounter = ref 0
let rec X modules_to_compile =
    printfn "! cycle %d ->\n" !weirdCounter
    let X = 
        modules_to_compile
        |> List.filter /> fun (_, _, _, X) -> X = false
        |> List.map /> fun (f, n, v, _) -> 
            let moduleOpens = opens |> List.filter /> fun (f_o, _, _) -> (f_o = f)
            let checkCompiled = 
                moduleOpens |> Seq.map /> fun (_, _, v_o) ->
                    match v_o with
                    | mx when v_o.StartsWith "System" -> (v_o, "System", "System", true)
                    | mx when v_o.StartsWith "FSharp" -> (v_o, "FSharp", "FSharp", true)
                    | _ ->  modules_to_compile |> Seq.filter /> fun (_, _, v_m, _) 
                                                                 -> (v_m = v_o)
                                               |> Seq.map    /> fun (f_m, f_n, _, X) 
                                                                 -> (v_o, f_m, f_n, X)
                            |> fun foundModule -> if (Seq.length foundModule > 0) then
                                                    Seq.head foundModule
                                                  else (v_o, "", "", false)
            let allComiled = checkCompiled
                               |> Seq.filter /> fun (_, _, _, X) -> X = false
                               |> Seq.length
                               |> fun notComiled -> notComiled = 0
            if allComiled then 
                printfn " >>> compiling %A" n
                shellxn X
                    <| sprintf "-o:%s.dll --fullpaths %s"
                       <| n.Split('.').[0] <| X <| f
                (f, n, v, true)
let exeFiles =
    source  |> Seq.filter /> fun f -> 
        X  |> Seq.filter /> fun (f_m, n_m, _, _) -> f.FullName = f_m
            |> Seq.length
            |> fun fm -> fm = 0
```
