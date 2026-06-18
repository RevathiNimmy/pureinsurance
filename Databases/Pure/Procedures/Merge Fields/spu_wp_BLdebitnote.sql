SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_BLdebitnote'
GO

CREATE PROCEDURE spu_wp_BLdebitnote
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SET NOCOUNT ON

DECLARE @document_id INT
DECLARE @total_premium_inc_tax MONEY
DECLARE @total_premium_exc_tax MONEY
DECLARE @total_ins_premium_exc_tax MONEY
DECLARE @total_tax MONEY
DECLARE @shared INT
DECLARE @share FLOAT
DECLARE @fee_minus_tax MONEY

SELECT @shared = CHARINDEX('|', @DocumentRef)

If @shared = 0
BEGIN
    SELECT @share = 100
END
ELSE
BEGIN
    SELECT @share = CAST(RTRIM(SUBSTRING(@DocumentRef, @shared + 1, 25 - @shared)) AS FLOAT)

    SELECT @DocumentRef = SUBSTRING(@DocumentRef, 1, @shared - 1)
END


/*Get the document_id*/
SELECT 
    @document_id = d.document_id
FROM insurance_file i
JOIN document d
    ON d.insurance_file_cnt = i.insurance_file_cnt
    AND d.company_id = i.source_id
WHERE i.insurance_file_cnt = @InsuranceFileCnt
AND d.document_ref = @DocumentRef

/*Get the total premium including tax for any insurers, extras and fees on the transaction.*/
SELECT
    @total_ins_premium_exc_tax = ROUND(ABS(ISNULL(SUM(ROUND(td.amount, 2)), 0) - ISNULL(SUM(ROUND((CASE WHEN SIGN(td.amount) <> SIGN(td.ref_amount) THEN td.ref_amount * -1 ELSE td.ref_amount END), 2)), 0)) * @share / 100, 2)
FROM transdetail td 
JOIN transdetail_type tt 
    ON tt.transdetail_type_id = td.transdetail_type_id
    AND tt.code = 'GROSS'
JOIN account a 
    ON a.account_id = td.account_id
JOIN party p
    ON p.party_cnt = a.account_key
JOIN party_type pt
    ON pt.party_type_id = p.party_type_id
    AND pt.code = 'IN'
WHERE td.document_id = @document_id 

/*Get the total premium including tax for any insurers, extras and fees on the transaction.*/
SELECT
    @total_premium_inc_tax = ROUND(ABS(ISNULL(SUM(ROUND(td.amount, 2)), 0)) * @share / 100, 2)
FROM transdetail td 
JOIN transdetail_type tt 
    ON tt.transdetail_type_id = td.transdetail_type_id
    AND tt.code IN ('GROSS', 'FEE', 'CFEE', 'IFEE', 'DIS')
WHERE td.document_id = @document_id 


/*Get the total tax amount for all insurers and extras*/
SELECT
    @total_tax = ROUND(ABS(ISNULL(SUM(ROUND(CASE WHEN SIGN(td.amount) <> SIGN(td.ref_amount) THEN td.ref_amount * -1 ELSE td.ref_amount END, 2)), 0)) * @share / 100, 2)
FROM transdetail td 
JOIN transdetail_type tt 
    ON tt.transdetail_type_id = td.transdetail_type_id
    AND tt.code IN ('GROSS', 'FEE', 'CFEE', 'IFEE', 'DIS')
WHERE td.document_id = @document_id 

--Fee minus tax
SELECT @fee_minus_tax = abs(ISNULL(SUM(td.amount),0))
FROM transdetail td 	
	join transdetail_type tt on tt.transdetail_type_id = td.transdetail_type_id
where tt.code = 'FEE'  AND td.document_id=@Document_id

SELECT @total_premium_exc_tax = @total_ins_premium_exc_tax + @fee_minus_tax

SELECT 
    @total_tax 'TotalTaxValue',
    @total_premium_exc_tax 'SubTotalValue',
    @total_ins_premium_exc_tax 'PremiumMinusTax'
