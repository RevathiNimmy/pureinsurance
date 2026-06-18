Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Public Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date:15/07/00
    '
    ' Description: Main interface.
    '
    ' Edit History: Pandu
    ' ***************************************************************** '
    'Replaced iPMFunc.GetResData to GetResData in the whole document

    ' Constant for the functions to identify
    ' which class this is.
    Private Const vbFormCode As Integer = 0
    Private Const ACClass As String = "frmInterface"

    Private Const Column1 As Integer = 1
    Private Const Column2 As Integer = 2
    Private Const Column3 As Integer = 3
    Private Const Column4 As Integer = 4
    Private Const Column5 As Integer = 5
    Private Const Column6 As Integer = 6
    Private Const Column7 As Integer = 7 '2005
    Private Const Column8 As Integer = 8
    Private Const Column9 As Integer = 9
    Private Const Column10 As Integer = 10

    'Constants for Defining Width of Columns in List View

    Private Const ColWidthBroking As Integer = 1700
    Private Const ColWidthUnderWriting As Integer = 1600

    ' Declare an instance of the FormControl object

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String
    Private m_dtEffectiveDate As Date
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_bCloseDatabase As Boolean
    Private m_oDatabase As dPMDAO.Database

    ' Variables for Find Claim
    Private m_lClaimCnt As Integer
    Private m_lSelectedClaimCnt As Integer
    Private m_sClaimRef As String
    Private m_lInsuranceFilecnt As Integer
    Private m_sPolicyRef As String
    Private m_sPolicyHolder As String
    Private m_lRealClaimID As Integer

    ' Declare an instance of the general interface object.
    Private isInitializingComponent As Boolean
    ' Declare an instance of the Business object.
    Private m_oBusiness As Business
    Private m_oFindClaim As bCLMFindClaim.Business
    Private m_oOpenClaim As bOpenClaim.Business
    Private m_oRiskDetails As bCLMRiskDetails.Business
    Private m_oCLMReinsurance As bCLMReinsurance.Form
    Private m_oCLMChangeClaimStatus As bCLMChangeClaimStatus.Business

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    Public m_vSearchData As Object

    Public lstClosedClaims As List(Of ClosedClaim)


    Private Sub Initialise()

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'For viewing the Form in TaskBar
            iPMFunc.ShowFormInTaskBar_Attach()

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUserName, 1, 1, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            m_oBusiness = New Business()
            m_oBusiness.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=0, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            m_oBusiness.SetProcessModes(vTransactionType:="C_CR")

            m_oFindClaim = New bCLMFindClaim.Business()
            m_oFindClaim.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=0, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            m_oFindClaim.SetProcessModes(vTransactionType:="C_CR")

            m_oOpenClaim = New bOpenClaim.Business
            m_oOpenClaim.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=0, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            m_oOpenClaim.SetProcessModes(vTransactionType:="C_CR")

            m_oRiskDetails = New bCLMRiskDetails.Business
            m_oRiskDetails.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=0, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            m_oRiskDetails.SetProcessModes(vTransactionType:="C_CR")

            m_oCLMReinsurance = New bCLMReinsurance.Form
            m_oCLMReinsurance.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=0, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            m_oCLMReinsurance.SetProcessModes(vTransactionType:="C_CR")

            m_oCLMChangeClaimStatus = New bCLMChangeClaimStatus.Business
            m_oCLMChangeClaimStatus.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=1, iSourceId:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=0, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            m_oCLMChangeClaimStatus.SetProcessModes(vTransactionType:="C_CR")

            lstClosedClaims = New List(Of ClosedClaim)

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdFindNow_Click(sender As Object, e As EventArgs) Handles cmdFindNow.Click
        Try

            Dim sQuery As String = String.Empty

            'If chkoption1.Checked Then
            '    sQuery = kGetClaimstoCloseOption1
            'End If
            'If chkOption2.Checked Then
            '    If sQuery = String.Empty Then
            '        sQuery = kGetClaimstoCloseOption2
            '    Else
            '        sQuery &= "union " & kGetClaimstoCloseOption2
            '    End If
            'End If
            'If chkOption3.Checked Then
            '    If sQuery = String.Empty Then
            '        sQuery = kGetClaimstoCloseOption3
            '    Else
            '        sQuery &= "union " & kGetClaimstoCloseOption3
            '    End If
            'End If
            sQuery = kGetClaimstoClose '& PrepareWhereClause()

            DisplayStatusSearching()

            m_lReturn = m_oBusiness.GetClaimDetailsSFU(m_vSearchData, sQuery)

            m_lReturn = DatatoInterface()

            DisplayStatusFound()

        Catch ex As Exception

        End Try
    End Sub

    Public Function PrepareWhereClause() As String
        'Dim sWhereClause As String = " WHERE"
        'Dim sClassOfBusiness As String = String.Empty
        'Dim sClaimStatus As String = String.Empty
        'Dim sProgressStatus As String = String.Empty
        'Dim sProduct As String = String.Empty
        'Dim sTemp As String = String.Empty

        'For Each lstitem As PresentationControls.CheckBoxComboBoxItem In cboClassOfBusiness.CheckBoxItems
        '    If lstitem.Checked Then
        '        If lstitem.Text <> kSelectAllItem Then
        '            sTemp = lstitem.Text.Substring(0, lstitem.Text.IndexOf("-")).Trim()
        '            sClassOfBusiness = IIf(sClassOfBusiness = String.Empty, sTemp, sClassOfBusiness & "," & sTemp)
        '        Else
        '            sClassOfBusiness = String.Empty
        '            Exit For
        '        End If
        '    End If
        'Next
        'For Each lstitem As PresentationControls.CheckBoxComboBoxItem In cboClaimStatus.CheckBoxItems
        '    If lstitem.Checked Then
        '        If lstitem.Text <> kSelectAllItem Then
        '            sTemp = lstitem.Text.Substring(0, lstitem.Text.IndexOf("-")).Trim()
        '            sClaimStatus = IIf(sClaimStatus = String.Empty, sTemp, sClaimStatus & "," & sTemp)
        '        Else
        '            sClaimStatus = String.Empty
        '            Exit For
        '        End If
        '    End If
        'Next
        'For Each lstitem As PresentationControls.CheckBoxComboBoxItem In cboProgressStatus.CheckBoxItems
        '    If lstitem.Checked Then
        '        If lstitem.Text <> kSelectAllItem Then
        '            sTemp = lstitem.Text.Substring(0, lstitem.Text.IndexOf("-")).Trim()
        '            sProgressStatus = IIf(sProgressStatus = String.Empty, sTemp, sProgressStatus & "," & sTemp)
        '        Else
        '            sProgressStatus = String.Empty
        '            Exit For
        '        End If
        '    End If
        'Next
        'For Each lstitem As PresentationControls.CheckBoxComboBoxItem In cboProduct.CheckBoxItems
        '    If lstitem.Checked Then
        '        If lstitem.Text <> kSelectAllItem Then
        '            sTemp = lstitem.Text.Substring(0, lstitem.Text.IndexOf("-")).Trim()
        '            sProduct = IIf(sProduct = String.Empty, sTemp, sProduct & "," & sTemp)
        '        Else
        '            sProduct = String.Empty
        '            Exit For
        '        End If
        '    End If
        'Next

        'sWhereClause = sWhereClause & " C.Claim_Status_id in (" & IIf(sClaimStatus = String.Empty, "C.Claim_Status_id", sClaimStatus) & ")"
        'sWhereClause = sWhereClause & " AND res3.class_of_business_id in (" & IIf(sClassOfBusiness = String.Empty, "res3.class_of_business_id", sClassOfBusiness) & ")"
        'sWhereClause = sWhereClause & " AND C.Progress_Status_id in (" & IIf(sProgressStatus = String.Empty, "C.Progress_Status_id", sProgressStatus) & ")"
        'sWhereClause = sWhereClause & " AND inf.product_id in (" & IIf(sProduct = String.Empty, "inf.product_id", sProduct) & ")"
        'If cboLossDateCriteria.SelectedText <> "--Select--" Then
        '    If cboLossDateCriteria.SelectedItem.ToString() = "EqualTo" Then
        '        sWhereClause = sWhereClause & " AND C.Loss_from_date = '" & dtpLossDateFrom.Value.Year & "-" & dtpLossDateFrom.Value.Month & "-" & dtpLossDateFrom.Value.Day & "'"
        '    ElseIf cboLossDateCriteria.SelectedItem.ToString() = "LessThan" Then
        '        sWhereClause = sWhereClause & " AND C.Loss_from_date < '" & dtpLossDateFrom.Value.Year & "-" & dtpLossDateFrom.Value.Month & "-" & dtpLossDateFrom.Value.Day & "'"
        '    ElseIf cboLossDateCriteria.SelectedItem.ToString() = "GreaterThan" Then
        '        sWhereClause = sWhereClause & " AND C.Loss_from_date > '" & dtpLossDateFrom.Value.Year & "-" & dtpLossDateFrom.Value.Month & "-" & dtpLossDateFrom.Value.Day & "'"
        '    ElseIf cboLossDateCriteria.SelectedItem.ToString() = "Between" Then
        '        sWhereClause = sWhereClause & " AND C.Loss_from_date >= '" & dtpLossDateFrom.Value.Year & "-" & dtpLossDateFrom.Value.Month & "-" & dtpLossDateFrom.Value.Day & "' AND C.Loss_from_date <= '" & dtpLossDateTo.Value.Year & "-" & dtpLossDateTo.Value.Month & "-" & dtpLossDateTo.Value.Day & "'"
        '    End If
        'End If

        'Return sWhereClause
    End Function

    ''' <summary>
    ''' DataToInterface:Updates all interface details from the search data storage.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DataToInterface() As Integer

        Dim nResult As Integer = 0
        Dim oListItem As ListViewItem = Nothing

        Const kACFindImage As String = "FindImage"

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwsearchdetails.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                Return nResult
            End If

            ' Assign the details to the interface.

            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                oListItem = lvwsearchdetails.Items.Add(CStr(lRow), kACFindImage)

                oListItem.SubItems.Add(1).Text = gPMFunctions.ToSafeString(CStr(m_vSearchData(1, lRow))).Trim()

                oListItem.SubItems.Add(2).Text = ""

                oListItem.SubItems.Add(3).Text = CStr(m_vSearchData(2, lRow))

                oListItem.SubItems.Add(4).Text = CStr(m_vSearchData(3, lRow)).Trim()

                oListItem.SubItems.Add(4).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=CStr(m_vSearchData(4, lRow)))

                oListItem.SubItems.Add(6).Text = ""

                oListItem.SubItems.Add(7).Text = ""

                oListItem.SubItems.Add(8).Text = ""

                oListItem.SubItems.Add(9).Text = CStr(m_vSearchData(5, lRow))

                oListItem.SubItems.Add(10).Text = CStr(m_vSearchData(0, lRow))

                oListItem.Tag = CStr(lRow)

                ' Refresh the first X amount of rows, to
                ' allow the user to see the results instantly.
                If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.
                    lvwsearchdetails.Items.Item(0).Selected = True

                    ' Refresh the initial results.
                    lvwsearchdetails.Refresh()
                End If
            Next lRow

            ' Select the first item.
            lvwsearchdetails.Items.Item(0).Selected = True

            ' Enable the interface now that the search has completed.
            'm_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            Return nResult

        End Try
    End Function

    Private Sub lvwsearchdetails_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lvwsearchdetails.ItemChecked
        'If e.Item.Checked Then
        '    Dim iSelectedIndex As Integer = CInt(e.Item.Text)
        '    Dim oClosedClaim As New ClosedClaim
        '    oClosedClaim.ClaimID = CInt(lvwsearchdetails.Items(iSelectedIndex).SubItems(10).Text)
        '    oClosedClaim.ClaimNumber = lvwsearchdetails.Items(iSelectedIndex).SubItems(1).Text
        '    oClosedClaim.Insured = lvwsearchdetails.Items(iSelectedIndex).SubItems(2).Text
        '    oClosedClaim.PolicyID = CInt(lvwsearchdetails.Items(iSelectedIndex).SubItems(3).Text)
        '    oClosedClaim.PolicyNumber = lvwsearchdetails.Items(iSelectedIndex).SubItems(4).Text
        '    oClosedClaim.DateofLoss = lvwsearchdetails.Items(iSelectedIndex).SubItems(5).Text
        '    oClosedClaim.ClassofBusiness = lvwsearchdetails.Items(iSelectedIndex).SubItems(6).Text
        '    oClosedClaim.ClaimStatus = lvwsearchdetails.Items(iSelectedIndex).SubItems(7).Text
        '    oClosedClaim.ClaimProgressStatus = lvwsearchdetails.Items(iSelectedIndex).SubItems(8).Text
        '    oClosedClaim.OutstandingAmount = CDbl(lvwsearchdetails.Items(iSelectedIndex).SubItems(9).Text)
        '    lstClosedClaims.Add(oClosedClaim)
        'End If
    End Sub

    Private Sub cmdView_Click(sender As Object, e As EventArgs) Handles cmdView.Click
        For Each lstSubItem As ListViewItem In lvwsearchdetails.Items
            lstSubItem.Checked = True
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        lstClosedClaims.Clear()
        For Each lstSubItem As ListViewItem In lvwsearchdetails.Items
            lstSubItem.Checked = False
        Next
    End Sub

    Private Sub DisplayStatusSearching()
        Static sMessage As String = ""
        Try
            If sMessage = "" Then
                sMessage = "Searching, Please wait..."
            End If
            ' Display the status message.
            _stbstatus_Panel1.Text = " " & sMessage
            Application.DoEvents()
        Catch excep As System.Exception
            Exit Sub
        End Try
    End Sub

    Private Sub DisplayStatusFound()
        Static sMessage As String = ""
        Dim lItemsFound As Integer
        Try
            ' Store the total of item found.
            If Not Information.IsArray(m_vSearchData) Then
                lItemsFound = 0
            Else

                lItemsFound = (m_vSearchData.GetUpperBound(1) + 1)
            End If
            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = "Item(s) found"
            End If
            ' Display the status message.
            _stbstatus_Panel1.Text = " " & lItemsFound & " " & sMessage
        Catch excep As System.Exception
            Exit Sub
        End Try
    End Sub

    Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim j As Integer = 1
        Dim k As Integer = 1

        

        Try
            'For Each oClosedClaim As ClosedClaim In lstClosedClaims
            For i = 0 To lvwsearchdetails.Items.Count - 1
                If lvwsearchdetails.Items(i).Checked Then
                    m_oDatabase.SQLBeginTrans()

                    m_lClaimCnt = CInt(lvwsearchdetails.Items(i).SubItems(10).Text)

                    m_sClaimRef = lvwsearchdetails.Items(i).SubItems(1).Text

                    m_lInsuranceFilecnt = CInt(lvwsearchdetails.Items(i).SubItems(3).Text)

                    m_sPolicyRef = lvwsearchdetails.Items(i).SubItems(4).Text

                    m_sPolicyHolder = ""

                    _stbstatus_Panel1.Text = "Processing Claim " & m_sClaimRef
                    Application.DoEvents()

                    result = ProcessClaimClosure()
                    If result <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_oDatabase.SQLRollbackTrans()
                    End If

                    m_oDatabase.SQLCommitTrans()
                    Threading.Thread.Sleep(1000)
                    j += 1
                End If
                'Next
            Next i

            _stbstatus_Panel1.Text = "Claim closure process complete."
            Application.DoEvents()
            MessageBox.Show("All claims status set to closed.")

            cmdFindNow_Click(Nothing, Nothing)

        Catch ex As Exception
            m_oDatabase.SQLRollbackTrans()

        End Try
    End Sub

    Private Function ProcessClaimClosure() As Integer
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim lCopyClaimId As Integer
        Dim bApplyReinsurance As Boolean
        Dim lClaimDateChanged As gPMConstants.PMEReturnCode
        Dim r_sClaimOldPolicyRef As String = String.Empty
        Dim m_lOriginalClaimID As Integer

        Try
            m_lReturn = m_oFindClaim.CleanUpDirtyClaims(m_lClaimCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(LockClaim(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMRecordInUse
            End If

            m_lReturn = m_oFindClaim.ProcessCopyClaim(v_lClaimId:=m_lClaimCnt, r_lCopyClaimId:=lCopyClaimId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lClaimCnt = lCopyClaimId

            m_lReturn = m_oBusiness.Update(m_lClaimCnt)

            m_lReturn = m_oRiskDetails.BalanceClaim(m_lClaimCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oCLMReinsurance.ClaimID = m_lClaimCnt
            m_oCLMReinsurance.BalanceAndCloseClaim = True
            m_lReturn = m_oCLMReinsurance.ApplyReinsurance(bApplyReinsurance)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oCLMReinsurance.CalculateRI
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oCLMChangeClaimStatus.UpdateClaimStatus(v_lClaimId:=m_lClaimCnt, v_lClaimStatusID:=3)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ProcessClaimStatus(m_lClaimCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oCLMChangeClaimStatus.FinaliseClaimDetails(v_lClaimId:=m_lClaimCnt, v_sClaimVersionDescription:="Claim balanced and closed automatically")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oCLMChangeClaimStatus.UpdateInsuranceFileSystem(m_lClaimCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            'If m_sTransactionType = "C_CR" Then
            '    Dim sFailureMessage As String = ""
            '    Dim sSystemOptionRollbackReservesForNonBaseCurrency As String = "0"
            '    m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5154,
            '                                        r_sOptionValue:=sSystemOptionRollbackReservesForNonBaseCurrency, v_iSourceID:=g_iSourceID)
            '    ' If yes call below
            '    If sSystemOptionRollbackReservesForNonBaseCurrency = "1" Then
            '        m_lReturn = m_oCLMChangeClaimStatus.RaiseRollbackTransactions(m_lClaimCnt, sFailureMessage, m_sTransactionType)
            '        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            '            If sFailureMessage <> "" Then
            '                'Enter rollbakc report entry for failure
            '                m_oCLMChangeClaimStatus.AddClaimRollbackReportTable(m_lClaimCnt, sFailureMessage)
            '            End If
            '        Else
            '            'Enter rollbakc report entry for failure
            '            m_oCLMChangeClaimStatus.AddClaimRollbackReportTable(m_lClaimCnt, "Error Occured while rollback")

            '            ' Log Error Message
            '            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RaiseRollbackTransactions Failed", _
            '                               vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessInterface", vErrNo:=Information.Err().Number)
            '        End If
            '    End If
            'End If

            m_lReturn = m_oCLMChangeClaimStatus.RaiseTransactions(v_lClaimId:=m_lClaimCnt, v_bSavedStats:=False, r_lDocumentId:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oCLMChangeClaimStatus.UpdateClaimIsDirty(v_lClaimId:=m_lClaimCnt, v_lIsDirty:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oCLMChangeClaimStatus.SetPaymentReferred(m_lClaimCnt, 0)

            m_lReturn = m_oCLMChangeClaimStatus.UpdateClaimDesc(v_lClaimId:=m_lClaimCnt, v_sClaimVersionDescription:="Claim balanced and closed automatically")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oCLMChangeClaimStatus.IsClaimDateChanged(v_lClaimId:=m_lClaimCnt, r_lChanged:=lClaimDateChanged)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If lClaimDateChanged = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oCLMChangeClaimStatus.CreateEvent(v_lClaimId:=m_lClaimCnt, v_lEventType:=PMBConst.PMBEventClaChange, v_sDescription:="Changed Claim Date")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lReturn = m_oCLMChangeClaimStatus.CreateEvent(v_lClaimId:=m_lClaimCnt, v_lEventType:=PMBConst.PMBEventClaChange, v_sDescription:="Claim balanced and closed automatically")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oCLMChangeClaimStatus.GetOriginalClaimId(v_lClaimId:=m_lClaimCnt, r_lOriginalClaimId:=m_lOriginalClaimID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' unlock the claim
            If m_sTransactionType <> "C_CP" Then
                m_lReturn = CType(UnlockClaim(v_lOriginalClaimID:=m_lOriginalClaimID), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result
        Catch ex As Exception

            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    '
    ' Name: LockClaim
    '
    ' Description:
    '
    ' History: 17/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function LockClaim() As Integer
        Dim result As Integer = 0
        Dim oPMLock As bPMLock.User
        Dim sLockedBy As String = ""
        Dim lOriginalClaimId As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            'Get bPMLock
            oPMLock = New bPMLock.User()
            oPMLock.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=0, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=0, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            m_lReturn = m_oFindClaim.GetOriginalClaimId(m_lClaimCnt, lOriginalClaimId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            m_lReturn = oPMLock.LockKey(sKeyName:="claim_id", vKeyValue:=lOriginalClaimId, iUserID:=1, sCurrentlyLockedBy:=sLockedBy, v_bOtherUserOnly:=False)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    'OK
                    m_lRealClaimID = lOriginalClaimId
                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    If sLockedBy = "ERROR" Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("Claim currently locked by " & sLockedBy & _
                                        Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Find Claim")
                        Return result
                    End If
                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select
            oPMLock = Nothing
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            Return result
        End Try
    End Function

    Private Function UnlockClaim(ByVal v_lOriginalClaimID As Integer) As Integer
        Dim result As Integer = 0
        Dim oPMLock As bPMLock.User

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Tracy Richards - Protect against trying to unlcok claims of id = 0,
            'which may be the case for brand new claims, but which do not need unlocking.
            If v_lOriginalClaimID > 0 Then

                'Get bPMLock
                oPMLock = New bPMLock.User()
                oPMLock.Initialise(sUsername:=m_sUserName, sPassword:=m_sPassword, iUserID:=0, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=0, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Return result
                End If


                m_lReturn = oPMLock.UnLockKey(sKeyName:="claim_id", vKeyValue:=v_lOriginalClaimID, iUserID:=1)

                ' DD 26/7/2004 - PN13122
                ' Only error if return = PMError. If return = PMFalse, it just means
                ' the claim was not locked in the first place.
                'If (m_lReturn <> PMTrue) Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unlock claim", vApp:=ACApp, vClass:=ACClass, vMethod:="UnUnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
                oPMLock = Nothing
            End If
            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Private Function ProcessClaimStatus(ByVal v_lClaimId As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray As Object

        On Error GoTo Catch_Renamed
        result = gPMConstants.PMEReturnCode.PMTrue


        If m_oCLMChangeClaimStatus.GetReserveRecoveryOS(v_lClaimId, vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to get outstanding amount for reserve/recovery", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            result = gPMConstants.PMEReturnCode.PMFalse
            GoTo Finally_Renamed
        End If

        If Not Information.IsArray(vResultArray) Then
            MessageBox.Show("Reserve/Recovery details not found", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            result = gPMConstants.PMEReturnCode.PMFalse
            GoTo Finally_Renamed
        End If

        'is it closed?
        ''Saurabh PN  58931
        If gPMFunctions.ToSafeLong(vResultArray(0, 0), 0) = 3 Or gPMFunctions.ToSafeLong(vResultArray(0, 0), 0) = 5 Then
            'warn user and change status back to live if either reserve or recovery is not zero
            If Math.Round(gPMFunctions.ToSafeCurrency(vResultArray(1, 0), 0)) <> 0 Or Math.Round(gPMFunctions.ToSafeCurrency(vResultArray(2, 0), 0)) <> 0 Then
                MessageBox.Show("Claim cannot be closed unless all outstanding reserves and recoveries are set to zero." & Strings.Chr(13) & Strings.Chr(10) & _
                                "Claim will remain open.", "Change Claim Status to Closed", MessageBoxButtons.OK, MessageBoxIcon.Information)

                'change work claim status back to live

                result = m_oCLMChangeClaimStatus.UpdateClaimStatus(v_lClaimId)
            End If
        End If

        GoTo Finally_Renamed

Catch_Renamed:

        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed check claim status and outstanding amount for reserve and recovery", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessClaimStatus()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result
    End Function

    Private Sub frmInterface_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim vArray As Object = Nothing
        Try
            m_lReturn = m_oBusiness.GetProgressStatus(vArray)
            ' m_lReturn = LoadDataInCombo(cboProgressStatus, vArray)

            m_lReturn = m_oBusiness.GetClaimStatus(vArray)
            'm_lReturn = LoadDataInCombo(cboClaimStatus, vArray)

            m_lReturn = m_oBusiness.GetClassOfBusiness(vArray)
            'm_lReturn = LoadDataInCombo(cboClassOfBusiness, vArray)

            m_lReturn = m_oBusiness.GetProduct(vArray)
            'm_lReturn = LoadDataInCombo(cboProduct, vArray)

            FillLossDateCriteria()

        Catch ex As Exception

        End Try
    End Sub

    Public Sub FillLossDateCriteria()
        'cboLossDateCriteria.Items.Add("--Select--")
        'cboLossDateCriteria.Items.Add("Between")
        'cboLossDateCriteria.Items.Add("GreaterThan")
        'cboLossDateCriteria.Items.Add("LessThan")
        'cboLossDateCriteria.Items.Add("EqualTo")
    End Sub

    Private Function LoadDataInCombo(ByRef cboControl As ComboBox, ByVal vntData As Object) As Integer
        Dim result As Integer = 0
        Dim sDescription As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'Check whether an array has been passed
            If Information.IsArray(vntData) Then
                'clear the combobox
                cboControl.Items.Clear()
                'Load the data from the Array to the combobox
                For lCount As Integer = vntData.GetLowerBound(1) To vntData.GetUpperBound(1)
                    sDescription = vntData(0, lCount)
                    cboControl.Items.Add(sDescription)
                Next lCount

                cboControl.Items.Insert(0, kSelectAllItem)
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load data in combobox", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadDataInCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Private Sub cboClassOfBusiness_CheckBoxCheckedChanged(sender As Object, e As EventArgs)
        'If CType(sender, PresentationControls.CheckBoxComboBoxItem).Text = kSelectAllItem Then
        '    For Each lstitem As PresentationControls.CheckBoxComboBoxItem In cboClassOfBusiness.CheckBoxItems
        '        If lstitem.Text <> kSelectAllItem Then
        '            lstitem.Enabled = Not CType(sender, PresentationControls.CheckBoxComboBoxItem).Checked
        '            lstitem.Checked = CType(sender, PresentationControls.CheckBoxComboBoxItem).Checked
        '        End If
        '    Next
        'End If
    End Sub

    Private Sub cboClaimStatus_CheckBoxCheckedChanged(sender As Object, e As EventArgs)
        'If CType(sender, PresentationControls.CheckBoxComboBoxItem).Text = kSelectAllItem Then
        '    For Each lstitem As PresentationControls.CheckBoxComboBoxItem In cboClaimStatus.CheckBoxItems
        '        If lstitem.Text <> kSelectAllItem Then
        '            lstitem.Enabled = Not CType(sender, PresentationControls.CheckBoxComboBoxItem).Checked
        '            lstitem.Checked = CType(sender, PresentationControls.CheckBoxComboBoxItem).Checked
        '        End If
        '    Next
        'End If
    End Sub

    Private Sub cboProgressStatus_CheckBoxCheckedChanged(sender As Object, e As EventArgs)
        'If CType(sender, PresentationControls.CheckBoxComboBoxItem).Text = kSelectAllItem Then
        '    For Each lstitem As PresentationControls.CheckBoxComboBoxItem In cboProgressStatus.CheckBoxItems
        '        If lstitem.Text <> kSelectAllItem Then
        '            lstitem.Enabled = Not CType(sender, PresentationControls.CheckBoxComboBoxItem).Checked
        '            lstitem.Checked = CType(sender, PresentationControls.CheckBoxComboBoxItem).Checked
        '        End If
        '    Next
        'End If
    End Sub

    Private Sub cboProduct_CheckBoxCheckedChanged(sender As Object, e As EventArgs)
        'If CType(sender, PresentationControls.CheckBoxComboBoxItem).Text = kSelectAllItem Then
        '    For Each lstitem As PresentationControls.CheckBoxComboBoxItem In cboProduct.CheckBoxItems
        '        If lstitem.Text <> kSelectAllItem Then
        '            lstitem.Enabled = Not CType(sender, PresentationControls.CheckBoxComboBoxItem).Checked
        '            lstitem.Checked = CType(sender, PresentationControls.CheckBoxComboBoxItem).Checked
        '        End If
        '    Next
        'End If
    End Sub

    Private Sub cboLossDateCriteria_SelectedIndexChanged(sender As Object, e As EventArgs)
        'If cboLossDateCriteria.SelectedItem.ToString() = "Between" Then
        '    dtpLossDateTo.Visible = True
        'Else
        '    dtpLossDateTo.Visible = False
        'End If
    End Sub

    Private Sub lvwsearchdetails_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvwsearchdetails.SelectedIndexChanged

    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        End
    End Sub
End Class
