SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PM_Select_Quote_Fees'
GO


CREATE PROCEDURE spu_PM_Select_Quote_Fees
    @risk_group_id int,
    @transaction_type_code varchar(10),
    @insurance_folder_cnt int=NULL
AS

BEGIN

    DECLARE @insurance_file_cnt_addon int

    SELECT @insurance_file_cnt_addon=MAX(insurance_file_cnt) 
    FROM insurance_file 
    WHERE insurance_folder_cnt=@insurance_folder_cnt
    AND addon_created=1

	SELECT
        p.shortname,
        fa.party_cnt,
        fa.fee_percentage,
        fa.fee_amount,
        fa.commission_percentage,
        fa.commission_amount,
        -- Changed from incorrectly looking at "display on quotes" field, to looking if the
        --transaction_type_id on fee_amounts is the same as the one for the code passed in,
        --because transaction_Type_id on Fee_amounts means "is mandatory" for that transaction_type
        isMandatory = (Select 1 From transaction_type tt
                         Where transaction_type_id = fa.transaction_type_id And code = @transaction_type_code),
        is_fee = CASE
        	WHEN pt.code = 'FE' THEN 1
        	ELSE 0
        END,
        active = CONVERT(tinyint, 0),
        discount = CONVERT(numeric(19, 4), 0),
        oikey = '',
        fa.extra_scheme_id,
        -- add a dummy flag for saying if IPT is applicable. Needs to be like this to keep
        --same structure as Rule File fees (extras/Addons). Cannot see how to determine if IPT
        -- is relevant for these records, not required at this point anyway.
 	fa.tax_group_id,
        -- Display on Quotes - use this to "select" (or tick) this fee by default, but it's not
        -- mandatory (unless isMandatory above = 1)
        CASE WHEN @transaction_type_code = 'G_RENEW' AND @insurance_file_cnt_addon <> NULL THEN
	    ISNULL((SELECT 1 FROM policy_fee pf 
	    WHERE pf.insurance_file_cnt= @insurance_file_cnt_addon AND pf.party_cnt=p.party_cnt),0)
	ELSE
 	    fa.display_on_quotes
	END,
	fa.commission_tax_group_id,
	fee_tax_amount = CONVERT(numeric(19, 4), 0),
	fee_commission_tax_amount = CONVERT(numeric(19, 4), 0),
	fa.fsa_type_of_sale_id,
 	fa.tax_rates_id,
	fa.extra_amount_basis
    FROM
        fee_amounts fa
    INNER JOIN
        party p ON p.party_cnt = fa.party_cnt
    INNER JOIN
        party_type pt ON p.party_type_id = pt.party_type_id
	--eck datasure this table is now redundant
	--LEFT JOIN
	--	tax_rates tr ON fa.tax_rates_id = tr.tax_rates_id
    LEFT OUTER JOIN transaction_type trant ON fa.transaction_type_id = trant.transaction_type_id
	
    WHERE
        fa.risk_group_id = @risk_group_id
	AND (fa.transaction_type_id IS NULL
	OR   (trant.code = @transaction_type_code OR trant.code IS NULL))
END

GO


