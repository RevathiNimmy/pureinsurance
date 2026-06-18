SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Insurance_File_Type'
GO

CREATE PROCEDURE spu_SAM_Get_Insurance_File_Type

@insurance_file_cnt int 

AS 

SELECT 
	insf.insurance_file_type_id, 
    ift.code insurance_file_type_code,
    insf.policy_version,
    insf.out_of_sequence_replaced 
FROM 
	insurance_file insf
INNER JOIN insurance_file_type AS ift
		ON insf.insurance_file_type_id = ift.insurance_file_type_id
WHERE 
	insf.insurance_file_cnt = @insurance_file_cnt                

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

