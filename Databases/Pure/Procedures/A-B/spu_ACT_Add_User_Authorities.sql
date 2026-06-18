SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_User_Authorities'
GO


CREATE PROCEDURE spu_ACT_Add_User_Authorities
    @user_id smallint,
    @has_write_off_authority tinyint,
    @write_off_amount numeric(19,4)
AS


BEGIN
INSERT INTO User_Authorities (
    user_id ,
    has_write_off_authority ,
    write_off_amount )
VALUES (
    @user_id,
    @has_write_off_authority,
    @write_off_amount)
END
GO


