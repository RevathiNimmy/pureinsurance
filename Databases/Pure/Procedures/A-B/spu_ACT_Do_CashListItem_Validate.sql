SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_CashListItem_Validate'
GO


CREATE PROCEDURE spu_ACT_Do_CashListItem_Validate
(
    @cashlist_id        INT,
    @cashlistitem_id    INT,
    @mediatype_id       INT,
    @media_ref          VARCHAR(100),
    @period_months      TINYINT OUTPUT,
    @valid              TINYINT OUTPUT,
    @validate_ui        TINYINT OUTPUT
)
AS

/*
This procedure validates a cash list item by checking if a media reference has been re-used within
a period of months. It returns 1 for ok, 0 for failure
DD 16/05/2002: Created
DD 22/08/2003: Added CashListItem ID to avoid clashing with an existing record when an update occurs
*/

DECLARE @bankaccount_id INT
DECLARE @cashlisttype_id INT
DECLARE @payment_date DATETIME

SELECT
    @cashlisttype_id=cashlisttype_id,
    @bankaccount_id=bankaccount_id,
    @payment_date=list_date
FROM
    CashList
WHERE
    cashlist_id=@cashlist_id

/* See if a specific validation exists */
SELECT
    @period_months=period_months
FROM
    CashListItem_Validation
WHERE
    mediatype_id=@mediatype_id
AND bankaccount_id=@bankaccount_id
AND cashlisttype_id=@cashlisttype_id
AND is_deleted=0

IF @period_months IS NULL
BEGIN
    /* See if a payment type validation exists */
    SELECT
        @period_months=period_months
    FROM
        CashListItem_Validation
    WHERE
        mediatype_id=@mediatype_id
    AND bankaccount_id IS NULL
    AND cashlisttype_id=@cashlisttype_id
    AND is_deleted=0
END

IF NOT @period_months IS NULL
BEGIN
    /* A validation exists so tell the UI to double-check */
    SELECT @validate_ui=1

    /* Validate the Item by seeing if the same reference has been used previously
    within our validation period */
    SELECT
        cli.cashlistitem_id
    FROM
        cashlistitem cli
    INNER JOIN
        cashlist cl ON cl.cashlist_id=cli.cashlist_id
    WHERE
        DATEDIFF(Month, cl.list_date,@payment_date)<=@period_months
    AND cli.media_ref=@media_ref
    AND cl.cashlisttype_id=@cashlisttype_id
    AND cli.cashlistitem_id<>@cashlistitem_id

    /* If Records are found then we have a failure */
    IF @@ROWCOUNT>0
        SELECT @valid=0
    ELSE
        SELECT @valid=1
END
ELSE
BEGIN
    /* Nothing to validate so OK */
    SELECT @valid=1
    SELECT @validate_ui=0
END
GO
