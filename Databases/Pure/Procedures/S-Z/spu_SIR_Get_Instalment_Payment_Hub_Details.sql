EXECUTE DDLDropProcedure 'spu_SIR_Get_Instalment_Payment_Hub_Details'
GO
CREATE PROCEDURE spu_SIR_Get_Instalment_Payment_Hub_Details  
    @nPFInstalmentsId INT 
AS  
BEGIN
	DECLARE @nPFPremFinanceCnt INT
	DECLARE @CompanyNo INT
	DECLARE @SchemeNo INT
	DECLARE @SchemeVersion INT
	DECLARE @TokenId VARCHAR(255)
	DECLARE @IntegerationId VARCHAR(255)
	DECLARE @MediaType VARCHAR(20)
	DECLARE @CurrencyCode VARCHAR(20)
	DECLARE @ClientId INT
	
	SELECT @nPFPremFinanceCnt=pfprem_finance_cnt 
	FROM PFInstalments 
	WHERE  pfinstalments_id=@nPFInstalmentsId

	SELECT @CompanyNo=CompanyNo ,@SchemeNo=SchemeNo,@SchemeVersion=SchemeVersion,
		   @TokenId=deposit_cc_tracking_number,@ClientId=ClientId,@IntegerationId='' 
	FROM PFPremiumFinance 
	WHERE  pfprem_finance_cnt=@nPFPremFinanceCnt
	 
    IF (@ClientId <>0 )  
    BEGIN  
		SELECT @TokenId=pb.cc_tracking_number,@IntegerationId = pb.manual_auth_number  
		FROM Party_Bank pb  
		INNER JOIN ACCOUNT a on pb.account_id =a.account_id  
		WHERE A.account_key =@ClientId AND cc_tracking_number=@TokenId  
	END 
	
	SELECT  
            @MediaType =mt.code,
			@CurrencyCode= c.code
    FROM  
        PFScheme s LEFT JOIN MediaType mt ON s.mediatype_id = mt.mediatype_id
		LEFT JOIN Currency c ON s.currency_id=s.currency_id
    WHERE  
        CompanyNo = @CompanyNo  
        AND SchemeNo = @SchemeNo  
        AND SchemeVersion = @SchemeVersion  
    ORDER BY  
        schemeno ASC,  
        schemeversion ASC  

 SELECT  
 @MediaType, 
 @CurrencyCode, 
 @IntegerationId,  
 @TokenId,
 @ClientId  

END  

