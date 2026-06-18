SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Renewal_PreList'
GO


CREATE PROCEDURE spu_Get_Renewal_PreList  
    @product_id int,  
    @source_id int,  
    @compare_date datetime,      -- Selection Date  
    @Start_Date datetime = null,  
    @UserId int=null,
	@AgentKey INT=0	
  
AS  
  
DECLARE @lapsed_reason_id INT  
SELECT @lapsed_reason_id = lapsed_reason_id FROM lapsed_reason WHERE UPPER(LTRIM(RTRIM((description)))) = UPPER('Non Renewable')  
SET @lapsed_reason_id = ISNULL(@lapsed_reason_id,0)  
  
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
     ifi.original_product_id,  
     p.tmpautrenfac  
  
    FROM Insurance_File ifi  
        JOIN Insurance_Folder ifo WITH(NOLOCK)  
            ON ifo.insurance_folder_cnt = ifi.insurance_folder_cnt  
        JOIN Product p WITH(NOLOCK)  
            ON  p.product_id = ifi.product_id AND p.is_renewable <> 0  
        LEFT JOIN Renewal_Stop_code irsc  WITH(NOLOCK)-- insurance stop code  
            ON irsc.Renewal_stop_code_id = ifi.Renewal_stop_code_id  
        JOIN Party h  WITH(NOLOCK)-- insurance holder  
            ON h.party_cnt = ifo.insurance_holder_cnt  
        LEFT JOIN Renewal_Stop_code hrsc WITH(NOLOCK) -- holder stop code  
            ON hrsc.Renewal_stop_code_id = h.Renewal_stop_code_id  
        LEFT JOIN Party a WITH(NOLOCK) -- agent  
            ON a.Party_cnt = ifi.Lead_agent_cnt  
        LEFT JOIN Renewal_Stop_code arsc WITH(NOLOCK) -- agent stop code  
            ON arsc.Renewal_stop_code_id = a.Renewal_stop_code_id  
        LEFT JOIN source s WITH(NOLOCK)  
            ON ifi.source_id = s.source_id  
        LEFT JOIN Party_Agent pa WITH(NOLOCK)  
            ON pa.party_cnt = a.party_cnt  
  
 LEFT JOIN (  
  SELECT TOP 1 ifile.insurance_file_cnt, ifile.insurance_folder_cnt  
  FROM insurance_file ifile WITH(NOLOCK)  
   INNER JOIN pfpremiumfinance pf WITH(NOLOCK) ON  
     pf.insurance_file_cnt = ifile.insurance_file_cnt  
  ORDER BY ifile.insurance_file_cnt DESC) pfIFile ON  
  ifi.insurance_folder_cnt = pfIFile.insurance_folder_cnt  
  
    WHERE ifi.insurance_file_status_id IS NULL  
        -- check for lapsed reason non renewable  
        AND ((ISNULL(ifi.lapsed_reason_id,0) <> @lapsed_reason_id) OR (ISNULL(ifi.lapsed_reason_id,0) = 0))  
        -- Date criteria  
        AND ((@Start_Date IS NULL AND ifi.renewal_date <= DATEADD(DAY, ISNULL(p.renewal_period, 0), @compare_date))  
            OR (@Start_Date IS NOT NULL AND ifi.renewal_date BETWEEN @Start_Date AND @compare_date))  
        -- Include valid insurance files  
        AND ifi.insurance_file_cnt IN  
                (SELECT TOP 1 ifi1.insurance_file_cnt   
                    FROM insurance_file ifi1 WITH(NOLOCK)  JOIN Insurance_File_System ifs1    
                    on ifi1.insurance_file_cnt=ifs1.insurance_file_cnt    
                    WHERE insurance_file_type_id IN (2, 5, 9)  and ISNULL(last_trans_type_id,0) not in (21,22,23) 
     AND insurance_folder_cnt = ifi.insurance_folder_cnt  ORDER BY ifi1.renewal_date DESC,ifi1.insurance_file_cnt DESC)  
            -- Exclude those in renewal  
        AND ifi.insurance_folder_cnt NOT IN (SELECT ifi2.insurance_folder_cnt FROM renewal_status RS
												INNER JOIN Insurance_File ifi2 ON RS.insurance_file_cnt=ifi2.insurance_file_cnt
												WHERE ifi2.insurance_folder_cnt=ifi.insurance_folder_cnt) 
     AND (ifi.source_id NOT IN(SELECT source_id FROM pmuser_source WITH(NOLOCK) WHERE user_id=@userid) OR @userid IS NULL)  
        AND (ifi.product_id = @product_id OR ISNULL(@product_id, 0) = 0)  
        AND (ifi.source_id = @source_id OR ISNULL(@source_id, 0) = 0)  
	AND (p.is_renewable = 1)
	AND (ifi.lead_agent_cnt=@AgentKey OR @AgentKey=0)

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
