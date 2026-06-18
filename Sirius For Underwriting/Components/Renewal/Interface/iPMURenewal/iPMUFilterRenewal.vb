Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Public Class frmFilterRenewal
    Inherits System.Windows.Forms.Form

    Private Const ACClass As String = "frmFilterRenewal"

    ' Declare an instance of the FormControl object
    Private m_oFormFields As Object

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' RenewalDate
    Private m_sRenewalDate As String = ""
    ' ProductId
    Private m_lProductId As Integer
    ' Status
    Private m_lStatus As Integer

    'Thinh Nguyen 20/03/2002 (start)
    Private m_lSourceID As Integer
    'Thinh Nguyen 20/03/2002 (end)
    Private m_lAgentId As Integer

    Dim m_oBusiness As Object

    'Thinh Nguyen 20/03/2002 (start)

    Public Property SourceID() As Integer
        Get
            Return m_lSourceID
        End Get
        Set(ByVal Value As Integer)
            m_lSourceID = Value
        End Set
    End Property

    'Thinh Nguyen 20/03/2002 (end)
    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property


    Public Property ProductId() As Integer
        Get
            Return m_lProductId
        End Get
        Set(ByVal Value As Integer)
            m_lProductId = Value
        End Set
    End Property


    Public Property RenewalDate() As String
        Get
            Return m_sRenewalDate
        End Get
        Set(ByVal Value As String)
            m_sRenewalDate = Value
        End Set
    End Property


    Public Property AgentId() As Integer
        Get
            Return m_lAgentId
        End Get
        Set(ByVal Value As Integer)
            m_lAgentId = Value
        End Set
    End Property


    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lProductId = 0
        m_sRenewalDate = ""

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Me.Hide()

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        If cboProduct.ListIndex > -1 Then
            m_lProductId = cboProduct.ItemId
        End If


        'status added to get correct records
        If dtpRenewalDate.Checked = False Or Convert.IsDBNull(dtpRenewalDate.Value) Or IsNothing(dtpRenewalDate.Value) Then
            m_sRenewalDate = ""
        Else

            m_sRenewalDate = dtpRenewalDate.Value
        End If

        'Thinh Nguyen 20/03/2002 (start)
        If cboBranch.SelectedIndex > -1 Then
            'm_lSourceID = cboBranch1.ItemId
            m_lSourceID = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)
        End If
        'Thinh Nguyen 20/03/2002 (end)

        If cboAgentCode.SelectedIndex > -1 Then
            m_lAgentId = VB6.GetItemData(cboAgentCode, cboAgentCode.SelectedIndex)
        Else
            m_lAgentId = 0
        End If


        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        Me.Hide()

    End Sub

    Private Sub Form_Initialize_Renamed()

        iPMFunc.ShowFormInTaskBar_Attach()

    End Sub


    Private Sub frmFilterRenewal_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        iPMFunc.ShowFormInTaskBar_Detach()

        ' Create an instance of the form control object.
        m_oFormFields = CreateLateBoundObject("iPMFormControl.FormFields")

        ' Set language
        m_oFormFields.LanguageID = g_iLanguageID

        m_lReturn = PopulateBranchCbo()
        m_lReturn = PopulateAgentCbo()

        'developer guide no. 38
        Me.cboProduct.FirstItem = "(All)"
        dtpRenewalDate.Value = Today.Date
        dtpRenewalDate.Checked = False
        iPMFunc.CenterForm(Me)

    End Sub

    Private Sub frmFilterRenewal_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        ' Terminate the form control object.
		m_oFormFields.Dispose()

        ' Destroy the instance of the form control object
        ' from memory.
        m_oFormFields = Nothing


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


            ' {* USER DEFINED CODE (Begin) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    ' ***************************************************************** '
    ' Name: PopulateBranchCbo
    '
    ' Parameters: n/a
    '
    ' Description: Populates the branch combo with branches that this
    '               user has access to whether they are closed or not
    '
    ' History:
    '           Created : MEvans : 17-03-2005 : PN19562
    ' ***************************************************************** '
    Public Function PopulateBranchCbo() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateBranchCbo"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vBranchDetails(,) As Object
        Dim llBound, lUBound As Integer

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        ' clear down the contents
        cboBranch.Items.Clear()

        ' get all branches available for this user

        lReturn = g_oRenewal.GetAllUserBranches(r_vResults:=vBranchDetails)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "bSirRenewal.GetAllUserBranches Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' if this user does have access to branches...
        If Information.IsArray(vBranchDetails) Then

            cboBranch.Items.Add("(All)")
            VB6.SetItemData(cboBranch, 0, 0)


            llBound = vBranchDetails.GetLowerBound(1)

            lUBound = vBranchDetails.GetUpperBound(1)

            For lBranch As Integer = llBound To lUBound


                cboBranch.Items.Add(CStr(vBranchDetails(2, lBranch)))

                VB6.SetItemData(cboBranch, cboBranch.Items.Count - 1, CInt(vBranchDetails(0, lBranch)))
                If CInt(vBranchDetails(0, lBranch)) = m_lSourceID Then
                    cboBranch.SelectedIndex = lBranch + 1

                End If
            Next
        Else
            gPMFunctions.RaiseError(kMethodName, "Unable to find branches for user:" & g_oObjectManager.UserID, gPMConstants.PMELogLevel.PMLogError)
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

    ' ***************************************************************** '
    ' Name: PopulateAgentCbo
    '
    ' Parameters: n/a
    '
    ' Description: Populates the Agent combo
    '
    ' History:
    '           Created : Deepak 01 November 2006
    ' ***************************************************************** '
    Public Function PopulateAgentCbo() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateAgentCbo"

        Dim lReturn As Integer
        Dim vAgentArray(,) As Object
        Dim llBound, lUBound, lBranch As Integer
        Try


        result = gPMConstants.PMEReturnCode.PMTrue
        If IsNothing(g_oObjectManager) Then
            g_oObjectManager = New bObjectManager.ObjectManager
        End If
        Dim temp_m_oBusiness As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRenewal.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oBusiness = temp_m_oBusiness

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get an instance of the business object.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If
        If cboBranch.Text = "(All)" Or cboBranch.Text.Trim() = "" Then

            m_lReturn = m_oBusiness.GetAgents(vAgentArray)
        Else

            If cboBranch.SelectedIndex = -1 Then
                cboBranch.SelectedIndex = 0
            End If
            m_lReturn = m_oBusiness.GetAgents(vAgentArray, VB6.GetItemData(cboBranch, cboBranch.SelectedIndex))
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' clear down the contents
        cboAgentCode.Items.Clear()

        Dim cboAgentCode_NewIndex As Integer = -1
        cboAgentCode_NewIndex = cboAgentCode.Items.Add("(All)")
        VB6.SetItemData(cboAgentCode, cboAgentCode_NewIndex, 0)

        If Information.IsArray(vAgentArray) Then

            For iAgentCount As Integer = 0 To vAgentArray.GetUpperBound(1)

                cboAgentCode_NewIndex = cboAgentCode.Items.Add(CStr(vAgentArray(1, iAgentCount)))

                VB6.SetItemData(cboAgentCode, cboAgentCode_NewIndex, CInt(vAgentArray(0, iAgentCount)))
                If CInt(vAgentArray(0, iAgentCount)) = m_lAgentId Then
                    cboAgentCode.SelectedIndex = iAgentCount + 1
                End If
            Next iAgentCount

        End If

        '    cboAgentCode.ListIndex = 0
        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=PopulateBranchCbo(), excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

'        Return result
'        Resume
'        Return result
        End Try
        Return result
    End Function

    Private Sub cboBranch_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranch.SelectedIndexChanged


        m_lReturn = PopulateAgentCbo()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to cboBranch_Click", vApp:=ACApp, vClass:=ACClass, vMethod:="cboBranch_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        End If

    End Sub
End Class
