EXECUTE DDLDropProcedure 'spu_SIR_MTA_Link_Sel'
GO

CREATE PROCEDURE spu_SIR_MTA_Link_Sel
    @InsuranceFileCnt int
AS

CREATE TABLE #TempPolicyRef
        (
        insurance_file_cnt INT NULL,
        original_linked_insurance_file_cnt INT NULL,
        new_linked_insurance_file_cnt INT NULL,
        type_ind SMALLINT NULL,
        original_insurance_file_status_id INT NULL
        )

INSERT INTO #TempPolicyRef
	SELECT insurance_file_cnt,
	       original_linked_insurance_file_cnt,
	       new_linked_insurance_file_cnt,
	       type_ind,
	       original_insurance_file_status_id
	FROM   mta_insurance_file_link
	WHERE  insurance_file_cnt = @InsuranceFileCnt
	AND    new_linked_insurance_file_cnt IS NOT NULL
	AND    type_ind > 0


INSERT INTO #TempPolicyRef 
	SELECT insurance_file_cnt,
	       original_linked_insurance_file_cnt,
	       cancelled_linked_insurance_file_cnt,
	       type_ind,
	       original_insurance_file_status_id
	FROM   mta_insurance_file_link
	WHERE  insurance_file_cnt = @InsuranceFileCnt
	AND    cancelled_linked_insurance_file_cnt IS NOT NULL
        AND    type_ind > 0
        
SELECT t.original_linked_insurance_file_cnt,
       t.new_linked_insurance_file_cnt,
       t.type_ind, 
       i.cover_start_date,
       i.policy_version,
       i.insurance_file_type_id,
       i.expiry_date,
       t.original_insurance_file_status_id
FROM   insurance_file i,
       #TempPolicyRef t
WHERE t.original_linked_insurance_file_cnt = i.insurance_file_cnt
ORDER BY t.new_linked_insurance_file_cnt ASC

DROP TABLE #TempPolicyRef