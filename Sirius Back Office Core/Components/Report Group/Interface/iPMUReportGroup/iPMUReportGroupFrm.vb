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
    ' Date: 09/06/1999
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
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
    Private m_lReportGroupId As Integer
    Private m_sReportGroupCode As String = "" 'Report_Group.code
    Private m_sReportGroupDescription As String = "" 'Report_Group.description
    Private m_lItemsFound As Integer

    'Variables to store data taken from the List View
    Private m_iAction As Integer
    Private m_lReportId As Integer
    Private m_sReportCode As String = ""
    Private m_sReportDescription As String = ""
    Private m_sReportName As String = ""
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUReportGroup.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    'Private m_oBusiness As bSIRRIModelLine.Business

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    ' Stores the search data from the business object.
    Private m_vAllReport(,) As Object
    Private m_vReportGroup(,) As Object
    Private m_vAllPMUserGroup(,) As Object
    Private m_vReportGroupUserGroup(,) As Object
    Private m_sUniqueId As String = ""
    Private m_SScreenHierarchy As String = ""


    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
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
    ' {* USER DEFINED CODE (Begin) *}
    Public WriteOnly Property ReportGroupID() As Integer
        Set(ByVal Value As Integer)
            m_lReportGroupId = Value
        End Set
    End Property

    Public WriteOnly Property ReportGroupDescription() As String
        Set(ByVal Value As String)
            m_sReportGroupDescription = Value
        End Set
    End Property

    Public WriteOnly Property ReportGroupCode() As String
        Set(ByVal Value As String)
            m_sReportGroupCode = Value
        End Set
    End Property

    Public WriteOnly Property UniqueId() As String
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public WriteOnly Property ScreenHierarchy() As String
        Set(ByVal Value As String)
            m_SScreenHierarchy = Value
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


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    '' ***************************************************************** '
    '' Name: DataToDetail
    ''
    '' Description: Populate ? Details
    ''              storage.
    ''
    '' ***************************************************************** '
    'Private Function DataToDetail() As Long
    '
    'Dim lSelectedItem As Long
    '
    '    On Error GoTo Err_DataToDetail
    '
    '    DataToDetail = PMTrue
    '
    '    ' Update the Detail details.
    '
    '    lSelectedItem& = _
    ''        lvwReportGroupContents.ListItems(lvwReportGroupContents.SelectedItem.Index).Tag
    '
    '    ' Update the property members.
    '
    '    ' {* USER DEFINED CODE (Begin) *}
    '
    '    m_lReportGroupId = m_vReportGroup(ACRReportId, lSelectedItem)
    '    m_lReportId = m_vAllReport(ACRReportId, lSelectedItem)
    '    m_sReportCode = m_vAllReport(ACRReportCode, lSelectedItem)
    '    m_sReportDescription = m_vAllReport(ACRReportDescription, lSelectedItem)
    '    m_sReportName = m_vAllReport(ACRReportName, lSelectedItem)
    '
    '    Exit Function
    '
    'Err_DataToDetail:
    '
    '    DataToDetail = PMError
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to update the Detail details from the search data storage", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="DataToDetail", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '    Resume
    '
    'End Function
    '
    '' ***************************************************************** '
    '' Name: ClearDetail
    ''
    '' Description: Clear ? Details
    ''              storage.
    ''
    '' ***************************************************************** '
    'Private Function ClearDetail() As Long
    '
    'Dim lSelectedItem As Long
    '
    '    On Error GoTo Err_ClearDetail
    '
    '    ClearDetail = PMTrue
    '
    '    ' Update the Detail details.
    '
    '    ' {* USER DEFINED CODE (Begin) *}
    '    m_lReportGroupId = 0
    '    m_lReportId = 0
    '    m_sReportCode = ""
    '    m_sReportName = ""
    '    m_sReportDescription = ""
    '
    '    Exit Function
    '
    'Err_ClearDetail:
    '
    '    ClearDetail = PMError
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to update the Detail details from the search data storage", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="ClearDetail", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '    Resume
    '
    'End Function

    '' ***************************************************************** '
    '' Name: DataRefresh
    ''
    '' Description: Populate ? Refreshs
    ''              storage.
    ''
    '' ***************************************************************** '
    'Private Function DataRefresh() As Long
    '
    'Dim lSelectedItem As Long
    'Dim lReportGroupID As Long
    '
    '    On Error GoTo Err_DataRefresh
    '
    '    DataRefresh = PMTrue
    '
    '    ' Update the property members.
    '
    '    ' {* USER DEFINED CODE (Begin) *}
    '
    '    Select Case m_iAction
    '    Case PMAdd
    '        If IsArray(m_vAllReport) Then
    '            lSelectedItem = UBound(m_vAllReport, 2) + 1
    '            ReDim Preserve m_vAllReport(ACRReportId, lSelectedItem)
    '        Else
    '            lSelectedItem = 0
    '            ReDim m_vAllReport(ACRReportId, lSelectedItem)
    '        End If
    '
    '        lReportID = m_lReportId
    '
    '    Case PMEdit
    '        lSelectedItem& = _
    ''            lvwReportGroupContents.ListItems(lvwReportGroupContents.SelectedItem.Index).Tag
    '
    '        lReportID = m_lReportId
    '
    '    Case PMDelete
    '        lSelectedItem& = _
    ''            lvwReportGroupContents.ListItems(lvwReportGroupContents.SelectedItem.Index).Tag
    '
    '        lReportID = 0
    '
    '    End Select
    '
    '    m_vAllReport(ACRReportId, lSelectedItem&) = m_lReportId
    '    m_vAllReport(ACRReportCode, lSelectedItem&) = m_sReportCode
    '    m_vAllReport(ACRReportDescription, lSelectedItem&) = m_sReportDescription
    '    m_vAllReport(ACRReportName, lSelectedItem&) = m_sReportName
    '
    '    m_lReturn = BusinessToInterface()
    '
    '    Exit Function
    '
    'Err_DataRefresh:
    '
    '    DataRefresh = PMError
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to update the Refresh Refreshs from the search data storage", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="DataRefresh", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

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

            ' {* USER DEFINED CODE (Begin) *}

            'Tell it what we're getting

            m_oBusiness.ReportGroupID = m_lReportGroupId

            'Get the full list of Reports

            m_lReturn = m_oBusiness.GetAllReport(r_vAllReport:=m_vAllReport)

            'Get list of report_id linked to the current group

            m_lReturn = m_oBusiness.GetReportGroupContents(r_vReportGroup:=m_vReportGroup)

            'Get the full list of PMUser Groups

            m_lReturn = m_oBusiness.GetAllPMUserGroup(r_vAllPMUserGroup:=m_vAllPMUserGroup)

            'Get list of pmuser_group_id linked to the current group

            m_lReturn = m_oBusiness.GetReportGroupUserGroups(r_vReportGroupUserGroup:=m_vReportGroupUserGroup)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details to the interface.
            ' ************************************************************
            ' Enter your code here to assign the all of the interface
            ' details from the business object, using the FormatField
            ' function for any type conversion.
            '
            ' Example:-
            '
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            'Developer Guide No. 26
            pnlLblRG(SSTabHelper.GetSelectedIndex(tabMainTab)).Text = m_sReportGroupCode

            'Get details for Report Group Contents
            lvwReportGroupContents.Items.Clear()

            If Information.IsArray(m_vAllReport) Then
                For lTemp As Integer = m_vAllReport.GetLowerBound(1) To m_vAllReport.GetUpperBound(1)

                    If CDbl(m_vAllReport(ACRReportId, lTemp)) <> 0 Then
                        oListItem = lvwReportGroupContents.Items.Add(CStr(m_vAllReport(ACRReportCode, lTemp)).Trim())

                        'Column 2 Name
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vAllReport(ACRReportName, lTemp))

                        'Column 3 Description
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vAllReport(ACRReportDescription, lTemp))

                        oListItem.Tag = CStr(lTemp)
                    End If
                Next lTemp

            End If


            'Get details for Report Group User Groups
            lvwReportGroupUserGroups.Items.Clear()

            If Information.IsArray(m_vAllPMUserGroup) Then
                For lTemp As Integer = m_vAllPMUserGroup.GetLowerBound(1) To m_vAllPMUserGroup.GetUpperBound(1)

                    If CDbl(m_vAllPMUserGroup(ACRPMUserGroupId, lTemp)) <> 0 Then
                        oListItem = lvwReportGroupUserGroups.Items.Add(CStr(m_vAllPMUserGroup(ACRPMUserGroupCode, lTemp)).Trim())

                        'Column 2 Description
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vAllPMUserGroup(ACRPMUserGroupDescription, lTemp))

                        oListItem.Tag = CStr(lTemp)
                    End If
                Next lTemp

            End If

            ' Assign the details from the business object
            ' to the data storage.
            ' in this case, find out and mark the reports in both lists
            m_lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set the Select/Unselect Button Caption

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function InterfaceToBusiness(Optional ByVal sUniqueId As String = "") As Integer

        'Dim lBusinessDataID As Long
        'Dim bDifferent As Boolean
        'Dim sDescription As String
        'Dim vDescription As Variant

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update Report Group Contents

            m_oBusiness.ReportGroupID = m_lReportGroupId

            If m_sUniqueId <> sUniqueId AndAlso Not String.IsNullOrEmpty(sUniqueId) Then
                m_sUniqueId = sUniqueId
            End If
            m_lReturn = m_oBusiness.UpdateReportGroupContents(v_vReportGroup:=m_vReportGroup, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_SScreenHierarchy)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            'update Report Group User Groups

            m_lReturn = m_oBusiness.UpdateReportGroupUserGroups(v_vReportGroupUserGroup:=m_vReportGroupUserGroup, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_SScreenHierarchy)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If
            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}
            'get tag linked report group
            If TagReportGroup() <> gPMConstants.PMEReturnCode.PMTrue Then
                '        If (m_lReturn& <> PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
                '        End If
            End If

            'get tag linked report group user groups
            If TagReportGroupUserGroups() <> gPMConstants.PMEReturnCode.PMTrue Then
                '        If (m_lReturn& <> PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
                '        End If
            End If
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' {* USER DEFINED CODE (Begin) *}
            result = UnloadReportGroup()
            ' {* USER DEFINED CODE (End) *}

            Return UnloadReportGroupUserGroup()

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)
            'Display the ListView Tab
            'default to first tab
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            'tabMainTab.TabVisible(1) = True    'until the user group work is ready
            tabMainTab.Top = VB6.TwipsToPixelsY(120)
            cmdApply.Enabled = False

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwReportGroupContents.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwReportGroupUserGroups.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the column widths for the list on tab 0.
            lvwReportGroupContents.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1500))
            lvwReportGroupContents.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(2200))
            lvwReportGroupContents.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(5000))

            ' Set the column widths for the list on tab 1.
            lvwReportGroupUserGroups.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1500))
            lvwReportGroupUserGroups.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(7000))

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


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************



            'lblReportGroup(SSTabHelper.GetSelectedIndex(tabMainTab)).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCReportGroupID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            pnlReportGroup(SSTabHelper.GetSelectedIndex(tabMainTab)).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCReportGroupID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwReportGroupContents.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwReportGroupContents.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHReportName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwReportGroupContents.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwReportGroupUserGroups.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwReportGroupUserGroups.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
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
    '
    ' Name: TagReportGroup
    '
    ' Description: loop through and put a tick for each record
    '              in m_vReportGroup on the ListView
    ' ***************************************************************** '
    Private Function TagReportGroup() As Integer

        Dim result As Integer = 0
        Dim lPos As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vReportGroup) Then
                For lCount As Integer = m_vReportGroup.GetLowerBound(1) To m_vReportGroup.GetUpperBound(1)
                    lPos = IsInArray(CInt(m_vReportGroup(0, lCount)), m_vAllReport)

                    If lPos <> -1 Then
                        'Me.lvwReportGroupContents.Items.Item(Me.lvwReportGroupContents.FocusedItem.Index).Tag = Convert.ToString(Me.lvwReportGroupContents.Items.Item(Me.lvwReportGroupContents.FocusedItem.Index).Tag) & "checked"
                        Me.lvwReportGroupContents.Items(lPos).Tag = Convert.ToString(Me.lvwReportGroupContents.Items(lPos).Tag) & "checked"
                        'Developer Guide No. 49
                        Me.lvwReportGroupContents.Items(lPos).ImageIndex = 0
                        Me.lvwReportGroupContents.Items(lPos).Selected = True
                        Me.lvwReportGroupContents.Select()
                    End If
                Next
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TagReportGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TagReportGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: TagReportGroupUserGroups
    '
    ' Description: loop through and put a tick for each record
    '              in m_vReportGroupUserGroup on the ListView
    ' ***************************************************************** '
    Private Function TagReportGroupUserGroups() As Integer

        Dim result As Integer = 0
        Dim lPos As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vReportGroupUserGroup) Then
                For lCount As Integer = m_vReportGroupUserGroup.GetLowerBound(1) To m_vReportGroupUserGroup.GetUpperBound(1)
                    lPos = IsInArray(CInt(m_vReportGroupUserGroup(0, lCount)), m_vAllPMUserGroup)

                    If lPos <> -1 Then
                        Me.lvwReportGroupUserGroups.Items.Item(lPos).Selected = True
                        'Me.lvwReportGroupUserGroups.Items.Item(Me.lvwReportGroupUserGroups.FocusedItem.Index).Tag = Convert.ToString(Me.lvwReportGroupUserGroups.Items.Item(Me.lvwReportGroupUserGroups.FocusedItem.Index).Tag) & "checked"
                        Me.lvwReportGroupUserGroups.Items.Item(lPos).Tag = Convert.ToString(Me.lvwReportGroupUserGroups.Items.Item(lPos).Tag) & "checked"


                        'Developer Guide No. 49
                        Me.lvwReportGroupUserGroups.Items(lPos).ImageIndex = 0

                    End If
                Next
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TagReportGroupUserGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TagReportGroupUserGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: IsInArray
    '
    ' Description:loop through r_vArray and return element position
    '             if first element is equals to v_lID
    '             return -1 if not found
    ' ***************************************************************** '
    Private Function IsInArray(ByVal v_lID As Integer, ByRef r_vArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = -1

            If Information.IsArray(r_vArray) Then
                For lCount As Integer = r_vArray.GetLowerBound(1) To r_vArray.GetUpperBound(1)

                    If v_lID = CDbl(r_vArray(0, lCount)) Then
                        result = lCount
                        Exit For
                    End If
                Next
            End If

            Return result

        Catch excep As System.Exception



            result = -1

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsInArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsInArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: UnloadReportGroup
    '
    ' Description: loop through listview and store selected item to array
    '
    ' ***************************************************************** '
    Private Function UnloadReportGroup() As Integer

        Dim result As Integer = 0
        Dim lPos As Integer
        Dim bFirstTime As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bFirstTime = True
            lPos = 0
            m_vReportGroup = VB6.CopyArray(Nothing)

            For lCount As Integer = 1 To lvwReportGroupContents.Items.Count

                lvwReportGroupContents.Items.Item(lCount - 1).Selected = True

                'check to see if user has selected this record
                lPos = (CStr(Convert.ToString(lvwReportGroupContents.Items.Item(lCount - 1).Tag)).IndexOf("checked") + 1)

                If lPos <> 0 Then

                    If bFirstTime Then
                        bFirstTime = False
                        ReDim m_vReportGroup(0, 0)
                    Else
                        ReDim Preserve m_vReportGroup(0, m_vReportGroup.GetUpperBound(1) + 1)
                    End If

                    ' m_vReportGroup(0, m_vReportGroup.GetUpperBound(1)) = m_vAllReport(0, CInt(Convert.ToString(lvwReportGroupContents.Items.Item(lvwReportGroupContents.FocusedItem.Index).Tag).Substring(0, lPos - 1)))
                    m_vReportGroup(0, m_vReportGroup.GetUpperBound(1)) = m_vAllReport(0, CInt(Convert.ToString(lvwReportGroupContents.Items.Item(lCount - 1).Tag).Substring(0, lPos - 1)))

                End If

                lvwReportGroupContents.Items.Item(lCount - 1).Selected = False

            Next

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnloadReportGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnloadReportGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: UnloadReportGroupUserGroup
    '
    ' Description: loop through listview and store selected item to array
    '
    ' ***************************************************************** '
    Private Function UnloadReportGroupUserGroup() As Integer

        Dim result As Integer = 0
        Dim lPos As Integer
        Dim bFirstTime As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bFirstTime = True
            lPos = 0
            m_vReportGroupUserGroup = VB6.CopyArray(Nothing)

            For lCount As Integer = 1 To lvwReportGroupUserGroups.Items.Count
                lvwReportGroupUserGroups.Items.Item(lCount - 1).Selected = True

                'check to see if user has selected this record
                lPos = (CStr(Convert.ToString(lvwReportGroupUserGroups.Items.Item(lCount - 1).Tag)).IndexOf("checked") + 1)

                If lPos <> 0 Then
                    If bFirstTime Then
                        bFirstTime = False
                        ReDim m_vReportGroupUserGroup(0, 0)
                    Else
                        ReDim Preserve m_vReportGroupUserGroup(0, m_vReportGroupUserGroup.GetUpperBound(1) + 1)
                    End If
                    'm_vReportGroupUserGroup(0, m_vReportGroupUserGroup.GetUpperBound(1)) = m_vAllPMUserGroup(0, CInt(Convert.ToString(lvwReportGroupUserGroups.Items.Item(lvwReportGroupUserGroups.FocusedItem.Index).Tag).Substring(0, lPos - 1)))
                    m_vReportGroupUserGroup(0, m_vReportGroupUserGroup.GetUpperBound(1)) = m_vAllPMUserGroup(0, CInt(Convert.ToString(lvwReportGroupUserGroups.Items.Item(lCount - 1).Tag).Substring(0, lPos - 1)))
                End If
            Next

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnloadReportGroupUserGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnloadReportGroupUserGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
        Dim Msg As String = ""
        ' Click event of the Apply button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Apply command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                cmdApply.Enabled = True
                Exit Sub
            End If
            cmdApply.Enabled = False

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Apply command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try
    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, MainModule.ScreenHelpID), gPMConstants.PMEReturnCode)


    End Sub

    Private Sub cmdSelectReportGroup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelectReportGroup.Click

        If Me.lvwReportGroupContents.FocusedItem Is Nothing Then
            Exit Sub
        End If
        'check to see if current record is selected
        Dim bSelectedChecked As Boolean = CStr(Convert.ToString(Me.lvwReportGroupContents.Items.Item(Me.lvwReportGroupContents.FocusedItem.Index).Tag)).IndexOf("checked") >= 0
        If bSelectedChecked = True Then
            bSelectedChecked = False
        Else
            bSelectedChecked = True
        End If

        ' For iCnt As Integer = 1 To Me.lvwReportGroupContents.Items.Count
        For iCnt As Integer = 0 To Me.lvwReportGroupContents.Items.Count - 1
            If Me.lvwReportGroupContents.Items.Item(iCnt).Selected Then
                SetItemState(iCnt, bSelectedChecked)
            End If
        Next iCnt
        cmdApply.Enabled = True

        'CMG 15082002 Keep the selected item selected
        Me.lvwReportGroupContents.Focus()
        Me.lvwReportGroupContents.FocusedItem = Me.lvwReportGroupContents.Items.Item(Me.lvwReportGroupContents.FocusedItem.Index)
        lvwReportGroupContents_Click(lvwReportGroupContents, New EventArgs())
    End Sub

    Private Sub cmdSelectUserGroups_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelectUserGroups.Click
        If Me.lvwReportGroupUserGroups.FocusedItem Is Nothing Then
            Exit Sub
        End If

        'check to see if current record is selected
        Dim lPos As Integer = (CStr(Convert.ToString(Me.lvwReportGroupUserGroups.Items.Item(Me.lvwReportGroupUserGroups.FocusedItem.Index).Tag)).IndexOf("checked") + 1)

        'select it if it's not selected or unselect if it's selected
        If lPos <> 0 Then
            'Me.lvwReportGroupUserGroups.Items.Item(Me.lvwReportGroupUserGroups.FocusedItem.Index).Tag = Convert.ToString(Me.lvwReportGroupUserGroups.Items.Item(Me.lvwReportGroupUserGroups.FocusedItem.Index).Tag).Substring(0, lPos - 1)
            Me.lvwReportGroupUserGroups.Items.Item(Me.lvwReportGroupUserGroups.SelectedItems(0).Index).Tag = Convert.ToString(Me.lvwReportGroupUserGroups.Items.Item(Me.lvwReportGroupUserGroups.SelectedItems(0).Index).Tag).Substring(0, lPos - 1)


            'Developer Guide No. 49
            Me.lvwReportGroupUserGroups.Items(Me.lvwReportGroupUserGroups.FocusedItem.Index).ImageIndex = 1
        Else
            Me.lvwReportGroupUserGroups.Items.Item(Me.lvwReportGroupUserGroups.SelectedItems(0).Index).Tag = Convert.ToString(Me.lvwReportGroupUserGroups.Items.Item(Me.lvwReportGroupUserGroups.SelectedItems(0).Index).Tag) & "checked"


            'Developer Guide No. 49
            Me.lvwReportGroupUserGroups.Items(Me.lvwReportGroupUserGroups.FocusedItem.Index).ImageIndex = 0
        End If
        cmdApply.Enabled = True

        'CMG 15082002 Keep the selected item selected
        Me.lvwReportGroupUserGroups.Focus()
        Me.lvwReportGroupUserGroups.FocusedItem = Me.lvwReportGroupUserGroups.Items.Item(Me.lvwReportGroupUserGroups.FocusedItem.Index)
        lvwReportGroupUserGroups_Click(lvwReportGroupUserGroups, New EventArgs())
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
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRReportGroup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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
            m_oGeneral = New iPMUReportGroup.General()

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

            m_oBusiness.ReportGroupID = m_lReportGroupId
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
    Private Const vbFormCode As Integer = 0
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
                'Process the next set of actions depending
                'upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                'Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.cancel = True
                    'Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

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

    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisableInterface) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DisableInterface(ByRef bDisable As Boolean) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'cmdOK.Enabled = Not bDisable
    'cmdEdit.Enabled = Not bDisable
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisplayStatusSearching) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub DisplayStatusSearching()
    '
    'Static sMessage As String = ""
    '
    'Try 
    '
    ' Get message text if not already present.
    'If sMessage = "" Then

    'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    'End If
    '
    ' Display the status message.
    '    stbStatus.SimpleText = " " & sMessage$
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisplayStatusFound) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub DisplayStatusFound()
    '
    'Static sMessage As String = ""
    'Dim lItemsFound As Integer
    '
    'Try 
    '
    '
    ' Get message text if not already present.
    'If sMessage = "" Then

    'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    'End If
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    '
    'End Sub

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
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                        End If
                End Select
            End With
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMainTab.SelectedIndex = 1
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim Msg As String = ""
        Dim sUniqueId As String = ""
        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK
            sUniqueId = GetUniqueID()
            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(sUniqueId), gPMConstants.PMEReturnCode)

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

    Private Sub lvwReportGroupContents_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwReportGroupContents.Click
        If Me.lvwReportGroupContents.Items.Count > 0 Then
            If Me.lvwReportGroupContents.SelectedItems.Count > 0 Then
                'If CStr(Convert.ToString(Me.lvwReportGroupContents.Items.Item(Me.lvwReportGroupContents.FocusedItem.Index).Tag)).IndexOf("checked") >= 0 Then
                If CStr(Convert.ToString(Me.lvwReportGroupContents.Items.Item(Me.lvwReportGroupContents.SelectedItems(0).Index).Tag)).IndexOf("checked") >= 0 Then
                    'CMG/PB 15082002 Double these up to stop
                    ' interference between Alt key select button selections
                    Me.cmdSelectReportGroup.Text = "&Unselect"
                    Me.cmdSelectUserGroups.Text = "&Select"
                Else
                    Me.cmdSelectReportGroup.Text = "&Select"
                    Me.cmdSelectUserGroups.Text = "&Unselect"
                End If
            End If
        End If
    End Sub

    Private Sub lvwReportGroupContents_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwReportGroupContents.Enter
        'CMG/PB 15082002 Use the click logic
        lvwReportGroupContents_Click(lvwReportGroupContents, New EventArgs())
        'lvwReportGroupContents_SelectedIndexChanged(lvwReportGroupContents, New EventArgs())
    End Sub

    Private Sub lvwReportGroupContents_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwReportGroupContents.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        Dim iSelected As Integer
        If Me.lvwReportGroupContents.Items.Count > 0 Then
            Select Case KeyCode
                Case Keys.PageUp
                    iSelected = 1
                Case Keys.PageDown
                    iSelected = Me.lvwReportGroupContents.Items.Count
                Case Keys.Up
                    iSelected = Me.lvwReportGroupContents.FocusedItem.Index + 1 - 1
                Case Keys.Down
                    iSelected = Me.lvwReportGroupContents.FocusedItem.Index + 1 + 1
                Case Else
                    iSelected = Me.lvwReportGroupContents.FocusedItem.Index + 1
            End Select

            'CMG 15082002 ignore the alt key
            If KeyCode <> 18 Then
                If iSelected > Me.lvwReportGroupContents.Items.Count Then
                    iSelected = Me.lvwReportGroupContents.Items.Count
                ElseIf iSelected < 1 Then
                    iSelected = 1
                End If

                If CStr(Convert.ToString(Me.lvwReportGroupContents.Items.Item(iSelected - 1).Tag)).IndexOf("checked") >= 0 Then
                    'CMG/PB 15082002 Double these up to stop
                    ' interference between Alt key select button selections
                    Me.cmdSelectReportGroup.Text = "&Unselect"
                    Me.cmdSelectUserGroups.Text = "&Select"
                Else
                    Me.cmdSelectReportGroup.Text = "&Select"
                    Me.cmdSelectUserGroups.Text = "&Unselect"
                End If
            End If
        End If

    End Sub

    Private Sub lvwReportGroupContents_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwReportGroupContents.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If lvwReportGroupContents.GetItemAt(x, y) Is Nothing Then
                'cmdApply.Enabled = False
            Else
                'cmdApply.Enabled = True
            End If
        End If

    End Sub

    Private Sub lvwReportGroupUserGroups_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwReportGroupUserGroups.Click
        If Me.lvwReportGroupUserGroups.Items.Count > 0 Then
            If Me.lvwReportGroupUserGroups.SelectedItems.Count > 0 Then
                If CStr(Convert.ToString(Me.lvwReportGroupUserGroups.Items.Item(Me.lvwReportGroupUserGroups.SelectedItems(0).Index).Tag)).IndexOf("checked") >= 0 Then
                    'CMG/PB 15082002 Double these up to stop
                    ' interference between Alt key select button selections
                    Me.cmdSelectUserGroups.Text = "&Unselect"
                    Me.cmdSelectReportGroup.Text = "&Select"
                Else
                    Me.cmdSelectUserGroups.Text = "&Select"
                    Me.cmdSelectReportGroup.Text = "&Unselect"
                End If
            End If
        End If
    End Sub

    Private Sub lvwReportGroupContents_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwReportGroupContents.DoubleClick
        'PN: 47805
        If m_iTask = gPMConstants.PMEComponentAction.PMView Then
            Exit Sub
        End If
        Dim idx As Integer = Me.lvwReportGroupContents.FocusedItem.Index + 1
        SetItemState(idx, CStr(Convert.ToString(Me.lvwReportGroupContents.Items.Item(idx - 1).Tag)).IndexOf("checked") >= 0)
        cmdApply.Enabled = True
    End Sub

    Private Sub lvwReportGroupUserGroups_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwReportGroupUserGroups.DoubleClick
        'PN: 47805
        If m_iTask = gPMConstants.PMEComponentAction.PMView Then
            Exit Sub
        End If
        Dim idx As Integer = Me.lvwReportGroupUserGroups.FocusedItem.Index + 1
        SetGroupsItemState(idx, CStr(Convert.ToString(Me.lvwReportGroupUserGroups.Items.Item(idx - 1).Tag)).IndexOf("checked") >= 0)
        cmdApply.Enabled = True

    End Sub

    Private Sub lvwReportGroupUserGroups_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwReportGroupUserGroups.Enter
        'CMG/PB 15082002 Use the click logic
        lvwReportGroupUserGroups_Click(lvwReportGroupUserGroups, New EventArgs())
    End Sub

    Private Sub lvwReportGroupUserGroups_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwReportGroupUserGroups.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        Dim iSelected As Integer
        If Me.lvwReportGroupUserGroups.Items.Count > 0 Then
            Select Case KeyCode
                Case Keys.PageUp
                    iSelected = 1
                Case Keys.PageDown
                    iSelected = Me.lvwReportGroupUserGroups.Items.Count
                Case Keys.Up
                    iSelected = Me.lvwReportGroupUserGroups.FocusedItem.Index + 1 - 1
                Case Keys.Down
                    iSelected = Me.lvwReportGroupUserGroups.FocusedItem.Index + 1 + 1
                Case Else
                    iSelected = Me.lvwReportGroupUserGroups.FocusedItem.Index + 1
            End Select

            'CMG 15082002 ignore the alt key
            If KeyCode <> 18 Then
                If iSelected > Me.lvwReportGroupUserGroups.Items.Count Then
                    iSelected = Me.lvwReportGroupUserGroups.Items.Count
                ElseIf iSelected < 1 Then
                    iSelected = 1
                End If

                If CStr(Convert.ToString(Me.lvwReportGroupUserGroups.Items.Item(iSelected - 1).Tag)).IndexOf("checked") >= 0 Then
                    'CMG/PB 15082002 Double these up to stop
                    ' interference between Alt key select button selections
                    Me.cmdSelectUserGroups.Text = "&Unselect"
                    Me.cmdSelectReportGroup.Text = "&Select"
                Else
                    Me.cmdSelectUserGroups.Text = "&Select"
                    Me.cmdSelectReportGroup.Text = "&Unselect"
                End If
            End If
        End If

    End Sub

    Private Sub lvwReportGroupUserGroups_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwReportGroupUserGroups.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If lvwReportGroupUserGroups.GetItemAt(x, y) Is Nothing Then
                'cmdApply.Enabled = False
            Else
                'cmdApply.Enabled = True
            End If
        End If

    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        'Developer Guide No. 26
        pnlLblRG(SSTabHelper.GetSelectedIndex(tabMainTab)).Text = m_sReportGroupCode
        tabMainTabPreviousTab = tabMainTab.SelectedIndex
    End Sub
    ' PRIVATE Events (End)
    Private Sub SetItemState(ByRef index As Integer, ByRef bSelected As Boolean)
        Dim sValTag As String = CStr(Conversion.Val(Convert.ToString(Me.lvwReportGroupContents.Items.Item(index).Tag))).Trim()

        'select it if it's not selected or unselect if it's selected
        If bSelected Then
            Me.lvwReportGroupContents.Items.Item(index).Tag = sValTag & "checked"
            'Developer Guide No. 49
            Me.lvwReportGroupContents.Items(index).ImageIndex = 0
        Else
            Me.lvwReportGroupContents.Items.Item(index).Tag = sValTag
            'Developer Guide No. 49
            Me.lvwReportGroupContents.Items(index).ImageIndex = 1
        End If

    End Sub
    'eck 141003 PN 4389

    Private Sub SetGroupsItemState(ByRef index As Integer, ByRef bSelected As Boolean)
        Dim sValTag As String = CStr(Conversion.Val(Convert.ToString(Me.lvwReportGroupUserGroups.Items.Item(index).Tag))).Trim()

        'select it if it's not selected or unselect if it's selected
        If bSelected Then
            Me.lvwReportGroupUserGroups.Items.Item(index).Tag = sValTag

            'Developer Guide No. 49
            Me.lvwReportGroupUserGroups.Items(index).ImageIndex = 1
        Else
            Me.lvwReportGroupUserGroups.Items.Item(index).Tag = sValTag & "checked"

            'Developer Guide No. 49
            Me.lvwReportGroupUserGroups.Items(index).ImageIndex = 0
        End If

    End Sub

    Private Sub lvwReportGroupContents_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwReportGroupContents.SelectedIndexChanged
        If Me.lvwReportGroupContents.Items.Count > 0 Then
            If Me.lvwReportGroupContents.SelectedItems.Count > 0 Then
                'If CStr(Convert.ToString(Me.lvwReportGroupContents.Items.Item(Me.lvwReportGroupContents.FocusedItem.Index).Tag)).IndexOf("checked") >= 0 Then
                If CStr(Convert.ToString(Me.lvwReportGroupContents.Items.Item(Me.lvwReportGroupContents.SelectedItems(0).Index).Tag)).IndexOf("checked") >= 0 Then
                    'CMG/PB 15082002 Double these up to stop
                    ' interference between Alt key select button selections
                    Me.cmdSelectReportGroup.Text = "&Unselect"
                    Me.cmdSelectUserGroups.Text = "&Select"
                Else
                    Me.cmdSelectReportGroup.Text = "&Select"
                    Me.cmdSelectUserGroups.Text = "&Unselect"
                End If
            End If
        End If
    End Sub
End Class
