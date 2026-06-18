SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_pmb_event_log_id_sel'
GO

CREATE PROCEDURE spu_pmb_event_log_id_sel
	@user_id int
AS

SELECT MAX(event_cnt) FROM event_log WHERE user_id=@user_id AND event_type_id=13

GO

