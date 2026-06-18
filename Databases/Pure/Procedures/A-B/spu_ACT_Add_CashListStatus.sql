SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_CashListStatus'
GO


CREATE PROCEDURE spu_ACT_Add_CashListStatus
    @cashliststatus_id int,
    @caption_id int,
    @is_deleted bit,
    @effective_date datetime,
    @description varchar(255),
    @code char(10)
AS


BEGIN
INSERT INTO CashListStatus (
    cashliststatus_id ,
    caption_id ,
    is_deleted ,
    effective_date ,
    description ,
    code )
VALUES (
    @cashliststatus_id,
    @caption_id,
    @is_deleted,
    @effective_date,
    @description,
    @code)
END
GO


