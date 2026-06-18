Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'Modified by Sudhanshu Behera on 5/19/2010 3:22:08 PM refer developer guide no. 129 (guide)
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface

    Private Const ACClass As String = "Interface"

    ' Return value
    Private m_lReturn As Integer

    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer

    ' Process modes
    Private m_vTask As Object
    Private m_vNavigate As Object
    Private m_vProcessMode As Object
    Private m_vTransactionType As Object
    Private m_vEffectiveDate As Object

    Private m_sInterfaceName As String = ""
    Private m_oInterface As Object

    ' PolicyID
    Private m_vPolicyID As Object
    ' ClientID
    Private m_vClientID As Object

    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    Public Property PolicyID() As Object
        Get
            Return m_vPolicyID
        End Get
        Set(ByVal Value As Object)


            m_vPolicyID = Value
        End Set
    End Property

    Public Property ClientID() As Object
        Get
            Return m_vClientID
        End Get
        Set(ByVal Value As Object)


            m_vClientID = Value
        End Set
    End Property

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get what interface we're using
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="DocumentInterface", r_sSettingValue:=m_sInterfaceName)

            If m_sInterfaceName.Trim() = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="There is no 'DocumentInterface' configured in the registry." & Environment.NewLine & _
                                    "Please configure one in HKLM\Software\PM\SiriusSolutions\Client" & Environment.NewLine & _
                                   "e.g. Fish", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Get the object
            ' Note: the object is multiuse because its late bound
            m_oInterface = Activator.CreateInstance(System.Reflection.Assembly.GetAssembly(Type.GetType(ACApp & "." & m_sInterfaceName + "," + (ACApp & "." & m_sInterfaceName).Substring(0, (ACApp & "." & m_sInterfaceName).LastIndexOf(".")))).FullName, ACApp & "." & m_sInterfaceName).Unwrap()

            ' Initialise it

            m_lReturn = CType(m_oInterface, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
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
                If m_oInterface IsNot Nothing Then
                    m_oInterface.Dispose()
                End If
                m_oInterface = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: ProcessLink
    '
    ' Description:
    '
    ' History: 23/04/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessLink() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Pass in the properties


        m_oInterface.PolicyID = PolicyID


        m_oInterface.ClientID = ClientID

        ' Start the object

        m_lReturn = m_oInterface.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            ' Process what we need to process
            m_lReturn = ProcessLink()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetKeys
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetKeys
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSummary
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetProcessModes
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByVal vTask As Object = Nothing, Optional ByVal vNavigate As Object = Nothing, Optional ByVal vProcessMode As Object = Nothing, Optional ByVal vTransactionType As Object = Nothing, Optional ByVal vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Information.IsNothing(vTask) Then


                m_vTask = vTask
            End If


            If Not Information.IsNothing(vNavigate) Then


                m_vNavigate = vNavigate
            End If


            If Not Information.IsNothing(vProcessMode) Then


                m_vProcessMode = vProcessMode
            End If


            If Not Information.IsNothing(vTransactionType) Then


                m_vTransactionType = vTransactionType
            End If


            If Not Information.IsNothing(vEffectiveDate) Then


                m_vEffectiveDate = vEffectiveDate
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
