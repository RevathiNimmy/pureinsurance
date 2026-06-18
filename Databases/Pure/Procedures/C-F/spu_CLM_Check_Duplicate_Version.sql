SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

DDLDROPPROCEDURE spu_CLM_Check_Duplicate_Version 
GO

CREATE PROCEDURE spu_CLM_Check_Duplicate_Version 
@claim_id INT,
@is_duplicate BIT OUTPUT
AS
DECLARE @version_id INT,
		@base_claim_id INT

SELECT @version_id=version_id, @base_claim_id=base_claim_id 
FROM claim WHERE claim_id=@claim_id

IF EXISTS(SELECT claim_id FROM claim WHERE version_id=@version_id AND base_claim_id=@base_claim_id AND is_dirty=0) 
SET @is_duplicate=1
ELSE
SET @is_duplicate=0

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
