echo off
echo "Make build"
xcopy /y %~dp0vorp_fishing_cl\bin\Debug\vorp_fishing.net.dll %~dp0Build\vorp_fishing\client
xcopy /y %~dp0vorp_fishing_sv\bin\Debug\vorp_fishing_sv.net.dll %~dp0Build\vorp_fishing\server
pause
