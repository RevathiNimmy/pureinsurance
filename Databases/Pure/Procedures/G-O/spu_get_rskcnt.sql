SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_rskcnt'
GO


/**********************************************************************************************************************************
** Created by Tom O'Toole
** 25/6/2002
**
** NAME:        spu_get_rskcnt
**
**
** DESCRIPTION: The field RSKCNT will exist on risk only when we've done a data transfer
**
**********************************************************************************************************************************
**
***********************************************************************************************************************************/
CREATE PROCEDURE spu_get_rskcnt
        (
         @risk_cnt int,
         @rskcnt int OUTPUT
         )
AS

DECLARE @SQL varchar (255)
CREATE TABLE #RSKCNT
    (rskcnt int null)

IF EXISTS (SELECT o.name, c.name
        FROM syscolumns c
        JOIN sysobjects o ON o.id = c.id AND o.type = 'U'
        WHERE c.name = 'rskcnt'
        AND o.name = 'risk')

    BEGIN
        print 'Column exists'
        SELECT @SQL = 'INSERT INTO #RSKCNT SELECT RSKCNT FROM risk where risk_cnt = ' + convert(varchar(10), @risk_cnt)
    END
    ELSE
    BEGIN
        print 'Column does not exist'
        SELECT @SQL =  'INSERT INTO #RSKCNT SELECT NULL'
    END

EXECUTE(@SQL)
SELECT  @rskcnt = rskcnt FROM #RSKCNT
DROP TABLE #RSKCNT

GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

