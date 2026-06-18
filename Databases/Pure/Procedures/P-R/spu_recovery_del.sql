SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_recovery_del'
GO

CREATE PROCEDURE spu_recovery_del  
    @recovery_id int  
AS  
  
    DELETE FROM recovery  
        WHERE recovery_id = @recovery_id  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
