SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Update_Policy_Fee'
GO



CREATE PROCEDURE spu_SIR_Update_Policy_Fee

@policy_fee_u_id int, 
@fee_percentage money, 
@fee_amount money,  
@FeeTypePercent bit = null

AS

BEGIN

	If @fee_percentage <> 0 and @FeeTypePercent IS NULL 
	  BEGIN
		SELECT @FeeTypePercent = 1
	  END 



	-- update policy_fee_u values
	UPDATE policy_fee_u SET
		fee_rate_percentage = @fee_percentage,
		fee_rate_amount = @fee_amount,    
  		FeeTypePercent = @FeeTypePercent

	WHERE policy_fee_u_id = @policy_fee_u_id 

	-- recalculate the fee amounts based on the new values
	EXEC spu_SIR_Calculate_Fee_Amounts @policy_fee_u_id =@policy_fee_u_id

	-- recalculate the tax amounts based on the new values
	EXEC spu_SIR_Calculate_Fee_Tax_Amounts @policy_fee_u_id = @policy_fee_u_id


END




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
