Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Public Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 25/07/1997
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '
    'developer guide no.243(Changed iPMFunc.GetResData to GetResData in the whole document

    ' Constants:
    'developer guide no.7
    Public Const vbFormCode As Integer = 0
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    'Constants for resource data

    Private Const ACGridNotUsed As String = ""
    Private Const ACGridDefaultRate As Double = 0

    Private Const ACCurrencyCurrencyID As Integer = 0
    Private Const ACCurrencyDescription As Integer = 1
    Private Const ACCurrencyMaxColumns As Integer = 1

    Private Const ACBranchCompanyID As Integer = 0
    Private Const ACBranchDescription As Integer = 1
    Private Const ACBranchCurrencyID As Integer = 2
    Private Const ACBranchCurrencyDescription As Integer = 3
    Private Const ACBranchMaxColumns As Integer = 3

    Private Const ACTableEffectiveFrom As Integer = 0
    Private Const ACTableRateAgainstBase As Integer = 1
    Private Const ACTableCurrencyId As Integer = 2
    Private Const ACTableCompanyId As Integer = 3
    Private Const ACTableStatus As Integer = 4
    Private Const ACTableMaxColumns As Integer = 4


    Private m_bGridCancel As Boolean
    Private m_oGridData As XArrayHelper
    Private m_vTableData(,) As Object

    Private m_vCurrencies(,) As Object
    Private m_vBranches(,) As Object

    Private m_dtEffectiveFrom As Date
    Private m_iCompanyID As Integer
    Private m_iSystemCurrencyID As Integer

    Private m_iTypeOfRates As Integer
    Private m_bEdittingGrid As Boolean

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTCurrencyRate.General

    ' Declare an instance of the Business objects.
    'Private m_oBusiness As Object 'bACTCurrencyRate.Form
    Private m_oBusiness As bACTCurrencyRate.Form

    Private m_oCurrency As bACTCurrency.Form 'bACTCurrency.Form
    'Private m_oCompanyCurrency As Object 'bACTCompanyCurrency.Form
    Private m_oCompanyCurrency As bACTCompanyCurrency.Form

    Private m_oCompany As bACTCompany.Form 'bACTCompany.Form


    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' Last size variables for screen resizing
    Private m_lWidth As Integer
    Private m_lHeight As Integer
    'Added a variable to store the changed cell value
    Dim valueCng As String = ""

    Private nonNumberEntered As Boolean = False

    Private m_vGridMap As Hashtable

    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)
    'UPGRADE_NOTE: (7001) The following declaration (let ErrorNumber) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub ErrorNumber(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the current form's error number.
    'm_lErrorNumber = Value
    '
    'End Sub
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

    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

            'AG - 08/10/2004 - PN15639 - START
            'Allow editing of currency rates.
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then 'Set grid to edit mode.
                'changed as per the requirement
                'start
                'grdMainData.AllowUserToAddRows = True
                grdMainData.ReadOnly = False
                'grdMainData.AllowUserToDeleteRows = True
            ElseIf m_iTask = gPMConstants.PMEComponentAction.PMView Then  ' Set grid to view mode.
                grdMainData.AllowUserToAddRows = False
                grdMainData.ReadOnly = True
                grdMainData.AllowUserToDeleteRows = False
                'end
            End If
            'AG - 08/10/2004 - PN15639 - END

        End Set
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

    ' {* USER DEFINED CODE (Begin) *}

    Public Property CompanyId() As Integer
        Get

            Return m_iCompanyID

        End Get
        Set(ByVal Value As Integer)

            m_iCompanyID = Value

        End Set
    End Property
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

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

            ' Get the mandatory details from the business object.

            '    m_lReturn& = m_oBusiness.GetMandatory( _
            ''        lAccountIDMandatory:=lAccountIDMandatory, lPurgefrequencyIDMandatory:=lPurgefrequencyIDMandatory, _
            '
            '    Set m_oFormFields = New iPMFormControl.FormFields
            '
            '    m_oFormFields.LanguageID = g_iLanguageID%

            'Const ACGridEffectiveFrom = 0
            'Const ACGridRateAgainstBase = 1
            'Const ACGridRateAgainstEuro = 2
            'Const ACGridIsMultiplier = 3
            'Const ACGridCompanycurrencyID = 4
            'Const ACGridCurrencyID = 5
            'Const ACGridCompanyID = 6

            '    m_lReturn = m_oFormFields.AddNewFormField( _
            ''                 ctlControl:=grdMainData, _
            ''                 lFieldType:=PMDate, _
            ''                 lFormat:=PMFormatDateLong, _
            ''                 lGridColumn:=ACGridEffectiveFrom, _
            ''                 lMandatory:=PMMandatory)
            '
            '    If m_lReturn <> PMTrue Then
            '      SetFieldValidation = PMFalse
            '      Exit Function
            '    End If
            '
            '    m_lReturn = m_oFormFields.AddNewFormField( _
            ''                 ctlControl:=grdMainData, _
            ''                 lFieldType:=PMDouble, _
            ''                 lFormat:=PMFormatDouble, _
            ''                 lGridColumn:=ACGridRateAgainstBase, _
            ''                 lDecimalPlaces:=6, _
            ''                 lMandatory:=PMMandatory)
            '
            '    If m_lReturn <> PMTrue Then
            '      SetFieldValidation = PMFalse
            '      Exit Function
            '    End If
            '
            '    m_lReturn = m_oFormFields.AddNewFormField( _
            ''                 ctlControl:=grdMainData, _
            ''                 lFieldType:=PMDouble, _
            ''                 lFormat:=PMFormatDouble, _
            ''                 lGridColumn:=ACGridRateAgainstEuro, _
            ''                 lMandatory:=PMMandatory)
            '
            '    If m_lReturn <> PMTrue Then
            '      SetFieldValidation = PMFalse
            '      Exit Function
            '    End If




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
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer
        Dim result As Integer = 0
        Dim sCaption As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'If this is the first time in then get all business details
            If m_iTypeOfRates = 0 Then

                m_lReturn = m_oBusiness.GetTypeOfRates(m_iTypeOfRates)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                Select Case m_iTypeOfRates
                    Case 1 'Only one set of rates for all branches, stored under branch one.
                        m_iCompanyID = 1
                    Case 3 'Multi ledger, only ever show rates for branch that user is logged in under.
                        If m_iCompanyID = 0 Then 'Unless a specific company has been selected.
                            m_iCompanyID = g_iSourceID
                        End If
                End Select

                m_lReturn = GetBranchesInfo()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Set the forms caption.

                sCaption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFormCaption1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                If m_iCompanyID = 0 Then

                    sCaption = sCaption & CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFormCaption2Each, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                ElseIf m_iTypeOfRates = 1 Then

                    sCaption = sCaption & CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFormCaption2All, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                Else

                    sCaption = sCaption & CStr(m_vBranches(ACBranchDescription, 0))
                End If
                sCaption = sCaption.Trim()
                Me.Text = sCaption
                cboBranch.FirstItem = "(All Branches)"
                cboBranch.ItemId = m_iCompanyID


                m_lReturn = GetCurrenciesInfo()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = SetGridInterfaceDefaults()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Get the currency rate details from the business object.
            'DC010305 : PN19109 : use company based on rate type rather than one actually selected

            m_lReturn = m_oBusiness.GetDetails(v_lCompanyID:=cboBranch.ItemId, v_dtEffectiveFrom:=m_dtEffectiveFrom)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_iTypeOfRates = 1 Then

                cboBranch.Visible = False
                lblAllBranches.Left = cboBranch.Left
                lblAllBranches.Top = lblBranch.Top
                lblAllBranches.Height = lblBranch.Height
                lblAllBranches.Text = "All Branches"
                lblAllBranches.Visible = True
            End If


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
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim lMoreRecords As gPMConstants.PMEReturnCode

        Dim dtEffectiveFrom As Date
        Dim dRateAgainstBase As Double
        Dim iCurrencyId, iCompanyId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lRow = 0
            ReDim m_vTableData(0, 0)

            ' Retrieve all of the details from the business object.

            lMoreRecords = m_oBusiness.GetNext(vEffectiveFrom:=dtEffectiveFrom, vRateAgainstBase:=dRateAgainstBase, vCurrencyID:=iCurrencyId, vCompanyID:=iCompanyId)

            Do While lMoreRecords = gPMConstants.PMEReturnCode.PMTrue
                If m_vTableData.GetUpperBound(0) = 0 Then
                    ReDim m_vTableData(ACTableMaxColumns, lRow)
                Else
                    ReDim Preserve m_vTableData(ACTableMaxColumns, lRow)
                End If


                m_vTableData(ACTableEffectiveFrom, lRow) = dtEffectiveFrom

                m_vTableData(ACTableRateAgainstBase, lRow) = dRateAgainstBase

                m_vTableData(ACTableCurrencyId, lRow) = iCurrencyId

                m_vTableData(ACTableCompanyId, lRow) = iCompanyId

                lRow += 1


                lMoreRecords = m_oBusiness.GetNext(vEffectiveFrom:=dtEffectiveFrom, vRateAgainstBase:=dRateAgainstBase, vCurrencyID:=iCurrencyId, vCompanyID:=iCompanyId)
            Loop

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in BusinessToData", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function DataToInterface() As Integer
        Dim result As Integer = 0
        Dim bFound As Boolean
        Dim lNumberOfRecords As Integer
        Dim vResultArray(,) As Object
        Dim bUseCurrency As Boolean
        Dim gridMapKey As String
        Dim colorValue As Color
        colorValue = SystemColors.Control
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oGridData = New XArrayHelper()
            m_vGridMap = New Hashtable()

            m_oGridData.RedimXArray(New Integer() {m_vCurrencies.GetUpperBound(1), m_vBranches.GetUpperBound(1) + 1}, New Integer() {0, 0})

            For lCurrentCol As Integer = 0 To m_vBranches.GetUpperBound(1) + 1
                If lCurrentCol = 0 Then
                    For lCurrentRow As Integer = 0 To m_vCurrencies.GetUpperBound(1)

                        'Added trim check to remove blank spaces.
                        'm_oGridData(lCurrentRow, lCurrentCol) = m_vCurrencies(ACCurrencyDescription, lCurrentRow)
                        m_oGridData(lCurrentRow, lCurrentCol) = gPMFunctions.ToSafeString(m_vCurrencies(ACCurrencyDescription, lCurrentRow)).Trim()
                    Next
                Else
                    'Get all of the valid currencies for this branch.
                    lNumberOfRecords = 0


                    m_oCompanyCurrency.CompanyId = m_vBranches(ACBranchCompanyID, lCurrentCol - 1)

                    m_lReturn = m_oCompanyCurrency.GetCompanyCurrencies(lNumberOfRecords:=lNumberOfRecords, vResultArray:=vResultArray, vnMode:=0)

                    For lCurrentRow As Integer = 0 To m_vCurrencies.GetUpperBound(1)
                        'Is the current currency a valid one for this branch.
                        bUseCurrency = False

                        For lLoop As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)



                            If CInt(vResultArray(0, lLoop)) = CDbl(m_vCurrencies(ACCurrencyCurrencyID, lCurrentRow)) And CInt(vResultArray(0, lLoop)) <> CDbl(m_vBranches(ACBranchCurrencyID, lCurrentCol - 1)) Then

                                bUseCurrency = True
                                Exit For
                            End If
                        Next
                        'Also use the currency if it is the system currency.

                        If CDbl(m_vCurrencies(ACCurrencyCurrencyID, lCurrentRow)) = m_iSystemCurrencyID Then
                            'But not if it is also the branch currency as it will always have a rate of 1.

                            bUseCurrency = Not (CDbl(m_vBranches(ACBranchCurrencyID, lCurrentCol - 1)) = m_iSystemCurrencyID)
                        End If

                        If bUseCurrency Then
                            'Used. Default rate.
                            m_oGridData(lCurrentRow, lCurrentCol) = ACGridDefaultRate
                        Else
                            'Not used. Blank the cell.
                            m_oGridData(lCurrentRow, lCurrentCol) = ACGridNotUsed
                        End If
                    Next
                End If
            Next

            If m_vTableData.GetUpperBound(0) = ACTableMaxColumns Then
                For lLoop As Integer = 0 To m_vTableData.GetUpperBound(1)

                    bFound = False
                    For lCurrentRow As Integer = 0 To m_vCurrencies.GetUpperBound(1)


                        If m_vTableData(ACTableCurrencyId, lLoop).Equals(m_vCurrencies(ACCurrencyCurrencyID, lCurrentRow)) Then
                            For lCurrentCol As Integer = 1 To m_vBranches.GetUpperBound(1) + 1


                                'changes for performance enhancement start
                                'mapped m_vTableData with m_oGridData in Hashtable
                                If m_vTableData(ACTableCompanyId, lLoop).Equals(m_vBranches(ACBranchCompanyID, lCurrentCol - 1)) Then
                                    gridMapKey = lCurrentRow.ToString() + "|" + lCurrentCol.ToString()
                                    If Not m_vGridMap.ContainsKey(gridMapKey) Then
                                        m_vGridMap.Add(gridMapKey, lLoop)
                                    End If
                                End If
                                'changes for performance enhancement end

                                If m_vTableData(ACTableCompanyId, lLoop).Equals(m_vBranches(ACBranchCompanyID, lCurrentCol - 1)) And Not m_vCurrencies(ACCurrencyDescription, lCurrentRow).Equals(m_vBranches(ACBranchCurrencyDescription, lCurrentCol - 1)) Then


                                    m_oGridData(lCurrentRow, lCurrentCol) = m_vTableData(ACTableRateAgainstBase, lLoop)
                                    bFound = True
                                    Exit For
                                End If
                            Next
                        End If
                        If bFound Then
                            Exit For
                        End If
                    Next

                Next
            End If


            'Dim bindingSource As BindingSource = New BindingSource(m_oGridData, "")
            grdMainData.AutoGenerateColumns = False
            grdMainData.DataSource = m_oGridData
            For index As Integer = 0 To grdMainData.ColumnCount - 1
                grdMainData.Columns(index).DataPropertyName = "Column" + (index + 1).ToString()
            Next
            'grdMainData.ReBind()

            'For Each dr As DataGridViewRow In grdMainData.Rows
            '    For Each cel As DataGridViewCell In dr.Cells
            '        If String.IsNullOrEmpty(cel.Value) Then
            '            cel.Style.BackColor = colorValue
            '            cel.Style.SelectionBackColor = colorValue
            '            cel.ReadOnly = True

            '        End If

            '    Next
            'Next
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in DataToInterface", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function BusinessToInterface() As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = BusinessToData()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = DataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in BusinessToInterface", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function InterfaceToBusiness() As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = InterfaceToData()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = DataToBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in InterfaceToBusiness", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function InterfaceToData() As Integer
        Dim result As Integer = 0
        Dim lTableLoop As Integer
        Dim bFound, bZeroUsed As Boolean
        Dim gridMapKey As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Update any outstanding edits.
            'grdMainData.UpdateCurrentRow()
            m_oGridData.AcceptChanges()

            bZeroUsed = False

            For lCurrentRow As Integer = 0 To m_oGridData.GetUpperBound(0)
                For lCurrentCol As Integer = 1 To m_oGridData.GetUpperBound(1)
                    gridMapKey = lCurrentRow.ToString() + "|" + lCurrentCol.ToString()
                    lTableLoop = -1  ' if lTableLoop  remains -1 then new branch/currency is added
                    If m_vGridMap.ContainsKey(gridMapKey) Then
                        lTableLoop = m_vGridMap(gridMapKey)
                    End If

                    If gPMFunctions.ToSafeString(m_oGridData(lCurrentRow, lCurrentCol)) <> ACGridNotUsed Then
                        If lTableLoop <> -1 AndAlso m_vTableData(ACTableEffectiveFrom, lTableLoop) = DateTime.ParseExact(m_dtEffectiveFrom.ToString("yyyy-dd-MM"), "yyyy-dd-MM", System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")) Then
                            m_vTableData(ACTableRateAgainstBase, lTableLoop) = m_oGridData(lCurrentRow, lCurrentCol)
                            If m_vTableData(ACTableStatus, lTableLoop) <> gPMConstants.PMEComponentAction.PMAdd Then
                                m_vTableData(ACTableStatus, lTableLoop) = gPMConstants.PMEComponentAction.PMEdit
                            End If
                            If gPMFunctions.ToSafeDouble(m_vTableData(ACTableRateAgainstBase, lTableLoop)) <= 0 Then
                                bZeroUsed = True
                            End If
                        Else
                            If m_vTableData.GetUpperBound(0) = ACTableMaxColumns Then
                                lTableLoop = m_vTableData.GetUpperBound(1) + 1
                                ReDim Preserve m_vTableData(ACTableMaxColumns, lTableLoop)
                            Else
                                lTableLoop = 0
                                ReDim m_vTableData(ACTableMaxColumns, lTableLoop)
                            End If
                            m_vGridMap.Remove(gridMapKey)
                            m_vGridMap.Add(gridMapKey, lTableLoop)
                            m_vTableData(ACTableEffectiveFrom, lTableLoop) = m_dtEffectiveFrom

                            ' developer guide no. 188
                            m_vTableData(ACTableRateAgainstBase, lTableLoop) = m_oGridData(lCurrentRow, lCurrentCol)


                            m_vTableData(ACTableCurrencyId, lTableLoop) = m_vCurrencies(ACCurrencyCurrencyID, lCurrentRow)


                            m_vTableData(ACTableCompanyId, lTableLoop) = m_vBranches(ACBranchCompanyID, lCurrentCol - 1)

                            m_vTableData(ACTableStatus, lTableLoop) = gPMConstants.PMEComponentAction.PMAdd

                        End If
                    Else
                        If lTableLoop = -1 OrElse m_vTableData(ACTableEffectiveFrom, lTableLoop) <> DateTime.ParseExact(m_dtEffectiveFrom.ToString("yyyy-dd-MM"), "yyyy-dd-MM", System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")) Then
                            If m_vTableData.GetUpperBound(0) = ACTableMaxColumns Then
                                lTableLoop = m_vTableData.GetUpperBound(1) + 1
                                ReDim Preserve m_vTableData(ACTableMaxColumns, lTableLoop)
                            Else
                                lTableLoop = 0
                                ReDim m_vTableData(ACTableMaxColumns, lTableLoop)
                            End If
                            m_vGridMap.Remove(gridMapKey)
                            m_vGridMap.Add(gridMapKey, lTableLoop)
                            m_vTableData(ACTableEffectiveFrom, lTableLoop) = m_dtEffectiveFrom

                            ' developer guide no. 188
                            m_vTableData(ACTableRateAgainstBase, lTableLoop) = 1


                            m_vTableData(ACTableCurrencyId, lTableLoop) = m_vBranches(ACBranchCurrencyID, lCurrentCol - 1)


                            m_vTableData(ACTableCompanyId, lTableLoop) = m_vBranches(ACBranchCompanyID, lCurrentCol - 1)

                            m_vTableData(ACTableStatus, lTableLoop) = gPMConstants.PMEComponentAction.PMAdd

                        End If

                    End If

                    'Check to ensure that zeros have not been used in the grid
                    'Added the following check to check for empty string
                    'start
                    If gPMFunctions.ToSafeString(m_oGridData(lCurrentRow, lCurrentCol)) <> "" Then
                        'developer guide no. 188 & 248
                        If gPMFunctions.ToSafeDouble(m_oGridData(lCurrentRow, lCurrentCol)) <= 0 Then
                            bZeroUsed = True
                        End If
                    End If
                    'end
                Next
            Next

            'The user cannot continue if there are invalid rates
            If bZeroUsed Then
                MessageBox.Show("You cannot enter rates of zero.", "Invalid Rate entered", MessageBoxButtons.OK, MessageBoxIcon.Error)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in InterfaceToData", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToBusiness
    '
    ' Description: Updates all business members from the data storage.
    '
    ' ***************************************************************** '
    Public Function DataToBusiness() As Integer

        Dim result As Integer = 0
        Dim lCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_vTableData.GetUpperBound(0) = ACTableMaxColumns Then
                For lRow As Integer = 0 To m_vTableData.GetUpperBound(1)


                    m_lReturn = m_oBusiness.GetCount(v_lCount:=lCount)
                    If m_vTableData(ACTableStatus, lRow) = gPMConstants.PMEComponentAction.PMAdd And lCount = lRow Then


                        m_lReturn = m_oBusiness.EditAdd(lRow:=lRow + 1, vEffectiveFrom:=m_vTableData(ACTableEffectiveFrom, lRow), vRateAgainstBase:=m_vTableData(ACTableRateAgainstBase, lRow), vCurrencyID:=m_vTableData(ACTableCurrencyId, lRow), vCompanyID:=m_vTableData(ACTableCompanyId, lRow))

                    ElseIf m_vTableData(ACTableStatus, lRow) = gPMConstants.PMEComponentAction.PMEdit Or m_vTableData(ACTableStatus, lRow) = gPMConstants.PMEComponentAction.PMAdd And lCount <> lRow Then

                        m_lReturn = m_oBusiness.EditUpdate(lRow:=lRow + 1, vEffectiveFrom:=m_vTableData(ACTableEffectiveFrom, lRow), vRateAgainstBase:=m_vTableData(ACTableRateAgainstBase, lRow), vCurrencyID:=m_vTableData(ACTableCurrencyId, lRow), vCompanyID:=m_vTableData(ACTableCompanyId, lRow))
                    End If

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Next
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetGridInterfaceDefaults
    '
    ' Description: Sets all of the grid default values.
    '
    ' ***************************************************************** '
    Public Function SetGridInterfaceDefaults() As Integer
        Dim result As Integer = 0
        Dim sHeader As String = ""
        Dim oColumn As DataGridViewColumn

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Add all of the needed colums and assign their headers.
            For lLoop As Integer = 0 To m_vBranches.GetUpperBound(1) + 1
                If lLoop = 0 Then
                    sHeader = ""
                Else

                    sHeader = CStr(m_vBranches(ACBranchDescription, lLoop - 1)).Trim() & Strings.Chr(13) & Strings.Chr(10) & CStr(m_vBranches(ACBranchCurrencyDescription, lLoop - 1)).Trim()
                End If

                'added here
                Dim newColumn As Windows.Forms.DataGridViewTextBoxColumn = Nothing
                newColumn = New Windows.Forms.DataGridViewTextBoxColumn()
                grdMainData.Columns.Insert(lLoop, newColumn)

                'developer guide no.162
                If lLoop > 0 Then
                    'commented and added above as new column is to be created before assigning any property to it.
                    oColumn = newColumn
                    oColumn.Visible = True
                    oColumn.DefaultCellStyle.Alignment = ContentAlignment.MiddleCenter
                    'developer guide no.(Due to VB to Dotnet conversion)
                    oColumn.Width = VB6.TwipsToPixelsX(1750)

                    'TODO: (Added three lines below)
                    oColumn.HeaderCell.Style.Font = VB6.FontChangeName(grdMainData.ColumnHeadersDefaultCellStyle.Font, "Verdana")
                    oColumn.DefaultCellStyle.Font = VB6.FontChangeName(grdMainData.DefaultCellStyle.Font, "VerDana")
                    oColumn.DefaultCellStyle.Font = VB6.FontChangeSize(grdMainData.DefaultCellStyle.Font, 7.5)
                    oColumn.Resizable = DataGridViewTriState.True
                End If
                'Added the following check for the First to matc\h design with VB Application
                'start
                If lLoop = 0 Then
                    grdMainData.Columns(lLoop).Frozen = True
                    grdMainData.Columns(lLoop).ReadOnly = True
                    grdMainData.DefaultCellStyle.Font = VB6.FontChangeName(grdMainData.DefaultCellStyle.Font, "Verdana")
                    grdMainData.DefaultCellStyle.Font = VB6.FontChangeSize(grdMainData.DefaultCellStyle.Font, 7.5)
                    'grdMainData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                End If
                'start
                grdMainData.Columns(lLoop).HeaderText = sHeader

            Next
            'Added the following code to match the design according to the VB Application
            'start
            grdMainData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            grdMainData.EditMode = DataGridViewEditMode.EditOnKeystroke
            grdMainData.ColumnHeadersDefaultCellStyle.BackColor = Color.White
            grdMainData.RowHeadersDefaultCellStyle.BackColor = Color.White
            grdMainData.BackgroundColor = Color.White
            grdMainData.RowHeadersVisible = False
            'end
            'Set grid to use the xarray and refresh it.
            'grdMainData.RowsCount = m_vCurrencies.GetUpperBound(1) + 1


            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the grid defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetGridInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

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
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                ' Set grid to view mode.
                grdMainData.AllowUserToAddRows = False
                grdMainData.ReadOnly = False
                grdMainData.AllowUserToDeleteRows = False
            End If
            'm_dtEffectiveFrom = DateTime.ParseExact(DateTime.Today.ToString("yyyy-dd-MM"), "yyyy-dd-MM", Threading.Thread.CurrentThread.CurrentCulture)
            m_dtEffectiveFrom = DateTime.Parse(DateTime.Today.Date, Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat)

            dtpEffectiveDate.CustomFormat = Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern
            dtpEffectiveDate.Format = DateTimePickerFormat.Custom
            dtpEffectiveDate.Value = DateTime.Today
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
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions


            divFilter.Caption = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFilterFrameCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBranch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBranchCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblEffectiveDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEffectiveDateCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdApply.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACApplyCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Sub cmdEffectiveDateBack_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEffectiveDateBack.Click
        m_lReturn = m_oBusiness.GetNextEffectiveDate(v_bNext:=False, v_iCompanyID:=m_iCompanyID, r_dtEffectiveDate:=m_dtEffectiveFrom)
        If dtpEffectiveDate.Value <> m_dtEffectiveFrom Then
            dtpEffectiveDate.Value = m_dtEffectiveFrom
            UpdateGrid()
        End If
    End Sub

    Private Sub cmdEffectiveDateForward_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEffectiveDateForward.Click


        m_lReturn = m_oBusiness.GetNextEffectiveDate(v_bNext:=True, v_iCompanyID:=m_iCompanyID, r_dtEffectiveDate:=m_dtEffectiveFrom)

        If dtpEffectiveDate.Value <> m_dtEffectiveFrom Then
            dtpEffectiveDate.Value = m_dtEffectiveFrom
            UpdateGrid()
        End If

    End Sub

    Private Sub dtpEffectiveDate_ValueChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles dtpEffectiveDate.ValueChanged

        m_dtEffectiveFrom = dtpEffectiveDate.Value
        UpdateGrid()
    End Sub

    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        iPMFunc.ShowFormInTaskBar_Attach()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            '<<<<<<<<<<<<<<<
            'begin temp(JY)
            '    Set m_oBusiness = New bACTCurrencyRate.Form
            '
            '    g_iLanguageID% = 1
            '    g_iSourceID% = 1
            '    g_iCompanyID% = 1
            '    m_lReturn& = m_oBusiness.Initialise(sUsername:="Basilb", sPassword:="Basilb", iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=1, sCallingAppName:="iACTCurrencyRate")

            'end temp
            '>>>>>>>>>>>>>>>
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTCurrencyRate.Form", vInstanceManager:="ClientManager")
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

            Dim temp_m_oCurrency As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrency, "bACTCurrency.Form", vInstanceManager:="ClientManager")
            m_oCurrency = temp_m_oCurrency

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            Dim temp_m_oCompanyCurrency As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCompanyCurrency, "bACTCompanyCurrency.Form", vInstanceManager:="ClientManager")
            m_oCompanyCurrency = temp_m_oCompanyCurrency

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            Dim temp_m_oCompany As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCompany, "bACTCompany.Form", vInstanceManager:="ClientManager")
            m_oCompany = temp_m_oCompany

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTCurrencyRate.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If
            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set resize details for form controls
            SetResize()

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

            cboBranch.FirstItem = "(All Branches)"
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = SetFieldValidation()

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
            ' {* USER DEFINED CODE (End) *}


            m_lReturn = m_oCurrency.GetSystemCurrency(m_iSystemCurrencyID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

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

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then
                'Call the update method to flush any
                'new data currently being added.
                'grdMainData.UpdateCurrentRow()

                'Process the next set of actions depending
                'upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                'Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Do not procced with the interface termination.
                    'developer guide no.7
                    eventArgs.Cancel = True
                    Cancel = 1

                    'Set the mouse pointer to normal.
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

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
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

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                RemoveHandler MyBase.FormClosing, AddressOf frmInterface_FormClosing
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            ' Enforce minimum sizes
            If Me.WindowState = FormWindowState.Normal Then
                If VB6.PixelsToTwipsX(Width) < 9690 Then Width = VB6.TwipsToPixelsX(9690)
                If VB6.PixelsToTwipsY(Height) < 5385 Then Height = VB6.TwipsToPixelsY(5385)
            End If

            If Me.WindowState <> FormWindowState.Minimized Then
                ' Resize the screen
                uctAnchor.Resize_Renamed(m_lWidth, m_lHeight, CInt(VB6.PixelsToTwipsX(ClientRectangle.Width)), CInt(VB6.PixelsToTwipsY(ClientRectangle.Height)))

                ' Store last sizes
                m_lWidth = CInt(VB6.PixelsToTwipsX(ClientRectangle.Width))
                m_lHeight = CInt(VB6.PixelsToTwipsY(ClientRectangle.Height))
            End If

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub





    Private Function GetCurrenciesInfo() As Integer
        Dim result As Integer = 0
        Dim lCurrencyCount As Integer
        Dim vCurrencyID, vDescription As Object
        Dim vIsDeleted As Byte
        Dim lNumberOfRecords As Integer
        Dim vResultArray(,) As Object
        Dim bUseCurrency As Boolean
        Dim lMoreRecords As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lCurrencyCount = 0


            If m_iCompanyID <> 0 Then
                lNumberOfRecords = 0

                m_oCompanyCurrency.CompanyId = m_iCompanyID

                m_lReturn = m_oCompanyCurrency.GetCompanyCurrencies(lNumberOfRecords:=lNumberOfRecords, vResultArray:=vResultArray, vnMode:=1)
            End If


            m_lReturn = m_oCurrency.GetDetails()


            lMoreRecords = m_oCurrency.GetNext(vCurrencyID:=vCurrencyID, vDescription:=vDescription, vIsDeleted:=vIsDeleted)

            Do While lMoreRecords = gPMConstants.PMEReturnCode.PMTrue

                If vIsDeleted <> 1 Then

                    If m_iCompanyID <> 0 Then
                        bUseCurrency = False

                        For lLoop As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)


                            If CInt(vResultArray(0, lLoop)) = CDbl(vCurrencyID) Then
                                bUseCurrency = True
                                Exit For
                            End If
                        Next
                    End If


                    If CDbl(vCurrencyID) = m_iSystemCurrencyID Then
                        bUseCurrency = True
                    End If

                    If m_iCompanyID = 0 Or bUseCurrency Then
                        ReDim Preserve m_vCurrencies(ACCurrencyMaxColumns, lCurrencyCount)


                        m_vCurrencies(ACCurrencyCurrencyID, lCurrencyCount) = vCurrencyID


                        m_vCurrencies(ACCurrencyDescription, lCurrencyCount) = vDescription
                        lCurrencyCount += 1
                    End If

                End If


                lMoreRecords = m_oCurrency.GetNext(vCurrencyID:=vCurrencyID, vDescription:=vDescription, vIsDeleted:=vIsDeleted)
            Loop

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrenciesInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetBranchesInfo() As Integer
        Dim result As Integer = 0
        Dim lBranchCount As Integer
        Dim vCompanyID, vBaseCurrency As Object
        Dim vDescription As String = ""
        Dim vCurrencyDescription As Object
        Dim lMoreRecords As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lBranchCount = 0

            If m_iCompanyID = 0 Then

                m_lReturn = m_oCompany.GetDetails()
            Else

                m_lReturn = m_oCompany.GetDetails(vCompanyID:=m_iCompanyID)
            End If


            lMoreRecords = m_oCompany.GetNext(vCompanyID:=vCompanyID, vBaseCurrency:=vBaseCurrency, vDescription:=vDescription)

            Do While lMoreRecords = gPMConstants.PMEReturnCode.PMTrue

                ReDim Preserve m_vBranches(ACBranchMaxColumns, lBranchCount)



                m_vBranches(ACBranchCompanyID, lBranchCount) = vCompanyID
                If m_iTypeOfRates = 1 Then
                    vDescription = "All Branches"
                End If

                m_vBranches(ACBranchDescription, lBranchCount) = vDescription


                m_vBranches(ACBranchCurrencyID, lBranchCount) = vBaseCurrency


                m_lReturn = m_oCurrency.GetDetails(vCurrencyID:=vBaseCurrency)

                m_lReturn = m_oCurrency.GetNext(vDescription:=vCurrencyDescription)



                m_vBranches(ACBranchCurrencyDescription, lBranchCount) = vCurrencyDescription

                lBranchCount += 1


                lMoreRecords = m_oCompany.GetNext(vCompanyID:=vCompanyID, vBaseCurrency:=vBaseCurrency, vDescription:=vDescription)

            Loop

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranchesInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function UpdateGrid() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Retrieve the interface details with the current effective date.
            m_lReturn = GetBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Apply business details to the interface.
            m_lReturn = BusinessToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateGrid", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function Validate_Renamed() As Integer
        Dim result As Integer = 0
        Dim bEdit As Boolean
        Dim sTitle, sMessage As String
        Dim vReply As DialogResult

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Check that all fields for system currency are filled in.
            For lLoop As Integer = 0 To m_vTableData.GetUpperBound(1)


                If CDbl(m_vTableData(ACTableCurrencyId, lLoop)) = m_iSystemCurrencyID And CDbl(m_vTableData(ACTableRateAgainstBase, lLoop)) = ACGridDefaultRate Then


                    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingRatesTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMissingRates, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next

            'Are we adding a new set of rates or editting and existing set.
            bEdit = False
            For lLoop As Integer = m_vTableData.GetLowerBound(1) To m_vTableData.GetUpperBound(1)
                If m_vTableData(ACTableStatus, lLoop) = gPMConstants.PMEComponentAction.PMEdit Then
                    bEdit = True
                    Exit For
                End If
            Next

            'Display confirmation message to the user.
            If bEdit Then

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReplaceRatesTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReplaceRates, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCreateRatesTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCreateRates, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            sMessage = sMessage.Replace("XXXXX", DateTime.Parse(m_dtEffectiveFrom).ToString("d"))

            vReply = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
            If vReply <> System.Windows.Forms.DialogResult.Yes Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Validate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***********************************************************
    ' Set the resizing anchors
    ' ***********************************************************
    Private Sub SetResize()

        Try

            ' Set start dimensions
            m_lWidth = CInt(VB6.PixelsToTwipsX(ClientRectangle.Width))
            m_lHeight = CInt(VB6.PixelsToTwipsY(ClientRectangle.Height))

            ' Search Block
            uctAnchor.Add(divFilter, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)

            ' Transaction Block
            uctAnchor.Add(divRates, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(grdMainData, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)

            ' Control Buttons
            uctAnchor.Add(cmdOK, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdApply, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            uctAnchor.Add(cmdCancel, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)

        Catch
        End Try




    End Sub

  

    Private Sub grdMainData_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdMainData.CellEndEdit
        If Not (grdMainData.CurrentCell.Value) Is Nothing Then
            If grdMainData.CurrentCell.Value.ToString() = "" Then
                grdMainData.CurrentCell.Value = 0
            End If
        End If
        
    End Sub


    Private Sub grdMainData_EditingControlShowing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles grdMainData.EditingControlShowing
        AddHandler e.Control.KeyPress, AddressOf grdMainData_KeyPress
        AddHandler e.Control.KeyDown, AddressOf grdMainData_KeyDown

    End Sub

    Private Sub grdMainData_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        ' Initialize the flag to false.
        nonNumberEntered = False

        ' Determine whether the keystroke is a number  from the top of the keyboard.
        If e.KeyCode < Keys.D0 OrElse e.KeyCode > Keys.D9 Then
            ' Determine whether the keystroke is a number from the keypad.
            If e.KeyCode < Keys.NumPad0 OrElse e.KeyCode > Keys.NumPad9 Then
                ' Determine whether the keystroke is a backspace.
                If e.KeyCode <> Keys.Back Then
                    ' A non-numerical keystroke was pressed. 
                    ' Set the flag to true and evaluate in KeyPress event.
                    nonNumberEntered = True
                End If
            End If
        End If
        'decimal and ctrl keys
        If e.KeyCode = Keys.Decimal OrElse e.KeyCode = Keys.RControlKey OrElse e.KeyCode = Keys.LControlKey OrElse e.KeyCode = Keys.OemPeriod Then
            nonNumberEntered = False
        End If

    End Sub

    Private Sub grdMainData_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        ' Check for the flag being set in the KeyDown event.
        If nonNumberEntered = True Then
            ' Stop the character from being entered into the control since it is non-numerical.
            e.Handled = True
        End If

    End Sub

  
    Private Sub grdMainData_CellFormatting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles grdMainData.CellFormatting
        Dim colorValue As Color
        colorValue = SystemColors.Control
        If e.Value IsNot Nothing Then
            If String.IsNullOrEmpty(e.Value) Then
                e.CellStyle.BackColor = colorValue
                grdMainData.Rows(e.RowIndex).Cells(e.ColumnIndex).ReadOnly = True
            End If
        End If
    End Sub
End Class
