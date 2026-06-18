SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Update_Risk_Status'
GO

CREATE PROCEDURE spu_SIR_Update_Risk_Status  
  
@risk_cnt int =0,  
@risk_status_code varchar(20),  
@insurance_file_cnt INT =0 ,
@ApplyCheck INT=0 
  
AS  
  
IF @risk_cnt<>0  
 BEGIN  
    UPDATE risk SET risk_status_id = (Select risk_status_id from risk_status where code =@risk_status_code)  
   WHERE risk_cnt = @risk_cnt  
   IF @risk_status_code='QUOTED'  
   BEGIN  
 Update insurance_file_pt_log set status_id=2 where risk_cnt=@risk_cnt and status_id=1  
 Update insurance_file_clone_log set status_id=2 where risk_cnt=@risk_cnt and status_id=1  
   END  
END  
ELSE IF @insurance_file_cnt<>0  
 BEGIN  
   DECLARE c_risk CURSOR FAST_FORWARD FOR  
          SELECT r.risk_cnt  
            FROM risk r  
       INNER JOIN insurance_file_risk_link ifrl  
              ON r.risk_cnt = ifrl.risk_cnt  
           WHERE insurance_file_cnt = @insurance_file_cnt  
           AND (ISNULL(@ApplyCheck,0)=0 OR(@ApplyCheck=1 AND (ifrl.status_flag NOT IN ('U','D') AND isnull(ifrl.is_risk_edited,0) =1)))

   OPEN c_risk  
      FETCH NEXT FROM c_risk INTO @risk_cnt  
      WHILE @@FETCH_STATUS = 0  
       BEGIN  
           UPDATE risk  
              SET risk_status_id = (Select risk_status_id from risk_status where code =@risk_status_code)  
 WHERE risk_cnt = @risk_cnt  
  
           FETCH NEXT FROM c_risk INTO @risk_cnt  
       END  
      CLOSE c_risk  
      DEALLOCATE c_risk  
  
 END  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

