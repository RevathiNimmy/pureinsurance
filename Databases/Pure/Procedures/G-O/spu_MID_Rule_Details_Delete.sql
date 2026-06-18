SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_MID_Rule_Details_Delete'
GO

CREATE PROCEDURE spu_MID_Rule_Details_Delete
	@MID_Rule_id INT
AS

BEGIN
	Update MID_Rule 
	SET is_Deleted = 1
	WHERE MID_Rule_id = @MID_Rule_id
END

GO