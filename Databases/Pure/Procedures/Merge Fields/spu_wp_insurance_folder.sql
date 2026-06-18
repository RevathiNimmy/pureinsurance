SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_insurance_folder'
GO


CREATE PROCEDURE spu_wp_insurance_folder
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @code VARCHAR(40) OUTPUT,
    @description VARCHAR(255) OUTPUT,
    @inception_date DATETIME OUTPUT,
    @quote_insurance_ref VARCHAR(255) OUTPUT,
    @next_insurance_ref VARCHAR(255) OUTPUT,
    @last_insurance_ref VARCHAR(255) OUTPUT,
    @renewal_count INT OUTPUT
AS


SELECT  @code = 
        CASE (SELECT count(source_id) 
	    FROM source 
	    WHERE source_id = i.source_id 
	    AND source_id = i.source_id
	    AND underwriting_branch_ind =1 
	    AND i.alternate_reference IS NOT NULL)
	WHEN 0 THEN
	    i.insurance_ref
	ELSE
	    i.alternate_reference
        END,
        @description = fo.description,
        @inception_date = fo.inception_date,
        @quote_insurance_ref = fo.quote_insurance_ref,
        @next_insurance_ref = fo.next_insurance_ref,
        @last_insurance_ref = fo.last_insurance_ref,
        @renewal_count = fo.renewal_count
    FROM    insurance_file i,
        insurance_folder fo
    WHERE   i.insurance_file_cnt = @InsuranceFileCnt
    AND fo.insurance_folder_cnt = i.insurance_folder_cnt
GO


