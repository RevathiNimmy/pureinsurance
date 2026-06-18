SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_transdetail_cache_sel'
GO

CREATE PROCEDURE spu_transdetail_cache_sel
    @user_id int,
    @source_id int
AS
BEGIN

    /*
        Currently the user_id isn't used.

        If it needs to be changed to be by user basis,
        then the only thing that needs to be changed are the SQL scripts.
        See spu_transdetail_cache_upd also
    */

    SELECT    user_id,
              source_id,
              account_id,
              currency_id,
              amount,
              comment,
              base_amount,
              exchange_rate,
              euro_amount,
              department_id,
              insurance_ref,
              purchase_order_no,
              purchase_invoice_no  
    FROM      transdetail_cache
    WHERE     source_id = @source_id

END
GO