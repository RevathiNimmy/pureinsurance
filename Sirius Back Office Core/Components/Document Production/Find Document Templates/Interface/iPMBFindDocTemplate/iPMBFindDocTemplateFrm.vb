Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms

Imports SharedFiles

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    Private Const ACClass As String = "frmInterface"


    Private Const vbFormCode As Integer = 0

    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_lNavigate As Integer
    Private m_lProcessMode As gPMConstants.PMEComponentAction
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lDocumentTemplateId As Integer
    Private m_lDocumentTypeId As Integer
    Private m_sDocumentCode As String = ""
    Private m_sDocumentTypeCode As String = ""
    Private m_sDocumentTypeDescription As String = "" 'MKW 281003 PN7287 1.8.5 to 1.8.6 catchup
    'RWH(06/09/2000) Added description as property.
    Private m_sDocumentTemplateDescription As String = ""

    Private m_lMode As Integer

    Private m_lSourceId As Integer

    Private m_sDelete As String = ""
    Private m_sUnDelete As String = ""

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBFindDocTemplate.General

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Declare an instance of the Lock object.

    Private m_oPMLock As bPMLock.User

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the search data from the business object.
    Private m_vSearchData(,) As Object
    Private m_oDocTemplate As iPMBDocTemplate.Interface_Renamed
    Private m_lInsuranceFileCnt As Integer
    Private m_lProcessType As Integer

    Private m_lRiskTypeId As Integer

    Private m_lProductId As Integer

    Private m_lGISPropertyID As Integer
    Private m_lGISObjectID As Integer

    Private m_sUnderwritingOrAgency As String = ""

    Private m_vSourceArray As Object 'MKW190903 PN6943
    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean

    Private m_lEmailDocumentID As Integer 'PN:62429 By Upendra

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

    Public Property GISPropertyID() As Integer
        Get
            Return m_lGISPropertyID
        End Get
        Set(ByVal Value As Integer)
            m_lGISPropertyID = Value
        End Set
    End Property

    Public Property GISObjectID() As Integer
        Get
            Return m_lGISObjectID
        End Get
        Set(ByVal Value As Integer)
            m_lGISObjectID = Value
        End Set
    End Property

    Public Property DocumentTypeDescription() As String
        Get
            Return m_sDocumentTypeDescription
        End Get
        Set(ByVal Value As String)
            m_sDocumentTypeDescription = Value
        End Set
    End Property

    'MKW190903 PN6943
    Public WriteOnly Property SourceArray() As Object
        Set(ByVal Value As Object)
            m_vSourceArray = Value
        End Set
    End Property

    Public WriteOnly Property ProcessType() As Integer
        Set(ByVal Value As Integer)
            m_lProcessType = Value
        End Set
    End Property

    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public WriteOnly Property RiskTypeId() As Integer
        Set(ByVal Value As Integer)
            m_lRiskTypeId = Value
        End Set
    End Property

    Public WriteOnly Property ProductId() As Integer
        Set(ByVal Value As Integer)
            m_lProductId = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get
            Return m_lErrorNumber
        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
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

    Public Property DocumentTemplateId() As Integer
        Get
            Return m_lDocumentTemplateId
        End Get
        Set(ByVal Value As Integer)
            m_lDocumentTemplateId = Value
        End Set
    End Property

    Public Property DocumentTemplateDescription() As String
        Get
            Return m_sDocumentTemplateDescription
        End Get
        Set(ByVal Value As String)
            m_sDocumentTemplateDescription = Value
        End Set
    End Property

    Public Property DocumentTypeId() As Integer
        Get
            Return m_lDocumentTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lDocumentTypeId = Value
        End Set
    End Property

    Public Property DocumentCode() As String
        Get
            Return m_sDocumentCode
        End Get
        Set(ByVal Value As String)
            m_sDocumentCode = Value
        End Set
    End Property

    Public Property DocumentTypeCode() As String
        Get
            Return m_sDocumentTypeCode
        End Get
        Set(ByVal Value As String)
            m_sDocumentTypeCode = Value
        End Set
    End Property

    Public Property Mode() As Integer
        Get
            Return m_lMode
        End Get
        Set(ByVal Value As Integer)
            m_lMode = Value
        End Set
    End Property

    Public Property SourceId() As Integer
        Get
            Return m_lSourceId
        End Get
        Set(ByVal Value As Integer)
            m_lSourceId = Value
        End Set
    End Property
   
    Public Function GetBusiness() As Integer
        Dim result As Integer = 0
        Dim bIncludeDeletedTemplates, bIncludeSubDocuments As Boolean ' RAM20050107 : Added for Sub Document Support
        Dim v_Return As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bIncludeDeletedTemplates = True
            If m_lProcessMode = gPMConstants.PMEComponentAction.PMView Then
                'check product options
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROOPTNoDeletedTempates, v_vBranch:=1, r_vUnderwriting:=v_Return)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'If error ignore, as not serious contined function as previous.
                    bIncludeDeletedTemplates = True
                Else
                    If v_Return = "1" Then
                        bIncludeDeletedTemplates = False
                    End If
                End If
            End If

            ' Get the details from the business object.

            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while
            ' a search is in progress.
            m_lReturn = DisableInterface(bDisable:=True)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the details from the business object.
            g_oBusiness.SourceId = m_lSourceId

            If m_sCallingAppName = "ClientManager" Then

                g_oBusiness.ViaClientManager = True
            End If

            If m_lProductId = 0 Then
                If m_lRiskTypeId = 0 Then
                    If cboType.SelectedIndex > 0 Then
                        m_lReturn = g_oBusiness.SearchByQuery(r_vResultArray:=m_vSearchData, v_vDocumentCode:=txtCode.Text.Trim(), v_vDocumentTypeId:=VB6.GetItemData(cboType, cboType.SelectedIndex), v_vValidSourceArray:=m_vSourceArray, v_bIncludeDeleted:=bIncludeDeletedTemplates, v_lExcludeCopyTempalte:=0, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(txtEffectiveDate.Text))
                    Else
                        If VB6.GetItemData(cboType, cboType.SelectedIndex) = 0 Then
                            If m_lMode = gSIRLibrary.ACNormalMode Then
                                bIncludeSubDocuments = True
                            End If
                        End If

                        m_lReturn = g_oBusiness.SearchByQuery(r_vResultArray:=m_vSearchData, v_vDocumentCode:=txtCode.Text.Trim(), v_vValidSourceArray:=m_vSourceArray, v_bIncludeDeleted:=bIncludeDeletedTemplates, v_bIncludeSubDocuments:=bIncludeSubDocuments, v_lExcludeCopyTempalte:=0, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(txtEffectiveDate.Text))
                    End If
                Else

                    m_lReturn = g_oBusiness.SearchByRiskType(r_vResultArray:=m_vSearchData, v_lRiskTypeId:=m_lRiskTypeId, v_vDocumentCode:=txtCode.Text.Trim(), lGISPropertyID:=m_lGISPropertyID, lGISObjectID:=m_lGISObjectID, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(txtEffectiveDate.Text))

                    If Information.IsArray(m_vSearchData) Then
                        m_lDocumentTypeId = gPMFunctions.ToSafeLong(CStr(m_vSearchData(ACIDocumentTypeId, 0)))

                        If m_lDocumentTypeId <> 0 Then

                            ' Get the related Listindex for the supplied m_lDocumentTypeID
                            For iCounter As Integer = 0 To cboType.Items.Count - 1
                                If VB6.GetItemData(cboType, iCounter) = m_lDocumentTypeId Then
                                    cboType.SelectedIndex = iCounter
                                    Exit For
                                End If
                            Next iCounter

                        End If
                    End If

                End If
            Else

                m_lReturn = g_oBusiness.SearchByproduct(r_vResultArray:=m_vSearchData, v_lProductId:=m_lProductId)
            End If

            'Assign Values to Interface
            m_lReturn = DataToInterface()

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
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
            End Select

            ' Display the number of item found message.
            DisplayStatusFound()

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


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
        Dim lCount As gPMConstants.PMEFormatStyle
        'Dim sClientTypeDesc As String

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

            lCount = gPMConstants.PMEFormatStyle.PMFormatString

            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1 Code
                If (m_lMode = gSIRLibrary.ACNormalMode) Or (CDbl(m_vSearchData(ACIDocumentIsDeleted, lRow)) = 0) Then


                    oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACIDocumentTemplateCode, lRow)).Trim(), ACFindImage)

                    If CDbl(m_vSearchData(ACIDocumentIsDeleted, lRow)) = 1 Then


                        'oListItem.Ghosted = True
                        oListItem.ForeColor = Color.Gray
                    End If

                    ' Assign details to the other columns

                    ' Column 2 Description
                    oListItem.SubItems.Add(1).Text = CStr(m_vSearchData(ACIDocumentTemplateDescription, lRow)).Trim()

                    ' Column 3 Type
                    oListItem.SubItems.Add(2).Text = CStr(m_vSearchData(ACIDocumentTypeCode, lRow)).Trim()

                    ' Column 4 Effective Date
                    oListItem.SubItems.Add(3).Text = DateTime.Parse(gPMFunctions.ToSafeDate(CStr(m_vSearchData(ACIDocumentEffectiveDate, lRow)))).ToString("d")

                    ' {* USER DEFINED CODE (End) *}

                    ' Set the tag property with the index of
                    ' the search data storage.
                    oListItem.Tag = CStr(lRow)

                    lCount = CType(lCount + 1, gPMConstants.PMEFormatStyle)

                    ' Refresh the first X amount of rows, to
                    ' allow the user to see the results instantly.
                    If lCount = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                        ' Select the first item.
                        lvwSearchDetails.Items.Item(0).Selected = True

                        ' Refresh the initial results.
                        lvwSearchDetails.Refresh()
                    End If
                End If
            Next lRow

            ' Enable the interface now that the search
            ' has completed.
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

            If m_lStatus = gPMConstants.PMEComponentAction.PMAdd Then
                If cboType.SelectedIndex < 1 Then
                    m_lDocumentTypeId = 0
                Else
                    m_lDocumentTypeId = CInt(m_vLookupDetails(0, cboType.SelectedIndex - 1))
                End If
                Return result
            End If

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.
            If lvwSearchDetails.FocusedItem Is Nothing Then
                lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(0).Tag)
            Else
                lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)
            End If

            m_lDocumentTemplateId = CInt(m_vSearchData(ACIDocumentTemplateId, lSelectedItem))
            m_sDocumentCode = CStr(m_vSearchData(ACIDocumentTemplateCode, lSelectedItem)).Trim()
            m_lDocumentTypeId = CInt(m_vSearchData(ACIDocumentTypeId, lSelectedItem))
            m_sDocumentTypeCode = CStr(m_vSearchData(ACIDocumentTypeCode, lSelectedItem)).Trim()
            m_sDocumentTemplateDescription = (CStr(m_vSearchData(ACIDocumentTemplateDescription, lSelectedItem)))

            m_sDocumentTypeDescription = (CStr(m_vSearchData(ACIDocumentTypeDescription, lSelectedItem)))
            m_dtEffectiveDate = gPMFunctions.ToSafeDate(CStr(m_vSearchData(ACIDocumentEffectiveDate, lSelectedItem)))

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Static bCalled As Boolean

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            If bCalled Then
                Return result
            End If

            bCalled = True

            m_lReturn = GetLookupValues()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get all of the lookup details.

            cboType.Items.Clear()
            cboType.Items.Insert(0, "(All)")

            m_lReturn = GetLookupDetails(sLookupTable:="document_type", ctlLookup:=cboType)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

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

            If m_lProcessMode = gPMConstants.PMEComponentAction.PMView Then
                cmdEdit.Visible = False
                cmdDelete.Visible = False
                cmdNew.Visible = False
            Else
                ' Position Edit and New controls
                If Not cmdNavigate.Visible Then
                    cmdEdit.Left = cmdNavigate.Left
                Else
                    cmdEdit.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdNavigate.Left) + VB6.PixelsToTwipsX(cmdNavigate.Width) + 105)
                End If

                cmdNew.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdEdit.Left) + VB6.PixelsToTwipsX(cmdEdit.Width) + 105)
                cmdDelete.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdNew.Left) + VB6.PixelsToTwipsX(cmdNew.Width) + 105)

                ' Disable until a template is selected
                cmdEdit.Enabled = False
                cmdDelete.Enabled = False
            End If
            'DC100603 -end

            cmdNew.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdEdit.Left) + VB6.PixelsToTwipsX(cmdEdit.Width) + 105)
            cmdDelete.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdNew.Left) + VB6.PixelsToTwipsX(cmdNew.Width) + 105)

            ' Disable until a template is selected
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False

            If m_lMode = gSIRLibrary.ACMergeMode Then
                cmdEdit.Visible = False
                cmdNew.Visible = False
                cmdDelete.Visible = False
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lvwSearchDetails.FullRowSelect = True

            'May need to be a little more clever...
            If m_lRiskTypeId <> 0 Then
                m_lDocumentTypeId = lCLAUSE_TYPE_ID
            End If

            If m_lProcessMode = gPMConstants.PMEComponentAction.PMView And (m_lDocumentTypeId = lCLAUSE_TYPE_ID Or m_lDocumentTypeId = lEMAIL_TYPE_ID) Then
                cboType.Enabled = False
            End If

            ' Display all of the lookup details.
            m_lReturn = DisplayLookupDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the column widths for the search list.
            lvwSearchDetails.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1600))
            lvwSearchDetails.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(3600))
            lvwSearchDetails.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(850))

            ' CTAF 160600
            'PN:62429 By Upendra 'PN:62429 By Upendra
            If m_lDocumentTypeId <> 0 Then
                For iCounter As Integer = 0 To cboType.Items.Count - 1
                    If (m_lDocumentTypeId = lEMAIL_TYPE_ID) And m_lEmailDocumentID > 0 Then
                        If VB6.GetItemData(cboType, iCounter) = m_lEmailDocumentID Then
                            cboType.SelectedIndex = iCounter
                            Exit For
                        End If
                    Else
                        If VB6.GetItemData(cboType, iCounter) = m_lDocumentTypeId Then
                            cboType.SelectedIndex = iCounter
                            If m_sCallingAppName = "iPMUProduct" Then ' PN 62193
                                cboType.DropDownStyle = ComboBoxStyle.DropDownList
                            End If
                            Exit For
                        End If
                    End If
                Next iCounter
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
            ' Check if the user still wishes to clear
            ' the interface.
            If bConfirm Then
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                ' Display the message.
                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    ' Don't continue with the clear.
                    Return result
                End If
            End If

            m_vSearchData = Nothing
            ' Clear the search list details.
            lvwSearchDetails.Items.Clear()

            ' Clear the search status bar.
            _stbStatus_Panel1.Text = ""

            ' Set to the first tab.
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            'MKR Issue No. 12545 01/07/2004
            'Clearing the Search Details from the Combo Box and the Text Box
            If cboType.Items.Count > 0 Then
                cboType.SelectedIndex = 0
            End If
            txtEffectiveDate.Text = ""
            txtCode.Text = ""

            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            m_lReturn = DisableInterface(bDisable:=True)

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
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
            ReDim m_ctlTabFirstLast(1, 0)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            m_ctlTabFirstLast(ACControlStart, 0) = txtCode
            m_ctlTabFirstLast(ACControlEnd, 0) = txtCode

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
            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdNew.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdFindNow.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindNowButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdNewSearch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewSearchButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            m_sDelete = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdDelete.Text = m_sDelete
            m_sUnDelete = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACUnDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            lvwSearchDetails.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchDetails.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchDetails.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchDetails.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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
    ' ***************************************************************** '
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            cmdOK.Enabled = Not bDisable
            'cmdEdit.Enabled = Not bDisable

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        ' TF131298
        ' Not used at present, but leave in as lookup boxes suppressed, not deleted

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    ' Edit History  :
    ' RAM20050107   : Code added to Support SUB Documents
    ' ***************************************************************** '

    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1
        Const ACDetailCode As Integer = 2

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.
            Dim newIndex As Integer = -1
            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.
                'RWH(08/08/2000) Process 12 - Only display 'Clause' option in maintenance mode.
                'Rejigged slightly by Tom
                If Not ((CStr(m_vLookupDetails(ACDetailCode, lCntr)).Trim() = CLAUSES_TYPE_CODE And m_sUnderwritingOrAgency <> "A") And (m_lMode = gSIRLibrary.ACMergeMode)) Or (m_lDocumentTypeId = lCLAUSE_TYPE_ID And m_sUnderwritingOrAgency <> "A") Then

                    If Not ((m_lMode = gSIRLibrary.ACMergeMode) And (CStr(m_vLookupDetails(ACDetailCode, lCntr)).Trim() = SUBDOC_TYPE_CODE)) Then



                        newIndex = ctlLookup.Items.Add(New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr), m_vLookupDetails(ACDetailKey, lCntr)))
                        If (CStr(m_vLookupDetails(2, lCntr)).Trim() = "EMAIL") And (m_lDocumentTypeId = lEMAIL_TYPE_ID) Then 'PN:62429 By Upendra
                            m_lEmailDocumentID = CInt(m_vLookupDetails(ACDetailKey, lCntr))
                        End If
                    End If

                    ' Check if this is the selected index.

                    If m_vLookupValues(ACValueID, lRow).Equals(m_vLookupDetails(ACDetailKey, lCntr)) Then



                        ctlLookup.SelectedIndex = newIndex
                    End If

                End If
            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then


                ctlLookup.SelectedIndex = 0
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    
    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""
        Try
            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbStatus_Panel1.Text = " " & sMessage

        Catch excep As System.Exception
            ' Error Section.

            ' Log Error.
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
            If Not Information.IsArray(m_vSearchData) Then
                lItemsFound = 0
            Else
                lItemsFound = (m_vSearchData.GetUpperBound(1) + 1)
            End If

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbStatus_Panel1.Text = " " & lItemsFound & " " & sMessage

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
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
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check all fields for data.

            If ((txtCode.Text.Trim() <> "") Or (cboType.SelectedIndex > 0)) Or (Information.IsDate(txtEffectiveDate.Text)) Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If
            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
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

            ImgImage.Left = Me.Width - VB6.TwipsToPixelsX(975)

            tabMainTab.Width = Me.Width - VB6.TwipsToPixelsX(1560)

            cmdFindNow.Left = Me.Width - VB6.TwipsToPixelsX(1335)
            cmdNewSearch.Left = Me.Width - VB6.TwipsToPixelsX(1335)

            lvwSearchDetails.Width = Me.Width - VB6.TwipsToPixelsX(360)
            lvwSearchDetails.Height = Me.Height - VB6.TwipsToPixelsY(3400)

            cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(1335)
            cmdHelp.Top = Me.Height - VB6.TwipsToPixelsY(1110)

            cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(2535)
            cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(1110)

            cmdOK.Left = Me.Width - VB6.TwipsToPixelsX(3735)
            cmdOK.Top = Me.Height - VB6.TwipsToPixelsY(1110)

            'cmdView.Top = Me.Height - 1110

            cmdDelete.Top = Me.Height - VB6.TwipsToPixelsY(1110)

            cmdNew.Top = Me.Height - VB6.TwipsToPixelsY(1110)
            cmdEdit.Top = Me.Height - VB6.TwipsToPixelsY(1110)

            If cmdNavigate.Visible Then
                cmdNavigate.Top = Me.Height - VB6.TwipsToPixelsY(1110)
            End If

            Return result

        Catch



            ' Error Section.


            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: LockDocument
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function LockDocument() As Integer

        Dim result As Integer = 0
        Dim sLockedBy As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create locking object if not already done so
            If m_oPMLock Is Nothing Then

                Dim temp_m_oPMLock As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oPMLock = temp_m_oPMLock

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="LockDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If


            m_lReturn = m_oPMLock.LockKey(sKeyName:="document_template_id", vKeyValue:=m_lDocumentTemplateId, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy)


            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    'OK

                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    If sLockedBy = "ERROR" Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to lock record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("Document template currently locked by " & sLockedBy & _
                                        Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Document Template Lock")
                        Return result
                    End If


                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock the document template", vApp:=ACApp, vClass:=ACClass, vMethod:="LockDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnlockDocument
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function UnlockDocument() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oPMLock.UnLockKey(sKeyName:="document_template_id", vKeyValue:=m_lDocumentTemplateId, iUserID:=g_oObjectManager.UserID)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to unlock the document template", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PRIVATE Methods (End)
    ' PRIVATE Events (Begin)


    Private Sub cboType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboType.SelectedIndexChanged

        If CheckMandatory() <> gPMConstants.PMEReturnCode.PMTrue Then
            ' No supplied data so cannot
            ' continue with the search.

            ' Set the mouse pointer to normal.
            cmdFindNow.Enabled = False
        Else
            cmdFindNow.Enabled = True
        End If

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Try
            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            'Code added to focus on the Type dropdown.
            If Not cboType.Focused Then
                cboType.Focus()
            End If

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

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEComponentAction.PMDelete

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Nothing OK, so we get out.
                'RKS 011004 PN15082
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            m_lReturn = LockDocument()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RKS 011004 PN15082
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            'Create document template object if not already done so
            If m_oDocTemplate Is Nothing Then

                ' Get an instance of the document template interface object via
                ' the public object manager.
                Dim temp_m_oDocTemplate As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oDocTemplate, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oDocTemplate = temp_m_oDocTemplate

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get template object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    'RKS 011004 PN15082
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    Exit Sub

                End If

            End If

            m_lReturn = m_oDocTemplate.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMDelete)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RKS 011004 PN15082
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If


            m_oDocTemplate.DocumentTemplateId = DocumentTemplateId

            m_oDocTemplate.DocumentTemplateCode = DocumentCode

            m_oDocTemplate.DocumentTypeId = DocumentTypeId

            m_oDocTemplate.DocumentTypeCode = DocumentTypeCode


            m_lReturn = m_oDocTemplate.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RKS 011004 PN15082
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            'Unlock it...
            m_lReturn = UnlockDocument()

            'If not cancelled, refresh grid

            If m_oDocTemplate.Status = gPMConstants.PMEReturnCode.PMCancel Then
                'RKS 011004 PN15082
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            cmdFindNow_Click(cmdFindNow, New EventArgs())

            cmdEdit.Enabled = False
            '    cmdNew.Enabled = False
            cmdDelete.Enabled = False

            'RKS 011004 PN15082
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the cmdDelete_Click event.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try
    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
        Dim sDocumentTemplateDescription As String = ""

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEComponentAction.PMEdit
            cmdEdit.Enabled = False
            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Nothing OK, so we get out.

                'RKS 011004 PN15082
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If


            'Ensure the future dated change exists for this document template.

            m_lReturn = g_oBusiness.GetFutureDatedTemplate(v_sTemplateCode:=DocumentCode, v_dtEffectiveDate:=m_dtEffectiveDate)

            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    If MessageBox.Show("Warning - a future dated change exists for this document template." & _
                                       Strings.Chr(13) & Strings.Chr(10) & " Are you sure you wish to edit this version of the document?", "Find Document Template", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.No Then

                        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                        Exit Sub
                    End If
                Case gPMConstants.PMEReturnCode.PMNotFound
                    'That's OK record not found.
                Case Else
                    'Log error.
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetFutureDatedTemplate Method", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdEdit")
                    Exit Sub
            End Select


            m_lReturn = LockDocument()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RKS 011004 PN15082
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            'Create document template object if not already done so
            If m_oDocTemplate Is Nothing Then

                ' Get an instance of the document template interface object via
                ' the public object manager.
                Dim temp_m_oDocTemplate As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oDocTemplate, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oDocTemplate = temp_m_oDocTemplate

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get template object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    'RKS 011004 PN15082
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    Exit Sub

                End If

            End If

            m_lReturn = m_oDocTemplate.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RKS 011004 PN15082
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            m_oDocTemplate.DocumentTemplateId = DocumentTemplateId
            m_oDocTemplate.DocumentTemplateCode = DocumentCode
            m_oDocTemplate.DocumentTypeId = DocumentTypeId
            m_oDocTemplate.DocumentTypeCode = DocumentTypeCode
            m_oDocTemplate.Mode = Mode ' RAM20050201 - Set the Mode (might be called from SWIFT)

            m_lReturn = m_oDocTemplate.Start()
            cmdEdit.Enabled = True
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RKS 011004 PN15082
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            'Unlock it...
            m_lReturn = UnlockDocument()

            'If not cancelled, refresh grid

            If m_oDocTemplate.Status = gPMConstants.PMEReturnCode.PMCancel Then
                'RKS 011004 PN15082
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            cmdFindNow_Click(cmdFindNow, New EventArgs())

            'RKS 011004 PN15082
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the cmdEdit_Click event.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try
    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click
        Dim sWildcardErrorMessage As String = ""
        
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If Not gPMFunctions.ValidWildcardSearch(v_bDisableWildcardSearchOption:=m_bDisableWildcardSearchOption, v_bEnablePartialWildcardSearchOption:=m_bEnablePartialWildcardSearchOption, r_sFieldValue:=txtCode.Text, r_sErrorMessage:=sWildcardErrorMessage) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                MessageBox.Show(sWildcardErrorMessage, "Find Party")
                txtCode.Focus()
                Exit Sub

            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
            End If

            If lvwSearchDetails.Items.Count > 0 Then
                VB6.SetDefault(cmdFindNow, False)
                VB6.SetDefault(cmdOK, False)
                cmdDelete.Enabled = True
                cmdEdit.Enabled = True
            End If

            ' Set the focus.
            lvwSearchDetails.Focus()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Find Now command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFindNow_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        End If
    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

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

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEComponentAction.PMAdd

            'Initialise this lot...
            DocumentTemplateId = 0
            DocumentCode = ""
            DocumentTypeId = 0
            DocumentTypeCode = ""

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Nothing OK, so we get out.
                'RKS 011004 PN15082
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            'Create document template object if not already done so
            If m_oDocTemplate Is Nothing Then

                ' Get an instance of the document template interface object via
                ' the public object manager.
                Dim temp_m_oDocTemplate As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oDocTemplate, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oDocTemplate = temp_m_oDocTemplate

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get template object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNew_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    'RKS 011004 PN15082
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    Exit Sub

                End If

            End If

            m_lReturn = m_oDocTemplate.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RKS 011004 PN15082
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If


            m_oDocTemplate.DocumentTemplateId = DocumentTemplateId

            m_oDocTemplate.DocumentTemplateCode = DocumentCode

            m_oDocTemplate.DocumentTypeId = DocumentTypeId

            m_oDocTemplate.DocumentTypeCode = DocumentTypeCode


            m_lReturn = m_oDocTemplate.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RKS 011004 PN15082
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            'If not cancelled, refresh grid

            If m_oDocTemplate.Status = gPMConstants.PMEReturnCode.PMCancel Then
                'RKS 011004 PN15082
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            cmdFindNow_Click(cmdFindNow, New EventArgs())

            cmdEdit.Enabled = False
            '    cmdNew.Enabled = False
            cmdDelete.Enabled = False

            'RKS 011004 PN15082
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the cmdNew_Click event.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNew_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try
    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

        ' Click event of the New Search button.

        Try

            ' Clear the interface details.
            m_lReturn = ClearInterface(bConfirm:=True)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If
            'Code added to focus on the Type dropdown.
            If Not cboType.Focused Then
                cboType.Focus()
            End If
        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Try

            If m_lProcessMode = PMEComponentAction.PMAdd And cmdEdit.Visible Then
                Call cmdEdit_Click(eventSender, eventArgs)
            Else
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMOK

                ' Process the next set of actions.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
                    Me.Hide()
                Else
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                End If

            End If

        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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
            m_oGeneral = New iPMBFindDocTemplate.General()

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


            m_sUnderwritingOrAgency = g_oBusiness.UnderwritingOrAgency

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

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

            ' Check if the search contains more or equal
            ' to the miniumum search length.

            ' {* USER DEFINED CODE (Begin) *}

            If CheckMandatory() <> gPMConstants.PMEReturnCode.PMTrue Then
                ' No supplied data so cannot
                ' continue with the search.

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' {* USER DEFINED CODE (End) *}

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



            ' Error Section

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

            ' Destroy the instance of the lock object
            ' from memory.
            If Not (m_oPMLock Is Nothing) Then
                m_oPMLock = Nothing
            End If

            ' Terminate the document template object (if used)
            If Not (m_oDocTemplate Is Nothing) Then


		m_oDocTemplate.Dispose()


                ' Destroy the instance of the document template object
                ' from memory.
                m_oDocTemplate = Nothing

            End If

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


            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            If VB6.PixelsToTwipsX(Me.Width) < 9165 Then
                Me.Width = VB6.TwipsToPixelsX(9165)
            End If

            If VB6.PixelsToTwipsY(Me.Height) < 6510 Then
                Me.Height = VB6.TwipsToPixelsY(6510)
            End If

            m_lReturn = ResizeInterface()

        Catch
            Exit Sub
        End Try
    End Sub

    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDetails.Columns(eventArgs.Column)

        ' Column click event for the search details
        Dim iDirection As Integer
        Static iDate As Integer
        Try

            With lvwSearchDetails

                If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwSearchDetails) Then
                    ' Set sort order opposite of
                    ' current direction.

                    If ListViewHelper.GetSortOrderProperty(lvwSearchDetails) = 1 Then
                        iDirection = SortOrder.Descending
                    Else
                        iDirection = SortOrder.Ascending
                    End If

                    ListViewHelper.SetSortOrderProperty(lvwSearchDetails, iDirection)

                Else
                    If ColumnHeader.Index = 3 Then
                        m_lReturn = CType(ListViewFunc.ListViewSortByDate(v_oListView:=lvwSearchDetails, v_iSourceColumn:=ColumnHeader.Index, v_iDirection:=iDate), gPMConstants.PMEReturnCode)

                        If ListViewHelper.GetSortOrderProperty(lvwSearchDetails) = 1 Then
                            iDate = SortOrder.Descending
                        Else
                            iDate = SortOrder.Ascending
                        End If
                    Else
                        ' Sort by this column (ascending).
                        ListViewHelper.SetSortedProperty(lvwSearchDetails, False)

                        ' Turn off sorting so that the list
                        ' is not sorted twice
                        ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
                        ListViewHelper.SetSortKeyProperty(lvwSearchDetails, ColumnHeader.Index + 1 - 1)
                        ListViewHelper.SetSortedProperty(lvwSearchDetails, True)

                    End If
                End If
            End With

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick

        ' Double click event for the search details.

        Try

            ' Check if there are any items available.
            If lvwSearchDetails.Items.Count = 0 Then
                Exit Sub
            End If

            If m_lProcessMode = gPMConstants.PMEComponentAction.PMAdd And cmdEdit.Visible Then
                cmdEdit_Click(cmdEdit, New EventArgs())
            Else
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMOK

                ' Process the next set of actions.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                Else
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                End If
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub lvwSearchDetails_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSearchDetails.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If lvwSearchDetails.GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
        Else
            'Only if we're not merging
            If (m_lMode = gSIRLibrary.ACNormalMode) Or (m_lMode = gSIRLibrary.ACSwiftEditMode) Then
                cmdDelete.Enabled = True
                If lvwSearchDetails.GetItemAt(x, y).ForeColor = Color.Gray Then
                    cmdDelete.Text = m_sUnDelete
                    cmdEdit.Enabled = False
                Else
                    cmdDelete.Text = m_sDelete
                    cmdEdit.Enabled = True
                End If
            End If
        End If

    End Sub

    ' PRIVATE Events (End)

    Private Sub txtCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        If CheckMandatory() <> gPMConstants.PMEReturnCode.PMTrue Then
            ' No supplied data so cannot
            ' continue with the search.

            ' Set the mouse pointer to normal.
            cmdFindNow.Enabled = False
        Else
            cmdFindNow.Enabled = True
        End If

    End Sub

    Public Sub getDocDetails(ByRef lDocID As Integer, ByRef lDocType As Integer)

        Dim lSelectedItem As Integer

        'MKW 07/01/02 - Determine if an item was selected in search details list.
        If lvwSearchDetails.FocusedItem Is Nothing Then

            'No item was selected
            'Therefore default document type to selected type

            'Check if type selected
            If cboType.SelectedIndex >= 0 Then
                'Document type not selected, therefore default to zero.
                lDocType = 0
            Else
                'Document type is selected, therefore default to selected type.
                lDocType = VB6.GetItemData(cboType, cboType.SelectedIndex)
            End If
        Else
            'Item Selected, therefore carry on as previous.

            lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)

            lDocID = CInt(m_vSearchData(ACIDocumentTemplateId, lSelectedItem))
            lDocType = CInt(m_vSearchData(ACIDocumentTypeId, lSelectedItem))
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            'Effective Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtEffectiveDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

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

    Private Sub txtEffectiveDate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cmdFindNow.Enabled = Not (CheckMandatory() <> gPMConstants.PMEReturnCode.PMTrue)
    End Sub

    Private Sub txtEffectiveDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtEffectiveDate)
    End Sub

    Private Sub txtEffectiveDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtEffectiveDate)
    End Sub

    
End Class
