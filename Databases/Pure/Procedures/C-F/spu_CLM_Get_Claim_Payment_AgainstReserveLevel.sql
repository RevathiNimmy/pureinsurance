EXECUTE DDLDropProcedure 'spu_CLM_Get_Claim_Payment_AgainstReserveLevel'
GO
CREATE PROCEDURE spu_CLM_Get_Claim_Payment_AgainstReserveLevel  
    @claim_peril_id int    

AS  
  
BEGIN
  
DECLARE @lPerilTypeID INT
SELECT @lPerilTypeID = peril_type_id FROM claim_peril WHERE claim_peril_id = @claim_peril_id

	SELECT  

	R.Reserve_id,
	RT.description ,
                R.this_payment,  
                TG.Code,    
                CP.Payment_Party_To,                  
                P.ShortName,                  
	MT.Code as 'MediaType',
	CP.Party_Bank_id,
	ISNULL(RT.Is_Excess,0) as 'Is_Excess',
	ISNULL(RT.Is_Indemnity,0) as 'Is_Indemnity' ,
	ISNULL(RT.Is_Expense,0) as 'Is_Expense',
	RT.Reserve_type_id,
	C.code as 'CurrencyCode',
	CP.media_ref,
	CP.claim_payment_to_id,
	CP.is_ex_gratia as 'is_ex_gratia'
  
        FROM    Reserve R  
        LEFT JOIN Reserve_type rt   
        ON R.reserve_type_id = rt.reserve_type_id   
        LEFT JOIN Claim_Payment CP  
        ON CP.Claim_Peril_ID = R.Claim_Peril_ID  
        LEFT JOIN Claim_Payment_Item CPI   
        ON R.Reserve_ID = CPI.Reserve_ID  
        LEFT JOIN Tax_Group TG   
        ON CPI.Tax_Group_id= TG.Tax_Group_ID  
        JOIN Party P ON P.Party_Cnt= CP.Party_Cnt   
        LEFT JOIN MediaType MT   
        ON MT.MediaType_id=CP.PayeeMediaType   
	LEFT JOIN Currency C
	on C.currency_id=CP.currency_id
        WHERE   R.claim_peril_id = @claim_peril_id    
	AND     rt.reserve_type_id IN (
			SELECT reserve_type_id FROM peril_type_reserve_type WHERE peril_type_id = @lPerilTypeID)

END  
Go