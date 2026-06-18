SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PFTransaction_id_ins'
GO

CREATE PROCEDURE spu_PFTransaction_id_ins
(
    @pfprem_finance_cnt INT,
    @pfprem_finance_version INT,
    @pftransaction_id INT,
    @insurance_file_cnt INT
)
AS
BEGIN
    INSERT INTO PFTransaction_id
        (pfprem_finance_cnt,pfprem_finance_version, pftransaction_id, insurance_file_cnt)
    VALUES
        (@pfprem_finance_cnt, @pfprem_finance_version, @pftransaction_id, @insurance_file_cnt)
END

GO