SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Coi_Value_del'
GO

CREATE PROCEDURE spe_Coi_Value_del
    @insurance_file_cnt int,
    @coi_value_id int
AS
-- Tracy Richards - 26/08/2003 
-- If the @coi_value_id is null then delete all for this insurance_file_cnt
IF (@coi_value_id IS NULL)
    DELETE FROM Coi_Value
       WHERE insurance_file_cnt = @insurance_file_cnt
ELSE
    DELETE FROM Coi_Value
       WHERE insurance_file_cnt = @insurance_file_cnt AND coi_value_id = @coi_value_id
GO

