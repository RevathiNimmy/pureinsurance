Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports System.Text
Imports System.IO
Imports Sspi.Common.Aws.S3
Imports System.Linq
Imports PMLookupControl

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


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    Private Const CloudImportFolder As String = "ImportExport/Import/"
    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTImportExport.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' File info
    Private m_vImport As Object
    Private m_vImported As Object
    Private m_vExport As Object
    Private m_vGenericMessages As Object
    ' File paths
    Private m_sImportPath As String = ""
    Private m_sImportedPath As String = ""
    Private m_sExportPath As String = ""
    Private m_sGenericMessagePath As String = ""

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private oPeriodArray As Object
    Private oPeriod As Object
    Private oCurrentPeriod As Object
    Private m_sGLExportPeriodAll As String

    Private m_vAgentTypes As Object
    Private m_sSpecialParty As String = ""
    Private m_bSuppressSubAgents As Boolean
    Private m_iSourceID As Integer ''Agent Filtering
    Private m_bAttachToScheduler As Boolean
    Private m_iBatchProcessId As Integer
    Private m_sBatchProcessName As String = ""
    Private m_oBatchParameters(,) As Object
    Private m_sbatchContentDetails As String
    Private m_dtParameters As DataTable = Nothing
    Private m_ibatchSchedulerId As Integer
    ' ***************************************************************** '
    '                         PUBLIC PROPERTIES
    ' ***************************************************************** '
    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have occurred on the interface.
            Return m_lErrorNumber
        End Get
    End Property
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
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
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    Public Property AgentTypes() As Object
        Get
            Return m_vAgentTypes
        End Get
        Set(ByVal Value As Object)
            m_vAgentTypes = Value
        End Set
    End Property

    Public WriteOnly Property SpecialParty() As String
        Set(ByVal Value As String)
            m_sSpecialParty = Value
        End Set
    End Property

    Public WriteOnly Property SuppressSubAgents() As Boolean
        Set(ByVal Value As Boolean)
            m_bSuppressSubAgents = Value
        End Set
    End Property

    Public Property BranchID() As Integer
        Get
            Return m_iSourceID
        End Get
        Set(ByVal Value As Integer)

            m_iSourceID = CInt(Value)
        End Set
    End Property

    Public Property AttachToScheduler() As Boolean
        Get
            Return m_bAttachToScheduler
        End Get
        Set(ByVal Value As Boolean)
            m_bAttachToScheduler = Value
        End Set
    End Property
    Public Property BatchProcessId() As Integer
        Get
            Return m_iBatchProcessId
        End Get
        Set(ByVal Value As Integer)
            m_iBatchProcessId = Value
        End Set
    End Property

    Public Property BatchProcessName() As String
        Get
            Return m_sBatchProcessName
        End Get
        Set(ByVal Value As String)
            m_sBatchProcessName = Value
        End Set
    End Property

    Public Property BatchParameters() As Object(,)
        Get
            Return m_oBatchParameters
        End Get
        Set(ByVal Value As Object(,))
            m_oBatchParameters = Value
        End Set
    End Property

    Public Property BatchFileContentDetails() As String
        Get
            Return m_sbatchContentDetails
        End Get
        Set(value As String)
            m_sbatchContentDetails = value
        End Set
    End Property

    Public Property BatchSchedulerId() As Integer
        Get
            Return m_ibatchSchedulerId
        End Get
        Set(ByVal Value As Integer)
            m_ibatchSchedulerId = Value
        End Set
    End Property
    ' ***************************************************************** '
    '                          PUBLIC METHODS
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business object.
    ' ***************************************************************** '
    Public Function BusinessToInterface(Optional ByVal v_lIndex As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lCount As Integer
        Dim oListItem As ListViewItem
        Dim oListSubItem As ListViewItem.ListViewSubItem

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "BusinessToInterface"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate each grid
            lReturn = CType(PopulateListView(lvwImport, m_vImport), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PopulateListView", "Unable to display import folder details")
            End If

            lReturn = CType(PopulateListView(lvwImported, m_vImported), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PopulateListView", "Unable to display imported folder details")
            End If
            If VB6.GetItemData(cboExportList, cboExportList.SelectedIndex) = gPMConstants.PMEExportInterface.pmeIEIMessageExport Then
                lReturn = CType(PopulateListView(lvwExport, m_vGenericMessages), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("PopulateListView", "Unable to display export folder details")
                End If
            Else
                lReturn = CType(PopulateListView(lvwExport, m_vExport), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("PopulateListView", "Unable to display export folder details")
                End If
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetBusiness"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get paths
            lReturn = CType(GetPaths(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetPaths", "Unable to get import/export paths")
            End If

            If m_oBusiness.CloudHostingEnabled Then
                Dim repository As IS3Repository = New S3Repository(Environment.GetEnvironmentVariable("AWS_IMPORTEXPORT_BUCKET_NAME"),
                Environment.GetEnvironmentVariable("AWS_REGION"), "")
                Dim configuredImportPath As String = IIf(m_sImportPath.EndsWith("\"), m_sImportPath, m_sImportPath & "\")
                lReturn = repository.DownloadFolderAsync(Environment.GetEnvironmentVariable("AWS_IMPORTEXPORT_FOLDER_NAME") & "/" & CloudImportFolder, m_sImportPath).Result
            End If

            ' Get the folder details from the business object.

            lReturn = m_oBusiness.GetFileSummary(sPath:=m_sImportPath, vResults:=m_vImport)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetFileSummary", "Unable to get file summary for import folder")
            End If


            lReturn = m_oBusiness.GetFileSummary(sPath:=m_sImportedPath, vResults:=m_vImported)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetFileSummary", "Unable to get file summary for imported folder")
            End If


            lReturn = m_oBusiness.GetFileSummary(sPath:=m_sExportPath, vResults:=m_vExport)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetFileSummary", "Unable to get file summary for export folder")
            End If


            lReturn = m_oBusiness.GetFileSummary(sPath:=m_sGenericMessagePath, vResults:=m_vGenericMessages)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetFileSummary", "Unable to get file summary for Generic Message folder")
            End If
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function




    ' ***************************************************************** '
    '                         PRIVATE METHODS
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: GetPaths
    '
    ' Description: Gets the import/export paths from system options
    ' ***************************************************************** '
    Private Function GetPaths() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetPaths"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the system option paths
            lReturn = CType(iPMFunc.GetSystemOption(ACImportPathOption, m_sImportPath), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetSystemOption", "Unable to get system option for import path")
            End If

            lReturn = CType(iPMFunc.GetSystemOption(ACImportedPathOption, m_sImportedPath), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetSystemOption", "Unable to get system option for imported path")
            End If

            lReturn = CType(iPMFunc.GetSystemOption(ACExportPathOption, m_sExportPath), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetSystemOption", "Unable to get system option for export path")
            End If

            lReturn = CType(iPMFunc.GetSystemOption(ACGenericMessagePathOption, m_sGenericMessagePath), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetSystemOption", "Unable to get system option for Generic path")
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally




        End Try
        Return result
    End Function
    Private Function PopulateListView(ByVal oListView As ListView, ByVal vData As Object) As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim oListSubItem As ListViewItem.ListViewSubItem

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "PopulateListView"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the list before we start
            oListView.Items.Clear()

            ' Process all items
            If Information.IsArray(vData) Then
                For lCount As Integer = vData.GetLowerBound(1) To vData.GetUpperBound(1)
                    ' Add the list item


                    oListItem = oListView.Items.Add(gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, CStr(vData(MainModule.ACImportExportFileInfoEnum.ACIEDate, lCount))))
                    ' Populate sub items 

                    If Convert.ToString(vData(MainModule.ACImportExportFileInfoEnum.ACIEInterface, lCount)).Trim() = "<Unknown>" Then
                        oListItem.SubItems.Add("Unknown")
                    Else
                        oListItem.SubItems.Add(Convert.ToString(vData(MainModule.ACImportExportFileInfoEnum.ACIEInterface, lCount)).Trim())
                    End If

                    oListItem.SubItems.Add(Convert.ToString(vData(MainModule.ACImportExportFileInfoEnum.ACIEReference, lCount)).Trim())
                    If Convert.ToString(vData(MainModule.ACImportExportFileInfoEnum.ACIERecords, lCount)).Trim() = "<Invalid>" Then
                        oListItem.SubItems.Add("Invalid")
                    Else
                        oListItem.SubItems.Add(Convert.ToString(vData(MainModule.ACImportExportFileInfoEnum.ACIERecords, lCount)).Trim())
                    End If
                    oListItem.SubItems.Add(Convert.ToString(vData(MainModule.ACImportExportFileInfoEnum.ACIEFilename, lCount)).Trim())

                    ' Set tag so we can trace back to array line
                    oListItem.Tag = CStr(lCount)
                Next lCount
            Else
                ' Instead of an array the data variant may contain an error/status message
                If Strings.Len(CStr(vData)) Then
                    ' Add the list item
                    oListItem = oListView.Items.Add(CStr(vData))
                    oListItem.Font = VB6.FontChangeBold(oListItem.Font, True)
                End If
            End If

            ' Ignore errors this is only a cosmetic nicety
            lReturn = CType(ListView6Func.ListViewAutoSize(oListView, True, True, Me), gPMConstants.PMEReturnCode)

            ' Refresh sort order
            SortList(oListView, ListViewHelper.GetSortKeyProperty(oListView), True)


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally





        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim iCount As Integer = 0

        Const kMethodName As String = "SetInterfaceDefaults"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            cmdReview.Enabled = False
            cmdClose.Enabled = True

            ' RDT 08/12/2006 - Changes for the CP Import Export Interface
            cboExportList.Items.Clear()
            cboExportList.Items.Add("General Ledger")
            VB6.SetItemData(cboExportList, 0, gPMConstants.PMEExportInterface.pmeIEIGLExport)
            cboExportList.Items.Add("Instalments")
            VB6.SetItemData(cboExportList, 1, gPMConstants.PMEExportInterface.pmeIEIInstalmentExport)
            cboExportList.Items.Add("Claims")
            VB6.SetItemData(cboExportList, 2, gPMConstants.PMEExportInterface.pmeIEIClaimExport)
            'Start (Girija chokkalingam) - (Tech Spec - PGR022 - Financial Interfaces.doc) - (5.3.2.1)
            cboExportList.Items.Add("Receipt Export")
            VB6.SetItemData(cboExportList, 3, gPMConstants.PMEExportInterface.pmeIEIReceiptExport)
            'End (Girija chokkalingam) - (Tech Spec - PGR022 - Financial Interfaces.doc) - (5.3.2.1)
            cboExportList.Items.Add("Payments")
            VB6.SetItemData(cboExportList, 4, gPMConstants.PMEExportInterface.pmeIEIPaymentExport)
            cboExportList.Items.Add("Instalment Plans")
            VB6.SetItemData(cboExportList, 5, gPMConstants.PMEExportInterface.pmeIEIInstalmentPlanExport)
            cboExportList.Items.Add("Policies")
            VB6.SetItemData(cboExportList, 6, gPMConstants.PMEExportInterface.pmeIEIPolicyExport)
            cboExportList.Items.Add("Message Export")
            VB6.SetItemData(cboExportList, 7, gPMConstants.PMEExportInterface.pmeIEIMessageExport)
            cboExportList.Items.Add("Document Export")
            VB6.SetItemData(cboExportList, 8, gPMConstants.PMEExportInterface.pmeIEIDocumentExport)
            'Start Tech Spec - 8.6 Premium Claims Analysis.doc
            cboExportList.Items.Add("Policy Batch Export")
            VB6.SetItemData(cboExportList, 9, gPMConstants.PMEExportInterface.pmeIEIPolicyBatchExport)
            ' End
            'WPR14-MID
            cboExportList.Items.Add("MID 1 Export")
            VB6.SetItemData(cboExportList, 10, gPMConstants.PMEExportInterface.pmeIEIMIDExport)
            'END WPR14-MID

            cboExportList.Items.Add("MID 2 Export")
            VB6.SetItemData(cboExportList, 11, gPMConstants.PMEExportInterface.pmeIEIMID2Export)
            cboExportList.SelectedIndex = 0

            'WPR03 Auto Release Of Commission
            cboExportList.Items.Add("Commission Export")
            VB6.SetItemData(cboExportList, 12, gPMConstants.PMEExportInterface.pmeIEICommissionExport)
            'END WPR03 Auto Release Of Commission

            ' LOAWR22 SMS Support
            cboEventType.Items.Add("SMS")
            VB6.SetItemData(cboEventType, 0, gPMConstants.PMEEventType.pmeIEISMS)

            txtExportBatchID.Enabled = True
            lblExportBatchID.Enabled = True
            txtExportLeadDays.Enabled = False
            lblExportLeadDays.Enabled = False
            cboPMExportBankAccountName.Enabled = False
            lblExportBankAccountName.Enabled = False
            cboPMExportMediaType.Enabled = False
            lblExportMediaType.Enabled = False
            chkExportAutoPost.Enabled = False
            lblEventType.Visible = False
            cboEventType.Visible = False
            cmdExport.Left = lblEventType.Left
            'PLICO 50
            lReturn = m_oBusiness.GetAllPeriods(oPeriodArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("bActImportExport.GetAllPeriods", "Unable to fetch period details")
            ElseIf Information.IsArray(oPeriodArray) = True Then
                cboAllPeriod.Items.Add(New ValueDescriptionPair(0, "(All)"))
                For iCount = 0 To oPeriodArray.GetUpperBound(1)
                    cboAllPeriod.Items.Add(New ValueDescriptionPair(CType(oPeriodArray(0, iCount), Integer), oPeriodArray(1, iCount).ToString))
                Next


                lReturn = m_oBusiness.GetPeriods(oPeriod)
                lReturn = m_oBusiness.GetCurrentPeriods(r_vPeriod:=oPeriod(8, 0), r_voCurrentPeriod:=oCurrentPeriod)
                lReturn = CType(iPMFunc.GetSystemOption(ACGLExportPeriodALL, m_sGLExportPeriodAll), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetSystemOption", "Unable to get Default GL Export Period To ALL")
                End If
                If m_sGLExportPeriodAll = "1" Then
                    cboAllPeriod.SelectedIndex = 0
                Else

                    For iCount = 0 To oPeriodArray.GetUpperBound(1)
                        If CDate(oPeriodArray(1, iCount)).ToShortDateString = CDate(oCurrentPeriod(5, 0)).ToShortDateString Then
                            cboAllPeriod.SelectedIndex = iCount + 1
                        End If

                    Next
                End If
            End If

            If Information.IsArray(m_vAgentTypes) Then

                cmbAgentType.Items.Clear()
                Dim cmbAgentType_NewIndex As Integer = -1
                cmbAgentType_NewIndex = 0
                cmbAgentType.Items.Insert(cmbAgentType_NewIndex, "")

                For partyCount As Integer = m_vAgentTypes.GetLowerBound(1) To m_vAgentTypes.GetUpperBound(1)
                    'PN 18683 : Used ItemData property instead of ListIndex for cmbAgentType
                    If CStr(m_vAgentTypes(0, iCount)) = "AGENT" Then
                        cmbAgentType_NewIndex = cmbAgentType.Items.Add(gSIRLibrary.SIRPartyTypeAgentText)
                        VB6.SetItemData(cmbAgentType, cmbAgentType_NewIndex, CInt(m_vAgentTypes(1, iCount)))
                    End If
                    If CStr(m_vAgentTypes(0, iCount)) = "SUB AGENT" Then
                        cmbAgentType_NewIndex = cmbAgentType.Items.Add(PMBConst.PMBAgentTypeSubAgentText)
                        VB6.SetItemData(cmbAgentType, cmbAgentType_NewIndex, CInt(m_vAgentTypes(1, iCount)))
                    End If
                    If CStr(m_vAgentTypes(0, iCount)) = "INTRODUCER" Then
                        cmbAgentType_NewIndex = cmbAgentType.Items.Add(PMBConst.PMBAgentTypeIntroducerText)
                        VB6.SetItemData(cmbAgentType, cmbAgentType_NewIndex, CInt(m_vAgentTypes(1, iCount)))
                    End If
                Next partyCount
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_sSpecialParty = "AG"
            If (m_sSpecialParty = PMBConst.PMBPartyTypeAgent) Or (m_sSpecialParty = PMBConst.PMBPartyTypeSubAgent) Or (m_sSpecialParty = PMBConst.PMBPartyTypeCommissionAccount) Or (m_sSpecialParty = PMBConst.PMBPartyTypeIntermediary) Then
                'KB PN Issue 1929
                'These types apply to Underwriting only so add switch

                If m_sSpecialParty = PMBConst.PMBPartyTypeSubAgent Then
                    cmbAgentType.Items.Insert(0, "")
                    cmbAgentType.Items.Insert(1, "")
                    cmbAgentType.Items.Insert(2, PMBConst.PMBAgentTypeSubAgentText)
                ElseIf m_sSpecialParty = PMBConst.PMBPartyTypeCommissionAccount Then
                    cmbAgentType.Items.Insert(0, "")
                    cmbAgentType.Items.Insert(1, "")
                    cmbAgentType.Items.Insert(2, "")
                    cmbAgentType.Items.Insert(3, PMBConst.PMBAgentTypeCommAccountText)
                ElseIf m_sSpecialParty = PMBConst.PMBPartyTypeIntermediary Then
                    cmbAgentType.Items.Insert(0, "")
                    cmbAgentType.Items.Insert(1, "")
                    cmbAgentType.Items.Insert(2, "")
                    cmbAgentType.Items.Insert(3, "")
                    cmbAgentType.Items.Insert(4, PMBConst.PMBAgentTypeIntermediaryText)
                Else
                    cmbAgentType.Items.Insert(0, "(ALL)")
                    cmbAgentType.Items.Insert(1, PMBConst.PMBAgentTypeBrokerText)
                    If Not (m_bSuppressSubAgents) Then
                        cmbAgentType.Items.Insert(2, PMBConst.PMBAgentTypeSubAgentText)
                        cmbAgentType.Items.Insert(3, PMBConst.PMBAgentTypeCommAccountText)
                        cmbAgentType.Items.Insert(4, PMBConst.PMBAgentTypeIntermediaryText)
                    Else
                        cmbAgentType.Items.Insert(2, PMBConst.PMBAgentTypeCommAccountText)
                        cmbAgentType.Items.Insert(3, PMBConst.PMBAgentTypeIntermediaryText)
                    End If
                    cmbAgentType.SelectedIndex = 0
                End If
            End If

            If AttachToScheduler Then
                cmdRefresh.Visible = False
                cmdReview.Visible = False
                cmdImport.Visible = False
                cmdExport.Visible = False
                SSTabHelper.SetTabEnabled(tabIE, 1, False)

                Select Case BatchProcessName
                    Case "Import"
                        SSTabHelper.SetSelectedTabIndex(tabIE, 0)
                        SSTabHelper.SetTabEnabled(tabIE, 2, False)
                        If Task = gPMConstants.PMEComponentAction.PMView Then
                            cmdImportSchedule.Text = "View &Schedule"
                        End If
                    Case "Export"
                        SSTabHelper.SetSelectedTabIndex(tabIE, 2)
                        SSTabHelper.SetTabEnabled(tabIE, 0, False)
                        Select Case m_iTask
                            Case gPMConstants.PMEComponentAction.PMEdit
                                lReturn = LoadBatchParameters()
                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError("LoadBatchParameters", "Unable to load parameters for batch schedular")
                                End If
                            Case gPMConstants.PMEComponentAction.PMView
                                cmdExportSchedule.Text = "View &Schedule"

                                lReturn = LoadBatchParameters()
                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError("LoadBatchParameters", "Unable to load parameters for batch schedular")
                                End If

                                EnableDisableInterfaceButton(False)

                        End Select
                End Select
            Else
                cmdImportSchedule.Visible = False
                cmdExportSchedule.Visible = False
            End If

            chkExportDueDay.Enabled = False
            'Populate the Strike day combo box and select default
            For iDay As Integer = 1 To 28
                cboExportDueDay.Items.Insert(iDay - 1, gPMFunctions.ToSafeString(iDay))
            Next iDay
            cboExportDueDay.SelectedIndex = 0

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally




        End Try
        Return result
    End Function

    Private Function SortList(ByVal oListView As ListView, ByVal lColumnIndex As Integer, Optional ByVal bReSort As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "SortList"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' We may just be refreshing after a item edit or addition
            If Not bReSort Then
                ' Reverse sort order if column hasn't changed
                If ListViewHelper.GetSortKeyProperty(oListView) = lColumnIndex Then
                    ListViewHelper.SetSortOrderProperty(oListView, IIf(ListViewHelper.GetSortOrderProperty(oListView) = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending))
                Else
                    ListViewHelper.SetSortOrderProperty(oListView, SortOrder.Ascending)
                End If
            End If

            ' Sort based on contents
            Select Case lColumnIndex
                Case 0 ' Date
                    'Developer Guide no.170
                    ListView6Func.ListViewSortByDate(oListView, lColumnIndex, ListViewHelper.GetSortOrderProperty(oListView), True)
                Case 3 ' Value..mostly :-)
                    'Developer Guide no.170
                    ListView6Func.ListViewSortByValue(oListView, lColumnIndex, ListViewHelper.GetSortOrderProperty(oListView), True)
                Case Else
                    ListViewHelper.SetSortKeyProperty(oListView, lColumnIndex)
                    ListViewHelper.SetSortedProperty(oListView, True)
            End Select


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function


    Private Sub cboExportList_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboExportList.SelectedIndexChanged
        Dim iInterface As gPMConstants.PMEExportInterface

        Dim vParamArray(,) As Object
        Dim lUbnd As Integer

        txtExportBatchID.Enabled = False
        lblExportBatchID.Enabled = False
        txtExportLeadDays.Enabled = False
        lblExportLeadDays.Enabled = False
        cboPMExportBankAccountName.Enabled = False
        lblExportBankAccountName.Enabled = False
        cboPMExportMediaType.Enabled = False
        lblExportMediaType.Enabled = False
        chkExportAutoPost.Enabled = False
        lblExportPFSchemeTypeCode.Enabled = False
        cboPMExportPFSchemeTypeCode.Enabled = False
        lblEventType.Visible = False
        cboEventType.Visible = False

        lblStartDate.Enabled = False
        dtpStartDate.Enabled = False
        lblEndDate.Enabled = False
        dtpEndDate.Enabled = False

        lblPeriod.Enabled = False
        cboAllPeriod.Enabled = False
        cmdExport.Left = lblEventType.Left
        chkExportMetadata.Visible = False
        lblCurrency.Enabled = False
        cboCurrency.Enabled = False
        lblAgentType.Enabled = False
        If cmbAgentType.Items.Count > 0 Then
            cmbAgentType.SelectedIndex = 0
        End If
        cmbAgentType.Enabled = False
        chkExportDueDay.Enabled = False
        cboExportDueDay.Enabled = False
        If chkExportDueDay.Checked Then
            cboExportDueDay.Visible = True
        Else
            cboExportDueDay.Visible = False
        End If

        'Select Case VB6.GetItemData(cboExportList, cboExportList.SelectedIndex)
        Select Case cboExportList.SelectedIndex
            Case gPMConstants.PMEExportInterface.pmeIEIGLExport
                txtExportBatchID.Enabled = True
                lblExportBatchID.Enabled = True
                lblPeriod.Enabled = True
                cboAllPeriod.Enabled = True
            Case gPMConstants.PMEExportInterface.pmeIEIClaimExport
                txtExportBatchID.Enabled = True
                lblExportBatchID.Enabled = True
                lblStartDate.Enabled = True
                dtpStartDate.Enabled = True
                lblEndDate.Enabled = True
                dtpEndDate.Enabled = True
                chkExportMetadata.Visible = True
            Case gPMConstants.PMEExportInterface.pmeIEIInstalmentExport
                txtExportBatchID.Enabled = True
                lblExportBatchID.Enabled = True
                txtExportLeadDays.Enabled = True
                lblExportLeadDays.Enabled = True
                cboPMExportBankAccountName.Enabled = True
                lblExportBankAccountName.Enabled = True
                cboPMExportMediaType.Enabled = True
                lblExportMediaType.Enabled = True
                chkExportAutoPost.Enabled = True
                chkExportDueDay.Enabled = True
                cboExportDueDay.Enabled = True
                If iInterface = gPMConstants.PMEExportInterface.pmeIEIInstalmentExport Then
                    If chkExportDueDay.CheckState = CheckState.Checked Then
                        lUbnd += 1
                        ReDim Preserve vParamArray(1, lUbnd)
                        vParamArray(0, lUbnd) = "DUE_DATE"
                        vParamArray(1, lUbnd) = cboExportDueDay.SelectedItem
                    End If
                End If

            Case gPMConstants.PMEExportInterface.pmeIEIPaymentExport
                txtExportBatchID.Enabled = True
                lblExportBatchID.Enabled = True
                txtExportLeadDays.Enabled = True
                lblExportLeadDays.Enabled = True
                cboPMExportBankAccountName.Enabled = True
                lblExportBankAccountName.Enabled = True
                cboPMExportMediaType.Enabled = True
                lblExportMediaType.Enabled = True
            Case gPMConstants.PMEExportInterface.pmeIEIInstalmentPlanExport
                txtExportBatchID.Enabled = True
                lblExportBatchID.Enabled = True
                lblExportPFSchemeTypeCode.Enabled = True
                cboPMExportPFSchemeTypeCode.Enabled = True
                'cboPMExportMediaType.Enabled = True
                'lblExportMediaType.Enabled = True
            Case gPMConstants.PMEExportInterface.pmeIEIPolicyExport
                'txtExportBatchID.Enabled = True
                'lblExportBatchID.Enabled = True
                chkExportMetadata.Visible = True
                'No options for the policy export
            Case gPMConstants.PMEExportInterface.pmeIEIMessageExport
                txtExportBatchID.Enabled = True
                lblExportBatchID.Enabled = True
                lblEventType.Visible = True
                cboEventType.Visible = True
                cmdExport.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cboEventType.Left) + VB6.PixelsToTwipsX(cboEventType.Width) + 300)
                cboEventType.SelectedIndex = 0
                'Start (Girija chokkalingam) - (Tech Spec - PGR022 - Financial Interfaces.doc) - (5.3.2.2)
            Case gPMConstants.PMEExportInterface.pmeIEIReceiptExport
                txtExportBatchID.Enabled = True
                lblExportBatchID.Enabled = True
                cboPMExportBankAccountName.Enabled = True
                lblExportBankAccountName.Enabled = True
                cboPMExportMediaType.Enabled = True
                lblExportMediaType.Enabled = True
                'End (Girija chokkalingam) - (Tech Spec - PGR022 - Financial Interfaces.doc) - (5.3.2.2)
            Case gPMConstants.PMEExportInterface.pmeIEIDocumentExport
                txtExportBatchID.Enabled = True
                lblExportBatchID.Enabled = True
                lblStartDate.Enabled = True
                dtpStartDate.Enabled = True
                lblEndDate.Enabled = True
                dtpEndDate.Enabled = True
                chkExportMetadata.Visible = True
                'Start Tech Spec - 8.6 Premium Claims Analysis.doc
            Case gPMConstants.PMEExportInterface.pmeIEIPolicyBatchExport
                txtExportBatchID.Enabled = True
                lblExportBatchID.Enabled = True
                lblStartDate.Enabled = True
                dtpStartDate.Enabled = True
                lblEndDate.Enabled = True
                dtpEndDate.Enabled = True
                'End
                'WPR14-MID
            Case gPMConstants.PMEExportInterface.pmeIEIMIDExport
                txtExportBatchID.Enabled = True
                lblExportBatchID.Enabled = True
                'dtpStartDate.Enabled = True
                'lblEndDate.Enabled = True
                'dtpEndDate.Enabled = True
                'END WPR14-MID
            Case gPMConstants.PMEExportInterface.pmeIEIMID2Export
                txtExportBatchID.Enabled = True
                lblExportBatchID.Enabled = True

            Case gPMConstants.PMEExportInterface.pmeIEICommissionExport
                lblExportLeadDays.Enabled = True
                txtExportLeadDays.Enabled = True
                lblCurrency.Enabled = True
                cboCurrency.Enabled = True
                Dim oBranch As Object = Nothing
                m_lReturn = m_oBusiness.GetBranchDetails(oBranch)
                If m_lReturn = PMEReturnCode.PMTrue AndAlso Information.IsArray(oBranch) = True Then
                    cboCurrency.CurrencyId = ToSafeInteger(oBranch(7, 0))
                End If
                lblAgentType.Enabled = True
                cmbAgentType.Enabled = True
                cmbAgentType.SelectedIndex = 3

                cboPMExportBankAccountName.Enabled = True
                lblExportBankAccountName.Enabled = True
                cboPMExportMediaType.Enabled = True
                lblExportMediaType.Enabled = True


        End Select
        If VB6.GetItemData(cboExportList, cboExportList.SelectedIndex) = gPMConstants.PMEExportInterface.pmeIEIMessageExport Then
            m_lReturn = CType(PopulateListView(lvwExport, m_vGenericMessages), gPMConstants.PMEReturnCode)
        Else
            m_lReturn = CType(PopulateListView(lvwExport, m_vExport), gPMConstants.PMEReturnCode)
        End If
        m_lReturn = GetCurrentUsersSetting()

    End Sub

    ' ***************************************************************** '
    '                             EVENTS
    ' ***************************************************************** '
    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click

        Dim lReturn As Integer
        Const kMethodName As String = "cmdClose_Click"


        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)
        Finally

        End Try
    End Sub

    ' RDT 08/12/2006 - Changes for the CP Import Export Interface
    Private Sub cmdExport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExport.Click

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim vParamArray(,) As Object = Nothing

        Const kMethodName As String = "cmdExport_Click"


        Try

            lReturn = SetExportParameters(vParamArray:=vParamArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("bActImportExport.ProcessManualExport", "Unable to set export parameters")
            End If

            lReturn = storeAwayCurrentUsersSetting()

            ' Process the import file

            lReturn = m_oBusiness.ProcessManualExport(v_iInterface:=cboExportList.SelectedIndex, v_lBatchID:=Conversion.Val(txtExportBatchID.Text), v_vParamArray:=vParamArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("bActImportExport.ProcessManualExport", "Unable to export request data")
            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)
        End Try
    End Sub

    Private Sub cmdImport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdImport.Click

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim iInterface As Integer
        Const kMethodName As String = "cmdImport_Click"


        Try

            ' Process the import file

            lReturn = m_oBusiness.ProcessManualImport("")
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("bActImportExport.ProcessManualImport", "Unable to Import supplied files")
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)
        Finally

        End Try
    End Sub

    Private Sub cmdRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRefresh.Click

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdRefresh_Click"


        Try

            ' Show hourglass
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get files again
            lReturn = GetBusiness()

            ' Refresh
            lReturn = BusinessToInterface()


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub

    Private Sub cmdReview_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReview.Click

        ' Interface detailss
        Dim sInterface As String = ""
        Dim sFilename As String = ""
        Dim sTitle As String = ""
        ' For convenience, on second review module replace with generic "Form"
        Dim oForm As Object

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdReview_Click"


        Try


            If tabIE.SelectedIndex = 0 Then ' IMPORT TAB is focused
                ' Check we have a selected item
                If lvwImport.FocusedItem Is Nothing Then
                    cmdReview.Enabled = False
                    Exit Sub
                End If
                ' Get row details
                sInterface = lvwImport.FocusedItem.SubItems(1).Text
                sFilename = lvwImport.FocusedItem.SubItems(4).Text
                sTitle = "Import Review"
            ElseIf tabIE.SelectedIndex = 1 Then
                ' Check we have a selected item
                If lvwImported.FocusedItem Is Nothing Then
                    cmdReview.Enabled = False
                    Exit Sub
                End If
                ' Get row details
                sInterface = lvwImported.FocusedItem.SubItems(1).Text
                sFilename = lvwImported.FocusedItem.SubItems(4).Text
                sTitle = "Import Review"
            ElseIf tabIE.SelectedIndex = 2 Then ' EXPORT TAB is focused
                If cboExportList.SelectedIndex = 0 Then
                    sInterface = "GL_EXPORT"
                    sFilename = lvwExport.FocusedItem.SubItems(4).Text
                Else
                    sInterface = lvwExport.FocusedItem.SubItems(1).Text
                    sFilename = lvwExport.FocusedItem.SubItems(4).Text
                End If
                sTitle = "Export Review"
            End If

            ' Check it's interface is valid
            Select Case sInterface.ToUpper()
                Case "RECEIPT_IMPORT"
                    ' Yes, get review form
                    oForm = New frmReceiptImport()
                    ' Run preview dialog
                    If tabIE.SelectedIndex = 0 Then
                        lReturn = CType(oForm.Start(m_oBusiness, m_sImportPath, sFilename), gPMConstants.PMEReturnCode)
                    ElseIf tabIE.SelectedIndex = 1 Then
                        lReturn = CType(oForm.Start(m_oBusiness, m_sImportedPath, sFilename, tabIE.SelectedIndex), gPMConstants.PMEReturnCode)
                    Else
                        lReturn = CType(oForm.Start(m_oBusiness, m_sExportPath, sFilename), gPMConstants.PMEReturnCode)
                    End If
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("oForm.Start", "Unable to start import review dialog")
                    End If

                Case Else '"GL_EXPORT"
                    'oForm = New frmGLView() 
                    Dim FileInfo As System.IO.FileInfo
                    If (sInterface.ToUpper().Contains("IMPORT")) Then
                        FileInfo = New System.IO.FileInfo(m_sImportPath & "\" & sFilename)
                    Else
                        FileInfo = New System.IO.FileInfo(m_sExportPath & "\" & sFilename)
                    End If
                    'Dim FileInfo As New System.IO.FileInfo(m_sExportPath & "\" & sFilename)
                    If (FileInfo.Length > 524285370) Then 'limit check 499.99MB  
                        MessageBox.Show(String.Format("File ""{0}"" is too large.{1}Maximum size allowed is 500MB ", sFilename, vbCrLf), "Review", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Else
                        oForm = New frmViewGL()
                        If tabIE.SelectedIndex = 0 Then
                            oForm.FileName = m_sImportPath & "\" & sFilename
                        ElseIf tabIE.SelectedIndex = 1 Then
                            oForm.FileName = m_sImportedPath & "\" & sFilename
                        Else
                            oForm.FileName = m_sExportPath & "\" & sFilename
                        End If
                        oForm.Title = sTitle
                        oForm.Show()
                        oForm = Nothing
                    End If

                    'Case Else
                    '    ' It's not
                    '    cmdReview.Enabled = False
                    '    GoTo Finally_Renamed
            End Select
            GC.Collect()
            GC.WaitForPendingFinalizers()


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim lReturn As Integer
        Const kMethodName As String = "Form_Initialize"


        Try

            ' Show form in task bar
            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTImportExport.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to get instance of business object")
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTImportExport.General()

            ' Call the initialise method passing this interface and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oGeneral.Initialise", "Unable to initialise General object")
            End If

            ' Set the interface status to cancelled. This is done so that any
            ' interface termination will be noted as cancelled except in the
            ' event of accepting the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

            ' Set error code
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

        Finally

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "Form_KeyDown"


        Try

            ' Refresh form
            If eventArgs.KeyCode = Keys.F5 Then
                ' Show hourglass
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                ' Get files again
                lReturn = GetBusiness()

                ' Refresh
                lReturn = BusinessToInterface()
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "Form_Load"

        cboPMExportMediaType.FirstItem = ""
        cboPMExportBankAccountName.FirstItem = "(None)"
        cboPMExportPFSchemeTypeCode.FirstItem = "(None)"
        cboCurrency.FirstItem = "(ALL)"
        Try


            ' Show form in task bar
            iPMFunc.ShowFormInTaskBar_Detach()
            SSTabHelper.SetSelectedTabIndex(tabIE, 0)
            ' Check if we have had an error so far. Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error, so we MUST exit now.

                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.SetProcessModes", "Failed to set the process modes for the business object")
            End If

            ' Set the interface default values.
            lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetInterfaceDefaults", "Failed to set interface default values")
            End If

            If m_oBusiness.CloudHostingEnabled Then
                txtXSLTDestFolder.Enabled = False
            End If

            ' Gets the interface details to be displayed.
            lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oGeneral.GetInterfaceDetails", "Failed to get interface details")
            End If
            cmdRefresh.Select()
            cmdRefresh.Focus()

            Exit Sub


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

            ' Set error code
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

        Finally
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim lReturn As Integer
        Const kMethodName As String = "Form_QueryUnload"


        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            'Developer Guide no.19
            If UnloadMode <> vbFormCode Then

                ' Process the next set of actions depending upon the interface task etc.
                lReturn = m_oGeneral.ProcessCommand()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

            m_oGeneral = Nothing

            ' Terminate the business object

            m_oBusiness.Dispose()

            m_oBusiness = Nothing


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            eventArgs.Cancel = Cancel <> 0
        End Try
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            ' RDT 08/12/2006 - Changes for the CP Import Export Interface
            ' Move the listview and buttons
            tabIE.SetBounds(VB6.TwipsToPixelsX(60), VB6.TwipsToPixelsY(60), ClientRectangle.Width - VB6.TwipsToPixelsX(120), ClientRectangle.Height - VB6.TwipsToPixelsY(510))
            lvwImport.Width = tabIE.Width - VB6.TwipsToPixelsX(180)
            lvwImport.Height = tabIE.Height - VB6.TwipsToPixelsY(1440)
            lvwImported.Width = tabIE.Width - VB6.TwipsToPixelsX(180)
            lvwImported.Height = tabIE.Height - VB6.TwipsToPixelsY(480)
            lvwExport.Width = tabIE.Width - VB6.TwipsToPixelsX(180)
            'lvwExport.Height = tabIE.Height - 3240
            lvwExport.Height = tabIE.Height - VB6.TwipsToPixelsY(3980)
            frameExport.Width = lvwExport.Width
            cmdRefresh.SetBounds(VB6.TwipsToPixelsX(60), ClientRectangle.Height - VB6.TwipsToPixelsY(390), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdReview.SetBounds(cmdRefresh.Width + VB6.TwipsToPixelsX(150), ClientRectangle.Height - VB6.TwipsToPixelsY(390), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdClose.SetBounds(ClientRectangle.Width - VB6.TwipsToPixelsX(1155), ClientRectangle.Height - VB6.TwipsToPixelsY(390), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub

    Private Sub lvwExport_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwExport.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwExport.Columns(eventArgs.Column)
        SortList(lvwExport, ColumnHeader.Index + 1 - 1)
    End Sub

    Private Sub lvwExport_ItemClick(ByVal Item As ListViewItem)
        'If cboExportList.SelectedIndex = 0 And iPMFunc.IsIn(ListViewHelper.GetListViewSubItem(Item, 1).Text, "GL_EXPORT") Then
        cmdReview.Enabled = True
        'Else
        'cmdReview.Enabled = False
        'End If
    End Sub
    Private Sub lvwImport_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwImport.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwImport.Columns(eventArgs.Column)
        SortList(lvwImport, ColumnHeader.Index + 1 - 1)
    End Sub

    Private Sub lvwImport_ItemClick(ByVal Item As ListViewItem)
        ' Check item interface, enable it for supported interfaces
        'cmdReview.Enabled = iPMFunc.IsIn(ListViewHelper.GetListViewSubItem(Item, 1).Text, "RECEIPT_IMPORT")
        cmdReview.Enabled = True
    End Sub

    Private Sub lvwImported_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwImported.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwImported.Columns(eventArgs.Column)
        SortList(lvwImported, ColumnHeader.Index + 1 - 1)
    End Sub

    Private Sub lvwImported_ItemClick(ByVal Item As ListViewItem)
        cmdReview.Enabled = True
    End Sub
    Private Sub tabIE_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabIE.SelectedIndexChanged
        ' Only show active grid, else resize code screws up display
        lvwImport.Visible = (SSTabHelper.GetSelectedIndex(tabIE) = 0)
        lvwImported.Visible = (SSTabHelper.GetSelectedIndex(tabIE) = 1)
        lvwExport.Visible = (SSTabHelper.GetSelectedIndex(tabIE) = 2)
        cmdReview.Enabled = False
    End Sub

    Private Sub lvwImport_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwImport.SelectedIndexChanged
        If lvwImport.SelectedItems.Count > 0 Then
            lvwImport_ItemClick(lvwImport.SelectedItems(0))
        End If
    End Sub

    Private Sub lvwImported_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwImported.SelectedIndexChanged
        If lvwImported.SelectedItems.Count > 0 Then
            lvwImported_ItemClick(lvwImported.SelectedItems(0))
        End If
    End Sub

    Private Sub lvwExport_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwExport.SelectedIndexChanged
        If lvwExport.SelectedItems.Count > 0 Then
            lvwExport_ItemClick(lvwExport.SelectedItems(0))
        End If
    End Sub
    ' ***************************************************************** '
    ' Name: GetCurrentUsersSetting
    '
    ' Description: Retrieves the details from the setting File.
    ' ***************************************************************** '
    Public Function GetCurrentUsersSetting() As Object
        Dim sMediaCode As String = cboPMExportMediaType.ItemCode
        Dim result As Object = Nothing
        Try
            With My.Settings
                result = gPMConstants.PMEReturnCode.PMTrue
                txtXSLTFilename.Text = String.Empty
                txtXSLTDestFolder.Text = String.Empty
                txtXSLTDestExtension.Text = String.Empty

                'Store away the current users settings
                If cboExportList.Text = "Instalments" Then
                    If Trim(sMediaCode) = "DD" Then
                        txtXSLTFilename.Text = .XSLTFileNameDD
                        txtXSLTDestFolder.Text = .XSLTDestFolderDD
                        txtXSLTDestExtension.Text = .XSLTDestExtnDD
                    ElseIf Trim(sMediaCode) = "CC" Then
                        txtXSLTFilename.Text = .XSLTFileNameCC
                        txtXSLTDestFolder.Text = .XSLTDestFolderCC
                        txtXSLTDestExtension.Text = .XSLTDestExtnCC
                    End If
                ElseIf cboExportList.Text = "Payments" Then
                    If Trim(sMediaCode) = "DD" Then
                        txtXSLTFilename.Text = .XSLTFilePayNameDD
                        txtXSLTDestFolder.Text = .XSLTDestPayFolderDD
                        txtXSLTDestExtension.Text = .XSLTDestPayExtnDD
                    ElseIf Trim(sMediaCode) = "CC" Then
                        txtXSLTFilename.Text = .XSLTFilePayNameCC
                        txtXSLTDestFolder.Text = .XSLTDestPayFolderCC
                        txtXSLTDestExtension.Text = .XSLTDestPayExtnCC
                    End If
                ElseIf cboExportList.Text.ToUpper() = "MID 2 EXPORT" Then
                    txtXSLTFilename.Text = .XSLTFileNamerMID2
                    txtXSLTDestFolder.Text = .XSLTDestFolderMID2
                    txtXSLTDestExtension.Text = .XSLTDestExtnMID2
                End If
            End With
            Return result

        Catch exc As System.Exception

            ' Error Section.
            result = ""
            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get data from the resource file", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentUsersSetting", excep:=exc)
            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: storeAwayCurrentUsersSetting
    '
    ' Description: Save the details in setting file.
    ' ***************************************************************** '
    Public Function storeAwayCurrentUsersSetting() As Object
        Dim sMediaCode As String = cboPMExportMediaType.ItemCode
        Dim result As Object = Nothing
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            With My.Settings
                result = gPMConstants.PMEReturnCode.PMTrue
                'Store away the current users settings
                If cboExportList.Text = "Instalments" Then
                    If Trim(sMediaCode) = "DD" Then
                        .XSLTFileNameDD = txtXSLTFilename.Text
                        .XSLTDestFolderDD = txtXSLTDestFolder.Text
                        .XSLTDestExtnDD = txtXSLTDestExtension.Text
                    ElseIf Trim(sMediaCode) = "CC" Then
                        .XSLTFileNameCC = txtXSLTFilename.Text
                        .XSLTDestFolderCC = txtXSLTDestFolder.Text
                        .XSLTDestExtnCC = txtXSLTDestExtension.Text
                    End If
                ElseIf cboExportList.Text = "Payments" Then
                    If Trim(sMediaCode) = "DD" Then
                        .XSLTFilePayNameDD = txtXSLTFilename.Text
                        .XSLTDestPayFolderDD = txtXSLTDestFolder.Text
                        .XSLTDestPayExtnDD = txtXSLTDestExtension.Text
                    ElseIf Trim(sMediaCode) = "CC" Then
                        .XSLTFilePayNameCC = txtXSLTFilename.Text
                        .XSLTDestPayFolderCC = txtXSLTDestFolder.Text
                        .XSLTDestPayExtnCC = txtXSLTDestExtension.Text
                    End If
                ElseIf cboExportList.Text = "MID 2 Export" Then
                    .XSLTFileNamerMID2 = txtXSLTFilename.Text
                    .XSLTDestFolderMID2 = txtXSLTDestFolder.Text
                    .XSLTDestExtnMID2 = txtXSLTDestExtension.Text
                End If
                .Save()
            End With

            Return result

        Catch exc As System.Exception

            ' Error Section.
            result = ""
            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get data from the resource file", vApp:=ACApp, vClass:=ACClass, vMethod:="storeAwayCurrentUsersSetting", excep:=exc)
            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: cboPMExportMediaType_ItemCodeChange
    '
    ' Description: Retrieves the details from setting file on changing the item in PMExportMediaType.
    ' ***************************************************************** '
    Private Sub cboPMExportMediaType_ItemCodeChange() Handles cboPMExportMediaType.ItemCodeChange
        m_lReturn = GetCurrentUsersSetting()
    End Sub

    Private Sub cmdImportSchedule_Click(sender As Object, e As EventArgs) Handles cmdImportSchedule.Click

        Dim purePath As String = Application.StartupPath()
        Dim result As gPMConstants.PMEReturnCode
        Const kImport As String = "Import"
        Const kMethodName As String = "cmdImportSchedule_Click"
        Dim batchFileContentDetails As String = ""
        Dim fileName As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If Task <> gPMConstants.PMEComponentAction.PMView Then
                fileName = kImport & "_" & Now.ToString("yyyyMMddhhmm")
                batchFileContentDetails = purePath & "\SIRIUSIMPORT.EXE "

                'Using w As New StreamWriter(Path.Combine(purePath, fileName))
                '    w.WriteLine(batchFileContentDetails)
                '    w.Close()
                'End Using
            End If
            result = ScheduleJob(batchFileName:=fileName,
                    batchFileContentDetails:=batchFileContentDetails,
                    processName:=kImport,
                    processDescription:=fileName,
                    sender:=sender,
                    eventArg:=e)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("iACTImportExport.cmdImportSchedule_Click", "Unable to schedule job")
            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

        End Try
    End Sub


    Private Sub cmdExportSchedule_Click(sender As Object, e As EventArgs) Handles cmdExportSchedule.Click

        Dim result As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdExportSchedule_Click"
        Dim vParamArray(,) As Object = Nothing
        Dim fileName As String = ""
        Dim batchFileContentDetails As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim interfaceName As String = ""
            Dim processName As String = cboExportList.SelectedItem.ToString()
            Dim processDescription As String = cboExportList.SelectedItem.ToString()

            If Task <> gPMConstants.PMEComponentAction.PMView Then
                result = SetExportParameters(vParamArray:=vParamArray,
                                              interfaceName:=interfaceName,
                                              processName:=processName,
                                              processDescription:=processDescription)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("iACTImportExport.cmdExportSchedule_Click", "Unable to set export parameters")
                End If

                Dim commandLine As New StringBuilder
                Dim lowerBound, upperBound As Integer

                If Information.IsArray(vParamArray) Then
                    lowerBound = vParamArray.GetLowerBound(1)
                    upperBound = vParamArray.GetUpperBound(1)

                    commandLine = New StringBuilder("")

                    For count As Integer = lowerBound To upperBound
                        If Not Object.Equals(vParamArray(0, count), Nothing) And Not Object.Equals(vParamArray(1, count), Nothing) Then
                            commandLine.Append(CStr(vParamArray(0, count)).TrimEnd() & "=""" & CStr(vParamArray(1, count)).TrimEnd() & """ ")
                        End If
                    Next count
                End If

                result = CreateSchedularProcessParameter(vParamArray:=vParamArray)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("iACTImportExport.cmdExportSchedule_Click", "Unable to set export parameters")
                End If

                Dim purePath As String = Application.StartupPath()
                fileName = interfaceName & "_" & Now.ToString("yyyyMMddhhmm")
                batchFileContentDetails = purePath & "\SIRIUSEXPORT.EXE " & interfaceName & " " & commandLine.ToString()

                'Using w As New StreamWriter(Path.Combine(purePath, fileName))
                '    w.WriteLine(batchFileContentDetails)
                '    w.Close()
                'End Using
            End If

            result = ScheduleJob(batchFileName:=fileName,
                        batchFileContentDetails:=batchFileContentDetails,
                        processName:=processName,
                        processDescription:=processDescription, sender:=sender, eventArg:=e)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("iACTImportExport.cmdExportSchedule_Click", "Unable to schedule job")
            End If
        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

        End Try
    End Sub

    Private Function SetProcessDescriptionForDate(processDescription As String) As String
        processDescription &= "_" & dtpStartDate.Value.ToString("yyyy-MM-dd")
        processDescription &= "_" & dtpEndDate.Value.ToString("yyyy-MM-dd")
        Return processDescription
    End Function

    Private Function ScheduleJob(ByVal batchFileName As String, ByVal batchFileContentDetails As String,
                            ByVal processName As String, ByVal processDescription As String, sender As Object, eventArg As EventArgs) As Integer
        Dim frequencySchedular As Object = Nothing
        ScheduleJob = gPMConstants.PMEReturnCode.PMTrue
        Try
            Dim iSIRFrequencyScheduler As iSIRFrequencyScheduler.Interface_Renamed = New iSIRFrequencyScheduler.Interface_Renamed
            ScheduleJob = g_oObjectManager.GetInstance(frequencySchedular, sClassName:="iSIRFrequencyScheduler.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            iSIRFrequencyScheduler = frequencySchedular
            iSIRFrequencyScheduler.BatchProcessId = m_iBatchProcessId
            iSIRFrequencyScheduler.BatchProcessName = m_sBatchProcessName
            iSIRFrequencyScheduler.BatchFileName = batchFileName
            iSIRFrequencyScheduler.BatchFileContentDetails = batchFileContentDetails
            iSIRFrequencyScheduler.Process = processName
            iSIRFrequencyScheduler.ProcessDescription = processDescription
            iSIRFrequencyScheduler.ProcessParameters = m_dtParameters
            iSIRFrequencyScheduler.BatchSchedulerId = m_ibatchSchedulerId
            iSIRFrequencyScheduler.Task = m_iTask
            iSIRFrequencyScheduler.UserName = g_sUsername
            frequencySchedular.Start()
            cmdClose_Click(sender, eventArg)
        Catch
            ScheduleJob = gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Private Sub EnableDisableInterfaceButton(ByVal enabled As Boolean)
        txtExportBatchID.Enabled = enabled
        lblExportBatchID.Enabled = enabled
        txtExportLeadDays.Enabled = enabled
        lblExportLeadDays.Enabled = enabled
        cboPMExportBankAccountName.Enabled = enabled
        lblExportBankAccountName.Enabled = enabled
        cboPMExportMediaType.Enabled = enabled
        lblExportMediaType.Enabled = enabled
        chkExportAutoPost.Enabled = enabled
        lblExportPFSchemeTypeCode.Enabled = enabled
        cboPMExportPFSchemeTypeCode.Enabled = enabled
        lblEventType.Enabled = enabled
        cboEventType.Enabled = enabled

        lblStartDate.Enabled = enabled
        dtpStartDate.Enabled = enabled
        lblEndDate.Enabled = enabled
        dtpEndDate.Enabled = enabled

        lblPeriod.Enabled = enabled
        cboAllPeriod.Enabled = enabled
        cmdExport.Left = lblEventType.Left
        chkExportMetadata.Enabled = enabled
        lblCurrency.Enabled = enabled
        cboCurrency.Enabled = enabled
        lblAgentType.Enabled = enabled
        cmbAgentType.Enabled = enabled
        txtXSLTDestExtension.Enabled = enabled
        lblXSLTDestExtension.Enabled = enabled
        txtXSLTDestFolder.Enabled = enabled
        lblXSLTDestFolder.Enabled = enabled
        txtXSLTFilename.Enabled = enabled
        lblXSLTFilename.Enabled = enabled

    End Sub

    Private Function LoadBatchParameters() As Integer

        Dim ctrl As Object
        Dim controlValue As String = ""
        LoadBatchParameters = gPMConstants.PMEReturnCode.PMTrue
        Try
            If Information.IsArray(m_oBatchParameters) Then
                For lCount As Integer = m_oBatchParameters.GetLowerBound(1) To m_oBatchParameters.GetUpperBound(1)

                    controlValue = (m_oBatchParameters(1, lCount))
                    ctrl = Me.Controls.Find(m_oBatchParameters(0, lCount), True).FirstOrDefault()
                    If Not ctrl Is Nothing Then
                        If (TypeOf ctrl Is TextBox) Then
                            ctrl.Text = controlValue
                        ElseIf (TypeOf ctrl Is ComboBox) Then
                            Select Case ctrl.name
                                Case "cboAllPeriod", "cboExportList"
                                    ctrl.SelectedIndex = controlValue
                                Case "cmbAgentType"
                                    ctrl.SelectedItem = controlValue
                            End Select
                        ElseIf (TypeOf ctrl Is UserControls.CurrencyLookup) Then
                            ctrl.CurrencyId = controlValue
                        ElseIf (TypeOf ctrl Is CheckBox) Then
                            ctrl.Checked = IIf(controlValue = "1", CheckState.Checked, CheckState.Unchecked)
                        ElseIf (TypeOf ctrl Is DateTimePicker) Then
                            ctrl.Value = Date.Parse(controlValue)
                        ElseIf (TypeOf ctrl Is cboPMLookup) Then
                            ctrl.ItemId = CInt(controlValue)
                        End If
                    End If
                Next
            End If
        Catch
            LoadBatchParameters = gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Private Function CreateSchedularProcessParameter(ByVal vParamArray(,) As Object) As Integer

        CreateSchedularProcessParameter = gPMConstants.PMEReturnCode.PMTrue
        Try
            m_dtParameters = New DataTable("Scheduler")

            m_dtParameters.Columns.AddRange(New DataColumn(4) {
                                                        New DataColumn("Id", System.Type.GetType("System.String")),
                                                       New DataColumn("ParameterName", System.Type.GetType("System.String")),
                                                       New DataColumn("DefaultValue", System.Type.GetType("System.String")),
                                                       New DataColumn("DataType", System.Type.GetType("System.String")),
                                                       New DataColumn("CurrentValue", System.Type.GetType("System.String"))})


            Dim lowerBound, upperBound As Integer
            Dim lookupArray() As String
            Dim parameterName As String = ""
            Dim currentValue As String = ""

            m_dtParameters.LoadDataRow(New String(4) {String.Empty, "cboExportList", "", "String", cboExportList.SelectedIndex}, True)
            If Information.IsArray(vParamArray) Then

                lowerBound = vParamArray.GetLowerBound(1)
                upperBound = vParamArray.GetUpperBound(1)

                For count As Integer = lowerBound To upperBound
                    If Not Object.Equals(vParamArray(1, count), Nothing) AndAlso Not Object.Equals(vParamArray(2, count), Nothing) Then
                        parameterName = vParamArray(2, count)
                        currentValue = vParamArray(1, count)
                        If parameterName.IndexOf("_") > 0 Then
                            lookupArray = Strings.Split(parameterName, "_")
                            parameterName = lookupArray(0)
                            currentValue = lookupArray(1)
                        End If
                        m_dtParameters.LoadDataRow(New String(4) {String.Empty, parameterName, "", "", currentValue}, True)
                    End If
                Next count
            End If
        Catch
            CreateSchedularProcessParameter = gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Private Function SetExportParameters(ByRef vParamArray(,) As Object, Optional ByRef interfaceName As String = "", Optional ByRef processName As String = "",
                                          Optional ByRef processDescription As String = "") As Integer

        Dim interfaceId As gPMConstants.PMEExportInterface
        Dim upperBound As Integer

        interfaceId = cboExportList.SelectedIndex
        vParamArray = Nothing
        SetExportParameters = gPMConstants.PMEReturnCode.PMTrue
        Try
            Select Case interfaceId
                Case gPMConstants.PMEExportInterface.pmeIEIInstalmentExport, gPMConstants.PMEExportInterface.pmeIEIPaymentExport, gPMConstants.PMEExportInterface.pmeIEIReceiptExport
                    Select Case interfaceId
                        Case gPMConstants.PMEExportInterface.pmeIEIInstalmentExport
                            interfaceName = "INSTALMENT_EXPORT"
                            processName = "Instalments Export"
                            processDescription = "Instalment"
                        Case gPMConstants.PMEExportInterface.pmeIEIReceiptExport
                            interfaceName = "RECEIPT_EXPORT"
                        Case gPMConstants.PMEExportInterface.pmeIEIPaymentExport
                            interfaceName = "PAYMENT_EXPORT"
                            processName = "Payments Export"
                    End Select

                    upperBound = 0
                    ReDim vParamArray(2, upperBound)
                    vParamArray(0, upperBound) = "MEDIA_TYPE_CODE"
                    vParamArray(1, upperBound) = cboPMExportMediaType.ItemCode.Trim()
                    vParamArray(2, upperBound) = "cboPMExportMediaType" & "_" & cboPMExportMediaType.ItemId
                    processDescription &= "_" & cboPMExportMediaType.ItemCaption.Trim()

                    If cboPMExportBankAccountName.ItemCaption <> "(None)" Then
                        upperBound += 1
                        ReDim Preserve vParamArray(2, upperBound)
                        vParamArray(0, upperBound) = "BANK_ACCOUNT_NAME"
                        vParamArray(1, upperBound) = cboPMExportBankAccountName.ItemCaption.Trim()
                        vParamArray(2, upperBound) = "cboPMExportBankAccountName" & "_" & cboPMExportBankAccountName.ItemId
                        processDescription &= "_" & cboPMExportBankAccountName.ItemCaption.Trim()
                    End If

                    If chkExportAutoPost.CheckState = CheckState.Checked Then
                        upperBound += 1
                        ReDim Preserve vParamArray(2, upperBound)
                        vParamArray(0, upperBound) = "AUTOPOST"
                        vParamArray(1, upperBound) = chkExportAutoPost.CheckState
                        vParamArray(2, upperBound) = "chkExportAutoPost"
                    End If

                    If txtExportLeadDays.Text <> "" Then
                        upperBound += 1
                        ReDim Preserve vParamArray(2, upperBound)
                        vParamArray(0, upperBound) = "LEAD_DAYS"
                        vParamArray(1, upperBound) = txtExportLeadDays.Text
                        vParamArray(2, upperBound) = "txtExportLeadDays"
                        processDescription &= "_" & txtExportLeadDays.Text
                    End If

                    If txtExportBatchID.Text <> "" Then
                        upperBound += 1
                        ReDim Preserve vParamArray(2, upperBound)
                        vParamArray(0, upperBound) = "BATCH_ID"
                        vParamArray(1, upperBound) = txtExportBatchID.Text
                        vParamArray(2, upperBound) = "txtExportBatchID"
                    End If

                Case gPMConstants.PMEExportInterface.pmeIEIInstalmentPlanExport
                    interfaceName = "INSTALMENT_PLAN_EXPORT"
                    processName = "Instalment Plans Export"
                    If cboPMExportPFSchemeTypeCode.ItemCaption <> "(None)" Then
                        upperBound = 0
                        ReDim vParamArray(2, upperBound)
                        vParamArray(0, upperBound) = "SCHEME_TYPE_CODE"
                        vParamArray(1, upperBound) = cboPMExportPFSchemeTypeCode.ItemCode
                        vParamArray(2, upperBound) = "cboPMExportPFSchemeTypeCode" & "_" & cboPMExportPFSchemeTypeCode.ItemId
                        processDescription &= "_" & cboPMExportPFSchemeTypeCode.ItemCaption
                    End If
                    If txtExportBatchID.Text <> "" Then
                        upperBound += 1
                        ReDim Preserve vParamArray(2, upperBound)
                        vParamArray(0, upperBound) = "BATCH_ID"
                        vParamArray(1, upperBound) = txtExportBatchID.Text
                        vParamArray(2, upperBound) = "txtExportBatchID"
                    End If
                Case gPMConstants.PMEExportInterface.pmeIEIMessageExport
                    interfaceName = "MESSAGE_EXPORT"
                    upperBound = 0
                    ReDim vParamArray(2, upperBound)
                    vParamArray(0, upperBound) = "EVENT_TYPE_CODE"
                    vParamArray(1, upperBound) = cboEventType.Text
                    vParamArray(2, upperBound) = "cboEventType"
                    processDescription &= "_" & cboEventType.Text
                    If txtExportBatchID.Text <> "" Then
                        upperBound += 1
                        ReDim Preserve vParamArray(2, upperBound)
                        vParamArray(0, upperBound) = "BATCH_ID"
                        vParamArray(1, upperBound) = txtExportBatchID.Text
                        vParamArray(2, upperBound) = "txtExportBatchID"
                    End If
                Case gPMConstants.PMEExportInterface.pmeIEIDocumentExport
                    interfaceName = "DOCUMENT_EXPORT"
                    upperBound = 0
                    If txtExportBatchID.Text <> "" Then
                        ReDim vParamArray(2, upperBound)
                        vParamArray(0, upperBound) = "BATCH_ID"
                        vParamArray(1, upperBound) = txtExportBatchID.Text
                        vParamArray(2, upperBound) = "txtExportBatchID"
                    End If
                    If dtpStartDate.Checked = True And dtpEndDate.Checked = True Then
                        If Not (Convert.IsDBNull(dtpStartDate.Value) Or IsNothing(dtpStartDate.Value) Or Convert.IsDBNull(dtpEndDate.Value) Or IsNothing(dtpEndDate.Value)) Then
                            If Information.IsArray(vParamArray) Then
                                upperBound += 1
                                ReDim Preserve vParamArray(2, upperBound)
                            Else
                                ReDim vParamArray(2, upperBound)
                            End If
                            vParamArray(0, upperBound) = "START_DATE"
                            vParamArray(1, upperBound) = dtpStartDate.Value
                            vParamArray(2, upperBound) = "dtpStartDate"
                        End If
                    End If
                    If dtpStartDate.Checked = True And dtpEndDate.Checked = True Then
                        If Not (Convert.IsDBNull(dtpStartDate.Value) Or IsNothing(dtpStartDate.Value) Or Convert.IsDBNull(dtpEndDate.Value) Or IsNothing(dtpEndDate.Value)) Then
                            If Information.IsArray(vParamArray) Then
                                upperBound += 1
                                ReDim Preserve vParamArray(2, upperBound)
                            Else
                                ReDim vParamArray(2, upperBound)
                            End If
                            vParamArray(0, upperBound) = "END_DATE"
                            vParamArray(1, upperBound) = dtpEndDate.Value
                            vParamArray(2, upperBound) = "dtpEndDate"
                        End If
                        processDescription = SetProcessDescriptionForDate(processDescription)
                    End If
                    If Not chkExportMetadata.Checked Then
                        If Information.IsArray(vParamArray) Then
                            upperBound += 1
                            ReDim Preserve vParamArray(2, upperBound)
                        Else
                            ReDim vParamArray(2, upperBound)
                        End If
                        vParamArray(0, upperBound) = "EXPORT_METADATA"
                        vParamArray(1, upperBound) = "False"
                        vParamArray(2, upperBound) = "chkExportMetadata_0"
                    End If
                Case gPMConstants.PMEExportInterface.pmeIEIPolicyBatchExport
                    interfaceName = "POLICY_BATCH_EXPORT"
                    upperBound = 0
                    If txtExportBatchID.Text <> "" Then
                        ReDim vParamArray(2, upperBound)
                        vParamArray(0, upperBound) = "BATCH_ID"
                        vParamArray(1, upperBound) = txtExportBatchID.Text
                        vParamArray(2, upperBound) = "txtExportBatchID"
                    End If
                    If dtpStartDate.Checked = True And dtpEndDate.Checked = True Then
                        If Not (Convert.IsDBNull(dtpStartDate.Value) Or IsNothing(dtpStartDate.Value) Or Convert.IsDBNull(dtpEndDate.Value) Or IsNothing(dtpEndDate.Value)) Then
                            If Information.IsArray(vParamArray) Then
                                upperBound += 1
                                ReDim Preserve vParamArray(2, upperBound)
                            Else
                                ReDim vParamArray(2, upperBound)
                            End If
                            vParamArray(0, upperBound) = "START_DATE"
                            vParamArray(1, upperBound) = dtpStartDate.Value.ToShortDateString()
                            vParamArray(2, upperBound) = "dtpStartDate"
                        End If
                    End If
                    ' If start date and end date have been provided
                    If dtpStartDate.Checked = True And dtpEndDate.Checked = True Then
                        If Not (Convert.IsDBNull(dtpStartDate.Value) Or IsNothing(dtpStartDate.Value) Or Convert.IsDBNull(dtpEndDate.Value) Or IsNothing(dtpEndDate.Value)) Then
                            If Information.IsArray(vParamArray) Then
                                upperBound += 1
                                ReDim Preserve vParamArray(2, upperBound)
                            Else
                                ReDim vParamArray(2, upperBound)
                            End If
                            vParamArray(0, upperBound) = "END_DATE"
                            vParamArray(1, upperBound) = dtpEndDate.Value.ToShortDateString()
                            vParamArray(2, upperBound) = "dtpEndDate"
                        End If
                        processDescription = SetProcessDescriptionForDate(processDescription)
                    End If
                Case gPMConstants.PMEExportInterface.pmeIEIClaimExport
                    interfaceName = "CLAIMS_EXPORT"
                    processName = "Claims Export"
                    upperBound = 0
                    If txtExportBatchID.Text <> "" And txtExportBatchID.Text <> "0" Then
                        ReDim vParamArray(2, upperBound)
                        vParamArray(0, upperBound) = "BATCH_ID"
                        vParamArray(1, upperBound) = txtExportBatchID.Text
                        vParamArray(2, upperBound) = "txtExportBatchID"
                    End If
                    If dtpStartDate.Checked = True And dtpEndDate.Checked = True Then
                        If Not (Convert.IsDBNull(dtpStartDate.Value) Or IsNothing(dtpStartDate.Value) Or Convert.IsDBNull(dtpEndDate.Value) Or IsNothing(dtpEndDate.Value)) Then
                            If Information.IsArray(vParamArray) Then
                                upperBound += 1
                                ReDim Preserve vParamArray(2, upperBound)
                            Else
                                ReDim vParamArray(2, upperBound)
                            End If
                            vParamArray(0, upperBound) = "DATE_FROM"
                            vParamArray(1, upperBound) = dtpStartDate.Value.ToShortDateString()
                            vParamArray(2, upperBound) = "dtpStartDate"
                        End If
                    End If
                    If dtpStartDate.Checked = True And dtpEndDate.Checked = True Then
                        If Not (Convert.IsDBNull(dtpStartDate.Value) Or IsNothing(dtpStartDate.Value) Or Convert.IsDBNull(dtpEndDate.Value) Or IsNothing(dtpEndDate.Value)) Then
                            If Information.IsArray(vParamArray) Then
                                upperBound += 1
                                ReDim Preserve vParamArray(2, upperBound)
                            Else
                                ReDim vParamArray(2, upperBound)
                            End If
                            vParamArray(0, upperBound) = "DATE_TO"
                            vParamArray(1, upperBound) = dtpEndDate.Value.ToShortDateString()
                            vParamArray(2, upperBound) = "dtpEndDate"
                            processDescription = SetProcessDescriptionForDate(processDescription)
                        End If
                    End If
                    ' If start date or end date have been provided
                    If dtpStartDate.Checked And dtpEndDate.Checked Then
                        If Not ((Convert.IsDBNull(dtpStartDate.Value) Or IsNothing(dtpStartDate.Value)) And (Convert.IsDBNull(dtpEndDate.Value) Or IsNothing(dtpEndDate.Value))) Then
                            If Information.IsArray(vParamArray) Then
                                upperBound += 1
                                ReDim Preserve vParamArray(2, upperBound)
                            Else
                                ReDim vParamArray(2, upperBound)
                            End If
                            vParamArray(0, upperBound) = "EXPORT_BY_DATE"
                            vParamArray(1, upperBound) = "True"
                        End If
                    End If
                    'Check added for optional metadata export
                    If Not chkExportMetadata.Checked Then
                        If Information.IsArray(vParamArray) Then
                            upperBound += 1
                            ReDim Preserve vParamArray(2, upperBound)
                        Else
                            ReDim vParamArray(2, upperBound)
                        End If
                        vParamArray(0, upperBound) = "EXPORT_METADATA"
                        vParamArray(1, upperBound) = "False"
                        vParamArray(2, upperBound) = "chkExportMetadata"
                    End If

                Case gPMConstants.PMEExportInterface.pmeIEIGLExport
                    interfaceName = "GL_EXPORT"
                    processName = "General Ledger Export"
                    processDescription = "General Ledger"
                    upperBound = 0

                    If txtExportBatchID.Text <> "" And txtExportBatchID.Text <> "0" Then
                        ReDim vParamArray(2, upperBound)
                        vParamArray(0, upperBound) = "BATCH_ID"
                        vParamArray(1, upperBound) = txtExportBatchID.Text
                        vParamArray(2, upperBound) = "txtExportBatchID"
                    End If

                    If CType(cboAllPeriod.SelectedItem, ValueDescriptionPair).Value > 0 Then
                        If Information.IsArray(vParamArray) Then
                            upperBound += 1
                            ReDim Preserve vParamArray(2, upperBound)
                        Else
                            ReDim vParamArray(2, upperBound)
                        End If
                        vParamArray(0, upperBound) = "PERIOD_ID"
                        vParamArray(1, upperBound) = CType(cboAllPeriod.SelectedItem, ValueDescriptionPair).Value
                        vParamArray(2, upperBound) = "cboAllPeriod"
                        processDescription &= "_" & CType(cboAllPeriod.SelectedItem, ValueDescriptionPair).Description

                    Else
                        processDescription &= "_All"
                    End If

                Case gPMConstants.PMEExportInterface.pmeIEIPolicyExport
                    interfaceName = "POLICY_EXPORT"
                    processName = "Policies Export"

                    upperBound = 0

                    'Check added for optional metadata export
                    If Not chkExportMetadata.Checked Then
                        If Information.IsArray(vParamArray) Then
                            upperBound += 1
                            ReDim Preserve vParamArray(2, upperBound)
                        Else
                            ReDim vParamArray(2, upperBound)
                        End If
                        vParamArray(0, upperBound) = "EXPORT_METADATA"
                        vParamArray(1, upperBound) = "False"
                        vParamArray(2, upperBound) = "chkExportMetadata"
                    End If


                Case gPMConstants.PMEExportInterface.pmeIEIMIDExport
                    interfaceName = "MID_EXPORT"
                    upperBound = 0
                    If txtExportBatchID.Text <> "" Then
                        ReDim vParamArray(2, upperBound)
                        vParamArray(0, upperBound) = "BATCH_ID"
                        vParamArray(1, upperBound) = txtExportBatchID.Text
                        vParamArray(2, upperBound) = "txtExportBatchID"
                    End If

                Case gPMConstants.PMEExportInterface.pmeIEIMID2Export
                    interfaceName = "MID2_EXPORT"
                    upperBound = 0
                    If txtExportBatchID.Text.Trim() <> "" Then
                        ReDim vParamArray(2, upperBound)
                        vParamArray(0, upperBound) = "BATCH_ID"
                        vParamArray(1, upperBound) = txtExportBatchID.Text
                        vParamArray(2, upperBound) = "txtExportBatchID"
                    End If
                Case gPMConstants.PMEExportInterface.pmeIEICommissionExport
                    interfaceName = "COMMISSION_EXPORT"
                    processDescription = "Commission"
                    upperBound = 0
                    ReDim vParamArray(2, upperBound)
                    vParamArray(0, upperBound) = "MEDIA_TYPE_CODE"
                    vParamArray(1, upperBound) = cboPMExportMediaType.ItemCode.Trim()
                    vParamArray(2, upperBound) = "cboPMExportMediaType" & "_" & cboPMExportMediaType.ItemId
                    processDescription &= "_" & cboPMExportMediaType.ItemCaption.Trim()

                    If txtExportLeadDays.Text <> "" Then
                        upperBound += 1
                        ReDim Preserve vParamArray(2, upperBound)
                        vParamArray(0, upperBound) = "LEAD_DAYS"
                        vParamArray(1, upperBound) = txtExportLeadDays.Text
                        vParamArray(2, upperBound) = "txtExportLeadDays"
                        processDescription &= "_" & txtExportLeadDays.Text
                    End If
                    If cboCurrency.CurrencyId <> "0" Then
                        upperBound += 1
                        ReDim Preserve vParamArray(2, upperBound)
                        vParamArray(0, upperBound) = "CURRENCY_ID"
                        vParamArray(1, upperBound) = cboCurrency.CurrencyId
                        vParamArray(2, upperBound) = "cboCurrency"
                        processDescription &= "_" & cboCurrency.Text
                    End If
                    If DirectCast(cmbAgentType.SelectedItem, String) <> "(ALL)" Then
                        upperBound += 1
                        ReDim Preserve vParamArray(2, upperBound)
                        vParamArray(0, upperBound) = "AGENT_TYPE_CODE"
                        vParamArray(1, upperBound) = DirectCast(cmbAgentType.SelectedItem, String)
                        vParamArray(2, upperBound) = "cmbAgentType"
                        processDescription &= "_" & cmbAgentType.SelectedItem
                    End If
                    If Not (Convert.IsDBNull(dtpStartDate.Value) OrElse IsNothing(dtpStartDate.Value)) Then
                        If Information.IsArray(vParamArray) Then
                            upperBound += 1
                            ReDim Preserve vParamArray(2, upperBound)
                        Else
                            ReDim vParamArray(2, upperBound)
                        End If
                        vParamArray(0, upperBound) = "ALLOCATION_DATE_FROM"
                        vParamArray(1, upperBound) = dtpStartDate.Value.ToShortDateString()
                        vParamArray(2, upperBound) = "dtpStartDate"
                    End If

                    If Not (Convert.IsDBNull(dtpEndDate.Value) OrElse IsNothing(dtpEndDate.Value)) Then
                        If Information.IsArray(vParamArray) Then
                            upperBound += 1
                            ReDim Preserve vParamArray(2, upperBound)
                        Else
                            ReDim vParamArray(2, upperBound)
                        End If
                        vParamArray(0, upperBound) = "ALLOCATION_DATE_TO"
                        vParamArray(1, upperBound) = dtpEndDate.Value.ToShortDateString()
                        vParamArray(2, upperBound) = "dtpEndDate"
                    End If

                    If cboPMExportBankAccountName.ItemCaption <> "(None)" Then
                        upperBound += 1
                        ReDim Preserve vParamArray(2, upperBound)
                        vParamArray(0, upperBound) = "BANK_ACCOUNT_NAME"
                        vParamArray(1, upperBound) = cboPMExportBankAccountName.ItemCaption.Trim()
                        vParamArray(2, upperBound) = "cboPMExportBankAccountName" & "_" & cboPMExportBankAccountName.ItemId
                        processDescription &= "_" & cboPMExportBankAccountName.ItemCaption.Trim()
                    End If
            End Select
            If txtXSLTFilename.Text <> "" Then
                If IsArray(vParamArray) Then
                    upperBound = upperBound + 1
                    ReDim Preserve vParamArray(2, upperBound)
                Else
                    ReDim vParamArray(2, upperBound)
                End If
                vParamArray(0, upperBound) = "XSLT_FILE_NAME"
                vParamArray(1, upperBound) = txtXSLTFilename.Text
                vParamArray(2, upperBound) = "txtXSLTFilename"
            End If

            If txtXSLTDestExtension.Text <> "" Then
                If IsArray(vParamArray) Then
                    upperBound = upperBound + 1
                    ReDim Preserve vParamArray(2, upperBound)
                Else
                    ReDim vParamArray(2, upperBound)
                End If
                vParamArray(0, upperBound) = "DEST_FILE_EXTN"
                vParamArray(1, upperBound) = txtXSLTDestExtension.Text
                vParamArray(2, upperBound) = "txtXSLTDestExtension"
            End If

            If txtXSLTDestFolder.Text <> "" Then
                If IsArray(vParamArray) Then
                    upperBound = upperBound + 1
                    ReDim Preserve vParamArray(2, upperBound)
                Else
                    ReDim vParamArray(2, upperBound)
                End If
                vParamArray(0, upperBound) = "DEST_FOLDER"
                vParamArray(1, upperBound) = IIf(Strings.Right(txtXSLTDestFolder.Text, 1) = "\", Strings.Left(txtXSLTDestFolder.Text, Len(txtXSLTDestFolder.Text) - 1), txtXSLTDestFolder.Text)
                vParamArray(2, upperBound) = "txtXSLTDestFolder"
            End If


            ' If start date or end date have been provided
            If dtpStartDate.Checked And dtpEndDate.Checked Then
                If Not ((Convert.IsDBNull(dtpStartDate.Value) Or IsNothing(dtpStartDate.Value)) And (Convert.IsDBNull(dtpEndDate.Value) Or IsNothing(dtpEndDate.Value))) Then
                    If Information.IsArray(vParamArray) Then
                        upperBound += 1
                        ReDim Preserve vParamArray(2, upperBound)
                    Else
                        ReDim vParamArray(2, upperBound)
                    End If

                    vParamArray(0, upperBound) = "OVERRIDE_BATCH_SELECT"

                    vParamArray(1, upperBound) = "True"
                End If
            End If
            processDescription &= "_" & Now.ToString("yyyyMMddhhmm")
        Catch
            SetExportParameters = gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    'Private Sub chkExportDueDay_CheckedChanged(sender As Object, e As EventArgs) Handles chkExportDueDay.CheckedChanged
    '    If chkExportDueDay.Checked Then
    '        cboExportDueDay.Enabled = True
    '        cboExportDueDay.Visible = True
    '    Else
    '        cboExportDueDay.Enabled = False
    '        cboExportDueDay.Visible = False
    '    End If

    'End Sub
    Private Sub chkExportStrikeDay_CheckedChanged(sender As Object, e As EventArgs) Handles chkExportDueDay.CheckedChanged
        If chkExportDueDay.CheckState = CheckState.Checked Then
            cboExportDueDay.Visible = True
            cboExportDueDay.SelectedIndex = 0
        Else
            cboExportDueDay.Visible = False
            cboExportDueDay.SelectedIndex = -1
        End If
    End Sub
End Class
