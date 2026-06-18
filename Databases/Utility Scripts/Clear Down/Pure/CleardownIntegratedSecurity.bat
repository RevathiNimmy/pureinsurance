@echo off
IF "%1"=="" goto noparams

del Script.log

osql -d%1 -E -iMetaProcedures.sql >> Script.log
osql -d%1 -E -iDisableDDL.sql >> Script.log
osql -d%1 -E -iPure_Clear.sql >> Script.log
osql -d%1 -E -iEnableDDL.sql >> Script.log
osql -d%1 -E -ireset_unique_number.sql >> Script.log
osql -d%1 -E -ireset_text_file_number.sql >> Script.log
osql -d%1 -E -iDDLRecompile.sql >> Script.log

recent.reg
goto end

:noparams
echo.
echo Usage: Cleardown DBName
echo.

:end