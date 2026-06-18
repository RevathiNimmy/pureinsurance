Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'Developer Guide No 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 04/10/1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Transmatch.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 06/02/2004
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

    ' Collection of TransMatches (Private)
    Private m_oTransMatches As bACTTransmatch.Transmatches

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

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

    ' Primary Keys to work with
    ' Source ID

    ' NavigatorV3 variables
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' PRIVATE Data Members (End)


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
                Case Is > m_oTransMatches.Count()
                    m_lCurrentRecord = m_oTransMatches.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oTransMatches.Count()

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
    Public ReadOnly Property Details() As Transmatches
        Get
            Return m_oTransMatches
        End Get
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummaryFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


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
            'EK 01/10/99 Use Party Services
            '    Set m_oDatabase = GetOrionDatabase(m_lReturn, m_bCloseDatabase, vDatabase)



            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)


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

            ' Create TransMatches Collection
            m_oTransMatches = New bACTTransmatch.Transmatches()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                m_oTransMatches = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
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


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single Transmatch directly into the database.
    '        Note: The Transmatch will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vTransmatchID As Integer = 0, Optional ByRef vAllocationdetailID As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vMatchID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBaseMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchXrate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oTransmatch As bACTTransmatch.Transmatch

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Transmatch
            oTransmatch = New bACTTransmatch.Transmatch()

            ' Populate Transmatch Attributes







            'developer guide no.98
            'm_lReturn = SetProperties(oTransmatch, gPMConstants.PMEComponentAction.PMAdd, vTransmatchID:=vTransmatchID, vAllocationdetailID:=CInt(vAllocationdetailID), vTransdetailID:=CInt(vTransdetailID), vMatchID:=CInt(vMatchID), vCurrencyID:=CInt(vCurrencyID), vBaseMatchAmount:=CDec(vBaseMatchAmount), vCurrencyMatchAmount:=CDec(vCurrencyMatchAmount), vCurrencyMatchXrate:=CDbl(vCurrencyMatchXrate))
            m_lReturn = SetProperties(oTransmatch, gPMConstants.PMEComponentAction.PMAdd,
                 vTransmatchID:=vTransmatchID,
                 vAllocationdetailID:=vAllocationdetailID,
                 vTransdetailID:=vTransdetailID,
                 vMatchID:=vMatchID,
                 vCurrencyID:=vCurrencyID,
                 vBaseMatchAmount:=vBaseMatchAmount,
                 vCurrencyMatchAmount:=vCurrencyMatchAmount,
                 vCurrencyMatchXrate:=vCurrencyMatchXrate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Transmatch to the Database
            m_lReturn = AddItem(oTransmatch)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the Transmatch Added

            If Not Informations.IsNothing(vTransmatchID) Then
                vTransmatchID = oTransmatch.TransmatchID
            End If

            ' {* USER DEFINED CODE (End) *}

            oTransmatch = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single Transmatch directly from the database.
    '        Note: The Transmatch will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vTransmatchID As Object = Nothing, Optional ByRef vAllocationdetailID As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vMatchID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBaseMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchXrate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oTransmatch As bACTTransmatch.Transmatch

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Transmatch
            oTransmatch = New bACTTransmatch.Transmatch()

            ' Populate Transmatch Attributes

            m_lReturn = SetProperties(oTransmatch, gPMConstants.PMEComponentAction.PMDelete, vTransmatchID:=CInt(vTransmatchID), vAllocationdetailID:=CInt(vAllocationdetailID), vTransdetailID:=CInt(vTransdetailID), vMatchID:=CInt(vMatchID), vCurrencyID:=CInt(vCurrencyID), vBaseMatchAmount:=CDec(vBaseMatchAmount), vCurrencyMatchAmount:=CDec(vCurrencyMatchAmount), vCurrencyMatchXrate:=CDbl(vCurrencyMatchXrate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Transmatch to the Database
            m_lReturn = DeleteItem(oTransmatch)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oTransmatch = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the Transmatch.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vTransmatchID As Object = Nothing, Optional ByRef vAllocationdetailID As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vMatchID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBaseMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchXrate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults








            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vTransmatchID:=CByte(vTransmatchID), vAllocationdetailID:=CByte(vAllocationdetailID), vTransdetailID:=CByte(vTransdetailID), vMatchID:=CByte(vMatchID), vCurrencyID:=CByte(vCurrencyID), vBaseMatchAmount:=CByte(vBaseMatchAmount), vCurrencyMatchAmount:=CByte(vCurrencyMatchAmount), vCurrencyMatchXrate:=CByte(vCurrencyMatchXrate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            'eck PN7313 replace integer with long

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required TransMatches and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vTransmatchID As Object = Nothing, Optional ByRef vLockMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oTransmatch As bACTTransmatch.Transmatch

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oTransMatches.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Do we have a key

            If Not Informations.IsNothing(vTransmatchID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vTransmatchID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vTransmatchID =" & CStr(vTransmatchID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If
                'eck PN7313 replace integer with long
                ' Add the TransmatchID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Transmatch_id", vValue:=CStr(vTransmatchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

                    ' Create New Transmatch
                    oTransmatch = New bACTTransmatch.Transmatch()

                    m_lReturn = SetPropertiesFromDB(oTransmatch:=oTransmatch, lRecordNumber:=lSub)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add Transmatch to collection
                    If (m_oTransMatches.Count = 0) Then
                        m_oTransMatches.Add(Nothing)
                    End If
                    m_lReturn = m_oTransMatches.Add(oNewTransmatch:=oTransmatch)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oTransmatch = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required TransMatches and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vTransmatchID As Object = Nothing, Optional ByRef vAllocationdetailID As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vMatchID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBaseMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchXrate As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oTransmatch As bACTTransmatch.Transmatch
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oTransMatches.Count() Then
                ' Increment current record pointer
                ' m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oTransmatch = m_oTransMatches.Item(m_lCurrentRecord)

            ' Get the Transmatch Property Values

            m_lReturn = GetProperties(oTransmatch, iStatus, vTransmatchID:=CInt(vTransmatchID), vAllocationdetailID:=CInt(vAllocationdetailID), vTransdetailID:=CInt(vTransdetailID), vMatchID:=CInt(vMatchID), vCurrencyID:=CInt(vCurrencyID), vBaseMatchAmount:=CDec(vBaseMatchAmount), vCurrencyMatchAmount:=CDec(vCurrencyMatchAmount), vCurrencyMatchXrate:=CDbl(vCurrencyMatchXrate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oTransmatch = Nothing


            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied Transmatch into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vTransmatchID As Object = Nothing, Optional ByRef vAllocationdetailID As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vMatchID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBaseMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchXrate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oTransmatch As bACTTransmatch.Transmatch

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oTransMatches.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new Transmatch
            oTransmatch = New bACTTransmatch.Transmatch()

            ' Populate Transmatch Attributes

            m_lReturn = SetProperties(oTransmatch, gPMConstants.PMEComponentAction.PMAdd, vTransmatchID:=CInt(vTransmatchID), vAllocationdetailID:=CInt(vAllocationdetailID), vTransdetailID:=CInt(vTransdetailID), vMatchID:=CInt(vMatchID), vCurrencyID:=CInt(vCurrencyID), vBaseMatchAmount:=CDec(vBaseMatchAmount), vCurrencyMatchAmount:=CDec(vCurrencyMatchAmount), vCurrencyMatchXrate:=CDbl(vCurrencyMatchXrate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oTransmatch = Nothing
                Return result
            End If

            ' Add Transmatch to collection
            If (m_oTransMatches.Count = 0) Then
                m_oTransMatches.Add(Nothing)
            End If
            m_lReturn = m_oTransMatches.Add(oNewTransmatch:=oTransmatch)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oTransmatch = Nothing
                Return result
            End If

            oTransmatch = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the Transmatch
    '              specified and updates the Transmatch with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vTransmatchID As Object = Nothing, Optional ByRef vAllocationdetailID As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vMatchID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBaseMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchXrate As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oTransmatch As bACTTransmatch.Transmatch
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oTransMatches.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oTransmatch = m_oTransMatches.Item(lRow)

            ' Check the Status of the Transmatch

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oTransmatch.DatabaseStatus
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

            ' Update Transmatch Attributes

            m_lReturn = SetProperties(oTransmatch, iStatus, vTransmatchID:=CInt(vTransmatchID), vAllocationdetailID:=CInt(vAllocationdetailID), vTransdetailID:=CInt(vTransdetailID), vMatchID:=CInt(vMatchID), vCurrencyID:=CInt(vCurrencyID), vBaseMatchAmount:=CDec(vBaseMatchAmount), vCurrencyMatchAmount:=CDec(vCurrencyMatchAmount), vCurrencyMatchXrate:=CDbl(vCurrencyMatchXrate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oTransmatch = Nothing
                Return result
            End If

            ' Release reference to Transmatch
            oTransmatch = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified Transmatch can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oTransmatch As bACTTransmatch.Transmatch

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oTransMatches.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oTransmatch = m_oTransMatches.Item(lRow)

            ' Check the Status of the Transmatch

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oTransmatch.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oTransmatch.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oTransmatch.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to Transmatch
            oTransmatch = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            For lSub As Integer = 1 To m_oTransMatches.Count() - 1
                Select Case m_oTransMatches.Item(lSub).DatabaseStatus
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oTransmatch As bACTTransmatch.Transmatch
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oTransMatches.Count()
                oTransmatch = m_oTransMatches.Item(lSub)


                Select Case oTransmatch.DatabaseStatus
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
                        m_lReturn = AddItem(oTransmatch)
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
                        m_lReturn = UpdateItem(oTransmatch)
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
                        m_lReturn = DeleteItem(oTransmatch)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oTransmatch = Nothing

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
                    Do While lSub <= m_oTransMatches.Count()

                        ' With the item
                        With m_oTransMatches.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oTransMatches.Delete(lSub)

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function AddItem(ByRef oTransmatch As bACTTransmatch.Transmatch) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = AddInputParam(oTransmatch:=oTransmatch)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add TransmatchID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Transmatch_id", vValue:=CStr(oTransmatch.TransmatchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oTransmatch.TransmatchID = m_oDatabase.Parameters.Item("Transmatch_id").Value

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
    Private Function UpdateItem(ByRef oTransmatch As bACTTransmatch.Transmatch) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = AddInputParam(oTransmatch:=oTransmatch)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add TransmatchID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Transmatch_id", vValue:=CStr(oTransmatch.TransmatchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' BB Not required by Orion
        ' Add Timestamp as an INPUT param for an update
        'm_lReturn& = m_oDatabase.Parameters.Add( _
        'sName:="timestamp", _
        'vValue:=oTransmatch.Timestamp, _
        'iDirection:=PMParamInput, _
        'iDataType:=PMBinary)

        'If (m_lReturn& <> PMTrue) Then
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
    Private Function DeleteItem(ByRef oTransmatch As bACTTransmatch.Transmatch) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the TransmatchID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Transmatch_id", vValue:=CStr(oTransmatch.TransmatchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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
    ' Description: Sets the supplied Transmatch properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oTransmatch As bACTTransmatch.Transmatch, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'Developer Guide No 21
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oTransmatch

            .TransmatchID = oFields("transmatch_id")

            If Convert.IsDBNull(oFields("allocationdetail_id")) Or Informations.IsNothing(oFields("allocationdetail_id")) Then
                .AllocationdetailID = 0
            Else
                .AllocationdetailID = oFields("allocationdetail_id")
            End If

            If Convert.IsDBNull(oFields("transdetail_id")) Or Informations.IsNothing(oFields("transdetail_id")) Then
                .TransdetailID = 0
            Else
                .TransdetailID = oFields("transdetail_id")
            End If

            If Convert.IsDBNull(oFields("match_id")) Or Informations.IsNothing(oFields("match_id")) Then
                .MatchID = 0
            Else
                .MatchID = oFields("match_id")
            End If

            If Convert.IsDBNull(oFields("currency_id")) Or Informations.IsNothing(oFields("currency_id")) Then
                .CurrencyID = 0
            Else
                .CurrencyID = oFields("currency_id")
            End If

            If Convert.IsDBNull(oFields("base_match_amount")) Or Informations.IsNothing(oFields("base_match_amount")) Then
                .BaseMatchAmount = 0
            Else
                .BaseMatchAmount = oFields("base_match_amount")
            End If

            If Convert.IsDBNull(oFields("currency_match_amount")) Or Informations.IsNothing(oFields("currency_match_amount")) Then
                .CurrencyMatchAmount = 0
            Else
                .CurrencyMatchAmount = oFields("currency_match_amount")
            End If

            If Convert.IsDBNull(oFields("currency_match_xrate")) Or Informations.IsNothing(oFields("currency_match_xrate")) Then
                .CurrencyMatchXrate = 0
            Else
                .CurrencyMatchXrate = oFields("currency_match_xrate")
            End If

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied Transmatch property values.
    '
    ' ***************************************************************** '
    'developer guide no.33
    Private Function SetProperties(ByRef oTransmatch As bACTTransmatch.Transmatch, ByRef iStatus As Integer, Optional ByRef vTransmatchID As Object = Nothing, Optional ByRef vAllocationdetailID As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vMatchID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBaseMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchXrate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CheckMandatory(vTransmatchID:=vTransmatchID, vAllocationdetailID:=vAllocationdetailID, vTransdetailID:=vTransdetailID, vMatchID:=vMatchID, vCurrencyID:=vCurrencyID, vBaseMatchAmount:=vBaseMatchAmount, vCurrencyMatchAmount:=vCurrencyMatchAmount, vCurrencyMatchXrate:=vCurrencyMatchXrate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Default Any Missing Parameters
            m_lReturn = DefaultParameters(bDefaultAll:=False, vTransmatchID:=vTransmatchID, vAllocationdetailID:=vAllocationdetailID, vTransdetailID:=vTransdetailID, vMatchID:=vMatchID, vCurrencyID:=vCurrencyID, vBaseMatchAmount:=vBaseMatchAmount, vCurrencyMatchAmount:=vCurrencyMatchAmount, vCurrencyMatchXrate:=vCurrencyMatchXrate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        m_lReturn = Validate(vTransmatchID:=vTransmatchID, vAllocationdetailID:=vAllocationdetailID, vTransdetailID:=vTransdetailID, vMatchID:=vMatchID, vCurrencyID:=vCurrencyID, vBaseMatchAmount:=vBaseMatchAmount, vCurrencyMatchAmount:=vCurrencyMatchAmount, vCurrencyMatchXrate:=vCurrencyMatchXrate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oTransmatch


            If Not Informations.IsNothing(vTransmatchID) Then
                If .TransmatchID <> vTransmatchID Then
                    .TransmatchID = vTransmatchID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAllocationdetailID) Then
                If .AllocationdetailID <> vAllocationdetailID Then
                    .AllocationdetailID = vAllocationdetailID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vTransdetailID) Then
                If .TransdetailID <> vTransdetailID Then
                    .TransdetailID = vTransdetailID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vMatchID) Then
                If .MatchID <> vMatchID Then
                    .MatchID = vMatchID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCurrencyID) Then
                If .CurrencyID <> vCurrencyID Then
                    .CurrencyID = vCurrencyID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBaseMatchAmount) Then
                If .BaseMatchAmount <> vBaseMatchAmount Then
                    .BaseMatchAmount = vBaseMatchAmount
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCurrencyMatchAmount) Then
                If .CurrencyMatchAmount <> vCurrencyMatchAmount Then
                    .CurrencyMatchAmount = vCurrencyMatchAmount
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCurrencyMatchXrate) Then
                If .CurrencyMatchXrate <> vCurrencyMatchXrate Then
                    .CurrencyMatchXrate = vCurrencyMatchXrate
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
    ' Name: GetProperties (Private)
    '
    ' Description: Returns the supplied Transmatch property values.
    '
    ' ***************************************************************** '
    Private Function GetProperties(ByRef oTransmatch As bACTTransmatch.Transmatch, ByRef iStatus As Integer, Optional ByRef vTransmatchID As Integer = 0, Optional ByRef vAllocationdetailID As Integer = 0, Optional ByRef vTransdetailID As Integer = 0, Optional ByRef vMatchID As Integer = 0, Optional ByRef vCurrencyID As Integer = 0, Optional ByRef vBaseMatchAmount As Decimal = 0, Optional ByRef vCurrencyMatchAmount As Decimal = 0, Optional ByRef vCurrencyMatchXrate As Double = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oTransmatch


            If Not Informations.IsNothing(vTransmatchID) Then
                vTransmatchID = .TransmatchID
            End If


            If Not Informations.IsNothing(vAllocationdetailID) Then
                vAllocationdetailID = .AllocationdetailID
            End If


            If Not Informations.IsNothing(vTransdetailID) Then
                vTransdetailID = .TransdetailID
            End If


            If Not Informations.IsNothing(vMatchID) Then
                vMatchID = .MatchID
            End If


            If Not Informations.IsNothing(vCurrencyID) Then
                vCurrencyID = .CurrencyID
            End If


            If Not Informations.IsNothing(vBaseMatchAmount) Then
                vBaseMatchAmount = .BaseMatchAmount
            End If


            If Not Informations.IsNothing(vCurrencyMatchAmount) Then
                vCurrencyMatchAmount = .CurrencyMatchAmount
            End If


            If Not Informations.IsNothing(vCurrencyMatchXrate) Then
                vCurrencyMatchXrate = .CurrencyMatchXrate
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
    Private Function AddInputParam(ByRef oTransmatch As bACTTransmatch.Transmatch) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase


            If oTransmatch.AllocationdetailID < 1 Then

                'Developer Guide no 85
                m_lReturn = .Parameters.Add(sName:="allocationdetail_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="allocationdetail_id", vValue:=CStr(oTransmatch.AllocationdetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransmatch.TransdetailID < 1 Then

                'Developer Guide no 85
                m_lReturn = .Parameters.Add(sName:="transdetail_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="transdetail_id", vValue:=CStr(oTransmatch.TransdetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransmatch.MatchID < 1 Then

                'Developer Guide no 85
                m_lReturn = .Parameters.Add(sName:="match_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="match_id", vValue:=CStr(oTransmatch.MatchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransmatch.CurrencyID < 1 Then

                'Developer Guide no 85
                m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=CStr(oTransmatch.CurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="base_match_amount", vValue:=CStr(oTransmatch.BaseMatchAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="currency_match_amount", vValue:=CStr(oTransmatch.CurrencyMatchAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="currency_match_xrate", vValue:=CStr(oTransmatch.CurrencyMatchXrate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Transmatch.
    '
    ' ***************************************************************** '
    'developer guide no.33
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vTransmatchID As Object = Nothing, Optional ByRef vAllocationdetailID As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vMatchID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBaseMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchXrate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vTransmatchID)) Or (vTransmatchID.Equals(0)) Or (bDefaultAll) Then
            vTransmatchID = 0
        End If



        If (Informations.IsNothing(vAllocationdetailID)) Or (vAllocationdetailID.Equals(0)) Or (bDefaultAll) Then
            vAllocationdetailID = 0
        End If



        If (Informations.IsNothing(vTransdetailID)) Or (vTransdetailID.Equals(0)) Or (bDefaultAll) Then
            vTransdetailID = 0
        End If



        If (Informations.IsNothing(vMatchID)) Or (vMatchID.Equals(0)) Or (bDefaultAll) Then
            vMatchID = 0
        End If



        If (Informations.IsNothing(vCurrencyID)) Or (vCurrencyID.Equals(0)) Or (bDefaultAll) Then
            vCurrencyID = 0
        End If



        If (Informations.IsNothing(vBaseMatchAmount)) Or (vBaseMatchAmount.Equals(0)) Or (bDefaultAll) Then
            vBaseMatchAmount = 0
        End If



        If (Informations.IsNothing(vCurrencyMatchAmount)) Or (vCurrencyMatchAmount.Equals(0)) Or (bDefaultAll) Then
            vCurrencyMatchAmount = 0
        End If



        If (Informations.IsNothing(vCurrencyMatchXrate)) Or (vCurrencyMatchXrate.Equals(0)) Or (bDefaultAll) Then
            vCurrencyMatchXrate = 0
        End If


        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Transmatch.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vTransmatchID As Object = Nothing, Optional ByRef vAllocationdetailID As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vMatchID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBaseMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchXrate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vTransmatchID)) Or (Object.Equals(vTransmatchID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vAllocationdetailID)) Or (Object.Equals(vAllocationdetailID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vTransdetailID)) Or (Object.Equals(vTransdetailID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vMatchID)) Or (Object.Equals(vMatchID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCurrencyID)) Or (Object.Equals(vCurrencyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vBaseMatchAmount)) Or (Object.Equals(vBaseMatchAmount, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCurrencyMatchAmount)) Or (Object.Equals(vCurrencyMatchAmount, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCurrencyMatchXrate)) Or (Object.Equals(vCurrencyMatchXrate, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the Transmatch for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vTransmatchID As Object = Nothing, Optional ByRef vAllocationdetailID As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vMatchID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vBaseMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchAmount As Object = Nothing, Optional ByRef vCurrencyMatchXrate As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vTransmatchID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        Dim dbNumericTemp2 As Double
        If (Not Double.TryParse(CStr(vAllocationdetailID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) And (Not (Convert.IsDBNull(vAllocationdetailID) Or Informations.IsNothing(vAllocationdetailID))) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vTransdetailID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp4 As Double
        If Not Double.TryParse(CStr(vMatchID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp5 As Double
        If Not Double.TryParse(CStr(vCurrencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp6 As Double
        If Not Double.TryParse(CStr(vBaseMatchAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp7 As Double
        If Not Double.TryParse(CStr(vCurrencyMatchAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp8 As Double
        If Not Double.TryParse(CStr(vCurrencyMatchXrate), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp8) Then
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
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
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
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