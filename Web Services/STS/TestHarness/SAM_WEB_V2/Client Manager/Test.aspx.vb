Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data.SqlTypes
Imports System.Data
Partial Class Client_Manager_Test
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim objBaseparty As Object
        Dim oAddPartyRequestType As New AddPartyRequestType
        Dim oAddPartyResponseType As New AddPartyResponseType
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")




        'set up request object with some values
        Dim BaseAddressWithContactsType() As BaseAddressWithContactsType = Nothing
        ReDim BaseAddressWithContactsType(0)

        BaseAddressWithContactsType(0) = New BaseAddressWithContactsType

        BaseAddressWithContactsType(0).AddressLine1 = "AddressLine1"
        BaseAddressWithContactsType(0).AddressLine2 = "AddressLine2"
        BaseAddressWithContactsType(0).AddressLine3 = "AddressLine3"
        BaseAddressWithContactsType(0).AddressLine4 = "AddressLine4"
        BaseAddressWithContactsType(0).AddressTypeCode = AddressTypeType.Item3131XCO
        BaseAddressWithContactsType(0).CountryCode = "India"
        BaseAddressWithContactsType(0).PostCode = "110021"

        Dim BasePartyPCType As New BasePartyPCType
        BasePartyPCType.BranchCode = "HeadOff"
        BasePartyPCType.Addresses = BaseAddressWithContactsType
        BasePartyPCType.Surname = "LNT"
        BasePartyPCType.Forename = "Test"
        BasePartyPCType.Initials = "J"
        BasePartyPCType.Title = "Test"

        Dim oBaseClientSharedDataType As New BaseClientSharedDataType
        BasePartyPCType.ClientDetail = oBaseClientSharedDataType

        oBaseClientSharedDataType.ServiceLevelCode = "St"

        With oAddPartyRequestType
            .BranchCode = "HeadOff"
            .Item = BasePartyPCType
        End With

        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'Dim oRequest As New FindPartyRequestType
        'Dim oResponse As New FindPartyResponseType
        'oRequest.BranchCode = "Headoff"
        'oRequest.Shortname = "%"
        'oRequest.PartyType = PartyTypeType.PC
        'oRequest.PartyTypeSpecified = True
        'oRequest.Status = "2"
        'oResponse = oSAM.FindParty(oRequest)



        oAddPartyResponseType = oSAM.AddParty(oAddPartyRequestType)

        Response.Write("ShortName" + oAddPartyResponseType.Shortname)
        Response.Write("Resolvedname" + oAddPartyResponseType.ResolvedName)

    End Sub
End Class
