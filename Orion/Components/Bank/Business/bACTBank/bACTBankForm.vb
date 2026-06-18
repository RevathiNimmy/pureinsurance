Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
'Developer Guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 08/09/1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Bank.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 09/12/2003
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
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of Banks (Private)
    Private m_oBanks As bACTBank.Banks

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business

    ' Primary Keys to work with
    ' Source ID
    ' Insurance File ID
    Private m_lInsuranceFileID As Integer
    ' Risk ID
    Private m_lRiskID As Integer

    ' CTAF 181000
    Private m_iBankID As Integer


    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' CTAF 181000
    Public ReadOnly Property BankID() As Integer
        Get
            Return m_iBankID
        End Get
    End Property

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oBanks.Count()
                    m_lCurrentRecord = m_oBanks.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oBanks.Count()

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

    Public Property SourceID() As Integer
        Get

            Return m_iSourceID

        End Get
        Set(ByVal Value As Integer)

            m_iSourceID = Value

        End Set
    End Property



    Public Property RiskID() As Integer
        Get

            Return m_lRiskID

        End Get
        Set(ByVal Value As Integer)

            m_lRiskID = Value

        End Set
    End Property
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFOrion

        End Get
    End Property


    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

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
            '
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


            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Get Reference to Database
            'ECK 23/7/99 Use component services
            '    Set m_oDatabase = GetOrionDatabase( _
            ''                            lOpenStatus:=m_lReturn, _
            ''                            bCloseDatabase:=m_bCloseDatabase, _
            ''                            vDatabase:=vDatabase)


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            ' Default RiskID to 0 (So we will work at Ins File Level by default)
            m_lRiskID = 0

            ' Create Banks Collection
            m_oBanks = New bACTBank.Banks()

            ' Create PMLookup Business Object
            m_oLookup = New bPMLookup.Business()

            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'JSB 28/10/98
            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion


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
                m_oBanks = Nothing
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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single Bank directly into the database.
    '        Note: The Bank will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vBankId As Integer = 0, Optional ByRef vCode As Object = Nothing, Optional ByRef vBranchCode As Object = Nothing, Optional ByRef vBankName As Object = Nothing, Optional ByRef vHeadOffice As Object = Nothing, Optional ByRef vBankAddress1 As Object = Nothing, Optional ByRef vBankAddress2 As Object = Nothing, Optional ByRef vBankAddress3 As Object = Nothing, Optional ByRef vBankAddress4 As Object = Nothing, Optional ByRef vBankPostalCode As Object = Nothing, Optional ByRef vBankCountry As Object = Nothing, Optional ByRef vBankPhoneAreaCode As Object = Nothing, Optional ByRef vBankPhoneNumber As Object = Nothing, Optional ByRef vBankPhoneExtension As Object = Nothing, Optional ByRef vBankFaxAreaCode As Object = Nothing, Optional ByRef vBankFaxNumber As Object = Nothing, Optional ByRef vBankFaxExtension As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vBankAccountType As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oBank As bACTBank.Bank

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Bank
            oBank = New bACTBank.Bank()

            ' Populate Bank Attributes


















            m_lReturn = CType(SetProperties(oBank, gPMConstants.PMEComponentAction.PMAdd, vBankId:=vBankId, vCode:=CStr(vCode), vBranchCode:=CStr(vBranchCode), vBankName:=CStr(vBankName), vHeadOffice:=CInt(vHeadOffice), vBankAddress1:=CStr(vBankAddress1), vBankAddress2:=CStr(vBankAddress2), vBankAddress3:=CStr(vBankAddress3), vBankAddress4:=CStr(vBankAddress4), vBankPostalCode:=CStr(vBankPostalCode), vBankCountry:=CInt(vBankCountry), vBankPhoneAreaCode:=CStr(vBankPhoneAreaCode), vBankPhoneNumber:=CStr(vBankPhoneNumber), vBankPhoneExtension:=CStr(vBankPhoneExtension), vBankFaxAreaCode:=CStr(vBankFaxAreaCode), vBankFaxNumber:=CStr(vBankFaxNumber), vBankFaxExtension:=CStr(vBankFaxExtension), vComments:=CStr(vComments), vBankAccountType:=CInt(vBankAccountType)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Bank to the Database
            m_lReturn = CType(AddItem(oBank), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the Bank Added

            If Not Information.IsNothing(vBankId) Then
                vBankId = oBank.BankID
            End If

            ' {* USER DEFINED CODE (End) *}

            oBank = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single Bank directly from the database.
    '        Note: The Bank will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vBankId As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vBranchCode As Object = Nothing, Optional ByRef vBankName As Object = Nothing, Optional ByRef vHeadOffice As Object = Nothing, Optional ByRef vBankAddress1 As Object = Nothing, Optional ByRef vBankAddress2 As Object = Nothing, Optional ByRef vBankAddress3 As Object = Nothing, Optional ByRef vBankAddress4 As Object = Nothing, Optional ByRef vBankPostalCode As Object = Nothing, Optional ByRef vBankCountry As Object = Nothing, Optional ByRef vBankPhoneAreaCode As Object = Nothing, Optional ByRef vBankPhoneNumber As Object = Nothing, Optional ByRef vBankPhoneExtension As Object = Nothing, Optional ByRef vBankFaxAreaCode As Object = Nothing, Optional ByRef vBankFaxNumber As Object = Nothing, Optional ByRef vBankFaxExtension As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vBankAccountType As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oBank As bACTBank.Bank

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Bank
            oBank = New bACTBank.Bank()

            ' Populate Bank Attributes



















            m_lReturn = CType(SetProperties(oBank, gPMConstants.PMEComponentAction.PMDelete, vBankId:=CInt(vBankId), vCode:=CStr(vCode), vBranchCode:=CStr(vBranchCode), vBankName:=CStr(vBankName), vHeadOffice:=CInt(vHeadOffice), vBankAddress1:=CStr(vBankAddress1), vBankAddress2:=CStr(vBankAddress2), vBankAddress3:=CStr(vBankAddress3), vBankAddress4:=CStr(vBankAddress4), vBankPostalCode:=CStr(vBankPostalCode), vBankCountry:=CInt(vBankCountry), vBankPhoneAreaCode:=CStr(vBankPhoneAreaCode), vBankPhoneNumber:=CStr(vBankPhoneNumber), vBankPhoneExtension:=CStr(vBankPhoneExtension), vBankFaxAreaCode:=CStr(vBankFaxAreaCode), vBankFaxNumber:=CStr(vBankFaxNumber), vBankFaxExtension:=CStr(vBankFaxExtension), vComments:=CStr(vComments), vBankAccountType:=CInt(vBankAccountType)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Bank to the Database
            m_lReturn = CType(DeleteItem(oBank), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oBank = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the Bank.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vBankId As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vBranchCode As Object = Nothing, Optional ByRef vBankName As Object = Nothing, Optional ByRef vHeadOffice As Object = Nothing, Optional ByRef vBankAddress1 As Object = Nothing, Optional ByRef vBankAddress2 As Object = Nothing, Optional ByRef vBankAddress3 As Object = Nothing, Optional ByRef vBankAddress4 As Object = Nothing, Optional ByRef vBankPostalCode As Object = Nothing, Optional ByRef vBankCountry As Object = Nothing, Optional ByRef vBankPhoneAreaCode As Object = Nothing, Optional ByRef vBankPhoneNumber As Object = Nothing, Optional ByRef vBankPhoneExtension As Object = Nothing, Optional ByRef vBankFaxAreaCode As Object = Nothing, Optional ByRef vBankFaxNumber As Object = Nothing, Optional ByRef vBankFaxExtension As Object = Nothing, Optional ByRef vComments As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults


















            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vBankId:=CByte(vBankId), vCode:=CStr(vCode), vBranchCode:=CStr(vBranchCode), vBankName:=CStr(vBankName), vHeadOffice:=CByte(vHeadOffice), vBankAddress1:=CStr(vBankAddress1), vBankAddress2:=CStr(vBankAddress2), vBankAddress3:=CStr(vBankAddress3), vBankAddress4:=CStr(vBankAddress4), vBankPostalCode:=CStr(vBankPostalCode), vBankCountry:=CByte(vBankCountry), vBankPhoneAreaCode:=CStr(vBankPhoneAreaCode), vBankPhoneNumber:=CStr(vBankPhoneNumber), vBankPhoneExtension:=CStr(vBankPhoneExtension), vBankFaxAreaCode:=CStr(vBankFaxAreaCode), vBankFaxNumber:=CStr(vBankFaxNumber), vBankFaxExtension:=CStr(vBankFaxExtension), vComments:=CStr(vComments)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCaptions (Public)
    '
    ' Description: Get the requested caption fields for a record.
    '
    ' ***************************************************************** '
    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer

        Dim result As Integer = 0
        'Developer Guide no.21
        Dim oFields As ADODB.Fields


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vFieldArray) Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Parameter vFieldArray must be a Variant Array", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have a Table name
            'BB If (IsMissing(vTable) = False) Then

            ' Is this our table
            '    If (Trim$(vTable) <> PMTableBank) Then

            '        GetCaptions = PMInvalidRequest


            '       Exit Function

            '    End If

            'End If

            ' Get the Captions ourself

            ' Check that this record exists
            m_lReturn = CType(CheckID(vID:=vID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Resize the Temporary Results Array
            Dim vResults(vFieldArray.GetUpperBound(0)) As Object

            ' Get a reference to the Fields returned
            oFields = m_oDatabase.Records.Item(1).Fields()

            With oFields

                ' For Each Field requested
                For lSub As Integer = vFieldArray.GetLowerBound(0) To vFieldArray.GetUpperBound(0)

                    'AK 230702 - check for null value

                    If Not (Convert.IsDBNull(oFields(vFieldArray(lSub))) Or IsNothing(oFields(vFieldArray(lSub)))) Then
                        ' Store the results in the Temporary results array

                        vResults(lSub) = oFields(vFieldArray(lSub))
                    Else
                        Select Case oFields(vFieldArray(lSub)).Type
                            'Developer Guide no.47(no solutions)
                            'start
                            'Case DbType.String, DbType.String, DbType.String, adLongVarWChar, DbType.String, DbType.String, adWChar

                            '    vResults(lSub) = ""

                            'Case DbType.Date, adDBDate

                            'vResults(lSub) = -1
                            'end
                            Case Else

                                vResults(lSub) = 0
                        End Select
                    End If

                Next lSub

            End With

            ' Assign the results
            vResultArray = vResults

            ' Release the reference to the Fields
            oFields = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required Banks and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vBankId As Object = Nothing, Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oBank As bACTBank.Bank

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oBanks.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Information.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Do we have a key

            If Not Information.IsNothing(vBankId) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vBankId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vBankID =" & CStr(vBankId), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Add the BankID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Bank_id", vValue:=CStr(vBankId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                ' No Key, Get All Records

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound

            Else

                ' Yes, load them into the collection

                For lSub As Integer = 1 To lRecordCount

                    ' Create New Bank
                    oBank = New bACTBank.Bank()
                    'Developer Guide no.162
                    m_lReturn = CType(SetPropertiesFromDB(oBank:=oBank, lRecordNumber:=lSub - 1), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add Bank to collection
                    m_lReturn = CType(m_oBanks.Add(oNewBank:=oBank), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oBank = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required Banks and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vBankId As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vBranchCode As Object = Nothing, Optional ByRef vBankName As Object = Nothing, Optional ByRef vHeadOffice As Object = Nothing, Optional ByRef vBankAddress1 As Object = Nothing, Optional ByRef vBankAddress2 As Object = Nothing, Optional ByRef vBankAddress3 As Object = Nothing, Optional ByRef vBankAddress4 As Object = Nothing, Optional ByRef vBankPostalCode As Object = Nothing, Optional ByRef vBankCountry As Object = Nothing, Optional ByRef vBankPhoneAreaCode As Object = Nothing, Optional ByRef vBankPhoneNumber As Object = Nothing, Optional ByRef vBankPhoneExtension As Object = Nothing, Optional ByRef vBankFaxAreaCode As Object = Nothing, Optional ByRef vBankFaxNumber As Object = Nothing, Optional ByRef vBankFaxExtension As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vBankAccountType As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oBank As bACTBank.Bank
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oBanks.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oBank = m_oBanks.Item(m_lCurrentRecord)

            ' Get the Bank Property Values



















            'Developer GUide no.98
            m_lReturn = GetProperties(oBank, iStatus, vBankId:=vBankId, vCode:=vCode, vBranchCode:=vBranchCode, vBankName:=vBankName, vHeadOffice:=vHeadOffice, vBankAddress1:=vBankAddress1, vBankAddress2:=vBankAddress2, vBankAddress3:=vBankAddress3, vBankAddress4:=vBankAddress4, vBankPostalCode:=vBankPostalCode, vBankCountry:=vBankCountry, vBankPhoneAreaCode:=vBankPhoneAreaCode, vBankPhoneNumber:=vBankPhoneNumber, vBankPhoneExtension:=vBankPhoneExtension, vBankFaxAreaCode:=vBankFaxAreaCode, vBankFaxNumber:=vBankFaxNumber, vBankFaxExtension:=vBankFaxExtension, vComments:=vComments, vBankAccountType:=vBankAccountType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oBank = Nothing


            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetOtherDetails
    '
    ' Description: Get other details from DB, for related parties.
    '
    ' ***************************************************************** '
    Public Function GetOtherDetails(Optional ByRef vHeadOfficeId As Object = Nothing, Optional ByRef vHeadOFficeName As String = "") As Integer


        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim oACTBank As bACTBank.Bank


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyPC - ned to go to core for agent
            oACTBank = New bACTBank.Bank()
            'Developer Guide no. 9
            m_lReturn = oACTBank.Initialise()





            If (Not Information.IsNothing(vHeadOfficeId)) And (Not Object.Equals(vHeadOfficeId, Nothing)) And (Not (Convert.IsDBNull(vHeadOfficeId) Or IsNothing(vHeadOfficeId))) Then

                'Get form DB

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetHeadOfficeDetailsSQL & CStr(vHeadOfficeId), sSQLName:=ACGetHeadOfficeDetailsName, bStoredProcedure:=ACGetHeadOfficeDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'return the data
                If Not Information.IsArray(vResultArray) Then
                    vHeadOFficeName = ""
                Else

                    vHeadOFficeName = CStr(vResultArray(0, 0))
                End If

                'put summat ere

            End If

            oACTBank = Nothing




            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetOtherDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a Account.
    '
    ' ***************************************************************** '
    'Modified,Developer Guide no.101
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim dtEffectiveDate As Date
        Const cBankCountry As Integer = 0

        ' {* USER DEFINED CODE (Begin) *}
        Dim oBank As bACTBank.Bank
        Dim vTabArray(3, cBankCountry) As Object
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing
            ' Reset Table Array

            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, cBankCountry) = gPMConstants.PMLookupCountry

            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oBank = m_oBanks.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, cBankCountry) = ""

                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    With oBank

                        ' {* USER DEFINED CODE (Begin) *}

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, cBankCountry) = .BankCountry
                        dtEffectiveDate = DateTime.Now
                        ' {* USER DEFINED CODE (End) *}

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oBank

                        ' {* USER DEFINED CODE (Begin) *}

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, cBankCountry) = .BankCountry
                        ' {* USER DEFINED CODE (End) *}

                    End With
                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

            End Select


            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array

            vTableArray = vTabArray

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetAccountDetails
    '
    ' Description: Get Account details for party.
    ' ***************************************************************** '
    Public Function GetAccountDetails(ByRef vBankId As Object, ByRef vAccounts(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add("bank_id", CStr(Conversion.Val(CStr(vBankId))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBankAccountSQL, sSQLName:=ACGetBankAccountName, bStoredProcedure:=ACGetBankAccountStored, vResultArray:=vAccounts)


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAccountDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied Bank into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vBankId As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vBranchCode As Object = Nothing, Optional ByRef vBankName As Object = Nothing, Optional ByRef vHeadOffice As Object = Nothing, Optional ByRef vBankAddress1 As Object = Nothing, Optional ByRef vBankAddress2 As Object = Nothing, Optional ByRef vBankAddress3 As Object = Nothing, Optional ByRef vBankAddress4 As Object = Nothing, Optional ByRef vBankPostalCode As Object = Nothing, Optional ByRef vBankCountry As Object = Nothing, Optional ByRef vBankPhoneAreaCode As Object = Nothing, Optional ByRef vBankPhoneNumber As Object = Nothing, Optional ByRef vBankPhoneExtension As Object = Nothing, Optional ByRef vBankFaxAreaCode As Object = Nothing, Optional ByRef vBankFaxNumber As Object = Nothing, Optional ByRef vBankFaxExtension As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vBankAccountType As Object = Nothing, Optional ByVal vUniqueId As String = "", Optional ByVal vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim oBank As bACTBank.Bank

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oBanks.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new Bank
            oBank = New bACTBank.Bank()

            ' Populate Bank Attributes



















            m_lReturn = CType(SetProperties(oBank, gPMConstants.PMEComponentAction.PMAdd, vBankId:=CInt(vBankId), vCode:=CStr(vCode), vBranchCode:=CStr(vBranchCode), vBankName:=CStr(vBankName), vHeadOffice:=CInt(vHeadOffice), vBankAddress1:=CStr(vBankAddress1), vBankAddress2:=CStr(vBankAddress2), vBankAddress3:=CStr(vBankAddress3), vBankAddress4:=CStr(vBankAddress4), vBankPostalCode:=CStr(vBankPostalCode), vBankCountry:=CInt(vBankCountry), vBankPhoneAreaCode:=CStr(vBankPhoneAreaCode), vBankPhoneNumber:=CStr(vBankPhoneNumber), vBankPhoneExtension:=CStr(vBankPhoneExtension), vBankFaxAreaCode:=CStr(vBankFaxAreaCode), vBankFaxNumber:=CStr(vBankFaxNumber), vBankFaxExtension:=CStr(vBankFaxExtension), vComments:=CStr(vComments), vBankAccountType:=CInt(vBankAccountType), vUniqueId:=vUniqueId, vScreenHierarchy:=vScreenHierarchy), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oBank = Nothing
                Return result
            End If

            ' Add Bank to collection
            m_lReturn = CType(m_oBanks.Add(oNewBank:=oBank), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oBank = Nothing
                Return result
            End If

            oBank = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the Bank
    '              specified and updates the Bank with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vBankId As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vBranchCode As Object = Nothing, Optional ByRef vBankName As Object = Nothing, Optional ByRef vHeadOffice As Object = Nothing, Optional ByRef vBankAddress1 As Object = Nothing, Optional ByRef vBankAddress2 As Object = Nothing, Optional ByRef vBankAddress3 As Object = Nothing, Optional ByRef vBankAddress4 As Object = Nothing, Optional ByRef vBankPostalCode As Object = Nothing, Optional ByRef vBankCountry As Object = Nothing, Optional ByRef vBankPhoneAreaCode As Object = Nothing, Optional ByRef vBankPhoneNumber As Object = Nothing, Optional ByRef vBankPhoneExtension As Object = Nothing, Optional ByRef vBankFaxAreaCode As Object = Nothing, Optional ByRef vBankFaxNumber As Object = Nothing, Optional ByRef vBankFaxExtension As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vBankAccountType As Object = Nothing, Optional ByVal vUniqueId As String = "", Optional ByVal vScreenHierarchy As String = "") As Integer


        Dim result As Integer = 0
        Dim oBank As bACTBank.Bank
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oBanks.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oBank = m_oBanks.Item(lRow)

            ' Check the Status of the Bank

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oBank.DatabaseStatus
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

            ' Update Bank Attributes



















            m_lReturn = CType(SetProperties(oBank, iStatus, vBankId:=CInt(vBankId), vCode:=CStr(vCode), vBranchCode:=CStr(vBranchCode), vBankName:=CStr(vBankName), vHeadOffice:=CInt(vHeadOffice), vBankAddress1:=CStr(vBankAddress1), vBankAddress2:=CStr(vBankAddress2), vBankAddress3:=CStr(vBankAddress3), vBankAddress4:=CStr(vBankAddress4), vBankPostalCode:=CStr(vBankPostalCode), vBankCountry:=CInt(vBankCountry), vBankPhoneAreaCode:=CStr(vBankPhoneAreaCode), vBankPhoneNumber:=CStr(vBankPhoneNumber), vBankPhoneExtension:=CStr(vBankPhoneExtension), vBankFaxAreaCode:=CStr(vBankFaxAreaCode), vBankFaxNumber:=CStr(vBankFaxNumber), vBankFaxExtension:=CStr(vBankFaxExtension), vComments:=CStr(vComments), vBankAccountType:=CInt(vBankAccountType), vUniqueId:=vUniqueId, vScreenHierarchy:=vScreenHierarchy), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oBank = Nothing
                Return result
            End If

            ' Release reference to Bank
            oBank = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified Bank can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oBank As bACTBank.Bank

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oBanks.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oBank = m_oBanks.Item(lRow)

            ' Check the Status of the Bank

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oBank.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oBank.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oBank.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to Bank
            oBank = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
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
            For lSub As Integer = 1 To m_oBanks.Count()
                Select Case m_oBanks.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateBank
    '
    ' Description:
    '
    ' History: 18/10/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateBank(ByVal v_iBankID As Integer, ByVal v_lBankAccountID As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "UPDATE bankaccount SET " &
                    "bank_id = " & v_iBankID & ", " &
                    "UniqueId = '" & Replace(v_sUniqueId, "'", "''") & "', " &
                    "ScreenHierarchy = '" & Replace(v_sScreenHierarchy, "'", "''") & "' " &
                    "WHERE bankaccount_id = " & CStr(v_lBankAccountID)


            ' Perform the Update
            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateBank", bStoredProcedure:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateBank Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBank", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oBank As bACTBank.Bank
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oBanks.Count()
                oBank = m_oBanks.Item(lSub)


                Select Case oBank.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = CType(AddItem(oBank), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = CType(UpdateItem(oBank), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = CType(DeleteItem(oBank), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oBank = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oBanks.Count()

                        ' With the item
                        With m_oBanks.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oBanks.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oBank As bACTBank.Bank) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = CType(AddInputParam(oBank:=oBank), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add BankID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Bank_id", vValue:=CStr(oBank.BankID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oBank.BankID = m_oDatabase.Parameters.Item("Bank_id").Value

        ' CTAF 18/10/00
        m_iBankID = oBank.BankID

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
    Private Function UpdateItem(ByRef oBank As bACTBank.Bank) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddInputParam(oBank:=oBank), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add BankID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Bank_id", vValue:=CStr(oBank.BankID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' BB Not required by Orion
        ' Add Timestamp as an INPUT param for an update
        'm_lReturn = m_oDatabase.Parameters.Add( _
        'sName:="timestamp", _
        'vValue:=oBank.Timestamp, _
        'iDirection:=PMParamInput, _
        'iDataType:=PMBinary)

        'If (m_lReturn <> PMTrue) Then
        '    UpdateItem = PMFalse
        '    Exit Function
        'End If

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

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Private Function DeleteItem(ByRef oBank As bACTBank.Bank) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the BankID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Bank_id", vValue:=CStr(oBank.BankID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=oBank.UniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=oBank.ScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If record wasn't deleted, error
        If lRecordsAffected > 0 Then
            ' Deleted, No action required
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied Bank properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oBank As bACTBank.Bank, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'Developer Guide no 21
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oBank

            .BankID = oFields("bank_id")

            If Convert.IsDBNull(oFields("code")) Or IsNothing(oFields("code")) Then
                .Code = ""
            Else
                .Code = oFields("code")
            End If

            If Convert.IsDBNull(oFields("branch_code")) Or IsNothing(oFields("branch_code")) Then
                .BranchCode = ""
            Else
                .BranchCode = oFields("branch_code")
            End If

            If Convert.IsDBNull(oFields("bank_name")) Or IsNothing(oFields("bank_name")) Then
                .BankName = ""
            Else
                .BankName = oFields("bank_name")
            End If
            .HeadOffice = oFields("head_office")

            If Convert.IsDBNull(oFields("bank_address1")) Or IsNothing(oFields("bank_address1")) Then
                .BankAddress1 = ""
            Else
                .BankAddress1 = oFields("bank_address1")
            End If

            If Convert.IsDBNull(oFields("bank_address2")) Or IsNothing(oFields("bank_address2")) Then
                .BankAddress2 = ""
            Else
                .BankAddress2 = oFields("bank_address2")
            End If

            If Convert.IsDBNull(oFields("bank_address3")) Or IsNothing(oFields("bank_address3")) Then
                .BankAddress3 = ""
            Else
                .BankAddress3 = oFields("bank_address3")
            End If

            If Convert.IsDBNull(oFields("bank_address4")) Or IsNothing(oFields("bank_address4")) Then
                .BankAddress4 = ""
            Else
                .BankAddress4 = oFields("bank_address4")
            End If

            If Convert.IsDBNull(oFields("bank_postal_code")) Or IsNothing(oFields("bank_postal_code")) Then
                .BankPostalCode = ""
            Else
                .BankPostalCode = oFields("bank_postal_code")
            End If

            If Convert.IsDBNull(oFields("bank_country")) Or IsNothing(oFields("bank_country")) Then
                .BankCountry = 0
            Else
                .BankCountry = oFields("bank_country")
            End If

            If Convert.IsDBNull(oFields("bank_phone_area_code")) Or IsNothing(oFields("bank_phone_area_code")) Then
                .BankPhoneAreaCode = ""
            Else
                .BankPhoneAreaCode = oFields("bank_phone_area_code")
            End If

            If Convert.IsDBNull(oFields("bank_phone_number")) Or IsNothing(oFields("bank_phone_number")) Then
                .BankPhoneNumber = ""
            Else
                .BankPhoneNumber = oFields("bank_phone_number")
            End If

            If Convert.IsDBNull(oFields("bank_phone_extension")) Or IsNothing(oFields("bank_phone_extension")) Then
                .BankPhoneExtension = ""
            Else
                .BankPhoneExtension = oFields("bank_phone_extension")
            End If

            If Convert.IsDBNull(oFields("bank_fax_area_code")) Or IsNothing(oFields("bank_fax_area_code")) Then
                .BankFaxAreaCode = ""
            Else
                .BankFaxAreaCode = oFields("bank_fax_area_code")
            End If

            If Convert.IsDBNull(oFields("bank_fax_number")) Or IsNothing(oFields("bank_fax_number")) Then
                .BankFaxNumber = ""
            Else
                .BankFaxNumber = oFields("bank_fax_number")
            End If

            If Convert.IsDBNull(oFields("bank_fax_extension")) Or IsNothing(oFields("bank_fax_extension")) Then
                .BankFaxExtension = ""
            Else
                .BankFaxExtension = oFields("bank_fax_extension")
            End If

            If Convert.IsDBNull(oFields("comments")) Or IsNothing(oFields("comments")) Then
                .Comments = ""
            Else
                .Comments = oFields("comments")
            End If


            If Convert.IsDBNull(oFields("bank_account_type_id")) Or IsNothing(oFields("bank_account_type_id")) Then
                .BankAccountType = 0
            Else
                .BankAccountType = oFields("bank_account_type_id")
            End If

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied Bank property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByRef oBank As bACTBank.Bank, ByRef iStatus As Integer, Optional ByRef vBankId As Integer = 0, Optional ByRef vCode As String = "", Optional ByRef vBranchCode As String = "", Optional ByRef vBankName As String = "", Optional ByRef vHeadOffice As Integer = 0, Optional ByRef vBankAddress1 As String = "", Optional ByRef vBankAddress2 As String = "", Optional ByRef vBankAddress3 As String = "", Optional ByRef vBankAddress4 As String = "", Optional ByRef vBankPostalCode As String = "", Optional ByRef vBankCountry As Integer = 0, Optional ByRef vBankPhoneAreaCode As String = "", Optional ByRef vBankPhoneNumber As String = "", Optional ByRef vBankPhoneExtension As String = "", Optional ByRef vBankFaxAreaCode As String = "", Optional ByRef vBankFaxNumber As String = "", Optional ByRef vBankFaxExtension As String = "", Optional ByRef vComments As String = "", Optional ByRef vBankAccountType As Integer = 0, Optional ByVal vUniqueId As String = "", Optional ByVal vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CType(CheckMandatory(vBankId:=vBankId, vCode:=vCode, vBranchCode:=vBranchCode, vBankName:=vBankName, vHeadOffice:=vHeadOffice, vBankAddress1:=vBankAddress1, vBankAddress2:=vBankAddress2, vBankAddress3:=vBankAddress3, vBankAddress4:=vBankAddress4, vBankPostalCode:=vBankPostalCode, vBankCountry:=vBankCountry, vBankPhoneAreaCode:=vBankPhoneAreaCode, vBankPhoneNumber:=vBankPhoneNumber, vBankPhoneExtension:=vBankPhoneExtension, vBankFaxAreaCode:=vBankFaxAreaCode, vBankFaxNumber:=vBankFaxNumber, vBankFaxExtension:=vBankFaxExtension, vComments:=vComments, vBankAccountType:=vBankAccountType), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Default Any Missing Parameters
            m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vBankId:=vBankId, vCode:=vCode, vBranchCode:=vBranchCode, vBankName:=vBankName, vHeadOffice:=vHeadOffice, vBankAddress1:=vBankAddress1, vBankAddress2:=vBankAddress2, vBankAddress3:=vBankAddress3, vBankAddress4:=vBankAddress4, vBankPostalCode:=vBankPostalCode, vBankCountry:=vBankCountry, vBankPhoneAreaCode:=vBankPhoneAreaCode, vBankPhoneNumber:=vBankPhoneNumber, vBankPhoneExtension:=vBankPhoneExtension, vBankFaxAreaCode:=vBankFaxAreaCode, vBankFaxNumber:=vBankFaxNumber, vBankFaxExtension:=vBankFaxExtension, vComments:=vComments, vBankAccountType:=vBankAccountType), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        m_lReturn = CType(Validate(vBankId:=vBankId, vCode:=vCode, vBranchCode:=vBranchCode, vBankName:=vBankName, vHeadOffice:=vHeadOffice, vBankAddress1:=vBankAddress1, vBankAddress2:=vBankAddress2, vBankAddress3:=vBankAddress3, vBankAddress4:=vBankAddress4, vBankPostalCode:=vBankPostalCode, vBankCountry:=vBankCountry, vBankPhoneAreaCode:=vBankPhoneAreaCode, vBankPhoneNumber:=vBankPhoneNumber, vBankPhoneExtension:=vBankPhoneExtension, vBankFaxAreaCode:=vBankFaxAreaCode, vBankFaxNumber:=vBankFaxNumber, vBankFaxExtension:=vBankFaxExtension, vComments:=vComments, vBankAccountType:=vBankAccountType), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oBank


            If Not Information.IsNothing(vBankId) Then
                If .BankID <> vBankId Then
                    .BankID = vBankId
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vCode) Then
                If .Code.Trim() <> vCode.Trim() Then
                    .Code = vCode
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBranchCode) Then
                If .BranchCode.Trim() <> vBranchCode.Trim() Then
                    .BranchCode = vBranchCode
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBankName) Then
                If .BankName.Trim() <> vBankName.Trim() Then
                    .BankName = vBankName
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vHeadOffice) Then
                If .HeadOffice <> vHeadOffice Then
                    .HeadOffice = vHeadOffice
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBankAddress1) Then
                If .BankAddress1.Trim() <> vBankAddress1.Trim() Then
                    .BankAddress1 = vBankAddress1
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBankAddress2) Then
                If .BankAddress2.Trim() <> vBankAddress2.Trim() Then
                    .BankAddress2 = vBankAddress2
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBankAddress3) Then
                If .BankAddress3.Trim() <> vBankAddress3.Trim() Then
                    .BankAddress3 = vBankAddress3
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBankAddress4) Then
                If .BankAddress4.Trim() <> vBankAddress4.Trim() Then
                    .BankAddress4 = vBankAddress4
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBankPostalCode) Then
                If .BankPostalCode.Trim() <> vBankPostalCode.Trim() Then
                    .BankPostalCode = vBankPostalCode
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBankCountry) Then
                If .BankCountry <> vBankCountry Then
                    .BankCountry = vBankCountry
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBankPhoneAreaCode) Then
                If .BankPhoneAreaCode.Trim() <> vBankPhoneAreaCode.Trim() Then
                    .BankPhoneAreaCode = vBankPhoneAreaCode
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBankPhoneNumber) Then
                If .BankPhoneNumber.Trim() <> vBankPhoneNumber.Trim() Then
                    .BankPhoneNumber = vBankPhoneNumber
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBankPhoneExtension) Then
                If .BankPhoneExtension.Trim() <> vBankPhoneExtension.Trim() Then
                    .BankPhoneExtension = vBankPhoneExtension
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBankFaxAreaCode) Then
                If .BankFaxAreaCode.Trim() <> vBankFaxAreaCode.Trim() Then
                    .BankFaxAreaCode = vBankFaxAreaCode
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBankFaxNumber) Then
                If .BankFaxNumber.Trim() <> vBankFaxNumber.Trim() Then
                    .BankFaxNumber = vBankFaxNumber
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vBankFaxExtension) Then
                If .BankFaxExtension.Trim() <> vBankFaxExtension.Trim() Then
                    .BankFaxExtension = vBankFaxExtension
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vComments) Then
                If .Comments.Trim() <> vComments.Trim() Then
                    .Comments = vComments
                    bDataChanged = True
                End If
            End If



            If Not Information.IsNothing(vBankAccountType) Then
                If .BankAccountType <> vBankAccountType Then
                    .BankAccountType = vBankAccountType
                    bDataChanged = True
                End If
            End If

            If Not String.IsNullOrEmpty(vUniqueId) Then
                .UniqueId = vUniqueId
                .ScreenHierarchy = vScreenHierarchy
            End If
            ' If we have changed one of the properties, update the status
            If bDataChanged Then
                .DatabaseStatus = iStatus
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Private)
    '
    ' Description: Returns the supplied Bank property values.
    '
    ' ***************************************************************** '
    Private Function GetProperties(ByRef oBank As bACTBank.Bank, ByRef iStatus As Integer, Optional ByRef vBankId As Integer = 0, Optional ByRef vCode As String = "", Optional ByRef vBranchCode As String = "", Optional ByRef vBankName As String = "", Optional ByRef vHeadOffice As Integer = 0, Optional ByRef vBankAddress1 As String = "", Optional ByRef vBankAddress2 As String = "", Optional ByRef vBankAddress3 As String = "", Optional ByRef vBankAddress4 As String = "", Optional ByRef vBankPostalCode As String = "", Optional ByRef vBankCountry As Integer = 0, Optional ByRef vBankPhoneAreaCode As String = "", Optional ByRef vBankPhoneNumber As String = "", Optional ByRef vBankPhoneExtension As String = "", Optional ByRef vBankFaxAreaCode As String = "", Optional ByRef vBankFaxNumber As String = "", Optional ByRef vBankFaxExtension As String = "", Optional ByRef vComments As String = "", Optional ByRef vBankAccountType As Integer = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oBank


            If Not Information.IsNothing(vBankId) Then
                vBankId = .BankID
            End If


            If Not Information.IsNothing(vCode) Then
                vCode = .Code
            End If


            If Not Information.IsNothing(vBranchCode) Then
                vBranchCode = .BranchCode
            End If


            If Not Information.IsNothing(vBankName) Then
                vBankName = .BankName
            End If


            If Not Information.IsNothing(vHeadOffice) Then
                vHeadOffice = .HeadOffice
            End If


            If Not Information.IsNothing(vBankAddress1) Then
                vBankAddress1 = .BankAddress1
            End If


            If Not Information.IsNothing(vBankAddress2) Then
                vBankAddress2 = .BankAddress2
            End If


            If Not Information.IsNothing(vBankAddress3) Then
                vBankAddress3 = .BankAddress3
            End If


            If Not Information.IsNothing(vBankAddress4) Then
                vBankAddress4 = .BankAddress4
            End If


            If Not Information.IsNothing(vBankPostalCode) Then
                vBankPostalCode = .BankPostalCode
            End If


            If Not Information.IsNothing(vBankCountry) Then
                vBankCountry = .BankCountry
            End If


            If Not Information.IsNothing(vBankPhoneAreaCode) Then
                vBankPhoneAreaCode = .BankPhoneAreaCode
            End If


            If Not Information.IsNothing(vBankPhoneNumber) Then
                vBankPhoneNumber = .BankPhoneNumber
            End If


            If Not Information.IsNothing(vBankPhoneExtension) Then
                vBankPhoneExtension = .BankPhoneExtension
            End If


            If Not Information.IsNothing(vBankFaxAreaCode) Then
                vBankFaxAreaCode = .BankFaxAreaCode
            End If


            If Not Information.IsNothing(vBankFaxNumber) Then
                vBankFaxNumber = .BankFaxNumber
            End If


            If Not Information.IsNothing(vBankFaxExtension) Then
                vBankFaxExtension = .BankFaxExtension
            End If


            If Not Information.IsNothing(vComments) Then
                vComments = .Comments
            End If


            If Not Information.IsNothing(vBankAccountType) Then
                vBankAccountType = .BankAccountType
            End If

            iStatus = .DatabaseStatus

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam(ByRef oBank As bACTBank.Bank) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase


            '        m_lReturn = .Parameters.Add( _
            ''              sName:="bank_id", _
            ''              vValue:=oBank.BankID, _
            ''              iDirection:=PMParamInput, _
            ''              iDataType:=PMInteger)
            '
            '        If (m_lReturn <> PMTrue) Then
            '            AddInputParam = PMFalse
            '            Exit Function
            '        End If

            m_lReturn = .Parameters.Add(sName:="code", vValue:=oBank.Code, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="branch_code", vValue:=oBank.BranchCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_name", vValue:=oBank.BankName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="head_office", vValue:=CStr(oBank.HeadOffice), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_address1", vValue:=oBank.BankAddress1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_address2", vValue:=oBank.BankAddress2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_address3", vValue:=oBank.BankAddress3, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_address4", vValue:=oBank.BankAddress4, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_postal_code", vValue:=oBank.BankPostalCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_country", vValue:=CStr(oBank.BankCountry), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_phone_area_code", vValue:=oBank.BankPhoneAreaCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_phone_number", vValue:=oBank.BankPhoneNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_phone_extension", vValue:=oBank.BankPhoneExtension, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_fax_area_code", vValue:=oBank.BankFaxAreaCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_fax_number", vValue:=oBank.BankFaxNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_fax_extension", vValue:=oBank.BankFaxExtension, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="comments", vValue:=oBank.Comments, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bank_account_type_id", vValue:=CStr(oBank.BankAccountType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=oBank.UniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=oBank.ScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Bank.
    '
    ' ***************************************************************** '
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vBankId As Byte = 0, Optional ByRef vCode As String = "", Optional ByRef vBranchCode As String = "", Optional ByRef vBankName As String = "", Optional ByRef vHeadOffice As Byte = 0, Optional ByRef vBankAddress1 As String = "", Optional ByRef vBankAddress2 As String = "", Optional ByRef vBankAddress3 As String = "", Optional ByRef vBankAddress4 As String = "", Optional ByRef vBankPostalCode As String = "", Optional ByRef vBankCountry As Byte = 0, Optional ByRef vBankPhoneAreaCode As String = "", Optional ByRef vBankPhoneNumber As String = "", Optional ByRef vBankPhoneExtension As String = "", Optional ByRef vBankFaxAreaCode As String = "", Optional ByRef vBankFaxNumber As String = "", Optional ByRef vBankFaxExtension As String = "", Optional ByRef vComments As String = "", Optional ByRef vBankAccountType As Byte = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Information.IsNothing(vBankId)) Or (vBankId.Equals(0)) Or (bDefaultAll) Then
            vBankId = 0
        End If



        If (Information.IsNothing(vCode)) Or (String.IsNullOrEmpty(vCode)) Or (bDefaultAll) Then
            vCode = ""
        End If



        If (Information.IsNothing(vBranchCode)) Or (String.IsNullOrEmpty(vBranchCode)) Or (bDefaultAll) Then
            vBranchCode = ""
        End If



        If (Information.IsNothing(vBankName)) Or (String.IsNullOrEmpty(vBankName)) Or (bDefaultAll) Then
            vBankName = ""
        End If



        If (Information.IsNothing(vHeadOffice)) Or (vHeadOffice.Equals(0)) Or (bDefaultAll) Then
            vHeadOffice = 0
        End If



        If (Information.IsNothing(vBankAddress1)) Or (String.IsNullOrEmpty(vBankAddress1)) Or (bDefaultAll) Then
            vBankAddress1 = ""
        End If



        If (Information.IsNothing(vBankAddress2)) Or (String.IsNullOrEmpty(vBankAddress2)) Or (bDefaultAll) Then
            vBankAddress2 = ""
        End If



        If (Information.IsNothing(vBankAddress3)) Or (String.IsNullOrEmpty(vBankAddress3)) Or (bDefaultAll) Then
            vBankAddress3 = ""
        End If



        If (Information.IsNothing(vBankAddress4)) Or (String.IsNullOrEmpty(vBankAddress4)) Or (bDefaultAll) Then
            vBankAddress4 = ""
        End If



        If (Information.IsNothing(vBankPostalCode)) Or (String.IsNullOrEmpty(vBankPostalCode)) Or (bDefaultAll) Then
            vBankPostalCode = ""
        End If



        If (Information.IsNothing(vBankCountry)) Or (vBankCountry.Equals(0)) Or (bDefaultAll) Then
            vBankCountry = 0
        End If



        If (Information.IsNothing(vBankPhoneAreaCode)) Or (String.IsNullOrEmpty(vBankPhoneAreaCode)) Or (bDefaultAll) Then
            vBankPhoneAreaCode = ""
        End If



        If (Information.IsNothing(vBankPhoneNumber)) Or (String.IsNullOrEmpty(vBankPhoneNumber)) Or (bDefaultAll) Then
            vBankPhoneNumber = ""
        End If



        If (Information.IsNothing(vBankPhoneExtension)) Or (String.IsNullOrEmpty(vBankPhoneExtension)) Or (bDefaultAll) Then
            vBankPhoneExtension = ""
        End If



        If (Information.IsNothing(vBankFaxAreaCode)) Or (String.IsNullOrEmpty(vBankFaxAreaCode)) Or (bDefaultAll) Then
            vBankFaxAreaCode = ""
        End If



        If (Information.IsNothing(vBankFaxNumber)) Or (String.IsNullOrEmpty(vBankFaxNumber)) Or (bDefaultAll) Then
            vBankFaxNumber = ""
        End If



        If (Information.IsNothing(vBankFaxExtension)) Or (String.IsNullOrEmpty(vBankFaxExtension)) Or (bDefaultAll) Then
            vBankFaxExtension = ""
        End If



        If (Information.IsNothing(vComments)) Or (String.IsNullOrEmpty(vComments)) Or (bDefaultAll) Then
            vComments = ""
        End If



        If (Information.IsNothing(vBankAccountType)) Or (vBankAccountType.Equals(0)) Or (bDefaultAll) Then
            vBankAccountType = 0
        End If
        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Bank.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vBankId As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vBranchCode As Object = Nothing, Optional ByRef vBankName As Object = Nothing, Optional ByRef vHeadOffice As Object = Nothing, Optional ByRef vBankAddress1 As Object = Nothing, Optional ByRef vBankAddress2 As Object = Nothing, Optional ByRef vBankAddress3 As Object = Nothing, Optional ByRef vBankAddress4 As Object = Nothing, Optional ByRef vBankPostalCode As Object = Nothing, Optional ByRef vBankCountry As Object = Nothing, Optional ByRef vBankPhoneAreaCode As Object = Nothing, Optional ByRef vBankPhoneNumber As Object = Nothing, Optional ByRef vBankPhoneExtension As Object = Nothing, Optional ByRef vBankFaxAreaCode As Object = Nothing, Optional ByRef vBankFaxNumber As Object = Nothing, Optional ByRef vBankFaxExtension As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vBankAccountType As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Information.IsNothing(vBankId)) Or (Object.Equals(vBankId, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vCode)) Or (Object.Equals(vCode, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vBranchCode)) Or (Object.Equals(vBranchCode, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vBankName)) Or (Object.Equals(vBankName, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vHeadOffice)) Or (Object.Equals(vHeadOffice, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vBankAddress1)) Or (Object.Equals(vBankAddress1, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vBankAddress2)) Or (Object.Equals(vBankAddress2, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vBankAddress3)) Or (Object.Equals(vBankAddress3, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vBankAddress4)) Or (Object.Equals(vBankAddress4, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vBankPostalCode)) Or (Object.Equals(vBankPostalCode, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vBankCountry)) Or (Object.Equals(vBankCountry, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vBankPhoneAreaCode)) Or (Object.Equals(vBankPhoneAreaCode, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vBankPhoneNumber)) Or (Object.Equals(vBankPhoneNumber, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vBankPhoneExtension)) Or (Object.Equals(vBankPhoneExtension, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vBankFaxAreaCode)) Or (Object.Equals(vBankFaxAreaCode, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vBankFaxNumber)) Or (Object.Equals(vBankFaxNumber, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vBankFaxExtension)) Or (Object.Equals(vBankFaxExtension, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vComments)) Or (Object.Equals(vComments, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        'Checking an optional parameter for IsMissing with (or IsEmpty) in business layer
        ' is quite suprising. If this parmeter not set in any of the calling interface always
        ' return PMMandatoryMissing


        If Not Information.IsNothing(vBankAccountType) Then

            If Object.Equals(vBankAccountType, Nothing) Then
                result = gPMConstants.PMEReturnCode.PMMandatoryMissing
            End If
        End If

        '    If (IsMissing(vBankAccountType) = True) _
        ''    Or (IsEmpty(vBankAccountType) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the Bank for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vBankId As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vBranchCode As Object = Nothing, Optional ByRef vBankName As Object = Nothing, Optional ByRef vHeadOffice As Object = Nothing, Optional ByRef vBankAddress1 As Object = Nothing, Optional ByRef vBankAddress2 As Object = Nothing, Optional ByRef vBankAddress3 As Object = Nothing, Optional ByRef vBankAddress4 As Object = Nothing, Optional ByRef vBankPostalCode As Object = Nothing, Optional ByRef vBankCountry As Object = Nothing, Optional ByRef vBankPhoneAreaCode As Object = Nothing, Optional ByRef vBankPhoneNumber As Object = Nothing, Optional ByRef vBankPhoneExtension As Object = Nothing, Optional ByRef vBankFaxAreaCode As Object = Nothing, Optional ByRef vBankFaxNumber As Object = Nothing, Optional ByRef vBankFaxExtension As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vBankAccountType As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vBankId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vHeadOffice), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vBankCountry), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp4 As Double
        If Not Double.TryParse(CStr(vBankAccountType), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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



            ' Error.
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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PRIVATE Methods (End)

    ' ***************************************************************** '
    ' Name: GetBankId
    '
    ' Description: Get Bank ID for a given reference (i.e. ShortCode)
    '
    ' History : Created by MKR on 27/10/2004
    ' ***************************************************************** '
    Public Function GetBankId(Optional ByRef vBankRef As Object = Nothing, Optional ByRef vBankId As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            If (Not Information.IsNothing(vBankRef)) And (Not Object.Equals(vBankRef, Nothing)) Then

                m_oDatabase.Parameters.Clear()

                ' Add the ID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Code", vValue:=CStr(vBankRef), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Get form DB
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBankIdSQL, sSQLName:=ACGetBankIdName, bStoredProcedure:=ACGetBankIdStored, lNumberRecords:=1, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'return the data
                If Not Information.IsArray(vResultArray) Then
                    vBankId = 0
                Else

                    vBankId = CInt(vResultArray(0, 0))
                End If

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetBankId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBankId", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetBranchBaseCountry
    '
    ' Description:  gets base country for branch
    '
    ' History: DM 09082006 created
    ' ***************************************************************** '
    Public Function GetBranchBaseCountry(ByVal v_lSourceID As Integer, ByRef r_iCountryID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT country_id FROM source "
            sSQL = sSQL & "WHERE source_id = " & CStr(v_lSourceID)

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetBranchBaseCountry", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                r_iCountryID = .Records.Fields("country_id")

            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBranchBaseCountry failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranchBaseCountry", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteBankAccount
    '
    ' Description:  Deletes bank account.
    '
    ' History: 18/06/2008 Gautam Poddar - Created.
    ' ***************************************************************** '
    Public Function DeleteBankAccount(ByVal v_lBankAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the BankAccountID INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_id", vValue:=CStr(v_lBankAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteBankAccountSQL, sSQLName:=ACDeleteBankAccountName, bStoredProcedure:=ACDeleteBankAccountStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'PN 47135,49017
            'If record wasn't deleted, error
            'If (lRecordsAffected& > 0) Then
            ' Deleted, No action required
            'Else
            '    DeleteBankAccount = PMFalse
            '    Exit Function
            'End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Bank Account Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteBankAccount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
        ' Error.
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

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
