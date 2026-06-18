Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Security_NET.Security")>
Public NotInheritable Class Security
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Security
    '
    ' Date: CL040600
    '
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 19/09/2003
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

    Public vRecipient As Object
    Public vSubject As Object
    Public vTextBody As Object
    Public vMailbox As Object
    Public vServer As Object

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Security"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Data Set Definition
    Private m_oDataSet As cGISDataSetControl.Application

    ' bGISSchemeBusiness component - CL150200
    Private m_oGISSchemeBusiness As bGISSchemeBusiness.Business

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' SQL String used for Adding/Updating Objects in DB
    Private m_sAddSQL As String = ""
    Private m_sUpdateSQL As String = ""
    Private m_sDeleteSQL As String = ""
    Private m_sAddUpdateSQL As String = "" 'sj 18/08/99

    ' File Number used for Creating Referrals
    Private m_iFileNumber As Integer

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
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

        'LogMessageToFile _
        'sUsername:="", _
        'iType:=PMLogOnError, _
        'sMsg:="bGIS.initialise starting...", _
        'vApp:=ACApp, _
        'vClass:=ACClass, _
        'vMethod:="bGIS"  ' TEMPDEBUG

        Dim result As Integer = 0
        Try

            Dim lReturn As gPMConstants.PMEReturnCode

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUserName
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

            lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

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
                m_oGISSchemeBusiness = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '


    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' RAG070600
    ' Method to register a new user
    Public Function RegisterUser(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_sForename As String, ByVal v_sSurname As String, ByVal v_sMothersMaidenName As String, ByVal v_sDateOfBirth As String, ByVal v_sEmailAddress As String, ByVal v_sMemorableDate As String, ByVal v_sAQuestion As String, ByVal v_sTheAnswer As String, ByVal v_sCurrentRenewalDate As String, ByRef r_sUserID As Object, ByRef r_sPassword As Object, ByRef r_lPartyCnt As Object, ByVal v_sTitle As String, ByVal v_sMaritalStatusCode As String, ByVal v_sAddress1 As String, ByVal v_sAddress2 As String, ByVal v_sAddress3 As String, ByVal v_sAddress4 As String, ByVal v_sPostcode As String, ByRef r_vAdditionalDataArray As Object) As Integer

        Dim result As Integer = 0
        Dim oBOM As Object
        Dim sClassBOMAppName As String = ""
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'LogMessageToFile m_sUsername, PMLogOnError, "Start", "bGIS", "Security", "RegisterUser"

            '
            ' Create the BackOfficeMapper Security class for the given DataModel
            '
            'sClassBOMAppName = "bGISBOM" & v_sGISDataModelCode & ".Security"
            'Set oBOM = CreateObject(sClassBOMAppName)

            'lReturn = oBOM.Initialise( _
            'm_sUsername, _
            'm_sPassword, _
            'm_iUserID, _
            'm_iSourceID, _
            'm_iLanguageID, _
            'm_iCurrencyID, _
            'm_iLogLevel, _
            'ACApp)

            ' RAG 16/11/00
            ' Use the CreateBOM function to create the BOM (if required)
            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                ' Call the RegisterUser method of the specific bGISBOMx.Security class

                lReturn = oBOM.RegisterUser(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sForename:=gPMFunctions.ToSafeString(v_sForename), v_sSurname:=gPMFunctions.ToSafeString(v_sSurname), v_sMothersMaidenName:=gPMFunctions.ToSafeString(v_sMothersMaidenName), v_sDateOfBirth:=gPMFunctions.ToSafeString(v_sDateOfBirth), v_sEmailAddress:=gPMFunctions.ToSafeString(v_sEmailAddress), v_sMemorableDate:=gPMFunctions.ToSafeString(v_sMemorableDate), v_sAQuestion:=gPMFunctions.ToSafeString(v_sAQuestion),
                                            v_sTheAnswer:=gPMFunctions.ToSafeString(v_sTheAnswer), v_sCurrentRenewalDate:=gPMFunctions.ToSafeString(v_sCurrentRenewalDate),
                                            r_sUserID:=r_sUserID, r_sPassword:=r_sPassword, r_lPartyCnt:=r_lPartyCnt,
                                            v_sTitle:=gPMFunctions.ToSafeString(v_sTitle), v_sMaritalStatusCode:=gPMFunctions.ToSafeString(v_sMaritalStatusCode),
                                            v_sAddress1:=gPMFunctions.ToSafeString(v_sAddress1), v_sAddress2:=gPMFunctions.ToSafeString(v_sAddress2), v_sAddress3:=gPMFunctions.ToSafeString(v_sAddress3),
                                            v_sAddress4:=gPMFunctions.ToSafeString(v_sAddress4), v_sPostcode:=gPMFunctions.ToSafeString(v_sPostcode),
                                            r_vAdditionalDataArray:=r_vAdditionalDataArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.Security class

                oBOM.Dispose()
                oBOM = Nothing

            End If

            'LogMessageToFile m_sUsername, PMLogOnError, "End", "bGIS", "Security", "RegisterUser"

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RegisterUser Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RegisterUser", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' RAG070600
    ' Method to attemp to log in an existing user
    ' RFC200700 - Return the PMUserID if the login is a TPA
    Public Function LoginUser(ByVal v_sGisDataModelCode As Object, ByVal v_sGisBusinessTypeCode As Object, ByVal v_sUserID As Object, ByVal v_sPassword As Object, ByRef r_lPartyCnt As Object, ByRef r_lPMUserID As Object, ByRef r_sSurname As Object, ByRef r_sForename As Object, ByRef r_dtDateOfBirth As Object, ByRef r_sEmailAddress As Object, ByRef r_vAdditionalDataArray As Object) As Integer

        Dim result As Integer = 0
        Dim oBOM As Object
        Dim sClassBOMAppName As String = ""
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '
            ' Create the BackOfficeMapper Security class for the given DataModel
            '
            'sClassBOMAppName = "bGISBOM" & v_sGISDataModelCode & ".Security"
            'Set oBOM = CreateObject(sClassBOMAppName)

            'lReturn = oBOM.Initialise( _
            'm_sUsername, _
            'm_sPassword, _
            'm_iUserID, _
            'm_iSourceID, _
            'm_iLanguageID, _
            'm_iCurrencyID, _
            'm_iLogLevel, _
            'ACApp)

            ' RAG 16/11/00
            ' Use the CreateBOM function to create the BOM (if required)

            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                ' Call the LoginUser method of the specific bGISBOMx.Security class

                lReturn = oBOM.LoginUser(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode),
                                         v_sUserID:=gPMFunctions.ToSafeString(v_sUserID), v_sPassword:=v_sPassword,
                                         r_lPartyCnt:=r_lPartyCnt, r_lPMUserID:=r_lPMUserID, r_sPartySurname:=r_sSurname, r_sPartyForename:=r_sForename,
                                         r_dtDateOfBirth:=r_dtDateOfBirth, r_sEMail:=r_sEmailAddress, r_vAdditionalDataArray:=r_vAdditionalDataArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.Security class

                oBOM.Dispose()
                oBOM = Nothing

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoginUser Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoginUser", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    'Private Function UpdateUser(ByVal v_lPartyCnt As Long, _
    ''                        ByVal v_sGISDataModelCode As String, _
    ''                        ByVal v_sForename As String, _
    ''                        ByVal v_sSurname As String, _
    ''                        ByVal v_sAddress1 As String, _
    ''                        ByVal v_sAddress2 As String, _
    ''                        ByVal v_sAddress3 As String, _
    ''                        ByVal v_sAddress4 As String, _
    ''                        ByVal v_sPostcode As String, _
    ''                        ByVal v_sMothersMaidenName As String, _
    ''                        ByVal v_sDateOfBirth As String, _
    ''                        ByVal v_sEmailAddress As String, _
    ''                        ByVal v_sMemorableDate As String, _
    ''                        ByVal v_sAQuestion As String, _
    ''                        ByVal v_sTheAnswer As String, _
    ''                        ByVal v_sCurrentRenewalDate As String) As Long
    '
    'Dim oBOM As Object
    'Dim sClassBOMAppName As String
    'Dim lReturn As Long
    '
    '    On Error GoTo Err_UpdateUser
    '
    '    UpdateUser = PMTrue
    '
    '    'LogMessageToFile m_sUsername, PMLogOnError, "Start", "bGIS", "Security", "UpdateUser"
    '
    '    '
    '    ' Create the BackOfficeMapper Security class for the given DataModel
    '    '
    '    sClassBOMAppName = "bGISBOM" & v_sGISDataModelCode & ".Security"
    '    Set oBOM = CreateObject(sClassBOMAppName)
    '
    '    lReturn = oBOM.Initialise( _
    ''        m_sUsername, _
    ''        m_sPassword, _
    ''        m_iUserID, _
    ''        m_iSourceID, _
    ''        m_iLanguageID, _
    ''        m_iCurrencyID, _
    ''        m_iLogLevel, _
    ''        ACApp)
    '
    '    lReturn = oBOM.UpdateUser(v_lPartyCnt:=gpmfunctions.ToSafeInteger(v_lPartyCnt), _
    ''                        v_sForename:=gpmfunctions.ToSafeString(v_sForename), _
    ''                        v_sSurname:=gpmfunctions.ToSafeString(v_sSurname), _
    ''                        v_sAddress1:=gpmfunctions.ToSafeString(v_sAddress1), _
    ''                        v_sAddress2:=gpmfunctions.ToSafeString(v_sAddress2), _
    ''                        v_sAddress3:=gpmfunctions.ToSafeString(v_sAddress3), _
    ''                        v_sAddress4:=gpmfunctions.ToSafeString(v_sAddress4), _
    ''                        v_sPostcode:=gpmfunctions.ToSafeString(v_sPostcode), _
    ''                        v_sMothersMaidenName:=gpmfunctions.ToSafeString(v_sMothersMaidenName), _
    ''                        v_sDateOfBirth:=gpmfunctions.ToSafeString(v_sDateOfBirth), _
    ''                        v_sEmailAddress:=gpmfunctions.ToSafeString(v_sEmailAddress), _
    ''                        v_sMemorableDate:=gpmfunctions.ToSafeString(v_sMemorableDate), _
    ''                        v_sAQuestion:=gpmfunctions.ToSafeString(v_sAQuestion), _
    ''                        v_sTheAnswer:=gpmfunctions.ToSafeString(v_sTheAnswer), _
    ''                        v_sCurrentRenewalDate:=gpmfunctions.ToSafeString(v_sCurrentRenewalDate))
    '
    '    If (lReturn <> PMTrue) Then
    '        UpdateUser = lReturn
    '        Exit Function
    '    End If
    '
    '    ' Destroy the BOM.Security class
    '    lReturn = oBOM.Terminate()
    '    Set oBOM = Nothing
    '
    '    Exit Function
    '
    'Err_UpdateUser:
    '    ' Error
    '    UpdateUser = PMError
    '
    '    ' Log Error Message
    '    LogMessageFile _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="UpdateUser Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="UpdateUser", _
    ''        vErrNo:=Informations.Err.Number, _
    ''        vErrDesc:=Informations.Err.Description
    '
    '    Exit Function
    '


    Public Function LoginAgent(ByVal v_sDataModelCode As Object, ByVal v_sBusinessTypeCode As Object, ByVal v_sUsername As Object, ByVal v_sPassword As Object, ByRef r_lAgentCnt As Object, ByRef r_lPMUserID As Object, ByRef r_bUnrestrictedSearch As Object, ByRef r_dtPasswordChangeDate As Object, ByRef r_dtLastlogin As Object, ByRef r_sForename As Object, ByRef r_sSurname As Object, ByRef r_sEmailAddress As Object, ByRef r_iLanguageId As Object, ByRef r_vSourceList As Object, ByRef r_vAdditionalDataArray As Object) As Integer
        '******************************************************************************
        '        Function Name:  LoginAgent
        '******************************************************************************
        '           Created By:  Ahmed "Jay" Bishtawi
        '           Created On:  19-Sep-2002
        '******************************************************************************
        '       Parameters Are:
        '                        (In)     - v_sDataModelCode       - Variant  -
        '                        (In)     - v_sBusinessTypeCode    - Variant  -
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
        ' Function Description:  This function is used to log Agent on Agent Online.
        '******************************************************************************

        Dim result As Integer = 0
        Try

            Dim oBOM As Object
            Dim lReturn As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Use the CreateBOM function to create the BOM (if required)

            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=v_sDataModelCode, v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoginAgent Failed to Create BOM Object", vApp:=ACApp, vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            If Not (oBOM Is Nothing) Then

                ' Call the LoginAgent method of the specific bGISBOMx.Security class

                lReturn = oBOM.LoginAgent(v_sDataModelCode:=v_sDataModelCode, v_sBusinessTypeCode:=gPMFunctions.ToSafeString(v_sBusinessTypeCode),
                                          v_sUsername:=gPMFunctions.ToSafeString(v_sUsername), v_sPassword:=v_sPassword,
                                          r_lAgentCnt:=r_lAgentCnt, r_lPMUserID:=r_lPMUserID, r_bUnrestrictedSearch:=r_bUnrestrictedSearch, r_dtPasswordChangeDate:=r_dtPasswordChangeDate,
                                          r_dtLastlogin:=r_dtLastlogin, r_sForename:=r_sForename, r_sSurname:=r_sSurname, r_sEmailAddress:=r_sEmailAddress, r_iLanguageId:=r_iLanguageId,
                                          r_vSourceList:=r_vSourceList, r_vAdditionalDataArray:=r_vAdditionalDataArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BOM Object LoginAgent Method Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Destroy the BOM.Security class

                oBOM.Dispose()
                oBOM = Nothing

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoginAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function
    Public Function LogoffAgent(ByVal v_sDataModelCode As Object, ByVal v_sBusinessTypeCode As Object, ByVal v_sUsername As Object, ByRef r_vAdditionalDataArray As Object) As Integer
        '******************************************************************************
        '        Function Name:  LogoffAgent
        '******************************************************************************
        '           Created By:  Ahmed "Jay" Bishtawi
        '           Created On:  19-Sep-2002
        '******************************************************************************
        '       Parameters Are:
        '                        (In)     - v_sDataModelCode       - Variant  -
        '                        (In)     - v_sBusinessTypeCode    - Variant  -
        '                        (In)     - v_sUserName            - Variant  -
        '                        (In/Out) - r_vAdditionalDataArray - Variant  -
        '
        ' Return Value Type Is:  Long -
        '******************************************************************************
        ' Function Description:  This method is used to log agent off the Agents Online
        '******************************************************************************
        Dim result As Integer = 0
        Try

            Dim oBOM As Object
            Dim lReturn As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Use the CreateBOM function to create the BOM (if required)

            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=v_sDataModelCode, v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LogoffAgent Failed to Create BOM Object", vApp:=ACApp, vClass:=ACClass, vMethod:="LogoffAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Not (oBOM Is Nothing) Then

                ' Call the LogoffAgent method of the specific bGISBOMx.Security class

                lReturn = oBOM.LogoffAgent(v_sDataModelCode:=v_sDataModelCode, v_sBusinessTypeCode:=gPMFunctions.ToSafeString(v_sBusinessTypeCode), v_sUsername:=gPMFunctions.ToSafeString(v_sUsername), r_vAdditionalDataArray:=r_vAdditionalDataArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BOM Object LogoffAgent Method Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LogoffAgent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Destroy the BOM.Security class

                oBOM.Dispose()
                oBOM = Nothing

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LogoffAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LogoffAgent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function
    Public Function UpdateAgentLogonDetails(ByVal v_sDataModelCode As Object, ByVal v_sBusinessTypeCode As Object, ByVal v_sUsername As Object, ByVal v_sPassword As Object, ByVal v_sNewPassword As Object, ByRef r_vAdditionalDataArray As Object) As Integer
        '******************************************************************************
        '        Function Name:  UpdateAgentLogonDetails
        '******************************************************************************
        '           Created By:  Ahmed "Jay" Bishtawi
        '           Created On:  19-Sep-2002
        '******************************************************************************
        '       Parameters Are:
        '                        (In)     - v_sDataModelCode       - Variant  -
        '                        (In)     - v_sBusinessTypeCode    - Variant  -
        '                        (In)     - v_sUserName            - Variant  -
        '                        (In)     - v_sPassword            - Variant  -
        '                        (In)     - v_sNewPassword         - Variant  -
        '                        (In/Out) - r_vAdditionalDataArray - Variant  -
        '
        ' Return Value Type Is:  Long -
        '******************************************************************************
        ' Function Description:  This method is used to update Agents Password,
        '                        additional elements could be added in the future.
        '******************************************************************************

        Dim result As Integer = 0
        Try

            Dim oBOM As Object
            Dim lReturn As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Use the CreateBOM function to create the BOM (if required)

            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=v_sDataModelCode, v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAgentLogonDetails Failed to Create BOM object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAgentLogonDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Not (oBOM Is Nothing) Then

                ' Call the LogoffAgent method of the specific bGISBOMx.Security class

                lReturn = oBOM.UpdateAgentLogonDetails(v_sDataModelCode:=v_sDataModelCode, v_sBusinessTypeCode:=gPMFunctions.ToSafeString(v_sBusinessTypeCode), v_sUsername:=gPMFunctions.ToSafeString(v_sUsername), v_sPassword:=v_sPassword, v_sNewPassword:=v_sNewPassword, r_vAdditionalDataArray:=r_vAdditionalDataArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BOM object UpdateAgentLogonDetails method Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAgentLogonDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Destroy the BOM.Security class

                oBOM.Dispose()
                oBOM = Nothing

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAgentLogonDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAgentLogonDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ForgottenPassword
    '
    ' Description: Calls the BOM to process a forgotten password
    '              i.e. new password + email user
    '
    ' History: 22/07/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function ForgottenPassword(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_sUsername As String, ByVal v_sIPAddress As String, ByVal v_sSubject As String, ByVal v_sMessage As Object, ByRef r_sEmailAddress As String, ByRef r_sNewPassword As String, Optional ByVal v_sFromEmail As String = "", Optional ByVal v_sFromName As String = "", Optional ByVal v_vProfileName As Object = Nothing, Optional ByVal v_vProfilePassword As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oBOM As Object
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Use the CreateBOM function to create the BOM (if required)
            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=v_sDataModelCode, v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ForgottenPassword Failed to Create BOM object", vApp:=ACApp, vClass:=ACClass, vMethod:="ForgottenPassword", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Not (oBOM Is Nothing) Then

                Dim sEmailAddress, sNewPassword As Object
                sEmailAddress = r_sEmailAddress
                sNewPassword = r_sNewPassword
                ' Call the ForgottenPassword method of the specific bGISBOMx.Security class

                lReturn = oBOM.ForgottenPassword(v_sUsername:=gPMFunctions.ToSafeString(v_sUsername), v_sIPAddress:=gPMFunctions.ToSafeString(v_sIPAddress),
                                                 v_sSubject:=gPMFunctions.ToSafeString(v_sSubject), v_sMessage:=gPMFunctions.ToSafeString(v_sMessage),
                                                 r_sEmailAddress:=sEmailAddress, r_sNewPassword:=sNewPassword,
                                                 v_sFromEmail:=gPMFunctions.ToSafeString(v_sFromEmail), v_sFromName:=gPMFunctions.ToSafeString(v_sFromName),
                                                 v_vProfileName:=v_vProfileName, v_vProfilePassword:=v_vProfilePassword)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BOM object ForgottenPassword method Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ForgottenPassword", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
                r_sEmailAddress = CStr(sEmailAddress)
                r_sNewPassword = CStr(sNewPassword)

                ' Destroy the BOM.Security class

                oBOM.Dispose()
                oBOM = Nothing

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ForgottenPassword Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ForgottenPassword", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function
End Class
