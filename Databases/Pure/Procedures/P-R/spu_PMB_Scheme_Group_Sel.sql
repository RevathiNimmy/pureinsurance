
EXECUTE DDLDropProcedure 'spu_PMB_Scheme_Group_Sel'
GO

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_PMB_Scheme_Group_Sel
@insurance_file_cnt as int
AS
BEGIN

	Select sg.GIS_Scheme_Group_Id  from GIS_Policy_Link pl, GIS_Qem_Usage qu, 
				            Risk_group rg, GIS_Scheme_Group sg
	Where  pl.Insurance_file_cnt = @insurance_file_cnt
	AND    qu.GIS_Scheme_ID = pl.GIS_Scheme_Id
	AND    qu.GIS_Data_Model_Id = pl.GIS_Data_Model_Id
	AND    rg.Risk_Group_Id = qu.Risk_Group_Id
	AND    sg.code = rg.code

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

