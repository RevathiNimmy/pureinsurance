/* AK - 04/02/2002 - created */
EXECUTE DDLDropProcedure 'spu_PMB_Process_Type_Sel'
GO

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_PMB_Process_Type_Sel
    @FunctionalArea int = 0
AS
    SELECT Process_Type_ID,  Description, Code 
    FROM Process_type
    WHERE  @FunctionalArea = 0 OR Functional_Area= @FunctionalArea	
    ORDER by Description

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



