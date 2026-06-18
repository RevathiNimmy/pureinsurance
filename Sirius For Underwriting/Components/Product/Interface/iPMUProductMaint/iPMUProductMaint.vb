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

    Private m_lProductID As Integer
    Private m_sCode As String = ""
    Private m_sDescription As String = ""
    Private m_dtProductEffectiveDate As Date
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUProductMaint.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the Product Interface object.

    Private m_oProduct As iPMUProduct.Interface_Renamed

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    'stores all products to be displayed on the listview
    Private m_vProductList(,) As Object

    Private m_iSortKey As Integer
    Private m_iDirection As SortOrder
    Private Const ACDateColumn As Integer = 2
    Private m_sUniqueId As String = ""
    Private m_SScreenHierarchy As String = ""


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


            m_lReturn = m_oBusiness.GetAllProducts(r_vResultArray:=m_vProductList)

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
    ' ***************************************************************** '
    Private Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the search details.
            Me.lvwProduct.Items.Clear()

            ' Assign the details to the interface.
            If Information.IsArray(m_vProductList) Then

                For lRow As Integer = m_vProductList.GetLowerBound(1) To m_vProductList.GetUpperBound(1)

                    ' {* USER DEFINED CODE (Begin) *}

                    ' Assign the details to listview control

                    'Column 1 Code


                    'Developer Guide No. 274
                    oListItem = Me.lvwProduct.Items.Add(CStr(m_vProductList(ACICode, lRow)).Trim())
                    ListViewHelper.SetListItemSmallIconProperty(oListItem, "Prodtype")


                    'Column 2 Description
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vProductList(ACIDescription, lRow))

                    'Column 3 Effective Date
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFormatDate, vControlValue:=m_vProductList(ACIProductEffectiveDate, lRow))

                    ' Check for errors
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to assign the data.
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtFormatDate.Text

                    oListItem.Tag = CStr(lRow)

                    'ghosted deleted records
                    If m_vProductList(ACIIsDeleted, lRow) = gPMConstants.PMEReturnCode.PMTrue Then
                        '                Me.lvwProduct.ListItems(lRow& + 1).Selected = True
                        '                Me.lvwProduct.SelectedItem.Ghosted = True

                        'Developer Guide No. 13 (no solution) comment the condition
                        oListItem.ForeColor = Color.Gray
                    End If

                    ' Refresh the first X amount of rows, to
                    ' allow the user to see the results instantly.
                    If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                        ' Select the first item.
                        Me.lvwProduct.Items.Item(0).Selected = True

                        ' Refresh the initial results.
                        Me.lvwProduct.Refresh()
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
                Case gPMConstants.PMEComponentAction.PMAdd
                    'Tomo22062001
                    'The ReDim numbers are massively out as the stored procedure returns more rows...
                    If Not Information.IsArray(m_vProductList) Then
                        '            ReDim m_vProductList(ACIReportPointer, 0)
                        lSelectedItem = 0
                        'PW230502 - remove preserve keyword as it caused error when used on non-array
                        ReDim m_vProductList(25, lSelectedItem)
                    Else
                        lSelectedItem = m_vProductList.GetUpperBound(1) + 1
                        '            ReDim Preserve m_vProductList(ACIMaxTotalClaimValue, lSelectedItem)
                        ReDim Preserve m_vProductList(m_vProductList.GetUpperBound(0), lSelectedItem)
                    End If

                    m_vProductList(ACIProductid, lSelectedItem) = m_lProductID
                    m_vProductList(ACICode, lSelectedItem) = m_sCode
                    m_vProductList(ACIDescription, lSelectedItem) = m_sDescription
                    m_vProductList(ACIProductEffectiveDate, lSelectedItem) = m_dtProductEffectiveDate
                    m_vProductList(ACIIsDeleted, lSelectedItem) = gPMConstants.PMEReturnCode.PMFalse

                Case gPMConstants.PMEComponentAction.PMEdit
                    If IsNothing(Me.lvwProduct.FocusedItem) Then
                        For iCounter As Integer = 0 To Me.lvwProduct.Items.Count - 1
                            If (lvwProduct.Items(iCounter).ForeColor <> Color.Gray) Then
                                lSelectedItem = lvwProduct.Items(iCounter).Tag
                                Exit For
                            End If
                        Next

                    Else
                        lSelectedItem = Convert.ToString(Me.lvwProduct.Items.Item(Me.lvwProduct.FocusedItem.Index).Tag)
                    End If
                    'lSelectedItem = Convert.ToString(Me.lvwProduct.Items.Item(Me.lvwProduct.FocusedItem.Index).Tag)

                    m_vProductList(ACIProductid, lSelectedItem) = m_lProductID
                    m_vProductList(ACICode, lSelectedItem) = m_sCode
                    m_vProductList(ACIDescription, lSelectedItem) = m_sDescription
                    m_vProductList(ACIProductEffectiveDate, lSelectedItem) = m_dtProductEffectiveDate
                    m_vProductList(ACIIsDeleted, lSelectedItem) = gPMConstants.PMEReturnCode.PMFalse

            End Select

            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Refresh from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataRefresh", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



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
            'm_lReturn = CType(SetExtraListViewProperties(v_hWndList:=Me.lvwProduct.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'Return gPMConstants.PMEReturnCode.PMFalse
            'End If
            lvwProduct.FullRowSelect = True

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


            'Developer Guide No. 243
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

        'Create product object if not already done so
        If m_oProduct Is Nothing Then

            ' Get an instance of the Short Period Rate interface object via
            ' the public object manager.
            Dim temp_m_oProduct As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oProduct, sClassName:="iPMUProduct.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oProduct = temp_m_oProduct

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Product object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub

            End If

        End If

        m_lReturn = CType(m_oProduct.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vProductList:=m_vProductList), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        m_sUniqueId = GetUniqueID()
        m_oProduct.UniqueId = m_sUniqueId

        m_lReturn = m_oProduct.Start()

        cmdOK.Enabled = True
        cmdCancel.Enabled = True

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        Me.Refresh()

        'If not cancelled, add to grid

        If m_oProduct.Status = gPMConstants.PMEReturnCode.PMCancel Then
            Exit Sub
        End If


        'get data back from SPR object

        m_lProductID = m_oProduct.ProductID

        m_sCode = m_oProduct.Code

        m_sDescription = m_oProduct.Description

        m_dtProductEffectiveDate = m_oProduct.ProductEffectiveDate


        m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)
        Exit Sub

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click


        m_iAction = gPMConstants.PMEComponentAction.PMDelete
        Dim iDelete As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sScreenHierarchy As String = ""

        Dim lSelectedItem As Integer
        If Me.lvwProduct.FocusedItem Is Nothing Then
            lSelectedItem = Convert.ToString(Me.lvwProduct.Items.Item(0).Tag)
        Else
            lSelectedItem = Convert.ToString(Me.lvwProduct.Items.Item(Me.lvwProduct.FocusedItem.Index).Tag)
        End If



        'Developer Guide No solution no. 12
        If Me.lvwProduct.SelectedItems.Item(0).ForeColor = Color.Gray Then
            iDelete = gPMConstants.PMEReturnCode.PMFalse
        End If

        m_sUniqueId = GetUniqueID()
        sScreenHierarchy = $"Product({lvwProduct.FocusedItem.Text.Trim()})"
        m_lReturn = m_oBusiness.DelProduct(m_vProductList(ACIProductid, lSelectedItem), iDelete, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=sScreenHierarchy)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_vProductList(ACIIsDeleted, lSelectedItem) = iDelete
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
        'Dim lSelectedItem As Integer = Convert.ToString(Me.lvwProduct.Items.Item(Me.lvwProduct.FocusedItem.Index).Tag)
        Dim lSelectedItem As Integer


        If IsNothing(Me.lvwProduct.FocusedItem) Then
            For iCounter As Integer = 0 To Me.lvwProduct.Items.Count - 1
                If (lvwProduct.Items(iCounter).ForeColor <> Color.Gray) Then
                    lSelectedItem = lvwProduct.Items(iCounter).Tag
                    Exit For
                End If
            Next

        Else
            lSelectedItem = Convert.ToString(Me.lvwProduct.Items.Item(Me.lvwProduct.FocusedItem.Index).Tag)
        End If




        'Create Product object if not already done so
        If m_oProduct Is Nothing Then

            ' Get an instance of the Product interface object via
            ' the public object manager.
            Dim temp_m_oProduct As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oProduct, sClassName:="iPMUProduct.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oProduct = temp_m_oProduct

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Product object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub

            End If

        End If

        m_lReturn = CType(m_oProduct.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vProductList:=m_vProductList), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        'pass selected details to SPR object

        m_oProduct.ProductID = CInt(m_vProductList(ACIProductid, lSelectedItem))

        m_sUniqueId = GetUniqueID()
        m_oProduct.UniqueId = m_sUniqueId

        m_lReturn = m_oProduct.Start()

        cmdOK.Enabled = True
        cmdCancel.Enabled = True

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        Me.Refresh()

        'get data back from Product object

        m_lProductID = m_oProduct.ProductID

        m_sCode = m_oProduct.Code

        m_sDescription = m_oProduct.Description

        m_dtProductEffectiveDate = m_oProduct.ProductEffectiveDate

        'Update the existing item
        m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to update the data.
            Exit Sub
        End If



    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        'Developer Guide No. 184 (Latest guide)
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, ScreenhelpID), gPMConstants.PMEReturnCode)

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
        Dim sMessage, sTitle As String
        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        ' Click event of the Cancel button.
        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            result = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.No Then
                ' Set return to false, meaning don't cancel.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Check the return value.
            If result = gPMConstants.PMEReturnCode.PMTrue Then
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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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
            m_oGeneral = New iPMUProductMaint.General()

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

            If Not (m_oProduct Is Nothing) Then
                ' Terminate the business object

                m_oProduct.Dispose()
                m_oProduct = Nothing

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

    Private Sub lvwProduct_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwProduct.Click
        If lvwProduct.Items.Count > 0 Then

            'If Me.lvwProduct.SelectedItem.Ghosted Then
            If Me.lvwProduct.SelectedItems.Item(0).ForeColor = Color.Gray Then
                cmdDelete.Text = "&UnDelete"
                cmdEdit.Enabled = False
            Else
                cmdDelete.Text = "&Delete"
                cmdEdit.Enabled = True
            End If
            cmdDelete.Enabled = True
        End If
    End Sub
    Private Sub lvwProduct_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwProduct.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwProduct.Columns(eventArgs.Column)
        Dim nOrder As SortOrder

        ' Column click event for the search details
        If ListViewHelper.GetSortOrderProperty(lvwProduct) = SortOrder.Descending Then
            nOrder = SortOrder.Ascending
        Else
            nOrder = SortOrder.Descending
        End If

        ListViewHelper.SetSortedProperty(lvwProduct, True)


        Try

            With lvwProduct

                If ColumnHeader.Index + 1 - 1 = ACDateColumn Then

                    If m_iSortKey <> ACDateColumn Then
                        m_iSortKey = ACDateColumn
                        m_iDirection = nOrder
                    Else
                        m_iDirection = nOrder
                    End If
                    'Developer Guide No. 170 (Latest guide)
                    m_lReturn = CType(ListViewFunc.ListViewSortByDate(v_oListView:=lvwProduct, v_iSourceColumn:=ACDateColumn, v_iDirection:=m_iDirection), gPMConstants.PMEReturnCode)
                    ' If current sort column header is
                    ' pressed.
                ElseIf (ColumnHeader.Index + 1 - 1 = m_iSortKey) Then
                    ' Set sort order opposite of
                    ' current direction.
                    m_iDirection = nOrder
                    ListViewHelper.SetSortKeyProperty(lvwProduct, m_iSortKey)
                    ListViewHelper.SetSortOrderProperty(lvwProduct, m_iDirection)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwProduct, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwProduct, nOrder)
                    ListViewHelper.SetSortKeyProperty(lvwProduct, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwProduct, True)
                    m_iDirection = ListViewHelper.GetSortOrderProperty(lvwProduct)
                    m_iSortKey = ListViewHelper.GetSortKeyProperty(lvwProduct)
                End If
            End With

        Catch excep As System.Exception
            ' Error Section
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwProduct_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwProduct_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwProduct.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        'Not if we're viewing, thank you very much
        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If Me.lvwProduct.GetItemAt(x, Y) Is Nothing Then
                cmdDelete.Enabled = False
                cmdAdd.Enabled = True
                cmdEdit.Enabled = False
            Else
                cmdEdit.Enabled = True
                cmdDelete.Enabled = True
            End If
        End If
    End Sub

    Private Sub tabMaintab_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabMaintab.KeyDown

    End Sub
End Class
