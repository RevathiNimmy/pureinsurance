EXECUTE DDLDropProcedure 'spu_Fee_amounts_u_add'
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE spu_Fee_amounts_u_add
    @fee_amount_key   int output,
    @party_cnt             int,
    @fee_amount_id    int,
    @product_group_id      int,
    @fee_percentage        numeric(7,4),
    @fee_amount            numeric(19,4),
    @transaction_type_id int,
    @effective_date datetime,
    @is_taxable	smallint,
    @currency_id int
AS


BEGIN

       INSERT INTO Fee_Amounts_u
        (
        party_cnt,
        product_group_id,
        fee_percentage,
        fee_amount,
        transaction_type_id,
        effective_date,
        is_taxable,
       currency_id
        )
    VALUES
        (
        @party_cnt,
        @product_group_id,
        @fee_percentage,
        @fee_amount,
        @transaction_type_id,
        @effective_date,
        @is_taxable,
        @currency_id
        )

SET @fee_amount_key =  @@IDENTITY

RETURN @fee_amount_key

END

GO


