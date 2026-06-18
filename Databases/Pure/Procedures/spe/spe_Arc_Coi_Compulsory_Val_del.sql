SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Arc_Coi_Compulsory_Val_del'
GO

CREATE PROCEDURE spe_Arc_Coi_Compulsory_Val_del
    @coi_compulsory_value_id int,
    @insurance_file_cnt int
AS
DELETE FROM Arc_Coi_Compulsory_Value
WHERE coi_compulsory_value_id = @coi_compulsory_value_id AND insurance_file_cnt = @insurance_file_cnt

GO

