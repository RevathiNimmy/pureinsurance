Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
' developer guide no 129
Imports SharedFiles
Imports iPMWrkComponentStarter
<System.Runtime.InteropServices.ProgId("uctListPolicy_NET.uctListPolicy")>
Partial Public Class uctListPolicy
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event EffectiveDateChange()
    Public Event TransactionTypeChange()
    Public Event ProcessModeChange()
    Public Event StatusChange()
    Public Event TaskChange()
    Public Event CallingAppNameChange()
    '*******************************************************************************
    ' Date: 07/10/1998
    '
    ' Description: List Policy User Control
    '
    ' Edit History: TF071098 - Created from iFindInsurance
    '
    '               PW031204 - replicate SJ changes from 05/07/04 which fix a bug.
    '               Comment out his changes of 18/10/04 as these relate to a version
    '               of Gemini which has not been released yet and break Broking.
    '               CJB180205 PN18878 Cater for NULL Status values when filtering by POLICY.
    '*******************************************************************************

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "uctListPolicyControl"

    'Default Property Values:
    Const m_def_BackColor As Integer = 0
    Const m_def_ForeColor As Integer = 0
    Const m_def_Enabled As Integer = 0
    Const m_def_BackStyle As Integer = 0
    Const m_def_BorderStyle As Integer = 0
    Const m_def_PartyCnt As Integer = 0
    'Property Variables:
    Dim m_BackColor As Integer
    Dim m_ForeColor As Integer
    Dim m_Enabled As Boolean
    Dim m_Font As Font
    Dim m_BackStyle As Integer
    Dim m_BorderStyle As Integer
    Dim m_PartyCnt As Integer
    'Event Declarations:
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs)
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs)
    Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs)
    Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs)
    Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs)
    Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs)
    Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs)
    Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs)
    Event lvwSearchDetailsDblClick(ByVal Sender As Object, ByVal e As lvwSearchDetailsDblClickEventArgs)
    'SJ 22/07/2004 - start
    Event PolicyListRefreshed(ByVal Sender As Object, ByVal e As PolicyListRefreshedEventArgs)
    'SJ 22/07/2004 - end

    'TN20010420 Start
    Dim m_lSelected As gPMConstants.PMEReturnCode
    Event lvwSearchDetailsMouseDown(ByVal Sender As Object, ByVal e As lvwSearchDetailsMouseDownEventArgs)
    'TN20010420 End

    Event cboStatusChange(ByVal Sender As Object, ByVal e As EventArgs)

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    'TN20001214 - Start
    Private m_sUnderwritingOrAgency As String = ""
    'TN20001214 - End

    Private m_lInsFileCnt As Integer
    Private m_sInsReference As String = ""
    Private m_lInsHolderCnt As Integer
    Private m_sShortName As String = "" 'JW190498
    Private m_sLongName As String = "" 'JW190498
    Private m_lInsuranceFolderCnt As Integer 'TF100398
    Private m_sRegistration As String = "" 'Tom 031198
    Private m_lProductId As Integer

    'TF211298
    Private m_lPartyUIK As Integer
    Private m_lPolicyUIK As Integer
    Private m_vLeadAgentCnt As String = ""
    'Tomo150300
    Private m_lPolicyTypeId As Integer
    Private m_sPolicyType As String = ""

    ' TF311298 - changed from NavProcessCode
    Private m_sInsFileType As String = ""
    'sj 5/11/99 - start
    Private m_bDisableInsFileType As Boolean
    'sj 5/11/99 - end
    'eck090500
    Private m_vSourceArray(,) As Object
    ' {* USER DEFINED CODE (End) *}


    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    'Private m_oBusiness As bSIRFindInsurance.Form

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    Private m_lItemsFound As gPMConstants.PMEFormatStyle
    Private m_lLifeItemsFound As Integer
    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the search data from the business object.
    Public m_vSearchData(,) As Object
    Public m_vSingleSearchData(,) As Object
    Public m_bCopyPolicy As Boolean
    Public m_bOKClick As Boolean
    Private m_oOption As Object

    Private m_bGeminiLink As Boolean
    Private m_bGeminiIILink As Boolean
    Private m_bSwiftLink As Boolean

    Private m_lFindType As Integer
    Private m_lPolicyType As Integer
    Private m_lInsuranceFileTypeId As Integer
    Private m_vGeminiPolicyStatus As String = "" 'JSB 08/06/01

    Private m_lNewInsuranceFileCnt As Integer

    ' CTAF 140801
    Private m_bCopiedElsewhere As Boolean
    ' CTAF 150801
    Private m_lTargetPartyCnt As Integer

    ' TargetShortName
    Private m_sTargetShortName As String = ""

    ' TargetResolvedName
    Private m_sTargetResolvedName As String = ""

    ' TargetLongName
    Private m_sTargetLongName As String = ""

    ' TargetPartyType
    Private m_sTargetPartyType As String = ""

    'sj 03/07/2002 - start
    ' UserInsurerCnt
    Private m_lUserInsurerCnt As Integer

    'SJ 18/02/2004 - start
    Private m_bUnderwritingBranchEnabled As Boolean
    Private m_bIsUnderwritingBranch As Boolean
    Private m_vAlternateReference As Object
    'SJ 18/02/2004 - end

    'SJ 08/04/2004 - start
    Private m_oAllowBranches As Hashtable
    Private m_oValidSource As Hashtable
    'SJ 08/04/2004 - end

    Private m_bResizing As Boolean
    Private m_sPolicyStatus As String = ""

    'WPR12- Enhancement Quote Collection Process
    Private m_bDontProceedMarkedForCollection As Boolean

    'Bug TFS 5868
    Private m_ItemSelected As Boolean
    <Browsable(False)>
    Public ReadOnly Property ItemSelected() As Boolean
        Get
            Return m_ItemSelected
        End Get
    End Property

    'SJ 20/02/2004 - start
    <Browsable(False)>
    Public ReadOnly Property UnderwritingBranchEnabled() As Boolean
        Get
            Return m_bUnderwritingBranchEnabled
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property IsUnderwritingBranch() As Boolean
        Get
            Return m_bIsUnderwritingBranch
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property AlternateReference() As Object
        Get
            Return m_vAlternateReference
        End Get
    End Property
    'SJ 20/02/2004 - end


    <Browsable(False)>
    Public WriteOnly Property UserInsurerCnt() As Integer
        Set(ByVal Value As Integer)
            m_lUserInsurerCnt = Value
        End Set
    End Property
    'sj 03/07/2002 - end

    'sj 30/08/2002 - start
    <Browsable(False)>
    Public ReadOnly Property ItemsFound() As Integer
        Get

            Dim result As Integer = 0
            Dim vSearchData(,) As Object



            m_lReturn = m_oBusiness.GetAllPolicyVersion(r_vResultArray:=vSearchData, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsFileCnt)
            If Not Information.IsArray(vSearchData) Then
                Return 0
            Else

                Return vSearchData.GetUpperBound(1) + 1
            End If

            Return result
        End Get
    End Property 'sj 30/08/2002 - end
    'eck 100903 PN6647
    <Browsable(False)>
    Public ReadOnly Property PolicyCount() As Integer
        Get
            Return lvwSearchDetails.Items.Count
        End Get
    End Property

    <Browsable(True)>
    Public Property TargetPartyType() As String
        Get
            Return m_sTargetPartyType
        End Get
        Set(ByVal Value As String)
            m_sTargetPartyType = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property TargetLongName() As String
        Get
            Return m_sTargetLongName
        End Get
        Set(ByVal Value As String)
            m_sTargetLongName = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property TargetShortName() As String
        Get
            Return m_sTargetShortName
        End Get
        Set(ByVal Value As String)
            m_sTargetShortName = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property TargetResolvedName() As String
        Get
            Return m_sTargetResolvedName
        End Get
        Set(ByVal Value As String)
            m_sTargetResolvedName = Value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property TargetPartyCnt() As Integer
        Get
            Return m_lTargetPartyCnt
        End Get
    End Property

    ' CTAF 140801
    <Browsable(False)>
    Public ReadOnly Property CopiedElsewhere() As Boolean
        Get
            Return m_bCopiedElsewhere
        End Get
    End Property


    <Browsable(False)>
    Public Property NewInsuranceFileCnt() As Integer
        Get
            Return m_lNewInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lNewInsuranceFileCnt = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property PolicyType() As Integer
        Set(ByVal Value As Integer)
            m_lPolicyType = Value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property InsuranceFileTypeId() As Integer
        Get
            Return m_lInsuranceFileTypeId
        End Get
    End Property


    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' CF 020799
    <Browsable(False)>
    Public ReadOnly Property Controls_Renamed() As Object
        Get
            Return Me.Controls_Renamed
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    <Browsable(False)>
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value
            RaiseEvent CallingAppNameChange()

        End Set
    End Property

    <Browsable(True)>
    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value
            RaiseEvent TaskChange()

        End Set
    End Property

    <Browsable(True)>
    Public Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the interface exit status.
            m_lStatus = Value
            RaiseEvent StatusChange()

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value
            RaiseEvent ProcessModeChange()

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property InsFileType() As String
        Set(ByVal Value As String)

            m_sInsFileType = Value

        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property DisableInsFileType() As Boolean
        Set(ByVal Value As Boolean)
            m_bDisableInsFileType = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value
            RaiseEvent TransactionTypeChange()

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value
            RaiseEvent EffectiveDateChange()

        End Set
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    <Browsable(False)>
    Public ReadOnly Property InsFileCnt() As Integer
        Get

            Return m_lInsFileCnt

        End Get
    End Property

    <Browsable(True)>
    Public Property InsReference() As String
        Get

            Return m_sInsReference

        End Get
        Set(ByVal Value As String)

            m_sInsReference = Value

        End Set
    End Property

    <Browsable(True)>
    Public Property InsHolderCnt() As Integer
        Get

            Return m_lInsHolderCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsHolderCnt = Value

        End Set
    End Property

    <Browsable(True)>
    Public Property ShortName() As String
        Get

            Return m_sShortName

        End Get
        Set(ByVal Value As String)

            m_sShortName = Value

        End Set
    End Property

    'TF100398
    <Browsable(False)>
    Public ReadOnly Property InsuranceFolderCnt() As Integer
        Get

            Return m_lInsuranceFolderCnt

        End Get
    End Property

    <Browsable(False)>
    Public WriteOnly Property FindType() As Integer
        Set(ByVal Value As Integer)

            m_lFindType = Value

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property ProductId() As Integer
        Set(ByVal Value As Integer)

            m_lProductId = Value

        End Set
    End Property

    'TN20010420 Start
    <Browsable(False)>
    Public ReadOnly Property Selected() As Integer
        Get
            Return m_lSelected
        End Get
    End Property
    'TN20010420 End

    'JSB 08/06/01 - Start Added public properties that are required to call Gemini II

    <Browsable(False)>
    Public ReadOnly Property GeminiPolicyStatus() As String
        Get

            Return m_vGeminiPolicyStatus

        End Get
    End Property


    <Browsable(False)>
    Public ReadOnly Property SelectedPolicyStatus() As String
        Get
            Return m_sPolicyStatus
        End Get
    End Property

    'JSB 08/06/01 - End

    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)

    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    'TN20010420 Start
    Public Sub lvwSearchDetailsSetFocus()
        lvwSearchDetails.Focus()
    End Sub
    'TN20010420 End

    ' ***************************************************************** '
    ' Name: GetPolicies
    '
    ' Description: Gets the interface details and sets the appropriate
    '              style.
    '
    ' ***************************************************************** '
    Public Function GetPolicies() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.pmtrue

            ' Get the interface details from the business object.
            m_lReturn = GetBusiness()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.pmtrue Then
                ' Failed to get the details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details from the search data storage
            ' to the interface.
            'm_lReturn& = DataToInterface()

            ' Check for errors
            'If (m_lReturn& <> PMTrue) Then
            '    ' Failed to assign the details.
            '    GetPolicies = PMFalse
            '    Exit Function
            'End If

            'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
            'Start (Sriram P)53582
            If lvwSearchDetails.Items.Count > 0 Then
                'developer guide no. after binding lvwSearchDetails.FocusedItem set to nothing.
                If Not (lvwSearchDetails.FocusedItem Is Nothing) Then
                    lvwSearchDetails.FocusedItem.Selected = False
                End If
            End If
            'End (Sriram P)53582
            'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the Policies", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicies", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' History    : Kevin Renshaw (CMG) 1708 check file type and if 2 - initial Key value
    '               for Cancel Policy pass in file type of 'POLICY'.
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim sInsRef, sInsFileType As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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

            sInsRef = "%"

            'TF020902 - Only set default if no other passed in
            If m_sInsFileType = "" Then
                sInsFileType = "%"
            Else
                Select Case m_sInsFileType
                    Case CStr(2)
                        sInsFileType = "POLICY"
                    Case Else
                        sInsFileType = m_sInsFileType
                End Select
            End If

            ' CF11088 - Added this so after a double click, GetPolicies works.
            m_lInsFileCnt = 0

            'SJ 05/07/2004 - start
            If m_oValidSource Is Nothing Then
                m_lReturn = GetValidSources()
                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If m_oAllowBranches Is Nothing Then
                m_lReturn = AllowOtherBranchesToViewPolicies()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AllowOtherBranchesToViewPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
                    Return result
                End If
            End If
            'SJ 05/07/2004 - end

            '070699 ECK


            Select Case m_lFindType
                Case 0

                    'Gaurav

                    m_lReturn = m_oBusiness.SearchAll(r_vResultArray:=m_vSearchData, v_vInsuranceRef:="", v_vInsFileType:=sInsFileType, v_vShortName:=txtClientCode.Text.Trim(), v_vPartyCnt:=m_lInsHolderCnt, v_vUserInsurerCnt:=m_lUserInsurerCnt)
                Case 1


                    m_lReturn = m_oBusiness.SearchAllGIIM(r_vResultArray:=m_vSearchData, v_lPartyCnt:=m_lInsHolderCnt, v_lPolicyTypeId:=m_lPolicyType)

                    ' BSJ 09/06/2000 - Added case for MTA's
                Case 2


                    m_lReturn = m_oBusiness.SearchMTAGIIM(r_vResultArray:=m_vSearchData, v_lPartyCnt:=m_lInsHolderCnt, v_lPolicyTypeId:=m_lPolicyType)

                Case 3


                    m_lReturn = m_oBusiness.SearchAllByProductId(r_vResultArray:=m_vSearchData, v_lPartyCnt:=m_lInsHolderCnt, v_lProductId:=m_lProductId)

                Case 4


                    m_lReturn = m_oBusiness.SearchAllPMUQuotes(r_vResultArray:=m_vSearchData, v_lPartyCnt:=m_lInsHolderCnt)

                Case 5

                    '        m_lReturn& = m_oBusiness.SearchAllPMUMTAs(r_vResultArray:=m_vSearchData, _
                    'v_lPartyCnt:=m_lInsHolderCnt)

                    m_lReturn = m_oBusiness.SearchAllPMUEditable(r_vResultArray:=m_vSearchData, v_lPartyCnt:=m_lInsHolderCnt)

                Case 6


                    m_lReturn = m_oBusiness.SearchAllPMUEditable(r_vResultArray:=m_vSearchData, v_lPartyCnt:=m_lInsHolderCnt)

                    ' BSJ 05/07/01 - Added new search - for all GIIM live policies
                Case 7


                    m_lReturn = m_oBusiness.SearchAllGIIMPOLICY(r_vResultArray:=m_vSearchData, v_lPartyCnt:=m_lInsHolderCnt, v_lPolicyTypeId:=m_lPolicyType)

            End Select

            m_lReturn = DataToInterface()

            ' {* USER DEFINED CODE (End) *}

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
                    gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
            End Select

            ' Display the number of item found message.
            DisplayStatusFound()

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", excep:=excep)

            Return result



            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    ' Tomo010200 - Include policy types, and include the code to
    '              populate the life stuff, not that it'll ever be used
    ' Kevin Renshaw (CMG) - 6/3/2003 - added check on nullable field before Trim
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim bMatch, bSearchSet, Repeatedata As Boolean
        Dim sString As String = ""

        Dim lColumnPolicyNumber, lColumnPolicyType, lColumnRiskType, lColumnRegarding, lColumnRenewalDate, lColumnInsurer, lColumnPremium, lColumnPolicyStatus, lColumnRiskTypeDescription, lColumnGeminiEDI, lColumnEventDescription As Integer 'Gaurav changed
        Dim bUseAlternateReference As Boolean
        'SJ 20/02/2004 - start
        Dim sInsuranceReference, sAlternateReference As String
        'SJ 20/02/2004 - end
        ''SJ 18/10/2004 - start
        Dim lColumnStoredInd As Integer
        ''SJ 18/10/2004 - end

        Const ACFindImage As String = "FindImage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sUnderwritingOrAgency = "A" Then
                'SJ 18/10/2004 - start
                If m_lFindType = 1 Then
                    'Used for Gemini II New Business
                    lColumnPolicyNumber = ACGColumnPolicyNumber - 1
                    lColumnStoredInd = ACGColumnStoredInd - 1
                    lColumnPolicyType = ACGColumnPolicyType - 1
                    lColumnRiskType = ACGColumnRiskType - 1
                    lColumnRegarding = ACGColumnRegarding - 1
                    lColumnRenewalDate = ACGColumnRenewalDate - 1
                    lColumnInsurer = ACGColumnInsurer - 1
                    lColumnPremium = ACGColumnPremium - 1
                    lColumnPolicyStatus = ACGColumnPolicyStatus - 1
                    lColumnRiskTypeDescription = ACGColumnRiskTypeDescription - 1
                    lColumnGeminiEDI = ACGColumnGeminiEDI - 1
                    lColumnEventDescription = ACGColumnEventDescription - 1 'Gaurav changed
                Else
                    'SJ 18/10/2004 - end
                    lColumnPolicyNumber = ACAColumnPolicyNumber - 1
                    lColumnPolicyType = ACAColumnPolicyType - 1
                    lColumnRiskType = ACAColumnRiskType - 1
                    lColumnRegarding = ACAColumnRegarding - 1
                    lColumnRenewalDate = ACAColumnRenewalDate - 1
                    lColumnInsurer = ACAColumnInsurer - 1
                    lColumnPremium = ACAColumnPremium - 1
                    lColumnPolicyStatus = ACAColumnPolicyStatus - 1
                    lColumnRiskTypeDescription = ACAColumnRiskTypeDescription - 1
                    lColumnGeminiEDI = ACAColumnGeminiEDI - 1
                    lColumnEventDescription = ACAColumnEventDescription - 1 'Gaurav changed
                End If
            Else
                lColumnPolicyNumber = ACUColumnPolicyNumber - 1
                lColumnPolicyType = ACUColumnPolicyType - 1
                lColumnRiskType = ACUColumnRiskType - 1
                lColumnRegarding = ACUColumnRegarding - 1
                lColumnRenewalDate = ACUColumnRenewalDate - 1
                lColumnInsurer = ACUColumnInsurer - 1
                lColumnPremium = ACUColumnPremium - 1
                lColumnPolicyStatus = ACUColumnPolicyStatus - 1
                lColumnRiskTypeDescription = ACUColumnRiskTypeDescription - 1
                lColumnGeminiEDI = ACUColumnGeminiEDI - 1
                lColumnEventDescription = ACUColumnEventDescription - 1 'Gaurav changed
            End If

            lblPolicy.Visible = m_bCopyPolicy
            txtPolicy.Visible = m_bCopyPolicy

            ' Update the interface details.

            'Default to Quote when it's new business
            'RWH(15/05/2001) Check different code for UW.

            If m_oBusiness.UnderwritingOrAgency = "A" Then
                If m_sTransactionType = "G_NB" Then
                    cboStatus.SelectedIndex = 2
                End If
            Else
                'RWH(15/05/2001) New Business is now "NB" rather than "G_NB" for UW.
                If m_sTransactionType = "NB" Then
                    cboStatus.SelectedIndex = 2
                End If

                If m_sTransactionType = "MTR" Then
                    cboStatus.SelectedIndex = 4
                End If
            End If

            ' Clear the search details.
            lvwSearchDetails.Items.Clear()

            m_lItemsFound = gPMConstants.PMEFormatStyle.PMFormatString

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                'SJ 22/07/2004 - start
                RaiseEvent PolicyListRefreshed(Me, New PolicyListRefreshedEventArgs(m_lItemsFound))
                'SJ 22/07/2004 - end
                Return result
            End If


            'SJ 20/02/2004 - start
            If m_bIsUnderwritingBranch Then
                If m_vSearchData.GetUpperBound(0) >= ACIAlternateReference Then
                    bUseAlternateReference = True
                End If
            End If
            'SJ 20/02/2004 - end


            'Hide the registration column if not used...

            ' Assign the details to the interface.
            Dim oInsurancefoldercnt() As Object=Nothing
            ReDim oInsurancefoldercnt(0)

            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                bMatch = False
                Repeatedata = False
                ' {* USER DEFINED CODE (Begin) *}

                If Not (Convert.IsDBNull(m_lInsFileCnt) Or IsNothing(m_lInsFileCnt)) And (m_lInsFileCnt <> 0) And (m_lInsFileCnt = CInt(m_vSearchData(ACIInsFileCnt, lRow))) Then
                Else
                    Select Case cboStatus.SelectedIndex
                        'CURRENT
                        Case 0
                            'BSJ 12/06/00 - Added case for MTA's
                            'DC300101 Added check for MTAQTEREIN


                            bMatch = ((Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow))) And ((CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "POLICY") Or (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTA PERM") Or (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTA TEMP") Or (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "RENEWAL") Or (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAQREINS") Or (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAQTEREIN")))
                            'PENDING
                        Case 1
                            'CT 5/12/00 this is an under renewal search. Commented out code needs changing
                            'to bring back policy status of under renewal


                            If m_oBusiness.UnderwritingOrAgency = "A" Then

                                'DC170701 -start -restructure following as was not allowing for condition in ELSE statement

                                If Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow)) Then

                                    bMatch = (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "RENEWAL")

                                Else

                                    bMatch = (CStr(m_vSearchData(ACIStatus, lRow)).Trim() = "REN" And (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "POLICY"))

                                End If
                                'DC170701 -end
                            Else
                                'TN20010723 - works differently for MTA
                                If m_sTransactionType = "MTA" Or m_sTransactionType = "MTC" Then

                                    bMatch = ((Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow))) And (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "RENEWAL"))
                                Else

                                    If Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow)) Then
                                        bMatch = False
                                    Else
                                        bMatch = (CStr(m_vSearchData(ACIStatus, lRow)).Trim() = "REN" And (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "POLICY"))
                                    End If
                                End If
                            End If
                            'QUOTE
                        Case 2
                            'Start Written Status
                            If (Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow))) And ((CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "QUOTE") Or (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAQUOTE") Or (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAQTETEMP")) Then

                                'End- Written Status
                                bMatch = True
                            Else
                                'RDT180900 - Added condition below so that policies are returned if the Gemini
                                '            status is Pending(20) or Pending Transmitted(30)
                                If m_lFindType = 1 Then
                                    bMatch = Conversion.Val(CStr(m_vSearchData(ACIGeminiPolicyStatus, lRow))) = 20 Or Conversion.Val(CStr(m_vSearchData(ACIGeminiPolicyStatus, lRow))) = 30
                                Else
                                    bMatch = False
                                End If
                            End If
                            'LAPSED
                        Case 3

                            If Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow)) Then
                                bMatch = False
                            Else
                                bMatch = (CStr(m_vSearchData(ACIStatus, lRow)).Trim() = "LAP")
                            End If
                            'CANCELLED
                        Case 4
                            'DC300101 Added checks for MTAQTECAN and NTAPERMCAN

                            If Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow)) Then
                                bMatch = (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAQTECAN" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAPERMCAN")
                            Else
                                bMatch = (CStr(m_vSearchData(ACIStatus, lRow)).Trim() = "CAN")
                            End If
                            'TRANSFERRED
                            'DC 11/09/00 now REPLACED
                        Case 5

                            If Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow)) Then
                                bMatch = False
                            Else
                                'DC 11/09/00 was TRA now REP
                                bMatch = (CStr(m_vSearchData(ACIStatus, lRow)).Trim() = "REP")
                            End If
                            'ACTIVE
                        Case 6

                            If m_oBusiness.UnderwritingOrAgency = "A" Then
                                'DC300101 Added checks for MTAQUOTE, MTA PERM, MTA TEMP, MTAQTEREIN
                                'KB 17022003 PN Issue 1596
                                '            Quotes should not be included as 'Active'

                                bMatch = ((Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow))) And (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "POLICY" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "RENEWAL" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTA PERM" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTA TEMP"))
                            Else

                                If (Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow))) And (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "POLICY" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "RENEWAL" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTA PERM" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTA TEMP" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAREINS") Then
                                     bMatch=True
                                    For lRow2 As Integer = 0 To oInsurancefoldercnt.GetUpperBound(0)
                                        If (Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow))) And (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTA PERM" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTA TEMP" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAREINS") Then
                                            If lRow2 > 0 Then
                                                If (oInsurancefoldercnt(lRow2 - 1) = m_vSearchData(ACIInsFolderCnt, lRow)) Then
                                                    bMatch = False
                                                    Exit For
                                                End If
                                            End If
                                        Else
                                            If (oInsurancefoldercnt(lRow2) = m_vSearchData(ACIInsFolderCnt, lRow)) Then
                                                bMatch = False
                                                Exit For
                                            End If
                                        End If
                                    Next lRow2
                                    If bMatch Then
                                            ReDim Preserve oInsurancefoldercnt(oInsurancefoldercnt.GetUpperBound(0) + 1)
                                            oInsurancefoldercnt(oInsurancefoldercnt.GetUpperBound(0))=m_vSearchData(ACIInsFolderCnt, lRow)
                                        End If
                                ElseIf ((IIf(Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow)), "", CStr(m_vSearchData(ACIStatus, lRow)))).Trim() = "REN" And (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "POLICY" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "RENEWAL" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTA PERM" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTA TEMP" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAREINS")) Then
                                    bMatch = True
                                Else
                                    bMatch = False
                                End If

                            End If
                            'INACTIVE
                        Case 7
                            'DC300101 Added checks for MTAQTECAN and MTAPERMCAN

                            If Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow)) Then
                                bMatch = (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAQTECAN" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAPERMCAN")
                            Else
                                'DC 11/09/00 was TRA now REP
                                bMatch = (CStr(m_vSearchData(ACIStatus, lRow)).Trim() = "LAP" Or CStr(m_vSearchData(ACIStatus, lRow)).Trim() = "CAN" Or CStr(m_vSearchData(ACIStatus, lRow)).Trim() = "REP")
                            End If
                            'DC 11/09/00 added new option
                        Case 8
                            'ALL
                            'DC300101 Added checks for MTAQTETEMP, MTAQTECAN, MTAQTEREIN, MTAPERMCAN
                            'KB PN Issue 3702 Added check for "Incomplete" - a GII status

                            If (Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow))) And ((CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "POLICY") Or (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "RENEWAL") Or (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "QUOTE") Or (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAQUOTE") Or (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTA PERM") Or (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTA TEMP") Or (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAQTETEMP") Or (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAQTECAN") Or (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAQTEREIN") Or (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAPERMCAN") Or (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "INCOMPLETE")) Then
                                bMatch = True
                            Else

                                If Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow)) Then
                                    bMatch = False
                                Else
                                    'CT 1/12/00 added in REN
                                    '                            If (Trim$(m_vSearchData(ACIStatus, lRow)) = "LAP" _
                                    ''                                Or Trim$(m_vSearchData(ACIStatus, lRow)) = "CAN" _
                                    ''                                Or Trim$(m_vSearchData(ACIStatus, lRow)) = "REP") _
                                    ''                            Then
                                    bMatch = (CStr(m_vSearchData(ACIStatus, lRow)).Trim() = "LAP" Or CStr(m_vSearchData(ACIStatus, lRow)).Trim() = "CAN" Or CStr(m_vSearchData(ACIStatus, lRow)).Trim() = "REP" Or CStr(m_vSearchData(ACIStatus, lRow)).Trim() = "REN")
                                End If
                            End If
                            ' TB 07/10/2004: For GII re-xmit, include cancelled boo
                        Case 9

                            If (Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow))) And (CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "POLICY" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "RENEWAL" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTA PERM" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTA TEMP" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAQTECAN" Or CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "MTAPERMCAN") Then
                                bMatch = True
                            Else

                                If Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow)) Then 'PN18878
                                    bMatch = False
                                Else
                                    bMatch = (CStr(m_vSearchData(ACIStatus, lRow)).Trim() = "LAP" Or CStr(m_vSearchData(ACIStatus, lRow)).Trim() = "CAN" Or CStr(m_vSearchData(ACIStatus, lRow)).Trim() = "REP" Or CStr(m_vSearchData(ACIStatus, lRow)).Trim() = "REN")
                                End If
                            End If
                    End Select

                    '04/1/2005 Only do the addtional check if we found a match - PN 17648
                    If bMatch Then
                        'SJ 24/02/2004 - start
                        If m_bUnderwritingBranchEnabled Then
                            bMatch = False
                            If m_bIsUnderwritingBranch Then
                                ' The branch/company we are logged on as is an "Insurer" one

                                If Not (Convert.IsDBNull(m_vSearchData(ACIAlternateReference, lRow)) Or IsNothing(m_vSearchData(ACIAlternateReference, lRow))) Then
                                    'This is and Edi policy created by an "Insurer" branch
                                    bMatch = True
                                ElseIf CDbl(m_vSearchData(ACIPolicyTypeId, lRow)) = PMBConst.PMBPolicyTypeGeneral Then
                                    'The is a general policy
                                    bMatch = True
                                ElseIf CDbl(m_vSearchData(ACIInsFileSourceId, lRow)) = g_iSourceID Then
                                    'This policy is owned by the current branch/company
                                    bMatch = True
                                End If
                            Else
                                ' The branch/company we are logged on as is a "Normal" one
                                If gPMFunctions.NullToLong(CStr(m_vSearchData(ACIUnderwritingBranchInd, lRow))) = 0 Then
                                    'this policy was not created by an "Insurer" branch
                                    bMatch = True
                                End If
                            End If
                        End If
                        'SJ 24/02/2004 - end
                    End If
                End If

                If bMatch Then
                    'eck050500
                    If ValidSource(vSource:=m_vSearchData(ACIInsFileSourceId, lRow)) Then
                        'eck050500
                        'MSS240701 - Start - Rearranging columns and adding risk type description
                        ' Policy Number, Regarding, Renewal Date, Insurer, Risk Type Desc(new)
                        ' Premium, Policy Status, Policy Type, Risk Type

                        ' Assign the details to the first column.
                        m_lItemsFound = CType(m_lItemsFound + 1, gPMConstants.PMEFormatStyle)

                        'SJ 20/02/2004 - start
                        sInsuranceReference = CStr(m_vSearchData(ACIInsReference, lRow)).Trim()

                        If lvwSearchDetails.Items.Count > 0 Then
                            For lRow1 As Integer = 0 To lvwSearchDetails.Items.Count - 1

                                If lvwSearchDetails.Items.Item(lRow1).Text = sInsuranceReference Then
                                    Repeatedata = gPMConstants.PMEReturnCode.PMTrue
                                End If

                            Next lRow1
                        End If
                        If Not Repeatedata Then
                            If bUseAlternateReference Then

                                If Convert.IsDBNull(m_vSearchData(ACIAlternateReference, lRow)) Or IsNothing(m_vSearchData(ACIAlternateReference, lRow)) Then
                                    sAlternateReference = ""
                                Else
                                    sAlternateReference = CStr(m_vSearchData(ACIAlternateReference, lRow)).Trim()
                                End If
                                If sAlternateReference <> "" Then
                                    sInsuranceReference = sAlternateReference
                                End If
                            End If
                            'SJ 20/02/2004 - end
                            oListItem = lvwSearchDetails.Items.Add(sInsuranceReference, ACFindImage)

                            If Convert.IsDBNull(m_vSearchData(ACIInsFolderName, lRow)) Or IsNothing(m_vSearchData(ACIInsFolderName, lRow)) Then
                                ListViewHelper.GetListViewSubItem(oListItem, lColumnRegarding).Text = ""

                                'sj 27/06/2002 - start
                            Else
                                ListViewHelper.GetListViewSubItem(oListItem, lColumnRegarding).Text = CStr(m_vSearchData(ACIInsFolderName, lRow)).Trim()
                            End If

                            If Convert.IsDBNull(m_vSearchData(ACILastModified, lRow)) Or IsNothing(m_vSearchData(ACILastModified, lRow)) Then
                                ListViewHelper.GetListViewSubItem(oListItem, lColumnRenewalDate).Text = ""
                            Else
                                If Information.IsDate(m_vSearchData(ACILastModified, lRow)) Then
                                    ListViewHelper.GetListViewSubItem(oListItem, lColumnRenewalDate).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=CStr(m_vSearchData(ACILastModified, lRow)))
                                Else
                                    ListViewHelper.GetListViewSubItem(oListItem, lColumnRenewalDate).Text = ""
                                End If
                            End If

                            If Convert.IsDBNull(m_vSearchData(ACIInsurer, lRow)) Or IsNothing(m_vSearchData(ACIInsurer, lRow)) Then
                                ListViewHelper.GetListViewSubItem(oListItem, lColumnInsurer).Text = ""
                            Else
                                ListViewHelper.GetListViewSubItem(oListItem, lColumnInsurer).Text = CStr(m_vSearchData(ACIInsurer, lRow)).Trim()
                            End If


                            If Convert.IsDBNull(m_vSearchData(ACIRiskDesc, lRow)) Or IsNothing(m_vSearchData(ACIRiskDesc, lRow)) Then
                                ListViewHelper.GetListViewSubItem(oListItem, lColumnRiskTypeDescription).Text = ""
                            Else
                                ListViewHelper.GetListViewSubItem(oListItem, lColumnRiskTypeDescription).Text = CStr(m_vSearchData(ACIRiskDesc, lRow)).Trim()
                            End If

                            'Gaurav Changed

                            If Convert.IsDBNull(m_vSearchData(ACIEventDescription, lRow)) Or IsNothing(m_vSearchData(ACIEventDescription, lRow)) Then
                                ListViewHelper.GetListViewSubItem(oListItem, lColumnEventDescription).Text = ""
                            Else
                                ListViewHelper.GetListViewSubItem(oListItem, lColumnEventDescription).Text = CStr(m_vSearchData(ACIEventDescription, lRow)).Trim()
                            End If

                            If Convert.IsDBNull(m_vSearchData(ACIPremium, lRow)) Or IsNothing(m_vSearchData(ACIPremium, lRow)) Then
                                ListViewHelper.GetListViewSubItem(oListItem, lColumnPremium).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(0))
                            Else
                                ListViewHelper.GetListViewSubItem(oListItem, lColumnPremium).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_vSearchData(ACIPremium, lRow)))
                            End If

                            ' Column x Policy Status
                            sString = ""

                            If m_lFindType = 1 Then
                                Select Case CInt(m_vSearchData(ACIGeminiPolicyStatus, lRow))
                                    Case GIIPolicyIncomplete
                                        sString = "Incomplete"
                                    Case GIIPolicyQuote
                                        sString = "Quote"
                                    Case GIIPolicyNBComplete
                                        sString = "NB Complete"
                                    Case GIIPolicyRequoteRequired
                                        sString = "Requote Required"
                                    Case GIIPolicyRequoted
                                        sString = "Requoted"
                                    Case GIIPolicyPending
                                        sString = "Pending"
                                    Case GIIPolicyPendingTransmitted
                                        sString = "Pending Transmitted"
                                    Case GIIPolicyLive
                                        sString = "Live"

                                        'PN 25688
                                        'AR 03/05/2006
                                        'ADDED MORE DESCRIPTIONS TO CATCH THE POLICY STATUS

                                    Case GIIPolicyMTAPermanent, GIIPolicyMTAIncomplete
                                        sString = "MTA PERM"
                                    Case GIIPolicyMTACancellation
                                        sString = "MTA CAN"
                                    Case Else
                                        sString = "Cancelled"

                                End Select
                            Else

                                If Convert.IsDBNull(m_vSearchData(ACIStatus, lRow)) Or IsNothing(m_vSearchData(ACIStatus, lRow)) Then
                                    If CStr(m_vSearchData(ACIInsFileType, lRow)).Trim() = "RENEWAL" Then
                                        sString = "IN RENEWAL"
                                    Else
                                        sString = "LIVE"
                                    End If
                                Else
                                    Select Case CStr(m_vSearchData(ACIStatus, lRow)).Trim()
                                        Case "CAN"
                                            sString = "CANCELLED"
                                        Case "LAP"
                                            sString = "LAPSED"
                                        Case "REN"
                                            sString = "RENEWED"
                                        Case "TRA"
                                            sString = "TRANSFERRED"
                                        Case "REP"
                                            sString = "REPLACED"
                                    End Select
                                End If
                            End If

                            ListViewHelper.GetListViewSubItem(oListItem, lColumnPolicyStatus).Text = sString

                            ' Column x Policy Type
                            ListViewHelper.GetListViewSubItem(oListItem, lColumnPolicyType).Text = CStr(m_vSearchData(ACIPolicyType, lRow)).Trim()

                            ' Column x Risk Type Status (or product type)
                            If m_sUnderwritingOrAgency = "U" Then
                                ListViewHelper.GetListViewSubItem(oListItem, lColumnRiskType).Text = CStr(m_vSearchData(ACIProductName, lRow)).Trim()
                            Else
                                ListViewHelper.GetListViewSubItem(oListItem, lColumnRiskType).Text = CStr(m_vSearchData(ACIInsFileType, lRow)).Trim()
                            End If

                            'Tomo200300
                            'Populate columns for GIIM
                            '                Select Case m_lFindType
                            '                Case 1
                            If m_bGeminiIILink Then
                                ListViewHelper.GetListViewSubItem(oListItem, lColumnGeminiEDI).Text = CStr(m_vSearchData(ACIGeminiIIEdi, lRow))
                            End If
                            '                End Select

                            'SJ 18/10/2004 - start
                            If m_lFindType = 1 Then
                                If CDbl(m_vSearchData(ACIStoredInd, lRow)) = 1 Then
                                    ListViewHelper.GetListViewSubItem(oListItem, lColumnStoredInd).Text = "Yes"
                                Else
                                    ListViewHelper.GetListViewSubItem(oListItem, lColumnStoredInd).Text = "No"
                                End If
                            End If
                            'SJ 18/10/2004 - end
                            ' {* USER DEFINED CODE (End) *}

                            ' Set the tag property with the index of
                            ' the search data storage.
                            oListItem.Tag = CStr(lRow)

                            ' Refresh the first X amount of rows, to
                            ' allow the user to see the results instantly.
                            If m_lItemsFound = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                                If Not bSearchSet Then
                                    ' Select the first item.
                                    lvwSearchDetails.Items.Item(0).Selected = True

                                    ' Refresh the initial results.
                                    lvwSearchDetails.Refresh()
                                    bSearchSet = True
                                End If
                            End If
                            'eck050500 endif for validsource
                        End If
                    End If

                End If
            Next lRow
            'MSS240701 - End - Rearranging columns and adding risk type description

            m_sPolicyStatus = cboStatus.Text ' pass the selected Polciy Status to Interface
            RaiseEvent PolicyListRefreshed(Me, New PolicyListRefreshedEventArgs(m_lItemsFound))


            ''TN20010420 Start
            'If m_sUnderwritingOrAgency = "U" Then
            '    If lvwSearchDetails.Items.Count > 0 Then
            '        lvwSearchDetails.Items.Item(0).Selected = True
            '        m_lSelected = gPMConstants.PMEReturnCode.PMTrue
            '    End If
            'End If
            ''TN20010420 End

            'Tomo200300
            'Hide columns not required
            Select Case m_lFindType
                Case 1
                    'MSS250701 - Changed to account for new ColumnHeader order
                    lvwSearchDetails.Columns.Item(lColumnPolicyStatus - 1).Width = CInt(0) ' Policy Type
                    'MSS250701 - End
            End Select

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
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", excep:=excep)

            Return result



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
        Dim iSourceID As Integer
        Dim lKeyID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' CF 210699 - Can't set properties if there's nothing in the list, so
            '             just exit.
            If lvwSearchDetails.Items.Count = 0 Then
                Return result
            End If

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}

            m_lInsFileCnt = CInt(m_vSearchData(ACIInsFileCnt, lSelectedItem))
            m_sInsReference = CStr(m_vSearchData(ACIInsReference, lSelectedItem)).Trim()
            '    m_lProductId& = Trim$(m_vSearchData(ACIProductID, lSelectedItem&))
            m_lInsHolderCnt = CInt(CStr(m_vSearchData(ACIInsHolderCnt, lSelectedItem)).Trim())
            m_sLongName = CStr(m_vSearchData(ACIInsuredLongName, lSelectedItem)).Trim()
            m_sShortName = CStr(m_vSearchData(ACIInsuredShortName, lSelectedItem)).Trim()
            m_lInsuranceFolderCnt = CInt(m_vSearchData(ACIInsFolderCnt, lSelectedItem))
            m_vLeadAgentCnt = CStr(m_vSearchData(ACILeadAgentCnt, lSelectedItem))
            'Tomo240300
            m_lPolicyTypeId = CInt(m_vSearchData(ACIPolicyTypeId, lSelectedItem))

            m_vGeminiPolicyStatus = CStr(m_vSearchData(ACIGeminiPolicyStatus, lSelectedItem)) 'JSB 08/06/01 - This is required to fire up GII

            If m_lFindType = 5 Then
                '        m_lInsuranceFileTypeId = m_vSearchData(25, lSelectedItem)
                m_lInsuranceFileTypeId = CInt(m_vSearchData(ACIInsuranceFileTypeId, lSelectedItem))
            End If

            'Calculate the combined UIKs
            iSourceID = CInt(m_vSearchData(ACIInsFileSourceId, lSelectedItem))
            lKeyID = CInt(m_vSearchData(ACIInsFileId, lSelectedItem))

            '   SJP 04072002 - CalcCombinedKey will return what is passed in
            '           Therefore we need to set it to the account key beforehand
            '          ie m_lPolicyUIK = m_lInsFileCnt
            m_lPolicyUIK = m_lInsFileCnt

            m_lReturn = m_oBusiness.CalcCombinedKey(v_lSourceID:=iSourceID, v_lKeyID:=lKeyID, r_lCombinedKeyID:=m_lPolicyUIK)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the Policy UIK.", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", excep:=New Exception(Information.Err().Description))
                Return result
            End If

            iSourceID = CInt(m_vSearchData(ACIInsuredSourceId, lSelectedItem))
            lKeyID = CInt(m_vSearchData(ACIInsuredId, lSelectedItem))

            '   SJP 04072002 - CalcCombinedKey will return what is passed in
            '           Therefore we need to set it to the account key beforehand
            '          ie m_lPartyUIK= m_lInsHolderCnt
            m_lPartyUIK = m_lInsHolderCnt

            m_lReturn = m_oBusiness.CalcCombinedKey(v_lSourceID:=iSourceID, v_lKeyID:=lKeyID, r_lCombinedKeyID:=m_lPartyUIK)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the Party UIK.", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", excep:=New Exception(Information.Err().Description))
                Return result
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetTargetParty
    '
    ' Description:
    '
    ' History: 02/08/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetTargetParty(ByRef r_lPartyCnt As Integer, ByRef r_sShortName As String, ByRef r_sResolvedName As String, ByRef r_sLongName As String, ByRef r_sPartyType As String) As Integer

        Dim result As Integer = 0

        'NIIT - Replaced with the Migrated code 1144 
        'Dim oFindParty As ClassInterface
        Dim oFindParty As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a new instance of FindParty
            Dim temp_oFindParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the short name

            oFindParty.ShortName = m_sShortName

            ' Start the component

            m_lReturn = oFindParty.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If oFindParty.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Return gPMConstants.PMEReturnCode.PMCancel
            End If

            ' Get the party_cnt

            r_lPartyCnt = oFindParty.PartyCnt

            r_sShortName = oFindParty.ShortName

            r_sResolvedName = oFindParty.ResolvedName

            r_sLongName = oFindParty.LongName

            r_sPartyType = oFindParty.PartyType

            ' Remove the instance of find party

            oFindParty.Dispose()
            oFindParty = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("r_lPartyCnt", r_lPartyCnt)
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTargetParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTargetParty", excep:=excep, oDicParms:=oDict)

            Return result




            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: CopyPolicyV2
    '
    ' Description:
    '
    ' History: 04/07/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function CopyPolicyV2() As Integer
        Dim result As Integer = 0

        Dim sOption As String = ""
        Dim lNewInsuranceFileCnt As Integer

        Dim oTextFile As iPMBTextFile.Copy

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'MKW PN6750 Reset the m_oOptions component START
            If Not (m_oOption Is Nothing) Then
                ' Terminate the business object

                m_oOption.Dispose()
                ' Destroy the instance of the business object
                ' from memory.
                m_oOption = Nothing

            End If

            Dim temp_m_oOption As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oOption, "bSIROptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oOption = temp_m_oOption

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create the options object", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyV2")

                Return result
            End If
            'MKW PN6750 Reset the m_oOptions component START

            ' Reset the insurance file cnt
            NewInsuranceFileCnt = 0

            ' Get the target party
            m_lReturn = GetTargetParty(r_lPartyCnt:=m_lTargetPartyCnt, r_sShortName:=m_sTargetShortName, r_sResolvedName:=m_sTargetResolvedName, r_sLongName:=m_sTargetLongName, r_sPartyType:=m_sTargetPartyType)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_bCopiedElsewhere = (m_lTargetPartyCnt <> m_lInsHolderCnt)

            ' Get the insurance file
            m_lReturn = DataToProperties()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TODO - Get CopyTextFiles System Option

            m_lReturn = m_oOption.getOption(iOptionNumber:=68, sValue:=sOption)

            'DJM 24/10/2003 : Never use this function to copy text files. It doesn't work properly                               .
            'Passing in zero for the source id will use the old policies source id.

            m_lReturn = m_oBusiness.CopyPolicyV2(v_lOldInsuranceFileCnt:=m_lInsFileCnt, r_lNewInsuranceFileCnt:=lNewInsuranceFileCnt, v_iSourceId:=0, v_lTargetPartyCnt:=m_lTargetPartyCnt, v_lDontCopyTextFiles:=1)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy policy", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyV2", excep:=New Exception(Information.Err().Description))
                Return result
            End If

            'DJM 24/10/2003 : Copy text files using new function.
            If sOption <> "0" Then

                Dim temp_oTextFile As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oTextFile, "iPMBTextFile.Copy", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                oTextFile = temp_oTextFile
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get an instance of the business object.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create the iPMBTextFile.Copy object", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyV2")

                    Return result
                End If


                m_lReturn = oTextFile.CopyTextFiles(m_lInsFileCnt, lNewInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oTextFile.CopyTextFiles failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyV2")

                    Return result
                End If

            End If

            ' TODO - Check System Option 67 (Copy Risk)

            m_lReturn = m_oOption.getOption(iOptionNumber:=67, sValue:=sOption)

            If sOption = "1" Then
                ' Copy the risk (if one exists) too

                m_lReturn = m_oBusiness.CopyRisk(v_lOldInsuranceFileCnt:=m_lInsFileCnt, v_lNewInsuranceFileCnt:=lNewInsuranceFileCnt)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy risk", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyV2", excep:=New Exception(Information.Err().Description))
                Return result
            End If

            ' Pass the insurance file cnt back here (to preserve bin compat)
            NewInsuranceFileCnt = lNewInsuranceFileCnt

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicyV2 Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyV2", excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyPolicy
    '
    ' Description:
    '
    ' History: 18/11/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function CopyPolicy(ByVal v_lOldInsuranceFileCnt As Integer, ByRef r_lNewInsuranceFileCnt As Integer, ByVal v_lVersion As Integer, ByVal v_bPermanentMTA As Boolean, ByVal v_dtMTADate As Date) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.CopyPolicy(v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt, r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_lVersion:=v_lVersion, v_bPermanentMTA:=v_bPermanentMTA, v_dtMTADate:=v_dtMTADate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lOldInsuranceFileCnt", v_lOldInsuranceFileCnt)
            oDict.Add("r_lNewInsuranceFileCnt", r_lNewInsuranceFileCnt)
            oDict.Add("v_dtMTADate", v_dtMTADate)
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicy", excep:=excep, oDicParms:=oDict)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetVersionByDate
    '
    ' Description:
    '
    ' History: 18/11/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function GetVersionByDate(ByRef r_lInsuranceFileCnt As Integer, ByVal v_dtStartDate As Date, ByRef r_lPolicyVersion As Integer, ByRef r_lErrorCode As Integer, Optional ByVal v_lInsuranceFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetVersionByDate(r_lInsuranceFileCnt:=r_lInsuranceFileCnt, v_dtStartDate:=v_dtStartDate, r_lPolicyVersion:=r_lPolicyVersion, r_lErrorCode:=r_lErrorCode, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("r_lInsuranceFileCnt", r_lInsuranceFileCnt)
            oDict.Add("v_dtStartDate", v_dtStartDate)
            oDict.Add("r_lErrorCode", r_lErrorCode)
            oDict.Add("v_lInsuranceFolderCnt", v_lInsuranceFolderCnt)
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVersionByDate", excep:=excep, oDicParms:=oDict)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckInRenewal
    '
    ' Description:
    '
    ' History: 09/02/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function CheckInRenewal(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lRenewalStatus As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.CheckInRenewal(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lRenewalStatus:=r_lRenewalStatus)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lInsuranceFileCnt", v_lInsuranceFileCnt)
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckInRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckInRenewal", excep:=excep, oDicParms:=oDict)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateRenewalStatus
    '
    ' Description:
    '
    ' History: 09/02/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateRenewalStatus(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.UpdateRenewalStatus(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lInsuranceFileCnt", v_lInsuranceFileCnt)
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRenewalStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRenewalStatus", excep:=excep, oDicParms:=oDict)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: PropertiesToInterface
    '
    ' Description: Updates the interface details from the property
    '              members.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (PropertiesToInterface) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function PropertiesToInterface() As Integer
    '
    'Dim result As Integer = 0
    'Dim lSelectedItem As Integer
    '
    'Try 
    '
    '
    ' Update the interface details.
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    '
    ' {* USER DEFINED CODE (End) *}
    '
    '
    'Return gPMConstants.PMEReturnCode.pmtrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogExcepMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: ValidateLookups
    '
    ' Description: Validates the interface lookups.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ValidateLookups) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ValidateLookups() As Integer
    '
    'Dim result As Integer = 0
    'Dim lReturn, lPartyCnt As Integer
    'Static sTitle, sMessage As String
    '
    'Try 
    '
    '
    '
    '
    'Return gPMConstants.PMEReturnCode.pmtrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogExcepMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate lookups", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateLookups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim lColumnPolicyNumber, lColumnPolicyType, lColumnRiskType, lColumnRegarding, lColumnRenewalDate, lColumnInsurer, lColumnPremium, lColumnPolicyStatus, lColumnRiskTypeDescription, lColumnGeminiEDI As Integer
        'SJ 18/10/2004 - start
        Dim lColumnStoredInd As Integer
        'SJ 18/10/2004 - end

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'sj 02/10/2002 - start
            With Toolbar1

                .ImageList = ImageList1
                'developer guide no. New Toolbar is created.
                .Items.Item("TB_Event").ImageIndex = 10
                .Items.Item("TB_RiskDetails").Visible = False
                .Items.Item("TB_InformationChecklist").Visible = False
                .Items.Item("TB_Policy").Visible = True
                .Items.Item("TB_Event").Visible = True
                If m_sUnderwritingOrAgency = "U" Then
                    .Items.Item("sep1").Visible = False
                    .Items.Item("sep2").Visible = False
                    '.Items.Item("Policy").ImageIndex = 6
                    .Items.Item("TB_Policy").ImageIndex = 6
                Else
                    .Items.Item("TB_Policy").Visible = False
                End If
            End With
            'sj 02/10/2002 - end

            If m_sUnderwritingOrAgency = "A" Then
                'SJ 18/10/2004 - start
                If m_lFindType = 1 Then
                    'Used for Gemini II New Business
                    lColumnPolicyNumber = ACGColumnPolicyNumber
                    lColumnStoredInd = ACGColumnStoredInd
                    lColumnPolicyType = ACGColumnPolicyType
                    lColumnRiskType = ACGColumnRiskType
                    lColumnRegarding = ACGColumnRegarding
                    lColumnRenewalDate = ACGColumnRenewalDate
                    lColumnInsurer = ACGColumnInsurer
                    lColumnPremium = ACGColumnPremium
                    lColumnPolicyStatus = ACGColumnPolicyStatus
                    lColumnRiskTypeDescription = ACGColumnRiskTypeDescription
                    lColumnGeminiEDI = ACGColumnGeminiEDI
                Else
                    'SJ 18/10/2004 - end
                    lColumnPolicyNumber = ACAColumnPolicyNumber
                    lColumnPolicyType = ACAColumnPolicyType
                    lColumnRiskType = ACAColumnRiskType
                    lColumnRegarding = ACAColumnRegarding
                    lColumnRenewalDate = ACAColumnRenewalDate
                    lColumnInsurer = ACAColumnInsurer
                    lColumnPremium = ACAColumnPremium
                    lColumnPolicyStatus = ACAColumnPolicyStatus
                    lColumnRiskTypeDescription = ACAColumnRiskTypeDescription
                    lColumnGeminiEDI = ACAColumnGeminiEDI
                End If
            Else
                lColumnPolicyNumber = ACUColumnPolicyNumber
                lColumnPolicyType = ACUColumnPolicyType
                lColumnRiskType = ACUColumnRiskType
                lColumnRegarding = ACUColumnRegarding
                lColumnRenewalDate = ACUColumnRenewalDate
                lColumnInsurer = ACUColumnInsurer
                lColumnPremium = ACUColumnPremium
                lColumnPolicyStatus = ACUColumnPolicyStatus
                lColumnRiskTypeDescription = ACUColumnRiskTypeDescription
                lColumnGeminiEDI = ACUColumnGeminiEDI
            End If

            ' Center the interface.
            '    CenterForm Me

            txtClientCode.Text = m_sShortName

            cboStatus.Items.Add("Current")
            'DC 11/09/00 Changed Pending to Under Renewal
            cboStatus.Items.Add("Under Renewal")
            cboStatus.Items.Add("Quote")
            cboStatus.Items.Add("Lapsed")
            cboStatus.Items.Add("Cancelled")
            'DC 11/09/00 Changed Transferred to Replaced
            cboStatus.Items.Add("Replaced")
            cboStatus.Items.Add("Active")
            cboStatus.Items.Add("Inactive")
            'DC 11/09/00 Added new option
            cboStatus.Items.Add("ALL")
            ' TB 07/10/2004 - New Option - Policies (inc cancelled)
            cboStatus.Items.Add("Policy")
            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add something here to set the default status to current


            ' {* USER DEFINED CODE (Begin) *}

            ' Set the column widths for the search list.
            lvwSearchDetails.Columns.Item(lColumnPolicyNumber - 1).Width = CInt(VB6.TwipsToPixelsX(1800))
            lvwSearchDetails.Columns.Item(lColumnRegarding - 1).Width = CInt(VB6.TwipsToPixelsX(1100))
            lvwSearchDetails.Columns.Item(lColumnRenewalDate - 1).Width = CInt(VB6.TwipsToPixelsX(1400))
            lvwSearchDetails.Columns.Item(lColumnInsurer - 1).Width = CInt(VB6.TwipsToPixelsX(1800))
            lvwSearchDetails.Columns.Item(lColumnRiskTypeDescription - 1).Width = CInt(VB6.TwipsToPixelsX(2200))
            lvwSearchDetails.Columns.Item(lColumnPremium - 1).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwSearchDetails.Columns.Item(lColumnPolicyStatus - 1).Width = CInt(VB6.TwipsToPixelsX(1400))
            lvwSearchDetails.Columns.Item(lColumnPolicyType - 1).Width = CInt(VB6.TwipsToPixelsX(1400))
            'SJ 18/10/2004 - start
            'lvwSearchDetails.ColumnHeaders(lColumnRiskType).Width = 1000
            'SJ 18/10/2004 - end
            ' Reverse SJ 18/10/2004 which comments out this line
            '    lvwSearchDetails.ColumnHeaders(lColumnRiskType).Width = 1000


            'Tomo200300
            'Add columns for GIIM
            'IF Gemini II installed...
            If m_bGeminiIILink Then
                lvwSearchDetails.Columns.Add("EDI", CInt(VB6.TwipsToPixelsX(720)))
            End If

            'SJ 18/10/2004 - start
            If m_lFindType = 1 Then
                lvwSearchDetails.Columns.Add(" ", CInt(VB6.TwipsToPixelsX(1440)))
                If m_bGeminiIILink Then
                    lvwSearchDetails.Columns.Item(lColumnGeminiEDI - 1).Width = CInt(VB6.TwipsToPixelsX(720))
                End If
                lvwSearchDetails.Columns.Item(lColumnStoredInd - 1).Width = CInt(VB6.TwipsToPixelsX(1000))
            End If
            lvwSearchDetails.Columns.Item(lColumnRiskType - 1).Width = CInt(VB6.TwipsToPixelsX(1400))
            'SJ 18/10/2004 - end

            ' CF 180699 - Made full row select on list views
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwSearchDetails.Handle.ToInt32(), v_vShowRowSelect:=True)
            ' Check for errors.
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'Return gPMConstants.PMEReturnCode.PMFalse
            'End If
            lvwSearchDetails.FullRowSelect = True
            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            '    ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'cboStatus.ListIndex = 0
            CheckInsFileType()

            ' {* USER DEFINED CODE (End) *}

            If m_sUnderwritingOrAgency = "U" Then
                Select Case m_sTransactionType
                    Case "NB", "MTR", "MTC"
                        cboStatus.Enabled = False
                        'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
                    Case "MTA"
                        cboStatus.Enabled = True
                        'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
                End Select
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", excep:=excep)

            Return result


            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: CheckInsFileType
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Sub CheckInsFileType()

        Try

            Select Case m_sInsFileType
                Case "Current"
                    cboStatus.SelectedIndex = 0
                    'DC 11/09/00 Changed from Pending to Under Renewal
                Case "Under Renewal"
                    cboStatus.SelectedIndex = 1
                Case "Quote"
                    cboStatus.SelectedIndex = 2
                Case "Lapsed"
                    cboStatus.SelectedIndex = 3
                Case "Cancelled"
                    cboStatus.SelectedIndex = 4
                    'DC 11/09/00 Changed from Transferred to Replaced
                Case "Replaced"
                    cboStatus.SelectedIndex = 5
                Case "Active"
                    cboStatus.SelectedIndex = 6
                Case "Inactive"
                    cboStatus.SelectedIndex = 7
                    'DC 11/09/00 Added new option for All stati
                Case "ALL"
                    cboStatus.SelectedIndex = 8
                    ' TB 07/10/2004:  Policy includes cancelled policies for re-xmit
                Case "Policy", "POLICY"
                    cboStatus.SelectedIndex = 9
                Case Else
                    'DC301010 set to 6 rather than 0
                    cboStatus.SelectedIndex = 6
            End Select

            If m_bDisableInsFileType Then
                cboStatus.Enabled = False
            End If

        Catch excep As System.Exception




            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckInsFileType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckInsFileType", excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: ClearInterface
    '
    ' Description: Clears all of the interface details for a new
    '              search.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ClearInterface) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ClearInterface() As Integer
    '
    'Dim result As Integer = 0
    'Dim iMsgResult As DialogResult
    'Dim sMessage, sTitle As String
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.pmtrue
    '
    ' Check if the user still wishes to clear
    ' the interface.
    '

    'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '
    ' Display the message.
    'iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
    '
    ' Check message result.
    'If iMsgResult = System.Windows.Forms.DialogResult.No Then
    ' Don't continue with the clear.
    'Return result
    'End If
    '
    ' Clear the interface details.
    '
    ' Clear the search data array.
    'm_vSearchData = Nothing
    '
    ' Clear the search list details.
    'lvwSearchDetails.Items.Clear()
    '
    ' Clear the search status bar.
    'stbStatus.Text = ""
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    'txtClientCode.Text = ""
    '
    ' Set to the first tab.
    'SSTabHelper.SetSelectedIndex(tabMainTab, 0)
    '
    '
    '
    ' {* USER DEFINED CODE (End) *}
    '
    ' Disable parts of the interface, so the
    ' user can now only enter a new search
    'm_lReturn = DisableInterface(bDisable:=True)
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogExcepMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

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
            ReDim m_ctlTabFirstLast(1, 2)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", excep:=excep)

            Return result

        End Try
    End Function
    'eck090500
    ' ********************************************************************************* '
    ' Name: Private Function                                                            '
    '                                                                                   '
    ' Description: Checks that the transaction is for one of the branches being paid    '
    '                                                                                   '
    ' ********************************************************************************* '
    Private Function ValidSource(ByVal vSource As Object) As Boolean
        Dim result As Boolean = False




        Dim vMultiCompany, vBranchLogon As Object

        If m_vSearchData.GetUpperBound(1) = 0 And CStr(m_vSearchData(2, 0)).Trim() = txtClientCode.Text.Trim() Then
            Return True
        End If


        Dim sSourceId As String = CStr(vSource)


        'developer guide no. 98
        iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=1, r_vUnderwriting:=vMultiCompany)


        'developer guide no. 98
        iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableBranchSelectAtLogon, v_vBranch:=1, r_vUnderwriting:=vBranchLogon)

        Dim bMultiCompany As Boolean = gPMFunctions.ToSafeString(vBranchLogon) = "1" And gPMFunctions.ToSafeString(vMultiCompany) = "1"

        If bMultiCompany Then
            If (m_oValidSource.ContainsKey(sSourceId) And m_oAllowBranches.ContainsKey(sSourceId)) Or StringsHelper.ToDoubleSafe(sSourceId) = g_iSourceID Then
                Return True
            End If
        Else
            If m_oValidSource.ContainsKey(sSourceId) Or m_oAllowBranches.ContainsKey(sSourceId) Then
                Return True
            End If
        End If

        '    If IsArray(m_vSourceArray) = False Then
        '            ValidSource = True
        '            Exit Function
        '    End If
        '    For i = 1 To UBound(m_vSourceArray, 2)
        '        If CLng(m_vSourceArray(1, i)) = CLng(vSource) Then
        '            ValidSource = True
        '        End If
        '    Next i
        'SJ 08/04/2004 - end

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Dim lColumnPolicyNumber, lColumnPolicyType, lColumnRiskType, lColumnRegarding, lColumnRenewalDate, lColumnInsurer, lColumnPremium, lColumnPolicyStatus, lColumnRiskTypeDescription, lColumnGeminiEDI As Integer
        'SJ 18/10/2004 - start
        Dim lColumnStoredInd As Integer
        'SJ 18/10/2004 - end

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sUnderwritingOrAgency = "A" Then
                'SJ 18/10/2004 - start
                If m_lFindType = 1 Then
                    'Used for Gemini II New Business
                    lColumnPolicyNumber = ACGColumnPolicyNumber
                    lColumnStoredInd = ACGColumnStoredInd
                    lColumnPolicyType = ACGColumnPolicyType
                    lColumnRiskType = ACGColumnRiskType
                    lColumnRegarding = ACGColumnRegarding
                    lColumnRenewalDate = ACGColumnRenewalDate
                    lColumnInsurer = ACGColumnInsurer
                    lColumnPremium = ACGColumnPremium
                    lColumnPolicyStatus = ACGColumnPolicyStatus
                    lColumnRiskTypeDescription = ACGColumnRiskTypeDescription
                    lColumnGeminiEDI = ACGColumnGeminiEDI
                Else
                    'SJ 18/10/2004 - end
                    lColumnPolicyNumber = ACAColumnPolicyNumber
                    lColumnPolicyType = ACAColumnPolicyType
                    lColumnRiskType = ACAColumnRiskType
                    lColumnRegarding = ACAColumnRegarding
                    lColumnRenewalDate = ACAColumnRenewalDate
                    lColumnInsurer = ACAColumnInsurer
                    lColumnPremium = ACAColumnPremium
                    lColumnPolicyStatus = ACAColumnPolicyStatus
                    lColumnRiskTypeDescription = ACAColumnRiskTypeDescription
                    lColumnGeminiEDI = ACAColumnGeminiEDI
                End If
            Else
                lColumnPolicyNumber = ACUColumnPolicyNumber
                lColumnPolicyType = ACUColumnPolicyType
                lColumnRiskType = ACUColumnRiskType
                lColumnRegarding = ACUColumnRegarding
                lColumnRenewalDate = ACUColumnRenewalDate
                lColumnInsurer = ACUColumnInsurer
                lColumnPremium = ACUColumnPremium
                lColumnPolicyStatus = ACUColumnPolicyStatus
                lColumnRiskTypeDescription = ACUColumnRiskTypeDescription
                lColumnGeminiEDI = ACUColumnGeminiEDI
            End If
            ' {* USER DEFINED CODE (Begin) *}
            lblClient.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblStatus.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchDetails.Columns.Item(lColumnPolicyNumber - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitlePolicyNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchDetails.Columns.Item(lColumnRegarding - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitleRegarding, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchDetails.Columns.Item(lColumnRenewalDate - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitleRenewalDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'RWH(10/04/2001) For UW we display Agent rather than Insurer.
            If m_oBusiness.UnderwritingOrAgency = "U" Then
                lvwSearchDetails.Columns.Item(lColumnInsurer - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitleAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else
                lvwSearchDetails.Columns.Item(lColumnInsurer - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitleInsurer, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
            lvwSearchDetails.Columns.Item(lColumnRiskTypeDescription - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitleRiskTypeDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchDetails.Columns.Item(lColumnPremium - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitlePremium, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchDetails.Columns.Item(lColumnPolicyStatus - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitlePolicyStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwSearchDetails.Columns.Item(lColumnPolicyType - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitlePolicyType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If m_oBusiness.UnderwritingOrAgency = "U" Then
                lvwSearchDetails.Columns.Item(lColumnRiskType - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitleProductName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else
                lvwSearchDetails.Columns.Item(lColumnRiskType - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitleRiskType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            If m_bGeminiIILink Then
                Select Case m_lFindType
                    Case 1
                        lvwSearchDetails.Columns.Item(lColumnRenewalDate - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitleStartDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                End Select
                lvwSearchDetails.Columns.Item(lColumnGeminiEDI - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitleEDI, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            'SJ 18/10/2004 - start
            If m_lFindType = 1 Then
                lvwSearchDetails.Columns.Item(lColumnStoredInd - 1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitleStoredInd, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
            'SJ 18/10/2004 - end
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            ' {* USER DEFINED CODE (End) *}
            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", excep:=excep)

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



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: CancelListPolicy
    '
    ' Description: Called when we wish to cancel any changes
    '
    ' ***************************************************************** '
    Private Function CancelListPolicy() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                'Me.Hide
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Cancel the ListPolicy", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelListPolicy", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectListPolicy
    '
    ' Description: Called when we wish to select
    '
    ' ***************************************************************** '
    Private Function SelectListPolicy() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                '        unloadInterface
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Select the ListPolicy", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectListPolicy", excep:=excep)

            Return result

        End Try
    End Function
    'eck190500
    ' ***************************************************************** '
    ' Name: GetCompany (Standard Method)
    '
    ' Description: Gets valid Source ID's  and if nessessary displays selection
    '
    ' ***************************************************************** '
    Public Function GetCompany(ByRef m_iCompanyID As Integer) As Integer
        Dim result As Integer = 0

        'NIIT - Replaced with the Migrated code 1144 
        'Dim m_oBranch As ClassInterface
        Dim m_oBranch As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_m_oBranch As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBranch, sClassName:="iPMBBranch.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oBranch = temp_m_oBranch

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oBranch.GetSource(iSourceID:=m_iCompanyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oBranch.Dispose()
            m_oBranch = Nothing
            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("m_iCompanyID", m_iCompanyID)
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Company", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompany", excep:=excep, oDicParms:=oDict)

            Return result

        End Try
    End Function
    'eck190500
    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Private Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if form has been cancelled, if so, prompt
            ' if you wish to lose details.
            If Status = gPMConstants.PMEReturnCode.PMCancel Then
                ' Get string messages
                'developer guide no.243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    ' Set return to false, meaning
                    ' don't cancel.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                ' its a cancel, so set STEPSTATUS to INCOMPLETE...

                'm_lReturn& = m_frmInterface.SetStatus(PMNavStatusIncomplete, PMNavStatusIncomplete, PMNavStatusIncomplete)

            Else
                ' Update the property member from the interface.
                m_lReturn = DataToProperties()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to update business.
                    Return result
                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""

        Try
            ' Get message text if not already present.
            If sMessage = "" Then

                'developer guide no. 243
                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            stbStatus.Text = " " & sMessage

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", excep:=excep)

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


        Try

            '    ' Store the total of item found.
            '    If (IsArray(m_vSearchData) = False) Then
            '        lItemsFound& = 0
            '    Else
            '        lItemsFound& = (UBound(m_vSearchData, 2) + 1)
            '    End If

            ' Get message text if not already present.
            If sMessage = "" Then

                'developer guide no. 243
                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            stbStatus.Text = " " & m_lItemsFound & " " & sMessage

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", excep:=excep)

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
    'UPGRADE_NOTE: (7001) The following declaration (CheckMandatory) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckMandatory() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Check all fields for data.
    '
    'If txtClientCode.Text.Trim() <> "" Then
    'Return gPMConstants.PMEReturnCode.pmtrue
    'End If
    '
    'If txtClientCode.Text.Trim() <> "" Then
    'Return gPMConstants.PMEReturnCode.pmtrue
    'End If
    '
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogExcepMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for mandatory fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: ResizeInterface
    '
    ' Description: Resizes the interface controls.
    '
    ' ***************************************************************** '
    Private Function ResizeInterface() As Integer

        Try

            If Not m_bResizing Then

                m_bResizing = True

                tabMainTab.Height = MyBase.Height - VB6.TwipsToPixelsY(600)
                tabMainTab.Width = MyBase.Width - VB6.TwipsToPixelsX(120)

                lvwSearchDetails.Height = tabMainTab.Height - VB6.TwipsToPixelsY(1750)
                lvwSearchDetails.Width = tabMainTab.Width - VB6.TwipsToPixelsX(360)

                'stbStatus.Top = lvwSearchDetails.Height + 70
                'stbStatus.Left = 5
                stbStatus.Width = lvwSearchDetails.Width


                m_bResizing = False

            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            ' Error Section.


            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function
    ' PRIVATE Methods (End)

    Private Sub cboStatus_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboStatus.SelectedIndexChanged

        m_lReturn = DataToInterface()
        ' Check the return values.
        Select Case (m_lReturn)
            Case gPMConstants.PMEReturnCode.PMTrue
                ' Found search details.

            Case gPMConstants.PMEReturnCode.PMNotFound
                ' No search details found.

            Case Else
                ' Failed to get details.

                ' Log Error.
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")


        End Select

        ' Display the number of item found message.
        DisplayStatusFound()

        ' Tell interface this has changed, so it disables OK button
        RaiseEvent cboStatusChange(Me, Nothing)

    End Sub

    Private Sub lvwSearchDetails_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSearchDetails.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)


        m_lSelected = gPMConstants.PMEReturnCode.PMTrue

        'ED22072002 - Remove conditional clause for Underwriting only
        If lvwSearchDetails.GetItemAt(x, y) Is Nothing Then
            m_lSelected = gPMConstants.PMEReturnCode.PMFalse

            ' CJB050902 To prevent empty grid errors when you click on it then do not
            ' raise event so exit instead
            Exit Sub
        End If

        RaiseEvent lvwSearchDetailsMouseDown(Me, New lvwSearchDetailsMouseDownEventArgs(m_lSelected))

    End Sub

    'sj 02/10/2002 - start
    Private Sub Toolbar1_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles TB_Event.Click, _Toolbar1_Button2.Click, TB_RiskDetails.Click, _Toolbar1_Button4.Click, TB_InformationChecklist.Click, TB_Policy.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        Dim lInsuranceFileCnt, lInsuranceFolderCnt, lIndex As Integer
        Dim sInsuranceRef As String = ""

        'developer guide no. ToolBar Button name Changed.
        If Button.Name = "TB_Policy" Then
            ShowPolicy(v_lPartyCnt:=m_lInsHolderCnt, v_sPartyShortName:=m_sShortName)
        Else

            If lvwSearchDetails.Items.Count = 0 Then
                Exit Sub
            End If

            If Not lvwSearchDetails.FocusedItem.Selected Then
                Exit Sub
            End If

            lIndex = CInt(Conversion.Val(Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)))

            lInsuranceFileCnt = CInt(Conversion.Val(CStr(m_vSearchData(ACIInsFileCnt, lIndex))))
            lInsuranceFolderCnt = CInt(Conversion.Val(CStr(m_vSearchData(ACIInsFolderCnt, lIndex))))
            sInsuranceRef = CStr(m_vSearchData(ACIInsReference, lIndex)).Trim()


            Select Case Button.Name
                'developer guide no. Button name changed from "Event" to "TB_Event"
                Case "TB_Event"

                    m_lReturn = iPMBListEvents.ShowEvents(v_lPartyCnt:=m_lInsHolderCnt, v_sTransactionType:=m_sTransactionType, v_lInsuranceFileCnt:=lInsuranceFileCnt, v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_sInsuranceRef:=sInsuranceRef)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ShowEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Toolbar1_ButtonClick")
                        Exit Sub
                    End If
            End Select
        End If

    End Sub
    'sj 02/10/2002 - end

    ' PRIVATE Events (Begin)

    Private Sub uctListPolicy_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        Try

            m_lReturn = ResizeInterface()

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub lvwSearchDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Click

        Dim sShortName As String = ""
        Dim iCount As Integer

        If lvwSearchDetails.Items.Count > 0 Then
            sShortName = lvwSearchDetails.FocusedItem.Text

            ' loop around and get the other details...
            For iCount = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                If CStr(m_vSearchData(ACIInsReference, iCount)).Trim() = sShortName Then
                    iCount += 1
                    Exit For
                End If
            Next iCount

            ' stick the other details in here...?

            ' Activate View button

            RaiseEvent lvwSearchDetailsMouseDown(Me, New lvwSearchDetailsMouseDownEventArgs(iCount))

        End If

    End Sub

    Private Sub lvwSearchDetails_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwSearchDetails.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim sShortName As String = ""
        Dim iCount As Integer

        If lvwSearchDetails.Items.Count > 0 Then
            sShortName = lvwSearchDetails.FocusedItem.Text

            ' loop around and get the other details...
            For iCount = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                If CStr(m_vSearchData(ACIInsReference, iCount)).Trim() = sShortName Then
                    iCount += 1
                    Exit For
                End If
            Next iCount

            RaiseEvent lvwSearchDetailsMouseDown(Me, New lvwSearchDetailsMouseDownEventArgs(iCount))

        End If

    End Sub

    Private Sub lvwSearchDetails_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwSearchDetails.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        '    If (KeyCode <> 13) Then
        '        cmdOK.Default = False
        '    End If

    End Sub

    'NIIT - Replaced with the Migrated code 1144 
    'Private Sub lvwSearchDetails_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles lvwSearchDetails.KeyPress
    '	Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

    '	Dim sShortName As String = ""

    '	If KeyAscii = 13 Then

    '		sShortName = lvwSearchDetails.FocusedItem.Text

    '		' loop around and get the other details...
    '		For iCount As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
    '			If CStr(m_vSearchData(ACIInsReference, iCount)) = sShortName Then
    '				Exit For
    '			End If
    '		Next iCount

    '		' stick the other details in here...?

    '	End If

    '	If KeyAscii = 0 Then
    '		eventArgs.Handled = True
    '	End If
    '	eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    'End Sub




    Public Function CancelClick() As Integer

        ' Click event of the Cancel button.

        Try


            Return CancelListPolicy()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelClick", excep:=excep)

            Exit Function

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ShowHelpScreen
    '
    ' Description: Shows the help screen
    '
    ' ***************************************************************** '
    Public Function ShowHelpScreen(Optional ByRef cmdHelp As Object = Nothing, Optional ByRef ScreenHelpID As Object = Nothing) As Integer
        ' Fire up the help screen
        Return SSfunc.ShowHelp(cmdHelp, ScreenHelpID)
    End Function
    ' ***************************************************************** '
    ' Name: OKClick
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function OKClick() As Integer

        Dim result As Integer = 0
        Try


            'CT 16/08/00 start
            'OKClick = SelectListPolicy()

            'The ok button on parent form in client manager has been clicked
            'so now act as if doubleclick on this component were pressed
            m_bOKClick = True
            m_bDontProceedMarkedForCollection = False
            lvwSearchDetails_DoubleClick(lvwSearchDetails, New EventArgs())

            'WPR12- Enhancement Quote Collection Process
            If m_bDontProceedMarkedForCollection Then
                Return result
            End If
            'CT 16/08/00 end 'CT 31/08/00 note that lvwSearchDetails_DblClick will set m_lReturn


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OKClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OKClick", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyClick
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function CopyClick() As Integer

        Dim result As Integer = 0
        Dim lNewInsuranceFileCnt, lPartyCnt As Integer
        Dim sResolvedName, sShortName, sPartyType As String


        'NIIT - Replaced with the Migrated code 1144 
        'Dim oListPolicy As ClassInterface
        Dim oListPolicy As Object
        Dim vKeyArray, sLongName As Object

        'NIIT - Replaced with the Migrated code 1144 
        'Dim oFindParty As ClassInterface
        Dim oFindParty As Object
        Dim sOption As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset the insurance file cnt
            NewInsuranceFileCnt = 0

            m_lInsFileCnt = 0
            m_lInsuranceFolderCnt = 0

            'WPR12- Enhancement Quote Collection Process
            m_bDontProceedMarkedForCollection = False

            'MSS280801 - Only Call the DblClick if we have selected an item
            If lvwSearchDetails.Items.Count > 0 Then
                If lvwSearchDetails.FocusedItem.Selected Then
                    lvwSearchDetails_DoubleClick(lvwSearchDetails, New EventArgs())
                End If
            End If

            'WPR12- Enhancement Quote Collection Process
            If m_bDontProceedMarkedForCollection Then
                Return result
            End If

            'Identify If Policy Already Selected For Copy

            If m_lInsFileCnt = 0 And m_lInsuranceFolderCnt = 0 Then

                'Select Party From Where Policy Is To Be Copied

                Dim temp_oFindParty As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                oFindParty = temp_oFindParty

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Set the short name

                oFindParty.ShortName = m_sShortName

                ' Start the component

                m_lReturn = oFindParty.Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If oFindParty.Status = gPMConstants.PMEReturnCode.PMCancel Then
                    Return gPMConstants.PMEReturnCode.PMCancel
                End If

                ' Get the party_cnt

                lPartyCnt = oFindParty.PartyCnt

                sShortName = oFindParty.ShortName

                sResolvedName = oFindParty.ResolvedName


                sLongName = oFindParty.LongName

                sPartyType = oFindParty.PartyType

                ' Remove the instance of find party

                oFindParty.Dispose()

                oFindParty = Nothing

                'List Policies And Select One To Copy For Selected Client

                Dim temp_oListPolicy As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oListPolicy, sClassName:="iPMBListPolicy.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                oListPolicy = temp_oListPolicy

                ReDim vKeyArray(1, 2)

                ' Insurance File Cnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lPartyCnt

                ' Insurance Folder Cnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameShortName

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = sShortName


                m_lReturn = oListPolicy.SetKeys(vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Start the component

                m_lReturn = oListPolicy.Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' MSS280801 - Commented out below. Was refering to wrong object
                ' and oListPolicy has no status property to check

                '       If (oFindParty.Status = PMCancel) Then
                '            CopyClick = PMCancel
                '            Exit Function
                '       End If


                m_lReturn = oListPolicy.GetKeys(vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the insurance file and folder count

                m_lInsFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0))

                m_lInsuranceFolderCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1))

                ' Remove the instance of find party

                oListPolicy.Dispose()

                oListPolicy = Nothing

            End If

            'TODO - Get CopyTextFiles System Option
            If sOption = "0" Then
                'Passing in zero for the source id will use the old policies source id.

                m_lReturn = m_oBusiness.CopyPolicyV2(v_lOldInsuranceFileCnt:=m_lInsFileCnt, r_lNewInsuranceFileCnt:=lNewInsuranceFileCnt, v_iSourceId:=0, v_lTargetPartyCnt:=m_lInsHolderCnt, v_lDontCopyTextFiles:=0)
            Else
                'Passing in zero for the source id will use the old policies source id.

                m_lReturn = m_oBusiness.CopyPolicyV2(v_lOldInsuranceFileCnt:=m_lInsFileCnt, r_lNewInsuranceFileCnt:=lNewInsuranceFileCnt, v_iSourceId:=0, v_lTargetPartyCnt:=m_lInsHolderCnt)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy policy", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyV2", excep:=New Exception(Information.Err().Description))
                Return result
            End If

            ' TODO - Check System Option 67 (Copy Risk)

            m_lReturn = m_oOption.getOption(iOptionNumber:=67, sValue:=sOption)

            If sOption = "1" Then
                ' Copy the risk (if one exists) too

                m_lReturn = m_oBusiness.CopyRisk(v_lOldInsuranceFileCnt:=m_lInsFileCnt, v_lNewInsuranceFileCnt:=lNewInsuranceFileCnt)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy risk", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyV2", excep:=New Exception(Information.Err().Description))
                Return result
            End If

            ' Pass the insurance file cnt back here (to preserve bin compat)
            NewInsuranceFileCnt = lNewInsuranceFileCnt

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClick", excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
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

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("vEffectiveDate", vEffectiveDate)
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", excep:=excep, oDicParms:=oDict)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        'Static bIsInitialised As Boolean
        Dim bIsInitialised As Boolean
        Dim sTitle, sMessage As String

        Dim sHelpFile As String = ""
        Dim m_lReturn As gPMConstants.PMEReturnCode
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if already initialised
            If bIsInitialised Then
                Return result
            End If

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' If UserID is 0 assume that user cancelled logon
            If g_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                g_oObjectManager = Nothing
                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                'eck220500
                g_iUserID = .UserID
            End With

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrieve Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
                'NIIT - Replaced with the Migrated code 1144 
                'App.HelpFile = sHelpFile
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRFindInsurance.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.

                'developer guide no. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            Dim temp_m_oOption As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oOption, "bSIROptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oOption = temp_m_oOption

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.

                'developer guide no. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            '    m_bGeminiLink = m_oBusiness.GeminiLink
            '    m_bGeminiIILink = m_oBusiness.GeminiIILink
            '    m_bSwiftLink = m_oBusiness.SwiftLink

            'TN20001214 - Start

            m_sUnderwritingOrAgency = m_oBusiness.UnderwritingOrAgency
            'TN20001214 - End

            'SJ 20/02/2004 - start
            m_lReturn = CType(CheckForUnderwritingBranch(v_iSourceId:=g_oObjectManager.SourceID, r_bUnderwritingBranchEnabled:=m_bUnderwritingBranchEnabled, r_bIsUnderwritingBranch:=m_bIsUnderwritingBranch), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForUnderwritingBranch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If
            'SJ 20/02/2004 - end

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            'developer guide no.107
            iPMBListEvents.g_oObjectManager = g_oObjectManager
            ' hold Initialised status
            bIsInitialised = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: LoadControl
    '
    ' Description: Does all the extra stuff that initialise doesn't
    '
    ' ***************************************************************** '
    Public Function LoadControl() As Integer

        Dim result As Integer = 0


        ' Forms load event.

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            ' {* USER DEFINED CODE (Begin) *}
            'eck090500
            If m_oValidSource Is Nothing Then
                m_lReturn = GetValidSources()
                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'eck090500
            ' {* USER DEFINED CODE (End) *}

            'SJ 08/04/2004 - start
            If m_oAllowBranches Is Nothing Then
                m_lReturn = AllowOtherBranchesToViewPolicies()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AllowOtherBranchesToViewPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")
                    Return result
                End If
            End If
            'SJ 08/04/2004 - end

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load control", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl", excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
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
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                m_oAllowBranches = Nothing
                m_oValidSource = Nothing
            End If


        End If
        Me.disposedValue = True
    End Sub

    Private Const vbFormCode As Integer = 0
    ' ***************************************************************** '
    ' Name: UnloadControl
    '
    ' Description: Cleans up then unloads the control
    '
    ' ***************************************************************** '
    Public Function UnLoadControl(ByRef Cancel As Integer, ByRef UnloadMode As Integer) As Integer

        ' Forms query unload event.

        Debug.WriteLine("unload control")

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            'NIIT - Replaced with the Migrated code 1144 

            If UnloadMode <> vbFormCode Then

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'eventArgs.cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Function
                End If
            End If


            Dispose()
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLoadControl", excep:=excep)

            Exit Function

        End Try

    End Function
    'eck090500
    ' ***************************************************************** '
    ' Name: GetValidSources (Standard Method)
    '
    ' Description: Calls the appropriate methods to get the Sources
    '              which the the current user can access
    '
    ' ***************************************************************** '
    Private Function GetValidSources() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oValidSource = New Hashtable()

            'David Kyle Thing
            'Call PMUser to get the Sources
            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oPMUser As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oPMUser = temp_g_oPMUser

            '    ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                '        ' Display error stating the problem.

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMUser.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

                Return result
            End If
            'eck220500


            m_lReturn = g_oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=g_iUserID, v_bIncludeDeletedSources:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get valid sources", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

                Return result
            End If
            '    ' Remove instance of PMUser
            If Not (g_oPMUser Is Nothing) Then

                g_oPMUser.Dispose()
                g_oPMUser = Nothing
            End If

            'SJ 08/04/2004 - start
            If Not Information.IsArray(m_vSourceArray) Then
                Return result
            End If
            'developer guide no. 
            For i As Integer = 0 To m_vSourceArray.GetUpperBound(1)
                'developer guide no. 
                m_oValidSource.Add(CStr(m_vSourceArray(0, i)), CStr(m_vSourceArray(0, i)))
            Next i
            'SJ 08/04/2004 - end

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.f
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' This will handle lvwSearchDetails_DoubleClick event 
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick
        Dim sOption As String = String.Empty
        Dim nNewInsuranceFileCnt As Integer = 0
        Dim sInsuranceRef As String = String.Empty
        Dim r_nDontProceed As Integer = 0
        Dim bIsMarketplacePolicy As Boolean = False
        Dim bIsReferredQuote As Boolean = False
        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMFalse

        Try

            m_bDontProceedMarkedForCollection = False

            m_lInsFileCnt = CInt(m_vSearchData(ACIInsFileCnt,
                                               ToSafeInteger(
                                                   lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)))

            nReturn = m_oBusiness.CheckIsMarketplacePolicy(nInsuranceFileKey:=m_lInsFileCnt,
                                                           o_bIsMarketplacePolicy:=bIsMarketplacePolicy,
                                                           o_bIsReferredQuote:=bIsReferredQuote)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.CheckIsMarketplacePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DoubleClick", excep:=New Exception(Information.Err().Description))
                Exit Sub
            End If

            If bIsMarketplacePolicy AndAlso Not bIsReferredQuote Then
                If MessageBox.Show(
                        "This is a Marketplace policy and all transactions should be initiated through the Marketplace channel. Are you sure you wish to proceed with this transaction?",
                        "Marketplace policy", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = DialogResult.OK Then
                    If MessageBox.Show("On completion of this transaction, please contact the SSP Marketplace team with details of the changes you have made and ensure these changes are replicated in Marketplace. Do you still wish to proceed with this transaction?",
                            "Marketplace policy", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = DialogResult.Cancel Then
                        m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                        Exit Sub
                    Else
                        nReturn = m_oBusiness.UpdateMarketplacePolicyStatus(nInsuranceFileKey:=m_lInsFileCnt,
                                                           o_bIsMarketplacePolicy:=False)
                    End If
                Else
                    m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                    Exit Sub
                End If
            End If

            'WPR12- Enhancement Quote Collection Process
            If m_sTransactionType = "NB" Then
                nReturn = CheckMarkedForCollection(r_iDontProceed:=r_nDontProceed)

                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                If r_nDontProceed = 1 Then
                    m_bDontProceedMarkedForCollection = True
                    Exit Sub
                End If
            End If

            If m_bCopyPolicy Then

                If Not m_bOKClick Then
                    Exit Sub
                End If

                If Strings.Len(txtPolicy.Text) > 0 Then
                    sInsuranceRef = txtPolicy.Text
                Else
                    If lvwSearchDetails.Items.Count > 0 Then
                        sInsuranceRef = CStr(m_vSearchData(ACIInsReference, Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag))).Trim()
                    Else
                        Exit Sub
                    End If
                End If


                m_lReturn = m_oBusiness.FindSingleRef(vResultArray:=m_vSingleSearchData, sInsuranceRef:=sInsuranceRef)


                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If nReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        MessageBox.Show("Please enter a valid policy", "Invalid Policy", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    Else
                        Throw New Exception()
                    End If
                End If

                m_lInsFileCnt = CInt(m_vSingleSearchData(0, 0))
                'm_lInsuranceFolderCnt& = CLng(m_vSingleSearchData(1, 0))

                nReturn = m_oOption.getOption(iOptionNumber:=67, sValue:=sOption)
                'TODO - Get CopyTextFiles System Option
                ' Call CopyPolicy
                If sOption = "0" Then
                    'Passing in zero for the source id will use the old policies source id.

                    nReturn = m_oBusiness.CopyPolicyV2(v_lOldInsuranceFileCnt:=m_lInsFileCnt, r_lNewInsuranceFileCnt:=nNewInsuranceFileCnt, v_iSourceId:=0, v_lTargetPartyCnt:=m_lInsHolderCnt, v_lDontCopyTextFiles:=1)
                Else
                    'Passing in zero for the source id will use the old policies source id.

                    nReturn = m_oBusiness.CopyPolicyV2(v_lOldInsuranceFileCnt:=m_lInsFileCnt, r_lNewInsuranceFileCnt:=nNewInsuranceFileCnt, v_iSourceId:=0, v_lTargetPartyCnt:=m_lInsHolderCnt)
                End If

                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'CopyClick = PMFalse
                    ' Log Error Message
                    gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy policy", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyV2", excep:=New Exception(Information.Err().Description))
                    Exit Sub
                End If

                ' TODO - Check System Option 67 (Copy Risk)

                nReturn = m_oOption.getOption(iOptionNumber:=67, sValue:=sOption)

                If sOption = "1" Then
                    ' Copy the risk (if one exists) too

                    nReturn = m_oBusiness.CopyRisk(v_lOldInsuranceFileCnt:=m_lInsFileCnt, v_lNewInsuranceFileCnt:=nNewInsuranceFileCnt)
                End If

                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'CopyClick = PMFalse
                    ' Log Error Message

                    '                    iPMFunc.LogExcepMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy risk", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyV2", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy risk", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyV2", excep:=New Exception(Information.Err().Description))
                    Exit Sub
                End If

                ' Pass the insurance file cnt back here (to preserve bin compat)
                NewInsuranceFileCnt = nNewInsuranceFileCnt
                m_lInsFileCnt = nNewInsuranceFileCnt

            Else
                ' Check if there are any items available.
                If lvwSearchDetails.Items.Count = 0 Then
                    Exit Sub
                End If

                'CT 16/08/00  nReturn = OKClick
                nReturn = SelectListPolicy() 'CT 16/08/00

                ' Check the return value.
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                Else

                End If

                RaiseEvent lvwSearchDetailsDblClick(Me, New lvwSearchDetailsDblClickEventArgs(m_lInsHolderCnt, m_lInsuranceFolderCnt, m_lInsFileCnt, m_sShortName, m_sInsReference, m_lPolicyTypeId))
            End If

        Catch ex As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", excep:=ex)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Enter

        ' GotFocus Event for the search details

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Set the default button
        'cmdOK.Default = True
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
        '
        ' Log Error.
        'iPMFunc.LogExcepMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Private Sub lvwSearchDetails_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Leave

        ' LostFocus Event for the search details

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Set the default button.
        'cmdFindNow.Default = True
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
        '
        ' Log Error.
        'iPMFunc.LogExcepMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDetails.Columns(eventArgs.Column)

        ' Column click event for the search details

        Try

            'DJM 15/10/2003 : Modified to use function similar to how Find Transaction does it.
            If lvwSearchDetails.Items.Count > 0 Then

                lvwSearchDetails.Items.Item(0).EnsureVisible()
                ' Column click event for the search details
                ' Defer to the common interface
                OnColumnClick(lvwSearchDetails, ColumnHeader, m_sUnderwritingOrAgency)
            End If

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", excep:=excep)

            Exit Sub

        End Try

    End Sub

    '*************************************************************************
    ' Name : ShowPolicy
    '
    ' Desc : launch client manager and show policy list
    '
    ' Auth : Thinh Nguyen 14/01/2004
    '*************************************************************************
    Private Sub ShowPolicy(ByRef v_lPartyCnt As Integer, ByRef v_sPartyShortName As String)


        Dim result As Integer = 0
        Dim sWhatFailed As String = ""
        Dim Component As iPMWrkComponentStarter.StartControl
        Dim lReturn As Integer
        Dim arrKeyArray(1, 6) As Object
        Dim r_sPartyType As String = ""
        Try



            m_lReturn = GetPartyType(v_lPartyCnt, r_sPartyType)
            If Not lvwSearchDetails.FocusedItem Is Nothing Then
                If lvwSearchDetails.FocusedItem.Index >= 0 Then
                    lReturn = DataToProperties()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to run DataToProperties : " & sWhatFailed, vApp:=ACApp, vClass:=ACClass, vMethod:="Show Policy")
                    End If
                End If
            End If
            result = gPMConstants.PMEReturnCode.PMTrue
            'Key Names
            arrKeyArray(0, 0) = PMNavKeyConst.PMKeyNameInsuranceFileCnt
            arrKeyArray(0, 1) = PMNavKeyConst.PMKeyNamePartyCnt
            arrKeyArray(0, 2) = PMNavKeyConst.PMKeyNamePartyType
            arrKeyArray(0, 3) = PMNavKeyConst.PMKeyNamePartyResolvedName
            arrKeyArray(0, 4) = PMNavKeyConst.PMKeyNameShortName
            arrKeyArray(0, 5) = PMNavKeyConst.PMKeyNameInsuranceFolderCnt
            arrKeyArray(0, 6) = PMNavKeyConst.PMKeyNameRunMode
            'Key Values
            arrKeyArray(1, 0) = 0
            arrKeyArray(1, 1) = v_lPartyCnt
            arrKeyArray(1, 2) = r_sPartyType
            arrKeyArray(1, 3) = ""
            arrKeyArray(1, 4) = v_sPartyShortName
            arrKeyArray(1, 5) = 0
            arrKeyArray(1, 6) = "POLICYLIST"
            ' Create Component Starter
            sWhatFailed = "Component Starter"

            Component = New iPMWrkComponentStarter.StartControl()
            'Developer Guide No. 9
            lReturn = Component.Initialise()

            Component.CallingAppName = ACApp
            Component.StartComponent(v_sComponent:="iPMBClientTaskWrapper.NavigatorV3", v_lPMAuthorityLevel:=2, v_vSetKeyArray:=arrKeyArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise iPMBClientTaskWrapper.NavigatorV3  : " & sWhatFailed, vApp:=ACApp, vClass:=ACClass, vMethod:="Show Policy")

            End If

        Catch excep As System.Exception
            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message			
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to run Show Policy" & sWhatFailed, vApp:=ACApp, vClass:=ACClass, vMethod:="Show Policy", excep:=excep)
        End Try

    End Sub
    ' PRIVATE Events (End)

    ' ***************************************************************** '
    '
    ' Name: AllowOtherBranchesToViewPolicies
    '
    ' Description:
    '
    ' History: 08/04/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function AllowOtherBranchesToViewPolicies() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vAllowBranchesArray(,) As Object
            Dim sSourceId, sValue As String

            m_oAllowBranches = New Hashtable()


            m_lReturn = m_oBusiness.AllowOtherBranchesToViewPolicies(r_vAllowBranchesArray:=vAllowBranchesArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AllowOtherBranchesToViewPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AllowOtherBranchesToViewPolicies")
                Return result
            End If

            If Not Information.IsArray(vAllowBranchesArray) Then
                Return result
            End If


            For i As Integer = 0 To vAllowBranchesArray.GetUpperBound(1)

                sSourceId = CStr(vAllowBranchesArray(0, i))

                sValue = CStr(vAllowBranchesArray(1, i))
                If sValue = "1" Then
                    m_oAllowBranches.Add(sSourceId, sValue)
                End If
            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AllowOtherBranchesToViewPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AllowOtherBranchesToViewPolicies", excep:=excep)

            Return result




            Return result
        End Try
    End Function

    Private Function GetPartyType(ByVal v_lPartyCnt As Integer, ByRef r_sPartyType As String) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetPartyType(v_lPartyCnt:=v_lPartyCnt, r_sPartyType:=r_sPartyType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get party type from business component", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyType", excep:=New Exception(Information.Err().Description))

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyType", excep:=excep)

            Return result




            Return result
        End Try
    End Function

    Public Function CheckMarkedForCollection(ByRef r_iDontProceed As Integer) As Integer
        Dim result As Integer = 0
        Dim bSIRFindInsurance As Object

        Const kMethodName As String = "CheckMarkedForCollection"

        Dim lReturn As Integer

        Dim oCheckMarkedForCollection As bSIRFindInsurance.Form
        Dim sInsuranceRef As String = ""
        Dim r_lIsMarked As Integer
        Dim r_dMarkedDate, dttodaydate As Date

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If lvwSearchDetails.Items.Count = 0 Then
                Return result
            End If

            If Strings.Len(txtPolicy.Text) > 0 Then
                sInsuranceRef = txtPolicy.Text
            Else
                If lvwSearchDetails.Items.Count > 0 Then
                    sInsuranceRef = CStr(m_vSearchData(ACIInsReference, Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag))).Trim()
                Else
                    Return result
                End If
            End If


            m_lReturn = m_oBusiness.FindSingleRef(vResultArray:=m_vSingleSearchData, sInsuranceRef:=sInsuranceRef)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            m_lInsFileCnt = CInt(m_vSingleSearchData(0, 0))

            Dim temp_oCheckMarkedForCollection As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oCheckMarkedForCollection, "bSIRFindInsurance.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oCheckMarkedForCollection = temp_oCheckMarkedForCollection
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oCheckMarkedForCollection.IsMarkedForCollection(v_lInsuranceFileCnt:=m_lInsFileCnt, r_lIsMarked:=r_lIsMarked, r_dMarkedDate:=r_dMarkedDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'developer guide no. 40
            dttodaydate = CDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=DateTime.Now))
            If r_lIsMarked = 1 Then
                'developer guide no. 40
                r_dMarkedDate = CDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=r_dMarkedDate))
                If r_dMarkedDate = dttodaydate Then
                    If MessageBox.Show("Quote already passed for collection process," & Strings.Chr(13) & Strings.Chr(10) & "do you wish to proceed ?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.Yes Then

                        m_lReturn = oCheckMarkedForCollection.UpdateMarkedForCollectionStatus(v_lInsuranceFileCnt:=m_lInsFileCnt, r_lIsMarked:=0)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        r_iDontProceed = 1
                    End If
                Else


                    m_lReturn = oCheckMarkedForCollection.UpdateMarkedForCollectionStatus(v_lInsuranceFileCnt:=m_lInsFileCnt, r_lIsMarked:=0)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If
            End If

            oCheckMarkedForCollection = Nothing


        Catch ex As Exception

            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMarkedForCollection Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMarkedForCollection", excep:=ex)

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    'developer guide no. 78
    Private Sub lvwSearchDetails_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lvwSearchDetails.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(e.KeyChar)
        Dim sShortName As String = ""

        If KeyAscii = 13 Then

            sShortName = lvwSearchDetails.FocusedItem.Text

            ' loop around and get the other details...
            For iCount As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                If m_vSearchData(ACIInsReference, iCount) = sShortName Then
                    Exit For
                End If
            Next iCount

            ' stick the other details in here...?

        End If

        If KeyAscii = 0 Then
            'developer guide no. 78
            e.Handled = True
        End If
        'developer guide no. 78
        e.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub tabMainTab_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabMainTab.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
            tabMainTab.Focus()
        End If
    End Sub

    Private Sub lvwSearchDetails_ItemSelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvwSearchDetails.ItemSelectionChanged
        Dim Item As ListViewItem = lvwSearchDetails.Items(e.Item.Index)
        Dim sShortName As String = ""
        Dim iCount As Integer
        If lvwSearchDetails.Items.Count > 0 Then
            m_ItemSelected = Item.Selected
            If lvwSearchDetails.FocusedItem IsNot Nothing Then
                sShortName = lvwSearchDetails.FocusedItem.Text
            End If
            ' loop around and get the other details...
            For iCount = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                If CStr(m_vSearchData(ACIInsReference, iCount)).Trim() = sShortName Then
                    iCount += 1
                    Exit For
                End If
            Next iCount
            ' stick the other details in here...?
            ' Activate View button
            RaiseEvent lvwSearchDetailsMouseDown(Me, New lvwSearchDetailsMouseDownEventArgs(iCount))
        End If
    End Sub
End Class
