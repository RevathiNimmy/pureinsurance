SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_wp_ClaimAltPayee'
GO


CREATE PROCEDURE spu_wp_ClaimAltPayee
		 @PartyCnt INT,
		 @InsuranceFileCnt INT,
		 @RiskID INT,
		 @ClaimCnt INT,
		 @DocumentRef VARCHAR(25),
		 @Instance1 INT,
		 @Instance2 INT,
		 @Instance3 INT
AS

SELECT
	(SELECT description FROM mediatype WHERE mediatype_id = PayeeMediaType) PayeeMediaType,
	PayeeName,
	PayeeBankName,
	PayeeSortCode,
	PayeeAccountNo,
	(SELECT description FROM country WHERE country_id = PayeeCountry) PayeeCountry,
	PayeeComments,
	ThirdPartyReference,
 	PayeeAddress1,
 	PayeeAddress2,
 	PayeeAddress3,
 	PayeeAddress4,
 	PayeePostalCode,
 	media_ref PayeeMediaRef,
 	(SELECT account_type from party_bank where party_bank_id = Claim_Payment.party_bank_id) PayeeAccountType
FROM Claim_Payment
WHERE claim_payment_id = 
	(
		SELECT MAX(claim_payment_id)
		FROM claim_payment
		WHERE claim_id = @ClaimCnt
		AND ISNULL(amount, 0) <> 0
	)
