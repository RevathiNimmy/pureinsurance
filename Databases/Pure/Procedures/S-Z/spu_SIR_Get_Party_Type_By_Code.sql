SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Get_Party_Type_By_Code'
GO


CREATE PROCEDURE spu_SIR_Get_Party_Type_By_Code

@code char(10)

AS

BEGIN

	SELECT description 
	FROM party_type 
	WHERE code = @code

END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
