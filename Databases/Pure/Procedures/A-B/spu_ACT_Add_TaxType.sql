SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_TaxType'
GO


CREATE PROCEDURE spu_ACT_Add_TaxType
    @taxtype_id smallint,
    @code char(10),
    @caption_id int,
    @description varchar(255),
    @tax_basis tinyint,
    @is_deleted tinyint,
    @effective_date datetime
AS


BEGIN
INSERT INTO TaxType (
    taxtype_id ,
    code ,
    caption_id ,
    description ,
    tax_basis ,
    is_deleted ,
    effective_date )
VALUES (
    @taxtype_id,
    @code,
    @caption_id,
    @description,
    @tax_basis,
    @is_deleted,
    @effective_date)
END
GO


