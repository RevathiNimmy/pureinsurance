SET Quoted_IdentIfier  Off
GO
SET Ansi_Nulls  ON
GO

EXECUTE ddlDropProcedure 'Spu_copy_Reinsurance_Treaty_Details'
GO
  
CREATE Procedure Spu_copy_Reinsurance_Treaty_Details  
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
  @ri_model_line_id INT,  
  @is_obligatory tinyint  
  
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
  is_obligatory,
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
    OR ral.This_Share_Percent = 0 OR ral.type ='FX')  
    AND ral.Premium_Value <> 0)  
   THEN  
     ral.Default_Share_Percent  
   ELSE  
     ral.This_Share_Percent  
   END,  
          CASE  
   WHEN ral.Type ='FX'  
   THEN  
     ral.Default_Share_Percent  
   ELSE  
     ral.This_Share_Percent  
   END,    
  ral.Agreement_Code,    
  ral.Priority,    
  ral.Number_Of_Lines,    
  ral.Line_Limit,    
  ral.Sum_Insured,    
  0,     0,     0,     0,     0,     0,     0,     0,    
  @version_id,    
  ral.ri_Arrangement_Line_Id,    
  ral.Lower_limit,    
  ral.retained,    
  ral.participation_percent,    
  ral.Grouping  ,  
  ISNULL(ral.is_obligatory,0),
  ISNULL(ral.manually_added,0)  
  FROM   ri_Arrangement_Line ral    
   JOIN ri_Arrangement ra    
   ON ra.ri_Arrangement_Id = ral.ri_Arrangement_Id    
  WHERE  ra.Risk_cnt = @risk_cnt    
   AND ra.ri_Band_Id = @ri_band_ID    
   AND ra.Original_Flag = 0    
   AND ral.type in ('FX','F')    
   AND ra.version_id= (SELECT MAX(version_id) FROM ri_Arrangement RA1 WHERE   RA1.Risk_cnt = @risk_cnt  
    AND RA1.ri_Band_Id = @ri_band_ID  
     AND RA1.Original_Flag = 0 )  
     
 Declare copy_treaty cursor Fast_Forward    
 For    
  Select    
   Case    
    When rt.code = 'RET' Then 'R'    
    When rt.code = 'XOL'  Then  'TX'    
    When rt.code = 'CAT' Then 'TC'  
    When rt.code = '001' Then 'TFS'  
    Else 'T'  
   End,  
  rml.treaty_id,  
  rml.share_percent,  
  isnull(rml.ceding_rate,0),  
  t.agreement_code,  
  rml.priority,  
  rml.number_of_lines,  
  rml.line_limit,  
  rml.lower_limit,  
  ISNULL(ral.is_obligatory,0),  
  rml.ri_model_line_id  
  From    ri_model_line rml    
   Join    Treaty t    
  On t.treaty_id = rml.treaty_id    
   Join    Reinsurance_type rt    
  On rt.reinsurance_type_id = t.reinsurance_type_id    
     Left Join    
        (Select  treaty_id,    
                 Sum(commission_percent * (share_percent / 100)) commission_percent    
         From    treaty_party    
         Group By    
                 treaty_id) tc    
         On tc.treaty_id = t.treaty_id    
        Join ri_arrangement_line RAL ON RAL.treaty_id = rml.treaty_id 
        join RI_Arrangement RA ON RA.ri_arrangement_id = RAL.ri_arrangement_id and Ra.ri_model_id =rml.ri_model_id and RA.risk_cnt =@risk_cnt  
        Where   RA.ri_model_id = @ri_model_id  
        AND RTRIM(rt.code) NOT IN ('XOL','CAT','RET')  AND RA.Original_Flag = 0  
     AND ra.ri_band_id = @ri_band_id 
 OPEN copy_treaty  
 FETCH NEXT FROM copy_treaty  
 INTO @type ,  
  @treaty_id ,  
  @share_percent ,  
  @ceding_rate ,  
  @agreement_code ,  
  @priority ,  
  @number_of_lines ,  
  @line_limit ,  
  @lower_limit,  
  @is_obligatory,  
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
  is_obligatory,  
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
  ISNULL(@line_limit,0)*@model_currency_rate,  
  0,    0,    0,    0,    0,    0,    0,    0,    0,    
  @version_id,    
  Null,    
  ISNULL(@lower_limit,0)*@model_currency_rate,   
  Null,    
  Null,    
  Null,  
  ISNULL(@is_obligatory,0),  
  @ri_model_line_id,
  0)  
 FETCH NEXT FROM copy_treaty  
 INTO @type ,  
  @treaty_id ,  
  @share_percent ,  
  @ceding_rate ,  
  @agreement_code,  
  @priority ,  
  @number_of_lines ,  
  @line_limit ,  
  @lower_limit,  
  @is_obligatory,  
  @ri_model_line_id  
 END  
 CLOSE copy_treaty  
 DEALLOCATE copy_treaty  
END  
  
GO