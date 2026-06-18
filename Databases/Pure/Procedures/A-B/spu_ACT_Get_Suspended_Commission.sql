EXEC DDLDropProcedure 'spu_ACT_Get_Suspended_Commission'
GO

CREATE PROCEDURE spu_ACT_Get_Suspended_Commission
	@insurance_file_cnt INT,
	@agent_type VARCHAR(20)

AS BEGIN
	
	DECLARE @is_lead_agent TINYINT

	IF @agent_type = 'LEAD'
		SELECT @is_lead_agent = 1
	ELSE
		SELECT @is_lead_agent = 0

	SELECT 	DISTINCT sat.suspended_transdetail_id
	FROM	Insurance_File ifl
	JOIN 	Insurance_Folder ifo ON ifo.insurance_folder_cnt = ifl.insurance_folder_cnt
	JOIN	Insurance_File ifl2 ON ifl2.insurance_folder_cnt = ifo.insurance_folder_cnt
	JOIN	Suspended_Accounts_Transactions sat ON sat.insurance_file_cnt = ifl2.insurance_file_cnt
	JOIN 	Agent_Commission ac ON ac.insurance_file_cnt = sat.insurance_file_cnt
	JOIN	Account a ON a.account_key = ac.party_cnt
	WHERE	ifl.insurance_file_cnt = @insurance_file_cnt
	AND		ac.is_lead_agent = @is_lead_agent
	AND		a.account_id = sat.destination_account_id
	AND		sat.is_deleted = 0
END
GO

