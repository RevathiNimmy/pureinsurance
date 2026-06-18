SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_party_insurer'
GO

CREATE PROCEDURE spu_wp_party_insurer
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @agency_number VARCHAR(255) OUTPUT,
    @binder_indicator INT OUTPUT,
    @report_indicator INT OUTPUT,
    @is_reinsurer INT OUTPUT,
    @reinsurance_type INT OUTPUT,
    @is_reinsurance_debit_credit TINYINT OUTPUT,
    @default_comm_rate FLOAT OUTPUT,
    @risk_transfer_agreement BIT OUTPUT
AS


SELECT  
    @agency_number = pi.agency_number,
    @binder_indicator = pi.binder_indicator,
    @report_indicator = pi.report_indicator,
    @is_reinsurer = pi.is_reinsurer,
    @reinsurance_type = pi.reinsurance_type,
    @is_reinsurance_debit_credit = pi.is_reinsurance_debit_credit_no,
    @default_comm_rate = pi.default_comm_rate,
    @risk_transfer_agreement = pi.risk_transfer_agreement
FROM party_insurer pi
WHERE pi.party_cnt = @PartyCnt

GO
