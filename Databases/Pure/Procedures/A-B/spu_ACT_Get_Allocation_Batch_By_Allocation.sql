SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_act_get_allocation_batch_by_allocation'
GO

CREATE PROCEDURE spu_act_get_allocation_batch_by_allocation
@allocation_id varchar(255)
AS

DECLARE @document_id as int
Select ISNULL(allocationbatch_id, 0)
From Allocation
Where allocation_id = @allocation_id
GO
