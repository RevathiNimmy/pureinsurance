Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("NavigatorV3_NET.NavigatorV3")> _
Public NotInheritable Class NavigatorV3
    Implements IDisposable
    Implements aPMNav.NavigatorV3

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "NavigatorV3"

#Region "Private Variable"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_sPMNavCancelAction As String = ""
    Private m_nTask As Integer
    Private m_nNavigate As Integer
    Private m_nProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_oInterface As MIDInterface

    Private bDisposedValue As Boolean

#End Region

#Region "Public Properties"

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
        End Get
    End Property

    Public WriteOnly Property PMNavCancelAction() As String
        Set(ByVal Value As String)

            m_sPMNavCancelAction = Value
        End Set
    End Property

    Public WriteOnly Property NavigatorV3_CallingAppName() As String Implements aPMNav.NavigatorV3.CallingAppName
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_oInterface.CallingAppName = Value
        End Set
    End Property

    Public WriteOnly Property NavigatorV3_PMAuthorityLevel() As Integer Implements aPMNav.NavigatorV3.PMAuthorityLevel
        Set(ByVal Value As Integer)

            m_oInterface.PMAuthorityLevel = Value
        End Set
    End Property

    Public ReadOnly Property NavigatorV3_Status() As Integer Implements aPMNav.NavigatorV3.Status
        Get

            ' Return the interface exit status.
            Return m_oInterface.Status
        End Get
    End Property

#End Region

#Region "Public Functions"

    ''' <summary>
    ''' Entry point for any initialisation code for this object
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Initialise() As Integer
        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Create instance of the Interface class
            m_oInterface = New MIDInterface()
            nResult = m_oInterface.Initialise()
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the Interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return nResult
            End If

            ' Initialise the process modes.
            m_nTask = gPMConstants.PMEComponentAction.PMEdit
            m_nNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_nProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

        Catch Excep As Exception
            ' Error Section.
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=Excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Entry point for any termination code for this object
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    ''' <summary>
    ''' Stores all of the parameter members with the key arrays
    ''' </summary>
    ''' <param name="vKeyArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NavigatorV3_SetKeys(ByRef vKeyArray(,) As Object) As Integer Implements aPMNav.NavigatorV3.SetKeys

        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Check we have a valid array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the corresponding Interface function
            nResult = CType(m_oInterface.SetKeys(r_aoKeyArray:=vKeyArray), gPMConstants.PMEReturnCode)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.SetKeys failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys")
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If
        Catch Excep As Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, excep:=Excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Stores all of the key array with the parameter members
    ''' </summary>
    ''' <param name="vKeyArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NavigatorV3_GetKeys(ByRef vKeyArray(,) As Object) As Integer Implements aPMNav.NavigatorV3.GetKeys

        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Call the corresponding Interface function
            nResult = CType(m_oInterface.GetKeys(r_aoKeyArray:=vKeyArray), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.GetKeys failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys")
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

        Catch Excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_GetKeys", excep:=Excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Stores all of the summary array with the parameter members
    ''' </summary>
    ''' <param name="vSummaryArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NavigatorV3_GetSummary(ByRef vSummaryArray As Object) As Integer Implements aPMNav.NavigatorV3.GetSummary

        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Call the corresponding Interface function
            nResult = CType(m_oInterface.GetSummary(r_oSummaryArray:=vSummaryArray), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.GetSummary failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary")
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If
        Catch Excep As Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_GetSummary", excep:=Excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Set the optional process modes.
    ''' </summary>
    ''' <param name="vTask"></param>
    ''' <param name="vNavigate"></param>
    ''' <param name="vProcessMode"></param>
    ''' <param name="vTransactionType"></param>
    ''' <param name="vEffectiveDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NavigatorV3_SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer Implements aPMNav.NavigatorV3.SetProcessModes
        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Call the corresponding Interface function
            nResult = CType(m_oInterface.SetProcessModes(r_oTask:=vTask, r_oNavigate:=vNavigate, r_oProcessMode:=vProcessMode, r_oTransactionType:=vTransactionType, r_oEffectiveDate:=vEffectiveDate), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.SetProcessModes failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

        Catch Excep As Exception
            ' Error Section.
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, excep:=Excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Entry point for the object to start its processing.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NavigatorV3_Start() As Integer Implements aPMNav.NavigatorV3.Start

        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Call the corresponding Interface function
            nResult = CType(m_oInterface.Start(), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Interface.Start failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

        Catch Excep As Exception
            ' Error Section.
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV3_Start", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, excep:=Excep)
        End Try

        Return nResult
    End Function

#End Region

#Region "Private/ Protected Functions"

    ''' <summary>
    ''' Calls Dispose
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ''' <summary>
    ''' Clears memory through object distruction
    ''' </summary>
    ''' <param name="disposing"></param>
    ''' <remarks></remarks>
    Protected Sub Dispose(disposing As Boolean)
        If Not Me.bDisposedValue Then
            Me.bDisposedValue = True
            If disposing Then
                If m_oInterface IsNot Nothing Then
                    m_oInterface.Dispose()

                End If
                m_oInterface = Nothing
            End If
        End If
        Me.bDisposedValue = True
    End Sub

#End Region

End Class
