SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Allow_Consolidated_Commission'
GO

Create Procedure spu_Allow_Consolidated_Commission
	@party_cnt integer
AS
	SELECT ISNULL(PA.allow_consolidated_commission,0)allow_consolidated_commission                                                         
    From Party_agent PA
	WHERE PA.party_cnt = @party_cnt






