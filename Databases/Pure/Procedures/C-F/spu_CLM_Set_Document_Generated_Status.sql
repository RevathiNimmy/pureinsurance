SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_CLM_Set_Document_Generated_Status'
GO

CREATE PROCEDURE spu_CLM_Set_Document_Generated_Status
    @claimkey int,
    @documentGeneratedStatus bit
AS
	BEGIN
		UPDATE claim
			SET document_generated_status=@documentGeneratedStatus

		WHERE   claim_id = @claimkey
 	END      

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
