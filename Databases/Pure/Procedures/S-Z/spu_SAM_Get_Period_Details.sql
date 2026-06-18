
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_SAM_Get_Period_Details
GO
CREATE PROCEDURE spu_SAM_Get_Period_Details  
    @IncludeAllocatedOnly INT=0  
AS  
  
BEGIN  
  
--IF @IncludeAllocatedOnly=1  
--BEGIN  
--SELECT DISTINCT P.Period_name 'PeriodName',P.Year_name 'YearName', P.Period_id 'PeriodId'  
--FROM Period P  
--JOIN TransDetail TD ON TD.period_id = P.period_id  
--JOIN AllocationDetail AD ON AD.transdetail_id =TD.transdetail_id  
--END  
--ELSE  
--BEGIN  
--SELECT DISTINCT P.Period_name 'PeriodName',P.Year_name 'YearName', P.Period_id 'PeriodId'  
--FROM Period P  
--END  
  

SELECT 
P.period_name 'PeriodName', 
P.year_name 'YearName', 
P.period_id 'PeriodId' , 
'AllocationIndicator' = Case when UnAllocated.period_id = P.period_id Then 0 ELSE 1 END FROM 
(
	--Get all the period id where any of the transaction is not allocated
	SELECT Trans.period_id 'period_id' FROM  
	(	
		--Get all the Transactions where outstanding amount  <> 0
		SELECT TD.outstanding_amount,TD.period_id FROM TransDetail TD WHERE ISNULL(TD.outstanding_amount,0) <> 0
	) AS Trans GROUP BY Trans.period_id
) AS UnAllocated

RIGHT JOIN Period P ON P.Period_id = UnAllocated.period_id
ORDER BY P.period_id 
 
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO





