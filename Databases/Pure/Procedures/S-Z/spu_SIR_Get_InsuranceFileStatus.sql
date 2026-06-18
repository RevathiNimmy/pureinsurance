SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_InsuranceFileStatus'
GO

CREATE PROCEDURE spu_SIR_Get_InsuranceFileStatus
  
@insurance_file_cnt INT  
  
AS  

	SELECT  INF.insurance_file_status_id, ISNULL(INFS.description,'')
	FROM    insurance_file INF LEFT JOIN Insurance_file_status INFS 
		ON INF.insurance_file_status_id = INFS.insurance_file_status_id
	WHERE   INF.insurance_file_cnt = @insurance_file_cnt

GO