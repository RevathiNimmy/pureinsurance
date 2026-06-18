EXECUTE DDLDropProcedure spu_Get_GIS_Policy_Link_Id
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

-- ED 17072002 - Original 

CREATE PROCEDURE spu_Get_GIS_Policy_Link_Id

@Insurance_File_Cnt Int 


AS

SELECT gis_policy_link_id FROM gis_policy_link WHERE insurance_file_cnt = @Insurance_File_Cnt 

GO


SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
