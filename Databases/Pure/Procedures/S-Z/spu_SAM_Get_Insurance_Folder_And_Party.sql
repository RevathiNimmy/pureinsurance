SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Insurance_Folder_And_Party'
GO

CREATE PROCEDURE spu_SAM_Get_Insurance_Folder_And_Party

@InsuranceFileRef varchar(255)

AS

BEGIN

SELECT DISTINCT insurance_folder_cnt 'Insurance_Folder_Cnt', 
		insured_cnt 'Party_Cnt'
FROM insurance_file
WHERE insurance_file.insurance_ref = @InsuranceFileRef

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO



