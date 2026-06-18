EXECUTE DDLDropProcedure 'spu_ACT_Payment_Run'
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC spu_ACT_Payment_Run
    @PaymentTypeID INT,
    @PaymentMethodID INT,
    @BankAccountID INT,
    @PaymentDueDate DATETIME,
    @UserID INT,
    @CompanyID INT
AS

DECLARE @BatchID INT
DECLARE @BatchTypeID INT
DECLARE @BatchStatusID INT
DECLARE @MediaTypeValidationID INT

SELECT @BatchTypeID = batch_type_id
FROM  batch_type bt
WHERE code = 'PRUN'

SELECT @BatchStatusID = BatchStatus_id
FROM  BatchStatus
WHERE code = 'R'

SELECT @MediaTypeValidationID = mediatype_validation_id
FROM mediatype
WHERE mediatype_id = @PaymentMethodID

--PSL 25/02/2003 Issue 2457 Table does not have an identity column
SELECT @batchID = MAX(batch_id) + 1 FROM Batch
IF (@batchID IS NULL) SELECT @batchID = 1

INSERT INTO Batch (
    Batch_id,
    BatchStatus_id,
    company_id,
    user_id,
    created_date,
    batch_type_id,
    total_amount,
    total_transactions,
    interface_code,
    batch_ref,
    mediatype_validation_id)
SELECT
    @BatchID,
    @BatchStatusID,
    @CompanyID,
    @UserID,
    getdate(),
    @BatchTypeID,
    SUM(ci.amount),
    COUNT(*),
    'PAYMENT_RUN',
    0,
    @MediaTypeValidationID
FROM
    CashListItem ci, CashList c, CashListType ct, cashlistitem_payment_status cp
WHERE
    ci.CashList_id = c.CashList_id
AND c.CashListType_id = ct.CashListType_id
AND ct.Code = 'P'
AND ci.cashlistitem_payment_status_id = cp.cashlistitem_payment_status_id
AND cp.Code = 'ISS'
AND ci.cashlistitem_payment_type_id = @PaymentTypeID
AND ci.MediaType_id = @PaymentMethodID
AND c.bankaccount_id = @BankAccountID
AND CONVERT(VARCHAR(10),Transaction_date,112) = CONVERT(VARCHAR(10),@PaymentDueDate,112)
AND ci.batch_id IS NULL


--PSL 25/02/2003 Issue 2457 Table does not have an identity column
--SELECT @BatchID = @@IDENTITY
UPDATE Batch
SET batch_ref = @BatchID
WHERE batch_id = @BatchID

--PSL 05/03/2003 Wasn't updating cashlistitems
UPDATE CashListItem
SET batch_id = @BatchID
FROM CashListItem ci, CashList c, CashListType ct, cashlistitem_payment_status cp
WHERE
    ci.CashList_id = c.CashList_id
AND c.CashListType_id = ct.CashListType_id
AND ct.Code = 'P'
AND ci.cashlistitem_payment_status_id = cp.cashlistitem_payment_status_id
AND cp.Code = 'ISS'
AND ci.cashlistitem_payment_type_id = @PaymentTypeID
AND ci.MediaType_id = @PaymentMethodID
AND c.bankaccount_id = @BankAccountID
AND CONVERT(VARCHAR(10),Transaction_date,112) = CONVERT(VARCHAR(10),@PaymentDueDate,112)
AND ci.batch_id IS NULL

SELECT @BatchID as Batch_id, total_transactions
FROM Batch
WHERE batch_id = @BatchID 

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
