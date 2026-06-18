SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_ACT_ManualSuspendedAccountsTransactions_Sel'
GO

CREATE PROCEDURE spu_ACT_ManualSuspendedAccountsTransactions_Sel
AS	
	SELECT SAT.linked_transdetail_id, MAX(AD.allocation_id) allocation_id
	FROM Suspended_Accounts_Transactions SAT
	INNER JOIN TransDetail TD ON TD.transdetail_id = SAT.linked_transdetail_id
	INNER JOIN Document D ON TD.document_id = D.document_id
	INNER JOIN TransDetail TD2 ON TD2.document_id = D.document_id AND TD2.account_id = TD.account_id
	INNER JOIN Insurance_File IFL ON D.insurance_file_cnt = IFL.insurance_file_cnt
	INNER JOIN AllocationDetail AD ON AD.transdetail_id = TD.transdetail_id
	WHERE SAT.is_deleted = 0 AND SAT.manually_released = 1

	GROUP BY SAT.linked_transdetail_id, SAT.released_on_full_settlement, 
		 SAT.released_for_whole_posting,
		 IFL.cover_start_date,
		 SAT.released_on_policy_effective

	HAVING (SUM(TD2.outstanding_amount) = 0 AND SAT.released_on_full_settlement = 1 AND
		SAT.released_for_whole_posting = 1 AND IFL.cover_start_date <= GETDATE()
		AND SAT.released_on_policy_effective = 1)

GO
