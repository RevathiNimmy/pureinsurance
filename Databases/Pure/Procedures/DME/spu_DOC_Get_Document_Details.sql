/* stored procedure to fetch the Document details in an array */
/* Sirius Process No 	   : 189  		   	      */
/* NRMA Project Process No : 231			      */

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_Get_Document_Details'
GO

CREATE PROCEDURE spu_DOC_Get_Document_Details
    @doc_num 	int    
AS
BEGIN
	SELECT 	* 
	FROM 	doc_document
	WHERE	doc_num = @doc_num
END
GO