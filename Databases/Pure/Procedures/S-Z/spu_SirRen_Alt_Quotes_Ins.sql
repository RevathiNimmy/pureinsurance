SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Alt_Quotes_Ins'
GO

CREATE PROCEDURE spu_SirRen_Alt_Quotes_Ins    
AS

-- MOTOR --

--quote1
UPDATE #PreOrderedResults
SET quote1 = (
	SELECT TOP 1 qqr.premium
	FROM renewal_control rc	
	INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
        INNER JOIN quote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
	INNER JOIN giimquick_quote_result qqr ON (qqr.quote_binder_id = qb.quote_binder_id)
        WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
	AND qb.preferred_ind = 1
	ORDER BY qqr.premium)
WHERE data_model_code = 'GIIMotor'

--excess1
UPDATE #PreOrderedResults
SET excess1 = (
	SELECT TOP 1 qqr.total_excess
	FROM renewal_control rc	
	INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
	INNER JOIN quote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
	INNER JOIN giimquick_quote_result qqr ON (qqr.quote_binder_id = qb.quote_binder_id)
	WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
	AND qb.preferred_ind = 1
	ORDER BY qqr.premium)
WHERE data_model_code = 'GIIMotor'

--quote2
UPDATE #PreOrderedResults
SET quote2 = (
	SELECT TOP 1 qqr.premium
	FROM renewal_control rc	
	INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
	INNER JOIN quote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
	INNER JOIN giimquick_quote_result qqr ON (qqr.quote_binder_id = qb.quote_binder_id)
	WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
	AND qb.preferred_ind = 1
	AND qb.quote_binder_id NOT IN
	(
	SELECT TOP 1 qb.quote_binder_id
		FROM renewal_control rc	
		INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
		INNER JOIN quote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
		INNER JOIN giimquick_quote_result qqr ON (qqr.quote_binder_id = qb.quote_binder_id)
		WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
		AND qb.preferred_ind = 1
	        ORDER BY qqr.premium
	)
	ORDER BY qqr.premium)
WHERE data_model_code = 'GIIMotor'

--excess2
UPDATE #PreOrderedResults
SET excess2 = (
	SELECT TOP 1 qqr.total_excess
	FROM renewal_control rc	
	INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
	INNER JOIN quote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
	INNER JOIN giimquick_quote_result qqr ON (qqr.quote_binder_id = qb.quote_binder_id)
	WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
	AND qb.preferred_ind = 1
	AND qb.quote_binder_id NOT IN
	(
	SELECT TOP 1 qb.quote_binder_id
		FROM renewal_control rc	
		INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
		INNER JOIN quote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
		INNER JOIN giimquick_quote_result qqr ON (qqr.quote_binder_id = qb.quote_binder_id)
		WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
		AND qb.preferred_ind = 1
		ORDER BY qqr.premium
	)
	ORDER BY qqr.premium)
WHERE data_model_code = 'GIIMotor'

--quote3
UPDATE #PreOrderedResults
SET quote3 = (
	SELECT TOP 1 qqr.premium
	FROM renewal_control rc	
	INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
	INNER JOIN quote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
	INNER JOIN giimquick_quote_result qqr ON (qqr.quote_binder_id = qb.quote_binder_id)
	WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
	AND qb.preferred_ind = 1
	AND qb.quote_binder_id NOT IN
	(
	SELECT TOP 2 qb.quote_binder_id
		FROM renewal_control rc	
		INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
		INNER JOIN quote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
		INNER JOIN giimquick_quote_result qqr ON (qqr.quote_binder_id = qb.quote_binder_id)
		WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
		AND qb.preferred_ind = 1
	        ORDER BY qqr.premium
	)
	ORDER BY qqr.premium)
WHERE data_model_code = 'GIIMotor'

--excess3
UPDATE #PreOrderedResults
SET excess3 = (
	SELECT TOP 1 qqr.total_excess
	FROM renewal_control rc	
	INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
	INNER JOIN quote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
	INNER JOIN giimquick_quote_result qqr ON (qqr.quote_binder_id = qb.quote_binder_id)
	WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
	AND qb.preferred_ind = 1
	AND qb.quote_binder_id NOT IN
	(
	SELECT TOP 2 qb.quote_binder_id
		FROM renewal_control rc	
		INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
		INNER JOIN quote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
		INNER JOIN giimquick_quote_result qqr ON (qqr.quote_binder_id = qb.quote_binder_id)
		WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
		AND qb.preferred_ind = 1
		ORDER BY qqr.premium
	)
	ORDER BY qqr.premium)
WHERE data_model_code = 'GIIMotor'


-- HOUSEHOLD --

--quote1
UPDATE #PreOrderedResults
SET quote1 = (
	SELECT TOP 1 qqr.out_total_ann_premium
	FROM renewal_control rc	
	INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
        INNER JOIN giihquote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
	INNER JOIN qh_quote_out qqr ON (qqr.giihquote_binder_id = qb.giihquote_binder_id)
        WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
	AND qb.preferred_ind = 1
	AND (qqr.out_buildings_flag is null AND qqr.out_contents_flag is null)
	ORDER BY qqr.out_total_ann_premium)
WHERE data_model_code = 'GIIHouse'

