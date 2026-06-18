SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Sel_Comm_Earned'
GO


CREATE PROCEDURE spu_ACT_Sel_Comm_Earned
    @transdetail_id int,
    @earned char(1) OUTPUT
AS

/*** eck 01/08/01 check company on COMM PAY document ****/
BEGIN
    DECLARE @document_ref varchar(25)
    DECLARE @original_transdetail_id int
    DECLARE @company_id int

    SELECT @original_transdetail_id = T.transdetail_id
    FROM Transdetail T
    WHERE T.document_id IN (
        SELECT DISTINCT T.document_id
        FROM Transmatch M,
            AllocationDetail A,
            Transdetail T

        WHERE M.transdetail_id = @transdetail_id
        AND M.allocationdetail_id = A.allocation_id
        AND A.transdetail_id = T.transdetail_id
        )

    AND T.spare = 'BROK'

    SELECT @document_ref = d.document_ref,@company_id=d.company_id
    FROM Transdetail T,
        Document D
    WHERE T.document_id = D.document_id
    AND T.transdetail_id = @original_transdetail_id

    SELECT @earned = "N"

    SELECT @earned = "Y"
    FROM Transdetail
    WHERE SPARE = "COMM PAY " + @document_ref
    AND company_id = @company_id
END
GO


