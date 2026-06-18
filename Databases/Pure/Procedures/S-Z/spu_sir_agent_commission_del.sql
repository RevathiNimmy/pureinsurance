EXECUTE DDLDropProcedure 'spu_sir_agent_commission_del'
GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_sir_agent_commission_del
    @insurance_file_cnt int
AS


BEGIN
	-- Remove the Tax breakdown first
	DELETE	Tax_Calculation
	WHERE	insurance_file_cnt = @insurance_file_cnt 
	AND		transtype='TTAC'

    DELETE  Agent_Commission
    WHERE   insurance_file_cnt = @insurance_file_cnt

END
GO




