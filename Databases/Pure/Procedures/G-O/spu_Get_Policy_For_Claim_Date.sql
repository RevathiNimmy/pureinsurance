SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Policy_For_Claim_Date'
GO

CREATE PROCEDURE spu_Get_Policy_For_Claim_Date
    @insurance_file_cnt INT
AS

SELECT  insurance_file_cnt,
        cover_start_date,
        expiry_date,
        insurance_ref,
		lapsed_reason.code
FROM    insurance_file
		LEFT JOIN lapsed_reason on lapsed_reason.lapsed_reason_id = insurance_file.lapsed_reason_id
WHERE   insurance_folder_cnt in
        (
        SELECT insurance_folder_cnt FROM insurance_file WHERE insurance_file_cnt = @insurance_file_cnt
        )
AND     insurance_file_type_id in (2,5,6,8,9,11)   -- policy, mta, temp mta and reinstated
ORDER BY cover_start_date DESC, insurance_file_cnt DESC
GO


