Imports System.Web.Services.Description
Imports SharedFiles
Imports SSP.S4I
Module MainModule

#Region "Application Constants"
    Public Const ACApp As String = "bSIRBatchRenewalController"

#End Region

#Region "Fields"
    ' Basic command details
    Private m_oInterface As ProcessJobs = Nothing
    Private m_bIsHelp As Boolean = False
    Private m_bIsList As Boolean = False
    Private m_sSAMURL As String
    Private m_sSAMUsername As String
    Private m_sSAMPassword As String
    Private m_sBranchCode As String
    Private m_sClientId As String
    Private m_sTenantId As String
    Private m_sTokenUrl As String
#End Region

#Region "Main Method"

    Sub Main()

        ' Encapsulate entire app in error loop
        Try
            ' Strip command line
            ProcessCommandLine()

            'Close DB Connection
            m_oInterface.CloseDBConnection()

            ' clean up any interops used by the export process
            m_oInterface = Nothing

        Catch ex As Exception

            OutputLine("Batch Renewal FAILED" & Environment.NewLine & Environment.NewLine & ex.ToString())

        End Try
    End Sub
#End Region

#Region "Public Methods"
    Public Function GetSystemOption(ByVal iOptionNumber As Integer) As String
        Dim lResult As Integer = 0
        Dim oSystemOptions As bSIROptions.Business = Nothing
        Dim sOptionValue As String = String.Empty

        Try

            ' Create the System Options Object
            oSystemOptions = New bSIROptions.Business
            If (oSystemOptions Is Nothing) Then
                Throw New Exception("Unable to create bSIROptions.Business")
            End If

            ' Initialise
            lResult = oSystemOptions.Initialise(
                sUsername:="",
                sPassword:="",
                iUserID:=1,
                iSourceID:=1,
                iLanguageID:=1,
                iCurrencyID:=26,
                iLogLevel:=PMELogLevel.PMLogError,
                sCallingAppName:=ACApp)
            If lResult <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to initialise bSIROptions.Business")
            End If

            ' Get the system option
            lResult = oSystemOptions.GetOption(
                iOptionNumber:=CShort(iOptionNumber),
                sValue:=sOptionValue,
                v_iSourceID:=CShort(1))
            If lResult <> PMEReturnCode.PMTrue Then
                Throw New Exception(String.Format("Unable to retrieve system option '{0}'", iOptionNumber))
            End If

            ' Return the option value
            Return sOptionValue

        Catch ex As Exception
            Throw New Exception("Unable to retrieve system option", ex)

        Finally
            If Not oSystemOptions Is Nothing Then
                oSystemOptions.Dispose()
            End If
            oSystemOptions = Nothing
        End Try
    End Function

    Public Sub GetProductOption(
    ByVal m_oDatabase As dPMDAO.Database,
    ByVal productOption As Integer,
    ByVal branchId As Integer,
    ByRef optionValue As String)

        If m_oDatabase IsNot Nothing Then

            AddParameterLite(m_oDatabase, "option_number", productOption, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "branch_id", branchId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "option_value", optionValue, PMEParameterDirection.PMParamOutput, PMEDataType.PMString)

            m_oDatabase.SQLSelect("spu_SAM_Get_Product_option", "spu_SAM_Get_Product_option", True)

            optionValue = m_oDatabase.Parameters.Item("option_value").Value.ToString()

        End If

    End Sub

    ' Outputs feedback, currently to the console
    Public Sub OutputLine(ByVal message As String)
        ' Write message with carriage return
        Console.WriteLine(message)
    End Sub

    Public Sub OutputLine()
        ' Write carriage return (or new line)
        Console.WriteLine()
    End Sub

    Public Sub Output(ByVal message As String)
        ' Write message without carriage return
        Console.Write(message)
    End Sub
#End Region

#Region "Private Methods"

    ' Process command line for flags and commands
    Private Sub ProcessCommandLine()

        Dim sJobCode As String = ""
        Dim sUserName As String = ""
        Dim sPassword As String = ""

        If My.Application.CommandLineArgs.Count = 3 Then
            sJobCode = My.Application.CommandLineArgs.Item(0).ToString
            sUserName = My.Application.CommandLineArgs.Item(1).ToString
            sPassword = My.Application.CommandLineArgs.Item(2).ToString
            ProcessSettings()
            m_oInterface = New ProcessJobs(m_sSAMURL, m_sSAMUsername, m_sClientId, m_sTenantId, m_sTokenUrl)
            m_oInterface.ProcessJobs(sJobCode, sUserName, sPassword)
        Else
            OutputLine("Missing parameters <Renewal Job Code>  <Sirius User>  <Sirius Password>")
        End If

    End Sub

    Public Function GetErrorMsgString(ByVal v_sModule As String, ByVal v_sMethod As String)
        GetErrorMsgString = "Module: " & v_sModule & ", Method: " & v_sMethod
    End Function
    Private Sub ProcessSettings()
        If String.IsNullOrEmpty(My.Settings.ApiEndpoint) Then
            Throw New ApplicationException("SAMURL not set in App.Config ")
        End If
        m_sSAMURL = My.Settings.ApiEndpoint

        If String.IsNullOrEmpty(My.Settings.SAMUserName) Then
            Throw New ApplicationException("SAMUsername not set in App.Config ")
        End If
        m_sSAMUsername = My.Settings.SAMUserName

        m_sSAMPassword = My.Settings.SAMPassword

        If String.IsNullOrEmpty(My.Settings.ClientId) Then
            Throw New ApplicationException("ClientId not set in App.Config ")
        End If
        m_sClientId = My.Settings.ClientId

        m_sTenantId = My.Settings.TenantId
        If String.IsNullOrEmpty(My.Settings.TokenUrl) Then
            Throw New ApplicationException("Token Url not set in App.Config ")
        End If
        m_sTokenUrl = My.Settings.TokenUrl
    End Sub
#End Region

#Region "DestroyInteropComObject"
    'Friend Sub DestroyCOMInterop( _
    '                  ByRef oObject As Object, _
    '                    Optional ByVal bIgnoreTerminate As Boolean = False)

    '    Dim iRet As Int32

    '    ' call terminate on the object before releasing it
    '    If bIgnoreTerminate = False Then
    '        oObject.Dispose()
    '    End If

    '    ' Destroy the object reference
    '    System.Runtime.InteropServices.Marshal.ReleaseComObject(oObject)
    '    oObject = Nothing
    'End Sub
#End Region


End Module
