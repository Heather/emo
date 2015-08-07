@echo off
cls

:: TODO: Remove NuGet.exe

:: Clean up
rm -rf bin

:: Nuget
SET EnableNuGetPackageRestore=true
if not exist "tools\emo\tools\emo.exe" (
	"tools\nuget\nuget.exe" "install" "emo" "-OutputDirectory" "tools" "-ExcludeVersion"
)

if "%1"=="--rebuild" goto Rebuild

"tools\emo\tools\emo.exe" --debug --notodo
goto End

:Rebuild

::If emo will fail there is hack:
set c=tools\Heather\tools\
set shelly=tools\shelly\tools\net40\shelly.dll
set mscor="C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\mscorlib.dll"
set sys="C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\System.dll"
set core="C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\System.Core.dll"
%c%fsc.exe -o:Forelock.dll --target:library --noframework --optimize+ -r:%c%FSharp.Core.dll -r:%mscor% -r:%sys% -r:%core% -r:%shelly% --warn:4 --utf8output --fullpaths Forelock.fs
%c%fsc.exe -o:emo.exe --noframework --optimize+ -r:%c%FSharp.Core.dll -r:%mscor% -r:%sys% -r:%core% -r:%shelly% -r:Forelock.dll --warn:4 --utf8output --fullpaths emo.fs

if exist "tools\emo\tools\Forelock.dll" (
	rm -f tools\emo\tools\Forelock.dll
)
if exist "tools\emo\tools\emo.exe" (
	rm -f tools\emo\tools\emo.exe
)

if exist "Forelock.dll" (
	if exist "emo.exe" (
		mv Forelock.dll tools\emo\tools\
		mv emo.exe tools\emo\tools\
	)
)

:End
