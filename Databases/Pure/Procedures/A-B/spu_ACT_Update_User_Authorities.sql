SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_User_Authorities'
GO


CREATE PROCEDURE spu_ACT_Update_User_Authorities
    @user_id smallint,
    @has_write_off_authority tinyint,
    @write_off_amount numeric(19,4)
AS


BEGIN
UPDATE User_Authorities
    SET
    has_write_off_authority=@has_write_off_authority,
    write_off_amount=@write_off_amount
WHERE user_id = @user_id
END
GO


