SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_copy_insurance_file_risk_link' 
GO
--******************************************************************
--- Description: Copies risk links between two versions of a insurance file
--- Author:Vidya Rangdale
--- Date Created:-16/09/2014 - 
--********************************************************************
CREATE PROCEDURE spu_copy_insurance_file_risk_link
    @OldInsuranceFileCnt int,
    @NewInsuranceFileCnt int,
    @CopyDeletedRisks	 bit
AS

    INSERT INTO insurance_file_risk_link (
		insurance_file_cnt,
		risk_cnt,
		status_flag)
	SELECT	@NewInsuranceFileCnt,
		risk_cnt,
	CASE WHEN status_flag = 'C' THEN 'U'
             WHEN status_flag = 'R' THEN 'U'
             WHEN status_flag = 'D' AND @CopyDeletedRisks = 1 THEN 'U'
	     WHEN status_flag = 'D' AND @CopyDeletedRisks = 0 THEN 'D'
	ELSE 'U'
	END
	FROM	insurance_file_risk_link
	WHERE	insurance_file_cnt = @OldInsuranceFileCnt
	AND     ((status_flag <> 'D' AND @CopyDeletedRisks = 0) OR @CopyDeletedRisks = 1)			    


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
