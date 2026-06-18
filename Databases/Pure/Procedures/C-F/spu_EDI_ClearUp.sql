SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_EDI_ClearUp'
GO

CREATE PROCEDURE spu_EDI_ClearUp
                    @gis_policy_link_id int,
                    @insurance_file_cnt int
AS
BEGIN

DECLARE @insurance_folder_cnt int
DECLARE @risk_folder_cnt int
DECLARE @risk_cnt int

-- Get values
SELECT  @insurance_folder_cnt = insurance_folder_cnt
FROM    insurance_file
WHERE   insurance_file_cnt = @insurance_file_cnt

SELECT  @risk_cnt = risk_cnt
FROM    insurance_file_risk_link
WHERE   insurance_file_cnt = @insurance_file_cnt

SELECT  @risk_folder_cnt = risk_folder_cnt
FROM    risk r
WHERE   risk_cnt = @risk_cnt

-- Delete the Risk Objects
EXECUTE spu_SIRRen_DeleteGISObject @gis_policy_link_id

PRINT 'Insurance File Risk Link'
DELETE FROM insurance_file_risk_link WHERE insurance_file_cnt = @insurance_file_cnt

PRINT 'Risk'
DELETE FROM risk WHERE risk_cnt = @risk_cnt

PRINT 'Risk Folder'
IF (SELECT count(*) FROM risk WHERE risk_folder_cnt = @risk_folder_cnt) = 0
    DELETE FROM risk_folder WHERE risk_folder_cnt = @risk_folder_cnt

PRINT 'Event Log'
DELETE FROM event_log WHERE insurance_file_cnt = @insurance_file_cnt

PRINT 'Policy Agents'
DELETE FROM policy_agents WHERE insurance_file_cnt = @insurance_file_cnt

PRINT 'Insurance File System'
DELETE FROM insurance_file_system WHERE insurance_file_cnt = @insurance_file_cnt

PRINT 'Insurance File'
DELETE FROM insurance_file WHERE insurance_file_cnt = @insurance_file_cnt

PRINT 'Insurance_Folder'
IF (SELECT count(*) FROM insurance_file WHERE insurance_folder_cnt = @insurance_folder_cnt) = 0
    DELETE FROM insurance_folder WHERE insurance_folder_cnt = @insurance_folder_cnt
END
GO