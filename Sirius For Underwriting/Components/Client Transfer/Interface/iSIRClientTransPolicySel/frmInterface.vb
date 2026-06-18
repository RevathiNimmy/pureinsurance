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
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date:15/07/00
    '
    ' Description: Main interface.
    '
    ' Edit History:Saurabh Agrawal
    ' ***************************************************************** '

    Private bCancelTheForm As Boolean = False
    Private Const vbFormCode As Integer = 0
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    Private m_bIsInitialised As Boolean
    'Constants for Defining Width of Columns in List View

    Private m_oFormFields As iPMFormControl.FormFields

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private m_iLanguageID As Integer
    Private m_iSourceID As Integer
    Private m_iUserId As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_iTask As Integer

    Private m_lReturn As Integer

    Private m_oBusiness As bSIRFindInsurance.Form

    Private m_vResultArray(,) As Object
    Private m_lFromClientCnt As Integer
    Private m_lToClientCnt As Integer
    Private m_sFromClientCode As String = ""
    Private m_sToClientCode As String = ""
    Private m_iChecked As Integer
    Private m_vSelectedPolicies As Object
    Private m_sOptionValue As String = String.Empty
    Private m_nCountActivePlan As Integer

    Public WriteOnly Property FromClientCode() As String
        Set(ByVal Value As String)

            m_sFromClientCode = Value

        End Set
    End Property

    Public WriteOnly Property ToClientCode() As String
        Set(ByVal Value As String)

            m_sToClientCode = Value

        End Set
    End Property

    Public WriteOnly Property FromClientCnt() As Integer
        Set(ByVal Value As Integer)

            m_lFromClientCnt = Value

        End Set
    End Property

    Public WriteOnly Property ToClientCnt() As Integer
        Set(ByVal Value As Integer)

            m_lToClientCnt = Value

        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get

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


    'DC180202
    Public Property Task() As Integer
        Get

            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            m_iTask = Value

        End Set
    End Property

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        bCancelTheForm = True
        Me.Hide()

    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
        m_lReturn = Validate_Renamed()
        Dim iYesNo As DialogResult


        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

            'iYesNo = MessageBox.Show("You are about to move " & gPMFunctions.ToSafeString(m_iChecked) & " policies from client " & m_sFromClientCode & " To Client " & m_sToClientCode, Application.ProductName, MessageBoxButtons.OKCancel)
            iYesNo = MessageBox.Show("You are about to move " & gPMFunctions.ToSafeString(m_iChecked) & " policies from client " & m_sFromClientCode & " To Client " & m_sToClientCode, "iSIRClientTransPolicySel", MessageBoxButtons.OKCancel)

            If iYesNo = System.Windows.Forms.DialogResult.OK Then
                m_lReturn = TransferPolicies()

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    MessageBox.Show(gPMFunctions.ToSafeString(m_iChecked) & " Policies transferred", "Client Portfolio Transfer")
                    m_lStatus = gPMConstants.PMEReturnCode.PMOK

                Else
                    m_lStatus = gPMConstants.PMEReturnCode.PMError
                End If
                bCancelTheForm = False
                Me.Hide()


            End If


        End If



    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        m_lReturn = SetupPolicyDetailsListView()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And gPMConstants.PMEReturnCode.PMNotFound Then
            gPMFunctions.RaiseError("Form_Load", "SetupPolicyDetailsListView Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
        m_lReturn = SearchPolicies()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And gPMConstants.PMEReturnCode.PMNotFound Then
            gPMFunctions.RaiseError("Form_Load", "SearchPolicies Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        m_lReturn = DataToInterface()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("Form_Load", "DataToInterface Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        Exit Sub
    End Sub


    Private Function SearchPolicies() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SearchPolicies"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRFindInsurance.Form", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError(kMethodName, "bSIRFindInsurance Initalization  Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            m_lReturn = m_oBusiness.SearchAll(r_vResultArray:=m_vResultArray, v_vShortName:=m_sFromClientCode, v_vPartyCnt:=m_lFromClientCnt, v_vInsFileType:="ALL", v_vInsuranceRef:="")


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError(kMethodName, "g_oBusiness.SearchAll Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing





        End Try
        Return result
    End Function


    Public Function DataToInterface() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DataToInterface"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            txtClientFrom.Text = m_sFromClientCode
            txtCLientTo.Text = m_sToClientCode


            m_lReturn = PopulatePolicyDetailsList()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("DataToInterface", "PopulatePolicyDetailsList", gPMConstants.PMELogLevel.PMLogError)
            End If
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
    ''' <summary>
    ''' This method is used to populate the list view of the policies
    ''' </summary>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function PopulatePolicyDetailsList() As Integer

        Dim result As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "PopulatePolicyDetailsList"

        Try
            
            Dim oListItem As ListViewItem
            Const knRestrict_Client_Portfolio_Transfer_on_Active_Instalment As Integer = 5258

            'Get System Option For Restrict_Client_Portfolio_Transfer_on_Active_Instalment

            If iPMFunc.GetSystemOption( _
                     v_iOptionNumber:=knRestrict_Client_Portfolio_Transfer_on_Active_Instalment, _
                     r_sOptionValue:=m_sOptionValue) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                iPMFunc.LogMessage( _
                                    iType:=gPMConstants.PMELogLevel.PMLogOnError, _
                                    sMsg:="Failed to Retrieve System Option for " & knRestrict_Client_Portfolio_Transfer_on_Active_Instalment & " ", _
                                    vApp:=ACApp, _
                                    vClass:=ACClass, _
                                    vMethod:="PopulatePolicyDetailsList", _
                                    vErrNo:=Err.Number, _
                                    vErrDesc:=Err)
                Return result

            End If

            'Set max rows to number of addresses - though must be at least 5
            lvwPolicies.Items.Clear()

            If gPMFunctions.IsArrayEmpty(m_vResultArray) Then
                Return result
            End If

            For iRowCount As Integer = m_vResultArray.GetLowerBound(1) To m_vResultArray.GetUpperBound(1)

                If Not m_vResultArray(MainModule.ENPolicy.InsuranceRef, iRowCount) Is Nothing Then
                    oListItem = lvwPolicies.Items.Add(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=gPMFunctions.ToSafeString(m_vResultArray(MainModule.ENPolicy.InsuranceRef, iRowCount)).Trim()))

                    If Not m_vResultArray(MainModule.ENPolicy.TypeDesc, iRowCount) Is Nothing Then
                        ListViewHelper.GetListViewSubItem(oListItem, kPolicyColHIndexType).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=gPMFunctions.ToSafeString(m_vResultArray(MainModule.ENPolicy.TypeDesc, iRowCount)).Trim())
                    End If

                    If Not m_vResultArray(MainModule.ENPolicy.Description, iRowCount) Is Nothing Then
                        ListViewHelper.GetListViewSubItem(oListItem, kPolicyColHIndexProductName).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=gPMFunctions.ToSafeString(m_vResultArray(MainModule.ENPolicy.Description, iRowCount)).Trim())
                    End If

                    If Not m_vResultArray(MainModule.ENPolicy.RenewalDate, iRowCount) Is Nothing Then
                        ListViewHelper.GetListViewSubItem(oListItem, kPolicyColHIndexrenewalDate).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=gPMFunctions.ToSafeString(m_vResultArray(MainModule.ENPolicy.RenewalDate, iRowCount)).Trim())
                    End If

                    If Not m_vResultArray(MainModule.ENPolicy.AgentName, iRowCount) Is Nothing Then
                        ListViewHelper.GetListViewSubItem(oListItem, kPolicyColHIndexagent).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=gPMFunctions.ToSafeString(m_vResultArray(MainModule.ENPolicy.AgentName, iRowCount)).Trim())
                    End If

                    If Not m_vResultArray(MainModule.ENPolicy.Premium, iRowCount) Is Nothing Then
                        ListViewHelper.GetListViewSubItem(oListItem, kPolicyColHIndexPremium).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=gPMFunctions.ToSafeString(m_vResultArray(MainModule.ENPolicy.Premium, iRowCount)).Trim())
                    End If

                    If Not m_vResultArray(MainModule.ENPolicy.Status, iRowCount) Is Nothing Then
                        ListViewHelper.GetListViewSubItem(oListItem, kPolicyColHIndexStatus).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=gPMFunctions.ToSafeString(m_vResultArray(MainModule.ENPolicy.Status, iRowCount)).Trim())
                    End If

                    If Not m_vResultArray(MainModule.ENPolicy.EventDesciption, iRowCount) Is Nothing Then
                        ListViewHelper.GetListViewSubItem(oListItem, kPolicyColHIndexEventDescription).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=gPMFunctions.ToSafeString(m_vResultArray(MainModule.ENPolicy.EventDesciption, iRowCount)).Trim())
                    End If

                    If Not m_vResultArray(MainModule.ENPolicy.ActivePlansCount, iRowCount) Is Nothing AndAlso Val(m_vResultArray(MainModule.ENPolicy.ActivePlansCount, iRowCount)) > 0 Then
                        ListViewHelper.GetListViewSubItem(oListItem, kPolicyColHIndexActivePlansCount).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatInteger, vFieldValue:=gPMFunctions.ToSafeInteger(m_vResultArray(MainModule.ENPolicy.ActivePlansCount, iRowCount)))
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, kPolicyColHIndexActivePlansCount).Text = String.Empty
                    End If

                    oListItem.Tag = CStr(gPMFunctions.ToSafeLong(m_vResultArray(MainModule.ENPolicy.InsuranceFileCnt, iRowCount)))

                    If Trim(m_sOptionValue) = "1" AndAlso Val(ListViewHelper.GetListViewSubItem(oListItem, kPolicyColHIndexActivePlansCount).Text) > 0 Then
                        oListItem.Checked = False
                        m_nCountActivePlan = m_nCountActivePlan + 1
                    Else
                        oListItem.Checked = True
                    End If

                End If
            Next iRowCount

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function
   Private Function Validate_Renamed() As Integer

        Dim result As Integer = 0
        Dim iRowCount, iCount As Integer
        Dim nResult As Integer = 0
        Dim nCountActivePlan As Integer = 0
        Const kMethodName As String = "Validate"
        Try
            ''Do All the Validation here.

            result = gPMConstants.PMEReturnCode.PMTrue


            iCount = 0
            m_iChecked = 0

            iRowCount = lvwPolicies.Items.Count


            For iCount = 1 To iRowCount
                If lvwPolicies.Items.Item(iCount - 1).Checked Then
                    If Information.IsArray(m_vSelectedPolicies) Then
                        ReDim Preserve m_vSelectedPolicies(m_iChecked)
                    Else
                        ReDim m_vSelectedPolicies(m_iChecked)
                    End If

                    m_vSelectedPolicies(m_iChecked) = Convert.ToString(lvwPolicies.Items.Item(iCount - 1).Tag)
                    m_iChecked += 1
                End If
                If ToSafeDouble(lvwPolicies.Items(iCount - 1).SubItems(kPolicyColHIndexActivePlansCount).Text) > 0 Then
                    nCountActivePlan = nCountActivePlan + 1
                End If
            Next iCount

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If Val(m_iChecked) = 0 Then

                MessageBox.Show("No Policies selected for transfer", "iSIRClientTransPolicySel")
                nResult = gPMConstants.PMEReturnCode.PMFalse

            ElseIf Val(nCountActivePlan) > 0 AndAlso Val(m_iChecked) < Val(nCountActivePlan) Then
                Dim dlgresult As DialogResult
                dlgresult = MessageBox.Show(nCountActivePlan & " Policies have been unticked and are excluded from the " & Microsoft.VisualBasic.vbCr & _
                                                         "transfer as they have active instalment plans.", "Portfolio transfer", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

                If dlgresult = System.Windows.Forms.DialogResult.Cancel Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process Validate_Renamed ", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=ex)
        End Try

        Return nResult

    End Function

    Private Function TransferPolicies() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "TransferPolicies"

        result = gPMConstants.PMEReturnCode.PMTrue
        Try



            m_lReturn = g_oBusiness.TransferClientPolicies(r_vPolicies:=m_vSelectedPolicies, r_sFromClientCode:=m_sFromClientCode, r_sToClientCode:=m_sToClientCode, v_lFromClientCnt:=m_lFromClientCnt, v_lToClientCnt:=m_lToClientCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, "Transfer Policies Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

        End Try
        Return result


    End Function



    '***************************************************************** '
    ' Name: SetupPolicyDetailsListView
    '
    ' Parameters: N/A
    '
    ' Description:
    '
    ' History:Saurabh Agrawal

    '***************************************************************** '
    Private Function SetupPolicyDetailsListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupPolicyDetailsListView"

        Dim lColWidth As Integer
        Dim sCaption As String = ""
        Const kColumnWidth As Integer = 2129

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lColWidth = CInt((VB6.PixelsToTwipsX(lvwPolicies.Width) - 100) / 10)

            lvwPolicies.Columns.Clear()


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwPolicyNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwPolicies.Columns.Insert(kPolicySelectionColHIndexPolicyNumber, "", sCaption, CInt(VB6.TwipsToPixelsX(kColumnWidth)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwPolicyType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwPolicies.Columns.Insert(kPolicySelectionColHIndexPolicyType, "", sCaption, CInt(VB6.TwipsToPixelsX(kColumnWidth)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwProductName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwPolicies.Columns.Insert(kPolicySelectionColHIndexProductName, "", sCaption, CInt(VB6.TwipsToPixelsX(kColumnWidth)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwRegarding, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwPolicies.Columns.Insert(kPolicySelectionColHIndexRegarding, "", sCaption, CInt(VB6.TwipsToPixelsX(kColumnWidth)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwRenewalDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwPolicies.Columns.Insert(kPolicySelectionColHIndexRenewalDate, "", sCaption, CInt(VB6.TwipsToPixelsX(kColumnWidth)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwPolicies.Columns.Insert(kPolicySelectionColHIndexAgent, "", sCaption, CInt(VB6.TwipsToPixelsX(kColumnWidth)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwPremium, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwPolicies.Columns.Insert(kPolicySelectionColHIndexPremium, "", sCaption, CInt(VB6.TwipsToPixelsX(kColumnWidth)), HorizontalAlignment.Right, -1)



            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwPolicyStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwPolicies.Columns.Insert(kPolicySelectionColHIndexPolicyStatus, "", sCaption, CInt(VB6.TwipsToPixelsX(kColumnWidth)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstRiskTypeDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwPolicies.Columns.Insert(kPolicySelectionColHIndexRiskTypeDescription, "", sCaption, CInt(VB6.TwipsToPixelsX(kColumnWidth)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstEventDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwPolicies.Columns.Insert(kPolicySelectionColHIndexEventDescription, "", sCaption, CInt(VB6.TwipsToPixelsX(kColumnWidth)), HorizontalAlignment.Left, -1)
            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstActivePlansCount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwPolicies.Columns.Insert(kPolicySelectionColHIndexActivePlansCount, "", sCaption, CInt(VB6.TwipsToPixelsX(kColumnWidth)), HorizontalAlignment.Left, -1)

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        If UnloadMode <> vbFormCode Then
            If bCancelTheForm Then

                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            End If
        End If
        eventArgs.Cancel = Cancel <> 0
    End Sub
    Private Sub lvwPolicies_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwPolicies.ItemChecked
        If Not e.Item Is Nothing Then
            If Trim(m_sOptionValue) = "1" AndAlso Val(ListViewHelper.GetListViewSubItem(e.Item, kPolicyColHIndexActivePlansCount).Text) > 0 Then
                e.Item.Checked = False
            End If
        End If
    End Sub
End Class