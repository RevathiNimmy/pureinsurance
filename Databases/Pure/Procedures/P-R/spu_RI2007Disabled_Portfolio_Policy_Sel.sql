SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_RI2007Disabled_Portfolio_Policy_Sel'
GO


CREATE PROCEDURE spu_RI2007Disabled_Portfolio_Policy_Sel
	@product_id int,
	@transfer_date datetime
AS

    SELECT	DISTINCT
    		ifi.insurance_file_cnt,
            ifi.insurance_ref,
    		p.shortname, 
    		p.resolved_name,
            MIN(CASE WHEN u2.effective_date > ifi.cover_start_date 
                THEN u2.effective_date 
                ELSE ifi.cover_start_date 
            END) effective_date,
			ifi.Insurance_File_Type_Id
    FROM	Insurance_File ifi
    JOIN    Insurance_File_System ifsys    ON ifi.insurance_file_cnt = ifsys.insurance_file_cnt 
    JOIN    Party p                        ON ifi.insured_cnt = p.party_cnt
    JOIN    insurance_file_risk_link l     ON ifi.insurance_file_cnt = l.insurance_file_cnt
    JOIN    risk r                         ON r.risk_cnt = l.risk_cnt
    JOIN    ri_arrangement ra              ON r.risk_cnt = ra.risk_cnt
    JOIN    risk_type_ri_model_usage u     ON u.risk_type_id = r.risk_type_id 
                                          AND u.ri_band = ra.ri_band_id
                                          AND u.ri_model_id = ra.ri_model_id
    JOIN    risk_type_ri_model_usage u2    ON u2.portfolio_transfer_from_cnt = u.risk_type_ri_model_usage_cnt
    LEFT JOIN 
            insurance_file_status ifs      ON ifi.insurance_file_status_id = ifs.insurance_file_status_id
    WHERE  (product_id = @product_id OR ISNULL(@product_id, 0) = 0)
    	AND @transfer_date BETWEEN ifi.cover_start_date AND ifi.expiry_date -- Policy is live at transfer date
        AND @transfer_date >= u2.effective_date -- New model is effective before transfer date
        AND (@transfer_date > ifi.cover_start_date 
            OR (@transfer_date = ifi.cover_start_date AND isnull(ifsys.last_trans_type_id, 0) <> 22)) --PM022219 and PM026804
    	 --PM022219 and PM026804 : For policy versions where after PT, RI model is unchanged on PT version , even though it should not come again in PT.     
         -- Or in other words, do not process policy version which has already processed by PT
    	AND ISNULL(ifs.code, '') <> 'REPPT' -- We don't want to re-select the already replaced one
    	AND ra.original_flag = 0 -- "live" RI model
    	AND u.is_deleted = 0
    	AND u2.is_deleted = 0
    	AND ifi.insurance_file_type_id in (2, 5, 9, 8, 3, 6) -- only select live policies and permanent MTAs or reinstatements
		
    GROUP BY 
    		ifi.insurance_file_cnt,
            ifi.insurance_ref,
    		p.shortname, 
    		p.resolved_name,
			ifi.Insurance_File_Type_Id
                --PM026804 This is reverted since can not be a condition : having sum(abs(r.total_this_premium))<>0
    ORDER BY
            ifi.insurance_file_cnt

GO