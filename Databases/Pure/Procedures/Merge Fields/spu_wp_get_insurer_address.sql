SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_get_insurer_address'
GO


CREATE PROCEDURE spu_wp_get_insurer_address
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @address_usage_code VARCHAR(10),
    @address1 VARCHAR(60) OUTPUT,
    @address2 VARCHAR(60) OUTPUT,
    @address3 VARCHAR(60) OUTPUT,
    @address4 VARCHAR(60) OUTPUT,
    @postal_code VARCHAR(20) OUTPUT,
    @country VARCHAR(255) OUTPUT

AS
BEGIN
/*
    Notes:
        The reason for the postal_code/address_id comparison: The postal_code
        field is mandatory, however certain countries do not use a post code
        system (eg Bahamas). To allow for this the address_id is written to
        the postal_code field. When the address is returned this needs to be
        stripped back out of the data.
*/

	/* DC291004 PN11127 -start*/
	SET NOCOUNT ON

	DECLARE @PolicyRiskGroupId int

	SELECT @PolicyRiskGroupId =
				(
				SELECT rc.risk_group_id
				FROM insurance_file i
				JOIN risk_code rc
				ON rc.risk_code_id = i.risk_code_id
				WHERE i.insurance_file_cnt = @InsuranceFileCnt
			)
	/* DC291004 PN11127 -end*/

	DECLARE @Insurer_cnt int

	SELECT @Insurer_cnt = lead_insurer_cnt
	FROM insurance_file
	WHERE insurance_file_cnt = @InsuranceFileCnt

	/* DC291004 PN11127 -start*/
	IF ((	SELECT 	count(*)
		FROM 	party_address_risk_link
		WHERE 	party_cnt = @Insurer_Cnt
		AND	risk_group_id = @PolicyRiskGroupId
	) > 0
	AND /* DC051104 PN11127 check if any branch addresses are set up */
	(	SELECT count(*)
		FROM party_address_usage
		WHERE party_cnt = @PartyCnt
		AND address_usage_type_id = ( SELECT address_usage_type_id FROM address_usage_type WHERE code = @address_usage_code)
	) > 0) AND @address_usage_code = '3131 XBA'
	BEGIN
	/* DC291004 PN11127 -end*/

	/* Risk Specific Branch Address */
		SELECT	@address1 = a.address1,
			@address2 = a.address2,
			@address3 = a.address3,
			@address4 = a.address4,
			@postal_code =
			(
				CASE
				WHEN a.postal_code = CONVERT(varchar(20), a.address_id) THEN ''
				ELSE a.postal_code
				END
			),
			@country = c.description
		FROM Party_Address_Usage AS pau
		JOIN Address AS a
		ON pau.address_cnt = a.address_cnt
		JOIN Address_Usage_Type AS aut
		ON pau.address_usage_type_id = aut.address_usage_type_id
		LEFT OUTER JOIN Country AS c
		ON a.country_id = c.country_id		
		LEFT OUTER JOIN Party_Address_Risk_Link parl
		ON parl.party_cnt = pau.party_cnt
		AND parl.address_cnt = a.address_cnt
		/*AND parl.risk_group_id =
			(
				SELECT rc.risk_group_id
				FROM insurance_file i
				JOIN risk_code rc
					ON rc.risk_code_id = i.risk_code_id
				WHERE i.insurance_file_cnt = @InsuranceFileCnt
			)*/
		WHERE pau.party_cnt = @Insurer_cnt
		AND aut.code = @address_usage_code
		AND parl.risk_group_id =
			(
				SELECT rc.risk_group_id
				FROM insurance_file i
				JOIN risk_code rc
					ON rc.risk_code_id = i.risk_code_id
				WHERE i.insurance_file_cnt = @InsuranceFileCnt
			)
		--OR parl.risk_group_id IS NULL
		ORDER BY parl.address_cnt ASC

	/* DC291004 PN11127 -start*/
	END
	ELSE
	BEGIN
		/* non risk speocific branch address */
		SELECT	@address1 = a.address1,
			@address2 = a.address2,
			@address3 = a.address3,
			@address4 = a.address4,
			@postal_code =
			(
				CASE
				WHEN a.postal_code = CONVERT(varchar(20), a.address_id) THEN ''
				ELSE a.postal_code
				END
			),
			@country = c.description
		FROM Party_Address_Usage AS pau
		JOIN Address AS a
		ON pau.address_cnt = a.address_cnt
		JOIN Address_Usage_Type AS aut
		ON pau.address_usage_type_id = aut.address_usage_type_id
		LEFT OUTER JOIN Country AS c
		ON a.country_id = c.country_id		
		WHERE pau.party_cnt = @Insurer_cnt
		AND aut.code = @address_usage_code
                           AND NOT EXISTS ( SELECT * FROM party_address_risk_link WHERE party_cnt = @partycnt )

		/* DC051104 PN11127 get correspondence address if no branch address */	
		IF @address1 IS NULL AND @address2 IS NULL AND @address3 IS NULL AND @address4 IS NULL AND @postal_code IS NULL
		BEGIN
			SELECT	@address1 = a.address1,
				@address2 = a.address2,
				@address3 = a.address3,
				@address4 = a.address4,
				@postal_code =
				(
					CASE
					WHEN a.postal_code = CONVERT(varchar(20), a.address_id) THEN ''
					ELSE a.postal_code
					END
				),
				@country = c.description
			FROM Party_Address_Usage AS pau
			JOIN Address AS a
			ON pau.address_cnt = a.address_cnt
			JOIN Address_Usage_Type AS aut
			ON pau.address_usage_type_id = aut.address_usage_type_id
			LEFT OUTER JOIN Country AS c
			ON a.country_id = c.country_id		
			WHERE pau.party_cnt = @Insurer_cnt
			AND aut.code = '3131 XCO'
		

		END
	END
  	/* DC291004 PN11127 -end*/
END
GO
