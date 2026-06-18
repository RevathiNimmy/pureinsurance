SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_complaint'
GO


CREATE PROCEDURE spu_wp_complaint
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


DECLARE
	@complaint_type_id int,
	@reference varchar(100),
	@date_opened datetime,
	@insurance_file_cnt int,
	@claim_id int,
	@date_settled datetime,
	@description varchar(8000),
	@fsa_complaint_method_id int,
	@long_complaint tinyint,
	@fsa_complaint_category_id int,
	@fsa_class_of_business_id int,
	@risk_group_id int,
	@party_cnt int,
	@party_handler_cnt int,
	@handler_id int,
	@contact varchar(255),
	@complaint_owner_id int,
	@complaint_upheld tinyint,
	@complaint_referred_to_fos tinyint,
	@compensation_paid money,
	@complaint_type varchar(255),
	@insurance_file_reference varchar(30),
	@claim_number varchar(30),
	@complaint_method varchar(100),
	@complaint_category varchar(100),
	@complaint_class_of_business varchar(100),
	@risk_group varchar(255),
	@client_name varchar(255),
	@member_of_staff varchar(255),
	@complaint_owner varchar(255)
	
IF @DocumentRef IS NOT NULL
	BEGIN

		SELECT
		@complaint_type_id=c.complaint_type_id,
		@complaint_type=CASE c.complaint_type_id WHEN 0 THEN 'Policy' WHEN 1 THEN 'Claim' WHEN 2 THEN 'General' END,
		@reference=c.reference,
		@date_opened=c.date_opened,
		@insurance_file_cnt=c.insurance_file_cnt,
		@claim_id=c.claim_id,
		@date_settled=c.date_settled,
		@description=SUBSTRING(c.description,1,8000),
		@complaint_method=cm.description,
		@long_complaint=c.long_complaint,
		@complaint_category=cc.description,
		@complaint_class_of_business=cob.description,
		@risk_group_id=c.risk_group_id,
		@client_name=p.resolved_name,
		@party_handler_cnt=c.party_handler_cnt,
		@handler_id=c.handler_id,
		@contact=c.contact,
		@complaint_owner_id=c.complaint_owner_id,
		@complaint_upheld=c.complaint_upheld,
		@complaint_referred_to_fos=c.complaint_referred_to_fos,
		@compensation_paid=c.compensation_paid
		FROM
		fsa_complaint_folder c
		INNER JOIN fsa_complaint_method cm ON c.fsa_complaint_method_id=cm.fsa_complaint_method_id
		INNER JOIN fsa_complaint_category cc ON c.fsa_complaint_category_id=cc.fsa_complaint_category_id
		INNER JOIN fsa_class_of_business cob ON c.fsa_class_of_business_id=cob.fsa_class_of_business_id
		INNER JOIN party p ON c.party_cnt=p.party_cnt
		WHERE c.reference=@DocumentRef
		
		if @insurance_file_cnt IS NOT NULL
			select @insurance_file_reference=i.insurance_ref FROM insurance_file i WHERE i.insurance_file_cnt=@insurance_file_cnt
		
		if @claim_id IS NOT NULL
			SELECT @claim_number=c.claim_number FROM claim c WHERE c.claim_id=@claim_id
		
		if @risk_group_id IS NOT NULL
			SELECT @risk_group=rg.description FROM risk_group rg WHERE rg.risk_group_id=@risk_group_id
			
		if @complaint_owner_id IS NOT NULL
			SELECT @complaint_owner=u.username FROM PMUser u WHERE u.user_id=@complaint_owner_id
			
		if @complaint_type_id=1
			SELECT @member_of_staff=h.description from handler h WHERE h.handler_id=@handler_id
		else
			SELECT @member_of_staff=u.username FROM PMUser u WHERE u.user_id=@party_handler_cnt
		
	END
	
	SELECT
		@complaint_type AS 'complaint_type',
		@reference AS 'complaint_reference',
		@date_opened AS 'complaint_opened',
		@insurance_file_reference AS 'insurance_file_reference',
		@claim_number AS 'claim_number',
		@date_settled AS 'complaint_settled',
		@description AS 'complaint_description',
		@complaint_method AS 'complaint_method',
		@long_complaint AS 'long_complaint',
		@complaint_category AS 'complaint_category',
		@complaint_class_of_business AS 'complaint_class_of_business',
		@risk_group AS 'complaint_risk_group',
		@client_name AS 'complaint_client_name',
		@member_of_staff AS 'complaint_member_of_staff',
		@contact AS 'complaint_contact',
		@complaint_owner AS 'complaint_owner',
		@complaint_upheld AS 'complaint_upheld',
		@complaint_referred_to_fos AS 'complaint_referred_to_fos',
		@compensation_paid AS 'complaint_compensation_paid'
		

    
            
GO


