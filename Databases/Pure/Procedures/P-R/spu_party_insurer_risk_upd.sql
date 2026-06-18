SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_party_insurer_risk_upd'
GO

CREATE PROCEDURE spu_party_insurer_risk_upd
(
    @party_cnt INT,
    @risk_code_id INT,
    @risk_transfer_agreement BIT,
    @delegated_authority BIT
)

AS

IF EXISTS
    (
        SELECT 
            NULL
        FROM party_insurer_risk
        WHERE party_cnt = @party_cnt
        AND risk_code_id = @risk_code_id
    )
BEGIN
    UPDATE party_insurer_risk
    SET risk_transfer_agreement = @risk_transfer_agreement,
        delegated_authority = @delegated_authority
    WHERE party_cnt = @party_cnt
    AND risk_code_id = @risk_code_id
END
ELSE
BEGIN
    INSERT INTO party_insurer_risk
    (
        party_cnt,
        risk_code_id,
        risk_transfer_agreement,
        delegated_authority
    )
    VALUES
    (
        @party_cnt,
        @risk_code_id,
        @risk_transfer_agreement,
        @delegated_authority
    )
END


GO
