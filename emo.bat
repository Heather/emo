@echo off
cls

if %PROCESSOR_ARCHITECTURE%==x86 (
         set MSBUILD="%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
				 set mscor="%ProgramFiles%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\mscorlib.dll"
				 set sys="%ProgramFiles%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\System.dll"
				 set core="%ProgramFiles%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\System.Core.dll"
) else ( set MSBUILD="%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"
    if not exist "%ProgramFiles(x86)%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\" (
        set mscor="%ProgramFiles(x86)%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\mscorlib.dll"
		set sys="%ProgramFiles(x86)%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\System.dll"
		set core="%ProgramFiles(x86)%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\System.Core.dll"
    ) else (
        set mscor="%ProgramFiles(x86)%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\mscorlib.dll"
		set sys="%ProgramFiles(x86)%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\System.dll"
		set core="%ProgramFiles(x86)%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\System.Core.dll"
    )
)
if not exist tools\nuget\NuGet.exe %MSBUILD% tools\nuget\NuGet.targets /t:CheckPrerequisites

:: Clean up
rm -rf bin

:: Nuget
SET EnableNuGetPackageRestore=true
if not exist "tools\emo\tools\emo.exe" (
	"tools\nuget\NuGet.exe" "install" "emo" "-OutputDirectory" "tools" "-ExcludeVersion"
)

if "%1"=="--rebuild" goto Rebuild

"tools\emo\tools\emo.exe" --debug --notodo --nokeypress
goto End

:Rebuild

::If emo will fail there is hack:
set c=tools\Heather\tools\
set shelly=tools\shelly\tools\net40\shelly.dll

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
