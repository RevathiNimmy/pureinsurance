Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports Artinsoft.VB6.Utils
'Developer Guide No. 129
Imports SharedFiles
Friend Partial Class frmRiskGroup
	Inherits System.Windows.Forms.Form
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_vRiskgroupId As Integer
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Private m_lErrorNumber As Integer
	
	Public Property RiskGroupId() As Integer
		Get
			Return m_vRiskgroupId
		End Get
		Set(ByVal Value As Integer)

			m_vRiskgroupId = CInt(Value)
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
	' Name: DisplayLookupDetails
	'
	' Description: Displays all of the lookup details using the lookup
	'              values/details.
	'
	' ***************************************************************** '
	Public Function DisplayLookupDetails() As Integer
		Dim result As Integer = 0
		Dim ACClass As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupRiskGroup, ctlLookup:=cboRiskGroup), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
        Dim result As Integer = 0
        Dim ACClass As Object
        Dim ctlLookupIndex As Byte

        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        'Const ACValueID As Integer = 1             '' Unused Local Variable
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            ctlLookupIndex = 0

            'ctlLookup.AddItem("")
            ReflectionHelper.Invoke(ctlLookup, "AddItem", New Object() {""})

            'ctlLookup.ItemData(ctlLookup.NewIndex) = 0
            ReflectionHelper.SetMember(ctlLookup, "ItemData", New Object() {ReflectionHelper.GetMember(ctlLookup, "NewIndex")}, 0)

            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.

                'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))
                ReflectionHelper.Invoke(ctlLookup, "AddItem", New Object() {m_vLookupDetails(ACDetailDesc, lCntr)})

                'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
                ReflectionHelper.SetMember(ctlLookup, "ItemData", New Object() {ReflectionHelper.GetMember(ctlLookup, "NewIndex")}, CInt(m_vLookupDetails(ACDetailKey, lCntr)))

                ' Check if this is the selected index.
                Dim dbNumericTemp As Double
                If Double.TryParse(CStr(m_vRiskgroupId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    If m_vRiskgroupId = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then

                        'ctlLookup.ListIndex = ctlLookup.NewIndex
                        ReflectionHelper.SetMember(ctlLookup, "ListIndex", ReflectionHelper.GetMember(ctlLookup, "NewIndex"))
                    End If
                End If
            Next lCntr

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub Cancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Cancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMOK
        If cboRiskGroup.SelectedIndex <> -1 Then
            m_vRiskgroupId = VB6.GetItemData(cboRiskGroup, cboRiskGroup.SelectedIndex)
        End If
        Me.Hide()
    End Sub

    Private Sub frmRiskGroup_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Dim ACClass As Object

        ' Forms load event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = CType(DisplayLookupDetails(), gPMConstants.PMEReturnCode)

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
