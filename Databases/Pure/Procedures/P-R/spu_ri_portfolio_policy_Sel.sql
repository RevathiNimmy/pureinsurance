EXECUTE DDLDropProcedure 'spu_ri_portfolio_policy_Sel'
GO

CREATE PROCEDURE spu_ri_portfolio_policy_Sel
	@product_id int,
 @source_id int,  
	@transfer_date datetime
AS

DECLARE @sSQL1 nVARCHAR(MAX)
DECLARE @sSQL2 nVARCHAR(4000)
DECLARE @productSQL nVARCHAR(100)
DECLARE @sourceSQL nVARCHAR(100)

IF ISNULL(@product_id,0)<>0
SELECT @productSQL= ' product_id =' + CONVERT(nVarchar,@product_id) + ' AND '
ELSE
SELECT @productSQL= '' 
IF ISNULL(@source_id,0)<>0
SELECT @sourceSQL= ' ifi.source_id =' + CONVERT(nVarchar,@source_id) + ' AND '
ELSE
SELECT @sourceSQL= ''

SELECT @sSQL1 = 'SELECT ifi.insurance_file_cnt, 
   ifi.insurance_ref,  
   p.shortname,  
   p.resolved_name,'''  
   + CONVERT(nVarchar(20),@transfer_date)+''',  
   cover_start_date,  
    ifi.expiry_date,  
    isnull(ifi.inception_date_tpi ,cover_start_date),  
    product_id,  
    ift.code,  
 1 orderby  
    FROM Insurance_File ifi  
      JOIN    Party p                        ON ifi.insured_cnt = p.party_cnt  
    LEFT JOIN  
            Insurance_File_Type ift      ON ifi.insurance_file_type_id = ift.insurance_file_type_id  
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
    LEFT JOIN  
            Insurance_File_Type ift      ON ifi.insurance_file_type_id = ift.insurance_file_type_id  
 
 JOIN    RI_Band_Version rb        ON rb.ri_band_id = u.ri_band  
 JOIN Proportional_RI_Calculation_Method pm ON PM.Proportional_RI_Calculation_Method_Id = rb.Proportional_RI_Cal_Method  
 JOIN RI_Model rim   ON rim.ri_model_id = u2.ri_model_id  
 LEFT JOIN Insurance_File_PT_RI_Usage IFPTU ON IFPTU.insurance_file_cnt=ifi.insurance_file_cnt
 LEFT JOIN Insurance_File_PT_log IFPTL ON (IFPTL.insurance_folder_cnt=ifi.insurance_folder_cnt and IFPTL.effective_date ='''+ CONVERT(nVarchar(20),@transfer_date) +''')
    WHERE '   + @productSQL + @sourceSQL + 
   '''' + CONVERT(nVarchar(20),@transfer_date) + ''' BETWEEN ifi.cover_start_date AND ifi.expiry_date 
   AND ''' + CONVERT(nVarchar(20),@transfer_date) + ''' >= u2.effective_date 
   AND ''' + CONVERT(nVarchar(20),@transfer_date) + ''' > ifi.cover_start_date 
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
  
SELECT @sSQL2= ' union all  
  
    SELECT	DISTINCT
    		ifi.insurance_file_cnt,
            ifi.insurance_ref,
    		p.shortname, 
   p.resolved_name,'''+  
   CONVERT(nVarchar(20),@transfer_date)+''',  
    cover_start_date,  
    ifi.expiry_date,  
    MAX(ifi.inception_date_tpi ),  
    product_id,  
    ift.code,  
     2 orderby  
    FROM	Insurance_File ifi
    JOIN    Party p                        ON ifi.insured_cnt = p.party_cnt
    JOIN    insurance_file_risk_link l     ON ifi.insurance_file_cnt = l.insurance_file_cnt
    JOIN    risk r                         ON r.risk_cnt = l.risk_cnt
    JOIN    ri_arrangement ra              ON r.risk_cnt = ra.risk_cnt
    JOIN    risk_type_ri_model_usage u     ON u.risk_type_id = r.risk_type_id 
                                          AND u.ri_band = ra.ri_band_id
                                          AND u.ri_model_id = ra.ri_model_id
    JOIN    risk_type_ri_model_usage u2    ON u2.portfolio_transfer_from_cnt = u.risk_type_ri_model_usage_cnt
    LEFT JOIN 
            insurance_file_status ifs      ON ifi.insurance_file_status_id = ifs.insurance_file_status_id
    LEFT JOIN  
            Insurance_File_Type ift      ON ifi.insurance_file_type_id = ift.insurance_file_type_id  
 
	JOIN    RI_Band_Version rb					   ON rb.ri_band_id = u.ri_band
	JOIN	Proportional_RI_Calculation_Method pm	ON PM.Proportional_RI_Calculation_Method_Id = rb.Proportional_RI_Cal_Method
	JOIN	RI_Model rim			ON rim.ri_model_id = u2.ri_model_id		
 LEFT JOIN Insurance_File_PT_log IFPTL ON IFPTL.insurance_file_cnt=ifi.insurance_file_cnt
    WHERE ' + @productSQL + @sourceSQL + 
    '''' + CONVERT(nVarchar(20),@transfer_date) + ''' >= u2.effective_date 
	AND ''' + CONVERT(nVarchar(20),@transfer_date) + ''' > ifi.cover_start_date 
   AND ra.original_flag = 0 
    	AND u.is_deleted = 0
    	AND u2.is_deleted = 0
   AND l.status_flag IN (''C'',''D'')  
   AND IFPTL.insurance_folder_cnt IS NULL
   AND rb.Proportional_RI_Cal_Method = 2 
  
   AND isnull(out_of_sequence_replaced,0) = 0  
   AND ra.version_id =1
 and ifi.insurance_file_type_id in (1,3,4,7,10,11)  
    GROUP BY 
    		ifi.insurance_file_cnt,
            ifi.insurance_ref,
    		p.shortname, 
    		p.resolved_name,
   rb.Proportional_RI_Cal_Method,  
       cover_start_date,  
    ifi.expiry_date,  
    ifi.inception_Date,  
    product_id,  
    ift.code,  
    ifi.insurance_file_type_id  

order by ifi.cover_start_date,ifi.insurance_file_cnt ' 

SELECT @sSQL1=@sSQL1 + @sSQL2

EXECUTE SP_EXECUTESQL @sSQL1
GO
