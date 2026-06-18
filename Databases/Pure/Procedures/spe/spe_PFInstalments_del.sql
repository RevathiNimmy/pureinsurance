Execute DDLDropProcedure 'spe_PFInstalments_del'
GO

CREATE PROCEDURE spe_PFInstalments_del  
    @pfprem_finance_cnt int,  
    @pfprem_finance_version int,  
    @InstalmentNumber int = NULL  
AS  
  
DELETE FROM  
 CashListItem_Instalments  
 WHERE pfinstalments_id IN  
    ( SELECT  pfinstalments_id  
  FROM  PFInstalments  
     WHERE  pfprem_finance_cnt = @pfprem_finance_cnt  
  AND  pfprem_finance_version = @pfprem_finance_version  
  AND  (InstalmentNumber = @InstalmentNumber OR @InstalmentNumber IS NULL)  
  AND  (Status IN(1,2,5,6))  
 )
 
 Update TransDetail
 SET PFInstalments_id = NULL
 where PFInstalments_id IN ( 
	 SELECT  pfinstalments_id
	  FROM  PFInstalments
	  WHERE  pfprem_finance_cnt = @pfprem_finance_cnt
	  AND  pfprem_finance_version = @pfprem_finance_version
	  AND  (InstalmentNumber = @InstalmentNumber OR @InstalmentNumber IS NULL)
	  AND (Status in (5,6))
 )  
  
DELETE FROM  
 Credit_Control_Item  
 WHERE pfinstalments_id IN  
    ( SELECT  pfinstalments_id  
  FROM  PFInstalments  
     WHERE  pfprem_finance_cnt = @pfprem_finance_cnt  
  AND  pfprem_finance_version = @pfprem_finance_version  
  AND  (InstalmentNumber = @InstalmentNumber OR @InstalmentNumber IS NULL)  
  AND  (Status IN (1,2,5,6))    
 )  
 
DELETE FROM PFInstalments_History
 WHERE pfinstalments_id IN  
    ( SELECT  pfinstalments_id  
  FROM  PFInstalments  
     WHERE  pfprem_finance_cnt = @pfprem_finance_cnt  
  AND  pfprem_finance_version = @pfprem_finance_version  
  AND  (InstalmentNumber = @InstalmentNumber OR @InstalmentNumber IS NULL)  
  AND  (Status in(1,2,5,6))  
 )  

DELETE FROM  
    PFInstalments  
 WHERE  
     pfprem_finance_cnt = @pfprem_finance_cnt  
 AND pfprem_finance_version = @pfprem_finance_version  
 AND (InstalmentNumber = @InstalmentNumber OR @InstalmentNumber IS NULL)  
 AND (Status in (1,2,5,6))



GO