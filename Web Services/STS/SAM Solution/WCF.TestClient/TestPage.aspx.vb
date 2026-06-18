Imports System.ServiceModel
Imports System.ServiceModel.Channels


Public Class About
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load


    End Sub

    Protected Sub btnFindParty_Click(sender As Object, e As EventArgs)
        Dim objSAM As WCF.SAMForInsuranceV2.PurePartyServiceClient = New WCF.SAMForInsuranceV2.PurePartyServiceClient()

        Try
            Dim oRequest As New WCF.SAMForInsuranceV2.FindPartyRequestType
            Dim oResponse As New WCF.SAMForInsuranceV2.FindPartyResponseType

            oRequest.BranchCode = "HeadOff"
            oRequest.PartyTypeSpecified = True
            oRequest.PartyType = WCF.SAMForInsuranceV2.PartyTypeType.PC
            oRequest.MaxRowsToFetch = 10
            oRequest.MaxRowsToFetchSpecified = True
            oRequest.Status = "All"
            oResponse = objSAM.FindParty(oRequest)

            With oResponse
                Dim abc As String = oResponse.Parties(0).Name
            End With
        Catch timeProblem As TimeoutException
            objSAM.Abort()
        Catch greetingFault As FaultException(Of WCF.SAMForInsuranceV2.SAMMethodResponseData)
            objSAM.Abort()
        Catch unknownFault As FaultException
            objSAM.Abort()
        Catch commProblem As CommunicationException
            objSAM.Abort()
        End Try

    End Sub

    Protected Sub btnGetUserDetail_Click(sender As Object, e As EventArgs)

        Dim objSAM As WCF.SAMForInsuranceV2.PurePartyServiceClient = New WCF.SAMForInsuranceV2.PurePartyServiceClient()

        Try
            Dim oRequest As New WCF.SAMForInsuranceV2.GetUserDetailsRequestType
            Dim oResponse As New WCF.SAMForInsuranceV2.GetUserDetailsResponseType

            oResponse = objSAM.GetUserDetails(oRequest)

            With oResponse

            End With
        Catch timeProblem As TimeoutException
            objSAM.Abort()
        Catch greetingFault As FaultException(Of WCF.SAMForInsuranceV2.SAMMethodResponseData)
            objSAM.Abort()
        Catch unknownFault As FaultException
            objSAM.Abort()
        Catch commProblem As CommunicationException
            objSAM.Abort()
        End Try
    End Sub

    Protected Sub btnAddParty_Click(sender As Object, e As EventArgs)
        Dim objSAM As WCF.SAMForInsuranceV2.PurePartyServiceClient = New WCF.SAMForInsuranceV2.PurePartyServiceClient()

        Try
            Dim oAddPartyRequest As New WCF.SAMForInsuranceV2.AddPartyRequestType
            Dim oAddPartyResponse As New WCF.SAMForInsuranceV2.AddPartyResponseType
            Dim oParty As New WCF.SAMForInsuranceV2.BasePartyCCType
            With oAddPartyRequest

                .BranchCode = "HeadOff"
                .SubBranchCode = "HeadOff"
                oParty.BranchCode = "HeadOff"
                oParty.CompanyName = "WCFTEST"
                oParty.NumberOfOfficesSpecified = True
                oParty.NumberOfOffices = 10
                oParty.NumberOfEmployees = "BAND2"
                oParty.MainContact = "kkg@aa.com"

                'Dim oAddress As New WCF.SAMForInsuranceV2.BaseAddressWithContactsType()

                'oAddress.AddressLine1 = "Add Line 1"
                'oAddress.AddressLine2 = "Add Line 2"
                'oAddress.AddressLine3 = "Add Line 2"
                'oAddress.AddressLine4 = "Add Line 2"
                'oAddress.AddressTypeCode = WCF.SAMForInsuranceV2.AddressTypeType.Item3131XCO
                'oAddress.CountryCode = "GBR"
                'oAddress.PostCode = "244255"

                'Dim oAddresses As New List(Of WCF.SAMForInsuranceV2.BaseAddressWithContactsType)
                'oAddresses.Add(oAddress)

                'oParty.Addresses = oAddresses
                .Item = oParty
            End With


            oAddPartyResponse = objSAM.AddParty(oAddPartyRequest)


            With oAddPartyResponse

            End With
        Catch timeProblem As TimeoutException
            objSAM.Abort()
        Catch NexusFault As FaultException(Of WCF.SAMForInsuranceV2.SAMMethodResponseData)
            objSAM.Abort()
        Catch unknownFault As FaultException
            objSAM.Abort()
        Catch commProblem As CommunicationException
            objSAM.Abort()
        End Try

    End Sub

    Protected Sub btnUpdateParty_Click(sender As Object, e As EventArgs)
        Dim objSAM As WCF.SAMForInsuranceV2.PurePartyServiceClient = New WCF.SAMForInsuranceV2.PurePartyServiceClient()

        Try
            Dim oRequest As New WCF.SAMForInsuranceV2.GetPartyRequestType
            Dim oResponse As New WCF.SAMForInsuranceV2.GetPartyResponseType

            oRequest.BranchCode = "HeadOff"
            oRequest.PartyKey = 2363

            oResponse = objSAM.GetParty(oRequest)

            With oResponse
                UpdateParty(oResponse, oRequest.PartyKey)
            End With
        Catch timeProblem As TimeoutException
            objSAM.Abort()
        Catch greetingFault As FaultException(Of WCF.SAMForInsuranceV2.SAMMethodResponseData)
            objSAM.Abort()
        Catch unknownFault As FaultException
            objSAM.Abort()
        Catch commProblem As CommunicationException
            objSAM.Abort()
        End Try
    End Sub

    Private Sub UpdateParty(ByVal oParty As WCF.SAMForInsuranceV2.GetPartyResponseType, ByVal iPartyKey As Integer)
        Dim objSAM As WCF.SAMForInsuranceV2.PurePartyServiceClient = New WCF.SAMForInsuranceV2.PurePartyServiceClient()
        Dim oUpdatePartyRequest As New WCF.SAMForInsuranceV2.UpdatePartyRequestType
        Dim oUpdatePartyResponse As New WCF.SAMForInsuranceV2.UpdatePartyResponseType

        With oUpdatePartyRequest
            .PartyKey = iPartyKey
            .Item.XMLDataset = oParty.Item.XMLDataset
            .BranchCode = oParty.Item.BranchCode
            .SubBranchCode = oParty.Item.SubBranchCode
            .PartyTimestamp = oParty.PartyTimestamp
            .Item = oParty.Item
            '.itemField = ""
            '.Item.BranchCode = .BranchCode
            '.PartyKey = r_oParty.Key
            '.PartyTimestamp = r_oParty.TimeStamp
            '.SubBranchCode = v_sSubBranchCode
            'If String.IsNullOrEmpty(.Item.Currency) Then
            '    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            '    'Currency will be populated with BaseCurrency set in Bo
            '    Dim oCurrencyColl As NexusProvider.CurrencyCollection
            '    oCurrencyColl = oWebService.GetCurrenciesByBranch(.Item.BranchCode)
            '    .Item.Currency = oCurrencyColl(0).BaseCurrencyCode
            'End If
            '.LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
        End With

        Try
            oUpdatePartyResponse = objSAM.UpdateParty(oUpdatePartyRequest)

            With oUpdatePartyResponse

            End With
        Catch timeProblem As TimeoutException
            objSAM.Abort()
        Catch greetingFault As FaultException(Of WCF.SAMForInsuranceV2.SAMMethodResponseData)
            objSAM.Abort()
        Catch unknownFault As FaultException
            objSAM.Abort()
        Catch commProblem As CommunicationException
            objSAM.Abort()
        End Try
    End Sub
End Class