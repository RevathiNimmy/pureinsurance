SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Update_Next_EDI_Batch_Number'
GO

CREATE PROCEDURE spu_Update_Next_EDI_Batch_Number
	@sBatchType varchar(3)
AS

BEGIN

	update next_edi_batch_number
	set next_batch_number = next_batch_number + 1
	where rtrim(edi_batch_type) = rtrim(@sBatchType)

END
GO
