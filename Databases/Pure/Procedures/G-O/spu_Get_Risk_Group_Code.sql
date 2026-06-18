EXECUTE DDLDropProcedure spu_Get_Risk_Group_Code
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

-- ED 17072002 - Original 

CREATE PROCEDURE spu_Get_Risk_Group_Code

@Insurance_File_Cnt Int 

AS

SELECT g.code 
  FROM insurance_file f 
 INNER JOIN risk_code c ON f.risk_code_id=c.risk_code_id
 INNER JOIN risk_group g ON c.risk_group_id=g.risk_group_id
 WHERE f.insurance_file_cnt = @Insurance_File_Cnt

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
