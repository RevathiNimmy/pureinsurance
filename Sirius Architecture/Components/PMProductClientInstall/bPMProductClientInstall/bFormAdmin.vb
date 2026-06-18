Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("FormAdmin_NET.FormAdmin")>
Public NotInheritable Class FormAdmin
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: FormAdmin
    '
    ' Date: 21/01/1999
    '
    ' Description:
    '
    '
    ' Edit History:
    '
    ' ***************************************************************** '


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


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "FormAdmin"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
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

            ' Check the Supplied Database.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lReturn = oCS.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID,  _
            'v_lPMProductFamily:=PMProductFamily, _
            'r_bNewInstanceCreated:=m_bCloseDatabase, _
            'r_oCheckedDatabase:=m_oDatabase, _
            'v_vDatabase:=vDatabase)

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            '    Set oCS = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    ' Name: GetAll
    '
    ' Description: Gets All of the Client Install Records from the
    '              database.
    '
    ' ***************************************************************** '
    Public Function GetAll(ByRef r_vClientInstallArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Parameters
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACSelectAllSQL, sSQLName:=ACSelectAllName, bStoredProcedure:=ACSelectAllStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vClientInstallArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAll", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSingle
    '
    ' Description: Gets the details for a Single Client Install Record
    '              from the database.
    '
    ' ***************************************************************** '
    Public Function GetSingle(ByVal v_iPMProductID As Integer, ByRef r_sRequiredServerVersion As String, ByRef r_dtServerSoftwareDate As Date, ByRef r_sLatestClientVersion As String, ByRef r_dtClientSoftwareDate As Date, ByRef r_iIsLatestClientMandatory As Integer, ByRef r_iIsClientAutoInstallable As Integer, ByRef r_sClientInstallPath As String, ByRef r_sClientInstallProgram As String, ByRef r_sClientInstallDescription As String, ByRef r_iClientRebootLevel As Integer) As Integer

        Dim result As Integer = 0
        Dim oCheckInstall As bPMProductClientInstall.CheckInstall
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the Check Install Class
            oCheckInstall = New bPMProductClientInstall.CheckInstall()
            lReturn = CType(oCheckInstall.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oCheckInstall = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            result = oCheckInstall.GetClientInstallDetails(v_iPMProductID:=v_iPMProductID, r_sRequiredServerVersion:=r_sRequiredServerVersion, r_dtServerSoftwareDate:=r_dtServerSoftwareDate, r_sLatestClientVersion:=r_sLatestClientVersion, r_dtClientSoftwareDate:=r_dtClientSoftwareDate, r_iIsLatestClientMandatory:=r_iIsLatestClientMandatory, r_iIsClientAutoInstallable:=r_iIsClientAutoInstallable, r_sClientInstallPath:=r_sClientInstallPath, r_sClientInstallProgram:=r_sClientInstallProgram, r_sClientInstallDescription:=r_sClientInstallDescription, r_iClientRebootLevel:=r_iClientRebootLevel)

            ' Destroy the Class
            oCheckInstall.Dispose()
            oCheckInstall = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSingleFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSingle", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd
    '
    ' Description: Adds or Updates a Client Install record in the
    '              database.
    '
    ' ***************************************************************** '
    Public Function DirectAddOrUpdate(ByVal v_bUpdate As Boolean, ByVal v_iPMProductID As Integer, ByVal v_sRequiredServerVersion As String, ByVal v_dtServerSoftwareDate As Date, ByVal v_sLatestClientVersion As String, ByVal v_dtClientSoftwareDate As Date, ByVal v_iIsLatestClientMandatory As Integer, ByVal v_iIsClientAutoInstallable As Integer, ByVal v_sClientInstallPath As String, ByVal v_sClientInstallProgram As String, ByVal v_sClientInstallDescription As String, ByVal v_iClientRebootLevel As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = CType(AddInputParam(v_sRequiredServerVersion:=v_sRequiredServerVersion, v_dtServerSoftwareDate:=v_dtServerSoftwareDate, v_sLatestClientVersion:=v_sLatestClientVersion, v_dtClientSoftwareDate:=v_dtClientSoftwareDate, v_iIsLatestClientMandatory:=v_iIsLatestClientMandatory, v_iIsClientAutoInstallable:=v_iIsClientAutoInstallable, v_sClientInstallPath:=v_sClientInstallPath, v_sClientInstallProgram:=v_sClientInstallProgram, v_sClientInstallDescription:=v_sClientInstallDescription, v_iClientRebootLevel:=v_iClientRebootLevel), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddKeyInputParam(v_iPMProductID:=v_iPMProductID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_bUpdate Then
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored)
            Else
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAddFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DirectDelete
    '
    ' Description: Deletes a Client Install Record From the Registry.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(ByVal v_iPMProductID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = CType(AddKeyInputParam(v_iPMProductID:=v_iPMProductID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDeleteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckForExisting
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function CheckForExisting(ByVal v_sPMProductCode As String, ByRef r_iPMProductID As Integer, ByRef r_iIsExistingInstall As Integer) As Integer

        Dim result As Integer = 0
        Dim oCheckInstall As bPMProductClientInstall.CheckInstall
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the Check Install Class
            oCheckInstall = New bPMProductClientInstall.CheckInstall()
            lReturn = CType(oCheckInstall.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oCheckInstall = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the Check For Client Install Method & Return the result
            result = oCheckInstall.CheckForClientInstall(v_sPMProductCode:=v_sPMProductCode, r_iPMProductID:=r_iPMProductID, r_iIsExistingInstall:=r_iIsExistingInstall)

            ' Destroy the Class
            oCheckInstall.Dispose()
            oCheckInstall = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForExistingFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckForExisting", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckServerVersion
    '
    ' Description: This method checks the Server Version of the Installed
    '              software for the Product Code supplied.
    '
    ' Returns:     PMTrue               All OK
    '              PMInvalidRequest     Product Code is unknown
    '              PMNotFound           Server Version not found in reg
    '              PMEarlier            The Version found is earlier
    '                                   than the one required.
    '              PMLater              The Version found is later
    '                                   then the one required.
    ' ***************************************************************** '
    Public Function CheckServerVersion(ByVal v_sPMProductCode As String, ByVal v_sRequiredServerVersion As String, ByRef r_sFoundVersion As String) As Integer

        Dim result As Integer = 0
        Dim oCheckInstall As bPMProductClientInstall.CheckInstall
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPMEProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Work out the Product Family from the Product Code
            lPMEProductFamily = CType(gPMConstants.PMProductFamilyByCode(v_sPMProductCode), gPMConstants.PMEProductFamily)

            ' Was the Product Code Valid?
            If lPMEProductFamily = 0 Then
                result = gPMConstants.PMEReturnCode.PMInvalidRequest
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Product Code " & v_sPMProductCode & " is not valid.", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckServerVersion")
                Return result
            End If

            ' Create a New Instance of the Check Install Class
            oCheckInstall = New bPMProductClientInstall.CheckInstall()
            lReturn = CType(oCheckInstall.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oCheckInstall = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Server Version
            result = oCheckInstall.CheckServerVersion(v_lPMEProductFamily:=lPMEProductFamily, v_sRequiredVersion:=v_sRequiredServerVersion, r_sFoundVersion:=r_sFoundVersion)

            oCheckInstall.Dispose()
            oCheckInstall = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckServerVersionFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckServerVersion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam(ByVal v_sRequiredServerVersion As String, ByVal v_dtServerSoftwareDate As Date, ByVal v_sLatestClientVersion As String, ByVal v_dtClientSoftwareDate As Date, ByVal v_iIsLatestClientMandatory As Integer, ByVal v_iIsClientAutoInstallable As Integer, ByVal v_sClientInstallPath As String, ByVal v_sClientInstallProgram As String, ByVal v_sClientInstallDescription As String, ByVal v_iClientRebootLevel As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="required_server_version", vValue:=v_sRequiredServerVersion, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="server_software_date", vValue:=Informations.FormatDateTime(v_dtServerSoftwareDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="latest_client_version", vValue:=v_sLatestClientVersion, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="client_software_date", vValue:=Informations.FormatDateTime(v_dtClientSoftwareDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_latest_client_mandatory", vValue:=CStr(v_iIsLatestClientMandatory), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_client_auto_installable", vValue:=CStr(v_iIsClientAutoInstallable), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="client_install_path", vValue:=v_sClientInstallPath, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="client_install_program", vValue:=v_sClientInstallProgram, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="client_install_description", vValue:=v_sClientInstallDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="client_reboot_level", vValue:=CStr(v_iClientRebootLevel), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyInputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY INPUT parameters
    '              required for a Select, Delete or Update.
    '
    ' ***************************************************************** '
    Private Function AddKeyInputParam(ByVal v_iPMProductID As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="pmproduct_id", vValue:=CStr(v_iPMProductID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
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
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: GetSysAdminStatus
    '
    ' Description: check if user is member of a SysAdmin user group.
    '
    ' History: RDC 17102002 created
    ' ***************************************************************** '
    Public Function GetSysAdminStatus(ByRef lStatus As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = CType(gPMComponentServices.GetSysAdminAccessStatus(m_sUsername, m_iUserID, m_iSourceID, m_iLanguageID, lStatus, m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSysAdminStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSysAdminStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class

