EXECUTE DDLDropProcedure 'spu_RI_Arrangement_saa'
GO

CREATE PROCEDURE spu_RI_Arrangement_saa          
    @risk_cnt int,
    @version_id int = NULL
AS

    DECLARE @RI2007Enabled INT
	SELECT @RI2007Enabled=ISNull(value,0) FROM hidden_options WHERE option_number=88

	IF @version_id IS NULL
		SELECT @version_id = max(version_id) from ri_arrangement Where risk_cnt = @risk_cnt

	IF (@RI2007Enabled=1)
	BEGIN
		SELECT  ra.ri_arrangement_id,
				ra.ri_band_id,
				ra.ri_model_id,
				ra.sum_insured,
				ra.premium,
				ra.original_flag,
				ra.is_modified,
				ISNULL(rm.fac_premium_type, 0) AS fac_premium_type,
				rm.code ,
				ra.extended_limit_amount,
				ra.is_extended_limit_applied,
				ISNULL(ra.xol_ri_model_id,0) AS xol_ri_model_id,
				rm2.code AS rmcode,
				ISNULL(ri_override_reason_id,0) AS rioverridereasonid   
		FROM    ri_arrangement ra
		LEFT JOIN
				ri_model rm
				On rm.ri_model_id = ra.ri_model_id
		LEFT JOIN
				ri_model rm2
				On rm2.ri_model_id = ra.xol_ri_model_id

		WHERE   ra.risk_cnt = @risk_cnt
		AND     ra.ri_band_id Is Not Null
		AND		((version_id = @version_id AND  @RI2007Enabled=1 ) OR @version_id IS NULL)
		ORDER BY
				ra.ri_band_id,
				ra.original_flag
		END
	ELSE
	BEGIN
		Select  ra.ri_arrangement_id,
				ra.ri_band_id,
				ra.ri_model_id,
				ra.sum_insured,
				ra.premium,
				ra.original_flag,
				ra.is_modified,
				IsNull(rm.fac_premium_type, 0)AS fac_premium_type,
				'' AS code,
				0 AS extended_limit_amount,
				NULL AS is_extended_limit_applied,
				0 AS xol_ri_model_id,
				'' AS rmcode,
				 ISNULL(ri_override_reason_id,0) AS rioverridereasonid   
		From    ri_arrangement ra (nolock)
		Left Join
				ri_model rm (nolock)
				On rm.ri_model_id = ra.ri_model_id
		Where   ra.risk_cnt = @risk_cnt
				And     ra.ri_band_id Is Not Null
				AND  ((version_id = @version_id ) OR @version_id IS NULL) 
		Order By
				ra.ri_band_id,
				ra.original_flag
	END