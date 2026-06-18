SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON

GO
Execute DDLDropProcedure 'spu_isParty_bank_Linked_With_Instalment'
GO

Create Procedure spu_isParty_bank_Linked_With_Instalment
	@Party_bank_id int
As
	Select party_bank_id FROM PFPremiumFinance 
	Where  party_bank_id = @party_bank_id
	AND StatusInd IN('040','140')