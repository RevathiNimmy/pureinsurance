SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_copy_sub_agent'
GO


CREATE PROCEDURE spu_copy_sub_agent
    @OldInsuranceFileCnt int,
    @NewInsuranceFileCnt int
AS

DECLARE @bExists int  = 0

SELECT @bExists = count(1) FROM insurance_file_agent WHERE insurance_file_cnt = @NewInsuranceFileCnt

IF @bExists = 0 
	BEGIN
		INSERT INTO insurance_file_agent (
			insurance_file_cnt,
			party_cnt,
			percentage,
			amount)
		SELECT  @NewInsuranceFileCnt,
			party_cnt,
			percentage,
			amount
		FROM    insurance_file_agent
		WHERE   insurance_file_cnt = @OldInsuranceFileCnt
	END
GO


