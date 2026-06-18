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
    ' Date: 30/04/2003
    ' Description: Main interface.
    ' Edit History: Cerated by Alix Bergeret
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lTaxGroupId As Integer
    Private m_sUniqueId As String
    Private m_sScreenHierarchy As String

    Private m_sDescription As String = ""

    'Variables to store data taken from the List View
    Private m_iAction As gPMConstants.PMEComponentAction

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUTaxGroupBands.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' Stores the search data from the business object.
    Private m_vTaxGroupTaxBands(,) As Object

    ' Alix
    Private m_lSelectedTaxBandID As Integer
    '020506 Datasure
    Private m_sIsUnderwritngOrAgency As String = ""

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


    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    ' Set the interface exit status.
    'm_lStatus = Value
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

    Public WriteOnly Property TaxGroupId() As Integer
        Set(ByVal Value As Integer)
            m_lTaxGroupId = Value
        End Set
    End Property

    Public WriteOnly Property UniqueId() As String
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public WriteOnly Property ScreenHierarchy() As String
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
        End Set
    End Property

    Public WriteOnly Property Description() As String
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property


    ' ***************************************************************** '
    ' Sets the rules for validating fields.
    ' ***************************************************************** '
    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSequence, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RDC 25102005 new field
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAllocationSequence, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger)
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
    ' Updates details with selected values
    ' ***************************************************************** '
    Private Function DataToDetail() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the Detail details.

            lSelectedItem = Convert.ToString(lvwTaxBands.Items.Item(lvwTaxBands.FocusedItem.Index).Tag)
            m_lSelectedTaxBandID = CInt(m_vTaxGroupTaxBands(ACPTaxBandID, lSelectedItem))

            ' tax band
            cboTaxband.DefaultItemId = m_lSelectedTaxBandID
            cboTaxband.RefreshList()
            ' sequence
            m_lReturn = m_oFormFields.FormatControl(txtSequence, m_vTaxGroupTaxBands(ACPSequence, lSelectedItem))

            ' RDC 25102005
            txtAllocationSequence.Text = CStr(m_vTaxGroupTaxBands(ACPAllocSequence, lSelectedItem))
            lblAllocationSequence.Visible = True
            txtAllocationSequence.Visible = True
            lblSequence.Visible = True
            optRule(gPMFunctions.ToSafeInteger(CInt(m_vTaxGroupTaxBands(ACPAllocRule, lSelectedItem)))).Checked = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Detail details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Clears details
    ' ***************************************************************** '
    Private Function ClearDetail() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default to first band
            cboTaxband.ItemId = 1

            ' Default to 1 (no sequencing) to mimic original behaviour
            m_lReturn = m_oFormFields.FormatControl(txtSequence, 1)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Detail details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Stores new or updated details
    ' ***************************************************************** '
    Private Function DataRefresh() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem, lTaxBandId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the property members.
            Select Case m_iAction
                Case gPMConstants.PMEComponentAction.PMAdd
                    If Information.IsArray(m_vTaxGroupTaxBands) Then
                        lSelectedItem = m_vTaxGroupTaxBands.GetUpperBound(1) + 1
                        ReDim Preserve m_vTaxGroupTaxBands(ACPMaxArray, lSelectedItem)
                    Else
                        lSelectedItem = 0
                        ReDim m_vTaxGroupTaxBands(ACPMaxArray, lSelectedItem)
                    End If

                    lTaxBandId = cboTaxband.ItemId

                Case gPMConstants.PMEComponentAction.PMEdit

                    lSelectedItem = Convert.ToString(lvwTaxBands.Items.Item(lvwTaxBands.FocusedItem.Index).Tag)
                    lTaxBandId = cboTaxband.ItemId

                Case gPMConstants.PMEComponentAction.PMDelete

                    lSelectedItem = Convert.ToString(lvwTaxBands.Items.Item(lvwTaxBands.FocusedItem.Index).Tag)
                    lTaxBandId = 0
            End Select

            m_vTaxGroupTaxBands(ACPTaxGroupID, lSelectedItem) = m_lTaxGroupId
            m_vTaxGroupTaxBands(ACPTaxBandID, lSelectedItem) = lTaxBandId
            m_vTaxGroupTaxBands(ACPDescription, lSelectedItem) = cboTaxband.ItemCaption
            m_vTaxGroupTaxBands(ACPSequence, lSelectedItem) = cboSequence.Text ' m_oFormFields.UnformatControl(txtSequence)


            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_vTaxGroupTaxBands(ACPAllocRule, lSelectedItem) = iPMFunc.GetOptionValue(optRule)
                'm_vTaxGroupTaxBands(ACPAllocSequence, lSelectedItem) = gPMFunctions.ToSafeInteger(CInt(txtAllocationSequence.Text))
                m_vTaxGroupTaxBands(ACPAllocSequence, lSelectedItem) = gPMFunctions.ToSafeInteger(txtAllocationSequence.Text)
            End If

            m_lReturn = BusinessToInterface()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Refresh Refreshs from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataRefresh", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Ensure tax band is only selected once
    ' ***************************************************************** '
    Private Function ValidatePolicyShare() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Validate Details.
            If cboTaxband.ItemId = -1 Then
                MessageBox.Show("Please Enter Valid Tax Band", "Tax Band", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_iAction = gPMConstants.PMEComponentAction.PMAdd Or m_iAction = gPMConstants.PMEComponentAction.PMEdit Then
                If Information.IsArray(m_vTaxGroupTaxBands) Then
                    For lRow As Integer = m_vTaxGroupTaxBands.GetLowerBound(1) To m_vTaxGroupTaxBands.GetUpperBound(1)
                        If cboTaxband.ItemId = CDbl(m_vTaxGroupTaxBands(ACPTaxBandID, lRow)) Then
                            ' Alix - In Edit mode, we should be able to reselect the same item
                            If m_iAction = gPMConstants.PMEComponentAction.PMAdd Or (m_iAction = gPMConstants.PMEComponentAction.PMEdit And m_lSelectedTaxBandID <> cboTaxband.ItemId) Then
                                MessageBox.Show("This Tax Band is already selected", "Tax Band", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    Next lRow
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Refresh Refreshs from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidatePolicyShare", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Retrieves the details from the business object.
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get all tax band for this tax group

            m_oBusiness.TaxGroupId = m_lTaxGroupId

            m_lReturn = m_oBusiness.GetTaxGroupTaxBands(r_vTaxGroupTaxBands:=m_vTaxGroupTaxBands)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
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
    ' Updates all interface details from the business object.
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details from the business object to the data storage.
            m_lReturn = BusinessToData()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details to the interface.
            pnlTaxGroup.Text = m_sDescription
            lvwTaxBands.Items.Clear()

            If Information.IsArray(m_vTaxGroupTaxBands) Then
                For lRow As Integer = m_vTaxGroupTaxBands.GetLowerBound(1) To m_vTaxGroupTaxBands.GetUpperBound(1)
                    If CDbl(m_vTaxGroupTaxBands(ACPTaxBandID, lRow)) <> 0 Then
                        ' Assign the details to the first column
                        With lvwTaxBands.Items.Add(CStr(m_vTaxGroupTaxBands(ACPDescription, lRow)).Trim())
                            ' Sequence
                            lvwTaxBands.Items(lvwTaxBands.Items.Count - 1).SubItems.Add(CStr(gPMFunctions.ToSafeInteger(CInt(m_vTaxGroupTaxBands(ACPSequence, lRow)))))
                            'Rule
                            lvwTaxBands.Items(lvwTaxBands.Items.Count - 1).SubItems.Add(CStr(Interaction.Choose(gPMFunctions.ToSafeInteger(CInt(m_vTaxGroupTaxBands(ACPAllocRule, lRow))) + 1, "Before Premium", "With Premium", "After Premium")))
                            ' Allocation Sequence
                            If gPMFunctions.ToSafeInteger(CInt(m_vTaxGroupTaxBands(ACPAllocRule, lRow))) <> 1 Then
                                lvwTaxBands.Items(lvwTaxBands.Items.Count - 1).SubItems.Add(gPMFunctions.ToSafeInteger(m_vTaxGroupTaxBands(ACPAllocSequence, lRow)))
                            Else
                                lvwTaxBands.Items(lvwTaxBands.Items.Count - 1).SubItems.Add("")
                            End If

                            ' Set the tag property with the index of the search data storage.
                            lvwTaxBands.Items(lvwTaxBands.Items.Count - 1).Tag = CStr(lRow)
                        End With

                        ' Refresh the first X amount of rows, to allow the user to see the results instantly.
                        If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                            ' Select the first item and refresh list
                            lvwTaxBands.Items.Item(0).Selected = True
                            lvwTaxBands.Refresh()
                        End If
                    End If
                Next lRow
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Updates all business members from the interface details.
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oBusiness.TaxGroupId = m_lTaxGroupId


            m_lReturn = m_oBusiness.Update(v_vTaxGroupTaxBands:=m_vTaxGroupTaxBands, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)
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

    ' ***************************************************************** '
    ' Updates the data storage from the business object.
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Updates the data storage from the interface details.
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Sets all of the interface default values.
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            'Display the ListView Tab
            tabDetailTab.Visible = False
            tabMainTab.Visible = True

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the column widths for the search list.
            lvwTaxBands.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(3000))
            lvwTaxBands.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(1400))

            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
            cmdAdd.Enabled = (Task <> gPMConstants.PMEComponentAction.PMView)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Display all language specific captions.
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

            ' Command buttons

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Tabs

            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabDetailTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Controls

            lblTaxGroup.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCTaxGroup, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTaxBand.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCTaxBand, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSequence.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCSequence, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Column headers


            lvwTaxBands.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHTaxband, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwTaxBands.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHSequence, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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

        tabMainTab.Visible = False
        tabDetailTab.Visible = True

        '020506 Datasure
        m_lReturn = iPMFunc.getUnderwritingOrAgency(m_sIsUnderwritngOrAgency)
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            lblRule.Visible = True
            optRule(0).Visible = True
            optRule(1).Visible = True
            optRule(2).Visible = True
            lblAllocationSequence.Visible = True
            txtAllocationSequence.Visible = True
            lblSequence.Visible = True
        End If


        PopulateSequence(gPMConstants.PMEComponentAction.PMAdd)

        m_lReturn = ClearDetail()



    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click


        m_iAction = gPMConstants.PMEComponentAction.PMDelete


        Dim lSelectedItem As Integer = Convert.ToString(lvwTaxBands.Items.Item(lvwTaxBands.FocusedItem.Index).Tag)

        m_vTaxGroupTaxBands(ACPTaxBandID, lSelectedItem) = 0

        cmdCancel.Enabled = True
        cmdAdd.Enabled = True
        cmdOK.Enabled = True
        cmdDelete.Enabled = False
        cmdEdit.Enabled = False

        m_lReturn = DataRefresh()

    End Sub

    Private Sub cmdDetailCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailCancel.Click

        tabDetailTab.Visible = False
        tabMainTab.Visible = True
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            cmdOK.Enabled = True
        End If

    End Sub

    Private Sub cmdDetailOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailOK.Click

        m_lReturn = ValidatePolicyShare()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        tabDetailTab.Visible = False
        tabMainTab.Visible = True
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True
        cmdOK.Enabled = True

        m_lReturn = DataRefresh()

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        m_iAction = gPMConstants.PMEComponentAction.PMEdit

        cmdOK.Enabled = False
        cmdCancel.Enabled = False

        tabMainTab.Visible = False
        tabDetailTab.Visible = True

        PopulateSequence(gPMConstants.PMEComponentAction.PMEdit)

        m_lReturn = DataToDetail()

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, lContextID:=MainModule.ScreenHelpID)

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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRTaxGroupBands.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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
            m_oGeneral = New iPMUTaxGroupBands.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

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
            'Developer Guide No. 220
            Me.cboTaxband.FirstItem = ""
            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' RDC 25102005
            tabDetailTab.Top = tabMainTab.Top
            tabDetailTab.Left = tabMainTab.Left

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

            m_oBusiness.TaxGroupId = m_lTaxGroupId

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

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


            If UnloadMode <> vbFormCode Then
                'Process the next set of actions depending
                'upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                'Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                    'Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
		m_oGeneral.Dispose()

            ' Check for errors.

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

            ' Check for errors.

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



    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim Msg As String = ""
        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
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

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
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

    Private Sub lvwTaxBands_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwTaxBands.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwTaxBands.Columns(eventArgs.Column)

        Static lOrder As SortOrder
        Static lLastCol As Integer

        If lLastCol <> ColumnHeader.Index + 1 Then
            lLastCol = ColumnHeader.Index + 1
            lOrder = SortOrder.Ascending
        Else
            lOrder = IIf(lOrder = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending)
        End If

        Select Case lLastCol
            Case 1
                ListViewHelper.SetSortedProperty(lvwTaxBands, False)
                ListViewHelper.SetSortKeyProperty(lvwTaxBands, lLastCol - 1)
                ListViewHelper.SetSortOrderProperty(lvwTaxBands, lOrder)
                ListViewHelper.SetSortedProperty(lvwTaxBands, True)
            Case 2
                'Developer Guide No. 178
                ListView6Func.ListViewSortByValue(lvwTaxBands, lLastCol - 1, lOrder, False, True)
        End Select
    End Sub

    Private Sub lvwTaxBands_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwTaxBands.DoubleClick
        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If Not (lvwTaxBands.FocusedItem Is Nothing) Then
                cmdEdit_Click(cmdEdit, New EventArgs())
            End If
        End If
    End Sub

    Private Sub lvwTaxBands_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwTaxBands.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        Dim x As Single = (eventArgs.X)
        Dim y As Single = (eventArgs.Y)
        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If lvwTaxBands.GetItemAt(x, y) Is Nothing Then
                cmdDelete.Enabled = False
                cmdEdit.Enabled = False
            Else
                cmdDelete.Enabled = True
                cmdEdit.Enabled = True
            End If
        End If

    End Sub

    ' ***************************************************************** '
    ' Name: PopulateSequence
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 07-03-2005 : PN19163
    ' ***************************************************************** '
    Public Function PopulateSequence(ByVal v_iTask As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateSequence"

        Dim lReturn, lItems, lSelectedIndex, lSelectedSequence, lSelectedItem As Integer

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        ' if in add mode
        If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
            ' set maximum sequence number = list items + 1
            lItems = lvwTaxBands.Items.Count + 1
            ' default sequence to next in logical sequence
            lSelectedSequence = lItems
        Else

            'lSelectedItem = Convert.ToString(lvwTaxBands.Items.Item(lvwTaxBands.FocusedItem.Index).Tag)
            lSelectedItem = Convert.ToString(lvwTaxBands.Items.Item(lvwTaxBands.SelectedItems(0).Index).Tag)
            lSelectedSequence = CInt(m_vTaxGroupTaxBands(ACPSequence, lSelectedItem))
            lItems = lvwTaxBands.Items.Count
        End If

        ' clear combo
        cboSequence.Items.Clear()

        lSelectedIndex = -1

        ' for each possible sequence
        For lItem As Integer = 1 To lItems

            ' add item to combo
            cboSequence.Items.Add(CStr(lItem))

            ' if this item matches the selected one
            ' save the index so we can select it
            ' after the list is fully populated
            If lItem = lSelectedSequence Then
                lSelectedIndex = lItem - 1
            End If

        Next

        If v_iTask = gPMConstants.PMEComponentAction.PMEdit Then
            If lSelectedIndex = -1 Then
                cboSequence.Items.Add(CStr(lSelectedSequence))
                lSelectedIndex = cboSequence.Items.Count - 1
            End If
        End If

        ' select the specified item
        cboSequence.SelectedIndex = lSelectedIndex


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

'        Return result
'        Resume
'        Return result
        End Try
        Return result
    End Function


    Private isInitializingComponent As Boolean
    Private Sub optRule_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optRule_2.CheckedChanged, _optRule_1.CheckedChanged, _optRule_0.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            ' Only enable sequence when rule is before or after premium
            lblAllocationSequence.Enabled = Not optRule(1).Checked
            txtAllocationSequence.Enabled = Not optRule(1).Checked
        End If
    End Sub

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            If tabMainTab.Visible = True Then
                tabMainTab.SelectedIndex = 0
            End If
        End If

        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D2 Then
            If tabDetailTab.Visible = True Then
                tabDetailTab.SelectedIndex = 0
            End If
        End If
    End Sub
End Class
