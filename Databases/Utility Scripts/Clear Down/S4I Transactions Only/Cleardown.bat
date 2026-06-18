@echo off
IF "%1"=="" goto noparams

del Script.log

isql -d%1 -Usa -P -iMetaProcedures.sql >> Script.log
isql -d%1 -Usa -P -iDisableDDL.sql >> Script.log
isql -d%1 -Usa -P -iSirius_Clear_Transactions.sql >> Script.log
isql -d%1 -Usa -P -iEnableDDL.sql >> Script.log
isql -d%1 -Usa -P -ireset_unique_number.sql >> Script.log
isql -d%1 -Usa -P -ireset_text_file_number.sql >> Script.log
isql -d%1 -Usa -P -iDDLRecompile.sql >> Script.log

goto end

:noparams
echo.
echo Usage: Cleardown DBName
echo.

:end