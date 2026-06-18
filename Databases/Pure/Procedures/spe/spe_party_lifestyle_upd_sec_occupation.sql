SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_lifestyle_upd_sec_occupation'
GO

CREATE PROCEDURE spe_party_lifestyle_upd_sec_occupation
    @party_cnt INT,
    @secondary_occupation_code VARCHAR(70)
AS
BEGIN
UPDATE party_lifestyle
    SET secondary_occupation_code = @secondary_occupation_code
WHERE party_cnt = @party_cnt
  AND party_lifestyle_id = 1
END

GO
