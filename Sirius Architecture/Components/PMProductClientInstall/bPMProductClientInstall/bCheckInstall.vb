Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("CheckInstall_NET.CheckInstall")>
Public NotInheritable Class CheckInstall
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: CheckInstall
    '
    ' Date: 02/02/1999
    '
    ' Description:
    '
    '
    ' Edit History:
    ' DAK221299 - Check for null in output parameters in
    '             CheckForClientInstall
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
    Private Const ACClass As String = "CheckInstall"

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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

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
    ' Name: CheckClientVersion
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function CheckClientVersion(ByVal v_lPMEProductFamily As Integer, ByVal v_sExistingClientVersion As String, ByRef r_sLatestClientVersion As String, ByRef r_iIsLatestClientMandatory As Integer, ByRef r_iIsClientAutoInstallable As Integer, ByRef r_sClientInstallPath As String, ByRef r_sClientInstallProgram As String, ByRef r_iClientRebootLevel As Integer, Optional ByRef r_sProductDescription As String = "") As Integer

        Dim result As Integer = 0
        Dim sPMProductCode As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim iPMProductID As Integer
        Dim iIsExistingInstall As gPMConstants.PMEReturnCode
        Dim sRequiredServerVersion As String = ""
        Dim dtServerSoftwareDate, dtClientSoftwareDate As Date
        Dim sClientInstallDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_sLatestClientVersion = ""

            ' Get the Product Code for this Family
            sPMProductCode = gPMConstants.PMProductCode(v_lPMEProductFamily)

            ' Is there a Client Install Entry for this Product Code
            lReturn = CheckForClientInstall(v_sPMProductCode:=sPMProductCode, r_iPMProductID:=iPMProductID, r_iIsExistingInstall:=iIsExistingInstall, r_sProductDescription:=r_sProductDescription)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If there isn't an entry just exit.
            If iIsExistingInstall = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If

            ' Get the Client Install Details
            lReturn = CType(GetClientInstallDetails(v_iPMProductID:=iPMProductID, r_sRequiredServerVersion:=sRequiredServerVersion, r_dtServerSoftwareDate:=dtServerSoftwareDate, r_sLatestClientVersion:=r_sLatestClientVersion, r_dtClientSoftwareDate:=dtClientSoftwareDate, r_iIsLatestClientMandatory:=r_iIsLatestClientMandatory, r_iIsClientAutoInstallable:=r_iIsClientAutoInstallable, r_sClientInstallPath:=r_sClientInstallPath, r_sClientInstallProgram:=r_sClientInstallProgram, r_sClientInstallDescription:=sClientInstallDescription, r_iClientRebootLevel:=r_iClientRebootLevel), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the Versions and Return the results.

            Return CompareVersions(v_sRequiredVersion:=r_sLatestClientVersion, v_sFoundVersion:=v_sExistingClientVersion)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckClientVersionFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckClientVersion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PUBLIC Methods (End)

    ' FRIEND Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetClientInstallDetails
    '
    ' Description: Gets the details for a Single Client Install Record
    '              from the database.
    '
    ' ***************************************************************** '
    Friend Function GetClientInstallDetails(ByVal v_iPMProductID As Integer, ByRef r_sRequiredServerVersion As String, ByRef r_dtServerSoftwareDate As Date, ByRef r_sLatestClientVersion As String, ByRef r_dtClientSoftwareDate As Date, ByRef r_iIsLatestClientMandatory As Integer, ByRef r_iIsClientAutoInstallable As Integer, ByRef r_sClientInstallPath As String, ByRef r_sClientInstallProgram As String, ByRef r_sClientInstallDescription As String, ByRef r_iClientRebootLevel As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Parameters
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="pmproduct_id", vValue:=CStr(v_iPMProductID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLSelect(sSQL:=ACSelectSQL, sSQLName:=ACSelectName, bStoredProcedure:=ACSelectStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            If m_oDatabase.Records.Count() < 1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'developer guide no.111 
            With m_oDatabase.Records.Item(0).Fields
                r_sRequiredServerVersion = m_oDatabase.Records.Item(0).Fields()("required_server_version")
                r_dtServerSoftwareDate = m_oDatabase.Records.Item(0).Fields("server_software_date")
                r_sLatestClientVersion = m_oDatabase.Records.Item(0).Fields("latest_client_version")
                r_dtClientSoftwareDate = m_oDatabase.Records.Item(0).Fields("client_software_date")
                r_iIsLatestClientMandatory = m_oDatabase.Records.Item(0).Fields("is_latest_client_mandatory")
                r_iIsClientAutoInstallable = m_oDatabase.Records.Item(0).Fields("is_client_auto_installable")
                r_sClientInstallPath = m_oDatabase.Records.Item(0).Fields("client_install_path")
                r_sClientInstallProgram = m_oDatabase.Records.Item(0).Fields("client_install_program")
                r_sClientInstallDescription = m_oDatabase.Records.Item(0).Fields("client_install_description")
                r_iClientRebootLevel = m_oDatabase.Records.Item(0).Fields("client_reboot_level")
            End With


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientInstallDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientInstallDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckForClientInstall
    '
    ' Description: Looks in the PMProductClientInstall table to see if
    '              there is an entry for the supplied PMProduct Code.
    '
    ' ***************************************************************** '
    Friend Function CheckForClientInstall(ByVal v_sPMProductCode As String, ByRef r_iPMProductID As Integer, ByRef r_iIsExistingInstall As Integer, Optional ByRef r_sProductDescription As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Parameters
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="pmproduct_code", vValue:=v_sPMProductCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="pmproduct_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="is_existing_install", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="description", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLAction(sSQL:=ACCheckSQL, sSQLName:=ACCheckName, bStoredProcedure:=ACCheckStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'DAK221299

                If Convert.IsDBNull(.Parameters.Item("pmproduct_id").Value) Or Informations.IsNothing(.Parameters.Item("pmproduct_id").Value) Then
                    r_iPMProductID = 0
                    r_iIsExistingInstall = 0
                    r_sProductDescription = ""
                Else
                    r_iPMProductID = .Parameters.Item("pmproduct_id").Value
                    r_iIsExistingInstall = .Parameters.Item("is_existing_install").Value
                    r_sProductDescription = .Parameters.Item("description").Value
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForClientInstallFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckForClientInstall", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckServerVersion
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Friend Function CheckServerVersion(ByVal v_lPMEProductFamily As Integer, ByVal v_sRequiredVersion As String, ByRef r_sFoundVersion As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_sFoundVersion = ""

            lReturn = GetServerVersion(v_lPMEProductFamily:=v_lPMEProductFamily, r_sVersion:=r_sFoundVersion)

            If r_sFoundVersion.Trim() = "" Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return CompareVersions(v_sRequiredVersion:=v_sRequiredVersion, v_sFoundVersion:=r_sFoundVersion)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckServerVersionFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckServerVersion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' FRIEND Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: CompareVersions
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function CompareVersions(ByVal v_sRequiredVersion As String, ByVal v_sFoundVersion As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sRequiredBuild As String = ""
        Dim sFoundBuild As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        lReturn = CType(CalcBuildNumber(v_sVersion:=v_sFoundVersion, r_sBuildNumber:=sFoundBuild), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = CType(CalcBuildNumber(v_sVersion:=v_sRequiredVersion, r_sBuildNumber:=sRequiredBuild), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If sRequiredBuild = sFoundBuild Then
            Return result
        ElseIf (sRequiredBuild > sFoundBuild) Then
            Return gPMConstants.PMEReturnCode.PMEarlier
        Else
            Return gPMConstants.PMEReturnCode.PMLater
        End If




        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CompareVersionsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CompareVersions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetServerVersion
    '
    ' Description: Gets the Server Version of the installed PM Product.
    '
    ' ***************************************************************** '
    Private Function GetServerVersion(ByVal v_lPMEProductFamily As Integer, ByRef r_sVersion As String) As Integer

        Dim result As Integer = 0
        Dim sMsg As String = ""
        Dim lReturn As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        r_sVersion = ""

        ' Get the Version from the Registry
        lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=v_lPMEProductFamily, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=gPMConstants.PMRegKeyVersion, r_sSettingValue:=r_sVersion)

        If r_sVersion.Trim() = "" Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get SERVER Software Version for Product Family: " & v_lPMEProductFamily, vApp:=ACApp, vClass:=ACClass, vMethod:="GetServerVersion")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

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

End Class

