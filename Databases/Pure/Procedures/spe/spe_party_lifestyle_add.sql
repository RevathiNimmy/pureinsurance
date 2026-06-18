SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_lifestyle_add'
GO

CREATE PROCEDURE spe_party_lifestyle_add
    @party_cnt int ,
    @party_lifestyle_id int OUTPUT ,
    @name varchar(255) ,
    @category int ,
    @date_of_birth datetime ,
    @gender_code varchar(70) ,
    @occupation_code varchar(70) ,
    @secondary_occupation_code varchar(70) ,
    @is_smoker tinyint
AS
BEGIN
IF @party_lifestyle_id = 0
                SELECT @party_lifestyle_id = NULL
IF @party_lifestyle_id IS NULL
                SELECT @party_lifestyle_id = MAX(party_lifestyle_id) + 1
    FROM party_lifestyle
                WHERE party_cnt = @party_cnt
IF @party_lifestyle_id IS NULL
    SELECT @party_lifestyle_id = 1
INSERT INTO party_lifestyle (
    party_cnt ,
    party_lifestyle_id ,
    name ,
    category ,
    date_of_birth ,
    gender_code ,
    occupation_code ,
    secondary_occupation_code ,
    is_smoker )
VALUES (
    @party_cnt,
    @party_lifestyle_id,
    @name,
    @category,
    @date_of_birth,
    @gender_code,
    @occupation_code,
    @secondary_occupation_code,
    @is_smoker)
END

GO

