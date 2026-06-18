Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmMDI
	Inherits System.Windows.Forms.Form
	Private Sub frmMDI_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	'
	' History:
	' CJB 080805 PN23013 Changed Toolbar1_ButtonClick to prevent access to view claims if no access given
	' CJB 280905 PN24360 Changed Toolbar1_ButtonClick to preevnt access to raise cash trans if no access given
	'
	
	Private Const ACClass As String = "frmMDI"
	Private Const ACCalledViaClientManager As Boolean = True
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Private m_lPartyCnt As Integer
	Private m_sShortName As String = "" 'ADDED MK 991014
	Private m_sResolvedName As String = "" 'ADDED MK 991014
	Private m_sPartyType As String = "" 'Added MKR PN 17193
	
	Private m_lInsFileCnt As Integer 'ADDED MK 991015
	Private m_lInsuranceFolderCnt As Integer 'ADDED MK 991015
	Private m_sInsReference As String = "" 'ADDED MK 991015
	Private m_lClaimCnt As Integer
	Private m_lPolicyTypeID As Integer
	Private m_vGeminiPolicyStatus As Integer
	'Private lRiskGroupId  As String         'ADDED MK 991015
	Private m_lRiskCnt As Integer 'eck140301
	Private m_sLiveForm As String = ""
	Private m_sClaimNo As String = ""
	
	Private m_oBusiness As Object
	
	Private m_sEmailAddress As String = ""
    Dim vContacts As Object

    Private objCM As MainModule
    Public WriteOnly Property ModuleClass() As MainModule
        Set(ByVal value As MainModule)
            objCM = value
        End Set
    End Property
	
	Public Property PartyCnt() As Integer
		Get
			Return m_lPartyCnt
		End Get
		Set(ByVal Value As Integer)
			m_lPartyCnt = Value
		End Set
	End Property
	
	Public Property ShortName() As String
		Get
			Return m_sShortName
		End Get
		Set(ByVal Value As String) 'ADDED MK 991014
			m_sShortName = Value
		End Set
	End Property
	
	Public Property ResolvedName() As String
		Get
			Return m_sResolvedName
		End Get
		Set(ByVal Value As String) 'ADDED MK 991014
			m_sResolvedName = Value
		End Set
	End Property
	
	Public Property PartyType() As String
		Get
			Return m_sPartyType
		End Get
		Set(ByVal Value As String) 'Added MKR PN 17193
			m_sPartyType = Value
		End Set
	End Property
	
	Public Property InsFileCnt() As Integer
		Get
			Return m_lInsFileCnt
		End Get
		Set(ByVal Value As Integer)
			m_lInsFileCnt = Value
		End Set
	End Property
	
	Public Property InsuranceFolderCnt() As Integer
		Get
			Return m_lInsuranceFolderCnt
		End Get
		Set(ByVal Value As Integer)
			m_lInsuranceFolderCnt = Value
		End Set
	End Property
	
	Public Property InsReference() As String
		Get
			Return m_sInsReference
		End Get
		Set(ByVal Value As String)
			m_sInsReference = Value
		End Set
	End Property
	'eck140301
	Public Property RiskCnt() As Integer
		Get
			Return m_lRiskCnt
		End Get
		Set(ByVal Value As Integer)
			m_lRiskCnt = Value
		End Set
	End Property
	'eck140301 end
	
	Public Property ClaimCnt() As Integer
		Get
			Return m_lClaimCnt
		End Get
		Set(ByVal Value As Integer)
			m_lClaimCnt = Value
		End Set
	End Property
	
	Public Property LiveForm() As String
		Get
			Return m_sLiveForm
		End Get
		Set(ByVal Value As String)
			m_sLiveForm = Value
		End Set
	End Property
	
	Public Property PolicyTypeId() As Integer
		Get
			Return m_lPolicyTypeID
		End Get
		Set(ByVal Value As Integer)
			m_lPolicyTypeID = Value
		End Set
	End Property
	
	Public Property GeminiPolicyStatus() As Integer
		Get
			Return m_vGeminiPolicyStatus
		End Get
		Set(ByVal Value As Integer)

			m_vGeminiPolicyStatus = CInt(Value)
		End Set
	End Property
	
	Public Property ClaimNo() As String
		Get
			
			Return m_sClaimNo
			
		End Get
		Set(ByVal Value As String)
			
			m_sClaimNo = Value
			
		End Set
    End Property

    Private Sub frmMDI_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        ''TODO
        '' CTAF 270900
        'm_lReturn = CType(ProcessSwiftFunction(v_iSwiftFunction:=ACSwiftCheckInstalled, r_vInstalled:=g_bSwiftInstalled), gPMConstants.PMEReturnCode)
        'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '	Exit Sub
        'End If

        '' Application starts here (Load event of Startup form).
        ''sj 03/07/2002 - start
        'If objCM.g_bRestrictInsurerAccess Then
        '	m_lReturn = CType(SetRestrictedToolbar(v_sFormName:=Me.Name), gPMConstants.PMEReturnCode)
        'Else
        '	m_lReturn = CType(objCM.SetToolbar(v_sFormName:=Me.Name), gPMConstants.PMEReturnCode)
        'End If
        ''sj 03/07/2002 - end

        'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '	MessageBox.Show("Problem Setting Toolbar keys", Application.ProductName)
        'End If

        'Show()
        '' Always set the working directory to the directory containing the application.
        'Directory.SetCurrentDirectory(My.Application.Info.DirectoryPath)
        '' Initialize the document form array, and show the first document.
        ''WhaTT?
        ''    Set Document = CreateObject("frmPartyPC.frm")
        'ReDim Document(1)
        ''WhaTT?
        'ReDim FState(1)
        'FState(1).Deleted = True
        ''    Document(1).Tag = 1
        'FState(1).Dirty = False
        '' Read System registry and set the recent menu file list control array appropriately.
        ''GetRecentFiles
        '' CTAF 170801 - Use objCM.LoadRecentFiles
        'm_lReturn = CType(objCM.LoadRecentFilesFromReg(), gPMConstants.PMEReturnCode)

        '' Set public variable gFindDirection which determines which direction
        '' the FindIt function will search in.
        'gFindDirection = 1

    End Sub

    Private Sub frmMDI_Closed(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles MyBase.Closing

        ' If the Unload event was not cancelled (in the QueryUnload events for the Notepad forms),
        ' there will be no document window left, so go ahead and end the application.
        'ECK 15/06/99 Commented this out - don't think it is neccessary
        '    If Not AnyPadsLeft() Then
        '        Cancel = False
        '    Else
        '        Cancel = True
        '    End If

        'PN26814 Do not allow the main MDI form to close if a Navigator process is still running
        'as the Terminate method will never be invoked, leaving the process running

        If Not eventArgs.Cancel Then
            If objCM.IsNavStartRunning Then
                MessageBox.Show("Cannot Close while Navigator Process is running", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                eventArgs.Cancel = IIf(0, False, True)
                Exit Sub
            End If

            If Not (objCM.g_oCMManager Is Nothing) Then
                ' Call the Client Manager Manager to tell it that we're terminating

                m_lReturn = objCM.g_oCMManager.TerminateCallBack(v_lPartyCnt:=PartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unload interface - TerminateCallBack",
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="frmMDI_Closed", vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

                objCM.g_oCMManager.Dispose()
            End If
        End If



        ' CTAF 20020731
        ' Remove all the licenses to clear up

        'Developer Guide No. 7(No Solutions)
        'While Licenses.Count > 0

        '	Licenses.Remove(0)
        'End While

    End Sub

    Public Sub mnuClientExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuClientExit.Click
        ' End the application.
        Me.Close() 'ADDED MK 991015
        'End
    End Sub

    Public Sub mnuClientOpen_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuClientOpen.Click

        m_lReturn = CType(objCM.OpenClient(), gPMConstants.PMEReturnCode)

    End Sub

    Public Sub mnuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpAbout.Click

        m_lReturn = CType(objCM.ShowSBOAbout(), gPMConstants.PMEReturnCode)

    End Sub

    Public Sub mnuRecentFile_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuRecentFile_0.Click, _mnuRecentFile_1.Click, _mnuRecentFile_2.Click, _mnuRecentFile_3.Click, _mnuRecentFile_4.Click, _mnuRecentFile_5.Click
        Dim Index As Integer = Array.IndexOf(mnuRecentFile, eventSender)

        m_lReturn = CType(objcm.ShowRecentFile(iIndex:=Index, r_oForm:=Me), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub Toolbar1_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _Toolbar1_Button1.Click, _Toolbar1_Button2.Click, _Toolbar1_Button3.Click, _Toolbar1_Button4.Click, _Toolbar1_Button5.Click, _Toolbar1_Button6.Click, _Toolbar1_Button7.Click, _Toolbar1_Button8.Click, _Toolbar1_Button9.Click, _Toolbar1_Button10.Click, _Toolbar1_Button11.Click, _Toolbar1_Button12.Click, _Toolbar1_Button13.Click, _Toolbar1_Button14.Click, _Toolbar1_Button15.Click, _Toolbar1_Button16.Click, _Toolbar1_Button17.Click, _Toolbar1_Button18.Click, _Toolbar1_Button19.Click, _Toolbar1_Button20.Click, _Toolbar1_Button21.Click, _Toolbar1_Button22.Click, _Toolbar1_Button23.Click, _Toolbar1_Button24.Click, _Toolbar1_Button25.Click, _Toolbar1_Button26.Click, _Toolbar1_Button27.Click, _Toolbar1_Button28.Click, _Toolbar1_Button29.Click, _Toolbar1_Button30.Click, _Toolbar1_Button31.Click, _Toolbar1_Button32.Click, _Toolbar1_Button33.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        Const kMethodName As String = "Toolbar1_ButtonClick"

        Dim vRiskCodeId As Object
        Dim lRiskCodeId As Integer
        Dim vRiskGroupId As Object
        Dim lRiskGroupId As Integer
        Dim bCarryOn As Boolean
        'CT 19/10/00 variables to hold Gis ScreenId
        Dim vSBORiskScreenId As Object
        Dim lSBORiskScreenId As Integer
        'eck070301
        Dim vRiskCnt As Object
        Dim lRiskCnt As Integer
        'DC060404 PN10667 refresh policy details if editting
        Dim sGisDataModelCode As String = ""
        Dim bBrokerlink As Boolean
        Dim sDmCode As String = ""

        Dim lClaimID, lInsuranceFileCnt As Integer
        Select Case Button.Name
            Case "New"
                objCM.FileNew(vPartyType:="P")

            Case "Open"
                objCM.FileOpenProc(v_lPartyCnt:=PartyCnt, vPartyType:="A")

            Case "Accounts"
                ' Call OrionLinkFunc function
                'eck050900
                If InsFileCnt = 0 Then
                    m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=ACIGotoAccounts, v_sShortName:=ShortName, v_bCalledViaClientManager:=ACCalledViaClientManager), gPMConstants.PMEReturnCode)
                Else
                    m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=ACIGotoAccounts, v_sShortName:=m_sShortName, v_sInsuranceRef:=m_sInsReference, v_bCalledViaClientManager:=ACCalledViaClientManager), gPMConstants.PMEReturnCode)
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
                ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.1.1)
            Case "InsuredAccounts"
                If InsFileCnt = 0 Then
                    m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=objCM.ACIGotoInsuredAccounts, v_sShortName:=ShortName, v_bCalledViaClientManager:=ACCalledViaClientManager), gPMConstants.PMEReturnCode)
                Else
                    m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=objCM.ACIGotoInsuredAccounts, v_sShortName:=m_sShortName, v_sInsuranceRef:=m_sInsReference, v_bCalledViaClientManager:=ACCalledViaClientManager), gPMConstants.PMEReturnCode)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "ProcessOrionFunc Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.1.1)

            Case "Debit"

                'DC080404 PN11508 call from frmPolicy itself to refresh after transaction
                'DC140404 PN11797 and also from frmPolicySummary now refreshes
                If Me.LiveForm = "frmPolicy" Or Me.LiveForm = "frmPolicySummary" Then
                    '2005 Client Manager Security
                    If Not objCM.g_bRaiseDebitAuthority Then
                        MessageBox.Show("You do not have authority to raise debits.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                            If Application.OpenForms.Item(iLoop1).Name = "frmPolicy" Then

                                'NIIT - Replaced with the Migrated code 1144
                                'If Application.OpenForms.Item(iLoop1).InsFileCnt = m_lInsFileCnt Then
                                If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsFileCnt") = m_lInsFileCnt Then

                                    'NIIT - Replaced with the Migrated code 1144
                                    'm_lReturn = Application.OpenForms.Item(iLoop1).RaiseDebitTransaction
                                    m_lReturn = ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "RaiseDebitTransaction")
                                    Exit For
                                End If
                            End If
                            If Application.OpenForms.Item(iLoop1).Name = "frmPolicySummary" Then

                                'NIIT - Replaced with the Migrated code 1144
                                'If Application.OpenForms.Item(iLoop1).InsuranceFileCnt = m_lInsFileCnt Then
                                If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFileCnt") = m_lInsFileCnt Then

                                    'NIIT - Replaced with the Migrated code 1144
                                    'm_lReturn = Application.OpenForms.Item(iLoop1).RaiseDebitTransaction
                                    m_lReturn = ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "RaiseDebitTransaction")
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                Else

                    ' Call OrionLinkFunc function
                    m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=ACIGoToTransactionDebit, v_lInsuranceFileCnt:=m_lInsFileCnt), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                            MessageBox.Show("Policy needs to be saved before transactions can be raised.", Application.ProductName)
                        End If
                        Exit Sub
                    End If

                End If

            Case "Credit"

                'DC080404 PN11508 call from frmPolicy itself to refresh after transaction
                'DC140404 PN11797 and also from frmPolicySummary now refreshes
                If Me.LiveForm = "frmPolicy" Or Me.LiveForm = "frmPolicySummary" Then
                    '2005 Client Manager Security
                    If Not objCM.g_bRaiseCreditAuthority Then
                        MessageBox.Show("You do not have authority to raise credits.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                            If Application.OpenForms.Item(iLoop1).Name = "frmPolicy" Then

                                'NIIT - Replaced with the Migrated code 1144
                                'If Application.OpenForms.Item(iLoop1).InsFileCnt = m_lInsFileCnt Then
                                If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsFileCnt") = m_lInsFileCnt Then

                                    'NIIT - Replaced with the Migrated code 1144
                                    'm_lReturn = Application.OpenForms.Item(iLoop1).RaiseCreditTransaction
                                    m_lReturn = ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "RaiseCreditTransaction")
                                    Exit For
                                End If
                            End If
                            If Application.OpenForms.Item(iLoop1).Name = "frmPolicySummary" Then

                                'NIIT - Replaced with the Migrated code 1144
                                'If Application.OpenForms.Item(iLoop1).InsuranceFileCnt = m_lInsFileCnt Then
                                If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFileCnt") = m_lInsFileCnt Then

                                    'NIIT - Replaced with the Migrated code 1144
                                    'm_lReturn = Application.OpenForms.Item(iLoop1).RaiseCreditTransaction
                                    m_lReturn = ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "RaiseCreditTransaction")
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                Else

                    ' Call OrionLinkFunc function
                    m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=ACIGoToTransactionCredit, v_lInsuranceFileCnt:=m_lInsFileCnt), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                            MessageBox.Show("Policy needs to be saved before transactions can be raised.", Application.ProductName)
                        End If
                        Exit Sub
                    End If

                End If

            Case "Cash"
                If objCM.g_bRaiseCashAuthority Then
                    ' Call OrionLinkFunc function
                    m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=ACIGotoTransactionCash, v_sShortName:=ShortName), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If
                Else
                    ' Client Manager security for this process has been disallowed for this user  PN24360
                    MessageBox.Show("You do not have authority to raise cash transactions.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If

            Case "Policy"
                ' Call Toolbar Control function
                'Also sent PartyType as parameter in show policy... PN 17193
                m_lReturn = CType(objCM.ShowPolicy(v_lPartyCnt:=PartyCnt, v_sShortName:=ShortName, v_sPartyType:=PartyType), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("'Continue as not serious", Application.ProductName)
                    Exit Sub
                End If

            Case "Event"
                ' Call Toolbar Control function
                'm_lReturn& = objCM.ShowEvents(v_lPartyCnt:=PartyCnt&, _
                'v_sShortName:=ShortName$, _
                'v_sPartyType:="C", _
                'v_sResolvedName:=ResolvedName)

                'MSS280901 - Added for merge from UW.
                m_lReturn = CType(objCM.ShowEvents(v_lPartyCnt:=PartyCnt, v_sShortName:=ShortName, v_sPartyType:="C", v_sResolvedName:=ResolvedName, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsFileCnt, v_sPolicyDesc:=m_sInsReference, v_lClaimCnt:=m_lClaimCnt, v_sClaimDesc:=""), gPMConstants.PMEReturnCode)
                'MSS280901 - Merge end.

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Continue as not serious
                    Exit Sub
                End If

            Case "Risk"
                Select Case m_lPolicyTypeID
                    ' PSA 20092000
                    'Case PMBPolicyTypeGIIMotor
                    ' 25/04/2001 PSA - Start
                    'Case PMBPolicyTypeGIIMotor, _
                    ''     PMBPolicyTypeGIIHousehold
                    Case PMBConst.PMBPolicyTypeGIIMotor, PMBConst.PMBPolicyTypeGIIHousehold, PMBConst.PMBPolicyTypeGIICommercialVehicle
                        ' 25/04/2001 PSA - End
                        ' PSA 20092000
                        m_lReturn = CType(objCM.ShowGeminiPolicyDetail(v_lPartyCnt:=m_lPartyCnt, v_sPartyType:="X", v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsFileCnt:=m_lInsFileCnt, v_lInsuranceFileStructureId:=0, v_sShortName:=m_sShortName, v_sInsReference:=m_sInsReference, v_bFromEvent:=False, v_lPolicyTypeId:=m_lPolicyTypeID, v_vGeminiPolicyStatus:=m_vGeminiPolicyStatus), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            'Continue as not serious
                            Exit Sub
                        End If

                        'CT 13/09/00 Show RSA underwriting ListRisk screen - start
                    Case PMBConst.PMBPolicyTypeUnderwriting
                        ' Make sure it's not already displayed.
                        For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                            If Application.OpenForms.Item(iLoop1).Name = LiveForm Then

                                'Is it this version?
                                'And who used different properties anyway?
                                bCarryOn = False

                                If LiveForm = "frmPolicyUnderwriting" Then

                                    'NIIT - Replaced with the Migrated code 1144
                                    'If Application.OpenForms.Item(iLoop1).InsFileCnt = Me.InsFileCnt Then
                                    If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsFileCnt") = Me.InsFileCnt Then
                                        bCarryOn = True
                                    End If
                                Else

                                    'NIIT - Replaced with the Migrated code 1144
                                    'If Application.OpenForms.Item(iLoop1).InsuranceFileCnt = Me.InsFileCnt Then
                                    If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFileCnt") = Me.InsFileCnt Then
                                        bCarryOn = True
                                    End If
                                End If

                                If bCarryOn Then
                                    'Show ListRiskForm as underwriting policies may have multiple risks against them
                                    ' Call Toolbar Control function
                                    m_lReturn = CType(objCM.ShowListofRisks(v_lPartyCnt:=PartyCnt, v_sShortName:=ShortName, v_sPartyType:="X", v_sResolvedName:="Resolved", v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsFileCnt, v_sPolicyDesc:=m_sInsReference, v_lRiskCodeId:=lRiskCodeId, v_lRiskGroupID:=lRiskGroupId, v_sInsuranceRef:=m_sInsReference, v_bFromEvent:=False), gPMConstants.PMEReturnCode)

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        'Continue as not serious
                                        Exit Sub
                                    End If
                                End If
                            End If
                        Next iLoop1
                        'CT 13/09/00 Show RSA underwriting ListRisk screen - end

                    Case Else
                        ' Make sure it's not already displayed.
                        For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                            If Application.OpenForms.Item(iLoop1).Name = LiveForm Then

                                'Is it this version?
                                'And who used different properties anyway?
                                bCarryOn = False

                                If LiveForm = "frmPolicy" Then

                                    'NIIT - Replaced with the Migrated code 1144
                                    'If Application.OpenForms.Item(iLoop1).InsFileCnt = Me.InsFileCnt Then
                                    If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsFileCnt") = Me.InsFileCnt Then
                                        bCarryOn = True
                                    End If
                                Else

                                    'NIIT - Replaced with the Migrated code 1144
                                    'If Application.OpenForms.Item(iLoop1).InsuranceFileCnt = Me.InsFileCnt Then
                                    If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFileCnt") = Me.InsFileCnt Then
                                        bCarryOn = True
                                    End If
                                End If

                                If bCarryOn Then
                                    ' Get the risk details
                                    If LiveForm = "frmPolicy" Or LiveForm = "frmPolicySummary" Then


                                        'NIIT - Replaced with the Migrated code 1144 
                                        'vRiskGroupId = Application.OpenForms.Item(iLoop1).uctPolicyControl1.RiskGroupId
                                        vRiskGroupId = ReflectionHelper.GetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPolicyControl1"), "RiskGroupId")


                                        'NIIT - Replaced with the Migrated code 1144 
                                        'vRiskCodeId = Application.OpenForms.Item(iLoop1).uctPolicyControl1.RiskCodeId
                                        vRiskCodeId = ReflectionHelper.GetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPolicyControl1"), "RiskCodeId")
                                        'CT19/10/00  get new risk screen id


                                        'NIIT - Replaced with the Migrated code 1144 
                                        'vSBORiskScreenId = Application.OpenForms.Item(iLoop1).uctPolicyControl1.RiskScreenId
                                        vSBORiskScreenId = ReflectionHelper.GetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPolicyControl1"), "RiskScreenId")
                                        'eck070301


                                        'NIIT - Replaced with the Migrated code 1144
                                        'vRiskCnt = Application.OpenForms.Item(iLoop1).uctPolicyControl1.RiskCnt
                                        vRiskCnt = ReflectionHelper.GetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPolicyControl1"), "RiskCnt")
                                    Else


                                        'NIIT - Replaced with the Migrated code 1144
                                        'vRiskGroupId = Application.OpenForms.Item(iLoop1).uctPolicySummControl1.RiskGroupId
                                        vRiskGroupId = ReflectionHelper.GetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPolicySummControl1"), "RiskGroupId")


                                        'NIIT - Replaced with the Migrated code 1144
                                        'vRiskCodeId = Application.OpenForms.Item(iLoop1).uctPolicySummControl1.RiskCodeId
                                        vRiskCodeId = ReflectionHelper.GetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPolicySummControl1"), "RiskCodeId")
                                        'CT19/10/00  get new risk screen id


                                        'NIIT - Replaced with the Migrated code 1144
                                        'vSBORiskScreenId = Application.OpenForms.Item(iLoop1).uctPolicySummControl1.RiskScreenId
                                        vSBORiskScreenId = ReflectionHelper.GetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPolicySummControl1"), "RiskScreenId")
                                        'eck070301


                                        'NIIT - Replaced with the Migrated code 1144
                                        'vRiskCnt = Application.OpenForms.Item(iLoop1).uctPolicySummControl1.RiskCnt
                                        vRiskCnt = ReflectionHelper.GetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPolicySummControl1"), "RiskCnt")
                                    End If


                                    If Convert.IsDBNull(vRiskCodeId) Or IsNothing(vRiskCodeId) Then
                                        lRiskCodeId = 0
                                    Else

                                        lRiskCodeId = CInt(vRiskCodeId)
                                    End If


                                    If Convert.IsDBNull(vRiskGroupId) Or IsNothing(vRiskGroupId) Then
                                        lRiskGroupId = 0
                                    Else

                                        lRiskGroupId = CInt(vRiskGroupId)
                                    End If


                                    If Convert.IsDBNull(vSBORiskScreenId) Or IsNothing(vSBORiskScreenId) Then
                                        lSBORiskScreenId = 0
                                    Else

                                        lSBORiskScreenId = CInt(vSBORiskScreenId)
                                    End If
                                    'eck070301

                                    If Convert.IsDBNull(vRiskCnt) Or IsNothing(vRiskCnt) Then
                                        lRiskCnt = 0
                                    Else

                                        lRiskCnt = CInt(vRiskCnt)
                                    End If

                                    m_lReturn = CType(objCM.CheckIfBrokerlinkRisk(v_lInsuranceFileCnt:=m_lInsFileCnt, r_bBrokerlink:=bBrokerlink, r_sDmCode:=sDmCode), gPMConstants.PMEReturnCode)
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        'Continue as not serious
                                        Exit Sub
                                    End If

                                    If bBrokerlink Then
                                        m_lReturn = CType(objCM.ShowBrokerlinkRisk(v_sGisDataModelCode:=sDmCode, v_lInsuranceFileCnt:=m_lInsFileCnt), gPMConstants.PMEReturnCode)
                                    Else
                                        ' Call Toolbar Control function
                                        'ISS1498 JAS 05/12/02 - added m_lPolicyTypeId

                                        m_lReturn = objCM.ShowRisk(v_lPartyCnt:=PartyCnt, v_sShortName:=ShortName, v_sPartyType:="X", v_sResolvedName:="Resolved", v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsFileCnt, v_sPolicyDesc:=m_sInsReference, v_lRiskCodeId:=lRiskCodeId, v_lRiskGroupID:=lRiskGroupId, v_bFromEvent:=False, v_lRiskScreenId:=lSBORiskScreenId, v_lRiskCnt:=lRiskCnt, v_lPolicyTypeId:=m_lPolicyTypeID)
                                    End If
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        'Continue as not serious
                                        Exit Sub
                                    End If

                                End If
                            End If
                        Next iLoop1
                End Select

            Case "Text"

                'DC110401 set risk code & risk group
                For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                    If Application.OpenForms.Item(iLoop1).Name = LiveForm Then

                        'Is it this version?
                        'And who used different properties anyway?
                        bCarryOn = False

                        If LiveForm = "frmPolicy" Then

                            'NIIT - Replaced with the Migrated code 1144
                            'If Application.OpenForms.Item(iLoop1).InsFileCnt = Me.InsFileCnt Then
                            If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsFileCnt") = Me.InsFileCnt Then
                                bCarryOn = True
                            End If
                            '<Pankaj PN:38833>
                        ElseIf (LiveForm = "frmPolicyUnderwriting") Then

                            'NIIT - Replaced with the Migrated code 1144
                            'If Application.OpenForms.Item(iLoop1).InsFileCnt = Me.InsFileCnt Then
                            If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsFileCnt") = Me.InsFileCnt Then
                                bCarryOn = False
                            End If
                        ElseIf (LiveForm = "frmPolicySummary") Then

                            'NIIT - Replaced with the Migrated code 1144
                            'If Application.OpenForms.Item(iLoop1).InsuranceFileCnt = Me.InsFileCnt Then
                            If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFileCnt") = Me.InsFileCnt Then
                                bCarryOn = True
                            End If
                        Else
                            ' MSS110701 - Added check to see if we are looking at any of the
                            ' party forms. If we are, we don't want to check the InsFileCnt
                            ' because it shouldn't have one
                            If LiveForm.Substring(0, Math.Min(LiveForm.Length, 8)) <> "frmParty" Then

                                'NIIT - Replaced with the Migrated code 1144
                                'If Application.OpenForms.Item(iLoop1).InsuranceFileCnt = Me.InsFileCnt Then
                                If ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFileCnt") = Me.InsFileCnt Then
                                    bCarryOn = True
                                End If
                            End If
                        End If

                        If bCarryOn Then

                            If (LiveForm = "frmPolicy") Or (LiveForm = "frmPolicySummary") Then


                                'NIIT - Replaced with the Migrated code 1144
                                'vRiskGroupId = Application.OpenForms.Item(iLoop1).uctPolicyControl1.RiskGroupId
                                vRiskGroupId = ReflectionHelper.GetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPolicyControl1"), "RiskGroupId")


                                'NIIT - Replaced with the Migrated code 1144
                                'vRiskCodeId = Application.OpenForms.Item(iLoop1).uctPolicyControl1.RiskCodeId
                                vRiskCodeId = ReflectionHelper.GetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPolicyControl1"), "RiskCodeId")
                            Else
                                ' +++JJ 15/08/2003 IR 5185
                                If LiveForm = "frmPolicySummaryUnderwriting" Then


                                    'NIIT - Replaced with the Migrated code 1144
                                    'vRiskGroupId = Application.OpenForms.Item(iLoop1).uctPMUPolicySummary1.RiskGroupId
                                    vRiskGroupId = ReflectionHelper.GetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPMUPolicySummary1"), "RiskGroupId")


                                    'NIIT - Replaced with the Migrated code 1144
                                    'vRiskCodeId = Application.OpenForms.Item(iLoop1).uctPMUPolicySummary1.RiskCodeId
                                    vRiskCodeId = ReflectionHelper.GetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPMUPolicySummary1"), "RiskCodeId")
                                Else
                                    ' Leave old code here incase some other control is used
                                    ' you never know.


                                    'NIIT - Replaced with the Migrated code 1144
                                    'vRiskGroupId = Application.OpenForms.Item(iLoop1).uctPolicySummControl1.RiskGroupId
                                    vRiskGroupId = ReflectionHelper.GetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPolicySummControl1"), "RiskGroupId")


                                    'NIIT - Replaced with the Migrated code 1144
                                    'vRiskCodeId = Application.OpenForms.Item(iLoop1).uctPolicySummControl1.RiskCodeId
                                    vRiskCodeId = ReflectionHelper.GetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPolicySummControl1"), "RiskCodeId")
                                End If
                                ' --- JJ 15/08/2003 IR 5185
                            End If


                            If Convert.IsDBNull(vRiskCodeId) Or IsNothing(vRiskCodeId) Then
                                lRiskCodeId = 0
                            Else

                                lRiskCodeId = CInt(vRiskCodeId)
                            End If


                            If Convert.IsDBNull(vRiskGroupId) Or IsNothing(vRiskGroupId) Then
                                lRiskGroupId = 0
                            Else

                                lRiskGroupId = CInt(vRiskGroupId)
                            End If

                        End If

                    End If

                Next iLoop1
                'DC110401

                ' Call Toolbar Control function
                If InsFileCnt = 0 Then
                    m_lReturn = CType(objCM.ShowTextFiles(v_lPartyCnt:=PartyCnt, v_sShortName:=m_sShortName, v_sPartyType:="C", v_sResolvedName:=m_sResolvedName), gPMConstants.PMEReturnCode)

                Else

                    'DC110401 pass through Risk Code and Group also
                    '                m_lReturn& = objCM.ShowTextFiles(v_lPartyCnt:=PartyCnt&, _
                    ''                                   v_sShortName:=m_sShortName, _
                    ''                                   v_sPartyType:="C", _
                    ''                                   v_sResolvedName:=m_sResolvedName, _
                    ''                                   v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, _
                    ''                                   v_lInsuranceFileCnt:=m_lInsFileCnt, _
                    ''                                   v_sPolicyDesc:=m_sInsReference)

                    m_lReturn = CType(objCM.ShowTextFiles(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sPartyType:="C", v_sResolvedName:=m_sResolvedName, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsFileCnt, v_sPolicyDesc:=m_sInsReference, v_lRiskCodeId:=lRiskCodeId, v_lRiskGroupID:=lRiskGroupId), gPMConstants.PMEReturnCode)
                    'DC110401

                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Continue as not serious
                    Exit Sub
                End If

            Case "Public"
                ' Call Toolbar Control function
                If InsFileCnt = 0 Then
                    m_lReturn = CType(objCM.CallNotes(v_sEntityType:=gSIRLibrary.SIREntityNameParty, v_lEntityCnt:=m_lPartyCnt, v_sTextType:="Public"), gPMConstants.PMEReturnCode)
                Else
                    m_lReturn = CType(objCM.CallNotes(v_sEntityType:=gSIRLibrary.SIREntityNamePolicy, v_lEntityCnt:=m_lInsFileCnt, v_sTextType:="Public", v_lPartyCnt:=m_lPartyCnt), gPMConstants.PMEReturnCode)
                End If

                For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                    If Application.OpenForms.Item(iLoop1).Name = "frmListEvents" Then

                        'NIIT - Replaced with the Migrated code 1144
                        'Application.OpenForms.Item(iLoop1).RefreshList()
                        ReflectionHelper.Invoke(Application.OpenForms.Item(iLoop1), "RefreshList", New Object() {})
                        Exit For
                    End If
                Next
                MyBase.Focus()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Continue as not serious
                    Exit Sub
                End If
            Case "Letter"

                If LiveForm = "frmListClaim" Or LiveForm = "frmListClaim_SFB" Then

                    Dim obj_uctClaim As Object 'PM029520 Sumit K
                    'NIIT - Replaced with the Migrated code 1144
                    'm_lReturn = Me.ActiveMdiChild.uctCLMVersions.GetSelectedClaimsDetails(r_lClaimID:=lClaimID, r_lInsuranceFileCnt:=lInsuranceFileCnt)
                    obj_uctClaim = ReflectionHelper.GetMember(Me.ActiveMdiChild, "uctCLMVersions")

                    m_lReturn = obj_uctClaim.GetSelectedClaimsDetails(lClaimID, lInsuranceFileCnt)

                    'NIIT - Replaced with the Migrated code 1144
                    'm_lReturn = Me.ActiveMdiChild.uctCLMVersions.GetSelectedClaimsDetails(r_lClaimID:=lClaimID, r_lInsuranceFileCnt:=lInsuranceFileCnt)

                    For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                        If Application.OpenForms.Item(iLoop1).Name = "frmListClaim" Then
                            ReflectionHelper.Invoke(Application.OpenForms.Item(iLoop1), "GetSelectedClaimsDetails", New Object() {lClaimID, lInsuranceFileCnt, 0, "", ""})
                            lClaimID = ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "ClaimID")
                            lInsuranceFileCnt = ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFileCnt")


                            Exit For
                        End If
                    Next

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to get selected claim id", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If

                    If lClaimID = 0 Then
                        MessageBox.Show("Please select claim from the list", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If


                End If

                ' Call Toolbar Control function
                'Arul- the variable "lInsuranceFileCnt" is replaced with the variable "m_lInsFileCnt"
                m_lReturn = CType(objCM.ProcessToolbar(v_iButton:=objCM.ACIButtonLetter, v_lPartyCnt:=m_lPartyCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsFileCnt, v_lClaimCnt:=lClaimID, v_sShortName:=m_sShortName), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Continue as not serious
                    Exit Sub
                End If
                'eck200700
            Case "Email"

                If LiveForm = "frmListClaim" Or LiveForm = "frmListClaim_SFB" Then


                    'NIIT - Replaced with the Migrated code 1144
                    'm_lReturn = Me.ActiveMdiChild.uctCLMVersions.GetSelectedClaimsDetails(r_lClaimID:=m_lClaimCnt, r_lInsuranceFileCnt:=m_lInsFileCnt, r_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, r_sInsuranceRef:=m_sInsReference, r_sClaimNumber:=m_sClaimNo)
                    ''m_lReturn = ReflectionHelper.Invoke(ReflectionHelper.GetMember(Me.ActiveMdiChild, "uctCLMVersions"), "GetSelectedClaimsDetails", New Object() {m_lClaimCnt, m_lInsFileCnt, m_lInsuranceFolderCnt, m_sInsReference, m_sClaimNo})

                    For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                        If Application.OpenForms.Item(iLoop1).Name = "frmListClaim" Then
                            ReflectionHelper.Invoke(Application.OpenForms.Item(iLoop1), "GetSelectedClaimsDetails", New Object() {lClaimID, lInsuranceFileCnt, m_lInsuranceFolderCnt, m_sInsReference, m_sClaimNo})
                            m_lClaimCnt = ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "ClaimID")
                            m_lInsFileCnt = ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFileCnt")
                            m_lInsuranceFolderCnt = ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsuranceFolderCnt")
                            m_sInsReference = ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "InsReference")
                            m_sClaimNo = ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "ClaimReference")
                            Exit For
                        End If
                    Next



                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to get selected claim id", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If

                    If m_lClaimCnt = 0 Then
                        MessageBox.Show("Please select a claim from the list", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If

                End If

                'Developer Guide No 162
                Dim vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) As Object


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lPartyCnt


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameClientCode

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_sShortName


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameInsFileCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lInsFileCnt


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNamePolicyNumber

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_sInsReference


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameInsFolderCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lInsuranceFolderCnt


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameClaimCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_lClaimCnt


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameClaimNumber

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_sClaimNo


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNameBranchId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = g_iSourceID

                ' Call Toolbar Control function
                m_lReturn = CType(objCM.ProcessToolbar(v_iButton:=ACIButtonEmail, v_lPartyCnt:=m_lPartyCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsFileCnt, v_lClaimCnt:=m_lClaimCnt, v_sShortName:=m_sShortName, v_vUserProp:=vKeyArray), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Continue as not serious
                    Exit Sub
                End If

                ' CTAF 270900
                ' CTAF 091100
            Case "Claim"
                If Not objCM.g_bEditClaimAuthority Then 'PN23013
                    MessageBox.Show("You do not have authority to view claims.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else
                    ' Call Toolbar Control function
                    m_lReturn = CType(objCM.ShowClaimList(v_lPartyCnt:=PartyCnt, v_sShortName:=ShortName, v_sInsReference:=m_sInsReference), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("'Continue as not serious", Application.ProductName)
                        Exit Sub
                    End If
                End If
                'eck131100
            Case "FinancePlan"
                m_lReturn = CType(objCM.ProcessFinancePlanFunction(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sLongname:=m_sResolvedName), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Continue as not serious
                    Exit Sub
                End If

                'DC041203
            Case "iMarket"
                m_lReturn = CType(objCM.ProcessToolbar(v_iButton:=objCM.ACIButtoniMarket), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Continue as not serious
                    Exit Sub
                End If
                '2005 Roadmap sticky-notes
            Case "StickyNote"
                m_lReturn = CType(objCM.ProcessToolbar(v_iButton:=objCM.ACIButtonStickyNote, v_lPartyCnt:=m_lPartyCnt), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Continue as not serious
                    Exit Sub
                End If
                'Start - Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
            Case "CashDeposit"
                m_lReturn = CType(objCM.ProcessToolbar(v_iButton:=objCM.ACIButtonCashDeposit, v_lPartyCnt:=m_lPartyCnt, v_sShortName:=ShortName, v_sResolvedName:=m_sResolvedName), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Continue as not serious
                    Exit Sub
                End If
                'End - Sankar - (WPR85_Cash_Deposit_Process) - Paralleling

        End Select

    End Sub

    ' ***************************************************************** '
    ' Name: RefreshPolicies
    '
    ' Description: Loops through the forms and refreshes each policy
    '              list
    '
    ' ***************************************************************** '
    Public Function RefreshPolicies() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Only attempt to refresh the listpolicy forms
            For Each frm As Form In Application.OpenForms
                If frm.Name = "frmListPolicy" Then

                    'NIIT - Replaced with the Migrated code 1144
                    'm_lReturn = frm.RefreshList()
                    m_lReturn = ReflectionHelper.Invoke(frm, "RefreshList", New Object() {})
                End If
            Next frm

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RefreshClaims
    '
    ' Description: Loops through the forms and refreshes each claim
    '              list
    '
    ' ***************************************************************** '
    Public Function RefreshClaims() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Only attempt to refresh the listpolicy forms
            For Each frm As Form In Application.OpenForms
                If (frm.Name = "frmListClaim") Or (frm.Name = "frmListClaim_SFB") Then

                    'NIIT - Replaced with the Migrated code 1144
                    'm_lReturn = frm.RefreshList()
                    m_lReturn = ReflectionHelper.Invoke(frm, "RefreshList", New Object() {})
                End If
            Next frm

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshCLaims", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'Developer Guide No. 69(Guide)
    'start'
    Public Sub frmMDILoad()
        ' Application starts here (Load event of Startup form).
        'sj 03/07/2002 - start
        If objCM.g_bRestrictInsurerAccess Then
            m_lReturn = CType(objCM.SetRestrictedToolbar(v_sFormName:=Me.Name), gPMConstants.PMEReturnCode)
        Else
            m_lReturn = CType(objCM.SetToolbar(v_sFormName:=Me.Name), gPMConstants.PMEReturnCode)
        End If
        'sj 03/07/2002 - end

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Problem Setting Toolbar keys", Application.ProductName)
        End If

        'Show()
        ' Always set the working directory to the directory containing the application.
        Directory.SetCurrentDirectory(My.Application.Info.DirectoryPath)
        ' Initialize the document form array, and show the first document.
        'WhaTT?
        '    Set Document = CreateObject("frmPartyPC.frm")
        ReDim objCM.Document(1)
        'WhaTT?
        ReDim objCM.FState(1)
        objCM.FState(1).Deleted = True
        '    Document(1).Tag = 1
        objCM.FState(1).Dirty = False
        ' Read System registry and set the recent menu file list control array appropriately.
        'GetRecentFiles
        ' CTAF 170801 - Use objCM.LoadRecentFiles
        m_lReturn = CType(objCM.LoadRecentFilesFromReg(), gPMConstants.PMEReturnCode)

        ' Set public variable gFindDirection which determines which direction
        ' the FindIt function will search in.
        objCM.gFindDirection = 1
    End Sub 'end'
End Class
