SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_sir_rating_section_sel'
  
GO

 CREATE PROCEDURE spu_sir_rating_section_sel  
                @Insurance_File_cnt INT,  
                @Risk_Id            INT  
AS  
  
SELECT   RST.Description,  
           PST.Description,  
           RT.Description,  
           RS.Annual_Rate,  
           RS.Sum_Insured,               
	   RS.This_Premium,  
           RS.annual_premium, 
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
           RS.override_reason,  
           RS.Auto_Calculated,  
           EP.description,  
           EP.earning_pattern_id,  
     0,  
         0,  
--UpdateRating Details method is expecting code..so we need to pass code instead of key.  
           RST.code,  
           RT.code,  
           C.code,  
           S.code,  
           EP.code,  
           Currency.code  
--UpdateRating Details method is expecting code..so we need to pass code instead of key.  
  
  FROM     Rating_Section RS  
           JOIN Rating_Section_Type RST  
             ON RST.Rating_Section_type_id = RS.Rating_Section_Type_Id  
           JOIN Rate_Type RT  
             ON RT.Rate_Type_id = RS.Rate_Type_id  
           LEFT JOIN Policy_Section_type PST  
             ON PST.Policy_Section_type_id = RS.Policy_Section_Type_id  
           LEFT JOIN Country C  
             ON C.country_id = RS.country_id  
           LEFT JOIN State S  
             ON s.state_id = RS.state_id  
           JOIN Earning_Pattern EP  
             ON EP.Earning_Pattern_Id = RS.Earning_Pattern_Id  
           LEFT JOIN Currency  
    ON RS.currency_id = Currency.currency_id  
  WHERE RS.Risk_cnt = @Risk_Id  
  ORDER BY RS.Rating_Section_id  
  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
