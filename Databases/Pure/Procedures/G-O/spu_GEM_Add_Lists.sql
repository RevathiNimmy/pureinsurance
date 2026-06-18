SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GEM_Add_Lists'
GO


CREATE PROCEDURE spu_GEM_Add_Lists
    @list_id int OUTPUT,
    @property_id varchar(10),
    @description varchar(50)
AS


BEGIN
INSERT INTO Lists (
    property_id,
    description)
VALUES (
    @property_id,
    @description)
END
BEGIN
SELECT @list_id = @@IDENTITY
END
GO


