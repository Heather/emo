@echo off
cls
SET EnableNuGetPackageRestore=true
if not exist "tools\emo\tools\emo.exe" (
	"tools\nuget\nuget.exe" "install" "emo" "-OutputDirectory" "tools" "-ExcludeVersion"
)

"tools\emo\tools\emo.exe"

::If emo will fail there is hack:
::set c=tools\Heather\tools\
::set shelly=tools\shelly\tools\net40\shelly.dll
::set mscor="C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\mscorlib.dll"
::set sys="C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.dll"
::set core="C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Core.dll"
::%c%fsc.exe -o:emo.exe --noframework --optimize+ -r:%c%FSharp.Core.dll -r:%mscor% -r:%sys% -r:%core% -r:%shelly% --warn:4 --utf8output --fullpaths emo.fs
