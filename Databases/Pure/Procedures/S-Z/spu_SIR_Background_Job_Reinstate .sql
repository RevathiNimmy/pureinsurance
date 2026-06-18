Execute DDLDropProcedure 'spu_SIR_Background_Job_Reinstate'
GO
CREATE PROCEDURE spu_SIR_Background_Job_Reinstate (

	@nbackground_job_id INT,
	@nAllowRetryCnt INT,
    @nReturnflag TInyINt  OutPut
		
	)

AS BEGIN
 
 Declare  @ntotRetriedCnt As Integer
 Set @ntotRetriedCnt=0
 Select  @ntotRetriedCnt=ISNUll(job_retry_count,0)   from  Background_Job Where background_job_id=@nbackground_job_id
  
If  @ntotRetriedCnt <= @nAllowRetryCnt

  Begin
        UPDATE	Background_Job
		        SET	job_status = 'W',
				job_retry_count=ISNUll(job_retry_count,0)+1,
				last_job_retry_time=GetDate()
		        
        WHERE 	background_job_id = @nbackground_job_id

	Set @nReturnflag =1

  End

  Else
      
	   Begin

	              Set @nReturnflag =0

	   End


 END

 GO
     