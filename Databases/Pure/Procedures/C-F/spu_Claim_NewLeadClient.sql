SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Claim_NewLeadClient'
GO

CREATE PROC spu_Claim_NewLeadClient
    @Party_cnt INT,
    @Insurance_File_cnt INT
as
    DECLARE @Address_cnt INT
    DECLARE @Client_short_name CHAR(20)
    DECLARE @Resolved_Name VARCHAR(100)
    DECLARE @Insurance_ref VARCHAR(30)

    --Get the address of the new lead client
    SELECT @Address_cnt = MIN(Address_cnt)
    FROM Party_Address_Usage pau, Address_Usage_Type aut
    WHERE pau.address_usage_type_id = aut.address_usage_type_id
    AND pau.Party_cnt = @Party_cnt
    AND aut.Code LIKE '%XCO%'

    --Get the other details for the new lead client
    --From the Party table
    SELECT 
        @Client_short_name = shortname,
        @Resolved_Name = Resolved_Name
    FROM Party
    WHERE Party_cnt = @Party_cnt
    
	--Get the policy number for matching up Claims
    SELECT @Insurance_Ref = Insurance_Ref
    FROM Insurance_File 
    WHERE Insurance_File_Cnt = @Insurance_File_Cnt

    --Update the claim iwth the new lead client details
	UPDATE Claim
    SET 
       Client_short_name = @Client_short_name,
       Client_name = @Resolved_name,
       Client_Address = @Address_cnt
    FROM Claim c
    WHERE Policy_Number = @Insurance_Ref

go
