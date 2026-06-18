EXECUTE DDLDropProcedure 'spu_Get_Portfolio_Transfer_Date'
GO
  
CREATE PROCEDURE spu_Get_Portfolio_Transfer_Date
    @dtTransfer_date DATE OUTPUT
AS  
  
DECLARE @nYearName INT  
DECLARE @dtPeriodEndDate DATE

SELECT @nYearName = year_name FROM period WHERE period_end_date = DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE())+1,0))

SELECT TOP 1 @dtPeriodEndDate = period_end_date  
FROM Period  
WHERE  year_name =@nYearName  
ORDER BY period_end_date  
  
SELECT @dtTransfer_date =DATEADD(m,DATEDIFF(m,0,@dtPeriodEndDate),0)  

GO