SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_SIR_Get_BackdatedRiskVersions'
GO

CREATE PROCEDURE spu_SIR_Get_BackdatedRiskVersions
	@insurance_file_cnt int  
AS  
BEGIN  

SELECT 	iFile.insurance_file_cnt,
	iFile.policy_version,
	ift.description,
	iFile.cover_start_date,
	iFile.expiry_date expiry_date,
	R.total_this_premium,
	CASE IFT.code	
		WHEN 'MTACAN' THEN 'REPLACED'
		ELSE CASE UPPER(IsNull(IFS.description,''))
				WHEN 'CANCELLED' THEN 'REPLACED'
				ELSE IsNull(IFS.description,'Live') END
	END [PolicyStatusDescription],
	R.risk_cnt,
	R.description [RiskDescription],
	RT.description [RiskTypeDescription],
	RT.risk_type_id,
	RT.gis_screen_id,
	iFile.insurance_folder_cnt,
	iFile.product_id,
	P.party_cnt,
	P.shortname,
	RS.description,
	Curr.code,
	IFRL.status_flag,
    r.risk_folder_cnt
FROM	Insurance_File iFile   
JOIN	Insurance_File_Type IFT ON iFile.insurance_file_type_id=IFT.insurance_file_type_id
JOIN	Insurance_File_Risk_Link IFRL ON iFile.insurance_file_cnt = IFRL.insurance_file_cnt
																	--	AND	IFRL.status_flag <> 'D'
JOIN	Party P ON P.party_cnt = iFile.insured_cnt
JOIN	Risk R ON R.risk_cnt = IFRL.risk_cnt
JOIN	Risk_Type RT ON RT.risk_type_id = R.risk_type_id
JOIN	Risk_Status RS ON RS.risk_status_id = R.risk_status_id
LEFT OUTER JOIN Insurance_File_Status IFS on 
	iFile.insurance_file_status_id=IFS.insurance_file_status_id  
left outer join Currency Curr on  iFile.currency_id = Curr.currency_id   
WHERE	base_insurance_file_cnt = @insurance_file_cnt
END  
