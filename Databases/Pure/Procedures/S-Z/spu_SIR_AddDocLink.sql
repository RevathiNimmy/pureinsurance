
--**********************************************************************************************  
-- Author : Pankaj Kaushik
--   
-- History: 03/03/2008    
--
-- Task : Renewal Printing
--**********************************************************************************************  

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure 'spu_SIR_AddDocLink'
GO


CREATE PROCEDURE spu_SIR_AddDocLink(  
 @GIS_Scheme_Id INT,  
 @Process_Type_Id INT,  
 @Document_Type_Id INT,  
 @Document_Template_Id INT,  
 @spool_document TINYINT,  
 @process_types_docs_id INT,  
 @functional_area TINYINT,  
 @product_id INT,  
 @source_id INT,  
 @is_client TINYINT,  
 @is_agent TINYINT,  
 @is_office TINYINT,  
 @production_order TINYINT,  
 @PMB_Doc_Link_Id INT OUTPUT,
 @generate_through_BO Bit,
 @generate_through_SAM Bit,
 @UserId INT = NULL,
 @UniqueId VARCHAR(50) = NULL,
 @ScreenHierarchy VARCHAR(500) = NULL)  
AS  
BEGIN  
  
 --Select @PMB_Doc_Link_Id = ISNULL(max(PMB_Doc_Link_Id),0) + 1 from PMB_Doc_Link  
  
 INSERT INTO pmb_doc_link(  
  
 GIS_Scheme_Id,  
 Process_Type_Id,  
 Document_Type_Id,  
 Document_Template_Id,  
 spool_document,  
 process_types_docs_id,  
 functional_area,  
 product_id,  
 source_id,  
 is_client,  
 is_agent,  
 is_office,  
 production_order,  
    Auto_Archive_Document, generate_through_BO,generate_through_SAM,
	UserId,
	UniqueId,
	ScreenHierarchy)  
  
 VALUES(  
  
 @GIS_Scheme_Id,  
 @Process_Type_Id,  
 @Document_Type_Id,  
 @Document_Template_Id,  
 @spool_document,  
 @process_types_docs_id,  
 @functional_area,  
 @product_id,  
 @source_id,  
 @is_client,  
 @is_agent,  
 @is_office,  
 @production_order,  
    0,@generate_through_BO,@generate_through_SAM,
	@UserId,
	@UniqueId,
	@ScreenHierarchy)  
  
    SET @PMB_Doc_Link_Id = SCOPE_IDENTITY()  
END    
GO
