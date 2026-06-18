EXECUTE DDLDropProcedure spu_SIRRen_Lapsed_Reason_sel
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_SIRRen_Lapsed_Reason_sel
     @insurance_file_cnt INT,
     @gis_data_model_id INT,
     @gis_scheme_id INT
AS

DECLARE @gis_data_model_code VARCHAR(10)
DECLARE @sql VARCHAR(255)
DECLARE @gis_policy_link_id INT

SELECT @gis_data_model_code = code FROM gis_data_model WHERE gis_data_model_id = @gis_data_model_id
SELECT @gis_data_model_code = RTRIM(@gis_data_model_code)

SELECT @gis_policy_link_id = gis_policy_link_id FROM gis_policy_link WHERE insurance_file_cnt = @insurance_file_cnt
SELECT @sql = 'SELECT r.lapsed_reason_id, r.code, r.description FROM ' + @gis_data_model_code + '_Output o, lapsed_reason r WHERE o.lapsed_reason_code = r.code AND o.scheme_id = ' + CONVERT(CHAR,@gis_scheme_id) + ' AND o.' + @gis_data_model_code + '_policy_binder_id = ' + CONVERT(CHAR,@gis_policy_link_id)
--SELECT @sql
EXEC (@sql)

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

