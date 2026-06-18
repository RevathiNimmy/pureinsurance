SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

DDLDropProcedure 'spu_CLR_Get_FinancePlan_By_ClaimNumber'
GO

-- PBI #37524: Instalment for Claim Recovery - New Plan
-- Returns claim recovery instalment plans filtered by claim number
-- Used by Plan Maintenance search when searching by claim reference
CREATE PROCEDURE spu_CLR_Get_FinancePlan_By_ClaimNumber
	@clientID       INT = 0,
	@Status         INT = 0,
	@ClaimNumber    VARCHAR(30) = NULL,
	@AgentKey       INT = 0
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		pf.PFPrem_Finance_Cnt       AS PlanKey,
		pf.PFPrem_Finance_Version   AS PlanVersion,
		pf.plan_reference           AS PlanReference,
		pf.StatusInd                AS StatusInd,
		pf.source_type              AS SourceType,
		pf.claim_number             AS ClaimNumber,
		pf.start_date               AS StartDate,
		pf.end_date                 AS EndDate,
		pf.amount_to_finance        AS AmountToFinance,
		pf.deposit_amount           AS DepositAmount,
		pf.total_cost               AS TotalCost,
		pf.no_of_instalments        AS NumberOfInstalments,
		pf.interest_rate            AS InterestRate,
		pf.currency_code            AS CurrencyCode,
		p.party_key                 AS PartyKey,
		p.party_name                AS PartyName,
		p.short_code                AS ShortCode
	FROM
		PFPremiumFinance pf
		INNER JOIN PAR_Party p
			ON pf.party_key = p.party_key
	WHERE
		(@ClaimNumber IS NOT NULL AND @ClaimNumber <> '')
		AND pf.claim_number = @ClaimNumber
		AND ISNULL(pf.source_type, 'PF') = 'CLR'
		AND (@clientID = 0 OR pf.party_key = @clientID)
		AND (@Status = 0 OR pf.StatusInd = @Status)
		AND (@AgentKey = 0 OR pf.agent_key = @AgentKey)
	ORDER BY
		pf.start_date DESC,
		pf.plan_reference

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
