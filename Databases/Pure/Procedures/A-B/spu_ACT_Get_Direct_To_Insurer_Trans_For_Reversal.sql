SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Get_Direct_To_Insurer_Trans_For_Reversal'
GO

CREATE PROCEDURE spu_ACT_Get_Direct_To_Insurer_Trans_For_Reversal
    @document_id INT
AS 
    
SELECT
    d2.document_id,
    td2.transdetail_id
FROM document d
JOIN transdetail td
    ON td.document_id = d.document_id
    AND td.document_sequence = 1
JOIN transmatch tm
    ON tm.transdetail_id = td.transdetail_id
    AND tm.is_reversed IS NULL
JOIN transmatch tm2
    ON tm2.match_id = tm.match_id
    AND tm2.transdetail_id <> tm.transdetail_id
JOIN transdetail td2
    ON td2.transdetail_id = tm2.transdetail_id
JOIN document d2
    ON d2.document_id = td2.document_id
JOIN documenttype dt2
    ON dt2.documenttype_id = d2.documenttype_id
WHERE d.document_id = @document_id
AND dt2.code IN ('DID', 'DIC')
AND (
        SELECT
            SUM(1)
        FROM transmatch
        WHERE match_id = tm.match_id
    ) = 2
    
    
GO