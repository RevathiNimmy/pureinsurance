SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_TaxType'
GO


CREATE PROCEDURE spu_ACT_Update_TaxType
    @taxtype_id smallint,
    @code char(10),
    @caption_id int,
    @description varchar(255),
    @tax_basis tinyint,
    @is_deleted tinyint,
    @effective_date datetime
AS


BEGIN
UPDATE TaxType
    SET
    code=@code,
    caption_id=@caption_id,
    description=@description,
    tax_basis=@tax_basis,
    is_deleted=@is_deleted,
    effective_date=@effective_date
WHERE taxtype_id = @taxtype_id
END
GO


