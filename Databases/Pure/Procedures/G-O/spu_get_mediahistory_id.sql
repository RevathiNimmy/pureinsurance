SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_mediahistory_id'
GO

CREATE PROCEDURE spu_get_mediahistory_id  
 @pfprem_finance_cnt  INT,  
 @pfprem_finance_version  INT,  
 @media_history_prev_id  INT = NULL OUTPUT,  
 @media_history_curr_id  INT = NULL OUTPUT  
  
AS  
 SELECT @media_history_curr_id = MAX(pfmediatypehistory_id)  
   FROM pfmediatypehistory  
   WHERE  pfprem_finance_cnt = @pfprem_finance_cnt  
    AND pfprem_finance_version = @pfprem_finance_version  
  
 SELECT @media_history_prev_id = MAX(pfmediatypehistory_id)  
   FROM pfmediatypehistory  
   WHERE  pfprem_finance_cnt = @pfprem_finance_cnt  
    AND pfprem_finance_version = @pfprem_finance_version  
    AND pfmediatypehistory_id < (SELECT MAX(pfmediatypehistory_id)  
        FROM pfmediatypehistory  
        WHERE  pfprem_finance_cnt = @pfprem_finance_cnt  
         AND pfprem_finance_version = @pfprem_finance_version)  
  
  
  

  
  
  



GO
