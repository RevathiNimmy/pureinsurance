Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic.Compatibility.VB6
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmClonedRIManual
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmClonedRIManual"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    'Private m_sTransactionType As String
    Private m_vClonedRIPoliciesData(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    Private m_lItemsFound As Integer
    Private m_bIsRenewalAmend As Boolean
    Private m_lProductId As Integer
    Private m_sRenewalDate As String
    Private m_oBusiness As Object
    Private m_bIsReadyToAccept As Boolean
    Private m_bIsAllRiskQuoted As Boolean

    Public Property RenewalDate() As String
        Get
            RenewalDate = m_sRenewalDate
        End Get
        Set(ByVal Value As String)
            m_sRenewalDate = Value
        End Set
    End Property

    Public Property ProductId() As Integer
        Get
            ProductId = m_lProductId
        End Get
        Set(ByVal Value As Integer)
            m_lProductId = Value
        End Set
    End Property

    Public Property IsRenewalAmend() As Boolean
        Get
            IsRenewalAmend = m_bIsRenewalAmend
        End Get
        Set(ByVal Value As Boolean)
            m_bIsRenewalAmend = Value
        End Set
    End Property

    Private Sub cmdAccept_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAccept.Click
        'Unused Constant
        'Const kMethodName As String = "cmdAccept_Click"

        Try

            Dim lstCurrItem As System.Windows.Forms.ListViewItem
            Dim lCurrItemIndex As Integer

            ' Check if there are any items available.
            If (lvwPolicies.Items.Count = 0) Then
                Exit Sub
            End If

            ' loop thru the selected items
            For Each lstCurrItem In lvwPolicies.Items
                If lstCurrItem.Selected Then

                    ' process that amendment
                    m_lReturn = ProcessAccept(v_lSelectedTag:=lstCurrItem.Tag)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    ' store the current item's index - set it back when the listview has refreshed
                    lCurrItemIndex = lstCurrItem.Index

                End If

            Next lstCurrItem

            ' refresh the listview
            ShowClonedRIPolicies()

            ' select the item which was last selected


        Catch excep As Exception


            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to accept the renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAccept", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try


    End Sub

    Private Sub cmdAmend_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAmend.Click
        Dim lCurrItemIndex As Integer
        Try

            ' Check if there are any items available.
            If (lvwPolicies.Items.Count = 0) Then
                Exit Sub
            End If

            ' loop thru the selected items
            For Each lstCurrItem As ListViewItem In lvwPolicies.Items
                If lstCurrItem.Selected Then

                    ' process that amendment
                    m_lReturn = ProcessAmendment(v_lSelectedTag:=lstCurrItem.Tag)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    ' store the current item's index - set it back when the listview has refreshed
                    lCurrItemIndex = lstCurrItem.Index

                End If

            Next lstCurrItem

            ' refresh the listview
            ShowClonedRIPolicies()

            ' select the item which was last selected
            If lvwPolicies.Items.Count > 0 Then
                ' unselect the first item
                lvwPolicies.Items.Item(0).Selected = False
                ' select the one we want
                ' make sure we can see it

                'Changes as per Vb code
                'lvwDeferred.FocusedItem.EnsureVisible()
                lvwPolicies.Items.Item(0).EnsureVisible()
            End If


        Catch excep As Exception


            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error occured whilst amending the policy", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAmend", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            m_lReturn = g_oObjectManager.GetInstance(oObject:=m_oBusiness, sClassName:="bSIRCloneRIBatchProcess.Business", vInstanceManager:=PMGetViaClientManager)

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


        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmClonedRIManual_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Const kMethodName As String = "Form_Load"

        Try


            ShowFormInTaskBar_Detach()

            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "An error has occured setting interface defaults")
            End If

            m_lReturn = ShowClonedRIPolicies()

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim oListItem As System.Windows.Forms.ListViewItem
        Dim lRow As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Update the interface details.

            ' Clear the search details.
            lvwPolicies.Items.Clear()

            m_lItemsFound = 0

            ' Check that search details are valid before
            ' continuing.
            If (IsArray(m_vClonedRIPoliciesData) = False) Then
                Return result
            End If

            ' Assign the details to the interface.
            For lRow = LBound(m_vClonedRIPoliciesData, 2) To UBound(m_vClonedRIPoliciesData, 2)
                ' Assign the details to the first column.
                m_lItemsFound = m_lItemsFound + 1

                'col 1 branch
                oListItem = lvwPolicies.Items.Add(Trim(m_vClonedRIPoliciesData(MainModule.enumClonedRIData.edBranchDesc, lRow)))

                oListItem.SubItems.Add(Trim(m_vClonedRIPoliciesData(MainModule.enumClonedRIData.edDefRIStatusDesc, lRow)))
                oListItem.SubItems.Add(Trim(m_vClonedRIPoliciesData(MainModule.enumClonedRIData.edInsuranceRef, lRow)))
                oListItem.SubItems.Add(Trim(m_vClonedRIPoliciesData(MainModule.enumClonedRIData.edPartyShortName, lRow)))
                oListItem.SubItems.Add(Trim(m_vClonedRIPoliciesData(MainModule.enumClonedRIData.edPartyName, lRow)))
                oListItem.SubItems.Add(Trim(m_vClonedRIPoliciesData(MainModule.enumClonedRIData.edProductDesc, lRow)))

                oListItem.Tag = CStr(lRow)

            Next lRow

            'Refresh the initial results.
            lvwPolicies.Refresh()


            ' Check for errors
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result
        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try

    End Function
    ' ***************************************************************** '
    ' Name: ShowClonedRIPolicies
    '
    ' Description: shows/refreshes the RI policies
    ' ***************************************************************** '
    Private Function ShowClonedRIPolicies() As Integer
        Const kMethodName As String = "ShowClonedRIPolicies"

        Try
            ShowClonedRIPolicies = gPMConstants.PMEReturnCode.PMTrue
            '' TODO 'Display a searching message..
            DisplayStatusSearching()

            m_lReturn = GetBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                RaiseError("ShowClonedRIPolicies", "An error has occured whilst obtaining the reinsurance policy details")
            End If

            m_lReturn = DataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                RaiseError("ShowClonedRIPolicies", "An error has occured whilst displaying the reinsurance policy details")
            End If

            EnableDisableButtons()

            '' TODO 'Display a searching message for number of Policies found
            DisplayStatusFound()

            'Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check for errors
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get details.
                ShowClonedRIPolicies = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As System.Exception

            ShowClonedRIPolicies = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ShowClonedRIPolicies, excep:=ex)

        End Try

    End Function

    ''' <summary>
    ''' ProcessAmendment
    ''' </summary>
    ''' <param name="v_lSelectedTag"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessAmendment(ByVal v_lSelectedTag As Integer) As Integer

        Dim oResult As MsgBoxResult
        Dim oKeyArray(,) As Object
        Dim sFailureMessage As String
        Dim nInsuranceFileCnt As Integer
        Dim bIsRisksQuoted As Boolean
        Dim nIsValid As Integer
        Dim nIsInValid As Integer
        Dim nLoop As Integer
        Dim oGetKeyArray(,) As Object
        Dim nResult As Integer

        Try
            nResult = PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseBusy)

            ' hmm, are you sure?
            oResult = MsgBox("Are you sure you want to process this amendment?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Amend")

            If oResult = MsgBoxResult.Yes Then

                nInsuranceFileCnt = m_vClonedRIPoliciesData(enumClonedRIData.edInsFileCnt, v_lSelectedTag)

                nResult = m_oBusiness.IsValidInsuranceFileToAmend(nInsuranceFileCnt:=nInsuranceFileCnt, r_nIsInValid:=nIsInValid)

                If nResult <> PMEReturnCode.PMTrue Then
                    sFailureMessage = "Failed to ValidInsuranceFileToAmend " & nInsuranceFileCnt
                    Throw New Exception(sFailureMessage)
                End If

                If nIsInValid Then
                    MessageBox.Show("Prior policy version pending. Please complete the same first. ", "Amend", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return PMEReturnCode.PMFalse

                End If

                nResult = m_oBusiness.RecalculateRI(nInsuranceFileCnt, nIsValid)
                If nResult <> PMEReturnCode.PMTrue Then
                    sFailureMessage = "Failed to calculate RI " & nInsuranceFileCnt
                    Throw New Exception(sFailureMessage)
                End If

                'run policy
                ReDim oKeyArray(1, 4)
                oKeyArray(0, 0) = "party_cnt"
                oKeyArray(1, 0) = m_vClonedRIPoliciesData(enumClonedRIData.edInsuredCnt, v_lSelectedTag)
                oKeyArray(0, 1) = "insurance_file_cnt"
                oKeyArray(1, 1) = nInsuranceFileCnt
                oKeyArray(0, 2) = "insurance_folder_cnt"
                oKeyArray(1, 2) = m_vClonedRIPoliciesData(enumClonedRIData.edInsFolderCnt, v_lSelectedTag)
                oKeyArray(0, 3) = "shortname"
                oKeyArray(1, 3) = m_vClonedRIPoliciesData(enumClonedRIData.edPartyShortName, v_lSelectedTag)
                oKeyArray(0, 4) = "Product_id"
                oKeyArray(1, 4) = m_vClonedRIPoliciesData(enumClonedRIData.edProductID, v_lSelectedTag)

                nResult = RunProcess(v_sComponent:="iPMUPolicy.NavigatorV3", v_vKeyArray:=oKeyArray, v_lProcessMode:=PMEComponentAction.PMView,
                                              v_sTransactionType:="DRI", r_sFailureMessage:=sFailureMessage)

                If nResult <> PMEReturnCode.PMTrue Then
                    If nResult = PMEReturnCode.PMCancel Then
                        Return nResult
                    Else
                        Throw New Exception(sFailureMessage)
                    End If
                End If

                'run list risks
                ReDim oKeyArray(1, 1)
                oKeyArray(0, 0) = "insurance_file_cnt"
                oKeyArray(1, 0) = nInsuranceFileCnt
                oKeyArray(0, 1) = "insurance_folder_cnt"
                oKeyArray(1, 1) = m_vClonedRIPoliciesData(MainModule.enumClonedRIData.edInsFolderCnt, v_lSelectedTag)
                ReDim oGetKeyArray(1, 1)
                oGetKeyArray(0, 0) = "Is_Ready_To_Accept"
                oGetKeyArray(1, 0) = m_bIsReadyToAccept
                oGetKeyArray(0, 1) = "Is_All_Risks_Quoted"
                oGetKeyArray(1, 1) = m_bIsAllRiskQuoted


                nResult = RunProcess(v_sComponent:="iPMUListRisks.NavigatorV3", r_sFailureMessage:=sFailureMessage, v_vKeyArray:=oKeyArray, v_lProcessMode:=PMEComponentAction.PMView, v_sTransactionType:="DRI", r_vGetKeyArray:=oGetKeyArray)

                If nResult <> PMEReturnCode.PMTrue Then
                    If nResult = PMEReturnCode.PMCancel Then
                        Return nResult
                    Else
                        Throw New Exception(sFailureMessage)
                    End If
                End If

                For nLoop = 0 To oGetKeyArray.getUpperBound(1)

                    Select Case oGetKeyArray(0, nLoop)

                        Case "Is_Ready_To_Accept"
                            m_bIsReadyToAccept = CBool(oGetKeyArray(1, nLoop))

                        Case "Is_All_Risks_Quoted"
                            m_bIsAllRiskQuoted = CBool(oGetKeyArray(1, nLoop))
                    End Select

                Next nLoop

                If m_bIsAllRiskQuoted Then
                    nResult = m_oBusiness.GetAllRiskStatus(nInsuranceFileCnt, bIsRisksQuoted)
                    If nResult <> PMEReturnCode.PMTrue Then
                        sFailureMessage = "Failed to Get All Risk Status "
                        Throw New Exception(sFailureMessage)
                    End If

                    If bIsRisksQuoted Then
                        If m_oBusiness.UpdateInsFileClonedRIUsage(v_lInsFileClonedRIUsageID:=m_vClonedRIPoliciesData(MainModule.enumClonedRIData.edInsFileClonedRIUsageID, v_lSelectedTag), v_lInsFileCnt:=nInsuranceFileCnt, v_lClonedRIStatusID:=ksPTRIStatusUpdate) <> PMEReturnCode.PMTrue Then
                            sFailureMessage = "Failed to update Insurance_File_Cloned_RI_Usage status to awaiting update"
                            Throw New Exception(sFailureMessage)
                        End If
                    Else
                        If m_oBusiness.UpdateInsFileClonedRIUsage(v_lInsFileClonedRIUsageID:=m_vClonedRIPoliciesData(MainModule.enumClonedRIData.edInsFileClonedRIUsageID, v_lSelectedTag), v_lInsFileCnt:=nInsuranceFileCnt, v_lClonedRIStatusID:=ksPTRIStatusAmend) <> PMEReturnCode.PMTrue Then
                            sFailureMessage = "Failed to update Insurance_File_Cloned_RI_Usage status to awaiting update"
                            Throw New Exception(sFailureMessage)
                        End If
                    End If

                    If m_bIsReadyToAccept Then
                        nResult = ProcessAccept(v_lSelectedTag)
                        If nResult <> PMEReturnCode.PMTrue Then
                            sFailureMessage = "Failed to accept "
                            Throw New Exception(sFailureMessage)
                        End If
                    End If
                End If
            End If

            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)

            Return nResult

        Catch ex As Exception

            nResult = PMEReturnCode.PMError

            If sFailureMessage = "" Then
                sFailureMessage = "Failed to Process the renewal Amendment"
            End If

            LogMessagePopup(iType:=PMELogLevel.PMLogOnError, sMsg:=sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAmendment", excep:=ex)

            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)

            Return nResult

        End Try

    End Function

    ' ***************************************************************** '
    ' Name:GetBusiness
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function GetBusiness() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetClonedRIPolicy(r_vResultArray:=m_vClonedRIPoliciesData)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetBusiness = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBusiness failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Const kMethodName As String = "SetInterfaceDefaults"

        Try

            SetInterfaceDefaults = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            SetInterfaceDefaults = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=SetInterfaceDefaults, excep:=ex)
            Return SetInterfaceDefaults
        End Try

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
                sMessage = iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString)
            End If
            If sMessage = "" Then
                sMessage = " Items found"
            End If


            ' Display the status message.
            _stbStatus_Panel1.Text = " " & sMessage

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusFound()

        Static sMessage As String

        Try

            ' Get message text if not already present.
            If (sMessage = "") Then
                sMessage = iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString)
            End If
            If sMessage = "" Then
                sMessage = " Items found"
            End If
            ' Display the status message.
            _stbStatus_Panel1.Text = " " & m_lItemsFound & " " & sMessage

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Private isInitializingComponent As Boolean
    Private Sub frmClonedRIManual_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize

        If isInitializingComponent Then
            Exit Sub
        End If
        ' ---------------------------------------------------------------------------
        ' AUTHOR: AMB
        ' DATE: 03 Sept 2003
        ' HISTORY: Created - ICB deferred reinsurance development
        ' ---------------------------------------------------------------------------


        ' store values locally for speed
        Dim lFrameLeft As Integer = CInt(VB6.PixelsToTwipsX(fmeDeferredRI.Left))
        Dim lFrameTop As Integer = CInt(VB6.PixelsToTwipsY(fmeDeferredRI.Top))

        ' resize frame
        fmeDeferredRI.Width = VB6.TwipsToPixelsX(Math.Abs(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - (lFrameLeft * 2)))
        fmeDeferredRI.Height = VB6.TwipsToPixelsY(Math.Abs(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - (lFrameTop + 900)))

        Dim lFrameWidth As Integer = CInt(VB6.PixelsToTwipsX(fmeDeferredRI.Width))
        Dim lFrameHeight As Integer = CInt(VB6.PixelsToTwipsY(fmeDeferredRI.Height))

        ' resize listview
        lvwPolicies.Width = VB6.TwipsToPixelsX(Math.Abs(lFrameWidth - (VB6.PixelsToTwipsX(lvwPolicies.Left) * 2)))
        lvwPolicies.Height = VB6.TwipsToPixelsY(Math.Abs(lFrameHeight - (VB6.PixelsToTwipsX(lvwPolicies.Left) * 3)))

        ' move buttons
        cmdAmend.Top = VB6.TwipsToPixelsY(lFrameTop + lFrameHeight + 120)
        cmdAccept.Top = cmdAmend.Top
        cmdSelectAll.Top = cmdAmend.Top
        cmdOK.SetBounds(VB6.TwipsToPixelsX(Math.Abs((lFrameLeft + lFrameWidth) - VB6.PixelsToTwipsX(cmdOK.Width))), cmdAmend.Top, 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

    End Sub

    Private Sub frmClonedRIManual_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        ' Terminate the business object
        m_oBusiness.Dispose()

        ' Destroy the instance of the renewals business object
        ' from memory.
        m_oBusiness = Nothing

    End Sub

    ''' <summary>
    ''' Process Accept
    ''' </summary>
    ''' <param name="v_lSelectedTag"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessAccept(ByVal v_lSelectedTag As Integer) As Integer

        Dim oResult As DialogResult
        Dim sFailureMessage As String
        Dim nResult As Integer

        Try

            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseBusy)

            If m_bIsReadyToAccept = False Then
                oResult = MessageBox.Show("Are you sure you want to process this acceptance?", "Accept", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            End If
            If oResult = MsgBoxResult.Yes OrElse m_bIsReadyToAccept = True Then

                nResult = m_oBusiness.CreateAndPostStats(nInsuranceFileCnt:=m_vClonedRIPoliciesData(enumClonedRIData.edInsFileCnt, v_lSelectedTag), nClonedInsuranceFileCnt:=m_vClonedRIPoliciesData(MainModule.enumClonedRIData.edInsFileCnt, v_lSelectedTag), bReverseCloned:=True, sTransactionType:="DRI", r_sMessage:=sFailureMessage)

                If nResult <> PMEReturnCode.PMTrue Then
                    Call LogMessagePopup(iType:=PMELogLevel.PMLogOnError, sMsg:=sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccept")
                    Return nResult
                End If

                nResult = m_oBusiness.UpdateRIArrangementClonedStatus(v_lRisk_cnt:=0, v_iCloned:=2, v_lInsuranceFileCnt:=ToSafeLong(m_vClonedRIPoliciesData(enumClonedRIData.edInsFileCnt, v_lSelectedTag)))

                ' Check for errors.
                If (nResult <> PMEReturnCode.PMTrue) Then
                    Return nResult
                End If

                ' if all is well, remove the accociated record from Insurace_File_Cloned_RI_Usage
                nResult = m_oBusiness.DeleteInsFileClonedRIUsage(nInsuranceFileCnt:=m_vClonedRIPoliciesData(enumClonedRIData.edInsFileCnt, v_lSelectedTag))

                ' Check for errors.
                If (nResult <> PMEReturnCode.PMTrue) Then
                    Return nResult
                End If

            End If

            Return nResult

        Catch excep As Exception

            nResult = PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="ProcessAccept Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccept", vErrNo:=Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        Finally

            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: EnableDisableButtons
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Sub EnableDisableButtons()
        Dim lstCurrItem As System.Windows.Forms.ListViewItem
        Dim lAmendCount As Integer
        Dim lUpdateCount As Integer

        Try

            If (lvwPolicies.Items.Count = 0) Then
                cmdAccept.Enabled = False
                cmdAmend.Enabled = False
                cmdSelectAll.Enabled = False
                Exit Sub
            End If

            For Each lstCurrItem In lvwPolicies.Items
                If lstCurrItem.Selected Then
                    ' check for amend status
                    If m_vClonedRIPoliciesData(MainModule.enumClonedRIData.edDefRIStatusID, lstCurrItem.Tag) = ksPTRIStatusAmend Then
                        lAmendCount = lAmendCount + 1
                        ' check for update status
                    ElseIf m_vClonedRIPoliciesData(MainModule.enumClonedRIData.edDefRIStatusID, lstCurrItem.Tag) = ksPTRIStatusUpdate Then
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

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableDisableButtons Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableDisableButtons", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: PopupMouseMenu
    '
    ' Description:
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
                    If CDbl(m_vClonedRIPoliciesData(MainModule.enumClonedRIData.edDefRIStatusID, Convert.ToString(lstCurrItem.Tag))) = ksPTRIStatusAmend Then
                        lAmendCount += 1
                        ' check for update status
                    ElseIf CDbl(m_vClonedRIPoliciesData(MainModule.enumClonedRIData.edDefRIStatusID, Convert.ToString(lstCurrItem.Tag))) = ksPTRIStatusUpdate Then
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

    Private Sub lvwPolicies_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwPolicies.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = Vb6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        If Button = MouseButtonConstants.RightButton Then
            PopupMouseMenu()
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

    '*******************************************************************************************************
    'Desc: run relevant component with provided keys and get back required keys from component if required
    '*******************************************************************************************************
    Private Function RunProcess(ByVal v_sComponent As String, ByVal v_vKeyArray(,) As Object, ByRef r_sFailureMessage As String, Optional ByRef r_vGetKeyArray(,) As Object = Nothing, Optional ByVal v_bDisplayMessage As Boolean = True, Optional ByVal v_lProcessMode As Integer = gPMConstants.PMEComponentAction.PMEdit, Optional ByVal v_sTransactionType As String = "DRI") As Integer

        Const kMethodName As String = "RunProcess"
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
                Exit Function
            End If

            If bNavigatorV3 Then
                'pass in relevant keys
                If oObject.NavigatorV3_SetKeys(v_vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                    RunProcess = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to set relevant Keys to " & v_sComponent
                    If v_bDisplayMessage Then
                        MsgBox(r_sFailureMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, ACApp)
                    End If
                    Exit Function
                End If

                'set process mode
                If oObject.NavigatorV3_SetProcessModes(vTask:=v_lProcessMode, vTransactionType:=v_sTransactionType) <> gPMConstants.PMEReturnCode.PMTrue Then
                    RunProcess = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to set process mode to " & v_sComponent
                    If v_bDisplayMessage Then
                        MsgBox(r_sFailureMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, ACApp)
                    End If
                    Exit Function
                End If

                'start component
                If oObject.NavigatorV3_Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                    RunProcess = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to start" & v_sComponent
                    If v_bDisplayMessage Then
                        MsgBox(r_sFailureMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, ACApp)
                    End If
                    Exit Function
                End If

                'get keys back if required
                If Not IsNothing(r_vGetKeyArray) Then
                    If oObject.NavigatorV3_GetKeys(vKeyArray:=r_vGetKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                        RunProcess = gPMConstants.PMEReturnCode.PMFalse
                        r_sFailureMessage = "Failed to get keys from " & v_sComponent
                        If v_bDisplayMessage Then
                            MsgBox(r_sFailureMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, ACApp)
                        End If
                        Exit Function
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
                    Exit Function
                End If

                'set process mode
                If oObject.SetProcessModes(vTask:=v_lProcessMode) <> gPMConstants.PMEReturnCode.PMTrue Then
                    RunProcess = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to set process mode to " & v_sComponent
                    If v_bDisplayMessage Then
                        MsgBox(r_sFailureMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, ACApp)
                    End If
                    Exit Function
                End If

                'start component
                If oObject.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                    RunProcess = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to start" & v_sComponent
                    If v_bDisplayMessage Then
                        MsgBox(r_sFailureMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, ACApp)
                    End If
                    Exit Function
                End If

                'get keys back if required
                If Not IsNothing(r_vGetKeyArray) Then
                    If oObject.GetKeys(vKeyArray:=r_vGetKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                        RunProcess = gPMConstants.PMEReturnCode.PMFalse
                        r_sFailureMessage = "Failed to get keys from " & v_sComponent
                        If v_bDisplayMessage Then
                            MsgBox(r_sFailureMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, ACApp)
                        End If
                        Exit Function
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
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=RunProcess, excep:=ex)

        Finally
            If Not (oObject Is Nothing) Then
                oObject.Dispose()
                oObject = Nothing
            End If


        End Try
    End Function
End Class
