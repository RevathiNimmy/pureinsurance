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
Imports System.Text
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctPMUPolicyExplorer_NET.uctPMUPolicyExplorer")>
Partial Public Class uctPMUPolicyExplorer
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event EffectiveDateChange()
    Public Event TransactionTypeChange()
    ' ***************************************************************** '
    ' Item: uctListPartyPolicy
    '
    ' Description: Enhance List Policy User Control.
    '
    ' ************09/07/2002*****************
    ' NOTE: the large amount of commented
    ' code has been left in because the
    ' component was at an early stage
    ' of development.  Feel free to remove
    ' if you think it's necessary.
    ' ************09/07/2002*****************
    '
    ' Edit History:
    '   03/07/2002 PWF - Created
    '   09/07/2002 AMB - Continued development
    '   20/08/2002 AMB - More development on BuildPolicyVersionListview
    ' ***************************************************************** '

    Private Const kbInDebug As Boolean = False

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "uctPMUPolicyExplorer"

    ' Default Property Values:
    Private Const m_def_BackColor As Integer = 0
    Private Const m_def_ForeColor As Integer = 0
    Private Const m_def_Enabled As Integer = 0
    Private Const m_def_BackStyle As Integer = 0
    Private Const m_def_BorderStyle As Integer = 0
    Private Const m_def_PartyCnt As Integer = 0

    ' treeview node levels
    Private Const ksRoot As String = "ROOT"
    Private Const ksStatus As String = "STATUS"
    Private Const ksProduct As String = "PRODUCT"
    Private Const ksPolicy As String = "POLICY"
    Private Const ksOtherPolicy As String = "OTHER"

    ' misc consts
    Const kSRootNode As String = "ROOT"
    Const ksDelimiter As String = "|"

    ' policy status constants
    Private Const ksStatusCurrent As String = "Current"
    Private Const ksStatusRenewal As String = "Renewal"
    Private Const ksStatusQuote As String = "Quote"
    Private Const ksStatusLapsed As String = "Lapsed"
    Private Const ksStatusCancelled As String = "Cancelled"
    Private Const ksStatusReplaced As String = "Replaced"
    Private Const ksStatusActive As String = "Active"
    Private Const ksStatusInactive As String = "Inactive"

    ' listview columns - policy versions
    Private Const klColInceptionDate As Integer = 1
    Private Const klColRenewalDate As Integer = 2
    Private Const klColLapsedDate As Integer = 3
    Private Const klColInsuredPersons As Integer = 4
    Private Const klColRegarding As Integer = 5
    Private Const klColBillingMethod As Integer = 6
    Private Const klColAmount As Integer = 7
    Private Const klColCurrency As Integer = 8
    Private Const klColIntermediary As Integer = 9
    Private Const klColPolicyType As Integer = 10
    Private Const klColEventDescription As Integer = 11 'Gaurav Changed
    Private Const klColPolicyStatus As Integer = 12
    Private Const klColTransactionDate As Integer = 13


    ' listview columns - risks
    Private Const klColRiskDesc As Integer = 1
    Private Const klColCoverage As Integer = 2
    Private Const klColSumInsured As Integer = 3
    Private Const klColExcess As Integer = 4
    Private Const klColExtensions As Integer = 5
    Private Const klColNCB As Integer = 6
    Private Const klColGrossPremium As Integer = 7
    Private Const klColDeleted As Integer = 8
    Private Const klColRiskLinkStatus As Integer = 9
    Private Const klColRiskCahngeDate As Integer = 10

    ' extra policy information - policy versions
    Private Const klEPIIsLead As Integer = 1 ' is insured person the lead?
    Private Const klEPIResolvedName As Integer = 4 ' insured person name
    ' Property Variables:
    Private m_BackColor As Integer
    Private m_ForeColor As Integer
    Private m_Enabled As Boolean
    Private m_Font As Font
    Private m_BackStyle As Integer
    Private m_BorderStyle As Integer
    Private m_PartyCnt As Integer

    ' Sizing variables
    Private m_VSize As Integer
    Private m_HSize As Integer

    ' UDT for treeview data
    Private Structure udtTreeData
        Dim sStatus As String
        Dim sProductCode As String
        Dim sProductDesc As String
        Dim sPolicyNo As String
        Dim lInsFileCnt As Integer
        Dim lInsFolderCnt As Integer
        Dim lNumberOfClaims As Integer
        Dim lAnniversaryCopy As Integer
        Dim sStatusOther As String
        Dim nProductId As Integer
        Public Shared Function CreateInstance() As udtTreeData
            Dim result As New udtTreeData
            result.sStatus = String.Empty
            result.sProductCode = String.Empty
            result.sProductDesc = String.Empty
            result.sPolicyNo = String.Empty
            result.sStatusOther = String.Empty
            result.nProductId = 0
            Return result
        End Function
    End Structure

    Private aTreeNodes() As udtTreeData = Nothing

    Private bInVersions As Boolean
    Private bInRisks As Boolean

    'Event Declarations:
    Public Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs)
    Public Event DblClick(ByVal Sender As Object, ByVal e As EventArgs)
    Public Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs)
    Public Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs)
    Public Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs)
    Public Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs)
    Public Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs)
    Public Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs)

    Event lvwRisksDblClick(ByVal Sender As Object, ByVal e As lvwRisksDblClickEventArgs)
    'TN20010117 End

    Event lvwRisksClick(ByVal Sender As Object, ByVal e As lvwRisksClickEventArgs)


    Event lvwVersionsDblClick(ByVal Sender As Object, ByVal e As lvwVersionsDblClickEventArgs)


    ' Declare an instance of the Business objects
    Private m_oBusiness As Object

    Private m_oOption As bSIROptions.Business
    Private m_oRiskyBusiness As Object
    Private m_oExtraPolicyInfoBusiness As Object
    Private m_oCurrency As Object

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lInsFileCnt As Integer
    Private m_sInsReference As String = ""
    Private m_lInsHolderCnt As Integer
    Private m_sShortName As String = "" 'JW190498
    Private m_sLongName As String = "" 'JW190498
    Private m_lInsuranceFolderCnt As Integer 'TF100398
    Private m_sRegistration As String = "" 'Tom 031198
    Private m_lProductId As Integer

    Private m_lRiskID As Integer
    Private m_sRiskDescription As String = ""
    Private m_sRiskTypeDescription As String = ""
    Private m_vRiskInceptionDate As Date
    Private m_vRiskExpiryDate As Date
    'Am 07122000
    Private m_sRiskStatus As String = ""
    Private m_vRiskTotalSI As String = ""
    Private m_vRiskTotalPremium As String = ""
    Private m_lRiskGisScreenId As Integer
    Private m_lRiskTypeId As Integer

    Private m_bOKToProceed As Boolean
    Private m_bPendingReinsurance As Boolean

    Private m_lPartyUIK As Integer
    Private m_lPolicyUIK As Integer
    Private m_vLeadAgentCnt As Object
    'Tomo150300
    Private m_lPolicyTypeID As Integer
    Private m_sPolicyType As String = ""

    ' TF311298 - changed from NavProcessCode
    Private m_sInsFileType As String = ""
    'sj 5/11/99 - start
    Private m_bDisableInsFileType As Boolean
    'TN20010111
    Private m_lIsReInsuranceAtRiskLevel As Integer

    Private m_sUnderwritingOrAgency As String = ""

    ' true when control is being resized
    Private m_bResizing As Boolean

    ' Stores the search data from the business object.
    Public m_vSearchData(,) As Object
    Public m_vSearchOtherData(,) As Object
    Public m_vPolicyVersionSearchData(,) As Object
    Public m_vRiskSearchData(,) As Object
    Public m_vInsuranceFileData As Object
    Public m_vExtraPolicyData() As Object
    'Public m_vSingleSearchData          As Variant
    Public m_bCopyPolicy As Boolean
    Public m_bOKClick As Boolean

    Private m_bGeminiLink As Boolean
    Private m_bGeminiIILink As Boolean
    Private m_bSwiftLink As Boolean

    Private m_lFindType As Integer
    Private m_lPolicyType As Integer
    Private m_lInsuranceFileTypeId As Integer
    Private m_vGeminiPolicyStatus As Object 'JSB 08/06/01

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

    'eck090500
    Private m_vSourceArray As Object
    Private m_lKeepOnTop As Integer

    Private m_nVersion As Integer
    Private m_nNewInsFileCnt As Integer
    Private m_iCurrencyID As Integer
    Private m_nNewRiskTypeId As Integer
    Private m_oRiskData As Object
    Private m_oBusiness1 As Object
    Private m_oPerilAllocation As Object
    Private m_oPolicyNumber As Object
    Private m_oIGis As Object
    Private m_oFindRiskType As Object
    Private m_oObjectListRisk As Object
    Private m_oRen As Object
    Private m_sCopiedQuoteRef As String = String.Empty

    ' ***************************************************************** '
    ' Name: ShowHelpScreen
    '
    ' Description: Shows the help screen
    '
    ' ***************************************************************** '
    Public Function ShowHelpScreen(Optional ByRef cmdHelp As Object = Nothing) As Integer
        ' Fire up the help screen
        'Developer Guide No. 20
        Return SSfunc.ShowHelp(cmdHelp, ScreenHelpID)


    End Function

    <Browsable(False)>
    Public WriteOnly Property KeepONTop() As Integer
        Set(ByVal Value As Integer)
            m_lKeepOnTop = Value
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
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property


    <Browsable(True)>
    Public Property Task() As Integer
        Get
            ' Return the objects task.
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            ' Set the objects task.
            m_iTask = Value
        End Set
    End Property


    <Browsable(True)>
    Public Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            ' Set the interface exit status.
            m_lStatus = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            ' Set the navigate flag.
            m_lNavigate = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            ' Set the process mode.
            m_lProcessMode = Value
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


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OKClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OKClick", excep:=excep)

            Return result

        End Try
    End Function
    Public Function CancelClick() As Integer

        ' Click event of the Cancel button.

        Try


            Return CancelListRisk()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelClick", excep:=excep)

            Exit Function

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
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

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
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Cancel the ListPolicy", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelListRisk", excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' ' build up the policy versions listview
    ''' </summary>
    ''' <param name="lInsFileCnt"></param>
    ''' <param name="lInsFolderCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Function BuildPolicyVersionsListview(ByVal lInsFileCnt As Integer, ByVal lInsFolderCnt As Integer) As Integer


        Dim nResult As Integer = 0
        Dim nReturn As gPMConstants.PMEReturnCode
        Dim lstCurrListItem As ListViewItem
        Dim sAgentName As String = ""

        Dim sInsuredLead As String = ""
        Dim sInsuredRest As New StringBuilder
        Dim vInsuredResults(,) As Object
        Dim nMin1, nMax1, nMin2, nMax2 As Integer
        Dim sLapsedDate, sCurrDesc As String
        Dim nFilterVersions As Integer

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If includecancelledquote.Checked Then
                nFilterVersions = 0
            Else
                nFilterVersions = 1
            End If

            ' get the policy versions
            'Gaurav

            nReturn = m_oBusiness.GetAllPolicyVersion(r_vResultArray:=m_vPolicyVersionSearchData, v_lInsuranceFolderCnt:=lInsFolderCnt, v_lInsuranceFileCnt:=lInsFileCnt, v_lViaClientManager:=1, v_lfilterBackdatedVersions:=nFilterVersions)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the policy versions business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildPolicyVersionsListview")
                Return nResult
            End If

            If Information.IsArray(m_vPolicyVersionSearchData) Then
                If includecancelledquote.Checked = True Then 'Ritu
                    ' clear both listviews
                    lvwVersions.Items.Clear()
                    lvwRisks.Items.Clear()
                    'add policy versions to the listview
                    nMin1 = m_vPolicyVersionSearchData.GetLowerBound(1)
                    nMax1 = m_vPolicyVersionSearchData.GetUpperBound(1)
                    For nLoopy As Integer = nMin1 To nMax1
                        nReturn = m_oExtraPolicyInfoBusiness.GetDetails(vinsurancefilecnt:=Trim(m_vPolicyVersionSearchData(ACPVInsuranceFileCnt, nLoopy)))
                        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            ' Log Error.
                            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the extra policy info business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildPolicyVersionsListview")
                            Return nResult
                        End If

                        nReturn = m_oExtraPolicyInfoBusiness.GetNext(r_vFieldArray:=m_vExtraPolicyData)
                        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            ' Log Error.
                            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the extra policy details business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildPolicyVersionsListview")
                            Return nResult
                        End If

                        nReturn = m_oCurrency.GetDetails(vCurrencyID:=m_vExtraPolicyData(ACXCurrencyID))
                        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            ' Log Error.
                            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the currency business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildPolicyVersionsListview")
                            Return nResult
                        End If

                        m_iCurrencyID = m_vExtraPolicyData(ACXCurrencyID)

                        nReturn = m_oCurrency.GetNext(vDescription:=sCurrDesc)
                        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            ' Log Error.
                            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get next from the currency business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildPolicyVersionsListview")
                            Return nResult
                        End If

                        If Not (Convert.IsDBNull(m_vExtraPolicyData(ACXLeadAgentCnt)) Or IsNothing(m_vExtraPolicyData(ACXLeadAgentCnt))) Then

                            nReturn = m_oExtraPolicyInfoBusiness.GetOtherDetails(vInsurerCnt:=Nothing, vInsurerName:="", vBrokerCnt:=Nothing, vBrokerName:="", vRiskId:=Nothing, vRiskDesc:="", vRiskGroupId:="", vAnalysisId:=Nothing, vAnalysisDesc:="", vHandlerCnt:=Nothing, vHandlerName:="", vAgentCnt:=m_vExtraPolicyData(ACXLeadAgentCnt), vAgentName:=sAgentName, vInsuranceFileCnt:=lInsFileCnt, vRelatedPolicyCnt:="", vRelatedPolicyCode:=Nothing, vRelationshipType:="", vPolicyTypeId:=Nothing, vPolicyTypeDesc:="", vSchemeId:=Nothing, vSchemeDesc:="")

                            If nReturn = gPMConstants.PMEReturnCode.PMError Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                ' Log Error.
                                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the extra policy details business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildPolicyVersionsListview")
                                Return nResult
                            End If
                        Else
                            sAgentName = ""
                        End If

                        ' AMB 20/08/2002 - get full list of insured persons, not just primary - START
                        nReturn = m_oExtraPolicyInfoBusiness.GetPolicyClient(v_lInsuranceFolderCnt:=lInsFolderCnt, v_lPartyCnt:=m_lInsHolderCnt, r_vResultArray:=vInsuredResults, v_lInsuranceFileCnt:=Trim(m_vPolicyVersionSearchData(ACPVInsuranceFileCnt, nLoopy)))
                        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            ' Log Error.
                            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get insured names from the extra policy details business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildPolicyVersionsListview")
                            Return nResult
                        End If

                        ' build up a string of insured persons
                        If Information.IsArray(vInsuredResults) Then
                            ' find the lead insurer, which could be anywhere in the array
                            ' this name should be at the front of the string
                            sInsuredLead = ""
                            sInsuredRest = New StringBuilder("")

                            nMin2 = vInsuredResults.GetLowerBound(1)

                            nMax2 = vInsuredResults.GetUpperBound(1)

                            For lLoopy2 As Integer = nMin2 To nMax2

                                If gPMFunctions.NullToString(CStr(vInsuredResults(klEPIIsLead, lLoopy2))) = "1" Then

                                    sInsuredLead = gPMFunctions.NullToString(CStr(vInsuredResults(klEPIResolvedName, lLoopy2)))
                                Else
                                    ' it's one of the other insured persons, not the lead

                                    sInsuredRest.Append(", " &
                                                     gPMFunctions.NullToString(CStr(vInsuredResults(klEPIResolvedName, lLoopy2))))
                                End If
                            Next lLoopy2
                        End If

                        ' AMB 20/08/2002 - get full list of insured persons, not just primary - END

                        'sj 24/07/2002 - Get rid of nulls (replace "Trim" with "NullToString")
                        ' inception date
                        lstCurrListItem = lvwVersions.Items.Add(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=gPMFunctions.NullToString(CStr(m_vPolicyVersionSearchData(ACPVCoverStartDate, nLoopy)))))

                        ' renewal date
                        ListViewHelper.GetListViewSubItem(lstCurrListItem, klColRenewalDate - 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=gPMFunctions.NullToString(CStr(m_vPolicyVersionSearchData(ACPVRenewalDate, nLoopy))))

                        ' Lapsed date (only visible in underwriting)
                        sLapsedDate = gPMFunctions.NullToString(CStr(m_vPolicyVersionSearchData(ACPVLapsedDate, nLoopy)))
                        If sLapsedDate.Trim() <> "" Then
                            If CDate(sLapsedDate).Year = 1899 Then
                                sLapsedDate = ""
                            End If
                        End If
                        ListViewHelper.GetListViewSubItem(lstCurrListItem, klColLapsedDate - 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=sLapsedDate)

                        ' insured persons
                        ListViewHelper.GetListViewSubItem(lstCurrListItem, klColInsuredPersons - 1).Text = sInsuredLead & sInsuredRest.ToString()

                        ' regarding
                        ListViewHelper.GetListViewSubItem(lstCurrListItem, klColRegarding - 1).Text = CStr(m_vPolicyVersionSearchData(ACPVRegarding, nLoopy))

                        ' billing method
                        ListViewHelper.GetListViewSubItem(lstCurrListItem, klColBillingMethod - 1).Text = gPMFunctions.NullToString(CStr(m_vExtraPolicyData(ACXMediaType)))

                        ' amount
                        ListViewHelper.GetListViewSubItem(lstCurrListItem, klColAmount - 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=gPMFunctions.NullToString(ToSafeDecimal(CStr(m_vPolicyVersionSearchData(ACPVPremium, nLoopy)))))

                        'Currency
                        ListViewHelper.GetListViewSubItem(lstCurrListItem, klColCurrency - 1).Text = sCurrDesc.Trim()

                        ' intermediary
                        ListViewHelper.GetListViewSubItem(lstCurrListItem, klColIntermediary - 1).Text = sAgentName.Trim()

                        ' Set type
                        ListViewHelper.GetListViewSubItem(lstCurrListItem, klColPolicyType - 1).Text = gPMFunctions.NullToString(CStr(m_vPolicyVersionSearchData(ACPVInsuranceFileType, nLoopy)))

                        ' policy status
                        'Kevin Renshaw (CMG)26/2/2003 - issue 2472 All policies showing as live
                        '              copied functionality from ListPolicyVersionControl
                        If CStr(m_vPolicyVersionSearchData(ACPVInsuranceFileStatus, nLoopy)) = "" Then
                            If nLoopy = 0 Then
                                'This is the current live version within all policy transactions
                                ListViewHelper.GetListViewSubItem(lstCurrListItem, klColPolicyStatus - 1).Text = "Current"
                            Else
                                ListViewHelper.GetListViewSubItem(lstCurrListItem, klColPolicyStatus - 1).Text = "Live"
                            End If
                        Else
                            ListViewHelper.GetListViewSubItem(lstCurrListItem, klColPolicyStatus - 1).Text = gPMFunctions.NullToString(CStr(m_vPolicyVersionSearchData(ACPVInsuranceFileStatus, nLoopy)))
                        End If
                        ListViewHelper.GetListViewSubItem(lstCurrListItem, klColTransactionDate - 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateTimeShort, vFieldValue:=gPMFunctions.NullToString(CStr(m_vPolicyVersionSearchData(ACPVLastTransDate, nLoopy))))

                        ' Gaurav Changed
                        ListViewHelper.GetListViewSubItem(lstCurrListItem, klColEventDescription - 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=gPMFunctions.NullToString(CStr(m_vPolicyVersionSearchData(ACPVEventDescription, nLoopy))))


                        ' set tag to lInsFileCnt
                        'Kevin Renshaw (CMG) 25/2/2003 - issue 2418.
                        lstCurrListItem.Tag = CStr(m_vPolicyVersionSearchData(ACPVInsuranceFileCnt, nLoopy)).Trim()
                        '            End If
                    Next nLoopy

                    ' if we've got this far, let's get the risks for the first policy version
                    nReturn = CType(BuildRiskListview(Convert.ToInt32(lvwVersions.Items.Item(0).Tag)), gPMConstants.PMEReturnCode)
                Else

                    '---------------------------------------------------
                    ' clear both listviews
                    lvwVersions.Items.Clear()
                    lvwRisks.Items.Clear()

                    ' add the policy versions to the listview
                    nMin1 = m_vPolicyVersionSearchData.GetLowerBound(1)
                    nMax1 = m_vPolicyVersionSearchData.GetUpperBound(1)
                    For nLoopy As Integer = nMin1 To nMax1

                        If Not ((m_vPolicyVersionSearchData(ACPVInsuranceFileTypeID, nLoopy) = "4" AndAlso Trim(m_vPolicyVersionSearchData(ACPVInsuranceFileStatus, nLoopy)) = "Cancelled") OrElse
                            (m_vPolicyVersionSearchData(ACPVInsuranceFileTypeID, nLoopy) = "10" AndAlso Trim(m_vPolicyVersionSearchData(ACPVInsuranceFileStatus, nLoopy)) = "Cancelled") OrElse
                            (m_vPolicyVersionSearchData(ACPVInsuranceFileTypeID, nLoopy) = "12" AndAlso Trim(m_vPolicyVersionSearchData(ACPVInsuranceFileStatus, nLoopy)) = "Cancelled")) Then

                            ' get the extra info we need about the policy

                            nReturn = m_oExtraPolicyInfoBusiness.GetDetails(vInsuranceFileCnt:=CStr(m_vPolicyVersionSearchData(ACPVInsuranceFileCnt, nLoopy)).Trim())
                            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                ' Log Error.
                                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the extra policy info business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildPolicyVersionsListview")
                                Return nResult
                            End If


                            nReturn = m_oExtraPolicyInfoBusiness.GetNext(r_vFieldArray:=m_vExtraPolicyData)
                            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                ' Log Error.
                                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the extra policy details business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildPolicyVersionsListview")
                                Return nResult
                            End If


                            nReturn = m_oCurrency.GetDetails(vCurrencyID:=m_vExtraPolicyData(ACXCurrencyID))
                            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                ' Log Error.
                                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the currency business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildPolicyVersionsListview")
                                Return nResult
                            End If

                            m_iCurrencyID = m_vExtraPolicyData(ACXCurrencyID)

                            nReturn = m_oCurrency.GetNext(vDescription:=sCurrDesc)
                            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                ' Log Error.
                                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get next from the currency business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildPolicyVersionsListview")
                                Return nResult
                            End If

                            ' get agent name

                            If Not (Convert.IsDBNull(m_vExtraPolicyData(ACXLeadAgentCnt)) Or IsNothing(m_vExtraPolicyData(ACXLeadAgentCnt))) Then



                                nReturn = m_oExtraPolicyInfoBusiness.GetOtherDetails(vInsurerCnt:=Nothing, vInsurerName:="", vBrokerCnt:=Nothing, vBrokerName:="", vRiskId:=Nothing, vRiskDesc:="", vRiskGroupId:="", vAnalysisId:=Nothing, vAnalysisDesc:="", vHandlerCnt:=Nothing, vHandlerName:="", vAgentCnt:=m_vExtraPolicyData(ACXLeadAgentCnt), vAgentName:=sAgentName, vInsuranceFileCnt:=lInsFileCnt, vRelatedPolicyCnt:="", vRelatedPolicyCode:=Nothing, vRelationshipType:="", vPolicyTypeId:=Nothing, vPolicyTypeDesc:="", vSchemeId:=Nothing, vSchemeDesc:="")

                                If nReturn = gPMConstants.PMEReturnCode.PMError Then
                                    nResult = gPMConstants.PMEReturnCode.PMFalse
                                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                    ' Log Error.
                                    gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the extra policy details business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildPolicyVersionsListview")
                                    Return nResult
                                End If
                            Else
                                sAgentName = ""
                            End If

                            ' AMB 20/08/2002 - get full list of insured persons, not just primary - START

                            nReturn = m_oExtraPolicyInfoBusiness.GetPolicyClient(v_lInsuranceFolderCnt:=lInsFolderCnt, v_lPartyCnt:=m_lInsHolderCnt, r_vResultArray:=vInsuredResults, v_lInsuranceFileCnt:=CStr(m_vPolicyVersionSearchData(ACPVInsuranceFileCnt, nLoopy)).Trim())

                            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                ' Log Error.
                                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get insured names from the extra policy details business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildPolicyVersionsListview")
                                Return nResult
                            End If

                            ' build up a string of insured persons
                            If Information.IsArray(vInsuredResults) Then

                                ' find the lead insurer, which could be anywhere in the array
                                ' this name should be at the front of the string
                                sInsuredLead = ""
                                sInsuredRest = New StringBuilder("")

                                nMin2 = vInsuredResults.GetLowerBound(1)

                                nMax2 = vInsuredResults.GetUpperBound(1)

                                For lLoopy2 As Integer = nMin2 To nMax2

                                    If gPMFunctions.NullToString(CStr(vInsuredResults(klEPIIsLead, lLoopy2))) = "1" Then

                                        sInsuredLead = gPMFunctions.NullToString(CStr(vInsuredResults(klEPIResolvedName, lLoopy2)))
                                    Else
                                        ' it's one of the other insured persons, not the lead

                                        sInsuredRest.Append(", " &
                                                            gPMFunctions.NullToString(CStr(vInsuredResults(klEPIResolvedName, lLoopy2))))
                                    End If
                                Next lLoopy2

                            End If
                            ' AMB 20/08/2002 - get full list of insured persons, not just primary - END

                            'sj 24/07/2002 - Get rid of nulls (replace "Trim" with "NullToString")
                            ' inception date
                            lstCurrListItem = lvwVersions.Items.Add(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=gPMFunctions.NullToString(CStr(m_vPolicyVersionSearchData(ACPVCoverStartDate, nLoopy)))))

                            ' renewal date
                            ListViewHelper.GetListViewSubItem(lstCurrListItem, klColRenewalDate - 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=gPMFunctions.NullToString(CStr(m_vPolicyVersionSearchData(ACPVRenewalDate, nLoopy))))

                            ' Lapsed date (only visible in underwriting)
                            sLapsedDate = gPMFunctions.NullToString(CStr(m_vPolicyVersionSearchData(ACPVLapsedDate, nLoopy)))
                            If sLapsedDate.Trim() <> "" Then
                                If CDate(sLapsedDate).Year = 1899 Then
                                    sLapsedDate = ""
                                End If
                            End If

                            ListViewHelper.GetListViewSubItem(lstCurrListItem, klColLapsedDate - 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=sLapsedDate)

                            ' insured persons
                            ListViewHelper.GetListViewSubItem(lstCurrListItem, klColInsuredPersons - 1).Text = sInsuredLead & sInsuredRest.ToString()

                            ' regarding
                            ListViewHelper.GetListViewSubItem(lstCurrListItem, klColRegarding - 1).Text = CStr(m_vPolicyVersionSearchData(ACPVRegarding, nLoopy))

                            ' billing method
                            ListViewHelper.GetListViewSubItem(lstCurrListItem, klColBillingMethod - 1).Text = gPMFunctions.NullToString(CStr(m_vExtraPolicyData(ACXMediaType)))

                            ' amount
                            ListViewHelper.GetListViewSubItem(lstCurrListItem, klColAmount - 1).Text = Convert.ToString(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=gPMFunctions.NullToString(ToSafeDecimal(CStr(m_vPolicyVersionSearchData(ACPVPremium, nLoopy))))))

                            'Currency
                            ListViewHelper.GetListViewSubItem(lstCurrListItem, klColCurrency - 1).Text = sCurrDesc.Trim()

                            ' intermediary
                            ListViewHelper.GetListViewSubItem(lstCurrListItem, klColIntermediary - 1).Text = sAgentName.Trim()

                            ' Set type
                            ListViewHelper.GetListViewSubItem(lstCurrListItem, klColPolicyType - 1).Text = gPMFunctions.NullToString(CStr(m_vPolicyVersionSearchData(ACPVInsuranceFileType, nLoopy)))

                            ' policy status
                            'Kevin Renshaw (CMG)26/2/2003 - issue 2472 All policies showing as live
                            '              copied functionality from ListPolicyVersionControl
                            If CStr(m_vPolicyVersionSearchData(ACPVInsuranceFileStatus, nLoopy)) = "" Then
                                If nLoopy = 0 Then
                                    'This is the current live version within all policy transactions
                                    ListViewHelper.GetListViewSubItem(lstCurrListItem, klColPolicyStatus - 1).Text = "Current"
                                Else
                                    ListViewHelper.GetListViewSubItem(lstCurrListItem, klColPolicyStatus - 1).Text = "Live"
                                End If
                            Else
                                ListViewHelper.GetListViewSubItem(lstCurrListItem, klColPolicyStatus - 1).Text = gPMFunctions.NullToString(CStr(m_vPolicyVersionSearchData(ACPVInsuranceFileStatus, nLoopy)))
                            End If

                            ListViewHelper.GetListViewSubItem(lstCurrListItem, klColTransactionDate - 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateTimeShort, vFieldValue:=gPMFunctions.NullToString(CStr(m_vPolicyVersionSearchData(ACPVLastTransDate, nLoopy))))

                            ' Gaurav Changed
                            ListViewHelper.GetListViewSubItem(lstCurrListItem, klColEventDescription - 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=gPMFunctions.NullToString(CStr(m_vPolicyVersionSearchData(ACPVEventDescription, nLoopy))))


                            ' set tag to lInsFileCnt
                            'Kevin Renshaw (CMG) 25/2/2003 - issue 2418.
                            lstCurrListItem.Tag = CStr(m_vPolicyVersionSearchData(ACPVInsuranceFileCnt, nLoopy)).Trim()
                        End If
                    Next nLoopy

                    ' if we've got this far, let's get the risks for the first policy version
                    nReturn = CType(BuildRiskListview(Convert.ToInt32(lvwVersions.Items.Item(0).Tag)), gPMConstants.PMEReturnCode)

                End If
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed while loading the policy version listview", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildPolicyVersionsListview", excep:=excep)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return gPMConstants.PMEReturnCode.PMFalse

        End Try
    End Function
    ''' <summary>
    ''' BuildRiskListview
    ''' </summary>
    ''' <param name="lInsFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function BuildRiskListview(ByVal lInsFileCnt As Integer) As Integer
        ' build up the policy versions listview

        Dim nResult As Integer
        Dim lstCurrListItem As ListViewItem
        Dim sRiskLinkStatus As String = String.Empty
        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            m_lReturn = m_oRiskyBusiness.SearchAll(r_vResultArray:=m_vRiskSearchData,
                                                    v_vInsuranceFileCnt:=lInsFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Return nResult
                End If
            End If

            If Information.IsArray(m_vRiskSearchData) Then

                ' add the policy versions to the listview
                lvwRisks.Items.Clear()

                For lLoopy As Integer = m_vRiskSearchData.GetLowerBound(1) To m_vRiskSearchData.GetUpperBound(1)

                    'sj 24/07/2002 - Get rid of nulls (replace "Trim" with "NullToString")
                    ' risk description
                    lstCurrListItem =
                        lvwRisks.Items.Add(gPMFunctions.NullToString(CStr(m_vRiskSearchData(ACIRiskDescription, lLoopy))))

                    ' coverage
                    ListViewHelper.GetListViewSubItem(lstCurrListItem, klColCoverage - 1).Text = "" '_
                    'Trim (m_vRiskSearchData(ACPVRenewalDate, lLoopy))

                    ' sum insured
                    ListViewHelper.GetListViewSubItem(lstCurrListItem, klColSumInsured - 1).Text =
                        gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency,
                                                    vFieldValue:=
                                                    gPMFunctions.NullToString(
                                                        CStr(m_vRiskSearchData(ACIRiskTotalSumInsured, lLoopy))))

                    ' excess
                    ListViewHelper.GetListViewSubItem(lstCurrListItem, klColExcess - 1).Text = "" '_
                    'Trim(m_vRiskSearchData("", lLoopy))

                    ' extensions
                    ListViewHelper.GetListViewSubItem(lstCurrListItem, klColExtensions - 1).Text = "" '_
                    'Trim(m_vRiskSearchData(ACPVPremium, lLoopy))

                    ' NCB
                    ListViewHelper.GetListViewSubItem(lstCurrListItem, klColNCB - 1).Text = "" '_
                    'Trim (m_vRiskSearchData("", lLoopy))

                    If gPMFunctions.NullToString(CStr(m_vRiskSearchData(kIRiskLinkStatus, lLoopy))).ToUpper() <> "U" _
                        Then
                        ' Gross Premium
                        ListViewHelper.GetListViewSubItem(lstCurrListItem, klColGrossPremium - 1).Text =
                            gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency,
                                                        vFieldValue:=
                                                        CStr(
                                                            (gPMFunctions.NullToDouble(
                                                                m_vRiskSearchData(ACIRiskTotalAnnualPremium, lLoopy))) +
                                                            (gPMFunctions.NullToDouble(m_vRiskSearchData(ACIRiskTotalFee,
                                                                                                         lLoopy))) +
                                                            (gPMFunctions.NullToDouble(m_vRiskSearchData(ACIRiskFeeTax,
                                                                                                         lLoopy)))))
                    Else
                        ListViewHelper.GetListViewSubItem(lstCurrListItem, klColGrossPremium - 1).Text = "0.00"
                    End If
                    ' Deleted

                    'If gPMFunctions.NullToString(CStr(m_vRiskSearchData(ACIRiskStatusFlag, lLoopy))).ToUpper() = "D" _
                    '    Then
                    '    ListViewHelper.GetListViewSubItem(lstCurrListItem, klColDeleted - 1).Text = "X"
                    'End If
                    If Convert.IsDBNull(gPMFunctions.NullToString(CStr(m_vRiskSearchData(kIRiskLinkStatus, lLoopy))).ToUpper()) Then
                        ListViewHelper.GetListViewSubItem(lstCurrListItem, klColRiskLinkStatus - 1).Text = ""
                    Else
                        Select Case gPMFunctions.NullToString(CStr(m_vRiskSearchData(kIRiskLinkStatus, lLoopy))).ToUpper()
                            Case "U"
                                sRiskLinkStatus = "Unchanged"
                            Case "C"
                                If ToSafeLong(m_vRiskSearchData(kIOriginalRiskCnt, lLoopy), -1) = -1 Then
                                    sRiskLinkStatus = "Added"
                                Else
                                    sRiskLinkStatus = "Changed"
                                End If
                            Case "D"
                                sRiskLinkStatus = "Deleted"
                            Case "R"
                                sRiskLinkStatus = "Renewed"
                            Case Else
                                sRiskLinkStatus = Trim(m_vRiskSearchData(kIRiskLinkStatus, lLoopy))
                        End Select
                        ListViewHelper.GetListViewSubItem(lstCurrListItem, klColRiskLinkStatus - 1).Text =
                            sRiskLinkStatus
                    End If

                    If Convert.IsDBNull(gPMFunctions.NullToString(CStr(m_vRiskSearchData(kIRiskLingDate, lLoopy)))) Then
                        ListViewHelper.GetListViewSubItem(lstCurrListItem, klColRiskCahngeDate - 1).Text = ""
                    Else
                        ListViewHelper.GetListViewSubItem(lstCurrListItem, klColRiskCahngeDate - 1).Text =
                            gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort,
                                                     vFieldValue:=
                                                        gPMFunctions.NullToString(CStr(m_vRiskSearchData(kIRiskLingDate,
                                                                                                         lLoopy))))
                    End If
                    lstCurrListItem.Tag = CStr(lLoopy) 'lInsFileCnt

                Next lLoopy

            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed while loading the policy version listview", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildRiskListview", excep:=excep)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return gPMConstants.PMEReturnCode.PMFalse

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim sInsRef, sInsFileType As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Disable parts of the interface while
            ' a search is in progress.
            m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sInsRef = "%"

            sInsFileType = "%"

            ' CF11088 - Added this so after a double click, GetPolicies works.
            m_lInsFileCnt = 0

            ' 070699 ECK


            m_lReturn = m_oBusiness.SearchAll(r_vResultArray:=m_vSearchData, v_vInsuranceRef:="", v_vInsFileType:=sInsFileType, v_vShortName:=m_sShortName.Trim(), v_vPartyCnt:=m_lInsHolderCnt)


            m_lReturn = m_oBusiness.SearchOtherPolicies(v_vPartyCnt:=m_lInsHolderCnt, r_vResultArray:=m_vSearchOtherData)



            ' {* USER DEFINED CODE (End) *}

            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.
                    m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)

                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No search details found.

                Case Else
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
            End Select

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
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '
    Private Function DisableInterface(ByVal bDisable As Boolean) As Integer

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
    ' Name: GetPolicies
    '
    ' Description: Gets the interface details and sets the appropriate
    '              style.
    '
    ' ***************************************************************** '
    Public Function GetPolicies() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the interface details from the business object.
            m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    ' Tomo010200 - Include policy types, and include the code to
    '              populate the life stuff, not that it'll ever be used
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0



        Dim sString As String = ""

        'Const ACFindImage As String = "FindImage"      ''Unused Local Variables

        Dim lNodeMin, lNodeMax, lNodeMaxOtherData, lNodeMinOtherData, lReturn As Integer
        Dim sStatus As String = ""
        Dim nCurrNode As TreeNode

        Dim lArraryCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            RemoveHandler tvwPolicies.AfterSelect, AddressOf tvwPolicies_AfterSelect
            ' Update the interface details.

            'm_lItemsFound = 0

            ' Check that search details are valid before
            ' continuing.
            If (Not Information.IsArray(m_vSearchData)) And (Not Information.IsArray(m_vSearchOtherData)) Then
                ' put a message in the policy treeview
                tvwPolicies.Nodes.Clear()
                tvwPolicies.ImageList = imgPolicies
                nCurrNode = tvwPolicies.Nodes.Add(kSRootNode, "Client has no policies", "Closed", "Open")
                tvwPolicies.Enabled = False
                cmdPrint.Enabled = False
                Return result
            Else
                tvwPolicies.Enabled = True
                cmdPrint.Enabled = True
            End If

            If Information.IsArray(m_vSearchData) Then
                lNodeMin = m_vSearchData.GetLowerBound(1)
                lNodeMax = m_vSearchData.GetUpperBound(1)

                ' re-dimension the treeview data array before we fill it
                aTreeNodes = Array.CreateInstance(GetType(udtTreeData), New Integer() {lNodeMax - lNodeMin + 1}, New Integer() {lNodeMin})

                For lLoopy As Integer = lNodeMin To lNodeMax


                    If Convert.IsDBNull(m_vSearchData(ACIStatus, lLoopy)) Or IsNothing(m_vSearchData(ACIStatus, lLoopy)) Then
                        If CStr(m_vSearchData(ACIInsFileType, lLoopy)).Trim().ToUpper() = "QUOTE" Then
                            sStatus = "QUOTE"
                        ElseIf (CStr(m_vSearchData(ACIInsFileType, lLoopy)).Trim().ToUpper() = "RENEWAL") Then
                            sStatus = "IN RENEWAL"
                            'Start Written Status-
                        ElseIf (ToSafeString(m_vSearchData(ACIInsFileType, lLoopy)).Trim().ToUpper() = "WRITTEN") Then
                            sStatus = "WRITTEN"
                            'End  Written Status-
                        Else
                            sStatus = "LIVE" '"ACTIVE"
                        End If
                    Else
                        Select Case CStr(m_vSearchData(ACIStatus, lLoopy)).Trim().ToUpper()
                            Case "CAN"
                                sStatus = "CANCELLED"
                            Case "LAP"
                                sStatus = "LAPSED"
                            Case "REN"
                                'Start - (Sankar) - (Tech Spec - VAL P14 - Policy Numbering - Client Manager Changes.doc) - (5.1.1.1)
                                'Changed from RENEWED to UNDER RENEWAL
                                sStatus = "UNDER RENEWAL"
                                'End - (Sankar) - (Tech Spec - VAL P14 - Policy Numbering - Client Manager Changes.doc) - (5.1.1.1)
                            Case "TRA"
                                sStatus = "TRANSFERRED"
                            Case "REP"
                                'Start - (Sankar) - (Tech Spec - VAL P14 - Policy Numbering - Client Manager Changes.doc) - (5.1.1.1)
                                'Changed from REPLACED to RENEWED
                                sStatus = "RENEWED"
                                'End - (Sankar) - (Tech Spec - VAL P14 - Policy Numbering - Client Manager Changes.doc) - (5.1.1.1)
                        End Select
                    End If

                    aTreeNodes(lLoopy).sStatus = sStatus
                    aTreeNodes(lLoopy).sProductCode = CStr(m_vSearchData(ACIProductCode, lLoopy)).Trim()
                    aTreeNodes(lLoopy).sProductDesc = CStr(m_vSearchData(ACIProductName, lLoopy)).Trim()
                    aTreeNodes(lLoopy).sPolicyNo = CStr(m_vSearchData(ACIInsReference, lLoopy)).Trim()
                    aTreeNodes(lLoopy).lInsFileCnt = CInt(CStr(m_vSearchData(ACIInsFileCnt, lLoopy)).Trim())
                    aTreeNodes(lLoopy).lInsFolderCnt = CInt(CStr(m_vSearchData(ACIInsFolderCnt, lLoopy)).Trim())
                    aTreeNodes(lLoopy).lAnniversaryCopy = gPMFunctions.ToSafeLong(CStr(m_vSearchData(ACIAnniversaryCopy, lLoopy)), 0)
                    aTreeNodes(lLoopy).nProductId = ToSafeInteger(m_vSearchData(ACIProductID, lLoopy).Trim())

                    ' store the number of claims that have been associated with this policy
                    If Not IsNothing(m_vSearchData(ACINumberOfClaims, lLoopy)) Then
                        If CStr(m_vSearchData(ACINumberOfClaims, lLoopy)).Trim() = "" Then
                            aTreeNodes(lLoopy).lNumberOfClaims = 0
                        Else
                            aTreeNodes(lLoopy).lNumberOfClaims = CInt(m_vSearchData(ACINumberOfClaims, lLoopy)) + CInt(m_vSearchData(ACIBlank, lLoopy))
                        End If
                    Else
                        aTreeNodes(lLoopy).lNumberOfClaims = 0
                    End If

                Next lLoopy
            End If

            '<Pk>
            If Information.IsArray(m_vSearchOtherData) Then
                ' re-dimension the treeview data array with preserving the previous values if exists
                If Not Information.IsArray(m_vSearchData) Then
                    lNodeMinOtherData = m_vSearchOtherData.GetLowerBound(1)
                    lNodeMaxOtherData = m_vSearchOtherData.GetUpperBound(1)
                    aTreeNodes = Array.CreateInstance(GetType(udtTreeData), New Integer() {lNodeMaxOtherData - lNodeMin + 1}, New Integer() {lNodeMin})
                Else
                    lNodeMinOtherData = m_vSearchOtherData.GetLowerBound(1) + lNodeMax + 1
                    lNodeMaxOtherData = m_vSearchOtherData.GetUpperBound(1) + lNodeMax + 1
                    aTreeNodes = ArraysHelper.RedimPreserve(Of udtTreeData())(aTreeNodes, New Integer() {lNodeMaxOtherData - lNodeMin + 1}, New Integer() {lNodeMin})
                End If

                lArraryCnt = m_vSearchOtherData.GetLowerBound(1)

                For lLoopy As Integer = lNodeMinOtherData To lNodeMaxOtherData


                    If Convert.IsDBNull(m_vSearchOtherData(8, lArraryCnt)) Or IsNothing(m_vSearchOtherData(8, lArraryCnt)) Then
                        If CStr(m_vSearchOtherData(7, lArraryCnt)).Trim().ToUpper() = "QUOTE" Then
                            sStatus = "QUOTE"
                        ElseIf (CStr(m_vSearchOtherData(7, lArraryCnt)).Trim().ToUpper() = "RENEWAL") Then
                            sStatus = "IN RENEWAL"
                            'Start -Written Status-
                        ElseIf (ToSafeString(m_vSearchOtherData(7, lArraryCnt)).Trim().ToUpper() = "WRITTEN") Then
                            sStatus = "WRITTEN"
                            'End -  Written Status
                        Else
                            sStatus = "LIVE" '"ACTIVE"
                        End If
                    Else
                        Select Case CStr(m_vSearchOtherData(8, lArraryCnt)).Trim().ToUpper()
                            Case "CAN"
                                sStatus = "CANCELLED"
                            Case "LAP"
                                sStatus = "LAPSED"
                            Case "REN"
                                sStatus = "UNDER RENEWAL"
                            Case "TRA"
                                sStatus = "TRANSFERRED"
                            Case "REP"
                                sStatus = "RENEWED"
                        End Select
                    End If

                    aTreeNodes(lLoopy).sStatus = sStatus
                    aTreeNodes(lLoopy).sStatusOther = "Other Policies"
                    aTreeNodes(lLoopy).sProductCode = CStr(m_vSearchOtherData(3, lArraryCnt)).Trim()
                    aTreeNodes(lLoopy).sProductDesc = CStr(m_vSearchOtherData(4, lArraryCnt)).Trim()
                    aTreeNodes(lLoopy).sPolicyNo = CStr(m_vSearchOtherData(1, lArraryCnt)).Trim()
                    aTreeNodes(lLoopy).lInsFileCnt = CInt(CStr(m_vSearchOtherData(0, lArraryCnt)).Trim())
                    aTreeNodes(lLoopy).lInsFolderCnt = CInt(CStr(m_vSearchOtherData(2, lArraryCnt)).Trim())
                    aTreeNodes(lLoopy).lAnniversaryCopy = gPMFunctions.ToSafeLong(CStr(m_vSearchOtherData(5, lArraryCnt)), 0)
                    aTreeNodes(lLoopy).nProductId = ToSafeInteger(m_vSearchOtherData(7, lArraryCnt).Trim())

                    ' store the number of claims that have been associated with this policy
                    If Not IsNothing(m_vSearchOtherData(6, lArraryCnt)) Then
                        If CStr(m_vSearchOtherData(6, lArraryCnt)).Trim() = "" Then
                            aTreeNodes(lLoopy).lNumberOfClaims = 0
                        Else
                            aTreeNodes(lLoopy).lNumberOfClaims = CInt(m_vSearchOtherData(6, lArraryCnt))
                        End If
                    Else
                        aTreeNodes(lLoopy).lNumberOfClaims = 0
                    End If
                    lArraryCnt += 1
                Next lLoopy
            End If
            ' </Pk>

            lReturn = BuildPolicyTreeview(v_bIsOther:=Information.IsArray(m_vSearchOtherData))

            PopulatePolicyDropdown()
            AddHandler tvwPolicies.AfterSelect, AddressOf tvwPolicies_AfterSelect
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

            If iPMBListEvents.g_oObjectManager Is Nothing Then
                iPMBListEvents.g_oObjectManager = MainModule.g_oObjectManager
            End If


            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrieve Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
                'Developer Guide No solution 39
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

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oRiskyBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oRiskyBusiness, "bSIRFindRisk.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oRiskyBusiness = temp_m_oRiskyBusiness

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

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oExtraPolicyInfoBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oExtraPolicyInfoBusiness, "bSIRInsuranceFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oExtraPolicyInfoBusiness = temp_m_oExtraPolicyInfoBusiness

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

            Dim temp_m_oCurrency As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrency, "bACTCurrency.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oCurrency = temp_m_oCurrency

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

            '    m_bGeminiLink = m_oBusiness.GeminiLink
            '    m_bGeminiIILink = m_oBusiness.GeminiIILink
            '    m_bSwiftLink = m_oBusiness.SwiftLink

            'TN20001214 - Start

            m_sUnderwritingOrAgency = m_oBusiness.UnderwritingOrAgency
            'TN20001214 - End


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
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            ' {* USER DEFINED CODE (Begin) *}
            'eck090500
            m_lReturn = CType(GetValidSources(), gPMConstants.PMEReturnCode)
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
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load control", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl", excep:=excep)

            Return result

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


            m_lReturn = g_oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=g_iUserID)
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

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=excep)

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

            'sj 02/10/2002 - start
            With Toolbar1
                .ImageList = ImageList1
                .Items.Item("TB_Event").ImageIndex = 10
                .Items.Item("TB_RiskDetails").Visible = False
                .Items.Item("TB_InformationChecklist").Visible = False
            End With
            'sj 02/10/2002 - end

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the column widths for the search list.
            lvwVersions.Columns.Item(klColInceptionDate - 1).Width = CInt(VB6.TwipsToPixelsX(1500))
            lvwVersions.Columns.Item(klColRenewalDate - 1).Width = CInt(VB6.TwipsToPixelsX(1500))

            ' Only display "lapsed date" in underwriting
            If m_sUnderwritingOrAgency = "U" Then
                lvwVersions.Columns.Item(klColLapsedDate - 1).Width = CInt(VB6.TwipsToPixelsX(1800))
            Else
                lvwVersions.Columns.Item(klColLapsedDate - 1).Width = CInt(0)
            End If

            lvwVersions.Columns.Item(klColInsuredPersons - 1).Width = CInt(VB6.TwipsToPixelsX(2000))

            ' Only display "regarding" in underwriting
            If m_sUnderwritingOrAgency = "U" Then
                lvwVersions.Columns.Item(klColRegarding - 1).Width = CInt(VB6.TwipsToPixelsX(1800))
            Else
                lvwVersions.Columns.Item(klColRegarding - 1).Width = CInt(0)
            End If

            lvwVersions.Columns.Item(klColBillingMethod - 1).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwVersions.Columns.Item(klColAmount - 1).Width = CInt(VB6.TwipsToPixelsX(1200))
            lvwVersions.Columns.Item(klColIntermediary - 1).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwVersions.Columns.Item(klColPolicyType - 1).Width = CInt(VB6.TwipsToPixelsX(1500))
            lvwVersions.Columns.Item(klColPolicyStatus - 1).Width = CInt(VB6.TwipsToPixelsX(1500))
            lvwVersions.Columns.Item(klColTransactionDate - 1).Width = CInt(VB6.TwipsToPixelsX(1800))
            lvwRisks.Columns.Item(klColRiskDesc - 1).Width = CInt(VB6.TwipsToPixelsX(7400))
            lvwRisks.Columns.Item(klColSumInsured - 1).Width = CInt(VB6.TwipsToPixelsX(1500))
            lvwRisks.Columns.Item(klColGrossPremium - 1).Width = CInt(VB6.TwipsToPixelsX(1500))
            lvwRisks.Columns.Item(klColDeleted - 1).Width = CInt(VB6.TwipsToPixelsX(0))
            lvwRisks.Columns.Item(klColRiskLinkStatus - 1).Width = CInt(VB6.TwipsToPixelsX(1600))
            lvwRisks.Columns.Item(klColRiskCahngeDate - 1).Width = CInt(VB6.TwipsToPixelsX(1800))

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' No print support for underwriting policies so hide the button
            If m_sUnderwritingOrAgency = "U" Then
                cmdPrint.Visible = False
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception


            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", excep:=excep)

            Return nResult

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


        'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.13.1.1)
        'Declaration missing in the tech spec
        Dim iLanguageId As Integer
        'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.13.1.1)

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.13.1.1)
            m_lReturn = CType(gPMFunctions.GetUserIsAmericanLanguageID(iLanguageId), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display all language specific captions.



            lvwVersions.Columns.Item(klColInceptionDate - 1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACInceptionDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.13.1.1)

            '    If (m_sUnderwritingOrAgency = "A") Then
            '        lColumnPolicyNumber = ACAColumnPolicyNumber
            '        lColumnPolicyType = ACAColumnPolicyType
            '        lColumnRiskType = ACAColumnRiskType
            '        lColumnRegarding = ACAColumnRegarding
            '        lColumnRenewalDate = ACAColumnRenewalDate
            '        lColumnInsurer = ACAColumnInsurer
            '        lColumnPremium = ACAColumnPremium
            '        lColumnPolicyStatus = ACAColumnPolicyStatus
            '        lColumnRiskTypeDescription = ACAColumnRiskTypeDescription
            '        lColumnGeminiEDI = ACAColumnGeminiEDI
            '    Else
            '        lColumnPolicyNumber = ACUColumnPolicyNumber
            '        lColumnPolicyType = ACUColumnPolicyType
            '        lColumnRiskType = ACUColumnRiskType
            '        lColumnRegarding = ACUColumnRegarding
            '        lColumnRenewalDate = ACUColumnRenewalDate
            '        lColumnInsurer = ACUColumnInsurer
            '        lColumnPremium = ACUColumnPremium
            '        lColumnPolicyStatus = ACUColumnPolicyStatus
            '        lColumnRiskTypeDescription = ACUColumnRiskTypeDescription
            '        lColumnGeminiEDI = ACUColumnGeminiEDI
            '    End If
            '
            '
            '     ' {* USER DEFINED CODE (Begin) *}
            '    lblClient.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACClientCode, _
            ''        iDataType:=PMResString)
            '
            '    lblStatus.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACStatus, _
            ''        iDataType:=PMResString)
            '
            '    lvwSearchDetails.ColumnHeaders(lColumnPolicyNumber).Text = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACListTitlePolicyNumber, _
            ''        iDataType:=PMResString)
            '
            '    lvwSearchDetails.ColumnHeaders(lColumnRegarding).Text = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACListTitleRegarding, _
            ''        iDataType:=PMResString)
            '
            '    lvwSearchDetails.ColumnHeaders(lColumnRenewalDate).Text = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACListTitleRenewalDate, _
            ''        iDataType:=PMResString)
            '
            '    'RWH(10/04/2001) For UW we display Agent rather than Insurer.
            '    If (m_oBusiness.UnderwritingOrAgency = "U") Then
            '        lvwSearchDetails.ColumnHeaders(lColumnInsurer).Text = iPMFunc.GetResData( _
            ''                                                iLangID:=g_iLanguageID%, _
            ''                                                lID:=ACListTitleAgent, _
            ''                                                iDataType:=PMResString)
            '    Else
            '
            '        lvwSearchDetails.ColumnHeaders(lColumnInsurer).Text = iPMFunc.GetResData( _
            ''                                                iLangID:=g_iLanguageID%, _
            ''                                                lID:=ACListTitleInsurer, _
            ''                                                iDataType:=PMResString)
            '    End If
            '
            '    lvwSearchDetails.ColumnHeaders(lColumnRiskTypeDescription).Text = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACListTitleRiskTypeDescription, _
            ''        iDataType:=PMResString)
            '
            '    lvwSearchDetails.ColumnHeaders(lColumnPremium).Text = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACListTitlePremium, _
            ''        iDataType:=PMResString)
            '
            '    lvwSearchDetails.ColumnHeaders(lColumnPolicyStatus).Text = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACListTitlePolicyStatus, _
            ''        iDataType:=PMResString)
            '
            '    lvwSearchDetails.ColumnHeaders(lColumnPolicyType).Text = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACListTitlePolicyType, _
            ''        iDataType:=PMResString)
            '
            '    lvwSearchDetails.ColumnHeaders(lColumnRiskType).Text = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACListTitleRiskType, _
            ''        iDataType:=PMResString)
            '
            '    If m_bGeminiIILink Then
            '        Select Case m_lFindType
            '        Case 1
            '            lvwSearchDetails.ColumnHeaders(lColumnRenewalDate).Text = iPMFunc.GetResData( _
            ''                iLangID:=g_iLanguageID%, _
            ''                lID:=ACListTitleStartDate, _
            ''                iDataType:=PMResString)
            '        End Select
            '
            '        lvwSearchDetails.ColumnHeaders(lColumnGeminiEDI).Text = iPMFunc.GetResData( _
            ''            iLangID:=g_iLanguageID%, _
            ''            lID:=ACListTitleEDI, _
            ''            iDataType:=PMResString)
            '
            '    End If
            '
            '    tabMainTab.TabCaption(0) = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACTabTitle1, _
            ''        iDataType:=PMResString)
            '
            '      ' {* USER DEFINED CODE (End) *}

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


            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ' ReDim m_ctlTabFirstLast(1, 2)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", excep:=excep)

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
                If m_oCurrency IsNot Nothing Then
                    m_oCurrency.Dispose()
                    m_oCurrency = Nothing
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
                m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'eventArgs.cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Function
                End If

                '
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
                m_lReturn = CType(DataToProperties(), gPMConstants.PMEReturnCode)

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
    ' Name: DataToProperties
    '
    ' Description: Updates the property member from the search data
    '              storage.
    '
    ' ***************************************************************** '
    Public Function DataToProperties() As Integer

        Dim result As Integer = 0




        Try



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", excep:=excep)

            Return result



            Return result
        End Try
    End Function

    Private Function BuildPolicyTreeview(Optional ByVal v_bIsOther As Boolean = False, Optional ByVal v_sSelectedPolicy As Object = "") As Integer
        ' build up the treeview from the database query results
        Dim nCurrNode As TreeNode
        Dim sNodeKey As String = ""
        Dim sParentNodeKey As String = ""
        Dim lLowBound, lUpperBound As Integer
        Dim sImage As String = ""

        Dim sOtherPolicyNodeKey As String

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' always need the root node - clear the tree and stick it in
            tvwPolicies.Nodes.Clear()
            tvwPolicies.Sorted = False
            tvwPolicies.HideSelection = False 'pk

            sNodeKey = kSRootNode
            sOtherPolicyNodeKey = ksOtherPolicy

            tvwPolicies.ImageList = imgPolicies
            nCurrNode = tvwPolicies.Nodes.Add(sNodeKey, "All", "Closed", "Open")

            nCurrNode.Tag = ksRoot
            'tvwPolicies.Nodes.Item(0).Nodes.a()
            ' generate the status nodes
            lLowBound = aTreeNodes.GetLowerBound(0)
            lUpperBound = aTreeNodes.GetUpperBound(0)
            For lLoopy As Integer = lLowBound To lUpperBound
                ' get the parent key
                sParentNodeKey = tvwPolicies.Nodes.Item(kSRootNode).Name.ToUpper()
                ' create a new key
                sNodeKey = (sParentNodeKey & ksDelimiter & aTreeNodes(lLoopy).sStatus).ToUpper()
                ' check the node doesn't already exist
                If FindTreeviewNode(sNodeKey) = gPMConstants.PMEReturnCode.PMFalse Then
                    ' add the node
                    'In case of Selected Policy, we need only the "LIVE" node
                    If (v_sSelectedPolicy.Trim().Length > 0 And (aTreeNodes(lLoopy).sStatus = "LIVE" Or aTreeNodes(lLoopy).sStatus = "IN RENEWAL") _
                        And v_sSelectedPolicy.Trim().ToString().ToUpper = aTreeNodes(lLoopy).sPolicyNo.ToUpper()) Or v_sSelectedPolicy.Trim().Length = 0 Then
                        nCurrNode = tvwPolicies.Nodes.Find(kSRootNode, True)(0).Nodes.Add(sNodeKey, aTreeNodes(lLoopy).sStatus, "Closed", "Open")
                        ' tag it as a status
                        nCurrNode.Tag = ksStatus & ksDelimiter & CStr(lLoopy)
                        ' make sure we can see it
                        nCurrNode.EnsureVisible()
                    End If
                End If
            Next lLoopy

            ' generate the Other Policy nodes
            lLowBound = aTreeNodes.GetLowerBound(0)
            lUpperBound = aTreeNodes.GetUpperBound(0)
            For lLoopy As Integer = lLowBound To lUpperBound
                ' get the parent key
                If aTreeNodes(lLoopy).sStatusOther <> "" Then
                    sParentNodeKey = CStr(kSRootNode & ksDelimiter & aTreeNodes(lLoopy).sStatus.ToUpper()).ToUpper()
                    sNodeKey = (sParentNodeKey & ksDelimiter & aTreeNodes(lLoopy).sStatusOther).ToUpper()
                    ' check the node doesn't already exist
                    If FindTreeviewNode(sParentNodeKey) = gPMConstants.PMEReturnCode.PMTrue Then
                        If FindTreeviewNode(sNodeKey) = gPMConstants.PMEReturnCode.PMFalse Then

                            nCurrNode = tvwPolicies.Nodes.Find(sParentNodeKey, True)(0).Nodes.Add(sNodeKey, "Other Policies", "Closed", "Open")
                            ' tag it as a status
                            nCurrNode.Tag = ksOtherPolicy & ksDelimiter & CStr(lLoopy)
                            ' make sure we can see it
                            nCurrNode.EnsureVisible()
                        End If
                    End If
                End If

            Next lLoopy

            ' generate the product nodes
            lLowBound = aTreeNodes.GetLowerBound(0)
            lUpperBound = aTreeNodes.GetUpperBound(0)
            For lLoopy As Integer = lLowBound To lUpperBound
                ' get the parent key
                If aTreeNodes(lLoopy).sStatusOther <> "" Then
                    sParentNodeKey = (kSRootNode & ksDelimiter & aTreeNodes(lLoopy).sStatus.ToUpper() & ksDelimiter & aTreeNodes(lLoopy).sStatusOther.ToUpper()).ToUpper()
                    '           sParentNodeKey = UCase(tvwPolicies.Nodes( _
                    ''                kSRootNode & ksDelimiter & _
                    ''                UCase(aTreeNodes(lLoopy).sStatus) & ksDelimiter & _
                    ''                UCase(aTreeNodes(lLoopy).sStatusOther)).Key)

                Else
                    sParentNodeKey = (kSRootNode & ksDelimiter & aTreeNodes(lLoopy).sStatus.ToUpper()).ToUpper()
                    '            sParentNodeKey = UCase(tvwPolicies.Nodes( _
                    ''                kSRootNode & ksDelimiter & _
                    ''                UCase(aTreeNodes(lLoopy).sStatus)).Key)
                End If
                ' create a new key
                sNodeKey = (sParentNodeKey & ksDelimiter & aTreeNodes(lLoopy).sProductCode).ToUpper()

                If FindTreeviewNode(sParentNodeKey) = gPMConstants.PMEReturnCode.PMTrue Then
                    ' check the node doesn't already exist
                    If FindTreeviewNode(sNodeKey) = gPMConstants.PMEReturnCode.PMFalse Then
                        ' add the node
                        nCurrNode = tvwPolicies.Nodes.Find(sParentNodeKey, True)(0).Nodes.Add(sNodeKey, aTreeNodes(lLoopy).sProductDesc, "Closed", "Open")

                        ' tag it as a product
                        nCurrNode.Tag = ksProduct & ksDelimiter & CStr(lLoopy)
                        ' make sure we can see it
                        nCurrNode.EnsureVisible()
                    End If
                End If
            Next lLoopy

            ' generate the policy nodes
            lLowBound = aTreeNodes.GetLowerBound(0)
            lUpperBound = aTreeNodes.GetUpperBound(0)
            For lLoopy As Integer = lLowBound To lUpperBound
                ' get the parent key
                If aTreeNodes(lLoopy).sStatusOther <> "" Then
                    sParentNodeKey = (kSRootNode & ksDelimiter & aTreeNodes(lLoopy).sStatus.ToUpper() & ksDelimiter & aTreeNodes(lLoopy).sStatusOther.ToUpper() & ksDelimiter & aTreeNodes(lLoopy).sProductCode.ToUpper()).ToUpper()
                    '            sParentNodeKey = UCase(tvwPolicies.Nodes( _
                    ''                             kSRootNode & ksDelimiter & _
                    ''                             UCase(aTreeNodes(lLoopy).sStatus) & ksDelimiter & _
                    ''                             UCase(aTreeNodes(lLoopy).sStatusOther) & ksDelimiter & _
                    ''                             UCase(aTreeNodes(lLoopy).sProductCode)).Key)
                Else
                    sParentNodeKey = (kSRootNode & ksDelimiter & aTreeNodes(lLoopy).sStatus.ToUpper() & ksDelimiter & aTreeNodes(lLoopy).sProductCode.ToUpper()).ToUpper()
                    '            sParentNodeKey = UCase(tvwPolicies.Nodes( _
                    ''                             kSRootNode & ksDelimiter & _
                    ''                             UCase(aTreeNodes(lLoopy).sStatus) & ksDelimiter & _
                    ''                             UCase(aTreeNodes(lLoopy).sProductCode)).Key)
                End If

                ' create a new key
                sNodeKey = (sParentNodeKey & ksDelimiter & aTreeNodes(lLoopy).sPolicyNo).ToUpper()
                ' check the node doesn't already exist
                If FindTreeviewNode(sParentNodeKey) = gPMConstants.PMEReturnCode.PMTrue Then
                    If FindTreeviewNode(sNodeKey) = gPMConstants.PMEReturnCode.PMFalse Then

                        If aTreeNodes(lLoopy).lAnniversaryCopy = 1 Then
                            sImage = "AnniversaryPol"
                        Else
                            sImage = "Policy"
                        End If

                        ' add the node
                        If (v_sSelectedPolicy.Trim().Length > 0 And aTreeNodes(lLoopy).sPolicyNo = v_sSelectedPolicy) Or v_sSelectedPolicy.Trim().Length = 0 Then
                            nCurrNode = tvwPolicies.Nodes.Find(sParentNodeKey, True)(0).Nodes.Add(sNodeKey, aTreeNodes(lLoopy).sPolicyNo, sImage, sImage)

                            ' set the nodes back colour to indicate if the policy has
                            ' had any claims made against it.
                            If aTreeNodes(lLoopy).lNumberOfClaims = 0 Then
                                nCurrNode.BackColor = Color.White
                            Else
                                nCurrNode.BackColor = Color.Yellow
                            End If

                            ' tag it as a policy
                            nCurrNode.Tag = ksPolicy & ksDelimiter & CStr(lLoopy)

                            ' make sure we can see it
                            nCurrNode.EnsureVisible()
                        End If
                    End If
                End If
            Next lLoopy


            ' select the top node
            tvwPolicies.Nodes.Item(kSRootNode).EnsureVisible()

            tvwPolicies.SelectedNode = tvwPolicies.Nodes.Item(kSRootNode)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed while loading the treeview", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildPolicyTreeview", excep:=excep)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return gPMConstants.PMEReturnCode.PMFalse

        End Try
    End Function

    Function FindTreeviewNode(ByVal sNodeKey As String) As Integer
        ' returns PMTrue if a node with a key of sNodeKey exists
        ' PMFalse otherwise
        Dim nSearchNode1() As TreeNode

        Try

            For Each nSearchNode As TreeNode In tvwPolicies.Nodes
                nSearchNode1 = tvwPolicies.Nodes.Find(sNodeKey, True)
                If (nSearchNode1.Length > 0) Then
                    If nSearchNode1(0).Name = sNodeKey Then
                        Return gPMConstants.PMEReturnCode.PMTrue
                    End If
                End If

            Next nSearchNode


            Return gPMConstants.PMEReturnCode.PMFalse

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error searching treeview", vApp:=ACApp, vClass:=ACClass, vMethod:="FindTreeviewNode", excep:=excep)

            Return gPMConstants.PMEReturnCode.PMFalse

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ResizeInterface
    '
    ' Description: Resizes the interface controls.
    ' ***************************************************************** '
    Private Function ResizeInterface() As Integer
        ' Defaults for sizing
        Const m_SizeSafeZoneLeft = 250
        Const m_SizeSafeZoneRight = 535
        Const m_SizeSafeZoneTop = 150
        Const m_SizeSafeZoneBottom = 350
        Const m_SizeGrabBar = 9
        Const m_ButtonsHeight = 39
        Const m_AbsLeft = 0
        Const m_AbsTop = 32
        Const m_Spacer = 6

        ' We want to do our best to make the screen look decent
        ' so keep going regardless
        Try

            ' avoid getting into multiple re-sizing calls
            If Not m_bResizing Then

                m_bResizing = True

                ' move the navigator label and treeview
                lblNavigator.SetBounds(m_AbsLeft, m_AbsTop, 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

                tvwPolicies.SetBounds(m_AbsLeft, lblNavigator.Top + lblNavigator.Height + m_Spacer, ((ClientRectangle.Width * (1 / 5)) - m_Spacer), ClientRectangle.Height - (tvwPolicies.Top + (m_Spacer)))

                ' Now resize the frames
                fraVersions.SetBounds(tvwPolicies.Left + tvwPolicies.Width + m_Spacer, m_AbsTop, (ClientRectangle.Width * (4 / 5)), (ClientRectangle.Height * (2 / 5)))

                fraRisks.SetBounds(tvwPolicies.Left + tvwPolicies.Width + m_Spacer, fraVersions.Top + fraVersions.Height + m_Spacer, (ClientRectangle.Width * (4 / 5)), (ClientRectangle.Height * (2 / 5)))

                ' Resize the listview inside the frames
                lvwVersions.SetBounds(m_Spacer, m_Spacer * 6, (fraVersions.Width - (m_Spacer * 2)), fraVersions.Height - (m_Spacer * 7))

                lvwRisks.SetBounds(m_Spacer, m_Spacer * 3, (fraRisks.Width - (m_Spacer * 2)), fraRisks.Height - (m_Spacer * 4))

                ' Finally move the buttons, relative to right side of screen
                cmdView.SetBounds(ClientRectangle.Width - (cmdView.Width), ClientRectangle.Height - (cmdView.Height + m_Spacer), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
                cmdPrint.SetBounds(cmdView.Left - (cmdPrint.Width + (m_Spacer)), cmdView.Top, 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
                If cmdPrint.Visible Then
                    cmdCopyPolicy.SetBounds(cmdPrint.Left - (cmdCopyPolicy.Width + (m_Spacer)), cmdPrint.Top, 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
                Else
                    cmdCopyPolicy.SetBounds(cmdView.Left - (cmdCopyPolicy.Width + (m_Spacer)), cmdView.Top, 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
                End If

                m_bResizing = False

                cboLivePolicies.Width = IIf(tvwPolicies.Width - lblNavigator.Width > 165, 165, tvwPolicies.Width - lblNavigator.Width)
                cboLivePolicies.SetBounds(tvwPolicies.Left + tvwPolicies.Width - cboLivePolicies.Width, lblNavigator.Top, 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

            End If
        Catch ex As Exception

        End Try
        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    Private Sub cboLivePolicies_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboLivePolicies.SelectedIndexChanged


        Dim lReturn As gPMConstants.PMEReturnCode = CType(BuildPolicyTreeview(v_bIsOther:=Information.IsArray(m_vSearchOtherData), v_sSelectedPolicy:=IIf(cboLivePolicies.SelectedIndex > 0, gPMFunctions.ToSafeString(cboLivePolicies.Text.Trim()), "")), gPMConstants.PMEReturnCode)
        lvwVersions.Items.Clear()
        lvwVersions.Enabled = False
        lvwVersions.BackColor = SystemColors.Control
        lvwRisks.Items.Clear()
        lvwRisks.Enabled = False
        lvwRisks.BackColor = SystemColors.Control

    End Sub

    ' ***************************************************************** '
    ' Name: ResizeInterface
    '
    ' Description: Resizes the interface controls.
    '
    ' History :
    '               ??/??/????  ??? Created
    '               24/02/2003  APS Amended validation message
    ' ***************************************************************** '

    Private Sub cmdPrint_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPrint.Click

        If kbInDebug Then
            MessageBox.Show("InsHolderCnt: " & m_lInsHolderCnt, "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        'CJR 3/1/03 Only alow for none underwriting Policy Types. As discussed with Tom O'Toole
        'ISS1700
        If m_lPolicyTypeID = 5 Then
            MessageBox.Show("Unavailable for Underwriting Policy Type.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            m_lReturn = CType(RunReport(v_lPartyCnt:=m_lInsHolderCnt, v_sReportName:="Policy_List_By_PartyCnt"), gPMConstants.PMEReturnCode)
        End If
    End Sub

    Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click

        Try

            If bInVersions Then
                lvwVersions_DoubleClick(lvwVersions, New EventArgs())
                Exit Sub
            End If

            If bInRisks Then
                lvwRisks_DoubleClick(lvwRisks, New EventArgs())
                Exit Sub
            End If

        Catch
        End Try




        ' Error Section.

        ' Log Error.
        gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed while viewing a risk", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdView_Click", excep:=New Exception(Information.Err().Description))

    End Sub



    ' ***************************************************************** '
    ' Name: Sizing Image Events
    '
    ' Description: All events raised by the sizing image controls
    ' ***************************************************************** '
    Private Sub imgVSize_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgVSize.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        If Button And MouseButtonConstants.LeftButton Then
            m_VSize = CInt(VB6.PixelsToTwipsY(imgVSize.Top) + y)
            m_lReturn = CType(ResizeInterface(), gPMConstants.PMEReturnCode)
        End If
    End Sub

    Private Sub imgHSize_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgHSize.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        If Button And MouseButtonConstants.LeftButton Then
            m_HSize = CInt(VB6.PixelsToTwipsX(imgHSize.Left) + x)
            m_lReturn = CType(ResizeInterface(), gPMConstants.PMEReturnCode)
        End If
    End Sub


    ' ***************************************************************** '
    ' Name: Risk Listview Events
    '
    ' Description: All events raised by the risk listview control
    ' ***************************************************************** '
    Private Sub lvwRisks_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwRisks.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwRisks.Columns(eventArgs.Column)





        Try

            ''PN 49922 - START
            'If iRisklastColumn = ColumnHeader.Index + 1 Then
            '    iOrder = Math.Abs(iRisklastOrder - 1)
            'Else
            '    iOrder = SortOrder.Ascending
            'End If

            'ListViewHelper.SetSortedProperty(lvwRisks, True)

            'Select Case (ColumnHeader.Index + 1)
            '    '    Case 1, 2, 3, 12
            '    '        ' Sort by date
            '    '        ListViewSortByDate lvwRisks, ColumnHeader.Index - 1, iOrder
            '    Case 3, 7
            '        ' Sort by currency
            '        'TODO
            '        ListViewFunc.ListViewSortByValue(lvwRisks, ColumnHeader.Index + 1 - 1, iOrder)
            '    Case Else
            '        ' Default sort
            '        ListViewHelper.SetSortOrderProperty(lvwRisks, iOrder)
            '        ListViewHelper.SetSortKeyProperty(lvwRisks, ColumnHeader.Index + 1 - 1)
            'End Select

            'iRisklastColumn = ColumnHeader.Index + 1
            'iRisklastOrder = iOrder
            'PN 49922 - START

            ListViewFunc.SortListView(lvwRisks, eventArgs)

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwRisks_ColumnClick", excep:=excep)

        End Try

    End Sub


    Private Sub lvwRisks_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRisks.Enter


    End Sub

    '---------------------------------------------------------------
    'Name   :   lvwRisks_ItemClick
    'Comments:
    'Parameters:
    'History:
    '           ??/??/????  ???  Created
    '           24/02/2003  APS  Amended to set the m_lPolicyTypeID if not selected
    '---------------------------------------------------------------
    Private Sub lvwRisks_ItemClick(ByVal Item As ListViewItem)

        Try

            If kbInDebug Then
                MessageBox.Show("Item key: " & Item.Name & "  Tag: " & Convert.ToString(Item.Tag), "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            'Set the policy type in order to check if an underwriting
            If m_lPolicyTypeID = 0 Then
                m_lPolicyTypeID = CInt(m_vPolicyVersionSearchData(ACPVPolicyTypeID, 0))
            End If

            If Item Is Nothing Then
                RaiseEvent lvwRisksClick(Me, New lvwRisksClickEventArgs(False, 0, 0, 0))

                EnablePrintAndViewButtons(False)
            Else

                RaiseEvent lvwRisksClick(Me, New lvwRisksClickEventArgs(True, CInt(m_vRiskSearchData(ACIRiskId, Convert.ToString(Item.Tag))), CInt(m_vRiskSearchData(ACIRiskGisScreen, Convert.ToString(Item.Tag))), CInt(m_vRiskSearchData(ACIRiskTypeId, Convert.ToString(Item.Tag)))))

                m_lRiskID = CInt(m_vRiskSearchData(ACIRiskId, Convert.ToString(Item.Tag)))
                m_lInsFileCnt = CInt(m_vRiskSearchData(ACRInsFileCnt, Convert.ToString(Item.Tag)))
                m_lRiskTypeId = CInt(m_vRiskSearchData(ACIRiskTypeId, Convert.ToString(Item.Tag)))
                m_lRiskGisScreenId = CInt(m_vRiskSearchData(ACIRiskGisScreen, Convert.ToString(Item.Tag)))

                m_sRiskDescription = CStr(m_vRiskSearchData(ACIRiskDescription, Convert.ToString(Item.Tag)))
                m_sRiskTypeDescription = CStr(m_vRiskSearchData(ACIRiskTypeDescription, Convert.ToString(Item.Tag)))
                m_vRiskInceptionDate = CDate(m_vRiskSearchData(ACIRiskInceptionDate, Convert.ToString(Item.Tag)))
                m_vRiskExpiryDate = CDate(m_vRiskSearchData(ACIRiskExpiryDate, Convert.ToString(Item.Tag)))

                m_vRiskTotalSI = CStr(m_vRiskSearchData(ACIRiskTotalSumInsured, Convert.ToString(Item.Tag)))
                m_vRiskTotalPremium = CStr(m_vRiskSearchData(ACIRiskTotalAnnualPremium, Convert.ToString(Item.Tag)))
                m_lInsuranceFolderCnt = CInt(m_vRiskSearchData(ACIInsuranceFolderCnt, Convert.ToString(Item.Tag)))


                m_lIsReInsuranceAtRiskLevel = m_oRiskyBusiness.IsRIAtRiskLevel(m_lRiskTypeId)
                If (Convert.ToString(Item.Tag) <> "") Then
                    EnablePrintAndViewButtons(True)
                    bInRisks = True
                    bInVersions = False
                Else
                    EnablePrintAndViewButtons(False)
                End If


            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to select the Risk", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectListRisk", excep:=excep)

        End Try

    End Sub



    ' ***************************************************************** '
    ' Name: SelectListRisk
    '
    ' Description: Called when we wish to select
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (SelectListRisk) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function SelectListRisk() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Set the interface status.
    'm_lStatus = gPMConstants.PMEReturnCode.PMOK
    '
    ' Process the next set of actions depending
    ' upon the interface task etc.
    'm_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)
    '
    ' Check the return value.
    'If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
    ' Everything OK, so we can hide the interface.
    '        unloadInterface
    'result = gPMConstants.PMEReturnCode.PMTrue
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error.
    'iPMFunc.LogExcepMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Select the ListRisk", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectListRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    'End Try
    'End Function

    Private Sub lvwRisks_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRisks.DoubleClick
        ' Double click event for the search details.

        Try

            ' Check if there are any items available.
            If lvwRisks.Items.Count = 0 Then
                Exit Sub
            End If

            If m_lRiskID = 0 Then
                lvwRisks_ItemClick(lvwRisks.SelectedItems(0))
            End If
            'PM035142 - Added m_sShortName
            RaiseEvent lvwRisksDblClick(Me, New lvwRisksDblClickEventArgs(m_lInsFileCnt, m_lRiskID, m_sRiskDescription, m_sRiskTypeDescription, m_vRiskInceptionDate, m_vRiskExpiryDate, m_vRiskTotalSI, m_vRiskTotalPremium, m_lRiskGisScreenId, m_lRiskTypeId, m_lIsReInsuranceAtRiskLevel, m_lInsuranceFolderCnt, m_sShortName))

            'TN20010117 End

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwRisks_DblClick", excep:=excep)

            Exit Sub

        End Try
    End Sub


    Private Sub lvwRisks_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwRisks.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        Dim oListItem As ListViewItem = lvwRisks.GetItemAt(x, y)

        lvwRisks_ItemClick(oListItem)

    End Sub

    ' ***************************************************************** '
    ' Name: Policy Version Listview Events
    '
    ' Description: All events raised by the version listview control
    ' ***************************************************************** '
    Private Sub lvwVersions_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwVersions.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwVersions.Columns(eventArgs.Column)





        Try

            ' Alix - 05/01/2003 - PN7998
            ' I completly re-did this section as the original code was.... bad.
            ' This code was copied from uctRiskScreen and uses the ListView global fonctions

            'If lastColumn = ColumnHeader.Index + 1 Then
            '    iOrder = Math.Abs(lastOrder - 1)
            'Else
            '    iOrder = SortOrder.Ascending
            'End If

            'ListViewHelper.SetSortedProperty(lvwVersions, True)

            'Select Case (ColumnHeader.Index + 1)
            '    Case 1, 2, 3, 13
            '        ' Sort by date
            '        'TODO
            '        ListViewFunc.ListViewSortByDate(lvwVersions, ColumnHeader.Index + 1 - 1, iOrder)
            '    Case 7
            '        ' Sort by currency
            '        'TODO
            '        ListViewFunc.ListViewSortByValue(lvwVersions, ColumnHeader.Index + 1 - 1, iOrder)
            '    Case Else
            '        ' Default sort
            '        ListViewHelper.SetSortOrderProperty(lvwVersions, iOrder)
            '        ListViewHelper.SetSortKeyProperty(lvwVersions, ColumnHeader.Index + 1 - 1)
            'End Select

            'lastColumn = ColumnHeader.Index + 1
            'lastOrder = iOrder
            ListViewFunc.SortListView(lvwVersions, eventArgs)



        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwVersions_ColumnClick", excep:=excep)

        End Try

    End Sub

    Private Sub lvwVersions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwVersions.SelectedIndexChanged

    End Sub

    Private Sub lvwVersions_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwVersions.DoubleClick
        ' Double click event for the search details.


        Try

            ' Check if there are any items available.
            If lvwVersions.Items.Count = 0 Then
                Exit Sub
            End If

            lvwVersions_ItemClick(DirectCast(eventSender, System.Windows.Forms.ListView).SelectedItems.Item(0))

            RaiseEvent lvwVersionsDblClick(Me, New lvwVersionsDblClickEventArgs(m_lInsHolderCnt, m_lInsuranceFolderCnt, m_lInsFileCnt, m_sShortName, m_sInsReference, m_lPolicyTypeID))

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwVersions_DblClick", excep:=excep)

        End Try

    End Sub


    Private Sub lvwVersions_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwVersions.Enter



    End Sub

    Private Sub lvwVersions_ItemClick(ByVal Item As ListViewItem)


        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sInsFileCnt As String = ""
        Dim lLoopy, lFound As Integer


        If kbInDebug Then
            MessageBox.Show("Item key: " & Item.Name & "  Tag: " & Convert.ToString(Item.Tag), "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        If lvwVersions.Items.Count > 0 Then

            sInsFileCnt = Convert.ToString(Item.Tag)

            ' loop around and get the other details...
            For lLoopy = m_vPolicyVersionSearchData.GetLowerBound(1) To m_vPolicyVersionSearchData.GetUpperBound(1)
                If CStr(m_vPolicyVersionSearchData(ACPVInsuranceFileCnt, lLoopy)) = sInsFileCnt Then
                    lFound = lLoopy
                    Exit For
                End If
            Next lLoopy

            m_lInsHolderCnt = CInt(m_vPolicyVersionSearchData(ACPVInsuranceHolderCnt, lLoopy))
            m_lInsuranceFolderCnt = CInt(m_vPolicyVersionSearchData(ACPVInsuranceFolderCnt, lLoopy))
            m_lInsFileCnt = CInt(sInsFileCnt)
            m_sShortName = CStr(m_vPolicyVersionSearchData(ACPVShortName, lLoopy)).Trim()
            m_sInsReference = CStr(m_vPolicyVersionSearchData(ACPVInsuranceRef, lLoopy)).Trim()
            m_lPolicyTypeID = CInt(m_vPolicyVersionSearchData(ACPVPolicyTypeID, lLoopy))

        End If

        EnablePrintAndViewButtons(Convert.ToString(Item.Tag) <> "")

        If Convert.ToString(Item.Tag) <> "" Then
            ' get the policy number from the key
            lReturn = CType(BuildRiskListview(Convert.ToString(Item.Tag)), gPMConstants.PMEReturnCode)
        End If

    End Sub

    Private Sub tvwPolicies_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvwPolicies.DoubleClick
        m_lReturn = OpenClientManager()
    End Sub

    Private Function OpenClientManager() As Integer
        Dim oClientManagerWrapper As Object
        Dim sClassName As String = ""

        If m_lInsFileCnt <> 0 Then
            sClassName = "iPMBClientManagerWrapper.Interface_Renamed"
            'UPGRADE_NOTE: (7015) The following call to GetInstance could not be automatically upgraded because of invalid parameters. More Information: http://www.vbtonet.com/ewis/ewi7015.aspx
            m_lReturn = g_oObjectManager.GetInstance(oObject:=oClientManagerWrapper, sClassName:=sClassName, vInstanceManager:=gPMConstants.PMGetLocalInterface)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of " & sClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="OpenClientManger", excep:=New Exception(Information.Err().Description))
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oClientManagerWrapper.InsuranceFileCnt = m_lInsFileCnt


            m_lReturn = oClientManagerWrapper.Start

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Srart method of " & sClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="OpenClientManger", excep:=New Exception(Information.Err().Description))
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

    End Function

    Private Sub tvwPolicies_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvwPolicies.Enter

        ' disable the view button again
        EnablePrintAndViewButtons(False)
        bInRisks = False
        bInVersions = False

    End Sub

    Private Sub tvwPolicies_AfterSelect(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwPolicies.AfterSelect
        Dim Node As TreeNode = eventArgs.Node

        Dim lNodeNum As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bCopyPolicyToQuoteEnabled As Boolean = False
        Dim sOptionValue As String = ""

        cmdCopyPolicy.Visible = False
        If kbInDebug Then
            MessageBox.Show("Node key: " & Node.Name & "  Tag: " & Convert.ToString(Node.Tag), "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        ' enable/disable things

        If CStr(Convert.ToString(Node.Tag)).IndexOf(ksPolicy) + 1 Then

            ' get the policy number from the tag
            lNodeNum = CInt(CStr(Convert.ToString(Node.Tag)).Substring(Convert.ToString(Node.Tag).Length - (Strings.Len(Convert.ToString(Node.Tag)) - (IIf(Convert.ToString(Node.Tag) = "" And ksDelimiter = "", 0, (CStr(Convert.ToString(Node.Tag)).LastIndexOf(ksDelimiter) + 1))))))
            '<Pankaj>
            m_lInsFileCnt = aTreeNodes(lNodeNum).lInsFileCnt
            m_lInsuranceFolderCnt = aTreeNodes(lNodeNum).lInsFolderCnt
            m_lProductId = aTreeNodes(lNodeNum).nProductId
            '</Pankaj>

            lReturn = bPMFunc.GetSystemOption(v_sUsername:="", v_sPassword:="", v_iUserID:=g_iUserID, v_iMainSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID,
                                            v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=gPMConstants.PMELogLevel.PMLogError, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=GeneralConst.kSystemOptionCopyPolicyToQuoteEnabled,
                                            r_sOptionValue:=sOptionValue)

            bCopyPolicyToQuoteEnabled = ToSafeBoolean(sOptionValue)
            If bCopyPolicyToQuoteEnabled Then
                cmdCopyPolicy.Visible = True
                cmdCopyPolicy.Enabled = True
            End If


            lReturn = CType(BuildPolicyVersionsListview(aTreeNodes(lNodeNum).lInsFileCnt, aTreeNodes(lNodeNum).lInsFolderCnt), gPMConstants.PMEReturnCode)

            If lvwVersions.Items.Count > 0 Then
                lvwVersions.Enabled = True
                lvwVersions.BackColor = SystemColors.Window
                RemoveHandler lvwVersions.ItemSelectionChanged, AddressOf lvwVersions_ItemSelectionChanged
                lvwVersions.Items(0).Selected = True
                AddHandler lvwVersions.ItemSelectionChanged, AddressOf lvwVersions_ItemSelectionChanged
                lvwVersions.Items(0).Focused = True
                'lvwVersions_ItemClick(lvwVersions.Items(0))
                includecancelledquote.Enabled = True
            Else
                lvwVersions.Enabled = False
                lvwVersions.BackColor = SystemColors.Control
                includecancelledquote.Enabled = False
            End If

            If lvwRisks.Items.Count > 0 Then
                lvwRisks.Enabled = True
                lvwRisks.BackColor = SystemColors.Window
                RemoveHandler lvwRisks.ItemSelectionChanged, AddressOf lvwRisks_ItemSelectionChanged
                lvwRisks.Items(0).Selected = True
                AddHandler lvwRisks.ItemSelectionChanged, AddressOf lvwRisks_ItemSelectionChanged
                lvwRisks.Items(0).Focused = True
            Else
                lvwRisks.Enabled = False
                lvwRisks.BackColor = SystemColors.Control
            End If

        Else
            ' everything else
            lvwVersions.Items.Clear()
            lvwVersions.Enabled = False
            lvwVersions.BackColor = SystemColors.Control
            lvwRisks.Items.Clear()
            lvwRisks.Enabled = False
            lvwRisks.BackColor = SystemColors.Control
            includecancelledquote.Enabled = False

        End If

    End Sub
    'sj 02/10/2002 - start
    Private Sub Toolbar1_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles TB_Event.Click, TB_RiskDetails.Click, sep2.Click, TB_InformationChecklist.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        Dim lIndex As Integer

        If lvwVersions.Items.Count = 0 Then
            Exit Sub
        End If

        If Not lvwVersions.FocusedItem.Selected Then
            Exit Sub
        End If

        Dim lInsuranceFileCnt As Integer = CInt(Conversion.Val(Convert.ToString(lvwVersions.Items.Item(lvwVersions.FocusedItem.Index).Tag)))

        m_lReturn = CType(GetPolicyVersionArrayIndex(v_lInsuranceFileCnt:=lInsuranceFileCnt, r_lIndex:=lIndex), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetPolicyVersionArrayIndex Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="??????")
            Exit Sub
        End If

        Dim lInsuranceFolderCnt As Integer = CInt(Conversion.Val(CStr(m_vPolicyVersionSearchData(ACPVInsuranceFolderCnt, lIndex))))
        Dim sInsuranceRef As String = CStr(m_vPolicyVersionSearchData(ACPVInsuranceRef, lIndex)).Trim()


        Select Case Button.Name
            Case "TB_Event"

                m_lReturn = CType(iPMBListEvents.ShowEvents(v_lPartyCnt:=m_lInsHolderCnt, v_sTransactionType:=m_sTransactionType, v_lInsuranceFileCnt:=lInsuranceFileCnt, v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_sInsuranceRef:=sInsuranceRef), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ShowEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Toolbar1_ButtonClick")
                    Exit Sub
                End If

        End Select

    End Sub
    ' ***************************************************************** '
    '
    ' Name: GetPolicyVersionArrayIndex
    '
    ' Description:
    '
    ' History: 04/10/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetPolicyVersionArrayIndex(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lIndex As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            r_lIndex = -1

            For lCnt As Integer = 0 To m_vPolicyVersionSearchData.GetUpperBound(1)
                If Conversion.Val(CStr(m_vPolicyVersionSearchData(ACPVInsuranceFileCnt, lCnt))) = v_lInsuranceFileCnt Then
                    r_lIndex = lCnt
                    Return result
                End If
            Next lCnt

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyVersionArrayIndex Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyVersionArrayIndex", excep:=excep)

            Return result




            Return result
        End Try
    End Function

    'sj 02/10/2002 - end
    ' ***************************************************************** '
    ' Name: UserControl Events
    '
    ' Description: All events raised by the usercontrol
    ' ***************************************************************** '
    Private Sub UserControl_Initialize()



        ' This again is a nicety, don't bother with errors

        Try

            ' Set default sizing values
            m_VSize = CInt(VB6.PixelsToTwipsY(ClientRectangle.Height) \ 3)
            m_HSize = CInt(ClientRectangle.Width \ 3)

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub

    Private Sub uctPMUPolicyExplorer_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.LostFocus

        ' disable the view button again
        EnablePrintAndViewButtons(False)
        bInRisks = False
        bInVersions = False
        'cmdView.Enabled = False
        With lvwVersions.SelectedItems
            If .Count > 0 Then
                .Clear()
            End If
        End With
        With lvwRisks.SelectedItems
            If .Count > 0 Then
                .Clear()
            End If
        End With

    End Sub


    Private Sub uctPMUPolicyExplorer_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize


        Try

            ' Pass through to resize function
            m_lReturn = CType(ResizeInterface(), gPMConstants.PMEReturnCode)

        Catch



            ' We error, we're not particularly fussed here as the only realistic
            ' error is that the user has shrunk the form and the usercontrol has
            ' shrunk too small to handle.
        End Try


    End Sub

    Private Sub EnablePrintAndViewButtons(ByRef bEnable As Boolean)
        cmdView.Enabled = bEnable
        cmdPrint.Enabled = bEnable

    End Sub

    Public Sub PopulatePolicyDropdown()
        Dim lstLivePolicies As ArrayList = New ArrayList()
        cboLivePolicies.Items.Clear()
        cboLivePolicies.Items.Add("<Show All>")
        If Information.IsArray(m_vSearchData) Then
            For lLoop As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                If (IsNothing(m_vSearchData(ACIStatus, lLoop)) And (Not (CStr(m_vSearchData(ACIInsFileType, lLoop)).Trim().ToUpper() = "QUOTE"))) Then
                    lstLivePolicies.Add(CStr(m_vSearchData(3, lLoop)))
                End If
            Next
            lstLivePolicies.Sort()
            cboLivePolicies.Items.AddRange(lstLivePolicies.ToArray())
        End If
        lstLivePolicies.Clear()
        lstLivePolicies = Nothing
    End Sub

    Private Sub lvwVersions_ItemSelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvwVersions.ItemSelectionChanged

        If e.IsSelected Then
            EnablePrintAndViewButtons(True)
            bInRisks = False
            bInVersions = True
            lvwVersions_ItemClick(e.Item)
        Else
            EnablePrintAndViewButtons(False)
            'cmdView.Enabled = False
        End If




    End Sub

    Private Sub lvwRisks_ItemSelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvwRisks.ItemSelectionChanged
        If e.IsSelected Then
            EnablePrintAndViewButtons(True)
            bInRisks = True
            bInVersions = False
            lvwRisks_ItemClick(e.Item)
        Else
            EnablePrintAndViewButtons(False)
        End If

    End Sub

    Private Sub includecancelledquote_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles includecancelledquote.CheckedChanged
        Dim eventagrs As New TreeViewEventArgs(tvwPolicies.SelectedNode)
        tvwPolicies_AfterSelect(sender, eventagrs)
    End Sub

    ''' <summary>
    ''' cmdCopyPolicy_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdCopyPolicy_Click(sender As Object, e As EventArgs) Handles cmdCopyPolicy.Click
        Dim nResult As Integer = 0

        cmdCopyPolicy.Enabled = False
        nResult = ProcessAddRisk()

        If nResult = PMEReturnCode.PMTrue Then
            MessageBox.Show("Copy and Create Quote '" + m_sCopiedQuoteRef + "' has been completed successfully.", "Success !!", MessageBoxButtons.OK)
        Else
            tvwPolicies.SelectedNode = Nothing
        End If
    End Sub

    ''' <summary>
    ''' ProcessAddRisk
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessAddRisk() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        'get risk type to add from user input
        nResult = CType(GetRiskType(), gPMConstants.PMEReturnCode)
        If nResult <> PMEReturnCode.PMTrue Then
            Return nResult
        End If

        nResult = GetLatestPolicyVersion(nInsuranceFileCnt:=m_lInsFileCnt, r_nPolicyVersion:=m_nVersion)
        If nResult <> PMEReturnCode.PMTrue Then
            Return nResult
        End If

        nResult = CheckDataModelCompatibility(nOldInsuranceFileCnt:=m_lInsFileCnt, nNewRiskTypeId:=m_nNewRiskTypeId)
        If nResult <> PMEReturnCode.PMTrue Then
            Return nResult
        End If

        nResult = CopyPolicyToQuote(nOldInsuranceFileCnt:=m_lInsFileCnt)
        If nResult <> PMEReturnCode.PMTrue Then
            Return nResult
        End If

        nResult = GetBusiness()

        If nResult <> PMEReturnCode.PMTrue Then
            Return nResult
        End If

        nResult = DataToInterface()

        If nResult <> PMEReturnCode.PMTrue Then
            Return nResult
        End If

        Return nResult

    End Function

    'WPR12
    ''' <summary>
    ''' GetRiskType
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRiskType() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        Try
            ' get instance of interface
            nResult = g_oObjectManager.GetInstance(m_oFindRiskType, sClassName:="iPMUFindRiskType.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            ' set process modes
            nResult = m_oFindRiskType.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            m_oFindRiskType.ProductTypeId = m_lProductId

            ' start interface
            m_oFindRiskType.Start()
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            ' get status
            m_lStatus = m_oFindRiskType.Status

            If m_oFindRiskType.Status = PMEReturnCode.PMOK Then
                m_nNewRiskTypeId = m_oFindRiskType.RiskTypeId
            Else
                nResult = PMEReturnCode.PMFalse
                Return nResult
            End If

            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetRiskType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskType")
            Return nResult
        End Try
    End Function
    'WPR12
    ''' <summary>
    ''' CheckDataModelCompatibility
    ''' </summary>
    ''' <param name="nOldInsuranceFileCnt"></param>
    ''' <param name="nNewRiskTypeId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckDataModelCompatibility(ByVal nOldInsuranceFileCnt As Integer, ByVal nNewRiskTypeId As Integer) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim nReturnValue As Integer = 0
        Try
            nResult = m_oBusiness.CheckDataModelCompatibility(nOldInsuranceFileCnt:=nOldInsuranceFileCnt, nNewRiskTypeId:=nNewRiskTypeId, r_nReturnValue:=nReturnValue)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
            If nReturnValue = 0 Then
                MessageBox.Show(String.Format("Copy Failed - The Risk Types are incompatible"))
                nResult = PMEReturnCode.PMFalse
                Return nResult
            Else
                Return nResult
            End If
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CheckDataModelCompatibility Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDataModelCompatibility")
            Return nResult
        End Try

    End Function

    'WPR12
    ''' <summary>
    ''' GetLatestPolicyVersion
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="r_nPolicyVersion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLatestPolicyVersion(ByVal nInsuranceFileCnt As Integer, ByRef r_nPolicyVersion As Integer) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try
            nResult = m_oBusiness.GetLatestPolicyVersion(v_lInsuranceFileCnt:=nInsuranceFileCnt,
                                     r_lPolicyVersion:=r_nPolicyVersion)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetLatestPolicyVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLatestPolicyVersion")
            Return nResult
        End Try
    End Function

    'WPR12
    ''' <summary>
    ''' CopyPolicyToQuote
    ''' </summary>
    ''' <param name="nOldInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyPolicyToQuote(ByVal nOldInsuranceFileCnt As Integer) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oObject As Object = Nothing
        Dim sPolicyRef As String = ""
        Dim nInsuranceFolderCnt As Integer
        Dim nNewInsuranceFolderCnt As Integer
        Dim nNewInsuranceFileCnt As Integer
        Dim sFailureReason As String = ""

        Try
            nResult = g_oObjectManager.GetInstance(oObject, "bSIRInsuranceFile.Services", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            oObject.InsuranceFileCnt = nOldInsuranceFileCnt

            nResult = oObject.GetDetails()
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nInsuranceFolderCnt = oObject.InsuranceFolderCnt
            g_iSourceID = oObject.SourceID
            m_lProductId = oObject.ProductId
            m_vLeadAgentCnt = oObject.LeadAgentCnt

            nResult = GetPolicyNumber(r_sPolicyRef:=sPolicyRef)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = m_oBusiness.CopyQuote(nOldInsuranceFileCnt:=nOldInsuranceFileCnt, nOldInsuranceFolderCnt:=nInsuranceFolderCnt, sPolicyRef:=sPolicyRef, r_nNewInsuranceFileCnt:=nNewInsuranceFileCnt, r_nNewInsuranceFolderCnt:=nNewInsuranceFolderCnt)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            m_nNewInsFileCnt = nNewInsuranceFileCnt
            m_lInsuranceFolderCnt = nNewInsuranceFolderCnt
            m_sCopiedQuoteRef = sPolicyRef

            nResult = CopyMTARisks(nInsuranceFileCnt:=nNewInsuranceFileCnt, nNewRiskTypeId:=m_nNewRiskTypeId)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = m_oBusiness.UpdateEventLogUser(v_lInsuranceFileCnt:=nNewInsuranceFileCnt)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CopyPolicyToQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyToQuote")
            Return nResult
        End Try
    End Function
    'WPR12
    ''' <summary>
    ''' GetPolicyNumber
    ''' </summary>
    ''' <param name="r_sPolicyRef"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPolicyNumber(ByRef r_sPolicyRef As String) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        nResult = g_oObjectManager.GetInstance(m_oPolicyNumber, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        If nResult <> PMEReturnCode.PMTrue Then
            Return nResult
        End If
        nResult = m_oPolicyNumber.GeneratePolicyNumber(1, g_iSourceID, m_lProductId, ToSafeInteger(m_vLeadAgentCnt), r_sPolicyRef)
        If nResult <> PMEReturnCode.PMTrue Then
            Return nResult
        End If
        Return nResult
    End Function
    'WPR12
    ''' <summary>
    ''' CopyMTARisks
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CopyMTARisks(ByVal nInsuranceFileCnt As Integer, ByVal nNewRiskTypeId As Integer) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim createLink As Integer = 0

        nResult = MainModule.g_oObjectManager.GetInstance(m_oBusiness1, "bSIRListRisks.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        If nResult <> PMEReturnCode.PMTrue Then
            Return nResult
        End If

        nResult = m_oBusiness1.CopyRisksMTA(nInsuranceFileCnt, createLink, False, False)
        If nResult <> PMEReturnCode.PMTrue Then
            Return nResult
        End If
        nResult = m_oBusiness.UpdateRiskStatus(nInsuranceFileCnt:=nInsuranceFileCnt, nRiskTypeId:=nNewRiskTypeId)
        If nResult <> PMEReturnCode.PMTrue Then
            Return nResult
        End If
        Return nResult
    End Function
    'WPR12
    ''' <summary>
    ''' QuoteRisk
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nInsuranceFolderCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function QuoteRisk(ByVal nInsuranceFileCnt As Integer, ByVal nInsuranceFolderCnt As Integer) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oGisPolicyLinkArray, oRiskArray(,) As Object
        Dim nQuoteType As Integer

        nResult = MainModule.g_oObjectManager.GetInstance(m_oRiskData, "bSIRRiskData.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        If nResult <> PMEReturnCode.PMTrue Then
            Return nResult
        End If

        nResult = MainModule.g_oObjectManager.GetInstance(m_oPerilAllocation, "bSirPerilAllocation.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        If nResult <> PMEReturnCode.PMTrue Then
            Return nResult
        End If

        'get all risks associated with InsuranceFileCnt
        nResult = m_oRiskData.GetRisk(v_lInsuranceFileCnt:=nInsuranceFileCnt, r_vResultArray:=oRiskArray)
        If nResult <> PMEReturnCode.PMTrue Then
            Return nResult
        End If

        For nCount As Integer = 0 To oRiskArray.GetUpperBound(1)
            nResult = m_oBusiness.UpdateIsRiskSelected(nRiskId:=oRiskArray(ACRiskPosCnt, nCount))
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
        Next
        'do we have any risks
        If Not Information.IsArray(oRiskArray) Then
            nResult = PMEReturnCode.PMFalse
            Return nResult
        End If

        Return nResult
    End Function

End Class
