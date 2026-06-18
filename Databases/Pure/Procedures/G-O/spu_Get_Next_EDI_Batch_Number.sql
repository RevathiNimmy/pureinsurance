SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Next_EDI_Batch_Number'
GO

CREATE PROCEDURE spu_Get_Next_EDI_Batch_Number
	@sBatchType varchar(3)
AS

BEGIN

	select Next_Batch_Number
	from next_edi_batch_number
	where rtrim(edi_batch_type) = rtrim(@sBatchType)

END
GO
