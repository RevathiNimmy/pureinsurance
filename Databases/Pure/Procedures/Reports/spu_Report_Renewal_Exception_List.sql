SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Renewal_Exception_List'
GO
CREATE PROCEDURE spu_Report_Renewal_Exception_List	
	

	@branch_id	int

AS

DECLARE	@iBranchID	int
SELECT	@iBranchID = ISNULL(@branch_id, 0)


 	CREATE TABLE #RenewalTemp
 	(
		client_code		char(20),
		client_name		varchar(255),
		policy_number		varchar(30) NULL,
		insurer_name	 	varchar(255) NULL,
		insurer_code	 	varchar(30) NULL,
		risk_group	 	varchar(255) NULL,	
		date_from	 	datetime NULL,
		date_to		 	datetime NULL,	
		risk_code	 	char(10) NULL,
		risk_description 	varchar(255) NULL,
		this_premium		numeric(19, 4) NULL,
		Commission_amount 	numeric(19, 4) NULL,
		Commission_percentage 	numeric(19, 4) NULL,
		warning_message		varchar(255) NULL,
		executive_code		char(20) NULL,
		executive_name		varchar(255),
		handler_code		char(20) NULL,
		handler_name		varchar(255)NULL
	)
 	INSERT INTO #RenewalTemp 

 
	SELECT 	PY1.Shortname,
		PY1.resolved_name, 
 		IFI.insurance_ref, 
 		ISNULL(PY2.resolved_name, ''),
 		ISNULL(PY2.shortname, 'NO INSURER'),
 		ISNULL (RG.description,'NO RISK GROUP'),
 		IFI.cover_start_date,
 		IFI.renewal_date,
 		ISNULL(RC.code, ''),
 		ISNULL(RC.description, 'NO RISK CODE'),
 		IFI.this_premium,
 		IFI.commission_amount,
 		IFI.commission_percentage, 
 		RSC.description,
 		ISNULL(PY3.shortname, ''),
 		ISNULL(PY3.resolved_name, ''), 
 		ISNULL(PY4.shortname, ''),
 		ISNULL(PY4.resolved_name, '')	 
		FROM	party 			PY1
		JOIN insurance_folder	IFO
			ON PY1.party_cnt = IFO.insurance_holder_cnt
 		JOIN insurance_file		IFI 
 			ON IFO.insurance_folder_cnt = IFI.insurance_folder_cnt
 		LEFT OUTER JOIN party 			PY2 
 			ON IFI.lead_insurer_cnt = PY2.party_cnt
 		LEFT OUTER JOIN party 			PY3 
 			ON PY1.consultant_cnt = PY3.party_cnt 
 		LEFT OUTER JOIN party			PY4
 			ON IFI.account_handler_cnt = PY4.party_cnt
 		LEFT OUTER JOIN risk_code		RC 
 			ON IFI.risk_code_id = RC.risk_code_id	
 		LEFT OUTER JOIN risk_group 		RG
 			ON RC.risk_group_id = RG.risk_group_id
 		LEFT OUTER JOIN renewal_stop_code			RSC
 			ON IFI.renewal_stop_code_id = RSC.renewal_stop_code_id 
	 
	WHERE	 
		PY1.party_type_id IN (1, 2, 4)
 	AND	IFI.insurance_file_type_id IN (2, 3, 5, 6)
 	AND	IFI.insurance_file_status_id IS NULL
 	AND	ISNULL(IFI.policy_ignore, 0) <> 1
  	AND	IFI.insurance_file_cnt = 
 	(
 		SELECT	MAX(insurance_file_cnt)
 		FROM	Insurance_File
 		WHERE	insurance_folder_cnt = IFO.insurance_folder_cnt
 		and 	insurance_file_type_id <> 4
 	)
	AND	(
		@iBranchID = 0
		OR
		(
			@iBranchID <> 0
			AND
			PY1.source_id = @iBranchID
		)	
		)

 	ORDER BY
 		IFI.renewal_date,
 		PY1.Shortname

	 

 	SELECT * FROM	#RenewalTemp
  	WHERE insurer_code = 'NO INSURER'
	OR    risk_description = 'NO RISK CODE'
	OR    risk_group = 'NO RISK GROUP'   
	 

 	DROP TABLE #RenewalTemp 
	 
 	
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

