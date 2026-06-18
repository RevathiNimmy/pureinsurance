SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Delete_Credit_Control_Item_Using_PFID'
GO

CREATE PROCEDURE spu_Delete_Credit_Control_Item_Using_PFID

	@PFInstalments_Id INT

AS

Begin

	UPDATE Credit_Control_Item WITH (ROWLOCK)
    SET is_deleted = 1
	WHERE   PFInstalments_Id = @PFInstalments_Id

END 
GO