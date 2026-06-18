SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_PaymentType'
GO


CREATE PROCEDURE spu_ACT_Update_PaymentType
    @paymenttype_id smallint,
    @caption_id int,
    @is_deleted bit,
    @effective_date datetime,
    @description varchar(255),
    @code char(10)
AS


BEGIN
UPDATE PaymentType
    SET
    caption_id=@caption_id,
    is_deleted=@is_deleted,
    effective_date=@effective_date,
    description=@description,
    code=@code
WHERE paymenttype_id = @paymenttype_id
END
GO


