SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Import_Create_0N_Instalment'
GO

CREATE PROCEDURE spu_ACT_Import_Create_0N_Instalment  
  
@pfprem_finance_cnt int,  
@pfprem_finance_version int  
  
AS  
  
  
DECLARE @pfinstalments_status_id int  
SELECT @pfinstalments_status_id = pfinstalments_status_id   
FROM pfinstalments_status   
WHERE code = 'U'  
  
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
tax  
)  
  
SELECT TOP 1  
@pfprem_finance_cnt,  
@pfprem_finance_version,   
0 instalmentnumber,   
getdate(),   
0 fee,   
0 amount,   
transactioncode,   
1 status,  
0 commission,   
0 tax

FROM pfinstalments  
 WHERE pfprem_finance_cnt = @pfprem_finance_cnt  
 AND pfprem_finance_version = @pfprem_finance_version  
 AND transactioncode in (  
 select pfinstalments_transaction_id   
 from pfinstalments_transaction   
 where code = '0N')  
  


GO
