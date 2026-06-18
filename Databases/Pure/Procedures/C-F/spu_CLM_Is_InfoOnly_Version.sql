SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Is_InfoOnly_Version'
GO

CREATE PROCEDURE spu_CLM_Is_InfoOnly_Version  
    @claim_id int
AS  

DECLARE  
    @Info_Only bit

    Select @Info_Only = Info_Only  from claim WITH (NOLOCK)
      where base_claim_id = (Select base_claim_id From claim WITH (NOLOCK) where claim_id = @claim_id)
	AND version_id = (Select version_id - 1 From claim WITH (NOLOCK) where claim_id = @claim_id)

    Select @Info_Only


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

