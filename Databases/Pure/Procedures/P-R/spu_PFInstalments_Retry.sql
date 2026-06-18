SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PFInstalments_Retry'
GO

CREATE PROCEDURE spu_PFInstalments_Retry  
  
@pfinstalments_id int,  
@failurereason int OUTPUT  
  
AS  
  
BEGIN  
  
 DECLARE @pfinstalments_due_date datetime  
 DECLARE @pfinstalment_failure_count int  
 DECLARE @pfrf_recollect_on_next int  
 DECLARE @pfrf_recollect_days int  
 DECLARE @pfrf_retry_limit int  
 DECLARE @pfprem_finance_cnt int  
 DECLARE @pfprem_finance_version int  
 DECLARE @duedate datetime  
  
 -- initial retry failure reason  
 SET @failurereason = 0  
  
 -- get details for specified instalment  
 SELECT  
  @pfprem_finance_cnt = pfpremiumfinance.pfprem_finance_cnt,  
  @pfprem_finance_version = pfpremiumfinance.pfprem_finance_version,  
  @pfinstalments_due_date = pfinstalments.duedate,  
  @pfinstalment_failure_count = ISNULL(pfinstalments.failure_count,0),  
  @pfrf_recollect_on_next = ISNULL(pfrf.recollect_on_next,0),  
  @pfrf_recollect_days = ISNULL(pfrf.recollect_days,0),  
  @pfrf_retry_limit = ISNULL(pfrf.retry_limit,0)  
  
 FROM pfinstalments  
  
  INNER JOIN pfpremiumfinance ON  
   pfinstalments.pfprem_finance_cnt = pfpremiumfinance.pfprem_finance_cnt  
             AND pfinstalments.pfprem_finance_version = pfpremiumfinance.pfprem_finance_version  
  
  INNER JOIN pfrf ON  
   pfpremiumfinance.pfrf_id = pfrf.pfrf_id  
  
 WHERE pfinstalments.pfinstalments_id = @pfinstalments_id  
  
  -- update failure count to indicate this has been retried
  UPDATE pfinstalments  
  SET failure_count = @pfinstalment_failure_count + 1
  WHERE pfinstalments_id = @pfinstalments_id  

 -- if the instalment failure count including this attempt  
 -- is greater than the retry limit  
 IF (@pfinstalment_failure_count + 1) > @pfrf_retry_limit  
 BEGIN  
  -- set pfinstalment status to failed  
  UPDATE pfinstalments  
  SET status = (Select pfinstalments_status_id from pfinstalments_status where code = 'F')  
  WHERE pfinstalments_id = @pfinstalments_id  
  
  -- return failure status so that calling code can  
  -- raise a work manager task to indicate it has failed  
  SET @failurereason = 1 -- retry limit reached  
 END  
 ELSE  
 BEGIN  
  -- determine the instalments new due date  
  -- if collect on the next due instalment date  
   IF @pfrf_recollect_on_next = 1  
  BEGIN  
    -- get the next due date - this is the next  
      -- duedate for an instalment on the same plan with  
    -- a status of "new" or "retry"  
    SELECT @duedate = Min(duedate)  
    FROM pfinstalments  
    WHERE pfprem_finance_cnt = @pfprem_finance_cnt  
    AND pfprem_finance_version = @pfprem_finance_version  
    AND status IN (SELECT pfinstalments_status_id FROM pfinstalments_status WHERE code in ('U', 'R'))  
    AND duedate > GetDate()  
   
  END  
  ELSE  
  BEGIN  
   DECLARE @OriginalDueDate AS DATETIME
   SELECT @OriginalDueDate = DueDate FROM PFInstalments WHERE pfinstalments_id = @pfinstalments_id
   IF @OriginalDueDate <= GETDATE()
   BEGIN
   SELECT @duedate = DATEADD(d, @pfrf_recollect_days,  GetDate())
   END
   ELSE
   BEGIN
   SELECT @duedate = DATEADD(d, @pfrf_recollect_days,  @OriginalDueDate)
   END
  END  
  SELECT @duedate = CAST(@duedate as Date)
  -- update pfinstalment  
  -- set status to retrying and duedate to the new duedate  
  UPDATE pfinstalments  
  SET duedate = ISNULL(@duedate,duedate),
  status = (Select pfinstalments_status_id from pfinstalments_status where code = 'R'),
  transactioncode = (Select pfinstalments_transaction_id from pfinstalments_transaction where code = '18')  
  WHERE pfinstalments_id = @pfinstalments_id  
 END  
END  



GO
