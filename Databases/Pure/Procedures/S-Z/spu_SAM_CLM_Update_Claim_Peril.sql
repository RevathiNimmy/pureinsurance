SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Update_Claim_Peril'
GO


CREATE PROCEDURE spu_SAM_CLM_Update_Claim_Peril  
  
@claim_peril_id integer,  
@description varchar(255),  
@comments varchar(255) = '',  
@class_of_business_id integer OUTPUT,  
@class_of_business_code varchar(10) OUTPUT  
  
AS  
  
BEGIN  
  
 SELECT 
  @class_of_business_id = cob.class_of_business_id,  
  @class_of_business_code = cob.code  
  
 FROM claim_peril cp  
  
  LEFT JOIN peril_type pt ON  
   pt.peril_type_id = cp.peril_type_id  
  
  LEFT JOIN class_of_business cob ON  
   cob.class_of_business_id = pt.class_of_business_id  
  
 WHERE claim_peril_id = @claim_peril_id  
  
 UPDATE Claim_Peril  
 SET description = @description,  
 comments = @comments  
 WHERE claim_peril_id = @claim_peril_id  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
