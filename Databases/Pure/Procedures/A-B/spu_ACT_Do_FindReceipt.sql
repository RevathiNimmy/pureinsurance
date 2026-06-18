SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Do_FindReceipt'
GO

CREATE PROCEDURE spu_ACT_Do_FindReceipt
    @start_date datetime = NULL,
    @end_date datetime = NULL,
    @cashlistitem_receipt_type_id int = NULL,
    @account_name varchar(60) = NULL,
    @media_reference varchar(100) = NULL,
    @their_reference varchar(30) = NULL,
    @amount numeric(19,4) = NULL,
    @batch_number int = NULL,
    @mediatype_id int = NULL,
    @receipt_number int = NULL,
    @batch_ref varchar(50) = NULL,
    @user_id SMALLINT = NULL,
    @MaxRowsToFetch integer = 500            -- RAM20040514 : ByDefault fetch only 500

AS

-- Get the id of a 'Receipt' from the cashlisttype table
-- use 'hard-coded' code to search for 'R'
DECLARE @receipt_id as INT
SELECT @receipt_id = (
    SELECT
        cashlisttype_id
    FROM
        CashListType
    WHERE
        code = 'R')

--- RAM20040514 : Performance enhancement
SET NOCOUNT ON
SET ROWCOUNT @MaxRowsToFetch
--- RAM20040514 : Performance enhancement

SELECT
    I.cashlistitem_id,
    I.transaction_date,
    M.description 'Media Type',
    I.media_ref,
    I.their_ref,
    R.description 'Receipt Type',
    A.account_name,
    I.amount,
    Al.Description,
    U.username,
    I.cashlist_id

FROM
    CashList C, CashListItem I 
    LEFT JOIN Batch B on B.batch_id = I.batch_ID
    LEFT JOIN AllocationStatus Al on I.allocationstatus_id = Al.allocationstatus_id,
    MediaType M,
    CashListItem_Receipt_Type R,
    Account A,
    PMUser U
    
-- Join Tables
WHERE C.cashlist_id = I.cashlist_id
AND   I.cashlistitem_receipt_type_id = R.cashlistitem_receipt_type_id
AND   I.mediatype_id = M.mediatype_id
AND   I.account_id = A.account_id
AND   I.pmuser_id = U.user_id

--Apply filters...

--Filter by @start_date if passed
AND (I.transaction_date >= @start_date OR @start_date IS NULL)

--Filter by @end_date if passed
AND (I.transaction_date <= @end_date OR @end_date IS NULL)

--Filter by @cashlistitem_receipt_type_id if passed
AND (I.cashlistitem_receipt_type_id = @cashlistitem_receipt_type_id or @cashlistitem_receipt_type_id IS NULL)

--Filter by @account_name if passed
-- 07/03/2003 - AMB IS2826: value passed in is actually short_code so search on that
AND (A.short_code  = @account_name or @account_name = NULL)

--Filter by @media_reference if passed
AND (I.media_ref = @media_reference or @media_reference IS NULL)

--Filter by @their_reference if passed
AND (I.their_ref = @their_reference or @their_reference IS NULL)

--Filter by @amount if passed
AND (I.amount = @amount or @amount IS NULL)

--Filter by @batch_number if passed
AND (I.batch_id = @batch_number or @batch_number IS NULL)

--Filter by @media_type_id if passed
AND (I.mediatype_id = @mediatype_id or @mediatype_id IS NULL)

--Filter by @receipt_number if passed
AND (I.cashlistitem_id = @receipt_number or @receipt_number IS NULL)

--Filter by @batch_ref if passed
AND (B.batch_ref LIKE @batch_ref or @batch_ref IS NULL)

-- Only want to get Receipts
AND C.cashlisttype_id = @receipt_id

-- DD 16/10/2003: And only from Branches to which the user has access
AND C.company_id IN (SELECT source_id FROM PMUser_Source_Allowed WHERE user_id=@user_id)
        
--- RAM20040514 : Performance enhancement
SET ROWCOUNT 0
SET NOCOUNT OFF
--- RAM20040514 : Performance enhancement
    
GO


