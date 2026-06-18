SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON

GO

EXECUTE DDLDropProcedure 'spu_Check_AutoReconciliation_Data_Exists'

GO

create PROCEDURE [dbo].[spu_Check_AutoReconciliation_Data_Exists]

    @DateGenerated datetime
	--@iCount int output
AS

begin

declare @iCount  int = 0
SELECT @iCount = Count(1) 
	   FROM Auto_ReconciliationRS 
	   WHERE DateGenerated = convert(datetime,@DateGenerated)

  SELECT @iCount

end 
go 
