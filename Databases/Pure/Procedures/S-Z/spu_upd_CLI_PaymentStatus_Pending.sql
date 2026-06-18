SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Upd_CLI_PaymentStatus_Pending'
GO

CREATE PROCEDURE spu_Upd_CLI_PaymentStatus_Pending
    @CashListItem_ID INT
AS
    UPDATE CashListItem SET CashListItem_Payment_Status_ID = (
    SELECT CashListItem_Payment_Status_ID FROM CashListItem_Payment_Status
    WHERE Code= 'PENDING')
    WHERE @CashListItem_ID  = CashListItem_ID