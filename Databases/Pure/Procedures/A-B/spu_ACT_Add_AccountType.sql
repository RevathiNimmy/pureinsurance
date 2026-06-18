SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_AccountType'
GO


CREATE PROCEDURE spu_ACT_Add_AccountType
    @accounttype_id smallint,
    @caption_id int,
    @is_deleted bit,
    @effective_date datetime,
    @description varchar(255),
    @code char(10),
    @fundamental_type smallint
AS


BEGIN
INSERT INTO AccountType (
    accounttype_id ,
    caption_id ,
    is_deleted ,
    effective_date ,
    description ,
    code ,
    fundamental_type )
VALUES (
    @accounttype_id,
    @caption_id,
    @is_deleted,
    @effective_date,
    @description,
    @code,
    @fundamental_type)
END
GO


