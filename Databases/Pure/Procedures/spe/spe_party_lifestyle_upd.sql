SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_lifestyle_upd'
GO

CREATE PROCEDURE spe_party_lifestyle_upd
    @party_cnt int,
    @party_lifestyle_id int,
    @name varchar(255),
    @category int,
    @date_of_birth datetime,
    @gender_code varchar(70),
    @occupation_code varchar(70),
    @secondary_occupation_code varchar(70),
    @is_smoker tinyint
AS
BEGIN
UPDATE party_lifestyle
    SET
    name=@name,
    category=@category,
    date_of_birth=@date_of_birth,
    gender_code=@gender_code,
    occupation_code=@occupation_code,
    secondary_occupation_code=@secondary_occupation_code,
    is_smoker=@is_smoker
WHERE party_cnt = @party_cnt AND party_lifestyle_id = @party_lifestyle_id
END

GO

