SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Claim_Details_By_Claim_Number'
GO

CREATE PROCEDURE spu_SAM_Get_Claim_Details_By_Claim_Number  
  
@claim_number varchar(30)  
  
AS  
  
 SELECT version_id , base_claim_id
 FROM claim  
 WHERE claim_number = @claim_number  

 ORDER By version_id DESC


GO
