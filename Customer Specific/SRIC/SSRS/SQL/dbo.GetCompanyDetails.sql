CREATE PROCEDURE [dbo].[GetCompanyDetails]
(
	@effectiveDate DATETIME = NULL
)
AS
BEGIN

	SELECT TOP 1
		[CompanyName],
		[VATNumber],
		[RegistrationNumber],
		[FAISNumber],
		[Directors],
		[Secretary],
		[PartyId],
		[AssessmentCost],
		[AssessmentCostRecomender],
		[AssessmentCostAuthoriser],
		[AssessmentPartyCode],
	        [AssessmentReserveTypeId], 
		[CountryCode]
	FROM [etana].[CompanyDetail]
	WHERE [EffectiveDate] <= ISNULL(@effectiveDate, GETDATE())
	ORDER BY EffectiveDate DESC
END



