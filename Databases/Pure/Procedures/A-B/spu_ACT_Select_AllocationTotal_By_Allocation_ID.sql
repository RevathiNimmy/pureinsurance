SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_AllocationTotal_By_Allocation_ID'
GO


CREATE PROCEDURE spu_ACT_Select_AllocationTotal_By_Allocation_ID
    @allocation_id int
AS

SELECT     
	SUM(alloc_base_amount + loss_gain_amount) * -1 AS Total_Alloc_Base_Amount, 
	SUM(alloc_ccy_amount) * -1 AS Total_Alloc_CCY_Amount

FROM         AllocationDetail

WHERE     allocation_id = @allocation_id

GO

