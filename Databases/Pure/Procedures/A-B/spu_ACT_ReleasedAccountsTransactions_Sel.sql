SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

Execute DDLDropProcedure 'spu_ACT_ReleasedAccountsTransactions_Sel'
GO

-- Object:  Stored Procedure spu_ACT_ReleasedAccountsTransactions_Sel 
-- Script Date: 3/12/2004  
-- PN21000 DC270505 added DISTINCT to ensure same results not given more than once

CREATE PROCEDURE spu_ACT_ReleasedAccountsTransactions_Sel
	@TriggerTransdetailId int = NULL,
	@AllocationId int  = NULL  
AS

IF @TriggerTransdetailId = 0
	SELECT @TriggerTransdetailId = NULL
IF @AllocationId = 0
	SELECT @AllocationId = NULL

If  @AllocationId IS NOT NULL AND  @TriggerTransdetailId IS NOT NULL  
BEGIN
	SELECT DISTINCT
	r.suspended_transdetail_id,
	t.transdetail_id 
	FROM released_accounts_transactions r
	JOIN suspended_accounts_transactions s on r.suspended_transdetail_id = s.suspended_transdetail_id
	JOIN transdetail dt on dt.transdetail_id =r.destination_transdetail_id
	JOIN transdetail t on t.document_id = dt.document_id
	WHERE s.linked_transdetail_id = @TriggerTransdetailId
	AND r.allocation_id = @AllocationId
	AND t.transdetail_id <> dt.transdetail_id

	RETURN
END

If  @AllocationId IS NOT NULL
BEGIN
	SELECT DISTINCT
	r.suspended_transdetail_id,
	t.transdetail_id 
	FROM released_accounts_transactions  r
	JOIN transdetail dt on dt.transdetail_id =r.destination_transdetail_id
	JOIN transdetail t on t.document_id = dt.document_id
	WHERE r.allocation_id = @AllocationId
	AND t.transdetail_id <> dt.transdetail_id
	RETURN
END
 
IF  @TriggerTransdetailId IS NOT NULL  
BEGIN
	SELECT DISTINCT
	r.suspended_transdetail_id,
	t.transdetail_id
	FROM released_accounts_transactions r
	JOIN suspended_accounts_transactions s on r.suspended_transdetail_id = s.suspended_transdetail_id 
	JOIN transdetail dt on dt.transdetail_id =r.destination_transdetail_id
	JOIN transdetail t on t.document_id = dt.document_id	
	WHERE s.linked_transdetail_id = @TriggerTransdetailId
	AND t.transdetail_id <> dt.transdetail_id
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

