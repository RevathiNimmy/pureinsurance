SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Claim_Status'
GO

CREATE PROCEDURE spu_CLM_Get_Claim_Status
@Claim_id INT
AS
SELECT UPPER(clmst.code) FROM claim_status clmst
				  INNER JOIN claim clm ON
                  clmst.claim_status_id=clm.claim_status_id 
				  and clm.claim_id=@Claim_id	 	