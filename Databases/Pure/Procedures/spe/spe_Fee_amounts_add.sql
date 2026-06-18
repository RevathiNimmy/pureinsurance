SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_fee_amounts_add'
GO

CREATE PROCEDURE spe_fee_amounts_add
    @party_cnt INT,
    @risk_group_id INT,
    @fee_percentage NUMERIC(7,4),
    @fee_amount NUMERIC(19,4),
    @commission_percentage NUMERIC(7,4),
    @commission_amount NUMERIC(19,4),
	@display_on_quotes INT,
	@transaction_type_id INT,
	@tax_group_id INT,
	@extra_scheme_id INT,
	@currency_id SMALLINT,
    @commission_tax_group_id INT,
    @fsa_type_of_sale_id INT = -1
AS

INSERT INTO fee_amounts 
(
	party_cnt,
	risk_group_id,
	fee_percentage,
	fee_amount,
	commission_percentage,
	commission_amount,
	display_on_quotes,
	transaction_type_id,
	tax_group_id,
	extra_scheme_id,
	currency_id,
	commission_tax_group_id,
	fsa_type_of_sale_id
)
VALUES 
(
	@party_cnt,
	@risk_group_id,
	@fee_percentage,
	@fee_amount,
	@commission_percentage,
	@commission_amount,
	@display_on_quotes,
	@transaction_type_id,
	@tax_group_id,
	@extra_scheme_id,
	@currency_id,
	@commission_tax_group_id,
	@fsa_type_of_sale_id
)

GO

