Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no 129. 
Imports SharedFiles

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form


#Region "Private Constants"
    Private Const ACClass As String = "frmInterface"
    'developer guide no. 7
    Private Const vbFormCode As Integer = 0
    Private Const kBusinessTypeIdCoinsFollow As Integer = 4
#End Region



#Region "Private Variables"
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lInsuranceFileCnt As CheckState
    Private m_lInsuranceFolderCnt As Integer
    Private m_sInsReference As String = ""
    Private m_lPartyCnt As Integer
    Private m_cPercAllocated As Decimal
    Private m_lItemsFound As Integer
    'Variables to store data taken from the List View
    Private m_iAction As gPMConstants.PMEComponentAction
    Private m_lInsCnt As Integer
    Private m_sClientName As String = ""
    Private m_sArrangementRef As String = ""
    Private m_cSharePercentage As Decimal
    Private m_cCommissionPercentage As Decimal


    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUCoinsurance.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    'Private m_oBusiness As bSIRCoinsurance.Business

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    ' Stores the search data from the business object.
    Private m_vCOIArrangement(,) As Object
    Private m_vCOIDefault(,) As Object
    Private m_vCOIValue(,) As Object

    Private m_bEvent As Boolean
    Private m_nBusinessTypeId As Integer
#End Region


#Region "Public Properties"
    Public WriteOnly Property BusinessTypeId() As Integer
        Set(ByVal value As Integer)
            m_nBusinessTypeId = value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            m_lNavigate = Value
        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            m_lProcessMode = Value
        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property
    ' {* USER DEFINED CODE (Begin) *}
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public WriteOnly Property InsuranceFolderCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    Public WriteOnly Property InsReference() As String
        Set(ByVal Value As String)
            m_sInsReference = Value
        End Set
    End Property

    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public Property FromEvent() As Boolean
        Get
            Return m_bEvent
        End Get
        Set(ByVal Value As Boolean)
            m_bEvent = Value
        End Set
    End Property
#End Region

