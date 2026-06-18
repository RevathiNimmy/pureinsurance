EXECUTE DDLDropProcedure 'spu_PFInstalments_update_bacs_status'
GO

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

CREATE  PROCEDURE spu_PFInstalments_update_bacs_status
    @pfprem_finance_cnt int,  
    @pfprem_finance_version int  
  
AS BEGIN  
  
declare @new_status_id int,   
 @retry_status_id int,  
        @onhold_status_id int,   
 @current_status_id int,  
 @daysdelay int,   
 @delayedpaymentdate datetime,  
 @recollect_on_next int,  
 @recollect_days int,   
 @retry_limit int,
 @media_history_curr_id int  
  
 select @new_status_id = ( select pfinstalments_status_id from pfinstalments_status where code = 'U' )  
 select @retry_status_id = ( select pfinstalments_status_id from pfinstalments_status where code = 'R' )  
 select @onhold_status_id = ( select pfinstalments_status_id from pfinstalments_status where code = 'H' )  
 SELECT @media_history_curr_id = MAX(pfmediatypehistory_id) FROM pfmediatypehistory
	WHERE  pfprem_finance_cnt = @pfprem_finance_cnt
	AND pfprem_finance_version = @pfprem_finance_version
   
 --update the existing auddis record or generate a new one  
 if exists (select null from pfinstalments   
   where pfprem_finance_cnt = @pfprem_finance_cnt  
   and pfprem_finance_version = @pfprem_finance_version  
   and instalmentnumber = 0  
  )  
 begin  
  --auddis record exists so update it  
  
  select @current_status_id = status   
   from pfinstalments   
   where pfprem_finance_cnt = @pfprem_finance_cnt  
   and pfprem_finance_version = @pfprem_finance_version  
   and instalmentnumber = 0  
  
  --current record is on hold or retry, so keep as is  
  if ( @current_status_id = @retry_status_id or  
   @current_status_id = @onhold_status_id   
   )  
  begin  
   update pfinstalments  
   set duedate = dateadd(day, -1, GetDate()),  
    posteddate = NULL  
   where pfprem_finance_cnt = @pfprem_finance_cnt  
   and pfprem_finance_version = @pfprem_finance_version  
   and instalmentnumber = 0 AND status NOT IN (2, 3) 
  end  
  else  
  begin  
   update pfinstalments  
   set  status = @new_status_id,  
    duedate = dateadd(day, -1, GetDate()),  
    posteddate = NULL,  
    batch_id = NULL  
   where pfprem_finance_cnt = @pfprem_finance_cnt  
   and pfprem_finance_version = @pfprem_finance_version  
   and instalmentnumber = 0 AND status NOT IN (2, 3)
  end  
 end  
 else  
 begin  
  
  DECLARE @MAXID AS INTEGER  
  SELECT @MAXID = (SELECT MAX(pfinstalments_id) + 1 FROM pfinstalments)  
  
  /*insert into pfinstalments  
  (  
  pfprem_finance_cnt,  
  pfprem_finance_version,  
  instalmentnumber,  
  duedate,  
  fee,  
  amount,  
  transactioncode,  
  status,  
  batchnumber,  
  batchexportdate,  
  posteddate,  
  pftransaction_id,  
  commission,  
  tax,  
  financefee,  
  pfinstalments_result_id,  
  batch_id,  
  loyalty_scheme_flag,  
  group_id,  
  failure_count)  
  values  
  (  
  @pfprem_finance_cnt,  
  @pfprem_finance_version,  
  0,  
  dateadd(day, -1, GetDate()),  
  0,  
  0,  
  1,  
  1,  
  NULL,  
  NULL,  
  NULL,  
  NULL,  
  0,  
  0,  
  NULL,  
  NULL,  
  NULL,  
  NULL,  
  NULL,  
  NULL    
  )*/  
  
 end  
  
 --if there is an existing instalment that is 'new', update next payment date, if before current date + mta days delay.  
 --to be current date + mta days delay  
  
 select @daysdelay = ISNULL(pfrf.daysdelay, 0)  
 from pfrf pfrf  
 join pfpremiumfinance pf on pf.schemeno = pfrf.schemeno and pf.schemeversion = pfrf.schemeversion  
 where pf.pfprem_finance_cnt = @pfprem_finance_cnt  
 and pf.pfprem_finance_version = @pfprem_finance_version  
 and pfrf.productfamily = 'MTA'  
 and pfrf.startdate <= GetDate()   
        and pfrf.enddate >= GetDate()  
  
 select @delayedpaymentdate = dateadd(dd, @daysdelay, GetDate())  
  
 update pfinstalments   
 set duedate = @delayedpaymentdate   
 where pfprem_finance_cnt = @pfprem_finance_cnt  
 and pfprem_finance_version = @pfprem_finance_version  
 and duedate < @delayedpaymentdate  
 and status = @new_status_id AND TransactionCode <>1 AND TransactionCode <>2  
  
 --cater for any instalments that are flagged as 'retry'  
  
 select  @recollect_on_next = ISNULL(pfrf.recollect_on_next, 0),  
  @recollect_days = ISNULL(pfrf.recollect_days, 0),  
  @retry_limit = ISNULL(pfrf.retry_limit, 0)  
 from pfrf pfrf  
 join pfpremiumfinance pf on pf.schemeno = pfrf.schemeno and pf.schemeversion = pfrf.schemeversion  
 where pf.pfprem_finance_cnt = @pfprem_finance_cnt  
 and pf.pfprem_finance_version = @pfprem_finance_version  
 and pfrf.productfamily = 'MTA'  
 and pfrf.startdate <= GetDate()   
        and pfrf.enddate >= GetDate()  
  
 if @recollect_on_next = 0   
 begin  
  update pfinstalments  
  set duedate = @delayedpaymentdate, failure_count = 0  
  where pfprem_finance_cnt = @pfprem_finance_cnt  
  and pfprem_finance_version = @pfprem_finance_version  
  and duedate < @delayedpaymentdate  
  and status = @retry_status_id  
  and ( ( failure_count < @retry_limit and @retry_limit <> 0 ) or @retry_limit = 0 )    
 end  
 else  
 begin  
  --as bank details changed, set the failure count back to zero,  
  --so as to have max number of retries on new details  
  --note duedate stays the same as it will be picked up on the next instalment  
  update pfinstalments  
  set failure_count = 0  
  where pfprem_finance_cnt = @pfprem_finance_cnt  
  and pfprem_finance_version = @pfprem_finance_version  
  and duedate < @delayedpaymentdate  
  and status = @retry_status_id  
  and ( ( failure_count < @retry_limit and @retry_limit <> 0 ) or @retry_limit = 0 )  
  
  --if the last instalment and no other instalment to pick this instalment up,  
  --set the due date to be current date + days delay  
  update pfinstalments  
  set duedate = @delayedpaymentdate  
  where pfprem_finance_cnt = @pfprem_finance_cnt  
  and pfprem_finance_version = @pfprem_finance_version  
  and duedate < @delayedpaymentdate  
  and status = @retry_status_id  
  and ( ( failure_count < @retry_limit and @retry_limit <> 0 ) or @retry_limit = 0 )  
  and instalmentnumber =  (  
      select max(pf2.instalmentnumber)  
      from pfinstalments pf2  
      where pf2.pfprem_finance_cnt = @pfprem_finance_cnt  
      and pf2.pfprem_finance_version = @pfprem_finance_version  
      )  
end  
  
  --PM049675(MediaHistoryID to be updated in case of change in bank/creditcard details)
  update pfinstalments
	 set pfmediatype_history_id = @media_history_curr_id
	 where pfprem_finance_cnt = @pfprem_finance_cnt
	 and pfprem_finance_version = @pfprem_finance_version
	 and [status] not in (3,4,6,10)
 
END  
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO



