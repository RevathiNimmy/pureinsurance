SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Get_Payment_Reversal_Details'
GO

CREATE PROCEDURE spu_ACT_Get_Payment_Reversal_Details
@cashlistitemid int
AS
select
cli.transdetail_id,
clirt.code
from 
cashlistitem cli,
cashlistitem_payment_type clirt
where
cli.cashlistitem_payment_type_id = clirt.cashlistitem_payment_type_id
and
cli.cashlistitem_id = @cashlistitemid
GO

