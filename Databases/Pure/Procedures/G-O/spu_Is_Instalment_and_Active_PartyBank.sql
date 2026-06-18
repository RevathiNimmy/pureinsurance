
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Is_Instalment_and_Active_PartyBank'
GO

CREATE PROCEDURE spu_Is_Instalment_and_Active_PartyBank
	@InsuranceFileCnt int  
AS

Declare @Installment_Exists INT
Declare @Party_Bank_Active INT

IF EXISTS ( SELECT insurance_file_cnt FROM PFPremiumFinance 
			WHERE PFPremiumFinance.insurance_file_cnt = @InsuranceFileCnt) 
	SET  @Installment_Exists  = 1
ELSE
	SET  @Installment_Exists  = 0

IF EXISTS ( SELECT insurance_file_cnt FROM PFPremiumFinance 
	LEFT JOIN Party_Bank PB ON 
	PB.party_bank_id = PFPremiumFinance.Party_bank_id
	WHERE PFPremiumFinance.insurance_file_cnt = @InsuranceFileCnt  
	AND (PB.is_deleted = 0 OR PB.is_deleted IS NULL))

SET @Party_Bank_Active = 1 
ELSE 
SET @Party_Bank_Active = 0


SELECT @Installment_Exists, @Party_Bank_Active

