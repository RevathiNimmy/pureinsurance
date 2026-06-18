SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_CLM_Show_InfoCheckList'
GO


CREATE PROCEDURE spu_CLM_Show_InfoCheckList
(
	@v_lRiskTypeId int
)
AS
	select Show_information_Checklist
	From Risk_type
	Where Risk_type_id = @v_lRiskTypeId 
	and is_deleted= 0
GO



