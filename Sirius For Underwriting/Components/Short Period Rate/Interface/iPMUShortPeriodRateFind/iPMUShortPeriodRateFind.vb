Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Public Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 12/07/2000
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    'Developer Guide No. 19 (No Solution)
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
    Private m_lProductId As Integer

    'JMK 27/03/2001 Display the ProductCode
    Private m_sProductCode As String = ""

    'Variables to store data taken from the List View
    Private m_sSPRType As String = ""
    Private m_sSPRPeriod As String = ""
    Private m_lSPRValue As Integer
    Private m_dtSPREffectiveDate As Date
    Private m_dSPRPercentage As Decimal

    Private m_iAction As gPMConstants.PMEComponentAction
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUShortPeriodRateFind.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the IPT Interface object.

    Private m_oSPR As iPMUShortPeriodRate.Interface_Renamed

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    ' Stores the search data from the business object.
    Private m_vSearchData(,) As Object
    Private m_vShortPeriodRates(,) As Object
    'JMK 27/03/2001 Display the ProductCode
    Private m_vProductCode(,) As Object


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

    ' {* USER DEFINED CODE (Begin) *}
    Public WriteOnly Property ProductId() As Integer
        Set(ByVal Value As Integer)

            m_lProductId = Value

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

            ' {* USER DEFINED CODE (End) *}

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFormatDate, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFormatPercent, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=2)


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
    ' Name: GetList
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetList() As Integer

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

            'Get ProductCode

            m_lReturn = m_oBusiness.GetProductCode(r_vResultArray:=m_vProductCode, v_vProductId:=m_lProductId)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oBusiness.SearchAll(r_vResultArray:=m_vSearchData, v_vProductId:=m_lProductId)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)

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
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetList")

                    Return result
            End Select

            ' Display the number of item found message.
            DisplayStatusFound()

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Terminate", excep:=excep)

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'JMK 27/03/2001 Display the ProductCode
            'pnlProduct = m_lProductId
            If Information.IsArray(m_vProductCode) Then
                ' Update the interface details.
                m_sProductCode = CStr(m_vProductCode(0, 0))

                'Developer Guide No. 26
                lblProduct.Text = m_sProductCode.Trim()

            End If
            'JMK END

            ' Clear the search details.
            lvwShortPeriodRates.Items.Clear()

            ' Assign the details to the interface.
            If Information.IsArray(m_vSearchData) Then

                For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                    ' {* USER DEFINED CODE (Begin) *}

                    ' Assign the details to listview control

                    ' Column 1 Type
                    Select Case CStr(m_vSearchData(ACIType, lRow)).ToUpper()
                        Case "P"

                            oListItem = lvwShortPeriodRates.Items.Add("Premium", "")
                            ListViewHelper.SetListItemSmallIconProperty(oListItem, "FindImage")
                        Case "C"

                            oListItem = lvwShortPeriodRates.Items.Add("Cancellation", "")
                            ListViewHelper.SetListItemSmallIconProperty(oListItem, "FindImage")
                        Case Else
                            'default to Cancellation

                            oListItem = lvwShortPeriodRates.Items.Add("Cancellation", "")
                            ListViewHelper.SetListItemSmallIconProperty(oListItem, "FindImage")

                    End Select

                    ' Column 2 Period
                    Select Case CStr(m_vSearchData(ACIPeriod, lRow)).ToUpper()
                        Case "D"
                            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "Daily"
                        Case "W"
                            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "Weekly"
                        Case "M"
                            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "Monthly"
                        Case Else
                            'default to Monthly
                            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "Monthly"
                    End Select

                    'Column 3 Value
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vSearchData(ACIValue, lRow))

                    'Column 4 Effective Date
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFormatDate, vControlValue:=m_vSearchData(ACIEffectiveDate, lRow))

                    ' Check for errors
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to assign the data.
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtFormatDate.Text

                    'Column 5 Percentage
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFormatPercent, vControlValue:=m_vSearchData(ACIPercentage, lRow))

                    ' Check for errors
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to assign the data.
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = txtFormatPercent.Text

                    oListItem.Tag = CStr(lRow)

                    'ghosted deleted records
                    If m_vSearchData(ACIIsDeleted, lRow) = gPMConstants.PMEReturnCode.PMTrue Then
                        Me.lvwShortPeriodRates.Items.Item(lRow).Selected = True

                        'Developer Guide No. 12 (No Solution)
                        Me.lvwShortPeriodRates.SelectedItems.Item(0).ForeColor = Color.Gray
                    End If

                    ' Refresh the first X amount of rows, to
                    ' allow the user to see the results instantly.
                    If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                        ' Select the first item.
                        lvwShortPeriodRates.Items.Item(0).Selected = True

                        ' Refresh the initial results.
                        lvwShortPeriodRates.Refresh()
                    End If

                Next lRow
            End If

            ' Enable the interface now that the search
            ' has completed.
            m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)

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

            '    Resume

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataRefresh
    '
    ' Description: Populate Refresh storage
    '
    ' ***************************************************************** '
    Private Function DataRefresh() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get new details from baby form (SPR)

            ' {* USER DEFINED CODE (Begin) *}
            ' JMK 27/03/2001
            ' make sure new details will not violate PK on short_period_rates table
            'JMK START1
            'JMK 05/04/2001 - oops, move bValid outside loop else brand new item will fail
            Dim bValid As Boolean
            bValid = True
            Dim sMsgType, sMsgPeriod As String
            If Information.IsArray(m_vSearchData) Then
                Select Case m_iAction
                    Case gPMConstants.PMEComponentAction.PMAdd
                        For i As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                            If CStr(m_vSearchData(ACIType, i)) = m_sSPRType Then
                                If CStr(m_vSearchData(ACIPeriod, i)) = m_sSPRPeriod Then
                                    If CDbl(m_vSearchData(ACIValue, i)) = m_lSPRValue Then
                                        If CDate(m_vSearchData(ACIEffectiveDate, i)) = m_dtSPREffectiveDate Then
                                            bValid = False
                                            Exit For
                                        End If
                                    End If
                                End If
                            End If
                        Next i
                    Case gPMConstants.PMEComponentAction.PMEdit

                        lSelectedItem = Convert.ToString(lvwShortPeriodRates.Items.Item(lvwShortPeriodRates.FocusedItem.Index).Tag)
                        For i As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                            If i <> lSelectedItem Then
                                If CStr(m_vSearchData(ACIType, i)) = m_sSPRType Then
                                    If CStr(m_vSearchData(ACIPeriod, i)) = m_sSPRPeriod Then
                                        If CDbl(m_vSearchData(ACIValue, i)) = m_lSPRValue Then
                                            If CDate(m_vSearchData(ACIEffectiveDate, i)) = m_dtSPREffectiveDate Then
                                                bValid = False
                                                Exit For
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Next i
                End Select
            End If
            If bValid Then
                'JMK END1
                Select Case m_iAction
                    Case gPMConstants.PMEComponentAction.PMAdd
                        If Not Information.IsArray(m_vSearchData) Then
                            ReDim m_vSearchData(6, 0)
                            lSelectedItem = 0
                        Else
                            lSelectedItem = m_vSearchData.GetUpperBound(1) + 1
                            ReDim Preserve m_vSearchData(6, lSelectedItem)
                        End If

                        m_vSearchData(ACIType, lSelectedItem) = m_sSPRType
                        m_vSearchData(ACIPeriod, lSelectedItem) = m_sSPRPeriod
                        m_vSearchData(ACIValue, lSelectedItem) = m_lSPRValue
                        m_vSearchData(ACIEffectiveDate, lSelectedItem) = m_dtSPREffectiveDate
                        m_vSearchData(ACIPercentage, lSelectedItem) = m_dSPRPercentage
                        m_vSearchData(ACIIsDeleted, lSelectedItem) = gPMConstants.PMEReturnCode.PMFalse

                    Case gPMConstants.PMEComponentAction.PMEdit

                        lSelectedItem = Convert.ToString(lvwShortPeriodRates.Items.Item(lvwShortPeriodRates.FocusedItem.Index).Tag)

                        m_vSearchData(ACIType, lSelectedItem) = m_sSPRType
                        m_vSearchData(ACIPeriod, lSelectedItem) = m_sSPRPeriod
                        m_vSearchData(ACIValue, lSelectedItem) = m_lSPRValue
                        m_vSearchData(ACIEffectiveDate, lSelectedItem) = m_dtSPREffectiveDate
                        m_vSearchData(ACIPercentage, lSelectedItem) = m_dSPRPercentage

                End Select

                m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)
                'JMK START2
            Else
                sMsgType = IIf(m_sSPRType = "P", "Premium", "Cancellation")
                Select Case m_sSPRPeriod
                    Case "D"
                        sMsgPeriod = "Daily"
                    Case "W"
                        sMsgPeriod = "Weekly"
                    Case "M"
                        sMsgPeriod = "Monthly"
                End Select
                MessageBox.Show("These values already exist as an entry and will not be updated:" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                Strings.Chr(9) & sMsgType & Strings.Chr(13) & Strings.Chr(10) & _
                                Strings.Chr(9) & sMsgPeriod & Strings.Chr(13) & Strings.Chr(10) & _
                                Strings.Chr(9) & CStr(m_lSPRValue) & Strings.Chr(13) & Strings.Chr(10) & _
                                Strings.Chr(9) & DateTimeHelper.ToString(m_dtSPREffectiveDate), "Short Period Rate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
            'JMK END2
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Refresh Refreshs from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataRefresh", excep:=excep)

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
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the details from the business object.
    '
    '
    ' {* USER DEFINED CODE (Begin) *}
    '

    'm_lReturn = m_oBusiness.GetDetails()
    '
    ' {* USER DEFINED CODE (End) *}
    '
    ' Check for errors
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to get details.
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
    '
    'Return result
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
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", excep:=excep)

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


            m_lReturn = m_oBusiness.UpdateShortPeriodRate(v_vProductId:=m_lProductId, v_vShortPeriodRates:=m_vShortPeriodRates)


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
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", excep:=excep)

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


            m_lReturn = m_oBusiness.GetNext()

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", excep:=excep)

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
        Dim lRow2 As Integer
        Dim bFirst As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lRow2 = 0
            bFirst = True

            'loop thru current list and add everything to m_vShortPeriodRates array
            If Information.IsArray(m_vSearchData) Then
                For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                    If bFirst Then
                        ReDim m_vShortPeriodRates(6, lRow2)
                        bFirst = False
                    Else
                        lRow2 += 1
                        ReDim Preserve m_vShortPeriodRates(6, lRow2)
                    End If

                    m_vShortPeriodRates(ACIType, lRow2) = m_vSearchData(ACIType, lRow)
                    m_vShortPeriodRates(ACIPeriod, lRow2) = m_vSearchData(ACIPeriod, lRow)
                    m_vShortPeriodRates(ACIValue, lRow2) = m_vSearchData(ACIValue, lRow)
                    m_vShortPeriodRates(ACIEffectiveDate, lRow2) = m_vSearchData(ACIEffectiveDate, lRow)
                    m_vShortPeriodRates(ACIPercentage, lRow2) = m_vSearchData(ACIPercentage, lRow)
                    m_vShortPeriodRates(ACIIsDeleted, lRow2) = m_vSearchData(ACIIsDeleted, lRow)

                Next lRow
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", excep:=excep)

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
            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
            cmdAdd.Enabled = True

            ' {* USER DEFINED CODE (Begin) *}
            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwShortPeriodRates.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the column widths for the search list.
            lvwShortPeriodRates.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1400))
            lvwShortPeriodRates.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(720))
            lvwShortPeriodRates.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(720))
            lvwShortPeriodRates.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(1400))
            lvwShortPeriodRates.Columns.Item(4).Width = CInt(VB6.TwipsToPixelsX(1400))

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", excep:=excep)

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
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", excep:=excep)

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


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            SSTabHelper.SetTabCaption(tabMaintab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", excep:=excep)

            Return result

        End Try
    End Function
    ' PRIVATE Methods (End)


    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
        m_iAction = gPMConstants.PMEComponentAction.PMAdd
        cmdOK.Enabled = False
        cmdCancel.Enabled = False

        'Create Short Period Rate object if not already done so
        If m_oSPR Is Nothing Then

            ' Get an instance of the Short Period Rate interface object via
            ' the public object manager.
            Dim temp_m_oSPR As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oSPR, sClassName:="iPMUShortPeriodRate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oSPR = temp_m_oSPR

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get fee object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddIPT_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub

            End If

        End If

        m_lReturn = CType(m_oSPR.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        'pass default data to SPR object

        m_oSPR.SPRType = "C"

        m_oSPR.SPRPeriod = "M"

        m_oSPR.SPRValue = 0

        m_oSPR.SPREffectiveDate = DateTime.Today

        m_oSPR.SPRPercentage = 0.0#



        m_lReturn = m_oSPR.Start()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        Me.Refresh()

        cmdOK.Enabled = True
        cmdCancel.Enabled = True


        'If not cancelled, add to grid

        If m_oSPR.Status = gPMConstants.PMEReturnCode.PMCancel Then
            Exit Sub
        End If


        'get data back from SPR object

        m_sSPRType = m_oSPR.SPRType

        m_sSPRPeriod = m_oSPR.SPRPeriod

        m_lSPRValue = m_oSPR.SPRValue

        m_dtSPREffectiveDate = m_oSPR.SPREffectiveDate

        m_dSPRPercentage = m_oSPR.SPRPercentage


        m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)
        Exit Sub



        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Exit Sub
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click


        m_iAction = gPMConstants.PMEComponentAction.PMDelete


        Dim lSelectedItem As Integer = Convert.ToString(lvwShortPeriodRates.Items.Item(lvwShortPeriodRates.FocusedItem.Index).Tag)


        'Developer Guide No. 12 (No Solution)
        If Me.lvwShortPeriodRates.SelectedItems.Item(0).ForeColor = Color.Gray Then
            m_vSearchData(MainModule.ACIIsDeleted, lSelectedItem) = gPMConstants.PMEReturnCode.PMFalse
        Else
            m_vSearchData(MainModule.ACIIsDeleted, lSelectedItem) = gPMConstants.PMEReturnCode.PMTrue
        End If

        cmdCancel.Enabled = True
        cmdAdd.Enabled = True
        cmdOK.Enabled = True

        m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)

    End Sub


    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        Dim lSelectedItem As Integer
        'only allow to edit non-deleted records

        'Developer Guide No. 12 (No Solution)
        If Not Me.lvwShortPeriodRates.SelectedItems.Item(0).ForeColor = Color.Gray Then
            m_iAction = gPMConstants.PMEComponentAction.PMEdit
            cmdOK.Enabled = False
            cmdCancel.Enabled = False


            lSelectedItem = Convert.ToString(lvwShortPeriodRates.Items.Item(lvwShortPeriodRates.FocusedItem.Index).Tag)

            'Create Short Period Rate object if not already done so
            If m_oSPR Is Nothing Then

                ' Get an instance of the Short Period Rate interface object via
                ' the public object manager.
                Dim temp_m_oSPR As Object
                m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oSPR, "iPMUShortPeriodRate.Interface_Renamed", gPMConstants.PMGetLocalInterface)
                m_oSPR = temp_m_oSPR

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get Short Period Rate object", MainModule.ACApp, ACClass, "cmdEditIPT_Click", Information.Err().Number, Information.Err().Description)

                    Exit Sub

                End If

            End If
            'Developer Guide No. 37
            m_lReturn = m_oSPR.SetProcessModes(gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'pass selected details to SPR object


            ReflectionHelper.SetMember(m_oSPR, "SPRType", CStr(m_vSearchData(MainModule.ACIType, lSelectedItem)))


            ReflectionHelper.SetMember(m_oSPR, "SPRPeriod", CStr(m_vSearchData(MainModule.ACIPeriod, lSelectedItem)))

            ReflectionHelper.SetMember(m_oSPR, "SPRValue", CStr(m_vSearchData(MainModule.ACIValue, lSelectedItem)))

            'unformat data first
            Me.txtFormatDate.Text = CStr(m_vSearchData(MainModule.ACIEffectiveDate, lSelectedItem))


            'ReflectionHelper.SetMember(m_oSPR, "SPREffectivedate", m_oFormFields.UnformatControl(txtFormatDate))
            m_oSPR.SPREffectiveDate = m_oFormFields.UnformatControl(txtFormatDate)
            Me.txtFormatPercent.Text = CStr(m_vSearchData(MainModule.ACIPercentage, lSelectedItem))


            'ReflectionHelper.SetMember(m_oSPR, "sprpercentage", m_oFormFields.UnformatControl(txtFormatPercent))
            m_oSPR.SPRPercentage = m_oFormFields.UnformatControl(txtFormatPercent)


            'Developer Guide No. 37
            m_lReturn = m_oSPR.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If ReflectionHelper.GetMember(m_oSPR, "Status") = gPMConstants.PMEReturnCode.PMCancel Then
                cmdOK.Enabled = True
                cmdCancel.Enabled = True
                Exit Sub
            End If

            Me.Refresh()
            'get data back from SPR object

            m_sSPRType = ReflectionHelper.GetMember(m_oSPR, "SPRType")

            m_sSPRPeriod = ReflectionHelper.GetMember(m_oSPR, "SPRPeriod")

            m_lSPRValue = ReflectionHelper.GetMember(m_oSPR, "SPRValue")

            'm_dtSPREffectiveDate = ReflectionHelper.GetMember(m_oSPR, "SPREffectivedate")
            m_dtSPREffectiveDate = m_oSPR.SPREffectiveDate

            'm_dSPRPercentage = ReflectionHelper.GetMember(m_oSPR, "sprpercentage")
            m_dSPRPercentage = m_oSPR.SPRPercentage

            'Update the existing item
            m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to update the data.
                Exit Sub
            End If

            cmdOK.Enabled = True
            cmdCancel.Enabled = True

        End If

        Exit Sub



        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEdit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Exit Sub

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, lContextID:=MainModule.ScreenHelpID), gPMConstants.PMEReturnCode)

    End Sub

    ' PRIVATE Events (Begin)

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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRShortPeriodRate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMUShortPeriodRateFind.General()

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

            m_oBusiness.ProductId = m_lProductId
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
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

            ' Check for errors.

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            If Not (m_oSPR Is Nothing) Then
                ' Terminate the business object

                m_oSPR.Dispose()

                ' Check for errors.

                m_oSPR = Nothing

            End If

            ' Terminate the business object

		m_oBusiness.Dispose()

            ' Check for errors.

            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing


            ' Terminate the form control object.
		m_oFormFields.Dispose()

            ' Check for errors.

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
    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            '    stbStatus.SimpleText = " " & sMessage$

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

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            'stbStatus.SimpleText = " " & m_lItemsFound& & " " & sMessage$

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

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

            With tabMaintab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMaintab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMaintab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabMaintab, SSTabHelper.GetSelectedIndex(tabMaintab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMaintab, SSTabHelper.GetTabCount(tabMaintab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMaintab) < (SSTabHelper.GetTabCount(tabMaintab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabMaintab, SSTabHelper.GetSelectedIndex(tabMaintab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            '                    If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
                            '                         m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
                            '                    End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            '                    If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
                            '                         m_ctlTabFirstLast(ACControlEnd, .Tab).SetFocus
                            '                    End If
                        End If
                End Select
            End With

        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub lvwShortPeriodRates_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwShortPeriodRates.Click
        If Me.lvwShortPeriodRates.Items.Count <> 0 Then

            'Developer Guide No. 12 (No Solution)
            If Me.lvwShortPeriodRates.SelectedItems.Item(0).ForeColor = Color.Gray Then
                cmdDelete.Text = "UnDelete"
                cmdEdit.Enabled = False
            Else
                cmdDelete.Text = "Delete"
            End If
        End If
    End Sub

    Private Sub lvwShortPeriodRates_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwShortPeriodRates.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        'Not if we're viewing, thank you very much
        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If lvwShortPeriodRates.GetItemAt(x, y) Is Nothing Then
                cmdDelete.Enabled = False
                cmdAdd.Enabled = True
                cmdEdit.Enabled = False
            Else
                cmdEdit.Enabled = True
                cmdDelete.Enabled = True
            End If
        End If

    End Sub

    Private Sub tabMaintab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMaintab.SelectedIndexChanged

        Try

            With tabMaintab

                VB6.SetDefault(cmdOK, True)


            End With

        Catch





            tabMainTabPreviousTab = tabMaintab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            'm_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            '    If m_lReturn <> PMTrue Then
            '      Exit Sub
            '    End If

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
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' PRIVATE Events (End)
End Class
