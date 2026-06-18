SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_Unmoved_Commission'
GO

CREATE PROCEDURE spu_ACT_Select_Unmoved_Commission
    
AS

SELECT
    sat.linked_transdetail_id,
    ISNULL(ad.allocation_id, 0)
FROM suspended_accounts_transactions sat
JOIN transdetail td 
    ON td.transdetail_id = sat.suspended_transdetail_id
JOIN transdetail td2 
    ON td2.transdetail_id = sat.linked_transdetail_id
LEFT JOIN allocationdetail ad
    ON ad.transdetail_id = td2.transdetail_id
WHERE sat.is_deleted = 0
AND td2.outstanding_amount = 0
AND td.outstanding_amount <> 0
AND (
        (
            ad.allocation_id IS NULL
            AND
            td2.amount = 0
        )
        OR
        (
            ad.allocation_id IS NOT NULL
            AND
            NOT EXISTS 
                (
                    SELECT 
                        NULL 
                    FROM released_accounts_transactions
                    WHERE suspended_transdetail_id = sat.suspended_transdetail_id
                    AND allocation_id = ad.allocation_id
                )
        )
    )
    
GO