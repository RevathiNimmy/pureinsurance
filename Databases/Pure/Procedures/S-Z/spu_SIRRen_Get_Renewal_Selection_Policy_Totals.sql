EXECUTE DDLDropProcedure 'spu_SIRRen_Get_Renewal_Selection_Policy_Totals'
GO

CREATE PROCEDURE spu_SIRRen_Get_Renewal_Selection_Policy_Totals
     @Batch_Renewal_Job_Code VARCHAR(20) ,
     @userName VARCHAR(255) = NULL 
  
AS  

BEGIN  

EXEC spu_SIRRen_Get_Renewal_Selection_Policy_List @Batch_Renewal_Job_Code=@Batch_Renewal_Job_Code,@userName=@userName,@nGetCount=1

END  
Go

