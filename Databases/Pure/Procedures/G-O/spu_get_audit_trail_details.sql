SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_audit_trail_details'
GO
CREATE PROCEDURE spu_get_audit_trail_details 
	@ModuleId   INT=0,
    @userID     INT=0,
	@startdate  DateTIME,
	@EndDate	DateTime

AS
BEGIN

SET @EndDate = DATEADD(SECOND, 86399, CAST(@EndDate AS DATETIME));

WITH CTE AS (
       SELECT    
            TableName, 
            key_field_name, 
            key_field_value,
            configuration_audit_master_id,
            fieldname,    
            COUNT(
                CASE ISNULL(oldvalue,'') 
                     WHEN '' THEN ISNULL(newvalue,'') 
                     ELSE ISNULL(OldValue,'') 
                END
            ) AS diff_count    
        FROM configuration_audit_details    
        GROUP BY TableName, key_field_name, key_field_value,configuration_audit_master_id,fieldname    
)
SELECT DISTINCT 
        cad.configuration_audit_detail_id, 
        cad.key_field_desc, 
        cad.FieldDisplayName,
        ISNULL(cad.OldValue,'') AS OldValue,
        ISNULL(cad.NewValue,'') AS NewValue,
        u.username,
        ca.UpdateDate
FROM configuration_audit_details cad
JOIN configuration_audit_master ca 
     ON ca.configuration_audit_master_id=cad.configuration_audit_master_id
JOIN (
    SELECT user_id, username
    FROM PMUser
    UNION ALL
    SELECT -1, 'PB2 User'
    WHERE NOT EXISTS (SELECT 1 FROM PMUser WHERE user_id = -1)
) u
     ON u.user_id=ca.UserId
JOIN CTE 
     ON cad.TableName = CTE.TableName
    AND cad.key_field_name = CTE.key_field_name
    AND cad.key_field_value = CTE.key_field_value
    AND cad.configuration_audit_master_id=CTE.configuration_audit_master_id
WHERE 
    (CTE.diff_count = 1 
     OR cad.TableName = 'Party_Relationship'
     OR cad.TableName = 'Credit_Control_Rule' 
     OR cad.TableName = 'Risk_Type_RI_Values'
	 OR cad.TableName = 'RI_Band_Version'
	 OR cad.TableName = 'tax_band_rate'
     OR cad.TableName = 'previous_accidents')
    AND (ca.UserId = @userID OR @userID = 0)
    AND (ca.Module_id = @ModuleId OR @ModuleId = 0)
    AND UpdateDate > @startdate 
    AND UpdateDate < @EndDate
    AND NOT (cad.type = 'D' 
             AND cad.TableName IN ('tax_band_rate','RI_Band_Version','Tax_group_tax_band','Peril_Type_Usage','index_linking_detail')
             AND cad.FieldName NOT IN ('Detail Deleted','Record Deleted'))
ORDER BY ca.UpdateDate DESC;
END;
GO


