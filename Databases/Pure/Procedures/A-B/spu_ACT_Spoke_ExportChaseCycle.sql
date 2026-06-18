SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Spoke_ExportChaseCycle'
GO


CREATE PROCEDURE spu_ACT_Spoke_ExportChaseCycle        
    @branch_code CHAR(10)        
AS        
        
BEGIN        
        
    SELECT cci.Chase_Cycle_item_id,        
    cci.Chase_Cycle_reason,        
    cci.insurance_folder_cnt,        
    cci.insurance_file_cnt,        
    cci.can_auto_cancel,        
    cci.will_auto_cancel,        
    cci.Chase_Cycle_step_id,        
    CONVERT(VARCHAR(10),cci.created_date,126),        
    CONVERT(VARCHAR(10),cci.due_date,126),        
    cci.letter_sent,        
    ccs.auto_cancel_policy,        
    ccs.check_auto_cancel,        
    ccs.pmuser_group_id,        
    ccs.pmwrk_task_id,        
    ccs.number_of_days,        
    ccs2.Chase_Cycle_step_id,        
    risk_count = (SELECT COUNT(*)        
        FROM Insurance_File_Risk_Link ifrl        
              WHERE ifrl.insurance_file_cnt = cci.insurance_file_cnt        
                  AND ifrl.status_flag <> 'D'),        
    cci.pmuser_group_id,        
    cci.pmuser_id,        
    cci.is_deleted,        
    '' data_changed,        
    dt.code,        
    ccs.document_template_id,        
    ccs.pmwrk_task_group_id,        
    ccs.step_description,    
    p.shortname      
        
FROM Chase_Cycle_Item cci        
        
INNER JOIN Chase_Cycle_Step ccs ON cci.Chase_Cycle_step_id = ccs.Chase_Cycle_step_id        
INNER JOIN Chase_Cycle_Rule ccr ON ccs.Chase_Cycle_rule_id = ccr.Chase_Cycle_rule_id        
INNER JOIN Source s ON ccr.source_id = s.source_id        
LEFT JOIN Insurance_File iff ON cci.insurance_file_cnt = iff.insurance_file_cnt        
LEFT JOIN Party p ON iff.insured_cnt = p.party_cnt        
 LEFT JOIN Chase_Cycle_Step ccs2 ON ccs.next_step = ccs2.step_number        
    AND ccs.Chase_Cycle_rule_id = ccs2.Chase_Cycle_rule_id        
 LEFT JOIN Document_Template dt ON        
     ccs.document_template_id = dt.document_template_id        
        
 WHERE s.code = @branch_code AND ISNULL(cci.is_deleted,0) <> 1        
        
 ORDER BY        
    iff.Insurance_folder_cnt,        
    iff.Insurance_file_cnt,        
    cci.created_date,        
    cci.will_auto_cancel desc,        
    cci.can_auto_cancel desc,        
    cci.due_date        
        
END   

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
