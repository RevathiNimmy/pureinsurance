SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO
EXECUTE DDLDropProcedure 'spu_Report_Commission_Statement'
GO

CREATE PROCEDURE spu_Report_Commission_Statement (  
	@batch_id VARCHAR(10),  
	@Start_date NVARCHAR(50),  
    @is_preview BOOLEAN = False
) AS  BEGIN

	SET NOCOUNT ON
	SELECT @Start_Date = CONVERT(varchar(60), CONVERT(DATETIME, @Start_date, 103))  
	IF  UPPER(@batch_id)='ALL'  BEGIN
	
		SELECT td.account_id AS account_id,  
		td.account_currency_id AS currency_id,  
		a.account_name AS account_name,  
		CONVERT(VARCHAR(255),P_C1.resolved_name) AS agent_name,  
		CONVERT(VARCHAR(255),p_c.resolved_name) AS client,  
		ifi.insurance_ref AS policy_ref,  
		pfin.InstalmentNumber AS instalment_number,  
		pfin.DueDate AS instalment_date,  
		d.document_ref AS document_ref,  
		--@statement_date AS statement_date,  
		@Start_date AS statement_date,  
		Case @is_preview 
			WHEN 1 THEN  td.outstanding_account_amount
			ELSE td.account_amount
		END outstanding_account_amount,					
		'CMS' + Convert(varchar(10),td.commission_payment_batch_id) AS batch_ref,  
		c.code AS Currency_Code,  
		AD.address1,  
		AD.address2,  
		AD.address3,  
		AD.address4,  
		AD.postal_code,  
		(SELECT TOP 1 RISK.DESCRIPTION  
		 FROM RISK 
		 INNER JOIN INSURANCE_FILE_RISK_LINK ON INSURANCE_FILE_RISK_LINK.RISK_CNT = RISK.RISK_CNT  
		 WHERE INSURANCE_FILE_RISK_LINK.INSURANCE_FILE_CNT= ifi.insurance_file_cnt
		 AND ISNULL(RISK.Is_Mandatory_Risk, 0) <> 1
		) AS policy_reference  
		FROM  TransDetail TD with (nolock) 		 
		LEFT JOIN  Released_Accounts_Transactions rat 	ON TD.transdetail_id=rat.destination_transdetail_id
		LEFT JOIN suspended_accounts_transactions sat 	ON rat.suspended_transdetail_id=sat.suspended_transdetail_id   
		INNER JOIN  document d with (nolock)  			On d.document_id = td.document_id  
		Left Join   insurance_file ifi with (nolock)  	On ifi.insurance_file_cnt = d.insurance_file_cnt  
		Left Join  	insurance_folder ifo with (nolock)  On ifo.insurance_folder_cnt = ifi.insurance_folder_cnt  
		Left Join  	party p_c with (nolock)				On p_c.party_cnt = ifo.insurance_holder_cnt  	-- Client party  		
		Left Join  	pfpremiumfinance pfpf with (nolock) ON d.insurance_file_cnt=pfpf.insurance_file_cnt  
		Left Join  	pfinstalments pfin with (nolock)  	ON rat.pfinstalments_id=pfin.pfinstalments_id  
		INNER Join  account a  							On a.account_id = td.account_id  
		LEFT JOIN   party P_C1 							ON a.account_key = P_C1.party_cnt  
		INNER JOIN  currency c with (nolock)  			On c.currency_id = td.account_currency_id  
		LEFT JOIN  	Party_Address_Usage PAU  			ON P_C1.party_cnt =  PAU.party_cnt and PAU.address_cnt=(select top 1 address_cnt from  Party_Address_Usage where PAU.party_cnt=Party_Address_Usage.party_cnt order by address_cnt desc )
		LEFT JOIN  	address AD 							ON PAU.address_cnt = AD.address_cnt  
		LEFT JOIN  	pfinstalments_Status pfin_Status    ON pfin.status = pfin_Status.PFInstalments_Status_id  
		WHERE TD.commission_payment_batch_id is not null
		AND (ISNULL(pfin_Status.code,'') IN ('','C'))  
		ORDER BY pfin.InstalmentNumber
	
	END ELSE BEGIN 
	
		Select @batch_id = batch_id From batch where batch_ref = @batch_id  
		SELECT td.account_id AS account_id,  
		td.account_currency_id AS currency_id,  
		a.account_name AS account_name,  
		CONVERT(VARCHAR(255),P_C1.resolved_name) AS agent_name,  
		CONVERT(VARCHAR(255),p_c.resolved_name) AS client,  
		ifi.insurance_ref AS policy_ref,  
		pfin.InstalmentNumber AS instalment_number,  
		pfin.DueDate AS instalment_date,  
		d.document_ref AS document_ref,  
		--@statement_date AS statement_date,  
		@Start_date AS statement_date,  
		Case @is_preview 
			WHEN 1 THEN  td.outstanding_account_amount
			ELSE td.account_amount
		END outstanding_account_amount,				
		'CMS' + Convert(varchar(10),td.commission_payment_batch_id) AS batch_ref,  
		c.code AS Currency_Code,  
		AD.address1,  
		AD.address2,  
		AD.address3,  
		AD.address4,  
		AD.postal_code,  
		(SELECT TOP 1 RISK.DESCRIPTION  
		FROM RISK INNER JOIN INSURANCE_FILE_RISK_LINK ON INSURANCE_FILE_RISK_LINK.RISK_CNT = RISK.RISK_CNT  
		WHERE INSURANCE_FILE_RISK_LINK.INSURANCE_FILE_CNT= ifi.insurance_file_cnt
		AND ISNULL(RISK.Is_Mandatory_Risk, 0) <> 1
		) AS policy_reference  
		FROM  TransDetail TD with (nolock)	 
		LEFT JOIN  	Released_Accounts_Transactions rat 		ON TD.transdetail_id=rat.destination_transdetail_id
		LEFT JOIN 	suspended_accounts_transactions sat 	ON rat.suspended_transdetail_id=sat.suspended_transdetail_id  
		INNER JOIN  document d with (nolock)  				On d.document_id = td.document_id  
		Left Join  	insurance_file ifi with (nolock)  		On ifi.insurance_file_cnt = d.insurance_file_cnt  
		Left Join  	insurance_folder ifo with (nolock)  	On ifo.insurance_folder_cnt = ifi.insurance_folder_cnt  
		Left Join  	party p_c with (nolock)					On p_c.party_cnt = ifo.insurance_holder_cnt  -- Client party  
		Left Join  	pfpremiumfinance pfpf with (nolock)  	ON d.insurance_file_cnt=pfpf.insurance_file_cnt  
		Left Join  	pfinstalments pfin with (nolock)  		ON rat.pfinstalments_id=pfin.pfinstalments_id  
		INNER Join  account a  								On a.account_id = td.account_id  
		LEFT JOIN   party P_C1 								ON a.account_key = P_C1.party_cnt  
		INNER JOIN  currency c with (nolock)  				On c.currency_id = td.account_currency_id  
		LEFT JOIN  	Party_Address_Usage PAU  				ON P_C1.party_cnt =  PAU.party_cnt  and PAU.address_cnt=(select top 1 address_cnt from  Party_Address_Usage where PAU.party_cnt=Party_Address_Usage.party_cnt order by address_cnt desc )
		LEFT JOIN  	address AD  							ON PAU.address_cnt = AD.address_cnt  
		LEFT JOIN  	pfinstalments_Status pfin_Status  		ON pfin.status = pfin_Status.PFInstalments_Status_id  
		WHERE TD.commission_payment_batch_id = @batch_id  
		AND (ISNULL(pfin_Status.code,'') IN ('','C'))
		ORDER BY pfin.InstalmentNumber
	END
END
GO



