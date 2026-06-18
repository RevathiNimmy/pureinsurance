EXECUTE DDLDropProcedure 'spu_SIR_Update_Risk_Type'
GO
CREATE PROCEDURE [dbo].[spu_SIR_Update_Risk_Type] 
@risk_cnt INT = 0,
@risk_type_ID INT = 0,
@risk_type_code VARCHAR(20) = '',
@insurance_file_cnt INT = 0

AS
  DECLARE @risk_id_code INT = 0
  IF @risk_type_ID = 0
  BEGIN
    SET @risk_id_code = (SELECT
      risk_type_id
    FROM risk_type
    WHERE code = @risk_type_code)
  END
  ELSE
  BEGIN
    SET @risk_id_code = @risk_type_ID
  END

  IF @risk_cnt <> 0
  BEGIN
    UPDATE risk
    SET risk_type_id = @risk_id_code
    WHERE risk_cnt = @risk_cnt
  END

  ELSE
  IF @insurance_file_cnt <> 0
  BEGIN
    DECLARE c_risk CURSOR FAST_FORWARD FOR
    SELECT
      r.risk_cnt
    FROM risk r
    INNER JOIN insurance_file_risk_link ifrl
      ON r.risk_cnt = ifrl.risk_cnt
    WHERE insurance_file_cnt = @insurance_file_cnt

    OPEN c_risk
    FETCH NEXT FROM c_risk INTO @risk_cnt
    WHILE @@FETCH_STATUS = 0
    BEGIN
      UPDATE risk
      SET risk_type_id = @risk_id_code
      WHERE risk_cnt = @risk_cnt

      FETCH NEXT FROM c_risk INTO @risk_cnt
    END
    CLOSE c_risk
    DEALLOCATE c_risk

  END
  GO