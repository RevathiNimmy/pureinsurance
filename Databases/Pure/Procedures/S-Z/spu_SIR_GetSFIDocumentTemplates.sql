
--**********************************************************************************************  
-- Author : Pankaj Kaushik
--   
-- History: 04/03/2008    
--
-- Task : Renewal Printing
--**********************************************************************************************  
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure 'spu_SIR_GetSFIDocumnetTemplates'
GO

CREATE PROCEDURE spu_SIR_GetSFIDocumnetTemplates( 
	@functional_area TINYINT,
	@product_Id INT,
	@Process_Type_Id INT = 0,
	@source_id INT= 0)
AS     
BEGIN  
	DECLARE @SQL VARCHAR(2000)

	SET @SQL = 'SELECT
	pdl.Document_Template_Id,
	dt.description
	
	FROM PMB_Doc_Link pdl        
	LEFT JOIN document_template dt ON dt.document_template_id = pdl.Document_Template_Id
	
	WHERE pdl.product_id = ' + CONVERT(VARCHAR(10),@product_Id) 

	SET @SQL = @SQL + ' AND pdl.functional_area = ' + CONVERT(VARCHAR(10),@functional_area)

	IF @Process_Type_Id > 0 BEGIN
		SET @SQL = @SQL + ' AND pdl.Process_Type_Id = ' + CONVERT(VARCHAR(10),@Process_Type_Id) END
	IF @source_id > 0 BEGIN
		SET @SQL = @SQL + ' AND pdl.source_id = ' + CONVERT(VARCHAR(10),@source_id) END
	EXEC(@SQL)
END

GO

