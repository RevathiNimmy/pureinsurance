SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmb_trans_det_ipt'
GO


CREATE PROCEDURE spu_pmb_trans_det_ipt
    @risk_code int,
    @effective_date datetime,
    @trans_fee_gross_amount numeric(19, 4),
    @trans_ipt_amount numeric(19, 4) OUTPUT --MKW PN2865 Define as output
AS


BEGIN
DECLARE
    @return_status          int,
/* variables from the extras_ipt record*/
    @ipt_rate           numeric(7,4)

DECLARE i_amounts CURSOR FAST_FORWARD FOR
    SELECT ipt_rate =I.rate
    FROM    IPT     I
    WHERE   I.risk_code_id = @risk_code
    AND I.effective_date <= @effective_date
    ORDER BY I.effective_date DESC

/* Open the Amounts Cursor */
OPEN i_amounts
FETCH NEXT FROM i_amounts INTO    @ipt_rate

/* Close and Deallocate Cursor */
CLOSE i_amounts
DEALLOCATE i_amounts

/* Calculate the  IPT */
SELECT @trans_ipt_amount = (@trans_fee_gross_amount * @ipt_rate / 100)

RETURN
END
GO


