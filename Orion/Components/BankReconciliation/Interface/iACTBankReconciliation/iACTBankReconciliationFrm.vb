Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    '*******************************************************************************
    ' Form Name: frmInterface
    '
    ' Description: Main interface.
    '
    ' History: 020999 - Created
    '
    '          PW141204 - PN17440 - Replicate changes made for PN14670 in Broking
    '                     1.8.6. Also included a couple of other minor changes that
    '                     had been made to the 1.8.6 code.
    ' RKS - 28/01/2005 - PN17668
    '*******************************************************************************


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    'developer guide no. 7
    Private Const vbFormCode As Integer = 0

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    Private m_iTask As Integer
    Private m_bCancelled As Boolean

    ' {* USER DEFINED CODE (Begin) *}


    ' What solution are we running as part of?
    Private m_iSolutionConfig As Integer

    ' Instance of Find Transaction

    Private m_oFindTransaction As iACTFindTransaction.Interface_Renamed
    'eck030500
    Private m_lAccountId As Integer
    Private m_vPaymentGroups As Object
    Private m_bFromGroups As Boolean

    Private m_bAllowStopped As Boolean
    'MKR 02/11/2004 PN 14672
    Private m_iMatchCurrencyId As Integer
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTBankReconciliation.General

    Private m_oFindAccount As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the search data from the business object.
    Public m_vSearchData(,) As Object
    'sj 29/04/2003 - start
    Private m_vTotalArray As Object
    Private m_cTotalReconciled As Decimal
    Private m_cTotalUnreconciled As Decimal

    'sj 29/04/2003 - end

    ' Stores the total marked value
    Private m_cTotalMarked As Decimal

    ' Instance of form control
    Private m_oFormFields As iPMFormControl.FormFields

    'sj 28/04/2003 - start
    Private m_lCurrentBankAccount As Integer
    Private m_sCurrentlyLockedBy As String = ""
    Private m_iBankAccountLocked As Integer
    'sj 28/04/2003 - end

    'DD 07/10/2003
    Private m_oBankAccount As Object
    Private m_cBalance As Decimal

    'CJB 041004 PN15225
    Private m_cOpeningBalance As Decimal

    ' PN17440
    Private m_cClosingBalance As Decimal

    'DJM 04/03/2004
    Private m_iBankCurrencyID As Integer

    Private m_bListViewManualClick As Boolean = True
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property


    ' {* USER DEFINED CODE (Begin) *}
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

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
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

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property

    Public Property AccountID() As Integer
        Get

            ' Return the objects parameter value.
            Return m_lAccountId

        End Get
        Set(ByVal Value As Integer)

            ' Set the object parameter value.
            m_lAccountId = Value


        End Set
    End Property

    Public ReadOnly Property NavigatorTitle() As String
        Get

            ' Return the objects parameter value.
            Return m_sNavigatorTitle

        End Get
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get

    End Property

    Public Property SourceId() As Integer
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            'm_oFormFields = New iPMFormControl.FormFields()

            'm_oFormFields.LanguageID = g_iLanguageID

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the controls to
            ' PMFormControl NOT WORKING YET
            '
            ' Example:-
            '
            '        ' Pass control and required settings to FormControl
            '        m_lReturn = m_oFormFields.AddNewFormField( _
            ''                       ctlControl:=<Control Name>, _
            ''                       lFieldType:=<PM field type>, _
            ''                       lFormat:=<PM format string>, _
            ''                       lMandatory:=<PMMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDateTo, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lFieldType:=gPMConstants.PMEDataType.PMDate, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBankStatementBalance, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            ' {* USER DEFINED CODE (End) *}


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim vAccountID As String = ""
        Dim vDateTo As String = ""
        Dim lMarkedStatus, lMonth As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

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

            ' {* USER DEFINED CODE (Begin) *}

            If CDbl(uctBankAccount.AccountId) <> 0 Then
                vAccountID = uctBankAccount.AccountId
            Else
                vAccountID = CStr(0)
            End If

            If txtDateTo.Text.Trim() <> "" Then

                vDateTo = CStr(m_oFormFields.UnformatControl(txtDateTo))
            Else
                vDateTo = ""
            End If

            ' Add the marked status
            Select Case cboMarkedStatus.Text.ToLower()
                Case "yes"
                    lMarkedStatus = 1
                Case "no"
                    lMarkedStatus = 0
                Case Else
                    lMarkedStatus = -1
            End Select

            ' Month
            Select Case cboMonth.Text
                Case "(all)"
                    lMonth = -1
                Case "January"
                    lMonth = 1
                Case "February"
                    lMonth = 2
                Case "March"
                    lMonth = 3
                Case "April"
                    lMonth = 4
                Case "May"
                    lMonth = 5
                Case "June"
                    lMonth = 6
                Case "July"
                    lMonth = 7
                Case "August"
                    lMonth = 8
                Case "September"
                    lMonth = 9
                Case "October"
                    lMonth = 10
                Case "November"
                    lMonth = 11
                Case "December"
                    lMonth = 12
            End Select


            m_lReturn = g_oBusiness.SearchDetails(v_lMarkedStatus:=lMarkedStatus, v_lMonth:=lMonth, v_vAccountID:=vAccountID, v_vDateTo:=vDateTo, lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=m_vSearchData, r_cTotalReconciled:=m_cTotalReconciled, r_cTotalUnreconciled:=m_cTotalUnreconciled)


            ' {* USER DEFINED CODE (End) *}

            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.

                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No found search details

                Case Else
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
            End Select


            m_lReturn = m_oBankAccount.GetBankStatementBalance(uctBankAccount.AccountId, m_cBalance)
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get Bank Statement Balance from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DJM 04/03/2004 : Get the currency for the bank.

            m_lReturn = m_oBankAccount.GetDetails(vBankAccountId:=uctBankAccount.Id)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = m_oBankAccount.GetNext(vCurrencyId:=m_iBankCurrencyID)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the banks currency from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Display the number of item found message.
            DisplayStatusFound()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim iLoop2 As Integer
        Dim sMonth As String = ""
        Dim sFormatted As String = ""
        Dim sListItem() As String
        Dim oListItemArr() As ListViewItem
        ' Const ACFindImage As String = "FindImage"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwSearchDetails.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then

                m_lReturn = UpdateTotalReconciled()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function UpdateTotalReconciled failed.")
                End If

                m_cClosingBalance = m_cOpeningBalance

                m_lReturn = g_oBusiness.CurrencyFormat(m_cClosingBalance, m_iBankCurrencyID, sFormatted)
                lblClosingBalance.Text = sFormatted

                Return result
            End If

            'EK 310100
            cmdMarkAll.Enabled = True
            ' Hide the list view

            ' Assign the details to the interface.

            ReDim sListItem(8)
            ReDim oListItemArr(m_vSearchData.GetUpperBound(1))
            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' ************************************************************
                ' Enter your code here to assign the all of the interface
                ' details from the search data storage, using the FormatField
                ' function for any type conversion.
                '
                ' Example:-
                '
                '    ' Assign the details to the first column.
                '    Set oListItem = lvwSearchDetails.ListItems.Add(, , _
                ''        Trim$(m_vSearchData(ACName, lRow&)), , ACFindImage)
                '
                '    ' Assign details to other the columns
                '     m_lReturn& = m_oFormFields.FormatControl( _
                ''                       oListItem.SubItems(1), _
                ''                       m_vSearchData(ACCode, lRow&))
                '
                ' NOTE: Replace this section with your new code.
                ' ************************************************************

                ' {* USER DEFINED CODE (End) *}

                Select Case CInt(m_vSearchData(ACMarkedStatus, lRow))
                    Case 0

                        sListItem(0) = m_vSearchData(ACSourceID, lRow).ToString
                        'developer guide no. 274
                        'ListViewHelper.SetListItemSmallIconProperty(oListItem, ACIconBlank)
                    Case 1
                        ' Use a reconciled mark
                        sListItem(0) = m_vSearchData(ACSourceID, lRow).ToString
                        'developer guide no. 274
                        'ListViewHelper.SetListItemSmallIconProperty(oListItem, ACIconReconciled)
                    Case 2
                        'MKR 02/11/2004 PN 14672
                        'Storing the currency ID
                        m_iMatchCurrencyId = CInt(m_vSearchData(ACCurrencyId, lRow))
                        ' Use a checked mark

                        'changes as per vb6 code
                        sListItem(0) = m_vSearchData(ACSourceID, lRow).ToString
                        'developer guide no. 274
                        'ListViewHelper.SetListItemSmallIconProperty(oListItem, ACIconCheck)
                End Select

                sListItem(1) = m_vSearchData(ACPeriodName, lRow).ToString
                sListItem(2) = m_vSearchData(ACClientCode, lRow).ToString
                sListItem(3) = m_vSearchData(ACChequeNo, lRow).ToString
                sListItem(4) = m_vSearchData(ACTransRef, lRow).ToString
                sListItem(5) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, m_vSearchData(ACTransDate, lRow)).ToString
                sListItem(6) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_vSearchData(ACTransAmt, lRow)).ToString
                sListItem(7) = m_vSearchData(ACCurrency, lRow).ToString
                sListItem(8) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, m_vSearchData(ACPeriodEndDate, lRow)).ToString
                oListItem = New ListViewItem(sListItem)
                oListItem.Tag = lRow.ToString
                oListItemArr(lRow) = oListItem
            Next lRow
            lvwSearchDetails.Items.AddRange(oListItemArr)

            ' Select the first item.
            lvwSearchDetails.Items.Item(0).Selected = True

            lvwSearchDetails.Items(0).Focused = True

            For iLoop2 = 0 To 8
                Me.lvwSearchDetails.Columns.Item(iLoop2).Width = 80
            Next

            ' Size the columns
            'm_lReturn = ListViewFunc.ListViewAutoSize(lvwList:=lvwSearchDetails, bSizeHeaders:=True)

            ' Show the list view

            're-order updates. PN17440
            'Update the total values
            m_lReturn = UpdateTotalReconciled()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function UpdateTotalReconciled failed.")
            End If

            m_lReturn = UpdateTotalMarked()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function UpdateTotalMarked failed.")
            End If


            'DD 07/10/2003
            m_oFormFields.FormatControl(txtBankStatementBalance, m_cBalance)
            'RKS 10/02/2005 PN18509- No need to disable txBankStatementBalance
            'If m_cBalance <> 0 Then
            '   txtBankStatementBalance.Enabled = False
            'Else
            txtBankStatementBalance.Enabled = True
            'End If

            ' Enable the interface now that the search
            ' has completed.
            m_lReturn = DisableInterface(bDisable:=False)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            'hide this column - we only use this to sort period_end column
            Me.lvwSearchDetails.Columns.Item(8).Width = CInt(0)

            Return result

        Catch excep As System.Exception



            ' Error Section.

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

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the selected item's tag, so we can use this
            ' as the index to the search data storage details.

            lSelectedItem = Convert.ToString(lvwSearchDetails.Items.Item(lvwSearchDetails.FocusedItem.Index).Tag)

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign the details from the search
            ' data storage to the property members.
            '
            ' Example:-
            '
            ' m_sName$ = Trim$(m_vSearchData(ACName, lSelectedItem&))
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            m_lReturn = GetLookupValues()

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



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0


        Try


            ' Update the interface details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign the details from the
            ' property members to the interface.
            '
            ' Example:-
            '
            ' txtName.Text = Trim$(m_sName$)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

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
        Dim sResult, sAppName, sSection As String
        Dim vDefault As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Get what solution we're part of

            m_lReturn = g_oSirConfig.GetRegSettings(sResult:=sResult, sAppName:=sAppName, sSection:=sSection, sKey:=gACTLibrary.ACTOrionSolutionValue, vDefault:=vDefault)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMOK) Or (sResult = "0") Then
                ' Default to MBP style of solution
                sResult = CStr(gACTLibrary.ACTOrionSolutionMBP)
            End If

            m_iSolutionConfig = CInt(sResult)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update the interface details with the
            ' property members.
            m_lReturn = PropertiesToInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwSearchDetails.Handle.ToInt32(), v_vShowRowSelect:=True)

            ' {* USER DEFINED CODE (Begin) *}
            'EK 310100
            lvwSearchDetails.FullRowSelect = True
            cmdMarkAll.Enabled = False

            With cboMarkedStatus
                .Items.Add("Both")
                .Items.Add("Yes")
                .Items.Add("No")
                .SelectedIndex = 2
            End With

            With cboMonth
                .Items.Add("(all)")
                .Items.Add("January")
                .Items.Add("February")
                .Items.Add("March")
                .Items.Add("April")
                .Items.Add("May")
                .Items.Add("June")
                .Items.Add("July")
                .Items.Add("August")
                .Items.Add("September")
                .Items.Add("October")
                .Items.Add("November")
                .Items.Add("December")
                .SelectedIndex = 0
            End With
            '
            ' Size the columns to the size of the headers
            m_lReturn = ListViewFunc.ListViewAutoSize(lvwList:=lvwSearchDetails)

            ' Shrink the first column
            '    lvwSearchDetails.ColumnHeaders(1).Width = 220

            ' Default the date
            txtDateTo.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, DateTime.Now)


            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
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
    Private Function ClearInterface() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle, sFormatted As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if the user still wishes to clear
            ' the interface.


            'developer guide no. 243
            'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display the message.
            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.No Then
                ' Don't continue with the clear.
                Return result
            End If
            'EK 310100
            cmdMarkAll.Enabled = False

            ' Clear the interface details.

            ' Clear the search data array.
            m_vSearchData = Nothing

            ' Clear the search list details.
            lvwSearchDetails.Items.Clear()

            ' Clear the search status bar.
            stbStatus.Text = ""

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to clear all of the interface details
            ' for a new search.
            '
            ' Example:-
            '
            '    txtName.Text = ""
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            cboMonth.SelectedIndex = 0
            cboMarkedStatus.SelectedIndex = 2
            txtDateTo.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, DateTime.Now)
            sFormatted = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=0).Trim()
            lblTotalMarked.Text = sFormatted
            lblTotalReconciled.Text = sFormatted
            lblTotalUnReconciled.Text = sFormatted
            lblOpeningBalance.Text = sFormatted
            lblClosingBalance.Text = sFormatted

            uctBankAccount.FirstItem = ""

            ' Set focus to the search details.
            uctBankAccount.Focus()

            ' {* USER DEFINED CODE (End) *}

            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            m_lReturn = DisableInterface(bDisable:=True)

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            'ReDim m_ctlTabFirstLast(1, )

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



            ' Error Section.

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


            'developer guide no. 243
            'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdReconcile.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPayButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdMark.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMarkButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDrill.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDrillButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

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



            lvwSearchDetails.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchDetails.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchDetails.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchDetails.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchDetails.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchDetails.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwSearchDetails.Columns.Item(6).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle7, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Item(7).Text = "Currency Code"

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

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdReconcile.Enabled = Not bDisable
            cmdDrill.Enabled = Not bDisable
            cmdMark.Enabled = Not bDisable

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
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup values.

                    m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Get all of the lookup values with the correct
                    ' effective date.

                    m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMView
                    ' Get lookup values for viewing only.

                    m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

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
    'For 'lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
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
    ' Add the details to the control.

    'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


    'ctlLookup.ItemData(ctlLookup.NewIndex) = m_vLookupDetails(ACDetailKey, lCntr)
    '
    ' Check if this is the selected index.
    'If m_vLookupValues(ACValueID, lRow).Equals(m_vLookupDetails(ACDetailKey, lCntr)) Then


    'ctlLookup.ListIndex = ctlLookup.NewIndex
    'End If
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
    ' Error Section.
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
                'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
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
        Dim lItemsFound As Integer

        Try

            ' Store the total of item found.
            If Not Information.IsArray(m_vSearchData) Then
                lItemsFound = 0
            Else
                lItemsFound = (m_vSearchData.GetUpperBound(1) + 1)
            End If

            ' Get message text if not already present.
            If sMessage = "" Then

                'developer guide no. 243
                'sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            'stbStatus.Text = " " & lItemsFound & " " & sMessage
            _stbStatus_Panel1.Text = " " & lItemsFound & " " & sMessage

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' PRIVATE Methods (End)

    ' ***************************************************************** '
    '
    ' Name: DrillDocument
    '
    ' Description: Drills the document selected using FindTransaction
    '
    ' History: 02/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function DrillDocument() As Integer

        Dim result As Integer = 0
        Dim sDocumentRef As String = ""
        Dim lAccountId, lRow As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Mouse pointer to busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If m_oFindTransaction Is Nothing Then

                ' Get an instance of the find object
                Dim temp_m_oFindTransaction As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oFindTransaction, sClassName:="iACTFindTransaction.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oFindTransaction = temp_m_oFindTransaction
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Mouse pointer back to normal
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Return result
                End If

            End If

            ' Set the object's property's
            If IsNothing(lvwSearchDetails.FocusedItem) Then
                lRow = 0
            Else
                lRow = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)
            End If

            sDocumentRef = CStr(m_vSearchData(ACTransRef, lRow))
            lAccountId = m_lAccountId

            ' Dont want to let them drill again

            m_oFindTransaction.DrillLevel = 2

            'DJM 23/05/2002 : Make sure that the drill is specific to company

            m_oFindTransaction.DrillCompany = CInt(m_vSearchData(ACSourceID, lRow))

            ' Document Ref

            m_oFindTransaction.DocumentRef = sDocumentRef
            ' Account ID

            m_oFindTransaction.AccountID = lAccountId

            ' Mouse pointer back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Start the object

            m_lReturn = m_oFindTransaction.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to drill document.", vApp:=ACApp, vClass:=ACClass, vMethod:="DrillDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Terminate is in QueryUnload

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DrillDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DrillDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdDrill_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDrill.Click

        ' Drill the document using FindTransaction
        m_lReturn = DrillDocument()

    End Sub



    ' ***************************************************************** '
    '
    ' Name: FreezeInterface
    '
    ' Description:
    '
    ' History: 16/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (FreezeInterface) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function FreezeInterface() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
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
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FreezeInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FreezeInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessReconcile
    '
    ' Description: Write Reconciliation record
    '
    '
    ' ***************************************************************** '
    Private Function ProcessReconcile() As Integer
        Dim result As Integer = 0
        Dim lTransdetailId As Integer
        Dim vTransDetailIds As Object

        Dim lRow As Integer
        Dim oListItem As ListViewItem
        Dim cAmount As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            For iLoop1 As Integer = 1 To lvwSearchDetails.Items.Count


                oListItem = lvwSearchDetails.Items.Item(iLoop1 - 1)
                ' Get the row

                lRow = Convert.ToString(oListItem.Tag)

                ' If its marked then...

                If CDbl(m_vSearchData(ACMarkedStatus, lRow)) = 2 Then

                    lTransdetailId = CInt(m_vSearchData(ACTransId, lRow))
                    If Not Information.IsArray(vTransDetailIds) Then
                        ReDim vTransDetailIds(0)
                    Else

                        ReDim Preserve vTransDetailIds(vTransDetailIds.GetUpperBound(0) + 1)
                    End If


                    vTransDetailIds(vTransDetailIds.GetUpperBound(0)) = lTransdetailId

                    ' Update the status
                    m_vSearchData(ACMarkedStatus, lRow) = 1


                    'developer guide no. 49
                    'oListItem.SmallIcon = ACIconReconciled
                    oListItem.ImageKey = ACIconReconciled


                    'developer guide no. 126 archana_TODO:
                    'oListItem.Icon = ACIconReconciled

                    'PN-22779 If Transaction Curr is diffrent than Base
                    If CInt(m_vSearchData(ACCurrencyId, lRow)) <> m_iBankCurrencyID Then
                        'Update the total reconciled amounts so that totals dispaly correctly.
                        cAmount = ConvertToBaseAmount(CDec(m_vSearchData(ACTransAmt, lRow)), CInt(m_vSearchData(ACSourceID, lRow)))
                    Else
                        'Update the total reconciled amounts so that totals dispaly correctly.
                        cAmount = CDec(m_vSearchData(ACTransAmt, lRow))
                    End If
                    m_cTotalReconciled += cAmount
                    m_cTotalUnreconciled -= cAmount

                End If


            Next iLoop1

            If Not Information.IsArray(vTransDetailIds) Then
                MessageBox.Show("You have not marked any transactions.", "No marked transactions", MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            m_lReturn = g_oBusiness.Reconcile(vTransDetailIds:=vTransDetailIds)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            'DD 07/10/2003 - reset the Bank Statement Balance

            m_lReturn = m_oBankAccount.UpdateBankStatementBalance(uctBankAccount.AccountId, 0)
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            m_oFormFields.FormatControl(txtBankStatementBalance, 0)
            txtBankStatementBalance.Enabled = True

            ' Set the mouse pointer back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            'DC310102 -report on reconciled reports

            m_lReturn = ReportReconciledItems()

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Interaction.MsgBox("Reconciliation Report Not Produced", MsgBoxStyle.OkCancel, "Bank Reconciliation Report")
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Messagex
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessREconcile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessREconcile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC300102 -Start -Allow Report To Be Produced
    Private Function ReportMarkedItems() As Integer
        Dim result As Integer = 0



        Dim oReport As iPMBReportPrint.Interface_Renamed
        Dim vKeyArray(1, 3) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get Instance of Reportprint component
            Dim temp_oReport As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oReport, sClassName:="iPMBReportPrint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oReport = temp_oReport

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create 'iPMBReportPrint.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportMarkedItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                If Information.IsReference(oReport) Then

                    oReport.Dispose()
                    oReport = Nothing
                End If
                Return result
            End If

            With oReport

                ' Send Report & Parameters into Report via SetKeys()

                vKeyArray(0, 0) = PMNavKeyConst.PMKeyNameReportName

                vKeyArray(1, 0) = "Marked_For_Reconciliation"


                vKeyArray(0, 1) = PMNavKeyConst.PMKeyNamePrintReport

                vKeyArray(1, 1) = PMNavKeyConst.AC_PRINT_AND_VIEW

                vKeyArray(1, 1) = PMNavKeyConst.AC_VIEW_ONLY

                'MKW160603 PN3779 START - Submit Bank Id (unique).

                vKeyArray(0, 2) = PMNavKeyConst.PMKeyNameParam1Name
                'vKeyArray(1, 2) = "bank_code"

                vKeyArray(1, 2) = "bank_id"

                'vKeyArray(0, 3) = "bank_code"
                'vKeyArray(1, 3) = Trim$(uctBankAccount.Code)


                vKeyArray(0, 3) = "bank_id"

                vKeyArray(1, 3) = CStr(uctBankAccount.Id).Trim()
                'MKW160603 PN3779 END


                m_lReturn = .SetKeys(vKeyArray:=vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Set Keys for Report.", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportMarkedItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


                    .Dispose()
                    oReport = Nothing
                    Return result
                End If


                ' Generate Report

                m_lReturn = .Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Generate the Report.", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportMarkedItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


                    .Dispose()
                    oReport = Nothing
                    Return result
                End If

                ' Close Report Component

                .Dispose()
                oReport = Nothing

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReportMarkedItems Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportMarkedItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC300102 -End

    'DC300102 -Start -Allow Report To Be Produced
    Private Function ReportReconciledItems() As Integer
        Dim result As Integer = 0

        Dim oReport As iPMBReportPrint.Interface_Renamed
        Dim vKeyArray(1, 3) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get Instance of Reportprint component
            Dim temp_oReport As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oReport, sClassName:="iPMBReportPrint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oReport = temp_oReport

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create 'iPMBReportPrint.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportMarkedItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                If Information.IsReference(oReport) Then

                    oReport.Dispose()
                    oReport = Nothing
                End If
                Return result
            End If

            With oReport

                ' Send Report & Parameters into Report via SetKeys()

                vKeyArray(0, 0) = PMNavKeyConst.PMKeyNameReportName

                vKeyArray(1, 0) = "Reconciliation_Report"


                vKeyArray(0, 1) = PMNavKeyConst.PMKeyNamePrintReport
                'DC130202

                vKeyArray(1, 1) = PMNavKeyConst.AC_VIEW_ONLY


                vKeyArray(0, 2) = PMNavKeyConst.PMKeyNameParam1Name
                'MKW240603 PN4945 Start
                '        vKeyArray(1, 2) = "bank_code"

                '        vKeyArray(0, 3) = "bank_code"
                '        vKeyArray(1, 3) = Trim$(uctBankAccount.Code)


                vKeyArray(1, 2) = "bank_id"


                vKeyArray(0, 3) = "bank_id"

                vKeyArray(1, 3) = CStr(uctBankAccount.Id).Trim()
                'MKW240603 PN4945 End

                m_lReturn = .SetKeys(vKeyArray:=vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Set Keys for Report.", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportReconciledItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


                    .Dispose()
                    oReport = Nothing
                    Return result
                End If


                ' Generate Report

                m_lReturn = .Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Generate the Report.", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportReconciledItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


                    .Dispose()
                    oReport = Nothing
                    Return result
                End If

                ' Close Report Component

                .Dispose()
                oReport = Nothing

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReportReconciledItems Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportReconciledItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC300102 -End
    ' ***************************************************************** '
    '
    ' Name: ProcessMark
    '
    ' Description: Marks all the selected transactions
    '
    ' History: 05/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    'EK 310100 New Option to Mark All transactions
    Private Function ProcessMark(ByRef bMarkAll As Boolean) As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Dim lTransdetailId As Integer
        Dim iCurrencyID As Integer
        Dim lRow As Integer

        Dim cGrossAmount As Decimal
        'EK 14/10/99

        'EK 090200
        Dim cAllocationAmount As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            For iLoop1 As Integer = 1 To lvwSearchDetails.Items.Count

                oListItem = lvwSearchDetails.Items.Item(iLoop1 - 1)

                ' If its selected then...
                If (oListItem.Selected) Or (bMarkAll) Then

                    ' Get the row

                    lRow = Convert.ToString(oListItem.Tag)

                    cGrossAmount = CDec(m_vSearchData(ACTransAmt, lRow))
                    ' Dont mark it if it's already been reconciled
                    If CDbl(m_vSearchData(ACMarkedStatus, lRow)) <> 1 Then
                        lTransdetailId = CInt(m_vSearchData(ACTransId, lRow))
                        ' Currency ID
                        iCurrencyID = CInt(m_vSearchData(ACCurrencyId, lRow))
                        ' if selected untick
                        If CDbl(m_vSearchData(ACMarkedStatus, lRow)) = 2 Then
                            'Don't Deselect if we are doing ALL
                            If Not bMarkAll Then
                                ' Call the business object

                                m_lReturn = g_oBusiness.UnMarkTransaction(v_lTransDetailID:=lTransdetailId)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                    ' Set the mouse pointer back to normal
                                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                    Return result
                                End If

                                'oListItem.ImageKey = ACIconBlank
                                m_bListViewManualClick = False
                                oListItem.Checked = False
                                m_bListViewManualClick = True

                                ' Update the status
                                m_vSearchData(ACMarkedStatus, lRow) = 0
                            End If
                        Else
                            'MKR 02/11/2004 PN 14672 -- Start
                            'If trying to choose a transaction with a diff. currencyID
                            'then disallow it..
                            If m_iMatchCurrencyId = 0 Then
                                m_iMatchCurrencyId = iCurrencyID
                            Else
                                If iCurrencyID <> m_iMatchCurrencyId Then
                                    MessageBox.Show("Cannot match transactions with different currencies", "Error")
                                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                    Return result
                                End If
                            End If
                            'MKR 02/11/2004 PN 14672
                            ' Call the business object

                            m_lReturn = g_oBusiness.MarkTransaction(v_lTransactionID:=lTransdetailId, v_iCurrencyID:=iCurrencyID, v_cPayment:=cAllocationAmount)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                ' Set the mouse pointer back to normal
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                Return result
                            End If

                            ' oListItem.ImageKey = ACIconCheck
                            m_bListViewManualClick = False
                            oListItem.Checked = True
                            m_bListViewManualClick = True
                            'archana_TODO:
                            'oListItem.Icon = ACIconCheck

                            ' Update the status
                            m_vSearchData(ACMarkedStatus, lRow) = 2
                            'mark all
                        End If
                        'not fully settled
                    End If
                    'selected or ALL
                End If

            Next iLoop1

            'Update total marked for payment
            m_lReturn = UpdateTotalMarked()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function UpdateTotalMarked failed.")
            End If

            'These totals don't change if marking a record so don't do them.
            'Getting the account balance is quite slow.
            'm_lReturn = UpdateTotalReconciled()
            'm_lReturn = GetAccountBalance()

            ' Set the mouse pointer back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessMark Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessMark", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdMarkAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMarkAll.Click
        m_lReturn = ProcessMark(bMarkAll:=True)

    End Sub

    Private Sub cmdMark_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMark.Click
        m_lReturn = ProcessMark(bMarkAll:=False)
    End Sub

    Private Sub cmdReconcile_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReconcile.Click
        'eck110911 Add confirmation Message
        Dim sFormatted As String = ""

        'DD 07/10/2003 - Validate if a Bank Statement Balance has been set

        Dim cBalance As Decimal = CDec(m_oFormFields.UnformatControl(txtBankStatementBalance))

        ' CJB 041004 - PN15225 Also take into account the opening balance when comparing...
        'If cBalance <> 0 And cBalance <> Round((m_cTotalMarked + m_cOpeningBalance), 2) Then
        If cBalance <> Math.Round(m_cTotalMarked + m_cOpeningBalance, 2) Then 'PN -71872
            ' label of text box has changed. PN17440
            MessageBox.Show("The Total Marked plus Opening Balance (" & Math.Round(m_cTotalMarked + m_cOpeningBalance, 2) & ") does not equal the Closing Bank Statement Balance Amount (" & CStr(cBalance) & ").", "Marked balance mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim sMessage As String = "About to reconcile items are you sure? - This cannot be reversed"
        Dim iMsgResult As DialogResult = MessageBox.Show(sMessage, "Confirm Reconciliation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        ' Check message result.
        If iMsgResult = System.Windows.Forms.DialogResult.Yes Then
            m_lReturn = ProcessReconcile()
        End If

        ' PN17440
        m_cOpeningBalance = m_cClosingBalance

        m_lReturn = g_oBusiness.CurrencyFormat(m_cOpeningBalance, m_iBankCurrencyID, sFormatted)
        lblOpeningBalance.Text = sFormatted

        m_lReturn = UpdateTotalMarked()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function UpdateTotalMarked failed.")
        End If

        m_lReturn = UpdateTotalReconciled()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function UpdateTotalReconciled failed.")
        End If

        'These totals don't change if marking a record so don't do them.
        'Getting the account balance is quite slow.
        'm_lReturn = GetAccountBalance()
    End Sub

    'DC300102 -to produce report
    Private Sub cmdReport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReport.Click
        'TODO: commented requires crystal reports
        m_lReturn = ReportMarkedItems()

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            Try

                ' Tell the resizer control about the controls on the form
                With uctPMResizer

                    .SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cmdHelp", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .SetControlResizeOption("cmdReconcile", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cmdMark", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    'PN4931 eck 230603
                    .SetControlResizeOption("cmdMarkAll", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .SetControlResizeOption("cmdDrill", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)


                    .SetControlResizeOption("cmdFindNow", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cmdNewSearch", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    'DJM 23/05/2002 : Add the report button to resize object
                    .SetControlResizeOption("cmdReport", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)


                    .SetControlResizeOption("imgImage", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .SetControlResizeOption("tabMainTab", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROWidthOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lvwSearchDetails", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .SetControlResizeOption("lblBank", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblDateTo", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblMarkedStatus", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblMonth", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .SetControlResizeOption("lblTotalMarkedLabel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblTotalMarked", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    ' added this control for 800x600 layout purposes. PN17440
                    .SetControlResizeOption("lblClosed", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    ' no resize for this text box. PN17440
                    .SetControlResizeOption("lblBankStatementBalance", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("txtBankStatementBalance", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .SetControlResizeOption("lblOpeningBalanceLabel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblOpeningBalance", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblTotalReconciledLabel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblTotalReconciled", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblTotalUnreconciledLabel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblTotalUnReconciled", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblClosingBalanceLabel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("lblClosingBalance", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .SetControlResizeOption("uctBankAccount", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("txtDateTo", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cboMarkedStatus", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                    .SetControlResizeOption("cboMonth", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    '.SetControlResizeOption "cmdMarkAll", pmeCRONoResize, pmeCRTRelativeToBottomRight
                    .SetControlResizeOption("cmdMarkAll", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .SetControlResizeOption("stbStatus", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCRONoResize, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                    .FormMinHeight = 5685
                    .FormMinWidth = 9675

                End With

                'sj 28/04/2003 - start
                m_lReturn = LockBankAccount()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
                End If
                'sj 28/04/2003 - end

                Exit Sub

            Catch excep As System.Exception



                ' Error Section
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to activate the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Activate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Exit Sub

            End Try
        End If
    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        iPMFunc.ShowFormInTaskBar_Attach()

        ' Forms initialise event.

        Try
            cmdMark.DialogResult = Windows.Forms.DialogResult.None
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTBankReconciliation.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=g_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBankAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBankAccount, "bACTBankAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBankAccount = temp_m_oBankAccount

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTBankAccount.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=New Exception(Information.Err().Description))

                Exit Sub
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        iPMFunc.ShowFormInTaskBar_Detach()

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

            ' Set the status for the business object.

            m_lReturn = g_oBusiness.SetStatus(sProcessStatus:=m_sProcessStatus.Value, sMapStatus:=m_sMapStatus.Value, sStepStatus:=m_sStepStatus.Value)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the status for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If
            ' Create an instance of the FormControl object.

            m_oFormFields = New iPMFormControl.FormFields()
            m_oFormFields.LanguageID = g_iLanguageID

            '
            ' Set validation for controls on form
            m_lReturn = SetFieldValidation()

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            'developer guide 38
            Me.uctBankAccount.CompanyId = SourceId
            Me.uctBankAccount.FirstItem = ""
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Check if the search contains more or equal
            ' to the miniumum search length.

            ' {* USER DEFINED CODE (Begin) *}


            ' {* USER DEFINED CODE (End) *}

            ' Gets the interface details to be displayed.
            'm_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If
            uctBankAccount.Select()
            Me.Refresh()


            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

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

            'sj 28/04/2003 - start
            'Unlock the bank account record
            m_lReturn = LockBankAccount(v_bUnLockOnly:=True)
            'sj 28/04/2003 - end

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the find transaction if needed
            If Not (m_oFindTransaction Is Nothing) Then
                ' Terminate the instance

                m_oFindTransaction.Dispose()


                ' Remove the instance
                m_oFindTransaction = Nothing
            End If

            ' Terminate the find transaction if needed
            If Not (m_oBankAccount Is Nothing) Then
                ' Terminate the instance

                m_oBankAccount.Dispose()


                ' Remove the instance
                m_oBankAccount = Nothing
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()



            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing


            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMainTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If
                End Select
            End With
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub lvwSearchDetails_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles lvwSearchDetails.ItemCheck
        ' Double click event for the search details.
        Try
            If m_bListViewManualClick Then
                e.NewValue = e.CurrentValue
            End If

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Item Check event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ItemCheck", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub


    Private Sub lvwSearchDetails_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSearchDetails.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If lvwSearchDetails.GetItemAt(x, y) Is Nothing Then
            m_lReturn = DisableInterface(bDisable:=True)
        Else
            'sj 28/04/2003 - start
            If m_iBankAccountLocked = g_kLockedByOtherUser Then
                m_lReturn = DisableInterface(bDisable:=True)
            Else
                m_lReturn = DisableInterface(bDisable:=False)
            End If
            '        m_lReturn& = DisableInterface(bDisable:=False)
            'sj 28/04/2003 - end
        End If

    End Sub

    ' ***************************************************************** '
    '
    ' Name: CheckEnableButtons
    '
    ' Description: Enables the Mark button if any items are selected.
    '
    ' History: 08/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function CheckEnableButtons() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check each list item

            For iLoop1 As Integer = 1 To lvwSearchDetails.Items.Count

                ' If it's selected then
                If lvwSearchDetails.Items.Item(iLoop1 - 1).Selected Then
                    ' Enable the mark button
                    'sj 28/04/2003 - start
                    If m_iBankAccountLocked = g_kLockedByOtherUser Then
                        cmdMark.Enabled = False
                        cmdReconcile.Enabled = False
                        'added as Drill button enable functionality should also work
                        cmdDrill.Enabled = False
                    Else
                        cmdMark.Enabled = True
                        cmdReconcile.Enabled = True
                        'added as Drill button enable functionality should also work
                        cmdDrill.Enabled = True
                    End If
                    '            cmdMark.Enabled = True
                    '            cmdReconcile.Enabled = True
                    'sj 28/04/2003 - end
                    ' Exit the loop
                    Exit For
                End If
            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckEnableButtons Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckEnableButtons", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub lvwSearchDetails_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSearchDetails.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        ' Check if anything was selected so as to enable the right
        ' button(s)
        m_lReturn = CheckEnableButtons()

    End Sub



    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs)

        Try

            With tabMainTab
                ' Set the default button.
                'If (.Tab < cmdNext.Count) Then
                '    cmdNext(.Tab).Default = True
                'Else
                '    cmdOK.Default = True
                'End If

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                End If
            End With

        Catch



            ' Error Section.


            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'eck050500
            If lvwSearchDetails.Items.Count = 0 Then
                Me.Hide()
                Exit Sub
            End If
            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

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
            'eck050500
            If lvwSearchDetails.Items.Count = 0 Then
                Me.Hide()
                Exit Sub
            End If

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click


        Dim sFormatted As String = ""

        ' Click event of the Cancel button.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            sFormatted = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=0).Trim()

            lblTotalMarked.Text = sFormatted
            lblTotalReconciled.Text = sFormatted
            lblTotalUnReconciled.Text = sFormatted
            lblOpeningBalance.Text = sFormatted
            lblClosingBalance.Text = sFormatted

            cmdMarkAll.Enabled = False

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
            End If

            m_bListViewManualClick = False
            ' Set the focus.
            lvwSearchDetails.Focus()
            For iItem As Integer = 0 To lvwSearchDetails.Items.Count - 1
                If CDbl(m_vSearchData(ACMarkedStatus, iItem)) = 2 Then
                    Me.lvwSearchDetails.Items.Item(iItem).Checked = True
                End If
            Next
            m_bListViewManualClick = True
            'sj 28/04/2003 - start
            m_lReturn = LockBankAccount()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockBankAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFindNow_Click")
            End If
            'sj 28/04/2003 - end
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Find Now command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFindNow_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

        ' Click event of the New Search button.

        Try

            ' Clear the interface details.
            m_lReturn = ClearInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If

            'sj 28/04/2003 - start
            'Unlock the bank account
            m_lReturn = LockBankAccount(v_bUnLockOnly:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockBankAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click")
            End If
            'sj 28/04/2003 - end

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick

        ' Double click event for the search details.
        Try

            ' Check if there are any items available.
            If lvwSearchDetails.Items.Count = 0 Then
                Exit Sub
            End If

            ' Call the edit button function
            If cmdMark.Enabled Then
                cmdMark_Click(cmdMark, New EventArgs())
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Enter

        ' GotFocus Event for the search details

        Try

            ' Set the default button
            VB6.SetDefault(cmdOK, True)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Leave

        ' LostFocus Event for the search details

        Try

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDetails.Columns(eventArgs.Column)

        Dim lHeaderIndex As Integer
        Dim sHeaderTag As String = ""

        ' Column click event for the search details
        Try


            sHeaderTag = Convert.ToString(ColumnHeader.Tag)
            lHeaderIndex = ColumnHeader.Index + 1

            If ColumnHeader.Index + 1 = 2 Then
                lHeaderIndex = 9
                sHeaderTag = "DATESORT"
            End If

            If ColumnHeader.Index + 1 = 1 Then
                sHeaderTag = "VALUESORT"
            End If


            With lvwSearchDetails
                'Identify the Date type columns
                If sHeaderTag.ToUpper() = "DATESORT" Then

                    ListViewHelper.SetSortedProperty(lvwSearchDetails, False)
                    If ListViewHelper.GetSortOrderProperty(lvwSearchDetails) = SortOrder.Ascending Then
                        ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Descending)
                    Else
                        ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
                    End If
                    'Special Sort function for Dates
                    ListViewFunc.ListViewSortByDate(lvwSearchDetails, lHeaderIndex - 1, ListViewHelper.GetSortOrderProperty(lvwSearchDetails))

                    'Identify the Value type columns (to sort numerics correctly)
                ElseIf sHeaderTag.ToUpper() = "VALUESORT" Then
                    ListViewHelper.SetSortedProperty(lvwSearchDetails, False)
                    'ListViewHelper.SetSortOrderProperty(lvwSearchDetails, (ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2)
                    If ListViewHelper.GetSortOrderProperty(lvwSearchDetails) = SortOrder.Ascending Then
                        ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Descending)
                    Else
                        ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
                    End If
                    'Use the special sort function for numerics
                    ListViewFunc.ListViewSortByValue(lvwSearchDetails, lHeaderIndex - 1, ListViewHelper.GetSortOrderProperty(lvwSearchDetails))

                    'See if this the column already sorted on
                ElseIf (lHeaderIndex - 1 = ListViewHelper.GetSortKeyProperty(lvwSearchDetails)) Then
                    ' Set sort order opposite of current direction.
                    If ListViewHelper.GetSortOrderProperty(lvwSearchDetails) = SortOrder.Ascending Then
                        ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Descending)
                    Else
                        ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
                    End If

                    'If this is the very first time that a header is clicked then
                    'if clicking on the first column we need to refresh. Otherwise we don't
                    If Not ListViewHelper.GetSortedProperty(lvwSearchDetails) Then
                        'Do the refresh
                        ListViewHelper.SetSortedProperty(lvwSearchDetails, True)
                    End If

                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwSearchDetails, False)

                    ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwSearchDetails, lHeaderIndex - 1)
                    ListViewHelper.SetSortedProperty(lvwSearchDetails, True)
                End If

            End With

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    'UPGRADE_NOTE: (7001) The following declaration (ListViewSortByCode) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ListViewSortByCode(ByVal v_oListView As ListView, ByVal v_iSourceColumn As Integer, ByVal v_iDirection As SortOrder) As Integer
    '
    'Dim result As Integer = 0
    'Dim sDate As String = ""
    'Dim iLoop1, iIndex As Integer
    'Const ACLVTag As String = "SORT_DATE_HIDDEN"
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Add the column
    '
    ' Get the index of this new column, -1 because it's a sub item
    'iIndex = v_iSourceColumn
    '
    ' Not sorted yet
    'ListViewHelper.SetSortedProperty(v_oListView, False)
    '
    '
    ' Sort now
    'ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)
    '
    ' Set the sort key
    'ListViewHelper.SetSortKeyProperty(v_oListView, iIndex)
    '
    'ListViewHelper.SetSortedProperty(v_oListView, True)
    '
    '
    ' Reset the sort key
    'v_oListView.SortKey = v_iSourceColumn%
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewSortByCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewSortByCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Private Sub txtBankStatementBalance_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankStatementBalance.Enter
        m_oFormFields.GotFocus(ctlControl:=txtBankStatementBalance)
    End Sub

    Private Sub txtBankStatementBalance_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankStatementBalance.Leave

        If CDbl(m_oFormFields.UnformatControl(txtBankStatementBalance)) <> 0 Then

            '            'DD 07/10/2003 - Update Bank Statement Balance

            m_lReturn = m_oBankAccount.UpdateBankStatementBalance(uctBankAccount.AccountId, m_oFormFields.UnformatControl(txtBankStatementBalance))
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
        End If

        m_oFormFields.LostFocus(ctlControl:=txtBankStatementBalance)
    End Sub

    ' PRIVATE Events (End)
    Private Sub txtDateTo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDateTo)
    End Sub

    Private Sub txtDateTo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDateTo)
    End Sub

    ' ***************************************************************** '
    '
    ' Name: UpdateTotalMarked
    '
    ' Description: Updates the total marked value
    '
    ' History: 01/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateTotalMarked() As Integer

        Dim result As Integer = 0
        Dim cTotal As Decimal

        Dim sFormatted As String
        Dim cMarkedinBase As Decimal
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cTotal = 0

            ' Exit if no data
            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If


            ' Get the total
            For lLoop1 As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                If CInt(m_vSearchData(ACMarkedStatus, lLoop1)) = 2 Then
                    'DC140801 was checking for Lng not Dbl, therefore anything less
                    'than a £1 would always become 0

                    'PN-22779 --Converting into base currency if not in Base currency
                    If CInt(m_vSearchData(ACCurrencyId, lLoop1)) <> m_iBankCurrencyID Then
                        cMarkedinBase = ConvertToBaseAmount(CDec(m_vSearchData(ACTransAmt, lLoop1)), CInt(m_vSearchData(ACSourceID, lLoop1)))
                    Else
                        cMarkedinBase = CDec(m_vSearchData(ACTransAmt, lLoop1))
                    End If

                    If (CDbl(m_vSearchData(ACTransAmt, lLoop1))) <> 0 Then
                        cTotal += cMarkedinBase
                    End If
                End If
            Next lLoop1

            'DD 17/07/2002: Fixed currency formatting
            'PSL  08/10/2003 Make all formating the same

            m_lReturn = g_oBusiness.CurrencyFormat(cTotal, m_iBankCurrencyID, sFormatted)
            lblTotalMarked.Text = sFormatted
            'lblTotalMarked.Caption = Trim$(FormatField(iFormatType:=PMFormatCurrency, vFieldValue:=cTotal))

            ' Store the total marked
            m_cTotalMarked = cTotal

            'MKR 02/11/2004 PN 14672
            If m_cTotalMarked = 0 Then
                m_iMatchCurrencyId = 0
            End If

            ' PN17440
            m_cClosingBalance = m_cOpeningBalance + m_cTotalMarked

            m_lReturn = g_oBusiness.CurrencyFormat(m_cClosingBalance, m_iBankCurrencyID, sFormatted)
            lblClosingBalance.Text = sFormatted

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateTotalMarked Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTotalMarked", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateTotalReconciled
    '
    ' Description: Updates the total marked value
    '
    ' History: 01/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateTotalReconciled() As Integer

        Dim result As Integer = 0
        Dim sFormatted As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.CurrencyFormat(m_cTotalReconciled, m_iBankCurrencyID, sFormatted)

            lblTotalReconciled.Text = sFormatted
            ' PN17440
            m_cOpeningBalance = m_cTotalReconciled
            lblOpeningBalance.Text = sFormatted


            m_lReturn = g_oBusiness.CurrencyFormat(m_cTotalUnreconciled, m_iBankCurrencyID, sFormatted)

            lblTotalUnReconciled.Text = sFormatted


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateTotalReconciled Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTotalReconciled", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LockBankAccount
    '
    ' Description:
    '
    ' History: 28/04/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Function LockBankAccount(Optional ByVal v_bUnLockOnly As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_iBankAccountLocked = g_kLockedByCurrentUser Then
                'Unlock the bank account

                m_lReturn = g_oBusiness.UnLockBankAccount(v_lAccountID:=m_lCurrentBankAccount)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    '            LogMessage _
                    ''                iType:=PMError, _
                    ''                sMsg:="Failed to unlock bank account for account id" & m_lCurrentBankAccount, _
                    ''                vApp:=ACApp, _
                    ''                vClass:=ACClass, _
                    ''                vMethod:="uctBankAccount_Click"
                    Return result
                End If
                m_iBankAccountLocked = g_kNotLocked
            End If

            If v_bUnLockOnly Then
                Return result
            End If

            m_lCurrentBankAccount = CInt(uctBankAccount.AccountId)

            'Lock the bank account

            m_lReturn = g_oBusiness.LockBankAccount(v_lAccountID:=m_lCurrentBankAccount, r_sCurrentlyLockedBy:=m_sCurrentlyLockedBy)
            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to lock bank account for account id" & m_lCurrentBankAccount, vApp:=ACApp, vClass:=ACClass, vMethod:="uctBankAccount_Click")
                Return result
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                'The bank account is locked by another user so disable the action
                'buttons and display as message
                m_iBankAccountLocked = g_kLockedByOtherUser
                cmdReconcile.Enabled = False
                cmdMark.Enabled = False
                cmdDrill.Enabled = False
                cmdMarkAll.Enabled = False
                cmdOK.Enabled = False
                MessageBox.Show("Warning! Account in use by " & m_sCurrentlyLockedBy, "Bank Reconciliation", MessageBoxButtons.OK)
            Else
                'Flag to say we have this account locked
                m_iBankAccountLocked = g_kLockedByCurrentUser
                cmdOK.Enabled = True
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockBankAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockBankAccount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAccountBalance
    '
    ' Description:
    '
    ' History: 29/04/2003 sj - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetAccountBalance) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetAccountBalance() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Dim vAccountBalance As Decimal
    'Dim sFormattedBalance As String = ""
    'Dim vAccountID As String = ""
    'Dim vAccountingDate As Date
    '
    'If CDbl(uctBankAccount.AccountId) <> 0 Then
    'vAccountID = uctBankAccount.AccountId
    'Else
    'vAccountID = CStr(0)
    'End If
    '
    'If txtDateTo.Text.Trim() <> "" Then

    'vAccountingDate = CDate(m_oFormFields.UnformatControl(txtDateTo))
    'Else
    'vAccountingDate = DateTime.Now
    'End If
    '
    'Closing Balance

    'm_lReturn = g_oBusiness.GetAccountBalance(r_vAccountBalance:=vAccountBalance, v_vAccountID:=vAccountID, v_vAccountingDate:=vAccountingDate, r_sFormattedBalance:=sFormattedBalance)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oBusiness.GetAccountBalance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountBalance")
    'Return result
    'End If
    '

    'm_lReturn = g_oBusiness.CurrencyFormat(vAccountBalance, m_iBankCurrencyID, sFormattedBalance)
    'lblClosingBalance.Text = sFormattedBalance
    '
    ' Save the closing balance for use later (when items are marked). PN17440
    'm_cClosingBalance = vAccountBalance
    '

    'm_lReturn = g_oBusiness.CurrencyFormat(vAccountBalance, m_iBankCurrencyID, sFormattedBalance)
    'lblOpeningBalance.Text = sFormattedBalance
    '
    ' CJB 041004 PN15225 - Save the opening balance for use later (if reconcile button clicked)
    'm_cOpeningBalance = vAccountBalance
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountBalance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountBalance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Private Sub uctBankAccount_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctBankAccount.Click
        If Not m_oFormFields Is Nothing Then
            m_oFormFields.FormatControl(txtBankStatementBalance, 0)
            'cmdFindNow.PerformClick()
        End If
    End Sub

    '**********************************************************************
    '
    ' Name: ConvertToBaseAmount
    '
    ' Description: Converts the Transaction into Base currency amount
    '
    ' History: 05/10/2005 JT - Created.
    '
    '**********************************************************************
    Private Function ConvertToBaseAmount(ByRef cAmount As Decimal, Optional ByRef iSourceID As Integer = 0) As Decimal
        Dim result As Decimal = 0

        Try
            ' Get an instance of the business object via
            ' the public object manager.


            Dim m_oCurrConverision As bACTCurrencyConvert.Form
            Dim r_vConversionRate As Double


            Dim temp_m_oCurrConverision As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrConverision, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oCurrConverision = temp_m_oCurrConverision

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("iSourceID", iSourceID)
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTCurrencyConvert.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=New Exception(Information.Err().Description), oDicParms:=oDict)

                Return result
            End If
            'To get the conversion Rate for selected transaction Currency

            m_lReturn = m_oCurrConverision.GetCurrencyRate(m_iMatchCurrencyId, iSourceID, m_dtEffectiveDate, r_vConversionRate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("iSourceID", iSourceID)
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oCurrConverision.GetCurrencyRate failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                Return result
            End If




            Return cAmount * r_vConversionRate

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConvertToBaseAmount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertToBaseAmount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

End Class
