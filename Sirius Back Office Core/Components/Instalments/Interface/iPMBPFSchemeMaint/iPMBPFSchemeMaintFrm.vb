Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'refer Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 20/10/2000
    '
    ' Description: Main interface.
    '
    ' Edit History: TF201000 - Created
    ' ***************************************************************** '

    'Developer Guide No 7
    Private Const vbFormCode As Integer = 0
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lCompanyNo As Integer
    Private m_lSchemeNo As Integer
    Private m_lSchemeVersion As Integer
    Private m_sSchemeCode As String = ""
    Private m_sSchemeName As String = ""
    Private m_lPartyCnt As Integer
    Private m_sPartyCode As String = ""
    Private m_sPartyName As String = ""

    Private m_iNotEditable As Integer

    Private m_bDeleteMode As Boolean
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBPFSchemeMaint.General

    ' Declare an instance of the Business object.

    Private m_oBusiness As bSIRPFScheme.Business

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookUpDetails As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Declare an instance of the Lock object.

    Private m_oPMLock As bPMLock.User

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the search data from the business object.
    Public m_vSearchData(,) As Object
    ' PRIVATE Data Members (End)

    'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.7.2.1)
    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean


    Public Property DisableWildcardSearchOption() As Boolean
        Get
            Return m_bDisableWildcardSearchOption
        End Get
        Set(ByVal Value As Boolean)
            m_bDisableWildcardSearchOption = Value
        End Set
    End Property


    Public Property EnablePartialWildcardSearchOption() As Boolean
        Get
            Return m_bEnablePartialWildcardSearchOption
        End Get
        Set(ByVal Value As Boolean)
            m_bEnablePartialWildcardSearchOption = Value
        End Set
    End Property
    'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.7.2.1)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)
    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    Public Property SchemeNo() As Integer
        Get

            Return m_lSchemeNo

        End Get
        Set(ByVal Value As Integer)

            m_lSchemeNo = Value

        End Set
    End Property

    Public Property SchemeVersion() As Integer
        Get

            Return m_lSchemeVersion

        End Get
        Set(ByVal Value As Integer)

            m_lSchemeVersion = Value

        End Set
    End Property

    Public ReadOnly Property SchemeName() As String
        Get

            Return m_sSchemeName

        End Get
    End Property

    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property

    Public Property PartyCode() As String
        Get

            Return m_sPartyCode

        End Get
        Set(ByVal Value As String)

            m_sPartyCode = Value

        End Set
    End Property

    Public ReadOnly Property PartyName() As String
        Get

            Return m_sPartyName

        End Get
    End Property
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

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

            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while
            ' a search is in progress.
            m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}
            ' Build query from supplied criteria

            m_lReturn = g_oBusiness.SearchByQuery(r_lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=m_vSearchData, v_vSchemeNumber:=txtSchemeNo.Text, v_vSchemeName:=txtSchemeName.Text, v_vPartyCode:=txtPartyCode.Text, v_vPartyName:=txtPartyName.Text)

            'Assign Values to Interface
            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)

            ' {* USER DEFINED CODE (End) *}

            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.
                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No search details found.
                Case Else
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
            End Select

            ' Display the number of item found message.
            DisplayStatusFound()

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    '
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Const ACFindImage As String = "FindImage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwSearchDetails.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If

            ' Assign the details to the interface.
            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1 Scheme name

                oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACISchemeName, lRow)).Trim(), ACFindImage)
                'oListItem.ImageKey = "FindImage"
                ' Assign details to the other columns
                ' Column 2 Scheme Number
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vSearchData(ACISchemeNo, lRow)).Trim()

                ' Column 3 Scheme Version
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vSearchData(ACISchemeVersion, lRow)).Trim()

                ' Column 4 Party Name
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vSearchData(ACIPartyName, lRow)).Trim()

                ' Column 5 Company Number
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vSearchData(ACICompanyNo, lRow)).Trim()

                ' {* USER DEFINED CODE (End) *}

                ' Set the tag property with the index of
                ' the search data storage.
                oListItem.Tag = CStr(lRow)

                ' Refresh the first X amount of rows, to
                ' allow the user to see the results instantly.
                If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.
                    lvwSearchDetails.Items.Item(0).Selected = True

                    ' Refresh the initial results.
                    lvwSearchDetails.Refresh()
                End If

            Next lRow

            ' Select the first item.
            If lvwSearchDetails.Items.Count > 0 Then
                lvwSearchDetails.Items.Item(0).Selected = True

                ' Enable the interface now that the search has completed.
                m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToProperties
    '
    ' Description: Updates the property member from the search data
    '              storage.
    '
    ' ***************************************************************** '
    Public Function DataToProperties() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}

            m_lCompanyNo = CInt(m_vSearchData(ACICompanyNo, lSelectedItem))
            m_lSchemeNo = CInt(m_vSearchData(ACISchemeNo, lSelectedItem))
            m_lSchemeVersion = CInt(m_vSearchData(ACISchemeVersion, lSelectedItem))
            m_sSchemeName = CStr(m_vSearchData(ACISchemeName, lSelectedItem)).Trim()
            m_sPartyCode = CStr(m_vSearchData(ACIPartyCode, lSelectedItem)).Trim()
            m_sPartyName = CStr(m_vSearchData(ACIPartyName, lSelectedItem)).Trim()
            m_lPartyCnt = CInt(m_vSearchData(ACIPartyCnt, lSelectedItem))

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: PropertiesToInterface
    '
    ' Description: Updates the interface details from the property
    '              members.
    '
    ' ***************************************************************** '
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' {* USER DEFINED CODE (Begin) *}

            If m_lSchemeNo <> 0 Then
                txtSchemeNo.Text = CStr(m_lSchemeNo)
            Else
                txtSchemeNo.Text = ""
            End If
            txtSchemeName.Text = m_sSchemeName.Trim()
            txtPartyCode.Text = m_sPartyCode.Trim()
            txtPartyName.Text = m_sPartyName.Trim()

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: HideColumns
    '
    ' Description: Hides columns. Leaves "v_iShowLeft" showing.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (HideColumns) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function HideColumns(ByVal v_iShowLeft As Integer) As Integer
    '
    'Dim result As Integer = 0
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'For 'iLoop1 As Integer = v_iShowLeft + 1 To lvwSearchDetails.Columns.Count
    'lvwSearchDetails.Columns.Item(iLoop1 - 1).Width = CInt(0)
    'Next iLoop1
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="HideColumns Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="HideColumns", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

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
                    'ECK 21/05/99 Don't nedd to see this anymore
                    cmdNavigate.Visible = False
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            ' Set the column widths for the search list.
            lvwSearchDetails.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1980))
            lvwSearchDetails.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(700))
            lvwSearchDetails.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(700))
            lvwSearchDetails.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(3500))
            lvwSearchDetails.Columns.Item(4).Width = CInt(VB6.TwipsToPixelsX(1980))


            'Developer Guide No 243
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Update the interface details with the
            ' property members.
            m_lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwSearchDetails.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function OnRecentFilesList(ByRef vShortName As String) As Integer

        Dim result As Integer = 0
        ' Counter variable.


        For i As Integer = 0 To 4
            If mnuRecentFile(i).Text = vShortName.Trim() Then
                Return i
            End If
        Next i

        Return False

    End Function

    Sub UpdateFileMenu(ByRef vFileName As String)


        ' Check if the open filename is already in the File menu control array.
        Dim vShortName As String = vFileName.Substring(0, vFileName.IndexOf(","c))
        Dim intRetVal As Integer = OnRecentFilesList(vShortName)
        '        If Not intRetVal Then
        ' Write open filename to the registry.
        WriteRecentFiles(vFileName, intRetVal)
        '        End If

        ' Update the list of the most recently opened files in the File menu control array.
        GetRecentFiles()

    End Sub

    Sub GetRecentFiles()
        ' This procedure demonstrates the use of the GetAllSettings function,
        ' which returns an array of values from the Windows registry. In this
        ' case, the registry contains the files most recently opened.  Use the
        ' SaveSetting statement to write the names of the most recent files.
        ' That statement is used in the WriteRecentFiles procedure.

        Dim iTemp As Integer
        Dim sTemp, sShortName, sLongName As String

        ' Get recent files from the registry using the GetAllSettings statement.
        ' ThisApp and ThisKey are constants defined in this module.

        If String.IsNullOrEmpty(Interaction.GetSetting(ThisApp, ThisKey, "RecentFile1", )) Then Exit Sub

        Dim varFiles As Object = Interaction.GetAllSettings(ThisApp, ThisKey) ' Variable to store the returned array.


        For i As Integer = 0 To varFiles.GetUpperBound(0)

            sTemp = CStr(varFiles(i, 1))
            iTemp = (sTemp.IndexOf(","c) + 1)
            sShortName = sTemp.Substring(0, iTemp - 1)
            sTemp = sTemp.Substring(iTemp)
            iTemp = (sTemp.IndexOf(","c) + 1)
            sLongName = sTemp.Substring(0, iTemp - 1)
            mnuRecentFile(0).Available = True
            mnuRecentFile(i).Text = sShortName


            mnuRecentFile(i).Tag = CStr(varFiles(i, 1))
            mnuRecentFile(i).Available = True
        Next i

    End Sub

    Sub WriteRecentFiles(ByRef vFileName As String, ByRef vId As Integer)
        ' This procedure uses the SaveSettings statement to write the names of
        ' recently opened files to the System registry. The SaveSetting
        ' statement requires three parameters. Two of the parameters are
        ' stored as constants and are defined in this module.  The GetAllSettings
        ' function is used in the GetRecentFiles procedure to retrieve the
        ' file names stored in this procedure.

        Dim strFile, key As String

        ' Copy RecentFile1 to RecentFile2, and so on.
        If vId = 5 Or vId = 0 Then
            For i As Integer = 4 To 1 Step -1
                key = "RecentFile" & i

                strFile = Interaction.GetSetting(ThisApp, ThisKey, key, )
                If strFile <> "" Then

                    key = "RecentFile" & (CStr(i + 1))
                    Interaction.SaveSetting(ThisApp, ThisKey, key, strFile)
                End If
            Next i
        Else
            For i As Integer = vId - 1 To 1 Step -1
                key = "RecentFile" & i

                strFile = Interaction.GetSetting(ThisApp, ThisKey, key, )
                If strFile <> "" Then

                    key = "RecentFile" & (CStr(i + 1))
                    Interaction.SaveSetting(ThisApp, ThisKey, key, strFile)
                End If
            Next i

        End If
        strFile = vFileName
        Interaction.SaveSetting(ThisApp, ThisKey, "RecentFile1", strFile)

    End Sub

    ' ***************************************************************** '
    ' Name: ClearInterface
    '
    ' Description: Clears all of the interface details for a new
    '              search.
    '
    ' ***************************************************************** '
    Private Function ClearInterface(ByRef bConfirm As Boolean) As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the interface details.

            If bConfirm Then

                ' Check if the user still wishes to clear
                ' the interface.


                'Developer Guide No 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guide No 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display the message.
                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    ' Don't continue with the clear.
                    Return result
                End If

            End If


            ' Clear the search data array.
            m_vSearchData = Nothing

            ' Clear the search list details.
            lvwSearchDetails.Items.Clear()

            ' Clear the search status bar.
            stbStatus.Text = ""

            ' {* USER DEFINED CODE (Begin) *}

            txtSchemeNo.Text = ""
            txtSchemeName.Text = ""
            txtPartyCode.Text = ""
            txtPartyName.Text = ""

            ' Set to the first tab.
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            ' Set focus to the search details.
            txtSchemeNo.Focus()

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

            ' {* USER DEFINED CODE (End) *}

            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            ReDim m_ctlTabFirstLast(1, 2)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            m_ctlTabFirstLast(ACControlStart, 0) = txtSchemeNo
            m_ctlTabFirstLast(ACControlEnd, 0) = txtPartyName

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

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


            'Developer Guide No 243
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            lblSchemeNo.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSchemeNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            lblSchemeName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSchemeName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            lblPartyCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPartyCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            lblPartyName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPartyName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            'Developer Guide No 243
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If m_bDeleteMode Then

                'Developer Guide No 243
                cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else

                'Developer Guide No 243
                cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If


            'Developer Guide No 243
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            cmdFindNow.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindNowButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            cmdNewSearch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewSearchButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            cmdNew.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}



            'Developer Guide No 243
            lvwSearchDetails.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No 243
            lvwSearchDetails.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No 243
            lvwSearchDetails.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No 243
            lvwSearchDetails.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No 243
            lvwSearchDetails.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdOK.Enabled = Not bDisable
            'If we're here we're searching.  Disable it until an item is clicked.
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

                'Developer Guide No 243
                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbStatus_Panel1.Text = " " & sMessage

        Catch excep As System.Exception




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
        Dim lItemsFound As Integer

        Try

            ' Store the total of item found.
            lItemsFound = lvwSearchDetails.Items.Count

            ' Get message text if not already present.
            If sMessage = "" Then

                'Developer Guide No 243
                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbStatus_Panel1.Text = " " & lItemsFound & " " & sMessage

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: CheckMandatory
    '
    ' Description: Check if all mandatory fields have been entered in
    '              order for the search to proceed.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory() As Integer

        Dim result As Integer = 0
        Dim sText As String = ""
        Dim iPos As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check all fields for data.
            ' At least one field must be populated

            sText = txtSchemeNo.Text.Trim()
            iPos = (sText.IndexOf("%"c) + 1)
            If iPos = 1 Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If
            If iPos > 1 Then
                sText = txtSchemeNo.Text.Substring(0, iPos - 1)
            End If
            If iPos <> 1 Then
                Dim dbNumericTemp As Double
                If (sText <> "") And (Double.TryParse(sText, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If txtSchemeName.Text.Trim() <> "" Then
                If txtSchemeName.Text.Trim().Length >= ACMinSearchLength Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If txtPartyCode.Text.Trim() <> "" Then
                If txtPartyCode.Text.Trim().Length >= ACMinSearchLength Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If txtPartyName.Text.Trim() <> "" Then
                If txtPartyName.Text.Trim().Length >= ACMinSearchLength Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for mandatory fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ResizeInterface
    '
    ' Description: Resizes the interface controls.
    '
    ' ***************************************************************** '
    Private Function ResizeInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdFindNow.Left = Me.Width - VB6.TwipsToPixelsX(1335)
            cmdNewSearch.Left = Me.Width - VB6.TwipsToPixelsX(1335)

            'imgImage.Left = Me.Width - 975

            tabMainTab.Width = Me.Width - VB6.TwipsToPixelsX(1560)

            lvwSearchDetails.Width = Me.Width - VB6.TwipsToPixelsX(360)
            lvwSearchDetails.Height = Me.Height - VB6.TwipsToPixelsY(4500)

            cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(1335)
            cmdHelp.Top = Me.Height - VB6.TwipsToPixelsY(1100)

            cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(2535)
            cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(1100)

            cmdOK.Left = Me.Width - VB6.TwipsToPixelsX(3735)
            cmdOK.Top = Me.Height - VB6.TwipsToPixelsY(1100)

            cmdNew.Top = Me.Height - VB6.TwipsToPixelsY(1100)
            cmdEdit.Top = Me.Height - VB6.TwipsToPixelsY(1100)
            cmdDelete.Top = Me.Height - VB6.TwipsToPixelsY(1100)

            If cmdNavigate.Visible Then
                cmdNavigate.Top = Me.Height - VB6.TwipsToPixelsY(1395)
            End If

            Return result

        Catch





            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: ProcessSchemeInterface(Private)
    '
    ' Description: Calls the appropriate Party Interface
    '
    ' ***************************************************************** '

    Private Function ProcessSchemeInterface(Optional ByVal v_lCompanyNo As Integer = 0, Optional ByVal v_lSchemeNo As Integer = 0, Optional ByVal v_lSchemeVersion As Integer = 0, Optional ByVal v_iTask As Integer = 0, Optional ByVal v_lIndex As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim oScheme As iPMBPFScheme.Interface_Renamed
        Dim vFileName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the interface
            Dim temp_oScheme As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oScheme, sClassName:="iPMBPFScheme.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oScheme = temp_oScheme

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                If Not (oScheme Is Nothing) Then

                    oScheme.Dispose()
                    oScheme = Nothing
                End If
                Throw New Exception()
            End If

            'DD110901
            ' if adding then search for a Finance Provider first.
            '    If (v_iTask = PMAdd) Then
            '
            '        ' Create the Find Finance Provider interface
            '        m_lReturn& = g_oObjectManager.GetInstance( _
            ''            oObject:=oProvider, _
            ''            sClassName:="iPMBFindParty.Interface", _
            ''            vInstanceManager:=PMGetLocalInterface)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            ProcessSchemeInterface = PMFalse
            '            If (oScheme Is Nothing) = False Then
            '                m_lReturn& = oScheme.Terminate()
            '                Set oScheme = Nothing
            '            End If
            '            GoTo Err_ProcessSchemeInterface
            '         End If
            '
            '        ReDim vKeyArray(1, 0)
            '        vKeyArray(PMKeyName, 0) = "special_party"
            '        vKeyArray(PMKeyValue, 0) = "FP"
            '        oProvider.SetKeys vKeyArray
            '
            '        ' start the object
            '        m_lReturn& = oProvider.Start()
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            ProcessSchemeInterface = PMFalse
            '            m_lReturn& = oProvider.Terminate()
            '            Set oProvider = Nothing
            '            GoTo Err_ProcessSchemeInterface
            '        Else
            '            ' get the returned Provider key
            '            oProvider.GetKeys vKeyArray
            '
            '            oScheme.PartyCnt = vKeyArray(1, 0)
            '
            '            ' shutdown the provider form
            '            m_lReturn& = oProvider.Terminate()
            '            Set oProvider = Nothing
            '
            '
            '            If (m_lReturn& <> PMTrue) Then
            '                ProcessSchemeInterface = PMFalse
            '                m_lReturn& = oScheme.Terminate()
            '                Set oScheme = Nothing
            '                GoTo Err_ProcessSchemeInterface
            '            End If
            '        End If
            '    End If

            If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                ' set the process mode if adding a new party

                m_lReturn = oScheme.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
            ElseIf (v_iTask = gPMConstants.PMEComponentAction.PMEdit) Then
                ' set the scheme no & version and process mode if editing
                With oScheme

                    .CompanyNo = v_lCompanyNo

                    .SchemeNo = v_lSchemeNo

                    .SchemeVersion = v_lSchemeVersion


                    m_lReturn = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
                End With
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    oScheme.Dispose()
                    oScheme = Nothing
                    Throw New Exception()
                End If

            End If

            ' start the object

            m_lReturn = oScheme.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oScheme.Dispose()
                oScheme = Nothing
                Return result
            End If


            If oScheme.Status <> gPMConstants.PMEReturnCode.PMCancel Then


                Select Case v_iTask
                    Case gPMConstants.PMEComponentAction.PMEdit
                        If v_lIndex = 0 Then
                            'update the details in the listview - they may have changed
                            With oScheme

                                lvwSearchDetails.Items.Item(v_lIndex - 1).Text = .SchemeName

                                ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(v_lIndex - 1), 1).Text = .SchemeNo

                                ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(v_lIndex - 1), 2).Text = .SchemeVersion

                                ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(v_lIndex - 1), 3).Text = .PartyName

                                ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(v_lIndex - 1), 4).Text = .CompanyNo
                            End With
                        End If

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' Clear the interface details.
                        m_lReturn = CType(ClearInterface(bConfirm:=False), gPMConstants.PMEReturnCode)

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to clear the interface details.
                        End If

                        'Set the scheme no field and do another search to populate with the new scheme

                        txtSchemeNo.Text = oScheme.SchemeNo

                        cmdFindNow_Click(cmdFindNow, New EventArgs())

                    Case Else

                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Process Scheme Interface.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSchemeInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        oScheme.Dispose()
                        oScheme = Nothing
                        Return result

                End Select
            End If

            'ECK 18/05/99
            If v_iTask = gPMConstants.PMEComponentAction.PMEdit Then



                vFileName = oScheme.SchemeName & "," & oScheme.SchemeNo & "," & oScheme.SchemeVersion
                UpdateFileMenu(vFileName)
            Else

                If oScheme.Status <> gPMConstants.PMEReturnCode.PMCancel Then



                    vFileName = oScheme.SchemeName & "," & oScheme.SchemeNo & "," & oScheme.SchemeVersion
                    UpdateFileMenu(vFileName)
                End If
            End If
            Dim Space_count As Integer = 0
            For i As Integer = 0 To 4
                If mnuRecentFile(i).Text = "" Then
                    Space_count += 1
                    If Space_count > 1 Then
                        mnuRecentFile(i).Visible = False
                    End If
                End If
            Next
            ' Destroy the object

            oScheme.Dispose()
            oScheme = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process the Scheme Interface.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSchemeInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'ECK 19/5/99
            ' Clear the interface details.
            m_lReturn = CType(ClearInterface(bConfirm:=False), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If
            Return result

        End Try
    End Function

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        Dim sMessage As String


        If Not (lvwSearchDetails.FocusedItem Is Nothing) Then
            If MessageBox.Show("Are you sure you wish to delete the Scheme '" & lvwSearchDetails.FocusedItem.Text & "'?", "Delete Scheme", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then

                ' Create the interface
                Dim temp_m_oBusiness As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPFScheme.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oBusiness = temp_m_oBusiness

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If Not (m_oBusiness Is Nothing) Then

                        m_oBusiness.Dispose()
                        m_oBusiness = Nothing
                    End If

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the PFScheme Business Object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Else

                    'Developer Guide No. 52
                    Dim sUniqueId As String = GetUniqueID()
                    m_lReturn = m_oBusiness.DirectDelete(lvwSearchDetails.FocusedItem.SubItems(4).Text, lvwSearchDetails.FocusedItem.SubItems(1).Text, lvwSearchDetails.FocusedItem.SubItems(2).Text, sUniqueId)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to delete.
                        'TR - A little more detail to the user. If the scheme
                        'has been used, it cannot be deleted. If the scheme
                        'hasn't been used then the user must go in and delete
                        'any rates, products and branches directly
                        sMessage = "In order to delete a scheme you must delete the " & "Rate records and remove the related Branches "
                        'Only Underwriting uses Products apparently
                        sMessage = sMessage & "and Products "

                        sMessage = sMessage & "in the Pick Lists. If this scheme " & "has been used by a customer then it cannot be deleted."
                        'TR - Display new message
                        MessageBox.Show(sMessage, Application.ProductName)
                    Else
                        'refresh the list
                        cmdFindNow_Click(cmdFindNow, New EventArgs())
                    End If


                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
            End If
        End If
    End Sub

    ' PRIVATE Methods (End)
    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        'refer Developer Guide No. 184
        m_lReturn = CType(PMHelpFunc.ShowHelp(objCnt:=cmdHelp, lContextID:=ScreenHelpID), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        End If

    End Sub
    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBPFSchemeMaint.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'Get bPMLock
            Dim temp_m_oPMLock As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMLock = temp_m_oPMLock

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                'Initialise = PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

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

            GetRecentFiles()

            ' {* USER DEFINED CODE (Begin) *}

            If CheckMandatory() <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Inadequate data so cannot
                ' continue with the search.
                DisableInterface(True)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' {* USER DEFINED CODE (End) *}

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


            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1

                    'Developer Guide No 7
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Destroy the instance of the lock object
            ' from memory.
            If Not (m_oPMLock Is Nothing) Then

                m_oPMLock = Nothing
            End If

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMainTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If
                End Select
            End With
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            m_lReturn = CType(ResizeInterface(), gPMConstants.PMEReturnCode)

        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        GetRecentFiles()

    End Sub

    Public Sub mnuRecentFile_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuRecentFile_0.Click, _mnuRecentFile_1.Click, _mnuRecentFile_2.Click, _mnuRecentFile_3.Click, _mnuRecentFile_4.Click
        Dim Index As Integer = Array.IndexOf(mnuRecentFile, eventSender)
        Dim vPartyCnt As Integer
        Dim sTemp As String = Convert.ToString(mnuRecentFile(Index).Tag)
        Dim iTemp As Integer = (sTemp.IndexOf(","c) + 1)
        'Developer Guide No 292
        Dim vSchemeName As String = ""
        If (sTemp.Length >= iTemp - 1) Then
            vSchemeName = sTemp.Substring(0, iTemp - 1)
        End If
        txtSchemeName.Text = vSchemeName
        txtSchemeNo.Text = ""
        txtPartyCode.Text = ""
        txtPartyName.Text = ""

        m_lPartyCnt = vPartyCnt

        cmdFindNow_Click(cmdFindNow, New EventArgs())

        'Update the list of recently opened files in the File menu control array.
        GetRecentFiles()

    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                ' Set the default button.
                '        If (.Tab < cmdNext.Count) Then
                '            cmdNext(.Tab).Default = True
                '        Else
                '            cmdOK.Default = True
                '        End If

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                End If
            End With

        Catch





            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click

        Dim sText As String = ""
        Dim iPos As Integer
        'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.7.2.2)
        Dim sWildcardErrorMessage As String = ""
        'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.7.2.2)

        ' Click event of the Cancel button.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Scheme Number
            sText = txtSchemeNo.Text.Trim()
            iPos = (sText.IndexOf("%"c) + 1)
            If iPos > 1 Then
                sText = txtSchemeNo.Text.Substring(0, iPos - 1)
            End If
            If iPos <> 1 Then
                'DD 09/07/2002: Added check that SchemeNo is not blank
                'otherwise a search could not go ahead on another field.
                Dim dbNumericTemp As Double
                If (Not Double.TryParse(sText, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) And (sText <> "") Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    MessageBox.Show("Scheme number must be numeric", "Find PF scheme.")
                    txtSchemeNo.Focus()
                    Exit Sub
                End If
            End If

            'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.7.2.2)
            'Check wildcard searches

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtSchemeNo.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Scheme")
                txtSchemeNo.Focus()
                Exit Sub

            End If

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtSchemeName.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Scheme")
                txtSchemeName.Focus()
                Exit Sub

            End If
            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.7.2.2)

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
            End If

            If lvwSearchDetails.Items.Count > 0 Then
                VB6.SetDefault(cmdFindNow, False)
                VB6.SetDefault(cmdOK, False)
                cmdEdit.Enabled = True
                cmdDelete.Enabled = True
            End If

            ' Set the focus.
            lvwSearchDetails.Focus()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Find Now command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFindNow_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

        ' Click event of the New Search button.

        Try

            ' Clear the interface details.
            m_lReturn = CType(ClearInterface(bConfirm:=True), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click

        Dim sPartyType As String = ""
        ' Click event of the New Button.

        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Process the Party Interface
            m_lReturn = CType(ProcessSchemeInterface(v_iTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)
            ' {* USER DEFINED CODE (End) *}

            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the New button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNew_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        Dim lRowID, lIndex, lCompanyNo, lSchemeNo, lSchemeVersion As Integer

        ' Click event of the Edit Button.

        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' Get the array position

            'Developer Guide No 234
            If Not Information.IsNothing(lvwSearchDetails.FocusedItem) Then
                lRowID = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)

                ' Get the index
                lIndex = lvwSearchDetails.FocusedItem.Index + 1
            Else
                lRowID = Convert.ToString(lvwSearchDetails.SelectedItems(0).Tag)

                ' Get the index
                lIndex = lvwSearchDetails.SelectedItems(0).Index + 1
            End If


            ' Get key values for the selected row id
            lCompanyNo = CInt(m_vSearchData(ACICompanyNo, lRowID))
            lSchemeNo = CInt(m_vSearchData(ACISchemeNo, lRowID))
            lSchemeVersion = CInt(m_vSearchData(ACISchemeVersion, lRowID))

            ' Process the Party Interface
            m_lReturn = CType(ProcessSchemeInterface(v_lCompanyNo:=lCompanyNo, v_lSchemeNo:=lSchemeNo, v_lSchemeVersion:=lSchemeVersion, v_iTask:=gPMConstants.PMEComponentAction.PMEdit, v_lIndex:=lIndex), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' {* USER DEFINED CODE (End) *}

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Edit button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Enter

        ' GotFocus Event for the search details

        Try

            ' Unset any default buttons so can
            VB6.SetDefault(cmdFindNow, False)
            VB6.SetDefault(cmdOK, False)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Leave

        ' LostFocus Event for the search details

        Try

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Click

        Dim sShortName As String = ""


        If lvwSearchDetails.Items.Count > 0 Then
            sShortName = lvwSearchDetails.FocusedItem.Text

            ' loop around and get the other details...
            For iCount As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                If CStr(m_vSearchData(ACIPartyCode, iCount)).Trim() = sShortName Then
                    txtPartyCode.Text = CStr(m_vSearchData(ACIPartyCode, iCount)).Trim()
                    txtPartyName.Text = CStr(m_vSearchData(ACIPartyName, iCount)).Trim()
                    '                txtAddress1.Text = Trim(m_vSearchData(ACIAddress1, iCount))
                    '                txtPostalCode.Text = Trim(m_vSearchData(ACIPostalCode, iCount))
                    '                txtTelephoneCode = Trim(m_vSearchData(ACITelAreaCode, iCount))
                    '                txtTelephone = Trim(m_vSearchData(ACITelNumber, iCount))
                    '                txtFileCode.Text = Trim(m_vSearchData(ACIFileCode, iCount))
                    '                txtDOB.Text = Trim(m_vSearchData(ACIDOB, iCount))
                    Exit For
                End If
            Next iCount

            VB6.SetDefault(cmdEdit, True)
            cmdEdit.Enabled = True
            cmdDelete.Enabled = True
        End If

    End Sub

    Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick

        ' Double click event for the search details.

        Try

            ' Check if there are any items available.
            If lvwSearchDetails.Items.Count = 0 Then
                Exit Sub
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            'DD120901
            cmdEdit_Click(cmdEdit, New EventArgs())
            'm_lReturn& = m_oGeneral.ProcessCommand()

            ' Check the return value.
            'If (m_lReturn& = PMTrue) Then
            ' Everything OK, so we can hide the interface.
            '    Me.Hide
            'End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwSearchDetails.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If KeyCode <> 13 Then
            VB6.SetDefault(cmdEdit, False)
        End If

    End Sub

    Private Sub lvwSearchDetails_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles lvwSearchDetails.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        Dim sShortName As String = ""

        If KeyAscii = 13 Then
            If lvwSearchDetails.Items.Count > 0 Then
                sShortName = lvwSearchDetails.FocusedItem.Text

                ' loop around and get the other details...
                For iCount As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                    If CStr(m_vSearchData(ACIPartyCode, iCount)).Trim() = sShortName Then
                        txtPartyCode.Text = CStr(m_vSearchData(ACIPartyCode, iCount)).Trim()
                        txtPartyName.Text = CStr(m_vSearchData(ACIPartyName, iCount)).Trim()
                        '                    txtAddress1.Text = Trim(m_vSearchData(ACIAddress1, iCount))
                        '                    txtPostalCode.Text = Trim(m_vSearchData(ACIPostalCode, iCount))
                        '                    txtTelephoneCode = Trim(m_vSearchData(ACITelAreaCode, iCount))
                        '                    txtTelephone = Trim(m_vSearchData(ACITelNumber, iCount))
                        Exit For
                    End If
                Next iCount

                cmdEdit_Click(cmdEdit, New EventArgs())

            End If
        End If

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDetails.Columns(eventArgs.Column)

        ' Column click event for the search details

        Try

            ListViewFunc.SortListView(lvwSearchDetails, eventArgs)
            'With lvwSearchDetails
            '    ' If current sort column header is
            '    ' pressed.
            '    If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwSearchDetails) Then
            '        ' Set sort order opposite of
            '        ' current direction.
            '        ListViewHelper.SetSortOrderProperty(lvwSearchDetails, (ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2)
            '    Else
            '        ' Sort by this column (ascending).
            '        ListViewHelper.SetSortedProperty(lvwSearchDetails, False)

            '        ' Turn off sorting so that the list
            '        ' is not sorted twice
            '        ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
            '        ListViewHelper.SetSortKeyProperty(lvwSearchDetails, ColumnHeader.Index + 1 - 1)
            '        ListViewHelper.SetSortedProperty(lvwSearchDetails, True)
            '    End If
            'End With

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub txtSchemeNo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSchemeNo.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtSchemeNo)

        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)

    End Sub

    Private Sub txtSchemeNo_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSchemeNo.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtSchemeName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSchemeName.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtSchemeName)

        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)

    End Sub

    Private Sub txtSchemeName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSchemeName.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtPartyCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPartyCode.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtPartyCode)

        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)

    End Sub

    Private Sub txtPartyCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPartyCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        If txtPartyCode.Text.Trim() <> "" Then
            txtPartyName.Text = ""
        End If

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Private Sub txtPartyName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPartyName.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtPartyName)

        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)

    End Sub

    Private Sub txtPartyName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPartyName.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub
    ' PRIVATE Events (End)
End Class
