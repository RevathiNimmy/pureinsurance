SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Sub_Commission_Party_sel'
GO

CREATE PROCEDURE spe_Sub_Commission_Party_sel
    @party_cnt int,
    @insurance_file_cnt int
AS
SELECT
    party_cnt,
    insurance_file_cnt,
    sequence_number,
    commission_percent
 FROM Sub_Commission_Party
WHERE party_cnt = @party_cnt AND insurance_file_cnt = @insurance_file_cnt

GO

