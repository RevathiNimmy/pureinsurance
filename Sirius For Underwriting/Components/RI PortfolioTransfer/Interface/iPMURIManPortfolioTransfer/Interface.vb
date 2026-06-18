Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 26/09/00
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '
    'Developer Guide No. 69
    Private frmRIManPortfolioTransfer As frmRIManPortfolioTransfer

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lPMAuthorityLevel As Integer

    Private m_lInsuranceFileCnt As Integer
    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Navigator starter
    Private WithEvents m_oNavStart As iPMNavStart.Interface_Renamed
    Private m_bNavClosed As Boolean
    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer
    Private m_bIsRenewalAmend As Boolean
    'sj 09/10/2002 - start
    Private m_lRunMode As Integer
    'sj 09/10/2002 - end

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            m_lPMAuthorityLevel = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property


    Public Property IsRenewalAmend() As Boolean
        Get
            Return m_bIsRenewalAmend
        End Get
        Set(ByVal Value As Boolean)
            m_bIsRenewalAmend = Value
        End Set
    End Property





    ' {* USER DEFINED CODE (Begin) *}


    ' {* USER DEFINED CODE (End) *}
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
    Public Function Initialise() As Integer


        Dim result As Integer = 0
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



            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer
        Const kMethodName As String = "SetKeys"
        Dim lRow As Integer

        Try

            SetKeys = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If (IsArray(vKeyArray) = False) Then
                SetKeys = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Step through the key array.
            For lRow = LBound(vKeyArray, 2) To UBound(vKeyArray, 2)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}

                'UPGRADE_WARNING: Couldn't resolve default property of object vKeyArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Select Case Trim(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)))
                    Case PMNavKeyConst.PMKeyNameInsFileCnt
                        'UPGRADE_WARNING: Couldn't resolve default property of object vKeyArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        g_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case "is_renewal_Amend"
                        'UPGRADE_WARNING: Couldn't resolve default property of object vKeyArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        IsRenewalAmend = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)
                        'sj 09/10/2002 - start
                    Case PMKeyNameRunMode
                        'UPGRADE_WARNING: Couldn't resolve default property of object vKeyArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        m_lRunMode = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'sj 09/10/2002 - end
                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=SetKeys, excep:=ex)

        Finally


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try


            ' {* USER DEFINED CODE (Begin) *}



            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try


            ' {* USER DEFINED CODE (Begin) *}


            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer
        Const kMethodName As String = "SetProcessModes"

        Try

            SetProcessModes = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If (IsNothing(vTask) = False) Then
                m_iTask = CShort(vTask)
            End If

            If (IsNothing(vNavigate) = False) Then
                m_lNavigate = CInt(vNavigate)
            End If

            If (IsNothing(vProcessMode) = False) Then
                m_lProcessMode = CInt(vProcessMode)
            End If

            If (IsNothing(vTransactionType) = False) Then
                m_sTransactionType = CStr(vTransactionType)
            End If

            If (IsNothing(vEffectiveDate) = False) Then
                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=SetProcessModes, excep:=ex)

        Finally


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default status to OK
            m_lStatus = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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


        result = gPMConstants.PMEReturnCode.PMTrue
        frmRIManPortfolioTransfer = New frmRIManPortfolioTransfer()
        frmRIManPortfolioTransfer.ShowDialog()

        frmRIManPortfolioTransfer = Nothing

        Return result

    End Function


    Private Function StartNavProcess(ByVal v_sProcessCode As String) As Integer
        Const kMethodName As String = "StartNavProcess"
        Dim vKeyArray(,) As Object



        StartNavProcess = gPMConstants.PMEReturnCode.PMTrue

        ' Get a new instance
        m_oNavStart = New iPMNavStart.Interface_Renamed

        ' Set the process code
        m_oNavStart.ProcessCode = v_sProcessCode

        ' Dont exit the function yet
        m_bNavClosed = False

        m_lReturn = m_oNavStart.Initialise()
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            StartNavProcess = gPMConstants.PMEReturnCode.PMFalse
            'UPGRADE_NOTE: Object m_oNavStart may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            m_oNavStart = Nothing
            Exit Function
        End If

        ' Pass InsuranceFileCnt
        ReDim vKeyArray(1, 0)
        'UPGRADE_WARNING: Couldn't resolve default property of object vKeyArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsFileCnt
        'UPGRADE_WARNING: Couldn't resolve default property of object vKeyArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lInsuranceFileCnt

        m_lReturn = m_oNavStart.SetKeys(vKeyArray:=vKeyArray)
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            StartNavProcess = gPMConstants.PMEReturnCode.PMFalse
            'UPGRADE_NOTE: Object m_oNavStart may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            m_oNavStart = Nothing
            Exit Function
        End If

        ' Start the component
        m_lReturn = m_oNavStart.Start()
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            StartNavProcess = gPMConstants.PMEReturnCode.PMFalse
            'UPGRADE_NOTE: Object m_oNavStart may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            m_oNavStart = Nothing
            Exit Function
        End If

        ' Wait for the process to finish
        While (m_bNavClosed = False)
            System.Windows.Forms.Application.DoEvents()
        End While

        ' Back to normal...
        ' The return status of Navigator is checked in m_oNavStart_SetProcessStatus

        ' Remove the instance
        'm_lReturn = m_oNavStart.Terminate()
        m_oNavStart.Dispose()

        'UPGRADE_NOTE: Object m_oNavStart may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        m_oNavStart = Nothing


    End Function

    Public Sub New()
        MyBase.New()

    End Sub

    Private Sub m_oNavStart_NavigatorClose() Handles m_oNavStart.NavigatorClose
        m_bNavClosed = True
    End Sub
End Class