SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_ACT_ReAdd_Credit_Control_Item_For_Instalment'
GO

CREATE PROCEDURE spu_ACT_ReAdd_Credit_Control_Item_For_Instalment
	@pfinstalments_id INT
AS 
BEGIN
	UPDATE 	Credit_Control_Item
	SET		is_deleted=0
	WHERE	pfinstalments_id=@pfinstalments_id
END
GO