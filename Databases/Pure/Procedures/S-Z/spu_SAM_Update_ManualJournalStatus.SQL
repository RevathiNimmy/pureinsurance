GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Update_ManualJournalStatus'
GO

CREATE PROCEDURE spu_SAM_Update_ManualJournalStatus  
@manualJournalId INT, 
@is_reffered INT, 
@userId INT,  
@approved INT  
AS  
  
INSERT INTO ManualJournalApproval (  
                         manualjournal_id,  
                         user_id,  
                         approved,  
                         approval_date)  
VALUES         (  
                         @manualJournalId,  
                         @userId,  
                         @approved,  
                         getdate())  
  
UPDATE ManualJournal SET is_reffered=@is_reffered 
     WHERE manualjournal_id =@manualJournalId  

UPDATE PMWrk_Task_Instance  
		SET        
		task_status=3,  
		is_urgent=0,  
		last_modified=getdate(),  
		modified_by_id=@userId
		FROM PMWrk_Task_Instance WTI
		inner join PMWrk_Task_Inst_Key WTIK ON wti.pmwrk_task_instance_cnt=WTIK.pmwrk_task_instance_cnt
		inner join ManualJournal MJ ON mj.ManualJournal_id=WTIK.key_value
		inner join PMNav_Key NK on nk.pmnav_key_id=WTIK.pmnav_key_id and NK.name='ManualJournalId' 
		WHERE  WTIK.key_value=CAST(@manualJournalId AS varchar)
GO	 
	 