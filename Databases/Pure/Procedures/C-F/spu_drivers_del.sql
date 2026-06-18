SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_drivers_del'
GO


CREATE PROCEDURE spu_drivers_del
    @insurance_file_cnt int
AS


DELETE FROM drivers
WHERE insurance_file_cnt = @insurance_file_cnt
GO


