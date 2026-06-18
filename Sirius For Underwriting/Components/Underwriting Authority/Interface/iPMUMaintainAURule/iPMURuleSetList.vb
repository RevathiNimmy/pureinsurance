Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmRuleSetList
    Inherits System.Windows.Forms.Form
    'Developer Guide No. 69
    Private frmRuleSet As frmRuleSet

    Private Const ACClass As String = "frmRuleSetList"

    Private m_vRuleSets(,) As Object
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_bUseExistingRuleAsBasis As Boolean

    ' Status
    Private m_lStatus As gPMConstants.PMEReturnCode
    ' SelectedRuleSetId
    Private m_lSelectedRuleSetId As Integer
    ' SelectedRuleSetDesc
    Private m_sSelectedRuleSetDesc As String = ""

    Public Property SelectedRuleSetDesc() As String
        Get
            Return m_sSelectedRuleSetDesc
        End Get
        Set(ByVal Value As String)
            m_sSelectedRuleSetDesc = Value
        End Set
    End Property


    Public Property SelectedRuleSetId() As Integer
        Get
            Return m_lSelectedRuleSetId
        End Get
        Set(ByVal Value As Integer)
            m_lSelectedRuleSetId = Value
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
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.


            m_lReturn = g_oBusiness.GetRuleSet(r_vRuleSets:=m_vRuleSets)

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

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Dim sTitle, sMessage As String
        Dim iMsgResult As DialogResult

        Try

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel


            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.Yes Then
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
        Dim oListItem As ListViewItem

        Try
            'Developer Guide No. 69
            frmRuleSet = New frmRuleSet
            ' Alix Bergeret - 17/02/2003
            ' We need to check if an item is selected in the list!
            If lvwRules.FocusedItem Is Nothing Then
                Exit Sub
            End If
            With frmRuleSet
                .RuleSetID = CInt(m_vRuleSets(ACRuleSetId, Convert.ToString(lvwRules.FocusedItem.Tag)))
                .Code = CStr(m_vRuleSets(ACRuleSetCode, Convert.ToString(lvwRules.FocusedItem.Tag)))

                'Developer Guide No. 24
                .Description = m_vRuleSets(MainModule.ACRuleSetDescription, Convert.ToString(lvwRules.FocusedItem.Tag))
                .EffectiveDate = CDate(m_vRuleSets(ACRuleSetEffectiveDate, Convert.ToString(lvwRules.FocusedItem.Tag)))
                .Live = CInt(m_vRuleSets(ACRuleSetLive, Convert.ToString(lvwRules.FocusedItem.Tag)))
                .FileName = CStr(m_vRuleSets(ACRuleSetFileName, Convert.ToString(lvwRules.FocusedItem.Tag)))

                .Task = gPMConstants.PMEComponentAction.PMEdit
                .ShowDialog()

                If .Status = gPMConstants.PMEReturnCode.PMOK Then

                    m_lReturn = CType(RetrieveRuleSetDetails(Convert.ToString(lvwRules.FocusedItem.Tag)), gPMConstants.PMEReturnCode)

                    oListItem = lvwRules.FocusedItem

                    m_lReturn = CType(PopulateListViewRow(oListItem, Convert.ToString(lvwRules.FocusedItem.Tag)), gPMConstants.PMEReturnCode)

                End If

            End With
            frmRuleSet.Close()
            frmRuleSet = Nothing

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

    Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click
        Dim oListItem As ListViewItem

        Try
            'Developer Guide No. 69
            frmRuleSet = New frmRuleSet

            frmRuleSet.Task = gPMConstants.PMEComponentAction.PMAdd

            If Not m_bUseExistingRuleAsBasis Then
                If MessageBox.Show("If you wish to use an exisiting Rule Set as a basis for this new one, " & "then please press 'Yes', select the Rule Set you require and press 'New' " & "again.", "Adding a rule", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then

                    m_bUseExistingRuleAsBasis = True
                    Exit Sub
                End If
            Else
                ' Alix Bergeret - 17/02/2003
                ' We need to check if an item is selected in the list
                If Not (lvwRules.FocusedItem Is Nothing) Then
                    frmRuleSet.BasisForNewFile = CStr(m_vRuleSets(MainModule.ACRuleSetFileName, Convert.ToString(lvwRules.FocusedItem.Tag)))
                Else
                    MessageBox.Show("You need to select an existing rule first.", "Adding a rule", MessageBoxButtons.OK)
                    m_bUseExistingRuleAsBasis = False
                    Exit Sub
                End If
            End If
            frmRuleSet.ShowDialog()

            m_bUseExistingRuleAsBasis = False
            If frmRuleSet.Status = gPMConstants.PMEReturnCode.PMOK Then
                If Information.IsArray(m_vRuleSets) Then
                    ReDim Preserve m_vRuleSets(ACRuleSetMaxIndex, m_vRuleSets.GetUpperBound(1) + 1)
                Else
                    ReDim m_vRuleSets(ACRuleSetMaxIndex, 0)
                End If

                m_lReturn = CType(RetrieveRuleSetDetails(m_vRuleSets.GetUpperBound(1)), gPMConstants.PMEReturnCode)


                oListItem = lvwRules.Items.Add("")

                m_lReturn = CType(PopulateListViewRow(oListItem, m_vRuleSets.GetUpperBound(1)), gPMConstants.PMEReturnCode)

            End If
            frmRuleSet.Close()
            frmRuleSet = Nothing

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the New command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNew_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Try

            m_lReturn = InterfaceToData()

            ' Alix Bergeret - 17/02/2003
            ' We check the return value
            If m_lReturn < gPMConstants.PMEReturnCode.PMError Then
                m_lStatus = gPMConstants.PMEReturnCode.PMOK
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub Form_Initialize_Renamed()

        m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

    End Sub


    Private Sub frmRuleSetList_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_bUseExistingRuleAsBasis = False

            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwRules.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            ' Gets the interface details to be displayed.
            m_lReturn = CType(BusinessToInterface(), gPMConstants.PMEReturnCode)

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
    '
    ' Name: BusinessToInterface
    '
    ' Description:
    '
    ' History: 05/01/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function BusinessToInterface() As Integer
        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(m_vRuleSets) Then
                For iRuleSetCount As Integer = 0 To m_vRuleSets.GetUpperBound(1)

                    oListItem = lvwRules.Items.Add("") '(Text:=m_vRules(ACUARuleDescription, iRuleCount))
                    m_lReturn = CType(PopulateListViewRow(oListItem, iRuleSetCount), gPMConstants.PMEReturnCode)


                Next iRuleSetCount
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BusinessToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    ' ***************************************************************** '
    Private Function PopulateListViewRow(ByRef oListItem As ListViewItem, ByRef iDataArrayIndex As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oListItem.Text = CStr(m_vRuleSets(ACRuleSetCaption, iDataArrayIndex))
            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vRuleSets(ACRuleSetCode, iDataArrayIndex))
            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vRuleSets(ACRuleSetEffectiveDate, iDataArrayIndex))
            If CDbl(m_vRuleSets(ACRuleSetLive, iDataArrayIndex)) = 1 Then
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Yes"
            Else
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "No"
            End If

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
    ' Name: RetrieveRuleSetDetails
    '
    ' Description:
    '
    ' History: 05/01/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function RetrieveRuleSetDetails(ByVal iDataArrayIndex As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            With frmRuleSet
                m_vRuleSets(ACRuleSetId, iDataArrayIndex) = .RuleSetID
                m_vRuleSets(ACRuleSetCode, iDataArrayIndex) = .Code

                m_vRuleSets(ACRuleSetDescription, iDataArrayIndex) = .Description
                m_vRuleSets(ACRuleSetEffectiveDate, iDataArrayIndex) = .EffectiveDate
                m_vRuleSets(ACRuleSetLive, iDataArrayIndex) = .Live
                m_vRuleSets(ACRuleSetFileName, iDataArrayIndex) = .FileName

                m_vRuleSets(ACRuleSetCaption, iDataArrayIndex) = .Description

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RetrieveRuleSetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RetrieveRuleSetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: InterfaceToData
    '
    ' Description:
    '
    ' History: 05/01/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Alix - 17/02/2003
            ' We need to check if an item is selected!!!
            If Not (lvwRules.FocusedItem Is Nothing) Then
                m_lSelectedRuleSetId = CInt(m_vRuleSets(ACRuleSetId, Convert.ToString(lvwRules.FocusedItem.Tag)))
                m_sSelectedRuleSetDesc = CStr(m_vRuleSets(ACRuleSetCaption, Convert.ToString(lvwRules.FocusedItem.Tag)))
            Else
                result = gPMConstants.PMEReturnCode.PMError
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InterfaceToData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAvailableRuleSetsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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


            lblInstruction.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSelectRuleSet, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub frmRuleSetList_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub
End Class
