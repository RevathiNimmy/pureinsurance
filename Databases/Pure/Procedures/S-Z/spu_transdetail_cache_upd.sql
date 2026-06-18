SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_transdetail_cache_upd'
GO

CREATE PROCEDURE spu_transdetail_cache_upd
    @user_id int,
    @source_id int,
    @account_id int,
    @currency_id smallint,
    @amount numeric(19, 4),
    @comment varchar(255), 
    @base_amount numeric(19, 4),
    @exchange_rate numeric(19, 4),
    @euro_amount numeric(19, 4),
    @department_id int, 
    @insurance_ref varchar(255),
    @purchase_order_no varchar(255),
    @purchase_invoice_no varchar(255)
AS
BEGIN

    /*
        Currently the user_id isn't used.

        If it needs to be changed to be by user basis,
        then the only thing that needs to be changed are the SQL scripts.
        See spu_transdetail_cache_sel also

        When user_id is used, remove all of the code to find the SIRIUS user_id

    */

    DECLARE @sirius_user_id int

    SELECT @sirius_user_id = user_id FROM PMUser WHERE username = 'sirius'
    
    IF EXISTS (SELECT * FROM transdetail_cache WHERE source_id = @source_id)
    BEGIN
        DELETE FROM transdetail_cache
        WHERE source_id = @source_id
        AND user_id = @sirius_user_id
    END

    INSERT INTO transdetail_cache
    ( user_id, source_id, account_id, currency_id, amount, comment, 
      base_amount, exchange_rate, euro_amount, department_id, insurance_ref,
      purchase_order_no, purchase_invoice_no )
    VALUES
    ( @sirius_user_id, @source_id, @account_id, @currency_id, @amount, @comment, 
      @base_amount, @exchange_rate, @euro_amount, @department_id, @insurance_ref,
      @purchase_order_no, @purchase_invoice_no )

END
GO