SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_getBOdetails'
GO

Create Procedure spu_getBOdetails
    @PolicyLink int
AS

DECLARE 
    @IsSFU bit,
    @InsFileCnt int,
    @SchemeID int,
    @Agent varchar(50),
    @OldInsFileCnt int,
    @ParentInsFileCnt int,	
    @InceptionDate as datetime,
    @Postcode as varchar(50),
    @MTAReason as varchar(255),
    @Address1 Varchar(250),
    @Address2 Varchar(250),
    @Address3 Varchar(250),
    @Address4 Varchar(250),
    @FuturePremium as numeric(19,4),
    @NetPremium as numeric(19,4)

-- Find out if this is an Underwriting installation
IF EXISTS (SELECT * FROM hidden_options WHERE option_number = 1 AND Value = 'U')
    SELECT @IsSFU = 1
ELSE
    SELECT @IsSFU = 0

-- Get the insurance file as appropriate, also get the scheme ID while we're there
IF @IsSFU = 1
Begin
    -- For sfu insurance_file_cnt stores the folder so go via the risk to be sure
    SELECT
            @InsFileCnt = ifrl.insurance_file_cnt,
            @SchemeID =gis_scheme_id        -- Should not be necessary for SFU
        FROM gis_policy_link gpl
            JOIN insurance_file_risk_link ifrl
                ON ifrl.risk_cnt = gpl.risk_id
        WHERE gpl.gis_policy_link_id = @policylink

IF @InsFileCnt is Null --if calling from claims
    SELECT  
            @InsFileCnt = clm.Policy_id,
            @SchemeID =gis_scheme_id        -- Should not be necessary for SFU  
        FROM gis_policy_link gpl  
            JOIN Claim clm  
                ON clm.claim_id = gpl.claim_id
        WHERE gpl.gis_policy_link_id = @policylink                 
        

	--Return Net Premium and annual premium from previous policy version
	SET @ParentInsFileCnt = 0
	SET @NetPremium = 0.0000
	SET @FuturePremium = 0.0000
	Select @ParentInsFileCnt = insurance_file_cnt from insurance_file_risk_link 
	       where risk_cnt = (select Top 1 original_risk_cnt from insurance_file_risk_link where original_risk_cnt is not NULL And insurance_file_cnt = @InsFileCnt)
	IF @ParentInsFileCnt > 0 
		Select @NetPremium = net_premium,
			@FuturePremium = annual_premium
				FROM insurance_file Where insurance_file_cnt = @ParentInsFileCnt


-- Get the agent    
SELECT @Agent = P.[name]
    FROM insurance_file I
        JOIN Party P
            ON I.lead_agent_cnt=P.Party_cnt
    WHERE I.insurance_file_cnt = @InsFileCnt

-- If scheme is null check for old scheme
IF @schemeID is null
BEGIN
    SELECT 	
            @SchemeID = gold.gis_Scheme_ID,
            @OldInsFileCnt = gold.insurance_file_cnt
        FROM gis_policy_link gold
            JOIN insurance_file_risk_link rold
                ON rold.insurance_file_cnt = gold.insurance_file_cnt
            JOIN insurance_file_risk_link rnew
                ON rnew.original_risk_cnt=rold.risk_cnt
            JOIN gis_policy_link gnew
                ON rnew.insurance_file_cnt = gnew.insurance_file_cnt
        WHERE gnew.gis_policy_link_id=@policyLink
END

-- Get address details
SELECT 
        @Postcode= a.postal_code,
        @Address1 = a.address1,
        @Address2 = a.address2,
        @Address3 = a.address3,
        @Address4 = a.address4
    FROM insurance_file i
        JOIN Party_address_usage u
            ON i.insured_cnt=u.party_cnt
        JOIN Address a
            ON u.address_cnt=a.address_cnt
    WHERE i.insurance_file_cnt=@InsFileCnt

-- Get the inception date
    SELECT @InceptionDate = ifo.inception_date
        FROM insurance_file ifi
            JOIN insurance_folder ifo
                ON ifo.insurance_folder_cnt = ifi.insurance_folder_cnt
        WHERE ifi.insurance_file_cnt = @InsFileCnt

-- Get mta reason
SELECT @MTAreason = m.Description
    FROM insurance_file i
        JOIN MTA_Reason m
            ON  i.user_defined_data_id=m.MTA_Reason_ID
        JOIN gis_policy_link g
            ON i.insurance_file_cnt=g.insurance_file_cnt
    WHERE (g.gis_policy_link_id =@policylink)


