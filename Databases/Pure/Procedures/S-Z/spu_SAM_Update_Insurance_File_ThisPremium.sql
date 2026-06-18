SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Update_Insurance_File_ThisPremium'
GO

/**********************************************************************************************************/  
/* spu_SAM_Update_Insurance_File_ThisPremium                                                              */  
/*                                                                                        	          */  
/* Update the Insurance file table and sets the insurance file this premium to the same as rating section */		
/**********************************************************************************************************/  

CREATE  PROCEDURE spu_SAM_Update_Insurance_File_ThisPremium
@insurance_file_cnt INT,
@insurance_file_type_code VARCHAR(20)= NULL  
AS  

DECLARE @ThisPremium money  
 
IF ( ISNULL (@insurance_file_type_code,'')='')
BEGIN  
	select @ThisPremium = sum(rating_section.this_premium)  
	from rating_section  
	INNER JOIN insurance_file_risk_link ifrl  
	 on rating_section.risk_cnt = ifrl.risk_cnt  
	where ifrl.Insurance_File_cnt = @insurance_file_cnt  
	  
	UPDATE  Insurance_File  
	SET  insurance_file.this_premium = @ThisPremium  
	WHERE insurance_file_cnt = @insurance_file_cnt  
END
ELSE
BEGIN
DECLARE @insurance_file_type_id INT

SELECT @insurance_file_type_id=Insurance_File_type_id FROM Insurance_File_Type WHERE Code=@insurance_file_type_code

UPDATE  Insurance_File  
	SET  Insurance_File_type_id = @insurance_file_type_id  
	WHERE insurance_file_cnt = @insurance_file_cnt  

END
  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
