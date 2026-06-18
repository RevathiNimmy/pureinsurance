SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Sub_Commission_Party_del'
GO

CREATE PROCEDURE spe_Sub_Commission_Party_del
    @party_cnt int,
    @insurance_file_cnt int
AS
DELETE FROM Sub_Commission_Party
WHERE party_cnt = @party_cnt AND insurance_file_cnt = @insurance_file_cnt

GO

