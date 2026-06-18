SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_check_ri_on_cloned'
GO


CREATE PROCEDURE spu_check_ri_on_cloned
    @nInsurance_file_cnt integer 
AS
SELECT Null FROM RI_arrangement 
	WHERE cloned=1 and original_flag =0
	AND version_id=2 
	AND risk_cnt in (SELECT risk_cnt 
				FROM insurance_file_risk_link 
					WHERE insurance_file_cnt =@nInsurance_file_cnt) 


GO
