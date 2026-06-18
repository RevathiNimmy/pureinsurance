SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Tax_Types_and_Bands'
GO

CREATE PROCEDURE spu_Get_Tax_Types_and_Bands    
	@tax_group_id int = null,    
	@effective_date datetime = null,    
	@transtype varchar(7) = null,    
 @insurance_file_cnt int = null    
AS    
 DECLARE @system_option AS VARCHAR(20)    
 DECLARE @policy_version AS INTEGER   
		

 SELECT  @system_option='0'    
	
 SELECT  @system_option=value    
 FROM System_Options    
 WHERE option_number=5019 AND branch_id=1    
	
 IF (@system_option='1' OR @system_option='2' OR @system_option='3') AND @insurance_file_cnt IS NOT NULL
  BEGIN
  -- Get details from insurance file
					SELECT
					@effective_date = CASE WHEN @system_option='3' THEN IFI.inception_date_tpi ELSE ifi.cover_start_date END,
					@policy_version = ifi.policy_version
				   FROM    insurance_file ifi
				   WHERE   ifi.insurance_file_cnt = @Insurance_File_Cnt
		
			declare @TransactionType as Varchar(10)
				
			BEGIN
				SELECT  @TransactionType = CASE WHEN SUM(p.this_premium) < 0 THEN 'RMTA' ELSE 'AMTA' END
				FROM    insurance_file_risk_link ifrl
				JOIN     insurance_file ifi ON ifi.insurance_file_cnt = ifrl.insurance_file_cnt
				JOIN    risk r ON r.risk_cnt = ifrl.risk_cnt
				JOIN    peril p ON p.risk_cnt = r.risk_cnt
				WHERE   ifrl.insurance_file_cnt = @insurance_file_cnt
				AND     ifrl.status_flag <> 'U'
				AND     r.is_risk_selected = 1
				AND     p.is_premium = 1
			END


		IF @TransactionType = 'RMTA'
		  BEGIN

				 -- Get details from insurance file
				   DECLARE @prev_insurance_file_cnt INT, @curr_risk_cnt  INT , @insurance_folder_cnt	 INT
				   DECLARE @prev_effective_date datetime

				   SELECT  @prev_effective_date =inception_date_tpi ,@insurance_folder_cnt=insurance_folder_cnt
				   FROM Insurance_File WHERE insurance_file_cnt=  @Insurance_File_Cnt

					SELECT TOP 1 @prev_insurance_file_cnt=insurance_file_cnt From Insurance_File
				   WHERE insurance_folder_cnt=@insurance_folder_cnt AND cover_start_date=@prev_effective_date

					IF EXISTS(SELECT
						1
					 FROM
					insurance_file_risk_link ifrl
				   JOIN risk r
					ON r.risk_cnt = ifrl.risk_cnt
				   JOIN peril p
					ON p.risk_cnt = r.risk_cnt
				   JOIN tax_group_tax_band tgtb
					ON tgtb.tax_group_id  = p.tax_group
				   JOIN tax_band tb
					ON tb.tax_band_id = tgtb.tax_band_id
				   JOIN tax_type tt
					ON tt.tax_type_id = tb.tax_type_id
				   JOIN tax_band_rate tbr
					ON tbr.tax_band_id = tb.tax_band_id
				   JOIN Class_Of_Business cob
					ON cob.class_of_business_id = p.class_of_business_id
				   WHERE ifrl.insurance_file_cnt = @prev_insurance_file_cnt
					AND tbr.effective_date <= @prev_effective_date
					AND tbr.use_for_refund_when_expired = 1
					AND tbr.is_deleted = 0
					AND tb.is_deleted = 0
					AND (cob.class_of_business_id = tbr.class_of_business_id
					OR ISNULL(tbr.class_of_business_id, 0) = 0)
									)
					BEGIN
					SELECT @effective_date = @prev_effective_date
					END
			 END
					IF @system_option = '2' OR @system_option = '3'
						BEGIN
									  SELECT @effective_date = ifi.inception_date_tpi
									  FROM   insurance_file ifi
									  WHERE  ifi.insurance_file_cnt = @Insurance_File_Cnt
						
						END
		 END

 ELSE
 
  BEGIN
   -- Effective date should be transaction date rather than cover start date
   SET @effective_date = ISNULL(@effective_date, GETDATE())
  END
	
	IF @tax_group_id IS NULL    
	
		SELECT  tt.tax_type_id,    
			 tt.description,    
			 tb.tax_band_id,    
			 tb.description,    
			 tbr.is_value,    
			 tbr.rate,    
			 tbr.currency_id,    
			 tt.code,    
			 1 sequence, -- no group so no cumulative taxes    
	  tbr.allow_tax_credit,    
	  tbr.country_id,    
	  tbr.state_id,    
  tbr.class_of_business_id, 
  tbr.Calc_Basis,   
  tbr.tax_band_rate_id,    
  tbr.is_suspended,  
  tt.is_include_tax_in_instalments -- PN 77923    
		FROM tax_type tt    
		JOIN    tax_band tb    
				ON tb.tax_type_id = tt.tax_type_id    
		JOIN    tax_band_rate tbr    
				ON tbr.tax_band_id = tb.tax_band_id    
		WHERE   tt.is_deleted = 0    
			AND tt.effective_date <= @effective_date    
			AND tb.is_deleted = 0    
			AND tb.effective_date <= @effective_date    
			AND tbr.tax_band_rate_id =         -- Ensure we only get one rate for the band!!!    
			   (SELECT  TOP 1 tax_band_rate_id    
				FROM    tax_band_rate tbr2    
				WHERE   tbr2.tax_band_id = tb.tax_band_id    
					AND tbr2.is_deleted = 0    
					AND tbr2.effective_date <= @effective_date    
					AND CASE @TransType    
		 WHEN 'TTRITP'  THEN tbr2.TTRI    
		 WHEN 'TTRIFP'  THEN tbr2.TTRI    
						   WHEN 'TTRI'  THEN tbr2.TTRI    
						   WHEN 'TTRITC' THEN tbr2.TTRIC    
						   WHEN 'TTRIFC' THEN tbr2.TTRIC    
						   WHEN 'TTRIC' THEN tbr2.TTRIC    
						   WHEN 'TTAC' THEN tbr2.TTAC    
						   WHEN 'TTF' THEN tbr2.TTF    
						   WHEN 'TTCP' THEN tbr2.TTCP    
						   WHEN 'TTCS' THEN tbr2.TTCS    
						   WHEN 'TTCR' THEN tbr2.TTCR    
						   WHEN 'TTI' THEN tbr2.TTI    
						   WHEN 'TTRIPR' THEN tbr2.TTRIPR    
						   ELSE 1    
						END > 0    
				ORDER BY    
						tbr2.effective_date DESC)    
		ORDER BY    
				tt.description,    
				tb.description    
	
	ELSE    
	
		SELECT  tt.tax_type_id,    
			 tt.description,    
			 tb.tax_band_id,    
			 tb.description,    
			 tbr.is_value,    
			 tbr.rate,    
			 tbr.currency_id,    
			 tt.code,    
			 tgtb.sequence,    
	  tbr.allow_tax_credit,    
	  tbr.country_id,    
	  tbr.state_id,    
	  tbr.class_of_business_id,
	  tbr.Calc_Basis,    
	  tbr.tax_band_rate_id,    
	  tbr.is_suspended,  
	  tt.is_include_tax_in_instalments -- PN 77923    
		FROM tax_type tt    
		JOIN    tax_band tb    
				ON tb.tax_type_id = tt.tax_type_id    
		JOIN    tax_band_rate tbr    
				ON tbr.tax_band_id = tb.tax_band_id    
		JOIN    tax_group_tax_band tgtb    
				ON tgtb.tax_band_id = tb.tax_band_id    
		WHERE   tt.is_deleted = 0    
			AND tt.effective_date <= @effective_date    
			AND tb.is_deleted = 0    
			AND tb.effective_date <= @effective_date    
			AND tbr.tax_band_rate_id =         -- Ensure we only get one rate for the band!!!    
			   (SELECT  TOP 1 tax_band_rate_id    
			 FROM    tax_band_rate tbr2    
				WHERE   tbr2.tax_band_id = tb.tax_band_id    
					AND tbr2.is_deleted = 0    
					AND tbr2.effective_date <= @effective_date    
					AND CASE @TransType    
						   WHEN 'TTRITP'  THEN tbr2.TTRI    
	  WHEN 'TTRIFP'  THEN tbr2.TTRI    
						   WHEN 'TTRI'  THEN tbr2.TTRI    
						   WHEN 'TTRITC' THEN tbr2.TTRIC    
						   WHEN 'TTRIFC' THEN tbr2.TTRIC    
						   WHEN 'TTRIC' THEN tbr2.TTRIC    
						   WHEN 'TTAC' THEN tbr2.TTAC    
						   WHEN 'TTF' THEN tbr2.TTF    
						   WHEN 'TTCP' THEN tbr2.TTCP    
						   WHEN 'TTCS' THEN tbr2.TTCS    
						   WHEN 'TTCR' THEN tbr2.TTCR    
						   WHEN 'TTI' THEN tbr2.TTI    
	  WHEN 'TTRIPR' THEN tbr2.TTRIPR    
						   ELSE 1    
						END > 0    
				ORDER BY    
						tbr2.effective_date DESC)    
			AND tgtb.tax_group_id = @tax_group_id    
		ORDER BY    
				tgtb.sequence,    
				tt.description,    
				tb.description 



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
