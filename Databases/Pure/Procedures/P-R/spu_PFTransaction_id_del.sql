SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PFTransaction_id_del'
GO

CREATE PROCEDURE spu_PFTransaction_id_del  
(  
    @pfprem_finance_cnt INT,  
    @pfprem_finance_version INT   
)  
AS  
BEGIN  
    DELETE FROM PFTransaction_id  
        Where pfprem_finance_cnt=@pfprem_finance_cnt and pfprem_finance_version=@pfprem_finance_version
END  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO