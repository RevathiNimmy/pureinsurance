
EXECUTE DDLDropProcedure 'spu_Ins_File_PT_RI_Usage_sel_amend'
GO

CREATE PROCEDURE spu_Ins_File_PT_RI_Usage_sel_amend   
@policy_number as varchar(30) = NULL ,
@insurance_file_cnt int =0,
@AgentKey INT=0  
AS    

IF @insurance_file_cnt>0
SELECT @policy_number = LTRIM(RTRIM(insurance_ref))     FROM insurance_file WHERE insurance_file_cnt = @insurance_file_cnt
    
-- get the records that are marked as 'manual review'   
If ISNULL(@policy_number,'') <> ''   
SELECT    
 ifd.ins_file_PT_RI_usage_id,    
 ifd.insurance_file_cnt,    
 ins.insurance_ref AS 'Policy Number',    
 src.source_id,    
 src.[description] AS 'Branch Desc',    
 ifd.status  ,    
 pri.description AS 'PT RI Status Desc',    
 ins.insured_cnt  ,    
 par.shortname AS 'Party Shortname',    
 par.[name] AS 'Party Name',    
 pro.product_id,    
 pro.[description] AS 'Product Desc',    
 ins.insurance_folder_cnt,    
 TransferDate,    
 ifd.new_insurance_file_cnt ,
  pro.code AS 'ProductCode',
 src.code AS 'BranchCode',
 ins.cover_start_date ,
 ins.expiry_date,
 ins.inception_date_tpi      
FROM    
 Insurance_File_PT_RI_Usage AS ifd    
INNER JOIN    
    Insurance_File AS ins ON ins.insurance_file_cnt = ifd.insurance_file_cnt    
INNER JOIN    
    Party AS par ON par.party_cnt = ins.insured_cnt    
INNER JOIN    
    Source AS src ON src.source_id = ins.source_id    
INNER JOIN    
    PT_RI_Status_Type AS pri ON pri.PT_RI_status_type_id = ifd.status    
    
INNER JOIN    
    Product AS pro ON pro.product_id = ins.product_id    
WHERE ins.insurance_ref =LTRIM (RTRIM(@policy_number))  
	  AND (ins.Lead_agent_cnt=@AgentKey OR @AgentKey=0)
ORDER BY    
    src.source_id ASC,    
    ins.insurance_ref ASC    
      
ELSE  
SELECT    
 ifd.ins_file_PT_RI_usage_id,    
 ifd.insurance_file_cnt,    
 ins.insurance_ref AS 'Policy Number',    
 src.source_id,    
 src.[description] AS 'Branch Desc',    
 ifd.status  ,    
 pri.description AS 'PT RI Status Desc',    
 ins.insured_cnt  ,    
 par.shortname AS 'Party Shortname',    
 par.[name] AS 'Party Name',    
 pro.product_id,    
 pro.[description] AS 'Product Desc',    
 ins.insurance_folder_cnt,    
 TransferDate,    
 ifd.new_insurance_file_cnt,
 pro.code AS 'ProductCode',
 src.code AS 'BranchCode' ,
 ins.cover_start_date ,
 ins.expiry_date,
 ins.inception_date_tpi     
FROM    
 Insurance_File_PT_RI_Usage AS ifd    
INNER JOIN    
    Insurance_File AS ins ON ins.insurance_file_cnt = ifd.insurance_file_cnt    
INNER JOIN    
    Party AS par ON par.party_cnt = ins.insured_cnt    
INNER JOIN    
    Source AS src ON src.source_id = ins.source_id    
INNER JOIN    
    PT_RI_Status_Type AS pri ON pri.PT_RI_status_type_id = ifd.status    
    
INNER JOIN    
    Product AS pro ON pro.product_id = ins.product_id    
WHERE (ins.Lead_agent_cnt=@AgentKey OR @AgentKey=0)  
ORDER BY    
    src.source_id ASC,    
    ins.insurance_ref ASC   
      
GO
