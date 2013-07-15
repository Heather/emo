@echo off
cls
SET EnableNuGetPackageRestore=true
if not exist "tools\emo\tools\emo.exe" (
	"tools\nuget\nuget.exe" "install" "emo" "-OutputDirectory" "tools" "-ExcludeVersion"
    if exist "%ProgramFiles%\Reference Assemblies\Microsoft\FSharp\.NETFramework\v4.0\4.3.0.0\FSharp.Core.dll" (
        cp "%ProgramFiles%\Reference Assemblies\Microsoft\FSharp\.NETFramework\v4.0\4.3.0.0\FSharp.Core.dll" "tools\emo\tools\FSharp.Core.dll"
    )
)
"tools\emo\tools\emo.exe"