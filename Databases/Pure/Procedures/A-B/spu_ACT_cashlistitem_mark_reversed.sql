SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_CashListItem_Mark_Reversed'
GO

CREATE PROCEDURE spu_ACT_CashListItem_Mark_Reversed
    @cashlistitem_id INT

AS

DECLARE @current_date DATETIME

SELECT @current_date = GETDATE()
SELECT @current_date = CAST(YEAR(@current_date) AS VARCHAR(20)) + '-' + CAST(MONTH(@current_date) AS VARCHAR(20)) + '-' + CAST(DAY(@current_date) AS VARCHAR(20))

/*
If already exported, then the Reversed flag is set to 1
If not already exported, then the Reversed flag is set to 2
*/
IF EXISTS
    (
        SELECT 
            is_exported
        FROM cashlistitem
        WHERE cashlistitem_id = @cashlistitem_id
        AND ISNULL(is_exported, 0) = 1
    )
BEGIN
    UPDATE cashlistitem
    SET is_exported = 0,
        is_reversed = 1,
        reversed_date = @current_date
    WHERE cashlistitem_id = @cashlistitem_id
END
ELSE
BEGIN
    UPDATE cashlistitem
    SET is_exported = 0,
        is_reversed = 2,
        reversed_date = @current_date
    WHERE cashlistitem_id = @cashlistitem_id
END

GO