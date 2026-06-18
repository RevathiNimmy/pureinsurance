SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Execute DDLDropProcedure 'spu_ACT_delete_cashlistitem_instalments_on_pfinstalmentId'
GO

CREATE PROCEDURE spu_act_delete_cashlistitem_instalments_on_pfinstalmentId    
    @nPFinstalments_id int
AS    
BEGIN    
   DELETE FROM cashlistitem_instalments
   WHERE pfinstalments_id = @nPFinstalments_id    
END     
GO
