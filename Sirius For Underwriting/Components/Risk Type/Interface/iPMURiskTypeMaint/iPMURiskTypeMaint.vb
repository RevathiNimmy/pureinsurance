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
    'Developer Guide No.243

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
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

    Private m_lRiskTypeID As Integer
    Private m_sCode As String = ""
    Private m_vDescription As String = ""
    Private m_dtRiskTypeEffectiveDate As Date
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMURiskTypeMaint.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the RiskType Interface object.

    Private m_oRiskType As iPMURiskType.Interface_Renamed

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    'stores all risktypes to be displayed on the listview
    Private m_vRiskTypeList(,) As Object

    Private m_iSortKey As Integer
    Private m_iDirection As SortOrder
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""
    Private Const ACDateColumn As Integer = 2

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


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFormatDate, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

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


            m_lReturn = m_oBusiness.GetAllRiskType(r_vResultArray:=m_vRiskTypeList)

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
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    '
    ' AMB 01-Oct-03: 1.8.6 Deferred Reinsurance development - bounds check added
    ' ***************************************************************** '
    Private Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim lLowBound, lUpBound As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the search details.
            Me.lvwRiskType.Items.Clear()

            ' Assign the details to the interface.
            If Information.IsArray(m_vRiskTypeList) Then

                lUpBound = m_vRiskTypeList.GetUpperBound(1)
                lLowBound = m_vRiskTypeList.GetLowerBound(1)

                For lRow As Integer = lLowBound To lUpBound
                    ' Assign the details to listview control

                    'Column 1 Code

                    'Developer Guide No.49
                    oListItem = Me.lvwRiskType.Items.Add(CStr(m_vRiskTypeList(ACICode, lRow)), "RiskType")

                    'Column 2 Description
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vRiskTypeList(ACIDescription, lRow))

                    'Column 3 Effective Date
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFormatDate, vControlValue:=m_vRiskTypeList(ACIRiskTypeEffectiveDate, lRow))

                    ' Check for errors
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to assign the data.
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtFormatDate.Text
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CDate(txtFormatDate.Text).ToString("dd/MM/yyyy")

                    oListItem.Tag = CStr(lRow)

                    'ghosted deleted records
                    If m_vRiskTypeList(4, lRow) = gPMConstants.PMEReturnCode.PMTrue Then

                        'Developer Guide No.(changed code as per work functionality)
                        oListItem.ForeColor = Color.Gray
                    Else
                        oListItem.ForeColor = Color.Black
                    End If

                    ' Refresh the first X amount of rows, to
                    ' allow the user to see the results instantly.
                    If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                        ' Select the first item.
                        Me.lvwRiskType.Items.Item(0).Selected = True

                        ' Refresh the initial results.
                        Me.lvwRiskType.Refresh()
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
                        If Not Information.IsArray(m_vRiskTypeList) Then
                            ReDim m_vRiskTypeList(4, 0)
                            lSelectedItem = 0
                        Else
                            lSelectedItem = m_vRiskTypeList.GetUpperBound(1) + 1
                            ReDim Preserve m_vRiskTypeList(4, lSelectedItem)
                        End If
                    Else


                        lSelectedItem = Convert.ToString(Me.lvwRiskType.Items.Item(Me.lvwRiskType.SelectedItems(0).Index).Tag)
                    End If

                    m_vRiskTypeList(ACIRiskTypeID, lSelectedItem) = m_lRiskTypeID
                    m_vRiskTypeList(ACICode, lSelectedItem) = m_sCode
                    m_vRiskTypeList(ACIDescription, lSelectedItem) = m_vDescription
                    m_vRiskTypeList(ACIRiskTypeEffectiveDate, lSelectedItem) = m_dtRiskTypeEffectiveDate
                    m_vRiskTypeList(ACIIsDeleted, lSelectedItem) = gPMConstants.PMEReturnCode.PMFalse

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

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

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

            ' AMB 30/05/2003: 1.8.6 Deferred RI RFC - add captions

            cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMaintab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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

        m_iAction = gPMConstants.PMEComponentAction.PMAdd
        cmdOK.Enabled = False
        cmdCancel.Enabled = False

        'Create Risk Type object if not already done so
        If m_oRiskType Is Nothing Then

            ' Get an instance of the Risk Type interface object via
            ' the public object manager.
            Dim temp_m_oRiskType As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oRiskType, sClassName:="iPMURiskType.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oRiskType = temp_m_oRiskType

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Risk Type object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub

            End If

        End If

        m_lReturn = CType(m_oRiskType.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vRiskTypeList:=m_vRiskTypeList), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        m_sUniqueId = GetUniqueID()

        m_oRiskType.UniqueId = m_sUniqueId
        m_lReturn = m_oRiskType.Start()

        cmdOK.Enabled = True
        cmdCancel.Enabled = True

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        Me.Refresh()

        'If not cancelled, add to grid

        If m_oRiskType.Status = gPMConstants.PMEReturnCode.PMCancel Then
            Exit Sub
        End If


        'get data back from SPR object

        m_lRiskTypeID = m_oRiskType.RiskTypeID

        m_sCode = m_oRiskType.Code

        m_vDescription = m_oRiskType.Description

        m_dtRiskTypeEffectiveDate = m_oRiskType.RiskTypeEffectiveDate


        m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)
        Exit Sub

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click


        m_iAction = gPMConstants.PMEComponentAction.PMDelete
        Dim iDelete As Integer = gPMConstants.PMEReturnCode.PMTrue


        Dim lSelectedItem As Integer = Convert.ToString(Me.lvwRiskType.Items.Item(Me.lvwRiskType.SelectedItems(0).Index).Tag)



        'If Me.lvwRiskType.SelectedItem.Ghosted Then
        'If Me.lvwRiskType.FocusedItem.ForeColor.Equals(Color.Gray) Then
        If Me.lvwRiskType.SelectedItems(0).ForeColor.Equals(Color.Gray) Then
            iDelete = gPMConstants.PMEReturnCode.PMFalse
        End If

        m_sUniqueId = GetUniqueID()

        m_sScreenHierarchy = $"Risk Type({CStr(m_vRiskTypeList(ACICode, lSelectedItem)).Trim()})"
        m_lReturn = m_oBusiness.DelRiskType(m_vRiskTypeList(ACIRiskTypeID, lSelectedItem), iDelete, m_sUniqueId, m_sScreenHierarchy)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_vRiskTypeList(ACIIsDeleted, lSelectedItem) = iDelete
            m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)
        End If

        cmdEdit.Enabled = False
        cmdDelete.Enabled = False
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True
        cmdOK.Enabled = True

    End Sub


    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click


        m_iAction = gPMConstants.PMEComponentAction.PMEdit
        cmdOK.Enabled = False
        cmdCancel.Enabled = False


        'Dim lSelectedItem As Integer = Convert.ToString(Me.lvwRiskType.Items.Item(Me.lvwRiskType.FocusedItem.Index).Tag)
        Dim lSelectedItem As Integer = Convert.ToString(Me.lvwRiskType.Items.Item(Me.lvwRiskType.SelectedItems(0).Index).Tag)

        'Create Risk Type object if not already done so
        If m_oRiskType Is Nothing Then

            ' Get an instance of the Risk Type interface object via
            ' the public object manager.
            Dim temp_m_oRiskType As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oRiskType, sClassName:="iPMURiskType.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oRiskType = temp_m_oRiskType

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Risk Type object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub

            End If

        End If

        m_lReturn = CType(m_oRiskType.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vRiskTypeList:=m_vRiskTypeList), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        'pass selected details to Risk Type object

        m_oRiskType.RiskTypeID = CInt(m_vRiskTypeList(ACIRiskTypeID, lSelectedItem))

        m_sUniqueId = GetUniqueID()

        m_oRiskType.UniqueId = m_sUniqueId
        m_lReturn = m_oRiskType.Start()

        cmdOK.Enabled = True
        cmdCancel.Enabled = True

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        Me.Refresh()

        'get data back from risktype object

        m_lRiskTypeID = m_oRiskType.RiskTypeID

        m_sCode = m_oRiskType.Code

        m_vDescription = m_oRiskType.Description

        m_dtRiskTypeEffectiveDate = m_oRiskType.RiskTypeEffectiveDate

        'Update the existing item
        m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to update the data.
            Exit Sub
        End If


    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 184

        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, lContextID:=MainModule.ScreenHelpID), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

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

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRiskType.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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
            m_oGeneral = New iPMURiskTypeMaint.General()

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

            m_iSortKey = -1

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


            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.cancel = True
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
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMaintab.SelectedIndex = 0
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub lvwRiskType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRiskType.Click
        If Not (lvwRiskType.FocusedItem Is Nothing) Then


            'If Me.lvwRiskType.SelectedItem.Ghosted Then
            If Me.lvwRiskType.FocusedItem.ForeColor.Equals(Color.Gray) Then

                cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACUndeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                cmdEdit.Enabled = False
            Else


                cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
        End If
    End Sub

    Private Sub lvwRiskType_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwRiskType.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwRiskType.Columns(eventArgs.Column)


        ' Column click event for the search details

        Try

            With lvwRiskType

                If ColumnHeader.Index + 1 - 1 = ACDateColumn Then

                    If m_iSortKey <> ACDateColumn Then
                        m_iSortKey = ACDateColumn
                        m_iDirection = SortOrder.Ascending
                    Else
                        m_iDirection = (m_iDirection + 1) Mod 2
                    End If

                    m_lReturn = CType(ListViewSortByDate(v_oListView:=lvwRiskType, v_iSourceColumn:=ACDateColumn, v_iDirection:=m_iDirection), gPMConstants.PMEReturnCode)
                    ' If current sort column header is
                    ' pressed.
                ElseIf (ColumnHeader.Index + 1 - 1 = m_iSortKey) Then
                    ' Set sort order opposite of
                    ' current direction.
                    m_iDirection = (m_iDirection + 1) Mod 2
                    ListViewHelper.SetSortKeyProperty(lvwRiskType, m_iSortKey)
                    ListViewHelper.SetSortOrderProperty(lvwRiskType, m_iDirection)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwRiskType, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwRiskType, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwRiskType, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwRiskType, True)
                    m_iDirection = ListViewHelper.GetSortOrderProperty(lvwRiskType)
                    m_iSortKey = ListViewHelper.GetSortKeyProperty(lvwRiskType)
                End If
            End With

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwRiskType_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: ListViewSortByDate
    '
    ' Description: Sorts the list view based on the column passed, and
    '              the order given.
    '
    ' Note : This hasn't been tested on the first column. I suspect
    '        changes might need to be made if sorting on the first
    '        column is needed (CF 060899).
    '
    ' ***************************************************************** '
    Private Function ListViewSortByDate(ByVal v_oListView As ListView, ByVal v_iSourceColumn As Integer, ByVal v_iDirection As SortOrder) As Integer

        Dim result As Integer = 0
        Dim sDate As String = ""
        Dim iIndex As Integer
        Const ACLVTag As String = "SORT_DATE_HIDDEN"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the column
            v_oListView.Columns.Add(ACLVTag, "Shhh Im hidden", CInt(VB6.TwipsToPixelsX(0)))

            ' Get the index of this new column, -1 because it's a sub item
            iIndex = v_oListView.Columns.Count - 1

            ' Not sorted yet
            ListViewHelper.SetSortedProperty(v_oListView, False)

            ' Add the items
            For iLoop1 As Integer = 1 To v_oListView.Items.Count

                If v_iSourceColumn = 0 Then
                    sDate = CDate(v_oListView.Items.Item(iLoop1 - 1).Text).ToString("yyyyMMddHHMMss")
                Else
                    Dim TempDate As Date
                    sDate = IIf(DateTime.TryParse(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(iLoop1 - 1), v_iSourceColumn).Text, TempDate), TempDate.ToString("yyyyMMddHHMMss"), ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(iLoop1 - 1), v_iSourceColumn).Text)
                End If
                ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(iLoop1 - 1), iIndex).Text = sDate

            Next iLoop1

            ' Sort now
            ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)

            ' Set the sort key
            ListViewHelper.SetSortKeyProperty(v_oListView, iIndex)

            ListViewHelper.SetSortedProperty(v_oListView, True)

            ' Remove the column now
            v_oListView.Columns.RemoveAt(iIndex)

            ' Reset the sort key
            'eck 010800 This resorts listview so remove it.
            '    v_oListView.SortKey = v_iSourceColumn%

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewSortByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewSortByDate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Sub lvwRiskType_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwRiskType.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        'Not if we're viewing, thank you very much
        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If Me.lvwRiskType.GetItemAt(x, y) Is Nothing Then
                cmdDelete.Enabled = False
                cmdAdd.Enabled = True
                cmdEdit.Enabled = False
            Else
                cmdEdit.Enabled = True
                cmdDelete.Enabled = True
            End If
        End If
    End Sub
End Class
