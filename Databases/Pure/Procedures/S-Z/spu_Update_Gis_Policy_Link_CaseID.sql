SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_Update_Gis_Policy_Link_CaseID'
GO

CREATE PROCEDURE spu_Update_Gis_Policy_Link_CaseID  
    @case_id INT,
    @screen_id INT
AS

DECLARE @gis_policy_link_id INT
DECLARE @gis_data_model_code VARCHAR(10)
DECLARE @sSQL VARCHAR(8000)
DECLARE @sSQL1 VARCHAR(8000)

SELECT @gis_data_model_code = rtrim(GD.code) 
FROM gis_data_model GD
JOIN gis_screen GS
ON GS.gis_data_model_id = GD.gis_data_model_id
WHERE gis_screen_id = @screen_id

SET @sSQL = 'DECLARE @gis_policy_link_id INT ' + Char(13)
SET @sSQL = @sSQL + 'SELECT @gis_policy_link_id = PB.gis_policy_link_id FROM ' + @gis_data_model_code + '_GENERAL G ' 
SET @sSQL = @sSQL + 'JOIN ' + @gis_data_model_code + '_Policy_binder PB ' + 'ON PB.' + @gis_data_model_code + '_Policy_binder_id'
SET @sSQL = @sSQL + ' = G.' + @gis_data_model_code + '_Policy_binder_id' + char(13)

SET @sSQL1 = 'IF @gis_policy_link_id IS NOT NULL ' + char(13)
SET @sSQL1 = @sSQL1 + 'UPDATE gis_policy_link '
SET @sSQL1 = @sSQL1 + 'SET case_id = ' + cast(@case_id as char(10))
SET @sSQL1 = @sSQL1 + 'WHERE  '
SET @sSQL1 = @sSQL1 + 'gis_policy_link_id = @gis_policy_link_id '
SET @sSQL1 = @sSQL1 + 'AND case_id IS NULL'

EXEC(@sSQL + @sSQL1)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


