SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Get_Standard_Policy_Wordings'
GO
--Start (girija) - UIIC WR22 – Capture Quote Details – Policy Standard Wordings-(6.1)
CREATE PROCEDURE spu_SAM_Get_Standard_Policy_Wordings  
    @insurance_file_cnt int,
    @get_fresh_with_default INT =0,
    @branch_id int=0 
    
AS  
  
BEGIN

IF (@get_fresh_with_default<>0)
BEGIN
    DECLARE @Product_type_id INT

    SELECT @Product_type_id=Insurance_File.product_id
    FROM Insurance_File
	WHERE insurance_file_cnt =@insurance_file_cnt

	SELECT  dt.code,
		dt.description,
		dt.document_template_id,
		'' 'OriginalDocumentCode'
	FROM
		document_template dt

    LEFT JOIN document_type ty ON ty.document_type_id=dt.document_type_id
    LEFT JOIN wording_product_link wpl ON wpl.document_template_id=dt.document_template_id
    WHERE
    ty.code = 'CLAUSES'  and dt.is_deleted=0 AND dt.effective_date<getdate()
    and dt.document_template_id in (
    SELECT  document_template_id
    FROM    wording_Product_link
    WHERE   Product_id = @Product_type_id)  AND  wpl.Product_id= @Product_type_id AND   wpl.branch_id =@branch_id AND wpl.[default]=1

END
ELSE
BEGIN

SELECT dt.code, dt.description, dt.document_template_id,
ISNULL((Select code from Document_Template where original_document_template_id=dt.document_template_id AND dt.document_template_id<0),dt.code) 'OriginalDocumentCode' 
FROM policy_standard_wording psw, document_template dt
 WHERE psw.insurance_file_cnt = @insurance_file_cnt
   AND psw.document_template_id = dt.document_template_id
 ORDER BY psw.policy_standard_wording_id

END
END

--End (girija) - UIIC WR22 – Capture Quote Details – Policy Standard Wordings-(6.1)


GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
