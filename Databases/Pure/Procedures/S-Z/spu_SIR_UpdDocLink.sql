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

EXEC DDLDropProcedure 'spu_SIR_UpdDocLink'
GO


CREATE PROCEDURE spu_SIR_UpdDocLink(  
 @PMB_Doc_Link_Id INT,  
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
 @generate_through_BO Bit,
 @generate_through_SAM Bit,
 @UserId INT = NULL,
 @UniqueId VARCHAR(50) = NULL,
 @ScreenHierarchy VARCHAR(500) = NULL)  
AS  
BEGIN  
 UPDATE pmb_doc_link  
 SET  
 GIS_Scheme_Id=@GIS_Scheme_Id,  
 Process_Type_Id=@Process_Type_Id,  
 Document_Type_Id=@Document_Type_Id,  
 Document_Template_Id=@Document_Template_Id,  
 spool_document=@spool_document,  
 process_types_docs_id=@process_types_docs_id,  
 functional_area=@functional_area,  
 product_id=@product_id,  
 source_id=@source_id,  
 is_client=@is_client,  
 is_agent=@is_agent,  
 is_office=@is_office,  
 production_order=@production_order,
 generate_through_BO=@generate_through_BO,
 generate_through_SAM=@generate_through_SAM,
 UserId = @UserId,
 UniqueId = @UniqueId,
 ScreenHierarchy = @ScreenHierarchy
 WHERE PMB_Doc_Link_Id = @PMB_Doc_Link_Id  
END  
GO
