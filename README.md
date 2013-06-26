emo
---

 - it wants to compile every F# project without project files at all
 - to build just run emo.exe
 
Projects list that could be compiled with emo:
----------------------------------------------

 - emo itself
 - Charmed ( https://github.com/Heather/Charmed )

``` fsharp
let rec ♥ modules_to_compile =
    printfn "! cycle %d ->\n" !weirdCounter
    let 悪魔 = 
        modules_to_compile
        |> List.filter /> fun (_, _, _, ✿) -> not ✿
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
                               |> Seq.filter /> fun (_, _, _, ✿) -> not ✿
                               |> Seq.length
                               |> fun notComiled -> notComiled = 0
            if allComiled then 
                printfn " >>> compiling %A" n
                n.Split('.').[0] |> fun new_fn ->
                    let ☭ =
                        sprintf "-o:bin\\%s.dll -r:%s %s s %s"
                        <| new_fn <| ☆ 
                        <| String.Join(" ",
                            [for (_, _, f_n, _) in checkCompiled ->
                                sprintf "-r:%s.dll" <| f_n.Split('.').[0]
                                ])
                        <| f
                    shellxn ★ ☭; (f, n, v, true)
```
