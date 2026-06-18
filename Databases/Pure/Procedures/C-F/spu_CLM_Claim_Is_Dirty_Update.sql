SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Claim_Is_Dirty_Update'
GO

CREATE PROCEDURE spu_CLM_Claim_Is_Dirty_Update 

@claim_id int, 
@is_dirty int

AS

BEGIN
	UPDATE claim 
	SET is_dirty = @is_dirty
	WHERE claim_id = @claim_id

END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
