SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Add_Credit_Control_Item_InsFile'
GO

CREATE PROCEDURE spu_ACT_Add_Credit_Control_Item_InsFile    
    @insurance_file_cnt INT,    
    @business_type      varchar(20)    
  
AS BEGIN
  
DECLARE @account_id      	int    
DECLARE @document_id         	int    
DECLARE @document_date       	datetime    
DECLARE @can_auto_cancel     	tinyint    
DECLARE @amount          	numeric(19,4)    
DECLARE @credit_control_step_id int    
DECLARE @due_date        datetime    
DECLARE @business_type_code  char(10)    
DECLARE @processing_days  smallint  
DECLARE @agent_account_id int   
DECLARE @peril_count int
DECLARE @auto_cancel_count int
DECLARE @trans_due_date datetime
DECLARE @use_effective_date TINYINT
DECLARE @cover_start_date DATETIME
DECLARE @greater_of_effective_transaction TINYINT
DECLARE @underwriting_broking CHAR(1)  
DECLARE @amountoutstanding NUMERIC(19,4)  
DECLARE @use_due_date TINYINT
DECLARE @PolicyFee NUMERIC(19,4)
DECLARE @intermediary_agent_account_id INT
DECLARE @party_agent_type INT
DECLARE @lead_agent_cnt INT
DECLARE @lead_agent_type VARCHAR(50)

--Check if this SP called from Import in the case of Supersede or Cancelled plan

DECLARE @Origional_business_type VARCHAR(20)

SET @Origional_business_type=@business_type

IF @business_type='IMPORT'
BEGIN	
	SET @business_type='MTA'
END

CREATE TABLE #temptable  
(  
 amount NUMERIC(19,4),  
 outstanding NUMERIC(19,4)  
)  
  
SELECT @underwriting_broking = value  
FROM hidden_options  
WHERE branch_id = 1  
AND option_number = 1  

SELECT @peril_count=COUNT(DISTINCT peril_type.peril_type_id) 
	FROM peril_type WITH(NOLOCK)
	INNER JOIN peril WITH(NOLOCK) ON peril.peril_type_id=peril_type.peril_type_id    
    WHERE risk_cnt IN (SELECT risk_cnt    
					   FROM insurance_file_risk_link   WITH(NOLOCK)   
					   WHERE insurance_file_cnt = @insurance_file_cnt)

SELECT @auto_cancel_count=Count(peril_type_id)
	FROM peril_type PT WITH(NOLOCK)   
    WHERE PT.is_auto_cancel = 1    
    and PT.peril_type_id in (    
         SELECT peril_type_id
		 FROM peril WITH(NOLOCK)    
         WHERE risk_cnt IN (SELECT risk_cnt    
                            FROM insurance_file_risk_link WITH(NOLOCK)    
                            WHERE insurance_file_cnt = @insurance_file_cnt))
  
IF @auto_cancel_count = @peril_count   
    SELECT @can_auto_cancel = 1    
ELSE    
    SELECT @can_auto_cancel = 0    
    
INSERT INTO #temptable(amount, outstanding) 
EXEC spu_ACT_Is_Policy_Paid @insurance_file_cnt  
  
