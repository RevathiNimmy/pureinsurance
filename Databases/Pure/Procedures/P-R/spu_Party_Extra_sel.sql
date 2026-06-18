SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Party_Extra_sel'
GO

CREATE PROCEDURE spu_Party_Extra_sel
(
    @party_cnt INT
)

AS

SELECT
    party_cnt,
    agency_number,
    fee_charge,
    risk_transfer_agreement,
    delegated_authority,
    fsa_product_id
FROM party_extra
WHERE party_cnt = @party_cnt
    
    
GO


