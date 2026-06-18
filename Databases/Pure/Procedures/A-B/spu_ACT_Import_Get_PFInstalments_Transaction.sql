SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Import_Get_PFInstalments_Transaction'
GO

CREATE PROCEDURE spu_ACT_Import_Get_PFInstalments_Transaction

AS

BEGIN

SELECT code, pfinstalments_transaction_id 
FROM pfInstalments_transaction


END


GO
