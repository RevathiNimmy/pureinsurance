Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 07/05/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRPartyConviction.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' ************************************************
    ' Added to replace global variables 18/12/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Collection of SIRPartyConvictions (Private)
    Private m_oSIRPartyConvictions As bSIRPartyConviction.SIRPartyConvictions

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private lPMAuthorityLevel As Integer

    ' Primary Keys to work with
    Private m_lPartyCnt As Integer
    Private m_lPartyConvictionID As Integer

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property
    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            Value = Value
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
                Case Is > m_oSIRPartyConvictions.Count()
                    m_lCurrentRecord = m_oSIRPartyConvictions.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select
        End Set
    End Property
    Public ReadOnly Property RecordCount() As Integer
        Get
            ' Return Number in Collection
            Return m_oSIRPartyConvictions.Count()
        End Get
    End Property
    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    Public Property PartyConvictionID() As Integer
        Get
            Return m_lPartyConvictionID
        End Get
        Set(ByVal Value As Integer)
            m_lPartyConvictionID = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create SIRPartyConvictions Collection
            m_oSIRPartyConvictions = New bSIRPartyConviction.SIRPartyConvictions()

            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUserName:=sUserName, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

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
            Me.disposedValue = True
            If disposing Then
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                End If
                m_oLookup = Nothing
                m_oSIRPartyConvictions = Nothing
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
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIRPartyConviction.
    '
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray As Object, ByRef iLanguageID As Integer, ByRef vResultArray As Object) As Integer


        Dim result As Integer = 0
        Return result




        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single SIRPartyConviction directly into the database.
    '        Note: The SIRPartyConviction will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyConvictionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vConvictionDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vFineAmt As Object = Nothing, Optional ByRef vSentenceCode As Object = Nothing, Optional ByRef vSentenceDescription As Object = Nothing, Optional ByRef vSentenceDuration As Object = Nothing, Optional ByRef vSentenceDurationQualifier As Object = Nothing, Optional ByRef vSentenceEffectiveDate As Object = Nothing, Optional ByRef vStatusCode As Object = Nothing, Optional ByRef vAlcoholLevel As Object = Nothing, Optional ByRef vAlcoholMeasurementMethod As Object = Nothing, Optional ByRef vDrivingLicencePenaltyPoints As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyConviction As bSIRPartyConviction.SIRPartyConviction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRPartyConviction
            oSIRPartyConviction = New bSIRPartyConviction.SIRPartyConviction()
            m_lReturn = CType(oSIRPartyConviction.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Populate SIRPartyConviction Attributes


            'Developer Guie no 98
            'm_lReturn = CType(oSIRPartyConviction.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=CInt(vPartyCnt), vPartyConvictionID:=CInt(vPartyConvictionID), vCode:=vCode, vConvictionDate:=vConvictionDate, vDescription:=vDescription, vFineAmt:=vFineAmt, vSentenceCode:=vSentenceCode, vSentenceDescription:=vSentenceDescription, vSentenceDuration:=vSentenceDuration, vSentenceDurationQualifier:=vSentenceDurationQualifier, vSentenceEffectiveDate:=vSentenceEffectiveDate, vStatusCode:=vStatusCode, vAlcoholLevel:=vAlcoholLevel, vAlcoholMeasurementMethod:=vAlcoholMeasurementMethod, vDrivingLicencePenaltyPoints:=vDrivingLicencePenaltyPoints), gPMConstants.PMEReturnCode)
            m_lReturn = CType(oSIRPartyConviction.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=vPartyCnt, vPartyConvictionID:=vPartyConvictionID, vCode:=vCode, vConvictionDate:=vConvictionDate, vDescription:=vDescription, vFineAmt:=vFineAmt, vSentenceCode:=vSentenceCode, vSentenceDescription:=vSentenceDescription, vSentenceDuration:=vSentenceDuration, vSentenceDurationQualifier:=vSentenceDurationQualifier, vSentenceEffectiveDate:=vSentenceEffectiveDate, vStatusCode:=vStatusCode, vAlcoholLevel:=vAlcoholLevel, vAlcoholMeasurementMethod:=vAlcoholMeasurementMethod, vDrivingLicencePenaltyPoints:=vDrivingLicencePenaltyPoints), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyConviction = Nothing
                Return result
            End If

            ' Add the SIRPartyConviction to the Database
            m_lReturn = CType(oSIRPartyConviction.AddItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyConviction = Nothing
                Return result
            End If

            ' Retain the Primary Key of the SIRPartyConviction Added
            With oSIRPartyConviction
                PartyCnt = .PartyCnt
                PartyConvictionID = .PartyConvictionID
            End With

            oSIRPartyConviction = Nothing

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
    ' Description: Deletes a single SIRPartyConviction directly from the database.
    '        Note: The SIRPartyConviction will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyConvictionID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyConviction As bSIRPartyConviction.SIRPartyConviction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRPartyConviction
            oSIRPartyConviction = New bSIRPartyConviction.SIRPartyConviction()
            m_lReturn = CType(oSIRPartyConviction.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Set SIRPartyConviction Primary Key


            'Developer Guie No 98
            'm_lReturn = CType(oSIRPartyConviction.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vPartyCnt:=CInt(vPartyCnt), vPartyConvictionID:=CInt(vPartyConvictionID)), gPMConstants.PMEReturnCode)
            m_lReturn = CType(oSIRPartyConviction.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vPartyCnt:=vPartyCnt, vPartyConvictionID:=vPartyConvictionID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyConviction = Nothing
                Return result
            End If

            ' Delete the SIRPartyConviction from the Database
            m_lReturn = CType(oSIRPartyConviction.DeleteItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyConviction = Nothing
                Return result
            End If

            oSIRPartyConviction = Nothing

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
        Dim lSub As Integer = 0
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
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required SIRPartyConvictions and populate the Collection
    '
    ' ***************************************************************** '
    'Developer Guie no 101
    'Public Function GetDetails(Optional ByRef vLockMode As gPMConstants.PMELockMode = 0, Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vPartyConvictionID As Integer = 0) As Integer
    Public Function GetDetails(Optional ByRef vLockMode As gPMConstants.PMELockMode = 0, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyConvictionID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oFields As ADODB.Fields
        Dim oSIRPartyConviction As bSIRPartyConviction.SIRPartyConviction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oSIRPartyConvictions.Clear()

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

            If (Not Information.IsNothing(vPartyCnt)) And (Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vPartyCnt=" & vPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If
            Dim dbNumericTemp3 As Double

            If (Not Information.IsNothing(vPartyConvictionID)) And (Not Double.TryParse(CStr(vPartyConvictionID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vPartyConvictionID=" & vPartyConvictionID, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            If (Not Information.IsNothing(vPartyCnt)) And (Not Information.IsNothing(vPartyConvictionID)) Then

                ' Create New SIRPartyConviction
                oSIRPartyConviction = New bSIRPartyConviction.SIRPartyConviction()
                m_lReturn = CType(oSIRPartyConviction.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


                ' Set component primary keys
                With oSIRPartyConviction
                    .PartyCnt = vPartyCnt
                    .PartyConvictionID = vPartyConvictionID

                    m_lReturn = CType(.SelectItem(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                ' Add SIRPartyConviction to collection
                m_lReturn = CType(m_oSIRPartyConvictions.Add(oNewSIRPartyConviction:=oSIRPartyConviction), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oSIRPartyConviction = Nothing

            Else

                ' No Key, Get All Records

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = m_oDatabase.Records.Count()

                ' Do we have any records ?
                If lRecordCount < 1 Then
                    ' No Records, return PMFalse
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Yes, load them into the collection
                For lSub As Integer = 1 To lRecordCount

                    ' Create New
                    oSIRPartyConviction = New bSIRPartyConviction.SIRPartyConviction()
                    'Developer Guie no 9
                    'm_lReturn = CType(CType(oSIRPartyConviction, SSP.S4I.Interfaces.IBusiness).Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName), gPMConstants.PMEReturnCode)
                    m_lReturn = oSIRPartyConviction.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

                    ' Set oFields to refer to one Record
                    oFields = m_oDatabase.Records.Item(lSub).Fields()

                    ' Set component primary keys from current record
                    With oSIRPartyConviction
                        .PartyCnt = gPMFunctions.NullToLong(oFields("party_cnt"))
                        .PartyConvictionID = gPMFunctions.NullToLong(oFields("party_conviction_id"))

                        m_lReturn = CType(.SelectItem(), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End With

                    ' Add SIRPartyConviction to collection
                    m_lReturn = CType(m_oSIRPartyConvictions.Add(oNewSIRPartyConviction:=oSIRPartyConviction), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oSIRPartyConviction = Nothing
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
    ' Description: Gets the required SIRPartyConvictions and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyConvictionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vConvictionDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vFineAmt As Object = Nothing, Optional ByRef vSentenceCode As Object = Nothing, Optional ByRef vSentenceDescription As Object = Nothing, Optional ByRef vSentenceDuration As Object = Nothing, Optional ByRef vSentenceDurationQualifier As Object = Nothing, Optional ByRef vSentenceEffectiveDate As Object = Nothing, Optional ByRef vStatusCode As Object = Nothing, Optional ByRef vAlcoholLevel As Object = Nothing, Optional ByRef vAlcoholMeasurementMethod As Object = Nothing, Optional ByRef vDrivingLicencePenaltyPoints As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim lRecordCount As Integer = 0
        Dim oSIRPartyConviction As bSIRPartyConviction.SIRPartyConviction
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oSIRPartyConvictions.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oSIRPartyConviction = m_oSIRPartyConvictions.Item(m_lCurrentRecord)

            ' Get the SIRPartyConviction Property Values


            'Developer Guie no 98
            'm_lReturn = CType(oSIRPartyConviction.GetProperties(iStatus, vPartyCnt:=CInt(vPartyCnt), vPartyConvictionID:=CInt(vPartyConvictionID), vCode:=vCode, vConvictionDate:=vConvictionDate, vDescription:=vDescription, vFineAmt:=vFineAmt, vSentenceCode:=vSentenceCode, vSentenceDescription:=vSentenceDescription, vSentenceDuration:=vSentenceDuration, vSentenceDurationQualifier:=vSentenceDurationQualifier, vSentenceEffectiveDate:=vSentenceEffectiveDate, vStatusCode:=vStatusCode, vAlcoholLevel:=vAlcoholLevel, vAlcoholMeasurementMethod:=vAlcoholMeasurementMethod, vDrivingLicencePenaltyPoints:=vDrivingLicencePenaltyPoints), gPMConstants.PMEReturnCode)
            m_lReturn = CType(oSIRPartyConviction.GetProperties(iStatus, vPartyCnt:=vPartyCnt, vPartyConvictionID:=vPartyConvictionID, vCode:=vCode, vConvictionDate:=vConvictionDate, vDescription:=vDescription, vFineAmt:=vFineAmt, vSentenceCode:=vSentenceCode, vSentenceDescription:=vSentenceDescription, vSentenceDuration:=vSentenceDuration, vSentenceDurationQualifier:=vSentenceDurationQualifier, vSentenceEffectiveDate:=vSentenceEffectiveDate, vStatusCode:=vStatusCode, vAlcoholLevel:=vAlcoholLevel, vAlcoholMeasurementMethod:=vAlcoholMeasurementMethod, vDrivingLicencePenaltyPoints:=vDrivingLicencePenaltyPoints), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyConviction = Nothing

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
    ' Description: Adds the supplied SIRPartyConviction into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyConvictionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vConvictionDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vFineAmt As Object = Nothing, Optional ByRef vSentenceCode As Object = Nothing, Optional ByRef vSentenceDescription As Object = Nothing, Optional ByRef vSentenceDuration As Object = Nothing, Optional ByRef vSentenceDurationQualifier As Object = Nothing, Optional ByRef vSentenceEffectiveDate As Object = Nothing, Optional ByRef vStatusCode As Object = Nothing, Optional ByRef vAlcoholLevel As Object = Nothing, Optional ByRef vAlcoholMeasurementMethod As Object = Nothing, Optional ByRef vDrivingLicencePenaltyPoints As Object = Nothing, Optional vUniqueId As String = "", Optional vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim oSIRPartyConviction As bSIRPartyConviction.SIRPartyConviction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oSIRPartyConvictions.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new SIRPartyConviction
            oSIRPartyConviction = New bSIRPartyConviction.SIRPartyConviction()
            m_lReturn = CType(oSIRPartyConviction.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Populate SIRPartyConviction Attributes


            'developer guide no.98
            m_lReturn = CType(oSIRPartyConviction.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=vPartyCnt, vPartyConvictionID:=vPartyConvictionID, vCode:=vCode, vConvictionDate:=vConvictionDate, vDescription:=vDescription, vFineAmt:=vFineAmt, vSentenceCode:=vSentenceCode, vSentenceDescription:=vSentenceDescription, vSentenceDuration:=vSentenceDuration, vSentenceDurationQualifier:=vSentenceDurationQualifier, vSentenceEffectiveDate:=vSentenceEffectiveDate, vStatusCode:=vStatusCode, vAlcoholLevel:=vAlcoholLevel, vAlcoholMeasurementMethod:=vAlcoholMeasurementMethod, vDrivingLicencePenaltyPoints:=vDrivingLicencePenaltyPoints, vUniqueId:=vUniqueId, vScreenHierarchy:=vScreenHierarchy), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRPartyConviction = Nothing
                Return result
            End If

            ' Add SIRPartyConviction to collection
            m_lReturn = CType(m_oSIRPartyConvictions.Add(oNewSIRPartyConviction:=oSIRPartyConviction), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyConviction = Nothing
                Return result
            End If

            oSIRPartyConviction = Nothing

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
    ' Description: Validates that this action is valid on the SIRPartyConviction
    '              specified and updates the SIRPartyConviction with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyConvictionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vConvictionDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vFineAmt As Object = Nothing, Optional ByRef vSentenceCode As Object = Nothing, Optional ByRef vSentenceDescription As Object = Nothing, Optional ByRef vSentenceDuration As Object = Nothing, Optional ByRef vSentenceDurationQualifier As Object = Nothing, Optional ByRef vSentenceEffectiveDate As Object = Nothing, Optional ByRef vStatusCode As Object = Nothing, Optional ByRef vAlcoholLevel As Object = Nothing, Optional ByRef vAlcoholMeasurementMethod As Object = Nothing, Optional ByRef vDrivingLicencePenaltyPoints As Object = Nothing, Optional vUniqueId As String = "", Optional vScreenHierarchy As String = "") As Integer


        Dim result As Integer = 0
        Dim oSIRPartyConviction As bSIRPartyConviction.SIRPartyConviction
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRPartyConvictions.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oSIRPartyConviction = m_oSIRPartyConvictions.Item(lRow)

            ' Check the Status of the SIRPartyConviction

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oSIRPartyConviction.DatabaseStatus
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

            ' Update SIRPartyConviction Attributes


            m_lReturn = CType(oSIRPartyConviction.SetProperties(iStatus:=iStatus, vPartyCnt:=CInt(vPartyCnt), vPartyConvictionID:=CInt(vPartyConvictionID), vCode:=vCode, vConvictionDate:=vConvictionDate, vDescription:=vDescription, vFineAmt:=vFineAmt, vSentenceCode:=vSentenceCode, vSentenceDescription:=vSentenceDescription, vSentenceDuration:=vSentenceDuration, vSentenceDurationQualifier:=vSentenceDurationQualifier, vSentenceEffectiveDate:=vSentenceEffectiveDate, vStatusCode:=vStatusCode, vAlcoholLevel:=vAlcoholLevel, vAlcoholMeasurementMethod:=vAlcoholMeasurementMethod, vDrivingLicencePenaltyPoints:=vDrivingLicencePenaltyPoints, vUniqueId:=vUniqueId, vScreenHierarchy:=vScreenHierarchy), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRPartyConviction = Nothing
                Return result
            End If

            ' Release reference to SIRPartyConviction
            oSIRPartyConviction = Nothing

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
    ' Description: Validate that the specified SIRPartyConviction can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer, Optional vUniqueId As String = "", Optional vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim oSIRPartyConviction As bSIRPartyConviction.SIRPartyConviction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRPartyConvictions.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oSIRPartyConviction = m_oSIRPartyConvictions.Item(lRow)

            ' Check the Status of the SIRPartyConviction

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oSIRPartyConviction.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oSIRPartyConviction.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oSIRPartyConviction.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            oSIRPartyConviction.UniqueId = vUniqueId
            oSIRPartyConviction.ScreenHierarchy = vScreenHierarchy
            ' Release reference to SIRPartyConviction
            oSIRPartyConviction = Nothing

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
            For lSub As Integer = 1 To m_oSIRPartyConvictions.Count()
                Select Case m_oSIRPartyConvictions.Item(lSub).DatabaseStatus
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
        Dim lRecordsAffected As Integer = 0
        Dim lSub As Integer
        Dim oSIRPartyConviction As bSIRPartyConviction.SIRPartyConviction
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oSIRPartyConvictions.Count()
                oSIRPartyConviction = m_oSIRPartyConvictions.Item(lSub)


                Select Case oSIRPartyConviction.DatabaseStatus
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
                        m_lReturn = CType(oSIRPartyConviction.AddItem(), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(oSIRPartyConviction.UpdateItem(), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(oSIRPartyConviction.DeleteItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Retain the Primary Key of the SIRPartyConviction
            With oSIRPartyConviction
                PartyCnt = .PartyCnt
                PartyConvictionID = .PartyConvictionID
            End With

            ' Release last reference
            oSIRPartyConviction = Nothing

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
                    Do While lSub <= m_oSIRPartyConvictions.Count()

                        ' With the item
                        With m_oSIRPartyConvictions.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oSIRPartyConvictions.Delete(lSub)

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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
    'Private Function CheckMandatory(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyConvictionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vConvictionDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vFineAmt As Object = Nothing, Optional ByRef vSentenceCode As Object = Nothing, Optional ByRef vSentenceDescription As Object = Nothing, Optional ByRef vSentenceDuration As Object = Nothing, Optional ByRef vSentenceDurationQualifier As Object = Nothing, Optional ByRef vSentenceEffectiveDate As Object = Nothing, Optional ByRef vStatusCode As Object = Nothing, Optional ByRef vAlcoholLevel As Object = Nothing, Optional ByRef vAlcoholMeasurementMethod As Object = Nothing, Optional ByRef vDrivingLicencePenaltyPoints As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
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

