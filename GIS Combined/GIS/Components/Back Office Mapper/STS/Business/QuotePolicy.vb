Option Strict Off
Option Explicit On
Imports System.Diagnostics.Contracts
Imports System.Globalization
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("QuotePolicy_NET.QuotePolicy")>
Public NotInheritable Class QuotePolicy
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: QuotePolicy
    '
    ' Date: RFC220600
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "QuotePolicy"

    Private Const ACLeadAgent As String = "PARTY_LEAD_AGENT_CNT"
    Private Const ACPartyCode As String = "PARTY_CODE"
    Private Const ACCurrencies As String = "CURRENCIES"

    '**** START CHANGES - Changed By: AAB  - Changed On: 23102002   ****
    '**** Added 5 new constant to support Group Client for Agents Online
    Private Const ACPartyTypeID As String = "PARTY_TYPE_ID"
    Private Const ACPartyGroupID As String = "PARTY_GROUP_ID"
    Private Const ACIsRegisteredCharity As String = "IS_REGISTERED_CHARITY"
    Private Const ACCharityNumber As String = "CHARITY_Number"
    Private Const ACNumberOfMembers As String = "NUMBER_OF_MEMBERS"
    Private Const ACSourceID As String = "SOURCE_ID"
    Private Const ACFileCode As String = "FILECODE"
    '****   END CHANGES - Changed By: AAB  - Changed On: 23102002   ****
    ' CTAF 030303
    Private Const ACGroupName As String = "GROUP_NAME"
    Private Const ACContactName As String = "CONTACT_NAME"
    Private Const ACCompanyName As String = "COMPANY_NAME"
    ' END CTAF 030303

    Private Const ACContactTypePhone As String = "TELEPHONE"
    Private Const ACContactTypeMain As String = "MAIN"
    Private Const ACContactTypeEmail As String = "E-MAIL"

    ' PW091105
    Private Const ACTPUserCode As String = "TP_USER_CODE"
    Private Const ACTPIntroducer As String = "TP_INTRODUCER"
    Private Const ACOccupationCode As String = "OCCUPATION_CODE"
    Private Const ACEmployerBusinessCode As String = "EMPLOYER_BUSINESS_CODE"
    Private Const ACEmploymentStatusCode As String = "EMPLOYMENT_STATUS_CODE"
    Private Const ACAlternativeID As String = "ALTERNATIVE_ID"
    Private Const ACBusinessCode As String = "BUSINESS_CODE"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' ************************************************
    ' Added to replace global variables 18/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Return value
    Private m_lReturn As Integer

    Private m_sGISDataModel As String = ""
    Private m_bSystemOptionEnhancedResolvedName As Boolean
    Private Const kSystemOptionEnhancedResolvedName As Integer = 5096

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)


    Public Property GISDataModel() As String
        Get
            Return m_sGISDataModel
        End Get
        Set(ByVal Value As String)
            m_sGISDataModel = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer




        Dim result As Integer = 0
        Dim sValue As String
        Try

            Dim lReturn As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            ' Set Username and Password
            m_sUsername = sUserName
            m_sPassword = sPassword

            ' Set User ID
            m_iUserID = iUserID

            ' Set Calling Application
            m_sCallingAppName = sCallingAppName

            ' Set Language ID
            m_iLanguageID = iLanguageID

            ' Set Source ID
            m_iSourceID = iSourceID

            ' Set Currency ID
            m_iCurrencyID = iCurrencyID

            ' Set Log Level
            m_iLogLevel = iLogLevel


            If Informations.IsNothing(vDatabase) Then
                lReturn = gPMComponentServices.CheckDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase)
            Else

                lReturn = gPMComponentServices.CheckDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed - Failed to create connection to Database", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:="Failed to create connection to Database")
                Return result
            End If

            m_lReturn = bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, kSystemOptionEnhancedResolvedName, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOptionValue Failed for Enhanced Resolved Name", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_bSystemOptionEnhancedResolvedName = (sValue = "1")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub



    ' ***************************************************************** '
    ' Name: GetQuotesPoliciesForParty
    '
    ' Description: List all Quotes AND/OR Policies of the type requested for
    '              this PartyCnt.
    '
    '              If NO type specified Quotes/Policies of ALL types
    '              are returned.
    '
    '              PolicyTypeCode = "MOTOR", "HOME", "TRAVEL" etc
    ' ***************************************************************** '
    Public Function GetQuotesPoliciesForParty(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lPartyCnt As Integer, ByVal v_lSearchType As Integer, ByRef r_vQuotePolicyArray(,) As Object, Optional ByVal v_sPolicyTypeCode As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer

        Dim vResultsArray(,) As Object
        Dim oSBOLink As bSIRIUSLink.SIRIUSLink

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    Set oSBOLink = CreateObject("bSIRIUSLink.SIRIUSLink")
            '
            '    lReturn = oSBOLink.Initialise(sUsername:=tosafestring(m_sUsername), _
            ''                                    sPassword:=tosafestring(m_sPassword), _
            ''                                    iUserID:=tosafeinteger(m_iUserID), _
            ''                                    iSourceID:=tosafeinteger(m_iSourceID), _
            ''                                    iLanguageID:=tosafestring(m_iLanguageID), _
            ''                                    iCurrencyID:=tosafeinteger(m_iCurrencyID), _
            ''                                    iLogLevel:=tosafeinteger(m_iLogLevel), _
            ''                                    sCallingAppName:=tosafestring(ACApp))
            ' Create bSiriusLink object

            oSBOLink = New bSIRIUSLink.SIRIUSLink
            lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotesPoliciesForParty Failed - bSiriusLink.GetQuotesAndPoliciesForParty method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuotesPoliciesForParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Return Policy of 2 = the effective policy

            lReturn = oSBOLink.GetQuotesAndPoliciesForParty(v_lPartyCnt:=v_lPartyCnt, r_vResults:=r_vQuotePolicyArray, v_sPolicyType:=v_sPolicyTypeCode, v_enmReturnPolicy:=2, v_dtDate:=DateTime.Today)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotesPoliciesForParty Failed - bSiriusLink.GetQuotesAndPoliciesForParty method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuotesPoliciesForParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'RJG 23/06/2000 - Loop through results array and return only selected fields
            If Informations.IsArray(r_vQuotePolicyArray) Then
                ReDim vResultsArray(5, r_vQuotePolicyArray.GetUpperBound(1))

                For i As Integer = 0 To r_vQuotePolicyArray.GetUpperBound(1)


                    vResultsArray(0, i) = r_vQuotePolicyArray(5, i) 'Insurance_File_Type.Code


                    vResultsArray(1, i) = r_vQuotePolicyArray(2, i) 'Insurance_file.InsuranceFileCnt



                    vResultsArray(2, i) = (CStr(r_vQuotePolicyArray(25, i)) & " " & CStr(r_vQuotePolicyArray(6, i))).Trim() 'Party Name


                    vResultsArray(3, i) = r_vQuotePolicyArray(3, i) 'Insurance Ref


                    vResultsArray(4, i) = r_vQuotePolicyArray(4, i) 'Insurance_Folder.Description

                    If CStr(r_vQuotePolicyArray(5, i)) = "QUOTE" Or CStr(r_vQuotePolicyArray(5, i)) = "MTAQUOTE" Then


                        vResultsArray(5, i) = r_vQuotePolicyArray(17, i) 'Created Date
                    Else


                        vResultsArray(5, i) = r_vQuotePolicyArray(10, i) 'Cover Start Date
                    End If
                Next i


                r_vQuotePolicyArray = vResultsArray
                vResultsArray = Nothing
            End If

            ' Destroy the class

            oSBOLink.Dispose()
            oSBOLink = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotesPoliciesForParty Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuotesPoliciesForParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetPolicyVersions
    '
    ' Description: List all versions of a policy given either Insurance
    '              File Ref or the Insurance Folder Ref
    '
    ' RFC120700 - Optionally Get the PolicyVersions via the InsuranceFileCnt
    '             OR Insurance File Reference (Policy Num)
    ' ***************************************************************** '
    Public Function GetPolicyVersions(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_lInsuranceFileCnt As Integer, ByRef r_vPolicyVersionArray(,) As Object, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_sInsuranceFileRef As String = "") As Integer


        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim vResultsArray(,) As Object
        Dim oSBOLink As bSIRIUSLink.SIRIUSLink
        Dim lInsuranceFile_Cnt As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    Set oSBOLink = CreateObject("bSIRIUSLink.SIRIUSLink")
            '
            '    lReturn = oSBOLink.Initialise(sUsername:=tosafestring(m_sUsername), _
            ''                                    sPassword:=tosafestring(m_sPassword), _
            ''                                    iUserID:=tosafeinteger(m_iUserID), _
            ''                                    iSourceID:=tosafeinteger(m_iSourceID), _
            ''                                    iLanguageID:=tosafestring(m_iLanguageID), _
            ''                                    iCurrencyID:=tosafeinteger(m_iCurrencyID), _
            ''                                    iLogLevel:=tosafeinteger(m_iLogLevel), _
            ''                                    sCallingAppName:=tosafestring(ACApp))
            ' bSIRIUSLink.SIRIUSLink

            oSBOLink = New bSIRIUSLink.SIRIUSLink
            lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyVersions Failed - Failed to create bSiriusLink object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetPolicyVersions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If CBool(CStr(v_sInsuranceFileRef = "").Trim()) Then
                ' Call the Sirius Link Object

                lReturn = oSBOLink.GetPolicyVersions(r_lInsuranceFileCnt:=lInsuranceFile_Cnt, r_vResults:=r_vPolicyVersionArray, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt)
                r_lInsuranceFileCnt = lInsuranceFile_Cnt
            Else
                ' Call the Sirius Link Object

                lReturn = oSBOLink.GetPolicyVersions(r_lInsuranceFileCnt:=lInsuranceFile_Cnt, r_vResults:=r_vPolicyVersionArray, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sPolicyNumber:=v_sInsuranceFileRef)
                r_lInsuranceFileCnt = lInsuranceFile_Cnt
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyVersions Failed - bSiriusLink.GetPolicyVersion method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetPolicyVersions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'RJG 23/06/2000 - Loop through results array and return only selected fields
            If Informations.IsArray(r_vPolicyVersionArray) Then
                ReDim vResultsArray(7, r_vPolicyVersionArray.GetUpperBound(1))

                For i As Integer = 0 To r_vPolicyVersionArray.GetUpperBound(1)


                    vResultsArray(0, i) = CStr(r_vPolicyVersionArray(3, i)).Trim() 'Insurance_File_Type.Code


                    vResultsArray(1, i) = r_vPolicyVersionArray(0, i) 'Insurance_File_Type.InsuranceFileCnt


                    vResultsArray(2, i) = CStr(r_vPolicyVersionArray(10, i)).Trim() & " " & CStr(r_vPolicyVersionArray(9, i)).Trim() 'Party Name


                    vResultsArray(3, i) = r_vPolicyVersionArray(6, i) 'Insurance Ref


                    vResultsArray(4, i) = r_vPolicyVersionArray(7, i) 'Insurance_Folder.Description

                    If CStr(r_vPolicyVersionArray(3, i)) = "QUOTE" Or CStr(r_vPolicyVersionArray(3, i)) = "MTAQUOTE" Then


                        vResultsArray(4, i) = r_vPolicyVersionArray(8, i) 'Created Date
                    Else


                        vResultsArray(5, i) = r_vPolicyVersionArray(4, i) 'Cover Start Date
                    End If


                    vResultsArray(6, i) = r_vPolicyVersionArray(1, i) 'Policy Version


                    vResultsArray(7, i) = r_vPolicyVersionArray(5, i) 'Expiry Date
                Next i


                r_vPolicyVersionArray = vResultsArray

                vResultsArray = Nothing

            End If

            ' Destroy the class

            oSBOLink.Dispose()
            oSBOLink = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyVersions Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetPolicyVersions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetContactName
    '
    ' Description: Gets the main contact name for a party
    '
    ' History: 20/03/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetExtraDetails(ByVal v_lPartyCnt As Integer, Optional ByRef r_sContactName As String = "", Optional ByRef r_sTradingName As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oParty As bSIRParty.Business
        Dim lCnt As Integer
        Dim vPartyCnt As Integer

        ' Declare this locally because we're changing data types
        ' This caused .NET to go into a tizzy
        Dim sContactName As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        oParty = New bSIRParty.Business
        m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetExtraDetails Failed - Failed to create bSIRParty.Business.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetExtraDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        vPartyCnt = v_lPartyCnt


        If Not Informations.IsNothing(r_sContactName) Then

            ' Get the main contact name

            m_lReturn = oParty.GetMainContact(vPartyCnt:=vPartyCnt, lMainContactCnt:=lCnt, sMainContactDesc:=sContactName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetExtraDetails Failed to get MainContactName", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetExtraDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                ' Clear up

                oParty.Dispose()
                oParty = Nothing
                Return result
            End If

            r_sContactName = sContactName

        End If


        If Not Informations.IsNothing(r_sTradingName) Then

            ' Get the other details now

            m_lReturn = oParty.GetDetails(vPartyCnt:=v_lPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetExtraDetails Failed to GetDetails", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetExtraDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                ' Clear up

                oParty.Dispose()
                oParty = Nothing
                Return result
            End If


            m_lReturn = oParty.GetNext(vPartyCnt:=v_lPartyCnt, vTradingName:=r_sTradingName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetExtraDetails Failed to GetNext", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetExtraDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                ' Clear up

                oParty.Dispose()
                oParty = Nothing
                Return result
            End If

            ' Clear up

            oParty.Dispose()
            oParty = Nothing

        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPartyGroupClient
    '
    ' Description: Gets extra information for group clients, not supported by SiriusLink
    '
    ' History: 25/03/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetPartyGroupClient(ByVal v_lPartyCnt As Object, Optional ByRef r_lPartyGroupTypeID As Integer = 0, Optional ByRef r_bIsRegisteredCharity As Boolean = False, Optional ByRef r_sNumberOfMembers As String = "", Optional ByRef r_sCharityNumber As String = "") As Integer

        Dim result As Integer = 0
        Dim lPartyGroupTypeID As Integer
        Dim bIsRegisteredCharity As Boolean
        Dim sNumberOfMembers, sCharityNumber As String
        Dim oParty As bSIRPartyGC.Business



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of bSIRParty

        oParty = New bSIRPartyGC.Business
        m_lReturn = oParty.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = oParty.GetDetails(vPartyCnt:=v_lPartyCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetPartyGroupClient Failed failed to GetDetails", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetPartyGroupClient", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If


        m_lReturn = oParty.GetNext(vPartyCnt:=v_lPartyCnt, vPartyGroupTypeID:=lPartyGroupTypeID, vIsRegisteredCharity:=bIsRegisteredCharity, vNumberofMembers:=sNumberOfMembers, vCharityNumber:=sCharityNumber)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Assign the variables back

        If Not Informations.IsNothing(r_lPartyGroupTypeID) Then
            r_lPartyGroupTypeID = lPartyGroupTypeID
        End If


        If Not Informations.IsNothing(r_bIsRegisteredCharity) Then
            r_bIsRegisteredCharity = bIsRegisteredCharity
        End If


        If Not Informations.IsNothing(r_sNumberOfMembers) Then
            r_sNumberOfMembers = sNumberOfMembers
        End If


        If Not Informations.IsNothing(r_sCharityNumber) Then
            r_sCharityNumber = sCharityNumber
        End If

        ' Clear up

        oParty.Dispose()
        oParty = Nothing

        Return result

    End Function


    Public Function GetParty(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lPartyCnt As Integer, ByRef r_sSurname As String, ByRef r_sForename As String, ByRef r_sPartyType As String, ByRef r_sAddress1 As String, ByRef r_sAddress2 As String, ByRef r_sAddress3 As String, ByRef r_sAddress4 As String, ByRef r_sPostCode As String, ByRef r_sDOB As String, ByRef r_sEMail As String, ByRef r_sUserID As String, ByRef r_sPassword As String, ByRef r_sShortName As String, ByRef r_sResolvedName As String, Optional ByRef r_sMothersMaidenName As Object = Nothing, Optional ByRef r_sTPUserCode As Object = Nothing, Optional ByRef r_sTPIntroducer As Object = Nothing, Optional ByRef r_sAQuestion As Object = Nothing, Optional ByRef r_sTheAnswer As Object = Nothing, Optional ByRef r_dMemorableDate As String = "", Optional ByRef r_dCurrInsRenewalDate As String = "", Optional ByRef r_sTitle As String = "", Optional ByRef r_sMaritalStatusCode As String = "", Optional ByRef r_sGenderCode As String = "", Optional ByRef r_sInitials As String = "", Optional ByRef r_sTelephoneNumber As Object = Nothing, Optional ByRef r_sContactName As Object = Nothing, Optional ByRef r_sTradingName As Object = Nothing, Optional ByRef r_lPartyGroupTypeID As Object = Nothing, Optional ByRef r_bIsRegisteredCharity As Object = Nothing, Optional ByRef r_sNumberOfMembers As Object = Nothing, Optional ByRef r_sCharityNumber As Object = Nothing, Optional ByRef r_sOccupationCode As String = "", Optional ByRef r_vAllContactsArray(,) As Object = Nothing, Optional ByRef r_sCountryCode As String = "", Optional ByRef r_lSourceId As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim oSBOLink As bSIRIUSLink.SIRIUSLink

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create bSiriusLink object
        oSBOLink = New bSIRIUSLink.SIRIUSLink
        Dim lReturn As Integer = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParty Failed - Failed to create bSiriusLink object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If


        lReturn = oSBOLink.GetParty(v_lPartyCnt:=v_lPartyCnt, r_sSurname:=r_sSurname, r_sForename:=r_sForename, r_sPartyType:=r_sPartyType, r_sAddress1:=r_sAddress1, r_sAddress2:=r_sAddress2, r_sAddress3:=r_sAddress3, r_sAddress4:=r_sAddress4, r_sPostCode:=r_sPostCode, r_dDOB:=r_sDOB, r_sEMail:=r_sEMail, r_sUserID:=r_sUserID, r_sPassword:=r_sPassword, r_sShortName:=r_sShortName, r_sResolvedName:=r_sResolvedName, r_sMothersMaidenName:=r_sMothersMaidenName, r_sTPUserCode:=r_sTPUserCode, r_sTPIntroducer:=r_sTPIntroducer, r_sAQuestion:=r_sAQuestion, r_sTheAnswer:=r_sTheAnswer, r_dMemorableDate:=r_dMemorableDate, r_dCurrInsRenewalDate:=r_dCurrInsRenewalDate, r_sTitle:=r_sTitle, r_sMaritalStatusCode:=r_sMaritalStatusCode, r_sGenderCode:=r_sGenderCode, r_sInitials:=r_sInitials, r_sTelephoneNumber:=r_sTelephoneNumber, r_sOccupationCode:=r_sOccupationCode, r_vAllContactsArray:=r_vAllContactsArray, r_sCountryCode:=r_sCountryCode, r_lSourceId:=r_lSourceId)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParty Failed - bSiriusLink.GetParty method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Destroy the link object

        oSBOLink.Dispose()
        oSBOLink = Nothing

        ' need to strip the time from the date
        If r_sDOB <> "" Then
            Dim TempDate As Date
            r_sDOB = If(DateTime.TryParse(r_sDOB, TempDate), TempDate.ToString("dd MMMM yyyy"), r_sDOB)
        End If

        If Not Informations.IsNothing(r_dMemorableDate) Then
            Dim TempDate2 As Date
            r_dMemorableDate = If(DateTime.TryParse(r_dMemorableDate, TempDate2), TempDate2.ToString("dd MMMM yyyy"), r_dMemorableDate)
        End If

        If Not Informations.IsNothing(r_dCurrInsRenewalDate) Then
            Dim TempDate3 As Date
            r_dCurrInsRenewalDate = If(DateTime.TryParse(r_dCurrInsRenewalDate, TempDate3), TempDate3.ToString("dd MMMM yyyy"), r_dCurrInsRenewalDate)
        End If

        ' CTAF 20030320 - Start - Get the contact name too

        m_lReturn = GetExtraDetails(v_lPartyCnt:=v_lPartyCnt, r_sContactName:=r_sContactName, r_sTradingName:=r_sTradingName)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParty Failed to get main contact name", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Get extra group client details?
        If r_sPartyType = "GC" Then
            r_sTitle = ""
            r_sMaritalStatusCode = ""
            r_sGenderCode = ""
            r_sInitials = ""
            m_lReturn = GetPartyGroupClient(v_lPartyCnt:=v_lPartyCnt, r_lPartyGroupTypeID:=CInt(r_lPartyGroupTypeID), r_bIsRegisteredCharity:=CBool(r_bIsRegisteredCharity), r_sNumberOfMembers:=CStr(r_sNumberOfMembers), r_sCharityNumber:=CStr(r_sCharityNumber))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetParty Failed to GetPartyGroupClient details", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

        End If


        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParty Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    Public Function FindParty(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_sPartyType As String, ByVal v_sShortname As String, ByVal v_sResolvedName As String, ByVal v_sUserID As String, ByVal v_sTelephoneNumber As String, ByVal v_sPostcode As String, ByRef r_vResultArray(,) As Object, Optional ByVal v_lLeadAgentCnt As Integer = 0, Optional ByVal v_sAddress1 As String = "", Optional ByVal v_vAdditionalDataArray As Object = Nothing, Optional ByVal v_sFileCode As String = "") As Integer

        Dim result As Integer = 0
        Try

            Dim lReturn As Integer
            Dim oSBOLink As bSIRIUSLink.SIRIUSLink

            Dim sPolicyNo As String = ""
            Dim blCalledFromSTS As Boolean

            result = gPMConstants.PMEReturnCode.PMTrue

            blCalledFromSTS = False

            For i As Integer = 0 To v_vAdditionalDataArray.GetUpperBound(1)
                Select Case v_vAdditionalDataArray(0, i)
                    Case CNCalledFromSTS

                        blCalledFromSTS = CBool(v_vAdditionalDataArray(1, i))
                        Exit For
                End Select
            Next i

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return result
            End If

            ' Create bSiriusLink object
            oSBOLink = New bSIRIUSLink.SIRIUSLink
            lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindParty Failed - Failed to Create bSiriusLink object", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="FindParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' CTAF 20030403 - Search on PolicyNumber aka insurance_file_ref
            ' CLG  20040216 - removed sPolicyNo processing as this is now done in bSIRFindParty

            ' CTAF 20030403 - Added Address1

            lReturn = oSBOLink.FindParty(r_vResultsArray:=r_vResultArray, v_sPartyType:=v_sPartyType, v_sShortname:=v_sShortname, v_sResolvedName:=v_sResolvedName, v_sTelephoneNumber:=v_sTelephoneNumber, v_sPostCode:=v_sPostcode, v_lLeadAgentCnt:=v_lLeadAgentCnt, v_sAddress1:=v_sAddress1, v_sPolicyNo:=sPolicyNo, v_vAdditionalDataArray:=v_vAdditionalDataArray, v_sFileCode:=v_sFileCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindParty Failed - bSiriusLink.FindParty method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="FindParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                End If
                ' Return the value
                Return lReturn
            End If

            ' Destroy the link object

            oSBOLink.Dispose()
            oSBOLink = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindParty Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="FindParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function FindQuote(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_vResultArray(,) As Object, Optional ByVal v_sQuoteRef As String = "", Optional ByVal v_dCoverStartDate As Date = GISSharedConstants.GISLowDate, Optional ByVal v_sDescription As String = "", Optional ByVal v_lLeadAgentCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            Dim lReturn As Integer
            Dim oSBOLink As bSIRIUSLink.SIRIUSLink

            result = gPMConstants.PMEReturnCode.PMTrue

            '    Set oSBOLink = CreateObject("bSIRIUSLink.SIRIUSLink")
            '
            '    lReturn = oSBOLink.Initialise(sUsername:=tosafestring(m_sUsername), _
            ''                                    sPassword:=tosafestring(m_sPassword), _
            ''                                    iUserID:=tosafeinteger(m_iUserID), _
            ''                                    iSourceID:=tosafeinteger(m_iSourceID), _
            ''                                    iLanguageID:=tosafestring(m_iLanguageID), _
            ''                                    iCurrencyID:=tosafeinteger(m_iCurrencyID), _
            ''                                    iLogLevel:=tosafeinteger(m_iLogLevel), _
            ''                                    sCallingAppName:=tosafestring(ACApp))
            ' Create bSiriusLink object

            oSBOLink = New bSIRIUSLink.SIRIUSLink
            lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindQuote Failed - Failed to create bSiriusLink object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="FindQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            lReturn = oSBOLink.FindQuote(r_vResultArray:=r_vResultArray, v_sQuoteRef:=v_sQuoteRef, v_dCoverStartDate:=v_dCoverStartDate, v_sDescription:=v_sDescription, v_lLeadAgentCnt:=v_lLeadAgentCnt)

            If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindQuote Failed - bSiriusLink.FindQuote failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="FindQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Destroy the link object

            oSBOLink.Dispose()
            oSBOLink = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindQuote Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="FindQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' FRIEND Methods (Begin)

    ' FRIEND Methods (End)

    ' PRIVATE Methods (Begin)


    '
    '
    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=tosafestring(ACApp), vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Public Function FindPartySiriusLink(ByRef r_vResultsArray(,) As Object, Optional ByVal v_sPartyType As String = "", Optional ByVal v_sShortname As String = "", Optional ByVal v_sResolvedName As String = "", Optional ByVal v_sTelephoneNumber As String = "", Optional ByVal v_sPostcode As String = "", Optional ByVal v_lLeadAgentCnt As Integer = 0, Optional ByVal v_sAddress1 As String = "") As Integer

        ' ***************************************************************** '
        ' Name: FindPartySiriusLink (was FindParty)
        ' Date: 15/11/2000
        ' Description:  Finds parties given any number of the optional parameters above
        ' NOTE: at least one of these optional params must be provided..
        ' ***************************************************************** '
        'AAB - 27-Aug-2002 11:57 - Added the optioal paramater v_sAddress1 to support Agents On Line.

        Dim result As Integer = 0
        Try

            Dim vResultArray(,) As Object = Nothing
            Dim sStructure As String = ""

            Dim oFindParty As bSIRFindParty.Business

            result = gPMConstants.PMEReturnCode.PMFalse

            'RJG 15/11/2000 - First check that at least one optional parameter has been passed.
            If v_sPartyType = "" And v_sShortname = "" And v_sResolvedName = "" And v_sTelephoneNumber = "" And v_sPostcode = "" And v_lLeadAgentCnt = 0 Then

                Return result
            End If

            ' Create bSIRFindParty.Business object
            oFindParty = New bSIRFindParty.Business
            m_lReturn = oFindParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            'RJG 15/11/2000 - Quit the function if the FindParty object failed to initialise
            'AAB - 16-Oct-2002 10:11 - Changed it from = PMFalse to <> PMTrue
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRFindParty.Business Object Failed to Initialize", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="FindParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return m_lReturn
            End If

            '**** START CHANGES - Changed By: AAB  - Changed On: 27-Aug-2002 11:57   ****
            '**** Added this mehtod to get the structure, so AGENTS can be excludeded.

            m_lReturn = oFindParty.GetStructure(sStructure)
            'AAB - 16-Oct-2002 10:11 - Changed it from = PMFalse to <> PMTrue
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRFindParty.Business.GetStructure Method Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="FindParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return m_lReturn
            End If
            '****   END CHANGES - Changed By: AAB  - Changed On: 27-Aug-2002 11:57   ****


            m_lReturn = oFindParty.SearchByQuery(r_vResultArray:=vResultArray, r_lNumberOfRecords:=100, v_vShortName:=v_sShortname, v_vName:=v_sResolvedName, v_vClientType:=v_sPartyType, v_vNumber:=v_sTelephoneNumber, v_vAgentCnt:=v_lLeadAgentCnt, v_vPostalCode:=v_sPostcode, v_vAddress1:=v_sAddress1, v_bIgnoreSourceCheck:=True, bLimitRecords:=False)

            'AAB - 16-Oct-2002 10:10 - Changed it from = PMFalse to <> PMTrue
            'Also assigned the Function value to the proper return value.
            'SJ 01/07/2004 - start
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                'No results so exit here
                Return m_lReturn
            End If
            'SJ 01/07/2004 - end

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'SJ 01/07/2004 - start
                '        LogMessage PMLogOnError, "m_oFindParty.SearchByQuery Failed.", ACApp, ACClass, "FindParty"
                bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "m_oFindParty.SearchByQuery Failed.", ACApp, ACClass, "FindParty")
                'SJ 01/07/2004 - end
                Return m_lReturn
            End If

            'RJG 15/11/2000 - Columns:
            '0  PartyCnt
            '1  ShortName
            '2  ResolvedName
            '3  Address1
            '4  Postcode
            '5  Telephone Number
            '6  Date of Birth
            '7  Agent CNT
            If Informations.IsArray(vResultArray) Then

                '**** START CHANGES - Changed By: AAB  - Changed On: 27-Aug-2002 11:09   ****
                '**** Changed it from 6 to 7 to add the Agent_Cnt

                ReDim r_vResultsArray(7, vResultArray.GetUpperBound(1))
                '****   END CHANGES - Changed By: AAB  - Changed On: 27-Aug-2002 11:09   ****


                For i As Integer = 0 To vResultArray.GetUpperBound(1)


                    r_vResultsArray(0, i) = vResultArray(0, i) 'PartyCnt


                    r_vResultsArray(1, i) = CStr(vResultArray(2, i)).Trim() 'ShortName


                    r_vResultsArray(2, i) = CStr(vResultArray(3, i)).Trim() 'ResolvedName


                    r_vResultsArray(3, i) = CStr(vResultArray(4, i)).Trim() 'Address1


                    r_vResultsArray(4, i) = CStr(vResultArray(5, i)).Trim() 'Postcode


                    r_vResultsArray(5, i) = (CStr(vResultArray(8, i)) & "").Trim() & (CStr(vResultArray(9, i)) & "").Trim() 'Telephone Number


                    r_vResultsArray(6, i) = vResultArray(16, i) 'Date of birth
                    '**** START CHANGES - Changed By: AAB  - Changed On: 27-Aug-2002 11:09   ****
                    '**** Added the Agent Cnt to the result Array
                    '**** I changed this from 19 - 20.  Element #19 = Source.  Element #20 = Agent_Cnt


                    r_vResultsArray(7, i) = vResultArray(20, i) 'Agent Cnt
                    '****   END CHANGES - Changed By: AAB  - Changed On: 27-Aug-2002 11:09   ****
                Next i
            End If

            result = m_lReturn

            oFindParty.Dispose()
            oFindParty = Nothing
            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindPartySiriusLink Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="FindPartySiriusLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetPartyShortName
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-02-2008 : SAM Client Shortname Fix
    ' ***************************************************************** '
    Public Function GetPartyShortName(ByVal v_sPartyType As String, ByVal v_sName As String, ByVal v_sInitials As String, ByVal v_lSourceId As Integer, ByRef r_sShortCode As String, Optional ByVal v_fName As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyShortName"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oClientNumbering As Object
        Dim sShortCode As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(GetValidShortNameForParty(v_sPartyType, v_sName, v_sInitials, v_lSourceId, sShortCode, v_fName), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetValidShortNameForParty Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = CType(GetUniqueShortNameForParty(sShortCode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetUniqueShortname Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            r_sShortCode = sShortCode.ToUpper()

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=ToSafeString(m_sUsername), v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetUniqueShortNameForParty
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-02-2008 : SAM Client Shortname Fix
    ' ***************************************************************** '
    Public Function GetUniqueShortNameForParty(ByRef r_sShortName As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetUniqueShortNameForParty"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oParty As bSIRParty.Business
        Dim sShortname As String = ""
        Dim lPartyCnt, lNumber As Integer
        Dim bPartyExists As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the AutoNumber Business Object
            oParty = New bSIRParty.Business
            lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create instance of bSirParty.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            sShortname = r_sShortName

            Do


                lReturn = oParty.GetPartyCnt(vPartyRef:=sShortname, vPartyCnt:=lPartyCnt)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "bSIRParty.Business.GetPartyCnt Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If lPartyCnt <> 0 Then

                    bPartyExists = True

                    lNumber += 1

                    If lNumber > 999 Then
                        sShortname = r_sShortName & StringsHelper.Format(CStr(lNumber), "0000")
                    Else
                        sShortname = r_sShortName & StringsHelper.Format(CStr(lNumber), "000")
                    End If

                Else

                    bPartyExists = False

                End If

            Loop While bPartyExists


            r_sShortName = sShortname

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=ToSafeString(m_sUsername), v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oParty = Nothing

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetValidShortNameForParty
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-02-2008 : SAM Client Shortname Fix
    ' ***************************************************************** '
    Public Function GetValidShortNameForParty(ByVal v_sPartyType As String, ByVal v_sName As String, ByVal v_sInitials As String, ByVal v_lSourceId As Integer, ByRef r_sShortCode As String, Optional ByVal v_fName As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetValidShortNameForParty"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sShortCode As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If v_sPartyType = "CC" Or v_sPartyType = "PC" Then

                lReturn = CType(GetNumberingSchemePCORCCShortName(v_sPartyType, v_sName, v_sInitials, v_lSourceId, sShortCode, v_fName), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetNumberingSchemePCORCCShortName Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            Else

                sShortCode = v_sName
                sShortCode = sShortCode.Replace(" ", "")
                sShortCode = sShortCode.Replace("'", "")
                sShortCode = sShortCode.Replace("|", "")
                sShortCode = sShortCode.Replace(",", "")

            End If


            r_sShortCode = sShortCode

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=ToSafeString(m_sUsername), v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetNumberingSchemePCORCCShortName
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-02-2008 : SAM Client Shortname Fix
    ' ***************************************************************** '
    Public Function GetNumberingSchemePCORCCShortName(ByVal v_sPartyType As String, ByVal v_sName As String, ByVal v_sInitials As String, ByVal v_lSourceId As Integer, ByRef r_sShortCode As String, Optional ByVal v_fName As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetNumberingSchemePCORCCShortName"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oClientNumbering As bSIRPolicyNumMaint.Business

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the AutoNumber Business Object
            'Not passing Same Connection for Scalability
            oClientNumbering = New bSIRPolicyNumMaint.Business
            lReturn = oClientNumbering.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)



            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create instance of bSIRPolicyNumMaint.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            If v_sPartyType = "CC" Then


                lReturn = oClientNumbering.GenerateCCShortname(v_lSourceID:=v_lSourceId, v_sTradingName:=v_sName, r_sShortName:=r_sShortCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "bSIRPolicyNumMaint.Business GenerateCCShortname Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            Else


                lReturn = oClientNumbering.GeneratePCShortname(v_lSourceID:=v_lSourceId, v_sSurName:=v_sName, v_sInitials:=v_sInitials, r_sShortName:=r_sShortCode, v_fName:=v_fName)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "bSIRPolicyNumMaint.Business GeneratePCShortname Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=ToSafeString(m_sUsername), v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oClientNumbering = Nothing

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function









    ' ***************************************************************** '
    ' Name: GenerateBackOfficeShortName
    '
    ' Description: Generates a Shortname for the New Party according to
    '              back office style:
    '                  [Surname][Initial]
    '              or, [Surname][Initial][UniqueID]
    '
    ' ***************************************************************** '
    Private Function GenerateBackOfficeShortName(ByVal v_sSurname As String, ByVal v_sInitials As String, ByRef r_sUserID As String) As Integer

        Dim result As Integer = 0
        Dim iNumber, iUpper, lLenID As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sClientID, sWorkingID As String
        Dim vNamesArray(,) As Object



        ' Trim out spaces and single quotes, limit to 8 chars, append first initial and convert to uppercase
        sClientID = v_sSurname.Replace(" ", "")
        sClientID = sClientID.Replace("'", "")
        'sClientID = v_sSurname
        sClientID = sClientID.Substring(0, 8) & v_sInitials.Substring(0, 1)
        sClientID = sClientID.ToUpper()

        ' Ensure this ID is unique
        iNumber = 0
        lLenID = sClientID.Length

        ' Get a list of similar names

        lReturn = CType(FindPartySiriusLink(vNamesArray, v_sShortname:=sClientID & "%"), gPMConstants.PMEReturnCode)

        If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
            Return lReturn
        End If

        ' Process the array
        If Informations.IsArray(vNamesArray) Then

            iUpper = vNamesArray.GetUpperBound(1)
            For iIndex As Integer = 0 To iUpper

                sWorkingID = CStr(vNamesArray(1, iIndex)).Trim()

                ' If the name matches move our index to 1
                If sWorkingID = sClientID Then
                    If iNumber = 0 Then
                        iNumber = 1
                    End If
                Else
                    ' Trim off the base id
                    sWorkingID = sWorkingID.Substring(lLenID)

                    ' We are only concerned with numerics now
                    Dim dbNumericTemp As Double
                    If Double.TryParse(sWorkingID, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        If iNumber <= CInt(sWorkingID) Then
                            iNumber = CInt(sWorkingID) + 1
                        End If
                    End If
                End If
            Next iIndex
        End If

        ' Build our final id string (if we need to add a number)
        If iNumber > 0 Then
            sClientID = sClientID & CStr(iNumber)
        End If

        ' Set return value
        r_sUserID = sClientID

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ***************************************************************** '
    ' Name: GenerateShortName
    '
    ' Description: Generates a Shortname for the New Party
    '
    ' CTAF - 20030227 This is GenerateShortName taken from SiriusLink
    '
    ' ***************************************************************** '
    Private Function GenerateShortName(ByRef r_sUserID As String) As Integer

        Dim result As Integer = 0
        Dim oAutoNum As bPMAutoNumber.Business
        Dim lReturn, lUserID As Integer
        Dim sPrefix, sSuffix As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Component Services

            ' Create the AutoNumber Business Object

            oAutoNum = New bPMAutoNumber.Business
            lReturn = oAutoNum.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oAutoNum = Nothing
                Return lReturn
            End If

            ' Create a Unique Number for this Party

            lReturn = oAutoNum.GenerateNewNumber(v_sPMProductCode:=gPMConstants.PMProductCode(gPMConstants.PMEProductFamily.pmePFSiriusSolutions), v_sGroupCode:="XELUSERID", v_sRangeCode:="XELUSERID", v_iUserId:=1, r_lNumber:=lUserID, r_sPrefix:=sPrefix, r_sSuffix:=sSuffix)
            '    If (lReturn <> PMTrue) Then
            If lUserID < 1 Then
                bPMFunc.LogMessage(CStr(gPMConstants.PMELogLevel.PMLogError), CInt("Failed to generate New Number for PMNumber_Group : XELUSERID and PMNumber_Range : XELUSERID"), ACApp, ACClass, "GenerateShortname")
                Return lReturn
            End If

            ' Change the Unique Number into an alphanumeric ID which can be used as the ShortName.

            result = oAutoNum.EncodeLongV2(lUserID, r_sUserID)


            oAutoNum.Dispose()
            oAutoNum = Nothing

            Return result

        Catch excep As System.Exception




            oAutoNum.Dispose()

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateShortNameFailed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GenerateShortName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ''' <summary>
    ''' Adds a new party record
    ''' </summary>
    ''' <param name="v_sSurname"></param>
    ''' <param name="v_sForename"></param>
    ''' <param name="v_sPartyType"></param>
    ''' <param name="v_sAddress1"></param>
    ''' <param name="v_sAddress2"></param>
    ''' <param name="v_sAddress3"></param>
    ''' <param name="v_sAddress4"></param>
    ''' <param name="v_sPostcode"></param>
    ''' <param name="v_dDOB"></param>
    ''' <param name="v_sEMail"></param>
    ''' <param name="v_sUserID"></param>
    ''' <param name="v_sPassword"></param>
    ''' <param name="r_lPartyCnt"></param>
    ''' <param name="r_sShortName"></param>
    ''' <param name="v_sMothersMaidenName"></param>
    ''' <param name="v_sTPUserCode"></param>
    ''' <param name="v_sTPIntroducer"></param>
    ''' <param name="v_sAQuestion"></param>
    ''' <param name="v_sTheAnswer"></param>
    ''' <param name="v_dMemorableDate"></param>
    ''' <param name="v_dCurrInsRenewalDate"></param>
    ''' <param name="v_sTitle"></param>
    ''' <param name="v_sMaritalStatusCode"></param>
    ''' <param name="v_sGenderCode"></param>
    ''' <param name="v_sInitials"></param>
    ''' <param name="v_sTelephoneNumber"></param>
    ''' <param name="v_lAgentCnt"></param>
    ''' <param name="v_bUseDefaultShortName"></param>
    ''' <param name="v_bIsProspect"></param>
    ''' <param name="v_lSourceId"></param>
    ''' <param name="v_iShortNameFormat"></param>
    ''' <param name="v_lPartyTypeID"></param>
    ''' <param name="v_lPartyGroupID"></param>
    ''' <param name="v_bIsRegisteredCharity"></param>
    ''' <param name="v_sCharityNumber"></param>
    ''' <param name="v_sCharityMembers"></param>
    ''' <param name="v_sContactName"></param>
    ''' <param name="v_sTradingName"></param>
    ''' <param name="v_vPaymentMethodCode"></param>
    ''' <param name="v_vPaymentTermCode"></param>
    ''' <param name="v_sCountryCode"></param>
    ''' <param name="v_sFileCode"></param>
    ''' <param name="v_sOccupationCode"></param>
    ''' <param name="v_sEmployerBusinessCode"></param>
    ''' <param name="v_sEmploymentStatusCode"></param>
    ''' <param name="v_sAlternativeID"></param>
    ''' <param name="v_sBusinessCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddPartySiriusLink(ByVal v_sSurname As String, ByVal v_sForename As String,
                                       ByVal v_sPartyType As String, ByVal v_sAddress1 As String, ByVal v_sAddress2 As String, ByVal v_sAddress3 As String,
                                       ByVal v_sAddress4 As String, ByVal v_sPostcode As String,
                                       ByVal v_dDOB As Date, ByVal v_sEMail As String, ByVal v_sUserID As String, ByVal v_sPassword As String,
                                       ByRef r_lPartyCnt As Integer, ByRef r_sShortName As String,
                                       Optional ByVal v_sMothersMaidenName As Object = Nothing,
                                       Optional ByVal v_sTPUserCode As Object = Nothing,
                                       Optional ByVal v_sTPIntroducer As Object = Nothing,
                                       Optional ByVal v_sAQuestion As Object = Nothing, Optional ByVal v_sTheAnswer As Object = Nothing,
                                       Optional ByVal v_dMemorableDate As Date = #12/30/1899#,
                                       Optional ByVal v_dCurrInsRenewalDate As Date = #12/30/1899#,
                                       Optional ByVal v_sTitle As String = "",
                                       Optional ByVal v_sMaritalStatusCode As String = "",
                                       Optional ByVal v_sGenderCode As Object = Nothing, Optional ByVal v_sInitials As String = "",
                                       Optional ByVal v_sTelephoneNumber As String = "", Optional ByVal v_lAgentCnt As Integer = 0,
                                       Optional ByVal v_bUseDefaultShortName As Boolean = False,
                                       Optional ByVal v_bIsProspect As Byte = 1, Optional ByVal v_lSourceId As Integer = SIRIUS_SOURCEID,
                                       Optional ByVal v_iShortNameFormat As Integer = 0,
                                       Optional ByVal v_lPartyTypeID As Byte = 0,
                                       Optional ByVal v_lPartyGroupID As Object = Nothing,
                                       Optional ByVal v_bIsRegisteredCharity As Boolean = False, Optional ByVal v_sCharityNumber As Object = Nothing,
                                       Optional ByVal v_sCharityMembers As Object = Nothing, Optional ByVal v_sContactName As String = "",
                                       Optional ByVal v_sTradingName As Object = Nothing,
                                       Optional ByVal v_vPaymentMethodCode As Object = Nothing,
                                       Optional ByVal v_vPaymentTermCode As Object = Nothing,
                                       Optional ByVal v_sCountryCode As String = "",
                                       Optional ByVal v_sFileCode As String = "",
                                       Optional ByVal v_sOccupationCode As String = "",
                                       Optional ByVal v_sEmployerBusinessCode As String = "",
                                       Optional ByVal v_sEmploymentStatusCode As String = "",
                                       Optional ByVal v_sAlternativeID As String = "",
                                       Optional ByVal v_sBusinessCode As String = "",
                                       Optional ByVal sAddress5 As String = "",
                                       Optional ByVal sAddress6 As String = "",
                                       Optional ByVal sAddress7 As String = "",
                                       Optional ByVal sAddress8 As String = "",
                                       Optional ByVal sAddress9 As String = "",
                                       Optional ByVal sAddress10 As String = "") As Integer

        Dim nResult As Integer = 0
        Try
            Dim oaContactArray(,) As Object
            Dim iContactIndex As Integer
            Dim sEncryptedPassword As String = ""
            Dim nCountryId As Integer
            Dim nBranchBaseCurrencyId As Integer
            Dim oPMLookup As BPMLOOKUP.Business
            Dim oParty As bSIRParty.Services
            nResult = gPMConstants.PMEReturnCode.PMFalse

            ' CJB 21/02/02 Allow usage of previous short name generation before the 19/06/2001 change was done
            ' PWF 19/06/2001 Allow usage of back office style short name
            ' GRW 08/01/01 Allow usage of default short name generation

            If Not v_bUseDefaultShortName Then
                ' Generate a User ID (Short name) for the Party

                If v_iShortNameFormat = 0 Then

                    m_lReturn = GetPartyShortName(v_sPartyType, v_sSurname, v_sInitials, v_lSourceId, r_sShortName, v_sForename)

                Else
                    m_lReturn = GenerateShortName(r_sUserID:=r_sShortName)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            oParty = New bSIRParty.Services
            m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            'RJG 18/05/2000 - Quit the function if the Party object failed to initialise
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPartySiriusLink Failed - Failed to Create bSIRParty object", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="FindParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            'RJG 13/06/2000 - Encrypt the password
            m_lReturn = bPMFunc.Encrypt(sPassword:=v_sPassword, sEncryptedPassword:=sEncryptedPassword)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPartySiriusLink Failed - Failed to encrypt password", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddPartySiriusLink", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            ' RJG 17/05/2000 - Set the necessary properties for the party object

            oParty.Name = v_sSurname
            oParty.Forename = v_sForename

            ' GRW 08/01/01 Allow usage of default short name generation
            If Not v_bUseDefaultShortName Then
                ' RFC110800 - Generate a Shortname

                oParty.Shortname = r_sShortName
            End If
            oParty.PartyType = v_sPartyType
            oParty.Address1 = v_sAddress1
            oParty.Address2 = v_sAddress2
            oParty.Address3 = v_sAddress3
            oParty.Address4 = v_sAddress4
            oParty.PostalCode = v_sPostcode
            oParty.Address5 = sAddress5
            oParty.Address6 = sAddress6
            oParty.Address7 = sAddress7
            oParty.Address8 = sAddress8
            oParty.Address9 = sAddress9
            oParty.Address10 = sAddress10
            oPMLookup = New BPMLOOKUP.Business
            m_lReturn = oPMLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            'RJG 18/05/2000 - Quit the function if the Party object failed to initialise
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPartySiriusLink Failed - Failed to Create bPMLookup object", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddPartySiriusLink", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            nCountryId = 0
            m_lReturn = oPMLookup.GetEffectiveIDFromCode(v_sTableName:=gPMConstants.PMLookupCountry, v_sCode:=v_sCountryCode, v_dtEffectiveDate:=DateTime.Now, r_lID:=nCountryId)
            'RJG 18/05/2000 - Quit the function if the Party object failed to initialise
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to PMLookup.GetEffectiveIDFromCode Failed - Failed to find entry in Country Lookup list for code - " & v_sCountryCode, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddPartySiriusLink", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If
            oParty.CountryId = nCountryId
            oPMLookup.Dispose()
            oPMLookup = Nothing

            ' PWF 19/06/2001 Allow usage of back office style resolved name
            ' Trim it, otherwise we get leading spaces for Corporate Clients. PW080206.
            If v_sTitle Is Nothing Then
                v_sTitle = ""
            End If
            If v_sInitials Is Nothing Then
                v_sInitials = ""
            End If
            If v_sSurname Is Nothing Then
                v_sSurname = ""
            End If
            Dim sOptionValue As String = String.Empty

            If v_sPartyType = "PC" Then  ''v_iOptionNumber must be exist in system_options table with 'Enhanced Personal Client Resolved Name' description
                m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername,
                                                          v_sPassword:=m_sPassword,
                                                          v_iUserID:=m_iUserID,
                                                          v_iMainSourceID:=m_iSourceID,
                                                          v_iLanguageID:=m_iLanguageID,
                                                          v_iCurrencyID:=m_iCurrencyID,
                                                          v_iLogLevel:=m_iLogLevel,
                                                          v_sCallingAppName:=m_sCallingAppName,
                                                          v_iOptionNumber:=GeneralConst.kSystemOptionEnhancedResolvedName,
                                                          r_sOptionValue:=sOptionValue,
                                                          v_iSourceID:=1), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If sOptionValue <> String.Empty AndAlso sOptionValue = "1" Then
                oParty.ResolvedName = (v_sTitle.Trim() & " " & v_sForename.Trim() & " " & v_sSurname.Trim()).Trim()

            Else
                oParty.ResolvedName = (v_sTitle.Trim() & " " & v_sInitials.Trim() & " " & v_sSurname.Trim()).Trim()
                '    m_oParty.ResolvedName = Trim$(v_sForename) & " " & Trim$(v_sSurname)
            End If

            oParty.DateOfBirth = v_dDOB

            oParty.UserID = v_sUserID

            oParty.Password = sEncryptedPassword

            If Not Informations.IsNothing(v_sMothersMaidenName) Then
                oParty.MothersMaidenName = v_sMothersMaidenName
            End If
            If Not Informations.IsNothing(v_sTPUserCode) Then
                oParty.TPUserCode = v_sTPUserCode
            End If
            If Not Informations.IsNothing(v_sTPIntroducer) Then
                oParty.TPIntroducerCode = v_sTPIntroducer
            End If
            If Not Informations.IsNothing(v_sAQuestion) Then
                oParty.AQuestion = v_sAQuestion
            End If
            If Not Informations.IsNothing(v_sTheAnswer) Then
                oParty.TheAnswer = v_sTheAnswer
            End If
            If Not Informations.IsNothing(v_dMemorableDate) Then
                If Informations.IsDate(v_dMemorableDate) Then
                    oParty.MemorableDate = Informations.DateSerial(v_dMemorableDate.Year, v_dMemorableDate.Month, v_dMemorableDate.Day)
                End If
            End If

            If Not Informations.IsNothing(v_dCurrInsRenewalDate) Then
                If Informations.IsDate(v_dCurrInsRenewalDate) Then
                    oParty.CurrInsRenewalDate = Informations.DateSerial(v_dCurrInsRenewalDate.Year,
                                                                       v_dCurrInsRenewalDate.Month,
                                                                       v_dCurrInsRenewalDate.Day)
                End If
            End If

            ' RFC050900 - Added Title & MaritalStatusCode optional parameters - START

            If Not Informations.IsNothing(v_sTitle) Then
                oParty.PartyTitleCode = v_sTitle.Trim()
            End If

            If Not Informations.IsNothing(v_sMaritalStatusCode) Then
                oParty.MaritalStatusCode = v_sMaritalStatusCode.Trim()
            End If

            If Not Informations.IsNothing(v_sGenderCode) Then
                oParty.GenderCode = v_sGenderCode
            End If
            If Not Informations.IsNothing(v_sInitials) Then
                oParty.Initials = v_sInitials
            End If

            If Not Informations.IsNothing(v_lAgentCnt) Then
                If v_lAgentCnt <> 0 Then
                    oParty.AgentCnt = v_lAgentCnt
                End If
            End If

            ' PWF 10/07/2001 Added v_bIsProspect as optional paramter (default 1) to allow Gemini
            ' to add clients directly without forcing the Propect flag on.

            oParty.IsProspect = v_bIsProspect

            ' PWF 01/07/2001 - Party source ID (defaulted to SIRIUS_SOURCEID)

            If False Or v_lSourceId.Equals(0) Then

                oParty.SourceID = SIRIUS_SOURCEID
            Else

                oParty.SourceID = v_lSourceId
            End If

            ' Set the currency to the base currency for the branch
            ' to mimic what backoffice does. PW080206.

            m_lReturn = bPMFunc.GetBranchBaseCurrency(v_lSourceID:=oParty.SourceID, v_oDatabase:=m_oDatabase, r_iCurrencyID:=nBranchBaseCurrencyId)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                oParty.CurrencyID = nBranchBaseCurrencyId
            Else

                oParty.CurrencyID = SIRIUS_CURRENCYID
            End If

            'RJG 29/09/2000 - Added Gender Code and Initials added as optional parameters - END
            If Informations.IsNothing(v_vPaymentMethodCode) Then
                oParty.PaymentMethodCode = ""
            End If
            If Informations.IsNothing(v_vPaymentTermCode) Then
                oParty.PaymentTermCode = ""
            End If

            If Not False Then
                oParty.FileCode = v_sFileCode
            End If

            'RJG 26/05/2000 - END

            ' PW091105 - add fields required for SAM solution
            If Not False Then

                oParty.OccupationCode = v_sOccupationCode
            End If
            If Not False Then

                oParty.EmployerBusinessCode = v_sEmployerBusinessCode
            End If
            If Not False Then

                oParty.EmploymentStatusCode = v_sEmploymentStatusCode
            End If
            If Not False Then

                oParty.AlternativeIdentifier = v_sAlternativeID
            End If
            If Not False Then

                oParty.PartyBusinessId = v_sBusinessCode
            End If

            If Not Informations.IsNothing(v_sTradingName) Then
                oParty.TradingName = v_sTradingName
            End If

            'RJG 12/06/2000 - Add E-Mail as a contact
            'PWF 18/06/2001 - Check for blank email
            If Not Informations.IsNothing(v_sEMail) AndAlso v_sEMail.Trim() <> "" Then
                'If v_sEMail.Trim() <> "" Then
                ' RAGFIX
                'ReDim vContactArray(3, iContactIndex)
                ReDim oaContactArray(4, iContactIndex)

                oaContactArray(0, iContactIndex) = "E-MAIL"
                oaContactArray(1, iContactIndex) = ""
                oaContactArray(2, iContactIndex) = v_sEMail
                oaContactArray(3, iContactIndex) = ""
                ' RAGFIX

                oaContactArray(4, iContactIndex) = "Email"

                iContactIndex += 1
            End If
            If Not Informations.IsNothing(v_sTelephoneNumber) Then
                If v_sTelephoneNumber.Trim() <> "" Then
                    If iContactIndex Then
                        ' RAGFIX
                        'ReDim Preserve vContactArray(3, iContactIndex)
                        ReDim Preserve oaContactArray(4, iContactIndex)
                    Else
                        ' RAGFIX
                        'ReDim vContactArray(3, iContactIndex)
                        ReDim oaContactArray(4, iContactIndex)
                    End If
                    oaContactArray(0, iContactIndex) = "TELEPHONE"
                    oaContactArray(1, iContactIndex) = ""
                    oaContactArray(2, iContactIndex) = v_sTelephoneNumber
                    oaContactArray(3, iContactIndex) = ""

                    ' RAGFIX

                    oaContactArray(4, iContactIndex) = "TelNo"

                    iContactIndex += 1
                End If
            End If

            ' CTAF 20030304 - Main Contact name start

            If Not Informations.IsNothing(v_sContactName) Then
                If v_sContactName.Trim() <> "" Then
                    If iContactIndex Then
                        ReDim Preserve oaContactArray(4, iContactIndex)
                    Else
                        ReDim oaContactArray(4, iContactIndex)
                    End If
                    oaContactArray(0, iContactIndex) = ACContactTypeMain
                    oaContactArray(1, iContactIndex) = ""
                    oaContactArray(2, iContactIndex) = v_sTelephoneNumber
                    oaContactArray(3, iContactIndex) = ""
                    ' RAGFIX
                    oaContactArray(4, iContactIndex) = v_sContactName
                    iContactIndex += 1
                End If
            End If
            ' CTAF 20030304 - Main Contact name end
            oParty.ContactArray = oaContactArray
            ' CTAF 20030228 - Group Party values
            If Not Informations.IsNothing(v_lPartyTypeID) And v_lPartyTypeID <> 0 Then

                oParty.PartyTypeID = v_lPartyTypeID
            End If
            'oParty.PartyGroupID = v_lPartyGroupID

            oParty.IsRegisteredCharity = v_bIsRegisteredCharity

            m_lReturn = oParty.CreateParty

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                r_lPartyCnt = oParty.PartyCnt
                nResult = gPMConstants.PMEReturnCode.PMTrue
            Else
                bPMFunc.LogMessage(CStr(gPMConstants.PMELogLevel.PMLogOnError), CInt("m_oParty.CreateParty Failed"),
                                   ACApp, ACClass, "RegisterUser")
            End If
            oParty.Dispose()
            oParty = Nothing

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPartyServices Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddPartyServices", excep:=excep)
            Return nResult

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' This function is used to createt he addresses agaisnt the supplied party_cnt
    ''' </summary>
    ''' <param name="v_sAddress1"></param>
    ''' <param name="v_sAddress2"></param>
    ''' <param name="v_sAddress3"></param>
    ''' <param name="v_sAddress4"></param>
    ''' <param name="v_sPostcode"></param>
    ''' <param name="v_lPartyCnt"></param>
    ''' <param name="r_lAddressCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateAddress(ByVal v_sAddress1 As String, ByVal v_sAddress2 As String,
                                   ByVal v_sAddress3 As String, ByVal v_sAddress4 As String,
                                   ByVal v_sPostcode As String, ByVal v_lPartyCnt As Integer,
                                   ByRef r_lAddressCnt As Integer) As Integer

        Return CreateAddress(v_sAddress1:=v_sAddress1,
                             v_sAddress2:=v_sAddress2, v_sAddress3:=v_sAddress3, v_sAddress4:=v_sAddress4, v_sPostcode:=v_sPostcode,
v_lPartyCnt:=v_lPartyCnt, r_lAddressCnt:=r_lAddressCnt,
                             sAddress5:="",
                             sAddress6:="",
                             sAddress7:="",
                             sAddress8:="",
                             sAddress9:="",
                             sAddress10:="")

    End Function
    ''' <summary>
    '''  This function is used to createt he addresses agaisnt the supplied party_cnt
    ''' </summary>
    ''' <param name="v_sAddress1"></param>
    ''' <param name="v_sAddress2"></param>
    ''' <param name="v_sAddress3"></param>
    ''' <param name="v_sAddress4"></param>
    ''' <param name="v_sPostcode"></param>
    ''' <param name="v_lPartyCnt"></param>
    ''' <param name="r_lAddressCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateAddress(ByVal v_sAddress1 As String, ByVal v_sAddress2 As String,
                                   ByVal v_sAddress3 As String, ByVal v_sAddress4 As String,
                                   ByVal v_sPostcode As String, ByVal v_lPartyCnt As Integer,
                                   ByRef r_lAddressCnt As Integer,
                                   ByVal sAddress5 As String,
                                     ByVal sAddress6 As String, ByVal sAddress7 As String,
                                     ByVal sAddress8 As String, ByVal sAddress9 As String,
                                     ByVal sAddress10 As String) As Integer
        Dim nResult As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oaResultArray(,) As Object = Nothing
        Dim sSQL As String = ""
        Dim nAddressUsageType As Integer
        Dim oAddress As bSIRAddress.Business
        Dim nAddressCnt As Integer

        nResult = gPMConstants.PMEReturnCode.PMTrue

        oAddress = New bSIRAddress.Business
        m_lReturn = oAddress.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            nResult = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="Failed to Initialise the Address Business", vApp:=ToSafeString(ACApp), vClass:=ACClass,
                               vMethod:="SendToOrion", vErrNo:=Informations.Err().Number,
                               vErrDesc:=Informations.Err().Description)

            Return nResult
        End If
        m_lReturn = oAddress.DirectAdd(vAddress1:=v_sAddress1, vAddress2:=v_sAddress2, vAddress3:=v_sAddress3,
                                                   vAddress4:=v_sAddress4, vPostalCode:=v_sPostcode,
                                                   sAddress5:=sAddress5,
                                                   sAddress6:=sAddress6, sAddress7:=sAddress7, sAddress8:=sAddress8,
                                                   sAddress9:=sAddress9, sAddress10:=sAddress10)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        nAddressCnt = oAddress.AddressCnt

        sSQL = "SELECT address_usage_type_id  FROM address_usage_type WHERE description = 'Correspondence Address' "

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetCorrespondenceId", bStoredProcedure:=False, vResultArray:=oaResultArray)

        If Informations.IsArray(oaResultArray) Then

            nAddressUsageType = CInt(oaResultArray(0, 0))
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        sSQL = "INSERT INTO party_address_usage " &
               "(address_cnt, " &
               "party_cnt, " &
               "address_usage_type_id) " &
               "VALUES (" &
               nAddressCnt & ", " &
               v_lPartyCnt & ", " &
               nAddressUsageType & ")"

        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddAddressUsage", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Return the address count
        r_lAddressCnt = nAddressCnt

        Return nResult
    End Function

    Private Function CreateContact(ByVal v_vType As Object, ByVal v_vAreaCode As Object, ByVal v_vNumber As Object, ByVal v_vExtension As Object, ByVal v_vDescription As Object, ByVal v_lPartyCnt As Object, ByVal v_lAddressCnt As Integer) As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vTypeId, vResultArray(,) As Object
        Dim sSQL As String = ""
        Dim oContact As bsircontact.Business
        Dim lContactCnt As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        oContact = New bsircontact.Business
        m_lReturn = oContact.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise the Address Business", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="CreateContact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If


        sSQL = "SELECT Contact_type_id  FROM Contact_type WHERE code = '" & CStr(v_vType) & "'"

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetContactTypeId", bStoredProcedure:=False, vResultArray:=vResultArray)

        If Informations.IsArray(vResultArray) Then


            vTypeId = vResultArray(0, 0)
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = oContact.DirectAdd(vContactTypeID:=vTypeId, vDescription:=v_vDescription, vAreaCode:=v_vAreaCode, vNumber:=v_vNumber, vExtension:=v_vExtension)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        lContactCnt = oContact.ContactCnt

        'Add Party Contact Usage


        sSQL = "INSERT INTO party_Contact_usage " &
               "(party_cnt, " &
               "contact_cnt) " &
               "VALUES (" &
               CStr(v_lPartyCnt) & ", " &
               lContactCnt & ")"

        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddContactUsage", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Add Address Contact Usage
        sSQL = "INSERT INTO Contact_Address_usage " &
               "(contact_cnt, " &
               "address_cnt) " &
               "VALUES (" &
               lContactCnt & ", " &
               v_lAddressCnt & ")"

        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddContactAddressUsage", bStoredProcedure:=False)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sSurname"></param>
    ''' <param name="v_sForename"></param>
    ''' <param name="v_sPartyType"></param>
    ''' <param name="v_sAddress1"></param>
    ''' <param name="v_sAddress2"></param>
    ''' <param name="v_sAddress3"></param>
    ''' <param name="v_sAddress4"></param>
    ''' <param name="v_sPostcode"></param>
    ''' <param name="v_dDOB"></param>
    ''' <param name="v_sEMail"></param>
    ''' <param name="v_sUserID"></param>
    ''' <param name="v_sPassword"></param>
    ''' <param name="r_lPartyCnt"></param>
    ''' <param name="r_sShortName"></param>
    ''' <param name="v_sMothersMaidenName"></param>
    ''' <param name="v_sTPUserCode"></param>
    ''' <param name="v_sTPIntroducer"></param>
    ''' <param name="v_sAQuestion"></param>
    ''' <param name="v_sTheAnswer"></param>
    ''' <param name="v_dMemorableDate"></param>
    ''' <param name="v_dCurrInsRenewalDate"></param>
    ''' <param name="v_sTitle"></param>
    ''' <param name="v_sMaritalStatusCode"></param>
    ''' <param name="v_sGenderCode"></param>
    ''' <param name="v_sInitials"></param>
    ''' <param name="v_sTelephoneNumber"></param>
    ''' <param name="v_lAgentCnt"></param>
    ''' <param name="v_bUseDefaultShortName"></param>
    ''' <param name="v_bIsProspect"></param>
    ''' <param name="v_lSourceId"></param>
    ''' <param name="v_iShortNameFormat"></param>
    ''' <param name="v_lPartyTypeID"></param>
    ''' <param name="v_lPartyTypeGroupID"></param>
    ''' <param name="v_bIsRegisteredCharity"></param>
    ''' <param name="v_sCharityNumber"></param>
    ''' <param name="v_sCharityMembers"></param>
    ''' <param name="v_sGroupName"></param>
    ''' <param name="v_sContactName"></param>
    ''' <param name="v_sTradingName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateParty(ByVal v_sSurname As String, ByVal v_sForename As String, ByVal v_sPartyType As String,
                                 ByVal v_sAddress1 As String, ByVal v_sAddress2 As String,
                                 ByVal v_sAddress3 As String, ByVal v_sAddress4 As String, ByVal v_sPostcode As String,
                                 ByVal v_dDOB As Date, ByVal v_sEMail As String, ByVal v_sUserID As String, ByVal v_sPassword As String,
                                 ByRef r_lPartyCnt As Integer, ByRef r_sShortName As String,
                                 Optional ByVal v_sMothersMaidenName As Object = Nothing,
                                 Optional ByVal v_sTPUserCode As Object = Nothing,
                                 Optional ByVal v_sTPIntroducer As Object = Nothing, Optional ByVal v_sAQuestion As Object = Nothing,
                                 Optional ByVal v_sTheAnswer As Object = Nothing,
                                 Optional ByVal v_dMemorableDate As Object = Nothing,
                                 Optional ByVal v_dCurrInsRenewalDate As Object = Nothing, Optional ByVal v_sTitle As Object = Nothing,
                                 Optional ByVal v_sMaritalStatusCode As Object = Nothing,
                                 Optional ByVal v_sGenderCode As Object = Nothing,
                                 Optional ByVal v_sInitials As Object = Nothing,
                                 Optional ByVal v_sTelephoneNumber As Object = Nothing,
                                 Optional ByVal v_lAgentCnt As Object = Nothing,
                                 Optional ByVal v_bUseDefaultShortName As Boolean = False, Optional ByVal v_bIsProspect As Byte = 1,
                                 Optional ByVal v_lSourceId As Integer = 0, Optional ByVal v_iShortNameFormat As Byte = 0,
                                 Optional ByVal v_lPartyTypeID As Object = Nothing,
                                 Optional ByVal v_lPartyTypeGroupID As Object = Nothing,
                                 Optional ByVal v_bIsRegisteredCharity As Boolean = False,
                                 Optional ByVal v_sCharityNumber As Object = Nothing, Optional ByVal v_sCharityMembers As String = "",
                                 Optional ByVal v_sGroupName As Object = Nothing, Optional ByVal v_sContactName As Object = Nothing,
                                 Optional ByVal v_sTradingName As Object = Nothing,
                                 Optional ByVal sAddress5 As String = "",
                                 Optional ByVal sAddress6 As String = "",
                                 Optional ByVal sAddress7 As String = "",
                                 Optional ByVal sAddress8 As String = "",
                                 Optional ByVal sAddress9 As String = "",
                                 Optional ByVal sAddress10 As String = "") As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oParty As Object
        Dim sClassName As String = ""
        Dim lAddressCnt As Integer
        Dim lPartyCnt As Object = Nothing
        Dim sShortName As Object = Nothing

        ' Construct the class name
        sClassName = "bSIRParty" & v_sPartyType & ".Business"

        ' CTAF 20040604 - Default source id to user's source id

        If Informations.IsNothing(v_lSourceId) Then
            v_lSourceId = m_iSourceID
        End If

        ' Create the party object
        m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oParty, v_sClassName:=sClassName, v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        Select Case v_sPartyType
            Case "GC"

                If v_sCharityMembers = "" Then
                    v_sCharityMembers = "0"
                End If

                m_lReturn = GenerateBackOfficeShortName(v_sSurname:=v_sSurname, v_sInitials:="", r_sUserID:=sShortName)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Not worried. Worth a try with EditAdd. If that fails then so be it
                End If

                m_lReturn = oParty.EditAdd(lRow:=1, vPartyCnt:=lPartyCnt, vPartyGroupTypeID:=ToSafeInteger(v_lPartyTypeGroupID), vIsRegisteredCharity:=If(ToSafeBoolean(v_bIsRegisteredCharity), 1, 0),
                                           vCharityNumber:=ToSafeString(v_sCharityNumber), vNumberofMembers:=ToSafeString(v_sCharityMembers), vShortname:=sShortName, vName:=ToSafeString(v_sGroupName),
                                           vResolvedName:=ToSafeString(v_sGroupName), vIsProspect:=ToSafeBoolean(v_bIsProspect), vAgentCnt:=ToSafeInteger(v_lAgentCnt), vCurrencyId:=26, vSourceID:=ToSafeInteger(v_lSourceId), vTradingName:=ToSafeString(v_sTradingName))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                Else
                    r_lPartyCnt = lPartyCnt
                    r_sShortName = sShortName
                End If

                ' Update the party

                m_lReturn = oParty.Update()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the new Party Cnt
                r_lPartyCnt = oParty.PartyCnt

                ' Add the address
                m_lReturn = CreateAddress(v_sAddress1:=v_sAddress1,
                                              v_sAddress2:=v_sAddress2,
                                              v_sAddress3:=v_sAddress3,
                                              v_sAddress4:=v_sAddress4,
                                              v_sPostcode:=v_sPostcode,
                                              v_lPartyCnt:=r_lPartyCnt,
                                              r_lAddressCnt:=lAddressCnt,
                                              sAddress5:=sAddress5,
                                              sAddress6:=sAddress6,
                                              sAddress7:=sAddress7,
                                              sAddress8:=sAddress8,
                                              sAddress9:=sAddress9, sAddress10:=sAddress10)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CreateAdddress", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="CreateParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                ' Create the telephone contact
                m_lReturn = CreateContact(v_vType:=ACContactTypePhone, v_vAreaCode:="",
                                          v_vNumber:=v_sTelephoneNumber, v_vExtension:="",
                                          v_vDescription:="TelNo", v_lPartyCnt:=r_lPartyCnt,
                                          v_lAddressCnt:=lAddressCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create Telephone Contact", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="CreateParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                m_lReturn = CreateContact(v_vType:=ACContactTypeMain, v_vAreaCode:="",
                                          v_vNumber:=v_sTelephoneNumber, v_vExtension:="",
                                          v_vDescription:=v_sContactName, v_lPartyCnt:=r_lPartyCnt,
                                          v_lAddressCnt:=lAddressCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create Main Contact", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="CreateParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                ' Add an email address
                If v_sEMail <> "" Then
                    m_lReturn = CreateContact(v_vType:=ACContactTypeEmail, v_vAreaCode:="",
                                              v_vNumber:=v_sEMail, v_vExtension:="Email",
                                              v_vDescription:=v_sContactName, v_lPartyCnt:=r_lPartyCnt,
                                              v_lAddressCnt:=lAddressCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create Email Contact", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="CreateParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If
                End If

            Case Else
                ' Reserved for your future pleasure

        End Select

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return nResult

    End Function
    ''' <summary>
    ''' It's being used to create the Party using SAM wrapper.
    ''' </summary>
    ''' <param name="v_sGisDataModelCode"></param>
    ''' <param name="v_sGisBusinessTypeCode"></param>
    ''' <param name="v_sPartyTypeCode"></param>
    ''' <param name="v_sForename"></param>
    ''' <param name="v_sSurname"></param>
    ''' <param name="v_sDateOfBirth"></param>
    ''' <param name="v_sEmailAddress"></param>
    ''' <param name="v_sCurrentRenewalDate"></param>
    ''' <param name="v_sAddress1"></param>
    ''' <param name="v_sAddress2"></param>
    ''' <param name="v_sAddress3"></param>
    ''' <param name="v_sAddress4"></param>
    ''' <param name="v_sPostcode"></param>
    ''' <param name="r_lPartyCnt"></param>
    ''' <param name="v_sTitle"></param>
    ''' <param name="v_sMaritalStatusCode"></param>
    ''' <param name="v_sGenderCode"></param>
    ''' <param name="v_sInitials"></param>
    ''' <param name="v_sTelephoneNumber"></param>
    ''' <param name="v_sTradingName"></param>
    ''' <param name="r_vAdditionalDataArray"></param>
    ''' <param name="v_vPaymentMethodCode"></param>
    ''' <param name="v_vPaymentTermCode"></param>

    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddParty(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_sPartyTypeCode As String,
                             ByVal v_sForename As String, ByVal v_sSurname As String, ByVal v_sDateOfBirth As String,
                             ByVal v_sEmailAddress As String, ByVal v_sCurrentRenewalDate As String,
                             ByVal v_sAddress1 As String, ByVal v_sAddress2 As String, ByVal v_sAddress3 As String,
                             ByVal v_sAddress4 As String, ByVal v_sPostcode As String,
                             ByRef r_lPartyCnt As Integer, Optional ByVal v_sTitle As String = "",
                             Optional ByVal v_sMaritalStatusCode As String = "",
                             Optional ByVal v_sGenderCode As String = "", Optional ByVal v_sInitials As String = "",
                             Optional ByVal v_sTelephoneNumber As String = "", Optional ByVal v_sTradingName As String = "",
                             Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing,
                             Optional ByVal v_vPaymentMethodCode As Object = Nothing,
                             Optional ByVal v_vPaymentTermCode As Object = Nothing,
                             Optional ByVal sAddress5 As String = "", Optional ByVal sAddress6 As String = "",
                             Optional ByVal sAddress7 As String = "", Optional ByVal sAddress8 As String = "",
                             Optional ByVal sAddress9 As String = "",
                             Optional ByVal sAddress10 As String = "") As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oSBOLink As bSIRIUSLink.SIRIUSLink
        Dim sPassword, sUserID, sShortname As String
        Dim nSourceID As Integer
        Dim nAgentCnt As Integer
        Dim bIsOrionInstalled, bIsUnderwriting As Boolean
        Dim oBusiness1 As bSIROptions.Business
        Dim oBusiness2 As bSIROrionUpdate.Business
        Dim nPartyTypeID As Integer
        Dim nPartyGroupID As Integer
        Dim bIsRegisteredCharity As Boolean
        Dim sCharityNumber As String = String.Empty
        Dim sCharityMembers As String = String.Empty
        Dim sGroupName As String = String.Empty
        Dim sCompanyName As String = String.Empty
        Dim sContactName As String = String.Empty
        Dim blCalledFromSTS As Boolean
        Dim sCountryCode As String = String.Empty
        Dim sFileCode As String = String.Empty
        Dim sTpUserCode As String = String.Empty
        Dim sTPIntroducer As String = String.Empty
        Dim sOccupationCode As String = String.Empty
        Dim sEmployerBusinessCode As String = String.Empty
        Dim sEmploymentStatusCode As String = String.Empty
        Dim sAlternativeId As String = String.Empty
        Dim sBusinessCode As String = String.Empty

        Try

            blCalledFromSTS = False

            For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                Select Case r_vAdditionalDataArray(0, i)
                    Case CNCalledFromSTS

                        blCalledFromSTS = CBool(r_vAdditionalDataArray(1, i))
                    Case CNAddressCountry

                        sCountryCode = CStr(r_vAdditionalDataArray(1, i))
                End Select
            Next i

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return nResult
            End If

            ' Create bSiriusLink object
            oSBOLink = New bSIRIUSLink.SIRIUSLink
            nResult = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nResult
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddParty Failed - Failed to Create bSiriusLink object", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            ' Generate a User ID
            sUserID = bPMFunc.GetGUID()

            'Generate a random password
            nResult = CType(GeneratePassword(r_sPassword:=sPassword), gPMConstants.PMEReturnCode)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nResult
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddParty Failed - Failed to Generate Password.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            'RJG 01/12/2000 - Check the Additional Data Array for a Lead Agent Cnt

            If Not Informations.IsNothing(r_vAdditionalDataArray) Then
                If Informations.IsArray(r_vAdditionalDataArray) Then
                    For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)

                        Select Case r_vAdditionalDataArray(0, i)
                            Case ACLeadAgent

                                nAgentCnt = CInt(r_vAdditionalDataArray(1, i))

                            Case ACPartyTypeID

                                nPartyTypeID = CInt(r_vAdditionalDataArray(1, i))

                            Case ACPartyGroupID

                                nPartyGroupID = CInt(r_vAdditionalDataArray(1, i))

                            Case ACIsRegisteredCharity

                                bIsRegisteredCharity = CBool(r_vAdditionalDataArray(1, i))

                            Case ACNumberOfMembers

                                sCharityMembers = CStr(r_vAdditionalDataArray(1, i))

                            Case ACGroupName

                                sGroupName = CStr(r_vAdditionalDataArray(1, i))

                            Case ACContactName

                                sContactName = CStr(r_vAdditionalDataArray(1, i))

                            Case ACCompanyName

                                sCompanyName = CStr(r_vAdditionalDataArray(1, i))

                            Case ACCharityNumber

                                sCharityNumber = CStr(r_vAdditionalDataArray(1, i))

                            Case ACSourceID

                                nSourceID = Val(CStr(r_vAdditionalDataArray(1, i)))

                            Case ACFileCode

                                sFileCode = CStr(r_vAdditionalDataArray(1, i))

                            Case ACTPUserCode

                                sTpUserCode = CStr(r_vAdditionalDataArray(1, i))

                            Case ACTPIntroducer

                                sTPIntroducer = CStr(r_vAdditionalDataArray(1, i))

                            Case ACOccupationCode

                                sOccupationCode = CStr(r_vAdditionalDataArray(1, i))

                            Case ACEmployerBusinessCode

                                sEmployerBusinessCode = CStr(r_vAdditionalDataArray(1, i))

                            Case ACEmploymentStatusCode

                                sEmploymentStatusCode = CStr(r_vAdditionalDataArray(1, i))

                            Case ACAlternativeID

                                sAlternativeId = CStr(r_vAdditionalDataArray(1, i))

                            Case ACBusinessCode

                                sBusinessCode = CStr(r_vAdditionalDataArray(1, i))

                        End Select

                    Next i
                End If
            End If

            Select Case v_sPartyTypeCode
                Case "GC"

                    nResult = CType(CreateParty(v_sSurname:=v_sSurname, v_sForename:=v_sForename, v_sPartyType:=v_sPartyTypeCode, v_sAddress1:=v_sAddress1,
                                                v_sAddress2:=v_sAddress2, v_sAddress3:=v_sAddress3, v_sAddress4:=v_sAddress4, v_sPostcode:=v_sPostcode,
                                                v_dDOB:=CDate(v_sDateOfBirth), v_sEMail:=v_sEmailAddress, v_sUserID:=sUserID, v_sPassword:=sPassword,
                                                r_lPartyCnt:=r_lPartyCnt, r_sShortName:=sShortname, v_sTitle:=v_sTitle, v_sMaritalStatusCode:=v_sMaritalStatusCode,
                                                v_sGenderCode:=v_sGenderCode, v_sInitials:=v_sInitials, v_sTelephoneNumber:=v_sTelephoneNumber,
                                                v_lAgentCnt:=nAgentCnt, v_bIsProspect:=0, v_lSourceId:=nSourceID, v_lPartyTypeID:=nPartyTypeID,
                                                v_lPartyTypeGroupID:=nPartyGroupID, v_bIsRegisteredCharity:=bIsRegisteredCharity,
                                                v_sCharityNumber:=sCharityNumber, v_sCharityMembers:=sCharityMembers, v_sGroupName:=sGroupName,
                                                v_sContactName:=sContactName,
                                                v_sTradingName:=v_sTradingName,
                                                sAddress5:=sAddress5,
                                                sAddress6:=sAddress6, sAddress7:=sAddress7,
                                                sAddress8:=sAddress8, sAddress9:=sAddress9,
                                                sAddress10:=sAddress10), gPMConstants.PMEReturnCode)
                Case Else

                    nResult = CType(AddPartySiriusLink(v_sSurname:=v_sSurname, v_sForename:=v_sForename,
                                                                        v_sPartyType:=v_sPartyTypeCode, v_sAddress1:=v_sAddress1,
                                                                        v_sAddress2:=v_sAddress2, v_sAddress3:=v_sAddress3,
                                                                        v_sAddress4:=v_sAddress4, v_sPostcode:=v_sPostcode,
                                                                        v_dDOB:=CDate(v_sDateOfBirth), v_sEMail:=v_sEmailAddress,
                                                                        v_sUserID:=sUserID, v_sPassword:=sPassword,
                                                                        r_lPartyCnt:=r_lPartyCnt, r_sShortName:=sShortname,
                                                                        v_sTitle:=CStr(v_sTitle),
                                                                        v_sMaritalStatusCode:=CStr(v_sMaritalStatusCode),
                                                                        v_sGenderCode:=v_sGenderCode, v_sInitials:=CStr(v_sInitials),
                                                                        v_sTelephoneNumber:=CStr(v_sTelephoneNumber),
                                                                        v_lAgentCnt:=nAgentCnt, v_bIsProspect:=0,
                                                                        v_lSourceId:=CInt(nSourceID), v_lPartyTypeID:=nPartyTypeID,
                                                                        v_lPartyGroupID:=nPartyGroupID,
                                                                        v_bIsRegisteredCharity:=bIsRegisteredCharity,
                                                                        v_sCharityNumber:=sCharityNumber,
                                                                        v_sCharityMembers:=sCharityMembers,
                                                                        v_sContactName:=sContactName,
                                                                        v_sTradingName:=v_sTradingName,
                                                                        v_vPaymentMethodCode:=v_vPaymentMethodCode,
                                                                        v_vPaymentTermCode:=v_vPaymentTermCode,
                                                                        v_sCountryCode:=sCountryCode, v_sFileCode:=sFileCode,
                                                                        v_sTPUserCode:=sTpUserCode,
                                                                        v_sTPIntroducer:=sTPIntroducer,
                                                                        v_sOccupationCode:=sOccupationCode,
                                                                        v_sEmployerBusinessCode:=sEmployerBusinessCode,
                                                                        v_sEmploymentStatusCode:=sEmploymentStatusCode,
                                                                        v_sAlternativeID:=sAlternativeId,
                                                                        v_sBusinessCode:=sBusinessCode, sAddress5:=sAddress5,
                                                                        sAddress6:=sAddress6, sAddress7:=sAddress7,
                                                                        sAddress8:=sAddress8, sAddress9:=sAddress9,
                                                                        sAddress10:=sAddress10), gPMConstants.PMEReturnCode)
            End Select
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nResult
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddParty Failed - bSiriusLink.AddParty method Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            ' Let's add the sShortName to AdditionalDataArray.  Needed for AOL.

            If Not Informations.IsNothing(r_vAdditionalDataArray) Then
                If Informations.IsArray(r_vAdditionalDataArray) Then
                    ReDim Preserve r_vAdditionalDataArray(1, r_vAdditionalDataArray.GetUpperBound(1) + 1)
                Else
                    ReDim r_vAdditionalDataArray(1, 0)
                End If

                r_vAdditionalDataArray(0, r_vAdditionalDataArray.GetUpperBound(1)) = ACPartyCode

                r_vAdditionalDataArray(1, r_vAdditionalDataArray.GetUpperBound(1)) = sShortname
            End If

            ' Destroy the link object

            oSBOLink.Dispose()
            oSBOLink = Nothing

            'AAB-31012003 - Now that we finished getting the information we MUST create the account
            ' Check to see if Orion is installed.
            nResult = CType(gPMComponentServices.CheckPMProductInstalled(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bInstalled:=bIsOrionInstalled), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nResult
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddParty Failed - Failed to check if Orion is installed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            If bIsOrionInstalled Then
                'Check to see if the system is Underwriting

                oBusiness1 = New bSIROptions.Business
                nResult = oBusiness1.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = nResult
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddParty Failed - Failed to create reference to bSIROptions.Business.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    ' It is not fatal, so assume Broking is installed
                    bIsUnderwriting = False

                    ' Else find out for sure
                Else

                    ' If it is Underwriting then set the indicator Else default to Broking

                    bIsUnderwriting = (oBusiness1.UnderwritingOrAgency = "U")
                    oBusiness1.Dispose()
                    oBusiness1 = Nothing
                End If '(m_nResult& <> PMTrue)

                If bIsUnderwriting Then
                    ' Create the business object
                    oBusiness2 = New bSIROrionUpdate.Business
                    nResult = oBusiness2.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = nResult
                        ' Log Error Message
                        bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddParty Failed - Failed to create reference to bSIROrionUpdate.Business.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If

                    ' Create the account
                    nResult = oBusiness2.SiriusToOrion(v_lPartyCnt:=r_lPartyCnt)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = nResult
                        ' Log Error Message
                        bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddParty Failed - bSIROrionUpdate.Business.SiriusToOrion method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If

                End If 'bIsUnderwriting = True

            End If ' bIsOrionInstalled = True
            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddParty Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddParty", excep:=excep)
            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return nResult
        End Try
    End Function

    Public Function GetQuotesForParty(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lPartyCnt As Integer, ByRef r_vQuoteArray As Object, Optional ByVal v_sPolicyTypeCode As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oSBOLink As bSIRIUSLink.SIRIUSLink
            Dim lReturn As Integer

            '    Set oSBOLink = CreateObject("bSIRIUSLink.SIRIUSLink")
            '
            '    lReturn = oSBOLink.Initialise(sUsername:=tosafestring(m_sUsername), _
            ''                                  sPassword:=tosafestring(m_sPassword), _
            ''                                  iUserID:=tosafeinteger(m_iUserID), _
            ''                                  iSourceID:=tosafeinteger(m_iSourceID), _
            ''                                  iLanguageID:=tosafestring(m_iLanguageID), _
            ''                                  iCurrencyID:=tosafeinteger(m_iCurrencyID), _
            ''                                  iLogLevel:=tosafeinteger(m_iLogLevel), _
            ''                                  sCallingAppName:=tosafestring(ACApp))

            ' Create bSiriusLink object

            oSBOLink = New bSIRIUSLink.SIRIUSLink
            lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotesForParty Failed - Failed to create bSiriusLink object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuotesForParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If


            lReturn = oSBOLink.GetQuotesForParty(v_lPartyCnt:=v_lPartyCnt, r_vResults:=r_vQuoteArray, v_sPolicyType:=v_sPolicyTypeCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotesForParty Failed - bSiriusLink.GetQuotesForParty method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuotesForParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            oSBOLink.Dispose()
            oSBOLink = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotesForParty Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuotesForParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetProductByAgent(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lAgentPartyCnt As Integer, ByRef r_vResultArray(,) As Object, Optional ByVal v_vAdditionalDataArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim blCalledFromSTS As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            blCalledFromSTS = False

            For i As Integer = 0 To v_vAdditionalDataArray.GetUpperBound(1)
                Select Case v_vAdditionalDataArray(0, i)
                    Case CNCalledFromSTS

                        blCalledFromSTS = CBool(v_vAdditionalDataArray(1, i))
                        Exit For
                End Select
            Next i

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return result
            End If

            ' Setup the call to the database
            m_oDatabase.Parameters.Clear()

            ' agent_party_cnt
            m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_party_cnt", vValue:=CStr(v_lAgentPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter agent_party_cnt", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetProductByAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' source_id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter source_id", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetProductByAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Call the Database
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProductsSQL, sSQLName:=ACGetProductsName, bStoredProcedure:=ACGetProductsStored, vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to execute SQL : " & ACGetProductsSQL, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetProductByAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductByAgent Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetProductByAgent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetRiskByProduct(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lProductId As Integer, ByRef r_vResultArray As Object, Optional ByVal v_vAdditionalDataArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim blCalledFromSTS As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            blCalledFromSTS = False

            For i As Integer = 0 To v_vAdditionalDataArray.GetUpperBound(1)
                Select Case v_vAdditionalDataArray(0, i)
                    Case CNCalledFromSTS

                        blCalledFromSTS = CBool(v_vAdditionalDataArray(1, i))
                        Exit For
                End Select
            Next i

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return result
            End If

            With m_oDatabase
                'Clear the paramters
                .Parameters.Clear()

                'Add the ONLY Parameter
                lReturn = .Parameters.Add(sName:="Product_ID", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter 'Product_ID'", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetRiskByProduct", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                lReturn = .SQLSelect(sSQL:=ACGetRiskByProductSQL, sSQLName:=ACGetRiskByProductName, bStoredProcedure:=ACGetRiskByProductStored, vResultArray:=r_vResultArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process query " & ACGetRiskByProductName, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetRiskByProduct", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskByProduct Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetRiskByProduct", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Public Function GetQuotes(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lPartyCnt As Integer, ByRef r_vQuoteArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oSBOLink As bSIRIUSLink.SIRIUSLink
            Dim lReturn As Integer

            '    Set oSBOLink = CreateObject("bSIRIUSLink.SIRIUSLink")
            '
            '    lReturn = oSBOLink.Initialise(sUsername:=tosafestring(m_sUsername), _
            ''                                  sPassword:=tosafestring(m_sPassword), _
            ''                                  iUserID:=tosafeinteger(m_iUserID), _
            ''                                  iSourceID:=tosafeinteger(m_iSourceID), _
            ''                                  iLanguageID:=tosafestring(m_iLanguageID), _
            ''                                  iCurrencyID:=tosafeinteger(m_iCurrencyID), _
            ''                                  iLogLevel:=tosafeinteger(m_iLogLevel), _
            ''                                  sCallingAppName:=tosafestring(ACApp))

            ' Create bSiriusLink object

            oSBOLink = New bSIRIUSLink.SIRIUSLink
            lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotes Failed - Failed to create bSiriusLink object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuotes", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If


            lReturn = oSBOLink.GetQuotes(v_lPartyCnt:=v_lPartyCnt, r_vResults:=r_vQuoteArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotes Failed - bSiriusLink.GetQuotes method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuotes", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            oSBOLink.Dispose()
            oSBOLink = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotes Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuotes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetQuoteDetails(ByVal v_lInsuranceFileCnt As Integer, ByVal v_bUnderwriting As Boolean, ByRef r_vQuoteArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lReturn As Integer
            Dim sSQL As String = ""

            With m_oDatabase
                'Clear the paramters
                .Parameters.Clear()

                'Add the ONLY Parameter
                lReturn = .Parameters.Add(sName:="Insurance_File_Cnt", vValue:=ToSafeInteger(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter 'Insurance_File_Cnt'", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuoteDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' CTAF 20030716 - start
                If v_bUnderwriting Then
                    sSQL = ACGetQuoteDetailsSFUSQL
                    lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:=ACGetQuoteDetailsName, bStoredProcedure:=ACGetQuoteDetailsStored, vResultArray:=r_vQuoteArray)
                Else
                    'SJ 27/05/2004 - start
                    'Replace with sp
                    'sSQL = ACGetQuoteDetailsSBOSQL
                    lReturn = .SQLSelect(sSQL:=ACGetQuoteDetailsSBOSQL, sSQLName:=ACGetQuoteDetailsSBOName, bStoredProcedure:=ACGetQuoteDetailsSBOStored, vResultArray:=r_vQuoteArray)
                    'SJ 27/05/2004 - end
                End If
                ' CTAF 20030716 - end

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process query " & sSQL, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuoteDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End With

            ' CTAF 20030106 - Format dates properly
            If Informations.IsArray(r_vQuoteArray) Then


                r_vQuoteArray(4, 0) = CDate(r_vQuoteArray(4, 0)).ToString("yyyy-MM-dd HH:mm:ss")


                r_vQuoteArray(5, 0) = CDate(r_vQuoteArray(5, 0)).ToString("yyyy-MM-dd HH:mm:ss")


                r_vQuoteArray(6, 0) = CDate(r_vQuoteArray(6, 0)).ToString("yyyy-MM-dd HH:mm:ss")


                r_vQuoteArray(9, 0) = CDate(r_vQuoteArray(9, 0)).ToString("yyyy-MM-dd HH:mm:ss")
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteDetails Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuoteDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Public Function GetQuoteRisks(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vQuoteArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oInsFile As bSIRInsuranceFile.Services
            Dim oSBOLink As bSIRIUSLink.SIRIUSLink
            Dim oBusiness As bSIRRITax.Business
            Dim oComm As BSirAgentCommission.Business
            Dim lReturn, lScreenID, lUBnd As Integer
            Dim vResultArray(,) As Object
            Dim l_vBigQuoteArray(,) As Object
            Dim lQADim1, lQADim2, lRiskCnt As Integer
            Dim vRiskTaxArray, vAgentCommission As Object
            Dim sTaxesDescription As String = ""

            ' Create bSiriusLink object

            oSBOLink = New bSIRIUSLink.SIRIUSLink
            lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteRisks Failed - Failed to create bSiriusLink object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuoteRisks", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If


            lReturn = oSBOLink.GetQuoteRisks(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vQuoteArray:=r_vQuoteArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteRisks Failed - bSiriusLink.GetQuoteRisks method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuoteRisks", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' RDT - 27/02/2003
            ' Merge the resultarray with Taxation and Agent Commision details
            If Informations.IsArray(r_vQuoteArray) Then

                lQADim1 = r_vQuoteArray.GetUpperBound(0)
                lQADim2 = r_vQuoteArray.GetUpperBound(1)

                ReDim l_vBigQuoteArray(39, lQADim2)

                For lCnt1 As Integer = 0 To lQADim1
                    For lCnt2 As Integer = 0 To lQADim2


                        l_vBigQuoteArray(lCnt1, lCnt2) = r_vQuoteArray(lCnt1, lCnt2)
                    Next lCnt2
                Next lCnt1


                r_vQuoteArray = l_vBigQuoteArray
            End If

            'Create the bSIRRITax.Business object

            oBusiness = New bSIRRITax.Business
            lReturn = oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteRisks Failed to Initialise bSIRRITax.Business object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuoteRisks", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

            'Create the bSirAgentCommission.Business object

            oComm = New BSirAgentCommission.Business
            lReturn = oComm.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteRisks Failed to Initialise bSirAgentCommission.Business object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuoteRisks", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

            'Create the bSIRInsuranceFile.Services object

            oInsFile = New bSIRInsuranceFile.Services
            lReturn = oInsFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteRisks Failed to Initialise bSIRInsuranceFile.Services object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuoteRisks", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If
            lUBnd = r_vQuoteArray.GetUpperBound(1)

            For lCnt As Integer = 0 To lUBnd
                ' We are going to get the the Gis Screen Code and DataModel Code
                ' and replace element 9 and 10 in the result array,
                ' if we do not get a results we are NOT going to set the function as an error
                'AAB-12122002 - I am making a change, instead of replacing the screen ID which we will need
                '               I will replace elment #17, #18, which are insured_Item & extensions

                lScreenID = CInt(r_vQuoteArray(9, lCnt))

                lRiskCnt = CInt(r_vQuoteArray(1, lCnt))

                With m_oDatabase
                    'Clear the paramters
                    .Parameters.Clear()

                    'Add the ONLY Parameter
                    lReturn = .Parameters.Add(sName:="Screen_ID", vValue:=CStr(lScreenID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter 'Screen_ID'", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuoteRisks", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    End If

                    lReturn = .SQLSelect(sSQL:=ACGetRiskScreenDataModelCodeSQL, sSQLName:=ACGetRiskScreenDataModelCodeName, bStoredProcedure:=ACGetRiskScreenDataModelCodeStored, vResultArray:=vResultArray)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process query " & ACGetRiskScreenDataModelCodeName, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuoteRisks", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    End If

                End With

                If Informations.IsArray(vResultArray) Then


                    r_vQuoteArray(19, lCnt) = CStr(vResultArray(0, 0)).TrimEnd() ' Screen Code


                    r_vQuoteArray(20, lCnt) = CStr(vResultArray(1, 0)).TrimEnd() ' DataModel Code
                End If

                '        m_lReturn = oBusiness.SetProcessModes( _
                ''            vTask:=PMEdit, _
                ''            vNavigate:=0, _
                ''            vProcessMode:=110, _
                ''            vTransactionType:="NB", _
                ''            vEffectiveDate:=Now)
                '        If (m_lReturn <> PMTrue) Then
                '            ' Log Error Message
                '            LogMessageFile _
                ''                iType:=PMLogOnError, _
                ''                sMsg:="NBQuoteAfter Failed to SetProcessModes for bSIRRITax.Business object.", _
                ''                vApp:=tosafestring(ACApp), _
                ''                vClass:=ACClass, _
                ''                vMethod:="NBQuoteAfter", _
                ''                vErrNo:=Err.Number, _
                ''                vErrDesc:=Err.Description
                '        End If

                'Set the Business Object Properties
                ' We are only setting the RiskCnt property to ensure everything is done at a risk level.

                oBusiness.RiskCnt = lRiskCnt

                lReturn = oBusiness.GetRiskTax(r_vRiskTax:=vRiskTaxArray, r_sDescription:=sTaxesDescription)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRRITax.Business.GetRiskTax method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuoteRisks", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                End If

                If Informations.IsArray(vRiskTaxArray) Then

                    r_vQuoteArray(21, lCnt) = 0

                    For lCnt2 As Integer = 0 To vRiskTaxArray.GetUpperBound(1)

                        If Val(CStr(vRiskTaxArray(8, lCnt2))) = 0 Then
                            'Add to QuoteArray



                            r_vQuoteArray(22, lCnt) = Val(CStr(r_vQuoteArray(22, lCnt))) + Val(CStr(vRiskTaxArray(4, lCnt2))) ' Tax Amount


                            r_vQuoteArray(22, lCnt) = CStr(r_vQuoteArray(22, lCnt))
                        End If
                    Next lCnt2
                Else
                    ' Set to Zero if we did not get results back.

                    r_vQuoteArray(21, lCnt) = "0" ' Tax Percent

                    r_vQuoteArray(22, lCnt) = "0" ' Tax Amount
                End If

                If lCnt = 0 Then
                    ' Get the commission on the first risk only

                    lReturn = oComm.GetAgentCommission(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vntResult:=vAgentCommission)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSirAgentCommission.Business.GetAgentCommission method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuoteRisks", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    End If
                    If Informations.IsArray(vAgentCommission) Then


                        r_vQuoteArray(23, lCnt) = CStr(vAgentCommission(6, 0)).TrimEnd() ' Comm Amount

                        r_vQuoteArray(24, lCnt) = "" ' Comm
                    Else

                        r_vQuoteArray(23, lCnt) = "0" ' Comm Amount

                        r_vQuoteArray(24, lCnt) = "" ' Comm
                    End If


                    oInsFile.InsuranceFileCnt = v_lInsuranceFileCnt

                    lReturn = oInsFile.GetDetails

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRInsuranceFile.Services.GetDetails method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuoteRisks", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    End If


                    r_vQuoteArray(25, lCnt) = oInsFile.LeadAgent & ""
                Else
                    ' Set the value = 0

                    r_vQuoteArray(23, lCnt) = "0" ' Comm Amount

                    r_vQuoteArray(24, lCnt) = "" ' Comm

                    r_vQuoteArray(25, lCnt) = ""
                End If

            Next



            oSBOLink.Dispose()
            oSBOLink = Nothing


            oBusiness.Dispose()
            oBusiness = Nothing


            oComm.Dispose()
            oComm = Nothing


            oInsFile.Dispose()
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteRisks Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetQuoteRisks", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Public Function GetRatingDetails(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByRef r_vRatingSections As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oSBOLink As bSIRIUSLink.SIRIUSLink

            ' ***********************************************************************
            ' ********* UNDERWRITING START ******************************************
            ' ***********************************************************************

            ' Create bSiriusLink object

            oSBOLink = New bSIRIUSLink.SIRIUSLink
            m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRatingDetails Failed - Failed to create bSiriusLink object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetRatingDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            m_lReturn = oSBOLink.GetRatingDetails(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=v_lRiskCnt, r_vRatingSections:=r_vRatingSections)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRatingDetails Failed - bSiriusLink.GetRatingDetails method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetRatingDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            oSBOLink.Dispose()
            oSBOLink = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRatingDetails Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetRatingDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateGroupClient
    '
    ' Description:
    '
    ' History: 24/03/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateGroupClient(ByVal v_vPartyCnt As Object, Optional ByRef v_vPartyGroupTypeID As Object = Nothing, Optional ByRef v_vIsRegisteredCharity As Boolean = False, Optional ByRef v_vCharityNumber As Object = Nothing, Optional ByRef v_vNumberofMembers As Object = Nothing, Optional ByRef v_vSourceID As Object = Nothing, Optional ByRef v_vShortname As Object = Nothing, Optional ByRef v_vName As Object = Nothing, Optional ByRef v_vResolvedName As Object = Nothing, Optional ByRef v_vIsAlsoAgent As Object = Nothing, Optional ByRef v_vIsProspect As Object = Nothing, Optional ByRef v_vAgentCnt As Object = Nothing, Optional ByRef v_vConsultantCnt As Object = Nothing, Optional ByRef v_vFileCode As Object = Nothing, Optional ByRef v_vCurrencyId As Object = Nothing, Optional ByRef v_vPaymentMethodCode As Object = Nothing, Optional ByRef v_vReminderTypeId As Object = Nothing, Optional ByRef v_vAreaId As Object = Nothing, Optional ByRef v_vServiceLevelId As Object = Nothing, Optional ByRef v_vCreditCardCode As Object = Nothing, Optional ByRef v_vPaymentTermCode As Object = Nothing, Optional ByRef v_vCCJs As Object = Nothing, Optional ByRef v_vPartyID As Object = Nothing, Optional ByRef v_vSeasonalGiftID As Object = Nothing, Optional ByRef v_vCorrespondenceTypeId As Object = Nothing, Optional ByRef v_vRenewalStopCodeId As Object = Nothing, Optional ByRef v_vSwiftPartyID As Object = Nothing, Optional ByRef v_vLoyaltyNumber As Object = Nothing, Optional ByRef v_vAlternativeIdentifier As Object = Nothing, Optional ByRef v_vMarketingSegementInd As Object = Nothing, Optional ByRef v_vTradingName As Object = Nothing, Optional ByRef v_vSubBranchId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim vPartyGroupTypeID As Object
        Dim oGroup As bSIRPartyGC.Business
        Dim vIsRegisteredCharity As Byte
        Dim vCharityNumber As Byte
        Dim vNumberofMembers As Byte
        Dim vName, vResolvedName As Object



        result = gPMConstants.PMEReturnCode.PMTrue


        oGroup = New bSIRPartyGC.Business
        m_lReturn = oGroup.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateGroupClient Failed - Failed to create bSIRPartyGC.Business object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If


        m_lReturn = oGroup.GetDetails(vPartyCnt:=v_vPartyCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateGroupClient Failed. Failed on GetDetails", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateGroupClient", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If


        m_lReturn = oGroup.GetNext(vPartyCnt:=v_vPartyCnt, vIsRegisteredCharity:=vIsRegisteredCharity, vCharityNumber:=vCharityNumber, vNumberofMembers:=vNumberofMembers, vName:=vName, vResolvedName:=vResolvedName)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateGroupClient Failed. Failed on GetNext", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateGroupClient", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If


        If Not Informations.IsNothing(v_vPartyGroupTypeID) Then


            vPartyGroupTypeID = v_vPartyGroupTypeID
        End If


        If Not Informations.IsNothing(v_vIsRegisteredCharity) Then
            vIsRegisteredCharity = If(v_vIsRegisteredCharity, 1, 0)
        End If


        If Not Informations.IsNothing(v_vCharityNumber) Then
            vCharityNumber = 0
        End If


        If Not Informations.IsNothing(v_vNumberofMembers) Then
            vNumberofMembers = 0
        End If


        If Not Informations.IsNothing(v_vName) Then


            vName = v_vName
        End If


        If Not Informations.IsNothing(v_vResolvedName) Then


            vResolvedName = v_vResolvedName
        End If

        ' Call the update

        m_lReturn = oGroup.EditUpdate(lRow:=1, vPartyGroupTypeID:=vPartyGroupTypeID, vIsRegisteredCharity:=vIsRegisteredCharity, vCharityNumber:=vCharityNumber, vNumberofMembers:=vNumberofMembers, vName:=vName, vResolvedName:=vResolvedName)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateGroupClient Failed. Failed on EditUpdate", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateGroupClient", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If


        m_lReturn = oGroup.Update()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateGroupClient Failed. Failed on Update", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateGroupClient", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If


        Return result

    End Function

    ''' <summary>
    ''' Update Party details.
    ''' </summary>
    ''' <param name="v_sGisDataModelCode"></param>
    ''' <param name="v_sGisBusinessTypeCode"></param>
    ''' <param name="v_lPartyCnt"></param>
    ''' <param name="v_sForename"></param>
    ''' <param name="v_sSurname"></param>
    ''' <param name="v_sDateOfBirth"></param>
    ''' <param name="v_sEmailAddress"></param>
    ''' <param name="v_sCurrentRenewalDate"></param>
    ''' <param name="v_sAddress1"></param>
    ''' <param name="v_sAddress2"></param>
    ''' <param name="v_sAddress3"></param>
    ''' <param name="v_sAddress4"></param>
    ''' <param name="v_sPostcode"></param>
    ''' <param name="v_sTitle"></param>
    ''' <param name="v_sMaritalStatusCode"></param>
    ''' <param name="v_sGenderCode"></param>
    ''' <param name="v_sInitials"></param>
    ''' <param name="v_sTelephoneNumber"></param>
    ''' <param name="v_vAdditionalDataArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateParty(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String,
                                ByVal v_lPartyCnt As Object, Optional ByVal v_sForename As Object = Nothing,
                                Optional ByVal v_sSurname As Object = Nothing,
                                Optional ByVal v_sDateOfBirth As Object = Nothing, Optional ByVal v_sEmailAddress As Object = Nothing,
                                Optional ByVal v_sCurrentRenewalDate As Object = Nothing, Optional ByVal v_sAddress1 As Object = Nothing,
                                Optional ByVal v_sAddress2 As Object = Nothing, Optional ByVal v_sAddress3 As Object = Nothing,
                                Optional ByVal v_sAddress4 As Object = Nothing, Optional ByVal v_sPostcode As Object = Nothing,
                                Optional ByVal v_sTitle As Object = Nothing, Optional ByVal v_sMaritalStatusCode As Object = Nothing,
                                Optional ByVal v_sGenderCode As Object = Nothing, Optional ByVal v_sInitials As Object = Nothing,
                                Optional ByVal v_sTelephoneNumber As Object = Nothing,
                                Optional ByVal v_vAdditionalDataArray(,) As Object = Nothing,
                                Optional ByVal sAddress5 As Object = Nothing,
                                Optional ByVal sAddress6 As Object = Nothing,
                                Optional ByVal sAddress7 As Object = Nothing,
                                Optional ByVal sAddress8 As Object = Nothing,
                                Optional ByVal sAddress9 As Object = Nothing,
                                Optional ByVal sAddress10 As Object = Nothing) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oSBOLink As bSIRIUSLink.SIRIUSLink
        Dim lAgentCnt, lPartyTypeID, lPartyGroupID As Integer
        Dim bIsRegisteredCharity As Boolean
        Dim sCharityMembers As String = String.Empty
        Dim sGroupName As String = String.Empty
        Dim sContactName As String = String.Empty
        Dim sCompanyName As String = String.Empty
        Dim sCharityNumber As String = String.Empty
        Dim sPartyTypeCode As String = String.Empty
        Dim blCalledFromSTS As Boolean
        Dim sCountryCode As String = String.Empty
        Dim sTpUserCode As String = String.Empty
        Dim sTPIntroducer As String = String.Empty
        Dim sOccupationCode As String = String.Empty
        Dim sEmployerBusinessCode As String = String.Empty
        Dim sEmploymentStatusCode As String = String.Empty
        Dim sAlternativeId As String = String.Empty
        Dim sBusinessCode As String = String.Empty
        Dim sTradingName As String = String.Empty
        Dim sFileCode As String = String.Empty
        Dim oSIROrionUpdate As Object

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue
            blCalledFromSTS = False

            For i As Integer = 0 To v_vAdditionalDataArray.GetUpperBound(1)
                Select Case v_vAdditionalDataArray(0, i)
                    Case CNCalledFromSTS

                        blCalledFromSTS = CBool(v_vAdditionalDataArray(1, i))
                        Exit For
                End Select
            Next i

            ' If this call hasn't arisen from the STS layer then skip out.
            If Not blCalledFromSTS Then
                Return nResult
            End If

            ' Create bSiriusLink object

            oSBOLink = New bSIRIUSLink.SIRIUSLink
            nResult = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateParty Failed - Failed to create bSiriusLink object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            ' CTAF - Added the extra ones (all from AddParty)

            If Not Informations.IsNothing(v_vAdditionalDataArray) Then
                If Informations.IsArray(v_vAdditionalDataArray) Then
                    For i As Integer = 0 To v_vAdditionalDataArray.GetUpperBound(1)

                        Select Case v_vAdditionalDataArray(0, i)
                            Case ACLeadAgent

                                lAgentCnt = CInt(v_vAdditionalDataArray(1, i))

                            Case ACPartyTypeID

                                lPartyTypeID = CInt(v_vAdditionalDataArray(1, i))

                            Case ACPartyGroupID

                                lPartyGroupID = CInt(v_vAdditionalDataArray(1, i))

                            Case ACIsRegisteredCharity

                                bIsRegisteredCharity = CBool(v_vAdditionalDataArray(1, i))

                            Case ACNumberOfMembers

                                sCharityMembers = CStr(v_vAdditionalDataArray(1, i))

                            Case ACGroupName

                                sGroupName = CStr(v_vAdditionalDataArray(1, i))

                            Case ACContactName

                                sContactName = CStr(v_vAdditionalDataArray(1, i))

                            Case ACCompanyName

                                sCompanyName = CStr(v_vAdditionalDataArray(1, i))

                            Case ACCharityNumber

                                sCharityNumber = CStr(v_vAdditionalDataArray(1, i))

                            Case "party_type_code"

                                sPartyTypeCode = CStr(v_vAdditionalDataArray(1, i))

                            Case CNAddressCountry

                                sCountryCode = CStr(v_vAdditionalDataArray(1, i))

                            Case "TP_USER_CODE"

                                sTpUserCode = CStr(v_vAdditionalDataArray(1, i))

                            Case "TP_INTRODUCER"

                                sTPIntroducer = CStr(v_vAdditionalDataArray(1, i))

                            Case "OCCUPATION_CODE"

                                sOccupationCode = CStr(v_vAdditionalDataArray(1, i))

                            Case "EMPLOYER_BUSINESS_CODE"

                                sEmployerBusinessCode = CStr(v_vAdditionalDataArray(1, i))

                            Case "EMPLOYMENT_STATUS_CODE"

                                sEmploymentStatusCode = CStr(v_vAdditionalDataArray(1, i))

                            Case "ALTERNATIVE_ID"

                                sAlternativeId = CStr(v_vAdditionalDataArray(1, i))

                            Case "BUSINESS_CODE"

                                sBusinessCode = CStr(v_vAdditionalDataArray(1, i))

                            Case "TRADING_NAME"

                                sTradingName = CStr(v_vAdditionalDataArray(1, i))

                            Case "FILECODE"

                                sFileCode = CStr(v_vAdditionalDataArray(1, i))

                        End Select

                    Next i
                End If
            End If
            Select Case sPartyTypeCode
                Case Else
                    ' Update the party
                    nResult = UpdatePartySiriusLink(v_lPartyCnt:=CInt(v_lPartyCnt),
                                          v_sSurname:=CStr(v_sSurname), v_sForename:=v_sForename,
                                                      v_sAddress1:=v_sAddress1,
                                                      v_sAddress2:=v_sAddress2,
                                                      v_sAddress3:=v_sAddress3,
                                                      v_sAddress4:=v_sAddress4, v_sPostcode:=v_sPostcode, v_dDOB:=v_sDateOfBirth, v_sEMail:=CStr(v_sEmailAddress), v_dCurrInsRenewalDate:=CDate(v_sCurrentRenewalDate), v_sTitle:=CStr(v_sTitle), v_sMaritalStatusCode:=CStr(v_sMaritalStatusCode), v_sGenderCode:=v_sGenderCode, v_sInitials:=CStr(v_sInitials), v_sTelephoneNumber:=CStr(v_sTelephoneNumber),
                                                      v_lAgentCnt:=lAgentCnt, v_sContactName:=sContactName, v_sCountryCode:=sCountryCode, v_sTPUserCode:=sTpUserCode, v_sTPIntroducer:=sTPIntroducer, v_sOccupationCode:=sOccupationCode, v_sEmployerBusinessCode:=sEmployerBusinessCode, v_sEmploymentStatusCode:=sEmploymentStatusCode, v_sAlternativeID:=sAlternativeId, v_sBusinessCode:=sBusinessCode,
                                                      v_sTradingName:=sTradingName,
                                                      v_sFileCode:=sFileCode,
                                                      sAddress5:=sAddress5,
                                                      sAddress6:=sAddress6,
                                                      sAddress7:=sAddress7,
                                                      sAddress8:=sAddress8,
                                                      sAddress9:=sAddress9,
                                                      sAddress10:=sAddress10,
                                                      v_sPartyType:=sPartyTypeCode)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateParty Failed - bSiriusLink.UpdateParty method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If

            End Select
            oSBOLink.Dispose()
            oSBOLink = Nothing

            ' Do any extra updates that havent already been done

            Select Case sPartyTypeCode
                Case "GC"
                    nResult = CType(UpdateGroupClient(v_vPartyCnt:=v_lPartyCnt, v_vPartyGroupTypeID:=lPartyGroupID, v_vIsRegisteredCharity:=bIsRegisteredCharity, v_vCharityNumber:=sCharityNumber, v_vNumberofMembers:=sCharityMembers, v_vName:=sCompanyName, v_vResolvedName:=sGroupName), gPMConstants.PMEReturnCode)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateParty Failed - bSiriusLink.UpdateParty method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If

            End Select
            ' Create the business object
            nResult = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oSIROrionUpdate, v_sClassName:="bSIROrionUpdate.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateParty Failed - Failed to create reference to bSIROrionUpdate.Business.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            ' Update the account
            m_lReturn = oSIROrionUpdate.SiriusToOrion(v_lPartyCnt:=ToSafeInteger(v_lPartyCnt), v_iOldSourceId:=ToSafeInteger(m_iSourceID))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateParty Failed - bSIROrionUpdate.Business.SiriusToOrion method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateParty Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateParty", excep:=excep)

            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' Update the Party
    ''' </summary>
    ''' <param name="v_lPartyCnt"></param>
    ''' <param name="v_sSurname"></param>
    ''' <param name="v_sForename"></param>
    ''' <param name="v_sShortname"></param>
    ''' <param name="v_sAddress1"></param>
    ''' <param name="v_sAddress2"></param>
    ''' <param name="v_sAddress3"></param>
    ''' <param name="v_sAddress4"></param>
    ''' <param name="v_sPostcode"></param>
    ''' <param name="v_dDOB"></param>
    ''' <param name="v_sEMail"></param>
    ''' <param name="v_sUserID"></param>
    ''' <param name="v_sPassword"></param>
    ''' <param name="v_sMothersMaidenName"></param>
    ''' <param name="v_sTPUserCode"></param>
    ''' <param name="v_sTPIntroducer"></param>
    ''' <param name="v_sAQuestion"></param>
    ''' <param name="v_sTheAnswer"></param>
    ''' <param name="v_dMemorableDate"></param>
    ''' <param name="v_dCurrInsRenewalDate"></param>
    ''' <param name="v_sTitle"></param>
    ''' <param name="v_sMaritalStatusCode"></param>
    ''' <param name="v_sGenderCode"></param>
    ''' <param name="v_sInitials"></param>
    ''' <param name="v_sTelephoneNumber"></param>
    ''' <param name="v_bIsProspect"></param>
    ''' <param name="v_lAgentCnt"></param>
    ''' <param name="v_sContactName"></param>
    ''' <param name="v_sCountryCode"></param>
    ''' <param name="v_sOccupationCode"></param>
    ''' <param name="v_sEmployerBusinessCode"></param>
    ''' <param name="v_sEmploymentStatusCode"></param>
    ''' <param name="v_sAlternativeID"></param>
    ''' <param name="v_sBusinessCode"></param>
    ''' <param name="v_sTradingName"></param>
    ''' <param name="v_sFileCode"></param>
    ''' <param name="v_sPartyType"></param>
    ''' <param name="sAddress5"></param>
    ''' <param name="sAddress6"></param>
    ''' <param name="sAddress7"></param>
    ''' <param name="sAddress8"></param>
    ''' <param name="sAddress9"></param>
    ''' <param name="sAddress10"></param>
    ''' <returns></returns>
    Public Function UpdatePartySiriusLink(ByVal v_lPartyCnt As Integer, Optional ByVal v_sSurname As String = "",
                                          Optional ByVal v_sForename As Object = Nothing, Optional ByVal v_sShortname As Object = Nothing,
                                          Optional ByVal v_sAddress1 As Object = Nothing, Optional ByVal v_sAddress2 As Object = Nothing,
                                          Optional ByVal v_sAddress3 As Object = Nothing, Optional ByVal v_sAddress4 As Object = Nothing,
                                          Optional ByVal v_sPostcode As Object = Nothing, Optional ByVal v_dDOB As Object = Nothing,
                                          Optional ByVal v_sEMail As String = "", Optional ByVal v_sUserID As Object = Nothing,
                                          Optional ByVal v_sPassword As Object = Nothing, Optional ByVal v_sMothersMaidenName As Object = Nothing,
                                          Optional ByVal v_sTPUserCode As Object = Nothing, Optional ByVal v_sTPIntroducer As Object = Nothing,
                                          Optional ByVal v_sAQuestion As Object = Nothing,
                                          Optional ByVal v_sTheAnswer As Object = Nothing, Optional ByVal v_dMemorableDate As Date = #12/30/1899#,
                                          Optional ByVal v_dCurrInsRenewalDate As Date = #12/30/1899#, Optional ByVal v_sTitle As String = "",
                                          Optional ByVal v_sMaritalStatusCode As String = "", Optional ByVal v_sGenderCode As Object = Nothing,
                                          Optional ByVal v_sInitials As String = "", Optional ByVal v_sTelephoneNumber As String = "",
                                          Optional ByVal v_bIsProspect As Object = Nothing, Optional ByVal v_lAgentCnt As Integer = 0,
                                          Optional ByVal v_sContactName As String = "", Optional ByVal v_sCountryCode As String = "",
                                          Optional ByVal v_sOccupationCode As String = "", Optional ByVal v_sEmployerBusinessCode As String = "",
                                          Optional ByVal v_sEmploymentStatusCode As String = "", Optional ByVal v_sAlternativeID As String = "",
                                          Optional ByVal v_sBusinessCode As String = "", Optional ByVal v_sTradingName As Object = Nothing,
                                          Optional ByVal v_sFileCode As String = "", Optional ByVal v_sPartyType As String = "",
                                          Optional ByVal sAddress5 As Object = Nothing,
                                          Optional ByVal sAddress6 As Object = Nothing,
                                          Optional ByVal sAddress7 As Object = Nothing,
                                          Optional ByVal sAddress8 As Object = Nothing,
                                          Optional ByVal sAddress9 As Object = Nothing,
                                          Optional ByVal sAddress10 As Object = Nothing) As Integer

        Dim nResult As Integer = 0
        Try

            Dim aoContactArray(,) As Object
            Dim iContactIndex As Integer
            Dim oPMLookup As BPMLOOKUP.Business
            Dim oParty As bSIRParty.Services
            Dim nCountryId As Integer

            nResult = gPMConstants.PMEReturnCode.PMFalse
            oParty = New bSIRParty.Services
            m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPartySiriusLink Failed - Failed to Create bSIRParty object", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="FindParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            oParty.PartyCnt = v_lPartyCnt

            m_lReturn = oParty.GetDetails

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return nResult
            End If

            If Not Informations.IsNothing(v_sSurname) Then
                oParty.Name = v_sSurname
            Else
                v_sSurname = String.Empty
            End If

            If Not Informations.IsNothing(v_sForename) Then
                oParty.Forename = v_sForename
            Else
                v_sForename = String.Empty
            End If

            If Not Informations.IsNothing(v_sShortname) Then
                oParty.Shortname = v_sShortname
            Else
                v_sShortname = String.Empty
            End If

            If Not Informations.IsNothing(v_sInitials) Then
                oParty.Initials = v_sInitials
            Else
                v_sInitials = String.Empty
            End If

            If Not Informations.IsNothing(v_sTitle) Then
                oParty.PartyTitleCode = v_sTitle.Trim()
            Else
                v_sTitle = String.Empty
            End If

            If Informations.IsNothing(v_sTitle) Then
                v_sTitle = " "
            End If

            If Informations.IsNothing(v_sInitials) Then
                v_sInitials = " "
            End If

            If Informations.IsNothing(v_sSurname) Then
                v_sSurname = " "
            End If

            Dim sOptionValue As String = String.Empty

            If v_sPartyType = "PC" Then  ''v_iOptionNumber must be exist in system_options table with 'Enhanced Personal Client Resolved Name' description
                m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername,
                                                          v_sPassword:=m_sPassword,
                                                          v_iUserID:=m_iUserID,
                                                          v_iMainSourceID:=m_iSourceID,
                                                          v_iLanguageID:=m_iLanguageID,
                                                          v_iCurrencyID:=m_iCurrencyID,
                                                          v_iLogLevel:=m_iLogLevel,
                                                          v_sCallingAppName:=m_sCallingAppName,
                                                          v_iOptionNumber:=GeneralConst.kSystemOptionEnhancedResolvedName,
                                                          r_sOptionValue:=sOptionValue,
                                                          v_iSourceID:=1), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If sOptionValue <> String.Empty AndAlso sOptionValue = "1" Then
                oParty.ResolvedName = (v_sTitle.Trim() & " " & v_sForename.Trim() & " " & v_sSurname.Trim()).Trim()

            Else
                oParty.ResolvedName = v_sTitle.Trim() & " " & v_sInitials.Trim() & " " & v_sSurname.Trim()
            End If

            If Not Informations.IsNothing(v_sAddress1) Then
                oParty.Address1 = v_sAddress1
            End If

            If Not Informations.IsNothing(v_sAddress2) Then
                oParty.Address2 = v_sAddress2
            End If

            If Not Informations.IsNothing(v_sAddress3) Then
                oParty.Address3 = v_sAddress3
            End If

            If Not Informations.IsNothing(v_sAddress4) Then
                oParty.Address4 = v_sAddress4
            End If

            If Not Informations.IsNothing(v_sPostcode) Then
                oParty.PostalCode = v_sPostcode
            End If

            If Not Informations.IsNothing(sAddress5) Then
                oParty.Address5 = sAddress5
            End If

            If Not Informations.IsNothing(sAddress6) Then
                oParty.Address6 = sAddress6
            End If

            If Not Informations.IsNothing(sAddress7) Then
                oParty.Address7 = sAddress7
            End If

            If Not Informations.IsNothing(sAddress8) Then
                oParty.Address8 = sAddress8
            End If

            If Not Informations.IsNothing(sAddress9) Then
                oParty.Address9 = sAddress9
            End If

            If Not Informations.IsNothing(sAddress10) Then
                oParty.Address10 = sAddress10
            End If

            oPMLookup = New BPMLOOKUP.Business
            m_lReturn = oPMLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePartySiriusLink Failed - Failed to Create bPMLookup object", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddPartySiriusLink", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            nCountryId = 0

            m_lReturn = oPMLookup.GetEffectiveIDFromCode(v_sTableName:=gPMConstants.PMLookupCountry, v_sCode:=v_sCountryCode, v_dtEffectiveDate:=DateTime.Now, r_lID:=nCountryId)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to PMLookup.GetEffectiveIDFromCode Failed - Failed to find entry in Country Lookup list for code - " & v_sCountryCode, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddPartySiriusLink", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

            oParty.CountryId = nCountryId

            oPMLookup.Dispose()

            oPMLookup = Nothing

            If Not Informations.IsNothing(v_dDOB) Then
                oParty.DateOfBirth = v_dDOB
            End If

            If Not Informations.IsNothing(v_sUserID) Then
                oParty.UserID = v_sUserID
            End If

            If Not Informations.IsNothing(v_sPassword) Then
                oParty.Password = v_sPassword
            End If

            If Not Informations.IsNothing(v_sMothersMaidenName) Then
                oParty.MothersMaidenName = v_sMothersMaidenName
            End If

            If Not Informations.IsNothing(v_sTPUserCode) Then
                oParty.TPUserCode = v_sTPUserCode
            End If

            If Not Informations.IsNothing(v_sTPIntroducer) Then
                oParty.TPIntroducerCode = v_sTPIntroducer
            End If

            If Not Informations.IsNothing(v_sAQuestion) Then
                oParty.AQuestion = v_sAQuestion
            End If

            If Not Informations.IsNothing(v_sTheAnswer) Then
                oParty.TheAnswer = v_sTheAnswer
            End If

            If Not Informations.IsNothing(v_dMemorableDate) Then
                If Informations.IsDate(v_dMemorableDate) Then
                    oParty.MemorableDate = Informations.DateSerial(v_dMemorableDate.Year, v_dMemorableDate.Month, v_dMemorableDate.Day)
                End If
            End If

            If Not Informations.IsNothing(v_dCurrInsRenewalDate) Then
                If Informations.IsDate(v_dCurrInsRenewalDate) Then
                    oParty.CurrInsRenewalDate = Informations.DateSerial(v_dCurrInsRenewalDate.Year, v_dCurrInsRenewalDate.Month, v_dCurrInsRenewalDate.Day)
                End If
            End If

            If Not Informations.IsNothing(v_sMaritalStatusCode) Then
                oParty.MaritalStatusCode = v_sMaritalStatusCode.Trim()
            End If

            If Not Informations.IsNothing(v_sGenderCode) Then
                oParty.GenderCode = v_sGenderCode
            End If
            oParty.OccupationCode = v_sOccupationCode
            oParty.EmployerBusinessCode = v_sEmployerBusinessCode
            oParty.EmploymentStatusCode = v_sEmploymentStatusCode
            oParty.AlternativeIdentifier = v_sAlternativeID
            oParty.PartyBusinessId = v_sBusinessCode

            If Not Informations.IsNothing(v_sTradingName) Then
                oParty.TradingName = v_sTradingName
            End If

            If Not Informations.IsNothing(v_sEMail) Then
                If v_sEMail.Trim() <> "" Then
                    ReDim aoContactArray(4, iContactIndex)

                    aoContactArray(0, iContactIndex) = "E-MAIL"

                    aoContactArray(1, iContactIndex) = ""

                    aoContactArray(2, iContactIndex) = v_sEMail

                    aoContactArray(3, iContactIndex) = ""

                    aoContactArray(4, iContactIndex) = "Email"

                    iContactIndex += 1
                End If
            End If

            If Not Informations.IsNothing(v_sTelephoneNumber) Then
                If v_sTelephoneNumber.Trim() <> "" Then
                    If iContactIndex Then
                        ReDim Preserve aoContactArray(4, iContactIndex)
                    Else
                        ReDim aoContactArray(4, iContactIndex)
                    End If

                    aoContactArray(0, iContactIndex) = "TELEPHONE"

                    aoContactArray(1, iContactIndex) = ""

                    aoContactArray(2, iContactIndex) = v_sTelephoneNumber

                    aoContactArray(3, iContactIndex) = ""

                    aoContactArray(4, iContactIndex) = "TelNo"

                    iContactIndex += 1
                End If
            End If

            If Not Informations.IsNothing(v_sContactName) Then
                If v_sContactName.Trim() <> "" Then
                    If iContactIndex Then
                        ReDim Preserve aoContactArray(4, iContactIndex)
                    Else
                        Dim ContactArray(4, iContactIndex) As Object
                    End If

                    aoContactArray(0, iContactIndex) = ACContactTypeMain

                    aoContactArray(1, iContactIndex) = ""

                    aoContactArray(2, iContactIndex) = v_sTelephoneNumber

                    aoContactArray(3, iContactIndex) = ""

                    aoContactArray(4, iContactIndex) = v_sContactName

                    iContactIndex += 1
                End If
            End If

            oParty.ContactArray = aoContactArray

            If Not Informations.IsNothing(v_lAgentCnt) Then
                If v_lAgentCnt <> 0 Then

                    oParty.AgentCnt = v_lAgentCnt
                End If
            End If

            If Not Informations.IsNothing(v_bIsProspect) Then
                oParty.IsProspect = v_bIsProspect
            End If

            oParty.FileCode = v_sFileCode

            m_lReturn = oParty.UpdateParty

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMTrue
            End If

            oParty.Dispose()
            oParty = Nothing

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePartySiriusLink Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdatePartySiriusLink", excep:=excep)

            Return nResult
        End Try
    End Function


    Public Function ProcessAccounts(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sTransactionType As String, Optional ByVal v_bMTAInstallments As Boolean = False, Optional ByVal v_sCancelRefundAmt As String = "", Optional ByVal v_bRenewalInstallments As Boolean = False, Optional ByVal v_iDebitAgainst As Integer = 0, Optional ByVal v_lPaymentAccountId As Integer = 0, Optional ByRef r_vTransactionArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oPFBusiness As Object
            Dim oBusiness As bControlTrans.Automated
            Dim oPFInterface As Object
            Dim lReturn As Integer
            Dim sFailureReason As String = ""
            Dim cThisPremium As Decimal
            Dim vPlanArray As Object
            Dim sRefund As String = ""
            Dim lOldInsuranceFileCnt As Object
            Dim sErrMsg As String = ""

            'Create the Transaction object

            oBusiness = New bControlTrans.Automated
            lReturn = oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccounts Failed - Failed to create bControlTrans.Automated object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Set the Insurance file count

            oBusiness.InsuranceFileCnt = v_lInsuranceFileCnt


            lReturn = oBusiness.GetThisPremium(cThisPremium)

            If cThisPremium = 0 Then
                If v_sTransactionType = "NB" Then
                    bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccounts Failed - bControlTrans.Automated.GetThisPremium returned premium = zero for NB.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                End If

                oBusiness.Dispose()
                oBusiness = Nothing
                Return result
            End If

            Select Case v_sTransactionType
                Case "NB", "PFNB"

                    lReturn = oBusiness.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeNBLive, vTransactionType:=v_sTransactionType, vEffectiveDate:=DateTime.Now)
                Case "MTA"

                    lReturn = oBusiness.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeMTAQuote, vTransactionType:=v_sTransactionType, vEffectiveDate:=DateTime.Now)
            End Select

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn


                oBusiness.Dispose()
                oBusiness = Nothing
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccounts Failed - bControlTrans.Automated.SetProcessModes method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result

            End If


            lReturn = oBusiness.Start(iPaymentAccountId:=v_lPaymentAccountId, iDebitAgainst:=v_iDebitAgainst)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccounts Failed - bControlTrans.Automated.Start method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            '    'Update the PF Commission TransactionID in the PFPremiumFinance Table
            '    lReturn& = oBusiness.UpdatePFCommissionTransactionID(v_lInsuranceFileCnt)
            '    If (lReturn& <> PMTrue) Then
            '        ProcessAccounts = lReturn&
            '        ' Log Error Message
            '        LogMessage sUsername:=tosafestring(m_sUsername), _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="ProcessAccounts Failed - bControlTrans.Automated.UpdatePFCommissionTransactionID method failed.", _
            ''            vApp:=tosafestring(ACApp), _
            ''            vClass:=ACClass, _
            ''            vMethod:="ProcessAccounts", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '        Exit Function
            '    End If


            'Get PF Transactions for going through the Navigator
            ' I am not sure about the TransactionArray if it needs to be populated first.

            lReturn = oBusiness.GetPFTransactions(ToSafeInteger(v_lInsuranceFileCnt), r_vTransactionArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccounts Failed - bControlTrans.Automated.GetPFTransactions method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If v_sTransactionType <> "NB" Then
                ' Create object for bSIRPremiumFinance
                'Thinh Nguyen 19/02/2002 - bSIRPremiumFinance.Business instead of bSIRPremFinance.Business
                lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oPFBusiness, v_sClassName:="bSIRPremiumFinance.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccounts Failed - Failed to create bControlTrans.Automated object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If

            ' CHECK MTA
            ' Get Original InsuranceFileCnt
            ' Use GetSingleFinancePlan Plan Array from old File
            ' Use above m_vTransactionArray to do Process MTA
            ' If fails - msgbox to use with reason and then offer NB route on Roadmap
            If v_sTransactionType = "MTA" Then
                'Thinh Nguyen 01/03/2002 (start) - get last version of policy which has instalments
                'm_lReturn = m_oBusiness.GetPreviousInsuranceFile(m_lInsuranceFileCnt, lOldInsuranceFileCnt)

                lReturn = oBusiness.GetPlanInsuranceFile(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lPlanInsuranceFileCnt:=lOldInsuranceFileCnt)
                'Thinh Nguyen 01/03/2002 (end) - get last version of policy which has instalments

                If lReturn = gPMConstants.PMEReturnCode.PMTrue Then


                    lReturn = oPFBusiness.GetSingleFinancePlan(DBNull.Value, DBNull.Value, vPlanArray, ToSafeInteger(lOldInsuranceFileCnt))
                    'Check to see if a plan exists
                    If lReturn = gPMConstants.PMEReturnCode.PMTrue And Informations.IsArray(vPlanArray) Then
                        'If MsgBox("Do You Want To Pay This Adjustment By Instalments", vbQuestion + vbYesNo, "Instalments") = vbYes Then
                        If v_bMTAInstallments Then
                            'Now run the Instalments MTA by passing the existing plan
                            'and new MTA Transactions

                            lReturn = oPFBusiness.ProcessMTA(vPlanArray, r_vTransactionArray, ToSafeInteger(v_lInsuranceFileCnt))

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                sErrMsg = "The MTA cannot be processed on the Instalment Plan because" & Strings.ChrW(13) & Strings.ChrW(10)
                                If lReturn = 9 Then
                                    sErrMsg = sErrMsg & "the MTA amount is below the minimum amount."
                                ElseIf lReturn = 99 Then
                                    sErrMsg = sErrMsg & "there are not enough instalments left to spread the MTA over."
                                    'Thinh Nguyen 28/02/2002 (start)
                                ElseIf lReturn = 998 Then
                                    sErrMsg = sErrMsg & "the return premium is greater than the outstanding value on the instalment plan."
                                ElseIf lReturn = 9999 Then
                                    sErrMsg = sErrMsg & "no finance rate available."

                                ElseIf lReturn = 10 Then
                                    sErrMsg = "Cannot process return premium." & Strings.ChrW(13) & Strings.ChrW(10)
                                    'Thinh Nguyen 28/02/2002 (end)

                                ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = lReturn
                                    ' Log Error Message
                                    bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccounts Failed - bSIRPremiumFinance.Business.ProcessMTA method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                    Return result
                                End If

                                'Tell the user what went wrong
                                'how are going to do that?
                                'MsgBox sErrMsg & vbCrLf & vbCrLf & "Please collect the MTA Amount manually.", vbExclamation, "Instalments MTA Failed"
                            End If
                        End If
                    ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = lReturn
                        ' Log Error Message
                        bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccounts Failed - bSIRPremiumFinance.Business.GetPlanInsuranceFile method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If
                End If

                'Reverse the old stats postings

                lReturn = oBusiness.ReverseStats(lOldInsuranceFileCnt)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccounts Failed - bControlTrans.Automated.ReverseStats method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If

            ' CHECK CANCEL
            ' Use GetSingleFinancePlan to see if Plan exists
            ' Prompt user for any refund default = 0
            ' Use CancelPlanInHouse
            If v_sTransactionType = "CANCEL" Then


                lReturn = oPFBusiness.GetSingleFinancePlan(DBNull.Value, DBNull.Value, vPlanArray, ToSafeInteger(v_lInsuranceFileCnt))
                'Check to see if a plan exists
                If lReturn = gPMConstants.PMEReturnCode.PMTrue And Informations.IsArray(vPlanArray) Then
                    'sRefund = InputBox("This Policy is on an Instalment Plan. Please enter the refund due for this plan or leave as zero.", _
                    '"Instalment Plan Refund", 0)

                    ' Cancel the Instalment Plan

                    lReturn = oPFBusiness.CancelPlanInHouse(vPlanArray, Val(v_sCancelRefundAmt))
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = lReturn
                        ' Log Error Message
                        bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccounts Failed - bSIRPremiumFinance.Business.CancelPlanInHouse method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If
                Else
                    result = lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccounts Failed - bSIRPremiumFinance.Business.GetSingleFinancePlan method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If

            ' CHECK RENEWAL
            ' See if Plan exists
            ' Convert Plan to live, passing over transactions
            ' Transact it through Orion
            If v_sTransactionType = "RENEWAL" Then


                lReturn = oPFBusiness.GetSingleFinancePlan(DBNull.Value, DBNull.Value, vPlanArray, ToSafeInteger(v_lInsuranceFileCnt))
                'Check to see if a plan exists
                If lReturn = gPMConstants.PMEReturnCode.PMTrue And Informations.IsArray(vPlanArray) Then
                    'If MsgBox("Do You Want To Pay By Instalments", vbQuestion + vbYesNo, "Instalments") = vbYes Then
                    If v_bRenewalInstallments Then
                        'Convert the Plan to Live

                        lReturn = oPFBusiness.TranslateQuoteToPlan(vPlanArray, r_vTransactionArray)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = lReturn
                            ' Log Error Message
                            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccounts Failed - bSIRPremiumFinance.Business.GetSingleFinancePlan method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            Return result
                        End If

                        'If the Plan doesn't have any Bank details or is not live
                        'then get the user to enter them


                        Do While CStr(vPlanArray(46, 0)).Trim() = "" Or CStr(vPlanArray(67, 0)) <> "040"
                            'Question should we do something different here?

                            '                    m_lReturn = g_oObjectManager.GetInstance(oPFInterface, "iPMBFinancePlanMaint.Interface", PMGetLocalInterface)
                            '                    If m_lReturn <> PMTrue Then
                            '                        ProcessInterface = PMFalse
                            '                        Exit Function
                            '                    End If
                            '
                            '                    'Open the Maintenance Form for Bank details
                            '
                            '                    'Thinh Nguyen 01/03/2002 (start)
                            '                    'oPFInterface.TransactionType = "NB" 'Treat as New Business
                            '                    m_lReturn = oPFInterface.SetProcessModes(vTransactionType:="NB")
                            '
                            '                    If m_lReturn <> PMTrue Then
                            '                        m_lReturn = oPFInterface.Terminate()
                            '                        Set oPFInterface = Nothing
                            '
                            '                        ProcessInterface = PMFalse
                            '                        Exit Function
                            '                    End If
                            '                    'Thinh Nguyen 01/03/2002 (end)
                            '
                            '                    oPFInterface.FinancePlanCnt = vPlanArray(0, 0)
                            '                    oPFInterface.FinancePlanVersion = vPlanArray(1, 0)
                            '                    oPFInterface.Spawned = True
                            '                    m_lReturn = oPFInterface.Start()
                            '                    If m_lReturn <> PMTrue Then
                            '                        ProcessInterface = PMFalse
                            '                        Exit Function
                            '                    End If
                            '
                            '                    'Clean up
                            '                    oPFInterface.Terminate
                            '                    Set oPFInterface = Nothing
                            '
                            '                    'Re-load the Finance Plan Array
                            '                    m_lReturn = oPFBusiness.GetSingleFinancePlan(Null, Null, vPlanArray, m_lInsuranceFileCnt)
                            '                    If m_lReturn <> PMTrue Then
                            '                        ProcessInterface = PMFalse
                            '                        Exit Function
                            '                    End If
                        Loop
                    End If
                End If
            End If

            If v_sTransactionType <> "NB" Then

                oPFBusiness.Dispose()
                oPFBusiness = Nothing
            End If


            oBusiness.Dispose()
            oBusiness = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccounts Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function



    Public Function GetSchemeList(ByRef r_vResultArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim obSIRSchemeSelect As Object
            Dim lReturn As Integer

            ' Create bSiriusLink object
            lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=obSIRSchemeSelect, v_sClassName:="bSIRSchemeSelect.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSchemeList Failed - Failed to create obSIRSchemeSelect object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetSchemeList", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            lReturn = obSIRSchemeSelect.GetProduct(r_vResultArray:=r_vResultArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSchemeList Failed - obSIRSchemeSelect.GetProduct method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetSchemeList", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            obSIRSchemeSelect.Dispose()
            obSIRSchemeSelect = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSchemeList Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetSchemeList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
