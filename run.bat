@echo off

cls
SET EnableNuGetPackageRestore=true

::F# Unicode
if not exist "tools\Heather\tools\net40\fsc.exe" (
    echo Getting Custom F# Compiler with Unicode Support
    "tools\nuget\nuget.exe" "install" "Heather" "-OutputDirectory" "tools" "-ExcludeVersion"
)

::shelly
if not exist "tools\shelly\tools\net40\shelly.dll" (
    echo Getting shelly
    "tools\nuget\nuget.exe" "install" "shelly" "-OutputDirectory" "tools" "-ExcludeVersion"
)

::ctodo
if not exist "tools\ctodo\tools\cctodo_100.exe" (
    echo Getting light todo list management util
	"tools\nuget\nuget.exe" "install" "ctodo" "-OutputDirectory" "tools" "-ExcludeVersion"
)

::Env
set c=tools\Heather\tools\net40\
set shelly=tools\shelly\tools\net40\shelly.dll
set mscor="C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\mscorlib.dll"
set sys="C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.dll"
set core="C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Core.dll"

::Compile CSS
%c%fsc.exe -o:settings.dll --noframework --optimize+ -r:%c%FSharp.Core.dll --target:library --warn:4 --utf8output --fullpaths settings.fs
%c%fsc.exe -o:emo.exe --noframework --optimize+ -r:%c%FSharp.Core.dll -r:%mscor% -r:%sys% -r:%core% -r:%shelly% -r:settings.dll --warn:4 --utf8output --fullpaths emo.fs

::Read todo
set todo=call todo.cmd
rm todo.db3

%todo% initdb
%todo% set git 0
%todo% set syncfile TODO
%todo% sync

echo +
echo + TODO:
%todo%

pause