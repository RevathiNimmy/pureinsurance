/* AK - 04/02/2002 - created */
EXECUTE DDLDropProcedure 'spu_PMB_Doc_Type_Sel'
GO

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_PMB_Doc_Type_Sel
AS

	Select Document_Type_Id, Description from Document_Type order by Description
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
