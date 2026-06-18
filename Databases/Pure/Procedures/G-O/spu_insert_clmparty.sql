SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_insert_clmparty'
GO


CREATE PROCEDURE spu_insert_clmparty
    @PartyID int OUTPUT,
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
BEGIN

    If @DOB is Null
    Begin
        Insert party_Claim (Claim_Party_Type_id,name,address1,address2,address3,address4,postcode,phone_number,
        fax_number,sex,License_type,license_number,Reg_Number,Party_status)
        values (@PartyTypeID,@Name,@Address1,@Address2,@Address3,@Address4,@PostCode,@PhoneNumber,@FaxNumber,@Sex,@LicenseType,
        @LicenseNumber,@RegNumber,@PartyStatus)
    End
    Else
    Begin
        Insert party_Claim (Claim_Party_Type_id,name,address1,address2,address3,address4,postcode,phone_number,
        fax_number,sex,License_type,license_number,Date_of_Birth,Reg_Number,Party_status)
        values (@PartyTypeID,@Name,@Address1,@Address2,@Address3,@Address4,@PostCode,@PhoneNumber,@FaxNumber,@Sex,@LicenseType,
        @LicenseNumber,@DOB,@RegNumber,@PartyStatus)
    End

END

Begin

 Select @Partyid=@@IDENTITY

End
GO


