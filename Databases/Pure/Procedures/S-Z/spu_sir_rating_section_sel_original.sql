SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


EXECUTE DDLDropProcedure 'spu_sir_rating_section_sel_original'
GO

CREATE   PROCEDURE spu_sir_rating_section_sel_original
    @Insurance_File_cnt Int,
  @Risk_Id Int,
    @New_Risk_Id Int = 0
AS

Declare   
      @Transtype integer,  
      @InsuranceFileType Integer,  
      @this_premium Integer,
      @new_Insurance_File_cnt Integer 
      select @new_Insurance_File_cnt = ISNULL(insurance_file_cnt,0) from insurance_file_risk_link where risk_cnt = @New_Risk_Id
      select @Transtype = ISNULL(last_trans_type_id,0) from Insurance_File_System where insurance_file_cnt = @new_Insurance_File_cnt  
      select @InsuranceFileType = ISNULL(Insurance_File_Type_id,0) from Insurance_File where insurance_file_cnt = @Insurance_File_cnt  
      select @this_premium = ISNULL(SUM(this_premium),0) from Rating_Section where risk_cnt = @Risk_Id and Original_flag=0  
if @InsuranceFileType IN (5,6,8,9)  
BEGIN  
SELECT  RST.Description ,  
        PST.Description ,  
        RT.Description,  
        RS.Annual_Rate,  
        RS.Sum_Insured,  
        RS.Annual_Premium,  
        RS.This_Premium,  
        C.Description,  
        S.Description,  
        RS.Rating_Section_id,  
        RS.Rating_Section_Type_id,  
        RS.Policy_Section_Type_Id,  
        RS.Rate_Type_Id,  
        RS.Original_Flag,  
        RS.currency_id,  
        RS.country_id,  
        RS.state_id,  
        RS.is_amended,  
        RS.calculated_premium,  
        RS.override_reason  
FROM    Rating_Section RS  
JOIN    Rating_Section_Type RST ON RST.Rating_Section_type_id = RS.Rating_Section_Type_Id  
JOIN    Rate_Type RT ON RT.Rate_Type_id = RS.Rate_Type_id  
LEFT JOIN  
        Policy_Section_type PST ON PST.Policy_Section_type_id = RS.Policy_Section_Type_id  
LEFT JOIN  
        Country C ON C.country_id = RS.country_id  
LEFT JOIN  
        State S ON s.state_id = RS.state_id  
WHERE   RS.Risk_cnt = @Risk_Id 
ORDER BY  
        RS.Rating_Section_id  
END 
ELSE  
BEGIN  
  
SELECT  RST.Description ,  
        PST.Description ,  
        RT.Description,  
        RS.Annual_Rate,  
        RS.Sum_Insured,  
        RS.This_Premium,
        RS.Annual_Premium,  
        C.Description,  
        S.Description,  
        RS.Rating_Section_id,  
        RS.Rating_Section_Type_id,  
        RS.Policy_Section_Type_Id,  
        RS.Rate_Type_Id,  
        RS.Original_Flag,  
        RS.currency_id,  
        RS.country_id,  
        RS.state_id,  
        RS.is_amended,  
        RS.calculated_premium,  
        RS.override_reason  
FROM    Rating_Section RS  
JOIN    Rating_Section_Type RST ON RST.Rating_Section_type_id = RS.Rating_Section_Type_Id  
JOIN    Rate_Type RT ON RT.Rate_Type_id = RS.Rate_Type_id  
LEFT JOIN  
        Policy_Section_type PST ON PST.Policy_Section_type_id = RS.Policy_Section_Type_id  
LEFT JOIN  
        Country C ON C.country_id = RS.country_id  
LEFT JOIN  
        State S ON s.state_id = RS.state_id  
WHERE   RS.Risk_cnt = @Risk_Id and RS.Original_flag=0  
ORDER BY  
        RS.Rating_Section_Id
END        


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

