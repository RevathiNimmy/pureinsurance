SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_get_Wrk_task_on_keys'
GO

CREATE PROCEDURE spu_SAM_get_Wrk_task_on_keys
       @InputXML varchar(5000), 
	   @AgentKey INT=0
AS  
BEGIN  
  
DECLARE @Input XML = @InputXML;  
DECLARE @PMWRK_Task_Instance_cnt As Integer = 0;  
DECLARE @SQL VARCHAR(max) = '';  
DECLARE @SQLWHERE VARCHAR(max) = '';  
  
DECLARE @QT VARCHAR(1)  
DECLARE @CR VARCHAR(2)  
  
SET @QT=CHAR(39)  
SET @CR=CHAR(13) + CHAR(10)  
  
CREATE TABLE #KeyNameValueCTE (  
 KeyName VARCHAR(255),  
 KeyValue VARCHAR(255)  
 )  
  
INSERT INTO #KeyNameValueCTE  
   SELECT  
              KeyName = nodes.value('local-name(.)', 'varchar(30)') ,  
              KeyValue = nodes.value('(.)[1]', 'varchar(50)')  
       FROM  
              @input.nodes('/start/*') AS Tbl(nodes)  
  
DECLARE @KeyValue VARCHAR(255);  
DECLARE KeyNameValueCTE_Cursor CURSOR  
 FOR  
  SELECT KeyValue FROM #KeyNameValueCTE;  
  
   OPEN KeyNameValueCTE_Cursor  
   FETCH NEXT FROM KeyNameValueCTE_Cursor INTO @KeyValue  
 WHILE @@FETCH_STATUS = 0  
 BEGIN  
  SET @SQLWHERE = @SQLWHERE + ' select PMWRK_Task_Instance_cnt from PMWrk_Task_Inst_Key where UPPER(RTRIM(LTRIM(key_value))) = UPPER(RTRIM(LTRIM('''  
      + @KeyValue +  
      '''))) INTERSECT'  
  
 FETCH NEXT FROM KeyNameValueCTE_Cursor INTO @KeyValue  
 END  
 CLOSE KeyNameValueCTE_Cursor  
 DEALLOCATE KeyNameValueCTE_Cursor  
  
SET @SQLWHERE = left(@SQLWHERE, len(@SQLWHERE) - 9);  
  
SET @SQL= 'SELECT  
              Distinct  
        ti.pmwrk_task_instance_cnt,  
        ti.is_urgent,  
        ti.task_status,  
  
              ( CASE t.type_of_task  
                     WHEN 0 THEN ' + @QT + 'Memo' + @QT +  
                     'WHEN 1 THEN ' + @QT + 'Non-Navigator Function' + @QT +  
                     'WHEN 2 THEN ' + @QT + 'Navigator Process' + @QT +  
                     'END)As Type_of_Task,  
        t.is_system_task,  
        ti.task_due_date,  
        ti.customer,  
        ti.description,  
        ti.pmuser_group_id,  
        ti.user_id,  
        t.pmnav_process_id,  
        t.component_object_name,  
        t.component_class_name,  
        t.display_icon,  
        t.is_view_only_task,  
        t.linked_object_name,  
        t.linked_class_name,  
        t.linked_caption_id,  
        ti.is_visible,  
        (     SELECT file_name  
            FROM pmnavxm_process  
            WHERE pmnavxm_process_id = t.pmnavxm_process_id  
        ) ''nav_xml_file'',  
  
        (     SELECT  
            c.shortname  
            FROM pmwrk_task_inst_key v  
            JOIN party c ON c.party_cnt = v.key_value  
            WHERE v.pmwrk_task_instance_cnt = ti.pmwrk_task_instance_cnt  
            AND v.pmnav_key_id = 2  
        ) ''shortname'',  
        ti.party_cnt PartyCnt,  
              p.shortname PartyName,  
        pug.Code As UserGroupCode,  
        pug.description as UserGroupDescription,  
              pu.username as UserDescription,  
              ti.pmwrk_task_group_id ' + @QT + 'TaskGroupKey' + @QT + ',  
              ti.pmwrk_task_id ' + @QT + 'TaskKey' + @QT + ',  
              ti.pmwrk_task_id ' + @QT + 'TaskKey' + @QT + ',  
              ti.PMWrk_task_parent_instance_cnt ,  
              Cast(ti.external_Workflow_id As varchar(Max)) As external_Workflow_id,  
              ti.Is_External_WorkItem  
              FROM pmwrk_task_instance ti  
              JOIN pmwrk_task t ON t.pmwrk_task_id = ti.pmwrk_task_id  
              LEFT JOIN pmuser_group pug ON pug.pmuser_group_id = ti.pmuser_group_id  
              LEFT JOIN pmuser pu ON pu.user_id = ti.user_id  
              LEFT JOIN party p ON (p.party_cnt = ti.party_cnt)  
              Left JOIN PMWrk_Task_Inst_Key tik on  tik.pmwrk_task_instance_cnt = ti.pmwrk_task_instance_cnt  
              Left JOIN PMNav_Key nk on nk.pmnav_key_id = tik.pmnav_key_id  
              where   
				(p.agent_cnt= ' +  CONVERT(VARCHAR(20),@AgentKey) + ' OR '+  CONVERT(VARCHAR(20),@AgentKey) + '=0 ' + ' OR p.party_cnt = ' + CONVERT(VARCHAR(20),@AgentKey) + '  )  
				AND ti.PMWrk_task_instance_cnt in    
				(' + @CR +  
					@SQLWHERE + @CR +  
				')  
     ORDER BY ti.task_due_date ASC'  
   EXECUTE(@SQL)  
  
  DROP TABLE #KeyNameValueCTE  
  
END  
GO



