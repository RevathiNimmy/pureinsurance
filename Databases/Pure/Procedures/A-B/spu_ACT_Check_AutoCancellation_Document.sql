

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Execute DDLDropProcedure 'spu_ACT_Check_AutoCancellation_Document'
GO

CREATE PROCEDURE spu_ACT_Check_AutoCancellation_Document 
@insurance_file_cnt INT,
@flag INT OUTPUT
AS
SELECT @flag=count(*) FROM document_spooler WHERE insurance_file_cnt = @insurance_file_cnt AND description ='Auto Cancellation Document Printed'
SELECT @flag

GO

