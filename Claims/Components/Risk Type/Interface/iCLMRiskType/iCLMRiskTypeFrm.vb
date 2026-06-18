Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name     : frmInterface
    ' Description   : Main interface.
    ' Date          : 30/08/2000
    ' Author        : Pandu
    ' Edit History  :
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    Private Const ACColumn1 As Integer = 1
    Private Const ACColumn2 As Integer = 2
    ' Alix
    Private Const ACColumn3 As Integer = 3
    Public Const ScreenHelpID As Integer = 30

    'Variable for Underwriting/Broking
    Private m_lSiriusUnderWritingBroking As String = ""

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iCLMRiskType.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    'Stores the data from the GetPerilsForReserve method of the business object.
    Public m_vTotalArray(,) As Object

    ' Alix
    Private m_lSelectedScreenID As Integer
    Private m_sSelectedDescription As String = ""

    Const ACColRiskTypeID As Integer = 0
    Const ACColRiskType As Integer = 1
    Const ACColDesc As Integer = 2
    ' Alix
    Const ACColScreenID As Integer = 3
    Const ACColScreenDesc As Integer = 4

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


    'Public Property Get ClaimId() As Long
    '
    '    'Return Claimid
    '   ClaimId = m_lClaimId&
    '
    'End Property
    'Public Property Let ClaimId(lClaimId As Long)
    '
    '    'Set Claim Number
    '    m_lClaimId& = lClaimId&
    '
    'End Property


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

    ' ***************************************************************** '
    ' Name:         GetBusiness
    '
    ' Description:  Retrieves the details from the business object.
    '
    ' Date:         11/07/00
    '
    ' Edit History: SK

    '
    ' ***************************************************************** '

    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Disable parts of the interface while
            ' a search is in progress.
            m_lReturn = DisableInterface(bDisable:=True)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If




            m_lReturn = g_oBusiness.GetRiskTypesUnderWriting(r_vResultArray:=m_vTotalArray)
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.

                Return gPMConstants.PMEReturnCode.PMFalse

            End If



            'populate the list view for each tab
            'Assign Values to Interface
            m_lReturn = DataToInterfaceSumm()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If




            'After loading & populating all the listviews
            'set the default to the 1st tab
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)


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


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DataToInterfaceSumm
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    ' Date:30/08/2000
    '
    ' Edit History:Pandu
    '
    '
    ' ***************************************************************** '
    Public Function DataToInterfaceSumm() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        'Const ACFindImage As String = "FindImage"
        Const ACColRiskTypeID As Integer = 0
        Const ACColRiskType As Integer = 1
        Const ACColDesc As Integer = 2


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            'make the tab on which the listview is to be put as the current tab
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)


            ' Clear the search details.
            lvwRiskType.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vTotalArray) Then
                Return result
            End If

            ' Assign the details to the interface.
            For lRow As Integer = m_vTotalArray.GetLowerBound(1) To m_vTotalArray.GetUpperBound(1)


                ' Assign the details to the first column.
                ' Column 1 Reserve Type
                oListItem = lvwRiskType.Items.Add(CStr(m_vTotalArray(ACColRiskType, lRow)).Trim())

                ' Assign details to other the columns
                ' Column 2 Description
                oListItem.SubItems.Add(1).Text = CStr(m_vTotalArray(ACColDesc, lRow)).Trim()

                ' Alix
                If ClaimBuilderIsEnable() Then
                    oListItem.SubItems.Add(2).Text = CStr(m_vTotalArray(ACColScreenDesc, lRow)).Trim()
                End If

                ' Set the tag property with Reserve Type ID
                oListItem.Tag = CStr(m_vTotalArray(ACColRiskTypeID, lRow)).Trim()



                ' Refresh the first X amount of rows, to
                ' allow the user to see the results instantly.
                If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.
                    lvwRiskType.Items.Item(0).Selected = True

                    ' Refresh the initial results.
                    lvwRiskType.Refresh()
                End If
                '    DoEvents
            Next lRow

            '    DoEvents


            ' Select the first item.
            lvwRiskType.Items.Item(0).Selected = True

            ' Enable the interface now that the search has completed.
            m_lReturn = DisableInterface(bDisable:=False)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterfaceSumm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToProperties
    '
    ' Description: Updates the property member from the search data
    '              storage.
    ' Date:15/07/00
    '
    ' Edit History:Pandu
    '
    ' ***************************************************************** '
    Public Function DataToProperties() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            lSelectedItem = Convert.ToString(lvwRiskType.Items.Item(lvwRiskType.FocusedItem.Index).Tag)


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' Date :15/07/2000
    '
    ' Edit History : Pandu
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const lColumnWidth As Integer = 2600

            'Disable Command Button OK if no details are available

            cmdOK.Enabled = False
            cmdDefineFields.Enabled = False
            CmdShowScreen.Enabled = False

            ' Center the interface.
            iPMFunc.CenterForm(Me)


            'Add the columns to the listview
            lvwRiskType.Columns.Insert(ACColumn1 - 1, "", 94)
            lvwRiskType.Columns.Insert(ACColumn2 - 1, "", 94)

            ' Alix
            If ClaimBuilderIsEnable() Then
                lvwRiskType.Columns.Insert(ACColumn3 - 1, "", 94)
            End If

            If ClaimBuilderIsEnable() Then
                lvwRiskType.Columns.Item(ACColumn1 - 1).Width = CInt(VB6.TwipsToPixelsX(1600))
                lvwRiskType.Columns.Item(ACColumn2 - 1).Width = CInt(VB6.TwipsToPixelsX(1600))
                lvwRiskType.Columns.Item(ACColumn3 - 1).Width = CInt(VB6.TwipsToPixelsX(1600))
            Else
                ''Set the column widths
                lvwRiskType.Columns.Item(ACColumn1 - 1).Width = CInt(VB6.TwipsToPixelsX(lColumnWidth))
                lvwRiskType.Columns.Item(ACColumn2 - 1).Width = CInt(VB6.TwipsToPixelsX(lColumnWidth))
            End If

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lvwRiskType.FullRowSelect = True

            ' Set to the first tab.
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

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
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            'Developer Guide No.: 243
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

            'Developer Guide No.: 243
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            cmdDefineFields.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDefineFields, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            CmdShowScreen.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShowScreen, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            lvwRiskType.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No.: 243
            lvwRiskType.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Alix
            If ClaimBuilderIsEnable() Then
                'Developer Guide No.: 243
                lvwRiskType.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
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
    ' Date :15/07/2000
    '
    ' Edit History :SK
    '
    ' ***************************************************************** '
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdOK.Enabled = Not bDisable
            cmdDefineFields.Enabled = Not bDisable
            CmdShowScreen.Enabled = Not bDisable

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ResizeInterface
    '
    ' Description: Resizes the interface controls.
    '
    ' Date :15/07/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Function ResizeInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Form Width-7950 Height-4785

            If VB6.PixelsToTwipsX(Me.Width) < 7950 Then Me.Width = VB6.TwipsToPixelsX(7950)
            If VB6.PixelsToTwipsY(Me.Height) < 4785 Then Me.Height = VB6.TwipsToPixelsY(4785)

            tabMainTab.Width = Width - VB6.TwipsToPixelsX(375) + 5
            tabMainTab.Height = Height - VB6.TwipsToPixelsY(1110) + 5

            cmdHelp.Left = Width - VB6.TwipsToPixelsX(1320)
            cmdHelp.Top = Height - VB6.TwipsToPixelsY(840)

            cmdCancel.Left = Width - VB6.TwipsToPixelsX(2520)
            cmdCancel.Top = Height - VB6.TwipsToPixelsY(840)

            cmdOK.Left = Width - VB6.TwipsToPixelsX(3720)
            cmdOK.Top = Height - VB6.TwipsToPixelsY(840)

            CmdShowScreen.Left = VB6.TwipsToPixelsX(120)
            CmdShowScreen.Top = Height - VB6.TwipsToPixelsY(840)

            lvwRiskType.Width = Width - VB6.TwipsToPixelsX(2055)

            lvwRiskType.Height = Height - VB6.TwipsToPixelsY(1770)

            cmdDefineFields.Left = tabMainTab.Width - VB6.TwipsToPixelsX(1410)

            'To fix the alignment issue of the button control- Sanjay
            cmdDefineFields.Top = tabMainTab.Height - VB6.TwipsToPixelsY(775)

            ' Alix
            cmdGISScreen.Left = tabMainTab.Width - VB6.TwipsToPixelsX(1410)
            cmdGISScreen.Top = tabMainTab.Height - VB6.TwipsToPixelsY(775)

            Frame1.Left = lvwRiskType.Left + lvwRiskType.Width + VB6.TwipsToPixelsX(40)
            Return result

        Catch
            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function


    Private Sub cmdCloseClaim_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCloseClaim.Click

        Const sFunctionName As String = "cmdCloseClaim_Click"

        Dim sDescription As String = ""
        Dim lItemId, lSelectedItem As Integer

        Dim oRuleEditor As iPMURuleEditor.Interface_Renamed

        Try

            If lvwRiskType.Items.Count > 0 Then

                If Not (lvwRiskType.FocusedItem Is Nothing) Then

                    If GetItemIndex(Convert.ToString(lvwRiskType.FocusedItem.Tag), lItemId) = gPMConstants.PMEReturnCode.PMTrue Then

                        lSelectedItem = lvwRiskType.FocusedItem.Index + 1

                        Dim temp_oRuleEditor As Object
                        m_lReturn = g_oObjectManager.GetInstance(temp_oRuleEditor, sClassName:="iPMURuleEditor.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                        oRuleEditor = temp_oRuleEditor

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object iPMURuleEditor.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGenericRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            Exit Sub
                        End If

                        CType(oRuleEditor, SSP.S4I.Interfaces.ILocalInterface).Initialise()
                        oRuleEditor.FixedFile = False
                        oRuleEditor.RuleFileName = "Claims\" & "RiskType" & CStr(lSelectedItem) & "_close.rul"
                        m_lReturn = oRuleEditor.Start()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iSIRFindParty.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyHolderInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            Exit Sub
                        End If

                        oRuleEditor.Dispose()
                        oRuleEditor = Nothing
                    Else
                        ' couldnt find index of specified row
                    End If

                End If

            End If

        Catch excep As System.Exception

            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)

        Finally
            oRuleEditor = Nothing
        End Try
        Exit Sub
    End Sub

    Private Sub cmdDefineFields_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDefineFields.Click

        'developer guide no. todo list
        Dim oDefineFields As Object

        ' Create Find Party object
        Dim temp_oDefineFields As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oDefineFields, sClassName:="iCLMDefnFlds.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oDefineFields = temp_oDefineFields

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iPMBFindParty.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyHolderInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Exit Sub
        End If

        ' Set component properties and start interface

        oDefineFields.CallingAppName = ACApp

        'TN20010501 Start

        oDefineFields.mode = gPMConstants.PMEComponentAction.PMAdd

        oDefineFields.DataMode = 0 'risk data
        'TN20010501 End
        oDefineFields.Typeid = Convert.ToString(lvwRiskType.FocusedItem.Tag)
        oDefineFields.TypeName = lvwRiskType.FocusedItem.Text

        m_lReturn = oDefineFields.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iSIRFindParty.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyHolderInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Exit Sub
        End If

        ' Destroy Find Party object

        oDefineFields.Dispose()
        oDefineFields = Nothing

    End Sub

    ' ***************************************************************** '
    ' Name: cmdGISScreen_Click
    '
    ' Parameters: n/a
    '
    ' Description: Allows the user to edit the associated Risk Types
    '                GIS Screen
    ' History:
    '           Created : MEvans : 09-04-2003 : CQ 629
    ' ***************************************************************** '
    Private Sub cmdGISScreen_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdGISScreen.Click

        Const sFunctionName As String = "cmdGISScreen_Click"

        Dim ofrmSelect As frmSelectGISScreen
        Dim sDescription As String = ""
        Dim lItemId, lGISScreenId As Integer
        Dim sGISScreenDesc As String = ""
        Dim lSelectedItem As Integer

        Try

            ' if there are items in the list
            If lvwRiskType.Items.Count > 0 Then

                ' if there is a selected item
                If Not (lvwRiskType.FocusedItem Is Nothing) Then

                    ' get the selected items details
                    If GetItemIndex(Convert.ToString(lvwRiskType.FocusedItem.Tag), lItemId) = gPMConstants.PMEReturnCode.PMTrue Then

                        ' create new instance of the form
                        ofrmSelect = New frmSelectGISScreen()

                        ' Load the form
                        'developer guide no. 68 
                        'Load(ofrmSelect)

                        ' get item details
                        lSelectedItem = lvwRiskType.FocusedItem.Index + 1

                        'RVH 8/5/2003 - BUG : ENDVR00001092
                        'START : Check to see if gis screen id is empty
                        If CStr(m_vTotalArray(ACColScreenID, lItemId)).Trim() = "" Then
                            lGISScreenId = 0
                        Else
                            lGISScreenId = gPMFunctions.NullToLong(CStr(m_vTotalArray(ACColScreenID, lItemId)))
                        End If
                        'END

                        sGISScreenDesc = CStr(m_vTotalArray(ACColScreenDesc, lItemId)).Trim()
                        sDescription = CStr(m_vTotalArray(ACColRiskType, lItemId)).Trim()

                        ' Pass values to form
                        ofrmSelect.txtRiskType.Text = sDescription
                        ofrmSelect.lstScreens.Text = sGISScreenDesc
                        ofrmSelect.PrepareInterface()

                        ' Show the form
                        ofrmSelect.ShowDialog()

                        ' read back from the form
                        m_lSelectedScreenID = ofrmSelect.SelectedScreenID
                        m_sSelectedDescription = ofrmSelect.SelectedDescription

                        ' Unload the form
                        ofrmSelect.Close()

                        ' IF user did NOT cancel
                        If m_lSelectedScreenID <> 0 Then

                            ' Update local data
                            m_vTotalArray(ACColScreenID, lItemId) = m_lSelectedScreenID
                            m_vTotalArray(ACColScreenDesc, lItemId) = m_sSelectedDescription

                            ' change the onscreen details
                            ListViewHelper.GetListViewSubItem(lvwRiskType.Items.Item(lSelectedItem - 1), 2).Text = m_sSelectedDescription

                            ' reselect previously highlighted row
                            lvwRiskType.Items.Item(lSelectedItem - 1).Selected = True

                        End If
                    Else
                        ' couldnt find index of specified row
                    End If
                End If
            End If

        Catch excep As System.Exception

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************
        Finally
            ' destroy form reference
            ofrmSelect = Nothing
        End Try
        Exit Sub
    End Sub

    Private Sub CmdShowScreen_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdShowScreen.Click

        Const PMKeyNameViewRiskFlag As String = "view_risk_flag"
        Const PMKeyNameRiskTypeID As String = "risk_type_id"
        'Const PMKeyClaimGISScreenID As String = "GIS_Screen_id"

        'developer guide no. 108
        Dim oGenericRisk As Object
        Dim Var(1, 2) As Object

        Try

            ' Alix
            If ClaimBuilderIsEnable() Then

                '****************
                ' MEvans : 09-04-2003 : CQ 629
                ' This was to be implemented but at the moment there
                ' is no easy way of getting the risk screen to
                ' display without passing in a load of keys that we just dont have...
                '
                '        ' get the selected items details
                '        If GetItemIndex(lvwRiskType.SelectedItem.Tag, lItemId) = PMTrue Then
                '
                '
                '            lRiskTypeId = lvwRiskType.SelectedItem.Tag
                '            lGISScreenId = NullToLong(m_vTotalArray(ACColScreenID, lItemId))
                '
                '            ' Create Risk object
                '            m_lReturn& = g_oObjectManager.GetInstance( _
                ''                oObject:=oGenericRisk, _
                ''                sClassName:="iPMURisk.Interface", _
                ''                vInstanceManager:=PMGetLocalInterface)
                '
                '            If (m_lReturn& <> PMTrue) Then
                '
                '                LogMessage _
                ''                    iType:=PMLogOnError, _
                ''                    sMsg:="Failed to create object '.Interface'.", _
                ''                    vApp:=ACApp, _
                ''                    vClass:=ACClass, _
                ''                    vMethod:="CmdShowScreen_Click", _
                ''                    vErrNo:=Err.Number, _
                ''                    vErrDesc:=Err.Description
                '                Exit Sub
                '            End If
                '
                '            ' init object
                '            m_lReturn& = oGenericRisk.Initialise()
                '
                '            ' Start object
                '            m_lReturn& = oGenericRisk.Start()
                '            If (m_lReturn& <> PMTrue) Then
                '
                '                LogMessage _
                ''                    iType:=PMLogOnError, _
                ''                    sMsg:="Failed to process object 'iPMURisk.Interface'.", _
                ''                    vApp:=ACApp, _
                ''                    vClass:=ACClass, _
                ''                    vMethod:="CmdShowScreen_Click", _
                ''                    vErrNo:=Err.Number, _
                ''                    vErrDesc:=Err.Description
                '                Exit Sub
                '            End If
                '
                '        End If
                '****************

            Else

                ' Create Find Party object
                Dim temp_oGenericRisk As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oGenericRisk, sClassName:="iCLMRiskDetails.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                oGenericRisk = temp_oGenericRisk

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object '.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGenericRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

                Var(0, 0) = PMKeyNameViewRiskFlag
                Var(1, 0) = True
                Var(0, 1) = PMKeyNameRiskTypeID
                Var(1, 1) = Convert.ToString(lvwRiskType.FocusedItem.Tag)
                oGenericRisk.SetKeys(Var)
                oGenericRisk.SetProcessModes(gPMConstants.PMEComponentAction.PMView)

                m_lReturn = oGenericRisk.Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iSIRFindParty.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyHolderInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

            End If

            ' Destroy Find Party object

            oGenericRisk.Dispose()
            oGenericRisk = Nothing

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CmdShowScreen_Click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdShowScreen_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub


    'Private Sub Command1_Click()
    '
    'End Sub



    ' ***************************************************************** '
    ' Name: FormIntialise
    '
    ' Description: Intialise all required details of the form
    '
    ' Date:15/07/00
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iCLMRiskType.General()


            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

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

        'Sanjay
        cmdDefineFields.Top = tabMainTab.Height - VB6.TwipsToPixelsY(300)

    End Sub
    ' ***************************************************************** '
    ' Name:         FormLoad
    ' Description:  Loads all required details of the form
    ' Date:         15/07/00
    ' Edit History: SK
    ' ***************************************************************** '

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

            ' Validate fields using Forms Control

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'Set the UnderWriting/Broking Constant

            m_lSiriusUnderWritingBroking = g_oBackofficelink.Sirius_Product


            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If


            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then


                ' Failed to get the interface details.
                'm_lErrorNumber& = PMFalse

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

        'Below line is to fix the alignment issue of the button control- Sanjay
        cmdDefineFields.Top = tabMainTab.Height - VB6.TwipsToPixelsY(700)

    End Sub
    Private Const vbFormCode As Integer = 0
    ' ***************************************************************** '
    ' Name: Form_Query Unload
    '
    ' Description: Store all Property Details before unloading form
    '
    ' Date:30/08/2000
    '
    ' Edit History:Pandu
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
                m_lReturn = m_oGeneral.ProcessCommand()

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
    ' Name:Form_Resize
    '
    ' Description: Resize the the controls on form
    '
    ' Date:30/08/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            m_lReturn = ResizeInterface()

        Catch


            Exit Sub
        End Try


    End Sub

    ' ***************************************************************** '
    ' Name: lvwRiskType_ColumnClick
    '
    ' Description:Sort the Details of List View as per the column clicked
    '
    ' Date:30/08/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub lvwRiskType_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwRiskType.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwRiskType.Columns(eventArgs.Column)
        Try
            'Commented the below code and handled sorting through below function.
            ListViewFunc.SortListView(lvwRiskType, eventArgs)

            'With lvwRiskType

            '    ' If date column clicked, then sort by date sort column
            '    If ColumnHeader.Index + 1 - 1 = 1 Then
            '        ListViewHelper.SetSortedProperty(lvwRiskType, False)
            '        If ListViewHelper.GetSortKeyProperty(lvwRiskType) <> 1 Then
            '            ListViewHelper.SetSortKeyProperty(lvwRiskType, 1)
            '            ListViewHelper.SetSortOrderProperty(lvwRiskType, SortOrder.Ascending)
            '        Else
            '            ListViewHelper.SetSortOrderProperty(lvwRiskType, (ListViewHelper.GetSortOrderProperty(lvwRiskType) + 1) Mod 2)
            '        End If
            '        ListViewHelper.SetSortedProperty(lvwRiskType, True)

            '        ' If current sort column header is
            '        ' pressed.
            '    ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwRiskType)) Then
            '        ' Set sort order opposite of
            '        ' current direction.
            '        ListViewHelper.SetSortOrderProperty(lvwRiskType, (ListViewHelper.GetSortOrderProperty(lvwRiskType) + 1) Mod 2)
            '        ListViewHelper.SetSortedProperty(lvwRiskType, True)

            '    Else
            '        ' Sort by this column (ascending).
            '        ListViewHelper.SetSortedProperty(lvwRiskType, False)

            '        ' Turn off sorting so that the list
            '        ' is not sorted twice
            '        ListViewHelper.SetSortOrderProperty(lvwRiskType, SortOrder.Ascending)
            '        ListViewHelper.SetSortKeyProperty(lvwRiskType, ColumnHeader.Index + 1 - 1)
            '        ListViewHelper.SetSortedProperty(lvwRiskType, True)
            '    End If
            'End With

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwRiskType_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: lvwRiskType_GotFocus
    '
    ' Description:Set Ok Button a default
    '
    ' Date:30/08/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub lvwRiskType_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRiskType.Enter
        ' GotFocus Event for the search details


        'Try 
        '
        ' Unset any default buttons so can select with Enter key.
        '    cmdFindNow.Default = False
        'cmdOK.Default = False
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error.
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwRiskType_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        '
        'End Try


    End Sub

    Private Sub lvwRiskType_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRiskType.DoubleClick
        If cmdGISScreen.Visible Then
            cmdGISScreen_Click(cmdGISScreen, New EventArgs())
        Else
            cmdDefineFields_Click(cmdDefineFields, New EventArgs())
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: lvwRiskType_LostFocus
    '
    ' Description:Set find now as default
    '
    ' Date:30/08/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub lvwRiskType_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRiskType.Leave
        ' LostFocus Event for the search details

        Try

            ' Set the default button.
            VB6.SetDefault(cmdOK, True)

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwRiskType_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: cmdOK_Click
    '
    ' Description:Set Properties of the form on clicking OK Button from the
    '               relevant list item under focus or clicked
    '
    ' Date:30/08/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Alix - Save Risk Type selected screen to DB
            If ClaimBuilderIsEnable() Then
                If Information.IsArray(m_vTotalArray) Then
                    For lRow As Integer = m_vTotalArray.GetLowerBound(1) To m_vTotalArray.GetUpperBound(1)
                        m_lReturn = g_oBusiness.UpdateRiskType(v_lRiskTypeId:=Conversion.Val(CStr(m_vTotalArray(ACColRiskTypeID, lRow))), v_lScreenID:=Conversion.Val(CStr(m_vTotalArray(ACColScreenID, lRow))))
                    Next lRow
                End If
            End If

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

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
    ' ***************************************************************** '
    ' Name: cmdCancel_Click
    '
    ' Description:Unload the Form
    '
    ' Date:30/08/2000
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

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

    ' ***************************************************************** '
    ' Name: DisplayMessage
    '
    ' Description: Display the Suitable Message
    '
    ' Date:30/08/2000
    '
    ' Edit History:Pandu

    ' ***************************************************************** '

    'Private Sub DisplayMessage(ByRef MessageConstant As Integer, ByRef sTitle As String)
    '
    'Static sMessage As String = ""
    '
    'Try 
    '
    '

    'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, MessageConstant, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '
    '
    ' Display the status message.
    '
    'MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    '**********************************************************************
    ' Function Name:    ClaimBuilderIsEnable
    ' Author:           Alix Bergeret
    ' Date:             16/10/2001
    ' Description:      Check if SIROPTClaimsBuilder product option is ON
    '**********************************************************************
    Private Function ClaimBuilderIsEnable() As Boolean

        Dim result As Boolean = False
        Dim vResult As Object

        Try

            result = True
            'developer guide no.98
            iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTClaimsBuilder, v_vBranch:=g_iSourceID, r_vUnderwriting:=vResult)


            Return gPMFunctions.ToSafeInteger(vResult) = 1

        Catch
        End Try



        Return False

    End Function


    ' ***************************************************************** '
    ' Name: GetItemIndex
    '
    ' Parameters: n/a
    '
    ' Description: Returns the position in the array of the required
    '               item...
    '
    ' History:
    '           Created : MEvans : 09-04-2003 : CQ 629
    ' ***************************************************************** '
    Private Function GetItemIndex(ByVal v_lRiskTypeId As Integer, ByRef r_lArrayIndex As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetItemIndex"

        Dim llBound, lUBound As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Is we have an array
            If Information.IsArray(m_vTotalArray) Then

                ' get array boundaries
                llBound = m_vTotalArray.GetLowerBound(1)
                lUBound = m_vTotalArray.GetUpperBound(1)

                ' for each item in the array
                For lItem As Integer = llBound To lUBound

                    ' find the record we want
                    If CInt(m_vTotalArray(ACColRiskTypeID, lItem)) = v_lRiskTypeId Then
                        ' return the array items position
                        r_lArrayIndex = lItem
                        Exit For
                    End If

                Next lItem

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    Private Sub tabMainTab_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabMainTab.KeyDown
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
            tabMainTab.Focus()
        End If
    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(objCnt:=cmdHelp, lContextID:=ScreenHelpID)
    End Sub
End Class