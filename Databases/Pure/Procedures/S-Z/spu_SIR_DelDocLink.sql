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

EXEC DDLDropProcedure 'spu_SIR_DelDocLink'
GO

CREATE PROCEDURE spu_SIR_DelDocLink(  
	@PMB_Doc_Link_Id INT,
	@UserId INT = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL)
AS 

BEGIN  
	UPDATE pmb_doc_link SET UserId = @UserId, UniqueId = @UniqueId, ScreenHierarchy = @ScreenHierarchy FROM pmb_doc_link WHERE PMB_Doc_Link_Id = @PMB_Doc_Link_Id
	DELETE FROM pmb_doc_link WHERE PMB_Doc_Link_Id = @PMB_Doc_Link_Id   
END  

GO
