SET QUOTED_IDENTIFIER OFF
Go
SET ANSI_NULLS OFF  
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_Mandatory_Risk'
GO

CREATE PROCEDURE spu_SIR_Get_Mandatory_Risk
    @risk_cnt INT,
    @insurance_file_cnt INT
AS
BEGIN
   --EM: 20120806 -Changed Sub Query to JOIN and added risk link status 
	SELECT	Risk.risk_cnt,
			Risk.[description],
			Risk.variation_number,
			insurance_file_risk_link.status_flag 
	FROM Risk
	INNER JOIN insurance_file_risk_link 
		ON insurance_file_risk_link.risk_cnt = Risk.risk_cnt AND insurance_file_cnt = @insurance_file_cnt
	WHERE Risk.Is_Mandatory_Risk = 1
	AND Risk.risk_cnt <> @risk_cnt

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO