SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Arc_Coi_Value_del'
GO

CREATE PROCEDURE spe_Arc_Coi_Value_del
    @coi_value_id int,
    @insurance_file_cnt int
AS
DELETE FROM Arc_Coi_Value
WHERE coi_value_id = @coi_value_id AND insurance_file_cnt = @insurance_file_cnt

GO

