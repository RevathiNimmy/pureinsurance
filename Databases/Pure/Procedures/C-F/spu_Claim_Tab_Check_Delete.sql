SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Claim_Tab_Check_Delete'
GO


CREATE PROCEDURE [spu_Claim_Tab_Check_Delete] 

@Tab_ID int

AS

SELECT Risk_Data_Defn_ID AS ID
FROM Risk_Data_Definition
WHERE Tab_ID = @Tab_ID

UNION

SELECT Peril_Data_Defn_ID AS ID
FROM Peril_Data_Definition
WHERE Tab_ID = @Tab_ID




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

