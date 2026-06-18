
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Is_Subsequent_Risk_Versions_Edited'
GO

CREATE PROCEDURE spu_Is_Subsequent_Risk_Versions_Edited  
    @riskid INT,
	@mta_effective_date DATETIME
	  
AS  

BEGIN

DECLARE @risk_folder_cnt INT
		

	SELECT @risk_folder_cnt=risk_folder_cnt 
	FROM risk 
	WHERE risk_cnt= @riskid

	SELECT SUM(r.total_this_premium) 
	FROM risk r
	JOIN insurance_file_risk_link ifrl
		ON ifrl.risk_cnt=r.risk_cnt
	JOIN insurance_file ifi
		ON ifrl.insurance_file_cnt=ifi.insurance_file_cnt
	WHERE ifi.cover_start_date> @mta_effective_date
	AND r.risk_folder_cnt=@risk_folder_cnt
	AND r.risk_status_id<>4
	GROUP BY r.risk_cnt
	HAVING SUM(total_this_premium)<>0

END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
