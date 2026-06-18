EXECUTE DDLDropProcedure 'spu_Risks_Cloned_RI_Status_Sel_amend'
GO

CREATE PROCEDURE spu_Risks_Cloned_RI_Status_Sel_amend
@sPolicy_number as varchar(30) = null ,  
@insurance_file_cnt int =0,
@AgentKey INT=0 
AS  
IF @insurance_file_cnt>0  
SELECT @sPolicy_number = LTRIM(RTRIM(insurance_ref)) FROM insurance_file WHERE insurance_file_cnt = @insurance_file_cnt 
  
-- get the records that are marked as 'manual review'
If ISNULL(@sPolicy_number,'')=''  
SELECT  
    ifd.ins_file_Cloned_RI_usage_id  
, ifd.insurance_file_cnt  
,   ins.insurance_ref AS 'Policy Number'  
,   src.source_id  
,   src.[description] AS 'Branch Desc'  
, ifd.status  
, Case ISNULL(ifd.status ,0)  
 WHEN 1 THEN 'Awaiting Amend'  
 ELSE 'Awaiting Update'  
 END AS 'Cloned RI Status Desc'  
,   ins.insured_cnt  
,   par.shortname AS 'Party Shortname'  
,   par.[name] AS 'Party Name'  
--,   par.resolved_name AS 'Party Name'  
,   pro.product_id  
,   pro.[description] AS 'Product Desc'  
,   ins.insurance_folder_cnt  
,   ifd.new_insurance_file_cnt
,  src.code AS 'BranchCode'
,pro.code AS 'ProductCode'
FROM  
 Insurance_File_Cloned_RI_Usage AS ifd  
INNER JOIN  
    Insurance_File AS ins ON ins.insurance_file_cnt = ifd.insurance_file_cnt  
INNER JOIN  
    Source AS src ON src.source_id = ins.source_id  
INNER JOIN  
    Party AS par ON par.party_cnt = ins.insured_cnt  
INNER JOIN  
    Product AS pro ON pro.product_id = ins.product_id 
WHERE (ins.Lead_agent_cnt=@AgentKey OR @AgentKey=0)
ORDER BY  
    src.description ASC,  ins.insurance_ref ASC, ifd.ins_file_Cloned_RI_usage_id ASC   
ELSE
SELECT  
    ifd.ins_file_Cloned_RI_usage_id  
, ifd.insurance_file_cnt  
,   ins.insurance_ref AS 'Policy Number'  
,   src.source_id  
,   src.[description] AS 'Branch Desc'  
, ifd.status  
, Case ISNULL(ifd.status ,0)  
 WHEN 1 THEN 'Awaiting Amend'  
 ELSE 'Awaiting Update'  
 END AS 'Cloned RI Status Desc'  
,   ins.insured_cnt  
,   par.shortname AS 'Party Shortname'  
,   par.[name] AS 'Party Name'  
--,   par.resolved_name AS 'Party Name'  
,   pro.product_id  
,   pro.[description] AS 'Product Desc'  
,   ins.insurance_folder_cnt  
,   ifd.new_insurance_file_cnt
,  src.code AS 'BranchCode'
,pro.code AS 'ProductCode'
  
FROM  
 Insurance_File_Cloned_RI_Usage AS ifd  
INNER JOIN  
    Insurance_File AS ins ON ins.insurance_file_cnt = ifd.insurance_file_cnt  
INNER JOIN  
    Source AS src ON src.source_id = ins.source_id  
INNER JOIN  
    Party AS par ON par.party_cnt = ins.insured_cnt  
INNER JOIN  
    Product AS pro ON pro.product_id = ins.product_id 
WHERE ins.insurance_ref =LTRIM (RTRIM(@sPolicy_number))
AND  (ins.Lead_agent_cnt=@AgentKey OR @AgentKey=0)  
ORDER BY  
    src.description ASC,  ins.insurance_ref ASC, ifd.ins_file_Cloned_RI_usage_id ASC      
