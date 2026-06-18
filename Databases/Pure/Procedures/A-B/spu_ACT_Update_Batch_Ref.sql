SET QUOTED_IDENTIFIER ON 
GO

EXECUTE DDLDropProcedure 'spu_ACT_Update_Batch_Ref'
GO


create procedure spu_ACT_Update_Batch_Ref
@batch_id int,
@batch_ref varchar(30)
as
update batch set batch_ref = @batch_ref where batch_id = @batch_id
GO

SET QUOTED_IDENTIFIER OFF 
GO
