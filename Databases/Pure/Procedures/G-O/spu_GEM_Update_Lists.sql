SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GEM_Update_Lists'
GO


CREATE PROCEDURE spu_GEM_Update_Lists
    @list_id int,
    @property_id varchar(10),
    @description varchar(50)
AS


BEGIN
UPDATE Lists
    SET
    property_id=@property_id,
    description=@description
WHERE list_id = @list_id
END
GO


