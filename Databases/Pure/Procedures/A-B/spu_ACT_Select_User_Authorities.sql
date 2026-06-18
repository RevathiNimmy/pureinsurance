SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_User_Authorities'
GO


CREATE PROCEDURE spu_ACT_Select_User_Authorities
    @user_id smallint
AS


SELECT
    user_id,
    has_write_off_authority,
    write_off_amount
FROM User_Authorities
WHERE user_id = @user_id
GO