--excess1
UPDATE #PreOrderedResults
SET excess1 = (
	SELECT TOP 1 qqr.out_buildings_excess
	FROM renewal_control rc	
	INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
	INNER JOIN giihquote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
	INNER JOIN qh_quote_out qqr ON (qqr.giihquote_binder_id = qb.giihquote_binder_id)
	WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
	AND qb.preferred_ind = 1
	AND (qqr.out_buildings_flag is null AND qqr.out_contents_flag is null)
	ORDER BY qqr.out_total_ann_premium)
WHERE data_model_code = 'GIIHouse'

--quote2
UPDATE #PreOrderedResults
SET quote2 = (
	SELECT TOP 1 qqr.out_total_ann_premium
	FROM renewal_control rc	
	INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
	INNER JOIN giihquote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
	INNER JOIN qh_quote_out qqr ON (qqr.giihquote_binder_id = qb.giihquote_binder_id)
	WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
	AND qb.preferred_ind = 1
	AND (qqr.out_buildings_flag is null AND qqr.out_contents_flag is null)
	AND qb.giihquote_binder_id NOT IN
	(
	SELECT TOP 1 qb.giihquote_binder_id
		FROM renewal_control rc	
		INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
		INNER JOIN giihquote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
		INNER JOIN qh_quote_out qqr ON (qqr.giihquote_binder_id = qb.giihquote_binder_id)
		WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
		AND qb.preferred_ind = 1
		AND (qqr.out_buildings_flag is null AND qqr.out_contents_flag is null)
	        ORDER BY qqr.out_total_ann_premium
	)
	ORDER BY qqr.out_total_ann_premium)
WHERE data_model_code = 'GIIHouse'

--excess2
UPDATE #PreOrderedResults
SET excess2 = (
	SELECT TOP 1 qqr.out_buildings_excess
	FROM renewal_control rc	
	INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
	INNER JOIN giihquote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
	INNER JOIN qh_quote_out qqr ON (qqr.giihquote_binder_id = qb.giihquote_binder_id)
	WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
	AND qb.preferred_ind = 1
	AND (qqr.out_buildings_flag is null AND qqr.out_contents_flag is null)
	AND qb.giihquote_binder_id NOT IN
	(
	SELECT TOP 1 qb.giihquote_binder_id
		FROM renewal_control rc	
		INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
		INNER JOIN giihquote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
		INNER JOIN qh_quote_out qqr ON (qqr.giihquote_binder_id = qb.giihquote_binder_id)
		WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
		AND qb.preferred_ind = 1
		AND (qqr.out_buildings_flag is null AND qqr.out_contents_flag is null)
		ORDER BY qqr.out_total_ann_premium
	)
	ORDER BY qqr.out_total_ann_premium)
WHERE data_model_code = 'GIIHouse'


--quote3
UPDATE #PreOrderedResults
SET quote3 = (
	SELECT TOP 1 qqr.out_total_ann_premium
	FROM renewal_control rc	
	INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
	INNER JOIN giihquote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
	INNER JOIN qh_quote_out qqr ON (qqr.giihquote_binder_id = qb.giihquote_binder_id)
	WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
	AND qb.preferred_ind = 1
	AND (qqr.out_buildings_flag is null AND qqr.out_contents_flag is null)
	AND qb.giihquote_binder_id NOT IN
	(
	SELECT TOP 2 qb.giihquote_binder_id
		FROM renewal_control rc	
		INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
		INNER JOIN giihquote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
		INNER JOIN qh_quote_out qqr ON (qqr.giihquote_binder_id = qb.giihquote_binder_id)
		WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
		AND qb.preferred_ind = 1
		AND (qqr.out_buildings_flag is null AND qqr.out_contents_flag is null)
	        ORDER BY qqr.out_total_ann_premium
	)
	ORDER BY qqr.out_total_ann_premium)
WHERE data_model_code = 'GIIHouse'

--excess3
UPDATE #PreOrderedResults
SET excess3 = (
	SELECT TOP 1 qqr.out_buildings_excess
	FROM renewal_control rc	
	INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
	INNER JOIN giihquote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
	INNER JOIN qh_quote_out qqr ON (qqr.giihquote_binder_id = qb.giihquote_binder_id)
	WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
	AND qb.preferred_ind = 1
	AND (qqr.out_buildings_flag is null AND qqr.out_contents_flag is null)
	AND qb.giihquote_binder_id NOT IN
	(
	SELECT TOP 2 qb.giihquote_binder_id
		FROM renewal_control rc	
		INNER JOIN gis_policy_link gpl ON (gpl.insurance_file_cnt = rc.renewal_insurance_file_cnt)
		INNER JOIN giihquote_binder qb ON (qb.gis_policy_link_id = gpl.gis_policy_link_id)
		INNER JOIN qh_quote_out qqr ON (qqr.giihquote_binder_id = qb.giihquote_binder_id)
		WHERE rc.insurance_folder_cnt = #PreOrderedResults.Insurance_folder_cnt
		AND qb.preferred_ind = 1
		AND (qqr.out_buildings_flag is null AND qqr.out_contents_flag is null)
		ORDER BY qqr.out_total_ann_premium
	)
	ORDER BY qqr.out_total_ann_premium)
WHERE data_model_code = 'GIIHouse'