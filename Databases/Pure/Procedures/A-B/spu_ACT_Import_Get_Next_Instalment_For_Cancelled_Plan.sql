SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Import_Get_Next_Instalment_For_Cancelled_Plan'
GO

CREATE PROCEDURE spu_ACT_Import_Get_Next_Instalment_For_Cancelled_Plan  
  
@pfprem_finance_cnt int,  
@pfprem_finance_version int,  
@instalment_id int OUTPUT,  
@amount money OUTPUT  
  
AS  
  
BEGIN  


--DECLARE @pfprem_finance_cnt int,  
--@pfprem_finance_version int,  
--@instalment_id int ,  
--@amount money   
  
--SET @pfprem_finance_cnt = 1
--SET @pfprem_finance_version =1 

 DECLARE @last_processed_intalment_id int  
  
 -- get the last instalment that was processed (set to status of (pending / completed))  
-- SELECT @last_processed_intalment_id =ISNULL(MAX(pfinstalments_id),0) FROM pfinstalments  
-- WHERE pfprem_finance_cnt = @pfprem_finance_cnt  
-- AND pfprem_finance_version = @pfprem_finance_version  
-- AND status in (SELECT pfinstalments_status_id from pfinstalments_status  
-- WHERE code in ('P','C'))  

SELECT @last_processed_intalment_id
  
 DECLARE @next_instalment_to_be_processed int  
  
 -- get the next instalment to be procesed ( where the status is set to one of (new / retrying / on hold))  
 -- and the instalment id is greater than the last one that was processed  
 -- dont pick up ( create ddi or cancel ddi ) instalment entries  
 SELECT @instalment_id =  MIN(pfinstalments_id)  
 FROM pfinstalments  
 WHERE status IN (1,5,7)  
 --AND pfinstalments_id > @last_processed_intalment_id  
 AND pfprem_finance_cnt = @pfprem_finance_cnt  
 AND pfprem_finance_version = @pfprem_finance_version  
 AND transactioncode NOT IN (1,2)  
  
 SELECT @amount = amount from pfinstalments where pfinstalments_id = @instalment_id  
  

 SELECT @amount, @instalment_id
END  



GO
