SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_Get_All_Non_Temp_Policy_Version
GO

CREATE PROCEDURE spu_Get_All_Non_Temp_Policy_Version
    @InsuranceFolderCnt INT,  
    @InsuranceFileCnt INT  
AS  
/******************************************************************************************************  
* Name : sp_Get_All_Policy_Version  
*  
* Desc : get all versions of policy  
*  
* Hist : 26/02/2001 Created - Tinny  
*  
* Ver  : 1.00.0000  
*  
* Note : either insurance_folder_cnt or insurance_file_cnt is present for this to work  
*  
* JMK 12/05/2001    Add Cover start and stop dates  
*                   Filter out Cancelled MTAs  
* SJ  13/09/2002    Do not show records under renewal  
* KR  27/01/2003    Quote Management (117) changes  
* SET 04/02/2003    Amendments cos table udl_mtar has been renamed MTA_Reason  
*                   & added to pmlookup maintenance.  
* JAS 11/03/2003    ISS1497 Only select Non-Temporary Policy Versions  
*      This SP based on spu_Get_All_Policy_Version  
* DC  06/05/2003 PN11785 cater for What If Renewals (Type 11) when displaying status description  
*******************************************************************************************************/  
BEGIN

    DECLARE @underwriting_flag VARCHAR(20)  
    SELECT @underwriting_flag = value 
    FROM   hidden_options 
    WHERE  branch_id = 1 AND option_number = 1  
    IF @InsuranceFolderCnt = 0  
    BEGIN  
        SELECT @InsuranceFolderCnt = insurance_folder_cnt 
        FROM   Insurance_File  
        WHERE  insurance_file_cnt = @InsuranceFileCnt  
    END  
    SELECT  ifi.insurance_folder_cnt,  
            ifi.insurance_file_cnt,  
            ifo.insurance_holder_cnt,  
            ifi.policy_type_id,  
            CASE (SELECT  count(source_id)  
                  FROM    source  
                  WHERE   source_id = ifi.source_id  
                      AND underwriting_branch_ind =1  
                      AND ifi.alternate_reference IS NOT NULL)  
                 WHEN 0 THEN  
                     ifi.insurance_ref  
                 ELSE  
                     ifi.alternate_reference  
            END,  
            ift.description,  
            --prd.description, --Removed MKW 030303 PN2276  
            rc.description, --Added MKW 030303 PN2276  
            ifi.renewal_date,  
            CASE pty.resolved_name  
                WHEN '' THEN pty.name  
                WHEN null THEN pty.name  
                ELSE pty.resolved_name  
            END,  
            pty.shortname,  
            ifi.this_premium,  
            --added mkw 040303 PN1413  
            CASE  
                WHEN @underwriting_flag = 'U' THEN ifs.description  
                WHEN (ifs.description = '' OR (ifs.description IS NULL)) 
                    AND (ifi.insurance_file_type_id <> 2 
                    AND ifi.insurance_file_type_id <> 5 
                    AND ifi.insurance_file_type_id <> 6 
                    AND ifi.insurance_file_type_id <> 11)  
                    THEN  'Incomplete'  
            ELSE  ifs.description  
            END AS description,  
            --end mkw 040303 PN1413  
            ifi.insurance_file_type_id,  
            ifi.cover_start_date,  
            ifi.expiry_date,  
            um.description  
    FROM    Insurance_File ifi
        INNER JOIN Insurance_Folder ifo
            ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt  
        INNER JOIN Party pty
            ON ifo.insurance_holder_cnt = pty.party_cnt
        INNER JOIN Product prd
            ON ifi.product_id = prd.product_id
        INNER JOIN Insurance_File_Type ift
            ON ifi.insurance_file_type_id = ift.insurance_file_type_id
        INNER JOIN Risk_Code rc --MKW 030303 PN2276  
            ON ifi.Risk_Code_ID = rc.Risk_Code_ID  --Added MKW030303 PN2276  
        LEFT OUTER JOIN Insurance_File_Status ifs    
            ON ifi.insurance_file_status_id = ifs.insurance_file_status_id
        LEFT OUTER JOIN MTA_Reason um -- SET 04/02/2003 table now called MTA_Reason      
            ON ifi.user_defined_data_id = um.MTA_Reason_id -- SET 04/02/2003 field now called MTA_Reason_id  
        --        udl_mtar um,  
    WHERE   ifi.insurance_folder_cnt = @InsuranceFolderCnt  
        AND (ifi.insurance_file_type_id <> (
                                            SELECT insurance_file_type_id 
                                            FROM insurance_file_type where code = 'RENEWAL'
                                            ) 
            OR  @underwriting_flag = 'U'
            )  
        --AND     ifi.user_defined_data_id *= um.udl_mtar_id  
        AND ifi.policy_ignore is null  
        AND ift.insurance_file_type_id not in (6,7) --Added JAS 11/03/2003  
    ORDER BY insurance_file_cnt  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO