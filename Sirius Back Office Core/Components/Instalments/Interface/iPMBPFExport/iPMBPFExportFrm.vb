Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'refer Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sStepStatus As String = ""
    Private m_bIsUsingDocRef As Boolean = False

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBPFExport.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    Private m_oExport As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lScreenType As Integer '0 - export, 1 - post, 2 - recall, 3 - payment
    Private m_lPartyCnt As Integer
    Private m_dtDueDate As Date
    Private m_lBatchNo As Integer
    Private m_vInstalments(,) As Object
    Private m_vInstalmentsStatus(,) As Object
    Private m_vInstalmentsModify() As Object 'instalments which has its status changed
    Private m_vInstalmentstoRecall As ArrayList

    ' {* USER DEFINED CODE (End) *}

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property


    Public Property Task() As Integer
        Get

            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            m_lNavigate = Value

        End Set
    End Property


    Public Property StepStatus() As String
        Get

            Return m_sStepStatus

        End Get
        Set(ByVal Value As String)

            m_sStepStatus = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            m_dtEffectiveDate = Value

        End Set
    End Property

    'User Defined Property (Start)
    Public Property ScreenType() As Integer
        Get
            Return m_lScreenType
        End Get
        Set(ByVal Value As Integer)
            m_lScreenType = Value
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
    'User Defined Property (End)

    ' PUBLIC Methods (Begin)

    Public Sub SetItem(ByRef oForm As frmSearch, ByVal v_lPosition As Integer)

        lvwInstalment.FocusedItem = lvwInstalment.Items.Item(v_lPosition - 1)

        lvwInstalment.Items(v_lPosition - 1).Selected = True

        lvwInstalment.Items(v_lPosition - 1).EnsureVisible()

        oForm.Activate()
    End Sub

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the controls to
            ' PMFormControl
            '
            ' Example:-
            '
            '        ' Pass control and required settings to FormControl
            '        m_lReturn = m_oFormFields.AddNewFormField( _
            ''                       ctlControl:=<Control Name>, _
            ''                       lFieldType:=<PM field type>, _
            ''                       lFormat:=<PM format string>, _
            ''                       lMandatory:=<PMMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If (m_lReturn <> PMTrue) Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            'export
            Select Case ScreenType
                Case 0 'export
                    m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLeadDays, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboPaymentMethod, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'mkw130204 PN10447 START
                    m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboMediaType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'mkw130204 PN10447 END

                Case 1, 2 'post/recall
                    m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBatchNo, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

            End Select

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface(Optional ByVal v_lGetData As Integer = gPMConstants.PMEReturnCode.PMTrue) As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lGetData = gPMConstants.PMEReturnCode.PMTrue Then
                If BusinessToData() <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Update the interface details.
            lvwInstalment.Items.Clear()

            'TF291002 - Disable Proceed button if no data present
            If Not Information.IsArray(m_vInstalments) Then
                cmdOK.Enabled = False
            Else
                cmdOK.Enabled = True
                For lCount As Integer = 0 To m_vInstalments.GetUpperBound(1)
                    'column 0
                    oListItem = lvwInstalment.Items.Add(CStr(m_vInstalments(gHUBSpokeConstants.eddPFInstalmentNo, lCount)).Trim())

                    'column 1
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vInstalments(gHUBSpokeConstants.eddPFInstalmentStatusDescription, lCount)).Trim()

                    'column 2
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vInstalments(gHUBSpokeConstants.eddPFAutoGeneratedPlanRef, lCount)).Trim()

                    'column 3
                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vInstalments(gHUBSpokeConstants.eddBankAccountNumber, lCount)).Trim()

                    'column 4
                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vInstalments(gHUBSpokeConstants.eddAccountName, lCount)).Trim()

                    'column 5
                    ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vInstalments(gHUBSpokeConstants.eddBankName, lCount)).Trim()

                    'column 6
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFormatCurrency, vControlValue:=m_vInstalments(gHUBSpokeConstants.eddAmount, lCount))
                    ListViewHelper.GetListViewSubItem(oListItem, 6).Text = txtFormatCurrency.Text

                    oListItem.Tag = CStr(lCount)

                    If lCount = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                        ' Select the first item.
                        lvwInstalment.Items.Item(0).Selected = True

                        ' Refresh the initial results.
                        lvwInstalment.Refresh()
                    End If

                Next
            End If

            If lvwInstalment.Items.Count > 0 Then
                lvwInstalment.Items.Item(0).Selected = True
            End If
            'refer Developer Guide No. 178
            ListViewFunc.ListViewAutoSize(lvwInstalment)

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim lFailedOrion As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update the business object.
            m_lReturn = UpdateStatus()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Select Case ScreenType
                Case 0 'export
                    'mkw130204 PN10447 START
                    If cboMediaType.ListIndex > 0 Then
                        m_lReturn = Export()
                    Else
                        MessageBox.Show("You must select a valid media type?", "Media Type", MessageBoxButtons.OK)
                    End If
                    'mkw130204 PN10447 END

                Case 1 'post

                    m_lReturn = m_oBusiness.PostInstalments(lBatchID:=m_lBatchNo, r_lRecordsPosted:=lRecordCount, r_lFailedPosting:=lFailedOrion)
                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        MessageBox.Show(CStr(lRecordCount) & " instalment(s) have been posted in this Batch to Accounts.", "Post Instalments", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        MessageBox.Show("Failed to post instalments to Accounts.", "Post Instalments", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show(CStr(lRecordCount) & " instalment(s) have been posted in this Batch to Accounts.", "Post Instalments", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        'StatusBar is not showing text on screen 
                        'StatusBar.Text = "Last Posting: " & lRecordCount & New String(" "c, 1) & "instalment(s)"
                        _StatusBar_Panel1.Text = "Last Posting: " & lRecordCount & New String(" "c, 1) & "instalment(s)"
                    End If

                    If lFailedOrion = gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Not all instalments could be posted to Accounts.", "Post Instalments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If

                Case 2 'recall

                    m_lReturn = m_oBusiness.RecallInstalments(lBatchID:=m_lBatchNo, r_lRecordsRecalled:=lRecordCount, r_lFailedRecall:=lFailedOrion, v_sDocumentRef:=txtDocumentRef.Text.Trim(), vInstalmentstoRecall:=m_vInstalmentstoRecall)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        MessageBox.Show("Failed to recall instalments from Account.", "Recall Instalments", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show(CStr(lRecordCount) & " instalment(s) have been recalled in this Batch from Accounts.", "Recall Instalments", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        'StatusBar is not showing text on screen 
                        _StatusBar_Panel1.Text = "Last Recall: " & lRecordCount & New String(" "c, 1) & "instalment(s)"
                    End If

                    If lFailedOrion = gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Not all instalments could be recalled from Accounts.", "Recall Instalments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If

            End Select

            m_lReturn = BusinessToInterface()

            'Clear arraylist
            m_vInstalmentstoRecall.Clear()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Resume

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from business
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Ensure this is cleared
            m_vInstalments = VB6.CopyArray(Nothing)

            Select Case ScreenType
                'DD 16/06/2003: Rewritten for Finance Spoke
                Case 0
                    'mkw130204 PN10447 START
                    If cboMediaType.ListIndex = 0 Then
                        MessageBox.Show("Please Select a Valid Media Type", "Media Type", MessageBoxButtons.OK)
                        Return result
                    End If
                    'mkw130204 PN10447 END


                    m_lReturn = m_oExport.GetInstalmentsForBatch(r_lBatchID:=0, lLeadDays:=m_oFormFields.UnformatControl(txtLeadDays), sMediaTypeCode:=cboMediaType.ItemCode.Trim(), bArrayOnly:=True, bForRecall:=False, r_vResultArray:=m_vInstalments)

                Case 1 'post

                    m_lReturn = m_oExport.GetInstalmentsForBatch(r_lBatchID:=m_oFormFields.UnformatControl(txtBatchNo), lLeadDays:=0, sMediaTypeCode:="", bArrayOnly:=True, bForRecall:=False, r_vResultArray:=m_vInstalments)

                Case 2 'recall

                    m_lReturn = m_oExport.GetInstalmentsForBatch(r_lBatchID:=m_oFormFields.UnformatControl(txtBatchNo), lLeadDays:=0, sMediaTypeCode:="", bArrayOnly:=True, bForRecall:=True, r_vResultArray:=m_vInstalments, v_sDocumentRef:=txtDocumentRef.Text.Trim())

            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BusinessToData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            Select Case ScreenType
                Case 0


                    'Developer Guide No.: To Do
                    Dim dtStartData As Date = ToSafeDate("#30/12/1899#", #12/30/1899#)
                    m_dtDueDate = CDate(DateAdd(DateInterval.Day, gPMFunctions.ToSafeInteger(txtLeadDays.Text), dtStartData))
                    'm_dtDueDate = dtStartData.AddDays(gPMFunctions.ToSafeInteger(txtLeadDays.Text))
                    'm_dtDueDate = m_oFormFields.UnformatControl(txtLeadDays)
                Case 1, 2 'post/recall
                    'm_lBatchNo = CInt(Conversion.Val(txtBatchNo.Text))
                    m_lBatchNo = CLng(Conversion.Val(txtBatchNo.Text))
            End Select

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            lvwInstalment.FullRowSelect = True

            'export
            Select Case ScreenType
                Case 0 'export
                    lblLeadDays.Left = lblBatchNo.Left
                    lblLeadDays.Top = lblBatchNo.Top

                    txtLeadDays.Left = txtBatchNo.Left
                    txtLeadDays.Top = txtBatchNo.Top

                    lblLeadDays.Visible = True
                    txtLeadDays.Visible = True

                    lblMediaType.Visible = True
                    cboMediaType.Visible = True
                    cboMediaType.FirstItem = "Please Select Valid Media Type" 'mkw130204 PN10447

                    lblPaymentMethod.Visible = True
                    cboPaymentMethod.Visible = True

                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtLeadDays, vControlValue:=0)



                    'Developer Guide No.: To Do
                    m_dtDueDate = Date.MinValue

                    m_lReturn = PopulatePaymentMethod()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    txtLeadDays.Text = CStr(0)

                Case 1, 2 'post/recall

                    lblBatchNo.Visible = True
                    txtBatchNo.Visible = True

                    lblBar.Top = lblBatchNo.Top + VB6.TwipsToPixelsY(350)
                    lvwInstalment.Top = lblBar.Top + lblBar.Height
                    lvwInstalment.Height += lblBar.Height

                    txtBatchNo.Text = CStr(0)

                    If ScreenType = 2 Then
                        lblDocumentRef.Visible = True
                        txtDocumentRef.Visible = True
                    End If

            End Select

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ReDim m_ctlTabFirstLast(1, 0)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            Select Case ScreenType
                Case 0, 3 'export/payment
                    m_ctlTabFirstLast(ACControlStart, 0) = txtLeadDays
                Case 1, 2 'post/recall
                    m_ctlTabFirstLast(ACControlStart, 0) = txtBatchNo
            End Select

            m_ctlTabFirstLast(ACControlEnd, 0) = lvwInstalment

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.

            Select Case ScreenType
                Case 0
                    Me.Text = "Instalments Export"
                Case 1
                    Me.Text = "Instalments Post"
                Case 2
                    Me.Text = "Instalments Recall"
                Case 3
                    Me.Text = "Instalments Payment"
            End Select

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If

            Select Case ScreenType
                Case 0 'export

                    'Developer Guide No 243
                    'starts
                    '                 cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACExport, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

                    '	Case 1 'post

                    '		cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPost, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

                    '	Case 2 'Recall

                    '		cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRecall, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

                    '	Case 3 'payment

                    '		cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPayment, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                    'End Select


                    'cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


                    'cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                    cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACExport, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                Case 1 'post

                    cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPost, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                Case 2 'Recall

                    cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRecall, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                Case 3 'payment

                    cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPayment, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End Select


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'Ends
            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.

            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            'Developer Guide No 243
            lblLeadDays.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLeadDays, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No 243
            lblBatchNo.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBatchNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ValidateForm
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function ValidateForm() As Integer

        Dim result As Integer = 0

        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Show help
        'refer Developer Guide No. 184(Latest Guide)
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, MainModule.ScreenHelpID)

    End Sub

    Private Sub cmdRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRefresh.Click

        'PN25431
        If ScreenType = 0 Then
            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtLeadDays.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                MessageBox.Show("The lead days must be a whole number.", "Invalid Lead Days", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtLeadDays.Text = ""
                txtLeadDays.Focus()
                Exit Sub
            End If
        Else
            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(txtBatchNo.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                MessageBox.Show("The reference must be a whole number.", "Invalid Batch Reference", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtBatchNo.Text = ""
                txtBatchNo.Focus()
                Exit Sub
            End If
        End If

        ' Check mandatory controls have been entered into.
        m_lReturn = m_oFormFields.CheckMandatoryControls()

        ' Check for errors
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        m_lReturn = InterfaceToData()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        m_lReturn = BusinessToInterface()

        If txtDocumentRef.Text.Trim() <> String.Empty Then
            m_bIsUsingDocRef = True
        Else
            m_bIsUsingDocRef = False
        End If

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            If ScreenType = 0 Then
                txtLeadDays.Focus()
                txtLeadDays.SelectionLength = Strings.Len(txtBatchNo.Text)
            Else
                txtBatchNo.Focus()
                txtBatchNo.SelectionLength = Strings.Len(txtBatchNo.Text)
            End If
        End If
    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.
        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPFInstalments.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                'Developer Guide No 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                'Developer Guide No 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            Dim temp_m_oExport As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oExport, "bSIRPFExport.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oExport = temp_m_oExport


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                MessageBox.Show("Failed to get an instance of bSIRPFExport", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBPFExport.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            m_vInstalmentstoRecall = New ArrayList()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.
        Try

            iPMFunc.ShowFormInTaskBar_Detach()
            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            'get a list of all instalment status

            m_lReturn = m_oBusiness.GetStatusList(vResultArray:=m_vInstalmentsStatus)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' {* USER DEFINED CODE (End) *}

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            'If UnloadMode <> vbFormCode Then
            '    'Process the next set of actions depending
            '    'upon the interface task etc.
            '    m_lReturn = m_oGeneral.ProcessCommand()

            '    'Check the return value.
            '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '        'Do not procced with the interface termination.
            '        Cancel = 1
            '        eventArgs.cancel = True
            'Set the mouse pointer to normal.
            'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            'Exit Sub
            '    End If
            'End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object


            ' Terminate the form control object.
            m_oFormFields.Dispose()

            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing



            m_oExport.Dispose()

            m_oExport = Nothing

            m_vInstalmentstoRecall = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.
        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            m_lReturn = ValidateForm()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMTrue

            ' Process the next set of actions depending
            ' upon the interface task etc.
            'DD031001 - not needed for this form
            'm_lReturn& = m_oGeneral.ProcessCommand()

            ' Check the return value.
            'DD 09/07/2002: Changed m_lReturn as this was unset.
            If m_lStatus = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwInstalment_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwInstalment.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwInstalment.Columns(eventArgs.Column)
        Static blnLoading As Boolean
        Dim frmSearchForm As frmSearch

        'pkh 30/09/2002 - Added Static variable to fix issue 785
        'If you click on the column it'll load the form but won't
        'acknowledge it until something happens to it (click on it, etc.)
        'Therefore, it'll try to show the form modally again (which it can't)
        'if the user double clicks on the column.  This stops it!
        'Also, amended to explicitly declare and create the form.

        If lvwInstalment.Items.Count < 1 Then
            MessageBox.Show("There are no items to search.", Me.Text)
        Else
            If Not blnLoading Then
                blnLoading = True
                frmSearchForm = New frmSearch()
                frmSearchForm.ShowDialog()
                blnLoading = False
                frmSearchForm = Nothing
            End If
        End If

    End Sub

    Private Sub lvwInstalment_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwInstalment.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        'Developer Guide No.: 74

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        Dim lSelectedItem As Integer

        Dim lvItem As ListViewItem = lvwInstalment.GetItemAt(x, y)

        If lvItem Is Nothing Then
            Exit Sub
        Else

            lvwInstalment.FocusedItem = lvItem


        End If




        'If Button = MouseButtonConstants.RightButton Then
        If eventArgs.Button = Windows.Forms.MouseButtons.Right Then
            'don't show popup menu for control transaction
            lSelectedItem = lvItem.Index + 1 - 1
            If CStr(m_vInstalments(gHUBSpokeConstants.eddPFInstalmentTransStatusID, lSelectedItem)) = "0N" Or CStr(m_vInstalments(gHUBSpokeConstants.eddPFInstalmentTransStatusID, lSelectedItem)) = "0C" Then

                Interaction.Beep()

                Exit Sub
            End If

            m_lReturn = ShowPopUpMenu()
        End If
    End Sub


    Public Sub mnuItem_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        Dim Index As Integer = Array.IndexOf(mnuItem, eventSender)

        Dim bFound As Boolean

        Const StatusID As Integer = 0
        Const StatusCode As Integer = 1
        Const StatusDesc As Integer = 2

        'get the selected instalment

        Dim lSelectedItem As Integer = Convert.ToString(lvwInstalment.Items.Item(lvwInstalment.FocusedItem.Index).Tag)

        'change to selected status
        m_vInstalments(gHUBSpokeConstants.eddPFInstalmentStatusID, lSelectedItem) = m_vInstalmentsStatus(StatusID, CInt(Convert.ToString(mnuItem(Index).Tag)))
        m_vInstalments(gHUBSpokeConstants.eddPFInstalmentStatusDescription, lSelectedItem) = m_vInstalmentsStatus(StatusDesc, CInt(Convert.ToString(mnuItem(Index).Tag)))

        'add instalment id to modify list if not already exist
        If Information.IsArray(m_vInstalmentsModify) Then
            bFound = False
            For Each m_vInstalmentsModify_item As Object In m_vInstalmentsModify

                bFound = (CDbl(m_vInstalmentsModify_item) = lSelectedItem)

                If bFound Then
                    Exit For
                End If
            Next m_vInstalmentsModify_item

            'if not in the list then add it in
            If Not bFound Then
                ReDim Preserve m_vInstalmentsModify(m_vInstalmentsModify.GetUpperBound(0) + 1)
                m_vInstalmentsModify(m_vInstalmentsModify.GetUpperBound(0)) = lSelectedItem
            End If

        Else
            ReDim m_vInstalmentsModify(0)
            m_vInstalmentsModify(0) = lSelectedItem
        End If


        'DD081001 - changed grid directly to avoid a refresh
        'refresh listview
        'm_lReturn = BusinessToInterface(v_lGetData:=PMFalse)
        'refer Developer Guide No. 52
        lvwInstalment.FocusedItem.SubItems(1).Text = m_vInstalmentsStatus(StatusDesc, CInt(Convert.ToString(mnuItem(Index).Tag)))
        If VB6.PixelsToTwipsX(lvwInstalment.Columns.Item(1).Width) < 550 Then
            lvwInstalment.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(800))
        End If
    End Sub

    ' ***************************************************************** '
    '
    ' Name: UpdateStatus
    '
    ' Description: loop thro and update instalments status
    '
    ' History: 13/08/2001 TN - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateStatus() As Integer

        Dim result As Integer = 0
        'Developer Guide No.: 291
        Dim vTransactionID As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bIsUsingDocRef = True Then
                m_lReturn = m_oBusiness.ChangeStatus(lPFInstalmentsID:=CInt(m_vInstalments(gHUBSpokeConstants.eddPFInstalmentID, 0)), nStatus:=m_vInstalments(gHUBSpokeConstants.eddPFInstalmentStatusID, 0), nTransactionCode:=vTransactionID, bIsUsingDocRef:=m_bIsUsingDocRef)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to change status for instalment ID :" & CStr(m_vInstalments(gHUBSpokeConstants.eddPFInstalmentID, 0)), vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateStatus", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result
            End If

            If Not Information.IsArray(m_vInstalmentsModify) Then
                Return result
            End If

            For lCount As Integer = 0 To m_vInstalmentsModify.GetUpperBound(0)

                'only change transaction code if status is now retry

                'Developer Guide No 98
                vTransactionID = IIf(CDbl(m_vInstalments(gHUBSpokeConstants.eddPFInstalmentStatusID, CInt(m_vInstalmentsModify(lCount)))) = 5, PFTransactionRepresent, DBNull.Value)


                m_lReturn = m_oBusiness.ChangeStatus(lPFInstalmentsID:=CInt(m_vInstalments(gHUBSpokeConstants.eddPFInstalmentID, CInt(m_vInstalmentsModify(lCount)))), nStatus:=m_vInstalments(gHUBSpokeConstants.eddPFInstalmentStatusID, CInt(m_vInstalmentsModify(lCount))), nTransactionCode:=vTransactionID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to change status for instalment ID :" & CStr(m_vInstalments(gHUBSpokeConstants.eddPFInstalmentID, CInt(m_vInstalmentsModify(lCount)))), vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateStatus", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next

            'Reset array
            ReDim m_vInstalmentsModify(0)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Export
    '
    ' Description:mark batch ready for export and do export
    '
    ' History: 13/08/2001 TN - Created.
    '
    ' ***************************************************************** '
    Private Function Export() As Integer

        Dim result As Integer = 0
        Dim lBatchID, lRecordCount As Integer
        Dim sFileName As String = ""
        Dim lFailedOrion As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oExport.PFPaymentMethod_cnt = VB6.GetItemData(cboPaymentMethod, cboPaymentMethod.SelectedIndex)

            m_lReturn = m_oExport.Export(lLeadDays:=m_oFormFields.UnformatControl(txtLeadDays), sMediaTypeCode:=cboMediaType.ItemCode.Trim(), r_lBatchID:=lBatchID, r_lRecordCount:=lRecordCount, r_sFilenameUsed:=sFileName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Failed to export batch", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Batch " & lBatchID & " has been created. " & CStr(lRecordCount) & " records(s) have been exported to the file:" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & sFileName, "Instalments Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
                _StatusBar_Panel1.Text = "Last export file: " & sFileName

            End If
            Dim sOptionValue As String = "0"
            Dim bPaymentHubEnabled As Boolean = False
            sOptionValue = GetSystemOption(kSystemOptionPaymentHubEnabled)
            bPaymentHubEnabled = (sOptionValue = "1")

            If (cboMediaType.ItemCode.Trim().ToUpper = "OCP" OrElse cboMediaType.ItemCode.Trim().ToUpper = "CC") AndAlso bPaymentHubEnabled Then
                m_oBusiness.CallingAppName = "SIRIUSEXPORT"
            End If

            ' Now also post instalments, if option is selected on "export formats" screen

            If m_oExport.AllowAutoPost Then
                ' Ask user anyway
                If MessageBox.Show("Do you wish to post instalments?", "Post", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                    ' Post!

                    m_lReturn = m_oBusiness.PostInstalments(lBatchID:=lBatchID, r_lRecordsPosted:=lRecordCount, r_lFailedPosting:=lFailedOrion)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to post instalments to Accounts.", "Post Instalments", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show(CStr(lRecordCount) & " instalment(s) have been posted in this Batch to Accounts.", "Post Instalments", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        _StatusBar_Panel1.Text = "Last Posting: " & lRecordCount & New String(" "c, 1) & "instalment(s)"
                    End If

                    If lFailedOrion = gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Not all instalments could be posted to Accounts.", "Post Instalments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Export Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Private Function GetSystemOption(ByVal iOptionNumber As Integer) As String
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
                iUserID:=0,
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

    ' ***************************************************************** '
    '
    ' Name: ShowPopUpMenu
    '
    ' Description: build up popup menu and display it
    '
    ' History: 14/08/2001 TN - Created.
    '
    ' ***************************************************************** '
    Private Function ShowPopUpMenu() As Integer

        Dim result As Integer = 0
        Dim lAdd As gPMConstants.PMEReturnCode
        Const StatusID As Integer = 0
        Const StatusCode As Integer = 1
        Const StatusDesc As Integer = 2
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(m_vInstalmentsStatus) Then
                Return result
            End If

            For lCount As Integer = 0 To m_vInstalmentsStatus.GetUpperBound(1)

                lAdd = gPMConstants.PMEReturnCode.PMTrue

                Select Case ScreenType
                    Case 0
                        Select Case CStr(m_vInstalmentsStatus(StatusID, lCount)).ToUpper()
                            Case CStr(bSIRPremFinConst.PFStatusPending), CStr(bSIRPremFinConst.PFStatusCollected), CStr(bSIRPremFinConst.PFStatusFailed), CStr(bSIRPremFinConst.PFStatusTransferred), CStr(bSIRPremFinConst.PFStatusChargeback)
                                lAdd = gPMConstants.PMEReturnCode.PMFalse
                        End Select
                    Case 1 'post
                        Select Case CStr(m_vInstalmentsStatus(StatusID, lCount)).ToUpper()
                            Case CStr(bSIRPremFinConst.PFStatusNew), CStr(bSIRPremFinConst.PFStatusCollected), CStr(bSIRPremFinConst.PFStatusTransferred), CStr(bSIRPremFinConst.PFStatusChargeback)
                                lAdd = gPMConstants.PMEReturnCode.PMFalse
                        End Select

                    Case 2 'recall
                        Select Case CStr(m_vInstalmentsStatus(StatusID, lCount)).ToUpper()
                            Case CStr(bSIRPremFinConst.PFStatusNew), CStr(bSIRPremFinConst.PFStatusPending), CStr(bSIRPremFinConst.PFStatusManual), CStr(bSIRPremFinConst.PFStatusTransferred)
                                lAdd = gPMConstants.PMEReturnCode.PMFalse
                        End Select

                End Select



                If lAdd = gPMConstants.PMEReturnCode.PMTrue Then

                    ContainerHelper.LoadControl(Me, "mnuItem", mnuItem.GetUpperBound(0) + 1)


                    'Developer Guide No.: 149
                    mnuItem(mnuItem.GetUpperBound(0)).Text = Convert.ToString(m_vInstalmentsStatus(StatusDesc, lCount))
                    mnuItem(mnuItem.GetUpperBound(0)).Tag = CStr(lCount)

                    ''Start: Sudhir
                    'If mnuItem(mnuItem.GetUpperBound(0)).Text = "" Then
                    '    mnuItem(mnuItem.GetUpperBound(0)).Text = Convert.ToString(m_vInstalmentsStatus(StatusDesc, lCount))
                    '    mnuItem(mnuItem.GetUpperBound(0)).Tag = CStr(lCount)

                    '    'Developer Guide No.: 149
                    '    'mnuItem(mnuItem.GetUpperBound(0)).Text = CStr(m_vInstalmentsStatus(StatusDesc, lCount))
                    'Else
                    '    ContainerHelper.LoadControl(Me, "mnuItem", mnuItem.GetUpperBound(0) + 1)
                    '    mnuItem(mnuItem.GetUpperBound(0)).Text = Convert.ToString(m_vInstalmentsStatus(StatusDesc, lCount))
                    '    mnuItem(mnuItem.GetUpperBound(0)).Tag = CStr(lCount)
                    'End If

                    'End: Sudhir

                End If

            Next





            'hide the temp menu item
            mnuItem(0).Visible = False

            Ctx_mnuPopUp.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)


            'making sure that the temp/base menu item is visible
            mnuItem(0).Visible = False

            'Below code is not showing mnuitem on the form, so commented
            'For lCount As Integer = 0 To mnuItem.GetUpperBound(0)
            '    ContainerHelper.UnloadControl(Me, "mnuItem", lCount)
            'Next



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowPopUpMenu Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPopUpMenu", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PopulatePaymentMethod
    '
    ' Description: get payment method and populate combobox
    '
    ' History: 11/09/2001 TN - Created.
    '
    ' ***************************************************************** '
    Private Function PopulatePaymentMethod() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Const ACFieldPosPaymentMethodCount As Integer = 0
        Const ACFieldPosPaymentMethodDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oExport.GetPaymentMethods(vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vResultArray) Then
                cboPaymentMethod.Enabled = False
                Return result
            End If

            cboPaymentMethod.Items.Clear()

            For lCount As Integer = 0 To vResultArray.GetUpperBound(1)
                'eck310102 Only export bacs

                If (CStr(vResultArray(ACFieldPosPaymentMethodDesc, lCount))) = "DDM BACS" Then
                    Dim cboPaymentMethod_NewIndex As Integer = -1

                    cboPaymentMethod_NewIndex = cboPaymentMethod.Items.Add(CStr(vResultArray(ACFieldPosPaymentMethodDesc, lCount)))

                    VB6.SetItemData(cboPaymentMethod, cboPaymentMethod_NewIndex, CInt(vResultArray(ACFieldPosPaymentMethodCount, lCount)))
                End If
            Next lCount

            cboPaymentMethod.SelectedIndex = 0

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulatePaymentMethod Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulatePaymentMethod", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    'As earlier event was not bind with the module
    Private Sub _mnuItem_0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _mnuItem_0.Click
        Dim Index As Integer = Array.IndexOf(mnuItem, sender)

        Dim bFound As Boolean

        Const StatusID As Integer = 0
        Const StatusCode As Integer = 1
        Const StatusDesc As Integer = 2

        'get the selected instalment

        Dim lSelectedItem As Integer = Convert.ToString(lvwInstalment.Items.Item(lvwInstalment.FocusedItem.Index).Tag)

        'change to selected status
        m_vInstalments(gHUBSpokeConstants.eddPFInstalmentStatusID, lSelectedItem) = m_vInstalmentsStatus(StatusID, CInt(Convert.ToString(mnuItem(Index).Tag)))
        m_vInstalments(gHUBSpokeConstants.eddPFInstalmentStatusDescription, lSelectedItem) = m_vInstalmentsStatus(StatusDesc, CInt(Convert.ToString(mnuItem(Index).Tag)))

        'add instalment id to modify list if not already exist
        If Information.IsArray(m_vInstalmentsModify) Then
            bFound = False
            For Each m_vInstalmentsModify_item As Object In m_vInstalmentsModify

                bFound = (CDbl(m_vInstalmentsModify_item) = lSelectedItem)

                If bFound Then
                    Exit For
                End If
            Next m_vInstalmentsModify_item

            'if not in the list then add it in
            If Not bFound Then
                ReDim Preserve m_vInstalmentsModify(m_vInstalmentsModify.GetUpperBound(0) + 1)
                m_vInstalmentsModify(m_vInstalmentsModify.GetUpperBound(0)) = lSelectedItem
            End If

        Else
            ReDim m_vInstalmentsModify(0)
            m_vInstalmentsModify(0) = lSelectedItem
        End If

        If (Not m_vInstalmentstoRecall.Contains(CInt(m_vInstalments(gHUBSpokeConstants.eddPFInstalmentID, lSelectedItem)))) Then
            m_vInstalmentstoRecall.Add(CInt(m_vInstalments(gHUBSpokeConstants.eddPFInstalmentID, lSelectedItem)))
        End If

        'DD081001 - changed grid directly to avoid a refresh
        'refresh listview
        'm_lReturn = BusinessToInterface(v_lGetData:=PMFalse)
        'refer Developer Guide No. 52
        lvwInstalment.FocusedItem.SubItems(1).Text = m_vInstalmentsStatus(StatusDesc, CInt(Convert.ToString(mnuItem(Index).Tag)))
        If VB6.PixelsToTwipsX(lvwInstalment.Columns.Item(1).Width) < 550 Then
            lvwInstalment.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(800))
        End If
    End Sub

    Private Sub Ctx_mnuPopUp_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuPopUp.Closing

    End Sub

    Private Sub StatusBar_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles StatusBar.ItemClicked

    End Sub

    Private Sub txtDocumentRef_Change()
        If Trim$(Me.txtDocumentRef.Text) = "" Then
            Me.txtBatchNo.Enabled = True
        Else
            Me.txtBatchNo.Enabled = False
            Me.txtBatchNo.Text = "0"
        End If
    End Sub

End Class
