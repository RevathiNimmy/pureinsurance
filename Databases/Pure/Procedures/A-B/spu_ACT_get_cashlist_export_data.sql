SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_get_cashlist_export_data'
GO

CREATE PROCEDURE spu_ACT_get_cashlist_export_data
    @Sourceid INT
AS

SELECT 
    ISNULL(cli.reversed_date, cl.list_date),
    s.description, 
    cli.amount, 
    b.batch_ref, 
    cli.media_ref, 
    a.short_code, 
    a.account_name, 
    mt.description 
FROM cashlist cl 
JOIN 
    (   
        -- We need a union in here because the is_exported flag takes the following values:
        -- NULL or 0    Export the amount as normal
        -- 1    Previously exported item has been reversed therefore export a reversed amount
        -- 2    Un-exported item has been reversed, therefore export. Export the original amount AND a reversed amount.

        SELECT cashlistitem_id, cashlist_id, amount, media_ref, account_id, mediatype_id, batch_id, is_exported, NULL 'reversed_date'
        FROM cashlistitem cli
        WHERE (ISNULL(is_reversed, 0) IN(0,2))
        UNION
        -- Reverse amount for reversal items
        SELECT cashlistitem_id, cashlist_id, amount * (-1), media_ref, account_id, mediatype_id, batch_id, is_exported, reversed_date
        FROM cashlistitem cli
        WHERE (ISNULL(is_reversed, 0) IN(1,2))
    ) cli
    ON cli.cashlist_id = cl.cashlist_id


JOIN source s
    ON s.source_id = cl.company_id
JOIN bankaccount ba 
    ON ba.bankaccount_id = cl.bankaccount_id
JOIN account a 
    ON a.account_id = cli.account_id
JOIN mediatype mt 
    ON mt.mediatype_id = cli.mediatype_id
LEFT JOIN batch b 
    ON b.batch_id = cli.batch_id 
WHERE s.source_id = @Sourceid
AND cl.cashliststatus_id <> 1
AND ISNULL(cli.is_exported, 0) <> 1 
ORDER BY mt.description, ISNULL(cli.reversed_date, cl.list_date)

GO
