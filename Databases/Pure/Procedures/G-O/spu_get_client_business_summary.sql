DDLDROPProcedure 'spu_get_client_business_summary'
GO
CREATE PROCEDURE spu_get_client_business_summary
@party_cnt int
AS
DECLARE @NoofPolicies int,
@NoofOpenClaims int,
@NoofCloseClaims int

	--select max(claim_id) from claim where policy_id in (Select insurance_file_cnt from insurance_file where insured_cnt=@party_cnt) GROUP BY base_claim_id
	Declare @Claim TABLE 
	(	Claim_id INT
	)
	
	INSERT INTO @Claim 
	SELECT MAX(Claim_ID) FROM Claim JOIN insurance_file ON Insurance_file.Insurance_file_cnt = Claim.Policy_ID 
	WHERE Insured_Cnt = @party_cnt AND ISNULL(is_dirty,0)=0 GROUP BY base_claim_id
	
Select @NoofPolicies=count(*) from insurance_folder where insurance_holder_cnt=@party_cnt

Select @NoofCloseClaims=count(*) from claim where claim_id in
                                                (
                                                Select Claim_id FROM @Claim --WHERE ISNULL(is_dirty,0)=0 
                                                )
                                                and claim_status_id in (3,5)

Select @NoofOpenClaims=count(*)  from claim where claim_id in
                                                (
                                                Select Claim_id FROM @Claim --WHERE ISNULL(is_dirty,0)=0 
                                                )
                                                and claim_status_id not in (3,5) and is_dirty<>1

SELECT @NoofPolicies NoofPolicies,@NoofOpenClaims NoofOpenClaims,@NoofCloseClaims NoofCloseClaims

