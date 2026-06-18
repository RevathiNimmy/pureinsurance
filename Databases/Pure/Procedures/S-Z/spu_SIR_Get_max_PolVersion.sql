EXECUTE DDLDropProcedure spu_SIR_Get_max_PolVersion
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

/* AK 250402 - Get the maximum policy version for a Insurance Folder */

CREATE PROCEDURE spu_SIR_Get_max_PolVersion
@Insurance_Folder_Cnt Int 
AS
BEGIN

	Select Max(Policy_Version) from insurance_file where insurance_folder_cnt= @Insurance_Folder_Cnt
END

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
