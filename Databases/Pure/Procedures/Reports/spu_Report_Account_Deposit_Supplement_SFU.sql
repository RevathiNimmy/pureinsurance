SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Account_Deposit_Supplement_SFU'
GO

CREATE PROCEDURE spu_Report_Account_Deposit_Supplement_SFU
    @cashlist_id INT
   
AS

SELECT --DISTINCT
    D.description 'CashDrawer', 
    B.code 'Bank Code',
    I.amount,
    M.description 'MediaType', 
    M.code 'MediaTypeCode',
    MV.code 'MediaTypeValidationCode'
    
FROM 
    CashListItem I, 
    CashList C, 
    CashList_Drawer D, 
    CashListItem_Bank B,
    MediaType M, 
    MediaType_Validation MV
-- Join Tables...
WHERE 
    I.cashlist_id = C.cashlist_id 
AND 
    C.cashlist_drawer_id = D.cashlist_drawer_id
AND 
    I.cashlistitem_bank_id = B.cashlistitem_bank_id
AND 
    I.mediatype_id = M.mediatype_id
AND 
    M.mediatype_validation_id = MV.mediatype_validation_id
-- Apply Filters...
AND
    C.cashlist_id = @cashlist_id 