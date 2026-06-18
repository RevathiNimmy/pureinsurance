SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_debitfeedetail'
GO

CREATE PROCEDURE spu_wp_debitfeedetail
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

BEGIN
	DECLARE @Taxamount numeric(19,4),
		@taxgroup varchar(255),
		@TaxGroupCode varchar(255),
		@SharedIndicator INT,
		@Share FLOAT,
		@EventInsuranceFileCnt INT

SELECT @SharedIndicator = CHARINDEX('|', @DocumentRef)

IF @SharedIndicator <> 0
BEGIN
    SELECT @Share = CONVERT(NUMERIC(15,11), RTRIM(SUBSTRING(@DocumentRef, @SharedIndicator + 1, 25 - @SharedIndicator)))
    SELECT @DocumentRef = SUBSTRING(@DocumentRef, 1, @SharedIndicator -1)
END
ELSE
BEGIN
	SET @Share = 100
END
	
SELECT @EventInsuranceFileCnt = event_log_id 
FROM transaction_export_folder TEF 
WHERE TEF.document_ref = @DocumentRef
AND TEF.insurance_ref = 
	(SELECT insurance_ref 
	FROM insurance_file 
	WHERE insurance_file_cnt = @InsuranceFileCnt)

DECLARE c_debitfee CURSOR SCROLL KEYSET READ_ONLY FOR

	 SELECT 	
		ROUND(isnull(epf.tax_amount,0) * (@Share/100),2),
		tg.Description,
	 	tg.code
	 FROM  
	 event_policy_fee epf  
	 INNER JOIN party p ON epf.party_cnt=p.party_cnt  
	 INNER JOIN party_type pt ON pt.party_type_id=p.party_type_id  
	 LEFT OUTER JOIN event_tax_calculation tc ON tc.policy_fee_id=epf.policy_fee_id  
	 LEFT OUTER JOIN tax_group tg ON tc.tax_group_id=tg.tax_group_id  
	 LEFT OUTER JOIN party_extra pe ON epf.party_cnt = pe.party_cnt  
	 WHERE  
	 epf.insurance_file_cnt=@EventInsuranceFileCnt  


OPEN c_debitfee

	FETCH ABSOLUTE @Instance1 FROM c_debitfee INTO
		@Taxamount,
		@taxgroup,
		@TaxGroupCode


	CLOSE c_debitfee
	DEALLOCATE c_debitfee

	SELECT 
		@Taxamount 'Taxamount',		
		@taxgroup 'taxgroup',
		@TaxGroupCode 'TaxGroupCode'
		

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO