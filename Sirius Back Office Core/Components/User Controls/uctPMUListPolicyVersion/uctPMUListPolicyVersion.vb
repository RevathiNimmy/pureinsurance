Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Data
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctPMUListPolicy_NET.uctPMUListPolicy")>
Partial Public Class uctPMUListPolicy
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
    ' Date: 07/10/1998
    '
    ' Description: List Policy User Control
    '
    ' Edit History: TF071098 - Created from iFindInsurance
    ' RKS 28/01/2005 - Restricting Cancelled Policy to be displayed in
    '                  'Underwriting Cancel Policy' Roadmap
    ' CJB 23/03/2005 - PN19733 Changed listview as follows: Remove Policy Number, Risk Type
    '                  and Insured columns, Move the Policy Status to the first column, Denote
    '                  the current active policy version (for Broking only) by having a different
    '                  icon in the listview. Code changed is: GetBusiness now calls
    '                  GetCurrentPolicyVersion for Broking. DataToInterface, SetInterfaceDefaults,
    '                  lvwSearchDetails_ColumnClick & DisplayCaptionschanges for changed listview
    '                  columns.
    ' CJB 30/03/2005 - PN19811 Changed lvwSearchDetails_DblClick to use new listview location for
    '                  policy status.
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "uctPMUListPolicy"

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
    Dim m_lSelected As gPMConstants.PMEReturnCode
    Dim m_lLockedItem As Integer

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

    Event lvwSearchDetailsMouseDown(ByVal Sender As Object, ByVal e As lvwSearchDetailsMouseDownEventArgs)

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
    Private m_lInsFileCnt As Integer
    Private m_sInsReference As String = ""
    Private m_lInsHolderCnt As Integer
    Private m_sShortName As String = "" 'JW190498
    Private m_lInsuranceFolderCnt As Integer 'TF100398
    Private m_lPolicyTypeID As Integer
    Private m_lInsuranceFileTypeID As Integer
    Private m_dtStartDate As Date
    Private m_lCurrentPolicyVersionInsFileCnt As Integer

    'eck090500
    Private m_vSourceArray As Object
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
    Private m_lItemsFound As Integer
    Private m_lLifeItemsFound As Integer
    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    Private m_lCancelledPolicyVersionInsFileCnt As Long
    Private m_dtCancelledPolicyVersionDate As Date

    ' Stores the search data from the business object.
    Public m_vSearchData(,) As Object

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
    <Browsable(True)>
    Public Property InsuranceFolderCnt() As Integer
        Get

            Return m_lInsuranceFolderCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFolderCnt = Value

        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property InsuranceFileTypeID() As Integer
        Get
            Return m_lInsuranceFileTypeID
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property CoverStartDate() As Date
        Get
            Return m_dtStartDate
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property Selected() As Integer
        Get
            Return m_lSelected
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property SelectedItem() As Integer
        Get
            Return Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)
        End Get
    End Property

    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)

    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    Public Sub lvwSearchDetailsSetFocus()
        lvwSearchDetails.Focus()
    End Sub
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
            m_lReturn = GetBusiness()

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the Policies", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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
        Dim lNonTempPolicies As gPMConstants.PMEReturnCode
        Dim sUnderwritingAgency As String = ""

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

            'MKW250703 PN4271 START 1.8.5 to 1.8.6 Catchup
            m_lReturn = iPMFunc.getUnderwritingOrAgency(sUnderwritingAgency)
            If sUnderwritingAgency = "A" Then
                'ISS1497 JAS 10/03/03
                If m_sTransactionType = "G_MTA" Then
                    lNonTempPolicies = gPMConstants.PMEReturnCode.PMTrue
                Else
                    lNonTempPolicies = gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oBusiness.GetAllPolicyVersion(r_vResultArray:=m_vSearchData, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsFileCnt, v_lNonTempPolicies:=lNonTempPolicies)
            Else

                m_lReturn = m_oBusiness.GetAllPolicyVersion(r_vResultArray:=m_vSearchData, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsFileCnt,
                                                     v_lfilterBackdatedVersions:=0)
            End If
            'MKW250703 PN4271 END 1.8.5 to 1.8.6 Catchup

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                stbStatus.Text = "Failed to get policy versions"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_sTransactionType = "MTR" Then
                m_lReturn = m_oBusiness.GetCancellationDate(v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, m_dtCancellationDate:=m_dtCancelledPolicyVersionDate, r_lInsFileCnt:=m_lCancelledPolicyVersionInsFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    stbStatus.Text = "Failed to get cancelled policy versions"
                    Return gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If

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
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
            End Select

            ' Display the number of item found message.
            DisplayStatusFound()

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'ArtinsoftUtils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Display all versions of policy
    '
    ' History    : Kevin Renshaw (CMG) 11/3/2003 PN2921 Amend record count
    '            : Jitendra 21/12/2004  PN 17437 Added case 9 for editing Reinstated
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim bInclude, bMTAReasonFound As Boolean
        Const ACFindImage As String = "FindImage"
        Const ACCurrentPolicyVersionImage As String = "CurrentPolicyVersion"
        Dim sListViewImage, sUnderwritingAgency, sPolicyStatus As String
        Dim r_dtResult As DataTable
        Dim nRenewalStatus As Long
        Dim nDoNotDeleteRenewalQuoteOnMta As Long



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvwSearchDetails.Items.Clear()

            m_lItemsFound = 0
            m_lLockedItem = 0

            'sj 20/09/2002 - start
            bMTAReasonFound = False
            'sj 20/09/2002 - end

            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If

            m_lReturn = iPMFunc.getUnderwritingOrAgency(sUnderwritingAgency)

            m_lReturn = GetRenewalDetails(v_lInsuranceFileCnt:=m_lInsFileCnt, r_dtResult:=r_dtResult)
            If r_dtResult IsNot Nothing AndAlso r_dtResult.Rows.Count > 0 Then
                nRenewalStatus = CLng(r_dtResult.Rows(0).Item(0))
                nDoNotDeleteRenewalQuoteOnMta = CLng(r_dtResult.Rows(0).Item(2))
            End If

            m_lItemsFound = m_vSearchData.GetUpperBound(1) + 1

            For lCount As Integer = 0 To m_vSearchData.GetUpperBound(1)

                'If we're in Client Manager show everything.
                'In MTA roadmap do not display cancelled policies
                'If we're in EDIT mode, show only completed policies

                bInclude = False

                Select Case m_sTransactionType
                    Case "EDIT"
                        If (CStr(m_vSearchData(ACIInsuranceFileTypeID, lCount)) = "2") Or (CStr(m_vSearchData(ACIInsuranceFileTypeID, lCount)) = "5") Or (CStr(m_vSearchData(ACIInsuranceFileTypeID, lCount)) = "9") Or (CStr(m_vSearchData(ACIInsuranceFileTypeID, lCount)) = "6") Then
                            If CStr(m_vSearchData(ACIInsuranceFileStatus, lCount)) <> "Replaced" Then
                                bInclude = True
                            Else
                                m_lItemsFound -= 1
                            End If
                        End If
                    Case "MTA"
                        ' Gaurav Arora
                        'Do not display canceled reinstatement quotes during MTA
                        'Do not display cancelled MTA quotes during MTA
                        If Not ((CStr(m_vSearchData(ACIInsuranceFileTypeID, lCount)) = "10" Or CStr(m_vSearchData(ACIInsuranceFileTypeID, lCount)) = "4" Or CStr(m_vSearchData(ACIInsuranceFileTypeID, lCount)) = "7") And CStr(m_vSearchData(ACIInsuranceFileStatus, lCount)) = "Cancelled") Then
                            bInclude = True
                        Else
                            m_lItemsFound -= 1
                        End If

                        'Lock down the list
                        '                If (m_vSearchData(ACIInsuranceFileTypeID, lCount&) = 4) And m_vSearchData(ACIInsuranceFileStatus, lCount&) <> "Cancelled" Then
                        '                    m_lLockedItem = lvwSearchDetails.ListItems.Count + 1
                        '                End If
                    Case "MTR"
                        ' Only display policy if cancellation or reinstatement quote version and still cancelled
                        'priya PN 74021
                        If (m_vSearchData(ACIInsuranceFileTypeID, lCount) = "8" And m_vSearchData(ACIInsuranceFileStatus, lCount) = "Cancelled" And (m_lCancelledPolicyVersionInsFileCnt = m_vSearchData(ACIInsuranceFileCnt, lCount) Or m_lCancelledPolicyVersionInsFileCnt = 0)) Or
                               (m_vSearchData(ACIInsuranceFileTypeID, lCount) = "10" And CStr(m_vSearchData(ACIInsuranceFileStatus, lCount)) <> "Cancelled") Then
                            bInclude = True
                        Else
                            m_lItemsFound -= 1
                        End If
                    Case "MTC"
                        'PN 17666 - Don't display the Cancelled Policy in 'Underwriting
                        'Cancel Policy' Roadmap
                        If CStr(m_vSearchData(ACIInsuranceFileStatus, lCount)) <> "Cancelled" Then
                            bInclude = True
                        Else
                            m_lItemsFound -= 1
                        End If
                    Case Else
                        bInclude = True
                End Select

                If bInclude Then

                    ' For Broking: If this is the current policy version entry then change the listview image to
                    ' denote it PN19733
                    If sUnderwritingAgency = "A" Then
                        If CDbl(m_vSearchData(ACIInsuranceFileCnt, lCount)) = m_lCurrentPolicyVersionInsFileCnt Then
                            sListViewImage = ACCurrentPolicyVersionImage
                        Else
                            sListViewImage = ACFindImage
                        End If
                    Else
                        sListViewImage = ACFindImage
                    End If

                    'column 1 - policy status
                    If CStr(m_vSearchData(ACIInsuranceFileStatus, lCount)) = "" Then
                        If nDoNotDeleteRenewalQuoteOnMta = 1 And nRenewalStatus <> 0 And m_vSearchData(ACIInsuranceFileTypeID, lCount) = 3 Then
                            sPolicyStatus = "Renewal Quote"
                        Else
                            sPolicyStatus = "Live"
                        End If
                    Else
                        sPolicyStatus = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=CStr(m_vSearchData(ACIInsuranceFileStatus, lCount)))
                    End If

                    oListItem = lvwSearchDetails.Items.Add(sPolicyStatus, "FindImage")

                    'column 2 - policy type
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=CStr(m_vSearchData(ACIInsuranceFileType, lCount)))

                    'column 3 - cover start date
                    'MKR 28/06/2004 Issue 11192
                    'Date format changed from PMFormatDateLong to PMFormatDateShort for columns 2,3 and 4
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=CStr(m_vSearchData(ACICoverStartDate, lCount)))

                    'column 4 - cover end date
                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=CStr(m_vSearchData(ACICoverEndDate, lCount)))
                    'column 5 - renewal date
                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=CStr(m_vSearchData(ACIRenewalDate, lCount)))
                    'column 6 - premium
                    ListViewHelper.GetListViewSubItem(oListItem, 5).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_vSearchData(ACIPremium, lCount)))

                    'column 7 - MTA Reason

                    If Not (Convert.IsDBNull(m_vSearchData(ACIMTAReason, lCount)) Or IsNothing(m_vSearchData(ACIMTAReason, lCount))) And CStr(m_vSearchData(ACIMTAReason, lCount)).Trim() <> "" Then
                        ListViewHelper.GetListViewSubItem(oListItem, 6).Text = CStr(m_vSearchData(ACIMTAReason, lCount)).Trim()
                        bMTAReasonFound = True
                    End If

                    ' Gaurav
                    'column 6 - Event Descriptiotn
                    ListViewHelper.GetListViewSubItem(oListItem, 7).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=CStr(m_vSearchData(ACIEventDescription, lCount)))

                    oListItem.Tag = CStr(lCount)
                End If
            Next

            'sj 20/09/2002 - start
            If Not bMTAReasonFound Then
                lvwSearchDetails.Columns.Item(6).Width = CInt(0)
            End If
            'sj 20/09/2002 - end

            If lvwSearchDetails.Items.Count > 0 Then
                If m_lLockedItem > 0 Then
                    lvwSearchDetails.Items.Item(m_lLockedItem - 1).Selected = True
                Else
                    lvwSearchDetails.Items.Item(0).Selected = True
                End If
                m_lSelected = gPMConstants.PMEReturnCode.PMTrue
            End If

            ListViewFunc.ListViewSortByDate(lvwSearchDetails, 2, 2)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'ArtinsoftUtils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

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
            m_lPolicyTypeID = CInt(m_vSearchData(ACIPolicyTypeID, lSelectedItem))
            m_lInsuranceFolderCnt = CInt(m_vSearchData(ACIInsuranceFolderCnt, lSelectedItem))
            m_lInsFileCnt = CInt(m_vSearchData(ACIInsuranceFileCnt, lSelectedItem))
            m_lInsHolderCnt = CInt(m_vSearchData(ACIInsuranceHolderCnt, lSelectedItem))
            m_sInsReference = CStr(m_vSearchData(ACIInsuranceRef, lSelectedItem))
            m_sShortName = CStr(m_vSearchData(ACIShortName, lSelectedItem))
            m_lInsuranceFileTypeID = CInt(m_vSearchData(ACIInsuranceFileTypeID, lSelectedItem))
            m_dtStartDate = CDate(m_vSearchData(ACICoverStartDate, lSelectedItem))
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    Public Function CopyPolicy(ByVal v_lOldInsuranceFileCnt As Integer,
                               ByRef r_lNewInsuranceFileCnt As Integer,
                               ByVal v_lVersion As Integer,
                               ByVal v_bPermanentMTA As Boolean,
                               ByVal v_dtMTADate As Date,
                               Optional ByVal v_vMTAEndDate As Object = Nothing) As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue


            If Information.IsNothing(v_vMTAEndDate) Then
                ' If the current task is "MTR" copy the policy as reinstatement

                m_lReturn = m_oBusiness.CopyPolicy(v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt,
                                                   r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt,
                                                   v_lVersion:=v_lVersion,
                                                   v_bPermanentMTA:=v_bPermanentMTA,
                                                   v_dtMTADate:=v_dtMTADate,
                                                   v_bCancellation:=(m_sTransactionType = "MTC"),
                                                   v_bReinstatement:=(m_sTransactionType = "MTR"))
            Else

                m_lReturn = m_oBusiness.CopyPolicy(v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt,
                                                   r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt,
                                                   v_lVersion:=v_lVersion,
                                                   v_bPermanentMTA:=v_bPermanentMTA,
                                                   v_dtMTADate:=v_dtMTADate,
                                                   v_bCancellation:=(m_sTransactionType = "MTC"),
                                                   v_bReinstatement:=(m_sTransactionType = "MTR"),
                                                   v_vMTAEndDate:=v_vMTAEndDate)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyPolicyForEdit
    '
    ' Description:
    '
    ' History: 18/11/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function CopyPolicyForEdit(ByVal v_lOldInsuranceFileCnt As Integer, ByRef r_lNewInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.CopyPolicyForEdit(v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt, r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicyForEdit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyForEdit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'ArtinsoftUtils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

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
    Public Function GetVersionByDate(ByRef r_lInsuranceFileCnt As Integer, ByVal v_dtStartDate As Date, ByRef r_lPolicyVersion As Integer, ByRef r_lErrorCode As Integer, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_lSubErrorCode As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetVersionByDate(r_lInsuranceFileCnt:=r_lInsuranceFileCnt, v_dtStartDate:=v_dtStartDate, r_lPolicyVersion:=r_lPolicyVersion, r_lErrorCode:=r_lErrorCode, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, r_lSubErrorCode:=r_lSubErrorCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVersionByDate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'ArtinsoftUtils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckInRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckInRenewal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRenewalStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRenewalStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
    'Dim result As Integer = 0
    'Dim m_lSelected As gPMConstants.PMEReturnCode
    '
    'Dim lReturn, lPartyCnt As Integer
    'Static sTitle, sMessage As String
    '
    'Try 
    '
    '
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate lookups", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateLookups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            '    CenterForm Me

            txtClientCode.Text = m_sShortName

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Set the column widths for the search list.
            lvwSearchDetails.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1800))
            lvwSearchDetails.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(2500))
            lvwSearchDetails.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(1400))
            lvwSearchDetails.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(1400))
            lvwSearchDetails.Columns.Item(4).Width = CInt(VB6.TwipsToPixelsX(1400))
            lvwSearchDetails.Columns.Item(5).Width = CInt(VB6.TwipsToPixelsX(1200))
            lvwSearchDetails.Columns.Item(6).Width = CInt(VB6.TwipsToPixelsX(2500))
            lvwSearchDetails.Columns.Item(7).Width = CInt(VB6.TwipsToPixelsX(2000))

            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwSearchDetails.Handle.ToInt32(), v_vShowRowSelect:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

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
            ReDim m_ctlTabFirstLast(1, 2)

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

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            lblClient.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClientCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchDetails.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitlePolicyStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchDetails.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitlePolicyType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchDetails.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitleCoverStart, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchDetails.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitleCoverEnd, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchDetails.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitleRenewalDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchDetails.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitlePremium, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchDetails.Columns.Item(6).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitleMTAReason, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Cancel the ListPolicy", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelListPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Select the ListPolicy", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectListPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCompany (Standard Method)
    '
    ' Description: Gets valid Source ID's  and if nessessary displays selection
    '
    ' ***************************************************************** '
    Public Function GetCompany(ByRef m_iCompanyID As Integer) As Integer
        Dim result As Integer = 0

        'Developer Guide No. 88
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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Company", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompany", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

                'Developer Guide No. 243
                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            stbStatus.Text = " " & sMessage

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

                'Developer Guide No. 243
                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            'stbStatus.Text = " " & m_lItemsFound & " " & sMessage
            _stbStatus_Panel1.Text = " " & m_lItemsFound & " " & sMessage

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    'Return gPMConstants.PMEReturnCode.PMTrue
    'End If
    '
    'If txtClientCode.Text.Trim() <> "" Then
    'Return gPMConstants.PMEReturnCode.PMTrue
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

        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            ' Error Section.


            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function
    ' PRIVATE Methods (End)

    Private Sub lvwSearchDetails_ItemClick(ByVal Item As ListViewItem)
        If m_lLockedItem > 0 And Item.Index + 1 <> m_lLockedItem Then
            lvwSearchDetails.Items.Item(m_lLockedItem - 1).Selected = True
        End If
    End Sub

    Private Sub lvwSearchDetails_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSearchDetails.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y


        m_lSelected = gPMConstants.PMEReturnCode.PMTrue

        If lvwSearchDetails.GetItemAt(x, y) Is Nothing Then
            m_lSelected = gPMConstants.PMEReturnCode.PMFalse
        End If

        RaiseEvent lvwSearchDetailsMouseDown(Me, New lvwSearchDetailsMouseDownEventArgs(m_lSelected))

    End Sub

    ' PRIVATE Events (Begin)

    Private Sub uctPMUListPolicy_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        Try

            m_lReturn = ResizeInterface()

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub lvwSearchDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Click

        Dim sShortName As String = ""

        If lvwSearchDetails.Items.Count > 0 Then
            sShortName = lvwSearchDetails.FocusedItem.Text

            ' loop around and get the other details...
            For iCount As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                If CStr(m_vSearchData(ACIInsuranceRef, iCount)) = sShortName Then
                    Exit For
                End If
            Next iCount

            ' stick the other details in here...?

            ' Activate View button
        End If

    End Sub

    Private Sub lvwSearchDetails_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwSearchDetails.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        '    If (KeyCode <> 13) Then
        '        cmdOK.Default = False
        '    End If

    End Sub

    Public Function CancelClick() As Integer

        ' Click event of the Cancel button.

        Try


            Return CancelListPolicy()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function OKClick() As Integer

        Dim result As Integer = 0
        Try


            'CT 16/08/00 start
            'OKClick = SelectListPolicy()

            'The ok button on parent form in client manager has been clicked
            'so now act as if doubleclick on this component were pressed
            lvwSearchDetails_DoubleClick(lvwSearchDetails, New EventArgs())
            'CT 16/08/00 end 'CT 31/08/00 note that lvwSearchDetails_DblClick will set m_lReturn


            Return m_lReturn

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'ArtinsoftUtils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

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

                'Developer Guide No. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guide No. 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

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

            ' {* USER DEFINED CODE (End) *}

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ''upendra for test
            'm_lReturn = GetPolicies()

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

    Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick
        Dim vInstall As Object

        ' Double click event for the search details.

        Try

            ' Check if there are any items available.
            If lvwSearchDetails.Items.Count = 0 Then
                Exit Sub
            End If

            'CT 16/08/00  m_lReturn = OKClick
            m_lReturn = SelectListPolicy() 'CT 16/08/00

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'DD 18/08/2004 - Only show for Underwriting

            m_lReturn = iPMFunc.getUnderwritingOrAgency(r_vUnderwriting:=CStr(vInstall))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            If gPMFunctions.NullToString(vInstall) <> "A" Then
                'PSL 16/08/2003 4722 Warning if reinstating a cancelled policy
                If m_iTask = 2 Then
                    Select Case lvwSearchDetails.FocusedItem.Text 'PN19811
                        Case "Cancelled"
                            ' This prompt is now only valid during a proper mta reinstatement
                            If m_sTransactionType = "MTR" Then
                                If MessageBox.Show("Are you sure you want to re-instate this cancelled Policy", "Cancelled Policy", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                    m_lReturn = gPMConstants.PMEReturnCode.PMCancel
                                    Exit Sub
                                End If
                            Else
                                If m_sTransactionType = "EDIT" Then
                                    Call MsgBox("Unable to edit cancelled Policy", vbExclamation + vbOKOnly, "Cancelled Policy")
                                    m_lReturn = gPMConstants.PMEReturnCode.PMCancel
                                    Exit Sub
                                End If
                            End If

                    End Select
                End If
            End If

            RaiseEvent lvwSearchDetailsDblClick(Me, New lvwSearchDetailsDblClickEventArgs(m_lInsHolderCnt, m_lInsuranceFolderCnt, m_lInsFileCnt, m_sShortName, m_sInsReference, m_lPolicyTypeID))

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDetails.Columns(eventArgs.Column)

        Static lastColumn As Integer
        Static lastOrder As SortOrder
        Dim iOrder As SortOrder

        Try

            ' Alix - 05/01/2003 - PN7998
            ' I completly re-did this section as the original code was.... bad.
            ' This code was copied from uctRiskScreen and uses the ListView global fonctions

            If lastColumn = ColumnHeader.Index + 1 Then
                iOrder = Math.Abs(lastOrder - 1)
            Else
                iOrder = SortOrder.Ascending
            End If

            ListViewHelper.SetSortedProperty(lvwSearchDetails, True)

            Select Case (ColumnHeader.Index + 1)
                Case 3, 4, 5
                    ' Sort by date
                    'TODO
                    'ListViewSortByDate(lvwSearchDetails, ColumnHeader.Index + 1 - 1, iOrder)
                    ListViewFunc.ListViewSortByDate(lvwSearchDetails, ColumnHeader.Index + 1 - 1, iOrder)
                Case 6
                    ' Sort by currency
                    'TODO
                    'ListViewSortByValue(lvwSearchDetails, ColumnHeader.Index + 1 - 1, iOrder)
                    ListViewFunc.ListViewSortByValue(lvwSearchDetails, ColumnHeader.Index + 1 - 1, iOrder)
                Case Else
                    ' Default sort
                    ListViewHelper.SetSortOrderProperty(lvwSearchDetails, iOrder)
                    ListViewHelper.SetSortKeyProperty(lvwSearchDetails, ColumnHeader.Index + 1 - 1)
            End Select

            lastColumn = ColumnHeader.Index + 1
            lastOrder = iOrder

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' PRIVATE Events (End)

    Public Function CheckInClaim(ByVal v_sInsuranceRef As String, ByRef r_lClaimStatus As Integer, ByVal v_dtStartDate As Date) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.CheckInClaim(v_sInsuranceRef:=v_sInsuranceRef, r_lClaimStatus:=r_lClaimStatus, v_dtStartDate:=v_dtStartDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckInClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckInClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetBasePolicyCntForBackDateMTA(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtMTADate As Date, ByRef lBaseInsuranceFileCnt As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetBasePolicyCntForBackDateMTA(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_dtMTADate:=v_dtMTADate, lBaseInsuranceFileCnt:=lBaseInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBasePolicyCntForBackDateMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBasePolicyCntForBackDateMTA", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetCoverFromDate(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtMTADate As Date, ByVal lBaseInsuranceFileCnt As Integer, ByRef dtMTAEndDate As Object) As Integer



        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetCoverFromDate(v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_dtMTADate:=v_dtMTADate, lBaseInsuranceFileCnt:=lBaseInsuranceFileCnt, dtMTAEndDate:=dtMTAEndDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCoverFromDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCoverFromDate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    Public Function GetCoverEndDate(ByVal v_lInsuranceFolderCnt As Long,
                             ByVal v_dtMTADate As Date,
                             ByVal lBaseInsuranceFileCnt As Long,
                             ByRef dtMTAEndDate As Object) As Long


        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.GetCoverEndDate(v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt,
                                     v_dtMTADate:=v_dtMTADate,
                                     lBaseInsuranceFileCnt:=lBaseInsuranceFileCnt,
                                     dtMTAEndDate:=dtMTAEndDate)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCoverEndDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCoverEndDate", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=excep)

            Return result
        End Try

    End Function

    Public Function GetCancellationDate(ByVal v_lInsuranceFolderCnt As Integer, ByRef m_dtCancellationDate As Date, Optional ByRef r_lInsFileCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetCancellationDate(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, m_dtCancellationDate:=m_dtCancellationDate, r_lInsFileCnt:=r_lInsFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCancellationDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCancellationDate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetInsuranceFileStatus
    '
    ' Description:
    '
    ' History: 01/09/2009
    '
    ' ***************************************************************** '
    Public Function GetInsuranceFileStatus(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vArray As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetInsuranceFileStatus"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetInsuranceFileStatus(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vArray:=r_vArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get Insurance File Status")
            End If


        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuranceFileStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFileStatus", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            result = gPMConstants.PMEReturnCode.PMFalse

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function
    'Developer Guide No. 78
    Private Sub lvwSearchDetails_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lvwSearchDetails.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(e.KeyChar)

        Dim sShortName As String = ""

        If KeyAscii = 13 Then

            sShortName = lvwSearchDetails.FocusedItem.Text

            ' loop around and get the other details...
            For iCount As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                If m_vSearchData(ACIInsuranceRef, iCount) = sShortName Then
                    Exit For
                End If
            Next iCount

            ' stick the other details in here...?



        End If

        If KeyAscii = 0 Then
            e.Handled = True
        End If
        e.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub lvwSearchDetails_ItemChecked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwSearchDetails.ItemChecked
        If m_lLockedItem > 0 And e.Item.Index + 1 <> m_lLockedItem Then
            lvwSearchDetails.Items.Item(m_lLockedItem - 1).Selected = True
        End If

    End Sub


    Private Sub _tabMainTab_TabPage0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _tabMainTab_TabPage0.Click

    End Sub

    Private Sub lvwSearchDetails_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwSearchDetails.SelectedIndexChanged

    End Sub
    'PN 74021
    Public Function GetLatestPolicyVersion(ByVal v_lInsuranceFileCnt As Long, ByRef r_lPolicyVersion As Long) As Long

        Try

            GetLatestPolicyVersion = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.GetLatestPolicyVersion(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                     r_lPolicyVersion:=r_lPolicyVersion)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetLatestPolicyVersion = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If


            Exit Function

        Catch ex As Exception

            GetLatestPolicyVersion = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lInsuranceFileCnt", v_lInsuranceFileCnt)
            gPMFunctions.LogMessagePopup(
                iType:=gPMConstants.PMELogLevel.PMLogError,
                sMsg:="GetLatestPolicyVersion Failed",
                vApp:=ACApp,
                vClass:=ACClass,
                vMethod:="GetLatestPolicyVersion",
                    excep:=ex,
                    oDicParms:=oDict)
            Exit Function

        End Try
    End Function

    Public Function GetRenewalDetails(ByVal v_lInsuranceFileCnt As Long,
                                      ByRef r_dtResult As DataTable) As Long

        Const kMethodName As String = "GetRenewalDetails"
        Dim nResult As Integer
        Dim m_oRenewal As Object
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oObjectManager.GetInstance(
                oObject:=m_oRenewal,
                sClassName:="bSIRRenewal.Business",
                vInstanceManager:=PMGetViaClientManager)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to get Renewal Details")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oRenewal.GetRenewalDetails(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                     r_dtResult:=r_dtResult)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to get Renewal Details")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oRenewal.Dispose()
            m_oRenewal = Nothing
        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRenewalDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


            nResult = gPMConstants.PMEReturnCode.PMError

        Finally
        End Try
        Return nResult
    End Function

End Class
