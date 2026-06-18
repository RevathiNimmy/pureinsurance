
Execute DDLDropProcedure 'spu_SIR_Update_Policy_Details'
GO

CREATE PROCEDURE spu_SIR_Update_Policy_Details

@insurance_file_cnt INT,  
@put_on_next_instalment_renewal TINYINT = 0,  
@payment_method VARCHAR(60) ='',  
@marked_for_collection TINYINT=0,
@marked_date DATETIME=NULL,
@payment_term INT = NULL,
@collection_frequency INT = NULL,
@nInsuranceFileStatus INT=0
     
AS    
    
BEGIN    

IF  (@nInsuranceFileStatus=0)
 BEGIN

 UPDATE insurance_file    
 SET put_on_next_instalment_renewal = @put_on_next_instalment_renewal,  
   payment_method=@payment_method,
   marked_for_collection=@marked_for_collection,
   marked_date=@marked_date,
   insurance_file_status_id =CASE WHEN @nInsuranceFileStatus = 0 THEN insurance_file_status_id ELSE @nInsuranceFileStatus END,    
   DOPaymentTerms_id = @payment_term,
   CollectionFrequency_id = @collection_frequency
  
 WHERE insurance_file_cnt =@insurance_file_cnt    
	END
	ELSE
	BEGIN
		UPDATE insurance_file
			SET insurance_file_status_id = @nInsuranceFileStatus  
		WHERE insurance_file_cnt =@insurance_file_cnt   
	END

 END
GO