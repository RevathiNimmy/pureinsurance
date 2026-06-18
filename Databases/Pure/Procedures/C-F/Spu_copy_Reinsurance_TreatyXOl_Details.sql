SET Quoted_IdentIfier  Off
GO
SET Ansi_Nulls  ON
GO

EXECUTE ddlDropProcedure 'Spu_copy_Reinsurance_TreatyXOl_Details'
GO
CREATE Procedure Spu_copy_Reinsurance_TreatyXOl_Details      
 @claim_id int,      
 @risk_cnt int,      
 @ri_band_id int,      
 @ri_model_id int,      
 @claim_ri_arrangement_id int,      
 @version_id int,      
  @model_currency_rate FLOAT =1      
As      
BEGIN      
      
 Declare      
  @type varchar(5),      
  @treaty_id int,      
  @share_percent float,      
  @ceding_rate float,      
  @agreement_code varchar(100),      
  @priority int,      
  @number_of_lines float,      
  @line_limit money,      
  @lower_limit money,      
  @counter int,      
  @RIRegen  varchar(20),      
  @XOL_Treaty_to_Recover_From int,      
  @Date_for_Prop_Calculation int ,      
  @ri_model_line_id INT  ,    
  @cover_start_date_ForRi DATETIME ,    
  @Insurance_file_cnt int    
  SELECT @RIRegen=value  FROM Hidden_options WHERE option_number=105     
      
  Select  @Insurance_file_cnt = Policy_id from claim where claim_id = @claim_id    
   SELECT           
      @cover_start_date_ForRi = inception_date_tpi   
    FROM insurance_file  (NOLOCK)          
    WHERE Insurance_file_cnt = @Insurance_file_cnt       
      
  SELECT top 1 @XOL_Treaty_to_Recover_From= XOL_Treaty_to_Recover_From_id     
  FROM ri_band_version     
  WHERE ri_band_id=@ri_band_id      
  AND CONVERT(DATE, effective_date, 23 )<= CONVERT(DATE, @cover_start_date_ForRi, 23)        
  ORDER BY effective_date DESC        
    
  SELECT top 1  @Date_for_Prop_Calculation=Proportional_RI_Cal_Method      
  FROM   RI_Band_Version      
  WHERE  ri_band_id = @ri_band_id      
   AND CONVERT(DATE, effective_date, 23 )<= CONVERT(DATE, @cover_start_date_ForRi, 23)        
   ORDER BY effective_date DESC        
    
 IF @Date_for_Prop_Calculation =2  OR @XOL_Treaty_to_Recover_From=2      
 SET @RIRegen =1      
  IF NOT (ISNULL(@RIRegen,'0')='1')      
  BEGIN      
   INSERT INTO Claim_ri_Arrangement_Line      
  (Claim_Id,      
  ri_Arrangement_Line_Id,      
  ri_Arrangement_Id,      
  Type,      
  Treaty_Id,      
  Party_cnt,      
  xol_Arrangement_Id,      
  Default_Share_Percent,      
  This_Share_Percent,      
  Agreement_Code,      
  Priority,      
  Number_Of_Lines,      
  Line_Limit,      
  Sum_Insured,      
  Reserve,      
  Payment,      
  Salvage,      
  Recovery,      
  This_Reserve,      
  This_Payment,      
  This_Salvage,      
  This_Recovery,      
  Version_Id,      
  Original_ri_Arrangement_Line_Id,      
  lower_limit,      
  Retained,      
  participation_percent,      
  Grouping,
  manually_added)      
      
   SELECT @claim_id,      
  ral.ri_Arrangement_Line_Id,      
  @claim_ri_arrangement_id,      
  ral.TYPE,      
  ral.Treaty_Id,      
  ral.Party_cnt,      
  NULL,      
  CASE      
   WHEN ((ral.This_Share_Percent = NULL      
    OR ral.This_Share_Percent = 0)      
    AND ral.Premium_Value <> 0)      
   THEN      
     ral.Default_Share_Percent      
   ELSE      
     ral.This_Share_Percent      
   END, -- Default percent is the share From NB      
      
          CASE      
   WHEN ral.Type = 'T'      
   THEN      
     ral.Default_Share_Percent      
   ELSE      
     ral.This_Share_Percent      
   END, -- Default percent is the share From NB      
  --NULL, -- This share percent is to be determined by claims RI, MUST be Null      
  ral.Agreement_Code,      
  ral.Priority,      
  ral.Number_Of_Lines,      
  ral.Line_Limit,      
  ral.Sum_Insured,      
  0,     0,     0,     0,     0,     0,     0,     0,  -- All zero for new ri      
  @version_id,      
  ral.ri_Arrangement_Line_Id,      
  ral.Lower_limit,      
  ral.retained,      
  ral.participation_percent,      
  ral.Grouping,
  ISNULL(ral.manually_added,0)      
  FROM   ri_Arrangement_Line ral      
   JOIN ri_Arrangement ra      
   ON ra.ri_Arrangement_Id = ral.ri_Arrangement_Id      
  WHERE  ra.Risk_cnt = @risk_cnt      
   AND ra.ri_Band_Id = @ri_band_ID      
   AND ra.Original_Flag = 0      
   AND ral.type not in ('TX','TC','R')      
   AND ra.version_id= (select MAX(version_id) from ri_Arrangement where   ra.Risk_cnt = @risk_cnt      
   AND ra.ri_Band_Id = @ri_band_ID      
   AND ra.Original_Flag = 0 )      
 END      
      
 Declare copy_tx cursor Fast_Forward      
 For      
  Select      
   Case      
    When rt.code = 'RET' Then 'R'      
    When rt.code = 'XOL'  Then  'TX'      
    When rt.code = 'CAT' Then 'TC'      
    When rt.code = '001' Then 'TFS'        Else 'T'      
   End,      
  rml.treaty_id,      
  rml.share_percent,      
  isnull(rml.ceding_rate,0),      
  t.agreement_code,      
  rml.priority,      
  rml.number_of_lines,      
  rml.line_limit,      
  rml.lower_limit,      
  rml.ri_model_line_id      
  From    ri_model_line rml      
   Join    Treaty t      
  On t.treaty_id = rml.treaty_id      
   Join    Reinsurance_type rt      
  On rt.reinsurance_type_id = t.reinsurance_type_id      
      
     Left Join      
         -- Calculate a summary commission rate for each treaty      
        (Select  treaty_id,      
                 Sum(commission_percent * (share_percent / 100)) commission_percent      
         From    treaty_party      
         Group By      
                 treaty_id) tc      
         On tc.treaty_id = t.treaty_id      
     Where   ri_model_id = @ri_model_id      
     AND RTRIM(rt.code) IN ('XOL','CAT','RET')      
      
 OPEN copy_tx      
      
 FETCH NEXT FROM copy_tx      
 INTO @type ,      
  @treaty_id ,      
  @share_percent ,      
  @ceding_rate ,      
  @agreement_code ,      
  @priority ,      
  @number_of_lines ,      
  @line_limit ,      
  @lower_limit ,      
  @ri_model_line_id      
      
   WHILE @@FETCH_STATUS = 0      
   BEGIN      
      
 Select @counter=isnull(max(claim_ri_arrangement_line_id),0) + 1      
 from claim_ri_arrangement_line      
      
 Insert Into Claim_ri_Arrangement_Line (      
        Claim_Id,      
        ri_Arrangement_Line_Id,      
        ri_Arrangement_Id,      
        TYPE,      
        Treaty_Id,      
      
        Default_Share_Percent,      
        This_Share_Percent,      
        Agreement_Code,      
        Priority,      
        Number_Of_Lines,      
        Line_Limit,      
        Sum_Insured,      
        Reserve,      
        Payment,      
        Salvage,      
        Recovery,      
        This_Reserve,      
        This_Payment,      
        This_Salvage,      
        This_Recovery,      
        Version_Id,      
        Original_ri_Arrangement_Line_Id,      
     lower_limit,      
        Retained,      
        participation_percent,      
        Grouping,      
        ri_model_line_id,
        manually_added)      
 Values(      
     @Claim_id,      
  @counter,      
  @claim_ri_arrangement_id,      
     @type,      
  @treaty_id ,      
  @share_percent ,      
  @ceding_rate ,      
  @agreement_code ,      
  @priority ,      
  @number_of_lines ,      
  @line_limit*@model_currency_rate,      
  0,    0,    0,    0,    0,    0,    0,    0,    0,  -- All zero for new ri      
  @version_id,      
  Null,      
  @lower_limit*@model_currency_rate,      
  Null,      
  Null,      
  Null,      
  @ri_model_line_id,
  0)      
      
 FETCH NEXT FROM copy_tx      
 INTO @type ,      
  @treaty_id ,      
  @share_percent ,      
  @ceding_rate ,      
  @agreement_code,      
  @priority ,      
  @number_of_lines ,      
  @line_limit ,      
  @lower_limit,      
  @ri_model_line_id      
      
 END      
      
 CLOSE copy_tx      
 DEALLOCATE copy_tx      
      
END 
GO
