
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

EXEC DDLDropProcedure 'spu_SIR_SelDocLink'
GO

CREATE PROCEDURE spu_SIR_SelDocLink( 
	@functional_area TINYINT,
	@product_Id INT,
	@Process_Type_Id INT = 0,
	@source_id INT= 0,
	@Document_Template_Id INT =0)
AS     
BEGIN  
	DECLARE @SQL VARCHAR(2000)

	SET @SQL = 'SELECT
	pdl.PMB_Doc_Link_Id,  
	pdl.GIS_Scheme_Id,  
	pdl.Process_Type_Id,  
	pdl.Document_Type_Id,  
	pdl.Document_Template_Id,
	pdl.spool_document,  
	pdl.process_types_docs_id,  
	pdl.functional_area,  
	pdl.product_id,  
	pdl.source_id,  
	pdl.is_client,  
	pdl.is_agent,  
	pdl.is_office,  
	pdl.production_order,
	pt.description,
	s.description,
	ptd.description,
	dt.description
	
	FROM PMB_Doc_Link pdl        
	LEFT JOIN process_type pt ON pt.process_type_id = pdl.process_type_id
	LEFT JOIN source s ON s.source_id = pdl.source_id
	LEFT JOIN process_Types_Docs ptd ON ptd.process_types_docs_id = pdl.process_types_docs_id
	LEFT JOIN document_template dt ON dt.document_template_id = pdl.Document_Template_Id

	WHERE pdl.product_id = ' + CONVERT(VARCHAR(10),@product_Id) 

	SET @SQL = @SQL + ' AND pdl.functional_area = ' + CONVERT(VARCHAR(10),@functional_area)

	IF @Process_Type_Id > 0 BEGIN
		SET @SQL = @SQL + ' AND pdl.Process_Type_Id = ' + CONVERT(VARCHAR(10),@Process_Type_Id) END
	IF @source_id > 0 BEGIN
		SET @SQL = @SQL + ' AND pdl.source_id = ' + CONVERT(VARCHAR(10),@source_id) END
	IF @Document_Template_Id > 0 BEGIN
		SET @SQL = @SQL + ' AND pdl.Document_Template_Id = ' + CONVERT(VARCHAR(10),@Document_Template_Id) END

	EXEC(@SQL)
END

GO