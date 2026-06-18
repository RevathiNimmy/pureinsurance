DDLDropProcedure 'spu_PFEDI_GetPolicyDetails'
GO

CREATE PROCEDURE spu_PFEDI_GetPolicyDetails
(
	@InsuranceFileCnt INT
)
AS

DECLARE @TotalExtras currency
DECLARE @party_cnt int
DECLARE @fee_amount currency
DECLARE @isIPTable int
DECLARE @IPTrate currency

SELECT @TotalExtras = 0

DECLARE c_cursor SCROLL CURSOR FOR 
SELECT F.party_cnt, F.fee_amount, F.isIPTable 
FROM policy_fee F
JOIN party P ON P.party_cnt= F.party_cnt
JOIN party_type T ON t.party_type_id = P.party_type_id
WHERE T.code = 'EX'
AND F.insurance_file_cnt = @insurancefilecnt

OPEN	c_cursor
FETCH FIRST FROM c_cursor 
	INTO	@party_cnt,
		@fee_amount,
		@isIPTable

WHILE @@FETCH_STATUS = 0
BEGIN

	SELECT @TotalExtras = @TotalExtras + @fee_amount

	IF @isIPTable = 1
	BEGIN
		SELECT @IPTRate = 0

		IF EXISTS ( SELECT NULL FROM ipt_extras WHERE party_cnt = @party_cnt AND effective_date <= GetDate())
		BEGIN
			SELECT @IPTRate = rate 
			FROM ipt_extras 
			WHERE party_cnt = @party_cnt AND effective_date <= GetDate()
		END
		ELSE
		BEGIN
			SELECT @IPTRate = rate 
			FROM ipt 
			WHERE risk_code_id = ( 	SELECT risk_code_id 
						FROM insurance_file 
						WHERE insurance_file_cnt = @InsuranceFileCnt )
			AND effective_date <= GetDate()
		END

		IF @IPTRate <> 0 
		BEGIN
			SELECT @TotalExtras = @TotalExtras +
				ROUND((@fee_amount * ( @IPTRate / 100 )), 2)			
		END
	END

	FETCH NEXT FROM c_cursor 
	INTO	@party_cnt,
		@fee_amount,
		@isIPTable

END

CLOSE 		c_cursor
DEALLOCATE	c_cursor

SELECT DISTINCT
    I.insurance_ref,
    I.cover_start_date,
    I.expiry_date,
    I.renewal_date,
    PI.name,
    RC.description,
    PRD.description,
    s.last_trans_description,
    (I.this_premium + 									--premium
    ISNULL((SELECT SUM(F.fee_amount)							-- + fees
    FROM policy_fee F
    JOIN party P ON P.party_cnt= F.party_cnt
    JOIN party_type T ON t.party_type_id = P.party_type_id
    WHERE T.code = 'FE'
    AND F.insurance_file_cnt = I.insurance_file_cnt),0) +
    @TotalExtras - 									-- + extras (inc ipt)
    ISNULL((SELECT SUM(F.fee_amount)							-- + discounts
    FROM policy_fee F
    JOIN party P ON P.party_cnt= F.party_cnt
    JOIN party_type T ON t.party_type_id = P.party_type_id
    WHERE T.code = 'DI'
    AND F.insurance_file_cnt = I.insurance_file_cnt),0)) premium,
    ISNULL((SELECT SUM(F.fee_amount)
    FROM policy_fee F
    JOIN party P ON P.party_cnt= F.party_cnt
    JOIN party_type T ON t.party_type_id = P.party_type_id
    WHERE T.code = 'FE'
    AND F.insurance_file_cnt = I.insurance_file_cnt),0) fees,
    @TotalExtras,
    PI.abi_code_on_81 insurer_abi_number,
    RG.abi_code
FROM
            Insurance_File I
LEFT JOIN   Insurance_File_System S ON I.insurance_file_Cnt = S.insurance_file_cnt
INNER JOIN  Party PI ON PI.party_cnt=I.lead_insurer_cnt
INNER JOIN  Product PRD ON PRD.product_id=I.product_id
LEFT JOIN   Risk_Code RC ON RC.risk_code_id=I.risk_code_id
LEFT JOIN   Risk_Group RG ON RC.risk_group_id=RG.risk_group_id
WHERE       I.insurance_file_cnt = @InsuranceFileCnt
GO