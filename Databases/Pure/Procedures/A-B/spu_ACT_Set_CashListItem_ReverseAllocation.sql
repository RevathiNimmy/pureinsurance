SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Set_CashListItem_ReverseAllocation'
GO

CREATE PROCEDURE spu_ACT_Set_CashListItem_ReverseAllocation
	@cashlistitem_id INT = NULL,
	@reversed_date datetime = NULL,
	@cashlistitem_reverse_pmuser_id SMALLINT = NULL,
	@cashlistitem_reverse_reason_id INT = NULL,
	@cashlistitem_reversal_transdetail_id INT = NULL,
    @nIsReceiptReversal INT=0    
AS
DECLARE @cashlistitem_payment_status_id AS INT
DECLARE @nCashlistitem_receipt_status_id AS INT
	UPDATE CashListItem SET        	    
		    reversed_date = @reversed_date,
		    cashlistitem_reverse_pmuser_id = @cashlistitem_reverse_pmuser_id,
		    cashlistitem_reverse_reason_id = @cashlistitem_reverse_reason_id,
		    cashlistitem_reversal_transdetail_id = @cashlistitem_reversal_transdetail_id 
	WHERE cashlistitem_id = @cashlistitem_id

        UPDATE CashListItem_Claim_Link SET is_deleted = 1 WHERE cashlistitem_id = @cashlistitem_id
	IF @nIsReceiptReversal=0 
	BEGIN
	SELECT @cashlistitem_payment_status_id = ISNULL(cashlistitem_payment_status_id,0) FROM cashlistitem_payment_status WHERE code = 'CAN' AND is_deleted = 0
	IF @cashlistitem_payment_status_id > 0 
	BEGIN		
		UPDATE CashListItem SET cashlistitem_payment_status_id = @cashlistitem_payment_status_id WHERE cashlistitem_id = @cashlistitem_id
	END
END
    ELSE IF @nIsReceiptReversal=1
	BEGIN
	SELECT @ncashlistitem_receipt_status_id = isnull(cashlistitem_receipt_status_id,0) FROM cashlistitem_receipt_status WHERE code = 'CAN' AND is_deleted = 0  
	IF @ncashlistitem_receipt_status_id > 0  
		BEGIN  
		UPDATE CashListItem SET cashlistitem_receipt_status_id = @ncashlistitem_receipt_status_id WHERE cashlistitem_id = @cashlistitem_id  
		END  
	END
GO
