  SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_IsPending_PT'
GO
CREATE PROCEDURE spu_SAM_IsPending_PT    
@sPolicyNumber Varchar(50),    
@nInsuranceFileCnt INT =0    
AS    
    
DECLARE @transfer_date DATETIME,  
  @year_name INT,  
  @period_end_date DATETIME ,  
  @current_System_Date DATETIME,  
 @sSQL nVARCHAR(4000),  
 @policyNumSQL nVarchar(50)  
  
SELECT @year_name= YEAR(GETDATE())  
SELECT @current_System_Date = CONVERT(DATE,GETDATE())  
SELECT top 1 @period_end_date = period_end_date  
 FROM Period  
   WHERE YEAR(period_end_date) = @year_name  
 ORDER BY period_end_date  
  
SELECT @transfer_date =DATEADD(m,DATEDIFF(m,0,@period_end_date),0)  
IF ISNULL(@sPolicyNumber,'')=''  
SELECT @sPolicyNumber = LTRIM(RTRIM(insurance_ref )) FROM Insurance_File WHERE insurance_file_cnt = @nInsuranceFileCnt  
  
SELECT @policyNumSQL= ' ifi.insurance_ref =''' + LTRIM(RTRIM(@sPolicyNumber)) + ''' AND '  
  
IF @current_System_Date >= @transfer_date  
SELECT @sSQL = 'SELECT ifi.insurance_file_cnt,  
   ifi.insurance_ref,  
   cover_start_date,  
    ifi.expiry_date,  
    product_id  
    FROM Insurance_File ifi  
WHERE insurance_file_cnt in (  
   SELECT max(ifi.insurance_file_cnt)  
    FROM Insurance_File ifi  
    JOIN    insurance_file_risk_link l     ON ifi.insurance_file_cnt = l.insurance_file_cnt  
    JOIN    risk r                         ON r.risk_cnt = l.risk_cnt  
    JOIN    ri_arrangement ra              ON r.risk_cnt = ra.risk_cnt  
    JOIN    risk_type_ri_model_usage u     ON u.risk_type_id = r.risk_type_id  
                                          AND u.ri_band = ra.ri_band_id  
                                          AND u.ri_model_id = ra.ri_model_id  
    JOIN    risk_type_ri_model_usage u2    ON u2.portfolio_transfer_from_cnt = u.risk_type_ri_model_usage_cnt  
    LEFT JOIN  
            insurance_file_status ifs      ON ifi.insurance_file_status_id = ifs.insurance_file_status_id  
 JOIN    RI_Band_Version rb        ON rb.ri_band_id = u.ri_band  
 JOIN Proportional_RI_Calculation_Method pm ON PM.Proportional_RI_Calculation_Method_Id = rb.Proportional_RI_Cal_Method  
 JOIN RI_Model rim   ON rim.ri_model_id = u2.ri_model_id  
 LEFT JOIN Insurance_File_PT_RI_Usage IFPTU ON IFPTU.insurance_file_cnt=ifi.insurance_file_cnt  
 LEFT JOIN Insurance_File_PT_log IFPTL ON (IFPTL.insurance_folder_cnt=ifi.insurance_folder_cnt and IFPTL.effective_date ='''+ CONVERT(nVarchar(20),@transfer_date) +''')  
  JOIN (SELECT MAX(insurance_file_cnt) MAX_insurance_file_cnt FROM insurance_file where insurance_ref =''' + @sPolicyNumber +''' and ''' + CONVERT(nVarchar(20),@transfer_date) + '''>cover_start_date AND insurance_file_type_id in (2,5,9)) M  
 ON M.MAX_insurance_file_cnt=IFI.insurance_file_cnt   
    WHERE '  + @policyNumSQL +  
   '''' + CONVERT(nVarchar(20),@transfer_date) + ''' BETWEEN ifi.cover_start_date AND ifi.expiry_date  
   AND ''' + CONVERT(nVarchar(20),@transfer_date) + ''' >= u2.effective_date  
   AND ISNULL(ifs.code, '''') NOT IN (''REPPT'',''CAN'')  
   AND ra.original_flag = 0  
   AND u.is_deleted = 0  
   AND u2.is_deleted = 0  
   AND rb.Proportional_RI_Cal_Method = 2  
   AND IFPTU.insurance_file_cnt IS NULL  
   AND IFPTL.insurance_folder_cnt IS NULL  
   AND isnull(out_of_sequence_replaced,0) =0  
   AND ra.version_id =1  
   AND ifi.insurance_file_type_id in (2,5,9)  
   AND EXISTS (SELECT rml.ri_model_id FROM ri_model_line rml JOIN Treaty T ON rml.Treaty_id=T.Treaty_id WHERE rml.ri_model_id=rim.ri_model_id AND reinsurance_type_id in (2,6,7,8))  
    GROUP BY  
     ifi.insurance_ref  )  '  
exec (@sSQL)  