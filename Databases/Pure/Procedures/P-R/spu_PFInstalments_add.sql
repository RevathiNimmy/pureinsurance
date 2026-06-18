EXECUTE DDLDropProcedure 'spu_PFInstalments_add'
GO

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_PFInstalments_add    
    @pfprem_finance_cnt int,    
    @pfprem_finance_version int,    
    @InstalmentNumber int,    
    @DueDate datetime,    
    @Fee numeric(19,4),    
    @Tax numeric(19,4),    
    @Commission numeric(19,4),    
    @Amount numeric(19,4),    
    @TransactionCode int,    
    @Status int,    
    @batch_id int = NULL,    
    @BatchNumber int = NULL,    
    @BatchExportDate datetime = NULL,    
    @PostedDate datetime = NULL,    
    @PFTransaction_id int = NULL,    
    @pfinstalments_result_id int = NULL,  
    @pfmediatype_history_id  INT = NULL  
    
AS BEGIN    
    
    INSERT INTO    
        PFInstalments(    
        pfprem_finance_cnt,    
        pfprem_finance_version,    
        InstalmentNumber,    
        DueDate,    
        Fee,    
        Tax,    
        Commission,    
        Amount,    
        TransactionCode,    
        Status,    
        batch_id,    
        BatchNumber,    
        BatchExportDate,    
        PostedDate,    
        PFTransaction_id,    
        pfinstalments_result_id,  
 	pfmediatype_history_id,
	original_DueDate)    
    VALUES (    
        @pfprem_finance_cnt,    
        @pfprem_finance_version,    
        @InstalmentNumber,    
        @DueDate,    
        @Fee,    
        @Tax,    
        @Commission,    
        @Amount,    
        @TransactionCode,    
        @Status,    
        @batch_id,    
        @BatchNumber,    
        @BatchExportDate,    
        @PostedDate,    
        @PFTransaction_id,    
        @pfinstalments_result_id,  
 	@pfmediatype_history_id,
	@DueDate)    
END    
GO
  
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
