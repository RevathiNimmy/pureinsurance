SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_UpdateCashListItem_For_AllocationStatus'
GO
Create Procedure spu_ACT_UpdateCashListItem_For_AllocationStatus
@nTransdetail_id int

As
DECLARE @crAmount Money
SET @crAmount = (SELECT  outstanding_amount  from TransDetail WHERE transdetail_id = @nTransdetail_id)
If @crAmount = 0
BEGIN
UPDATE CashListItem Set allocationstatus_id=(Select allocationstatus_id FROM allocationstatus WHERE code ='A') 
WHERE transdetail_id = @nTransdetail_id
END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO