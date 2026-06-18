SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Code_For_Template_Id'
GO

/*******************************************************************************************************/
/* spu_SAM_Get_Code_For_Template_Id     */                                                                              
/* GEt Documnet template code for a given Id */
/*******************************************************************************************************/

CREATE PROCEDURE spu_SAM_Get_Code_For_Template_Id
  @document_template_id  INT
AS

SET NOCOUNT ON 

SELECT 
	code 
FROM 
	document_template
WHERE
	document_template_id = @document_template_id

SET NOCOUNT OFF

GO






  