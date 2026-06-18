SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_vehicles_del'
GO


CREATE PROCEDURE spu_vehicles_del
    @insurance_file_cnt int
AS


DELETE FROM vehicles
WHERE insurance_file_cnt = @insurance_file_cnt
GO


