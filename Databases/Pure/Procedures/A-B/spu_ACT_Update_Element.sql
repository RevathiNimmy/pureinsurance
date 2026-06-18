SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_Element'
GO


CREATE PROCEDURE spu_ACT_Update_Element
    @element_id int,
    @element_name varchar(30),
    @parent_id int
AS

/*eck180901 increase element name from 20 to 30 characters*/
BEGIN
UPDATE Element
    SET
    element_name=@element_name,
    parent_id=@parent_id
WHERE element_id = @element_id
END
GO


