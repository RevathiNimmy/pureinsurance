Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Friend Partial Class frmRiskGroups
	Inherits System.Windows.Forms.Form
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_vRiskgroupId As Object
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	Private m_vRiskGroupDetails( ,  ) As Object
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Private m_lErrorNumber As Integer
	
	Public Property RiskGroupId() As Object
		Get
			Return m_vRiskgroupId
		End Get
		Set(ByVal Value As Object)

            m_vRiskgroupId = Value
        End Set
    End Property

    Public Property LookupValues() As Object
        Get
            Return VB6.CopyArray(m_vLookupValues)
        End Get
        Set(ByVal Value As Object)
            m_vLookupValues = Value
        End Set
    End Property
    Public Property LookupDetails() As Object
        Get
            Return VB6.CopyArray(m_vLookupDetails)
        End Get
        Set(ByVal Value As Object)
            m_vLookupDetails = Value
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

    ' ***************************************************************** '
    ' Name: LoadRiskGroups
    '
    ' Description: Load Risk Group Array
    '
    ' ***************************************************************** '
    Public Function LoadRiskGroups() As Integer
        Dim result As Integer = 0
        Dim ACClass As Object
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        'Const ACValueID As Integer = 1         '' Unused Local Variable
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}
            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If (CStr(m_vLookupValues(ACValueTableName, lRow)).Trim()) = gSIRLibrary.SIRLookupRiskGroup Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Risk Group Details", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRiskGroups")

                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.
                If Not Information.IsArray(m_vRiskGroupDetails) Then
                    ReDim m_vRiskGroupDetails(2, 0)
                Else
                    ReDim Preserve m_vRiskGroupDetails(2, m_vRiskGroupDetails.GetUpperBound(1) + 1)

                End If

                m_vRiskGroupDetails(0, m_vRiskGroupDetails.GetUpperBound(1)) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
                m_vRiskGroupDetails(1, m_vRiskGroupDetails.GetUpperBound(1)) = m_vLookupDetails(ACDetailDesc, lCntr)
                m_vRiskGroupDetails(2, m_vRiskGroupDetails.GetUpperBound(1)) = False
            Next lCntr

            If Information.IsArray(m_vRiskgroupId) Then

                For lCntr2 As Integer = m_vRiskgroupId.GetLowerBound(0) To m_vRiskgroupId.GetUpperBound(0)
                    For lCntr As Integer = 0 To m_vRiskGroupDetails.GetUpperBound(1)

                        If CDbl(m_vRiskGroupDetails(0, lCntr)) = CInt(m_vRiskgroupId(lCntr2)) Then
                            m_vRiskGroupDetails(2, lCntr) = True
                            Exit For
                        End If
                    Next lCntr
                Next lCntr2
            End If
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the risk groups", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRiskGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayRiskGroups
    '
    ' Description: Displays Risk Groups
    '
    ' ***************************************************************** '
    Public Function DisplayRiskGroups() As Integer
        Dim result As Integer = 0
        Dim ACClass As Object
        Dim oListItem As ListViewItem
        Dim olist As ListView

        Dim oListAvailable, oListSelected As ListView
        Dim lItems As Integer

        Const ACRiskImage As String = "RiskImage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(ListViewFunc.ListViewBatchStart(lvwList:=lvwAvailable), gPMConstants.PMEReturnCode)
            oListAvailable = lvwAvailable
            m_lReturn = CType(ListViewFunc.ListViewBatchStart(lvwList:=lvwSelected), gPMConstants.PMEReturnCode)
            oListSelected = lvwSelected

            oListAvailable.Items.Clear()
            oListSelected.Items.Clear()

            lItems = m_vRiskGroupDetails.GetUpperBound(1)

            ' Assign the details to the interface.
            For lRow As Integer = m_vRiskGroupDetails.GetLowerBound(1) To lItems

                ' {* USER DEFINED CODE (Begin) *}
                If Not CBool(m_vRiskGroupDetails(2, lRow)) Then
                    olist = oListAvailable
                Else
                    olist = oListSelected
                End If

                oListItem = olist.Items.Add(CStr(m_vRiskGroupDetails(1, lRow)).Trim(), ACRiskImage)

                With oListItem

                    .Tag = CStr(m_vRiskGroupDetails(0, lRow))
                End With

            Next lRow
            If lvwAvailable.Items.Count > 0 Or lvwSelected.Items.Count > 0 Then
                m_lReturn = CType(ListViewFunc.ListViewBatchEnd(), gPMConstants.PMEReturnCode)
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the risk groups", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayRiskGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetRiskGroups
    '
    ' Description: Get Updated Selected Risk Groups
    '
    ' ***************************************************************** '
    Public Function GetRiskGroups() As Integer
        Dim result As Integer = 0
        Dim ACClass As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_vRiskgroupId = Nothing

            For lRow As Integer = 0 To m_vRiskGroupDetails.GetUpperBound(1)

                If CBool(m_vRiskGroupDetails(2, lRow)) Then

                    If Not Information.IsArray(m_vRiskgroupId) Then
                        ReDim m_vRiskgroupId(0)
                    Else

                        ReDim Preserve m_vRiskgroupId(m_vRiskgroupId.GetUpperBound(0) + 1)
                    End If

                    m_vRiskgroupId(m_vRiskgroupId.GetUpperBound(0)) = CInt(m_vRiskGroupDetails(0, lRow))

                End If
            Next lRow

            Return result

        Catch excep As System.Exception

            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the risk groups", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddAllGroups
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function AddAllGroups() As Integer
        Dim result As Integer = 0
        Dim ACClass As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iRow As Integer = 1 To lvwAvailable.Items.Count
                m_lReturn = CType(AddGroup(v_lRiskGroupid:=Convert.ToString(lvwAvailable.Items.Item(iRow - 1).Tag)), gPMConstants.PMEReturnCode)
            Next iRow

            m_lReturn = CType(DisplayRiskGroups(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAllGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAllGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: AddGroups
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function AddGroups() As Integer
        Dim result As Integer = 0
        Dim ACClass As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iRow As Integer = 1 To lvwAvailable.Items.Count
                If lvwAvailable.Items.Item(iRow - 1).Selected Then
                    m_lReturn = CType(AddGroup(v_lRiskGroupid:=Convert.ToString(lvwAvailable.Items.Item(iRow - 1).Tag)), gPMConstants.PMEReturnCode)
                End If

            Next iRow

            m_lReturn = CType(DisplayRiskGroups(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: AddGroup
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function AddGroup(ByRef v_lRiskGroupid As Object) As Integer
        Dim result As Integer = 0
        Dim ACClass As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iRow As Integer = 0 To m_vRiskGroupDetails.GetUpperBound(1)

                If CInt(v_lRiskGroupid) = CInt(m_vRiskGroupDetails(0, iRow)) Then
                    m_vRiskGroupDetails(2, iRow) = True
                End If

            Next iRow

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: RemoveAllGroups
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function RemoveAllGroups() As Integer
        Dim result As Integer = 0
        Dim ACClass As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iRow As Integer = 1 To lvwSelected.Items.Count
                m_lReturn = CType(RemoveGroup(v_lRiskGroupid:=Convert.ToString(lvwSelected.Items.Item(iRow - 1).Tag)), gPMConstants.PMEReturnCode)
            Next iRow

            m_lReturn = CType(DisplayRiskGroups(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveAllGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveAllGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: RemoveGroups
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function RemoveGroups() As Integer
        Dim result As Integer = 0
        Dim ACClass As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iRow As Integer = 1 To lvwSelected.Items.Count
                If lvwSelected.Items.Item(iRow - 1).Selected Then
                    m_lReturn = CType(RemoveGroup(v_lRiskGroupid:=Convert.ToString(lvwSelected.Items.Item(iRow - 1).Tag)), gPMConstants.PMEReturnCode)
                End If

            Next iRow

            m_lReturn = CType(DisplayRiskGroups(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: RemoveGroup
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function RemoveGroup(ByRef v_lRiskGroupid As Object) As Integer
        Dim result As Integer = 0
        Dim ACClass As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iRow As Integer = 0 To m_vRiskGroupDetails.GetUpperBound(1)

                If CInt(v_lRiskGroupid) = CInt(m_vRiskGroupDetails(0, iRow)) Then
                    m_vRiskGroupDetails(2, iRow) = False
                End If
            Next iRow

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        m_lReturn = AddGroups()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Interaction.MsgBox("Failed to select Risk Groups", "Error")
        End If

    End Sub

    Private Sub cmdAddAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAll.Click

        m_lReturn = AddAllGroups()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Interaction.MsgBox("Failed to select Risk Groups", "Error")
        End If

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Me.Hide()

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        m_lReturn = GetRiskGroups()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Interaction.MsgBox("Failed to udate Selected Risk Groups", "Error")
        End If

        Me.Hide()

    End Sub

    Private Sub cmdRemove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemove.Click

        m_lReturn = RemoveGroups()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Interaction.MsgBox("Failed to deselect Risk Groups", "Error")
        End If
    End Sub

    Private Sub cmdRemoveAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemoveAll.Click

        m_lReturn = RemoveAllGroups()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Interaction.MsgBox("Failed to deselect Risk Groups", "Error")
        End If

    End Sub

    Private Sub frmRiskGroups_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Dim ACClass As Object

        ' Forms load event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = CType(LoadRiskGroups(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_lReturn = CType(DisplayRiskGroups(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            iPMFunc.CenterForm(Me)

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
End Class
