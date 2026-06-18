SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_get_REN_policy_details'
GO

Create procedure spu_get_REN_policy_details
@insFileCnt int
AS
BEGIN
SELECT  
        ifi.insurance_file_cnt,  
        ifo.insurance_holder_cnt,  
        ifi.product_id,  
        ifi.lead_agent_cnt,  
        ifi.Insurance_ref,  
        ifi.cover_start_date,  
        ifi.expiry_date,  
        client_name = CASE WHEN ISNULL(h.resolved_name, '') = '' THEN h.name ELSE h.resolved_name END,  
        agent_name = CASE WHEN ISNULL(a.resolved_name, '') = '' THEN a.name ELSE a.resolved_name END,  
        p.is_auto_renewable,  
        p.description Product_description,  
        irsc.description Policy_stop_reason,  
        hrsc.description Client_stop_reason,  
        ifi.is_referred_at_renewal,  
        ifi.insurance_folder_cnt,  
        ifi.renewal_date,  
        h.shortname client_code,  
        arsc.description Agent_stop_reason,  
        s.is_deleted,  
        pa.is_in_transfer_mode,  
        p.is_true_monthly_policy,  
        ifi.anniversary_copy,  
        ifi.renewal_day_number,  
        ifi.anniversary_date,  
        p.anniversary_renewal_weeks,  
        ifi.put_on_next_instalment_renewal,  
        pfIFile.insurance_file_cnt,  
        ifi.lead_allow_consolidated_commission,  
        ifi.sub_allow_consolidated_commission,  
        ifo.renewal_count,  
        ifi.renewal_product_id,  
        ifi.original_product_id  
  
        FROM Insurance_File ifi  
            JOIN Insurance_Folder ifo  
                ON ifo.insurance_folder_cnt = ifi.insurance_folder_cnt  
            JOIN Product p  
                ON  p.product_id = ifi.product_id  
            LEFT JOIN Renewal_Stop_code irsc -- insurance stop code  
                ON irsc.Renewal_stop_code_id = ifi.Renewal_stop_code_id  
            JOIN Party h -- insurance holder  
                ON h.party_cnt = ifo.insurance_holder_cnt  
            LEFT JOIN Renewal_Stop_code hrsc -- holder stop code  
                ON hrsc.Renewal_stop_code_id = h.Renewal_stop_code_id  
            LEFT JOIN Party a -- agent  
                ON a.Party_cnt = ifi.Lead_agent_cnt  
            LEFT JOIN Renewal_Stop_code arsc -- agent stop code  
                ON arsc.Renewal_stop_code_id = a.Renewal_stop_code_id  
            LEFT JOIN source s  
                ON ifi.source_id = s.source_id  
            LEFT JOIN Party_Agent pa  
                ON pa.party_cnt = a.party_cnt  
  
            LEFT JOIN (  
          SELECT TOP 1 ifile.insurance_file_cnt, ifile.insurance_folder_cnt  
          FROM insurance_file ifile  
           INNER JOIN pfpremiumfinance pf ON  
             pf.insurance_file_cnt = ifile.insurance_file_cnt  
          ORDER BY ifile.insurance_file_cnt DESC) pfIFile ON  
          ifi.insurance_folder_cnt = pfIFile.insurance_folder_cnt  
  
        WHERE ifi.insurance_file_cnt = @insFileCnt
END
GO

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS ON 
GO
