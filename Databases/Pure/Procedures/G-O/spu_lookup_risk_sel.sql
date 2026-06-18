SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_lookup_risk_sel'
GO


CREATE PROCEDURE spu_lookup_risk_sel
    @insurance_file_cnt int,
    @risk_id int
AS


SELECT  risk_type_id

FROM    risk

WHERE   risk_cnt = @risk_id
--AND   insurance_file_cnt = @insurance_file_cnt
GO


