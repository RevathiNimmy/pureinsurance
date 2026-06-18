SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Add_CCItem_For_Cancelled_InstPlan'
GO

CREATE PROCEDURE spu_ACT_Add_CCItem_For_Cancelled_InstPlan      
    @insurance_file_cnt INT,      
    @business_type      varchar(10), 
    @transdetail_id     int      
    
AS BEGIN

	-- NB: This stored procedure 
	-- should only be used for direct business
	-- the amounts calculated here will not 
	-- work for agency business as they
	-- dont take commission into account
    
	Declare @account_id       	int      
	Declare @document_id          	int      
	Declare @document_date        	datetime      
	Declare @can_auto_cancel      	tinyint      
	Declare @amount           	numeric(19,4)      
	Declare @credit_control_step_id int      
	Declare @due_date        	datetime      
	Declare @business_type_code  	char(10)      
	Declare @processing_days  	smallint    
	Declare @agent_account_id 	int
	Declare @agent_type			char(10)
    
	IF (SELECT COUNT(*)
		  FROM peril_type PT      
	     WHERE PT.is_auto_cancel = 1      
	       AND PT.peril_type_id IN (      
	         SELECT peril_type_id 
			   FROM peril      
	          WHERE risk_cnt IN (SELECT risk_cnt      
	                               FROM insurance_file_risk_link      
	                              WHERE insurance_file_cnt = @insurance_file_cnt))) > 0      
	    SET @can_auto_cancel = 1      
	ELSE      
	    SET @can_auto_cancel = 0      
      
	SELECT       
	     @account_id =Account.account_id,      
	     @document_id =Document.document_id,      
	     @document_date =Document.document_date,      
	     @business_type_code =Business_Type.code,      
	     @credit_control_step_id = Credit_Control_Step.credit_control_step_id,      
	     @processing_days =Credit_Control_Rule.processing_days,      
	     @agent_account_id =Account1.account_id,
		 @agent_type = party_agent_type.code    
	    
	FROM Insurance_File    
	    
	 LEFT JOIN Account ON    
	  Insurance_File.insured_cnt = Account.Account_Key    
	    
	 LEFT JOIN Document ON    
	  INsurance_File.Insurance_file_Cnt = Document.Insurance_file_Cnt    
	    
	 LEFT JOIN Business_Type ON    
	  InsurancE_file.Business_type_id = Business_Type.Business_Type_Id    
	    
	 LEFT JOIN Credit_Control_Rule ON    
	  Insurance_file.Source_Id = Credit_Control_Rule.Source_Id    
	    
	 LEFT JOIN Credit_Control_Step ON    
	  Credit_Control_Rule.Credit_Control_Rule_Id = Credit_Control_Step.Credit_Control_Rule_Id     
	    
	 LEFT JOIN Account Account1 ON    
	  Insurance_file.lead_agent_cnt = Account1.Account_Key    
	    
	  LEFT JOIN party_agent ON 
	   Account1.account_key = party_agent.party_cnt

	   left join party_agent_type on
	   party_agent.party_agent_type_id = party_agent_type.party_agent_type_id
	    
	 LEFT JOIN (SELECT Insurance_File_Cnt, SUM(commission_value) AS commission_value FROM Agent_Commission  
	     	    WHERE is_lead_agent = 1  
		    GROUP BY insurance_file_cnt) Agent_Commission ON  
	 	Insurance_file.Insurance_file_Cnt = Agent_Commission.Insurance_file_Cnt  
	  
	WHERE Credit_Control_step.Step_Number = 1    
	AND Credit_Control_Rule.is_active= 1    
	AND Credit_Control_Rule.Business_Type = @business_type    
	AND Insurance_File.Insurance_file_Cnt = @insurance_file_cnt    
    AND (Credit_Control_Rule.Product_ID IS NULL OR  Credit_Control_Rule.Product_ID = Insurance_file.Product_ID)

	-- when the instalment plan is cancelled 
	-- a single debit transaction is added for the full amount of the outstanding debt 
	-- this transaction includes (gross + tax) not commission and so will not return 
	-- a true credit control figure for agency business

    --	Select @amount = outstanding_amount from transdetail where transdetail_id = @transdetail_id
    SET @amount = 0  
	
	SELECT @due_date = DATEADD(day, IsNull(@processing_days,0), getdate())      
	
    IF @business_type_code <> 'DIRECT'  AND EXISTS(SELECT NULL 													 FROM party p                                                 INNER JOIN party_agent pa ON p.party_cnt = pa.party_cnt                                                 INNER JOIN account ac ON p.shortname = ac.short_code 												    WHERE account_id = @agent_account_id AND party_agent_type_id <> 3)  		
	if @business_type_code <> 'DIRECT' and @agent_type <> 'Comm Acc'
		BEGIN
			if @agent_type = 'Intermed'
				select @account_id = intermediary_agent_account_id from Insurance_File where insurance_file_cnt = @insurance_file_cnt
			else
				select @account_id = @agent_account_id    
		END
	
	IF @account_id IS NOT NULL 
		AND @credit_control_step_id IS NOT NULL 
			AND @document_id IS NOT NULL 
				AND @document_date IS NOT NULL 
	AND @amount IS NOT NULL BEGIN         	 
	 	 
		INSERT INTO Credit_Control_Item (credit_control_reason,      
			account_id,      
			document_id,      
			document_date,      
			insurance_file_cnt,      
			pfprem_finance_cnt,      
			pfprem_finance_version,      
			amount,      
			can_auto_cancel,      
			will_auto_cancel,      
			credit_control_step_id,      
			created_date,      
			due_date,      
			letter_sent,      
					recurrence_count,  
					is_deleted)  
		   VALUES	(@business_type,      
			@account_id,      
			@document_id,      
			@document_date,      
			@insurance_file_cnt,      
			null,      
			null,      
			@amount,      
			@can_auto_cancel,      
			0,      
			@credit_control_step_id,      
			getdate(),      
			@due_date,      
			0,      
					0,  
					NULL)  
	END      
    
END 
 
  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
