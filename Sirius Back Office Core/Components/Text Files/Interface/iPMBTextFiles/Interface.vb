Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.IO
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface

    ' Private constants
    Private Const ACClass As String = "Interface"

    ' Private variables
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lPartyCnt As Integer
    Private m_lReturn As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    'DC250101
    Private m_lRiskCodeId As Integer
    Private m_lRiskGroupId As Integer
    'DC290702
    Private m_lClaimCnt As Integer
    Private m_sClaimRef As String = ""

    ' Public Properties

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
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

    'DC250101
    Public Property RiskCodeId() As Integer
        Get
            Return m_lRiskCodeId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskCodeId = Value
        End Set
    End Property
    'DC250101
    Public Property RiskGroupId() As Integer
        Get
            Return m_lRiskGroupId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskGroupId = Value
        End Set
    End Property
    'DC260702
    Public Property ClaimCnt() As Integer
        Get
            Return m_lClaimCnt
        End Get
        Set(ByVal Value As Integer)
            m_lClaimCnt = Value
        End Set
    End Property
    Public Property ClaimRef() As String
        Get
            Return m_sClaimRef
        End Get
        Set(ByVal Value As String)
            m_sClaimRef = Value
        End Set
    End Property
    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 18/05/2000 CTAF - Created.
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
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 18/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessInterface
    '
    ' Description:
    '
    ' History: 18/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0
        Dim oMain As frmMain



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get a new instance of the form
        oMain = New frmMain()

        ' Set the properties
        With oMain
            .Task = m_iTask
            .PartyCnt = m_lPartyCnt
            .InsuranceFileCnt = m_lInsuranceFileCnt
            .InsuranceFolderCnt = m_lInsuranceFolderCnt
            'DC250101
            .RiskCodeId = m_lRiskCodeId
            .RiskGroupId = m_lRiskGroupId
            'DC290702
            .ClaimCnt = m_lClaimCnt
            .ClaimRef = m_sClaimRef
        End With


        ' Display
        oMain.ShowDialog()

        ' Get any properites
        With oMain
            m_lStatus = .Status
        End With

        ' Unload it
        oMain.Close()

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 18/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = ProcessInterface()

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
    ' Name: SetProcessModes
    '
    ' Description:
    '
    ' History: 17/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetKeys
    '
    ' Description:
    '
    ' History: 18/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If no keys, then exit out
            If Not Information.IsArray(vKeyArray) Then
                Return result
            End If

            ' Get the keys we need
            For iLoop1 As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)



                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)).Trim()
                    ' Party_cnt
                    Case PMNavKeyConst.PMKeyNamePartyCnt

                        m_lPartyCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameInsFileCnt

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameInsFolderCnt

                        m_lInsuranceFolderCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                        'DC250101
                    Case PMNavKeyConst.PMKeyNameRiskCodeID

                        m_lRiskCodeId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                        'DC250101
                    Case PMNavKeyConst.PMKeyNameRiskGroupID

                        m_lRiskGroupId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                        'DC290702 -start
                    Case PMNavKeyConst.PMKeyNameClaimID

                        m_lClaimCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case "claim_ref"

                        m_sClaimRef = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                        'DC290702 -end

                    Case Else
                        ' Do nothing

                End Select

            Next iLoop1

            Return result

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
    ' History: 18/05/2000 CTAF - Created.
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
    ' History: 18/05/2000 CTAF - Created.
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
End Class

