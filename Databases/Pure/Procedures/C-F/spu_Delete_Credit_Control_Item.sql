SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Delete_Credit_Control_Item'
GO

CREATE PROCEDURE spu_Delete_Credit_Control_Item

	@PFInstalments_Id INT

AS

Begin

	DELETE 
	FROM 	Credit_Control_Item WITH (ROWLOCK)
	WHERE   PFInstalments_Id = @PFInstalments_Id

END 
GO