--Start (Sriram P) - (Tech Spec - UIIC WR22 – Capture Quote Details – Get Rating Section Types.doc)(6.1.1)

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SAM_get_rating_section_types'
GO

CREATE PROCEDURE spu_SAM_get_rating_section_types 

   @Insurance_File_cnt INT  
  
AS  
  BEGIN  
  
  DECLARE  @CoverStartDate DATETIME  
  
  SELECT @CoverStartDate = cover_start_date  
  FROM   insurance_file  
  WHERE  insurance_file_cnt = @Insurance_File_cnt  
  
    SELECT RST.Rating_Section_Type_id,  
           RST.Code,  
           RST.Description,  
           RST.Rate_Type_id,
	   RT.code rate_code,  
           RST.Rate,  
           RST.currency_id,
	   cr.code currency_code,  
	   RST.country_id,  
   	   c.code country_code,  
           RST.state_id,  
           s.code state_code,
           (SELECT code  
            FROM   Earning_Pattern_Usage eu Left Join Earning_Pattern e 
		   on e.Earning_Pattern_id=eu.Earning_Pattern_id
            WHERE  Rating_Section_type_id = RST.rating_section_type_id  
                   AND Earning_Pattern_Usage_id = (SELECT MAX(Earning_Pattern_Usage_id)  
                                                   FROM   Earning_Pattern_Usage  
                                                   WHERE  Rating_Section_type_id = RST.rating_section_type_id  
                AND effective_date <= @CoverStartDate)) 'Earning_Pattern_code'  
    FROM   Rating_Section_Type RST  LEFT JOIN rate_type RT on RST.rate_type_id=RT.rate_type_id
    LEFT JOIN Currency cr on RST.currency_id=cr.currency_id
    LEFT JOIN Country c on RST.country_id=c.country_id
    LEFT JOIN State s on RST.state_id=s.state_id
    WHERE  RST.Is_Deleted = 0  
  
  END  

GO 
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

--End (Sriram P) - (Tech Spec - UIIC WR22 – Capture Quote Details – Get Rating Section Types.doc)(6.1.1)


