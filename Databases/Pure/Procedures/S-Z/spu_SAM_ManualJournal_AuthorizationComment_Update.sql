GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_ManualJournal_AuthorizationComment_Update'
GO


CREATE PROCEDURE spu_SAM_ManualJournal_AuthorizationComment_Update      
  @manualJournalId INT,      
  @sDescription VARCHAR(MAX)      
      
 AS      
BEGIN  
DECLARE @DeclineComment VARCHAR(200)  
  
UPDATE ManualJournal SET Authorisation_comment = @sDescription + CHAR(10) + ISNULL(Authorisation_comment,'')  WHERE ManualJournal_id =@manualJournalId       
  
IF EXISTS(SELECT NULL FROM ManualJournal WHERE ManualJournal_id=@manualJournalId AND is_reffered =2)  
BEGIN  
 SELECT @DeclineComment = LTRIM(RTRIM(LEFT(@sDescription, CHARINDEX('-', @sDescription) - 1))) WHERE CHARINDEX('-', @sDescription) > 0  
     
 UPDATE PMWrk_Task_Instance      
   SET            
   description = description + ', Remarks - '+ RTRIM(@DeclineComment)      
   WHERE pmwrk_task_instance_cnt=(SELECT MAX(WTI.pmwrk_task_instance_cnt)    
   FROM PMWrk_Task_Instance WTI    
   INNER JOIN PMWrk_Task_Inst_Key WTIK ON wti.pmwrk_task_instance_cnt=WTIK.pmwrk_task_instance_cnt    
   INNER JOIN ManualJournal MJ ON mj.ManualJournal_id=WTIK.key_value    
   INNER JOIN PMNav_Key NK on nk.pmnav_key_id=WTIK.pmnav_key_id and NK.name='ManualJournalId'     
   WHERE  WTIK.key_value=CAST(@manualJournalId AS VARCHAR)) AND @DeclineComment <> ''  
END   
      
END 