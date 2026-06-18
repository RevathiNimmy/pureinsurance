Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
'Develoer Guide no 129
Imports SharedFiles
Partial Friend Class frmInterface
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

    '********************************
    ' General Property variables
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lPMAuthorityLevel As Integer
    Private m_bError As Integer

    Private m_oBusiness As bACTAccount.Form
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_bInterfaceError As Boolean
    '********************************

    Private m_lAccountId As Integer
    Private m_dtDateOfPaymentFrom As Date
    Private m_dtDateOfPaymentTo As Date
    Private m_oFindAccount As Object
    Private m_vUnallocatedClaimPayments(,) As Object
    Private m_sAccountCode As String = ""
    Private m_sAccountName As String = ""
    Private m_bSelected As Boolean

    Private WithEvents m_oNavStart As iPMNavStart.Interface_Renamed
    Private m_bNavCompleted As Boolean
    Private m_bProcessComplete As Boolean

    Private m_oUserAuthorities As bACTUserAuthorities.Business

    Private m_obACTCurrencyConvert As bACTCurrencyConvert.Form
    Private m_cTotalAmountReferredForAuthorisation As Decimal
    Private m_iTotalTransactionsReferredForAuthorisation As Integer
    Private m_oFormFields As iPMFormControl.FormFields
    Private m_vSummary(,) As Object
    Private m_bChequeProduction As Boolean

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click
        ProcessFind()
    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click
        lvwClaimPayments.Items.Clear()
        txtAccountCode.Text = ""
        txtAccountName.Text = ""
        txtDateOfPaymentFrom.Text = ""
        txtDateOfPaymentTo.Text = ""
        txtAccountCode.Enabled = True
        txtDateOfPaymentFrom.Enabled = True
        txtDateOfPaymentTo.Enabled = True
        txtDateOfPaymentFrom.BackColor = SystemColors.Window
        txtDateOfPaymentTo.BackColor = SystemColors.Window
        txtAccountCode.BackColor = SystemColors.Window
        txtAccountCode.Focus()
    End Sub
    'WR05 - Settle All the Selected Payment one by one
    Private Sub cmdSettleAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSettleAll.Click

        Const kMethodName As String = "cmdSettleAll_Click"

        Dim lReturn, lSubValue As Integer

        Try



            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_cTotalAmountReferredForAuthorisation = 0
            m_iTotalTransactionsReferredForAuthorisation = 0

            m_lReturn = SettleAllPayments()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SettleAllPayments Failed")
            End If
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            m_lReturn = ShowSummary()

            ' refresh the unallocated claim payment items
            'Developer guide No 113
            If gPMFunctions.ToSafeDate(txtDateOfPaymentFrom.Text, #12/30/1989#) <> #12/30/1989# And gPMFunctions.ToSafeDate(txtDateOfPaymentTo.Text, #12/30/1989#) <> #12/30/1989# Then
                lReturn = ProcessUnallocatedClaimPayments(True)
            Else
                lReturn = ProcessUnallocatedClaimPayments(False)
            End If

            m_vSummary = Nothing



        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)
        Finally
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Exit Sub
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If
        ResizeMe()
    End Sub


    'Developer Guide No 273
    Private Sub lvwClaimPayments_ItemChecked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwClaimPayments.ItemChecked
        Dim Item As ListViewItem = lvwClaimPayments.Items(e.Item.Index)
        ProcessItemChecked()
    End Sub

    ' ***************************************************************** '
    ' Region: m_oNavStart Events
    ' ***************************************************************** '
    Private Sub m_oNavStart_NavigatorClose() Handles m_oNavStart.NavigatorClose
        m_bNavCompleted = True
    End Sub

    Private Sub m_oNavStart_SetProcessStatus(ByVal v_bProcessComplete As Boolean) Handles m_oNavStart.SetProcessStatus
        m_bProcessComplete = v_bProcessComplete
    End Sub

    '********************************
    ' General Interface Properties
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
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
            Return m_lStatus
        End Get
    End Property
    Public ReadOnly Property Error_Renamed() As Integer
        Get
            Return m_bError
        End Get
    End Property
    '********************************

    'UPGRADE_NOTE: (7001) The following declaration (cmdAccountCode_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdAccountCode_Click()
    'ProcessFindAccountWrapper()
    'End Sub

    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click
        Me.Hide()
    End Sub

    Private Sub cmdPayAsOne_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPayAsOne.Click
        ProcessMakeSinglePayment()
    End Sub

    Private Sub cmdPayIndividually_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPayIndividually.Click
        ProcessMakeMultiplePayments()
    End Sub

    Private Sub cmdSelectAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelectAll.Click
        ActionSelectAll()
    End Sub

    ' ***************************************************************** '
    ' Name: Form_Initialize
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 31-01-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Sub Form_Initialize_Renamed()

        Const kMethodName As String = "Form_Initialize"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSubValue As Integer

        Try



            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' initialise form error indicator
            m_bError = False

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of BUSINESS", gPMConstants.PMELogLevel.PMLogError)
            End If

            Dim temp_m_oUserAuthorities As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oUserAuthorities = temp_m_oUserAuthorities
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("kMethodName", "Failed to create instance of UserAuthorities")
            End If

            Dim temp_m_obACTCurrencyConvert As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_obACTCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_obACTCurrencyConvert = temp_m_obACTCurrencyConvert
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("kMethodName", "Failed to create instance of CurrencyConvert")
            End If

            m_oFormFields = New iPMFormControl.FormFields()
            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID


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
    '           Created : MEvans : Date : Process Id
    ' ***************************************************************** '
    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
        Try



            m_oUserAuthorities.Dispose()
            ' Check for errors.


            m_obACTCurrencyConvert.Dispose()
            ' Check for errors.

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
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
    '           Created : MEvans : 31-01-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        Const kMethodName As String = "Form_Unload"

        Dim lReturn, lSubValue As Integer

        Try



            ' destroy instance of find account
            m_oFindAccount = Nothing

            ' Terminate the business object

            m_oBusiness.Dispose()
            ' Check for errors.


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
    '           Created : MEvans : 31-01-2006 : Claims Document Production
    ' ***************************************************************** '

    Public Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Const kMethodName As String = "Form_Load"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSubValue As Integer

        Try



            iPMFunc.ShowFormInTaskBar_Detach()

            ' set up interface
            lReturn = SetupForm()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupForm Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Validate fields using Forms Control
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form_Load", "SetFieldValidation Failed")
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: SetupForm
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 31-01-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function SetupForm() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupForm"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = SetupClaimPaymentListView()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupClaimPaymentListView Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetUnallocatedClaimPayments
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 31-01-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function GetUnallocatedClaimPayments(ByVal v_lAccountID As Integer, Optional ByVal v_bSearchByPaymentDate As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetUnallocatedClaimPayments"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If Not v_bSearchByPaymentDate Then
                ' get any unallocated claim payments for the account

                lReturn = m_oBusiness.GetUnallocatedClaimPayments(v_lAccountID, r_vResults:=m_vUnallocatedClaimPayments)
            Else
                ' get any unallocated claim payments for the account

                lReturn = m_oBusiness.GetUnallocatedClaimPaymentsForPaymentDate(m_dtDateOfPaymentFrom, m_dtDateOfPaymentTo, r_vResults:=m_vUnallocatedClaimPayments)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    MessageBox.Show("There are no unallocated claim payments for this account", "Claim Payment Processing Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    gPMFunctions.RaiseError(kMethodName, "bACTAccount.Form.GetUnallocatedClaimPayments Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessFindAccount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function ProcessFindAccount() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessFindAccount"

        Dim lReturn As Integer
        Dim sAccountCode As String = ""
        Dim vKeyArray(,) As Object
        Dim bAllowStopped As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' dont allow the find account to pick up stopped accounts
            bAllowStopped = False

            ' get an instance of FindAccount
            lReturn = GetFindAccount()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetFindAccount Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' ensure the account id is always cleared down before starting the interface

            m_oFindAccount.AccountID = 0

            'Setting the NotEditable property of FindAccount to avoid editing of account details

            m_oFindAccount.NotEditable = True

            ' get the account code that has been specified
            sAccountCode = txtAccountCode.Text.Trim()

            ' define the set key array

            ReDim vKeyArray(1, 1)


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyAllowStoppedAccounts

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = bAllowStopped


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameShortCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = sAccountCode

            ' set the keys

            lReturn = m_oFindAccount.SetKeys(vKeyArray)

            ' start the find account interface

            lReturn = m_oFindAccount.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iACTFindAccount.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if the user didn't cancel from find account then store the new data

            If m_oFindAccount.Status <> gPMConstants.PMEReturnCode.PMCancel Then


                m_sAccountCode = m_oFindAccount.ShortCode

                m_lAccountId = m_oFindAccount.AccountID

                m_sAccountName = m_oFindAccount.AccountName

                txtAccountCode.Text = m_sAccountCode
                txtAccountName.Text = m_sAccountName

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetFindAccount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 31-01-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function GetFindAccount() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetFindAccount"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if the find account component has "NOT" already been created
            If m_oFindAccount Is Nothing Then

                ' attempt to get an instance of the find account component
                Dim temp_m_oFindAccount As Object
                lReturn = g_oObjectManager.GetInstance(temp_m_oFindAccount, sClassName:="iACTFindAccount.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oFindAccount = temp_m_oFindAccount

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to get instance of iACTFindAccount.Interface", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessFindAccountWrapper
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 31-01-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function ProcessFindAccountWrapper() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessFindAccountWrapper"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSelectedAccountId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' clear down the claim payments when attempting to find a new account
            lvwClaimPayments.Items.Clear()

            ' get the new account id
            lReturn = CType(ProcessFindAccount(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ProcessFindAccount Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get any unallocated claim payments for the specified account
            lReturn = ProcessUnallocatedClaimPayments()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetUnallocatedClaimPayments Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ProcessUnallocatedClaimPayments
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 31-01-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function ProcessUnallocatedClaimPayments(Optional ByVal v_bSearchByPaymentDate As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessUnallocatedClaimPayments"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get any unallocated claim payments for the specified account id
            lReturn = CType(GetUnallocatedClaimPayments(m_lAccountId, v_bSearchByPaymentDate), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetUnallocatedClaimPayments Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = PopulateClaimPaymentListView()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateClaimPaymentListView Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulateClaimPaymentListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 31-01-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function PopulateClaimPaymentListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateClaimPaymentListView"

        Dim lReturn, llBound, lUBound, lDocumentId As Integer
        Dim sDocumentRef, sClaimNumber, sCurrencyFormatString As String
        Dim crPaymentCurrencyAmount As Decimal
        Dim sDocumentComment As String = ""
        Dim oListItem As ListViewItem
        Dim sCurrencyDescription, sClaimPaymentDate As String
        Dim lPayeeMediaTypeId, lCurrencyId As Integer

        Dim crBaseAmount As Decimal
        Dim lBaseCurrencyId As Integer
        Dim sBaseCurrencyDescription, sBaseCurrencyFormat As String
        Dim lClaimPaymentId As Integer
        Dim sAccountName As String = ""
        Dim lAccountID As Integer
        'WR05
        Dim sStatus, sMediaType, sBranch, sBank As String
        ''WR05

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' clear down the claim payment list view
            lvwClaimPayments.Items.Clear()

            ' if there are unallocated claim payments
            If Information.IsArray(m_vUnallocatedClaimPayments) Then

                ' get the arrays boundaries
                llBound = m_vUnallocatedClaimPayments.GetLowerBound(1)
                lUBound = m_vUnallocatedClaimPayments.GetUpperBound(1)

                ' for each claim payment
                For lClaimPayment As Integer = llBound To lUBound

                    ' get claim payment details
                    lDocumentId = gPMFunctions.ToSafeLong(m_vUnallocatedClaimPayments(kClaimPaymentDetailsDocumentId, lClaimPayment), 0)
                    sDocumentRef = CStr(m_vUnallocatedClaimPayments(kClaimPaymentDetailsDocumentRef, lClaimPayment)).Trim()
                    sClaimNumber = CStr(m_vUnallocatedClaimPayments(kClaimPaymentDetailsClaimNumber, lClaimPayment)).Trim()
                    crPaymentCurrencyAmount = gPMFunctions.ToSafeCurrency(m_vUnallocatedClaimPayments(kClaimPaymentDetailsCurrencyAmount, lClaimPayment), 0) * -1
                    sCurrencyFormatString = CStr(m_vUnallocatedClaimPayments(kClaimPaymentDetailsCurrencyFormatString, lClaimPayment)).Trim()
                    sDocumentComment = CStr(m_vUnallocatedClaimPayments(kClaimPaymentDetailsDocComment, lClaimPayment)).Trim()
                    sCurrencyDescription = CStr(m_vUnallocatedClaimPayments(kClaimPaymentDetailsCurrencyDescription, lClaimPayment)).Trim()

                    sAccountName = CStr(m_vUnallocatedClaimPayments(kClaimPaymentDetailsAccountName, lClaimPayment)).Trim()
                    lAccountID = gPMFunctions.ToSafeLong(m_vUnallocatedClaimPayments(kClaimPaymentDetailsAccountId, lClaimPayment), 0)

                    ' use claim payment date as first choice, document date as second and todays date as third
                    'Developer Guide No 113
                    If gPMFunctions.ToSafeDate(m_vUnallocatedClaimPayments(kClaimPaymentDetailsClaimPaymentdate, lClaimPayment), #12/30/1989#) <> #12/30/1989# Then
                        sClaimPaymentDate = gPMFunctions.ToSafeDate(m_vUnallocatedClaimPayments(kClaimPaymentDetailsClaimPaymentdate, lClaimPayment), DateTime.Today).ToString("dd/MM/yyyy")
                        'Developer Guide No 113
                    ElseIf gPMFunctions.ToSafeDate(m_vUnallocatedClaimPayments(kClaimPaymentDetailsDocumentDate, lClaimPayment), #12/30/1989#) <> #12/30/1989# Then
                        sClaimPaymentDate = gPMFunctions.ToSafeDate(m_vUnallocatedClaimPayments(kClaimPaymentDetailsDocumentDate, lClaimPayment), DateTime.Today).ToString("dd/MM/yyyy")
                    Else
                        sClaimPaymentDate = DateTime.Today.ToString("dd/MM/yyyy")
                    End If

                    lPayeeMediaTypeId = gPMFunctions.ToSafeLong(m_vUnallocatedClaimPayments(kClaimPaymentDetailsPaymentMediaTypeId, lClaimPayment), 0)
                    lCurrencyId = gPMFunctions.ToSafeLong(m_vUnallocatedClaimPayments(kClaimPaymentDetailsCurrencyId, lClaimPayment), 0)

                    crBaseAmount = gPMFunctions.ToSafeCurrency(m_vUnallocatedClaimPayments(kClaimPaymentDetailsBaseAmount, lClaimPayment), 0) * -1
                    lBaseCurrencyId = gPMFunctions.ToSafeLong(m_vUnallocatedClaimPayments(kClaimPaymentDetailsBaseCurrencyId, lClaimPayment), 0)
                    sBaseCurrencyDescription = CStr(m_vUnallocatedClaimPayments(kClaimPaymentDetailsBaseCurrencyDescription, lClaimPayment))
                    sBaseCurrencyFormat = CStr(m_vUnallocatedClaimPayments(kClaimPaymentDetailsBaseCurrencyFormat, lClaimPayment))
                    lClaimPaymentId = gPMFunctions.ToSafeLong(m_vUnallocatedClaimPayments(kClaimPaymentDetailsClaimPaymentId, lClaimPayment), 0)
                    'WR05
                    sStatus = gPMFunctions.ToSafeString(m_vUnallocatedClaimPayments(kClaimPaymentDetailsStatus, lClaimPayment), CStr(0))
                    sMediaType = gPMFunctions.ToSafeString(m_vUnallocatedClaimPayments(kClaimPaymentDetailsMediaType, lClaimPayment), CStr(0))
                    sBranch = gPMFunctions.ToSafeString(m_vUnallocatedClaimPayments(kClaimPaymentDetailsBranch, lClaimPayment), CStr(0))
                    sBank = gPMFunctions.ToSafeString(m_vUnallocatedClaimPayments(kClaimPaymentDetailsBank, lClaimPayment), CStr(0))
                    ''WR05

                    ' add a new list item

                    'Changes done as per VB code
                    oListItem = lvwClaimPayments.Items.Add(CStr(lDocumentId))
                    oListItem.Tag = CStr(lClaimPayment)
                    'oListItem.Text = CStr(lDocumentId)

                    ' document ref
                    oListItem.Name = sDocumentRef.Trim()

                    ' populate the list items details
                    ListViewHelper.GetListViewSubItem(oListItem, kSubItemAccountName).Text = sAccountName
                    ListViewHelper.GetListViewSubItem(oListItem, kSubItemDocumentReference).Text = sDocumentRef

                    ' claim number
                    If sClaimNumber <> "" Then
                        ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimReference).Text = sClaimNumber
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimReference).Text = sDocumentComment.Replace("Payment for claim ref. ", "").Trim()
                    End If

                    ' claim payment date
                    ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentDate).Text = sClaimPaymentDate

                    ' claim payment amount formatted
                    If sCurrencyFormatString <> "" Then
                        ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentAmount).Text = StringsHelper.Format(crPaymentCurrencyAmount, sCurrencyFormatString)
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentAmount).Text = StringsHelper.Format(crPaymentCurrencyAmount, "0.00")
                    End If

                    ' currency description
                    ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentCurrency).Text = sCurrencyDescription

                    ' base amount formatted
                    If sBaseCurrencyFormat <> "" Then
                        ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimBaseAmount).Text = StringsHelper.Format(crBaseAmount, sBaseCurrencyFormat)
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimBaseAmount).Text = StringsHelper.Format(crBaseAmount, "0.00")
                    End If

                    ' base description
                    ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimBaseCurrency).Text = sBaseCurrencyDescription

                    ' payee media type id
                    ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentMediaTypeId).Text = CStr(lPayeeMediaTypeId)

                    ' currency id
                    ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentCurrencyId).Text = CStr(lCurrencyId)
                    'WR05
                    ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentStatus).Text = sStatus.Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentMediaType).Text = sMediaType.Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentBranch).Text = sBranch.Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentBank).Text = sBank.Trim()
                    ''WR05

                Next

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetupClaimPaymentListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-02-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function SetupClaimPaymentListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupClaimPaymentListView"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lvwClaimPayments.Columns.Clear()

            lvwClaimPayments.Columns.Insert(kColumnHeaderIndexDocumentId - 1, kColumnHeaderKeyDocumentId, "", CInt(VB6.TwipsToPixelsX(300)))
            lvwClaimPayments.Columns.Insert(kColumnHeaderIndexAccountName - 1, kColumnHeaderKeyAccountName, "Account", CInt(VB6.TwipsToPixelsX(1500)))
            lvwClaimPayments.Columns.Insert(kColumnHeaderIndexDocumentReference - 1, kColumnHeaderKeyDocumentReference, "Doc Reference", CInt(VB6.TwipsToPixelsX(2000)))
            lvwClaimPayments.Columns.Insert(kColumnHeaderIndexClaimReference - 1, kColumnHeaderKeyClaimReference, "Claims Reference", CInt(VB6.TwipsToPixelsX(2150)))
            lvwClaimPayments.Columns.Insert(kColumnHeaderIndexClaimPaymentDate - 1, kColumnHeaderKeyClaimPaymentDate, "Payment Date", CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Right, -1)
            lvwClaimPayments.Columns.Insert(kColumnHeaderIndexClaimPaymentAmount - 1, kColumnHeaderKeyClaimPaymentAmount, "Payment Amount", CInt(VB6.TwipsToPixelsX(1750)), HorizontalAlignment.Right, -1)
            lvwClaimPayments.Columns.Insert(kColumnHeaderIndexClaimPaymentCurrency - 1, kColumnHeaderKeyClaimPaymentCurrency, "Payment Currency", CInt(VB6.TwipsToPixelsX(2300)))
            lvwClaimPayments.Columns.Insert(kColumnHeaderIndexClaimBaseAmount - 1, kColumnHeaderKeyClaimBaseAmount, "Base  Amount", CInt(VB6.TwipsToPixelsX(1750)), HorizontalAlignment.Right, -1)
            lvwClaimPayments.Columns.Insert(kColumnHeaderIndexClaimBaseCurrency - 1, kColumnHeaderKeyClaimBaseCurrency, "Base Currency", CInt(VB6.TwipsToPixelsX(2300)))
            lvwClaimPayments.Columns.Insert(kColumnHeaderIndexClaimPaymentMediaTypeId - 1, kColumnHeaderKeyClaimPaymentMediaTypeId, "Payment Media Type", CInt(VB6.TwipsToPixelsX(0)))
            lvwClaimPayments.Columns.Insert(kColumnHeaderIndexClaimPaymentCurrencyId - 1, kColumnHeaderKeyClaimPaymentCurrencyId, "Payment Currency Id", CInt(VB6.TwipsToPixelsX(0)))
            'WR05
            lvwClaimPayments.Columns.Insert(kColumnHeaderIndexClaimPaymentStatus - 1, kColumnHeaderKeyClaimPaymentStatus, "Status", CInt(VB6.TwipsToPixelsX(1800)))
            lvwClaimPayments.Columns.Insert(kColumnHeaderIndexClaimPaymentMediaType - 1, kColumnHeaderKeyClaimPaymentMediaType, "Media Type", CInt(VB6.TwipsToPixelsX(1800)))
            lvwClaimPayments.Columns.Insert(kColumnHeaderIndexClaimPaymentBranch - 1, kColumnHeaderKeyClaimPaymentBranch, "Branch", CInt(VB6.TwipsToPixelsX(1800)))
            lvwClaimPayments.Columns.Insert(kColumnHeaderIndexClaimPaymentBank - 1, kColumnHeaderKeyClaimPaymentBank, "Bank", CInt(VB6.TwipsToPixelsX(1800)))



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function



    Private Sub txtAccountCode_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles txtAccountCode.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If eventArgs.KeyCode = Keys.Return Then

            ' without calling the find account interface check if this is a valid account
            SilentProcessFindAccountWrapper()

        End If

    End Sub

    ' ***************************************************************** '
    ' Name: SilentProcessFindAccountWrapper
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-02-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function SilentProcessFindAccountWrapper() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SilentProcessFindAccountWrapper"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sAccountCode As String = ""
        Dim lAccountID As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the account code
            sAccountCode = txtAccountCode.Text

            ' attempt to get an account id for the specified code

            lReturn = m_oBusiness.GetAccountID(v_sShortCode:=sAccountCode, r_lAccountID:=lAccountID)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue And lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError(kMethodName, "GetAccountId Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if a valid account id was returned
            If lAccountID <> 0 Then

                ' get the account id
                m_lAccountId = lAccountID

                ' get the details for the account
                lReturn = GetAccountDetails()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetAccountDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' get the unallocated claim payments for the account
                lReturn = ProcessUnallocatedClaimPayments()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "ProcessUnallocatedClaimPayments Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            Else

                ' its not a valid account code so call the find account interface
                lReturn = ProcessFindAccountWrapper()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "ProcessFindAccountWrapper Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetAccountDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetAccountDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAccountDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sAccountName, sContactName, sPhoneAreaCode, sPhoneNumber, sPhoneExtension As String
        Dim vAccountBalance As Object
        Dim sAccountCode As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the extra account details

            lReturn = m_oBusiness.GetAccountDetails(r_lAccountID:=m_lAccountId, sAccountName:=sAccountName, sContactName:=sContactName, sPhoneAreaCode:=sPhoneAreaCode, sPhoneNumber:=sPhoneNumber, sPhoneExtension:=sPhoneExtension, r_vdAccountBalance:=vAccountBalance, r_sAccountCode:=sAccountCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetAccountDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' set the account details
            txtAccountName.Text = sAccountName


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ActionSelectAll
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-02-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function ActionSelectAll() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionSelectAll"

        Dim lReturn As Integer
        Dim bClaimCashListItemLinkExists As Boolean
        Dim bSettleAll As Boolean
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            bSettleAll = True

            If Not m_bSelected Then

                ' if there are list items
                If lvwClaimPayments.Items.Count <> 0 Then

                    ' select all list items
                    For lListItem As Integer = 1 To lvwClaimPayments.Items.Count
                        lvwClaimPayments.Items.Item(lListItem - 1).Checked = True
                    Next

                    ' reset select all caption
                    cmdSelectAll.Text = "Deselect All Payments"

                    ' set indicator to show select all has been done
                    m_bSelected = True
                End If
                For Each oListItem As ListViewItem In lvwClaimPayments.Items
                    If oListItem.Checked Then


                        If ToSafeLong(m_vUnallocatedClaimPayments(kClaimPaymentCashItemLink, oListItem.Tag), 0) <> 0 AndAlso ToSafeLong(m_vUnallocatedClaimPayments(kClaimPaymentIsReversed, oListItem.Tag), 0) <> 1 Then
                            bClaimCashListItemLinkExists = True
                        End If


                        If ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentMediaType).Text = "" Or ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentBank).Text = "" Or ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentStatus).Text = "Awaiting Settlement Authorisation" Then
                            bSettleAll = False
                        End If
                    End If
                Next oListItem
                If bClaimCashListItemLinkExists Then
                    cmdPayAsOne.Enabled = False
                    cmdPayIndividually.Enabled = False
                    cmdSettleAll.Enabled = False
                Else
                    cmdPayAsOne.Enabled = True
                    cmdPayIndividually.Enabled = True
                End If
            Else

                ' if there are list items
                If lvwClaimPayments.Items.Count <> 0 Then

                    ' unselect all list items
                    For lListItem As Integer = 1 To lvwClaimPayments.Items.Count
                        lvwClaimPayments.Items.Item(lListItem - 1).Checked = False
                    Next

                    ' reset select all caption
                    cmdSelectAll.Text = "Select All Payments"

                    ' set indicator to show select all has not been done
                    m_bSelected = False
                    cmdPayAsOne.Enabled = True
                    cmdPayIndividually.Enabled = True
                    cmdSettleAll.Enabled = True
                End If

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ActionClearPayments
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function ActionClearPayments() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionClearPayments"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' clear down any claim payments
            lvwClaimPayments.Items.Clear()

            ' reset selected status
            m_bSelected = False


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessMakeSinglePayment
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-02-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function ProcessMakeSinglePayment() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessMakeSinglePayment"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSelectedItemCount As Integer
        Dim vDocuments As Object
        Dim lMediaTypeId, lCurrencyId, lAccountID As Integer
        Dim vClaimPaymentIDs As Object
        Dim oListItem As ListViewItem
        Dim iIndex As Integer
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(GetNumberOfSelectedItems(lSelectedItemCount), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetNumberOfSelectedItems Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if an item has been selected
            If lSelectedItemCount > 0 Then

                ' if only one items has been selected
                If lSelectedItemCount = 1 Then

                    ' process it using the make multiple payments process
                    lReturn = ProcessMakeMultiplePayments()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "ProcessMakeMultiplePayments Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                Else

                    ' get the claim payment details to apply for all
                    ' of the selected payments

                    lReturn = CType(GetClaimPaymentDetailsWrapper(lMediaTypeId, lCurrencyId, vDocuments, lAccountID), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetDocumentArray Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    'Get the Claim Payments ID's of the selected entries

                    ReDim vClaimPaymentIDs(lSelectedItemCount - 1)

                    'Get array of selected claim payment items
                    For iVar As Integer = 1 To m_vUnallocatedClaimPayments.GetUpperBound(1) + 1
                        oListItem = lvwClaimPayments.Items.Item(iVar - 1)
                        If lvwClaimPayments.Items.Item(iVar - 1).Checked Then

                            vClaimPaymentIDs(iIndex) = gPMFunctions.ToSafeLong(m_vUnallocatedClaimPayments(kClaimPaymentDetailsClaimPaymentId, Convert.ToString(oListItem.Tag)), 0)
                            iIndex += 1
                        End If
                    Next

                    ' create cashlist payment
                    lReturn = CType(CreateCashListPayment(v_lMediaTypeId:=lMediaTypeId, v_lAccountID:=lAccountID, v_lCurrencyId:=lCurrencyId, v_vDocumentIds:=vDocuments, v_vClaimPaymentIDs:=vClaimPaymentIDs), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "CreateCashListPayment Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                End If

                ' refresh the unallocated claim payment items
                'Developer Guide No 113
                If gPMFunctions.ToSafeDate(txtDateOfPaymentFrom.Text, #12/30/1989#) <> #12/30/1989# And gPMFunctions.ToSafeDate(txtDateOfPaymentTo.Text, #12/30/1989#) <> #12/30/1989# Then
                    lReturn = CType(ProcessUnallocatedClaimPayments(True), gPMConstants.PMEReturnCode)
                Else
                    lReturn = CType(ProcessUnallocatedClaimPayments(False), gPMConstants.PMEReturnCode)
                End If

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "ProcessUnallocatedClaimPayments Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ProcessMakeMultiplePayments
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-02-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function ProcessMakeMultiplePayments() As Integer

        Dim result As Integer = 0
        Dim bACTCashList As Object

        Const kMethodName As String = "ProcessMakeMultiplePayments"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim nClaimPayments As Integer
        Dim oListItem As ListViewItem

        Dim vDocumentArray(0) As Object
        Dim lMediaTypeId, lCurrencyId, lDocumentId, lSelectedItemCount, lClaimPaymentId, lAccountID As Integer
        Dim vClaimPaymentIDs(0) As Object
        Dim bClaimLinkPayment As Boolean
        Dim oCashList As bACTCashList.Form
        Dim sOurReference As String
        Dim sTheirReference As String
        Dim sPayeeName As String
        Dim nPartyBankId As Integer
        Dim sPayeeAccountNo As String
        Dim sPayeeShortCode As String
        Dim bIsreversed As Boolean
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(GetNumberOfSelectedItems(lSelectedItemCount), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetNumberOfSelectedItems Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if an item has been selected
            If lSelectedItemCount > 0 Then

                nClaimPayments = lvwClaimPayments.Items.Count

                ' for each selected item
                For lClaimPayment As Integer = 1 To nClaimPayments

                    If lvwClaimPayments.Items.Item(lClaimPayment - 1).Checked Then

                        lReturn = CType(GetClaimPaymentDetails(lClaimPayment, lClaimPaymentId, lMediaTypeId, lCurrencyId, lDocumentId, lAccountID, r_sPayeeName:=sPayeeName, r_sOurRef:=sOurReference, r_sTheirRef:=sTheirReference, r_nPartyBankId:=nPartyBankId, r_sPayeeAccountNo:=sPayeeAccountNo, r_sPayeeShortCode:=sPayeeShortCode, r_bIsReversed:=bIsreversed), gPMConstants.PMEReturnCode)

                        vDocumentArray(0) = lDocumentId

                        vClaimPaymentIDs(0) = lClaimPaymentId

                        Dim temp_oCashList As Object
                        lReturn = g_oObjectManager.GetInstance(temp_oCashList, "bACTCashList.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                        oCashList = temp_oCashList
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New Exception
                        End If

                        bClaimLinkPayment = False

                        'UPGRADE_TODO: (1067) Member CheckClaimLink is not defined in type bActCashList.Form. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                        If bIsreversed Then
                            bClaimLinkPayment = False
                            bIsreversed = False
                        Else
                            lReturn = oCashList.CheckClaimLink(v_lClaimPaymentId:=lClaimPaymentId, r_bResults:=bClaimLinkPayment)

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Throw New Exception
                            End If
                        End If

                        'UPGRADE_TODO: (1067) Member Terminate is not defined in type bActCashList.Form. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                        oCashList.Dispose()

                        oCashList = Nothing
                        If Not bClaimLinkPayment Then

                            ' Build Documents Array.
                            lReturn = CType(CreateCashListPayment(v_lMediaTypeId:=lMediaTypeId, v_lAccountID:=lAccountID, v_lCurrencyId:=lCurrencyId, v_vDocumentIds:=vDocumentArray, v_lClaimPaymentId:=lClaimPaymentId, v_vClaimPaymentIDs:=vClaimPaymentIDs, v_sPayeeName:=sPayeeName, v_sOurReference:=sOurReference, v_sTheirReference:=sTheirReference, v_nPartyBankId:=nPartyBankId, v_sPayeeAccountNo:=sPayeeAccountNo, v_sPayeeShortCode:=sPayeeShortCode), gPMConstants.PMEReturnCode)

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                MessageBox.Show("Create cash list failed or was cancelled by the user", "Claim Payment Processing Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Exit For
                            End If
                        End If
                    End If
                Next

                ' refresh the unallocated claim payment items
                'Developer Guide No 113
                If gPMFunctions.ToSafeDate(txtDateOfPaymentFrom.Text, #12/30/1989#) <> #12/30/1989# And gPMFunctions.ToSafeDate(txtDateOfPaymentTo.Text, #12/30/1989#) <> #12/30/1989# Then
                    lReturn = CType(ProcessUnallocatedClaimPayments(True), gPMConstants.PMEReturnCode)
                Else
                    lReturn = CType(ProcessUnallocatedClaimPayments(False), gPMConstants.PMEReturnCode)
                End If

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "ProcessUnallocatedClaimPayments Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    'WR05

    Private Function SettleAllPayments() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SettleAllPayments"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSelectedItemCount As Integer
        Dim sChequeProduction As String
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=60, v_vBranch:=g_iSourceID, r_vUnderwriting:=sChequeProduction)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "RetrieveSingleSystemOption Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If sChequeProduction = "1" Then
                m_bChequeProduction = True
            Else
                m_bChequeProduction = False
            End If

            lReturn = CType(GetNumberOfSelectedItems(lSelectedItemCount), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetNumberOfSelectedItems Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If lSelectedItemCount > 0 Then

                For iIndex As Integer = 1 To lvwClaimPayments.Items.Count
                    If lvwClaimPayments.Items.Item(iIndex - 1).Checked Then
                        lReturn = CType(SettlePayment(iIndex), gPMConstants.PMEReturnCode)
                    End If
                Next

                If m_cTotalAmountReferredForAuthorisation > 0 Then
                    m_lReturn = CType(SaveSummary("Payment Referred for Auth.", m_cTotalAmountReferredForAuthorisation), gPMConstants.PMEReturnCode)
                    m_vSummary(InterfaceMain.ACListPaymentSummary.PSPaymentCount, m_vSummary.GetUpperBound(1)) = m_iTotalTransactionsReferredForAuthorisation
                End If

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    Private Function SettlePayment(ByRef iIndex As Integer) As Integer
        Dim result As Integer = 0
        Dim bACTCashListPost, bActAllocate, bActUserAuthorities, bActBankAccount, bActAccount, bActCashListItem, bActCashList As Object

        Const kMethodName As String = "SettlePayment"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSelectedItemCount As Integer
        Dim vDocuments As Object
        Dim lCurrencyId As Integer

        Dim vClaimPaymentIDs As Object
        Dim iVar As Integer
        Dim oListItem As ListViewItem

        Dim lAccountID, lDocumentId, lClaimPaymentId, lBankAccountId, lCashListId, lMediaTypeId As Integer
        Dim cClaimPaymentamount As Decimal

        Dim vDocumentArray(0) As Object
        Dim lCashListItemId As Integer
        Dim bProceedFurther As Boolean
        Dim vOSTransactions As Object
        Dim lTransDetailID As Integer
        Dim vCashListItemDetails As Object
        Dim bHasPaymentsAuthority As Boolean
        Dim cPaymentAuthority As Decimal
        Dim r_bIsDeleted As Boolean
        Dim vCashListItem()
        Dim vKeyArray(,) As Object

        Dim sPayeeName As String
        Dim sOurReference As String

        Dim oCashList As bACTCashList.Form

        Dim oCashListItem As bACTCashlistitem.Form

        Dim oAccount As bACTAccount.Form

        Dim oBankAccount As bACTBankAccount.Form

        Dim oUserAuthorities As bACTUserAuthorities.Business

        Dim oAllocate As bACTAllocate.Business

        Dim oCashListPost As bACTCashListPost.Automated
        Dim bProceedtoPost As Boolean
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            'Get the CashList business Object
            Dim temp_oCashList As Object
            lReturn = g_oObjectManager.GetInstance(temp_oCashList, "bACTCashList.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oCashList = temp_oCashList
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to GetInstance For - bACTCashList.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the CashListItem Business Object
            Dim temp_oCashListItem As Object
            lReturn = g_oObjectManager.GetInstance(temp_oCashListItem, "bACTCashlistitem.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oCashListItem = temp_oCashListItem
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to GetInstance For - bACTCashlistitem.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the Account Business Object
            Dim temp_oAccount As Object
            lReturn = g_oObjectManager.GetInstance(temp_oAccount, "bACTAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oAccount = temp_oAccount
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to GetInstance For - bActAccount.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the BankAccount Business Object
            Dim temp_oBankAccount As Object
            lReturn = g_oObjectManager.GetInstance(temp_oBankAccount, "bACTBankAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBankAccount = temp_oBankAccount
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to GetInstance For - bActBankAccount.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the UserAuthorities Business Object
            Dim temp_oUserAuthorities As Object
            lReturn = g_oObjectManager.GetInstance(temp_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oUserAuthorities = temp_oUserAuthorities
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to GetInstance For - bACTUserAuthorities.Business.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the UserAuthorities Business Object
            Dim temp_oAllocate As Object
            lReturn = g_oObjectManager.GetInstance(temp_oAllocate, "bACTAllocate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oAllocate = temp_oAllocate
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to GetInstance For - bACTAllocate.Business.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Create CashListPost
            If oCashListPost Is Nothing Then
                Dim temp_oCashListPost As Object
                lReturn = g_oObjectManager.GetInstance(temp_oCashListPost, "bACTCashListPost.Automated", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oCashListPost = temp_oCashListPost
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to create bACTCashListPost.Automated")
                End If
            End If

            lReturn = CType(GetNumberOfSelectedItems(lSelectedItemCount), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetNumberOfSelectedItems Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            bProceedFurther = True
            bProceedtoPost = True
            lReturn = CType(GetClaimPaymentDetails(iIndex, lClaimPaymentId, lMediaTypeId, lCurrencyId, lDocumentId, lAccountID, cClaimPaymentamount, lBankAccountId, , , sOurReference), gPMConstants.PMEReturnCode)

            vDocumentArray(0) = lDocumentId

            lReturn = oAccount.IsDeleted(lAccountID, r_bIsDeleted)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oAccount.IsDeleted Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If r_bIsDeleted Then
                bProceedFurther = False
            End If

            m_lReturn = CType(GetUserAuthorities(bHasPaymentsAuthority, cPaymentAuthority), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetUserAuthorities Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If bHasPaymentsAuthority And cPaymentAuthority < cClaimPaymentamount * -1 Then
                bProceedtoPost = False
                'Create a WorkMAnager Task
            End If

            If bProceedFurther Then
                'Add the CashListDetails

                lReturn = oCashList.DirectAdd(vCashListID:=lCashListId, vCashListStatusID:=1, vCashListTypeID:=3, vCashListRef:="", vCompanyID:=g_oObjectManager.SourceID, vBankAccountID:=lBankAccountId, vCurrencyID:=lCurrencyId, vListDate:=gPMFunctions.ToSafeDate(DateTime.Now), vControlTotal:=0, vItemCount:=0)

                'Add CashListItem Details


                lReturn = CType(PrepareCashListItem(lAccountID, lCashListId, cClaimPaymentamount, lMediaTypeId, lBankAccountId, lCurrencyId, vCashListItem, , sOurReference), gPMConstants.PMEReturnCode)

                lReturn = oCashListItem.DirectAdd(r_vCashListItem:=vCashListItem)

                lReturn = oCashListItem.Update
                oListItem = lvwClaimPayments.Items.Item(iIndex - 1)


                lCashListItemId = CInt(vCashListItem(0))

                ReDim vKeyArray(1, 1)


                vKeyArray(0, 0) = PMNavKeyConst.ACTKeyNameCashListId

                vKeyArray(1, 0) = lCashListId

                vKeyArray(0, 1) = PMNavKeyConst.ACTKeyNameCashListItemId

                vKeyArray(1, 1) = lCashListItemId


                m_lReturn = oCashListPost.SetKeys(vKeyArray:=vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "oCashListPost.SetKeys Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If Not bProceedtoPost Then
                    m_lReturn = CType(AddTaskToWorkManager(lCashListId, lCashListItemId, lAccountID, cClaimPaymentamount), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("SettlePayment", "AddTaskToWorkManager Failed")
                    End If

                    m_cTotalAmountReferredForAuthorisation += cClaimPaymentamount * -1
                    m_iTotalTransactionsReferredForAuthorisation += 1

                    m_lReturn = oCashListItem.AddCashListItemClaimLink(v_lClaim_payment_Id:=lClaimPaymentId, v_lCashListItem_id:=lCashListItemId, v_lClaim_receipt_id:=0)

                    m_lReturn = oCashListItem.UpdateCLIPaymentStatus(lCashListItemId)


                Else

                    oCashListPost.ChequeProduction = m_bChequeProduction
                    m_lReturn = oCashListPost.PostUnallocatedCash(v_vCashListID:=lCashListId, sFailureReason:="")
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "oCashListPost.PostUnallocatedCash Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If


                    m_lReturn = oCashListItem.GetDetails(vCashListID:=lCashListId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "oCashListItem.GetDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        m_lReturn = oCashListItem.GetNext(vCashListItemDetails)

                        lTransDetailID = CInt(vCashListItemDetails(gACTLibrary.eCashListItem.TransdetailID))
                    End If


                    m_lReturn = oAccount.GetAccountOSTransForDocuments(v_lAccountId:=lAccountID, v_vDocumentIds:=vDocumentArray, r_vOSTransactions:=vOSTransactions)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetAccountOSTransForDocuments Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If


                    If oAllocate.PerformAutoAllocation(r_lAccountId:=lAccountID, r_lTransDetailId:=lTransDetailID, v_vOSTransactions:=vOSTransactions, v_lCashListItemID:=lCashListItemId) <> gPMConstants.PMEReturnCode.PMTrue Then

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "GetAccountOSTransForDocuments Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    Else

                        m_lReturn = CType(SaveSummary(ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentMediaType).Text, cClaimPaymentamount * -1), gPMConstants.PMEReturnCode)

                    End If
                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oAccount = Nothing
            oAllocate = Nothing
            oBankAccount = Nothing
            oCashList = Nothing
            oCashListItem = Nothing
            oUserAuthorities = Nothing




        End Try
        Return result
    End Function

    Public Function AddTaskToWorkManager(ByRef lCashListId As Integer, ByRef lCashListItemId As Integer, ByRef lAccountID As Integer, ByRef cClaimPaymentamount As Decimal) As Integer
        Dim result As Integer = 0
        Dim lTaskInstanceCnt As Integer
        Dim sTaskDesc, vCashListRef As String
        Dim vListDate As Date
        Dim vKeyArray(,) As Object
        Dim sCashItemOurRef As String
        Dim cCashItemAmount As Decimal
        Dim sTaskDescComplete As String = ""
        Dim sUserGroup As String = ""
        Dim oAccount As bACTAccount.Form
        Dim sAccountShortCode As String = ""
        Dim oCashListItemBusiness As bACTCashlistitem.Form

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oCashListItemBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oCashListItemBusiness, "bACTCashListItem.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oCashListItemBusiness = temp_oCashListItemBusiness

            m_lReturn = CType(GetCashListDetails(v_lCashListID:=lCashListId, r_vCashListRef:=vCashListRef, r_vListDate:=vListDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process GetCashListDetails.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")
                Return result
            End If

            ''m_lReturn& = m_oBusiness.GetCashListType(v_lCashListTypeID:=m_lCashListTypeID, _
            'r_sCashListType:=sTaskDesc)
            sTaskDesc = "Claim Payment"

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process GetCashListType.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")
                Return result
            End If


            'Creating Account Business Object
            Dim temp_oAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAccount, "bACTAccount.Form", vInstanceManager:="ClientManager")
            oAccount = temp_oAccount

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create the Account business object", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")

                Return result
            End If



            ''For iLoop% = LBound(m_vListData, 2) To UBound(m_vListData, 2)
            'Only create items for CashListItems wiht status Pending
            ''  If NullToLong(m_vListData(ACCashListItemPaymentStatusID, iLoop%)) = ACStatusPendingID Then
            'm_lCashListItemID = m_vListData(ACSubCashListItemID, iLoop%)
            '    sCashItemMediaRef = NullToString(m_vListData(ACSubMediaRef, iLoop%))
            '   sCashItemOurRef = NullToString(m_vListData(ACSubOurRef, iLoop%))
            '   sCashItemTheirRef = NullToString(m_vListData(ACSubTheirRef, iLoop%))
            ''   cCashItemAmount = Abs(NullToCurrency(m_vListData(ACSubAmount, iLoop%)))
            cCashItemAmount = -1 * cClaimPaymentamount
            'Getting Account Short Code
            '''pass accountid

            m_lReturn = oAccount.GetDetails(vAccountID:=lAccountID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the account business object", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")

                Return result
            End If


            m_lReturn = oAccount.GetNext(vShortCode:=sAccountShortCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the Account business object", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")
            End If


            ReDim vKeyArray(2, 4)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameCashListItemId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lCashListItemId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameCashListId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = lCashListId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameCashListTypeId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = 3

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameActionKey

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = "approve" 'ACTApprove

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameCashListItemMode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = 2


            sUserGroup = "SLACS"


            sTaskDescComplete = sTaskDesc & " - Cash / Cheque" & New String(" "c, 1)
            If Not String.IsNullOrEmpty(sCashItemOurRef) Then
                sTaskDescComplete = sTaskDescComplete & " - Reference: " & sCashItemOurRef.Trim() & New String(" "c, 1)
            End If
            'sTaskDesc = sTaskDesc & "List Date :" & Trim$(vListDate)
            sTaskDescComplete = sTaskDescComplete & " - The Amount: " & StringsHelper.Format(cCashItemAmount, "#,##0.00")
            'eck 130901 change for Tinny - pass in short code
            sUserGroup = "SYSADMIN"

            m_lReturn = oCashListItemBusiness.AddTaskToWorkManager(r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_sCustomer:=sAccountShortCode, v_sDescription:=sTaskDescComplete, v_dtTaskDueDate:=DateTime.Today.AddDays(1).AddSeconds(-1), v_sTaskCode:="SHOWCLITEM", v_sTaskGroupCode:="SLACS", v_sUserGroupCode:=sUserGroup, v_vKeyArray:=vKeyArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process AddTaskToWorkManager.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")
                Return result
            End If

            'End If


            oAccount = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    'UPGRADE_NOTE: (7001) The following declaration (CreateWorkManagerTask) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CreateWorkManagerTask(ByRef lCashListId As Integer) As Integer
    'Dim result As Integer = 0
    'Dim bPMLookup, iPMWrkTaskInstance As Object
    '

    'Dim oWrkTaskInstance As iPMWrkTaskInstance.NavigatorV3

    'Dim oPMLookUp As bPMLookup.Business
    '
    'Dim lTaskID, lTaskGroupID As Integer
    'Dim vKeys As Object
    '
    'Dim vCashListRef As String = ""
    'Dim vListDate As String = ""
    '
    'Dim sTaskDesc As String = ""
    'Const kMethodName As String = "CreateWorkManagerTask"
    '
    'On Error GoTo Catch_Renamed
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ''ReDim vKeys(1, 8)
    '
    ' Change the cursor mode
    'iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseBusy)
    '
    ' Object to create work manager tasks
    'Dim temp_oWrkTaskInstance As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_oWrkTaskInstance, "iPMWrkTaskInstance.NavigatorV3", vInstanceManager:=gPMConstants.PMGetLocalInterface)
    'oWrkTaskInstance = temp_oWrkTaskInstance
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Log Error Message
    'gPMFunctions.RaiseError(kMethodName, "Unable to get instance of iPMWrkTaskInstance.NavigatorV3")
    ' Change the cursor mode
    'End If
    '
    ' Set to ADD mode
    'm_lReturn = CType(oWrkTaskInstance.NavigatorV3_SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "Unable to execute NavigatorV3_SetProcessModes(")
    'End If
    '
    ' Set the authority level

    '
    ' Create an instance of bPMLookup
    'Dim temp_oPMLookUp As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_oPMLookUp, "bPMLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
    'oPMLookUp = temp_oPMLookUp
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "Unable to get bPMLookup.Business")
    'End If
    '
    ' Set the product family

    'oPMLookUp.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
    '
    ' Use the lookup to get the ID of the ACTRCTV2 task

    'm_lReturn = oPMLookUp.GetEffectiveIDFromCode(v_sTableName:="pmwrk_task", v_sCode:="ACTRCTV2", v_dtEffectiveDate:=DateTime.Now, r_lID:=lTaskID)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "Unable to get oPMLookUp.GetEffectiveIDFromCode")
    'End If
    '
    ' Use the lookup to get the ID of the PLACS task group

    'm_lReturn = oPMLookUp.GetEffectiveIDFromCode(v_sTableName:="pmwrk_task_group", v_sCode:="PLACS", v_dtEffectiveDate:=DateTime.Now, r_lID:=lTaskGroupID)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "Failed to GetEffectiveIDFromCode")
    'End If
    ' Remove instance of lookup

    'm_lReturn = oPMLookUp.Terminate()
    'oPMLookUp = Nothing
    '
    'm_lReturn = CType(GetCashListDetails(v_lCashListID:=gACTLibrary.eCashListItem.CashlistID, r_vCashListRef:=vCashListRef, r_vListDate:=vListDate), gPMConstants.PMEReturnCode)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "Failed to GetEffectiveIDFromCode")
    'End If
    '
    'm_lReturn = CType(iACTFunc.VBsprintf(sTaskDesc, "[%s] [%s] Receipt - Cash / Cheque", vCashListRef.Trim(), vListDate.Trim()), gPMConstants.PMEReturnCode)
    '
    ' Set up the key array

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameTaskGroupCode

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = "PLACS"

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameTaskID

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = lTaskID

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameTaskCode

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = "ACTRCTV2"

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameTaskDescription

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = sTaskDesc & " deepak"

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameTaskCustomer

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = "Customer"

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameTaskDueDate

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = DateTime.Today.AddDays(1).AddSeconds(-1)

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameTaskIsUrgent

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = gPMConstants.PMEReturnCode.PMTrue

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNameTaskGroupID

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = lTaskGroupID

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.ACTKeyNameCashListId

    'vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = gACTLibrary.eCashListItem.CashlistID
    '
    ' Pass the keys in
    'm_lReturn = CType(oWrkTaskInstance.NavigatorV3_SetKeys(vKeyArray:=vKeys), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "Failed to set keys")
    'End If
    '
    ' Change the cursor mode
    'iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    '
    ' Display the form

    'm_lReturn = oWrkTaskInstance.NavigatorV3_Start()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "Failed to start WrkTaskInstance")
    'End If
    '

    'If oWrkTaskInstance.NavigatorV3_Status = gPMConstants.PMEReturnCode.PMCancel Then
    'result = gPMConstants.PMEReturnCode.PMCancel
    'End If
    '
    ' Terminate the object

    'm_lReturn = oWrkTaskInstance.Terminate()
    'oWrkTaskInstance = Nothing
    '
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    '
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    'Finally_Renamed: '
    'iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    'Return result
    'Resume 
    'Return result
    'End Function
    Private Function GetCashListDetails(ByVal v_lCashListID As Integer, Optional ByRef r_vCashListStatusID As Object = Nothing, Optional ByRef r_vCashListRef As Object = Nothing, Optional ByRef r_vCompanyID As Object = Nothing, Optional ByRef r_vBankAccountID As Object = Nothing, Optional ByRef r_vCurrencyID As Object = Nothing, Optional ByRef r_vListDate As Object = Nothing, Optional ByRef r_vControlTotal As Object = Nothing, Optional ByRef r_vItemCount As Object = Nothing, Optional ByRef r_vCashDrawerID As Double = 0, Optional ByRef r_vBatchID As Object = Nothing, Optional ByRef r_bHasSecurityAccess As Boolean = False) As Integer
        Dim result As Integer = 0

        Dim oCashList As bACTCashList.Form
        Dim vResultArray(,) As Object

        Const ACCashDrawerId As Integer = 1

        Try

            'SMJB CQ1966 06/08/03: If cash drawer was locked then we won't have an ID here
            '(it will be 0, so test for it and quietly exit)
            If v_lCashListID = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of bACTCashList
            Dim temp_oCashList As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oCashList, "bACTCashList.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oCashList = temp_oCashList

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCashListDetails Failed to create instance of bACTCashList", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                ' Remove the instance.

                oCashList.Dispose()
                oCashList = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Getdetails

            m_lReturn = oCashList.GetDetails(vCashListID:=v_lCashListID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for cashlistid " & v_lCashListID, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                ' Remove the instance.

                oCashList.Dispose()
                oCashList = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oCashList.GetNext(vCashListID:=v_lCashListID, vCashListStatusID:=r_vCashListStatusID, vCashListRef:=r_vCashListRef, vCompanyID:=r_vCompanyID, vBankAccountID:=r_vBankAccountID, vCurrencyID:=r_vCurrencyID, vListDate:=r_vListDate, vControlTotal:=r_vControlTotal, vItemCount:=r_vItemCount, vCashlist_drawer_id:=r_vCashDrawerID, vBatch_id:=r_vBatchID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to getnext for cashlistid " & v_lCashListID, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                ' Remove the instance.

                oCashList.Dispose()
                oCashList = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DJM 13/10/2003 : Also check to see if r_vCashDrawerID has passed into or not.
            ' DD 25/09/2003
            ' Moved security check here. Essentially any user can view a cash drawer
            ' but only those with security rights can add/reverse/allocate

            If Not Information.IsNothing(r_vCashDrawerID) Then
                If r_vCashDrawerID > 0 Then

                    r_bHasSecurityAccess = False

                    ' Get CashLists User has access to

                    m_lReturn = oCashList.GetAllUserCashListDrawer(v_lUserId:=g_oObjectManager.UserID, r_vResultArray:=vResultArray)

                    ' Check for errors
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to get details.
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to run GetAllUserCashListDrawer in business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListDetails")

                        Return result

                    Else
                        ' Scan all rows in array data
                        If Information.IsArray(vResultArray) Then

                            For iRowCount As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                                ' Check Cash Drawer is in Users available list

                                If Conversion.Val(CStr(vResultArray(ACCashDrawerId, iRowCount))) = r_vCashDrawerID Then
                                    r_bHasSecurityAccess = True
                                    Exit For
                                End If
                            Next iRowCount
                        End If
                    End If
                Else
                    'Not a cash drawer so everyone has access
                    r_bHasSecurityAccess = True
                End If
            Else
                'Not a cash drawer so everyone has access
                r_bHasSecurityAccess = True
            End If

            ' Remove the instance.

            oCashList.Dispose()
            oCashList = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCashListDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''WR05

    Private Function GetUserAuthorities(ByRef r_bHasPaymentsAuthority As Boolean, ByRef r_cPaymentsAmount As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetUserAuthorities"
        Dim vResults As Object
        Dim crConvertedCurrency As Decimal
        Dim iPaymentsCurrencyID As Integer
        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oUserAuthorities.GetDetails(vUserID:=g_oObjectManager.UserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Unable to get User Authorities Details")
            End If


            m_lReturn = m_oUserAuthorities.GetNext(vHasPaymentsAuthority:=r_bHasPaymentsAuthority, vPaymentsAmount:=r_cPaymentsAmount, vPaymentsCurrencyID:=iPaymentsCurrencyID)

            If r_bHasPaymentsAuthority And g_oObjectManager.CurrencyID <> iPaymentsCurrencyID Then

                m_lReturn = m_obACTCurrencyConvert.CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=iPaymentsCurrencyID, v_crCurrencyAmountFrom:=r_cPaymentsAmount, v_lCompanyId:=g_iSourceID, v_lCurrencyIdTo:=g_oObjectManager.CurrencyID, r_crCurrencyAmountTo:=crConvertedCurrency)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetUserAuthorities", "CurrencyToCurrencyConversion Failed")
                End If

                r_cPaymentsAmount = crConvertedCurrency
            End If


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    Private Function PrepareCashListItem(ByVal lAccountID As Integer, ByVal lCashListId As Integer, ByVal cAmount As Decimal, ByVal iMediaTypeId As Integer, ByVal iBankAccountIDId As Integer, ByVal iCurrencyId As Integer, ByRef r_vCashListItem() As Object, Optional ByVal v_sPayeeName As String = "", Optional ByVal v_sOurRef As String = "") As Integer
        Dim result As Integer = 0
        Dim bACTCurrencyConvert As Object

        Const kMethodName As String = "CreateCashListPayment"
        Dim iBaseCurrencyID As Integer
        Dim cBaseCurrentAmount As Decimal
        Dim iAccountCurrencyID As Integer
        Dim cAccountCurrentAmount As Decimal
        Dim iSystemCurrencyID As Integer
        Dim cSystemCurrentAmount As Decimal
        Dim dTransToBaseExchangeRate, dAccountToBaseExchangeRate, dSystemToBaseExchangeRate As Double

        Dim oCurrencyBusiness As bACTCurrencyConvert.Form
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim r_vCashListItem(gACTLibrary.eCashListItem.LastItem)

            'Get business object.
            Dim temp_oCurrencyBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oCurrencyBusiness, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oCurrencyBusiness = temp_oCurrencyBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("", "Unable to get bACTCurrencyConvert object")
            End If


            m_lReturn = oCurrencyBusiness.DoCurrencyConversion(v_lAccountID:=lAccountID, v_lCompanyId:=g_oObjectManager.SourceID, v_iCurrencyID:=iCurrencyId, v_cCurrencyAmountUnrounded:=cAmount, r_iBaseCurrencyID:=iBaseCurrencyID, r_cBaseAmount:=cBaseCurrentAmount, r_iAccountCurrencyID:=iAccountCurrencyID, r_cAccountAmount:=cAccountCurrentAmount, r_iSystemCurrencyID:=iSystemCurrencyID, r_cSystemAmount:=cSystemCurrentAmount, r_dCurrencyBaseXrate:=dTransToBaseExchangeRate, r_dtCurrencyBaseDate:=DateTime.Now, r_dAccountBaseXrate:=dAccountToBaseExchangeRate, r_dtAccountBaseDate:=DateTime.Now, r_dSystemBaseXrate:=dSystemToBaseExchangeRate, r_dtSystemBaseDate:=DateTime.Now)


            r_vCashListItem(gACTLibrary.eCashListItem.CashlistitemID) = 0

            r_vCashListItem(gACTLibrary.eCashListItem.AllocationstatusID) = 1

            r_vCashListItem(gACTLibrary.eCashListItem.MediaTypeID) = iMediaTypeId

            r_vCashListItem(gACTLibrary.eCashListItem.CashlistID) = lCashListId

            r_vCashListItem(gACTLibrary.eCashListItem.AccountID) = lAccountID

            r_vCashListItem(gACTLibrary.eCashListItem.MediaRef) = ""

            r_vCashListItem(gACTLibrary.eCashListItem.OurRef) = v_sOurRef

            r_vCashListItem(gACTLibrary.eCashListItem.TheirRef) = ""

            r_vCashListItem(gACTLibrary.eCashListItem.Amount) = cAmount


            r_vCashListItem(gACTLibrary.eCashListItem.AddressCountry) = "1"

            r_vCashListItem(gACTLibrary.eCashListItem.Transaction_Date) = DateTime.Now

            r_vCashListItem(gACTLibrary.eCashListItem.Amount_Tendered) = -cAmount


            r_vCashListItem(gACTLibrary.eCashListItem.CashListItem_Payment_Type_id) = 1 ' For Claim Payment

            r_vCashListItem(gACTLibrary.eCashListItem.CashListItem_Payment_Status_id) = 1 '


            r_vCashListItem(gACTLibrary.eCashListItem.CurrencyBaseDate) = DateTime.Now

            r_vCashListItem(gACTLibrary.eCashListItem.CurrencyBaseXrate) = dTransToBaseExchangeRate

            r_vCashListItem(gACTLibrary.eCashListItem.AccountBaseDate) = DateTime.Now

            r_vCashListItem(gACTLibrary.eCashListItem.AccountBaseXrate) = dAccountToBaseExchangeRate

            r_vCashListItem(gACTLibrary.eCashListItem.SystemBaseDate) = DateTime.Now

            r_vCashListItem(gACTLibrary.eCashListItem.SystemBaseXrate) = dSystemToBaseExchangeRate

            r_vCashListItem(gACTLibrary.eCashListItem.OverrideReason) = 0





        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CreateCashListPayment
    '
    ' Parameters: n/a
    '
    ' Description: Start the navigator process to create a cash list
    '               for the specified claim payment
    '
    ' History:
    '           Created : MEvans : 18-01-2006 : Claims Document Production
    ' ***************************************************************** '
    Private Function CreateCashListPayment(ByVal v_lMediaTypeId As Integer, ByVal v_lAccountID As Integer, ByVal v_lCurrencyId As Integer, ByVal v_vDocumentIds As Object, Optional ByVal v_lClaimPaymentId As Integer = 0, Optional ByVal v_vClaimPaymentIDs As Object = Nothing, Optional ByVal v_sPayeeName As String = "", Optional ByVal v_sOurReference As String = "", Optional ByVal v_sTheirReference As String = "", Optional ByVal v_nPartyBankId As Integer = 0, Optional ByVal v_sPayeeAccountNo As String = "", Optional ByVal v_sPayeeShortCode As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateCashListPayment"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vKeyArray(1, 17) As Object

        Try



            ' create an instance of navigator xm
            m_oNavStart = New iPMNavStart.Interface_Renamed()


            ' initialise it
            'Developer guide No 9
            lReturn = m_oNavStart.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to initialise iPMNavStart.Interface")
            End If

            ' set its properties
            m_oNavStart.CallingAppName = ACApp

            ' set the process to start
            m_oNavStart.ProcessCode = "ACTPAYV2"

            'The XML roadmap to use
            m_oNavStart.NavXMLFile = "ACTPAYV2.XML"

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "cash_list_roadmap"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = "PAYMENTS"

            ' pass negative amount to force "iACTCashList" into payment mode
            ' IMPORTANT NB: this is not the actual payment amount ("-0.01")
            ' the actual payment amount will be determined within
            ' "bACTCashList.ConvertPaymentAmount"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTotalPremium

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = -0.01


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameCurrencyID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_lCurrencyId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameDocumentID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = v_vDocumentIds


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameMediaTypeID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = v_lMediaTypeId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.ACTKeyNameAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = v_lAccountID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameScreenType

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = "CLP"


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.ACTKeyAllowAllocateButton

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = 0


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.ACTKeyNameCashListItemMode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = 2


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameClaimPayment

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = "UNDERWRITING"


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = PMNavKeyConst.PMKeyNameClaimPaymentId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = v_lClaimPaymentId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = PMNavKeyConst.PMKeyNameClaimPaymentIDs


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = v_vClaimPaymentIDs

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 12) = PMNavKeyConst.PMKeyNamePayeeName


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 12) = v_sPayeeName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 13) = PMNavKeyConst.PMKeyNameOurRef


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 13) = v_sOurReference

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 14) = PMNavKeyConst.PMKeyNameTheirRef


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 14) = v_sTheirReference

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 15) = PMNavKeyConst.PMKeyPartyBankId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 15) = v_nPartyBankId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 16) = PMNavKeyConst.PMKeyNamePayeeAccountCode


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 16) = v_sPayeeAccountNo

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 17) = PMNavKeyConst.PMKeyNamePayeeSortCode


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 17) = v_sPayeeShortCode



            ' set the navigators processes keys
            lReturn = m_oNavStart.SetKeys(vKeyArray:=vKeyArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMNavStart.Interface.SetKeys Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' default the navigation completed actions to false
            m_bProcessComplete = False
            m_bNavCompleted = False

            ' start the specified navigator process
            lReturn = m_oNavStart.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMUNavStart.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' wait while the navigator process is completed
            Do
                Application.DoEvents()
            Loop While Not m_bNavCompleted

        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' terminate this instance of the navigator process
            m_oNavStart.Dispose()


            ' clean up the object instances
            m_oNavStart = Nothing

            ' if the process is now complete
        End Try
        If m_bProcessComplete Then
            ' indicate the procedure was successfully run
            Return gPMConstants.PMEReturnCode.PMTrue
        Else
            ' indicate the procedure failed to complete
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
    End Function

    ' ***************************************************************** '
    ' Name: GetNumberOfSelectedItems
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 02-02-2006 : Claim Document Production
    ' ***************************************************************** '
    Public Function GetNumberOfSelectedItems(ByRef r_lSelectedItemCount As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetNumberOfSelectedItems"

        Dim lReturn, nClaimPayments As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get number of unallocated claim payments in the list
            nClaimPayments = lvwClaimPayments.Items.Count

            ' if there are unallocatd claims payments
            If nClaimPayments > 0 Then

                ' for each claim payment item in the list
                For lClaimPayment As Integer = 1 To nClaimPayments

                    ' if the claim payment item is selected
                    If lvwClaimPayments.Items.Item(lClaimPayment - 1).Checked Then

                        ' increment selected item count
                        r_lSelectedItemCount += 1

                    End If

                Next

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimPaymentDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 02-02-2006 : Claims Document Production
    ' ***************************************************************** '
    '61600
    'Public Function GetClaimPaymentDetails(ByVal v_lListItemIndex As Integer, ByRef r_lCLaimPaymentId As Integer, ByRef r_lMediaTypeId As Integer, ByRef r_lCurrencyId As Integer, ByRef r_lDocumentId As Integer, ByRef r_lAccountid As Integer, Optional ByRef r_cClaimPaymentAmount As Decimal = 0, Optional ByRef r_lBankAccountId As Integer = 0) As Integer
    Public Function GetClaimPaymentDetails(ByVal v_lListItemIndex As Integer, ByRef r_lCLaimPaymentId As Integer, ByRef r_lMediaTypeId As Integer, ByRef r_lCurrencyId As Integer, ByRef r_lDocumentId As Integer, ByRef r_lAccountid As Integer, Optional ByRef r_cClaimPaymentAmount As Decimal = 0, Optional ByRef r_lBankAccountId As Integer = 0, Optional ByVal r_iSourceId As Integer = 0, Optional ByRef r_sPayeeName As String = "", Optional ByRef r_sOurRef As String = "", Optional ByRef r_sTheirRef As String = "", Optional ByRef r_nPartyBankId As Integer = 0, Optional ByRef r_sPayeeAccountNo As String = "", Optional ByRef r_sPayeeShortCode As String = "", Optional ByRef r_bIsReversed As Boolean = False) As Integer
        '61600

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimPaymentDetails"

        Dim lReturn As Integer
        Dim oListItem As ListViewItem

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the selected claim payment item
            oListItem = lvwClaimPayments.Items.Item(v_lListItemIndex - 1)

            ' get payment details
            r_lCLaimPaymentId = gPMFunctions.ToSafeLong(m_vUnallocatedClaimPayments(kClaimPaymentDetailsClaimPaymentId, Convert.ToString(oListItem.Tag)), 0)
            r_lMediaTypeId = CInt(ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentMediaTypeId).Text)
            r_lCurrencyId = CInt(ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentCurrencyId).Text)
            r_lDocumentId = CInt(oListItem.Text)
            r_lAccountid = gPMFunctions.ToSafeLong(m_vUnallocatedClaimPayments(kClaimPaymentDetailsAccountId, Convert.ToString(oListItem.Tag)), 0)
            r_cClaimPaymentAmount = gPMFunctions.ToSafeDecimal(m_vUnallocatedClaimPayments(kClaimPaymentDetailsAccountAmount, Convert.ToString(oListItem.Tag)), 0)
            r_lMediaTypeId = gPMFunctions.ToSafeInteger(m_vUnallocatedClaimPayments(kClaimPaymentMediaTypeID, Convert.ToString(oListItem.Tag)), 0)
            r_lBankAccountId = gPMFunctions.ToSafeLong(m_vUnallocatedClaimPayments(kClaimPaymentBankAccountID, Convert.ToString(oListItem.Tag)), 0)
            '61600
            r_iSourceId = gPMFunctions.ToSafeInteger(m_vUnallocatedClaimPayments(kClaimPaymentSourceID, oListItem.Tag), 1)
            '61600
            r_sPayeeName = gPMFunctions.ToSafeString(m_vUnallocatedClaimPayments(kClaimPayeeName, oListItem.Tag), 0)
            r_sOurRef = gPMFunctions.ToSafeString(m_vUnallocatedClaimPayments(kClaimPaymentOurRef, oListItem.Tag), 0)
            r_sTheirRef = gPMFunctions.ToSafeString(m_vUnallocatedClaimPayments(kClaimPaymentTheirRef, oListItem.Tag), 0)
            r_nPartyBankId = gPMFunctions.ToSafeInteger(m_vUnallocatedClaimPayments(kClaimPaymentPartyBankId, oListItem.Tag), 0)
            r_sPayeeAccountNo = gPMFunctions.ToSafeString(m_vUnallocatedClaimPayments(kClaimPaymentPayeeAccountNo, oListItem.Tag), 0)
            r_sPayeeShortCode = gPMFunctions.ToSafeString(m_vUnallocatedClaimPayments(kClaimPaymentPayeeSortCode, oListItem.Tag), 0)
            r_bIsReversed = gPMFunctions.ToSafeBoolean(m_vUnallocatedClaimPayments(kClaimPaymentIsReversed, Convert.ToString(oListItem.Tag)), 0)

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimPaymentDetailsWrapper
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 02-02-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function GetClaimPaymentDetailsWrapper(ByRef r_lMediaTypeId As Integer, ByRef r_lCurrencyId As Integer, ByRef r_vDocuments() As Object, ByRef r_lAccountid As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimPaymentDetailsWrapper"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vDocuments() As Object
        Dim lPaymentCurrencyId, lCurrencyId, lMediaTypeId, lPaymentMediaTypeId As Integer
        Dim bPaymentSet, bMediaTypeSet As Boolean
        Dim lSelectedItemCount, nClaimPayments, lDocumentId, lClaimPaymentId, lAccountID As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get number of unallocated claim payments in the list
            nClaimPayments = lvwClaimPayments.Items.Count

            ' if there are unallocatd claims payments
            If nClaimPayments > 0 Then

                ' for each claim payment item in the list
                For lClaimPayment As Integer = 1 To nClaimPayments

                    ' if the claim payment item is selected
                    If lvwClaimPayments.Items.Item(lClaimPayment - 1).Checked Then

                        ' get the claim payment details for this item
                        lReturn = CType(GetClaimPaymentDetails(lClaimPayment, lClaimPaymentId, lMediaTypeId, lCurrencyId, lDocumentId, lAccountID), gPMConstants.PMEReturnCode)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        ' resize array to hold multiple documents
                        ReDim Preserve vDocuments(lSelectedItemCount)

                        ' add next document to process

                        vDocuments(lSelectedItemCount) = lDocumentId

                        ' get the currency id
                        ' if the currency has not already been retrieved then
                        If (lCurrencyId <> lPaymentCurrencyId) And (Not bPaymentSet) Then
                            ' save the payment currency
                            lPaymentCurrencyId = lCurrencyId
                            bPaymentSet = True
                        ElseIf (lCurrencyId <> lPaymentCurrencyId) And (bPaymentSet) Then
                            ' if it has already been selected and is different to the current
                            ' currency; set default currency to zero and let the cashlist
                            ' component determine the currency to use
                            lPaymentCurrencyId = 0
                        End If

                        ' get the media type id
                        ' if the media type has not already been retrieved then
                        If lMediaTypeId <> lPaymentMediaTypeId And Not bMediaTypeSet Then
                            ' save the media type
                            bMediaTypeSet = False
                            lPaymentMediaTypeId = lMediaTypeId
                        ElseIf (lMediaTypeId <> lPaymentMediaTypeId) And (bMediaTypeSet) Then
                            ' if it has already been selected and is different to the current
                            ' media type; set default media type to zero and let the cashlist
                            ' component determine the currency to use
                            lPaymentMediaTypeId = 0
                        End If

                        ' increment the selected item count
                        lSelectedItemCount += 1

                    End If

                Next

            End If

            ' set the return parameters
            r_lMediaTypeId = lPaymentMediaTypeId
            r_lCurrencyId = lPaymentCurrencyId
            r_vDocuments = vDocuments
            r_lAccountid = lAccountID



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ResizeMe
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 02-02-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function ResizeMe() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ResizeMe"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            fraPayee.Width = Me.Width - VB6.TwipsToPixelsX(240)

            fraUnallocatedClaimPayments.Width = Me.Width - VB6.TwipsToPixelsX(240)
            fraUnallocatedClaimPayments.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) - VB6.PixelsToTwipsY(fraPayee.Height) - 1200)

            lvwClaimPayments.Width = fraUnallocatedClaimPayments.Width - VB6.TwipsToPixelsX(240)
            lvwClaimPayments.Height = fraUnallocatedClaimPayments.Height - VB6.TwipsToPixelsY(840)

            cmdPayAsOne.Top = fraUnallocatedClaimPayments.Height - VB6.TwipsToPixelsY(495)
            cmdPayIndividually.Top = cmdPayAsOne.Top
            cmdSelectAll.Top = cmdPayAsOne.Top
            cmdSettleAll.Top = cmdPayAsOne.Top
            cmdClose.Top = Me.Height - VB6.TwipsToPixelsY(975)
            cmdClose.Left = Me.Width - VB6.TwipsToPixelsX(1830)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessFind
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function ProcessFind() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessFind"

        Dim lReturn, lSubValue As Integer

        Try



            ' if there are no valid search criteria
            If txtAccountCode.Text = "" Then
                If txtDateOfPaymentFrom.Text = "" And txtDateOfPaymentTo.Text = "" Then
                    MessageBox.Show("Please provide an account code or a payment date", "Claim Payment Processing", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf txtDateOfPaymentFrom.Text = "" Or txtDateOfPaymentTo.Text = "" Then
                    MessageBox.Show("Please provide valid payment date interval", "Claim Payment Processing", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf ToSafeDate(txtDateOfPaymentFrom.Text, "00:00:00") > ToSafeDate(txtDateOfPaymentTo.Text, "00:00:00") Then
                    MessageBox.Show("Please provide valid payment date interval", "Claim Payment Processing", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                'Else
                '    If txtDateOfPaymentFrom.Text = "" Or txtDateOfPaymentTo.Text = "" Then
                '        MessageBox.Show("Please provide valid payment date interval", "Claim Payment Processing", MessageBoxButtons.OK, MessageBoxIcon.Information)
                '    End If
                'End If
            End If

            If txtAccountCode.Text <> "" Then
                SilentProcessFindAccountWrapper()
            Else
                'Developer Guide No 113
                'Starts
                If gPMFunctions.ToSafeDate(txtDateOfPaymentFrom.Text, #12/30/1989#) <> #12/30/1989# And gPMFunctions.ToSafeDate(txtDateOfPaymentTo.Text, #12/30/1989#) <> #12/30/1989# Then
                    m_dtDateOfPaymentFrom = gPMFunctions.ToSafeDate(txtDateOfPaymentFrom.Text, #12/30/1989#)
                    m_dtDateOfPaymentTo = gPMFunctions.ToSafeDate(txtDateOfPaymentTo.Text, #12/30/1989#)
                    'Ends
                    ProcessUnallocatedClaimPayments(v_bSearchByPaymentDate:=True)
                End If
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Public Function ShowSummary() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ShowSummary"
        Try

            Dim lReturn As Integer
            Dim ofrmPaymentProcessedSummary As frmPaymentProcessed
            ofrmPaymentProcessedSummary = New frmPaymentProcessed()
            If Information.IsArray(m_vSummary) Then
                With ofrmPaymentProcessedSummary
                    .PaymentArray = VB6.CopyArray(m_vSummary)
                    .ShowDialog()
                End With
            End If
            ofrmPaymentProcessedSummary.Close()
            ofrmPaymentProcessedSummary = Nothing




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ProcessItemChecked
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-04-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function ProcessItemChecked() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ProcessItemChecked"

        Dim lReturn As Integer
        Dim lSelectedAccountCount, lSelectedAccountId, lRowCount As Integer
        Dim bSettleAll, bClaimCashListItemLinkExists As Boolean
        Try

            bClaimCashListItemLinkExists = False
            bSettleAll = True
            For Each oListItem As ListViewItem In lvwClaimPayments.Items


                If oListItem.Checked Then

                    ' get account details
                    If lRowCount = 0 AndAlso lSelectedAccountId = 0 Then
                        lSelectedAccountId = gPMFunctions.ToSafeLong(m_vUnallocatedClaimPayments(kClaimPaymentDetailsAccountId, Convert.ToString(oListItem.Tag)), 0)
                    End If

                    If lSelectedAccountId <> 0 Then
                        If lSelectedAccountId <> gPMFunctions.ToSafeLong(m_vUnallocatedClaimPayments(kClaimPaymentDetailsAccountId, Convert.ToString(oListItem.Tag)), 0) Then
                            lSelectedAccountCount += 1
                        End If
                    End If

                    If ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentMediaType).Text = "" Or ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentBank).Text = "" Or ListViewHelper.GetListViewSubItem(oListItem, kSubItemClaimPaymentStatus).Text = "Awaiting Settlement Authorisation" Then
                        bSettleAll = False
                    End If
                    lRowCount += 1
                End If

            Next oListItem

            For Each oListItem As ListViewItem In lvwClaimPayments.Items
                If oListItem.Checked = True Then
                    If txtAccountCode.Enabled Then
                        If ToSafeLong(m_vUnallocatedClaimPayments(kClaimPaymentCashItemLink, oListItem.Tag), 0) <> 0 AndAlso ToSafeLong(m_vUnallocatedClaimPayments(kClaimPaymentIsReversed, oListItem.Tag), 0) <> 1 Then
                            bClaimCashListItemLinkExists = True
                        End If
                    Else
                        If ToSafeLong(m_vUnallocatedClaimPayments(kClaimPaymentCashItemLink, oListItem.Tag), 0) <> 0 Then
                            bClaimCashListItemLinkExists = True
                        End If
                    End If
                End If
            Next oListItem


            If bClaimCashListItemLinkExists Then
                cmdPayAsOne.Enabled = False
                cmdPayIndividually.Enabled = False
                cmdSettleAll.Enabled = False
            Else
                cmdPayAsOne.Enabled = Not (lSelectedAccountCount > 0)
                cmdPayIndividually.Enabled = True
                cmdSettleAll.Enabled = True
            End If

            cmdSettleAll.Enabled = bSettleAll And Not bClaimCashListItemLinkExists


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    Private Function SaveSummary(ByRef sMediaType As String, ByRef cAmount As Decimal) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SaveSummary"
        Dim iIndex As Integer
        Dim itemExists As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vSummary) Then

                iIndex = m_vSummary.GetUpperBound(1) + 1

                For i As Integer = 0 To m_vSummary.GetUpperBound(1)
                    If CStr(m_vSummary(InterfaceMain.ACListPaymentSummary.PSMediaType, i)) = sMediaType Then
                        m_vSummary(InterfaceMain.ACListPaymentSummary.PSPaymentValue, i) = CDbl(m_vSummary(InterfaceMain.ACListPaymentSummary.PSPaymentValue, i)) + cAmount
                        m_vSummary(InterfaceMain.ACListPaymentSummary.PSPaymentCount, i) = CDbl(m_vSummary(InterfaceMain.ACListPaymentSummary.PSPaymentCount, i)) + 1
                        itemExists = True

                    End If
                Next

                If Not itemExists Then
                    ReDim Preserve m_vSummary(2, iIndex)
                    m_vSummary(InterfaceMain.ACListPaymentSummary.PSMediaType, iIndex) = sMediaType
                    m_vSummary(InterfaceMain.ACListPaymentSummary.PSPaymentValue, iIndex) = cAmount
                    m_vSummary(InterfaceMain.ACListPaymentSummary.PSPaymentCount, iIndex) = 1
                End If
            Else
                ReDim m_vSummary(2, 0)
                m_vSummary(InterfaceMain.ACListPaymentSummary.PSMediaType, 0) = sMediaType
                m_vSummary(InterfaceMain.ACListPaymentSummary.PSPaymentValue, 0) = cAmount
                m_vSummary(InterfaceMain.ACListPaymentSummary.PSPaymentCount, 0) = 1
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally






        End Try
        Return result
    End Function

    Private Sub txtAccountCode_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles txtAccountCode.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        If txtAccountCode.Text <> "" Then
            txtDateOfPaymentFrom.Text = ""
            txtDateOfPaymentTo.Text = ""
            txtDateOfPaymentFrom.Enabled = False
            txtDateOfPaymentTo.Enabled = False
            txtDateOfPaymentFrom.BackColor = SystemColors.Control
            txtDateOfPaymentTo.BackColor = SystemColors.Control
        End If
    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (txtDateOfPayment_KeyDown) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub txtDateOfPayment_KeyDown(ByRef KeyCode As Integer, ByRef Shift As Integer)
    '
    'If KeyCode = Keys.Return Then
    '
    ' without calling the find account interface check if this is a valid account
    'ProcessFind()
    '
    'End If
    '
    'End Sub



    Private Sub txtDateOfPaymentFrom_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles txtDateOfPaymentFrom.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        If txtDateOfPaymentFrom.Text <> "" Then
            txtAccountCode.Text = ""
            txtAccountCode.Enabled = False
            txtAccountCode.BackColor = SystemColors.Control
        End If
    End Sub
    'UPGRADE_NOTE: (7001) The following declaration (txtDateOfPaymentToKeyUp) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub txtDateOfPaymentToKeyUp(ByRef KeyCode As Integer, ByRef Shift As Integer)
    'If txtDateOfPaymentTo.Text <> "" Then
    'txtAccountCode.Text = ""
    'txtAccountCode.Enabled = False
    'txtAccountCode.BackColor = SystemColors.Control
    'End If
    'End Sub
    Private Sub txtDateOfPaymentFrom_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateOfPaymentFrom.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDateOfPaymentFrom)
    End Sub

    Private Sub txtDateOfPaymentTo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateOfPaymentTo.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDateOfPaymentTo)
    End Sub

    Private Sub txtDateOfPaymentFrom_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateOfPaymentFrom.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDateOfPaymentFrom)
    End Sub

    Private Sub txtDateOfPaymentTo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateOfPaymentTo.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDateOfPaymentTo)

    End Sub
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetFieldValidation"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            'From Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDateOfPaymentFrom, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "txtDateOfPaymentFrom - AddNewFormField Failed ")
            End If

            'To Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDateOfPaymentTo, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "txtDateOfPaymentFrom - AddNewFormField Failed ")
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (SortListView) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function SortListView(ByVal v_iIndex As Integer) As Integer
    'Dim result As Integer = 0
    'Const kMethodName As String = "SortListView"
    'On Error GoTo Catch_Renamed
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Tell it that it's not sorted
    'ListViewHelper.SetSortedProperty(lvwClaimPayments, False)
    '
    ' Set the column to sort on
    'ListViewHelper.SetSortKeyProperty(lvwClaimPayments, v_iIndex)
    '
    ' Swap the ascending/descending around
    'If ListViewHelper.GetSortOrderProperty(lvwClaimPayments) = SortOrder.Ascending Then
    'ListViewHelper.SetSortOrderProperty(lvwClaimPayments, SortOrder.Descending)
    'Else
    'ListViewHelper.SetSortOrderProperty(lvwClaimPayments, SortOrder.Ascending)
    'End If
    '
    ' Tell it that it's now sorted
    'ListViewHelper.SetSortedProperty(lvwClaimPayments, True)
    '
    'GoTo Finally_Renamed
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    ' If you want to rollback a transaction or something, do it here
    'Finally_Renamed: '
    '
    'Return result
    'Resume 
    'Return result
    'End Function


    Private Sub lvwClaimPayments_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwClaimPayments.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwClaimPayments.Columns(eventArgs.Column)
        Const kMethodName As String = "lvwClaimPayments_ColumnClick"
        Dim lSubValue As Integer
        Try

            If lvwClaimPayments.Items.Count > 0 Then

                lvwClaimPayments.Items.Item(0).EnsureVisible()
                ' Column click event for the search details
                ' Defer to the common interface
                OnColumnClick(lvwClaimPayments, ColumnHeader)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        Finally


        End Try
        Exit Sub
    End Sub

End Class
