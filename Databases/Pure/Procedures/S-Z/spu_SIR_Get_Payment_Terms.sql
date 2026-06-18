--**********************************************************************************************
--Author:- Vidya Rangdale
--Date:- 15/09/2014
--Description:-It will get payment terms
--**********************************************************************************************
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_Payment_Terms'
GO

CREATE Procedure spu_SIR_Get_Payment_Terms
  @InsuranceFileCnt INT,  
  @UserID INT,  
  @InvoiceEnabled BIT = 0 OUTPUT,  
  @InstalmentsEnabled BIT = 0 OUTPUT,  
  @PaynowEnabled BIT = 0 OUTPUT,
  @BankGuaranteeEnabled BIT = 0 OUTPUT,    
  @Branch_id INTEGER =1,
  @CashDepositEnabled BIT = 0 OUTPUT --Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
AS  
  
DECLARE @BusinessTypeID INT  
DECLARE @LeadAgentCnt INT  
DECLARE @ProductID INT  
DECLARE @COInsuranceLinkToAgent BIT  
DECLARE @ConditionA BIT  
DECLARE @ConditionB BIT  
DECLARE @ConditionC BIT  
SET @COInsuranceLinkToAgent=0

SELECT @COInsuranceLinkToAgent=ISNULL(value,0) FROM System_options WHERE option_number=5026 
AND Branch_id=@Branch_id

SELECT @BusinessTypeID=business_type_id, @LeadAgentCnt=lead_agent_cnt, @ProductID=product_id FROM insurance_file WHERE insurance_file_cnt = @InsuranceFileCnt  

 
SELECT @ConditionA = can_make_live_invoice FROM User_Authorities WHERE user_id = @UserID  

IF @LeadAgentCnt IS NULL OR RTRIM(@LeadAgentCnt)=''
BEGIN
    IF @COInsuranceLinkToAgent=1 AND (@BusinessTypeID=3 OR @BusinessTypeID=4 )
    BEGIN 
        SET @ConditionB=1                
    END
END
ELSE
BEGIN
    SELECT @ConditionB = can_make_live_invoice 
    FROM   Party_Agent 
    WHERE  party_cnt = @LeadAgentCnt     
END
IF @BusinessTypeID=1 
    SET @ConditionB=1

SELECT @ConditionC = can_make_live_invoice 
FROM   Product 
WHERE  product_id = @ProductID  

  
IF @ConditionA = 1  AND @ConditionB = 1  AND @ConditionC = 1  
 BEGIN  
    SELECT @InvoiceEnabled = 1  
 END  
ELSE  
 BEGIN  
   SELECT @InvoiceEnabled = 0  
 END  


SET @ConditionB=0

SELECT @ConditionA = can_make_live_instalments 
FROM   User_Authorities 
WHERE  user_id = @UserID  

IF @LeadAgentCnt IS NULL OR RTRIM(@LeadAgentCnt)=''
BEGIN
    IF @COInsuranceLinkToAgent=1 AND (@BusinessTypeID=3 OR @BusinessTypeID=4 )
    BEGIN 
        SET @ConditionB=1                
    END
END
ELSE
BEGIN
    SELECT @ConditionB = can_make_live_instalments 
    FROM   Party_Agent 
    WHERE  party_cnt = @LeadAgentCnt     
END
IF @BusinessTypeID=1 
    SET @ConditionB=1

SELECT @ConditionC = can_make_live_instalments 
FROM   Product 
WHERE  
product_id = @ProductID  
  
IF @ConditionA = 1  
 AND (@ConditionB = 1)  
 AND @ConditionC = 1  
 BEGIN  
   SELECT @InstalmentsEnabled = 1  
 END  
ELSE  
 BEGIN  
    SELECT @InstalmentsEnabled = 0  
 END  


SET @ConditionB=0

SELECT @ConditionA = can_make_live_paynow 
FROM User_Authorities WHERE user_id = @UserID 

IF @LeadAgentCnt IS NULL OR RTRIM(@LeadAgentCnt)=''
BEGIN
    IF @COInsuranceLinkToAgent=1 AND (@BusinessTypeID=3 OR @BusinessTypeID=4 )
    BEGIN 
        SET @ConditionB=1                
    END
END
ELSE
BEGIN
    SELECT @ConditionB = can_make_live_paynow 
    FROM   Party_Agent 
    WHERE  party_cnt = @LeadAgentCnt     
END
IF @BusinessTypeID=1 
    SET @ConditionB=1

SELECT @ConditionC = can_make_live_paynow FROM Product WHERE product_id = @ProductID  
  
IF @ConditionA = 1  
 AND @ConditionB = 1 
 AND @ConditionC = 1  
 BEGIN  
    SELECT @PaynowEnabled = 1  
 END  
ELSE  
 BEGIN  
    SELECT @PaynowEnabled = 0  
 END  
SELECT @ConditionA = ISNULL(can_make_live_BankGuarantee,0) 
FROM   User_Authorities 
WHERE  user_id = @UserID    
    
IF @LeadAgentCnt IS NULL OR RTRIM(@LeadAgentCnt)=''    
BEGIN    
    IF @COInsuranceLinkToAgent=1 AND (@BusinessTypeID=3 OR @BusinessTypeID=4 )    
  BEGIN    
        SET @ConditionB=1    
    END    
END    
ELSE    
BEGIN    
    SELECT @ConditionB = can_make_live_BankGuarantee 
    FROM   Party_Agent 
    WHERE  party_cnt = @LeadAgentCnt    
END    
IF @BusinessTypeID=1    
    SET @ConditionB=1    
    
SELECT @ConditionC = can_make_live_BankGuarantee 
FROM   Product 
WHERE  product_id = @ProductID    
    
IF @ConditionA = 1    
 AND @ConditionB = 1    
 AND @ConditionC = 1    
 BEGIN    
    SELECT @BankGuaranteeEnabled = 1    
 END    
ELSE    
 BEGIN    
    SELECT @BankGuaranteeEnabled = 0    
END    
    
SELECT InvoiceEnabled = @InvoiceEnabled, InstalmentsEnabled = @InstalmentsEnabled, PaynowEnabled = @PaynowEnabled, BankGuaranteeEnabled = @BankGuaranteeEnabled    
  
