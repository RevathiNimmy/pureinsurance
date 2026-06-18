SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_SIR_Update_Policy_Posting_Period'
GO

CREATE PROCEDURE spu_SIR_Update_Policy_Posting_Period
	@insurance_file_cnt	integer,
	@posting_period_id	integer
AS

	UPDATE Insurance_File SET posting_period_id = @posting_period_id where insurance_file_cnt = @insurance_file_cnt

GO
