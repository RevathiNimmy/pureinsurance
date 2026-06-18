SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmb_trans_det_extra_ipt'
GO


CREATE PROCEDURE spu_pmb_trans_det_extra_ipt
    @risk_code int,
    @fee_party int,
    @effective_date datetime,
    @trans_fee_gross_amount numeric(19, 4),
    @trans_fee_ipt_amount numeric(19, 4) OUTPUT --MKW PN2865 Define as output
AS


BEGIN
DECLARE
    @return_status          int,
/* variables from the extras_ipt record*/
    @extra_ipt_rate         numeric(7,4),
    @extra_ipt_value        numeric(19,4),
/* ipt amount from ipt rate table*/
    @trans_ipt_amount       numeric(19,4)

DECLARE e_amounts CURSOR FAST_FORWARD FOR
    SELECT extra_ipt_rate =E.rate,
    extra_ipt_value = E.amount
    FROM    IPT_Extras      E
    WHERE   E.party_cnt = @fee_party
    AND E.effective_date <= @effective_date
    ORDER BY E.effective_date DESC

    /* Open the Amounts Cursor */
    OPEN e_amounts
    FETCH NEXT FROM e_amounts INTO
        @extra_ipt_rate,
        @extra_ipt_value

    /* Close and Deallocate Cursor */
    CLOSE e_amounts
    DEALLOCATE e_amounts

/* Calculate the Extra IPT */
    SELECT @trans_fee_ipt_amount = (@trans_fee_gross_amount * @extra_ipt_rate /100) + @extra_ipt_value

    IF @trans_fee_ipt_amount IS NULL BEGIN
        EXECUTE @return_status = spu_pmb_trans_det_ipt
            @risk_code,
            @effective_date,
            @trans_fee_gross_amount,
            @trans_ipt_amount OUTPUT

        If @trans_ipt_amount IS NOT NULL
            SELECT @trans_fee_ipt_amount = @trans_ipt_amount
    END

RETURN
END
GO


