
EXECUTE DDLDropProcedure 'spu_Report_GetUnderwritingType_SFU'
GO
/**********************************************************************************************************************************
** Created by Jude Killip
** 19/11/2001
**
** NAME:        sp_Report_GetUnderwritingType
**
** PARAMETERS:  @UWType char (1)            OUTPUT
**
** USAGE:       DECLARE @UWType char (1)
**              EXECUTE sp_Report_GetUnderwritingType @UWType OUTPUT
**
** DESCRIPTION: Gets UW_Type from Hidden_Options. Used for deciding whether "Reinsurer" or "Insurer" is displayed
**              on reports. Default is 'U' ("Reinsurer")
**
**********************************************************************************************************************************
** 1.01 11/07/2002  JMK     comment out 'print' statements - upsetting reports
***********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_GetUnderwritingType_SFU
        (
         @UWType char (1) OUTPUT
         )
AS

-- It needs to be like this until Everyone gets the extra column 'UW_Type'
-- SQL will eventually and simply be this: SELECT @UWType = IsNull(UW_type,'U') FROM Hidden_Options

DECLARE @SQL varchar (255)
CREATE TABLE #HiddenOption
    (uwtype char (1))

IF EXISTS (SELECT o.name, c.name
        FROM syscolumns c
        JOIN sysobjects o ON o.id = c.id AND o.type = 'U'
        WHERE c.name = 'UW_type'
        AND o.name = 'hidden_options')

    BEGIN
        --print 'Column exists'
        SELECT @SQL = 'INSERT INTO #HiddenOption SELECT IsNull(UW_type,"U") FROM Hidden_Options'
    END
    ELSE
    BEGIN
        --print 'Column does not exist'
        SELECT @SQL =  'INSERT INTO #HiddenOption SELECT ' + '"U"'
    END

EXECUTE(@SQL)
SELECT  @UWType = uwtype FROM #HiddenOption
DROP TABLE #HiddenOption

GO