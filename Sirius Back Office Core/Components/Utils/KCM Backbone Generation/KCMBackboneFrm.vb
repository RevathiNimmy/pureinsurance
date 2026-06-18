Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Imports PMResizerControl
Imports System.Collections.Generic

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    Public Const vbFormCode As Integer = 0
    Private Const ACClass As String = "frmInterface"
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDateProperty As Date
    Private m_oGeneral As KCMBackboneGeneration.General
    Private m_oBusiness As Object
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_dtEffectiveDate As Date

    Private Const m_iSourceid As Short = 1
    Private Const m_iLanguageID As Short = 1

    Public Property ErrorNumber() As Integer
        Get
            Return m_lErrorNumber
        End Get
        Set(ByVal Value As Integer)
            m_lErrorNumber = Value
        End Set
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
            m_dtEffectiveDateProperty = Value
        End Set
    End Property


    Private headerCheckBox As CheckBox = New CheckBox()
    Public Function BusinessToData(ByVal aoDataModels(,) As Object) As Integer
        Dim result As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim dt As DataTable = New DataTable()
            dt.Columns.Add(New DataColumn("Data Model ID"))
            dt.Columns.Add(New DataColumn("Data Model Code"))
            dt.Columns.Add(New DataColumn("Data Model Description"))
            dt.Columns.Add(New DataColumn("Data Model Type"))

            For i As Integer = 0 To aoDataModels.GetLength(1) - 1
                Dim row As DataRow = dt.NewRow()
                For j As Integer = 0 To aoDataModels.GetLength(0) - 1
                    row(j) = aoDataModels(j, i)
                Next
                dt.Rows.Add(row)
            Next

            dgvGISDataModels.DataSource = dt
            dgvGISDataModels.AutoResizeColumns(DataGridViewAutoSizeColumnMode.ColumnHeader)
            dgvGISDataModels.AutoResizeColumns()

            'Find the Location of Header Cell.
            Dim headerCellLocation As Point = dgvGISDataModels.GetCellDisplayRectangle(0, -1, True).Location

            'Place the Header CheckBox in the Location of the Header Cell.
            headerCheckBox.Location = New Point(headerCellLocation.X + 15, headerCellLocation.Y + 4)
            headerCheckBox.BackColor = Color.Transparent
            headerCheckBox.Size = New Size(18, 18)

            'Assign Click event to the Header CheckBox.
            AddHandler headerCheckBox.Click, AddressOf HeaderCheckBox_Clicked
            dgvGISDataModels.Controls.Add(headerCheckBox)

            Dim checkBoxColumn As New DataGridViewCheckBoxColumn()
            checkBoxColumn.HeaderText = ""
            checkBoxColumn.Width = 40
            checkBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            checkBoxColumn.Name = "chkSelectDataModel"
            dgvGISDataModels.Columns.Insert(0, checkBoxColumn)
            AddHandler dgvGISDataModels.CellContentClick, AddressOf DataGridView_CellClick

            dgvGISDataModels.Columns("Data Model ID").Visible = False

            Return result
        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data to grid", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Sub HeaderCheckBox_Clicked(ByVal sender As Object, ByVal e As EventArgs)

        dgvGISDataModels.EndEdit()

        'Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
        For Each row As DataGridViewRow In dgvGISDataModels.Rows
            Dim checkBox As DataGridViewCheckBoxCell = (TryCast(row.Cells("chkSelectDataModel"), DataGridViewCheckBoxCell))
            checkBox.Value = headerCheckBox.Checked
        Next

    End Sub

    Private Sub DataGridView_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs)

        If e.RowIndex >= 0 AndAlso e.ColumnIndex = 0 Then

            'Loop to verify whether all row CheckBoxes are checked or not.
            Dim isChecked As Boolean = True
            For Each row As DataGridViewRow In dgvGISDataModels.Rows
                If Convert.ToBoolean(row.Cells("chkSelectDataModel").EditedFormattedValue) = False Then
                    isChecked = False
                    Exit For
                End If
            Next

            headerCheckBox.Checked = isChecked
        End If
    End Sub

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

            Return result
        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try

    End Function

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
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(13) & Strings.Chr(10) &
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If

            cmdOk.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGenerateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result
        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try

    End Function

    Private Sub Form_Initialize_Renamed()
        Dim sMessage, sTitle As String

        Try
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMCurrency.Form", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New KCMBackboneGeneration.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try

    End Sub

    Public Sub frmInterfaceLoad()
        Try
            iPMFunc.ShowFormInTaskBar_Detach()
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
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

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Try
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If UnloadMode <> vbFormCode Then

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    'developer guide no.7
                    eventArgs.Cancel = True
                    Cancel = 1

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            m_oGeneral = Nothing
            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
        Catch excep As System.Exception
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
            eventArgs.Cancel = Cancel <> 0
        End Try
    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
        Try
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_lReturn = CType(ValidateOK(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            cmdOk.Enabled = False
            dgvGISDataModels.Enabled = False

            m_lReturn = CType(GenerateKCMBackbone(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="Failed to generate KCM backbone for selected Data Models",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click",
                                   vErrNo:=Information.Err().Number, vErrDesc:="KCM backbone generation failed.")
            End If

            cmdOk.Enabled = True
            dgvGISDataModels.Enabled = True

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Dim iMsgResult As DialogResult
        Try
            iMsgResult = MessageBox.Show("Do you want to cancel the Backobone Generation",
                                         "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                         MessageBoxDefaultButton.Button1)

            If iMsgResult = System.Windows.Forms.DialogResult.Yes Then
                Me.Hide()
                Me.Close()
            End If

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try

    End Sub

    Private Function ValidateOK() As Integer

        Dim result As Integer
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'Update the array
            Dim isSelected As Boolean = False
            Dim message As String = String.Empty

            For Each row As DataGridViewRow In dgvGISDataModels.Rows
                isSelected = Convert.ToBoolean(row.Cells("chkSelectDataModel").Value)
                If isSelected Then
                    Exit For
                End If
            Next

            If Not isSelected Then
                MessageBox.Show("At least one DataModel must be selected for backbone generation.",
                                "DataModel Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Dim iMsgResult As DialogResult
            iMsgResult = MessageBox.Show("The backbone generation process might take some time, do you wish to continue.",
                                         "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If iMsgResult = System.Windows.Forms.DialogResult.No Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOKFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result

        End Try

    End Function

    Public Function GetBusiness() As Object

        GetBusiness = gPMConstants.PMEReturnCode.PMTrue

        m_oBusiness = New bGISMaintainDataDictionary.Business

        m_lReturn = m_oBusiness.SetProcessModes(vTask:=CObj(m_iTask), vNavigate:=CObj(m_lNavigate), vProcessMode:=CObj(m_lProcessMode), vTransactionType:=CObj(m_sTransactionType), vEffectiveDate:=CObj(m_dtEffectiveDate))

        ' Check for errors.
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

            GetBusiness = gPMConstants.PMEReturnCode.PMFalse

            SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

            Exit Function
        End If

        m_lReturn = m_oBusiness.Initialise(sUserName:=m_sUserName, sPassword:=m_sPassword, iUserID:=0, iSourceID:=m_iSourceid, iLanguageID:=m_iLanguageID, iCurrencyID:=1, iLogLevel:=0, sCallingAppName:=ACApp)

        ' Check for errors.
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object GetBusiness. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            GetBusiness = gPMConstants.PMEReturnCode.PMFalse

            SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

            Exit Function
        End If

    End Function

    Public Function GetDataModels(ByRef r_vDataModels(,) As Object) As Integer

        Dim oDatabase As dPMDAO.Database
        Dim sSQL As String = ""

        GetDataModels = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = gPMComponentServices.NewDatabase(v_sUsername:=m_sUserName, v_iSourceID:=m_iSourceid, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_oDatabase:=oDatabase)

        sSQL = sSQL & "SELECT" & vbCrLf
        sSQL = sSQL & " gdm.gis_data_model_id," & vbCrLf
        sSQL = sSQL & " gdm.code," & vbCrLf
        sSQL = sSQL & " gdm.description," & vbCrLf
        sSQL = sSQL & " gdt.description" & vbCrLf
        sSQL = sSQL & " FROM GIS_data_model gdm" & vbCrLf
        sSQL = sSQL & "   INNER JOIN GIS_Data_Model_Type gdt ON gdm.gis_data_model_type_id = gdt.gis_data_model_type_id" & vbCrLf
        sSQL = sSQL & " WHERE gdm.is_deleted = 0" & vbCrLf
        sSQL = sSQL & " And CONVERT(DATE,gdm.effective_date,103) <= CONVERT(DATE, GETDATE(),103)" & vbCrLf
        sSQL = sSQL & " AND EXISTS" & vbCrLf
        sSQL = sSQL & " (" & vbCrLf
        sSQL = sSQL & "   SELECT" & vbCrLf
        sSQL = sSQL & "   NULL" & vbCrLf
        sSQL = sSQL & "   FROM sysobjects so" & vbCrLf
        sSQL = sSQL & "   WHERE so.name = RTRIM(gdm.code) + '_policy_binder'" & vbCrLf
        sSQL = sSQL & "   AND so.xtype = 'U'" & vbCrLf
        sSQL = sSQL & "   AND gdm.code NOT LIKE 'GII%'" & vbCrLf
        sSQL = sSQL & " )"

        ' Execute SQL Statement
        m_lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDataModels", bStoredProcedure:=False, vResultArray:=r_vDataModels)

        oDatabase = Nothing
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GetDataModels = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

    End Function


    ''' <summary>
    ''' Generates KCM backbone for selected datamodels
    ''' </summary>
    ''' <returns></returns>
    Public Function GenerateKCMBackbone() As Integer

        Dim result As Integer = 0
        Dim isSelected As Boolean = False
        Dim totalSelected As Integer = 0
        Dim gisDataModelCode As String = String.Empty
        Dim kcmDocumentProduction As String = String.Empty
        Dim recreateKCMBackbone As Boolean = False
        Dim includeCoreFields As Boolean = False

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oDatabase As dPMDAO.Database = Nothing
            m_lReturn = gPMComponentServices.NewDatabase(v_sUsername:=m_sUserName, v_iSourceID:=m_iSourceid,
                                                         v_iLanguageID:=m_iLanguageID,
                                                         v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions,
                                                         r_oDatabase:=oDatabase)

            'Get system option for KCMDocumentProduction
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=0,
                                                v_iMainSourceID:=m_iSourceid, v_iLanguageID:=m_iLanguageID,
                                                v_iCurrencyID:=1, v_iLogLevel:=0, v_sCallingAppName:=ACApp,
                                                v_iOptionNumber:=GeneralConst.kSystemOptionDocumentProductionSystem,
                                                r_sOptionValue:=kcmDocumentProduction)

            'if CCM is enabled or SCH is Enabled
            If kcmDocumentProduction = "1" OrElse kcmDocumentProduction = "2" Then
                recreateKCMBackbone = True
                includeCoreFields = chkIncludeCoreFieldsets.Checked

                Dim oCCMDocumentProdBusiness As bCCMDocumentProduction.Business = New bCCMDocumentProduction.Business
                'result = oCCMDocumentProdBusiness.Initialise(m_sUserName, m_sPassword, m_iUserID, m_iSourceid, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, vDatabase:=(oDatabase))

                result = oCCMDocumentProdBusiness.Initialise(m_sUserName, m_sPassword, 1, m_iSourceid, m_iLanguageID, 1, 1, ACApp, vDatabase:=(oDatabase))
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                For Each row As DataGridViewRow In dgvGISDataModels.Rows
                    If Convert.ToBoolean(row.Cells("chkSelectDataModel").Value) Then
                        totalSelected += 1
                        gisDataModelCode = String.Format("{0},{1}", gisDataModelCode, Convert.ToString(row.Cells("Data Model Code").Value))
                    End If
                Next

                pBar.Minimum = 0
                pBar.Maximum = totalSelected
                gisDataModelCode = gisDataModelCode.Trim(New Char() {","})
                Dim selectedgisDataModels As String = gisDataModelCode

                Dim arrayOfSelectedDataModels As String() = Nothing
                arrayOfSelectedDataModels = gisDataModelCode.Split(New Char() {","c})

                For Each element As String In arrayOfSelectedDataModels

                    pBar.Value = pBar.Value + 1
                    Dim pBarPercent As Integer = (pBar.Value / pBar.Maximum) * 100
                    ''lblPBarPercent.Text = pBarPercent.ToString() + "% completed"

                    gisDataModelCode = element.ToString()

                    If gisDataModelCode.Trim.Length > 0 AndAlso totalSelected > 0 Then
                        result = oCCMDocumentProdBusiness.CCMRecreateDataSets(sGISDataModelCode:=selectedgisDataModels,
                                                                              bRecreateDataBackBone:=recreateKCMBackbone,
                                                                              bIncludeCoreFieldset:=includeCoreFields)
                        If result <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                Next

            End If

            oDatabase = Nothing
            Dim iMsgResult As DialogResult
            iMsgResult = MessageBox.Show("Backbone generation process has completed.", "Complete",
                                         MessageBoxButtons.OK, MessageBoxIcon.Information)
            If iMsgResult = System.Windows.Forms.DialogResult.OK Then
                Me.Hide()
                Me.Close()
            End If

            Return result

        Catch excep As System.Exception

            pBar.Value = 0
            lblPBarPercent.Text = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try

    End Function

    Private Sub frmInterface_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        Dim kcmDocumentProduction As String = String.Empty
        'Get system option for KCMDocumentProduction
        m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=0,
                                                v_iMainSourceID:=m_iSourceid, v_iLanguageID:=m_iLanguageID,
                                                v_iCurrencyID:=1, v_iLogLevel:=0, v_sCallingAppName:=ACApp,
                                                v_iOptionNumber:=GeneralConst.kSystemOptionDocumentProductionSystem,
                                                r_sOptionValue:=kcmDocumentProduction)

        If Not (kcmDocumentProduction = "1" OrElse kcmDocumentProduction = "2") Then
            Dim iMsgResult As DialogResult
            iMsgResult = MessageBox.Show("KCM Backbone cannot be generated if KCM Document Production system is not selected at the system options.",
                                         "Required System Option", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If iMsgResult = DialogResult.OK Then
                Me.Hide()
                Me.Close()
            End If
        End If

    End Sub

End Class
