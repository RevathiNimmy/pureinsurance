Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no 129. 
Imports SharedFiles

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' Property members
    Private m_sCallingAppName As String = ""
    Private m_lErrorNumber As Integer
    Private m_lStatus As Integer
    Private m_lNavigate As Integer
    Private m_sNavigatorTitle As String = ""
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lReturn As Integer
    Private m_vSourceArray(,) As Object
    Private m_sSourceCode As String = ""
    Private m_sSourceName As String = ""
    Private m_iSourceID As Integer
    Private m_iCountryID As Integer
    'TN20010706 start
    Private m_lPartyCnt As Integer
    'TN20010706 end

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)
    Private m_lProductId As Integer

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property
    Public ReadOnly Property NavigatorTitle() As String
        Get

            ' Return the objects parameter value.
            Return m_sNavigatorTitle

        End Get
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    'developer guide no.17
    Public WriteOnly Property SourceArray() As Object
        Set(ByVal Value As Object)

            ' Set the valid sources for the user
            m_vSourceArray = Value

        End Set
    End Property
    Public Property SourceID() As Integer
        Get

            ' Set the valid sources for the user
            Return m_iSourceID

        End Get
        Set(ByVal Value As Integer)

            ' Set the valid sources for the user
            m_iSourceID = Value

        End Set
    End Property
    Public Property SourceCode() As String
        Get

            ' Set the valid source code the user
            Return m_sSourceCode

        End Get
        Set(ByVal Value As String)

            ' Set the valid sources code
            m_sSourceCode = Value

        End Set
    End Property
    Public Property SourceName() As String
        Get

            ' Set the valid sources for the user
            Return m_sSourceName

        End Get
        Set(ByVal Value As String)

            ' Set the valid sources for the user
            m_sSourceName = Value

        End Set
    End Property
    Public ReadOnly Property CountryID() As Integer
        Get

            ' Set the valid country for the user
            Return m_iCountryID

        End Get
    End Property

    'TN20010706 start
    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    'TN20010706 end
    Public Property ProductId() As Integer
        Get
            ' Set the valid sources for the user
            Return m_lProductId
        End Get
        Set(ByVal Value As Integer)
            ' Set the valid sources for the user
            m_lProductId = Value
        End Set
    End Property

    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Updates all interface details from the search data storage.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataToInterface() As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim nCount As Integer
        Try

            cboBranches.Items.Clear()

            For i As Integer = m_vSourceArray.GetLowerBound(1) To m_vSourceArray.GetUpperBound(1)
                'Load options from Source details
                Dim cboBranchesNewIndex As Integer = -1
                If (Not m_vSourceArray(2, i) Is Nothing) Then
                    cboBranchesNewIndex = cboBranches.Items.Add(m_vSourceArray(2, i).ToString.Trim)
                    VB6.SetItemData(cboBranches, cboBranchesNewIndex, CInt(m_vSourceArray(0, i)))
                End If
            Next i
            If m_lPartyCnt <> 0 Then
                m_lReturn = DefaultPartySourceId(v_lPartyCnt:=m_lPartyCnt)
            Else
                For nCount = 0 To cboBranches.Items.Count - 1
                    If cboBranches.Items(nCount).ItemData = g_iSourceID Then
                        cboBranches.SelectedIndex = nCount
                        Exit For
                    End If
                Next
            End If
            'TN200100607 end

            Return nResult

        Catch excep As System.Exception
            ' Error Section.

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        m_lReturn = ValidateOK()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        m_iSourceID = VB6.GetItemData(cboBranches, cboBranches.SelectedIndex)
        'developer guide no. 162
        For i As Integer = m_vSourceArray.GetLowerBound(1) To m_vSourceArray.GetUpperBound(1)
            If CInt(m_vSourceArray(0, i)) = m_iSourceID Then
                m_sSourceCode = CStr(m_vSourceArray(1, i))
                m_sSourceName = CStr(m_vSourceArray(2, i))
                'As m_vSourceArray is coming through property we are not confirm
                'of that there will be always country_id provided
                'DM 11082006
                If m_vSourceArray.GetUpperBound(0) + 1 > 3 Then
                    m_iCountryID = gPMFunctions.ToSafeInteger(CStr(m_vSourceArray(3, i)))
                End If
                Exit For
            End If
        Next i
        Me.Close()
    End Sub

    Private Sub Form_Initialize_Renamed()
        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        m_lReturn = DataToInterface()

    End Sub


    '***************************************************************************
    ' Name : DefaultPartySourceId
    '
    ' Desc : get source_id for party_cnt and default combo with it
    '
    ' Hist : 07 June 2001 Tinny - Created
    '***************************************************************************
    Private Function DefaultPartySourceId(ByVal v_lPartyCnt As Integer) As Integer
        Dim result As Integer = 0

        Dim oObject As bSirParty.Services
        Dim lSourceID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oObject As Object = Nothing
            'developer guide no. 218
            result = g_oObjectManager.GetInstance(temp_oObject, "bSIRParty.Services", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oObject = temp_oObject

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            oObject.PartyCnt = v_lPartyCnt

            result = oObject.GetDetails()

            If result = gPMConstants.PMEReturnCode.PMTrue Then

                lSourceID = oObject.SourceID

                For lCount As Integer = 0 To cboBranches.Items.Count - 1
                    If VB6.GetItemData(cboBranches, lCount) = lSourceID Then
                        cboBranches.SelectedIndex = lCount
                        Exit For
                    End If
                Next
            End If


		oObject.Dispose()

            oObject = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DefaultPartySourceId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DefaultPartySourceId", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function ValidateOK() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If cboBranches.SelectedIndex = -1 Then
                MessageBox.Show("Please select Branch" & Strings.Chr(9), "Invalid branch", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If cboBranches.Enabled Then
                    cboBranches.Focus()
                End If
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOKFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub tabMain_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabMain.KeyDown
        If e.Alt And e.KeyCode = Keys.B Then
            tabMain.SelectedIndex = 0
            tabMain.Focus()
        End If
    End Sub
End Class
