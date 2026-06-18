SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Check_CoInsurers'
GO


CREATE PROCEDURE spu_SIR_Check_CoInsurers 
@insurance_file_cnt int,
@Missing_Coinsurers tinyint OUTPUT
AS    
    
BEGIN    

	IF NOT EXISTS (SELECT NULL FROM policy_coinsurers WHERE insurance_file_cnt=@insurance_file_cnt)
		SELECT @Missing_CoInsurers = 1
	ELSE
		SELECT @Missing_CoInsurers = 0

END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

 