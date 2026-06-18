Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date:18/06/2007
    '
    ' Description: Main interface.
    '
    ' Edit History: VB
    ' ***************************************************************** '
    'developer guide no.7
    Public Const vbFormCode As Integer = 0

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"

    'Constants for Defining Width of Columns in List View
    Private Const ColWidth As Integer = 1300

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    ' Variables for Find Case
    Private m_sCaseNumber As String = ""
    Private m_lBaseCaseID As Integer

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Stores the search data from the business object.
    Public m_vSearchData As Object

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



    'Private Sub Status(ByVal Value As Integer)
    ' Standard Property.
    ' Set the interface exit status.
    'm_lStatus = Value
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get
            ' Standard Property.
            ' Return the interface exit status.
            Return m_lStatus
        End Get
    End Property


    Public Property CaseNumber() As String
        Get
            Return m_sCaseNumber
        End Get
        Set(ByVal Value As String)
            m_sCaseNumber = Value
        End Set
    End Property


    Public Property BaseCaseID() As Integer
        Get
            Return m_lBaseCaseID
        End Get
        Set(ByVal Value As Integer)
            m_lBaseCaseID = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name          : GetBusiness
    ' Description   : Retrieves the details from the business object.
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue




            m_vSearchData = Nothing

            ' Get the Case details from the business object.


            m_lReturn = m_oBusiness.GetCaseHistory(v_lBaseCaseID:=m_lBaseCaseID, r_vResultArray:=m_vSearchData)

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

            'Assign Values to Interface
            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name        : DataToInterface
    ' Description : Updates all interface details from the search data.
    '               storage.
    ' Date        : 18/06/2007
    ' Edit History: VB
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        'Const ACFindImage As String = "FindImage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwCaseHistory.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If
            cmdView.Enabled = True

            ' Assign the details to the interface.

            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                ' Assign the details to the first column.

                oListItem = lvwCaseHistory.Items.Add(CStr(gPMFunctions.ToSafeLong(CStr(m_vSearchData(kICaseID, lRow)))))


                ' Assign details to other the columns

                oListItem.SubItems.Add(1).Text = StringsHelper.Format(gPMFunctions.ToSafeDate(CStr(m_vSearchData(kIDateOfChange, lRow))), "dd/mm/yyyy hh:mm:ss AMPM")

                oListItem.SubItems.Add(2).Text = gPMFunctions.ToSafeString(CStr(m_vSearchData(kIDescription, lRow))).Trim()

                oListItem.SubItems.Add(3).Text = gPMFunctions.ToSafeString(CStr(m_vSearchData(kIProgressStatus, lRow))).Trim()

                oListItem.SubItems.Add(4).Text = gPMFunctions.ToSafeString(CStr(m_vSearchData(kIUser, lRow))).Trim()

                ' Set the tag property with the index of the search data storage.
                oListItem.Tag = CStr(lRow)

                ' Refresh the first X amount of rows, to allow
                ' the user to see the results instantly.
                If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.
                    lvwCaseHistory.Items.Item(0).Selected = True
                    ' Refresh the initial results.
                    lvwCaseHistory.Refresh()
                End If
            Next lRow

            ' Select the first item.
            lvwCaseHistory.Items.Item(0).Selected = True


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : SetInterfaceDefaults
    ' Description   : Sets all of the interface default values.
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)
            lvwCaseHistory.Columns.Item(kILvwCaseID - 1).Width = CInt(0)
            lvwCaseHistory.Columns.Item(kILvwDateOfChange - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth + 100))
            lvwCaseHistory.Columns.Item(kILvwDescription - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth + 100))
            lvwCaseHistory.Columns.Item(kILvwProgressStatus - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth))
            lvwCaseHistory.Columns.Item(kILvwUser - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth))

            cmdView.Enabled = False

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Made full row select on list views
            'developer guide no.303
            'm_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwCaseHistory.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
            lvwCaseHistory.FullRowSelect = True
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : DisplayCaptions
    ' Description   : Display all language specific captions.
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Close Button

            cmdClose.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kCloseButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'View Button

            cmdView.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kViewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Date of change


            lvwCaseHistory.Columns.Item(kILvwDateOfChange - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kLvwColNameDateOfChange, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Change Description


            lvwCaseHistory.Columns.Item(kILvwDescription - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kLvwColNameDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Case progress status


            lvwCaseHistory.Columns.Item(kILvwProgressStatus - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kLvwColNameProgressStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'User


            lvwCaseHistory.Columns.Item(kILvwUser - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kLvwColNameUser, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : ResizeInterface
    ' Description   : Resizes the interface controls.
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Function ResizeInterface() As Integer

        Dim result As Integer = 0
        Dim lColWidth As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvwCaseHistory.Height = Me.Height - (cmdView.Height + VB6.TwipsToPixelsY(550))
            lvwCaseHistory.Width = Me.Width - VB6.TwipsToPixelsX(100)

            cmdView.Top = lvwCaseHistory.Height + VB6.TwipsToPixelsY(40)
            cmdClose.Top = lvwCaseHistory.Height + VB6.TwipsToPixelsY(40)
            cmdClose.Left = Me.Width - (cmdClose.Width + VB6.TwipsToPixelsX(120))

            lColWidth = CInt((VB6.PixelsToTwipsX(lvwCaseHistory.Width) - 500) / 4)

            lvwCaseHistory.Columns.Item(kILvwDateOfChange - 1).Width = CInt(VB6.TwipsToPixelsX(lColWidth))
            lvwCaseHistory.Columns.Item(kILvwDescription - 1).Width = CInt(VB6.TwipsToPixelsX(lColWidth))
            lvwCaseHistory.Columns.Item(kILvwProgressStatus - 1).Width = CInt(VB6.TwipsToPixelsX(lColWidth))
            lvwCaseHistory.Columns.Item(kILvwUser - 1).Width = CInt(VB6.TwipsToPixelsX(lColWidth))

            Return result

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click

        Const ACMethod As String = "cmdView_Click"

        Dim bShowCustomScreen As Boolean
        Dim lPreviousDataModelId, lGISPolicyLinkID As Integer
        Dim lSelectedItem As Integer
        If Information.isnothing(lvwCaseHistory.FocusedItem) Then
            lSelectedItem = Convert.ToString(lvwCaseHistory.Items.Item(lvwCaseHistory.Items.Item(0).Index).Tag)
        Else
            lSelectedItem = Convert.ToString(lvwCaseHistory.Items.Item(lvwCaseHistory.FocusedItem.Index).Tag)
        End If
        ' use details from the initial search data array get generic fields

        Dim lCaseID As Integer = gPMFunctions.ToSafeLong(CStr(m_vSearchData(kICaseID, lSelectedItem)))

        If GetPreviousCaseBuilderDataModel(v_lCaseID:=lCaseID, r_lPreviousDataModelId:=lPreviousDataModelId, r_lGISPolicyLinkID:=lGISPolicyLinkID) <> gPMConstants.PMEReturnCode.PMTrue Then

            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to get previous screen data model info.")
        End If

        If lPreviousDataModelId > 0 Then
            If MessageBox.Show("Warning: the data model screen has been changed for Case," & _
                               Strings.Chr(13) & Strings.Chr(10) & "continuing will reset the custom data.", "Custom Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Cancel Then

                bShowCustomScreen = False
            Else
                'Delete all Party Builder GIS data for the policy link
                bShowCustomScreen = Not (DeleteCustomData(v_lGISPolicyLinkID:=lGISPolicyLinkID) <> gPMConstants.PMEReturnCode.PMTrue)
            End If
        Else
            bShowCustomScreen = True
        End If

        If bShowCustomScreen Then
            m_lReturn = CType(ShowCaseScreen(v_lTask:=gPMConstants.PMEComponentAction.PMView, v_lCaseID:=lCaseID), gPMConstants.PMEReturnCode)
        End If
    End Sub

    ' ***************************************************************** '
    ' Name          : FormIntialise
    ' Description   : Intialise all required details of the form
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Exit Sub
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_sUserName = .UserName
            End With

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMCase.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            End If

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception


            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name          : FormLoad
    ' Description   : Loads all required details of the form
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '

    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            'For viewing the Form in TaskBar
            iPMFunc.ShowFormInTaskBar_Detach()

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Validate fields using Forms Control

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

            m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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
    ' Name          : Form_Query Unload
    ' Description   : Store all Property Details before unloading form
    ' Date          : 18/06/2007
    ' Edit History  : VB
    ' ***************************************************************** '
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

            End If

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
    ' Name          :Form_Resize
    ' Description   :Resize the the controls on form
    ' Date          :18/06/2007
    ' Edit History  :VB
    ' ***************************************************************** '
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

    Private Sub lvwCaseHistory_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwCaseHistory.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwCaseHistory.Columns(eventArgs.Column)
        Dim iDirection As SortOrder

        ' Column click event for the search details

        Try

            With lvwCaseHistory

                ' If date column clicked, then sort by date sort column
                If ColumnHeader.Index + 1 - 1 = 4 Then

                    If ListViewHelper.GetSortKeyProperty(lvwCaseHistory) <> 4 Then
                        ListViewHelper.SetSortKeyProperty(lvwCaseHistory, 4)
                        iDirection = SortOrder.Ascending
                    Else
                        iDirection = (ListViewHelper.GetSortOrderProperty(lvwCaseHistory) + 1) Mod 2
                    End If
                    'developer guide no.170
                    m_lReturn = CType(ListViewFunc.ListViewSortByDate(v_oListView:=lvwCaseHistory, v_iSourceColumn:=4, v_iDirection:=iDirection), gPMConstants.PMEReturnCode)

                    ' If current sort column header is
                    ' pressed.
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwCaseHistory)) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwCaseHistory, (ListViewHelper.GetSortOrderProperty(lvwCaseHistory) + 1) Mod 2)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwCaseHistory, False)
                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwCaseHistory, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwCaseHistory, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwCaseHistory, True)
                End If
            End With

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwCaseHistory_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name         : lvwCaseHistory_DblClick
    ' Description  : Move to the next form in the road map
    ' Date         : 18/06/2007
    ' Edit History : VB
    ' ***************************************************************** '
    Private Sub lvwCaseHistory_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwCaseHistory.DoubleClick

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            If cmdView.Enabled Then
                cmdView_Click(cmdView, New EventArgs())
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwCaseHistory_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name         : ShowCaseScreen
    ' Description  :
    ' Date         : 18/06/2007
    ' Edit History : VB
    ' ***************************************************************** '
    Private Function ShowCaseScreen(ByVal v_lTask As Integer, ByVal v_lCaseID As Integer) As Integer
        Dim result As Integer = 0
        Dim oObject As iPMURisk.Interface_Renamed
        Dim sCaseScreenID As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim temp_oObject As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMURisk.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            'developer guide no.9
            m_lReturn = oObject.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            m_lReturn = oObject.SetProcessModes(vTask:=v_lTask)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oObject.Dispose()
                oObject = Nothing
                Return result
            End If

            iPMFunc.GetSystemOption(5035, sCaseScreenID)


            oObject.ScreenId = gPMFunctions.ToSafeLong(sCaseScreenID)

            If v_lCaseID > 0 Then

                oObject.CaseID = v_lCaseID

                oObject.BaseCaseID = m_lBaseCaseID

                oObject.CaseNumber = m_sCaseNumber
            End If


            m_lReturn = oObject.Start()


            oObject.Dispose()
            oObject = Nothing

            Return result

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowCaseScreen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowCaseScreen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ************************************************************************************** '
    ' Name: GetPreviousCaseBuilderDataModel
    ' Description: Returns previous data model Id if screen data model has changed else zero
    '              Returns GIS Policy Link Id if there is any
    ' ************************************************************************************** '
    Private Function GetPreviousCaseBuilderDataModel(ByVal v_lCaseID As Integer, ByRef r_lPreviousDataModelId As Integer, ByRef r_lGISPolicyLinkID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPreviousCaseBuilderDataModel"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the details of the screen from the db based on the screen code

            lReturn = m_oBusiness.GetPreviousDataModel(v_lCaseID:=v_lCaseID, r_lPreviousDataModelId:=r_lPreviousDataModelId, r_lGISPolicyLinkID:=r_lGISPolicyLinkID)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(CStr(lReturn), "GetPreviousDataModel failed.")
            End If

            If r_lPreviousDataModelId > 0 And r_lGISPolicyLinkID <= 0 Then
                gPMFunctions.RaiseError(CStr(lReturn), "Failed to get GIS Policy Link.")
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function


    ' **************************************************************************** '
    ' Name: DeleteCustomData
    ' Description: Deletes all corresponding GIS data for a GIS Policy Link Id
    ' **************************************************************************** '
    Private Function DeleteCustomData(ByVal v_lGISPolicyLinkID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteCustomData"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the details of the screen from the db based on the screen code

            lReturn = m_oBusiness.DeleteCustomData(v_lGISPolicyLinkID:=v_lGISPolicyLinkID)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(CStr(lReturn), "DeleteCustomData failed.")
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    ' PRIVATE Methods (End)
End Class