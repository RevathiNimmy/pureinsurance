SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_Update_MarketplacePolicy_Status
GO

CREATE PROCEDURE spu_Update_MarketplacePolicy_Status 

	@nInsuranceFileKey INT,

	@nIsMarketplacePolicy INT

AS

    UPDATE Insurance_File SET is_marketplace_policy = @nIsMarketplacePolicy		
    WHERE  insurance_file_cnt = @nInsuranceFileKey

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

