
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_ACT_GetSetReleaseManualTransProcessFlag'
GO

CREATE PROCEDURE spu_ACT_GetSetReleaseManualTransProcessFlag  
    @IsStarted INT = NULL,
    @ReturnValue INT = NULL OUTPUT  
AS  
       
  SET @ReturnValue = 0
  
  IF @IsStarted IS NULL 
  BEGIN
    IF EXISTS(SELECT * FROM ReleaseManualTransProcess 
               WHERE ProcessRunning = 1 AND DATEDIFF(mi,StartTime, GETDATE()) < 5)
        SET @ReturnValue = 1  
    ELSE
  	BEGIN
  	  	 Set @ReturnValue = 0        
  	  	 UPDATE ReleaseManualTransProcess SET ProcessRunning = 0 
  	END
  END	
  Else
  	 UPDATE ReleaseManualTransProcess SET ProcessRunning = @IsStarted, StartTime = GETDATE()   
  	     
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO                         

 