SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_renumber_risks'
GO

-- Created: PW041002
-- PW061202 - risk variations must now start from 1 not 0

CREATE PROCEDURE spu_renumber_risks
    @insurance_file_cnt integer
AS

BEGIN

    DECLARE @risk_cnt integer,
            @risk_number integer,
            @include_deleted tinyint
            
Select @include_deleted = 0

If Exists(Select Null From mta_insurance_file_link Where new_linked_insurance_file_cnt = @insurance_file_cnt)   
	Select @include_deleted = 1

    DECLARE c_risk CURSOR FAST_FORWARD FOR
        SELECT r.risk_cnt
          FROM risk r
    INNER JOIN insurance_file_risk_link ifrl
            ON r.risk_cnt = ifrl.risk_cnt
         WHERE insurance_file_cnt = @insurance_file_cnt AND (ifrl.status_flag <>'D' OR @include_deleted = 1)
      ORDER BY r.risk_number

    SELECT @risk_number = 1
    OPEN c_risk
    FETCH NEXT FROM c_risk INTO @risk_cnt
    WHILE @@FETCH_STATUS = 0
    BEGIN

        UPDATE risk
           SET risk_number = @risk_number,
               variation_number = 1
         WHERE risk_cnt = @risk_cnt

        SELECT @risk_number = (@risk_number + 1)

        FETCH NEXT FROM c_risk INTO @risk_cnt
    END
    CLOSE c_risk
    DEALLOCATE c_risk

END
GO
