SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_copy_comb_liab_to_event'
GO


CREATE PROCEDURE spu_copy_comb_liab_to_event
    @event_cnt int,
    @insurance_file_cnt int
AS
--REDUNDANT PROCEDURE IN CURRENT VERSION
GO


