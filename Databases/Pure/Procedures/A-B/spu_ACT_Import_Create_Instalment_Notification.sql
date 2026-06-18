SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Import_Create_Instalment_Notification'
GO

CREATE PROCEDURE spu_ACT_Import_Create_Instalment_Notification    
    
@pfprem_finance_cnt int,    
@pfprem_finance_version int,    
@pfinstalments_transaction_code varchar(10),    
@pfmedia_history_id INT = NULL  
  
AS    
    
DECLARE @pfinstalments_status_id int    
SELECT @pfinstalments_status_id = pfinstalments_status_id    
FROM pfinstalments_status    
WHERE code = 'U'    
    
DECLARE @pfinstalments_transaction_id int    
SELECT @pfinstalments_transaction_id =pfinstalments_transaction_id    
FROM pfinstalments_transaction    
WHERE code = @pfinstalments_transaction_code    
    
INSERT INTO pfinstalments    
(    
pfprem_finance_cnt,    
pfprem_finance_version,    
instalmentnumber,    
duedate,    
fee,    
amount,    
transactioncode,    
status,    
commission,    
tax,  
pfmediatype_history_id    
)    
    
VALUES (    
@pfprem_finance_cnt,    
@pfprem_finance_version,    
0,    
getdate(),    
0,    
0,    
@pfinstalments_transaction_id,    
@pfinstalments_status_id,    
0,    
0,  
@pfmedia_history_id)    
  
  



GO
