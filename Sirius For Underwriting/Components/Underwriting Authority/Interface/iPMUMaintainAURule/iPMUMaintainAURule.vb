Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    'Developer Guide No. 69
    Private frmDetail As frmDetail
    Private Const ACClass As String = "frmInterface"

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_vRules(,) As Object
    Private m_vActionArray() As Object

    ' Object parameter members.
    Private m_iTask As Integer
    Private m_lStatus As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_iSourceId As Integer
    Private m_sCallingAppName As String = ""
    Private m_oGeneral As General


    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property


    Public Property SourceId() As Integer
        Get
            Return m_iSourceId
        End Get
        Set(ByVal Value As Integer)
            m_iSourceId = Value
        End Set
    End Property


    Public Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property


    Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property


    Public Property ProcessMode() As Integer
        Get
            Return m_lProcessMode
        End Get
        Set(ByVal Value As Integer)
            m_lProcessMode = Value
        End Set
    End Property


    Public Property Navigate() As Integer
        Get
            Return m_lNavigate
        End Get
        Set(ByVal Value As Integer)
            m_lNavigate = Value
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


    Public Property ErrorNumber() As Integer
        Get
            Return m_lErrorNumber
        End Get
        Set(ByVal Value As Integer)
            m_lErrorNumber = Value
        End Set
    End Property


    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property



    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim sTitle, sMessage As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bSIRMaintainAURule.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oBusiness = temp_g_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = g_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If


            m_lReturn = g_oBusiness.GetRuleSetLinks(r_vRules:=m_vRules)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    'Don't exit, we need to terminate
                    '            Exit Function
                End If
            End If

            'Dimension ActionArray to correct size.
            If Information.IsArray(m_vRules) Then
                ReDim m_vActionArray(m_vRules.GetUpperBound(1))

            End If

            '    ' Terminate the business object
            '    m_lReturn& = m_oBusiness.Terminate()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to terminate the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetBusiness"
            '    End If
            '
            '    ' Destroy the instance of the business object
            '    ' from memory.
            '    Set m_oBusiness = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: BusinessToInterface
    '
    ' Description:
    '
    ' History: 02/11/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer
        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear list view prior to populating with data.
            lvwRules.Items.Clear()

            If Information.IsArray(m_vRules) Then
                For iRuleCount As Integer = 0 To m_vRules.GetUpperBound(1)

                    oListItem = lvwRules.Items.Add("") '(Text:=m_vRules(ACUARuleDescription, iRuleCount))
                    m_lReturn = CType(PopulateListViewRow(oListItem, iRuleCount), gPMConstants.PMEReturnCode)
                Next iRuleCount
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BusinessToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
        Dim vCopyArray(,) As Object
        Dim iNewIndex, iDeletedIndex As Integer

        Try

            'AG - 15/10/2004 - PN15842 - Add the conditions to check rule is selected or not.
            'START
            If lvwRules.FocusedItem Is Nothing Then
                If lvwRules.Items.Count > 0 Then
                    MessageBox.Show("Please select rule.", Application.ProductName)
                End If
                Exit Sub
            End If
            'END

            If MessageBox.Show("Are you sure you wish to delete the selected rule ?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.Yes Then

                '        'Delete the data from the database.
                '        m_lReturn = g_oBusiness.DeleteRule(Val(m_vRules(ACUARuleId, lvwRules.SelectedItem.Index - 1)), _
                ''                                            Val(m_vRules(ACUARuleTypeId, lvwRules.SelectedItem.Index - 1)), _
                ''                                            Val(m_vRules(ACUARuleIsUnderwriter, lvwRules.SelectedItem.Index - 1)), _
                ''                                            Val(m_vRules(ACUARuleProductId, lvwRules.SelectedItem.Index - 1)), _
                ''                                            Val(m_vRules(ACUARuleTransTypeId, lvwRules.SelectedItem.Index - 1)))

                'Update array and all db updates made when click OK.
                'Update array every time as this has to be done in Edit & Add and
                'anomolies would could occur if we just remove from ListView here.
                '            iDeletedIndex = lvwRules.SelectedItem.Index - 1

                iDeletedIndex = Convert.ToString(lvwRules.FocusedItem.Tag)
                iNewIndex = 0
                For iCount As Integer = 0 To m_vRules.GetUpperBound(1)
                    If iCount <> iDeletedIndex Then
                        If iNewIndex = 0 Then
                            ReDim vCopyArray(ACRuleFieldMaxIndex, 0)
                        Else
                            ReDim Preserve vCopyArray(ACRuleFieldMaxIndex, iNewIndex)
                        End If

                        vCopyArray(ACUARuleId, iNewIndex) = m_vRules(ACUARuleId, iCount)

                        vCopyArray(ACUARuleCaptionId, iNewIndex) = m_vRules(ACUARuleCaptionId, iCount)

                        vCopyArray(ACUARuleCode, iNewIndex) = m_vRules(ACUARuleCode, iCount)

                        vCopyArray(ACUARuleDescription, iNewIndex) = m_vRules(ACUARuleDescription, iCount)

                        vCopyArray(ACUARuleEffectiveDate, iNewIndex) = m_vRules(ACUARuleEffectiveDate, iCount)

                        vCopyArray(ACUARuleFileName, iNewIndex) = m_vRules(ACUARuleFileName, iCount)

                        vCopyArray(ACUARuleLive, iNewIndex) = m_vRules(ACUARuleLive, iCount)

                        vCopyArray(ACUARuleTypeId, iNewIndex) = m_vRules(ACUARuleTypeId, iCount)

                        vCopyArray(ACUARuleIsUnderwriter, iNewIndex) = m_vRules(ACUARuleIsUnderwriter, iCount)

                        vCopyArray(ACUARuleProductId, iNewIndex) = m_vRules(ACUARuleProductId, iCount)

                        vCopyArray(ACUARuleTransTypeId, iNewIndex) = m_vRules(ACUARuleTransTypeId, iCount)

                        vCopyArray(ACUARuleAuthTypeDescription, iNewIndex) = m_vRules(ACUARuleAuthTypeDescription, iCount)

                        vCopyArray(ACUARuleProductDescription, iNewIndex) = m_vRules(ACUARuleProductDescription, iCount)
                        'sj 01/04/2003 - start

                        vCopyArray(ACUARuleTransactionTypeDescription, iNewIndex) = m_vRules(ACUARuleTransactionTypeDescription, iCount)
                        'sj 01/04/2003 - end
                        iNewIndex += 1
                    End If
                Next iCount

                m_vRules = Nothing


                m_vRules = vCopyArray

                m_lReturn = CType(BusinessToInterface(), gPMConstants.PMEReturnCode)

                '        End If

            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Delete command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
        Dim oListItem As ListViewItem

        Try
            'Developer Guide No. 69
            frmDetail = New frmDetail

            '    If (lvwRules.SelectedItem.Index < 1) Then
            '        MsgBox "Please select rule."
            '        Exit Sub
            '    End If

            'AG - 15/10/2004 - PN15842 - Add the conditions to check rule is selected or not.
            'START
            If lvwRules.FocusedItem Is Nothing Then
                If lvwRules.Items.Count > 0 Then
                    MessageBox.Show("Please select rule.", Application.ProductName)
                End If
                Exit Sub
            End If
            'END
            With frmDetail
                .Task = gPMConstants.PMEComponentAction.PMEdit
                .AuthorityLevelId = CInt(m_vRules(ACUARuleTypeId, Convert.ToString(lvwRules.FocusedItem.Tag)))
                .ProductId = CInt(m_vRules(ACUARuleProductId, Convert.ToString(lvwRules.FocusedItem.Tag)))
                .TransactionTypeId = CInt(m_vRules(ACUARuleTransTypeId, Convert.ToString(lvwRules.FocusedItem.Tag)))
                .IsUnderwriter = (CDbl(m_vRules(ACUARuleIsUnderwriter, Convert.ToString(lvwRules.FocusedItem.Tag))) = 1)
                .RuleSetID = CInt(m_vRules(ACUARuleId, Convert.ToString(lvwRules.FocusedItem.Tag)))
                .RuleSetDescription = CStr(m_vRules(ACUARuleDescription, Convert.ToString(lvwRules.FocusedItem.Tag)))

                .ShowDialog()
                If .Status = gPMConstants.PMEReturnCode.PMOK Then
                    m_lReturn = CType(RetrieveLinkDetails(Convert.ToString(lvwRules.FocusedItem.Tag)), gPMConstants.PMEReturnCode)

                    oListItem = lvwRules.FocusedItem
                    m_lReturn = CType(PopulateListViewRow(oListItem, Convert.ToString(lvwRules.FocusedItem.Tag)), gPMConstants.PMEReturnCode)

                End If
            End With
            frmDetail.Close()
            frmDetail = Nothing

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Edit command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, lContextID:=MainModule.ScreenHelpID), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click
        Dim oListItem As ListViewItem
        Dim bDuplicates As Boolean

        Try
            'Developer Guide No. 69
            frmDetail = New frmDetail

            frmDetail.Task = gPMConstants.PMEComponentAction.PMAdd
            frmDetail.ShowDialog()

            If frmDetail.Status = gPMConstants.PMEReturnCode.PMOK Then
                m_lReturn = CType(CheckForDuplicates(r_bDuplicates:=bDuplicates), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    frmDetail.Close()
                    frmDetail = Nothing
                    Exit Sub
                End If

                If bDuplicates Then
                    MessageBox.Show("This combination is already defined", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    frmDetail.Close()
                    frmDetail = Nothing
                    Exit Sub
                End If

                If Information.IsArray(m_vRules) Then
                    ReDim Preserve m_vRules(ACRuleFieldMaxIndex, m_vRules.GetUpperBound(1) + 1)
                Else
                    ReDim m_vRules(ACRuleFieldMaxIndex, 0)
                End If

                m_lReturn = CType(RetrieveLinkDetails(m_vRules.GetUpperBound(1)), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    frmDetail.Close()
                    frmDetail = Nothing
                    Exit Sub
                End If


                oListItem = lvwRules.Items.Add("") '(Text:=m_vRules(ACUARuleDescription, UBound(m_vRules, 2)))

                m_lReturn = CType(PopulateListViewRow(oListItem, m_vRules.GetUpperBound(1)), gPMConstants.PMEReturnCode)

            End If
            '    End With
            frmDetail.Close()
            frmDetail = Nothing

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the New command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNew_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        ' Click event of the OK button.

        Try

            'At present all updates will already have been done by this time, so no
            'further processing will be done.

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="Err_cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub



        End Try

    End Sub

    Private Sub Form_Initialize_Renamed()
        ' Forms initialise event.
        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue


            ' Create an instance of the general interface object.
            m_oGeneral = New iPMUMaintainAURule.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=g_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

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

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
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
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwRules.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            ' {* USER DEFINED CODE (Begin) *}#

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


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************



            lvwRules.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblRuleFile, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwRules.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblAuthorityLevel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwRules.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblProduct, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwRules.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblIsUnderwriter, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwRules.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblTransactionType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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
            Dim m_ctlTabFirstLast(1, 0) As Object

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

            '    Set m_ctlTabFirstLast(ACControlStart, 0) = tvwDataDictionary
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = tvwDataDictionary

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
    '
    ' Name: UpdateBusiness
    '
    ' Description:
    '
    ' History: 04/01/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBusiness")

                Return result
            End If


            m_lReturn = g_oBusiness.Update(m_vRules)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBusiness")
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PopulateListViewRow
    '
    ' Description:
    '
    ' History: 05/01/2001 RWH - Created.
    '
    ' RAM20030109  : Updated the code to reflect the new Transaction Type
    '                Ref. NRMA Project Changes. Process No. 426
    ' ***************************************************************** '
    Private Function PopulateListViewRow(ByRef oListItem As ListViewItem, ByRef iDataArrayIndex As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oListItem.Text = CStr(m_vRules(ACUARuleDescription, iDataArrayIndex))
            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vRules(ACUARuleAuthTypeDescription, iDataArrayIndex))
            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vRules(ACUARuleProductDescription, iDataArrayIndex))
            If CDbl(m_vRules(ACUARuleIsUnderwriter, iDataArrayIndex)) = 1 Then
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Yes"
            Else
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "No"
            End If

            'sj 01/04/2003 - start
            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vRules(ACUARuleTransactionTypeDescription, iDataArrayIndex))

            '    Select Case (m_vRules(ACUARuleTransTypeId, iDataArrayIndex))
            '        Case SIRTransCodeIdNewBusiness
            '            oListItem.SubItems(4) = SIRTransCodeDescNewBusiness
            '
            '        Case SIRTransCodeIdAdditionalPremium
            '            oListItem.SubItems(4) = SIRTransCodeDescAdditionalPremium
            '
            '        Case SIRTransCodeIdReturnPremium
            '            oListItem.SubItems(4) = SIRTransCodeDescReturnPremium
            '
            '        Case SIRTransCodeIdRenewal
            '            oListItem.SubItems(4) = SIRTransCodeDescRenewal
            '
            '        Case SIRTransCodeIdClaimOpen
            '            oListItem.SubItems(4) = SIRTransCodeDescClaimOpen
            '
            '        Case SIRTransCodeIdClaimRevision
            '            oListItem.SubItems(4) = SIRTransCodeDescClaimRevision
            '
            '        Case SIRTransCodeIdClaimPaid
            '            oListItem.SubItems(4) = SIRTransCodeDescClaimPaid
            '
            '        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '        ' RAM20030109 : Added the following 2 more Transaction Type
            '        Case SIRTransCodeIdBackdatedCancellation
            '            oListItem.SubItems(4) = SIRTransCodeDescBackdatedCancellation
            '
            '        Case SIRTransCodeIdBackdatedMTA
            '            oListItem.SubItems(4) = SIRTransCodeDescBackdatedMTA
            '        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '
            '    End Select
            'sj 01/04/2003 - end
            oListItem.Tag = CStr(iDataArrayIndex)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateListViewRow Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateListViewRow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RetrieveLinkDetails
    '
    ' Description:
    '
    ' History: 05/01/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function RetrieveLinkDetails(ByVal iDataArrayIndex As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            With frmDetail
                m_vRules(ACUARuleTypeId, iDataArrayIndex) = .AuthorityLevelId
                m_vRules(ACUARuleProductId, iDataArrayIndex) = .ProductId
                m_vRules(ACUARuleTransTypeId, iDataArrayIndex) = .TransactionTypeId
                m_vRules(ACUARuleId, iDataArrayIndex) = .RuleSetID
                m_vRules(ACUARuleDescription, iDataArrayIndex) = .RuleSetDescription
                m_vRules(ACUARuleAuthTypeDescription, iDataArrayIndex) = .AuthorityLevelDescription
                m_vRules(ACUARuleProductDescription, iDataArrayIndex) = .ProductDescription
                'sj 01/04/2003 - start
                m_vRules(ACUARuleTransactionTypeDescription, iDataArrayIndex) = .TransactionType
                'sj 01/04/2003 - end
                If .IsUnderwriter Then
                    m_vRules(ACUARuleIsUnderwriter, iDataArrayIndex) = 1
                Else
                    m_vRules(ACUARuleIsUnderwriter, iDataArrayIndex) = 0
                End If
            End With


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RetrieveLinkDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RetrieveLinkDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function CheckForDuplicates(ByRef r_bDuplicates As Boolean) As Integer

        Dim result As Integer = 0
        Dim lAuthorityLevelId, lProductId, lTransactionTypeId, lIsUnderwriter As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_bDuplicates = False

            If Not Information.IsArray(m_vRules) Then
                Return result
            End If
            lAuthorityLevelId = frmDetail.AuthorityLevelId
            lProductId = frmDetail.ProductId
            lTransactionTypeId = frmDetail.TransactionTypeId
            If frmDetail.IsUnderwriter Then
                lIsUnderwriter = 1
            Else
                lIsUnderwriter = 0
            End If

            For lCount As Integer = m_vRules.GetLowerBound(1) To m_vRules.GetUpperBound(1)
                If (CDbl(m_vRules(ACUARuleTypeId, lCount)) = lAuthorityLevelId) And (CDbl(m_vRules(ACUARuleProductId, lCount)) = lProductId) And (CDbl(m_vRules(ACUARuleTransTypeId, lCount)) = lTransactionTypeId) And (CDbl(m_vRules(ACUARuleTypeId, lCount)) = lAuthorityLevelId) Then
                    r_bDuplicates = True
                    Return result
                End If
            Next lCount

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForDuplicates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckForDuplicates", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub
End Class
