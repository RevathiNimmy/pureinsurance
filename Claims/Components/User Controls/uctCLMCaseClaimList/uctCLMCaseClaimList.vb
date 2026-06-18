Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctCLMCaseClaim_NET.uctCLMCaseClaim")> _
Partial Public Class uctCLMCaseClaim
    Inherits System.Windows.Forms.UserControl
    Public Event EnabledChange()

    Public Event LinkedOrUnlinked()

    Private Const ACClass As String = "CLMCaseClaimList1"

    Public Event UnRecoverableError(ByVal Sender As Object, ByVal e As EventArgs)

    ' objects
    Private m_oObjectManager As bObjectManager.ObjectManager

    Private m_oBusiness As bCLMCase.Business

    ' generic interface details
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_iLanguageID As Integer
    Private m_iSourceID As Integer
    Private m_iUserId As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_dtEffectiveDate As Date
    Private m_sTransactionType As String = ""

    'Case Claim Details
    Private m_lClaimID As Integer
    Dim m_lOriginalClaimId As Integer
    Private m_lCaseID As Integer
    Private m_lBaseCaseId As Integer
    Private m_dtLossDate As Date
    Private m_sStatus As String = ""
    Private m_lInsuranceFileCnt As Integer
    Private m_lPartyCnt As Integer
    Private m_sCaseProgressStatusCode As String = ""

    ' Navigator process
    Private WithEvents m_oNavStartOpenCLM As iPMNavStart.Interface_Renamed
    Private WithEvents m_oNavStartMainCLM As iPMNavStart.Interface_Renamed
    Private WithEvents m_oNavStartPayCLM As iPMNavStart.Interface_Renamed
    Private WithEvents m_oNavStartSalvage As iPMNavStart.Interface_Renamed
    Private WithEvents m_oNavStartTPRecovery As iPMNavStart.Interface_Renamed

    Private m_bNavCompleted As Boolean
    Private m_bProcessComplete As Boolean

    Private m_bIsOpenClaimCompleted As Boolean
    Private m_bIsMaintainClaimCompleted As Boolean
    Private m_bIsPayClaimCompleted As Boolean
    Private m_bIsSalvageCompleted As Boolean
    Private m_bIsTpRecoveryCompleted As Boolean

    'Other
    Private m_lMinimumWidth As Integer
    Private m_lMinimumHeight As Integer
    Private m_bIsInitialised As Boolean
    Private m_lReturn As Integer
    Private m_sCaseNumber As String = ""

    'Array variables
    Private m_vCaseClaimList(,) As Object
    Private m_vCaseLink As Object
    Private m_vCaseUnlink As Object
    Private m_vClaimDetail(,) As Object
    Private m_vCaseDetails As Object

    'Default Property
    Const m_def_Enabled As Boolean = True

    <Browsable(True)> _
    Public Property MinimumWidth() As Integer
        Get
            Return m_lMinimumWidth
        End Get
        Set(ByVal Value As Integer)
            m_lMinimumWidth = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property MinimumHeight() As Integer
        Get
            Return m_lMinimumHeight
        End Get
        Set(ByVal Value As Integer)
            m_lMinimumHeight = Value
        End Set
    End Property



    <Browsable(True)> _
    Public Property ClaimId() As Integer
        Get
            Return m_lClaimID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimID = Value
        End Set
    End Property

    <Browsable(True)> _
    Public Property CaseID() As Integer
        Get
            Return m_lCaseID
        End Get
        Set(ByVal Value As Integer)
            m_lCaseID = Value
        End Set
    End Property


    <Browsable(True)> _
    Public Property BaseCaseId() As Integer
        Get
            Return m_lBaseCaseId
        End Get
        Set(ByVal Value As Integer)
            m_lBaseCaseId = Value
        End Set
    End Property


    <Browsable(True)> _
    Public Shadows Property Enabled() As Boolean
        Get
            Return MyBase.Enabled
        End Get
        Set(ByVal Value As Boolean)
            MyBase.Enabled = Value
            RaiseEvent EnabledChange()
        End Set
    End Property
    ''62125
    <Browsable(False)> _
    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    <Browsable(False)> _
    Public Property CaseNumber() As String
        Get
            Return m_sCaseNumber
        End Get
        Set(ByVal Value As String)
            m_sCaseNumber = Value
        End Set
    End Property
    <Browsable(True)> _
    Public Property CaseProgressStatusCode() As String
        Get
            Return m_sCaseProgressStatusCode
        End Get
        Set(ByVal Value As String)
            m_sCaseProgressStatusCode = Value
        End Set
    End Property

    Private Sub cmdLink_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdLink.Click
        LinkClaimToCase()
        RaiseEvent LinkedOrUnlinked()
    End Sub

    Private Sub cmdMaintain_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMaintain.Click

        MaintainClaim()

        'Get Case Claim links
        Dim lReturn As Integer = GetCaseClaimLink()
        lReturn = PopulateCaseClaimList()
        lReturn = UpdatedCaseClaimList()
        Me.ParentForm.Focus()
    End Sub

    Private Sub cmdOpen_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOpen.Click

        OpenClaim()
        'Get Case Claim links
        Dim lReturn As Integer = GetCaseClaimLink()
        lReturn = PopulateCaseClaimList()
        lReturn = UpdatedCaseClaimList()
        Me.ParentForm.Focus()
    End Sub

    Private Sub cmdPay_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPay.Click

        PayClaim()
        'Get Case Claim links
        Dim lReturn As Integer = GetCaseClaimLink()
        lReturn = PopulateCaseClaimList()
        lReturn = UpdatedCaseClaimList()
        Me.ParentForm.Focus()
    End Sub

    Private Sub cmdSalvage_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSalvage.Click

        OpenSalvage()
        'Get Case Claim links
        Dim lReturn As Integer = GetCaseClaimLink()
        lReturn = PopulateCaseClaimList()
        lReturn = UpdatedCaseClaimList()
        Me.ParentForm.Focus()
    End Sub

    Private Sub cmdTPRecovery_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTPRecovery.Click

        OpenThirdPartyRecovery()
        'Get Case Claim links
        Dim lReturn As Integer = GetCaseClaimLink()
        lReturn = PopulateCaseClaimList()
        lReturn = UpdatedCaseClaimList()
        Me.ParentForm.Focus()
    End Sub

    Private Sub cmdUnlink_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdUnlink.Click
        UnLinkClaimFromCase()
        RaiseEvent LinkedOrUnlinked()
    End Sub

    Private Sub m_oNavStartMainCLM_NavigatorClose() Handles m_oNavStartMainCLM.NavigatorClose
        m_bIsMaintainClaimCompleted = True
        cmdMaintain.Enabled = True
    End Sub

    Private Sub m_oNavStartOpenCLM_NavigatorClose() Handles m_oNavStartOpenCLM.NavigatorClose
        m_bIsOpenClaimCompleted = True
        cmdOpen.Enabled = True
    End Sub

    Private Sub m_oNavStartPayCLM_NavigatorClose() Handles m_oNavStartPayCLM.NavigatorClose
        m_bIsPayClaimCompleted = True
        cmdPay.Enabled = True
    End Sub

    Private Sub m_oNavStartSalvage_NavigatorClose() Handles m_oNavStartSalvage.NavigatorClose
        m_bIsSalvageCompleted = True
        cmdSalvage.Enabled = True
    End Sub

    Private Sub m_oNavStartTPRecovery_NavigatorClose() Handles m_oNavStartTPRecovery.NavigatorClose
        m_bIsTpRecoveryCompleted = True
        cmdTPRecovery.Enabled = True
    End Sub

    Private Sub UserControl_Initialize()
        MinimumWidth = 8250
        MinimumHeight = 1995
        SetupListView()
    End Sub

    'developer guide no. 1 No Solution
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)
        MyBase.Enabled = CBool(PropBag.ReadProperty("Enabled", m_def_Enabled))
    End Sub

    Private Sub uctCLMCaseClaim_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        Const iButtonMinHeight As Integer = 315


        If VB6.PixelsToTwipsY(MyBase.Height) < m_lMinimumHeight Then
            MyBase.Height = VB6.TwipsToPixelsY(m_lMinimumHeight)
        End If

        If VB6.PixelsToTwipsX(MyBase.Width) < MinimumWidth Then
            MyBase.Width = VB6.TwipsToPixelsX(MinimumWidth)
        End If

        '  fraCaseClaimDetails.Height = UserControl.Height - 50
        '  fraCaseClaimDetails.Width = UserControl.Width - 50

        lvwCaseClaimList.Height = MyBase.Height - VB6.TwipsToPixelsY(500) '(fraCaseClaimDetails.Height - iButtonMinHeight) - 480
        lvwCaseClaimList.Width = MyBase.Width 'fraCaseClaimDetails.Width - lvwCaseClaimList.Left - 120

        lvwCaseClaimList.Top = 0
        lvwCaseClaimList.Left = 0

        cmdOpen.Left = 0
        cmdOpen.Top = VB6.TwipsToPixelsY((VB6.PixelsToTwipsY(lvwCaseClaimList.Top) + VB6.PixelsToTwipsY(lvwCaseClaimList.Height)) + 100)

        cmdMaintain.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdOpen.Left) + VB6.PixelsToTwipsX(cmdOpen.Width) + 60)
        cmdMaintain.Top = cmdOpen.Top

        cmdPay.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdMaintain.Left) + VB6.PixelsToTwipsX(cmdMaintain.Width) + 60)
        cmdPay.Top = cmdOpen.Top

        cmdSalvage.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdPay.Left) + VB6.PixelsToTwipsX(cmdPay.Width) + 60)
        cmdSalvage.Top = cmdOpen.Top

        cmdTPRecovery.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdSalvage.Left) + VB6.PixelsToTwipsX(cmdSalvage.Width) + 60)
        cmdTPRecovery.Top = cmdOpen.Top

        cmdUnlink.Left = lvwCaseClaimList.Left + lvwCaseClaimList.Width - cmdUnlink.Width 'fraCaseClaimDetails.Width - 120
        cmdUnlink.Top = cmdOpen.Top

        cmdLink.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdUnlink.Left) - VB6.PixelsToTwipsX(cmdLink.Width) - 60)
        cmdLink.Top = cmdOpen.Top

        m_lMinimumWidth = 8250
        m_lMinimumHeight = 1995

        Dim sngWidth As Single = (VB6.PixelsToTwipsX(lvwCaseClaimList.Width) - 100) / 6
        For iCol As Integer = 1 To lvwCaseClaimList.Columns.Count - 1
            If iCol <> 1 Then
                lvwCaseClaimList.Columns.Item(iCol - 1).Width = CInt(VB6.TwipsToPixelsX(sngWidth))
            End If
        Next iCol

    End Sub


    ' ***************************************************************** '
    ' Name: Load
    ' Parameters: n/a
    ' Description:
    ' History:
    ' Created: VB : 06-07-2007
    ' ***************************************************************** '
    Public Function Load_Renamed() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Load"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' set up taxes list view
            lReturn = SetupListView()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupListView Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get Case Claim links
            lReturn = GetCaseClaimLink()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetCaseClaimLink Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = PopulateCaseClaimList()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateCaseClaimList Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = GetCaseDetails()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetCaseDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            SetInterfaceforCloseCase()


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

    '***************************************************************** '
    ' Name: SetupListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : VB : Date :
    '***************************************************************** '
    Private Function SetupListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupListView"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lColWidth As Integer
        Dim sCaption As String = ""


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lColWidth = CInt((VB6.PixelsToTwipsX(lvwCaseClaimList.Width) - 100) / 8)

            lvwCaseClaimList.Columns.Clear()

            'developer guide no.     
            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwClaimId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwCaseClaimList.Columns.Insert(KCaseColHIndexClaimid - 1, kTaxDetColHCodeClaimid, sCaption, CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left, -1)
            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwClaimNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwCaseClaimList.Columns.Insert(kCaseColHIndexClaimNumber - 1, kTaxDetColHCodeClaimNumber, sCaption, CInt(VB6.TwipsToPixelsX(lColWidth)), HorizontalAlignment.Left, -1)
            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwLossDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwCaseClaimList.Columns.Insert(kCaseColHIndexLossDate - 1, kTaxDetColHCodeLossDate, sCaption, CInt(VB6.TwipsToPixelsX(lColWidth)), HorizontalAlignment.Left, -1)
            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwClaimHandler, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwCaseClaimList.Columns.Insert(kCaseColHIndexClaimHandler - 1, kTaxDetColHCodeClaimHandler, sCaption, CInt(VB6.TwipsToPixelsX(lColWidth)), HorizontalAlignment.Center, -1)
            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwRiskType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwCaseClaimList.Columns.Insert(kCaseColHIndexRiskType - 1, kTaxDetColHCodeRiskType, sCaption, CInt(VB6.TwipsToPixelsX(lColWidth)), HorizontalAlignment.Center, -1)
            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwCaseClaimList.Columns.Insert(kCaseColHIndexStatus - 1, kTaxDetColHCodeStatus, sCaption, CInt(VB6.TwipsToPixelsX(lColWidth)), HorizontalAlignment.Center, -1)
            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwTotalIndemnity, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwCaseClaimList.Columns.Insert(kCaseColHIndexTotalIndemnity - 1, kTaxDetColHCodeTotalIndemnity, sCaption, CInt(VB6.TwipsToPixelsX(lColWidth)), HorizontalAlignment.Right, -1)
            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwTotalExpense, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwCaseClaimList.Columns.Insert(kCaseColHIndexTotalExpense - 1, kTaxDetColHCodeTotalExpense, sCaption, CInt(VB6.TwipsToPixelsX(lColWidth)), HorizontalAlignment.Right, -1)
            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwTotalExcess, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwCaseClaimList.Columns.Insert(kCaseColHIndexTotalExcess - 1, kTaxDetColHCodeTotalExcess, sCaption, CInt(VB6.TwipsToPixelsX(lColWidth)), HorizontalAlignment.Right, -1)
            lvwCaseClaimList.Columns.Insert(kCaseColHIndexInsuranceFileCnt - 1, kTaxDetColHCodeInsuranceFileCnt, "Policy ID", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left, -1)


            lvwCaseClaimList.LabelEdit = False

            ' add the grid lines and full row select for the Reserve List view
            'lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwCaseClaimList.Handle.ToInt32(), v_vShowGridLines:=False, v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
            lvwCaseClaimlist.FullRowSelect = True
            lvwCaseClaimlist.GridLines = False



            'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	gPMFunctions.RaiseError(kMethodName, "SetExtraListViewProperties  Failed", gPMConstants.PMELogLevel.PMLogError)
            'End If

            DisableInterface(bEnabled:=False, bAllControl:=False)



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: SetProcessModes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : VB : 06-07-2007 :
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "SetProcessModes"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Information.IsNothing(vTask) Then

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
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
    ' Name: DisableInterface
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : VB : 06-07-2007 :
    ' ***************************************************************** '
    Private Function DisableInterface(ByVal bEnabled As Boolean, Optional ByVal bAllControl As Boolean = True) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DisableInterface"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If bAllControl Then
                cmdOpen.Enabled = bEnabled
                cmdLink.Enabled = bEnabled
            End If

            cmdMaintain.Enabled = bEnabled
            cmdPay.Enabled = bEnabled
            cmdSalvage.Enabled = bEnabled
            cmdTPRecovery.Enabled = bEnabled
            cmdUnlink.Enabled = bEnabled



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : VB : 06-07-2007 :
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Initialise"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if already initialised
            If m_bIsInitialised Then
                Return result
            End If

            'Set m_colPaymentItems = New Collection

            ' Create an instance of the object manager.
            m_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            lReturn = m_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "g_oOBjectManager.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' If UserID is 0 assume that user cancelled logon
            If m_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Store the language ID from the object manager to the public variables,
            ' to enable us to use them throughout the object.
            With m_oObjectManager
                m_iLanguageID = .LanguageID
                m_iSourceID = .SourceID
                m_iUserId = .UserID
            End With

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via the public object manager.
            Dim temp_m_oBusiness As Object
            lReturn = m_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMCase.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bClMCase.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' hold Initialised status
            m_bIsInitialised = True


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: Save
    '
    ' Parameters: n/a
    '
    ' Description: saves case details
    '
    ' History:
    '           Created : VB : 06-07-2007 :
    ' ***************************************************************** '
    Public Function Save() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Save"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bTransStarted As Boolean
        Dim lDocumentTemplateID As Integer
        Dim sDocumentTemplateID As String = ""
        Dim lDocumentTypeID As Integer
        Dim sMsg As String = ""
        Dim lSpoolMode As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' start transaction

            lReturn = m_oBusiness.BeginTrans
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Begin Transaction Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            bTransStarted = True

            If Information.IsArray(m_vCaseLink) And m_lBaseCaseId > 0 Then

                lReturn = m_oBusiness.LinkClaims(v_lBaseCaseID:=m_lBaseCaseId, v_vLinkArray:=m_vCaseLink)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "m_oBusiness.LinkClaim Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            If Information.IsArray(m_vCaseUnlink) Then

                lReturn = m_oBusiness.UnlinkClaims(v_vUnlinkArray:=m_vCaseUnlink)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "m_oBusiness.UnlinkClaim Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            ' commit transaction

            lReturn = m_oBusiness.CommitTrans
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Commit Transaction Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            Select Case m_iTask
                Case gPMConstants.PMEComponentAction.PMAdd

                    iPMFunc.GetSystemOption(5032, sDocumentTemplateID)

                    lDocumentTemplateID = gPMFunctions.ToSafeLong(CInt(sDocumentTemplateID), 0)


                    sMsg = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstOpenCaseDocument, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                Case gPMConstants.PMEComponentAction.PMEdit

                    iPMFunc.GetSystemOption(5033, sDocumentTemplateID)

                    lDocumentTemplateID = gPMFunctions.ToSafeLong(CInt(sDocumentTemplateID), 0)


                    sMsg = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstEditCaseDocument, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            End Select

            If lDocumentTemplateID <> 0 Then


                If MessageBox.Show(sMsg, "Case", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then

                    If Information.IsArray(m_vCaseLink) Then
                        m_lClaimID = gPMFunctions.ToSafeInteger(CInt(m_vCaseLink(0)))
                    End If

                    '            If MsgBox("Do you wish to spool this document?", vbYesNo + vbQuestion, "Case") = vbYes Then
                    '                lSpoolMode = ACSpoolDocMode
                    '            Else
                    '                lSpoolMode = ACPrintSilentMode
                    '            End If
                    lReturn = CType(GetTemplateType(lDocumentTemplateID:=lDocumentTemplateID, r_lDocumentTypeID:=lDocumentTypeID), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetTemplateType Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    '            lReturn = PrintDocument(v_lDocumentTemplateID:=lDocumentTemplateID, _
                    ''                                    v_lDocumentTypeID:=lDocumentTypeID, _
                    ''                                    v_lSpoolMode:=lSpoolMode)
                    '
                    '            If lReturn <> PMTrue Then
                    '                RaiseError kMethodName, "PrintDocument Failed", PMLogError
                    '            End If
                    lReturn = CType(UseTheTemplate(v_lDocId:=lDocumentTemplateID, v_lDocTypeId:=lDocumentTypeID), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "UseTheTemplate Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' if a transaction was started and an error occurred then
            ' rollback all updates...
            If bTransStarted Then

                m_oBusiness.RollbackTrans()
            End If

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: OpenClaim
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' Created :
    ' ***************************************************************** '
    Private Function OpenClaim() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "OpenClaim"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vKeyArray(,) As Object
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset mouse
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check if already initialised
            If Not m_bIsInitialised Then
                Return result
            End If

            ' Get an instance of m_oNavStartOpenCLM
            m_oNavStartOpenCLM = New iPMNavStart.Interface_Renamed()

            ' Initialise it
            'developer guide no.9
            lReturn = m_oNavStartOpenCLM.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to initialise m_oNavStartOpenClM", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set its properties
            m_oNavStartOpenCLM.CallingAppName = ACApp

            ' Set the process to start
            m_oNavStartOpenCLM.ProcessCode = kRoadMapConstantOpenClaim

            ReDim vKeyArray(1, 1)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "base_case_id"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lBaseCaseId

            'The XML roadmap to use
            m_oNavStartOpenCLM.NavXMLFile = m_oNavStartOpenCLM.ProcessCode & ".XML"

            'Setkeys
            lReturn = m_oNavStartOpenCLM.SetKeys(vKeyArray:=vKeyArray)

            m_bIsOpenClaimCompleted = False

            cmdOpen.Enabled = False

            ' Start it
            lReturn = m_oNavStartOpenCLM.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to start m_oNavStartOpenClM", gPMConstants.PMELogLevel.PMLogError)
            End If



            Do
                Application.DoEvents()
            Loop While Not m_bIsOpenClaimCompleted

            '   Setkeys
            lReturn = m_oNavStartOpenCLM.GetKeys(vKeyArray:=vKeyArray)

            If Information.IsArray(vKeyArray) Then

                For iLoop As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                    'developer guide no.248
                    If Convert.ToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop)).Trim() = "claim_cnt" Then

                        m_lReturn = LinkClaimToCase(v_lClaimID:=gPMFunctions.ToSafeLong(CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))), v_bIsMessageSilent:=True)
                    End If
                Next iLoop
            End If

            ' Terminate Navigator
            m_oNavStartOpenCLM.Dispose()

           



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally
            m_oNavStartOpenCLM = Nothing



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: MaintainClaim
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' Created :
    ' ***************************************************************** '
    Private Function MaintainClaim() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "MaintainClaim"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vKeyArray(,) As Object
        Dim lCopyClaimId As Integer
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset mouse
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If Not m_bIsInitialised Then
                Return result
            End If

            lReturn = CType(CopyClaim(v_lClaimID:=m_lClaimID, r_lCopyClaimId:=lCopyClaimId, v_sTransactionType:="C_CR"), gPMConstants.PMEReturnCode)
            If lReturn = gPMConstants.PMEReturnCode.PMRecordInUse Then
                Return result
            ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to copy Claim", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lClaimID = lCopyClaimId

            ' Get an instance of m_oNavStartMainCLM
            m_oNavStartMainCLM = New iPMNavStart.Interface_Renamed()

            ' Initialise it
            'developer guide no.9
            lReturn = m_oNavStartMainCLM.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to initialise m_oNavStartMainCLM", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set its properties
            m_oNavStartMainCLM.CallingAppName = ACApp

            ' Set the process to start
            m_oNavStartMainCLM.ProcessCode = kRoadMapConstantMaintainClaim

            ReDim vKeyArray(1, 4)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "claim_cnt"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = "restart_step"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = 1

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "insurancefile_cnt"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "claim_mode"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = 2 'EditMode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = "claim_id"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lOriginalClaimId

            'The XML roadmap to use
            m_oNavStartMainCLM.NavXMLFile = m_oNavStartMainCLM.ProcessCode & ".XML"

            'Setkeys
            lReturn = m_oNavStartMainCLM.SetKeys(vKeyArray:=vKeyArray)

            m_bIsMaintainClaimCompleted = False

            ' Start it
            lReturn = m_oNavStartMainCLM.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to start m_oNavStartMainCLM", gPMConstants.PMELogLevel.PMLogError)
            End If
            cmdMaintain.Enabled = False

            Do
                Application.DoEvents()
            Loop While Not m_bIsMaintainClaimCompleted

            ' Terminate Navigator
            m_oNavStartMainCLM.Dispose()

           



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            m_oNavStartMainCLM = Nothing



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PayClaim
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' Created :
    ' ***************************************************************** '
    Private Function PayClaim() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PayClaim"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vKeyArray(,) As Object
        Dim lCopyClaimId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset mouse
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If Not m_bIsInitialised Then
                Return result
            End If

            lReturn = CType(CopyClaim(v_lClaimID:=m_lClaimID, r_lCopyClaimId:=lCopyClaimId, v_sTransactionType:="C_CP"), gPMConstants.PMEReturnCode)

            If lReturn = gPMConstants.PMEReturnCode.PMRecordInUse Then
                Return result
            ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to copy Claim", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lClaimID = lCopyClaimId

            ' Get an instance of m_oNavStartPayCLM
            m_oNavStartPayCLM = New iPMNavStart.Interface_Renamed()

            ' Initialise it
            'developer guide no.9
            lReturn = m_oNavStartPayCLM.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to initialise m_oNavStartPayCLM", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set its properties
            m_oNavStartPayCLM.CallingAppName = ACApp

            ' Set the process to start
            m_oNavStartPayCLM.ProcessCode = kRoadMapConstantPayClaim

            ReDim vKeyArray(1, 3)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "claim_cnt"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = "restart_step"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = 1

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "insurancefile_cnt"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "claim_mode"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = 2 'EditMode

            'The XML roadmap to use
            m_oNavStartPayCLM.NavXMLFile = m_oNavStartPayCLM.ProcessCode & ".XML"

            'Setkeys
            lReturn = m_oNavStartPayCLM.SetKeys(vKeyArray:=vKeyArray)

            m_bIsPayClaimCompleted = False

            ' Start it
            lReturn = m_oNavStartPayCLM.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to start m_oNavStartPayCLM", gPMConstants.PMELogLevel.PMLogError)
            End If
            cmdPay.Enabled = False
            Do
                Application.DoEvents()
            Loop While Not m_bIsPayClaimCompleted

            ' Terminate Navigator
            m_oNavStartPayCLM.Dispose()

           



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            m_oNavStartPayCLM = Nothing



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: OpenSalvage
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' Created :
    ' ***************************************************************** '
    Private Function OpenSalvage() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "OpenSalvage"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vKeyArray(,) As Object
        Dim lCopyClaimId As Integer
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset mouse
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If Not m_bIsInitialised Then
                Return result
            End If

            lReturn = CType(CopyClaim(v_lClaimID:=m_lClaimID, r_lCopyClaimId:=lCopyClaimId, v_sTransactionType:="C_SA"), gPMConstants.PMEReturnCode)

            If lReturn = gPMConstants.PMEReturnCode.PMRecordInUse Then
                Return result
            ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to copy Claim", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lClaimID = lCopyClaimId

            ' Get an instance of m_oNavStartSalvage
            m_oNavStartSalvage = New iPMNavStart.Interface_Renamed()

            ' Initialise it
            'developer guide no.9
            lReturn = m_oNavStartSalvage.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to initialise m_oNavStartSalvage", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set its properties
            m_oNavStartSalvage.CallingAppName = ACApp

            ' Set the process to start
            m_oNavStartSalvage.ProcessCode = kRoadMapConstantSalvage

            ReDim vKeyArray(1, 3)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "claim_cnt"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = "restart_step"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = 1

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "insurancefile_cnt"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "claim_mode"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = 2 'EditMode


            'The XML roadmap to use
            m_oNavStartSalvage.NavXMLFile = m_oNavStartSalvage.ProcessCode & ".XML"

            'Setkeys
            lReturn = m_oNavStartSalvage.SetKeys(vKeyArray:=vKeyArray)

            m_bIsSalvageCompleted = False

            ' Start it
            lReturn = m_oNavStartSalvage.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to start m_oNavStartSalvage", gPMConstants.PMELogLevel.PMLogError)
            End If

            cmdSalvage.Enabled = False

            Do
                Application.DoEvents()
            Loop While Not m_bIsSalvageCompleted

            ' Terminate Navigator
            m_oNavStartSalvage.Dispose()

          



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            m_oNavStartSalvage = Nothing



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: OpenThirdPartyRecovery
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' Created :
    ' ***************************************************************** '
    Private Function OpenThirdPartyRecovery() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "OpenThirdPartyRecovery"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vKeyArray(,) As Object
        Dim lCopyClaimId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset mouse
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If Not m_bIsInitialised Then
                Return result
            End If

            lReturn = CType(CopyClaim(v_lClaimID:=m_lClaimID, r_lCopyClaimId:=lCopyClaimId, v_sTransactionType:="C_RV"), gPMConstants.PMEReturnCode)

            If lReturn = gPMConstants.PMEReturnCode.PMRecordInUse Then
                Return result
            ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to copy Claim", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lClaimID = lCopyClaimId

            ' Get an instance of m_oNavStartTPRecovery
            m_oNavStartTPRecovery = New iPMNavStart.Interface_Renamed()

            ' Initialise it
            'developer guide no.9
            lReturn = m_oNavStartTPRecovery.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to initialise m_oNavStartTPRecovery", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set its properties
            m_oNavStartTPRecovery.CallingAppName = ACApp

            ' Set the process to start
            m_oNavStartTPRecovery.ProcessCode = kRoadMapConstantTPRecovery

            ReDim vKeyArray(1, 3)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "claim_cnt"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = "restart_step"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = 1

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "insurancefile_cnt"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "claim_mode"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = 2 'EditMode

            'The XML roadmap to use
            m_oNavStartTPRecovery.NavXMLFile = m_oNavStartTPRecovery.ProcessCode & ".XML"

            'Setkeys
            lReturn = m_oNavStartTPRecovery.SetKeys(vKeyArray:=vKeyArray)

            m_bIsTpRecoveryCompleted = False

            ' Start it
            lReturn = m_oNavStartTPRecovery.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to start m_oNavStartTPRecovery", gPMConstants.PMELogLevel.PMLogError)
            End If

            cmdTPRecovery.Enabled = False

            Do
                Application.DoEvents()
            Loop While Not m_bIsTpRecoveryCompleted

            ' Terminate Navigator
            m_oNavStartTPRecovery.Dispose()

           



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            m_oNavStartTPRecovery = Nothing



        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: LinkClaimToCase
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Private Function LinkClaimToCase(Optional ByVal v_lClaimID As Integer = 0, Optional ByVal v_bIsMessageSilent As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LinkClaimToCase"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vTempArr As Object
        Dim lClaimId As Integer
        Dim oListItem As ListViewItem
        Dim sCaseNumber As String = ""
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If Not m_bIsInitialised Then
                Return result
            End If
            If v_lClaimID = 0 Then
                lReturn = CType(GetClaim(r_lClaimID:=lClaimId), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                lClaimId = gPMFunctions.ToSafeLong(v_lClaimID)
            End If

            If gPMFunctions.ToSafeLong(lClaimId) <> 0 Then

                lReturn = CType(GetClaimDetail(v_lClaimID:=lClaimId), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If Information.IsArray(m_vClaimDetail) Then

                    If Information.IsArray(m_vCaseLink) Then
                        For Each m_vCaseLink_item As Object In m_vCaseLink
                            'developer guide no.248
                            If gPMFunctions.ToSafeLong(m_vCaseLink_item) = lClaimId Then
                                Return result
                            End If
                        Next m_vCaseLink_item
                    End If

                    If Not v_bIsMessageSilent Then
                        If Information.IsArray(m_vClaimDetail) Then
                            'developer guide no.248
                            If MessageBox.Show("You have chosen to link Claim Number: " & gPMFunctions.ToSafeString(m_vClaimDetail(kClaimDetailClaimNumber, 0)).Trim() & Strings.Chr(13) & Strings.Chr(10) & _
                                               "to case number: " & m_sCaseNumber.Trim() & _
                                               ". Is this correct?", "Case", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                Return result
                            End If
                        End If
                        'developer guide no.248
                        If gPMFunctions.ToSafeLong(m_vClaimDetail(kClaimDetailBaseCaseID, 0)) > 0 Then
                            'developer guide no.248
                            MessageBox.Show("The claim you have selected is already linked to Case Number: " & _
                                            gPMFunctions.ToSafeString(m_vClaimDetail(kClaimDetailCaseNumber, 0)).Trim() & "." & Strings.Chr(13) & Strings.Chr(10) & _
                                            "A Claim can only be linked to one Case.", "Case", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return result
                        Else
                            If Not Information.IsArray(m_vClaimDetail) Then
                                'developer guide no.248
                                MessageBox.Show("No Case is associated with the selected Claim '" & gPMFunctions.ToSafeString(m_vClaimDetail(kClaimDetailClaimNumber, 0)).Trim() & "'.", "Case", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                        End If
                    End If

                    If Information.IsArray(m_vCaseLink) Then
                        ReDim Preserve m_vCaseLink(m_vCaseLink.GetUpperBound(0) + 1)
                    Else
                        ReDim m_vCaseLink(0)
                    End If

                    If Information.IsArray(m_vCaseUnlink) Then
                        For Each m_vCaseUnlink_item As Object In m_vCaseUnlink
                            'developer guide no.248
                            If gPMFunctions.ToSafeLong(m_vCaseUnlink_item) <> lClaimId Then
                                If Information.IsArray(vTempArr) Then

                                    ReDim Preserve vTempArr(vTempArr.GetUpperBound(0) + 1)
                                Else
                                    ReDim vTempArr(0)
                                End If


                                'developer guide no.248
                                vTempArr(vTempArr.GetUpperBound(0)) = gPMFunctions.ToSafeLong(m_vCaseUnlink_item)
                            End If
                        Next m_vCaseUnlink_item

                        m_vCaseUnlink = vTempArr
                    End If

                    m_vCaseLink(m_vCaseLink.GetUpperBound(0)) = gPMFunctions.ToSafeLong(lClaimId)

                    oListItem = lvwCaseClaimlist.Items.Add(CStr(lClaimId))
                    Dim lReturnError As Integer = 0


                    ' populate list sub item details
                    'developer guide no.248
                    ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsClaimNumber).Text = gPMFunctions.ToSafeString(m_vClaimDetail(kClaimDetailClaimNumber, 0))
                    ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsLossdate).Text = DateTime.Parse(gPMFunctions.ToSafeDate(m_vClaimDetail(kClaimDetailLossDate, 0))).ToString("D")
                    ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsStatus).Text = gPMFunctions.ToSafeString(m_vClaimDetail(kClaimDetailStatus, 0))
                    ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalIndemnity).Text = CStr(gPMFunctions.ToSafeCurrency(m_vClaimDetail(kClaimDetailTotalIndemnity, 0)))
                    ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalExpense).Text = CStr(gPMFunctions.ToSafeCurrency(m_vClaimDetail(kClaimDetailTotalExpense, 0)))
                    ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalExcess).Text = CStr(gPMFunctions.ToSafeCurrency(m_vClaimDetail(kClaimDetailTotalExcess, 0)))
                    ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsInsuranceFileCnt).Text = CStr(gPMFunctions.ToSafeLong(m_vClaimDetail(kClaimDetailInsuranceFileCnt, 0)))
                    oListItem.Tag = CStr(lClaimId)

                    If Not Information.IsArray(m_vCaseLink) Then
                        m_oBusiness.GenerateCaseCode(r_sCaseCode:=CaseNumber, v_iclaimid:=lClaimId, lReturnError)
                    ElseIf m_vCaseLink.Length = 1 AndAlso CaseNumber IsNot Nothing AndAlso CaseNumber.Contains("NA") Then
                        m_oBusiness.GenerateCaseCode(r_sCaseCode:=CaseNumber, v_iclaimid:=lClaimId, lReturnError)
                    End If
                    MessageBox.Show("Claim linked successfully", "Case", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: UnLinkClaimFromCase
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Private Function UnLinkClaimFromCase() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UnLinkClaimFromCase"

        Dim lReturn, lClaimId As Integer
        Dim vTempArr As Object
        Dim sMsg As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If Not m_bIsInitialised Then
                Return result
            End If

            'sMsg = iPMFunc.GetResData( _
            'iLangID:=m_iLanguageID%, _
            'lId:=kRegKeyConstUnLinkMsg, _
            'iDataType:=PMResString)
            'developer guide no.248
            If IsNothing(lvwCaseClaimlist.FocusedItem) Then
                sMsg = "You have chosen to remove the link between claim number: " & gPMFunctions.ToSafeString(ListViewHelper.GetListViewSubItem(lvwCaseClaimlist.Items.Item(lvwCaseClaimlist.Items(0).Index), 1).Text) & Strings.Chr(13) & Strings.Chr(10) & _
                 "and case number: " & m_sCaseNumber.Trim() & ". Is this correct?"
            Else
                sMsg = "You have chosen to remove the link between claim number: " & gPMFunctions.ToSafeString(ListViewHelper.GetListViewSubItem(lvwCaseClaimlist.Items.Item(lvwCaseClaimlist.FocusedItem.Index), 1).Text) & Strings.Chr(13) & Strings.Chr(10) & _
                "and case number: " & m_sCaseNumber.Trim() & ". Is this correct?"
            End If
            If MessageBox.Show(sMsg, "Case Claim list", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Cancel Then
                Return result
            End If

            If IsNothing(lvwCaseClaimlist.FocusedItem) Then
                lClaimId = Convert.ToString(lvwCaseClaimlist.Items.Item(lvwCaseClaimlist.Items(0).Index).Tag)
            Else
                lClaimId = Convert.ToString(lvwCaseClaimlist.Items.Item(lvwCaseClaimlist.FocusedItem.Index).Tag)
            End If
            If gPMFunctions.ToSafeLong(lClaimId) <> 0 Then
                If Information.IsArray(m_vCaseUnlink) Then
                    For Each m_vCaseUnlink_item As Object In m_vCaseUnlink
                        'developer guide no.248
                        If gPMFunctions.ToSafeLong(m_vCaseUnlink_item) = lClaimId Then
                            Return result
                        End If
                    Next m_vCaseUnlink_item
                    ReDim Preserve m_vCaseUnlink(m_vCaseUnlink.GetUpperBound(0) + 1)
                Else
                    ReDim m_vCaseUnlink(0)
                End If

                If Information.IsArray(m_vCaseLink) Then
                    For Each m_vCaseLink_item As Object In m_vCaseLink
                        'developer guide no.248
                        If gPMFunctions.ToSafeLong(m_vCaseLink_item) <> lClaimId Then
                            If Information.IsArray(vTempArr) Then

                                ReDim Preserve vTempArr(vTempArr.GetUpperBound(0) + 1)
                            Else
                                ReDim vTempArr(0)
                            End If


                            'developer guide no.248
                            vTempArr(vTempArr.GetUpperBound(0)) = gPMFunctions.ToSafeLong(m_vCaseLink_item)
                        End If
                    Next m_vCaseLink_item

                    m_vCaseLink = vTempArr
                End If

                m_vCaseUnlink(m_vCaseUnlink.GetUpperBound(0)) = gPMFunctions.ToSafeLong(lClaimId)

                For lIndex As Integer = 1 To lvwCaseClaimlist.Items.Count
                    'developer guide no.248
                    If gPMFunctions.ToSafeLong(lvwCaseClaimlist.Items.Item(lIndex - 1).Tag) = lClaimId Then
                        RemoveHandler lvwCaseClaimlist.ItemSelectionChanged, AddressOf lvwCaseClaimlist_ItemSelectionChanged
                        lvwCaseClaimlist.Items.RemoveAt(lIndex - 1)
                        AddHandler lvwCaseClaimlist.ItemSelectionChanged, AddressOf lvwCaseClaimlist_ItemSelectionChanged
                    End If
                    'Put the following check outside the if condition.
                    If lvwCaseClaimlist.Items.Count <= lIndex Then
                        Exit For
                    End If
                Next lIndex
            End If

            If lvwCaseClaimlist.Items.Count < 1 Then
                DisableInterface(bEnabled:=False, bAllControl:=False)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: PopulateCaseClaimList
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Private Function PopulateCaseClaimList() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateCaseClaimList"

        Dim lReturn As Integer
        Dim dtDate As Date
        Dim lClaimId, llBound, lUBound As Integer
        Dim bFound As Boolean
        Dim oListItem As ListViewItem


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lvwCaseClaimList.Items.Clear()

            If Information.IsArray(m_vCaseClaimList) Then

                llBound = m_vCaseClaimList.GetLowerBound(1)
                lUBound = m_vCaseClaimList.GetUpperBound(1)

                For lItem As Integer = llBound To lUBound

                    lClaimId = gPMFunctions.ToSafeLong(CInt(m_vCaseClaimList(kCaseClaimListClaimId, lItem)))

                    bFound = False
                    If Information.IsArray(m_vCaseUnlink) Then
                        For Each m_vCaseUnlink_item As Object In m_vCaseUnlink
                            If gPMFunctions.ToSafeLong(CInt(m_vCaseUnlink_item)) = lClaimId Then
                                bFound = True
                                Exit For
                            End If
                        Next m_vCaseUnlink_item
                    End If

                    If Not bFound Then

                        ' add list item
                        oListItem = lvwCaseClaimList.Items.Add(CStr(lClaimId))

                        ' populate list sub item details
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsClaimNumber).Text = gPMFunctions.ToSafeString(m_vCaseClaimList(kCaseClaimListClaimNumber, lItem))
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsLossdate).Text = DateTime.Parse(gPMFunctions.ToSafeDate(m_vCaseClaimList(kCaseClaimListLossDate, lItem))).ToString("D")
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsClaimHandler).Text = gPMFunctions.ToSafeString(m_vCaseClaimList(kCaseClaimListClaimHandler, lItem))
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsRiskType).Text = gPMFunctions.ToSafeString(m_vCaseClaimList(kCaseClaimListRiskType, lItem))
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsStatus).Text = gPMFunctions.ToSafeString(m_vCaseClaimList(kCaseClaimListStatus, lItem))
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalIndemnity).Text = CStr(gPMFunctions.ToSafeCurrency(m_vCaseClaimList(kCaseClaimListTotalIndemnity, lItem)))
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalIndemnity).Text = CInt(gPMFunctions.ToSafeCurrency(m_vCaseClaimList(kCaseClaimListTotalIndemnity, lItem)))
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalExpense).Text = CStr(gPMFunctions.ToSafeCurrency(m_vCaseClaimList(kCaseClaimListTotalExpense, lItem)))
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalExpense).Text = CInt(gPMFunctions.ToSafeCurrency(m_vCaseClaimList(kCaseClaimListTotalExpense, lItem)))
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalExcess).Text = CStr(gPMFunctions.ToSafeCurrency(m_vCaseClaimList(kCaseClaimListTotalExcess, lItem)))
                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalExcess).Text = CInt(gPMFunctions.ToSafeCurrency(m_vCaseClaimList(kCaseClaimListTotalExcess, lItem)))

                        ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsInsuranceFileCnt).Text = CStr(gPMFunctions.ToSafeLong(m_vCaseClaimList(kCaseClaimListInsuranceFileCnt, lItem)))
                        oListItem.Tag = CStr(lClaimId)
                        bFound = False
                        If Information.IsArray(m_vCaseLink) Then
                            For Each m_vCaseLink_item As Object In m_vCaseLink
                                'developer guide no.248
                                If gPMFunctions.ToSafeLong(lClaimId) = gPMFunctions.ToSafeLong(m_vCaseLink_item) Then
                                    bFound = True
                                    Exit For
                                End If
                            Next m_vCaseLink_item
                        End If

                        If Not bFound Then
                            If Information.IsArray(m_vCaseLink) Then
                                ReDim Preserve m_vCaseLink(m_vCaseLink.GetUpperBound(0) + 1)
                            Else
                                ReDim m_vCaseLink(0)
                            End If
                            m_vCaseLink(m_vCaseLink.GetUpperBound(0)) = gPMFunctions.ToSafeLong(lClaimId)
                        End If

                        'Set m_vCaseUnlink = Nothing

                        m_lClaimID = lClaimId
                        'developer guide no.248
                        m_lInsuranceFileCnt = gPMFunctions.ToSafeLong(m_vCaseClaimList(kCaseClaimListInsuranceFileCnt, lItem))
                    End If
                Next lItem
                DisableInterface(bEnabled:=True, bAllControl:=False)
            Else
                DisableInterface(bEnabled:=False, bAllControl:=False)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetCaseClaimLink
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : VB : 06-07-2007 :
    ' ***************************************************************** '
    Private Function GetCaseClaimLink() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCaseClaimLink"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lBaseCaseId > 0 Then


                lReturn = m_oBusiness.GetLinks(v_lBaseCaseID:=m_lBaseCaseId, r_vLinks:=m_vCaseClaimList)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetLinks Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Parameters: n/a
    '
    ' Description: Display all language specific captions.
    '
    ' History:
    '           Created :
    ' ***************************************************************** '

    'Private Function DisplayCaptions() As Integer
    '
    'Dim result As Integer = 0
    'Const kMethodName As String = "DisplayCaptions"
    '
    'Dim lReturn As Integer
    'On Error GoTo Catch_Renamed
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' set the caption for the button

    'cmdOpen.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstOpenClaimButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '

    'cmdMaintain.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstMaintainClaimButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '

    'cmdPay.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstPayClaimButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '

    'cmdSalvage.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstSalvageButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '

    'cmdTPRecovery.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstTPRecoveryButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '

    'cmdLink.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLinkButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '

    'cmdUnlink.Text = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstUninkButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '
    ' set the caption for the List View
    '
    'lvwCaseClaimList.ColumnHeaders(0).
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    ' DO Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    ' If you want to rollback a transaction or something, do it here
    'Finally_Renamed: '
    '
    'Return result
    'Resume 
    'Return result
    'End Function


    ' ***************************************************************** '
    ' Name: GetClaim
    '
    ' Parameters: n/a
    '
    ' Description: Function to fetch the claim
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Private Function GetClaim(ByRef r_lClaimID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaim"

        Dim lReturn As Integer

        'developer guide no. 50
        Dim oFindClaim As Object 'iCLMFindClaim.Interface

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oFindClaim As Object
            'developer guide no.(Changed as per DLL name)
            lReturn = m_oObjectManager.GetInstance(temp_oFindClaim, sClassName:="iCLMFindClaim.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindClaim = temp_oFindClaim

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create object 'ICLMFINDCLAIM.Interface'.", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = oFindClaim.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to start Find Claim", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Check the status first

            If oFindClaim.Status <> gPMConstants.PMEReturnCode.PMCancel Then


                r_lClaimID = oFindClaim.Claimcnt
                '      MsgBox oFindClaim.InsFileCnt
                '      MsgBox FormatField(iFormatType:=PMFormatString, vFieldValue:=Trim$(oFindClaim.ClaimRef))
                '      MsgBox FormatField(iFormatType:=PMFormatString, vFieldValue:=Trim$(oFindClaim.PolicyRef))
            End If

            ' Destroy Find Insurance object

            oFindClaim.Dispose()
           



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetClaimDetail
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' Created : VB : 06-07-2007 :
    ' ***************************************************************** '
    Private Function GetClaimDetail(ByVal v_lClaimID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimDetail"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetClaimDetail(v_lClaimID:=v_lClaimID, r_vResultArray:=m_vClaimDetail)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLinks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: CopyClaim
    ' Parameters: n/a
    ' Description: Function to fetch the claim
    ' History:
    ' Created :
    ' ***************************************************************** '
    Private Function CopyClaim(ByVal v_lClaimID As Integer, ByRef r_lCopyClaimId As Integer, Optional ByVal v_sTransactionType As String = "") As Integer
        Dim result As Integer = 0
        Dim bCLMFindClaim As Object

        Const kMethodName As String = "CopyClaim"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oBusiness As bCLMFindClaim.Business
        Dim lCopyClaimId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oBusiness As Object
            lReturn = m_oObjectManager.GetInstance(temp_oBusiness, "bCLMFindClaim.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create object 'bCLMFindClaim.Interface'.", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Set TransactionType

            lReturn = oBusiness.SetProcessModes(vTransactionType:=v_sTransactionType)

            ' clean up any existing dirty claims prior to taking out a new lock

            lReturn = oBusiness.CleanUpDirtyClaims(v_lClaimID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to CleanUpDirtyClaims", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = CType(LockClaim(r_oBusiness:=oBusiness), gPMConstants.PMEReturnCode)
            If lReturn = gPMConstants.PMEReturnCode.PMRecordInUse Then
                result = gPMConstants.PMEReturnCode.PMRecordInUse
                Return result
            ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Lock Claim", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = oBusiness.ProcessCopyClaim(v_lClaimID:=v_lClaimID, r_lCopyClaimId:=lCopyClaimId)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to ProcessCopyClaim", gPMConstants.PMELogLevel.PMLogError)
            End If

            r_lCopyClaimId = lCopyClaimId



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally
            If Not (oBusiness Is Nothing) Then
                ' Destroy object

                oBusiness.Dispose()
                
            End If



        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: LockClaim
    '
    ' Description:
    '
    ' History: VB
    '
    ' ***************************************************************** '
    Private Function LockClaim(ByRef r_oBusiness As Object) As Integer
        Dim result As Integer = 0
        Dim bPMLock As Object

        Const kMethodName As String = "LockClaim"


        Dim oPMLock As bPMLock.User
        Dim sLockedBy As String = ""
        Dim lReturn As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'Get bPMLock
            Dim temp_oPMLock As Object
            lReturn = m_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            'Check for errors.
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to process the interface.", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = r_oBusiness.GetOriginalClaimId(m_lClaimID, m_lOriginalClaimId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to process the interface.", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oPMLock.LockKey(sKeyName:="claim_id", vKeyValue:=m_lOriginalClaimId, iUserID:=m_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy, v_bOtherUserOnly:=False)


            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    'OK
                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    If sLockedBy = "ERROR" Then
                        gPMFunctions.RaiseError(kMethodName, "Error trying to lock record", gPMConstants.PMELogLevel.PMLogError)
                    Else
                        result = gPMConstants.PMEReturnCode.PMRecordInUse
                        MessageBox.Show("Claim currently locked by " & sLockedBy & _
                                        Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Find Claim")
                        Return result
                    End If
                Case Else
                    ' Log Error.
                    gPMFunctions.RaiseError(kMethodName, "Failed to lock the screen", gPMConstants.PMELogLevel.PMLogError)
            End Select


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        Finally



        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: UpdatedCaseClaimList
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Private Function UpdatedCaseClaimList() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatedCaseClaimList"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lClaimId, llBound, lUBound As Integer
        Dim bFound As Boolean
        Dim oListItem As ListViewItem

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vCaseLink) Then

                llBound = m_vCaseLink.GetLowerBound(0)
                lUBound = m_vCaseLink.GetUpperBound(0)

                For lItem As Integer = llBound To lUBound

                    lClaimId = gPMFunctions.ToSafeLong(CInt(m_vCaseLink(lItem)))

                    lReturn = CType(GetClaimDetail(v_lClaimID:=lClaimId), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If Information.IsArray(m_vClaimDetail) Then

                        bFound = False
                        If lvwCaseClaimList.Items.Count > 0 Then
                            For lIndex As Integer = 1 To lvwCaseClaimList.Items.Count
                                'developer guide no.248
                                If gPMFunctions.ToSafeString(ListViewHelper.GetListViewSubItem(lvwCaseClaimList.Items.Item(lIndex - 1), kPayDetailsSubItemsClaimNumber).Text) = gPMFunctions.ToSafeString(m_vClaimDetail(kClaimDetailClaimNumber, 0)) Then
                                    bFound = True
                                    Exit For
                                End If
                            Next lIndex
                        End If

                        If Not bFound Then
                            'developer guide no.248
                            lClaimId = gPMFunctions.ToSafeLong(m_vClaimDetail(kClaimDetailClaimId, 0))
                            oListItem = lvwCaseClaimList.Items.Add(CStr(lClaimId))

                            If Information.IsArray(m_vClaimDetail) Then

                                ' populate list sub item details
                                ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsClaimNumber).Text = gPMFunctions.ToSafeString(m_vClaimDetail(kClaimDetailClaimNumber, 0))
                                ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsLossdate).Text = DateTime.Parse(gPMFunctions.ToSafeDate(m_vClaimDetail(kClaimDetailLossDate, 0))).ToString("D")
                                ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsStatus).Text = gPMFunctions.ToSafeString(m_vClaimDetail(kClaimDetailStatus, 0))
                                ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalIndemnity).Text = CStr(gPMFunctions.ToSafeCurrency(m_vClaimDetail(kClaimDetailTotalIndemnity, 0)))
                                ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalExpense).Text = CStr(gPMFunctions.ToSafeCurrency(m_vClaimDetail(kClaimDetailTotalExpense, 0)))
                                ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsTotalExcess).Text = CStr(gPMFunctions.ToSafeCurrency(m_vClaimDetail(kClaimDetailTotalExcess, 0)))
                                ListViewHelper.GetListViewSubItem(oListItem, kPayDetailsSubItemsInsuranceFileCnt).Text = CStr(gPMFunctions.ToSafeLong(m_vClaimDetail(kClaimDetailInsuranceFileCnt, 0)))
                                oListItem.Tag = CStr(lClaimId)
                                m_vCaseLink(lItem) = lClaimId
                            End If
                        End If
                    End If
                Next lItem
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function



    'developer guide no. 1 No Solution
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        PropBag.WriteProperty("Enabled", MyBase.Enabled, m_def_Enabled)
    End Sub


    ' ***************************************************************** '
    ' Name: PrintDocument
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '

    'Private Function PrintDocument(ByVal v_lDocumentTemplateID As Integer, ByVal v_lDocumentTypeID As Integer, ByVal v_lSpoolMode As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Const kMethodName As String = "PrintDocument"
    '
    'Dim lReturn As gPMConstants.PMEReturnCode
    'Dim oDocManagerWrapper As bSIRDocManagerWrapper.Interface
    '
    'On Error GoTo Catch_Renamed
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    '
    'If v_lDocumentTemplateID <> 0 Then
    '
    'Hook up document management
    'oDocManagerWrapper = New bSIRDocManagerWrapper.Interface()
    'lReturn = CType(CType(oDocManagerWrapper, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRDocManagerWrapper", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    'Set up the document
    'oDocManagerWrapper.DocumentTemplateId = v_lDocumentTemplateID
    'oDocManagerWrapper.DocumentTypeId = v_lDocumentTypeID
    'oDocManagerWrapper.ClaimCnt = m_lClaimID
    'oDocManagerWrapper.PartyCnt = r_vResultArray(ACIPartyCnt, lRow)
    'If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
    'oDocManagerWrapper.SpoolDesc = "Open Case Print Letter"
    'ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit Then 
    'oDocManagerWrapper.SpoolDesc = "Edit Case Print Letter"
    'End If
    '
    'oDocManagerWrapper.Mode = v_lSpoolMode
    '
    ' Print the document
    'lReturn = oDocManagerWrapper.Start()
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "Failed to Start", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    '
    'oDocManagerWrapper.Terminate()
    '
    'End If
    '
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    ' If you want to rollback a transaction or something, do it here
    '
    'Finally_Renamed: '
    'Return result
    'Resume 
    'Return result
    'End Function


    ' ***************************************************************** '
    ' Name: GetTemplateType
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Private Function GetTemplateType(ByVal lDocumentTemplateID As Integer, ByRef r_lDocumentTypeID As Integer) As Integer
        Dim result As Integer = 0
        Dim bSIRDocTemplate As Object


        Const kMethodName As String = "GetTemplateType"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oDocTemplate As bSIRDocTemplate.Business

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            Dim temp_oDocTemplate As Object
            lReturn = m_oObjectManager.GetInstance(temp_oDocTemplate, "bSIRDocTemplate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oDocTemplate = temp_oDocTemplate
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRDocManagerWrapper", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oDocTemplate.GetDetails(vDocumentTemplateID:=lDocumentTemplateID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            m_lReturn = oDocTemplate.GetNext(vDocumentTypeId:=r_lDocumentTypeID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            oDocTemplate.Dispose()
            oDocTemplate = Nothing

            



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: GetCaseDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : VB : 06-07-2007 :
    ' ***************************************************************** '
    Private Function GetCaseDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCaseDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lCaseID > 0 Then


                lReturn = m_oBusiness.GetCaseDetails(v_lCaseID:=m_lCaseID, r_vResultArray:=m_vCaseDetails)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetCaseDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function



    ' ***************************************************************** '
    '
    ' Name: UseTheTemplate
    '
    ' Description:
    '
    ' History:
    '
    ' ***************************************************************** '
    Private Function UseTheTemplate(ByVal v_lDocId As Integer, ByVal v_lDocTypeId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UseTheTemplate"

        Dim lReturn As Integer

        'developer guide no. 50
        Dim oObject As iPMBDocTemplate.Interface_Renamed

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            Dim temp_oObject As Object
            m_lReturn = m_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oObject = Nothing
                Return result
            End If


            oObject.Claimcnt = m_lClaimID

            oObject.DocumentTemplateId = v_lDocId

            oObject.DocumentTypeId = v_lDocTypeId

            oObject.CallingAppName = ACApp
            ''62125

            oObject.PartyCnt = m_lPartyCnt
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                oObject.spooldesc = "Open Case Print Letter"
            ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                oObject.spooldesc = "Edit Case Print Letter"
            End If


            oObject.Mode = gSIRLibrary.ACMergeMode


            m_lReturn = oObject.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            oObject.Dispose()

           



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally
            oObject = Nothing
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
        Return result
    End Function
    'developer guide no.(Matching event as per VB)
    Private Sub lvwCaseClaimlist_ItemSelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvwCaseClaimlist.ItemSelectionChanged
        If lvwCaseClaimlist.Items.Count > 0 Then

            m_lClaimID = Convert.ToString(lvwCaseClaimlist.FocusedItem.Tag)
            m_lInsuranceFileCnt = CInt(ListViewHelper.GetListViewSubItem(lvwCaseClaimlist.Items.Item(lvwCaseClaimlist.FocusedItem.Index), kPayDetailsSubItemsInsuranceFileCnt).Text)

            cmdMaintain.Enabled = True

            If gPMFunctions.ToSafeString(ListViewHelper.GetListViewSubItem(lvwCaseClaimlist.Items.Item(lvwCaseClaimlist.FocusedItem.Index), kPayDetailsSubItemsStatus).Text).Trim().ToUpper() = "CLOSED" Then


                cmdTPRecovery.Enabled = False
                cmdSalvage.Enabled = False
                cmdPay.Enabled = False
            Else
                cmdTPRecovery.Enabled = True
                cmdSalvage.Enabled = True
                cmdPay.Enabled = True
            End If
            cmdUnlink.Enabled = True
        Else
            DisableInterface(bEnabled:=False, bAllControl:=False)
        End If

        SetInterfaceforCloseCase()
    End Sub

    Private Sub SetInterfaceforCloseCase()

        If m_sCaseProgressStatusCode = "CLOSED" Then
            cmdLink.Enabled = False
            cmdMaintain.Enabled = False
            cmdOpen.Enabled = False
            cmdPay.Enabled = False
            cmdSalvage.Enabled = False
            cmdTPRecovery.Enabled = False
            cmdUnlink.Enabled = False

        End If
    End Sub
End Class
