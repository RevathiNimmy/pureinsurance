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
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 26/07/2000
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const vbFormCode As Integer = 0
    Private Const ACClass As String = "frmInterface"

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

    ' {* USER DEFINED CODE (Begin) *}
    Private m_iAction As gPMConstants.PMEComponentAction

    Private m_lLookupKey As Integer
    Private m_lLineKey As Integer
    Private m_sKeyLevel As String = ""
    Private m_sValue As String = ""
    Private m_lType As Integer
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMURuleLookupData.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the RiskType Interface object.
    Private m_oRiskType As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    'stores all lookup headers to be displayed on the listview
    Private m_vLookupData(,) As Object

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


    ' {* USER DEFINED CODE (End) *}

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)

    'Private Sub Status(ByVal Value As Integer)
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
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

    ' {* USER DEFINED CODE (Begin) *}

    Public WriteOnly Property LookupKey() As Integer
        Set(ByVal Value As Integer)
            m_lLookupKey = Value
        End Set
    End Property
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Private Function SetFieldValidation() As Integer

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
            ''                       lMandatory:=<PMNonMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLineKey, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtKeyLevel, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtValue, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetList
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetList() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'disable the OK button
            Me.cmdOK.Enabled = False

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oBusiness.GetAllLookupData(v_lLookupKey:=m_lLookupKey, r_vResultArray:=m_vLookupData)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetList")

                    Return result
            End Select


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details from the interface to the data storage.
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update the business object.

            m_lReturn = m_oBusiness.UpdateLookupData(v_iTask:=m_iAction, v_lLookupKey:=m_lLookupKey, v_lLineKey:=m_lLineKey, v_vKeyLevel:=m_sKeyLevel, v_vValue:=m_sValue, v_vType:=m_lType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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



            m_lLineKey = CInt(m_oFormFields.UnformatControl(ctlControl:=txtLineKey))

            m_sKeyLevel = CStr(m_oFormFields.UnformatControl(ctlControl:=txtKeyLevel))

            m_sValue = CStr(m_oFormFields.UnformatControl(ctlControl:=txtValue))
            m_lType = VB6.GetItemData(cboType, cboType.SelectedIndex)

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
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    '
    ' ***************************************************************** '
    Private Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Modified,save the selected item
            Dim obj As Integer
            If Not IsNothing(Me.lvwLookupData.FocusedItem) Then
                obj = Me.lvwLookupData.FocusedItem.Index
            End If
            ' Clear the search details.
            Me.lvwLookupData.Items.Clear()

            ' Assign the details to the interface.
            If Information.IsArray(m_vLookupData) Then

                For lRow As Integer = m_vLookupData.GetLowerBound(1) To m_vLookupData.GetUpperBound(1)

                    ' Assign the details to listview control
                    'Column 1 Line key

                    oListItem = Me.lvwLookupData.Items.Add(CStr(m_vLookupData(ACFLineKey, lRow)), "LookupData")

                    'Column 2 key level
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vLookupData(ACFKeyLevel, lRow))

                    'Column 3 Value
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vLookupData(ACFValue, lRow))

                    'Column 4 type
                    For lCount As Integer = 0 To cboType.Items.Count - 1
                        cboType.SelectedIndex = lCount
                        If VB6.GetItemData(cboType, lCount) = CDbl(m_vLookupData(ACFType, lRow)) Then
                            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = cboType.Text
                        End If
                    Next lCount

                    oListItem.Tag = CStr(lRow)

                    '            'ghosted deleted records
                    '            If (m_vLookupData(ACFStatus, lRow&) = PMFalse) Then
                    '                Me.lvwLookupData.ListItems(lRow& + 1).Selected = True
                    '                Me.lvwLookupData.SelectedItem.Ghosted = True
                    '            End If

                    ' Refresh the first X amount of rows, to
                    ' allow the user to see the results instantly.
                    If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                        ' Select the first item.
                        Me.lvwLookupData.Items.Item(0).Selected = True

                        ' Refresh the initial results.
                        Me.lvwLookupData.Refresh()
                    End If

                Next lRow
            End If

            'enable the OK button
            Me.cmdOK.Enabled = True

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            'Modified,use the selected item
            If tabLookupDetail.Visible = True Then
                If IsNothing(Me.lvwLookupData.FocusedItem) Then
                    Me.lvwLookupData.FocusedItem = Me.lvwLookupData.Items(obj)
                End If
            End If
            'end
            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            '    Resume

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataRefresh
    '
    ' Description: Populate Refresh storage
    '
    ' ***************************************************************** '
    Private Function DataRefresh() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get new details from baby form (SPR)

            ' {* USER DEFINED CODE (Begin) *}
            Select Case m_iAction
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit
                    If m_iAction = gPMConstants.PMEComponentAction.PMAdd Then
                        If Not Information.IsArray(m_vLookupData) Then
                            ReDim m_vLookupData(ACFType, 0)
                            lSelectedItem = 0
                        Else
                            lSelectedItem = m_vLookupData.GetUpperBound(1) + 1
                            ReDim Preserve m_vLookupData(ACFType, lSelectedItem)
                        End If
                    Else


                        lSelectedItem = Convert.ToString(lvwLookupData.Items.Item(lvwLookupData.FocusedItem.Index).Tag)
                    End If

                    m_vLookupData(ACFLookupKey, lSelectedItem) = m_lLookupKey
                    m_vLookupData(ACFLineKey, lSelectedItem) = m_lLineKey
                    m_vLookupData(ACFKeyLevel, lSelectedItem) = m_sKeyLevel
                    m_vLookupData(ACFValue, lSelectedItem) = m_sValue
                    m_vLookupData(ACFType, lSelectedItem) = m_lType
            End Select

            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Refresh from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataRefresh", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'Public Methods (End)

    'Private Methods (Begin)
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

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
            cmdAdd.Enabled = True

            ' {* USER DEFINED CODE (Begin) *}
            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=Me.lvwLookupData.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'hide header detail tab and line it up with main tab
            tabLookupDetail.Visible = False
            tabLookupDetail.Left = tabMaintab.Left
            tabLookupDetail.Top = tabMaintab.Top

            'set default details for type combo
            Dim cboType_NewIndex As Integer = -1
            cboType_NewIndex = cboType.Items.Add("Finish")
            VB6.SetItemData(cboType, cboType_NewIndex, 0)
            cboType_NewIndex = cboType.Items.Add("Step")
            VB6.SetItemData(cboType, cboType_NewIndex, 1)
            cboType_NewIndex = cboType.Items.Add("Gradient")
            VB6.SetItemData(cboType, cboType_NewIndex, 2)
            cboType_NewIndex = cboType.Items.Add("Constant")
            VB6.SetItemData(cboType, cboType_NewIndex, 3)

            cboType.SelectedIndex = 0

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

            'Dim m_ctlTabFirstLast(1, 0) As Object
            Dim m_ctlTabFirstLast(,) As Object
            ReDim m_ctlTabFirstLast(1, 0)

            m_ctlTabFirstLast(ACControlStart, 0) = txtLineKey
            m_ctlTabFirstLast(ACControlEnd, 0) = cboType


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



            SSTabHelper.SetTabCaption(tabMaintab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabLookupDetail, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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


            lblLineKey.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACILineKey, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblKeyLevel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACIKeyLevel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblValue.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACIValue, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACIType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        Dim lLineKey As Integer

        m_iAction = gPMConstants.PMEComponentAction.PMAdd

        'default type if its not first one
        If lvwLookupData.Items.Count > 0 Then
            For lCount As Integer = 0 To cboType.Items.Count - 1
                cboType.SelectedIndex = lCount
                If CDbl(m_vLookupData(ACFType, 0)) = VB6.GetItemData(cboType, lCount) Then
                    Exit For
                End If
            Next
        Else
            cboType.SelectedIndex = 0
        End If

        'get next available line key

        If m_oBusiness.GetNextLineKey(v_lLookupKey:=m_lLookupKey, r_lLineKey:=lLineKey) <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to get next available line key", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtLineKey, vControlValue:=lLineKey)

        ' Check for errors
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to assign the data.
            MessageBox.Show("Failed to format line key", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        'hide main tab and show detail tab
        tabMaintab.Visible = False
        tabLookupDetail.Visible = True

        txtKeyLevel.Focus()
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click


        m_iAction = gPMConstants.PMEComponentAction.PMDelete


        Dim lSelectedItem As Integer = Convert.ToString(lvwLookupData.Items.Item(lvwLookupData.FocusedItem.Index).Tag)


        m_lReturn = m_oBusiness.DeleteLookupData(v_lLookupKey:=m_vLookupData(ACFLookupKey, lSelectedItem), v_lLineKey:=m_vLookupData(ACFLineKey, lSelectedItem))

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            'set status to pmfalse so it won't get display
            m_lReturn = CType(GetList(), gPMConstants.PMEReturnCode)
        End If

        cmdEdit.Enabled = False
        cmdDelete.Enabled = False
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True
        cmdOK.Enabled = True

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click


        m_iAction = gPMConstants.PMEComponentAction.PMEdit


        Dim lSelectedItem As Integer = Convert.ToString(lvwLookupData.Items.Item(lvwLookupData.FocusedItem.Index).Tag)

        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtLineKey, vControlValue:=m_vLookupData(ACFLineKey, lSelectedItem))

        ' Check for errors
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to assign the data.
            MessageBox.Show("Failed to format line key", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtKeyLevel, vControlValue:=m_vLookupData(ACFKeyLevel, lSelectedItem))

        ' Check for errors
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to assign the data.
            MessageBox.Show("Failed to format effective date", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtValue, vControlValue:=m_vLookupData(ACFValue, lSelectedItem))

        ' Check for errors
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to assign the data.
            MessageBox.Show("Failed to format lookup definition", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        For lCount As Integer = 0 To cboType.Items.Count - 1
            cboType.SelectedIndex = lCount
            If VB6.GetItemData(cboType, lCount) = CDbl(m_vLookupData(ACFType, lSelectedItem)) Then
                Exit For
            End If
        Next

        'hide main tab and show detail tab
        tabMaintab.Visible = False
        tabLookupDetail.Visible = True

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID), gPMConstants.PMEReturnCode)


    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            If tabLookupDetail.Visible Then
                ' Check mandatory controls have been entered into.
                m_lReturn = m_oFormFields.CheckMandatoryControls()

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                'm_lReturn = ValidateForm()

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                'redisplay data on listview
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)

                    'hide detail tab and show main tab
                    tabMaintab.Visible = True
                    tabLookupDetail.Visible = False

                End If

            Else
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            If Not tabLookupDetail.Visible Then
                ' Check the return value.
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If
            Else
                tabMaintab.Visible = True
                tabLookupDetail.Visible = False
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRuleLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

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

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMURuleLookupData.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

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

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            ' Validate fields using Forms Control
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

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

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            'Developer Guide No. 19 (No Solution)
            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
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

            If Not (m_oRiskType Is Nothing) Then
                ' Terminate the business object

                m_oRiskType.Dispose()
                m_oRiskType = Nothing

            End If

            ' Terminate the business object

            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing


            ' Terminate the form control object.
            m_oFormFields.Dispose()
            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

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

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMaintab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMaintab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMaintab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabMaintab, SSTabHelper.GetSelectedIndex(tabMaintab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMaintab, SSTabHelper.GetTabCount(tabMaintab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMaintab) < (SSTabHelper.GetTabCount(tabMaintab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabMaintab, SSTabHelper.GetSelectedIndex(tabMaintab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            '                    If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
                            '                         m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
                            '                    End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            '                    If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
                            '                         m_ctlTabFirstLast(ACControlEnd, .Tab).SetFocus
                            '                    End If
                        End If
                End Select
            End With

            If tabMaintab.Visible Then

                'Developer Guide No.293

                If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                    tabMaintab.SelectedIndex = 0
                End If
            End If

            If tabLookupDetail.Visible Then

                'Developer Guide No.293

                If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                    tabLookupDetail.SelectedIndex = 0
                End If
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub lvwLookupData_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwLookupData.Click
        'CMG/PB 16082002 Bug 396 fix error if no items in list when clicked
        If lvwLookupData.Items.Count > 0 Then

        End If

    End Sub

    Private Sub lvwLookupData_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwLookupData.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.70
        Dim x As Single = eventArgs.X
        'Developer Guide No.70
        Dim y As Single = eventArgs.Y
        'Not if we're viewing, thank you very much
        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If lvwLookupData.GetItemAt(x, y) Is Nothing Then
                cmdDelete.Enabled = False
                cmdAdd.Enabled = True
                cmdEdit.Enabled = False
            Else
                cmdEdit.Enabled = True
                cmdDelete.Enabled = True
            End If
        End If
    End Sub

    Private Sub txtLineKey_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLineKey.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtLineKey)
    End Sub

    Private Sub txtLineKey_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLineKey.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtLineKey)
    End Sub

    Private Sub txtKeyLevel_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtKeyLevel.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtKeyLevel)
    End Sub

    Private Sub txtKeyLevel_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtKeyLevel.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtKeyLevel)
    End Sub

    Private Sub txtValue_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtValue.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtValue)
    End Sub

    Private Sub txtValue_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtValue.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtValue)
    End Sub

    Private Sub cboType_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboType.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=cboType)
    End Sub

    Private Sub cboType_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboType.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=cboType)
    End Sub

    Private Sub tabMaintab_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabMaintab.KeyDown
        If tabMaintab.Visible Then

            'Developer Guide No.293

            If e.Alt And e.KeyCode = Keys.D1 Then
                tabMaintab.SelectedIndex = 0
                tabMaintab.Focus()
            End If
        End If

        If tabLookupDetail.Visible Then

            'Developer Guide No.293

            If e.Alt And e.KeyCode = Keys.D1 Then
                tabLookupDetail.SelectedIndex = 0
                tabMaintab.Focus()
            End If
        End If
    End Sub
End Class
