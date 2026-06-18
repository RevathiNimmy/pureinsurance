SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_Pay_PFInstalment'
GO


CREATE PROCEDURE spu_Pay_PFInstalment
 			@pfprem_finance_cnt INT, 
                 	@pfprem_finance_version INT,
			@InstalmentNumber INT,
			@TransactionID INT
AS



UPDATE PFInstalments
		SET status = (select PFInstalments_status_id from PFInstalments_status 
         			where description = 'Collected') ,
	    	batchNUmber = 0,
 	    	batchExportdate = (SELECT GETDATE()),
 	    	posteddate = (SELECT GETDATE()),
	    	PFtransaction_id =  @TransactionID  
    
WHERE	pfprem_finance_cnt=@pfprem_finance_cnt
AND	pfprem_finance_version=@pfprem_finance_version
AND 	instalmentnumber=@InstalmentNumber



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

