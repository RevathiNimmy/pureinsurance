Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Modified by Archana Tokas on 5/3/2010 2:38:40 PM refer developer guide no. 129
Imports SharedFiles
Imports Artinsoft.VB6.Utils
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 09/06/1999
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
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
    Private m_lPartyCnt As Integer
    Private m_lItemsFound As Integer
    Private m_vAssociates(,) As Object
    Private m_vSearchData(,) As Object
    Private m_vRelationships(,) As Object
    Private m_vSaveSearchData(,) As Object
    'Variables to store data taken from the List View
    Private m_iAction As gPMConstants.PMEComponentAction
    Private m_lAssociatePartyCnt As Integer
    Private lPartyCnt As Integer
    Private m_sAssociatePartyShortname As String = ""
    Private m_sAssociatePartyName As String = ""
    Private m_lRelationshipTypeId As Integer
    Private m_sRelationshipTypeDesc As String = ""
    Private m_oCurrencyConvert As bACTCurrencyConvert.Form

    ' PW260702
    Private m_sPartyType As String = ""
    Private m_iSelectedIndex As Integer = -1
    Dim PrevWidth As Integer = 464
    Dim PrevHeight As Integer = 269
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBAssociates.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Stores party agent type information.
    Private m_sPartyAgentType As String = ""

    ' Stores information if commission transfer is true
    Private m_bCommissionTransfer As Boolean

    ' Stores information for underwriter or agent.
    Private m_sUnderwritingOrAgency As String = ""

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

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

    ' PW260702
    Public WriteOnly Property PartyType() As String
        Set(ByVal Value As String)

            m_sPartyType = Value

        End Set
    End Property


    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
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

    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property


    Public Property SearchData() As Object
        Get

            Return VB6.CopyArray(m_vSearchData)

        End Get
        Set(ByVal Value As Object)

            m_vSearchData = Value

            m_vSaveSearchData = VB6.CopyArray(m_vSearchData)

        End Set
    End Property

    'NIIT - Replaced with the Migrated code 1144 
    'Public WriteOnly Property Relationships() As Object()
    '    Set(ByVal Value() As Object)

    '        m_vRelationships = Value

    '    End Set
    'End Property
    Public WriteOnly Property Relationships() As Object(,)
        Set(ByVal Value As Object(,))

            m_vRelationships = Value

        End Set
    End Property
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
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
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=pnlClientLookup, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Reference must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboRelationships, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetUpRelationships
    '
    ' Description: Set Up Relationships
    '
    ' ***************************************************************** '
    Private Function SetUpRelationships() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Relationships List
            cboRelationships.Items.Clear()
            cboRelationships.SelectedIndex = -1

            ' Check if there is a list
            If Not Information.IsArray(m_vRelationships) Then
                Return result
            End If

            ' Assign the details to the interface.
            For lRow As Integer = m_vRelationships.GetLowerBound(1) To m_vRelationships.GetUpperBound(1)

                Dim cboRelationships_NewIndex As Integer = -1
                cboRelationships_NewIndex = cboRelationships.Items.Add(CStr(m_vRelationships(1, lRow)))
                VB6.SetItemData(cboRelationships, cboRelationships_NewIndex, CInt(m_vRelationships(0, lRow)))


                If m_vRelationships(1, lRow).Equals(m_sRelationshipTypeDesc) And cboRelationships.SelectedIndex = -1 Then
                    cboRelationships.Text = CStr(m_vRelationships(1, lRow))
                    cboRelationships.SelectedIndex = lRow
                End If

            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set up Relationships", vApp:=ACApp, vClass:=ACClass, vMethod:="SetUpRelationships", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim oPartyBusiness As bSIRFindParty.Business
        Dim sFormattedCurrency As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oObjectManager.GetInstance(oPartyBusiness, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            Dim temp_m_oCurrencyConvert As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oCurrencyConvert = temp_m_oCurrencyConvert

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTCurrencyConvert", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            ' Update the interface details.

            ' Clear the search details.
            lvwAssociates.Items.Clear()

            m_lItemsFound = 0

            ' Check that search details are valid before
            ' continuing.

            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If

            ' Assign the details to the interface.
            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                Dim dtAccountBalance As New DataTable
                Dim dtClaimIncurred As New DataTable
                'Don't show Deleted item
                If CStr(m_vSearchData(ACIAssociatePartyShortname, lRow)) <> "" Then

                    ' {* USER DEFINED CODE (Begin) *}

                    m_lItemsFound += 1

                    ' Assign the details to the first column.
                    ' Column 1 Associate Code

                    oListItem = lvwAssociates.Items.Add(CStr(m_vSearchData(ACIAssociatePartyShortname, lRow)).Trim())

                    ' Assign details to the other columns

                    ' Column 2 Associate Name

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vSearchData(ACIAssociatePartyName, lRow)).Trim()

                    ' Column 3 Relationship

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vSearchData(ACIRelationshipTypeDesc, lRow)).Trim()


                    ' {* USER DEFINED CODE (End) *}

                    ' Set the tag property with the index of
                    ' the search data storage.
                    oListItem.Tag = CStr(lRow)

                    ' Refresh the first X amount of rows, to
                    ' allow the user to see the results instantly.
                    If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                        ' Select the first item.
                        If m_iSelectedIndex <= 0 Then lvwAssociates.Items.Item(0).Selected = True

                        ' Refresh the initial results.
                        lvwAssociates.Refresh()
                    End If


                    ' Get account balance
                    m_lReturn = CType(oPartyBusiness.GetAccountBalance(lPartyCnt:=m_vSearchData(ACIAssociatePartyCnt, lRow), dtResult:=dtAccountBalance), gPMConstants.PMEReturnCode)
                    If dtAccountBalance IsNot Nothing AndAlso dtAccountBalance.Rows.Count > 0 Then
                        m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=CInt(dtAccountBalance.Rows(0).Item("currency_id")), vCurrencyAmount:=CStr(dtAccountBalance.Rows(0).Item("SumAmount")).Trim(), vFormattedCurrency:=sFormattedCurrency)
                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = sFormattedCurrency
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Error has occured
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get account balance.", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface")

                        Return result
                    End If

                    ' Get Claim Incurred
                    m_lReturn = CType(oPartyBusiness.GetClaimIncurred(lPartyCnt:=m_vSearchData(ACIAssociatePartyCnt, lRow), dtResult:=dtClaimIncurred), gPMConstants.PMEReturnCode)

                    If dtClaimIncurred IsNot Nothing AndAlso dtClaimIncurred.Rows.Count > 0 Then
                        m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=CInt(dtAccountBalance.Rows(0).Item("currency_id")), vCurrencyAmount:=CStr(dtClaimIncurred.Rows(0).Item("Claim_Incurred")).Trim(), vFormattedCurrency:=sFormattedCurrency)
                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = sFormattedCurrency
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Error has occured
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get claim incurred.", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface")

                        Return result
                    End If
                End If
            Next lRow

            ' Enable the interface now that the search
            ' has completed.

            m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            If m_iSelectedIndex > 0 AndAlso lvwAssociates.Items.Count - 1 >= m_iSelectedIndex Then
                'lvwSearchDetails.SelectedItems.Clear()
                'lvwSearchDetails.FocusedItem = lvwSearchDetails.Items(m_iSelectedIndex)
                lvwAssociates.Items(m_iSelectedIndex).EnsureVisible()
                lvwAssociates.Items(m_iSelectedIndex).Selected = True
                lvwAssociates.Items(m_iSelectedIndex).Focused = True
            End If

            cmdDelete.Enabled = False
            cmdAdd.Enabled = Not (Task = gPMConstants.PMEComponentAction.PMView)
            cmdEdit.Enabled = False
            cmdOK.Enabled = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToDetail
    '
    ' Description: Populate Associate Details
    '              storage.
    '
    ' ***************************************************************** '
    Private Function DataToDetail() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer

        Dim sPartyAgentType As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the Detail details.


            lSelectedItem = Convert.ToString(lvwAssociates.Items.Item(lvwAssociates.FocusedItem.Index).Tag)

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}

            m_lAssociatePartyCnt = CInt(m_vSearchData(ACIAssociatePartyCnt, lSelectedItem))
            m_sAssociatePartyShortname = CStr(m_vSearchData(ACIAssociatePartyShortname, lSelectedItem))
            m_sAssociatePartyName = CStr(m_vSearchData(ACIAssociatePartyName, lSelectedItem))
            m_lRelationshipTypeId = CInt(m_vSearchData(ACIRelationshipTypeId, lSelectedItem))
            m_sRelationshipTypeDesc = CStr(m_vSearchData(ACIRelationshipTypeDesc, lSelectedItem))
            m_bCommissionTransfer = gPMFunctions.ToSafeBoolean(m_vSearchData(ACIAssociateCommTransf, lSelectedItem))

            'Modified by Archana Tokas on 5/3/2010 2:39:51 PM refer developer guide no. 51
            'pnlClientLookup.Caption = m_sAssociatePartyName
            'Modified by milan.rawat on 6/10/2010 8:39:33 PM refer developer guide no. 26 (latest guide)
            'pnlClientLookup.Name = m_sAssociatePartyName
            lblClientLookup.Text = m_sAssociatePartyName

            ' Get sub agent type information
            m_lReturn = CType(GetPartyAgentType(v_lPartyCnt:=m_lAssociatePartyCnt, r_sPartyAgentType:=sPartyAgentType), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Error has occured
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get party agent type information.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdClientLookup_Click")

                Return result
            End If

            ' Set chkCommissionTransaction property and value.
            If m_sPartyAgentType <> "Sub-Agent" And sPartyAgentType = "Sub-Agent" Then
                If m_bCommissionTransfer Then
                    chkCommissionTransaction.CheckState = CheckState.Checked
                Else
                    chkCommissionTransaction.CheckState = CheckState.Unchecked
                End If

                chkCommissionTransaction.Enabled = True
            Else
                chkCommissionTransaction.CheckState = CheckState.Unchecked
                chkCommissionTransaction.Enabled = False
            End If



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Detail details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ClearDetail
    '
    ' Description: Clear Associate Details
    '              storage.
    '
    ' ***************************************************************** '
    Private Function ClearDetail() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the Detail details.

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}

            m_lAssociatePartyCnt = 0
            m_sAssociatePartyShortname = ""
            m_sAssociatePartyName = ""
            m_lRelationshipTypeId = 0
            m_sRelationshipTypeDesc = ""


            'Modified by Archana Tokas on 5/3/2010 2:42:46 PM refer developer guide no. 51
            'pnlClientLookup.Caption = " " & gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatStringCase, vFieldValue:=m_sAssociatePartyName)
            'Modified by milan.rawat on 6/10/2010 8:39:33 PM refer developer guide no. 26 (latest guide)
            'pnlClientLookup.Name = " " & gPMFunctions.FormatFieldiFormatType:=(gPMConstants.PMEFormatStyle.PMFormatStringCase,vFieldValue:= m_sAssociatePartyName)
            lblClientLookup.Text = " " & gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatStringCase, vFieldValue:=m_sAssociatePartyName)

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
    ' Description: Populate Associate Refreshs
    '              storage.
    '
    ' ***************************************************************** '
    Private Function DataRefresh() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the Refresh Refreshs.

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}


            Select Case m_iAction
                Case gPMConstants.PMEComponentAction.PMAdd

                    If Not Information.IsArray(m_vSearchData) Then
                        ReDim m_vSearchData(6, 0)
                        lSelectedItem = 0
                    Else
                        lSelectedItem = m_vSearchData.GetUpperBound(1) + 1
                        ReDim Preserve m_vSearchData(6, lSelectedItem)
                    End If

                    m_vSearchData(ACIAssociatePartyCnt, lSelectedItem) = CStr(m_lAssociatePartyCnt)
                    m_vSearchData(ACIAssociatePartyShortname, lSelectedItem) = m_sAssociatePartyShortname
                    m_vSearchData(ACIAssociatePartyName, lSelectedItem) = m_sAssociatePartyName
                    m_vSearchData(ACIRelationshipTypeId, lSelectedItem) = CStr(m_lRelationshipTypeId)
                    m_vSearchData(ACIRelationshipTypeDesc, lSelectedItem) = m_sRelationshipTypeDesc
                    m_vSearchData(ACIAssociateCommTransf, lSelectedItem) = gPMFunctions.ToSafeBoolean(chkCommissionTransaction.Checked)

                Case gPMConstants.PMEComponentAction.PMEdit


                    lSelectedItem = Convert.ToString(lvwAssociates.Items.Item(lvwAssociates.FocusedItem.Index).Tag)
                    m_vSearchData(ACIAssociatePartyCnt, lSelectedItem) = CStr(m_lAssociatePartyCnt)
                    m_vSearchData(ACIAssociatePartyShortname, lSelectedItem) = m_sAssociatePartyShortname
                    m_vSearchData(ACIAssociatePartyName, lSelectedItem) = m_sAssociatePartyName
                    m_vSearchData(ACIRelationshipTypeId, lSelectedItem) = CStr(m_lRelationshipTypeId)
                    m_vSearchData(ACIRelationshipTypeDesc, lSelectedItem) = m_sRelationshipTypeDesc
                    m_vSearchData(ACIAssociateCommTransf, lSelectedItem) = gPMFunctions.ToSafeBoolean(chkCommissionTransaction.Checked)

            End Select

            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Refresh Refreshs from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataRefresh", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateAssociate
    '
    ' Description: Populate Associate Refreshs
    '              storage.
    '
    ' ***************************************************************** '

    Private Function ValidateAssociate() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Validate Details.

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}


            'Modified by Archana Tokas on 5/3/2010 2:43:11 PM refer developer guide no. 51
            'If pnlClientLookup.Caption = " " Then

            'Modified by milan.rawat on 6/10/2010 8:39:33 PM refer developer guide no. 26 (latest guide)
            'If pnlClientLookup.Name = " " Then
            If lblClientLookup.Text = " " Then
                MessageBox.Show("Please Enter Valid Agent", "Agent", MessageBoxButtons.OK)

                '    Else
                '
                '        If cboRelationships.Text = "" Then
                '
                '            MsgBox "Please Enter Valid Relationship", vbOKOnly, "Agent"
                '            ValidateAssociate = PMFalse
                '
                '        Else
                '
                '        End If
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.CheckMandatoryControls()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Refresh Refreshs from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateAssociate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' ***************************************************************** '

    'Modified by milan.rawat on 6/2/2010 10:34:41 AM refer developer guide no. 17(latest guide)
    'Private Function SelectParty(ByRef vPartyCnt As Integer, ByRef vShortName As String, Optional ByRef vName As String = "", Optional ByRef vSpecialParty As String = "", Optional ByRef sAgentType As String = "") As Integer
    Private Function SelectParty(ByRef vPartyCnt As Integer, ByRef vShortName As Object, Optional ByRef vName As Object = Nothing, Optional ByRef vSpecialParty As Object = Nothing, Optional ByRef sAgentType As Object = Nothing) As Integer

        Dim result As Integer = 0
        'Modified by milan.rawat on 6/2/2010 10:34:41 AM refer developer guide no. 108(latest guide)
        'Dim oFindParty As iPMBFindParty.Interface
        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Modified by milan.rawat on 6/2/2010 10:34:41 AM refer developer guide no. 108(latest guide)
            'oFindParty = New iPMBFindParty.Interface()
            oFindParty = New iPMBFindParty.Interface_Renamed()

            'Set appropriate key if agent only


            If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then

                ReDim vKeyArray(1, 0)

                vKeyArray(0, 0) = "special_party"

                vKeyArray(1, 0) = vSpecialParty

                m_lErrorNumber = oFindParty.SetKeys(vKeyArray)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    oFindParty = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lErrorNumber = CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.CallingAppName = "PolicyControl"
            'eck 0901003 PN7334
            m_lErrorNumber = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'MKW PN11249 Ignore DPA Questions for Policy Shares.
            oFindParty.IgnoreDPAQuestions = True

            m_lErrorNumber = oFindParty.Start()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then

                vPartyCnt = oFindParty.PartyCnt
                vShortName = oFindParty.ShortName

                'Modified by milan.rawat on 6/9/2010 10:36:31 PM refer developer guide no. 143(latest guide)
                vName = oFindParty.LongName
                If Not Information.IsNothing(vName) Then
                    vName = oFindParty.LongName
                    'Modified by milan.rawat on 6/9/2010 10:36:27 PM refer developer guide no. 229(latest guide)
                Else
                    vName = ""
                End If
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
    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetBusiness) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetBusiness() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    '
    ' Get the details from the business object.
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    'DC Not Required
    '    m_lReturn& = m_oBusiness.GetDetails()
    ''
    '    ' {* USER DEFINED CODE (End) *}
    ''
    '    ' Check for errors
    '    If (m_lReturn& <> PMTrue) Then
    '        ' Failed to get details.
    '        GetBusiness = PMFalse
    ''
    '        ' Log Error.
    '        LogMessage _
    ''            iType:=PMLogError, _
    ''            sMsg:="Failed to get details from the business object", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetBusiness"
    ''
    '        Exit Function
    '    End If
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to retreive all of the lookup
            ' descriptions for a given lookup type.
            ' The GetLookupDetails function will allow you to do this.
            '
            ' Example:-
            '
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=PMLookupCodeName, _
            ''        ctlLookup:=cmbCodeName)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            'DC Not Required
            '    m_lReturn& = m_oBusiness.GetNext()
            '
            '    ' {* USER DEFINED CODE (End) *}
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        BusinessToData = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to retreive the details from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="BusinessToData"
            '    End If
            '
            '    Exit Function

        Catch
        End Try




        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer
        Dim result As Integer = 0
        Dim lRow2 As Integer
        Dim bFirst As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            lRow2 = 0
            bFirst = True
            'eck 091003 PN7334
            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If

            'Go thru Associate List to new Associate details
            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                If CStr(m_vSearchData(ACIAssociatePartyShortname, lRow)) = "" Then
                Else
                    If bFirst Then
                        ReDim m_vAssociates(5, lRow2)
                        bFirst = False
                    Else
                        lRow2 += 1
                        ReDim Preserve m_vAssociates(5, lRow2)
                    End If

                    m_vAssociates(ACIAssociatePartyCnt, lRow2) = m_vSearchData(ACIAssociatePartyCnt, lRow)
                    m_vAssociates(ACIAssociatePartyShortname, lRow2) = m_vSearchData(ACIAssociatePartyShortname, lRow)
                    m_vAssociates(ACIAssociatePartyName, lRow2) = m_vSearchData(ACIAssociatePartyName, lRow)
                    m_vAssociates(ACIRelationshipTypeId, lRow2) = m_vSearchData(ACIRelationshipTypeId, lRow)
                    m_vAssociates(ACIRelationshipTypeDesc, lRow2) = m_vSearchData(ACIRelationshipTypeDesc, lRow)
                    m_vAssociates(ACIAssociateCommTransf, lRow2) = m_vSearchData(ACIAssociateCommTransf, lRow)
                End If

            Next lRow

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            tabDetailTab.Top = VB6.TwipsToPixelsY(120000000)
            tabAssociates.Top = VB6.TwipsToPixelsY(120)
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
            ' Set the column widths for the search list.
            lvwAssociates.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1500))
            lvwAssociates.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(1500))
            lvwAssociates.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(1500))

            cmdEdit.Enabled = False

            cmdDelete.Enabled = False

            cmdAdd.Enabled = Not (Task = gPMConstants.PMEComponentAction.PMView)

            lvwAssociates.FullRowSelect = True

            ' Get information about underwriter or agent.
            m_lReturn = CType(iPMFunc.getUnderwritingOrAgency(r_vUnderwriting:=m_sUnderwritingOrAgency), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Error has occured
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get underwriter or agent information.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")

                Return result
            End If

            If m_lPartyCnt <> 0 Then
                m_lReturn = CType(GetPartyAgentType(v_lPartyCnt:=m_lPartyCnt, r_sPartyAgentType:=m_sPartyAgentType), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get party agent type information.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")

                    Return result
                End If
            End If

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
            ' PW260702 - Set caption depending on party type
            If m_sPartyType <> ACPartyAgent Then

                'Modified by milan.rawat on 6/2/2010 11:20:50 AM refer developer guide no.  76(latest guide)
                'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else

                'Modified by milan.rawat on 6/2/2010 11:20:50 AM refer developer guide no.  76(latest guide)
                'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitleAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitleAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            'Modified by milan.rawat on 6/2/2010 11:20:50 AM refer developer guide no.  76(latest guide)
            'cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Modified by milan.rawat on 6/2/2010 11:20:50 AM refer developer guide no.  76(latest guide)
            'cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Modified by milan.rawat on 6/2/2010 11:20:50 AM refer developer guide no.  76(latest guide)
            'cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Modified by milan.rawat on 6/2/2010 11:20:50 AM refer developer guide no.  76(latest guide)
            'SSTabHelper.SetTabCaption(tabAssociates, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabAssociates, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Modified by milan.rawat on 6/2/2010 11:20:50 AM refer developer guide no.  76(latest guide)
            'SSTabHelper.SetTabCaption(tabDetailTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabDetailTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Modified by milan.rawat on 6/2/2010 11:20:50 AM refer developer guide no.  76(latest guide)
            'chkCommissionTransaction.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCommTrans, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            chkCommissionTransaction.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCommTrans, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            ' PW260702 - set search button to agents if appropriate
            If m_sPartyType = ACPartyAgent Then

                'Modified by milan.rawat on 6/2/2010 11:20:50 AM refer developer guide no.  76(latest guide)
                'cmdClientLookup.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSearchButtonAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                cmdClientLookup.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSearchButtonAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try


            ' Gets all of the lookup values.

            'DC Not Required
            '    ' Check the task.
            '    Select Case (m_iTask)
            '        Case PMAdd
            '            ' Get all of the lookup values.
            '            m_lReturn& = m_oBusiness.GetLookupValues( _
            ''                iLookupType:=PMLookupAll, _
            ''                vTableArray:=m_vLookupValues, _
            ''                iLanguageID:=g_iLanguageID%, _
            ''                vResultArray:=m_vLookupDetails)
            '
            '        Case PMEdit
            '            ' Get all of the lookup values with the correct
            '            ' effective date.
            '            m_lReturn& = m_oBusiness.GetLookupValues( _
            ''                iLookupType:=PMLookupAllEffective, _
            ''                vTableArray:=m_vLookupValues, _
            ''                iLanguageID:=g_iLanguageID%, _
            ''                vResultArray:=m_vLookupDetails)
            '
            '        Case PMView
            '            ' Get lookup values for viewing only.
            '            m_lReturn& = m_oBusiness.GetLookupValues( _
            ''                iLookupType:=PMLookupSingle, _
            ''                vTableArray:=m_vLookupValues, _
            ''                iLanguageID:=g_iLanguageID%, _
            ''                vResultArray:=m_vLookupDetails)
            '    End Select
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        GetLookupValues = PMFalse
            '
            '        ' Log Error.
            '        LogMessagePopup _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to get the lookup values from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetLookupValues"
            '
            '        Exit Function
            '    End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRow As Integer
    'Dim bFoundMatch As Boolean
    '
    ' Lookup value contants.
    'Const ACValueTableName As Integer = 0
    'Const ACValueID As Integer = 1
    'Const ACValueStartPos As Integer = 2
    'Const ACValueNumber As Integer = 3
    '
    ' Lookup detail contants.
    'Const ACDetailKey As Integer = 0
    'Const ACDetailDesc As Integer = 1
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the lookup values.
    '
    'bFoundMatch = False
    '
    'For 'lRow = m_vLookupValues.GetLowerBound(2) To m_vLookupValues.GetUpperBound(2)
    ' Check for a match of the table name.
    'If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
    ' Found a match
    'bFoundMatch = True
    'Exit For
    'End If
    'Next lRow
    '
    ' Check if there has been a table match.
    'If Not bFoundMatch Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
    '
    'Return result
    'End If
    '
    ' Using the lookup values, populate the control with
    ' the details from the lookup details array.
    '
    'For 'lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
    '
    ' Add the details to the control.

    'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


    'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
    '
    ' Compare long value not string
    ' Check if this is the selected index.
    'If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
    'If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


    'ctlLookup.ListIndex = ctlLookup.NewIndex
    'End If
    'End If
    '
    'Next lCntr
    '
    ' Check if the selected index is blank. If so,
    ' we set the controls index to zero.
    'If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

    'ctlLookup.ListIndex = 0
    'End If
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' PRIVATE Methods (End)

    Private Sub cboRelationships_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRelationships.SelectedIndexChanged

        Try

            If cboRelationships.SelectedIndex <> -1 Then

                '        m_lRelationshipTypeId = CLng(cboRelationships.ListIndex) + 1
                m_lRelationshipTypeId = VB6.GetItemData(cboRelationships, cboRelationships.SelectedIndex)
                m_sRelationshipTypeDesc = cboRelationships.Text

            Else

                m_lRelationshipTypeId = 0
                m_sRelationshipTypeDesc = ""

            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cboRelationships_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        m_iAction = gPMConstants.PMEComponentAction.PMAdd
        cmdOK.Enabled = False
        cmdCancel.Enabled = False
        tabAssociates.Top = VB6.TwipsToPixelsY(120000000)
        tabDetailTab.Top = VB6.TwipsToPixelsY(120)
        chkCommissionTransaction.CheckState = CheckState.Unchecked
        chkCommissionTransaction.Enabled = False

        ' Not required if not underwriting.

        m_oFormFields.Item("pnlClientLookup-0").IsMandatory = True
        m_oFormFields.Item("cboRelationships-0").IsMandatory = True



        tabDetailTab.Visible = True
        tabAssociates.Visible = False
        m_lReturn = CType(ClearDetail(), gPMConstants.PMEReturnCode)
        m_lReturn = CType(SetUpRelationships(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdClientLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClientLookup.Click

        Dim vCnt As Integer
        Dim vShortName, vName As Object
        'Modified by milan.rawat on 6/9/2010 2:44:22 PM refer developer guide no. 101(latest guide)
        'Dim sTemp, sPartyAgentType As String
        Dim sTemp, sPartyAgentType As Object

        Try

            ' PW260702 - pass party type


            'm_lReturn = CType(SelectParty(vPartyCnt:=vCnt, vShortName:=CStr(vShortName), vName:=CStr(vName), vSpecialParty:=m_sPartyType), gPMConstants.PMEReturnCode)
            m_lReturn = CType(SelectParty(vPartyCnt:=vCnt, vShortName:=vShortName, vName:=vName, vSpecialParty:=m_sPartyType), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If vCnt = m_lPartyCnt Then

                MessageBox.Show("Agent cannot be associated with itself", "Agent", MessageBoxButtons.OK)
                Exit Sub

            End If

            'Ensure that the associate doesn't already exist
            If Information.IsArray(m_vSearchData) Then

                For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                    If CStr(m_vSearchData(ACIAssociatePartyCnt, lRow)) = CStr(vCnt) And CStr(m_vSearchData(ACIAssociatePartyShortname, lRow)) <> "" Then

                        MessageBox.Show("Associate already exists", Application.ProductName)
                        Exit Sub

                    End If

                Next lRow

            End If

            'save the count in the tag and update controls
            pnlClientLookup.Tag = CStr(vCnt)

            m_lAssociatePartyCnt = vCnt


            m_sAssociatePartyName = CStr(vName)

            m_sAssociatePartyShortname = CStr(vShortName)
            sTemp = m_sAssociatePartyName
            m_lReturn = CType(PMBGeneralFunc.DoubleCharacter(r_sString:=sTemp, v_sChar:="&"), gPMConstants.PMEReturnCode)

            'Modified by Archana Tokas on 5/3/2010 2:43:11 PM refer developer guide no. 51
            'pnlClientLookup.Caption = "" & gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatStringCase, vFieldValue:=sTemp)

            'Modified by milan.rawat on 6/10/2010 8:09:44 PM refer developer guide no. 26(latest guide)
            'pnlClientLookup.Name = "" & gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatStringCase, vFieldValue:=sTemp)
            lblClientLookup.Text = "" & gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatStringCase, vFieldValue:=sTemp)


            m_lReturn = CType(GetPartyAgentType(v_lPartyCnt:=vCnt, r_sPartyAgentType:=sPartyAgentType), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get party agent type information.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdClientLookup_Click")

                Exit Sub
            End If

            If m_sPartyAgentType <> "Sub-Agent" And sPartyAgentType = "Sub-Agent" Then
                chkCommissionTransaction.Enabled = True
            Else
                chkCommissionTransaction.CheckState = CheckState.Unchecked
                chkCommissionTransaction.Enabled = False
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdClientLookup_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try



    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click


        m_iAction = gPMConstants.PMEComponentAction.PMDelete

        Dim lSelectedItem As Integer = Convert.ToString(lvwAssociates.Items.Item(lvwAssociates.FocusedItem.Index).Tag)
        m_vSearchData(ACIAssociatePartyShortname, lSelectedItem) = ""
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True
        cmdOK.Enabled = True
        m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdDetailCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailCancel.Click

        tabDetailTab.Top = VB6.TwipsToPixelsY(120000000)
        tabAssociates.Top = VB6.TwipsToPixelsY(120)
        tabAssociates.Visible = True
        tabDetailTab.Visible = False
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            cmdOK.Enabled = True
        End If

    End Sub

    Private Sub cmdDetailOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailOK.Click

        m_lReturn = CType(ValidateAssociate(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If
        tabDetailTab.Top = VB6.TwipsToPixelsY(120000000)
        tabAssociates.Top = VB6.TwipsToPixelsY(120)
        tabAssociates.Visible = True
        tabDetailTab.Visible = False
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True
        cmdOK.Enabled = True
        m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        m_iAction = gPMConstants.PMEComponentAction.PMEdit
        cmdOK.Enabled = False
        cmdCancel.Enabled = False
        tabAssociates.Top = VB6.TwipsToPixelsY(120000000)
        tabDetailTab.Top = VB6.TwipsToPixelsY(120)
        tabDetailTab.Visible = True
        tabAssociates.Visible = False
        m_lReturn = CType(DataToDetail(), gPMConstants.PMEReturnCode)
        m_lReturn = CType(SetUpRelationships(), gPMConstants.PMEReturnCode)

    End Sub


    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBAssociates.General()


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
        PrevHeight = Me.Height
        PrevWidth = Me.Width
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

            'DC Not Required
            '    ' Set the process modes for the busines object.
            '    m_lReturn& = m_oBusiness.SetProcessModes( _
            ''        vTask:=CVar(m_iTask%), _
            ''        vNavigate:=CVar(m_lNavigate&), _
            ''        vProcessMode:=CVar(m_lProcessMode&), _
            ''        vTransactionType:=CVar(m_sTransactionType$), _
            ''        vEffectiveDate:=CVar(m_dtEffectiveDate))
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Failed to process the interface.
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Log Error Message
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to set the process modes for the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="Form_Load"
            '
            '        Exit Sub
            '    End If
            '
            '    ' Set the business keys.
            '    ' {* USER DEFINED CODE (Begin) *}
            '    m_oBusiness.PartyCnt = m_lPartyCnt
            '    ' {* USER DEFINED CODE (End) *}
            '
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

            'DC Not Required
            '    ' Gets the interface details to be displayed.
            '    m_lReturn& = m_oGeneral.GetInterfaceDetails()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Failed to get the interface details.
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Set the mouse pointer to normal.
            '        SetMousePointer PMMouseNormal
            '
            '        Exit Sub
            '    End If

            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)

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
                    eventArgs.cancel = True
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

            'DC Not Required
            '    ' Terminate the business object
            '    m_lReturn& = m_oBusiness.Terminate()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to terminate the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="Form_QueryUnload"
            '    End If
            '
            '    ' Destroy the instance of the business object
            '    ' from memory.
            '    Set m_oBusiness = Nothing

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
            'developer guide no.293

            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabAssociates.SelectedIndex = 0
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim Msg As String = ""
        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                m_vSearchData = VB6.CopyArray(m_vSaveSearchData)

                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwAssociates_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwAssociates.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwAssociates.Columns(e.Column)

        Static lastColumn As Integer
        Static lastOrder As SortOrder
        Dim iOrder As SortOrder

        Try

            If ListViewHelper.GetSortOrderProperty(lvwAssociates) = SortOrder.Ascending Then
                iOrder = SortOrder.Descending
            Else
                iOrder = SortOrder.Ascending
            End If

            ListViewHelper.SetSortedProperty(lvwAssociates, True)

            Select Case (Convert.ToString(ColumnHeader.Tag))
                Case "DateColumn"
                    ' Sort by date
                    ListViewFunc.ListViewSortByDate(lvwAssociates, ColumnHeader.Index + 1 - 1, iOrder)
                Case "Currency"
                    ' Sort by currency
                    ListViewFunc.ListViewSortByValue(lvwAssociates, ColumnHeader.Index + 1 - 1, iOrder)
                Case Else
                    ' Default sort
                    ListViewHelper.SetSortedProperty(lvwAssociates, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwAssociates, iOrder)

                    ListViewHelper.SetSortKeyProperty(lvwAssociates, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwAssociates, True)
                    iOrder = ListViewHelper.GetSortKeyProperty(lvwAssociates)
            End Select

            lastColumn = ColumnHeader.Index + 1
            lastOrder = iOrder

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwRenewalProcess_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Exit Sub

        End Try

    End Sub
    Private Sub lvwAssociates_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwAssociates.SelectedIndexChanged
        If Not (lvwAssociates.SelectedItems Is Nothing) Then
            If lvwAssociates.SelectedItems.Count > 0 Then
                m_iSelectedIndex = lvwAssociates.SelectedItems(0).Index
            End If
        End If
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

    Private Sub lvwAssociates_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAssociates.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Modified by milan.rawat on 6/10/2010 10:04:48 PM refer developer guide no. 70(Latest Guide)
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If lvwAssociates.GetItemAt(x, y) Is Nothing Then
                cmdDelete.Enabled = False
                cmdAdd.Enabled = True
                cmdEdit.Enabled = False
            Else
                cmdDelete.Enabled = True
                cmdAdd.Enabled = True
                cmdEdit.Enabled = True
            End If
        End If

    End Sub

    ' ***************************************************************** '
    ' Name: GetPartyAgentType
    '
    ' Description: Gets party agent type using the partyCnt values.
    ' ***************************************************************** '
    Private Function GetPartyAgentType(ByVal v_lPartyCnt As Integer, ByRef r_sPartyAgentType As String) As Integer
        Dim result As Integer = 0
        Dim bSIRFindParty As Object

        Const kMethodName As String = "GetPartyAgentType"


        Dim oBusiness As bSIRFindParty.Business

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.RaiseError(kMethodName, "Failed to get an instance of the business object", gPMConstants.PMELogLevel.PMLogError)

                Return result
            End If

            ' Get agent party type information from the business object.

            m_lReturn = oBusiness.GetPartyAgentType(v_lPartyCnt:=v_lPartyCnt, r_sPartyAgentType:=r_sPartyAgentType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Failed to get agent party type.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.RaiseError(kMethodName, "Failed to get agent party type.", gPMConstants.PMELogLevel.PMLogError)

                Return result
            End If



        Catch ex As Exception

            ' Do not call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


        Finally

            If Not (oBusiness Is Nothing) Then
                ' Terminate the business object

                oBusiness.Dispose()
                ' Destroy the instance of the business object from memory.
                oBusiness = Nothing
            End If


        End Try
        Return result
    End Function

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        'Dim fSize As New SizeF((Me.Width / PrevWidth), (Me.Height / PrevHeight))
        'tabAssociates.Scale(fSize)
        'cmdOK.Scale(fSize)
        'cmdCancel.Scale(fSize)
        'cmdHelp.Scale(fSize)
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            m_lReturn = ResizeInterface()

        Catch
            Exit Sub
        End Try
    End Sub
    Private Function ResizeInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            tabAssociates.Width = Me.Width - VB6.TwipsToPixelsX(360)
            tabAssociates.Height = Me.Height - VB6.TwipsToPixelsY(1890)
            lvwAssociates.Width = Me.Width - VB6.TwipsToPixelsX(360)
            lvwAssociates.Height = Me.Height - VB6.TwipsToPixelsY(2640)


            '    cmdNew.Top = Me.Height - 1110
            cmdEdit.Top = Me.Height - VB6.TwipsToPixelsY(1605) ' changed 1395
            '''    cmdEdit.Top = Me.Height - 1110
            cmdAdd.Top = Me.Height - VB6.TwipsToPixelsY(1605) ' changed 1395
            '''    cmdNavigate.Top = Me.Height - 1110
            cmdDelete.Top = Me.Height - VB6.TwipsToPixelsY(1605) ' ch

            cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(1335)
            '    cmdHelp.Top = Me.Height - 1110
            cmdHelp.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) - 1395 + 330) ' changed 1395

            cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(2535)
            '    cmdCancel.Top = Me.Height - 1110
            cmdCancel.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) - 1395 + 330) ' changed 1395

            cmdOK.Left = Me.Width - VB6.TwipsToPixelsX(3735)
            '   cmdOK.Top = Me.Height - 1110
            cmdOK.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) - 1395 + 330) ' changed 1395

            Return result

        Catch
            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Private Sub frmInterface_ResizeBegin(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeBegin
        PrevWidth = Me.Width
        PrevHeight = Me.Height
    End Sub
End Class
