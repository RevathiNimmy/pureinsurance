Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Public Class frmDeferredRIManual
    Inherits System.Windows.Forms.Form

    ' ***************************************************************** '
    ' Form Name: frmDeferredRIManual
    '
    ' Date: 26/09/00
    '
    ' Description: Interface for Renewals processing.
    '
    ' Edit History: CT 26/09/2000 - Created
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmDeferredRIManual"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    'Private m_sTransactionType As String
    Private m_vDefRIPoliciesData(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    Private m_lItemsFound As Integer
    ' IsRenewalAmend
    Private m_bIsRenewalAmend As Boolean
    Private m_lProductId As Integer
    Private m_sRenewalDate As String = ""

    Private m_lBusinessTypeId As Integer


    Public Property RenewalDate() As String
        Get
            Return m_sRenewalDate
        End Get
        Set(ByVal Value As String)
            m_sRenewalDate = Value
        End Set
    End Property


    Public Property ProductId() As Integer
        Get
            Return m_lProductId
        End Get
        Set(ByVal Value As Integer)
            m_lProductId = Value
        End Set
    End Property


    Public Property IsRenewalAmend() As Boolean
        Get
            Return m_bIsRenewalAmend
        End Get
        Set(ByVal Value As Boolean)
            m_bIsRenewalAmend = Value
        End Set
    End Property



    Private Sub cmdAccept_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAccept.Click
        ' AMB 09-Sep-03: 1.8.6 Deferred Reinsurance development
        Dim lCurrItemIndex As Integer

        Try

            ' Check if there are any items available.
            If lvwDeferred.Items.Count = 0 Then
                Exit Sub
            End If

            ' loop thru the selected items
            For Each lstCurrItem As ListViewItem In lvwDeferred.Items
                If lstCurrItem.Selected Then

                    ' process that acceptance...
                    m_lReturn = ProcessAccept(v_lSelectedTag:=Convert.ToString(lstCurrItem.Tag))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to accept the deferred reinsurance policy", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAccept", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Exit Sub
                    End If

                    ' store the current item's index - set it back when the listview has refreshed
                    lCurrItemIndex = lstCurrItem.Index + 1

                End If

            Next lstCurrItem

            ' refresh the listview
            ShowDeferredRIPolicies()

            ' select the item as close to the last selected one
            If lvwDeferred.Items.Count Then
                ' unselect the first item
                lvwDeferred.Items.Item(0).Selected = False
                ' select the one we want
                lvwDeferred.Items.Item(CInt(Math.Min(lvwDeferred.Items.Count, lCurrItemIndex)) - 1).Selected = True
                ' make sure we can see it

                'Changes as per Vb code
                'lvwDeferred.FocusedItem.EnsureVisible()
                lvwDeferred.SelectedItems(0).EnsureVisible()
            End If

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to accept the renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAccept", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdAmend_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAmend.Click
        ' AMB 09-Sep-03: 1.8.6 Deferred Reinsurance development

        Dim lCurrItemIndex As Integer

        Try

            ' Check if there are any items available.
            If lvwDeferred.Items.Count = 0 Then
                Exit Sub
            End If

            ' loop thru the selected items
            For Each lstCurrItem As ListViewItem In lvwDeferred.Items
                If lstCurrItem.Selected Then

                    ' process that amendment - yeee-haar!
                    m_lReturn = ProcessAmendment(v_lSelectedTag:=Convert.ToString(lstCurrItem.Tag))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    ' store the current item's index - set it back when the listview has refreshed
                    lCurrItemIndex = lstCurrItem.Index + 1

                End If

            Next lstCurrItem

            ' refresh the listview
            ShowDeferredRIPolicies()

            ' select the item which was last selected
            If lvwDeferred.Items.Count Then
                ' unselect the first item
                lvwDeferred.Items.Item(0).Selected = False
                ' select the one we want
                lvwDeferred.Items.Item(lCurrItemIndex - 1).Selected = True
                ' make sure we can see it

                'Changes done as per Vb code
                'lvwDeferred.FocusedItem.EnsureVisible()
                lvwDeferred.SelectedItems(0).EnsureVisible()
            End If

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error occured whilst amending the policy", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAmend", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Me.Close()

    End Sub


    Private Sub cmdSelectAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelectAll.Click

        For Each lstCurrItem As ListViewItem In lvwDeferred.Items
            lstCurrItem.Selected = True
        Next lstCurrItem

        EnableDisableButtons()

        lvwDeferred.Focus()

    End Sub


    Private Sub Form_Initialize_Renamed()
        ' Forms initialise event.
        Dim sMessage, sTitle As String

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

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
            Dim temp_g_oDeferredRI As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oDeferredRI, "bSIRDeferredRIAuto.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oDeferredRI = temp_g_oDeferredRI

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                'Developer Guie No 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guie No 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            Dim temp_g_oAutoDeferredRI As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oAutoDeferredRI, "bSIRDeferredRIAuto.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oAutoDeferredRI = temp_g_oAutoDeferredRI

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                'Developer Guie No 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guie No 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

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


    Private Sub frmDeferredRIManual_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            iPMFunc.ShowFormInTaskBar_Detach()

            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occured setting interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="form_load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            'RWH(03/11/2000) Moved into interface class.
            '    'RWH(04/10/2000)
            '    If (m_bIsRenewalAmend = False) Then
            '        SetMousePointer PMMouseNormal
            '        frmFilterRenewal.Show vbModal
            '        SetMousePointer PMMouseBusy
            '
            '        m_lProductId = frmFilterRenewal.ProductId
            '        m_sRenewalDate = frmFilterRenewal.RenewalDate
            '
            '    End If

            'If lvwDeferred.Items.Count > 0 Then
            '    lvwDeferred.Items(0).Selected = True
            'End If


            m_lReturn = ShowDeferredRIPolicies()


        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwDeferred.Items.Clear()

            m_lItemsFound = 0

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vDefRIPoliciesData) Then
                Return result
            End If

            ' Assign the details to the interface.
            For lRow As Integer = m_vDefRIPoliciesData.GetLowerBound(1) To m_vDefRIPoliciesData.GetUpperBound(1)
                ' Assign the details to the first column.
                m_lItemsFound += 1

                'col 1 branch
                oListItem = lvwDeferred.Items.Add(CStr(m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edBranchDesc, lRow)).Trim())

                'col 2 deferred ri status desc
                ListViewHelper.GetListViewSubItem(oListItem, MainModule.enumDeferredRIListView.elDefRIStatusDesc).Text = CStr(m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edDefRIStatusDesc, lRow)).Trim()

                'col 3 policy no
                ListViewHelper.GetListViewSubItem(oListItem, MainModule.enumDeferredRIListView.elInsuranceRef).Text = CStr(m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edInsuranceRef, lRow)).Trim()

                'col 4 party shortname
                ListViewHelper.GetListViewSubItem(oListItem, MainModule.enumDeferredRIListView.elPartyShortName).Text = CStr(m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edPartyShortName, lRow)).Trim()

                'col 5 party name
                ListViewHelper.GetListViewSubItem(oListItem, MainModule.enumDeferredRIListView.elPartyName).Text = CStr(m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edPartyName, lRow)).Trim()

                'col 6 product desc
                ListViewHelper.GetListViewSubItem(oListItem, MainModule.enumDeferredRIListView.elProductDesc).Text = CStr(m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edProductDesc, lRow)).Trim()

                oListItem.Tag = CStr(lRow)

            Next lRow

            'Refresh the initial results.
            lvwDeferred.Refresh()


            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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
    ' Name: ShowDeferredRIPolicies
    '
    ' Description: shows/refreshes the deferred RI policies
    ' ***************************************************************** '
    Private Function ShowDeferredRIPolicies() As Integer

        Dim result As Integer = 0
        Try

            'Display a searching message.
            DisplayStatusSearching()

            m_lReturn = GetBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occured whilst obtaining the deferred reinsurance policy details", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDeferredRIPolicies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            m_lReturn = DataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occured whilst displaying the deferred reinsurance policy details", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDeferredRIPolicies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            If lvwDeferred.Items.Count > 0 Then
                lvwDeferred.Items(0).Selected = True
            End If

            EnableDisableButtons()

            'Display a searching message.
            DisplayStatusFound()

            'Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDeferredRIPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessAmendment
    '
    ' Description: Displays renewal for amendment and then goes though
    '              subsequent amendment processing.
    '
    ' AMB 09-Sep-03: 1.8.6 Deferred Reinsurance development - amended from frmRenewal version
    ' ***************************************************************** '
    Public Function ProcessAmendment(ByVal v_lSelectedTag As Integer) As Integer

        Dim result As Integer = 0
        Dim lPolicyCnt, lStatus As Integer
        Dim vbResult As DialogResult
        Dim vKeyArray(,) As Object
        Dim sFailureMessage As String = ""
        Dim dtPolicyStartDate As Date
        Dim lNewInsuranceFileCnt, lInsuranceFileCnt As Integer, nInsuranceFolderCnt As Integer
        'Dim nInsStatus As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' hmm, are you sure?

            vbResult = MessageBox.Show("Are you sure you want to process this amendment?", "Amend", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If vbResult = System.Windows.Forms.DialogResult.Yes Then

                lInsuranceFileCnt = CInt(m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edInsFileCnt, v_lSelectedTag))
                nInsuranceFolderCnt = CInt(m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edInsFolderCnt, v_lSelectedTag))
                'lInsuranceFileCnt = m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edInsFileCnt, v_lSelectedTag)
                'create deferred policy version

                If g_oDeferredRI.CopyPolicyHeader(nInsuranceFileCnt:=lInsuranceFileCnt, nNewInsurancefileCnt:=lNewInsuranceFileCnt, r_sMessage:=sFailureMessage, sSetOldInsuranceFileStatus:=gSIRLibrary.SIRInsFileStatusReplacedDRI, sTransactionType:="DRI", sInsStatus:=MainModule.enumDeferredRIData.edDefRIStatusDesc) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New Exception

                End If

                'relink risks to new policy version

                If g_oDeferredRI.CopyDefRIRisks(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_lNewInsuranceFileCnt:=lNewInsuranceFileCnt, v_lInsuranceFolderCnt:=nInsuranceFolderCnt, v_dtPolicyStartDate:=dtPolicyStartDate, v_sTransactionType:="DRI", r_sMessage:=sFailureMessage, v_bIgnoreError:=True) <> gPMConstants.PMEReturnCode.PMTrue Then
                    sFailureMessage = "Failed to relink risks to new policy version"
                    Throw New Exception(sFailureMessage)
                End If

                'If g_oDeferredRI.RelinkRisk(v_lOldInsuranceFileCnt:=lInsuranceFileCnt, v_lNewInsuranceFileCnt:=lNewInsuranceFileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                '    sFailureMessage = "Failed to relink risks to new policy version"
                '    Throw New Exception(sFailureMessage)
                'End If

                'run policy
                ReDim vKeyArray(1, 4)

                vKeyArray(0, 0) = "party_cnt"

                vKeyArray(1, 0) = m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edInsuredCnt, v_lSelectedTag)

                vKeyArray(0, 1) = "insurance_file_cnt"

                vKeyArray(1, 1) = lNewInsuranceFileCnt

                vKeyArray(0, 2) = "insurance_folder_cnt"

                vKeyArray(1, 2) = m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edInsFolderCnt, v_lSelectedTag)

                vKeyArray(0, 3) = "shortname"

                vKeyArray(1, 3) = m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edPartyShortName, v_lSelectedTag)

                vKeyArray(0, 4) = "Product_id"

                vKeyArray(1, 4) = m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edProductID, v_lSelectedTag)

                result = RunProcess(v_sComponent:="iPMUPolicy.NavigatorV3", v_vKeyArray:=vKeyArray, r_sFailureMessage:=sFailureMessage)

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    If result = gPMConstants.PMEReturnCode.PMCancel Then
                        Return result
                    Else
                        Throw New Exception
                    End If
                End If

                'run list risks
                ReDim vKeyArray(1, 2)

                vKeyArray(0, 0) = "insurance_file_cnt"

                vKeyArray(1, 0) = lNewInsuranceFileCnt

                vKeyArray(0, 1) = "insurance_folder_cnt"

                vKeyArray(1, 1) = m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edInsFolderCnt, v_lSelectedTag)

                vKeyArray(0, 2) = "Party_Short_Name" 'PMKeyNameShortName
                vKeyArray(1, 2) = m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edPartyShortName, v_lSelectedTag)


                result = RunProcess(v_sComponent:="iPMUListRisks.NavigatorV3", r_sFailureMessage:=sFailureMessage, v_vKeyArray:=vKeyArray, v_sTransactionType:="DRI")

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    If result = gPMConstants.PMEReturnCode.PMCancel Then
                        Return result
                    Else
                        Throw New Exception
                    End If
                End If

                ' update Insurance_File_Deferred_RI_Usage with new insurance_file_cnt and change status to awaiting update

                If g_oDeferredRI.SetDeferredRIStatus(v_lInsFileDeferredRIUsageID:=m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edInsFileDeferredRIUsageID, v_lSelectedTag), v_lInsFileCnt:=lNewInsuranceFileCnt, v_lDefRIStatusID:=ksDeferredRIStatusUpdate) <> gPMConstants.PMEReturnCode.PMTrue Then
                    sFailureMessage = "Failed to update Insurance_File_Deferred_RI_Usage status to awaiting update"
                    Throw New Exception(sFailureMessage)
                End If

            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            If sFailureMessage = "" Then
                sFailureMessage = "Failed to Process the renewal Amendment"
            End If

            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAmendment", excep:=ex)

        Finally
            'delete new policy version if we failed any of the steps above
            If result <> gPMConstants.PMEReturnCode.PMTrue Then

                If g_oDeferredRI.DeletePolicy(v_lInsuranceFileCnt:=lNewInsuranceFileCnt) = gPMConstants.PMEReturnCode.PMTrue Then
                    'reset old policy version back to NULL

                    If g_oDeferredRI.SetPolicyStatus(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_lInsuranceFileStatusID:=0) <> gPMConstants.PMEReturnCode.PMTrue Then
                        sFailureMessage = "Failed to set old policy status back to LIVE - " & lInsuranceFileCnt
                        gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAmendment")
                    End If
                Else
                    sFailureMessage = "Failed to delete new policy version - " & lNewInsuranceFileCnt
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAmendment")
                End If

            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
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


            m_lReturn = g_oDeferredRI.GetDeferredRIPolicy(r_vResultArray:=m_vDefRIPoliciesData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBusiness failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            'RWH(03/10/2000) -  Show appropriate command buttons.
            '''    If (m_bIsRenewalAmend = True) Then
            '''        cmdAmend.Visible = True
            '''        cmdDelete.Visible = True
            '''        cmdLapse.Visible = True
            '''        cmdAccept.Visible = False
            '''        cmdSelectAll.Visible = False
            '''
            '''        'Thinh Nguyen 20/03/2002 (start)
            '''        cmdFilter.Visible = True
            '''        cmdFilter.Top = cmdChangeStatus.Top
            '''        cmdFilter.Left = cmdChangeStatus.Width + cmdChangeStatus.Left + 200
            '''        'Thinh Nguyen 20/03/2002 (end)
            '''
            ''''        cmdChange.Visible = False
            '''    Else
            '''        cmdAmend.Visible = False
            '''        cmdDelete.Visible = False
            '''        cmdLapse.Visible = False
            '''        cmdAccept.Top = cmdAmend.Top
            '''        cmdAccept.Left = cmdAmend.Left
            '''        cmdAccept.Visible = True
            '''        cmdSelectAll.Top = cmdDelete.Top
            '''        cmdSelectAll.Left = cmdDelete.Left
            '''        cmdSelectAll.Visible = True
            '''        cmdFilter.Top = cmdLapse.Top
            '''        cmdFilter.Left = cmdLapse.Left
            '''        cmdFilter.Visible = True
            ''''        cmdChange.Visible = True
            ''''        cmdChange.Enabled = False
            '''    End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                'Developer Guie No 243
                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            'Developer Guide No 168
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

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                'develoepr Guie No 243
                'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            'stbStatus.Text = " " & m_lItemsFound & " " & sMessage
            _stbStatus_Panel1.Text = " " & m_lItemsFound & " " & sMessage

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmDeferredRIManual_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
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
        lvwDeferred.Width = VB6.TwipsToPixelsX(Math.Abs(lFrameWidth - (VB6.PixelsToTwipsX(lvwDeferred.Left) * 2)))
        lvwDeferred.Height = VB6.TwipsToPixelsY(Math.Abs(lFrameHeight - (VB6.PixelsToTwipsX(lvwDeferred.Left) * 3)))

        ' move buttons
        cmdAmend.Top = VB6.TwipsToPixelsY(lFrameTop + lFrameHeight + 120)
        cmdAccept.Top = cmdAmend.Top
        cmdSelectAll.Top = cmdAmend.Top
        cmdOK.SetBounds(VB6.TwipsToPixelsX(Math.Abs((lFrameLeft + lFrameWidth) - VB6.PixelsToTwipsX(cmdOK.Width))), cmdAmend.Top, 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

    End Sub

    Private Sub frmDeferredRIManual_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        ' Terminate the business object

        g_oDeferredRI.Dispose()
        ' Destroy the instance of the renewals business object
        ' from memory.
        g_oDeferredRI = Nothing


        g_oAutoDeferredRI.Dispose()
        ' Destroy the instance of the renewals business object
        ' from memory.
        g_oAutoDeferredRI = Nothing

    End Sub

    Private Sub lvwDeferred_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwDeferred.Click

        ' AMB 08-Sep-03: 1.8.6 Deferred Reinsurance development
        EnableDisableButtons()

    End Sub

    Private Sub lvwDeferred_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwDeferred.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwDeferred.Columns(eventArgs.Column)
        'RWH(06/02/2001) Store previous SortKey in static variable as when sorting date
        'an extra column is created as SortKey and then removed. SortKey reverts to 0. If then
        'set SortKey to correct column, list resorts incorrectly.
        Dim lDirection As SortOrder
        Static iPreviousSortKey As Integer

        ' Column click event for the search details

        Try

            With lvwDeferred

                If ColumnHeader.Index + 1 - 1 = ACDateColumn Then

                    '            If (.SortKey <> ACDateColumn) Then
                    If iPreviousSortKey <> ACDateColumn Then
                        ListViewHelper.SetSortKeyProperty(lvwDeferred, ACDateColumn)
                        lDirection = SortOrder.Ascending
                    Else
                        lDirection = (ListViewHelper.GetSortOrderProperty(lvwDeferred) + 1) Mod 2
                    End If

                    m_lReturn = ListViewSortByDate(v_oListView:=lvwDeferred, v_iSourceColumn:=ACDateColumn, v_iDirection:=lDirection)

                    iPreviousSortKey = ACDateColumn

                    ' If current sort column header is
                    ' pressed.
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwDeferred) And ListViewHelper.GetSortedProperty(lvwDeferred)) Then
                    '        ElseIf (ColumnHeader.Index - 1 = iPreviousSortKey) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwDeferred, (ListViewHelper.GetSortOrderProperty(lvwDeferred) + 1) Mod 2)

                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwDeferred, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwDeferred, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwDeferred, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwDeferred, True)
                    iPreviousSortKey = ListViewHelper.GetSortKeyProperty(lvwDeferred)

                End If
            End With

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDeferred_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: ProcessAccept
    '
    ' Description: Process the acceptancefor each selected policy
    '
    ' History: AMB 09-Sep-03: 1.8.6 Deferred Reinsurance development
    ' ***************************************************************** '
    Public Function ProcessAccept(ByVal v_lSelectedTag As Integer) As Integer

        Dim result As Integer = 0
        Dim vbResult As DialogResult
        Dim sFailureMessage As String = ""

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' hmm, are you sure?

            vbResult = MessageBox.Show("Are you sure you want to process this acceptance?", "Accept", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If vbResult = System.Windows.Forms.DialogResult.Yes Then


                m_lReturn = g_oAutoDeferredRI.CreateAndPostStats(v_lInsuranceFileCnt:=m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edInsFileCnt, v_lSelectedTag), v_sTransactionType:="DRI", r_sMessage:=sFailureMessage)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccept")

                    result = False
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Return result
                End If

                ' if all is well, remove the accociated record from Insurace_File_Deferred_RI_Usage

                m_lReturn = g_oDeferredRI.DeleteInsFileDefRIUsage(v_lInsFileDeferredRIUsageID:=m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edInsFileDeferredRIUsageID, v_lSelectedTag))

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = False
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Return result
                End If

            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccept Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccept", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EnableDisableButtons
    '
    ' Description:
    '
    ' History: 07/11/2002 sj - Created.
    ' AMB 08-Sep-03: 1.8.6 Deferred Reinsurance development - modified
    '
    ' ***************************************************************** '
    Private Sub EnableDisableButtons()

        Dim lAmendCount, lUpdateCount As Integer

        Try

            ' disable all
            If lvwDeferred.Items.Count = 0 Then
                cmdAccept.Enabled = False
                cmdAmend.Enabled = False
                cmdSelectAll.Enabled = False
                Exit Sub
            End If

            For Each lstCurrItem As ListViewItem In lvwDeferred.Items
                If lstCurrItem.Selected Then
                    ' check for amend status
                    If CDbl(m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edDefRIStatusID, Convert.ToString(lstCurrItem.Tag))) = ksDeferredRIStatusAmend Then
                        lAmendCount += 1
                        ' check for update status
                    ElseIf CDbl(m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edDefRIStatusID, Convert.ToString(lstCurrItem.Tag))) = ksDeferredRIStatusUpdate Then
                        lUpdateCount += 1
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
    ' History: AMB 08-Sep-03: 1.8.6 Deferred Reinsurance development
    '
    ' ***************************************************************** '
    Private Sub PopupMouseMenu()

        Dim lAmendCount, lUpdateCount As Integer
        Dim sBoldMenu As ToolStripMenuItem

        Try

            ' disable all
            If (lvwDeferred.Items.Count = 0) Or (lvwDeferred.FocusedItem Is Nothing) Then
                mnuPopup.Available = False
                Exit Sub
            End If

            For Each lstCurrItem As ListViewItem In lvwDeferred.Items
                If lstCurrItem.Selected Then
                    ' check for amend status
                    If CDbl(m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edDefRIStatusID, Convert.ToString(lstCurrItem.Tag))) = ksDeferredRIStatusAmend Then
                        lAmendCount += 1
                        ' check for update status
                    ElseIf CDbl(m_vDefRIPoliciesData(MainModule.enumDeferredRIData.edDefRIStatusID, Convert.ToString(lstCurrItem.Tag))) = ksDeferredRIStatusUpdate Then
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


    Private Sub lvwDeferred_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwDeferred.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        ' trigger the click event when user uses cursor keys
        Select Case KeyCode
            Case Keys.Up, Keys.Down, Keys.Home, Keys.End, Keys.PageUp, Keys.PageDown

                lvwDeferred_Click(lvwDeferred, New EventArgs())

        End Select

    End Sub

    Private Sub lvwDeferred_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwDeferred.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        If Button = MouseButtonConstants.RightButton Then
            PopupMouseMenu()
        End If

    End Sub

    Public Sub mnuPopupAccept_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopupAccept.Click

        cmdAccept_Click(cmdAccept, New EventArgs())

    End Sub


    Public Sub mnuPopupAmend_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopupAmend.Click

        cmdAmend_Click(cmdAmend, New EventArgs())

    End Sub


    Public Sub mnuPopupSelectAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPopupSelectAll.Click

        cmdSelectAll_Click(cmdSelectAll, New EventArgs())

    End Sub


    '*******************************************************************************************************
    'Desc: run relevant component with provided keys and get back required keys from component if required
    '*******************************************************************************************************
    Private Function RunProcess(ByVal v_sComponent As String, ByVal v_vKeyArray(,) As Object, ByRef r_sFailureMessage As String, Optional ByRef r_vGetKeyArray(,) As Object = Nothing, Optional ByVal v_bDisplayMessage As Boolean = True, Optional ByVal v_lProcessMode As Integer = gPMConstants.PMEComponentAction.PMEdit, Optional ByVal v_sTransactionType As String = "DRI") As Integer

        Dim result As Integer = 0
        Dim oObject As Object
        Dim bNavigatorV3 As Boolean

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'are we using NavigatorV3 or interface class?
            bNavigatorV3 = (v_sComponent.ToUpper().IndexOf(".NAVIGATORV3") >= 0)

            'create an instance of required object
            m_lReturn = g_oObjectManager.GetInstance(oObject:=oObject, sClassName:=v_sComponent, vInstanceManager:=gPMConstants.PMGetLocalInterface)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                r_sFailureMessage = "Failed to instantiate object " & v_sComponent
                If v_bDisplayMessage Then
                    MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                Return result
            End If

            If bNavigatorV3 Then
                'pass in relevant keys

                If oObject.NavigatorV3_SetKeys(v_vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to set relevant Keys to " & v_sComponent
                    If v_bDisplayMessage Then
                        MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return result
                End If

                'set process mode

                If oObject.NavigatorV3_SetProcessModes(vTask:=v_lProcessMode, vTransactionType:=v_sTransactionType) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to set process mode to " & v_sComponent
                    If v_bDisplayMessage Then
                        MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return result
                End If

                'start component

                If oObject.NavigatorV3_Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to start" & v_sComponent
                    If v_bDisplayMessage Then
                        MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return result
                End If

                'get keys back if required

                If Not Information.IsNothing(r_vGetKeyArray) Then

                    If oObject.NavigatorV3_GetKeys(vKeyArray:=r_vGetKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        r_sFailureMessage = "Failed to get keys from " & v_sComponent
                        If v_bDisplayMessage Then
                            MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        Return result
                    End If
                End If

                'did we cancel or error?

                If oObject.NavigatorV3_Status = gPMConstants.PMEReturnCode.PMError Then
                    r_sFailureMessage = "Error in " & v_sComponent
                    result = gPMConstants.PMEReturnCode.PMError
                ElseIf oObject.NavigatorV3_Status = gPMConstants.PMEReturnCode.PMCancel Then
                    r_sFailureMessage = "Process was cancelled"
                    result = gPMConstants.PMEReturnCode.PMCancel
                End If

            Else
                'we are using interface class
                'pass in relevant keys

                If oObject.SetKeys(v_vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to set relevant Keys to " & v_sComponent
                    If v_bDisplayMessage Then
                        MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return result
                End If

                'set process mode

                If oObject.SetProcessModes(vTask:=v_lProcessMode) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to set process mode to " & v_sComponent
                    If v_bDisplayMessage Then
                        MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return result
                End If

                'start component

                If oObject.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to start" & v_sComponent
                    If v_bDisplayMessage Then
                        MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return result
                End If

                'get keys back if required

                If Not Information.IsNothing(r_vGetKeyArray) Then

                    If oObject.GetKeys(vKeyArray:=r_vGetKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        r_sFailureMessage = "Failed to get keys from " & v_sComponent
                        If v_bDisplayMessage Then
                            MessageBox.Show(r_sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        Return result
                    End If
                End If

                'did we cancel or error?

                If oObject.Status = gPMConstants.PMEReturnCode.PMError Then
                    r_sFailureMessage = "Error in " & v_sComponent
                    result = gPMConstants.PMEReturnCode.PMError
                ElseIf oObject.Status = gPMConstants.PMEReturnCode.PMCancel Then
                    r_sFailureMessage = "Process was cancelled"
                    result = gPMConstants.PMEReturnCode.PMCancel
                End If

            End If 'NavigatorV3



        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            r_sFailureMessage = "Failed to run component " & v_sComponent

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="RunProcess()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            Return result

        Finally

            If Not (oObject Is Nothing) Then

                oObject.Dispose()
                oObject = Nothing
            End If



        End Try
        Return result
    End Function
End Class
