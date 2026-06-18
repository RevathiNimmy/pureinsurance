
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_UpdateTransMatchWithCashListItem'
GO

CREATE PROCEDURE [dbo].[spu_UpdateTransMatchWithCashListItem]
    @CashListItemKey INT,
    @MarkedTransKeys NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE transmatch 
    SET cashlistitem_id = @CashListItemKey
    WHERE transdetail_id IN (
        SELECT CAST(value AS INT) 
        FROM STRING_SPLIT(@MarkedTransKeys, ',')
        WHERE ISNUMERIC(value) = 1
    );
END
Go
