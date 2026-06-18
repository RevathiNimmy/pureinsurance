SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_check_agent_cancelled'
GO

CREATE PROCEDURE spu_check_agent_cancelled
	@agent_id int
AS
BEGIN
	
	declare @date_cancelled datetime
	
	SELECT	@date_cancelled = date_cancelled 
	FROM	party_agent
	WHERE	party_cnt = @agent_id

	IF(@date_cancelled <> '1899-12-29' AND getdate() >= @date_cancelled)
		-- Cancelled
		SELECT 1
	ELSE
		SELECT 0

END

