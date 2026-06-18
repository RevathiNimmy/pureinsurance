SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_PFInstalment_Status_Update'
GO
CREATE PROCEDURE spu_ACT_PFInstalment_Status_Update
@pfinstalments_id int,
@pfinstalments_status_id int

AS
BEGIN

DECLARE @pfprem_finance_cnt INT
DECLARE @pfprem_finance_version INT

SELECT  @pfprem_finance_cnt = pfprem_finance_cnt,
        @pfprem_finance_version = pfprem_finance_version 
FROM   PFInstalments 
WHERE  pfinstalments_id = @pfinstalments_id


UPDATE pfinstalments
SET status = @pfinstalments_status_id
WHERE pfinstalments_id = @pfinstalments_id

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
END
GO
