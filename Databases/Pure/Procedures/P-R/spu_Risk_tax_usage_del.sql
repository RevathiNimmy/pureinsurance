SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Risk_Tax_Usage_Del'
GO

CREATE PROCEDURE spu_Risk_Tax_Usage_Del
    @risk_code_id int 

AS


DELETE FROM Risk_Tax_Usage WHERE risk_code_id = @risk_code_id
 
GO