#Region "Public Methods"
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the controls to
            ' PMFormControl
            '
            ' Example:-
            '
            '        ' Pass control and required settings to FormControl
            '        m_lReturn = m_oFormFields.AddNewFormField( _
            ''                       ctlControl:=<Control Name>, _
            ''                       lFieldType:=<PM field type>, _
            ''                       lFormat:=<PM format string>, _
            ''                       lMandatory:=<PMNonMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFormatPercent, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtArrangementRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSharePercentage, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCommissionPercentage, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DataToInterface) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DataToInterface() As Integer
    '
    'Dim result As Integer = 0
    'Dim oListItem As ListViewItem
    'Dim bMatch As Boolean
    'Dim cThisPerc As Decimal
    '
    'Const ACFindImage As String = "FindImage"
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Update the interface details.
    '
    ' Clear the search details.
    'lvwCoinsurance.Items.Clear()
    '
    '
    'm_lItemsFound = 0
    'm_cPercAllocated = 0
    '

    'pnlInsuranceFileRef.Caption = m_sInsReference
    '
    'm_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFormatPercent, vControlValue:=m_cPercAllocated)
    '

    'pnlPercAllocated.Caption = txtFormatPercent.Text
    '
    'If Not Information.IsArray(m_vCOIValue) Then
    'Return result
    'End If
    '
    ' Assign the details to the interface.
    '
    'For 'lRow As Integer = m_vCOIValue.GetLowerBound(1) To m_vCOIValue.GetUpperBound(1)
    'If CDbl(m_vCOIValue(ACVPartyCnt, lRow)) <> 0 Then
    ' {* USER DEFINED CODE (Begin) *}
    'm_lItemsFound += 1
    ' Assign the details to the first column.
    ' Column 1 Reference

    'oListItem = lvwCoinsurance.Items.Add(CStr(m_vCOIValue(ACVPartyName, lRow)).Trim(), "")
    '
    ' Assign details to the other columns
    '
    ' Column 2 ref
    '
    'ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vCOIValue(ACVArrangementRef, lRow))
    '
    'Column 3 share %
    '
    'm_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFormatPercent, vControlValue:=m_vCOIValue(ACVSharePercent, lRow))
    '
    'ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtFormatPercent.Text
    '
    'm_cPercAllocated += CDec(CStr(m_vCOIValue(ACVSharePercent, lRow)).Trim())
    '
    ' Column 4 commission %
    'm_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFormatPercent, vControlValue:=m_vCOIValue(ACVCommissionPercent, lRow))
    '
    'ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtFormatPercent.Text
    ' {* USER DEFINED CODE (End) *}
    '
    ' Set the tag property with the index of
    ' the search data storage.
    'oListItem.Tag = CStr(lRow)
    '
    ' Refresh the first X amount of rows, to
    ' allow the user to see the results instantly.
    'If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
    ' Select the first item.
    'lvwCoinsurance.Items.Item(0).Selected = True
    '
    ' Refresh the initial results.
    'lvwCoinsurance.Refresh()
    'End If
    'End If
    'Next lRow
    '
    'm_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFormatPercent, vControlValue:=m_cPercAllocated)
    '

    'pnlPercAllocated.Caption = txtFormatPercent.Text
    '
    ' Enable the interface now that the search
    ' has completed.
    'm_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)
    '
    ' Check for errors
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to get details.
    'result = gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'cmdDelete.Enabled = False
    '
    'cmdAdd.Enabled = Not (Task = gPMConstants.PMEComponentAction.PMView)
    '
    'cmdEdit.Enabled = False
    'cmdOK.Enabled = True
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result


    '
    'Return result
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: DataToDetail
    '
    ' Description: Populate Co-Insurer Details
    '              storage.
    '
    ' ***************************************************************** '
    Private Function DataToDetail() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the Detail details.

            If lvwCoinsurance.SelectedItems.Count > 0 Then
                lSelectedItem = Convert.ToString(lvwCoinsurance.Items.Item(lvwCoinsurance.SelectedItems(0).Index).Tag)
            Else
                lSelectedItem = Convert.ToString(lvwCoinsurance.Items.Item(lvwCoinsurance.Items(lvwCoinsurance.Items.Count - 1).Index).Tag)
            End If



            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}

            m_lInsCnt = CInt(m_vCOIValue(ACVPartyCnt, lSelectedItem))
            m_sClientName = CStr(m_vCOIValue(ACVPartyName, lSelectedItem)).Trim()

            m_cSharePercentage = CDec(m_vCOIValue(ACVSharePercent, lSelectedItem))

            m_cCommissionPercentage = CDec(m_vCOIValue(ACVCommissionPercent, lSelectedItem))


            'developer guide no. 51
            lblClient.Text = m_sClientName

            txtArrangementRef.Text = CStr(m_vCOIValue(ACVArrangementRef, lSelectedItem))

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSharePercentage, vControlValue:=m_cSharePercentage)

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCommissionPercentage, vControlValue:=m_cCommissionPercentage)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Detail details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearDetail
    '
    ' Description: Clear Co-Insurer Details
    '              storage.
    '
    ' ***************************************************************** '
    Private Function ClearDetail() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the Detail details.

            ' {* USER DEFINED CODE (Begin) *}
            m_lInsCnt = 0
            m_sClientName = ""
            m_sArrangementRef = ""
            m_cSharePercentage = 0
            m_cCommissionPercentage = 0

            'AJM (17/05/2001) Only default the remaining value after initial allocation
            If (m_cPercAllocated > 0) And (m_cPercAllocated <> 100) Then
                'If (m_cPercAllocated < 100) Then
                m_cSharePercentage = 100 - m_cPercAllocated
            End If


            'developer guide no. 51
            lblClient.Text = m_sClientName

            txtArrangementRef.Text = m_sArrangementRef

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSharePercentage, vControlValue:=m_cSharePercentage)

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCommissionPercentage, vControlValue:=m_cCommissionPercentage)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Detail details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataRefresh
    '
    ' Description: Populate Co-Insurer Refreshs
    '              storage.
    '
    ' ***************************************************************** '
    Private Function DataRefresh() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}

            If txtSharePercentage.Text = "" Then
                m_cSharePercentage = 0
                '    Else
                '        m_cSharePercentage = m_oFormFields.UnformatControl(txtSharePercentage)
            End If

            Select Case m_iAction
                Case gPMConstants.PMEComponentAction.PMAdd
                    If Information.IsArray(m_vCOIValue) Then
                        lSelectedItem = m_vCOIValue.GetUpperBound(1) + 1
                        ReDim Preserve m_vCOIValue(ACVPartyName, lSelectedItem)
                    Else
                        lSelectedItem = 0
                        ReDim m_vCOIValue(ACVPartyName, lSelectedItem)
                    End If

                Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete

                    'lSelectedItem = Convert.ToString(lvwCoinsurance.Items.Item(lvwCoinsurance.FocusedItem.Index).Tag)
                    If lvwCoinsurance.SelectedItems.Count > 0 Then
                        lSelectedItem = Convert.ToString(lvwCoinsurance.Items.Item(lvwCoinsurance.SelectedItems(0).Index).Tag)
                    Else
                        lSelectedItem = Convert.ToString(lvwCoinsurance.Items.Item(lvwCoinsurance.Items(lvwCoinsurance.Items.Count - 1).Index).Tag)
                    End If
            End Select

            If m_iAction <> gPMConstants.PMEComponentAction.PMDelete Then ' Sankar - PN 69363 - Added the if condition
                m_vCOIValue(ACVPartyCnt, lSelectedItem) = m_lInsCnt
                m_vCOIValue(ACVPartyName, lSelectedItem) = m_sClientName
                m_vCOIValue(ACVArrangementRef, lSelectedItem) = txtArrangementRef.Text

                m_cSharePercentage = gPMMaths.PMRoundupValueCurrency(v_vWholeValue:=m_cSharePercentage, v_eNumberOfDP:=gPMConstants.PMECurrencyNoOfDP.pmeCurDPTwo, v_eRoundingFactor:=gPMConstants.PMERoundupFactor.pmeRFactor50Up)

                m_vCOIValue(ACVSharePercent, lSelectedItem) = m_cSharePercentage

                m_cCommissionPercentage = gPMMaths.PMRoundupValueCurrency(v_vWholeValue:=m_cCommissionPercentage, v_eNumberOfDP:=gPMConstants.PMECurrencyNoOfDP.pmeCurDPTwo, v_eRoundingFactor:=gPMConstants.PMERoundupFactor.pmeRFactor50Up)

                m_vCOIValue(ACVCommissionPercent, lSelectedItem) = m_cCommissionPercentage

                m_vCOIValue(ACVSharePremium, lSelectedItem) = 0
                m_vCOIValue(ACVCommissionValue, lSelectedItem) = 0
                m_vCOIValue(ACVSurchargePercent, lSelectedItem) = 0
                m_vCOIValue(ACVSurchargeValue, lSelectedItem) = 0
                m_vCOIValue(ACVIsStandardSurcharge, lSelectedItem) = 0
                m_vCOIValue(ACVPremiumTaxRecoveryPercent, lSelectedItem) = 0
                m_vCOIValue(ACVPremiumTaxRecoveryValue, lSelectedItem) = 0
                m_vCOIValue(ACVIsManualPremiumTax, lSelectedItem) = 0
            End If ' Sankar - PN 69363
            m_lReturn = CType(BusinessToInterface(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Refresh Refreshs from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataRefresh", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidatePolicyShare
    '
    ' Description: Populate Co-Insurer Refreshs
    '              storage.
    '
    ' ***************************************************************** '
    Private Function ValidatePolicyShare() As Integer

        Dim result As Integer = 0
        Dim cRate As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Validate Details.

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}

            'developer guide no. 51
            If lblClient.Text.Trim() = "" Then
                'Sankar - PN 69363
                MessageBox.Show("Please Enter Valid Client", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            'TN20001120 - its now nonMandatory
            '    If (Trim$(txtArrangementRef.Text) = "") Then
            '        MsgBox "Please Enter Arrangement Reference"
            '        ValidatePolicyShare = PMFalse
            '    End If

            If txtSharePercentage.Text = "" Then
                'Sankar - PN 69363
                MessageBox.Show("Please Enter Share Percentage", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            cRate = CDec(m_oFormFields.UnformatControl(txtSharePercentage))

            If cRate > 100 Then
                'Sankar - PN 69363
                MessageBox.Show("Share cannot be greater than 100%", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If cRate <= 0 Then
                'Sankar - PN 69363
                MessageBox.Show("Share cannot be less or equal to 0%", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set the values, can't rely on the events


            'developer guide no. 51
            m_sClientName = m_sClientName$ 'lblClient.Text
            m_sArrangementRef = txtArrangementRef.Text


            m_cSharePercentage = CDec(m_oFormFields.UnformatControl(ctlControl:=txtSharePercentage))


            m_cCommissionPercentage = CDec(m_oFormFields.UnformatControl(ctlControl:=txtCommissionPercentage))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Refresh Refreshs from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidatePolicyShare", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' ***************************************************************** '
    Private Function SelectParty(ByRef vPartyCnt As Object, ByRef vShortName As Object, Optional ByRef vName As Object = Nothing, Optional ByRef vSpecialParty As String = "") As Integer
        Dim result As Integer = 0

        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    Set oFindParty = CreateObject("iPMBFindParty.Interface")
            Dim temp_oFindParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oFindParty.CallingAppName = "PolicyControl"


            m_lErrorNumber = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=GIIConstants.GIIPMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set appropriate key if agent only


            'Developer guide no 143. 
            If Not (vSpecialParty Is Nothing) Then
                If Not (CStr(vSpecialParty).ToString().Trim() = "") Then
                    'If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then

                    ReDim vKeyArray(1, 0)

                    vKeyArray(0, 0) = "special_party"

                    vKeyArray(1, 0) = vSpecialParty


                    m_lErrorNumber = oFindParty.SetKeys(vKeyArray)

                    If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                        oFindParty = Nothing
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If


            m_lErrorNumber = oFindParty.Start()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then



                vPartyCnt = oFindParty.PartyCnt


                vShortName = oFindParty.ShortName


                'If Not Information.IsNothing(vName) Then


                '	vName = oFindParty.LongName
                'End If
                vName = oFindParty.LongName
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            oFindParty.Dispose()

            oFindParty = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

#End Region


    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}

            'Tell it what we're getting

            m_oBusiness.FromEvent = FromEvent


            m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt


            m_lReturn = m_oBusiness.GetCoinsurance(r_vCOIArrangement:=m_vCOIArrangement, r_vCOIValue:=m_vCOIValue, r_vCOIDefault:=m_vCOIDefault)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Static bAlreadyRun As Boolean

        Const ACFindImage As String = "FindImage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_cPercAllocated = 0

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign the all of the interface
            ' details from the business object, using the FormatField
            ' function for any type conversion.
            '
            ' Example:-
            '
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            'developer guide no. 51
            lblInsuranceFile.Text = m_sInsReference

            If Not bAlreadyRun Then
                If Information.IsArray(m_vCOIArrangement) Then
                    chkIsRecovered.CheckState = CInt(m_vCOIArrangement(ACAIsRecovered, 0))
                    chkIsSurcharged.CheckState = CInt(m_vCOIArrangement(ACAIsSurcharged, 0))

                    cboCOIDefault.DefaultItemId = CInt(m_vCOIArrangement(ACACOIDefaultId, 0))

                    If Information.IsArray(m_vCOIDefault) Then
                        For lTemp As Integer = m_vCOIDefault.GetLowerBound(1) To m_vCOIDefault.GetUpperBound(1)

                            If m_vCOIDefault(ACDCOIDefaultId, lTemp).Equals(m_vCOIArrangement(ACACOIDefaultId, 0)) Then

                                chkIsRecovered.Enabled = (CDbl(m_vCOIDefault(ACDIsRecoveredOverrideable, lTemp)) = 1)

                                chkIsSurcharged.Enabled = (CDbl(m_vCOIDefault(ACDIsSurchargedOverrideable, lTemp)) = 1)

                                Exit For
                            End If

                        Next lTemp
                    End If

                End If

                cboCOIDefault.RefreshList()

            End If

            lvwCoinsurance.Items.Clear()

            If Information.IsArray(m_vCOIValue) Then
                For lTemp As Integer = m_vCOIValue.GetLowerBound(1) To m_vCOIValue.GetUpperBound(1)
                    If CDbl(m_vCOIValue(ACVPartyCnt, lTemp)) <> 0 Then

                        oListItem = lvwCoinsurance.Items.Add(CStr(m_vCOIValue(ACVPartyName, lTemp)), "")

                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vCOIValue(ACVArrangementRef, lTemp))

                        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFormatPercent, vControlValue:=m_vCOIValue(ACVSharePercent, lTemp))

                        m_cPercAllocated += CDbl(m_vCOIValue(ACVSharePercent, lTemp))

                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtFormatPercent.Text

                        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFormatPercent, vControlValue:=m_vCOIValue(ACVCommissionPercent, lTemp))

                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtFormatPercent.Text

                        oListItem.Tag = CStr(lTemp)
                        oListItem.StateImageIndex = ImageList1.Images.Count - 1
                    End If
                Next lTemp

            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFormatPercent, vControlValue:=m_cPercAllocated)


            ' developer guide no. 51
            lblPercAllocated.Text = txtFormatPercent.Text

            bAlreadyRun = True

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim lBusinessDataID As Integer
        Dim bDifferent As Boolean
        Dim sDescription As String = ""
        Dim vDescription As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt


            m_lReturn = m_oBusiness.Update(v_vCOIArrangement:=m_vCOIArrangement, v_vCOIValue:=m_vCOIValue)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try


            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Dim lTemp As Integer
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(m_vCOIArrangement) Then
                ReDim m_vCOIArrangement(ACACOIDefaultId, 0)
            End If

            m_vCOIArrangement(ACAInsuranceFileCnt, 0) = m_lInsuranceFileCnt
            m_vCOIArrangement(ACAIsRecovered, 0) = chkIsRecovered.CheckState
            m_vCOIArrangement(ACAIsSurcharged, 0) = chkIsSurcharged.CheckState
            m_vCOIArrangement(ACACOIDefaultId, 0) = cboCOIDefault.ItemId

            '    If (lvwCoinsurance.ListItems.Count = 0) Then
            '        m_vCOIValue = ""
            '    Else
            '        ReDim m_vCOIValue(ACVIsManualPremiumTax, lvwCoinsurance.ListItems.Count - 1)
            '    End If
            '
            '    For lTemp = 0 To lvwCoinsurance.ListItems.Count - 1
            '        Set oListItem = lvwCoinsurance.ListItems(lTemp + 1)
            '
            '        m_vCOIValue(ACVInsuranceFileCnt, lTemp) = m_lInsuranceFileCnt
            '        m_vCOIValue(ACVCOIValueId, lTemp) = lTemp + 1
            '        m_vCOIValue(ACVPartyCnt, lTemp) = oListItem.Tag
            '        m_vCOIValue(ACVArrangementRef, lTemp) = oListItem.SubItems(1)
            '
            '        txtFormatPercent = oListItem.SubItems(2)
            '        m_vCOIValue(ACVSharePercent, lTemp) = m_oFormFields.UnformatControl(ctlControl:=txtFormatPercent)
            '
            '        m_vCOIValue(ACVSharePremium, lTemp) = 0
            '        m_vCOIValue(ACVCommissionPercent, lTemp) = 0
            '        m_vCOIValue(ACVCommissionValue, lTemp) = 0
            '        m_vCOIValue(ACVSurchargePercent, lTemp) = 0
            '        m_vCOIValue(ACVSurchargeValue, lTemp) = 0
            '        m_vCOIValue(ACVIsStandardSurcharge, lTemp) = 0
            '        m_vCOIValue(ACVPremiumTaxRecoveryPercent, lTemp) = 0
            '        m_vCOIValue(ACVPremiumTaxRecoveryValue, lTemp) = 0
            '        m_vCOIValue(ACVIsManualPremiumTax, lTemp) = 0
            '
            '    Next lTemp

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)
            'Display the ListView Tab

            tabDetailTab.Visible = False
            tabDetailTab.Top = VB6.TwipsToPixelsY(120)
            tabMainTab.Top = VB6.TwipsToPixelsY(120)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwCoinsurance.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the column widths for the search list.
            lvwCoinsurance.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(3000))
            lvwCoinsurance.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(1500))
            lvwCoinsurance.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwCoinsurance.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(1500))

            cmdEdit.Enabled = False

            cmdDelete.Enabled = False

            cmdAdd.Enabled = Not (Task = gPMConstants.PMEComponentAction.PMView)

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

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

            ' Display all language specific captions.


            'developer guide no 243. 
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            'developer guide no 243. 
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabDetailTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = ipmFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString, bResFile:=My.Resources.ResourceManager)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************



            lblInsuranceFileRef.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCInsuranceFile, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            chkIsRecovered.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCIsRecoverd, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            chkIsSurcharged.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCIsSurcharged, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAllocatedPerc.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCAllocatedPercent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCOIDefault.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCDefault, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwCoinsurance.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHCoinsurer, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwCoinsurance.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHArrangementRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwCoinsurance.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHSharePercent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwCoinsurance.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHCommissionPercent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdInsurerLookup.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLCoinsurer, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblArrangementRef.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLArrangementRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSharePercentage.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLSharePercent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCommissionPercentage.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLCommissionPercent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

    Private Sub cboCOIDefault_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCOIDefault.Click


        If Information.IsArray(m_vCOIDefault) Then
            For lTemp As Integer = m_vCOIDefault.GetLowerBound(1) To m_vCOIDefault.GetUpperBound(1)

                If CInt(m_vCOIDefault(ACDCOIDefaultId, lTemp)) = cboCOIDefault.ItemId Then

                    chkIsRecovered.CheckState = CInt(m_vCOIDefault(ACDIsRecovered, lTemp))

                    chkIsRecovered.Enabled = (CDbl(m_vCOIDefault(ACDIsRecoveredOverrideable, lTemp)) = 1)

                    chkIsSurcharged.CheckState = CInt(m_vCOIDefault(ACDIsSurcharged, lTemp))

                    chkIsSurcharged.Enabled = (CDbl(m_vCOIDefault(ACDIsSurchargedOverrideable, lTemp)) = 1)

                    Exit For
                End If

            Next lTemp
        End If

    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        m_iAction = gPMConstants.PMEComponentAction.PMAdd

        cmdOK.Enabled = False
        cmdCancel.Enabled = False

        tabMainTab.Visible = False
        tabDetailTab.Top = VB6.TwipsToPixelsY(120)
        tabDetailTab.Visible = True

        m_lReturn = CType(ClearDetail(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'developer guide no. 20
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdInsurerLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdInsurerLookup.Click

        'developer guide no. Change as per vb code
        Dim vCnt As Object
        Dim vShortName, vName As Object
        Dim sTemp As String = ""

        Try

            m_lReturn = CType(SelectParty(vPartyCnt:=vCnt, vShortName:=vShortName, vName:=vName, vSpecialParty:="IN"), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Ensure that the client doesn't already exist
            If Information.IsArray(m_vCOIValue) Then
                For lRow As Integer = m_vCOIValue.GetLowerBound(1) To m_vCOIValue.GetUpperBound(1)
                    If CStr(m_vCOIValue(ACVPartyCnt, lRow)) = vCnt Then
                        'Sankar - PN 69363
                        MessageBox.Show("This Insurer already has a share", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If
                Next lRow
            End If

            'save the count in the tag and update controls

            pnlClient.Tag = vCnt

            m_lInsCnt = CInt(vCnt)


            m_sClientName = CStr(vName)
            sTemp = m_sClientName

            m_lReturn = CType(PMBGeneralFunc.DoubleCharacter(r_sString:=sTemp, v_sChar:="&"), gPMConstants.PMEReturnCode)


            'developer guide no. 51
            lblClient.Text = sTemp

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdInsurerLookup_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click


        m_iAction = gPMConstants.PMEComponentAction.PMDelete


        Dim lSelectedItem As Integer
        If lvwCoinsurance.SelectedItems.Count > 0 Then
            lSelectedItem = Convert.ToString(lvwCoinsurance.Items.Item(lvwCoinsurance.SelectedItems(0).Index).Tag)
        Else
            lSelectedItem = Convert.ToString(lvwCoinsurance.Items.Item(lvwCoinsurance.Items(lvwCoinsurance.Items.Count - 1).Index).Tag)
        End If
        m_vCOIValue(ACVPartyCnt, lSelectedItem) = 0

        cmdCancel.Enabled = True
        cmdAdd.Enabled = True
        cmdOK.Enabled = True

        m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)

        'Start PN-72057 (Sushil Kumar)
        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If lvwCoinsurance.Items.Count = 0 Then
                cmdDelete.Enabled = False
                cmdEdit.Enabled = False
            Else
                cmdDelete.Enabled = True
                cmdEdit.Enabled = True
            End If
        End If
        'Start PN-72057 (Sushil Kumar)

    End Sub

    Private Sub cmdDetailCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailCancel.Click

        tabDetailTab.Visible = False
        tabMainTab.Visible = True
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            cmdOK.Enabled = True
        End If

    End Sub

    Private Sub cmdDetailOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailOK.Click

        m_lReturn = CType(ValidatePolicyShare(), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        tabDetailTab.Visible = False
        tabMainTab.Visible = True
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True
        cmdOK.Enabled = True

        m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)



    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        m_iAction = gPMConstants.PMEComponentAction.PMEdit

        cmdOK.Enabled = False
        cmdCancel.Enabled = False

        tabMainTab.Visible = False
        tabDetailTab.Visible = True

        m_lReturn = CType(DataToDetail(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRCoinsurance.Business", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                'developer guide no 243. 
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMUCoinsurance.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub



        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            m_oBusiness.InsuranceFolderCnt = m_lInsuranceFolderCnt

            m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt

            m_oBusiness.FromEvent = m_bEvent
            ' {* USER DEFINED CODE (End) *}

            ' Validate fields using Forms Control
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

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

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




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
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'developer guide no. 7
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

            ' Terminate the business object

            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Terminate the form control object.
            m_oFormFields.Dispose()
            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub
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

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdOK.Enabled = Not bDisable
            'cmdEdit.Enabled = Not bDisable

            Return result

        Catch excep As System.Exception



            ' Error Section.

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
    'UPGRADE_NOTE: (7001) The following declaration (DisplayStatusSearching) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub DisplayStatusSearching()
    '
    'Static sMessage As String = ""
    '
    'Try 
    '
    ' Get message text if not already present.
    'If sMessage = "" Then

    'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    'End If
    '
    ' Display the status message.
    '    stbStatus.SimpleText = " " & sMessage$
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisplayStatusFound) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub DisplayStatusFound()
    '
    'Static sMessage As String = ""
    'Dim lItemsFound As Integer
    '
    'Try 
    '
    '
    ' Get message text if not already present.
    'If sMessage = "" Then

    'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    'End If
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    '
    'End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabDetailTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabDetailTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabDetailTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabDetailTab, SSTabHelper.GetSelectedIndex(tabDetailTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabDetailTab, SSTabHelper.GetTabCount(tabDetailTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabDetailTab) < (SSTabHelper.GetTabCount(tabDetailTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabDetailTab, SSTabHelper.GetSelectedIndex(tabDetailTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                        End If
                End Select
            End With
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    ''' <summary>
    ''' cmdOK_Click
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim sMsg As String = ""
        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_cPercAllocated = gPMMaths.PMRoundupValueCurrency(v_vWholeValue:=m_cPercAllocated, _
                                                               v_eNumberOfDP:= _
                                                                  gPMConstants.PMECurrencyNoOfDP.pmeCurDPTwo, _
                                                               v_eRoundingFactor:= _
                                                                  gPMConstants.PMERoundupFactor.pmeRFactor50Up)

            'AJM 17/05/2001 - Give message box a title
            If m_cPercAllocated > 100 Then
                MessageBox.Show("Policy over allocated, please amend", "Over Allocation")
                Exit Sub
            End If

            'RWH(19/10/2000) We do not need to test for less than 100%.
            'AJM(17/05/2001) Reinstate test for 100%
            If m_cPercAllocated < 100 Then
                'AJM 17/05/2001 - Give message box a title
                MessageBox.Show("Policy under allocated, please amend", "Under Allocation")
                Exit Sub
            End If


            'Validate coinsuer to check retainer in list

            Dim iCoinCnt As Integer = 0
            Dim iIsRetainer As Integer = 0
            Dim iRetainerCnt As Integer = 0

            If Information.IsArray(m_vCOIValue) Then
                For iCoinCnt = 0 To m_vCOIValue.GetUpperBound(1)
                    Dim iInsCnt As Integer = CInt(m_vCOIValue(ACVPartyCnt, iCoinCnt))

                    m_lReturn = m_oBusiness.GetRetainFlag(v_iPartyCnt:=iInsCnt, r_iIsRetainer:=iIsRetainer)
                    ' Check the return value.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If iIsRetainer = 1 Then
                        iRetainerCnt += 1
                    End If
                Next
            End If

            If iRetainerCnt = 0 Then
                MessageBox.Show(" A Retained entry must be included in the Co-insurer list.", "Invalid Retainer")
                Exit Sub
            ElseIf iRetainerCnt > 1 Then
                MessageBox.Show(" Adding more than one co-insurer which is linked to your retained account is not allowed.", "Restricted")
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass,vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cmdNavigate_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdNavigate_Click()
    '
    ' Click event of the Navigate button.
    '
    'Try 
    '
    ' Set the interface status.
    'm_lStatus = gPMConstants.PMEReturnCode.PMNavigate
    '
    ' Process the next set of actions depending
    ' upon the interface task etc.
    'm_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
    '
    ' Check the return value.
    'If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
    ' Everything OK, so we can hide the interface.
    'Me.Hide()
    'End If
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    ' PRIVATE Events (End)

    Private Sub lvwCoinsurance_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwCoinsurance.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If lvwCoinsurance.GetItemAt(x, y) Is Nothing Then
                cmdDelete.Enabled = False
                cmdEdit.Enabled = False
            Else
                cmdDelete.Enabled = True
                cmdEdit.Enabled = True
            End If
        End If

    End Sub

    Private Sub txtArrangementRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtArrangementRef.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtArrangementRef)

    End Sub

    Private Sub txtArrangementRef_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtArrangementRef.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtArrangementRef)

    End Sub

    Private Sub txtCommissionPercentage_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCommissionPercentage.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCommissionPercentage)

    End Sub

    Private Sub txtCommissionPercentage_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCommissionPercentage.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCommissionPercentage)

    End Sub

    Private Sub txtSharePercentage_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSharePercentage.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtSharePercentage)

    End Sub

    Private Sub txtSharePercentage_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSharePercentage.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtSharePercentage)

    End Sub
End Class
