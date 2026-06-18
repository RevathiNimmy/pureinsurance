SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PFTransaction_id_getpolicies'
GO

CREATE PROCEDURE spu_PFTransaction_id_getpolicies
(
    @pfprem_finance_cnt INT,
    @pfprem_finance_version INT
)
AS
BEGIN
    SELECT
        insurance_file_cnt
    FROM
        PFTransaction_id
    WHERE
        pfprem_finance_cnt=@pfprem_finance_cnt
    AND pfprem_finance_version=@pfprem_finance_version
    GROUP BY
        insurance_file_cnt
END

GO