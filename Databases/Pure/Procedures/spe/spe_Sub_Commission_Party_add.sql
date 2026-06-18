SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Sub_Commission_Party_add'
GO

CREATE PROCEDURE spe_Sub_Commission_Party_add
    @party_cnt int,
    @insurance_file_cnt int,
    @sequence_number int,
    @commission_percent numeric(12,8)
AS
BEGIN
INSERT INTO Sub_Commission_Party (
    party_cnt ,
    insurance_file_cnt ,
    sequence_number ,
    commission_percent )
VALUES (
    @party_cnt,
    @insurance_file_cnt,
    @sequence_number,
    @commission_percent)
END

GO

