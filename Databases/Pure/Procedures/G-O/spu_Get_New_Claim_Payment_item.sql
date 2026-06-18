EXECUTE DDLDropProcedure 'spu_Get_New_Claim_Payment_item'
GO
CREATE PROCEDURE spu_Get_New_Claim_Payment_item  
	@nClaimPerilID INT,
	@nReserveTypeID INT
AS  

BEGIN

	SELECT
	  
	R.Reserve_id,
	RT.description ,		
	0.00 as 'this_payment',
	'' as 'Code',
	'' as 'Payment_Party_To',
	'' as 'ShortName',
	'' as 'MediaType',
	0  as 'Party_Bank_id',
	ISNULL(RT.Is_Excess,0) as 'Is_Excess',
	ISNULL(RT.Is_Indemnity,0) as 'Is_Indemnity' ,
	ISNULL(RT.Is_Expense,0) as 'Is_Expense',
	RT.Reserve_type_id,
	'' as 'CurrencyCode',
	'' as 'MediaRef',
	NULL as 'claim_payment_to_id',
	0 as 'is_ex_gratia'

	FROM    Reserve  R with (Nolock)
	LEFT JOIN Reserve_type RT with (Nolock)
	ON R.reserve_type_id = rt.reserve_type_id
	WHERE   R.claim_peril_id = @nClaimPerilID
	AND R.Reserve_type_id =@nReserveTypeID
		
END  

Go
