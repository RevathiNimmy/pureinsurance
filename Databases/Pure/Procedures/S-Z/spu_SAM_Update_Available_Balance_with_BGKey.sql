SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_SAM_Update_Available_Balance_with_BGKey'
GO

CREATE procedure spu_SAM_Update_Available_Balance_with_BGKey    
@BG_ID INT,    
@Available_Bal numeric(20,2)    
    
AS    
    
 	UPDATE Bank_Guarantee SET available_bal=available_bal+@Available_Bal WHERE bg_id=@BG_ID 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

