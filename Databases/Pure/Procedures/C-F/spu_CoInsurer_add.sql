SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_CoInsurer_add'
GO

CREATE PROCEDURE spu_CoInsurer_add
    @insurance_file_cnt INT,
    @party_cnt INT,
    @coinsurer_percentage FLOAT,
    @coinsurer_value MONEY,
    @coinsurer_commission_rate FLOAT,
    @coinsurer_commission_amount MONEY,
    @coinsurer_ipt_amount MONEY,
    @coinsurer_policy_number VARCHAR(30),
    @insurance_section_id INT,
    @coinsurer_net_commission MONEY,
    @coinsurer_commission_tax MONEY,
    @coinsurer_cover_percentage MONEY,
    @risk_transfer_agreement BIT,
    @signed_line_percentage NUMERIC(19,4) = 0,
    @line_stands TINYINT = 0,
    @written_line_percentage NUMERIC(19,4) = 0,
    @signed_line_amount NUMERIC(19,4) = 0,
    @bureau_party_cnt INT = NULL,
    @is_lead_underwriter TINYINT = 0,  
	@risk_transfer_editable Bit
AS

DECLARE @coinsurer_count INT

SELECT 
    @coinsurer_count = ISNULL(MAX(coinsurer_count),0) + 1
FROM policy_coinsurers 
WHERE insurance_file_cnt = @insurance_file_cnt

INSERT INTO Policy_Coinsurers
(
    insurance_file_cnt,
    party_cnt,
    coinsurer_count,
    coinsurer_percentage,
    coinsurer_value,
    coinsurer_commission_rate,
    coinsurer_commission_amount,
    coinsurer_ipt_amount,
    coinsurer_policy_number,
    insurance_section_id,
    coinsurer_net_commission,
    coinsurer_commission_tax,
    coinsurer_cover_percentage,
    risk_transfer_agreement,
    signed_line_percentage,
    linestands,
    written_line_percentage,
    signed_line_amount,
    bureau_party_cnt,
    isleadunderwriter,  
	risk_transfer_editable
)
VALUES
(
    @insurance_file_cnt,
    @party_cnt,
    @coinsurer_count,
    @coinsurer_percentage,
    @coinsurer_value,
    @coinsurer_commission_rate,
    @coinsurer_commission_amount,
    @coinsurer_ipt_amount,
    @coinsurer_policy_number,
    @insurance_section_id,
    @coinsurer_net_commission,
    @coinsurer_commission_tax,
    @coinsurer_cover_percentage,
    @risk_transfer_agreement,
    ISNULL(@signed_line_percentage,0),
    ISNULL(@line_stands,0),
    ISNULL(@written_line_percentage,0),
    ISNULL(@signed_line_amount,0),
    @bureau_party_cnt,
    ISNULL(@is_lead_underwriter,0),  
 	@risk_transfer_editable
)


GO


