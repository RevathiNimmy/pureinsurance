SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_sir_check_policy_version_lapsed'
GO
CREATE PROCEDURE spu_sir_check_policy_version_lapsed
	@Insurance_file_cnt INT,
	@lapsed_reason_id	INT = 0 OUTPUT 
AS    
BEGIN
--set out put parameter if we found the policy is lapsed 
	SELECT @lapsed_reason_id=insurance_file_status_id 
		FROM insurance_file 
		WHERE Insurance_file_cnt=@Insurance_file_cnt  and insurance_file_status_id in (2, 15)

End
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



