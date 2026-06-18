SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_event_Policy_fee_add'
GO

 
CREATE PROCEDURE spe_event_Policy_fee_add
    @insurance_file_cnt        int,
    @party_cnt                 int,
    @fee_percentage            numeric(7,4),
    @fee_amount                numeric(19,4),
    @commission_percentage     numeric(7,4),
    @commission_amount         numeric(19,4),
    @isIPTable                 int,
    @extra_scheme_id           int,
    @base_currency_id	       int,
    @tax_amount		       numeric(19,4),
    @total_fee		       numeric(19,4),
    @commission_tax_amount     numeric(19,4),
    @total_commission	       numeric(19,4),
    @fsa_type_of_sale_id       int = -1,
    @insurer_fee_type	       char(1)=NULL
AS
BEGIN

    INSERT INTO event_Policy_fee 
        (
        insurance_file_cnt,
        party_cnt,
        fee_percentage,
        fee_amount,
        commission_percentage,
        commission_amount,
        isIPTable,
        extra_scheme_id,
	base_currency_id,
	tax_amount,
	total_fee,
	commission_tax_amount,
	total_commission,
        fsa_type_of_sale_id,
        insurer_fee_type
        )
    VALUES 
        (
        @insurance_file_cnt,
        @party_cnt,
        @fee_percentage,
        @fee_amount,
        @commission_percentage,
        @commission_amount,
        @isIPTable,
        @extra_scheme_id,
	@base_currency_id,
	@tax_amount,
    	@total_fee,
    	@commission_tax_amount,
    	@total_commission,
        @fsa_type_of_sale_id,
        @insurer_fee_type
        )


SELECT @@IDENTITY AS id

END
GO

