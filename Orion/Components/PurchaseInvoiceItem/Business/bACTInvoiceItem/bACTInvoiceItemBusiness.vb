Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 02/11/1998
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a ACTInvoiceItem.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 03/04/2007
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of ACTInvoiceItems (Private)
    Private m_oACTInvoiceItems As bACTInvoiceItem.ACTInvoiceItems

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date


    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' Primary Keys to work with
    Private m_lInvoiceID As Integer
    Private m_lInvoiceItemNo As Integer

    'Invoice item array
    Private m_vInvoiceItem As Object

    ' NavigatorV3 variables
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer

    ' PRIVATE Data Members (End)

    Public Function DeleteInvoiceItems(Optional ByRef vInvoiceID As Integer = 0) As Integer

        'This function deletes all the invoice items for a given invoice

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            If Information.IsNothing(vInvoiceID) Then
                vInvoiceID = m_lInvoiceID
            End If


            'Clear the invoice items collection
            For lPtr As Integer = 1 To m_oACTInvoiceItems.Count()
                m_oACTInvoiceItems.Delete(1)
            Next lPtr

            With m_oDatabase.Parameters

                .Clear()

                'Remove the Invoice Items from the Table
                m_lReturn = .Add(sName:="invoice_id", vValue:=CStr(vInvoiceID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteInvoiceSQL, sSQLName:=ACDeleteInvoiceName, bStoredProcedure:=ACDeleteInvoiceStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch



            Return gPMConstants.PMEReturnCode.PMError
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result

        End Try
    End Function

    Public ReadOnly Property InvoiceItem() As Object
        Get

            Return m_vInvoiceItem

        End Get
    End Property


    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFOrion

        End Get
    End Property

    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property



    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oACTInvoiceItems.Count()
                    m_lCurrentRecord = m_oACTInvoiceItems.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oACTInvoiceItems.Count()

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property


    Public Property InvoiceID() As Integer
        Get

            Return m_lInvoiceID

        End Get
        Set(ByVal Value As Integer)

            m_lInvoiceID = Value

        End Set
    End Property

    Public Property InvoiceItemNo() As Integer
        Get

            Return m_lInvoiceItemNo

        End Get
        Set(ByVal Value As Integer)

            m_lInvoiceItemNo = Value

        End Set
    End Property

    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: SetKeys
    '
    ' Description: Navigator SetKeys function.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys
    '
    ' Description: Navigator GetKeys function.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vKeyArray = ""

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vKeyArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetSummary
    '
    ' Description: GetSummary Navigator function.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummaryFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Navigator Start function. Entry point into Navigator.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel




            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Remove component services

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create ACTInvoiceItems Collection
            m_oACTInvoiceItems = New bACTInvoiceItem.ACTInvoiceItems()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            If disposing Then
                m_oACTInvoiceItems = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


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



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single ACTInvoiceItem directly into the database.
    '        Note: The ACTInvoiceItem will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceItemNo As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vNominalCode As Object = Nothing, Optional ByRef vValue As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vDeptAmount As Object = Nothing, Optional ByRef vVATRate As Object = Nothing, Optional ByRef vHasVAT As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oACTInvoiceItem As bACTInvoiceItem.ACTInvoiceItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new ACTInvoiceItem
            oACTInvoiceItem = New bACTInvoiceItem.ACTInvoiceItem()

            ' Populate ACTInvoiceItem Attributes










            m_lReturn = SetProperties(oACTInvoiceItem, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vInvoiceID:=CInt(vInvoiceID), vInvoiceItemNo:=CInt(vInvoiceItemNo), vDescription:=CStr(vDescription), vNominalCode:=CStr(vNominalCode), vValue:=CDbl(vValue), vCurrencyID:=CInt(vCurrencyID), vDepartmentID:=CInt(vDepartmentID), vDeptAmount:=CByte(vDeptAmount), vVATRate:=CByte(vVATRate), vHasVAT:=CBool(vHasVAT))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oACTInvoiceItem = Nothing
                Return result
            End If

            ' Add the ACTInvoiceItem to the Database
            m_lReturn = AddItem(oACTInvoiceItem)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oACTInvoiceItem = Nothing
                Return result
            End If

            ' Retain the Primary Key of the ACTInvoiceItem Added
            With oACTInvoiceItem
                InvoiceID = .InvoiceID
                InvoiceItemNo = .InvoiceItemNo
            End With

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            oACTInvoiceItem = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single ACTInvoiceItem directly from the database.
    '        Note: The ACTInvoiceItem will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceItemNo As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oACTInvoiceItem As bACTInvoiceItem.ACTInvoiceItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new ACTInvoiceItem
            oACTInvoiceItem = New bACTInvoiceItem.ACTInvoiceItem()

            ' Set ACTInvoiceItem Primary Key


            m_lReturn = SetProperties(oACTInvoiceItem, iStatus:=gPMConstants.PMEComponentAction.PMDelete, vInvoiceID:=CInt(vInvoiceID), vInvoiceItemNo:=CInt(vInvoiceItemNo))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oACTInvoiceItem = Nothing
                Return result
            End If

            ' Delete the ACTInvoiceItem from the Database
            m_lReturn = DeleteItem(oACTInvoiceItem)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oACTInvoiceItem = Nothing
                Return result
            End If

            oACTInvoiceItem = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckID (Public)
    '
    ' Description: Checks to see if the supplied ID is a valid record.
    '
    ' ***************************************************************** '
    Public Function CheckID(ByRef vID As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(vID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIDSQL, sSQLName:=ACCheckIDName, bStoredProcedure:=ACCheckIDStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied Allocation properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oInvoiceItem As ACTInvoiceItem, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'Developer Guide No. 112
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        'Developer Guide No.111
        oFields = m_oDatabase.Records.Item(0).Fields()


        ' Populate Base Details

        With oInvoiceItem


            If Convert.IsDBNull(oFields("invoice_id")) Or IsNothing(oFields("invoice_id")) Then
                .InvoiceID = 0
            Else
                .InvoiceID = oFields("invoice_id")
            End If


            If Convert.IsDBNull(oFields("invoice_item_no")) Or IsNothing(oFields("invoice_item_no")) Then
                .InvoiceItemNo = 0
            Else
                .InvoiceItemNo = oFields("invoice_item_no")
            End If


            If Convert.IsDBNull(oFields("description")) Or IsNothing(oFields("description")) Then
                .Description = ""
            Else
                .Description = oFields("description")
            End If


            If Convert.IsDBNull(oFields("nominal_code")) Or IsNothing(oFields("nominal_code")) Then
                .NominalCode = ""
            Else
                .NominalCode = oFields("nominal_code")
            End If


            If Convert.IsDBNull(oFields("value")) Or IsNothing(oFields("value")) Then
                .Value = 0
            Else
                .Value = oFields("value")
            End If


            If Convert.IsDBNull(oFields("currency_id")) Or IsNothing(oFields("currency_id")) Then
                .CurrencyID = 0
            Else
                .CurrencyID = oFields("currency_id")
            End If


            If Convert.IsDBNull(oFields("department_id")) Or IsNothing(oFields("department_id")) Then
                .DepartmentID = 0
            Else
                .DepartmentID = oFields("department_id")
            End If


            If Convert.IsDBNull(oFields("dept_amount")) Or IsNothing(oFields("dept_amount")) Then
                .DeptAmount = 0
            Else
                .DeptAmount = oFields("dept_amount")
            End If


            If Convert.IsDBNull(oFields("vat_rate")) Or IsNothing(oFields("vat_rate")) Then
                .VATRate = 0
            Else
                .VATRate = oFields("vat_Rate")
            End If


            If Convert.IsDBNull(oFields("has_vat")) Or IsNothing(oFields("has_vat")) Then
                .HasVat = 0
            Else
                .HasVat = oFields("has_vat")
            End If

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required ACTInvoiceItems and populate the Collection
    '
    ' ***************************************************************** '
    'Developer Guide No.101
    Public Function GetDetails(Optional ByRef vLockMode As gPMConstants.PMELockMode = 0, Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceItemNo As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Dim oACTInvoiceItem As bACTInvoiceItem.ACTInvoiceItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oACTInvoiceItems.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Information.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Check for Valid Primary Key
            Dim dbNumericTemp2 As Double

            If (Not Information.IsNothing(vInvoiceID)) And (Not Double.TryParse(CStr(vInvoiceID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vInvoiceID=" & vInvoiceID, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If
            Dim dbNumericTemp3 As Double

            If (Not Information.IsNothing(vInvoiceItemNo)) And (Not Double.TryParse(CStr(vInvoiceItemNo), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vInvoiceItemNo=" & vInvoiceItemNo, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            If (Not Information.IsNothing(vInvoiceID)) And (Not Information.IsNothing(vInvoiceItemNo)) Then

                ' Create New ACTInvoiceItem
                oACTInvoiceItem = New bACTInvoiceItem.ACTInvoiceItem()

                ' Set component primary keys
                With oACTInvoiceItem
                    .InvoiceID = vInvoiceID
                    .InvoiceItemNo = vInvoiceItemNo

                    'm_lReturn& = .SelectItem()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                ' Add ACTInvoiceItem to collection
                m_lReturn = m_oACTInvoiceItems.Add(oNewACTInvoiceItem:=oACTInvoiceItem)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oACTInvoiceItem = Nothing

            Else

                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="invoice_id", vValue:=CStr(vInvoiceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords)
                'vResultArray:=m_vInvoiceItem)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?

            If lRecordCount > 0 Then

                ' Yes, load them into the collection

                For lSub As Integer = 1 To lRecordCount

                    ' Create New Allocation
                    oACTInvoiceItem = New bACTInvoiceItem.ACTInvoiceItem()

                    m_lReturn = SetPropertiesFromDB(oInvoiceItem:=oACTInvoiceItem, lRecordNumber:=lSub)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add Allocation to collection
                    m_lReturn = m_oACTInvoiceItems.Add(oNewACTInvoiceItem:=oACTInvoiceItem)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oACTInvoiceItem = Nothing

                Next lSub

            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required ACTInvoiceItems and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceItemNo As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vNominalCode As Object = Nothing, Optional ByRef vValue As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vDeptAmount As Object = Nothing, Optional ByRef vVATRate As Object = Nothing, Optional ByRef vHasVAT As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oACTInvoiceItem As bACTInvoiceItem.ACTInvoiceItem
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oACTInvoiceItems.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oACTInvoiceItem = m_oACTInvoiceItems.Item(m_lCurrentRecord)

            ' Get the ACTInvoiceItem Property Values

            'developer guide no.98
            m_lReturn = GetProperties(oACTInvoiceItem, iStatus, vInvoiceID:=vInvoiceID, vInvoiceItemNo:=vInvoiceItemNo, vDescription:=vDescription, vNominalCode:=vNominalCode, vValue:=vValue, vCurrencyID:=vCurrencyID, vDepartmentID:=vDepartmentID, vDeptAmount:=vDeptAmount, vVATRate:=vVATRate, vHasVAT:=vHasVAT)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oACTInvoiceItem = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied ACTInvoiceItem into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceItemNo As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vNominalCode As Object = Nothing, Optional ByRef vValue As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vDeptAmount As Object = Nothing, Optional ByRef vVATRate As Object = Nothing, Optional ByRef vHasVAT As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oACTInvoiceItem As bACTInvoiceItem.ACTInvoiceItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oACTInvoiceItems.Count() <> (lRow) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new ACTInvoiceItem
            oACTInvoiceItem = New bACTInvoiceItem.ACTInvoiceItem()

            ' Populate ACTInvoiceItem Attributes

            m_lReturn = SetProperties(oACTInvoiceItem, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vInvoiceID:=CInt(vInvoiceID), vInvoiceItemNo:=CInt(vInvoiceItemNo), vDescription:=CStr(vDescription), vNominalCode:=CStr(vNominalCode), vValue:=CDbl(vValue), vCurrencyID:=CInt(vCurrencyID), vDepartmentID:=CInt(vDepartmentID), vDeptAmount:=CByte(vDeptAmount), vVATRate:=CByte(vVATRate), vHasVAT:=CBool(vHasVAT))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oACTInvoiceItem = Nothing
                Return result
            End If

            ' Add ACTInvoiceItem to collection
            m_lReturn = m_oACTInvoiceItems.Add(oNewACTInvoiceItem:=oACTInvoiceItem)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oACTInvoiceItem = Nothing
                Return result
            End If

            oACTInvoiceItem = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the ACTInvoiceItem
    '              specified and updates the ACTInvoiceItem with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceItemNo As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vNominalCode As Object = Nothing, Optional ByRef vValue As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vDeptAmount As Object = Nothing, Optional ByRef vVATRate As Object = Nothing, Optional ByRef vHasVAT As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oACTInvoiceItem As bACTInvoiceItem.ACTInvoiceItem
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oACTInvoiceItems.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oACTInvoiceItem = m_oACTInvoiceItems.Item(lRow)

            ' Check the Status of the ACTInvoiceItem

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oACTInvoiceItem.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    iStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    iStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            ' Update ACTInvoiceItem Attributes

            m_lReturn = SetProperties(oACTInvoiceItem, iStatus:=iStatus, vInvoiceID:=CInt(vInvoiceID), vInvoiceItemNo:=CInt(vInvoiceItemNo), vDescription:=CStr(vDescription), vNominalCode:=CStr(vNominalCode), vValue:=CDbl(vValue), vCurrencyID:=CInt(vCurrencyID), vDepartmentID:=CInt(vDepartmentID), vDeptAmount:=CByte(vDeptAmount), vVATRate:=CByte(vVATRate), vHasVAT:=CBool(vHasVAT))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oACTInvoiceItem = Nothing
                Return result
            End If

            ' Release reference to ACTInvoiceItem
            oACTInvoiceItem = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified ACTInvoiceItem can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oACTInvoiceItem As bACTInvoiceItem.ACTInvoiceItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oACTInvoiceItems.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oACTInvoiceItem = m_oACTInvoiceItems.Item(lRow)

            ' Check the Status of the ACTInvoiceItem

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oACTInvoiceItem.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oACTInvoiceItem.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oACTInvoiceItem.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to ACTInvoiceItem
            oACTInvoiceItem = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Cancel (Public)
    '
    ' Description: Checks the Collection to see if Cancel is OK.
    '              i.e. Do we need any Adding, Deleting or Updating.
    '
    '              Returns PMTrue if all items are clean
    '                      (PMView or PMDummyDelete)
    '              Otherwise returns PMDataChanged.
    ' ***************************************************************** '
    Public Function Cancel() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For lSub As Integer = 1 To m_oACTInvoiceItems.Count()
                Select Case m_oACTInvoiceItems.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Loops round the collection, doing any required
    '              Adds, Deletes or Updates.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lSub As Integer
        Dim oACTInvoiceItem As bACTInvoiceItem.ACTInvoiceItem
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            'If there is nothing to update then exit
            If m_oACTInvoiceItems.Count() = 0 Then
                Return result
            End If

            ' Loop round Collection

            For lSub = 1 To m_oACTInvoiceItems.Count()
                oACTInvoiceItem = m_oACTInvoiceItems.Item(lSub)


                Select Case oACTInvoiceItem.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = AddItem(oACTInvoiceItem)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = UpdateItem(oACTInvoiceItem)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = DeleteItem(oACTInvoiceItem)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Retain the Primary Key of the ACTInvoiceItem
            With oACTInvoiceItem
                InvoiceID = .InvoiceID
                InvoiceItemNo = .InvoiceItemNo
            End With

            ' Release last reference
            oACTInvoiceItem = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CommitTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oACTInvoiceItems.Count()

                        ' With the item
                        With m_oACTInvoiceItems.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oACTInvoiceItems.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = RollbackTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oACTInvoiceItem As bACTInvoiceItem.ACTInvoiceItem) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = AddInputParam(oACTInvoiceItem)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add PrimaryKey parameters
        m_lReturn = AddKeyInputParam(oACTInvoiceItem)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam(ByRef oACTInvoiceItem As bACTInvoiceItem.ACTInvoiceItem) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="description", vValue:=oACTInvoiceItem.Description, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="nominal_code", vValue:=oACTInvoiceItem.NominalCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="value", vValue:=CStr(oACTInvoiceItem.Value), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=CStr(oACTInvoiceItem.CurrencyID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oACTInvoiceItem.DepartmentID = 0 Then

                'Developer Guide No. 85
                m_lReturn = .Parameters.Add(sName:="department_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="department_id", vValue:=CStr(oACTInvoiceItem.DepartmentID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If oACTInvoiceItem.DeptAmount.Equals(0) Then

                'Developer Guide NO. 85
                m_lReturn = .Parameters.Add(sName:="dept_amount", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            Else
                m_lReturn = .Parameters.Add(sName:="dept_amount", vValue:=CStr(oACTInvoiceItem.DeptAmount), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If oACTInvoiceItem.VATRate.Equals(0) Then

                'Developer Guide No. 85
                m_lReturn = .Parameters.Add(sName:="vat_rate", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            Else
                m_lReturn = .Parameters.Add(sName:="vat_rate", vValue:=CStr(oACTInvoiceItem.VATRate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If oACTInvoiceItem.VATRate.Equals(0) Then

                'Developer Guide No. 85
                m_lReturn = .Parameters.Add(sName:="has_vat", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                If oACTInvoiceItem.HasVat Then
                    m_lReturn = .Parameters.Add(sName:="has_vat", vValue:=CStr(1), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                Else
                    m_lReturn = .Parameters.Add(sName:="has_vat", vValue:=CStr(0), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                End If
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyInputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY INPUT parameters
    '              required for a Select, Delete or Update.
    '
    ' ***************************************************************** '
    Private Function AddKeyInputParam(ByRef oACTInvoiceItem As bACTInvoiceItem.ACTInvoiceItem) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="invoice_id", vValue:=CStr(oACTInvoiceItem.InvoiceID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="invoice_item_no", vValue:=CStr(oACTInvoiceItem.InvoiceItemNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        End With

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Private Function DeleteItem(ByRef oACTInvoiceItem As bACTInvoiceItem.ACTInvoiceItem) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            ' Clear the Database Parameters Collection
            .Parameters.Clear()

            ' Add PrimaryKey as INPUT parameters
            m_lReturn = AddKeyInputParam(oACTInvoiceItem)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = .SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record wasn't deleted, error
            If lRecordsAffected > 0 Then
                ' Deleted, No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oACTInvoiceItem As bACTInvoiceItem.ACTInvoiceItem) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = AddInputParam(oACTInvoiceItem)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add PrimaryKey as INPUT parameters
        m_lReturn = AddKeyInputParam(oACTInvoiceItem)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check to see that the record was updated OK
        If lRecordsAffected > 0 Then
            ' Updated No action required
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DefaultParameters (Private)
    '
    ' Description: Sets the Default Values for a ACTInvoiceItem.
    '
    ' ***************************************************************** '
    'Developer Guide No. 17
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceItemNo As Object = Nothing, Optional ByRef vDescription As String = "", Optional ByRef vNominalCode As String = "", Optional ByRef vValue As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vDeptAmount As Object = Nothing, Optional ByRef vVATRate As Object = Nothing, Optional ByRef vHasVAT As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Information.IsNothing(vInvoiceID)) Or (vInvoiceID.Equals(0)) Or (bDefaultAll) Then
            vInvoiceID = 0
        End If



        If (Information.IsNothing(vInvoiceItemNo)) Or (vInvoiceItemNo.Equals(0)) Or (bDefaultAll) Then
            vInvoiceItemNo = 0
        End If



        If (Information.IsNothing(vDescription)) Or (String.IsNullOrEmpty(vDescription)) Or (bDefaultAll) Then
            vDescription = ""
        End If



        If (Information.IsNothing(vNominalCode)) Or (String.IsNullOrEmpty(vNominalCode)) Or (bDefaultAll) Then
            vNominalCode = ""
        End If



        If (Information.IsNothing(vValue)) Or (vValue.Equals(0)) Or (bDefaultAll) Then
            vValue = 0
        End If



        If (Information.IsNothing(vCurrencyID)) Or (vCurrencyID.Equals(0)) Or (bDefaultAll) Then
            vCurrencyID = 0
        End If



        If (Information.IsNothing(vDepartmentID)) Or (vDepartmentID.Equals(0)) Or (bDefaultAll) Then
            vDepartmentID = 0
        End If



        If (Information.IsNothing(vDeptAmount)) Or (vDeptAmount.Equals(0)) Or (bDefaultAll) Then
            vDeptAmount = 0
        End If



        If (Information.IsNothing(vVATRate)) Or (vVATRate.Equals(0)) Or (bDefaultAll) Then
            vVATRate = 0
        End If



        If (Information.IsNothing(vHasVAT)) Or (vHasVAT.Equals(0)) Or (bDefaultAll) Then
            vHasVAT = 0
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the ACTInvoiceItem for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceItemNo As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vNominalCode As Object = Nothing, Optional ByRef vValue As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vDeptAmount As Object = Nothing, Optional ByRef vVATRate As gPMConstants.PMEReturnCode = 0, Optional ByRef vHasVAT As gPMConstants.PMEReturnCode = 0) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Information.IsNothing(vInvoiceID) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vInvoiceID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vInvoiceItemNo) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vInvoiceItemNo), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vValue) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vCurrencyID) Then

            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(vCurrencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vDepartmentID) Then

            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(vDepartmentID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vDeptAmount) Then

            Dim dbNumericTemp6 As Double
            If Not Double.TryParse(CStr(vDeptAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vVATRate) Then
            Dim dbNumericTemp7 As Double
            If Not Double.TryParse(CStr(vVATRate), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
                vVATRate = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
        End If


        If Not Information.IsNothing(vHasVAT) Then
            Dim dbNumericTemp8 As Double
            If Not Double.TryParse(CStr(vHasVAT), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp8) Then
                vHasVAT = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function
    ' PRIVATE Methods (End)

    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the ACTInvoiceItem.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceItemNo As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vNominalCode As Object = Nothing, Optional ByRef vValue As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vDeptAmount As Object = Nothing, Optional ByRef vVATRate As Object = Nothing, Optional ByRef vHasVAT As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults
            'Developer Guide No.98
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vInvoiceID:=vInvoiceID, vInvoiceItemNo:=vInvoiceItemNo, vDescription:=CStr(vDescription), vNominalCode:=CStr(vNominalCode), vValue:=CByte(vValue), vCurrencyID:=CByte(vCurrencyID), vDepartmentID:=CByte(vDepartmentID), vDeptAmount:=CByte(vDeptAmount), vVATRate:=CByte(vVATRate), vHasVAT:=CByte(vHasVAT))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetDepartment
    '
    ' Description:
    '
    ' History: 08/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetDepartment(ByVal v_iDepartmentID As Integer, ByRef r_vDepartment As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT c.caption FROM PMCaption c, CostCentre D " & _
                   "WHERE c.caption_id = d.caption_id " & _
                   "AND d.costcentre_id = {department_id} " & _
                   "AND d.is_deleted = 0 " & _
                   "AND d.effective_date <= {effective_date}"

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add them
            m_lReturn = m_oDatabase.Parameters.Add(sName:="department_id", vValue:=CStr(v_iDepartmentID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=ToSafeDate(DateTime.Today), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDepartment", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the department


            r_vDepartment = vResultArray(0, 0)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDepartment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDepartment", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the supplied ACTInvoiceItem property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByRef oACTInvoiceItem As bACTInvoiceItem.ACTInvoiceItem, ByRef iStatus As Integer, Optional ByRef vInvoiceID As Integer = 0, Optional ByRef vInvoiceItemNo As Integer = 0, Optional ByRef vDescription As String = "", Optional ByRef vNominalCode As String = "", Optional ByRef vValue As Double = 0, Optional ByRef vCurrencyID As Integer = 0, Optional ByRef vDepartmentID As Integer = 0, Optional ByRef vDeptAmount As Byte = 0, Optional ByRef vVATRate As Byte = 0, Optional ByRef vHasVAT As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Default Any Missing Parameters
            m_lReturn = DefaultParameters(bDefaultAll:=False, vInvoiceID:=vInvoiceID, vInvoiceItemNo:=vInvoiceItemNo, vDescription:=vDescription, vNominalCode:=vNominalCode, vValue:=vValue, vCurrencyID:=vCurrencyID, vDepartmentID:=vDepartmentID, vDeptAmount:=vDeptAmount, vVATRate:=vVATRate, vHasVAT:=vHasVAT)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        m_lReturn = Validate(vInvoiceID:=vInvoiceID, vInvoiceItemNo:=vInvoiceItemNo, vDescription:=vDescription, vNominalCode:=vNominalCode, vValue:=vValue, vCurrencyID:=vCurrencyID, vDepartmentID:=vDepartmentID, vDeptAmount:=vDeptAmount, vVATRate:=vVATRate, vHasVAT:=vHasVAT)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False

        ' Set Property values.
        With oACTInvoiceItem


            If Not Information.IsNothing(vInvoiceID) Then
                If .InvoiceID <> vInvoiceID Then
                    .InvoiceID = vInvoiceID
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vInvoiceItemNo) Then
                If .InvoiceItemNo <> vInvoiceItemNo Then
                    .InvoiceItemNo = vInvoiceItemNo
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vDescription) Then
                If .Description.Trim() <> vDescription.Trim() Then
                    .Description = vDescription
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vNominalCode) Then
                If .NominalCode.Trim() <> vNominalCode.Trim() Then
                    .NominalCode = vNominalCode
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vValue) Then
                If .Value <> vValue Then
                    .Value = vValue
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vCurrencyID) Then
                If .CurrencyID <> vCurrencyID Then
                    .CurrencyID = vCurrencyID
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vDepartmentID) Then
                If .DepartmentID <> vDepartmentID Then
                    .DepartmentID = vDepartmentID
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vDeptAmount) Then
                If .DeptAmount <> vDeptAmount Then
                    .DeptAmount = vDeptAmount
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vVATRate) Then
                If .VATRate <> vVATRate Then
                    .VATRate = vVATRate
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vHasVAT) Then
                If .HasVat <> vHasVAT Then
                    .HasVat = vHasVAT
                    bDataChanged = True
                End If
            End If

            ' If we have changed one of the properties, update the status
            If bDataChanged Then
                .DatabaseStatus = iStatus
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Public)
    '
    ' Description: Returns the supplied ACTInvoiceItem property values.
    '
    ' ***************************************************************** '
    'Developer Guide No.101
    Private Function GetProperties(ByRef oACTInvoiceItem As bACTInvoiceItem.ACTInvoiceItem, ByRef iStatus As Integer, Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceItemNo As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vNominalCode As String = "", Optional ByRef vValue As Object = Nothing, Optional ByRef vCurrencyID As Integer = 0, Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vDeptAmount As Object = Nothing, Optional ByRef vVATRate As Object = Nothing, Optional ByRef vHasVAT As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oACTInvoiceItem


            vInvoiceID = .InvoiceID


            vInvoiceItemNo = .InvoiceItemNo


            vDescription = .Description


            vNominalCode = .NominalCode


            vValue = .Value


            vCurrencyID = .CurrencyID


            vDepartmentID = .DepartmentID


            vDeptAmount = .DeptAmount


            vVATRate = .VATRate


            vHasVAT = .HasVat

            iStatus = .DatabaseStatus

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SelectItem (Public)
    '
    ' Description: Reads the Base Details from the Database .
    '
    ' ***************************************************************** '
    Public Function SelectItem(ByRef oACTInvoiceItem As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With oACTInvoiceItem

                ' Select a record from the database
                'm_lReturn& = .SelectSingle()
                '
                'If (m_lReturn& <> PMTrue) Then
                '    SelectItem = PMFalse
                '    Exit Function
                'End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Private)
    '
    ' Description: Check Mandatory parameters have been passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CheckMandatory) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckMandatory(Optional ByRef vInvoiceID As Object = Nothing, Optional ByRef vInvoiceItemNo As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vNominalCode As Object = Nothing, Optional ByRef vValue As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vDeptAmount As Object = Nothing, Optional ByRef vVATRate As Object = Nothing, Optional ByRef vHasVAT As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' {* USER DEFINED CODE (Begin) *}
    '


    'If (Information.IsNothing(vDescription)) Or (Object.Equals(vDescription, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Information.IsNothing(vNominalCode)) Or (Object.Equals(vNominalCode, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Information.IsNothing(vValue)) Or (Object.Equals(vValue, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Information.IsNothing(vCurrencyID)) Or (Object.Equals(vCurrencyID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '
    ' {* USER DEFINED CODE (End) *}
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
