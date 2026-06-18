SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Renewal_Get_Payment_Method_Del_PF'
GO
CREATE  PROCEDURE spu_Renewal_Get_Payment_Method_Del_PF 
    @Insurance_file_cnt int, 
    @Payment_Method  varchar(60) output    
 
AS  
	IF @Insurance_file_cnt!=0  
		BEGIN
			SELECT @Payment_Method=Payment_Method FROM  Insurance_file WHERE insurance_file_cnt=@Insurance_file_cnt  
			IF UPPER(LTRIM(RTRIM(@Payment_Method)))='PAYNOW'OR UPPER(LTRIM(RTRIM(@Payment_Method)))='INVOICE'OR
			UPPER(LTRIM(RTRIM(@Payment_Method)))='BANKGUARANTEE'
				BEGIN
					DELETE FROM PFpremiumfinance  WHERE insurance_file_cnt=@Insurance_file_cnt   
				END  
			 SELECT @Payment_Method=UPPER(LTRIM(RTRIM(@Payment_Method)))
		END

 GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 

 