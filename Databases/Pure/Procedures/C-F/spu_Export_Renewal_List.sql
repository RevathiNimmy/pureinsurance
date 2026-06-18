SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Export_Renewal_List'
GO
 

CREATE PROCEDURE spu_Export_Renewal_List	
	@branch_id	int,
	@business_type  varchar(10)

AS

DECLARE	@iBranchID	int
SELECT	@iBranchID = ISNULL(@branch_id, 0)

--mkw130603 START
DECLARE @InsuranceFileAccountExec varchar(1)
SELECT @InsuranceFileAccountExec = value
    FROM Hidden_Options
    WHERE option_number = 40
IF ISNULL(@InsuranceFileAccountExec, '') = '' BEGIN
    SELECT @InsuranceFileAccountExec = '0'
END
--mkw130603 END

DECLARE @sBusiness_Type	varchar(10)

IF @business_type = 'ALL' BEGIN
	SELECT @business_type  = NULL
END

SELECT @sbusiness_type = ISNULL(@business_type, '')

SELECT 	PY1.Shortname 			Client_code,
	PY1.resolved_name 		client_name,
	IFI.insurance_ref		Policy_number,
	ISNULL(PY2.resolved_name, '')	insurer_name,
	ISNULL(PY2.shortname, '')	insurer_code,
	RG.description			Risk_group,
	IFI.cover_start_date		Date_from,
	IFI.renewal_date		Date_to,
	RC.code				Risk_code,
	RC.description			Risk_description,
	BT.description			Branch_type,
	IFI.this_premium		this_prem,
	IFI.commission_amount		Commission_amount,
	IFI.commission_percentage	commission_percentage,
	ISNULL( (SELECT SUM(F.fee_amount)
 			FROM Policy_fee	 F,
 			     Party P, 
 			     Party_Type PT
			WHERE F.insurance_file_Cnt = IFI.insurance_file_cnt 
			AND F.party_cnt = P.party_cnt
			AND P.party_type_id = PT.party_type_id 			
			AND PT.description = 'Fee Account')				
	       ,0
	       )			Fees,
		ISNULL( (SELECT SUM(F.fee_amount)
 			FROM Policy_fee	 F,
 			     Party P, 
 			     Party_Type PT
			WHERE F.insurance_file_Cnt = IFI.insurance_file_cnt 
			AND F.party_cnt = P.party_cnt
			AND P.party_type_id = PT.party_type_id 			
			AND PT.description = 'Discount Account')				
	       ,0
	       )				Discount,
		ISNULL( (SELECT SUM(A.agent_commission_value)
 			FROM Policy_Agents 	A,
			     Party_Agent	AG,
			     Party_Agent_type   AGT 
			WHERE A.insurance_file_Cnt = IFI.insurance_file_cnt
			AND A.agent_cnt = AG.party_cnt 
			AND AG.party_agent_type_id = AGt.party_agent_type_id
			AND AGT.description = 'SUB AGENT'
			)				
	       ,0
	       )				SubAgent,
		ISNULL( (SELECT SUM(A.agent_commission_value)
 			FROM Policy_Agents 	A,
			     Party_Agent	AG,
			     Party_Agent_type   AGT 
			WHERE A.insurance_file_Cnt = IFI.insurance_file_cnt
			AND A.agent_cnt = AG.party_cnt 
			AND AG.party_agent_type_id = AGt.party_agent_type_id
			AND AGT.description = 'AGENT'
			)				
	       ,0
	       )				 Agent,
	RSC.description			warning_message,
	(CASE @InsuranceFileAccountExec WHEN '1' THEN ISNULL(PY5.shortname, '') else ISNULL(PY3.shortname, '') end)	Executive_code,  --mkw130603
	(CASE @InsuranceFileAccountExec WHEN '1' THEN ISNULL(PY5.resolved_name, '') else ISNULL(PY3.resolved_name, '') end)	Executive_name,  --mkw130603
--	ISNULL(PY3.shortname, '')	Executive_code,
--	ISNULL(PY3.resolved_name, '')	Executive_name,
	ISNULL(PY4.shortname, '')	Handler_code,
	ISNULL(PY4.resolved_name, '')	Handler_name 
	 
FROM insurance_file IFI
JOIN insurance_folder IFO
    ON IFO.insurance_folder_cnt = IFI.insurance_folder_cnt
JOIN party PY1
    ON PY1.party_cnt = IFO.insurance_holder_cnt
JOIN party PY2
    ON PY2.party_cnt = IFI.lead_insurer_cnt
LEFT JOIN party PY4
    ON PY4.party_cnt = IFI.account_handler_cnt
LEFT JOIN party PY3
    ON PY3.party_cnt = PY1.consultant_cnt
LEFT JOIN Party PY5
    ON PY5.party_cnt = IFI.account_executive_cnt
JOIN risk_code RC
    ON RC.risk_code_id = IFI.risk_code_id
JOIN risk_group RG
    ON RG.risk_group_id = RC.risk_group_id
LEFT JOIN renewal_stop_code RSC
    ON RSC.renewal_stop_code_id = IFI.renewal_stop_code_id
LEFT JOIN business_type BT 
	ON IFI.business_type_id = BT.business_type_id

WHERE PY1.party_type_id IN (1, 2, 4) /*Personal, Group and Corporate Clients*/
AND IFI.insurance_file_type_id IN (2, 5, 6,7) /*Live Policy, Permanent MTA, Temporary MTA and Temporary MTA Quotation*/
AND ISNULL(IFI.insurance_file_status_id, 3) = 3 /*Live and Under Renewal*/
AND ISNULL(IFI.policy_ignore, 0) <> 1
AND IFI.policy_version =
    (
        SELECT
            MAX(policy_version)
        FROM insurance_file
        WHERE insurance_folder_cnt = IFO.insurance_folder_cnt
        AND insurance_file_type_id NOT IN ( 3,4) /*Policy Under Renewal and MTA Quotation*/
    )
AND	(
		@iBranchID = 0
		OR
		(
			@iBranchID <> 0
			AND
			IFI.source_id = @iBranchID
		)	
	)
AND (@sBusiness_Type = '' 
     OR (@sBusiness_Type <> '' AND BT.code = @sBusiness_Type)
     )

ORDER BY
	IFI.renewal_date,
	PY1.Shortname

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

