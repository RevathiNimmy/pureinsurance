SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Party_Name_From_Shortname'
GO

CREATE PROCEDURE spu_Get_Party_Name_From_Shortname
    @shortname char(20)
AS
/**************************************************************************************/
/* History : 16/08/2002 - Created */
/**************************************************************************************/

BEGIN

    SET NOCOUNT ON

    SELECT name
    FROM  party 
    where shortname = @shortname

    SET NOCOUNT OFF

END
GO

