SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_get_address'
GO

CREATE PROCEDURE spu_wp_get_address
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @address_usage_code VARCHAR(10),
    @address1 VARCHAR(60) OUTPUT,
    @address2 VARCHAR(60) OUTPUT,
    @address3 VARCHAR(60) OUTPUT,
    @address4 VARCHAR(60) OUTPUT,
    @postal_code VARCHAR(20) OUTPUT,
    @country VARCHAR(255) OUTPUT,
    @Address5 VARCHAR(60) =NULL OUTPUT,  
    @Address6 VARCHAR(60) =NULL OUTPUT,  
    @Address7 VARCHAR(60) =NULL OUTPUT,  
    @Address8 VARCHAR(60) =NULL OUTPUT,  
	@Address9 VARCHAR(60)  =NULL OUTPUT,  
    @Address10 VARCHAR(60) =NULL OUTPUT
AS
BEGIN
/*
    PWF 01/11/2001 - Updated
        Removed cursor creation in favour of straight select
        Changed joins to ANSI standard
        Added convert to address_id comparison to avoid datatype conflicts

    Notes:
        The reason for the postal_code/address_id comparison: The postal_code
        field is mandatory, however certain countries do not use a post code
        system (eg Bahamas). To allow for this the address_id is written to
        the postal_code field. When the address is returned this needs to be
        stripped back out of the data.
*/
    DECLARE @risk_group INT

    IF @insuranceFileCnt IS NOT NULL BEGIN
        SELECT @risk_group = RG.risk_group_id
            FROM Risk_Group AS RG
            INNER JOIN Risk_Code AS RC ON RC.risk_group_id = RG.risk_group_id
            INNER JOIN Insurance_File AS I ON I.risk_code_id = RC.risk_code_id
            WHERE i.insurance_File_Cnt = @insuranceFileCnt
    END ELSE BEGIN
        SELECT @risk_group = NULL
    END

    IF @risk_group IS NOT NULL BEGIN
        SELECT
            @address1 = a.address1,
            @address2 = a.address2,
            @address3 = a.address3,
            @address4 = a.address4,
            @postal_code = (
                CASE WHEN a.postal_code = CONVERT(varchar(20), a.address_id) THEN ''
                ELSE a.postal_code
                END
            ),
            @country = c.description,
            @Address5 = a.address5,
			@Address6 = a.address6,
			@Address7 = a.address7,
			@Address8 = a.address8,
			@Address9 = a.address9,
			@Address10 = a.address10
            FROM Party_Address_Usage AS pau
            INNER JOIN Address AS a ON pau.address_cnt = a.address_cnt
            INNER JOIN Address_Usage_Type AS aut ON pau.address_usage_type_id = aut.address_usage_type_id
            LEFT OUTER JOIN Party_Address_Risk_Link AS parl ON pau.address_cnt = parl.address_cnt
                                                            AND parl.party_cnt = @PartyCnt
                                                            AND parl.risk_group_id = @risk_group
            LEFT OUTER JOIN Country AS c ON a.country_id = c.country_id
            WHERE pau.party_cnt = @PartyCnt
            AND aut.code = @address_usage_code
    END ELSE BEGIN
        -- PW210602 - Remove the join to the Party_Address_Risk_Link table in the following,
        -- section, as it was there to ensure that the correct address for the Risk Group
        -- was being returned. However, we have no Risk Group if we are in this code branch.
        -- This was preventing an address from being returned if there were no
        -- Party_Address_Risk_Link records.
        SELECT
            @address1 = a.address1,
            @address2 = a.address2,
            @address3 = a.address3,
            @address4 = a.address4,
            @postal_code = (
                CASE WHEN a.postal_code = CONVERT(varchar(20), a.address_id) THEN ''
                ELSE a.postal_code
                END
            ),
            @country = c.description,
            @Address5 = a.address5,
			@Address6 = a.address6,
			@Address7 = a.address7,
			@Address8 = a.address8,
			@Address9 = a.address9,
			@Address10 = a.address10 
            FROM Party_Address_Usage AS pau
            INNER JOIN Address AS a ON pau.address_cnt = a.address_cnt
            INNER JOIN Address_Usage_Type AS aut ON pau.address_usage_type_id = aut.address_usage_type_id
            LEFT OUTER JOIN Country AS c ON a.country_id = c.country_id
            WHERE pau.party_cnt = @PartyCnt
            AND aut.code = @address_usage_code
    END
END
GO
