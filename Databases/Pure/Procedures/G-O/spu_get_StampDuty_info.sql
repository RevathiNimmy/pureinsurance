SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_StampDuty_info'
GO

CREATE PROCEDURE spu_get_StampDuty_info
    @insurance_file_cnt INT
AS
BEGIN
    -- Declare a variable to store the result of the sum
    DECLARE @total_premium DECIMAL(18, 2);

    -- Calculate the sum of premiums
    SELECT @total_premium = ISNULL(SUM(this_premium), 0)
    FROM peril AS p
    LEFT JOIN insurance_file_risk_link AS IFRL ON IFRL.risk_cnt = p.risk_cnt
    LEFT JOIN Peril_Type AS PT ON PT.peril_type_id = p.peril_type_id
    WHERE IFRL.insurance_file_cnt = @insurance_file_cnt
      AND PT.add_stamp_duty_in_first_instalment = 1;

    -- Return the result
    SELECT @total_premium AS this_premium;
END;
GO