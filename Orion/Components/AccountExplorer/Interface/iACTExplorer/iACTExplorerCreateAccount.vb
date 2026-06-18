Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Imports Artinsoft.VB6.Utils

Partial Friend Class frmCreateAccount
    Inherits System.Windows.Forms.Form
    ' History:
    ' CJB 070405 PN14472 Changed SetInterfaceDefaults to set new ShowEditOnFindAccount property on
    '            uctAccountLookup to True as this is the only place where we want the Edit button
    '            shown on Find Account.
    '

    Private Const ACClass As String = "frmCreateAccount"

    ' Public properties
    Public Code As String = ""
    Public AccName As String = ""
    Public FullPath As String = ""
    Public AccountType As Integer
    Public Description As String = ""
    Public TotallingID As Integer
    Public ReportMapID As Integer
    Public AccountMapID As Integer
    Public ExtrasOnly As Boolean
    Public CompanyID As Integer 'DN 06/12/02
    Public SubBranchID As Integer 'DN 06/12/02
    Public Company As String = "" 'DN 06/12/02
    Public SubBranch As String = "" 'DN 06/12/02
    Public LedgerID As Integer 'eck PN5946 110803
    ' Property members
    Private m_bResult As Boolean
    Private m_lProcessMode As Integer
    Private m_oBusiness As bACTExplorer.Form

    'eck PN5946 110803
    Private m_iLedgerId As Integer

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    'developer guide no. 33
    Private m_vLookupDetails As Object
    Private m_lReturn As gPMConstants.PMEReturnCode

    Public WriteOnly Property Business() As bACTExplorer.Form
        Set(ByVal Value As bACTExplorer.Form)
            ' Set the business object
            m_oBusiness = Value
        End Set
    End Property

    Public ReadOnly Property Result() As Boolean
        Get
            ' Return the dialog result
            Return m_bResult
        End Get
    End Property
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    'EK130199 Bug 190
    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    ' ***************************************************************** '
    Public Function SetInterfaceDefaults() As Integer
        Dim result As Integer = 0
        Dim sUnderwritingOrAgency As String = ""
        Dim bShowSubBranchID As Boolean
        Dim sValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Quick check of company
            If CompanyID = 0 Then
                CompanyID = g_iSourceID
            End If

            ' Center the interface.
            iPMFunc.CenterForm(Me)
            txtCode.Text = Code
            txtAccName.Text = AccName
            lblFullPath.Text = FullPath

            'DN 06/12/02 - Build and populate the branch dropdown
            m_lReturn = BuildBranch()

            If cboBranch.SelectedIndex >= 0 Then
                CompanyID = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)
            Else
                CompanyID = g_iSourceID
            End If


            ' Build and populate the sub_branch dropdown
            m_lReturn = CType(BuildSubBranch(CompanyID), gPMConstants.PMEReturnCode)

            'eck PN5945 110803

            If Not Information.IsNothing(SubBranchID) Then
                cboSubBranch.SelectedValue = SubBranchID
            End If


            m_lReturn = CType(GetLedgerDetails(ctlLookup:=cboLedgerID), gPMConstants.PMEReturnCode)


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Show the Edit button on the Find Account screen  PN14472
            uctAccountLookup.ShowEditOnFindAccount = True

            'EK 220300
            If ExtrasOnly Then
                Me.Text = "Account Extras"
                cboBranch.Enabled = False
                cboSubBranch.Enabled = False
                txtDescription.Text = Description
                txtCode.Enabled = False
                txtAccName.Enabled = False
                uctAccountType.Enabled = False
                uctAccountType.ItemId = AccountType
                uctAccountLookup.AccountId = AccountMapID
                cboLedgerID.Enabled = False
            Else
                Me.Text = "Create Account"

                'DJM 29/07/2003 : Default the account type combo depending on whichever folder you are in.

                If FullPath.ToUpper().IndexOf("INCOME") >= 0 Then
                    uctAccountType.ItemId = 1
                ElseIf FullPath.ToUpper().IndexOf("EXPENSE") >= 0 Or FullPath.ToUpper().IndexOf("EXPENDITURE") >= 0 Then
                    uctAccountType.ItemId = 2
                ElseIf FullPath.ToUpper().IndexOf("ASSET") >= 0 Then
                    uctAccountType.ItemId = 3
                ElseIf FullPath.ToUpper().IndexOf("LIABILITY") >= 0 Or FullPath.ToUpper().IndexOf("LIABILITIES") >= 0 Then
                    uctAccountType.ItemId = 4
                ElseIf FullPath.ToUpper().IndexOf("SUSPENSE") >= 0 Then
                    uctAccountType.ItemId = 5
                Else
                    uctAccountType.ItemId = 3
                End If
            End If

            'Temp for Now until totalling table is introduced
            cboTotallingType.Items.Add("Sub Account")
            cboTotallingType.Items.Add("Heading Account")
            cboTotallingType.Items.Add("Prime Account")
            cboTotallingType.Items.Add("Grouped Account")
            cboTotallingType.SelectedIndex = TotallingID

            iPMFunc.getUnderwritingOrAgency(sUnderwritingOrAgency)

            'Broking doesn't show Sub Branch by default
            iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTSubBranchShowingForBroking, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue)

            bShowSubBranchID = True

            lblSubBranch.Visible = bShowSubBranchID
            cboSubBranch.Visible = bShowSubBranchID

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=Nothing, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'EK 220300
    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.
            m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get all of the lookup details.

            'Department
            'MKW090603 PN4574 Changed to use Table CostCentre instead of Department.
            m_lReturn = CType(GetLookupDetails(sLookupTable:=gACTLibrary.ACTLookupCostCentre, ctlLookup:=cboReportMapId), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:="FrmCreateAccount", vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'eck PN5946 110803
    ' ***************************************************************** '
    ' Name: GetLedgerDetails
    '
    ' Description: Gets ledger details for this company
    '              then assigns them to the list or combo control passed.
    '
    ' ***************************************************************** '
    'Developer Guide No 153
    Private Function GetLedgerDetails(ByRef ctlLookup As ComboBox) As Integer

        ' Constants for ledger details

        Dim result As Integer = 0
        Const CLedgerID As Integer = 0
        Const CLedgerName As Integer = 1
        Dim vLedgerDetails As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get array containing list of ledger dets

            m_lReturn = m_oBusiness.GetLedgerDetails(vResultArray:=vLedgerDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If



            'Developer guide No 153
            Dim listIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem("", 0))



            For lLedgerRow As Integer = 1 To vLedgerDetails.GetUpperBound(0)


                'Developer Guide No 153
                Dim listIndexTemp As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(vLedgerDetails(lLedgerRow, CLedgerName), CInt(vLedgerDetails(lLedgerRow, CLedgerID))))

                ' Check if this is the selected index.


                If CInt(vLedgerDetails(lLedgerRow, CLedgerID)) = LedgerID Then


                    'Developer Guide No 153
                    ctlLookup.SelectedIndex = listIndexTemp


                End If

            Next lLedgerRow


            'Developer Guide No 153
            If ctlLookup.SelectedIndex = -1 Then

                'Developer Guide No 153
                ctlLookup.SelectedIndex = 0


            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get all of the lookup values.

            m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails, vReportMapId:=ReportMapID)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:="frmCreateAccount", vMethod:="GetLookupValues")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:="frmCreateAccount", vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    'Developer Guide No 153
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox, Optional ByRef bSecondary As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lRow, lRow2 As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
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
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:="frmCreateAccount", vMethod:="GetLookupDetails")

                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.
            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.

                'Developer Guide No 153
                Dim listIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr), CInt(m_vLookupDetails(ACDetailKey, lCntr))))




                'Developer Guide No 153
                ' Check if this is the selected index.
                If bSecondary Then
                    lRow2 = lRow + 1
                Else
                    lRow2 = lRow
                End If
                If CStr(m_vLookupValues(ACValueID, lRow2)) <> "" Then
                    If CDbl(m_vLookupValues(ACValueID, lRow2)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


                        'Developer Guide No 153
                        ctlLookup.SelectedIndex = listIndex


                    End If

                End If

            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            ' No - to minus 1...
            If CStr(m_vLookupValues(ACValueID, lRow2)) = "" Then


                'Developer Guide No 153
                ctlLookup.SelectedIndex = -1

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:="frmCreateAccount", vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DN 06/12/02
    Private Sub cboBranch_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranch.SelectedIndexChanged
        CompanyID = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)
        m_lReturn = CType(BuildSubBranch(CompanyID), gPMConstants.PMEReturnCode)
    End Sub
    Private Sub cboLedgerID_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboLedgerID.Leave
        LedgerID = VB6.GetItemData(cboLedgerID, cboLedgerID.SelectedIndex)
    End Sub



    Private isInitializingComponent As Boolean
    Private Sub cboReportMapId_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboReportMapId.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_lReturn = CType(PMBGeneralFunc.FieldOnControlChange(Me), gPMConstants.PMEReturnCode)
    End Sub

    Private Sub cboReportMapId_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboReportMapId.Enter
        m_lReturn = CType(PMBGeneralFunc.ControlGotFocus(cboReportMapId), gPMConstants.PMEReturnCode)
    End Sub

    Private Sub cboReportMapId_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboReportMapId.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        m_lReturn = CType(PMBGeneralFunc.FieldOnControlChange(Me, KeyCode, Shift), gPMConstants.PMEReturnCode)
    End Sub

    Private Sub cboReportMapId_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboReportMapId.Leave
        m_lReturn = CType(PMBGeneralFunc.ControlLostFocus(cboReportMapId), gPMConstants.PMEReturnCode)
    End Sub

    Private Sub cboTotallingType_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTotallingType.Leave
        TotallingID = cboTotallingType.SelectedIndex
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_bResult = False
        'Me.Close()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim bComplete As Boolean

        Try

            m_bResult = True

            m_lReturn = CType(ValidateMandatory(r_bComplete:=bComplete), gPMConstants.PMEReturnCode)

            If bComplete Then
                'Developer Guide No 231
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdOK_Click failed", vApp:=ACApp, vClass:="frmCreateAccount", vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    'TODO:MILAN::
    Private Sub frmCreateAccount_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        'If m_bResult Then
        'Code = txtCode.Text
        ''DN 06/12/02
        'CompanyID = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)
        'SubBranchID = VB6.GetItemData(cboSubBranch, cboSubBranch.SelectedIndex)
        'AccName = txtAccName.Text
        'FullPath = lblFullPath.Text
        'AccountType = uctAccountType.ItemId
        ''eck PN5046 110803
        'LedgerID = VB6.GetItemData(cboLedgerID, cboLedgerID.SelectedIndex)
        ''EK220300
        'TotallingID = cboTotallingType.SelectedIndex
        'Description = txtDescription.Text
        'If cboReportMapId.SelectedIndex = -1 Then
        '    ReportMapID = 0
        'Else
        '    ReportMapID = VB6.GetItemData(cboReportMapId, cboReportMapId.SelectedIndex)
        'End If
        'AccountMapID = uctAccountLookup.AccountId
        'End If
    End Sub

    Private Sub txtAccName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccName.Enter
        txtAccName.SelectionStart = 0
        txtAccName.SelectionLength = Strings.Len(txtAccName.Text)
    End Sub

    Private Sub txtCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Enter
        txtCode.SelectionStart = 0
        txtCode.SelectionLength = Strings.Len(txtCode.Text)
    End Sub

    Private Function BuildBranch() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: BuildBranch
        ' PURPOSE: Populate branch control and select current or default
        ' AUTHOR: David Newson
        ' DATE: 06-Dec-02, 02:20 PM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vBranchArray(,) As Object
        Dim lLower, lUpper, lCurrentBranch, lDefaultBranch As Integer


        Try

        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = m_oBusiness.GetBranches(r_vBranchArray:=vBranchArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check for branch array
        If Information.IsArray(vBranchArray) Then
            ' Walk the array

            lLower = vBranchArray.GetLowerBound(1)

            lUpper = vBranchArray.GetUpperBound(1)

            lCurrentBranch = -1
            lDefaultBranch = -1

            For lCount As Integer = lLower To lUpper
                'PN #29745 check to filter deleted branches

                Dim cboBranch_NewIndex As Integer = -1
                If CDbl(vBranchArray(5, lCount)) <> 1 Then

                    cboBranch_NewIndex = cboBranch.Items.Add(CStr(vBranchArray(2, lCount)))

                    VB6.SetItemData(cboBranch, cboBranch_NewIndex, CInt(vBranchArray(0, lCount)))
                End If

                'MKW150703 PN5367 START -Get ListIndex of the selected branch

                If CompanyID = CDbl(vBranchArray(0, lCount)) Then
                    lCurrentBranch = cboBranch_NewIndex
                ElseIf g_iSourceID = CDbl(vBranchArray(0, lCount)) Then
                    lDefaultBranch = cboBranch_NewIndex
                End If
                'MKW150703 PN5367 END

            Next lCount

            ' Set default item to selected branch.
            If lCurrentBranch >= 0 Then
                cboBranch.SelectedIndex = lCurrentBranch
            ElseIf lDefaultBranch >= 0 Then
                cboBranch.SelectedIndex = lDefaultBranch
            Else
                cboBranch.SelectedIndex = 0
            End If

        Else
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        Catch ex As Exception
        Select Case Information.Err().Number
            Case Else
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildBranch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                result = gPMConstants.PMEReturnCode.PMFalse


        End Select

        Finally


        End Try
        Return result
    End Function

    Private Function BuildSubBranch(ByRef lCompanyID As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: BuildSubBranch
        ' PURPOSE: Populate sub_branch control and select current or default
        ' AUTHOR: Peter Finney
        ' DATE: 09-Oct-02, 04:26 PM
        ' RETURNS: PMTrue for success
        ' CHANGES: Pass in compay ID
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vSubBranchArray(,) As Object
        Dim lLower, lUpper, lCurrent As Integer


        Try

        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = m_oBusiness.GetSubBranches(r_vSubBranchArray:=vSubBranchArray, v_vCompanyId:=lCompanyID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check for sub_branch array
        If Information.IsArray(vSubBranchArray) Then

            'DN 06/12/02
            cboSubBranch.Items.Clear()

            ' Walk the array

            lLower = vSubBranchArray.GetLowerBound(1)

            lUpper = vSubBranchArray.GetUpperBound(1)

            For lCount As Integer = lLower To lUpper
                Dim cboSubBranch_NewIndex As Integer = -1

                cboSubBranch_NewIndex = cboSubBranch.Items.Add(CStr(vSubBranchArray(3, lCount)))

                VB6.SetItemData(cboSubBranch, cboSubBranch_NewIndex, CInt(vSubBranchArray(0, lCount)))

                If SubBranchID = CDbl(vSubBranchArray(0, lCount)) Then
                    lCurrent = cboSubBranch_NewIndex
                End If
            Next lCount

            ' Set default item (if none found default value of 0 will be first item)
            cboSubBranch.SelectedIndex = lCurrent
        Else
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If



        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildSubBranch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function ValidateMandatory(ByRef r_bComplete As Boolean) As Integer

        ' Variables for Displaying validation errors.
        Dim result As Integer = 0
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_bComplete = False
            If cboBranch.SelectedIndex = -1 Then
                ' Get description from the resource file.

                'Developer Guide no 76
                'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingMandatoryFieldsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingMandatoryFieldsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show("Please select Branch", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cboBranch.Enabled Then
                    cboBranch.Focus()
                End If
                Return result
            End If

            If Strings.Len(txtCode.Text) = 0 Then
                ' Get description from the resource file.

                'Developer Guide no 76
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingMandatoryFieldsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingShortCodeText, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                txtCode.Focus()

                Return result
            End If

            'DC220703 -ISS5503 -start -make name mandatory
            If Strings.Len(txtAccName.Text) = 0 Then
                ' Get description from the resource file.
                'Developer Guide No 76
                'Starts
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingMandatoryFieldsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingAccountNameText, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                'Ends
                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                txtAccName.Focus()

                Return result 'MKW290104 PN10073
            End If
            'eck PN5946 110803
            If cboLedgerID.SelectedIndex = 0 Then
                ' Get description from the resource file.

                'Developer Guide No 76
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingMandatoryFieldsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingMandatoryFieldsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                cboLedgerID.Focus()


                Return result
            End If
            'DC220703 -ISS5503 -end

            r_bComplete = True

            Return result

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateMandatory failed", vApp:=ACApp, vClass:="frmCreateAccount", vMethod:="ValidateMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub frmCreateAccount_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If m_bResult Then
            Code = txtCode.Text
            'DN 06/12/02
            CompanyID = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)
            SubBranchID = VB6.GetItemData(cboSubBranch, cboSubBranch.SelectedIndex)
            AccName = txtAccName.Text
            FullPath = lblFullPath.Text
            AccountType = uctAccountType.ItemId
            'eck PN5046 110803
            LedgerID = VB6.GetItemData(cboLedgerID, cboLedgerID.SelectedIndex)
            'EK220300
            TotallingID = cboTotallingType.SelectedIndex
            Description = txtDescription.Text
            If cboReportMapId.SelectedIndex = -1 Then
                ReportMapID = 0
            Else
                ReportMapID = VB6.GetItemData(cboReportMapId, cboReportMapId.SelectedIndex)
            End If
            AccountMapID = uctAccountLookup.AccountId
        End If
    End Sub
End Class
