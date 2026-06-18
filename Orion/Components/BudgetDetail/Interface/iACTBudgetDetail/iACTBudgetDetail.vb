Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 27/10/1998
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"


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

    Private m_lBudgetDetailID As Integer
    Private m_lBudgetID As Integer
    Private m_lBudgetSequence As Integer
    Private m_lPeriodID As Integer
    Private m_lAccountID As Integer
    Private m_cBudgetAmount As Decimal
    Private m_cActualAmount As Decimal
    Private m_cVarianceAmount As Decimal

    Private m_sPeriodYearName As String = ""

    Private m_lRevisesBudgetID As Integer
    Private m_lBasedOnBudgetID As Integer

    Private m_oFindAccount As Object

    Private m_vDetailsArray(,) As Object

    ' Data contained in grid
    Private m_vGridData(,,) As Object

    ' Start and end columns for the periods
    Private m_iStartColumn As Integer
    Private m_iEndColumn As Integer

    ' Current column and row on the grid
    Private m_lCurrRow As Integer
    Private m_lCurrColumn As Integer

    ' Array to store the account ids in
    Private m_vAccountIDs(,) As Object

    ' Processing a cell at present
    Private m_bProcessing As Boolean

    ' Current periods of the year
    Private m_vPeriods(,) As Object


    ' {* USER DEFINED CODE (End) *}

    Private m_sStepStatus As New FixedLengthString(2)

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTBudgetDetail.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)
    'developer guide no. 7
    Private Const vbFormCode As Integer = 0
    Public Property PeriodYearName() As String
        Get
            Return m_sPeriodYearName
        End Get
        Set(ByVal Value As String)
            m_sPeriodYearName = Value
        End Set
    End Property

    Public Property BudgetID() As Integer
        Get
            Return m_lBudgetID
        End Get
        Set(ByVal Value As Integer)
            m_lBudgetID = Value
        End Set
    End Property

    Public Property RevisesBudgetID() As Integer
        Get
            Return m_lRevisesBudgetID
        End Get
        Set(ByVal Value As Integer)
            m_lRevisesBudgetID = Value
        End Set
    End Property

    Public Property BasedOnBudgetID() As Integer
        Get
            Return m_lBasedOnBudgetID
        End Get
        Set(ByVal Value As Integer)
            m_lBasedOnBudgetID = Value
        End Set
    End Property

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

    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

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
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

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

            m_lReturn = m_oBusiness.GetDetailsForBudgetID(v_lBudgetID:=m_lBudgetID, r_vBudgetDetails:=m_vDetailsArray)
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



            ' Error Section.

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
            '    txtDesc.Text = FormatField( _
            ''        iFormatType:=PMFormatString, _
            ''        vFieldValue:=m_sDDesc$)
            '
            '    optChoice.Value = CBool(FormatField( _
            ''        iFormatType:=PMFormatBoolean, _
            ''        vFieldValue:=m_iDChoice%))
            '
            '    txtDate.Text = FormatField( _
            ''        iFormatType:=PMFormatDateLong, _
            ''        vFieldValue:=m_dtDDate)
            '
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            If Information.IsArray(m_vDetailsArray) Then
                ' display the data on the grid
                m_lReturn = CType(DisplayGridData(), gPMConstants.PMEReturnCode)
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

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

        Dim lBudgetID, lBudgetDetailID, lBudgetSequence, lPeriodID, lAccountID As Integer
        Dim cBudgetAmount As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 2

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit

                    ' Inform the business object with a new data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    If Information.IsArray(m_vAccountIDs) Then

                        ' add each account
                        For lLoop1 As Integer = 0 To m_vGridData.GetUpperBound(0)


                            If Not Object.Equals(m_vGridData(lLoop1, 0, 0), Nothing) Then

                                ' add each period
                                For lLoop2 As Integer = 0 To m_vGridData.GetUpperBound(1)

                                    lBudgetID = CInt(m_vGridData(lLoop1, lLoop2, ACGridBudgetID))
                                    lBudgetSequence = CInt(m_vGridData(lLoop1, lLoop2, ACGridBudgetSequence))
                                    lPeriodID = CInt(m_vGridData(lLoop1, lLoop2, ACGridPeriodID))
                                    lAccountID = CInt(m_vGridData(lLoop1, lLoop2, ACGridAccountID))
                                    cBudgetAmount = CDec(m_vGridData(lLoop1, lLoop2, ACGridBudgetAmount))


                                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID - 1, vBudgetDetailID:=lBudgetDetailID, vBudgetID:=lBudgetID, vBudgetSequence:=lBudgetSequence, vPeriodID:=lPeriodID, vAccountID:=lAccountID, vBudgetAmount:=cBudgetAmount)
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If

                                    ' Increment
                                    lBusinessDataID += 1

                                Next lLoop2

                            End If

                        Next lLoop1

                    End If

                    ' {* USER DEFINED CODE (End) *}

                    '        Case PMEdit
                    '            ' Inform the business object with an updated data item.
                    '
                    '            ' {* USER DEFINED CODE (Begin) *}
                    '            If (IsArray(m_vAccountIDs) = True) Then
                    '
                    '                ' add each account
                    '                For lLoop1& = 0 To UBound(m_vGridData, 1)
                    '
                    '                    ' add each period
                    '                    For lLoop2& = 0 To UBound(m_vGridData, 2)
                    '
                    '                        lBudgetID = CLng(m_vGridData(lLoop1, lLoop2, ACGridBudgetID))
                    '                        lBudgetSequence = CLng(m_vGridData(lLoop1, lLoop2, ACGridBudgetSequence))
                    '                        lPeriodID = CLng(m_vGridData(lLoop1, lLoop2, ACGridPeriodID))
                    '                        lAccountID = CLng(m_vGridData(lLoop1, lLoop2, ACGridAccountID))
                    '                        cBudgetAmount = CCur(m_vGridData(lLoop1, lLoop2, ACGridBudgetAmount))
                    '                        lBudgetDetailID = CLng(m_vGridData(lLoop1, lLoop2, ACGridBudgetDetailID))
                    '
                    '                        If (lBudgetDetailID = 0) Then
                    '                        ' lBusinessDataID&
                    '                            m_lReturn& = m_oBusiness.EditAdd(lRow:=lBusinessDataID&, _
                    ''                                            vBudgetDetailID:=lBudgetDetailID, _
                    ''                                            vBudgetID:=lBudgetID, _
                    ''                                            vBudgetSequence:=lBudgetSequence, _
                    ''                                            vPeriodID:=lPeriodID, _
                    ''                                            vAccountID:=lAccountID, _
                    ''                                            vBudgetAmount:=cBudgetAmount)
                    '                            If (m_lReturn& <> PMTrue) Then
                    '                                InterfaceToBusiness = PMFalse
                    '                                Debug.Print "Error Edit Add"
                    '                                Exit Function
                    '                            End If
                    '
                    '                            ' Increment
                    '                            lBusinessDataID& = lBusinessDataID& + 1
                    '
                    '                            m_lReturn& = m_oBusiness.Update()
                    '
                    '                        Else
                    '
                    '                            m_lReturn& = m_oBusiness.GetDetails( _
                    ''                                                        vBudgetDetailID:=lBudgetDetailID)
                    '                            If (m_lReturn& <> PMTrue) Then
                    '                                InterfaceToBusiness = PMFalse
                    '                                Debug.Print "error GetDetails"
                    '                                Exit Function
                    '                            End If
                    '
                    '                            m_lReturn& = m_oBusiness.GetNext(vBudgetDetailID:=lBudgetDetailID)
                    '                            If (m_lReturn& <> PMTrue) Then
                    '                                InterfaceToBusiness = PMFalse
                    '                                Debug.Print "error GetNext"
                    '                                Exit Function
                    '                            End If
                    '
                    '                            m_lReturn& = m_oBusiness.EditUpdate(lRow:=1, _
                    ''                                            vBudgetDetailID:=lBudgetDetailID, _
                    ''                                            vBudgetID:=lBudgetID, _
                    ''                                            vBudgetSequence:=lBudgetSequence, _
                    ''                                            vPeriodID:=lPeriodID, _
                    ''                                            vAccountID:=lAccountID, _
                    ''                                            vBudgetAmount:=cBudgetAmount)
                    '                            If (m_lReturn& <> PMTrue) Then
                    '                                InterfaceToBusiness = PMFalse
                    '                                Debug.Print "error Edit Update"
                    '                                Exit Function
                    '                            End If
                    '
                    '                            m_lReturn& = m_oBusiness.Update()
                    '
                    '                        End If
                    '
                    '                        If (m_lReturn& <> PMTrue) Then
                    '                            InterfaceToBusiness = PMFalse
                    '                            Debug.Print "Error EditUpdate/Add "; lLoop1; " "; lLoop2
                    '                            Exit Function
                    '                        End If
                    '
                    '                    Next lLoop2&
                    '
                    '                Next lLoop1&
                    '
                    '            End If

                    ' {* USER DEFINED CODE (End) *}
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

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

            'm_lReturn& = m_oBusiness.GetNext()

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
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

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

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
        Dim lCols, lRows As Integer


        Dim cBudgetAmount As Decimal

        Dim lBudgetDetailID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' If its empty, then it's already been processed as a cancel

            If m_vAccountIDs Is Nothing Then
                Return result
            End If

            ' Number of accounts used
            lRows = m_vAccountIDs.GetUpperBound(1) + 1
            lCols = (m_iEndColumn - m_iStartColumn) + 1

            ReDim m_vGridData(lRows, lCols - 1, ACGridRows - 1)

            ' For each Account
            For lLoop1 As Integer = 0 To lRows - 1


                If Not Object.Equals(m_vAccountIDs(ACAccountID - 1, lLoop1), Nothing) Then

                    ' Get this account's starting budget_detail_id
                    lBudgetDetailID = CInt(m_vAccountIDs(ACBudgetDetailID - 1, lLoop1))

                    ' For each Period
                    For lLoop2 As Integer = (m_iStartColumn - 2) To (m_iEndColumn - 2)

                        ' Budget ID
                        m_vGridData(lLoop1, lLoop2, ACGridBudgetID) = m_lBudgetID

                        ' Budget Sequence
                        m_vGridData(lLoop1, lLoop2, ACGridBudgetSequence) = lLoop2 + 1

                        ' Period ID
                        '                If (IsArray(m_vDetailsArray) = True) Then
                        '                    m_vGridData(lLoop1, lLoop2, ACGridPeriodID) = CLng(m_vDetailsArray(1, lLoop2))
                        '                Else
                        m_vGridData(lLoop1, lLoop2, ACGridPeriodID) = CInt(m_vPeriods(1, lLoop2))
                        '                End If

                        ' Account ID
                        m_vGridData(lLoop1, lLoop2, ACGridAccountID) = CInt(m_vAccountIDs(ACAccountID - 1, lLoop1))

                        ' Budget Detail ID
                        m_vGridData(lLoop1, lLoop2, ACGridBudgetDetailID) = lBudgetDetailID
                        ' If its a valid budget_detail_id, then move to the next one
                        If lBudgetDetailID <> 0 Then
                            lBudgetDetailID += 1
                        End If

                        ' Get from table
                        'uctPMGrid.Col = lLoop2 + 3
                        'uctPMGrid.Row = lLoop1 + 1
                        If Not grdBudgetDetails.Rows(lLoop1).Cells(lLoop2 + 2).Value Is Nothing Then
                            'If uctPMGrid.ColText.Trim() <> "" Then
                            cBudgetAmount = CDec(grdBudgetDetails.Rows(lLoop1).Cells(lLoop2 + 2).Value)
                        Else
                            cBudgetAmount = 0
                        End If

                        ' Budget Amount
                        m_vGridData(lLoop1, lLoop2, ACGridBudgetAmount) = cBudgetAmount

                    Next lLoop2

                End If

            Next lLoop1

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayGridData
    '
    ' Description: Transfers m_vDetailsArray onto the grid control
    '
    ' ***************************************************************** '
    Private Function DisplayGridData() As Integer

        Dim result As Integer = 0
        Dim iLoop1 As Integer
        Dim cAnnualAmount As Decimal
        Dim lRow, lCol As Integer
        Dim sAccountCode As String = ""
        Dim lOldPeriod As Integer
        Dim index As Integer
        Dim lLastID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'uctPMGrid.Row = 1
            cAnnualAmount = 0

            lOldPeriod = 0

            ' Check if the array needs initialising

            If m_vAccountIDs Is Nothing Then
                ' if so then redim it and
                ReDim m_vAccountIDs(1, 0)
                ' fill it with some dummy data
                m_vAccountIDs(0, 0) = 0
            End If

            ' get the number of rows here...
            'lRow = uctPMGrid.Row
            lRow = 0
            index = 1
            lLastID = 0

            If Information.IsArray(m_vDetailsArray) Then
                grdBudgetDetails.Rows.Clear()
                Dim iRowCnt As Integer = ((m_vDetailsArray.GetUpperBound(1)) / 12) + 1
                grdBudgetDetails.Rows.Add(iRowCnt)
            End If

            ' fill the columns in for each period
            For iLoop1 = m_vDetailsArray.GetLowerBound(1) To m_vDetailsArray.GetUpperBound(1)

                ' Its a 2D array, so we need to find out if we have moved onto the
                ' next row. Just keep track of the period and if the next one is less
                ' then we've reached the end of the year/row
                If lOldPeriod > CInt(m_vDetailsArray(0, iLoop1)) Then

                    ' store the account id
                    If lRow > m_vAccountIDs.GetUpperBound(1) Then
                        ReDim Preserve m_vAccountIDs(1, index)

                    End If

                    lLastID = iLoop1 - lOldPeriod

                    ' store the account_id
                    m_vAccountIDs(ACAccountID - 1, index - 1) = CInt(m_vDetailsArray(2, lLastID))

                    ' store the budget_detail_id
                    m_vAccountIDs(ACBudgetDetailID - 1, index - 1) = CInt(m_vDetailsArray(4, lLastID))


                    'uctPMGrid.Col = 2
                    'uctPMGrid.Text = CStr(cAnnualAmount)
                    grdBudgetDetails.Rows(lRow).Cells(1).Value = CStr(cAnnualAmount)
                    m_lReturn = CType(FormatCell(1, lRow), gPMConstants.PMEReturnCode)

                    'uctPMGrid.Col = 1

                    m_lReturn = m_oBusiness.GetBudgetAccountName(v_lAccountID:=m_vAccountIDs(ACAccountID - 1, index - 1), r_sAccountName:=sAccountCode)

                    'uctPMGrid.Text = sAccountCode
                    grdBudgetDetails.Rows(lRow).Cells(0).Value = sAccountCode

                    'uctPMGrid.Row = lRow
                    lRow = lRow + 1
                    cAnnualAmount = 0
                    index = index + 1
                End If

                lOldPeriod = CInt(m_vDetailsArray(0, iLoop1))

                lCol = CInt(1 + CDbl(m_vDetailsArray(0, iLoop1)))

                If CStr(m_vDetailsArray(3, iLoop1)) = "" Then
                    m_vDetailsArray(3, iLoop1) = 0
                End If

                If lCol <= grdBudgetDetails.Columns.Count - 1 Then
                    grdBudgetDetails.Rows(lRow).Cells(lCol).Value = m_vDetailsArray(3, iLoop1)
                    m_lReturn = CType(FormatCell(lCol, lRow), gPMConstants.PMEReturnCode)
                    cAnnualAmount += CDec(m_vDetailsArray(3, iLoop1))
                End If

            Next iLoop1

            lLastID = iLoop1 - lOldPeriod

            ' store the account id
            If lRow > m_vAccountIDs.GetUpperBound(1) Then
                ReDim Preserve m_vAccountIDs(1, index - 1)
            End If

            ' store the account_id
            m_vAccountIDs(ACAccountID - 1, index - 1) = CInt(m_vDetailsArray(2, lLastID))
            ' store the budget_detail_id
            m_vAccountIDs(ACBudgetDetailID - 1, index - 1) = CInt(m_vDetailsArray(4, lLastID))

            'uctPMGrid.Col = 2
            'uctPMGrid.Text = CStr(cAnnualAmount)
            grdBudgetDetails.Rows(lRow).Cells(1).Value = cAnnualAmount
            m_lReturn = CType(FormatCell(1, lRow), gPMConstants.PMEReturnCode)

            'uctPMGrid.Col = 1

            m_lReturn = m_oBusiness.GetBudgetAccountName(v_lAccountID:=m_vAccountIDs(ACAccountID - 1, index - 1), r_sAccountName:=sAccountCode)
            'uctPMGrid.Text = sAccountCode
            grdBudgetDetails.Rows(lRow).Cells(0).Value = sAccountCode


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayGridDataFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayGridData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim sBudgetRef As String = ""
        Dim bPosted As Boolean
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

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' we're not processing the grid at the moment
            m_bProcessing = False

            ' Set the caption of the year

            'developer guide no.26
            lblPanYear.Text = m_sPeriodYearName



            ' Set the panel of the budget reference

            m_lReturn = m_oBusiness.GetBudgetRef(v_vBudgetID:=m_lBudgetID, r_sBudgetRef:=sBudgetRef)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'developer guide no.26
            lblpanBudgetRef.Text = sBudgetRef
            ' Call m_oBusiness.GetPeriodsForYear and get the headers


            m_lReturn = m_oBusiness.GetPeriodsForYear(v_sPeriodYearName:=m_sPeriodYearName, r_vPeriods:=m_vPeriods)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(m_vPeriods) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Hide the other columns
            'uctPMGrid.VisibleCols = m_vPeriods.GetUpperBound(1) + 3
            'uctPMGrid.MaxCols = m_vPeriods.GetUpperBound(1) + 3

            'MKW PN16206
            If ((VB6.PixelsToTwipsX(tabMainTab.Width) - VB6.PixelsToTwipsX(tabMainTab.Left)) - (2 * VB6.PixelsToTwipsX(grdBudgetDetails.Left))) > 0 Then
                grdBudgetDetails.Width = (tabMainTab.Width - tabMainTab.Left) - (2 * grdBudgetDetails.Left)
            End If

            m_iStartColumn = m_vPeriods.GetLowerBound(1) + 2
            m_iEndColumn = m_vPeriods.GetUpperBound(1) + 2
            ' Set the header captions
            'uctPMGrid.Row = 0
            For iLoop1 As Integer = m_vPeriods.GetLowerBound(1) To m_vPeriods.GetUpperBound(1)
                grdBudgetDetails.Columns.Add(CStr(m_vPeriods(0, iLoop1)), CStr(m_vPeriods(0, iLoop1)))
                grdBudgetDetails.Columns(iLoop1 + 2).Width = 70
            Next iLoop1

            ' revises_budget_id
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                If m_lRevisesBudgetID <> 0 Then
                    ' Get the budget details

                    m_lReturn = m_oBusiness.GetDetailsForBudgetID(v_lBudgetID:=m_lRevisesBudgetID, r_vBudgetDetails:=m_vDetailsArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            ' based_on_budget_id
            If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_iTask = gPMConstants.PMEComponentAction.PMEdit) Then
                If m_lBasedOnBudgetID <> 0 Then
                    ' Get the budget details

                    m_lReturn = m_oBusiness.GetDetailsForBudgetID(v_lBudgetID:=m_lBasedOnBudgetID, r_vBudgetDetails:=m_vDetailsArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            If Information.IsArray(m_vDetailsArray) Then
                ' display the data on the grid
                m_lReturn = CType(DisplayGridData(), gPMConstants.PMEReturnCode)
            End If

            ' Check if status = posted

            m_lReturn = m_oBusiness.CheckPosted(r_bPosted:=bPosted, v_lBudgetID:=m_lBudgetID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If its posted, then disable the controls on the form
            If bPosted Then
                grdBudgetDetails.Enabled = False
                chkApportion.Enabled = False
                cmdOK.Enabled = False
            Else
                grdBudgetDetails.Enabled = True
                chkApportion.Enabled = True
                cmdOK.Enabled = True
            End If

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
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0


        Try


            ' Create and the object
            '    Set m_oFormFields = New iPMFormControl.FormFields
            '
            '    m_oFormFields.LanguageID = g_iLanguageID%
            '
            '    ' For each column, add form control
            '    For lLoop1& = 2 To m_iEndColumn%
            '
            '        m_lReturn& = m_oFormFields.AddNewFormField( _
            ''            ctlControl:=uctPMGrid, _
            ''            lFormat:=PMFormatCurrency, _
            ''            lFieldType:=PMCurrency, _
            ''            lGridColumn:=lLoop1, _
            ''            lMandatory:=PMNonMandatory, _
            ''            lDecimalPlaces:=4)
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            SetFieldValidation = PMFalse
            '            Exit Function
            '        End If
            '
            '    Next lLoop1&

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Set Field Validation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ReDim m_ctlTabFirstLast(1, 0)

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

            m_ctlTabFirstLast(ACControlStart, 0) = chkApportion
            m_ctlTabFirstLast(ACControlEnd, 0) = grdBudgetDetails

            ' {* USER DEFINED CODE (End) *}

            Return result

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


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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


            lblBudgetRef.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBudgetRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblYear.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACYear, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            chkApportion.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACApportion, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' set the column header
            'uctPMGrid.Col = 1
            'uctPMGrid.Row = 0


            grdBudgetDetails.Columns(0).HeaderText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColAccount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            ' set the column header
            'uctPMGrid.Col = 2
            'uctPMGrid.Row = 0

            grdBudgetDetails.Columns(1).HeaderText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACColAnnual, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
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

            ' Check the task.
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


    'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
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
    ' Name: CalculateAnnual
    '
    ' Description: Calculates the annual total
    '
    ' ***************************************************************** '
    Private Function CalculateAnnual(ByRef lCol As Integer, ByRef lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim cYear As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Exit if theres no text in the current cell
            If grdBudgetDetails.Rows(lRow).Cells(lCol).Value.Trim() = "" Then
                Return result
            End If

            ' Reset the year total
            cYear = 0

            ' Loop through and get the total
            For iLoop1 As Integer = m_iStartColumn To m_iEndColumn
                If Not grdBudgetDetails.Rows(lRow).Cells(iLoop1).Value Is Nothing Then
                    If grdBudgetDetails.Rows(lRow).Cells(iLoop1).Value.Trim() <> "" Then
                        cYear += CDec(grdBudgetDetails.Rows(lRow).Cells(iLoop1).Value)
                    End If
                End If
            Next iLoop1

            ' Set the year tota
            'uctPMGrid.Row = lRow
            'uctPMGrid.Col = 2
            grdBudgetDetails.Rows(lRow).Cells(1).Value = CStr(cYear)

            m_lReturn = CType(FormatCell(1, lRow), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Calculate Annual total.", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateAnnual", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: FillBudget
    '
    ' Description: Apportions the budget for the year
    '
    ' ***************************************************************** '
    Private Function FillBudget(ByRef lCol As Integer, ByRef lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim iLoop1 As Integer
        Dim cMonth, cYear, cDisplayedYear As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            If grdBudgetDetails.Rows(lRow).Cells(lCol).Value Is Nothing Or grdBudgetDetails.Rows(lRow).Cells(lCol).Value.Trim() = "" Then
                Return result
            End If

            cYear = CDec(grdBudgetDetails.Rows(lRow).Cells(lCol).Value)

            cDisplayedYear = 0

            ' Calculate the monthly amounts
            cMonth = cYear / ((m_iEndColumn - m_iStartColumn) + 1)
            cMonth = gPMMaths.PMTruncateCurrency(cMonth, gPMConstants.PMECurrencyNoOfDP.pmeCurDPTwo)

            For iLoop1 = m_iStartColumn To m_iEndColumn

                grdBudgetDetails.Rows(lRow).Cells(iLoop1).Value = CStr(cMonth)
                m_lReturn = CType(FormatCell(iLoop1, lRow), gPMConstants.PMEReturnCode)
                cDisplayedYear += cMonth
            Next iLoop1

            ' Check for precision loss
            If cDisplayedYear <> cYear Then

                ' Set the last value to offset the precision loss
                'uctPMGrid.Col = iLoop1 - 1
                'uctPMGrid.Row = lRow

                grdBudgetDetails.Rows(lRow).Cells(iLoop1 - 1).Value = CStr(cMonth + (cYear - cDisplayedYear))

                m_lReturn = CType(FormatCell(iLoop1 - 1, lRow), gPMConstants.PMEReturnCode)

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to apportion annual budget.", vApp:=ACApp, vClass:=ACClass, vMethod:="FillBudget", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FillZeros
    '
    ' Description: Fills the current row with 0's. Used when a new
    '              line is used for an account.
    '
    ' ***************************************************************** '
    Private Function FillZeros(ByVal v_lCurrentRow As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iLoop1 As Integer = (m_iStartColumn) To m_iEndColumn
                'uctPMGrid.Row = v_lCurrentRow
                'uctPMGrid.Col = iLoop1
                grdBudgetDetails.Rows(v_lCurrentRow).Cells(iLoop1).Value = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, "0")
            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FillZerosFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillZeros", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindAccount
    '
    ' Description: Calls up the find account form.
    '
    ' ***************************************************************** '
    Private Function FindAccount() As Integer

        Dim result As Integer = 0
        Dim lAccountID As Integer
        Dim sShortCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Blank out the find account details so that if its calldd more than once
            ' it doesnt automatically return the 1st search results.

            m_oFindAccount.ShortCode = ""

            m_oFindAccount.AccountUIK = 0

            m_oFindAccount.AccountID = 0


            m_lReturn = m_oFindAccount.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If m_oFindAccount.Status <> gPMConstants.PMEReturnCode.PMCancel Then


                lAccountID = m_oFindAccount.AccountID

                sShortCode = m_oFindAccount.AccountName

                'MsgBox CStr(m_lCurrColumn&) & ", " & CStr(m_lCurrRow)

                If m_lCurrRow < 1 Then
                    m_lCurrRow = 1
                End If
                If m_lCurrColumn < 1 Then
                    m_lCurrColumn = 1
                End If

                grdBudgetDetails.CurrentRow.Cells(0).ReadOnly = False
                '' Set the shortcode

                grdBudgetDetails.CurrentRow.Cells(0).Value = sShortCode
                grdBudgetDetails.CurrentRow.Cells(0).ReadOnly = True

                If Not Information.IsArray(m_vAccountIDs) Then
                    ReDim m_vAccountIDs(1, 0)
                    m_vAccountIDs(ACBudgetDetailID - 1, 0) = 0
                End If

                ' store the account id
                If m_lCurrRow > m_vAccountIDs.GetUpperBound(1) + 1 Then

                    ' if its a new account_id then redim the array
                    ReDim Preserve m_vAccountIDs(1, m_lCurrRow - 1)

                    ' and store a 0 value, no budget_detail yet
                    m_vAccountIDs(ACBudgetDetailID - 1, m_lCurrRow - 1) = 0

                    ' Fill the year with 0's
                    m_lReturn = CType(FillZeros(v_lCurrentRow:=m_lCurrRow - 1), gPMConstants.PMEReturnCode)

                End If

                ' store the account_id
                m_vAccountIDs(ACAccountID - 1, m_lCurrRow - 1) = lAccountID

            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindAccount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LockColumn
    '
    ' Description: Locks or unlocks a column on the grid
    '
    ' ***************************************************************** '
    'Private Function LockColumn(ByRef bLocked As Boolean, ByRef iColumn As Integer) As Integer

    '    Dim result As Integer = 0

    '    Try

    '        result = gPMConstants.PMEReturnCode.PMTrue

    '        ' Lock each row in the specified column
    '        For iLoop1 As Integer = 1 To uctPMGrid.MaxRows
    '            uctPMGrid.Row = iLoop1
    '            uctPMGrid.Col = iColumn
    '            uctPMGrid.ColLock = bLocked
    '        Next iLoop1

    '        Return result

    '    Catch excep As System.Exception



    '        result = gPMConstants.PMEReturnCode.PMError

    '        ' Log Error Message
    '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockColumnFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockColumn", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

    '        Return result

    '    End Try
    'End Function

    'Private Sub chkApportion_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkApportion.CheckStateChanged

    '    ' Lock or unlock the annual column
    '    If chkApportion.CheckState = CheckState.Checked Then
    '        m_lReturn = CType(LockColumn(bLocked:=False, iColumn:=2), gPMConstants.PMEReturnCode)
    '    Else
    '        m_lReturn = CType(LockColumn(bLocked:=True, iColumn:=2), gPMConstants.PMEReturnCode)
    '    End If

    'End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'developer guide no. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)
    End Sub

    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            If g_oObjectManager Is Nothing Then
                g_oObjectManager = New bObjectManager.ObjectManager()
                m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            End If

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTBudgetDetail.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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
            m_oGeneral = New iACTBudgetDetail.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the Find Account object
            Dim temp_m_oFindAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oFindAccount, sClassName:="iACTFindAccount.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oFindAccount = temp_m_oFindAccount

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the cancelled property to true. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            'Cancelled = True

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

        ' Forms load event.

        Try

            grdBudgetDetails.Rows.Add(22)
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

            ' {* USER DEFINED CODE (End) *}

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

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
            'New Code
            grdBudgetDetails.ScrollBars = ScrollBars.Both
            grdBudgetDetails.Columns(1).ReadOnly = True


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

            ' Terminate the Find Account object

            m_oFindAccount.Dispose()



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

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        '    With tabMainTab
        '        ' Set the default button.
        '        If (.Tab < cmdNext.Count) Then
        '            cmdNext(.Tab).Default = True
        '        Else
        '            cmdOK.Default = True
        '        End If
        ''
        '        ' Now I know this is crap, this goes against
        '        ' all my principles, but for some reason when
        '        ' using the mouse to select a tab the setfocus
        '        ' code below doesn't work. The cursor sticks,
        '        ' and you can't tab off. Therefore I've used
        '        ' this to get around the problem.
        '        DoEvents
        ''
        '        ' Set focus to the first control on the tab.
        '        If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
        '            m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
        '        End If
        '    End With
        '
        'Catch 
        '
        '
        '
        ' Error Section.
        '
        '
        'tabMainTabPreviousTab = tabMainTab.SelectedIndex
        'End Try

    End Sub

    ' ***************************************************************** '
    ' Name: FillBlanks
    '
    ' Description: Checks the months/periods for each account and fills
    '              any empty cells with 0
    '
    ' ***************************************************************** '
    Private Function FillBlanks() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vAccountIDs) Then

                For iLoop1 As Integer = m_vAccountIDs.GetLowerBound(1) To m_vAccountIDs.GetUpperBound(1)


                    If Not Object.Equals(m_vAccountIDs(0, iLoop1), Nothing) Then
                        For iLoop2 As Integer = m_iStartColumn To m_iEndColumn

                            If Not grdBudgetDetails.Rows(iLoop1).Cells(iLoop2).Value Is Nothing Then
                                If grdBudgetDetails.Rows(iLoop1).Cells(iLoop2).Value.ToString() = "" Then
                                    grdBudgetDetails.Rows(iLoop1).Cells(iLoop2).Value = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, "0")
                                End If
                            End If
                        Next iLoop2
                    End If

                Next iLoop1

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FillBlanks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillBlanks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' If theres no account id's then the grid is empty
            ' so treat OK as a CANCEL

            If m_vAccountIDs Is Nothing Then
                'PN10232 eck 05022004
                '
                '       m_lStatus& = PMCancel
                MessageBox.Show("Please Enter a valid account", "Warning", MessageBoxButtons.OK)
                Exit Sub
            Else
                m_lStatus = gPMConstants.PMEReturnCode.PMOK
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = CType(FillBlanks(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                m_lReturn = CType(DeleteExistingBudgetDetails(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    m_lReturn = gPMConstants.PMEReturnCode.PMError
                    Exit Sub
                End If
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

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

    ' ***************************************************************** '
    ' Name: DeleteExistingBudgetDetails (Private)
    '
    ' Description: Deletes existing budget details from the database.
    '
    ' ***************************************************************** '
    Private Function DeleteExistingBudgetDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.DeleteBudgetDetails(vBudgetID:=m_lBudgetID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception


            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete existing budget details.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteExistingBudgetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

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



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cmdNext_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdNext_Click(ByRef Index As Integer)
    '
    'Try 
    '
    ' Change to the next tab.
    'If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
    'SSTabHelper.SetSelectedIndex(tabMainTab, Index + 1)
    'End If
    '
    ' Set focus to the first control on the tab.
    'If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
    'm_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
    'End If
    '
    'Catch 
    '
    '
    '
    ' Error Section
    '
    'Exit Sub
    'End Try
    '
    '
    'End Sub

    'Private Sub uctPMGrid_Change(ByVal Sender As Object, ByVal e As PMGridControl.uctPMGridControl.ChangeEventArgs) Handles uctPMGrid.Change


    '    Dim bInvalid As Boolean

    '    ' Dont want to go in again if we are processing it already
    '    If Not m_bProcessing Then

    '        m_bProcessing = True

    '        'uctPMGrid.Col = Col
    '        uctPMGrid.Col = e.Col
    '        'uctPMGrid.Row = Row
    '        uctPMGrid.Row = e.Row

    '        bInvalid = False

    '        If m_vAccountIDs Is Nothing Then
    '            bInvalid = True
    '        ElseIf e.Row > m_vAccountIDs.GetUpperBound(1) + 1 Then
    '            bInvalid = True
    '        End If

    '        If bInvalid Then
    '            MessageBox.Show("Values cannot be added to rows without accounts.", "Invalid Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '            uctPMGrid.ColText = ""
    '            m_bProcessing = False
    '            Exit Sub
    '        End If

    '        Dim dbNumericTemp As Double
    '        If Not Double.TryParse(uctPMGrid.ColText, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
    '            ' TODO - Somehow move cursor back to error cell
    '            MessageBox.Show("Value must be numerical. ReSet to zero", "Invalid Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '            'eckPN10240 set none numeric values to zero
    '            uctPMGrid.ColText = "00.00"
    '            m_bProcessing = False
    '            Exit Sub
    '        End If

    '        ' Fill in the years budget if checked
    '        If chkApportion.CheckState = CheckState.Checked Then

    '            ' only process if its the annual column
    '            If e.Col = 2 Then
    '                m_lReturn = CType(FillBudget(lCol:=e.Col, lRow:=e.Row), gPMConstants.PMEReturnCode)
    '            End If

    '        End If

    '        ' re-calculate the annual figure
    '        If e.Col > 2 Then
    '            m_lReturn = CType(CalculateAnnual(lCol:=e.Col, lRow:=e.Row), gPMConstants.PMEReturnCode)
    '        End If

    '        ' Format the cell to currency
    '        m_lReturn = CType(FormatCell(e.Col, e.Row), gPMConstants.PMEReturnCode)

    '        m_bProcessing = False

    '    End If

    'End Sub

    'Private Sub uctPMGrid_DblClick(ByVal eventSender As Object, ByVal e As PMGridControl.uctPMGridControl.DblClickEventArgs) Handles uctPMGrid.DblClick

    '    m_lCurrColumn = e.Col
    '    m_lCurrRow = e.Row

    '    If e.Col = 1 Then
    '        m_lReturn = CType(FindAccount(), gPMConstants.PMEReturnCode)
    '    End If

    'End Sub

    ' ***************************************************************** '
    ' Name: FormatCell
    '
    ' Description: Formats a cell to currency.
    '
    ' ***************************************************************** '
    Private Function FormatCell(ByRef Col As Integer, ByRef Row As Integer, Optional ByRef NewCol As Integer = 0, Optional ByRef NewRow As Integer = 0) As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Select the row and column
            'uctPMGrid.Row = Row
            'uctPMGrid.Col = Col

            ' Set the new text

            grdBudgetDetails.Rows(Row).Cells(Col).Value = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, grdBudgetDetails.Rows(Row).Cells(Col).Value.ToString())

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatCell Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatCell", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Private Sub uctPMGrid_DeleteCell(ByVal Sender As Object, ByVal e As PMGridControl.uctPMGridControl.DeleteCellEventArgs) Handles uctPMGrid.DeleteCell
    '    'eckPN10243
    '    'Removed this as there is no delete button the form
    '    'This function is randomly activated by the delete key but doesn't work
    '    '    Debug.Print "DeleteCell"
    '    '
    '    '    ' Delete the current row
    '    '    If (Col = 1) Then
    '    '
    '    '        m_lReturn& = MsgBox("Are you sure you wish to remove this account and its details?", _
    '    ''                    vbYesNo + vbQuestion, _
    '    ''                    "Delete")
    '    '        If (m_lReturn& = vbYes) Then
    '    '
    '    '            uctPMGrid.Col = 1
    '    '            uctPMGrid.ColLock = False
    '    '            uctPMGrid.Row = m_lCurrRow&
    '    '            ' remove the row
    '    '
    '    '        End If
    '    '
    '    '    End If

    'End Sub

    'Private Sub uctPMGrid_LeaveCell(ByVal Sender As Object, ByVal e As PMGridControl.uctPMGridControl.LeaveCellEventArgs) Handles uctPMGrid.LeaveCell

    '    ' store the positions globally
    '    'm_lCurrColumn& = NewCol
    '    'm_lCurrRow& = NewRow

    'End Sub

    ' PRIVATE Events (End)









    Private Sub grdBudgetDetails_CellEndEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdBudgetDetails.CellEndEdit
        Dim bInvalid As Boolean
        If e.ColumnIndex = 0 Then
            Exit Sub
        End If

        ' Dont want to go in again if we are processing it already
        If Not m_bProcessing Then

            m_bProcessing = True
            bInvalid = False

            If m_vAccountIDs Is Nothing Then
                bInvalid = True
            ElseIf e.RowIndex > m_vAccountIDs.GetUpperBound(1) Then
                bInvalid = True
            End If

            If bInvalid Then
                MessageBox.Show("Values cannot be added to rows without accounts.", "Invalid Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                grdBudgetDetails.CurrentRow.Cells(e.ColumnIndex).Value = ""
                m_bProcessing = False
                Exit Sub
            End If

            Dim dbNumericTemp As Double
            If e.ColumnIndex >= 1 Then
                If Not Double.TryParse(grdBudgetDetails.CurrentRow.Cells(e.ColumnIndex).Value, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    ' TODO - Somehow move cursor back to error cell
                    MessageBox.Show("Value must be numerical. ReSet to zero", "Invalid Data Entry", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    'eckPN10240 set none numeric values to zero
                    grdBudgetDetails.CurrentRow.Cells(e.ColumnIndex).Value = "00.00"
                    m_bProcessing = False
                    Exit Sub
                End If
            End If
            ' Fill in the years budget if checked
            If chkApportion.CheckState = CheckState.Checked Then

                ' only process if its the annual column
                If e.ColumnIndex = 1 Then
                    m_lReturn = CType(FillBudget(lCol:=e.ColumnIndex, lRow:=e.RowIndex), gPMConstants.PMEReturnCode)
                End If

            End If

            ' re-calculate the annual figure
            If e.ColumnIndex > 1 Then
                m_lReturn = CType(CalculateAnnual(lCol:=e.ColumnIndex, lRow:=e.RowIndex), gPMConstants.PMEReturnCode)
            End If

            ' Format the cell to currency
            m_lReturn = CType(FormatCell(e.ColumnIndex, e.RowIndex), gPMConstants.PMEReturnCode)

            m_bProcessing = False

        End If
    End Sub

    Private Sub chkApportion_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApportion.CheckedChanged
        If chkApportion.Checked Then
            grdBudgetDetails.Columns(1).ReadOnly = False
        End If
    End Sub

    Private Sub grdBudgetDetails_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdBudgetDetails.CellDoubleClick
        m_lCurrColumn = e.ColumnIndex
        m_lCurrRow = e.RowIndex + 1
        If e.ColumnIndex = 0 Then
            m_lReturn = CType(FindAccount(), gPMConstants.PMEReturnCode)
        End If
    End Sub
End Class
