SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Party_Extra_ins'
GO

CREATE PROCEDURE spu_Party_Extra_ins
(
    @party_cnt INT,
    @agency_number VARCHAR(255) = NULL,
    @fee_charge TINYINT = NULL,
    @risk_transfer_agreement BIT = NULL,
    @delegated_authority BIT = NULL,
    @fsa_product_id INT = NULL
)

AS

INSERT INTO party_extra
(
    party_cnt,
    agency_number,
    fee_charge,
    risk_transfer_agreement,
    delegated_authority,
    fsa_product_id
)
VALUES
(
    @party_cnt,
    @agency_number,
    @fee_charge,
    @risk_transfer_agreement,
    @delegated_authority,
    @fsa_product_id
)


GO


