Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles
Imports Artinsoft.VB6.Gui

Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date:
    '
    ' Description: Main interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    '********************************
    ' General Property variables
    Private m_sSessionGUID As String
    Private m_sItemsFound As String
    Public m_sAgentId As String

    Private m_iStatus As Integer
    Private m_bError As Integer
    Private m_iReturn As Integer
    Private m_iUserPaymentCurrencyId As Integer
    Private m_iHasPaymentsAuthority As Integer
    Private m_iErrorNumber As Integer
    Private m_iBatchId As Integer
    Private m_iNavigate As Integer

    Private m_cTotalAmountReferredForAuthorisation As Decimal

    Private m_bPaymentinProgress As Boolean
    Private m_bChequeProduction As Boolean
    Private m_bNextClicked As Boolean

    Private m_iTotalTransactionsReferredForAuthorisation As Short

    Private m_oBusiness As Object
    Private m_oUserAuthorities As Object
    Private m_obACTCurrencyConvert As Object

    Private m_vSearchData As Object
    Private m_vMarkData As Object
    Private m_vMarkAccounts As Object
    Private m_vSummary As Object
    Private m_vAgentId As Object
    Private m_vMarkAgentId As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Declare an instance of the Business object.
    Private m_oGeneral As iACTCommissionPayments.General
    Private m_iPartyCnt As Integer
    'SAVE Button Code
    Private m_sStatementDate As String
    Private m_sTransDateFrom As String
    Private m_sTransDateTo As String
    Private m_iCurrencyItemID As Integer
    Private m_iProductItemID As Integer
    Private m_iBranchItemID As Integer
    Private m_iTransAuthLimit As Integer
    Private m_bAutoSearch As Boolean

    ' ***************************************************************** '
    ' Name: cmdNewSearch_Click
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' Created :
    ' ***************************************************************** '

    Private Sub cmdNewSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNewSearch.Click
        ' Click event of the New Search button.
        Const kMethodName As String = "cmdNewSearch_Click"

        Dim lSubValue As Integer

        Try



            ' Clear the interface details.
            m_iReturn = ClearInterface(bConfirm:=True)

            ' Check for errors.
            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to clear the interface details.
                RaiseError(kMethodName, "cmdNewSearch_Click Failed")
            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: cmdNext_Click
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' Created :
    ' ***************************************************************** '

    Private Sub cmdNext_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNext.Click

        Const kMethodName As String = "cmdNext_Click"

        Dim bIsValid As Boolean
        Dim lBatchId As Integer
        Dim lSubValue As Integer

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            ' Get the details from the business object.
            ' Display a Processing message.
            DisplayStatusProcessing()

            m_cTotalAmountReferredForAuthorisation = 0
            m_iTotalTransactionsReferredForAuthorisation = 0

            If ToSafeDate(dateStatementDate.Value, CDate("00:00:00")) = CDate("00:00:00") Then
                Call MsgBox("Please enter a valid Statement Date", MsgBoxStyle.Exclamation, Text)
                Exit Sub
            End If

            If m_bPaymentinProgress = False Then
                m_bPaymentinProgress = True
                m_iReturn = SettleAllPayments()

                If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Function Settle All Payments Failed")
                End If

                m_vSummary = Nothing
                m_bPaymentinProgress = False

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            m_iReturn = EnableDisableButtons()
            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                'Failed in EnableDisableCancel method
                RaiseError(kMethodName, "Function EnableDisableButtons Failed")
            End If

            m_vSummary = Nothing

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Display the number of item found message.
            'DisplayStatusFound



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: cmdPreviewStatement_Click
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' Created :
    ' ***************************************************************** '

    Private Sub cmdPreviewStatement_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdPreviewStatement.Click

        Const kMethodName As String = "cmdNext_Click"

        Dim lSubValue As Integer

        Try

            m_iReturn = ShowCommissionStatementReport(v_bIsPreview:=True)

            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Function ShowCommissionStatementReport Failed")
            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here


        Finally


        End Try
        Exit Sub
    End Sub

    Private Sub cmdSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSave.Click

        Const kMethodName As String = "cmdSave_Click"
        Try

            m_iReturn = SaveSelection()
            If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SaveSelection Failed.")
            End If

            m_iReturn = UnlockAgentForCommissionPayment()
            ' Check for errors.
            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "UnlockAgentForCommissionPayment Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            Me.Hide()

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally


        End Try
    End Sub

    Private Sub dateStatementDate_Change(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles dateStatementDate.ValueChanged
        Dim lSubValue As Integer

        Try




            m_iReturn = m_oFormFields.LostFocus(ctlControl:=dateStatementDate)
            If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                dateStatementDate.Focus()
            End If

            dateTo.Value = ToSafeDate(dateStatementDate.Value)
            If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("dateStatementDate_Change", "dateStatementDate_Change Failed")
            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="dateStatementDate_Change", r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally


        End Try
        Exit Sub
    End Sub


    ' ***************************************************************** '
    ' Name: Form_Initialize
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    'Created :
    ' ***************************************************************** '
    Private Sub Form_Initialize_Renamed()
        Dim bObjectManager As Object

        Const kMethodName As String = "Form_Initialize"

        Dim lReturn As Integer
        Dim lSubValue As Integer

        Try


            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' initialise form error indicator
            m_bError = False

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_iStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager

            ' Call the initialise method.

            lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Get Instance of bObjectManager.ObjectManager Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager

                g_iLanguageID = .LanguageID

                g_iSourceID = .SourceID

                g_iUserID = .UserID
            End With

            ' Get an instance of the business object via
            ' the public object manager.

            lReturn = g_oObjectManager.GetInstance(oObject:=m_oBusiness, sClassName:="bACTCommissionPayments.Business", vInstanceManager:=PMGetViaClientManager)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to get instance of bSIRListRisks.Business", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = g_oObjectManager.GetInstance(oObject:=m_oUserAuthorities, sClassName:="bACTUserAuthorities.Business", vInstanceManager:=PMGetViaClientManager)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to create instance of UserAuthorities")
            End If

            m_oGeneral = New iACTCommissionPayments.General

            lReturn = m_oGeneral.Initialise(frmInterface:=Me)


            lReturn = g_oObjectManager.GetInstance(oObject:=m_obACTCurrencyConvert, sClassName:="bACTCurrencyConvert.Form", vInstanceManager:=PMGetViaClientManager)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to create instance of CurrencyConvert")
            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Form_QueryUnload
    '
    ' Description: Determines whether any actions need to take place
    '               before unload.
    ' History:
    '           Created :  : Date : Process Id
    ' ***************************************************************** '
    Private Sub frmInterface_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason

        Dim lSubValue As Integer

        ' Forms query unload event.
        Try


            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.
            If (UnloadMode <> vbFormCode) Then
                ' Set the interface status.
                m_iStatus = gPMConstants.PMEReturnCode.PMCancel

                m_iReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    Exit Sub
                End If
                m_iReturn = UnlockAgentForCommissionPayment()
                ' Check for errors.
                If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    RaiseError("Form_QueryUnload", "UnlockAgentForCommissionPayment failed")
                End If

                ' Check if we have an instance of the Object Manager.
                If ((g_oObjectManager Is Nothing) = False) Then

                    ' Call the terminate method.

                    g_oObjectManager.Dispose()



                    ' Destroy the instance of the object manager
                    ' from memory.

                    g_oObjectManager = Nothing

                End If

                m_oGeneral.Dispose()

                ' Check for errors.



                m_oGeneral = Nothing


                m_oUserAuthorities.Dispose()



                m_oUserAuthorities = Nothing
            End If




        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="Form_QueryUnload", r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
            eventArgs.Cancel = Cancel
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Form_Terminate
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :  : Date : Process ID
    ' ***************************************************************** '
    Private Sub Form_Terminate_Renamed()
        Const kMethodName As String = "Form_Terminate"
        Dim lSubValue As Integer
        Try



            'Terminate will have done this, but just in case...
            If ((m_oBusiness Is Nothing) = False) Then
                ' Terminate the business object

                m_oBusiness.Dispose()



                ' Destroy the instance of the business object
                ' from memory.
                m_oBusiness = Nothing
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            ' destroy object reference

            m_oBusiness = Nothing



        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Form_Unload
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Sub frmInterface_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Const kMethodName As String = "Form_Unload"

        Dim lReturn As Integer
        Dim lSubValue As Integer

        Try



            ' Terminate the business object

            m_oBusiness = Nothing


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            ' destroy object reference

            m_oBusiness = Nothing


        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Form_Load
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Sub frmInterface_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Const kMethodName As String = "Form_Load"

        Dim lReturn As Integer
        Dim lSubValue As Integer
        Dim lCntRow As Integer
        Dim lArrCnt As Integer
        Dim sLockedBy As String
        txtLeadDays.Visible = False
        lblLeadDays.Visible = False
        lblAlctdBfr.Visible = False
        txtLeadDays.MaxLength = 3
        Try


            ' Center the interface.
            iPMFunc.CenterForm(Me)

            m_oFormFields = New iPMFormControl.FormFields

            If IsArray(m_vAgentId) Then
                lArrCnt = 0

                For lCntRow = 0 To UBound(m_vAgentId)
                    'Call Lock code here START
                    sLockedBy = "" 'PN# 68577
                    lReturn = LockAgentForCommissionPayment(ToSafeLong(m_vAgentId(lCntRow)), sLockedBy)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MsgBox(sLockedBy, MsgBoxStyle.Information, "Commission Payments")
                    Else
                        If lArrCnt = 0 Then
                            ReDim m_vMarkAgentId(lArrCnt)
                        Else
                            ReDim Preserve m_vMarkAgentId(lArrCnt)
                        End If


                        m_vMarkAgentId(lArrCnt) = m_vAgentId(lCntRow)

                        lArrCnt = lArrCnt + 1

                    End If
                    'Call Lock code here END
                Next
            ElseIf m_iPartyCnt > 0 Then
                'Call Lock code here START
                lReturn = LockAgentForCommissionPayment(m_iPartyCnt, sLockedBy)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MsgBox(sLockedBy, MsgBoxStyle.Information, "Commission Payments")
                Else
                    ReDim m_vMarkAgentId(0)

                    m_vMarkAgentId(0) = m_iPartyCnt
                End If
            End If

            lReturn = SetFieldValidation()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SetupForm Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Set the interface default values.
            lReturn = SetInterfaceDefaults()
            ' Check for errors.
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                m_iErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If
            Me.CboMediaType.FirstItem = ""
            Me.cboCurrency.FirstItem = "(ALL)"
            Me.cmbBranch.FirstItem = "(ALL)"
            Me.cmbProduct.FirstItem = "(ALL)"
            lReturn = GetUserAuthorities()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("Form_Load", "Failed to get User Authorities")
            End If
            If m_bAutoSearch Then
                Call cmdFindNow_Click(cmdFindNow, New System.EventArgs())
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Try



            SetFieldValidation = gPMConstants.PMEReturnCode.PMTrue

            Dim lDecimalPlaces As Integer

            lDecimalPlaces = 2
            ' {* USER DEFINED CODE (Begin) *}


            m_iReturn = m_oFormFields.AddNewFormField(ctlControl:=dateStatementDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                SetFieldValidation = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_iReturn = m_oFormFields.AddNewFormField(ctlControl:=dateFrom, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                SetFieldValidation = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_iReturn = m_oFormFields.AddNewFormField(ctlControl:=dateTo, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                SetFieldValidation = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_iReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLimitAmount, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDouble, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=lDecimalPlaces)

            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                SetFieldValidation = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' {* USER DEFINED CODE (End) *}


        Catch ex As Exception

            ' Error Section.

            SetFieldValidation = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Finally



        End Try
        Exit Function

    End Function

    Private Sub lvwCommPayments_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ColumnClickEventArgs) Handles lvwCommPayments.ColumnClick
        Dim ColumnHeader As System.Windows.Forms.ColumnHeader = lvwCommPayments.Columns(eventArgs.Column)
        Try


            ' If new column reset sort order
            If ListViewHelper.GetSortKeyProperty(lvwCommPayments) <> ColumnHeader.Index Then
                lvwCommPayments.Sorting = System.Windows.Forms.SortOrder.Descending
            End If

            ' If current sort column header is pressed.
            Select Case ColumnHeader.Index
                Case MainModule.ListViewCommissionEnum.ACLTAgent, MainModule.ListViewCommissionEnum.ACLTAgentName, MainModule.ListViewCommissionEnum.ACLTCurrency
                    ' String columns
                    ListViewHelper.SetSortedProperty(lvwCommPayments, False)
                    ListViewHelper.SetSortKeyProperty(lvwCommPayments, ColumnHeader.Index)
                    If ListViewHelper.GetSortOrderProperty(lvwCommPayments) = SortOrder.Ascending Then
                        ListViewHelper.SetSortOrderProperty(lvwCommPayments, SortOrder.Descending)
                    Else
                        ListViewHelper.SetSortOrderProperty(lvwCommPayments, SortOrder.Ascending)
                    End If
                    'ListViewHelper.SetSortOrderProperty(lvwTransactions, (ListViewHelper.GetSortOrderProperty(lvwTransactions) + 1) Mod 2)
                    ListViewHelper.SetSortedProperty(lvwCommPayments, True)
                Case MainModule.ListViewCommissionEnum.ACLTAccountId, MainModule.ListViewCommissionEnum.ACLTTotalComm, MainModule.ListViewCommissionEnum.ACLTAuthLimit
                    ' Currency columns
                    If ListViewHelper.GetSortOrderProperty(lvwCommPayments) = SortOrder.Ascending Then
                        m_iReturn = ListView6Func.ListViewSortByValue(v_oListView:=lvwCommPayments, v_iSourceColumn:=ColumnHeader.Index, v_iDirection:=SortOrder.Descending, v_bMarkSortedColumn:=True, v_bIsCurrency:=True)
                    Else
                        m_iReturn = ListView6Func.ListViewSortByValue(v_oListView:=lvwCommPayments, v_iSourceColumn:=ColumnHeader.Index, v_iDirection:=SortOrder.Ascending, v_bMarkSortedColumn:=True, v_bIsCurrency:=True)
                    End If

                    'm_iReturn = ListView6Func.ListViewSortByValue(v_oListView:=lvwCommPayments, v_iSourceColumn:=ColumnHeader.Index - 1, v_iDirection:=(lvwCommPayments.Sorting + 1) Mod 2, v_bMarkSortedColumn:=True, v_bIsCurrency:=True)
            End Select


        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwCommPayments_ColumnClick", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Finally

        End Try
    End Sub


    Private Sub lvwCommPayments_ItemClick(ByVal Item As System.Windows.Forms.ListViewItem)
        If Item.Checked Then
            Item.Checked = True
        Else
            Item.Checked = False
        End If
    End Sub

    Private Sub txtLimitAmount_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtLimitAmount.Enter

        Dim lSubValue As Integer
        Try




            m_iReturn = m_oFormFields.GotFocus(ctlControl:=txtLimitAmount)
            If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("txtLimitAmount_GotFocus", "txtLimitAmount_GotFocus Failed")
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="txtLimitAmount_GotFocus", r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally


        End Try
        Exit Sub
    End Sub

    Private Sub txtLimitAmount_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtLimitAmount.Leave

        Dim lSubValue As Integer

        Try



            If txtLimitAmount.Text <> "" Then
                If IsNumeric(txtLimitAmount.Text) = False Then
                    MsgBox("Field amount doesn't allow alpha-numeric characters. Please re-enter.", MsgBoxStyle.Critical)
                    txtLimitAmount.Text = ""
                    txtLimitAmount.Focus()
                End If
            End If

            m_iReturn = m_oFormFields.LostFocus(ctlControl:=txtLimitAmount)
            If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("txtLimitAmount_LostFocus", "txtLimitAmount_LostFocus Failed")
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="txtLimitAmount_LostFocus", r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Exit Sub
    End Sub


    Private Function GetUserAuthorities() As Integer

        Const kMethodName As String = "GetUserAuthorities"

        Dim crConvertedCurrency As Decimal
        Dim iPaymentsCurrencyID As Short
        Dim cPaymentsAmount As Decimal
        Dim iCount As Short
        Dim vResults As Object

        Try


            GetUserAuthorities = gPMConstants.PMEReturnCode.PMTrue



            m_iReturn = m_oUserAuthorities.GetUserAuthoritiesDetails(v_lUserId:=g_oObjectManager.UserID, r_vResults:=vResults)
            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Unable to get User Authorities Details")
            End If

            If IsArray(vResults) Then
                m_iHasPaymentsAuthority = ToSafeInteger(vResults(ACHasPaymentsAuthority, 0))

                iPaymentsCurrencyID = ToSafeInteger(vResults(ACPaymentsCurrencyID, 0))

                m_iUserPaymentCurrencyId = iPaymentsCurrencyID

                cPaymentsAmount = ToSafeCurrency(vResults(ACPaymentsAmount, 0))
            End If

            If m_iHasPaymentsAuthority <> gPMConstants.PMEReturnCode.PMTrue Then
                fraAuthLimit.Visible = False
                txtAuthCurrency.Text = ""
                txtLimitAmount.Text = CStr(0)
                chkTransAuthLimit.CheckState = System.Windows.Forms.CheckState.Unchecked
            Else

                For iCount = 0 To cboCurrency.ListCount - 1

                    If cboCurrency.ItemData(iCount) = iPaymentsCurrencyID Then

                        txtAuthCurrency.Text = cboCurrency.List(iCount)
                        Exit For
                    End If
                Next
                txtLimitAmount.Text = VB6.Format(cPaymentsAmount, "0.00")
            End If



        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetUserAuthorities, excep:=ex)

        Finally

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearInterface
    '
    ' Description: Clears all of the interface details for a new
    '              search.
    '
    ' ***************************************************************** '
    Private Function ClearInterface(ByRef bConfirm As Boolean) As Integer

        Dim iMsgResult As Short
        Dim sMessage As String
        Dim sTitle As String
        Dim lReturn As Integer

        Try


            ClearInterface = gPMConstants.PMEReturnCode.PMTrue

            ' Check if the user still wishes to clear the interface.
            ' Clear the interface details.

            If bConfirm Then

                sTitle = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)


                sMessage = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)

                ' Display the message.
                iMsgResult = MsgBox(sMessage, MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2 + MsgBoxStyle.Question, sTitle)

                ' Check message result.
                If (iMsgResult = MsgBoxResult.No) Then
                    ' Don't continue with the clear.
                    Exit Function
                End If
            End If

            ' Clear the search data array.

            m_vSearchData = Nothing

            ' Clear the search list details.
            lvwCommPayments.Items.Clear()

            ' Clear the search status bar.
            stbStatus.Text = ""
            _stbStatus_Panel1.Text = ""
            ' {* USER DEFINED CODE (Begin) *}

            ' Clearing the Search Details from the Combo Box and the Text Box

            If cboCurrency.ListCount > 0 Then

                cboCurrency.ListIndex = 0
            End If


            If cmbBranch.ListCount > 0 Then

                cmbBranch.ListIndex = 0
            End If

            If CboMediaType.ListCount > 0 Then

                CboMediaType.ListIndex = 0
            End If

            If cmbProduct.ListCount > 0 Then

                cmbProduct.ListIndex = 0
            End If

            'lReturn = m_oFormFields.FormatControl(dateStatementDate, Date)
            dateStatementDate.Value = ToSafeDate(Today)
            dateFrom.Value = ToSafeDate(Today)
            dateFrom.Checked = False


            chkTransAuthLimit.CheckState = System.Windows.Forms.CheckState.Checked


            lReturn = m_oFormFields.LostFocus(ctlControl:=dateStatementDate)

            If (lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
                'lReturn& = m_oFormFields.FormatControl(dateTo, dateStatementDate)
                dateTo.Value = ToSafeDate(dateStatementDate.Value)
            Else
                dateStatementDate.Focus()
            End If
            chkCommForAllctdTrans.Checked = False
            txtLeadDays.Text = ""

            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            lReturn = DisableInterface(bDisable:=True)



        Catch ex As Exception

            ClearInterface = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Finally


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Try


            DisableInterface = gPMConstants.PMEReturnCode.PMTrue

            cmdNext.Enabled = Not bDisable
            cmdPreviewStatement.Enabled = Not bDisable
            cmdSave.Enabled = Not bDisable



        Catch ex As Exception
            ' Error Section.

            DisableInterface = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Finally


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Try

            DisplayCaptions = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            Me.Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)

            ' Check for an error.
            If (Me.Text = "") Then
                RaiseError("DisplayCaptions", "iPMFunc.GetResData failed")
            End If


            lblStatementDate.Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStatementDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            lblTransDateFrom.Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTransDateFrom, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            lblTransDateTo.Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTransDateTo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            fraFilterTrans.Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFraTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            fraAuthLimit.Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFraTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            lblCurrency.Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            lblProduct.Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACProduct, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            lblAuthCurrency.Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            lblLimitAmount.Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLimitAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            chkTransAuthLimit.Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTransAuthLimit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            cmdNext.Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNextButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            cmdCancel.Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            cmdFindNow.Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindNowButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            cmdNewSearch.Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewSearchButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            cmdPreviewStatement.Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPreviewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)

            lvwCommPayments.Columns.Item(k_CommissonColHIndexCheckBox).Text = ""
            lvwCommPayments.Columns.Item(k_CommissonColHIndexAccountId).Text = ""
            lvwCommPayments.Columns.Item(k_CommissonColHIndexAgent).Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            lvwCommPayments.Columns.Item(k_CommissonColHIndexAgentName).Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            lvwCommPayments.Columns.Item(k_CommissonColHIndexTotalComm).Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            lvwCommPayments.Columns.Item(k_CommissonColHIndexCurrency).Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            lvwCommPayments.Columns.Item(k_CommissonColHIndexAuthLimit).Text = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            lvwCommPayments.Columns.Item(k_CommissonColHIndexCheckBox).Width = VB6.TwipsToPixelsX(400)
            lvwCommPayments.Columns.Item(k_CommissonColHIndexAccountId).Width = 0
            lvwCommPayments.Columns.Item(k_CommissonColHIndexAgent).Width = VB6.TwipsToPixelsX(1800)
            lvwCommPayments.Columns.Item(k_CommissonColHIndexAgentName).Width = VB6.TwipsToPixelsX(3000)
            lvwCommPayments.Columns.Item(k_CommissonColHIndexTotalComm).Width = VB6.TwipsToPixelsX(2000)
            lvwCommPayments.Columns.Item(k_CommissonColHIndexCurrency).Width = VB6.TwipsToPixelsX(1600)
            lvwCommPayments.Columns.Item(k_CommissonColHIndexAuthLimit).Width = VB6.TwipsToPixelsX(2000)



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="DisplayCaptions", r_lFunctionReturn:=DisplayCaptions, excep:=ex)

        Finally


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Const kMethodName As String = "SetInterfaceDefaults"
        Dim lReturn As Integer

        Try


            SetInterfaceDefaults = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.
            lReturn = DisplayCaptions()

            'Check for errors.
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                SetInterfaceDefaults = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            lReturn = SetExtraListViewProperties(v_hWndList:=lvwCommPayments.Handle.ToInt32, v_vShowRowSelect:=True)
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                SetInterfaceDefaults = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            dateStatementDate.Value = ToSafeDate(Today)

            dateTo.Value = ToSafeDate(Today)

            If m_bAutoSearch Then
                If StatementDate <> "" Then
                    dateStatementDate.Value = ToSafeDate(StatementDate)
                End If
                If TransDateFrom <> "" Then
                    dateFrom.Value = ToSafeDate(TransDateFrom)
                    'Else
                    '    dateFrom.Value = ""
                End If
                If TransDateTo <> "" Then
                    dateTo.Value = ToSafeDate(TransDateTo)
                    'Else
                    '    dateTo.Value = ""
                End If

                cboCurrency.CurrencyId = CurrencyItemId

                cmbProduct.ItemId = ProductItemId

                cmbBranch.ItemId = BranchItemId
                If TransAuthLimit = 1 Then
                    chkTransAuthLimit.CheckState = System.Windows.Forms.CheckState.Checked
                Else
                    chkTransAuthLimit.CheckState = System.Windows.Forms.CheckState.Unchecked
                End If
            Else
                chkTransAuthLimit.CheckState = System.Windows.Forms.CheckState.Checked
            End If


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=SetInterfaceDefaults, excep:=ex)

        Finally


        End Try
    End Function

    ' PRIVATE Methods (Begin)
    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click

        Dim lSubValue As Integer

        ' Click event of the Cancel button.
        Try

            ' Set the interface status.
            m_iStatus = gPMConstants.PMEReturnCode.PMCancel

            If Not m_bNextClicked Then
                m_iReturn = RemoveComissionBatch(m_iBatchId)
                ' Check for errors.
                If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    RaiseError("cmdCancel_Click", "RemoveComissionBatch Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
            m_iReturn = UnlockAgentForCommissionPayment()
            ' Check for errors.
            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError("cmdCancel_Click", "UnlockAgentForCommissionPayment Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            ' Process the next set of actions.
            m_iReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If (m_iReturn = gPMConstants.PMEReturnCode.PMTrue) Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If


        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="cmdCancel_Click", r_lFunctionReturn:=lSubValue, excep:=ex)
        Finally


        End Try
    End Sub


    Public Property Status() As Integer
        Get
            Status = m_iStatus
        End Get
        Set(ByVal Value As Integer)
            m_iStatus = Value
        End Set
    End Property

    Public Property AgentIdsArray() As Object
        Get
            AgentIdsArray = m_vAgentId
        End Get
        Set(ByVal Value As Object)
            m_vAgentId = Value
        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get
            PartyCnt = m_iPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_iPartyCnt = Value
        End Set
    End Property

    Public Property StatementDate() As String
        Get
            StatementDate = m_sStatementDate
        End Get
        Set(ByVal Value As String)
            m_sStatementDate = Value
        End Set
    End Property

    Public Property TransDateFrom() As String
        Get
            TransDateFrom = m_sTransDateFrom
        End Get
        Set(ByVal Value As String)
            m_sTransDateFrom = Value
        End Set
    End Property

    Public Property TransDateTo() As String
        Get
            TransDateTo = m_sTransDateTo
        End Get
        Set(ByVal Value As String)
            m_sTransDateTo = Value
        End Set
    End Property

    Public Property CurrencyItemId() As Integer
        Get
            CurrencyItemId = m_iCurrencyItemID
        End Get
        Set(ByVal Value As Integer)
            m_iCurrencyItemID = Value
        End Set
    End Property

    Public Property ProductItemId() As Integer
        Get
            ProductItemId = m_iProductItemID
        End Get
        Set(ByVal Value As Integer)
            m_iProductItemID = Value
        End Set
    End Property

    Public Property BranchItemId() As Integer
        Get
            BranchItemId = m_iBranchItemID
        End Get
        Set(ByVal Value As Integer)
            m_iBranchItemID = Value
        End Set
    End Property

    Public Property TransAuthLimit() As Short
        Get
            TransAuthLimit = m_iTransAuthLimit
        End Get
        Set(ByVal Value As Short)
            m_iTransAuthLimit = Value
        End Set
    End Property

    Public Property AutoSearch() As Boolean
        Get
            AutoSearch = m_bAutoSearch
        End Get
        Set(ByVal Value As Boolean)
            m_bAutoSearch = Value
        End Set
    End Property
    ' ***************************************************************** '
    ' Name: DataToProperties
    ' Description: Updates the property member from the search data
    '              storage.
    ' ***************************************************************** '
    Public Function DataToProperties() As Integer

        Const kMethodName As String = "DataToProperties"
        Dim lSelectedItem As Integer

        Try


            DataToProperties = gPMConstants.PMEReturnCode.PMTrue
            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.
            lSelectedItem = ToSafeLong(VB.Right(lvwCommPayments.FocusedItem.Name, Len(lvwCommPayments.FocusedItem.Name) - 2))

        Catch ex As Exception
            ' Error Section.
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=DataToProperties, v_sUsername:=g_sUsername.Value, excep:=ex)
        Finally


        End Try
        Exit Function

    End Function

    Private Sub cmdFindNow_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFindNow.Click

        Dim lSubValue As Integer
        Dim bIsValid As Boolean

        Try


            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If ToSafeDate(dateStatementDate.Value, CDate("00:00:00")) = CDate("00:00:00") Then
                Call MsgBox("Please enter a valid Statement Date", MsgBoxStyle.Exclamation, Text)
                Exit Sub
            End If

            m_iReturn = PerformSearch()

            ' Check for errors.
            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get the interface details.
                RaiseError("cmdFindNow_Click", "PerformSearch Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_iReturn = EnableDisableButtons()

            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                'Failed in EnableDisableCancel method
                RaiseError("cmdFindNow_Click", "EnableDisableButtons Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="cmdFindNow_Click", r_lFunctionReturn:=lSubValue, excep:=ex)
        Finally
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
        Exit Sub
    End Sub

    ' ***************************************************************** '
    ' Name:         PerformSearch
    ' Description:  Retrieves the details from the business object.
    ' ***************************************************************** '
    Public Function PerformSearch() As Integer

        Const kMethodName As String = "PerformSearch"

        Dim dTransDateFrom As Date = Date.Parse(" 1/1/1899")
        Dim dTransDateTo As Date = Date.MaxValue.Date
        Dim iCurrencyId As Short
        Dim lProductId As Integer
        Dim lCompanyId As Integer
        Dim lUserId As Integer
        Dim iOnlyAuthorityLimit As Short
        Dim lCntRow As Integer

        Try


            PerformSearch = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.
            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while
            ' a search is in progress.
            m_iReturn = DisableInterface(bDisable:=True)

            ' Check for errors
            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get details.
                PerformSearch = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            If dateFrom.Checked Then
                If ToSafeDate(VB6.Format(dateFrom.Value, "short date"), CDate("00:00:00")) <> CDate("00:00:00") Then
                    dTransDateFrom = ToSafeDate(VB6.Format(dateFrom.Value, "short date"), CDate("00:00:00"))
                End If
            End If

            If dateTo.Checked Then
                If ToSafeDate(dateTo.Value, CDate("00:00:00")) <> CDate("00:00:00") Then
                    dTransDateTo = ToSafeDate(VB6.Format(dateTo.Value, "short date"), CDate("00:00:00"))
                End If
            End If
            If ToSafeDate(dateFrom.Value, CDate("00:00:00")) <> CDate("00:00:00") And ToSafeDate(dateTo.Value, CDate("00:00:00")) <> CDate("00:00:00") Then
                If ToSafeDate(VB6.Format(dTransDateFrom, "Short Date"), CDate("00:00:00")) > ToSafeDate(VB6.Format(dTransDateTo, "short date"), CDate("00:00:00")) Then
                    MsgBox("Please provide valid transaction date interval", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, Me.Text)
                    If dateTo.Enabled Then dateTo.Focus()
                    Exit Function
                End If
            End If

            ' Currency ID

            If cboCurrency.ListIndex > 0 Then
                iCurrencyId = ToSafeLong(cboCurrency.ItemData(cboCurrency.ListIndex))
            Else
                'For All Currency
                iCurrencyId = 0
            End If

            ' Product ID

            If cmbProduct.ListIndex > 0 Then
                lProductId = ToSafeLong(cmbProduct.ItemData(cmbProduct.ListIndex))
            Else
                'For All Product
                lProductId = 0
            End If

            ' Branch ID

            If cmbBranch.ListIndex > 0 Then
                lCompanyId = ToSafeLong(cmbBranch.ItemData(cmbBranch.ListIndex))
            Else
                ''For All Branches
                lCompanyId = 0
            End If

            ' User ID
            lUserId = g_iUserID

            ' Only within authority limit
            If chkTransAuthLimit.CheckState = System.Windows.Forms.CheckState.Checked Then
                iOnlyAuthorityLimit = chkTransAuthLimit.CheckState
            Else
                iOnlyAuthorityLimit = 0
            End If

            If IsArray(m_vMarkAgentId) Then
                'If UBound(m_vMarkAgentId) > 0 Then
                For lCntRow = 0 To UBound(m_vMarkAgentId)
                    If lCntRow = 0 Then

                        m_sAgentId = m_vMarkAgentId(lCntRow)
                    Else

                        m_sAgentId = m_sAgentId & "," & m_vMarkAgentId(lCntRow)
                    End If
                Next
                ' ElseIf UBound(m_vMarkAgentId) = 0 Then
                'm_sAgentId = m_vMarkAgentId(0)
                'End If

                If Len(m_sAgentId) > 0 Then
                    ' Prepare Agent Summary
                    If chkCommForAllctdTrans.Checked Then
                        
                        If txtLeadDays.Text.Contains(".") Then
                            txtLeadDays.Text = ""
                            MessageBox.Show("Please provide valid data for searching.", "Lead Days", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit Function
                        End If
                        If txtLeadDays.Text = "" Then
                            txtLeadDays.Text = 0
                        End If
                        dTransDateTo = dTransDateTo.AddDays(-Convert.ToInt32(txtLeadDays.Text))

                        m_iReturn = m_oBusiness.PrepareAgentSummaryForAllocatedTrans(v_dTransDateFrom:=dTransDateFrom, v_dTransDateTo:=dTransDateTo, v_iCurrencyID:=iCurrencyId, v_lProductId:=lProductId, v_lCompanyID:=lCompanyId, v_lUserId:=lUserId, v_iOnlyAuthorityLimit:=iOnlyAuthorityLimit, v_sAgentId:=m_sAgentId, r_sSessionGUID:=m_sSessionGUID, r_vResultArray:=m_vSearchData)
                    Else
                        m_iReturn = m_oBusiness.PrepareAgentSummary(v_dTransDateFrom:=dTransDateFrom, v_dTransDateTo:=dTransDateTo, v_iCurrencyID:=iCurrencyId, v_lProductId:=lProductId, v_lCompanyID:=lCompanyId, v_lUserId:=lUserId, v_iOnlyAuthorityLimit:=iOnlyAuthorityLimit, v_sAgentId:=m_sAgentId, r_sSessionGUID:=m_sSessionGUID, r_vResultArray:=m_vSearchData)
                    End If


                    ' Check the return values.
                    Select Case (m_iReturn)
                        Case gPMConstants.PMEReturnCode.PMTrue
                            ' Found search details.
                            DataToInterface()

                            Call DisplayStatusFound() 'PN# 68529 START
                        Case gPMConstants.PMEReturnCode.PMNotFound
                            ' No found search details
                            Call DisplayStatusFound(v_bItemFound:=True, v_lItemsFound:=0) 'PN# 68529 START
                        Case Else
                            ' Failed to get details.
                            ' Raise Error.
                            RaiseError(kMethodName, "Failed to get search details from the business object")
                    End Select
                End If
            Else
                Call DisplayStatusFound(v_bItemFound:=True, v_lItemsFound:=0) 'PN# 68529 START
            End If

        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=PerformSearch, v_sUsername:=g_sUsername.Value, excep:=ex)
        Finally

        End Try
        Exit Function
    End Function

    

    ' ***************************************************************** '
    ' Name: DataToInterface
    ' Description: Updates all interface details from the search data.
    '              storage.
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Const kMethodName As String = "DataToInterface"
        Dim sAgent As String
        Dim sAgentName As String
        Dim sAccount As String
        Dim sCurrency As String

        Dim oListItem As System.Windows.Forms.ListViewItem

        Dim iLBound As Short
        Dim iUBound As Short

        Dim dTotalComm As Double
        Dim dAuthLimit As Double

        Dim lCurrencyId As Integer
        Dim lAccountID As Integer
        Dim lRow As Integer
        Dim lArrCnt As Integer

        Dim dAuthLimitChanged As Decimal
        Dim bIsDeleted As Boolean
        Dim lItemFound As Integer
        Dim bDuplicate As Boolean
        Dim vDuplicates As Object
        Dim lIndex As Integer
        Dim bDuplicateStored As Boolean
        Dim sDuplicateAgentNames As String = ""

        Try


            DataToInterface = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the search details.
            lvwCommPayments.Items.Clear()

            ' Check that search details are valid before continuing.
            If (IsArray(m_vSearchData) = False) Then
                Exit Function
            End If

            ' Assign the details to the interface.
            iUBound = UBound(m_vSearchData, 2)
            iLBound = LBound(m_vSearchData, 2)
            lArrCnt = 0

            For lRow = iLBound To iUBound
                ''Assign details to other the columns
                bDuplicate = False
                m_iReturn = CheckForDuplicateAgent(lRow, bDuplicate)

                lAccountID = ToSafeLong(m_vSearchData(k_AccountId, lRow))

                sAccount = "t_" & ToSafeString(lAccountID)
                sAgent = ToSafeString(m_vSearchData(k_Agent, lRow))
                sAgentName = ToSafeString(m_vSearchData(k_AgentName, lRow))
                dTotalComm = ToSafeDouble(m_vSearchData(k_TotalComm, lRow)) ' * -1 PN# 68599
                sCurrency = ToSafeString(m_vSearchData(k_Currency, lRow))
                bDuplicateStored = False
                If bDuplicate Then
                    If IsArray(vDuplicates) Then
                        For lIndex = 0 To UBound(vDuplicates)

                            If lAccountID = vDuplicates(lIndex) Then
                                bDuplicateStored = True
                            End If
                        Next
                        If bDuplicateStored = False Then

                            ReDim vDuplicates(UBound(vDuplicates) + 1)

                            vDuplicates(UBound(vDuplicates)) = lAccountID
                            sDuplicateAgentNames = sDuplicateAgentNames & "  " & sAgentName
                        End If

                    Else

                        ReDim vDuplicates(0)

                        vDuplicates(0) = lAccountID
                        sDuplicateAgentNames = sDuplicateAgentNames & "  " & sAgentName
                    End If
                End If

                dAuthLimit = ToSafeDouble(txtLimitAmount.Text)
                lCurrencyId = ToSafeLong(m_vSearchData(k_CurrencyID, lRow))

                If bDuplicate = False Then
                    'Check is the user has Payments Authority
                    If ToSafeBoolean(m_iHasPaymentsAuthority) Then
                        If m_iUserPaymentCurrencyId <> lCurrencyId Then
                            m_iReturn = m_obACTCurrencyConvert.CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=lCurrencyId, v_crCurrencyAmountFrom:=dAuthLimit, v_lCompanyID:=g_iSourceID, v_lCurrencyIdTo:=m_iUserPaymentCurrencyId, r_crCurrencyAmountTo:=dAuthLimitChanged)
                            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                RaiseError(kMethodName, "m_obACTCurrencyConvert.CurrencyToCurrencyConversion Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        Else
                            dAuthLimitChanged = dAuthLimit
                        End If

                        If (System.Math.Abs(ToSafeDouble(dTotalComm)) > ToSafeDouble(dAuthLimitChanged)) Then
                            If (chkTransAuthLimit.CheckState = 1) Then
                                'NOT DISPLAY THIS ROW
                            Else
                                oListItem = lvwCommPayments.Items.Add(sAccount, "", "")
                                oListItem.Tag = CStr(lRow)

                                If oListItem.SubItems.Count > k_CommissonColHIndexAgent - 1 Then
                                    ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAgent).Text = Trim(sAgent)
                                Else
                                    ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAgent).Text = New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Trim(sAgent)).Text
                                End If

                                If oListItem.SubItems.Count > k_CommissonColHIndexAgentName - 1 Then
                                    ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAgentName).Text = Trim(sAgentName)
                                Else
                                    ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAgentName).Text = New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Trim(sAgentName)).Text
                                End If

                                If oListItem.SubItems.Count > k_CommissonColHIndexTotalComm - 1 Then
                                    ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexTotalComm).Text = VB6.Format(Trim(CStr(dTotalComm)), "0.00")
                                Else
                                    ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexTotalComm).Text = New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, VB6.Format(Trim(CStr(dTotalComm)), "0.00")).Text
                                End If

                                If oListItem.SubItems.Count > k_CommissonColHIndexCurrency - 1 Then
                                    ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexCurrency).Text = Trim(sCurrency)
                                Else
                                    ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexCurrency).Text = New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Trim(sCurrency)).Text
                                End If

                                If oListItem.SubItems.Count > k_CommissonColHIndexAuthLimit - 1 Then
                                    ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAuthLimit).Text = VB6.Format(Trim(CStr(dAuthLimitChanged)), "0.00")
                                Else
                                    ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAuthLimit).Text = New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, VB6.Format(Trim(CStr(dAuthLimitChanged)), "0.00")).Text
                                End If
                                oListItem.Selected = False
                                RemoveHandler lvwCommPayments.ItemCheck, AddressOf lvwCommPayments_ItemCheck
                                oListItem.Checked = False
                                AddHandler lvwCommPayments.ItemCheck, AddressOf lvwCommPayments_ItemCheck
                                lItemFound = lItemFound + 1
                            End If
                        ElseIf (System.Math.Abs(ToSafeDouble(dTotalComm)) <= ToSafeDouble(dAuthLimitChanged)) Then

                            oListItem = lvwCommPayments.Items.Add(sAccount, "", "")
                            oListItem.Tag = CStr(lRow)

                            If oListItem.SubItems.Count > k_CommissonColHIndexAgent - 1 Then
                                ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAgent).Text = Trim(sAgent)
                            Else
                                ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAgent).Text = New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Trim(sAgent)).Text
                            End If

                            If oListItem.SubItems.Count > k_CommissonColHIndexAgentName - 1 Then
                                ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAgentName).Text = Trim(sAgentName)
                            Else
                                ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAgentName).Text = New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Trim(sAgentName)).Text
                            End If

                            If oListItem.SubItems.Count > k_CommissonColHIndexTotalComm - 1 Then
                                ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexTotalComm).Text = VB6.Format(Trim(CStr(dTotalComm)), "0.00")
                            Else
                                ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexTotalComm).Text = New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, VB6.Format(Trim(CStr(dTotalComm)), "0.00")).Text
                            End If

                            If oListItem.SubItems.Count > k_CommissonColHIndexCurrency - 1 Then
                                ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexCurrency).Text = Trim(sCurrency)
                            Else
                                ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexCurrency).Text = New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Trim(sCurrency)).Text
                            End If

                            If oListItem.SubItems.Count > k_CommissonColHIndexAuthLimit - 1 Then
                                ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAuthLimit).Text = VB6.Format(Trim(CStr(dAuthLimitChanged)), "0.00")
                            Else
                                ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAuthLimit).Text = New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, VB6.Format(Trim(CStr(dAuthLimitChanged)), "0.00")).Text
                            End If
                            oListItem.Selected = True
                            RemoveHandler lvwCommPayments.ItemCheck, AddressOf lvwCommPayments_ItemCheck
                            oListItem.Checked = True
                            AddHandler lvwCommPayments.ItemCheck, AddressOf lvwCommPayments_ItemCheck
                            If lArrCnt = 0 Then
                                ReDim m_vMarkAccounts(lArrCnt)
                            Else
                                ReDim Preserve m_vMarkAccounts(lArrCnt + 1)
                            End If
                            m_vMarkAccounts(lArrCnt) = lAccountID
                            lArrCnt = lArrCnt + 1
                            lItemFound = lItemFound + 1
                        End If
                    Else
                        oListItem = lvwCommPayments.Items.Add(sAccount, "", "")
                        oListItem.Tag = CStr(lRow)

                        If oListItem.SubItems.Count > k_CommissonColHIndexAgent - 1 Then
                            ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAgent).Text = Trim(sAgent)
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAgent).Text = New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Trim(sAgent)).Text
                        End If

                        If oListItem.SubItems.Count > k_CommissonColHIndexAgentName - 1 Then
                            ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAgentName).Text = Trim(sAgentName)
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAgentName).Text = New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Trim(sAgentName)).Text
                        End If

                        If oListItem.SubItems.Count > k_CommissonColHIndexTotalComm - 1 Then
                            ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexTotalComm).Text = VB6.Format(Trim(CStr(dTotalComm)), "0.00")
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexTotalComm).Text = New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, VB6.Format(Trim(CStr(dTotalComm)), "0.00")).Text
                        End If

                        If oListItem.SubItems.Count > k_CommissonColHIndexCurrency - 1 Then
                            ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexCurrency).Text = Trim(sCurrency)
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexCurrency).Text = New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Trim(sCurrency)).Text
                        End If

                        If oListItem.SubItems.Count > k_CommissonColHIndexAuthLimit - 1 Then
                            ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAuthLimit).Text = VB6.Format(Trim(CStr(dAuthLimit)), "0.00")
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, k_CommissonColHIndexAuthLimit).Text = New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, VB6.Format(Trim(CStr(dAuthLimit)), "0.00")).Text
                        End If
                        oListItem.Selected = True
                        RemoveHandler lvwCommPayments.ItemCheck, AddressOf lvwCommPayments_ItemCheck
                        oListItem.Checked = True
                        AddHandler lvwCommPayments.ItemCheck, AddressOf lvwCommPayments_ItemCheck
                        If lArrCnt = 0 Then
                            ReDim m_vMarkAccounts(lArrCnt)
                        Else
                            ReDim Preserve m_vMarkAccounts(lArrCnt + 1)
                        End If
                        m_vMarkAccounts(lArrCnt) = lAccountID

                        lArrCnt = lArrCnt + 1
                        lItemFound = lItemFound + 1
                    End If
                End If
            Next lRow

            'Call function to Mark Commission Payments for the selected accounts
            m_iReturn = MarkCommission()
            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "MarkCommission Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If IsArray(vDuplicates) Then
                MsgBox("Broker " & sDuplicateAgentNames & "  has commission payments in two or more Account Currencies and cannot be processed as a single payment, please exclude this broker and rerun", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Commission Payments")
            End If

            m_iReturn = DisableInterface(bDisable:=False)

            ' Check for errors
            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get details.
                Exit Function
            End If

        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=DataToInterface, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            'Call DisplayStatusFound(v_bItemFound:=True, v_lItemsFound:=lItemFound)


        End Try
        Exit Function

    End Function

    ' ***************************************************************** '
    ' Name: EnableDisableButtons
    '
    ' Description: Enable Disable Buttons on the screen.
    '
    ' ***************************************************************** '

    Private Function EnableDisableButtons() As Integer

        Const kMethodName As String = "EnableDisableButtons"
        'Dim lRowID As Integer
        Dim bIsDeleted As Boolean
        Try


            EnableDisableButtons = gPMConstants.PMEReturnCode.PMTrue

            If (lvwCommPayments.Items.Count > 0 And lvwCommPayments.CheckedItems.Count > 0) Then
                'lRowID = ToSafeLong(VB.Right(lvwCommPayments.SelectedItems(0).Name, Len(lvwCommPayments.SelectedItems(0).Name) - 2))
                If IsArray(m_vMarkAccounts) Then
                    cmdNext.Enabled = True
                    cmdPreviewStatement.Enabled = True
                Else
                    cmdNext.Enabled = False
                    cmdPreviewStatement.Enabled = False
                End If
                cmdSave.Enabled = True
            Else
                cmdNext.Enabled = False
                cmdPreviewStatement.Enabled = False
                cmdSave.Enabled = False
            End If


        Catch ex As Exception
            EnableDisableButtons = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=EnableDisableButtons, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally

        End Try
        Exit Function
    End Function

    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusSearching()

        Static sMessage As String
        Try

            ' Get message text if not already present.
            If (sMessage = "") Then

                sMessage = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStatusSearching, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
            End If

            ' Display the status message.
            stbStatus.Text = " " & sMessage
            _stbStatus_Panel1.Text = " " & sMessage


        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Finally


        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusFound(Optional ByVal v_bItemFound As Boolean = False, Optional ByVal v_lItemsFound As Integer = 0)

        Dim lItemsFound As Integer
        Try


            ' Store the total of item found.
            If v_bItemFound Then
                lItemsFound = v_lItemsFound
            Else
                If ToSafeLong(lvwCommPayments.Items.Count) > 0 Then
                    lItemsFound = ToSafeLong(lvwCommPayments.Items.Count)
                Else
                    lItemsFound = 0
                End If
            End If

            ' Get message text if not already present.
            If (m_sItemsFound = "") Then
                m_sItemsFound = "Item(s) found"
            End If

            ' Display the status message.
            stbStatus.Text = " " & lItemsFound & " " & m_sItemsFound
            _stbStatus_Panel1.Text = " " & lItemsFound & " " & m_sItemsFound


        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
        Finally


        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: MarkCommission
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function MarkCommission() As Integer

        Dim lUserId As Integer
        Const kMethodName As String = "MarkCommission"
        Dim dStatementDate As Date
        Try


            MarkCommission = gPMConstants.PMEReturnCode.PMTrue

            ' Disable parts of the interface while
            ' a Mark Process is in progress.
            m_iReturn = DisableInterface(bDisable:=True)
            ' Check for errors
            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get details.
                MarkCommission = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' User ID
            lUserId = g_iUserID

            If ToSafeDate(dateStatementDate.Value, CDate("00:00:00")) <> CDate("00:00:00") Then
                dStatementDate = ToSafeDate(dateStatementDate.Value, CDate("00:00:00"))
            End If

            If IsArray(m_vMarkAccounts) Then
                If UBound(m_vMarkAccounts) >= 0 Then

                    m_iReturn = m_oBusiness.MarkCommissionPayments(v_lUserId:=lUserId, v_sSession_Guid:=m_sSessionGUID, v_vSelectedAccounts:=m_vMarkAccounts, v_dStatementDate:=dStatementDate, r_lBatchID:=m_iBatchId, r_vResultArray:=m_vMarkData)

                    ' Check the return values.
                    Select Case (m_iReturn)
                        Case gPMConstants.PMEReturnCode.PMTrue
                            ' Found search details.
                            'TempDisplayData
                        Case gPMConstants.PMEReturnCode.PMNotFound
                            ' No found search details
                        Case Else
                            ' Failed to get details.
                            ' Raise Error.
                            RaiseError(kMethodName, "Failed to get search details from the business object")

                    End Select
                End If
            End If

        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=MarkCommission, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally


        End Try
        Exit Function
    End Function



    ' ***************************************************************** '
    ' Name: DisplayStatusProcessing
    '
    ' Description: Display the status processing message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusProcessing()

        Static sMessage As String
        Try


            ' Get message text if not already present.
            If (sMessage = "") Then

                sMessage = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStatusProcessing, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString)
            End If

            ' Display the status message.
            stbStatus.Text = " " & sMessage
            _stbStatus_Panel1.Text = " " & sMessage


        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusProcessing", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Finally


        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: SettleAllPayments
    '
    ' Description: Call to Post and Allocate Payments.
    '
    ' ***************************************************************** '

    Private Function SettleAllPayments() As Integer

        Const kMethodName As String = "SettleAllPayments"
        Dim sChequeProduction As String = ""
        Dim lReturn As Integer
        Dim lSelectedItemCount As Integer

        Dim iIndex As Short
        Try



            SettleAllPayments = gPMConstants.PMEReturnCode.PMTrue

            If sChequeProduction = "1" Then
                m_bChequeProduction = True
            Else
                m_bChequeProduction = False
            End If

            lReturn = GetNumberOfSelectedItems(lSelectedItemCount)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetNumberOfSelectedItems Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If lSelectedItemCount > 0 Then
                For iIndex = 1 To lvwCommPayments.Items.Count
                    'If lvwCommPayments.ListItems.Item(iIndex).Selected = True Then

                    If lvwCommPayments.Items.Item(iIndex - 1).Checked = True Then
                        lReturn = SettlePayment(iIndex)
                    End If
                Next

                If m_cTotalAmountReferredForAuthorisation > 0 Then
                    lReturn = SaveSummary("Payment Referred for Auth.", m_cTotalAmountReferredForAuthorisation)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "GetNumberOfSelectedItems Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    m_vSummary(MainModule.ACListPaymentSummary.PSPaymentCount, UBound(m_vSummary, 2)) = m_iTotalTransactionsReferredForAuthorisation
                End If

            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetNumberOfSelectedItems Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If lSelectedItemCount > 0 Then
                lReturn = ShowCommissionStatementReport(v_bIsPreview:=False)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Show Commission Statement Report Failed", gPMConstants.PMELogLevel.PMLogError)
                Else
                    m_bNextClicked = True
                End If
            Else
                m_bNextClicked = True
            End If

            lReturn = PerformSearch()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetNumberOfSelectedItems Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                Me.Hide()
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=SettleAllPayments, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SettlePayment
    '
    ' Description: Call to Post and Allocate Payments.
    '
    ' ***************************************************************** '


    Private Function SettlePayment(ByRef iIndex As Short) As Integer

        Const kMethodName As String = "SettlePayment"

        Dim lReturn As Integer
        Dim lSelectedItemCount As Integer
        Dim lCurrencyId As Integer
        Dim lAgentAccountId As Integer
        Dim lMediaTypeId As Integer
        Dim lAccCurrencyId As Integer
        Dim lDocumentId As Integer
        Dim lBankAccountId As Integer
        Dim lBatchId As Integer
        Dim lTransDetailID As Integer
        Dim lCashListId As Integer
        Dim lCashListItemId As Integer
        Dim lRowCount As Integer

        Dim vDocuments As Object
        Dim vClaimPaymentIDs As Object
        Dim vDocumentArray() As Object
        Dim vDocumentTempArray As Object
        Dim vCashListItem As Object
        Dim vKeyArray As Object
        Dim vCashListItemDetails As Object
        Dim vOSTransactions As Object

        Dim iVar As Short
        Dim iSourceID As Short

        Dim oListItem As System.Windows.Forms.ListViewItem

        Dim sAgentAddress As String
        Dim sCommType As String
        Dim sPaymentType As String
        Dim sPayeeName As String
        Dim sAgentName As String
        Dim sMediaType As String

        Dim dStatementDate As Date

        Dim cCommPaymentAmount As Decimal
        Dim cPaymentAuthority As Decimal

        Dim bProceedFurther As Boolean
        Dim bHasPaymentsAuthority As Boolean
        Dim r_bIsDeleted As Boolean
        Dim bProceedtoPost As Boolean

        Dim oCashList As Object
        Dim oCashListItem As Object
        Dim oAccount As Object
        Dim oBankAccount As Object
        Dim oUserAuthorities As Object
        Dim oAllocate As Object
        Dim oCashListPost As Object
        Dim oPartyBank As Object = Nothing

        Try



            SettlePayment = gPMConstants.PMEReturnCode.PMTrue
            'Get the CashList business Object

            lReturn = g_oObjectManager.GetInstance(oObject:=oCashList, sClassName:="bACTCashList.Form", vInstanceManager:=PMGetViaClientManager)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to GetInstance For - bActCashList.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the CashListItem Business Object

            lReturn = g_oObjectManager.GetInstance(oObject:=oCashListItem, sClassName:="bACTCashlistitem.Form", vInstanceManager:=PMGetViaClientManager)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to GetInstance For - bActCashListItem.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the Account Business Object

            lReturn = g_oObjectManager.GetInstance(oObject:=oAccount, sClassName:="bACTAccount.Form", vInstanceManager:=PMGetViaClientManager)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to GetInstance For - bACTAccount.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the BankAccount Business Object

            lReturn = g_oObjectManager.GetInstance(oObject:=oBankAccount, sClassName:="bACTBankAccount.Form", vInstanceManager:=PMGetViaClientManager)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to GetInstance For - bACTBankAccount.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the UserAuthorities Business Object

            lReturn = g_oObjectManager.GetInstance(oObject:=oUserAuthorities, sClassName:="bACTUserAuthorities.Business", vInstanceManager:=PMGetViaClientManager)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to GetInstance For - bACTUserAuthorities.Business.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the UserAuthorities Business Object

            lReturn = g_oObjectManager.GetInstance(oObject:=oAllocate, sClassName:="bACTAllocate.Business", vInstanceManager:=PMGetViaClientManager)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to GetInstance For - bActAllocate.Business.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Create CashListPost
            If oCashListPost Is Nothing Then

                lReturn = g_oObjectManager.GetInstance(oObject:=oCashListPost, sClassName:="bACTCashListPost.Automated", vInstanceManager:=PMGetViaClientManager)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed to create bACTCashListPost.Automated", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            lReturn = GetNumberOfSelectedItems(lSelectedItemCount)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetNumberOfSelectedItems Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            bProceedFurther = True
            bProceedtoPost = True

            ' Get the required details of Agent
            lReturn = GetCommPaymentDetails(iIndex, lAgentAccountId, sAgentAddress, sCommType, sPaymentType, dStatementDate, lMediaTypeId, cCommPaymentAmount, lAccCurrencyId, lDocumentId, lBankAccountId, lBatchId, sAgentName)

            lReturn = g_oObjectManager.GetInstance(oObject:=oPartyBank, sClassName:="bSIRPartyBank.Business", vInstanceManager:=PMGetViaClientManager)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to GetInstance For - bSIRPartyBank.Business.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            Dim oPartyBankDetails(,) As Object = Nothing
            lReturn = oPartyBank.GetPartyBankDetails(oPartyBankDetails, Nothing, lAgentAccountId)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "oPartyBank.GetPartyBankDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = oAccount.IsDeleted(lAgentAccountId, r_bIsDeleted)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "oAccount.IsDeleted Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If r_bIsDeleted = True Then
                bProceedFurther = False
            End If

            m_iReturn = GetUserAuthoritiesForPayment(bHasPaymentsAuthority, cPaymentAuthority)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetUserAuthorities Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If bHasPaymentsAuthority And cPaymentAuthority < cCommPaymentAmount * -1 Then
                bProceedtoPost = False
                'Create a WorkMAnager Task
            End If

            If bProceedFurther Then
                'Add the CashListDetails for payment
                If lBankAccountId <> 0 Then

                    lReturn = oCashList.DirectAdd(vCashListID:=lCashListId, vCashListStatusID:=1, vCashListTypeID:=1, vCashListRef:="", vCompanyID:=1, vBankAccountID:=lBankAccountId, vCurrencyID:=lAccCurrencyId, vListDate:=ToSafeDate(dStatementDate), vControlTotal:=0, vItemCount:=0)

                Else

                    lReturn = oCashList.DirectAdd(vCashListID:=lCashListId, vCashListStatusID:=1, vCashListTypeID:=1, vCashListRef:="", vCompanyID:=1, vCurrencyID:=lAccCurrencyId, vListDate:=ToSafeDate(dStatementDate), vControlTotal:=0, vItemCount:=0)
                End If

                'Add CashListItem Details
                lReturn = PrepareCashListItem(lAgentAccountId, lCashListId, cCommPaymentAmount * -1, lMediaTypeId, lBankAccountId, lAccCurrencyId, vCashListItem, sAgentName,  oPartyBankDetails)


                lReturn = oCashListItem.DirectAdd(r_vCashListItem:=vCashListItem)

                lReturn = oCashListItem.Update

                oListItem = lvwCommPayments.Items.Item(iIndex - 1)

                lCashListItemId = vCashListItem(0)
                ReDim vKeyArray(1, 1)


                vKeyArray(0, 0) = ACTKeyNameCashListId

                vKeyArray(1, 0) = lCashListId

                vKeyArray(0, 1) = ACTKeyNameCashListItemId

                vKeyArray(1, 1) = lCashListItemId


                m_iReturn = oCashListPost.SetKeys(vKeyArray:=vKeyArray)
                If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "oCashListPost.SetKeys Failed", gPMConstants.PMELogLevel.PMLogError)
                End If


                oCashListPost.ChequeProduction = m_bChequeProduction

                m_iReturn = oCashListPost.PostUnallocatedCash(v_vCashListID:=lCashListId, sFailureReason:="")
                If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "oCashListPost.PostUnallocatedCash Failed", gPMConstants.PMELogLevel.PMLogError)
                End If


                m_iReturn = oCashListItem.GetDetails(vCashListID:=lCashListId)
                If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "oCashListItem.GetDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If (m_iReturn = gPMConstants.PMEReturnCode.PMTrue) Then

                    m_iReturn = oCashListItem.GetNext(vCashListItemDetails)

                    lTransDetailID = vCashListItemDetails(MainModule.eCashListItem.TransdetailID)
                End If

                'Create a new function in Business Object and call to fill the document Array

                m_iReturn = GetDocumentsForAccountBatch(v_lAccountId:=lAgentAccountId, v_lBatchID:=lBatchId, r_vDocumentIds:=vDocumentTempArray)

                ReDim vDocumentArray(UBound(vDocumentTempArray, 2))

                For lRowCount = 0 To UBound(vDocumentTempArray, 2)


                    vDocumentArray(lRowCount) = vDocumentTempArray(0, lRowCount)
                Next

                If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetDocumentsForAccountBatch Failed", gPMConstants.PMELogLevel.PMLogError)
                End If


                m_iReturn = oAccount.GetAccountOSCommForDocuments(v_lAccountId:=lAgentAccountId, v_vDocumentIds:=vDocumentArray, r_vOSTransactions:=vOSTransactions)

                If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetAccountOSTransForDocuments Failed", gPMConstants.PMELogLevel.PMLogError)
                End If


                If oAllocate.PerformAutoAllocation(r_lAccountid:=lAgentAccountId, r_lTransdetailid:=lTransDetailID, v_vOSTransactions:=vOSTransactions, v_lCashlistItemID:=lCashListItemId) <> gPMConstants.PMEReturnCode.PMTrue Then

                    If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "GetAccountOSTransForDocuments Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Else
                    m_iReturn = SaveSummary(sMediaType, cCommPaymentAmount * -1)
                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=SettlePayment, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oAccount.Dispose()

            oAccount = Nothing

            oAllocate.Dispose()

            oAllocate = Nothing

            oBankAccount.Dispose()

            oBankAccount = Nothing

            oCashList.Dispose()

            oCashList = Nothing

            oCashListItem.Dispose()

            oCashListItem = Nothing

            oUserAuthorities.Dispose()

            oUserAuthorities = Nothing

            oCashListPost.Dispose()
            oCashListPost = Nothing



        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetNumberOfSelectedItems
    '
    ' Parameters:
    '
    ' Description:
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Function GetNumberOfSelectedItems(ByRef r_lSelectedItemCount As Integer) As Integer

        Const kMethodName As String = "GetNumberOfSelectedItems"

        Dim lReturn As Integer
        Dim lCommPayment As Integer
        Dim nCommPayments As Integer

        Try



            GetNumberOfSelectedItems = gPMConstants.PMEReturnCode.PMTrue

            ' get number of unallocated claim payments in the list
            nCommPayments = lvwCommPayments.Items.Count

            ' if there are unallocatd claims payments
            If nCommPayments > 0 Then
                ' for each claim payment item in the list
                For lCommPayment = 1 To nCommPayments
                    ' if the claim payment item is selected

                    If lvwCommPayments.Items.Item(lCommPayment - 1).Checked Then
                        ' increment selected item count
                        r_lSelectedItemCount = r_lSelectedItemCount + 1
                    End If
                Next
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetNumberOfSelectedItems, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Exit Function

    End Function

    ' ***************************************************************** '
    ' Name: GetCommPaymentDetails
    '
    ' Parameters:
    '
    ' Description:
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Function GetCommPaymentDetails(ByVal v_lListItemIndex As Integer, ByRef r_lAgentAccountId As Integer, ByRef r_sAgentAddress As String, ByRef r_sCommType As String, ByRef r_sPaymentType As String, ByRef r_dStatementDate As Date, ByRef r_lMediaTypeId As Integer, ByRef r_cCommPaymentAmount As Decimal, ByRef r_lAccCurrencyId As Integer, ByRef r_lDocumentId As Integer, ByRef r_lBankAccountId As Integer, ByRef r_lBatchID As Integer, ByRef r_sAgentName As String) As Integer

        Const kMethodName As String = "GetCommPaymentDetails"

        Dim lReturn As Integer
        Dim oListItem As System.Windows.Forms.ListViewItem
        Dim m_vAgentData As Object

        Try



            GetCommPaymentDetails = gPMConstants.PMEReturnCode.PMTrue

            ' get the selected claim payment item

            oListItem = lvwCommPayments.Items.Item(v_lListItemIndex - 1)

            r_lAgentAccountId = ToSafeLong(m_vSearchData(k_AccountId, oListItem.Tag))


            m_iReturn = m_oBusiness.GetAgentDetailsforPayments(v_lAccountId:=r_lAgentAccountId, v_lSourceId:=g_iSourceID, r_vResultArray:=m_vAgentData)

            r_sAgentAddress = ToSafeString(m_vAgentData(k_AgentAddress, 0), CStr(0))
            r_sCommType = "Payment"
            r_sPaymentType = "Commission"

            If ToSafeDate(dateStatementDate.Value, CDate("00:00:00")) <> CDate("00:00:00") Then
                r_dStatementDate = ToSafeDate(dateStatementDate.Value, CDate("00:00:00"))
            End If

            r_lMediaTypeId = ToSafeLong(m_vAgentData(k_AgentMedia_Type, 0), 0)
            r_cCommPaymentAmount = ToSafeCurrency(m_vSearchData(k_TotalComm, oListItem.Tag), 0)
            r_sAgentName = ToSafeString(m_vSearchData(k_AgentName, oListItem.Tag), CStr(0))
            r_lAccCurrencyId = ToSafeLong(m_vSearchData(k_CurrencyID, oListItem.Tag), 0)
            r_lDocumentId = 0
            r_lBankAccountId = ToSafeLong(m_vAgentData(k_Bank_Account_Id, 0), 0)
            r_lBatchID = m_iBatchId


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetCommPaymentDetails, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Exit Function

    End Function
    ' ***************************************************************** '
    ' Name: GetUserAuthoritiesForPayment
    '
    ' Parameters:
    '
    ' Description: Get User Authorities for performing Payments
    '
    ' History:
    '
    ' ***************************************************************** '

    Private Function GetUserAuthoritiesForPayment(ByRef r_bHasPaymentsAuthority As Boolean, ByRef r_cPaymentsAmount As Decimal) As Integer

        Const kMethodName As String = "GetUserAuthoritiesForPayment"
        Dim vResults As Object
        Dim crConvertedCurrency As Decimal
        Dim iPaymentsCurrencyID As Short

        Try



            GetUserAuthoritiesForPayment = gPMConstants.PMEReturnCode.PMTrue



            m_iReturn = m_oUserAuthorities.GetDetails(vUserId:=g_oObjectManager.UserID)
            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Unable to get User Authorities Details")
            End If


            m_iReturn = m_oUserAuthorities.GetNext(vHasPaymentsAuthority:=r_bHasPaymentsAuthority, vPaymentsAmount:=r_cPaymentsAmount, vPaymentsCurrencyID:=iPaymentsCurrencyID)


            If r_bHasPaymentsAuthority = True And g_oObjectManager.CurrencyID <> iPaymentsCurrencyID Then


                m_iReturn = m_obACTCurrencyConvert.CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=iPaymentsCurrencyID, v_crCurrencyAmountFrom:=r_cPaymentsAmount, v_lCompanyID:=g_iSourceID, v_lCurrencyIdTo:=g_oObjectManager.CurrencyID, r_crCurrencyAmountTo:=crConvertedCurrency)

                If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("GetUserAuthorities", "CurrencyToCurrencyConversion Failed")
                End If

                r_cPaymentsAmount = crConvertedCurrency
            End If



        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetUserAuthoritiesForPayment, excep:=ex)

        Finally



        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PrepareCashListItem
    '
    ' Parameters:
    '
    ' Description: Prepare CashList Item
    '
    ' History:
    '
    ' ***************************************************************** '

    Private Function PrepareCashListItem(ByVal lAccountID As Integer, ByVal lCashListId As Integer, ByVal cAmount As Decimal, ByVal iMediaTypeId As Short, ByVal iBankAccountId As Short, ByVal iCurrencyId As Short, ByRef r_vCashListItem As Object, Optional ByVal v_sPayeeName As String = "",Optional ByVal vPartyBankDetails(,) As Object = Nothing) As Integer

        Const kMethodName As String = "CreateCashListPayment"

        Dim iBaseCurrencyID As Short
        Dim iAccountCurrencyID As Short
        Dim iSystemCurrencyID As Short

        Dim cBaseCurrentAmount As Decimal
        Dim cAccountCurrentAmount As Decimal
        Dim cSystemCurrentAmount As Decimal

        Dim dTransToBaseExchangeRate As Double
        Dim dAccountToBaseExchangeRate As Double
        Dim dSystemToBaseExchangeRate As Double

        Dim oCurrencyBusiness As Object
        Try



            PrepareCashListItem = gPMConstants.PMEReturnCode.PMTrue

            ReDim r_vCashListItem(MainModule.eCashListItem.LastItem)

            'Get business object.

            m_iReturn = g_oObjectManager.GetInstance(oObject:=oCurrencyBusiness, sClassName:="bACTCurrencyConvert.Form", vInstanceManager:=PMGetViaClientManager)

            If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("", "Unable to get bACTCurrencyConvert object")
            End If



            m_iReturn = oCurrencyBusiness.DoCurrencyConversion(v_lAccountId:=lAccountID, v_lCompanyID:=g_oObjectManager.SourceID, v_iCurrencyID:=iCurrencyId, v_cCurrencyAmountUnrounded:=cAmount, r_iBaseCurrencyID:=iBaseCurrencyID, r_cBaseAmount:=cBaseCurrentAmount, r_iAccountCurrencyID:=iAccountCurrencyID, r_cAccountAmount:=cAccountCurrentAmount, r_iSystemCurrencyID:=iSystemCurrencyID, r_cSystemAmount:=cSystemCurrentAmount, r_dCurrencyBaseXrate:=dTransToBaseExchangeRate, r_dtCurrencyBaseDate:=Now, r_dAccountBaseXrate:=dAccountToBaseExchangeRate, r_dtAccountBaseDate:=Now, r_dSystemBaseXrate:=dSystemToBaseExchangeRate, r_dtSystemBaseDate:=Now)


            r_vCashListItem(MainModule.eCashListItem.CashlistitemID) = 0

            r_vCashListItem(MainModule.eCashListItem.AllocationstatusID) = 1

            r_vCashListItem(MainModule.eCashListItem.MediaTypeID) = iMediaTypeId

            r_vCashListItem(MainModule.eCashListItem.CashlistID) = lCashListId

            r_vCashListItem(MainModule.eCashListItem.AccountId) = lAccountID

            r_vCashListItem(MainModule.eCashListItem.MediaRef) = ""

            r_vCashListItem(MainModule.eCashListItem.OurRef) = ""

            r_vCashListItem(MainModule.eCashListItem.TheirRef) = ""

            r_vCashListItem(MainModule.eCashListItem.Amount) = cAmount


            r_vCashListItem(MainModule.eCashListItem.AddressCountry) = "1"

            r_vCashListItem(MainModule.eCashListItem.Transaction_Date) = Now

            r_vCashListItem(MainModule.eCashListItem.Amount_Tendered) = -cAmount


            r_vCashListItem(MainModule.eCashListItem.CashListItem_Payment_Type_id) = 2

            r_vCashListItem(MainModule.eCashListItem.CashListItem_Payment_Status_id) = 1 '


            r_vCashListItem(MainModule.eCashListItem.CurrencyBaseDate) = Now

            r_vCashListItem(MainModule.eCashListItem.CurrencyBaseXrate) = dTransToBaseExchangeRate

            r_vCashListItem(MainModule.eCashListItem.AccountBaseDate) = Now

            r_vCashListItem(MainModule.eCashListItem.AccountBaseXrate) = dAccountToBaseExchangeRate

            r_vCashListItem(MainModule.eCashListItem.SystemBaseDate) = Now

            r_vCashListItem(MainModule.eCashListItem.SystemBaseXrate) = dSystemToBaseExchangeRate

            r_vCashListItem(MainModule.eCashListItem.OverrideReason) = 0

            r_vCashListItem(MainModule.eCashListItem.PaymentName) = v_sPayeeName

            If Not Information.IsNothing(vPartyBankDetails) AndAlso IsArray(vPartyBankDetails) Then
                If Not Information.IsNothing(vPartyBankDetails(2, 0)) Then
                    r_vCashListItem(eCashListItem.PartyBankId) = vPartyBankDetails(2, 0)
                End If
                r_vCashListItem(eCashListItem.PaymentAccountCode) = vPartyBankDetails(8, 0)
                r_vCashListItem(eCashListItem.PaymentBranchCode) = vPartyBankDetails(11, 0)
                If Not Information.IsNothing(vPartyBankDetails(18, 0)) AndAlso IsArray(vPartyBankDetails(18, 0)) Then
                    r_vCashListItem(eCashListItem.AddressCountry) = vPartyBankDetails(18, 0)(0)
                End If
                r_vCashListItem(eCashListItem.BIC) = vPartyBankDetails(37, 0)
                r_vCashListItem(eCashListItem.IBAN) = vPartyBankDetails(38, 0)
            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=PrepareCashListItem, excep:=ex)

        Finally

            oCurrencyBusiness.Dispose()

            oCurrencyBusiness = Nothing


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveSummary
    '
    ' Parameters:
    '
    ' Description: After Performing the Allocation save a Summary
    '
    ' History:
    '
    ' ***************************************************************** '

    Private Function SaveSummary(ByRef sMediaType As String, ByRef cAmount As Decimal) As Integer

        Const kMethodName As String = "SaveSummary"

        Dim i As Short
        Dim iIndex As Short

        Dim itemExists As Boolean

        Try



            SaveSummary = gPMConstants.PMEReturnCode.PMTrue

            If IsArray(m_vSummary) Then

                iIndex = UBound(m_vSummary, 2) + 1

                For i = 0 To UBound(m_vSummary, 2)

                    If m_vSummary(MainModule.ACListPaymentSummary.PSMediaType, i) = sMediaType Then

                        m_vSummary(MainModule.ACListPaymentSummary.PSPaymentValue, i) = m_vSummary(MainModule.ACListPaymentSummary.PSPaymentValue, i) + cAmount

                        m_vSummary(MainModule.ACListPaymentSummary.PSPaymentCount, i) = m_vSummary(MainModule.ACListPaymentSummary.PSPaymentCount, i) + 1
                        itemExists = True
                    End If
                Next

                If Not itemExists Then
                    ReDim Preserve m_vSummary(2, iIndex)

                    m_vSummary(MainModule.ACListPaymentSummary.PSMediaType, iIndex) = sMediaType

                    m_vSummary(MainModule.ACListPaymentSummary.PSPaymentValue, iIndex) = cAmount

                    m_vSummary(MainModule.ACListPaymentSummary.PSPaymentCount, iIndex) = 1
                End If
            Else
                ReDim m_vSummary(2, 0)

                m_vSummary(MainModule.ACListPaymentSummary.PSMediaType, 0) = sMediaType

                m_vSummary(MainModule.ACListPaymentSummary.PSPaymentValue, 0) = cAmount

                m_vSummary(MainModule.ACListPaymentSummary.PSPaymentCount, 0) = 1
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=SaveSummary, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocumentsForAccountBatch
    '
    ' Parameters:
    '
    ' Description: Get List of the Documents For the selected Account ID and Batch ID
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Function GetDocumentsForAccountBatch(ByVal v_lAccountId As Integer, ByVal v_lBatchID As Integer, ByRef r_vDocumentIds As Object) As Integer

        Const kMethodName As String = "GetDocumentsForAccountBatch"

        Dim lReturn As Integer

        Dim oListItem As System.Windows.Forms.ListViewItem

        Dim m_vAgentData As Object

        Try



            GetDocumentsForAccountBatch = gPMConstants.PMEReturnCode.PMTrue


            m_iReturn = m_oBusiness.GetDocumentsForAccountBatch(v_lAccountId:=v_lAccountId, v_lBatchID:=v_lBatchID, r_vResultArray:=r_vDocumentIds)
            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "m_oBusiness.GetDocumentsForAccountBatch Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetDocumentsForAccountBatch, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Exit Function

    End Function

    ' ***************************************************************** '
    ' Name: RemoveComissionBatch
    '
    ' Parameters:
    '
    ' Description:
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Function RemoveComissionBatch(ByVal v_lBatchID As Integer) As Integer

        Const kMethodName As String = "RemoveComissionBatch"
        Dim lReturn As Integer
        Try


            RemoveComissionBatch = gPMConstants.PMEReturnCode.PMTrue

            m_iReturn = m_oBusiness.RemoveCommissionPaymentsBatch(v_lBatchID:=v_lBatchID)
            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "m_oBusiness.RemoveCommissionPaymentsBatch Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=RemoveComissionBatch, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Exit Function

    End Function
    ''WR05

    ' ***************************************************************** '
    ' Name: ShowCommissionStatementReport
    '
    ' Description: Shows Commission Statement Report.
    '
    ' ***************************************************************** '
    Private Function ShowCommissionStatementReport(ByVal v_bIsPreview As Boolean) As Integer

        Dim i As Short
        Dim oReport As Object

        Dim vKeyArray(1, 11) As Object
        Try


            'Method Code folows
            ShowCommissionStatementReport = gPMConstants.PMEReturnCode.PMTrue

            m_iReturn = g_oObjectManager.GetInstance(oObject:=oReport, sClassName:="iPMBReportPrint.Interface_Renamed", vInstanceManager:=PMGetLocalInterface)

            With oReport
                ' Send Report & Parameters into Report via SetKeys()

                vKeyArray(0, 0) = PMKeyNameReportName '"report_name"

                vKeyArray(1, 0) = "Commission_Statement"


                vKeyArray(0, 1) = PMKeyNamePrintReport '"report_print_options"

                vKeyArray(1, 1) = AC_VIEW_ONLY 'AC_PRINT_AND_VIEW

                'Submit (Account Id) to generate raport.

                vKeyArray(0, 2) = PMKeyNameParam1Name '"param_name1"

                vKeyArray(1, 2) = "batch_id"


                vKeyArray(0, 3) = "batch_id"

                vKeyArray(1, 3) = "CMS" & m_iBatchId

                'Submit (Date to) Parameter to generate report.

                vKeyArray(0, 4) = PMKeyNameParam2Name '"param_name2"

                vKeyArray(1, 4) = "Start_date"


                vKeyArray(0, 5) = "Start_date"
                If ToSafeDate(dateStatementDate.Value, CDate("00:00:00")) <> CDate("00:00:00") Then

                    vKeyArray(1, 5) = ToSafeDate(dateStatementDate.Value, CDate("00:00:00")).ToString("MMM dd yyyy")
                End If


                vKeyArray(0, 6) = PMKeyNameParam3Name '"param_name3"

                vKeyArray(1, 6) = "is_preview"


                vKeyArray(0, 7) = "is_preview"
                If v_bIsPreview Then

                    vKeyArray(1, 7) = True
                Else

                    vKeyArray(1, 7) = False
                End If


                m_iReturn = .SetKeys(vKeyArray:=vKeyArray)
                If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    RaiseError("ShowCommissionStatementReport", "oReport.SetKeys Failed", gPMConstants.PMELogLevel.PMLogError)
                End If


                m_iReturn = .Start()
                If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    RaiseError("ShowCommissionStatementReport", "oReport.Start Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End With


        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="ShowCommissionStatementReport", r_lFunctionReturn:=ShowCommissionStatementReport, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally

            oReport.Dispose()

            oReport = Nothing

        End Try
        Exit Function
    End Function

    ' ***************************************************************** '
    '
    ' Name: UnlockAgentForCommissionPayment
    '
    ' Description:
    '
    ' History:
    '
    ' ***************************************************************** '
    Private Function UnlockAgentForCommissionPayment() As Integer

        Dim oPMLock As Object
        Dim cntRow As Integer

        Try



            UnlockAgentForCommissionPayment = gPMConstants.PMEReturnCode.PMTrue

            'Get bPMLock

            m_iReturn = g_oObjectManager.GetInstance(oObject:=oPMLock, sClassName:="bPMLock.User", vInstanceManager:=PMGetViaClientManager)

            ' Check for errors.
            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError("UnlockAgentForCommissionPayment", "g_oObjectManager.GetInstance Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If IsArray(m_vMarkAgentId) Then
                'If UBound(m_vMarkAgentId) > 0 Then
                For cntRow = 0 To UBound(m_vMarkAgentId)


                    m_iReturn = oPMLock.UnlockKey(sKeyName:=ACLockName, vKeyValue:=m_vMarkAgentId(cntRow), iUserID:=g_oObjectManager.UserID)

                    If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        RaiseError("UnlockAgentForCommissionPayment", "oPMLock.UnlockKey Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Next
            End If


        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="UnlockAgentForCommissionPayment", r_lFunctionReturn:=UnlockAgentForCommissionPayment, v_sUsername:=g_sUsername.Value, excep:=ex)


        Finally

            oPMLock.Dispose()

            oPMLock = Nothing
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Exit Function
    End Function

    ' ***************************************************************** '
    '
    ' Name: LockAgentForCommissionPayment
    '
    ' Description:
    '
    ' History:
    '
    ' ***************************************************************** '
    Private Function LockAgentForCommissionPayment(ByVal v_lAgentID As Integer, ByRef r_sLockedBy As String) As Integer

        Dim oPMLock As Object
        Dim sLockedBy As String
        Dim sPartyShortName As String

        Try



            LockAgentForCommissionPayment = gPMConstants.PMEReturnCode.PMTrue

            'Get bPMLock

            m_iReturn = g_oObjectManager.GetInstance(oObject:=oPMLock, sClassName:="bPMLock.User", vInstanceManager:=PMGetViaClientManager)

            ' Check for errors.
            If (m_iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError("LockAgentForCommissionPayment", "g_oObjectManager.GetInstance Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_iReturn = oPMLock.LockKey(sKeyName:=ACLockName, vKeyValue:=v_lAgentID, iUserID:=g_iUserID, sCurrentlyLockedBy:=sLockedBy)


            Select Case m_iReturn

                Case gPMConstants.PMEReturnCode.PMTrue
                    'OK

                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    LockAgentForCommissionPayment = gPMConstants.PMEReturnCode.PMFalse
                    If (sLockedBy = "ERROR") Then
                        RaiseError("LockAgentForCommissionPayment", "oPMLock.LockKey Failed", gPMConstants.PMELogLevel.PMLogError)
                    Else

                        m_iReturn = m_oBusiness.GetShortNameForParty(v_lPartyCnt:=v_lAgentID, r_sPartyShortName:=sPartyShortName)
                        If m_iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError("LockAgentForCommissionPayment", "m_oBusiness.GetShortNameForParty Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        r_sLockedBy = r_sLockedBy & "Agent " & "'" & sPartyShortName & "'" & " locked for Commission Payments by '" & sLockedBy & " '. Processing cannot be done on this Account." & vbCrLf
                    End If

                Case Else
                    RaiseError("LockAgentForCommissionPayment", "oPMLock.LockKey Failed", gPMConstants.PMELogLevel.PMLogError)

            End Select


        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="LockAgentForCommissionPayment", r_lFunctionReturn:=LockAgentForCommissionPayment, v_sUsername:=g_sUsername.Value, excep:=ex)


        Finally

            oPMLock.Dispose()

            oPMLock = Nothing
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Exit Function
    End Function

    Private Function SaveSelection() As Integer
        Const kMethodName As String = "SaveSelection"
        Dim vCriteria(1, 8) As Object
        Dim sAgentIds As String
        Dim iVar As Short
        Try


            SaveSelection = gPMConstants.PMEReturnCode.PMTrue

            vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMKeySRCHStatementDate


            vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = dateStatementDate.Value
            If ToSafeDate(dateFrom.Value, CDate("00:00:00")) <> CDate("00:00:00") And dateFrom.Checked = True Then

                vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMKeySRCHTransDateFrom

                vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = ToSafeDate(dateFrom.Value, CDate("00:00:00"))
            Else

                vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMKeySRCHTransDateFrom

                vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = ""
            End If

            If ToSafeDate(dateTo.Value, CDate("00:00:00")) <> CDate("00:00:00") And dateTo.Checked = True Then

                vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMKeySRCHTransDateTo

                vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = ToSafeDate(dateTo.Value, CDate("00:00:00"))
            Else

                vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMKeySRCHTransDateTo

                vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = ""
            End If

            vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMKeySRCHCurrencyItemID



            vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = ToSafeLong(cboCurrency.ItemData(cboCurrency.ListIndex))


            vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMKeySRCHProductItemID

            vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = ToSafeLong(cmbProduct.ItemData(cmbProduct.ListIndex))

            vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMKeySRCHBranchItemID

            vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = ToSafeLong(cmbBranch.ItemData(cmbBranch.ListIndex))


            vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMKeySRCHTransAuthLimit
            If chkTransAuthLimit.CheckState = System.Windows.Forms.CheckState.Checked Then

                vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = 1
            Else

                vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = 0
            End If


            sAgentIds = m_vMarkAgentId(0)

            For iVar = 1 To UBound(m_vMarkAgentId)
                sAgentIds = sAgentIds & "|" & ToSafeString(m_vMarkAgentId(iVar))
            Next


            vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = "SearchResults"

            vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = sAgentIds


            vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMKeySRCHAutoSearch

            vCriteria(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = 1



            m_oBusiness.SaveSelection(vCriteria)



        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="SaveSelection", r_lFunctionReturn:=SaveSelection, v_sUsername:=g_sUsername.Value, excep:=ex)


        Finally


        End Try
        Exit Function
    End Function


    Private Sub frmInterface_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize

        Const kMethodName As String = "Form_Resize"
        Try


            Call ResizeInterface()


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
        Finally


        End Try
    End Sub
    Private Function CheckForDuplicateAgent(ByVal v_lIndex As Integer, ByRef v_bDuplicate As Boolean) As Integer

        Const kMethodName As String = "CheckForDuplicateAgent"
        Dim lRow As Integer
        Dim lAccountID As Integer
        Try


            CheckForDuplicateAgent = gPMConstants.PMEReturnCode.PMTrue
            lAccountID = ToSafeLong(m_vSearchData(k_AccountId, v_lIndex))

            For lRow = 0 To UBound(m_vSearchData, 2)
                If v_lIndex <> lRow Then
                    If lAccountID = ToSafeLong(m_vSearchData(k_AccountId, lRow)) Then
                        v_bDuplicate = True
                        Exit For
                    End If
                End If
            Next lRow



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CheckForDuplicateAgent, excep:=ex)
        Finally


        End Try
    End Function

    Private Function ResizeInterface() As Integer

        Const BUTTON_HEIGHT As Short = 720
        Const BUTTON_WIDTH As Short = 1335

        Dim lResizeWidth As Integer
        Dim lResizeHeight As Integer

        Try


            ResizeInterface = gPMConstants.PMEReturnCode.PMTrue
            If VB6.PixelsToTwipsX(Me.Width) < 10900 Then
                Me.Width = VB6.TwipsToPixelsX(10900)
            End If
            If VB6.PixelsToTwipsY(Me.Height) < 6000 Then
                Me.Height = VB6.TwipsToPixelsY(6000)
            End If

            lResizeWidth = VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - BUTTON_WIDTH
            lResizeHeight = VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - BUTTON_HEIGHT

            cmdFindNow.Left = VB6.TwipsToPixelsX(lResizeWidth - 50)
            cmdNewSearch.Left = VB6.TwipsToPixelsX(lResizeWidth - 50)

            cmdNext.Left = VB6.TwipsToPixelsX(lResizeWidth - VB6.PixelsToTwipsX(cmdCancel.Width) - 300)
            cmdCancel.Left = VB6.TwipsToPixelsX(lResizeWidth - 50)

            cmdCancel.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - 760)
            cmdNext.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - 760)

            cmdPreviewStatement.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - 760)
            cmdSave.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - 760)

            fraMainFilter.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdFindNow.Left) - 200)

            lvwCommPayments.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - 500)
            lvwCommPayments.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(lvwCommPayments.Top) - 1000)



        Catch ex As Exception
            'Do nothing
        Finally

        End Try
    End Function
    Private Sub lvwCommPayments_ItemCheck(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lvwCommPayments.ItemCheck
        e.NewValue = e.CurrentValue
    End Sub

    Private Sub chkCommForAllctdTrans_CheckedChanged(sender As Object, e As EventArgs) Handles chkCommForAllctdTrans.CheckedChanged
        If chkCommForAllctdTrans.Checked Then
            txtLeadDays.Visible = True
            lblLeadDays.Visible = True
            lblAlctdBfr.Visible = True
        Else
            lblLeadDays.Visible = False
            lblAlctdBfr.Visible = False
            txtLeadDays.Visible = False
            txtLeadDays.Text = ""
        End If
    End Sub

    Private Sub txtLeadDays_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLeadDays.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) AndAlso (e.KeyChar <> "."c) Then
            e.Handled = True
        End If

        If (e.KeyChar = "."c) AndAlso ((TryCast(sender, TextBox)).Text.IndexOf("."c) > -1) Then
            e.Handled = True
        End If
    End Sub
End Class
