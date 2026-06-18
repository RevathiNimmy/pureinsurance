SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Get_MTAQuotePolicyVersions'
GO

CREATE PROCEDURE spu_SIR_Get_MTAQuotePolicyVersions
	@insurance_file_cnt INT,  
	@insurance_folder_cnt INT   

AS

BEGIN

	SELECT insurance_file_cnt,insurance_folder_cnt,insurance_file_status_id 
	FROM insurance_file  IFL
    JOIN Insurance_file_type T ON IFL.insurance_file_type_id = T.insurance_file_type_id 
	WHERE insurance_folder_cnt = @insurance_folder_cnt   
        AND (T.Code = 'MTAQUOTE' OR T.Code='MTAQTETEMP'
		--AND (insurance_file_type_id=4 OR insurance_file_type_id=7 
		--Also select MTA reinstated quotes for cancellation
		OR T.Code ='MTAQREINS' OR T.Code ='MTAQCAN') AND Isnull(insurance_file_status_id,0)<>1
        --or insurance_file_type_id=10 or insurance_file_type_id=12) AND Isnull(insurance_file_status_id,0)<>1		
		AND insurance_file_cnt <> @insurance_file_cnt  
		AND (base_insurance_file_cnt IS NULL OR ISNULL(base_insurance_file_cnt, 0) = insurance_file_cnt)

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON 
