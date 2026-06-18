SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_add_stats_details_comm'
GO


CREATE PROCEDURE spu_add_stats_details_comm    
    @stats_folder_cnt int    
AS    
    
/****************************************************************************************/    
/* sp_add_stats_details_commission creates stats details records for                    */    
/* sub-agent commission by product/class/peril.  (should this be by Product ?)         */    
/*                                                                                      */    
/* 1 parameter is passed in - @stats_folder_cnt                               */    
/*                                                                                      */    
/* This stored procedure is called by sp_add_stats_details_control.                 */    
/*                                                                                      */    
/* A failure in this procedure will be passed back to the calling procedure.           */    
/****************************************************************************************/    
    
BEGIN    
    
/* Declare variable for all columns in stats_details table */    
DECLARE @stats_detail_id int,    
     @stats_detail_type char(3),    
     @risk_id int,    
     @risk_type_id int,    
     @risk_type_code char(10),    
     @peril_id int,    
     @peril_description varchar(30),    
     @peril_type_id int,    
     @peril_type_code char(10),    
     @policy_section_type_id int,    
     @policy_section_type_code char(10),    
     @class_of_business_id int,    
     @class_of_business_code char(10),    
     @tax_type_id int,    
     @tax_type_code char(10),    
     @tax_value money,    
     @ri_party_cnt int,    
     @ri_shortname varchar(20),    
     @ri_party_type char(3),    
     @ri_share_percent numeric(12, 8),    
     @ri_agreement_code varchar(20),    
     @annual_premium numeric(19, 4),    
     @currency_code char(10),    
     @currency_id int,    
     @currency_rate numeric(19, 8),    
     @system_rate numeric(19, 8),    
     @this_premium_original numeric(19, 4),    
     @this_premium_home numeric(19, 4),    
     @this_premium_system numeric(19, 4),    
     @commission_percent numeric(12, 8),    
     @lead_commission_value_home numeric(19, 4),    
     @lead_commission_value_system numeric(19, 4),    
     @sub_commission_value_home numeric(19, 4),    
     @sub_commission_value_system numeric(19, 4),    
     @sum_insured_home numeric(19, 4),    
     @sum_insured_system numeric(19, 4),    
     @sum_insured_currency_code char(10),    
     @sum_insured_change numeric(19, 4),    
     @transaction_ledger_id char(2),    
     @transaction_account_id int,    
     @account_type_code char(10),    
     @ceded_ref char(10),    
     @cover_share_percent numeric(12, 8),    
     @sum_insured_total numeric(19, 4),    
     @charges_total numeric(19, 4),    
     @taxes_total numeric(19, 4),    
     @recoveries_total numeric(19, 4),    
     @commission_excluded numeric(19, 4),    
     @withholding_tax_excluded numeric(19, 4),    
     @cob_id  INT,
	 @Decimal_Places tinyint  
    
DECLARE @gross_count int,    
  @gross_row int,    
  @total_premium numeric(19,4),    
  @this_sub_commission numeric(19,4),    
  @this_sub_commission_home numeric(19,4),    
  @this_sub_commission_system numeric(19,4),    
  @remain_sub_commission numeric(19,4),    
  @agent varchar(20)    
    
DECLARE @insurance_file_cnt INT    
DECLARE @company_id INT    
DECLARE @return_status INT   
declare @document_ref	varchar(25)
    
/* Get the count of the Gross lines */    
SELECT @gross_count=COUNT(*),    
  @total_premium=SUM(this_premium_original)    
FROM Stats_Detail    
WHERE stats_folder_cnt=@stats_folder_cnt    
AND  stats_detail_type='GRS' /* Open the Gross Cursor */    
    
/*Get insurance file*/    
SELECT @insurance_file_cnt=insurance_file_cnt
,@document_ref = document_ref    
FROM  stats_folder    
WHERE  stats_folder_cnt = @stats_folder_cnt    
    
/*Get details from insurance file*/    
SELECT    
 @company_id = source_id,    
 @currency_id = currency_id,    
 @currency_rate = currency_base_xrate,    
 @system_rate = system_base_xrate    
FROM insurance_file    
WHERE insurance_file_cnt = @insurance_file_cnt    

if substring(LTRIM(RTRIM(@document_ref)),0,4) ='SDD'
begin
set @currency_rate = 0
end 

/*Get details about the currency*/    
SELECT    
 @currency_code = code ,@Decimal_Places=ISNULL(decimal_places,0)   
FROM currency    
WHERE currency_id = @currency_id    
    
/* Declare commission cursor */    
DECLARE c_commission CURSOR FAST_FORWARD FOR    
    SELECT  P.shortname,    
   AC.commission_value,    
   AC.class_of_business_id    
    FROM    Stats_Folder        SF    
 INNER JOIN Agent_Commission AC    
    ON  AC.insurance_file_cnt=SF.insurance_file_cnt    
 INNER JOIN Party P    
 ON  P.party_cnt=AC.party_cnt    
    WHERE   SF.stats_folder_cnt = @stats_folder_cnt    
 AND  AC.is_lead_agent=0    
    
/* Open the Commission Cursor */    
OPEN c_commission    
FETCH NEXT FROM c_commission INTO @agent, @sub_commission_value_home, @cob_id    
    
/* Get the column values */    
WHILE (@@FETCH_STATUS = 0)    
BEGIN    
 /* Get the Gross total and split between Risk */    
 DECLARE c_gross CURSOR FAST_FORWARD FOR    
     SELECT  risk_id,    
                   risk_type_id,    
                   risk_type_code,    
                   peril_id,    
                   peril_description,    
                   peril_type_id,    
                   peril_type_code,    
                   policy_section_type_id,    
                   policy_section_type_code,    
                   class_of_business_id,    
                   class_of_business_code,    
     annual_premium,    
     this_premium_original,    
     this_premium_home,    
     this_premium_system    
  FROM  Stats_Detail    
  WHERE  stats_folder_cnt=@stats_folder_cnt    
  AND   stats_detail_type='GRS'    
  AND  class_of_business_id = CASE WHEN ISNULL(@cob_id,0) <> 0 THEN  @cob_id ELSE class_of_business_id END    
    
 SELECT @gross_row=1    
 SELECT @remain_sub_commission=@sub_commission_value_home  
 If ISNULL(@cob_id,0) <> 0    
 BEGIN    
  --SET @gross_row = @gross_count    
  SELECT @total_premium=SUM(this_premium_original)    
 FROM Stats_Detail    
 WHERE stats_folder_cnt=@stats_folder_cnt    
 AND  stats_detail_type='GRS'  
 and class_of_business_id = @cob_id  
 END    
    
  
 OPEN c_gross    
 FETCH NEXT FROM c_gross INTO    
   @risk_id,    
   @risk_type_id,    
   @risk_type_code,    
   @peril_id,    
   @peril_description,    
   @peril_type_id,    
   @peril_type_code,    
   @policy_section_type_id,    
   @policy_section_type_code,    
   @class_of_business_id,    
   @class_of_business_code,    
   @annual_premium,    
   @this_premium_original,    
   @this_premium_home,    
   @this_premium_system    
    
 /* Get the column values */    
 WHILE (@@FETCH_STATUS = 0)    
 BEGIN    
  /* Calculate the percentage of this sub agent commission for this Gross line */    
  IF (@gross_row=@gross_count)    
   SELECT @this_sub_commission=@remain_sub_commission    
  ELSE    
  BEGIN    
   SELECT @this_sub_commission=CASE WHEN @total_premium<>0 THEN @sub_commission_value_home*(@this_premium_original/@total_premium) ELSE 0 END
   SELECT @this_sub_commission= ROUND( @this_sub_commission,@Decimal_Places)        
   SELECT @remain_sub_commission=@remain_sub_commission-@this_sub_commission    
   SELECT @gross_row=@gross_row+1    
  END    
    
     /* Set record type for SUB-AGENT COMMISSION record */    
     SELECT  @stats_detail_type = 'SUB'    
    
  /*Get commission in base currency*/    
  EXEC spu_ACT_Do_Currency_Conversion    
     @company_id = @company_id,    
     @currency_id = @currency_id,    
     @currency_amount_unrounded = @this_sub_commission,    
     @mode = 'ALL',    
	 @decimal_places =4,
     @base_amount = @this_sub_commission_home OUTPUT,    
     @system_amount = @this_sub_commission_system OUTPUT,    
     @currency_base_xrate = @currency_rate OUTPUT,    
     @system_base_xrate = @system_rate OUTPUT,    
     @return_status = @return_status OUTPUT    
    
     /* Set stats_detail_id */    
     SELECT  @stats_detail_id = MAX(stats_detail_id) + 1    
     FROM    Stats_Detail    
     WHERE   stats_folder_cnt = @stats_folder_cnt    
    
     IF  @stats_detail_id is  NULL    
         SELECT  @stats_detail_id = 1    
    
     /* Insert the Stats Detail */    
     INSERT INTO Stats_Detail    
  (    
   stats_folder_cnt,    
   stats_detail_id,    
   stats_detail_type,    
   risk_id,    
   risk_type_id,    
   risk_type_code,    
   peril_id,    
   peril_description,    
   peril_type_id,    
   peril_type_code,    
   policy_section_type_id,    
   policy_section_type_code,    
   class_of_business_id,    
   class_of_business_code,    
   tax_type_id,    
   tax_type_code,    
   tax_value,    
   ri_party_cnt,    
   ri_shortname,    
   ri_party_type,    
   ri_share_percent,    
   ri_agreement_code,    
   annual_premium,    
   currency_code,    
   currency_rate,    
   this_premium_original,    
   this_premium_home,    
   this_premium_system,    
   commission_percent,          
   sub_commission_value_home,    
   sub_commission_value_system,    
   sum_insured_home,    
   sum_insured_system,    
   sum_insured_currency_code,    
   sum_insured_change,    
   transaction_ledger_id,    
   transaction_account_id,    
   account_type_code,    
   ceded_ref,    
   cover_share_percent,    
   sum_insured_total,    
   charges_total,    
   taxes_total,    
   recoveries_total,    
   commission_excluded,    
   withholding_tax_excluded    
  )    
     VALUES    
  (    
   @stats_folder_cnt,    
   @stats_detail_id,    
   @stats_detail_type,    
   @risk_id,    
   @risk_type_id,    
   @risk_type_code,    
   @peril_id,    
   @peril_description,    
   @peril_type_id,    
   @peril_type_code,    
   @policy_section_type_id,    
   @policy_section_type_code,    
   @class_of_business_id,    
   @class_of_business_code,    
   @tax_type_id,    
   @tax_type_code,    
   @tax_value,    
   @ri_party_cnt,    
   @ri_shortname,    
   @ri_party_type,    
   @ri_share_percent,    
   @ri_agreement_code,    
   @annual_premium,    
   @currency_code,    
   @currency_rate,    
   @this_premium_original,    
   @this_premium_home,    
   @this_premium_system,    
   @commission_percent,          
   @this_sub_commission_home,    
   @this_sub_commission_system,    
   @sum_insured_home,    
   @sum_insured_system,    
   @sum_insured_currency_code,    
   @sum_insured_change,    
   @transaction_ledger_id,    
   @transaction_account_id,    
   @account_type_code,    
   @agent,    
   @cover_share_percent,    
   @sum_insured_total,    
   @charges_total,    
   @taxes_total,    
   @recoveries_total,    
   @commission_excluded,    
   @withholding_tax_excluded    
  )    
    
  /* Get the next one */    
  FETCH NEXT FROM c_gross INTO    
   @risk_id,    
   @risk_type_id,    
   @risk_type_code,    
   @peril_id,    
   @peril_description,    
   @peril_type_id,    
   @peril_type_code,    
   @policy_section_type_id,    
   @policy_section_type_code,    
   @class_of_business_id,    
   @class_of_business_code,    
   @annual_premium,    
   @this_premium_original,    
   @this_premium_home,    
   @this_premium_system    
 END    
    
 CLOSE c_gross    
 DEALLOCATE c_gross    
    
    /* Fetch Next */    
 FETCH NEXT FROM c_commission INTO @agent, @sub_commission_value_home  ,@cob_id    
    
END    
    
CLOSE c_commission    
DEALLOCATE c_commission    
    
DECLARE @risk_cnt INT,    
  @out_stats_detail_type VARCHAR(20),    
  @out_stats_detail_id INT,    
  @out_currency_rate FLOAT,    
  @out_ri_shortname VARCHAR(20),    
  @out_system_rate FLOAT    
    
DECLARE @tax_band_id INT,    
     @tax_band_code VARCHAR(20),    
  @tax_premium NUMERIC(19,4),    
  @tax_percentage FLOAT,    
  @tax_is_value TINYINT,    
  @out_tax_value_home NUMERIC(19,4),    
  @out_tax_value_system NUMERIC(19,4),    
  @agent_shortname VARCHAR(20)    
    
-- ********************************************************************    
--                     TAX RECORDS FOR COMMISSION    
-- ********************************************************************    
    
/*Declare Tax Cursor*/    
DECLARE Taxes_Cursor CURSOR FAST_FORWARD FOR    
    SELECT  tc.tax_band_id,    
            tc.premium,    
            tc.percentage,    
            tc.value,    
            tc.is_value,    
            tb.code,    
   tc.risk_cnt,    
            R.risk_type_id,    
            RT.code,    
   P.shortname    
    FROM    Tax_Calculation TC    
    JOIN    tax_band TB     ON tb.tax_band_id = tc.tax_band_id    
    JOIN    Tax_Type TT  ON tt.tax_type_id = tb.tax_type_id    
LEFT JOIN Risk R    ON R.risk_cnt=TC.risk_cnt    
LEFT JOIN Risk_Type RT ON RT.risk_type_id=R.risk_type_id    
 JOIN Agent_Commission AC ON AC.agent_commission_cnt = TC.agent_commission_cnt    
 JOIN Party P   ON P.party_cnt = AC.party_cnt    
    WHERE   TC.insurance_file_cnt = @insurance_file_cnt    
 AND  TC.transtype='TTAC'    
    
OPEN Taxes_Cursor    
FETCH NEXT FROM Taxes_Cursor    
    INTO    @tax_band_id,    
            @tax_premium,    
            @tax_percentage,    
            @tax_value,    
            @tax_is_value,    
            @tax_band_code,    
   @risk_cnt,    
      @risk_type_id,    
      @risk_type_code,    
   @agent_shortname    
    
WHILE (@@FETCH_STATUS = 0)    
BEGIN    
      
        -- Set record type for TAX record    
        SELECT  @out_stats_detail_type = 'TCO'    
    
  EXEC spu_ACT_Do_Currency_Conversion    
     @company_id = @company_id,    
     @currency_id = @currency_id,    
     @currency_amount_unrounded = @tax_value,    
     @mode = 'ALL',   
	 @decimal_places =4, 
     @base_amount = @out_tax_value_home OUTPUT,    
     @system_amount = @out_tax_value_system OUTPUT,    
     @currency_base_xrate = @currency_rate OUTPUT,    
     @system_base_xrate = @system_rate OUTPUT,    
     @return_status = @return_status OUTPUT    
    
        -- Get next stats_detail_id and set type    
        SELECT  @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0) + 1    
        FROM    Stats_Detail    
        WHERE   stats_folder_cnt = @stats_folder_cnt    
    
        -- Insert the Stats Detail    
        INSERT INTO Stats_Detail    
  (    
   stats_folder_cnt,    
   stats_detail_id,    
   stats_detail_type,    
   risk_id,    
   risk_type_id,    
   risk_type_code,    
   ri_share_percent,    
   this_premium_original,    
   this_premium_home,    
   this_premium_system,    
   currency_rate,    
   currency_code,    
   tax_type_id,    
   tax_type_code,    
   tax_value,    
   ceded_ref    
  )    
        VALUES    
  (    
   @stats_folder_cnt,    
   @out_stats_detail_id,    
   @out_stats_detail_type,    
   @risk_cnt,    
   @risk_type_id,    
   @risk_type_code,    
   @tax_percentage,    
   @tax_value,    
   @out_tax_value_home,    
   @out_tax_value_system,    
   @out_currency_rate,    
   @currency_code,    
   @tax_band_id,    
   @tax_band_code,    
   @tax_value,    
   @agent_shortname    
  )    
    
        --Write tax payable record    
        SELECT  @out_ri_shortname = 'NOTA' + RTRIM(@tax_band_code)    
    
        -- Get next stats_detail_id and set type    
        SELECT  @out_stats_detail_type = 'TAN'    
    
        SELECT  @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0) + 1    
        FROM    Stats_Detail    
        WHERE   stats_folder_cnt = @stats_folder_cnt    
    
        INSERT INTO Stats_Detail    
  (    
   stats_folder_cnt,    
   stats_detail_id,    
   stats_detail_type,    
   risk_id,    
   risk_type_id,    
   risk_type_code,    
   ri_shortname,    
   ri_share_percent,    
   this_premium_original,    
   this_premium_home,    
   this_premium_system,    
   currency_rate,    
   currency_code,    
   tax_type_id,    
   tax_type_code,    
   tax_value    
  )    
        VALUES    
  (    
   @stats_folder_cnt,    
   @out_stats_detail_id,    
   @out_stats_detail_type,    
   @risk_cnt,    
   @risk_type_id,    
   @risk_type_code,    
   @out_ri_shortname,    
   @tax_percentage,    
   @tax_value,    
   @out_tax_value_home,    
   @out_tax_value_system,    
   @out_currency_rate,    
   @currency_code,    
   @tax_band_id,    
   @tax_band_code,    
   @tax_value    
  )    
  --IF @tax_value <> 0    
    
 FETCH NEXT FROM Taxes_Cursor    
     INTO    @tax_band_id,    
             @tax_premium,    
             @tax_percentage,    
             @tax_value,    
             @tax_is_value,    
 @tax_band_code,    
    @risk_cnt,    
       @risk_type_id,    
       @risk_type_code,    
    @agent_shortname    
END    
    
-- Only close the cursor, we will use it again    
CLOSE Taxes_Cursor    
DEALLOCATE Taxes_Cursor    
    
  
END  
GO


