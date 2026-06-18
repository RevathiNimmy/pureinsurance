SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Process_Make_Live_Risks'
GO

CREATE PROCEDURE spu_SIR_Policy_Discount_Process_Make_Live_Risks  
 @insurance_file_cnt int  
AS  
  
BEGIN  

DECLARE @insurance_folder_cnt INT ,
@InsFile INT 

SELECT @insurance_folder_cnt=insurance_folder_cnt from Insurance_File where insurance_file_cnt = @insurance_file_cnt  

DECLARE risk_cursor Cursor fast_forward For
SELECT insurance_file_cnt FROM insurance_file WHERE insurance_folder_cnt=@insurance_folder_cnt and insurance_file_cnt >= @insurance_file_cnt order by insurance_file_cnt

OPEN risk_cursor 
FETCH NEXT FROM risk_cursor INTO @Insfile 
WHILE @@FETCH_STATUS = 0
 BEGIN
  
	UPDATE risk  
	SET is_discounted = 0,  
	premium_this_year = ISNULL(original_risk.premium_this_year,0) + ISNULL(total_this_premium,0)  

	FROM risk r  

	INNER JOIN insurance_file_risk_link ifrl ON  
	r.risk_cnt = ifrl.risk_cnt  

	LEFT OUTER JOIN (SELECT risk_cnt, premium_this_year FROM risk) original_risk ON  
	ifrl.original_risk_cnt = original_risk.risk_cnt  

	INNER JOIN POLICY_DISCOUNT_VALID_RISK_STATUSES rs ON  
	r.risk_status_id = rs.risk_status_id  

	WHERE ifrl.insurance_file_cnt = @Insfile  
	AND r.is_risk_selected = 1  

UPDATE insurance_file  
SET lapsed_date=cover_start_date  
WHERE insurance_file_cnt = @insurance_file_cnt  
and insurance_file_type_id = 8  

FETCH NEXT FROM risk_cursor INTO @Insfile 
	
 END
CLOSE risk_cursor  
END    



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
