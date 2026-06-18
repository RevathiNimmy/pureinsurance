SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_fee_amounts_u_del'
GO


CREATE PROCEDURE spu_fee_amounts_u_del    
    @fee_amount_id   int      
AS    
  
Update Fee_amounts  Set is_deleted = 1 
WHERE fee_amount_id = @fee_amount_id    





GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
