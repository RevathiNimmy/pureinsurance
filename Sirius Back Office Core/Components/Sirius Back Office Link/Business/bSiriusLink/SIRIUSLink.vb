Option Strict Off
Option Explicit On
Imports System.Globalization
'developer guide no. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("SIRIUSLink_NET.SIRIUSLink")> _
Public NotInheritable Class SIRIUSLink

    Implements IDisposable
    ' ************************************************
    ' Added to replace global variables 27/10/2003
    ' Username.
    Private m_sUserName As String = ""

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


    Private Const ACClass As String = "SIRIUSLink"

    'SIRIUS ID constants
    'Logon details
    Private Const SIRIUS_USERNAME As String = "sirius"
    Private Const SIRIUS_PASSWORD As String = "XceqMUbg"
    Private Const SIRIUS_USERID As Integer = 1
    Private Const SIRIUS_SOURCEID As Integer = 1
    Private Const SIRIUS_LANGUAGEID As Integer = 1
    Private Const SIRIUS_CURRENCYID As Integer = 26

    '**** Added By: AAB  -  Added On:  19-Aug-2002 16:16 ****
    '**** Added LogLevel Constant
    Private Const SIRIUS_LOGLEVEL As Integer = 4
    Private Const SIRIUS_AGENT_TYPE_CODE As String = "AG"

    'Insurance File Types
    Private Const INSURANCE_FILE_TYPE_QUOTE_ID As Integer = 1
    Private Const INSURANCE_FILE_TYPE_POLICY_ID As Integer = 2

    'Policy Types
    Private Const POLICY_TYPE_GNETMOTOR_CODE As String = "GNET MOTOR"
    Private Const POLICY_TYPE_UNDERWRITING_CODE As String = "UNDERWRITE"

    'Insurance file structure
    Private Const INSURANCE_FILE_STRUCTURE_GEMINI_ID As Integer = 2
    Private Const INSURANCE_FILE_STRUCTURE_GEMINI_CODE As String = "GEM"

    'Insurance File Statuses
    Private Const INSURANCE_FILE_STATUS_CANCELLED As String = "CAN"
    Private Const INSURANCE_FILE_STATUS_LAPSED As String = "LAP"

    Private Const POLICY_RENEWAL_FREQUENCY As String = "ANNUAL"
    Private Const AGENTS_ONLINE_DATAMODEL_CODE As String = "AOL"

    'Party, Find Party, Insurance File and Find Insurance instances
    Private m_oParty As bSIRParty.Services 'bSIRParty.Services
    Private m_oQuote As bSIRInsuranceFile.Services 'bSIRInsuranceFile.Services
    Private m_oFindInsurance As bSIRFindInsurance.Form 'bSIRFindInsurance.Form
    Private m_oInsuranceFileBusiness As bSIRInsuranceFile.Business 'bSIRInsuranceFile.Business
    Private m_oFindParty As bSIRFindParty.Business 'bSIRFindParty.Business

    '**** Added By: AAB  -  Added On:  16-Aug-2002 16:17 ****
    '**** Added PMUser instance
    Private m_oUser As Object

    'PF051101 - Added database property for SFU Risk updates
    Private m_oDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean

    'PF261001 - Added GIS codes as properties for functions that may be interested
    Private m_sGisBusinessTypeCode As String = ""
    Private m_sGisDataModelCode As String = ""


    'General Return value used in class functions
    Private m_lReturn As Integer

    'Enum for Insurance File Types
    Public Enum InsuranceFileType
        IFTQuote = 1
        IFTPolicy = 2
        IFTRenewal = 3
        IFTMTAQuote = 4
        IFTMTAPerm = 5
        IFTMTATemp = 6
        IFTMTAQuoteTemp = 7
        IFTMTAQuoteCancel = 8
        IFTMTAQuoteReInstate = 9
        IFTMTAPermCancel = 10 ' CL191000
    End Enum

    Private IFTCodes(9) As Object

    'RJG 21/06/2000 - This enum is also in bSIRFindInsurance.  It has to be placed
    'here so that calling gemini net apps can get to it.
    Public Enum InsuranceFileSearchType
        IFSTQuote = 1
        IFSTPolicy = 2
        IFSTRenewal = 3
        IFSTQuotePolicy = 4
        IFSTQuotePolicyRenewal = 5
        IFSTMTAQuote = 6
        IFSTMTAQuoteMTATempQuote = 7
    End Enum

    'RJG 08/08/2000 - Enum for policy to return
    Public Enum ReturnedPolicy
        RPOriginal = 1
        RPEffective = 2
        RPLatest = 3
    End Enum


    Public Function GetRatingDetails(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByRef r_vRatingSections As Object) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        '******************************************************************************
        '        Function Name:  GetRatingDetails
        '******************************************************************************
        '           Created By:  Ahmed "Jay" Bishtawi
        '           Created On:  19-Sep-2002
        '******************************************************************************
        '       Parameters Are:
        '                        (In)     - v_lInsuranceFolderCnt - Long     -
        '                        (In)     - v_lInsuranceFileCnt   - Long     -
        '                        (In)     - v_lRiskCnt            - Long     -
        '                        (In/Out) - r_vRatingSections     - Variant  -
        '
        ' Return Value Type Is:  Long -
        '******************************************************************************
        ' Function Description:  This function returns the rating section in an array
        '                        based onvariables passed in.
        '******************************************************************************
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oPerilAllocation As bSirPerilAllocation.Business = Nothing
        'AAB-16-Oct-2002 14:39 - To add a clearer error message...
        Dim sErrorMessage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oPerilAllocation = New bSirPerilAllocation.Business()
            'AAB-16-Oct-2002 14:01 - Added Error Trapping
            If oPerilAllocation Is Nothing Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, GetRatingDetails Failed to create bSIRPerilAllocation.Business object")
            End If


            lReturn = CType(oPerilAllocation.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=CShort(m_iUserID), iSourceID:=CShort(m_iSourceID), iLanguageID:=CShort(m_iLanguageID), iCurrencyID:=CShort(m_iCurrencyID), iLogLevel:=CShort(m_iLogLevel), sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AAB - 16-Oct-2002 09:37 - Added LogMesseage Statement
                result = lReturn
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, bSIRPerilAllocation.Business object failed to initialize")
            End If

            'Set the Business Object Properties
            oPerilAllocation.InsuranceFolderCnt = v_lInsuranceFolderCnt
            oPerilAllocation.InsuranceFileCnt = v_lInsuranceFileCnt
            oPerilAllocation.RiskID = v_lRiskCnt

            'Get the values of Rating section from the database
            lReturn = CType(oPerilAllocation.GetRatingSections(vResultArray:=r_vRatingSections), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AAB - 16-Oct-2002 09:40 - Added LogMessage m_sUsername, Statement
                result = lReturn
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, bSIRPerilAllocation Business.GetRatingSections Method Failed")
            End If

            'Destroy the object.
            oPerilAllocation.Dispose()
            oPerilAllocation = Nothing

            Return result

        Catch excep As System.Exception



            'AAB-16-Oct-2002 14:00 - Added for a clearer error message
            If Informations.Err().Number = gPMConstants.Constants.vbObjectError Then
                sErrorMessage = excep.Message
            Else
                result = gPMConstants.PMEReturnCode.PMError
                sErrorMessage = "GetRatingDetails Failed"
            End If

            'Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage, sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetRatingDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            'AAB-16-Oct-2002 14:00 - Destroy the object if needed
            If Not (oPerilAllocation Is Nothing) Then
                oPerilAllocation.Dispose()
                oPerilAllocation = Nothing
            End If

            Return result
        End Try
    End Function
    '****   END CHANGES - Changed By: AAB  - Changed On: 19-Sep-2002 16:19   ****
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property


    ' ***************************************************************** '
    ' Name: GIS Codes
    ' Date: 26/10/2001
    ' Description: Exposes GIS codes as writeable properties for functions
    '              that may be interested.
    '
    ' PF 261001 - Created
    ' ***************************************************************** '
    Public WriteOnly Property GISBusinessTypeCode() As String
        Set(ByVal Value As String)
            m_sGisBusinessTypeCode = Value
        End Set
    End Property

    Public WriteOnly Property GISDataModelCode() As String
        Set(ByVal Value As String)
            m_sGisDataModelCode = Value
        End Set
    End Property
    'developer guide no 17. 
    Public Function AddParty(ByVal v_sSurname As String, ByVal v_sForename As String, ByVal v_sPartyType As String, ByVal v_sAddress1 As String, ByVal v_sAddress2 As String, ByVal v_sAddress3 As String, ByVal v_sAddress4 As String, ByVal v_sPostCode As String, ByVal v_dDOB As Date, ByVal v_sEMail As String, ByVal v_sUserID As String, ByVal v_sPassword As String, ByRef r_lPartyCnt As Integer, ByRef r_sShortName As String, Optional ByVal v_sMothersMaidenName As Object = Nothing, Optional ByVal v_sTPUserCode As Object = Nothing, Optional ByVal v_sTPIntroducer As Object = Nothing, Optional ByVal v_sAQuestion As Object = Nothing, Optional ByVal v_sTheAnswer As Object = Nothing, Optional ByVal v_dMemorableDate As Object = Nothing, Optional ByVal v_dCurrInsRenewalDate As Object = Nothing, Optional ByVal v_sTitle As Object = Nothing, Optional ByVal v_sMaritalStatusCode As Object = Nothing, Optional ByVal v_sGenderCode As Object = Nothing, Optional ByVal v_sInitials As Object = Nothing, Optional ByVal v_sTelephoneNumber As Object = Nothing, Optional ByVal v_lAgentCnt As Object = Nothing, Optional ByVal v_bUseDefaultShortName As Boolean = False, Optional ByVal v_bIsProspect As Object = 1, Optional ByVal v_lSourceID As Object = 1, Optional ByVal v_iShortNameFormat As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        ' ***************************************************************** '
        ' Name: AddParty
        ' Date: 17/05/2000
        ' Description:  Adds a new party record
        '
        ' RFC050900 - Added Title & MaritalStatusCode optional parameters.
        ' RJG011200 - Added Lead Agent Cnt optional parameter.
        ' ***************************************************************** '

        Try

            Dim vContactArray(,) As Object = Nothing
            Dim iContactIndex As Integer
            Dim sEncryptedPassword As String = ""

            result = gPMConstants.PMEReturnCode.PMFalse


            'developer guide no. 206
            TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "Start", ACApp, ACClass, "RegisterUser")

            m_oParty = New bSIRParty.Services()

            'LogMessageFile PMLogOnError, "before m_oParty.Initialise", ACApp, ACClass, "RegisterUser"

            ' CJB 21/02/02 Allow usage of previous short name generation before the 19/06/2001 change was done
            ' PWF 19/06/2001 Allow usage of back office style short name
            ' GRW 08/01/01 Allow usage of default short name generation

            If Not v_bUseDefaultShortName Then
                ' Generate a User ID (Short name) for the Party

                If v_iShortNameFormat = 0 Then
                    m_lReturn = GenerateBackOfficeShortName(v_sSurname:=v_sSurname, v_sInitials:=v_sInitials, r_sUserID:=r_sShortName)
                Else
                    m_lReturn = GenerateShortName(r_sUserID:=r_sShortName)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            m_lReturn = m_oParty.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            'RJG 18/05/2000 - Quit the function if the Party object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then

                'developer guide no. 206
                TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "m_oParty.Initialise Failed", ACApp, ACClass, "RegisterUser")
                Return result
            End If

            'LogMessageFile PMLogOnError, "before Encrypt", ACApp, ACClass, "RegisterUser"

            'RJG 13/06/2000 - Encrypt the password
            m_lReturn = bPMFunc.Encrypt(sPassword:=v_sPassword, sEncryptedPassword:=sEncryptedPassword)

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then

                'developer guide no. 206
                TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "Encrypt Failed", ACApp, ACClass, "RegisterUser")
                Return result
            End If

            'RJG 17/05/2000 - Set the necessary properties for the party object
            m_oParty.Name = ToSafeInteger(v_sSurname)
            m_oParty.Forename = ToSafeInteger(v_sForename)

            ' GRW 08/01/01 Allow usage of default short name generation
            If Not v_bUseDefaultShortName Then
                ' RFC110800 - Generate a Shortname
                m_oParty.Shortname = ToSafeInteger(r_sShortName)
            End If
            m_oParty.PartyType = v_sPartyType
            m_oParty.Address1 = v_sAddress1
            m_oParty.Address2 = v_sAddress2
            m_oParty.Address3 = v_sAddress3
            m_oParty.Address4 = v_sAddress4
            m_oParty.PostalCode = v_sPostCode

            ' PWF 19/06/2001 Allow usage of back office style resolved name
            m_oParty.ResolvedName = ToSafeInteger(v_sTitle.Trim() & " " & v_sInitials.Trim() & " " & v_sSurname.Trim())
            '    m_oParty.ResolvedName = Trim$(v_sForename) & " " & Trim$(v_sSurname)

            'RJG 26/05/2000 - BEGIN - Add extra parameters
            m_oParty.DateOfBirth = ToSafeInteger(v_dDOB.ToOADate)

            'developer guide no. 24
            m_oParty.UserID = v_sUserID

            'developer guide no. 24
            m_oParty.Password = sEncryptedPassword


            If Not Informations.IsNothing(v_sMothersMaidenName) Then


                'developer guide no. 24
                m_oParty.MothersMaidenName = v_sMothersMaidenName
            End If


            If Not Informations.IsNothing(v_sTPUserCode) Then


                'developer guide no. 24
                m_oParty.TPUserCode = v_sTPUserCode
            End If


            If Not Informations.IsNothing(v_sTPIntroducer) Then

                'developer guide no. 24
                m_oParty.TPIntroducerCode = v_sTPIntroducer

                'developer guide no. 206
                TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "Addquote TPIntroducerCode = " & v_sTPIntroducer, ACApp, ACClass, "RegisterUser")
            Else

                'developer guide no. 206
                TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "Addquote TPIntroducerCode missing", ACApp, ACClass, "RegisterUser")
            End If


            If Not Informations.IsNothing(v_sAQuestion) Then


                'developer guide no. 24
                m_oParty.AQuestion = v_sAQuestion
            End If


            If Not Informations.IsNothing(v_sTheAnswer) Then


                'developer guide no. 24
                m_oParty.TheAnswer = v_sTheAnswer
            End If


            If Not Informations.IsNothing(v_dMemorableDate) Then
                If Informations.IsDate(v_dMemorableDate) Then

                    'developer guide no. 24
                    m_oParty.MemorableDate = Informations.DateSerial(v_dMemorableDate.Year, v_dMemorableDate.Month, v_dMemorableDate.Day)
                End If
            End If


            If Not Informations.IsNothing(v_dCurrInsRenewalDate) Then
                If Informations.IsDate(v_dCurrInsRenewalDate) Then

                    'developer guide no. 24

                    m_oParty.CurrInsRenewalDate = Informations.DateSerial(v_dCurrInsRenewalDate.Year, v_dCurrInsRenewalDate.Month, (v_dCurrInsRenewalDate.Day))
                End If
            End If

            ' RFC050900 - Added Title & MaritalStatusCode optional parameters - START

            If Not Informations.IsNothing(v_sTitle) Then
                m_oParty.PartyTitleCode = ToSafeInteger(v_sTitle.Trim())
            End If


            If Not Informations.IsNothing(v_sMaritalStatusCode) Then
                m_oParty.MaritalStatusCode = ToSafeInteger(v_sMaritalStatusCode.Trim())
            End If

            ' RFC050900 - Added Title & MaritalStatusCode optional parameters - END

            ' RJG 29/09/2000 - Added Gender Code and Initials added as optional parameters

            If Not Informations.IsNothing(v_sGenderCode) Then
                m_oParty.GenderCode = v_sGenderCode
            End If


            If Not Informations.IsNothing(v_sInitials) Then
                m_oParty.Initials = ToSafeInteger(v_sInitials)
            End If


            If Not Informations.IsNothing(v_lAgentCnt) Then
                If v_lAgentCnt <> 0 Then
                    m_oParty.AgentCnt = v_lAgentCnt
                End If
            End If

            ' PWF 10/07/2001 Added v_bIsProspect as optional paramter (default 1) to allow Gemini
            ' to add clients directly without forcing the Propect flag on.

            'developer guide no. 24
            m_oParty.IsProspect = v_bIsProspect

            ' PWF 01/07/2001 - Party source ID (defaulted to SIRIUS_SOURCEID)

            If False Or v_lSourceID.Equals(0) Then

                'developer guide no. 24
                m_oParty.SourceID = SIRIUS_SOURCEID
            Else

                'developer guide no. 24
                m_oParty.SourceID = v_lSourceID
            End If

            '    If Not IsMissing(v_sFileCode$) Then
            '        m_oParty.FileCode = v_sFileCode$
            '    End If
            'RJG 29/09/2000 - Added Gender Code and Initials added as optional parameters - END

            'RJG 26/05/2000 - END

            'RJG 12/06/2000 - Add E-Mail as a contact
            'PWF 18/06/2001 - Check for blank email
            If v_sEMail.Trim() <> "" Then
                ' RAGFIX
                'ReDim vContactArray(3, iContactIndex)
                ReDim vContactArray(4, iContactIndex)


                vContactArray(0, iContactIndex) = "E-MAIL"

                vContactArray(1, iContactIndex) = ""

                vContactArray(2, iContactIndex) = v_sEMail

                vContactArray(3, iContactIndex) = ""
                ' RAGFIX

                vContactArray(4, iContactIndex) = "Email"

                iContactIndex += 1
            End If


            If Not Informations.IsNothing(v_sTelephoneNumber) Then
                If v_sTelephoneNumber.Trim() <> "" Then
                    If iContactIndex Then
                        ' RAGFIX
                        'ReDim Preserve vContactArray(3, iContactIndex)
                        ReDim Preserve vContactArray(4, iContactIndex)
                    Else
                        ' RAGFIX
                        'ReDim vContactArray(3, iContactIndex)
                        ReDim vContactArray(4, iContactIndex)
                    End If


                    vContactArray(0, iContactIndex) = "TELEPHONE"

                    vContactArray(1, iContactIndex) = ""

                    vContactArray(2, iContactIndex) = v_sTelephoneNumber

                    vContactArray(3, iContactIndex) = ""

                    ' RAGFIX

                    vContactArray(4, iContactIndex) = "TelNo"

                    iContactIndex += 1
                End If
            End If



            'developer guide no. 24
            m_oParty.ContactArray = vContactArray


            'developer guide no. 206
            TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "before m_oParty.CreateParty", ACApp, ACClass, "RegisterUser")

            m_lReturn = m_oParty.CreateParty()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                r_lPartyCnt = m_oParty.PartyCnt
                result = gPMConstants.PMEReturnCode.PMTrue
            Else

                'developer guide no. 206
                TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "m_oParty.CreateParty Failed", ACApp, ACClass, "RegisterUser")
            End If

            m_oParty.Dispose()
            m_oParty = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message


            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPartyFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="AddParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function


    Public Function CancelPolicy(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_cCancelPremium As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        ' ***************************************************************** '
        ' Name: CancelPolicy
        ' Date: 13/07/2000
        ' Description: Given v_lInsuranceFileCnt set the InsuranceFileStatus
        '   to Cancelled
        ' ***************************************************************** '

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'm_oQuote = New bSIRInsuranceFile.Services()
            m_oQuote = New bSIRInsuranceFile.Services()

            m_lReturn = m_oQuote.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            'Quit the function if the quote object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TF070102 - Update premium first
            With m_oQuote

                'Retrieve the details and change the InsuranceFileTypeID from Quote to Policy
                .InsuranceFileCnt = v_lInsuranceFileCnt


                If Not Informations.IsNothing(v_cCancelPremium) Then


                    ' developer guide no. 24
                    .ThisPremium = CDec(v_cCancelPremium)
                    m_lReturn = .UpdatePolicy()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        .Dispose()
                        m_oQuote = Nothing
                    End If
                End If

                m_lReturn = .CancelPolicy()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    .Dispose()
                    m_oQuote = Nothing
                End If

                .Dispose()

            End With
            m_oQuote = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message


            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CancelPolicyFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="CancelPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function AddPartyFees(ByRef v_lInsuranceFileCnt As Object, ByRef v_lLegalPartyCnt As Integer, ByRef v_cLegalAmount As Decimal, ByRef v_cLegalCommissionAmount As Decimal, ByRef v_lBreakdownPartyCnt As Integer, ByRef v_cBreakdownAmount As Decimal, ByRef v_cBreakdownCommissionAmount As Decimal, ByRef v_lPromptPartyCnt As Integer, ByRef v_cPromptAmount As Decimal, ByRef v_cPromptCommission As Decimal, ByRef v_lCreditCardChargePartyCnt As Integer, ByRef v_cCreditCardChargeAmount As Decimal, Optional ByRef v_lBrokerFeePartyCnt As Decimal = 0, Optional ByRef v_cBrokerFeeAmount As Decimal = 0) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        ' ***************************************************************** '
        ' Name: AddPartyFees
        ' Date: 12/10/2000
        ' Description: Creates Party Fees for Add On's for a policy
        ' ***************************************************************** '

        Dim vPartyFeesArray As Object
        Dim iFeesCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oInsuranceFileBusiness = New bSIRInsuranceFile.Business()

            m_lReturn = m_oInsuranceFileBusiness.Initialise(sUsername:=SIRIUS_USERNAME, sPassword:=SIRIUS_PASSWORD, iUserID:=SIRIUS_USERID, iSourceID:=SIRIUS_SOURCEID, iLanguageID:=SIRIUS_LANGUAGEID, iCurrencyID:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            'RJG 12/10/2000 - Quit the function if the quote object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            iFeesCount = 0

            'RJG 12/10/2000 - The structure of the Party Fees array is as follows
            '0  -   Not used
            '1  -   Not used
            '2  -   Fee Percentage
            '3  -   Fee Amount
            '4  -   Party Cnt
            '5  -   Commission Percentage
            '6  -   Commission Amount
            '7  -   Is IPTable

            ReDim vPartyFeesArray(7, iFeesCount)

            If v_cLegalAmount <> 0 Or v_cLegalCommissionAmount <> 0 Then

                vPartyFeesArray(2, iFeesCount) = 0

                vPartyFeesArray(3, iFeesCount) = v_cLegalAmount

                vPartyFeesArray(4, iFeesCount) = v_lLegalPartyCnt

                vPartyFeesArray(5, iFeesCount) = 0

                vPartyFeesArray(6, iFeesCount) = v_cLegalCommissionAmount

                vPartyFeesArray(7, iFeesCount) = 1
                iFeesCount += 1
            End If

            If v_cBreakdownAmount <> 0 Or v_cBreakdownCommissionAmount <> 0 Then
                ReDim Preserve vPartyFeesArray(7, iFeesCount)

                vPartyFeesArray(2, iFeesCount) = 0

                vPartyFeesArray(3, iFeesCount) = v_cBreakdownAmount

                vPartyFeesArray(4, iFeesCount) = v_lBreakdownPartyCnt

                vPartyFeesArray(5, iFeesCount) = 0

                vPartyFeesArray(6, iFeesCount) = v_cBreakdownCommissionAmount

                vPartyFeesArray(7, iFeesCount) = 1
                iFeesCount += 1
            End If

            If v_cPromptAmount <> 0 Or v_cPromptCommission <> 0 Then
                ReDim Preserve vPartyFeesArray(7, iFeesCount)

                vPartyFeesArray(2, iFeesCount) = 0

                vPartyFeesArray(3, iFeesCount) = v_cPromptAmount

                vPartyFeesArray(4, iFeesCount) = v_lPromptPartyCnt

                vPartyFeesArray(5, iFeesCount) = 0

                vPartyFeesArray(6, iFeesCount) = v_cPromptCommission

                vPartyFeesArray(7, iFeesCount) = 0
                iFeesCount += 1
            End If

            If v_cCreditCardChargeAmount <> 0 Then
                ReDim Preserve vPartyFeesArray(7, iFeesCount)

                vPartyFeesArray(2, iFeesCount) = 0

                vPartyFeesArray(3, iFeesCount) = v_cCreditCardChargeAmount

                vPartyFeesArray(4, iFeesCount) = v_lCreditCardChargePartyCnt

                vPartyFeesArray(5, iFeesCount) = 0

                vPartyFeesArray(6, iFeesCount) = 0

                vPartyFeesArray(7, iFeesCount) = 0
                iFeesCount += 1
            End If


            If Not Informations.IsNothing(v_cBrokerFeeAmount) Then
                If v_cBrokerFeeAmount <> 0 Then
                    ReDim Preserve vPartyFeesArray(7, iFeesCount)

                    vPartyFeesArray(2, iFeesCount) = 0

                    vPartyFeesArray(3, iFeesCount) = v_cBrokerFeeAmount

                    vPartyFeesArray(4, iFeesCount) = v_lBrokerFeePartyCnt

                    vPartyFeesArray(5, iFeesCount) = 0

                    vPartyFeesArray(6, iFeesCount) = 0

                    vPartyFeesArray(7, iFeesCount) = 0
                    iFeesCount += 1
                End If
            End If

            If iFeesCount > 0 Then

                m_lReturn = m_oInsuranceFileBusiness.AddFees(vInsuranceFileCnt:=v_lInsuranceFileCnt, vFees:=vPartyFeesArray)
                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPartyFeesFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="AddPartyFees", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function FindQuote(ByRef r_vResultArray As Object, Optional ByVal v_sQuoteRef As String = "", Optional ByVal v_dCoverStartDate As Object = Nothing, Optional ByVal v_sDescription As String = "", Optional ByVal v_lLeadAgentCnt As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        ' ***************************************************************** '
        ' Name: FindQuote
        ' Date: 04/12/2000
        ' Description:  Finds Quotes given any number of the optional parameters above
        ' NOTE: at least one of these optional params must be provided..
        ' ***************************************************************** '

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oFindInsurance = New bSIRFindInsurance.Form()

            'RJG 04/12/2000 - Check that some search Criteria has been passed through

            If (v_sQuoteRef & "").Trim() = "" And Object.Equals(v_dCoverStartDate, Nothing) And (v_sDescription & "").Trim() = "" Then

                'developer guide no. 206
                TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "No Parameters Passed.", ACApp, ACClass, "FindQuote")
                Return result
            End If


            m_lReturn = m_oFindInsurance.Initialise(sUsername:=SIRIUS_USERNAME, sPassword:=SIRIUS_PASSWORD, iUserID:=SIRIUS_USERID, iSourceId:=SIRIUS_SOURCEID, iLanguageId:=SIRIUS_LANGUAGEID, iCurrencyId:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            'RJG 04/12/2000 - Quit the function if the FindInsurance object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then

                'developer guide no. 206
                TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "Failed to Initialise FindInsurance object.", ACApp, ACClass, "FindQuote")
                Return result
            End If


            'RJG 04/12/2000 - Quit the function if the FindInsurance object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then

                'developer guide no. 206
                TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "Failed to Initialise FindInsurance object.", ACApp, ACClass, "FindQuote")
                Return result
            End If


            m_lReturn = m_oFindInsurance.FindQuote(r_vResultArray:=r_vResultArray, v_sQuoteRef:=ToSafeString(v_sQuoteRef), v_dtCoverStartDate:=CType(v_dCoverStartDate, Object), v_sInsuranceFolderDescription:=ToSafeString(v_sDescription), v_lLeadAgentCnt:=CStr(v_lLeadAgentCnt))

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then

                'developer guide no. 206
                TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "m_oFindInsurance.FindQuote Failed.", ACApp, ACClass, "FindQuote")
                m_oFindInsurance.Dispose()
                m_oFindInsurance = Nothing
                Return result
            End If

            m_oFindInsurance.Dispose()
            m_oFindInsurance = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindQuoteFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="FindQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GenerateShortName
    '
    ' Description: Generates a Shortname for the New Party
    '
    ' ***************************************************************** '
    Private Function GenerateShortName(ByRef r_sUserID As String) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Dim oAutoNum As bPMAutoNumber.Business = Nothing
        Dim lReturn, lUserID As Integer
        Dim sPrefix As String = String.Empty
        Dim sSuffix As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Component Services

            ' Create the AutoNumber Business Object


            oAutoNum = New bPMAutoNumber.Business
            lReturn = oAutoNum.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oAutoNum = Nothing
                Return lReturn
            End If

            ' Create a Unique Number for this Party

            lReturn = oAutoNum.GenerateNewNumber(v_sPMProductCode:=gPMConstants.PMProductCode(gPMConstants.PMEProductFamily.pmePFSiriusSolutions), v_sGroupCode:="XELUSERID", v_sRangeCode:="XELUSERID", v_iUserId:=1, r_lNumber:=lUserID, r_sPrefix:=sPrefix, r_sSuffix:=sSuffix)
            '    If (lReturn <> PMTrue) Then
            If lUserID < 1 Then

                'developer guide no. 206
                TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogError, "Failed to generate New Number for PMNumber_Group : XELUSERID and PMNumber_Range : XELUSERID", ACApp, ACClass, "GenerateShortname")
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

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateShortNameFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateShortName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
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
        Dim ACApp As String = "bSiriusLink"

        Dim iNumber, iUpper, lLenID As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sClientID, sWorkingID As String
        Dim vNamesArray(,) As Object = Nothing



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

        lReturn = CType(FindParty(vNamesArray, v_sShortname:=sClientID & "%"), gPMConstants.PMEReturnCode)

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
                        If iNumber <= ToSafeInteger(sWorkingID) Then
                            iNumber = ToSafeInteger(sWorkingID) + 1
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



    Public Function GetProductByAgent(ByVal v_lAgentPartyCnt As Integer, ByRef r_vResultsArray As Object) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        ' ***************************************************************** '
        ' Name: GetProductByAgent
        ' Date: 09/01/2001
        ' Description:  Returns all of the available products given an Agents
        '       Party_Cnt
        ' ***************************************************************** '
        'todo list (Iteration 3)
        'Dim oFindProductType As bSIRFindProductType.Business
        Dim oFindProductType As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'todo list (Iteration 3)
            'oFindProductType = New bSIRFindProductType.Business()
            oFindProductType = New Object()


            m_lReturn = oFindProductType.Initialise(sUsername:=ToSafeString(SIRIUS_USERNAME), sPassword:=ToSafeString(SIRIUS_PASSWORD), iUserID:=ToSafeInteger(SIRIUS_USERID), iSourceID:=ToSafeInteger(SIRIUS_SOURCEID), iLanguageID:=ToSafeInteger(SIRIUS_LANGUAGEID), iCurrencyID:=ToSafeInteger(SIRIUS_CURRENCYID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))


            'RJG 09/01/2001 - Quit the function if the FindProductType object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then

                'developer guide no. 206
                TempFunc.LogMessage(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "Failed to Initialise FindProductType object.", ACApp, ACClass, "GetProductByAgent")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oFindProductType.GetProductByAgent(ToSafeInteger(v_lAgentPartyCnt), r_vResultsArray)

            'RJG 09/01/2001 - Quit the function if the FindProductType object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then

                'developer guide no. 206
                TempFunc.LogMessage(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "oFindProductType.GetProductByAgent Failed.", ACApp, ACClass, "GetProductByAgent")
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindProductType = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductByAgent Failed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductByAgent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As dPMDAO.Database = Nothing) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUserName = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Initialisation Code.

            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            '    ' Check the Supplied Database.
            '    m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, _
            ''        v_lPMProductFamily:=PMProductFamily, _
            ''        r_bNewInstanceCreated:=m_bCloseDatabase, _
            ''        r_oCheckedDatabase:=m_oDatabase, _
            ''        v_vDatabase:=vDatabase)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        Initialise = PMFalse
            '        Exit Function
            '    End If


            If Not Informations.IsNothing(vDatabase) Then
                m_oDatabase = vDatabase
                m_bCloseDatabase = False
            End If


            ' Store the database
            'If Not IsMissing(vDatabase) Then
            '    Set m_oDatabase = vDatabase
            'End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
    Public Function LapseQuotes(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_bOnlyLast7Days As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        ' ***************************************************************** '
        ' Name: LapseQuotes
        ' Date: 14/07/2000
        ' Description:  Lapses Quotes given an InsuranceFileCnt.
        ' If OnlyLast7Days is True works the date out for that and passes that too!
        '
        ' ***************************************************************** '

        Try

            Dim dtLapsedDate As Date

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oQuote = New bSIRInsuranceFile.Services()

            m_lReturn = m_oQuote.Initialise(sUsername:=SIRIUS_USERNAME, sPassword:=SIRIUS_PASSWORD, iUserID:=SIRIUS_USERID, iSourceID:=SIRIUS_SOURCEID, iLanguageID:=SIRIUS_LANGUAGEID, iCurrencyID:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            'RJG 14/07/2000 - Quit the function if the quote object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oQuote.InsuranceFileCnt = v_lInsuranceFileCnt

            If v_bOnlyLast7Days Then
                dtLapsedDate = DateTime.Today.AddDays(-7)
                m_lReturn = m_oQuote.UpdateLapsed(ToSafeDate(dtLapsedDate))
            Else
                m_lReturn = m_oQuote.UpdateLapsed()
            End If

            m_oQuote.Dispose()
            m_oQuote = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LapseQuotesFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="LapseQuotes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function SuspendAllMTAQuotes(ByVal v_lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        ' ***************************************************************** '
        ' Name: SuspendAllMTAQuotes
        ' Date: 13/07/2000
        ' Description: Given the PartyCnt gets the Permanent & Temporary MTA quotes
        ' and updates their Insurance_File_Type to MTASUSPEND
        '
        ' ***************************************************************** '

        Try

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SuspendAllMTAQuotesFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="SuspendAllMTAQuotes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

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
            Me.disposedValue = True
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
                If m_oParty IsNot Nothing Then
                    m_oParty.Dispose()
                    m_oParty = Nothing
                End If
                If m_oQuote IsNot Nothing Then
                    m_oQuote.Dispose()
                    m_oQuote = Nothing
                End If
                If m_oFindInsurance IsNot Nothing Then
                    m_oFindInsurance.Dispose()
                    m_oFindInsurance = Nothing
                End If
                If m_oInsuranceFileBusiness IsNot Nothing Then
                    m_oInsuranceFileBusiness.Dispose()
                    m_oInsuranceFileBusiness = Nothing
                End If

            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Function AddQuote(ByVal v_lPartyCnt As Integer, ByVal v_dtStartDate As Date, ByVal v_dtEndDate As Date, ByVal v_sInsuranceFolderCode As String, ByVal v_sVehicleMakeModel As String, ByVal v_sInsuranceRef As String, ByRef r_lInsuranceFileCnt As Integer, ByVal v_cAnnualPremium As Decimal, Optional ByVal v_sRiskCode As String = "", Optional ByVal v_lLeadInsurerABICode As String = "", Optional ByVal v_sInsuranceFolderDescription As String = "", Optional ByVal v_lLeadAgentCnt As Integer = 0, Optional ByVal v_lPolicySourceID As Integer = 1, Optional ByVal v_sInsuredName As String = "", Optional ByRef r_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_sInsuranceFileStructure As String = "QEM", Optional ByVal v_cNetPremium As Decimal = 0, Optional ByVal v_sRenewalFrequency As String = "", Optional ByVal v_sBusinessType As String = "", Optional ByVal v_sPaymentMethod As String = "", Optional ByVal v_lProductID As Integer = 0, Optional ByVal v_lCurrencyID As Integer = 1, Optional ByVal v_lAnalysisCodeId As Integer = 0, Optional ByVal v_sPolicyStatusCode As String = "", Optional ByVal v_lPolicyVersion As Integer = 0, Optional ByVal v_blConsLeadAgntComm As Boolean = False, Optional ByVal v_blConsSubAgntComm As Boolean = False, Optional ByVal v_lLapsedReasonId As Integer = 0, Optional ByVal v_dtLapsedDate As Date = GISSharedConstants.GISLowDate, Optional ByVal v_sLapsedReasonDescription As String = "", Optional ByVal v_dtInceptionDate As Date = GISSharedConstants.GISLowDate, Optional ByVal v_dtInceptionDateTPI As Date = GISSharedConstants.GISLowDate, Optional ByVal v_dtRenewalDate As Date = GISSharedConstants.GISLowDate, Optional ByVal v_sAlternateReference As String = "", Optional ByVal v_sOldPolicyNumber As String = "", Optional ByVal v_sAccountExecutiveShortname As String = "", Optional ByVal v_sAccountHandlerShortname As String = "", Optional ByVal v_sInsuranceFileTypeCode As String = "", Optional ByVal v_bCCTermsAgreed As Boolean = False, Optional ByVal v_lTypeOfSaleId As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        ' ***************************************************************** '
        ' Name: AddQuote
        ' Date: 18/05/2000
        ' Description:  Adds a new quote record
        '
        ' RJG 06/12/2000 - Added v_sInsuranceFolderDescription and v_lLeadAgentCnt
        ' PW090206 - Changed v_sRiskCode to variant so ismissing check works correctly
        '            and risk code id is not set regardless. Breaks compatibility.
        ' ***************************************************************** '
        Const kMethodName As String = "AddQuote"

        Dim sPolicyTypeCode As String = String.Empty
        Dim sProductCode As String = String.Empty
        Dim iBaseCurrencyId As Integer
        Dim lPartyCnt, lGracePeriod As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oQuote = New bSIRInsuranceFile.Services()

            m_lReturn = m_oQuote.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            'RJG 18/05/2000 - Quit the function if the quote object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If


            m_oQuote.InsuranceFileType = CStr(IFTCodes(0))

            'developer guide no. 24
            m_oQuote.InsuranceFileTypeID = InsuranceFileType.IFTQuote

            ' RDT 19112002 - Changed to allow other structures to be specified, but still defaults to "GEM"
            m_oQuote.InsuranceFileStructure = v_sInsuranceFileStructure

            'RDT PN17825 - Set the policy dates correctly

            'developer guide no. 24
            m_oQuote.CoverStartDate = v_dtStartDate

            'developer guide no. 24
            m_oQuote.ProposalDate = DateTime.Now

            'Start - Sankar - PN 55108
            m_lReturn = GetGracePeriod(v_lProductID, lGracePeriod)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetGracePeriod Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            'developer guide no. 24
            m_oQuote.QuoteExpiryDate = DateTime.Now.AddDays(lGracePeriod) ' Changed 30 to lGracePeriod
            'End - Sankar - PN 55108


            'developer guide no. 24
            m_oQuote.AnniversaryDate = v_dtStartDate.AddYears(1)

            'developer guide no. 24
            m_oQuote.CurrencyID = v_lCurrencyID


            'developer guide no. 24
            m_oQuote.AnalysisCodeId = If(v_lAnalysisCodeId = 0, Nothing, v_lAnalysisCodeId)


            'refer developer guide no. 24
            m_oQuote.InsuranceHolderCnt = v_lPartyCnt
            m_oQuote.InsuranceFolderCode = v_sInsuranceFolderCode

            ' Get the base currency for the branch. PW090206.
            m_lReturn = bPMFunc.GetBranchBaseCurrency(v_lSourceID:=v_lPolicySourceID, v_oDatabase:=m_oDatabase, r_iCurrencyID:=iBaseCurrencyId)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'developer guide no. 24
                m_oQuote.BaseCurrencyID = iBaseCurrencyId
            Else

                'developer guide no. 24
                m_oQuote.BaseCurrencyID = SIRIUS_CURRENCYID
            End If

            ' ***************************************************************** '
            ' BEGIN: Get PolicyTypeCode and ProductCode from registry
            '
            ' If we have data model and business type codes get settings from
            ' registry, if we don't use default values:
            '   PolicyTypeCode = POLICY_TYPE_GNETMOTOR_CODE
            '   ProductCode = <Not Set>
            '
            ' PWF261001 - Created
            ' ***************************************************************** '
            If m_sGisDataModelCode.Length > 0 And m_sGisBusinessTypeCode.Length > 0 Then
                If m_sGisDataModelCode = AGENTS_ONLINE_DATAMODEL_CODE Then
                    m_oQuote.PolicyType = POLICY_TYPE_UNDERWRITING_CODE
                Else
                    ' Get the policy type code
                    m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=m_sGisDataModelCode, v_sBusinessTypeCode:=m_sGisBusinessTypeCode, v_sSettingName:=GISSharedConstants.GISRegPolicyTypeCode, r_sSettingValue:=sPolicyTypeCode)
                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMFalse) And sPolicyTypeCode.Length > 0 Then
                        m_oQuote.PolicyType = sPolicyTypeCode
                    Else
                        m_oQuote.PolicyType = POLICY_TYPE_GNETMOTOR_CODE
                    End If

                End If

                ' Get the product code
                m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=m_sGisDataModelCode, v_sBusinessTypeCode:=m_sGisBusinessTypeCode, v_sSettingName:=GISSharedConstants.GISRegProductCode, r_sSettingValue:=sProductCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMFalse) And sProductCode.Length > 0 Then
                    m_oQuote.Product = sProductCode
                End If

            Else
                ' We don't so use the default method
                m_oQuote.PolicyType = POLICY_TYPE_GNETMOTOR_CODE
            End If

            ' Log information for debuging purposes

            TempFunc.LogMessageFile(gPMConstants.PMELogLevel.PMLogDebug1, "Retrieved PolicyTypeCode and ProductCode. Values:" & Strings.ChrW(13) & Strings.ChrW(10) &
                                    "GISDataModelCode:    " & m_sGisDataModelCode & Strings.ChrW(13) & Strings.ChrW(10) &
                                    "GISBusinessTypeCode: " & m_sGisBusinessTypeCode & Strings.ChrW(13) & Strings.ChrW(10) &
                                    "PolicyType:          " & sPolicyTypeCode & Strings.ChrW(13) & Strings.ChrW(10) &
                                    "ProductCode:         " & sProductCode, ACApp, ACClass, "AddQuote")
            ' ***************************************************************** '
            ' END: Get PolicyTypeCode and ProductCode from registry
            ' ***************************************************************** '


            If String.IsNullOrEmpty(v_sInsuranceFolderDescription) Then
                m_oQuote.InsuranceFolderDescription = "NEW QUOTE"
            Else
                m_oQuote.InsuranceFolderDescription = v_sInsuranceFolderDescription
            End If

            'developer guide no. 24
            m_oQuote.LastTransDescription = v_sVehicleMakeModel

            ' You can't check IsMissing on a non-variant parameter


            If Not Informations.IsNothing(v_sRiskCode) Then
                If v_sRiskCode.ToUpper().Trim() = "TPFT" Then
                    m_oQuote.RiskCode = CStr(292)
                ElseIf v_sRiskCode.ToUpper().Trim() = "TPO" Then
                    m_oQuote.RiskCode = CStr(291)
                ElseIf v_sRiskCode.ToUpper().Trim() = "COMP" Then
                    m_oQuote.RiskCode = CStr(293) 'COMP
                Else
                    m_oQuote.RiskCode = Nothing
                End If
            End If


            If Not Informations.IsNothing(v_lLeadInsurerABICode) Then
                m_oQuote.LeadInsurerABICode = v_lLeadInsurerABICode
            End If

            m_oQuote.InsuranceRef = v_sInsuranceRef

            'developer guide no. 24
            m_oQuote.AnnualPremium = v_cAnnualPremium

            If Not False And v_lLeadAgentCnt <> 0 Then

                'developer guide no. 24
                m_oQuote.LeadAgentCnt = v_lLeadAgentCnt
            End If


            If False Or v_lPolicySourceID.Equals(0) Then

                'developer guide no. 24
                m_oQuote.SourceID = SIRIUS_SOURCEID
            Else

                'developer guide no. 24
                m_oQuote.SourceID = v_lPolicySourceID
            End If

            If v_sInsuredName.Length > 0 Then

                'developer guide no. 24
                m_oQuote.InsuredName = v_sInsuredName
            End If

            ' RDT - 19112002 - Added additional Optional parameters

            If Not False Then

                'developer guide no. 24
                m_oQuote.NetPremium = v_cNetPremium
            End If

            If v_sRenewalFrequency IsNot Nothing Then
                m_oQuote.RenewalFrequency = v_sRenewalFrequency
            End If

            If v_sBusinessType IsNot Nothing Then
                m_oQuote.BusinessType = v_sBusinessType
            End If

            If v_sPaymentMethod IsNot Nothing Then

                'developer guide no. 24
                m_oQuote.PaymentMethod = v_sPaymentMethod
            End If

            If v_lProductID <> 0 Then

                'developer guide no. 24
                m_oQuote.ProductID = v_lProductID
            End If

            If v_lPolicyVersion <> 0 Then

                'developer guide no. 24
                m_oQuote.PolicyVersion = v_lPolicyVersion
            End If

            If r_lInsuranceFolderCnt <> 0 Then

                'developer guide no. 24
                m_oQuote.InsuranceFolderCnt = r_lInsuranceFolderCnt
            End If

            If v_sPolicyStatusCode IsNot Nothing Then
                Select Case v_sPolicyStatusCode.TrimEnd()
                    Case "CUR"

                        m_oQuote.InsuranceFileStatus = Nothing
                        m_oQuote.PolicyStatus = v_sPolicyStatusCode
                    Case "CAN", "LAP", "REN", "REP"
                        m_oQuote.InsuranceFileStatus = v_sPolicyStatusCode
                        m_oQuote.PolicyStatus = v_sPolicyStatusCode
                    Case Else

                        m_oQuote.InsuranceFileStatus = Nothing

                        m_oQuote.PolicyStatus = Nothing
                End Select
            End If

            m_oQuote.LeadAgentAllowCommission = If(v_blConsLeadAgntComm, 1, 0)
            m_oQuote.SubAgentAllowCommission = If(v_blConsSubAgntComm, 1, 0)

            If v_lLapsedReasonId <> 0 Then

                'developer guide no. 24
                m_oQuote.LapsedReasonID = v_lLapsedReasonId
            End If

            If v_dtLapsedDate <> GISSharedConstants.GISLowDate Then

                'developer guide no. 24
                m_oQuote.LapsedDate = v_dtLapsedDate
            End If

            If v_sLapsedReasonDescription <> "" Then

                'developer guide no. 24
                m_oQuote.LapsedDescription = v_sLapsedReasonDescription
            End If

            If v_dtInceptionDate <> GISSharedConstants.GISLowDate Then

                'developer guide no. 24
                m_oQuote.InceptionDate = v_dtInceptionDate

                'developer guide no. 24
                m_oQuote.CCInceptionDate = v_dtInceptionDate
            Else

                'developer guide no. 24
                m_oQuote.CCInceptionDate = v_dtStartDate
            End If

            If v_dtInceptionDateTPI <> GISSharedConstants.GISLowDate Then

                'developer guide no. 24
                m_oQuote.InceptionTPI = v_dtInceptionDateTPI
            Else

                'developer guide no. 24
                m_oQuote.InceptionTPI = v_dtStartDate
            End If

            If v_dtRenewalDate <> GISSharedConstants.GISLowDate Then

                'developer guide no. 24
                m_oQuote.RenewalDate = v_dtRenewalDate
            Else

                'developer guide no. 24
                m_oQuote.RenewalDate = v_dtEndDate
            End If

            If v_dtEndDate <> GISSharedConstants.GISLowDate Then

                'developer guide no. 24
                m_oQuote.ExpiryDate = v_dtEndDate
            ElseIf v_dtRenewalDate <> GISSharedConstants.GISLowDate Then

                'developer guide no. 24
                m_oQuote.ExpiryDate = v_dtRenewalDate.AddDays(-1)
            End If

            If v_sAlternateReference <> "" Then

                'developer guide no. 24
                m_oQuote.AlternateReference = v_sAlternateReference
            End If

            If v_sOldPolicyNumber <> "" Then

                'developer guide no. 24
                m_oQuote.OldPolicyNumber = v_sOldPolicyNumber
            End If

            If v_sAccountExecutiveShortname <> "" Then
                m_lReturn = GetPartyCntFromShortname(v_sAccountExecutiveShortname, lPartyCnt)

                'developer guide no. 24
                m_oQuote.AccountExecutiveCnt = lPartyCnt
                m_oQuote.AccountExecutive = v_sAccountExecutiveShortname
            End If

            If v_sAccountHandlerShortname <> "" Then
                m_lReturn = GetPartyCntFromShortname(v_sAccountHandlerShortname, lPartyCnt)

                'developer guide no. 24
                m_oQuote.AccountHandlerCnt = lPartyCnt
                m_oQuote.AccountHandler = v_sAccountHandlerShortname
            End If

            If v_sInsuranceFileTypeCode <> "" Then
                m_oQuote.InsuranceFileType = v_sInsuranceFileTypeCode
            End If
            If Not False Then

                'developer guide no. 24
                m_oQuote.CCTermsAgreed = v_bCCTermsAgreed
            End If

            If Not False Then

                'developer guide no. 24
                m_oQuote.FSATypeOfSaleID = v_lTypeOfSaleId
            End If

            m_lReturn = m_oQuote.CreatePolicy()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                r_lInsuranceFileCnt = m_oQuote.InsuranceFileCnt

                r_lInsuranceFolderCnt = m_oQuote.InsuranceFolderCnt
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            m_oQuote.Dispose()
            m_oQuote = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddQuoteFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="AddQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    'Start - Sankar - PN 55108
    Public Function GetGracePeriod(ByVal lProductId As Integer, ByRef r_lGracePeriod As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetGracePeriod"

        Dim vResults(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="column_name", vValue:="grace_period", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            ' Execute selection Query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetGracePeriodSQL, sSQLName:=ACGetGracePeriodName, bStoredProcedure:=ACGetGracePeriodStored, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetGracePeriodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            If Informations.IsArray(vResults) Then
                r_lGracePeriod = gPMFunctions.ToSafeLong(vResults(0, 0), 0)
            Else
                gPMFunctions.RaiseError(kMethodName, ACGetGracePeriodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result

        Catch ex As Exception



            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


            Return result
        End Try
    End Function
    'End - Sankar - PN 55108

    ' ***************************************************************** '
    ' Name: GetPartyCntFromShortname
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : RDT : 12-07-2007
    ' ***************************************************************** '
    Public Function GetPartyCntFromShortname(ByVal v_sPartyShortname As String, ByRef r_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyCntFromShortname"


        Dim vResults(,) As Object = Nothing
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ShortName", vValue:=v_sPartyShortname, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=ACGetPartyByShortNameSQL, sSQLName:=ACGetPartyByShortNameName, bStoredProcedure:=True, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, ACGetPartyByShortNameName & " Failed for return Party_cnt for Party - " & v_sPartyShortname, gPMConstants.PMELogLevel.PMLogError)

            End If

            If Not Informations.IsArray(vResults) Then
                gPMFunctions.RaiseError(kMethodName, ACGetPartyByShortNameName & " Failed for return Party_cnt for Party - " & v_sPartyShortname, gPMConstants.PMELogLevel.PMLogError)
            Else

                r_lPartyCnt = ToSafeInteger(vResults(0, 0))
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    Public Function FindInsurance(ByVal v_lPartyCnt As Integer, ByRef r_vResults As Object) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        ' ***************************************************************** '
        ' Name: FindInsurance
        ' Date: 17/05/2000
        ' Description:  Returns the details of Insurance files given v_lPartyCnt
        '   unless PartyCnt = 0
        '
        ' ***************************************************************** '

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oFindInsurance = New bSIRFindInsurance.Form()

            m_lReturn = m_oFindInsurance.Initialise(sUsername:=SIRIUS_USERNAME, sPassword:=SIRIUS_PASSWORD, iUserID:=SIRIUS_USERID, iSourceId:=SIRIUS_SOURCEID, iLanguageId:=SIRIUS_LANGUAGEID, iCurrencyId:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            'RJG 23/05/2000 - Quit the function if the FindInsurance object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If

            m_lReturn = m_oFindInsurance.SearchAll(r_vResults, , , , ToSafeInteger(v_lPartyCnt))

            result = m_lReturn

            m_oFindInsurance.Dispose()
            m_oFindInsurance = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindInsuranceFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="FindInsurance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function



    Public Function FindParty(ByRef r_vResultsArray(,) As Object, Optional ByVal v_sPartyType As String = "", Optional ByVal v_sShortname As String = "", Optional ByVal v_sResolvedName As String = "", Optional ByVal v_sTelephoneNumber As String = "", Optional ByVal v_sPostCode As String = "", Optional ByVal v_lLeadAgentCnt As Integer = 0, Optional ByVal v_sAddress1 As String = "", Optional ByVal v_sPolicyNo As String = "", Optional ByRef v_vAdditionalDataArray(,) As Object = Nothing, Optional ByVal v_sFileCode As String = "") As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        ' ***************************************************************** '
        ' Name: FindParty
        ' Date: 15/11/2000
        ' Description:  Finds parties given any number of the optional parameters above
        ' NOTE: at least one of these optional params must be provided..
        ' ***************************************************************** '
        'AAB - 27-Aug-2002 11:57 - Added the optioal paramater v_sAddress1 to support Agents On Line.
        'CTAF 20030304 - Added v_sPolicyNo parameter

        Try

            Dim vResultArray As Object(,) = Nothing
            Dim sStructure As String = ""

            result = gPMConstants.PMEReturnCode.PMFalse

            'RJG 15/11/2000 - First check that at least one optional parameter has been passed.
            'CLG 20040217 - Added check for v_vAdditionalDataArray as any parameter can now be passed this way
            If v_sPartyType = "" And v_sShortname = "" And v_sResolvedName = "" And v_sTelephoneNumber = "" And v_sPostCode = "" And v_lLeadAgentCnt = 0 And Not Informations.IsArray(v_vAdditionalDataArray) Then

                'developer guide no. 206
                TempFunc.LogMessage(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "No Parameters Passed.", ACApp, ACClass, "FindParty")
                Return result
            End If

            m_oFindParty = New bSIRFindParty.Business()


            m_lReturn = m_oFindParty.Initialise(sUsername:=ToSafeString(SIRIUS_USERNAME), sPassword:=ToSafeString(SIRIUS_PASSWORD), iUserID:=ToSafeInteger(SIRIUS_USERID), iSourceID:=ToSafeInteger(SIRIUS_SOURCEID), iLanguageID:=ToSafeInteger(SIRIUS_LANGUAGEID), iCurrencyID:=ToSafeInteger(SIRIUS_CURRENCYID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)

            'RJG 15/11/2000 - Quit the function if the FindParty object failed to initialise
            'AAB - 16-Oct-2002 10:11 - Changed it from = PMFalse to <> PMTrue
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message

                TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRFindParty.Business Object Failed to Initialize", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="FindParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return m_lReturn
            End If

            '**** START CHANGES - Changed By: AAB  - Changed On: 27-Aug-2002 11:57   ****
            '**** Added this mehtod to get the structure, so AGENTS can be excludeded.
            m_lReturn = m_oFindParty.GetStructure(sStructure)
            'AAB - 16-Oct-2002 10:11 - Changed it from = PMFalse to <> PMTrue
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message

                TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRFindParty.Business.GetStructure Method Failed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="FindParty", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return m_lReturn
            End If
            '****   END CHANGES - Changed By: AAB  - Changed On: 27-Aug-2002 11:57   ****

            ' CTAF 20030304 - Added PolicyNo


            m_lReturn = m_oFindParty.SearchByQuery(r_vResultArray:=vResultArray, r_lNumberOfRecords:=100, v_vShortName:=v_sShortname, v_vName:=v_sResolvedName, v_vClientType:=v_sPartyType, v_vNumber:=v_sTelephoneNumber, v_vAgentCnt:=v_lLeadAgentCnt, v_vPostalCode:=v_sPostCode, v_vAddress1:=v_sAddress1, v_vInsuranceRef:=v_sPolicyNo, v_vAdditionalDataArray:=v_vAdditionalDataArray, v_vFileCode:=v_sFileCode)

            'AAB - 16-Oct-2002 10:10 - Changed it from = PMFalse to <> PMTrue
            'Also assigned the Function value to the proper return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' CTAF 20040312 - Only log errors if its other than PMNotFound
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

                    'developer guide no. 206
                    TempFunc.LogMessage(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "m_oFindParty.SearchByQuery Failed.", ACApp, ACClass, "FindParty")
                End If
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
            m_oFindParty.Dispose()
            m_oFindParty = Nothing
            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindPartyFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="FindParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetParty
    ' Date: 15/11/2000
    ' Description:  Gets a parties details given the Party Cnt
    ' ***************************************************************** '
    Public Function GetParty(ByVal v_lPartyCnt As Integer, ByRef r_sSurname As String, ByRef r_sForename As String, ByRef r_sPartyType As String, ByRef r_sAddress1 As String, ByRef r_sAddress2 As String, ByRef r_sAddress3 As String, ByRef r_sAddress4 As String, ByRef r_sPostCode As String, ByRef r_dDOB As String, ByRef r_sEMail As String, ByRef r_sUserID As String, ByRef r_sPassword As String, ByRef r_sShortName As String, ByRef r_sResolvedName As String, Optional ByRef r_sMothersMaidenName As Object = Nothing, Optional ByRef r_sTPUserCode As Object = Nothing, Optional ByRef r_sTPIntroducer As Object = Nothing, Optional ByRef r_sAQuestion As Object = Nothing, Optional ByRef r_sTheAnswer As Object = Nothing, Optional ByRef r_dMemorableDate As Object = Nothing, Optional ByRef r_dCurrInsRenewalDate As Object = Nothing, Optional ByRef r_sTitle As Integer = 0, Optional ByRef r_sMaritalStatusCode As Integer = 0, Optional ByRef r_sGenderCode As Integer = 0, Optional ByRef r_sInitials As Integer = 0, Optional ByRef r_sTelephoneNumber As Object = Nothing, Optional ByRef r_sOccupationCode As String = "", Optional ByRef r_vAllContactsArray(,) As Object = Nothing, Optional ByRef r_sCountryCode As String = "", Optional ByRef r_lSourceId As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Dim sPassword As String = ""
        Dim vContactArray(,) As Object = Nothing
        Dim oPMLookup As bPMLookup.Business

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'm_oParty = New bSIRParty.Services()
            m_oParty = New bSIRParty.Services()


            m_lReturn = m_oParty.Initialise(sUsername:=SIRIUS_USERNAME, sPassword:=SIRIUS_PASSWORD, iUserID:=SIRIUS_USERID, iSourceID:=SIRIUS_SOURCEID, iLanguageID:=SIRIUS_LANGUAGEID, iCurrencyID:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            'RJG 15/11/2000 - Quit the function if the Party object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If

            m_oParty.PartyCnt = v_lPartyCnt

            m_lReturn = m_oParty.GetDetails()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'RJG 15/11/2000 - GetDetails was successful so stuff the properties we are
                'interested in into the parameters passed

                r_sSurname = CStr(m_oParty.Name).Trim()
                r_sForename = CStr(m_oParty.Forename).Trim()
                '**** START CHANGES - Changed By: AAB  - Changed On: 28-Aug-2002 10:31   ****
                '**** I changed the return property to SolutionPartyType
                'r_sPartyType = Trim(m_oParty.PartyType)
                r_sPartyType = m_oParty.SolutionPartyType.Trim()
                '****   END CHANGES - Changed By: AAB  - Changed On: 28-Aug-2002 10:31   ****
                r_sAddress1 = m_oParty.Address1.Trim()
                r_sAddress2 = m_oParty.Address2.Trim()
                r_sAddress3 = m_oParty.Address3.Trim()
                r_sAddress4 = m_oParty.Address4.Trim()
                r_sPostCode = m_oParty.PostalCode.Trim()

                If Not False Then



                    oPMLookup = New bPMLookup.Business
                    m_lReturn = oPMLookup.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

                    'RJG 18/05/2000 - Quit the function if the Party object failed to initialise
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error Message
                        bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPartySiriusLink Failed - Failed to Create bPMLookup object", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPartySiriusLink", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If


                    m_lReturn = oPMLookup.GetCodeFromID(v_sTableName:=gPMConstants.PMLookupCountry, v_lID:=m_oParty.CountryId, r_sCode:=r_sCountryCode)

                    'RJG 18/05/2000 - Quit the function if the Party object failed to initialise
                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                        ' Log Error Message
                        bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to PMLookup.GetCodeFromID Failed - Failed to find entry in Country Lookup list for ID - " & m_oParty.CountryId, vApp:=ACApp, vClass:=ACClass, vMethod:="AddPartySiriusLink", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    End If

                    r_sCountryCode = r_sCountryCode.Trim()


                    oPMLookup.Dispose()

                    oPMLookup = Nothing

                End If

                r_dDOB = CStr(m_oParty.DateOfBirth).Trim()
                r_sEMail = m_oParty.EMailAddress.Trim()
                r_sUserID = m_oParty.UserID.Trim()
                r_sPassword = m_oParty.Password.Trim()
                r_sShortName = CStr(m_oParty.Shortname).Trim()
                r_sResolvedName = CStr(m_oParty.ResolvedName).Trim()

                'CLG only check password if not blank
                sPassword = r_sPassword
                If sPassword <> "" Then
                    m_lReturn = bPMFunc.Decrypt(v_sPassword:=sPassword, r_sDecryptedPassword:=r_sPassword)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                        Return result
                    End If
                End If

                If Not Informations.IsNothing(r_sMothersMaidenName) Then


                    r_sMothersMaidenName = m_oParty.MothersMaidenName
                End If


                If Not Informations.IsNothing(r_sTPUserCode) Then


                    r_sTPUserCode = m_oParty.TPUserCode
                End If


                If Not Informations.IsNothing(r_sTPIntroducer) Then


                    r_sTPIntroducer = m_oParty.TPIntroducerCode
                End If


                If Not Informations.IsNothing(r_sAQuestion) Then


                    r_sAQuestion = m_oParty.AQuestion
                End If


                If Not Informations.IsNothing(r_sTheAnswer) Then


                    r_sTheAnswer = m_oParty.TheAnswer
                End If


                If Not Informations.IsNothing(r_dMemorableDate) Then


                    r_dMemorableDate = m_oParty.MemorableDate
                End If


                If Not Informations.IsNothing(r_dCurrInsRenewalDate) Then


                    r_dCurrInsRenewalDate = m_oParty.CurrInsRenewalDate
                End If


                If Not Informations.IsNothing(r_sTitle) Then
                    r_sTitle = m_oParty.PartyTitleCode
                End If


                If Not Informations.IsNothing(r_sMaritalStatusCode) Then
                    r_sMaritalStatusCode = m_oParty.MaritalStatusCode
                End If


                If Not Informations.IsNothing(r_sGenderCode) Then
                    r_sGenderCode = m_oParty.GenderCode
                End If


                If Not Informations.IsNothing(r_sInitials) Then
                    r_sInitials = m_oParty.Initials
                End If

                If Not False Then

                    r_lSourceId = m_oParty.SourceID
                End If


                If Not Informations.IsNothing(r_sTelephoneNumber) Then
                    '            For i = 0 To UBound(m_oParty.ContactArray, 2)
                    '                'RJG 21/12/2000 - Dunno what else to do here as there could be more
                    '                'than one of these I guess but the return codes don't help me so I
                    '                'may as well just go for the last one (or first in most cases anyway)
                    '                r_sTelephoneNumber = m_oParty.ContactArray(2, i)
                    '            Next i

                    ' PWF 18/07/2001 - More reliable version of above, ensures array is populated
                    ' and returns the last value
                    If Informations.IsArray(m_oParty.ContactArray) Then


                        vContactArray = m_oParty.ContactArray



                        r_sTelephoneNumber = vContactArray(2, vContactArray.GetUpperBound(1))
                    End If
                End If
            End If


            If Not Informations.IsNothing(r_vAllContactsArray) Then


                r_vAllContactsArray = m_oParty.AllContactsArray
            End If

            r_sOccupationCode = CStr(m_oParty.OccupationCode)

            result = m_lReturn

            m_oParty.Dispose()
            m_oParty = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function GetPolicyVersionForMTA(ByRef r_lInsuranceFileCnt As Integer, ByVal v_dtDate As Date, ByRef r_lPolicyVersion As Integer, ByRef r_lErrorCode As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        ' ***************************************************************** '
        ' Name: GetPolicyVersionForMTA
        ' Date: 23/05/2000
        ' Description:  Returns the policy version we should be looking at
        ' for a given date and also the policy version number.
        ' NOTE: Error codes are returned from the SIRIUS function call
        ' if there was a problem:
        '   1 - If there are Future adjustments that are not renewals
        '   2 - If there are any current Temporary Adjustments
        '   3 - A policy version was not found
        ' ***************************************************************** '

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oFindInsurance = New bSIRFindInsurance.Form()


            m_lReturn = m_oFindInsurance.Initialise(sUsername:=SIRIUS_USERNAME, sPassword:=SIRIUS_PASSWORD, iUserID:=SIRIUS_USERID, iSourceId:=SIRIUS_SOURCEID, iLanguageId:=SIRIUS_LANGUAGEID, iCurrencyId:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            'RJG 23/05/2000 - Quit the function if the FindInsurance object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If

            m_lReturn = m_oFindInsurance.GetVersionByDate(r_lInsuranceFileCnt:=r_lInsuranceFileCnt, v_dtStartDate:=ToSafeDate(v_dtDate), r_lPolicyVersion:=r_lPolicyVersion, r_lErrorCode:=r_lErrorCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And r_lErrorCode = 0 Then
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            m_oFindInsurance.Dispose()
            m_oFindInsurance = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyVersionForMTAFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyVersionForMTA", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function GetPoliciesByType(ByRef r_vResults As Object, ByVal v_sPolicyType As String, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_IFSTInsuranceFileType As InsuranceFileSearchType = 0, Optional ByVal v_bIncludeLapsedAndCancelled As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        ' ***************************************************************** '
        ' Name: GetPoliciesByType
        ' Date: 25/05/2000
        ' Description:  Retrieves policy details by PolicyTypeID and
        ' optionally also by Party
        ' ***************************************************************** '

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oFindInsurance = New bSIRFindInsurance.Form()


            m_lReturn = m_oFindInsurance.Initialise(sUsername:=SIRIUS_USERNAME, sPassword:=SIRIUS_PASSWORD, iUserID:=SIRIUS_USERID, iSourceId:=SIRIUS_SOURCEID, iLanguageId:=SIRIUS_LANGUAGEID, iCurrencyId:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            'RJG 23/05/2000 - Quit the function if the FindInsurance object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If

            m_lReturn = m_oFindInsurance.SearchAllByType(r_vResultArray:=r_vResults, v_lPartyCnt:=ToSafeInteger(v_lPartyCnt), v_sPolicyType:=ToSafeString(v_sPolicyType), v_IFSTInsuranceFileType:=ToSafeInteger(v_IFSTInsuranceFileType), v_bIncludeLapsedAndCancelled:=ToSafeBoolean(v_bIncludeLapsedAndCancelled))

            result = m_lReturn

            m_oFindInsurance.Dispose()
            m_oFindInsurance = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPoliciesByTypeFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPoliciesByType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function GetQuotesForParty(ByVal v_lPartyCnt As Integer, ByRef r_vResults As Object, ByVal v_sPolicyType As String) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        ' ***************************************************************** '
        ' Name: GetQuotesForParty
        ' Date: 08/08/2000
        ' Description:  Retrieves Quotes for a given Party
        ' ***************************************************************** '

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'm_oFindInsurance = New bSIRFindInsurance.Form()
            m_oFindInsurance = Nothing
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oFindInsurance, v_sClassName:="bSIRFindInsurance.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim r_sMessage As String = "Failed to create an instance of bSIRFindInsurance.Form"
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessClaim", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_lReturn = m_oFindInsurance.Initialise(sUsername:=ToSafeString(SIRIUS_USERNAME), sPassword:=ToSafeString(SIRIUS_PASSWORD), iUserID:=ToSafeInteger(SIRIUS_USERID), iSourceID:=ToSafeInteger(SIRIUS_SOURCEID), iLanguageID:=ToSafeInteger(SIRIUS_LANGUAGEID), iCurrencyID:=ToSafeInteger(SIRIUS_CURRENCYID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            'RJG 08/08/2000 - Quit the function if the FindInsurance object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RJG 08/08/2000 - InsuranceFileSearchType = 8 for Quotes and MTAQTECAN
            m_lReturn = m_oFindInsurance.SearchAllByType(v_lPartyCnt:=ToSafeInteger(v_lPartyCnt), r_vResultArray:=r_vResults, v_sPolicyType:=ToSafeString(v_sPolicyType), v_IFSTInsuranceFileType:=8, v_bIncludeLapsedAndCancelled:=False)
            'RFC Do not return PMNotFound as an error
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                result = m_lReturn
            End If

            m_oFindInsurance.Dispose()
            m_oFindInsurance = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotesForPartyFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuotesForParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function
    Public Function GetQuotesAndPoliciesForParty(ByVal v_lPartyCnt As Integer, ByRef r_vResults(,) As Object, ByVal v_sPolicyType As String, ByVal v_enmReturnPolicy As ReturnedPolicy, Optional ByVal v_dtDate As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        ' ***************************************************************** '
        ' Name: GetQuotesAndPoliciesForParty
        ' Date: 09/08/2000
        ' Description:  Retrieves Quotes And Policies for a given Party
        ' ***************************************************************** '

        Dim QuotesArray(,) As Object = Nothing
        Dim PoliciesArray(,) As Object = Nothing
        Dim iPolicyIndex As Integer
        Dim blnQuotesExist As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'RJG 09/08/2000 - Get the quotes
            m_lReturn = GetQuotesForParty(v_lPartyCnt:=v_lPartyCnt, r_vResults:=QuotesArray, v_sPolicyType:=v_sPolicyType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If



            m_lReturn = GetPoliciesForParty(v_lPartyCnt:=v_lPartyCnt, r_vResults:=PoliciesArray, v_sPolicyType:=v_sPolicyType, v_enmReturnPolicy:=v_enmReturnPolicy, v_dtDate:=CDate(v_dtDate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'RJG 09/08/2000 - Dim the correct size of the results array
            If Informations.IsArray(QuotesArray) And Informations.IsArray(PoliciesArray) Then


                ReDim r_vResults(QuotesArray.GetUpperBound(0), QuotesArray.GetUpperBound(1) + PoliciesArray.GetUpperBound(1) + 1)
            Else
                If Informations.IsArray(QuotesArray) Then

                    ReDim r_vResults(QuotesArray.GetUpperBound(0), QuotesArray.GetUpperBound(1))
                Else

                    ReDim r_vResults(PoliciesArray.GetUpperBound(0), PoliciesArray.GetUpperBound(1))
                End If
            End If

            If Informations.IsArray(QuotesArray) Then

                blnQuotesExist = True
                'RJG 09/08/2000 - Copy the array

                For i As Integer = 0 To QuotesArray.GetUpperBound(1)

                    For j As Integer = 0 To QuotesArray.GetUpperBound(0)


                        r_vResults(j, i) = QuotesArray(j, i)
                    Next j
                Next i

            End If

            If Informations.IsArray(PoliciesArray) Then


                For i As Integer = 0 To PoliciesArray.GetUpperBound(1)
                    If blnQuotesExist Then

                        iPolicyIndex = i + QuotesArray.GetUpperBound(1) + 1
                    Else
                        iPolicyIndex = i
                    End If


                    For j As Integer = 0 To PoliciesArray.GetUpperBound(0)


                        r_vResults(j, iPolicyIndex) = PoliciesArray(j, i)
                    Next j
                Next i

            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotesAndPoliciesForPartyFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuotesAndPoliciesForParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function GetPoliciesForParty(ByVal v_lPartyCnt As Integer, ByRef r_vResults As Object, ByVal v_sPolicyType As String, ByVal v_enmReturnPolicy As ReturnedPolicy, Optional ByVal v_dtDate As Date = #12/30/1899#) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        ' ***************************************************************** '
        ' Name: GetPoliciesForParty
        ' Date: 08/08/2000
        ' Description:  Retrieves Policies and MTA Perms for a given Party
        ' ***************************************************************** '

        Try

            Dim ResultArray(,) As Object = Nothing
            Dim dtStartDate, dtEndDate, dtCompareDate As Date
            Dim k As Integer

            Dim lCurrentInsuranceFolderCount As Integer
            Dim iIndex, iResultIndex As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            'RJG 10/08/2000 - Do a bit of validation on the date parameter before we go any further
            If v_enmReturnPolicy = ReturnedPolicy.RPEffective Then

                If Not Informations.IsNothing(v_dtDate) Then
                    If Not Informations.IsDate(v_dtDate) Then
                        Return gPMConstants.PMEReturnCode.PMError
                    End If
                Else
                    Return gPMConstants.PMEReturnCode.PMError
                End If
            End If

            ' m_oFindInsurance = New bSIRFindInsurance.Form()
            m_oFindInsurance = Nothing
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oFindInsurance, v_sClassName:="bSIRFindInsurance.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim r_sMessage As String = "Failed to create an instance of bSIRFindInsurance.Form"
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessClaim", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_lReturn = m_oFindInsurance.Initialise(sUsername:=ToSafeString(SIRIUS_USERNAME), sPassword:=ToSafeString(SIRIUS_PASSWORD), iUserID:=ToSafeInteger(SIRIUS_USERID), iSourceID:=ToSafeInteger(SIRIUS_SOURCEID), iLanguageID:=ToSafeInteger(SIRIUS_LANGUAGEID), iCurrencyID:=ToSafeInteger(SIRIUS_CURRENCYID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            'RJG 08/08/2000 - Quit the function if the FindInsurance object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If

            'RJG 08/08/2000 - InsuranceFileSearchType = 2 for Policies and MTA Perms
            m_lReturn = m_oFindInsurance.SearchAllByType(v_lPartyCnt:=ToSafeInteger(v_lPartyCnt), r_vResultArray:=r_vResults, v_sPolicyType:=ToSafeString(v_sPolicyType), v_IFSTInsuranceFileType:=ToSafeInteger(InsuranceFileSearchType.IFSTPolicy), v_bIncludeLapsedAndCancelled:=False)

            If Informations.IsArray(r_vResults) Then

                ' RAG 170800
                ReDim ResultArray(r_vResults.GetUpperBound(0), 0)
                k = 0

                Select Case v_enmReturnPolicy
                    Case ReturnedPolicy.RPOriginal

                        For i As Integer = 0 To r_vResults.GetUpperBound(1)

                            If CStr(r_vResults(5, i)).Trim() = "POLICY" Then
                                'RJG 09/08/2000 -  We have found the original Policy copy this
                                'element of the array as it will be our results array

                                ' RAG 170800
                                'ReDim ResultArray(0 To UBound(r_vResults, 1), 0)
                                ReDim Preserve ResultArray(r_vResults.GetUpperBound(0), k)

                                For j As Integer = 0 To r_vResults.GetUpperBound(0)
                                    ' RAG 170800
                                    'ResultArray(j, 0) = r_vResults(j, i)


                                    ResultArray(j, k) = r_vResults(j, i)
                                Next j

                                ' RAG 170800
                                k += 1
                            End If

                        Next i
                    Case ReturnedPolicy.RPEffective
                        'RJG 09/08/2000 - The results are ordered by Policy start date so work
                        'backwards to find the current effective one

                        dtCompareDate = Informations.DateSerial(v_dtDate.Year, v_dtDate.Month, (v_dtDate.Day))

                        iIndex = -1
                        iResultIndex = 0

                        'Get the initial Insurance Folder count

                        lCurrentInsuranceFolderCount = ToSafeInteger(r_vResults(12, iResultIndex))

                        Do While iResultIndex <= r_vResults.GetUpperBound(1)


                            If lCurrentInsuranceFolderCount <> CDbl(r_vResults(12, iResultIndex)) Then

                                lCurrentInsuranceFolderCount = ToSafeInteger(r_vResults(12, iResultIndex))
                                If iIndex <> -1 Then
                                    ReDim Preserve ResultArray(r_vResults.GetUpperBound(0), k)
                                    For j As Integer = 0 To r_vResults.GetUpperBound(0)
                                        ' RAG 170800
                                        'ResultArray(j, 0) = r_vResults(j, i)


                                        ResultArray(j, k) = r_vResults(j, iIndex)
                                    Next j
                                    k += 1
                                    iIndex = -1
                                End If

                            End If


                            dtStartDate = Informations.DateSerial(CDate(r_vResults(10, iResultIndex)).Year, CDate(r_vResults(10, iResultIndex)).Month, (CDate(r_vResults(10, iResultIndex))).Day)

                            dtEndDate = Informations.DateSerial(CDate(r_vResults(26, iResultIndex)).Year, CDate(r_vResults(26, iResultIndex)).Month, (CDate(r_vResults(26, iResultIndex))).Day)
                            If dtStartDate <= dtCompareDate And dtEndDate >= dtCompareDate Then
                                'RJG 29/08/2000 - Set the index as we have found an effective version
                                iIndex = iResultIndex
                            End If

                            'RJG 29/08/2000 - if it's the last time round and the row is selectable copy it too the array
                            If iResultIndex = r_vResults.GetUpperBound(1) Then
                                If iIndex <> -1 Then
                                    ReDim Preserve ResultArray(r_vResults.GetUpperBound(0), k)
                                    For j As Integer = 0 To r_vResults.GetUpperBound(0)
                                        ' RAG 170800
                                        'ResultArray(j, 0) = r_vResults(j, i)


                                        ResultArray(j, k) = r_vResults(j, iIndex)
                                    Next j
                                    k += 1
                                    iIndex = -1
                                End If
                            End If

                            iResultIndex += 1
                        Loop
                    Case ReturnedPolicy.RPLatest
                        'RJG 09/08/2000 - Array is ordered by Policy start date.
                        'The last one will always be the latest

                        ' RAG 170800
                        'ReDim ResultArray(0 To UBound(r_vResults, 1), 0)

                        iResultIndex = 0
                        iIndex = 0

                        'Get the initial Insurance Folder count

                        lCurrentInsuranceFolderCount = ToSafeInteger(r_vResults(12, iResultIndex))

                        Do While iResultIndex <= r_vResults.GetUpperBound(1)

                            If lCurrentInsuranceFolderCount <> CDbl(r_vResults(12, iResultIndex)) Then

                                lCurrentInsuranceFolderCount = ToSafeInteger(r_vResults(12, iResultIndex))
                                ReDim Preserve ResultArray(r_vResults.GetUpperBound(0), k)
                                For j As Integer = 0 To r_vResults.GetUpperBound(0)
                                    ' RAG 170800
                                    'ResultArray(j, 0) = r_vResults(j, i)


                                    ResultArray(j, k) = r_vResults(j, iIndex)
                                Next j
                                k += 1
                            End If

                            iIndex = iResultIndex

                            'RJG 29/08/2000 - Catch the last instance
                            If iResultIndex = r_vResults.GetUpperBound(1) Then
                                ReDim Preserve ResultArray(r_vResults.GetUpperBound(0), k)
                                For j As Integer = 0 To r_vResults.GetUpperBound(0)


                                    ResultArray(j, k) = r_vResults(j, iIndex)
                                Next j
                            End If

                            iResultIndex += 1
                        Loop

                End Select
            End If


            r_vResults = ResultArray

            'RFC Do not return PMNotFound as an error
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                result = m_lReturn
            End If

            m_oFindInsurance.Dispose()
            m_oFindInsurance = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPoliciesForPartyFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPoliciesForParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function GetPolicyVersions(ByRef r_lInsuranceFileCnt As Integer, ByRef r_vResults As Object, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_sPolicyNumber As String = "") As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        ' ***************************************************************** '
        ' Name: GetPolicyVersions
        ' Date: 23/05/2000
        ' Description:  Returns all of the different versions of a policy
        ' which have the same InsuranceFolderCnt
        '
        ' ***************************************************************** '

        Try

            m_oFindInsurance = New bSIRFindInsurance.Form()

            m_lReturn = m_oFindInsurance.Initialise(sUsername:=SIRIUS_USERNAME, sPassword:=SIRIUS_PASSWORD, iUserID:=SIRIUS_USERID, iSourceId:=SIRIUS_SOURCEID, iLanguageId:=SIRIUS_LANGUAGEID, iCurrencyId:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim r_sMessage As String = "Failed to create an instance of bSIRFindInsurance.Form"
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessClaim", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'RJG 23/05/2000 - Quit the function if the FindInsurance object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If

            m_lReturn = m_oFindInsurance.GetVersionArray(r_lInsuranceFileCnt, r_vResults, v_lInsuranceFolderCnt, v_sPolicyNumber)

            result = m_lReturn

            m_oFindInsurance.Dispose()
            m_oFindInsurance = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindPolicyVersionFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="FindPolicyVersion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function Login(ByVal v_sUserID As String, ByRef r_lPartyCnt As Integer, ByRef r_sPartySurname As String, ByRef r_sPartyForename As String, ByRef r_sMothersMaidenName As String, ByRef r_dtDateOfBirth As Date, ByRef r_sEMail As String, ByRef r_dtCurrInsRenewal As Date, ByRef r_sPassword As String, Optional ByRef r_sPartyTypeCode As String = "") As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        ' ***************************************************************** '
        ' Name: Login
        ' Date: 09/06/2000
        ' Description:  Checks a Net Clients Login details
        ' 14/11/00 - RJG - Added optional parameter for PartyTypeCode
        ' ***************************************************************** '

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'm_oParty = New bSIRParty.Services()
            m_oParty = New bSIRParty.Services()


            m_lReturn = m_oParty.Initialise(sUsername:=SIRIUS_USERNAME, sPassword:=SIRIUS_PASSWORD, iUserID:=SIRIUS_USERID, iSourceID:=SIRIUS_SOURCEID, iLanguageID:=SIRIUS_LANGUAGEID, iCurrencyID:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If
            'developer guide no. 24
            m_oParty.UserID = v_sUserID
            m_lReturn = m_oParty.Login()

            r_lPartyCnt = m_oParty.PartyCnt

            m_lReturn = m_oParty.GetDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            r_sPartySurname = (CStr(m_oParty.Name) & "").Trim()
            r_sPartyForename = (CStr(m_oParty.Forename) & "").Trim()
            r_sMothersMaidenName = (m_oParty.MothersMaidenName & "").Trim()
            r_dtDateOfBirth = DateTime.FromOADate(m_oParty.DateOfBirth)
            r_sEMail = (m_oParty.EMailAddress & "").Trim()

            r_dtCurrInsRenewal = m_oParty.MemorableDate
            r_sPassword = (m_oParty.Password & "").Trim()

            'RJG 14/11/00 - Also Return PartyTypeCode
            r_sPartyTypeCode = (m_oParty.PartyType & "").Trim()

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oParty.Dispose()
            m_oParty = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoginFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="Login", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function NewPolicyVersion(ByVal v_lCurrentInsuranceFileCnt As Integer, ByVal v_dCoverStartDate As Date, ByVal v_dExpiryDate As Date, ByVal v_enmInsuranceFileType As InsuranceFileType, ByVal v_lPolicyVersion As Integer, ByRef r_lNewInsuranceFileCnt As Integer, ByVal v_cAnnualPremium As Decimal, Optional ByVal v_sInsuranceRef As String = "", Optional ByRef r_lInsuranceFolderCnt As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        ' ***************************************************************** '
        ' Name: NewPolicyVersion
        ' Date: 17/05/2000
        ' Description: Creates a new version of a policy given InsuranceFileCnt
        '   and some updated information and returns the InsuranceFileCnt of the
        '   new policy record.
        '
        ' ***************************************************************** '

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oQuote = New bSIRInsuranceFile.Services()

            m_lReturn = m_oQuote.Initialise(sUsername:=SIRIUS_USERNAME, sPassword:=SIRIUS_PASSWORD, iUserID:=SIRIUS_USERID, iSourceID:=SIRIUS_SOURCEID, iLanguageID:=SIRIUS_LANGUAGEID, iCurrencyID:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            'RJG 18/05/2000 - Quit the function if the quote object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If

            'RJG 17/05/2000 - Retrieve the details change selected details and create a new policy version
            m_oQuote.InsuranceFileCnt = v_lCurrentInsuranceFileCnt

            m_lReturn = m_oQuote.GetDetails()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'Set the properites for the new version of the policy

                'developer guide no. 24
                m_oQuote.CoverStartDate = v_dCoverStartDate

                'developer guide no. 24
                m_oQuote.ExpiryDate = v_dExpiryDate

                m_oQuote.InsuranceFileType = CStr(IFTCodes(v_enmInsuranceFileType - 1))

                'developer guide no. 24
                m_oQuote.PolicyVersion = v_lPolicyVersion

                'developer guide no. 24
                m_oQuote.AnnualPremium = v_cAnnualPremium

                If Not Informations.IsNothing(v_sInsuranceRef) Then
                    m_oQuote.InsuranceRef = v_sInsuranceRef
                End If

                m_lReturn = m_oQuote.CreatePolicy()
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    'The new policy has been created pass back the NewInsuranceFileCnt
                    r_lNewInsuranceFileCnt = m_oQuote.InsuranceFileCnt

                    ' RAG 03/12/2001 - Return the InsuranceFolderCnt If you want it.

                    r_lInsuranceFolderCnt = m_oQuote.InsuranceFolderCnt

                    result = gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            m_oQuote.Dispose()
            m_oQuote = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewPolicyVersionFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="NewPolicyVerson", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function Quote2Policy(ByVal v_lInsuranceFileCnt As Integer, ByVal v_IFTNewInsuranceFileType As InsuranceFileType, Optional ByVal v_sInsuranceRef As String = "", Optional ByVal v_sVehicleMakeModel As Object = Nothing, Optional ByVal v_lInsuranceHolderCnt As Object = Nothing, Optional ByVal v_lLeadInsurerABICode As String = "", Optional ByVal v_sPaymentMethod As Object = Nothing, Optional ByVal v_sInsuranceFolderDesc As Object = Nothing, Optional ByVal v_cBrokerageAmount As Object = Nothing, Optional ByVal v_cThisPremium As Object = Nothing, Optional ByVal v_cNetPremium As Object = Nothing, Optional ByVal v_cCommissionAmount As Object = Nothing, Optional ByVal v_cTaxAmount As Object = Nothing, Optional ByVal v_dCommissionPerc As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        ' ***************************************************************** '
        ' Name: Quote2Policy
        ' Date: 17/05/2000
        ' Description: Given v_lInsuranceFileCnt set the InsuranceFileTypeID
        '   to Policy from Quote (provided the PolicyIgnore flag is not set.
        '
        ' ***************************************************************** '

        ' Dim oPolicyFee As bSIRFee.Business

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oQuote = New bSIRInsuranceFile.Services()
            'Set oPolicyFee = CreateObject("bSIRFee.Business")
            m_lReturn = m_oQuote.Initialise(sUsername:=SIRIUS_USERNAME, sPassword:=SIRIUS_PASSWORD, iUserID:=SIRIUS_USERID, iSourceID:=SIRIUS_SOURCEID, iLanguageID:=SIRIUS_LANGUAGEID, iCurrencyID:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            'RJG 18/05/2000 - Quit the function if the quote object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then

                TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise returned False", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="Quote2Policy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'RJG 17/05/2000 - Retrieve the details and change the InsuranceFileTypeID from Quote to Policy
            m_oQuote.InsuranceFileCnt = v_lInsuranceFileCnt
            m_lReturn = m_oQuote.GetDetails()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'Check it is not deleted

                If m_oQuote.PolicyIgnore <> 1 Or Convert.IsDBNull(m_oQuote.PolicyIgnore) Or Informations.IsNothing(m_oQuote.PolicyIgnore) Then


                    If m_oQuote.InsuranceFileType = CStr(IFTCodes(0)) Or m_oQuote.InsuranceFileType = CStr(IFTCodes(3)) Then 'Quote or MTAQuote

                        m_oQuote.InsuranceFileType = CStr(IFTCodes(v_IFTNewInsuranceFileType - 1)) 'Policy


                        If Not Informations.IsNothing(v_sInsuranceRef) Then
                            m_oQuote.InsuranceRef = v_sInsuranceRef
                        End If


                        If Not Informations.IsNothing(v_sVehicleMakeModel) Then


                            'developer guide no. 24
                            m_oQuote.LastTransDescription = v_sVehicleMakeModel
                        End If


                        If Not Informations.IsNothing(v_lInsuranceHolderCnt) Then


                            'developer guide no. 24
                            m_oQuote.InsuranceHolderCnt = v_lInsuranceHolderCnt
                        End If


                        If Not Informations.IsNothing(v_lLeadInsurerABICode) Then
                            m_oQuote.LeadInsurerABICode = v_lLeadInsurerABICode

                            'developer guide no. 206
                            TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "Quote2Policy LeadInsurerABIcode = " & v_lLeadInsurerABICode & " LeadInsurercnt = " & m_oQuote.LeadInsurerCnt, ACApp, ACClass, "RegisterUser")
                        Else

                            'developer guide no. 206
                            TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "Quote2Policy LeadInsurerABIcode missing", ACApp, ACClass, "RegisterUser")
                        End If


                        If Not Informations.IsNothing(v_sPaymentMethod) Then


                            'developer guide no. 24
                            m_oQuote.PaymentMethod = v_sPaymentMethod
                        End If


                        If Not Informations.IsNothing(v_sInsuranceFolderDesc) Then


                            'developer guide no. 24
                            m_oQuote.InsuranceFolderDescription = v_sInsuranceFolderDesc
                        End If


                        If Not Informations.IsNothing(v_cBrokerageAmount) Then


                            'developer guide no. 24
                            m_oQuote.BrokerageAmount = v_cBrokerageAmount
                        End If


                        If Not Informations.IsNothing(v_cThisPremium) Then


                            'developer guide no. 24
                            m_oQuote.ThisPremium = v_cThisPremium
                        End If


                        If Not Informations.IsNothing(v_cNetPremium) Then


                            'developer guide no. 24
                            m_oQuote.NetPremium = v_cNetPremium
                        End If


                        If Not Informations.IsNothing(v_cCommissionAmount) Then


                            'developer guide no. 24
                            m_oQuote.CommissionAmount = v_cCommissionAmount

                            'developer guide no. 24
                            m_oQuote.IsInsurerRateTable = 1
                        End If


                        If Not Informations.IsNothing(v_cTaxAmount) Then


                            'developer guide no. 24
                            m_oQuote.TaxAmount = v_cTaxAmount
                        End If


                        If Not Informations.IsNothing(v_dCommissionPerc) Then


                            'developer guide no. 24
                            m_oQuote.CommissionPercentage = v_dCommissionPerc
                        End If

                        m_lReturn = m_oQuote.UpdatePolicy()
                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMTrue
                        Else

                            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Policy returned " & m_lReturn, sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="Quote2Policy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        End If

                        'm_lReturn = oPolicyFee.Initialise(sUsername:=SIRIUS_USERNAME, _
                        ''                    sPassword:=SIRIUS_PASSWORD, _
                        ''                    iUserID:=SIRIUS_USERID, _
                        ''                    iSourceID:=SIRIUS_SOURCEID, _
                        ''                    iLanguageID:=SIRIUS_LANGUAGEID, _
                        ''                    iCurrencyID:=SIRIUS_CURRENCYID, _
                        ''                    iLogLevel:=m_iLogLevel%, _
                        ''                    sCallingAppName:=ACApp)

                        'oPolicyFee.PartyCnt = v_lPolicyFeePartyCnt
                        'oPolicyFee.pre

                    End If
                End If

            Else

                TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails returned " & m_lReturn, sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="Quote2Policy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

            m_oQuote.Dispose()
            m_oQuote = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Quote2PolicyFailed " & Informations.Err().Number & " " & excep.Message, sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="Quote2Policy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function MTAQuote2Policy(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_sInsuranceRef As String = "", Optional ByVal v_sVehicleMakeModel As Object = Nothing, Optional ByVal v_lInsuranceHolderCnt As Object = Nothing, Optional ByVal v_sPaymentMethod As Object = Nothing, Optional ByVal v_cThisPremium As Object = Nothing, Optional ByVal v_cNetPremium As Object = Nothing, Optional ByVal v_cCommissionAmount As Object = Nothing, Optional ByVal v_dCommissionPerc As Object = Nothing, Optional ByVal v_cBrokerageAmount As Object = Nothing, Optional ByVal v_cTaxAmount As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"


        ' ***************************************************************** '
        ' Name: MTAQuote2Policy
        ' Date: 12/07/2000
        ' Description: Given v_lInsuranceFileCnt set the InsuranceFileTypeID
        '   to MTA Perm from MTA Quote (provided the PolicyIgnore flag is not set.)
        '       TF141201 - Additional parameters added to line up with NB
        ' ***************************************************************** '

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oQuote = New bSIRInsuranceFile.Services()

            m_lReturn = m_oQuote.Initialise(sUsername:=SIRIUS_USERNAME, sPassword:=SIRIUS_PASSWORD, iUserID:=SIRIUS_USERID, iSourceID:=SIRIUS_SOURCEID, iLanguageID:=SIRIUS_LANGUAGEID, iCurrencyID:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            'RJG 18/05/2000 - Quit the function if the quote object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If

            'RJG 17/05/2000 - Retrieve the details and change the InsuranceFileTypeID from Quote to Policy
            m_oQuote.InsuranceFileCnt = v_lInsuranceFileCnt
            m_lReturn = m_oQuote.GetDetails()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'Check it is not deleted

                If m_oQuote.PolicyIgnore <> 1 Or Convert.IsDBNull(m_oQuote.PolicyIgnore) Or Informations.IsNothing(m_oQuote.PolicyIgnore) Then


                    If m_oQuote.InsuranceFileType = CStr(IFTCodes(3)) Or m_oQuote.InsuranceFileType = CStr(IFTCodes(6)) Then 'MTAQuote or MTAQTETEMP


                        If m_oQuote.InsuranceFileType = CStr(IFTCodes(3)) Then

                            m_oQuote.InsuranceFileType = CStr(IFTCodes(4)) 'Permanent MTA

                            'developer guide no. 24
                            m_oQuote.PolicyVersion = m_oQuote.PolicyVersion + 1
                        Else

                            m_oQuote.InsuranceFileType = CStr(IFTCodes(5)) 'Temporary MTA
                        End If


                        If Not Informations.IsNothing(v_sInsuranceRef) Then
                            m_oQuote.InsuranceRef = v_sInsuranceRef
                        End If


                        If Not Informations.IsNothing(v_sVehicleMakeModel) Then
                            'm_oQuote.InsuranceFolderDescription = v_sVehicleMakeModel


                            'developer guide no. 24
                            m_oQuote.LastTransDescription = v_sVehicleMakeModel
                        End If


                        If Not Informations.IsNothing(v_lInsuranceHolderCnt) Then


                            'developer guide no. 24
                            m_oQuote.InsuranceHolderCnt = v_lInsuranceHolderCnt
                        End If


                        If Not Informations.IsNothing(v_sPaymentMethod) Then


                            'developer guide no. 24
                            m_oQuote.PaymentMethod = v_sPaymentMethod
                        End If


                        If Not Informations.IsNothing(v_cThisPremium) Then


                            'developer guide no. 24
                            m_oQuote.ThisPremium = v_cThisPremium
                        End If


                        If Not Informations.IsNothing(v_cNetPremium) Then


                            'developer guide no. 24
                            m_oQuote.NetPremium = v_cNetPremium
                        End If


                        If Not Informations.IsNothing(v_cCommissionAmount) Then


                            'developer guide no. 24
                            m_oQuote.CommissionAmount = v_cCommissionAmount
                        End If


                        If Not Informations.IsNothing(v_dCommissionPerc) Then


                            'developer guide no. 24
                            m_oQuote.CommissionPercentage = v_dCommissionPerc
                        End If

                        'TF141201 - Added from Quote2Policy

                        If Not Informations.IsNothing(v_cBrokerageAmount) Then


                            'developer guide no. 24
                            m_oQuote.BrokerageAmount = v_cBrokerageAmount
                        End If

                        'TF141201 - Added from Quote2Policy

                        If Not Informations.IsNothing(v_cTaxAmount) Then


                            'developer guide no. 24
                            m_oQuote.TaxAmount = v_cTaxAmount
                        End If


                        m_lReturn = m_oQuote.UpdatePolicy()
                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMTrue
                        Else

                            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Policy returned " & m_lReturn, sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote2Policy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        End If
                    End If
                End If
            End If

            m_oQuote.Dispose()
            m_oQuote = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTAQuote2PolicyFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote2Policy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function RemovePolicyVersion(ByVal v_lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        ' ***************************************************************** '
        ' Name: RemovePolicyVersion
        ' Date: 17/05/2000
        ' Description: Given InsuranceFileCnt gets the policy details and
        '   sets the PolicyIgnore flag to 1
        '
        ' ***************************************************************** '

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oQuote = New bSIRInsuranceFile.Services()

            m_lReturn = m_oQuote.Initialise(sUsername:=SIRIUS_USERNAME, sPassword:=SIRIUS_PASSWORD, iUserID:=SIRIUS_USERID, iSourceID:=SIRIUS_SOURCEID, iLanguageID:=SIRIUS_LANGUAGEID, iCurrencyID:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            'RJG 18/05/2000 - Quit the function if the quote object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If

            'RJG 17/05/2000 - Retrieve the details and set the PolicyIgnore flag to 1
            m_oQuote.InsuranceFileCnt = v_lInsuranceFileCnt
            m_lReturn = m_oQuote.GetDetails()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'developer guide no. 24
                m_oQuote.PolicyIgnore = 1
                m_lReturn = m_oQuote.UpdatePolicy()
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            m_oQuote.Dispose()
            m_oQuote = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemovePolicyVersionFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="RemovePolicyVerson", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SFUAddRisk
    ' Description: Add risk details when running in underwriting mode
    '
    ' History: 05/11/2001 PWF - Created.
    ' ***************************************************************** '
    Public Function SFUAddRisk(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lRiskFolderCnt As Integer, ByRef r_lRiskCnt As Integer, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_lOldRiskFolderCnt As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Dim sRiskTypeID As String = String.Empty
        Dim sRiskScreenID As String = String.Empty
        Dim lRiskTypeID, lRiskScreenID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Run the three procedure in a transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, Unable to Begin SQL Transaction")
            End If

            ' ***************************************************************** '
            ' BEGIN: Add the risk folder
            ' ***************************************************************** '
            ' Use the old risk folder if available
            If v_lOldRiskFolderCnt <> 0 Then
                ' Copy the folder cnt
                r_lRiskFolderCnt = v_lOldRiskFolderCnt
            Else
                ' Add parameters
                With m_oDatabase.Parameters
                    .Clear()

                    ' Set all parameters
                    m_lReturn = .Add("risk_folder_cnt", CStr(r_lRiskFolderCnt), gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMLong)
                    m_lReturn = .Add("risk_folder_id", CStr(0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    m_lReturn = .Add("source_id", CStr(1), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    m_lReturn = .Add("risk_folder_type_id", CStr(1), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    m_lReturn = .Add("code", "", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                    m_lReturn = .Add("description", "Gnet Motor", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                    m_lReturn = .Add("insurance_folder_cnt", CStr(v_lInsuranceFolderCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                End With

                ' Execute the command
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddRiskFolderSQL, sSQLName:=ACAddRiskFolderName, bStoredProcedure:=ACAddRiskFolderStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, Unable to add risk folder")
                End If

                ' Get return parameter
                r_lRiskFolderCnt = m_oDatabase.Parameters.Item("risk_folder_cnt").Value
            End If
            ' ***************************************************************** '
            ' END: Add the risk folder
            ' ***************************************************************** '

            ' ***************************************************************** '
            ' BEGIN: Add the risk
            ' ***************************************************************** '
            ' Get registry settings for risk type id
            m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=m_sGisDataModelCode, v_sBusinessTypeCode:=m_sGisBusinessTypeCode, v_sSettingName:="RiskTypeID", r_sSettingValue:=sRiskTypeID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, " + "Unable to retrieve RiskTypeID from registry. DataModelCode: " & m_sGisDataModelCode & ", BusinessTypeCode: " & m_sGisBusinessTypeCode)
            End If

            Dim dbNumericTemp As Double
            If Double.TryParse(sRiskTypeID, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                lRiskTypeID = ToSafeInteger(sRiskTypeID)
            Else
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, " + "RiskTypeID is not numeric. DataModelCode: " & m_sGisDataModelCode & ", BusinessTypeCode: " & m_sGisBusinessTypeCode)
            End If

            ' Get registry settings for risk screen id
            m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=m_sGisDataModelCode, v_sBusinessTypeCode:=m_sGisBusinessTypeCode, v_sSettingName:="RiskScreenID", r_sSettingValue:=sRiskScreenID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, " + "Unable to retrieve RiskScreenID from registry. DataModelCode: " & m_sGisDataModelCode & ", BusinessTypeCode: " & m_sGisBusinessTypeCode)
            End If

            Dim dbNumericTemp2 As Double
            If Double.TryParse(sRiskScreenID, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                lRiskScreenID = ToSafeInteger(sRiskScreenID)
            Else
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, " + "RiskScreenID is not numeric. DataModelCode: " & m_sGisDataModelCode & ", BusinessTypeCode: " & m_sGisBusinessTypeCode)
            End If

            ' Add parameters
            With m_oDatabase.Parameters
                .Clear()

                m_lReturn = .Add("risk_cnt", CStr(r_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Add("risk_status_id", CStr(3), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Add("risk_folder_cnt", CStr(r_lRiskFolderCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 85
                m_lReturn = .Add("accumulation_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Add("risk_type_id", CStr(lRiskTypeID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Add("description", "Gnet Motor", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                m_lReturn = .Add("sequence_number", CStr(1), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 85
                m_lReturn = .Add("sum_insured_requested", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 85
                m_lReturn = .Add("inception_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 85
                m_lReturn = .Add("expiry_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Add("is_not_index_linked", CStr(1), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Add("is_accumulated", CStr(1), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 85
                m_lReturn = .Add("lapsed_reason_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 85
                m_lReturn = .Add("lapsed_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 85
                m_lReturn = .Add("lapsed_description", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 85
                m_lReturn = .Add("var_data_ref", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 85
                m_lReturn = .Add("total_sum_insured", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 85
                m_lReturn = .Add("total_annual_premium", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 85
                m_lReturn = .Add("total_this_premium", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Add("is_ri_at_risk_level", CStr(1), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                ' RAG 2001-11-21 set this to 1 to make the accounts posting work.
                m_lReturn = .Add("is_auto_reinsured", CStr(1), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Add("gis_screen_id", CStr(lRiskScreenID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 85
                m_lReturn = .Add("eml_percentage", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
            End With

            ' Execute the command
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddRiskSQL, sSQLName:=ACAddRiskName, bStoredProcedure:=ACAddRiskStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, Unable to add risk")
            End If

            ' Get return parameter
            r_lRiskCnt = m_oDatabase.Parameters.Item("risk_cnt").Value
            ' ***************************************************************** '
            ' END: Add the risk
            ' ***************************************************************** '

            ' ***************************************************************** '
            ' BEGIN: Add the insurance file risk
            ' ***************************************************************** '
            ' Add parameters
            With m_oDatabase.Parameters
                .Clear()

                ' Set all parameters
                m_lReturn = .Add("insurance_file_cnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Add("risk_cnt", CStr(r_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Add("status_flag", "C", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                'developer guide no. 85
                m_lReturn = .Add("original_risk_cnt", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End With

            ' Execute the command
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsuranceFileRiskSQL, sSQLName:=ACInsuranceFileRiskName, bStoredProcedure:=ACInsuranceFileRiskStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, Unable to add insurance file risk link")
            End If
            ' ***************************************************************** '
            ' END: Add the risk folder
            ' ***************************************************************** '

            ' Run the three procedure in a transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, Unable to Commit SQL Transaction")
            End If

            ' Write a complete line and the return values to the log
            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogDebug1, sMsg:="SFUAddRisk Completed" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                   "  RiskFolderCnt: " & CStr(r_lRiskFolderCnt) & Strings.ChrW(13) & Strings.ChrW(10) &
                                                   "  RiskCnt:       " & CStr(r_lRiskCnt), vApp:=ACApp, vClass:=ACClass, vMethod:="SFUAddRisk")

            Return result

        Catch excep As System.Exception



            ' Check for sneaky warning error or real error
            If Informations.Err().Number = gPMConstants.Constants.vbObjectError Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                result = gPMConstants.PMEReturnCode.PMError
            End If

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SFUAddRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SFUAddRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            ' We will probably need to do this so call it
            m_oDatabase.SQLRollbackTrans()

            Return result

        End Try
    End Function

    Public Function UpdateCoverDetails(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_dtCoverStartDate As Object = Nothing, Optional ByVal v_dtExpiryDate As Object = Nothing, Optional ByVal v_sInsuranceRef As String = "", Optional ByVal v_sVehicleMakeModel As Object = Nothing, Optional ByVal v_lPartyCnt As Object = Nothing, Optional ByVal v_sRiskCode As String = "", Optional ByVal v_lLeadInsurerABICode As String = "", Optional ByVal v_lCurrencyID As Integer = -1, Optional ByVal v_sInsuredName As String = "") As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        ' ***************************************************************** '
        ' Name: UpdateCoverDetails
        ' Date: 27/06/2000
        ' Description:  Updates Cover Details
        '
        ' ***************************************************************** '
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oQuote = New bSIRInsuranceFile.Services()

            m_lReturn = m_oQuote.Initialise(sUsername:=SIRIUS_USERNAME, sPassword:=SIRIUS_PASSWORD, iUserID:=SIRIUS_USERID, iSourceID:=SIRIUS_SOURCEID, iLanguageID:=SIRIUS_LANGUAGEID, iCurrencyID:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            'RJG 18/05/2000 - Quit the function if the quote object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If

            'RJG 27/06/2000 - Retrieve the details change selected details
            m_oQuote.InsuranceFileCnt = v_lInsuranceFileCnt

            m_lReturn = m_oQuote.GetDetails()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'Set the properites for the new version of the policy

                If Not Informations.IsNothing(v_dtCoverStartDate) Then


                    'developer guide no. 24
                    m_oQuote.CoverStartDate = v_dtCoverStartDate


                    'developer guide no. 24
                    m_oQuote.InceptionDate = v_dtCoverStartDate


                    'developer guide no. 24
                    m_oQuote.InceptionTPI = v_dtCoverStartDate
                End If


                If Not Informations.IsNothing(v_dtExpiryDate) Then


                    'developer guide no. 24
                    m_oQuote.ExpiryDate = v_dtExpiryDate


                    'developer guide no. 24
                    m_oQuote.RenewalDate = v_dtExpiryDate
                End If


                If Not Informations.IsNothing(v_sInsuranceRef) Then
                    m_oQuote.InsuranceRef = v_sInsuranceRef
                End If


                If Not Informations.IsNothing(v_lPartyCnt) Then


                    'developer guide no. 24
                    m_oQuote.InsuranceHolderCnt = v_lPartyCnt


                    'developer guide no. 24
                    m_oQuote.InsuredCnt = v_lPartyCnt
                End If


                If Not Informations.IsNothing(v_sVehicleMakeModel) Then
                    'm_oQuote.InsuranceFolderDescription = v_sVehicleMakeModel


                    'developer guide no. 24
                    m_oQuote.LastTransDescription = v_sVehicleMakeModel
                End If


                If Not Informations.IsNothing(v_sRiskCode) Then
                    If v_sRiskCode.ToUpper() = "TPFT" Then
                        m_oQuote.RiskCode = CStr(292)
                    ElseIf v_sRiskCode.ToUpper() = "TPO " Then
                        m_oQuote.RiskCode = CStr(291)
                    Else
                        m_oQuote.RiskCode = CStr(293) 'COMP
                    End If
                End If


                If Not Informations.IsNothing(v_lLeadInsurerABICode) Then
                    m_oQuote.LeadInsurerABICode = v_lLeadInsurerABICode
                End If

                m_oQuote.RenewalFrequency = POLICY_RENEWAL_FREQUENCY

                ' If the CurrecyID has not been supplied then do not update the existing value.
                If v_lCurrencyID > 0 Then

                    'developer guide no. 24
                    m_oQuote.CurrencyID = v_lCurrencyID
                End If

                If Not False Then

                    'developer guide no. 24
                    m_oQuote.InsuredName = v_sInsuredName
                End If

                m_lReturn = m_oQuote.UpdatePolicy()

                result = m_lReturn
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateCoverDetailsFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCoverDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function UpdateParty(ByVal v_lPartyCnt As Integer, Optional ByVal v_sSurname As Integer = 0, Optional ByVal v_sForename As Integer = 0, Optional ByVal v_sShortname As Integer = 0, Optional ByVal v_sAddress1 As String = "", Optional ByVal v_sAddress2 As String = "", Optional ByVal v_sAddress3 As String = "", Optional ByVal v_sAddress4 As String = "", Optional ByVal v_sPostCode As String = "", Optional ByVal v_dDOB As Integer = 0, Optional ByVal v_sEMail As String = "", Optional ByVal v_sUserID As Object = Nothing, Optional ByVal v_sPassword As Object = Nothing, Optional ByVal v_sMothersMaidenName As Object = Nothing, Optional ByVal v_sTPUserCode As Object = Nothing, Optional ByVal v_sTPIntroducer As String = "", Optional ByVal v_sAQuestion As Object = Nothing, Optional ByVal v_sTheAnswer As Object = Nothing, Optional ByVal v_dMemorableDate As Date = #12/30/1899#, Optional ByVal v_dCurrInsRenewalDate As Date = #12/30/1899#, Optional ByVal v_sTitle As String = "", Optional ByVal v_sMaritalStatusCode As String = "", Optional ByVal v_sGenderCode As Integer = 0, Optional ByVal v_sInitials As Integer = 0, Optional ByVal v_sTelephoneNumber As String = "", Optional ByVal v_bIsProspect As Object = Nothing, Optional ByVal v_lAgentCnt As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        ' ***************************************************************** '
        ' Name: UpdateParty
        ' Date: 12/06/2000
        ' Description:  Updates Party Details
        '
        ' RFC050900 - Added Title & MaritalStatusCode optional parameters.
        ' AAB-09-Oct-2002 14:24 - Added Lead Agent Cnt optional parameter.
        ' ***************************************************************** '
        Try

            Dim vContactArray(,) As Object = Nothing
            Dim iContactIndex As Integer

            result = gPMConstants.PMEReturnCode.PMFalse

            m_oParty = New bSIRParty.Services()


            m_lReturn = m_oParty.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            'RJG 12/06/2000 - Quit the function if the Party object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If

            'RJG 12/06/2000 - Get the Party Details from the Party
            m_oParty.PartyCnt = v_lPartyCnt
            m_lReturn = m_oParty.GetDetails()

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If

            'RJG 12/06/2000 - Set the necessary properties for the party object

            If Not Informations.IsNothing(v_sSurname) Then
                m_oParty.Name = v_sSurname
            End If


            If Not Informations.IsNothing(v_sForename) Then
                m_oParty.Forename = v_sForename
            End If


            If Not Informations.IsNothing(v_sShortname) Then
                m_oParty.Shortname = v_sShortname
            End If


            If Not Informations.IsNothing(v_sAddress1) Then
                m_oParty.Address1 = v_sAddress1
            End If


            If Not Informations.IsNothing(v_sAddress2) Then
                m_oParty.Address2 = v_sAddress2
            End If


            If Not Informations.IsNothing(v_sAddress3) Then
                m_oParty.Address3 = v_sAddress3
            End If


            If Not Informations.IsNothing(v_sAddress4) Then
                m_oParty.Address4 = v_sAddress4
            End If


            If Not Informations.IsNothing(v_sPostCode) Then
                m_oParty.PostalCode = v_sPostCode
            End If


            If Not Informations.IsNothing(v_dDOB) Then
                m_oParty.DateOfBirth = v_dDOB
            End If


            If Not Informations.IsNothing(v_sUserID) Then


                'developer guide no. 24
                m_oParty.UserID = v_sUserID
            End If


            If Not Informations.IsNothing(v_sPassword) Then


                'developer guide no. 24
                m_oParty.Password = v_sPassword
            End If


            If Not Informations.IsNothing(v_sMothersMaidenName) Then


                'developer guide no. 24
                m_oParty.MothersMaidenName = v_sMothersMaidenName
            End If


            If Not Informations.IsNothing(v_sTPUserCode) Then


                'developer guide no. 24
                m_oParty.TPUserCode = v_sTPUserCode
            End If


            If Not Informations.IsNothing(v_sTPIntroducer) Then

                'developer guide no. 24
                m_oParty.TPIntroducerCode = v_sTPIntroducer

                'developer guide no. 206
                TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "UpdateParty TPIntroducerCode = " & v_sTPIntroducer, ACApp, ACClass, "RegisterUser")
            Else

                'developer guide no. 206
                TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "UpdateParty TPIntroducerCode missing", ACApp, ACClass, "RegisterUser")
            End If


            If Not Informations.IsNothing(v_sAQuestion) Then


                'developer guide no. 24
                m_oParty.AQuestion = v_sAQuestion
            End If


            If Not Informations.IsNothing(v_sTheAnswer) Then


                'developer guide no. 24
                m_oParty.TheAnswer = v_sTheAnswer
            End If


            If Not Informations.IsNothing(v_dMemorableDate) Then
                If Informations.IsDate(v_dMemorableDate) Then

                    'developer guide no. 24
                    m_oParty.MemorableDate = Informations.DateSerial(v_dMemorableDate.Year, v_dMemorableDate.Month, v_dMemorableDate.Day)
                End If
            End If


            If Not Informations.IsNothing(v_dCurrInsRenewalDate) Then
                If Informations.IsDate(v_dCurrInsRenewalDate) Then

                    'developer guide no. 24
                    m_oParty.CurrInsRenewalDate = Informations.DateSerial(v_dCurrInsRenewalDate.Year, v_dCurrInsRenewalDate.Month, (v_dCurrInsRenewalDate.Day))
                End If
            End If

            ' RFC050900 - Added Title & MaritalStatusCode optional parameters - START

            If Not Informations.IsNothing(v_sTitle) Then

                'developer guide no. 24
                m_oParty.PartyTitle = v_sTitle.Trim()
            End If


            If Not Informations.IsNothing(v_sMaritalStatusCode) Then
                m_oParty.MaritalStatusCode = ToSafeInteger(v_sMaritalStatusCode.Trim())
            End If
            ' RFC050900 - Added Title & MaritalStatusCode optional parameters - END

            ' GRW 04/10/2000 - Added Gender Code and Initials added as optional parameters

            If Not Informations.IsNothing(v_sGenderCode) Then
                m_oParty.GenderCode = v_sGenderCode
            End If


            If Not Informations.IsNothing(v_sInitials) Then
                m_oParty.Initials = v_sInitials
            End If

            'GRW 04/10/2000 - Added Gender Code and Initials added as optional parameters - END

            'RJG 12/06/2000 - Add E-Mail as a contact
            '**** START CHANGES - Changed By: AAB  - Changed On: 09-Oct-2002 14:26   ****
            '**** I copied the code from the AddParty method
            'SPW 010705 changed line below trim on empty string causes error
            'If Trim$(v_sEMail) <> "" Then

            If Not Informations.IsNothing(v_sEMail) Then
                ' RAGFIX
                'ReDim vContactArray(3, iContactIndex)
                ReDim vContactArray(4, iContactIndex)


                vContactArray(0, iContactIndex) = "E-MAIL"

                vContactArray(1, iContactIndex) = ""

                vContactArray(2, iContactIndex) = v_sEMail

                vContactArray(3, iContactIndex) = ""
                ' RAGFIX

                vContactArray(4, iContactIndex) = "Email"

                iContactIndex += 1
            End If


            If Not Informations.IsNothing(v_sTelephoneNumber) Then
                If v_sTelephoneNumber.Trim() <> "" Then
                    If iContactIndex Then
                        ' RAGFIX
                        'ReDim Preserve vContactArray(3, iContactIndex)
                        ReDim Preserve vContactArray(4, iContactIndex)
                    Else
                        ' RAGFIX
                        'ReDim vContactArray(3, iContactIndex)
                        ReDim vContactArray(4, iContactIndex)
                    End If


                    vContactArray(0, iContactIndex) = "TELEPHONE"

                    vContactArray(1, iContactIndex) = ""

                    vContactArray(2, iContactIndex) = v_sTelephoneNumber

                    vContactArray(3, iContactIndex) = ""

                    ' RAGFIX

                    vContactArray(4, iContactIndex) = "TelNo"

                    iContactIndex += 1
                End If
            End If



            'developer guide no. 24
            m_oParty.ContactArray = vContactArray


            If Not Informations.IsNothing(v_lAgentCnt) Then
                If v_lAgentCnt <> 0 Then
                    m_oParty.AgentCnt = v_lAgentCnt
                End If
            End If
            '****   END CHANGES - Changed By: AAB  - Changed On: 09-Oct-2002 14:26   ****


            'BD - IsProspect flag

            If Not Informations.IsNothing(v_bIsProspect) Then


                'developer guide no. 24
                m_oParty.IsProspect = v_bIsProspect
            End If

            m_lReturn = m_oParty.UpdateParty()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            m_oParty.Dispose()
            m_oParty = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePartyFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearPartyDefaults
    ' Date: 06/12/2001
    '
    ' Description: Clear payment defaults generated by SBO
    '               and not required by GNET
    '
    ' History: TF061201 - Created.
    ' ***************************************************************** '
    Public Function ClearPartyDefaults(ByVal v_lPartyCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Dim oParty As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'With oCS


            'oParty = New bSIRParty.Business

            result = gPMComponentServices.CreateBusinessObject(r_oObject:=oParty, v_sClassName:="bSIRParty.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim r_sMessage As String = "Failed to create an instance of bSIRInsuranceFile.Business"
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessClaim", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_lReturn = m_oParty.Initialise(sUsername:=m_sUserName.ToString(), sPassword:=m_sPassword.ToString(), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=CStr(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                'developer guide no. 206
                TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "Failed to create bSIRParty.Business", ACApp, ACClass, "ClearPartyDefaults")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'End With

            With oParty

                m_lReturn = .GetDetails(vPartyCnt:=ToSafeInteger(v_lPartyCnt))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    'developer guide no. 206
                    TempFunc.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oParty.GetDetails Failed", ACApp, ACClass, "ClearPartyDefaults")
                    result = gPMConstants.PMEReturnCode.PMFalse

                    .Dispose()
                    oParty = Nothing
                    Return result
                End If


                m_lReturn = .GetNext()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    'developer guide no. 206
                    TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "oParty.GetNext Failed", ACApp, ACClass, "ClearPartyDefaults")
                    result = gPMConstants.PMEReturnCode.PMFalse

                    .Dispose()
                    oParty = Nothing
                    Return result
                End If



                m_lReturn = .EditUpdate(lRow:=1, vPaymentMethodCode:=DBNull.Value, vPaymentTermCode:=DBNull.Value, vRenewalStopCodeId:=DBNull.Value)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    'developer guide no. 206
                    TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "oParty.EditUpdate Failed", ACApp, ACClass, "ClearPartyDefaults")
                    result = gPMConstants.PMEReturnCode.PMFalse

                    .Dispose()
                    oParty = Nothing
                    Return result
                End If


                m_lReturn = .Update()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    'developer guide no. 206
                    TempFunc.LogMessageFile(m_sUserName, gPMConstants.PMELogLevel.PMLogOnError, "oParty.Update Failed", ACApp, ACClass, "ClearPartyDefaults")
                    result = gPMConstants.PMEReturnCode.PMFalse

                    .Dispose()
                    oParty = Nothing
                    Return result
                End If


                .Dispose()
            End With
            oParty = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearPartyDefaults Failed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="ClearPartyDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePartyRef
    ' Date: 03/07/2000
    '
    ' Description: Update the ins holder cnt of the ins.folder for a
    ' given ins.file and party cnt
    '
    ' Author: CL
    ' ***************************************************************** '

    Public Function UpdatePartyCnt(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oQuote = New bSIRInsuranceFile.Services()

            m_lReturn = m_oQuote.Initialise(sUsername:=SIRIUS_USERNAME, sPassword:=SIRIUS_PASSWORD, iUserID:=SIRIUS_USERID, iSourceID:=SIRIUS_SOURCEID, iLanguageID:=SIRIUS_LANGUAGEID, iCurrencyID:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            'RJG 18/05/2000 - Quit the function if the quote object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If

            m_oQuote.InsuranceFileCnt = v_lInsuranceFileCnt

            lReturn = m_oQuote.GetDetails() ' Load the details for this ins file
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'developer guide no. 24
            m_oQuote.InsuranceHolderCnt = v_lPartyCnt ' Set the new party count

            lReturn = m_oQuote.UpdatePolicy() ' Commit changes
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oQuote.Dispose()
            m_oQuote = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePartyCnt Failed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePartyCnt", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCompulsoryAddons
    '
    ' Description:  Get compulsory add-ons applicable to given Insurance File.
    '               Administration fees/Introductory Fees (NB Only)
    '
    ' History:  TF200202 - Created
    ' ***************************************************************** '
    Public Function GetCompulsoryAddons(ByVal v_lInsuranceFileCnt As Integer, ByRef r_cPremium As Decimal, ByRef r_cIPT As Decimal, Optional ByVal v_bAddToPolicy As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Dim cFeePercentage, cFeeAmount As Decimal
        Dim lPartyCnt As Integer
        Dim cCommPercentage, cCommAmount As Decimal
        Dim vFeeArray As Object = Nothing

        '0  -   Not used
        '1  -   Not used
        '2  -   Fee Percentage
        '3  -   Fee Amount
        '4  -   Party Cnt
        '5  -   Commission Percentage
        '6  -   Commission Amount
        '7  -   Is IPTable

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMError

                    TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter 'insurance_file_cnt'", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompulsoryAddons", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = .SQLSelect(sSQL:=ACGetFeeAmountsSQL, sSQLName:=ACGetFeeAmountsName, bStoredProcedure:=ACGetFeeAmountsStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMError

                    TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process query " & ACGetFeeAmountsSQL, sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompulsoryAddons", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If


                If v_bAddToPolicy Then
                    ReDim vFeeArray(7, .Records.Count())
                End If

                For lLoop As Integer = 1 To .Records.Count()
                    cFeePercentage = gPMFunctions.NullToDecimal(.Records.Item(lLoop).Fields()("fee_percentage"))
                    cFeeAmount = gPMFunctions.NullToDecimal(.Records.Item(lLoop).Fields()("fee_amount"))

                    r_cPremium += r_cPremium * cFeePercentage / 100
                    r_cPremium += cFeeAmount

                    If v_bAddToPolicy Then

                        vFeeArray(2, lLoop) = cFeePercentage

                        vFeeArray(3, lLoop) = cFeeAmount

                        vFeeArray(4, lLoop) = lPartyCnt

                        vFeeArray(5, lLoop) = cCommPercentage

                        vFeeArray(6, lLoop) = cCommAmount

                        vFeeArray(7, lLoop) = False
                    End If
                Next lLoop
            End With

            m_oInsuranceFileBusiness = New bSIRInsuranceFile.Business()

            m_lReturn = m_oInsuranceFileBusiness.Initialise(sUsername:=SIRIUS_USERNAME, sPassword:=SIRIUS_PASSWORD, iUserID:=SIRIUS_USERID, iSourceID:=SIRIUS_SOURCEID, iLanguageID:=SIRIUS_LANGUAGEID, iCurrencyID:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create 'bSIRInsuranceFile.Business'.", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompulsoryAddons", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If



            m_lReturn = m_oInsuranceFileBusiness.AddFees(vInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt), vFees:=vFeeArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process 'm_oInsuranceFileBusiness.AddFees'.", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompulsoryAddons", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCompulsoryAddons Failed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompulsoryAddons", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function


    Public Sub New()
        MyBase.New()

        'Populate the IFT Codes array

        IFTCodes(0) = "QUOTE"

        IFTCodes(1) = "POLICY"

        IFTCodes(2) = "RENEWAL"

        IFTCodes(3) = "MTAQUOTE"

        IFTCodes(4) = "MTA PERM"

        IFTCodes(5) = "MTA TEMP"

        IFTCodes(6) = "MTAQTETEMP"

        IFTCodes(7) = "MTAQTECAN"

        IFTCodes(8) = "MTAQTEREIN"

        IFTCodes(9) = "MTAPERMCAN" ' CL191000

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub



    Public Function LoginAgent(ByVal v_sUsername As String, ByVal v_sPassword As String, ByRef r_lAgentCnt As Integer, ByRef r_lPMUserID As Integer, ByRef r_bUnrestrictedSearch As Boolean, ByRef r_dtPasswordChangeDate As Date, ByRef r_dtLastlogin As Date, ByRef r_sForename As String, ByRef r_sSurname As String, ByRef r_sEmailAddress As String, ByRef r_iLanguageId As Integer, ByRef r_vSourceList As Object, ByRef r_vAdditionalDataArray As Object) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        '******************************************************************************
        '        Function Name:  LoginAgent
        '******************************************************************************
        '           Created By:  Ahmed "Jay" Bishatwi
        '           Created On:  20-Sep-2002
        '******************************************************************************
        '       Parameters Are:
        '                        (In)     - v_sUserName            - Variant  -
        '                        (In)     - v_sPassword            - Variant  -
        '                        (In/Out) - r_lAgentCnt            - Variant  -
        '                        (In/Out) - r_lPMUserID            - Variant  -
        '                        (In/Out) - r_bUnrestrictedSearch  - Variant  -
        '                        (In/Out) - r_dtPasswordChangeDate - Variant  -
        '                        (In/Out) - r_dtLastlogin          - Variant  -
        '                        (In/Out) - r_sForename            - Variant  -
        '                        (In/Out) - r_sSurname             - Variant  -
        '                        (In/Out) - r_sEmailAddress        - Variant  -
        '                        (In/Out) - r_iLanguageId          - Variant  -
        '                        (In/Out) - r_vSourceList          - Variant  -
        '                        (In/Out) - r_vAdditionalDataArray - Variant  -
        '
        ' Return Value Type Is:  Long -
        '******************************************************************************
        ' Function Description:  This function logs agent into Agents OnLine Website.
        '******************************************************************************
        Dim lReturn As Integer
        Dim oUser As Bpmuser.Business = Nothing
        Dim oUserGroup As bPMUserGroup.Utilities = Nothing
        Dim sErrorMessage As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sEncryptedPassword As String = String.Empty
            Dim sPassword As String
            Dim iLanguageID As Integer
            Dim sUsername As String = ""
            Dim dtEffectiveDate As Date
            Dim lAgentCnt, lUserID As Integer
            Dim dtPasswordChangeDate, dtLastlogin As Date
            Dim MemberOf As Boolean
            'Variables needed for GetParty Method
            Dim sASurName As String = String.Empty
            Dim sAForeName As String = String.Empty
            Dim sPartyType As String = String.Empty
            Dim sAAddress1 As String = String.Empty
            Dim sAAddress2 As String = String.Empty
            Dim sAAddress3 As String = String.Empty
            Dim sAAddress4 As String = String.Empty
            Dim sAPostCode As String = String.Empty
            Dim dtADOB As String = String.Empty
            Dim sAEMail As String = String.Empty
            Dim sAUserID As String = String.Empty
            Dim sAPassword As String = String.Empty
            Dim sAShortName As String = String.Empty
            Dim sAResolvedName As String = String.Empty
            'AAB-16-Oct-2002 14:05 - to log a clearer error message

            sUsername = v_sUsername
            dtEffectiveDate = DateTime.Now

            ' CTAF 20030402 - Changed to use ComponentServices


            oUser = New Bpmuser.Business
            lReturn = oUser.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get instance of bPMUser.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            '    Set oUser = CreateObject("bPMUser.Business")
            '    If (oUser Is Nothing) = True Then
            '        LoginAgent = PMFalse
            '        Err.Raise vbObjectError, "Internal", "LoginAgent Failed to Create bPMUser.Business Object"
            '    End If
            '
            '    'Initialize the object
            '    lReturn = oUser.Initialise(sUsername:=SIRIUS_USERNAME, _
            ''                               sPassword:=SIRIUS_PASSWORD, _
            ''                               iUserID:=SIRIUS_USERID, _
            ''                               iSourceID:=SIRIUS_SOURCEID, _
            ''                               iLanguageID:=SIRIUS_LANGUAGEID, _
            ''                               iCurrencyID:=SIRIUS_CURRENCYID, _
            ''                               iLogLevel:=SIRIUS_LOGLEVEL, _
            ''                               sCallingAppName:=ACApp)
            '    If (lReturn <> PMTrue) Then
            '        LoginAgent = lReturn
            '        Err.Raise vbObjectError, "Internal", "bPMUser.Business Object Failed to Initialize"
            '    End If

            'Encrypt the password
            sPassword = v_sPassword
            'AAB-16-Oct-2002 14:46 - Added the named paramters
            lReturn = bPMFunc.Encrypt(sPassword:=sPassword, sEncryptedPassword:=sEncryptedPassword)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="LoginAgent Method Failed to Encrypt Password", vApp:=ACApp, vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Check Logon for the user and password passed in.

            lReturn = oUser.CheckLogon(sCheckUsername:=sUsername, sCheckPassword:=sEncryptedPassword, dtEffectiveFrom:=dtEffectiveDate, iLanguageID:=iLanguageID, lPartyCnt:=lAgentCnt, vUserId:=lUserID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="bPMUser.Business.CheckLogon Method Failed for username - " & sUsername, vApp:=ACApp, vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Get the details of the user
            'AAB-16-Oct-2002 14:48 - Added the named Parameter

            lReturn = oUser.GetDetails(vUserId:=lUserID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="bPMUser.Business.GetDetails Method Failed for User - " & lUserID, vApp:=ACApp, vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Get the Passwordchangedate and the last login date

            lReturn = oUser.GetNext(vUserId:=lUserID, vDateCreated:=dtPasswordChangeDate, vLastLogin:=dtLastlogin)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="bPMUser.Business.GetNext Method Failed for Userid - " & lUserID, vApp:=ACApp, vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            lReturn = GetParty(lAgentCnt, r_sSurname:=sASurName, r_sForename:=sAForeName, r_sPartyType:=sPartyType, r_sAddress1:=sAAddress1, r_sAddress2:=sAAddress2, r_sAddress3:=sAAddress3, r_sAddress4:=sAAddress4, r_sPostCode:=sAPostCode, r_dDOB:=dtADOB, r_sEMail:=sAEMail, r_sUserID:=sAUserID, r_sPassword:=sAPassword, r_sShortName:=sAShortName, r_sResolvedName:=sAResolvedName)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    result = gPMConstants.PMEReturnCode.PMUserNotLinkedAgent
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="User '" & v_sUsername & "' is not associated with an agent party record.", vApp:=ACApp, vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="bPMUser.Business.GetParty Method Failed for Agent Cnt - " & lAgentCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'I am commenting this line out because the GetParty method does NOT return the party type.
            '    If sPartyType <> SIRIUS_AGENT_TYPE_CODE Then
            '        LoginAgent = PMFalse
            '        Exit Function
            '    End If

            '    Set oUserGroup = CreateObject("bPMUserGroup.Utilities")
            '    If (oUserGroup Is Nothing) = True Then
            '        LoginAgent = PMFalse
            '        Err.Raise vbObjectError, "Internal", "LoginAgent Failed to Create bPMUserGroup.Utilities Object"
            '    End If
            '    lReturn = oUserGroup.Initialise(sUsername:=SIRIUS_USERNAME, _
            ''                                    sPassword:=SIRIUS_PASSWORD, _
            ''                                    iUserID:=SIRIUS_USERID, _
            ''                                    iSourceID:=SIRIUS_SOURCEID, _
            ''                                    iLanguageID:=SIRIUS_LANGUAGEID, _
            ''                                    iCurrencyID:=SIRIUS_CURRENCYID, _
            ''                                    iLogLevel:=SIRIUS_LOGLEVEL, _
            ''                                    sCallingAppName:=ACApp)
            '    If (lReturn <> PMTrue) Then
            '        LoginAgent = lReturn
            '        Err.Raise vbObjectError, "Internal", "bPMUserGroup.Utilities Object Failed to Initialize"
            '    End If

            ' CTAF 20030402 - Changed to use ComponentServices


            oUserGroup = New bPMUserGroup.Utilities
            lReturn = oUserGroup.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get instance of bPMUserGroup.Utilities", vApp:=ACApp, vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'AAB-16-Oct-2002 14:50 - Added the named parameters

            lReturn = oUserGroup.IsUserIdMemberOfGroup(v_iUserID:=lUserID, v_sGroupCode:="AOLSuper", r_bUserIsMember:=MemberOf)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="bPMUserGroup.Utilities.IsUserIDMemberOfGroup Method Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Retrieve the branches for the given user.

            lReturn = oUser.GetUserSources(r_vSourceArray:=r_vSourceList, v_vUserID:=lUserID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="bPMUser.Business.GetUserSources Method Failed for User - " & sUsername, vApp:=ACApp, vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'After we verified everything we need then we logon the user.
            'Logon the user for the time being we are going to leave the client as AgentsOnLine

            lReturn = oUser.Logon(v_sUsername:=sUsername, v_sLoggedOnAtClient:="AgentsOnLine")
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="bPMUser.Business.Logon Method Failed for User - " & sUsername, vApp:=ACApp, vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Set the ByRef parameters
            r_lAgentCnt = lAgentCnt
            r_lPMUserID = lUserID
            r_bUnrestrictedSearch = True
            r_dtPasswordChangeDate = dtPasswordChangeDate
            r_dtLastlogin = dtLastlogin
            r_sForename = ""
            r_sSurname = ""
            r_sEmailAddress = sAEMail
            r_iLanguageId = iLanguageID
            r_bUnrestrictedSearch = MemberOf

            'Set the created object to nothing

            oUser.Dispose()
            oUser = Nothing

            oUserGroup.Dispose()
            oUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            'AAB-16-Oct-2002 14:06 - To add a clearer error message
            If Informations.Err().Number = gPMConstants.Constants.vbObjectError Then
                sErrorMessage = excep.Message
            Else
                result = gPMConstants.PMEReturnCode.PMError
                sErrorMessage = "LoginAgent Failed"
            End If

            ' LoginAgent Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage, sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            'AAB-16-Oct-2002 11:57 - Destroy the objects we created in case of error
            If Not (oUser Is Nothing) Then

                oUser.Dispose()
                oUser = Nothing
            End If
            If Not (oUserGroup Is Nothing) Then

                oUserGroup.Dispose()
                oUserGroup = Nothing
            End If

            Return result
        End Try
    End Function
    Public Function LogoffAgent(ByVal v_sUsername As Object) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        '******************************************************************************
        '        Function Name:  LogoffAgent
        '******************************************************************************
        '           Created By:  Ahmed "Jay" Bishatwi
        '           Created On:  20-Sep-2002
        '******************************************************************************
        '       Parameters Are:
        '                        (In) - v_sUserName - Variant  -
        '
        ' Return Value Type Is:  Long -
        '******************************************************************************
        ' Function Description:  This functions is used to log agents off the Agents
        '                        Online web site.
        '******************************************************************************

        Dim oUser As Bpmuser.Business = Nothing
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sErrorMessage As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AAB-16-Oct-2002 14:11 - To log a clearer error message

            oUser = New Bpmuser.Business()
            If oUser Is Nothing Then
                'AAB-16-Oct-2002 12:14 - Log a clearer error message.
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, LogoffAgent Failed to Create bPMUser.Business object")
            End If

            'Initialize the object

            lReturn = oUser.Initialise(sUsername:=ToSafeString(SIRIUS_USERNAME), sPassword:=ToSafeString(SIRIUS_PASSWORD), iUserID:=ToSafeInteger(SIRIUS_USERID), iSourceID:=ToSafeInteger(SIRIUS_SOURCEID), iLanguageID:=ToSafeInteger(SIRIUS_LANGUAGEID), iCurrencyID:=ToSafeInteger(SIRIUS_CURRENCYID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            'Exit function if we failed to initialize object.
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AAB-16-Oct-2002 12:15 - Log a clearer error message.
                result = lReturn
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, bPMUser.Business Object Failed to Initialize")
            End If


            lReturn = oUser.Logoff(v_sUsername:=CStr(v_sUsername))
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AAB-16-Oct-2002 12:15 -  Log a clearer error message.
                result = lReturn
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, bPMUser.Business.Logoff Method Failed")
            End If

            'Set the created object to nothing
            oUser.Dispose()
            oUser = Nothing

            Return result

        Catch excep As System.Exception


            'AAB-16-Oct-2002 14:12 - To log a clearer error message
            If Informations.Err().Number = gPMConstants.Constants.vbObjectError Then
                sErrorMessage = excep.Message
            Else
                result = gPMConstants.PMEReturnCode.PMError
                sErrorMessage = "LogoffAgent Failed"
            End If

            ' LoginAgent Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage, sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="LogoffAgent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            'AAB-16-Oct-2002 12:14 - Destroy the object in case of error.
            If Not (oUser Is Nothing) Then
                oUser.Dispose()
                oUser = Nothing
            End If
            Return result
        End Try
    End Function

    Public Function UpdateAgentLogonDetails(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_sNewPassword As String) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        '******************************************************************************
        '        Function Name:  UpdateAgentLogonDetails
        '******************************************************************************
        '           Created By:  Ahmed "Jay" Bishatwi
        '           Created On:  20-Sep-2002
        '******************************************************************************
        '       Parameters Are:
        '                        (In) - v_sUserName    - Variant  -
        '                        (In) - v_sPassword    - Variant  -
        '                        (In) - v_sNewPassword - Variant  -
        '
        ' Return Value Type Is:  Long -
        '******************************************************************************
        ' Function Description:  This method is used to update the Agent Password in
        '                        Agents OnLine website.
        '******************************************************************************

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oUser As Bpmuser.Business = Nothing
        Dim sErrorMessage As String = String.Empty
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sEncryptedPassword As String = String.Empty
            Dim sPassword As String
            Dim iLanguageID As Integer
            Dim sUsername As String = ""
            Dim dtEffectiveDate As Date
            Dim lAgentCnt, lUserID As Integer

            'AAB-16-Oct-2002 14:16 - to add a clearer error message

            sUsername = v_sUsername
            'RDT 10122002 - Removed the formating as it causes problem on systems with other locales.
            dtEffectiveDate = DateTime.Now

            oUser = New Bpmuser.Business()
            If oUser Is Nothing Then
                'AAB-16-Oct-2002 13:20 - Added Error Trapping
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, UpdateAgentLogonDetails Failed to create bPMUser.Business object.")
            End If

            'Initialize the object

            lReturn = oUser.Initialise(sUsername:=ToSafeString(SIRIUS_USERNAME), sPassword:=ToSafeString(SIRIUS_PASSWORD), iUserID:=ToSafeInteger(SIRIUS_USERID), iSourceID:=ToSafeInteger(SIRIUS_SOURCEID), iLanguageID:=ToSafeInteger(SIRIUS_LANGUAGEID), iCurrencyID:=ToSafeInteger(SIRIUS_CURRENCYID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            'Exit function if we failed to initialize object.
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AAB-16-Oct-2002 13:20 - Added Error Trapping
                result = lReturn
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, bPMUser.Business object Failed to Initialize.")
            End If

            'Encrypt the password
            sPassword = v_sPassword
            lReturn = CType(bPMFunc.Encrypt(sPassword, sEncryptedPassword), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AAB-16-Oct-2002 13:20 - Added Error Trapping
                result = lReturn
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, UpdateAgentLogonDetails Failed to Encrypt the old password")
            End If

            'Check Logon for the user and password passed in.
            lReturn = oUser.CheckLogon(sCheckUsername:=sUsername, sCheckPassword:=sEncryptedPassword, dtEffectiveFrom:=dtEffectiveDate, iLanguageID:=iLanguageID, lPartyCnt:=lAgentCnt, vUserId:=lUserID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AAB-16-Oct-2002 13:20 - Added Error Trapping
                result = lReturn
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, bPMUser.Business.CheckLogon method failed.")
            End If

            'Get the details of the user
            lReturn = oUser.GetDetails(lUserID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AAB-16-Oct-2002 13:20 - Added Error Trapping
                result = lReturn
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, bPMUser.Business.GetDetails method failed.")
            End If

            'Encrypt the NEW password
            sPassword = v_sNewPassword
            lReturn = CType(bPMFunc.Encrypt(sPassword, sEncryptedPassword), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AAB-16-Oct-2002 13:20 - Added Error Trapping
                result = lReturn
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, UpdateAgentLogonDetails Failed to Encrypt the new password")
            End If

            'We will set the row to 1 since we know there is only one row.
            lReturn = oUser.EditUpdate(lRow:=gPMConstants.PMEReturnCode.PMTrue, vPassword:=sEncryptedPassword, vPasswordChangeDate:=DateTime.Now)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AAB-16-Oct-2002 13:20 - Added Error Trapping
                result = lReturn
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, bPMUser.Business.EditUpdate method failed.")
            End If

            'Update the oUser object
            lReturn = oUser.Update()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AAB-16-Oct-2002 13:20 - Added Error Trapping
                result = lReturn
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, bPMUser.Business.Update method failed.")
            End If

            'Set the created object to nothing
            oUser.Dispose()
            oUser = Nothing

            Return result

        Catch excep As System.Exception



            'AAB-16-Oct-2002 14:18 - To log a clearer error message
            If Informations.Err().Number = gPMConstants.Constants.vbObjectError Then
                sErrorMessage = excep.Message
            Else
                result = gPMConstants.PMEReturnCode.PMError
                sErrorMessage = "UpdateAgentLogonDetails Failed"
            End If

            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAgentLogonDetails", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAgentLogonDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            'AAB-16-Oct-2002 13:23 - Destroy the object in case of an error
            If Not (oUser Is Nothing) Then
                oUser.Dispose()
                oUser = Nothing
            End If

            Return result
        End Try
    End Function

    Public Function GetQuotes(ByVal v_lPartyCnt As Integer, ByRef r_vResults As Object) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        '******************************************************************************
        '        Function Name:  GetQuotes
        '******************************************************************************
        '           Created By:  Ahmed "Jay" Bishatwi
        '           Created On:  20-Sep-2002
        '******************************************************************************
        '       Parameters Are:
        '                        (In)     - v_lPartyCnt - Long     -
        '                        (In/Out) - r_vResults  - Variant  -
        '
        ' Return Value Type Is:  Long -
        '******************************************************************************
        ' Function Description:  Returns the Quotes for a perticular party.
        '******************************************************************************
        Dim lReturn As gPMConstants.PMEReturnCode
        'AAB-16-Oct-2002 14:42 - To add clearer error message
        Dim sErrorMessage As String = ""
        Dim blIsUnderwriting As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oOptions As bSIROptions.Business

            ' Get an instance of bSIROptions


            oOptions = New bSIROptions.Business
            m_lReturn = oOptions.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message

                TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise bSIROptions.Business", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="IsUnderwriting", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' What we running?
            blIsUnderwriting = True


            oOptions.Dispose()

            oOptions = Nothing


            m_oFindInsurance = New bSIRFindInsurance.Form()
            'AAB-16-Oct-2002 13:26 - Added error trapping
            If m_oFindInsurance Is Nothing Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Internal, GetQuotes Failed To create bSIRFindInsurance.Form object")
            End If


            lReturn = m_oFindInsurance.Initialise(sUsername:=SIRIUS_USERNAME, sPassword:=SIRIUS_PASSWORD, iUserID:=SIRIUS_USERID, iSourceId:=SIRIUS_SOURCEID, iLanguageId:=SIRIUS_LANGUAGEID, iCurrencyId:=SIRIUS_CURRENCYID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)
            'AAB-16-Oct-2002 13:27 - Added error trapping
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Internal, bSIRFindInsurance.Form object failed to initialize")
            End If


            lReturn = m_oFindInsurance.SearchAll(r_vResultArray:=r_vResults, v_vInsuranceRef:="", v_vInsFileType:="%", v_vShortName:="%", v_vPartyCnt:=ToSafeInteger(v_lPartyCnt), v_vUserInsurerCnt:=CStr(0))

            'RFC Do not return PMNotFound as an error
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                result = lReturn
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, bSIRFindInsurance.Form.SearchAll failed")
            End If


            'Destroy the object.
            m_oFindInsurance.Dispose()
            m_oFindInsurance = Nothing

            Return result

        Catch excep As System.Exception



            'AAB-16-Oct-2002 13:45 - Check to see if we raised the error and set the error message
            If Informations.Err().Number = gPMConstants.Constants.vbObjectError Then
                sErrorMessage = excep.Message
            Else
                result = gPMConstants.PMEReturnCode.PMError
                sErrorMessage = "GetQuote Failed"
            End If

            'Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage, sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuotes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            'AAB-16-Oct-2002 13:46 - Destroy the object if needed
            If Not (m_oFindInsurance Is Nothing) Then
                m_oFindInsurance.Dispose()
                m_oFindInsurance = Nothing
            End If

            Return result
        End Try
    End Function
    Public Function GetQuoteRisks(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vQuoteArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"
        '******************************************************************************
        '        Function Name:  GetQuoteRisks
        '******************************************************************************
        '           Created By:  Ahmed "Jay" Bishatwi
        '           Created On:  20-Sep-2002
        '******************************************************************************
        '       Parameters Are:
        '                        (In)     - v_lInsuranceFileCnt - Long     -
        '                        (In/Out) - r_vQuoteArray       - Variant  -
        '
        ' Return Value Type Is:  Long -
        '******************************************************************************
        ' Function Description:  This function Get the Risk for a specific Insurance
        '                        File Cnt to be used in Agents OnLine web site.
        '******************************************************************************
        Dim lReturn As gPMConstants.PMEReturnCode
        'todo list (Iteration 2)
        'Dim oFindRisks As bSIRFindRisk.Form
        Dim oFindRisks As bSIRFindRisk.Form = Nothing
        Dim sErrorMessage As String = ""
        Try

            'AAB-16-Oct-2002 14:42 - To add clearer error message

            result = gPMConstants.PMEReturnCode.PMTrue
            'todo list (Iteration 3)
            oFindRisks = New Object()
            'AAB-16-Oct-2002 12:18 - Added Error Trapping.

            oFindRisks = New bSIRFindRisk.Form
            lReturn = oFindRisks.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=CStr(ACApp), vDatabase:=m_oDatabase)
            If oFindRisks Is Nothing Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, GetQuotes Failed To create bSIRFindRisk.Form object")
            End If


            lReturn = oFindRisks.Initialise(sUsername:=ToSafeString(SIRIUS_USERNAME), sPassword:=ToSafeString(SIRIUS_PASSWORD), iUserID:=ToSafeInteger(SIRIUS_USERID), iSourceID:=ToSafeInteger(SIRIUS_SOURCEID), iLanguageID:=ToSafeInteger(SIRIUS_LANGUAGEID), iCurrencyID:=ToSafeInteger(SIRIUS_CURRENCYID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            'AAB-16-Oct-2002 12:20 - Added Error Trapping
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, bSIRFindRisk.Form object Failed to initialize")
            End If

            lReturn = oFindRisks.SearchAll(r_vResultArray:=r_vQuoteArray, v_vInsuranceFileCnt:=v_lInsuranceFileCnt)
            'AAB-16-Oct-2002 12:21 - Added Error Trapping
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                Throw New System.Exception(gPMConstants.Constants.vbObjectError.ToString() + ", Internal, bSIRFindRisk.Form.SearchAll Method Failed")
            End If

            'Destroy The object
            oFindRisks.Dispose()
            oFindRisks = Nothing

            Return result

        Catch excep As System.Exception



            'AAB-16-Oct-2002 13:53 - Check to see if we raised the error and set the error message
            If Informations.Err().Number = gPMConstants.Constants.vbObjectError Then
                sErrorMessage = excep.Message
            Else
                result = gPMConstants.PMEReturnCode.PMError
                sErrorMessage = "GetQuote Failed"
            End If


            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage, sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteRisks", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            'AAB-16-Oct-2002 12:23 - Destroy the object in case of an error
            If Not (oFindRisks Is Nothing) Then
                oFindRisks.Dispose()
                oFindRisks = Nothing
            End If

            Return result
        End Try
    End Function

    Public Function AddRisk(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRiskTypeId As Integer, ByVal v_lRiskScreenId As Integer, ByVal v_sRiskDescription As String, ByVal v_lProductID As Integer, ByRef r_lRiskFolderCnt As Integer, ByRef r_lRiskCnt As Integer, Optional ByVal v_lOldRiskFolderCnt As Integer = 0, Optional ByRef r_oDataset As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            Dim lReturn As Integer
            Dim oSIRRiskScreen As Object 'bSIRRiskScreen.Business
            Dim vRiskDetails As Object = Nothing
            Dim vRiskTypeDetails As Object = Nothing
            Dim sGISDataModel As String = ""
            Dim lGISDataModelId As Integer
            Dim vPolicyLinkId As Object
            Dim sParentOIKey As String = ""
            Dim lRiskId As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            ' ***************************************************************** '
            '             BEGIN: bSIRRiskScreen.Business Object Code
            ' ***************************************************************** '
            ' Create Business Object


            'oSIRRiskScreen = New bSIRRiskScreen.Business
            oSIRRiskScreen = Nothing
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=oSIRRiskScreen, v_sClassName:="bSIRRiskScreen.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim r_sMessage As String = "Failed to create an instance of bSIRRiskScreen.Business"
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bSIRRiskScreen.Business", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

            lReturn = oSIRRiskScreen.Initialise(sUsername:=m_sUserName.ToString(), sPassword:=m_sPassword.ToString(), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=CStr(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRisk method failed to create bSIRRiskScreen.Business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Set the process modes for the busines object.

            lReturn = oSIRRiskScreen.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vNavigate:=1, vProcessMode:=1, vTransactionType:="AOL", vEffectiveDate:=DateTime.Now)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRRiskScreen.Business.SetProcessModes method Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Set the Business object keys
            'oSIRRiskScreen.PartyCnt = m_lPartyCnt

            oSIRRiskScreen.InsuranceFolderCnt = v_lInsuranceFolderCnt

            oSIRRiskScreen.InsuranceFileCnt = v_lInsuranceFileCnt

            oSIRRiskScreen.RiskId = r_lRiskCnt

            oSIRRiskScreen.RiskTypeId = v_lRiskTypeId
            '**************

            oSIRRiskScreen.ProductId = v_lProductID
            '**************


            oSIRRiskScreen.RiskFolderCnt = r_lRiskFolderCnt

            ' CTAF 20040227 Begin - This next section doesnt seem to be needed?
            If v_lRiskScreenId > 0 Then

                ' Set the risk screen id

                oSIRRiskScreen.ScreenId = v_lRiskScreenId

                ' Get DataModel code

                lReturn = oSIRRiskScreen.GetGISDataModel(r_lGISDataModelID:=ToSafeInteger(lGISDataModelId), r_sGISDataModel:=sGISDataModel.ToString)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRRiskScreen.Business.GetGISDataModel method Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End If
            ' CTAF 20040227 End


            lReturn = oSIRRiskScreen.GetRisk(vRiskArray:=vRiskDetails, vRiskTypeArray:=vRiskTypeDetails)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRRiskScreen.Business.GetRisk method Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If



            vPolicyLinkId = DBNull.Value

            If Informations.IsArray(vRiskDetails) Then

                r_lRiskCnt = ToSafeInteger(vRiskDetails(0, 0))

                r_lRiskFolderCnt = ToSafeInteger(vRiskDetails(2, 0))
            Else
                lRiskId = r_lRiskCnt
            End If

            With m_oDatabase
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="risk_cnt", vValue:=CStr(r_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="description", vValue:=v_sRiskDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .SQLAction(sSQL:=ACUpdateRiskDescriptionSQL, sSQLName:=ACUpdateRiskDescriptionName, bStoredProcedure:=ACUpdateRiskDescriptionStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Not a reason to halt the process but log the error

                    TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the description for Risk - " & r_lRiskCnt & "'", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="AddRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                End If

            End With


            ' ***************************************************************** '
            '                END: bGIS.Application Object Code
            ' ***************************************************************** '

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add the new Risk record", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class

