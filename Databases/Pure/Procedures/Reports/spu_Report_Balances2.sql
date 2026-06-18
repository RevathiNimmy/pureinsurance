SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Report_Balances2'
GO


CREATE PROCEDURE spu_Report_Balances2
    @branch_id int,
    @start_date datetime,
    @end_date datetime
AS


SELECT  T.amount,
        L.ledger_name,
        DT.code,
        D.document_date,
        opening_balance =
        (
        SELECT  ISNULL(SUM(amount), 0)
        FROM    Transdetail   T1,
            Document      D1,
            Account       A1
        WHERE   D1.document_id = T1.document_id
        AND     D1.document_date < @start_date
        AND     A1.account_id = T1.account_id
        AND     A1.ledger_id = L.ledger_id
        )
    FROM    Transdetail   T,
        Account       A,
        Document      D,
        DocumentType  DT,
        Ledger        L
    WHERE   L.ledger_id = A.ledger_id
    AND D.document_id = T.document_id
    AND DT.documenttype_id = D.documenttype_id
    AND A.account_id = T.account_id
    ORDER BY    L.ledger_name,
            DT.code
GO


