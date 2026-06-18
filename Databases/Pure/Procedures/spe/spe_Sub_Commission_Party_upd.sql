SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Sub_Commission_Party_upd'
GO

CREATE PROCEDURE spe_Sub_Commission_Party_upd
    @party_cnt int,
    @insurance_file_cnt int,
    @sequence_number int,
    @commission_percent numeric(12,8)
AS
BEGIN
UPDATE Sub_Commission_Party
    SET
    sequence_number=@sequence_number,
    commission_percent=@commission_percent
WHERE party_cnt = @party_cnt AND insurance_file_cnt = @insurance_file_cnt
END

GO

