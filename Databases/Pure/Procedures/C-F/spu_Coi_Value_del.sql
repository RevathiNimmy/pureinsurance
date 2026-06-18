SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Coi_Value_del'
GO


CREATE PROCEDURE spu_Coi_Value_del
    @insurance_file_cnt int
AS


DELETE FROM Coi_Value

WHERE insurance_file_cnt = @insurance_file_cnt
GO


