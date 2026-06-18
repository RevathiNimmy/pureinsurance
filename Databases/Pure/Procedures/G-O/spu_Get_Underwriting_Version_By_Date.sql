SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Underwriting_Version_By_Date'
GO


CREATE PROCEDURE spu_Get_Underwriting_Version_By_Date
	@insurance_folder_cnt int = 0,
	@insurance_file_cnt int
AS

BEGIN
	IF @insurance_folder_cnt = 0
	BEGIN
		SELECT @insurance_folder_cnt = insurance_folder_cnt FROM insurance_file (NOLOCK) WHERE insurance_file_cnt = @insurance_file_cnt    
		CREATE TABLE #tbl_MIFL
		(
			insurance_file_cnt INT, 
			original_linked_insurance_file_cnt INT,
			cancelled_linked_insurance_file_cnt INT
		)

		INSERT INTO #tbl_MIFL 
			SELECT M.insurance_file_cnt, original_linked_insurance_file_cnt, cancelled_linked_insurance_file_cnt  FROM MTA_Insurance_FIle_link M
			JOIN insurance_file I ON  M.original_linked_insurance_file_cnt = I.insurance_file_cnt
			WHERE I.insurance_folder_cnt = @insurance_folder_cnt
		UNION
			SELECT M.insurance_file_cnt, original_linked_insurance_file_cnt,cancelled_linked_insurance_file_cnt FROM MTA_Insurance_FIle_link M
			JOIN insurance_file I ON  M.cancelled_linked_insurance_file_cnt = I.insurance_file_cnt
			WHERE I.insurance_folder_cnt = @insurance_folder_cnt

		SELECT DISTINCT ifi.insurance_file_cnt,ifi.policy_version,ifs.last_trans_date,ift.code,ifi.cover_start_date,
			ifi.expiry_date,ifst.code,s.description,s.is_deleted,s.closed_allow_temp_mta,s.closed_allow_perm_mta 
		FROM insurance_file ifi (NOLOCK)
		INNER JOIN insurance_file_system ifs (NOLOCK)
			ON ifi.insurance_file_cnt = ifs.insurance_file_cnt AND ifi.policy_ignore is null 
		INNER JOIN insurance_file_type ift (NOLOCK)
			ON ifi.insurance_file_type_id = ift.insurance_file_type_id 
		INNER JOIN  source s (NOLOCK)
			ON ifi.source_id = s.source_id 
		LEFT OUTER JOIN insurance_file_status ifst (NOLOCK)
			ON ifi.insurance_file_status_id = ifst.insurance_file_status_id 
		LEFT JOIN #tbl_MIFL MIFL 
			ON MIFL.original_linked_insurance_file_cnt=ifi.insurance_file_cnt or MIFL.cancelled_linked_insurance_file_cnt=ifi.insurance_file_cnt 
		WHERE ifi.insurance_folder_cnt = @insurance_folder_cnt AND 
		(MIFL.original_linked_insurance_file_cnt IS NULL 	OR 
			   MIFL.cancelled_linked_insurance_file_cnt IS NULL OR 
			   MIFL.cancelled_linked_insurance_file_cnt =0  OR 
			   ISNULL(ifi.insurance_file_status_id,0)<>1 )
		ORDER BY ifi.cover_start_date, ifi.insurance_file_cnt, ifi.expiry_date  

		DROP TABLE #tbl_MIFL
	END
	ELSE
		SELECT ifi.insurance_file_cnt,
        ifi.policy_version,
        ifs.last_trans_date,
        ift.code,
        ifi.cover_start_date,
        ifi.expiry_date,
        ifst.code,
        s.description,
        s.is_deleted,
        s.closed_allow_temp_mta,
        s.closed_allow_perm_mta
        FROM insurance_file ifi
        INNER JOIN Insurance_file_system ifs
        ON ifi.insurance_file_cnt = ifs.insurance_file_cnt
        AND ifi.policy_ignore is null
        AND ifi.insurance_folder_cnt = @insurance_folder_cnt
        INNER JOIN insurance_file_type ift
        ON ifi.insurance_file_type_id = ift.insurance_file_type_id
        INNER JOIN source s
        ON ifi.source_id = s.source_id
        LEFT OUTER JOIN insurance_file_status ifst
        ON ifi.insurance_file_status_id = ifst.insurance_file_status_id
        WHERE ISNull(ifi.out_of_sequence_replaced, 0) <> 1 
		ORDER BY ifi.cover_start_date, ifi.insurance_file_cnt, ifi.expiry_date		
END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
