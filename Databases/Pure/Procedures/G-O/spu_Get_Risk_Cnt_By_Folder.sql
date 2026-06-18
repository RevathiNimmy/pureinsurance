SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_risk_cnt_by_folder'
GO

CREATE PROCEDURE spu_get_risk_cnt_by_folder  
    @insurance_file_cnt INT,  
    @risk_folder_cnt INT,  
@nOriginalrisk_cnt INT=0,  
@sTransactiontype VARCHAR(20)=NULL  
AS  
  
IF @sTransactiontype='NB'  
  BEGIN  
  
  SELECT  
   r.risk_cnt  
  FROM  
   insurance_file_risk_link ifrl  
  JOIN  
   risk r  
   ON  
    r.risk_cnt = ifrl.risk_cnt  
  WHERE  
   ifrl.insurance_file_cnt = @insurance_file_cnt  
   AND r.risk_folder_cnt = @risk_folder_cnt  
   AND  ifrl.risk_cnt= @nOriginalrisk_cnt  
  
  END  
Else  
BEGIN  
  SELECT  
   MAX(r.risk_cnt) risk_cnt  
  FROM  
   insurance_file_risk_link ifrl  
  JOIN  
   risk r  
   ON  
    r.risk_cnt = ifrl.risk_cnt  
  WHERE  
   ifrl.insurance_file_cnt = @insurance_file_cnt  
   AND r.risk_folder_cnt = @risk_folder_cnt  
   AND ( ( ( ifrl.status_flag='C' And ifrl.original_risk_cnt= @nOriginalrisk_cnt  
        OR ifrl.status_flag='U' And ifrl.risk_cnt= @nOriginalrisk_cnt )  
  
      ) OR ISNULL(@nOriginalrisk_cnt,0)=0)  
      HAVING MAX(r.risk_cnt) IS NOT NULL
  
END  
GO



