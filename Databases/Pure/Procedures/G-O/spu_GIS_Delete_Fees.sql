SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GIS_Delete_Fees'
GO

CREATE PROCEDURE spu_GIS_Delete_Fees
    @gis_data_model_code    varchar(20),
    @gis_policy_link_id     int

AS

BEGIN

DECLARE @sql                  varchar(500),
        @gis_policy_binder_id int

-- Get the policy_binder_id
SELECT  @sql =
    'DECLARE @gis_policy_binder_id int ' +
    'SELECT @gis_policy_binder_id = ' + @gis_data_model_code + '_policy_binder_id ' +
    'FROM ' + @gis_data_model_code + '_Policy_Binder ' +
    'WHERE gis_policy_link_id = ' + CONVERT(varchar(20), @gis_policy_link_id) 

-- Now delete the fees
SELECT  @sql = @sql +
    'DELETE FROM ' + @gis_data_model_code + '_OUTPUT_FEES ' +
    'WHERE ' + @gis_data_model_code + '_policy_binder_id = @gis_policy_binder_id'

--Now execute the SQL
EXECUTE (@sql)

END
GO

