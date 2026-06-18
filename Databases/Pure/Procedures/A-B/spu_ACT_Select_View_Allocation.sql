EXECUTE DDLDropProcedure 'spu_ACT_Select_View_Allocation'
GO

CREATE PROCEDURE spu_ACT_Select_View_Allocation
    @lTransDetailId INT,
    @iCancelPayment INT	= NULL,
    @iIncludeExtended BIT = 0  
	  
AS

IF ISNULL(@iCancelPayment,0) = 0  AND @iIncludeExtended= 0
BEGIN
SELECT
    ACC.account_name 'Account',
    AD.transdetail_id 'TransDetailId',
    TD.Accounting_date 'Trans. Date',
    A.allocation_date 'Allocated Date',
    AD.alloc_base_amount 'Allocated Amount',        
    AD.orig_base_amount 'Original Amount',          
    U.username 'User',
    DT.description 'Doc. Type',
    TD.insurance_ref 'Insurance Ref.',
    CLI.cashlistitem_id 'Cash List Item Id',
    AD.cashlistitem_id 'Allocation Cash List Item Id',
    A.allocation_id 'AllocationId', 
    AD.allocationdetail_id 'AllocationDetailId', 
    AD.write_off_amount 'Write_Off_Amount',
    AD.document_ref 'Document Ref',
    TD.Spare 'Claim_ref',
    AD.loss_gain_amount,
    AD.Round_off_amount,
	Curr.code 'CurrencyCode'

FROM
    AllocationDetail AD
JOIN
    Allocation A ON (A.allocation_id = AD.allocation_id)
JOIN
    PMUser U ON (U.user_id = A.user_id)
JOIN
    DocumentType DT ON (DT.documenttype_id = AD.documenttype_id)
JOIN
    TransDetail TD ON (TD.transdetail_id = AD.transdetail_id)
JOIN
    Account ACC ON (ACC.account_id = TD.account_id)
LEFT OUTER JOIN
    CashListItem CLI ON (CLI.TransDetail_Id = TD.TransDetail_Id)
LEFT OUTER JOIN Currency Curr on  ACC.currency_id = Curr.currency_id  				  
WHERE
    AD.allocation_id IN (SELECT
        allocation_id
    FROM
        allocationdetail ad2 
       
     WHERE
		ad2.transdetail_id = @lTransDetailId AND   ISNULL(ad2.is_reversed,0) = 0    
        )
    AND 
    ISNULL(ad.is_reversed,0) = 0

END
ELSE IF (ISNULL(@iCancelPayment,0) = 0  AND @iIncludeExtended = 1)
	BEGIN 
		SELECT  
			ACC.account_name 'Account',  
			AD.transdetail_id 'TransDetailId',  
			TD.Accounting_date 'Trans. Date',  
			A.allocation_date 'Allocated Date',  
			AD.alloc_base_amount 'Allocated Amount',        
			AD.orig_base_amount 'Original Amount',         
			U.username 'User',  
			DT.description 'Doc. Type',  
			TD.insurance_ref 'Insurance Ref.',  
			CLI.cashlistitem_id 'Cash List Item Id',  
			AD.cashlistitem_id 'Allocation Cash List Item Id',  
			A.allocation_id 'AllocationId',  
			AD.allocationdetail_id 'AllocationDetailId',  
			AD.write_off_amount 'Write_Off_Amount',  
			AD.document_ref 'Document Ref',  
			TD.Spare 'Claim_ref',  
			AD.loss_gain_amount,  
			AD.Round_off_amount,  
			Curr.code 'CurrencyCode' ,
			AD.transdetailex_id  'TransdetailExtendedKey',
			A.allocationbatch_id 'AllocationBatchId',
			AD.is_reversed 'IsReversed'
		  
		FROM  
			AllocationDetail AD  
		JOIN  
			Allocation A ON (A.allocation_id = AD.allocation_id)  
		JOIN  
			Allocation batch ON (batch.allocationbatch_id = A.allocationbatch_id)  			
		JOIN  
			PMUser U ON (U.user_id = A.user_id)  
		JOIN  
			DocumentType DT ON (DT.documenttype_id = AD.documenttype_id)  
		JOIN  
			TransDetail TD ON (TD.transdetail_id = AD.transdetail_id)  
		JOIN  
			Account ACC ON (ACC.account_id = TD.account_id)  
		LEFT OUTER JOIN  
			CashListItem CLI ON (CLI.TransDetail_Id = TD.TransDetail_Id)  
		LEFT OUTER JOIN Currency Curr on  ACC.currency_id = Curr.currency_id  
		WHERE  
			batch.allocation_id IN 
		    
		  (SELECT  
				allocation_id  
			FROM  
				allocationdetail ad2  
				
			WHERE  
				ad2.transdetail_id = @lTransDetailId 
	   	  )  
						  
	END 
ELSE
	BEGIN
	SELECT
		A.account_name,
		TD.transdetail_id,
		I.transaction_date,
		NULL,
		ABS(I.amount) AS AMOUNT,
		ABS(I.amount) AS AMOUNT,
		U.username,
		D.document_ref,
		TD.insurance_ref,
		I.cashlistitem_id,
		0,
		0,
		0,
		ABS(I.amount) AS AMOUNT,
		DT.description,
		'' -- TD.spare 
	FROM CashListItem I
		LEFT JOIN CashList C
				ON I.cashlist_id = C.cashlist_id
		LEFT JOIN TransDetail TD 
				ON I.TransDetail_ID = TD.Transdetail_id  
		LEFT JOIN cashlistitem_payment_type PT 
				ON  I.cashlistitem_payment_type_id = PT.cashlistitem_payment_type_id  
		JOIN Document D
				ON D.Document_id = TD.Document_id
		LEFT JOIN Account A 
				ON I.account_id = A.account_id 
		LEFT JOIN CashListItem_Payment_Status PS
				ON I.cashlistitem_payment_status_id = PS.cashlistitem_payment_status_id   
		LEFT JOIN CashListItem_Payment_Type CLIPT
				ON I.CashListItem_Payment_Type_id = CLIPT.CashListItem_Payment_Type_id    
		LEFT Join Party P 
				ON A.Account_key= P.Party_cnt
		LEFT JOIN PMUSER U 
				ON  I.pmuser_id = U.user_id         
		LEFT OUTER JOIN User_Authorities UA
				ON I.pmuser_id =  UA.user_id
		LEFT OUTER JOIN CLAIM CLM
				ON D.insurance_file_cnt = CLM.policy_id
		LEFT JOIN DocumentType DT ON DT.documenttype_id=D.documenttype_id
	WHERE    
		TD.transdetail_id = @lTransDetailId
END

GO
