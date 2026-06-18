SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_update_clmparty'
GO


CREATE PROCEDURE spu_update_clmparty
    @PartyID int,
    @PartyTypeID int,
    @Name varchar(30),
    @Address1 varchar(70),
    @Address2 varchar(70),
    @Address3 varchar(70),
    @Address4 varchar(70),
    @PostCode varchar(20),
    @PhoneNumber varchar(50),
    @FaxNumber varchar(50),
    @Sex int,
    @DOB datetime,
    @LicenseType int,
    @LicenseNumber varchar(50),
    @RegNumber varchar(50),
    @PartyStatus int
AS

/* DC 050601 added extra address fields */
Update Party_Claim
    Set
    Party_Claim.Name=@Name,Party_Claim.Address1=@Address1,Party_Claim.Address2=@Address2,
    Party_Claim.Address3=@Address3,Party_Claim.Address4=@Address4,
    Party_Claim.PostCode=@PostCode,Party_Claim.Sex=@Sex,
    Party_Claim.License_type=@LicenseType,Party_Claim.License_Number=@LicenseNumber,
    Party_Claim.Date_of_Birth=@DOB,Party_Claim.Party_Status=@PartyStatus,
    Party_Claim.Phone_number=@PhoneNumber, Party_Claim.Fax_Number=@FaxNumber,
    Party_Claim.Reg_Number=@RegNumber
    where
    Party_Claim.Party_Claim_id=@PartyID
GO


