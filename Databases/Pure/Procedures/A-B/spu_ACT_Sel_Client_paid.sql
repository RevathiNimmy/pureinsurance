SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Sel_Client_Paid'
GO

CREATE PROCEDURE spu_ACT_Sel_Client_Paid
    @transdetail_id int, 
    @true char(1) OUTPUT    
AS

BEGIN

DECLARE @client_amount numeric(19,4),
    @client_matched numeric(19,4)


SELECT @client_amount = 
            (SELECT sum(T2.amount)
                FROM Transdetail T,
                    Transdetail T2,
                    Account A
     
                WHERE   T.transdetail_id = @transdetail_id
                AND     T2.document_id = T.document_id
                AND     A.account_id = T2.account_id
                AND     A.ledger_id = 2 
            )
SELECT @client_matched =  
            (SELECT sum(M.base_match_amount)
                FROM Transdetail T,
                    Transdetail T2,
                    Account A,
                    Transmatch M
     
                WHERE   T.transdetail_id = @transdetail_id
                AND     T2.document_id = T.document_id
                AND     A.account_id = T2.account_id
                AND     A.ledger_id = 2
                AND     T2.transdetail_id = M.transdetail_id
                AND NOT EXISTS 
                    (SELECT ad3.transdetail_id 
                    FROM allocationdetail ad2,
                             allocationdetail ad3,
                         transdetail      t3 
                    WHERE ad3.allocation_Id = ad2.allocation_Id
                    AND   ad2.transdetail_id = T2.transdetail_id
                    AND   ad3.transdetail_id <> ad2.transdetail_id
                    AND   ad3.documenttype_id in (33,34)
                    AND   ad3.transdetail_id = t3.transdetail_id
                    AND   t3.amount = 
                        (
                            SELECT SUM(base_match_amount)
                            FROM transmatch
                            WHERE transdetail_id = ad3.transdetail_id
                        )
                    )
                        
                AND NOT EXISTS 
                    (Select t3.transdetail_id 
                    from transdetail t3,
                    account a3
                    where t3.document_id = T.document_id
                    and t3.account_id = a3.account_id
                    and a3.ledger_id = 10)
            )

SELECT @true = "N"

SELECT @true = "Y"
    WHERE @client_amount - @client_matched = 0
        
 
END
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

