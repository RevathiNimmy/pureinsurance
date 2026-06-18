SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Sel_Commission_Main_Moved'
GO


CREATE PROCEDURE spu_ACT_Sel_Commission_Main_Moved
    @transdetail_id int,
    @earned char(1) OUTPUT
AS
BEGIN

/*Has any commission moved for this document*/

DECLARE @document_ref varchar(25)
DECLARE @Company_Id int

SELECT @document_ref = d.document_ref
			FROM document d, transdetail t
			WHERE t.transdetail_id = @transdetail_id
			AND t.document_id = d.document_Id

SELECT @company_id = d.company_id
			FROM document d, transdetail t
			WHERE t.transdetail_id = @transdetail_id
			AND t.document_id = d.document_Id

SELECT @earned = "N"

SELECT @earned = "Y"
FROM Transdetail
WHERE SPARE = "COMM PAY " + @document_ref
		AND company_id = @company_id
 

END

GO


