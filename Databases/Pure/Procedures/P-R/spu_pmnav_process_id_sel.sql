SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmnav_process_id_sel'
GO


CREATE PROCEDURE spu_pmnav_process_id_sel
    @process_code varchar(10)
AS


SELECT pnp.pmnav_process_id
    FROM PMNav_Process pnp
    WHERE pnp.code = @process_code
GO


