Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed


    Implements IDisposable
    ' Instance of the form
    Private m_frmInterface As frmInterface
    Private m_lReturn As Integer

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date


    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer
        Dim result As Integer = 0
        Dim sTitle, sMessage As String

        Const kMethodName As String = "Initialise"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Start"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = ProcessInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                gPMFunctions.RaiseError(kMethodName, "Failed to process the interface", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessInterface"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = LoadInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            gPMFunctions.RaiseError(kMethodName, "Failed to load interface", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Display the interface.
        m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            gPMFunctions.RaiseError(kMethodName, "Failed to show interface", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Destroy the interface from memory.
        m_lReturn = UnLoadInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the interface.
            gPMFunctions.RaiseError(kMethodName, "Failed to unload interface", gPMConstants.PMELogLevel.PMLogError)
        End If



        Return result
    End Function


    ' ***************************************************************** '
    ' Name: LoadInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    '
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LoadInterface"


        result = gPMConstants.PMEReturnCode.PMTrue

        m_frmInterface = New frmInterface()
        ' Assign the parameters to the interface properties.


        'Developer Guide No. 68


        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UnLoadInterface"


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Unload and destroy the instance of the interface
        ' from memory.
        m_frmInterface.Close()
        m_frmInterface = Nothing



        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "ShowInterface"


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        VB6.ShowForm(m_frmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            '        If (frmInterface.ErrorNumber <> 0) Then
            '            ShowInterface = frmInterface.ErrorNumber
            '        End If
        End If



        Return result

    End Function
    'PRIVATE Methods (End)



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
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        Const kMethodName As String = "Class_Initialize"
        Try


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
    End Sub
    '
    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub



    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' Date :18/06/2007
    '
    ' Edit History:VB
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        'Dim lRow As Long
        '
        '    On Error GoTo Err_SetKeys
        '
        Return gPMConstants.PMEReturnCode.PMTrue
        '
        '    ' Check we have a valid array.
        '    If (IsArray(vKeyArray) = False) Then
        '        SetKeys = PMFalse
        '        Exit Function
        '    End If
        '
        '    ' Step through the key array.
        '    For lRow& = LBound(vKeyArray, 2) To UBound(vKeyArray, 2)
        '
        '        Select Case Trim$(CStr(vKeyArray(PMKeyName, lRow&)))
        '        End Select
        '
        '    Next lRow&
        '
        '    Exit Function
        '
        'Err_SetKeys:
        '
        '    SetKeys = PMError
        '
        '    ' Log Error Message
        '    LogMessage _
        ''        iType:=PMLogOnError, _
        ''        sMsg:="SetKeys Failed", _
        ''        vApp:=ACApp, _
        ''        vClass:=ACClass, _
        ''        vMethod:="SetKeys", _
        ''        vErrNo:=Err.Number, _
        ''        vErrDesc:=Err.Description
        '
        '    Exit Function

    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' Date :18/06/2007
    '
    ' Edit History:VB
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        'Dim lRow As Long
        '
        '    On Error GoTo Err_GetKeys
        '
        Return gPMConstants.PMEReturnCode.PMTrue
        '
        '    ' {* USER DEFINED CODE (Begin) *}
        '
        '    ' Initialise the key array with the number of
        '    ' keys needed to be returned.
        '    ' Note: Remember arrays are zero based.
        '    ReDim vKeyArray(1, 7)
        '
        '    ' Assign the key array with the parameter members.
        '    vKeyArray(PMKeyName, 0) = "case" '- -PMKeyNameClaimCnt
        '    vKeyArray(PMKeyValue, 0) = 0
        '    vKeyArray(PMKeyName, 1) = PMKeyNamePolicyID
        '    vKeyArray(PMKeyValue, 1) = 0
        '    vKeyArray(PMKeyName, 2) = PMKeyNameClaimReference
        '    vKeyArray(PMKeyValue, 2) = 0
        '
        '    ' {* USER DEFINED CODE (End) *}
        '
        '    Exit Function
        '
        'Err_GetKeys:
        '
        '    GetKeys = PMError
        '
        '    ' Log Error Message
        '    LogMessage _
        ''        iType:=PMLogOnError, _
        ''        sMsg:="GetKeys Failed", _
        ''        vApp:=ACApp, _
        ''        vClass:=ACClass, _
        ''        vMethod:="GetKeys", _
        ''        vErrNo:=Err.Number, _
        ''        vErrDesc:=Err.Description
        '
        '    Exit Function

    End Function

    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer


        Return gPMConstants.PMEReturnCode.PMTrue
        '
        '    ' {* USER DEFINED CODE (Begin) *}
        '
        '    ' Initialise the summary array with the number of
        '    ' items needed to be returned.
        '    ' Note: Remember arrays are zero based.
        '    ReDim vSummaryArray(PMNavSummValue, 0)
        '
        '    ' Assign the key array with the parameter members.
        '    vSummaryArray(PMNavSummHeading, 0) = "Claim Reference"
        '    vSummaryArray(PMNavSummValue, 0) = m_sClaimRef$
        '
        '    ' {* USER DEFINED CODE (End) *}
        '
        '    Exit Function
        '
        '
        'Err_GetSummary:
        '
        '    GetSummary = PMError
        '
        '    ' Log Error Message
        '    LogMessage _
        ''        iType:=PMLogOnError, _
        ''        sMsg:="GetSummary Failed", _
        ''        vApp:=ACApp, _
        ''        vClass:=ACClass, _
        ''        vMethod:="GetSummary", _
        ''        vErrNo:=Err.Number, _
        ''        vErrDesc:=Err.Description
        '
        '    Exit Function

    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' Date :18/06/2007
    '
    ' Edit History :VB
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
End Class
