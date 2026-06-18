SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_Report_Diary_SFU  
GO

/* eck030702 Problems when evaluating sub-select as tried to convert TK.key-value to int before
making other selection solution to do varchar check on insurance_file_cnt */  
CREATE PROCEDURE spu_Report_Diary_SFU  
AS  
BEGIN
  
    SELECT 
        ISNULL( U1.username, '') operator,  
        TI.task_due_date action_date,  
        ISNULL(TI.customer, '') client,  
        ISNULL(  
        (  
            SELECT 
                I.insurance_ref  
            FROM 
                Insurance_File I
                INNER JOIN PMWrk_Task_Inst_Key TK
                    ON CONVERT(VARCHAR, I.insurance_file_cnt) = TK.key_value
                INNER JOIN PMNav_Key K
                    ON K.pmnav_key_id = TK.pmnav_key_id
                    AND K.name = 'insurance_file_cnt'        
            WHERE 
                TK.pmwrk_task_instance_cnt = TI.pmwrk_task_instance_cnt  
                /**eck030702***/  
                -- AND I.insurance_file_cnt = convert(int, TK.key_value)  
        )  
        , '') policy_no,  
        ISNULL(TI.description, '') description,  
        ISNULL(U2.username, '') logged_by  
    FROM 
        PMWrk_Task_Instance TI  
        INNER JOIN pmuser U2
            ON U2.user_id = TI.created_by_id 
            AND TI.task_status <> 3 -- Ram 17-01-2001 to filter All Completed tasks    
        LEFT OUTER JOIN pmuser U1
            ON U1.user_id = TI.user_id    
    ORDER BY  
        operator,  
        task_due_date  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
