SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Coi_Compulsory_Value_del'
GO

CREATE PROCEDURE spe_Coi_Compulsory_Value_del
    @insurance_file_cnt int,
    @coi_compulsory_value_id int
AS
DELETE FROM Coi_Compulsory_Value
WHERE insurance_file_cnt = @insurance_file_cnt AND coi_compulsory_value_id = @coi_compulsory_value_id

GO

