SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Check_Policy_Number_Is_Unique'
GO

CREATE PROCEDURE spu_SAM_Check_Policy_Number_Is_Unique  
  
@PolicyNo VarChar(30),  
@IsExisting Bit output  
  
AS  
Begin  
  
    If Exists(Select * from insurance_file where insurance_ref = @PolicyNo)  
        Set @IsExisting = 0  
    Else  
        Set @IsExisting = 1  
  
End  


SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
  
