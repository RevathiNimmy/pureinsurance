@echo off


SETLOCAL ENABLEEXTENSIONS

IF "%1"=="" goto noparams
IF "%2"=="" goto noparams
IF "%3"=="" goto noparams


REM By default do not delete agents

del Script.log

osql -d%1 -S%2 -U%3 -P%4 -iData_Model_Clear.sql >> Script.log

goto end

:noparams
echo.
echo Usage: Cleardown DBName [/ED /SD]
echo.

:end
ENDLOCAL