IF EXISTS (SELECT outstanding FROM #temptable)  
BEGIN  
    SELECT @amountoutstanding = ISNULL(outstanding,0) From #temptable  
END  
ELSE  
BEGIN  
    SET @amountoutstanding = 0  
END  
DROP TABLE #temptable  
    
SELECT     
     @account_id = Account.account_id,    
     @document_id = Document.document_id,    
     @document_date = Document.document_date,    
     @amount = (CASE @amountoutstanding  
				WHEN 0 THEN  
				Insurance_File.this_premium + ISNULL(Insurance_file.tax_amount,0) - ISNULL(commission_value, 0)  
				ELSE  
				@amountoutstanding  
				END  
				),  
     @business_type_code = Business_Type.code,    
     @credit_control_step_id = Credit_Control_Step.credit_control_step_id,    
     @processing_days = Credit_Control_Rule.processing_days,    
     @agent_account_id = Account1.account_id,
     @cover_start_date = Insurance_File.cover_start_date,	
     @use_effective_date = isnull(Credit_Control_Rule.use_effective_date,0),
     @greater_of_effective_transaction = isnull(Credit_Control_Rule.use_greater_of_transaction_and_effective_date,0),
	 @use_due_date = isnull(credit_control_rule.use_due_date,0), 
	 @trans_due_date = COALESCE(td.due_date, insurance_file.cover_start_date),
	 @intermediary_agent_account_id = Insurance_File.intermediary_agent_account_id,
     @lead_agent_cnt = Insurance_File.lead_agent_cnt
  
FROM Insurance_File WITH(NOLOCK)
  
 LEFT JOIN Account WITH(NOLOCK) ON  
  Insurance_File.insured_cnt = Account.Account_Key  
  
 LEFT JOIN Document WITH(NOLOCK) ON  
  Insurance_File.Insurance_file_Cnt = Document.Insurance_file_Cnt and document.document_ref NOT LIKE 'SEC%'
  
  LEFT JOIN transdetail td with (nolock) ON 
  td.document_id = document.document_id AND td.spare = 'GROSS'
  
 LEFT JOIN Business_Type WITH(NOLOCK) ON  
  InsurancE_file.Business_type_id = Business_Type.Business_Type_Id  
  
 LEFT JOIN Credit_Control_Rule WITH(NOLOCK) ON  
  Insurance_file.Source_Id = Credit_Control_Rule.Source_Id  
  
 LEFT JOIN Credit_Control_Step WITH(NOLOCK) ON  
  Credit_Control_Rule.Credit_Control_Rule_Id = Credit_Control_Step.Credit_Control_Rule_Id   
  
 LEFT JOIN Account Account1 WITH(NOLOCK) ON  
  Insurance_file.lead_agent_cnt = Account1.Account_Key  
  
 LEFT JOIN (SELECT Insurance_File_Cnt, Sum(commission_value) AS commission_value from Agent_Commission WITH(NOLOCK)
	    WHERE is_lead_agent = 1
	    GROUP BY insurance_file_cnt) Agent_Commission ON
	InsurancE_file.Insurance_file_Cnt = Agent_Commission.Insurance_file_Cnt

WHERE Credit_Control_step.Step_Number = 1  
AND Credit_Control_Rule.is_active= 1  
AND Credit_Control_Rule.Business_Type = @business_type  
AND Insurance_File.Insurance_file_Cnt = @insurance_file_cnt  
AND (Credit_Control_Rule.product_id IS NULL OR Credit_Control_Rule.product_id = Insurance_file.product_id)

IF (@use_due_date = 1)
BEGIN
	SELECT @due_date = DATEADD(day, IsNull(@processing_days,0), @trans_due_date)
END
ELSE
BEGIN
	IF(@use_effective_date = 1 AND @business_type='REN WTG UPDATE')
		SELECT @due_date = DATEADD(day, IsNull(@processing_days,0), @cover_start_date)
	ELSE IF @use_effective_date = 0 
		SELECT @due_date = DATEADD(day, IsNull(@processing_days,0), getdate())
	ELSE IF (@greater_of_effective_transaction = 1 AND @cover_start_date < getdate())
		SELECT @due_date = DATEADD(day, IsNull(@processing_days,0), getdate())
	ELSE
		SELECT @due_date = DATEADD(day, IsNull(@processing_days,0), @cover_start_date)
END

--Don't delete existing credit control item entry in case of Import (Superseded or Cancelled plan)
IF (@Origional_business_type<>'IMPORT')
	BEGIN
		DELETE Credit_Control_Item WITH (ROWLOCK) WHERE insurance_file_cnt = @insurance_file_cnt
	END

IF @business_type_code <> 'DIRECT'    
    BEGIN
		SELECT  @lead_agent_type = party_agent_type_id
			FROM    Party_Agent
			WHERE   Party_Agent.party_cnt = @lead_agent_cnt

        IF NOT(@lead_agent_type = 3 OR @agent_account_id <> ISNULL(@intermediary_agent_account_id,@agent_account_id)) 
			SELECT @account_id = @agent_account_id 
	END

IF (@Origional_business_type='IMPORT')
 BEGIN	
	SELECT @amount=SUM(amount) FROM TransDetail TD WITH(NOLOCK) 
	INNER JOIN Document Doc WITH(NOLOCK) ON TD.document_id=Doc.document_id
	WHERE insurance_file_cnt=@insurance_file_cnt and td.account_id=@account_id And TD.document_id=@document_id

	SET @due_date=GETDATE()
 END
 
  
  
if ((@document_id IS NOT NULL AND @document_date IS NOT NULL) 
    OR (@business_type = 'REN WTG UPDATE')) AND
    @account_id IS NOT NULL AND
    @amount IS NOT NULL AND
    @credit_control_step_id IS NOT NULL   
  
 BEGIN    
  INSERT INTO Credit_Control_Item (    
   credit_control_reason,    
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
   recurrence_count)    
  VALUES (    
   @business_type,    
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
   0)    
 END    
  
END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
  
  
  
