SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_SuspendJobs_BatchRenewal'
GO

CREATE PROCEDURE spu_SIR_SuspendJobs_BatchRenewal(
 @BatchRenewalJobId INT,
 @is_active TINYINT)
AS  
BEGIN  
  
 UPDATE batch_renewal_job SET is_active = @is_active WHERE batch_renewal_job_id= @BatchRenewalJobId  
  
END  
GO