-- Return all the data we've gathered with a little extra
SELECT
        CASE @NetPremium
        WHEN 0 THEN
			i.net_premium
		ELSE
			@NetPremium	
		END,
        i.cover_start_date,
        i.expiry_date,
        i.cover,
        t.description as [Insurance_File_Type.decription],
        s.description as [Insurance_File_Status.decription],
        @schemeID as schemeId,
        @Agent as Agent,
        @InceptionDate as InceptionDate,
        @Postcode as PostCode,
        CASE @FuturePremium 
        WHEN 0 THEN
			i.annual_premium
        ELSE
			@FuturePremium
		END
        as FuturePremium,
        @MTAReason as MTAReason,
        @Address1 as address1,
        @Address2 as address2,
        @Address3 as address3,
        @Address4 as address4,
        i.renewal_date as renewalDate,
        t.code as [Insurance_File_Type.code]
    FROM Insurance_File i
        JOIN  Insurance_File_Type t
            ON i.insurance_file_type_id = t.insurance_file_type_id
        LEFT JOIN  Insurance_File_Status s
            ON i.insurance_file_status_id = s.insurance_file_status_id
    WHERE i.insurance_file_cnt = @InsFileCnt
END
ELSE
BEGIN
	SELECT @Agent=P.[name]
	FROM insurance_file I
		INNER JOIN Party P
		ON I.lead_agent_cnt=P.Party_cnt
		INNER JOIN gis_policy_link g
		ON i.insurance_file_cnt=g.insurance_file_cnt

	WHERE     (g.gis_policy_link_id =@policylink)
	SELECT @SchemeID =gis_scheme_id
	FROM gis_policy_link
	WHERE gis_policy_link_ID=@policylink
	IF @schemeID is null
	BEGIN

		SELECT 	@SchemeID=gold.gis_Scheme_ID,
			@OldInsFileCnt=gold.insurance_file_cnt

		FROM gis_policy_link gold

		INNER JOIN insurance_file_risk_link rold
		ON rold.insurance_file_cnt = gold.insurance_file_cnt
		INNER JOIN insurance_file_risk_link rnew
		ON rnew.original_risk_cnt=rold.risk_cnt
		INNER JOIN gis_policy_link gnew
		ON rnew.insurance_file_cnt = gnew.insurance_file_cnt
		WHERE gnew.gis_policy_link_id=@policyLink
	END
	SELECT @InsFileCnt =insurance_file_cnt
	FROM gis_policy_link
	WHERE gis_policy_link_id=@policyLInk
	SELECT @Postcode= a.postal_code
	, @Address1 = a.address1
	, @Address2 = a.address2
	, @Address3 = a.address3
	, @Address4 = a.address4
	FROM insurance_file i
		INNER JOIN Party_address_usage u
		ON i.insured_cnt=u.party_cnt
		INNER JOIN Address a
		ON u.address_cnt=a.address_cnt
	WHERE i.insurance_file_cnt=@InsFileCnt

	SELECT @InceptionDate = cover_start_date
	FROM  Insurance_file
	WHERE Insurance_file_cnt=@OldInsFileCnt

	SELECT @MTAreason= m.Description
	FROM insurance_file i
	INNER JOIN MTA_Reason m
		ON  i.user_defined_data_id=m.MTA_Reason_ID
	INNER JOIN gis_policy_link g
		ON i.insurance_file_cnt=g.insurance_file_cnt
	WHERE     (g.gis_policy_link_id =@policylink)
	SELECT @FuturePremium=annual_premium
	FROM insurance_file i
	INNER JOIN gis_policy_link g
		ON i.insurance_file_cnt=g.insurance_file_cnt
	WHERE     (g.gis_policy_link_id =@policylink)
	SELECT   i.net_premium,
		i.cover_start_date,
		i.expiry_date,
		i.cover,
		t.description as [Insurance_File_Type.decription],
		s.description as [Insurance_File_Status.decription],
		@schemeID as schemeId,
		@Agent as Agent,
		@InceptionDate as InceptionDate,
		@Postcode as PostCode,
		@FuturePremium as FuturePremium,
		@MTAReason as MTAReason,
		@Address1 as address1,
		@Address2 as address2,
		@Address3 as address3,
		@Address4 as address4,
		i.renewal_date as renewalDate,
		t.code as [Insurance_File_Type.code],
		p.resolved_name,
		'',
		pp.date_of_birth
	FROM     Insurance_File i
		INNER JOIN  Insurance_Folder f
		ON i.insurance_folder_cnt = f.insurance_folder_cnt

		INNER JOIN  Insurance_File_Type t
		ON i.insurance_file_type_id = t.insurance_file_type_id
		LEFT OUTER JOIN  Insurance_File_Status s
		ON i.insurance_file_status_id = s.insurance_file_status_id
		INNER JOIN Insurance_file_risk_link r
		ON i.insurance_file_cnt = r.insurance_file_cnt
		INNER JOIN gis_policy_link g
		ON i.insurance_file_cnt=g.insurance_file_cnt
		INNER JOIN Party P ON P.party_cnt = I.insured_cnt
		LEFT OUTER JOIN Party_Lifestyle PP ON PP.party_cnt = P.party_cnt
	WHERE     (g.gis_policy_link_id =@policylink)
END



GO


