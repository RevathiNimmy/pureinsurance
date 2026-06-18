SET QUOTED_IDENTIFIER OFF 

SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Amount_Not_Included_In_Instalment'
GO

CREATE PROCEDURE spu_Get_Amount_Not_Included_In_Instalment 
@transdetail_id int, 
@amount NUMERIC(18,2) output AS
BEGIN 

	DECLARE @spare varchar(10)
	DECLARE @InsuranceFileCnt int
	DECLARE @tax_band_id int
	DECLARE @tax_group_id int

	SELECT @spare=spare,@InsuranceFileCnt=doc.insurance_file_cnt,@tax_band_id = tax_band_id, @tax_group_id = tax_group_id      
	    FROM  Document doc      
	    INNER JOIN      
	        TransDetail td ON td.document_id = doc.document_id      
	    WHERE      
	        td.transdetail_id = @transdetail_id

	 IF @spare='TAX'  
	   Select @amount=ISNULL(SUM(value),0) FROM tax_calculation  
	          WHERE insurance_file_cnt=@InsuranceFileCnt
			  AND tax_band_id = @tax_band_id
			  AND tax_group_id = @tax_group_id
	          AND include_tax_in_instalments=0 AND is_not_applied_to_client=0
			  AND transtype not in ('TTRITP','TTRITC')
			  AND pfprem_finance_cnt IS  NULL
	 ELSE IF @spare='FEE'  
	   Select @amount=ISNULL(SUM(base_fee_amount),0) FROM policy_fee_u  
	          WHERE insurance_file_cnt=@InsuranceFileCnt  
	          AND include_fee_in_instalments=0  
	 ELSE  
  
  	SET @amount=0  

END
GO