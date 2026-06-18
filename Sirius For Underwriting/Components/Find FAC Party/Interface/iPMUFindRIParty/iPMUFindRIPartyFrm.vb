Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 17/02/1997
    '
    ' Description: Main interface.
    '
    ' Edit History:
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_sLongName As String = ""
    Private m_sPostalCode As String = ""
    Private m_sFileCode As String = ""
    Private m_iNotEditable As Integer

    Private m_bDeleteMode As Boolean

    Private m_vSourceArray As Object

    'Array for holding data for include Closed branch as well
    Private m_vSourceArrayIncludeClosedBranch As Object

    'Var for Checking that whether Include Closed Branch Checkbox is Checked or Not
    Private m_bIsIncludeClosedBranchChecked As Boolean


    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUFindRIParty.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Variables to store the lookup values/details.
    'Private m_vLookupValues As Variant
    'Private m_vLookUpDetails As Variant


    ' Declare an instance of the Lock object.
    Private m_oPMLock As Object


    Private m_sAgencyOrunderwriting As String = ""


    ' PartyType
    Private m_sPartyType As String = ""
    Private m_sPartyLongName As String = ""
    Private m_sPartyStatus As String = ""
    Private m_vNavStep As Object
    Private m_sUnderwritingType As String = ""
    Private m_bIncludeClosedBranches As Boolean

    'QBENZ005
    Private m_oXa As XArrayHelper

    Private cPremiumTax As Decimal
    Private cCommTax As Decimal


    Private m_obSIRReinsurance As bSIRReinsurance.Form
    Private vParticipantArray As XArrayHelper
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_iProcessId As Integer

    Private bIsFAx As Boolean
    Private m_lRi_Arrangement_Id As Integer
    Private m_lGroupingId As Integer
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_cUpperLimit As Decimal
    Private m_cLowerLimit As Decimal
    Private m_dRetained_percent As Double
    Private m_dParticipation_percent As Double
    Private m_dComm_percent As Double
    Private m_cSumInsured As Decimal
    Private m_cPremium As Decimal
    Private m_cPremiumTax As Decimal
    Private m_cCommission As Decimal
    Private m_cCommTax As Decimal
    Private m_lRiskId As Integer
    Private m_cTotalSumInsured As Decimal
    Private m_lInsuranceFileCnt As Integer

    Public m_vSearchData(,) As Object
    Private m_bParticipation, m_bPremium As Boolean
    'Developer Guide no. 33
    Private m_vExistingLimits As Object
    Private m_bGridPlacementCheck As Boolean
    Private m_bNewSearch As Boolean
    Private m_lClaim_id As Integer
    Private m_bIsChanged As Boolean
    Private m_bIsBrokerChanged As Boolean
    Private m_bFACPropExists As Boolean
    Private m_lOldGroupingid As Integer
    Private m_sLineAddMode As String = ""
    Private m_asDeletedParticipant(,) As String ' PN 40873
    Private m_vAddedFindRIPartyLines As Object 'PN 44646
    Private m_sAgreementCode As String = "" 'Sankar - PN 50348
    Private m_cGrossPremium As Decimal
    Public Property AgreementCode() As String
        Get
            Return m_sAgreementCode
        End Get
        Set(ByVal Value As String)
            m_sAgreementCode = Value
        End Set
    End Property

    'PN 44646
    'PN 44646
    Public Property AddedFindRIPartyLines() As Object
        Get
            Return m_vAddedFindRIPartyLines
        End Get
        Set(ByVal Value As Object)


            m_vAddedFindRIPartyLines = Value
        End Set
    End Property
    Public WriteOnly Property AddMode() As String
        Set(ByVal Value As String)
            m_sLineAddMode = Value
        End Set
    End Property



    Public Property IncludeClosedBranches() As Boolean
        Get
            Return m_bIncludeClosedBranches
        End Get
        Set(ByVal Value As Boolean)
            m_bIncludeClosedBranches = Value
        End Set
    End Property

    Public Property IsIncludeClosedBranchChecked() As Boolean
        Get
            Return m_bIsIncludeClosedBranchChecked
        End Get
        Set(ByVal Value As Boolean)
            m_bIsIncludeClosedBranchChecked = Value
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

    Public Property PartyStatus() As String
        Get
            Return m_sPartyStatus
        End Get
        Set(ByVal Value As String)
            m_sPartyStatus = Value
        End Set
    End Property

    Public Property FileCode() As String
        Get
            Return m_sFileCode
        End Get
        Set(ByVal Value As String)
            m_sFileCode = Value
        End Set
    End Property

    Public WriteOnly Property FACPropExists() As Boolean
        Set(ByVal Value As Boolean)
            m_bFACPropExists = Value
        End Set
    End Property


    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.
            m_sCallingAppName = Value

        End Set
    End Property
    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property


    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)
    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
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

            m_sTransactionType = Value

            Select Case m_sTransactionType
                Case "NB", "MTA", "REN", "MTC", "", "MTR", "PT", "DRI", "CRI"
                    m_iProcessId = 1
                    'Sankar - PN 69026 - Added for Salvage & Third Party Recovery
                Case "C_CO", "C_CP", "C_CR", "C_CV", "C_SA", "C_RV"
                    m_iProcessId = 2

            End Select
        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property

    ' {* USER DEFINED CODE (Begin) *}

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
        Set(ByVal Value As String)

            m_sShortName = Value

        End Set
    End Property


    Public Property LongName() As String
        Get

            Return m_sLongName

        End Get
        Set(ByVal Value As String)

            m_sLongName = Value

        End Set
    End Property


    Public Property NotEditable() As Integer
        Get

            Return m_iNotEditable

        End Get
        Set(ByVal Value As Integer)

            m_iNotEditable = Value

        End Set
    End Property


    Public Property DeleteMode() As Boolean
        Get

            Return m_bDeleteMode

        End Get
        Set(ByVal Value As Boolean)

            m_bDeleteMode = Value

        End Set
    End Property
    Public WriteOnly Property SourceArray() As Object
        Set(ByVal Value As Object)
            ' Set the valid sources for the user


            m_vSourceArray = Value
        End Set
    End Property
    Public WriteOnly Property SourceArrayIncludeClosedBranch() As Object
        Set(ByVal Value As Object)
            ' Set the valid sources for the user


            m_vSourceArrayIncludeClosedBranch = Value
        End Set
    End Property

    Public WriteOnly Property RIArrangementid() As Integer
        Set(ByVal Value As Integer)
            m_lRi_Arrangement_Id = Value
        End Set
    End Property

    'Developer Guide no. 33
    Public Property ExistingLimits() As Object
        Get
            Return m_vExistingLimits
        End Get
        'Developer Guide no. 33
        Set(ByVal Value As Object)
            m_vExistingLimits = Value
        End Set
    End Property


    Public Property ClaimId() As Integer
        Get
            Return m_lClaim_id
        End Get
        Set(ByVal Value As Integer)
            m_lClaim_id = Value
        End Set
    End Property


    Public Property IsFAX() As Boolean
        Get
            Return bIsFAx
        End Get
        Set(ByVal Value As Boolean)
            bIsFAx = Value
        End Set
    End Property

    Public WriteOnly Property Task() As Integer
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property


    Public Property UpperLimit() As Decimal
        Get
            Return m_cUpperLimit
        End Get
        Set(ByVal Value As Decimal)
            m_cUpperLimit = Value
        End Set
    End Property


    Public Property LowerLimit() As Decimal
        Get
            Return m_cLowerLimit
        End Get
        Set(ByVal Value As Decimal)
            m_cLowerLimit = Value
        End Set
    End Property


    Public Property GroupingId() As Integer
        Get
            Return m_lGroupingId
        End Get
        Set(ByVal Value As Integer)
            m_lGroupingId = Value
        End Set
    End Property

    Public ReadOnly Property SumInsured() As Decimal
        Get
            Return m_cSumInsured
        End Get
    End Property

    Public ReadOnly Property CommTax() As Decimal
        Get
            Return m_cCommTax
        End Get
    End Property

    Public ReadOnly Property Commission() As Decimal
        Get
            Return m_cCommission
        End Get
    End Property

    Public ReadOnly Property Premium() As Decimal
        Get
            Return m_cPremium
        End Get
    End Property

    Public ReadOnly Property PremiumTax() As Decimal
        Get
            Return m_cPremiumTax
        End Get
    End Property

    Public ReadOnly Property Retained_Percent() As Double
        Get
            Return m_dRetained_percent
        End Get
    End Property

    Public ReadOnly Property Comm_percent() As Double
        Get
            Return m_dComm_percent
        End Get
    End Property

    Public WriteOnly Property RiskId() As Integer
        Set(ByVal Value As Integer)
            m_lRiskId = Value
        End Set
    End Property

    Public Property TotalSumInsured() As Decimal
        Get
            Return m_cTotalSumInsured
        End Get
        Set(ByVal Value As Decimal)
            m_cTotalSumInsured = Value
        End Set
    End Property

    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public Property GrossPremium() As Decimal
        Get
            Return m_cGrossPremium
        End Get
        Set(ByVal value As Decimal)
            m_cGrossPremium = value
        End Set
    End Property


    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    ' ***************************************************************** '

    Public Function GetBusiness() As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while
            ' a search is in progress.
            m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If chkIncludeClosedBranches.CheckState = CheckState.Checked Then

                m_lReturn = g_oBusiness.FindNow(vName:=txtLongName.Text.Trim(), vShortName:=txtShortName.Text.Trim(), vFileCode:=txtFileCode.Text.Trim(), vValidBranches:=m_vSourceArrayIncludeClosedBranch, m_vSearchData:=m_vSearchData, m_lPartyCnt:=m_lPartyCnt, bIsFAx:=bIsFAx, bIsParticipant:=False)
            Else

                m_lReturn = g_oBusiness.FindNow(vName:=txtLongName.Text.Trim(), vShortName:=txtShortName.Text.Trim(), vFileCode:=txtFileCode.Text.Trim(), vValidBranches:=m_vSourceArray, m_vSearchData:=m_vSearchData, m_lPartyCnt:=m_lPartyCnt, bIsFAx:=bIsFAx, bIsParticipant:=False)
            End If

            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.
                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No search details found.
                Case Else
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
            End Select

            Return result

        Catch excep As System.Exception

            ''Debugger.Break()


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim iRow As Integer
        Dim vArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the search details.
            lvwSearchDetails.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                ' Reset the number of items found message.
                DisplayStatusFound()
                Return result
            End If


            ' Assign the details to the interface.
            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACIRI2007ShortName, lRow)).Trim())

                ' Assign details to other the columns
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vSearchData(ACIRI2007LongName, lRow)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vSearchData(ACIRI2007AccType, lRow)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vSearchData(ACIRI2007Address1, lRow)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vSearchData(ACIRI2007Address2, lRow)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vSearchData(ACIRI2007PostalCode, lRow)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 6).Text = "Reinsurer"
                ListViewHelper.GetListViewSubItem(oListItem, 7).Text = CStr(m_vSearchData(ACIRI2007SourceName, lRow)).Trim()

                ' {* USER DEFINED CODE (End) *}

                ' Set the tag property with the index of
                ' the search data storage.
                oListItem.Tag = CStr(lRow)

                ' Refresh the first X amount of rows, to
                ' allow the user to see the results instantly.
                If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.
                    lvwSearchDetails.Items.Item(0).Selected = True

                    ' Refresh the initial results.
                    lvwSearchDetails.Refresh()
                End If
                'End If
            Next lRow

            ' Select the first item.
            If lvwSearchDetails.Items.Count > 0 Then
                lvwSearchDetails.Items.Item(0).Selected = True


                'm_lPartyCnt = CLng(m_vSearchData(ACIRI2007PartyCnt, lvwSearchDetails.SelectedItem.Tag))

                ' Enable the interface now that the search has completed.
                m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Populate Grid
            If (m_iTask = gPMConstants.PMEComponentAction.PMView Or m_iTask = gPMConstants.PMEComponentAction.PMEdit) And Not m_bNewSearch Then

                If m_iProcessId = 1 Then

                    m_lReturn = g_oBusiness.GetGroupedRiLines(m_lRi_Arrangement_id:=m_lRi_Arrangement_Id, m_iProcessId:=m_iProcessId, m_lGroupingId:=m_lGroupingId, vArray:=vArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface")

                    End If

                    If Information.IsArray(vArray) Then

                        For lRow As Integer = 0 To vArray.GetUpperBound(1)
                            'm_oXa.AppendRows()
                            m_oXa.Rows.InsertAt(m_oXa.NewRow, m_oXa.Rows.Count - 1)
                            iRow = m_oXa.Rows.Count - 2  'm_oXa.GetUpperBound(0)

                            m_oXa(iRow, GridCol_ReinsurerCode) = CStr(vArray(GridCol_ReinsurerCode, lRow)).Trim()

                            m_oXa(iRow, GridCol_Name) = CStr(vArray(GridCol_Name, lRow)).Trim()

                            m_oXa(iRow, GridCol_AcType) = CStr(vArray(GridCol_AcType, lRow)).Trim()

                            m_oXa(iRow, GridCol_Premium) = vArray(GridCol_Premium, lRow)

                            m_oXa(iRow, GridCol_Participation) = CDbl(vArray(GridCol_Participation, lRow)) / 100

                            m_oXa(iRow, GridCol_SumInsured) = vArray(GridCol_SumInsured, lRow)

                            m_oXa(iRow, GridCol_Tax) = vArray(GridCol_Tax, lRow)

                            m_oXa(iRow, GridCol_Comm) = CDbl(vArray(GridCol_Comm, lRow)) / 100

                            m_oXa(iRow, GridCol_Commission) = vArray(GridCol_Commission, lRow)

                            m_oXa(iRow, GridCol_CommTax) = vArray(GridCol_CommTax, lRow)

                            m_oXa(iRow, GridCol_PartyCnt) = vArray(GridCol_PartyCnt, lRow)

                            m_oXa(iRow, GridCol_RILineId) = vArray(GridCol_RILineId, lRow)
                            'Start - Sankar - PN 50348

                            m_oXa(iRow, GridCol_AgreementCode) = gPMFunctions.NullToString(CStr(vArray(GridCol_AgreementCode, lRow)))
                            'End - Sankar - PN 50348
                        Next
                        If m_oXa.Rows.Count > 0 Then
                            grdPlacement.ReBind()
                            RefreshNetColumnTotal()
                        End If
                    End If
                Else

                    m_lReturn = g_oBusiness.GetClaimGroupedRiLines(m_lClaim_id:=m_lClaim_id, m_iProcessId:=m_iProcessId, m_lGroupingId:=m_lGroupingId, vArray:=vArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface")

                    End If

                    If Information.IsArray(vArray) Then

                        For lRow As Integer = 0 To vArray.GetUpperBound(1)
                            'm_oXa.AppendRows()
                            m_oXa.Rows.InsertAt(m_oXa.NewRow, m_oXa.Rows.Count - 1)
                            iRow = m_oXa.Rows.Count - 2  'm_oXa.GetUpperBound(0)

                            m_oXa(iRow, GridCol_ReinsurerCode) = CStr(vArray(GridCol_ReinsurerCode, lRow)).Trim()

                            m_oXa(iRow, GridCol_Name) = CStr(vArray(GridCol_Name, lRow)).Trim()

                            m_oXa(iRow, GridCol_AcType) = CStr(vArray(GridCol_AcType, lRow)).Trim()

                            m_oXa(iRow, GridCol_Participation) = CDbl(vArray(GridCol_Participation, lRow)) / 100

                            m_oXa(iRow, GridCol_SumInsured) = vArray(GridCol_SumInsured, lRow)

                            m_oXa(iRow, GridCol_PartyCnt) = vArray(6, lRow)

                            m_oXa(iRow, GridCol_RILineId) = vArray(7, lRow)
                        Next
                        If m_oXa.Rows.Count > 0 Then
                            grdPlacement.ReBind()
                            RefreshNetColumnTotal()
                        End If
                    End If
                End If
                If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                    For iCol As Integer = 0 To grdPlacement.Columns.Count - 1
                        Select Case iCol
                            Case GridCol_Premium, GridCol_Participation, GridCol_SumInsured, GridCol_Tax, GridCol_Comm, GridCol_Commission, GridCol_CommTax
                                grdPlacement.Columns(iCol).ReadOnly = True
                                grdPlacement.Columns(iCol).CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight
                            Case Else
                                grdPlacement.Columns(iCol).ReadOnly = True
                        End Select
                    Next
                End If
            End If

            ' Display the number of items found message.
            DisplayStatusFound()

            Return result

        Catch excep As System.Exception

            ''Debugger.Break()

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim lPartyID As Integer
        Dim bIsAgent, bIsAgencyAgreementValid As Boolean
        Dim sTitle, sMsg As String
        Dim bExistsOnSirius As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)

            ' Update the property members.
            ' {* USER DEFINED CODE (Begin) *}
            m_lPartyCnt = CInt(m_vSearchData(ACIRI2007PartyCnt, lSelectedItem))
            m_sShortName = CStr(m_vSearchData(ACIRI2007ShortName, lSelectedItem)).Trim()
            m_sLongName = CStr(m_vSearchData(ACIRI2007LongName, lSelectedItem)).Trim()
            m_sFileCode = CStr(m_vSearchData(ACIRI2007FileCode, lSelectedItem)).Trim()

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            ''Debugger.Break()


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LockParty
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (LockParty) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function LockParty() As Integer
    '
    'Dim result As Integer = 0
    'Dim sLockedBy As String = ""
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'm_lReturn = m_oPMLock.LockKey(sKeyName:="party_cnt", vKeyValue:=m_lPartyCnt, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy)
    '
    '
    'Select Case m_lReturn
    'Case gPMConstants.PMEReturnCode.PMTrue
    'OK
    '
    'Case gPMConstants.PMEReturnCode.PMFalse
    'Locked or error
    'If sLockedBy = "ERROR" Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to lock record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return result
    'Else
    'result = gPMConstants.PMEReturnCode.PMFalse
    'MessageBox.Show("Party currently locked by " & sLockedBy &  _
    '                Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Party Lock")
    'Return result
    'End If
    '
    '
    'Case Else
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock the party", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return result
    '
    'End Select
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: UnlockParty
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (UnlockParty) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function UnlockParty() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'm_lReturn = m_oPMLock.UnLockKey(sKeyName:="party_cnt", vKeyValue:=m_lPartyCnt, iUserID:=g_oObjectManager.UserID)
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to process the interface.
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to unlock the party", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return result
    '
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: PropertiesToInterface
    '
    ' Description: Updates the interface details from the property
    '              members.
    '
    ' ***************************************************************** '
    Public Function PropertiesToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            If m_iTask = gPMConstants.PMEComponentAction.PMView Or m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                txtFacUpperLimit.Text = m_cUpperLimit.ToString("N2")
                txtFacLowerLimit.Text = m_cLowerLimit.ToString("N2")
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                cmdEdit.Text = "View"
                cmdFindNow.Enabled = False
                cmdNewSearch.Enabled = False
                cmdSelect.Enabled = False
                cmdDelete.Enabled = False
                txtFacLowerLimit.ReadOnly = True
                txtFacUpperLimit.ReadOnly = True
                '    ElseIf m_iTask = PMEdit And m_sLineAddMode = "AU" Then
                '        cmdSelect.Enabled = False
                '        cmdDelete.Enabled = False
            End If


            Return result

        Catch excep As System.Exception

            ''Debugger.Break()


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        Dim result As Integer = 0
        Dim iTemp As Integer
        Dim bRestrictedInsurerAccess As Boolean
        Dim iCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            txtFacLowerLimit.Text = "0.00"
            txtFacUpperLimit.Text = "0.00"


            If m_iProcessId = 2 Then
                Me.Text = "CLAIM FAC XOL Placement"
            End If


            'TDbG Default Setting
            m_oXa = New XArrayHelper()
            m_oXa.RedimXArray(New Integer() {-1, 12}, New Integer() {0, 0})

            vParticipantArray = New XArrayHelper()
            vParticipantArray.RedimXArray(New Integer() {-1, 4}, New Integer() {0, 0})

            m_oXa.Rows.Add(m_oXa.NewRow)
            Dim dgVCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            dgVCellStyle2.Format = "0.00%"
            With grdPlacement

                Dim bindingSource As BindingSource = New BindingSource(m_oXa, "")
                .DataSource = bindingSource

                .ReBind()
                .Refresh()

                .Columns(GridCol_ReinsurerCode).HeaderText = "Reinsurer Code"
                .Columns(GridCol_ReinsurerCode).ReadOnly = True
                .Columns(GridCol_ReinsurerCode).Width = 100
                .Columns(GridCol_ReinsurerCode).DefaultCellStyle.Alignment = ContentAlignment.MiddleLeft
                .Columns(GridCol_ReinsurerCode).SortMode = DataGridViewColumnSortMode.NotSortable

                .Columns(GridCol_Name).HeaderText = "Name"
                .Columns(GridCol_Name).ReadOnly = True
                .Columns(GridCol_Name).Width = 200
                .Columns(GridCol_Name).DefaultCellStyle.Alignment = ContentAlignment.MiddleLeft
                .Columns(GridCol_Name).SortMode = DataGridViewColumnSortMode.NotSortable

                .Columns(GridCol_AcType).HeaderText = "Acct Type"
                .Columns(GridCol_AcType).ReadOnly = True
                .Columns(GridCol_AcType).Width = 50
                .Columns(GridCol_AcType).DefaultCellStyle.Alignment = ContentAlignment.MiddleCenter
                .Columns(GridCol_AcType).SortMode = DataGridViewColumnSortMode.NotSortable

                .Columns(GridCol_Participation).HeaderText = "Participation%"
                .Columns(GridCol_Participation).ReadOnly = False
                .Columns(GridCol_Participation).Width = 75
                .Columns(GridCol_Participation).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight
                .Columns(GridCol_Participation).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(GridCol_Participation).DefaultCellStyle.BackColor = Color.White

                .Columns(GridCol_SumInsured).HeaderText = "Sum Insured"
                .Columns(GridCol_SumInsured).ReadOnly = True
                .Columns(GridCol_SumInsured).Width = 100
                .Columns(GridCol_SumInsured).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight
                .Columns(GridCol_SumInsured).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(GridCol_SumInsured).DefaultCellStyle = dgVCellStyle2

                .Columns(GridCol_Premium).HeaderText = "Premium"
                .Columns(GridCol_Premium).ReadOnly = False
                .Columns(GridCol_Premium).Width = 100
                .Columns(GridCol_Premium).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight
                .Columns(GridCol_Premium).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(GridCol_Premium).DefaultCellStyle.BackColor = Color.White
                .Columns(GridCol_SumInsured).DefaultCellStyle = dgVCellStyle2

                .Columns(GridCol_Tax).HeaderText = "TAX"
                .Columns(GridCol_Tax).ReadOnly = True
                .Columns(GridCol_Tax).Width = 50
                .Columns(GridCol_Tax).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight
                .Columns(GridCol_Tax).SortMode = DataGridViewColumnSortMode.NotSortable

                .Columns(GridCol_Comm).HeaderText = "Comm%"
                .Columns(GridCol_Comm).ReadOnly = False
                .Columns(GridCol_Comm).Width = 75
                .Columns(GridCol_Comm).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight
                .Columns(GridCol_Comm).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(GridCol_Comm).DefaultCellStyle.BackColor = Color.White

                .Columns(GridCol_Commission).HeaderText = "Commission"
                .Columns(GridCol_Commission).ReadOnly = True
                .Columns(GridCol_Commission).Width = 100
                .Columns(GridCol_Commission).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight
                .Columns(GridCol_Commission).SortMode = DataGridViewColumnSortMode.NotSortable

                .Columns(GridCol_CommTax).HeaderText = "Comm Tax"
                .Columns(GridCol_CommTax).ReadOnly = True
                .Columns(GridCol_CommTax).Width = 75
                .Columns(GridCol_CommTax).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight
                .Columns(GridCol_CommTax).SortMode = DataGridViewColumnSortMode.NotSortable

                .Columns(GridCol_AgreementCode).HeaderText = "Agreement"
                .Columns(GridCol_AgreementCode).ReadOnly = False
                .Columns(GridCol_AgreementCode).Width = 75
                .Columns(GridCol_AgreementCode).DefaultCellStyle.Alignment = ContentAlignment.MiddleLeft
                .Columns(GridCol_AgreementCode).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(GridCol_AgreementCode).DefaultCellStyle.BackColor = Color.White

                .Columns(GridCol_PartyCnt).HeaderText = "PartCnt"
                .Columns(GridCol_PartyCnt).ReadOnly = True
                .Columns(GridCol_PartyCnt).Width = 0
                .Columns(GridCol_PartyCnt).DefaultCellStyle.Alignment = ContentAlignment.MiddleLeft
                .Columns(GridCol_PartyCnt).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(GridCol_PartyCnt).Visible = False

                .Columns(GridCol_RILineId).HeaderText = "RILineId"
                .Columns(GridCol_RILineId).ReadOnly = True
                .Columns(GridCol_RILineId).Width = 0
                .Columns(GridCol_RILineId).DefaultCellStyle.Alignment = ContentAlignment.MiddleLeft
                .Columns(GridCol_RILineId).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(GridCol_RILineId).Visible = False

                If m_iProcessId = 2 Then 'Claims
                    .Columns(GridCol_Premium).Visible = False
                    .Columns(GridCol_Tax).Visible = False
                    .Columns(GridCol_Comm).Visible = False
                    .Columns(GridCol_Commission).Visible = False
                    .Columns(GridCol_CommTax).Visible = False
                End If
            End With

            If cmbType.Items.Count = 0 Then
                cmbType.Items.Add("<ALL>")
                cmbType.Items.Add("Reinsurer")
                cmbType.SelectedIndex = 1
            End If

            grdPlacement.Rows(grdPlacement.Rows.Count - 1).DefaultCellStyle.BackColor = SystemColors.ButtonFace
            grdPlacement.Rows(grdPlacement.Rows.Count - 1).ReadOnly = True
            grdPlacement.Rows(grdPlacement.Rows.Count - 1).DefaultCellStyle.Font = New Font(grdPlacement.Font, FontStyle.Bold)
            m_oXa.Rows(m_oXa.Rows.Count - 1)(GridCol_AcType) = "Total:"


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            chkIncludeClosedBranches.CheckState = CheckState.Unchecked
            chkIncludeClosedBranches.Visible = m_bIncludeClosedBranches


            ' Set the column widths for the search list.
            'DC081101 increased from 1400 to 2000
            lvwSearchDetails.Columns.Insert(0, "", "Reinsurer Code", CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)
            lvwSearchDetails.Columns.Insert(1, "", "Name", CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)
            lvwSearchDetails.Columns.Insert(2, "", "Acc Type", CInt(VB6.TwipsToPixelsX(1000)), HorizontalAlignment.Left, -1)
            lvwSearchDetails.Columns.Insert(3, "", "Address Line 1", CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)
            lvwSearchDetails.Columns.Insert(4, "", "Address Line 2", CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)
            lvwSearchDetails.Columns.Insert(5, "", "Postcode", CInt(VB6.TwipsToPixelsX(1300)), HorizontalAlignment.Left, -1)
            lvwSearchDetails.Columns.Insert(6, "", "Type", CInt(VB6.TwipsToPixelsX(1300)), HorizontalAlignment.Left, -1)
            lvwSearchDetails.Columns.Insert(7, "", "Source", CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Left, -1)

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwSearchDetails.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}
            Return result

        Catch excep As System.Exception

            ''Debugger.Break()


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


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
    Private Function ClearInterface(ByRef bConfirm As Boolean) As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the interface details.

            If bConfirm Then

                ' Check if the user still wishes to clear
                ' the interface.


                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display the message.
                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    ' Don't continue with the clear.
                    Return result
                End If

            End If


            ' Clear the search data array.
            m_vSearchData = Nothing

            ' Clear the search list details.
            lvwSearchDetails.Items.Clear()

            ' Clear the search status bar.
            _stbStatus_Panel1.Text = ""

            ' {* USER DEFINED CODE (Begin) *}

            chkIncludeClosedBranches.CheckState = CheckState.Unchecked
            txtShortName.Text = ""
            txtLongName.Text = ""
            txtFileCode.Text = ""

            m_lPartyCnt = 0
            ' Set focus to the search details.
            txtShortName.Focus()

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

            ' {* USER DEFINED CODE (End) *}

            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception

            ''Debugger.Break()


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function ValidateUpperLowerLimits() As Integer

        Dim result As Integer = 0
        Dim cLowerLimit, cUpperLimit As Decimal
        Dim lGroupingId As Integer
        Dim sMessage As String = ""
        Dim bLimitsOverLap, bDeleted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validations

            If gPMFunctions.ToSafeCurrency(txtFacLowerLimit.Text.Trim()) < 0.01 Then
                MessageBox.Show("FAC XOL Lower Limit must be greater than 0.01", "Find RI Party", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtFacLowerLimit.Focus()
                result = gPMConstants.PMEReturnCode.PMFalse
                txtFacLowerLimit_Enter(txtFacLowerLimit, New EventArgs())
                Return result
            End If

            If gPMFunctions.ToSafeCurrency(txtFacUpperLimit.Text.Trim()) < 0.01 Then
                MessageBox.Show("FAC XOL Upper Limit must be greater than 0.01", "Find RI Party", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtFacUpperLimit.Focus()
                result = gPMConstants.PMEReturnCode.PMFalse
                txtFacUpperLimit_Enter(txtFacUpperLimit, New EventArgs())
                Return result
            ElseIf gPMFunctions.ToSafeCurrency(txtFacUpperLimit.Text.Trim()) > TotalSumInsured And m_iProcessId = 1 Then  'Sum Insured Validation is not applicable to Claims (Pn 37220).
                If Not m_bFACPropExists Then
                    MessageBox.Show("FAC XOL Upper Limit must be less than or equal to Gross Sum Insured", "Find RI Party", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("FAC XOL Upper Limit must be less than or equal to Gross Sum Insured less FAC Proportional Sum Insured(s)", "Find RI Party", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                txtFacUpperLimit.Focus()
                result = gPMConstants.PMEReturnCode.PMFalse
                txtFacUpperLimit_Enter(txtFacUpperLimit, New EventArgs())
                Return result
            End If

            If gPMFunctions.ToSafeCurrency(txtFacUpperLimit.Text.Trim()) <= gPMFunctions.ToSafeCurrency(txtFacLowerLimit.Text.Trim()) Then
                MessageBox.Show("FAC XOL Upper Limit must be greater than FAC XOL Lower Limit", "Find RI Party", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtFacUpperLimit.Focus()
                result = gPMConstants.PMEReturnCode.PMFalse
                txtFacUpperLimit_Enter(txtFacUpperLimit, New EventArgs())
                Return result
            End If

            If Information.IsArray(ExistingLimits) Then
                For lCounter As Integer = 0 To ExistingLimits.GetUpperBound(0)
                    cLowerLimit = ExistingLimits(lCounter, 0)
                    cUpperLimit = ExistingLimits(lCounter, 1)
                    lGroupingId = CInt(ExistingLimits(lCounter, 2))

                    bDeleted = False
                    If Information.IsArray(m_asDeletedParticipant) Then
                        For i As Integer = 0 To m_asDeletedParticipant.GetUpperBound(1)
                            If gPMFunctions.ToSafeLong(m_asDeletedParticipant(1, i)) = lGroupingId Then
                                bDeleted = True
                                Exit For
                            End If
                        Next i
                    End If

                    If m_lGroupingId <> lGroupingId And Not bDeleted Then
                        If (m_lOldGroupingid <> 0 And m_lOldGroupingid <> lGroupingId) Or m_lOldGroupingid = 0 Then
                            If gPMFunctions.ToSafeDouble(txtFacLowerLimit.Text) > gPMFunctions.ToSafeDouble(CStr(cLowerLimit)) And gPMFunctions.ToSafeDouble(txtFacLowerLimit.Text) < gPMFunctions.ToSafeDouble(CStr(cUpperLimit)) Then
                                sMessage = "FAC XOL lower limit overlaps with another FAC XOL Layer"
                                bLimitsOverLap = True
                                Exit For
                            ElseIf gPMFunctions.ToSafeDouble(txtFacUpperLimit.Text) > gPMFunctions.ToSafeDouble(CStr(cLowerLimit)) And gPMFunctions.ToSafeDouble(txtFacUpperLimit.Text) < gPMFunctions.ToSafeDouble(CStr(cUpperLimit)) Then
                                sMessage = "FAC XOL upper limit overlaps with another FAC XOL Layer"
                                bLimitsOverLap = True
                                Exit For
                            ElseIf gPMFunctions.ToSafeDouble(txtFacLowerLimit.Text) >= gPMFunctions.ToSafeDouble(CStr(cLowerLimit)) And gPMFunctions.ToSafeDouble(txtFacUpperLimit.Text) <= gPMFunctions.ToSafeDouble(CStr(cUpperLimit)) Then
                                sMessage = "FAC XOL lower limit overlaps with another FAC XOL Layer"
                                bLimitsOverLap = True
                                Exit For
                            ElseIf gPMFunctions.ToSafeDouble(txtFacLowerLimit.Text) <= gPMFunctions.ToSafeDouble(CStr(cLowerLimit)) And gPMFunctions.ToSafeDouble(txtFacUpperLimit.Text) >= gPMFunctions.ToSafeDouble(CStr(cUpperLimit)) Then
                                sMessage = "FAC XOL limit overlaps with another FAC XOL Layer"
                                bLimitsOverLap = True
                                Exit For
                            Else
                                bLimitsOverLap = False
                            End If
                        End If
                    End If
                Next lCounter
            End If
            If bLimitsOverLap Then
                MessageBox.Show(sMessage, "Validate FAC XOL Limits", MessageBoxButtons.OK, MessageBoxIcon.Information)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ''Debugger.Break()


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Validate Fac Xol Lower/Upper Limits", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateUpperLowerLimits", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdOK.Enabled = Not bDisable
            'If we're here we're searching.  Disable it until an item is clicked.
            '    cmdEdit.Enabled = False

            Return result

        Catch excep As System.Exception

            ''Debugger.Break()


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Display the status message.
            _stbStatus_Panel1.Text = " " & sMessage

        Catch excep As System.Exception

            ''Debugger.Break()


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
        Dim lItemsFound As Integer

        Try

            ' Store the total of item found.

            lItemsFound = lvwSearchDetails.Items.Count
            '
            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            _stbStatus_Panel1.Text = " " & lItemsFound & " " & sMessage

        Catch excep As System.Exception
            ''Debugger.Break()



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
    Private Function CheckMandatory() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check all fields for data.
            ' At least one field must be populated

            If txtShortName.Text.Trim() <> "" Then
                If txtShortName.Text.Trim().Length >= ACMinSearchLength Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If txtLongName.Text.Trim() <> "" Then
                If txtLongName.Text.Trim().Length >= ACMinSearchLength Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If txtFileCode.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If cmbType.Text.ToUpper() <> "<ALL>" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception
            ''Debugger.Break()



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for mandatory fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        Try
            Dim i As Integer
            Dim lCount As Integer
            Dim iArrayIndex As Integer
            Dim lNewGroupingId As Integer
            If Not grdPlacement.CurrentRow() Is Nothing Then
                If grdPlacement.CurrentRow().Equals(grdPlacement.Rows(grdPlacement.Rows.Count - 1)) Then
                    Exit Sub
                End If
            End If
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                m_lOldGroupingid = m_lGroupingId
                'delete from Ri_arrangement_line
                'Start PN 40873
                iArrayIndex = m_asDeletedParticipant.GetUpperBound(1)
                ReDim Preserve m_asDeletedParticipant(5, iArrayIndex + 1)
                If grdPlacement.CurrentRow().Cells(2).Value = "Broker" Then
                    m_asDeletedParticipant(0, iArrayIndex) = "Broker"

                    m_asDeletedParticipant(1, iArrayIndex) = grdPlacement.CurrentRow().Cells(12).Value
                    m_asDeletedParticipant(2, iArrayIndex) = "0"
                    m_asDeletedParticipant(3, iArrayIndex) = CStr(m_iProcessId)
                    m_asDeletedParticipant(4, iArrayIndex) = CStr(m_lClaim_id)
                    m_asDeletedParticipant(5, iArrayIndex) = CStr(lNewGroupingId)
                ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit And gPMFunctions.ToSafeLong(grdPlacement.CurrentRow().Cells(12).Value) > 0 Then
                    m_asDeletedParticipant(0, iArrayIndex) = ""

                    m_asDeletedParticipant(1, iArrayIndex) = grdPlacement.CurrentRow().Cells(12).Value
                    m_asDeletedParticipant(2, iArrayIndex) = ""
                    m_asDeletedParticipant(3, iArrayIndex) = CStr(m_iProcessId)
                    m_asDeletedParticipant(4, iArrayIndex) = CStr(m_lClaim_id)
                    m_asDeletedParticipant(5, iArrayIndex) = CStr(lNewGroupingId)
                End If
                'End PN 40873
            End If

            With grdPlacement

                If .RowsCount > 0 Then
                    If .Columns(GridCol_AcType).HeaderText = "Broker" Then
                        If vParticipantArray.GetUpperBound(0) > -1 Then
                            lCount = vParticipantArray.GetUpperBound(0)
                            For iRow As Integer = 0 To lCount
                                For iRow1 As Integer = 0 To vParticipantArray.GetUpperBound(0)
                                    'Developer Guide no. 188 (Latest Guide)
                                    If grdPlacement.CurrentRow().Cells(MainModule.GridCol_PartyCnt).Value = vParticipantArray(iRow1, ACIBrokerAssociationPartyCnt) Then
                                        vParticipantArray.DeleteRows(iRow1)
                                        Exit For
                                    End If
                                Next
                            Next
                        End If
                    End If
                    m_oXa.Rows(.CurrentRowIndex).Delete()
                    m_oXa.AcceptChanges()
                End If
            End With


            m_lReturn = RefreshNetColumnTotal()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CmdDelete_Click", "Failed to refresh column totals.", gPMConstants.PMELogLevel.PMLogError)
                Exit Sub
            End If

            If grdPlacement.RowsCount = 2 Then ' In Case of deleting all the lines in Edit Mode
                m_lGroupingId = 0
            End If
            grdPlacement.Rows(grdPlacement.Rows.Count - 1).DefaultCellStyle.BackColor = SystemColors.ButtonFace
            grdPlacement.Rows(grdPlacement.Rows.Count - 1).ReadOnly = True
            grdPlacement.Rows(grdPlacement.Rows.Count - 1).DefaultCellStyle.Font = New Font(grdPlacement.Font, FontStyle.Bold)
        Catch excep As System.Exception

            ''Debugger.Break()

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Delete command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try
    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

        Try

            ' Clear the interface details.
            m_lReturn = CType(ClearInterface(bConfirm:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If

        Catch excep As System.Exception
            ''Debugger.Break()



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelect.Click

        Dim rowlv, iRow As Integer
        Dim bExists As Boolean

        If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
            If lvwSearchDetails.Items.Count < 1 Then Exit Sub

            If lvwSearchDetails.Items.Count > 0 Then
                If lvwSearchDetails.FocusedItem Is Nothing Then
                    lvwSearchDetails.Items(0).Selected = True
                    lvwSearchDetails.Items(0).Focused = True
                End If
            End If

            rowlv = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)
            If m_oXa.Rows.Count > 1 Then
                For i As Integer = 0 To m_oXa.Rows.Count - 2 'm_oXa.GetUpperBound(0)
                    'Developer Guide no. 188
                    If m_oXa(i, 0) = m_vSearchData(MainModule.ACIRI2007ShortName, rowlv).Trim() Then
                        bExists = True
                        Exit For
                    End If
                Next
            End If
            If bExists Then
                MessageBox.Show("Reinsurer is already on the list.", "Duplicate Check", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            If CStr(m_vSearchData(ACIRI2007AccType, rowlv)).Trim() <> "Broker" Then

                m_lReturn = CType(AddGridRow(rowlv), gPMConstants.PMEReturnCode)
            Else
                'Call Broker Participant Screen
                m_lReturn = CType(ProcessRIBrokerParticipants(CInt(m_vSearchData(ACIRI2007PartyCnt, rowlv)), 1, rowlv), gPMConstants.PMEReturnCode)
            End If

            grdPlacement.Rows(grdPlacement.Rows.Count - 1).DefaultCellStyle.BackColor = SystemColors.ButtonFace
            grdPlacement.Rows(grdPlacement.Rows.Count - 1).ReadOnly = True
            grdPlacement.Rows(grdPlacement.Rows.Count - 1).DefaultCellStyle.Font = New Font(grdPlacement.Font, FontStyle.Bold)
        End If
    End Sub


    Private Sub cmdSelect_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cmdSelect.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        If KeyCode = 9 Then
            txtFacLowerLimit.Focus()
        End If
    End Sub


    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sValue As String = ""

        ' Forms initialise event.

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue


            ' Create an instance of the general interface object.
            m_oGeneral = New iPMUFindRIParty.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'Get bPMLock
            Dim temp_m_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMLock = temp_m_oPMLock
            'Initilize Dynamic Array of Deleted Participants

            ReDim Preserve m_asDeletedParticipant(5, 0) 'PN 40873

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                'Initialise = PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If


            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel


            ' Get an instance of the business object via the public object manager.
            ' For Tax Calculation
            ' Puneet
            Dim temp_m_obSIRReinsurance As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_obSIRReinsurance, "bSIRReinsurance.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_obSIRReinsurance = temp_m_obSIRReinsurance
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            ''Debugger.Break()

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            iPMFunc.ShowFormInTaskBar_Detach()

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            m_sAgencyOrunderwriting = g_oBusiness.UnderwritingOrAgency


            m_sUnderwritingType = g_oBusiness.UnderwritingType


            m_obSIRReinsurance.InsuranceFileCnt = m_lInsuranceFileCnt

            m_obSIRReinsurance.RiskID = m_lRiskId


            g_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt

            g_oBusiness.RiskCnt = m_lRiskId


            ' Set the interface default values.
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


            '    m_lReturn& = GetBusiness()


            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            ''Debugger.Break()


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel ''This code has been added to pass the Cancel status when user click on (X) button. PN No. 62070
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'Developer Guide No. 7
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Destroy the instance of the lock object
            ' from memory.
            If Not (m_oPMLock Is Nothing) Then
                m_oPMLock = Nothing
            End If

            If Not (m_obSIRReinsurance Is Nothing) Then
                m_obSIRReinsurance = Nothing
            End If


            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception

            ''Debugger.Break()


            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub



    Private Sub grdPlacement_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles grdPlacement.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        Dim iRowIndex As Integer
        Try

            Dim counter As Integer
            If (KeyCode = 9) And Not (Convert.IsDBNull(grdPlacement.Rows(0).Index) Or IsNothing(grdPlacement.Rows(0).Index)) Then

                iRowIndex = grdPlacement.Rows(0).Index
                counter = grdPlacement.CurrentCell.ColumnIndex
                For iColIndex As Integer = counter To grdPlacement.Columns.Count - 1

                    'TODO
                    'If grdPlacement.Splits(0).Columns(iColIndex).style.Value <> "Locked" And grdPlacement.Splits(0).Columns(iColIndex).style.Value <> "RightAlignLocked" Then
                    'If grdPlacement.CurrentCell.ColumnIndex >= 10 Then
                    '    If m_oXa.GetUpperBound(0) >= 1 And grdPlacement.CurrentRowIndex + iRowIndex < m_oXa.GetUpperBound(0) Then
                    '        grdPlacement.CurrentRowIndex += 1
                    '    Else
                    '        grdPlacement.MoveNext()
                    '        cmdEdit.Focus()
                    '    End If
                    '    Exit Sub
                    'End If
                    'If Not IsNothing(grdPlacement.CurrentRow) Then
                    '    grdPlacement.CurrentCell = grdPlacement.CurrentRow.Cells(iColIndex)
                    'End If
                    'Exit For
                    'End If
                Next

            End If

        Catch excep As System.Exception

            ''Debugger.Break()
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the grdPlacement_KeyUp", vApp:=ACApp, vClass:=ACClass, vMethod:="grdPlacement_KeyUp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try
    End Sub

    Private Sub lvwSearchDetails_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwSearchDetails.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        Dim iRow As Integer

        ' Ram 22-03-2001
        ' To Show the proper telephone number we have to display the
        ' details of the selected row rather than checking the short name
        ' Since a Party can have multiple Telephone Number
        If lvwSearchDetails.Items.Count > 0 Then
            If (eventArgs.KeyCode = Keys.Up) Or eventArgs.KeyCode = Keys.Down Then

                iRow = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)
                txtShortName.Text = CStr(m_vSearchData(ACIRI2007ShortName, iRow)).Trim()
                txtLongName.Text = CStr(m_vSearchData(ACIRI2007LongName, iRow)).Trim()
                txtFileCode.Text = CStr(m_vSearchData(ACIRI2007FileCode, iRow)).Trim()
            End If
        End If

    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click


        Try

            'PN 40873
            Dim iArrayIndex As Integer 'PN 40873
            'Blank Row Check
            Dim iRow, iCol As Integer
            Dim sReInsCode As String = ""

            Dim cTotalPremium As Decimal


            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then

                m_lReturn = ValidateUpperLowerLimits()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If


                iRow = m_oXa.Rows.Count - 2
                If iRow < 0 Then
                    MessageBox.Show("No Placement Details present to save", "Find RI Party", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    cmdSelect.Focus()
                    Exit Sub
                End If

                'Participation % Check'
                If ToSafeDecimal(grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(GridCol_Participation).Value.ToString.Replace("%", "")) <> 100.0 Then
                    MessageBox.Show("Total Participant percentage should be equal to 100%.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If


                m_lReturn = PlacementDetailsMandatory()
                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    If m_iProcessId = 1 Then
                        If m_bPremium Then
                            MessageBox.Show("Premium is mandatory", "Mandatory Check", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            grdPlacement.Focus()
                            Exit Sub
                        End If
                    End If
                    If m_bParticipation Then
                        MessageBox.Show("Participation% is mandatory", "Mandatory Check", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        grdPlacement.Focus()
                        Exit Sub
                    End If

                End If



                cTotalPremium = ToSafeDecimal(grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(GridCol_Premium).Value, 0)

                If IsFAX = True Then
                    If cTotalPremium > m_cGrossPremium Then
                        MessageBox.Show("FAC XOL premium is more than the band premium, please change the premium to proceed further", "Find RI Party", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        grdPlacement.Focus()
                        Exit Sub
                    End If
                End If


                'Start PN 40873
                iArrayIndex = m_asDeletedParticipant.GetUpperBound(1)
                If m_iTask = gPMConstants.PMEComponentAction.PMEdit And iArrayIndex > 0 Then
                    For i As Integer = 0 To iArrayIndex - 1
                        m_lOldGroupingid = m_lGroupingId
                        'delete from Ri_arrangement_line
                        If m_asDeletedParticipant(0, i) = "Broker" Then

                            m_lReturn = g_oBusiness.DeleteBrokerParticipants(m_lRi_Arrangement_line_id:=gPMFunctions.ToSafeLong(m_asDeletedParticipant(1, i)), m_lPartyCnt:=0, m_iProcessId:=m_asDeletedParticipant(3, i))
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("CmdOK_click", "Failed to delete attached broker participants.", gPMConstants.PMELogLevel.PMLogError)
                                Exit Sub
                            End If
                        End If
                        If m_iTask = gPMConstants.PMEComponentAction.PMEdit And gPMFunctions.ToSafeLong(m_asDeletedParticipant(1, i)) > 0 Then

                            m_lReturn = g_oBusiness.DeletePlacementRILine(m_lRi_Arrangement_line_id:=gPMFunctions.ToSafeLong(m_asDeletedParticipant(1, i)), m_iProcessId:=gPMFunctions.ToSafeInteger(m_asDeletedParticipant(3, i)), ClaimId:=gPMFunctions.ToSafeLong(m_asDeletedParticipant(4, i)), GroupingId:=gPMFunctions.ToSafeLong(m_asDeletedParticipant(5, i)))
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("CmdOK_click", "Failed to delete Reinsurer Placement Line.", gPMConstants.PMELogLevel.PMLogError)
                                Exit Sub
                            End If
                            If m_lGroupingId <> gPMFunctions.ToSafeLong(m_asDeletedParticipant(5, i)) Then
                                m_lGroupingId = CInt(m_asDeletedParticipant(5, i))
                            End If
                        End If
                    Next
                End If
                'End PN 40873


                'Set Properties
                InterfaceToProperties()

                ' Update the business from the interface.
                m_lReturn = CType(InterfaceToBusiness(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                End If
            End If
            'Developer Guide No. 231
            Me.Hide()

        Catch excep As System.Exception

            ''Debugger.Break()

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_lPartyCnt = 0 'PN20059

                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception

            ''Debugger.Break()


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click

        ' Click event of the Cancel button.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get the interface details from the business object.
            m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the details.
            End If

            m_bNewSearch = True

            ' Assign the details from the search data storage
            ' to the interface.
            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the details.
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
            End If

            If lvwSearchDetails.Items.Count > 0 Then
                VB6.SetDefault(cmdFindNow, False)
                VB6.SetDefault(cmdOK, False)
                cmdEdit.Enabled = True
            End If

            ' Set the focus.
            lvwSearchDetails.Focus()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            ''Debugger.Break()


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Find Now command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFindNow_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        Dim m_lRiArrangement_id As Integer
        Dim sStr As String = ""
        ' Click event of the Edit Button.
        Try

            If Not grdPlacement.CurrentRow() Is Nothing Then
                If grdPlacement.CurrentRow().Equals(grdPlacement.Rows(grdPlacement.Rows.Count - 1)) Then
                    Exit Sub
                End If
            End If
            'if is_ri_broker then open Broker participants screen by sending Ri Arrangement line
            Dim rowlv, iRow As Integer

            If Convert.IsDBNull(grdPlacement.Rows(0).Index) Or IsNothing(grdPlacement.Rows(0).Index) Or Convert.IsDBNull(grdPlacement.CurrentRowIndex) Or IsNothing(grdPlacement.CurrentRowIndex) Then
                Exit Sub
            End If

            If m_iProcessId = 1 Then
                sStr = "POLICY"
            Else
                sStr = "CLAIM"
            End If

            rowlv = grdPlacement.CurrentRowIndex + grdPlacement.Rows(0).Index

            If grdPlacement.CurrentRow().Cells(GridCol_AcType).Value = "Broker" Then
                'Call Broker Participant Screen
                m_lReturn = CType(ProcessRIBrokerParticipants(grdPlacement.CurrentRow().Cells(GridCol_PartyCnt).Value, 2, rowlv, grdPlacement.CurrentRow().Cells(GridCol_RILineId).Value), gPMConstants.PMEReturnCode)
            Else
                MessageBox.Show("The 'Edit' of " & sStr & " RI Broker Participants is only relevant and available if the Acc Type equal 'Broker'.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch excep As System.Exception

            ''Debugger.Break()
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Edit button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    'QBENZ005
    Private Function ProcessRIBrokerParticipants(ByVal vPartyInsurerCnt As Integer, ByVal m_iAction As Integer, ByVal lRowLv As Integer, Optional ByRef lRiArrangementlineId As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim oForm As frmRIBrokerParticipants
        Dim iRow, iRow1 As Integer
        Dim lCount As Integer
        Dim m_vSelectedArray(,) As Object
        Dim vBrokerArray(,) As Object

        Const kMethodName As String = "ProcessRIBrokerParticipants"
        Try


            oForm = New frmRIBrokerParticipants()
            oForm.TransactionType = m_sTransactionType


            'Developer Guide no. 24
            oForm.SourceArray = m_vSourceArray
            'Add Action i.e. click Select = 1  or edit = 2 or view = 3
            oForm.Action = m_iAction


            'Developer Guide no. 24
            oForm.SourceArrayIncludeClosedBranch = m_vSourceArrayIncludeClosedBranch
            oForm.IncludeClosedBranches = m_bIncludeClosedBranches
            If m_iAction = 2 Then
                If lRiArrangementlineId = 0 Or m_bIsBrokerChanged Then
                    lCount = 0
                    For iRow = 0 To vParticipantArray.GetUpperBound(0)
                        'Developer Guide no. 188
                        If ToSafeInteger(vParticipantArray(iRow, ACIBrokerAssociationPartyCnt)) = vPartyInsurerCnt Then
                            lCount += 1
                        End If
                    Next
                    ReDim m_vSelectedArray(lCount - 1, 4)
                    iRow1 = 0
                    For iRow = 0 To vParticipantArray.GetUpperBound(0)
                        'Developer Guide no. 188
                        If ToSafeInteger(vParticipantArray(iRow, ACIBrokerAssociationPartyCnt)) = vPartyInsurerCnt Then

                            'Developer Guide no. 188
                            m_vSelectedArray(iRow1, MainModule.ACIBrokerShortName) = vParticipantArray(iRow1, ACIBrokerShortName)

                            'Developer Guide no. 188
                            m_vSelectedArray(iRow1, MainModule.ACIBrokerLongName) = vParticipantArray(iRow1, ACIBrokerLongName)

                            'Developer Guide no. 188
                            m_vSelectedArray(iRow1, MainModule.ACIBrokerParticipant_percent) = vParticipantArray(iRow1, ACIBrokerParticipant_percent)

                            'Developer Guide no. 188
                            m_vSelectedArray(iRow1, MainModule.ACIBrokerAssociationPartyCnt) = vParticipantArray(iRow1, ACIBrokerAssociationPartyCnt)

                            'Developer Guide no. 188
                            m_vSelectedArray(iRow1, MainModule.ACIBrokerPartyCnt) = vParticipantArray(iRow1, ACIBrokerPartyCnt)
                            iRow1 += 1
                        End If
                    Next

                    oForm.BrokerArray = m_vSelectedArray
                Else
                    oForm.RiArrangementLineID = lRiArrangementlineId
                End If
            End If
            oForm.PartyCnt = vPartyInsurerCnt
            oForm.IsFAX = bIsFAx
            oForm.Task = m_iTask
            oForm.IsBrokerChanged = m_bIsBrokerChanged
            If m_iProcessId = 2 Then
                oForm.Text = "CLAIM RI Broker Participants"
            End If

            oForm.ShowDialog()

            If oForm.Status = gPMConstants.PMEReturnCode.PMOK Then
                'get the broker array

                vBrokerArray = oForm.BrokerArray

                If m_iAction = 2 Then
                    If vParticipantArray.Rows.Count > 0 Then
                        'lCount = vParticipantArray.GetUpperBound(0)
                        For iRow = 0 To vParticipantArray.Rows.Count - 2
                            'For iRow1 = 0 To vParticipantArray.GetUpperBound(0)
                            'Developer Guide no. 188
                            If ToSafeInteger(grdPlacement.Rows(iRow).Cells(MainModule.GridCol_PartyCnt).Value) = ToSafeInteger(vParticipantArray(iRow, ACIBrokerAssociationPartyCnt)) Then
                                'vParticipantArray.DeleteRows(iRow1)
                                vParticipantArray.Rows.RemoveAt(iRow)
                                vParticipantArray.AcceptChanges()
                                Exit For
                            End If
                            'Next
                        Next
                    End If
                    m_bIsBrokerChanged = oForm.IsBrokerChanged
                End If

                If Information.IsArray(vBrokerArray) Then

                    For lCount = 0 To vBrokerArray.GetUpperBound(0)
                        vParticipantArray.AppendRows()
                        iRow = vParticipantArray.GetUpperBound(0)

                        vParticipantArray(iRow, ACIBrokerShortName) = vBrokerArray(lCount, ACIBrokerShortName)

                        vParticipantArray(iRow, ACIBrokerLongName) = vBrokerArray(lCount, ACIBrokerLongName)

                        vParticipantArray(iRow, ACIBrokerParticipant_percent) = vBrokerArray(lCount, ACIBrokerParticipant_percent)

                        vParticipantArray(iRow, ACIBrokerAssociationPartyCnt) = vBrokerArray(lCount, ACIBrokerAssociationPartyCnt)

                        vParticipantArray(iRow, ACIBrokerPartyCnt) = vBrokerArray(lCount, ACIBrokerPartyCnt)
                    Next
                End If
                If m_iAction = 1 Then
                    'Add rows to Grid
                    m_lReturn = CType(AddGridRow(lRowLv), gPMConstants.PMEReturnCode)
                End If
                '    Else
                '        m_lStatus = PMCancel
            End If
            oForm.Close()

            Return result

        Catch ex As Exception
            ''Debugger.Break()
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result
        End Try
    End Function


    Private Function AddGridRow(ByVal lRowLv As Integer) As Integer

        Dim result As Integer = 0
        Dim iRow As Integer

        Const kMethodName As String = "AddGridRow"
        Try

            m_oXa.Rows.InsertAt(m_oXa.NewRow, m_oXa.Rows.Count - 1)
            iRow = m_oXa.Rows.Count - 2 'm_oXa.GetUpperBound(0)

            'grdPlacement.CurrentRowIndex = grdPlacement.RowsCount - 1
            m_oXa(iRow, GridCol_ReinsurerCode) = CStr(m_vSearchData(ACIRI2007ShortName, lRowLv)).Trim()
            m_oXa(iRow, GridCol_Name) = CStr(m_vSearchData(ACIRI2007LongName, lRowLv)).Trim()
            m_oXa(iRow, GridCol_AcType) = CStr(m_vSearchData(ACIRI2007AccType, lRowLv)).Trim()

            m_oXa(iRow, GridCol_Premium) = 0
            m_oXa(iRow, GridCol_Participation) = 0
            m_oXa(iRow, GridCol_SumInsured) = 0
            m_oXa(iRow, GridCol_Tax) = 0
            m_oXa(iRow, GridCol_Comm) = CStr(m_vSearchData(ACIRI2007DefaultCommission, lRowLv)).Trim()
            m_oXa(iRow, GridCol_CommTax) = 0
            m_oXa(iRow, GridCol_Commission) = 0

            m_oXa(iRow, GridCol_PartyCnt) = CStr(m_vSearchData(ACIRI2007PartyCnt, lRowLv)).Trim()
            m_oXa(iRow, GridCol_RILineId) = 0
            grdPlacement.ReBind()

            Return result

        Catch ex As Exception
            ''Debugger.Break()
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            Return result
        End Try
    End Function


    Private Sub lvwSearchDetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Enter

        ' GotFocus Event for the search details

        Try

            ' Unset any default buttons so can
            VB6.SetDefault(cmdFindNow, False)
            VB6.SetDefault(cmdOK, False)

        Catch excep As System.Exception

            ''Debugger.Break()


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Click

        Dim sShortName As String = ""
        Dim iCount, iRow, n As Integer

        If lvwSearchDetails.Items.Count > 0 Then
            sShortName = lvwSearchDetails.FocusedItem.Text

            ' To Show the proper telephone number we have to display the
            ' details of the selected row rather than checking the short name
            ' Since a Party can have multiple Telephone Number

            iRow = gPMFunctions.ToSafeInteger(Convert.ToString(lvwSearchDetails.FocusedItem.Tag))

            m_lPartyCnt = CInt(m_vSearchData(ACIRI2007PartyCnt, iRow))

            txtShortName.Text = CStr(m_vSearchData(ACIRI2007ShortName, iRow)).Trim()
            txtLongName.Text = CStr(m_vSearchData(ACIRI2007LongName, iRow)).Trim()
            txtFileCode.Text = CStr(m_vSearchData(ACIRI2007FileCode, iRow)).Trim()

            VB6.SetDefault(cmdSelect, True)
        End If

    End Sub

    Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick

        ' Double click event for the search details.

        Try

            'MKR 21/07/04   - Start
            'Issue No. 13511 Multiple errors when double clicking on empty Find Client list view
            If (lvwSearchDetails.Items.Count > 0) And cmdSelect.Enabled Then
                ' CTAF 260601
                ' Call OK_Click. Its just the same code
                cmdSelect_Click(cmdSelect, New EventArgs())
            End If
            'MKR 21/07/04   - End

        Catch excep As System.Exception

            ''Debugger.Break()


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwSearchDetails.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        If KeyCode <> 13 Then
            VB6.SetDefault(cmdSelect, False)
        End If
    End Sub

    Private Sub lvwSearchDetails_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles lvwSearchDetails.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        Dim sShortName As String = ""
        Dim iCount, iRow As Integer

        If KeyAscii = 13 Then
            If lvwSearchDetails.Items.Count > 0 Then
                sShortName = lvwSearchDetails.FocusedItem.Text


                iRow = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)
                txtShortName.Text = CStr(m_vSearchData(ACIRI2007ShortName, iRow)).Trim()
                txtLongName.Text = CStr(m_vSearchData(ACIRI2007LongName, iRow)).Trim()
                txtFileCode.Text = CStr(m_vSearchData(ACIRI2007FileCode, iRow)).Trim()

                VB6.SetDefault(cmdSelect, True)
            End If
        End If

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDetails.Columns(eventArgs.Column)

        ' Column click event for the search details

        Try

            With lvwSearchDetails
                ' If current sort column header is
                ' pressed.
                If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwSearchDetails) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwSearchDetails, (ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwSearchDetails, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwSearchDetails, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwSearchDetails, True)
                End If
            End With

        Catch excep As System.Exception

            ''Debugger.Break()


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Private Sub txtFacLowerLimit_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFacLowerLimit.Enter
        txtFacLowerLimit.SelectionStart = 0
        txtFacLowerLimit.SelectionLength = Strings.Len(txtFacLowerLimit.Text)
        VB6.SetDefault(cmdOK, True)
    End Sub


    Private Sub txtFacLowerLimit_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles txtFacLowerLimit.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        If KeyCode <> 13 Then
            VB6.SetDefault(cmdOK, False)
        End If
    End Sub

    Private Sub txtFacLowerLimit_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFacLowerLimit.Leave
        Dim dbNumericTemp As Double
        If Double.TryParse(txtFacLowerLimit.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            If Conversion.Val(txtFacLowerLimit.Text) < 0 Then
                txtFacLowerLimit.Text = CStr(CDbl(txtFacLowerLimit.Text) * -1)
            End If
        Else
            MessageBox.Show("Please Enter a Valid Amount", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtFacLowerLimit.Text = "0.00"
            txtFacLowerLimit_Enter(txtFacLowerLimit, New EventArgs())
            Exit Sub
        End If

        'txtFacLowerLimit.Text = Format(txtFacLowerLimit.Text, "###,##0.00")

        txtFacLowerLimit.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=txtFacLowerLimit.Text)


        m_lReturn = CalcSumInsured()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("txtFACLowerLimit_LostFocus", "Failed to calculate SumInsured.", gPMConstants.PMELogLevel.PMLogError)
            Exit Sub
        End If

        m_lReturn = RefreshNetColumnTotal()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("txtFACLowerLimit_LostFocus", "Failed to Refresh column totals in grid.", gPMConstants.PMELogLevel.PMLogError)
            Exit Sub
        End If

        VB6.SetDefault(cmdOK, True)

    End Sub



    Private Sub txtFacUpperLimit_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFacUpperLimit.Enter
        txtFacUpperLimit.SelectionStart = 0
        txtFacUpperLimit.SelectionLength = Strings.Len(txtFacUpperLimit.Text)
        VB6.SetDefault(cmdOK, True)
    End Sub

    Private Sub txtFacUpperLimit_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles txtFacUpperLimit.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        If KeyCode <> 13 Then
            VB6.SetDefault(cmdOK, False)
        End If
    End Sub

    Private Sub txtFacUpperLimit_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFacUpperLimit.Leave
        Dim dbNumericTemp As Double
        If Double.TryParse(txtFacUpperLimit.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            If Conversion.Val(txtFacUpperLimit.Text) < 0 Then
                txtFacUpperLimit.Text = CStr(CDbl(txtFacUpperLimit.Text) * -1)
            End If
        Else
            MessageBox.Show("Please Enter a Valid Amount", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtFacUpperLimit.Text = "0.00"
            txtFacUpperLimit_Enter(txtFacUpperLimit, New EventArgs())
            Exit Sub
        End If

        txtFacUpperLimit.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=txtFacUpperLimit.Text)

        m_lReturn = CalcSumInsured()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("txtFACLowerLimit_LostFocus", "Failed to calculate SumInsured.", gPMConstants.PMELogLevel.PMLogError)
            Exit Sub
        End If

        m_lReturn = RefreshNetColumnTotal()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("txtFACUpperLimit_LostFocus", "Failed to Refresh column totals in grid.", gPMConstants.PMELogLevel.PMLogError)
            Exit Sub
        End If
        VB6.SetDefault(cmdOK, True)
        SelectGridviewCell()
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub txtFileCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFileCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
            cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
        End If
    End Sub

    Private Sub txtFileCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFileCode.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtFileCode)

        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)

    End Sub

    Private Sub txtShortName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShortName.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtShortName)

        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)

    End Sub

    Private Sub txtShortName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShortName.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
            cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
        End If
    End Sub

    Private Sub txtLongName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLongName.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtLongName)

        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)

    End Sub

    Private Sub txtLongName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLongName.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
            cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
        End If
    End Sub

    Private Sub cmbType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbType.SelectedIndexChanged

        cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)

    End Sub

    Public Function CalcSumInsured() As Integer

        Dim result As Integer = 0
        Dim cSumIns As Decimal
        Dim iRow As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cSumIns = 0
            If grdPlacement.CurrentRowIndex < 0 Then
                Return result
            Else
                iRow = CInt(grdPlacement.CurrentRowIndex + grdPlacement.Rows(0).Index)
            End If

            For lCount As Integer = 0 To grdPlacement.RowsCount - 2
                'grdPlacement.CurrentRowIndex = lCount
                Dim dTempValue As Decimal = 0D
                Decimal.TryParse((gPMFunctions.ToSafeCurrency(txtFacUpperLimit.Text) - gPMFunctions.ToSafeCurrency(txtFacLowerLimit.Text)) * (gPMFunctions.ToSafeDouble(grdPlacement.Rows(lCount).Cells(GridCol_Participation).Value.ToString.Replace("%", "").Trim)), dTempValue)
                If dTempValue = 0D Then
                    grdPlacement.Rows(lCount).Cells(GridCol_SumInsured).Value = "0.00"
                Else
                    grdPlacement.Rows(lCount).Cells(GridCol_SumInsured).Value = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDecimal, vFieldValue:=Decimal.Round(dTempValue, 2).ToString)
                End If
            Next


            'grdPlacement.CurrentRowIndex = iRow

            Return result

        Catch excep As System.Exception

            ''Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed calcSumInsured", vApp:=ACApp, vClass:=ACClass, vMethod:="calcSumInsured", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function RefreshNetColumnTotal() As Integer

        Dim result As Integer = 0
        Dim iRow As Integer
        Dim dTotalPercent, dCommPercent As Double

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            iRow = grdPlacement.CurrentRowIndex
            dTotalPercent = 0


            For ctr As Integer = 0 To m_oXa.GetUpperBound(1)
                Select Case ctr
                    Case GridCol_Commission, GridCol_CommTax, GridCol_Premium, GridCol_SumInsured, GridCol_Tax
                        dTotalPercent = 0
                        For lCount As Integer = 0 To grdPlacement.Rows.Count - 2

                            'grdPlacement.CurrentRowIndex = lCount
                            dTotalPercent += gPMFunctions.ToSafeCurrency(grdPlacement.Rows(lCount).Cells(ctr).Value.ToString.Replace("%", ""))
                        Next
                       
                        If grdPlacement.Columns(ctr).Visible Then
                            If ctr = GridCol_SumInsured Or ctr = GridCol_Premium Then
                                ' grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(ctr).Value = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, _
                                '                                vFieldValue:=Math.Round(dTotalPercent, 0))
                                grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(ctr).Value = dTotalPercent

                            Else
                                'grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(ctr).Value = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(dTotalPercent))
                                grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(ctr).Value = dTotalPercent '78206

                            End If

                        End If

                    Case GridCol_Participation
                        dTotalPercent = 0
                        For lCount As Integer = 0 To grdPlacement.Rows.Count - 2
                            'grdPlacement.CurrentRowIndex = lCount
                            dTotalPercent += gPMFunctions.ToSafeDouble(grdPlacement.Rows(lCount).Cells(ctr).Value.ToString.Replace("%", ""))
                        Next
                        If grdPlacement.Columns(ctr).Visible Then
                            'If m_iTask = PMEComponentAction.PMView Then
                            grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(ctr).Value = dTotalPercent.ToString("P4")
                            'Else
                            '    grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(ctr).Value = (dTotalPercent / 100).ToString("P4")
                            'End If

                        End If
                End Select
            Next ctr

            If grdPlacement.Columns(MainModule.GridCol_Comm).Visible And gPMFunctions.ToSafeCurrency(grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(GridCol_Premium).Value.ToString.Replace("%", "")) <> 0 Then
                dCommPercent = (gPMFunctions.ToSafeCurrency(grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(GridCol_Commission).Value.ToString.Replace("%", "")) / gPMFunctions.ToSafeCurrency(grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(GridCol_Premium).Value.ToString.Replace("%", ""))) * 100
                grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(GridCol_Comm).Value = (dCommPercent / 100).ToString("P4")
            End If

            grdPlacement.Rows(grdPlacement.Rows.Count - 1).DefaultCellStyle.BackColor = SystemColors.ButtonFace
            grdPlacement.Rows(grdPlacement.Rows.Count - 1).ReadOnly = True
            grdPlacement.Rows(grdPlacement.Rows.Count - 1).DefaultCellStyle.Font = New Font(grdPlacement.Font, FontStyle.Bold)

            Return result

        Catch excep As System.Exception


            ''Debugger.Break()

            ' Log Error.
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Refresh Total Percentage ", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshNetColumnTotal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function InterfaceToBusiness() As Integer


        Dim result As Integer = 0
        Dim iLoop1 As Integer
        Dim vPlacementArray(,) As Object, vBrokerArray(,) As Object
        Dim sType As String = ""
        Dim lNewGroupingId As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Create Array

            ReDim vPlacementArray(m_oXa.Rows.Count - 2, m_oXa.GetUpperBound(1))
            For iRow As Integer = 0 To m_oXa.Rows.Count - 2  'm_oXa.GetUpperBound(0)

                'Developer Guide no. 188
                vPlacementArray(iRow, GridCol_ReinsurerCode) = m_oXa(iRow, GridCol_ReinsurerCode)

                'Developer Guide no. 188
                vPlacementArray(iRow, GridCol_Name) = m_oXa(iRow, GridCol_Name)

                'Developer Guide no. 188

                vPlacementArray(iRow, GridCol_AcType) = m_oXa(iRow, GridCol_AcType)

                Dim strTempVal As String = ""
                strTempVal = IIf(gPMFunctions.ToSafeString(m_oXa(iRow, GridCol_Participation)) Is Nothing, CStr(0), gPMFunctions.ToSafeString(m_oXa(iRow, GridCol_Participation)))
                If strTempVal.IndexOf("%") <> -1 Then
                    strTempVal = strTempVal.Replace("%", "").Trim
                    strTempVal = gPMFunctions.ToSafeDouble(strTempVal) / 100
                End If

                vPlacementArray(iRow, GridCol_Participation) = strTempVal

                'Developer Guide no. 188
                vPlacementArray(iRow, GridCol_SumInsured) = m_oXa(iRow, GridCol_SumInsured)


                'Developer Guide no. 188
                vPlacementArray(iRow, GridCol_Premium) = IIf(gPMFunctions.ToSafeString(m_oXa(iRow, GridCol_Premium)) Is Nothing, CStr(0), m_oXa(iRow, GridCol_Premium))


                'Developer Guide no. 188
                vPlacementArray(iRow, GridCol_Tax) = IIf(gPMFunctions.ToSafeString(m_oXa(iRow, GridCol_Tax)) Is Nothing, CStr(0), m_oXa(iRow, GridCol_Tax))


                'Developer Guide no. 188

                Dim strTempVal1 As String = ""
                strTempVal1 = IIf(gPMFunctions.ToSafeString(m_oXa(iRow, GridCol_Comm)) Is Nothing, CStr(0), gPMFunctions.ToSafeString(m_oXa(iRow, GridCol_Comm)))
                strTempVal1 = strTempVal1.Replace("%", "").Trim
                If bIsCommPercentChanged Then
                    vPlacementArray(iRow, GridCol_Comm) = ToSafeDouble(strTempVal1) / 100
                Else
                    vPlacementArray(iRow, GridCol_Comm) = ToSafeDouble(strTempVal1)
                End If



                'Developer Guide no. 188
                vPlacementArray(iRow, GridCol_Commission) = IIf(gPMFunctions.ToSafeString(m_oXa(iRow, GridCol_Commission)) Is Nothing, CStr(0), m_oXa(iRow, GridCol_Commission))


                'Developer Guide no. 188				
                vPlacementArray(iRow, GridCol_CommTax) = IIf(gPMFunctions.ToSafeString(m_oXa(iRow, GridCol_CommTax)) Is Nothing, CStr(0), m_oXa(iRow, GridCol_CommTax))


                'Developer Guide no. 188			
                vPlacementArray(iRow, GridCol_AgreementCode) = IIf(gPMFunctions.ToSafeString(m_oXa(iRow, GridCol_AgreementCode)) Is Nothing, "", m_oXa(iRow, GridCol_AgreementCode))

                'Developer Guide no. 188
                vPlacementArray(iRow, GridCol_PartyCnt) = m_oXa(iRow, GridCol_PartyCnt)

                'Developer Guide no. 188
                vPlacementArray(iRow, GridCol_RILineId) = m_oXa(iRow, GridCol_RILineId)

            Next

            If vParticipantArray.GetUpperBound(0) > -1 Then
                ReDim vBrokerArray(vParticipantArray.GetUpperBound(0), vParticipantArray.GetUpperBound(1))
                For iRow As Integer = 0 To vParticipantArray.GetUpperBound(0)
                    For iCol As Integer = 0 To vParticipantArray.GetUpperBound(1)

                        'Developer Guide no. 188
                        vBrokerArray(iRow, iCol) = vParticipantArray(iRow, iCol)
                    Next
                Next
            End If

            If Information.IsArray(vPlacementArray) Then
                If bIsFAx Then
                    sType = "FAX"
                Else
                    sType = "FAP"
                End If


                If m_iTask = gPMConstants.PMEComponentAction.PMEdit And m_bIsChanged Then

                    m_lReturn = g_oBusiness.UpdatePlacementRILines(RiArrangementId:=m_lRi_Arrangement_Id, vPlacementArray:=vPlacementArray, vParticipantArray:=vBrokerArray, bIsFAx:=bIsFAx, m_cUpperLimit:=gPMFunctions.ToSafeCurrency(txtFacUpperLimit.Text), m_cLowerLimit:=gPMFunctions.ToSafeCurrency(txtFacLowerLimit.Text), m_iProcessId:=m_iProcessId, m_lGroupingId:=m_lGroupingId, ClaimId:=m_lClaim_id, lNewGroupingId:=lNewGroupingId, vAddedFindRIPartyLines:=m_vAddedFindRIPartyLines)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("InterfaceToBusiness", "Failed to Update Lines", gPMConstants.PMELogLevel.PMLogError)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If lNewGroupingId <> 0 And m_lGroupingId = 0 Then
                        m_lGroupingId = lNewGroupingId
                    End If

                End If

                If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                    m_lReturn = g_oBusiness.AddPlacementRILines(RiArrangementId:=m_lRi_Arrangement_Id, vPlacementArray:=vPlacementArray, vParticipantArray:=vBrokerArray, bIsFAx:=bIsFAx, m_cUpperLimit:=gPMFunctions.ToSafeCurrency(txtFacUpperLimit.Text), m_cLowerLimit:=gPMFunctions.ToSafeCurrency(txtFacLowerLimit.Text), m_iProcessId:=m_iProcessId, m_lGroupingId:=m_lGroupingId, ClaimId:=m_lClaim_id, vAddedFindRIPartyLines:=m_vAddedFindRIPartyLines)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("InterfaceToBusiness", "Failed to Add Lines", gPMConstants.PMELogLevel.PMLogError)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If
            Return result

        Catch excep As System.Exception
            ''Debugger.Break()

            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Sub InterfaceToProperties()

        Dim dRetained As Double


        Try


            dRetained = 0
            For iRow As Integer = 0 To grdPlacement.RowsCount - 2

                'grdPlacement.CurrentRowIndex = iRow
                If grdPlacement.Rows(iRow).Cells(GridCol_AcType).Value = "Retained" Then
                    dRetained += gPMFunctions.ToSafeDouble(grdPlacement.Rows(iRow).Cells(GridCol_Participation).Value.ToString.Replace("%", "").Trim)
                End If
            Next
            If grdPlacement.RowsCount > 2 Then
                m_sPartyLongName = ""
                m_sShortName = "Multiple Acts"
                m_lPartyCnt = 0
            ElseIf grdPlacement.RowsCount = 2 Then
                m_sPartyLongName = ""

                m_sShortName = grdPlacement.Rows(0).Cells(GridCol_ReinsurerCode).Value

                m_lPartyCnt = grdPlacement.Rows(0).Cells(GridCol_PartyCnt).Value
            End If

            m_cUpperLimit = gPMFunctions.ToSafeCurrency(txtFacUpperLimit.Text)
            m_cLowerLimit = gPMFunctions.ToSafeCurrency(txtFacLowerLimit.Text)
            m_dRetained_percent = dRetained * 100

            m_dParticipation_percent = gPMFunctions.ToSafeDouble(grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(GridCol_Participation).Value.ToString.Replace("%", "").Trim)

            m_cSumInsured = gPMFunctions.ToSafeCurrency(grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(GridCol_SumInsured).Value)

            'Claim Check
            If m_iProcessId = 1 Then

                m_dComm_percent = gPMFunctions.ToSafeDouble(grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(GridCol_Comm).Value.ToString.Replace("%", ""))

                m_cPremium = gPMFunctions.ToSafeCurrency(grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(GridCol_Premium).Value)

                m_cPremiumTax = gPMFunctions.ToSafeCurrency(grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(GridCol_Tax).Value)

                m_cCommission = gPMFunctions.ToSafeCurrency(grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(GridCol_Commission).Value)

                m_cCommTax = gPMFunctions.ToSafeCurrency(grdPlacement.Rows(grdPlacement.Rows.Count - 1).Cells(GridCol_CommTax).Value)
            Else
                m_dComm_percent = 0
                m_cPremium = 0
                m_cPremiumTax = 0
                m_cCommission = 0
                m_cCommTax = 0
            End If
            'Start - Sankar - PN 50348
            If grdPlacement.RowsCount = 2 Then

                m_sAgreementCode = Convert.ToString(grdPlacement.Rows(grdPlacement.RowsCount - 2).Cells(GridCol_AgreementCode).Value)
            End If
            'End - Sankar - PN 50348

        Catch excep As System.Exception

            ''Debugger.Break()

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set Properties", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub


    Private Function PlacementDetailsMandatory() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_bParticipation = False
            m_bPremium = False

            Dim bisEndorsement As Boolean

            Select Case m_sTransactionType
                Case "MTA", "MTC"
                    bisEndorsement = True
                Case Else
                    bisEndorsement = False
            End Select

            For ctr As Integer = 0 To 10

                Select Case ctr
                    Case GridCol_Participation
                        For lCount As Integer = 0 To grdPlacement.RowsCount - 2

                            'grdPlacement.CurrentRowIndex = lCount
                            If gPMFunctions.ToSafeDouble(grdPlacement.Rows(lCount).Cells(ctr).Value.ToString.Replace("%", "").Trim) <= 0 Then
                                m_bParticipation = True
                                result = gPMConstants.PMEReturnCode.PMFalse
                                Exit For
                            End If
                        Next

                    Case GridCol_Premium

                        For lCount As Integer = 0 To grdPlacement.RowsCount - 2

                            'grdPlacement.CurrentRowIndex = lCount 
                            If grdPlacement.Rows(lCount).Cells(GridCol_AcType).Value <> "Retained" And gPMFunctions.ToSafeCurrency(grdPlacement.Rows(lCount).Cells(GridCol_Premium).Value) <= 0 And Not bisEndorsement Then
                                m_bPremium = True
                                result = gPMConstants.PMEReturnCode.PMFalse
                                Exit For
                            End If
                        Next

                End Select
            Next ctr

            Return result

        Catch excep As System.Exception

            ''Debugger.Break()
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed PlacementDetailsValidation ", vApp:=ACApp, vClass:=ACClass, vMethod:="PlacementDetailsValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function
    Dim OldValue As Object
    Private bIsCommPercentChanged As Boolean
    Private Sub grdPlacement_CellEndEdit(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellEventArgs) Handles grdPlacement.CellEndEdit
        If Not grdPlacement.CurrentCell Is Nothing Then
            If Not grdPlacement.CurrentCell.Equals(grdPlacement.Rows(eventArgs.RowIndex).Cells(eventArgs.ColumnIndex)) Then
                Exit Sub
            End If
        End If
        Dim ColIndex As Integer = eventArgs.ColumnIndex
        Dim Cancel As Integer = 0
        Dim NewValue As String = ""
        Dim lRow As Integer
        Dim sMessage As String = ""
        Dim bisEndorsement As Boolean

        Const kMethodName As String = "grdPlacement_CellEndEdit"

        Try

            Select Case m_sTransactionType
                Case "MTA", "MTC"
                    bisEndorsement = True
                Case Else
                    bisEndorsement = False
            End Select


            NewValue = IIf(gPMFunctions.ToSafeString(grdPlacement.Rows(eventArgs.RowIndex).Cells(eventArgs.ColumnIndex).Value) Is Nothing, "", gPMFunctions.ToSafeString(grdPlacement.Rows(eventArgs.RowIndex).Cells(eventArgs.ColumnIndex).Value))  'EventArgs.Value

            If (OldValue = NewValue) Then
                Exit Sub
            End If

            lRow = CInt(grdPlacement.CurrentRowIndex + grdPlacement.Rows(0).Index)
            NewValue = NewValue.Replace("%", "").Trim
            m_bGridPlacementCheck = False

            Select Case ColIndex
                Case GridCol_Premium

                    If gPMFunctions.ToSafeCurrency(NewValue, 0) = StringsHelper.ToDoubleSafe(NewValue) Then

                        If grdPlacement.CurrentRow().Cells(GridCol_AcType).Value = "Retained" And gPMFunctions.ToSafeCurrency(NewValue) <> 0 Then
                            sMessage = "FAC XOL Premium must be zero when Acct Type is 'Retained'"
                            grdPlacement.Rows(eventArgs.RowIndex).Cells(eventArgs.ColumnIndex).Value = "0.00"
                        ElseIf grdPlacement.CurrentRow().Cells(GridCol_AcType).Value <> "Retained" And gPMFunctions.ToSafeCurrency(NewValue) <= 0 And Not bisEndorsement Then
                            sMessage = "FAC XOL Premium must be greater then zero"
                        End If
                    Else
                        sMessage = "Premium must be a valid currency value"
                    End If

                Case GridCol_Participation

                    NewValue = NewValue.Replace("%", "")
                    Dim dbNumericTemp As Double
                    If Double.TryParse(NewValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        ' Validate percentage is between 0 and 100
                        If (StringsHelper.ToDoubleSafe(NewValue) >= 0) And (StringsHelper.ToDoubleSafe(NewValue) <= 100) Then
                            ' If we use automatic grid formatting then it treats percentages as 0..1 which is a pain to enter so scale them nicely for the user
                            grdPlacement.CurrentRow().Cells(GridCol_Participation).Value = ToSafeDouble(NewValue) / 100 '(CDbl(NewValue) / 100).ToString("P4")
                        Else
                            sMessage = "Participation percentage must be between 0% and 100%"
                        End If
                    Else
                        sMessage = "Participation percentage must be a valid numeric between 0 and 100"
                    End If

                Case GridCol_Comm
                    NewValue = NewValue.Replace("%", "")
                    Dim dbNumericTemp2 As Double
                    If Double.TryParse(NewValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                        ' Validate percentage is between 0 and 100
                        If (StringsHelper.ToDoubleSafe(NewValue) >= 0) And (StringsHelper.ToDoubleSafe(NewValue) <= 100) Then
                            ' If we use automatic grid formatting then it treats percentages as 0..1 which is a pain to enter so scale them nicely for the user
                            grdPlacement.CurrentRow().Cells(GridCol_Comm).Value = ToSafeDouble(NewValue) / 100    '(CDbl(NewValue) / 100).ToString("P4")
                        Else
                            sMessage = "Commission percentage must be between 0% and 100%"
                        End If
                    Else
                        sMessage = "Commission percentage must be a valid numeric between 0 and 100"
                    End If
                Case Else

            End Select

            ' If we have a message then show it
            If sMessage.Length Then
                MessageBox.Show(sMessage, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                grdPlacement.Rows(eventArgs.RowIndex).Cells(eventArgs.ColumnIndex).Value = OldValue
                m_bGridPlacementCheck = True
            End If

            grdPlacement.Refresh()

        Catch ex As Exception

            ''Debugger.Break()
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)



            If Cancel <> 0 Then
                grdPlacement.CancelEdit()
            End If
        End Try



        'Dim ColIndex As Integer = eventArgs.ColumnIndex
        If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
            m_bIsChanged = True
        End If

        Dim cPremiumTax, cCommTax, cPremium, cCommission As Decimal

        If m_bGridPlacementCheck Then Exit Sub

        lRow = CInt(grdPlacement.CurrentRowIndex + grdPlacement.Rows(0).Index)
        NewValue = NewValue.Replace("%", "")


        Select Case ColIndex
            Case GridCol_Premium, GridCol_Comm

                If m_iProcessId = 1 Then

                    If gPMFunctions.ToSafeCurrency(NewValue, 0) = StringsHelper.ToDoubleSafe(NewValue) Then

                        ' Recalculate commission
                        If grdPlacement.CurrentRow().Cells(GridCol_AcType).Value <> "Retained" Then

                            Dim strTempCommPer As String = IIf(gPMFunctions.ToSafeString(grdPlacement.CurrentRow().Cells(GridCol_Comm).Value) Is Nothing, String.Empty, gPMFunctions.ToSafeString(grdPlacement.CurrentRow().Cells(GridCol_Comm).Value))


                            Dim dComm As Decimal = 0D
                            'gPMFunctions.ToSafeDecimal(strTempCommPer.Replace("%", "").Trim) / 100
                            If strTempCommPer.IndexOf("%") > 0 Then
                                dComm = gPMFunctions.ToSafeDecimal(strTempCommPer.Replace("%", "").Trim) / 100
                            Else
                                dComm = gPMFunctions.ToSafeDecimal(strTempCommPer.Trim)
                            End If

                            grdPlacement.CurrentRow().Cells(GridCol_Commission).Value = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDecimal, vFieldValue:=gPMFunctions.ToSafeCurrency(CStr(gPMFunctions.ToSafeCurrency(grdPlacement.CurrentRow().Cells(GridCol_Premium).Value) * dComm), 0))

                            m_lReturn = CalcSumInsured()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Exit Sub
                            End If

                            ' Get new treaty taxes
                            'Developer Guide no. 188
                            cPremium = gPMFunctions.ToSafeCurrency(m_oXa(lRow, GridCol_Premium))
                            'Developer Guide no. 188
                            cCommission = gPMFunctions.ToSafeCurrency(m_oXa(lRow, GridCol_Commission))


                            m_lReturn = m_obSIRReinsurance.CalculateFacTax(v_lArrangementLineID:=0, v_lPartyCnt:=gPMFunctions.ToSafeLong(grdPlacement.CurrentRow().Cells(GridCol_PartyCnt).Value), v_cPremium:=cPremium, v_cCommission:=cCommission, r_cPremiumTax:=cPremiumTax, r_cCommissionTax:=cCommTax)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("CalculateFacTax", "Unable to recalculate facultative taxes")
                                Exit Sub
                            End If
                            ' Store new values

                            'grdPlacement.CurrentRowIndex = lRow
                            grdPlacement.Rows(lRow).Cells(MainModule.GridCol_Tax).Value = cPremiumTax
                            grdPlacement.Rows(lRow).Cells(GridCol_CommTax).Value = Convert.ToString(cCommTax)
                        End If
                    End If
                End If

            Case GridCol_Participation, GridCol_Comm

                NewValue = NewValue.Replace("%", "")
                Dim dbNumericTemp As Double
                If Double.TryParse(NewValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    ' Validate percentage is between 0 and 100
                    If (StringsHelper.ToDoubleSafe(NewValue) >= 0) And (StringsHelper.ToDoubleSafe(NewValue) <= 100) Then
                        ' If we use automatic grid formatting then it treats percentages as 0..1 which is a pain to enter so scale them nicely for the user
                        If ColIndex = GridCol_Participation Then
                            m_lReturn = CalcSumInsured()
                        Else
                            grdPlacement.CurrentRow().Cells(GridCol_Commission).Value = gPMFunctions.ToSafeCurrency(CStr(gPMFunctions.ToSafeCurrency(grdPlacement.CurrentRow().Cells(GridCol_Premium).Value) * gPMFunctions.ToSafeDecimal(grdPlacement.CurrentRow().Cells(GridCol_Comm).Value)), 0)
                        End If
                    End If
                End If
            Case GridCol_Premium
                grdPlacement.CurrentRow().Cells(GridCol_Commission).Value = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDecimal, vFieldValue:=NewValue)
            Case Else
        End Select

        If sMessage.Length Then
            MessageBox.Show(sMessage, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
        'grdPlacement.ReBind

        m_lReturn = RefreshNetColumnTotal()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("RefreshNetColumnTotal", "Unable to Refresh Net Column Totals.")
            Exit Sub
        End If


    End Sub

    Private Sub txtFacLowerLimit_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFacLowerLimit.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
            m_bIsChanged = True
        End If
    End Sub

    Private Sub txtFacUpperLimit_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFacUpperLimit.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
            m_bIsChanged = True
        End If
    End Sub

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub

    Private Sub grdPlacement_CellBeginEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles grdPlacement.CellBeginEdit
        'OldValue = grdPlacement.CurrentRow.Cells(e.ColumnIndex).Value.ToString.Replace("%", "").Trim
        Dim strTemp As String = gPMFunctions.ToSafeString(grdPlacement.CurrentRow.Cells(e.ColumnIndex).Value)
        If Not String.IsNullOrEmpty(strTemp) Then
            OldValue = strTemp.Replace("%", "").Trim
        End If
    End Sub
    Private Sub grdPlacement_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles grdPlacement.CellFormatting
        If Not e.Value Is DBNull.Value Then
            Dim dTempVal As Double = 0D
            If e.ColumnIndex = GridCol_Comm Then

            End If

            Select Case e.ColumnIndex
                Case GridCol_Comm
                    If (Double.TryParse(e.Value.ToString.Replace("%", ""), dTempVal)) Then
                        If e.RowIndex = grdPlacement.Rows.Count - 1 Then
                            Exit Sub
                        End If
                        'If bIsCommPercentChanged Then
                        '    e.Value = (dTempVal / 100).ToString("P4")
                        'Else
                        e.Value = dTempVal.ToString("P4")
                        'End If
                    End If
                Case GridCol_Participation
                    If e.RowIndex = grdPlacement.Rows.Count - 1 Then
                        Exit Sub
                    End If
                    If (Double.TryParse(e.Value, dTempVal)) Then
                        e.Value = dTempVal.ToString("P4")
                    End If
                Case GridCol_SumInsured, GridCol_Premium, GridCol_Commission
                    Dim dVal As Decimal = 0D
                    Decimal.TryParse(e.Value, dVal)
                    If dVal = 0D Then
                        e.Value = "0.00"
                    Else
                        e.Value = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDecimal, vFieldValue:=Decimal.Round(dVal, 2).ToString)
                    End If
            End Select
        End If
    End Sub

    Private Sub SelectGridviewCell()
        RemoveHandler txtFacUpperLimit.Leave, AddressOf txtFacUpperLimit_Leave
        If grdPlacement.RowCount > 1 Then
            grdPlacement.Rows(grdPlacement.RowCount - 2).Cells(GridCol_Participation).Selected = True
        End If
        AddHandler txtFacUpperLimit.Leave, AddressOf txtFacUpperLimit_Leave
    End Sub
End Class
