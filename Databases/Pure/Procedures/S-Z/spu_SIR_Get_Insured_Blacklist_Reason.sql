SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Get_Insured_Blacklist_Reason'
GO

CREATE PROCEDURE spu_SIR_Get_Insured_Blacklist_Reason  
  
@insurance_file_cnt int  
  
AS  
  
BEGIN  
  
 DECLARE @insured_cnt int  
 Select @insured_cnt= insured_cnt from insurance_file where insurance_file_cnt = @insurance_file_cnt  
  
 EXEC spu_SIR_Party_Blacklist_Reason_Sel @insured_cnt  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
