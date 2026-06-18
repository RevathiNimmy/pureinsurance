SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Update_CashListItem_Allocation_Status'
GO

--Start (Prakash C Varghese) - (Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc) - (6.2.6)
CREATE  PROCEDURE spu_SAM_Update_CashListItem_Allocation_Status
@CashListItem_ID int,
@AllocationStatus_ID int

AS
BEGIN
    UPDATE CashListItem
    SET    AllocationStatus_ID = @AllocationStatus_ID
    WHERE  CashListItem_ID = @CashListItem_ID

END
--End (Prakash C Varghese) - (Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc) - (6.2.6)
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
