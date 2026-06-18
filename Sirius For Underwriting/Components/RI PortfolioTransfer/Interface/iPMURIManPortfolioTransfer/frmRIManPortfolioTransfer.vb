Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic.Compatibility
'Developer Guide No. 129
Imports SharedFiles

Partial Friend Class frmRIManPortfolioTransfer
    Inherits System.Windows.Forms.Form

    ' ***************************************************************** '
    ' Form Name: frmRIManPortfolioTransfer
    '
    ' Date: 14/03/2011
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmRIManPortfolioTransfer"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    Private m_lItemsFound As Integer

    Private m_oBusiness As Object

    Private m_oBusiness2007disabled As Object
    Private m_vPTRIPoliciesData(,) As Object
    Private m_bIsReadyToAccept As Boolean
    Dim m_vIsRI2007 As Object
    ' **************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    ' Purpose: E007
    '
    ' Author : Kuljeet Kaur 13/03/2011
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim oListItem As System.Windows.Forms.ListViewItem
        Dim lRow As Integer

        Try

            DataToInterface = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwPolicies.Items.Clear()

            m_lItemsFound = 0

            ' Check that search details are valid before
            ' continuing.
            If (IsArray(m_vPTRIPoliciesData) = False) Then
                Exit Function
            End If

            ' Assign the details to the interface.
            For lRow = LBound(m_vPTRIPoliciesData, 2) To UBound(m_vPTRIPoliciesData, 2)
                ' Assign the details to the first column.
                m_lItemsFound = m_lItemsFound + 1

                oListItem = lvwPolicies.Items.Add(Trim(m_vPTRIPoliciesData(MainModule.enumPTRIData.edBranchDesc, lRow)))

                oListItem.SubItems.Add(Trim(m_vPTRIPoliciesData(MainModule.enumPTRIData.edPTRIStatusDesc, lRow)))

                oListItem.SubItems.Add(Trim(m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsuranceRef, lRow)))

                oListItem.SubItems.Add(Trim(m_vPTRIPoliciesData(MainModule.enumPTRIData.edPartyShortName, lRow)))

                oListItem.SubItems.Add(Trim(m_vPTRIPoliciesData(MainModule.enumPTRIData.edPartyName, lRow)))

                oListItem.SubItems.Add(Trim(m_vPTRIPoliciesData(MainModule.enumPTRIData.edProductDesc, lRow)))

                oListItem.Tag = CStr(lRow)

            Next lRow

            'Refresh the initial results.
            lvwPolicies.Refresh()


            ' Check for errors
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get details.
                DataToInterface = gPMConstants.PMEReturnCode.PMFalse
            End If



        Catch ex As Exception

            DataToInterface = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusFound()

        Static sMessage As String
        Dim lItemsFound As Integer

        Try

            ' Get message text if not already present.
            If (sMessage = "") Then
                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
            If m_lItemsFound > 1 And sMessage = "" Then
                sMessage = "Items found"
            Else
                sMessage = "Item found"
            End If
            ' Display the status message.
            _stbStatus_Panel1.Text = " " & m_lItemsFound & " " & sMessage


        Catch ex As Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Sub

        End Try
    End Sub
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
                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbStatus_Panel1.Text = " " & sMessage



        Catch ex As Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Sub

        End Try
    End Sub

    ' ***************************************************************** '
    ' Name : EnableDisableButtons
    '
    ' Purpose : E007
    '
    ' Author : Kuljeet Kaur 13/03/2011
    ' ***************************************************************** '
    Private Sub EnableDisableButtons()

        Dim lstCurrItem As System.Windows.Forms.ListViewItem
        Dim lAmendCount As Integer
        Dim lUpdateCount As Integer

        Try

            ' disable all
            If (lvwPolicies.Items.Count = 0) Then
                cmdAccept.Enabled = False
                cmdAmend.Enabled = False
                cmdSelectAll.Enabled = False
                Exit Sub
            End If

            For Each lstCurrItem In lvwPolicies.Items
                If lstCurrItem.Selected Then
                    ' check for amend status
                    If m_vPTRIPoliciesData(MainModule.enumPTRIData.edPTRIStatusID, lstCurrItem.Tag) = ksPTRIStatusAmend Then
                        lAmendCount = lAmendCount + 1
                        ' check for update status
                    ElseIf m_vPTRIPoliciesData(MainModule.enumPTRIData.edPTRIStatusID, lstCurrItem.Tag) = ksPTRIStatusUpdate Then
                        lUpdateCount = lUpdateCount + 1
                    End If
                End If
            Next lstCurrItem

            ' enable/disable buttons depending on selection
            If (lAmendCount = 0) And (lUpdateCount = 0) Then
                cmdAccept.Enabled = False
                cmdAmend.Enabled = False
            ElseIf (lAmendCount > 0) And (lUpdateCount > 0) Then
                cmdAccept.Enabled = False
                cmdAmend.Enabled = False
            ElseIf (lAmendCount > 0) And (lUpdateCount = 0) Then
                cmdAccept.Enabled = False
                cmdAmend.Enabled = True
            ElseIf (lAmendCount = 0) And (lUpdateCount > 0) Then
                cmdAccept.Enabled = True
                cmdAmend.Enabled = False
            End If
            cmdSelectAll.Enabled = True



        Catch ex As Exception


            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableDisableButtons Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableDisableButtons", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Sub

        End Try
    End Sub


    ' ***************************************************************** '
    ' Name: PopupMouseMenu
    '
    ' Description: E007
    '
    ' Author: Kuljeet
    '
    ' ***************************************************************** '
    Private Sub PopupMouseMenu()

        Dim lAmendCount, lUpdateCount As Integer
        Dim sBoldMenu As ToolStripMenuItem

        Try

            ' disable all
            If (lvwPolicies.Items.Count = 0) Or (lvwPolicies.FocusedItem Is Nothing) Then
                mnuPopup.Available = False
                Exit Sub
            End If

            For Each lstCurrItem As ListViewItem In lvwPolicies.Items
                If lstCurrItem.Selected Then
                    ' check for amend status
                    If CDbl(m_vPTRIPoliciesData(MainModule.enumPTRIData.edPTRIStatusID, Convert.ToString(lstCurrItem.Tag))) = ksPTRIStatusAmend Then
                        lAmendCount += 1
                        ' check for update status
                    ElseIf CDbl(m_vPTRIPoliciesData(MainModule.enumPTRIData.edPTRIStatusID, Convert.ToString(lstCurrItem.Tag))) = ksPTRIStatusUpdate Then
                        lUpdateCount += 1
                    End If
                End If
            Next lstCurrItem

            ' enable/disable menus depending on selection
            If (lAmendCount = 0) And (lUpdateCount = 0) Then
                mnuPopupAmend.Available = False
                mnuPopupAccept.Available = False
                mnuPopupDivider.Available = False
            ElseIf (lAmendCount > 0) And (lUpdateCount > 0) Then
                mnuPopupAmend.Available = False
                mnuPopupAccept.Available = False
                mnuPopupDivider.Available = False
            ElseIf (lAmendCount > 0) And (lUpdateCount = 0) Then
                mnuPopupAmend.Available = True
                mnuPopupAccept.Available = False
                mnuPopupDivider.Available = True
                sBoldMenu = mnuPopupAmend
            ElseIf (lAmendCount = 0) And (lUpdateCount > 0) Then
                mnuPopupAmend.Available = False
                mnuPopupAccept.Available = True
                mnuPopupDivider.Available = True
                sBoldMenu = mnuPopupAccept
            End If
            mnuPopupSelectAll.Available = True

            If sBoldMenu Is Nothing Then
                Ctx_unresolved.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
            Else
                Ctx_unresolved.Show(Me, 0, PointToClient(Cursor.Position).Y)
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopupMouseMenu Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopupMouseMenu", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub
    Private Function GetBusiness() As Object

        Try

            GetBusiness = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.GetPTRIPolicy(r_vResultArray:=m_vPTRIPoliciesData)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetBusiness = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If



        Catch ex As Exception

            GetBusiness = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBusiness failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: ShowPTRIPolicies
    '
    ' Description: shows/refreshes the Portfolio Transfer RI policies
    '
    ' Purpose: E007
    '
    ' Author : Kuljeet Kaur 13/03/2011
    ' ***************************************************************** '
    Private Function ShowPTRIPolicies() As Integer

        Try

            'Display a searching message.
            DisplayStatusSearching()

            m_lReturn = GetBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occured whilst obtaining the reinsurance policy details", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPTRIPolicies", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Function
            End If

            m_lReturn = DataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occured whilst displaying the reinsurance policy details", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPTRIPolicies", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Function
            End If

            EnableDisableButtons()

            'Display a searching message for number of Policies found
            DisplayStatusFound()

            'Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check for errors
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get details.
                ShowPTRIPolicies = gPMConstants.PMEReturnCode.PMFalse
            End If



        Catch ex As Exception

            ShowPTRIPolicies = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPTRIPolicies", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessAccept
    '
    ' Description: Process the acceptancefor each selected policy
    '
    ' Purpose: E007
    '
    ' Author : Kuljeet Kaur 13/03/2011
    ' ***************************************************************** '
    Public Function ProcessAccept(ByVal v_lSelectedTag As Integer) As Integer

        Dim vbResult As MsgBoxResult
        Dim sFailureMessage As String
        Dim nResult As Integer
        Dim nReturn As Integer
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            If m_bIsReadyToAccept = False Then
                ' hmm, are you sure?
                vbResult = MsgBox("Are you sure you want to process this acceptance?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Accept")
            End If
            If vbResult = MsgBoxResult.Yes OrElse m_bIsReadyToAccept = True Then

                nReturn = m_oBusiness.CreateAndPostStats(nInsuranceFileCnt:=m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsFileCnt, v_lSelectedTag), nPTInsuranceFileCnt:=m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsFileCnt, v_lSelectedTag), bReversePT:=False, sTransactionType:="PT", dtTransferDate:=m_vPTRIPoliciesData(MainModule.enumPTRIData.edTransferDate, v_lSelectedTag), r_sMessage:=sFailureMessage)

                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Call LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccept")

                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Return nResult
                End If

                ' if all is well, remove the accociated record from Insurace_File_PT_RI_Usage
                nReturn = m_oBusiness.DeleteInsFilePTRIUsage(v_lInsFilePTRIUsageID:=m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsFilePTRIUsageID, v_lSelectedTag))

                ' Check for errors.
                If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Return nResult
                End If

            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return nResult
        Catch ex As Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccept Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccept", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_lSelectedTag"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessAccept2007Disabled(ByVal v_lSelectedTag As Integer) As Integer

        Dim vbResult As MsgBoxResult
        Dim sFailureMessage As String
        Dim nReturn As Integer

        Try
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        vbResult = MsgBox("Are you sure you want to process this acceptance?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Accept")

        If vbResult = MsgBoxResult.Yes Then

                m_lReturn = m_oBusiness2007disabled.CreateAndPostStats(v_lInsuranceFileCnt:=m_vPTRIPoliciesData(MainModule.enumPTRIData.edNewInsFileCnt, v_lSelectedTag), v_lPTInsuranceFileCnt:=m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsFileCnt, v_lSelectedTag), v_bReversePT:=True, v_sTransactionType:="PT", r_sMessage:=sFailureMessage)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Call LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccept")

                    nReturn = False
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                End If

            ' if all is well, remove the accociated record from Insurace_File_PT_RI_Usage
                m_lReturn = m_oBusiness2007disabled.DeleteInsFilePTRIUsage(v_lInsFilePTRIUsageID:=m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsFilePTRIUsageID, v_lSelectedTag))

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                nReturn = False
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Return nReturn
            End If

        End If
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        nReturn = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            nReturn = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccept Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccept", vErrNo:=Err.Number, vErrDesc:=Err.Description)

        End Try
        Return nReturn
    End Function
    ''' <summary>
    ''' Displays renewal for amendment and then goes though
    ''' subsequent amendment processing.
    ''' </summary>
    ''' <param name="v_lSelectedTag"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessAmendment(ByVal v_lSelectedTag As Integer) As Integer

        Dim vbResult As MsgBoxResult
        Dim oKeyArray(,) As Object
        Dim sFailureMessage As String
        Dim nNewInsuranceFileCnt As Integer
        Dim nInsuranceFileCnt As Integer
        Dim dTransferDate As Date
        Dim bIsRisksQuoted As Boolean
        Dim dProRataRate As Double
        Dim nIsValid As Integer
        Dim nProductId As Integer
        Dim dtCoverStartDate As Date
        Dim dtCoverEndDate As Date
        Dim dtInceptionDate As Date
        Dim oGetKeyArray(,) As Object
        Dim iLoop As Integer
        Dim nResult As Integer
        Dim nReturn As Integer
        Try
            nResult = PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseBusy)

            ' hmm, are you sure?
            vbResult = MsgBox("Are you sure you want to process this amendment?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Amend")

            If vbResult = MsgBoxResult.Yes Then

                nInsuranceFileCnt = m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsFileCnt, v_lSelectedTag)
                dTransferDate = m_vPTRIPoliciesData(MainModule.enumPTRIData.edTransferDate, v_lSelectedTag)
                dtCoverStartDate = TosafeDate(m_vPTRIPoliciesData(MainModule.enumPTRIData.edCoverStartDate, v_lSelectedTag))
                dtCoverEndDate = TosafeDate(m_vPTRIPoliciesData(MainModule.enumPTRIData.edCoverEndDate, v_lSelectedTag))
                dtInceptionDate = TosafeDate(m_vPTRIPoliciesData(MainModule.enumPTRIData.edInceptionDate, v_lSelectedTag))
                nProductId = Tosafeinteger(m_vPTRIPoliciesData(MainModule.enumPTRIData.edProductID, v_lSelectedTag))

                nReturn = m_oBusiness.GetProRataRate(nProductID:=nProductId, dtOldStartDate:=dtCoverStartDate, dtOldEndDate:=dtCoverEndDate, dtStartDate:=dTransferDate, dtEndDate:=dtCoverEndDate, o_dProRataRate:=dProRataRate, o_dtInceptionDate:=dtInceptionDate)
                If nReturn <> PMEReturnCode.PMTrue Then
                    RaiseError("ProcessAmendment", sFailureMessage)
                End If

                nReturn = m_oBusiness.RecalculateRI(nInsuranceFileCnt:=nInsuranceFileCnt, dtTransferDate:=dTransferDate, dProRataRate:=dProRataRate, nIsPT:=1, r_nIsValid:=nIsValid, bIsForAmend:=True)

                If nReturn <> PMEReturnCode.PMTrue Then
                    RaiseError("ProcessAmendment", sFailureMessage)
                End If

                'run policy
                ReDim oKeyArray(1, 4)
                oKeyArray(0, 0) = "party_cnt"
                oKeyArray(1, 0) = m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsuredCnt, v_lSelectedTag)
                oKeyArray(0, 1) = "insurance_file_cnt"
                oKeyArray(1, 1) = nInsuranceFileCnt
                oKeyArray(0, 2) = "insurance_folder_cnt"
                oKeyArray(1, 2) = m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsFolderCnt, v_lSelectedTag)
                oKeyArray(0, 3) = "shortname"
                oKeyArray(1, 3) = m_vPTRIPoliciesData(MainModule.enumPTRIData.edPartyShortName, v_lSelectedTag)
                oKeyArray(0, 4) = "Product_id"
                oKeyArray(1, 4) = m_vPTRIPoliciesData(MainModule.enumPTRIData.edProductID, v_lSelectedTag)

                nReturn = RunProcess(v_sComponent:="iPMUPolicy.NavigatorV3", v_vKeyArray:=oKeyArray, r_sFailureMessage:=sFailureMessage, v_lProcessMode:=PMEComponentAction.PMView, v_sTransactionType:="PT")

                If nReturn <> PMEReturnCode.PMTrue Then
                    If nReturn = PMEReturnCode.PMCancel Then
                        If nNewInsuranceFileCnt > 0 Then
                            DeletePolicy(nNewInsuranceFileCnt)
                        End If
                        Return PMEReturnCode.PMCancel
                    Else
                        RaiseError("ProcessAmendment", sFailureMessage)
                    End If
                End If

                'run list risks
                ReDim oKeyArray(1, 1)
                oKeyArray(0, 0) = "insurance_file_cnt"
                oKeyArray(1, 0) = nInsuranceFileCnt
                oKeyArray(0, 1) = "insurance_folder_cnt"
                oKeyArray(1, 1) = m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsFolderCnt, v_lSelectedTag)
                ReDim oGetKeyArray(1, 0)
                oGetKeyArray(0, 0) = "Is_Ready_To_Accept"
                oGetKeyArray(1, 0) = m_bIsReadyToAccept

                nReturn = RunProcess(v_sComponent:="iPMUListRisks.NavigatorV3", r_sFailureMessage:=sFailureMessage, v_vKeyArray:=oKeyArray, r_vGetKeyArray:=oGetKeyArray, v_lProcessMode:=PMEComponentAction.PMView, v_sTransactionType:="PT")

                If nReturn <> PMEReturnCode.PMTrue Then
                    If nReturn = PMEReturnCode.PMCancel Then
                        DeletePolicy(nNewInsuranceFileCnt)
                        Return PMEReturnCode.PMCancel
                    Else
                        RaiseError("ProcessAmendment", sFailureMessage)
                    End If
                End If
                For iLoop = 0 To oGetKeyArray.GetUpperBound(1)

                    Select Case oGetKeyArray(0, iLoop)

                        Case "Is_Ready_To_Accept"
                            m_bIsReadyToAccept = CBool(oGetKeyArray(1, iLoop))
                    End Select
                Next iLoop

                nReturn = m_oBusiness.GetAllRiskStatus(nInsuranceFileCnt, bIsRisksQuoted)

                If bIsRisksQuoted Then

                    If m_oBusiness.SetPTRIStatus(v_lInsFilePTRIUsageID:=m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsFilePTRIUsageID, v_lSelectedTag), v_lInsFileCnt:=nInsuranceFileCnt, v_lPTRIStatusID:=ksPTRIStatusUpdate) <> PMEReturnCode.PMTrue Then
                        sFailureMessage = "Failed to update Insurance_File_PT_RI_Usage status to awaiting update"
                        RaiseError("ProcessAmendment", sFailureMessage)
                    End If
                Else

                    If m_oBusiness.SetPTRIStatus(v_lInsFilePTRIUsageID:=m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsFilePTRIUsageID, v_lSelectedTag), v_lInsFileCnt:=nInsuranceFileCnt, v_lPTRIStatusID:=ksPTRIStatusAmend) <> PMEReturnCode.PMTrue Then
                        sFailureMessage = "Failed to update Insurance_File_PT_RI_Usage status to awaiting amend"
                        RaiseError("ProcessAmendment", sFailureMessage)
                    End If

                End If
                If m_bIsReadyToAccept Then
                    nReturn = ProcessAccept(v_lSelectedTag)
                    If nReturn <> 1 Then
                        sFailureMessage = "Failed to accept "
                        RaiseError("ProcessAmendment", sFailureMessage)
                    End If

                End If
            End If
            Return nResult
        Catch ex As Exception

            nResult = PMEReturnCode.PMError

            If sFailureMessage = "" Then
                sFailureMessage = "Failed to Process the renewal Amendment"
            End If

            LogMessagePopup(iType:=PMELogLevel.PMLogOnError, sMsg:=sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAmendment", excep:=ex)

        Finally
            'delete new policy version if we failed any of the steps above
            If nReturn <> PMEReturnCode.PMTrue Then
                If nNewInsuranceFileCnt > 0 Then
                    DeletePolicy(nNewInsuranceFileCnt)
                End If
            End If

            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)

        End Try
        Return nResult
    End Function
    Public Function ProcessAmendment2007Disabled(ByVal v_lSelectedTag As Integer) As Integer
        Dim nPolicyCnt As Integer
        Dim lStatus As Integer
        Dim vbResult As MsgBoxResult
        Dim vKeyArray(,) As Object
        Dim sFailureMessage As String
        Dim nNewInsuranceFileCnt As Integer
        Dim lInsuranceFileCnt As Integer
        Dim dTransferDate As Date
        Dim bIsRisksQuoted As Boolean
        Dim dtPolicyStartDate As Date
        Dim nReturn As Integer



        Try
            nReturn = gPMConstants.PMEReturnCode.PMTrue
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            ' hmm, are you sure?
            vbResult = MsgBox("Are you sure you want to process this amendment?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Amend")
            If vbResult = MsgBoxResult.Yes Then
                If ToSafeLong(m_vPTRIPoliciesData(MainModule.enumPTRIData.edNewInsFileCnt, v_lSelectedTag)) = 0 Then
                    lInsuranceFileCnt = m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsFileCnt, v_lSelectedTag)
                    dTransferDate = m_vPTRIPoliciesData(MainModule.enumPTRIData.edTransferDate, v_lSelectedTag)

                    'create deferred policy version
                    If m_oBusiness2007disabled.CopyPolicyHeader(v_lInsuranceFileCnt:=lInsuranceFileCnt, r_lNewInsurancefileCnt:=nNewInsuranceFileCnt, r_sMessage:=sFailureMessage, v_sSetOldInsuranceFileStatus:=SIRInsFileStatusReplacedPT, v_sTransactionType:="PT", v_dTransferDate:=dTransferDate) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw (New Exception)
                    End If

                    If m_oBusiness2007disabled.CopyAllRisk(v_nInsuranceFileCnt:=lInsuranceFileCnt, v_nNewInsuranceFileCnt:=nNewInsuranceFileCnt, v_nInsuranceFolderCnt:=m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsFolderCnt, v_lSelectedTag), v_dtPolicyStartDate:=dtPolicyStartDate, v_sTransactionType:="PT", r_sMessage:=sFailureMessage, v_dtTransferDate:=dTransferDate, v_bIgnoreError:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                    End If
                Else
                    nNewInsuranceFileCnt = m_vPTRIPoliciesData(MainModule.enumPTRIData.edNewInsFileCnt, v_lSelectedTag)
                    End If
                'run policy
                ReDim vKeyArray(1, 4)
                vKeyArray(0, 0) = "party_cnt"
                vKeyArray(1, 0) = m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsuredCnt, v_lSelectedTag)
                vKeyArray(0, 1) = "insurance_file_cnt"
                vKeyArray(1, 1) = nNewInsuranceFileCnt
                vKeyArray(0, 2) = "insurance_folder_cnt"
                vKeyArray(1, 2) = m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsFolderCnt, v_lSelectedTag)
                vKeyArray(0, 3) = "shortname"
                vKeyArray(1, 3) = m_vPTRIPoliciesData(MainModule.enumPTRIData.edPartyShortName, v_lSelectedTag)
                vKeyArray(0, 4) = "Product_id"
                vKeyArray(1, 4) = m_vPTRIPoliciesData(MainModule.enumPTRIData.edProductID, v_lSelectedTag)

                nReturn = RunProcess(v_sComponent:="iPMUPolicy.NavigatorV3", v_vKeyArray:=vKeyArray, r_sFailureMessage:=sFailureMessage)

                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If nReturn = gPMConstants.PMEReturnCode.PMCancel Then
                        Exit Function
                    Else

                        Throw (New Exception)
                    End If
                End If

                'run list risks
                ReDim vKeyArray(1, 1)
                vKeyArray(0, 0) = "insurance_file_cnt"
                vKeyArray(1, 0) = nNewInsuranceFileCnt
                vKeyArray(0, 1) = "insurance_folder_cnt"
                vKeyArray(1, 1) = m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsFolderCnt, v_lSelectedTag)
                nReturn = RunProcess(v_sComponent:="iPMUListRisks.NavigatorV3", r_sFailureMessage:=sFailureMessage, v_vKeyArray:=vKeyArray)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If nReturn = gPMConstants.PMEReturnCode.PMError Then
                        Exit Function
                    Else
                        Throw (New Exception)
                    End If
                End If
                nReturn = m_oBusiness2007disabled.GetAllRiskStatus(nNewInsuranceFileCnt, bIsRisksQuoted)
                If bIsRisksQuoted Then
                    If m_oBusiness2007disabled.SetPTRIStatus(v_lInsFilePTRIUsageID:=m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsFilePTRIUsageID, v_lSelectedTag), v_lInsFileCnt:=nNewInsuranceFileCnt, v_lPTRIStatusID:=ksPTRIStatusUpdate) <> gPMConstants.PMEReturnCode.PMTrue Then
                        sFailureMessage = "Failed to update Insurance_File_PT_RI_Usage status to awaiting update"
                    End If
                Else
                    If m_oBusiness2007disabled.SetPTRIStatus(v_lInsFilePTRIUsageID:=m_vPTRIPoliciesData(MainModule.enumPTRIData.edInsFilePTRIUsageID, v_lSelectedTag), v_lInsFileCnt:=nNewInsuranceFileCnt, v_lPTRIStatusID:=ksPTRIStatusAmend) <> gPMConstants.PMEReturnCode.PMTrue Then
                        sFailureMessage = "Failed to update Insurance_File_PT_RI_Usage status to awaiting amend"
                    End If

                End If
            End If
        Catch ex As Exception

            nReturn = gPMConstants.PMEReturnCode.PMError

            If sFailureMessage = "" Then
                sFailureMessage = "Failed to Process the renewal Amendment"
            End If
      LogMessagePopup(iType:=PMELogLevel.PMLogOnError, sMsg:=sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAmendment2007Disabled", excep:=ex)
            Finally
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso nReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                If m_oBusiness2007disabled.DeletePolicy(v_lInsuranceFileCnt:=nNewInsuranceFileCnt) = gPMConstants.PMEReturnCode.PMTrue Then
                    'reset old policy version back to NULL
                    If m_oBusiness2007disabled.SetPolicyStatus(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_lInsuranceFileStatusID:=0) <> gPMConstants.PMEReturnCode.PMTrue Then
                        sFailureMessage = "Failed to set old policy status back to LIVE - " & lInsuranceFileCnt
                        Call LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAmendment")
                    End If
                Else
                    sFailureMessage = "Failed to delete new policy version - " & nNewInsuranceFileCnt
                    Call LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAmendment")
                End If
            End If
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try

        Return nReturn
    End Function

    Private Function DeletePolicy(ByVal nInsuranceFileCnt As Integer) As Integer
        Dim sFailureMessage As String

        If m_oBusiness.DeletePolicy(v_lInsuranceFileCnt:=nInsuranceFileCnt) = gPMConstants.PMEReturnCode.PMTrue Then
            'reset old policy version back to NULL
            If m_oBusiness.SetPolicyStatus(v_lInsuranceFileCnt:=nInsuranceFileCnt, v_lInsuranceFileStatusID:=0) <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailureMessage = "Failed to set old policy status back to LIVE - " & nInsuranceFileCnt
                Call LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAmendment")
            End If
        Else
            sFailureMessage = "Failed to delete new policy version - " & nInsuranceFileCnt
            Call LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAmendment")
        End If
        Return gPMConstants.PMEReturnCode.PMTrue

    End Function
    '*******************************************************************************************************
    'Desc: run relevant component with provided keys and get back required keys from component if required
    '*******************************************************************************************************
    Private Function RunProcess(ByVal v_sComponent As String, ByVal v_vKeyArray(,) As Object, ByRef r_sFailureMessage As String, Optional ByRef r_vGetKeyArray(,) As Object = Nothing, Optional ByVal v_bDisplayMessage As Boolean = True, Optional ByVal v_lProcessMode As Integer = gPMConstants.PMEComponentAction.PMEdit, Optional ByVal v_sTransactionType As String = "DRI") As Integer

        Dim oObject As Object
        Dim bNavigatorV3 As Boolean

        Try
            RunProcess = gPMConstants.PMEReturnCode.PMTrue

            'are we using NavigatorV3 or interface class?
            bNavigatorV3 = (InStr(1, UCase(v_sComponent), ".NAVIGATORV3") <> 0)

            'create an instance of required object
            m_lReturn = g_oObjectManager.GetInstance(oObject:=oObject, sClassName:=v_sComponent, vInstanceManager:=PMGetLocalInterface)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RunProcess = gPMConstants.PMEReturnCode.PMFalse
                r_sFailureMessage = "Failed to instantiate object " & v_sComponent
                If v_bDisplayMessage Then
                    MsgBox(r_sFailureMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, ACApp)
                End If
                Return 0
            End If

            If bNavigatorV3 Then
                'pass in relevant keys
                If oObject.NavigatorV3_SetKeys(v_vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                    RunProcess = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to set relevant Keys to " & v_sComponent
                    If v_bDisplayMessage Then
                        MsgBox(r_sFailureMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, ACApp)
                    End If
                    Return 0
                End If

                'set process mode
                If oObject.NavigatorV3_SetProcessModes(vTask:=v_lProcessMode, vTransactionType:=v_sTransactionType) <> gPMConstants.PMEReturnCode.PMTrue Then
                    RunProcess = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to set process mode to " & v_sComponent
                    If v_bDisplayMessage Then
                        MsgBox(r_sFailureMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, ACApp)
                    End If
                    Return 0
                End If

                'start component
                If oObject.NavigatorV3_Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                    RunProcess = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to start" & v_sComponent
                    If v_bDisplayMessage Then
                        MsgBox(r_sFailureMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, ACApp)
                    End If
                    Return 0
                End If

                'get keys back if required
                If Not IsNothing(r_vGetKeyArray) Then
                    If oObject.NavigatorV3_GetKeys(vKeyArray:=r_vGetKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                        RunProcess = gPMConstants.PMEReturnCode.PMFalse
                        r_sFailureMessage = "Failed to get keys from " & v_sComponent
                        If v_bDisplayMessage Then
                            MsgBox(r_sFailureMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, ACApp)
                        End If
                        Return 0
                    End If
                End If

                'did we cancel or error?
                If oObject.NavigatorV3_Status = gPMConstants.PMEReturnCode.PMError Then
                    r_sFailureMessage = "Error in " & v_sComponent
                    RunProcess = gPMConstants.PMEReturnCode.PMError
                ElseIf oObject.NavigatorV3_Status = gPMConstants.PMEReturnCode.PMCancel Then
                    r_sFailureMessage = "Process was cancelled"
                    RunProcess = gPMConstants.PMEReturnCode.PMCancel
                End If

            Else 'we are using interface class
                'pass in relevant keys
                If oObject.SetKeys(v_vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                    RunProcess = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to set relevant Keys to " & v_sComponent
                    If v_bDisplayMessage Then
                        MsgBox(r_sFailureMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, ACApp)
                    End If
                    Return 0
                End If

                'set process mode
                If oObject.SetProcessModes(vTask:=v_lProcessMode) <> gPMConstants.PMEReturnCode.PMTrue Then
                    RunProcess = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to set process mode to " & v_sComponent
                    If v_bDisplayMessage Then
                        MsgBox(r_sFailureMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, ACApp)
                    End If
                    Return 0
                End If

                'start component
                If oObject.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                    RunProcess = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to start" & v_sComponent
                    If v_bDisplayMessage Then
                        MsgBox(r_sFailureMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, ACApp)
                    End If
                    Return 0
                End If

                'get keys back if required
                If Not IsNothing(r_vGetKeyArray) Then
                    If oObject.GetKeys(vKeyArray:=r_vGetKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                        RunProcess = gPMConstants.PMEReturnCode.PMFalse
                        r_sFailureMessage = "Failed to get keys from " & v_sComponent
                        If v_bDisplayMessage Then
                            MsgBox(r_sFailureMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, ACApp)
                        End If
                        Return 0
                    End If
                End If

                'did we cancel or error?
                If oObject.Status = gPMConstants.PMEReturnCode.PMError Then
                    r_sFailureMessage = "Error in " & v_sComponent
                    RunProcess = gPMConstants.PMEReturnCode.PMError
                ElseIf oObject.Status = gPMConstants.PMEReturnCode.PMCancel Then
                    r_sFailureMessage = "Process was cancelled"
                    RunProcess = gPMConstants.PMEReturnCode.PMCancel
                End If

            End If 'NavigatorV3



        Catch ex As Exception
            RunProcess = gPMConstants.PMEReturnCode.PMError

            r_sFailureMessage = "Failed to run component " & v_sComponent

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="RunProcess()", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Return 0

        Finally

            If Not (oObject Is Nothing) Then
                oObject.Dispose()
                oObject = Nothing
            End If



        End Try
        Exit Function
    End Function
    Private Sub cmdAccept_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAccept.Click

        Dim lCurrItemIndex As Integer
        Dim lstCurrItem As System.Windows.Forms.ListViewItem

        Try
            ' Check if there are any items available.
            If (lvwPolicies.Items.Count = 0) Then
                Exit Sub
            End If

            ' loop thru the selected items
            For Each lstCurrItem In lvwPolicies.Items
                If lstCurrItem.Selected Then
                    ' process that amendment

                    If (m_vIsRI2007 = "1") Then
                        m_lReturn = ProcessAccept(v_lSelectedTag:=lstCurrItem.Tag)
                    Else
                        m_lReturn = ProcessAccept2007Disabled(v_lSelectedTag:=lstCurrItem.Tag)
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If
                    ' store the current item's index - set it back when the listview has refreshed
                    lCurrItemIndex = lstCurrItem.Index

                End If

            Next lstCurrItem

            ' refresh the listview
            ShowPTRIPolicies()

        Catch ex As Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error occured whilst amending the policy", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAmend", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Sub


    Private Sub cmdAmend_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAmend.Click

        Dim nCurrItemIndex As Integer
        Dim lstCurrItem As System.Windows.Forms.ListViewItem

        Try
            ' Check if there are any items available.
            If (lvwPolicies.Items.Count = 0) Then
                Exit Sub
            End If

            ' loop thru the selected items
            For Each lstCurrItem In lvwPolicies.Items
                If lstCurrItem.Selected Then
                    ' process that amendment
                    If (m_vIsRI2007 = "1") Then
                        m_lReturn = ProcessAmendment(v_lSelectedTag:=lstCurrItem.Tag)
                    Else
                        m_lReturn = ProcessAmendment2007Disabled(v_lSelectedTag:=lstCurrItem.Tag)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    ' store the current item's index - set it back when the listview has refreshed
                    nCurrItemIndex = lstCurrItem.Index
                End If

            Next lstCurrItem

            ' refresh the listview
            ShowPTRIPolicies()

            ' select the item which was last selected
            If lvwPolicies.Items.Count > 0 Then
                ' unselect the first item
                'lvwPolicies.Items.Item(1).Selected = False
                ' select the one we want
            End If



        Catch ex As Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error occured whilst amending the policy", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAmend", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
        Me.Close()
    End Sub

    Private Sub cmdSelectAll_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelectAll.Click

        Dim lstCurrItem As System.Windows.Forms.ListViewItem

        For Each lstCurrItem In lvwPolicies.Items
            lstCurrItem.Selected = True
        Next lstCurrItem

        EnableDisableButtons()

        lvwPolicies.Focus()

    End Sub


    Private Sub Form_Initialize_Renamed()
        ' Forms initialise event.
        Dim sMessage As String
        Dim sTitle As String

        Try

            ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Create business  object
            ' Create business  object
            m_lReturn = g_oObjectManager.GetInstance(oObject:=m_oBusiness, sClassName:="bSIRRIPortfolioTransfer.Business", vInstanceManager:=PMGetViaClientManager)
            m_lReturn = g_oObjectManager.GetInstance(oObject:=m_oBusiness2007disabled, sClassName:="bSIRRIPortfolioTransfer.RI2007DisabledBusiness", vInstanceManager:=PMGetViaClientManager)
            If bPMFunc.getProductOptionValue(v_sUsername:="", _
                       v_sPassword:="", v_iUserID:=0, _
                       v_iMainSourceID:=0, v_iLanguageID:=0, _
                       v_iCurrencyID:=0, v_iLogLevel:=0, _
                       v_sCallingAppName:="", _
                       v_vOptionNumber:=SIRHiddenOptions.SIROPTEnableRI2007, _
                       v_vBranch:=g_iSourceID, _
                       r_vUnderwriting:=m_vIsRI2007) Then
            End If

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.
                sTitle = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString)

                sMessage = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString)

                ' Display message.
                MsgBox(sMessage, MsgBoxStyle.Critical, sTitle)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)




        Catch ex As Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)


        End Try
    End Sub

    Private Sub frmRIManPortfolioTransfer_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Try

            ShowFormInTaskBar_Detach()

            m_lReturn = ShowPTRIPolicies()



        Catch ex As Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)



        End Try
    End Sub

    Private Sub frmRIManPortfolioTransfer_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize

        Dim lFrameLeft As Integer
        Dim lFrameTop As Integer
        Dim lFrameWidth As Integer
        Dim lFrameHeight As Integer

        ' store values locally for speed
        lFrameLeft = VB6.PixelsToTwipsX(fraPolicies.Left)
        lFrameTop = VB6.PixelsToTwipsY(fraPolicies.Top)

        ' resize frame
        fraPolicies.Width = VB6.TwipsToPixelsX(System.Math.Abs(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - (lFrameLeft * 2)))
        fraPolicies.Height = VB6.TwipsToPixelsY(System.Math.Abs(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - (lFrameTop + 900)))

        lFrameWidth = VB6.PixelsToTwipsX(fraPolicies.Width)
        lFrameHeight = VB6.PixelsToTwipsY(fraPolicies.Height)

        ' resize listview
        lvwPolicies.Width = VB6.TwipsToPixelsX(System.Math.Abs(lFrameWidth - (VB6.PixelsToTwipsX(lvwPolicies.Left) * 2)))
        lvwPolicies.Height = VB6.TwipsToPixelsY(System.Math.Abs(lFrameHeight - (VB6.PixelsToTwipsX(lvwPolicies.Left) * 3)))

        ' move buttons
        cmdAmend.Top = VB6.TwipsToPixelsY(lFrameTop + lFrameHeight + 120)
        cmdAccept.Top = cmdAmend.Top
        cmdSelectAll.Top = cmdAmend.Top
        cmdOK.SetBounds(VB6.TwipsToPixelsX(System.Math.Abs((lFrameLeft + lFrameWidth) - VB6.PixelsToTwipsX(cmdOK.Width))), cmdAmend.Top, 0, 0, Windows.Forms.BoundsSpecified.X Or Windows.Forms.BoundsSpecified.Y)

    End Sub

    Private Sub frmRIManPortfolioTransfer_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ' Terminate the business object
        m_oBusiness.Dispose()

        ' Destroy the instance of the renewals business object
        ' from memory.
        m_oBusiness = Nothing

    End Sub
    Private Sub lvwPolicies_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lvwPolicies.Click
        EnableDisableButtons()
    End Sub

    Private Sub lvwPolicies_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ColumnClickEventArgs) Handles lvwPolicies.ColumnClick
        Const kMethodName As String = "lvwPolicies_ColumnClick"
        Dim ColumnHeader As System.Windows.Forms.ColumnHeader = lvwPolicies.Columns(eventArgs.Column)
        Dim lDirection As Integer
        Dim lReturn As Integer

        ' Column click event for the search details


        Try

            With lvwPolicies

                ' If date column clicked, then sort by date sort column
                If ListViewHelper.GetSortOrderProperty(lvwPolicies) = 1 Then
                    lDirection = SortOrder.Descending
                Else
                    lDirection = SortOrder.Ascending
                End If
                ' Changes Done by : Krishna Nand
                ' Purpose: correct the sorting on Date Column
                ' PN: 67176
                ' Dated: 04/02/2010
                If ColumnHeader.Index + 1 - 1 = 2 Then
                    ''If (ColumnHeader.Index - 1 = 6) Then
                    'TN20010425 Start
                    '.Sorted = False

                    'If (.SortKey <> 5) Then
                    ''If (.SortKey <> 4) Then
                    ''.SortKey = 4
                    '.SortKey = 5

                    ''    iDirection = 0
                    ''Else


                    ''End If
                    'End of Changes done by Krishna Nand on 04/02/2010 for PN 67176
                    'developer guide no. 178
                    m_lReturn = CType(ListViewFunc.ListViewSortByDate(v_oListView:=lvwPolicies, v_iSourceColumn:=2, v_iDirection:=lDirection), gPMConstants.PMEReturnCode)

                    '            .Sorted = True
                    'TN20010425 End

                    ' If current sort column header is
                    ' pressed.
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwPolicies)) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwPolicies, lDirection)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwPolicies, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwPolicies, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwPolicies, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwPolicies, True)
                End If
            End With


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        Finally


        End Try
    End Sub


    Private Sub lvwPolicies_KeyUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles lvwPolicies.KeyUp
        Dim KeyCode As Short = eventArgs.KeyCode
        Dim Shift As Short = eventArgs.KeyData \ &H10000

        ' trigger the click event when user uses cursor keys
        Select Case KeyCode
            Case System.Windows.Forms.Keys.Up, System.Windows.Forms.Keys.Down, System.Windows.Forms.Keys.Home, System.Windows.Forms.Keys.End, System.Windows.Forms.Keys.PageUp, System.Windows.Forms.Keys.PageDown

                lvwPolicies_Click(lvwPolicies, New System.EventArgs())

        End Select

    End Sub
    Private Sub lvwPolicies_MouseUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles lvwPolicies.MouseUp
        Dim Button As Short = eventArgs.Button \ &H100000
        Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        If Button = VB6.MouseButtonConstants.RightButton Then
            'PopupMouseMenu()
        End If

    End Sub
    Public Sub mnuPopupAccept_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuPopupAccept.Click
        cmdAccept_Click(cmdAccept, New System.EventArgs())
    End Sub

    Public Sub mnuPopupAmend_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuPopupAmend.Click
        cmdAmend_Click(cmdAmend, New System.EventArgs())
    End Sub

    Public Sub mnuPopupSelectAll_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuPopupSelectAll.Click
        cmdSelectAll_Click(cmdSelectAll, New System.EventArgs())
    End Sub

End Class
