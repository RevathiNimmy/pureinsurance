SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_insurer'
go

CREATE PROCEDURE spu_wp_insurer @PartyCnt INT,
     @InsuranceFileCnt INT,
     @RiskID INT,
     @ClaimCnt INT,
     @DocumentRef VARCHAR(25),
     @Instance1 INT,
     @Instance2 INT,
     @Instance3 INT
AS

DECLARE
	@insurer_address1 VARCHAR(60),
	@insurer_address2 VARCHAR(60),
	@insurer_address3 VARCHAR(60),
	@insurer_address4 VARCHAR(60),
	@insurer_postal_code VARCHAR(20),
	@insurer_country VARCHAR(255),
	@address_usage_code VARCHAR(10),
	@insurer_name VARCHAR(60),
	@insurer_branch_address1 VARCHAR(60),
	@insurer_branch_address2 VARCHAR(60),
	@insurer_branch_address3 VARCHAR(60),
	@insurer_branch_address4 VARCHAR(60),
	@insurer_branch_postal_code VARCHAR(20),
	@insurer_branch_country VARCHAR(255),
	@insurer_agency_number Varchar(255),
	@insurer_fsa_status varchar(255),
	@insurer_fsa_registration_number varchar(255),
	@insurer_fsa_credit_rating varchar(255),
	@insurer_claims_rating_agency varchar(255),
	@insurer_claims_rating_text varchar(4000),
	@insurer_claims_rating_grading varchar(255),
	@insurer_claims_rating_date datetime,
	@insurer_claims_rating_description varchar(4000),
	@terms_of_payment varchar(255)
 
SELECT @address_usage_code = '3131 XCO'

IF ISNULL(@InsuranceFileCnt, 0) <> 0
BEGIN
    SELECT	@PartyCnt = insf.lead_insurer_cnt 
    FROM	insurance_file insf
    WHERE	insf.insurance_File_cnt = @InsuranceFileCnt
END

SELECT	@insurer_agency_number=pi.agency_number,
		@insurer_fsa_registration_number = pi.fsa_registration_number,
		@insurer_fsa_status = fis.description,
		@insurer_fsa_credit_rating = ficr.description,
		@insurer_claims_rating_agency = cra.description,
		@insurer_claims_rating_text = (SELECT TOP 1 crat.claims_rating_agency_text
			FROM claims_rating_agency_text crat
			WHERE crat.claims_rating_agency_id = pi.claims_rating_agency_id
			ORDER BY claims_rating_agency_text_id DESC),
		@insurer_claims_rating_grading = pi.claims_rating_grading,
		@insurer_claims_rating_date = pi.claims_rating_date,
		@insurer_claims_rating_description = pi.claims_rating_description
FROM	party_insurer pi
LEFT OUTER JOIN claims_rating_agency cra ON pi.claims_rating_agency_id = cra.claims_rating_agency_id
LEFT JOIN fsa_insurerstatus fis ON pi.fsa_insurerstatus_id = fis.fsa_insurerstatus_id
LEFT JOIN fsa_insurercreditrating ficr ON pi.fsa_insurercreditrating_id = ficr.fsa_insurercreditrating_id
WHERE	pi.party_cnt = @PartyCnt

EXEC spu_wp_get_address @PartyCnt,
	@InsuranceFileCnt,
	@ClaimCnt,
	@address_usage_code,
	@insurer_address1 OUTPUT,
	@insurer_address2 OUTPUT,
	@insurer_address3 OUTPUT,
	@insurer_address4 OUTPUT,
	@insurer_postal_code OUTPUT,
	@insurer_country OUTPUT
	
SELECT	@insurer_name = p.name,
		@terms_of_payment = pf.description
FROM	Party p
LEFT JOIN PFFrequency pf ON pf.pffrequency_id=p.payment_term_code
WHERE p.party_cnt = @PartyCnt
      
SELECT @address_usage_code = '3131 XBA'

EXEC spu_wp_get_insurer_address @PartyCnt,
	@InsuranceFileCnt,
	@ClaimCnt,
	@address_usage_code,
	@insurer_branch_address1 OUTPUT,
	@insurer_branch_address2 OUTPUT,
	@insurer_branch_address3 OUTPUT,
	@insurer_branch_address4 OUTPUT,
	@insurer_branch_postal_code OUTPUT,
	@insurer_branch_country OUTPUT

SELECT  'insurer_address1' = @insurer_address1,
		'insurer_address2' = @insurer_address2,
		'insurer_address3' = @insurer_address3,
		'insurer_address4' = @insurer_address4,
		'insurer_postal_code' = @insurer_postal_code,
		'insurer_country' = @insurer_country,
		'insurer_name' = @insurer_name,
		'insurer_branch_address1' = @insurer_branch_address1,
		'insurer_branch_address2' = @insurer_branch_address2,
		'insurer_branch_address3' = @insurer_branch_address3,
		'insurer_branch_address4' = @insurer_branch_address4,
		'insurer_branch_postal_code' = @insurer_branch_postal_code,
		'insurer_branch_country' = @insurer_branch_country,
		'Insurer_agency_number' = @insurer_agency_number,
		'insurer_fsa_status' = @insurer_fsa_status,
		'insurer_fsa_registration_number' = @insurer_fsa_registration_number,
		'insurer_fsa_credit_rating' = @insurer_fsa_credit_rating,
		'insurer_claims_rating_agency' = @insurer_claims_rating_agency,
		'insurer_claims_rating_text' = @insurer_claims_rating_text,
		'insurer_claims_rating_grading' = @insurer_claims_rating_grading,
		'insurer_claims_rating_date' = @insurer_claims_rating_date,
		'insurer_claims_rating_description' = @insurer_claims_rating_description,
		'terms_of_payment' = @terms_of_payment  
