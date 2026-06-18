SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_policy_coinsurers_section_add'
GO

CREATE PROCEDURE spu_policy_coinsurers_section_add
(
    @insurance_file_cnt INT,
    @party_cnt INT,
    @COB_rating_section_id INT, 
    @sequence INT = NULL,
    @share_percent FLOAT,
    @premium_exc_tax MONEY,
    @premium_inc_tax MONEY,
    @tax_group_id INT = NULL,
    @commission_percent FLOAT = NULL,
    @commission_charge MONEY = NULL,
    @commission_exc_tax MONEY = NULL,
    @commission_inc_tax MONEY = NULL,
    @commission_tax_group_id INT = NULL,
    @base_premium_exc_tax MONEY,
    @base_premium_inc_tax MONEY,
    @base_commission_charge MONEY = NULL,
    @base_commission_exc_tax MONEY = NULL,
    @base_commission_inc_tax MONEY = NULL,
    @override_rate_table tinyint,
    @is_applied Bit
)
AS



IF @Sequence IS NULL
BEGIN
    SELECT 
        @Sequence = ISNULL(MAX(sequence),1) 
    FROM policy_coinsurers_section 
    WHERE insurance_file_cnt = @insurance_file_cnt
END

INSERT INTO policy_coinsurers_section
(
    insurance_file_cnt,
    party_cnt,
    COB_rating_section_id,
    sequence,
    share_percent,
    premium_exc_tax,
    premium_inc_tax,
    tax_group_id,
    commission_percent,
    commission_charge,
    commission_exc_tax,
    commission_inc_tax,
    commission_tax_group_id,
    base_premium_exc_tax,
    base_premium_inc_tax,
    base_commission_charge,
    base_commission_exc_tax,
    base_commission_inc_tax,
    override_rate_table,
    is_applied
)
VALUES
(
    @insurance_file_cnt,
    @party_cnt,
    @COB_rating_section_id,
    @sequence,
    @share_percent,
    @premium_exc_tax,
    @premium_inc_tax,
    @tax_group_id,
    @commission_percent,
    @commission_charge,
    @commission_exc_tax,
    @commission_inc_tax,
    @commission_tax_group_id,
    @base_premium_exc_tax,
    @base_premium_inc_tax,
    @base_commission_charge,
    @base_commission_exc_tax,
    @base_commission_inc_tax,
    @override_rate_table,
    @is_applied
)

SELECT @@IDENTITY AS id


GO
