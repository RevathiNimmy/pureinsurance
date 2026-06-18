SET QUOTED_IDENTIFIER ON
GO
EXECUTE DDLDropProcedure 'spu_Get_PartyBank_instances'
GO
CREATE PROCEDURE spu_Get_PartyBank_instances
    @nPlanID INT,
    @nNoofInstances INT  OUTPUT
AS
BEGIN
   DECLARE @enumStatusLive     INT = '040',
		   @enumStatusOnHold   INT = '140'
		   
    SELECT @nNoofInstances =  Count(*) FROM PFPremiumFinance WHERE party_bank_id IN (
		SELECT Party_bank_id FROM PFPremiumFinance
		WHERE pfprem_finance_cnt = @nPlanID
		AND StatusIND IN(@enumStatusLive,@enumStatusOnHold)
		AND party_bank_id IS NOT NULL)   
	AND StatusInd= @enumStatusLive
END

SET QUOTED_IDENTIFIER OFF
GO

