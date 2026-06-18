/* AK - 04/02/2002 - created */
EXECUTE DDLDropProcedure 'spu_PMB_Risk_Group_Sel'
GO

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_PMB_Risk_Group_Sel
AS

	Select Risk_Group_Id, Description from Risk_Group order by Description
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
