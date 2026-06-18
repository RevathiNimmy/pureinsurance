SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_fees_update_party_extra'
GO

CREATE PROCEDURE spu_fees_update_party_extra
(
    @party_cnt  INT,
    @fee_charge TINYINT,
    @risk_transfer_agreement BIT,
    @delegated_authority BIT,
    @fsa_product_id INT
)

AS 

UPDATE party_extra 
SET fee_charge = @fee_charge,
    risk_transfer_agreement = @risk_transfer_agreement,
    delegated_authority = @delegated_authority,
    fsa_product_id = @fsa_product_id
WHERE party_cnt = @party_cnt


GO
