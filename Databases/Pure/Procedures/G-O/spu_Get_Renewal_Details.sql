SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Renewal_Details'
GO
CREATE PROCEDURE spu_Get_Renewal_Details  
 @nInsurance_file_cnt INTEGER
 
 As  
 BEGIN  
	SELECT rs.renewal_status_type_id, 
		   P.is_true_monthly_policy,
		   P.do_not_delete_renQuote_on_mta,
		   if2.anniversary_copy,
		   RS.renewal_status_cnt,
		   P.Delete_And_ReRun_RenQuote
	FROM    
		renewal_status RS 
		JOIN insurance_file if2  
		ON rs.renewal_insurance_file_cnt = if2.insurance_file_cnt
		JOIN insurance_file if1 
		ON if2.insurance_folder_cnt = if1.insurance_folder_cnt 
		JOIN Product P ON if1.product_id = P.product_id
	WHERE if1.insurance_file_cnt =@nInsurance_file_cnt
 END  
    
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
