Option Strict Off
Option Explicit On
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Security_NET.Security")> _
Public NotInheritable Class Security
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Security
    '
    ' Date: CL020600
    '
    ' Description: Back Office Mapper for the GeminiNet (motor) solution.
    '
    ' Edit History:
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Security"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Data Set Definition
    Private m_oDataSet As cGISDataSetControl.Application

    Private m_sGISDataModel As String = ""

    Private m_lReturn As gPMConstants.PMEReturnCode

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

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    ' PUBLIC Property Procedures (End)

    Public Function LogoffAgent(ByVal v_sDataModelCode As Object, ByVal v_sBusinessTypeCode As Object, ByVal v_sUsername As Object, ByRef r_vAdditionalDataArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oSBOLink As bSIRIUSLink.SIRIUSLink
            Dim lReturn As Integer

            ' Create bSiriusLink object

            oSBOLink = New bSIRIUSLink.SIRIUSLink
            lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LogoffAgent Failed - Failed to create bSiriusLink object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="LogoffAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If



            lReturn = oSBOLink.LogoffAgent(v_sUsername)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LogoffAgent Failed - bSiriusLink.LogoffAgent method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="LogoffAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            oSBOLink.Dispose()
            oSBOLink = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LogoffAgent Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="LogoffAgent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function




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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lReturn As Integer

            ' Initialisation Code.

            ' Set Username and Password
            m_sUsername = sUsername
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
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed - Failed to create connection to Database", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:="Failed to create connection to Database")
                Return result
            End If

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
                m_oDataSet = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: RegisterUser
    '
    ' Description: Register a new User
    '
    ' Date: RAG070600
    '
    ' RFC050900 - Added Title, MaritalStatusCode & Address/postcode  parameters, needed for its4me.
    ' RFC050900 - Added BusinessTypeCode Parameter
    ' RFC050900 - Added AdditonalDataArray. This is a array of name/value
    '             pairs, that can be used in the future to pass to OR return
    '             extra data from the Back Office Mapper, without need to change
    '             the method interface.
    ' ***************************************************************** '
    Public Function RegisterUser(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_sForename As String, ByVal v_sSurname As String, ByVal v_sMothersMaidenName As String, ByVal v_sDateOfBirth As String, ByVal v_sEmailAddress As String, ByVal v_sMemorableDate As String, ByVal v_sAQuestion As String, ByVal v_sTheAnswer As String, ByVal v_sCurrentRenewalDate As String, ByRef r_sUserID As String, ByRef r_sPassword As String, ByRef r_lPartyCnt As Integer, ByVal v_sTitle As String, ByVal v_sMaritalStatusCode As String, ByVal v_sAddress1 As String, ByVal v_sAddress2 As String, ByVal v_sAddress3 As String, ByVal v_sAddress4 As String, ByVal v_sPostcode As String, ByRef r_vAdditionalDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim sIsEmailRequired As String = ""
        Dim oSBOLink As bSIRIUSLink.SIRIUSLink
        Dim dtDateOfBirth, dtCurrentRenewal As Date
        Dim sFrom, sSubject, sTPIntroducer, sBrandCode As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Extract brand code from additional array - CL161100
            If Informations.IsArray(r_vAdditionalDataArray) Then

                sBrandCode = CStr(r_vAdditionalDataArray(ACAdditionalArrayRowValue, ACAdditionalArrayColumnBrand))
            Else
                sBrandCode = "???"
            End If

            ' RAG070600
            ' Here, we need to:
            ' Generate a User ID (stored in the SBO against Short_name),
            ' Generate a Xelector User ID using the Xelector XIDGenerator
            ' and generate a randon password,
            ' then store the details in the Sirius Back Office
            ' and return the UserID, password and party_cnt(from SBO)

            ' Convert date-of-birth string to date
            If Not Informations.IsDate(v_sDateOfBirth) Then
                'RegisterUser = PMFalse
                'Exit Function
                dtDateOfBirth = CDate("01/01/00")
            Else
                dtDateOfBirth = CDate(v_sDateOfBirth)
            End If


            ' Convert renewal-date string to date
            If Not Informations.IsDate(v_sCurrentRenewalDate) Then
                'RegisterUser = PMFalse
                'Exit Function
                dtCurrentRenewal = CDate("01/01/00")
            Else
                dtCurrentRenewal = CDate(v_sCurrentRenewalDate)
            End If

            ' Generate a random password
            lReturn = GeneratePassword(r_sPassword:=r_sPassword)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RegisterUser Failed - GeneratePassword method failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RegisterUser", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'LogMessageToFile m_sUsername, PMLogOnError, "Before SBO Link", "bGISBOM" & ACDataModelCode, "Security", "RegisterUser"

            ' Create a SBO Link object
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
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RegisterUser Failed - Failed to create bSiriusLink object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RegisterUser", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'LogMessageToFile m_sUsername, PMLogOnError, "oSBOLink.Initialise OK", "bGISBOM" & ACDataModelCode, "Security", "RegisterUser"

            ' RAG221100 - For any BusinessTypeCode other than XEL, Introducer code is the same
            ' e.g. for first-e BusinessTypeCode = "101"

            Select Case v_sGisBusinessTypeCode
                Case "XEL"
                    sTPIntroducer = "201"
                    'Case "XXX"
                    '    sTPIntroducer = "000"
                Case Else
                    sTPIntroducer = v_sGisBusinessTypeCode
            End Select


            ' Add a party into the SBO

            ' RAG 15-08-00 Add Channel ID as v_sTPIntroducer (=201 for xelector)
            ' RAG 31-05-01 Add Title to this call.

            lReturn = oSBOLink.AddParty(v_sSurname:=v_sSurname, v_sForename:=v_sForename, v_sPartyType:="PC", v_sAddress1:=v_sAddress1, v_sAddress2:=v_sAddress2, v_sAddress3:=v_sAddress3, v_sAddress4:=v_sAddress4, v_sPostCode:=v_sPostcode, v_dDOB:=dtDateOfBirth, v_sEMail:=v_sEmailAddress, v_sUserID:="", v_sPassword:=r_sPassword, r_lPartyCnt:=r_lPartyCnt, r_sShortName:=r_sUserID, v_sMothersMaidenName:=v_sMothersMaidenName, v_dCurrInsRenewalDate:=dtCurrentRenewal, v_dMemorableDate:=v_sMemorableDate, v_sAQuestion:=v_sAQuestion, v_sTheAnswer:=v_sTheAnswer, v_sTPIntroducer:=sTPIntroducer, v_sTitle:=v_sTitle)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RegisterUser Failed - bSiriusLink.AddParty method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RegisterUser", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'LogMessageToFile m_sUsername, PMLogOnError, "oSBOLink.AddParty OK", "bGISBOM" & ACDataModelCode, "Security", "RegisterUser"

            ' Destroy the link object

            oSBOLink.Dispose()
            oSBOLink = Nothing

            'LogMessageToFile m_sUsername, PMLogOnError, "oSBOLink Destroyed", "bGISBOM" & ACDataModelCode, "Security", "RegisterUser"

            ' Check registry to see if this email is needed
            lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegRegistrationEmailRequired, r_sSettingValue:=sIsEmailRequired, v_sBusinessTypeCode:=v_sGisBusinessTypeCode, v_sSubKey:=GISSharedConstants.GISRegSubKeyEmails)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RegisterUser Failed - GetRegSettingFromDataBusModel method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RegisterUser", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'LogMessageToFile m_sUsername, PMLogOnError, "Before Email", "bGISBOM" & ACDataModelCode, "Security", "RegisterUser"

            If sIsEmailRequired = "1" Then

                ' Check registry to get Subject
                lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegRegistrationEmailSubject, r_sSettingValue:=sSubject, v_sBusinessTypeCode:=v_sGisBusinessTypeCode, v_sSubKey:=GISSharedConstants.GISRegSubKeyEmails)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RegisterUser Failed - GetRegSettingFromDataBusModel method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RegisterUser", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Check registry to get From
                lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegRegistrationEmailFrom, r_sSettingValue:=sFrom, v_sBusinessTypeCode:=v_sGisBusinessTypeCode, v_sSubKey:=GISSharedConstants.GISRegSubKeyEmails)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RegisterUser Failed - GetRegSettingFromDataBusModel method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RegisterUser", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Send an email to the customer with User Registration details
                lReturn = GenerateRegistrationEmail(v_sEmailAddress:=v_sEmailAddress, v_sUserID:=r_sUserID, v_sPassword:=r_sPassword, v_sFrom:=sFrom, v_sSubject:=sSubject, v_sBrandCode:=sBrandCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RegisterUser Failed - GenerateRegistrationEmail method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RegisterUser", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            Else

                ' RAG 26/07/2000
                ' If no email sent, write to Sirius.Log.
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "No Email " & r_sUserID & "/" & r_sPassword, ACApp, ACClass, "RegisterUser")

            End If

            Return result

        Catch excep As System.Exception


            ' Error
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RegisterUser Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RegisterUser", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoginUser
    '
    ' Description: Log in an existing user
    '
    ' Date: RAG070600
    '
    ' RFC200700 - Return the PMUserID if the login is a TPA
    ' RFC050900 - Added DataModel & BusinessTypeCode Parameters
    ' RFC050900 - Added AdditonalDataArray. This is a array of name/value
    '             pairs, that can be used in the future to pass to OR return
    '             extra data from the Back Office Mapper, without need to change
    '             the method interface.
    ' ***************************************************************** '
    Public Function LoginUser(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_sUserID As String, ByVal v_sPassword As String, ByRef r_lPartyCnt As Integer, ByRef r_lPMUserID As Integer, ByRef r_sPartySurname As String, ByRef r_sPartyForename As String, ByRef r_dtDateOfBirth As Date, ByRef r_sEMail As String, ByRef r_vAdditionalDataArray As Object) As Integer

        Dim result As Integer = 0
        Try

            Dim oSBOLink As bSIRIUSLink.SIRIUSLink
            Dim lReturn As gPMConstants.PMEReturnCode
            Dim sEncryptedPassword As String = ""
            Dim oPMUser As Bpmuser.Business
            Dim sMothersMaidenName As String = ""
            Dim dtCurrInsRenewal As Date
            Dim sPassword As String = ""

            result = gPMConstants.PMEReturnCode.PMFalse

            ' RAG070600
            ' Here, we need to validate the UserID and password against the SBO party record
            ' then return the party_cnt, if a matching record is found.

            'RJG 13/06/2000 Create a SBO Link object
            'Set oSBOLink = New bSIRIUSLink.SIRIUSLink
            '    Set oSBOLink = CreateObject("bSIRIUSLink.SIRIUSLink")
            ' Create bSiriusLink object

            oSBOLink = New bSIRIUSLink.SIRIUSLink
            lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoginUser Failed - Failed to create bSiriusLink object", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="LoginUser", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'RJG 13/06/2000 - Encrypt the input password to compare to the one stored
            lReturn = CType(bPMFunc.Encrypt(sPassword:=v_sPassword, sEncryptedPassword:=sEncryptedPassword), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoginUser Failed - Encrypt method for Password failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="LoginUser", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            lReturn = oSBOLink.Login(v_sUserID:=v_sUserID, r_lPartyCnt:=r_lPartyCnt, r_sPartySurname:=r_sPartySurname, r_sPartyForename:=r_sPartyForename, r_sMothersMaidenName:=sMothersMaidenName, r_dtDateOfBirth:=r_dtDateOfBirth, r_sEMail:=r_sEMail, r_dtCurrInsRenewal:=dtCurrInsRenewal, r_sPassword:=sPassword)

            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'RJG 13/06/2000 - The UserID was found Check password
                'lReturn = oSBOLink.findparty(v_lPartyCnt:=tosafeinteger(r_lPartyCnt), r_vResults:=vPartyDetails)

                'If IsArray(vPartyDetails) Then
                'If Trim$(vPartyDetails(0, 10)) = Trim$(sEncryptedPassword) Then
                If sPassword = sEncryptedPassword.Trim() Then
                    result = gPMConstants.PMEReturnCode.PMTrue
                End If
                'End If
            Else
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoginUser Failed - bSiriulink.Login method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="LoginUser", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Set vPartyDetails = Nothing
            oSBOLink = Nothing

            '    ' PMUser stuff commented out temporarily, and replaced with this...
            '    r_lPMUserID = -1
            '    Exit Function
            '
            '    ' **********************************************************************************



            ' RFC200700 - Return the PMUserID if the login is a TPA
            '    Set oPMUser = CreateObject("bPMUser.Business")
            '
            '    lReturn = oPMUser.Initialise( _
            ''        m_sUsername, _
            ''        m_sPassword, _
            ''        m_iUserID, _
            ''        m_iSourceID, _
            ''        m_iLanguageID, _
            ''        m_iCurrencyID, _
            ''        m_iLogLevel, _
            ''        ACApp)
            '    If (lReturn <> PMTrue) Then
            '        LoginUser = lReturn
            '        Exit Function
            '    End If


            oPMUser = New Bpmuser.Business
            lReturn = oPMUser.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoginUser Failed - Failed to create bPMUser object", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="LoginUser", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Get the PMUserID for the Party, if there is one.

            lReturn = oPMUser.GetUserIDForParty(v_lPartyCnt:=r_lPartyCnt, r_lUserId:=r_lPMUserID)


            Select Case lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Nothing to do

                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' PMUserID not found for Party, default to -1
                    r_lPMUserID = -1

                Case Else
                    ' Error
                    r_lPMUserID = -1
                    Return lReturn

            End Select


            oPMUser.Dispose()
            oPMUser = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoginUser Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="LoginUser", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GenerateRegistrationEmail
    '
    ' Description: Build and send a registration email
    '
    ' Date: RG080600
    '
    ' ***************************************************************** '
    Private Function GenerateRegistrationEmail(ByVal v_sEmailAddress As String, ByVal v_sUserID As String, ByVal v_sPassword As String, ByVal v_sFrom As String, ByVal v_sSubject As String, ByVal v_sBrandCode As String) As Integer
        Dim result As Integer = 0
        Dim sMsg As String = ""
        Dim oCDONTS As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Create the email body
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        sMsg = ""

        sMsg = "Thank you for registering." & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) ''' DEFAULT


        ' AUTOBYTEL - opening text
        If v_sBrandCode = GISSharedConstants.GISXelBrandCodeAutoBytel Then
            sMsg = "Thank you for registering with Autobytel motor insurance." & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        End If

        ' FIRST E - opening text
        If v_sBrandCode = GISSharedConstants.GISXelBrandCodeFirste Then
            sMsg = "Thank you for registering with our motor insurance service. This service was brought to you in conjunction with first-e, the internet bank." & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        End If

        ' FT - opening text
        If v_sBrandCode = GISSharedConstants.GISXelBrandCodeFTYM Then
            sMsg = "Thank you for registering with our motor insurance service. This service was brought to you by " & GISSharedConstants.GISXelBrandNameFTYM & "." & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        End If


        ' Moneynet - opening text
        If v_sBrandCode = GISSharedConstants.GISXelBrandCodeMoneyNet Then
            sMsg = "Thank you for registering with our motor insurance service. This service was brought to you by " & GISSharedConstants.GISXelBrandNameMoneyNet & "." & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        End If


        ' etrade - opening text
        If v_sBrandCode = GISSharedConstants.GISXelBrandCodeETrade Then
            sMsg = "Thank you for registering with our motor insurance service. This service was brought to you by " & GISSharedConstants.GISXelBrandNameETrade & "." & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        End If


        sMsg = sMsg & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "In case you forget your password we suggest that you print out this e-mail or file it carefully." & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "The user Id and password shown here will allow you to save your personal details and a number "
        sMsg = sMsg & "of quotations. When you wish to buy this will also give you access to your Policy details "
        sMsg = sMsg & "and instructions on how to change your policy." & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Your User ID and Password are as follows:" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "User ID: " & v_sUserID & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Password: " & v_sPassword & Strings.ChrW(13) & Strings.ChrW(10)


        ' add hard text footer - CL141100

        sMsg = sMsg & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "How to get a quote - The Process" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "To get a quote you need to go through a number of steps" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "1.  Enter your details - we will ask you a number of questions which will enable us to give you a quotation. We will ask questions about" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "  - Who drives the car" & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "  - Driver details" & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "  - Car details " & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "  - Cover details" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "2. Receive a quotation - see which insurers are offering you a quotation and tailor your quote to best suit your needs" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "3. Complete personal details - we will ask you some personal questions. This will be in a secure session, so don't worry your details are safe." & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "4. Enter your payment details on-line - again don't worry your details are safe we use 128 bit SSL encryption to protect your details" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "5. Congratulations - you have bought your motor insurance on line. Your Certificate of insurance will be sent to you in the post. Thank You." & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "If you need further help you can contact us on help@insurance-enquiries.com, or ring on 0845 603 8080 " & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "This e-mail message is CONFIDENTIAL and may contain legally privileged Informations.  If you are not the intended recipient you should not read, copy, distribute, disclose or otherwise use the information in this e-mail." & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & "Please also telephone (0208 308 6469) or fax (0208 308 6444) us immediately and delete the message from your system. E-mail may be susceptible to data corruption, interception and unauthorised amendment, and we do not accept"
        sMsg = sMsg & " liability for any such corruption, interception or amendment or the consequences thereof or your reliance on any information contained therein if you are not the intended recipient. " & Strings.ChrW(13) & Strings.ChrW(10)
        sMsg = sMsg & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10)



        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Now send the email
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Developer guide no. 205

        oCDONTS = New Object

        ' If we are running on PWS then we cannot sent the Mail
        ' as CDONTS and the SMTP service is NOT available.

        If oCDONTS Is Nothing Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Set up the sender, recipient, subject & text message
        With oCDONTS

            .To = v_sEmailAddress

            .From = v_sFrom

            .Subject = v_sSubject

            .Body = sMsg
        End With

        'Send the message


        oCDONTS.send()

        oCDONTS = Nothing

        Return result

    End Function
    Public Function LoginAgent(ByVal v_sDataModelCode As Object, ByVal v_sBusinessTypeCode As Object, ByVal v_sUsername As Object, ByVal v_sPassword As Object, ByRef r_lAgentCnt As Object, ByRef r_lPMUserID As Object, ByRef r_bUnrestrictedSearch As Object, ByRef r_dtPasswordChangeDate As Object, ByRef r_dtLastlogin As Object, ByRef r_sForename As Object, ByRef r_sSurname As Object, ByRef r_sEmailAddress As Object, ByRef r_iLanguageId As Object, ByRef r_vSourceList As Object, ByRef r_vAdditionalDataArray As Object) As Integer

        Dim result As Integer = 0
        Dim oSBOLink As bSIRIUSLink.SIRIUSLink
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create bSiriusLink object

            oSBOLink = New bSIRIUSLink.SIRIUSLink
            lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoginAgent Failed - Failed to create bSiriusLink object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            lReturn = oSBOLink.LoginAgent(v_sUsername:=v_sUsername, v_sPassword:=v_sPassword, r_lAgentCnt:=r_lAgentCnt, r_lPMUserID:=r_lPMUserID, r_bUnrestrictedSearch:=r_bUnrestrictedSearch, r_dtPasswordChangeDate:=r_dtPasswordChangeDate, r_dtLastlogin:=r_dtLastlogin, r_sForename:=r_sForename, r_sSurname:=r_sSurname, r_sEmailAddress:=r_sEmailAddress, r_iLanguageId:=r_iLanguageId, r_vSourceList:=r_vSourceList, r_vAdditionalDataArray:=r_vAdditionalDataArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoginAgent Failed - bSiriusLink.LoginAgent method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            oSBOLink.Dispose()
            oSBOLink = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoginAgent Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    Public Function UpdateAgentLogonDetails(ByVal v_sDataModelCode As Object, ByVal v_sBusinessTypeCode As Object, ByVal v_sUsername As Object, ByVal v_sPassword As Object, ByVal v_sNewPassword As Object, ByRef r_vAdditionalDataArray As Object) As Integer

        Dim result As Integer = 0
        Dim oSBOLink As bSIRIUSLink.SIRIUSLink
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            oSBOLink = New bSIRIUSLink.SIRIUSLink
            lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAgentLogonDetails Failed - Failed to create bSiriusLink object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateAgentLogonDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            lReturn = oSBOLink.UpdateAgentLogonDetails(v_sUsername:=v_sUsername, v_sPassword:=v_sPassword, v_sNewPassword:=v_sNewPassword)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAgentLogonDetails Failed - bSiriusLink.UpdateAgentLogonDetails method failed.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateAgentLogonDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If


            oSBOLink.Dispose()
            oSBOLink = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAgentLogonDetails Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateAgentLogonDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateUserPassword
    '
    ' Description: Updates the password for a user and gets their email address
    '
    ' History: 22/07/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateUserPassword(ByVal v_sUsername As String, ByVal v_sPassword As String, ByRef r_sEmailAddress As String) As Integer

        Dim result As Integer = 0
        Dim oUser As Bpmuser.Business
        Dim sUsername As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue


        oUser = New Bpmuser.Business
        m_lReturn = oUser.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get instance of bPMUser.Business", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateUserPassword", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            ' Remove it
            oUser = Nothing

        End If

        ' Load the details

        m_lReturn = oUser.GetDetails()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetDetails for all users", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateUserPassword", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Iterrate all the users until we find ours
        While (sUsername.ToLower() <> v_sUsername.ToLower())

            ' Read the next record from the collection

            m_lReturn = oUser.GetNext(vUsername:=sUsername, vEmailAddress:=r_sEmailAddress)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                ' Terminate

                oUser.Dispose()

                ' Remove it
                oUser = Nothing

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetNext user record for : " & v_sUsername, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateUserPassword", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

        End While

        ' Update it if it has an email address associated with it
        If r_sEmailAddress <> "" Then

            ' Update the password for the user now too


            m_lReturn = oUser.EditUpdate(lRow:=oUser.CurrentRecord, vPassword:=v_sPassword)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Terminate

                oUser.Dispose()

                ' Remove it
                oUser = Nothing

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to EditUpdate user record for : " & v_sUsername, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateUserPassword", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Finally, update it

            m_lReturn = oUser.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Terminate

                oUser.Dispose()

                ' Remove it
                oUser = Nothing

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update user record for : " & v_sUsername, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="UpdateUserPassword", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

        End If

        ' Terminate

        oUser.Dispose()

        ' Remove it
        oUser = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: SendEmailCDO
    '
    ' Description: Sends an email via CDO using Exchange Server
    '
    ' History: 22/07/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function SendEmailCDO(ByVal v_sProfileName As String, ByVal v_sSubject As String, ByVal v_sEmailAddress As String, ByVal v_sMessage As String, Optional ByVal v_vProfilePassword As Object = Nothing) As Integer
        Dim result As Integer = 0

        Dim oSession, oMessage, oUser As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Use the new CDO object
            'Developer guide no. 205,as per vb code

            oSession = New Object

            ' Log On

            If Informations.IsNothing(v_vProfilePassword) Then

                oSession.Logon(ProfileName:=ToSafeString(v_sProfileName), ShowDialog:=False)
            Else
                oSession.Logon(ProfileName:=ToSafeString(v_sProfileName), ProfilePassword:=v_vProfilePassword, ShowDialog:=False)
            End If

            ' Create a new message

            oMessage = oSession.Outbox.Messages.Add

            ' Set it up

            oMessage.Subject = v_sSubject

            oMessage.Text = v_sMessage

            ' Add a recipient

            oUser = oMessage.Recipients.Add

            ' Set that up
            oUser.Name = v_sEmailAddress

            oUser.Type = 1 ' cdoTo

            oUser.Resolve()

            ' Send it

            oMessage.Update()

            oMessage.send(ShowDialog:=False)

            ' Log Off

            oSession.Logoff()
            oSession = Nothing
            oMessage = Nothing
            oUser = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            Select Case Informations.Err().Number
                Case -2147221231
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to logon to Exchange Server (" & v_sProfileName & "). Check profile name and password in STS web.config", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="SendEmailCDO", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Case Else
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SendEmailCDO Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="SendEmailCDO", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            End Select

            Return result


            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SendEmailCDONTS
    '
    ' Description: Sends an email via CDONTS
    '
    ' History: 22/07/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function SendEmailCDONTS(ByVal v_sSubject As String, ByVal v_sMessage As String, ByVal v_sEmailAddress As String, ByVal v_sFromEmailAddress As String) As Integer
        Dim result As Integer = 0

        Dim oCDONTS As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of CDONTS
            'Developer guide no. 205

            oCDONTS = New Object

            ' Configure and send
            With oCDONTS

                .To = v_sEmailAddress

                .From = v_sFromEmailAddress

                .Subject = v_sSubject

                .Body = v_sMessage

                .send()
            End With

            ' Remove the instance
            oCDONTS = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            Select Case Informations.Err().Number
                Case 429
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to create instance of CDONTS.NewMail. Try specifying a ProfileName to use CDO 1.2x instead.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="SendEmailCDONTS", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Case Else
                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SendEmailCDONTS Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="SendEmailCDONTS", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            End Select


            Return result


            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: EmailUserNewPassword
    '
    ' Description: Sends an email to the user informing them of their new password
    '
    ' History: 22/07/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (EmailUserNewPassword) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function EmailUserNewPassword(ByVal v_sUsername As String, ByVal v_sIPAddress As String, ByVal v_sPassword As String, ByVal v_sMessage As String, ByVal v_sSubject As String, ByVal v_sEmailAddress As String, Optional ByVal v_sFromEmail As String = "", Optional ByVal v_sFromName As String = "", Optional ByVal v_vProfileName As String = "", Optional ByVal v_vProfilePassword As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Dim sMessage As String = ""
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Construct the message
    '
    ' Parse any tokens inside the message
    'sMessage = v_sMessage
    'sMessage = sMessage.Replace("{username}", v_sUsername)
    'sMessage = sMessage.Replace("{password}", v_sPassword)
    'sMessage = sMessage.Replace("{ip_address}", v_sIPAddress)
    'sMessage = sMessage.Replace("\n", Environment.NewLine)
    'sMessage = sMessage.Replace("\t", Strings.ChrW(9))
    'sMessage = sMessage.Replace("{newline}", Environment.NewLine)
    'sMessage = sMessage.Replace("{tab}", Strings.ChrW(9))
    '
    '(
    'If Informations.IsNothing(v_vProfileName) Then
    'v_vProfileName = ""
    'End If
    '
    ' Choose which Method to use
    'If v_vProfileName = "" Then
    '
    ' Attempt to use CDONTS
    'm_lReturn = CType(SendEmailCDONTS(v_sSubject:=v_sSubject, v_sMessage:=sMessage, v_sEmailAddress:=v_sEmailAddress, v_sFromEmailAddress:=v_sFromEmail), gPMConstants.PMEReturnCode)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error Message
    'bPMFunc.LogMessage(sUsername:=tosafestring(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to send email via CDONTS", vApp:=tosafestring(ACApp), vClass:=ACClass, vMethod:="EmailUserNewPassword", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    'Return result
    'End If
    '
    'Else
    '
    ' Use CDO 1.2x
    'm_lReturn = CType(SendEmailCDO(v_sProfileName:=v_vProfileName, v_sSubject:=v_sSubject, v_sEmailAddress:=v_sEmailAddress, v_sMessage:=sMessage, v_vProfilePassword:=v_vProfilePassword), gPMConstants.PMEReturnCode)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error Message
    'bPMFunc.LogMessage(sUsername:=tosafestring(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to send email via CDO 1.2x", vApp:=tosafestring(ACApp), vClass:=ACClass, vMethod:="EmailUserNewPassword", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    'Return result
    'End If
    '
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(sUsername:=tosafestring(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EmailUserNewPassword Failed", vApp:=tosafestring(ACApp), vClass:=ACClass, vMethod:="EmailUserNewPassword", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    '(

    '
    'Return result
    'End Try
    'End Function


    ' ***************************************************************** '
    '
    ' Name: ForgottenPassword
    '
    ' Description: Creates a new password and emails it to the user
    '
    ' History: 22/07/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function ForgottenPassword(ByVal v_sUsername As String, ByVal v_sIPAddress As String, ByVal v_sSubject As String, ByVal v_sMessage As String, ByRef r_sEmailAddress As String, ByRef r_sNewPassword As String, Optional ByVal v_sFromEmail As String = "", Optional ByVal v_sFromName As String = "", Optional ByVal v_vProfileName As Object = Nothing, Optional ByVal v_vProfilePassword As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sEncryptedPassword As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Generate Password
            m_lReturn = CType(GeneratePassword(r_sPassword:=r_sNewPassword), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then ' Failed to generate password
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Encrypt
            m_lReturn = CType(bPMFunc.Encrypt(sPassword:=r_sNewPassword, sEncryptedPassword:=sEncryptedPassword), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then ' Failed to encrypt password
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Save Encrypted in DB and get email address
            m_lReturn = CType(UpdateUserPassword(v_sUsername:=v_sUsername, v_sPassword:=sEncryptedPassword, r_sEmailAddress:=r_sEmailAddress), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMEOF Then
                    Return gPMConstants.PMEReturnCode.PMUserNotExist
                Else
                    Return gPMConstants.PMEReturnCode.PMUpdateUserFailed ' Failed to encrypt password
                End If
            End If

            If r_sEmailAddress = "" Then

                result = gPMConstants.PMEReturnCode.PMNoEmailAddress ' Return that we found no email address

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ForgottenPassword Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ForgottenPassword", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function


    Public Sub New()
        MyBase.New()

        m_oDataSet = Nothing
        If m_bCloseDatabase Then
            m_oDatabase.CloseDatabase()
        End If
        m_oDatabase = Nothing
    End Sub
End Class
