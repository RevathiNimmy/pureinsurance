/**********************************************************************************************************************************
**
** Created by : Mohit Kumar Rohella
** Date       : 11/08/2004
** Issue      : PN : 13630
**
**********************************************************************************************************************************/

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_gis_list_pm_caption_update'
GO

--Stored procedure created -- MKR -- 11/08/04 - PN : 13630

CREATE PROCEDURE spu_gis_list_pm_caption_update
    @table varchar(30),
    @field varchar(2000)
AS
BEGIN
    
        SELECT @table = rtrim(@table)
        SELECT @field = rtrim(@field)
        
        --Adding the column in DB
        EXECUTE DDLAddColumn @Table,@field,'varchar (30)'
    
END
GO

