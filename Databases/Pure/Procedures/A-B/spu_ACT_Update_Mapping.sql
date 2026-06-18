SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_Mapping'
GO


CREATE PROCEDURE spu_ACT_Update_Mapping
    @mapping_id int,
    @company_id smallint,
    @maptype_id smallint,
    @description varchar(255)
AS


BEGIN
UPDATE Mapping
    SET
    company_id=@company_id,
    maptype_id=@maptype_id,
    description=@description
WHERE mapping_id = @mapping_id
END
GO


