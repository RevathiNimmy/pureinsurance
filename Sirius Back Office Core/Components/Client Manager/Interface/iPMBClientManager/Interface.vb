Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    '
    ' History:
    ' CJB 090805 PN23035 Changed Start & CheckSecurity to also get value for Client Manager Security option
    '            for Delete Policy.
    '
    ' *****************************************************************
    '
    ' Name: Interface.cls
    ' Desc: Creatable class for object.
    '
    ' *****************************************************************

    Private objCM As MainModule

    Private Const ACClass As String = "Interface"

    ' Private variables
    Private m_sPartyResolvedName As String = ""
    Private m_lPartyCnt As Integer
    Private m_sPartyShortName As String = ""
    Private m_sPartyType As String = ""

    'JT PN-13238 01-11-2004
    Private m_bIsIncludeClosedBranchChecked As Boolean
    'PN26814 Declared withevents to handle to NavigatorClose event
    Private WithEvents m_objPMNavStart As iPMNavStart.Interface_Renamed
    Private m_frmMDI As frmMDI
    Public ReadOnly Property MDIForm() As Form
        Get
            Return m_frmMDI
        End Get
    End Property

    Private m_lReturn As Integer
    'sj 03/07/2002 - start
    Public WriteOnly Property RestrictInsurerAccess() As Boolean
        Set(ByVal Value As Boolean)
            objCM.g_bRestrictInsurerAccess = Value
        End Set
    End Property
    Public WriteOnly Property UserInsurerCnt() As Integer
        Set(ByVal Value As Integer)
            objCM.g_lUserInsurerCnt = Value
        End Set
    End Property
    'sj 03/07/2002 - end
    ' *************** PUBLIC PROPERTIES (BEGIN)************************

    Public Property PartyResolvedName() As String
        Get
            Return m_sPartyResolvedName
        End Get
        Set(ByVal Value As String)
            m_sPartyResolvedName = Value
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

    Public Property PartyShortName() As String
        Get
            Return m_sPartyShortName
        End Get
        Set(ByVal Value As String)
            m_sPartyShortName = Value
            'TODO
            objCM.m_sShortName = Value
        End Set
    End Property
    Public Property PartyType() As String
        Get
            Return m_sPartyType
        End Get
        Set(ByVal Value As String)
            m_sPartyType = Value
        End Set
    End Property
    'JT PN-13238 01-11-2004 To hold that whether the CheckBox of Include Closed Branch
    'was checked or not in FindParty
    Public Property IsIncludeClosedBranchChecked() As Boolean
        Get
            Return m_bIsIncludeClosedBranchChecked
        End Get
        Set(ByVal Value As Boolean)
            m_bIsIncludeClosedBranchChecked = Value
        End Set
    End Property

    ' *************** PUBLIC PROPERTIES (END)**************************

    ' *************** PUBLIC FUNCTIONS (BEGIN)*************************

    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Static bLoaded As Boolean
        Dim vValue As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' PW220502 - move the code to get the underwriting/agency flag
            ' to before the load frmMDI. The frmMDI load invokes the load
            ' of the relevant child form which requires the u/a flag in order
            ' to set up it's menus correctly.

            'Thinh Nguyen 15/04/2002 (start) - get hidden option
            If GetSystemMode(r_sValue:=objCM.g_sUnderwritingOrAgency) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get system option", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")

                Return result
            End If
            'Thinh Nguyen 15/04/2002 (end) - get hidden option

            'sj 04/10/2002 - start
            ' Get the product option for hiding public/private notes

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTHidePublicPrivateNotes, v_vBranch:=objCM.g_oObjectManager.SourceID, r_vUnderwriting:=vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for hide public/private notes", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'AK 20021110 - It has been agreed with MR that these buttons will be disabled permanently
            '              If we need to upgrade existing Notes from Broking customers, the data can be upgraded
            '              to convert 'Notes' event type to 'Notes - Customer', as agreed with SP

            'DC081103 PN8966 -visible again
            objCM.g_bHidePublicPrivateNotes = False

            '    If Val(vValue) = 1 Then
            '        objCM.g_bHidePublicPrivateNotes = True
            '    Else
            '        objCM.g_bHidePublicPrivateNotes = False
            '    End If
            'sj 04/10/2002 - end

            If Not bLoaded Then
                ' Load the MDI form so that we can open the summary
                'Dim tempLoadForm As frmMDI = frmMDI
                'Developer Guide No. 69(Guide)
                'added code start'
                m_frmMDI = New frmMDI
                objCM.m_ofrmMDI = m_frmMDI
                m_frmMDI.ModuleClass = objCM
                m_frmMDI.frmMDILoad()
                'End'
                bLoaded = True
            End If

            '2005 Client Manager Security
            '14/10/2005 New option for datasure
            m_lReturn = CheckSecurity(r_bEditClientAuthority:=objCM.g_bEditClientAuthority, r_bEditPolicyAuthority:=objCM.g_bEditPolicyAuthority, r_bEditClaimAuthority:=objCM.g_bEditClaimAuthority, r_bEditFinancePlanAuthority:=objCM.g_bEditFinancePlanAuthority, r_bRaiseDebitAuthority:=objCM.g_bRaiseDebitAuthority, r_bRaiseCreditAuthority:=objCM.g_bRaiseCreditAuthority, r_bRaiseFeeAuthority:=objCM.g_bRaiseFeeAuthority, r_bRaiseCashAuthority:=objCM.g_bRaiseCashAuthority, r_bReverseTransactionsAuthority:=objCM.g_bReverseTransactionsAuthority, r_bReverseAllocationsAuthority:=objCM.g_bReverseAllocationsAuthority, r_bRaiseManualDIDAuthority:=objCM.g_bRaiseManualDIDAuthority, r_bDeletePolicyAuthority:=objCM.g_bDeletePolicyAuthority, r_bEditSchemePolicyAuthority:=objCM.g_bEditSchemePolicyAuthority)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If m_lPartyCnt = 0 Then
                m_lReturn = objCM.OpenFile(vPartyCnt:=PartyCnt, vPartyShortName:=PartyShortName, vPartyType:=PartyType, vPartyResolvedName:=PartyResolvedName)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to open detail file for new party", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")

                    Return result

                End If
            Else
                ' Open the summary information for the passed file
                'JT PN-13238
                'passed optional Parameter for Include ChkBox
                objCM.m_frmParentMdiForm = m_frmMDI
                m_lReturn = objCM.OpenSummaryFile(vPartyCnt:=PartyCnt, vPartyShortName:=PartyShortName, vPartyType:=PartyType, vPartyResolvedName:=PartyResolvedName, bIsIncludeClosedBranchchecked:=IsIncludeClosedBranchChecked)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to open summary file for " & m_sPartyResolvedName, vApp:=ACApp, vClass:=ACClass, vMethod:="Start")

                    Return result

                End If
            End If

            'PN26814 Create an instance of the iPMNavStart.Interface object and pass a reference
            'to the MainModule module for use in calling NavXM processes
            m_objPMNavStart = New iPMNavStart.Interface_Renamed()
            'MainModule.PMNavStart = m_objPMNavStart
            objCM.PMNavStart = m_objPMNavStart

            ' Display the form
            m_frmMDI.Show()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Sub InitaliseModuleClass()
        objCM = New MainModule
    End Sub


    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef r_oCMManager As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            ' Create an instance of the object manager.
            objCM.g_oObjectManager = New bObjectManager.ObjectManager()

            ' Store an instance of client manager manager
            objCM.g_oCMManager = r_oCMManager

            ' Call the initialise method.
            m_lReturn = objCM.g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                objCM.g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With objCM.g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                objCM.g_iCurrencyId = .CurrencyID
                objCM.g_iUserId = .UserID 'MKW070703 PN4026 Retrieve current userid.
                objCM.g_iCountryID = .CountryID 'eck Datasure
                objCM.g_sUserName = .UserName
            End With

            m_lReturn = InitialiseOrionLinkFunc()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Developer Guide No. 107
            PartyBuilderHandler.g_oObjectManager = objCM.g_oObjectManager
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If objCM.g_oEvent IsNot Nothing Then
                    objCM.g_oEvent.Dispose()
                    objCM.g_oEvent = Nothing
                End If
                If objCM.g_oObjectManager IsNot Nothing Then
                    objCM.g_oObjectManager.Dispose()
                    objCM.g_oObjectManager = Nothing
                End If
                TerminateOrionLinkFunc()
                m_objPMNavStart = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SwitchTo
    '
    ' Description: Switches the focus to this form. Called from the
    '              Client Manager Manager.
    '
    ' ***************************************************************** '
    Public Function SwitchTo() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_frmMDI.Visible Then

                ' Resize the window
                m_frmMDI.WindowState = FormWindowState.Maximized

                ' Set the focus to it
                m_frmMDI.Activate()

            End If

            Return result

        Catch excep As System.Exception



            If Information.Err().Number = 5 Then
                ' Alix - 24/11/2003 - Issue PN7468
                ' This can happen if we try to open a client which is already open, with
                ' a sub-screen open (like accounts). It tries to set the focus to that client
                ' screen, but fails because of the sub-screen. We can't do much to avoid this,
                ' but at least let's not disturb the user with dodgy error messages!
            Else

                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SwitchTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SwitchTo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ShowPolicyEdit
    '
    ' Description: Shows a policy in edit mode (for after a copy)
    '
    ' History: 15/08/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function ShowPolicyEdit(ByVal v_lPartyCnt As Integer, ByVal v_sPartyType As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsFileCnt As Integer, ByVal v_sShortName As String, ByVal v_sInsReference As String, ByVal v_lPolicyTypeId As Integer, Optional ByVal v_bCopiedPolicy As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Show the policy to edit it

            m_lReturn = CInt(objCM.ShowPolicyDetail(v_lPartyCnt:=v_lPartyCnt, v_sPartyType:=v_sPartyType, v_lInsuranceFolderCnt:=0, v_lInsFileCnt:=v_lInsFileCnt, v_sShortName:=v_sShortName, v_sInsReference:="", v_lInsuranceFileStructureId:=0, v_bFromEvent:=False, v_lPolicyTypeId:=0, v_vGeminiPolicyStatus:=0, v_bCopiedPolicy:=v_bCopiedPolicy))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowPolicyEdit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicyEdit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowPolicy
    '
    ' Description: Needed for an entry point so the policy can be shown
    '              when coming from a work manager task
    '
    ' MSS040701 - Created
    ' ***************************************************************** '
    Public Function ShowPolicy(ByVal v_lPartyCnt As Integer, ByVal v_sPartyType As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsFileCnt As Integer, ByVal v_sShortName As String, ByVal v_sInsReference As String, ByVal v_lPolicyTypeId As Integer) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try


            m_lReturn = CInt(objCM.ShowPolicySummary(v_lPartyCnt:=v_lPartyCnt, v_sPartyType:=v_sPartyType, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsFileCnt:=v_lInsFileCnt, v_sShortName:=v_sShortName, v_sInsReference:=v_sInsReference, v_lPolicyTypeId:=v_lPolicyTypeId))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *************** PUBLIC FUNCTIONS (END)***************************
    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub
    ' ***************************************************************** '
    ' Name: CheckSecurity (Standard Method)
    '
    ' Description: Check whether the user has authority to view clients
    ' History:     2005 Client Security  20/04/2005
    '               14/10/2005 Added nwe option for Datasure EditSchemePolicy
    ' ***************************************************************** '
    Private Function CheckSecurity(ByRef r_bEditClientAuthority As Boolean, ByRef r_bEditPolicyAuthority As Boolean, ByRef r_bEditClaimAuthority As Boolean, ByRef r_bEditFinancePlanAuthority As Boolean, ByRef r_bRaiseDebitAuthority As Boolean, ByRef r_bRaiseCreditAuthority As Boolean, ByRef r_bRaiseFeeAuthority As Boolean, ByRef r_bRaiseCashAuthority As Boolean, ByRef r_bReverseTransactionsAuthority As Boolean, ByRef r_bReverseAllocationsAuthority As Boolean, ByRef r_bRaiseManualDIDAuthority As Boolean, ByRef r_bDeletePolicyAuthority As Boolean, ByRef r_bEditSchemePolicyAuthority As Boolean) As Integer

        Dim result As Integer = 0


        Dim sValue As String = ""
        Dim iIsEditClient, iIsEditPolicy, iIsDeletePolicy, iIsEditClaim, iIsEditFinancePlan, iIsRaiseDebit, iIsRaiseCredit, iIsRaiseFee, iIsRaiseCash, iIsReverseTransactions, iIsReverseAllocations, iIsRaiseManualDID, iIsEditSchemePolicy As Integer

        result = gPMConstants.PMEReturnCode.PMTrue


        r_bEditClientAuthority = True
        r_bEditPolicyAuthority = True
        r_bDeletePolicyAuthority = True
        r_bEditClaimAuthority = True
        r_bEditFinancePlanAuthority = True
        r_bRaiseDebitAuthority = True
        r_bRaiseCreditAuthority = True
        r_bRaiseFeeAuthority = True
        r_bRaiseCashAuthority = True
        r_bReverseTransactionsAuthority = True
        r_bReverseAllocationsAuthority = True
        r_bRaiseManualDIDAuthority = True
        r_bEditSchemePolicyAuthority = True

        Dim temp_g_oUserAuthorities As Object
        m_lReturn = objCM.g_oObjectManager.GetInstance(temp_g_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        objCM.g_oUserAuthorities = temp_g_oUserAuthorities
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTUserAuthorities.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckSecurity", excep:=New Exception(Information.Err().Description))
            Return result
        End If

        'Party View
        If sValue <> "1" Then

            m_lReturn = objCM.g_oUserAuthorities.GetPartyViewOptions(v_lUserId:=objCM.g_iUserId, r_bIsViewOnlyClientManager:=objCM.g_bIsViewOnlyClientManager)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get GetPartyViewOptions", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckSecurity", excep:=New Exception(Information.Err().Description))
                Return result
            End If

            If Not (objCM.g_oUserAuthorities Is Nothing) Then

                objCM.g_oUserAuthorities.Dispose()
                objCM.g_oUserAuthorities = Nothing
            End If
            Return result
        End If



        m_lReturn = objCM.g_oUserAuthorities.GetDetails(vUserID:=objCM.g_iUserId)


        m_lReturn = objCM.g_oUserAuthorities.GetNext(vIsEditClient:=iIsEditClient, vIsEditPolicy:=iIsEditPolicy, vIsEditClaim:=iIsEditClaim, vIsEditFinancePlan:=iIsEditFinancePlan, vIsRaiseDebit:=iIsRaiseDebit, vIsRaiseCredit:=iIsRaiseCredit, vIsRaiseFee:=iIsRaiseFee, vIsRaiseCash:=iIsRaiseCash, vIsReverseTransactions:=iIsReverseTransactions, vIsReverseAllocations:=iIsReverseAllocations, vIsRaiseManualDID:=iIsRaiseManualDID, vIsDeletePolicy:=iIsDeletePolicy, vIsEditSchemePolicy:=iIsEditSchemePolicy)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Security Settings for User", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckSecurity", excep:=New Exception(Information.Err().Description))
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            r_bEditClientAuthority = False
            r_bEditPolicyAuthority = False
            r_bDeletePolicyAuthority = False
            r_bEditClaimAuthority = False
            r_bEditFinancePlanAuthority = False
            r_bRaiseDebitAuthority = False
            r_bRaiseCreditAuthority = False
            r_bRaiseFeeAuthority = False
            r_bRaiseCashAuthority = False
            r_bReverseTransactionsAuthority = False
            r_bReverseAllocationsAuthority = False
            r_bRaiseManualDIDAuthority = False
            r_bEditSchemePolicyAuthority = False

            Return result
        End If

        r_bEditClientAuthority = iIsEditClient = 1

        r_bEditPolicyAuthority = iIsEditPolicy = 1

        r_bDeletePolicyAuthority = iIsDeletePolicy = 1

        r_bEditClaimAuthority = iIsEditClaim = 1

        r_bEditFinancePlanAuthority = iIsEditFinancePlan = 1

        r_bRaiseDebitAuthority = iIsRaiseDebit = 1

        r_bRaiseCreditAuthority = iIsRaiseCredit = 1

        r_bRaiseFeeAuthority = iIsRaiseFee = 1

        r_bRaiseCashAuthority = iIsRaiseCash = 1

        r_bReverseTransactionsAuthority = iIsReverseTransactions = 1

        r_bReverseAllocationsAuthority = iIsReverseAllocations = 1

        r_bRaiseManualDIDAuthority = iIsRaiseManualDID = 1

        r_bEditSchemePolicyAuthority = iIsEditSchemePolicy = 1

        If Not (objCM.g_oUserAuthorities Is Nothing) Then

            objCM.g_oUserAuthorities.Dispose()
            objCM.g_oUserAuthorities = Nothing
        End If

        Return result

    End Function


    '*****************************************************
    ' Name : GetSystemMode
    '
    ' Desc : Get system option
    '
    ' Hist : 15/04/2002 (created) Thinh Nguyen
    '*****************************************************
    Private Function GetSystemMode(ByRef r_sValue As String) As Integer
        Dim result As Integer = 0
        Dim bSIRFindParty As Object


        Dim oObject As bSIRFindParty.Business



        result = gPMConstants.PMEReturnCode.PMTrue

        If objCM.g_oObjectManager Is Nothing Then
            Initialise(objCM.g_oCMManager)
        End If

        Dim temp_oObject As Object
        m_lReturn = objCM.g_oObjectManager.GetInstance(temp_oObject, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oObject = temp_oObject

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bSIRFindParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemMode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result
        End If



        r_sValue = oObject.UnderwritingOrAgency


        oObject.Dispose()

        oObject = Nothing

        Return result

    End Function

    Private Sub m_objPMNavStart_NavigatorClose() Handles m_objPMNavStart.NavigatorClose
        'PN26814 Call the Terminate method to release the NavXM process
        m_objPMNavStart.Dispose()
        'Reset the NavXM running flag
        objCM.IsNavStartRunning = False
    End Sub

    '*******************************************************************************************
    ' Name : ShowPolicyList
    '
    ' Desc : so we call launch this from another app
    '
    ' Auth : Thinh Nguyen 14/01/2004
    '*******************************************************************************************
    Public Function ShowPolicyList(ByVal v_lPartyCnt As Integer, ByVal v_sPartyShortName As String) As Integer
        m_lReturn = objCM.ShowPolicy(v_lPartyCnt:=v_lPartyCnt, v_sShortName:=v_sPartyShortName)
    End Function
End Class

