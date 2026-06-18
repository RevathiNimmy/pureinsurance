SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_Batch_Info'
GO

CREATE PROCEDURE spu_ACT_Get_Batch_Info 
@batchsourcecode varchar(30)
AS
select 
batch_source_id, bankaccount_id
from
batch_source
where 
code = @batchsourcecode
GO
