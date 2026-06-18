SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Policy_Refer_At_Renewal_Indicator_Value'
GO

CREATE PROCEDURE spu_Get_Policy_Refer_At_Renewal_Indicator_Value
	@insurance_file_cnt INT,
	@ReferAtRenewal     INT OUTPUT
AS
BEGIN

	SELECT  @ReferAtRenewal = is_referred_at_renewal 
	FROM    insurance_file 
	WHERE 	insurance_file_cnt = @insurance_file_cnt

END
GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO