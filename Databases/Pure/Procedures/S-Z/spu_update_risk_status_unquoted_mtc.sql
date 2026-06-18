SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_update_risk_status_unquoted_mtc'
GO

-- Created: PW211002

CREATE PROCEDURE spu_update_risk_status_unquoted_mtc
    @insurance_file_cnt integer
AS

BEGIN

    DECLARE @risk_status integer,
            @risk_cnt integer

    SELECT @risk_status = risk_status_id
      FROM risk_status
     WHERE code = 'UNQUOTED'

    DECLARE c_risk CURSOR FAST_FORWARD FOR
        SELECT r.risk_cnt
          FROM risk r
    INNER JOIN insurance_file_risk_link ifrl
            ON r.risk_cnt = ifrl.risk_cnt
         WHERE insurance_file_cnt = @insurance_file_cnt
	 AND ISNULL(ifrl.is_risk_edited, 0) = 0
    OPEN c_risk
    FETCH NEXT FROM c_risk INTO @risk_cnt
    WHILE @@FETCH_STATUS = 0
    BEGIN

        UPDATE risk
           SET risk_status_id = @risk_status
         WHERE risk_cnt = @risk_cnt

        FETCH NEXT FROM c_risk INTO @risk_cnt
    END
    CLOSE c_risk
    DEALLOCATE c_risk

END
GO