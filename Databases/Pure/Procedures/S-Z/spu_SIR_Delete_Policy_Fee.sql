SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Delete_Policy_Fee'
GO


CREATE PROCEDURE spu_SIR_Delete_Policy_Fee

@policy_fee_u_id int

AS

BEGIN

	DELETE FROM Tax_Calculation
	WHERE policy_fee_u_id = @policy_fee_u_id

	DELETE FROM policy_fee_u
	WHERE policy_fee_u_id = @policy_fee_u_id
	
END




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
