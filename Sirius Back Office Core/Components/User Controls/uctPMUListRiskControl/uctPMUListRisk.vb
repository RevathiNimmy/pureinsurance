Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
Imports System.Runtime.InteropServices

<System.Runtime.InteropServices.ProgId("uctPMUListRisk_NET.uctPMUListRisk")>
Partial Public Class uctPMUListRisk
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event EffectiveDateChange()
    Public Event TransactionTypeChange()
    Public Event ProcessModeChange()
    Public Event StatusChange()
    Public Event TaskChange()
    Public Event CallingAppNameChange()
    ' ***************************************************************** '
    '
    ' Date: 12/09/2000
    '
    ' Description: Risk List User Control
    '
    ' Edit History: CT 12/09/00 - Created from PolicyListUser control
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "uctRiskList"

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
    Dim m_CoverNoteAttached As Integer

    Public Event RiskItemCheckChanged(ByVal Sender As Object, ByVal e As RiskItemCheckChangedEventArgs)
    Public Event AboutToChange(ByVal Sender As Object, ByVal e As EventArgs)
    'Start (Sriram P)Tech Spec - WR19 - Cover Note Functionality
    Dim vResultArrayCoverNote As Object
    'End (Sriram P)Tech Spec - WR19 - Cover Note Functionality
    'Event Declarations:
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs)
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs)
    Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs)
    Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs)
    Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs)
    Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs)
    Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs)
    Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs)

    'TN20010117 Start
    'added v_lIsReInsuranceAtRiskLevel and v_lInsuranceFolderCnt

    Event lvwSearchDetailsDblClick(ByVal Sender As Object, ByVal e As lvwSearchDetailsDblClickEventArgs)
    'TN20010117 End

    ' PW311002 - add the risk no, variation no and checkbox state to the event
    ' PW021202 - add existing risk flag to the event
    Event lvwSearchDetailsClick(ByVal Sender As Object, ByVal e As lvwSearchDetailsClickEventArgs)

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lInsFileCnt As Integer
    Private m_lRiskId As Integer
    Private m_sRiskDescription As String = ""
    Private m_sRiskTypeDescription As String = ""
    Private m_vRiskInceptionDate As Date
    Private m_vRiskExpiryDate As Date
    'Am 07122000
    Private m_sRiskStatus As String = ""
    Private m_vRiskTotalSI As Object
    Private m_vRiskTotalPremium As Object
    Private m_lRiskGisScreenId As Integer
    Private m_lRiskTypeId As Integer

    Private m_bOKToProceed As Boolean
    Private m_bPendingReinsurance As Boolean
    Private m_bQuoteAll As Boolean
    Private m_lInsHolderCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_sShortName As String = ""
    Private m_lProductID As Integer

    Private m_sInsReference As String = ""

    Private m_lPartyUIK As Integer
    Private m_lPolicyUIK As Integer
    Private m_vLeadAgentCnt As Object
    'Tomo150300
    Private m_lPolicyTypeId As Integer
    Private m_sPolicyType As String = ""

    ' TF311298 - changed from NavProcessCode
    Private m_sInsFileType As String = ""
    'sj 5/11/99 - start
    Private m_bDisableInsFileType As Boolean
    'sj 5/11/99 - end
    'eck090500
    Private m_vSourceArray As Object

    ' PW221102
    ' PS411
    Private m_bWhenTaxesRequired As Boolean

    ' {* USER DEFINED CODE (End) *}


    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    Private m_oUserAuthorities As Object 'bACTUserAuthorities.Business

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
    Public m_vSearchData As Object
    Public m_vInsuranceFileData As Object
    ' PW021202 - stores the original search data
    Public m_vOriginalSearchData As Object

    Private m_bGeminiLink As Boolean
    Private m_bGeminiIILink As Boolean
    Private m_bSwiftLink As Boolean

    Private m_lFindType As Integer
    Private m_lPolicyType As Integer

    'TN20010111
    Private m_lIsReInsuranceAtRiskLevel As Integer

    Private m_bAllowOverride As Boolean

    Private m_bDeleted As Boolean

    'Flag for Item.Checked
    Private m_bItemChanged As Boolean

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    Private m_bCurrencyChanged As Boolean
    Private m_abEdittedArray() As Boolean
    Private m_bEditted As Boolean
    Private m_bCurrenciesUpdated As Boolean

    Private m_crTotalPremium As Decimal
    Private m_crTotalTax As Decimal
    Private m_crTotalFeeTax As Decimal
    Private m_crTotalFeePremium As Decimal
    Private m_lNoOfSelectedQuotedRisks As Integer
    Private m_bIsApplyDiscount As Boolean
    Private m_bCancelAboutToChangeAction As Boolean
    Private m_lDiscountedRiskCount As Integer
    Private m_bLoading As Boolean

    Private m_bIsCNDetailsChanges As Boolean
    Private m_vCoverNote As Object
    Private hScrollValue As Integer = 0
    Private m_iSelectedIndex As Integer = -1
    Private m_bFormLoading As Boolean = False
    Dim m_bAllRiskStatusQuoted As Boolean = False
    'Win32 API declarations to preserve list view horizontal scroll position after sort
    Const LVM_FIRST As Int32 = &H1000
    Const LVM_SCROLL As Int32 = LVM_FIRST + 20
    Const SBS_HORZ As Integer = 0

    Const ACFindImage As String = "FindImage"
    Const ACDiscountImage As String = "Discount"
    'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
    Const ACTick As String = "tick"

    'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
    Public Property FormLoading() As Boolean
        Get
            Return m_bFormLoading
        End Get
        Set(ByVal Value As Boolean)
            m_bFormLoading = Value
        End Set
    End Property
    Public Property AllRiskStatusQuoted() As Boolean
        Get
            Return m_bAllRiskStatusQuoted
        End Get
        Set(ByVal Value As Boolean)
            m_bAllRiskStatusQuoted = Value
        End Set
    End Property
    <Browsable(False)>
    Public ReadOnly Property DiscountedRiskCount() As Integer
        Get
            Return m_lDiscountedRiskCount
        End Get
    End Property

    <Browsable(False)>
    Public WriteOnly Property CancelAboutToChangeAction() As Boolean
        Set(ByVal Value As Boolean)
            m_bCancelAboutToChangeAction = Value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property IsApplyDiscount() As Boolean
        Get
            Return m_bIsApplyDiscount
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property TotalNoSelectedQuotedRisks() As Integer
        Get
            GetNoSelectedQuotedRisks()
            Return m_lNoOfSelectedQuotedRisks
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property TotalFeePremium() As Decimal
        Get
            GetTotalFeePremium()
            Return m_crTotalFeePremium
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property TotalFeeTax() As Decimal
        Get
            GetTotalFeeTax()
            Return m_crTotalFeeTax
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property TotalPremium() As Decimal
        Get
            GetTotalPremium()
            Return m_crTotalPremium
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property TotalTax() As Decimal
        Get
            GetTotalTax()
            Return m_crTotalTax
        End Get
    End Property

    '
    ' PW311002 - return an array of risk cnt/selected flags
    '
    <Browsable(False)>
    Public ReadOnly Property RiskSelectionStatus() As Object
        Get

            Dim result As Object = Nothing
            Dim vReturn(,) As Object

            ' Check if there are any items in the list
            If lvwSearchDetails.Items.Count < 1 Then
                'Developer Guide No 28 (no solution)
                'Return ""
                RiskSelectionStatus = Nothing
                vResultArrayCoverNote = Nothing
                Return result
            End If

            ' Build the array
            ReDim vReturn(1, lvwSearchDetails.Items.Count - 1)

            For i As Integer = 0 To lvwSearchDetails.Items.Count - 1

                vReturn(0, i) = m_vSearchData(ACIRiskId, CInt(Conversion.Val(Convert.ToString(lvwSearchDetails.Items.Item(i - 0).Tag))))


                'vReturn(1, i) = lvwSearchDetails.Items.Item(i - 1).Checked
                vReturn(1, i) = lvwSearchDetails.Items.Item(i - 0).Checked
            Next
            result = vReturn
            vResultArrayCoverNote = vReturn

            Return result
        End Get
    End Property

    <Browsable(False)>
    Public WriteOnly Property PolicyType() As Integer
        Set(ByVal Value As Integer)
            m_lPolicyType = Value
        End Set
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
    '
    ' PW311002 - get number of selected risks
    '
    <Browsable(False)>
    Public ReadOnly Property SelectedRiskCount() As Integer
        Get


            ' PW311002 - count the number of selected items
            Dim iCount As Integer = 0
            For i As Integer = 1 To lvwSearchDetails.Items.Count
                If lvwSearchDetails.Items.Item(i - 1).Checked Then
                    iCount += 1
                End If
            Next

            Return iCount

        End Get
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
    <Browsable(True)>
    Public Property InsFileCnt() As Integer
        Get
            Return m_lInsFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsFileCnt = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property RiskID() As Integer
        Get
            Return m_lRiskId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskId = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property RiskDescription() As String
        Get
            Return m_sRiskDescription
        End Get
        Set(ByVal Value As String)
            m_sRiskDescription = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property RiskTypeDescription() As String
        Get
            Return m_sRiskTypeDescription
        End Get
        Set(ByVal Value As String)
            m_sRiskTypeDescription = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property RiskInceptionDate() As Date
        Get
            Return m_vRiskInceptionDate
        End Get
        Set(ByVal Value As Date)
            m_vRiskInceptionDate = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property RiskExpiryDate() As Date
        Get
            Return m_vRiskExpiryDate
        End Get
        Set(ByVal Value As Date)
            m_vRiskExpiryDate = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property RiskStatus() As String
        Get
            Return m_sRiskStatus
        End Get
        Set(ByVal Value As String)
            m_sRiskStatus = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property RiskTotalSI() As Object
        Get
            Return m_vRiskTotalSI
        End Get
        Set(ByVal Value As Object)
            m_vRiskTotalSI = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property RiskTotalPremium() As Object
        Get
            Return m_vRiskTotalPremium
        End Get
        Set(ByVal Value As Object)


            m_vRiskTotalPremium = Value
        End Set
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
    Public Property ProductID() As Integer
        Get
            Return m_lProductID
        End Get
        Set(ByVal Value As Integer)
            m_lProductID = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property FindType() As Integer
        Set(ByVal Value As Integer)
            m_lFindType = Value
        End Set
    End Property


    'TN20001208 - START
    <Browsable(False)>
    Public ReadOnly Property RiskCount() As Integer
        Get

            Return lvwSearchDetails.Items.Count

        End Get
    End Property
    'TN20001208 - END

    <Browsable(False)>
    Public ReadOnly Property OKToProceed() As Boolean
        Get
            Return m_bOKToProceed
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property QuoteAll() As Boolean
        Get
            Return m_bQuoteAll
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property PendingReinsurance() As Boolean
        Get
            Return m_bPendingReinsurance
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property Deleted() As Boolean
        Get
            Return m_bDeleted
        End Get
    End Property

    <Browsable(False)>
    Public WriteOnly Property Editted() As Boolean
        Set(ByVal Value As Boolean)
            m_bEditted = Value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property CurrenciesUpdated() As Boolean
        Get
            'Only return false if currency has changed and selected risks haven't been editted.
            Return m_bCurrenciesUpdated Or Not m_bCurrencyChanged
        End Get
    End Property

    <DllImport("user32.dll")>
    Private Shared Function GetScrollPos(ByVal hWnd As System.IntPtr, ByVal nBar As Integer) As Integer

    End Function
    <DllImport("user32.dll")>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As Boolean

    End Function
    <DllImport("user32.dll")>
    Private Shared Function LockWindowUpdate(ByVal Handle As IntPtr) As Boolean

    End Function
    'Store the horizontal scroll value.
    Private Sub StoreHScrollValue()
        hScrollValue = GetScrollPos(lvwSearchDetails.Handle, SBS_HORZ)
    End Sub
    'Recover the old scroll position
    Private Sub RecoverHorizontalScroll()
        LockWindowUpdate(lvwSearchDetails.Handle)
        'Calculate the value the scroll needs to scroll back.
        Dim dx As Integer = hScrollValue - GetScrollPos(lvwSearchDetails.Handle, SBS_HORZ)
        'Send the scroll message.
        Dim b As Boolean = SendMessage(lvwSearchDetails.Handle, LVM_SCROLL, dx, 0)
        LockWindowUpdate(IntPtr.Zero)

    End Sub


    ''' <summary>
    ''' CheckIfRiskExists
    ''' </summary>
    ''' <param name="v_iArrayIndex"></param>
    ''' <param name="r_bExistingRisk"></param>
    ''' <param name="r_bMandatoryRisk"></param>
    ''' <param name="r_bIsRiskSelected"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckIfRiskExists(ByVal v_iArrayIndex As Integer, ByRef r_bExistingRisk As Boolean,
                                       Optional ByRef r_bMandatoryRisk As Boolean = False,
                                       Optional ByRef r_bIsRiskSelected As Boolean = False,
                                       Optional ByRef r_bEditable As Boolean = False,
                                       Optional ByRef r_sRiskStatus As String = "") As Integer

        Dim nResult As Integer = 0

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Check if the current risk was in the original list
            r_bExistingRisk = False

            If Information.IsArray(m_vOriginalSearchData) Then
                For icnt As Integer = m_vOriginalSearchData.GetLowerBound(1) To m_vOriginalSearchData.GetUpperBound(1)

                    If icnt <= m_vSearchData.GetUpperBound(1) AndAlso (Conversion.Val(CStr(m_vSearchData(ACIIsSelected, icnt))) = 1) Then
                        r_bIsRiskSelected = True
                    End If
                    If _
                        Conversion.Val(CStr(m_vOriginalSearchData(ACIRiskNo, icnt))) =
                        Conversion.Val(CStr(m_vSearchData(ACIRiskNo, v_iArrayIndex))) AndAlso
                        Conversion.Val(CStr(m_vOriginalSearchData(ACIVariationNo, icnt))) =
                        Conversion.Val(CStr(m_vSearchData(ACIVariationNo, v_iArrayIndex))) Then
                        r_bExistingRisk = True
                        Exit For
                    End If
                Next
            End If

            If IsArray(m_vSearchData) Then

                For icnt As Integer = LBound(m_vSearchData, 2) To _
                    UBound(m_vSearchData, 2)
                    If Val(m_vSearchData(ACIIsMandatoryRisk, icnt)) = 1 AndAlso icnt = v_iArrayIndex Then
                        r_bMandatoryRisk = True
                        Exit For
                    End If
                Next
            End If

            If IsArray(m_vSearchData) Then
                For nCnt As Integer = LBound(m_vSearchData, 2) To _
                    UBound(m_vSearchData, 2)
                    If Val(m_vSearchData(kIEditable, nCnt)) = 1 And nCnt = v_iArrayIndex Then
                        r_bEditable = True
                        Exit For
                    End If
                Next
            End If
            If IsArray(m_vSearchData) Then
                For nCnt As Integer = LBound(m_vSearchData, 2) To _
                    UBound(m_vSearchData, 2)
                    If ToSafeString(m_vSearchData(ACIRiskStatus, nCnt)).ToUpper <> "QUOTED" And nCnt = v_iArrayIndex Then
                        r_sRiskStatus = ToSafeString(m_vSearchData(ACIRiskStatus, nCnt)).ToUpper
                        Exit For
                    End If
                Next
                If r_sRiskStatus = "" Then
                    r_sRiskStatus = "QUOTED"
                End If
            End If

            Return nResult

        Catch excep As System.Exception


            nResult = gPMConstants.PMEReturnCode.PMError


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check if the risk exists.", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIfRiskExists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function

    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)

    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: GetGetRisks
    '
    ' Description: Gets the interface details/Risks and sets the appropriate
    '              style.
    '
    ' ***************************************************************** '
    ' PW021202 - add original flag
    Public Function GetRisks(Optional ByVal v_bOriginalFlag As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the interface details from the business object.
            m_lReturn = GetBusiness(v_bOriginalFlag:=v_bOriginalFlag)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the Risks", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRisks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    ' PW021202 - add original flag
    Public Function GetBusiness(ByVal v_bOriginalFlag As Boolean) As Integer

        Dim result As Integer = 0

        Dim vOverrideDate, vOverrideRate As Object

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

            m_lReturn = m_oBusiness.SearchInsuranceFile(r_vResultArray:=m_vInsuranceFileData, v_vInsuranceFileCnt:=InsFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso m_lReturn <> PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                If m_vInsuranceFileData IsNot Nothing AndAlso IsArray(m_vInsuranceFileData) Then
                    m_lProductID = CInt(m_vInsuranceFileData(0, 0))
                End If
            End If

            'Get UserAuthorities business object
            m_lReturn = CreateUserAuthorities()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to run function, CreateUserAuthorities")
            End If

            'Get authority details for current user.

            m_lReturn = m_oUserAuthorities.GetDetails(vUserID:=g_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to run function, m_oUserAuthorities.GetDetails")
            End If

            'Get override options for current user.

            m_lReturn = m_oUserAuthorities.GetNext(vOverrideDate:=vOverrideDate, vOverrideRate:=vOverrideRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to run function, m_oUserAuthorities.GetNext")
            End If



            m_bAllowOverride = CInt(vOverrideDate) = 1 Or CInt(vOverrideRate) = 1

            ' Get data

            m_lReturn = m_oBusiness.SearchAll(r_vResultArray:=m_vSearchData, v_vInsuranceFileCnt:=InsFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Not Information.IsArray(m_vOriginalSearchData) Then
                m_vOriginalSearchData = VB6.CopyArray(m_vSearchData)
            End If

            If Information.IsArray(m_vSearchData) Then
                ReDim Preserve m_abEdittedArray(m_vSearchData.GetUpperBound(1) + 1)
            End If

            m_lReturn = DataToInterface()

            ' {* USER DEFINED CODE (End) *}

            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.
                    If m_iSelectedIndex > 0 AndAlso lvwSearchDetails.Items.Count - 1 >= m_iSelectedIndex Then
                        'lvwSearchDetails.SelectedItems.Clear()
                        'lvwSearchDetails.FocusedItem = lvwSearchDetails.Items(m_iSelectedIndex)
                        lvwSearchDetails.Items(m_iSelectedIndex).EnsureVisible()
                        lvwSearchDetails.Items(m_iSelectedIndex).Selected = True
                        lvwSearchDetails.Items(m_iSelectedIndex).Focused = True
                    End If
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




            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Updates all interface details from the search data storage.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DataToInterface() As Integer

        Dim nResult As Integer
        Dim oListItem As ListViewItem
        Dim bSearchSet As Boolean
        Dim cPremium As Decimal
        Dim bIsDiscounted As Boolean
        Dim sRiskLinkStatus As String
        Const kCNFound As String = "1"
        Const kCNNotFound As String = "0"

        Try
            m_bLoading = True

            nResult = PMEReturnCode.PMTrue

            ' Update the interface details.
            ' Clear the search details.
            lvwSearchDetails.Items.Clear()
            'ListViewFunc.ListViewBatchStart(lvwSearchDetails)
            m_lItemsFound = gPMConstants.PMEFormatStyle.PMFormatString

            cPremium = 0
            m_bOKToProceed = True
            m_bPendingReinsurance = False
            m_bCurrenciesUpdated = True
            m_bQuoteAll = False
            m_bIsApplyDiscount = False

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPremium, vControlValue:=cPremium)
                Return nResult
            End If

            ' Assign the details to the interface.
            ' WR19 - Cover Note Functionality section()
            ReDim m_vCoverNote(ACDataCNCount, 0)
            m_lDiscountedRiskCount = 0
            m_bAllRiskStatusQuoted = True
            For nRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                ' Assign the details to the first column.
                m_lItemsFound = CType(m_lItemsFound + 1, PMEFormatStyle)

                ' determine if this row has been discount - 1 is discounted / 0 is not discounted
                bIsDiscounted = ToSafeLong(m_vSearchData(ACIRiskIsDiscounted, nRow), 0)

                ' AM 061200 - Do not display the actual risk ID, just the folder icon
                ' Column 1 RiskId
                m_bLoading = True

                ' highlight discounted risks
                If bIsDiscounted Then
                    oListItem = lvwSearchDetails.Items.Add("", ACDiscountImage)
                    ' increment the discounted risk counter
                    m_lDiscountedRiskCount += 1
                Else
                    oListItem = lvwSearchDetails.Items.Add("", ACFindImage)
                End If

                ' PW311002 - Set the checkbox state
                oListItem.Checked = (Val(CStr(m_vSearchData(ACIIsSelected, nRow))) = 1)

                m_bLoading = False

                If CDbl(m_vSearchData(ACIRiskId, nRow)) = m_lRiskId AndAlso m_bEditted Then
                    m_abEdittedArray(nRow) = True
                    m_bEditted = False
                End If

                If oListItem.Checked And Not m_abEdittedArray(nRow) Then
                    m_bCurrenciesUpdated = False
                End If

                'Editted
                If m_abEdittedArray(nRow) Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosEditted).Text = "Yes"
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosEditted).Text = "No"
                End If

                ' Column 2, Risk No

                If Convert.IsDBNull(m_vSearchData(ACIRiskNo, nRow)) OrElse IsNothing(m_vSearchData(ACIRiskNo, nRow)) Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskNo).Text = ""
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskNo).Text = CStr(m_vSearchData(ACIRiskNo, nRow)).Trim()
                End If

                ReDim Preserve m_vCoverNote(ACDataCNCount, nRow)

                m_vCoverNote(ACDataPosCNRiskId, nRow) =
                    ToSafeLong(BlankToNull(m_vSearchData(ACIRiskId, nRow)))
                m_vCoverNote(ACDataPosCNRef, nRow) =
                    ToSafeString(BlankToNull(m_vSearchData(ACICNRef, nRow)))
                m_vCoverNote(ACDataPosCNFrom, nRow) = BlankToNull(m_vSearchData(ACICNFrom, nRow))
                m_vCoverNote(ACDataPosCNTo, nRow) = BlankToNull(m_vSearchData(ACICNTo, nRow))
                m_vCoverNote(ACDataPosRowTag, nRow) = nRow

                If m_sTransactionType = "MTC" Then
                    Dim subitm As New ListViewItem.ListViewSubItem

                    Dim oAuxVar As Object = BlankToNull(m_vSearchData(ACICNNoteId, nRow))
                    If Not (Convert.IsDBNull(oAuxVar) OrElse IsNothing(oAuxVar)) Then
                        m_vCoverNote(ACDataPosCNAttach, nRow) = True
                        subitm.Text = "T"
                        oListItem.SubItems.Insert(ACColPosCNAttached, subitm)
                        oListItem.SubItems.Item(ACColPosCNAttached).Tag = kCNFound
                    Else
                        m_vCoverNote(ACDataPosCNAttach, nRow) = False
                        subitm.Text = ""
                        oListItem.SubItems.Insert(ACColPosCNAttached, subitm)
                        oListItem.SubItems.Item(ACColPosCNAttached).Tag = kCNNotFound
                    End If
                Else
                    Dim oAuxVar_2 As Object = BlankToNull(m_vSearchData(ACICNNoteId, nRow))
                    Dim subitm As New ListViewItem.ListViewSubItem
                    subitm.Text = "T"
                    If Convert.IsDBNull(oAuxVar_2) OrElse IsNothing(oAuxVar_2) OrElse
                        CStr(m_vSearchData(ACIRiskStatusCode, nRow)).Trim() <> "QUOTED" Then

                        m_vCoverNote(ACDataPosCNAttach, nRow) = False
                        subitm.Text = ""
                        oListItem.SubItems.Insert(ACColPosCNAttached, subitm)
                        oListItem.SubItems.Item(ACColPosCNAttached).Tag = kCNNotFound
                    Else
                        m_vCoverNote(ACDataPosCNAttach, nRow) = True
                        subitm.Text = "T"
                        oListItem.SubItems.Insert(ACColPosCNAttached, subitm)
                        oListItem.SubItems.Item(ACColPosCNAttached).Tag = kCNFound
                    End If
                End If

                ' discounted
                If bIsDiscounted Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosDiscounted).Text = "Yes"
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosDiscounted).Text = "No"
                End If

                ' Column 3, Risk Variation No

                If Convert.IsDBNull(m_vSearchData(ACIVariationNo, nRow)) OrElse IsNothing(m_vSearchData(ACIVariationNo, nRow)) Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosVariationNo).Text = ""
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosVariationNo).Text =
                        CStr(m_vSearchData(ACIVariationNo, nRow)).Trim()
                End If

                ' AM 061200 - Add new column for risk status
                ' Column 4, Risk Status

                If Convert.IsDBNull(m_vSearchData(ACIRiskStatus, nRow)) OrElse IsNothing(m_vSearchData(ACIRiskStatus, nRow)) _
                    Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text = ""
                Else
                    If (CStr(m_vSearchData(ACIRiskStatusFlag, nRow)) = "D") AndAlso CStr(m_vSearchData(ACIRiskStatus, nRow)).Trim() = "Quoted" Then
                        ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text = "Deleted"
                    ElseIf CStr(m_vSearchData(ACIRiskStatus, nRow)).Trim() = "Pending Reinsurance" AndAlso m_sTransactionType = "PT" Then
                        ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text = "Pending Reinsurance Due To Portfolio Transfer"
                    ElseIf CStr(m_vSearchData(ACIRiskStatus, nRow)).Trim() = "Pending Reinsurance" AndAlso m_sTransactionType = "DRI" Then
                        ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text = "Pending Reinsurance Due To Clone Rework"
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text =
                            CStr(m_vSearchData(ACIRiskStatus, nRow)).Trim()
                    End If
                End If

                If ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text = "Unquoted" Then
                    m_bQuoteAll = True
                End If

                If _
                    ((ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text = "Referred") OrElse
                     (ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text = "Declined") OrElse
                     (ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text = "Unquoted") OrElse
                     (ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text = "") OrElse
                     (ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text = "Pending Reinsurance") OrElse
                      (ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text = "Pending Reinsurance Due To Portfolio Transfer") OrElse
                       (ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text = "Pending Reinsurance Due To Clone Transfer")) AndAlso
                    oListItem.Checked Then
                    m_bOKToProceed = False
                End If
                If (ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text.ToUpper <> "QUOTED") AndAlso (ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text <> "Quoted - Reinsurance Deferred") AndAlso (ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text.ToUpper <> "DELETED") AndAlso m_bAllRiskStatusQuoted Then
                    m_bAllRiskStatusQuoted = False
                    m_bOKToProceed = False
                End If

                If ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text = "Pending Reinsurance" Then
                    m_bLoading = True
                    oListItem.Checked = False
                    m_bLoading = False
                End If

                ' PW311002 - only set flag to false if this is a selected risk
                If (ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskStatus).Text = "Pending Reinsurance") AndAlso oListItem.Checked Then
                    m_bPendingReinsurance = True
                End If

                ' Column 5, Coverage

                If Convert.IsDBNull(m_vSearchData(ACICoverage, nRow)) OrElse IsNothing(m_vSearchData(ACICoverage, nRow)) _
                    Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosCoverage).Text = ""
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosCoverage).Text =
                        CStr(m_vSearchData(ACICoverage, nRow)).Trim()
                End If

                ' Column 6, Insured Item

                If Convert.IsDBNull(m_vSearchData(ACIInsuredItem, nRow)) OrElse IsNothing(m_vSearchData(ACIInsuredItem, nRow)) Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosInsuredItem).Text = ""
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosInsuredItem).Text =
                        CStr(m_vSearchData(ACIInsuredItem, nRow)).Trim()
                End If

                ' Column 7, Extensions

                If Convert.IsDBNull(m_vSearchData(ACIExtensions, nRow)) OrElse IsNothing(m_vSearchData(ACIExtensions, nRow)) _
                    Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosExtensions).Text = ""
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosExtensions).Text =
                        CStr(m_vSearchData(ACIExtensions, nRow)).Trim()
                End If

                ' Column 8, Risk total Sum Insured

                If Convert.IsDBNull(m_vSearchData(ACIRiskTotalSumInsured, nRow)) OrElse IsNothing(m_vSearchData(ACIRiskTotalSumInsured, nRow)) Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosSumInsured).Text = ""
                Else
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCurrency,
                                                            vControlValue:=m_vSearchData(ACIRiskTotalSumInsured, nRow))

                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosSumInsured).Text = txtCurrency.Text
                End If

                'If we're doing an MTA we don't have any additional premiums, fees or taxes for unchanged risks
                If CStr(m_vSearchData(ACIRiskStatusFlag, nRow)) = "U" OrElse CStr(m_vSearchData(ACIRiskStatusFlag, nRow) = "R") Then
                    m_vSearchData(ACIRiskTotalAnnualPremium, nRow) = 0
                    m_vSearchData(ACIRiskTax, nRow) = 0
                    m_vSearchData(ACIFeePremium, nRow) = 0
                    m_vSearchData(ACIFeeTax, nRow) = 0
                End If

                ' fee tax
                ListViewHelper.GetListViewSubItem(oListItem, ACColPosFeeTax).Text =
                    StringsHelper.Format(Conversion.Val(CStr(m_vSearchData(ACIFeeTax, nRow))), "0.00")

                ' fee premium
                ListViewHelper.GetListViewSubItem(oListItem, ACColPosFeePremium).Text =
                    StringsHelper.Format(Conversion.Val(CStr(m_vSearchData(ACIFeePremium, nRow))), "0.00")

                ' Column -Tax
                ListViewHelper.GetListViewSubItem(oListItem, ACColPosTax).Text =
                    StringsHelper.Format(Conversion.Val(CStr(m_vSearchData(ACIRiskTax, nRow))), "0.00")

                ' Column 9, Risk Total Annual Premium
                If _
                    Convert.IsDBNull(m_vSearchData(ACIRiskTotalAnnualPremium, nRow)) OrElse
                    IsNothing(m_vSearchData(ACIRiskTotalAnnualPremium, nRow)) Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosPremium).Text = ""
                Else
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCurrency,
                                                            vControlValue:=
                                                               m_vSearchData(ACIRiskTotalAnnualPremium, nRow))

                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosPremium).Text = txtCurrency.Text

                    Dim dbNumericTemp As Double
                    If _
                        Double.TryParse(CStr(m_vSearchData(ACIRiskTotalAnnualPremium, nRow)), NumberStyles.Number,
                                        CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) AndAlso oListItem.Checked _
                        Then
                        cPremium += CDec(m_vSearchData(ACIRiskTotalAnnualPremium, nRow))
                    End If
                End If

                ' Column 10, Risk Inception Date

                If Convert.IsDBNull(m_vSearchData(ACIRiskInceptionDate, nRow)) OrElse IsNothing(m_vSearchData(ACIRiskInceptionDate, nRow)) Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosStartDate).Text = ""
                Else
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate,
                                                            vControlValue:=m_vSearchData(ACIRiskInceptionDate, nRow))

                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosStartDate).Text = txtDate.Text
                End If

                ' Column 11, Risk Expiry Date

                If Convert.IsDBNull(m_vSearchData(ACIRiskExpiryDate, nRow)) OrElse IsNothing(m_vSearchData(ACIRiskExpiryDate, nRow)) Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosEndDate).Text = ""
                Else
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate,
                                                            vControlValue:=m_vSearchData(ACIRiskExpiryDate, nRow))

                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosEndDate).Text = txtDate.Text
                End If

                ' Column 12, Risk description

                If _
                    Convert.IsDBNull(m_vSearchData(ACIRiskDescription, nRow)) OrElse
                    IsNothing(m_vSearchData(ACIRiskDescription, nRow)) Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskDesc).Text = ""
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskDesc).Text =
                        CStr(m_vSearchData(ACIRiskDescription, nRow)).Trim()
                End If

                ' Column 13, Risk Type description

                If _
                    Convert.IsDBNull(m_vSearchData(ACIRiskTypeDescription, nRow)) OrElse
                    IsNothing(m_vSearchData(ACIRiskTypeDescription, nRow)) Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskTypeDesc).Text = ""
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskTypeDesc).Text =
                        CStr(m_vSearchData(ACIRiskTypeDescription, nRow)).Trim()
                End If

                ' Column 14, Gis Screen (invisible)

                If _
                    Convert.IsDBNull(m_vSearchData(ACIRiskGisScreen, nRow)) OrElse
                    IsNothing(m_vSearchData(ACIRiskGisScreen, nRow)) Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosGISScreen).Text = ""
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosGISScreen).Text =
                        CStr(m_vSearchData(ACIRiskGisScreen, nRow)).Trim()
                End If

                ' Column 15, RiskType Id (invisible)

                If Convert.IsDBNull(m_vSearchData(ACIRiskTypeId, nRow)) OrElse IsNothing(m_vSearchData(ACIRiskTypeId, nRow)) _
                    Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskTypeID).Text = ""
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosRiskTypeID).Text =
                        CStr(m_vSearchData(ACIRiskTypeId, nRow)).Trim()
                End If
                'Start - Sankar - (WR29 - Stamp Duty Process) - Paralleling

                If _
                    Convert.IsDBNull(m_vSearchData(ACIStampDutyInsurer, nRow)) OrElse
                    IsNothing(m_vSearchData(ACIStampDutyInsurer, nRow)) Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosStampInsurer).Text = ""
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosStampInsurer).Text =
                        CStr(m_vSearchData(ACIStampDutyInsurer, nRow)).Trim()
                End If

                If _
                    Convert.IsDBNull(m_vSearchData(ACIStampDutyInsured, nRow)) OrElse
                    IsNothing(m_vSearchData(ACIStampDutyInsured, nRow)) Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosStampInsured).Text = ""
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, ACColPosStampInsured).Text =
                        CStr(m_vSearchData(ACIStampDutyInsured, nRow)).Trim()
                End If

                If Convert.IsDBNull(m_vSearchData(kIRiskLinkStatus, nRow)) Then
                    ListViewHelper.GetListViewSubItem(oListItem, kColPosRiskLinkStatus).Text = ""
                Else
                    Select Case Trim(m_vSearchData(kIRiskLinkStatus, nRow))
                        Case "U"
                            sRiskLinkStatus = "Unchanged"
                        Case "C"
                            If ToSafeLong(m_vSearchData(kIOriginalRiskCnt, nRow), -1) = -1 Then
                                sRiskLinkStatus = "Added"
                            Else
                                sRiskLinkStatus = "Changed"
                            End If
                        Case "D"
                            sRiskLinkStatus = "Deleted"
                        Case "R"
                            sRiskLinkStatus = "Renewed"
                        Case Else
                            sRiskLinkStatus = Trim(m_vSearchData(kIRiskLinkStatus, nRow))
                    End Select
                    ListViewHelper.GetListViewSubItem(oListItem, kColPosRiskLinkStatus).Text = sRiskLinkStatus
                End If

                If Convert.IsDBNull(m_vSearchData(kIRiskLingDate, nRow)) Then
                    ListViewHelper.GetListViewSubItem(oListItem, kColPosRiskLingDate).Text = ""
                Else
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate,
                                                            vControlValue:=m_vSearchData(kIRiskLingDate, nRow))

                    ListViewHelper.GetListViewSubItem(oListItem, kColPosRiskLingDate).Text = txtDate.Text
                End If

                If Convert.IsDBNull(m_vSearchData(ACIRiskFolderCnt, nRow)) OrElse IsNothing(m_vSearchData(ACIRiskFolderCnt, nRow)) Then
                    ListViewHelper.GetListViewSubItem(oListItem, kColPosRiskFolderKey).Text = ""
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, kColPosRiskFolderKey).Text = CStr(m_vSearchData(ACIRiskFolderCnt, nRow)).Trim()
                End If

                ' PW311002 end: Change order of columns and add new ones
                '
                If CStr(m_vSearchData(ACIRiskStatusFlag, nRow)) = "D" Then

                    oListItem.ForeColor = Color.Gray
                End If

                ' Set the tag property with the index of
                ' the search data storage.
                oListItem.Tag = CStr(nRow)

                ' Refresh the first X amount of rows, to
                ' allow the user to see the results instantly.
                If m_lItemsFound = PMEFormatStyle.PMListRefreshValue Then
                    If Not bSearchSet Then
                        ' Select the first item.
                        If m_iSelectedIndex <= 0 Then lvwSearchDetails.Items.Item(0).Selected = True

                        ' Refresh the initial results.
                        lvwSearchDetails.Refresh()
                        bSearchSet = True
                    End If
                End If

                m_lCoverNoteUpTo = ToSafeLong(m_vSearchData(ACICoverNoteUpTo, nRow))
            Next nRow

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPremium, vControlValue:=cPremium)

            ' Enable the interface now that the search
            ' has completed.

            m_lReturn = DisableInterface(bDisable:=False)

            ' Check for errors
            If m_lReturn <> PMEReturnCode.PMTrue Then
                ' Failed to get details.
                nResult = PMEReturnCode.PMFalse
            End If

            ListViewFunc.ListViewBatchEnd()
            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        Finally

            m_bLoading = False

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

            'Can't set properties if theres nothing in the list, so just exit.

            If lvwSearchDetails.Items.Count = 0 Then
                Return result
            End If

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            If Not IsNothing(lvwSearchDetails.FocusedItem) Then
                lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)
            Else
                lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(0).Tag)
            End If


            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}


            If Convert.IsDBNull(m_vSearchData(ACIInsFileCnt, lSelectedItem)) Or IsNothing(m_vSearchData(ACIInsFileCnt, lSelectedItem)) Then
                m_lInsFileCnt = 0
            Else
                m_lInsFileCnt = CInt(m_vSearchData(ACIInsFileCnt, lSelectedItem))
            End If


            If Convert.IsDBNull(m_vSearchData(ACIRiskId, lSelectedItem)) Or IsNothing(m_vSearchData(ACIRiskId, lSelectedItem)) Then
                m_lRiskId = 0
            Else
                m_lRiskId = CInt(CStr(m_vSearchData(ACIRiskId, lSelectedItem)).Trim())
            End If


            If Convert.IsDBNull(m_vSearchData(ACIRiskDescription, lSelectedItem)) Or IsNothing(m_vSearchData(ACIRiskDescription, lSelectedItem)) Then
                m_sRiskDescription = ""
            Else
                m_sRiskDescription = CStr(m_vSearchData(ACIRiskDescription, lSelectedItem)).Trim()
            End If


            If Convert.IsDBNull(m_vSearchData(ACIRiskTypeDescription, lSelectedItem)) Or IsNothing(m_vSearchData(ACIRiskTypeDescription, lSelectedItem)) Then
                m_sRiskTypeDescription = ""
            Else
                m_sRiskTypeDescription = CStr(m_vSearchData(ACIRiskTypeDescription, lSelectedItem)).Trim()
            End If


            If Convert.IsDBNull(m_vSearchData(ACIRiskInceptionDate, lSelectedItem)) Or IsNothing(m_vSearchData(ACIRiskInceptionDate, lSelectedItem)) Then

            Else
                m_vRiskInceptionDate = CDate(CStr(m_vSearchData(ACIRiskInceptionDate, lSelectedItem)).Trim())
            End If


            If Convert.IsDBNull(m_vSearchData(ACIRiskExpiryDate, lSelectedItem)) Or IsNothing(m_vSearchData(ACIRiskExpiryDate, lSelectedItem)) Then
            Else
                m_vRiskExpiryDate = CDate(CStr(m_vSearchData(ACIRiskExpiryDate, lSelectedItem)).Trim())
            End If
            'AM 0712200 - add risk status

            If Convert.IsDBNull(m_vSearchData(ACIRiskStatus, lSelectedItem)) Or IsNothing(m_vSearchData(ACIRiskStatus, lSelectedItem)) Then
                m_sRiskStatus = ""
            Else
                m_sRiskStatus = CStr(m_vSearchData(ACIRiskStatus, lSelectedItem)).Trim()
            End If


            If Convert.IsDBNull(m_vSearchData(ACIRiskTotalSumInsured, lSelectedItem)) Or IsNothing(m_vSearchData(ACIRiskTotalSumInsured, lSelectedItem)) Then

                m_vRiskTotalSI = 0
            Else
                'Tomo100401
                'This causes an overflow error when the value is over 2,147,483,647
                'Of course currencies have problems when we get to 922,337,203,685,477.5807
                '        m_vRiskTotalSI = CLng(m_vSearchData(ACIRiskTotalSumInsured, lSelectedItem&))

                m_vRiskTotalSI = m_vSearchData(ACIRiskTotalSumInsured, lSelectedItem)
            End If


            If Convert.IsDBNull(m_vSearchData(ACIRiskTotalAnnualPremium, lSelectedItem)) Or IsNothing(m_vSearchData(ACIRiskTotalAnnualPremium, lSelectedItem)) Then

                m_vRiskTotalPremium = 0
            Else

                m_vRiskTotalPremium = m_vSearchData(ACIRiskTotalAnnualPremium, lSelectedItem)
            End If


            If Convert.IsDBNull(m_vSearchData(ACIRiskGisScreen, lSelectedItem)) Or IsNothing(m_vSearchData(ACIRiskGisScreen, lSelectedItem)) Then
                m_lRiskGisScreenId = 0
            Else
                m_lRiskGisScreenId = CInt(m_vSearchData(ACIRiskGisScreen, lSelectedItem))
            End If


            If Convert.IsDBNull(m_vSearchData(ACIRiskTypeId, lSelectedItem)) Or IsNothing(m_vSearchData(ACIRiskTypeId, lSelectedItem)) Then
                m_lRiskTypeId = 0
            Else
                m_lRiskTypeId = CInt(m_vSearchData(ACIRiskTypeId, lSelectedItem))
            End If

            'TN20010111 Start

            m_lIsReInsuranceAtRiskLevel = m_oBusiness.IsRIAtRiskLevel(m_lRiskTypeId)
            'TN20010111 End

            'TN20010117 Start
            m_lInsuranceFolderCnt = CInt(m_vSearchData(ACIInsuranceFolderCnt, lSelectedItem))
            'TN20010117 End

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add something here to set the default status to current

            ' {* USER DEFINED CODE (Begin) *}



            ' Set the column widths for the search list.
            ' PW311002 - columns reordered and new columns added
            lvwSearchDetails.Columns.Item(ACColPosIsSelected).Width = CInt(VB6.TwipsToPixelsX(550))
            If m_bCurrencyChanged Then
                lvwSearchDetails.Columns.Item(ACColPosEditted).Width = CInt(VB6.TwipsToPixelsX(765))
            Else
                lvwSearchDetails.Columns.Item(ACColPosEditted).Width = CInt(0)
            End If

            lvwSearchDetails.Columns.Item(ACColPosRiskNo).Width = CInt(VB6.TwipsToPixelsX(500))
            'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
            lvwSearchDetails.Columns.Item(ACColPosCNAttached).Width = CInt(VB6.TwipsToPixelsX(500))
            'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()

            lvwSearchDetails.Columns.Item(ACColPosDiscounted).Width = CInt(VB6.TwipsToPixelsX(1200))
            lvwSearchDetails.Columns.Item(ACColPosVariationNo).Width = CInt(VB6.TwipsToPixelsX(550))
            lvwSearchDetails.Columns.Item(ACColPosRiskStatus).Width = CInt(VB6.TwipsToPixelsX(1200))
            lvwSearchDetails.Columns.Item(ACColPosCoverage).Width = CInt(VB6.TwipsToPixelsX(1100))
            lvwSearchDetails.Columns.Item(ACColPosInsuredItem).Width = CInt(VB6.TwipsToPixelsX(1300))
            lvwSearchDetails.Columns.Item(ACColPosExtensions).Width = CInt(VB6.TwipsToPixelsX(1100))
            lvwSearchDetails.Columns.Item(ACColPosSumInsured).Width = CInt(VB6.TwipsToPixelsX(1300))
            lvwSearchDetails.Columns.Item(ACColPosFeeTax).Width = CInt(VB6.TwipsToPixelsX(1100))
            lvwSearchDetails.Columns.Item(ACColPosFeePremium).Width = CInt(VB6.TwipsToPixelsX(1100))
            lvwSearchDetails.Columns.Item(ACColPosTax).Width = CInt(VB6.TwipsToPixelsX(1100))
            lvwSearchDetails.Columns.Item(ACColPosPremium).Width = CInt(VB6.TwipsToPixelsX(1100))
            lvwSearchDetails.Columns.Item(ACColPosStartDate).Width = CInt(VB6.TwipsToPixelsX(1700))
            lvwSearchDetails.Columns.Item(ACColPosEndDate).Width = CInt(VB6.TwipsToPixelsX(1700))
            lvwSearchDetails.Columns.Item(ACColPosRiskDesc).Width = CInt(VB6.TwipsToPixelsX(1700))
            lvwSearchDetails.Columns.Item(ACColPosRiskTypeDesc).Width = CInt(VB6.TwipsToPixelsX(1200))
            lvwSearchDetails.Columns.Item(ACColPosGISScreen).Width = CInt(0)
            lvwSearchDetails.Columns.Item(ACColPosRiskTypeID).Width = CInt(0)
            lvwSearchDetails.Columns.Item(kColPosRiskLinkStatus).Width = CInt(VB6.TwipsToPixelsX(1200))
            lvwSearchDetails.Columns.Item(kColPosRiskLingDate).Width = CInt(VB6.TwipsToPixelsX(1700))
            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

            Return nResult

        Catch excep As System.Exception




            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult


        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CheckInsFileType
    '
    ' Description:
    '
    '
    ' ***************************************************************** '

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
    'result = gPMConstants.PMEReturnCode.PMTrue
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
    ' {* USER DEFINED CODE (Begin) *}
    '
    '
    ' Set to the first tab.
    'tabMainTab.Tab = 0
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
            'ReDim m_ctlTabFirstLast(1, 2)
            ReDim m_ctlTabFirstLast(0, 0)
            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Display all language specific captions.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DisplayCaptions() As Integer

        Dim nResult As Integer
        Try

            nResult = PMEReturnCode.PMTrue

            lvwSearchDetails.Columns.Item(ACColPosIsSelected).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColRisk, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Item(ACColPosEditted).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColEditted, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Item(ACColPosRiskNo).Text = "C/N"

            lvwSearchDetails.Columns.Item(ACColPosRiskNo).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColRiskNo, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Item(ACColPosDiscounted).Text = "Discounted"

            lvwSearchDetails.Columns.Item(ACColPosVariationNo).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColRiskVar, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Item(ACColPosRiskStatus).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColRiskStatus, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Item(ACColPosCoverage).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColCoverage, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Item(ACColPosInsuredItem).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColInsuredItem, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Item(ACColPosExtensions).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColExtensions, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Item(ACColPosSumInsured).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColSumInsured, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Item(ACColPosFeeTax).Text = "Fee Tax"
            lvwSearchDetails.Columns.Item(ACColPosFeePremium).Text = "Fee Premium"
            lvwSearchDetails.Columns.Item(ACColPosTax).Text = "Risk Tax"

            lvwSearchDetails.Columns.Item(ACColPosPremium).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColPremium, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Item(ACColPosStartDate).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColStartDate, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Item(ACColPosEndDate).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColEndDate, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Item(ACColPosRiskDesc).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColRiskDesc, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Item(ACColPosRiskTypeDesc).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColRiskTypeDesc, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            txtPremium.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPremiumTitle, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Item(kColPosRiskLinkStatus).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kColRiskLinkStatusTitle, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Item(kColPosRiskLingDate).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kColRiskLinkDateTitle, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return nResult

        Catch excep As Exception

            nResult = PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CancelListRisk
    '
    ' Description: Called when we wish to cancel any changes
    '
    ' ***************************************************************** '
    Private Function CancelListRisk() As Integer

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Cancel the ListPolicy", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelListRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteRisk
    '
    ' Description: Called when we wish to cancel any changes
    '
    ' ***************************************************************** '
    Private Function DeleteRisk() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.DeleteRisk(lInsuranceFileCnt:=m_lInsFileCnt, lRiskID:=m_lRiskId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Cancel the ListPolicy", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectListRisk
    '
    ' Description: Called when we wish to select
    '
    ' ***************************************************************** '
    Private Function SelectListRisk() As Integer

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Select the ListRisk", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectListRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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

                'If we are creating a transaction then warn user.
                'Otherwise we are probably just in ClientManager and don't want the message.
                If m_sTransactionType <> "" Then

                    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                    ' Check message result.
                    If iMsgResult = System.Windows.Forms.DialogResult.No Then
                        ' Set return to false, meaning
                        ' don't cancel.
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

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


        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' PW051102
    ' ***************************************************************** '
    ' Name: CheckForMandatoryRisks
    '
    ' Description: For Admiral Only - Check that a Banking Risk has been
    ' added and either a Retail or Trade Risk
    '
    ' Note this should not be used for any other deployment other than
    ' Admiral
    ' APS   02-12-02    Amended so the risk checking process uses the new
    '                   field values.
    ' ***************************************************************** '
    Public Function CheckForMandatoryRisks() As Integer
        Dim result As Integer = 0
        Dim sBankingCode, sRetailCode, sTradeCode As String
        Dim vResult(,) As Object
        Dim bBankingFound, bTradeOrRetail As Boolean
        Dim sBankingDescription, sRetailDescription, sTradeDescription As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get Risk Types via Registry

            m_lReturn = m_oBusiness.GetRiskTypes(sBankingCode, sRetailCode, sTradeCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Determine Risk Description Text

            m_lReturn = m_oBusiness.GetRiskDescription(sBankingCode, sRetailCode, sTradeCode, vResult)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Extract Descriptions

            For iCount As Integer = vResult.GetLowerBound(1) To vResult.GetUpperBound(1)

                Select Case CStr(vResult(0, iCount)).Trim()
                    Case sBankingCode.Trim()
                        sBankingDescription = CStr(vResult(1, iCount)).Trim()
                    Case sRetailCode.Trim()
                        sRetailDescription = CStr(vResult(1, iCount)).Trim()
                    Case sTradeCode.Trim()
                        sTradeDescription = CStr(vResult(1, iCount)).Trim()
                End Select
            Next iCount

            ' Check that values were returned
            If sBankingCode = "" Or sRetailCode = "" Or sTradeCode = "" Then
                ' Log Error.
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error extracting risk descriptions", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckForMandatoryRisks", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            bBankingFound = False
            bTradeOrRetail = False
            For Each lstItem As ListViewItem In lvwSearchDetails.Items
                If lstItem.Checked Then


                    ' Loop through and check if a Banking Risk has been added, flag if it has not
                    If ListViewHelper.GetListViewSubItem(lstItem, ACColPosRiskDesc).Text.Trim() = sBankingDescription Then
                        ' Check that it is quoted
                        If ListViewHelper.GetListViewSubItem(lstItem, ACColPosVariationNo).Text.Trim().ToUpper() = "QUOTED" Then
                            bBankingFound = True
                        End If
                    End If

                    ' Loop through and check if a Retail or Trade Risk has been added
                    If ListViewHelper.GetListViewSubItem(lstItem, ACColPosRiskDesc).Text.Trim() = sRetailDescription Or ListViewHelper.GetListViewSubItem(lstItem, ACColPosRiskDesc).Text.Trim() = sTradeDescription Then
                        ' Check that it is quoted
                        If ListViewHelper.GetListViewSubItem(lstItem, ACColPosVariationNo).Text.Trim().ToUpper() = "QUOTED" Then
                            bTradeOrRetail = True
                        End If
                    End If
                End If
            Next lstItem


            ' Alert the user if no banking risk has been added
            If Not bBankingFound Then
                MessageBox.Show("A quoted banking details risk must be added", "Risk Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Alert the user if no Retail or Trade Risk has been added
            If Not bTradeOrRetail Then
                MessageBox.Show("A quoted Tradesman Policy or Retail risk must be added", "Risk Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to run CheckForMandatoryRisks", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLoadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
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
    '
    ' Check all fields for data.
    '
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for mandatory fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lNewWidth, lNewHeight As Integer

            lNewWidth = CInt(VB6.PixelsToTwipsX(MyBase.Width))
            lNewHeight = CInt(VB6.PixelsToTwipsY(MyBase.Height))

            If lNewWidth > 0 Then
                lvwSearchDetails.Width = VB6.TwipsToPixelsX(lNewWidth)
            End If

            If lNewHeight > 0 Then
                lvwSearchDetails.Height = VB6.TwipsToPixelsY(lNewHeight)
            End If

            Return result

        Catch



            ' Error Section.


            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Private Sub lvwSearchDetails_ItemClick(ByVal Item As ListViewItem)

        ' PW021202
        Dim bExistingRisk As Boolean
        'WPR 33-75 added
        Dim bMandatoryRisk As Boolean
        Dim bEditable As Boolean
        Dim sRiskStatus As String = ""
        Try
            ' Wpr53 : pass mandatory risk indicator in below event
            If Item Is Nothing Then
                'WPR 33-75 added
                RaiseEvent lvwSearchDetailsClick(Me, New lvwSearchDetailsClickEventArgs(False, 0, 0, 0, 0, 0, False, False, False, ""))
            Else


                'Developer Guide No 12 (no solution)
                m_bDeleted = Item.ForeColor = Color.Gray
                ' PW021202 - check if this is an existing risk
                'WPR 33-75 added

                m_lReturn = CheckIfRiskExists(v_iArrayIndex:=Conversion.Val(Convert.ToString(Item.Tag)),
                                              r_bExistingRisk:=bExistingRisk,
                                              r_bMandatoryRisk:=bMandatoryRisk,
                                              r_bEditable:=bEditable,
                                              r_sRiskStatus:=sRiskStatus)

                ' PW311002 - add the risk/variation no to the parameter list
                'WPR 33-75 added
                RaiseEvent lvwSearchDetailsClick(Me, New lvwSearchDetailsClickEventArgs(True, CInt(m_vSearchData(ACIRiskId, Convert.ToString(Item.Tag))),
                                                                                        CInt(m_vSearchData(ACIRiskGisScreen, Convert.ToString(Item.Tag))),
                                                                                        CInt(m_vSearchData(ACIRiskTypeId, Convert.ToString(Item.Tag))),
                                                                                        Conversion.Val(CStr(m_vSearchData(ACIRiskNo, Convert.ToString(Item.Tag)))),
                                                                                        Conversion.Val(CStr(m_vSearchData(ACIVariationNo, Convert.ToString(Item.Tag)))),
                                                                                        bExistingRisk,
                                                                                        bMandatoryRisk,
                                                                                        bEditable, sRiskStatus))

                m_lRiskId = CInt(m_vSearchData(ACIRiskId, Convert.ToString(Item.Tag)))

            End If

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to select the row", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ItemClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
    Private Sub lvwSearchDetails_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSearchDetails.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y


        Const ACCNFound As String = "0"
        ''Const ACCNNotFound As String = "1"        ''Unused Local Variables


        Dim oProduct As bSIRProduct.Business
        Dim lSelectedItem, lProductId As Integer
        Dim sColumnName As String = ""
        Dim vProductarray(,) As Object
        Dim lRow As Integer


        If Button = Windows.Forms.MouseButtons.Right Then

            lProductId = m_lProductID
            sColumnName = "Cover_Note_numbering_id"


            If lvwSearchDetails.Items.Count > 0 Then

                lSelectedItem = lvwSearchDetails.FocusedItem.Index + 1

                Dim temp_oProduct As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oProduct, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oProduct = temp_oProduct

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then


                    m_lReturn = oProduct.GetProductValue(v_lProductId:=gPMFunctions.ToSafeLong(lProductId), v_sColumnName:=gPMFunctions.ToSafeString(sColumnName), r_vProductArray:=vProductarray)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        lRow = gPMFunctions.ToSafeLong(Convert.ToString(lvwSearchDetails.Items.Item(lSelectedItem - 1).Tag))


                        If m_sTransactionType <> "MTC" Then

                            If m_sTransactionType = "NB" Or m_sTransactionType = "REN" Or m_sTransactionType = "MTR" Then

                                Dim auxVar As Object = gPMFunctions.ZeroToNull(CStr(vProductarray(0, 0)))


                                If Convert.IsDBNull(auxVar) Or IsNothing(auxVar) Or CStr(m_vSearchData(ACIRiskStatusCode, lRow)).Trim() <> "QUOTED" Then
                                    mnuAttachCoverNote.Text = "&Attach Cover Note"
                                    mnuAttachCoverNote.Enabled = False
                                Else
                                    If Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).SubItems.Item(ACColPosCNAttached).Tag) = ACCNFound Then
                                        mnuAttachCoverNote.Text = "&Attach Cover Note"
                                        mnuAttachCoverNote.Enabled = True
                                    Else
                                        mnuAttachCoverNote.Text = "&Detach Cover Note"
                                        mnuAttachCoverNote.Enabled = True
                                    End If
                                End If
                            ElseIf m_sTransactionType = "MTA" Then

                                If CStr(m_vSearchData(ACIRiskStatusFlag, lRow)).Trim() = "U" Then
                                    If Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).SubItems.Item(ACColPosCNAttached).Tag) = ACCNFound Then
                                        mnuAttachCoverNote.Text = "&Attach Cover Note"
                                        mnuAttachCoverNote.Enabled = False
                                    Else
                                        mnuAttachCoverNote.Text = "&Detach Cover Note"
                                        mnuAttachCoverNote.Enabled = False
                                    End If
                                ElseIf CStr(m_vSearchData(ACIRiskStatusFlag, lRow)).Trim() = "C" Then
                                    Dim auxVar_2 As Object = gPMFunctions.ZeroToNull(CStr(vProductarray(0, 0)))


                                    If Convert.IsDBNull(auxVar_2) Or IsNothing(auxVar_2) Or CStr(m_vSearchData(ACIRiskStatusCode, lRow)).Trim() <> "QUOTED" Then
                                        mnuAttachCoverNote.Text = "&Attach Cover Note"
                                        mnuAttachCoverNote.Enabled = False
                                    Else
                                        If Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).SubItems.Item(ACColPosCNAttached).Tag) = ACCNFound Then
                                            mnuAttachCoverNote.Text = "&Attach Cover Note"
                                            mnuAttachCoverNote.Enabled = True
                                        Else
                                            mnuAttachCoverNote.Text = "&Detach Cover Note"
                                            mnuAttachCoverNote.Enabled = True
                                        End If
                                    End If
                                End If


                            End If
                        Else
                            If CStr(m_vSearchData(ACIRiskStatusFlag, lRow)).Trim() = "C" Or CStr(m_vSearchData(ACIRiskStatusCode, lRow)).Trim() <> "QUOTED" Then
                                If Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).SubItems.Item(ACColPosCNAttached).Tag) = ACCNFound Then
                                    mnuAttachCoverNote.Text = "&Attach Cover Note"
                                    mnuAttachCoverNote.Enabled = False
                                Else
                                    mnuAttachCoverNote.Text = "&Detach Cover Note"
                                    mnuAttachCoverNote.Enabled = False
                                End If
                            End If

                        End If
                    End If

                End If
            Else
                mnuAttachCoverNote.Text = "&Attach Cover Note"
                mnuAttachCoverNote.Enabled = False
            End If

            'TODO
            'PopupMenu(mnuRiskOptions)


            ctxMenuStrip.Visible = True
            mnuAttachCoverNote.Visible = True
            mnuRiskOptions.Visible = False
            'ctxMenuStrip.Left = x + Me.Left
            'ctxMenuStrip.Top = y + Me.Top
            ctxMenuStrip.Items.Add(mnuAttachCoverNote)
            ctxMenuStrip.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)


        End If
    End Sub
    'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()

    'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
    Public Sub mnuAttachCoverNote_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuAttachCoverNote.Click


        'Const ACTick As String = "tick"        '' Unused Local Variables
        Const ACCNFound As String = "1"
        Const ACCNNotFound As String = "0"
        'ListViewFunc.ListViewBatchStart(lvwSearchDetails)
        'PN-62206 Start
        m_CoverNoteAttached = 0
        For lRiskID As Integer = 0 To m_vCoverNote.GetUpperBound(1)
            If CBool(m_vCoverNote(ACDataPosCNAttach, lRiskID)) Then
                m_CoverNoteAttached += 1
            End If
        Next
        ''PN-62206 End
        Dim lSelectedItem As Integer = lvwSearchDetails.FocusedItem.Index + 1
        If lSelectedItem <> 0 And lvwSearchDetails.Items.Count > 0 Then
            'TODO
            Dim subitm As New ListViewItem.ListViewSubItem

            Select Case mnuAttachCoverNote.Text
                Case "&Attach Cover Note"
                    ''PN-62206 Start
                    If m_CoverNoteAttached >= m_lCoverNoteUpTo Then
                        MessageBox.Show("Only " & m_lCoverNoteUpTo & " covers notes are allowed to attach with selected product.", "Cover Note Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If
                    'PN-62206 End
                    m_lReturn = AttachDetachCoverNote(lSelectedItem, True)

                    'lvwSearchDetails.Items.Item(lSelectedItem - 1).SubItems.RemoveAt(ACColPosCNAttached - 1)
                    ''Developer Guide No. 13
                    'lvwSearchDetails.Items.Item(lSelectedItem - 1).SubItems.Add(ACColPosCNAttached, Nothing, Nothing, Nothing)
                    'lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).SubItems.Item(ACColPosCNAttached - 1).Tag = ACCNFound
                    subitm.Text = "T"
                    lvwSearchDetails.Items.Item(lSelectedItem - 1).SubItems.RemoveAt(ACColPosCNAttached)
                    lvwSearchDetails.Items.Item(lSelectedItem - 1).SubItems.Insert(ACColPosCNAttached, subitm)
                    lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).SubItems.Item(ACColPosCNAttached).Tag = ACCNFound

                    m_CoverNoteAttached += 1

                    'ListViewHelper.SetListItemSmallIconProperty(lvwSearchDetails.Items.Item(lSelectedItem - 1), "")
                Case "&Detach Cover Note"

                    m_lReturn = AttachDetachCoverNote(lSelectedItem, False)

                    'lvwSearchDetails.Items.Item(lSelectedItem - 1).SubItems.RemoveAt(ACColPosCNAttached - 1)
                    'lvwSearchDetails.Items.Item(lSelectedItem - 1).SubItems.Add(ACColPosCNAttached)
                    'lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).SubItems.Item(ACColPosCNAttached - 1).Tag = ACCNNotFound
                    subitm.Text = ""
                    lvwSearchDetails.Items.Item(lSelectedItem - 1).SubItems.RemoveAt(ACColPosCNAttached)
                    lvwSearchDetails.Items.Item(lSelectedItem - 1).SubItems.Insert(ACColPosCNAttached, subitm)
                    lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).SubItems.Item(ACColPosCNAttached).Tag = ACCNNotFound

                    m_CoverNoteAttached -= 1
                    'ListViewHelper.SetListItemSmallIconProperty(lvwSearchDetails.Items.Item(lSelectedItem - 1), "")
            End Select

        End If
        'ListViewFunc.ListViewBatchEnd()
    End Sub
    'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()

    'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
    Private Function AttachDetachCoverNote(ByRef lSelectedItem As Integer, ByVal bAttach As Boolean) As Integer


        Dim lRow As Integer


        'Dim vRefreshedCNArray As Variant

        If bAttach Then

            lRow = gPMFunctions.ToSafeLong(Convert.ToString(lvwSearchDetails.Items.Item(lSelectedItem - 1).Tag))
            m_vCoverNote(ACDataPosCNAttach, lRow) = True
        Else

            lRow = gPMFunctions.ToSafeLong(Convert.ToString(lvwSearchDetails.Items.Item(lSelectedItem - 1).Tag))
            m_vCoverNote(ACDataPosCNAttach, lRow) = False

        End If
    End Function
    'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()






    ' PRIVATE Events (Begin)
    Private Sub uctPMUListRisk_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        Try

            m_lReturn = ResizeInterface()

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    '
    ' PW091202 - set the checkbox for the first item added
    ' ISS1571
    '
    Public Function CheckClick() As Integer

        ' Click event of the check box.

        Try

            lvwSearchDetails.Items.Item(0).Checked = True
            'Developer Guide No. 13
            lvwSearchDetails_ItemChecked(lvwSearchDetails, New ItemCheckedEventArgs(lvwSearchDetails.Items.Item(0)))

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Check box", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Function

    Public Function CancelClick() As Integer

        ' Click event of the Cancel button.

        Try


            Return CancelListRisk()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Function

        End Try
    End Function

    Public Function DeleteClick() As Integer

        ' Click event of the Delete button.

        Try


            Return DeleteRisk()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Delete command button", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'Developer Guide No. 20
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        Return SSfunc.ShowHelp(cmdHelp, ScreenHelpID)


    End Function

    ' ***************************************************************** '
    ' Name: OKClick
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function OKClick(Optional ByRef bCheckMultiCurrency As Boolean = True) As Integer

        Dim result As Integer = 0
        Try

            'CT 16/08/00 start
            'OKClick = SelectListPolicy()

            'The ok button on parent form in client manager has been clicked
            'so now act as if doubleclick on this component were pressed
            lvwSearchDetails_DoubleClick(lvwSearchDetails, New EventArgs())
            'CT 16/08/00 end
            result = m_lReturn 'CT 31/08/00 note that lvwSearchDetails_DblClick will set m_lReturn

            ' RDC 13052004 multi-currency
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And bCheckMultiCurrency Then
                m_lReturn = ShowMultiCurrencyDialogue()
                'RKS 03/11/2004 PN15210
                result = m_lReturn
            End If

            'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
            m_lReturn = ProcessCoverNotes(False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMCancel
            End If
            'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OKClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OKClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Static bIsInitialised As Boolean

        Dim sTitle, sMessage As String

        Dim sHelpFile As String = ""
        Dim m_lReturn As gPMConstants.PMEReturnCode
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        ' PW221102
        ' PS411
        Dim iOptionValue As Integer
        ' RDC 12052004

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
                'Developer Guide No SOLUTION 39
                'App.HelpFile = sHelpFile
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRFindRisk.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' PW221102 - Get the When Taxes Required system option
            ' PS411

            m_lReturn = m_oBusiness.getOption(v_iOptionNumber:=ACOptWhenTaxesRequired, r_nOptionValue:=iOptionValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' If option is not there (or NULL), we assume it's off
                m_bWhenTaxesRequired = False
            Else
                m_bWhenTaxesRequired = (iOptionValue = 1)
            End If

            m_oFormFields = New iPMFormControl.FormFields()

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPremium, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lFieldType:=gPMConstants.PMEDataType.PMDate, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

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
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Has currency changed

            m_lReturn = m_oBusiness.GetHasCurrencyChanged(v_lInsuranceFileCnt:=m_lInsFileCnt, r_bHasCurrencyChanged:=m_bCurrencyChanged)
            ReDim m_abEdittedArray(1)

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
            m_lReturn = GetValidSources()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            'eck090500
            ' {* USER DEFINED CODE (End) *}

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load control", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_oUserAuthorities IsNot Nothing Then
                    m_oUserAuthorities.Dispose()
                    m_oUserAuthorities = Nothing
                End If
                If m_oFormFields IsNot Nothing Then
                    m_oFormFields.Dispose()
                    m_oFormFields = Nothing
                End If


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


            ' Terminate the general object.
            Dispose()

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLoadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            'David Kyle Thing
            'call PMUser to get the Sources
            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oPMUser As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oPMUser = temp_g_oPMUser

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMUser.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

                Return result
            End If
            'eck220500


            m_lReturn = g_oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=g_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get valid sources", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

                Return result
            End If
            ' Remove instance of PMUser
            If Not (g_oPMUser Is Nothing) Then

                g_oPMUser.Dispose()
                g_oPMUser = Nothing
            End If

            Return result

        Catch excep As System.Exception


            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick
        ' Double click event for the search details.

        Try

            ' Check if there are any items available.
            If lvwSearchDetails.Items.Count = 0 Then
                Exit Sub
            End If

            m_lReturn = SelectListRisk()

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            Else

            End If

            'TN20010117 Start
            'added m_lIsReInsuranceAtRiskLevel and m_lInsuranceFolderCnt
            RaiseEvent lvwSearchDetailsDblClick(Me, New lvwSearchDetailsDblClickEventArgs(m_lInsFileCnt, m_lRiskId, m_sRiskDescription, m_sRiskTypeDescription, m_vRiskInceptionDate, m_vRiskExpiryDate, m_vRiskTotalSI, m_vRiskTotalPremium, m_lRiskGisScreenId, m_lRiskTypeId, m_lIsReInsuranceAtRiskLevel, m_lInsuranceFolderCnt))

            'TN20010117 End

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub
    Private sortColumn As Integer = -1
    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        'Dim ColumnHeader As ColumnHeader = lvwSearchDetails.Columns(eventArgs.Column)

        ' PW311002
        'Dim iSortOrder As SortOrder
        'Dim iColumn As Integer
        'Static s_iSortKey As Integer
        'For storing the position of the horizontal scroll bar.
        StoreHScrollValue()
        ' Column click event for the search details


        Try
            m_bLoading = True
            'RemoveHandler lvwSearchDetails.ItemChecked, AddressOf lvwSearchDetails_ItemChecked

            Dim iEmptyColCtr As Integer = 0
            With lvwSearchDetails
                ''Sorting Checkbox column
                If eventArgs.Column = ACColPosIsSelected Then
                    For Each lvItem As ListViewItem In .Items
                        If lvItem.Checked Then
                            lvItem.SubItems(eventArgs.Column).Text = "1"
                        Else
                            lvItem.SubItems(eventArgs.Column).Text = "2"
                        End If
                    Next
                End If

                'check for whole empty column
                For Each lvItem As ListViewItem In .Items
                    Try
                        If lvItem.SubItems(eventArgs.Column).Text.Trim.Equals(String.Empty) Then
                            iEmptyColCtr += 1
                        End If
                    Catch ex As ArgumentOutOfRangeException
                        iEmptyColCtr += 1
                        Continue For
                    End Try
                Next
                If iEmptyColCtr = .Items.Count Then
                    Exit Sub
                End If

                If eventArgs.Column <> sortColumn Then
                    sortColumn = eventArgs.Column
                    .Sorting = SortOrder.Ascending
                Else
                    If .Sorting = SortOrder.Ascending Then
                        .Sorting = SortOrder.Descending
                    Else
                        .Sorting = SortOrder.Ascending
                    End If

                End If
                .Sort()
                .ListViewItemSorter = New ListViewItemComparer(eventArgs.Column, .Sorting)


                If eventArgs.Column = ACColPosIsSelected Then
                    For Each lvItem As ListViewItem In .Items
                        lvItem.SubItems(eventArgs.Column).Text = ""
                    Next
                End If

            End With
            'AddHandler lvwSearchDetails.ItemChecked, AddressOf lvwSearchDetails_ItemChecked

            m_bLoading = False
            'For recovering the position of horizontal scroll bar. 
            RecoverHorizontalScroll()

        Catch excep As System.Exception

            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' PRIVATE Events (End)

    '*******************************************************************************
    ' Name : ShowMultiCurrencyDialogue
    '
    ' Description : Displays the multi-currency dialogue if required:
    '               If policy and base currency are different AND ((User cannot
    '               change rates AND System Option 156 enabled) OR user
    '               can change rates)
    '
    ' History :
    ' 12052004 RDC created
    '*******************************************************************************
    Private Function ShowMultiCurrencyDialogue() As Integer
        Dim result As Integer = 0


        Dim iTransactionCurrencyID, iBaseCurrencyID As Integer
        Dim lStatus As gPMConstants.PMEReturnCode
        Dim sOption157 As String = ""
        Dim oForm As frmMultiCurrency

        Dim oParty As bSIRParty.Business

        Dim oInsuranceFile As bSIRInsuranceFile.Business
        Dim vInsuranceFile As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' get the transaction currency from InsuranceFile
            Dim temp_oInsuranceFile As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oInsuranceFile, "bSIRInsuranceFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oInsuranceFile = temp_oInsuranceFile

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bSIRInsuranceFile.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowMultiCurrencyDialogue")
                Return result
            End If

            m_lReturn = oInsuranceFile.GetDetails(vInsuranceFileCnt:=m_lInsFileCnt)
            m_lReturn = oInsuranceFile.GetNext(r_vFieldArray:=vInsuranceFile)

            oInsuranceFile.Dispose()
            oInsuranceFile = Nothing

            Dim temp_oParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oParty, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oParty = temp_oParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowMultiCurrencyDialogue")
                Return result
            End If

            'get the Base Currency from InsuranceFile
            m_lReturn = oParty.GetBaseCurrencyID(lSourceID:=CInt(vInsuranceFile(InsuranceFileConst.ACSourceID)), iCurrencyID:=iBaseCurrencyID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRParty.Business.GetBaseCurrencyID failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowMultiCurrencyDialogue")
                oParty.Dispose()
                oParty = Nothing
                Return result
            End If

            oParty.Dispose()
            oParty = Nothing

            iTransactionCurrencyID = CInt(vInsuranceFile(InsuranceFileConst.ACCurrencyID))
            m_vLeadAgentCnt = vInsuranceFile(InsuranceFileConst.ACLeadAgentCnt)

            If iTransactionCurrencyID = iBaseCurrencyID Then
                ' policy currency matches source's base currency
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            ' get option 156
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=157, r_sOptionValue:=sOption157)

            'If user hasn not got the authority to override rates and show currency screen
            'is not set, then do not show the currency screen.
            If Not m_bAllowOverride And sOption157.Trim() <> "1" Then
                ' don't show dialogue
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            oForm = New frmMultiCurrency()

            'Developer Guide No. 68
            'Load(oForm)
            'oForm.ShowDialog()
            'set up data
            oForm.InsuranceFileCnt = m_lInsFileCnt


            oForm.DirectBusiness = Convert.IsDBNull(m_vLeadAgentCnt) Or IsNothing(m_vLeadAgentCnt)

            'm_lReturn = CType(oForm, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            m_lReturn = oForm.Initialise()

            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the multi-currency dialogue", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowMultiCurrencyDialogue")
                Return result
            End If

            oForm.ShowDialog()

            lStatus = oForm.Status

            If lStatus = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
            Else
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            oForm.Dispose()

            oForm.Close()

            oForm = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowMultiCurrencyDialogue failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowMultiCurrencyDialogue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function CreateUserAuthorities() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oUserAuthorities Is Nothing Then
                Dim temp_m_oUserAuthorities As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oUserAuthorities = temp_m_oUserAuthorities
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to create bACTUserAuthorities.Business")
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateUserAuthorities failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateUserAuthorities", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTotalPremium
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetTotalPremium() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTotalPremium"

        Dim lReturn As Integer
        Dim crTotalPremium As Decimal

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' determine if there are any selected items
            For lItem As Integer = 1 To lvwSearchDetails.Items.Count
                If lvwSearchDetails.Items.Item(lItem - 1).Checked Then
                    crTotalPremium += gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lItem - 1), ACColPosPremium).Text)
                End If
            Next

            m_crTotalPremium = crTotalPremium



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetTotalTax
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetTotalTax() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTotalTax"

        Dim lReturn As Integer
        Dim crTotalTax As Decimal

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' determine if there are any selected items
            For lItem As Integer = 1 To lvwSearchDetails.Items.Count
                If lvwSearchDetails.Items.Item(lItem - 1).Checked Then
                    crTotalTax += CDbl(m_vSearchData(ACIRiskTax, lItem - 1))
                End If
            Next

            m_crTotalTax = Math.Round(crTotalTax, 2)



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetTotalFeeTax
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetTotalFeeTax() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTotalFeeTax"

        Dim lReturn As Integer
        Dim crTotalFeeTax As Decimal

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' determine if there are any selected items
            For lItem As Integer = 1 To lvwSearchDetails.Items.Count
                If lvwSearchDetails.Items.Item(lItem - 1).Checked Then
                    crTotalFeeTax += CDbl(ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lItem - 1), ACColPosFeeTax).Text)
                End If
            Next

            m_crTotalFeeTax = crTotalFeeTax



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetTotalFeePremium
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 05-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function GetTotalFeePremium() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTotalFeePremium"

        Dim lReturn As Integer
        Dim crTotalFeePremium As Decimal

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' determine if there are any selected items
            For lItem As Integer = 1 To lvwSearchDetails.Items.Count
                If lvwSearchDetails.Items.Item(lItem - 1).Checked Then
                    crTotalFeePremium += CDbl(ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lItem - 1), ACColPosFeePremium).Text)
                End If
            Next

            m_crTotalFeePremium = crTotalFeePremium



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetNoSelectedQuotedRisks
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetNoSelectedQuotedRisks() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetNoSelectedQuotedRisks"

        Dim lReturn, lNoOfSelectedQuotedRisk As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' determine if there are any selected items
            For lItem As Integer = 1 To lvwSearchDetails.Items.Count
                If lvwSearchDetails.Items.Item(lItem - 1).Checked Then
                    If ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lItem - 1), ACColPosRiskStatus).Text = "Quoted" Then
                        lNoOfSelectedQuotedRisk += 1
                    End If
                End If
            Next

            m_lNoOfSelectedQuotedRisks = lNoOfSelectedQuotedRisk



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
    Public Function SaveAllCoverNotesOnQuote() As Integer
        Dim result As Integer = 0
        Dim vCoverNoteRisk As Object
        Dim iCounter As Integer
        Const kMethodName As String = "SaveAllCoverNotesOnQuote"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            If Information.IsArray(m_vCoverNote) Then
                m_lReturn = ProcessCoverNotes(True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Save Cover Note Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

            '        Return result
            '        Resume


            '        Return result
        End Try
        Return result
    End Function
    'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()

    'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
    Private Function ProcessCoverNotes(ByVal bQuite As Boolean) As Integer


        Dim result As Integer = 0
        Dim ofrmCoverNote As New frmCoverNote

        Dim lSourceID, lProductId, lAgentID As Integer




        Const kMethodName As String = "ProcessCoverNotes"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vCoverNote) Then
                For iCounter As Integer = m_vCoverNote.GetLowerBound(1) To m_vCoverNote.GetUpperBound(1)
                    If CBool(m_vCoverNote(ACDataPosCNAttach, iCounter)) Then
                        If Information.IsArray(m_vSearchData) And Information.IsArray(vResultArrayCoverNote) Then
                            'If Not bQuite And CStr(m_vSearchData(ACIRiskStatusFlag, iCounter)).Trim() <> "U" And (CBool(vResultArrayCoverNote(2, iCounter + 1))) Then
                            If Not bQuite And CStr(m_vSearchData(ACIRiskStatusFlag, iCounter)).Trim() <> "U" And (CBool(vResultArrayCoverNote(1, iCounter))) Then
                                With ofrmCoverNote
                                    .RiskID = CInt(m_vCoverNote(ACDataPosCNRiskId, iCounter))
                                    .SourceID = gPMFunctions.ToSafeLong(g_iSourceID)
                                    .ProductID = gPMFunctions.ToSafeLong(m_lProductID)
                                    .AgentID = gPMFunctions.ToSafeLong(m_vSearchData(ACIAgentCnt, iCounter))
                                    .RiskNo = gPMFunctions.ToSafeLong(m_vSearchData(ACIRiskNo, iCounter))
                                    .RiskDesc = gPMFunctions.ToSafeString(m_vSearchData(ACIRiskDescription, iCounter))
                                    .DateTo = gPMFunctions.ToSafeDate(m_vSearchData(ACIRiskInceptionDate, iCounter))
                                    .TransactionType = m_sTransactionType
                                    If m_sTransactionType = "MTA" Then
                                        If ToSafeDate(m_vSearchData(ACIRiskInceptionDate, iCounter)) > DateTime.Now Then
                                            .DateFrom = ToSafeDate(m_vSearchData(ACIRiskInceptionDate, iCounter))
                                        End If
                                        If ToSafeDate(m_vSearchData(ACIRiskInceptionDate, iCounter)) < DateTime.Now Then
                                            .DateFrom = Now
                                        End If
                                    End If

                                    .ShowDialog()
                                    m_vCoverNote(ACDataPosCNRef, iCounter) = gPMFunctions.ToSafeString(.CoverNoteNo)
                                    m_vCoverNote(ACDataPosCNFrom, iCounter) = gPMFunctions.ToSafeDate(.DateFrom)
                                    m_vCoverNote(ACDataPosCNTo, iCounter) = gPMFunctions.ToSafeDate(.DateTo)

                                    m_lReturn = .ReasonForFailure
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Return gPMConstants.PMEReturnCode.PMCancel
                                    End If
                                End With
                            End If
                        End If
                    End If

                    If Information.IsArray(m_vSearchData) Then
                        If CStr(m_vSearchData(ACIRiskStatusFlag, iCounter)).Trim() <> "U" Then

                            m_lReturn = m_oBusiness.AttachCoverNotes(m_vCoverNote, iCounter)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "Attach Cover Note Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If


                        m_lReturn = m_oBusiness.DetachCoverNotes(m_vCoverNote, iCounter)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Detach Cover Note Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
                Next


                For iCounter As Integer = m_vCoverNote.GetLowerBound(1) To m_vCoverNote.GetUpperBound(1)
                    If CBool(m_vCoverNote(ACDataPosCNAttach, iCounter)) Then
                        If Information.IsArray(m_vSearchData) And Information.IsArray(vResultArrayCoverNote) Then
                            'If Not bQuite And CStr(m_vSearchData(ACIRiskStatusFlag, iCounter)).Trim() <> "U" And (CBool(vResultArrayCoverNote(2, iCounter+1))) Then
                            If Not bQuite And CStr(m_vSearchData(ACIRiskStatusFlag, iCounter)).Trim() <> "U" And (CBool(vResultArrayCoverNote(1, iCounter))) Then
                                m_lReturn = UseDocTemplate(gPMFunctions.ToSafeLong(m_vSearchData(ACIDocTemplateId, CInt(m_vCoverNote(ACDataPosRowTag, iCounter)))), gPMFunctions.ToSafeLong(m_vSearchData(ACIDocTemplateTypeId, iCounter)), gPMFunctions.ToSafeLong(m_vSearchData(ACIPartyCnt, iCounter)), gPMFunctions.ToSafeLong(m_vSearchData(ACIInsuranceFolderCnt, iCounter)), gPMFunctions.ToSafeLong(m_vSearchData(ACIInsFileCnt, iCounter)), 0, gPMFunctions.ToSafeLong(m_vSearchData(ACIRiskId, iCounter)), gPMFunctions.ToSafeBoolean(m_vSearchData(ACIDocIsEditableAfterMerging, iCounter)))
                            End If
                        End If
                    End If
                Next

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Generation of Cover Notes Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally





        End Try
        Return result
    End Function

    'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()

    'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
    Private Function UseDocTemplate(ByVal v_lDocumentTemplateId As Integer, ByVal v_lDocumentTypeId As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimCnt As Integer, Optional ByVal v_lRiskCnt As Integer = 0, Optional ByVal v_bIsEditable As Boolean = False) As Integer

        Dim result As Integer = 0

        'Developer Guide No. 88
        Dim oObject As Object

        Const kMethodName As String = "UseDocTemplate"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            Dim temp_oObject As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject
            '/* PN 67183 BY SANTOSH PAYASI ON 08FEB2010 - Starts
            '    m_lReturn = oObject.Initialise()
            '
            '    If (m_lReturn <> PMTrue) Then
            '        RaiseError kMethodName, "Generation of Cover Notes Failed", PMLogError
            '    End If
            '
            '    m_lReturn = oObject.SetProcessModes(vTask:=PMEdit)
            '   PN 67183 BY SANTOSH PAYASI ON 08FEB2010 - Ends */


            oObject.PartyCnt = v_lPartyCnt

            oObject.InsuranceFolderCnt = v_lInsuranceFolderCnt

            oObject.InsuranceFileCnt = v_lInsuranceFileCnt

            oObject.ClaimCnt = v_lClaimCnt


            oObject.DocumentTemplateId = v_lDocumentTemplateId

            oObject.DocumentTypeId = v_lDocumentTypeId


            If v_bIsEditable Then
                '/* PN 67183 BY SANTOSH PAYASI ON 08FEB2010 - Starts

                oObject.Mode = gSIRLibrary.ACUserChoice
                '   PN 67183 BY SANTOSH PAYASI ON 08FEB2010 - Ends */
            Else

                oObject.Mode = gSIRLibrary.ACSpoolDocMode
            End If



            oObject.RiskCnt = v_lRiskCnt


            m_lReturn = oObject.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Generation of Cover Notes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            oObject.Dispose()
            oObject = Nothing




        Catch ex As Exception

            oObject = Nothing

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


        Finally






        End Try
        Return result
    End Function

    ''' <summary>
    ''' lvwSearchDetails_ItemChecked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lvwSearchDetails_ItemChecked(ByVal sender As System.Object,
                                             ByVal e As System.Windows.Forms.ItemCheckedEventArgs) _
        Handles lvwSearchDetails.ItemChecked

        If m_bFormLoading Then
            m_bLoading = m_bFormLoading
        End If
        If m_bLoading Then Exit Sub

        If e.Item.Index < 0 Then
            Exit Sub
        End If
        Dim lviItem As ListViewItem = lvwSearchDetails.Items(e.Item.Index)

        Dim nRiskNo As Integer
        Dim bExistingRisk As Boolean
        Dim bIsRiskSelected As Boolean
        Dim nVariationNo As Integer
        Dim nRow As Integer


        Dim nItemIndex As Integer = lviItem.Index + 1
        Dim bItemChecked As Boolean = lviItem.Checked
        Dim bMandatoryRisk As Boolean

        Try

            ' raise about to change event to give user a chance to cancel action
            m_bLoading = True
            RaiseEvent AboutToChange(Me, Nothing)

            If m_bCancelAboutToChangeAction Then
                m_bCancelAboutToChangeAction = False

                lviItem.Checked = Not lviItem.Checked
                ''Add the Event Hnadler Before Exit

                m_bLoading = False

                Exit Sub
            Else
                ' ensure if the item has been rolled back that
                ' we still have a reference to the item
                lviItem = lvwSearchDetails.Items.Item(nItemIndex - 1)
                lviItem.Checked = bItemChecked
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                lviItem.Checked = Not lviItem.Checked
                Exit Sub
            End If

            ' get the risk cnt
            Dim nRiskFolderKey As Integer = CInt(m_vSearchData(ACIRiskFolderCnt, Conversion.Val(Convert.ToString(lviItem.Tag))))
            Dim nRiskCnt As Integer = CInt(m_vSearchData(ACIRiskId, Conversion.Val(Convert.ToString(lviItem.Tag))))
            'Set flag to false if a selected risk has not been editted yet.
            m_bCurrenciesUpdated = True
            For i As Integer = 1 To lvwSearchDetails.Items.Count

                nRow = Conversion.Val(Convert.ToString(lvwSearchDetails.Items.Item(i - 1).Tag))
                If lvwSearchDetails.Items.Item(i - 1).Checked AndAlso Not m_abEdittedArray(nRow) Then
                    m_bCurrenciesUpdated = False
                    Exit For
                End If
            Next

            If lviItem.Checked Then

                If _
                    lviItem.ListView IsNot Nothing AndAlso
                    ((ListViewHelper.GetListViewSubItem(lviItem, ACColPosRiskStatus).Text.ToLower().IndexOf("quoted") + 1) = 0 OrElse
                     ListViewHelper.GetListViewSubItem(lviItem, ACColPosRiskStatus).Text.ToLower().IndexOf("unquoted") >= 0) _
                    Then

                    lviItem.Checked = False

                    MessageBox.Show("You cannot select a Risk that is not quoted.", "Risk Status", MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation)
                Else

                    nRiskFolderKey = CInt(Conversion.Val(CStr(m_vSearchData(ACIRiskFolderCnt, Convert.ToString(lviItem.Tag)))))
                    nRiskNo = CInt(Conversion.Val(CStr(m_vSearchData(ACIRiskNo, Convert.ToString(lviItem.Tag)))))
                    For i As Integer = 1 To lvwSearchDetails.Items.Count
                        If lvwSearchDetails.Items.Item(i - 1).Checked Then

                            If Conversion.Val(ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(i - 1), kColPosRiskFolderKey).Text) = nRiskFolderKey AndAlso
                                Not Convert.ToString(lvwSearchDetails.Items.Item(i - 1).Tag).Equals(Convert.ToString(lviItem.Tag)) AndAlso
                                ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(i - 1), ACColPosRiskStatus).Text.ToLower() <> "deleted" Then

                                m_lReturn = CheckIfRiskExists(v_iArrayIndex:=Conversion.Val(Convert.ToString(lvwSearchDetails.Items.Item(i - 1).Tag)), r_bExistingRisk:=bExistingRisk)
                                If bExistingRisk Then
                                    nVariationNo = CInt(Conversion.Val(ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(i - 1), ACColPosVariationNo).Text))
                                    Exit For
                                End If
                            End If
                        End If
                    Next

                    If bExistingRisk Then
                        lviItem.Checked = False
                        MessageBox.Show(
                            "You cannot select this Risk as it will result in " &
                            "another Risk that is already part of the policy being " &
                            "unselected. In order to select this Risk, remove " & " the other Risk (Risk " & CStr(nRiskNo) &
                            ", Variation " & CStr(nVariationNo) & ") " & "using the 'delete' button first.", "Risk Status",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else

                        nRiskFolderKey = CInt(Conversion.Val(CStr(m_vSearchData(ACIRiskFolderCnt, Convert.ToString(lviItem.Tag)))))

                        For i As Integer = 1 To lvwSearchDetails.Items.Count
                            If lvwSearchDetails.Items.Item(i - 1).Checked Then

                                If Conversion.Val(ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(i - 1), kColPosRiskFolderKey).Text) = nRiskFolderKey AndAlso
                                    Not Convert.ToString(lvwSearchDetails.Items.Item(i - 1).Tag).Equals(Convert.ToString(lviItem.Tag)) AndAlso
                                    ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(i - 1), ACColPosRiskStatus).Text.ToLower() <> "deleted" Then
                                    lvwSearchDetails.Items.Item(i - 1).Checked = False
                                End If
                            End If
                        Next

                        ' raise event to indicate that selection has been changed.
                        RaiseEvent RiskItemCheckChanged(Me, New RiskItemCheckChangedEventArgs(nRiskCnt, CheckState.Checked))

                    End If
                End If
            Else
                m_lReturn = CheckIfRiskExists(v_iArrayIndex:=Conversion.Val(Convert.ToString(lviItem.Tag)),
                                              r_bExistingRisk:=bExistingRisk, r_bMandatoryRisk:=bMandatoryRisk,
                                              r_bIsRiskSelected:=bIsRiskSelected)

                If bExistingRisk AndAlso m_sTransactionType <> "NB" Then

                    lviItem.Checked = True
                    MessageBox.Show(
                        "You cannot unselect a Risk that is already part of " &
                        "the policy. If you want to remove this Risk from the " &
                        "policy you must use the 'delete' button.", "Change Selection", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation)

                ElseIf bMandatoryRisk = True AndAlso m_sTransactionType = "NB" Then
                    lviItem.Checked = True
                    MessageBox.Show("You cannot unselect a mandatory risk", "Change Selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else

                    ' raise event to indicate that selection has been changed.
                    RaiseEvent RiskItemCheckChanged(Me, New RiskItemCheckChangedEventArgs(nRiskCnt, CheckState.Unchecked))
                End If
            End If

            Dim cPremium As Decimal = 0
            For i As Integer = 1 To lvwSearchDetails.Items.Count
                If lvwSearchDetails.Items.Item(i - 1).Checked Then
                    cPremium +=
                        gPMFunctions.ToSafeCurrency(
                            ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(i - 1), ACColPosPremium).Text)
                End If
            Next

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPremium, vControlValue:=cPremium)

            m_bLoading = False

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="lvwSearchDetails_ItemChecked Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ItemChecked", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Sub

    Private Sub lvwSearchDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwSearchDetails.Click
        m_bLoading = False
        lvwSearchDetails_ItemClick(lvwSearchDetails.FocusedItem)
    End Sub


    Private Sub lvwSearchDetails_DrawSubItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawListViewSubItemEventArgs) Handles lvwSearchDetails.DrawSubItem
        If e.SubItem.Text.ToUpper = "T" Then
            e.Graphics.DrawImageUnscaled(imglImages.Images("tick"), e.Bounds)
        Else
            e.DrawDefault = True
        End If
    End Sub

    Private Sub lvwSearchDetails_DrawColumnHeader(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawListViewColumnHeaderEventArgs) Handles lvwSearchDetails.DrawColumnHeader
        e.DrawDefault = True
    End Sub

    Private Sub lvwSearchDetails_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwSearchDetails.SelectedIndexChanged
        If Not (lvwSearchDetails.SelectedItems Is Nothing) Then
            If lvwSearchDetails.SelectedItems.Count > 0 Then
                m_iSelectedIndex = lvwSearchDetails.SelectedItems(0).Index
            End If
        End If
    End Sub
End Class
