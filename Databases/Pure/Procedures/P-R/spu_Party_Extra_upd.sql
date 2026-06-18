SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Party_Extra_upd'
GO

CREATE PROCEDURE spu_Party_Extra_upd
(
    @party_cnt INT,
    @agency_number VARCHAR(255) = NULL,
    @fee_charge TINYINT = NULL,
    @risk_transfer_agreement BIT = NULL,
    @delegated_authority BIT = NULL,
    @fsa_product_id INT = NULL
)

AS 

UPDATE party_extra
SET agency_number = @agency_number,
    fee_charge = @fee_charge,
    risk_transfer_agreement = @risk_transfer_agreement,
    delegated_authority = @delegated_authority,
    fsa_product_id = @fsa_product_id
WHERE party_cnt = @party_cnt 


GO
