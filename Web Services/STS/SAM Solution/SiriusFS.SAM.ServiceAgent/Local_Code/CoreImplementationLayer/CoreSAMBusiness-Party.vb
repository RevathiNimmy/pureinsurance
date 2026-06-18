Option Strict On

Imports SiriusFS.SAM.Structure.STSErrorPublisher
Imports SiriusFS.SAM.Structure
Imports Microsoft.ApplicationBlocks.ExceptionManagement
Imports Microsoft.ApplicationBlocks.Data
Imports System.Net.Mail
Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports SiriusFS.SAM.Structure.SAMConstants
Imports dPMDAOBridge

Partial Public Class CoreSAMBusiness

    Public Overloads Function AddParty(ByVal AddPartyRequest As BaseAddPartyRequestType) As BaseAddPartyResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                            _SiriusUser.Username, _SiriusUser.SourceID, _
                                            _SiriusUser.LanguageID, _
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseAddPartyResponseType

            Try
                con.BeginTransaction()
                oResponse = AddParty(con, AddPartyRequest)
                con.CommitTransaction()
                If oResponse.XMLDataset IsNot Nothing Then
                    Dim PartyXBuilder As New CoreImplementation.XBuilder.Party(_SiriusUser)
                    PartyXBuilder.GetDataSet(AddPartyRequest.BranchCode, oResponse.PartyKey, oResponse.XMLDataset)
                End If
                AddPartyHistory(con, oResponse.PartyKey, oResponse.XMLDataset)
            Catch
                con.RollbackTransaction()

                Throw
            End Try

            Return oResponse

        End Using

    End Function
    ''' <summary>
    ''' Add Party History
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="v_nPartyKey"></param>
    ''' <param name="v_sXMLDataset"></param>
    ''' <remarks></remarks>
    Private Sub AddPartyHistory(ByVal con As SiriusConnection, ByVal v_nPartyKey As Integer, ByVal v_sXMLDataset As String)

        Dim oBSirPartyBusiness As bSIRParty.Business = Nothing
        Try
            oBSirPartyBusiness = New bSIRParty.Business
            InitialiseSBOObject(con, oBSirPartyBusiness, _SiriusUser, "bSIRParty.Business")
            oBSirPartyBusiness.AddPartyHistory(v_nPartyKey, v_sXMLDataset)
        Catch
            Throw
        End Try
    End Sub
    ' ***************************************************************** '
    ' Name: AddParty
    '
    ' Description: This method accepts the base party implementation type 
    '              and adds a new party record.
    '
    ' ***************************************************************** '
    Public Overloads Function AddParty(ByVal con As SiriusConnection, ByVal AddPartyRequest As BaseAddPartyRequestType) As BaseAddPartyResponseType

        Const ACMethodName As String = "AddParty"

        'Declare the Response object 
        Dim oResponse As New BaseImplementationTypes.BaseAddPartyResponseType

        Dim iRet As System.Int32
        Dim sPartyType As String = String.Empty
        Dim vAddresses As Object = Nothing
        Dim vContacts As Object = Nothing
        Dim bIsLocked As Boolean
        Dim iAgentCnt As Integer = -1

        Dim sEmployersBusiness As String = String.Empty
        Dim sOccupation As String = String.Empty
        Dim sBusiness As String = String.Empty
        Dim r_oConvictions(,) As Object = Nothing
        Dim r_oAccidents(,) As Object = Nothing
        Dim r_oOtherPartyInfo(26) As Object
        Dim r_oSupplierBusiness(,) As Object = Nothing
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim sSurname As String = String.Empty

        Dim sEnhancedResolvedName As String = String.Empty

        Dim STSError As New STSErrorPublisher

        Dim samForBrokingRequest As SAMForBrokingImplementationTypes.AddPartyRequestType = Nothing

        ' Check that some input has been passed
        If (AddPartyRequest Is Nothing) = True Then
            Return Nothing
        End If

        Dim nTypeOfPackage As enumTypeOfPackage

        If AddPartyRequest.GetType Is GetType(AgentImplementationTypes.AddPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oResponse = New AgentImplementationTypes.AddPartyResponseType
        ElseIf AddPartyRequest.GetType Is GetType(BaseImplementationTypes.BaseAddPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            oResponse = New BaseImplementationTypes.BaseAddPartyResponseType
        ElseIf AddPartyRequest.GetType Is GetType(CustomerImplementationTypes.AddPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.CustomersPackage
            oResponse = New CustomerImplementationTypes.AddPartyResponseType
        ElseIf AddPartyRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.AddPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.AddPartyResponseType
        ElseIf AddPartyRequest.GetType Is GetType(SAMForBrokingImplementationTypes.AddPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage
            oResponse = New SAMForBrokingImplementationTypes.AddPartyResponseType
            samForBrokingRequest = DirectCast(AddPartyRequest, SAMForBrokingImplementationTypes.AddPartyRequestType)
        ElseIf AddPartyRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.AddPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.AddPartyResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        If nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage Then
            If CoreBusiness.PartyIsAgent(samForBrokingRequest.UserPartyType) Then
                If oCoreBusiness.CheckAgentKey(samForBrokingRequest.UserPartyKey) = False Then
                    STSError = New STSErrorPublisher(STSErrorCodes.AgentRecordNotFound, "Agent Key validation failed", "The Agent record does not exist for key: " & samForBrokingRequest.UserPartyKey)
                    STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Key Validation", True)
                    Return oResponse
                End If

                If oCoreBusiness.AgentSecurityCheck(samForBrokingRequest.UserName, samForBrokingRequest.SourceId, PMEEntityType.Source) = False Then
                    STSError = New STSErrorPublisher(STSErrorCodes.SecurityCheckFailed, "Security Check Failed", samForBrokingRequest.UserName & " does not have permission to access source " & samForBrokingRequest.SourceId)
                    STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "AddParty", True)
                    Return oResponse
                End If
            End If
        End If

        ' Check that mandatory base request fields have been provided
        '  Party BranchCode
        If ((AddPartyRequest.BranchCode = "") And (AddPartyRequest.Party.BranchCode = "")) Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        ElseIf AddPartyRequest.BranchCode = "" Then
            AddPartyRequest.BranchCode = AddPartyRequest.Party.BranchCode
        End If

        If AddPartyRequest.Party.BranchCode = "" Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        ' Check if the address structure was passed in
        If Not IsArray(AddPartyRequest.Party.Addresses) Then
            STSError.AddInvalidField("Address", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Address"), "")
        Else
        End If

        Dim oBasePartyPC As New BaseImplementationTypes.BasePartyPCType
        Dim oBasePartyCC As New BaseImplementationTypes.BasePartyCCType
        Dim oBasePartyOther As New BaseImplementationTypes.BasePartyOTHERType
        Dim bOtherParty As Boolean = False
        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection

        ' Determine the Party Type
        If AddPartyRequest.Party.GetType Is GetType(BaseImplementationTypes.BasePartyPCType) Then
            sPartyType = "PC"
            oBasePartyPC = DirectCast(AddPartyRequest.Party, BaseImplementationTypes.BasePartyPCType)

            If AddPartyRequest.BranchCode = "" Then AddPartyRequest.BranchCode = oBasePartyPC.BranchCode

            ' Check that mandatory PC request fields have been provided
            If oBasePartyPC.Forename = "" Then
                STSError.AddInvalidField("Forename", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Forename"), "")
            End If
            If oBasePartyPC.Surname = "" Then
                STSError.AddInvalidField("Surname", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Surname"), "")
            End If
            If oBasePartyPC.Title = "" Then
                STSError.AddInvalidField("Title", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Title"), "")
            End If
            If oBasePartyPC.Initials = "" Then
                STSError.AddInvalidField("Initials", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Initials"), "")
            End If

            If oBasePartyPC.Currency = "" Then
                STSError.AddInvalidField("Currency", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Currency"), "")
            End If

 	    If oBasePartyPC.Lifestyle IsNot Nothing Then
                For Each oLifeStyle As BasePartyPCTypeLifestyle In oBasePartyPC.Lifestyle
                    If oLifeStyle.LifestyleKey <> 0 AndAlso String.IsNullOrEmpty(oLifeStyle.CategoryCode) Then
                        STSError.AddInvalidField("CategoryCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "CategoryCode"), "")
                    End If
                Next
            End If

            ' If GenderCode and DateOfBirth are mandatory for anonymous
            ' they will have been checked before here, in the anonymous business

            oBasePartyPC.Validate(CType(oSAMErrorCollection, Object))
            ' if there were any errors throw an exception
            oSAMErrorCollection.CheckForErrors()

        ElseIf AddPartyRequest.Party.GetType Is GetType(BaseImplementationTypes.BasePartyCCType) Then
            sPartyType = "CC"
            oBasePartyCC = DirectCast(AddPartyRequest.Party, BaseImplementationTypes.BasePartyCCType)
            If AddPartyRequest.BranchCode = "" Then AddPartyRequest.BranchCode = oBasePartyCC.BranchCode
            ' Check that mandatory CC request fields have been provided
            If oBasePartyCC.CompanyName = "" Then
                STSError.AddInvalidField("CompanyName", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "CompanyName"), "")
            End If

            oBasePartyCC.Validate(CType(oSAMErrorCollection, Object))
            ' if there were any errors throw an exception
            oSAMErrorCollection.CheckForErrors()

        ElseIf AddPartyRequest.Party.GetType Is GetType(BaseImplementationTypes.BasePartyOTHERType) Then
            oBasePartyOther = DirectCast(AddPartyRequest.Party, BaseImplementationTypes.BasePartyOTHERType)

            sPartyType = oBasePartyOther.TypeCode
            bOtherParty = True
            oBasePartyOther.Validate(CType(oSAMErrorCollection, Object))
            ' if there were any errors throw an exception
            oSAMErrorCollection.CheckForErrors()
        End If

        If AddPartyRequest.BranchCode = "" Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        ' Lookup Codes to ensure they are valid
        Try
            Dim iSourceId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", AddPartyRequest.BranchCode)
        Catch ex As Exception
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BranchCode"), AddPartyRequest.BranchCode)
        End Try

        If AddPartyRequest.SubBranchCode <> "" Then
            Try
                Dim iSourceId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", AddPartyRequest.SubBranchCode)
            Catch ex As Exception
                STSError.AddInvalidField("SubBranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "SubBranchCode"), AddPartyRequest.SubBranchCode)
            End Try
        End If

        Dim lAccountExecutiveID, lCurrencyId, lBranchId, lNoOfEmployees, lSubBranchId As Integer
        Dim oUpdatePartyAddDetails As New Hashtable
        Dim sAlternativeIdentifier As String = Nothing

        If sPartyType = "PC" Then

            ' Check Account Executive Code
            If Not (String.IsNullOrEmpty(oBasePartyPC.AccountExecutiveCode)) Then
                Try
                    lAccountExecutiveID = GetAndValidateSpecifiedTableCode(con, "party", "party_cnt", "shortname", oBasePartyPC.AccountExecutiveCode, oSAMErrorCollection, "AccountExecutiveCode")
                Catch ex As Exception
                    STSError.AddInvalidField("AccountExecutiveCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "AccountExecutiveCode"), oBasePartyPC.AccountExecutiveCode)
                End Try
            End If
            ' Check currency
            If Not (String.IsNullOrEmpty(oBasePartyPC.Currency)) Then
                Try
                    lCurrencyId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "currency", oBasePartyPC.Currency, "currency_id", oSAMErrorCollection)
                Catch ex As Exception
                    STSError.AddInvalidField("currency", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "currency"), oBasePartyPC.Currency)
                End Try
            End If
            ' Check Source
            If Not (String.IsNullOrEmpty(oBasePartyPC.BranchCode)) Then
                Try
                    lBranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "source", oBasePartyPC.BranchCode, "source_id", oSAMErrorCollection)
                Catch ex As Exception
                    STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BranchCode"), oBasePartyPC.Source)
                End Try
            End If
            ' Check SubBranchCode
            If Not (String.IsNullOrEmpty(oBasePartyPC.SubBranchCode)) Then
                Try
                    lSubBranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "sub_branch", oBasePartyPC.SubBranchCode, "sub_branch_id", oSAMErrorCollection)
                Catch ex As Exception
                    STSError.AddInvalidField("SubBranchCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "SubBranchCode"), oBasePartyPC.SubBranchCode)
                End Try
            End If

            ' Check Occupation Code
            If oBasePartyPC.OccupationCode <> "" Then
                Try
                    '20080525
                    'Dim iOccupationID As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.UserDefinedTable, "2228226", oBasePartyPC.OccupationCode, sOccupation)
                    Dim strOccupationCode As Object = GetListCode(Of String)(Core.STSListType.UserDefinedTable, "2228226", _
                       oBasePartyPC.OccupationCode, "caption", True) ' 2706

                    sOccupation = oBasePartyPC.OccupationCode

                Catch ex As Exception
                    STSError.AddInvalidField("OccupationCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "OccupationCode"), oBasePartyPC.OccupationCode)
                End Try
            End If
            ' Check Employers Business Code
            If oBasePartyPC.EmployersBusinessCode <> "" Then
                Try

                    'Dim iEmployerBusinessID As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.UserDefinedTable, "2228228", oBasePartyPC.EmployersBusinessCode, sEmployersBusiness)
                    Dim strEmployersBusinessCode As Object = GetListCode(Of String)(Core.STSListType.UserDefinedTable, "2228228", _
                            oBasePartyPC.EmployersBusinessCode, "caption", True)

                Catch ex As Exception
                    STSError.AddInvalidField("EmployersBusinessCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "EmployersBusinessCode"), oBasePartyPC.EmployersBusinessCode)
                End Try
            End If
            ' Check Title

            If oBasePartyPC.Title <> "" Then

                Try
                    Dim iTitleId As Integer = oCoreBusiness.GetListItemFromCaption(Core.STSListType.UserDefinedTable, "131085", oBasePartyPC.Title)
                Catch ex As Exception
                    STSError.AddInvalidField("Title", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "Title"), oBasePartyPC.Title)
                End Try

            End If

            ' Check Gender
            If oBasePartyPC.GenderCode <> "" Then
                Try
                    Dim iGenderId As Integer = oCoreBusiness.GetListItemFromCaption(Core.STSListType.UserDefinedTable, "131091", oBasePartyPC.GenderCode)
                Catch ex As Exception
                    STSError.AddInvalidField("GenderCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "GenderCode"), oBasePartyPC.GenderCode)
                End Try
            End If

            ValidatePartyPCTypeLookup(oBasePartyPC, oCoreBusiness, STSError)

            oSAMErrorCollection.CheckForErrors()

            oUpdatePartyAddDetails.Add("consultant_cnt", lAccountExecutiveID)
            oUpdatePartyAddDetails.Add("currency_id", lCurrencyId)
            oUpdatePartyAddDetails.Add("source_id", lBranchId)
            oUpdatePartyAddDetails.Add("sub_branch_id", lSubBranchId)
            sAlternativeIdentifier = oBasePartyPC.AlternativeId

        Else
            ' Check Business Code
            If oBasePartyCC.BusinessCode <> "" Then
                Try
                    '20080525
                    'Dim iBusinessID As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.UserDefinedTable, "2228228", oBasePartyCC.BusinessCode, sBusiness)
                    Dim strBusinessCode As Object = GetListCode(Of String)(Core.STSListType.UserDefinedTable, "2228228", _
                            oBasePartyCC.BusinessCode, "caption", True)

                    sBusiness = oBasePartyCC.BusinessCode

                Catch ex As Exception
                    STSError.AddInvalidField("BusinessCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BusinessCode"), oBasePartyCC.BusinessCode)
                End Try
            End If

            If sPartyType = "CC" Then
                ValidatePartyCCTypeLookup(oBasePartyCC, oCoreBusiness, STSError)

                If Not (String.IsNullOrEmpty(oBasePartyCC.AccountExecutiveCode)) Then
                    Try
                        lAccountExecutiveID = GetAndValidateSpecifiedTableCode(con, "party", "party_cnt", "shortname", oBasePartyCC.AccountExecutiveCode, oSAMErrorCollection, "AccountExecutiveCode")
                    Catch ex As Exception
                        STSError.AddInvalidField("AccountExecutiveCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "AccountExecutiveCode"), oBasePartyCC.AccountExecutiveCode)
                    End Try
                End If
                ' Check currency
                If Not (String.IsNullOrEmpty(oBasePartyCC.Currency)) Then
                    Try
                        lCurrencyId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "currency", oBasePartyCC.Currency, "currency_id", oSAMErrorCollection)
                    Catch ex As Exception
                        STSError.AddInvalidField("currency", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "currency"), oBasePartyCC.Currency)
                    End Try
                End If
                ' Check Source
                If Not (String.IsNullOrEmpty(oBasePartyCC.BranchCode)) Then
                    Try
                        lBranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "source", oBasePartyCC.BranchCode, "source_id", oSAMErrorCollection)
                    Catch ex As Exception
                        STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BranchCode"), oBasePartyCC.Source)
                    End Try
                End If
                ' Check SubBranchCode
                If Not (String.IsNullOrEmpty(oBasePartyCC.SubBranchCode)) Then
                    Try
                        lSubBranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "sub_branch", oBasePartyCC.SubBranchCode, "sub_branch_id", oSAMErrorCollection)
                    Catch ex As Exception
                        STSError.AddInvalidField("SubBranchCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "SubBranchCode"), oBasePartyCC.SubBranchCode)
                    End Try
                End If
                If nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then
                    If Not (String.IsNullOrEmpty(oBasePartyCC.NumberOfEmployees)) Then
                        Try
                            lNoOfEmployees = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "EmployeeBand", oBasePartyCC.NumberOfEmployees, "employeeband_id", oSAMErrorCollection)
                        Catch ex As Exception
                            STSError.AddInvalidField("NumberOfEmployees", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "NumberOfEmployees"), oBasePartyCC.NumberOfEmployees)
                        End Try
                    End If
                End If
                oSAMErrorCollection.CheckForErrors()
                oUpdatePartyAddDetails.Add("consultant_cnt", lAccountExecutiveID)
                oUpdatePartyAddDetails.Add("currency_id", lCurrencyId)
                oUpdatePartyAddDetails.Add("employeeband_id", lNoOfEmployees)
                oUpdatePartyAddDetails.Add("source_id", lBranchId)
                oUpdatePartyAddDetails.Add("sub_branch_id", lSubBranchId)
                sAlternativeIdentifier = oBasePartyCC.AlternativeId

            ElseIf sPartyType.StartsWith("OT") Then
                ValidatePartyOtherTypeLookup(oBasePartyOther, oCoreBusiness, STSError)
            End If

            End If

        ' exit if there are any invalid parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Lookup field validation", True)
            Return oResponse
        End If

        ' Ensure DOB is within range, if supplied
        If sPartyType = "PC" Then
            If oBasePartyPC.DateOfBirth <> New Date Then
                ' Check the Date of Birth is valid
                If oBasePartyPC.DateOfBirth > Today Then
                    Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.PartyDOBIsInFuture, "Date of Birth is in the future", oBasePartyPC.DateOfBirth.ToString)
                    STSErrorEX.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Date of Birth is in the future", True)
                    Return oResponse
                End If
                If oBasePartyPC.DateOfBirth.AddYears(120) < Today Then
                    Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.PartyDOBIsTooOld, "Date of Birth is over 120 years ago", oBasePartyPC.DateOfBirth.ToString)
                    STSErrorEX.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Date of Birth is over 120 years ago", True)
                    Return oResponse
                End If
            End If
        End If

        ' Ensure there is a correspondence address
        Dim bFound As Boolean = False
        For iCnt As Integer = 0 To AddPartyRequest.Party.Addresses.GetUpperBound(0)
            If AddPartyRequest.Party.Addresses(iCnt).AddressTypeCode = AddressTypeType.Correspondence Then
                bFound = True
                Exit For
            End If
        Next
        If Not bFound Then
            Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.AddressRecordNotFound, "No correspondence address was provided", "")
            STSErrorEX.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "No correspondence address was provided", True)
            Return oResponse
        End If

        ' If the request has come through the Agents interface then make sure we have the AgentKey
        If AddPartyRequest.GetType Is GetType(AgentImplementationTypes.AddPartyRequestType) Then
            Dim oAgentAddPartyRequest As AgentImplementationTypes.AddPartyRequestType = DirectCast(AddPartyRequest, AgentImplementationTypes.AddPartyRequestType)
            iAgentCnt = CInt(oAgentAddPartyRequest.AgentKey)
        ElseIf AddPartyRequest.GetType Is GetType(CustomerImplementationTypes.AddPartyRequestType) Then
            Dim oCustAddPartyRequest As CustomerImplementationTypes.AddPartyRequestType = DirectCast(AddPartyRequest, CustomerImplementationTypes.AddPartyRequestType)
            iAgentCnt = CInt(oCustAddPartyRequest.AgentKey)
        ElseIf AddPartyRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.AddPartyRequestType) Then
            Dim oCustAddPartyRequest As SAMForInsuranceImplementationTypes.AddPartyRequestType = DirectCast(AddPartyRequest, SAMForInsuranceImplementationTypes.AddPartyRequestType)
            iAgentCnt = CInt(oCustAddPartyRequest.AgentKey)
        ElseIf AddPartyRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.AddPartyRequestType) Then
            Dim oCustAddPartyRequest As SAMForInsuranceV2ImplementationTypes.AddPartyRequestType = DirectCast(AddPartyRequest, SAMForInsuranceV2ImplementationTypes.AddPartyRequestType)
            iAgentCnt = CInt(oCustAddPartyRequest.AgentKey)
        ElseIf AddPartyRequest.GetType Is GetType(BaseImplementationTypes.BaseAddPartyRequestType) Then
            iAgentCnt = AddPartyRequest.AgentKey
        End If

        ' Move all addresses to a 2D array
        STSError = ProcessAddresses(AddPartyRequest.Party.Addresses, vAddresses, con)
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        Dim vSingleContacts As Object = Nothing
        Dim vSingleContactsArray As Object(,) = Nothing
        Dim vAddContacts As Object(,) = Nothing
        Dim existUBound As Integer

        If AddPartyRequest.Party.Addresses.GetType Is GetType(BaseAddressWithContactsType()) Then
            If AddPartyRequest.Party.Addresses IsNot Nothing Then
                For addCNT As Integer = AddPartyRequest.Party.Addresses.GetLowerBound(0) To _
                                            AddPartyRequest.Party.Addresses.GetUpperBound(0)
                    Dim oAddressWithContact As BaseAddressWithContactsType
                    oAddressWithContact = DirectCast(AddPartyRequest.Party.Addresses(addCNT), BaseAddressWithContactsType)
                    If oAddressWithContact.Contacts IsNot Nothing Then

                        STSError = ProcessContacts(oAddressWithContact.Contacts, vSingleContacts, addCNT + 1)

                        vSingleContactsArray = DirectCast(vSingleContacts, Object(,))

                        If vAddContacts Is Nothing Then

                            existUBound = 0
                            ReDim Preserve vAddContacts(4, UBound(vSingleContactsArray) - 1)
                            For CountFor As Integer = 0 To UBound(vSingleContactsArray, 2)
                                vAddContacts(0, CountFor) = vSingleContactsArray(0, CountFor)
                                vAddContacts(1, CountFor) = vSingleContactsArray(1, CountFor)
                                vAddContacts(2, CountFor) = vSingleContactsArray(2, CountFor)
                                vAddContacts(3, CountFor) = vSingleContactsArray(3, CountFor)
                                vAddContacts(4, CountFor) = vSingleContactsArray(4, CountFor)
                            Next

                            'Array.ConstrainedCopy(vSingleContacts, 0, vAddContacts, existUBound, UBound(vSingleContacts, 2) * 5)
                        Else

                            existUBound = UBound(vAddContacts)
                            ReDim Preserve vAddContacts(4, UBound(vSingleContactsArray))
                            For CountFor As Integer = 0 To UBound(vSingleContactsArray, 2)
                                vAddContacts(0, existUBound + CountFor) = vSingleContactsArray(0, CountFor)
                                vAddContacts(1, existUBound + CountFor) = vSingleContactsArray(1, CountFor)
                                vAddContacts(2, existUBound + CountFor) = vSingleContactsArray(2, CountFor)
                                vAddContacts(3, existUBound + CountFor) = vSingleContactsArray(3, CountFor)
                                vAddContacts(4, existUBound + CountFor) = vSingleContactsArray(4, CountFor)
                            Next
                            'Array.ConstrainedCopy(vSingleContacts, 0, vAddContacts, 0, (UBound(vSingleContacts, 2) + 1) * 5)
                        End If
                        'Array.ConvertAll(vAddContacts, vSingleContacts)
                        ''Array.Resize(vAddContacts.Rank, existUBound + UBound(vSingleContacts) + 1)

                        If STSError.HasErrors Then
                            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
                            Return oResponse
                        End If
                    End If
                Next
            End If
        End If

        ' Move all contacts to a 2D array
        If AddPartyRequest.Party.Contacts IsNot Nothing Then
            STSError = ProcessContacts(AddPartyRequest.Party.Contacts, vContacts)
            If STSError.HasErrors Then
                STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
                Return oResponse
            End If
        End If

        If bOtherParty = True Then

            ' Vivek: differing from Tech Spec
            If nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then
                ' Vivek: we should not call it here as discussed with Gaurav
                ' we should handle the convictions using new SPs if its SAMForInsuranceV2Package
                ' so no call to ProcessConvictions here.

                'If oBasePartyOther.Convictions IsNot Nothing Then
                '    ProcessConvictions(oBasePartyOther.Convictions, _
                '                        r_oConvictions)

            Else
                If oBasePartyOther.Conviction IsNot Nothing Then
                    ProcessConvictions(oBasePartyOther.Conviction, _
                                        r_oConvictions)
                End If
            End If

            If oBasePartyOther.Accident IsNot Nothing Then
                ProcessAccidents(oBasePartyOther.Accident, _
                                    r_oAccidents)
            End If

            If oBasePartyOther.SupplierBusiness IsNot Nothing Then
                ProcessSupplierBusiness(oBasePartyOther.SupplierBusiness, r_oSupplierBusiness)
            End If

            ProcessOtherPartyInfo(oBasePartyOther, _
                                r_oOtherPartyInfo)

        End If

        '---------------------------------------------------
        ' TODO SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Creating AddParty performance counter")

        ' TODO Dim oCounters As New SiriusPerfCounters("AddParty")

        '' Set up the extra data needed to transact under a broking system
        'Dim oData As AdditionalData

        'Dim ErrEx As Exception = Nothing

        ' TODO SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Entering AddParty")
        ' TODO SAMFunc.STSLogMessageIndent(True)

        Dim oGIS As bGIS.STS = Nothing
        Try
            oGIS = New bGIS.STS
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.FailedToInitialiseBackofficeComponent, "Failed to initialise bGIS.STS", ex.Message)
            STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "oGIS.Initialise", True)
            Return oResponse
        End Try

        ' TODO SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Initialising bGIS.QuotePolicy")

        ' Initialise the GIS
        SAMFunc.InitialiseGISSTS(Con:=con, oGIS:=oGIS, SiriusUser:=_SiriusUser)

        ' Call the Method
        Try
            ' TODO SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Calling oGIS.AddParty")
            ' NB - trading name is or Personal Clients and currently we are not passed this
            ' throught the web service
            oResponse.Shortname = String.Empty

            If sPartyType = "PC" Then
                sSurname = oBasePartyPC.Surname
            ElseIf sPartyType.StartsWith("OT") Then
                sSurname = oBasePartyOther.Name
            End If

            Dim sMaritalStatusCode As String = Nothing
            If oBasePartyPC.MaritalStatusCodeSpecified = True Then

                If (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.Divorced) Then
                    sMaritalStatusCode = "Divorced"
                ElseIf (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.Married) Then
                    sMaritalStatusCode = "Married"

                ElseIf (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.MarriedCommonLaw) Then
                    sMaritalStatusCode = "Married - Common Law"
                ElseIf (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.NotApplicable) Then
                    sMaritalStatusCode = "Not Applicable"
                ElseIf (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.NotAvailable) Then
                    sMaritalStatusCode = "Not Available"

                ElseIf (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.Partnered) Then
                    sMaritalStatusCode = "Partnered"
                ElseIf (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.Separated) Then
                    sMaritalStatusCode = "Separated"
                ElseIf (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.Single_) Then
                    sMaritalStatusCode = "Single"
                ElseIf (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.Widowed) Then
                    sMaritalStatusCode = "Widowed"
                End If
            Else
                sMaritalStatusCode = Nothing
            End If

            iRet = CType(oGIS.AddParty( _
                    sPartyTypeCode:=sPartyType, _
                    sBranchCode:=AddPartyRequest.BranchCode, _
                    vAddresses:=DirectCast(vAddresses, Object(,)), _
                    sTPUserCode:=AddPartyRequest.Party.TPUserCode, _
                    sTPIntroducer:=AddPartyRequest.Party.TPIntroducer, _
                    vContacts:=DirectCast(vContacts, Object(,)), _
                    sSurname:=sSurname, _
                    sForename:=oBasePartyPC.Forename, _
                    sDateOfBirth:=CType(oBasePartyPC.DateOfBirth, String), _
                    sTitle:=oBasePartyPC.Title, _
                    sMaritalStatusCode:=sMaritalStatusCode, _
                    sGenderCode:=oBasePartyPC.GenderCode, _
                    sInitials:=oBasePartyPC.Initials, _
                    sOccupationCode:=sOccupation, _
                    sEmployerBusinessCode:=sEmployersBusiness, _
                    sEmploymentStatusCode:=oBasePartyPC.EmploymentStatusCode.ToString, _
                    sAlternativeID:=sAlternativeIdentifier, _
                    sCompanyName:=oBasePartyCC.CompanyName, _
                    sTradingName:="", _
                    sBusinessCode:=sBusiness, _
                    lAgentCnt:=iAgentCnt, _
                    r_lPartyCnt:=oResponse.PartyKey, _
                    r_sPartyCode:=oResponse.Shortname, _
                    sMainContact:=oBasePartyCC.MainContact, _
                    r_oConvictions:=r_oConvictions, _
                    r_oAccidents:=r_oAccidents, _
                    vAddressContacts:=vAddContacts, _
                    r_oSuppBusiness:=r_oSupplierBusiness, _
                    r_oOtherPartyInfo:=r_oOtherPartyInfo, _
                    r_sTaxNumber:=AddPartyRequest.Party.TaxNumber, _
                    r_bDomiciledForTax:=BooleanDataConvert.ToInt16(AddPartyRequest.Party.DomiciledForTax), _
                    r_bTaxExempt:=BooleanDataConvert.ToInt16(AddPartyRequest.Party.TaxExempt), _
                    r_lPercentage:=CInt(AddPartyRequest.Party.TaxPercentage), _
                    v_sFileCode:=AddPartyRequest.Party.FileCode), Integer)

        Catch ex As Exception
            'oCounters.FailMethod()
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            Dim STSErrorLocal As New STSErrorPublisher("bGIS.STS.AddParty failed", ex)
            STSErrorLocal.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "AddPartyException", True)
        End Try

        If (iRet <> PMEReturnCode.PMTrue) Then
            'oCounters.FailMethod()
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            Dim STSErrorLocal As New STSErrorPublisher(iRet, "bGIS.STS.AddParty failed")
            STSErrorLocal.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "AddPartyReturn", True)
        End If

        If oGIS IsNot Nothing Then
            oGIS.Dispose()
            oGIS = Nothing
        End If
        '----------------------------------------------------------

        ' Return the Resolved name

        If sPartyType = "PC" Then

            oCoreBusiness.GetSystemOption(AddPartyRequest.BranchCode, SystemOption.EnhancedResolvedName, sEnhancedResolvedName)

            If sEnhancedResolvedName = "1" Then
                oResponse.ResolvedName = Trim( _
                    oBasePartyPC.Title & " " & _
                    oBasePartyPC.Forename & " " & _
                    oBasePartyPC.Surname)
            Else
                oResponse.ResolvedName = Trim( _
                    oBasePartyPC.Title & " " & _
                    oBasePartyPC.Initials & " " & _
                    oBasePartyPC.Surname)
            End If
        Else
            oResponse.ResolvedName = oBasePartyCC.CompanyName
        End If

        '***********************************************************************
        ' process risk data
        If nTypeOfPackage = enumTypeOfPackage.UnknownPackage OrElse _
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage OrElse _
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then

            Dim PartyXBuilder As New CoreImplementation.XBuilder.Party(_SiriusUser)
            PartyXBuilder.AddDataSet(con, AddPartyRequest.BranchCode, oResponse.PartyKey, AddPartyRequest.Party.XMLDataset)
            oResponse.XMLDataset = AddPartyRequest.Party.XMLDataset
        End If
        '***********************************************************************

        ' Get the Timestamp
        Dim AnyError As STSErrorType
        AnyError = oCoreBusiness.GetTimestamp(con, _
                            AddPartyRequest.BranchCode, _
                            CoreBusiness.LockName.PartyCnt, _
                            oResponse.PartyKey, _
                            oResponse.PartyTimestamp, _
                            bIsLocked)
        ' Return AnyErrors
        If AnyError Is Nothing = False Then
            oResponse.STSError = AnyError
        End If

        If nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
            Dim oResponseAgent As New AgentImplementationTypes.AddPartyResponseType
            oResponseAgent = DirectCast(oResponse, AgentImplementationTypes.AddPartyResponseType)
            oResponse = oResponseAgent
        End If

        'If nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then
        If sPartyType = "PC" Then

            ' update data specific to PC type
            UpdatePartyPCDetails(con, oCoreBusiness, oBasePartyPC, oResponse.PartyKey)

            ProcessLifeStyle(con, oCoreBusiness, oBasePartyPC.Lifestyle, oResponse.PartyKey)

            If oBasePartyPC.ClientDetail IsNot Nothing Then

                'update contents of ClientSharedDetails

                'UpdatePartyDetails(con, oCoreBusiness, oBasePartyPC.ClientDetail, oResponse.PartyKey)
                UpdatePartyDetails(con, oCoreBusiness, oBasePartyPC.ClientDetail, oResponse.PartyKey, oUpdatePartyAddDetails)

                ProcessProspect(con, oCoreBusiness, oBasePartyPC.ClientDetail, oResponse.PartyKey)
                ProcessAssociates(con, oCoreBusiness, oBasePartyPC.ClientDetail.Associates, oResponse.PartyKey)
                ProcessPartyConvictions(con, oCoreBusiness, oBasePartyPC.ClientDetail.Convictions, oResponse.PartyKey)
                ProcessLoyaltyScheme(con, oCoreBusiness, oBasePartyPC.ClientDetail.LoyaltyScheme, oResponse.PartyKey)
                ProcessProspectPolicies(con, oCoreBusiness, oBasePartyPC.ClientDetail.ProspectPolicies, oResponse.PartyKey)
            Else
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_prospect_add")

                    cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = oResponse.PartyKey
                    cmd.AddInParameter("@current_intermediary", SqlDbType.Int).Value = SqlInt32.Null
                    cmd.AddInParameter("@agent_reference", SqlDbType.VarChar, 70).Value = DBNull.Value
                    cmd.AddInParameter("@previous_broker_cnt", SqlDbType.Int).Value = SqlInt32.Null
                    cmd.AddInParameter("@Strength_code_id", SqlDbType.Int, 70).Value = DBNull.Value
                    cmd.AddInParameter("@previous_insurer_cnt", SqlDbType.Int).Value = SqlInt32.Null
                    cmd.AddInParameter("@prospect_status_id", SqlDbType.Int, 70).Value = SqlInt32.Null


                    con.ExecuteNonQuery(cmd)

                End Using
            End If

        ElseIf sPartyType = "CC" Then

            ' update data specific to CC type

            'UpdatePartyCCDetails(con, oCoreBusiness, oBasePartyCC, oResponse.PartyKey)
            UpdatePartyCCDetails(con, oCoreBusiness, oBasePartyCC, oResponse.PartyKey, oUpdatePartyAddDetails)

            If oBasePartyCC.ClientDetail IsNot Nothing Then

                'update contents of ClientSharedDetails

                UpdatePartyDetails(con, oCoreBusiness, oBasePartyCC.ClientDetail, oResponse.PartyKey, oUpdatePartyAddDetails)
                ProcessProspect(con, oCoreBusiness, oBasePartyCC.ClientDetail, oResponse.PartyKey)
                ProcessAssociates(con, oCoreBusiness, oBasePartyCC.ClientDetail.Associates, oResponse.PartyKey)
                ProcessPartyConvictions(con, oCoreBusiness, oBasePartyCC.ClientDetail.Convictions, oResponse.PartyKey)
                ProcessLoyaltyScheme(con, oCoreBusiness, oBasePartyCC.ClientDetail.LoyaltyScheme, oResponse.PartyKey)
                ProcessProspectPolicies(con, oCoreBusiness, oBasePartyCC.ClientDetail.ProspectPolicies, oResponse.PartyKey)
            Else
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_prospect_add")

                    cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = oResponse.PartyKey
                    cmd.AddInParameter("@current_intermediary", SqlDbType.Int).Value = SqlInt32.Null
                    cmd.AddInParameter("@agent_reference", SqlDbType.VarChar, 70).Value = DBNull.Value
                    cmd.AddInParameter("@previous_broker_cnt", SqlDbType.Int).Value = SqlInt32.Null
                    cmd.AddInParameter("@Strength_code_id", SqlDbType.Int, 70).Value = DBNull.Value
                    cmd.AddInParameter("@previous_insurer_cnt", SqlDbType.Int).Value = SqlInt32.Null
                    cmd.AddInParameter("@prospect_status_id", SqlDbType.Int, 70).Value = SqlInt32.Null

                    con.ExecuteNonQuery(cmd)

                End Using


            End If

        ElseIf sPartyType.StartsWith("OT") Then

            ProcessPartyConvictions(con, oCoreBusiness, oBasePartyOther.Convictions, oResponse.PartyKey)

        End If
        'End If

        Return oResponse

    End Function

    Public Overloads Function FindParty(ByVal FindPartyRequest As BaseFindPartyRequestType) As BaseFindPartyResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                            _SiriusUser.Username, _SiriusUser.SourceID, _
                                            _SiriusUser.LanguageID, _
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseFindPartyResponseType = Nothing

            If FindPartyRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.FindPartyRequestType) Then
                oResponse = FindParty(con, FindPartyRequest)
            ElseIf (FindPartyRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.FindPartyRequestType)) Or _
                (FindPartyRequest.GetType Is GetType(BaseImplementationTypes.BaseFindPartyRequestType)) Then

                If FindPartyRequest.PartyType = "PC" Or FindPartyRequest.PartyType = "CC" Or FindPartyRequest.PartyType = "GC" Or FindPartyRequest.PartyType = "BDXIgnorePartyType" Then
                    oResponse = FindPartyVersion2(con, FindPartyRequest)
                Else
                    oResponse = FindSpecialParty(con, FindPartyRequest)
                End If
            End If

            Return oResponse
        End Using

    End Function

    '''<summary>
    '''(Vijayakumar Ramasamy) - (Tech Spec - UIICWR50 - MTC - Party Search) - (7.1.3.2.4.2) 
    '''<summary>
    '''<param name="SiriusConnection">This parameter used to open the Database connection </param>
    '''<param name="BaseFindPartyRequestType">This parameter is used to send find party request type </param>

    Public Function FindPartyVersion2(ByVal con As SiriusConnection, ByVal FindPartyRequest As BaseFindPartyRequestType) As BaseFindPartyResponseType

        Dim sDisableWildcardSearchOption As String
        Dim sEnablePartialWildcardSearchOption As String
        sDisableWildcardSearchOption = ""
        sEnablePartialWildcardSearchOption = ""
        Dim bDisableWildcardSearchOption As Boolean
        Dim bEnablePartialWildcardSearchOption As Boolean
        Dim oErrors As New SAMErrorCollection

        Dim nTypeOfPackage As enumTypeOfPackage
        Dim iPartyType As Integer
        Dim sPartyDescription As String = Nothing
        Dim oOut As New BaseFindPartyResponseType
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim oResultArray As Object = Nothing

        Dim iRecordCount As Integer = 500
        Dim oResultData As Object = Nothing
        Dim docFindParty As New System.Xml.XmlDocument
        Dim oResultArrayOut As Object = Nothing
        Dim oResultArrayCopy(,) As Object = Nothing
        Dim obSIRFindPartyBusiness As New bSIRFindParty.Business
        Dim dsFindPartyResponse As New DataSet
        Dim iComReturnValue As Integer
        Dim sErrorMessage As String = ""
        Dim oSources As Object(,) = Nothing
        'Dim oFilterCol() As Object = {1, 6, 8, 11, 12, 13, 14, 19, 21, 22, 23, 24} 'These are the column needs to be filter from object array
        Dim oWithCol() As Object = {0, 2, 3, 4, 5, 7, 9, 10, 15, 16, 17, 18, 21, 31, 33, 34}
        Dim oArrColHeaders(,) As Object = {{"PartyKey",
                                            "ShortName",
                                            "ResolvedName",
                                            "AddressLine1",
                                            "PostCode",
                                            "Type",
                                            "ContactTelephoneNumber",
                                            "Status",
                                            "FileCode",
                                            "DateOfBirth",
                                            "SwiftLink",
                                            "AddressLine2",
                                            "AgentKey",
                                            "IsDomiciledForTax",
                                            "ServiceLevelCode",
                                            "ServiceLevelDescription"},
                                                          {System.Type.GetType("System.Int32"),
                                                          System.Type.GetType("System.String"),
                                                          System.Type.GetType("System.String"),
                                                          System.Type.GetType("System.String"),
                                                          System.Type.GetType("System.String"),
                                                          System.Type.GetType("System.Int32"),
                                                          System.Type.GetType("System.String"),
                                                          System.Type.GetType("System.String"),
                                                          System.Type.GetType("System.String"),
                                                          System.Type.GetType("System.DateTime"),
                                                          System.Type.GetType("System.String"),
                                                          System.Type.GetType("System.String"),
                                                          System.Type.GetType("System.Int32"),
                                                          System.Type.GetType("System.Int32"),
                                                          System.Type.GetType("System.String"),
                                                          System.Type.GetType("System.String")}}

        'Following line is being added by rk as part of SAM Interop conversions and as per internal and SSP discussion in place of LOC next to it (now commented somewhere below saying oDatabase = con.PMDAODatabase)
        Dim oDatabase As Object = Nothing
        'Dim param As SqlParameter
        'Dim cn As System.Data.SqlClient.SqlConnection
        'con = SiriusConnection.FromAny(connectionString:=ConnectionString)
        'cn = con.SqlConnection
        oDatabase = con.PMDAODatabase

        If FindPartyRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.FindPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oOut = New SAMForInsuranceV2ImplementationTypes.FindPartyResponseType
        ElseIf FindPartyRequest.GetType Is GetType(BaseImplementationTypes.BaseFindPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oOut = New SAMForInsuranceV2ImplementationTypes.FindPartyResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        Dim oSAMErrorsCollection As New SAMErrorCollection

        ' STRUCTURE VALIDATION
        FindPartyRequest.Validate(CType(oSAMErrorsCollection, Object))
        oSAMErrorsCollection.CheckForErrors()

        oCoreBusiness.GetSystemOption(FindPartyRequest.BranchCode, SystemOption.DisableWildcardSearch, sDisableWildcardSearchOption)
        oCoreBusiness.GetSystemOption(FindPartyRequest.BranchCode, SystemOption.EnablePartialWildcardSearch, sEnablePartialWildcardSearchOption)
        If (Trim(sDisableWildcardSearchOption) = "1") Then
            bDisableWildcardSearchOption = True
        Else
            bDisableWildcardSearchOption = False
        End If
        If (Trim(sEnablePartialWildcardSearchOption) = "1") Then
            bEnablePartialWildcardSearchOption = True
        Else
            bEnablePartialWildcardSearchOption = False
        End If

        If Not oCoreBusiness.ValidWildcardSearch( _
               bDisableWildcardSearchOption, _
               bEnablePartialWildcardSearchOption, _
               FindPartyRequest.Shortname, sErrorMessage) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     sErrorMessage, _
                                     "Shortname")
        End If
        oErrors.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
              bDisableWildcardSearchOption, _
              bEnablePartialWildcardSearchOption, _
              FindPartyRequest.Name, sErrorMessage) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     sErrorMessage, _
                                     "Name")
        End If
        oErrors.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
              bDisableWildcardSearchOption, _
              bEnablePartialWildcardSearchOption, _
              FindPartyRequest.Firstname, sErrorMessage) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     sErrorMessage, _
                                     "Firstname")
        End If
        oErrors.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
              bDisableWildcardSearchOption, _
              bEnablePartialWildcardSearchOption, _
              FindPartyRequest.FileCode, sErrorMessage) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     sErrorMessage, _
                                     "FileCode")
        End If
        oErrors.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
              bDisableWildcardSearchOption, _
              bEnablePartialWildcardSearchOption, _
              FindPartyRequest.AddressLine1, sErrorMessage) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     sErrorMessage, _
                                     "AddressLine1")
        End If
        oErrors.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
              bDisableWildcardSearchOption, _
              bEnablePartialWildcardSearchOption, _
              FindPartyRequest.AreaCode, sErrorMessage) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     sErrorMessage, _
                                     "AreaCode")
        End If
        oErrors.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
              bDisableWildcardSearchOption, _
              bEnablePartialWildcardSearchOption, _
              FindPartyRequest.PolicyRef, sErrorMessage) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     sErrorMessage, _
                                     "PolicyRef")
        End If
        oErrors.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
              bDisableWildcardSearchOption, _
              bEnablePartialWildcardSearchOption, _
              FindPartyRequest.RiskRequestdex, sErrorMessage) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     sErrorMessage, _
                                     "RiskRequestdex")
        End If
        oErrors.CheckForErrors()

        ' DATA VALIDATION
        ' party type must be a valid code
        If FindPartyRequest.PartyType.ToString <> "BDXIgnorePartyType" Then
            If Not String.IsNullOrEmpty(FindPartyRequest.PartyType.ToString) Then
                iPartyType = GetAndValidateSpecifiedTableCode(con, "Party_Type", "party_type_id", "code", FindPartyRequest.PartyType.ToString, oSAMErrorsCollection, "PartyTypeCode")
            End If


            If iPartyType = 0 Then
                oSAMErrorsCollection.AddBusinessRule( _
                             SAMConstants.SAMBusinessErrors.PartyTypeNotFound, _
                             SAMConstants.SAMBusinessErrors.PartyTypeNotFound.ToString)
                oSAMErrorsCollection.CheckForErrors()
            Else
                sPartyDescription = GetAndValidateDescriptionById(con, "Party_Type", "Description", "party_type_id", iPartyType.ToString)
            End If
        End If
        'Get Agent Group Details if AgentKey is provided
        With FindPartyRequest
            If (.AgentKey <> 0) Then
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Agent_Group_From_Agent_Key")
                    cmd.AddOutParameter("@agent_group_cnt", SqlDbType.Int)
                    cmd.AddInParameter("@agent_cnt", SqlDbType.Int).Value = .AgentKey

                    con.ExecuteNonQuery(cmd)
                    .AgentGroupKey = Cast.ToInt32(cmd.Parameters("@agent_group_cnt").Value, 0)
                End Using
            End If
        End With

        ' validate the branch code
        FindPartyRequest.SourceId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
            PMLookupTable.Source, FindPartyRequest.BranchCode, "BranchCode", oSAMErrorsCollection)

        GetValidSources(con, oSources, FindPartyRequest.IncludeClosedBranches)
        If nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package AndAlso Not String.IsNullOrEmpty(FindPartyRequest.ClaimsRiskIndex) Then

            Dim oClaimsData As Object = Nothing
            Dim oGISSearchDataArray As Object = Nothing
            'Dim oGISSearchDataArray As Object(,) = Nothing
            Dim iCounter As Integer
            Dim sACMethodName As String = "STS"
            'Dim oDatabase As Object = Nothing
            Dim oBackofficelink As New bBackOfficeLink.bBOLink
            Dim oClaimBusiness As New bCLMFindClaim.Business

            'oDatabase = con.PMDAODatabase
            Try

                oBackofficelink.Initialise(sUsername:=_SiriusUser.Username, sPassword:=_SiriusUser.Password, _
                                   iUserID:=_SiriusUser.UserID, iSourceId:=_SiriusUser.SourceID, _
                                   iLanguageID:=_SiriusUser.LanguageID, iCurrencyID:=_SiriusUser.CurrencyID, _
                                   iLogLevel:=SiriusUserDefaults.LogLevel, sCallingAppName:=sACMethodName, vDatabase:=oDatabase)

                Dim oGISSearchDataArray1(,) As Object = CType(oGISSearchDataArray, Object(,))
                With FindPartyRequest
                    'iComReturnValue = oBackofficelink.FindLikeIndex(sIndex:=.ClaimsRiskIndex, lNumberOfRecords:=FindPartyRequest.MaxRowsToFetch, vResultArray:=DirectCast(oGISSearchDataArray, Object(,)), sDataModelType:="CLAIM", v_vAgentGroupCnt:=.AgentGroupKey)
                    iComReturnValue = oBackofficelink.FindLikeIndex(sIndex:=.ClaimsRiskIndex, lNumberOfRecords:=FindPartyRequest.MaxRowsToFetch, vResultArray:=oGISSearchDataArray1, sDataModelType:="CLAIM", v_vAgentGroupCnt:=.AgentGroupKey)
                End With
                oGISSearchDataArray = CType(oGISSearchDataArray1, Object)

                If (iComReturnValue <> PMEReturnCode.PMTrue) Then
                    If iComReturnValue = PMEReturnCode.PMNotFound Then
                        oOut.ResultDataset = Nothing
                        oOut.ResultData = Nothing
                        Return oOut
                    Else
                        RaiseComMethodException("bBackOfficeLink.bBOLink.FindLikeIndex", iComReturnValue)
                    End If

                Else
                    iComReturnValue = oClaimBusiness.GetMultiPolicyClaims(vInputData:=oGISSearchDataArray, vOutputData:=oClaimsData, v_vSiriusProduct:=oBackofficelink.Sirius_Product, v_vRegNumber:=FindPartyRequest.ClaimsRiskIndex)
                    If (iComReturnValue <> PMEReturnCode.PMTrue) Then
                        If iComReturnValue = PMEReturnCode.PMNotFound Then
                            oOut.ResultDataset = Nothing
                            oOut.ResultData = Nothing
                            Return oOut
                        Else
                            RaiseComMethodException("bCLMFindClaim.Business.GetMultiPolicyClaims", iComReturnValue)
                        End If
                    End If
                End If
                If (IsArray(oClaimsData) = False) Then
                    RaiseComMethodException("bCLMFindClaim.Business.GetMultiPolicyClaims", iComReturnValue)
                Else
                    Dim dsClaimsData As DataSet
                    Dim dsResultData As New DataSet

                    obSIRFindPartyBusiness.Initialise(sUsername:=_SiriusUser.Username, sPassword:=_SiriusUser.Password, _
                                                      iUserID:=_SiriusUser.UserID, iSourceID:=_SiriusUser.SourceID, _
                                                      iLanguageID:=_SiriusUser.LanguageID, iCurrencyID:=_SiriusUser.CurrencyID, _
                                                      iLogLevel:=SiriusUserDefaults.LogLevel, sCallingAppName:=sACMethodName, vDatabase:=oDatabase)

                    dsClaimsData = Utilities.ArrayToDataSet(oClaimsData)
                    If dsClaimsData.Tables.Count > 0 Then
                        If dsClaimsData.Tables(0).Rows.Count > 0 Then
                            For iCounter = 0 To dsClaimsData.Tables(0).Rows.Count - 1
                                Dim dsPartyRow As DataSet
                                Dim oResultData1(,) As Object = CType(oResultData, Object(,))
                                iComReturnValue = obSIRFindPartyBusiness.SearchByQuery(r_vResultArray:=oResultData1, r_lNumberOfRecords:=FindPartyRequest.MaxRowsToFetch, v_vClaimNumber:=dsClaimsData.Tables(0).Rows(iCounter)(3))
                                oResultData = CType(oResultData1, Object)
                                If (iComReturnValue <> PMEReturnCode.PMTrue) Then
                                    RaiseComMethodException("bSIRFindParty.Business.SearchByQuery", iComReturnValue)
                                End If
                                dsPartyRow = Utilities.ArrayToDataSet(oResultData)
                                If (dsPartyRow.Tables(0).Rows.Count > 0) Then
                                    dsResultData.Merge(dsPartyRow)
                                End If
                            Next
                        End If
                    End If
                    oResultData = Utilities.DataSetToArray(dsResultData)
                    oResultArrayOut = CopyArrayWithColumns(CType(oResultData, Object(,)), oResultArrayCopy, oWithCol)
                    dsFindPartyResponse = Utilities.ArrayToDataSet(oResultArrayOut, oArrColHeaders, "BaseFindPartyResponseTypeParties")
                    If dsFindPartyResponse.Tables.Count > 0 Then
                        If dsFindPartyResponse.Tables(0).Rows.Count > 0 Then
                            For iCount As Integer = 0 To dsFindPartyResponse.Tables(0).Rows.Count - 1
                                If dsFindPartyResponse.Tables(0).Rows(iCount).Item("Status").ToString = "1" Then
                                    dsFindPartyResponse.Tables(0).Columns("Status").ReadOnly = False
                                    dsFindPartyResponse.Tables(0).Rows(iCount).Item("Status") = "Prospect"
                                    dsFindPartyResponse.Tables(0).Columns("Status").ReadOnly = True
                                Else
                                    dsFindPartyResponse.Tables(0).Columns("Status").ReadOnly = False
                                    dsFindPartyResponse.Tables(0).Rows(iCount).Item("Status") = "Client"
                                    dsFindPartyResponse.Tables(0).Columns("Status").ReadOnly = True
                                End If
                            Next
                        End If
                    End If

                    If FindPartyRequest.WCFSecurityToken = "" Then
                    docFindParty.LoadXml(dsFindPartyResponse.GetXml)
                    oOut.ResultDataset = docFindParty.DocumentElement()
                End If

                    oOut.ResultData = dsFindPartyResponse
                End If
            Finally
                If oBackofficelink IsNot Nothing Then
                    oBackofficelink.Dispose()
                    oBackofficelink = Nothing
                End If
                If oClaimBusiness IsNot Nothing Then
                    oClaimBusiness.Dispose()
                    oClaimBusiness = Nothing
                End If
                If obSIRFindPartyBusiness IsNot Nothing Then
                    obSIRFindPartyBusiness.Dispose()
                    obSIRFindPartyBusiness = Nothing
                End If
            End Try
        ElseIf nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package AndAlso (Not String.IsNullOrEmpty(FindPartyRequest.RiskRequestdex) OrElse Not String.IsNullOrEmpty(FindPartyRequest.PartyIndex)) Then
            Dim dsPartyByRiskOrPartyIndex As DataSet
                With FindPartyRequest
                If Not String.IsNullOrEmpty(FindPartyRequest.PartyIndex) Then
                    dsPartyByRiskOrPartyIndex = FindPartyByPartyIndex(con, .PartyIndex, .PartyTypeSpecified, .PartyType, FindPartyRequest.MaxRowsToFetch, FindPartyRequest.LoginUserName).ResultArray
            Else
                    dsPartyByRiskOrPartyIndex = FindPartyByRiskIndex(con, .RiskRequestdex, .PartyTypeSpecified, .PartyType, .AgentGroupKey, FindPartyRequest.MaxRowsToFetch).ResultArray
            End If
            End With

            If FindPartyRequest.WCFSecurityToken = "" Then
                docFindParty.LoadXml(dsPartyByRiskOrPartyIndex.GetXml())
            oOut.ResultDataset = docFindParty.DocumentElement()
            End If
            oOut.ResultData = dsPartyByRiskOrPartyIndex

        Else

            'Find party using back office interop
            Try

                If FindPartyRequest.DateOfBirth > System.DateTime.Now AndAlso FindPartyRequest.DateOfBirthSpecified = True Then
                    oOut.ResultDataset = Nothing
                Else
                    'rk updates following as part of SAM SFI Interop conversions
                    'Following commented by rk because the calls for methods next to this LOC dont succeed
                    SAMFunc.InitialiseSBOObject(con, obSIRFindPartyBusiness, _SiriusUser, "bSIRFindParty.Business")
                    'vikas commented check it
                    'obSIRFindPartyBusiness.Initialise(sUsername:=_SiriusUser.Username, sPassword:=_SiriusUser.Password, _
                    '                                  iUserID:=_SiriusUser.UserID, iSourceID:=_SiriusUser.SourceID, _
                    '                                  iLanguageID:=_SiriusUser.LanguageID, iCurrencyID:=_SiriusUser.CurrencyID, _
                    '                                  iLogLevel:=SiriusUserDefaults.LogLevel, sCallingAppName:=sACMethodName)

                    Dim oDOB As Object
                    'If Cast.ToBoolean(FindPartyRequest.DateOfBirthSpecified, False) Then
                    If FindPartyRequest.DateOfBirthSpecified = True Then
                        oDOB = FindPartyRequest.DateOfBirth
                    Else
                        oDOB = Nothing
                    End If
                    If FindPartyRequest.MaxRowsToFetchSpecified Then
                        iRecordCount = FindPartyRequest.MaxRowsToFetch
                    End If
                    Dim oResultArray1(,) As Object = CType(oResultArray, Object(,))
                    With FindPartyRequest

                        iComReturnValue = obSIRFindPartyBusiness.SearchByQuery(r_vResultArray:=oResultArray1, _
                                                                         r_lNumberOfRecords:=iRecordCount, _
                                                                         v_vShortName:=.Shortname, _
                                                                         v_vName:=.Name, _
                                                                         v_vFileCode:=.FileCode, _
                                                                         v_vClientType:=sPartyDescription, _
                                                                         v_vStatusType:=.Status, _
                                                                         v_vAddress1:=.AddressLine1, _
                                                                         v_vPostalCode:=.PostCode, _
                                                                         v_vAreaCode:=.AreaCode, _
                                                                         v_vNumber:=.TelephoneNumber, _
                                                                         v_vInsuranceRef:=.PolicyRef, _
                                                                         v_vDOB:=oDOB, _
                                                                         v_vClaimNumber:=.ClaimNumber, _
                                                                         v_vAgentCnt:=.AgentKey, _
                                                                         v_vRiskIndex:=.ClaimsRiskIndex, _
                                                                         v_vValidSourceArray:=CType(oSources, Object), _
                                                                         v_vAgentGroupCnt:=.AgentGroupKey, _
                                                                             v_lNumberOfRecords:=FindPartyRequest.MaxRowsToFetch, _
                                                                             v_vCaseNumber:=FindPartyRequest.CaseNumber)

                    End With
                    oResultArray = CType(oResultArray1, Object)
                    If iComReturnValue <> PMEReturnCode.PMTrue Then
                        If iComReturnValue = PMEReturnCode.PMNotFound Then
                            oOut.ResultDataset = Nothing
                            Return oOut
                        Else
                            RaiseComMethodException("bSIRFindParty.Business.SearchByQuery", iComReturnValue)
                        End If
                    End If
                    oResultArrayOut = CopyArrayWithColumns(CType(oResultArray, Object(,)), oResultArrayCopy, oWithCol)
                    dsFindPartyResponse = Utilities.ArrayToDataSet(oResultArrayOut, oArrColHeaders, "BaseFindPartyResponseTypeParties")

                    If dsFindPartyResponse.Tables.Count > 0 Then
                        If dsFindPartyResponse.Tables(0).Rows.Count > 0 Then
                            For iCount As Integer = 0 To dsFindPartyResponse.Tables(0).Rows.Count - 1
                                If dsFindPartyResponse.Tables(0).Rows(iCount).Item("Status").ToString = "1" Then
                                    dsFindPartyResponse.Tables(0).Columns("Status").ReadOnly = False
                                    dsFindPartyResponse.Tables(0).Rows(iCount).Item("Status") = "Prospect"
                                    dsFindPartyResponse.Tables(0).Columns("Status").ReadOnly = True
                                Else
                                    dsFindPartyResponse.Tables(0).Columns("Status").ReadOnly = False
                                    dsFindPartyResponse.Tables(0).Rows(iCount).Item("Status") = "Client"
                                    dsFindPartyResponse.Tables(0).Columns("Status").ReadOnly = True
                                End If
                            Next
                        End If
                    End If
                    If FindPartyRequest.WCFSecurityToken = "" Then
                    docFindParty.LoadXml(dsFindPartyResponse.GetXml())
                    oOut.ResultDataset = docFindParty.DocumentElement()
                End If
                    oOut.ResultData = dsFindPartyResponse

                End If
            Finally
                If obSIRFindPartyBusiness IsNot Nothing Then
                    obSIRFindPartyBusiness.Dispose()
                    obSIRFindPartyBusiness = Nothing
                End If
            End Try
        End If
        Return oOut
    End Function

    '''<summary>
    ''' This is core SAM method for FindSpecialParty 
    '''</summary>
    '''<param name="con" type="SiriusConnection"></param>   
    '''<param name="oFindPartyRequest" type="BaseFindPartyRequestType"></param>   
    '''<returns>BaseFindPartyResponseType</returns>
    '''<remarks></remarks> 
    Public Function FindSpecialParty(ByVal con As SiriusConnection, ByVal oFindPartyRequest As BaseFindPartyRequestType) As BaseFindPartyResponseType

        Dim oCoreBusiness As New CoreBusiness
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim iPartyType As Integer
        Dim sPartyDescription As String = ""
        Dim sAgentDescription As String = ""
        Dim oResponse As New BaseFindPartyResponseType
        Dim docFindSpecialParty As New System.Xml.XmlDocument
        Dim dsFindSpecialParty As New DataSet
        Dim oErrors As New SAMErrorCollection
        Dim oSources As Object(,) = Nothing

        Dim sDisableWildcardSearchOption As String
        Dim sEnablePartialWildcardSearchOption As String
        sDisableWildcardSearchOption = ""
        sEnablePartialWildcardSearchOption = ""
        Dim bDisableWildcardSearchOption As Boolean
        Dim bEnablePartialWildcardSearchOption As Boolean
        Dim sErrorMessage As String = ""

        If oFindPartyRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.FindPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.FindPartyResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        '*******************
        ' DATA VALIDATION
        '*******************

        oCoreBusiness.GetSystemOption(oFindPartyRequest.BranchCode, SystemOption.DisableWildcardSearch, sDisableWildcardSearchOption)
        oCoreBusiness.GetSystemOption(oFindPartyRequest.BranchCode, SystemOption.EnablePartialWildcardSearch, sEnablePartialWildcardSearchOption)
        If (Trim(sDisableWildcardSearchOption) = "1") Then
            bDisableWildcardSearchOption = True
        Else
            bDisableWildcardSearchOption = False
        End If
        If (Trim(sEnablePartialWildcardSearchOption) = "1") Then
            bEnablePartialWildcardSearchOption = True
        Else
            bEnablePartialWildcardSearchOption = False
        End If

        If Not oCoreBusiness.ValidWildcardSearch( _
               bDisableWildcardSearchOption, _
               bEnablePartialWildcardSearchOption, _
               oFindPartyRequest.Shortname, _
               sErrorMessage) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     sErrorMessage, _
                                     "Shortname")
        End If
        oErrors.CheckForErrors()
        If Not oCoreBusiness.ValidWildcardSearch( _
                      bDisableWildcardSearchOption, _
                      bEnablePartialWildcardSearchOption, _
                      oFindPartyRequest.Name, _
                      sErrorMessage) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     sErrorMessage, _
                                     "Name")
        End If
        oErrors.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
              bDisableWildcardSearchOption, _
              bEnablePartialWildcardSearchOption, _
              oFindPartyRequest.FileCode, _
              sErrorMessage) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     sErrorMessage, _
                                     "FileCode")
        End If
        oErrors.CheckForErrors()

        ' validate the branch code
        oFindPartyRequest.SourceId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
            PMLookupTable.Source, oFindPartyRequest.BranchCode, "BranchCode", oErrors)
        ' party type must be a valid code
        If Not String.IsNullOrEmpty(oFindPartyRequest.PartyType) AndAlso oFindPartyRequest.PartyTypeSpecified = True Then

            If oFindPartyRequest.PartyType <> "OTOTHERPARTY" Then
                iPartyType = GetAndValidateSpecifiedTableCode(con, "Party_Type", "party_type_id", "code", oFindPartyRequest.PartyType, oErrors, "PartyTypeCode")
            Else
                If oFindPartyRequest.OtherPartyTypeCode IsNot Nothing AndAlso oFindPartyRequest.OtherPartyTypeCode <> "" Then
                iPartyType = GetAndValidateSpecifiedTableCode(con, "Party_Type", "party_type_id", "code", oFindPartyRequest.OtherPartyTypeCode, oErrors, "PartyTypeCode")
                Else
                    iPartyType = -1
            End If
            End If
            'To get party Description
            If iPartyType = -1 Then
                sPartyDescription = "OT"
            Else
            sPartyDescription = GetAndValidateDescriptionById(con, "Party_Type", "Description", "party_type_id", iPartyType.ToString)
            End If

        Else
            If (oFindPartyRequest.SearchType <> "") Then
                sPartyDescription = oFindPartyRequest.SearchType
            Else
            sPartyDescription = Nothing
        End If

        End If
        oErrors.CheckForErrors()
        If oFindPartyRequest.AgentTypeSpecified Then
            Select Case oFindPartyRequest.AgentType
                Case PartyAgentType.Broker
                    sAgentDescription = "Broker"
                Case PartyAgentType.CommissionAccount
                    sAgentDescription = "Commission Account"
                Case PartyAgentType.Intermediary
                    sAgentDescription = "Intermediary"
                Case PartyAgentType.SubAgent
                    sAgentDescription = "Sub-Agent"
            End Select
        Else
            sAgentDescription = Nothing
        End If

        If nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then
            'Find party using back office interop
            Dim oFindPartyBusiness As bSIRFindParty.Business = Nothing
            Dim comReturnValue As Integer
            Dim vResultArray As Object(,) = Nothing
            Dim iRecordCount As Integer = 500

            Try
		GetValidSources(con, oSources, oFindPartyRequest.IncludeClosedBranches)
                oFindPartyBusiness = New bSIRFindParty.Business
                SAMFunc.InitialiseSBOObject(con, oFindPartyBusiness, _SiriusUser, "bSIRFindParty.Business")

                oFindPartyBusiness.SetProcessModes(vTransactionType:=CObj(oFindPartyRequest.TransactionType))
                If oFindPartyRequest.MaxRowsToFetchSpecified Then
                    iRecordCount = oFindPartyRequest.MaxRowsToFetch
                End If
                With oFindPartyRequest
                    comReturnValue = oFindPartyBusiness.SearchSpecialPartyByQuery( _
                    r_lNumberOfRecords:=iRecordCount, _
                    r_vResultArray:=vResultArray, _
                    v_vShortName:=.Shortname, _
                    v_vName:=.Name, _
                    v_vFileCode:=.FileCode, _
                    v_vClientType:=sPartyDescription, _
                    v_vAgentType:=sAgentDescription, _
                    v_vStatusType:=.Status, _
                    v_vAddress1:=.AddressLine1, _
                    v_vPostalCode:=.PostCode, _
                    v_vAreaCode:=.AreaCode, _
                    v_vNumber:=.TelephoneNumber, _
                    v_vInsuranceRef:=.PolicyRef, _
                    v_vBranch:=.PartySourceId, _
                    v_vValidSourceArray:=oSources, _
                    v_bSuppressSubAgents:=Convert.ToBoolean(IIf(.SupressSubAgentsSpecified, .SupressSubAgents, False)), _
                    v_lCommissionLevel:=0, V_BranchCode:=.BranchCode, sAgentGroup:=.AgentGroup, _
                    v_sSearchType:=.SearchType) 'Till CommissionLevel filter is added on nexus screen, we pass default 0
                End With

                If comReturnValue <> PMEReturnCode.PMTrue AndAlso comReturnValue <> PMReturnCode.PMNotFound Then
                    RaiseComMethodException("bSIRFindParty.Business.SearchSpecialPartyByQuery", comReturnValue)
                End If

                dsFindSpecialParty = Utilities.ArrayToDataSet(vResultArray, "BaseFindPartyResponseTypeParties")
                If dsFindSpecialParty IsNot Nothing Then
                    If dsFindSpecialParty.Tables(0).Rows.Count > 0 Then
                        dsFindSpecialParty.DataSetName = "BaseFindPartyResponseTypeParties"
                        dsFindSpecialParty.Tables(0).TableName = "Row"
                        dsFindSpecialParty.Tables(0).Columns(0).ColumnName = "PartyKey"
                        dsFindSpecialParty.Tables(0).Columns(1).ColumnName = "Type"
                        dsFindSpecialParty.Tables(0).Columns(2).ColumnName = "ShortName"
                        dsFindSpecialParty.Tables(0).Columns(3).ColumnName = "Name"
                        dsFindSpecialParty.Tables(0).Columns(4).ColumnName = "AddressLine1"
                        dsFindSpecialParty.Tables(0).Columns(5).ColumnName = "PostCode"
                        dsFindSpecialParty.Tables(0).Columns(6).ColumnName = "PartySourceId"
                        dsFindSpecialParty.Tables(0).Columns(8).ColumnName = "ReinsuranceType"
                        dsFindSpecialParty.Tables(0).Columns(10).ColumnName = "IsProspect"
                        dsFindSpecialParty.Tables(0).Columns(13).ColumnName = "ResolvedName"
                        dsFindSpecialParty.Tables(0).Columns(14).ColumnName = "AgentType"
                        dsFindSpecialParty.Tables(0).Columns(15).ColumnName = "FileCode"
                        dsFindSpecialParty.Tables(0).Columns(17).ColumnName = "SwiftLink"
                        dsFindSpecialParty.Tables(0).Columns(18).ColumnName = "AddressLine2"
                        dsFindSpecialParty.Tables(0).Columns(20).ColumnName = "PartySourceDescription"
                        dsFindSpecialParty.Tables(0).Columns(24).ColumnName = "DateCancelled"
                        dsFindSpecialParty.Tables(0).Columns(25).ColumnName = "AllowConsolidatedCommission"
                        dsFindSpecialParty.Tables(0).Columns(26).ColumnName = "IsDomiciledForTax"

                        If dsFindSpecialParty.Tables(0).Columns.Count > 28 Then
                            dsFindSpecialParty.Tables(0).Columns(28).ColumnName = "CountryCode"
                        End If

                        dsFindSpecialParty.Tables(0).Columns.RemoveAt(23)
                        dsFindSpecialParty.Tables(0).Columns.RemoveAt(22)
                        dsFindSpecialParty.Tables(0).Columns.RemoveAt(21)
                        dsFindSpecialParty.Tables(0).Columns.RemoveAt(19)
                        dsFindSpecialParty.Tables(0).Columns.RemoveAt(16)
                        dsFindSpecialParty.Tables(0).Columns.RemoveAt(12)
                        dsFindSpecialParty.Tables(0).Columns.RemoveAt(11)
                        dsFindSpecialParty.Tables(0).Columns.RemoveAt(9)
                        dsFindSpecialParty.Tables(0).Columns.RemoveAt(7)

                        If oFindPartyRequest.WCFSecurityToken = "" Then
                        docFindSpecialParty.LoadXml(dsFindSpecialParty.GetXml())
                        oResponse.ResultDataset = docFindSpecialParty.DocumentElement()
                        End If
                        oResponse.ResultData = dsFindSpecialParty
                    Else
                        oResponse.ResultDataset = Nothing
                        oResponse.ResultData = Nothing
                    End If
                Else
                    oResponse.ResultDataset = Nothing
                    oResponse.ResultData = Nothing
                End If
            Finally
                If oFindPartyBusiness IsNot Nothing Then
                    oFindPartyBusiness.Dispose()
                    oFindPartyBusiness = Nothing
                End If
            End Try
        End If
        Return oResponse
    End Function

    '''<summary>
    ''' This is generic function which used to filter the two dimentional array - Gaurav .
    '''<summary>
    '''<param name="oCopyFrom(,)">This is the result array set which is returned by back office component</param>
    '''<param name="oCopyTo(,)">This is empty array which is used to copy from result array set which is returned by back office component </param>
    '''<param name=" oWithCol()">This parameter is used to send parameters which is to retained in an array </param>

    Public Function CopyArrayWithColumns(ByVal oCopyFrom(,) As Object, ByRef oCopyTo(,) As Object, ByVal oWithCol() As Object) As Object(,)

        Dim iUBoundColFrom As Integer
        Dim iLBoundColFrom As Integer
        Dim iUBoundColTo As Integer
        Dim iUBoundRowTo As Integer

        oCopyFrom = CType(oCopyFrom, System.Object(,))

        iLBoundColFrom = oCopyFrom.GetLowerBound(0)
        iUBoundColFrom = oCopyFrom.GetUpperBound(0)

        iUBoundColTo = oWithCol.GetUpperBound(0) '- 1
        iUBoundRowTo = oCopyFrom.GetUpperBound(1)

        Dim iPasteColTo As Integer = 0
        ReDim oCopyTo(iUBoundColTo, iUBoundRowTo)
        Dim bFilterThisColumn As Boolean
        For iCount As Integer = iLBoundColFrom To iUBoundColFrom
            For iCheckIfExists As Integer = 0 To oWithCol.GetUpperBound(0)
                If Cast.ToInt32(iCount).ToString <> Cast.ToInt32(oWithCol(iCheckIfExists)).ToString AndAlso _
                    iCheckIfExists = oWithCol.GetUpperBound(0) Then
                    bFilterThisColumn = True
                    Exit For
                ElseIf Cast.ToInt32(iCount).ToString = Cast.ToInt32(oWithCol(iCheckIfExists)).ToString Then
                    bFilterThisColumn = False
                    Exit For
                End If
            Next
            If bFilterThisColumn = False Then
                For iPasteData As Integer = 0 To oCopyFrom.GetUpperBound(1)
                    oCopyTo(iPasteColTo, iPasteData) = oCopyFrom(iCount, iPasteData)
                Next
                iPasteColTo += 1
            End If

        Next
        Return oCopyTo

    End Function

    '''<summary>
    ''' This is generic function which used to filter the two dimentional array - Gaurav .
    '''<summary>
    '''<param name="oCopyFrom(,)">This is the result array set which is returned by back office component</param>
    '''<param name="oCopyTo(,)">This is empty array which is used to copy from result array set which is returned by back office component </param>
    '''<param name=" oFilterCol()">This parameter is used to send fiter column which needs to filter from the com component result set </param>

    Public Function CopyArrayWithFilters(ByVal oCopyFrom(,) As Object, ByRef oCopyTo(,) As Object, ByVal oFilterCol() As Object) As Object(,)

        Dim iUBoundColFrom As Integer
        Dim iLBoundColFrom As Integer
        Dim iUBoundColTo As Integer
        Dim iUBoundRowTo As Integer

        oCopyFrom = CType(oCopyFrom, System.Object(,))

        iLBoundColFrom = oCopyFrom.GetLowerBound(0)
        iUBoundColFrom = oCopyFrom.GetUpperBound(0)

        iUBoundColTo = oCopyFrom.GetUpperBound(0) - oFilterCol.GetUpperBound(0) - 1
        iUBoundRowTo = oCopyFrom.GetUpperBound(1)

        Dim iPasteColTo As Integer = 0
        ReDim oCopyTo(iUBoundColTo, iUBoundRowTo)
        Dim bFilterThisColumn As Boolean
        For iCount As Integer = iLBoundColFrom To iUBoundColFrom
            For iCheckIfExists As Integer = 0 To oFilterCol.GetUpperBound(0)
                If Cast.ToInt32(iCount).ToString = Cast.ToInt32(oFilterCol(iCheckIfExists)).ToString Then
                    bFilterThisColumn = True
                    Exit For
                ElseIf Cast.ToInt32(iCount).ToString <> Cast.ToInt32(oFilterCol(iCheckIfExists)).ToString AndAlso _
                    iCheckIfExists = oFilterCol.GetUpperBound(0) Then
                    bFilterThisColumn = False
                End If
            Next
            If bFilterThisColumn = False Then
                For iPasteData As Integer = 0 To oCopyFrom.GetUpperBound(1)
                    oCopyTo(iPasteColTo, iPasteData) = oCopyFrom(iCount, iPasteData)
                Next
                iPasteColTo += 1
            End If

        Next
        Return oCopyTo

    End Function

    Public Overloads Function FindParty(ByVal con As SiriusConnection, ByVal FindPartyRequest As BaseFindPartyRequestType) As BaseFindPartyResponseType
        Const ACMethodName As String = "FindParty"

        Dim oFindPartyResponse As New BaseImplementationTypes.BaseFindPartyResponseType
        Dim oAgentFindPartyRequest As AgentImplementationTypes.FindPartyRequestType = Nothing
        Dim oSAMForBrokingRequest As SAMForBrokingImplementationTypes.FindPartyRequestType = Nothing
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim STSError As New STSErrorPublisher
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim nPartyTypeId As Integer
        Dim sPartyTypeDesc As String = String.Empty
        Dim iSourceId As Integer = 0
        Dim lAgentKey As Integer = 0

        Dim sDisableWildcardSearchOption As String
        Dim sEnablePartialWildcardSearchOption As String
        sDisableWildcardSearchOption = ""
        sEnablePartialWildcardSearchOption = ""
        Dim bDisableWildcardSearchOption As Boolean
        Dim bEnablePartialWildcardSearchOption As Boolean
        Dim oErrors As New SAMErrorCollection

        If FindPartyRequest.GetType Is GetType(AgentImplementationTypes.FindPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oFindPartyResponse = New AgentImplementationTypes.FindPartyResponseType
            oAgentFindPartyRequest = DirectCast(FindPartyRequest, AgentImplementationTypes.FindPartyRequestType)
        ElseIf FindPartyRequest.GetType Is GetType(BaseImplementationTypes.BaseFindPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            oFindPartyResponse = New BaseImplementationTypes.BaseFindPartyResponseType
        ElseIf FindPartyRequest.GetType Is GetType(CustomerImplementationTypes.FindPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.CustomersPackage
            oFindPartyResponse = New CustomerImplementationTypes.FindPartyResponseType
        ElseIf FindPartyRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.FindPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oFindPartyResponse = New SAMForInsuranceImplementationTypes.FindPartyResponseType
            Dim oSAMForInsuranceRequest As SAMForInsuranceImplementationTypes.FindPartyRequestType = DirectCast(FindPartyRequest, SAMForInsuranceImplementationTypes.FindPartyRequestType)
            lAgentKey = CInt(oSAMForInsuranceRequest.AgentKey)
        ElseIf FindPartyRequest.GetType Is GetType(SAMForBrokingImplementationTypes.FindPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage
            oFindPartyResponse = New SAMForBrokingImplementationTypes.FindPartyResponseType
            oSAMForBrokingRequest = DirectCast(FindPartyRequest, SAMForBrokingImplementationTypes.FindPartyRequestType)
        ElseIf FindPartyRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.FindPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oFindPartyResponse = New SAMForInsuranceV2ImplementationTypes.FindPartyResponseType
            Dim oSAMForInsuranceV2Request As SAMForInsuranceV2ImplementationTypes.FindPartyRequestType = DirectCast(FindPartyRequest, SAMForInsuranceV2ImplementationTypes.FindPartyRequestType)
            lAgentKey = CInt(oSAMForInsuranceV2Request.AgentKey)
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        ' Check mandatory fields
        If FindPartyRequest.BranchCode = "" Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oFindPartyResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oFindPartyResponse
        End If

        ' Lookup Codes to ensure they are valid
        Try
            iSourceId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", FindPartyRequest.BranchCode)
        Catch ex As Exception
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BranchCode"), FindPartyRequest.BranchCode)
        End Try

        If nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage AndAlso FindPartyRequest.PartyType.Contains(",") Then

            Dim aSep() As Char = {","c}
            Dim asList() As String = FindPartyRequest.PartyType.Split(aSep)
            Dim sSinglePartyTypeDesc As String = String.Empty

            sPartyTypeDesc = String.Empty

            For Each sItem As String In asList
                Try
                    nPartyTypeId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Party_Type", sItem, sSinglePartyTypeDesc)
                    sPartyTypeDesc = sPartyTypeDesc & sSinglePartyTypeDesc & ","
                Catch ex As Exception
                    STSError.AddInvalidField("PartyType", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "PartyType"), FindPartyRequest.PartyType)
                    Exit For
                End Try
            Next

            If sPartyTypeDesc.Length > 0 Then
                sPartyTypeDesc = sPartyTypeDesc.Substring(0, sPartyTypeDesc.Length - 1)
            End If

        Else

            If String.IsNullOrEmpty(FindPartyRequest.PartyType) Then
                sPartyTypeDesc = "<ALL>"
            Else
                Try
                    nPartyTypeId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Party_Type", FindPartyRequest.PartyType, sPartyTypeDesc)
                Catch ex As Exception
                    STSError.AddInvalidField("PartyType", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "PartyType"), FindPartyRequest.PartyType)
                End Try
            End If

        End If

        ' exit if there are any invalid parameters
        If STSError.HasErrors Then
            STSError.SetContext(oFindPartyResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Lookup field validation", True)
            Return oFindPartyResponse
        End If

        oCoreBusiness.GetSystemOption(FindPartyRequest.BranchCode, SystemOption.DisableWildcardSearch, sDisableWildcardSearchOption)
        oCoreBusiness.GetSystemOption(FindPartyRequest.BranchCode, SystemOption.EnablePartialWildcardSearch, sEnablePartialWildcardSearchOption)
        If (Trim(sDisableWildcardSearchOption) = "1") Then
            bDisableWildcardSearchOption = True
        Else
            bDisableWildcardSearchOption = False
        End If
        If (Trim(sEnablePartialWildcardSearchOption) = "1") Then
            bEnablePartialWildcardSearchOption = True
        Else
            bEnablePartialWildcardSearchOption = False
        End If

        If Not oCoreBusiness.ValidWildcardSearch( _
               bDisableWildcardSearchOption, _
               bEnablePartialWildcardSearchOption, _
               FindPartyRequest.Shortname) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     SAMInvalidData.InvalidWildcardSearch.ToString, _
                                     "Shortname")
        End If
        oErrors.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
              bDisableWildcardSearchOption, _
              bEnablePartialWildcardSearchOption, _
              FindPartyRequest.Name) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     SAMInvalidData.InvalidWildcardSearch.ToString, _
                                     "Name")
        End If
        oErrors.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
              bDisableWildcardSearchOption, _
              bEnablePartialWildcardSearchOption, _
              FindPartyRequest.Firstname) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     SAMInvalidData.InvalidWildcardSearch.ToString, _
                                     "Firstname")
        End If
        oErrors.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
              bDisableWildcardSearchOption, _
              bEnablePartialWildcardSearchOption, _
              FindPartyRequest.FileCode) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     SAMInvalidData.InvalidWildcardSearch.ToString, _
                                     "FileCode")
        End If
        oErrors.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
              bDisableWildcardSearchOption, _
              bEnablePartialWildcardSearchOption, _
              FindPartyRequest.AddressLine1) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     SAMInvalidData.InvalidWildcardSearch.ToString, _
                                     "AddressLine1")
        End If
        oErrors.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
              bDisableWildcardSearchOption, _
              bEnablePartialWildcardSearchOption, _
              FindPartyRequest.AreaCode) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     SAMInvalidData.InvalidWildcardSearch.ToString, _
                                     "AreaCode")
        End If
        oErrors.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
              bDisableWildcardSearchOption, _
              bEnablePartialWildcardSearchOption, _
              FindPartyRequest.PolicyRef) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     SAMInvalidData.InvalidWildcardSearch.ToString, _
                                     "PolicyRef")
        End If
        oErrors.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
              bDisableWildcardSearchOption, _
              bEnablePartialWildcardSearchOption, _
              FindPartyRequest.RiskRequestdex) Then
            oErrors.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     SAMInvalidData.InvalidWildcardSearch.ToString, _
                                     "Risk Index")
        End If
        oErrors.CheckForErrors()

        If STSError.HasErrors Then
            STSError.SetContext(oFindPartyResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Wildcard validation", True)
            Return oFindPartyResponse
        End If

        Dim oFindPartyIn As New FindPartyIn
        Dim oFindPartyOut As FindPartyOut
        Dim vAdditionalData() As AdditionalData
        ReDim vAdditionalData(5)

        vAdditionalData(0) = New AdditionalData
        vAdditionalData(0).Name = "shortname"
        vAdditionalData(0).Value = FindPartyRequest.Shortname

        vAdditionalData(1) = New AdditionalData
        vAdditionalData(1).Name = "name"
        vAdditionalData(1).Value = FindPartyRequest.Name

        vAdditionalData(2) = New AdditionalData
        vAdditionalData(2).Name = "address1"
        vAdditionalData(2).Value = FindPartyRequest.AddressLine1

        vAdditionalData(3) = New AdditionalData
        vAdditionalData(3).Name = "postalcode"
        vAdditionalData(3).Value = FindPartyRequest.PostCode

        vAdditionalData(4) = New AdditionalData
        vAdditionalData(4).Name = "insuranceref"
        vAdditionalData(4).Value = FindPartyRequest.PolicyRef

        vAdditionalData(5) = New AdditionalData
        vAdditionalData(5).Name = "riskindex"
        vAdditionalData(5).Value = FindPartyRequest.RiskRequestdex

        If FindPartyRequest.DateOfBirthSpecified Then
            ReDim Preserve vAdditionalData(6)
            vAdditionalData(6) = New AdditionalData
            vAdditionalData(6).Name = "dob"
            vAdditionalData(6).Value = FindPartyRequest.DateOfBirth
        End If

        If nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage Then
            If CoreBusiness.PartyIsEmployee(oSAMForBrokingRequest.UserPartyType) Then
                Dim iUpper As Integer = UBound(vAdditionalData, 1) + 1
                Dim sData(0 To 1, 0 To 0) As String
                sData(0, 0) = FindPartyRequest.BranchCode
                sData(1, 0) = iSourceId.ToString
                ReDim Preserve vAdditionalData(iUpper)
                vAdditionalData(iUpper) = New AdditionalData
                vAdditionalData(iUpper).Name = "validsourcearray"
                vAdditionalData(iUpper).Value = sData
            End If
        End If

        With oFindPartyIn
            If nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
                .lLeadAgentCnt = CInt(oAgentFindPartyRequest.AgentKey)
            ElseIf nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage Or nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then
                .lLeadAgentCnt = lAgentKey
            ElseIf nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage Then
                If CoreBusiness.PartyIsAgent(oSAMForBrokingRequest.UserPartyType) Then
                    .lLeadAgentCnt = oSAMForBrokingRequest.UserPartyKey
                End If
            End If
            .FileCode = FindPartyRequest.Shortname
            .sAddress1 = FindPartyRequest.AddressLine1
            .sGisBusinessTypeCode = "NB"
            .sGisDataModelCode = "AOL"

            If FindPartyRequest.PartyType.StartsWith("OT") Then
                .sPartyType = FindPartyRequest.PartyType
            Else
                .sPartyType = sPartyTypeDesc
            End If

            .sPolicyNo = FindPartyRequest.PolicyRef
            .sPostcode = FindPartyRequest.PostCode
            .sResolvedName = FindPartyRequest.Name
            .sShortname = FindPartyRequest.Shortname
            .sTelephoneNumber = FindPartyRequest.TelephoneNumber
            '.sUserID 
            .vAdditionalDataArray = vAdditionalData
        End With

        'Handle Risk Index Search per PN 42259
        'If nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage AndAlso FindPartyRequest.RiskRequestdex.Trim() <> String.Empty Then
        '    oFindPartyOut = FindPartyByRiskIndex(con, FindPartyRequest.RiskRequestdex)
        'ElseIf nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package AndAlso FindPartyRequest.RiskRequestdex.Trim() <> String.Empty Then
        '    oFindPartyOut = oCoreBusiness.FindParty(oFindPartyIn)

        Dim tRisk As Object

        tRisk = IIf((FindPartyRequest.RiskRequestdex Is Nothing), "1", "2")

        If nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage AndAlso FindPartyRequest.PartyIndex IsNot Nothing AndAlso Not String.IsNullOrEmpty(FindPartyRequest.PartyIndex) Then
            oFindPartyOut = FindPartyByPartyIndex(con, FindPartyRequest.PartyIndex.Trim, FindPartyRequest.PartyTypeSpecified, FindPartyRequest.PartyType, sLoggedInUserName:=FindPartyRequest.LoginUserName)
        ElseIf nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage AndAlso CStr(tRisk).ToString = "2" Then
            If (FindPartyRequest.RiskRequestdex.Trim <> String.Empty) Then
                oFindPartyOut = FindPartyByRiskIndex(con, FindPartyRequest.RiskRequestdex.Trim, FindPartyRequest.PartyTypeSpecified, FindPartyRequest.PartyType)
            Else
                oFindPartyOut = oCoreBusiness.FindParty(oFindPartyIn)
            End If
        Else
            oFindPartyOut = oCoreBusiness.FindParty(oFindPartyIn)
        End If


        If oFindPartyOut Is Nothing = False Then
            Dim oXmlDoc As New System.Xml.XmlDocument
            Try
                If FindPartyRequest.WCFSecurityToken = "" Then
                oXmlDoc.LoadXml(oFindPartyOut.ResultArray.GetXml)
                End If
            Catch ex As Exception
                Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.GeneralFailure, "Failed to convert xml", ex.Message)
                STSErrorEx.SetContext(oFindPartyResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
                Return oFindPartyResponse
            End Try

            If FindPartyRequest.WCFSecurityToken = "" Then
                Try
                    oFindPartyResponse.ResultDataset = oXmlDoc.DocumentElement
                Catch ex As Exception
                    Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.GeneralFailure, "Failed to convert xml dataset", ex.Message)
                    STSErrorEx.SetContext(oFindPartyResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
                    Return oFindPartyResponse
                End Try
        End If
            oFindPartyResponse.ResultData = oFindPartyOut.ResultArray

        End If

        'TypeCast the response Back.
        If nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
            Dim oResponse As New AgentImplementationTypes.FindPartyResponseType
            oResponse = DirectCast(oFindPartyResponse, AgentImplementationTypes.FindPartyResponseType)
            oFindPartyResponse = oResponse
        End If

        Return oFindPartyResponse
    End Function

    Private Function FindPartyByRiskIndex(ByVal con As SiriusConnection, ByVal riskIndex As String, ByVal PartyTypeSpecified As Boolean, ByVal PartyType As String, Optional ByVal lAgentGroupKey As Integer = 0, Optional ByVal lMaxRecords As Long = -1) As FindPartyOut

        Dim oOut As FindPartyOut = New FindPartyOut()

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_FindParty_By_RiskIndex")
            cmd.AddInParameter("@search_object_name", SqlDbType.VarChar, 70).Value = String.Empty
            cmd.AddInParameter("@search_value", SqlDbType.VarChar, 255).Value = riskIndex
            cmd.AddInParameter("@is_insurance_ref_reqd", SqlDbType.SmallInt).Value = 1
            cmd.AddInParameter("@Specials_Type_Filter", SqlDbType.Int).Value = 0

            cmd.AddInParameter("@agent_group_cnt", SqlDbType.Int).Value = lAgentGroupKey

            If lMaxRecords <> -1 Then
                cmd.AddInParameter("@MaxRowsToFetch", SqlDbType.Int).Value = lMaxRecords
            End If
            If PartyTypeSpecified Then
                cmd.AddInParameter("@sPartyType", SqlDbType.VarChar).Value = PartyType
            End If
            Dim ds As New DataSet("BaseFindPartyResponseTypeParties")
            Dim ret As Integer = con.ExecuteDataSet(cmd, ds, "Row")
            oOut.ResultArray = ds
        End Using

        Return oOut
    End Function

    Private Function FindPartyByPartyIndex(ByVal con As SiriusConnection, ByVal PartyIndex As String, ByVal PartyTypeSpecified As Boolean, ByVal PartyType As String, Optional ByVal lMaxRecords As Long = -1, Optional ByVal sLoggedInUserName As String = "") As FindPartyOut

        Dim oOut As FindPartyOut = New FindPartyOut()

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_FindParty_By_PartyIndex")

            cmd.AddInParameter("@sSearch_value", SqlDbType.VarChar, 255).Value = PartyIndex
            cmd.AddInParameter("@sLoggedInUserName", SqlDbType.VarChar, 255).Value = sLoggedInUserName
            If lMaxRecords <> -1 Then
                cmd.AddInParameter("@nMaxRowsToFetch", SqlDbType.Int).Value = lMaxRecords
            End If
            If PartyTypeSpecified Then
                cmd.AddInParameter("@sPartyType", SqlDbType.VarChar).Value = PartyType
            End If
            Dim ds As New DataSet("BaseFindPartyResponseTypeParties")
            Dim ret As Integer = con.ExecuteDataSet(cmd, ds, "Row")
            oOut.ResultArray = ds
        End Using

        Return oOut
    End Function

    Public Function ForgottenPassword(ByVal ForgottenPasswordRequest As BaseForgottenPasswordRequestType) As BaseForgottenPasswordResponseType
        Const ACMethodName As String = "ForgottenPassword"

        Dim oForgottenPasswordResponse As New BaseImplementationTypes.BaseForgottenPasswordResponseType
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim STSError As New STSErrorPublisher
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        'Dim nPartyTypeId As Integer
        Dim sPartyTypeDesc As String = String.Empty

        Dim iRet As Int32 = 0
        Dim iEmailConfig As Int16 = 0
        Dim sSubject As String = String.Empty
        Dim sMessage As String = String.Empty
        Dim sIPAddress As String = String.Empty
        Dim sFromEmail As String = String.Empty
        Dim sFromName As String = String.Empty
        Dim sProfileName As String = String.Empty
        Dim sProfilePassword As String = String.Empty
        Dim sSMTPServer As String = String.Empty
        Dim sEmailFormat As String = String.Empty
        Dim sUserEmailAddress As String = String.Empty
        Dim sNewUserPassword As String = String.Empty

        If ForgottenPasswordRequest.GetType Is GetType(AgentImplementationTypes.ForgottenPasswordRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oForgottenPasswordResponse = New AgentImplementationTypes.ForgottenPasswordResponseType
        ElseIf ForgottenPasswordRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.ForgottenPasswordRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oForgottenPasswordResponse = New SAMForInsuranceImplementationTypes.ForgottenPasswordResponseType
        ElseIf ForgottenPasswordRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.ForgottenPasswordRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oForgottenPasswordResponse = New SAMForInsuranceV2ImplementationTypes.ForgottenPasswordResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        ' Check mandatory fields
        If ForgottenPasswordRequest.BranchCode = "" Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        If ForgottenPasswordRequest.UserName = "" Then
            STSError.AddInvalidField("UserName", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "UserName"), "")
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oForgottenPasswordResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oForgottenPasswordResponse
        End If

        ' Lookup Codes to ensure they are valid
        Try
            Dim iSourceId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", ForgottenPasswordRequest.BranchCode)
        Catch ex As Exception
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BranchCode"), ForgottenPasswordRequest.BranchCode)
        End Try

        ' exit if there are any invalid parameters
        If STSError.HasErrors Then
            STSError.SetContext(oForgottenPasswordResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Lookup field validation", True)
            Return oForgottenPasswordResponse
        End If

        ' create gis security object
        Dim oGIS As bGIS.Security = Nothing
        Try
            oGIS = New bGIS.Security
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.FailedToInitialiseBackofficeComponent, "Failed to initialise bGIS.STS", ex.Message)
            STSErrorEx.SetContext(oForgottenPasswordResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "oGIS.Initialise", True)
            Return oForgottenPasswordResponse
        End Try

        ' Initialise the GIS
        SAMFunc.InitialiseGISSecurity(oGIS, _SiriusUser)

        ' Get initial settings from web.config        
        sSubject = System.Configuration.ConfigurationManager.AppSettings("FPSubject")
        sMessage = System.Configuration.ConfigurationManager.AppSettings("FPBody")

        ' Get the email profile details from web.config
        sProfileName = System.Configuration.ConfigurationManager.AppSettings("EmailProfileName")
        If (sProfileName Is Nothing) Then
            Throw New Exception("ProfileName is incorrectly configured in web.config")
        End If

        sProfilePassword = System.Configuration.ConfigurationManager.AppSettings("EmailProfilePassword")
        If (sProfilePassword Is Nothing) Then
            sProfilePassword = ""
        End If

        sSMTPServer = System.Configuration.ConfigurationManager.AppSettings("EmailSMTPServer")
        If (sSMTPServer Is Nothing) Then
            sSMTPServer = ""
        End If

        sEmailFormat = System.Configuration.ConfigurationManager.AppSettings("EmailFormat")
        If (sEmailFormat Is Nothing) Then
            ' Default it to Text
            sEmailFormat = "1"
        End If

        sFromEmail = System.Configuration.ConfigurationManager.AppSettings("FPFromEmail")
        If (sFromEmail Is Nothing) Then
            sFromEmail = ""
        End If

        sFromName = System.Configuration.ConfigurationManager.AppSettings("FPFromName")
        If (sFromName Is Nothing) Then
            sFromName = ""
        End If

        Try

            iRet = oGIS.ForgottenPassword( _
                                    v_sDataModelCode:="AOL", _
                                    v_sBusinessTypeCode:="", _
                                    v_sUsername:=ForgottenPasswordRequest.UserName, _
                                    v_sIPAddress:="", _
                                    v_sSubject:=sSubject, _
                                    v_sMessage:=sMessage, _
                                    r_sEmailAddress:=sUserEmailAddress, _
                                    r_sNewPassword:=sNewUserPassword, _
                                    v_vProfileName:=sProfileName, _
                                    v_vProfilePassword:=sProfilePassword)

        Catch ex As MissingMemberException
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            iRet = PMEReturnCode.PMGISOutDated
        Catch ex As Exception
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            ExceptionManager.Publish(ex)
            Throw New Exception("Failed to call bGIS.ForgottenPassword", ex)
        End Try

        ' destroy the interop bGIS.Security
        If oGIS IsNot Nothing Then
            oGIS.Dispose()
            oGIS = Nothing
        End If

        ' if the return value indicates success
        If (iRet = PMEReturnCode.PMTrue) Then
            Try
                'Clear the Cache after Change Password Method Succeeds
                Dim cacheManager As Microsoft.Practices.EnterpriseLibrary.Caching.ICacheManager = Microsoft.Practices.EnterpriseLibrary.Caching.CacheFactory.GetCacheManager()
                If Not cacheManager Is Nothing Then
                    cacheManager.Flush()
                End If
                ' create and send forgotten mail message
                Dim oSmtpClient As SmtpClient = New SmtpClient
                oSmtpClient.Host = sSMTPServer

                sMessage = String.Format(sMessage, _
                                           ForgottenPasswordRequest.UserName, _
                                           sNewUserPassword, "you")

                Dim mail As New MailMessage( _
                                    sFromEmail, _
                                    sUserEmailAddress, _
                                    sSubject, _
                                    sMessage)

                If sEmailFormat = InternalSAMConstants.ACMailFormatText Then
                    mail.IsBodyHtml = False
                ElseIf sEmailFormat = InternalSAMConstants.ACMailFormatHTML Then
                    mail.IsBodyHtml = True
                End If

                oSmtpClient.Send(mail)

            Catch ex As Exception
                ExceptionManager.Publish(ex)
                Throw New Exception("Failed to generate and send email via the SMTP Server - " & sSMTPServer, ex)

            End Try

        Else
            ' raise the appropriate error
            Select Case iRet
                Case PMEReturnCode.PMUserNotExist
                    Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.UserDoesNotExist, "The specified user does not exist in the database", ForgottenPasswordRequest.UserName)
                    STSErrorEX.SetContext(oForgottenPasswordResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "The specified user does not exist in the database", True)
                Case PMEReturnCode.PMNoEmailAddress
                    Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.NoEmailAddressDefinedForUser, "No email address has been defined for the specified user", ForgottenPasswordRequest.UserName)
                    STSErrorEX.SetContext(oForgottenPasswordResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "No email address has been defined for the specified user", True)
                Case PMEReturnCode.PMUpdateUserFailed
                    Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.UserUpdateFailed, "The update to the specified user failed", ForgottenPasswordRequest.UserName)
                    STSErrorEX.SetContext(oForgottenPasswordResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "The update to the specified user failed", True)
                Case PMEReturnCode.PMFalse
                    Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.GeneralFailure, "General Failure Occurred", "")
                    STSErrorEX.SetContext(oForgottenPasswordResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
            End Select

            Return oForgottenPasswordResponse

        End If

        Return oForgottenPasswordResponse

    End Function

    Public Function GetParty(ByVal GetPartyRequest As BaseGetPartyRequestType) As BaseGetPartyResponseType
        Const ACMethodName As String = "GetParty"

        Dim oGetPartyResponse As New BaseImplementationTypes.BaseGetPartyResponseType
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim STSError As New STSErrorPublisher
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim oAgentRequest As AgentImplementationTypes.GetPartyRequestType = Nothing
        Dim oSAMForBrokingRequest As SAMForBrokingImplementationTypes.GetPartyRequestType = Nothing
        Dim bPerformSecurityCheck As Boolean
        Dim sUsername As String = String.Empty
        Dim oSAMForInsuranceRequest As SAMForInsuranceImplementationTypes.GetPartyRequestType = Nothing

        If GetPartyRequest.GetType Is GetType(AgentImplementationTypes.GetPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oGetPartyResponse = New AgentImplementationTypes.GetPartyResponseType
            oAgentRequest = DirectCast(GetPartyRequest, AgentImplementationTypes.GetPartyRequestType)
        ElseIf GetPartyRequest.GetType Is GetType(BaseImplementationTypes.BaseGetPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            oGetPartyResponse = New BaseImplementationTypes.BaseGetPartyResponseType
        ElseIf GetPartyRequest.GetType Is GetType(CustomerImplementationTypes.GetPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.CustomersPackage
            oGetPartyResponse = New CustomerImplementationTypes.GetPartyResponseType
        ElseIf GetPartyRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.GetPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oGetPartyResponse = New SAMForInsuranceImplementationTypes.GetPartyResponseType
            oSAMForInsuranceRequest = DirectCast(GetPartyRequest, SAMForInsuranceImplementationTypes.GetPartyRequestType)
        ElseIf GetPartyRequest.GetType Is GetType(SAMForBrokingImplementationTypes.GetPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage
            oGetPartyResponse = New SAMForBrokingImplementationTypes.GetPartyResponseType
            oSAMForBrokingRequest = DirectCast(GetPartyRequest, SAMForBrokingImplementationTypes.GetPartyRequestType)
        ElseIf GetPartyRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetPartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oGetPartyResponse = New SAMForInsuranceV2ImplementationTypes.GetPartyResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        If GetPartyRequest.BranchCode = "" Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        If GetPartyRequest.PartyKey = 0 Then
            STSError.AddInvalidField("PartyKey", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "PartyKey"), "")
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oGetPartyResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oGetPartyResponse
        End If

        Dim iSourceID As Int32
        iSourceID% = 1
        ' Convert branch code to ID
        Try
            iSourceID% = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", GetPartyRequest.BranchCode)
        Catch ex As Exception
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BranchCode"), GetPartyRequest.BranchCode)
        End Try

        If STSError.HasErrors Then
            STSError.SetContext(oGetPartyResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Lookup field validation", True)
            Return oGetPartyResponse
        End If

        If nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage Then
            If oSAMForInsuranceRequest.AgentKey <> 0 Then
            sUsername = oSAMForInsuranceRequest.UserName
            bPerformSecurityCheck = True
            End If
        ElseIf nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
            sUsername = oAgentRequest.UserName
            bPerformSecurityCheck = True
        ElseIf nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage Then
            If CoreBusiness.PartyIsAgent(oSAMForBrokingRequest.UserPartyType) Then
                sUsername = oSAMForBrokingRequest.UserName
                bPerformSecurityCheck = True
            End If
        End If

        If bPerformSecurityCheck Then
            If (oCoreBusiness.AgentSecurityCheck(sUsername, GetPartyRequest.PartyKey, PMEEntityType.Party) = False) Then
                STSError = New STSErrorPublisher(STSErrorCodes.SecurityCheckFailed, "Security check failed", sUsername & " does not have permission to access party " & GetPartyRequest.PartyKey)
                STSError.SetContext(oGetPartyResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetParty", True)
                Return oGetPartyResponse
            End If
        End If

        'GetPartyType (PC or CC)
        Dim dr As DataSet = Nothing
        'Dim iRet As Int32
        Dim sPartyType As String = String.Empty

        ' BSJ April 09 - SQL Mixed Mode Compliance
        Using Con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_Party_Type_For_Party")

                cmd.AddInParameter("@PartyCnt", SqlDbType.Int, 10).Value = GetPartyRequest.PartyKey
                dr = Con.ExecuteDataSet(cmd, "dr")
            End Using

        If dr.Tables(0).Rows.Count > 0 Then
            With dr.Tables(0).Rows(0)
                    sPartyType = .Item("Code").ToString().Trim()
            End With
        Else
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.SQLServerReturnedAnError, "No Data Returned for PartyType", "")
            STSErrorEx.SetContext(oGetPartyResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
            Return oGetPartyResponse
        End If

        ' if this is the sam for insurance package
        If nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage Or _
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then

            ' if this party type is OTHER
                If sPartyType.StartsWith("OT") OrElse sPartyType.StartsWith("AG") OrElse sPartyType.StartsWith("IN") Then

                ' get the party data
                ProcessGetPartyData(oCoreBusiness, GetPartyRequest, oGetPartyResponse, sPartyType)
            End If

        End If

        ' Get Extra party details required using SAM stored proc. TODO eventually add
        ' all other required party fields to SAM SP rather than calling all other
        ' SPs individually.

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_get_party")

                cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = GetPartyRequest.PartyKey

                dr = Con.ExecuteDataSet(cmd, "dr")
            End Using

        If Trim(sPartyType) = "PC" Then
            Dim oPartyPC As New BasePartyPCType

            ' Store the stuff we got from the SAM SP
            If dr.Tables(0).Rows.Count > 0 Then
                With oPartyPC
                    .AccountExecutive = dr.Tables(0).Rows(0).Item("AccountExec").ToString

                    .AccountExecutiveCode = dr.Tables(0).Rows(0).Item("AccountExecCode").ToString

                    .Currency = dr.Tables(0).Rows(0).Item("PartyCurrency").ToString
                    '.Agent = dr.Tables(0).Rows(0).Item("Agent").ToString
                End With
            End If

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_addresses_for_party")

                    cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = GetPartyRequest.PartyKey
                    dr = Con.ExecuteDataSet(cmd, "dr")
                End Using

            With dr.Tables(0)
                Dim oAddressResponse As New BaseGetAddressResponseType
                Dim oAddressRequest As New BaseGetAddressRequestType
                Dim oAddressType As AddressTypeType
                ReDim oPartyPC.Addresses(.Rows.Count - 1)
                For iCnt As Integer = 0 To .Rows.Count - 1
                    oAddressResponse = New BaseGetAddressResponseType
                    oAddressType = SAMFunc.GetAddressTypeCode(GetListCodeFromID(Core.STSListType.PMLookup, "Address_usage_type", Cast.ToInt32(.Rows(iCnt).Item("address_usage_type_id"), 0)))

                    oAddressRequest.AddressKey = Cast.ToInt32(.Rows(iCnt).Item("address_cnt"), 0)
                    oAddressRequest.BranchCode = GetPartyRequest.BranchCode
                    oAddressResponse = GetAddress(oAddressRequest)
                    oAddressResponse.Address.AddressTypeCode = oAddressType

                    oPartyPC.Addresses(iCnt) = oAddressResponse.Address
                Next
            End With

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_contacts_for_party")

                    cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = GetPartyRequest.PartyKey

                    dr = Con.ExecuteDataSet(cmd, "dr")
                End Using

            'Modification For PN 42880 To Filter out Main Contact Type
            Dim dataViewPC As DataView = New DataView
            With dataViewPC
                .Table = dr.Tables(0)
                .RowFilter = "contacttypecode <> 'MAIN'"
            End With
            With dataViewPC.ToTable
                ReDim oPartyPC.Contacts(.Rows.Count - 1)
                Dim oContact As BaseContactDetailType
                For iCnt As Integer = 0 To .Rows.Count - 1
                    oContact = New BaseContactDetailType

                    oPartyPC.Contacts(iCnt) = New BaseContactType
                    oPartyPC.Contacts(iCnt).AreaCode = .Rows(iCnt).Item("Area_Code").ToString
                    oContact.Item = .Rows(iCnt).Item("number").ToString
                    oPartyPC.Contacts(iCnt).ContactDetail = oContact
                    oPartyPC.Contacts(iCnt).ContactTypeId = Cast.ToInt32(.Rows(iCnt).Item("Contact_Type_id"), 0) ' Vivek: 20080704 - added the missing item to fix bug in existing system (Amend Client)
                    oPartyPC.Contacts(iCnt).Description = Cast.ToString(.Rows(iCnt).Item("description")) ' Vivek: 20080704 - added the missing item to fix bug in existing system (Amend Client)
                        Select Case Trim(.Rows(iCnt).Item("ContactTypeCode").ToString)
                            Case "E-MAIL"
                                oPartyPC.Contacts(iCnt).ContactTypeCode = ContactTypeType.EMAIL
                                oContact.ItemElementName = ItemChoiceType.EmailAddress
                            Case "MEMAIL"
                                oPartyPC.Contacts(iCnt).ContactTypeCode = ContactTypeType.MAINEMAILCONTACT
                                oContact.ItemElementName = ItemChoiceType.EmailAddress
                            Case "HOMEPHONE"
                                oPartyPC.Contacts(iCnt).ContactTypeCode = ContactTypeType.HOMEPHONE
                                oContact.ItemElementName = ItemChoiceType.Number
                            Case "MOBILE"
                                oPartyPC.Contacts(iCnt).ContactTypeCode = ContactTypeType.MOBILE
                                oContact.ItemElementName = ItemChoiceType.Number
                            Case "FAX"
                                oPartyPC.Contacts(iCnt).ContactTypeCode = ContactTypeType.FAX
                                oContact.ItemElementName = ItemChoiceType.Number
                            Case "WEB"
                                oPartyPC.Contacts(iCnt).ContactTypeCode = ContactTypeType.WEB
                                oContact.ItemElementName = ItemChoiceType.EmailAddress
                            Case "MAIN"
                                oPartyPC.Contacts(iCnt).ContactTypeCode = ContactTypeType.MAIN
                            Case "TELEPHONE"
                                oPartyPC.Contacts(iCnt).ContactTypeCode = ContactTypeType.TELEPHONE
                                oContact.ItemElementName = ItemChoiceType.Number
                            Case "LETTER"
                                oPartyPC.Contacts(iCnt).ContactTypeCode = ContactTypeType.LETTER
                                oContact.ItemElementName = ItemChoiceType.Number
                            Case "OTHER"
                                oPartyPC.Contacts(iCnt).ContactTypeCode = ContactTypeType.OTHER
                                oContact.ItemElementName = ItemChoiceType.Number
                                oPartyPC.Contacts(iCnt).OtherContactTypeCode = Cast.ToString(.Rows(iCnt).Item("ContactTypeCode").ToString)
                        End Select
                        If Trim(dataViewPC.ToTable.Rows(iCnt).Item("ContactTypeCode").ToString()) = "OTHER" Then
                            oPartyPC.Contacts(iCnt).ContactTypeDescription = "OTHER"
                        Else
                            oPartyPC.Contacts(iCnt).ContactTypeDescription = Cast.ToString(.Rows(iCnt).Item("ContactTypeDescription").ToString)
                        End If

                        If Trim(dataViewPC.ToTable.Rows(iCnt).Item("ContactTypeCode").ToString()) = "OTHER" Then
                            oPartyPC.Contacts(iCnt).OtherContactTypeCode = "OTHER"
                        Else
                            oPartyPC.Contacts(iCnt).OtherContactTypeCode = Cast.ToString(.Rows(iCnt).Item("ContactTypeCode").ToString)
                        End If

                    oPartyPC.Contacts(iCnt).Extension = Cast.ToString(.Rows(iCnt).Item("extension")) 'Sankar: 200809-22 - Added as per discussion with Rahul
                Next
            End With

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Party_sel")

                    cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = GetPartyRequest.PartyKey

                    dr = Con.ExecuteDataSet(cmd, "dr")
                End Using

            If dr.Tables(0).Rows.Count > 0 Then
                With oPartyPC
                    'Party
                    .AlternativeId = dr.Tables(0).Rows(0).Item("alternative_identifier").ToString
                    .Surname = dr.Tables(0).Rows(0).Item("name").ToString
                    .BranchCode = GetListCodeFromID(Core.STSListType.PMLookup, "source", Cast.ToInt32(dr.Tables(0).Rows(0).Item("source_id"), 0))
                    .FileCode = dr.Tables(0).Rows(0).Item("file_code").ToString

                    ' missing in Tech Spec
                    .TradingName = dr.Tables(0).Rows(0).Item("Trading_Name").ToString
                    .Currency = GetListCodeFromID(Core.STSListType.PMLookup, "currency", Cast.ToInt32(dr.Tables(0).Rows(0).Item("currency_id"), 0))

                    .SubBranchCode = GetListCodeFromID(Core.STSListType.PMLookup, "sub_branch", Cast.ToInt32(dr.Tables(0).Rows(0).Item("sub_branch_id"), 0))
                    Dim lBranchId As Integer
                    lBranchId = Cast.ToInt32(dr.Tables(0).Rows(0).Item("source_id"), 0)
                    Dim oClientAccountDetails As New BaseClientSharedDataType
                    Dim oPartyProspect As New BaseClientSharedDataType

                    Dim oPartyClientdet As New BaseClientSharedDataType
                    GetPartyDetails(dr, oCoreBusiness, oPartyClientdet)
                    .ClientDetail = oPartyClientdet
                    .ClientDetail.ShortName = dr.Tables(0).Rows(0).Item("shortname").ToString
                        .ClientDetail.ResolvedName = dr.Tables(0).Rows(0).Item("resolved_name").ToString
                        oPartyProspect = GetPartyProspect(Con, GetPartyRequest.PartyKey)
                        If oPartyProspect IsNot Nothing Then

                            .ClientDetail.AgentReference = oPartyProspect.AgentReference
                            .ClientDetail.CurrentIntermediaryKey = oPartyProspect.CurrentIntermediaryKey
                            .ClientDetail.CurrentIntermediaryKeySpecified = oPartyProspect.CurrentIntermediaryKeySpecified
                            .ClientDetail.StrengthCode = oPartyProspect.StrengthCode
                            .ClientDetail.StatusCode = oPartyProspect.StatusCode
                            .ClientDetail.PreviousInsurerKey = oPartyProspect.PreviousInsurerKey
                            .ClientDetail.PreviousInsurerKeySpecified = oPartyProspect.PreviousInsurerKeySpecified
                            .ClientDetail.PreviousBrokerKey = oPartyProspect.PreviousBrokerKey
                            .ClientDetail.PreviousBrokerKeySpecified = oPartyProspect.PreviousBrokerKeySpecified
                            .ClientDetail.CurrentIntermediaryName = oPartyProspect.CurrentIntermediaryName
                            .ClientDetail.PreviousBrokerCode = oPartyProspect.PreviousBrokerCode
                            .ClientDetail.PreviousBrokerName = oPartyProspect.PreviousBrokerName
                            .ClientDetail.PreviousInsurerCode = oPartyProspect.PreviousInsurerCode
                            .ClientDetail.PreviousInsurerName = oPartyProspect.PreviousInsurerName
                            .ClientDetail.PreviousBrokerName = oPartyProspect.PreviousBrokerName
                        End If
                        oClientAccountDetails = GetClientAccountDetails(Con, lBranchId, oPartyPC.BranchCode, GetPartyRequest.PartyKey)

                    If (oClientAccountDetails IsNot Nothing) Then
                        .ClientDetail.AccountBalance = oClientAccountDetails.AccountBalance
                        .ClientDetail.LastYearTurnover = oClientAccountDetails.LastYearTurnover
                        .ClientDetail.YearToDateTurnover = oClientAccountDetails.YearToDateTurnover
                        If oClientAccountDetails.AccountBalance = 0 Then
                            .ClientDetail.AccountBalanceSpecified = False
                        Else
                            .ClientDetail.AccountBalanceSpecified = True
                        End If
                        If oClientAccountDetails.LastYearTurnover = 0 Then
                            .ClientDetail.LastYearTurnoverSpecified = False
                        Else
                            .ClientDetail.LastYearTurnoverSpecified = True
                        End If
                        If oClientAccountDetails.YearToDateTurnover = 0 Then
                            .ClientDetail.YearToDateTurnoverSpecified = False
                        Else
                            .ClientDetail.YearToDateTurnoverSpecified = True
                        End If
                    End If

                End With
            End If

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Party_lifestyle_sel")

                    cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = GetPartyRequest.PartyKey
                    cmd.AddInParameter("@party_lifestyle_id", SqlDbType.Int).Value = 1

                    dr = Con.ExecuteDataSet(cmd, "dr")
                End Using

            If dr.Tables(0).Rows.Count > 0 Then
                With oPartyPC
                    'lifestyle
                    .DateOfBirth = Cast.ToDateTime(dr.Tables(0).Rows(0).Item("date_of_birth"), Date.MinValue)
                    .DateOfBirthSpecified = (.DateOfBirth <> New Date)
                    .GenderCode = dr.Tables(0).Rows(0).Item("gender_code").ToString
                    .OccupationCode = dr.Tables(0).Rows(0).Item("occupation_code").ToString

                    ' missing in Tech Spec
                    .SecOccupationCode = dr.Tables(0).Rows(0).Item("secondary_occupation_code").ToString

                End With
            End If

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Party_Net_Data_sel")

                    cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = GetPartyRequest.PartyKey

                    dr = Con.ExecuteDataSet(cmd, "dr")
                End Using

            If dr.Tables(0).Rows.Count > 0 Then
                With oPartyPC
                    'NetData
                    .TPIntroducer = dr.Tables(0).Rows(0).Item("tp_introducer_code").ToString
                    .TPUserCode = dr.Tables(0).Rows(0).Item("tp_user_code").ToString
                End With
            End If

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Party_Personal_Client_sel")

                    cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = GetPartyRequest.PartyKey

                    dr = Con.ExecuteDataSet(cmd, "dr")
                End Using

            If dr.Tables(0).Rows.Count > 0 Then
                With oPartyPC
                    '.Addresses
                    '.Contacts
                    .EmployersBusinessCode = dr.Tables(0).Rows(0).Item("employer_business").ToString
                    .EmploymentStatusCodeSpecified = True
                    Select Case Trim(dr.Tables(0).Rows(0).Item("employment_status_code").ToString)
                        Case "Company"
                            .EmploymentStatusCode = EmploymentStatusCodeType.Company
                        Case "Employed"
                            .EmploymentStatusCode = EmploymentStatusCodeType.Employed
                        Case "Household"
                            .EmploymentStatusCode = EmploymentStatusCodeType.HouseholdDuties
                        Case "Independent"
                            .EmploymentStatusCode = EmploymentStatusCodeType.IndependentMeans
                        Case "Education"
                            .EmploymentStatusCode = EmploymentStatusCodeType.InFullOrPartTimeEducation
                        Case "Disability"
                            .EmploymentStatusCode = EmploymentStatusCodeType.NotEmployedDueToDisability
                        Case "Retired"
                            .EmploymentStatusCode = EmploymentStatusCodeType.Retired
                        Case "SelfEmployed"
                            .EmploymentStatusCode = EmploymentStatusCodeType.SelfEmployed
                        Case "Unemployed"
                            .EmploymentStatusCode = EmploymentStatusCodeType.Unemployed
                        Case "Voluntary"
                            .EmploymentStatusCode = EmploymentStatusCodeType.VoluntaryWork
                        Case Else
                            .EmploymentStatusCodeSpecified = False
                    End Select
                    .Forename = dr.Tables(0).Rows(0).Item("forename").ToString
                    .Initials = dr.Tables(0).Rows(0).Item("initials").ToString
                    '.MaritalStatusCode = dr.Tables(0).Rows(0).Item("marital_status_code").ToString
                    .MaritalStatusCodeSpecified = True

                    Select Case dr.Tables(0).Rows(0).Item("marital_status_code").ToString
                        Case "Divorced"
                            .MaritalStatusCode = MaritalStatusCodeType.Divorced
                        Case "Married"
                            .MaritalStatusCode = MaritalStatusCodeType.Married

                        Case "Married - Common Law"
                            .MaritalStatusCode = MaritalStatusCodeType.MarriedCommonLaw
                        Case "Not Applicable"
                            .MaritalStatusCode = MaritalStatusCodeType.NotApplicable
                        Case "Not Available"
                            .MaritalStatusCode = MaritalStatusCodeType.NotAvailable

                        Case "Partnered"
                            .MaritalStatusCode = MaritalStatusCodeType.Partnered
                        Case "Separated"
                            .MaritalStatusCode = MaritalStatusCodeType.Separated
                        Case "Single"
                            .MaritalStatusCode = MaritalStatusCodeType.Single_
                        Case "Widowed"
                            .MaritalStatusCode = MaritalStatusCodeType.Widowed
                        Case Else
                            .MaritalStatusCodeSpecified = False
                    End Select

                    .Title = dr.Tables(0).Rows(0).Item("party_title_code").ToString

                    ' missing in Tech Spec
                    .SecEmployersBusinessCode = dr.Tables(0).Rows(0).Item("secondary_employer_business").ToString

                    ' Got the following values from Gaurav for Secondary Employment Status Code
                    '2228230    Company                            C
                    '2228230    Employed                           E
                    '2228230    Household Duties                   H
                    '2228230    In Full Or Part Time Education     F
                    '2228230    Independent Means                  I
                    '2228230    Not Employed Due To Disability     N
                    '2228230    Retired                            R
                    '2228230    Self Employed                      S
                    '2228230    Unemployed                         U
                    '2228230    Voluntary Work                     V

                    ' Vivek: first we are setting it to true if there is some value (any value) in DB
                    .SecEmploymentStatusCodeSpecified = Not IsDBNull(dr.Tables(0).Rows(0).Item("secondary_employment_status_co"))
                    Select Case dr.Tables(0).Rows(0).Item("secondary_employment_status_co").ToString
                        Case "Company"
                            .SecEmploymentStatusCode = EmploymentStatusCodeType.Company
                        Case "Employed"
                            .SecEmploymentStatusCode = EmploymentStatusCodeType.Employed
                        Case "Household Duties"
                            .SecEmploymentStatusCode = EmploymentStatusCodeType.HouseholdDuties
                        Case "Independent Means"
                            .SecEmploymentStatusCode = EmploymentStatusCodeType.IndependentMeans
                        Case "In Full Or Part Time Education"
                            .SecEmploymentStatusCode = EmploymentStatusCodeType.InFullOrPartTimeEducation
                        Case "Not Employed Due To Disability"
                            .SecEmploymentStatusCode = EmploymentStatusCodeType.NotEmployedDueToDisability
                        Case "Retired"
                            .SecEmploymentStatusCode = EmploymentStatusCodeType.Retired
                        Case "Self Employed"
                            .SecEmploymentStatusCode = EmploymentStatusCodeType.SelfEmployed
                        Case "Unemployed"
                            .SecEmploymentStatusCode = EmploymentStatusCodeType.Unemployed
                        Case "Voluntary Work"
                            .SecEmploymentStatusCode = EmploymentStatusCodeType.VoluntaryWork
                        Case Else
                            ' Vivek: if the value is not a valid value we are setting it to false
                            .SecEmploymentStatusCodeSpecified = False
                    End Select

                    If Cast.ToInt32(dr.Tables(0).Rows(0).Item("Nationality_id"), 0) = 0 Then
                        .NationalityCode = Nothing
                    Else
                        .NationalityCode = GetListCodeFromID(Core.STSListType.PMLookup, "Nationality", Cast.ToInt32(dr.Tables(0).Rows(0).Item("Nationality_id"), 0))
                    End If
                    .AccommodationCode = dr.Tables(0).Rows(0).Item("accommodation_type_code").ToString
                    .Salutation = dr.Tables(0).Rows(0).Item("salutation").ToString
                    .Source = dr.Tables(0).Rows(0).Item("source").ToString

                    .TPS = Cast.ToBoolean(dr.Tables(0).Rows(0).Item("tpsind"), False)
                    .TPSSpecified = Not IsDBNull(dr.Tables(0).Rows(0).Item("tpsind"))

                    .MPS = Cast.ToBoolean(dr.Tables(0).Rows(0).Item("mailshot"), False)
                    .MPSSpecified = Not IsDBNull(dr.Tables(0).Rows(0).Item("mailshot"))

                    .eMPS = Cast.ToBoolean(dr.Tables(0).Rows(0).Item("empsind"), False)
                    .eMPSSpecified = Not IsDBNull(dr.Tables(0).Rows(0).Item("empsind"))

                    .PetOwner = Cast.ToBoolean(dr.Tables(0).Rows(0).Item("is_pet_owner"), False)
                    .PetOwnerSpecified = Not IsDBNull(dr.Tables(0).Rows(0).Item("is_pet_owner"))

                End With
            End If

            oGetPartyResponse.Item = oPartyPC

            ' added this call as suggested by Gaurav
            GetPartyAdditionalData(GetPartyRequest, oGetPartyResponse, "PC")

        ElseIf Trim(sPartyType) = "CC" Then

            Dim oPartyCC As New BasePartyCCType

            ' Store the stuff we got from the SAM SP
            If dr.Tables(0).Rows.Count > 0 Then
                With oPartyCC
                    .AccountExecutive = dr.Tables(0).Rows(0).Item("AccountExec").ToString

                    .AccountExecutiveCode = dr.Tables(0).Rows(0).Item("AccountExecCode").ToString

                    .Currency = dr.Tables(0).Rows(0).Item("PartyCurrency").ToString
                    .NumberOfOffices = Cast.ToInt32(dr.Tables(0).Rows(0).Item("NoOfOffices"), 0)

                    If (.NumberOfOffices > 0) Then
                        .NumberOfOfficesSpecified = True
                    End If

		    If Cast.ToInt32(dr.Tables(0).Rows(0).Item("NoOfEmployees"), 0) = 0 Then
                        .NumberOfEmployees = Nothing
                    Else
                        .NumberOfEmployees = GetListCodeFromID(Core.STSListType.PMLookup, "Employeeband", Cast.ToInt32(dr.Tables(0).Rows(0).Item("NoOfEmployees"), 0))
                    End If
                    .Agent = dr.Tables(0).Rows(0).Item("Agent").ToString
                End With
            End If

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_addresses_for_party")

                    cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = GetPartyRequest.PartyKey

                    dr = Con.ExecuteDataSet(cmd, "dr")
                End Using

            With dr.Tables(0)
                Dim oAddressResponse As New BaseGetAddressResponseType
                Dim oAddressRequest As New BaseGetAddressRequestType
                Dim oAddressType As AddressTypeType
                ReDim oPartyCC.Addresses(.Rows.Count - 1)
                For iCnt As Integer = 0 To .Rows.Count - 1
                    oAddressResponse = New BaseGetAddressResponseType
                    oAddressType = SAMFunc.GetAddressTypeCode(GetListCodeFromID(Core.STSListType.PMLookup, "Address_usage_type", Cast.ToInt32(.Rows(iCnt).Item("address_usage_type_id"), 0)))

                    oAddressRequest.AddressKey = Cast.ToInt32(.Rows(iCnt).Item("address_cnt"), 0)
                    oAddressRequest.BranchCode = GetPartyRequest.BranchCode
                    oAddressResponse = GetAddress(oAddressRequest)
                    oAddressResponse.Address.AddressTypeCode = oAddressType

                    oPartyCC.Addresses(iCnt) = oAddressResponse.Address
                Next
            End With

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_contacts_for_party")

                    cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = GetPartyRequest.PartyKey

                    dr = Con.ExecuteDataSet(cmd, "dr")
                End Using

            'Modification For PN 42880 To Filter out Main Contact Type
            Dim dataViewCC As DataView = New DataView
            With dataViewCC
                .Table = dr.Tables(0)
                .RowFilter = "contacttypecode <> 'MAIN'"
            End With

            With dataViewCC.ToTable
                ReDim oPartyCC.Contacts(.Rows.Count - 1)
                Dim oContact As BaseContactDetailType
                For iCnt As Integer = 0 To .Rows.Count - 1
                    oContact = New BaseContactDetailType

                    oPartyCC.Contacts(iCnt) = New BaseContactType
                    oPartyCC.Contacts(iCnt).AreaCode = .Rows(iCnt).Item("Area_Code").ToString
                    oPartyCC.Contacts(iCnt).ContactTypeId = Cast.ToInt32(.Rows(iCnt).Item("Contact_Type_id"), 0) ' Vivek: 20080704 - added the missing item to fix bug in existing system (Amend Client)
                    oPartyCC.Contacts(iCnt).Description = Cast.ToString(.Rows(iCnt).Item("description")) ' Vivek: 20080704 - added the missing item to fix bug in existing system (Amend Client)

                    oContact.Item = .Rows(iCnt).Item("number").ToString
                        Select Case Trim(.Rows(iCnt).Item("ContactTypeCode").ToString)
                            Case "E-MAIL"
                                oPartyCC.Contacts(iCnt).ContactTypeCode = ContactTypeType.EMAIL
                                oContact.ItemElementName = ItemChoiceType.EmailAddress
                            Case "MEMAIL"
                                oPartyCC.Contacts(iCnt).ContactTypeCode = ContactTypeType.MAINEMAILCONTACT
                                oContact.ItemElementName = ItemChoiceType.EmailAddress
                            Case "HOMEPHONE"
                                oPartyCC.Contacts(iCnt).ContactTypeCode = ContactTypeType.HOMEPHONE
                                oContact.ItemElementName = ItemChoiceType.Number
                            Case "MOBILE"
                                oPartyCC.Contacts(iCnt).ContactTypeCode = ContactTypeType.MOBILE
                                oContact.ItemElementName = ItemChoiceType.Number
                            Case "FAX"
                                oPartyCC.Contacts(iCnt).ContactTypeCode = ContactTypeType.FAX
                                oContact.ItemElementName = ItemChoiceType.Number
                            Case "WEB"
                                oPartyCC.Contacts(iCnt).ContactTypeCode = ContactTypeType.WEB
                                oContact.ItemElementName = ItemChoiceType.EmailAddress
                            Case "TELEPHONE"
                                oPartyCC.Contacts(iCnt).ContactTypeCode = ContactTypeType.TELEPHONE
                                oContact.ItemElementName = ItemChoiceType.Number
                            Case "MAIN"
                                oPartyCC.Contacts(iCnt).ContactTypeCode = ContactTypeType.MAIN
                                oContact.ItemElementName = ItemChoiceType.Number
                            Case "LETTER"
                                oPartyCC.Contacts(iCnt).ContactTypeCode = ContactTypeType.LETTER
                                oContact.ItemElementName = ItemChoiceType.Number
                            Case "OTHER"
                                oPartyCC.Contacts(iCnt).ContactTypeCode = ContactTypeType.OTHER
                                oContact.ItemElementName = ItemChoiceType.Number
                                oPartyCC.Contacts(iCnt).OtherContactTypeCode = Cast.ToString(.Rows(iCnt).Item("ContactTypeCode").ToString)
                        End Select

                        If Trim(dataViewCC.ToTable.Rows(iCnt).Item("ContactTypeCode").ToString()) = "OTHER" Then
                            oPartyCC.Contacts(iCnt).ContactTypeDescription = "OTHER"
                        Else
                            oPartyCC.Contacts(iCnt).ContactTypeDescription = Cast.ToString(.Rows(iCnt).Item("ContactTypeDescription").ToString)
                        End If

                        oPartyCC.Contacts(iCnt).ContactDetail = oContact
                    oPartyCC.Contacts(iCnt).Extension = Cast.ToString(.Rows(iCnt).Item("extension")) 'Sankar: 200809-22 - Added as per discussion with Rahul
                Next
            End With

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Party_sel")

                    cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = GetPartyRequest.PartyKey

                    dr = Con.ExecuteDataSet(cmd, "dr")
                End Using

            If dr.Tables(0).Rows.Count > 0 Then
                With oPartyCC
                    'Party
                    .BranchCode = GetListCodeFromID(Core.STSListType.PMLookup, "source", Cast.ToInt32(dr.Tables(0).Rows(0).Item("source_id"), 0))
                    .CompanyName = dr.Tables(0).Rows(0).Item("Name").ToString
                    .FileCode = dr.Tables(0).Rows(0).Item("file_code").ToString

                    ' missing in Tech Spec
	            .AlternativeId = dr.Tables(0).Rows(0).Item("alternative_identifier").ToString
                    .Currency = GetListCodeFromID(Core.STSListType.PMLookup, "currency", Cast.ToInt32(dr.Tables(0).Rows(0).Item("currency_id"), 0))

                    .SubBranchCode = GetListCodeFromID(Core.STSListType.PMLookup, "Sub_Branch", Cast.ToInt32(dr.Tables(0).Rows(0).Item("sub_branch_id"), 0))
                    Dim lBranchId As Integer
                    lBranchId = Cast.ToInt32(dr.Tables(0).Rows(0).Item("source_id"), 0)
                    Dim oClientAccountDetails As New BaseClientSharedDataType
                    Dim oPartyProspect As New BaseClientSharedDataType

                    Dim oPartyClientdet As New BaseClientSharedDataType
                    GetPartyDetails(dr, oCoreBusiness, oPartyClientdet)
                    .ClientDetail = oPartyClientdet
                        .ClientDetail.ShortName = dr.Tables(0).Rows(0).Item("shortname").ToString

                        oPartyProspect = GetPartyProspect(Con, GetPartyRequest.PartyKey)
                        If oPartyProspect IsNot Nothing Then
                        .ClientDetail.AgentReference = oPartyProspect.AgentReference
                        .ClientDetail.CurrentIntermediaryKey = oPartyProspect.CurrentIntermediaryKey
                        .ClientDetail.CurrentIntermediaryKeySpecified = oPartyProspect.CurrentIntermediaryKeySpecified
                        .ClientDetail.StrengthCode = oPartyProspect.StrengthCode
                        .ClientDetail.StatusCode = oPartyProspect.StatusCode
                        .ClientDetail.PreviousInsurerKey = oPartyProspect.PreviousInsurerKey
                        .ClientDetail.PreviousInsurerKeySpecified = oPartyProspect.PreviousInsurerKeySpecified
                        .ClientDetail.PreviousBrokerKey = oPartyProspect.PreviousBrokerKey
                        .ClientDetail.PreviousBrokerKeySpecified = oPartyProspect.PreviousBrokerKeySpecified
                        .ClientDetail.CurrentIntermediaryName = oPartyProspect.CurrentIntermediaryName
                        .ClientDetail.PreviousBrokerCode = oPartyProspect.PreviousBrokerCode
                        .ClientDetail.PreviousBrokerName = oPartyProspect.PreviousBrokerName
                        .ClientDetail.PreviousInsurerCode = oPartyProspect.PreviousInsurerCode
                        .ClientDetail.PreviousInsurerName = oPartyProspect.PreviousInsurerName
                        .ClientDetail.PreviousBrokerName = oPartyProspect.PreviousBrokerName
                        End If
                        oClientAccountDetails = GetClientAccountDetails(Con, lBranchId, oPartyCC.BranchCode, GetPartyRequest.PartyKey)

                    If oClientAccountDetails IsNot Nothing Then
                        .ClientDetail.AccountBalance = oClientAccountDetails.AccountBalance
                        .ClientDetail.LastYearTurnover = oClientAccountDetails.LastYearTurnover
                        .ClientDetail.YearToDateTurnover = oClientAccountDetails.YearToDateTurnover
                        If oClientAccountDetails.AccountBalance = 0 Then
                            .ClientDetail.AccountBalanceSpecified = False
                        Else
                            .ClientDetail.AccountBalanceSpecified = True
                        End If
                        If oClientAccountDetails.LastYearTurnover = 0 Then
                            .ClientDetail.LastYearTurnoverSpecified = False
                        Else
                            .ClientDetail.LastYearTurnoverSpecified = True
                        End If
                        If oClientAccountDetails.YearToDateTurnover = 0 Then
                            .ClientDetail.YearToDateTurnoverSpecified = False
                        Else
                            .ClientDetail.YearToDateTurnoverSpecified = True
                        End If
                    End If

                End With
            End If

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_ClientsCorporate")

                    cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = GetPartyRequest.PartyKey

                    dr = Con.ExecuteDataSet(cmd, "dr")
                End Using

            If dr.Tables(0).Rows.Count > 0 Then
                With oPartyCC
                    oPartyCC.BusinessCode = dr.Tables(0).Rows(0).Item("Business").ToString
                    oPartyCC.TradeCode = dr.Tables(0).Rows(0).Item("Trade").ToString
                    oPartyCC.MainContact = dr.Tables(0).Rows(0).Item("MainContact").ToString
                End With
            End If

            ' missing in Tech Spec
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Party_Corporate_Client_sel")

                    cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = GetPartyRequest.PartyKey

                    dr = Con.ExecuteDataSet(cmd, "dr")
                End Using

            If dr.Tables(0).Rows.Count > 0 Then
                With oPartyCC
                    oPartyCC.CompanyReg = dr.Tables(0).Rows(0).Item("company_reg").ToString
                    oPartyCC.SICCode = dr.Tables(0).Rows(0).Item("SIC_code_id").ToString
                    'Chaged as per Gaurav's advice on 04-09-08
                    If Not (String.IsNullOrEmpty(oPartyCC.SICCode)) Then
                            oPartyCC.SICCode = oCoreBusiness.GetListItemFromID(Core.STSListType.PMLookup,
                        "SIC_code", Convert.ToInt32(oPartyCC.SICCode), "Code")
                    End If
                    oPartyCC.TurnoverCode = dr.Tables(0).Rows(0).Item("turnover").ToString
                    If Not (String.IsNullOrEmpty(oPartyCC.TurnoverCode)) Then
                        If (Convert.ToInt32(dr.Tables(0).Rows(0).Item("turnover")) > 0) Then
                                oPartyCC.TurnoverCode = oCoreBusiness.GetListItemFromID(Core.STSListType.PMLookup,
                            "TurnoverBand", CInt(oPartyCC.TurnoverCode), "Code")
                        Else
                            oPartyCC.TurnoverCode = ""
                        End If
                    End If
                    oPartyCC.Salutation = dr.Tables(0).Rows(0).Item("salutation").ToString
                    oPartyCC.Source = dr.Tables(0).Rows(0).Item("source").ToString

                    oPartyCC.TradingSince = Cast.ToDateTime(dr.Tables(0).Rows(0).Item("trading_since_date"), New Date)
                    oPartyCC.TradingSinceSpecified = Not IsDBNull(dr.Tables(0).Rows(0).Item("trading_since_date"))

                    oPartyCC.WageRoll = Cast.ToDecimal(dr.Tables(0).Rows(0).Item("wage_roll"), 0)
                    oPartyCC.WageRollSpecified = Not IsDBNull(dr.Tables(0).Rows(0).Item("wage_roll"))

                    oPartyCC.FinancialYear = Cast.ToDateTime(dr.Tables(0).Rows(0).Item("financial_year"), New Date)
                    oPartyCC.FinancialYearSpecified = Not IsDBNull(dr.Tables(0).Rows(0).Item("financial_year"))

                    oPartyCC.TPS = Cast.ToBoolean(dr.Tables(0).Rows(0).Item("tpsind"), False)
                    oPartyCC.TPSSpecified = Not IsDBNull(dr.Tables(0).Rows(0).Item("tpsind"))

                    oPartyCC.MPS = Cast.ToBoolean(dr.Tables(0).Rows(0).Item("mailshot"), False)
                    oPartyCC.MPSSpecified = Not IsDBNull(dr.Tables(0).Rows(0).Item("mailshot"))

                    oPartyCC.eMPS = Cast.ToBoolean(dr.Tables(0).Rows(0).Item("empsind"), False)
                    oPartyCC.eMPSSpecified = Not IsDBNull(dr.Tables(0).Rows(0).Item("empsind"))
                End With
            End If

            oGetPartyResponse.Item = oPartyCC

            ' added this call as suggested by Gaurav
            GetPartyAdditionalData(GetPartyRequest, oGetPartyResponse, "CC")

        End If

        If (Trim(sPartyType) = "PC" Or Trim(sPartyType) = "CC") Then

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Party_Details")

                    cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = GetPartyRequest.PartyKey

                    dr = Con.ExecuteDataSet(cmd, "dr")
                End Using

            If (dr.Tables(0).Rows.Count > 0) Then
                oGetPartyResponse.Item.TaxNumber = dr.Tables(0).Rows(0).Item("tax_number").ToString
                oGetPartyResponse.Item.DomiciledForTax = Cast.ToBoolean(dr.Tables(0).Rows(0).Item("domiciled_for_tax"), False)
                If (oGetPartyResponse.Item.DomiciledForTax) Then
                    oGetPartyResponse.Item.DomiciledForTaxSpecified = True
                End If
                oGetPartyResponse.Item.TaxExempt = Cast.ToBoolean(dr.Tables(0).Rows(0).Item("tax_exempt"), False)
                If (oGetPartyResponse.Item.TaxExempt) Then
                    oGetPartyResponse.Item.TaxExemptSpecified = True
                End If
                oGetPartyResponse.Item.TaxPercentage = Cast.ToDecimal(dr.Tables(0).Rows(0).Item("tax_percentage"), 0)
                If (oGetPartyResponse.Item.TaxPercentage > 0) Then
                    oGetPartyResponse.Item.TaxPercentageSpecified = True
                End If
                End If

                Dim arParams(0) As SqlParameter
                arParams(0) = New SqlParameter("@party_cnt", GetPartyRequest.PartyKey)
                dr = SqlHelper.ExecuteDataset(SAMFunc.ConnectionString, _
                           CommandType.StoredProcedure, "spu_get_client_business_summary", arParams)
                If (dr.Tables(0).Rows.Count > 0) Then
                    oGetPartyResponse.NoofPolicies = Cast.ToInt32(dr.Tables(0).Rows(0).Item("NoofPolicies"), 0)
                    oGetPartyResponse.NoofOpenClaims = Cast.ToInt32(dr.Tables(0).Rows(0).Item("NoofOpenClaims"), 0)
                    oGetPartyResponse.NoofClosedClaims = Cast.ToInt32(dr.Tables(0).Rows(0).Item("NoofCloseClaims"), 0)
                End If

            End If

        End Using

        '***********************************************************************
        ' get risk data 
        If nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage Or _
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then
            Dim PartyXBuilder As New CoreImplementation.XBuilder.Party(_SiriusUser)
            PartyXBuilder.GetDataSet(GetPartyRequest.BranchCode, GetPartyRequest.PartyKey, oGetPartyResponse.Item.XMLDataset)
        End If
        '***********************************************************************

        ' Get the Timestamp
        Dim bIsLocked As Boolean
        Dim AnyError As STSErrorType
        AnyError = oCoreBusiness.GetTimestamp( _
                            GetPartyRequest.BranchCode, _
                            CoreBusiness.LockName.PartyCnt, _
                            GetPartyRequest.PartyKey, _
                            oGetPartyResponse.PartyTimestamp, _
                            bIsLocked)
        ' Return AnyErrors
        If AnyError Is Nothing = False Then
            oGetPartyResponse.STSError = AnyError
        End If

        'TypeCast the response Back.
        If nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
            Dim oResponse As New AgentImplementationTypes.GetPartyResponseType
            oResponse = DirectCast(oGetPartyResponse, AgentImplementationTypes.GetPartyResponseType)
            oGetPartyResponse = oResponse
        End If

        Return oGetPartyResponse
    End Function

    Public Function GetPartySummary(ByVal GetPartySummaryRequest As BaseGetPartySummaryRequestType) As BaseGetPartySummaryResponseType
        Const ACMethodName As String = "GetPartySummary"

        Const kSQL_SP_POLICY_LIST As String = "spu_SAM_Party_Policy_list"
        'Pure 3.0 ---- WPR 41
        Const kSQL_SP_POLICY_LIST_WITHOUT_VERSIONING As String = "spu_SAM_Party_Policy_list_Without_Versioning"
        Const kSQL_SP_CLAIM_LIST As String = "spu_SAM_Party_Claim_list"

        Dim oGetPartySummaryResponse As New BaseImplementationTypes.BaseGetPartySummaryResponseType
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim sStoredProc As String = String.Empty
        Dim STSError As New STSErrorPublisher
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim oAgentRequest As AgentImplementationTypes.GetPartySummaryRequestType = Nothing
        Dim oSAMForBrokingRequest As SAMForBrokingImplementationTypes.GetPartySummaryRequestType = Nothing
        Dim bPerformSecurityCheck As Boolean
        Dim sUsername As String = String.Empty
        Dim sOptionValue As String = String.Empty
        Dim lAgentKey As Integer = 0

        'Get QuoteVersioning from SystemOptions
        oCoreBusiness.GetSystemOption(GetPartySummaryRequest.BranchCode, _
                                    SystemOption.QuoteVersioning, _
                                    sOptionValue)
        If GetPartySummaryRequest.GetType Is GetType(AgentImplementationTypes.GetPartySummaryRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oGetPartySummaryResponse = New AgentImplementationTypes.GetPartySummaryResponseType
            oAgentRequest = DirectCast(GetPartySummaryRequest, AgentImplementationTypes.GetPartySummaryRequestType)
        ElseIf GetPartySummaryRequest.GetType Is GetType(CustomerImplementationTypes.GetPartySummaryRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.CustomersPackage
            oGetPartySummaryResponse = New CustomerImplementationTypes.GetPartySummaryResponseType
        ElseIf GetPartySummaryRequest.GetType Is GetType(BaseImplementationTypes.BaseGetPartySummaryRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            oGetPartySummaryResponse = New BaseImplementationTypes.BaseGetPartySummaryResponseType
        ElseIf GetPartySummaryRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.GetPartySummaryRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oGetPartySummaryResponse = New SAMForInsuranceImplementationTypes.GetPartySummaryResponseType
            Dim oSAMForInsuranceRequest As SAMForInsuranceImplementationTypes.GetPartySummaryRequestType = DirectCast(GetPartySummaryRequest, SAMForInsuranceImplementationTypes.GetPartySummaryRequestType)
            lAgentKey = CInt(oSAMForInsuranceRequest.AgentKey)
        ElseIf GetPartySummaryRequest.GetType Is GetType(SAMForBrokingImplementationTypes.GetPartySummaryRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage
            oGetPartySummaryResponse = New SAMForBrokingImplementationTypes.GetPartySummaryResponseType
            oSAMForBrokingRequest = DirectCast(GetPartySummaryRequest, SAMForBrokingImplementationTypes.GetPartySummaryRequestType)
        ElseIf GetPartySummaryRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetPartySummaryRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oGetPartySummaryResponse = New SAMForInsuranceV2ImplementationTypes.GetPartySummaryResponseType
            Dim oSAMForInsuranceV2Request As SAMForInsuranceV2ImplementationTypes.GetPartySummaryRequestType = DirectCast(GetPartySummaryRequest, SAMForInsuranceV2ImplementationTypes.GetPartySummaryRequestType)
            lAgentKey = CInt(oSAMForInsuranceV2Request.AgentKey)
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        If GetPartySummaryRequest.BranchCode = "" Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        If GetPartySummaryRequest.PartyKey = 0 Then
            STSError.AddInvalidField("PartyKey", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "PartyKey"), "")
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oGetPartySummaryResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oGetPartySummaryResponse
        End If

        Dim iSourceID As Int32
        iSourceID% = 1
        ' Convert branch code to ID
        Try
            iSourceID% = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", GetPartySummaryRequest.BranchCode)
        Catch ex As Exception
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BranchCode"), GetPartySummaryRequest.BranchCode)
        End Try

        If STSError.HasErrors Then
            STSError.SetContext(oGetPartySummaryResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Lookup field validation", True)
            Return oGetPartySummaryResponse
        End If

        If nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
            sUsername = oAgentRequest.UserName
            bPerformSecurityCheck = True
        ElseIf nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage Then
            If CoreBusiness.PartyIsAgent(oSAMForBrokingRequest.UserPartyType) Then
                sUsername = oSAMForBrokingRequest.UserName
                bPerformSecurityCheck = True
            End If
        End If

        If bPerformSecurityCheck Then
            If (oCoreBusiness.AgentSecurityCheck(sUsername, GetPartySummaryRequest.PartyKey, PMEEntityType.Party) = False) Then
                STSError = New STSErrorPublisher(STSErrorCodes.SecurityCheckFailed, "Security check failed", sUsername & " does not have permission to access party " & GetPartySummaryRequest.PartyKey)
                STSError.SetContext(oGetPartySummaryResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetParty", True)
                Return oGetPartySummaryResponse
            End If
        End If

        Dim oGetPartyRequest As New BaseGetPartyRequestType
        Dim oGetPartyResponse As BaseGetPartyResponseType
        oGetPartyRequest.BranchCode = GetPartySummaryRequest.BranchCode
        oGetPartyRequest.PartyKey = GetPartySummaryRequest.PartyKey

        oGetPartyResponse = GetParty(oGetPartyRequest)

        oGetPartySummaryResponse.Item = oGetPartyResponse.Item
        oGetPartySummaryResponse.PartyTimestamp = oGetPartyResponse.PartyTimestamp
        oGetPartySummaryResponse.STSError = oGetPartyResponse.STSError

        Dim dr As DataSet = Nothing

        ' BSJ April 09 - SQL Mixed Mode Compliance
        Using Con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            If Not String.IsNullOrEmpty(sOptionValue) AndAlso CType(sOptionValue, Integer) = 1 Then
                Using Cmd As SiriusCommand = SiriusCommand.FromProcedure(kSQL_SP_POLICY_LIST.ToString)

                    Cmd.AddInParameter("@nParty_Cnt", SqlDbType.Int).Value = GetPartySummaryRequest.PartyKey
                    If lAgentKey > 0 Then
                        Cmd.AddInParameter("@Agent_Key", SqlDbType.Int).Value = lAgentKey
                    Else
                        Cmd.AddInParameter("@Agent_Key", SqlDbType.Int).Value = DBNull.Value
                    End If
                    Cmd.AddInParameter("@nUserID", SqlDbType.Int).Value = _SiriusUser.UserID
                    dr = Con.ExecuteDataSet(Cmd, "dr")
                End Using
            Else
                'Pure 3.0 ---- WPR 41
                Using Cmd As SiriusCommand = SiriusCommand.FromProcedure(kSQL_SP_POLICY_LIST_WITHOUT_VERSIONING.ToString)

                    Cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = GetPartySummaryRequest.PartyKey
                    Cmd.AddInParameter("@user_id", SqlDbType.Int).Value = _SiriusUser.UserID
                    Cmd.AddInParameter("@RetrieveAssociates", SqlDbType.Int).Value = IIf(GetPartySummaryRequest.RetrieveAssociates = True, 1, 0)
                    dr = Con.ExecuteDataSet(Cmd, "dr")
                    If GetPartySummaryRequest.RetrieveAssociates Then
                        AddExtraXMLElementForPolicyAssociate(dr, "AssociatedClients")
                    End If
                End Using
            End If
            
            ' Name the dataset, table and columns
            dr.DataSetName = "BaseGetPartySummaryResponseTypePolicies"
            dr.Tables(0).TableName = "Row"

            If nTypeOfPackage <> enumTypeOfPackage.SamForBrokingPackage Then
                dr.Tables(0).Columns.Remove("PolicyVersion")
                dr.Tables(0).Columns.Remove("RiskCodeDescription")
            Else
                Dim oCol As New System.Data.DataColumn
                oCol.ColumnName = "MtaAllowed"
                oCol.DataType = System.Type.GetType("System.Boolean")
                dr.Tables(0).Columns.Add(oCol)

                'Check Policy is mta'able and set data to recordset (for serialization).
                Dim mtaAllowed As Boolean
                If dr.Tables.Count = 1 Then
                    Dim counter As Int32
                    For counter = 0 To dr.Tables(0).Rows.Count - 1
                        mtaAllowed = True

                        'Check Policy is 12-month
                        If IsDate(dr.Tables(0).Rows(counter).Item("CoverStartDate")) _
                            And IsDate(dr.Tables(0).Rows(counter).Item("ExpiryDate")) Then
                            If DateDiff(DateInterval.Month, Cast.ToDateTime(dr.Tables(0).Rows(counter).Item("CoverStartDate"), Date.MinValue), _
                                    Cast.ToDateTime(dr.Tables(0).Rows(counter).Item("ExpiryDate"), Date.MinValue)) <> 12 Then
                                mtaAllowed = False
                            End If
                        End If

                        'Check no future dated mta's outstanding
                        Dim ds As DataSet = Nothing

                        Using Cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_CheckFutureDatedMtas")

                            Cmd.AddInParameter("@insurance_folder_cnt", SqlDbType.Int).Value = dr.Tables(0).Rows(counter).Item("InsuranceFolderKey")
                            Cmd.AddInParameter("@Check_Date", SqlDbType.DateTime).Value = Now

                            ds = Con.ExecuteDataSet(Cmd, "ds")
                        End Using

                        If ds.Tables.Count = 1 Then
                            Try
                                If Not IsNothing(ds.Tables(0).Rows(0)) Then
                                    If CInt(ds.Tables(0).Rows(0).Item(0)) > 0 Then
                                        mtaAllowed = False
                                    End If
                                End If
                            Catch ex As Exception
                            End Try
                        End If

                        'Check Policy is the latest live version
                        If mtaAllowed Then
                            Dim currentInsuranceFileCnt As Int32
                            Dim latestInsFileCnt As Int32

                            Using Cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_GetCurrentInsuranceFile")

                                Cmd.AddInParameter("@insurance_folder_cnt", SqlDbType.Int).Value = dr.Tables(0).Rows(counter).Item("InsuranceFolderKey")

                                ds = Con.ExecuteDataSet(Cmd, "ds")
                            End Using

                            If ds.Tables.Count = 1 Then
                                Try
                                    If ds.Tables(0).Rows.Count > 0 Then
                                        currentInsuranceFileCnt = CInt(ds.Tables(0).Rows(0).Item("insurance_file_cnt").ToString)
                                        latestInsFileCnt = CInt(ds.Tables(0).Rows(0).Item(4).ToString)
                                    End If
                                Catch ex As Exception
                                    Dim STSErrorEx As New STSErrorPublisher("Failed to Get LatestInsurancefileCnt for Folder.", ex)
                                    STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "AddMtaQuote", True)
                                End Try
                            Else
                                'No Live Versions To Adjust
                                Dim STSErrorEx As New STSErrorPublisher(0, "No Live Policy Version to Adjust.")
                                STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "AddMtaQuote", True)
                            End If
                            If latestInsFileCnt <> Cast.ToInt32(dr.Tables(0).Rows(counter).Item("InsuranceFileKey"), 0) Then
                                mtaAllowed = False
                            End If
                        End If

                        dr.Tables(0).Rows(counter).Item("MtaAllowed") = mtaAllowed
                    Next counter
                End If
            End If

            Dim oXmlDoc As New System.Xml.XmlDocument
            Try
                oXmlDoc.LoadXml(dr.GetXml)
                oGetPartySummaryResponse.InsuranceFileDataset = oXmlDoc.DocumentElement
            Catch ex As Exception
                Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.XMLDocumentBadlyFormed, "Failed to convert InsuranceFileDataset XML", ex.Message)
                STSErrorEx.SetContext(oGetPartySummaryResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
                Return oGetPartySummaryResponse
            End Try

            Dim oClaimXmlDoc As New System.Xml.XmlDocument

            If nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage Then

                Using Cmd As SiriusCommand = SiriusCommand.FromProcedure(kSQL_SP_CLAIM_LIST.ToString)

                    Cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = GetPartySummaryRequest.PartyKey

                    dr = Con.ExecuteDataSet(Cmd, "dr")
                End Using

                ' Name the dataset, table and columns
                dr.DataSetName = "GetPartySummaryResponseTypeClaims"
                dr.Tables(0).TableName = "Row"

                dr.Tables(0).Columns(0).ColumnName = "claim_id"
                dr.Tables(0).Columns(1).ColumnName = "claim_number"
                dr.Tables(0).Columns(2).ColumnName = "description"
                dr.Tables(0).Columns(3).ColumnName = "loss_date"
                dr.Tables(0).Columns(4).ColumnName = "reported_date"
                dr.Tables(0).Columns(5).ColumnName = "policy_number"
                dr.Tables(0).Columns(6).ColumnName = "product_description"
                dr.Tables(0).Columns(7).ColumnName = "claim_status_id"
                dr.Tables(0).Columns(8).ColumnName = "claim_status_code"
                dr.Tables(0).Columns(9).ColumnName = "claim_status_description"
                dr.Tables(0).Columns(10).ColumnName = "claim_closed_status"
                dr.Tables(0).Columns(10).ColumnName = "AssociatedClients"
                Try
                    oClaimXmlDoc.LoadXml(dr.GetXml)
                Catch ex As Exception
                    Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.XMLDocumentBadlyFormed, "Failed to convert ClaimDataset XML", ex.Message)
                    STSErrorEx.SetContext(oGetPartySummaryResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "", True)
                    Return oGetPartySummaryResponse
                End Try

            End If
            oGetPartySummaryResponse.ResultData = dr
            'TypeCast the response Back.
            If nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
                Dim oResponse As New AgentImplementationTypes.GetPartySummaryResponseType
                oResponse = DirectCast(oGetPartySummaryResponse, AgentImplementationTypes.GetPartySummaryResponseType)
                oGetPartySummaryResponse = oResponse
            ElseIf nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage Then
                Dim oResponse As New SAMForBrokingImplementationTypes.GetPartySummaryResponseType
                oResponse = DirectCast(oGetPartySummaryResponse, SAMForBrokingImplementationTypes.GetPartySummaryResponseType)
                oResponse.ClaimDataset = oClaimXmlDoc.DocumentElement
                oGetPartySummaryResponse = oResponse
            End If
        End Using

        Return oGetPartySummaryResponse
    End Function

    Public Overloads Function UpdateParty(ByVal UpdatePartyRequest As BaseUpdatePartyRequestType) As BaseUpdatePartyResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                            _SiriusUser.Username, _SiriusUser.SourceID, _
                                            _SiriusUser.LanguageID, _
                                            SiriusUserDefaults.AppName)

            Dim oResponse As BaseUpdatePartyResponseType

            Try
                con.BeginTransaction()
                oResponse = UpdateParty(con, UpdatePartyRequest)
                con.CommitTransaction()
                AddPartyHistory(con, UpdatePartyRequest.PartyKey, String.Empty)
            Catch
                con.RollbackTransaction()
                Throw
            End Try

            Return oResponse

        End Using

    End Function

    Public Overloads Function UpdateParty(ByVal con As SiriusConnection, ByVal UpdatePartyRequest As BaseUpdatePartyRequestType) As BaseUpdatePartyResponseType

        'TODO Associations
        'TODO Loyalties

        'Return oUpdatePartyResponse
        Const ACMethodName As String = "UpdateParty"
        Const ACAddressCorrespondence As Integer = 6
        'Declare the Response object 
        Dim oResponse As New BaseImplementationTypes.BaseUpdatePartyResponseType

        Dim iRet As System.Int32
        Dim sPartyType As String = String.Empty
        Dim vAddresses As Object = Nothing
        Dim vContacts As Object = Nothing
        'Dim bIsLocked As Boolean
        Dim sBusinessCode As String = String.Empty
        Dim iAgentCnt As Integer = -1
        Dim sMainContact As String = String.Empty
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim STSError As New STSErrorPublisher
        Dim nTypeOfPackage As enumTypeOfPackage

        Dim oAgentRequest As AgentImplementationTypes.UpdatePartyRequestType = Nothing
        Dim samForBrokingRequest As SAMForBrokingImplementationTypes.UpdatePartyRequestType = Nothing

        If UpdatePartyRequest.GetType Is GetType(AgentImplementationTypes.UpdatePartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oResponse = New AgentImplementationTypes.UpdatePartyResponseType
            oAgentRequest = DirectCast(UpdatePartyRequest, AgentImplementationTypes.UpdatePartyRequestType)
        ElseIf UpdatePartyRequest.GetType Is GetType(AnonymousImplementationTypes.UpdatePartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            oResponse = New AnonymousImplementationTypes.UpdatePartyResponseType
        ElseIf UpdatePartyRequest.GetType Is GetType(CustomerImplementationTypes.UpdatePartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.CustomersPackage
            oResponse = New CustomerImplementationTypes.UpdatePartyResponseType
        ElseIf UpdatePartyRequest.GetType Is GetType(BaseImplementationTypes.BaseUpdatePartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            oResponse = New BaseImplementationTypes.BaseUpdatePartyResponseType
        ElseIf UpdatePartyRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.UpdatePartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.UpdatePartyResponseType
        ElseIf UpdatePartyRequest.GetType Is GetType(SAMForBrokingImplementationTypes.UpdatePartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage
            oResponse = New SAMForBrokingImplementationTypes.UpdatePartyResponseType
            samForBrokingRequest = DirectCast(UpdatePartyRequest, SAMForBrokingImplementationTypes.UpdatePartyRequestType)
        ElseIf UpdatePartyRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.UpdatePartyRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.UpdatePartyResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        ' Check that some input has been passed
        If (UpdatePartyRequest Is Nothing) = True Then
            Return Nothing
        End If

        If nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage Then
            If CoreBusiness.PartyIsAgent(samForBrokingRequest.UserPartyType) Then
                If oCoreBusiness.CheckAgentKey(samForBrokingRequest.UserPartyKey) = False Then
                    STSError = New STSErrorPublisher(STSErrorCodes.AgentRecordNotFound, "Agent Key validation failed", "The Agent record does not exist for key: " & samForBrokingRequest.UserPartyKey)
                    STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Key Validation", True)
                    Return oResponse
                End If

                If oCoreBusiness.AgentSecurityCheck(samForBrokingRequest.UserName, samForBrokingRequest.PartyKey, PMEEntityType.Party) = False Then
                    STSError = New STSErrorPublisher(STSErrorCodes.SecurityCheckFailed, "Security Check Failed", samForBrokingRequest.UserName & " does not have permission to access party " & samForBrokingRequest.UserPartyKey)
                    STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "UpdateParty", True)
                    Return oResponse
                End If
            End If
        End If

        If UpdatePartyRequest.Party Is Nothing Then
            STSError.AddInvalidField("Party", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "Party"), "")
        End If

        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        ' Check that mandatory base request fields have been provided
        If UpdatePartyRequest.PartyKey = 0 Then
            STSError.AddInvalidField("PartyKey", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "PartyKey"), "")
        End If

        If UpdatePartyRequest.Party.BranchCode = "" Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        If UpdatePartyRequest.BranchCode = "" Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        If UpdatePartyRequest.PartyTimestamp Is Nothing Then
            STSError.AddInvalidField("PartyTimestamp", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "PartyTimestamp"), "")
        End If

        Dim oBasePartyPC As New BaseImplementationTypes.BasePartyPCType
        Dim oBasePartyCC As New BaseImplementationTypes.BasePartyCCType
        Dim oBasePartyOther As New BaseImplementationTypes.BasePartyOTHERType
        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection

        Dim bOtherParty As Boolean = False
        ' Determine the Party Type
        If UpdatePartyRequest.Party.GetType Is GetType(BaseImplementationTypes.BasePartyPCType) Then
            sPartyType = "PC"
            oBasePartyPC = DirectCast(UpdatePartyRequest.Party, BaseImplementationTypes.BasePartyPCType)

            If oBasePartyPC.Lifestyle IsNot Nothing Then
                For Each oLifeStyle As BasePartyPCTypeLifestyle In oBasePartyPC.Lifestyle
                    If oLifeStyle.LifestyleKey <> 0 AndAlso String.IsNullOrEmpty(oLifeStyle.CategoryCode) Then

                        STSError.AddInvalidField("CategoryCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "CategoryCode"), "")
                    End If
                Next
            End If
            oSAMErrorCollection.CheckForErrors()

            oBasePartyPC.Validate(CType(oSAMErrorCollection, Object))
            ' if there were any errors throw an exception
            oSAMErrorCollection.CheckForErrors()

        ElseIf UpdatePartyRequest.Party.GetType Is GetType(BaseImplementationTypes.BasePartyCCType) Then
            sPartyType = "CC"
            oBasePartyCC = DirectCast(UpdatePartyRequest.Party, BaseImplementationTypes.BasePartyCCType)
            sMainContact = oBasePartyCC.MainContact
            ' Check Business Code
            ' Vivek: commented because its not working: Gaurav to help
            If oBasePartyCC.BusinessCode <> "" Then
                Try
                    'Dim iBusinessID As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.UserDefinedTable, "2228228", oBasePartyCC.BusinessCode, sBusinessCode)
                    Dim strBusinessCode As Object = GetListCode(Of String)(Core.STSListType.UserDefinedTable, "2228228", _
                            oBasePartyCC.BusinessCode, "caption", True)
                    'Modified on 17-09-08
                    sBusinessCode = oBasePartyCC.BusinessCode
                Catch ex As Exception
                    STSError.AddInvalidField("BusinessCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BusinessCode"), oBasePartyCC.BusinessCode)
                End Try
            End If

            oBasePartyCC.Validate(CType(oSAMErrorCollection, Object))
            ' if there were any errors throw an exception
            oSAMErrorCollection.CheckForErrors()

        Else
            sPartyType = "Other"
            oBasePartyOther = DirectCast(UpdatePartyRequest.Party, BaseImplementationTypes.BasePartyOTHERType)
            bOtherParty = True
            oBasePartyOther.Validate(CType(oSAMErrorCollection, Object)) 'Uncomment Later
            ' if there were any errors throw an exception
            oSAMErrorCollection.CheckForErrors()
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        Dim iSourceID As Int32
        iSourceID% = 1
        ' Convert branch code to ID
        Try
            iSourceID% = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", UpdatePartyRequest.BranchCode)
        Catch ex As Exception
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BranchCode"), UpdatePartyRequest.BranchCode)
        End Try

        If UpdatePartyRequest.SubBranchCode <> "" Then
            Try
                Dim iSubBranchId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", UpdatePartyRequest.SubBranchCode)
            Catch ex As Exception
                STSError.AddInvalidField("SubBranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "SubBranchCode"), UpdatePartyRequest.SubBranchCode)
            End Try
        End If

        Dim lAccountExecutiveID, lCurrencyId, lNoOfEmployees, lBranchId, lSubBranchId As Integer
        Dim oUpdatePartyAddDetails As New Hashtable
        Dim sAlternativeIdentifier As String = Nothing
        If sPartyType = "PC" Then
            ValidatePartyPCTypeLookup(oBasePartyPC, oCoreBusiness, STSError)
            'Added on 06-09-08 as per Rahul's Advice
            If Not (String.IsNullOrEmpty(oBasePartyPC.AccountExecutiveCode)) Then
                lAccountExecutiveID = GetAndValidateSpecifiedTableCode(con, "party", "party_cnt", "shortname", oBasePartyPC.AccountExecutiveCode, oSAMErrorCollection, "AccountExecutiveCode")
            End If
            If Not (String.IsNullOrEmpty(oBasePartyPC.Currency)) Then
                lCurrencyId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "currency", oBasePartyPC.Currency, "currency_id", oSAMErrorCollection)
            End If
            'Added on 18-09-08 as per Rahul's advice
            If Not (String.IsNullOrEmpty(oBasePartyPC.BranchCode)) Then
                lBranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "source", oBasePartyPC.BranchCode, "source_id", oSAMErrorCollection)
            End If
            If Not (String.IsNullOrEmpty(oBasePartyPC.SubBranchCode)) Then
                lSubBranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "sub_branch", oBasePartyPC.SubBranchCode, "sub_branch_id", oSAMErrorCollection)
            End If
            oSAMErrorCollection.CheckForErrors()
            oUpdatePartyAddDetails.Add("consultant_cnt", lAccountExecutiveID)
            oUpdatePartyAddDetails.Add("currency_id", lCurrencyId)
            oUpdatePartyAddDetails.Add("source_id", lBranchId)
            oUpdatePartyAddDetails.Add("sub_branch_id", lSubBranchId)
            sAlternativeIdentifier = oBasePartyPC.AlternativeId
        ElseIf sPartyType = "CC" Then
            ValidatePartyCCTypeLookup(oBasePartyCC, oCoreBusiness, STSError)
            If Not (String.IsNullOrEmpty(oBasePartyCC.AccountExecutiveCode)) Then
                lAccountExecutiveID = GetAndValidateSpecifiedTableCode(con, "party", "party_cnt", "shortname", oBasePartyCC.AccountExecutiveCode, oSAMErrorCollection, "AccountExecutiveCode")
            End If
            If Not (String.IsNullOrEmpty(oBasePartyCC.Currency)) Then
                lCurrencyId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "currency", oBasePartyCC.Currency, "currency_id", oSAMErrorCollection)
            End If
            'Added on 17-09-08 as per Rahul's advice
            If Not (String.IsNullOrEmpty(oBasePartyCC.NumberOfEmployees)) Then
                lNoOfEmployees = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "EmployeeBand", oBasePartyCC.NumberOfEmployees, "employeeband_id", oSAMErrorCollection)
            End If
            If Not (String.IsNullOrEmpty(oBasePartyCC.BranchCode)) Then
                lBranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "source", oBasePartyCC.BranchCode, "source_id", oSAMErrorCollection)
            End If
            If Not (String.IsNullOrEmpty(oBasePartyCC.SubBranchCode)) Then
                lSubBranchId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "sub_branch", oBasePartyCC.SubBranchCode, "sub_branch_id", oSAMErrorCollection)
            End If
            'Added on 06-09-08 as per Rahul's Advice
            oSAMErrorCollection.CheckForErrors()
            oUpdatePartyAddDetails.Add("consultant_cnt", lAccountExecutiveID)
            oUpdatePartyAddDetails.Add("currency_id", lCurrencyId)
            oUpdatePartyAddDetails.Add("employeeband_id", lNoOfEmployees)
            oUpdatePartyAddDetails.Add("source_id", lBranchId)
            oUpdatePartyAddDetails.Add("sub_branch_id", lSubBranchId)
            sAlternativeIdentifier = oBasePartyCC.AlternativeId
        ElseIf sPartyType.StartsWith("OT") Then
            ValidatePartyOtherTypeLookup(oBasePartyOther, oCoreBusiness, STSError)
        End If

        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Lookup field validation", True)
            Return oResponse
        End If

        If nTypeOfPackage = enumTypeOfPackage.AgentsPackage Then
            If (oCoreBusiness.AgentSecurityCheck(oAgentRequest.UserName, oAgentRequest.PartyKey, PMEEntityType.Party) = False) Then
                STSError = New STSErrorPublisher(STSErrorCodes.SecurityCheckFailed, "Security check failed", oAgentRequest.UserName & " does not have permission to access party " & oAgentRequest.PartyKey)
                STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "UpdateParty", True)
                Return oResponse
            End If
        End If

        ' Check that the timestamp matches and lock the Party
        Dim AnyError As STSErrorType
        AnyError = oCoreBusiness.CheckTSAndLock(con, _
            BranchCode:=UpdatePartyRequest.BranchCode, _
            Lockname:=CoreBusiness.LockName.PartyCnt, _
            LockValue:=UpdatePartyRequest.PartyKey, _
            TStamp:=UpdatePartyRequest.PartyTimestamp)
        ' Check for Errors
        If AnyError Is Nothing = False Then
            ' Either the timestamp didn't match or the record couldn't be locked for some reason, return the error.
            oResponse.STSError = AnyError
            Return oResponse
        End If

        ' If the request has come through the Agents interface then make sure we have the AgentKey
        If (nTypeOfPackage = enumTypeOfPackage.AgentsPackage) Then
            Dim oAgentUpdatePartyRequest As AgentImplementationTypes.UpdatePartyRequestType = DirectCast(UpdatePartyRequest, AgentImplementationTypes.UpdatePartyRequestType)
            iAgentCnt = CInt(oAgentUpdatePartyRequest.AgentKey)
        ElseIf nTypeOfPackage = enumTypeOfPackage.AnonymousPackage Then
            Dim oAnonUpdatePartyRequest As AnonymousImplementationTypes.UpdatePartyRequestType = DirectCast(UpdatePartyRequest, AnonymousImplementationTypes.UpdatePartyRequestType)
            iAgentCnt = CInt(oAnonUpdatePartyRequest.AgentKey)
        ElseIf nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage Then
            Dim oSAMForInsurancePartyRequest As SAMForInsuranceImplementationTypes.UpdatePartyRequestType = DirectCast(UpdatePartyRequest, SAMForInsuranceImplementationTypes.UpdatePartyRequestType)
            iAgentCnt = CInt(oSAMForInsurancePartyRequest.AgentKey)
        ElseIf nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then
            Dim oSAMForInsuranceV2PartyRequest As SAMForInsuranceV2ImplementationTypes.UpdatePartyRequestType = DirectCast(UpdatePartyRequest, SAMForInsuranceV2ImplementationTypes.UpdatePartyRequestType)
            iAgentCnt = CInt(oSAMForInsuranceV2PartyRequest.AgentKey)
        End If

        ' Move all addresses to a 2D array
        If IsArray(UpdatePartyRequest.Party.Addresses) = True Then
            STSError = ProcessAddresses(UpdatePartyRequest.Party.Addresses, vAddresses, con)
            If STSError.HasErrors Then
                STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
                Return oResponse
            End If
        End If

        ' Move all contacts to a 2D array
        If IsArray(UpdatePartyRequest.Party.Contacts) = True Then
            STSError = ProcessContacts(UpdatePartyRequest.Party.Contacts, vContacts)
            If STSError.HasErrors Then
                STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
                Return oResponse
            End If
        End If

        ' Move all Address contacts to a 2D array
        Dim vSingleContacts As Object = Nothing
        Dim vSingleContactsArray As Object(,)
        Dim vAddContacts As Object(,) = Nothing
        Dim existUBound As Integer
        If UpdatePartyRequest.Party.Addresses.GetType Is GetType(BaseAddressWithContactsType()) Then
            If IsArray(UpdatePartyRequest.Party.Addresses) = True Then
                For addCNT As Integer = UpdatePartyRequest.Party.Addresses.GetLowerBound(0) To _
                                        UpdatePartyRequest.Party.Addresses.GetUpperBound(0)
                    Dim oAddressWithContacts() As BaseAddressWithContactsType
                    oAddressWithContacts = DirectCast(UpdatePartyRequest.Party.Addresses, BaseAddressWithContactsType())
                    If oAddressWithContacts IsNot Nothing Then
                        STSError = ProcessContacts(oAddressWithContacts(addCNT).Contacts, vSingleContacts, addCNT + 1)
                        If STSError IsNot Nothing AndAlso STSError.HasErrors Then
                            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
                            Return oResponse
                        End If
                        vSingleContactsArray = DirectCast(vSingleContacts, Object(,))

                        If vSingleContactsArray IsNot Nothing Then
                            If vAddContacts Is Nothing Then
                                existUBound = 0
                                ReDim Preserve vAddContacts(4, UBound(vSingleContactsArray, 2))
                                For CountFor As Integer = 0 To UBound(vSingleContactsArray, 2)
                                    vAddContacts(0, CountFor) = vSingleContactsArray(0, CountFor)
                                    vAddContacts(1, CountFor) = vSingleContactsArray(1, CountFor)
                                    vAddContacts(2, CountFor) = vSingleContactsArray(2, CountFor)
                                    vAddContacts(3, CountFor) = vSingleContactsArray(3, CountFor)
                                    vAddContacts(4, CountFor) = vSingleContactsArray(4, CountFor)
                                Next

                                'Array.ConstrainedCopy(vSingleContacts, 0, vAddContacts, existUBound, UBound(vSingleContacts, 2) * 5)
                            Else

                                existUBound = UBound(vAddContacts, 2)
                                ReDim Preserve vAddContacts(4, (existUBound + UBound(vSingleContactsArray, 2)))
                                For CountFor As Integer = 0 To UBound(vSingleContactsArray, 2)
                                    vAddContacts(0, existUBound + CountFor) = vSingleContactsArray(0, CountFor)
                                    vAddContacts(1, existUBound + CountFor) = vSingleContactsArray(1, CountFor)
                                    vAddContacts(2, existUBound + CountFor) = vSingleContactsArray(2, CountFor)
                                    vAddContacts(3, existUBound + CountFor) = vSingleContactsArray(3, CountFor)
                                    vAddContacts(4, existUBound + CountFor) = vSingleContactsArray(4, CountFor)
                                Next
                                'Array.ConstrainedCopy(vSingleContacts, 0, vAddContacts, 0, (UBound(vSingleContacts, 2) + 1) * 5)
                            End If

                            'Array.ConvertAll(vAddContacts, vSingleContacts)
                            ''Array.Resize(vAddContacts.Rank, existUBound + UBound(vSingleContacts) + 1)

                        End If

                    End If
                Next
            End If
        End If

        If bOtherParty = True Then
            ' Call Validate Data
            ValidateUpdatePartyData(oCoreBusiness, oBasePartyOther, _
                UpdatePartyRequest.PartyKey, oSAMErrorCollection, nTypeOfPackage)

            oSAMErrorCollection.CheckForErrors()

        End If

        Dim oGIS As bGIS.STS = Nothing
        Try
            oGIS = New bGIS.STS
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.FailedToInitialiseBackofficeComponent, "Failed to initialise bGIS.STS", ex.Message)
            STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "oGIS.Initialise", True)
            Return oResponse
        End Try

        '' Call the Method
        Dim r_oSuppBusiness(,) As Object = Nothing

        If oBasePartyOther.SupplierBusiness IsNot Nothing Then
            ProcessSupplierBusiness(oBasePartyOther.SupplierBusiness, _
                                        r_oSuppBusiness, _
                                        UpdatePartyRequest.PartyKey)
        End If

        ' Initialise the GIS
        SAMFunc.InitialiseGISSTS(Con:=con, oGIS:=oGIS, SiriusUser:=_SiriusUser)

        Try

            ' Process Tax Details
            Dim sTaxNumber As String = String.Empty
            Dim bDomiciledForTax As Integer
            Dim bTaxExempt As Integer
            Dim lPercentage As Integer

            sTaxNumber = UpdatePartyRequest.Party.TaxNumber
            bDomiciledForTax = Cast.ToInt32(UpdatePartyRequest.Party.DomiciledForTax).GetValueOrDefault(0)
            bTaxExempt = Cast.ToInt32(UpdatePartyRequest.Party.TaxExempt).GetValueOrDefault(0)
            lPercentage = CInt(Cast.ToDecimal(UpdatePartyRequest.Party.TaxPercentage, 0))

            Dim sMaritalStatusCode As String = Nothing
            If (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.Divorced) Then
                sMaritalStatusCode = "Divorced"
            ElseIf (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.Married) Then
                sMaritalStatusCode = "Married"

            ElseIf (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.MarriedCommonLaw) Then
                sMaritalStatusCode = "Married - Common Law"
            ElseIf (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.NotApplicable) Then
                sMaritalStatusCode = "Not Applicable"
            ElseIf (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.NotAvailable) Then
                sMaritalStatusCode = "Not Available"

            ElseIf (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.Partnered) Then
                sMaritalStatusCode = "Partnered"
            ElseIf (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.Separated) Then
                sMaritalStatusCode = "Separated"
            ElseIf (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.Single_) Then
                sMaritalStatusCode = "Single"
            ElseIf (oBasePartyPC.MaritalStatusCode = MaritalStatusCodeType.Widowed) Then
                sMaritalStatusCode = "Widowed"
            End If
            Dim vAddContactsSimple As Object = vAddContacts
            ' NB - trading name is or Personal Clients and currently we are not passed this
            ' through the web service
            iRet = CType(oGIS.UpdateParty( _
                    sPartyTypeCode:=sPartyType, _
                    sBranchCode:=UpdatePartyRequest.BranchCode, _
                    vAddresses:=DirectCast(vAddresses, Object(,)), _
                    sTPUserCode:=UpdatePartyRequest.Party.TPUserCode, _
                    sTPIntroducer:=UpdatePartyRequest.Party.TPIntroducer, _
                    vContacts:=DirectCast(vContacts, Object(,)), _
                    sSurname:=oBasePartyPC.Surname, _
                    sForename:=oBasePartyPC.Forename, _
                    sDateOfBirth:=CType(oBasePartyPC.DateOfBirth, String), _
                    sTitle:=oBasePartyPC.Title, _
                    sMaritalStatusCode:=sMaritalStatusCode, _
                    sGenderCode:=oBasePartyPC.GenderCode, _
                    sInitials:=oBasePartyPC.Initials, _
                    sOccupationCode:=oBasePartyPC.OccupationCode, _
                    sEmployerBusinessCode:=oBasePartyPC.EmployersBusinessCode, _
                    sEmploymentStatusCode:=oBasePartyPC.EmploymentStatusCode.ToString, _
                    sAlternativeID:=sAlternativeIdentifier, _
                    sCompanyName:=oBasePartyCC.CompanyName, _
                    sTradingName:="", _
                    sBusinessCode:=sBusinessCode, _
                    lAgentCnt:=iAgentCnt, _
                    r_lPartyCnt:=UpdatePartyRequest.PartyKey, _
                    vAddressContacts:=DirectCast(vAddContactsSimple, Object(,)), _
                    r_sTaxNumber:=sTaxNumber, _
                    r_bDomiciledForTax:=CShort(bDomiciledForTax), _
                    r_bTaxExempt:=CShort(bTaxExempt), _
                    r_lPercentage:=lPercentage, _
                    vSupplierBusiness:=Nothing, _
                    v_sFileCode:=UpdatePartyRequest.Party.FileCode, _
                    sMainContact:=oBasePartyCC.MainContact), Integer)
            If (iRet <> PMEReturnCode.PMTrue) Then
                Dim STSErrorLocal As New STSErrorPublisher(iRet, "bGIS.STS.UpdateParty failed")
                STSErrorLocal.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "UpdateParty Return", True)
            End If

            vAddContacts = DirectCast(vAddContactsSimple, Object(,))

            ' if this is sam for insurance
            If nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage Or _
                nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then

                Dim bSaveAddressesWithContacts As Boolean
                ' determine if we have any addresses with associated contacts
                Dim oAddressWithContacts As BaseAddressWithContactsType
                For Each oAddress As BaseAddressType In UpdatePartyRequest.Party.Addresses

                    oAddressWithContacts = TryCast(oAddress, BaseAddressWithContactsType)
                    If oAddressWithContacts IsNot Nothing AndAlso oAddressWithContacts.Contacts IsNot Nothing AndAlso oAddressWithContacts.Contacts.Length > 0 Then
                        bSaveAddressesWithContacts = True

                        ' delete existing party_address_usage as it will be replaced below
                        DeletePartyAddressUsage(con, UpdatePartyRequest.PartyKey)

                        Exit For

                    End If

                Next

                ' determine if we need to replace the saved addresses to store the additional contact info
                If bSaveAddressesWithContacts Then

                    For Each oAddress As BaseAddressType In UpdatePartyRequest.Party.Addresses

                        oAddressWithContacts = TryCast(oAddress, BaseAddressWithContactsType)

                        If oAddressWithContacts IsNot Nothing Then

                            ' save the address contact to the database
                            SaveAddressContactUsage(con, oCoreBusiness, oAddressWithContacts, UpdatePartyRequest.BranchCode, UpdatePartyRequest.SourceId, UpdatePartyRequest.PartyKey)

                        End If

                    Next

                End If

                ' if this is a party of type "OTHER"
                If bOtherParty = True Then

                    ' process party convictions
                    ' Vivek: differing from Tech Spec
                    If nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then
                        ProcessPartyConvictions(con, oCoreBusiness, oBasePartyOther.Convictions, UpdatePartyRequest.PartyKey)
                    Else
                        ProcessOtherPartyConvictions(con, oBasePartyOther.Conviction, UpdatePartyRequest.PartyKey)
                    End If

                    ' process party accidents
                    ProcessOtherPartyAccidents(con, oBasePartyOther.Accident, UpdatePartyRequest.PartyKey)

                    ' process party supplier busines
                    ProcessOtherPartySupplierBusiness(con, oBasePartyOther.SupplierBusiness, UpdatePartyRequest.PartyKey)

                    ' process party details
                    UpdateOtherPartyDetails(con, oBasePartyOther, UpdatePartyRequest.PartyKey)

                End If

            End If

            '***********************************************************************
            ' update risk data - only supported by SAMForInsurance
            If nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage Or _
                nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then
                Dim PartyXBuilder As New CoreImplementation.XBuilder.Party(_SiriusUser)
                PartyXBuilder.UpdateDataSet(con, UpdatePartyRequest.BranchCode, UpdatePartyRequest.PartyKey, UpdatePartyRequest.Party.XMLDataset)
            End If
            '***********************************************************************

            If nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then
                Dim nCount As Integer
                If sPartyType = "PC" Then

                    ' update data specific to PC type
                    UpdatePartyPCDetails(con, oCoreBusiness, oBasePartyPC, UpdatePartyRequest.PartyKey)

                    ProcessLifeStyle(con, oCoreBusiness, oBasePartyPC.Lifestyle, UpdatePartyRequest.PartyKey)

                    'update contents of ClientSharedDetails
                    UpdatePartyDetails(con, oCoreBusiness, oBasePartyPC.ClientDetail, UpdatePartyRequest.PartyKey, oUpdatePartyAddDetails)
                    ProcessProspect(con, oCoreBusiness, oBasePartyPC.ClientDetail, UpdatePartyRequest.PartyKey)

                    ProcessAssociates(con, oCoreBusiness, oBasePartyPC.ClientDetail.Associates, UpdatePartyRequest.PartyKey)
                    ProcessPartyConvictions(con, oCoreBusiness, oBasePartyPC.ClientDetail.Convictions, UpdatePartyRequest.PartyKey)
                    ProcessLoyaltyScheme(con, oCoreBusiness, oBasePartyPC.ClientDetail.LoyaltyScheme, UpdatePartyRequest.PartyKey)
                    ProcessProspectPolicies(con, oCoreBusiness, oBasePartyPC.ClientDetail.ProspectPolicies, UpdatePartyRequest.PartyKey)

                ElseIf sPartyType = "CC" Then

                    ' update data specific to CC type
                    UpdatePartyCCDetails(con, oCoreBusiness, oBasePartyCC, UpdatePartyRequest.PartyKey, oUpdatePartyAddDetails)

                    'update contents of ClientSharedDetails
                    UpdatePartyDetails(con, oCoreBusiness, oBasePartyCC.ClientDetail, UpdatePartyRequest.PartyKey, oUpdatePartyAddDetails)
                    ProcessProspect(con, oCoreBusiness, oBasePartyCC.ClientDetail, UpdatePartyRequest.PartyKey)

                    ProcessAssociates(con, oCoreBusiness, oBasePartyCC.ClientDetail.Associates, UpdatePartyRequest.PartyKey)
                    ProcessPartyConvictions(con, oCoreBusiness, oBasePartyCC.ClientDetail.Convictions, UpdatePartyRequest.PartyKey)
                    ProcessLoyaltyScheme(con, oCoreBusiness, oBasePartyCC.ClientDetail.LoyaltyScheme, UpdatePartyRequest.PartyKey)
                    ProcessProspectPolicies(con, oCoreBusiness, oBasePartyCC.ClientDetail.ProspectPolicies, UpdatePartyRequest.PartyKey)

                    ' Vivek: we are not required to call it here
                    ' because its already called in "If bOtherParty = True Then" condition above
                    'ElseIf sPartyType.StartsWith("OT") Then

                    '    ProcessPartyConvictions(con, oCoreBusiness, oBasePartyOther.Convictions, UpdatePartyRequest.PartyKey)
                    Dim nCountryID As Integer
                    For nCount = 0 To UpdatePartyRequest.Party.Addresses.Length
                        If UpdatePartyRequest.Party.Addresses(nCount).AddressTypeCode = ACAddressCorrespondence Then
                            Try
                                nCountryID = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Country", UpdatePartyRequest.Party.Addresses(nCount).CountryCode)
                            Catch ex As Exception
                                STSError.AddInvalidField("CountryCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "CountryCode"), UpdatePartyRequest.Party.Addresses(nCount).CountryCode)
                            End Try
                            UpdatePartyAccountAddressDetails(con, UpdatePartyRequest.Party.Addresses(nCount).AddressLine1, _
                                                                    UpdatePartyRequest.Party.Addresses(nCount).AddressLine2, _
                                                                     UpdatePartyRequest.Party.Addresses(nCount).AddressLine3, _
                                                                       UpdatePartyRequest.Party.Addresses(nCount).AddressLine4, _
                                                                         UpdatePartyRequest.Party.Addresses(nCount).PostCode, _
                                                                          UpdatePartyRequest.PartyKey, _
                                                                           nCountryID)

                            Exit For
                        End If

                    Next nCount
                End If
            End If

            ' commit all the data to the db

            ' Unlock and return the new timestamp 
            AnyError = oCoreBusiness.UnlockAndGetTS(con, _
                BranchCode:=UpdatePartyRequest.BranchCode, _
                Lockname:=CoreBusiness.LockName.PartyCnt, _
                LockValue:=UpdatePartyRequest.PartyKey, _
                TStamp:=oResponse.PartyTimestamp)

            If (AnyError Is Nothing) = False Then

                ' The Default Rules failed to run.
                oResponse.STSError = AnyError
                Return oResponse

            End If

            Return oResponse

        Catch ex As Exception

            ' an error occurred so ensure the transaction is rolled back

            If nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage Or _
                nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then
                ' if this is sam for insurance then just throw the current exception
                Throw
            Else
                Dim STSErrorLocal As New STSErrorPublisher("bGIS.STS.UpdateParty failed", ex)
                STSErrorLocal.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "UpdateParty Exception", True)
            End If

        Finally
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
        End Try

        Return oResponse

    End Function

    '''<summary>
    ''' Retrieves details of a party bank item or all Party bank items for a specified party.
    '''</summary>
    '''<param name="oGetPartyBankDetailsRequest" type="BaseGetPartyBankDetailsRequestType"></param>   
    '''<returns>BaseGetPartyBankDetailsResponseType</returns>
    '''<remarks></remarks> 
    Public Overloads Function GetPartyBankDetails(ByVal oGetPartyBankDetailsRequest As BaseGetPartyBankDetailsRequestType) As BaseGetPartyBankDetailsResponseType
        Using conPartyBank As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Return (GetPartyBankDetails(conPartyBank, oGetPartyBankDetailsRequest))
        End Using
    End Function

    '''<summary>
    ''' Retrieves details of a party bank item or all Party bank items for a specified party.
    '''</summary>
    '''<param name="conPartyBank" type="SiriusConnection"></param>
    '''<param name="oGetPartyBankDetailsRequest" type="BaseGetPartyBankDetailsPolicyRequestType"></param>   
    '''<returns>BaseGetPartyBankDetailsResponseType</returns>
    '''<remarks></remarks> 
    Public Overloads Function GetPartyBankDetails(ByVal conPartyBank As SiriusConnection, ByVal oGetPartyBankDetailsRequest As BaseGetPartyBankDetailsRequestType) As BaseGetPartyBankDetailsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim oResponse As New BaseGetPartyBankDetailsResponseType
        Dim sReturnValue As String
        Dim lPartyKey As Integer = 0
        Dim lAccountKey As Integer = 0

        Dim nTypeofPackage As enumTypeOfPackage
        If oGetPartyBankDetailsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetPartyBankDetailsRequestType) Then
            nTypeofPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.GetPartyBankDetailsResponseType
        Else
            nTypeofPackage = enumTypeOfPackage.UnknownPackage
            Return oResponse
        End If

        With oGetPartyBankDetailsRequest
            'Check Mandatory Field
            .Validate(CObj(oSAMErrorCollection))
            oSAMErrorCollection.CheckForErrors()

            'Look up validation
            Dim iSourceId As Integer
            iSourceId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, .BranchCode, "BranchCode", oSAMErrorCollection)
            oSAMErrorCollection.CheckForErrors()

            'Party 
            If .PartyKey <> 0 Then
                sReturnValue = GetAndValidateDescriptionById(conPartyBank, "Party", "Party_cnt", "Party_Cnt", .PartyKey.ToString())
                Try
                    If (String.IsNullOrEmpty(sReturnValue) OrElse Convert.ToInt32(sReturnValue) = 0) Then
                        oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                                        "PartyKey", _
                                                       .PartyKey.ToString())
                    Else
                        'Get the account associated with this party
                        sReturnValue = GetAndValidateDescriptionById(conPartyBank, "Account", "Account_ID", "Account_Key", .PartyKey.ToString())
                        If String.IsNullOrEmpty(sReturnValue) = False Then
                            lAccountKey = Convert.ToInt32(sReturnValue)
                        End If
                        End If
                Catch
                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                    SAMInvalidData.InvalidLookupListValue.ToString, _
                                                    "PartyKey", _
                                                   .PartyKey.ToString())
                End Try
                oSAMErrorCollection.CheckForErrors()
            End If

            'Account
            If .AccountKey <> 0 Then
                sReturnValue = GetAndValidateDescriptionById(conPartyBank, "Account", "Account_ID", "Account_Id", .AccountKey.ToString())
                Try
                    If (String.IsNullOrEmpty(sReturnValue) OrElse Convert.ToInt32(sReturnValue) = 0) Then
                        oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                                        "AccountKey", _
                                                       .AccountKey.ToString())
                    Else
                        'Get the party associated with this account
                        sReturnValue = GetAndValidateDescriptionById(conPartyBank, "Account", "Account_Key", "Account_ID", .AccountKey.ToString())

                        lPartyKey = Convert.ToInt32(sReturnValue)
                        If lPartyKey <> 0 Then
                            If .PartyKey <> 0 Then
                                If oGetPartyBankDetailsRequest.PartyKey <> lPartyKey Then
                                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.ValidationRulesFailed, _
                                                                  SAMInvalidData.ValidationRulesFailed.ToString, _
                                                                  "Party is not associated with given account")
                                End If
                            Else
                                'If party key is not provided, populate it
                                .PartyKey = lPartyKey
                            End If
                        Else
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.ValidationRulesFailed, _
                                                                 SAMInvalidData.ValidationRulesFailed.ToString, _
                                                                 "Party is not associated with given account")
                        End If
                    End If
                Catch
                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                      SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                      "AccountKey", _
                                                                     .AccountKey.ToString())
                End Try
                oSAMErrorCollection.CheckForErrors()
            End If

            'If account key is not provided, populate it
            If lAccountKey <> 0 AndAlso .AccountKey = 0 Then
                .AccountKey = lAccountKey
            End If

            'Party Bank
            If .PartyBankKey > 0 Then
                ValidatePartyBank(conPartyBank, oSAMErrorCollection, .PartyBankKey, .PartyKey, .AccountKey)
                
                oSAMErrorCollection.CheckForErrors()
            End If

            'Transacton Key
            If .IncludeLastTransactedPartyBankKey AndAlso .TransactionKey > 0 Then
                Dim r_bIsValidTransaction As Boolean = False
                Dim r_bIsValidTransactionParty As Boolean = False
                'Validate provided transaction
                ValidatePartyBankLastTransaction(conPartyBank, r_bIsValidTransaction, r_bIsValidTransactionParty, .PartyKey, .LastTransactionType, .TransactionKey)

                If Not r_bIsValidTransaction Then
                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                "TransactionKey", _
                                                              .TransactionKey.ToString())
                End If

                If Not r_bIsValidTransactionParty Then
                    If .LastTransactionType = LastTransactionTypeWithPartyBank.POLICY Then
                        oSAMErrorCollection.AddInvalidData(SAMInvalidData.ValidationRulesFailed, _
                                                      SAMInvalidData.ValidationRulesFailed.ToString, _
                                                      "Policy is not associated with given party")
                    ElseIf .LastTransactionType = LastTransactionTypeWithPartyBank.CLAIM Then
                        oSAMErrorCollection.AddInvalidData(SAMInvalidData.ValidationRulesFailed, _
                                                      SAMInvalidData.ValidationRulesFailed.ToString, _
                                                      "Claim is not associated with given party")
                    End If
                End If
                    oSAMErrorCollection.CheckForErrors()
                End If
        End With
        
        If oGetPartyBankDetailsRequest.PartyBankKey > 0 Then
            oResponse.PartyBankDetails = GetPartyBankDetailsByPartyBankKey(conPartyBank, oCoreBusiness, oGetPartyBankDetailsRequest)
        Else
            oResponse.PartyBankDetails = GetPartyBankDetailsByPartyKey(conPartyBank, oCoreBusiness, oGetPartyBankDetailsRequest)
        End If

        'Get the system option "Credit card processing method" and check whether it is external.
        Dim sCreditCardProcessingMethod As String
        oCoreBusiness.GetSystemOption(oGetPartyBankDetailsRequest.BranchCode, SystemOption.CreditCardProcessingMethod, sCreditCardProcessingMethod)
        If (Not String.IsNullOrEmpty(sCreditCardProcessingMethod) AndAlso sCreditCardProcessingMethod.Trim() = "1") Then
            oResponse.IsExternalCreditCardHandling = True
        End If

        'Get the party bank id on which the policy has made last transaction, if required
        If oGetPartyBankDetailsRequest.IncludeLastTransactedPartyBankKey AndAlso oGetPartyBankDetailsRequest.TransactionKey > 0 Then
            oResponse.LastTransactedPartyBankKey = GetLastTransactedPartyBankKey(conPartyBank, oGetPartyBankDetailsRequest.TransactionKey, oGetPartyBankDetailsRequest.LastTransactionType)
        End If

        Return oResponse
    End Function

    ''''<summary>
    '''' Validates the party bank provided with the party/account.
    ''''</summary>
    ''''<param name="conPartyBank" type="SiriusConnection"></param>
    ''''<param name="bIsValid" type="Boolean"></param>
    ''''<param name="bIsNumberExisting" type="Boolean"></param>
    ''''<param name="lPartyBankKey" type="Integer"></param>
    ''''<param name="lPartyKey" type="Integer"></param>
    ''''<param name="lAccountKey" type="Integer"></param>
    ''''<param name="bIsBank" type="Boolean"></param>
    ''''<param name="sNumber" type="String"></param>
    ''''<remarks></remarks> 
    'Private Overloads Sub ValidatePartyBank(ByVal conPartyBank As SiriusConnection, _
    '                                    ByRef bIsValid As Boolean, _
    '                                    ByRef bIsNumberExisting As Boolean, _
    '                                    ByVal lPartyBankKey As Integer, _
    '                                    Optional ByVal lPartyKey As Integer = 0, _
    '                                    Optional ByVal lAccountKey As Integer = 0, _
    '                                    Optional ByVal bIsBank As Boolean = False, _
    '                                    Optional ByVal sNumber As String = Nothing)

    '    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_PartyBank_Validate")
    '        cmd.AddOutParameter("@isValid", SqlDbType.TinyInt)
    '        cmd.AddOutParameter("@isNumberExisting", SqlDbType.TinyInt)
    '        cmd.AddInParameter("@party_bank_id", SqlDbType.Int).Value = lPartyBankKey
    '        cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(lPartyKey, 0)
    '        cmd.AddInParameter("@account_id", SqlDbType.Int).Value = Cast.NullIfDefault(lAccountKey, 0)
    '        If bIsBank Then
    '            cmd.AddInParameter("@is_bank", SqlDbType.TinyInt).Value = 1
    '            cmd.AddInParameter("@account_number", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(sNumber, String.Empty)
    '            cmd.AddInParameter("@cc_number", SqlDbType.VarChar, 30).Value = SqlString.Null
    '        Else
    '            cmd.AddInParameter("@is_bank", SqlDbType.TinyInt).Value = 0
    '            cmd.AddInParameter("@account_number", SqlDbType.VarChar, 50).Value = SqlString.Null
    '            cmd.AddInParameter("@cc_number", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(sNumber, String.Empty)
    '        End If

    '        conPartyBank.ExecuteNonQuery(cmd)

    '        bIsValid = Cast.ToInt32(cmd.Parameters("@isValid").Value, 0) <> 0
    '        bIsNumberExisting = Cast.ToInt32(cmd.Parameters("@isNumberExisting").Value, 0) <> 0
    '    End Using

    '''<summary>
    ''' Validates the party bank provided with the party/account.
    '''</summary>
    '''<param name="conPartyBank" type="SiriusConnection"></param>
    '''<param name="oSAMErrorCollection" type="SAMErrorCollection"></param>
    '''<param name="lPartyBankKey" type="Integer"></param>
    '''<param name="lPartyKey" type="Integer"></param>
    '''<param name="lAccountKey" type="Integer"></param>
    '''<param name="bIsBank" type="Boolean"></param>
    '''<param name="sNumber" type="String"></param>
    '''<remarks></remarks> 
    Private Overloads Sub ValidatePartyBank(ByVal conPartyBank As SiriusConnection, _
                                        ByRef oSAMErrorCollection As SAMErrorCollection, _
                                        ByVal lPartyBankKey As Integer, _
                                        Optional ByVal lPartyKey As Integer = 0, _
                                        Optional ByVal lAccountKey As Integer = 0, _
                                        Optional ByVal bIsBank As Boolean = False, _
                                        Optional ByVal sNumber As String = Nothing)
        Dim r_bIsValid As Boolean = False
        Dim r_bISNumberExisting As Boolean = False

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_PartyBank_Validate")
            cmd.AddOutParameter("@isValid", SqlDbType.TinyInt)
            cmd.AddOutParameter("@isNumberExisting", SqlDbType.TinyInt)
            cmd.AddInParameter("@party_bank_id", SqlDbType.Int).Value = lPartyBankKey
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(lPartyKey, 0)
            cmd.AddInParameter("@account_id", SqlDbType.Int).Value = Cast.NullIfDefault(lAccountKey, 0)
            If bIsBank Then
                cmd.AddInParameter("@is_bank", SqlDbType.TinyInt).Value = 1
                cmd.AddInParameter("@account_number", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(sNumber, String.Empty)
                cmd.AddInParameter("@cc_number", SqlDbType.VarChar, 30).Value = SqlString.Null
            Else
                cmd.AddInParameter("@is_bank", SqlDbType.TinyInt).Value = 0
                cmd.AddInParameter("@account_number", SqlDbType.VarChar, 50).Value = SqlString.Null
                cmd.AddInParameter("@cc_number", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(sNumber, String.Empty)
            End If

            conPartyBank.ExecuteNonQuery(cmd)

            r_bIsValid = Cast.ToInt32(cmd.Parameters("@isValid").Value, 0) <> 0
            r_bISNumberExisting = Cast.ToInt32(cmd.Parameters("@isNumberExisting").Value, 0) <> 0
        End Using

        If Not r_bIsValid Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.ValidationRulesFailed, _
                                            "Either the given party bank key is invalid or it does not belong to given PartyKey/AccountKey", _
                                            "PartyBankKey", _
                                          lPartyBankKey.ToString())
        End If

        If Not String.IsNullOrEmpty(sNumber) Then
            If Not r_bISNumberExisting Then
                If bIsBank Then
                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.ValidationRulesFailed, _
                                                    "Bank account number mismatch for the given party bank item", _
                                                    "PartyBankKey", _
                                                  lPartyBankKey.ToString())
                Else
                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.ValidationRulesFailed, _
                                                "Credit card number mismatch for the given party bank item", _
                                                "PartyBankKey", _
                                              lPartyBankKey.ToString())
                End If
                
            End If
        End If

    End Sub

    '''<summary>
    ''' Validates the the transaction details given for the party
    '''</summary>
    '''<param name="conPartyBank" type="SiriusConnection"></param>
    '''<param name="bIsValidTransaction" type="Boolean"></param>
    '''<param name="bIsValidTransactionParty" type="Boolean"></param>
    '''<param name="lPartyKey" type="Integer"></param>
    '''<param name="oTransactionType" type="LastTransactionTypeWithPartyBank"></param>
    '''<param name="lTransactionKey" type="Integer"></param>
    '''<remarks></remarks> 
    Private Sub ValidatePartyBankLastTransaction(ByVal conPartyBank As SiriusConnection, _
                                        ByRef bIsValidTransaction As Boolean, _
                                        ByRef bIsValidTransactionParty As Boolean, _
                                        ByVal lPartyKey As Integer, _
                                        ByVal oTransactionType As LastTransactionTypeWithPartyBank, _
                                        ByVal lTransactionKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_PartyBank_Validate_Last_Transaction")
            cmd.AddOutParameter("@isValidTransaction", SqlDbType.TinyInt)
            cmd.AddOutParameter("@isValidTransactionParty", SqlDbType.TinyInt)
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = lPartyKey
            If oTransactionType = LastTransactionTypeWithPartyBank.CLAIM Then
                cmd.AddInParameter("@claim_id", SqlDbType.Int).Value = Cast.NullIfDefault(lTransactionKey, 0)
                cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = SqlInt32.Null
            ElseIf oTransactionType = LastTransactionTypeWithPartyBank.POLICY Then
                cmd.AddInParameter("@claim_id", SqlDbType.Int).Value = SqlInt32.Null
                cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(lTransactionKey, 0)
            End If

            conPartyBank.ExecuteNonQuery(cmd)

            bIsValidTransaction = Cast.ToInt32(cmd.Parameters("@isValidTransaction").Value, 0) <> 0
            bIsValidTransactionParty = Cast.ToInt32(cmd.Parameters("@isValidTransactionParty").Value, 0) <> 0
        End Using

    End Sub
    '''<summary>
    ''' Retrieves details of all Party bank items for a specified party.
    '''</summary>
    '''<param name="conPartyBank" type="SiriusConnection"></param>
    '''<param name="oBusiness" type="CoreBusiness"></param>
    '''<param name="oGetPartyBankDetailsRequest" type="BaseGetPartyBankDetailsPolicyRequestType"></param>   
    '''<returns>Array of BasePartyBankType</returns>
    '''<remarks></remarks> 
    Private Function GetPartyBankDetailsByPartyKey(ByVal conPartyBank As SiriusConnection, ByVal oBusiness As CoreBusiness, ByVal oGetPartyBankDetailsRequest As BaseGetPartyBankDetailsRequestType) As BasePartyBankType()
        Dim oPartyBankDetails() As BasePartyBankType = Nothing
        Dim dtPartyBankDetails As DataTable = Nothing

        ' retrieve the party bank details from the database
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PartyBank_Details_Sel")
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(oGetPartyBankDetailsRequest.PartyKey, 0)
            cmd.AddInParameter("@Account_ID", SqlDbType.Int).Value = Cast.NullIfDefault(oGetPartyBankDetailsRequest.AccountKey, 0)
            dtPartyBankDetails = conPartyBank.ExecuteDataTable(cmd)
        End Using

        ' Populate records if any, to response
        If dtPartyBankDetails IsNot Nothing AndAlso dtPartyBankDetails.Rows.Count > 0 Then
            'Get the history items if required
            Dim dtPartyBankHistory As DataTable = Nothing
            If oGetPartyBankDetailsRequest.IncludeHistory Then
                dtPartyBankHistory = GetPartyBankHistoryByPartyKey(conPartyBank, oGetPartyBankDetailsRequest.PartyKey)
            End If
            ReDim oPartyBankDetails(dtPartyBankDetails.Rows.Count - 1)
            Dim oPartyBankItem As BasePartyBankType = Nothing
            Dim iCountryKey As Integer = 0
            Dim iArrayIndex As Integer = 0

            For Each drPartyBank As DataRow In dtPartyBankDetails.Rows
                oPartyBankItem = New BasePartyBankType
                With oPartyBankItem
                    .RowKey = iArrayIndex
                    .PartyBankKey = Cast.ToInt32(drPartyBank.Item("party_bank_id"), 0)
                    .IsBankItem = Cast.ToBoolean(drPartyBank.Item("is_bank"), False)
                    .AccountType = Cast.ToString(drPartyBank.Item("account_type"), String.Empty)
                    .AccountKey = Cast.ToInt32(drPartyBank.Item("account_id"), 0)
                    .BankPaymentTypeKey = Cast.ToInt32(drPartyBank.Item("bank_payment_type_id"), 0)
                    If .BankPaymentTypeKey > 0 Then
                        .BankPaymentTypeCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.BankPaymentType, .BankPaymentTypeKey)
                    Else
                        .BankPaymentTypeCode = String.Empty
                    End If
                    .AccountHolderName = Cast.ToString(drPartyBank.Item("account_holder_name"), String.Empty)
                    .IsDeleted = Cast.ToBoolean(drPartyBank.Item("is_deleted"), False)

                    If Cast.ToInt32(drPartyBank.Item("PFLINKEXISTS"), 0) > 0 Or Cast.ToInt32(drPartyBank.Item("CLILINKEXISTS"), 0) > 0 Or Cast.ToInt32(drPartyBank.Item("CPLINKEXISTS"), 0) > 0 Then
                        .IsPartyBankInUse = True
                    Else
                        .IsPartyBankInUse = False
                    End If

                    If Cast.ToInt32(drPartyBank.Item("PFLINKEXISTS"), 0) > 0 Then
                        .IsPartyBankLinkedWithInst = True
                    Else
                        .IsPartyBankLinkedWithInst = False
                    End If

                    'Populate bank details or credit card details according to party bank item type
                    If oPartyBankItem.IsBankItem Then
                        Dim sDescription As String = ""
                        .CreditCard = Nothing
                        .Bank = New BaseBankType
                        With .Bank
                            .AccountNumber = Cast.ToString(drPartyBank.Item("account_number"), String.Empty)
                            .BankKey = Cast.ToInt32(drPartyBank.Item("bank_name_id"), 0)
                            If .BankKey > 0 Then
                                .BankCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.CashListItemBank, .BankKey, sDescription)
                                .BankName = sDescription
                            Else
                                .BankCode = String.Empty
                            End If
                            .Branch = Cast.ToString(drPartyBank.Item("bank_branch"), String.Empty)
                            .BranchCode = Left(Cast.ToString(drPartyBank.Item("bank_branch_code"), String.Empty), 8)
                            .BIC = Cast.ToString(drPartyBank.Item("business_identifier_code"), String.Empty)
                            .IBAN = Cast.ToString(drPartyBank.Item("international_bank_account_number"), String.Empty)

                            'Bank address details
                            .BankAddress = New BaseSimpleAddressType
                            With .BankAddress
                                .AddressLine1 = Cast.ToString(drPartyBank.Item("bank_add1"), String.Empty)
                                .AddressLine2 = Cast.ToString(drPartyBank.Item("bank_add2"), String.Empty)
                                .AddressLine3 = Cast.ToString(drPartyBank.Item("bank_town"), String.Empty)
                                .AddressLine4 = Cast.ToString(drPartyBank.Item("bank_region"), String.Empty)
                                .PostCode = Cast.ToString(drPartyBank.Item("bank_pcode"), String.Empty)
                                iCountryKey = Convert.ToInt32(Cast.ToString(drPartyBank.Item("bank_country"), "0"))
                                If iCountryKey > 0 Then
                                    .CountryCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.Country, iCountryKey)
                                Else
                                    .CountryCode = String.Empty
                                End If
                            End With
                        End With
                    Else
                        .Bank = Nothing
                        .CreditCard = New BaseCreditCardType

                        With .CreditCard
                            .Number = Cast.ToString(drPartyBank.Item("cc_num"), String.Empty)
                            .NameOnCreditCard = Cast.ToString(drPartyBank.Item("name_on_card"), String.Empty)
                            .StartDate = Cast.ToString(drPartyBank.Item("cc_start_date"), String.Empty)
                            .ExpiryDate = Cast.ToString(drPartyBank.Item("cc_expiry_date"), String.Empty)
                            .Issue = Cast.ToString(drPartyBank.Item("cc_issue_num"), String.Empty)
                            .Pin = Cast.ToString(drPartyBank.Item("cc_pin"), String.Empty)
                            .ManualAuthCode = Cast.ToString(drPartyBank.Item("manual_auth_number"), String.Empty)
                            .IsRegisteredCardHolder = Cast.ToBoolean(drPartyBank.Item("is_registered"), False)

                            'Card Holder Address
                            .CardHolder = New BaseCreditCardTypeCardHolder
                            With .CardHolder
                                .Name = Cast.ToString(drPartyBank.Item("account_holder_name"), String.Empty)
                                .AddressLine1 = Cast.ToString(drPartyBank.Item("cc_add1"), String.Empty)
                                .AddressLine2 = Cast.ToString(drPartyBank.Item("cc_add2"), String.Empty)
                                .AddressLine3 = Cast.ToString(drPartyBank.Item("cc_town"), String.Empty)
                                .AddressLine4 = Cast.ToString(drPartyBank.Item("cc_add3"), String.Empty)
                                .PostCode = Cast.ToString(drPartyBank.Item("cc_pcode"), String.Empty)
                                iCountryKey = Convert.ToInt32(Cast.ToString(drPartyBank.Item("cc_country"), "0"))

                                If iCountryKey > 0 Then
                                    .CountryCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.Country, iCountryKey)
                                Else
                                    .CountryCode = String.Empty
                                End If
                            End With
                        End With
                    End If
                End With

                'Party bank history
                If oGetPartyBankDetailsRequest.IncludeHistory AndAlso dtPartyBankHistory IsNot Nothing Then
                    oPartyBankItem.History = GetPartyBankHistoryByPartyBankKey(conPartyBank, oBusiness, dtPartyBankHistory, oPartyBankItem.PartyBankKey, oPartyBankItem.IsBankItem)
                Else
                    oPartyBankItem.History = Nothing
                End If

                oPartyBankDetails(iArrayIndex) = oPartyBankItem
                oPartyBankItem = Nothing
                iArrayIndex = iArrayIndex + 1
            Next
        End If
        Return oPartyBankDetails
    End Function

    '''<summary>
    ''' Retrieves details of Party bank item for a specified party bank item.
    '''</summary>
    '''<param name="conPartyBank" type="SiriusConnection"></param>
    '''<param name="oBusiness" type="CoreBusiness"></param>
    '''<param name="oGetPartyBankDetailsRequest" type="BaseGetPartyBankDetailsPolicyRequestType"></param>   
    '''<returns>Array of BasePartyBankType</returns>
    '''<remarks></remarks> 
    Private Function GetPartyBankDetailsByPartyBankKey(ByVal conPartyBank As SiriusConnection, ByVal oBusiness As CoreBusiness, ByVal oGetPartyBankDetailsRequest As BaseGetPartyBankDetailsRequestType) As BasePartyBankType()
        Dim oPartyBankDetails() As BasePartyBankType = Nothing
        Dim dtPartyBankDetails As New DataTable
        Dim dtPartyBankPaymentDetails As New DataTable
        Dim iCount As Integer = 0
        Dim iPartyBankKey As Integer = 0
        Dim iPartyCount As Integer = 0

        'Retrieve PartyBankKey and account type from the database 
        If oGetPartyBankDetailsRequest.BankPaymentTypeCode <> "" Then
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Get_Party_AccountsType")
                cmd.AddInParameter("@AccountID", SqlDbType.Int).Value = oGetPartyBankDetailsRequest.AccountKey
                cmd.AddInParameter("@PartyCnt", SqlDbType.Int).Value = oGetPartyBankDetailsRequest.PartyKey
                cmd.AddInParameter("@BankPaymentTypeCode", SqlDbType.VarChar).Value = oGetPartyBankDetailsRequest.BankPaymentTypeCode
                cmd.AddInParameter("@ISBank", SqlDbType.VarChar).Value = oGetPartyBankDetailsRequest.ISBank
                cmd.AddInParameter("@IncludeInActive", SqlDbType.Int).Value = 1
                dtPartyBankPaymentDetails = conPartyBank.ExecuteDataTable(cmd)
                iCount = dtPartyBankPaymentDetails.Rows.Count - 1

            End Using
        Else
            iCount = 0
        End If

        ReDim oPartyBankDetails(iCount)

        For iPartyCount = 0 To iCount

            If oGetPartyBankDetailsRequest.BankPaymentTypeCode <> "" Then
                'iPartyBankKey = Cast.ToInt32(dtPartyBankDetails.Rows(0).Item("party_bank_id"), 0)
                iPartyBankKey = Cast.ToInt32(dtPartyBankPaymentDetails.Rows(iPartyCount).Item("party_bank_id"), 0)
            Else
                iPartyBankKey = oGetPartyBankDetailsRequest.PartyBankKey
            End If

            ' Retrieve the party bank details based on PartyBankKey from the database
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PartyBank_Details_ByID")
                cmd.AddInParameter("@party_bank_id", SqlDbType.Int).Value = iPartyBankKey
                dtPartyBankDetails = conPartyBank.ExecuteDataTable(cmd)
            End Using

            ' Populate records if any, to response
            If dtPartyBankDetails IsNot Nothing AndAlso dtPartyBankDetails.Rows.Count > 0 Then
                'Get the history items if required
                Dim dtPartyBankHistory As DataTable = Nothing
                If oGetPartyBankDetailsRequest.IncludeHistory Then
                    dtPartyBankHistory = GetPartyBankHistoryByPartyKey(conPartyBank, oGetPartyBankDetailsRequest.PartyKey)
                End If

                Dim iArrayIndex As Integer = 0
                Dim oPartyBankItem As BasePartyBankType = Nothing
                Dim iCountryKey As Integer = 0
                Dim drPartyBank As DataRow = dtPartyBankDetails.Rows(0)

                oPartyBankItem = New BasePartyBankType
                With oPartyBankItem
                    .RowKey = iArrayIndex
                    .PartyBankKey = Cast.ToInt32(drPartyBank.Item("party_bank_id"), 0)
                    .IsBankItem = Cast.ToBoolean(drPartyBank.Item("is_bank"), False)
                    .AccountType = Cast.ToString(drPartyBank.Item("account_type"), String.Empty)
                    .AccountKey = Cast.ToInt32(drPartyBank.Item("account_id"), 0)
                    .BankPaymentTypeKey = Cast.ToInt32(drPartyBank.Item("bank_payment_type_id"), 0)
                    If .BankPaymentTypeKey > 0 Then
                        .BankPaymentTypeCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.BankPaymentType, .BankPaymentTypeKey)
                    Else
                        .BankPaymentTypeCode = String.Empty
                    End If
                    .AccountHolderName = Cast.ToString(drPartyBank.Item("account_holder_name"), String.Empty)
                    .IsDeleted = Cast.ToBoolean(drPartyBank.Item("is_deleted"), False)
                    If Cast.ToInt32(drPartyBank.Item("PFLINKEXISTS"), 0) > 0 Or Cast.ToInt32(drPartyBank.Item("CLILINKEXISTS"), 0) > 0 Or Cast.ToInt32(drPartyBank.Item("CPLINKEXISTS"), 0) > 0 Then
                        .IsPartyBankInUse = True
                    Else
                        .IsPartyBankInUse = False
                    End If

                    If Cast.ToInt32(drPartyBank.Item("PFLINKEXISTS"), 0) > 0 Then
                        .IsPartyBankLinkedWithInst = True
                    Else
                        .IsPartyBankLinkedWithInst = False
                    End If

                    'Populate bank details or credit card details according to party bank item type
                    If oPartyBankItem.IsBankItem Then
                        Dim sDescription As String = ""
                        .CreditCard = Nothing
                        .Bank = New BaseBankType
                        With .Bank
                            .AccountNumber = Cast.ToString(drPartyBank.Item("account_number"), String.Empty)
                            .BankKey = Cast.ToInt32(drPartyBank.Item("bank_name_id"), 0)
                            If .BankKey > 0 Then
                                .BankCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.CashListItemBank, .BankKey, sDescription)
                                .BankName = sDescription
                            Else
                                .BankCode = String.Empty
                            End If
                            .Branch = Cast.ToString(drPartyBank.Item("bank_branch"), String.Empty)
                            .BranchCode = Cast.ToString(drPartyBank.Item("bank_branch_code"), String.Empty)
                            .BIC = Cast.ToString(drPartyBank.Item("business_identifier_code"), String.Empty)
                            .IBAN = Cast.ToString(drPartyBank.Item("international_bank_account_number"), String.Empty)



                            'Bank address details
                            .BankAddress = New BaseSimpleAddressType
                            With .BankAddress
                                .AddressLine1 = Cast.ToString(drPartyBank.Item("bank_add1"), String.Empty)
                                .AddressLine2 = Cast.ToString(drPartyBank.Item("bank_add2"), String.Empty)
                                .AddressLine3 = Cast.ToString(drPartyBank.Item("bank_town"), String.Empty)
                                .AddressLine4 = Cast.ToString(drPartyBank.Item("bank_region"), String.Empty)
                                .PostCode = Cast.ToString(drPartyBank.Item("bank_pcode"), String.Empty)
                                iCountryKey = Convert.ToInt32(Cast.ToString(drPartyBank.Item("bank_country"), "0"))
                                If iCountryKey > 0 Then
                                    .CountryCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.Country, iCountryKey)
                                Else
                                    .CountryCode = String.Empty
                                End If
                            End With
                        End With
                    Else
                        .Bank = Nothing
                        .CreditCard = New BaseCreditCardType

                        With .CreditCard
                            .Number = Cast.ToString(drPartyBank.Item("cc_num"), String.Empty)
                            .NameOnCreditCard = Cast.ToString(drPartyBank.Item("name_on_card"), String.Empty)
                            .StartDate = Cast.ToString(drPartyBank.Item("cc_start_date"), String.Empty)
                            .ExpiryDate = Cast.ToString(drPartyBank.Item("cc_expiry_date"), String.Empty)
                            .Issue = Cast.ToString(drPartyBank.Item("cc_issue_num"), String.Empty)
                            .Pin = Cast.ToString(drPartyBank.Item("cc_pin"), String.Empty)
                            .ManualAuthCode = Cast.ToString(drPartyBank.Item("manual_auth_number"), String.Empty)
                            .IsRegisteredCardHolder = Cast.ToBoolean(drPartyBank.Item("is_registered"), False)

                            'Card Holder Address
                            .CardHolder = New BaseCreditCardTypeCardHolder
                            With .CardHolder
                                .AddressLine1 = Cast.ToString(drPartyBank.Item("cc_add1"), String.Empty)
                                .AddressLine2 = Cast.ToString(drPartyBank.Item("cc_add2"), String.Empty)
                                .AddressLine3 = Cast.ToString(drPartyBank.Item("cc_town"), String.Empty)
                                .AddressLine4 = Cast.ToString(drPartyBank.Item("cc_add3"), String.Empty)
                                .PostCode = Cast.ToString(drPartyBank.Item("cc_pcode"), String.Empty)
                                iCountryKey = Convert.ToInt32(Cast.ToString(drPartyBank.Item("cc_country"), "0"))
                                If iCountryKey > 0 Then
                                    .CountryCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.Country, iCountryKey)
                                Else
                                    .CountryCode = String.Empty
                                End If
                            End With
                        End With
                    End If
                End With

                'Party bank history
                If oGetPartyBankDetailsRequest.IncludeHistory AndAlso dtPartyBankHistory IsNot Nothing Then
                    oPartyBankItem.History = GetPartyBankHistoryByPartyBankKey(conPartyBank, oBusiness, dtPartyBankHistory, oPartyBankItem.PartyBankKey, oPartyBankItem.IsBankItem)
                Else
                    oPartyBankItem.History = Nothing
                End If

                oPartyBankDetails(iPartyCount) = oPartyBankItem
                oPartyBankItem = Nothing
            End If
        Next

        Return oPartyBankDetails
    End Function

    '''<summary>
    ''' Retrieves all History items for a specified party.
    '''</summary>
    '''<param name="conPartyBank" type="SiriusConnection"></param>
    '''<param name="lPartyKey" type="Integer"></param>
    '''<returns>DataTable</returns>
    '''<remarks></remarks> 
    Private Function GetPartyBankHistoryByPartyKey(ByVal conPartyBank As SiriusConnection, ByVal lPartyKey As Integer) As DataTable

        Dim dtPartyBankHistory As DataTable = Nothing

        ' retrieve the party bank details from the database
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PartyBank_History_Sel")
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = lPartyKey
            dtPartyBankHistory = conPartyBank.ExecuteDataTable(cmd)
        End Using

        Return dtPartyBankHistory
    End Function

    '''<summary>
    ''' Retrieves History items related to a party bank item.
    '''</summary>
    '''<param name="conPartyBank" type="SiriusConnection"></param>
    '''<param name="oBusiness" type="CoreBusiness"></param>
    '''<param name="dtPartyBankHistory" type="DataTable"></param>
    '''<param name="lPartyBankKey" type="Integer"></param>   
    '''<param name="bIsBank" type="Boolean"></param> 
    '''<returns>Array of BasePartyBankHistoryType</returns>
    '''<remarks></remarks> 
    Private Function GetPartyBankHistoryByPartyBankKey(ByVal conPartyBank As SiriusConnection, ByVal oBusiness As CoreBusiness, ByVal dtPartyBankHistory As DataTable, ByVal lPartyBankKey As Integer, ByVal bIsBank As Boolean) As BasePartyBankHistoryType()
        Dim oPartyBankHistory() As BasePartyBankHistoryType = Nothing
        Dim drPartyBankHistory As DataRow() = Nothing
        Dim strFilter As String = "party_bank_id=" & lPartyBankKey.ToString
        Dim sDescription As String = ""

        ' Populate records if any, to response
        If dtPartyBankHistory IsNot Nothing AndAlso dtPartyBankHistory.Rows.Count > 0 Then
            drPartyBankHistory = dtPartyBankHistory.Select(strFilter)

            If drPartyBankHistory IsNot Nothing AndAlso drPartyBankHistory.Length > 0 Then
                ReDim oPartyBankHistory(drPartyBankHistory.GetUpperBound(0))
                Dim oPartyBankHistoryItem As BasePartyBankHistoryType = Nothing

                Dim iArrayIndex As Integer = 0
                Dim lBankKey As Integer = 0
                Dim sBankCode As String = ""
                For Each drHistoryItem As DataRow In drPartyBankHistory
                    oPartyBankHistoryItem = New BasePartyBankHistoryType
                    With oPartyBankHistoryItem
                        .PartyBankKey = Cast.ToInt32(drHistoryItem.Item("party_bank_id"), 0)
                        .AccountType = Cast.ToString(drHistoryItem.Item("account_type"), String.Empty)
                        .AccountHolderName = Cast.ToString(drHistoryItem.Item("account_holder_name"), String.Empty)
                        .ActionCode = Cast.ToString(drHistoryItem.Item("action_code"), String.Empty)
                        .DateModified = Cast.ToDateTime(drHistoryItem.Item("date_modified"), DateTime.MinValue)
                        .UserName = Cast.ToString(drHistoryItem.Item("username"), String.Empty)

                        If bIsBank Then
                            'Populate bank related details
                            .AccountNumber = Cast.ToString(drHistoryItem.Item("account_number"), String.Empty)
                            lBankKey = Cast.ToInt32(drHistoryItem.Item("bank_name_id"), 0)
                            If lBankKey > 0 Then
                                sBankCode = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.CashListItemBank, lBankKey, sDescription)
                                .BankName = sDescription
                            Else
                                .BankName = String.Empty
                            End If
                            .BankBranch = Cast.ToString(drHistoryItem.Item("bank_branch"), String.Empty)
                            .BankBranchCode = Cast.ToString(drHistoryItem.Item("bank_branch_code"), String.Empty)
                            .StreetName = Cast.ToString(drHistoryItem.Item("bank_add1"), String.Empty)
                            .PostCode = Cast.ToString(drHistoryItem.Item("bank_pcode"), String.Empty)
                            .BIC = Cast.ToString(drHistoryItem.Item("business_identifier_code"), String.Empty)
                            .IBAN = Cast.ToString(drHistoryItem.Item("international_bank_account_number"), String.Empty)
                        Else
                            'Populate credit card related details
                            .AccountNumber = Cast.ToString(drHistoryItem.Item("cc_num"), String.Empty)
                            .BankName = String.Empty
                            .BankBranch = String.Empty
                            .BankBranchCode = String.Empty
                            .StreetName = Cast.ToString(drHistoryItem.Item("cc_add1"), String.Empty)
                            .PostCode = Cast.ToString(drHistoryItem.Item("cc_pcode"), String.Empty)
                        End If
                    End With

                    oPartyBankHistory(iArrayIndex) = oPartyBankHistoryItem
                    oPartyBankHistoryItem = Nothing
                    iArrayIndex = iArrayIndex + 1
                Next
            End If
        End If
        Return oPartyBankHistory
    End Function

    '''<summary>
    ''' Retrieves all History items for a specified party.
    '''</summary>
    '''<param name="conPartyBank" type="SiriusConnection"></param>
    '''<param name="lPartyKey" type="Integer"></param>
    '''<returns>DataTable</returns>
    '''<remarks></remarks> 
    Private Function GetLastTransactedPartyBankKey(ByVal conPartyBank As SiriusConnection, ByVal lTransactionKey As Integer, ByVal oTransactionType As LastTransactionTypeWithPartyBank) As Integer

        Dim lPartyBankKey As Integer

        ' retrieve the party bank details from the database
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_PartyBank_Get_Last_Transacted_ID")
            cmd.AddOutParameter("@party_bank_id", SqlDbType.Int)
            If oTransactionType = LastTransactionTypeWithPartyBank.POLICY Then
                cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = lTransactionKey
                cmd.AddInParameter("@claim_id", SqlDbType.Int).Value = SqlInt32.Null
            ElseIf oTransactionType = LastTransactionTypeWithPartyBank.CLAIM Then
                cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = SqlInt32.Null
                cmd.AddInParameter("@claim_id", SqlDbType.Int).Value = lTransactionKey
            End If

            conPartyBank.ExecuteNonQuery(cmd)

            lPartyBankKey = Cast.ToInt32(cmd.Parameters("@party_bank_id").Value, 0)
        End Using
        Return lPartyBankKey
    End Function

    '''<summary>
    ''' Adds details of a party bank item or a collection of Party bank items for a specified party.
    '''</summary>
    '''<param name="oAddPartyBankDetailsRequest" type="BaseAddPartyBankDetailsRequestType"></param>   
    '''<returns>BaseAddPartyBankDetailsResponseType</returns>
    '''<remarks></remarks> 
    Public Overloads Function AddPartyBankDetails(ByVal oAddPartyBankDetailsRequest As BaseAddPartyBankDetailsRequestType) As BaseAddPartyBankDetailsResponseType
        Using conPartyBank As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Return (AddPartyBankDetails(conPartyBank, oAddPartyBankDetailsRequest))
        End Using
    End Function
#Region "UpdatePartyBankDetails"
    'Start (BK)-(UpdatePartyBankDetails functionality)
    '''<summary>
    ''' Updates details of a party bank item or a collection of Party bank items for a specified party.
    '''</summary>
    Public Overloads Function UpdatePartyBankDetails(ByVal oUpdatePartyBankDetailsRequest As BaseUpdatePartyBankDetailsRequestType) As BaseUpdatePartyBankDetailsResponseType
        Using conPartyBank As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Return (UpdatePartyBankDetails(conPartyBank, oUpdatePartyBankDetailsRequest))
        End Using
    End Function

    Public Overloads Function UpdatePartyBankDetails(ByVal conPartyBank As SiriusConnection, ByVal oUpdatePartyBankDetailsRequest As BaseUpdatePartyBankDetailsRequestType) As BaseUpdatePartyBankDetailsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim oResponse As New BaseUpdatePartyBankDetailsResponseType
        Dim sReturnValue As String = String.Empty
        Dim bMultiPartyBankTransaction As Boolean = False
        Dim iCount As Int32
        Dim lPartyKey As Int32
        Dim nTypeofPackage As enumTypeOfPackage

        If oUpdatePartyBankDetailsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.UpdatePartyBankDetailsRequestType) Then
            nTypeofPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.UpdatePartyBankDetailsResponseType
        Else
            nTypeofPackage = enumTypeOfPackage.UnknownPackage
            Return oResponse
        End If
        'Check Mandatory Field
        oUpdatePartyBankDetailsRequest.Validate(CObj(oSAMErrorCollection))
        oSAMErrorCollection.CheckForErrors()

        'Look up validation
        Dim iSourceId As Integer
        iSourceId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oUpdatePartyBankDetailsRequest.BranchCode, "BranchCode", oSAMErrorCollection)
        oSAMErrorCollection.CheckForErrors()
        Dim oPartyBankStatus() As BaseUpdatePartyBankStatusType

        'Get party key
        For iCount = 0 To oUpdatePartyBankDetailsRequest.PartyBankDetails.GetUpperBound(0)
            ReDim Preserve oPartyBankStatus(iCount)
            sReturnValue = GetAndValidateDescriptionById(conPartyBank, "Account", "Account_Key", "Account_ID", oUpdatePartyBankDetailsRequest.PartyBankDetails(iCount).AccountKey.ToString())

            lPartyKey = Convert.ToInt32(sReturnValue)

            If oUpdatePartyBankDetailsRequest.PartyBankDetails.Length > 1 Then
                bMultiPartyBankTransaction = True
            Else
                bMultiPartyBankTransaction = False
            End If

            oPartyBankStatus(iCount) = UpdatePartyBankItem(conPartyBank, oCoreBusiness, oUpdatePartyBankDetailsRequest.PartyBankDetails(iCount), lPartyKey, bMultiPartyBankTransaction)
        Next
        oResponse.PartyBankStatus = oPartyBankStatus
        Return oResponse
    End Function

    Public Function UpdatePartyBankItem(ByVal conPartyBank As SiriusConnection, ByVal oCoreBusiness As CoreBusiness, ByVal oPartyBankItem As BasePartyBankType, ByVal lPartyKey As Integer, ByVal bMultiPartyBankTransaction As Boolean) As BaseUpdatePartyBankStatusType
        Dim oResponse As New BaseUpdatePartyBankStatusType
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim sReturnValue As String = String.Empty

        With oPartyBankItem
            oResponse.RowKey = .RowKey

            'Structure Validation
            .Validate(CObj(oSAMErrorCollection))

            If oSAMErrorCollection IsNot Nothing AndAlso oSAMErrorCollection.Count > 0 Then
                If bMultiPartyBankTransaction Then
                    'Error occured. Pass the error to item's error collection. By pass default error handling.
                    ReDim oResponse.Errors(oSAMErrorCollection.Count - 1)
                    For cntIndex As Integer = 0 To oSAMErrorCollection.Count - 1
                        oResponse.Errors(cntIndex) = ConvertToBaseImplementationSAMError(oSAMErrorCollection.Item(cntIndex))
                    Next
                    Return oResponse
                Else
                    oSAMErrorCollection.CheckForErrors()
                End If
            End If

            'Account
            If .AccountKey <> 0 Then
                Try
                    sReturnValue = GetAndValidateDescriptionById(conPartyBank, "Account", "Account_ID", "Account_Id", .AccountKey.ToString())
                    Try
                        If (String.IsNullOrEmpty(sReturnValue) OrElse Convert.ToInt32(sReturnValue) = 0) Then
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                "AccountKey", _
                                                                .AccountKey.ToString)
                        Else
                            Dim iPartyKey As Integer
                            sReturnValue = GetAndValidateDescriptionById(conPartyBank, "Account", "Account_Key", "Account_Id", .AccountKey.ToString())
                            iPartyKey = Convert.ToInt32(sReturnValue)

                            If iPartyKey = 0 Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.ValidationRulesFailed, _
                                                                     SAMInvalidData.ValidationRulesFailed.ToString, _
                                                                     "Party is not associated with given account")
                            End If

                        End If

                    Catch
                        oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                            SAMInvalidData.InvalidLookupListValue.ToString, _
                                                            "AccountKey", _
                                                            .AccountKey.ToString())
                    End Try
                Catch ex As Exception
                    oSAMErrorCollection.AddFatal(ex)
                End Try
            End If

            'Bank payment type
            Try
                sReturnValue = GetAndValidateDescriptionById(conPartyBank, PMLookupTable.BankPaymentType, "bank_payment_type_id", "Code", .BankPaymentTypeCode)
                Try
                    If (String.IsNullOrEmpty(sReturnValue) OrElse Convert.ToInt32(sReturnValue) = 0) Then
                        oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                     SAMInvalidData.InvalidLookupListValue.ToString, _
                                                     "BankPaymentCode", _
                                                     .BankPaymentTypeCode)
                    Else
                        .BankPaymentTypeKey = Convert.ToInt32(sReturnValue)

                    End If
                Catch
                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                                        "BankPaymentCode", _
                                                        .BankPaymentTypeCode)

                End Try
            Catch ex As Exception
                oSAMErrorCollection.AddFatal(ex)
            End Try


            If .IsBankItem Then
                If .Bank IsNot Nothing Then
                    Try
                        sReturnValue = GetAndValidateDescriptionById(conPartyBank, PMLookupTable.CashListItemBank, "cashlistitem_bank_id", "Code", .Bank.BankCode)
                        Try
                            If (String.IsNullOrEmpty(sReturnValue) OrElse Convert.ToInt32(sReturnValue) = 0) Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                        "BankCode", _
                                                                       .Bank.BankCode)
                            Else
                                .Bank.BankKey = Convert.ToInt32(sReturnValue)
                            End If
                        Catch
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                        "BankCode", _
                                                                       .Bank.BankCode)
                        End Try

                    Catch ex As Exception
                        oSAMErrorCollection.AddFatal(ex)
                    End Try

                    If .Bank.BankAddress IsNot Nothing AndAlso Not String.IsNullOrEmpty(.Bank.BankAddress.CountryCode) Then
                        Try
                            sReturnValue = GetAndValidateDescriptionById(conPartyBank, PMLookupTable.Country, "country_id", "Code", .Bank.BankAddress.CountryCode)
                            Try
                                If (String.IsNullOrEmpty(sReturnValue) OrElse Convert.ToInt32(sReturnValue) = 0) Then
                                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                            SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                            "BankCounrtyCode", _
                                                                           .Bank.BankAddress.CountryCode)
                                Else
                                    .Bank.BankAddress.CountryKey = Convert.ToInt32(sReturnValue)
                                End If
                            Catch
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                            SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                           "BankCounrtyCode", _
                                                                           .Bank.BankAddress.CountryCode)
                            End Try
                        Catch Ex As Exception
                            oSAMErrorCollection.AddFatal(Ex)
                        End Try
                    End If

                End If


            Else
                If .CreditCard IsNot Nothing AndAlso .CreditCard.CardHolder IsNot Nothing AndAlso Not String.IsNullOrEmpty(.CreditCard.CardHolder.CountryCode) Then
                    Try
                        sReturnValue = GetAndValidateDescriptionById(conPartyBank, PMLookupTable.Country, "country_id", "Code", .CreditCard.CardHolder.CountryCode)
                        Try
                            If (String.IsNullOrEmpty(sReturnValue) OrElse Convert.ToInt32(sReturnValue) = 0) Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                        "CardHolderCounrtyCode", _
                                                                      .CreditCard.CardHolder.CountryCode)
                            Else
                                .CreditCard.CardHolder.CountryKey = Convert.ToInt32(sReturnValue)
                            End If
                        Catch
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                       "CardHolderCounrtyCode", _
                                                                      .CreditCard.CardHolder.CountryCode)
                        End Try
                    Catch ex As Exception
                        oSAMErrorCollection.AddFatal(ex)
                    End Try
                End If

            End If



            'Check all data validation errors and assign it to party bank item.
            If oSAMErrorCollection IsNot Nothing AndAlso oSAMErrorCollection.Count > 0 Then
                If bMultiPartyBankTransaction Then
                    'Error occured. Pass the error to item's error collection. Bypass default error handling.
                    ReDim oResponse.Errors(oSAMErrorCollection.Count - 1)
                    For cntIndex As Integer = 0 To oSAMErrorCollection.Count - 1
                        oResponse.Errors(cntIndex) = ConvertToBaseImplementationSAMError(oSAMErrorCollection.Item(cntIndex))
                    Next
                    Return oResponse
                Else
                    oSAMErrorCollection.CheckForErrors()
                End If
            End If

            Try
                'Create description of event
                Dim sDescription As String = "Bank Details Created - " + .AccountHolderName + ", " + .AccountType + ", "
                If .IsBankItem Then
                    sDescription = sDescription + .Bank.BankCode + ", " + .Bank.AccountNumber
                Else
                    sDescription = sDescription + .CreditCard.Number + ", " + .CreditCard.ExpiryDate
                End If

                conPartyBank.BeginTransaction()

                'Add the party bank item to database
                .PartyBankKey = UpdatePartyBankItem(conPartyBank, oPartyBankItem, lPartyKey)

                'Add party public text
                AddPartyPublicText(conPartyBank, lPartyKey, sDescription.Trim)

                'Add party bank history
                AddPartyBankHistory(conPartyBank, oPartyBankItem, PartyBankHistoryActionCode.Amendment, lPartyKey)

                'Add eveny log entry
                Dim lEventKey As Integer
                CreateEvent(conPartyBank, lPartyKey, _
                            Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, _
                            1, _SiriusUser.UserID, Now, sDescription, _
                            Nothing, Nothing, Nothing, .AccountKey, Nothing, Nothing, Nothing, Nothing, Nothing, lEventKey)

                oResponse.PartyBankKey = .PartyBankKey
                conPartyBank.CommitTransaction()
            Catch ex As Exception
                conPartyBank.RollbackTransaction()
                oSAMErrorCollection.AddFatal(ex)
            End Try

            'Check for errors and assign it to party bank item.
            If oSAMErrorCollection IsNot Nothing AndAlso oSAMErrorCollection.Count > 0 Then
                If bMultiPartyBankTransaction Then
                    'Error occured. Pass the error to item's error collection. By pass default error handling.
                    ReDim oResponse.Errors(oSAMErrorCollection.Count - 1)
                    For cntIndex As Integer = 0 To oSAMErrorCollection.Count - 1
                        oResponse.Errors(cntIndex) = ConvertToBaseImplementationSAMError(oSAMErrorCollection.Item(cntIndex))
                    Next
                    Return oResponse
                Else
                    oSAMErrorCollection.CheckForErrors()
                End If
            End If

        End With
        Return oResponse
    End Function

    Private Function UpdatePartyBankItem(ByVal conPartyBank As SiriusConnection, ByVal oPartyBankItem As BasePartyBankType, ByVal lPartyKey As Integer) As Integer

        Dim lPartyBankKey As Integer

        ' retrieve the party bank details from the database
        With oPartyBankItem
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PartyBank_Details_Upd")
                cmd.AddInParameter("@party_bank_id", SqlDbType.Int).Value = Cast.NullIfDefault(.PartyBankKey, 0)
                cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(lPartyKey, 0)
                cmd.AddInParameter("@account_id", SqlDbType.Int).Value = Cast.NullIfDefault(.AccountKey, 0)
                cmd.AddInParameter("@account_holder_name", SqlDbType.VarChar, 255).Value = Cast.ToString(.AccountHolderName, String.Empty)

                cmd.AddInParameter("@bank_payment_type_id", SqlDbType.Int).Value = Cast.ToInt32(.BankPaymentTypeKey, 0)
                cmd.AddInParameter("@account_type", SqlDbType.VarChar, 255).Value = Cast.ToString(.AccountType, String.Empty)
                If .IsBankItem Then
                    cmd.AddInParameter("@is_bank", SqlDbType.TinyInt).Value = 1
                Else
                    cmd.AddInParameter("@is_bank", SqlDbType.TinyInt).Value = 0
                End If

                'Bank
                If .Bank IsNot Nothing Then
                    With .Bank
                        cmd.AddInParameter("@account_number", SqlDbType.VarChar, 50).Value = .AccountNumber
                        cmd.AddInParameter("@bank_name_id", SqlDbType.Int).Value = .BankKey
                        cmd.AddInParameter("@bank_branch", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(.Branch, String.Empty)
                        cmd.AddInParameter("@bank_branch_code", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(.BranchCode, String.Empty)
                        cmd.AddInParameter("@sBusinessIdentifierCode", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(.BIC, String.Empty)
                        cmd.AddInParameter("@sInternationalBankAccountNumber", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(.IBAN, String.Empty)



                        'Bank address
                        If .BankAddress IsNot Nothing Then
                            With .BankAddress
                                cmd.AddInParameter("@Bank_Add1", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(.AddressLine1, String.Empty)
                                cmd.AddInParameter("@Bank_Add2", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(.AddressLine2, String.Empty)
                                cmd.AddInParameter("@Bank_Add3", SqlDbType.VarChar, 40).Value = SqlString.Null
                                cmd.AddInParameter("@Bank_Town", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(.AddressLine3, String.Empty)
                                cmd.AddInParameter("@Bank_Region", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(.AddressLine4, String.Empty)
                                cmd.AddInParameter("@Bank_PCode", SqlDbType.VarChar, 20).Value = Cast.NullIfDefault(.PostCode, String.Empty)
                                cmd.AddInParameter("@Bank_Country", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(.CountryKey.ToString(), String.Empty)
                            End With
                        Else
                            cmd.AddInParameter("@Bank_Add1", SqlDbType.VarChar, 40).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_Add2", SqlDbType.VarChar, 40).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_Add3", SqlDbType.VarChar, 40).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_Town", SqlDbType.VarChar, 40).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_Region", SqlDbType.VarChar, 40).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_PCode", SqlDbType.VarChar, 20).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_Country", SqlDbType.VarChar, 30).Value = SqlString.Null
                        End If
                    End With
                Else
                    cmd.AddInParameter("@account_number", SqlDbType.VarChar, 50).Value = SqlString.Null
                    cmd.AddInParameter("@bank_name_id", SqlDbType.Int).Value = SqlInt32.Null
                    cmd.AddInParameter("@bank_branch", SqlDbType.VarChar, 50).Value = SqlString.Null
                    cmd.AddInParameter("@bank_branch_code", SqlDbType.VarChar, 50).Value = SqlString.Null

                    cmd.AddInParameter("@Bank_Add1", SqlDbType.VarChar, 40).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_Add2", SqlDbType.VarChar, 40).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_Add3", SqlDbType.VarChar, 40).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_Town", SqlDbType.VarChar, 40).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_Region", SqlDbType.VarChar, 40).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_PCode", SqlDbType.VarChar, 20).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_Country", SqlDbType.VarChar, 30).Value = SqlString.Null
                End If


                'Credit card
                If .CreditCard IsNot Nothing Then
                    With .CreditCard
                        cmd.AddInParameter("@CC_Num", SqlDbType.VarChar, 30).Value = .Number
                        cmd.AddInParameter("@CC_Expiry_Date", SqlDbType.VarChar, 10).Value = .ExpiryDate
                        cmd.AddInParameter("@CC_Start_Date", SqlDbType.VarChar, 10).Value = Cast.NullIfDefault(.StartDate, String.Empty)
                        cmd.AddInParameter("@name_on_card", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(.NameOnCreditCard, String.Empty)
                        cmd.AddInParameter("@manual_auth_number", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(.ManualAuthCode, String.Empty)
                        'cmd.AddInParameter("@cc_tracking_number", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(.TrackingNumber, String.Empty)
                        cmd.AddInParameter("@CC_Issue_Num", SqlDbType.VarChar, 2).Value = Cast.NullIfDefault(.Issue, String.Empty)
                        cmd.AddInParameter("@CC_Pin", SqlDbType.VarChar, 20).Value = Cast.NullIfDefault(.Pin, String.Empty)

                        If .IsRegisteredCardHolder Then
                            cmd.AddInParameter("@Is_Registered", SqlDbType.TinyInt).Value = 1
                        Else
                            cmd.AddInParameter("@Is_Registered", SqlDbType.TinyInt).Value = 0
                        End If

                        'Credit card address
                        If .CardHolder IsNot Nothing Then
                            With .CardHolder
                                cmd.AddInParameter("@CC_Add1", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(.AddressLine1, String.Empty)
                                cmd.AddInParameter("@CC_Add2", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(.AddressLine2, String.Empty)
                                cmd.AddInParameter("@CC_Add3", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(.AddressLine3, String.Empty)
                                cmd.AddInParameter("@CC_Town", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(.AddressLine4, String.Empty)
                                cmd.AddInParameter("@CC_PCode", SqlDbType.VarChar, 20).Value = Cast.NullIfDefault(.PostCode, String.Empty)
                                cmd.AddInParameter("@CC_Country", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(.CountryKey.ToString(), String.Empty)
                            End With
                        Else
                            cmd.AddInParameter("@CC_Add1", SqlDbType.VarChar, 100).Value = SqlString.Null
                            cmd.AddInParameter("@CC_Add2", SqlDbType.VarChar, 100).Value = SqlString.Null
                            cmd.AddInParameter("@CC_Add3", SqlDbType.VarChar, 100).Value = SqlString.Null
                            cmd.AddInParameter("@CC_Town", SqlDbType.VarChar, 100).Value = SqlString.Null
                            cmd.AddInParameter("@CC_PCode", SqlDbType.VarChar, 20).Value = SqlString.Null
                            cmd.AddInParameter("@CC_Country", SqlDbType.VarChar, 30).Value = SqlString.Null
                        End If
                    End With
                Else
                    cmd.AddInParameter("@CC_Num", SqlDbType.VarChar, 30).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Expiry_Date", SqlDbType.VarChar, 10).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Start_Date", SqlDbType.VarChar, 10).Value = SqlString.Null
                    cmd.AddInParameter("@name_on_card", SqlDbType.VarChar, 100).Value = SqlString.Null
                    cmd.AddInParameter("@manual_auth_number", SqlDbType.VarChar, 50).Value = SqlString.Null
                    'cmd.AddInParameter("@cc_tracking_number", SqlDbType.VarChar, 255).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Issue_Num", SqlDbType.VarChar, 2).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Pin", SqlDbType.VarChar, 20).Value = SqlString.Null

                    cmd.AddInParameter("@Is_Registered", SqlDbType.TinyInt).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Add1", SqlDbType.VarChar, 100).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Add2", SqlDbType.VarChar, 100).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Add3", SqlDbType.VarChar, 100).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Town", SqlDbType.VarChar, 100).Value = SqlString.Null
                    cmd.AddInParameter("@CC_PCode", SqlDbType.VarChar, 20).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Country", SqlDbType.VarChar, 30).Value = SqlString.Null
                End If

                conPartyBank.ExecuteNonQuery(cmd)

                lPartyBankKey = Cast.ToInt32(cmd.Parameters("@party_bank_id").Value, 0)
            End Using
        End With

        Return lPartyBankKey
    End Function

    '(BK changes-WPR012) 
#End Region

#Region "DeletePartyBank"
    '(BK changes- WPR012)
    Public Overloads Function DeletePartyBank(ByVal oDeletePartyBankRequest As BaseDeletePartyBankDetailsRequestType) As BaseDeletePartyBankDetailsResponseType
        ' validate the request structure against the specified business rules
        Using conDeletePartyBank As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Dim oResponse As BaseDeletePartyBankDetailsResponseType
            oResponse = DeletePartyBank(conDeletePartyBank, oDeletePartyBankRequest)
            Return oResponse
        End Using
    End Function

    Public Overloads Function DeletePartyBank(ByVal conDeletePartyBank As SiriusConnection, ByVal oDeletePartyBankRequest As BaseDeletePartyBankDetailsRequestType) As BaseDeletePartyBankDetailsResponseType

        Dim oDeletePartyBankResponse As New BaseDeletePartyBankDetailsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection

        Dim nTypeOfPackage As enumTypeOfPackage

        If oDeletePartyBankRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.DeletePartyBankDetailsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oDeletePartyBankResponse = New SAMForInsuranceV2ImplementationTypes.DeletePartyBankDetailsResponseType
        Else
            Return Nothing
        End If

        ' Check mandatory fields
        'Differing from the tech spec- Validations done in the validate method
        oDeletePartyBankRequest.Validate(CObj(oSAMErrorCollection))
        oSAMErrorCollection.CheckForErrors()

        ' Delete the Party Bank details
        If oDeletePartyBankRequest.PartBankDetails IsNot Nothing Then
            If oDeletePartyBankRequest.PartBankDetails.Row IsNot Nothing Then
                Dim iLenght As Int32 = oDeletePartyBankRequest.PartBankDetails.Row.Length
                For iCount As Int32 = 0 To iLenght - 1
                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PartyBank_Details_DelDB")
                        cmd.AddInParameter("@Party_Bank_id", SqlDbType.Int).Value = Cast.ToInt32(oDeletePartyBankRequest.PartBankDetails.Row(iCount).PartyBankKey, 0)
                        conDeletePartyBank.ExecuteNonQuery(cmd)
                    End Using
                Next
            End If
        End If

        Return oDeletePartyBankResponse
    End Function
    '(BK changes-WPR012) 
#End Region

#Region "ActivatePartyBank"
    '(BK changes-WPR012)
    Public Overloads Function ActivatePartyBank(ByVal oActivatePartyBankRequest As BaseActivatePartyBankRequestType) As BaseActivatePartyBankResponseType
        ' validate the request structure against the specified business rules
        Using conActivatePartyBank As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Dim oResponse As BaseActivatePartyBankResponseType
            oResponse = ActivatePartyBank(conActivatePartyBank, oActivatePartyBankRequest)
            Return oResponse
        End Using
    End Function

    Public Overloads Function ActivatePartyBank(ByVal conActivatePartyBank As SiriusConnection, ByVal oActivatePartyBankRequest As BaseActivatePartyBankRequestType) As BaseActivatePartyBankResponseType

        Dim oActivatePartyBankResponse As New BaseActivatePartyBankResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim nTypeOfPackage As enumTypeOfPackage

        If oActivatePartyBankRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.ActivatePartyBankRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oActivatePartyBankResponse = New SAMForInsuranceV2ImplementationTypes.ActivatePartyBankResponseType
        Else
            Return Nothing
        End If

        ' Check mandatory fields
        oActivatePartyBankRequest.Validate(CObj(oSAMErrorCollection))
        oSAMErrorCollection.CheckForErrors()

        ' activate/deactivate the Party Bank details
        If oActivatePartyBankRequest.PartBankDetails IsNot Nothing Then
            If oActivatePartyBankRequest.PartBankDetails.Row IsNot Nothing Then
                Dim iLenght As Int32 = oActivatePartyBankRequest.PartBankDetails.Row.Length
                For iCount As Int32 = 0 To iLenght - 1
                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PartyBank_Details_DelUndel")
                        cmd.AddInParameter("@Party_Bank_id", SqlDbType.Int).Value = oActivatePartyBankRequest.PartBankDetails.Row(iCount).PartyBankKey
                        cmd.AddInParameter("@delete", SqlDbType.Int).Value = IIf(oActivatePartyBankRequest.PartBankDetails.Row(iCount).MakeActive = True, 0, 1)
                        conActivatePartyBank.ExecuteNonQuery(cmd)
                    End Using

                    Dim oPartyBankItemsIn As New BaseGetPartyBankDetailsRequestType
                    Dim oPartyBankItemsOut As New BaseGetPartyBankDetailsResponseType
                    Dim oPartyBankItem As BasePartyBankType = Nothing

                    oPartyBankItemsIn.PartyBankKey = oActivatePartyBankRequest.PartBankDetails.Row(iCount).PartyBankKey
                    oPartyBankItemsOut.PartyBankDetails = GetPartyBankDetailsByPartyBankKey(conActivatePartyBank, oCoreBusiness, oPartyBankItemsIn)

                    With oPartyBankItemsOut
                        'Party bank items
                        If .PartyBankDetails IsNot Nothing Then
                            'Temporary objects to hold the party bank details
                            Dim oPartyBankDetails() As BasePartyBankType = Nothing
                            Dim oBankAddress As BaseSimpleAddressType = Nothing
                            Dim oBank As BaseBankType = Nothing
                            Dim oCreditCard As BaseCreditCardType = Nothing
                            Dim oCardHolder As BaseCreditCardTypeCardHolder = Nothing
                            Dim oHistory As BasePartyBankHistoryType = Nothing

                            ReDim oPartyBankDetails(.PartyBankDetails.GetUpperBound(0))

                            For cntIndex As Integer = 0 To .PartyBankDetails.GetUpperBound(0)
                                oPartyBankItem = New BasePartyBankType
                                With .PartyBankDetails(cntIndex)
                                    oPartyBankItem.PartyBankKey = .PartyBankKey
                                    oPartyBankItem.BankPaymentTypeCode = .BankPaymentTypeCode
                                    If .BankPaymentTypeKey <> 0 Then
                                        oPartyBankItem.BankPaymentTypeKey = .BankPaymentTypeKey
                                    End If
                                    oPartyBankItem.AccountKey = .AccountKey
                                    oPartyBankItem.AccountHolderName = .AccountHolderName
                                    oPartyBankItem.AccountType = .AccountType
                                    oPartyBankItem.IsBankItem = .IsBankItem
                                    oPartyBankItem.RowKey = .RowKey
                                    If .Bank IsNot Nothing Then
                                        oBank = New BaseBankType
                                        With .Bank
                                            oBank.AccountNumber = .AccountNumber
                                            oBank.BankCode = .BankCode
                                            oBank.Branch = .Branch
                                            oBank.BranchCode = .BranchCode
                                            oBank.BIC = .BIC
                                            oBank.IBAN = .IBAN

                                            'Bank Address Details
                                            If .BankAddress IsNot Nothing Then
                                                oBankAddress = New BaseSimpleAddressType
                                                With .BankAddress
                                                    oBankAddress.AddressLine1 = .AddressLine1
                                                    oBankAddress.AddressLine2 = .AddressLine2
                                                    oBankAddress.AddressLine3 = .AddressLine3
                                                    oBankAddress.AddressLine4 = .AddressLine4
                                                    oBankAddress.PostCode = .PostCode
                                                    oBankAddress.CountryCode = .CountryCode
                                                End With
                                                oBank.BankAddress = oBankAddress
                                            End If
                                        End With
                                        oPartyBankItem.Bank = oBank
                                    End If

                                    If .History IsNot Nothing Then
                                        oHistory = New BasePartyBankHistoryType
                                        With .History(cntIndex)
                                            oHistory.AccountHolderName = .AccountHolderName
                                            oHistory.AccountNumber = .AccountNumber
                                            oHistory.AccountType = .AccountType
                                            oHistory.ActionCode = .ActionCode
                                            oHistory.BankBranchCode = .BankBranchCode
                                            oHistory.BankName = .BankName
                                            oHistory.DateModified = .DateModified
                                            oHistory.PartyBankKey = .PartyBankKey
                                            oHistory.PostCode = .PostCode
                                            oHistory.StreetName = .StreetName
                                            oHistory.UserName = .UserName
                                        End With
                                        ReDim oPartyBankItem.History(cntIndex)
                                        oPartyBankItem.History(cntIndex) = New BasePartyBankHistoryType
                                        oPartyBankItem.History(cntIndex) = oHistory
                                    End If

                                    If .CreditCard IsNot Nothing Then
                                        oCreditCard = New BaseCreditCardType
                                        With .CreditCard
                                            oCreditCard.Number = .Number
                                            oCreditCard.StartDate = .StartDate
                                            oCreditCard.ExpiryDate = .ExpiryDate
                                            oCreditCard.NameOnCreditCard = .NameOnCreditCard
                                            oCreditCard.Issue = .Issue
                                            oCreditCard.Pin = .Pin
                                            oCreditCard.IsRegisteredCardHolder = .IsRegisteredCardHolder
                                            oCreditCard.ManualAuthCode = .ManualAuthCode
                                            oCreditCard.TrackingNumber = .TrackingNumber

                                            'CardHolder Address Details
                                            If .CardHolder IsNot Nothing Then
                                                oCardHolder = New BaseCreditCardTypeCardHolder
                                                With .CardHolder
                                                    oCardHolder.AddressLine1 = .AddressLine1
                                                    oCardHolder.AddressLine2 = .AddressLine2
                                                    oCardHolder.AddressLine3 = .AddressLine3
                                                    oCardHolder.AddressLine4 = .AddressLine4
                                                    oCardHolder.PostCode = .PostCode
                                                    oCardHolder.CountryCode = .CountryCode
                                                End With
                                                oCreditCard.CardHolder = oCardHolder
                                            End If
                                        End With
                                        oPartyBankItem.CreditCard = oCreditCard
                                    End If

                                End With
                                oPartyBankDetails(cntIndex) = oPartyBankItem
                            Next
                            oPartyBankItemsOut.PartyBankDetails = oPartyBankDetails
                        End If
                    End With

                    'Add party bank history
                    AddPartyBankHistory(conActivatePartyBank, oPartyBankItem, CStr(IIf(oActivatePartyBankRequest.PartBankDetails.Row(iCount).MakeActive = True, 0, 1)), oActivatePartyBankRequest.PartBankDetails.Row(iCount).PartyBankKey)
                Next
            End If
        End If

        Return oActivatePartyBankResponse

    End Function
    '(BK changes-wpr012) 
#End Region

#Region "ValidateBankAccountNumber"
    '(BK changes- wpr012)
    Public Overloads Function ValidateBankAccountNumber(ByVal ValidateBankAccountNumberRequest As BaseValidateBankAccountNumberRequestType) As BaseValidateBankAccountNumberResponseType

        Dim oSAMErrorCollection As New SAMErrorCollection
        ' validate the request structure against the specified business rules\

        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                        _SiriusUser.Username, _SiriusUser.SourceID, _
                                        _SiriusUser.LanguageID, _
                                        SiriusUserDefaults.AppName)

            Dim oResponse As New BaseValidateBankAccountNumberResponseType
            Try
                oResponse = ValidateBankAccountNumber(con, ValidateBankAccountNumberRequest)

            Catch ex As Exception
                Throw
            End Try
            Return oResponse
        End Using

    End Function

    Public Overloads Function ValidateBankAccountNumber(ByVal con As SiriusConnection, ByVal ValidateBankAccountNumberRequest As BaseValidateBankAccountNumberRequestType) As BaseValidateBankAccountNumberResponseType


        'Declare the Response object 
        Dim oResponse As New BaseImplementationTypes.BaseValidateBankAccountNumberResponseType
        Dim obMediaTypeValidationbusiness As bSirMediaTypeValidation.Business
        Dim oErrorCollection As New SAMErrorCollection
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim sBankName As String = ""
        Dim bValid As Boolean = Nothing
        Dim sAddressLine1 As String = ""
        Dim sAddressLine2 As String = ""
        Dim sAddressLine3 As String = ""
        Dim sAddressLine4 As String = ""
        Dim sPostalCode As String = ""
        Dim bValidationOverridable As Boolean
        Dim vValidationMessage As Object

        Dim STSError As New STSErrorPublisher
        Dim oGetPartyBankRequest As New SAMForInsuranceV2ImplementationTypes.GetPartyBankDetailsRequestType
        Dim oGetPartyBankResponse As New SAMForInsuranceV2ImplementationTypes.GetPartyBankDetailsResponseType

        Try
            obMediaTypeValidationbusiness = New bSirMediaTypeValidation.Business
            Dim nTypeOfPackage As enumTypeOfPackage

            If ValidateBankAccountNumberRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.ValidateBankAccountNumberRequestType) Then
                nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
                oResponse = New SAMForInsuranceV2ImplementationTypes.ValidateBankAccountNumberResponseType
            Else
                nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            End If

            ' Check that mandatory base request fields have been provided
            ' BranchCode
            If String.IsNullOrEmpty(ValidateBankAccountNumberRequest.BranchCode) Then
                STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
            ElseIf ValidateBankAccountNumberRequest.BranchCode = "" Then
                ValidateBankAccountNumberRequest.SourceId = 1 'gaurav

            End If

            ' BankMediaID
            If Cast.ToInt32(ValidateBankAccountNumberRequest.BankMediaKey, 0) = 0 Then
                STSError.AddInvalidField("BankMediaKey", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, " BankMediaKey "), "")
            End If
            ' BankCountryID
            If Cast.ToInt32(ValidateBankAccountNumberRequest.BankCountryKey, 0) = 0 Then
                STSError.AddInvalidField("BankCountryKey ", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, " BankCountryKey "), "")
            End If
            ' AccountNumber
            If String.IsNullOrEmpty(ValidateBankAccountNumberRequest.AccountNumber) Then
                STSError.AddInvalidField("AccountNumber ", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, " AccountNumber "), "")
            End If

            SAMFunc.InitialiseSBOObject(con, obMediaTypeValidationbusiness, _SiriusUser, sObjectName:="bSirMediaTypeValidation.Business")

            sBankName = ValidateBankAccountNumberRequest.BankName
            With ValidateBankAccountNumberRequest


                obMediaTypeValidationbusiness.ValidateNumber(v_lMediaID:=.BankMediaKey,
                                                             v_lCountryID:=.BankCountryKey,
                                                             v_sNumber:=.AccountNumber,
                                                             r_bValid:=bValid,
                                                             r_sBankName:=sBankName,
                                                             r_sAddress1:=sAddressLine1,
                                                             r_sAddress2:=sAddressLine2,
                                                             r_sAddress3:=sAddressLine3,
                                                             r_sAddress4:=sAddressLine4,
                                                             r_sPostalCode:=sPostalCode,
                                                             r_vValidationMessage:=vValidationMessage,
                                                             r_bValidationOverridable:=bValidationOverridable,
                                                             sMediaCode:=.BankMediaCode,
                                                             sBIC:=.BIC,
                                                             sIBAN:=.IBAN)
            End With


            Dim ds As New DataSet
            ds.DataSetName = "BaseValidateBankAccountNumberResponseTypeValidationDetails"

            Dim dr As DataRow
            Dim dt As DataTable
            Dim i As Integer
            dt = New DataTable("Row")

            Dim dc1 As DataColumn = New DataColumn("BankName")
            dc1.DataType = System.Type.GetType("System.String")
            dt.Columns.Add(dc1)

            Dim dc2 As DataColumn = New DataColumn("IsValid")
            dc2.DataType = System.Type.GetType("System.Boolean")
            dt.Columns.Add(dc2)

            Dim dc3 As DataColumn = New DataColumn("IsValidSpecified")
            dc3.DataType = System.Type.GetType("System.Boolean")
            dt.Columns.Add(dc3)

            Dim dc4 As DataColumn = New DataColumn("AddressLine1")
            dc4.DataType = System.Type.GetType("System.String")
            dt.Columns.Add(dc4)

            Dim dc5 As DataColumn = New DataColumn("AddressLine2")
            dc5.DataType = System.Type.GetType("System.String")
            dt.Columns.Add(dc5)

            Dim dc6 As DataColumn = New DataColumn("AddressLine3")
            dc6.DataType = System.Type.GetType("System.String")
            dt.Columns.Add(dc6)

            Dim dc7 As DataColumn = New DataColumn("AddressLine4")
            dc7.DataType = System.Type.GetType("System.String")
            dt.Columns.Add(dc7)

            Dim dc8 As DataColumn = New DataColumn("PostalCode")
            dc8.DataType = System.Type.GetType("System.String")
            dt.Columns.Add(dc8)

            Dim dc9 As DataColumn = New DataColumn("ValidationMessageDataset")
            dc9.DataType = System.Type.GetType("System.String")
            dt.Columns.Add(dc9)

            Dim dc10 As DataColumn = New DataColumn("IsValidationOverridable")
            dc10.DataType = System.Type.GetType("System.Boolean")
            dt.Columns.Add(dc10)

            Dim dc11 As DataColumn = New DataColumn("IsValidationOverridableSpecified")
            dc11.DataType = System.Type.GetType("System.Boolean")
            dt.Columns.Add(dc11)


            Dim iCount As Integer = 0
            Dim sValidationMessage As New StringBuilder
            Dim aValidationMessage As Object()

            aValidationMessage = CType(vValidationMessage, Object())

            If IsArray(vValidationMessage) Then
                For iCount = 0 To aValidationMessage.GetUpperBound(0)
                    If aValidationMessage(iCount) IsNot Nothing AndAlso Not String.IsNullOrEmpty(aValidationMessage(iCount).ToString) Then
                        sValidationMessage.Append(aValidationMessage(iCount).ToString & "</br>")
                    End If
                Next

            End If


            dr = dt.NewRow()
            dr.Item("BankName") = sBankName
            dr.Item("IsValid") = CBool(bValid)
            dr.Item("IsValidSpecified") = CBool(bValid)
            dr.Item("AddressLine1") = sAddressLine1
            dr.Item("AddressLine2") = sAddressLine2
            dr.Item("AddressLine3") = sAddressLine3
            dr.Item("AddressLine4") = sAddressLine4
            dr.Item("PostalCode") = sPostalCode
            dr.Item("ValidationMessageDataset") = sValidationMessage
            dr.Item("IsValidationOverridable") = bValidationOverridable
            dr.Item("IsValidationOverridableSpecified") = bValidationOverridable

            dt.Rows.Add(dr)
            ds.Tables.Add(dt)
            If ValidateBankAccountNumberRequest.WCFSecurityToken = "" Then
            For i = 0 To ds.Tables(0).Rows.Count - 1
                Dim oXMLDoc As New System.Xml.XmlDocument
                oXMLDoc.LoadXml(ds.GetXml)
                oResponse.ResultDataset = oXMLDoc.DocumentElement
            Next i
            End If
            oResponse.ResultData = ds
        Catch ex As Exception
            oErrorCollection.AddFatal(ex)
        Finally
            obMediaTypeValidationbusiness.Dispose()
            obMediaTypeValidationbusiness = Nothing
        End Try
        Return oResponse

    End Function

    '(BK changes -WPR012) 
#End Region

    '''<summary>
    ''' Adds details of a party bank item or a collection of Party bank items for a specified party.
    '''</summary>
    '''<param name="conPartyBank" type="SiriusConnection"></param>
    '''<param name="oAddPartyBankDetailsRequest" type="BaseAddPartyBankDetailsRequestType"></param>   
    '''<returns>BaseAddPartyBankDetailsResponseType</returns>
    '''<remarks></remarks> 
    Public Overloads Function AddPartyBankDetails(ByVal conPartyBank As SiriusConnection, ByVal oAddPartyBankDetailsRequest As BaseAddPartyBankDetailsRequestType) As BaseAddPartyBankDetailsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim oResponse As New BaseAddPartyBankDetailsResponseType
        Dim sReturnValue As String = String.Empty
        Dim bMultiPartyBankTransaction As Boolean = False
        Dim oPartyBankStatus() As BaseAddPartyBankStatusType = Nothing

        Dim nTypeofPackage As enumTypeOfPackage
        If oAddPartyBankDetailsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.AddPartyBankDetailsRequestType) Then
            nTypeofPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.AddPartyBankDetailsResponseType
        Else
            nTypeofPackage = enumTypeOfPackage.UnknownPackage
            Return oResponse
        End If

        'Check Mandatory Field
        oAddPartyBankDetailsRequest.Validate(CObj(oSAMErrorCollection))
        oSAMErrorCollection.CheckForErrors()

        'Look up validation
        Dim iSourceId As Integer
        iSourceId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oAddPartyBankDetailsRequest.BranchCode, "BranchCode", oSAMErrorCollection)
        oSAMErrorCollection.CheckForErrors()

        'Party 
        If oAddPartyBankDetailsRequest.PartyKey <> 0 Then
            sReturnValue = GetAndValidateDescriptionById(conPartyBank, "Party", "Party_cnt", "Party_Cnt", oAddPartyBankDetailsRequest.PartyKey.ToString())
            Try
                If (String.IsNullOrEmpty(sReturnValue) OrElse Convert.ToInt32(sReturnValue) = 0) Then
                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                            SAMInvalidData.InvalidLookupListValue.ToString, _
                                                            "PartyKey", _
                                                           oAddPartyBankDetailsRequest.PartyKey.ToString())
                End If
            Catch
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                            SAMInvalidData.InvalidLookupListValue.ToString, _
                                                            "PartyKey", _
                                                           oAddPartyBankDetailsRequest.PartyKey.ToString())
            End Try
            oSAMErrorCollection.CheckForErrors()
        End If

        With oAddPartyBankDetailsRequest
            If .PartyBankDetails.Length > 1 Then
                bMultiPartyBankTransaction = True
            Else
                bMultiPartyBankTransaction = False
            End If

            ReDim oPartyBankStatus(.PartyBankDetails.GetUpperBound(0))

            For cntItems As Integer = .PartyBankDetails.GetLowerBound(0) To .PartyBankDetails.GetUpperBound(0)
                If .PartyBankDetails(cntItems) IsNot Nothing Then
                    oPartyBankStatus(cntItems) = AddPartyBankItem(conPartyBank, oCoreBusiness, .PartyBankDetails(cntItems), .PartyKey, bMultiPartyBankTransaction)
                End If
            Next
        End With
        oResponse.PartyBankStatus = oPartyBankStatus

        Return oResponse
    End Function

    ''' <summary>
    ''' Adds details of a party bank item for a specified party.
    ''' </summary>
    '''<param name="conPartyBank" type="SiriusConnection"></param>
    '''<param name="oCoreBusiness" type="CoreBusiness"></param>
    '''<param name="oPartyBankItem" type="BasePartyBankType"></param>   
    '''<param name="lPartyKey" type="Integer"></param>  
    '''<param name="bMultiPartyBankTransaction" type="Boolean"></param>  
    ''' <returns>object of type BaseAddPartyBankStatusType</returns>
    ''' <remarks></remarks>
    Public Function AddPartyBankItem(ByVal conPartyBank As SiriusConnection, ByVal oCoreBusiness As CoreBusiness, ByVal oPartyBankItem As BasePartyBankType, ByVal lPartyKey As Integer, ByVal bMultiPartyBankTransaction As Boolean) As BaseAddPartyBankStatusType
        Dim oResponse As New BaseAddPartyBankStatusType
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim sReturnValue As String = String.Empty

        With oPartyBankItem
            oResponse.RowKey = .RowKey

            'Structure Validation
            .Validate(CObj(oSAMErrorCollection))

            If .PartyBankKey <> 0 Then
                oSAMErrorCollection.AddInvalidData( _
                    SAMConstants.SAMInvalidData.ValidationRulesFailed, _
                    "AddPartyBank method does not update existing party bank item.", _
                    "ParyBankKey", .PartyBankKey.ToString())
            End If

            If oSAMErrorCollection IsNot Nothing AndAlso oSAMErrorCollection.Count > 0 Then
                If bMultiPartyBankTransaction Then
                    'Error occured. Pass the error to item's error collection. By pass default error handling.
                    ReDim oResponse.Errors(oSAMErrorCollection.Count - 1)
                    For cntIndex As Integer = 0 To oSAMErrorCollection.Count - 1
                        oResponse.Errors(cntIndex) = ConvertToBaseImplementationSAMError(oSAMErrorCollection.Item(cntIndex))
                    Next
                    Return oResponse
                Else
                    oSAMErrorCollection.CheckForErrors()
                End If
            End If

            'Account
            If .AccountKey <> 0 Then
                Try
                    sReturnValue = GetAndValidateDescriptionById(conPartyBank, "Account", "Account_ID", "Account_Id", .AccountKey.ToString())
                    Try
                        If (String.IsNullOrEmpty(sReturnValue) OrElse Convert.ToInt32(sReturnValue) = 0) Then
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                    SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                    "AccountKey", _
                                                                   .AccountKey.ToString())
                        Else
                            Dim iPartyKey As Integer
                            sReturnValue = GetAndValidateDescriptionById(conPartyBank, "Account", "Account_Key", "Account_Id", .AccountKey.ToString())

                            iPartyKey = Convert.ToInt32(sReturnValue)
                            If iPartyKey <> 0 AndAlso lPartyKey <> 0 AndAlso iPartyKey <> lPartyKey Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.ValidationRulesFailed, _
                                                              SAMInvalidData.ValidationRulesFailed.ToString, _
                                                              "Party is not associated with given account")
                            End If

                        End If
                    Catch
                        oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                    SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                    "AccountKey", _
                                                                   .AccountKey.ToString())
                    End Try
                Catch Ex As Exception
                    oSAMErrorCollection.AddFatal(Ex)
                End Try
            End If

            'Bank payment type
            Try
                sReturnValue = GetAndValidateDescriptionById(conPartyBank, PMLookupTable.BankPaymentType, "bank_payment_type_id", "Code", .BankPaymentTypeCode)
                Try
                    If (String.IsNullOrEmpty(sReturnValue) OrElse Convert.ToInt32(sReturnValue) = 0) Then
                        oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                "BankPaymentCode", _
                                                               .BankPaymentTypeCode)
                    Else
                        .BankPaymentTypeKey = Convert.ToInt32(sReturnValue)
                    End If
                Catch
                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                "BankPaymentCode", _
                                                               .BankPaymentTypeCode)
                End Try
            Catch Ex As Exception
                oSAMErrorCollection.AddFatal(Ex)
            End Try

            If .IsBankItem Then
                If .Bank IsNot Nothing Then
                    'Bank Name
                    Try
                        sReturnValue = GetAndValidateDescriptionById(conPartyBank, PMLookupTable.CashListItemBank, "cashlistitem_bank_id", "Code", .Bank.BankCode)
                        Try
                            If (String.IsNullOrEmpty(sReturnValue) OrElse Convert.ToInt32(sReturnValue) = 0) Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                        "BankCode", _
                                                                       .Bank.BankCode)
                            Else
                                .Bank.BankKey = Convert.ToInt32(sReturnValue)
                            End If
                        Catch
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                        "BankCode", _
                                                                       .Bank.BankCode)
                        End Try
                    Catch Ex As Exception
                        oSAMErrorCollection.AddFatal(Ex)
                    End Try

                    If .Bank.BankAddress IsNot Nothing AndAlso Not String.IsNullOrEmpty(.Bank.BankAddress.CountryCode) Then
                        'Bank country
                        Try
                            sReturnValue = GetAndValidateDescriptionById(conPartyBank, PMLookupTable.Country, "country_id", "Code", .Bank.BankAddress.CountryCode)
                            Try
                                If (String.IsNullOrEmpty(sReturnValue) OrElse Convert.ToInt32(sReturnValue) = 0) Then
                                    oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                            SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                            "BankCounrtyCode", _
                                                                           .Bank.BankAddress.CountryCode)
                                Else
                                    .Bank.BankAddress.CountryKey = Convert.ToInt32(sReturnValue)
                                End If
                            Catch
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                            SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                           "BankCounrtyCode", _
                                                                           .Bank.BankAddress.CountryCode)
                            End Try
                        Catch Ex As Exception
                            oSAMErrorCollection.AddFatal(Ex)
                        End Try
                    End If
                End If
            Else

                If .CreditCard IsNot Nothing AndAlso .CreditCard.CardHolder IsNot Nothing AndAlso Not String.IsNullOrEmpty(.CreditCard.CardHolder.CountryCode) Then
                    'Credit card country
                    Try
                        sReturnValue = GetAndValidateDescriptionById(conPartyBank, PMLookupTable.Country, "country_id", "Code", .CreditCard.CardHolder.CountryCode)
                        Try
                            If (String.IsNullOrEmpty(sReturnValue) OrElse Convert.ToInt32(sReturnValue) = 0) Then
                                oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                        "CardHolderCounrtyCode", _
                                                                      .CreditCard.CardHolder.CountryCode)
                            Else
                                .CreditCard.CardHolder.CountryKey = Convert.ToInt32(sReturnValue)
                            End If
                        Catch
                            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue, _
                                                                        SAMInvalidData.InvalidLookupListValue.ToString, _
                                                                       "CardHolderCounrtyCode", _
                                                                      .CreditCard.CardHolder.CountryCode)
                        End Try
                    Catch Ex As Exception
                        oSAMErrorCollection.AddFatal(Ex)
                    End Try
                End If
            End If

            'Check all data validation errors and assign it to party bank item.
            If oSAMErrorCollection IsNot Nothing AndAlso oSAMErrorCollection.Count > 0 Then
                If bMultiPartyBankTransaction Then
                    'Error occured. Pass the error to item's error collection. Bypass default error handling.
                    ReDim oResponse.Errors(oSAMErrorCollection.Count - 1)
                    For cntIndex As Integer = 0 To oSAMErrorCollection.Count - 1
                        oResponse.Errors(cntIndex) = ConvertToBaseImplementationSAMError(oSAMErrorCollection.Item(cntIndex))
                    Next
                    Return oResponse
                Else
                    oSAMErrorCollection.CheckForErrors()
                End If
            End If

            Try
                'Create description of event
                Dim sDescription As String = "Bank Details Created - " + .AccountHolderName + ", " + .AccountType + ", "
                If .IsBankItem Then
                    sDescription = sDescription + .Bank.BankCode + ", " + .Bank.AccountNumber
                Else
                    sDescription = sDescription + .CreditCard.Number + ", " + .CreditCard.ExpiryDate
                End If

                conPartyBank.BeginTransaction()

                'Add the party bank item to database
                .PartyBankKey = AddPartyBankItem(conPartyBank, oPartyBankItem, lPartyKey)

                'Add party public text
                AddPartyPublicText(conPartyBank, lPartyKey, sDescription.Trim)

                'Add party bank history
                AddPartyBankHistory(conPartyBank, oPartyBankItem, PartyBankHistoryActionCode.Setup, lPartyKey)

                'Add eveny log entry
                Dim lEventKey As Integer
                CreateEvent(conPartyBank, lPartyKey, _
                            Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, _
                            1, _SiriusUser.UserID, Now, sDescription, _
                            Nothing, Nothing, Nothing, .AccountKey, Nothing, Nothing, Nothing, Nothing, Nothing, lEventKey)

                oResponse.PartyBankKey = .PartyBankKey
                conPartyBank.CommitTransaction()
            Catch ex As Exception
                conPartyBank.RollbackTransaction()
                oSAMErrorCollection.AddFatal(ex)
            End Try

            'Check for errors and assign it to party bank item.
            If oSAMErrorCollection IsNot Nothing AndAlso oSAMErrorCollection.Count > 0 Then
                If bMultiPartyBankTransaction Then
                    'Error occured. Pass the error to item's error collection. By pass default error handling.
                    ReDim oResponse.Errors(oSAMErrorCollection.Count - 1)
                    For cntIndex As Integer = 0 To oSAMErrorCollection.Count - 1
                        oResponse.Errors(cntIndex) = ConvertToBaseImplementationSAMError(oSAMErrorCollection.Item(cntIndex))
                    Next
                    Return oResponse
                Else
                    oSAMErrorCollection.CheckForErrors()
                End If
            End If

        End With
        Return oResponse
    End Function

    '''<summary>
    ''' Adds details of a party bank item for a specified party.
    '''</summary>
    '''<param name="conPartyBank" type="SiriusConnection"></param>
    '''<param name="oPartyBankItem" type="BasePartyBankType"></param>
    '''<param name="lPartyKey" type="Integer"></param>
    '''<returns>Integer</returns>
    '''<remarks></remarks> 
    Private Function AddPartyBankItem(ByVal conPartyBank As SiriusConnection, ByVal oPartyBankItem As BasePartyBankType, ByVal lPartyKey As Integer) As Integer

        Dim lPartyBankKey As Integer

        ' retrieve the party bank details from the database
        With oPartyBankItem
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PartyBank_Details_Add")
                cmd.AddOutParameter("@party_bank_id", SqlDbType.Int)
                cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(lPartyKey, 0)
                cmd.AddInParameter("@account_id", SqlDbType.Int).Value = Cast.NullIfDefault(.AccountKey, 0)
                cmd.AddInParameter("@account_holder_name", SqlDbType.VarChar, 255).Value = .AccountHolderName
                cmd.AddInParameter("@bank_payment_type_id", SqlDbType.Int).Value = .BankPaymentTypeKey
                cmd.AddInParameter("@account_type", SqlDbType.VarChar, 255).Value = .AccountType
                If .IsBankItem Then
                    cmd.AddInParameter("@is_bank", SqlDbType.TinyInt).Value = 1
                Else
                    cmd.AddInParameter("@is_bank", SqlDbType.TinyInt).Value = 0
                End If

                'Bank
                If .Bank IsNot Nothing Then
                    With .Bank
                        cmd.AddInParameter("@account_number", SqlDbType.VarChar, 50).Value = .AccountNumber
                        cmd.AddInParameter("@bank_name_id", SqlDbType.Int).Value = .BankKey
                        cmd.AddInParameter("@bank_branch", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(.Branch, String.Empty)
                        cmd.AddInParameter("@bank_branch_code", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(.BranchCode, String.Empty)
                        cmd.AddInParameter("@sBusinessIdentifierCode", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(.BIC, String.Empty)
                        cmd.AddInParameter("@sInternationalBankAccountNumber", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(.IBAN, String.Empty)

                        'Bank address
                        If .BankAddress IsNot Nothing Then
                            With .BankAddress
                                cmd.AddInParameter("@Bank_Add1", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(.AddressLine1, String.Empty)
                                cmd.AddInParameter("@Bank_Add2", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(.AddressLine2, String.Empty)
                                cmd.AddInParameter("@Bank_Add3", SqlDbType.VarChar, 40).Value = SqlString.Null
                                cmd.AddInParameter("@Bank_Town", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(.AddressLine3, String.Empty)
                                cmd.AddInParameter("@Bank_Region", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(.AddressLine4, String.Empty)
                                cmd.AddInParameter("@Bank_PCode", SqlDbType.VarChar, 20).Value = Cast.NullIfDefault(.PostCode, String.Empty)
                                cmd.AddInParameter("@Bank_Country", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(.CountryKey.ToString(), String.Empty)
                            End With
                        Else
                            cmd.AddInParameter("@Bank_Add1", SqlDbType.VarChar, 40).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_Add2", SqlDbType.VarChar, 40).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_Add3", SqlDbType.VarChar, 40).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_Town", SqlDbType.VarChar, 40).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_Region", SqlDbType.VarChar, 40).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_PCode", SqlDbType.VarChar, 20).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_Country", SqlDbType.VarChar, 30).Value = SqlString.Null
                        End If
                    End With
                Else
                    cmd.AddInParameter("@account_number", SqlDbType.VarChar, 50).Value = SqlString.Null
                    cmd.AddInParameter("@bank_name_id", SqlDbType.Int).Value = SqlInt32.Null
                    cmd.AddInParameter("@bank_branch", SqlDbType.VarChar, 50).Value = SqlString.Null
                    cmd.AddInParameter("@bank_branch_code", SqlDbType.VarChar, 50).Value = SqlString.Null

                    cmd.AddInParameter("@Bank_Add1", SqlDbType.VarChar, 40).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_Add2", SqlDbType.VarChar, 40).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_Add3", SqlDbType.VarChar, 40).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_Town", SqlDbType.VarChar, 40).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_Region", SqlDbType.VarChar, 40).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_PCode", SqlDbType.VarChar, 20).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_Country", SqlDbType.VarChar, 30).Value = SqlString.Null
                End If

                'Credit card
                If .CreditCard IsNot Nothing Then
                    With .CreditCard
                        cmd.AddInParameter("@CC_Num", SqlDbType.VarChar, 30).Value = .Number
                        cmd.AddInParameter("@CC_Expiry_Date", SqlDbType.VarChar, 10).Value = .ExpiryDate
                        cmd.AddInParameter("@CC_Start_Date", SqlDbType.VarChar, 10).Value = Cast.NullIfDefault(.StartDate, String.Empty)
                        cmd.AddInParameter("@name_on_card", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(.NameOnCreditCard, String.Empty)
                        cmd.AddInParameter("@manual_auth_number", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(.ManualAuthCode, String.Empty)
                        cmd.AddInParameter("@cc_tracking_number", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(.TrackingNumber, String.Empty)
                        cmd.AddInParameter("@CC_Issue_Num", SqlDbType.VarChar, 2).Value = Cast.NullIfDefault(.Issue, String.Empty)
                        cmd.AddInParameter("@CC_Pin", SqlDbType.VarChar, 20).Value = Cast.NullIfDefault(.Pin, String.Empty)
                        If .IsRegisteredCardHolder Then
                            cmd.AddInParameter("@Is_Registered", SqlDbType.TinyInt).Value = 1
                        Else
                            cmd.AddInParameter("@Is_Registered", SqlDbType.TinyInt).Value = 0
                        End If

                        'Credit card address
                        If .CardHolder IsNot Nothing Then
                            With .CardHolder
                                cmd.AddInParameter("@CC_Add1", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(.AddressLine1, String.Empty)
                                cmd.AddInParameter("@CC_Add2", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(.AddressLine2, String.Empty)
                                cmd.AddInParameter("@CC_Add3", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(.AddressLine3, String.Empty)
                                cmd.AddInParameter("@CC_Town", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(.AddressLine4, String.Empty)
                                cmd.AddInParameter("@CC_PCode", SqlDbType.VarChar, 20).Value = Cast.NullIfDefault(.PostCode, String.Empty)
                                cmd.AddInParameter("@CC_Country", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(.CountryKey.ToString(), String.Empty)
                            End With
                        Else
                            cmd.AddInParameter("@CC_Add1", SqlDbType.VarChar, 100).Value = SqlString.Null
                            cmd.AddInParameter("@CC_Add2", SqlDbType.VarChar, 100).Value = SqlString.Null
                            cmd.AddInParameter("@CC_Add3", SqlDbType.VarChar, 100).Value = SqlString.Null
                            cmd.AddInParameter("@CC_Town", SqlDbType.VarChar, 100).Value = SqlString.Null
                            cmd.AddInParameter("@CC_PCode", SqlDbType.VarChar, 20).Value = SqlString.Null
                            cmd.AddInParameter("@CC_Country", SqlDbType.VarChar, 30).Value = SqlString.Null
                        End If
                    End With
                Else
                    cmd.AddInParameter("@CC_Num", SqlDbType.VarChar, 30).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Expiry_Date", SqlDbType.VarChar, 10).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Start_Date", SqlDbType.VarChar, 10).Value = SqlString.Null
                    cmd.AddInParameter("@name_on_card", SqlDbType.VarChar, 100).Value = SqlString.Null
                    cmd.AddInParameter("@manual_auth_number", SqlDbType.VarChar, 50).Value = SqlString.Null
                    cmd.AddInParameter("@cc_tracking_number", SqlDbType.VarChar, 255).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Issue_Num", SqlDbType.VarChar, 2).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Pin", SqlDbType.VarChar, 20).Value = SqlString.Null

                    cmd.AddInParameter("@Is_Registered", SqlDbType.TinyInt).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Add1", SqlDbType.VarChar, 100).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Add2", SqlDbType.VarChar, 100).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Add3", SqlDbType.VarChar, 100).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Town", SqlDbType.VarChar, 100).Value = SqlString.Null
                    cmd.AddInParameter("@CC_PCode", SqlDbType.VarChar, 20).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Country", SqlDbType.VarChar, 30).Value = SqlString.Null
                End If

                conPartyBank.ExecuteNonQuery(cmd)

                lPartyBankKey = Cast.ToInt32(cmd.Parameters("@party_bank_id").Value, 0)
            End Using
        End With

        Return lPartyBankKey
    End Function

    '''<summary>
    ''' Adds description of event in party public text table.
    '''</summary>
    '''<param name="conPartyBank" type="SiriusConnection"></param>
    '''<param name="lPartyKey" type="Integer"></param>
    '''<param name="sDescription" type="String"></param>
    '''<remarks></remarks> 
    Private Sub AddPartyPublicText(ByVal conPartyBank As SiriusConnection, ByVal lPartyKey As Integer, ByVal sDescription As String)

        Dim dtPartyPublicText As DataTable = Nothing
        Dim lPartyPublicTextID As Integer = 1

        'Get the last party public text id for the given party
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Party_Public_Text_saa")
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(lPartyKey, 0)

            dtPartyPublicText = conPartyBank.ExecuteDataTable(cmd)
        End Using

        If dtPartyPublicText IsNot Nothing AndAlso dtPartyPublicText.Rows.Count > 0 Then
            'Number of records denotes the maximum party public text id used so far for the given party
            lPartyPublicTextID = dtPartyPublicText.Rows.Count + 1
        End If

        'Add a new entry to party public text table
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Party_Public_Text_add")
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = lPartyKey
            cmd.AddInParameter("@party_public_text_id", SqlDbType.Int).Value = lPartyPublicTextID
            cmd.AddInParameter("@text_line", SqlDbType.VarChar, 255).Value = sDescription

            conPartyBank.ExecuteNonQuery(cmd)
        End Using
    End Sub

    '''<summary>
    ''' Adds details of a party bank item for a specified party.
    '''</summary>
    '''<param name="conPartyBank" type="SiriusConnection"></param>
    '''<param name="oPartyBankItem" type="BasePartyBankType"></param>
    '''<param name="lPartyKey" type="Integer"></param>
    '''<remarks></remarks> 
    Private Sub AddPartyBankHistory(ByVal conPartyBank As SiriusConnection, ByVal oPartyBankItem As BasePartyBankType, ByVal sActionCode As String, ByVal lPartyKey As Integer)

        ' retrieve the party bank details from the database
        With oPartyBankItem
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("Spu_partyBank_History_Add")
                cmd.AddInParameter("@party_bank_id", SqlDbType.Int).Value = .PartyBankKey
                cmd.AddInParameter("@action_Code", SqlDbType.VarChar, 10).Value = Cast.NullIfDefault(sActionCode, String.Empty)
                cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(lPartyKey, 0)
                cmd.AddInParameter("@account_id", SqlDbType.Int).Value = Cast.NullIfDefault(.AccountKey, 0)
                cmd.AddInParameter("@account_holder_name", SqlDbType.VarChar, 255).Value = .AccountHolderName
                cmd.AddInParameter("@bank_payment_type_id", SqlDbType.Int).Value = .BankPaymentTypeKey
                cmd.AddInParameter("@account_type", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(.AccountType, String.Empty)
                cmd.AddInParameter("@user_id", SqlDbType.Int).Value = Cast.NullIfDefault(_SiriusUser.UserID, 0)

                'Bank
                If .Bank IsNot Nothing Then
                    With .Bank
                        cmd.AddInParameter("@account_number", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(.AccountNumber, String.Empty)
                        cmd.AddInParameter("@bank_name_id", SqlDbType.Int).Value = Cast.NullIfDefault(.BankKey, 0)
                        cmd.AddInParameter("@bank_branch", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(.Branch, String.Empty)
                        cmd.AddInParameter("@bank_branch_code", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(.BranchCode, String.Empty)
                        cmd.AddInParameter("@sBusinessIdentifierCode", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(.BIC, String.Empty)
                        cmd.AddInParameter("@sInternationalBankAccountNumber", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(.IBAN, String.Empty)

                        'Bank address
                        If .BankAddress IsNot Nothing Then
                            With .BankAddress
                                cmd.AddInParameter("@Bank_Add1", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(.AddressLine1, String.Empty)
                                cmd.AddInParameter("@Bank_Add2", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(.AddressLine2, String.Empty)
                                cmd.AddInParameter("@Bank_Add3", SqlDbType.VarChar, 40).Value = SqlString.Null
                                cmd.AddInParameter("@Bank_Town", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(.AddressLine3, String.Empty)
                                cmd.AddInParameter("@Bank_Region", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(.AddressLine4, String.Empty)
                                cmd.AddInParameter("@Bank_PCode", SqlDbType.VarChar, 20).Value = Cast.NullIfDefault(.PostCode, String.Empty)
                                cmd.AddInParameter("@Bank_Country", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(.CountryKey.ToString(), String.Empty)
                            End With
                        Else
                            cmd.AddInParameter("@Bank_Add1", SqlDbType.VarChar, 40).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_Add2", SqlDbType.VarChar, 40).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_Add3", SqlDbType.VarChar, 40).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_Town", SqlDbType.VarChar, 40).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_Region", SqlDbType.VarChar, 40).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_PCode", SqlDbType.VarChar, 20).Value = SqlString.Null
                            cmd.AddInParameter("@Bank_Country", SqlDbType.VarChar, 30).Value = SqlString.Null
                        End If
                    End With
                Else
                    cmd.AddInParameter("@account_number", SqlDbType.VarChar, 50).Value = SqlString.Null
                    cmd.AddInParameter("@bank_name_id", SqlDbType.Int).Value = SqlInt32.Null
                    cmd.AddInParameter("@bank_branch", SqlDbType.VarChar, 50).Value = SqlString.Null
                    cmd.AddInParameter("@bank_branch_code", SqlDbType.VarChar, 50).Value = SqlString.Null

                    cmd.AddInParameter("@Bank_Add1", SqlDbType.VarChar, 40).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_Add2", SqlDbType.VarChar, 40).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_Add3", SqlDbType.VarChar, 40).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_Town", SqlDbType.VarChar, 40).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_Region", SqlDbType.VarChar, 40).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_PCode", SqlDbType.VarChar, 20).Value = SqlString.Null
                    cmd.AddInParameter("@Bank_Country", SqlDbType.VarChar, 30).Value = SqlString.Null
                End If

                'Credit card
                If .CreditCard IsNot Nothing Then
                    With .CreditCard
                        cmd.AddInParameter("@CC_Num", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(.Number, String.Empty)
                        cmd.AddInParameter("@CC_Expiry_Date", SqlDbType.VarChar, 10).Value = Cast.NullIfDefault(.ExpiryDate, String.Empty)
                        cmd.AddInParameter("@CC_Start_Date", SqlDbType.VarChar, 10).Value = Cast.NullIfDefault(.StartDate, String.Empty)
                        cmd.AddInParameter("@name_on_card", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(.NameOnCreditCard, String.Empty)
                        cmd.AddInParameter("@manual_auth_number", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(.ManualAuthCode, String.Empty)
                        cmd.AddInParameter("@cc_tracking_number", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(.TrackingNumber, String.Empty)
                        cmd.AddInParameter("@CC_Issue_Num", SqlDbType.VarChar, 2).Value = Cast.NullIfDefault(.Issue, String.Empty)
                        cmd.AddInParameter("@CC_Pin", SqlDbType.VarChar, 20).Value = Cast.NullIfDefault(.Pin, String.Empty)
                        If .IsRegisteredCardHolder Then
                            cmd.AddInParameter("@Is_Registered", SqlDbType.TinyInt).Value = 1
                        Else
                            cmd.AddInParameter("@Is_Registered", SqlDbType.TinyInt).Value = 0
                        End If

                        'Credit card address
                        If .CardHolder IsNot Nothing Then
                            With .CardHolder
                                cmd.AddInParameter("@CC_Add1", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(.AddressLine1, String.Empty)
                                cmd.AddInParameter("@CC_Add2", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(.AddressLine2, String.Empty)
                                cmd.AddInParameter("@CC_Add3", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(.AddressLine3, String.Empty)
                                cmd.AddInParameter("@CC_Town", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(.AddressLine4, String.Empty)
                                cmd.AddInParameter("@CC_PCode", SqlDbType.VarChar, 20).Value = Cast.NullIfDefault(.PostCode, String.Empty)
                                cmd.AddInParameter("@CC_Country", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(.CountryKey.ToString(), String.Empty)
                            End With
                        Else
                            cmd.AddInParameter("@CC_Add1", SqlDbType.VarChar, 100).Value = SqlString.Null
                            cmd.AddInParameter("@CC_Add2", SqlDbType.VarChar, 100).Value = SqlString.Null
                            cmd.AddInParameter("@CC_Add3", SqlDbType.VarChar, 100).Value = SqlString.Null
                            cmd.AddInParameter("@CC_Town", SqlDbType.VarChar, 100).Value = SqlString.Null
                            cmd.AddInParameter("@CC_PCode", SqlDbType.VarChar, 20).Value = SqlString.Null
                            cmd.AddInParameter("@CC_Country", SqlDbType.VarChar, 30).Value = SqlString.Null
                        End If
                    End With
                Else
                    cmd.AddInParameter("@CC_Num", SqlDbType.VarChar, 30).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Expiry_Date", SqlDbType.VarChar, 10).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Start_Date", SqlDbType.VarChar, 10).Value = SqlString.Null
                    cmd.AddInParameter("@name_on_card", SqlDbType.VarChar, 100).Value = SqlString.Null
                    cmd.AddInParameter("@manual_auth_number", SqlDbType.VarChar, 50).Value = SqlString.Null
                    cmd.AddInParameter("@cc_tracking_number", SqlDbType.VarChar, 255).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Issue_Num", SqlDbType.VarChar, 2).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Pin", SqlDbType.VarChar, 20).Value = SqlString.Null

                    cmd.AddInParameter("@Is_Registered", SqlDbType.TinyInt).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Add1", SqlDbType.VarChar, 100).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Add2", SqlDbType.VarChar, 100).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Add3", SqlDbType.VarChar, 100).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Town", SqlDbType.VarChar, 100).Value = SqlString.Null
                    cmd.AddInParameter("@CC_PCode", SqlDbType.VarChar, 20).Value = SqlString.Null
                    cmd.AddInParameter("@CC_Country", SqlDbType.VarChar, 30).Value = SqlString.Null
                End If

                conPartyBank.ExecuteNonQuery(cmd)
            End Using
        End With
    End Sub

    ''' <summary>
    ''' This method converts an error object of type Sirius.Architecture.ExceptionHandling.SAMError to SiriusFS.SAM.Structure.BaseImplementationTypes.SAMError
    ''' </summary>
    ''' <param name="oError">Error object of type Sirius.Architecture.ExceptionHandling.SAMError</param>
    ''' <returns>Error object of type SiriusFS.SAM.Structure.BaseImplementationTypes.SAMError</returns>
    ''' <remarks></remarks>
    Private Function ConvertToBaseImplementationSAMError(ByVal oError As Sirius.Architecture.ExceptionHandling.SAMError) As SiriusFS.SAM.Structure.BaseImplementationTypes.SAMError

        Dim oReturnError As SiriusFS.SAM.Structure.BaseImplementationTypes.SAMError = Nothing

        Select Case oError.GetType.FullName
            Case GetType(Sirius.Architecture.ExceptionHandling.SAMErrorInvalidData).FullName
                Dim oSError As Sirius.Architecture.ExceptionHandling.SAMErrorInvalidData = DirectCast(oError, Sirius.Architecture.ExceptionHandling.SAMErrorInvalidData)
                Dim oDError As New SiriusFS.SAM.Structure.BaseImplementationTypes.SAMErrorInvalidData
                oDError.Code = oSError.Code
                oDError.Description = oSError.Description
                oDError.FieldName = oSError.FieldName
                oDError.SuppliedValue = oSError.SuppliedValue
                oReturnError = oDError
            Case GetType(Sirius.Architecture.ExceptionHandling.SAMErrorBusinessRule).FullName
                Dim oSError As Sirius.Architecture.ExceptionHandling.SAMErrorBusinessRule = DirectCast(oError, Sirius.Architecture.ExceptionHandling.SAMErrorBusinessRule)
                Dim oDError As New SiriusFS.SAM.Structure.BaseImplementationTypes.SAMErrorBusinessRule
                oDError.Code = oSError.Code
                oDError.Description = oSError.Description
                oDError.Detail = oSError.Detail
                oReturnError = oDError
            Case GetType(Sirius.Architecture.ExceptionHandling.SAMErrorFatal).FullName
                Dim osError As Sirius.Architecture.ExceptionHandling.SAMErrorFatal = DirectCast(oError, Sirius.Architecture.ExceptionHandling.SAMErrorFatal)
                Dim oDError As New SiriusFS.SAM.Structure.BaseImplementationTypes.SAMErrorFatal
                oDError.Type = osError.Type
                oReturnError = oDError
        End Select

        Return oReturnError
    End Function

    Private Sub ValidatePartyPCTypeLookup(ByRef oBasePartyPC As BasePartyPCType, _
            ByRef oCoreBusiness As CoreBusiness, _
            ByRef STSError As STSErrorPublisher)

        If Not String.IsNullOrEmpty(oBasePartyPC.NationalityCode) Then
            Try
                Dim iNationalityId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Nationality", oBasePartyPC.NationalityCode)
            Catch ex As Exception
                STSError.AddInvalidField("NationalityCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "NationalityCode"), oBasePartyPC.NationalityCode)
            End Try
        End If

        ' Vivek: no table name is specified in Tech Spec
        'If Not String.IsNullOrEmpty(oBasePartyPC.AccommodationCode) Then
        '    Try
        '        Dim iAccommodationId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, , oBasePartyPC.AccommodationCode)
        '    Catch ex As Exception
        '        STSError.AddInvalidField("AccommodationCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "AccommodationCode"), oBasePartyPC.AccommodationCode)
        '    End Try

        ' Vivek: commented because its not working: Gaurav to help
        If Not String.IsNullOrEmpty(oBasePartyPC.SecOccupationCode) Then
            Try
                'Dim iSecOccupationId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.GisList, "2228226", oBasePartyPC.SecOccupationCode)
                Dim strSecOccupationCode As Object = GetListCode(Of String)(Core.STSListType.GisList, "2228226", _
                    oBasePartyPC.SecOccupationCode, "caption", True)
            Catch ex As Exception
                STSError.AddInvalidField("SecOccupationCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "SecOccupationCode"), oBasePartyPC.SecOccupationCode)
            End Try
        End If

        ' Vivek: commented because its not working: Gaurav to help
        If Not String.IsNullOrEmpty(oBasePartyPC.SecEmployersBusinessCode) Then
            Try
                'Dim iSecEmployersBusinessId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.GisList, "2228228", oBasePartyPC.SecEmployersBusinessCode)
                Dim strSecEmployersBusinessCode As Object = GetListCode(Of String)(Core.STSListType.GisList, "2228228", _
                    oBasePartyPC.SecEmployersBusinessCode, "caption", True)
            Catch ex As Exception
                STSError.AddInvalidField("SecEmployersBusinessCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "SecEmployersBusinessCode"), oBasePartyPC.SecEmployersBusinessCode)
            End Try
        End If

        ' Vivek: Rahul is going to give us details how to handle this
        'If oBasePartyPC.SecEmploymentStatusCodeSpecified Then
        '    Try
        '        Dim iSecEmploymentStatusId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.GisList, "2228230", oBasePartyPC.SecEmploymentStatusCode)
        '    Catch ex As Exception
        '        STSError.AddInvalidField("SecEmploymentStatusCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "SecEmploymentStatusCode"), oBasePartyPC.SecEmploymentStatusCode)
        '    End Try

        If oBasePartyPC.Lifestyle IsNot Nothing Then
            For Each oLifeStyle As BasePartyPCTypeLifestyle In oBasePartyPC.Lifestyle
                If Not String.IsNullOrEmpty(oLifeStyle.CategoryCode) Then
                    Try
                        Dim iCategoryId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "LifeStyle_Category", oLifeStyle.CategoryCode)
                    Catch ex As Exception
                        STSError.AddInvalidField("CategoryCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "CategoryCode"), oLifeStyle.CategoryCode)
                    End Try
                End If

                ' Vivek: Rahul is going to give us details how to handle this
                'If oLifeStyle.GenderCodeSpecified Then
                '    Try
                '        Dim iGenderId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.GisList, "131091", oLifeStyle.GenderCode)
                '    Catch ex As Exception
                '        STSError.AddInvalidField("GenderCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "GenderCode"), oLifeStyle.GenderCode)
                '    End Try

                ' Vivek: commented because its not working: Gaurav to help
                If Not String.IsNullOrEmpty(oLifeStyle.OccupationCode) Then
                    Try
                        'Dim iOccupationId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.GisList, "2228226", oLifeStyle.OccupationCode)
                        Dim strOccupationCode As Object = GetListCode(Of String)(Core.STSListType.GisList, "2228226", _
                            oLifeStyle.OccupationCode, "caption", True)
                    Catch ex As Exception
                        STSError.AddInvalidField("OccupationCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "OccupationCode"), oLifeStyle.OccupationCode)
                    End Try
                End If

                ' Vivek: commented because its not working: Gaurav to help
                If Not String.IsNullOrEmpty(oLifeStyle.SecOccupationCode) Then
                    Try
                        'Dim iSecOccupationId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.GisList, "2228226", oLifeStyle.SecOccupationCode)
                        Dim strSecOccupationCode As Object = GetListCode(Of String)(Core.STSListType.GisList, "2228226", _
                                oLifeStyle.SecOccupationCode, "caption", True)
                    Catch ex As Exception
                        STSError.AddInvalidField("SecOccupationCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "SecOccupationCode"), oLifeStyle.SecOccupationCode)
                    End Try
                End If
            Next
        End If

        ValidateClientSharedDataLookup(oBasePartyPC.ClientDetail, oCoreBusiness, STSError)

    End Sub

    Private Sub ValidatePartyCCTypeLookup(ByRef oBasePartyCC As BasePartyCCType, _
            ByRef oCoreBusiness As CoreBusiness, _
            ByRef STSError As STSErrorPublisher)

        If Not String.IsNullOrEmpty(oBasePartyCC.SICCode) Then
            Try
                Dim iSICId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "SIC_Code", oBasePartyCC.SICCode)
            Catch ex As Exception
                STSError.AddInvalidField("SICCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "SICCode"), oBasePartyCC.SICCode)
            End Try
        End If

        ' Vivek: commented because its not working: Gaurav to help
        If Not String.IsNullOrEmpty(oBasePartyCC.TurnoverCode) Then
            Try
                'Dim iTurnoverId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "TurnoverBand", oBasePartyCC.TurnoverCode)
                Dim strTurnoverCode As Object = GetListCode(Of String)(Core.STSListType.PMLookup, "TurnoverBand", _
                        oBasePartyCC.TurnoverCode, "code", True)
            Catch ex As Exception
                STSError.AddInvalidField("TurnoverCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "TurnoverCode"), oBasePartyCC.TurnoverCode)
            End Try
        End If

        ' Vivek: commented because its not working: Gaurav to help
        If Not String.IsNullOrEmpty(oBasePartyCC.TradeCode) Then
            Try
                'Dim iTradeId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.GisList, "2228228", oBasePartyCC.TradeCode)
                Dim strTradeCode As Object = GetListCode(Of String)(Core.STSListType.GisList, "2228228", _
                        oBasePartyCC.TradeCode, "caption", True)
            Catch ex As Exception
                STSError.AddInvalidField("TradeCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "TradeCode"), oBasePartyCC.TradeCode)
            End Try
        End If

        ValidateClientSharedDataLookup(oBasePartyCC.ClientDetail, oCoreBusiness, STSError)

    End Sub

    Private Sub ValidatePartyOtherTypeLookup(ByRef oBasePartyOther As BasePartyOTHERType, _
            ByRef oCoreBusiness As CoreBusiness, _
            ByRef STSError As STSErrorPublisher)

        ValidateConvictionType(oBasePartyOther.Convictions, oCoreBusiness, STSError)

    End Sub

    Private Sub ValidateClientSharedDataLookup(ByRef oClientData As BaseClientSharedDataType, _
            ByRef oCoreBusiness As CoreBusiness, _
            ByRef STSError As STSErrorPublisher)

        If oClientData IsNot Nothing Then

            If Not String.IsNullOrEmpty(oClientData.ServiceLevelCode) Then
                Try
                    Dim iServiceLevelId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Service_Level", oClientData.ServiceLevelCode)
                Catch ex As Exception
                    STSError.AddInvalidField("ServiceLevelCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "ServiceLevelCode"), oClientData.ServiceLevelCode)
                End Try
            End If

            If Not String.IsNullOrEmpty(oClientData.AreaCode) Then
                Try
                    Dim iAreaId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Area", oClientData.AreaCode)
                Catch ex As Exception
                    STSError.AddInvalidField("AreaCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "AreaCode"), oClientData.AreaCode)
                End Try
            End If

            If Not String.IsNullOrEmpty(oClientData.CorrespondenceCode) Then
                Try
                    Dim iCorrespondenceId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Contact_Type", oClientData.CorrespondenceCode)
                Catch ex As Exception
                    STSError.AddInvalidField("CorrespondenceCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "CorrespondenceCode"), oClientData.CorrespondenceCode)
                End Try
            End If

            ' Vivek: commented because its not working: Gaurav to help
            If Not String.IsNullOrEmpty(oClientData.PaymentCode) Then
                Try
                    'Dim iPaymentId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Payment_Method", oClientData.PaymentCode)
                    Dim strPaymentCode As Object = GetListCode(Of String)(Core.STSListType.PMLookup, "Payment_Method", _
                            oClientData.PaymentCode, "caption", True)
                Catch ex As Exception
                    STSError.AddInvalidField("PaymentCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "PaymentCode"), oClientData.PaymentCode)
                End Try
            End If

            If Not String.IsNullOrEmpty(oClientData.ReminderCode) Then
                Try
                    Dim iReminderId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Reminder_Type", oClientData.ReminderCode)
                Catch ex As Exception
                    STSError.AddInvalidField("ReminderCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "ReminderCode"), oClientData.ReminderCode)
                End Try
            End If

            ' Vivek: table name is not specified in Tech Spec
            'If Not String.IsNullOrEmpty(oClientData.PaymentTermCode) Then
            '    Try
            '        Dim iPaymentTermId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, , oClientData.PaymentTermCode)
            '    Catch ex As Exception
            '        STSError.AddInvalidField("PaymentTermCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "PaymentTermCode"), oClientData.PaymentTermCode)
            '    End Try

            If Not String.IsNullOrEmpty(oClientData.RenewalStopCode) Then
                Try
                    Dim iRenewalStopId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Renewal_Stop_Code", oClientData.RenewalStopCode)
                Catch ex As Exception
                    STSError.AddInvalidField("RenewalStopCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "RenewalStopCode"), oClientData.RenewalStopCode)
                End Try
            End If

            If Not String.IsNullOrEmpty(oClientData.SeasonalGiftCode) Then
                Try
                    Dim iSeasonalGiftId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Seasonal_Gift", oClientData.SeasonalGiftCode)
                Catch ex As Exception
                    STSError.AddInvalidField("SeasonalGiftCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "SeasonalGiftCode"), oClientData.SeasonalGiftCode)
                End Try
            End If

            If Not String.IsNullOrEmpty(oClientData.StrengthCode) Then
                Try
                    Dim iStrengthId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Strength_Code", oClientData.StrengthCode)
                Catch ex As Exception
                    STSError.AddInvalidField("StrengthCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "StrengthCode"), oClientData.StrengthCode)
                End Try
            End If

            If Not String.IsNullOrEmpty(oClientData.StatusCode) Then
                Try
                    Dim iStatusId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Prospect_Status", oClientData.StatusCode)
                Catch ex As Exception
                    STSError.AddInvalidField("StatusCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "StatusCode"), oClientData.StatusCode)
                End Try
            End If

            ' Vivek: This field doesn't belong to BaseClientSharedDataType
            'If Not String.IsNullOrEmpty(oClientData.RelationshipCode) Then
            '    Try
            '        Dim iRelationshipId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Relationship_Type", oClientData.RelationshipCode)
            '    Catch ex As Exception
            '        STSError.AddInvalidField("RelationshipCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "RelationshipCode"), oClientData.RelationshipCode)
            '    End Try

            ' Vivek: commented because its not working: Gaurav to help
            If oClientData.LoyaltyScheme IsNot Nothing Then
                For Each oLoyalty As BaseClientSharedDataTypeLoyaltyScheme In oClientData.LoyaltyScheme
                    If Not String.IsNullOrEmpty(oLoyalty.LoyaltySchemeCode) Then
                        Try
                            'Dim iLoyaltySchemeId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "LoyaltyScheme", oLoyalty.LoyaltySchemeCode)
                            Dim strLoyaltySchemeCode As Object = GetListCode(Of String)(Core.STSListType.PMLookup, "Loyalty_Scheme", _
                                    oLoyalty.LoyaltySchemeCode, "code", True)
                        Catch ex As Exception
                            STSError.AddInvalidField("LoyaltySchemeCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "Loyalty_Scheme"), oLoyalty.LoyaltySchemeCode)
                        End Try
                    End If
                Next
            End If

            If oClientData.ProspectPolicies IsNot Nothing Then
                For Each oProspectPolicy As BaseClientSharedDataTypeProspectPolicies In oClientData.ProspectPolicies
                    If Not String.IsNullOrEmpty(oProspectPolicy.ProspectTypeCode) Then
                        Try
                            Dim iProspectTypeId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Risk_Group", oProspectPolicy.ProspectTypeCode)
                        Catch ex As Exception
                            STSError.AddInvalidField("ProspectTypeCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "ProspectTypeCode"), oProspectPolicy.ProspectTypeCode)
                        End Try
                    End If
                Next
            End If

            If oClientData.Associates IsNot Nothing Then
                For Each oAssociate As BaseAssociateType In oClientData.Associates
                    If Not String.IsNullOrEmpty(oAssociate.RelationshipCode) Then
                        Try
                            Dim iRelationshipId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Relationship_Type", oAssociate.RelationshipCode)
                        Catch ex As Exception
                            STSError.AddInvalidField("RelationshipCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "RelationshipCode"), oAssociate.RelationshipCode)
                        End Try
                    End If
                Next
            End If

            ValidateConvictionType(oClientData.Convictions, oCoreBusiness, STSError)

        End If
    End Sub

    Private Sub ValidateConvictionType(ByVal oConvictions As BaseConvictionType(), _
            ByRef oCoreBusiness As CoreBusiness, _
            ByRef STSError As STSErrorPublisher)

        If oConvictions IsNot Nothing Then
            For Each oConviction As BaseConvictionType In oConvictions
                ' Vivek: commented because its not working: Gaurav to help
                If Not String.IsNullOrEmpty(oConviction.TypeCode) Then
                    Try
                        'Dim iRelationshipId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.GisList, "1114113", oConviction.TypeCode)
                        'Dim strTypeCode As Object = GetListCode(Of String)(Core.STSListType.GisList, "1245186", _
                        Dim strTypeCode As Object = GetListCode(Of String)(Core.STSListType.UserDefinedTable, "1114113", _
                                oConviction.TypeCode, "caption", True)
                    Catch ex As Exception
                        STSError.AddInvalidField("TypeCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "TypeCode"), oConviction.TypeCode)
                    End Try
                End If

                ' Vivek: commented because its not working: Gaurav to help
                If Not String.IsNullOrEmpty(oConviction.StatusCode) Then
                    Try
                        'Dim iStatusId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.GisList, "1114124", oConviction.StatusCode)
                        Dim strStatusCode As Object = GetListCode(Of String)(Core.STSListType.UserDefinedTable, "1114124", _
                                oConviction.StatusCode, "caption", True)
                    Catch ex As Exception
                        STSError.AddInvalidField("StatusCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "StatusCode"), oConviction.StatusCode)
                    End Try
                End If

                ' Vivek: commented because its not working: Gaurav to help
                If Not String.IsNullOrEmpty(oConviction.SentenceTypeCode) Then
                    Try
                        'Dim iSentenceTypeId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.GisList, "1114119", oConviction.SentenceTypeCode)
                        Dim strSentenceTypeCode As Object = GetListCode(Of String)(Core.STSListType.UserDefinedTable, "1114119", _
                                oConviction.SentenceTypeCode, "caption", True)
                    Catch ex As Exception
                        STSError.AddInvalidField("SentenceTypeCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "SentenceTypeCode"), oConviction.SentenceTypeCode)
                    End Try
                End If

                ' Vivek: commented because its not working: Gaurav to help
                If Not String.IsNullOrEmpty(oConviction.SentenceDurationQualifier) Then
                    Try
                        'Dim iSentenceDurationId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.GisList, "1114122", oConviction.SentenceDurationQualifier)
                        Dim strSentenceDurationQualifier As Object = GetListCode(Of String)(Core.STSListType.UserDefinedTable, "1114122", _
                                oConviction.SentenceDurationQualifier, "caption", True)
                    Catch ex As Exception
                        STSError.AddInvalidField("SentenceDurationQualifier", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "SentenceDurationQualifier"), oConviction.SentenceDurationQualifier)
                    End Try
                End If

                ' Vivek: commented because its not working: Gaurav to help
                If Not String.IsNullOrEmpty(oConviction.AlcoholMeasurementMethod) Then
                    Try
                        'Dim iAlcoholMeasurementMethodId As Integer = oCoreBusiness.GetListItemFromCode(Core.STSListType.GisList, "1114126", oConviction.AlcoholMeasurementMethod)
                        Dim strAlcoholMeasurementMethod As Object = GetListCode(Of String)(Core.STSListType.UserDefinedTable, "1114126", _
                                oConviction.AlcoholMeasurementMethod, "caption", True)
                    Catch ex As Exception
                        STSError.AddInvalidField("AlcoholMeasurementMethod", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.DateInputInvalid, "AlcoholMeasurementMethod"), oConviction.AlcoholMeasurementMethod)
                    End Try
                End If
            Next
        End If

    End Sub

    Private Sub ProcessOtherPartySupplierBusiness(ByVal con As SiriusConnection, _
    ByVal oSupplierBusinesses As BasePartyOTHERTypeSupplierBusiness(), _
    ByVal PartyKey As Integer)

        ' delete all existing party supplier business
        DeleteOtherPartySupplierBusiness(con, PartyKey)

        ' Vivek: 20080605 - bug fix: added checking for oSupplierBusinesses is nothing
        If oSupplierBusinesses IsNot Nothing Then
            ' add each item back in for this party
            For Each oSupplierBusiness As BasePartyOTHERTypeSupplierBusiness In oSupplierBusinesses
                AddOtherPartySupplierBusiness(con, oSupplierBusiness, PartyKey)
            Next
        End If

    End Sub

    Private Sub DeleteOtherPartySupplierBusiness(ByVal con As SiriusConnection, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Party_Supplier_Business_Del")
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    Private Sub AddOtherPartySupplierBusiness(ByVal con As SiriusConnection, _
    ByVal oSupplierBusiness As BasePartyOTHERTypeSupplierBusiness, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Party_Supplier_Business_Add")
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@supplier_business_id", SqlDbType.Int).Value = oSupplierBusiness.BusinessId

            ' supplier speciality is optional
            If oSupplierBusiness.SpecialityId <> 0 Then
                cmd.AddInParameter("@supplier_speciality_id", SqlDbType.Int).Value = oSupplierBusiness.SpecialityId
            Else
                cmd.AddInParameter("@supplier_speciality_id", SqlDbType.Int).Value = Nothing
            End If

            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    Private Sub UpdateOtherPartyDetails(ByVal con As SiriusConnection, _
    ByVal oOtherParty As BasePartyOTHERType, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_other_upd")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            ' Vivek: 20080605 - bug fix: added call to NullIfDefault for LicenseTypeId
            cmd.AddInParameter("@license_type_id", SqlDbType.Int).Value = Cast.NullIfDefault(oOtherParty.LicenseTypeId)
            cmd.AddInParameter("@license_number", SqlDbType.VarChar, 20).Value = oOtherParty.LicenseNumber
            cmd.AddInParameter("@date_of_birth", SqlDbType.DateTime).Value = CDate(Cast.DefaultIfNull(oOtherParty.DateOfBirth, Date.MinValue.ToString))
            cmd.AddInParameter("@gender", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oOtherParty.GenderDescription, String.Empty)
            cmd.AddInParameter("@party_status", SqlDbType.Int).Value = oOtherParty.DrivingStatusId
            cmd.AddInParameter("@reg_number", SqlDbType.VarChar, 20).Value = Cast.NullIfDefault(oOtherParty.RegNumber, String.Empty)

            ' if this isnt a supplier default all the specified details to false
            ' so the zero values are used rather than the ones specified
            If oOtherParty.TypeCode <> OtherParty.Supplier Then
                oOtherParty.ActiveIndicatorSpecified = False
                oOtherParty.AfterHoursIndicatorSpecified = False
                oOtherParty.PriorityIndicatorSpecified = False
            End If

            If oOtherParty.ActiveIndicatorSpecified Then
                cmd.AddInParameter("@active_indicator", SqlDbType.Bit).Value = oOtherParty.ActiveIndicator
            Else
                cmd.AddInParameter("@active_indicator", SqlDbType.Bit).Value = 0
            End If

            If oOtherParty.AfterHoursIndicatorSpecified Then
                cmd.AddInParameter("@after_hours_indicator", SqlDbType.Bit).Value = oOtherParty.AfterHoursIndicator
            Else
                cmd.AddInParameter("@after_hours_indicator", SqlDbType.Bit).Value = 0
            End If

            If oOtherParty.PriorityIndicatorSpecified Then
                cmd.AddInParameter("@priority_indicator", SqlDbType.TinyInt).Value = oOtherParty.PriorityIndicator
            Else
                cmd.AddInParameter("@priority_indicator", SqlDbType.Bit).Value = 0
            End If

            ' default the values for these fields, this data is not currently part of the request
            cmd.AddInParameter("@date_passed_test", SqlDbType.DateTime).Value = Date.MinValue
            cmd.AddInParameter("@reference_number", SqlDbType.VarChar, 20).Value = Nothing
            cmd.AddInParameter("@external_id", SqlDbType.Int).Value = Nothing
            cmd.AddInParameter("@contact_name", SqlDbType.VarChar, 255).Value = Nothing
            cmd.AddInParameter("@contact_telephone_number", SqlDbType.VarChar, 255).Value = Nothing
            cmd.AddInParameter("@insurer_name", SqlDbType.VarChar, 255).Value = Nothing
            cmd.AddInParameter("@insurer_address1", SqlDbType.VarChar, 60).Value = Nothing
            cmd.AddInParameter("@insurer_address2", SqlDbType.VarChar, 60).Value = Nothing
            cmd.AddInParameter("@insurer_address3", SqlDbType.VarChar, 60).Value = Nothing
            cmd.AddInParameter("@insurer_address4", SqlDbType.VarChar, 60).Value = Nothing
            cmd.AddInParameter("@insurer_postcode", SqlDbType.VarChar, 20).Value = Nothing
            cmd.AddInParameter("@insurer_telephone_number", SqlDbType.VarChar, 255).Value = Nothing
            cmd.AddInParameter("@insurer_fax_number", SqlDbType.VarChar, 255).Value = Nothing
            cmd.AddInParameter("@insurer_contact_name", SqlDbType.VarChar, 255).Value = Nothing
            cmd.AddInParameter("@insurer_email", SqlDbType.VarChar, 255).Value = Nothing
            cmd.AddInParameter("@insurer_notes", SqlDbType.VarChar, 2000).Value = Nothing
            cmd.AddInParameter("@company_notes", SqlDbType.VarChar, 2000).Value = Nothing

            con.ExecuteNonQuery(cmd)

        End Using

    End Sub

    Private Sub ProcessOtherPartyAccidents( _
      ByVal con As SiriusConnection, _
      ByVal oAccidents As BasePartyOTHERTypeAccident(), _
      ByVal PartyKey As Integer)

        If oAccidents IsNot Nothing Then

            ' set default processing status for each conviction
            For Each oAccident As BasePartyOTHERTypeAccident In oAccidents

                If oAccident.AccidentKey = 0 Then
                    ' update the conviction record - mark as processed
                    oAccident.ProcessingStatus = ProcessingStatus.Insert
                Else
                    ' mark all records with a valid conviction key as to be viewed
                    oAccident.ProcessingStatus = ProcessingStatus.View
                End If
            Next

            ' get existing party convictions
            Dim oExistingAccidents As BasePartyOTHERTypeAccident() = GetPartyAccidents(con, PartyKey)

            If oExistingAccidents IsNot Nothing Then

                If oAccidents IsNot Nothing Then

                    For Each oExistingAccident As BasePartyOTHERTypeAccident In oExistingAccidents

                        Dim bFound As Boolean = False

                        For Each oAccident As BasePartyOTHERTypeAccident In oAccidents

                            If oAccident.AccidentKey = oExistingAccident.AccidentKey Then

                                ' update the conviction record - mark as processed
                                oAccident.ProcessingStatus = ProcessingStatus.Update

                                bFound = True

                                Exit For

                            End If

                        Next

                        If bFound <> True Then
                            oExistingAccident.ProcessingStatus = ProcessingStatus.Delete
                        End If

                    Next

                End If

            End If

            ' process each conviction 
            For Each oAccident As BasePartyOTHERTypeAccident In oAccidents

                If oAccident.ProcessingStatus = ProcessingStatus.Insert Then

                    ' insert conviction
                    AddOtherPartyAccident(con, oAccident, PartyKey)

                ElseIf oAccident.ProcessingStatus = ProcessingStatus.Update Then

                    ' update conviction
                    UpdateOtherPartyAccident(con, oAccident, PartyKey)
                End If
            Next

            For Each oAccident As BasePartyOTHERTypeAccident In oExistingAccidents

                If oAccident.ProcessingStatus = ProcessingStatus.Delete Then
                    ' delete conviction
                    DeleteOtherPartyAccident(con, oAccident.AccidentKey, PartyKey)
                End If

            Next
        Else
            ' delete all other party convictions
            DeleteAllOtherPartyAccidents(con, PartyKey)
        End If
    End Sub

    ' Vivek: differing with Tech Spec
    Private Sub ProcessOtherPartyConvictions( _
    ByVal con As SiriusConnection, _
    ByVal oConvictions As BasePartyOTHERTypeConviction(), _
    ByVal PartyKey As Integer)

        If oConvictions IsNot Nothing Then

            ' set default processing status for each conviction
            For Each oConviction As BasePartyOTHERTypeConviction In oConvictions

                If oConviction.ConvictionKey = 0 Then

                    ' update the conviction record - mark as to be updated
                    oConviction.ProcessingStatus = ProcessingStatus.Insert
                End If
            Next

            ' get existing party convictions
            Dim oExistingConvictions As BasePartyOTHERTypeConviction() = GetPartyConvictions(con, PartyKey)

            If oExistingConvictions IsNot Nothing Then

                If oConvictions IsNot Nothing Then

                    For Each oExistingConviction As BasePartyOTHERTypeConviction In oExistingConvictions

                        Dim bFound As Boolean = False

                        For Each oConviction As BasePartyOTHERTypeConviction In oConvictions

                            If oConviction.ConvictionKey = oExistingConviction.ConvictionKey Then

                                ' update the conviction record - mark as to be updated
                                oConviction.ProcessingStatus = ProcessingStatus.Update

                                bFound = True

                                Exit For
                            End If

                        Next

                        If bFound <> True Then
                            oExistingConviction.ProcessingStatus = ProcessingStatus.Delete
                        End If
                    Next

                End If

            End If

            ' process each conviction 
            For Each oConviction As BasePartyOTHERTypeConviction In oConvictions

                If oConviction.ProcessingStatus = ProcessingStatus.Insert Then

                    ' insert conviction
                    AddOtherPartyConviction(con, oConviction, PartyKey)

                ElseIf oConviction.ProcessingStatus = ProcessingStatus.Update Then

                    ' update conviction
                    UpdateOtherPartyConviction(con, oConviction, PartyKey)

                End If

            Next

            For Each oConviction As BasePartyOTHERTypeConviction In oExistingConvictions

                If oConviction.ProcessingStatus = ProcessingStatus.Delete Then

                    ' delete conviction
                    DeleteOtherPartyConviction(con, oConviction.ConvictionKey, PartyKey)

                End If

            Next

        Else
            ' delete all other party convictions
            DeleteAllOtherPartyConvictions(con, PartyKey)
        End If
    End Sub

    ' got from Jaideep
    Private Sub ProcessPartyConvictions( _
        ByVal con As SiriusConnection, _
        ByVal oBusiness As CoreBusiness, _
        ByVal oConvictions As BaseConvictionType(), _
        ByVal PartyKey As Integer)

        If oConvictions IsNot Nothing Then

            ' set default processing status for each conviction
            For Each oConviction As BaseConvictionType In oConvictions

                If oConviction.ConvictionKey = 0 Then

                    ' update the conviction record - mark as to be updated
                    oConviction.ProcessingStatus = ProcessingStatus.Insert
                End If
            Next

            ' get existing party convictions
            Dim oExistingConvictions As BaseConvictionType() = GetPartyConvictions_v2(con, PartyKey)

            If oExistingConvictions IsNot Nothing Then

                If oConvictions IsNot Nothing Then

                    For Each oExistingConviction As BaseConvictionType In oExistingConvictions

                        Dim bFound As Boolean = False

                        For Each oConviction As BaseConvictionType In oConvictions

                            If oConviction.ConvictionKey = oExistingConviction.ConvictionKey Then

                                ' update the conviction record - mark as to be updated
                                oConviction.ProcessingStatus = ProcessingStatus.Update

                                bFound = True

                                Exit For
                            End If

                        Next

                        If bFound <> True Then
                            oExistingConviction.ProcessingStatus = ProcessingStatus.Delete
                        End If
                    Next

                End If

            End If

            ' process each conviction 
            For Each oConviction As BaseConvictionType In oConvictions

                If oConviction.ProcessingStatus = ProcessingStatus.Insert Then

                    ' insert conviction
                    AddPartyConviction(con, oBusiness, oConviction, PartyKey)

                ElseIf oConviction.ProcessingStatus = ProcessingStatus.Update Then

                    ' update conviction
                    UpdatePartyConvictions(con, oBusiness, oConviction, PartyKey)

                End If

            Next

            '20080525
            If oExistingConvictions IsNot Nothing Then
                For Each oConviction As BaseConvictionType In oExistingConvictions

                    If oConviction.ProcessingStatus = ProcessingStatus.Delete Then

                        ' delete conviction
                        DeletePartyConviction(con, oConviction.ConvictionKey, PartyKey)

                    End If

                Next
            End If

        Else
            ' delete all other party convictions
            DeleteAllOtherPartyConvictions(con, PartyKey)
        End If
    End Sub

    Private Sub DeleteAllOtherPartyAccidents(ByVal con As SiriusConnection, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_previous_accidents_dar")
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    Private Sub AddOtherPartyAccident( _
    ByVal con As SiriusConnection, _
    ByVal oAccident As BasePartyOTHERTypeAccident, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_previous_accidents_add")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@previous_accidents_id", SqlDbType.Int).Value = 0
            cmd.AddInParameter("@Date", SqlDbType.DateTime).Value = Cast.NullIfDefault(oAccident.Date, Date.MinValue)
            cmd.AddInParameter("@Description", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oAccident.Description, String.Empty)
            cmd.AddInParameter("@is_at_fault", SqlDbType.TinyInt).Value = BooleanDataConvert.ToByte(oAccident.IsAtFault)

            con.ExecuteNonQuery(cmd)

        End Using

    End Sub

    Private Sub UpdateOtherPartyAccident( _
    ByVal con As SiriusConnection, _
    ByVal oAccident As BasePartyOTHERTypeAccident, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_previous_accidents_upd")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@previous_accidents_id", SqlDbType.Int).Value = oAccident.AccidentKey
            cmd.AddInParameter("@Date", SqlDbType.DateTime).Value = Cast.NullIfDefault(oAccident.Date, Date.MinValue)
            cmd.AddInParameter("@Description", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oAccident.Description, String.Empty)
            cmd.AddInParameter("@is_at_fault", SqlDbType.TinyInt).Value = BooleanDataConvert.ToByte(oAccident.IsAtFault)

            con.ExecuteNonQuery(cmd)

        End Using

    End Sub

    Private Sub DeleteOtherPartyAccident( _
    ByVal con As SiriusConnection, _
    ByVal AccidentKey As Integer, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_previous_accidents_del")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@previous_accidents_id", SqlDbType.Int).Value = AccidentKey

            con.ExecuteNonQuery(cmd)

        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and deletes the respective records in the database
    '''</summary>
    '''<param name="PartyKey" type="Integer"></param>
    '''<remarks></remarks>
    Private Sub DeleteAllOtherPartyConvictions(ByVal con As SiriusConnection, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_conviction_dar")
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    ' Girija: differing with Tech Spec
    Private Sub AddOtherPartyConviction( _
    ByVal con As SiriusConnection, _
    ByVal oConviction As BasePartyOTHERTypeConviction, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_conviction_add")

            cmd.AddOutParameter("@party_conviction_id", SqlDbType.Int)
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@code", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.TypeCode, String.Empty)
            cmd.AddInParameter("@conviction_date", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(oConviction.Date.ToString("dd MMMM yyyy"), String.Empty) ' Vivek: 20080704
            cmd.AddInParameter("@description", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.Description, String.Empty)
            cmd.AddInParameter("@sentence_code", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.SentenceTypeCode, String.Empty)
            cmd.AddInParameter("@sentence_description", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.SentenceDescription, String.Empty)
            cmd.AddInParameter("@sentence_duration_qualifier", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.SentenceDurationQualifier, String.Empty)
            cmd.AddInParameter("@sentence_effective_date", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(IIf(oConviction.SentenceEffectiveDateSpecified, oConviction.SentenceEffectiveDate.ToString("dd MMMM yyyy"), String.Empty), String.Empty) ' Vivek: 20080704
            cmd.AddInParameter("@status_code", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.StatusCode, String.Empty)
            cmd.AddInParameter("@alcohol_measurement_method", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.AlcoholMeasurementMethod, String.Empty)

            If oConviction.FineAmountSpecified Then
                cmd.AddInParameter("@fine_amt", SqlDbType.Decimal, 19, 4).Value = CDec(oConviction.FineAmount)
            Else
                cmd.AddInParameter("@fine_amt", SqlDbType.Decimal, 19, 4).Value = 0D
            End If

            If oConviction.SentenceDurationSpecified Then
                cmd.AddInParameter("@sentence_duration", SqlDbType.Decimal, 14, 2).Value = CDec(oConviction.SentenceDuration)
            Else
                cmd.AddInParameter("@sentence_duration", SqlDbType.Decimal, 14, 2).Value = 0D
            End If

            If oConviction.AlcoholLevelSpecified Then
                cmd.AddInParameter("@alcohol_level", SqlDbType.Decimal, 14, 2).Value = CDec(oConviction.AlcoholLevel)
            Else
                cmd.AddInParameter("@alcohol_level", SqlDbType.Decimal, 14, 2).Value = 0D
            End If

            If oConviction.DrivingLicencePenaltyPointsSpecified Then
                cmd.AddInParameter("@driving_licence_penalty_pts", SqlDbType.Decimal, 14, 2).Value = CDec(oConviction.DrivingLicencePenaltyPoints)
            Else
                cmd.AddInParameter("@driving_licence_penalty_pts", SqlDbType.Decimal, 14, 2).Value = 0D
            End If

            con.ExecuteNonQuery(cmd)
            '20080525
            oConviction.ConvictionKey = Cast.ToInt32(cmd.Parameters("@party_conviction_id").Value, 0)

        End Using

    End Sub

    ' Girija: differing with Tech Spec
    Private Sub UpdateOtherPartyConviction( _
    ByVal con As SiriusConnection, _
    ByVal oConviction As BasePartyOTHERTypeConviction, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_conviction_upd")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@party_conviction_id", SqlDbType.Int).Value = oConviction.ConvictionKey
            cmd.AddInParameter("@code", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.TypeCode, String.Empty)
            cmd.AddInParameter("@conviction_date", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(oConviction.Date.ToString, String.Empty)
            cmd.AddInParameter("@description", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.Description, String.Empty)
            cmd.AddInParameter("@sentence_code", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.SentenceTypeCode, String.Empty)
            cmd.AddInParameter("@sentence_description", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.SentenceDescription, String.Empty)
            cmd.AddInParameter("@sentence_duration_qualifier", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.SentenceDurationQualifier, String.Empty)
            cmd.AddInParameter("@sentence_effective_date", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(oConviction.SentenceEffectiveDate.ToString, String.Empty)
            cmd.AddInParameter("@status_code", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.StatusCode, String.Empty)
            cmd.AddInParameter("@alcohol_measurement_method", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.AlcoholMeasurementMethod, String.Empty)

            If oConviction.FineAmountSpecified Then
                cmd.AddInParameter("@fine_amt", SqlDbType.Decimal, 19, 4).Value = CDec(oConviction.FineAmount)
            Else
                cmd.AddInParameter("@fine_amt", SqlDbType.Decimal, 19, 4).Value = 0D
            End If

            If oConviction.SentenceDurationSpecified Then
                cmd.AddInParameter("@sentence_duration", SqlDbType.Decimal, 14, 2).Value = CDec(oConviction.SentenceDuration)
            Else
                cmd.AddInParameter("@sentence_duration", SqlDbType.Decimal, 14, 2).Value = 0D
            End If

            If oConviction.AlcoholLevelSpecified Then
                cmd.AddInParameter("@alcohol_level", SqlDbType.Decimal, 14, 2).Value = CDec(oConviction.AlcoholLevel)
            Else
                cmd.AddInParameter("@alcohol_level", SqlDbType.Decimal, 14, 2).Value = 0D
            End If

            If oConviction.DrivingLicencePenaltyPointsSpecified Then
                cmd.AddInParameter("@driving_licence_penalty_pts", SqlDbType.Decimal, 14, 2).Value = CDec(oConviction.DrivingLicencePenaltyPoints)
            Else
                cmd.AddInParameter("@driving_licence_penalty_pts", SqlDbType.Decimal, 14, 2).Value = 0D
            End If

            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    Private Sub DeleteOtherPartyConviction( _
    ByVal con As SiriusConnection, _
    ByVal ConvictionKey As Integer, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_conviction_del")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@party_conviction_id", SqlDbType.Int).Value = ConvictionKey

            con.ExecuteNonQuery(cmd)

        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and updates the conviction
    '''</summary>
    '''<param name="oConviction" type="BaseConvictionType"></param>
    '''<remarks></remarks>
    Private Sub UpdatePartyConvictions( _
    ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oConviction As BaseConvictionType, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_conviction_upd")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@party_conviction_id", SqlDbType.Int).Value = oConviction.ConvictionKey
            cmd.AddInParameter("@code", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.TypeCode, String.Empty)
            cmd.AddInParameter("@conviction_date", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(oConviction.Date.ToString("dd MMMM yyyy"), String.Empty) ' Vivek: 20080704
            cmd.AddInParameter("@description", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.Description, String.Empty)
            cmd.AddInParameter("@sentence_code", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.SentenceTypeCode, String.Empty)
            cmd.AddInParameter("@sentence_description", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.SentenceDescription, String.Empty)
            cmd.AddInParameter("@sentence_duration_qualifier", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.SentenceDurationQualifier, String.Empty)
            cmd.AddInParameter("@sentence_effective_date", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(IIf(oConviction.SentenceEffectiveDateSpecified, oConviction.SentenceEffectiveDate.ToString("dd MMMM yyyy"), String.Empty), String.Empty) ' Vivek: 20080704
            cmd.AddInParameter("@status_code", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.StatusCode, String.Empty)
            cmd.AddInParameter("@alcohol_measurement_method", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.AlcoholMeasurementMethod, String.Empty)

            If oConviction.FineAmountSpecified Then
                cmd.AddInParameter("@fine_amt", SqlDbType.Decimal, 19, 4).Value = CDec(oConviction.FineAmount)
            Else
                cmd.AddInParameter("@fine_amt", SqlDbType.Decimal, 19, 4).Value = 0D
            End If

            If oConviction.SentenceDurationSpecified Then
                cmd.AddInParameter("@sentence_duration", SqlDbType.Decimal, 14, 2).Value = CDec(oConviction.SentenceDuration)
            Else
                cmd.AddInParameter("@sentence_duration", SqlDbType.Decimal, 14, 2).Value = 0D
            End If

            If oConviction.AlcoholLevelSpecified Then
                cmd.AddInParameter("@alcohol_level", SqlDbType.Decimal, 14, 2).Value = CDec(oConviction.AlcoholLevel)
            Else
                cmd.AddInParameter("@alcohol_level", SqlDbType.Decimal, 14, 2).Value = 0D
            End If

            If oConviction.DrivingLicensePenaltyPointsSpecified Then
                cmd.AddInParameter("@driving_licence_penalty_pts", SqlDbType.Decimal, 14, 2).Value = CDec(oConviction.DrivingLicensePenaltyPoints)
            Else
                cmd.AddInParameter("@driving_licence_penalty_pts", SqlDbType.Decimal, 14, 2).Value = 0D
            End If

            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and updates the party
    '''</summary>
    '''<param name="oAssociate" type="BaseAssociateType"></param>
    '''<remarks></remarks>

    Private Sub UpdatePartyAssociates( _
    ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oAssociate As BaseAssociateType, _
    ByVal PartyKey As Integer)

        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Party_Relationship_upd")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = oAssociate.ClientKey
            cmd.AddInParameter("@relation_cnt", SqlDbType.Int).Value = oAssociate.AssociateKey
            cmd.AddInParameter("@relationship_type_id", SqlDbType.SmallInt).Value = Cast.ToInt16(oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.RelationshipType, oAssociate.RelationshipCode, "Code", oSAMErrorCollection), 0)
            cmd.AddInParameter("@description", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(oAssociate.RelationshipDescription, String.Empty)
            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and updates the party life style
    '''</summary>
    '''<param name="oLifestyle" type="BasePartyPCTypeLifestyle"></param>
    '''<remarks></remarks>
    Private Sub UpdatePartyLifeStyle( _
    ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oLifestyle As BasePartyPCTypeLifestyle, _
    ByVal PartyKey As Integer)

        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_lifestyle_upd")

            cmd.AddInParameter("@party_lifestyle_id", SqlDbType.Int).Value = oLifestyle.LifestyleKey
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@name", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(oLifestyle.Name, String.Empty)

            cmd.AddInParameter("@category", SqlDbType.Int).Value = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.LifestyleCategory, oLifestyle.CategoryCode, "Code", oSAMErrorCollection)

            cmd.AddInParameter("@occupation_code", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oLifestyle.OccupationCode, String.Empty)
            cmd.AddInParameter("@secondary_occupation_code", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oLifestyle.SecOccupationCode, String.Empty)
            '20080525
            If oLifestyle.SmokerSpecified Then
                If oLifestyle.Smoker = True Then
                    cmd.AddInParameter("@is_smoker", SqlDbType.TinyInt).Value = 1
                Else
                    cmd.AddInParameter("@is_smoker", SqlDbType.TinyInt).Value = 0
                End If
            Else
                cmd.AddInParameter("@is_smoker", SqlDbType.TinyInt).Value = SqlInt16.Null
            End If

            '20080525
            cmd.AddInParameter("@date_of_birth", SqlDbType.DateTime).Value = IIf(oLifestyle.DateOfBirthSpecified And oLifestyle.DateOfBirth > Date.MinValue, oLifestyle.DateOfBirth, SqlDateTime.Null)
            'If oLifestyle.DateOfBirthSpecified Then
            '    cmd.AddInParameter("@date_of_birth", SqlDbType.DateTime).Value = oLifestyle.DateOfBirth
            'Else
            '    cmd.AddInParameter("@date_of_birth", SqlDbType.DateTime).Value = SqlDateTime.Null

            If oLifestyle.GenderCodeSpecified Then
                cmd.AddInParameter("@gender_code", SqlDbType.VarChar, 70).Value = _
                    IIf(oLifestyle.GenderCode = GenderCodeType.Male, "Male", "Female")
            Else
                cmd.AddInParameter("@gender_code", SqlDbType.VarChar, 70).Value = SqlString.Null

            End If

            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and updates the party loyalty scheme
    '''</summary>
    '''<param name="oLoyaltyScheme" type="BaseClientSharedDataTypeLoyaltyScheme"></param>
    '''<remarks></remarks>

    Private Sub UpdatePartyLoyaltyScheme( _
    ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oLoyaltyScheme As BaseClientSharedDataTypeLoyaltyScheme, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Update_PartyLoyaltyScheme")

            Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection

            cmd.AddInParameter("@party_loyalty_scheme_id", SqlDbType.Int).Value = oLoyaltyScheme.LoyaltySchemeKey
            'cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            'cmd.AddInParameter("@loyalty_scheme_id", SqlDbType.Int).Value = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.LoyaltyScheme, oLoyaltyScheme.LoyaltySchemeCode, "Code", oSAMErrorCollection)
            cmd.AddInParameter("@membership_number", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(oLoyaltyScheme.MembershipNumber, String.Empty)
            cmd.AddInParameter("@other_reference", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(oLoyaltyScheme.OtherReference, String.Empty)
            '20080525
            cmd.AddInParameter("@start_date", SqlDbType.DateTime).Value = IIf(oLoyaltyScheme.StartDate > Date.MinValue, oLoyaltyScheme.StartDate, SqlDateTime.Null)
            'cmd.AddInParameter("@start_date", SqlDbType.DateTime).Value = oLoyaltyScheme.StartDate

            cmd.AddInParameter("@main_membership_number", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(oLoyaltyScheme.MainMember, String.Empty)
            If oLoyaltyScheme.ActiveSpecified Then
                cmd.AddInParameter("@is_active", SqlDbType.Bit).Value = oLoyaltyScheme.Active
            Else
                cmd.AddInParameter("@is_active", SqlDbType.Bit).Value = False
            End If
            '20080525
            cmd.AddInParameter("@end_date", SqlDbType.DateTime).Value = IIf(oLoyaltyScheme.EndDateSpecified And oLoyaltyScheme.EndDate > Date.MinValue, oLoyaltyScheme.EndDate, SqlDateTime.Null)
            'If oLoyaltyScheme.EndDateSpecified Then
            '    cmd.AddInParameter("@end_date", SqlDbType.DateTime).Value = oLoyaltyScheme.EndDate
            'Else
            '    cmd.AddInParameter("@end_date", SqlDbType.DateTime).Value = SqlDateTime.Null

            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and updates the party prospect policies
    '''</summary>
    '''<param name="oProspectPolicy" type="BaseClientSharedDataTypeProspectPolicies"></param>
    '''<remarks></remarks>
    Private Sub UpdatePartyProspectPolicies( _
    ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oProspectPolicy As BaseClientSharedDataTypeProspectPolicies, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Update_PartyProspectPolicies")

            Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection

            cmd.AddInParameter("@prospect_policy_id", SqlDbType.Int).Value = oProspectPolicy.ProspectPolicyKey
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@risk_group_id", SqlDbType.Int).Value = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.RiskGroup, oProspectPolicy.ProspectTypeCode, "Code", oSAMErrorCollection)

            '20080525
            cmd.AddInParameter("@renewal_date", SqlDbType.DateTime).Value = IIf(oProspectPolicy.RenewalDateSpecified And oProspectPolicy.RenewalDate > Date.MinValue, oProspectPolicy.RenewalDate, SqlDateTime.Null)
            'If oProspectPolicy.RenewalDateSpecified Then
            '    cmd.AddInParameter("@renewal_date", SqlDbType.DateTime).Value = oProspectPolicy.RenewalDate
            'Else
            '    cmd.AddInParameter("@renewal_date", SqlDbType.DateTime).Value = SqlDateTime.Null

            If oProspectPolicy.TargetPremiumSpecified Then
                cmd.AddInParameter("@target_premium", SqlDbType.Decimal, 19, 4).Value = oProspectPolicy.TargetPremium
            Else
                cmd.AddInParameter("@target_premium", SqlDbType.Decimal, 19, 4).Value = SqlDecimal.Null
            End If
            If oProspectPolicy.TimesQuotedSpecified Then
                ' Vivek: as per Tech Spec we have defined this field as Deciaml
                ' but in DB its INT. So using the conversion function here.
                cmd.AddInParameter("@no_of_times_quoted", SqlDbType.Int).Value = Convert.ToInt32(oProspectPolicy.TimesQuoted)
            Else
                cmd.AddInParameter("@no_of_times_quoted", SqlDbType.Int).Value = SqlInt32.Null
            End If

            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and adds new conviction
    '''</summary>
    '''<param name="oConviction" type="BaseConvictionType"></param>
    '''<remarks></remarks>
    Private Sub AddPartyConviction( _
    ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oConviction As BaseConvictionType, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_conviction_add")

            cmd.AddOutParameter("@party_conviction_id", SqlDbType.Int)
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@code", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.TypeCode, String.Empty)
            cmd.AddInParameter("@conviction_date", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(oConviction.Date.ToLongDateString(), String.Empty) ' Vivek: 20080704
            cmd.AddInParameter("@description", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.Description, String.Empty)
            cmd.AddInParameter("@sentence_code", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.SentenceTypeCode, String.Empty)
            cmd.AddInParameter("@sentence_description", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.SentenceDescription, String.Empty)
            cmd.AddInParameter("@sentence_duration_qualifier", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.SentenceDurationQualifier, String.Empty)
            cmd.AddInParameter("@sentence_effective_date", SqlDbType.VarChar, 40).Value = Cast.NullIfDefault(IIf(oConviction.SentenceEffectiveDateSpecified, oConviction.SentenceEffectiveDate.ToLongDateString(), String.Empty), String.Empty) ' Vivek: 20080704
            cmd.AddInParameter("@status_code", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.StatusCode, String.Empty)
            cmd.AddInParameter("@alcohol_measurement_method", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oConviction.AlcoholMeasurementMethod, String.Empty)

            If oConviction.FineAmountSpecified Then
                cmd.AddInParameter("@fine_amt", SqlDbType.Decimal, 19, 4).Value = CDec(oConviction.FineAmount)
            Else
                cmd.AddInParameter("@fine_amt", SqlDbType.Decimal, 19, 4).Value = 0D
            End If

            If oConviction.SentenceDurationSpecified Then
                cmd.AddInParameter("@sentence_duration", SqlDbType.Decimal, 14, 2).Value = CDec(oConviction.SentenceDuration)
            Else
                cmd.AddInParameter("@sentence_duration", SqlDbType.Decimal, 14, 2).Value = 0D
            End If

            If oConviction.AlcoholLevelSpecified Then
                cmd.AddInParameter("@alcohol_level", SqlDbType.Decimal, 14, 2).Value = CDec(oConviction.AlcoholLevel)
            Else
                cmd.AddInParameter("@alcohol_level", SqlDbType.Decimal, 14, 2).Value = 0D
            End If

            If oConviction.DrivingLicensePenaltyPointsSpecified Then
                cmd.AddInParameter("@driving_licence_penalty_pts", SqlDbType.Decimal, 14, 2).Value = CDec(oConviction.DrivingLicensePenaltyPoints)
            Else
                cmd.AddInParameter("@driving_licence_penalty_pts", SqlDbType.Decimal, 14, 2).Value = 0D
            End If

            con.ExecuteNonQuery(cmd)
            '20080525
            oConviction.ConvictionKey = Cast.ToInt32(cmd.Parameters("@party_conviction_id").Value, 0)

        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and adds  the party associates
    '''</summary>
    '''<param name="oAssociate" type="BaseAssociateType"></param>
    '''<remarks></remarks>

    ' Vivek: TODO: rename to make it consistant
    Private Sub AddPartyAssociates( _
    ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oAssociate As BaseAssociateType, _
    ByVal PartyKey As Integer)

        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Add_PartyAssociates")

            '20080605
            cmd.AddInParameter("@relation_cnt", SqlDbType.Int).Value = oAssociate.AssociateKey
            '20080525
            ' instead of using the party key sent by user, use the one we have got as method param
            ' otherwise it causes problem in case of AddParty
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey 'oAssociate.ClientKey
            cmd.AddInParameter("@relationship_type_id", SqlDbType.TinyInt).Value = Cast.ToInt16(oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.RelationshipType, oAssociate.RelationshipCode, "Code", oSAMErrorCollection), 0)
            cmd.AddInParameter("@description", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(oAssociate.RelationshipDescription, String.Empty)

            con.ExecuteNonQuery(cmd)

            '20080525
            oAssociate.AssociateKey = Cast.ToInt32(cmd.Parameters("@relation_cnt").Value, 0)
        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and deletes  the party associates
    '''</summary>
    '''<param name="iPartyKey,iAssociateKey" type="Integer,integer"></param>
    '''<remarks></remarks>

    Private Sub DeletePartyAssociates( _
    ByVal con As SiriusConnection, _
    ByVal iAssociateKey As Integer, _
    ByVal iPartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Party_Relationship_del")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = iPartyKey
            cmd.AddInParameter("@relation_cnt", SqlDbType.Int).Value = iAssociateKey

            con.ExecuteNonQuery(cmd)

        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and deletes  the party convictions
    '''</summary>
    '''<param name="iPartyKey,iConvictionKey" type="Integer,integer"></param>
    '''<remarks></remarks>

    ' Vivek: differing from Tech Spec
    Private Sub DeletePartyConviction( _
    ByVal con As SiriusConnection, _
    ByVal iConvictionKey As Integer, _
    ByVal iPartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_conviction_del")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = iPartyKey
            cmd.AddInParameter("@party_conviction_id", SqlDbType.Int).Value = iConvictionKey

            con.ExecuteNonQuery(cmd)

        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and deletes  the party life style
    '''</summary>
    '''<param name="iPartyKey,iLifestyleKey" type="Integer,integer"></param>
    '''<remarks></remarks>

    Private Sub DeletePartyLifeStyle( _
    ByVal con As SiriusConnection, _
    ByVal iLifestyleKey As Integer, _
    ByVal iPartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_lifestyle_del")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = iPartyKey
            cmd.AddInParameter("@party_lifestyle_id", SqlDbType.Int).Value = iLifestyleKey

            con.ExecuteNonQuery(cmd)

        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and deletes  the party loyalty scheme
    '''</summary>
    '''<param name="iPartyKey,iLoyaltySchemeKey" type="Integer,integer"></param>
    '''<remarks></remarks>

    Private Sub DeletePartyLoyaltyScheme( _
    ByVal con As SiriusConnection, _
    ByVal iLoyaltySchemeKey As Integer, _
    ByVal iPartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Delete_PartyLoyaltyScheme")

            cmd.AddInParameter("@party_loyalty_scheme_id", SqlDbType.Int).Value = iLoyaltySchemeKey

            con.ExecuteNonQuery(cmd)

        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and deletes  the Party Prospect Policies
    '''</summary>
    '''<param name="PartyKey,ProspectPolicyKey" type="Integer,integer"></param>
    '''<remarks></remarks>

    Private Sub DeletePartyProspectPolicies( _
    ByVal con As SiriusConnection, _
    ByVal ProspectPolicyKey As Integer, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Delete_ProspectPolicies")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@prospect_policy_id", SqlDbType.Int).Value = ProspectPolicyKey

            con.ExecuteNonQuery(cmd)

        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and adds the party life style
    '''</summary>
    '''<param name="oLifestyle" type="BasePartyPCTypeLifestyle"></param>
    '''<remarks></remarks>

    Private Sub AddPartyLifeStyle( _
    ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oLifestyle As BasePartyPCTypeLifestyle, _
    ByVal PartyKey As Integer)

        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_lifestyle_add")

            cmd.AddOutParameter("@party_lifestyle_id", SqlDbType.Int)
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@name", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(oLifestyle.Name, String.Empty)
            cmd.AddInParameter("@category", SqlDbType.Int).Value = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.LifestyleCategory, oLifestyle.CategoryCode, "Code", oSAMErrorCollection)
            cmd.AddInParameter("@occupation_code", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oLifestyle.OccupationCode, String.Empty)
            cmd.AddInParameter("@secondary_occupation_code", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oLifestyle.SecOccupationCode, String.Empty)
            '20080525
            If oLifestyle.SmokerSpecified Then
                If oLifestyle.Smoker = True Then
                    cmd.AddInParameter("@is_smoker", SqlDbType.TinyInt).Value = 1
                Else
                    cmd.AddInParameter("@is_smoker", SqlDbType.TinyInt).Value = 0
                End If
            Else
                cmd.AddInParameter("@is_smoker", SqlDbType.TinyInt).Value = SqlInt16.Null
            End If

            '20080525
            cmd.AddInParameter("@date_of_birth", SqlDbType.DateTime).Value = IIf(oLifestyle.DateOfBirthSpecified And oLifestyle.DateOfBirth > Date.MinValue, oLifestyle.DateOfBirth, SqlDateTime.Null)
            'If oLifestyle.DateOfBirthSpecified Then
            '    cmd.AddInParameter("@date_of_birth", SqlDbType.DateTime).Value = oLifestyle.DateOfBirth
            'Else
            '    cmd.AddInParameter("@date_of_birth", SqlDbType.DateTime).Value = SqlDateTime.Null

            If oLifestyle.GenderCodeSpecified Then
                cmd.AddInParameter("@gender_code", SqlDbType.VarChar, 70).Value = _
                    IIf(oLifestyle.GenderCode = GenderCodeType.Male, "Male", "Female")
            Else
                cmd.AddInParameter("@gender_code", SqlDbType.VarChar, 70).Value = SqlString.Null
            End If

            con.ExecuteNonQuery(cmd)
            '20080525
            oLifestyle.LifestyleKey = Cast.ToInt32(cmd.Parameters("@party_lifestyle_id").Value, 0)

        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and adds  the party loyalty scheme
    '''</summary>
    '''<param name="oLoyaltyScheme" type="BaseClientSharedDataTypeLoyaltyScheme"></param>
    '''<remarks></remarks>

    Private Sub AddPartyLoyaltyScheme( _
    ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oLoyaltyScheme As BaseClientSharedDataTypeLoyaltyScheme, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Add_PartyLoyaltyScheme")

            Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection

            cmd.AddOutParameter("@party_loyalty_scheme_id", SqlDbType.Int)
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@loyalty_scheme_id", SqlDbType.Int).Value = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.LoyaltyScheme, oLoyaltyScheme.LoyaltySchemeCode, "Code", oSAMErrorCollection)
            cmd.AddInParameter("@membership_number", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(oLoyaltyScheme.MembershipNumber, String.Empty)
            cmd.AddInParameter("@other_reference", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(oLoyaltyScheme.OtherReference, String.Empty)
            '20080525
            cmd.AddInParameter("@start_date", SqlDbType.DateTime).Value = IIf(oLoyaltyScheme.StartDate > Date.MinValue, oLoyaltyScheme.StartDate, SqlDateTime.Null)
            'cmd.AddInParameter("@start_date", SqlDbType.DateTime).Value = oLoyaltyScheme.StartDate

            cmd.AddInParameter("@main_membership_number", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(oLoyaltyScheme.MainMember, String.Empty)
            If oLoyaltyScheme.ActiveSpecified Then
                cmd.AddInParameter("@is_active", SqlDbType.Bit).Value = oLoyaltyScheme.Active
            Else
                cmd.AddInParameter("@is_active", SqlDbType.Bit).Value = False
            End If
            '20080525
            cmd.AddInParameter("@end_date", SqlDbType.DateTime).Value = IIf(oLoyaltyScheme.EndDateSpecified And oLoyaltyScheme.EndDate > Date.MinValue, oLoyaltyScheme.EndDate, SqlDateTime.Null)
            'If oLoyaltyScheme.EndDateSpecified Then
            '    cmd.AddInParameter("@end_date", SqlDbType.DateTime).Value = oLoyaltyScheme.EndDate
            'Else
            '    cmd.AddInParameter("@end_date", SqlDbType.DateTime).Value = SqlDateTime.Null

            con.ExecuteNonQuery(cmd)
            '20080525
            oLoyaltyScheme.LoyaltySchemeKey = Cast.ToInt32(cmd.Parameters("@party_loyalty_scheme_id").Value, 0)

        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and deletes  the party associates
    '''</summary>
    '''<param name="iPartyKey,iAssociateKey" type="Integer,integer"></param>
    '''<remarks></remarks>

    Private Sub AddPartyProspectPolicies( _
    ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oProspectPolicies As BaseClientSharedDataTypeProspectPolicies, _
    ByVal PartyKey As Integer)
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Add_PartyProspectPolicies")

            Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection

            cmd.AddOutParameter("@prospect_policy_id", SqlDbType.Int)
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@risk_group_id", SqlDbType.Int).Value = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.RiskGroup, oProspectPolicies.ProspectTypeCode, "Code", oSAMErrorCollection)
            '20080525
            cmd.AddInParameter("@renewal_date", SqlDbType.DateTime).Value = IIf(oProspectPolicies.RenewalDateSpecified And oProspectPolicies.RenewalDate > Date.MinValue, oProspectPolicies.RenewalDate, SqlDateTime.Null)
            'If oProspectPolicies.RenewalDateSpecified Then
            '    cmd.AddInParameter("@renewal_date", SqlDbType.DateTime).Value = oProspectPolicies.RenewalDate
            'Else
            '    cmd.AddInParameter("@renewal_date", SqlDbType.DateTime).Value = SqlDateTime.Null

            If oProspectPolicies.TargetPremiumSpecified Then
                cmd.AddInParameter("@target_premium", SqlDbType.Decimal, 19, 4).Value = oProspectPolicies.TargetPremium
            Else
                cmd.AddInParameter("@target_premium", SqlDbType.Decimal, 19, 4).Value = SqlDecimal.Null
            End If
            If oProspectPolicies.TimesQuotedSpecified Then
                ' Vivek: as per Tech Spec we have defined this field as Deciaml
                ' but in DB its INT. So using the conversion function here.
                cmd.AddInParameter("@no_of_times_quoted", SqlDbType.Int).Value = Convert.ToInt32(oProspectPolicies.TimesQuoted)
            Else
                cmd.AddInParameter("@no_of_times_quoted", SqlDbType.Int).Value = SqlInt32.Null
            End If
            con.ExecuteNonQuery(cmd)

            '20080525
            oProspectPolicies.ProspectPolicyKey = Cast.ToInt32(cmd.Parameters("@prospect_policy_id").Value, 0)
        End Using

    End Sub

    Private Sub DeletePartyAddressUsage( _
    ByVal con As SiriusConnection, _
    ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Delete_Party_Address_Usage")
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    Private Sub SaveAddressContactUsage( _
    ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oAddressWithContacts As BaseAddressWithContactsType, _
    ByVal BranchCode As String, _
    ByVal SourceId As Integer, _
    ByVal PartyKey As Integer)

        Dim AddressId As Integer
        Dim ContactId As Integer

        ' insert an entry into the address table
        AddressId = SaveAddressDetails(con, oBusiness, oAddressWithContacts, SourceId, PartyKey)

        If oAddressWithContacts.Contacts IsNot Nothing Then
            ' now save all the contacts against the address
            For Each oContact As BaseContactType In oAddressWithContacts.Contacts

                ' save contact details
                ContactId = SaveContactDetails(con, oBusiness, oContact, SourceId)

                SaveContactAddressUsage(con, AddressId, ContactId)

            Next

        End If

    End Sub

    Private Sub SaveContactAddressUsage(ByVal con As SiriusConnection, _
    ByVal AddressId As Integer, _
    ByVal ContactId As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Contact_Address_Usage_add")

            cmd.AddInParameter("@contact_cnt", SqlDbType.Int).Value = ContactId
            cmd.AddInParameter("@address_cnt", SqlDbType.Int).Value = AddressId
            cmd.AddInParameter("@description", SqlDbType.VarChar, 255).Value = Nothing

            con.ExecuteNonQuery(cmd)

        End Using

    End Sub

    Private Function SaveAddressDetails(ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oAddressWithContacts As BaseAddressWithContactsType, _
    ByVal SourceId As Integer, _
    ByVal PartyKey As Integer) As Integer

        Dim AddressId As Integer

        ' reusing this stored procedure as this table is still attached to the unique number table
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Address_add")

            cmd.AddOutParameter("@address_cnt", SqlDbType.Int)

            cmd.AddInParameter("@source_id", SqlDbType.Int).Value = Cast.DefaultIfNull(SourceId, 0)
            cmd.AddInParameter("@address_id", SqlDbType.Int).Value = 0 ' autogenerated by the procedure from unique number table
            cmd.AddInParameter("@address1", SqlDbType.VarChar, 60).Value = Cast.DefaultIfNull(oAddressWithContacts.AddressLine1, Nothing)
            cmd.AddInParameter("@address2", SqlDbType.VarChar, 60).Value = Cast.DefaultIfNull(oAddressWithContacts.AddressLine2, Nothing)
            cmd.AddInParameter("@address3", SqlDbType.VarChar, 60).Value = Cast.DefaultIfNull(oAddressWithContacts.AddressLine3, Nothing)
            cmd.AddInParameter("@address4", SqlDbType.VarChar, 60).Value = Cast.DefaultIfNull(oAddressWithContacts.AddressLine4, Nothing)
            cmd.AddInParameter("@postal_code", SqlDbType.VarChar, 20).Value = Cast.DefaultIfNull(oAddressWithContacts.PostCode, Nothing)
            cmd.AddInParameter("@country_id", SqlDbType.Int).Value = Cast.DefaultIfNull(oAddressWithContacts.CountryId, Nothing)
            cmd.AddInParameter("@created_by_id", SqlDbType.Int).Value = _SiriusUser.UserID
            cmd.AddInParameter("@date_created", SqlDbType.DateTime).Value = Date.Now
            cmd.AddInParameter("@modified_by_id", SqlDbType.Int).Value = _SiriusUser.UserID
            cmd.AddInParameter("@last_modified", SqlDbType.DateTime).Value = Date.Now
            cmd.AddInParameter("@ExternalId", SqlDbType.Int).Value = SqlInt32.Null

            con.ExecuteNonQuery(cmd)

            AddressId = Cast.ToInt32(cmd.Parameters("@address_cnt").Value, 0)

        End Using

        ' get address usage type id
        Dim AddressUsageTypeId As Integer
        AddressUsageTypeId = oBusiness.GetListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.AddressUsageType, SAMFunc.GetABIAddressCode(oAddressWithContacts.AddressTypeCode))

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Party_Address_Usage_add")
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@address_cnt", SqlDbType.Int).Value = AddressId
            cmd.AddInParameter("@description", SqlDbType.VarChar, 255).Value = Nothing
            cmd.AddInParameter("@address_usage_type_id", SqlDbType.Int).Value = AddressUsageTypeId
            con.ExecuteNonQuery(cmd)
        End Using

        Return AddressId

    End Function

    Private Function SaveContactDetails(ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oContact As BaseContactType, _
    ByVal SourceId As Integer) As Integer

        ' reusing this stored procedure as this table is still attached to the unique number table
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Contact_add")

            cmd.AddOutParameter("@contact_cnt", SqlDbType.Int)

            ' get contact type description default
            'Dim sContactTypeDescription As String = GetExtraContactDetails(oBusiness, oContact)

            cmd.AddInParameter("@contact_type_id", SqlDbType.Int).Value = oContact.ContactTypeId
            cmd.AddInParameter("@source_id", SqlDbType.Int).Value = SourceId
            cmd.AddInParameter("@contact_id", SqlDbType.Int).Value = 0 ' autogenerated by the procedure from unique number table
            cmd.AddInParameter("@country_id", SqlDbType.Int).Value = SourceId
            'cmd.AddInParameter("@description", SqlDbType.VarChar, 255).Value = sContactTypeDescription ' Vivek: 20080704 - using the newly added field, instead of hard coded values
            cmd.AddInParameter("@description", SqlDbType.VarChar, 255).Value = oContact.Description ' Vivek: 20080704 - added the missing item to fix bug in existing system (Amend Client)
            cmd.AddInParameter("@area_code", SqlDbType.VarChar, 10).Value = oContact.AreaCode
            cmd.AddInParameter("@number", SqlDbType.VarChar, 255).Value = oContact.ContactDetail.Item
            cmd.AddInParameter("@extension", SqlDbType.VarChar, 6).Value = Nothing
            cmd.AddInParameter("@created_by_id", SqlDbType.Int).Value = _SiriusUser.UserID
            cmd.AddInParameter("@date_created", SqlDbType.DateTime).Value = Date.Now
            cmd.AddInParameter("@modified_by_id", SqlDbType.Int).Value = _SiriusUser.UserID
            cmd.AddInParameter("@last_modified", SqlDbType.DateTime).Value = Date.Now

            con.ExecuteNonQuery(cmd)

            ' return the contact type id
            Return Cast.ToInt32(cmd.Parameters("@contact_cnt").Value, 0)

        End Using

    End Function

    Private Sub ProcessGetPartyData(ByVal oBusiness As CoreBusiness, _
    ByVal oRequest As BaseGetPartyRequestType, _
    ByVal oResponse As BaseGetPartyResponseType, _
    ByVal sPartyType As String)

        '**********************
        ' STRUCTURE VALIDATION 
        '**********************

        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection

        oRequest.Validate(CType(oSAMErrorCollection, Object))

        '**********************
        ' DATA VALIDATION 
        '**********************

        ' validate branch code
        oRequest.SourceId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, oRequest.BranchCode, "BranchCode", oSAMErrorCollection)

        '**********************
        ' GET PARTY DETAILS
        '**********************
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Dim oParty As New BaseImplementationTypes.BasePartyType

            ' get party 
            GetPartyData(con, oBusiness, oRequest, oResponse, sPartyType)

        End Using

    End Sub

    Private Sub GetPartyData(ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oRequest As BaseGetPartyRequestType, _
    ByVal oResponse As BaseGetPartyResponseType, _
    ByVal sPartyType As String)

        Dim dt As DataTable = Nothing

        ' retrieve the party data from the database
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Party_Details")

            cmd.AddInParameter("party_cnt", SqlDbType.Int).Value = oRequest.PartyKey

            dt = con.ExecuteDataTable(cmd)

        End Using

        ' if there is a matching row
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            ' for each party - should be only one
            For Each dr As DataRow In dt.Rows

                ' determine which type of party it is 
                If sPartyType.StartsWith("OT") OrElse sPartyType.StartsWith("AG") OrElse sPartyType.StartsWith("IN") Then

                    Dim oParty As New BaseImplementationTypes.BasePartyOTHERType

                    ' get party other details
                    GetPartyOtherData(con, oRequest, oRequest.PartyKey, oParty, dr)

                    ' assign the party back to the response
                    oResponse.Item = oParty

                ElseIf sPartyType = "PC" Then

                    ' TODO: MEVANS : will be moved later
                    Dim oParty As New BaseImplementationTypes.BasePartyPCType

                    ' assign the party back to the response
                    oResponse.Item = oParty

                ElseIf sPartyType = "CC" Then

                    ' TODO: MEVANS : will be moved later
                    Dim oParty As New BaseImplementationTypes.BasePartyCCType

                    ' assign the party back to the response
                    oResponse.Item = oParty

                End If

            Next

        End If

        Dim bIsLocked As Boolean
        oBusiness.GetSAMTimestamp( _
                            oRequest.BranchCode, _
                            CoreBusiness.LockName.PartyCnt, _
                            oRequest.PartyKey, _
                            oResponse.PartyTimestamp, _
                            bIsLocked)

    End Sub

    Private Sub GetPartyBaseData(ByVal con As SiriusConnection, _
    ByVal lPartyCnt As Integer, _
    ByVal oParty As BasePartyType, _
    ByVal dr As DataRow)

        ' get the base parties details
        oParty.BranchCode = Cast.ToString(dr.Item("source_code"), String.Empty).Trim
        oParty.TaxNumber = Cast.ToString(dr.Item("tax_number"), String.Empty).Trim
        oParty.DomiciledForTax = CBool(Cast.DefaultIfNull(dr.Item("domiciled_for_tax"), 0))
        oParty.TaxExempt = CBool(Cast.DefaultIfNull(dr.Item("tax_exempt"), 0))
        oParty.TaxPercentage = CDec(Cast.ToDouble(dr.Item("tax_percentage"), 0))
        oParty.TPUserCode = Cast.ToString(dr.Item("tp_user_code"), String.Empty).Trim
        oParty.TPIntroducer = Cast.ToString(dr.Item("tp_introducer_code"), String.Empty).Trim
        oParty.AccountExecutive = Cast.ToString(dr.Item("account_executive"), String.Empty).Trim
        oParty.Currency = Cast.ToString(dr.Item("currency_code"), String.Empty).Trim

        oParty.DomiciledForTaxSpecified = True
        oParty.TaxExemptSpecified = True
        oParty.TaxPercentageSpecified = True

        ' get addresses
        oParty.Addresses = GetPartyAddresses(con, lPartyCnt)

        ' get address related contacts
        oParty.Contacts = GetPartyContacts(con, oParty, lPartyCnt)

    End Sub

    Private Function GetPartyAddresses(ByVal con As SiriusConnection, _
    ByVal lPartyCnt As Integer) As BaseAddressWithContactsType()

        Dim dt As DataTable = Nothing

        ' get party data
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Party_Addresses")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = lPartyCnt

            dt = con.ExecuteDataTable(cmd)

        End Using

        Dim dtContacts As DataTable = Nothing

        ' get party data
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Party_Address_Contacts")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = lPartyCnt

            dtContacts = con.ExecuteDataTable(cmd)

        End Using

        ' if the party has had previous accidents
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim oAddresses As BaseAddressWithContactsType() = Nothing

            ' resize array
            Array.Resize(oAddresses, dt.Rows.Count)

            Dim iAddress As Integer = 0

            For Each dr As DataRow In dt.Rows

                Dim oAddress As New BaseImplementationTypes.BaseAddressWithContactsType
                Dim oAddressType As AddressTypeType
                Dim iAddressCnt As Integer = Cast.ToInt32(dr.Item("Address_Cnt"), 0)

                oAddress.AddressLine1 = Cast.ToString(dr.Item("address1"), String.Empty).Trim
                oAddress.AddressLine2 = Cast.ToString(dr.Item("address2"), String.Empty).Trim
                oAddress.AddressLine3 = Cast.ToString(dr.Item("address3"), String.Empty).Trim
                oAddress.AddressLine4 = Cast.ToString(dr.Item("address4"), String.Empty).Trim
                oAddress.AddressLine6 = Cast.ToString(dr.Item("address6"), String.Empty).Trim
                oAddress.AddressLine7 = Cast.ToString(dr.Item("address7"), String.Empty).Trim
                oAddress.AddressLine8 = Cast.ToString(dr.Item("address8"), String.Empty).Trim
                oAddress.AddressLine9 = Cast.ToString(dr.Item("address9"), String.Empty).Trim
                oAddress.AddressLine10 = Cast.ToString(dr.Item("address10"), String.Empty).Trim
                oAddress.AddressLine5 = Cast.ToString(dr.Item("address5"), String.Empty).Trim


                oAddress.PostCode = Cast.ToString(dr.Item("postal_code"), String.Empty).Trim
                oAddressType = SAMFunc.GetAddressTypeCode(Cast.ToString(dr.Item("address_usage_type_code"), String.Empty).Trim)
                oAddress.AddressTypeCode = oAddressType
                oAddress.CountryCode = Cast.ToString(dr.Item("country_code"), String.Empty).Trim

                If dtContacts IsNot Nothing AndAlso dtContacts.Rows.Count > 0 Then

                    Dim iContact As Integer = 0

                    For Each drContact As DataRow In dtContacts.Rows

                        Dim iContactAddressCnt As Integer = Cast.ToInt32(drContact.Item("Address_Cnt"), 0)

                        If iContactAddressCnt = iAddressCnt Then

                            Array.Resize(oAddress.Contacts, iContact + 1)

                            Dim oContact As New BaseImplementationTypes.BaseContactType
                            Dim oContactDetail As New BaseImplementationTypes.BaseContactDetailType
                            Dim sContactType As String = Cast.ToString(drContact.Item("Contact_Type_Code"), String.Empty).Trim

                            ' default the item element name to number
                            oContactDetail.ItemElementName = ItemChoiceType.Number

                            Select Case sContactType
                                Case ContactType.Email
                                    oContact.ContactTypeCode = ContactTypeType.EMAIL
                                    ' override the default item element to email 
                                    oContactDetail.ItemElementName = ItemChoiceType.EmailAddress
                                Case ContactType.Homephone
                                    oContact.ContactTypeCode = ContactTypeType.HOMEPHONE
                                Case ContactType.Mobile
                                    oContact.ContactTypeCode = ContactTypeType.MOBILE
                                Case ContactType.Fax
                                    oContact.ContactTypeCode = ContactTypeType.FAX
                                Case ContactType.Web
                                    oContact.ContactTypeCode = ContactTypeType.WEB
                                Case ContactType.Main
                                    oContact.ContactTypeCode = ContactTypeType.MAIN
                                Case ContactType.MainEmailContact
                                    oContact.ContactTypeCode = ContactTypeType.MAINEMAILCONTACT
                                Case ContactType.Telephone
                                    oContact.ContactTypeCode = ContactTypeType.TELEPHONE
                                Case ContactType.Other
                                    oContact.ContactTypeCode = ContactTypeType.OTHER
                                Case ContactType.Letter
                                    oContact.ContactTypeCode = ContactTypeType.LETTER
                            End Select

                            oContactDetail.Item = Cast.ToString(drContact.Item("number"), String.Empty).Trim
                            oContact.AreaCode = Cast.ToString(drContact.Item("area_code"), String.Empty).Trim
                            ' TODO: NOT AVAILABLE in SP
                            'oContact.ContactTypeId = Cast.ToInt32(drContact.Item(Contact_Type_id), 0) ' Vivek: 20080704
                            'oContact.Description = Cast.ToString(drContact.Item(description)) ' Vivek: 20080704

                            oContact.ContactDetail = oContactDetail

                            oAddress.Contacts(iContact) = oContact

                            iContact += 1

                        End If
                    Next

                End If

                oAddresses(iAddress) = oAddress

                iAddress += 1

            Next

            Return oAddresses

        End If

        Return Nothing

    End Function

    Private Function GetPartyContacts(ByVal con As SiriusConnection, _
    ByVal oParty As BasePartyType, _
    ByVal lPartyCnt As Integer _
    ) As BaseContactType()

        Dim dt As DataTable = Nothing

        ' get party data
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Party_Contacts")

            cmd.AddInParameter("party_cnt", SqlDbType.Int).Value = lPartyCnt

            dt = con.ExecuteDataTable(cmd)

        End Using

        ' if the party has had previous accidents
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim oContacts As BaseContactType() = Nothing

            ' resize array
            Array.Resize(oContacts, dt.Rows.Count)

            Dim iContact As Integer = 0

            ' for each previous accident
            For Each dr As DataRow In dt.Rows

                Dim oContact As New BaseImplementationTypes.BaseContactType
                Dim oContactDetail As New BaseImplementationTypes.BaseContactDetailType
                Dim sContactType As String = Cast.ToString(dr.Item("Contact_Type_Code"), String.Empty).Trim

                ' default the item element name to number
                oContactDetail.ItemElementName = ItemChoiceType.Number

                Select Case sContactType
                    Case ContactType.Email
                        oContact.ContactTypeCode = ContactTypeType.EMAIL
                        ' override the default item element to email 
                        oContactDetail.ItemElementName = ItemChoiceType.EmailAddress
                    Case ContactType.MainEmailContact
                        oContact.ContactTypeCode = ContactTypeType.MAINEMAILCONTACT
                        ' override the default item element to email 
                        oContactDetail.ItemElementName = ItemChoiceType.EmailAddress
                    Case ContactType.Homephone
                        oContact.ContactTypeCode = ContactTypeType.HOMEPHONE
                    Case ContactType.Mobile
                        oContact.ContactTypeCode = ContactTypeType.MOBILE
                    Case ContactType.Fax
                        oContact.ContactTypeCode = ContactTypeType.FAX
                    Case ContactType.Web
                        oContact.ContactTypeCode = ContactTypeType.WEB
                    Case ContactType.Main
                        oContact.ContactTypeCode = ContactTypeType.MAIN
                    Case ContactType.Telephone
                        oContact.ContactTypeCode = ContactTypeType.TELEPHONE
                    Case ContactType.Other
                        oContact.ContactTypeCode = ContactTypeType.OTHER
                        oContact.OtherContactTypeCode = sContactType
                    Case ContactType.Letter
                        oContact.ContactTypeCode = ContactTypeType.LETTER
                    Case ContactType.MainEmailContact
                        oContact.ContactTypeCode = ContactTypeType.MAINEMAILCONTACT
                End Select

                oContactDetail.Item = Cast.ToString(dr.Item("number"), String.Empty).Trim
                oContact.AreaCode = Cast.ToString(dr.Item("area_code"), String.Empty).Trim
                ' TODO: NOT AVAILABLE in SP
                'oContact.ContactTypeId = Cast.ToInt32(dr.Item(Contact_Type_id), 0) ' Vivek: 20080704
                'oContact.Description = Cast.ToString(dr.Item(description)) ' Vivek: 20080704

                oContact.ContactDetail = oContactDetail
                oContacts(iContact) = oContact

                iContact += 1

            Next

            Return oContacts

        End If

        Return Nothing

    End Function

    Private Function GetPartyAccidents(ByVal con As SiriusConnection, _
    ByVal lPartyCnt As Integer) As BasePartyOTHERTypeAccident()

        Dim dt As DataTable = Nothing

        ' get party data
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Party_Accidents")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = lPartyCnt

            dt = con.ExecuteDataTable(cmd)

        End Using

        ' if the party has had previous accidents
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim oAccidents As BasePartyOTHERTypeAccident() = Nothing

            ' resize array
            Array.Resize(oAccidents, dt.Rows.Count)

            Dim iAccident As Integer = 0

            ' for each previous accident
            For Each dr As DataRow In dt.Rows

                Dim oAccident As New BasePartyOTHERTypeAccident

                ' add the item to the array
                oAccident.AccidentKey = Cast.ToInt32(dr.Item("previous_accidents_id"), 0)
                oAccident.Date = Cast.ToDateTime(dr.Item("date"), Date.Today)
                oAccident.Description = Cast.ToString(dr.Item("description"), String.Empty).Trim
                oAccident.IsAtFault = CBool(Cast.DefaultIfNull(dr.Item("is_at_fault"), False))

                oAccidents(iAccident) = oAccident

                iAccident += 1

            Next

            Return oAccidents

        End If

        Return Nothing

    End Function

    Private Function GetPartySupplierBusiness(ByVal con As SiriusConnection, _
    ByVal lPartyCnt As Integer) As BasePartyOTHERTypeSupplierBusiness()

        Dim dt As DataTable = Nothing

        ' get party data
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Party_Supplier_Business")

            cmd.AddInParameter("party_cnt", SqlDbType.Int).Value = lPartyCnt

            dt = con.ExecuteDataTable(cmd)

        End Using
        ' if the party has had previous accidents
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim oSupplierBusinesses As BasePartyOTHERTypeSupplierBusiness() = Nothing

            ' resize array
            Array.Resize(oSupplierBusinesses, dt.Rows.Count)

            Dim iSupplierBusiness As Integer = 0

            ' for each previous accident
            For Each dr As DataRow In dt.Rows

                Dim oSupplierBusiness As New BasePartyOTHERTypeSupplierBusiness

                oSupplierBusiness.BusinessCode = Cast.ToString(dr.Item("business_code"), String.Empty).Trim
                oSupplierBusiness.SpecialityCode = Cast.ToString(dr.Item("speciality_code"), String.Empty).Trim

                oSupplierBusinesses(iSupplierBusiness) = oSupplierBusiness

                iSupplierBusiness += 1

            Next

            Return oSupplierBusinesses

        End If

        Return Nothing

    End Function

    Private Function GetPartyConvictions(ByVal con As SiriusConnection, _
    ByVal lPartyCnt As Integer) As BasePartyOTHERTypeConviction()

        Dim dt As DataTable = Nothing

        ' get party data
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Party_Convictions")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = lPartyCnt

            dt = con.ExecuteDataTable(cmd)

        End Using

        ' if the party has had previous accidents
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim oConvictions As BasePartyOTHERTypeConviction() = Nothing

            ' resize array
            Array.Resize(oConvictions, dt.Rows.Count)

            Dim iConviction As Integer = 0

            ' for each previous accident
            For Each dr As DataRow In dt.Rows

                Dim oConviction As New BaseImplementationTypes.BasePartyOTHERTypeConviction

                oConviction.ConvictionKey = Cast.ToInt32(dr.Item("party_conviction_id"), 0)
                oConviction.TypeCode = Cast.ToString(dr.Item("code"), String.Empty)
                oConviction.Date = CDate(Cast.ToString(dr.Item("conviction_date"), Date.MinValue.ToString))
                oConviction.Description = Cast.ToString(dr.Item("Description"), String.Empty)
                oConviction.FineAmount = Cast.ToDecimal(dr.Item("fine_amt"), 0)
                oConviction.SentenceTypeCode = Cast.ToString(dr.Item("sentence_code"), String.Empty)
                oConviction.SentenceDescription = Cast.ToString(dr.Item("sentence_description"), String.Empty)
                oConviction.SentenceDuration = Cast.ToDecimal(dr.Item("sentence_duration"), 0)
                oConviction.SentenceDurationQualifier = Cast.ToString(dr.Item("sentence_duration_qualifier"), String.Empty).Trim
                oConviction.SentenceEffectiveDate = CDate(Cast.ToString(dr.Item("sentence_effective_date"), Date.MinValue.ToString))
                oConviction.StatusCode = Cast.ToString(dr.Item("status_code"), String.Empty)
                oConviction.AlcoholLevel = Cast.ToDecimal(dr.Item("alcohol_level"), 0)
                oConviction.AlcoholMeasurementMethod = Cast.ToString(dr.Item("alcohol_measurement_method"), String.Empty)
                oConviction.DrivingLicencePenaltyPoints = Cast.ToDecimal(dr.Item("driving_licence_penalty_pts"), 0)

                oConvictions(iConviction) = oConviction

                iConviction += 1

            Next

            Return oConvictions

        End If

        Return Nothing

    End Function
    ' TODO: Need to change this name. Gaurav will suggest some name

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and gets the Party conviction
    '''</summary>
    '''<param name="PartyKey" type="Integer"></param>
    '''<remarks></remarks>

    Private Function GetPartyConvictions_v2(ByVal con As SiriusConnection, _
    ByVal lPartyCnt As Integer) As BaseConvictionType()

        Dim dt As DataTable = Nothing

        ' get party data
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_conviction_saa")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = lPartyCnt

            dt = con.ExecuteDataTable(cmd)

        End Using

        ' if the party has had previous accidents
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim oConvictions As BaseConvictionType() = Nothing

            ' resize array
            Array.Resize(oConvictions, dt.Rows.Count)

            Dim iConviction As Integer = 0

            ' for each previous accident
            For Each dr As DataRow In dt.Rows

                Dim oConviction As New BaseImplementationTypes.BaseConvictionType

                oConviction.ConvictionKey = Cast.ToInt32(dr.Item("party_conviction_id"), 0)
                oConviction.TypeCode = Cast.ToString(dr.Item("code"), String.Empty)

                '20080525
                If IsDBNull(dr.Item("conviction_date")) Then
                    oConviction.Date = Nothing
                Else
                    oConviction.Date = Convert.ToDateTime(dr.Item("conviction_date"))
                End If
                oConviction.Description = Cast.ToString(dr.Item("Description"), String.Empty)
                oConviction.FineAmount = Cast.ToDecimal(dr.Item("fine_amt"), 0)
                oConviction.FineAmountSpecified = Not IsDBNull(dr.Item("fine_amt"))
                oConviction.SentenceTypeCode = Cast.ToString(dr.Item("sentence_code"), String.Empty)
                oConviction.SentenceDescription = Cast.ToString(dr.Item("sentence_description"), String.Empty)
                oConviction.SentenceDuration = Cast.ToDecimal(dr.Item("sentence_duration"), 0)
                oConviction.SentenceDurationSpecified = Not IsDBNull(dr.Item("sentence_duration"))
                oConviction.SentenceDurationQualifier = Cast.ToString(dr.Item("sentence_duration_qualifier"), String.Empty).Trim
                '20080525
                If IsDBNull(dr.Item("sentence_effective_date")) OrElse Convert.ToDateTime(Cast.NullIfDefault(dr.Item("sentence_effective_date"), Date.MinValue)) <= Date.MinValue Then
                    oConviction.SentenceEffectiveDate = Nothing
                Else
                    oConviction.SentenceEffectiveDate = Convert.ToDateTime(dr.Item("sentence_effective_date"))
                End If
                oConviction.SentenceEffectiveDateSpecified = Not IsDBNull(dr.Item("sentence_effective_date"))
                oConviction.StatusCode = Cast.ToString(dr.Item("status_code"), String.Empty)
                oConviction.AlcoholLevel = Cast.ToDecimal(dr.Item("alcohol_level"), 0)
                If (oConviction.AlcoholLevel > 0) Then
                    oConviction.AlcoholLevelSpecified = True
                End If
                oConviction.AlcoholMeasurementMethod = Cast.ToString(dr.Item("alcohol_measurement_method"), String.Empty)
                oConviction.DrivingLicensePenaltyPoints = Cast.ToDecimal(dr.Item("driving_licence_penalty_pts"), 0)
                If (oConviction.DrivingLicensePenaltyPoints > 0) Then
                    oConviction.DrivingLicensePenaltyPointsSpecified = True
                End If
                oConvictions(iConviction) = oConviction

                iConviction += 1

            Next

            Return oConvictions

        End If

        Return Nothing

    End Function

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and gets the party associates
    '''</summary>
    '''<param name="lPartyCnt" type="integer"></param>
    '''<remarks></remarks>

    Private Function GetPartyAssociates(ByVal con As SiriusConnection, _
    ByVal lPartyCnt As Integer) As BaseAssociateType()

        Dim dt As DataTable = Nothing
        Dim dt1 As DataTable = Nothing
        Dim dt2 As DataTable = Nothing
        ' get party data
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_PartyAssociates")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = lPartyCnt

            dt = con.ExecuteDataTable(cmd)

        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim oAssociates As BaseAssociateType() = Nothing

            ' resize array
            Array.Resize(oAssociates, dt.Rows.Count)

            Dim iAssociate As Integer = 0

            For Each dr As DataRow In dt.Rows

                Dim oAssociate As New BaseImplementationTypes.BaseAssociateType

                oAssociate.AssociateKey = Cast.ToInt32(dr.Item("relation_cnt"), 0)
                If Cast.ToInt32(dr.Item("relationship_type_id"), 0) = 0 Then
                    oAssociate.RelationshipCode = Nothing
                Else
                    oAssociate.RelationshipCode = GetListCodeFromID(Core.STSListType.PMLookup, "Relationship_Type", Cast.ToInt32(dr.Item("relationship_type_id"), 0))
                End If

                oAssociate.AssociateKey = Cast.ToInt32(dr.Item("relation_cnt"), 0)
                oAssociate.ClientKey = Cast.ToInt32(dr.Item("party_cnt"), 0)
                oAssociate.RelationshipDescription = Cast.ToString(dr.Item("Description"), String.Empty)

                oAssociate.AssociateCode = Cast.ToString(dr.Item("shortname"), String.Empty)
                oAssociate.AssociateName = Cast.ToString(dr.Item("resolved_name"), String.Empty)
                oAssociate.CurrencyCode = Cast.ToString(dr.Item("CurrencyCode"), String.Empty)
                ' get Account Balance
                Using cmd1 As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_ACT_Select_AccountBal")

                    cmd1.AddInParameter("@party_cnt", SqlDbType.Int).Value = oAssociate.AssociateKey

                    dt1 = con.ExecuteDataTable(cmd1)

                End Using
                If dt1 IsNot Nothing AndAlso dt1.Rows.Count > 0 Then
                    oAssociate.AccountBalance = Cast.ToDecimal(dt1.Rows(0).Item("SumAmount"), 0)

                End If
                ' get Claim Incurred
                Using cmd2 As SiriusCommand = SiriusCommand.FromProcedure("spu_Select_Incurred_on_all_claims")

                    cmd2.AddInParameter("@party_cnt", SqlDbType.Int).Value = oAssociate.AssociateKey

                    dt2 = con.ExecuteDataTable(cmd2)

                End Using
                If dt2 IsNot Nothing AndAlso dt2.Rows.Count > 0 Then
                    oAssociate.ClaimIncurred = Cast.ToDecimal(dt2.Rows(0).Item("Claim_Incurred"), 0)
                End If
                oAssociates(iAssociate) = oAssociate

                iAssociate += 1

            Next

            Return oAssociates

        End If

        Return Nothing

    End Function

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and gets the party life style
    '''</summary>
    '''<param name="lPartyCnt" type="integer"></param>
    '''<remarks></remarks>

    Private Function GetPartyLifeStyle(ByVal con As SiriusConnection, _
    ByVal lPartyCnt As Integer) As BasePartyPCTypeLifestyle()

        Dim dt As DataTable = Nothing

        ' get party data
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_lifestyle_saa")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = lPartyCnt

            dt = con.ExecuteDataTable(cmd)

        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim oLifeStyles As BasePartyPCTypeLifestyle() = Nothing

            ' resize array
            Array.Resize(oLifeStyles, dt.Rows.Count)

            Dim iLifeStyle As Integer = 0

            For Each dr As DataRow In dt.Rows

                Dim oLifeStyle As New BaseImplementationTypes.BasePartyPCTypeLifestyle

                oLifeStyle.LifestyleKey = Cast.ToInt32(dr.Item("party_lifestyle_id"), 0)
                oLifeStyle.Name = Cast.ToString(dr.Item("name"), String.Empty)
                '20080525
                oLifeStyle.DateOfBirth = Cast.ToDateTime(dr.Item("date_of_birth"), Nothing)
                oLifeStyle.DateOfBirthSpecified = Not IsDBNull(dr.Item("date_of_birth"))

                If Cast.ToInt32(dr.Item("category"), 0) = 0 Then
                    oLifeStyle.CategoryCode = Nothing
                Else
                    oLifeStyle.CategoryCode = GetListCodeFromID(Core.STSListType.PMLookup, "Lifestyle_Category", Cast.ToInt32(dr.Item("category"), 0))
                End If

                If String.Compare(Cast.ToString(dr.Item("gender_code"), String.Empty), "Male", True) = 0 Then
                    oLifeStyle.GenderCode = BaseImplementationTypes.GenderCodeType.Male
                Else
                    oLifeStyle.GenderCode = BaseImplementationTypes.GenderCodeType.Female
                End If
                oLifeStyle.GenderCodeSpecified = Not IsDBNull(dr.Item("gender_code"))

                oLifeStyle.OccupationCode = Cast.ToString(dr.Item("occupation_code"), String.Empty)

                oLifeStyle.SecOccupationCode = Cast.ToString(dr.Item("secondary_occupation_code"), String.Empty)

                If Cast.ToInt16(dr.Item("is_smoker"), 0) = 1 Then
                    oLifeStyle.Smoker = True
                Else
                    oLifeStyle.Smoker = False
                End If
                '20080525
                oLifeStyle.SmokerSpecified = Not IsDBNull(dr.Item("is_smoker"))

                oLifeStyles(iLifeStyle) = oLifeStyle

                iLifeStyle += 1

            Next

            Return oLifeStyles

        End If

        Return Nothing

    End Function

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and gets the party loyalty scheme
    '''</summary>
    '''<param name="lPartyCnt" type="integer"></param>
    '''<remarks></remarks>

    Private Function GetPartyLoyaltyScheme(ByVal con As SiriusConnection, _
    ByVal lPartyCnt As Integer) As BaseClientSharedDataTypeLoyaltyScheme()

        Dim dt As DataTable = Nothing

        ' get party data
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_GET_PartyLoyaltyScheme")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = lPartyCnt

            dt = con.ExecuteDataTable(cmd)

        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim oLoyaltySchemes As BaseClientSharedDataTypeLoyaltyScheme() = Nothing

            ' resize array
            Array.Resize(oLoyaltySchemes, dt.Rows.Count)

            Dim iLoyaltyScheme As Integer = 0

            For Each dr As DataRow In dt.Rows

                Dim oLoyaltyScheme As New BaseImplementationTypes.BaseClientSharedDataTypeLoyaltyScheme

                oLoyaltyScheme.LoyaltySchemeKey = Cast.ToInt32(dr.Item("party_loyalty_scheme_id"), 0)

                If Cast.ToInt32(dr.Item("loyalty_scheme_id"), 0) = 0 Then
                    oLoyaltyScheme.LoyaltySchemeCode = Nothing
                Else
                    oLoyaltyScheme.LoyaltySchemeCode = GetListCodeFromID(Core.STSListType.PMLookup, "Loyalty_Scheme", Cast.ToInt32(dr.Item("loyalty_scheme_id"), 0))
                End If

                oLoyaltyScheme.MembershipNumber = Cast.ToString(dr.Item("membership_number"), String.Empty)
                oLoyaltyScheme.OtherReference = Cast.ToString(dr.Item("other_reference"), String.Empty)
                oLoyaltyScheme.StartDate = Cast.ToDateTime(dr.Item("start_date"), Nothing)
                oLoyaltyScheme.EndDate = Cast.ToDateTime(dr.Item("end_date"), Nothing)
                oLoyaltyScheme.EndDateSpecified = Not IsDBNull(dr.Item("end_date"))
                oLoyaltyScheme.MainMember = Cast.ToString(dr.Item("main_membership_number"), String.Empty)

                If Cast.ToInt16(dr.Item("is_active"), 0) = 1 Then
                    oLoyaltyScheme.Active = True
                Else
                    oLoyaltyScheme.Active = False
                End If
                oLoyaltyScheme.ActiveSpecified = Not IsDBNull(dr.Item("is_active"))

                oLoyaltySchemes(iLoyaltyScheme) = oLoyaltyScheme

                iLoyaltyScheme += 1

            Next

            Return oLoyaltySchemes

        End If

        Return Nothing

    End Function

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and gets the party prospect policies
    '''</summary>
    '''<param name="lPartyCnt" type="integer"></param>
    '''<remarks></remarks>

    Private Function GetPartyProspectPolicies(ByVal con As SiriusConnection, _
    ByVal lPartyCnt As Integer) As BaseClientSharedDataTypeProspectPolicies()

        Dim dt As DataTable = Nothing

        ' get party data
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_PartyProspectPolicies")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = lPartyCnt

            dt = con.ExecuteDataTable(cmd)

        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim oProspectPolicies As BaseClientSharedDataTypeProspectPolicies() = Nothing

            ' resize array
            Array.Resize(oProspectPolicies, dt.Rows.Count)

            Dim iProspectPolicy As Integer = 0

            For Each dr As DataRow In dt.Rows

                Dim oProspectPolicy As New BaseImplementationTypes.BaseClientSharedDataTypeProspectPolicies

                oProspectPolicy.ProspectPolicyKey = Cast.ToInt32(dr.Item("prospect_policy_id"), 0)

                If Cast.ToInt32(dr.Item("risk_group_id"), 0) = 0 Then
                    oProspectPolicy.ProspectTypeCode = Nothing
                Else
                    oProspectPolicy.ProspectTypeCode = GetListCodeFromID(Core.STSListType.PMLookup, "Risk_Group", Cast.ToInt32(dr.Item("risk_group_id"), 0))
                End If

                oProspectPolicy.RenewalDate = Cast.ToDateTime(dr.Item("renewal_date"), Nothing)
                oProspectPolicy.RenewalDateSpecified = Not IsDBNull(dr.Item("renewal_date"))
                oProspectPolicy.TimesQuoted = Cast.ToInt32(dr.Item("no_of_times_quoted"), 0)
                oProspectPolicy.TimesQuotedSpecified = Not IsDBNull(dr.Item("no_of_times_quoted"))
                oProspectPolicy.TargetPremium = Cast.ToDecimal(dr.Item("target_premium"), 0)
                oProspectPolicy.TargetPremiumSpecified = Not IsDBNull(dr.Item("target_premium"))

                oProspectPolicies(iProspectPolicy) = oProspectPolicy

                iProspectPolicy += 1

            Next

            Return oProspectPolicies

        End If

        Return Nothing

    End Function

    Private Sub GetPartyOtherData(ByVal con As SiriusConnection, _
    ByVal oRequest As BaseGetPartyRequestType, _
    ByVal lPartyCnt As Integer, _
    ByVal oParty As BasePartyOTHERType, _
    ByVal dr As DataRow)

        oParty.Code = Cast.ToStringTrim(dr.Item("shortname"), String.Empty)
        oParty.Name = Cast.ToStringTrim(dr.Item("resolved_name"), String.Empty)
        oParty.TypeCode = Cast.ToStringTrim(dr.Item("party_type_code"), String.Empty)
        oParty.LicenseTypeCode = Cast.ToStringTrim(dr.Item("license_type_code"), String.Empty)
        oParty.DriverStatusCode = Cast.ToStringTrim(dr.Item("driver_status_code"), String.Empty)
        oParty.LicenseNumber = Cast.ToStringTrim(dr.Item("license_number"), String.Empty)
        oParty.DateOfBirth = Cast.ToString(dr.Item("date_of_birth").ToString, Date.MinValue.ToString)
        oParty.Gender = Cast.ToStringTrim(dr.Item("gender"), String.Empty)
        oParty.RegNumber = Cast.ToStringTrim(dr.Item("reg_number"), String.Empty)
        oParty.ActiveIndicator = CBool(Cast.DefaultIfNull(dr.Item("active_indicator"), False))
        oParty.AfterHoursIndicator = CBool(Cast.DefaultIfNull(dr.Item("after_hours_indicator"), False))
        oParty.PriorityIndicator = Cast.ToInt32(dr.Item("priority_indicator"), 0)

        oParty.ActiveIndicatorSpecified = True
        oParty.AfterHoursIndicatorSpecified = True
        oParty.PriorityIndicatorSpecified = True

        ' get accidents
        oParty.Accident = GetPartyAccidents(con, lPartyCnt)

        ' get convictions

        ' Vivek: differing from Tech Spec
        If oRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetPartyRequestType) Then
            oParty.Convictions = GetPartyConvictions_v2(con, lPartyCnt)
        Else
            oParty.Conviction = GetPartyConvictions(con, lPartyCnt)
        End If

        ' get supplier business
        oParty.SupplierBusiness = GetPartySupplierBusiness(con, lPartyCnt)

        ' get party base data
        GetPartyBaseData(con, lPartyCnt, oParty, dr)

    End Sub

    ' Vivek: modified method signature to add nTypeOfPackage
    ' this is getting called only from UpdateParty 
    Private Sub ValidateUpdatePartyData(ByVal oBusiness As CoreBusiness, _
        ByVal oParty As BasePartyOTHERType, _
        ByVal lPartyCnt As Integer, _
        ByVal oSamErrorCollection As SAMErrorCollection, _
        ByVal nTypeOfPackage As enumTypeOfPackage)

        ' Vivek: differing from Tech Spec
        If nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package Then
            If oParty.Convictions IsNot Nothing Then
                ' Validate Conviction id's
                ValidateConviction(oParty.Convictions, _
                                    lPartyCnt, _
                                    oSamErrorCollection)
            End If
        Else
            If oParty.Conviction IsNot Nothing Then
                ' Validate Conviction id's
                ValidateConviction(oParty.Conviction, _
                                    lPartyCnt, _
                                    oSamErrorCollection)
            End If
        End If

        If oParty.Accident IsNot Nothing Then
            ' Validate Accident Id's
            ValidateAccident(oParty.Accident, _
                                lPartyCnt, _
                                oSamErrorCollection)
        End If

        ' license type code - pmproduct lookup
        If Not String.IsNullOrEmpty(oParty.LicenseTypeCode) Then
            oParty.LicenseTypeId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.LicenseType, oParty.LicenseTypeCode, "LicenseTypeCode", oSamErrorCollection)
        End If

        ' driver status - pmproduct lookup
        If Not String.IsNullOrEmpty(oParty.DriverStatusCode) Then
            oParty.DrivingStatusId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.DriverStatus, oParty.DriverStatusCode, "DriverStatus", oSamErrorCollection)
        End If

        ' gender code - user_defined_table table
        If Not String.IsNullOrEmpty(oParty.Gender) Then
            ' Vivek: FIXING the error: in DB we have Male / Female instead of M / F
            oParty.GenderId = Cast.ToInt32(GetListCode(Of String)(Core.STSListType.UserDefinedTable, GIITables.Gender, oParty.Gender, "caption", True, "id"), 4)
            'oParty.GenderId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.UserDefinedTable, GIITables.Gender, oParty.Gender, "Gender", oSamErrorCollection, oParty.GenderDescription)
        End If

        ' other party type must always begin with OT
        If Not oParty.TypeCode.StartsWith("OT") Then
            ' if it doesnt raise an invalid type code error
            oSamErrorCollection.AddInvalidData(SAMInvalidData.OtherPartyTypeCodeDoesntStartWithOT, _
                                                SAMInvalidData.OtherPartyTypeCodeDoesntStartWithOT.ToString, _
                                                "TypeCode")
        Else
            ' party type 
            If Not String.IsNullOrEmpty(oParty.TypeCode) Then
                oParty.TypeId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.PartyType, oParty.TypeCode, "TypeCode", oSamErrorCollection)
            End If
        End If

        ' if the specified party type is not a supplier
        If oParty.TypeCode <> OtherParty.Supplier Then

            ' ensure that no supplier business has been specified
            If oParty.SupplierBusiness IsNot Nothing Then

                ' if it doesnt raise an invalid type code error
                oSamErrorCollection.AddInvalidData(SAMInvalidData.SupplierBusinessShouldNotBeSpecifiedWhenThePartyIsNotASupplier, _
                                                    SAMInvalidData.SupplierBusinessShouldNotBeSpecifiedWhenThePartyIsNotASupplier.ToString, _
                                                    "SupplierBusiness")

            End If

        Else

            ' if this is a supplier 
            If oParty.SupplierBusiness IsNot Nothing Then

                ' validate that both lookups items in each supplier business are valid
                For Each oSupplierBusiness As BasePartyOTHERTypeSupplierBusiness In oParty.SupplierBusiness

                    ' supplier business - pmproduct lookup
                    oSupplierBusiness.BusinessId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.SupplierBusiness, oSupplierBusiness.BusinessCode, "BusinessCode", oSamErrorCollection)

                    ' supplier speciality - pmproduct lookup ( note speciality doesnt need to be provided)
                    If Not String.IsNullOrEmpty(oSupplierBusiness.SpecialityCode) Then
                        oSupplierBusiness.SpecialityId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.SupplierSpeciality, oSupplierBusiness.SpecialityCode, "SpecialityCode", oSamErrorCollection)
                    Else
                        oSupplierBusiness.SpecialityId = 0
                    End If

                Next

            End If

        End If

    End Sub

    Private Sub ValidateConviction(ByVal oConviction() As SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyOTHERTypeConviction, _
                                        ByVal lPartyCnt As Integer, _
                                        ByVal oSamErrorCollection As SAMErrorCollection)

        Dim oBusiness As New CoreBusiness
        Dim r_dsOptionalValues As New DataSet

        For ConvictionCount As Integer = oConviction.GetLowerBound(0) To oConviction.GetUpperBound(0)
            If oConviction(ConvictionCount).ConvictionKey <> 0 Then
                oBusiness.setDefaultValues("Party_Conviction", _
                                                "party_cnt", _
                                                 lPartyCnt.ToString, _
                                                r_dsOptionalValues, _
                                                "Party_Conviction_Id", _
                                                oConviction(ConvictionCount).ConvictionKey.ToString)

                If r_dsOptionalValues.Tables(0).Rows.Count = 0 Then
                    ' add an error to the collection
                    oSamErrorCollection.AddInvalidData(SAMInvalidData.GeneralFailure, _
                                                        SAMInvalidData.GeneralFailure.ToString, _
                                                        "ConvictionId", _
                                                        Cast.ToString(oConviction(ConvictionCount).ConvictionKey, String.Empty))

                    Exit Sub
                End If
            End If
        Next

    End Sub

    ' Vivek: differing from Tech Spec
    Private Sub ValidateConviction(ByVal oConviction() As BaseImplementationTypes.BaseConvictionType, _
                                        ByVal lPartyCnt As Integer, _
                                        ByVal oSamErrorCollection As SAMErrorCollection)

        Dim oBusiness As New CoreBusiness
        Dim r_dsOptionalValues As New DataSet

        For ConvictionCount As Integer = oConviction.GetLowerBound(0) To oConviction.GetUpperBound(0)
            If oConviction(ConvictionCount).ConvictionKey <> 0 Then
                oBusiness.setDefaultValues("Party_Conviction", _
                                                "party_cnt", _
                                                 lPartyCnt.ToString, _
                                                r_dsOptionalValues, _
                                                "Party_Conviction_Id", _
                                                oConviction(ConvictionCount).ConvictionKey.ToString)

                If r_dsOptionalValues.Tables(0).Rows.Count = 0 Then
                    ' add an error to the collection
                    oSamErrorCollection.AddInvalidData(SAMInvalidData.GeneralFailure, _
                                                        SAMInvalidData.GeneralFailure.ToString, _
                                                        "ConvictionId", _
                                                        Cast.ToString(oConviction(ConvictionCount).ConvictionKey, String.Empty))

                    Exit Sub
                End If
            End If
        Next

    End Sub

    Private Sub ValidateAccident(ByVal oAccidents() As SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyOTHERTypeAccident, _
                                    ByVal lPartyCnt As Integer, _
                                    ByVal oSamErrorCollection As SAMErrorCollection)
        Dim oBusiness As New CoreBusiness
        Dim r_dsOptionalValues As New DataSet

        For AccidentCount As Integer = oAccidents.GetLowerBound(0) To oAccidents.GetUpperBound(0)
            If oAccidents(AccidentCount).AccidentKey <> 0 Then
                Dim strParty As String
                strParty = Convert.ToString(lPartyCnt)

                Dim strAccKey As String
                strAccKey = Convert.ToString(oAccidents(AccidentCount).AccidentKey)

                oBusiness.setDefaultValues("Previous_Accidents", _
                                                "party_cnt", _
                                                strParty, _
                                                r_dsOptionalValues, _
                                                "previous_accidents_Id", _
                                                strAccKey)

                If r_dsOptionalValues.Tables(0).Rows.Count = 0 Then
                    ' add an error to the collection
                    oSamErrorCollection.AddInvalidData(SAMInvalidData.GeneralFailure, _
                                                        SAMInvalidData.GeneralFailure.ToString, _
                                                        "AccidentId", _
                                                        oAccidents(AccidentCount).AccidentKey.ToString)

                    Exit Sub

                End If
            End If
        Next

    End Sub

    'GAURAV
    Private Sub ProcessConvictions(ByVal oPartyConvictions() As SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyOTHERTypeConviction, _
                                            ByRef r_oConvictions(,) As Object, _
                                            Optional ByVal lPartyCnt As Integer = 0)
        Dim oBusiness As New CoreBusiness
        Dim r_dsOptionalValues As New DataSet

        ReDim r_oConvictions(oPartyConvictions.GetUpperBound(0), 14)
        For Cnt As Integer = oPartyConvictions.GetLowerBound(0) To oPartyConvictions.GetUpperBound(0)
            r_oConvictions(Cnt, 0) = lPartyCnt ' Mandatory
            r_oConvictions(Cnt, 1) = oPartyConvictions(Cnt).TypeCode ' Mandatory
            r_oConvictions(Cnt, 2) = oPartyConvictions(Cnt).StatusCode ' Mandatory
            r_oConvictions(Cnt, 3) = oPartyConvictions(Cnt).Description ' Mandatory
            r_oConvictions(Cnt, 5) = oPartyConvictions(Cnt).Date ' Mandatory

            r_oConvictions(Cnt, 4) = oPartyConvictions(Cnt).FineAmount 'Optional
            r_oConvictions(Cnt, 6) = oPartyConvictions(Cnt).SentenceTypeCode 'Optional
            r_oConvictions(Cnt, 7) = oPartyConvictions(Cnt).SentenceDescription 'Optional
            r_oConvictions(Cnt, 8) = oPartyConvictions(Cnt).SentenceDuration 'Optional
            r_oConvictions(Cnt, 9) = oPartyConvictions(Cnt).SentenceDurationQualifier 'Optional
            r_oConvictions(Cnt, 10) = oPartyConvictions(Cnt).SentenceEffectiveDate 'Optional
            r_oConvictions(Cnt, 11) = oPartyConvictions(Cnt).AlcoholLevel 'Optional
            r_oConvictions(Cnt, 12) = oPartyConvictions(Cnt).AlcoholMeasurementMethod 'Optional
            r_oConvictions(Cnt, 13) = oPartyConvictions(Cnt).DrivingLicencePenaltyPoints 'Optional
            r_oConvictions(Cnt, 14) = oPartyConvictions(Cnt).ConvictionKey

            'oPartyConvictions(Cnt).ConvictionKey = Cnt + 1
            'r_oConvictions(Cnt, 14) = Cnt + 1
            If oPartyConvictions(Cnt).ConvictionKey <> 0 Then
                oBusiness.setDefaultValues("Party_Conviction", _
                                                "party_cnt", _
                                                lPartyCnt.ToString, _
                                                r_dsOptionalValues, _
                                                "Party_Conviction_Id", _
                                                Cast.ToString(oPartyConvictions(Cnt).ConvictionKey, String.Empty))

                If oPartyConvictions(Cnt).FineAmount = Nothing Then r_oConvictions(Cnt, 4) = r_dsOptionalValues.Tables(0).Rows(0).Item("fine_amt")
                If oPartyConvictions(Cnt).SentenceTypeCode = Nothing Then r_oConvictions(Cnt, 6) = r_dsOptionalValues.Tables(0).Rows(0).Item("sentence_code")
                If oPartyConvictions(Cnt).SentenceDescription = Nothing Then r_oConvictions(Cnt, 7) = r_dsOptionalValues.Tables(0).Rows(0).Item("sentence_description")
                If oPartyConvictions(Cnt).SentenceDuration = Nothing Then r_oConvictions(Cnt, 8) = r_dsOptionalValues.Tables(0).Rows(0).Item("sentence_duration")
                If oPartyConvictions(Cnt).SentenceDurationQualifier = Nothing Then r_oConvictions(Cnt, 9) = r_dsOptionalValues.Tables(0).Rows(0).Item("sentence_duration_qualifier")
                If oPartyConvictions(Cnt).SentenceEffectiveDate = Nothing Then r_oConvictions(Cnt, 10) = r_dsOptionalValues.Tables(0).Rows(0).Item("sentence_effective_date")
                If oPartyConvictions(Cnt).AlcoholLevel = Nothing Then r_oConvictions(Cnt, 11) = r_dsOptionalValues.Tables(0).Rows(0).Item("alcohol_level")
                If oPartyConvictions(Cnt).AlcoholMeasurementMethod = Nothing Then r_oConvictions(Cnt, 12) = r_dsOptionalValues.Tables(0).Rows(0).Item("alcohol_measurement_method")
                If oPartyConvictions(Cnt).DrivingLicencePenaltyPoints = Nothing Then r_oConvictions(Cnt, 13) = r_dsOptionalValues.Tables(0).Rows(0).Item("driving_licence_penalty_pts")

            End If
        Next

    End Sub

    'GAURAV

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and  process the associates
    '''</summary>
    '''<param name="oAssociates" type="BaseAssociateType"></param>
    '''<remarks></remarks>

    ' got from Jaideep
    Private Sub ProcessAssociates( _
    ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oAssociates As BaseAssociateType(), _
    ByVal PartyKey As Integer)

        ' get existing party Associates
        Dim oExistingAssociates As BaseAssociateType() = GetPartyAssociates(con, PartyKey)

        If oAssociates IsNot Nothing Then

            ' set default processing status for each conviction
            For Each oAssociate As BaseAssociateType In oAssociates

                '20080605
                ' by default mark all records to be inserted
                ' - this is because we can't check if AssociateKey = 0, as its not the primary key, but foreign key
                oAssociate.ProcessingStatus = ProcessingStatus.Insert
            Next

            If oExistingAssociates IsNot Nothing Then

                For Each oExistingAssociate As BaseAssociateType In oExistingAssociates

                    Dim bFound As Boolean = False

                    For Each oAssociate As BaseAssociateType In oAssociates

                        ' if any of the records are existing mark them to be updated
                        If oAssociate.AssociateKey = oExistingAssociate.AssociateKey Then

                            ' update the Associate record - mark as to be updated
                            oAssociate.ProcessingStatus = ProcessingStatus.Update

                            bFound = True

                            Exit For
                        End If

                    Next

                    If bFound <> True Then
                        oExistingAssociate.ProcessingStatus = ProcessingStatus.Delete
                    End If
                Next

            End If

            ' process each Associate 
            For Each oAssociate As BaseAssociateType In oAssociates

                If oAssociate.ProcessingStatus = ProcessingStatus.Insert Then

                    ' insert Associate
                    AddPartyAssociates(con, oBusiness, oAssociate, PartyKey)

                ElseIf oAssociate.ProcessingStatus = ProcessingStatus.Update Then

                    ' update Associate
                    UpdatePartyAssociates(con, oBusiness, oAssociate, PartyKey)

                End If

            Next

        End If

        If oExistingAssociates IsNot Nothing Then
            For Each oAssociate As BaseAssociateType In oExistingAssociates

                '20080525
                If oAssociate.ProcessingStatus = ProcessingStatus.Delete OrElse oAssociates Is Nothing Then

                    ' delete Associate
                    DeletePartyAssociates(con, oAssociate.AssociateKey, PartyKey)

                End If

            Next
        End If
    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and process the life style
    '''</summary>
    '''<param name="oLifeStyles" type="BasePartyPCTypeLifestyle"></param>
    '''<remarks></remarks>
    ' got from Jaideep
    Private Sub ProcessLifeStyle( _
    ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oLifeStyles As BasePartyPCTypeLifestyle(), _
    ByVal PartyKey As Integer)

        ' get existing party LifeStyles
        Dim oExistingLifeStyles As BasePartyPCTypeLifestyle() = GetPartyLifeStyle(con, PartyKey)

        If oLifeStyles IsNot Nothing Then

            ' set default processing status for each LifeStyle
            For Each oLifeStyle As BasePartyPCTypeLifestyle In oLifeStyles

                If oLifeStyle.LifestyleKey = 0 Then

                    ' update the LifeStyle record - mark as to be updated
                    oLifeStyle.ProcessingStatus = ProcessingStatus.Insert
                End If
            Next

            If oExistingLifeStyles IsNot Nothing Then

                For Each oExistingLifeStyle As BasePartyPCTypeLifestyle In oExistingLifeStyles

                    Dim bFound As Boolean = False

                    For Each oLifeStyle As BasePartyPCTypeLifestyle In oLifeStyles

                        If oLifeStyle.LifestyleKey = oExistingLifeStyle.LifestyleKey Then

                            ' update the LifeStyle record - mark as to be updated
                            oLifeStyle.ProcessingStatus = ProcessingStatus.Update

                            bFound = True

                            Exit For
                        End If

                    Next

                    If bFound <> True Then

                        If UCase(oExistingLifeStyle.CategoryCode) <> "INSURED" Then
                        oExistingLifeStyle.ProcessingStatus = ProcessingStatus.Delete
                        Else
                            oExistingLifeStyle.ProcessingStatus = ProcessingStatus.Update
                        End If

                    End If

                Next

            End If

            ' process each LifeStyle 
            For Each oLifeStyle As BasePartyPCTypeLifestyle In oLifeStyles

                If oLifeStyle.ProcessingStatus = ProcessingStatus.Insert Then

                    ' insert LifeStyle
                    AddPartyLifeStyle(con, oBusiness, oLifeStyle, PartyKey)

                ElseIf oLifeStyle.ProcessingStatus = ProcessingStatus.Update Then

                    ' update LifeStyle
                    UpdatePartyLifeStyle(con, oBusiness, oLifeStyle, PartyKey)

                End If

            Next

        End If

            If oExistingLifeStyles IsNot Nothing Then
                For Each oLifeStyle As BasePartyPCTypeLifestyle In oExistingLifeStyles

                    '20080525
		If oLifeStyle.ProcessingStatus = ProcessingStatus.Delete Then

                        ' delete LifeStyle
                        DeletePartyLifeStyle(con, oLifeStyle.LifestyleKey, PartyKey)

                    End If

                Next
            End If
    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and process the loyalty scheme
    '''</summary>
    '''<param name="oLoyaltySchemes" type="BaseClientSharedDataTypeLoyaltyScheme"></param>
    '''<remarks></remarks>

    ' got from Jaideep
    Private Sub ProcessLoyaltyScheme( _
    ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oLoyaltySchemes As BaseClientSharedDataTypeLoyaltyScheme(), _
    ByVal PartyKey As Integer)

        ' get existing party LoyaltySchemes
        Dim oExistingLoyaltySchemes As BaseClientSharedDataTypeLoyaltyScheme() = GetPartyLoyaltyScheme(con, PartyKey)

        If oLoyaltySchemes IsNot Nothing Then

            ' set default processing status for each LoyaltyScheme
            For Each oLoyaltyScheme As BaseClientSharedDataTypeLoyaltyScheme In oLoyaltySchemes

                If oLoyaltyScheme.LoyaltySchemeKey = 0 Then

                    ' update the LoyaltyScheme record - mark as to be updated
                    oLoyaltyScheme.ProcessingStatus = ProcessingStatus.Insert
                End If
            Next

            If oExistingLoyaltySchemes IsNot Nothing Then

                For Each oExistingLoyaltyScheme As BaseClientSharedDataTypeLoyaltyScheme In oExistingLoyaltySchemes

                    Dim bFound As Boolean = False

                    For Each oLoyaltyScheme As BaseClientSharedDataTypeLoyaltyScheme In oLoyaltySchemes

                        If oLoyaltyScheme.LoyaltySchemeKey = oExistingLoyaltyScheme.LoyaltySchemeKey Then

                            ' update the LoyaltyScheme record - mark as to be updated
                            oLoyaltyScheme.ProcessingStatus = ProcessingStatus.Update

                            bFound = True

                            Exit For
                        End If

                    Next

                    If bFound <> True Then
                        oExistingLoyaltyScheme.ProcessingStatus = ProcessingStatus.Delete
                    End If
                Next

            End If

            ' process each LoyaltyScheme 
            For Each oLoyaltyScheme As BaseClientSharedDataTypeLoyaltyScheme In oLoyaltySchemes

                If oLoyaltyScheme.ProcessingStatus = ProcessingStatus.Insert Then

                    ' insert LoyaltyScheme
                    AddPartyLoyaltyScheme(con, oBusiness, oLoyaltyScheme, PartyKey)

                ElseIf oLoyaltyScheme.ProcessingStatus = ProcessingStatus.Update Then

                    ' update LoyaltyScheme
                    UpdatePartyLoyaltyScheme(con, oBusiness, oLoyaltyScheme, PartyKey)

                End If

            Next

        End If

        If oExistingLoyaltySchemes IsNot Nothing Then
            For Each oLoyaltyScheme As BaseClientSharedDataTypeLoyaltyScheme In oExistingLoyaltySchemes

                '20080525
                If oLoyaltyScheme.ProcessingStatus = ProcessingStatus.Delete OrElse oLoyaltySchemes Is Nothing Then

                    ' delete LoyaltyScheme
                    DeletePartyLoyaltyScheme(con, oLoyaltyScheme.LoyaltySchemeKey, PartyKey)

                End If

            Next
        End If

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and process the prospect policies
    '''</summary>
    '''<param name="oProspectPolicies" type="BaseClientSharedDataTypeProspectPolicies"></param>
    '''<remarks></remarks>

    ' got from Jaideep
    Private Sub ProcessProspectPolicies( _
    ByVal con As SiriusConnection, _
    ByVal oBusiness As CoreBusiness, _
    ByVal oProspectPolicies As BaseClientSharedDataTypeProspectPolicies(), _
    ByVal PartyKey As Integer)

        ' get existing party ProspectPolicies
        Dim oExistingProspectPolicies As BaseClientSharedDataTypeProspectPolicies() = GetPartyProspectPolicies(con, PartyKey)

        If oProspectPolicies IsNot Nothing Then

            ' set default processing status for each ProspectPolicy
            For Each oProspectPolicy As BaseClientSharedDataTypeProspectPolicies In oProspectPolicies

                If oProspectPolicy.ProspectPolicyKey = 0 Then

                    ' update the ProspectPolicy record - mark as to be updated
                    oProspectPolicy.ProcessingStatus = ProcessingStatus.Insert
                End If
            Next

            If oExistingProspectPolicies IsNot Nothing Then

                For Each oExistingProspectPolicy As BaseClientSharedDataTypeProspectPolicies In oExistingProspectPolicies

                    Dim bFound As Boolean = False

                    For Each oProspectPolicy As BaseClientSharedDataTypeProspectPolicies In oProspectPolicies

                        If oProspectPolicy.ProspectPolicyKey = oExistingProspectPolicy.ProspectPolicyKey Then

                            ' update the ProspectPolicy record - mark as to be updated
                            oProspectPolicy.ProcessingStatus = ProcessingStatus.Update

                            bFound = True

                            Exit For
                        End If

                    Next

                    If bFound <> True Then
                        oExistingProspectPolicy.ProcessingStatus = ProcessingStatus.Delete
                    End If
                Next

            End If

            ' process each ProspectPolicy 
            For Each oProspectPolicy As BaseClientSharedDataTypeProspectPolicies In oProspectPolicies

                If oProspectPolicy.ProcessingStatus = ProcessingStatus.Insert Then

                    ' insert ProspectPolicy
                    AddPartyProspectPolicies(con, oBusiness, oProspectPolicy, PartyKey)

                ElseIf oProspectPolicy.ProcessingStatus = ProcessingStatus.Update Then

                    ' update ProspectPolicy
                    UpdatePartyProspectPolicies(con, oBusiness, oProspectPolicy, PartyKey)

                End If

            Next

        End If

        If oExistingProspectPolicies IsNot Nothing Then
            For Each oProspectPolicy As BaseClientSharedDataTypeProspectPolicies In oExistingProspectPolicies

                '20080525
                If oProspectPolicy.ProcessingStatus = ProcessingStatus.Delete OrElse oProspectPolicies Is Nothing Then

                    ' delete ProspectPolicy
                    DeletePartyProspectPolicies(con, oProspectPolicy.ProspectPolicyKey, PartyKey)

                End If

            Next
        End If
    End Sub

    Private Sub ProcessAccidents(ByVal oAccidents() As SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyOTHERTypeAccident, _
                                            ByRef r_oAccidents(,) As Object, _
                                            Optional ByVal lPartyCnt As Integer = 0)
        Dim oBusiness As New CoreBusiness
        Dim r_dsOptionalValues As DataSet = Nothing

        ReDim r_oAccidents(oAccidents.GetUpperBound(0), 4)
        For cnt As Integer = oAccidents.GetLowerBound(0) To oAccidents.GetUpperBound(0)
            r_oAccidents(cnt, 0) = lPartyCnt
            r_oAccidents(cnt, 1) = oAccidents(cnt).Date
            r_oAccidents(cnt, 2) = oAccidents(cnt).Description
            r_oAccidents(cnt, 3) = oAccidents(cnt).IsAtFault
            r_oAccidents(cnt, 4) = oAccidents(cnt).AccidentKey
        Next

    End Sub

    Private Sub ProcessOtherPartyInfo(ByVal oOtherPartyInfo As SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyOTHERType, _
                                                ByRef r_oOtherPartyInfo() As Object, _
                                                Optional ByVal lPartyCnt As Integer = 0, _
                                                Optional ByVal SetDefaults As Boolean = False)

        r_oOtherPartyInfo(0) = lPartyCnt

        r_oOtherPartyInfo(1) = oOtherPartyInfo.LicenseTypeCode 'Optional
        r_oOtherPartyInfo(2) = oOtherPartyInfo.LicenseNumber 'Optional
        r_oOtherPartyInfo(3) = oOtherPartyInfo.DateOfBirth 'Optional
        r_oOtherPartyInfo(4) = oOtherPartyInfo.Gender 'Optional
        r_oOtherPartyInfo(5) = oOtherPartyInfo.DriverStatusCode 'Optional
        r_oOtherPartyInfo(6) = Nothing  ' reference_number
        r_oOtherPartyInfo(7) = Nothing ' External Id
        r_oOtherPartyInfo(8) = oOtherPartyInfo.RegNumber 'Optional
        r_oOtherPartyInfo(9) = Nothing 'date_passed_test
        r_oOtherPartyInfo(10) = Nothing 'contact_name
        r_oOtherPartyInfo(11) = Nothing 'contact_telephone_number
        r_oOtherPartyInfo(12) = Nothing 'insurer_name
        r_oOtherPartyInfo(13) = Nothing 'insurer_address1
        r_oOtherPartyInfo(14) = Nothing 'insurer_address2
        r_oOtherPartyInfo(15) = Nothing 'insurer_address3
        r_oOtherPartyInfo(16) = Nothing 'insurer_address4
        r_oOtherPartyInfo(17) = Nothing 'insurer_postcode
        r_oOtherPartyInfo(18) = Nothing 'insurer_telephone_number
        r_oOtherPartyInfo(19) = Nothing 'insurer_fax_number
        r_oOtherPartyInfo(20) = Nothing 'insurer_contact_name
        r_oOtherPartyInfo(21) = Nothing 'insurer_email
        r_oOtherPartyInfo(22) = Nothing 'insurer_notes
        r_oOtherPartyInfo(23) = Nothing 'company_notes

        r_oOtherPartyInfo(24) = oOtherPartyInfo.ActiveIndicator
        r_oOtherPartyInfo(25) = oOtherPartyInfo.AfterHoursIndicator
        r_oOtherPartyInfo(26) = oOtherPartyInfo.PriorityIndicator

        If SetDefaults = True Then

            Dim oBusiness As New CoreBusiness
            Dim r_dsOptionalValues As DataSet = Nothing
            oBusiness.setDefaultValues("Party_other", _
                                            "party_cnt", _
                                            lPartyCnt.ToString, _
                                            r_dsOptionalValues)

            If oOtherPartyInfo.LicenseTypeCode = Nothing Then r_oOtherPartyInfo(1) = r_dsOptionalValues.Tables(0).Rows(0).Item("license_type_id")
            If oOtherPartyInfo.LicenseNumber = Nothing Then r_oOtherPartyInfo(2) = r_dsOptionalValues.Tables(0).Rows(0).Item("license_number")
            If oOtherPartyInfo.DateOfBirth = Nothing Then r_oOtherPartyInfo(3) = r_dsOptionalValues.Tables(0).Rows(0).Item("date_of_birth")
            If oOtherPartyInfo.Gender = Nothing Then r_oOtherPartyInfo(4) = r_dsOptionalValues.Tables(0).Rows(0).Item("gender")
            If oOtherPartyInfo.DriverStatusCode = Nothing Then r_oOtherPartyInfo(5) = r_dsOptionalValues.Tables(0).Rows(0).Item("status_code")
            If oOtherPartyInfo.RegNumber = Nothing Then r_oOtherPartyInfo(8) = r_dsOptionalValues.Tables(0).Rows(0).Item("reg_number")
            If oOtherPartyInfo.ActiveIndicator = Nothing Then r_oOtherPartyInfo(24) = r_dsOptionalValues.Tables(0).Rows(0).Item("active_indicator")
            If oOtherPartyInfo.AfterHoursIndicator = Nothing Then r_oOtherPartyInfo(25) = r_dsOptionalValues.Tables(0).Rows(0).Item("after_hours_indicator")
            If oOtherPartyInfo.PriorityIndicator = Nothing Then r_oOtherPartyInfo(26) = r_dsOptionalValues.Tables(0).Rows(0).Item("priority_indicator")

        End If

    End Sub

    Private Sub ProcessSupplierBusiness(ByVal oSuppBusiness() As SiriusFS.SAM.Structure.BaseImplementationTypes.BasePartyOTHERTypeSupplierBusiness, _
                                            ByRef r_oSuppBusiness(,) As Object, _
                                            Optional ByVal lPartyCnt As Integer = 0)

        ReDim r_oSuppBusiness(oSuppBusiness.GetUpperBound(0), 2)
        For cnt As Integer = oSuppBusiness.GetLowerBound(0) To oSuppBusiness.GetUpperBound(0)
            r_oSuppBusiness(cnt, 0) = lPartyCnt
            r_oSuppBusiness(cnt, 1) = oSuppBusiness(cnt).BusinessCode
            r_oSuppBusiness(cnt, 2) = oSuppBusiness(cnt).SpecialityCode
        Next

    End Sub

    Private Function ProcessAddresses(ByVal oAddresses() As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseAddressType, ByRef r_vAddresses As Object, Optional ByVal con As SiriusConnection = Nothing) As SiriusFS.SAM.Structure.STSErrorPublisher

        Dim iCount As Int32
        Dim oBaseAddress As New BaseImplementationTypes.BaseAddressType
        Dim STSError As New SiriusFS.SAM.Structure.STSErrorPublisher
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim vAddresses As Object(,) = Nothing

        '26-09-08 Added as per discussion with Rahul
        Dim bFound As Boolean = False
        Dim dt As DataTable = Nothing
        For iCount = oAddresses.GetLowerBound(0) To oAddresses.GetUpperBound(0)
            If oAddresses(iCount).AddressTypeCode = AddressTypeType.Correspondence Then
                bFound = True
                Exit For
            End If
        Next
        If Not bFound Then
            STSError.AddInvalidField("AddressTypeCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "No correspondence address was provided"), "")
        End If

        If STSError.HasErrors Then
            Return STSError
        End If
        '''''''''''''''''''''''''''''

        For iCount = oAddresses.GetLowerBound(0) To oAddresses.GetUpperBound(0)

            Dim iCountryID As Integer

            oBaseAddress = oAddresses(iCount)

            ' Check Address Line 1 / CountryCode
            If SAMFunc.NothingToString(oAddresses(iCount).AddressLine1) = "" Then
                STSError.AddInvalidField("AddressLine1", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "AddressLine1"), "")
            End If
            If oAddresses(iCount).CountryCode = "" Then
                STSError.AddInvalidField("CountryCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "CountryCode"), "")
            End If

            ' exit if there are any missing parameters
            If STSError.HasErrors Then
                Return STSError
            End If

            ' Lookup the Country Code
            Try
                iCountryID = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Country", oAddresses(iCount).CountryCode)
            Catch ex As Exception
                STSError.AddInvalidField("CountryCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "CountryCode"), oAddresses(iCount).CountryCode)
            End Try

            ' exit if there are any invalid parameters
            If STSError.HasErrors Then
                Return STSError
            End If

            ' Is the Postcode required for this Country Code ?
            If oCoreBusiness.GetPostCodeVisibility(iCountryID) = InternalSAMConstants.PMPostCodeVisibilityVisible Then
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_CountryIDPostCodeVisibility")
                    cmd.AddInParameter("@country_id", SqlDbType.Int).Value = iCountryID
                    dt = con.ExecuteDataTable(cmd)
                End Using
                If Cast.ToInt32(dt.Rows(0).Item("postcode_visibility_id"), 0) = 3 Then
                    If SAMFunc.NothingToString(oAddresses(iCount).PostCode) = "" Then
                        STSError.AddInvalidField("PostCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "PostCode"), "")
                    End If
                End If
            End If

            ' exit if there are any missing parameters
            If STSError.HasErrors Then
                Return STSError
            End If

            ''Inscrease array size from 8 to 14 for Additional_fields of address
            ReDim Preserve vAddresses(14, iCount)

            vAddresses(0, iCount) = CInt(oBaseAddress.AddressTypeCode)
            vAddresses(1, iCount) = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Address_Usage_Type", SAMFunc.GetABIAddressCode(oBaseAddress.AddressTypeCode))
            vAddresses(2, iCount) = SAMFunc.NothingToString(oBaseAddress.AddressLine1)
            vAddresses(3, iCount) = SAMFunc.NothingToString(oBaseAddress.AddressLine2)
            vAddresses(4, iCount) = SAMFunc.NothingToString(oBaseAddress.AddressLine3)
            vAddresses(5, iCount) = SAMFunc.NothingToString(oBaseAddress.AddressLine4)
            vAddresses(6, iCount) = oBaseAddress.CountryCode.ToString
            vAddresses(7, iCount) = SAMFunc.NothingToString(oBaseAddress.PostCode)
            vAddresses(8, iCount) = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Country", oBaseAddress.CountryCode.ToString)
            vAddresses(9, iCount) = SAMFunc.NothingToString(oBaseAddress.AddressLine5)
            vAddresses(10, iCount) = SAMFunc.NothingToString(oBaseAddress.AddressLine6)
            vAddresses(11, iCount) = SAMFunc.NothingToString(oBaseAddress.AddressLine7)
            vAddresses(12, iCount) = SAMFunc.NothingToString(oBaseAddress.AddressLine8)
            vAddresses(13, iCount) = SAMFunc.NothingToString(oBaseAddress.AddressLine9)
            vAddresses(14, iCount) = SAMFunc.NothingToString(oBaseAddress.AddressLine10)

        Next iCount

        r_vAddresses = vAddresses
        Return STSError

    End Function

    Private Function ProcessContacts(ByVal oContacts() As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseContactType, ByRef r_vContacts As Object, Optional ByVal lAddressCode As Integer = 0) As SiriusFS.SAM.Structure.STSErrorPublisher

        Dim iCount As Integer
        Dim oBaseContact As New BaseImplementationTypes.BaseContactType
        Dim STSError As New SiriusFS.SAM.Structure.STSErrorPublisher
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim vContacts As Object(,) = Nothing
        If oContacts IsNot Nothing Then
            Dim iCnt As Integer = 0
            For iCount = oContacts.GetLowerBound(0) To oContacts.GetUpperBound(0)

                If oContacts(iCount) IsNot Nothing AndAlso oContacts(iCount).ContactDetail IsNot Nothing Then

                If SAMFunc.NothingToString(oContacts(iCount).ContactDetail.Item) = "" Then
                    STSError.AddInvalidField("ContactItem", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "ContactItem"), "")
                End If

                ' exit if there are any missing parameters
                If STSError.HasErrors Then
                    Return STSError
                End If

                oBaseContact = oContacts(iCount)

                    ReDim Preserve vContacts(6, iCnt) ' Vivek: 20080704 - changed size to 5, originally it was 4
                    vContacts(0, iCnt) = CInt(oBaseContact.ContactTypeCode)
                If oBaseContact.ContactTypeCode = ContactTypeType.OTHER Then
                        vContacts(1, iCnt) = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Contact_Type", SAMFunc.GetPMContactTypeCode(oBaseContact.ContactTypeCode, oBaseContact.OtherContactTypeCode))
                Else
                        vContacts(1, iCnt) = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Contact_Type", SAMFunc.GetPMContactTypeCode(oBaseContact.ContactTypeCode))
                    End If
                    vContacts(2, iCnt) = SAMFunc.NothingToString(oBaseContact.ContactDetail.Item)
                    vContacts(3, iCnt) = SAMFunc.NothingToString(oBaseContact.AreaCode)
                    vContacts(4, iCnt) = lAddressCode
                    vContacts(5, iCnt) = oBaseContact.Description ' Vivek: 20080704 - added the missing item to fix bug in existing system (Amend Client)
                    vContacts(6, iCnt) = oBaseContact.Extension ' Sankar: 20080922 - added the missing field Extension
                    iCnt = iCnt + 1
                End If
            Next iCount

            r_vContacts = vContacts

        End If

        Return STSError

    End Function

    ' given by Rahul
    Private Sub ProcessProspect( _
            ByVal con As SiriusConnection, _
            ByVal oBusiness As CoreBusiness, _
            ByVal oProspect As BaseClientSharedDataType, _
            ByVal PartyKey As Integer)

        ' get existing party ProspectPolicies

        Dim oExistingProspect As BaseClientSharedDataType = GetPartyProspect(con, PartyKey)

        If oExistingProspect IsNot Nothing Then
            UpdatePartyProspect(con, oBusiness, oProspect, PartyKey)
        Else
            AddPartyProspect(con, oBusiness, oProspect, PartyKey)
        End If
    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and gets the party prospect

    '''</summary>
    '''<param name="lPartyCnt" type="Integer"></param>
    '''<remarks></remarks>

    'Amendeded this function implementation since the fields(used below) in BaseClientSharedDataType can hold a singe value 
    'and not the array of values for Particular fields used below.
    Private Function GetPartyProspect(ByVal con As SiriusConnection, _
      ByVal lPartyCnt As Integer) As BaseClientSharedDataType

        Dim dt As DataTable = Nothing

        ' get party  Prospect data
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_prospect_sel")

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = lPartyCnt

            dt = con.ExecuteDataTable(cmd)

        End Using

        ' if the party has had previous Prospects
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim oProspect As BaseClientSharedDataType = Nothing

            ' resize array
            'Array.Resize(oProspects, dt.Rows.Count)

            'Dim iProspect As Integer = 0

            ' for each previous Prospects
            For Each dr As DataRow In dt.Rows

                oProspect = New BaseClientSharedDataType

                oProspect.AgentReference = Cast.ToString(dr.Item("Agent_Reference"), String.Empty)
                oProspect.CurrentIntermediaryKey = Cast.ToInt32(dr.Item("Current_Intermediary"), 0)
                oProspect.CurrentIntermediaryKeySpecified = Not IsDBNull(dr.Item("Current_Intermediary"))

                Dim iStrengthCodeID As Integer = Cast.ToInt32(dr.Item("Strength_Code_ID"), 0)
                If iStrengthCodeID <> 0 Then
                    oProspect.StrengthCode = GetListCodeFromID(Core.STSListType.PMLookup, "Strength_Code", iStrengthCodeID)
                End If

                Dim iProspectStatusID As Integer = Cast.ToInt32(dr.Item("Prospect_Status_ID"), 0)
                If iProspectStatusID <> 0 Then
                    oProspect.StatusCode = GetListCodeFromID(Core.STSListType.PMLookup, "Prospect_Status", iProspectStatusID)
                End If

                oProspect.PreviousInsurerKey = Cast.ToInt32(dr.Item("Previous_Insurer_Cnt"), 0)
                oProspect.PreviousInsurerKeySpecified = Not IsDBNull(dr.Item("Previous_Insurer_Cnt"))
                oProspect.PreviousBrokerKey = Cast.ToInt32(dr.Item("Previous_Broker_Cnt"), 0)
                oProspect.PreviousBrokerKeySpecified = Not IsDBNull(dr.Item("Previous_Broker_Cnt"))

                oProspect.CurrentIntermediaryName = Cast.ToString(dr.Item("currentintermediary_resolved_name"), String.Empty)
                oProspect.PreviousInsurerCode = Cast.ToString(dr.Item("previous_insurer_shortname"), String.Empty)
                oProspect.PreviousInsurerName = Cast.ToString(dr.Item("previous_insurer_resolved_name"), String.Empty)
                oProspect.PreviousBrokerCode = Cast.ToString(dr.Item("previous_broker_shortname"), String.Empty)
                oProspect.PreviousBrokerName = Cast.ToString(dr.Item("previous_broker_resolved_name"), String.Empty)

                'oProspects(iProspect) = oProspect

                'iProspect += 1

            Next

            Return oProspect

        End If

        Return Nothing

    End Function

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and gets the the client account details

    '''</summary>
    '''<param name="lPartyCnt" type="Integer"></param>
    '''<remarks></remarks>
    Private Function GetClientAccountDetails(ByVal con As SiriusConnection, ByVal lBranchId As Integer, _
      ByVal sBranchCode As String, ByVal lPartyCnt As Integer) As BaseClientSharedDataType

        Dim dt As DataTable = Nothing
        Dim obusiness As New CoreBusiness

        Dim lIncludeTurnover As String
        Dim lIncludeincome As String
        lIncludeTurnover = String.Empty
        lIncludeincome = String.Empty

        'obusiness.GetSystemOption(sBranchCode, 5007, lIncludeTurnover) 'As said by Rahul
        'obusiness.GetSystemOption(sBranchCode, 5008, lIncludeincome) 'As said by Rahul
        lIncludeTurnover = "0"
        lIncludeincome = "0"

        ' get party  Prospect data
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Select_Client_Account_Details")

            cmd.AddInParameter("@account_key", SqlDbType.Int).Value = lPartyCnt
            cmd.AddInParameter("@company_id", SqlDbType.Int).Value = lBranchId
            cmd.AddInParameter("@include_tax_on_ytd_turnover", SqlDbType.Int).Value = Convert.ToInt32(lIncludeTurnover)
            cmd.AddInParameter("@include_tax_on_ytd_income", SqlDbType.Int).Value = Convert.ToInt32(lIncludeincome)

            dt = con.ExecuteDataTable(cmd)

        End Using

        ' if the party has had previous Prospects
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim oClientAccountDetails As New BaseClientSharedDataType

            oClientAccountDetails.YearToDateTurnover = Convert.ToDecimal(dt.Rows(0)(0).ToString())
            oClientAccountDetails.LastYearTurnover = Convert.ToDecimal(dt.Rows(0)(1).ToString())
            oClientAccountDetails.AccountBalance = Convert.ToDecimal(dt.Rows(0)(2).ToString())
            Return oClientAccountDetails

        End If

        Return Nothing

    End Function

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and updates the party prospect

    '''</summary>
    '''<param name="oProspect" type="BaseClientSharedDataType"></param>
    '''<remarks></remarks>
    Private Sub UpdatePartyProspect( _
        ByVal con As SiriusConnection, _
        ByVal oBusiness As CoreBusiness, _
        ByVal oProspect As BaseClientSharedDataType, _
        ByVal PartyKey As Integer)

        ' update Prospect details
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_prospect_upd")
            Dim lStrengthId, lProspectId As Integer

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@current_intermediary", SqlDbType.Int).Value = _
                IIf(oProspect.CurrentIntermediaryKeySpecified, oProspect.CurrentIntermediaryKey, SqlInt32.Null)
            cmd.AddInParameter("@agent_reference", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oProspect.AgentReference, String.Empty)
            cmd.AddInParameter("@previous_broker_cnt", SqlDbType.Int).Value = _
                IIf(oProspect.PreviousBrokerKeySpecified, oProspect.PreviousBrokerKey, SqlInt32.Null)
            If Not String.IsNullOrEmpty(oProspect.StrengthCode) Then
                lStrengthId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "strength_code", oProspect.StrengthCode, "Code")
            End If
            cmd.AddInParameter("@Strength_code_id", SqlDbType.Int).Value = Cast.NullIfDefault(lStrengthId, 0)
            cmd.AddInParameter("@previous_insurer_cnt", SqlDbType.Int).Value = _
                IIf(oProspect.PreviousInsurerKeySpecified, oProspect.PreviousInsurerKey, SqlInt32.Null)
            If Not String.IsNullOrEmpty(oProspect.StatusCode) Then
                lProspectId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "prospect_status", oProspect.StatusCode, "Code")
            End If
            cmd.AddInParameter("@prospect_status_id", SqlDbType.Int).Value = Cast.NullIfDefault(lProspectId, 0)

            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and adds the party prospect

    '''</summary>
    '''<param name="oProspect" type="BaseClientSharedDataType"></param>
    '''<remarks></remarks>
    Private Sub AddPartyProspect( _
        ByVal con As SiriusConnection, _
        ByVal oBusiness As CoreBusiness, _
        ByVal oProspect As BaseClientSharedDataType, _
        ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_party_prospect_add")

            Dim lStrengthId, lProspectId As Integer

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@current_intermediary", SqlDbType.Int).Value = _
                IIf(oProspect.CurrentIntermediaryKeySpecified, oProspect.CurrentIntermediaryKey, SqlInt32.Null)
            cmd.AddInParameter("@agent_reference", SqlDbType.VarChar, 70).Value = Cast.NullIfDefault(oProspect.AgentReference, String.Empty)
            cmd.AddInParameter("@previous_broker_cnt", SqlDbType.Int).Value = _
                IIf(oProspect.PreviousBrokerKeySpecified, oProspect.PreviousBrokerKey, SqlInt32.Null)

            If Not String.IsNullOrEmpty(oProspect.StrengthCode) Then
                lStrengthId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "strength_code", oProspect.StrengthCode, "Code")
            End If
            cmd.AddInParameter("@Strength_code_id", SqlDbType.Int, 70).Value = Cast.NullIfDefault(lStrengthId, 0)

            cmd.AddInParameter("@previous_insurer_cnt", SqlDbType.Int).Value = _
                IIf(oProspect.PreviousInsurerKeySpecified, oProspect.PreviousInsurerKey, SqlInt32.Null)

            If Not String.IsNullOrEmpty(oProspect.StatusCode) Then
                lProspectId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "prospect_status", oProspect.StatusCode, "Code")
            End If
            cmd.AddInParameter("@prospect_status_id", SqlDbType.Int, 70).Value = Cast.NullIfDefault(lProspectId, 0)

            con.ExecuteNonQuery(cmd)

        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and updates the partyccdetails
    '''</summary>
    '''<param name="oPartyCCType" type="BasePartyCCType"></param>
    '''<remarks></remarks>
    Private Sub UpdatePartyCCDetails( _
        ByVal con As SiriusConnection, _
        ByVal oBusiness As CoreBusiness, _
        ByVal oPartyCCType As BasePartyCCType, _
        ByVal PartyKey As Integer, Optional ByVal oUpdatePartyAddDetails As Hashtable = Nothing)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_update_partyCCdetails")

            Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection

            cmd.AddInParameter("@CompanyReg", SqlDbType.VarChar, 60).Value = oPartyCCType.CompanyReg
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            cmd.AddInParameter("@TradeCode", SqlDbType.VarChar, 70).Value = oPartyCCType.TradeCode
            If String.IsNullOrEmpty(oPartyCCType.SICCode) Then
                cmd.AddInParameter("@SICCode", SqlDbType.Int).Value = SqlInt32.Null
            Else
                cmd.AddInParameter("@SICCode", SqlDbType.Int).Value = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.SICCode, oPartyCCType.SICCode, "Code", oSAMErrorCollection)
            End If

            'cmd.AddInParameter("@TurnoverCode", SqlDbType.Int).Value = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.TurnoverBand, oPartyCCType.TurnoverCode, "Code", oSAMErrorCollection)
            If String.IsNullOrEmpty(oPartyCCType.TurnoverCode) Then
                cmd.AddInParameter("@TurnoverCode", SqlDbType.Int, 19, 4).Value = SqlDecimal.Null
            Else
                cmd.AddInParameter("@TurnoverCode", SqlDbType.Int, 19, 4).Value = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "TurnoverBand", oPartyCCType.TurnoverCode, "Code", oSAMErrorCollection)
            End If

            cmd.AddInParameter("@Salutation", SqlDbType.VarChar, 255).Value = oPartyCCType.Salutation

            If oPartyCCType.TradingSinceSpecified Then
                cmd.AddInParameter("@TradingSince", SqlDbType.DateTime).Value = oPartyCCType.TradingSince
            Else
                cmd.AddInParameter("@TradingSince", SqlDbType.DateTime).Value = SqlDateTime.Null
            End If
            If oPartyCCType.WageRollSpecified Then
                cmd.AddInParameter("@WageRoll", SqlDbType.Decimal).Value = oPartyCCType.WageRoll
            Else

                oPartyCCType.WageRoll = 0D
                cmd.AddInParameter("@WageRoll", SqlDbType.Decimal).Value = oPartyCCType.WageRoll 'Cast.DefaultIfNull(oPartyCCType.WageRoll, 0.0)

            End If
            If oPartyCCType.FinancialYearSpecified Then
                cmd.AddInParameter("@FinancialYear", SqlDbType.DateTime).Value = oPartyCCType.FinancialYear
            Else
                cmd.AddInParameter("@FinancialYear", SqlDbType.DateTime).Value = SqlDateTime.Null
            End If
            If oPartyCCType.TPSSpecified Then
                If oPartyCCType.TPS = True Then
                    cmd.AddInParameter("@TPS", SqlDbType.TinyInt).Value = 1
                Else
                    cmd.AddInParameter("@TPS", SqlDbType.TinyInt).Value = 0
                End If

            Else
                cmd.AddInParameter("@TPS", SqlDbType.TinyInt).Value = SqlInt16.Null
            End If

            If oPartyCCType.MPSSpecified Then
                If oPartyCCType.MPS = True Then
                    cmd.AddInParameter("@MPS", SqlDbType.TinyInt).Value = 1
                Else
                    cmd.AddInParameter("@MPS", SqlDbType.TinyInt).Value = 0
                End If
            Else
                cmd.AddInParameter("@MPS", SqlDbType.TinyInt).Value = SqlInt16.Null
            End If

            If oPartyCCType.eMPSSpecified Then
                If oPartyCCType.eMPS = True Then
                    cmd.AddInParameter("@eMPS", SqlDbType.TinyInt).Value = 1
                Else
                    cmd.AddInParameter("@eMPS", SqlDbType.TinyInt).Value = 0
                End If
            Else
                cmd.AddInParameter("@eMPS", SqlDbType.TinyInt).Value = SqlInt16.Null
            End If

            cmd.AddInParameter("@Source", SqlDbType.VarChar, 255).Value = oPartyCCType.Source
            'Added on 17-09-08 as per discussion with Rahul
            If (oPartyCCType.NumberOfOfficesSpecified) Then
                cmd.AddInParameter("@no_of_offices", SqlDbType.Int).Value = oPartyCCType.NumberOfOffices
            End If
            If (oUpdatePartyAddDetails IsNot Nothing) Then
                cmd.AddInParameter("@employeeband_id", SqlDbType.Int).Value = Cast.NullIfDefault(oUpdatePartyAddDetails.Item("employeeband_id"), 0)
            End If

            If Not String.IsNullOrEmpty(oPartyCCType.CompanyName) Then
                cmd.AddInParameter("@Company_Name", SqlDbType.VarChar, 255).Value = oPartyCCType.CompanyName
            End If

            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and updates the partypcdetails
    '''</summary>
    '''<param name="oPartyPCType" type="BasePartyPCType"></param>
    '''<remarks></remarks>
    Private Sub UpdatePartyPCDetails( _
        ByVal con As SiriusConnection, _
        ByVal oBusiness As CoreBusiness, _
        ByVal oPartyPCType As BasePartyPCType, _
        ByVal PartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_update_partyPCdetails")

            Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection

            cmd.AddInParameter("@SecEmployersBusinessCode", SqlDbType.VarChar, 70).Value = oPartyPCType.SecEmployersBusinessCode
            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
            If String.IsNullOrEmpty(oPartyPCType.NationalityCode) Then
                cmd.AddInParameter("@NationalityCode", SqlDbType.Int).Value = SqlInt32.Null
            Else
                cmd.AddInParameter("@NationalityCode", SqlDbType.Int).Value = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, "Nationality", oPartyPCType.NationalityCode, "Code", oSAMErrorCollection)
            End If
            cmd.AddInParameter("@AccommodationCode", SqlDbType.VarChar, 70).Value = oPartyPCType.AccommodationCode
            cmd.AddInParameter("@Trading_Name", SqlDbType.VarChar, 255).Value = oPartyPCType.TradingName
            cmd.AddInParameter("@Salutation", SqlDbType.VarChar, 255).Value = oPartyPCType.Salutation

            ' Got the following values from Gaurav for Secondary Employment Status Code
            '2228230    Company                            C
            '2228230    Employed                           E
            '2228230    Household Duties                   H
            '2228230    In Full Or Part Time Education     F
            '2228230    Independent Means                  I
            '2228230    Not Employed Due To Disability     N
            '2228230    Retired                            R
            '2228230    Self Employed                      S
            '2228230    Unemployed                         U
            '2228230    Voluntary Work                     V
            If oPartyPCType.SecEmploymentStatusCodeSpecified Then
                Select Case oPartyPCType.SecEmploymentStatusCode
                    Case EmploymentStatusCodeType.Company
                        cmd.AddInParameter("@SecEmploymentStatusCode", SqlDbType.VarChar, 70).Value = "Company"
                    Case EmploymentStatusCodeType.Employed
                        cmd.AddInParameter("@SecEmploymentStatusCode", SqlDbType.VarChar, 70).Value = "Employed"
                    Case EmploymentStatusCodeType.HouseholdDuties
                        cmd.AddInParameter("@SecEmploymentStatusCode", SqlDbType.VarChar, 70).Value = "Household Duties"
                    Case EmploymentStatusCodeType.IndependentMeans
                        cmd.AddInParameter("@SecEmploymentStatusCode", SqlDbType.VarChar, 70).Value = "Independent Means"
                    Case EmploymentStatusCodeType.InFullOrPartTimeEducation
                        cmd.AddInParameter("@SecEmploymentStatusCode", SqlDbType.VarChar, 70).Value = "In Full Or Part Time Education"
                    Case EmploymentStatusCodeType.NotEmployedDueToDisability
                        cmd.AddInParameter("@SecEmploymentStatusCode", SqlDbType.VarChar, 70).Value = "Not Employed Due To Disability"
                    Case EmploymentStatusCodeType.Retired
                        cmd.AddInParameter("@SecEmploymentStatusCode", SqlDbType.VarChar, 70).Value = "Retired"
                    Case EmploymentStatusCodeType.SelfEmployed
                        cmd.AddInParameter("@SecEmploymentStatusCode", SqlDbType.VarChar, 70).Value = "Self Employed"
                    Case EmploymentStatusCodeType.Unemployed
                        cmd.AddInParameter("@SecEmploymentStatusCode", SqlDbType.VarChar, 70).Value = "Unemployed"
                    Case EmploymentStatusCodeType.VoluntaryWork
                        cmd.AddInParameter("@SecEmploymentStatusCode", SqlDbType.VarChar, 70).Value = "Voluntary Work"
                    Case Else
                        cmd.AddInParameter("@SecEmploymentStatusCode", SqlDbType.VarChar, 70).Value = SqlString.Null
                End Select
            Else
                cmd.AddInParameter("@SecEmploymentStatusCode", SqlDbType.VarChar, 70).Value = SqlString.Null
            End If

            If oPartyPCType.TPSSpecified Then
                If oPartyPCType.TPS = True Then
                    cmd.AddInParameter("@TPS", SqlDbType.TinyInt).Value = 1
                Else
                    cmd.AddInParameter("@TPS", SqlDbType.TinyInt).Value = 0
                End If

            Else
                cmd.AddInParameter("@TPS", SqlDbType.TinyInt).Value = SqlInt16.Null
            End If

            If oPartyPCType.MPSSpecified Then
                If oPartyPCType.MPS = True Then
                    cmd.AddInParameter("@MPS", SqlDbType.TinyInt).Value = 1
                Else
                    cmd.AddInParameter("@MPS", SqlDbType.TinyInt).Value = 0
                End If
            Else
                cmd.AddInParameter("@MPS", SqlDbType.TinyInt).Value = SqlInt16.Null
            End If

            If oPartyPCType.eMPSSpecified Then
                If oPartyPCType.eMPS = True Then
                    cmd.AddInParameter("@eMPS", SqlDbType.TinyInt).Value = 1
                Else
                    cmd.AddInParameter("@eMPS", SqlDbType.TinyInt).Value = 0
                End If
            Else
                cmd.AddInParameter("@eMPS", SqlDbType.TinyInt).Value = SqlInt16.Null
            End If

            If oPartyPCType.PetOwnerSpecified Then
                If oPartyPCType.PetOwner = True Then
                    cmd.AddInParameter("@PetOwner", SqlDbType.TinyInt).Value = 1
                Else
                    cmd.AddInParameter("@PetOwner", SqlDbType.TinyInt).Value = 0
                End If
            Else
                cmd.AddInParameter("@PetOwner", SqlDbType.TinyInt).Value = SqlInt16.Null
            End If

            cmd.AddInParameter("@Source", SqlDbType.VarChar, 255).Value = oPartyPCType.Source

            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    ''' <summary>
    '''This method sends the request object values to the Stored Procedure and updates the party details
    '''</summary>
    '''<param name="oSharedDataType" type="BaseClientSharedDataType"></param>
    '''<remarks></remarks>
    Private Sub UpdatePartyDetails( _
        ByVal con As SiriusConnection, _
        ByVal oBusiness As CoreBusiness, _
        ByVal oSharedDataType As BaseClientSharedDataType, _
        ByVal PartyKey As Integer, Optional ByVal oUpdatePartyAddDetails As Hashtable = Nothing)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Update_party")

            Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection

            If String.IsNullOrEmpty(oSharedDataType.ServiceLevelCode) Then
                cmd.AddInParameter("@ServiceLevelId", SqlDbType.Int).Value = SqlInt32.Null
            Else
                cmd.AddInParameter("@ServiceLevelId", SqlDbType.Int).Value = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.ServiceLevel, oSharedDataType.ServiceLevelCode, "Code", oSAMErrorCollection)
            End If

            cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey

            If String.IsNullOrEmpty(oSharedDataType.AreaCode) Then
                cmd.AddInParameter("@AreaId", SqlDbType.Int).Value = SqlInt32.Null
            Else
                cmd.AddInParameter("@AreaId", SqlDbType.Int).Value = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Area, oSharedDataType.AreaCode, "Code", oSAMErrorCollection)
            End If

            If oSharedDataType.LeadAgentKeySpecified Then
                cmd.AddInParameter("@AgentCnt", SqlDbType.Int).Value = oSharedDataType.LeadAgentKey
            Else
                cmd.AddInParameter("@AgentCnt", SqlDbType.Int).Value = SqlInt32.Null
            End If

            If oSharedDataType.IsProspectSpecified Then
                If oSharedDataType.IsProspect = True Then
                    cmd.AddInParameter("@IsProspect", SqlDbType.TinyInt).Value = 1
                Else
                    cmd.AddInParameter("@IsProspect", SqlDbType.TinyInt).Value = 0
                End If
            Else

                cmd.AddInParameter("@IsProspect", SqlDbType.TinyInt).Value = 0

            End If

            If oSharedDataType.IsAgentSpecified Then
                If oSharedDataType.IsAgent = True Then
                    cmd.AddInParameter("@IsAlsoAgent", SqlDbType.TinyInt).Value = 1
                Else
                    cmd.AddInParameter("@IsAlsoAgent", SqlDbType.TinyInt).Value = 0
                End If
            Else
                cmd.AddInParameter("@IsAlsoAgent", SqlDbType.TinyInt).Value = 0
            End If

            If String.IsNullOrEmpty(oSharedDataType.CorrespondenceCode) Then
                cmd.AddInParameter("@CorrespondenceTypeId", SqlDbType.Int).Value = SqlInt32.Null
            Else
                cmd.AddInParameter("@CorrespondenceTypeId", SqlDbType.Int).Value = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.ContactType, oSharedDataType.CorrespondenceCode, "Code", oSAMErrorCollection)
            End If

            cmd.AddInParameter("@PaymentMethodCode", SqlDbType.VarChar, 70).Value = oSharedDataType.PaymentCode

            If String.IsNullOrEmpty(oSharedDataType.ReminderCode) Then
                cmd.AddInParameter("@ReminderTypeId", SqlDbType.Int).Value = SqlInt32.Null
            Else
                cmd.AddInParameter("@ReminderTypeId", SqlDbType.Int).Value = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.ReminderType, oSharedDataType.ReminderCode, "Code", oSAMErrorCollection)
            End If

            cmd.AddInParameter("@PaymentTermCode", SqlDbType.VarChar, 70).Value = oSharedDataType.PaymentTermCode

            If String.IsNullOrEmpty(oSharedDataType.RenewalStopCode) Then
                cmd.AddInParameter("@RenewalStopCodeId", SqlDbType.Int).Value = SqlInt32.Null
            Else
                cmd.AddInParameter("@RenewalStopCodeId", SqlDbType.Int).Value = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.RenewalStopCode, oSharedDataType.RenewalStopCode, "Code", oSAMErrorCollection)
            End If

            cmd.AddInParameter("@LoyaltyNumber", SqlDbType.VarChar, 20).Value = oSharedDataType.LoyaltyNumber

            If String.IsNullOrEmpty(oSharedDataType.SeasonalGiftCode) Then
                cmd.AddInParameter("@SeasonalGiftId", SqlDbType.Int).Value = SqlInt32.Null
            Else
                cmd.AddInParameter("@SeasonalGiftId", SqlDbType.Int).Value = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.SeasonalGift, oSharedDataType.SeasonalGiftCode, "Code", oSAMErrorCollection)
            End If

            If oSharedDataType.CountyCourtJudgmentsSpecified Then
                ' Vivek: as per Tech Spec we have defined this field as Deciaml
                ' but in DB its INT. So using the conversion function here.
                cmd.AddInParameter("@CCJs", SqlDbType.Int).Value = Convert.ToInt32(oSharedDataType.CountyCourtJudgments)
            Else
                '20080525
                cmd.AddInParameter("@CCJs", SqlDbType.Int).Value = SqlInt32.Null
            End If
            If (oUpdatePartyAddDetails IsNot Nothing) Then
                cmd.AddInParameter("@currency_id", SqlDbType.Int).Value = Cast.NullIfDefault(oUpdatePartyAddDetails.Item("currency_id"), 0)
                cmd.AddInParameter("@consultant_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(oUpdatePartyAddDetails.Item("consultant_cnt"), 0)
                cmd.AddInParameter("@source_id", SqlDbType.Int).Value = Cast.NullIfDefault(oUpdatePartyAddDetails.Item("source_id"), 0)
                cmd.AddInParameter("@sub_branch_id", SqlDbType.Int).Value = Cast.NullIfDefault(oUpdatePartyAddDetails.Item("sub_branch_id"), 0)
            End If
            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    Private Sub UpdatePartyAccountAddressDetails(ByVal con As SiriusConnection, ByVal sAddress1 As String, ByVal sAddress2 As String, _
                                                       ByVal sAddress3 As String, ByVal sAddress4 As String, ByVal sPostcode As String, _
                                                       ByVal sCountry As String, ByVal nPartyKey As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Update_party_Account_Address")
            cmd.AddInParameter("@account_key", SqlDbType.Int).Value = nPartyKey
            cmd.AddInParameter("@address1", SqlDbType.VarChar).Value = sAddress1
            cmd.AddInParameter("@address2", SqlDbType.VarChar).Value = sAddress2
            cmd.AddInParameter("@address3", SqlDbType.VarChar).Value = sAddress3
            cmd.AddInParameter("@address4", SqlDbType.VarChar).Value = sAddress4
            cmd.AddInParameter("@postal_code", SqlDbType.VarChar).Value = sPostcode
            cmd.AddInParameter("@address_country", SqlDbType.VarChar).Value = sCountry

            con.ExecuteNonQuery(cmd)
        End Using

    End Sub

    ' given by Gaurav
    Private Sub GetPartyAdditionalData(ByRef oRequest As BaseGetPartyRequestType, _
        ByRef oResponse As BaseGetPartyResponseType, _
        ByVal sPartyType As String)

        If sPartyType = "PC" Then
            'Get Associate Details
            'Get Prospect Details
            'Get Lifestyle Details
            'Get Party Loyality
            'Get Party Conviction

            Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

                Dim oBasePartyPC As BaseImplementationTypes.BasePartyPCType
                oBasePartyPC = DirectCast(oResponse.Item, BaseImplementationTypes.BasePartyPCType)

                If oBasePartyPC.ClientDetail Is Nothing Then
                    oBasePartyPC.ClientDetail = New BaseClientSharedDataType
                End If

                Dim oLifeStyle As BasePartyPCTypeLifestyle() = GetPartyLifeStyle(con, oRequest.PartyKey)
                If Not oLifeStyle Is Nothing Then oBasePartyPC.Lifestyle = oLifeStyle

                Dim oAssociates As BaseImplementationTypes.BaseAssociateType() = GetPartyAssociates(con, oRequest.PartyKey)
                If Not oAssociates Is Nothing Then oBasePartyPC.ClientDetail.Associates = oAssociates

                Dim oProspectPolicies As BaseClientSharedDataTypeProspectPolicies()
                oProspectPolicies = GetPartyProspectPolicies(con, oRequest.PartyKey)
                If Not oProspectPolicies Is Nothing Then oBasePartyPC.ClientDetail.ProspectPolicies = oProspectPolicies

                Dim oLoyality As BaseClientSharedDataTypeLoyaltyScheme() = GetPartyLoyaltyScheme(con, oRequest.PartyKey)
                If Not oLoyality Is Nothing Then oBasePartyPC.ClientDetail.LoyaltyScheme = oLoyality

                Dim oConviction As BaseConvictionType() = GetPartyConvictions_v2(con, oRequest.PartyKey)
                If Not oConviction Is Nothing Then oBasePartyPC.ClientDetail.Convictions() = oConviction

                oResponse.Item = oBasePartyPC
            End Using
        ElseIf sPartyType = "CC" Then
            Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

                Dim oBasePartyCC As BaseImplementationTypes.BasePartyCCType
                oBasePartyCC = DirectCast(oResponse.Item, BaseImplementationTypes.BasePartyCCType)

                If oBasePartyCC.ClientDetail Is Nothing Then
                    oBasePartyCC.ClientDetail = New BaseClientSharedDataType
                End If

                Dim oAssociates As BaseImplementationTypes.BaseAssociateType() = GetPartyAssociates(con, oRequest.PartyKey)
                If Not oAssociates Is Nothing Then oBasePartyCC.ClientDetail.Associates = oAssociates

                Dim oProspectPolicies As BaseClientSharedDataTypeProspectPolicies()
                oProspectPolicies = GetPartyProspectPolicies(con, oRequest.PartyKey)
                If Not oProspectPolicies Is Nothing Then oBasePartyCC.ClientDetail.ProspectPolicies = oProspectPolicies

                Dim oLoyality As BaseClientSharedDataTypeLoyaltyScheme() = GetPartyLoyaltyScheme(con, oRequest.PartyKey)
                If Not oLoyality Is Nothing Then oBasePartyCC.ClientDetail.LoyaltyScheme = oLoyality

                Dim oConviction As BaseConvictionType() = GetPartyConvictions_v2(con, oRequest.PartyKey)
                If Not oConviction Is Nothing Then oBasePartyCC.ClientDetail.Convictions() = oConviction

                oResponse.Item = oBasePartyCC
            End Using
        End If
    End Sub

    ' Vivek: missing in Tech Spec
    Private Sub GetPartyDetails( _
        ByVal dr As DataSet, _
        ByVal oCoreBusiness As CoreBusiness, _
        ByVal oSharedDataType As BaseClientSharedDataType)

        Dim iServiceLevelId As Integer = Cast.ToInt32(dr.Tables(0).Rows(0).Item("Service_Level_ID"), 0)
        If iServiceLevelId <> 0 Then
            oSharedDataType.ServiceLevelCode = oCoreBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.ServiceLevel, iServiceLevelId)
        End If

        Dim iAreaID As Integer = Cast.ToInt32(dr.Tables(0).Rows(0).Item("Area_ID"), 0)
        If iAreaID <> 0 Then
            oSharedDataType.AreaCode = oCoreBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.Area, iAreaID)
        End If

        Dim iCorrespondenceTypeID As Integer = Cast.ToInt32(dr.Tables(0).Rows(0).Item("Correspondence_Type_ID"), 0)
        If iCorrespondenceTypeID <> 0 Then
            oSharedDataType.CorrespondenceCode = oCoreBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.ContactType, iCorrespondenceTypeID)
        End If

        oSharedDataType.PaymentCode = Cast.ToString(dr.Tables(0).Rows(0).Item("Payment_Method_Code"))

        Dim iReminderTypeID As Integer = Cast.ToInt32(dr.Tables(0).Rows(0).Item("Reminder_Type_ID"), 0)
        If iReminderTypeID <> 0 Then
            oSharedDataType.ReminderCode = oCoreBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.ReminderType, iReminderTypeID)
        End If

        oSharedDataType.PaymentTermCode = ToSafeString(dr.Tables(0).Rows(0).Item("Payment_Term_Code"))

        Dim iRenewalStopCodeID As Integer = Cast.ToInt32(dr.Tables(0).Rows(0).Item("Renewal_Stop_Code_ID"), 0)
        If iRenewalStopCodeID <> 0 Then
            oSharedDataType.RenewalStopCode = oCoreBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.RenewalStopCode, iRenewalStopCodeID)
        End If

        oSharedDataType.LoyaltyNumber = Cast.ToString(dr.Tables(0).Rows(0).Item("Loyalty_Number"))

        Dim iSeasonalGiftID As Integer = Cast.ToInt32(dr.Tables(0).Rows(0).Item("Seasonal_Gift_ID"), 0)
        If iSeasonalGiftID <> 0 Then
            oSharedDataType.SeasonalGiftCode = oCoreBusiness.GetListItemFromID(Core.STSListType.PMLookup, PMLookupTable.SeasonalGift, iSeasonalGiftID)
        End If

        oSharedDataType.CountyCourtJudgments = Cast.ToInt32(dr.Tables(0).Rows(0).Item("CCJs"), 0)
        oSharedDataType.CountyCourtJudgmentsSpecified = Not IsDBNull(dr.Tables(0).Rows(0).Item("CCJs"))

        oSharedDataType.LeadAgentKey = Cast.ToInt32(dr.Tables(0).Rows(0).Item("Agent_Cnt"), 0)
        oSharedDataType.LeadAgentKeySpecified = Not IsDBNull(dr.Tables(0).Rows(0).Item("Agent_Cnt"))

        oSharedDataType.IsProspect = Cast.ToBoolean(dr.Tables(0).Rows(0).Item("Is_Prospect"), False)
        oSharedDataType.IsProspectSpecified = Not IsDBNull(dr.Tables(0).Rows(0).Item("Is_Prospect"))

        oSharedDataType.IsAgent = Cast.ToBoolean(dr.Tables(0).Rows(0).Item("Is_Also_Agent"), False)
        oSharedDataType.IsAgentSpecified = Not IsDBNull(dr.Tables(0).Rows(0).Item("Is_Also_Agent"))

        oSharedDataType.LeadAgentCode = Cast.ToString(dr.Tables(0).Rows(0).Item("LeadAgent_shortname"))
        oSharedDataType.LeadAgentName = Cast.ToString(dr.Tables(0).Rows(0).Item("LeadAgent_resolved_name"))

    End Sub

    Public Sub GetValidSources(ByVal con As SiriusConnection, ByRef oSources As Object(,), ByVal bIncludeClosed As Boolean)
        Dim dt As DataTable = Nothing
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_pm_get_user_sources")

            cmd.AddInParameter("@UserID", SqlDbType.Int).Value = _SiriusUser.UserID
            cmd.AddInParameter("@IncludeClosed", SqlDbType.TinyInt).Value = Cast.ToInt16(bIncludeClosed)

            dt = con.ExecuteDataTable(cmd)

        End Using

        ' if the user has available sources 

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim iSource As Integer = 0

            ReDim oSources(0 To 3, dt.Rows.Count - 1)
            ' for each source
            For Each dr As DataRow In dt.Rows
                ' populate the response type
                oSources(0, iSource) = Cast.ToInt32(dr.Item("source_id"), 0)
                oSources(1, iSource) = Cast.ToString(dr.Item("code"), String.Empty)
                oSources(2, iSource) = Cast.ToString(dr.Item("description"), String.Empty)
                oSources(3, iSource) = Cast.ToInt32(dr.Item("country_id"), 0)
                iSource += 1

            Next
        End If
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub UpdatePartyMainDetails(ByVal oParty As BasePartyCCType, ByVal PartyKey As Integer)
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_updateparty_via_messaging")
                cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
                cmd.AddInParameter("@trading_name", SqlDbType.VarChar, 255).Value = oParty.CompanyName
                cmd.AddInParameter("@main_contact", SqlDbType.VarChar, 255).Value = oParty.MainContact
                con.ExecuteNonQuery(cmd)
            End Using
        End Using
    End Sub

    Public Sub UpdatePartyPCMainDetails(ByVal oParty As BasePartyPCType, ByVal PartyKey As Integer)
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_updateparty_PC_via_messaging")
                cmd.AddInParameter("@party_cnt", SqlDbType.Int).Value = PartyKey
                cmd.AddInParameter("@Initials", SqlDbType.VarChar, 255).Value = oParty.Initials
                cmd.AddInParameter("@Forename", SqlDbType.VarChar, 255).Value = oParty.Forename
                cmd.AddInParameter("@Surname", SqlDbType.VarChar, 255).Value = oParty.Surname
                cmd.AddInParameter("@Title", SqlDbType.VarChar, 255).Value = oParty.Title
                con.ExecuteNonQuery(cmd)
            End Using
        End Using
    End Sub

    Public Overloads Function FindReinsurer(ByVal oFindReinsurerRequest As BaseFindReinsurerRequestType) As BaseFindReinsurerResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                           _SiriusUser.Username, _SiriusUser.SourceID, _
                                           _SiriusUser.LanguageID, _
                                           SiriusUserDefaults.AppName)

            Dim oResponse As BaseFindReinsurerResponseType

            oResponse = FindReinsurer(con, oFindReinsurerRequest)

            Return oResponse

        End Using

    End Function

    Public Overloads Function FindReinsurer(ByVal con As SiriusConnection, ByVal FindReinsurerRequest As BaseFindReinsurerRequestType) As BaseFindReinsurerResponseType

        Dim oFindReinsurerResponse As New BaseFindReinsurerResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection

        Dim oSources As Object(,) = Nothing
        Dim sSourcesString As String

        Dim iSourceId As Integer
        Dim nTypeOfPackage As enumTypeOfPackage

        Dim sDisableWildcardSearchOption As String
        Dim bEnablePartialWildcardSearchOption As Boolean
        Dim bDisableWildcardSearchOption As Boolean
        Dim sEnablePartialWildcardSearchOption As String

        If FindReinsurerRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.FindReinsurerRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oFindReinsurerResponse = New SAMForInsuranceV2ImplementationTypes.FindReinsurerResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' Check mandatory fields
        FindReinsurerRequest.Validate(CType(oSAMErrorCollection, Object))
        oSAMErrorCollection.CheckForErrors()

        oCoreBusiness.GetSystemOption(FindReinsurerRequest.BranchCode, SystemOption.DisableWildcardSearch, sDisableWildcardSearchOption)
        oCoreBusiness.GetSystemOption(FindReinsurerRequest.BranchCode, SystemOption.EnablePartialWildcardSearch, sEnablePartialWildcardSearchOption)
        If (Trim(sDisableWildcardSearchOption) = "1") Then
            bDisableWildcardSearchOption = True
        Else
            bDisableWildcardSearchOption = False
        End If
        If (Trim(sEnablePartialWildcardSearchOption) = "1") Then
            bEnablePartialWildcardSearchOption = True
        Else
            bEnablePartialWildcardSearchOption = False
        End If

        If Not oCoreBusiness.ValidWildcardSearch( _
               bDisableWildcardSearchOption, _
               bEnablePartialWildcardSearchOption, _
               FindReinsurerRequest.RICode) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     SAMInvalidData.InvalidWildcardSearch.ToString, _
                                     "RICode")
        End If
        oSAMErrorCollection.CheckForErrors()

        If Not oCoreBusiness.ValidWildcardSearch( _
              bDisableWildcardSearchOption, _
              bEnablePartialWildcardSearchOption, _
              FindReinsurerRequest.RIName) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     SAMInvalidData.InvalidWildcardSearch.ToString, _
                                     "RIName")
        End If
        oSAMErrorCollection.CheckForErrors()


        If Not oCoreBusiness.ValidWildcardSearch( _
              bDisableWildcardSearchOption, _
              bEnablePartialWildcardSearchOption, _
              FindReinsurerRequest.FileCode) Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.InvalidWildcardSearch, _
                                     SAMInvalidData.InvalidWildcardSearch.ToString, _
                                     "FileCode")
        End If
        oSAMErrorCollection.CheckForErrors()

        GetValidSources(con, oSources, FindReinsurerRequest.IncludeClosedBranches)

        For iSourceId = 0 To UBound(oSources, 2)
            If iSourceId = 0 Then
                sSourcesString = oSources(0, iSourceId).ToString
            Else
                sSourcesString = sSourcesString & "," & oSources(0, iSourceId).ToString
            End If
        Next

        Dim dsReinsurers As DataSet = Nothing
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("Spu_Sir_GetPartyInsurerDetails")
            If String.IsNullOrEmpty(sSourcesString) = False Then
                cmd.AddInParameter("@Branch", SqlDbType.VarChar, 100).Value = sSourcesString
            End If
            If String.IsNullOrEmpty(FindReinsurerRequest.FileCode) = False Then
                cmd.AddInParameter("@FileCode", SqlDbType.VarChar, 10).Value = FindReinsurerRequest.FileCode
            End If
            If String.IsNullOrEmpty(FindReinsurerRequest.RICode) = False Then
                cmd.AddInParameter("@ShortName", SqlDbType.VarChar, 20).Value = FindReinsurerRequest.RICode
            End If
            If String.IsNullOrEmpty(FindReinsurerRequest.RIName) = False Then
                cmd.AddInParameter("@Name", SqlDbType.VarChar, 255).Value = FindReinsurerRequest.RIName
            End If
            If FindReinsurerRequest.IsBrokerSpecified Then
                cmd.AddInParameter("@is_ri_broker", SqlDbType.Int).Value = IIf(FindReinsurerRequest.IsBroker, 1, 0)
            End If
            If FindReinsurerRequest.IsFAXSpecified Then
                cmd.AddInParameter("@Is_FAX", SqlDbType.Int).Value = IIf(FindReinsurerRequest.IsFAX, 1, 0)
            End If
            If String.IsNullOrEmpty(FindReinsurerRequest.RITypeCode) = False Then
                cmd.AddInParameter("@ri_type_code", SqlDbType.VarChar, 20).Value = FindReinsurerRequest.RITypeCode
            End If
            If FindReinsurerRequest.IsRetainedSpecified Then
                cmd.AddInParameter("@is_retained", SqlDbType.Int).Value = IIf(FindReinsurerRequest.IsRetained, 1, 0)
            End If
            dsReinsurers = con.ExecuteDataSet(cmd, "Row")
        End Using

        If (dsReinsurers.Tables.Count > 0) Then

            dsReinsurers.Tables(0).Columns(0).ColumnName = "ReinsurerCode"
            dsReinsurers.Tables(0).Columns(1).ColumnName = "RIName"
            dsReinsurers.Tables(0).Columns(2).ColumnName = "AccountType"
            dsReinsurers.Tables(0).Columns(3).ColumnName = "Address1"
            dsReinsurers.Tables(0).Columns(4).ColumnName = "Address2"
            dsReinsurers.Tables(0).Columns(5).ColumnName = "PostalCode"
            dsReinsurers.Tables(0).Columns(17).ColumnName = "BranchCode"
            dsReinsurers.Tables(0).Columns(9).ColumnName = "BranchName"
            dsReinsurers.Tables(0).Columns(10).ColumnName = "ReinsurerKey"
            dsReinsurers.Tables(0).Columns(16).ColumnName = "ReinsuranceTypeCode"
            dsReinsurers.Tables(0).Columns(15).ColumnName = "IsRetained"
            dsReinsurers.Tables(0).Columns(14).ColumnName = "IsBroker"
            dsReinsurers.Tables(0).Columns(21).ColumnName = "TaxNumber"
            dsReinsurers.Tables(0).Columns(13).ColumnName = "IsDomiciledForTax"
            dsReinsurers.Tables(0).Columns(18).ColumnName = "IsTaxExempt"
            dsReinsurers.Tables(0).Columns(19).ColumnName = "TaxPercentage"
            dsReinsurers.Tables(0).Columns(22).ColumnName = "TaxGroupCode"
            dsReinsurers.Tables(0).Columns(12).ColumnName = "DefaultCommissionPercentage"

            dsReinsurers.Tables(0).Columns.RemoveAt(20)
            dsReinsurers.Tables(0).Columns.RemoveAt(11)
            dsReinsurers.Tables(0).Columns.RemoveAt(8)
            dsReinsurers.Tables(0).Columns.RemoveAt(7)
            dsReinsurers.Tables(0).Columns.RemoveAt(6)
            dsReinsurers.Tables(0).Columns.Add("TaxPercentageSpecified", GetType(System.Boolean))

            For icount As Integer = 0 To dsReinsurers.Tables(0).Rows.Count - 1
                dsReinsurers.Tables(0).Rows(icount).Item("TaxPercentageSpecified") = True
            Next

            If (dsReinsurers.Tables(0).Rows.Count > 0) Then

                Dim xmlDoc As New System.Xml.XmlDocument

                dsReinsurers.DataSetName = "BaseFindReinsurerResponseTypeReinsurers"
                dsReinsurers.Tables(0).TableName = "Row"
                If FindReinsurerRequest.WCFSecurityToken = "" Then
                    xmlDoc.LoadXml(dsReinsurers.GetXml)
                    oFindReinsurerResponse.Reinsurers = xmlDoc.DocumentElement()
                End If
                oFindReinsurerResponse.ResultData = dsReinsurers
            End If
        End If
        Return oFindReinsurerResponse

    End Function

    Public Overloads Function GetTreatyPartyDetails(ByVal oGetTreatyPartyDetailsRequest As BaseGetTreatyPartyDetailsRequestType) As BaseGetTreatyPartyDetailsResponseType

        ' validate the request structure against the specified business rules
        Using conGetTreatyPartyDetails As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Dim oResponse As BaseGetTreatyPartyDetailsResponseType

            oResponse = GetTreatyPartyDetails(conGetTreatyPartyDetails, oGetTreatyPartyDetailsRequest)

            Return oResponse

        End Using
    End Function

    Public Overloads Function GetTreatyPartyDetails(ByVal con As SiriusConnection, ByVal oGetTreatyPartyDetailsRequest As BaseGetTreatyPartyDetailsRequestType) As BaseGetTreatyPartyDetailsResponseType

        Dim oGetTreatyPartyDetailsResponse As New BaseGetTreatyPartyDetailsResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim iTreatyId As Integer = 0

        If oGetTreatyPartyDetailsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetTreatyPartyDetailsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oGetTreatyPartyDetailsResponse = New SAMForInsuranceV2ImplementationTypes.GetTreatyPartyDetailsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        ' Check mandatory fields
        oGetTreatyPartyDetailsRequest.Validate(CType(oSAMErrorCollection, Object))
        oSAMErrorCollection.CheckForErrors()

        iTreatyId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
          PMLookupTable.Treaty, oGetTreatyPartyDetailsRequest.TreatyCode, "TreatyCode", oSAMErrorCollection)
        oSAMErrorCollection.CheckForErrors()

        ' Dataset that will hold the returned results       
        Dim dsGetTreatyPartyDetails As DataSet = Nothing
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Treaty_Party_saa")
            cmd.AddInParameter("@treaty_id", SqlDbType.Int).Value = iTreatyId
            dsGetTreatyPartyDetails = con.ExecuteDataSet(cmd, "Row")
        End Using

        dsGetTreatyPartyDetails.DataSetName = "BaseGetTreatyPartyDetailsResponseType"

        ' Map the response back 
        dsGetTreatyPartyDetails.Tables(0).Columns("treaty_party_id").ColumnName = "TreatyPartyKey"
        dsGetTreatyPartyDetails.Tables(0).Columns("party_cnt").ColumnName = "PartyKey"
        dsGetTreatyPartyDetails.Tables(0).Columns("Resolved_Name").ColumnName = "ResolvedName"
        dsGetTreatyPartyDetails.Tables(0).Columns("treaty_id").ColumnName = "TreatyKey"
        dsGetTreatyPartyDetails.Tables(0).Columns("share_percent").ColumnName = "SharePercent"
        dsGetTreatyPartyDetails.Tables(0).Columns("commission_percent").ColumnName = "CommissionPercent"
        dsGetTreatyPartyDetails.Tables(0).Columns("domiciled_for_tax").ColumnName = "IsDomiciledForTax"
        dsGetTreatyPartyDetails.Tables(0).Columns("tax_group_id").ColumnName = "TaxGroupKey"
        dsGetTreatyPartyDetails.Tables(0).Columns("tax_group_descr").ColumnName = "Description"
        dsGetTreatyPartyDetails.Tables(0).Columns("Is_Reinsurer_Approved").ColumnName = "IsReinsurerApproved"

        If (dsGetTreatyPartyDetails.Tables.Count > 0) Then

            If (dsGetTreatyPartyDetails.Tables(0).Rows.Count > 0) Then

                Dim xmlDoc As New System.Xml.XmlDocument

                dsGetTreatyPartyDetails.DataSetName = "BaseGetTreatyPartyDetailsResponseTypeParties"
                dsGetTreatyPartyDetails.Tables(0).TableName = "Row"
                xmlDoc.LoadXml(dsGetTreatyPartyDetails.GetXml)
                oGetTreatyPartyDetailsResponse.Parties = xmlDoc.DocumentElement()
                oGetTreatyPartyDetailsResponse.ResultData = dsGetTreatyPartyDetails
            End If
        End If

        Return oGetTreatyPartyDetailsResponse

    End Function

#Region "GetClientDataExtract"

    Public Overloads Function GetClientDataExtract(ByVal BaseGetClientDataExtractRequestType As BaseGetClientDataExtractRequestType) As BaseGetClientDataExtractResponseType
        Using oSiriusCon As SiriusConnection = New SiriusConnectionPMDAO(_SiriusUser.Username,
                                                                         _SiriusUser.SourceID,
                                                                         _SiriusUser.LanguageID,
                                                                         SiriusUserDefaults.AppName)
            Dim oResponse As BaseGetClientDataExtractResponseType
            oResponse = GetClientDataExtract(oSiriusCon, BaseGetClientDataExtractRequestType)

            Return oResponse
        End Using
    End Function

    Public Overloads Function GetClientDataExtract(ByVal oSiriusCon As SiriusConnection, ByVal BaseGetClientDataExtractRequestType As BaseGetClientDataExtractRequestType) As BaseGetClientDataExtractResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim oResponse As New BaseGetClientDataExtractResponseType
        Dim nReturn As Integer
        Dim STSError As New STSErrorPublisher
        Dim nTypeofPackage As enumTypeOfPackage
        Dim nPartyCnt As Integer
        Dim sFilePassword As String = String.Empty
        Dim sBranchCode As String = String.Empty
        Dim nSourceId As Integer
        Dim btByteArray() As Byte
        Const ACMethodName As String = "GetClientDataExtract"

        If BaseGetClientDataExtractRequestType.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetClientDataExtractRequestType) Then
            nTypeofPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.GetClientDataExtractResponseType
        Else
            nTypeofPackage = enumTypeOfPackage.UnknownPackage
            Return oResponse
        End If

        If BaseGetClientDataExtractRequestType.PartyCnt = 0 Then
            STSError.AddInvalidField("PartyCnt", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "PartyCnt"), "")
        Else
            nPartyCnt = BaseGetClientDataExtractRequestType.PartyCnt
        End If

        If BaseGetClientDataExtractRequestType.FilePassword Is Nothing OrElse BaseGetClientDataExtractRequestType.FilePassword = String.Empty Then
            STSError.AddInvalidField("FilePassword", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "FilePassword"), "")
        Else
            sFilePassword = BaseGetClientDataExtractRequestType.FilePassword
        End If

        If BaseGetClientDataExtractRequestType.BranchCode IsNot Nothing AndAlso BaseGetClientDataExtractRequestType.BranchCode <> String.Empty Then
            sBranchCode = BaseGetClientDataExtractRequestType.BranchCode
        End If

        'Check Mandatory Field
        BaseGetClientDataExtractRequestType.Validate(CType(oSAMErrorCollection, Object))
        oSAMErrorCollection.CheckForErrors()

        'Look up validation
        nSourceId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.Source, BaseGetClientDataExtractRequestType.BranchCode, "BranchCode", oSAMErrorCollection)
        oSAMErrorCollection.CheckForErrors()

        Dim oGIS As bGIS.Application = Nothing
        Try
            oGIS = New bGIS.Application()
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.FailedToCreateBackofficeComponent, "Failed to create bGIS.Application", ex.Message)
            STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "CreatebGISSTS", True)
            Return oResponse
        End Try
        Try
            'SAMFunc.InitialiseGIS(Con:=oSiriusCon, oGIS:=oGIS, SiriusUser:=_SiriusUser)
            If oSiriusCon IsNot Nothing Then
                nReturn = Convert.ToInt32(oGIS.Initialise(sUserName:=_SiriusUser.Username,
                                                        sPassword:=_SiriusUser.Password,
                                                        iUserID:=CInt(_SiriusUser.UserID),
                                                        iSourceID:=CInt(_SiriusUser.SourceID),
                                                        iLanguageID:=CInt(_SiriusUser.LanguageID),
                                                        iCurrencyID:=CInt(_SiriusUser.CurrencyID),
                                                        iLogLevel:=SiriusUserDefaults.LogLevel,
                                                        sCallingAppName:=ACMethodName,
                                                        vDatabase:=oSiriusCon.PMDAODatabase))
            Else
                nReturn = Convert.ToInt32(oGIS.Initialise(sUserName:=_SiriusUser.Username,
                                                        sPassword:=_SiriusUser.Password,
                                                        iUserID:=CInt(_SiriusUser.UserID),
                                                        iSourceID:=CInt(_SiriusUser.SourceID),
                                                        iLanguageID:=CInt(_SiriusUser.LanguageID),
                                                        iCurrencyID:=CInt(_SiriusUser.CurrencyID),
                                                        iLogLevel:=SiriusUserDefaults.LogLevel,
                                                        sCallingAppName:=ACMethodName))
            End If
            If (nReturn <> PMEReturnCode.PMTrue) Then
                Dim STSError2 As New STSErrorPublisher(nReturn, "Failed to initialise  bGIS.Application")
                STSError2.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "oGIS.Initialise", True)
            End If

            nReturn = oGIS.ExtractData(nPartyCnt, sFilePassword, btByteArray)
            If (nReturn <> PMEReturnCode.PMTrue) Then
                Dim STSError2 As New STSErrorPublisher(nReturn, "Failed to initialise extract data")
                STSError2.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "oGIS.ExtractData", True)
            End If
            oResponse.ClientDataFile = btByteArray

        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.FailedToInitialiseBackofficeComponent, "Failed to initialise bGIS.Application", ex.Message)
            STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "bGIS", True)
            Return oResponse
        Finally
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
        End Try

        Return oResponse
    End Function
#End Region

#Region "WPR 05 RI 2007 OFF"
    ''' <summary>
    ''' GetRITreatyPartyDetailsWithTax
    ''' </summary>
    ''' <param name="oGetRITreatyPartyDetailsWithTaxRequest"></param>
    ''' <returns></returns>
    Public Overloads Function GetRITreatyPartyDetailsWithTax(ByVal oGetRITreatyPartyDetailsWithTaxRequest As BaseGetRITreatyPartyDetailsWithTaxRequestType) As BaseGetRITreatyPartyDetailsWithTaxResponseType

        ' validate the request structure against the specified business rules
        Using conGetRITreatyPartyDetailsWithTax As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Dim oResponse As BaseGetRITreatyPartyDetailsWithTaxResponseType

            oResponse = GetRITreatyPartyDetailsWithTax(conGetRITreatyPartyDetailsWithTax, oGetRITreatyPartyDetailsWithTaxRequest)

            Return oResponse

        End Using
    End Function

    ''' <summary>
    ''' GetRITreatyPartyDetailsWithTax
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oGetRITreatyPartyDetailsWithTaxRequest"></param>
    ''' <returns></returns>
    Public Overloads Function GetRITreatyPartyDetailsWithTax(ByVal con As SiriusConnection, ByVal oGetRITreatyPartyDetailsWithTaxRequest As BaseGetRITreatyPartyDetailsWithTaxRequestType) As BaseGetRITreatyPartyDetailsWithTaxResponseType

        Dim oGetTreatyPartyDetailsResponse As New BaseGetRITreatyPartyDetailsWithTaxResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim iTreatyId As Integer = 0

        If oGetRITreatyPartyDetailsWithTaxRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetRITreatyPartyDetailsWithTaxRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oGetTreatyPartyDetailsResponse = New SAMForInsuranceV2ImplementationTypes.GetRITreatyPartyDetailsWithTaxResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        If oGetRITreatyPartyDetailsWithTaxRequest.TreatyID = 0 Then
            iTreatyId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
          PMLookupTable.Treaty, oGetRITreatyPartyDetailsWithTaxRequest.TreatyCode, "TreatyCode", oSAMErrorCollection)
            oSAMErrorCollection.CheckForErrors()
        Else
            iTreatyId = oGetRITreatyPartyDetailsWithTaxRequest.TreatyID
        End If

        If Not oGetRITreatyPartyDetailsWithTaxRequest.IgnoreTreatyDetails Then
            Dim dsDetails As New DataSet
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Treaty_sel")
                With oGetRITreatyPartyDetailsWithTaxRequest
                    cmd.AddInParameter("@treaty_id", SqlDbType.Int).Value = iTreatyId

                    dsDetails = con.ExecuteDataSet(cmd, "ds")
                End With
            End Using
            If dsDetails IsNot Nothing Then
                With dsDetails
                    If .Tables IsNot Nothing AndAlso .Tables.Count > 0 AndAlso .Tables(0).Rows IsNot Nothing AndAlso .Tables(0).Rows.Count > 0 Then
                        oGetTreatyPartyDetailsResponse.CommissionPercent = Convert.ToDecimal(.Tables(0).Rows(0).Item("avg_commission").ToString())
                        oGetTreatyPartyDetailsResponse.IsRetained = Convert.ToInt32(.Tables(0).Rows(0).Item("is_retained").ToString())
                        oGetTreatyPartyDetailsResponse.AgreementCode = Convert.ToString(.Tables(0).Rows(0).Item("agreement_code").ToString())
                    End If
                End With
            End If

        End If
        If Not oGetRITreatyPartyDetailsWithTaxRequest.IgnoreTax Then
            DeleteTaxCalculationEntries(con, oGetRITreatyPartyDetailsWithTaxRequest.RiskID, oGetRITreatyPartyDetailsWithTaxRequest.InsuranceFileID, TaxType.TREATYPREMIUMTAXTYPE, TaxType.TREATYCOMMISSIONTAXTYPE, oGetRITreatyPartyDetailsWithTaxRequest.RIArrangementLineID)

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Calculate_Treaty_Tax_Amounts")
                With oGetRITreatyPartyDetailsWithTaxRequest
                    cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = Cast.ToInt32(.InsuranceFileID)
                    cmd.AddInParameter("@risk_cnt", SqlDbType.Int).Value = Cast.ToInt32(.RiskID)
                    cmd.AddInParameter("@ri_arrangement_line_id", SqlDbType.Int).Value = .RIArrangementLineID
                    cmd.AddInParameter("@treaty_id", SqlDbType.Int).Value = iTreatyId
                    cmd.AddInParameter("@premium", SqlDbType.Decimal).Value = .Premium
                    cmd.AddInParameter("@commission", SqlDbType.Decimal).Value = .Commission
                    cmd.AddInParameter("@premium_transtype", SqlDbType.VarChar, 20).Value = .PremiumTransType
                    cmd.AddInParameter("@commission_transtype", SqlDbType.VarChar, 20).Value = .CommissionTransType
                    cmd.AddOutParameter("@premium_tax", SqlDbType.Money)
                    cmd.AddOutParameter("@commission_tax", SqlDbType.Money)

                    con.ExecuteNonQuery(cmd)
                    oGetTreatyPartyDetailsResponse.PremiumTax = Convert.ToDecimal(cmd.Parameters("@premium_tax").Value)
                    oGetTreatyPartyDetailsResponse.CommissionTax = Convert.ToDecimal(cmd.Parameters("@commission_tax").Value)
                End With
            End Using

        End If
            Return oGetTreatyPartyDetailsResponse

    End Function

    ''' <summary>
    ''' GetRIPropTreaties
    ''' </summary>
    ''' <param name="oGetRIPropTreatiesRequest"></param>
    ''' <returns></returns>
    Public Overloads Function GetRIPropTreaties(ByVal oGetRIPropTreatiesRequest As BaseGetRIPropTreatiesRequestType) As BaseGetRIPropTreatiesResponseType

        ' validate the request structure against the specified business rules
        Using conGetRIPropTreaties As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Dim oResponse As BaseGetRIPropTreatiesResponseType

            oResponse = GetRIPropTreaties(conGetRIPropTreaties, oGetRIPropTreatiesRequest)

        Return oResponse

        End Using
    End Function

    ''' <summary>
    ''' GetRIPropTreaties
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oGetRIPropTreatiesRequest"></param>
    ''' <returns></returns>
    Public Overloads Function GetRIPropTreaties(ByVal con As SiriusConnection, ByVal oGetRIPropTreatiesRequest As BaseGetRIPropTreatiesRequestType) As BaseGetRIPropTreatiesResponseType

        Dim oGetRIPropTreatiesResponse As New BaseGetRIPropTreatiesResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim iTreatyId As Integer = 0

        If oGetRIPropTreatiesRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetRIPropTreatiesRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oGetRIPropTreatiesResponse = New SAMForInsuranceV2ImplementationTypes.GetRIPropTreatiesResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        Dim dsDetails As New DataSet
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Treaty_saa")
            cmd.AddInParameter("@IgnoreDelete", SqlDbType.Int).Value = 1
            dsDetails = con.ExecuteDataSet(cmd, "ds")
        End Using

        dsDetails.DataSetName = "BaseGetRIPropTreatiesResponseType"

        ' Map the response back 
        dsDetails.Tables(0).Columns("treaty_id").ColumnName = "TreatyId"
        dsDetails.Tables(0).Columns("code").ColumnName = "TreatyCode"
        dsDetails.Tables(0).Columns("description").ColumnName = "TreatyDescription"

        If (dsDetails.Tables.Count > 0) Then

            If (dsDetails.Tables(0).Rows.Count > 0) Then

                Dim xmlDoc As New System.Xml.XmlDocument

                dsDetails.DataSetName = "BaseGetRIPropTreatiesResponseTypeTreaties"
                dsDetails.Tables(0).TableName = "Row"
                xmlDoc.LoadXml(dsDetails.GetXml)
                oGetRIPropTreatiesResponse.PropTreaties = xmlDoc.DocumentElement()
                oGetRIPropTreatiesResponse.ResultData = dsDetails
            End If
        End If

        Return oGetRIPropTreatiesResponse

    End Function

#End Region
#Region "UpdateRiOverrideReasonInRiArrangement"
    ''' <summary>
    ''' UpdateRiOverrideReasonInRiArrangement
    ''' </summary>
    ''' <param name="oUpdateRiOverrideReasonInRiArrangement"></param>
    ''' <returns></returns>
    Public Overloads Function UpdateRiOverrideReasonInRiArrangement(ByVal oUpdateRiOverrideReasonInRiArrangementRequest As BaseUpdateRiOverrideReasonInRiArrangementRequestType) As BaseUpdateRiOverrideReasonInRiArrangementResponseType

        ' validate the request structure against the specified business rules
        Using conUpdateRiOverrideReasonInRiArrangement As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Dim oResponse As BaseUpdateRiOverrideReasonInRiArrangementResponseType

            oResponse = UpdateRiOverrideReasonInRiArrangement(conUpdateRiOverrideReasonInRiArrangement, oUpdateRiOverrideReasonInRiArrangementRequest)

            Return oResponse

        End Using
    End Function

    ''' <summary>
    ''' UpdateRiOverrideReasonInRiArrangement
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oUpdateRiOverrideReasonInRiArrangementRequest"></param>
    ''' <returns></returns>
    Public Overloads Function UpdateRiOverrideReasonInRiArrangement(ByVal con As SiriusConnection, ByVal oUpdateRiOverrideReasonInRiArrangementRequest As BaseUpdateRiOverrideReasonInRiArrangementRequestType) As BaseUpdateRiOverrideReasonInRiArrangementResponseType

        Dim oUpdateRiOverrideReasonInRiArrangementResponse As New BaseUpdateRiOverrideReasonInRiArrangementResponseType
        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim iTreatyId As Integer = 0

        If oUpdateRiOverrideReasonInRiArrangementRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.UpdateRiOverrideReasonInRiArrangementRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oUpdateRiOverrideReasonInRiArrangementResponse = New SAMForInsuranceV2ImplementationTypes.UpdateRiOverrideReasonInRiArrangementResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        If oUpdateRiOverrideReasonInRiArrangementRequest.RiOverrideReasonId <> 0 AndAlso oUpdateRiOverrideReasonInRiArrangementRequest.RiArrangementId <> 0 Then

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Ri_Arrangement_RiOverrideReasonId_upd")
                With oUpdateRiOverrideReasonInRiArrangementRequest
                    cmd.AddInParameter("@RiArrangementId", SqlDbType.Int).Value = oUpdateRiOverrideReasonInRiArrangementRequest.RiArrangementId
                    cmd.AddInParameter("@RiOverrideReasonId", SqlDbType.Int).Value = oUpdateRiOverrideReasonInRiArrangementRequest.RiOverrideReasonId
                    con.ExecuteNonQuery(cmd)
                End With
            End Using
        End If

        Return oUpdateRiOverrideReasonInRiArrangementResponse

    End Function
#End Region
End Class

