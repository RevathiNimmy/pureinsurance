SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_BankGuarantee_Details_DelUndel'
GO

CREATE PROCEDURE spu_BankGuarantee_Details_DelUndel    
 @bg_id  INT,    
 @delete  INT    
AS    
    
BEGIN    
                DECLARE @BG_status INT    
    
                IF @delete = 1    
                   SET @BG_status = 4    
                ELSE    
                   SET @BG_status = 1    
    
                UPDATE Bank_Guarantee    
                 SET is_deleted = @delete,    
                 bg_status_id = @BG_status    
                 WHERE bg_id=@bg_id    
    
    
END    
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
  
