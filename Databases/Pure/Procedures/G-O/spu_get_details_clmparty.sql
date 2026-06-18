SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_details_clmparty'
GO


CREATE PROCEDURE spu_get_details_clmparty
    @PartyID int,
    @PartyTypeID int
AS

/* DC050601 added extra fields for address, i.e. address2, address3, address4 & postcode & renamed address to address1 */
If @PartyTypeID =0
    Begin
        Select
        Party_Claim.Party_Claim_id,Party_Claim.Claim_Party_type_id,Party_Claim.Name,
        Party_Claim.Address1,Party_Claim.Address2,Party_Claim.Address3,Party_Claim.Address4,Party_Claim.PostCode,Party_Claim.Sex,Party_Claim.License_type,
        Party_Claim.License_Number,Party_Claim.Date_of_Birth,Party_Claim.Party_Status,
        Party_Claim.Phone_number,Party_Claim.Fax_Number,Party_Claim.Reg_Number
        from party_claim where Party_Claim.Party_Claim_id=@PartyID
    End
    Else
    Begin
        Select
        Party_Claim.Party_Claim_id,Party_Claim.Claim_Party_type_id,Party_Claim.Name,
        Party_Claim.Address1,Party_Claim.Address2,Party_Claim.Address3,Party_Claim.Address4,Party_Claim.PostCode,Party_Claim.Sex,Party_Claim.License_type,
        Party_Claim.License_Number,Party_Claim.Date_of_Birth,Party_Claim.Party_Status,
        Party_Claim.Phone_number,Party_Claim.Fax_Number,Party_Claim.Reg_Number
        from party_claim where Party_Claim.Party_Claim_id=@PartyID AND Party_Claim.Claim_Party_type_id=@PartyTypeID
    End
GO


