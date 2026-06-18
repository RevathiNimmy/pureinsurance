Imports NexusProvider.SAMForInsurance.PureService
Imports System.Configuration.Provider
Imports System.Web
Imports System.Web.HttpContext
Imports System.Web.Configuration.WebConfigurationManager
Imports Microsoft.Web.Services3.Security.Tokens
Imports SiriusFS.SAM.Client.Security
Imports System.Xml.Serialization
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.Diagnostics
Imports System.Text
Imports System.Xml
Imports System.IO
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Resources
Imports System.ServiceModel
Partial Public Class ProviderSAMForInsuranceV2 : Inherits NexusProvider.ProviderBase

    ''' <summary>
    ''' This method changes the status of the Risk
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_iRiskKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateRiskStatus(ByVal v_iInsuranceFileKey As Integer, _
                                     ByVal v_iRiskKey As Integer, ByVal v_RiskStatusType As NexusProvider.RiskStatusType, _
                                      Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oUpdateRiskStatusRequest As UpdateRiskStatusRequestType
            Dim oUpdateRiskStatusResponse As UpdateRiskStatusResponseType
            Dim sbLogMessage As StringBuilder

            Try

                oSAM = InitializeServiceMethod()
                oUpdateRiskStatusRequest = New UpdateRiskStatusRequestType
                oUpdateRiskStatusResponse = New UpdateRiskStatusResponseType
                sbLogMessage = New StringBuilder

                With oUpdateRiskStatusRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    ' if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        ' if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            '  Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            ' Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        ' use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                        .InsuranceFileKeySpecified = True
                    Else
                        .InsuranceFileKeySpecified = False
                    End If

                    If v_iRiskKey > 0 Then
                        .RiskKey = v_iRiskKey
                        .RiskKeySpecified = True
                    Else
                        .RiskKeySpecified = False
                    End If

                    .RiskStatusCode = v_RiskStatusType

                End With


                ' Calling the SAM Method with Request Type
                ' add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oUpdateRiskStatusResponse = oSAM.UpdateRiskStatus(oUpdateRiskStatusRequest)
                End Using

                With oUpdateRiskStatusResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                End With

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdateRiskStatusResponse = Nothing
                oUpdateRiskStatusRequest = Nothing
            End Try


        End SyncLock
    End Sub

    ''' <summary>
    ''' this will the outer most layer values of the TREE
    ''' </summary>
    ''' <param name="v_oReinsurance2007"></param>
    ''' <param name="v_sRIModelCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub GetRIModelDetails(ByRef v_oReinsurance2007 As Reinsurances, _
                                 ByVal v_sRIModelCode As String, _
                                 Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetRIModelDetailsRequest As PureService.GetRIModelDetailsRequestType
            Dim oGetRIModelDetailsResponse As GetRIModelDetailsResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                sbLogMessage = New StringBuilder
                oGetRIModelDetailsRequest = New GetRIModelDetailsRequestType
                oGetRIModelDetailsResponse = New GetRIModelDetailsResponseType

                With oGetRIModelDetailsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_sRIModelCode.Length > 0 Then
                        .RIModelCode = v_sRIModelCode
                    Else
                        Throw New ArgumentNullException("v_sRIModelCode")
                    End If

                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetRIModelDetailsResponse = oSAM.GetRIModelDetails(oGetRIModelDetailsRequest)
                End Using

                With oGetRIModelDetailsResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                    v_oReinsurance2007.Code = .Code
                    v_oReinsurance2007.CurrencyCode = .CurrencyCode
                    v_oReinsurance2007.Description = .Description
                    v_oReinsurance2007.EffectiveDate = .EffectiveDate
                    v_oReinsurance2007.ExpiryDate = .ExpiryDate
                    v_oReinsurance2007.FACPremiums = .FACPremiums
                    v_oReinsurance2007.RIModelType = .RIModelType
                    v_oReinsurance2007.RIModelLineKey = .RIModelKey
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetRIModelDetails executed ok" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetRIModelDetailsRequest = Nothing
                oGetRIModelDetailsResponse = Nothing
            End Try

        End SyncLock
    End Sub
    ''' <summary>
    ''' this will get the middle layerof tree ( treaty line)
    ''' </summary>
    ''' <param name="v_oReinsurance2007"></param>
    ''' <param name="v_sRIModelCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub GetRIModelLineDetails(ByRef v_oReinsurance2007 As Reinsurances, _
                                          ByVal v_sRIModelCode As String, _
                                    Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetRIModelLineDetailsRequest As GetRIModelLineDetailsRequestType
            Dim oGetRIModelLineDetailsResponse As GetRIModelLineDetailsResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                sbLogMessage = New StringBuilder
                oGetRIModelLineDetailsRequest = New GetRIModelLineDetailsRequestType
                oGetRIModelLineDetailsResponse = New GetRIModelLineDetailsResponseType

                With oGetRIModelLineDetailsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_sRIModelCode.Length > 0 Then
                        .RIModelCode = v_sRIModelCode
                    Else
                        Throw New ArgumentNullException("v_sRIModelCode")
                    End If

                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetRIModelLineDetailsResponse = oSAM.GetRIModelLineDetails(oGetRIModelLineDetailsRequest)
                End Using

                Dim oRILineDetailsCollection As New RIModelLineDetailsCollection
                With oGetRIModelLineDetailsResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                    Dim oRALinesItemType As RIModelLineDetails
                    For Each oRILineDetails As BaseGetRIModelLineDetailsResponseTypeLinesRow In .Lines
                        oRALinesItemType = New RIModelLineDetails
                        With oRALinesItemType
                            .CedePremiumOnly = oRILineDetails.CedePremiumOnly
                            .CedingRate = oRILineDetails.CedingRate
                            .Description = oRILineDetails.Description
                            .LineLimit = oRILineDetails.LineLimit
                            .LowerLimit = oRILineDetails.LowerLimit
                            .NoOfLines = oRILineDetails.NoOfLines
                            .Priority = oRILineDetails.Priority
                            .ReinsuranceTypeCode = oRILineDetails.ReinsuranceTypeCode
                            .ReinsuranceTypeKey = oRILineDetails.ReinsuranceTypeKey
                            .RIModelKey = oRILineDetails.RIModelKey
                            .RIModelLineKey = oRILineDetails.RIModelLineKey
                            .SharePercentage = oRILineDetails.SharePercent
                            .TreatyCode = oRILineDetails.TreatyCode
                            .TreatyKey = oRILineDetails.TreatyKey
                            .TreatyTypeCode = oRILineDetails.TreatyTypeCode
                            .TreatyTypeKey = oRILineDetails.TreatyTypeKey
                            .EffectiveDate = oRILineDetails.EffectiveDate
                            .ManuallyAdded = oRILineDetails.ManuallyAddedTreaty
                            oRILineDetailsCollection.Add(oRALinesItemType)
                        End With
                    Next
                End With
                v_oReinsurance2007.RIModelLineDetails = oRILineDetailsCollection

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetRIModelLineDetails executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("oRILineDetailsCollection = " & oRILineDetailsCollection.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetRIModelLineDetailsRequest = Nothing
                oGetRIModelLineDetailsResponse = Nothing
            End Try

        End SyncLock

    End Sub
    ''' <summary>
    ''' this will return the ttreaty parties
    ''' </summary>
    ''' <param name="v_sTreatyCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetRITreatyPartyDetails(ByVal v_sTreatyCode As String, _
                                        Optional ByVal v_sBranchCode As String = Nothing) As NexusProvider.RITreatyParty

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetRITreatyPartyDetailsRequest As GetTreatyPartyDetailsRequestType
            Dim oGetRITreatyPartyDetailsResponse As GetTreatyPartyDetailsResponseType
            Dim sbLogMessage As StringBuilder
            Dim oRITreatyPartyCollection As New RITreatyPartyCollection
            Dim oRITreatyParty As RITreatyParty = Nothing
            Try
                oSAM = InitializeServiceMethod()
                sbLogMessage = New StringBuilder
                oGetRITreatyPartyDetailsRequest = New GetTreatyPartyDetailsRequestType
                oGetRITreatyPartyDetailsResponse = New GetTreatyPartyDetailsResponseType

                With oGetRITreatyPartyDetailsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .TreatyCode = v_sTreatyCode

                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetRITreatyPartyDetailsResponse = oSAM.GetTreatyPartyDetails(oGetRITreatyPartyDetailsRequest)
                End Using

                With oGetRITreatyPartyDetailsResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                    For Each oRITreatyPartyItems As BaseGetTreatyPartyDetailsResponseTypePartiesRow In .Parties
                        oRITreatyParty = New RITreatyParty
                        With oRITreatyPartyItems
                            oRITreatyParty.CommissionPercentage = .CommissionPercent
                            oRITreatyParty.Description = .Description
                            oRITreatyParty.IsDomiciledForTax = .IsDomiciledForTax
                            oRITreatyParty.IsReinsurerApproved = .IsReinsurerApproved
                            oRITreatyParty.PartyKey = .PartyKey
                            oRITreatyParty.ResolvedName = .ResolvedName
                            oRITreatyParty.SharePercentage = .SharePercent
                            oRITreatyParty.TaxGroupKey = .TaxGroupKey
                            oRITreatyParty.TreatyKey = .TreatyKey
                            oRITreatyParty.TreatyPartyKey = .TreatyPartyKey

                        End With

                    Next
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetTreatyPartyDetails executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("oRITreatyPartyCollection = " & oRITreatyPartyCollection.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetRITreatyPartyDetailsRequest = Nothing
                oGetRITreatyPartyDetailsResponse = Nothing
            End Try

            Return oRITreatyParty
        End SyncLock
    End Function
    ''' <summary>
    ''' this will summarize the whole tree values
    ''' </summary>
    ''' <param name="v_sRIModelCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function RIModelDetails(ByVal v_sRIModelCode As String, _
                                                      Optional ByVal v_sBranchCode As String = Nothing, _
                                                      Optional ByVal v_bIsXOL As Boolean = False, _
                                                      Optional ByVal v_iFilterType As Integer = 0, _
                                                      Optional ByVal v_lRIArrangementID As Long = 0) As Reinsurances

        Dim oReinsurance2007 As New Reinsurances
        If Not (String.IsNullOrEmpty(v_sRIModelCode)) Then
            GetRIModelDetails(oReinsurance2007, v_sRIModelCode, v_sBranchCode)
            GetRIModelLineDetails(oReinsurance2007, v_sRIModelCode, v_sBranchCode)
        End If

        ' Manually added treaties exist only on the arrangement, not on the model definition.
        ' Fetch arrangement lines and inject them as RIModelLineDetails entries so they appear in the tree.
        If v_lRIArrangementID > 0 Then
            Dim oArrangementLines As ArrangementLinesTypeCollection = GetRiskReinsuranceArrangementLinesRI2007(CInt(v_lRIArrangementID), v_sBranchCode)
            Dim iMaxPriority As Integer = 0
            For Each oModelLine As RIModelLineDetails In oReinsurance2007.RIModelLineDetails
                If oModelLine.Priority > iMaxPriority Then iMaxPriority = oModelLine.Priority
            Next
            Dim iManualPriority As Integer = iMaxPriority + 1
            For Each oArrLine As ArrangementLinesType In oArrangementLines
                If oArrLine.ManuallyAdded Then
                    Dim oNewLine As New RIModelLineDetails
                    oNewLine.TreatyCode = oArrLine.TreatyCode
                    oNewLine.TreatyKey = oArrLine.TreatyKey
                    oNewLine.Description = oArrLine.RIName
                    oNewLine.Priority = iManualPriority
                    oNewLine.LineLimit = CDec(oArrLine.SumInsured)
                    oNewLine.LowerLimit = CDec(oArrLine.LowerLimit)
                    oNewLine.SharePercentage = CDec(oArrLine.ThisPerc)
                    oNewLine.ReinsuranceTypeCode = oArrLine.ReinsuranceTypeCode
                    oNewLine.TreatyTypeCode = If(v_bIsXOL, "XOL", "PROP")
                    oNewLine.ManuallyAdded = True
                    oReinsurance2007.RIModelLineDetails.Add(oNewLine)
                End If
            Next
        End If

        Dim oRITreatyPartyCollection As New NexusProvider.RITreatyPartyCollection
        Dim oRITreatyParty As NexusProvider.RITreatyParty
        For i As Integer = 0 To oReinsurance2007.RIModelLineDetails.Count - 1
            oRITreatyParty = New RITreatyParty
            oRITreatyParty = GetRITreatyPartyDetails(oReinsurance2007.RIModelLineDetails.Item(i).TreatyCode)
            oRITreatyPartyCollection.Add(oRITreatyParty)
        Next
        oReinsurance2007.RITreatyParty = oRITreatyPartyCollection
        Return oReinsurance2007

    End Function
    ''' <summary>
    ''' this will retrun the Risk RI Line from SAM
    ''' </summary>
    ''' <param name="v_iArrangementKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetRiskReinsuranceArrangementLinesRI2007(ByVal v_iArrangementKey As Integer, _
                                                                        Optional ByVal v_sBranchCode As String = Nothing) As ArrangementLinesTypeCollection
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetRiskReinsuranceArrangementLinesRI2007Request As GetRiskReinsuranceArrangementLinesRI2007RequestType
            Dim oGetRiskReinsuranceArrangementLinesRI2007Response As GetRiskReinsuranceArrangementLinesRI2007ResponseType
            Dim sbLogMessage As StringBuilder
            Dim oArrangementLinesRI2007 As ArrangementLinesType
            Dim oArrangementLinesRI2007Collection As New ArrangementLinesTypeCollection
            Try
                oSAM = InitializeServiceMethod()
                sbLogMessage = New StringBuilder
                oGetRiskReinsuranceArrangementLinesRI2007Request = New GetRiskReinsuranceArrangementLinesRI2007RequestType
                oGetRiskReinsuranceArrangementLinesRI2007Response = New GetRiskReinsuranceArrangementLinesRI2007ResponseType

                With oGetRiskReinsuranceArrangementLinesRI2007Request
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_iArrangementKey > 0 Then
                        .ArrangementKey = v_iArrangementKey
                    Else
                        Throw New ArgumentNullException("GetRiskReinsuranceArrangementLinesRI2007.ArrangementId")
                    End If
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetRiskReinsuranceArrangementLinesRI2007Response = oSAM.GetRiskReinsuranceArrangementLinesRI2007(oGetRiskReinsuranceArrangementLinesRI2007Request)
                End Using

                With oGetRiskReinsuranceArrangementLinesRI2007Response

                    If .ArrangementLines IsNot Nothing AndAlso .ArrangementLines.Count > 0 Then

                        For Each oArrangementLinesRI2007TypeRow As BaseRiskRIArrangementLineType In .ArrangementLines

                            oArrangementLinesRI2007 = New ArrangementLinesType

                            With oArrangementLinesRI2007TypeRow
                                oArrangementLinesRI2007.ActionType = .ActionType
                                oArrangementLinesRI2007.AgreementCode = .AgreementCode
                                oArrangementLinesRI2007.CedePremiumOnly = .CedePremiumOnly
                                oArrangementLinesRI2007.CommissionPerc = .CommissionPercent
                                oArrangementLinesRI2007.CommissionTax = .CommissionTax
                                oArrangementLinesRI2007.CommissionValue = .CommissionValue
                                oArrangementLinesRI2007.DefaultPerc = .DefaultSharePercent
                                oArrangementLinesRI2007.Grouping = .Grouping
                                oArrangementLinesRI2007.IsCommissionModified = .IsCommissionModified
                                oArrangementLinesRI2007.IsDomiciledForTax = .IsDomiciledForTax
                                oArrangementLinesRI2007.IsRIBroker = .IsRIBroker
                                oArrangementLinesRI2007.LineLimit = .LineLimit
                                oArrangementLinesRI2007.LowerLimit = .LowerLimit
                                oArrangementLinesRI2007.NumberOfLines = .NumberOfLines
                                oArrangementLinesRI2007.ParticipationPercent = .ParticipationPercent
                                oArrangementLinesRI2007.PartyKey = .PartyKey
                                oArrangementLinesRI2007.PremiumPercent = .PremiumPercent
                                oArrangementLinesRI2007.PremiumTax = .PremiumTax
                                oArrangementLinesRI2007.PremiumValue = .PremiumValue
                                oArrangementLinesRI2007.Priority = .Priority
                                oArrangementLinesRI2007.ReinsuranceTypeCode = .ReinsuranceTypeCode
                                oArrangementLinesRI2007.Retained = .Retained
                                oArrangementLinesRI2007.RIarrangementKey = .RIArrangementKey
                                oArrangementLinesRI2007.RIArrangementLineKey = .RIArrangementLineKey
                                oArrangementLinesRI2007.RIName = .RIName
                                oArrangementLinesRI2007.RIPlacement = .RIPlacement
                                oArrangementLinesRI2007.SumInsured = .SumInsured
                                oArrangementLinesRI2007.ThisPerc = .ThisSharePercent
                                oArrangementLinesRI2007.TreatyCode = .TreatyCode
                                oArrangementLinesRI2007.Type = .Type
                                ' PBI 35359: Map SAM IsEditedDB (DB value) to IsEditedDB - IsEdited stays False on page load
                                oArrangementLinesRI2007.IsEditedDB = .IsEditedDB
                                oArrangementLinesRI2007.IsPremiumEdited = .IsPremiumEdited
                                If .Type.ToUpper.Trim = "R" Then
                                    oArrangementLinesRI2007.Retained = 100.0
                                End If
                                If .BrokerParticipants IsNot Nothing AndAlso .BrokerParticipants.Count > 0 AndAlso .Type = "F" Then

                                    oArrangementLinesRI2007.BrokerParticipants = New NexusProvider.BrokerParticipantsCollection

                                    For Each oBrokerParticipant As BaseBrokerParticipants In .BrokerParticipants
                                        Dim oBrokerPart As New NexusProvider.BrokerParticipants
                                        With oBrokerParticipant
                                            oBrokerPart.ParticipationPercentage = .ParticpationPercentage
                                            oBrokerPart.PartyKey = .PartyKey
                                            oBrokerPart.PartyCode = .PartyCode
                                            oBrokerPart.PartyName = .PartyName
                                        End With

                                        oArrangementLinesRI2007.BrokerParticipants.Add(oBrokerPart)
                                    Next
                                End If

                                If .FAXParticipants IsNot Nothing AndAlso .FAXParticipants.Count > 0 AndAlso .Type = "FX" Then
                                    For Each oFAXPart As BaseFAXParticipants In .FAXParticipants
                                        Dim oFAXParticipent As New FAXParticipants
                                        With oFAXPart
                                            oFAXParticipent.AccountType = .AccountType
                                            oFAXParticipent.AgreementCode = .AgreementCode
                                            oFAXParticipent.CommissionPercent = .CommissionPercent
                                            oFAXParticipent.CommissionTax = .CommissionTax
                                            oFAXParticipent.CommissionValue = .CommissionValue
                                            oFAXParticipent.ParticipationPercentage = .ParticpationPercentage
                                            oFAXParticipent.PartyCode = .PartyCode
                                            oFAXParticipent.PartyKey = .PartyKey
                                            oFAXParticipent.PartyName = .PartyName
                                            oFAXParticipent.PremiumTax = .PremiumTax
                                            oFAXParticipent.PremiumValue = .PremiumValue
                                            oFAXParticipent.RIArrangementLineKey = .RIArrangementLineKey
                                            oFAXParticipent.SumInsured = .SumInsured

                                            If .BrokerParticipants IsNot Nothing AndAlso .BrokerParticipants.Count > 0 Then

                                                For Each oBrokerParticipant As BaseBrokerParticipants In .BrokerParticipants
                                                    Dim oBrokerPart As New NexusProvider.BrokerParticipants
                                                    With oBrokerParticipant
                                                        oBrokerPart.ParticipationPercentage = .ParticpationPercentage
                                                        oBrokerPart.PartyKey = .PartyKey
                                                        oBrokerPart.PartyCode = .PartyCode
                                                        oBrokerPart.PartyName = .PartyName
                                                    End With
                                                    oFAXParticipent.BrokerParticipants.Add(oBrokerPart)
                                                Next
                                            End If
                                        End With
                                        oArrangementLinesRI2007.FAXParticipants.Add(oFAXParticipent)
                                    Next
                                End If
                            End With

                            oArrangementLinesRI2007Collection.Add(oArrangementLinesRI2007)
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetRiskReinsuranceArrangementLinesRI2007" & vbCrLf)

                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("Branch Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Branch Code : " & v_sBranchCode & vbCrLf)
                    End If

                    If IsNothing(v_iArrangementKey) Then
                        sbLogMessage.AppendLine("ArrangementId : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("ArrangementId : " & v_iArrangementKey & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oArrangementLinesRI2007Collection.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetRiskReinsuranceArrangementLinesRI2007Request = Nothing
                oGetRiskReinsuranceArrangementLinesRI2007Response = Nothing
            End Try

            Return oArrangementLinesRI2007Collection
        End SyncLock
    End Function
    ''' <summary>
    ''' GetClaimRIArrangementLinesRI2007
    ''' </summary>
    ''' <param name="v_iClaimKey"></param>
    ''' <param name="v_iArrangementKey"></param>
    ''' <param name="v_iMode"></param>
    ''' <param name="v_bIsRecovery"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetClaimRIArrangementLinesRI2007(ByVal v_iClaimKey As Integer, _
                                                        ByVal v_iArrangementKey As Integer, _
                                                        ByVal v_iMode As Integer, _
                                                        ByVal v_bIsRecovery As Boolean, _
                                                        Optional ByVal v_sBranchCode As String = Nothing) As ArrangementLinesTypeCollection
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetReinsuranceArrangementLinesRI2007Request As GetClaimRIArrangementLinesRI2007RequestType
            Dim oGetReinsuranceArrangementLinesRI2007Response As GetClaimRIArrangementLinesRI2007ResponseType
            Dim sbLogMessage As StringBuilder
            Dim oArrangementLinesRI2007 As ArrangementLinesType
            Dim oArrangementLinesRI2007Collection As New ArrangementLinesTypeCollection
            Try
                oSAM = InitializeServiceMethod()
                sbLogMessage = New StringBuilder
                oGetReinsuranceArrangementLinesRI2007Request = New GetClaimRIArrangementLinesRI2007RequestType
                oGetReinsuranceArrangementLinesRI2007Response = New GetClaimRIArrangementLinesRI2007ResponseType

                With oGetReinsuranceArrangementLinesRI2007Request
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .ArrangementKey = v_iArrangementKey
                    .ClaimKey = v_iClaimKey

                    If v_iMode < 0 Then
                        .ModeSpecified = False
                    Else
                        .Mode = v_iMode
                        .ModeSpecified = True
                    End If

                    .IsRecovery = v_bIsRecovery
                    .IsRecoverySpecified = v_bIsRecovery
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetReinsuranceArrangementLinesRI2007Response = oSAM.GetClaimRIArrangementLinesRI2007(oGetReinsuranceArrangementLinesRI2007Request)
                End Using

                With oGetReinsuranceArrangementLinesRI2007Response
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        If .RIArrangementLines IsNot Nothing Then

                            For Each oArrangementLinesRI2007TypeRow As BaseClaimRiskRIArrangementLineType In .RIArrangementLines

                                oArrangementLinesRI2007 = New ArrangementLinesType

                                With oArrangementLinesRI2007TypeRow
                                    oArrangementLinesRI2007.ActionType = .ActionType
                                    oArrangementLinesRI2007.AgreementCode = .AgreementCode
                                    oArrangementLinesRI2007.CedePremiumOnly = .CedePremiumOnly
                                    oArrangementLinesRI2007.DefaultPerc = .DefaultSharePercent
                                    oArrangementLinesRI2007.Grouping = .Grouping
                                    oArrangementLinesRI2007.IsDomiciledForTax = .IsDomiciledForTax
                                    oArrangementLinesRI2007.IsRIBroker = .IsRIBroker
                                    oArrangementLinesRI2007.LineLimit = .LineLimit
                                    oArrangementLinesRI2007.LowerLimit = .LowerLimit
                                    oArrangementLinesRI2007.NumberOfLines = .NumberOfLines
                                    oArrangementLinesRI2007.PartyKey = .PartyKey
                                    oArrangementLinesRI2007.Priority = .Priority
                                    oArrangementLinesRI2007.ReinsuranceTypeCode = .ReinsuranceTypeCode
                                    oArrangementLinesRI2007.Retained = .Retained
                                    oArrangementLinesRI2007.RIarrangementKey = .RIArrangementKey
                                    oArrangementLinesRI2007.RIArrangementLineKey = .RIArrangementLineKey
                                    oArrangementLinesRI2007.RIName = .RIName
                                    oArrangementLinesRI2007.RIPlacement = .RIPlacement
                                    oArrangementLinesRI2007.SumInsured = .SumInsured
                                    oArrangementLinesRI2007.ThisPerc = .ThisSharePercent
                                    oArrangementLinesRI2007.TreatyCode = .TreatyCode
                                    oArrangementLinesRI2007.Type = .Type
                                    oArrangementLinesRI2007.Balance = .Balance
                                    oArrangementLinesRI2007.PaymentToDate = .PaymentToDate
                                    oArrangementLinesRI2007.RecoverToDate = .RecoverToDate
                                    oArrangementLinesRI2007.ReserveToDate = .ReserveToDate
                                    oArrangementLinesRI2007.ThisPayment = .ThisPayment
                                    oArrangementLinesRI2007.ThisReserve = .ThisReserve
                                    oArrangementLinesRI2007.Incurred = .Incurred

                                    If .BrokerParticipants IsNot Nothing AndAlso .BrokerParticipants.Count > 0 Then
                                        oArrangementLinesRI2007.BrokerParticipants = New NexusProvider.BrokerParticipantsCollection
                                        For Each oBrokerParticipant As BaseBrokerParticipants In .BrokerParticipants
                                            Dim oBrokerPart As New NexusProvider.BrokerParticipants
                                            With oBrokerParticipant
                                                oBrokerPart.ParticipationPercentage = .ParticpationPercentage
                                                oBrokerPart.PartyKey = .PartyKey
                                                oBrokerPart.PartyCode = .PartyCode
                                                oBrokerPart.PartyName = .PartyName
                                            End With

                                            oArrangementLinesRI2007.BrokerParticipants.Add(oBrokerPart)
                                        Next
                                    End If

                                    If .FAXParticipants IsNot Nothing AndAlso .FAXParticipants.Count > 0 Then
                                        For Each oFAXPart As BaseClaimFAXParticipants In .FAXParticipants
                                            Dim oFAXParticipent As New NexusProvider.FAXParticipants
                                            With oFAXPart
                                                oFAXParticipent.AccountType = .AccountType
                                                oFAXParticipent.AgreementCode = .AgreementCode
                                                oFAXParticipent.ParticipationPercentage = .ParticpationPercentage
                                                oFAXParticipent.PartyCode = .PartyCode
                                                oFAXParticipent.PartyKey = .PartyKey
                                                oFAXParticipent.PartyName = .PartyName
                                                oFAXParticipent.RIArrangementLineKey = .RIArrangementLineKey
                                                oFAXParticipent.SumInsured = .SumInsured
                                                oFAXParticipent.ActionType = .ActionType
                                                oFAXParticipent.PaymentToDate = .PaymentToDate
                                                oFAXParticipent.RecoverToDate = .RecoverToDate
                                                oFAXParticipent.ReserveToDate = .ReserveToDate
                                                oFAXParticipent.ThisPayment = .ThisPayment
                                                oFAXParticipent.ThisReserve = .ThisReserve

                                                If .BrokerParticipants IsNot Nothing AndAlso .BrokerParticipants.Count > 0 Then
                                                    For Each oBrokerParticipant As BaseBrokerParticipants In .BrokerParticipants
                                                        Dim oBrokerPart As New NexusProvider.BrokerParticipants
                                                        With oBrokerParticipant
                                                            oBrokerPart.ParticipationPercentage = .ParticpationPercentage
                                                            oBrokerPart.PartyKey = .PartyKey
                                                            oBrokerPart.PartyCode = .PartyCode
                                                            oBrokerPart.PartyName = .PartyName
                                                        End With
                                                        oFAXParticipent.BrokerParticipants.Add(oBrokerPart)
                                                    Next
                                                End If
                                            End With
                                            oArrangementLinesRI2007.FAXParticipants.Add(oFAXParticipent)
                                        Next
                                    End If
                                End With
                                oArrangementLinesRI2007Collection.Add(oArrangementLinesRI2007)
                            Next
                        End If
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetClaimReinsuranceArrangementLinesRI2007 executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iClaimID = " & v_iClaimKey.ToString() & vbCrLf)
                    sbLogMessage.AppendLine("v_iArrangementID = " & v_iArrangementKey.ToString() & vbCrLf)
                    sbLogMessage.AppendLine("v_iMode = " & v_iMode.ToString() & vbCrLf)

                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    sbLogMessage.AppendLine("oRALinesRI2007Collection" & oArrangementLinesRI2007Collection.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetReinsuranceArrangementLinesRI2007Request = Nothing
                oGetReinsuranceArrangementLinesRI2007Response = Nothing
            End Try

            Return oArrangementLinesRI2007Collection
        End SyncLock
    End Function
    ''' <summary>
    ''' FindReinsurer
    ''' </summary>
    ''' <param name="v_oReinsurerSearchCriteria"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function FindReinsurer(ByVal v_oReinsurerSearchCriteria As ReinsurerSearchCriteria, _
                                    Optional ByVal v_sBranchCode As String = Nothing) As ReinsurerCollection


        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oFindReinsurerRequest As FindReinsurerRequestType
            Dim oFindReinsurerResponse As FindReinsurerResponseType
            Dim sbLogMessage As StringBuilder
            Dim oReinsurerCollection As New ReinsurerCollection
            Try
                oSAM = InitializeServiceMethod()
                sbLogMessage = New StringBuilder
                oFindReinsurerRequest = New FindReinsurerRequestType
                oFindReinsurerResponse = New FindReinsurerResponseType

                With oFindReinsurerRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    .RICode = v_oReinsurerSearchCriteria.RICode
                    .RIName = v_oReinsurerSearchCriteria.RIName
                    .IsRetained = v_oReinsurerSearchCriteria.IsRetained
                    .IsRetainedSpecified = v_oReinsurerSearchCriteria.IsRetained
                    .FileCode = v_oReinsurerSearchCriteria.FileCode
                    .IsBroker = v_oReinsurerSearchCriteria.IsBroker
                    .IsBrokerSpecified = v_oReinsurerSearchCriteria.IsBrokerSpecified
                    If v_oReinsurerSearchCriteria.IsFAXSpecified Then
                        .IsFAXSpecified = True
                        .IsFAX = v_oReinsurerSearchCriteria.IsFAX
                    End If
                End With

                Using trace As New Tracer(Category.Trace)
                    oFindReinsurerResponse = oSAM.FindReinsurer(oFindReinsurerRequest)
                End Using

                With oFindReinsurerResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        Dim oRIInsurer As Reinsurer
                        Dim oAddress As Address

                        If .Reinsurers IsNot Nothing Then

                            For Each oReinsureItem As BaseFindReinsurerResponseTypeReinsurersRow In .Reinsurers
                                oRIInsurer = New Reinsurer
                                oAddress = New Address
                                With oReinsureItem
                                    oRIInsurer.Address1 = .Address1
                                    oRIInsurer.Address2 = .Address2
                                    oRIInsurer.BranchCode = .BranchCode
                                    oRIInsurer.IsBroker = .IsBroker
                                    oRIInsurer.IsDomiciledForTax = .IsDomiciledForTax
                                    oRIInsurer.IsRetained = .IsRetained
                                    oRIInsurer.IsTaxExempt = .IsTaxExempt
                                    oRIInsurer.PostCode = .PostalCode
                                    oRIInsurer.ReinsuranceTypeCode = .ReinsuranceTypeCode
                                    oRIInsurer.ReinsurerCode = .ReinsurerCode
                                    oRIInsurer.ReinsurerKey = .ReinsurerKey
                                    oRIInsurer.RIName = .RIName
                                    oRIInsurer.CommissionPercentage = (.DefaultCommissionPercentage * 100)
                                    oRIInsurer.TaxGroupCode = .TaxGroupCode
                                    oRIInsurer.TaxNumber = .TaxNumber
                                    oRIInsurer.TaxPercentage = .TaxPercentage
                                    oRIInsurer.AccountType = .AccountType
                                End With
                                oReinsurerCollection.Add(oRIInsurer)
                            Next
                        End If
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("FindReinsurer executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oFindReinsurerSearchCriteria = " & oReinsurerCollection.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(oReinsurerCollection) Then
                        sbLogMessage.AppendLine("Returned " & oReinsurerCollection.Count.ToString & " results" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Returned 0 results" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oFindReinsurerRequest = Nothing
                oFindReinsurerResponse = Nothing
            End Try

            Return oReinsurerCollection

        End SyncLock

    End Function
    ''' <summary>
    ''' This Method Update the Added/Changed RI Line into Database for NB/MTA/MTR/MTC/REN
    ''' </summary>
    ''' <param name="v_oRIArrangementLines"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateArrangementLinesRI2007(ByVal v_oRIArrangementLines As ArrangementLinesTypeCollection, _
                                  Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal v_dTotalSI As Double = 0)


        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oUpdateRIArrangementLinesRI2007Request As UpdateArrangementLinesRI2007RequestType
            Dim oUpdateRIArrangementLinesRI2007Response As UpdateArrangementLinesRI2007ResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                sbLogMessage = New StringBuilder
                oUpdateRIArrangementLinesRI2007Request = New UpdateArrangementLinesRI2007RequestType
                oUpdateRIArrangementLinesRI2007Response = New UpdateArrangementLinesRI2007ResponseType

                With oUpdateRIArrangementLinesRI2007Request
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    Dim oRIArrangementLine(v_oRIArrangementLines.Count - 1) As BaseRiskRIArrangementLineType
                    Dim i As Integer
                    For i = 0 To v_oRIArrangementLines.Count - 1
                        oRIArrangementLine(i) = New BaseRiskRIArrangementLineType
                        oRIArrangementLine(i).ActionType = v_oRIArrangementLines.Item(i).ActionType
                        oRIArrangementLine(i).AgreementCode = v_oRIArrangementLines.Item(i).AgreementCode
                        oRIArrangementLine(i).CedePremiumOnly = v_oRIArrangementLines.Item(i).CedePremiumOnly
                        oRIArrangementLine(i).CommissionPercent = v_oRIArrangementLines.Item(i).CommissionPerc
                        oRIArrangementLine(i).CommissionTax = v_oRIArrangementLines.Item(i).CommissionTax
                        If v_oRIArrangementLines.Item(i).CommissionTax > 0 Then
                            oRIArrangementLine(i).CommissionTaxSpecified = True
                        End If

                        oRIArrangementLine(i).CommissionValue = v_oRIArrangementLines.Item(i).CommissionValue
                        oRIArrangementLine(i).IsCommissionModified = v_oRIArrangementLines.Item(i).IsCommissionModified
                        oRIArrangementLine(i).DefaultSharePercent = v_oRIArrangementLines.Item(i).DefaultPerc
                        oRIArrangementLine(i).Grouping = v_oRIArrangementLines.Item(i).Grouping
                        oRIArrangementLine(i).IsDomiciledForTax = v_oRIArrangementLines.Item(i).IsDomiciledForTax

                        If v_oRIArrangementLines.Item(i).Grouping > 0 Then
                            oRIArrangementLine(i).GroupingSpecified = True
                        End If

                        oRIArrangementLine(i).LineLimit = v_oRIArrangementLines.Item(i).LineLimit
                        oRIArrangementLine(i).LowerLimit = v_oRIArrangementLines.Item(i).LowerLimit

                        If v_oRIArrangementLines.Item(i).LowerLimit > 0 Then
                            oRIArrangementLine(i).LowerLimitSpecified = True

                        End If

                        oRIArrangementLine(i).NumberOfLines = v_oRIArrangementLines.Item(i).NumberOfLines
                        oRIArrangementLine(i).ParticipationPercent = v_oRIArrangementLines.Item(i).ParticipationPercent

                        If v_oRIArrangementLines.Item(i).ParticipationPercent > 0 Then
                            oRIArrangementLine(i).ParticipationPercentSpecified = True
                        End If

                        oRIArrangementLine(i).PartyKey = v_oRIArrangementLines.Item(i).PartyKey

                        If v_oRIArrangementLines.Item(i).PartyKey > 0 Then
                            oRIArrangementLine(i).PartyKeySpecified = True
                        End If

                        oRIArrangementLine(i).PremiumPercent = v_oRIArrangementLines.Item(i).PremiumPercent
                        oRIArrangementLine(i).FACPropPremiumPerc = v_oRIArrangementLines.Item(i).FACPropPremiumPerc
                        oRIArrangementLine(i).PremiumTax = v_oRIArrangementLines.Item(i).PremiumTax

                        If v_oRIArrangementLines.Item(i).PremiumTax > 0 Then
                            oRIArrangementLine(i).PremiumTaxSpecified = True
                        End If

                        oRIArrangementLine(i).PremiumValue = v_oRIArrangementLines.Item(i).PremiumValue
                        oRIArrangementLine(i).Priority = v_oRIArrangementLines.Item(i).Priority
                        oRIArrangementLine(i).Retained = v_oRIArrangementLines.Item(i).Retained

                        If v_oRIArrangementLines.Item(i).Retained > 0 Then
                            oRIArrangementLine(i).RetainedSpecified = True
                        End If

                        oRIArrangementLine(i).SumInsured = v_oRIArrangementLines.Item(i).SumInsured
                        If v_oRIArrangementLines.Item(i).Type.Trim.ToUpper = "F" And v_dTotalSI <> 0 Then
                            oRIArrangementLine(i).ThisSharePercent = (oRIArrangementLine(i).SumInsured / v_dTotalSI) * 100
                        Else
                            oRIArrangementLine(i).ThisSharePercent = v_oRIArrangementLines.Item(i).ThisPerc
                        End If
                        oRIArrangementLine(i).TreatyCode = v_oRIArrangementLines.Item(i).TreatyCode
                        oRIArrangementLine(i).Type = v_oRIArrangementLines.Item(i).Type
                        oRIArrangementLine(i).IsRIBroker = v_oRIArrangementLines.Item(i).IsRIBroker
                        oRIArrangementLine(i).ReinsuranceTypeCode = v_oRIArrangementLines.Item(i).ReinsuranceTypeCode
                        oRIArrangementLine(i).RIArrangementKey = v_oRIArrangementLines.Item(i).RIarrangementKey
                        oRIArrangementLine(i).RIArrangementLineKey = v_oRIArrangementLines.Item(i).RIArrangementLineKey
                        oRIArrangementLine(i).RIName = v_oRIArrangementLines.Item(i).RIName
                        oRIArrangementLine(i).RIPlacement = v_oRIArrangementLines.Item(i).RIPlacement
                        ' PBI 35359: Send IsEditedDB (DB-persisted flag) to SAM — not the session UI flag IsEdited
                        oRIArrangementLine(i).IsEditedDB = v_oRIArrangementLines.Item(i).IsEditedDB
                        oRIArrangementLine(i).IsPremiumEdited = v_oRIArrangementLines.Item(i).IsPremiumEdited
                      
                        'Broker Participents
                        If v_oRIArrangementLines.Item(i).Type.Trim.ToUpper = "F" Then
                            If v_oRIArrangementLines.Item(i).BrokerParticipants IsNot Nothing AndAlso v_oRIArrangementLines.Item(i).BrokerParticipants.Count > 0 Then
                                Dim oBaseBrokerPart(v_oRIArrangementLines.Item(i).BrokerParticipants.Count - 1) As BaseBrokerParticipants

                                For iCount As Integer = 0 To v_oRIArrangementLines.Item(i).BrokerParticipants.Count - 1
                                    oBaseBrokerPart(iCount) = New BaseBrokerParticipants

                                    oBaseBrokerPart(iCount).ParticpationPercentage = v_oRIArrangementLines.Item(i).BrokerParticipants(iCount).ParticipationPercentage
                                    oBaseBrokerPart(iCount).PartyCode = v_oRIArrangementLines.Item(i).BrokerParticipants(iCount).PartyCode
                                    oBaseBrokerPart(iCount).PartyKey = v_oRIArrangementLines.Item(i).BrokerParticipants(iCount).PartyKey
                                    oBaseBrokerPart(iCount).PartyName = v_oRIArrangementLines.Item(i).BrokerParticipants(iCount).PartyName
                                Next
                                oRIArrangementLine(i).BrokerParticipants = oBaseBrokerPart.ToList().ConvertAll(New Converter(Of BaseBrokerParticipants, BaseBrokerParticipants)(AddressOf ToBrokerParticipantList))
                            End If
                        End If
                        'FAX Praticipents
                        If v_oRIArrangementLines.Item(i).Type.Trim.ToUpper = "FX" Then
                            If v_oRIArrangementLines.Item(i).FAXParticipants IsNot Nothing AndAlso v_oRIArrangementLines.Item(i).FAXParticipants.Count > 0 Then
                                Dim oBaseFAXParticipants(v_oRIArrangementLines.Item(i).FAXParticipants.Count - 1) As BaseFAXParticipants
                                For iCount As Integer = 0 To v_oRIArrangementLines.Item(i).FAXParticipants.Count - 1
                                    oBaseFAXParticipants(iCount) = New BaseFAXParticipants

                                    oBaseFAXParticipants(iCount).ParticpationPercentage = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).ParticipationPercentage
                                    oBaseFAXParticipants(iCount).PartyCode = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).PartyCode
                                    oBaseFAXParticipants(iCount).PartyKey = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).PartyKey
                                    oBaseFAXParticipants(iCount).PartyName = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).PartyName
                                    oBaseFAXParticipants(iCount).AccountType = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).AccountType
                                    oBaseFAXParticipants(iCount).ActionType = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).ActionType
                                    oBaseFAXParticipants(iCount).AgreementCode = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).AgreementCode
                                    oBaseFAXParticipants(iCount).RIArrangementLineKey = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).RIArrangementLineKey
                                    oBaseFAXParticipants(iCount).SumInsured = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).SumInsured
                                    oBaseFAXParticipants(iCount).CommissionPercent = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).CommissionPercent

                                    If v_oRIArrangementLines.Item(i).FAXParticipants(iCount).CommissionTax > 0 Then
                                        oBaseFAXParticipants(iCount).CommissionTax = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).CommissionTax
                                        oBaseFAXParticipants(iCount).CommissionTaxSpecified = True
                                    Else
                                        oBaseFAXParticipants(iCount).CommissionTaxSpecified = False
                                    End If

                                    oBaseFAXParticipants(iCount).CommissionValue = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).CommissionValue

                                    If v_oRIArrangementLines.Item(i).FAXParticipants(iCount).PremiumTax > 0 Then
                                        oBaseFAXParticipants(iCount).PremiumTax = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).PremiumTax
                                        oBaseFAXParticipants(iCount).PremiumTaxSpecified = True
                                    Else
                                        oBaseFAXParticipants(iCount).PremiumTaxSpecified = False
                                    End If

                                    oBaseFAXParticipants(iCount).PremiumValue = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).PremiumValue

                                    'FAX Broker Participent
                                    If v_oRIArrangementLines.Item(i).FAXParticipants(iCount).BrokerParticipants IsNot Nothing AndAlso v_oRIArrangementLines.Item(i).FAXParticipants(iCount).BrokerParticipants.Count > 0 Then
                                        Dim oBaseFAXBrokerPart(v_oRIArrangementLines.Item(i).FAXParticipants(iCount).BrokerParticipants.Count - 1) As BaseBrokerParticipants

                                        For jCount As Integer = 0 To v_oRIArrangementLines.Item(i).FAXParticipants(iCount).BrokerParticipants.Count - 1
                                            oBaseFAXBrokerPart(jCount) = New BaseBrokerParticipants

                                            oBaseFAXBrokerPart(jCount).ParticpationPercentage = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).BrokerParticipants(jCount).ParticipationPercentage
                                            oBaseFAXBrokerPart(jCount).PartyCode = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).BrokerParticipants(jCount).PartyCode
                                            oBaseFAXBrokerPart(jCount).PartyKey = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).BrokerParticipants(jCount).PartyKey
                                            oBaseFAXBrokerPart(jCount).PartyName = v_oRIArrangementLines.Item(i).FAXParticipants(iCount).BrokerParticipants(jCount).PartyName
                                        Next
                                        oBaseFAXParticipants(iCount).BrokerParticipants = oBaseFAXBrokerPart.ToList().ConvertAll(New Converter(Of BaseBrokerParticipants, BaseBrokerParticipants)(AddressOf ToBrokerParticipantList))
                                    End If
                                Next
                                oRIArrangementLine(i).FAXParticipants = oBaseFAXParticipants.ToList().ConvertAll(New Converter(Of BaseFAXParticipants, BaseFAXParticipants)(AddressOf ToBaseFaxParticipants))
                            End If
                        End If
                    Next

                    .RIArrangementLines = oRIArrangementLine.ToList().ConvertAll(New Converter(Of BaseRiskRIArrangementLineType, BaseRiskRIArrangementLineType)(AddressOf ToBaseRiskRIArrangementLineTypeList))
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                End With

                Using trace As New Tracer(Category.Trace)
                    oUpdateRIArrangementLinesRI2007Response = oSAM.UpdateArrangementLinesRI2007(oUpdateRIArrangementLinesRI2007Request)
                End Using

                With oUpdateRIArrangementLinesRI2007Response

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateRIArrangementLinesRI2007 executed ok" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdateRIArrangementLinesRI2007Request = Nothing
                oUpdateRIArrangementLinesRI2007Response = Nothing
            End Try

        End SyncLock

    End Sub
    ''' <summary>
    ''' ToBrokerParticipantList
    ''' </summary>
    ''' <param name="oImpList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToBrokerParticipantList(oImpList As BaseBrokerParticipants) As BaseBrokerParticipants

        Dim oservice As New BaseBrokerParticipants

        If oImpList IsNot Nothing Then
            With oImpList
                oservice.PartyKey = .PartyKey
                oservice.PartyCode = .PartyCode
                oservice.PartyName = .PartyName
                oservice.ParticpationPercentage = .ParticpationPercentage
            End With
        End If
        Return oservice
    End Function
    ''' <summary>
    ''' ToBaseRiskRIArrangementLineTypeList
    ''' </summary>
    ''' <param name="oImpList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToBaseRiskRIArrangementLineTypeList(ByVal oImpList As BaseRiskRIArrangementLineType) As BaseRiskRIArrangementLineType
        Dim oservice As New BaseRiskRIArrangementLineType
        If oImpList IsNot Nothing Then
            With oImpList

                oservice.ActionType = .ActionType
                oservice.AgreementCode = .AgreementCode
                oservice.CedePremiumOnly = .CedePremiumOnly
                oservice.CommissionPercent = .CommissionPercent
                oservice.CommissionTax = .CommissionTax
                If .CommissionTax > 0 Then
                    oservice.CommissionTaxSpecified = True
                End If

                oservice.CommissionValue = .CommissionValue
                oservice.IsCommissionModified = .IsCommissionModified
                oservice.DefaultSharePercent = .DefaultSharePercent
                oservice.Grouping = .Grouping
                oservice.IsDomiciledForTax = .IsDomiciledForTax

                If .Grouping > 0 Then
                    oservice.GroupingSpecified = True
                End If

                oservice.LineLimit = .LineLimit
                oservice.LowerLimit = .LowerLimit

                If .LowerLimit > 0 Then
                    oservice.LowerLimitSpecified = True

                End If

                oservice.NumberOfLines = .NumberOfLines
                oservice.ParticipationPercent = .ParticipationPercent

                If .ParticipationPercent > 0 Then
                    oservice.ParticipationPercentSpecified = True
                End If

                oservice.PartyKey = .PartyKey

                If .PartyKey > 0 Then
                    oservice.PartyKeySpecified = True
                End If

                oservice.PremiumPercent = .PremiumPercent
                oservice.FACPropPremiumPerc = .FACPropPremiumPerc
                oservice.PremiumTax = .PremiumTax

                If .PremiumTax > 0 Then
                    oservice.PremiumTaxSpecified = True
                End If

                oservice.PremiumValue = .PremiumValue
                oservice.Priority = .Priority
                oservice.Retained = .Retained

                If .Retained > 0 Then
                    oservice.RetainedSpecified = True
                End If

                oservice.SumInsured = .SumInsured
                oservice.ThisSharePercent = .ThisSharePercent
                oservice.TreatyCode = .TreatyCode
                oservice.Type = .Type
                oservice.IsRIBroker = .IsRIBroker
                oservice.ReinsuranceTypeCode = .ReinsuranceTypeCode
                oservice.RIArrangementKey = .RIArrangementKey
                oservice.RIArrangementLineKey = .RIArrangementLineKey
                oservice.RIName = .RIName
                oservice.RIPlacement = .RIPlacement
                ' PBI 35359: Send IsEditedDB (DB-persisted flag) through the converter
                oservice.IsEditedDB = .IsEditedDB
                oservice.IsPremiumEdited = .IsPremiumEdited
                'Broker Participents
                If .Type.Trim.ToUpper = "F" Then
                    If .BrokerParticipants IsNot Nothing AndAlso .BrokerParticipants.Count > 0 Then
                        Dim oBaseBrokerPart(.BrokerParticipants.Count - 1) As BaseBrokerParticipants

                        oservice.BrokerParticipants = .BrokerParticipants.ToList().ConvertAll(New Converter(Of BaseBrokerParticipants, BaseBrokerParticipants)(AddressOf ToBrokerParticipantList))
                    End If
                End If
                'FAX Praticipents
                If .Type.Trim.ToUpper = "FX" Then
                    If .FAXParticipants IsNot Nothing AndAlso .FAXParticipants.Count > 0 Then
                        Dim oBaseFAXParticipants(.FAXParticipants.Count - 1) As BaseFAXParticipants
                        oservice.FAXParticipants = .FAXParticipants.ToList().ConvertAll(New Converter(Of BaseFAXParticipants, BaseFAXParticipants)(AddressOf ToBaseFaxParticipants))
                    End If
                End If
            End With
        End If
        Return oservice

    End Function
    ''' <summary>
    ''' ToBaseFaxParticipants
    ''' </summary>
    ''' <param name="oImpList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToBaseFaxParticipants(ByVal oImpList As BaseFAXParticipants) As BaseFAXParticipants
        Dim oservice As New BaseFAXParticipants
        If oImpList IsNot Nothing Then

            With oImpList
                oservice.ParticpationPercentage = .ParticpationPercentage
                oservice.PartyCode = .PartyCode
                oservice.PartyKey = .PartyKey
                oservice.PartyName = .PartyName
                oservice.AccountType = .AccountType
                oservice.ActionType = .ActionType
                oservice.AgreementCode = .AgreementCode
                oservice.RIArrangementLineKey = .RIArrangementLineKey
                oservice.SumInsured = .SumInsured
                oservice.CommissionPercent = .CommissionPercent

                If .CommissionTax > 0 Then
                    oservice.CommissionTax = .CommissionTax
                    oservice.CommissionTaxSpecified = True
                Else
                    oservice.CommissionTaxSpecified = False
                End If

                oservice.CommissionValue = .CommissionValue

                If .PremiumTax > 0 Then
                    oservice.PremiumTax = .PremiumTax
                    oservice.PremiumTaxSpecified = True
                Else
                    oservice.PremiumTaxSpecified = False
                End If

                oservice.PremiumValue = .PremiumValue

                'FAX Broker Participent
                If .BrokerParticipants IsNot Nothing AndAlso .BrokerParticipants.Count > 0 Then
                    Dim oBaseFAXBrokerPart(.BrokerParticipants.Count - 1) As BaseBrokerParticipants

                    oservice.BrokerParticipants = .BrokerParticipants.ToList().ConvertAll(New Converter(Of BaseBrokerParticipants, BaseBrokerParticipants)(AddressOf ToBrokerParticipantList))
                End If
            End With

        End If
        Return oservice
    End Function
    ''' <summary>
    ''' ToBaseClaimRiskRIArrangementLineTypeList
    ''' </summary>
    ''' <param name="oImpList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToBaseClaimRiskRIArrangementLineTypeList(ByVal oImpList As BaseClaimRiskRIArrangementLineType) As BaseClaimRiskRIArrangementLineType
        Dim oservice As New BaseClaimRiskRIArrangementLineType
        If oImpList IsNot Nothing Then
            With oImpList
                oservice.ActionType = .ActionType
                oservice.AgreementCode = .AgreementCode

                If .Balance > 0 Then
                    oservice.Balance = .Balance
                    oservice.BalanceSpecified = True
                Else
                    oservice.BalanceSpecified = False
                End If

                oservice.CedePremiumOnly = .CedePremiumOnly
                oservice.DefaultSharePercent = .DefaultSharePercent

                If .Grouping > 0 Then
                    oservice.Grouping = .Grouping
                    oservice.GroupingSpecified = True
                Else
                    oservice.GroupingSpecified = False
                End If

                oservice.IsDomiciledForTax = .IsDomiciledForTax
                oservice.IsDomiciledForTaxSpecified = .IsDomiciledForTax
                oservice.IsRIBroker = .IsRIBroker
                oservice.IsRIBrokerSpecified = .IsRIBroker
                oservice.LineLimit = .LineLimit

                If .LowerLimit > 0 Then
                    oservice.LowerLimit = .LowerLimit
                    oservice.LowerLimitSpecified = True
                Else
                    oservice.LowerLimitSpecified = False
                End If

                oservice.NumberOfLines = .NumberOfLines

                If .PartyKey > 0 Then
                    oservice.PartyKey = .PartyKey
                    oservice.PartyKeySpecified = True
                Else
                    oservice.PartyKeySpecified = False
                End If

                oservice.PaymentToDate = .PaymentToDate
                oservice.Priority = .Priority

                If .RecoverToDate > 0 Then
                    oservice.RecoverToDate = .RecoverToDate
                    oservice.RecoverToDateSpecified = True
                Else
                    oservice.RecoverToDateSpecified = False
                End If

                oservice.ReinsuranceTypeCode = .ReinsuranceTypeCode
                oservice.ReserveToDate = .ReserveToDate

                If .Retained > 0 Then
                    oservice.Retained = .Retained
                    oservice.RetainedSpecified = True
                Else
                    oservice.RetainedSpecified = False
                End If

                oservice.RIArrangementKey = .RIArrangementKey
                oservice.RIArrangementLineKey = .RIArrangementLineKey
                oservice.RIName = .RIName
                oservice.RIPlacement = .RIPlacement
                oservice.SumInsured = .SumInsured
                oservice.ThisPayment = .ThisPayment
                oservice.ThisReserve = .ThisReserve
                oservice.ThisSharePercent = .ThisSharePercent
                oservice.TreatyCode = .TreatyCode
                oservice.Type = .Type
                If .BrokerParticipants.Count > 0 Then
                    oservice.BrokerParticipants = .BrokerParticipants.ToList().ConvertAll(New Converter(Of BaseBrokerParticipants, BaseBrokerParticipants)(AddressOf ToBrokerParticipantList))
                End If

                If .FAXParticipants IsNot Nothing AndAlso .FAXParticipants.Count > 0 Then
                    oservice.FAXParticipants = .FAXParticipants.ToList().ConvertAll(New Converter(Of BaseClaimFAXParticipants, BaseClaimFAXParticipants)(AddressOf ToClaimFAXParticipantList))
                End If
            End With
        End If
        Return oservice
    End Function
    ''' <summary>
    ''' ToClaimFAXParticipantList
    ''' </summary>
    ''' <param name="oImpList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToClaimFAXParticipantList(ByVal oImpList As BaseClaimFAXParticipants) As BaseClaimFAXParticipants
        Dim oservice As New BaseClaimFAXParticipants
        If oImpList IsNot Nothing Then
            With oImpList
                oservice.AccountType = .AccountType
                oservice.AgreementCode = .AgreementCode
                oservice.ParticpationPercentage = .ParticpationPercentage
                oservice.PartyCode = .PartyCode
                oservice.PartyKey = .PartyKey
                oservice.PartyName = .PartyName
                oservice.RIArrangementLineKey = .RIArrangementLineKey
                oservice.SumInsured = .SumInsured
                oservice.ActionType = .ActionType
                oservice.PaymentToDate = .PaymentToDate

                If .RecoverToDate > 0 Then
                    oservice.RecoverToDate = .RecoverToDate
                    oservice.RecoverToDateSpecified = True
                Else
                    oservice.RecoverToDateSpecified = False
                End If

                oservice.ReserveToDate = .ReserveToDate
                oservice.ThisPayment = .ThisPayment
                oservice.ThisReserve = .ThisReserve

                If .BrokerParticipants.Count > 0 Then
                    oservice.BrokerParticipants = .BrokerParticipants.ToList().ConvertAll(New Converter(Of BaseBrokerParticipants, BaseBrokerParticipants)(AddressOf ToBrokerParticipantList))
                End If

            End With

        End If

        Return oservice
    End Function
    ''' <summary>
    ''' This Method Update the Added/Changed RI Line into Database for Claim
    ''' </summary>
    ''' <param name="v_oRIArrangementLines"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateClaimRIArrangementLinesRI2007(ByVal v_oRIArrangementLines As ArrangementLinesTypeCollection, _
                                  Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oUpdateClaimRIArrangementLinesRI2007Request As UpdateClaimRIArrangementLinesRI2007RequestType
            Dim oUpdateClaimRIArrangementLinesRI2007Response As UpdateClaimRIArrangementLinesRI2007ResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                sbLogMessage = New StringBuilder
                oUpdateClaimRIArrangementLinesRI2007Request = New UpdateClaimRIArrangementLinesRI2007RequestType
                oUpdateClaimRIArrangementLinesRI2007Response = New UpdateClaimRIArrangementLinesRI2007ResponseType

                With oUpdateClaimRIArrangementLinesRI2007Request
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_oRIArrangementLines.Count > 0 Then
                        Dim oRIArrangementLine(v_oRIArrangementLines.Count - 1) As BaseClaimRiskRIArrangementLineType
                        .ClaimRIArrangementLines = oRIArrangementLine.ToList().ConvertAll(New Converter(Of BaseClaimRiskRIArrangementLineType, BaseClaimRiskRIArrangementLineType)(AddressOf ToBaseClaimRiskRIArrangementLineTypeList))
                    End If
                End With

                Using trace As New Tracer(Category.Trace)
                    oUpdateClaimRIArrangementLinesRI2007Response = oSAM.UpdateClaimRIArrangementLinesRI2007(oUpdateClaimRIArrangementLinesRI2007Request)
                End Using

                With oUpdateClaimRIArrangementLinesRI2007Response

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateClaimRIArrangementLinesRI2007 executed ok" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdateClaimRIArrangementLinesRI2007Request = Nothing
                oUpdateClaimRIArrangementLinesRI2007Response = Nothing
            End Try

        End SyncLock

    End Sub
    ''' <summary>
    '''  This Method Return the dataset for NB/MTA/MTR/MTC/REN Reinsurance, If RI2007 is OFF
    ''' </summary>
    ''' <param name="iRiskKey"></param>
    ''' <param name="bIsRI2007"></param>
    ''' <param name="sReturnXML"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetReinsurance(ByVal iRiskKey As Integer, _
                                           Optional ByVal bIsRI2007 As Boolean = False, _
                                           Optional ByRef sReturnXML As String = Nothing, _
                                           Optional ByVal oPreFetchedBandsCollection As NexusProvider.ReinsuranceBandsCollection = Nothing) As DataSet

        Dim oReinsurarerBandCollection As NexusProvider.ReinsuranceBandsCollection
        Dim dsArrangementGridData As New DataSet
        Dim dsParticipentGridData As New DataSet
        Dim dtCurParticipent As New DataTable("Current_Broker_Participent")
        Dim dtOrgParticipent As New DataTable("Original_Broker_Participent")
        Dim dtCurFAXBrokerParticipent As New DataTable("Current_FAX_Broker_Participent")
        Dim dtOrgFAXBrokerParticipent As New DataTable("Original_FAX_Broker_Participent")
        Dim dtCurFAXParticipent As New DataTable("Current_FAX_Participent")
        Dim dtOrgFAXParticipent As New DataTable("Original_FAX_Participent")

        If oPreFetchedBandsCollection IsNot Nothing AndAlso oPreFetchedBandsCollection.Count > 0 Then
            oReinsurarerBandCollection = oPreFetchedBandsCollection
        Else
            oReinsurarerBandCollection = GetRiskReinsuranceBands(iRiskKey)
        End If

        If bIsRI2007 Then
            'RI 2007 is ON
            'FAX Participent Table
            dtCurFAXParticipent.Columns.Add("IsNew", GetType(Boolean))
            dtCurFAXParticipent.Columns.Add("IsEdited", GetType(Boolean))
            dtCurFAXParticipent.Columns.Add("IsDeleted", GetType(Boolean))
            dtCurFAXParticipent.Columns.Add("PartyKey", GetType(Integer))
            dtCurFAXParticipent.Columns.Add("ParticipationPercent", GetType(Double))
            dtCurFAXParticipent.Columns.Add("PartyCode", GetType(String))
            dtCurFAXParticipent.Columns.Add("PartyName", GetType(String))
            dtCurFAXParticipent.Columns.Add("AccountType", GetType(String))
            dtCurFAXParticipent.Columns.Add("AgreementCode", GetType(String))
            dtCurFAXParticipent.Columns.Add("CommissionPercent", GetType(Double))
            dtCurFAXParticipent.Columns.Add("CommissionTax", GetType(Double))
            dtCurFAXParticipent.Columns.Add("CommissionValue", GetType(Double))
            dtCurFAXParticipent.Columns.Add("PremiumTax", GetType(Double))
            dtCurFAXParticipent.Columns.Add("PremiumValue", GetType(Double))
            dtCurFAXParticipent.Columns.Add("SumInsured", GetType(Double))
            dtCurFAXParticipent.Columns.Add("RIArrangementLineKey", GetType(Integer))
            dtCurFAXParticipent.Columns.Add("RIarrangementKey", GetType(Integer))
            dtCurFAXParticipent.Columns.Add("Grouping", GetType(Integer))
            dsParticipentGridData.Tables.Add(dtCurFAXParticipent)

            'Original FAX Participent Table
            dtOrgFAXParticipent.Columns.Add("PartyKey", GetType(Integer))
            dtOrgFAXParticipent.Columns.Add("ParticipationPercent", GetType(Double))
            dtOrgFAXParticipent.Columns.Add("PartyCode", GetType(String))
            dtOrgFAXParticipent.Columns.Add("PartyName", GetType(String))
            dtOrgFAXParticipent.Columns.Add("AccountType", GetType(String))
            dtOrgFAXParticipent.Columns.Add("AgreementCode", GetType(String))
            dtOrgFAXParticipent.Columns.Add("CommissionPercent", GetType(Double))
            dtOrgFAXParticipent.Columns.Add("CommissionTax", GetType(Double))
            dtOrgFAXParticipent.Columns.Add("CommissionValue", GetType(Double))
            dtOrgFAXParticipent.Columns.Add("PremiumTax", GetType(Double))
            dtOrgFAXParticipent.Columns.Add("PremiumValue", GetType(Double))
            dtOrgFAXParticipent.Columns.Add("SumInsured", GetType(Double))
            dtOrgFAXParticipent.Columns.Add("RIArrangementLineKey", GetType(Integer))
            dtOrgFAXParticipent.Columns.Add("RIarrangementKey", GetType(Integer))
            dtOrgFAXParticipent.Columns.Add("Grouping", GetType(Integer))
            dsParticipentGridData.Tables.Add(dtOrgFAXParticipent)

            'FAX Broker Participent Table
            dtCurFAXBrokerParticipent.Columns.Add("IsNew", GetType(Boolean))
            dtCurFAXBrokerParticipent.Columns.Add("IsEdited", GetType(Boolean))
            dtCurFAXBrokerParticipent.Columns.Add("IsDeleted", GetType(Boolean))
            dtCurFAXBrokerParticipent.Columns.Add("PartyKey", GetType(Integer))
            dtCurFAXBrokerParticipent.Columns.Add("ParticipationPercent", GetType(Double))
            dtCurFAXBrokerParticipent.Columns.Add("RIArrangementLineKey", GetType(Integer))
            dtCurFAXBrokerParticipent.Columns.Add("RIarrangementKey", GetType(Integer))
            dtCurFAXBrokerParticipent.Columns.Add("PartyCode", GetType(String))
            dtCurFAXBrokerParticipent.Columns.Add("PartyName", GetType(String))
            dsParticipentGridData.Tables.Add(dtCurFAXBrokerParticipent)

            'Original FAX Broker Paricipent Table
            dtOrgFAXBrokerParticipent.Columns.Add("PartyKey", GetType(Integer))
            dtOrgFAXBrokerParticipent.Columns.Add("ParticipationPercent", GetType(Double))
            dtOrgFAXBrokerParticipent.Columns.Add("RIArrangementLineKey", GetType(Integer))
            dtOrgFAXBrokerParticipent.Columns.Add("RIarrangementKey", GetType(Integer))
            dtOrgFAXBrokerParticipent.Columns.Add("PartyCode", GetType(String))
            dtOrgFAXBrokerParticipent.Columns.Add("PartyName", GetType(String))
            dsParticipentGridData.Tables.Add(dtOrgFAXBrokerParticipent)

            'Broker Paricipent Table
            dtCurParticipent.Columns.Add("IsNew", GetType(Boolean))
            dtCurParticipent.Columns.Add("IsEdited", GetType(Boolean))
            dtCurParticipent.Columns.Add("IsDeleted", GetType(Boolean))
            dtCurParticipent.Columns.Add("PartyKey", GetType(Integer))
            dtCurParticipent.Columns.Add("ParticipationPercent", GetType(Double))
            dtCurParticipent.Columns.Add("RIArrangementLineKey", GetType(Integer))
            dtCurParticipent.Columns.Add("RIarrangementKey", GetType(Integer))
            dtCurParticipent.Columns.Add("PartyCode", GetType(String))
            dtCurParticipent.Columns.Add("PartyName", GetType(String))
            dsParticipentGridData.Tables.Add(dtCurParticipent)

            'Original Broker Paricipent Table
            dtOrgParticipent.Columns.Add("PartyKey", GetType(Integer))
            dtOrgParticipent.Columns.Add("ParticipationPercent", GetType(Double))
            dtOrgParticipent.Columns.Add("RIArrangementLineKey", GetType(Integer))
            dtOrgParticipent.Columns.Add("RIarrangementKey", GetType(Integer))
            dtOrgParticipent.Columns.Add("PartyCode", GetType(String))
            dtOrgParticipent.Columns.Add("PartyName", GetType(String))
            dsParticipentGridData.Tables.Add(dtOrgParticipent)
        End If

        For Each oReinsuranceBands As NexusProvider.ReinsuranceBands In oReinsurarerBandCollection
            Dim dtArrangements As New DataTable("Current_" & oReinsuranceBands.BandKey.ToString)
            Dim dtOrgArrangements As New DataTable("Original_" & oReinsuranceBands.BandKey.ToString)

            Dim iArrangementId As Integer
            Dim iOrgArrangementId As Integer
            Dim drArrangementGridRow As DataRow

            Dim drOrgArrangementGridRow As DataRow
            Dim dSumAssured, dPremium As Double
            Dim dAllocatedTax, dAllocatedPremium, dAllocatedThisPerc, dAllocatedSumAssured, dAllocatedCommision, dAllocatedCommisionTax As Double
            Dim dOriginalSumAssuredTotal, dOriginalPremiumTotal As Decimal
            dAllocatedTax = 0
            dAllocatedPremium = 0
            dAllocatedThisPerc = 0
            dAllocatedSumAssured = 0
            dAllocatedCommision = 0
            dAllocatedCommisionTax = 0
            dSumAssured = 0
            dPremium = 0
            dOriginalSumAssuredTotal = 0
            dOriginalPremiumTotal = 0
            If bIsRI2007 Then
                'RI 2007 is ON

                ' declaring table for adding into the dataset
                dtArrangements.Columns.Add("IsNew", GetType(Boolean))
                dtArrangements.Columns.Add("IsEdited", GetType(Boolean))
                dtArrangements.Columns.Add("IsEditedDB", GetType(Boolean))
                dtArrangements.Columns.Add("IsPremiumEdited", GetType(Boolean))
                dtArrangements.Columns.Add("IsDeleted", GetType(Boolean))
                dtArrangements.Columns.Add("Type", GetType(String))
                dtArrangements.Columns.Add("TreatyCode", GetType(String))
                dtArrangements.Columns.Add("RIArrangementLineKey", GetType(Integer))
                dtArrangements.Columns.Add("RIarrangementKey", GetType(Integer))
                dtArrangements.Columns.Add("Retained", GetType(Double))
                dtArrangements.Columns.Add("ReinsuranceTypeCode", GetType(String))
                dtArrangements.Columns.Add("Priority", GetType(Integer))
                dtArrangements.Columns.Add("PremiumPercent", GetType(Double))
                dtArrangements.Columns.Add("PartyKey", GetType(Integer))
                dtArrangements.Columns.Add("ParticipationPercent", GetType(Double))
                dtArrangements.Columns.Add("NumberOfLines", GetType(Decimal))
                dtArrangements.Columns.Add("LineLimit", GetType(Double))
                dtArrangements.Columns.Add("LowerLimit", GetType(Double))
                dtArrangements.Columns.Add("IsRIBroker", GetType(Boolean))
                dtArrangements.Columns.Add("IsDomiciledForTax", GetType(Boolean))
                dtArrangements.Columns.Add("IsCommissionModified", GetType(Boolean))
                dtArrangements.Columns.Add("Grouping", GetType(Integer))
                dtArrangements.Columns.Add("CedePremiumOnly", GetType(Double))
                dtArrangements.Columns.Add("ActionType", GetType(RowAction))
                dtArrangements.Columns.Add("Placement", GetType(String))
                dtArrangements.Columns.Add("Name", GetType(String))
                dtArrangements.Columns.Add("DefaultPerc", GetType(String))
                dtArrangements.Columns.Add("ThisPerc", GetType(String))
                dtArrangements.Columns.Add("SumInsured", GetType(String))
                dtArrangements.Columns.Add("Premium", GetType(String))
                dtArrangements.Columns.Add("Tax", GetType(String))
                dtArrangements.Columns.Add("CommissionPerc", GetType(String))
                dtArrangements.Columns.Add("Commission", GetType(String))
                dtArrangements.Columns.Add("CommissionTax", GetType(String))
                dtArrangements.Columns.Add("Agreement", GetType(String))
                dtArrangements.Columns.Add("ExtendedLimitAmount", GetType(Double))
                dtArrangements.Columns.Add("IsExtendedLimitApplied", GetType(Boolean))
                dsArrangementGridData.Tables.Add(dtArrangements)

                dtOrgArrangements.Columns.Add("Type", GetType(String))
                dtOrgArrangements.Columns.Add("TreatyCode", GetType(String))
                dtOrgArrangements.Columns.Add("RIArrangementLineKey", GetType(Integer))
                dtOrgArrangements.Columns.Add("RIarrangementKey", GetType(Integer))
                dtOrgArrangements.Columns.Add("Retained", GetType(Double))
                dtOrgArrangements.Columns.Add("ReinsuranceTypeCode", GetType(String))
                dtOrgArrangements.Columns.Add("Priority", GetType(Integer))
                dtOrgArrangements.Columns.Add("PremiumPercent", GetType(Double))
                dtOrgArrangements.Columns.Add("PartyKey", GetType(Integer))
                dtOrgArrangements.Columns.Add("ParticipationPercent", GetType(Double))
                dtOrgArrangements.Columns.Add("NumberOfLines", GetType(Decimal))
                dtOrgArrangements.Columns.Add("LineLimit", GetType(Double))
                dtOrgArrangements.Columns.Add("LowerLimit", GetType(Double))
                dtOrgArrangements.Columns.Add("IsRIBroker", GetType(Boolean))
                dtOrgArrangements.Columns.Add("IsDomiciledForTax", GetType(Boolean))
                dtOrgArrangements.Columns.Add("IsCommissionModified", GetType(Boolean))
                dtOrgArrangements.Columns.Add("Grouping", GetType(Integer))
                dtOrgArrangements.Columns.Add("CedePremiumOnly", GetType(Double))
                dtOrgArrangements.Columns.Add("ActionType", GetType(RowAction))
                dtOrgArrangements.Columns.Add("Placement", GetType(String))
                dtOrgArrangements.Columns.Add("Name", GetType(String))
                dtOrgArrangements.Columns.Add("DefaultPerc", GetType(String))
                dtOrgArrangements.Columns.Add("ThisPerc", GetType(String))
                dtOrgArrangements.Columns.Add("SumInsured", GetType(String))
                dtOrgArrangements.Columns.Add("Premium", GetType(String))
                dtOrgArrangements.Columns.Add("Tax", GetType(String))
                dtOrgArrangements.Columns.Add("CommissionPerc", GetType(String))
                dtOrgArrangements.Columns.Add("Commission", GetType(String))
                dtOrgArrangements.Columns.Add("CommissionTax", GetType(String))
                dtOrgArrangements.Columns.Add("Agreement", GetType(String))
                ' dsOrgArrangementGridData.Tables.Add(dtOrgArrangements)
                dsArrangementGridData.Tables.Add(dtOrgArrangements)

            Else
                'RI 2007 is OFF
                ' declaring table for adding into the dataset
                dtArrangements.Columns.Add("Name", GetType(String))
                dtArrangements.Columns.Add("DefaultPerc", GetType(String))
                dtArrangements.Columns.Add("ThisPerc", GetType(String))
                dtArrangements.Columns.Add("SumInsured", GetType(String))
                dtArrangements.Columns.Add("Premium", GetType(String))
                dtArrangements.Columns.Add("Tax", GetType(String))
                dtArrangements.Columns.Add("CommissionPerc", GetType(String))
                dtArrangements.Columns.Add("Commission", GetType(String))
                dtArrangements.Columns.Add("CommissionTax", GetType(String))
                dtArrangements.Columns.Add("Agreement", GetType(String))
                dsArrangementGridData.Tables.Add(dtArrangements)

                dtOrgArrangements.Columns.Add("Name", GetType(String))
                dtOrgArrangements.Columns.Add("DefaultPerc", GetType(String))
                dtOrgArrangements.Columns.Add("ThisPerc", GetType(String))
                dtOrgArrangements.Columns.Add("SumInsured", GetType(String))
                dtOrgArrangements.Columns.Add("Premium", GetType(String))
                dtOrgArrangements.Columns.Add("Tax", GetType(String))
                dtOrgArrangements.Columns.Add("CommissionPerc", GetType(String))
                dtOrgArrangements.Columns.Add("Commission", GetType(String))
                dtOrgArrangements.Columns.Add("CommissionTax", GetType(String))
                dtOrgArrangements.Columns.Add("Agreement", GetType(String))
                dsArrangementGridData.Tables.Add(dtOrgArrangements)

            End If

            ' Obtaining the value of ArrangementsType for specific riskkey from SAM
            Dim oReinsuranceTypeArragementsCollection As NexusProvider.ArrangementsTypeCollection = Nothing

            oReinsuranceTypeArragementsCollection = GetRiskReinsuranceArrangements(iRiskKey)

            ' PBI 35359: Supplement RiOverrideReasonId from DB - REST API may not return it.
            Try
                If oReinsuranceTypeArragementsCollection IsNot Nothing AndAlso oReinsuranceTypeArragementsCollection.Count > 0 Then
                    Dim oCmdArr As New System.Data.SqlClient.SqlCommand("spu_RI_Arrangement_saa")
                    oCmdArr.CommandType = System.Data.CommandType.StoredProcedure
                    oCmdArr.Parameters.AddWithValue("@risk_cnt", iRiskKey)
                    Dim oDrArr As System.Data.SqlClient.SqlDataReader = Nexus.Utils.funcDB.ExecSql(oCmdArr)
                    Dim dictOverride As New Dictionary(Of Integer, Integer)
                    While oDrArr.Read()
                        Dim iArrId As Integer = 0
                        Dim iReasonId As Integer = 0
                        If Not IsDBNull(oDrArr("ri_arrangement_id")) Then Integer.TryParse(oDrArr("ri_arrangement_id").ToString(), iArrId)
                        If Not IsDBNull(oDrArr("rioverridereasonid")) Then Integer.TryParse(oDrArr("rioverridereasonid").ToString(), iReasonId)
                        If iArrId > 0 Then dictOverride(iArrId) = iReasonId
                    End While
                    oDrArr.Close()
                    For Each oArr As NexusProvider.ArrangementsType In oReinsuranceTypeArragementsCollection
                        Dim iReason As Integer = 0
                        If dictOverride.TryGetValue(oArr.ArrangementId, iReason) Then
                            oArr.RiOverrideReasonId = iReason
                        End If
                    Next
                End If
            Catch : End Try

            'iterating through and adding the selected bands values into the dataset
            'While doing NB IsOriginal is False, for MTA - IsOriginal is True.
            For Each oArrangementType As NexusProvider.ArrangementsType In oReinsuranceTypeArragementsCollection
                If oArrangementType.BandId = oReinsuranceBands.BandKey Then
                    ' PBI 35359: Store OverrideReasonId in DataTable ExtendedProperties for RIBAND XML stamping
                    If Not oArrangementType.IsOriginal Then
                        dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).ExtendedProperties("OverrideReasonId") = oArrangementType.RiOverrideReasonId
                    Else
                        dsArrangementGridData.Tables("Original_" & oArrangementType.BandId.ToString).ExtendedProperties("OverrideReasonId") = oArrangementType.RiOverrideReasonId
                    End If
                    With oArrangementType
                        If .IsOriginal Then
                            iOrgArrangementId = oArrangementType.ArrangementId

                            ' Obtaining the value of ArrangementLinesType for specific riskkey from SAM
                            Dim oReinsuranceArrangmentCollection As NexusProvider.ArrangementLinesTypeCollection

                            If bIsRI2007 Then
                                'RI 2007 is ON
                                oReinsuranceArrangmentCollection = GetRiskReinsuranceArrangementLinesRI2007(iOrgArrangementId)

                                drOrgArrangementGridRow = dsArrangementGridData.Tables("Original_" & oArrangementType.BandId.ToString).NewRow
                                drOrgArrangementGridRow("Placement") = "GROSS"
                                drOrgArrangementGridRow("Name") = "Band Total"
                                drOrgArrangementGridRow("SumInsured") = Format(Math.Round(.SumInsured, 2), "0.00")
                                drOrgArrangementGridRow("Premium") = Format(Math.Round(.Premium, 2), "0.00")
                                dOriginalSumAssuredTotal = Format(Math.Round(.SumInsured, 2), "0.00")
                                dOriginalPremiumTotal = Format(Math.Round(.Premium, 2), "0.00")
                                dsArrangementGridData.Tables("Original_" & oArrangementType.BandId.ToString).Rows.Add(drOrgArrangementGridRow)
                                drOrgArrangementGridRow = Nothing

                                For Each oArrangementLinesType As NexusProvider.ArrangementLinesType In oReinsuranceArrangmentCollection
                                    'This below condition only allow if dataset has any row else it will not allow inside the condition
                                    If dsArrangementGridData IsNot Nothing AndAlso dsArrangementGridData.Tables("Original_" & oArrangementType.BandId.ToString).Rows.Count > 0 Then

                                        With oArrangementLinesType
                                            drOrgArrangementGridRow = dsArrangementGridData.Tables("Original_" & oArrangementType.BandId.ToString).NewRow

                                            drOrgArrangementGridRow("Type") = .Type
                                            drOrgArrangementGridRow("TreatyCode") = .TreatyCode
                                            drOrgArrangementGridRow("RIArrangementLineKey") = .RIArrangementLineKey
                                            drOrgArrangementGridRow("RIarrangementKey") = .RIarrangementKey
                                            drOrgArrangementGridRow("Retained") = .Retained
                                            drOrgArrangementGridRow("ReinsuranceTypeCode") = .ReinsuranceTypeCode
                                            drOrgArrangementGridRow("Priority") = .Priority
                                            drOrgArrangementGridRow("PremiumPercent") = Format(Math.Round(.PremiumPercent, 2), "0.00")
                                            drOrgArrangementGridRow("PartyKey") = .PartyKey
                                            drOrgArrangementGridRow("ParticipationPercent") = Format(Math.Round(.ParticipationPercent, 4), "0.00")
                                            drOrgArrangementGridRow("NumberOfLines") = .NumberOfLines
                                            drOrgArrangementGridRow("LineLimit") = Format(Math.Round(.LineLimit, 2), "0.00")
                                            drOrgArrangementGridRow("LowerLimit") = Format(Math.Round(.LowerLimit, 2), "0.00")
                                            drOrgArrangementGridRow("IsRIBroker") = .IsRIBroker
                                            drOrgArrangementGridRow("IsDomiciledForTax") = .IsDomiciledForTax
                                            drOrgArrangementGridRow("IsCommissionModified") = .IsCommissionModified
                                            drOrgArrangementGridRow("Grouping") = .Grouping
                                            drOrgArrangementGridRow("CedePremiumOnly") = .CedePremiumOnly
                                            drOrgArrangementGridRow("ActionType") = .ActionType
                                            drOrgArrangementGridRow("Placement") = .RIPlacement
                                            drOrgArrangementGridRow("Name") = .RIName
                                            drOrgArrangementGridRow("DefaultPerc") = Format(Math.Round(CType(.DefaultPerc, Decimal), 4), "0.0000")
                                            drOrgArrangementGridRow("ThisPerc") = .ThisPerc 'Format(Math.Round(.ThisPerc, 4), "0.0000")
                                            drOrgArrangementGridRow("SumInsured") = Format(Math.Round(.SumInsured, 2), "0.00")
                                            drOrgArrangementGridRow("Premium") = Format(Math.Round(.PremiumValue, 2), "0.00")
                                            drOrgArrangementGridRow("Tax") = Format(Math.Round(.PremiumTax, 2), "0.00")
                                            drOrgArrangementGridRow("CommissionPerc") = Format(Math.Round(.CommissionPerc, 2), "0.00")
                                            drOrgArrangementGridRow("Commission") = Format(Math.Round(.CommissionValue, 2), "0.00")
                                            drOrgArrangementGridRow("CommissionTax") = Format(Math.Round(.CommissionTax, 2), "0.00")
                                            drOrgArrangementGridRow("Agreement") = .AgreementCode

                                            dSumAssured = dSumAssured + .SumInsured
                                            dPremium = dPremium + .PremiumValue

                                            ' FAX Participent (FAC XOL)
                                            If .FAXParticipants IsNot Nothing AndAlso .FAXParticipants.Count > 0 Then
                                                For Each oFAXPaticipent As FAXParticipants In .FAXParticipants
                                                    Dim drFAXParticipent As DataRow
                                                    With oFAXPaticipent
                                                        drFAXParticipent = dsParticipentGridData.Tables("Original_FAX_Participent").NewRow
                                                        drFAXParticipent("PartyKey") = .PartyKey
                                                        drFAXParticipent("ParticipationPercent") = Math.Round(.ParticipationPercentage, 4)
                                                        drFAXParticipent("PartyCode") = .PartyCode
                                                        drFAXParticipent("PartyName") = .PartyName
                                                        drFAXParticipent("AccountType") = .AccountType
                                                        drFAXParticipent("AgreementCode") = .AgreementCode
                                                        drFAXParticipent("CommissionPercent") = .CommissionPercent
                                                        drFAXParticipent("CommissionTax") = .CommissionTax
                                                        drFAXParticipent("CommissionValue") = .CommissionValue
                                                        drFAXParticipent("PremiumTax") = .PremiumTax
                                                        drFAXParticipent("PremiumValue") = .PremiumValue
                                                        drFAXParticipent("SumInsured") = .SumInsured
                                                        drFAXParticipent("RIArrangementLineKey") = .RIArrangementLineKey
                                                        drFAXParticipent("RIarrangementKey") = oArrangementLinesType.RIarrangementKey
                                                        drFAXParticipent("Grouping") = oArrangementLinesType.Grouping

                                                        'FAX Broker Participent
                                                        If .BrokerParticipants IsNot Nothing AndAlso .BrokerParticipants.Count > 0 Then
                                                            For Each oPaticipent As BrokerParticipants In .BrokerParticipants
                                                                Dim drParticipent As DataRow
                                                                With oPaticipent
                                                                    drParticipent = dsParticipentGridData.Tables("Original_FAX_Broker_Participent").NewRow
                                                                    drParticipent("PartyCode") = .PartyCode
                                                                    drParticipent("PartyName") = .PartyName
                                                                    drParticipent("PartyKey") = .PartyKey
                                                                    drParticipent("ParticipationPercent") = .ParticipationPercentage
                                                                    drParticipent("RIArrangementLineKey") = oFAXPaticipent.RIArrangementLineKey
                                                                    drParticipent("RIarrangementKey") = oArrangementLinesType.RIarrangementKey
                                                                End With

                                                                dsParticipentGridData.Tables("Original_FAX_Broker_Participent").Rows.Add(drParticipent)
                                                            Next
                                                        End If
                                                    End With
                                                    dsParticipentGridData.Tables("Original_FAX_Participent").Rows.Add(drFAXParticipent)
                                                Next
                                            End If

                                            'Broker Participent (FAC Prop)
                                            If .BrokerParticipants IsNot Nothing AndAlso .BrokerParticipants.Count > 0 Then
                                                For Each oPaticipent As BrokerParticipants In .BrokerParticipants
                                                    Dim drParticipent As DataRow
                                                    With oPaticipent
                                                        drParticipent = dsParticipentGridData.Tables("Original_Broker_Participent").NewRow
                                                        drParticipent("PartyCode") = .PartyCode
                                                        drParticipent("PartyName") = .PartyName
                                                        drParticipent("PartyKey") = .PartyKey
                                                        drParticipent("ParticipationPercent") = .ParticipationPercentage
                                                        drParticipent("RIArrangementLineKey") = oArrangementLinesType.RIArrangementLineKey
                                                        drParticipent("RIarrangementKey") = oArrangementLinesType.RIarrangementKey
                                                    End With

                                                    dsParticipentGridData.Tables("Original_Broker_Participent").Rows.Add(drParticipent)
                                                Next
                                            End If
                                        End With

                                        dsArrangementGridData.Tables("Original_" & oArrangementType.BandId.ToString).Rows.Add(drOrgArrangementGridRow)
                                    End If
                                    ' adding rows to the table
                                    drOrgArrangementGridRow = Nothing
                                Next
                            Else
                                'RI 2007 is OFF
                                oReinsuranceArrangmentCollection = GetRiskReinsuranceArrangementLines(iOrgArrangementId)

                                drOrgArrangementGridRow = dsArrangementGridData.Tables("Original_" & oArrangementType.BandId.ToString).NewRow

                                drOrgArrangementGridRow("Name") = "Band Total"
                                drOrgArrangementGridRow("SumInsured") = New Money(.SumInsured, Current.Session(CNCurrenyCode)).Formatted
                                drOrgArrangementGridRow("Premium") = New Money(.Premium, Current.Session(CNCurrenyCode)).Formatted
                                dsArrangementGridData.Tables("Original_" & oArrangementType.BandId.ToString).Rows.Add(drOrgArrangementGridRow)
                                drOrgArrangementGridRow = Nothing

                                For Each oArrangementLinesType As NexusProvider.ArrangementLinesType In oReinsuranceArrangmentCollection
                                    'This below condition only allow if dataset has any row else it will not allow inside the condition
                                    If dsArrangementGridData IsNot Nothing AndAlso dsArrangementGridData.Tables("Original_" & oArrangementType.BandId.ToString).Rows.Count > 0 Then
                                        With oArrangementLinesType
                                            drOrgArrangementGridRow = dsArrangementGridData.Tables("Original_" & oArrangementType.BandId.ToString).NewRow
                                            drOrgArrangementGridRow("Name") = .Name
                                            drOrgArrangementGridRow("DefaultPerc") = CType(.DefaultPerc, Decimal)
                                            drOrgArrangementGridRow("ThisPerc") = .ThisPerc
                                            drOrgArrangementGridRow("SumInsured") = New Money(.SumInsured, Current.Session(CNCurrenyCode)).Formatted
                                            drOrgArrangementGridRow("Premium") = New Money(.PremiumValue, Current.Session(CNCurrenyCode)).Formatted
                                            drOrgArrangementGridRow("Tax") = New Money(.Tax, Current.Session(CNCurrenyCode)).Formatted
                                            drOrgArrangementGridRow("CommissionPerc") = .CommissionPerc
                                            drOrgArrangementGridRow("Commission") = New Money(.CommissionValue, Current.Session(CNCurrenyCode)).Formatted
                                            drOrgArrangementGridRow("CommissionTax") = New Money(.CommissionTax, Current.Session(CNCurrenyCode)).Formatted
                                            drOrgArrangementGridRow("Agreement") = .AgreementCode

                                            dSumAssured = dSumAssured + .SumInsured
                                            dPremium = dPremium + .PremiumValue
                                        End With

                                        dsArrangementGridData.Tables("Original_" & oArrangementType.BandId.ToString).Rows.Add(drOrgArrangementGridRow)
                                    End If

                                    ' adding rows to the table

                                    drOrgArrangementGridRow = Nothing
                                Next
                            End If
                        Else
                            iArrangementId = oArrangementType.ArrangementId

                            ' Obtaining the value of ArrangementLinesType for specific riskkey from SAM
                            Dim oReinsuranceArrangmentCollection As NexusProvider.ArrangementLinesTypeCollection

                            If bIsRI2007 Then
                                'RI 2007 is ON

                                oReinsuranceArrangmentCollection = GetRiskReinsuranceArrangementLinesRI2007(iArrangementId)

                                drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).NewRow
                                drArrangementGridRow("Placement") = "GROSS"
                                drArrangementGridRow("Name") = "Band Total"
                                drArrangementGridRow("SumInsured") = Format(Math.Round(.SumInsured, 2), "0.00")
                                drArrangementGridRow("Premium") = Format(Math.Round(.Premium, 2), "0.00")
                                drArrangementGridRow("IsDeleted") = False
                                drArrangementGridRow("RIarrangementKey") = iArrangementId
                                drArrangementGridRow("ExtendedLimitAmount") = .ExtendedLimitAmount
                                drArrangementGridRow("IsExtendedLimitApplied") = .IsExtendedLimitapplied
                                dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).Rows.Add(drArrangementGridRow)
                                drArrangementGridRow = Nothing

                                ' iterating through and adding the ArrangementLine values into the dataset
                                For Each oArrangementLinesType As NexusProvider.ArrangementLinesType In oReinsuranceArrangmentCollection

                                    With oArrangementLinesType
                                        drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).NewRow

                                        drArrangementGridRow("IsNew") = False
                                        drArrangementGridRow("IsDeleted") = False
                                        ' PBI 35359 refactor: IsEdited is the session UI flag — always False on page load.
                                        ' IsEditedDB carries the DB-persisted is_edited value for the Recalculate guard.
                                        drArrangementGridRow("IsEdited") = False
                                        drArrangementGridRow("IsEditedDB") = .IsEditedDB
                                        ' IsPremiumEdited: user directly edited this premium — preserved by RecalculateForTreatyPremium
                                        If dtArrangements.Columns.Contains("IsPremiumEdited") Then
                                            drArrangementGridRow("IsPremiumEdited") = .IsPremiumEdited
                                        End If
                                        drArrangementGridRow("Type") = .Type
                                        drArrangementGridRow("TreatyCode") = .TreatyCode
                                        drArrangementGridRow("RIArrangementLineKey") = .RIArrangementLineKey
                                        drArrangementGridRow("RIarrangementKey") = .RIarrangementKey
                                        drArrangementGridRow("Retained") = .Retained
                                        drArrangementGridRow("ReinsuranceTypeCode") = .ReinsuranceTypeCode
                                        drArrangementGridRow("Priority") = .Priority
                                        drArrangementGridRow("PremiumPercent") = Format(Math.Round(.PremiumPercent, 2), "0.00")
                                        drArrangementGridRow("PartyKey") = .PartyKey
                                        drArrangementGridRow("ParticipationPercent") = Format(Math.Round(.ParticipationPercent, 4), "0.00")
                                        drArrangementGridRow("NumberOfLines") = .NumberOfLines
                                        drArrangementGridRow("LineLimit") = Format(Math.Round(.LineLimit, 2), "0.00")
                                        drArrangementGridRow("LowerLimit") = Format(Math.Round(.LowerLimit, 2), "0.00")
                                        drArrangementGridRow("IsRIBroker") = .IsRIBroker
                                        drArrangementGridRow("IsDomiciledForTax") = .IsDomiciledForTax
                                        drArrangementGridRow("IsCommissionModified") = .IsCommissionModified
                                        drArrangementGridRow("Grouping") = .Grouping
                                        drArrangementGridRow("CedePremiumOnly") = .CedePremiumOnly
                                        drArrangementGridRow("ActionType") = .ActionType
                                        drArrangementGridRow("Placement") = .RIPlacement
                                        drArrangementGridRow("Name") = .RIName
                                        drArrangementGridRow("DefaultPerc") = Format(Math.Round(CType(.DefaultPerc, Decimal), 4), "0.0000")
                                        drArrangementGridRow("ThisPerc") = .ThisPerc 'Format(Math.Round(.ThisPerc, 4), "0.0000")
                                        drArrangementGridRow("SumInsured") = Format(Math.Round(.SumInsured, 2), "0.00")
                                        drArrangementGridRow("Premium") = Format(Math.Round(.PremiumValue, 2), "0.00")
                                        drArrangementGridRow("Tax") = Format(Math.Round(.PremiumTax, 2), "0.00")
                                        drArrangementGridRow("CommissionPerc") = Format(Math.Round(.CommissionPerc, 2), "0.00")
                                        drArrangementGridRow("Commission") = Format(Math.Round(.CommissionValue, 2), "0.00")
                                        drArrangementGridRow("CommissionTax") = Format(Math.Round(.CommissionTax, 2), "0.00")
                                        drArrangementGridRow("Agreement") = .AgreementCode

                                        dAllocatedThisPerc = dAllocatedThisPerc + .ThisPerc
                                        dAllocatedSumAssured = dAllocatedSumAssured + .SumInsured
                                        dAllocatedPremium = dAllocatedPremium + .PremiumValue
                                        dAllocatedTax = dAllocatedTax + .Tax
                                        dAllocatedCommision = dAllocatedCommision + .CommissionValue
                                        dAllocatedCommisionTax = dAllocatedCommisionTax + .CommissionTax

                                        ' FAX Participent (FAC XOL)
                                        If .FAXParticipants IsNot Nothing AndAlso .FAXParticipants.Count > 0 Then
                                            For Each oFAXPaticipent As FAXParticipants In .FAXParticipants
                                                Dim drFAXParticipent As DataRow
                                                With oFAXPaticipent
                                                    drFAXParticipent = dsParticipentGridData.Tables("Current_FAX_Participent").NewRow
                                                    drFAXParticipent("IsNew") = False
                                                    drFAXParticipent("IsDeleted") = False
                                                    drFAXParticipent("PartyKey") = .PartyKey
                                                    drFAXParticipent("ParticipationPercent") = Format(Math.Round(.ParticipationPercentage, 4), "0.0000")
                                                    drFAXParticipent("PartyCode") = .PartyCode
                                                    drFAXParticipent("PartyName") = .PartyName
                                                    drFAXParticipent("AccountType") = .AccountType
                                                    drFAXParticipent("AgreementCode") = .AgreementCode
                                                    drFAXParticipent("CommissionPercent") = Format(Math.Round(.CommissionPercent, 2), "0.00")
                                                    drFAXParticipent("CommissionTax") = Format(Math.Round(.CommissionTax, 2), "0.00")
                                                    drFAXParticipent("CommissionValue") = Format(Math.Round(.CommissionValue, 2), "0.00")
                                                    drFAXParticipent("PremiumTax") = Format(Math.Round(.PremiumTax, 2), "0.00")
                                                    drFAXParticipent("PremiumValue") = Format(Math.Round(.PremiumValue, 2), "0.00")
                                                    drFAXParticipent("SumInsured") = Format(Math.Round(.SumInsured, 2), "0.00")
                                                    drFAXParticipent("RIArrangementLineKey") = .RIArrangementLineKey
                                                    drFAXParticipent("RIarrangementKey") = oArrangementLinesType.RIarrangementKey
                                                    drFAXParticipent("Grouping") = oArrangementLinesType.Grouping

                                                    'FAX Broker Participent
                                                    If .BrokerParticipants IsNot Nothing AndAlso .BrokerParticipants.Count > 0 Then
                                                        For Each oPaticipent As BrokerParticipants In .BrokerParticipants
                                                            Dim drParticipent As DataRow
                                                            With oPaticipent
                                                                drParticipent = dsParticipentGridData.Tables("Current_FAX_Broker_Participent").NewRow
                                                                drParticipent("IsNew") = False
                                                                drParticipent("IsDeleted") = False
                                                                drParticipent("PartyCode") = .PartyCode
                                                                drParticipent("PartyName") = .PartyName
                                                                drParticipent("PartyKey") = .PartyKey
                                                                drParticipent("ParticipationPercent") = Format(Math.Round(.ParticipationPercentage, 4), "0.0000")
                                                                drParticipent("RIArrangementLineKey") = oFAXPaticipent.RIArrangementLineKey
                                                                drParticipent("RIarrangementKey") = oArrangementLinesType.RIarrangementKey
                                                            End With

                                                            dsParticipentGridData.Tables("Current_FAX_Broker_Participent").Rows.Add(drParticipent)
                                                        Next
                                                    End If
                                                End With
                                                dsParticipentGridData.Tables("Current_FAX_Participent").Rows.Add(drFAXParticipent)
                                            Next
                                        End If

                                        'Broker Participent (FAC PROP)
                                        If .BrokerParticipants IsNot Nothing AndAlso .BrokerParticipants.Count > 0 Then
                                            For Each oPaticipent As BrokerParticipants In .BrokerParticipants
                                                Dim drParticipent As DataRow
                                                With oPaticipent
                                                    drParticipent = dsParticipentGridData.Tables("Current_Broker_Participent").NewRow
                                                    drParticipent("IsNew") = False
                                                    drParticipent("IsDeleted") = False
                                                    drParticipent("PartyCode") = .PartyCode
                                                    drParticipent("PartyName") = .PartyName
                                                    drParticipent("PartyKey") = .PartyKey
                                                    drParticipent("ParticipationPercent") = Math.Round(.ParticipationPercentage, 4)
                                                    drParticipent("RIArrangementLineKey") = oArrangementLinesType.RIArrangementLineKey
                                                    drParticipent("RIarrangementKey") = oArrangementLinesType.RIarrangementKey
                                                End With

                                                dsParticipentGridData.Tables("Current_Broker_Participent").Rows.Add(drParticipent)
                                            Next
                                        End If
                                    End With

                                    ' adding rows to the table
                                    dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).Rows.Add(drArrangementGridRow)
                                    drArrangementGridRow = Nothing
                                Next
                            Else
                                'RI 2007 is OFF

                                oReinsuranceArrangmentCollection = GetRiskReinsuranceArrangementLines(iArrangementId)

                                drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).NewRow

                                drArrangementGridRow("Name") = "Band Total"
                                drArrangementGridRow("SumInsured") = New Money(.SumInsured, Current.Session(CNCurrenyCode)).Formatted
                                drArrangementGridRow("Premium") = New Money(.Premium, Current.Session(CNCurrenyCode)).Formatted
                                dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).Rows.Add(drArrangementGridRow)
                                drArrangementGridRow = Nothing

                                ' iterating through and adding the ArrangementLine values into the dataset
                                For Each oArrangementLinesType As NexusProvider.ArrangementLinesType In oReinsuranceArrangmentCollection

                                    With oArrangementLinesType
                                        drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).NewRow
                                        drArrangementGridRow("Name") = .Name
                                        drArrangementGridRow("DefaultPerc") = CType(.DefaultPerc, Decimal)
                                        drArrangementGridRow("ThisPerc") = .ThisPerc
                                        drArrangementGridRow("SumInsured") = New Money(.SumInsured, Current.Session(CNCurrenyCode)).Formatted
                                        drArrangementGridRow("Premium") = New Money(.PremiumValue, Current.Session(CNCurrenyCode)).Formatted
                                        drArrangementGridRow("Tax") = New Money(.Tax, Current.Session(CNCurrenyCode)).Formatted
                                        drArrangementGridRow("CommissionPerc") = .CommissionPerc
                                        drArrangementGridRow("Commission") = New Money(.CommissionValue, Current.Session(CNCurrenyCode)).Formatted
                                        drArrangementGridRow("CommissionTax") = New Money(.CommissionTax, Current.Session(CNCurrenyCode)).Formatted
                                        drArrangementGridRow("Agreement") = .AgreementCode

                                        dAllocatedThisPerc = dAllocatedThisPerc + .ThisPerc
                                        dAllocatedSumAssured = dAllocatedSumAssured + .SumInsured
                                        dAllocatedPremium = dAllocatedPremium + .PremiumValue
                                        dAllocatedTax = dAllocatedTax + .Tax
                                        dAllocatedCommision = dAllocatedCommision + .CommissionValue
                                        dAllocatedCommisionTax = dAllocatedCommisionTax + .CommissionTax
                                    End With

                                    ' adding rows to the table
                                    dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).Rows.Add(drArrangementGridRow)
                                    drArrangementGridRow = Nothing
                                Next
                            End If
                        End If
                    End With
                End If
            Next

            Dim oResource As ResXResourceReader
            Dim en As IDictionaryEnumerator
            If bIsRI2007 Then
                'RI 2007 is ON
                oResource = New ResXResourceReader(HttpContext.Current.Server.MapPath(AppSettings("WebRoot") & "Controls/App_LocalResources/Reinsurance2007.ascx.resx"))
            Else
                'RI 2007 is OFF
                oResource = New ResXResourceReader(HttpContext.Current.Server.MapPath(AppSettings("WebRoot") & "Controls/App_LocalResources/Reinsurance.ascx.resx"))
            End If

            en = oResource.GetEnumerator()
            ' assigning the dataset to the grid
            If dsArrangementGridData IsNot Nothing AndAlso dsArrangementGridData.Tables.Count > 0 AndAlso dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandKey.ToString).Rows.Count > 0 Then

                If bIsRI2007 Then
                    'RI 2007 is ON
                    'Allocated
                    drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandKey.ToString).NewRow
                    While (en.MoveNext)
                        If en.Key.ToString.Trim = "lbl_Allocated" Then
                            drArrangementGridRow("Name") = en.Value
                            Exit While
                        End If
                    End While
                    drArrangementGridRow("IsDeleted") = False
                    drArrangementGridRow("SumInsured") = Format(Math.Round(dAllocatedSumAssured, 2), "0.00")
                    drArrangementGridRow("Premium") = Format(Math.Round(dAllocatedPremium, 2), "0.00")
                    dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandKey.ToString).Rows.Add(drArrangementGridRow)
                    drArrangementGridRow = Nothing
                Else
                    'RI 2007 is OFF
                    'Allocated
                    drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandKey.ToString).NewRow
                    While (en.MoveNext)
                        If en.Key.ToString.Trim = "lbl_Allocated" Then
                            drArrangementGridRow("Name") = en.Value
                            Exit While
                        End If
                    End While

                    drArrangementGridRow("ThisPerc") = dAllocatedThisPerc
                    drArrangementGridRow("SumInsured") = New Money(dAllocatedSumAssured, Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("Premium") = New Money(dAllocatedPremium, Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("Tax") = New Money(dAllocatedTax, Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("Commission") = New Money(dAllocatedCommision, Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("CommissionTax") = New Money(dAllocatedCommisionTax, Current.Session(CNCurrenyCode)).Formatted
                    dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandKey.ToString).Rows.Add(drArrangementGridRow)
                    drArrangementGridRow = Nothing
                End If

                If dsArrangementGridData IsNot Nothing AndAlso dsArrangementGridData.Tables("Original_" & oReinsuranceBands.BandKey.ToString).Rows.Count > 0 Then

                    If bIsRI2007 Then
                        'RI 2007 is ON
                        'Oroginal RI Totals
                        drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandKey.ToString).NewRow

                        en.Reset()
                        While (en.MoveNext)
                            If en.Key.ToString.Trim = "lbl_OriginalRITotals" Then
                                drArrangementGridRow("Name") = en.Value
                                Exit While
                            End If
                        End While
                        drArrangementGridRow("IsDeleted") = "False"
                        drArrangementGridRow("SumInsured") = dOriginalSumAssuredTotal
                        drArrangementGridRow("Premium") = dOriginalPremiumTotal

                        dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandKey.ToString).Rows.Add(drArrangementGridRow)
                        drArrangementGridRow = Nothing

                        'Net
                        drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandKey.ToString).NewRow

                        en.Reset()
                        While (en.MoveNext)
                            If en.Key.ToString.Trim = "lbl_NetTotals" Then
                                drArrangementGridRow("Name") = en.Value
                                Exit While
                            End If
                        End While
                        drArrangementGridRow("IsDeleted") = "False"
                        drArrangementGridRow("SumInsured") = New Money((dAllocatedSumAssured + dSumAssured), Current.Session(CNCurrenyCode)).Formatted
                        drArrangementGridRow("Premium") = New Money((dAllocatedPremium + dPremium), Current.Session(CNCurrenyCode)).Formatted

                        dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandKey.ToString).Rows.Add(drArrangementGridRow)
                        drArrangementGridRow = Nothing
                    Else
                        'RI 2007 is OFF
                        'Oroginal RI Totals
                        drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandKey.ToString).NewRow

                        en.Reset()
                        While (en.MoveNext)
                            If en.Key.ToString.Trim = "lbl_OriginalRITotals" Then
                                drArrangementGridRow("Name") = en.Value
                                Exit While
                            End If
                        End While

                        drArrangementGridRow("SumInsured") = New Money(dSumAssured, Current.Session(CNCurrenyCode)).Formatted
                        drArrangementGridRow("Premium") = New Money(dPremium, Current.Session(CNCurrenyCode)).Formatted

                        dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandKey.ToString).Rows.Add(drArrangementGridRow)
                        drArrangementGridRow = Nothing

                        'Net
                        drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandKey.ToString).NewRow

                        en.Reset()
                        While (en.MoveNext)
                            If en.Key.ToString.Trim = "lbl_NetTotals" Then
                                drArrangementGridRow("Name") = en.Value
                                Exit While
                            End If
                        End While

                        drArrangementGridRow("SumInsured") = New Money((dAllocatedSumAssured + dSumAssured), Current.Session(CNCurrenyCode)).Formatted
                        drArrangementGridRow("Premium") = New Money((dAllocatedPremium + dPremium), Current.Session(CNCurrenyCode)).Formatted

                        dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandKey.ToString).Rows.Add(drArrangementGridRow)
                        drArrangementGridRow = Nothing
                    End If
                End If
            End If
        Next

        'Need to convert it into XML
        If bIsRI2007 Then
            Dim oXMLDoc As New XmlDocument

            ' Write down the XML declaration
            Dim xmlDeclaration As XmlDeclaration = oXMLDoc.CreateXmlDeclaration("1.0", "utf-8", Nothing)

            ' Create the root element
            Dim rootNode As XmlElement = oXMLDoc.CreateElement("rows")
            oXMLDoc.InsertBefore(xmlDeclaration, oXMLDoc.DocumentElement)
            oXMLDoc.AppendChild(rootNode)

            For Each oRITable As DataTable In dsArrangementGridData.Tables

                ' Create the root element
                Dim sElement As String = "RIBAND"
                Dim RIBand As XmlElement = oXMLDoc.CreateElement(sElement)
                RIBand.SetAttribute("Name", oRITable.TableName)
                ' PBI 35359: Stamp OverrideReasonId from arrangement ExtendedProperties onto RIBAND node
                If oRITable.ExtendedProperties.ContainsKey("OverrideReasonId") Then
                    RIBand.SetAttribute("OverrideReasonId", Convert.ToString(oRITable.ExtendedProperties("OverrideReasonId")))
                Else
                    RIBand.SetAttribute("OverrideReasonId", "0")
                End If
                rootNode.AppendChild(RIBand)

                'First Other Lines Should be addes
                RIArrangementRow_Others(oRITable, oXMLDoc, RIBand, dsParticipentGridData)

                'Second FAX XOL should be added
                RIArrangementRow_FAC(oRITable, oXMLDoc, RIBand, dsParticipentGridData, "FAC XOL")

                'Third FAC Prop should be added
                RIArrangementRow_FAC(oRITable, oXMLDoc, RIBand, dsParticipentGridData, "FAC Prop")

                'Calculate Net FAC
                CalculateFACNet(oRITable, oXMLDoc, RIBand)

                If oRITable.TableName.Substring(0, 7) = "Current" Then
                    'Calculate UnAllocated
                    CalculateUnAllocated(oRITable, oXMLDoc, RIBand)
                End If
            Next

            sReturnXML = oXMLDoc.OuterXml

        End If

        Return dsArrangementGridData
    End Function
    ''' <summary>
    ''' This Method Return the XML for NB/MTA/MTR/MTC/REN Reinsurance, If RI2007 is ON
    ''' <param name="iClaimKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' </summary>
    ''' <param name="iRiskKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetReinsurance2007(ByVal iRiskKey As Integer,
                                                    Optional ByVal nVersionId As Integer = 0,
                                                    Optional ByVal oReinsuranceBandsCollection As NexusProvider.ReinsuranceBandsCollection = Nothing) As String
        'Need to convert it into XML
        Dim sReturnXML As String = Nothing
        Dim dsArrangementGridData As DataSet
        dsArrangementGridData = GetReinsurance(iRiskKey, True, sReturnXML, oReinsuranceBandsCollection)

        Return sReturnXML
    End Function
    ''' <summary>
    '''  This Method Return the DataSet for Claim Reinsurance, If RI2007 is OFF
    ''' </summary>
    ''' <param name="iClaimKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' </summary>
    ''' <param name="iClaimKey"></param>
    ''' <param name="bIsRI2007"></param>
    ''' <param name="sReturnXML"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetClaimReinsurance(ByVal iClaimKey As Integer, _
                                                  Optional ByVal bIsRI2007 As Boolean = False, _
                                                  Optional ByRef sReturnXML As String = Nothing) As DataSet
        Dim oReinsurarerBandCollection As NexusProvider.ReinsuranceBandsCollection
        Dim dsArrangementGridData As New DataSet
        Dim dsParticipentGridData As New DataSet
        Dim dtCurParticipent As New DataTable("Current_Broker_Participent")
        Dim dtCurFAXBrokerParticipent As New DataTable("Current_FAX_Broker_Participent")
        Dim dtCurFAXParticipent As New DataTable("Current_FAX_Participent")


        If bIsRI2007 Then
            'RI 2007 is ON
            'FAX Participent Table
            dtCurFAXParticipent.Columns.Add("IsNew", GetType(Boolean))
            dtCurFAXParticipent.Columns.Add("IsEdited", GetType(Boolean))
            dtCurFAXParticipent.Columns.Add("IsDeleted", GetType(Boolean))
            dtCurFAXParticipent.Columns.Add("PartyKey", GetType(Integer))
            dtCurFAXParticipent.Columns.Add("ParticipationPercent", GetType(Double))
            dtCurFAXParticipent.Columns.Add("PartyCode", GetType(String))
            dtCurFAXParticipent.Columns.Add("PartyName", GetType(String))
            dtCurFAXParticipent.Columns.Add("AccountType", GetType(String))
            dtCurFAXParticipent.Columns.Add("AgreementCode", GetType(String))
            dtCurFAXParticipent.Columns.Add("SumInsured", GetType(Double))
            dtCurFAXParticipent.Columns.Add("PaymentToDate", GetType(Double))
            dtCurFAXParticipent.Columns.Add("RecoverToDate", GetType(Double))
            dtCurFAXParticipent.Columns.Add("ReserveToDate", GetType(Double))
            dtCurFAXParticipent.Columns.Add("ThisPayment", GetType(Double))
            dtCurFAXParticipent.Columns.Add("ThisReserve", GetType(Double))
            dtCurFAXParticipent.Columns.Add("ActionType", GetType(RowAction))
            dtCurFAXParticipent.Columns.Add("RIArrangementLineKey", GetType(Integer))
            dtCurFAXParticipent.Columns.Add("RIarrangementKey", GetType(Integer))
            dtCurFAXParticipent.Columns.Add("Grouping", GetType(Integer))
            dsParticipentGridData.Tables.Add(dtCurFAXParticipent)

            'FAX Broker Participent Table
            dtCurFAXBrokerParticipent.Columns.Add("IsNew", GetType(Boolean))
            dtCurFAXBrokerParticipent.Columns.Add("IsEdited", GetType(Boolean))
            dtCurFAXBrokerParticipent.Columns.Add("IsDeleted", GetType(Boolean))
            dtCurFAXBrokerParticipent.Columns.Add("PartyKey", GetType(Integer))
            dtCurFAXBrokerParticipent.Columns.Add("ParticipationPercent", GetType(Double))
            dtCurFAXBrokerParticipent.Columns.Add("RIArrangementLineKey", GetType(Integer))
            dtCurFAXBrokerParticipent.Columns.Add("RIarrangementKey", GetType(Integer))
            dtCurFAXBrokerParticipent.Columns.Add("PartyCode", GetType(String))
            dtCurFAXBrokerParticipent.Columns.Add("PartyName", GetType(String))
            dsParticipentGridData.Tables.Add(dtCurFAXBrokerParticipent)

            'Broker Paricipent Table
            dtCurParticipent.Columns.Add("IsNew", GetType(Boolean))
            dtCurParticipent.Columns.Add("IsEdited", GetType(Boolean))
            dtCurParticipent.Columns.Add("IsDeleted", GetType(Boolean))
            dtCurParticipent.Columns.Add("PartyKey", GetType(Integer))
            dtCurParticipent.Columns.Add("ParticipationPercent", GetType(Double))
            dtCurParticipent.Columns.Add("RIArrangementLineKey", GetType(Integer))
            dtCurParticipent.Columns.Add("RIarrangementKey", GetType(Integer))
            dtCurParticipent.Columns.Add("PartyCode", GetType(String))
            dtCurParticipent.Columns.Add("PartyName", GetType(String))
            dsParticipentGridData.Tables.Add(dtCurParticipent)

        End If


        oReinsurarerBandCollection = GetClaimReinsurancebands(iClaimKey)

        For Each oReinsuranceBands As NexusProvider.ReinsuranceBands In oReinsurarerBandCollection
            Dim drArrangementGridRow As DataRow
            Dim dtArrangements As New DataTable("Current_" & oReinsuranceBands.BandID.ToString)
            Dim iArrangementId As Integer
            ' Obtaining the value of ArrangementsType for specific riskkey from SAM
            Dim oReinsuranceArrangementLineCollection As NexusProvider.ReinsuranceArrangementLineCollection = Nothing

            If Current.Session(CNMode) = Mode.ViewClaim Then
                oReinsuranceArrangementLineCollection = GetClaimReinsuranceArrangements(iClaimKey, 0)
            Else
                '-1 is passed since user has not specified any mode
                oReinsuranceArrangementLineCollection = GetClaimReinsuranceArrangements(iClaimKey, -1)
            End If

            Dim dAllocatedReserveToDate, dAllocatedSumInsured, dAllocatedThisReserve, dAllocatedThisPayment, dAllocatedPaymentToDate, dAllocatedBalance As Double
            Dim dAllocatedRecoverToDate As Double

            dAllocatedReserveToDate = 0
            dAllocatedSumInsured = 0
            dAllocatedThisReserve = 0
            dAllocatedPaymentToDate = 0
            dAllocatedBalance = 0
            dAllocatedRecoverToDate = 0
            dAllocatedThisPayment = 0

            If bIsRI2007 Then
                'RI 2007 is ON
                ' declaring table for adding into the dataset
                dtArrangements.Columns.Add("IsNew", GetType(Boolean))
                dtArrangements.Columns.Add("IsEdited", GetType(Boolean))
                dtArrangements.Columns.Add("IsEditedDB", GetType(Boolean))
                dtArrangements.Columns.Add("IsPremiumEdited", GetType(Boolean))
                dtArrangements.Columns.Add("IsDeleted", GetType(Boolean))
                dtArrangements.Columns.Add("ActionType", GetType(RowAction))
                dtArrangements.Columns.Add("AgreementCode", GetType(String))
                dtArrangements.Columns.Add("CedePremiumOnly", GetType(Boolean))
                dtArrangements.Columns.Add("DefaultSharePercent", GetType(Double))
                dtArrangements.Columns.Add("Grouping", GetType(Integer))
                dtArrangements.Columns.Add("IsDomiciledForTax", GetType(Boolean))
                dtArrangements.Columns.Add("IsRIBroker", GetType(Boolean))
                dtArrangements.Columns.Add("LineLimit", GetType(Double))
                dtArrangements.Columns.Add("LowerLimit", GetType(Double))
                dtArrangements.Columns.Add("NumberOfLines", GetType(Decimal))
                dtArrangements.Columns.Add("PartyKey", GetType(Integer))
                dtArrangements.Columns.Add("Priority", GetType(Integer))
                dtArrangements.Columns.Add("ReinsuranceTypeCode", GetType(String))
                dtArrangements.Columns.Add("Retained", GetType(Double))
                dtArrangements.Columns.Add("RIArrangementKey", GetType(Integer))
                dtArrangements.Columns.Add("RIArrangementLineKey", GetType(Integer))
                dtArrangements.Columns.Add("Placement", GetType(String))
                dtArrangements.Columns.Add("TreatyCode", GetType(String))
                dtArrangements.Columns.Add("Type", GetType(String))
                dtArrangements.Columns.Add("RecoverToDate", GetType(Double))
                dtArrangements.Columns.Add("ThisPayment", GetType(Double))
                dtArrangements.Columns.Add("Name", GetType(String))
                dtArrangements.Columns.Add("DefaultPerc", GetType(Double))
                dtArrangements.Columns.Add("ThisPerc", GetType(Double))
                dtArrangements.Columns.Add("SumInsured", GetType(Double))
                dtArrangements.Columns.Add("ReserveToDate", GetType(Double))
                dtArrangements.Columns.Add("PaymentToDate", GetType(Double))
                dtArrangements.Columns.Add("ThisReserve", GetType(Double))
                dtArrangements.Columns.Add("Balance", GetType(Double))
                dtArrangements.Columns.Add("Agreement", GetType(String))
                dtArrangements.Columns.Add("Incurred", GetType(Double))
                dsArrangementGridData.Tables.Add(dtArrangements)

            Else
                'RI 2007 is OFF
                ' declaring table for adding into the dataset
                dtArrangements.Columns.Add("Placement", GetType(String))
                dtArrangements.Columns.Add("Name", GetType(String))
                dtArrangements.Columns.Add("DefaultPerc", GetType(String))
                dtArrangements.Columns.Add("ThisPerc", GetType(String))
                dtArrangements.Columns.Add("SumInsured", GetType(String))
                dtArrangements.Columns.Add("ReserveToDate", GetType(String))
                dtArrangements.Columns.Add("PaymentToDate", GetType(String))
                dtArrangements.Columns.Add("RecoverToDate", GetType(String))
                dtArrangements.Columns.Add("ThisReserve", GetType(String))
                dtArrangements.Columns.Add("ThisPayment", GetType(String))
                dtArrangements.Columns.Add("Balance", GetType(String))
                dtArrangements.Columns.Add("Agreement", GetType(String))
                dsArrangementGridData.Tables.Add(dtArrangements)

            End If
            ' Obtaining the value of ArrangementLinesType for specific riskkey from SAM
            For Each oArrangementType As NexusProvider.ReinsuranceArrangementLines In oReinsuranceArrangementLineCollection

                If oArrangementType.BandId = oReinsuranceBands.BandID Then
                    iArrangementId = oArrangementType.ArrangementId
                    Dim oArrangementLinesTypeCollection As ArrangementLinesTypeCollection
                    If bIsRI2007 Then
                        'RI 2007 is ON
                        If Current.Session(CNMode) = Mode.ViewClaim Then
                            Dim oClaim As NexusProvider.Claim = Current.Session(CNClaim)
                            If oClaim.IsRecovery = True Then
                                oArrangementLinesTypeCollection = GetClaimRIArrangementLinesRI2007(iClaimKey, iArrangementId, 0, True)
                            Else
                                oArrangementLinesTypeCollection = GetClaimRIArrangementLinesRI2007(iClaimKey, iArrangementId, 0, False)
                            End If
                        ElseIf Current.Session(CNMode) = Mode.TPRecovery Or Current.Session(CNMode) = Mode.SalvageClaim Then
                            '-1 is passed since user has not specified any mode
                            oArrangementLinesTypeCollection = GetClaimRIArrangementLinesRI2007(iClaimKey, iArrangementId, -1, True)
                        Else
                            '-1 is passed since user has not specified any mode
                            oArrangementLinesTypeCollection = GetClaimRIArrangementLinesRI2007(iClaimKey, iArrangementId, -1, False)
                        End If


                        drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).NewRow
                        drArrangementGridRow("Placement") = "GROSS"
                        drArrangementGridRow("Name") = "Band Total"
                        drArrangementGridRow("SumInsured") = oArrangementType.SumInsured
                        drArrangementGridRow("Balance") = oArrangementType.Balance
                        drArrangementGridRow("PaymentToDate") = oArrangementType.PaymentToDate
                        drArrangementGridRow("ReserveToDate") = oArrangementType.ReserveToDate
                        drArrangementGridRow("RecoverToDate") = oArrangementType.RecoveryToDate
                        drArrangementGridRow("ThisPayment") = oArrangementType.ThisPayment
                        drArrangementGridRow("ThisReserve") = oArrangementType.ThisReserve

                        dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).Rows.Add(drArrangementGridRow)
                        drArrangementGridRow = Nothing

                    Else
                        'RI 2007 is OFF
                        oArrangementLinesTypeCollection = GetClaimReinsuranceArrangementLines(iClaimKey, iArrangementId, 0)

                        drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).NewRow
                        drArrangementGridRow("Placement") = "GROSS"
                        drArrangementGridRow("Name") = "Band Total"
                        drArrangementGridRow("SumInsured") = oArrangementType.SumInsured
                        drArrangementGridRow("Balance") = oArrangementType.Balance
                        drArrangementGridRow("PaymentToDate") = oArrangementType.PaymentToDate
                        drArrangementGridRow("ReserveToDate") = oArrangementType.ReserveToDate
                        drArrangementGridRow("RecoverToDate") = oArrangementType.RecoveryToDate
                        drArrangementGridRow("ThisPayment") = oArrangementType.ThisPayment
                        drArrangementGridRow("ThisReserve") = oArrangementType.ThisReserve
                        dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).Rows.Add(drArrangementGridRow)
                        drArrangementGridRow = Nothing

                    End If

                    For Each oArrangementLinesType As NexusProvider.ArrangementLinesType In oArrangementLinesTypeCollection
                        If bIsRI2007 Then
                            'RI 2007 is ON

                            With oArrangementLinesType
                                drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandID.ToString).NewRow
                                drArrangementGridRow("IsNew") = False
                                drArrangementGridRow("IsEdited") = False
                                drArrangementGridRow("IsEditedDB") = .IsEditedDB
                                 If dtArrangements.Columns.Contains("IsPremiumEdited") Then drArrangementGridRow("IsPremiumEdited") = .IsPremiumEdited
                                drArrangementGridRow("IsDeleted") = False
                                drArrangementGridRow("ActionType") = .ActionType
                                drArrangementGridRow("AgreementCode") = .AgreementCode
                                drArrangementGridRow("CedePremiumOnly") = .CedePremiumOnly
                                drArrangementGridRow("Grouping") = .Grouping
                                drArrangementGridRow("IsDomiciledForTax") = .IsDomiciledForTax
                                drArrangementGridRow("IsRIBroker") = .IsRIBroker
                                drArrangementGridRow("LineLimit") = .LineLimit
                                drArrangementGridRow("LowerLimit") = .LowerLimit
                                drArrangementGridRow("NumberOfLines") = .NumberOfLines
                                drArrangementGridRow("PartyKey") = .PartyKey
                                drArrangementGridRow("Priority") = .Priority
                                drArrangementGridRow("ReinsuranceTypeCode") = .ReinsuranceTypeCode
                                drArrangementGridRow("Retained") = .Retained
                                drArrangementGridRow("RIArrangementKey") = .RIarrangementKey
                                drArrangementGridRow("RIArrangementLineKey") = .RIArrangementLineKey
                                drArrangementGridRow("Placement") = .RIPlacement
                                drArrangementGridRow("TreatyCode") = .TreatyCode
                                drArrangementGridRow("Type") = .Type
                                drArrangementGridRow("RecoverToDate") = .RecoverToDate
                                drArrangementGridRow("ThisPayment") = .ThisPayment
                                drArrangementGridRow("Name") = .RIName
                                drArrangementGridRow("DefaultPerc") = .DefaultPerc
                                drArrangementGridRow("ThisPerc") = .ThisPerc
                                drArrangementGridRow("SumInsured") = .SumInsured
                                drArrangementGridRow("ReserveToDate") = .ReserveToDate
                                drArrangementGridRow("PaymentToDate") = .PaymentToDate
                                drArrangementGridRow("ThisReserve") = .ThisReserve
                                drArrangementGridRow("Balance") = .Balance
                                drArrangementGridRow("Agreement") = .AgreementCode
                                drArrangementGridRow("Incurred") = .Incurred

                                dAllocatedSumInsured = dAllocatedSumInsured + .SumInsured
                                dAllocatedReserveToDate = dAllocatedReserveToDate + .ReserveToDate
                                dAllocatedThisReserve = dAllocatedThisReserve + .ThisReserve
                                dAllocatedPaymentToDate = dAllocatedPaymentToDate + .PaymentToDate
                                dAllocatedRecoverToDate = dAllocatedRecoverToDate + .RecoverToDate
                                dAllocatedBalance = dAllocatedBalance + .Balance
                                dAllocatedThisPayment = dAllocatedThisPayment + .ThisPayment

                                ' FAX Participent (FAC XOL)
                                If .FAXParticipants IsNot Nothing AndAlso .FAXParticipants.Count > 0 Then
                                    For Each oFAXPaticipent As FAXParticipants In .FAXParticipants
                                        Dim drFAXParticipent As DataRow
                                        With oFAXPaticipent
                                            drFAXParticipent = dsParticipentGridData.Tables("Current_FAX_Participent").NewRow
                                            drFAXParticipent("PartyKey") = .PartyKey
                                            drFAXParticipent("ParticipationPercent") = .ParticipationPercentage
                                            drFAXParticipent("PartyCode") = .PartyCode
                                            drFAXParticipent("PartyName") = .PartyName
                                            drFAXParticipent("AccountType") = .AccountType
                                            drFAXParticipent("AgreementCode") = .AgreementCode
                                            drFAXParticipent("SumInsured") = .SumInsured
                                            drFAXParticipent("PaymentToDate") = .PaymentToDate
                                            drFAXParticipent("RecoverToDate") = .RecoverToDate
                                            drFAXParticipent("ReserveToDate") = .ReserveToDate
                                            drFAXParticipent("ThisPayment") = .ThisPayment
                                            drFAXParticipent("ThisReserve") = .ThisReserve
                                            drFAXParticipent("ActionType") = .ActionType
                                            drFAXParticipent("RIArrangementLineKey") = .RIArrangementLineKey
                                            drFAXParticipent("RIarrangementKey") = oArrangementLinesType.RIarrangementKey
                                            drFAXParticipent("Grouping") = oArrangementLinesType.Grouping

                                            'FAX Broker Participent
                                            If .BrokerParticipants IsNot Nothing AndAlso .BrokerParticipants.Count > 0 Then
                                                For Each oPaticipent As BrokerParticipants In .BrokerParticipants
                                                    Dim drParticipent As DataRow
                                                    With oPaticipent
                                                        drParticipent = dsParticipentGridData.Tables("Current_FAX_Broker_Participent").NewRow
                                                        drParticipent("PartyCode") = .PartyCode
                                                        drParticipent("PartyName") = .PartyName
                                                        drParticipent("PartyKey") = .PartyKey
                                                        drParticipent("ParticipationPercent") = .ParticipationPercentage
                                                        drParticipent("RIArrangementLineKey") = oFAXPaticipent.RIArrangementLineKey
                                                        drParticipent("RIarrangementKey") = oArrangementLinesType.RIarrangementKey
                                                    End With

                                                    dsParticipentGridData.Tables("Current_FAX_Broker_Participent").Rows.Add(drParticipent)
                                                Next
                                            End If
                                        End With
                                        dsParticipentGridData.Tables("Current_FAX_Participent").Rows.Add(drFAXParticipent)
                                    Next
                                End If

                                'Broker Participent (FAC PROP)
                                If .BrokerParticipants IsNot Nothing AndAlso .BrokerParticipants.Count > 0 Then
                                    For Each oPaticipent As BrokerParticipants In .BrokerParticipants
                                        Dim drParticipent As DataRow
                                        With oPaticipent
                                            drParticipent = dsParticipentGridData.Tables("Current_Broker_Participent").NewRow
                                            drParticipent("IsNew") = False
                                            drParticipent("PartyCode") = .PartyCode
                                            drParticipent("PartyName") = .PartyName
                                            drParticipent("PartyKey") = .PartyKey
                                            drParticipent("ParticipationPercent") = .ParticipationPercentage
                                            drParticipent("RIArrangementLineKey") = oArrangementLinesType.RIArrangementLineKey
                                            drParticipent("RIarrangementKey") = oArrangementLinesType.RIarrangementKey
                                        End With

                                        dsParticipentGridData.Tables("Current_Broker_Participent").Rows.Add(drParticipent)
                                    Next
                                End If
                            End With
                        Else
                            'RI 2007 is OFF

                            With oArrangementLinesType
                                drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandID.ToString).NewRow
                                drArrangementGridRow("Name") = .Name
                                drArrangementGridRow("DefaultPerc") = New Money(CType(.DefaultPerc, Decimal), Current.Session(CNCurrenyCode)).Formatted
                                drArrangementGridRow("ThisPerc") = New Money(.ThisPerc, Current.Session(CNCurrenyCode)).Formatted
                                drArrangementGridRow("SumInsured") = New Money(.SumInsured, Current.Session(CNCurrenyCode)).Formatted
                                drArrangementGridRow("ReserveToDate") = New Money(.ReserveToDate, Current.Session(CNCurrenyCode)).Formatted
                                drArrangementGridRow("RecoverToDate") = New Money(.RecoverToDate, Current.Session(CNCurrenyCode)).Formatted
                                drArrangementGridRow("PaymentToDate") = New Money(.PaymentToDate, Current.Session(CNCurrenyCode)).Formatted
                                drArrangementGridRow("ThisPayment") = New Money(.ThisPayment, Current.Session(CNCurrenyCode)).Formatted
                                drArrangementGridRow("ThisReserve") = New Money(.ThisReserve, Current.Session(CNCurrenyCode)).Formatted
                                drArrangementGridRow("Balance") = New Money(.Balance, Current.Session(CNCurrenyCode)).Formatted
                                drArrangementGridRow("Agreement") = .AgreementCode

                                dAllocatedSumInsured = dAllocatedSumInsured + CDbl(.SumInsured)
                                dAllocatedReserveToDate = dAllocatedReserveToDate + CDbl(.ReserveToDate)
                                dAllocatedThisReserve = dAllocatedThisReserve + CDbl(.ThisReserve)
                                dAllocatedPaymentToDate = dAllocatedPaymentToDate + CDbl(.PaymentToDate)
                                dAllocatedBalance = dAllocatedBalance + CDbl(.Balance)

                            End With

                        End If
                        ' adding rows to the table
                        dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandID.ToString).Rows.Add(drArrangementGridRow)
                    Next
                End If
            Next

            Dim oResource As ResXResourceReader
            Dim en As IDictionaryEnumerator
            If bIsRI2007 Then
                'RI 2007 is ON
                oResource = New ResXResourceReader(HttpContext.Current.Server.MapPath(AppSettings("WebRoot") & "Controls/App_LocalResources/ClaimReinsurance2007.ascx.resx"))
                en = oResource.GetEnumerator()

                'Allocated
                drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandID.ToString).NewRow
                While (en.MoveNext)
                    If en.Key.ToString.Trim = "lbl_Allocated" Then
                        drArrangementGridRow("Name") = en.Value
                        Exit While
                    End If
                End While

                drArrangementGridRow("ReserveToDate") = dAllocatedReserveToDate
                drArrangementGridRow("SumInsured") = dAllocatedSumInsured
                drArrangementGridRow("ThisReserve") = dAllocatedThisReserve
                drArrangementGridRow("ThisPayment") = dAllocatedThisPayment
                drArrangementGridRow("PaymentToDate") = dAllocatedPaymentToDate
                drArrangementGridRow("Balance") = dAllocatedBalance
                drArrangementGridRow("RecoverToDate") = dAllocatedRecoverToDate
                dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandID.ToString).Rows.Add(drArrangementGridRow)
                drArrangementGridRow = Nothing
            Else
                'RI 2007 is OFF
                oResource = New ResXResourceReader(HttpContext.Current.Server.MapPath(AppSettings("WebRoot") & "Controls/App_LocalResources/ClaimReinsurance.ascx.resx"))
                en = oResource.GetEnumerator()
                'Allocated
                drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandID.ToString).NewRow
                While (en.MoveNext)
                    If en.Key.ToString.Trim = "lbl_Allocated" Then
                        drArrangementGridRow("Name") = en.Value
                        Exit While
                    End If
                End While

                drArrangementGridRow("ReserveToDate") = dAllocatedReserveToDate
                drArrangementGridRow("SumInsured") = dAllocatedSumInsured
                drArrangementGridRow("ThisReserve") = dAllocatedThisReserve
                drArrangementGridRow("PaymentToDate") = dAllocatedPaymentToDate
                drArrangementGridRow("Balance") = dAllocatedBalance
                drArrangementGridRow("RecoverToDate") = dAllocatedRecoverToDate
                dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandID.ToString).Rows.Add(drArrangementGridRow)
                drArrangementGridRow = Nothing
            End If
        Next

        'Need to convert it into XML
        If bIsRI2007 Then
            Dim oXMLDoc As New XmlDocument

            ' Write down the XML declaration
            Dim xmlDeclaration As XmlDeclaration = oXMLDoc.CreateXmlDeclaration("1.0", "utf-8", Nothing)

            ' Create the root element
            Dim rootNode As XmlElement = oXMLDoc.CreateElement("rows")
            oXMLDoc.InsertBefore(xmlDeclaration, oXMLDoc.DocumentElement)
            oXMLDoc.AppendChild(rootNode)

            For Each oRITable As DataTable In dsArrangementGridData.Tables

                ' Create the root element
                Dim sElement As String = "RIBAND"
                Dim RIBand As XmlElement = oXMLDoc.CreateElement(sElement)
                RIBand.SetAttribute("Name", oRITable.TableName)
                ' Stamp OverrideReasonId from arrangement ExtendedProperties onto RIBAND node (GAP 2 fix)
                If oRITable.ExtendedProperties.ContainsKey("OverrideReasonId") Then
                    RIBand.SetAttribute("OverrideReasonId", Convert.ToString(oRITable.ExtendedProperties("OverrideReasonId")))
                Else
                    RIBand.SetAttribute("OverrideReasonId", "0")
                End If
                rootNode.AppendChild(RIBand)

                'First Other Lines Should be addes
                ClaimRIArrangementRow_Others(oRITable, oXMLDoc, RIBand, dsParticipentGridData)

                'Second FAX XOL should be added
                RIArrangementRow_FAC(oRITable, oXMLDoc, RIBand, dsParticipentGridData, "FAC XOL")

                'Third FAC Prop should be added
                RIArrangementRow_FAC(oRITable, oXMLDoc, RIBand, dsParticipentGridData, "FAC Prop")

                'Calculate Net FAC
                CalculateClaimFACNet(oRITable, oXMLDoc, RIBand)

                'Treaty Surplus should be added
                RIArrangementRow_FAC(oRITable, oXMLDoc, RIBand, dsParticipentGridData, "Treaty Surplus")

                'Treaty QSh should be added
                RIArrangementRow_FAC(oRITable, oXMLDoc, RIBand, dsParticipentGridData, "Treaty QSH")

                'Treaty XOL should be added
                RIArrangementRow_FAC(oRITable, oXMLDoc, RIBand, dsParticipentGridData, "Treaty XOL")

                'Treaty CAT should be added
                RIArrangementRow_FAC(oRITable, oXMLDoc, RIBand, dsParticipentGridData, "Treaty CAT")

                'Calculate UnAllocated
                CalculateClaimUnAllocated(oRITable, oXMLDoc, RIBand)
            Next

            sReturnXML = oXMLDoc.OuterXml

        End If

        Return dsArrangementGridData
    End Function

    ''' <summary>
    ''' This Method Return the XML for Claim Reinsurance, If RI2007 is ON
    ''' </summary>
    ''' <param name="iClaimKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetClaimReinsurance2007(ByVal iClaimKey As Integer) As String
        Dim sReturnXML As String = Nothing
        Dim dsArrangementGridData As DataSet
        dsArrangementGridData = GetClaimReinsurance(iClaimKey, True, sReturnXML)

        Return sReturnXML
    End Function
    ''' <summary>
    '''  This Method Calculate the UnAllocated Band for Claim
    ''' </summary>
    ''' <param name="oRITable"></param>
    ''' <param name="oXMLDoc"></param>
    ''' <param name="RIBand"></param>
    ''' <remarks></remarks>
    Sub CalculateClaimUnAllocated(ByVal oRITable As DataTable, ByRef oXMLDoc As XmlDocument, ByVal RIBand As XmlElement)
        'Calculate/Retreive Band Total
        Dim dBANDSumInsured, dBANDReserveToDate, dBANDThisReserve, dBANDPaymentToDate, dBANDBalance, dBANDRecoverToDate As Decimal
        Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Name='Band Total']")
        If oNode IsNot Nothing Then
            Decimal.TryParse(oNode.Attributes("SumInsured").Value, dBANDSumInsured)
            Decimal.TryParse(oNode.Attributes("ReserveToDate").Value, dBANDReserveToDate)
            Decimal.TryParse(oNode.Attributes("ThisReserve").Value, dBANDThisReserve)
            Decimal.TryParse(oNode.Attributes("PaymentToDate").Value, dBANDPaymentToDate)
            Decimal.TryParse(oNode.Attributes("Balance").Value, dBANDBalance)
            Decimal.TryParse(oNode.Attributes("RecoverToDate").Value, dBANDRecoverToDate)
        End If

        'Calculate/Retreive Allocated Total
        Dim dAllocatedSumInsured, dAllocatedReserveToDate, dAllocatedThisReserve, dAllocatedPaymentToDate, dAllocatedBalance, dAllocatedRecoverToDate As Decimal
        oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Name='Allocated']")
        If oNode IsNot Nothing Then
            Decimal.TryParse(oNode.Attributes("SumInsured").Value, dAllocatedSumInsured)
            Decimal.TryParse(oNode.Attributes("ReserveToDate").Value, dAllocatedReserveToDate)
            Decimal.TryParse(oNode.Attributes("ThisReserve").Value, dAllocatedThisReserve)
            Decimal.TryParse(oNode.Attributes("PaymentToDate").Value, dAllocatedPaymentToDate)
            Decimal.TryParse(oNode.Attributes("Balance").Value, dAllocatedBalance)
            Decimal.TryParse(oNode.Attributes("RecoverToDate").Value, dAllocatedRecoverToDate)
        End If

        'Add UnAllocated if there is Any
        Dim dUnAllocatedSumInsured, dUnAllocatedReserveToDate, dUnAllocatedThisReserve, dUnAllocatedPaymentToDate, dUnAllocatedRecoverToDate, dUnAllocatedBalance As Decimal
        dUnAllocatedSumInsured = dBANDSumInsured - dAllocatedSumInsured
        dUnAllocatedReserveToDate = dBANDReserveToDate - dAllocatedReserveToDate
        dUnAllocatedThisReserve = dBANDThisReserve - dAllocatedThisReserve
        dUnAllocatedPaymentToDate = dBANDPaymentToDate - dAllocatedPaymentToDate
        dUnAllocatedRecoverToDate = dBANDRecoverToDate - dAllocatedRecoverToDate
        dUnAllocatedBalance = dBANDBalance - dAllocatedBalance

        If dUnAllocatedSumInsured <> 0 Or dUnAllocatedReserveToDate <> 0 Or dUnAllocatedThisReserve <> 0 Or dUnAllocatedPaymentToDate <> 0 _
        Or dUnAllocatedBalance <> 0 Or dUnAllocatedRecoverToDate <> 0 Then
            'Add into the XML
            Dim sArrangementRow As String = "ArrangementRow"
            Dim ArrangementRow As XmlElement = oXMLDoc.CreateElement(sArrangementRow)
            Dim myCol As DataColumn
            Dim sValue As String = ""
            For Each myCol In oRITable.Columns
                If myCol.ColumnName = "SumInsured" Or myCol.ColumnName = "ReserveToDate" _
                Or myCol.ColumnName = "Name" Or myCol.ColumnName = "ThisReserve" _
                Or myCol.ColumnName = "PaymentToDate" Or myCol.ColumnName = "Balance" _
                Or myCol.ColumnName = "RecoverToDate" Then

                    'Name
                    If myCol.ColumnName = "Name" Then
                        ArrangementRow.SetAttribute(myCol.ColumnName, "Unallocated")
                    End If

                    'Sum Insured
                    sValue = ""
                    If myCol.ColumnName = "SumInsured" Then
                        sValue = dUnAllocatedSumInsured
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    End If

                    'ReserveToDate
                    sValue = ""
                    If myCol.ColumnName = "ReserveToDate" Then
                        sValue = dUnAllocatedReserveToDate
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    End If

                    'ThisReserve
                    sValue = ""
                    If myCol.ColumnName = "ThisReserve" Then
                        sValue = dUnAllocatedThisReserve
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    End If

                    'PaymentToDate
                    sValue = ""
                    If myCol.ColumnName = "PaymentToDate" Then
                        sValue = dUnAllocatedPaymentToDate
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    End If

                    'Balance
                    sValue = ""
                    If myCol.ColumnName = "Balance" Then
                        sValue = dUnAllocatedBalance
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    End If

                    'RecoverToDate
                    sValue = ""
                    If myCol.ColumnName = "RecoverToDate" Then
                        sValue = dUnAllocatedRecoverToDate
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    End If
                Else
                    sValue = ""
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If
            Next

            RIBand.InsertAfter(ArrangementRow, RIBand.LastChild)
        End If
    End Sub
    ''' <summary>
    ''' This Method Calculate Net FAC BAND for Claim
    ''' </summary>
    ''' <param name="oRITable"></param>
    ''' <param name="oXMLDoc"></param>
    ''' <param name="RIBand"></param>
    ''' <remarks></remarks>
    Sub CalculateClaimFACNet(ByVal oRITable As DataTable, ByRef oXMLDoc As XmlDocument, ByVal RIBand As XmlElement)
        Dim bDataFound As Boolean = False

        'Calculate/Retreive Band Total
        Dim dBANDThisPayment, dBANDSumInsured, dBANDReserveToDate, dBANDThisReserve, dBANDPaymentToDate, dBANDBalance, dBANDRecoverToDate As Decimal
        Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Name='Band Total']")
        If oNode IsNot Nothing Then
            Decimal.TryParse(oNode.Attributes("SumInsured").Value, dBANDSumInsured)
            Decimal.TryParse(oNode.Attributes("ReserveToDate").Value, dBANDReserveToDate)
            Decimal.TryParse(oNode.Attributes("ThisReserve").Value, dBANDThisReserve)
            Decimal.TryParse(oNode.Attributes("PaymentToDate").Value, dBANDPaymentToDate)
            Decimal.TryParse(oNode.Attributes("Balance").Value, dBANDBalance)
            Decimal.TryParse(oNode.Attributes("RecoverToDate").Value, dBANDRecoverToDate)
            Decimal.TryParse(oNode.Attributes("ThisPayment").Value, dBANDThisPayment)
        End If

        'Retreive the FAC Prop
        Dim dFACTotThisPayment, dFACTotSumInsured, dFACTotReserveToDate, dFACTotThisReserve, dFACTotPaymentToDate, dFACTotBalance, dFACTotRecoverToDate As Decimal
        Dim xmlNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Placement='FAC Prop']")
        Dim xmlNode As XmlNode = Nothing
        If xmlNodes IsNot Nothing AndAlso xmlNodes.Count > 0 Then
            bDataFound = True
            For Each xmlNode In xmlNodes
                Dim dFACThisPayment, dFACSumInsured, dFACReserveToDate, dFACThisReserve, dFACPaymentToDate, dFACBalance, dFACRecoverToDate As Decimal
                Decimal.TryParse(xmlNode.Attributes("SumInsured").Value, dFACSumInsured)
                Decimal.TryParse(xmlNode.Attributes("ReserveToDate").Value, dFACReserveToDate)
                Decimal.TryParse(xmlNode.Attributes("ThisReserve").Value, dFACThisReserve)
                Decimal.TryParse(xmlNode.Attributes("PaymentToDate").Value, dFACPaymentToDate)
                Decimal.TryParse(xmlNode.Attributes("Balance").Value, dFACBalance)
                Decimal.TryParse(xmlNode.Attributes("RecoverToDate").Value, dFACRecoverToDate)
                Decimal.TryParse(xmlNode.Attributes("ThisPayment").Value, dFACThisPayment)

                dFACTotSumInsured += dFACSumInsured
                dFACTotReserveToDate += dFACReserveToDate
                dFACTotThisReserve += dFACThisReserve
                dFACTotPaymentToDate += dFACPaymentToDate
                dFACTotBalance += dFACBalance
                dFACTotRecoverToDate += dFACRecoverToDate
                dFACTotThisPayment += dFACThisPayment
            Next
        End If

        'Retreive the FAC XOL
        xmlNodes = Nothing
        xmlNodes = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Placement='FAC XOL']")

        If xmlNodes IsNot Nothing AndAlso xmlNodes.Count > 0 Then
            bDataFound = True
            xmlNode = Nothing
            For Each xmlNode In xmlNodes
                Dim dFACThisPayment, dFACSumInsured, dFACReserveToDate, dFACThisReserve, dFACPaymentToDate, dFACBalance, dFACRecoverToDate As Decimal
                Decimal.TryParse(xmlNode.Attributes("SumInsured").Value, dFACSumInsured)
                Decimal.TryParse(xmlNode.Attributes("ReserveToDate").Value, dFACReserveToDate)
                Decimal.TryParse(xmlNode.Attributes("ThisReserve").Value, dFACThisReserve)
                Decimal.TryParse(xmlNode.Attributes("PaymentToDate").Value, dFACPaymentToDate)
                Decimal.TryParse(xmlNode.Attributes("Balance").Value, dFACBalance)
                Decimal.TryParse(xmlNode.Attributes("RecoverToDate").Value, dFACRecoverToDate)
                Decimal.TryParse(xmlNode.Attributes("ThisPayment").Value, dFACThisPayment)

                dFACTotSumInsured += dFACSumInsured
                dFACTotReserveToDate += dFACReserveToDate
                dFACTotThisReserve += dFACThisReserve
                dFACTotPaymentToDate += dFACPaymentToDate
                dFACTotBalance += dFACBalance
                dFACTotRecoverToDate += dFACRecoverToDate
                dFACTotThisPayment += dFACThisPayment
            Next
        End If

        'Add into XML
        Dim sArrangementRow As String = "ArrangementRow"
        Dim ArrangementRow As XmlElement = oXMLDoc.CreateElement(sArrangementRow)
        Dim myCol As DataColumn
        Dim sValue As String = ""
        For Each myCol In oRITable.Columns
            If myCol.ColumnName = "SumInsured" Or myCol.ColumnName = "ReserveToDate" _
              Or myCol.ColumnName = "Name" Or myCol.ColumnName = "ThisReserve" _
              Or myCol.ColumnName = "PaymentToDate" Or myCol.ColumnName = "Balance" _
              Or myCol.ColumnName = "RecoverToDate" _
              Or myCol.ColumnName = "ThisPayment" _
              Or myCol.ColumnName = "Placement" Then

                'Placement
                If myCol.ColumnName = "Placement" Then
                    ArrangementRow.SetAttribute(myCol.ColumnName, "GROSS NET")
                End If
                'Name
                If myCol.ColumnName = "Name" Then
                    ArrangementRow.SetAttribute(myCol.ColumnName, "Net of FAC")
                End If

                'Sum Insured
                sValue = ""
                If myCol.ColumnName = "SumInsured" Then
                    sValue = dBANDSumInsured - dFACTotSumInsured
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If

                'ReserveToDate
                sValue = ""
                If myCol.ColumnName = "ReserveToDate" Then
                    sValue = dBANDReserveToDate - dFACTotReserveToDate
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If

                'ThisReserve
                sValue = ""
                If myCol.ColumnName = "ThisReserve" Then
                    sValue = dBANDThisReserve - dFACTotThisReserve
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If

                'PaymentToDate
                sValue = ""
                If myCol.ColumnName = "PaymentToDate" Then
                    sValue = dBANDPaymentToDate - dFACTotPaymentToDate
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If

                'Balance
                sValue = ""
                If myCol.ColumnName = "Balance" Then
                    sValue = dBANDBalance - dFACTotBalance
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If

                'RecoverToDate
                sValue = ""
                If myCol.ColumnName = "RecoverToDate" Then
                    sValue = dBANDRecoverToDate - dFACTotRecoverToDate
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If

                'ThisPayment
                sValue = ""
                If myCol.ColumnName = "ThisPayment" Then
                    sValue = dBANDThisPayment - dFACTotThisPayment
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If
            Else
                sValue = ""
                ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
            End If
        Next myCol

        If bDataFound = True Then
            RIBand.InsertAfter(ArrangementRow, xmlNode)
        End If

    End Sub
    ''' <summary>
    ''' This Method Calculate the UnAllocated Band
    ''' </summary>
    ''' <param name="oRITable"></param>
    ''' <param name="oXMLDoc"></param>
    ''' <param name="RIBand"></param>
    ''' <remarks></remarks>
    Sub CalculateUnAllocated(ByVal oRITable As DataTable, ByRef oXMLDoc As XmlDocument, ByVal RIBand As XmlElement)
        'Calculate/Retreive Band Total
        Dim dBANDSumInsured, dBANDPremium As Decimal
        Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Name='Band Total']")
        If oNode IsNot Nothing Then
            Decimal.TryParse(oNode.Attributes("SumInsured").Value, dBANDSumInsured)
            Decimal.TryParse(oNode.Attributes("Premium").Value, dBANDPremium)
        End If

        'Calculate/Retreive Allocated Total
        Dim dAllocatedSumInsured, dAllocatedPremium As Decimal
        oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Name='Allocated']")
        If oNode IsNot Nothing Then
            Decimal.TryParse(oNode.Attributes("SumInsured").Value, dAllocatedSumInsured)
            Decimal.TryParse(oNode.Attributes("Premium").Value, dAllocatedPremium)
        End If

        'Add UnAllocated if there is Any
        Dim dUnAllocatedSumInsured, dUnAllocatedPremium As Decimal
        dUnAllocatedSumInsured = dBANDSumInsured - dAllocatedSumInsured
        dUnAllocatedPremium = dBANDPremium - dAllocatedPremium

        If dUnAllocatedSumInsured <> 0 Or dUnAllocatedPremium <> 0 Then
            'Add into the XML
            Dim sArrangementRow As String = "ArrangementRow"
            Dim ArrangementRow As XmlElement = oXMLDoc.CreateElement(sArrangementRow)
            Dim myCol As DataColumn
            Dim sValue As String = ""
            For Each myCol In oRITable.Columns
                If myCol.ColumnName = "SumInsured" Or myCol.ColumnName = "Premium" _
                Or myCol.ColumnName = "Name" Or myCol.ColumnName = "IsDeleted" Then

                    'Name
                    If myCol.ColumnName = "Name" Then
                        ArrangementRow.SetAttribute(myCol.ColumnName, "Unallocated")
                    End If

                    'Sum Insured
                    sValue = ""
                    If myCol.ColumnName = "SumInsured" Then
                        sValue = dUnAllocatedSumInsured
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    End If

                    'Premium
                    sValue = ""
                    If myCol.ColumnName = "Premium" Then
                        sValue = dUnAllocatedPremium
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    End If

                    'IsDeleted
                    If myCol.ColumnName = "IsDeleted" Then
                        ArrangementRow.SetAttribute(myCol.ColumnName, "False")
                    End If

                Else
                    sValue = ""
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If
            Next

            RIBand.InsertAfter(ArrangementRow, RIBand.LastChild)
        End If
    End Sub
    ''' <summary>
    ''' This Method calculate Net FAC Band
    ''' </summary>
    ''' <param name="oRITable"></param>
    ''' <param name="oXMLDoc"></param>
    ''' <param name="RIBand"></param>
    ''' <remarks></remarks>
    Sub CalculateFACNet(ByVal oRITable As DataTable, ByRef oXMLDoc As XmlDocument, ByVal RIBand As XmlElement)
        Dim bDataFound As Boolean = False

        'Calculate/Retreive Band Total
        Dim dBANDSumInsured, dBANDPremium As Decimal
        Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Name='Band Total']")
        If oNode IsNot Nothing Then
            Decimal.TryParse(oNode.Attributes("SumInsured").Value, dBANDSumInsured)
            Decimal.TryParse(oNode.Attributes("Premium").Value, dBANDPremium)
        End If

        'Retreive the FAC Prop
        Dim dFACTotSumInsured, dFACTotPremium, dFACTotTax, dFACTotComm, dFACTotCommTax As Decimal
        Dim xmlNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Placement='FAC Prop']")
        Dim xmlNode As XmlNode = Nothing
        If xmlNodes IsNot Nothing AndAlso xmlNodes.Count > 0 Then
            bDataFound = True
            For Each xmlNode In xmlNodes
                Dim dFACSumInsured, dFACPremium, dFACTax, dFACComm, dFACCommTax As Decimal
                Decimal.TryParse(xmlNode.Attributes("SumInsured").Value, dFACSumInsured)
                Decimal.TryParse(xmlNode.Attributes("Premium").Value, dFACPremium)
                Decimal.TryParse(xmlNode.Attributes("Tax").Value, dFACTax)
                Decimal.TryParse(xmlNode.Attributes("Commission").Value, dFACComm)
                Decimal.TryParse(xmlNode.Attributes("CommissionTax").Value, dFACCommTax)

                dFACTotSumInsured += dFACSumInsured
                dFACTotPremium += dFACPremium
                dFACTotTax += dFACTax
                dFACTotComm += dFACComm
                dFACTotCommTax += dFACCommTax
            Next
        End If

        'Retreive the FAC XOL
        xmlNodes = Nothing
        xmlNodes = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Placement='FAC XOL']")

        If xmlNodes IsNot Nothing AndAlso xmlNodes.Count > 0 Then
            bDataFound = True
            xmlNode = Nothing
            For Each xmlNode In xmlNodes
                Dim dFACSumInsured, dFACPremium, dFACTax, dFACComm, dFACCommTax As Decimal
                Decimal.TryParse(xmlNode.Attributes("SumInsured").Value, dFACSumInsured)
                Decimal.TryParse(xmlNode.Attributes("Premium").Value, dFACPremium)
                Decimal.TryParse(xmlNode.Attributes("Tax").Value, dFACTax)
                Decimal.TryParse(xmlNode.Attributes("Commission").Value, dFACComm)
                Decimal.TryParse(xmlNode.Attributes("CommissionTax").Value, dFACCommTax)

                dFACTotSumInsured += dFACSumInsured
                dFACTotPremium += dFACPremium
                dFACTotTax += dFACTax
                dFACTotComm += dFACComm
                dFACTotCommTax += dFACCommTax
            Next
        End If

        'Add into XML
        Dim sArrangementRow As String = "ArrangementRow"
        Dim ArrangementRow As XmlElement = oXMLDoc.CreateElement(sArrangementRow)
        Dim myCol As DataColumn
        Dim sValue As String = ""
        For Each myCol In oRITable.Columns
            If myCol.ColumnName = "SumInsured" Or myCol.ColumnName = "Premium" _
            Or myCol.ColumnName = "Tax" Or myCol.ColumnName = "Commission" _
            Or myCol.ColumnName = "CommissionTax" Or myCol.ColumnName = "Name" _
            Or myCol.ColumnName = "Placement" Then
                'Placement
                If myCol.ColumnName = "Placement" Then
                    ArrangementRow.SetAttribute(myCol.ColumnName, "GROSS NET")
                End If
                'Name
                If myCol.ColumnName = "Name" Then
                    ArrangementRow.SetAttribute(myCol.ColumnName, "Net of FAC")
                End If

                'Sum Insured
                sValue = ""
                If myCol.ColumnName = "SumInsured" Then
                    sValue = dBANDSumInsured - dFACTotSumInsured
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If

                'Premium
                sValue = ""
                If myCol.ColumnName = "Premium" Then
                    sValue = dBANDPremium - dFACTotPremium
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If

                'Tax
                sValue = ""
                If myCol.ColumnName = "Tax" Then
                    sValue = dFACTotTax
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If

                'Commission
                sValue = ""
                If myCol.ColumnName = "Commission" Then
                    sValue = dFACTotComm
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If

                'CommissionTax
                sValue = ""
                If myCol.ColumnName = "CommissionTax" Then
                    sValue = dFACTotCommTax
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If
                If myCol.ColumnName = "IsDeleted" Then
                    sValue = "False"
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If
            ElseIf myCol.ColumnName = "IsDeleted" Then
                sValue = "False"
                ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
            Else
                sValue = ""
                ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
            End If
        Next myCol

        If bDataFound = True Then
            RIBand.InsertAfter(ArrangementRow, xmlNode)
        End If

    End Sub
    ''' <summary>
    ''' Claim RIArrangementRow_Others calculate the other row from RI Table
    ''' </summary>
    ''' <param name="oRITable"></param>
    ''' <param name="oXMLDoc"></param>
    ''' <param name="RIBand"></param>
    ''' <param name="dsParticipentGridData"></param>
    ''' <remarks></remarks>
    Sub ClaimRIArrangementRow_Others(ByVal oRITable As DataTable, ByRef oXMLDoc As XmlDocument, _
                        ByVal RIBand As XmlElement, _
                        ByVal dsParticipentGridData As DataSet)
        If oRITable.TableName.Contains("Current") Then
            'sReturnXML = Nothing
            For Each myRow As DataRow In oRITable.Rows

                If IsDBNull(myRow("Placement")) = True Or (IsDBNull(myRow("Placement")) = False _
               AndAlso (myRow("Placement").Trim.ToUpper <> "FAC XOL" And myRow("Placement").Trim.ToUpper <> "FAC PROP" _
                        And myRow("Placement").Trim.ToUpper <> "TREATY SURPLUS" And myRow("Placement").Trim.ToUpper <> "TREATY QSH" _
                        And myRow("Placement").Trim.ToUpper <> "TREATY XOL" And myRow("Placement").Trim.ToUpper <> "TREATY CAT")) Then

                    Dim sArrangementRow As String = "ArrangementRow"
                    Dim ArrangementRow As XmlElement = oXMLDoc.CreateElement(sArrangementRow)
                    Dim myCol As DataColumn
                    For Each myCol In oRITable.Columns

                        Dim sValue As String = ""
                        If IsDBNull(myRow.Item(myCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myRow.Item(myCol.ColumnName)) = False Then
                            sValue = myRow.Item(myCol.ColumnName)
                        End If
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    Next myCol

                    'Broker Participent (FAC PROP)
                    Dim oBrokerPartTable As DataTable = dsParticipentGridData.Tables("Current_Broker_Participent")
                    For Each myBrokerPartRow As DataRow In oBrokerPartTable.Rows
                        Dim sBrokerParticipentRow As String = "BrokerParticipentRow"
                        Dim BrokerParticipentRow As XmlElement = oXMLDoc.CreateElement(sBrokerParticipentRow)
                        Dim myBrokerPartCol As DataColumn

                        'Search the related records
                        If IsDBNull(myRow("RIArrangementLineKey")) = False AndAlso IsDBNull(myRow("RIarrangementKey")) = False Then
                            If myRow("RIArrangementLineKey") = myBrokerPartRow("RIArrangementLineKey") AndAlso myRow("RIarrangementKey") = myBrokerPartRow("RIarrangementKey") Then
                                'Found
                                'Add in to XML

                                For Each myBrokerPartCol In oBrokerPartTable.Columns
                                    Dim sValue As String = ""

                                    If IsDBNull(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False Then
                                        sValue = myBrokerPartRow.Item(myBrokerPartCol.ColumnName)
                                    End If
                                    BrokerParticipentRow.SetAttribute(myBrokerPartCol.ColumnName, sValue)
                                Next
                                ArrangementRow.AppendChild(BrokerParticipentRow)

                            End If
                        End If
                    Next

                    'FAX Participent
                    Dim oFAXPartTable As DataTable = dsParticipentGridData.Tables("Current_FAX_Participent")
                    For Each myBrokerPartRow As DataRow In oFAXPartTable.Rows

                        Dim myBrokerPartCol As DataColumn
                        'Search the related records
                        If IsDBNull(myRow("RIarrangementKey")) = False AndAlso IsDBNull(myRow("RIArrangementLineKey")) = False Then
                            If myRow("RIArrangementLineKey") = myBrokerPartRow("RIArrangementLineKey") AndAlso myRow("RIarrangementKey") = myBrokerPartRow("RIarrangementKey") Then
                                'Found
                                'Add in to XML
                                Dim sFAXParticipentRow As String = "FAXParticipentRow"
                                Dim FAXParticipentRow As XmlElement = oXMLDoc.CreateElement(sFAXParticipentRow)

                                For Each myBrokerPartCol In oBrokerPartTable.Columns
                                    Dim sValue As String = ""

                                    If IsDBNull(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False Then
                                        sValue = myBrokerPartRow.Item(myBrokerPartCol.ColumnName)
                                    End If
                                    FAXParticipentRow.SetAttribute(myBrokerPartCol.ColumnName, sValue)
                                Next

                                'FAX Broker Participent
                                Dim oFAXBrokerPartTable As DataTable = dsParticipentGridData.Tables("Current_FAX_Broker_Participent")
                                For Each myFAXBrokerPartRow As DataRow In oFAXBrokerPartTable.Rows

                                    Dim myFAXBrokerPartCol As DataColumn
                                    If IsDBNull(myBrokerPartRow("RIarrangementKey")) = False AndAlso IsDBNull(myBrokerPartRow("RIArrangementLineKey")) = False Then
                                        If myBrokerPartRow("RIarrangementKey") = myFAXBrokerPartRow("RIarrangementKey") AndAlso myBrokerPartRow("RIArrangementLineKey") = myFAXBrokerPartRow("RIArrangementLineKey") Then
                                            'Found
                                            'Add in to XML
                                            Dim sFAXBrokerParticipentRow As String = "FAXParticipentRow"
                                            Dim FAXBrokerParticipentRow As XmlElement = oXMLDoc.CreateElement(sFAXBrokerParticipentRow)

                                            For Each myFAXBrokerPartCol In oFAXBrokerPartTable.Columns

                                                Dim sValue As String = ""
                                                If IsDBNull(myFAXBrokerPartRow.Item(myFAXBrokerPartCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myFAXBrokerPartRow.Item(myFAXBrokerPartCol.ColumnName)) = False Then
                                                    sValue = myFAXBrokerPartRow.Item(myFAXBrokerPartCol.ColumnName)
                                                End If
                                                FAXBrokerParticipentRow.SetAttribute(myFAXBrokerPartCol.ColumnName, sValue)
                                            Next
                                            FAXParticipentRow.AppendChild(FAXBrokerParticipentRow)
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                    'Current Table End here

                    'Add into XML Document
                    RIBand.AppendChild(ArrangementRow)

                End If
            Next
        End If
    End Sub
    ''' <summary>
    ''' RIArrangementRow_Others calculate the other row from RI Table
    ''' </summary>
    ''' <param name="oRITable"></param>
    ''' <param name="oXMLDoc"></param>
    ''' <param name="RIBand"></param>
    ''' <param name="dsParticipentGridData"></param>
    ''' <remarks></remarks>
    Sub RIArrangementRow_Others(ByVal oRITable As DataTable, ByRef oXMLDoc As XmlDocument, _
                         ByVal RIBand As XmlElement, _
                         ByVal dsParticipentGridData As DataSet)
        If oRITable.TableName.Contains("Current") Then
            'sReturnXML = Nothing
            For Each myRow As DataRow In oRITable.Rows

                If IsDBNull(myRow("Placement")) = True Or (IsDBNull(myRow("Placement")) = False _
               AndAlso (myRow("Placement").Trim.ToUpper <> "FAC XOL" And myRow("Placement").Trim.ToUpper <> "FAC PROP")) Then

                    Dim sArrangementRow As String = "ArrangementRow"
                    Dim ArrangementRow As XmlElement = oXMLDoc.CreateElement(sArrangementRow)
                    Dim myCol As DataColumn
                    For Each myCol In oRITable.Columns

                        Dim sValue As String = ""
                        If IsDBNull(myRow.Item(myCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myRow.Item(myCol.ColumnName)) = False Then
                            sValue = myRow.Item(myCol.ColumnName)
                        End If
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    Next myCol

                    'Broker Participent (FAC PROP)
                    Dim oBrokerPartTable As DataTable = dsParticipentGridData.Tables("Current_Broker_Participent")
                    For Each myBrokerPartRow As DataRow In oBrokerPartTable.Rows
                        Dim sBrokerParticipentRow As String = "BrokerParticipentRow"
                        Dim BrokerParticipentRow As XmlElement = oXMLDoc.CreateElement(sBrokerParticipentRow)
                        Dim myBrokerPartCol As DataColumn

                        'Search the related records
                        If IsDBNull(myRow("RIArrangementLineKey")) = False AndAlso IsDBNull(myRow("RIarrangementKey")) = False Then
                            If myRow("RIArrangementLineKey") = myBrokerPartRow("RIArrangementLineKey") AndAlso myRow("RIarrangementKey") = myBrokerPartRow("RIarrangementKey") Then
                                'Found
                                'Add in to XML

                                For Each myBrokerPartCol In oBrokerPartTable.Columns
                                    Dim sValue As String = ""

                                    If IsDBNull(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False Then
                                        sValue = myBrokerPartRow.Item(myBrokerPartCol.ColumnName)
                                    End If
                                    BrokerParticipentRow.SetAttribute(myBrokerPartCol.ColumnName, sValue)
                                Next
                                ArrangementRow.AppendChild(BrokerParticipentRow)

                            End If
                        End If
                    Next

                    'FAX Participent
                    Dim oFAXPartTable As DataTable = dsParticipentGridData.Tables("Current_FAX_Participent")
                    For Each myBrokerPartRow As DataRow In oFAXPartTable.Rows

                        Dim myBrokerPartCol As DataColumn
                        'Search the related records
                        If IsDBNull(myRow("RIarrangementKey")) = False AndAlso IsDBNull(myRow("RIArrangementLineKey")) = False Then
                            If myRow("RIArrangementLineKey") = myBrokerPartRow("RIArrangementLineKey") AndAlso myRow("RIarrangementKey") = myBrokerPartRow("RIarrangementKey") Then
                                'Found
                                'Add in to XML
                                Dim sFAXParticipentRow As String = "FAXParticipentRow"
                                Dim FAXParticipentRow As XmlElement = oXMLDoc.CreateElement(sFAXParticipentRow)

                                For Each myBrokerPartCol In oBrokerPartTable.Columns
                                    Dim sValue As String = ""

                                    If IsDBNull(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False Then
                                        sValue = myBrokerPartRow.Item(myBrokerPartCol.ColumnName)
                                    End If
                                    FAXParticipentRow.SetAttribute(myBrokerPartCol.ColumnName, sValue)
                                Next

                                'FAX Broker Participent
                                Dim oFAXBrokerPartTable As DataTable = dsParticipentGridData.Tables("Current_FAX_Broker_Participent")
                                For Each myFAXBrokerPartRow As DataRow In oFAXBrokerPartTable.Rows

                                    Dim myFAXBrokerPartCol As DataColumn
                                    If IsDBNull(myBrokerPartRow("RIarrangementKey")) = False AndAlso IsDBNull(myBrokerPartRow("RIArrangementLineKey")) = False Then
                                        If myBrokerPartRow("RIarrangementKey") = myFAXBrokerPartRow("RIarrangementKey") AndAlso myBrokerPartRow("RIArrangementLineKey") = myFAXBrokerPartRow("RIArrangementLineKey") Then
                                            'Found
                                            'Add in to XML
                                            Dim sFAXBrokerParticipentRow As String = "FAXParticipentRow"
                                            Dim FAXBrokerParticipentRow As XmlElement = oXMLDoc.CreateElement(sFAXBrokerParticipentRow)

                                            For Each myFAXBrokerPartCol In oFAXBrokerPartTable.Columns

                                                Dim sValue As String = ""
                                                If IsDBNull(myFAXBrokerPartRow.Item(myFAXBrokerPartCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myFAXBrokerPartRow.Item(myFAXBrokerPartCol.ColumnName)) = False Then
                                                    sValue = myFAXBrokerPartRow.Item(myFAXBrokerPartCol.ColumnName)
                                                End If
                                                FAXBrokerParticipentRow.SetAttribute(myFAXBrokerPartCol.ColumnName, sValue)
                                            Next
                                            FAXParticipentRow.AppendChild(FAXBrokerParticipentRow)
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                    'Current Table End here

                    'Add into XML Document
                    RIBand.AppendChild(ArrangementRow)

                End If
            Next

        ElseIf oRITable.TableName.Contains("Original") Then
            For Each myRow As DataRow In oRITable.Rows

                If IsDBNull(myRow("Placement")) = True Or (IsDBNull(myRow("Placement")) = False _
                           AndAlso (myRow("Placement").Trim.ToUpper <> "FAC XOL" And myRow("Placement").Trim.ToUpper <> "FAC PROP")) Then
                    Dim sArrangementRow As String = "ArrangementRow"
                    Dim ArrangementRow As XmlElement = oXMLDoc.CreateElement(sArrangementRow)
                    Dim myCol As DataColumn
                    For Each myCol In oRITable.Columns

                        Dim sValue As String = ""
                        If IsDBNull(myRow.Item(myCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myRow.Item(myCol.ColumnName)) = False Then

                            sValue = myRow.Item(myCol.ColumnName)
                        End If
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    Next myCol

                    'Broker Participent (FAC PROP)
                    Dim oBrokerPartTable As DataTable = dsParticipentGridData.Tables("Original_Broker_Participent")
                    For Each myBrokerPartRow As DataRow In oBrokerPartTable.Rows

                        Dim myBrokerPartCol As DataColumn
                        'Search the related records
                        If IsDBNull(myRow("RIArrangementLineKey")) = False AndAlso IsDBNull(myRow("RIarrangementKey")) = False Then
                            If String.IsNullOrEmpty(myRow("RIArrangementLineKey")) = False AndAlso String.IsNullOrEmpty(myRow("RIarrangementKey")) = False Then
                                If myRow("RIArrangementLineKey") = myBrokerPartRow("RIArrangementLineKey") AndAlso myRow("RIarrangementKey") = myBrokerPartRow("RIarrangementKey") Then
                                    'Found
                                    'Add in to XML
                                    Dim sBrokerParticipentRow As String = "BrokerParticipentRow"
                                    Dim BrokerParticipentRow As XmlElement = oXMLDoc.CreateElement(sBrokerParticipentRow)

                                    For Each myBrokerPartCol In oBrokerPartTable.Columns

                                        Dim sValue As String = ""
                                        If IsDBNull(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False Then

                                            sValue = myBrokerPartRow.Item(myBrokerPartCol.ColumnName)
                                        End If
                                        BrokerParticipentRow.SetAttribute(myBrokerPartCol.ColumnName, sValue)
                                    Next
                                    ArrangementRow.AppendChild(BrokerParticipentRow)

                                End If
                            End If
                        End If
                    Next

                    'FAX Participent
                    Dim oFAXPartTable As DataTable = dsParticipentGridData.Tables("Original_FAX_Participent")
                    For Each myBrokerPartRow As DataRow In oBrokerPartTable.Rows

                        Dim myBrokerPartCol As DataColumn
                        'Search the related records
                        If IsDBNull(myRow("RIArrangementLineKey")) = False AndAlso IsDBNull(myRow("RIarrangementKey")) = False Then
                            If String.IsNullOrEmpty(myRow("RIarrangementKey")) = False Then
                                If myRow("RIarrangementKey") = myBrokerPartRow("RIarrangementKey") Then
                                    'Found
                                    'Add in to XML
                                    Dim sFAXParticipentRow As String = "FAXParticipentRow"
                                    Dim FAXParticipentRow As XmlElement = oXMLDoc.CreateElement(sFAXParticipentRow)

                                    For Each myBrokerPartCol In oBrokerPartTable.Columns

                                        Dim sValue As String = ""
                                        If IsDBNull(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False Then

                                            sValue = myBrokerPartRow.Item(myBrokerPartCol.ColumnName)

                                        End If
                                        FAXParticipentRow.SetAttribute(myBrokerPartCol.ColumnName, sValue)
                                    Next

                                    'FAX Broker Participent
                                    Dim oFAXBrokerPartTable As DataTable = dsParticipentGridData.Tables("Original_FAX_Broker_Participent")
                                    For Each myFAXBrokerPartRow As DataRow In oFAXBrokerPartTable.Rows

                                        Dim myFAXBrokerPartCol As DataColumn
                                        If IsDBNull(myBrokerPartRow("RIArrangementLineKey")) = False AndAlso IsDBNull(myBrokerPartRow("RIarrangementKey")) = False Then
                                            If String.IsNullOrEmpty(myBrokerPartRow("RIarrangementKey")) = False AndAlso String.IsNullOrEmpty(myBrokerPartRow("RIArrangementLineKey")) = False Then
                                                If myBrokerPartRow("RIarrangementKey") = myFAXBrokerPartRow("RIarrangementKey") AndAlso myBrokerPartRow("RIArrangementLineKey") = myFAXBrokerPartRow("RIArrangementLineKey") Then
                                                    'Found
                                                    'Add in to XML
                                                    Dim sFAXBrokerParticipentRow As String = "FAXBrokerParticipentRow"
                                                    Dim FAXBrokerParticipentRow As XmlElement = oXMLDoc.CreateElement(sFAXParticipentRow)

                                                    For Each myFAXBrokerPartCol In oFAXBrokerPartTable.Columns

                                                        Dim sValue As String = ""
                                                        If IsDBNull(myFAXBrokerPartRow.Item(myFAXBrokerPartCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myFAXBrokerPartRow.Item(myFAXBrokerPartCol.ColumnName)) = False Then

                                                            sValue = myFAXBrokerPartRow.Item(myFAXBrokerPartCol.ColumnName)

                                                        End If
                                                        FAXBrokerParticipentRow.SetAttribute(myFAXBrokerPartCol.ColumnName, sValue)
                                                    Next
                                                    FAXParticipentRow.AppendChild(FAXBrokerParticipentRow)

                                                End If
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    Next

                    'Current Table End here

                    'Add into XML Document
                    RIBand.AppendChild(ArrangementRow)

                End If
            Next
        End If
    End Sub
    ''' <summary>
    ''' This Method Calculate the FAC Prop or FAC XOL
    ''' </summary>
    ''' <param name="oRITable"></param>
    ''' <param name="oXMLDoc"></param>
    ''' <param name="RIBand"></param>
    ''' <param name="dsParticipentGridData"></param>
    ''' <param name="sCondition"></param>
    ''' <remarks></remarks>
    Sub RIArrangementRow_FAC(ByVal oRITable As DataTable, ByRef oXMLDoc As XmlDocument, _
                         ByVal RIBand As XmlElement, _
                         ByVal dsParticipentGridData As DataSet, ByVal sCondition As String)

        If oRITable.TableName.Contains("Current") Then
            'sReturnXML = Nothing
            If sCondition.Trim.ToUpper = "TREATY XOL" Or sCondition.Trim.ToUpper = "TREATY CAT" Then
                Dim XOLRows() As DataRow = oRITable.Select("Placement='" & sCondition.Trim & "'", "LowerLimit Desc", DataViewRowState.CurrentRows)

                For Each myRow As DataRow In XOLRows
                    Dim sArrangementRow As String = "ArrangementRow"
                    Dim ArrangementRow As XmlElement = oXMLDoc.CreateElement(sArrangementRow)
                    Dim myCol As DataColumn
                    For Each myCol In oRITable.Columns

                        Dim sValue As String = ""
                        If IsDBNull(myRow.Item(myCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myRow.Item(myCol.ColumnName)) = False Then
                            sValue = myRow.Item(myCol.ColumnName)
                        End If
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    Next myCol

                    If sCondition.Trim.ToUpper = "TREATY XOL" Then
                        Dim oNetFACNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & RIBand.Attributes("Name").Value & "']/ArrangementRow[@Name='Net of FAC']")
                        Dim oTreatySurplusNode As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & RIBand.Attributes("Name").Value & "']/ArrangementRow[@Placement='Treaty Surplus']")
                        Dim oTreatyQSHNode As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & RIBand.Attributes("Name").Value & "']/ArrangementRow[@Placement='Treaty QSH']")

                        If oTreatyQSHNode IsNot Nothing AndAlso oTreatyQSHNode.Count > 0 Then
                            RIBand.InsertAfter(ArrangementRow, oTreatyQSHNode(oTreatyQSHNode.Count - 1))
                        ElseIf oTreatySurplusNode IsNot Nothing AndAlso oTreatySurplusNode.Count > 0 Then
                            RIBand.InsertAfter(ArrangementRow, oTreatySurplusNode(oTreatySurplusNode.Count - 1))
                        ElseIf oNetFACNode IsNot Nothing Then
                            RIBand.InsertAfter(ArrangementRow, oNetFACNode)
                        Else
                            If RIBand.HasChildNodes Then
                                RIBand.InsertAfter(ArrangementRow, RIBand.FirstChild)
                            Else
                                RIBand.AppendChild(ArrangementRow)
                            End If
                        End If
                    ElseIf sCondition.Trim.ToUpper = "TREATY CAT" Then
                        Dim oNetFACNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & RIBand.Attributes("Name").Value & "']/ArrangementRow[@Name='Net of FAC']")
                        Dim oTreatySurplusNode As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & RIBand.Attributes("Name").Value & "']/ArrangementRow[@Placement='Treaty Surplus']")
                        Dim oTreatyQSHNode As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & RIBand.Attributes("Name").Value & "']/ArrangementRow[@Placement='Treaty QSH']")
                        Dim oTreatyXOLNode As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & RIBand.Attributes("Name").Value & "']/ArrangementRow[@Placement='Treaty XOL']")

                        If oTreatyXOLNode IsNot Nothing AndAlso oTreatyXOLNode.Count > 0 Then
                            RIBand.InsertAfter(ArrangementRow, oTreatyXOLNode(oTreatyXOLNode.Count - 1))
                        ElseIf oTreatyQSHNode IsNot Nothing AndAlso oTreatyQSHNode.Count > 0 Then
                            RIBand.InsertAfter(ArrangementRow, oTreatyQSHNode(oTreatyQSHNode.Count - 1))
                        ElseIf oTreatySurplusNode IsNot Nothing AndAlso oTreatySurplusNode.Count > 0 Then
                            RIBand.InsertAfter(ArrangementRow, oTreatySurplusNode(oTreatySurplusNode.Count - 1))
                        ElseIf oNetFACNode IsNot Nothing Then
                            RIBand.InsertAfter(ArrangementRow, oNetFACNode)
                        Else
                            If RIBand.HasChildNodes Then
                                RIBand.InsertAfter(ArrangementRow, RIBand.FirstChild)
                            Else
                                RIBand.AppendChild(ArrangementRow)
                            End If
                        End If
                    End If
                Next

            Else
                For Each myRow As DataRow In oRITable.Rows

                    If IsDBNull(myRow("Placement")) = False AndAlso myRow("Placement").Trim.ToUpper = sCondition.Trim.ToUpper Then

                        Dim sArrangementRow As String = "ArrangementRow"
                        Dim ArrangementRow As XmlElement = oXMLDoc.CreateElement(sArrangementRow)
                        Dim myCol As DataColumn
                        For Each myCol In oRITable.Columns

                            Dim sValue As String = ""
                            If IsDBNull(myRow.Item(myCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myRow.Item(myCol.ColumnName)) = False Then
                                sValue = myRow.Item(myCol.ColumnName)
                            End If
                            ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                        Next myCol

                        'Broker Participent (FAC PROP)
                        Dim oBrokerPartTable As DataTable = dsParticipentGridData.Tables("Current_Broker_Participent")
                        For Each myBrokerPartRow As DataRow In oBrokerPartTable.Rows
                            Dim sBrokerParticipentRow As String = "BrokerParticipentRow"
                            Dim BrokerParticipentRow As XmlElement = oXMLDoc.CreateElement(sBrokerParticipentRow)
                            Dim myBrokerPartCol As DataColumn

                            'Search the related records
                            If IsDBNull(myRow("RIArrangementLineKey")) = False AndAlso IsDBNull(myRow("RIarrangementKey")) = False Then
                                If myRow("RIArrangementLineKey") = myBrokerPartRow("RIArrangementLineKey") AndAlso myRow("RIarrangementKey") = myBrokerPartRow("RIarrangementKey") Then
                                    'Found
                                    'Add in to XML

                                    For Each myBrokerPartCol In oBrokerPartTable.Columns
                                        Dim sValue As String = ""

                                        If IsDBNull(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False Then
                                            sValue = myBrokerPartRow.Item(myBrokerPartCol.ColumnName)
                                        End If
                                        BrokerParticipentRow.SetAttribute(myBrokerPartCol.ColumnName, sValue)
                                    Next
                                    ArrangementRow.AppendChild(BrokerParticipentRow)

                                End If
                            End If
                        Next

                        'FAX Participent
                        Dim oFAXPartTable As DataTable = dsParticipentGridData.Tables("Current_FAX_Participent")
                        For Each myBrokerPartRow As DataRow In oFAXPartTable.Rows
                            'Found
                            'Add in to XML
                            Dim sFAXParticipentRow As String = "FAXParticipentRow"
                            Dim FAXParticipentRow As XmlElement = oXMLDoc.CreateElement(sFAXParticipentRow)

                            Dim myBrokerPartCol As DataColumn
                            'Search the related records

                            If IsDBNull(myRow("Grouping")) = False AndAlso IsDBNull(myRow("Grouping")) = False Then
                                If myRow("Grouping") = myBrokerPartRow("Grouping") AndAlso myRow("RIarrangementKey") = myBrokerPartRow("RIarrangementKey") Then

                                    For Each myBrokerPartCol In oFAXPartTable.Columns
                                        Dim sValue As String = ""

                                        If IsDBNull(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False Then
                                            sValue = myBrokerPartRow.Item(myBrokerPartCol.ColumnName)
                                        End If
                                        FAXParticipentRow.SetAttribute(myBrokerPartCol.ColumnName, sValue)
                                    Next

                                    'FAX Broker Participent
                                    Dim oFAXBrokerPartTable As DataTable = dsParticipentGridData.Tables("Current_FAX_Broker_Participent")
                                    For Each myFAXBrokerPartRow As DataRow In oFAXBrokerPartTable.Rows

                                        Dim myFAXBrokerPartCol As DataColumn
                                        If IsDBNull(myBrokerPartRow("RIarrangementKey")) = False AndAlso IsDBNull(myBrokerPartRow("RIArrangementLineKey")) = False Then
                                            If myBrokerPartRow("RIarrangementKey") = myFAXBrokerPartRow("RIarrangementKey") AndAlso myBrokerPartRow("RIArrangementLineKey") = myFAXBrokerPartRow("RIArrangementLineKey") Then
                                                'Found
                                                'Add in to XML
                                                Dim sFAXBrokerParticipentRow As String = "FAXBrokerParticipentRow"
                                                Dim FAXBrokerParticipentRow As XmlElement = oXMLDoc.CreateElement(sFAXBrokerParticipentRow)

                                                For Each myFAXBrokerPartCol In oFAXBrokerPartTable.Columns

                                                    Dim sValue As String = ""
                                                    If IsDBNull(myFAXBrokerPartRow.Item(myFAXBrokerPartCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myFAXBrokerPartRow.Item(myFAXBrokerPartCol.ColumnName)) = False Then
                                                        sValue = myFAXBrokerPartRow.Item(myFAXBrokerPartCol.ColumnName)
                                                    End If
                                                    FAXBrokerParticipentRow.SetAttribute(myFAXBrokerPartCol.ColumnName, sValue)
                                                Next
                                                FAXParticipentRow.AppendChild(FAXBrokerParticipentRow)
                                            End If
                                        End If
                                    Next
                                    ArrangementRow.AppendChild(FAXParticipentRow)
                                End If
                            End If
                        Next
                        'Current Table End here

                        'Add into XML Document
                        If sCondition.Trim.ToUpper = "TREATY SURPLUS" Then
                            Dim oNetFACNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & RIBand.Attributes("Name").Value & "']/ArrangementRow[@Name='Net of FAC']")
                            If oNetFACNode IsNot Nothing Then
                                RIBand.InsertAfter(ArrangementRow, oNetFACNode)
                            Else
                                If RIBand.HasChildNodes Then
                                    RIBand.InsertAfter(ArrangementRow, RIBand.FirstChild)
                                Else
                                    RIBand.AppendChild(ArrangementRow)
                                End If
                            End If
                        ElseIf sCondition.Trim.ToUpper = "TREATY QSH" Then
                            Dim oNetFACNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & RIBand.Attributes("Name").Value & "']/ArrangementRow[@Name='Net of FAC']")
                            Dim oTreatySurplusNode As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & RIBand.Attributes("Name").Value & "']/ArrangementRow[@Placement='Treaty Surplus']")
                            If oNetFACNode IsNot Nothing AndAlso (oTreatySurplusNode Is Nothing Or oTreatySurplusNode IsNot Nothing AndAlso oTreatySurplusNode.Count = 0) Then
                                RIBand.InsertAfter(ArrangementRow, oNetFACNode)
                            ElseIf oTreatySurplusNode IsNot Nothing AndAlso oTreatySurplusNode.Count > 0 Then
                                RIBand.InsertAfter(ArrangementRow, oTreatySurplusNode(oTreatySurplusNode.Count - 1))
                            Else
                                If RIBand.HasChildNodes Then
                                    RIBand.InsertAfter(ArrangementRow, RIBand.FirstChild)
                                Else
                                    RIBand.AppendChild(ArrangementRow)
                                End If
                            End If
                        Else
                            If RIBand.HasChildNodes Then
                                RIBand.InsertAfter(ArrangementRow, RIBand.FirstChild)
                            Else
                                RIBand.AppendChild(ArrangementRow)
                            End If
                        End If
                    End If
                Next
            End If
        ElseIf oRITable.TableName.Contains("Original") Then
            For Each myRow As DataRow In oRITable.Rows

                If IsDBNull(myRow("Placement")) = False AndAlso myRow("Placement").Trim.ToUpper = sCondition.Trim.ToUpper Then

                    Dim sArrangementRow As String = "ArrangementRow"
                    Dim ArrangementRow As XmlElement = oXMLDoc.CreateElement(sArrangementRow)
                    Dim myCol As DataColumn
                    For Each myCol In oRITable.Columns

                        Dim sValue As String = ""
                        If IsDBNull(myRow.Item(myCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myRow.Item(myCol.ColumnName)) = False Then
                            sValue = myRow.Item(myCol.ColumnName)
                        End If
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    Next myCol

                    'Broker Participent (FAC PROP)
                    Dim oBrokerPartTable As DataTable = dsParticipentGridData.Tables("Original_Broker_Participent")
                    For Each myBrokerPartRow As DataRow In oBrokerPartTable.Rows

                        Dim myBrokerPartCol As DataColumn
                        'Search the related records
                        If String.IsNullOrEmpty(myRow("RIArrangementLineKey")) = False AndAlso String.IsNullOrEmpty(myRow("RIarrangementKey")) = False Then
                            If myRow("RIArrangementLineKey") = myBrokerPartRow("RIArrangementLineKey") AndAlso myRow("RIarrangementKey") = myBrokerPartRow("RIarrangementKey") Then
                                'Found
                                'Add in to XML
                                Dim sBrokerParticipentRow As String = "BrokerParticipentRow"
                                Dim BrokerParticipentRow As XmlElement = oXMLDoc.CreateElement(sBrokerParticipentRow)

                                For Each myBrokerPartCol In oBrokerPartTable.Columns

                                    Dim sValue As String = ""
                                    If IsDBNull(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False Then

                                        sValue = myBrokerPartRow.Item(myBrokerPartCol.ColumnName)
                                    End If
                                    BrokerParticipentRow.SetAttribute(myBrokerPartCol.ColumnName, sValue)
                                Next
                                ArrangementRow.AppendChild(BrokerParticipentRow)

                            End If
                        End If
                    Next

                    'FAX Participent
                    Dim oFAXPartTable As DataTable = dsParticipentGridData.Tables("Original_FAX_Participent")
                    For Each myBrokerPartRow As DataRow In oBrokerPartTable.Rows

                        Dim myBrokerPartCol As DataColumn
                        'Search the related records
                        If String.IsNullOrEmpty(myRow("RIarrangementKey")) = False Then
                            If myRow("RIarrangementKey") = myBrokerPartRow("RIarrangementKey") Then
                                'Found
                                'Add in to XML
                                Dim sFAXParticipentRow As String = "FAXParticipentRow"
                                Dim FAXParticipentRow As XmlElement = oXMLDoc.CreateElement(sFAXParticipentRow)

                                For Each myBrokerPartCol In oBrokerPartTable.Columns

                                    Dim sValue As String = ""
                                    If IsDBNull(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myBrokerPartRow.Item(myBrokerPartCol.ColumnName)) = False Then

                                        sValue = myBrokerPartRow.Item(myBrokerPartCol.ColumnName)

                                    End If
                                    FAXParticipentRow.SetAttribute(myBrokerPartCol.ColumnName, sValue)
                                Next

                                'FAX Broker Participent
                                Dim oFAXBrokerPartTable As DataTable = dsParticipentGridData.Tables("Original_FAX_Broker_Participent")
                                For Each myFAXBrokerPartRow As DataRow In oFAXBrokerPartTable.Rows

                                    Dim myFAXBrokerPartCol As DataColumn
                                    If String.IsNullOrEmpty(myBrokerPartRow("RIarrangementKey")) = False AndAlso String.IsNullOrEmpty(myBrokerPartRow("RIArrangementLineKey")) = False Then
                                        If myBrokerPartRow("RIarrangementKey") = myFAXBrokerPartRow("RIarrangementKey") AndAlso myBrokerPartRow("RIArrangementLineKey") = myFAXBrokerPartRow("RIArrangementLineKey") Then
                                            'Found
                                            'Add in to XML
                                            Dim sFAXBrokerParticipentRow As String = "FAXBrokerParticipentRow"
                                            Dim FAXBrokerParticipentRow As XmlElement = oXMLDoc.CreateElement(sFAXParticipentRow)

                                            For Each myFAXBrokerPartCol In oFAXBrokerPartTable.Columns

                                                Dim sValue As String = ""
                                                If IsDBNull(myFAXBrokerPartRow.Item(myFAXBrokerPartCol.ColumnName)) = False AndAlso String.IsNullOrEmpty(myFAXBrokerPartRow.Item(myFAXBrokerPartCol.ColumnName)) = False Then

                                                    sValue = myFAXBrokerPartRow.Item(myFAXBrokerPartCol.ColumnName)

                                                End If
                                                FAXBrokerParticipentRow.SetAttribute(myFAXBrokerPartCol.ColumnName, sValue)
                                            Next
                                            FAXParticipentRow.AppendChild(FAXBrokerParticipentRow)

                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next

                    'Current Table End here
                    'Add into XML Document
                    If RIBand.HasChildNodes Then
                        RIBand.InsertAfter(ArrangementRow, RIBand.FirstChild)
                    Else
                        RIBand.AppendChild(ArrangementRow)
                    End If

                End If
            Next
        End If
    End Sub
    ''' <summary>
    ''' This Method Returns the SPecific Arrangement Line from XML
    ''' </summary>
    ''' <param name="v_iRIArrangementLineKey"></param>
    ''' <param name="v_sXML"></param>
    ''' <param name="v_bIsClaim"></param>
    ''' <returns>ArrangementLinesType</returns>
    ''' <remarks></remarks>
    Public Overrides Function GetRIArrangementLineFromXML(ByVal v_iRIArrangementLineKey As Integer, ByVal v_sXML As String, Optional ByVal v_bIsClaim As Boolean = False, Optional ByVal v_sRIBAND As String = "", Optional ByVal v_sTreatyCode As String = "") As ArrangementLinesType
        'Variable to Hold the returned data
        Dim oArrangementLineType As New ArrangementLinesType
        'Load the XML Data 
        Dim oXMLDoc As New XmlDocument
        oXMLDoc.LoadXml(v_sXML)

        Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("//*[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']")
        ' If not found by key, try by TreatyCode (for new lines with key=0)
        If oNode Is Nothing AndAlso Not String.IsNullOrEmpty(v_sTreatyCode) AndAlso Not String.IsNullOrEmpty(v_sRIBAND) Then
            oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & v_sRIBAND & "']/ArrangementRow[@TreatyCode='" & v_sTreatyCode.Replace("'", "''") & "']")
        End If

        If v_bIsClaim Then
            'For Claim
            If oNode IsNot Nothing Then
                'Populate Arrangement Line Details
                With oArrangementLineType

                    .ActionType = oNode.Attributes("ActionType").Value
                    .AgreementCode = oNode.Attributes("AgreementCode").Value
                    Boolean.TryParse(oNode.Attributes("CedePremiumOnly").Value, .CedePremiumOnly)
                    Integer.TryParse(oNode.Attributes("Grouping").Value, .Grouping)
                    Boolean.TryParse(oNode.Attributes("IsDomiciledForTax").Value, .IsDomiciledForTax)
                    Boolean.TryParse(oNode.Attributes("IsRIBroker").Value, .IsRIBroker)
                    Decimal.TryParse(oNode.Attributes("LineLimit").Value, .LineLimit)
                    Decimal.TryParse(oNode.Attributes("LowerLimit").Value, .LowerLimit)
                    Decimal.TryParse(oNode.Attributes("NumberOfLines").Value, .NumberOfLines)
                    Integer.TryParse(oNode.Attributes("PartyKey").Value, .PartyKey)
                    Integer.TryParse(oNode.Attributes("Priority").Value, .Priority)
                    .ReinsuranceTypeCode = oNode.Attributes("ReinsuranceTypeCode").Value
                    Decimal.TryParse(oNode.Attributes("Retained").Value, .Retained)
                    Integer.TryParse(oNode.Attributes("RIArrangementKey").Value, .RIarrangementKey)
                    Integer.TryParse(oNode.Attributes("RIArrangementLineKey").Value, .RIArrangementLineKey)
                    .RIPlacement = oNode.Attributes("Placement").Value
                    .TreatyCode = oNode.Attributes("TreatyCode").Value
                    .Type = oNode.Attributes("Type").Value
                    Decimal.TryParse(oNode.Attributes("RecoverToDate").Value, .RecoverToDate)
                    Decimal.TryParse(oNode.Attributes("ThisPayment").Value, .ThisPayment)
                    .RIName = oNode.Attributes("Name").Value
                    Decimal.TryParse(oNode.Attributes("DefaultPerc").Value, .DefaultPerc)
                    Decimal.TryParse(oNode.Attributes("ThisPerc").Value, .ThisPerc)
                    Decimal.TryParse(oNode.Attributes("SumInsured").Value, .SumInsured)
                    Decimal.TryParse(oNode.Attributes("ReserveToDate").Value, .ReserveToDate)
                    Decimal.TryParse(oNode.Attributes("PaymentToDate").Value, .PaymentToDate)
                    Decimal.TryParse(oNode.Attributes("ThisReserve").Value, .ThisReserve)
                    Decimal.TryParse(oNode.Attributes("Balance").Value, .Balance)
                    .AgreementCode = oNode.Attributes("Agreement").Value
                    Decimal.TryParse(oNode.Attributes("Incurred").Value, .Incurred)

                    If .Type = "F" Then
                        'Populate Broker Details
                        Dim oBrokerPart As BrokerParticipants
                        Dim oBrokerNodeList As XmlNodeList = oNode.SelectNodes("//BrokerParticipentRow")
                        If oBrokerNodeList IsNot Nothing AndAlso oBrokerNodeList.Count > 0 Then
                            For Each oBrokerPartNode As XmlNode In oBrokerNodeList
                                oBrokerPart = New BrokerParticipants
                                With oBrokerPart
                                    .PartyCode = oBrokerPartNode.Attributes("PartyCode").Value
                                    .PartyName = oBrokerPartNode.Attributes("PartyName").Value
                                    Integer.TryParse(oBrokerPartNode.Attributes("PartyKey").Value, .PartyKey)
                                    Decimal.TryParse(oBrokerPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                End With
                                oArrangementLineType.BrokerParticipants.Add(oBrokerPart)
                            Next
                        End If
                    End If
                    If .Type = "FX" Then
                        'Populate FAX Participents
                        Dim oFAXPart As FAXParticipants
                        Dim oFAXNodeList As XmlNodeList = oNode.SelectNodes("//FAXParticipentRow")
                        If oFAXNodeList IsNot Nothing AndAlso oFAXNodeList.Count > 0 Then
                            For Each oFAXPartNode As XmlNode In oFAXNodeList
                                oFAXPart = New FAXParticipants
                                With oFAXPart

                                    .PartyCode = oFAXPartNode.Attributes("PartyCode").Value
                                    .PartyName = oFAXPartNode.Attributes("PartyName").Value
                                    Integer.TryParse(oFAXPartNode.Attributes("PartyKey").Value, .PartyKey)
                                    Decimal.TryParse(oFAXPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                    .AccountType = oFAXPartNode.Attributes("AccountType").Value
                                    .AgreementCode = oFAXPartNode.Attributes("AgreementCode").Value
                                    Decimal.TryParse(oFAXPartNode.Attributes("SumInsured").Value, .SumInsured)
                                    Decimal.TryParse(oFAXPartNode.Attributes("PaymentToDate").Value, .PaymentToDate)
                                    Decimal.TryParse(oFAXPartNode.Attributes("RecoverToDate").Value, .RecoverToDate)
                                    Decimal.TryParse(oFAXPartNode.Attributes("ReserveToDate").Value, .ReserveToDate)
                                    Decimal.TryParse(oFAXPartNode.Attributes("ThisPayment").Value, .ThisPayment)
                                    Decimal.TryParse(oFAXPartNode.Attributes("ThisReserve").Value, .ThisReserve)
                                    Decimal.TryParse(oFAXPartNode.Attributes("Premium").Value, .PremiumValue)
                                    Decimal.TryParse(oFAXPartNode.Attributes("PremiumTax").Value, .PremiumTax)
                                    .ActionType = oFAXPartNode.Attributes("ActionType").Value
                                    Integer.TryParse(oFAXPartNode.Attributes("RIArrangementLineKey").Value, .RIArrangementLineKey)

                                    'Populate FAX Broker Part
                                    Dim oFAXBrokerPart As BrokerParticipants
                                    Dim oFAXBrokerNodeList As XmlNodeList = oNode.SelectNodes("//FAXBrokerParticipentRow")
                                    If oFAXBrokerNodeList IsNot Nothing AndAlso oFAXBrokerNodeList.Count > 0 Then
                                        For Each oFAXBrokerPartNode As XmlNode In oFAXBrokerNodeList
                                            oFAXBrokerPart = New BrokerParticipants

                                            With oFAXBrokerPart
                                                .PartyCode = oFAXBrokerPartNode.Attributes("PartyCode").Value
                                                .PartyName = oFAXBrokerPartNode.Attributes("PartyName").Value
                                                Integer.TryParse(oFAXBrokerPartNode.Attributes("PartyKey").Value, .PartyKey)
                                                Decimal.TryParse(oFAXBrokerPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                            End With
                                            oFAXPart.BrokerParticipants.Add(oFAXBrokerPart)
                                        Next
                                    End If
                                End With
                                oArrangementLineType.FAXParticipants.Add(oFAXPart)
                            Next
                        End If
                    End If
                End With
            End If
        Else
            'For NB/REN/MTA/MTC/MTR
            oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & v_sRIBAND & "']/ArrangementRow[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']")
            ' Fallback: if not found by key and a treaty code is supplied, locate by TreatyCode
            If oNode Is Nothing AndAlso Not String.IsNullOrEmpty(v_sTreatyCode) Then
                oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & v_sRIBAND & "']/ArrangementRow[@TreatyCode='" & v_sTreatyCode.Replace("'", "''") & "']")
            End If

            If oNode IsNot Nothing Then
                'Populate Arrangement Line Details
                With oArrangementLineType
                    If oNode.Attributes("IsDeleted").Value = "True" Then
                        .ActionType = NexusProvider.RowAction.DeleteRow
                    Else
                        .ActionType = oNode.Attributes("ActionType").Value
                    End If
                    .AgreementCode = oNode.Attributes("Agreement").Value
                    .CedePremiumOnly = oNode.Attributes("CedePremiumOnly").Value
                    Decimal.TryParse(oNode.Attributes("CommissionPerc").Value, .CommissionPerc)
                    Integer.TryParse(oNode.Attributes("Grouping").Value, .Grouping)
                    Boolean.TryParse(oNode.Attributes("IsCommissionModified").Value, .IsCommissionModified)
                    Boolean.TryParse(oNode.Attributes("IsDomiciledForTax").Value, .IsDomiciledForTax)
                    Boolean.TryParse(oNode.Attributes("IsRIBroker").Value, .IsRIBroker)
                    Decimal.TryParse(oNode.Attributes("LineLimit").Value, .LineLimit)
                    Decimal.TryParse(oNode.Attributes("LowerLimit").Value, .LowerLimit)
                    Decimal.TryParse(oNode.Attributes("NumberOfLines").Value, .NumberOfLines)
                    Integer.TryParse(oNode.Attributes("PartyKey").Value, .PartyKey)
                    Decimal.TryParse(oNode.Attributes("PremiumPercent").Value, .PremiumPercent)
                    Integer.TryParse(oNode.Attributes("Priority").Value, .Priority)
                    .ReinsuranceTypeCode = oNode.Attributes("ReinsuranceTypeCode").Value
                    Decimal.TryParse(oNode.Attributes("Retained").Value, .Retained)
                    Integer.TryParse(oNode.Attributes("RIarrangementKey").Value, .RIarrangementKey)
                    Integer.TryParse(oNode.Attributes("RIArrangementLineKey").Value, .RIArrangementLineKey)
                    .RIPlacement = oNode.Attributes("Placement").Value
                    .TreatyCode = oNode.Attributes("TreatyCode").Value
                    .Type = oNode.Attributes("Type").Value
                    .RIName = oNode.Attributes("Name").Value
                    Decimal.TryParse(oNode.Attributes("DefaultPerc").Value, .DefaultPerc)
                    Decimal.TryParse(oNode.Attributes("ThisPerc").Value, .ThisPerc)
                    Decimal.TryParse(oNode.Attributes("SumInsured").Value, .SumInsured)
                    .AgreementCode = oNode.Attributes("Agreement").Value
                    Decimal.TryParse(oNode.Attributes("ParticipationPercent").Value, .ParticipationPercent)
                    Decimal.TryParse(oNode.Attributes("Premium").Value, .PremiumValue)
                    Decimal.TryParse(oNode.Attributes("Tax").Value, .Tax)
                    Decimal.TryParse(oNode.Attributes("Commission").Value, .CommissionValue)
                    Decimal.TryParse(oNode.Attributes("CommissionTax").Value, .CommissionTax)

                    ' PBI 35359 refactor: Read IsEditedDB (DB-persisted flag) from XML for saving to SAM
                    Dim bIsEditedDB As Boolean = False
                    If oNode.Attributes("IsEditedDB") IsNot Nothing Then
                        Boolean.TryParse(oNode.Attributes("IsEditedDB").Value, bIsEditedDB)
                    End If
                    .IsEditedDB = bIsEditedDB

                    ' Read IsPremiumEdited from XML for saving to SAM
                    Dim bIsPremiumEdited As Boolean = False
                    If oNode.Attributes("IsPremiumEdited") IsNot Nothing Then
                        Boolean.TryParse(oNode.Attributes("IsPremiumEdited").Value, bIsPremiumEdited)
                    End If
                    .IsPremiumEdited = bIsPremiumEdited
                    If oNode.Attributes("Type").Value.ToUpper.Trim = "F" Then
                        Dim oBrokerPart As BrokerParticipants
                        Dim oBrokerNodeList As XmlNodeList = oNode.SelectNodes("/rows/RIBAND[@Name='" & v_sRIBAND & "']/ArrangementRow[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']/BrokerParticipentRow")
                        If oBrokerNodeList IsNot Nothing AndAlso oBrokerNodeList.Count > 0 Then
                            For Each oBrokerPartNode As XmlNode In oBrokerNodeList
                                oBrokerPart = New BrokerParticipants
                                With oBrokerPart
                                    .PartyCode = oBrokerPartNode.Attributes("PartyCode").Value
                                    .PartyName = oBrokerPartNode.Attributes("PartyName").Value
                                    Integer.TryParse(oBrokerPartNode.Attributes("PartyKey").Value, .PartyKey)
                                    Decimal.TryParse(oBrokerPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                End With
                                oArrangementLineType.BrokerParticipants.Add(oBrokerPart)
                            Next
                        End If
                    End If
                    If oNode.Attributes("Type").Value.ToUpper.Trim = "FX" Then
                        'Populate FAX Participents
                        Dim oFAXPart As FAXParticipants
                        Dim oFAXNodeList As XmlNodeList = oNode.SelectNodes("/rows/RIBAND[@Name='" & v_sRIBAND & "']/ArrangementRow[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']/FAXParticipentRow")
                        If oFAXNodeList IsNot Nothing AndAlso oFAXNodeList.Count > 0 Then
                            For Each oFAXPartNode As XmlNode In oFAXNodeList
                                oFAXPart = New FAXParticipants
                                With oFAXPart
                                    If oFAXPartNode.Attributes("IsDeleted").Value = "True" Then
                                        .ActionType = NexusProvider.RowAction.DeleteRow
                                    ElseIf oFAXPartNode.Attributes("IsNew").Value = "True" Then
                                        .ActionType = NexusProvider.RowAction.AddRow
                                    Else
                                        .ActionType = NexusProvider.RowAction.EditRow
                                    End If
                                    .PartyCode = oFAXPartNode.Attributes("PartyCode").Value
                                    .PartyName = oFAXPartNode.Attributes("PartyName").Value
                                    Integer.TryParse(oFAXPartNode.Attributes("PartyKey").Value, .PartyKey)
                                    If oFAXPartNode.Attributes("ParticipationPercentage") IsNot Nothing Then
                                        Decimal.TryParse(oFAXPartNode.Attributes("ParticipationPercentage").Value, .ParticipationPercentage)
                                    Else
                                        Decimal.TryParse(oFAXPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                    End If
                                    .AccountType = oFAXPartNode.Attributes("AccountType").Value
                                    .AgreementCode = oFAXPartNode.Attributes("AgreementCode").Value
                                    Decimal.TryParse(oFAXPartNode.Attributes("CommissionPercent").Value, .CommissionPercent)
                                    Decimal.TryParse(oFAXPartNode.Attributes("CommissionTax").Value, .CommissionTax)
                                    Decimal.TryParse(oFAXPartNode.Attributes("CommissionValue").Value, .CommissionValue)
                                    Decimal.TryParse(oFAXPartNode.Attributes("PremiumTax").Value, .PremiumTax)
                                    Decimal.TryParse(oFAXPartNode.Attributes("PremiumValue").Value, .PremiumValue)
                                    Decimal.TryParse(oFAXPartNode.Attributes("SumInsured").Value, .SumInsured)
                                    Integer.TryParse(oFAXPartNode.Attributes("RIArrangementLineKey").Value, .RIArrangementLineKey)

                                    'Populate FAX Broker Part
                                    Dim oFAXBrokerPart As BrokerParticipants
                                    'Dim oFAXBrokerNodeList As XmlNodeList = oFAXPartNode.SelectNodes("//FAXBrokerParticipentRow")
                                    Dim oFAXBrokerNodeList As XmlNodeList = oNode.SelectNodes("/rows/RIBAND[@Name='" & v_sRIBAND & "']/ArrangementRow[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']/FAXParticipentRow[@PartyCode='" & .PartyCode.Trim & "']/FAXBrokerParticipentRow")
                                    If oFAXBrokerNodeList IsNot Nothing AndAlso oFAXBrokerNodeList.Count > 0 Then
                                        For Each oFAXBrokerPartNode As XmlNode In oFAXBrokerNodeList
                                            oFAXBrokerPart = New BrokerParticipants
                                            With oFAXBrokerPart
                                                .PartyCode = oFAXBrokerPartNode.Attributes("PartyCode").Value
                                                .PartyName = oFAXBrokerPartNode.Attributes("PartyName").Value
                                                Integer.TryParse(oFAXBrokerPartNode.Attributes("PartyKey").Value, .PartyKey)
                                                Decimal.TryParse(oFAXBrokerPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                            End With
                                            oFAXPart.BrokerParticipants.Add(oFAXBrokerPart)
                                        Next
                                    End If
                                End With
                                oArrangementLineType.FAXParticipants.Add(oFAXPart)
                            Next
                        End If
                    End If
                End With
            End If
        End If
        'Return the Data
        Return oArrangementLineType
    End Function
    ''' <summary>
    ''' This method checks FAC XOL Validation
    ''' </summary>
    ''' <param name="v_sXML"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function CheckFACXOLLimit(ByVal v_sXML As String) As Boolean
        Dim bValidation As Boolean = True
        'Load the XML Data 
        Dim oXMLDoc As New XmlDocument
        oXMLDoc.LoadXml(v_sXML)

        If oXMLDoc.HasChildNodes Then
            'Find Root Node
            For Each oRootNode As XmlNode In oXMLDoc.ChildNodes
                If oRootNode.HasChildNodes Then

                    If oRootNode.Attributes("Name") IsNot Nothing AndAlso oRootNode.Attributes("Name").Value.Contains("Current") = True Then

                        'Find the Band Total
                        'Calculate/Retreive Band Total
                        Dim dBANDPremium As Decimal
                        Dim oBandNode As XmlNode = oRootNode.SelectSingleNode("/rows/RIBAND[@Name='" & oRootNode.Attributes("Name").Value & "']/ArrangementRow[@Name='Band Total']")
                        If oBandNode IsNot Nothing Then
                            Decimal.TryParse(oBandNode.Attributes("Premium").Value, dBANDPremium)
                        End If

                        'Find the FAC XOL
                        Dim dTotFACXOLPremium As Decimal
                        Dim FACXOLNodes As XmlNodeList = oRootNode.SelectNodes("/rows/RIBAND[@Name='" & oRootNode.Attributes("Name").Value & "']/ArrangementRow[@Placement='FAC XOL']")

                        For Each oNode As XmlNode In FACXOLNodes
                            Dim dFACXOLPremium As Decimal
                            Decimal.TryParse(oNode.Attributes("Premium").Value, dFACXOLPremium)
                            dTotFACXOLPremium = dTotFACXOLPremium + dFACXOLPremium
                        Next

                        'if Total FAC XOL premium is greater than BAND premium
                        If dTotFACXOLPremium > dBANDPremium Then
                            bValidation = False
                            Exit For
                        End If
                    End If
                End If
            Next
        End If

        Return bValidation
    End Function
    ''' <summary>
    ''' this will get the Treaty party tax percentage values
    ''' </summary>
    ''' <param name="v_iRiskKey"></param>
    ''' <param name="v_iPartyKey"></param>
    ''' <param name="v_dPremium"></param>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="r_dTaxPercentage"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub CalculateRITax(ByVal v_iRiskKey As Integer, _
                                           ByVal v_iPartyKey As Integer, _
                                           ByVal v_dPremium As Double, _
                                           ByVal v_iInsuranceFileKey As Integer, _
                                           ByRef r_dTaxPercentage As Double, _
                                           Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oCalculateRITaxRequest As CalculateRITaxRequestType
            Dim oCalculateRITaxResponse As CalculateRITaxResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                sbLogMessage = New StringBuilder
                oCalculateRITaxRequest = New CalculateRITaxRequestType
                oCalculateRITaxResponse = New CalculateRITaxResponseType

                With oCalculateRITaxRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_iRiskKey > 0 Then
                        .RiskKey = v_iRiskKey
                    Else
                        Throw New ArgumentNullException("v_iRiskKey Not Available")
                    End If

                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                    Else
                        Throw New ArgumentNullException("v_iInsuranceFileKey Not Available")
                    End If

                    .PartyKey = v_iPartyKey
                    .Premium = v_dPremium
                    .Commission = 0

                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oCalculateRITaxResponse = oSAM.CalculateRITax(oCalculateRITaxRequest)
                End Using

                With oCalculateRITaxResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                    If (.PremiumTax <> 0) Then
                        r_dTaxPercentage = (Convert.ToDouble(.PremiumTax) / Convert.ToDouble(v_dPremium)) * 100
                    Else
                        r_dTaxPercentage = 0
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oCalculateRITaxRequest = Nothing
                oCalculateRITaxResponse = Nothing
            End Try
        End SyncLock
    End Sub

    Public Overrides Sub UpdateRIArrangementOverrideReason(ByVal v_iRIArrangementId As Integer, ByVal v_iOverrideReasonId As Integer)
        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oRequest As UpdateRiOverrideReasonInRiArrangementRequestType
            Dim oResponse As UpdateRiOverrideReasonInRiArrangementResponseType

            Try
                oSAM = InitializeServiceMethod()
                oRequest = New UpdateRiOverrideReasonInRiArrangementRequestType
                oResponse = New UpdateRiOverrideReasonInRiArrangementResponseType

                With oRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    .RiArrangementId = v_iRIArrangementId
                    .RiOverrideReasonId = v_iOverrideReasonId
                End With

                Using trace As New Tracer(Category.Trace)
                    oResponse = oSAM.UpdateRiOverrideReasonInRiArrangement(oRequest)
                End Using

                With oResponse
                    If .Errors IsNot Nothing Then
                        Throw New NexusException(.Errors)
                    End If
                End With

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oRequest = Nothing
                oResponse = Nothing
            End Try
        End SyncLock
    End Sub

End Class
