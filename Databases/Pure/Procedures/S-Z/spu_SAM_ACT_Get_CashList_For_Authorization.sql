GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_ACT_Get_CashList_For_Authorization'
GO

CREATE PROCEDURE spu_SAM_ACT_Get_CashList_For_Authorization
@PayeeName VARCHAR(200)=NULL,
@CreatedBy VARCHAR(100)=NULL,
@Branch VARCHAR(100)=NULL,
@CashListItemId int=NULL,
@Date_From DATETIME= NULL,
@Date_To DATETIME=NULL,
@AssignedTo VARCHAR(100) = NULL,
@PaymentType VARCHAR(100) = NULL
AS

IF @AssignedTo ='All Group'
SELECT @AssignedTo = NULL

IF @Branch ='(All Branches)'
SELECT @Branch = NULL

IF @PaymentType  = 'All'
SELECT @PaymentType = NULL

DECLARE @kPaymentPendingApproval INT = 7 

BEGIN

	SELECT distinct key_value Cashlistitem_id ,pti.pmwrk_task_instance_cnt pmwrk_task_instance_cnt, pmug.description,pti.date_created,
	ROW_NUMBER() OVER(PARTITION BY key_value ORDER BY pti.pmwrk_task_instance_cnt DESC) rowNum 
	INTO #TEMP 
	FROM  pmwrk_task_inst_key ptik    
	JOIN PMWrk_Task_Instance pti on pti.pmwrk_task_instance_cnt=ptik.pmwrk_task_instance_cnt  
	JOIN PMUser_Group PMUG on PMUG.pmuser_group_id=pti.pmuser_group_id WHERE pmnav_key_id=6 and key_value<>0    
	
	
    SELECT CL.cashlist_id,    
               CLI.cashListItem_id,    
			   t.pmwrk_task_instance_cnt,
      C.Description Branch ,    
      CU.code CurrencyCode,    
      b.bank_account_name Bank_Account,    
      CLI.transaction_date,    
      CLPT.description Payment_type,    
      M.description media_type,    
      cli.media_ref,    
      i.insurance_ref policy_ref ,    
      CLM.Claim_Number Claim_ref,    
      ISNULL(p.resolved_name,a.account_name) account_name,    
      cli.amount,    
      pu.username,    
      CASE ISNULL(PA.No_of_steps,0) WHEN 0 THEN cps.description ELSE CASE pa1.approved WHEN 0 THEN 'Declined at step ' ELSE 'Approved at step ' END + CAST(No_of_steps AS CHAR)  END CurrentStatus  ,    
      t.description Assigned_to,    
      t.date_created Assigned_date,    
      CAST(ROUND(cli.currency_base_xrate*cli.amount,2) AS NUMERIC(19,2)) base_currency_amount    
      FROM cashList CL JOIN CashListItem CLI ON CL.cashlist_id=CLI.cashlist_id    
    JOIN mediatype M ON M.mediatype_id=CLI.mediatype_id    
    JOIN cashlistitem_payment_type CLPT ON  CLPT.cashlistitem_payment_type_id=CLI.cashlistitem_payment_type_id    
    JOIN account a ON a.account_id=CLI.account_id    
    LEFT JOIN party p ON p.party_cnt=a.account_key    
    JOIN bankaccount b ON b.bankaccount_id=CL.bankaccount_id    
    JOIN Company c ON c.company_id=cl.company_id    
    JOIN Pmuser pu ON pu.user_id=cli.pmuser_id    
    LEFT JOIN CashListItem_Claim_Link CCL on CLI.cashListItem_id =CCL.cashListItem_id    
    LEFT JOIN claim_payment CP on cp.claim_payment_id=CCL.claim_payment_id    
    LEFT JOIN claim clm on clm.claim_id=cp.claim_id    
    LEFT JOIN Insurance_File i on i.cashlistitem_id=cli.cashlistitem_id    
    LEFT JOIN Currency CU ON CU.Currency_Id = CL.currency_id    
    left JOIN   
 (SELECT payment_cnt,COUNT(payment_cnt) No_of_steps,  
 MAX(approval_cnt) approval_cnt FROM payment_approval GROUP BY payment_cnt) pa ON pa.payment_cnt=CLI.cashlistitem_id    
    LEFT JOIN payment_approval pa1 ON pa1.approval_cnt=pa.approval_cnt  
	JOIN #TEMP t  on t.Cashlistitem_id=clI.cashlistitem_id and rowNum=1 
    LEFT JOIN CashListItem_Payment_Status cps ON cps.cashlistitem_payment_status_id=cli.cashlistitem_payment_status_id    
    WHERE (@kPaymentPendingApproval IS NULL OR cli.cashlistitem_payment_status_id=@kPaymentPendingApproval) --Pending    
    AND (@PayeeName IS NULL OR ISNULL(p.resolved_name,a.account_name) LIKE @PayeeName)    
    AND (@CreatedBy IS NULL OR pu.username=@CreatedBy)    
    AND (@Branch IS NULL OR c.code=@Branch)    
    AND (@CashListItemId IS NULL OR CLI.cashlistitem_id= @CashListItemId )    
    AND (@Date_From IS NULL or transaction_date >=@Date_From )    
    AND (@Date_To IS NULL or transaction_date <=@Date_to )    
    AND (@AssignedTo IS NULL OR t.description= @AssignedTo )    
    AND (@PaymentType IS NULL OR CLPT.code = @PaymentType)   
 ORDER BY cli.transaction_date DESC    
 DROP TABLE #TEMP
      
END 