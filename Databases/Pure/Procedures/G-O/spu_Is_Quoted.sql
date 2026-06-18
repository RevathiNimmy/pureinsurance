SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Is_Quoted'
GO


CREATE PROCEDURE spu_Is_Quoted
    @insurance_file_cnt int
AS

/********************************************************************************************************
-- Desc : are all the risks quoted
-- Hist : 09/07/2001 TN - Created
**********************************************************************************************************/
If exists(Select * from insurance_file_risk_link where insurance_file_cnt = @insurance_file_cnt) Begin
	SELECT  count(r.risk_cnt)  
	FROM    insurance_file_risk_link rfrl,  
	    risk r  
	WHERE   rfrl.risk_cnt = r.risk_cnt  
	AND rfrl.status_flag <> 'D'                             -- not deleted  
	AND (r.risk_status_id not in (3,9) or r.risk_status_id is null) -- not quoted  
	AND rfrl.insurance_file_cnt = @insurance_file_cnt
     End
Else
     Begin
	Select 1
     End
GO

