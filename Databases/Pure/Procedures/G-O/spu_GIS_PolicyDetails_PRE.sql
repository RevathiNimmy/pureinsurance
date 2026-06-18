SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_PolicyDetails_PRE'
GO

CREATE PROCEDURE [dbo].[spu_GIS_PolicyDetails_PRE] 

    @nGis_policy_link_id INT
  
AS
BEGIN
	WITH cte_ins_policy
	AS
	(
	SELECT  inf.insurance_ref,inf.insurance_file_cnt,inf.cover_start_date,inf.expiry_date AS cover_end_date,r.risk_number
	FROM	gis_policy_link gpl  (NOLOCK)
	INNER JOIN	risk r (NOLOCK) ON r.risk_cnt = gpl.risk_id
	INNER JOIN    insurance_file_risk_link ifrl  (NOLOCK) ON ifrl.risk_cnt=r.risk_cnt
	INNER JOIN	Insurance_File inf  (NOLOCK) ON inf.insurance_file_cnt=ifrl.insurance_file_cnt
	WHERE	gpl.gis_policy_link_id =  @nGis_policy_link_id
	)

	SELECT TOP 1 inf.insurance_ref,inf.insurance_file_cnt,inf.insurance_folder_cnt,inf.cover_start_date,inf.expiry_date AS cover_end_date,c.risk_number
	FROM cte_ins_policy c 
	JOIN Insurance_File inf ON c.insurance_ref=inf.insurance_ref 
	WHERE  inf.insurance_file_type_id in (1,2,3) AND inf.insurance_file_status_id IS NULL AND inf.cover_start_date<=c.cover_start_date
	ORDER BY inf.cover_start_date DESC
END
GO