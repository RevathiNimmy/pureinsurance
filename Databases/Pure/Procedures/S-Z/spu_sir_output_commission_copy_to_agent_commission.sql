
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_sir_output_commission_copy_to_agent_commission'
GO

CREATE PROCEDURE spu_sir_output_commission_copy_to_agent_commission(  
    @insurance_file_cnt int,  
    @transaction_type varchar(10),
    @EditCommission TINYINT = 0 OUTPUT  
) AS BEGIN  
  
 --Delete the existing entires  
    If NOT Exists(
			Select * from agent_commission ac INNER JOIN insurance_file ifi 
			    ON ifi.insurance_file_cnt = ac.insurance_file_cnt 
				AND ifi.lead_agent_cnt = ac.party_cnt
					Where ac.Insurance_file_cnt = @insurance_file_cnt AND ac.is_amended = 1)
					BEGIN
   EXEC spu_sir_agent_commission_del @insurance_file_cnt  
		             END  

--PM043757 Jai Prakash 08 Oct 2015
IF @transaction_type = 'REN'		             
EXEC spu_sir_agent_commission_del @insurance_file_cnt
  
 DECLARE @DataModelCode VARCHAR(255)  
 DECLARE @Gis_Policy_link_Id INT  
 DECLARE @Risk_Id INT  
 DECLARE @SQL VARCHAR(8000)  
  
 DECLARE @policy_binder_id INT,  
   @output_id INT,  
   @output_commission_id INT,  
   @agent_commission_cnt INT  
  
 DECLARE @party_cnt INT,  
   @is_lead_agent TINYINT,  
   @peril_id INT,  
   @premium MONEY,  
   @commission_value MONEY,  
   @calculated_commission_value MONEY,  
   @commission_band_id INT,  
   @account_currency_id INT,  
   @tax_group_id INT,  
   @risk_type_id INT,  
   @commission_percentage FLOAT,  
   @override_reason VARCHAR(255),  
   @account_id INT,  
   @company_id INT,  
   @currency_id INT,  
   @tax_currency_amount MONEY,  
   @tax_base_amount MONEY,  
   @tax_account_amount MONEY,  
   @return_status INT,  
   @run_once TINYINT,
   @cob_code VARCHAR(255),  
   @class_of_business_id INT
  
   SET @run_once = 0  
  
   SELECT @account_id = account_id  
   FROM account  
   WHERE account_key = @party_cnt  
  
   SELECT  
    @company_id = source_id,  
    @currency_id = currency_id  
   FROM insurance_file  
   WHERE insurance_file_cnt = @insurance_file_cnt  
  



   -- Check for Records in outPut_commission before proceeding 
    DECLARE @SQL1 NVARCHAR(1000) 
    DECLARE @Cnt INT
    DECLARE @RecordCount INT =0
    DECLARE @ParmDefinition nvarchar(500);
    SET @ParmDefinition = N'@Cnt INT OUTPUT'

 DECLARE cur CURSOR FAST_FORWARD FOR  

     SELECT DISTINCT LTRIM(RTRIM(gdm.code))
     from gis_policy_link gpl  
     INNER JOIN gis_data_model gdm ON gdm.gis_data_model_id = gpl.gis_data_model_id  
     INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = gpl.risk_id  
     INNER JOIN insurance_file ifi ON ifi.insurance_file_cnt  = ifrl.insurance_file_cnt  
     WHERE ifi.insurance_file_cnt = @insurance_file_cnt  

     OPEN cur
     Fetch Next From cur Into @DataModelCode

     WHILE @@FETCH_STATUS = 0 
     BEGIN
       -- DO we have entries in Output Commission 
       SET @SQL1 = 'SELECT @Cnt = Count(*) FROM ' + @DataModelCode + '_Output_Commission'
       EXEC sp_executesql   @SQL1 ,@ParmDefinition ,@Cnt = @Cnt OUTPUT 
       IF @Cnt > 0 
            SET @RecordCount  =1 
              
       Fetch Next From cur Into @DataModelCode
     END
 CLOSE cur
 DEALLOCATE cur

 IF @RecordCount = 0 
    Return



 CREATE TABLE #OUTPUTCOMM (  
  policy_binder_id INT,  
  output_id INT,  
  output_commission_id INT,  
  insurance_file_cnt INT,  
  party_cnt INT,  
  peril_id INT,  
  premium MONEY,  
  commission_value MONEY,  
  calculated_commission_value MONEY,  
  commission_band_id INT,  
  account_currency_id INT,  
  tax_group_id INT,  
  risk_type_id INT,  
  commission_percentage FLOAT,  
  override_reason VARCHAR(255),
  COB_code VARCHAR(255)
 )  
  
 DECLARE Gis_Binder_Cursor CURSOR FAST_FORWARD FOR  
 --SELECT LTRIM(RTRIM(gdm.code)), gpl.gis_policy_link_id, gpl.risk_id from gis_policy_link gpl  
 -- LEFT JOIN insurance_file ifi ON ifi.insurance_file_cnt = gpl.insurance_file_cnt  
 -- LEFT JOIN gis_data_model gdm ON gdm.gis_data_model_id = gpl.gis_data_model_id  
 -- INNER JOIN insurance_file_risk_link ifrl ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt  
 -- WHERE gpl.insurance_file_cnt = (select insurance_folder_cnt from insurance_file where insurance_file_cnt = @insurance_file_cnt)  
 SELECT  
 LTRIM(RTRIM(gdm.code)),  
 gpl.gis_policy_link_id,  
 gpl.risk_id  
 from gis_policy_link gpl  
 INNER JOIN gis_data_model gdm ON gdm.gis_data_model_id = gpl.gis_data_model_id  
 INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = gpl.risk_id  
 INNER JOIN insurance_file ifi ON ifi.insurance_file_cnt  = ifrl.insurance_file_cnt  
 WHERE ifi.insurance_file_cnt = @insurance_file_cnt  
  
 Open Gis_Binder_Cursor  
 Fetch Next From Gis_Binder_Cursor Into @DataModelCode, @Gis_Policy_link_Id, @Risk_Id  
  
 While @@Fetch_Status = 0  
 BEGIN  
  -- Put the intended commission into the temp table  
  -- We must do this because we need to be able to traverse the lines individually  
  
  DELETE #OUTPUTCOMM  
  
  SET @SQL = 'INSERT INTO #OUTPUTCOMM('  
  SET @SQL = @SQL + 'policy_binder_id, ' + CHAR(13)  
  SET @SQL = @SQL + 'output_id, ' + CHAR(13)  
  SET @SQL = @SQL + 'output_commission_id, ' + CHAR(13)  
  SET @SQL = @SQL + 'insurance_file_cnt, ' + CHAR(13)  
  SET @SQL = @SQL + 'party_cnt, ' + CHAR(13)  
  SET @SQL = @SQL + 'peril_id, ' + CHAR(13)  
  SET @SQL = @SQL + 'premium, ' + CHAR(13)  
  SET @SQL = @SQL + 'commission_value, ' + CHAR(13)  
  SET @SQL = @SQL + 'calculated_commission_value, ' + CHAR(13)  
  SET @SQL = @SQL + 'commission_band_id, ' + CHAR(13)  
  SET @SQL = @SQL + 'account_currency_id, ' + CHAR(13)  
  SET @SQL = @SQL + 'tax_group_id, ' + CHAR(13)  
  SET @SQL = @SQL + 'risk_type_id, ' + CHAR(13)  
  SET @SQL = @SQL + 'commission_percentage, ' + CHAR(13)  
  SET @SQL = @SQL + 'override_reason, ' + CHAR(13)  
  SET @SQL = @SQL + 'COB_code) ' + CHAR(13)
  
  SET @SQL = @SQL + ' SELECT ' + CHAR(13)  
  SET @SQL = @SQL + ' OC.' + @DataModelCode + '_policy_binder_id,' + CHAR(13)  
  SET @SQL = @SQL + 'output_id,' + CHAR(13)  
  SET @SQL = @SQL + ' OC.' + @DataModelCode + '_output_commission_id,' + CHAR(13)  
  SET @SQL = @SQL + CONVERT(VARCHAR(20), @insurance_file_cnt) + ',' + CHAR(13)  
  SET @SQL = @SQL + ' P.party_cnt,' + CHAR(13)  
  SET @SQL = @SQL + ' PT.Peril_type_id,' + CHAR(13)  
  SET @SQL = @SQL + ' OC.gross_ap_rp, ' + CHAR(13)  
  
  IF @transaction_type = 'NB' or  @transaction_type = 'REN'  
 BEGIN  
  SET @SQL = @SQL + ' OC.Net_annual_commission_overriden, ' + CHAR(13)  
  SET @SQL = @SQL + ' OC.Net_annual_commission, ' + CHAR(13)  
 END  
  ELSE  
 BEGIN  
  SET @SQL = @SQL + ' OC.net_ap_rp_overriden, ' + CHAR(13)  
  SET @SQL = @SQL + ' OC.net_ap_rp, ' + CHAR(13)  
 END  
  
  SET @SQL = @SQL + ' CB.commission_band_id, ' + CHAR(13)  
  SET @SQL = @SQL + ' Curr.currency_id, ' + CHAR(13)  
  SET @SQL = @SQL + ' TG.Tax_Group_id, ' + CHAR(13)  
  SET @SQL = @SQL + ' risk.risk_type_id, ' + CHAR(13)  
  SET @SQL = @SQL + ' OC.Commission_percent_overriden, ' + CHAR(13)    -- NEW COLUMN IN OUTPUT_COMMISSION  
  SET @SQL = @SQL + ' NULL,  ' + CHAR(13) --OVERRIDE REASON  
  SET @SQL = @SQL + ' OC.COB_code  ' + CHAR(13) 
  SET @SQL = @SQL + ' From ' + CHAR(13)  
  SET @SQL = @SQL + @DataModelCode + '_Output_Commission OC ' + CHAR(13)  
  SET @SQL = @SQL + ' INNER JOIN Party P On OC.Agent_Party_Code = P.Shortname AND OC.Agent_Party_Code = LTrim(Rtrim(P.shortname)) ' + CHAR(13)  
  SET @SQL = @SQL + ' LEFT JOIN ' + @DataModelCode + '_Policy_Binder PB ON PB.' + @DataModelCode + '_policy_binder_id = OC.' + @DataModelCode + '_policy_binder_id ' + CHAR(13)  
  SET @SQL = @SQL + ' LEFT JOIN gis_policy_link gpl ON gpl.gis_policy_link_id = PB.gis_policy_link_id ' + CHAR(13)  
  SET @SQL = @SQL + ' LEFT JOIN Peril_Type PT ON OC.peril_code = PT.code ' + CHAR(13)  
  SET @SQL = @SQL + ' LEFT JOIN Commission_Band CB ON CB.code = OC.Commission_band_code ' + CHAR(13)  
  SET @SQL = @SQL + ' LEFT JOIN Currency Curr ON Curr.code = OC.Currency_Code ' + CHAR(13)  
  SET @SQL = @SQL + ' LEFT JOIN Tax_Group TG ON TG.code = OC.tax_group_code ' + CHAR(13)  
  SET @SQL = @SQL + ' LEFT JOIN insurance_file_risk_link ifr ON gpl.risk_id = ifr.risk_cnt ' + CHAR(13)  
  SET @SQL = @SQL + ' LEFT JOIN risk ON risk.risk_cnt = ifr.risk_cnt ' + CHAR(13)  
  SET @SQL = @SQL + ' WHERE ifr.insurance_file_cnt = ' + CONVERT(VARCHAR(20),@insurance_file_cnt) + CHAR(13)  
  SET @SQL = @SQL + ' AND risk.risk_cnt = ' + CONVERT(VARCHAR(20), @Risk_Id) + CHAR(13)  
  SET @SQL = @SQL + ' AND ISNULL(PT.is_deleted,0) = 0' + CHAR(13)  
  SET @SQL = @SQL + ' AND ISNULL(CB.is_deleted,0) = 0' + CHAR(13)  
  SET @SQL = @SQL + ' AND ISNULL(Curr.is_deleted,0)= 0' + CHAR(13)  
  SET @SQL = @SQL + ' AND ISNULL(TG.is_deleted,0) = 0' + CHAR(13)  
  SET @SQL = @SQL + ' AND risk.is_risk_selected = 1' + CHAR(13)  
  SET @SQL = @SQL + ' AND is_retained = 0'      + CHAR(13)  
  SET @SQL = @SQL + ' Order by risk.risk_cnt' + CHAR(13)  
  
  EXEC(@SQL)  
  
  -- Now Loop Through the Temp Records so that Scripted Tax can be passed through for each one.  
  DECLARE Commission_Cursor CURSOR FAST_FORWARD FOR  
  SELECT policy_binder_id, output_id, output_commission_id,  
   insurance_file_cnt, party_cnt, peril_id,  
   premium, commission_value, calculated_commission_value,  
   commission_band_id, account_currency_id, tax_group_id,  
   risk_type_id, commission_percentage, override_reason,
   COB_code 
  FROM #OUTPUTCOMM  
  
  Open Commission_Cursor  
  Fetch Next From Commission_Cursor Into @policy_binder_id, @output_id, @output_commission_id,  
            @insurance_file_cnt, @party_cnt, @peril_id,  
            @premium, @commission_value, @calculated_commission_value,  
            @commission_band_id, @account_currency_id, @tax_group_id,  
            @risk_type_id, @commission_percentage, @override_reason,
            @cob_code  
  
  While @@Fetch_Status = 0  
  BEGIN  
   -- Set the Lead Agent flag  
   SET @is_lead_agent=0  
  
   SELECT @is_lead_agent=1  
   FROM Insurance_File  
   WHERE insurance_file_cnt = @insurance_file_cnt  
   AND lead_agent_cnt = @party_cnt
     
   SELECT @class_of_business_id = class_of_business_id  from CLASS_OF_BUSINESS where code = RTRIM(LTRIM(@cob_code))
  
   INSERT INTO Agent_Commission (insurance_file_cnt,  
    party_cnt,  
    peril_type_id,  
    premium,  
    commission_value,  
    calculated_commission_value,  
    commission_band_id,  
    account_currency_id,  
    tax_group_id,  
    risk_type_id,  
    commission_percentage,  
    override_reason,  
    is_locked,  
    is_amended,  
    is_lead_agent,
    class_of_business_id)  
   SELECT  
    @insurance_file_cnt,  
    @party_cnt,  
    @peril_id,  
    @premium,  
    @commission_value,  
    @calculated_commission_value,  
    @commission_band_id,  
    @account_currency_id,  
    @tax_group_id,  
    @risk_type_id,  
    IsNull(@commission_percentage,0),  
    @override_reason,  
    0,  
    0,  
    @is_lead_agent,
    @class_of_business_id  
  
   SET @agent_commission_cnt = @@IDENTITY  
   
   SET @EditCommission = 1

   /* Calculate the tax breakdown */  
   EXEC spu_SIR_Calculate_Tax_Amounts  
    @company_id=@company_id,  
    @tax_group_id=@tax_group_id,  
    @transtype='TTAC',  
    @currency_id=@currency_id,  
    @amount=@commission_value,  
    @tax_currency_amount=@tax_currency_amount OUTPUT,  
    @tax_base_amount=@tax_base_amount OUTPUT,  
    @associated_key_id=@agent_commission_cnt,  
    @insurance_file_cnt=@insurance_file_cnt,  
        @risk_cnt=NULL,@premium=@premium  
  
   /* Convert to Account currency */  
   EXECUTE spu_ACT_Do_Currency_Conversion  
    @account_id = @account_id,  
    @company_id = @company_id,  
    @currency_id = @currency_id,  
    @currency_amount_unrounded = @tax_currency_amount,  
    @mode = 'ALL',  
    @account_amount = @tax_account_amount OUTPUT,  
    @return_status = @return_status OUTPUT  
  
   /* Revise Tax amounts */  
   UPDATE  Agent_Commission  
   SET     tax_amount=@tax_currency_amount,  
     tax_account_amount=@tax_account_amount,  
     tax_base_amount=@tax_base_amount  
   WHERE   agent_commission_cnt=@agent_commission_cnt  
  
  
   IF @is_lead_agent = 0 --and @commission_percentage <> 0 --there is already a check for is_retained = 0 in the cursor  
   BEGIN  
            If @run_once = 0  
            BEGIN  
                  SET @run_once = 1  
                  EXEC spu_Del_SubAgents @insurance_file_cnt  
            END  
  
    IF NOT EXISTS(SELECT * FROM insurance_file_agent where party_cnt = @party_cnt and insurance_file_cnt = @insurance_file_cnt)  
  EXEC spu_Add_SubAgents  
  @insurance_file_cnt = @insurance_file_cnt,  
  @party_cnt = @party_cnt,  
  @percentage = @commission_percentage,  
  @amount =  @commission_value  
   END  
  
   Fetch Next From Commission_Cursor Into @policy_binder_id, @output_id, @output_commission_id,  
            @insurance_file_cnt, @party_cnt, @peril_id,  
            @premium, @commission_value, @calculated_commission_value,  
            @commission_band_id, @account_currency_id, @tax_group_id,  
            @risk_type_id, @commission_percentage, @override_reason,
            @cob_code  
  
  END --END OF WHILE OF Commission_Cursor  
  
  FETCH NEXT FROM Gis_Binder_Cursor Into @DataModelCode, @Gis_Policy_link_Id, @Risk_Id  
  
  -- Close and Deallocate  
  Close Commission_Cursor  
  Deallocate Commission_Cursor  
 END --END OF WHILE OF Gis_Binder_Cursor  
  
 -- Close and Deallocate  
 Close Gis_Binder_Cursor  
 Deallocate Gis_Binder_Cursor  
  
 DROP TABLE #OUTPUTCOMM  
END  
 
