SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_Payment_Status_Code'
GO

CREATE PROCEDURE spu_ACT_Get_Payment_Status_Code
@statusid int
as
select 
code 
from 
cashlistitem_payment_status
where
cashlistitem_payment_status_id = @statusid
GO
