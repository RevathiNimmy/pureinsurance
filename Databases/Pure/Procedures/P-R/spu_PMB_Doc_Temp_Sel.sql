/* AK  - 04/02/2002 - created */
/* CJB - 03/09/2002 - Also retrieve the document template document type id so that they can be filtered in the interface */
 
EXECUTE DDLDropProcedure 'spu_PMB_Doc_Temp_Sel'
GO

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_PMB_Doc_Temp_Sel
AS
--DC230503 -ISS1871 ignore deleted templates
	Select Document_Template_Id, Description, document_type_id from Document_Template where is_deleted = 0 order by Description
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
