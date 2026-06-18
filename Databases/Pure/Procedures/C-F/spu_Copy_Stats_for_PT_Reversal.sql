

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Copy_Stats_for_PT_Reversal'
GO

CREATE PROCEDURE spu_Copy_Stats_for_PT_Reversal
@PTInsuranceFileCnt  INT,
@StatsFolderCnt INT
AS  
Declare @oldStatsFolderCnt INT,
		@risk_cnt INT,
		@risk_folder_cnt INT,
		@pro_rata_rate float,
		@new_pro_rata_rate float,
		@insurance_file_cnt INT
		
SELECT @oldStatsFolderCnt=Stats_Folder_cnt from Stats_Folder WHERE insurance_file_cnt=@PTInsuranceFileCnt AND transaction_type_code  NOT IN ('C_CO','C_CR')

SELECT @insurance_file_cnt=insurance_file_cnt from Stats_Folder WHERE stats_folder_cnt=@StatsFolderCnt


DECLARE curRisks CURSOR FOR 
	SELECT r.risk_cnt,pro_rata_rate,risk_folder_cnt from insurance_file_risk_link ifrl 
	JOIN risk r ON ifrl.risk_cnt=r.risk_cnt WHERE ISNULL(original_risk_cnt,0)<>0 AND insurance_file_cnt=@PTInsuranceFileCnt
	
OPEN curRisks 

FETCH NEXT FROM curRisks INTO @risk_cnt,@pro_rata_rate,@risk_folder_cnt
WHILE @@FETCH_STATUS=0
BEGIN
	IF @pro_rata_rate<>0
	BEGIN
		
		SELECT @new_pro_rata_rate=pro_rata_rate from insurance_file_risk_link ifrl 
		JOIN risk r ON ifrl.risk_cnt=r.risk_cnt WHERE insurance_file_cnt=@insurance_file_cnt and risk_folder_cnt=@risk_folder_cnt
	
		Insert Into Stats_Detail
		([stats_folder_cnt]
           ,[stats_detail_id]
           ,[stats_detail_type]
           ,[risk_id]
           ,[risk_type_id]
           ,[risk_type_code]
           ,[peril_id]
           ,[peril_description]
           ,[peril_type_id]
           ,[peril_type_code]
           ,[policy_section_type_id]
           ,[policy_section_type_code]
           ,[class_of_business_id]
           ,[class_of_business_code]
           ,[tax_type_id]
           ,[tax_type_code]
           ,[tax_value]
           ,[ri_party_cnt]
           ,[ri_shortname]
           ,[ri_party_type]
           ,[ri_share_percent]
           ,[ri_agreement_code]
           ,[annual_premium]
           ,[currency_code]
           ,[currency_rate]
           ,[this_premium_original]
           ,[this_premium_home]
           ,[commission_percent]
           ,[lead_commission_value_home]
           ,[sub_commission_value_home]
           ,[sum_insured_home]
           ,[sum_insured_currency_code]
           ,[sum_insured_change]
           ,[transaction_ledger_id]
           ,[transaction_account_id]
           ,[account_type_code]
           ,[ceded_ref]
           ,[cover_share_percent]
           ,[sum_insured_total]
           ,[charges_total]
           ,[taxes_total]
           ,[recoveries_total]
           ,[commission_excluded]
           ,[withholding_tax_excluded]
           ,[purchase_order_no]
           ,[purchase_invoice_no]
           ,[stats_version]
           ,[this_premium_system]
           ,[lead_commission_value_system]
           ,[sub_commission_value_system]
           ,[sum_insured_system]
           ,[is_commission_modified]
           ,[original_flag]
           ,[cover_to_date]
           ,[Claim_RI_Only_Amendment]
           ,[Earning_Pattern_id]
           ,[ri_arrangement_line_Id])
		Select @StatsFolderCnt,stats_detail_id,stats_detail_type,risk_id,risk_type_id,risk_type_code,
				peril_id,peril_description,peril_type_id,peril_type_code,policy_section_type_id,policy_section_type_code,
				class_of_business_id,class_of_business_code,tax_type_id,tax_type_code,-tax_value,ri_party_cnt,ri_shortname,
				ri_party_type,ri_share_percent,ri_agreement_code,-annual_premium,currency_code,currency_rate,
				-this_premium_original*@new_pro_rata_rate/@pro_rata_rate,-this_premium_home*@new_pro_rata_rate/@pro_rata_rate,commission_percent,-lead_commission_value_home*@new_pro_rata_rate/@pro_rata_rate,-sub_commission_value_home,
				-sum_insured_home,sum_insured_currency_code,-sum_insured_change,transaction_ledger_id,transaction_account_id,account_type_code,ceded_ref,
				cover_share_percent,-sum_insured_total,-charges_total,-taxes_total,-recoveries_total,commission_excluded,withholding_tax_excluded,purchase_order_no,
				purchase_invoice_no,stats_version,-this_premium_system*@new_pro_rata_rate/@pro_rata_rate,-lead_commission_value_system*@new_pro_rata_rate/@pro_rata_rate,-sub_commission_value_system,-sum_insured_system,is_commission_modified,
				original_flag,cover_to_date,Claim_RI_Only_Amendment,Earning_Pattern_id,ri_arrangement_line_Id
		 from Stats_Detail  WHERE Stats_Folder_cnt=@oldStatsFolderCnt and ri_arrangement_line_Id is not null
		 and risk_id = @risk_cnt
	END 
	FETCH NEXT FROM curRisks INTO @risk_cnt,@pro_rata_rate,@risk_folder_cnt
 
END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


