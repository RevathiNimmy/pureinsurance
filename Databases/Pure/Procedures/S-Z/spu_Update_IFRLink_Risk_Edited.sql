SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_Update_IFRLink_Risk_Edited'
GO

CREATE Procedure spu_Update_IFRLink_Risk_Edited  
@insurance_file_cnt int,  
@risk_cnt int,  
@is_Edited tinyint=1 

As  
Begin  
  
   	Update Insurance_file_Risk_Link 
        	Set is_Risk_Edited = @is_Edited  
		Where insurance_file_cnt = @insurance_file_cnt AND risk_cnt = @risk_cnt 

End

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO