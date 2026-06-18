SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_wp_insurance_file_system
GO

CREATE PROCEDURE spu_wp_insurance_file_system  
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @ClaimCnt INT,  
    @endorsement_count INT OUTPUT,  
    @created_by VARCHAR(100) OUTPUT,  
    @date_created DATETIME OUTPUT,  
    @modified_by_id VARCHAR(100) OUTPUT,  
    @last_modified DATETIME OUTPUT,  
    @last_trans_date DATETIME OUTPUT,  
    @last_trans_type VARCHAR(255) OUTPUT,  
    @last_trans_description VARCHAR(255) OUTPUT,  
    @last_trans_debit_credit VARCHAR(1) OUTPUT,  
    @last_trans_document_ref VARCHAR(25) OUTPUT,  
    @last_trans_cover_start_date DATETIME OUTPUT,  
    @last_trans_expiry_date DATETIME OUTPUT  
AS  
BEGIN  
  
    SELECT  
        @endorsement_count = ifs.endorsement_count,  
        @created_by = p1.username,  
        @date_created = ifs.date_created,  
        @modified_by_id = p2.username,  
        @last_modified = ifs.last_modified,  
        @last_trans_date = ifs.last_trans_date,  
        @last_trans_type = tp.description,  
        @last_trans_description = ifs.last_trans_description,  
        @last_trans_debit_credit = ifs.last_trans_debit_credit,  
        @last_trans_document_ref = ifs.last_trans_document_ref,  
        @last_trans_cover_start_date = ifs.last_trans_cover_start_date,  
        @last_trans_expiry_date = ifs.last_trans_expiry_date  
    FROM  
        insurance_file_system ifs  
        LEFT OUTER JOIN PMUser p1  
            ON ifs.created_by_id = p1.user_id  
        LEFT OUTER JOIN PMUser p2  
            ON ifs.modified_by_id = p2.user_id  
        LEFT OUTER JOIN Transaction_Type tp  
            ON ifs.last_trans_type_id = tp.transaction_type_id
    WHERE  
        ifs.insurance_file_cnt = @InsuranceFileCnt  
  
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
