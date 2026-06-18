SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_risk_claim_del'
GO


CREATE PROCEDURE spu_risk_claim_del
    @insurance_file_cnt int
AS


DELETE FROM risk_claim
WHERE insurance_file_cnt = @insurance_file_cnt
GO


