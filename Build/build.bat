cd /d %~dp0\..\
echo off

set BUILD=Build
set ERROR=%BUILD%\@error

set OUTPUT=Output\Release
set OUTPUTx86=%OUTPUT%\x86
set OUTPUTx64=%OUTPUT%\x64
set VER_TARGET=%OUTPUTx86%\MnMn.exe
set ZIP=%BUILD%\zip.vbs
set GV=%BUILD%\get-ver.vbs

set DOTNETVER=v4.0.30319

del /Q %ERROR%

rmdir /S /Q "%OUTPUTx86%"
rem rmdir /S /Q "%OUTPUTx64%"

rem if "%PROCESSOR_ARCHITECTURE%" NEQ "x86" (
rem 	set MB=%windir%\microsoft.net\framework64\%DOTNETVER%\msbuild
rem ) else (
rem 	set MB=%windir%\microsoft.net\framework\%DOTNETVER%\msbuild
rem )
set MB=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe

echo build x86
"%MB%" Source\MnMn.sln /p:DefineConstants="BUILD;%1" /p:Configuration=Release;Platform=x86 /t:Rebuild /m /p:TargetFrameworkVersion=v4.7
set ERROR_X86=%ERRORLEVEL%

rem echo build x64
rem "%MB%" Source\MnMn.sln /p:DefineConstants="BUILD;%1" /p:Configuration=Release;Platform=x64 /t:Rebuild /m /p:TargetFrameworkVersion=v4.6
rem set ERROR_X64=%ERRORLEVEL%

if not %ERROR_X86% == 0 echo "build error x86: %ERROR_X86%" >> "%ERROR%"
rem if not %ERROR_X64% == 0 echo "build error x64: %ERROR_X64%" >> "%ERROR%"

for /F "usebackq" %%s in (`cscript "%GV%" "%VER_TARGET%"`) do set EXEVER=%%s

if "%2" == "FULL" goto REMOVED

echo remove
echo remove *.pdb, *.xml
del /S /Q *.pdb
del /S /Q "%OUTPUTx86%\lib\*.xml"
rem del /S /Q "%OUTPUTx64%\lib\*.xml"

:REMOVED

echo compression
cscript "%ZIP%" "%OUTPUTx86%" "%OUTPUT%\MnMn_%EXEVER%_x86.zip"
rem cscript "%ZIP%" "%OUTPUTx64%" "%OUTPUT%\MnMn_%EXEVER%_x64.zip"

