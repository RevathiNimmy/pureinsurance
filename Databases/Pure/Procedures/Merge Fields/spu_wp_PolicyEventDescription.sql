SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON  SET NOCOUNT ON
GO
    
EXECUTE DDLDropProcedure 'spu_wp_PolicyEventDescription'
  
GO

CREATE PROCEDURE spu_wp_PolicyEventDescription
    @PartyCnt INT,      
    @InsuranceFileCnt INT,      
    @RiskId INT,      
    @ClaimCnt INT,      
    @DocumentRef VARCHAR(25),      
    @Instance1 INT,      
    @Instance2 INT,
    @Instance3 INT

AS

    DECLARE @Single_event_desc    	VARCHAR(255)
    DECLARE @manual_event_desc 	        VARCHAR(255)
    DECLARE @StartPos   		INT

    SELECT @Single_event_desc = el.description      
    FROM Event_Log el      
    WHERE el.insurance_file_cnt = @InsuranceFileCnt      
    AND  el.event_type_id = 5      
    
    SELECT @manual_event_desc = el.description     
    FROM Event_Log el      
    WHERE el.insurance_file_cnt = @InsuranceFileCnt      
    AND  el.is_manual_description = 1      


SET 	@StartPos = CHARINDEX(']', @manual_event_desc,0)
IF 	@StartPos <> 0
   	SET @manual_event_desc =  RIGHT(@manual_event_desc,LEN(@manual_event_desc) - @StartPos)
	
SELECT @Single_event_desc 'EventDesc', @Manual_event_desc 'ManualEventDesc'    
