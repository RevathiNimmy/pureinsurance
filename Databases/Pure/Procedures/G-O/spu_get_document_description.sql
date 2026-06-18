-- Returns Document template description 
-- based on document_template_id

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_document_description'
GO

CREATE  PROCEDURE spu_get_document_description
    @document_template_id INT
   
AS

    SELECT 
	dt.document_template_id,
    	dt.code,
    	dp.description,  
    	dt.description
    FROM    document_template dt
    JOIN document_type dp
    ON dt.document_type_id=dp.document_type_id
    WHERE   document_template_id = @document_template_id

GO
