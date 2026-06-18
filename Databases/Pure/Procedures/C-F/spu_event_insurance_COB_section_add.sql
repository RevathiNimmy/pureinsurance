SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_event_insurance_COB_section_add'
GO
 
CREATE PROCEDURE spu_event_insurance_COB_section_add
    @Insurance_file_cnt integer,
    @Insurance_section_id Integer,
    @COB_Rating_section_id Integer,
    @Premium_Excluding_Tax Numeric(19,4),
    @Tax_applied Numeric(19,4),
    @Premium_Including_Tax Numeric(19,4),   
    @Tax_group_id integer,
    @Commission_Cnt integer,
    @Commission_Percentage Numeric(19,4),
    @Commission_Charge Numeric(19,4),
    @Commission_Net numeric(19,4),
    @Commission_tax_applied Numeric(19,4),
    @Commission_Payable Numeric(19,4),
    @Commission_Tax_group_id integer,
    @Is_minimum_brokerage Tinyint,
    @Override_rate_table Tinyint,
    @Base_Premium_Excluding_Tax Numeric(19,4),
    @Base_Tax_Applied Numeric(19,4),
    @Base_Premium_Including_Tax Numeric(19,4),
    @Base_Commission_Charge Numeric(19,4),
    @Base_Commission_Net Numeric(19,4),
    @Base_Commission_Tax_Applied Numeric(19,4),
    @Base_Commission_Payable Numeric(19,4),
    @is_applied Bit

AS

INSERT INTO event_insurance_COB_section
( 
    Insurance_file_cnt,
    COB_Rating_section_id ,
    Premium_Excluding_Tax ,
    Tax_applied,
    Premium_Including_Tax ,
    Tax_group_id,
    commission_cnt,
    commission_percentage,
    commission_charge,
    commission_net,
    commission_tax_applied,
    commission_payable,
    commission_tax_group_id,
    is_minimum_brokerage,
    override_rate_table,
    base_premium_excluding_tax,
    base_tax_applied,
    base_premium_including_tax,
    base_commission_charge,
    base_commission_net,
    base_commission_tax_applied,
    base_commission_payable,
    is_applied
)
VALUES 
(
    @Insurance_file_cnt, 
    @COB_Rating_section_id ,
    @Premium_Excluding_Tax ,
    @Tax_applied,
    @Premium_Including_Tax ,
    @Tax_group_id,
    @commission_cnt,
    @commission_percentage,
    @commission_charge,
    @commission_net,
    @commission_tax_applied,
    @commission_payable,
    @commission_tax_group_id,
    @is_minimum_brokerage,
    @override_rate_table,
    @base_premium_excluding_tax,
    @base_tax_applied,
    @base_premium_including_tax,
    @base_commission_charge,
    @base_commission_net,
    @base_commission_tax_applied,
    @base_commission_payable,
    @is_applied
)

SELECT @insurance_section_id = @@IDENTITY

SELECT @insurance_section_id
GO
 
