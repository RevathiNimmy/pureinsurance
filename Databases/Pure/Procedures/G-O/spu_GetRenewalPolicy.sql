SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

--
-- @RenewalType = 0 select all types of renewal
-- @RenewalType = 1 select only 'amendment'
-- @RenewalType = 2 select only 'acceptance'
-- @RenewalType = 3 select only 'notice print'
--

EXECUTE DDLDropProcedure 'spu_GetRenewalPolicy'
GO

CREATE PROCEDURE spu_GetRenewalPolicy     
 @InsuranceFileCnt int = null,     
 @InsuranceRef varchar(30) = null,     
 @RenewalDate datetime = null,     
 @ProductID int = null,     
 @BranchID int = null,     
 @RenewalType int,     
 @LeadAgentCnt int = null,     
 @Agentcode int =null,     
 @UserID int     
    
AS     
  
    
SELECT     
 rs.renewal_status_cnt,     
 p.description,     
 rs.insurance_holder_cnt,     
 pa.shortname,     
 pt.code,     
 (SELECT insurance_ref FROM Insurance_File WHERE insurance_file_cnt = rs.insurance_file_cnt),     
 rs.renewal_insurance_file_cnt,     
 ifi.insurance_ref 'Renewal_Insurance_Ref',     
 ifi.insurance_folder_cnt,     
 ifi.insurance_file_structure_id,     
 rs.renewal_status_type_id,     
 rst.description,     
 rs.critical_date,     
 rs.insurance_file_cnt,     
 ifi.cover_start_date,     
 ifi.expiry_date,     
 ifi.lead_agent_cnt,     
 p.product_id,     
 (SELECT renewal_date FROM Insurance_File WHERE insurance_file_cnt = rs.insurance_file_cnt),     
 (SELECT IsNull(Max(shortname),'Direct') FROM Party WHERE party_cnt = rs.lead_agent_cnt) LeadAgent,     
 (SELECT IsNull(Max(shortname),'Direct') FROM Party WHERE party_cnt = ifi.account_handler_cnt) AccHandler,     
 s.code 'BranchCode',     
 CASE WHEN Exists(SELECT claim_id FROM Claim WHERE policy_number = ifi.insurance_ref) THEN 'YES' ELSE 'NO' END 'Claim_Indicator',     
 ifi.source_id,     
 0 DeleteFromList,     
    s.Is_Deleted,     
    pta.is_in_transfer_mode,     
    pta.transfer_to_party_cnt,     
    xferpa.shortname,     
    (SELECT IsNull(Max(pa2.shortname),'') FROM Party pa2 JOIN Insurance_File ifi2 ON pa2.party_cnt = ifi2.lead_agent_cnt WHERE ifi2.insurance_file_cnt = rs.insurance_file_cnt) LivePolicyAgentCode,     
p.is_true_monthly_policy,     
ifi.anniversary_copy, ifi.payment_method,     
ifi.alternate_reference, --PN 33588 (RC)     
--ISNULL(ptap.shortname,'Direct') LeadAgent,   
pa.resolved_name,   
ISNULL(ptap.name,'') agent_name   
FROM Insurance_File ifi     
 JOIN Renewal_Status rs with (nolock) ON ifi.insurance_file_cnt = rs.renewal_insurance_file_cnt     
 JOIN Product p ON rs.product_id = p.product_id     
 JOIN Renewal_Status_Type rst ON rs.renewal_status_type_id = rst.renewal_status_type_id     
 JOIN Party pa ON rs.insurance_holder_cnt = pa.party_cnt     
 JOIN Party_Type pt ON pa.party_type_id = pt.party_type_id     
    JOIN Source s ON ifi.Source_ID = s.Source_ID     
    LEFT JOIN Party_Agent pta ON pta.party_cnt = rs.lead_agent_cnt     
    LEFT JOIN Party ptap ON ptap.party_cnt = rs.lead_agent_cnt     
    LEFT JOIN Party xferpa ON xferpa.party_cnt = pta.transfer_to_party_cnt     
WHERE (@InsuranceRef IS NULL OR ifi.insurance_ref LIKE @InsuranceRef)     
AND (@InsuranceFileCnt IS NULL OR ifi.insurance_file_cnt = @InsuranceFileCnt)     
AND (@RenewalDate IS NULL OR ifi.cover_start_date <= @RenewalDate)     
AND (@ProductID IS NULL OR ifi.product_id = @ProductID)     
AND ((@BranchID IS NULL AND ifi.source_id NOT IN (SELECT source_id FROM PMUser_Source WHERE user_id = @UserID)) OR ifi.source_id = @BranchID)     
AND (@LeadAgentCnt IS NULL OR (rs.lead_agent_cnt = @LeadAgentCnt AND rs.renewal_status_type_id = (SELECT renewal_status_type_id FROM Renewal_Status_Type WHERE code = 'BROKERXFER')))   
  
AND   
 (   
  (@AgentCode IS NULL OR @AgentCode =0 OR rs.lead_agent_cnt=@Agentcode)     
 OR   
  (@AgentCode = -1 AND rs.lead_agent_cnt IS NULL)     
 )   
AND (     
  (@RenewalType = 1 AND rs.renewal_status_type_id NOT IN (5,2))     
  OR (@RenewalType = 2 AND rs.renewal_status_type_id IN (5))     
  OR (@RenewalType = 3 AND rs.renewal_status_type_id = 2 AND IsNull(rs.is_invite_printed, 0) = 0)     
  OR (@RenewalType = 0)     
 )     
GO
