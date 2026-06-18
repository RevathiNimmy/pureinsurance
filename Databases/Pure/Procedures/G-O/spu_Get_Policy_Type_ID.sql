SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Policy_Type_ID'
GO

CREATE PROCEDURE spu_Get_Policy_Type_ID
    @InsuranceFileCnt int
AS
BEGIN

	SELECT policy_type_id
	FROM   insurance_file
	WHERE  insurance_file_cnt = @InsuranceFileCnt

END

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
