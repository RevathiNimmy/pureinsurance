
Execute DDLDropProcedure 'spu_add_stats_details_fee'
GO

CREATE PROCEDURE spu_add_stats_details_fee  
    @stats_folder_cnt int  
AS  
BEGIN  
  
-- Declare variable for all columns in stats_details table  
DECLARE @out_currency_id SMALLINT,  
   @out_currency_code VARCHAR(20),  
   @out_stats_detail_type CHAR(3),  
   @out_stats_detail_id INT,  
   @out_currency_rate FLOAT,  
   @out_ri_shortname VARCHAR(20),  
   @out_system_rate FLOAT,  
   @out_fee_value_home NUMERIC(19, 4),  
   @out_fee_value_system NUMERIC(19, 4)  
  
DECLARE @insurance_file_cnt int,  
        @fee_amount NUMERIC(19,4),  
   @fee_shortname VARCHAR(20)  
  
DECLARE @risk_cnt INT,  
      @risk_type_id INT,  
      @risk_type_code VARCHAR(10)  
  
DECLARE @tax_band_id INT,  
      @tax_band_code VARCHAR(20),  
   @tax_value NUMERIC(19,4),  
   @tax_premium NUMERIC(19,4),  
   @tax_percentage FLOAT,  
   @tax_is_value TINYINT,  
   @out_tax_value_home NUMERIC(19,4),  
   @out_tax_value_system NUMERIC(19,4),      
   @tax_is_not_applied_to_client TINYINT,
   @fee_party VARCHAR(50)  
  
DECLARE @company_id INT  
DECLARE @return_status INT  
declare @document_ref	varchar(25)
  
-- Get the Insurance_File_Cnt for this policy  
SELECT  @insurance_file_cnt = insurance_file_cnt  
,@document_ref = document_ref 
FROM stats_folder  
WHERE   stats_folder.stats_folder_cnt = @stats_folder_cnt  
  
/*Get details from insurance file*/  
SELECT  
  @company_id = source_id,  
  @out_currency_id = currency_id,  
  @out_currency_rate = currency_base_xrate,  
  @out_system_rate = system_base_xrate  
FROM insurance_file  
WHERE insurance_file_cnt = @insurance_file_cnt  

if substring(LTRIM(RTRIM(@document_ref)),0,4) ='SDD'
begin
set @out_currency_rate = 0
end 

/*Get details about the currency*/  
SELECT  
  @out_currency_code = code  
FROM currency  
WHERE currency_id = @out_currency_id  
  
/*Declare Fee Cursor*/  
DECLARE Fees_Cursor CURSOR FAST_FORWARD FOR  
  SELECT  
  p.shortname,  
   ISNULL(pf.currency_amount, 0),  
  pf.risk_cnt,  
  R.risk_type_id,  
      RT.code  
  FROM Policy_fee_u pf
  JOIN Party p ON p.party_cnt = pf.party_cnt
  LEFT JOIN Risk R ON r.risk_cnt=pf.risk_cnt
  LEFT JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = pf.risk_cnt
  LEFT JOIN Risk_Type RT ON RT.risk_type_id=R.risk_type_id
  WHERE pf.insurance_file_cnt = @insurance_file_cnt
  AND (ifrl.risk_cnt = pf.risk_cnt OR pf.risk_cnt IS NULL)
  AND (ISNULL(pf.currency_amount, 0) <> 0) 
  
-- ********************************************************************  
--                     GROSS RECORDS FOR FEES  
-- ********************************************************************  
OPEN Fees_Cursor  
FETCH NEXT FROM Fees_Cursor INTO @fee_shortname, @fee_amount, @risk_cnt,  
          @risk_type_id, @risk_type_code  
  
