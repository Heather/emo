module Forelock

open System
open System.IO

let downloadNugetTo path =
    let fullPath = path |> Path.GetFullPath;
    if File.Exists fullPath then
        printf "Downloading NuGet..."
        use webClient = new System.Net.WebClient()
        fullPath |> Path.GetDirectoryName |> Directory.CreateDirectory |> ignore
        webClient.DownloadFile("https://nuget.org/nuget.exe", path |> Path.GetFullPath)
        printfn "Done."
