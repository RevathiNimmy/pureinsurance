EXECUTE DDLDropProcedure 'spu_Get_Initial_Policy_Payment_Terms'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_Get_Initial_Policy_Payment_Terms
	@insurance_file_cnt int
AS
DECLARE @insurance_folder_cnt int
DECLARE @init_payment_method varchar(60)
DECLARE @init_insurance_file_cnt int
DECLARE @latest_instalment_policy_version int
DECLARE @Renewal_version int

SELECT 
	@insurance_folder_cnt = insurance_folder_cnt 
FROM 
	insurance_file 
WHERE 
	insurance_file_cnt = @insurance_file_cnt

IF EXISTS(SELECT * 
			FROM insurance_file inf 
			INNER JOIN insurance_file_status ifs ON inf.insurance_file_status_id = ifs.insurance_file_status_id
			WHERE ifs.code = 'REP' AND inf.insurance_folder_cnt = @insurance_folder_cnt)
BEGIN
	--The policy has been renewed, look for latest renewal
	SELECT 
		@Renewal_version = MAX(policy_version) + 1
	FROM 
		insurance_file inf 
	INNER JOIN 
		insurance_file_status ifs ON inf.insurance_file_status_id = ifs.insurance_file_status_id
	WHERE 
		ifs.code = 'REP' AND inf.insurance_folder_cnt = @insurance_folder_cnt
END
ELSE
BEGIN
	--The policy has not been renewed. Version 1 is the correct version
	SELECT @Renewal_version = 1
END

--Get payment method from policy version
SELECT 
	@init_payment_method = payment_method,
	@init_insurance_file_cnt = insurance_file_cnt
FROM 
	insurance_file 
WHERE 
	insurance_folder_cnt = @insurance_folder_cnt
AND
	policy_version = @Renewal_version

--Get version of policy with the latest version of the instalments
SELECT 
	@latest_instalment_policy_version = MAX(pfp1.insurance_file_cnt)
FROM 
	PFPremiumFinance pfp1
INNER JOIN
	PFPremiumFinance pfp2 ON pfp1.pfprem_finance_cnt = pfp2.pfprem_finance_cnt 
WHERE
	pfp2.insurance_file_cnt = @init_insurance_file_cnt
	

SELECT	@init_payment_method Initial_Payment_Method, 
		@latest_instalment_policy_version Latest_Instalment_Policy_Version

GO
