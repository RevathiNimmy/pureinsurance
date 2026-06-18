SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_lifestyle_saa'
GO

CREATE PROCEDURE spe_party_lifestyle_saa
    @party_cnt int
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
    is_smoker,
	CASE 
	WHEN category = 1 THEN "Insured"
	WHEN category = 2 THEN "Spouse"
	WHEN category = 3 THEN "1st Child"
	WHEN category = 4 THEN "2nd Child"
	WHEN category = 5 THEN "3rd Child"
	WHEN category = 6 THEN "4th Child"
	WHEN category = 7 THEN "5th Child"
	WHEN category = 8 THEN "6th Child"
	WHEN category = 9 THEN "Other Child"
	WHEN category = 10 THEN "Partner"
	WHEN category = 11 THEN "Undefined Relationship"
	END As 'Category Description'
FROM party_lifestyle 
WHERE party_cnt = @party_cnt
ORDER BY party_lifestyle_id ASC

GO

