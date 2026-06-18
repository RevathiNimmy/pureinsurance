SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Get_GIS_Insurer_ID'
GO


CREATE PROCEDURE spu_Get_GIS_Insurer_ID
    @Party_Id Int,
    @GIS_Insurer_Id Int OUTPUT,
    @GIS_Insurer_Desc varchar(255) OUTPUT
AS

-- *******************************************************************************
-- Gets the GIS INSURER ID For a GIVEN Party ID (From Party Table)
-- *******************************************************************************

    IF EXISTS (SELECT * FROM hidden_options WHERE option_number = 1 AND value = 'U')
        SELECT @GIS_Insurer_id = GIS_Insurer_Id,
               @GIS_Insurer_Desc = Description
        FROM  GIS_Insurer
        WHERE gis_insurer_id = 1
    ELSE
        SELECT @GIS_Insurer_id = GIS_Insurer_Id,
               @GIS_Insurer_Desc = Description
        FROM  GIS_Insurer
        WHERE Polaris_Insurer_No = @Party_Id

GO

