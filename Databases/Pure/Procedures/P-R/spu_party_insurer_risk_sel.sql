SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_party_insurer_risk_sel'
GO

CREATE PROCEDURE spu_party_insurer_risk_sel
(
    @party_cnt INT,
    @risk_code_id INT
)

AS

DECLARE @default_risk_transfer_agreement BIT

SELECT 
    @default_risk_transfer_agreement = ISNULL(risk_transfer_agreement, 0)
FROM party_insurer 
WHERE party_cnt = @party_cnt

SELECT
    rc.risk_code_id, 
    rc.description, 
    ISNULL(pir.risk_transfer_agreement, @default_risk_transfer_agreement),
    ISNULL(pir.delegated_authority, rc.is_delegated_authority)
FROM risk_code rc
LEFT JOIN party_insurer_risk pir
    ON pir.risk_code_id = rc.risk_code_id
    AND pir.party_cnt = @party_cnt
WHERE rc.is_deleted = 0
AND rc.risk_code_id = ISNULL(@risk_code_id, rc.risk_code_id)


GO