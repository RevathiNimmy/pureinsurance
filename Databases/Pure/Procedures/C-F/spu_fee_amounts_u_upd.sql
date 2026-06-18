EXECUTE DDLDropProcedure 'spu_fee_amounts_u_upd'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE spu_fee_amounts_u_upd

    @fee_amount_id            int,
    @product_group_id         int,
    @fee_percentage        numeric(7,4),
    @fee_amount            numeric(19,4),
    @transaction_type_id int,
    @effective_date datetime,
    @is_taxable	smallint,
    @is_ammended	int,
    @currency_id int
AS


BEGIN

    UPDATE
        Fee_amounts_u
    SET
        product_group_id=@product_group_id,
        fee_percentage=@fee_percentage,
        fee_amount=@fee_amount,
        transaction_type_id=@transaction_type_id,
        effective_date=@effective_date,
        is_taxable=@is_taxable,
        is_ammended=@is_ammended,
        currency_id=@currency_id
    WHERE
        fee_amount_id = @fee_amount_id  
  
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

