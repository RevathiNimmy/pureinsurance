SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Skip_New_Policy_Number'
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE spu_Skip_New_Policy_Number  
     @insurance_file_cnt int,  
@bSkipNewPolicyNumber int  OUTPUT  
AS 


DECLARE @iIndexCount int 

SET @bSkipNewPolicyNumber = 0

SELECT @iIndexCount = CHARINDEX('_',SUBSTRING(ifi.insurance_ref,LEN(ifi.insurance_ref)-2,3))  
FROM insurance_file ifi
INNER JOIN product p ON ifi.product_id = p.product_id  
WHERE ifi.insurance_file_cnt =  @insurance_file_cnt
and p.is_retain_policy_number_on_copy = 1 


IF @iIndexCount > 0 
BEGIN
set @bSkipNewPolicyNumber = 1
END



 