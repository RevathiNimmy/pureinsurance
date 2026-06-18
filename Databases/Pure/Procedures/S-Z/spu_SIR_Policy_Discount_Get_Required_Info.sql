SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Get_Required_Info'
GO

CREATE PROCEDURE spu_SIR_Policy_Discount_Get_Required_Info

@code varchar(20)

AS

BEGIN

	SELECT transaction_type_id FROM transaction_type WHERE code = @code

END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
