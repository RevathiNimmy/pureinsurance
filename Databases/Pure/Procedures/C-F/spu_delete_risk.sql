SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_delete_risk'
GO

-- History: PW091002 - created
--          PW151102 - only delete the risk link record

CREATE PROCEDURE spu_delete_risk
    @insurance_file_cnt int,
    @risk_cnt int

AS
BEGIN

DECLARE @arrangement_id integer

    DELETE FROM insurance_file_risk_link
     WHERE insurance_file_cnt = @insurance_file_cnt
       AND risk_cnt = @risk_cnt

END
GO
