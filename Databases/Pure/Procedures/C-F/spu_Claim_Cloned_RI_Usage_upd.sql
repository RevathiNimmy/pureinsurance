SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Claim_Cloned_RI_Usage_upd'
GO

CREATE PROCEDURE spu_Claim_Cloned_RI_Usage_upd
	@NewInsuranceFileCnt INT,
	@status	INT  
AS

	UPDATE Claim_Cloned_RI_Usage
	SET status = @status -- Deleted/ Complete
	WHERE New_insurance_file_cnt = @NewInsuranceFileCnt
	
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO