SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_lifestyle_sel'
GO

CREATE PROCEDURE spe_party_lifestyle_sel
    @party_cnt int,
    @party_lifestyle_id int
AS
SELECT
    party_cnt,
    party_lifestyle_id,
    name,
    category,
    date_of_birth,
    gender_code,
    occupation_code,
    secondary_occupation_code,
    is_smoker
 FROM party_lifestyle
WHERE party_cnt = @party_cnt AND party_lifestyle_id = @party_lifestyle_id

GO

