SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_DR_Allocation_Detail_IDs'
GO


CREATE PROCEDURE spu_ACT_Get_DR_Allocation_Detail_IDs
    @allocation_id int AS

	SELECT	allocationdetail_id,
                DT.code as document_type_Code 
	FROM AllocationDetail AD
	INNER JOIN TransDetail TD
		ON AD.transdetail_id = TD.transdetail_id 
	INNER JOIN Document D
		ON D.document_id = TD.document_id 
	INNER JOIN DocumentType DT
		ON DT.documenttype_id = D.documenttype_id 
	WHERE (allocation_id = @allocation_id) AND (orig_base_amount > 0)
GO