@echo off


SETLOCAL ENABLEEXTENSIONS

IF "%1"=="" goto noparams
IF "%2"=="" goto noparams
IF "%3"=="" goto noparams
IF "%4"=="" goto noparams

REM By default do not delete agents
Set AGENTSHANDLING=Simple

IF /I "%2" EQU "/SD" SET AGENTSHANDLING=Simple
IF /I "%2" EQU "-SD" SET AGENTSHANDLING=Simple

IF /I "%2" EQU "/ED" SET AGENTSHANDLING=Extended
IF /I "%2" EQU "-ED" SET AGENTSHANDLING=Extended

del Script.log

osql -d%1 -S%3 -U%4 -P%5 -iMetaProcedures.sql >> Script.log
osql -d%1 -S%3 -U%4 -P%5 -iDisableDDL.sql >> Script.log



REM How do we want to delete agents based on command line switch
IF /I "%AGENTSHANDLING%" EQU "Simple" GOTO SimpleAgentHandling
IF /I "%AGENTSHANDLING%" EQU "Extended" GOTO ExtendedAgentHandling

:SimpleAgentHandling
Echo Simple Party Delete >> Script.log
osql -d%1 -S%3 -U%4 -P%5 -iPure_Clear.sql >> Script.log
goto continue

:ExtendedAgentHandling
Echo Extended Party Delete >> Script.log
osql -d%1 -S%3 -U%4 -P%5 -iPure_Clear_Extended.sql >> Script.log
goto continue

:continue
osql -d%1 -S%3 -U%4 -P%5 -iEnableDDL.sql >> Script.log
osql -d%1 -S%3 -U%4 -P%5 -ireset_unique_number.sql >> Script.log
osql -d%1 -S%3 -U%4 -P%5 -ireset_text_file_number.sql >> Script.log
osql -d%1 -S%3 -U%4 -P%5 -iDDLRecompile.sql >> Script.log

recent.reg
goto end

:noparams
echo.
echo Usage: Cleardown DBName [/ED /SD]
echo parameters :
echo     /ED [Extended Party Delete] 
echo     /ED [Simple Party Delete] 
echo.

:end
ENDLOCAL