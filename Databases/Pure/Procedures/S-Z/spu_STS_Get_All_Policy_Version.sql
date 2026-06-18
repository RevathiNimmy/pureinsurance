SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_STS_Get_All_Policy_Version'
GO

CREATE PROCEDURE spu_STS_Get_All_Policy_Version
    @InsuranceFolderCnt int
AS

    SELECT ifi.insurance_folder_cnt,
           ifi.insurance_file_cnt,
           ifo.insurance_holder_cnt,
           ptp.code,
           ifi.insurance_ref,
           ift.description,
           prd.description,
           ifi.renewal_date,
           pty.shortname,
           ifi.this_premium,
           ift.code,
           ifi.insurance_file_type_id,
           ifi.cover_start_date,
           ifi.expiry_date,
           ifi.quote_expiry_date,
           evt.description,
           ifi.tax_amount,
           prd.grace_period,
           prd.code,
           ifi.policy_version,
           ifi.payment_method
      FROM Insurance_File ifi
INNER JOIN Insurance_Folder ifo ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt
INNER JOIN Party pty ON ifo.insurance_holder_cnt = pty.party_cnt
INNER JOIN Product prd ON ifi.product_id = prd.product_id
INNER JOIN Insurance_File_Type ift ON ifi.insurance_file_type_id = ift.insurance_file_type_id
 LEFT JOIN Insurance_File_Status ifs ON ifi.insurance_file_status_id = ifs.insurance_file_status_id
 LEFT JOIN Event_Log evt ON ifi.insurance_file_cnt = evt.insurance_file_cnt AND evt.event_type_id = 18
 LEFT JOIN Policy_Type ptp ON ifi.policy_type_id = ptp.policy_type_id
     WHERE ifi.insurance_folder_cnt = @InsuranceFolderCnt
       AND ifi.insurance_file_type_id <> (SELECT insurance_file_type_id FROM insurance_file_type WHERE code = 'RENEWAL')
       AND ifi.policy_ignore IS NULL
  ORDER BY ifi.insurance_file_cnt

GO