WHILE (@@FETCH_STATUS = 0)  
BEGIN  
  /*Set type and account name*/  
 SELECT  
   @out_stats_detail_type = 'PFG',  
   @out_ri_shortname = @fee_shortname   
  
  EXEC spu_ACT_Do_Currency_Conversion  
    @company_id = @company_id,  
    @currency_id = @out_currency_id,  
    @currency_amount_unrounded = @fee_amount,  
    @mode = 'ALL',  
    @base_amount = @out_fee_value_home OUTPUT,  
    @system_amount = @out_fee_value_system OUTPUT,  
    @currency_base_xrate = @out_currency_rate OUTPUT,  
    @system_base_xrate = @out_system_rate OUTPUT,  
    @return_status = @return_status OUTPUT  
  
  /*Get next stats_detail_id*/  
  SELECT @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0) + 1  
  FROM Stats_Detail  
  WHERE stats_folder_cnt = @stats_folder_cnt  
  
  /*Insert the client/agent side of the fee into stats_detail*/  
  INSERT INTO Stats_Detail  
 (  
  stats_folder_cnt,  
  stats_detail_id,  
  stats_detail_type,  
  risk_id,  
  risk_type_id,  
  risk_type_code,  
  ri_shortname,  
  this_premium_original,  
  this_premium_home,  
  this_premium_system,  
  currency_rate,  
  currency_code  
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
  @fee_amount,  
  @out_fee_value_home,  
  @out_fee_value_system,  
  @out_currency_rate,  
  @out_currency_code  
 )  
  
  /*Set type and account name*/  
 SELECT  
   @out_stats_detail_type = 'PFE',  
   @out_ri_shortname = @fee_shortname    
  
  /*Get next stats_detail_id*/  
  SELECT @out_stats_detail_id = ISNULL(MAX(stats_detail_id), 0) + 1  
  FROM Stats_Detail  
  WHERE stats_folder_cnt = @stats_folder_cnt  
  
  /*Insert the fee account side of the fee into stats_detail*/  
  INSERT INTO Stats_Detail  
 (  
  stats_folder_cnt,  
  stats_detail_id,  
  stats_detail_type,  
  risk_id,  
  risk_type_id,  
  risk_type_code,  
  ri_shortname,  
  this_premium_original,  
  this_premium_home,  
  this_premium_system,  
  currency_code,  
  currency_rate  
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
  -@fee_amount,  
  -@out_fee_value_home,  
  -@out_fee_value_system,  
  @out_currency_code,  
  @out_currency_rate  
 )  
  
  FETCH NEXT FROM Fees_Cursor INTO @fee_shortname, @fee_amount, @risk_cnt,  
          @risk_type_id, @risk_type_code  
  
END  
  
/*Close and deallocate the cursor*/  
CLOSE Fees_Cursor  
DEALLOCATE Fees_Cursor  
  
-- ********************************************************************  
--                     TAX RECORDS FOR FEES  
-- ********************************************************************  
  
/*Declare Fee Cursor*/  
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
   tt.is_not_applied_to_client,
	p.shortname    
    FROM    Tax_Calculation TC  
    JOIN    tax_band TB     ON tb.tax_band_id = tc.tax_band_id  
    JOIN    Tax_Type TT     ON tt.tax_type_id = tb.tax_type_id  
LEFT JOIN Risk R     ON R.risk_cnt=TC.risk_cnt  
LEFT JOIN Risk_Type RT ON RT.risk_type_id=R.risk_type_id 
LEFT JOIN policy_fee_u pfu ON pfu.policy_fee_u_id = TC.policy_fee_u_id
LEFT JOIN party p ON p.party_cnt = pfu.party_cnt 
    WHERE   TC.insurance_file_cnt = @insurance_file_cnt  
 AND  TC.transtype='TTF'  
  
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
   @tax_is_not_applied_to_client, 
  @fee_party 
  
WHILE (@@FETCH_STATUS = 0)  
BEGIN  
    IF @tax_value <> 0  
    BEGIN  
        -- Set record type for TAX record  
        SELECT  @out_stats_detail_type = 'TFE',  
                @out_ri_shortname = 'NOTAOUT' + RTRIM(@tax_band_code)  
  
   EXEC spu_ACT_Do_Currency_Conversion  
      @company_id = @company_id,  
      @currency_id = @out_currency_id,  
      @currency_amount_unrounded = @tax_value,  
      @mode = 'ALL',  
      @base_amount = @out_tax_value_home OUTPUT,  
      @system_amount = @out_tax_value_system OUTPUT,  
      @currency_base_xrate = @out_currency_rate OUTPUT,  
      @system_base_xrate = @out_system_rate OUTPUT,  
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
  -@tax_value,  
  -@out_tax_value_home,  
  -@out_tax_value_system,
   @out_currency_rate,  
   @out_currency_code,  
   @tax_band_id,  
   @tax_band_code,  
  -@tax_value  
  )  
  
        --Write tax payable record  
        SELECT  @out_ri_shortname = 'NOTA' + RTRIM(@tax_band_code)  
  
        -- Get next stats_detail_id and set type  
        IF @tax_is_not_applied_to_client = 1  
            SELECT  @out_stats_detail_type = 'TAX',  
                    @out_ri_shortname = 'NOTAOUT' + RTRIM(@tax_band_code)  
        ELSE  
            SELECT  @out_stats_detail_type = 'TAG',  
                    @out_ri_shortname = @fee_party   
  
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
   @out_currency_code,  
   @tax_band_id,  
   @tax_band_code,  
   @tax_value  
  )  
    END --IF @tax_value <> 0  
  
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
    @tax_is_not_applied_to_client ,
	@fee_party 
END  
  
-- Only close the cursor, we will use it again  
CLOSE Taxes_Cursor  
DEALLOCATE Taxes_Cursor  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
