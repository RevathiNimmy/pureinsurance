SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Transaction_Type_Details'
GO

CREATE PROCEDURE spu_CLM_Get_Transaction_Type_Details

@code varchar(10)

AS
BEGIN
	SELECT transaction_type_id, description 
	FROM transaction_type 
	WHERE code = @code
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
