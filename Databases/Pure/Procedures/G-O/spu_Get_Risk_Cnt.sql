
EXECUTE DDLDropProcedure spu_Get_Risk_Cnt
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

-- ED 17072002 - Original 

CREATE PROCEDURE spu_Get_Risk_Cnt

@Insurance_File_Cnt Int 


AS

Select risk_cnt from insurance_file_risk_link where insurance_file_cnt=@Insurance_File_Cnt

GO


SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
