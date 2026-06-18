SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

DDLDropProcedure 'spu_get_all_rating_section_types'
GO

 CREATE PROCEDURE spu_get_all_rating_section_types  
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
           RST.Rate,  
           RST.currency_id,  
           RST.country_id,  
           RST.state_id,  
           (SELECT Earning_Pattern_id  
            FROM   Earning_Pattern_Usage  
            WHERE  Rating_Section_type_id = RST.rating_section_type_id  
                   AND Earning_Pattern_Usage_id = (SELECT MAX(Earning_Pattern_Usage_id)  
                                                   FROM   Earning_Pattern_Usage  
                                                   WHERE  Rating_Section_type_id = RST.rating_section_type_id  
       							  AND effective_date <= @CoverStartDate)) 'Earning_Pattern_id'  
    FROM   Rating_Section_Type RST  
    WHERE  Is_Deleted = 0  
                          
  END
  
GO