SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFInstalments_postitem'
GO
CREATE PROCEDURE spe_PFInstalments_postitem
    @pfinstalments_id INT,
    @TransactionID INT
AS

DECLARE @pfprem_finance_cnt int
DECLARE @pfprem_finance_version int

UPDATE
    PFInstalments
SET
    Status = 3, -- collected
    PFTransaction_id = @TransactionID,
    PostedDate = GetDate(),
    pfinstalments_result_id = NULL
WHERE
    pfinstalments_id = @pfinstalments_id
AND PostedDate IS NULL
AND (Status = 2 OR Status = 5) -- pending or retry

SELECT @pfprem_finance_cnt = ( SELECT pfprem_finance_cnt FROM pfinstalments WHERE pfinstalments_id = @pfinstalments_id )
SELECT @pfprem_finance_version = ( SELECT pfprem_finance_version FROM pfinstalments WHERE pfinstalments_id = @pfinstalments_id )

--DC301105 PN26051 update plan if all instalments are posted
IF NOT EXISTS (  
			   SELECT NULL 
			   FROM pfinstalments  
			   WHERE pfprem_finance_cnt = @pfprem_finance_cnt 
			 	 AND pfprem_finance_version = @pfprem_finance_version     
				 AND status <> 3 
				 AND TransactionCode NOT IN (SELECT PFInstalments_Transaction_id 
											 FROM pfinstalments_transaction 
											 WHERE code in ('0N','0C'))
				)
BEGIN
	UPDATE pfpremiumfinance
	SET StatusInd = '900'
	WHERE pfprem_finance_cnt = @pfprem_finance_cnt
	AND pfprem_finance_version = @pfprem_finance_version
END

GO
