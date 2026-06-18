SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_Element'
GO


CREATE PROCEDURE spu_ACT_Add_Element
    @element_id int OUTPUT,
    @element_name varchar(30) = NULL,
    @parent_id int = NULL
AS


BEGIN
INSERT INTO Element (
    element_name,
    parent_id)
VALUES (
    @element_name,
    @parent_id)
END
BEGIN
SELECT @element_id = @@IDENTITY
END
GO


