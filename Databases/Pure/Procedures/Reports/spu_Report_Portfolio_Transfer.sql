SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Report_Portfolio_Transfer'
GO
CREATE PROCEDURE spu_Report_Portfolio_Transfer  
 AS  
 BEGIN
SELECT distinct p.description 'Product Description',ins.insurance_ref ,r.description 'Risk Description',  
 CASE  
 when ifpt.status =1 Then 'Reinsurance Pending'  
 When ifpt .status = 2 Then 'RI Calculated'  
 When ifpt.status = 3 Then 'Posted'  
 end  as 'status',  
 CASE  
 when ifpt.status =1 Then 'Reinsurance is Pending'  
 When ifpt .status = 2 Then 'RI is Calculated'  
 When ifpt.status = 3 Then 'Done Portfolio Transfer'  
 end  AS status_description FROM Insurance_File_PT_RI_Usage  ifpt  
 JOIN insurance_file ins ON ins.insurance_file_cnt = ifpt.insurance_file_cnt 
 Join insurance_file_risk_link ifrl ON ifrl.insurance_file_cnt = ifpt.insurance_file_cnt          
 JOIN Risk r ON r.risk_cnt = ifrl.risk_cnt  
 JOIN Product P ON p.product_id = ins.product_id  
 
 UNION ALL
   
 SELECT distinct p.description 'Product Description',ins.insurance_ref ,r.description 'Risk Description',  
 CASE  
 when ifpt.status_id =1 Then 'Reinsurance Pending'  
 When ifpt .status_id = 2 Then 'RI Calculated'  
 When ifpt.status_id = 3 Then 'Posted'  
 end  as 'status',  
 CASE  
 when ifpt.status_id =1 Then 'Reinsurance is Pending'  
 When ifpt .status_id = 2 Then 'RI is Calculated'  
 When ifpt.status_id = 3 Then 'Done Portfolio Transfer'  
 end  AS status_description FROM insurance_file_pt_log ifpt  
 JOIN insurance_file ins ON ins.insurance_file_cnt = ifpt.insurance_file_cnt  
        AND ins.insurance_folder_cnt = ifpt.insurance_folder_cnt  
 JOIN Risk r ON r.risk_cnt = ifpt.risk_cnt  
 JOIN Product P ON p.product_id = ins.product_id  
 WHERE ifpt.insurance_file_cnt NOT IN (SELECT insurance_file_cnt from Insurance_File_PT_RI_Usage)
 ORDER BY ins.insurance_ref  
END  

GO