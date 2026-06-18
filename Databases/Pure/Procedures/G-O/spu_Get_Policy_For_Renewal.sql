SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Policy_For_Renewal'
GO


CREATE PROCEDURE spu_Get_Policy_For_Renewal    
   @insurance_folder_cnt INT    
AS    
    
/*********************************************************************************************    
* Name : spu_Get_Policy_For_Renewal    
* Desc : get correnct version of policy for renewal    
* Hist : 13/02/2001 Created - Tinny    
* Ver  : 1.00.0000    
* 1.1   Retrieved lapsed versions of policies as well    
*   as live.                        RWH 26/05/01    
*    
* 21/02/2003 Thinh Nguyen - get client code and agent stop code    
*                           (make sure its the same as spu_get_renewal_prelist)    
**********************************************************************************************/    
DECLARE @insurance_file_cnt INT
DECLARE @anniversary_copy INT  
  
-- Get the correct insurance file based on the active folder and those currently in renewal  
SELECT  TOP 1 @insurance_file_cnt = ifi.insurance_file_cnt     
FROM    insurance_file  ifi JOIN Insurance_File_System ifs    
ON ifi.insurance_file_cnt=ifs.insurance_file_cnt    
WHERE   insurance_folder_cnt = @insurance_folder_cnt    
AND     insurance_file_type_id IN (SELECT insurance_file_type_id FROM insurance_file_type WHERE code IN ('POLICY', 'MTA PERM', 'MTAREINS'))    
AND     (insurance_file_status_id IS NULL OR insurance_file_status_id IN (3,2,5,6,309,4))    
AND     ifi.insurance_file_cnt NOT IN (SELECT renewal_insurance_file_cnt FROM renewal_status WHERE renewal_insurance_file_cnt IS NOT NULL)    
ORDER BY ifi.inception_date_tpi DESC,ifi.insurance_file_cnt DESC  
  

DECLARE @lapsed_reason_id INT
SELECT @lapsed_reason_id = lapsed_reason_id FROM lapsed_reason WHERE UPPER(LTRIM(RTRIM((description)))) = UPPER('Non Renewable')

SELECT @anniversary_copy=0
IF   NOT EXISTS (SELECT NULL FROM Renewal_Status RS JOIN insurance_file ifi ON rs.renewal_insurance_file_cnt=ifi.insurance_file_cnt 
 WHERE RS.insurance_file_cnt= @insurance_file_cnt  AND anniversary_copy=1) 
 BEGIN
    SELECT @anniversary_copy= 1
 END
  
-- Now get the rest of the data and validate the chosen insurance file (couple of extra checks  
SELECT  IFile.insurance_file_cnt,  
        IFolder.insurance_holder_cnt,  
        IFile.product_id,  
        IFile.lead_agent_cnt,  
        IFile.Insurance_ref,  
        IFile.cover_start_date,  
        IFile.expiry_date,  
        client_name = CASE pt.resolved_name  
                        WHEN NULL THEN pt.name  
                        WHEN '' THEN pt.name  
                        ELSE pt.resolved_name END,  
        agent_name = CASE pt2.resolved_name  
                        WHEN NULL THEN pt2.name  
                        WHEN '' THEN pt2.name  
                        ELSE pt2.resolved_name END,  
        Prod.is_auto_renewable,  
        Prod.description Product_description,  
        RSC1.description Policy_stop_reason,  
        RSC2.description Client_stop_reason,  
        IFile.is_referred_at_renewal,  
        IFile.insurance_folder_cnt,  
        IFile.renewal_date,  
        pt.shortname client_code,  
        RSC3.description Agent_stop_reason,  
        s.is_deleted,  
        PA.is_in_transfer_mode,  
 prod.is_true_monthly_policy,  
 ifile.anniversary_copy,  
 ifile.renewal_day_number,  
 ifile.anniversary_date,  
 prod.anniversary_renewal_weeks,  
 ifile.put_on_next_instalment_renewal,  
 pfIFile.insurance_file_cnt,  
 iFile.lead_allow_consolidated_commission,  
 iFile.sub_allow_consolidated_commission,  
 iFolder.renewal_count,
 iFile.renewal_product_id,
 iFile.original_product_id,
 prod.tmpautrenfac,
 Prod.is_renewable,
  IFile.alternate_reference      
  
FROM  Insurance_File IFile  
  
        JOIN Product Prod ON IFile.product_id = Prod.product_id  
        JOIN Insurance_Folder IFolder ON IFile.insurance_folder_cnt = IFolder.insurance_folder_cnt  
        LEFT JOIN Party Pt ON IFolder.insurance_holder_cnt = pt.party_cnt  
        LEFT JOIN Party Pt2 ON IFile.lead_agent_cnt = Pt2.party_cnt  
        LEFT JOIN Renewal_stop_code RSC1 ON IFile.renewal_stop_code_id = RSC1.renewal_stop_code_id  
        LEFT JOIN Renewal_stop_code RSC2 ON Pt.renewal_stop_code_id = RSC2.renewal_stop_code_id  
        LEFT JOIN Renewal_stop_code RSC3 ON Pt2.renewal_stop_code_id = RSC3.renewal_stop_code_id  
        LEFT JOIN source s ON IFile.source_id = s.source_id  
        LEFT JOIN Party_Agent PA ON PA.party_cnt=PT2.party_cnt  
  
 LEFT JOIN (  
SELECT TOP 1 ifile.insurance_file_cnt, ifile.insurance_folder_cnt  
FROM insurance_file ifile  
INNER JOIN pfpremiumfinance pf ON  
 pf.insurance_file_cnt = ifile.insurance_file_cnt  
 WHERE ifile.insurance_folder_cnt = @insurance_folder_cnt
ORDER BY ifile.inception_date_tpi DESC,ifile.insurance_file_cnt DESC) pfIFile ON  
 ifile.insurance_folder_cnt = pfIFile.insurance_folder_cnt  
  
 
WHERE   IFile.insurance_file_cnt = @insurance_file_cnt  
-- check for lapsed reason non renewable 
AND (ISNULL(IFile.lapsed_reason_id,0) <> @lapsed_reason_id  OR ISNULL(@lapsed_reason_id,0) = 0)
AND   ((IFile.insurance_file_status_id IS NULL)  
OR (IFile.insurance_file_status_id IN  (SELECT insurance_file_status_id FROM Insurance_File_Status WHERE code IN ('LAP','REPBDMTA','REP'))))   
AND     ((ifile.insurance_file_cnt NOT IN (SELECT insurance_file_cnt FROM renewal_status) AND @anniversary_copy=1) OR @anniversary_copy=0)
  
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
