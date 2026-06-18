SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_Mapping'
GO


CREATE PROCEDURE spu_ACT_Add_Mapping
    @mapping_id int OUTPUT,
    @company_id smallint,
    @maptype_id smallint,
    @description varchar(255)
AS


BEGIN
INSERT INTO Mapping (
    company_id,
    maptype_id,
    description)
VALUES (
    @company_id,
    @maptype_id,
    @description)
END
BEGIN
SELECT @mapping_id = @@IDENTITY
END
GO


