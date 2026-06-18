SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_wp_ChaseCycleItem'
GO
CREATE PROCEDURE spu_wp_ChaseCycleItem      
    @PartyCnt INT,      
    @InsuranceFileCnt INT,      
    @RiskId INT,      
    @ClaimCnt INT,      
    @DocumentRef VARCHAR(25),      
    @Instance1 INT,      
    @Instance2 INT,      
    @Instance3 INT      
AS      
        
    SELECT will_auto_cancel = CCI.can_auto_cancel      
      
    FROM chase_cycle_item CCI    
    
      
    WHERE   CCI.insurance_file_cnt = @InsuranceFileCnt      
       AND CCI. chase_cycle_item_id = @Instance2
	   
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
