Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 25/06/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRPartyNC.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 11/02/2004
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

    ' Collection of SIRPartyNCs (Private)
    Private m_oSIRPartyNCs As bSIRPartyNC.SIRPartyNCs

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

    Private m_bGeminiIILink As Boolean

    Private m_oGIIDatabase As dPMDAO.Database

    ' Primary Keys to work with
    Private m_lPartyCnt As Integer

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business
    ' PRIVATE Data Members (End)

    Public Function Login(ByRef sUserID As String) As Integer
        ' ***************************************************************** '
        ' Name: Login (Public)
        '
        ' Description: Gets the required SIRPartyNC login details and returns
        ' a PartyCnt
        '
        ' ***************************************************************** '


        Dim result As Integer = 0
        Dim oSIRPartyNC As bSIRPartyNC.SIRPartyNC

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRPartyNC
            oSIRPartyNC = New bSIRPartyNC.SIRPartyNC()
            m_lReturn = CType(oSIRPartyNC.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            oSIRPartyNC.UserID = sUserID

            m_lReturn = oSIRPartyNC.Login()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Retain the Party Cnt from the login.
            PartyCnt = oSIRPartyNC.PartyCnt

            oSIRPartyNC = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Login Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Login", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Property Procedures (Begin)

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
                Case Is > m_oSIRPartyNCs.Count()
                    m_lCurrentRecord = m_oSIRPartyNCs.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oSIRPartyNCs.Count()

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

    Public ReadOnly Property GeminiIILink() As Boolean
        Get

            Return m_bGeminiIILink

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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer





        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
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

            'Tomo040400
            'Is GII there?

            m_lReturn = CType(gPMComponentServices.CheckPMProductInstalled(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFGeminiII, r_bInstalled:=m_bGeminiIILink), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_bGeminiIILink Then
                m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFGeminiII, r_oDatabase:=m_oGIIDatabase), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create SIRPartyNCs Collection
            m_oSIRPartyNCs = New bSIRPartyNC.SIRPartyNCs()

            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUserName, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions


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
            Me.disposedValue = True
            If disposing Then
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                End If
                m_oLookup = Nothing
                m_oSIRPartyNCs = Nothing
                If Not (m_oGIIDatabase Is Nothing) Then
                    m_oGIIDatabase.CloseDatabase()



                    m_oGIIDatabase = Nothing
                End If
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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIRPartyNC.
    '
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyNC As bSIRPartyNC.SIRPartyNC = Nothing
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 0) As Object
        Dim vCurrencyID As Object = Nothing
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            'developer guide no. 146
            vResultArray = Nothing
            ' Reset Table Array

            'developer guide no. 146
            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = gSIRLibrary.SIRLookupCurrency

            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oSIRPartyNC = m_oSIRPartyNCs.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    With oSIRPartyNC

                        ' {* USER DEFINED CODE (Begin) *}

                        m_lReturn = .bSIRParty.GetLookupProperties(vCurrentRecord:=ToSafeInteger(m_lCurrentRecord), vAreaId:=Nothing, vCurrencyId:=vCurrencyID, vReminderTypeId:=Nothing,
                           vServiceLevelId:=Nothing, vSeasonalGiftID:=Nothing, vCorrespondenceTypeId:=Nothing,
                           vRenewalStopCodeId:=Nothing, vPaymentTermCode:=Nothing)



                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vCurrencyID
                        ' {* USER DEFINED CODE (End) *}

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oSIRPartyNC

                        ' {* USER DEFINED CODE (Begin) *}
                        m_lReturn = CType(.GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView), gPMConstants.PMEReturnCode)



                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vCurrencyID
                        ' {* USER DEFINED CODE (End) *}

                    End With

            End Select

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Release SIRPartyNC reference
            oSIRPartyNC = Nothing

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single SIRPartyNC directly into the database.
    '        Note: The SIRPartyNC will NOT be added to the collection.
    '
    ' ***************************************************************** '
    'eck060600 Pass through Name and Date of Birth
    Public Function DirectAdd(Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vName As Object = Nothing, Optional ByRef vShortName As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vMothersMaidenName As Object = Nothing, Optional ByRef vTPIntroducerCode As Object = Nothing, Optional ByRef vTPUserCode As Object = Nothing, Optional ByRef vMemorableDate As Object = Nothing, Optional ByRef vAQuestion As Object = Nothing, Optional ByRef vTheAnswer As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vCurrInsRenewalDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyNC As bSIRPartyNC.SIRPartyNC
        Dim lPartyTypeId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get party type id for a Net client
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPartyType, v_sCode:=gSIRLibrary.SIRPartyTypeNetClient, v_dtEffectiveDate:=DateTime.Now, r_lID:=lPartyTypeId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Create a new SIRPartyNC
            oSIRPartyNC = New bSIRPartyNC.SIRPartyNC()
            m_lReturn = CType(oSIRPartyNC.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            'eck060600
            'RJG 08/06/2000 - Added Shortname to this DirectAdd call.

            m_lReturn = oSIRPartyNC.bSIRParty.DirectAdd(vPartyTypeId:=ToSafeInteger(lPartyTypeId), vShortName:=vShortName, vName:=vName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyNC = Nothing
                Return result
            End If
            ' Retrieve Primary Key of new Core record

            vPartyCnt = oSIRPartyNC.bSIRParty.PartyCnt
            'EK 22/9/99
            oSIRPartyNC.PartyCnt = vPartyCnt
            ' Populate SIRPartyNC Attributes
            'eck060600 Pass date of birth



            m_lReturn = CType(oSIRPartyNC.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=vPartyCnt, vDateOfBirth:=CDate(vDateOfBirth), vPassword:=CStr(vPassword), vMothersMaidenName:=vMothersMaidenName, vTPIntroducerCode:=vTPIntroducerCode, vTPUserCode:=vTPUserCode, vMemorableDate:=vMemorableDate, vAQuestion:=vAQuestion, vTheAnswer:=vTheAnswer, vUserID:=CStr(vUserID), vCurrInsRenewalDate:=vCurrInsRenewalDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyNC = Nothing
                Return result
            End If

            ' Add the SIRPartyNC to the Database
            m_lReturn = CType(oSIRPartyNC.AddItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyNC = Nothing
                Return result
            End If

            ' Retain the Primary Key of the SIRPartyNC Added
            With oSIRPartyNC
                PartyCnt = .PartyCnt
            End With

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            oSIRPartyNC = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single SIRPartyNC directly from the database.
    '        Note: The SIRPartyNC will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vPartyCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyNC As bSIRPartyNC.SIRPartyNC

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRPartyNC
            oSIRPartyNC = New bSIRPartyNC.SIRPartyNC()
            m_lReturn = CType(oSIRPartyNC.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Set SIRPartyNC Primary Key

            m_lReturn = CType(oSIRPartyNC.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vPartyCnt:=CInt(vPartyCnt)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyNC = Nothing
                Return result
            End If

            ' Delete the SIRPartyNC from the Database
            m_lReturn = CType(oSIRPartyNC.DeleteItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyNC = Nothing
                Return result
            End If


            m_lReturn = oSIRPartyNC.bSIRParty.DirectDelete(vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyNC = Nothing
                Return result
            End If

            oSIRPartyNC = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required SIRPartyNCs and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vLockMode As gPMConstants.PMELockMode = 0, Optional ByRef vPartyCnt As Object = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        'developer guide no. 112
        Dim oFields As DataRow
        Dim oSIRPartyNC As bSIRPartyNC.SIRPartyNC

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oSIRPartyNCs.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Check for Valid Primary Key
            Dim dbNumericTemp2 As Double

            If (Not Informations.IsNothing(vPartyCnt)) And (Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vPartyCnt=" & vPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            If Not Informations.IsNothing(vPartyCnt) Then

                ' Create New SIRPartyNC
                oSIRPartyNC = New bSIRPartyNC.SIRPartyNC()
                m_lReturn = CType(oSIRPartyNC.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                ' Set component primary keys
                With oSIRPartyNC
                    .PartyCnt = vPartyCnt

                    m_lReturn = CType(.SelectItem(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                ' Add SIRPartyNC to collection
                If m_oSIRPartyNCs.Count = 0 Then
                    m_oSIRPartyNCs.Add(Nothing)
                End If
                m_lReturn = CType(m_oSIRPartyNCs.Add(oNewSIRPartyNC:=oSIRPartyNC), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Call the core Business method

                m_lReturn = oSIRPartyNC.bSIRParty.GetDetails(vLockMode:=ToSafeInteger(vLockMode), vPartyCnt:=vPartyCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oSIRPartyNC = Nothing

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
                    oSIRPartyNC = New bSIRPartyNC.SIRPartyNC()
                    m_lReturn = CType(oSIRPartyNC.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                    ' Set oFields to refer to one Record
                    'developer guide no. 111
                    oFields = m_oDatabase.Records.Item(lSub - 1).Fields()

                    ' Set component primary keys from current record
                    With oSIRPartyNC
                        .PartyCnt = gPMFunctions.NullToLong(oFields("party_cnt"))

                        m_lReturn = CType(.SelectItem(), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = .bSIRParty.GetDetails(vLockMode:=ToSafeInteger(vLockMode), vPartyCnt:=ToSafeInteger(.PartyCnt))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End With

                    ' Add SIRPartyNC to collection
                    If m_oSIRPartyNCs.Count = 0 Then
                        m_oSIRPartyNCs.Add(Nothing)
                    End If
                    m_lReturn = CType(m_oSIRPartyNCs.Add(oNewSIRPartyNC:=oSIRPartyNC), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oSIRPartyNC = Nothing
                Next lSub
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required SIRPartyNCs and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyId As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vMothersMaidenName As Object = Nothing, Optional ByRef vTPIntroducerCode As Object = Nothing, Optional ByRef vTPUserCode As Object = Nothing, Optional ByRef vMemorableDate As Object = Nothing, Optional ByRef vAQuestion As Object = Nothing, Optional ByRef vTheAnswer As Object = Nothing, Optional ByRef vSourceId As Object = Nothing, Optional ByRef vShortName As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vCurrInsRenewalDate As Object = Nothing) As Integer

        Dim result As Integer = 0

        Dim oSIRPartyNC As bSIRPartyNC.SIRPartyNC
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oSIRPartyNCs.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oSIRPartyNC = m_oSIRPartyNCs.Item(m_lCurrentRecord)

            ' Get the SIRPartyNC Property Values




            m_lReturn = CType(oSIRPartyNC.GetProperties(iStatus, vPartyCnt:=vPartyCnt, vPassword:=vPassword, vMothersMaidenName:=vMothersMaidenName, vTPIntroducerCode:=vTPIntroducerCode, vTPUserCode:=vTPUserCode, vMemorableDate:=vMemorableDate, vAQuestion:=vAQuestion, vTheAnswer:=vTheAnswer, vDateOfBirth:=vDateOfBirth, vUserID:=vUserID, vCurrInsRenewalDate:=vCurrInsRenewalDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get Core details

            m_lReturn = oSIRPartyNC.bSIRParty.GetDetails(vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oSIRPartyNC.bSIRParty.GetNext(vSourceId:=vSourceId, vPartyId:=vPartyId, vShortName:=vShortName, vName:=vName, vCurrencyID:=vCurrencyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyNC = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied SIRPartyNC into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Object, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vMothersMaidenName As Object = Nothing, Optional ByRef vTPIntroducerCode As Object = Nothing, Optional ByRef vTPUserCode As Object = Nothing, Optional ByRef vMemorableDate As Object = Nothing, Optional ByRef vAQuestion As Object = Nothing, Optional ByRef vTheAnswer As Object = Nothing, Optional ByRef vShortName As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vCurrInsRenewalDate As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oSIRPartyNC As bSIRPartyNC.SIRPartyNC
        Dim lPartyTypeId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get party type id for a Net Client
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPartyType, v_sCode:=gSIRLibrary.SIRPartyTypeNetClient, v_dtEffectiveDate:=DateTime.Now, r_lID:=lPartyTypeId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oSIRPartyNCs.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new SIRPartyNC
            oSIRPartyNC = New bSIRPartyNC.SIRPartyNC()
            m_lReturn = CType(oSIRPartyNC.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Populate SIRPartyNC Attributes



            m_lReturn = CType(oSIRPartyNC.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=CInt(vPartyCnt), vPassword:=CStr(vPassword), vMothersMaidenName:=vMothersMaidenName, vTPIntroducerCode:=vTPIntroducerCode, vTPUserCode:=vTPUserCode, vMemorableDate:=vMemorableDate, vAQuestion:=vAQuestion, vTheAnswer:=vTheAnswer, vUserID:=CStr(vUserID), vCurrInsRenewalDate:=vCurrInsRenewalDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRPartyNC = Nothing
                Return result
            End If

            ' Add SIRPartyNC to collection
            If m_oSIRPartyNCs.Count = 0 Then
                m_oSIRPartyNCs.Add(Nothing)
            End If
            m_lReturn = CType(m_oSIRPartyNCs.Add(oNewSIRPartyNC:=oSIRPartyNC), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyNC = Nothing
                Return result
            End If

            'Added resolved name

            m_lReturn = oSIRPartyNC.bSIRParty.EditAdd(lRow:=lRow, vPartyTypeId:=ToSafeInteger(lPartyTypeId),
                                                      vShortName:=vShortName, vName:=vName, vResolvedName:=vName,
                                                      vCurrencyID:=vCurrencyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyNC = Nothing
                Return result
            End If
            oSIRPartyNC = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the SIRPartyNC
    '              specified and updates the SIRPartyNC with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Object, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vMothersMaidenName As Object = Nothing, Optional ByRef vTPIntroducerCode As Object = Nothing, Optional ByRef vTPUserCode As Object = Nothing, Optional ByRef vMemorableDate As Object = Nothing, Optional ByRef vAQuestion As Object = Nothing, Optional ByRef vTheAnswer As Object = Nothing, Optional ByRef vShortName As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vCurrInsRenewalDate As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyNC As bSIRPartyNC.SIRPartyNC
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRPartyNCs.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oSIRPartyNC = m_oSIRPartyNCs.Item(lRow)

            ' Check the Status of the SIRPartyNC

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oSIRPartyNC.DatabaseStatus
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

            ' Update SIRPartyNC Attributes




            m_lReturn = CType(oSIRPartyNC.SetProperties(iStatus:=iStatus, vPartyCnt:=CInt(vPartyCnt), vPassword:=CStr(vPassword), vMothersMaidenName:=vMothersMaidenName, vTPIntroducerCode:=vTPIntroducerCode, vTPUserCode:=vTPUserCode, vMemorableDate:=vMemorableDate, vAQuestion:=vAQuestion, vTheAnswer:=vTheAnswer, vUserID:=CStr(vUserID), vCurrInsRenewalDate:=vCurrInsRenewalDate, vDateOfBirth:=CDate(vDateOfBirth)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRPartyNC = Nothing
                Return result
            End If

            'Added resolved name

            m_lReturn = oSIRPartyNC.bSIRParty.EditUpdate(lRow:=lRow, vShortName:=vShortName, vName:=vName,
                                                         vResolvedName:=vName, vCurrencyID:=vCurrencyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyNC = Nothing
                Return result
            End If
            ' Release reference to SIRPartyNC
            oSIRPartyNC = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified SIRPartyNC can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyNC As bSIRPartyNC.SIRPartyNC

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRPartyNCs.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oSIRPartyNC = m_oSIRPartyNCs.Item(lRow)

            ' Check the Status of the SIRPartyNC

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oSIRPartyNC.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oSIRPartyNC.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oSIRPartyNC.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If


            m_lReturn = oSIRPartyNC.bSIRParty.EditDelete(lRow:=ToSafeInteger(lRow))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRPartyNC = Nothing
                Return result
            End If
            ' Release reference to SIRPartyNC
            oSIRPartyNC = Nothing

            Return result

        Catch excep As System.Exception




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
            For lSub As Integer = 1 To m_oSIRPartyNCs.Count()
                Select Case m_oSIRPartyNCs.Item(lSub).DatabaseStatus
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
        Dim oSIRPartyNC As bSIRPartyNC.SIRPartyNC = Nothing
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oSIRPartyNCs.Count()
                oSIRPartyNC = m_oSIRPartyNCs.Item(lSub)


                Select Case oSIRPartyNC.DatabaseStatus
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

                        m_lReturn = oSIRPartyNC.bSIRParty.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                        ' Retrieve Primary Key of Core Item added

                        PartyCnt = oSIRPartyNC.bSIRParty.PartyCnt
                        oSIRPartyNC.PartyCnt = PartyCnt

                        m_lReturn = CType(oSIRPartyNC.AddItem(), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(oSIRPartyNC.UpdateItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If


                        m_lReturn = oSIRPartyNC.bSIRParty.Update()
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
                        m_lReturn = CType(oSIRPartyNC.DeleteItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If


                        m_lReturn = oSIRPartyNC.bSIRParty.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If
                End Select

            Next lSub

            ' Retain the Primary Key of the SIRPartyNC
            With oSIRPartyNC
                PartyCnt = .PartyCnt
            End With

            ' Release last reference
            oSIRPartyNC = Nothing

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
                    Do While lSub <= m_oSIRPartyNCs.Count()

                        ' With the item
                        With m_oSIRPartyNCs.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oSIRPartyNCs.Delete(lSub)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Name: CheckReference (Public)
    '
    ' Description: Checks if the passed shortname (reference) already exists
    '
    ' ***************************************************************** '
    Public Function CheckReference(ByRef sReference As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckReference"

        Dim oParty As bSIRParty.Business
        Dim bExists As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create an instance of the main party business object
            oParty = New bSIRParty.Business()
            m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oParty.Initialise", "oParty = bSIRParty.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Pass in the account code to see if it already exists
            m_lReturn = oParty.CheckIfAccountCodeExists(v_sAccountCode:=sReference, r_bExists:=bExists)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oParty.CheckIfAccountCodeExists", "v_sAccountCode:=" & sReference, gPMConstants.PMELogLevel.PMLogError)
            End If

            'If the Reference was found then return empty string
            If bExists Then
                sReference = ""
            End If

            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '		Return result

            ' This is for debugging only
            '		Resume 

            '		Return result
        End Try
        Return result
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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAddressDetails
    '
    ' Description: Get address details for party.
    '
    ' ***************************************************************** '
    Public Function GetAddressDetails(ByRef vAddresses As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyNC As bSIRPartyNC.SIRPartyNC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyNC - need to hit core for address stuff
            oSIRPartyNC = New bSIRPartyNC.SIRPartyNC()

            m_lReturn = CType(oSIRPartyNC.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            m_lReturn = oSIRPartyNC.bSIRParty.GetAddressDetails(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vAddresses:=vAddresses)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyNC = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAddressDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddressDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAddressTypeLookups
    '
    ' Description: Get address type lookups.
    '
    ' ***************************************************************** '
    Public Function GetAddressTypeLookups(ByRef vAddressTypes As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyNC As bSIRPartyNC.SIRPartyNC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyNC - need to hit core for address stuff
            oSIRPartyNC = New bSIRPartyNC.SIRPartyNC()

            m_lReturn = CType(oSIRPartyNC.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            m_lReturn = oSIRPartyNC.bSIRParty.GetAddressTypeLookups(vAddressTypes:=vAddressTypes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyNC = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAddressTypeLookups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddressTypeLookups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetContactDetails
    '
    ' Description: Get contact details for party.
    '
    ' ***************************************************************** '
    Public Function GetContactDetails(ByRef vContacts As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyNC As bSIRPartyNC.SIRPartyNC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyNC - need to hit core for address stuff
            oSIRPartyNC = New bSIRPartyNC.SIRPartyNC()

            m_lReturn = CType(oSIRPartyNC.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            m_lReturn = oSIRPartyNC.bSIRParty.GetContactDetails(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vContacts:=vContacts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyNC = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetContactDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetContactDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UpdateAddresses
    '
    ' Description: Update the party_address usage table with old
    ' and new addresses for the party.
    '
    ' ***************************************************************** '
    Public Function UpdateAddresses(ByRef vPartyCnt As Object, Optional ByRef vAddAddresses As Object = Nothing, Optional ByRef vDeleteAddresses As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyNC As bSIRPartyNC.SIRPartyNC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyNC - need to hit core for address updates
            oSIRPartyNC = New bSIRPartyNC.SIRPartyNC()

            m_lReturn = CType(oSIRPartyNC.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            m_lReturn = oSIRPartyNC.bSIRParty.UpdateAddresses(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vAddAddresses:=vAddAddresses,
                                                              vDeleteAddresses:=vDeleteAddresses)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyNC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddresses", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateGemini
    '
    ' Description: Update Gemini
    '
    ' ***************************************************************** '
    Public Function UpdateGemini(ByRef vPartyCnt As Object, ByRef vTask As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyNC As bSIRPartyNC.SIRPartyNC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyPC - need to hit core to update gemini
            oSIRPartyNC = New bSIRPartyNC.SIRPartyNC()

            m_lReturn = CType(oSIRPartyNC.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oSIRPartyNC.bSIRParty.UpdateGemini(vPartyCnt:=vPartyCnt, vTask:=vTask)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyNC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateGemini Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateGemini", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: UpdateContacts
    '
    ' Description: Update the party_contact usage table with old
    ' and new contacts for the party.
    '
    ' ***************************************************************** '
    Public Function UpdateContacts(ByRef vPartyCnt As Object, Optional ByRef vAddContacts As Object = Nothing, Optional ByRef vDeleteContacts As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRPartyNC As bSIRPartyNC.SIRPartyNC


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create New SIRPartyNC - need to hit core for address updates
            oSIRPartyNC = New bSIRPartyNC.SIRPartyNC()

            m_lReturn = CType(oSIRPartyNC.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            m_lReturn = oSIRPartyNC.bSIRParty.UpdateContacts(vPartyCnt:=ToSafeInteger(m_lPartyCnt), vAddContacts:=vAddContacts, vDeleteContacts:=vDeleteContacts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRPartyNC = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateContacts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateContacts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    'Private Function CheckMandatory(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vMothersMaidenName As Object = Nothing, Optional ByRef vTPIntroducerCode As Object = Nothing, Optional ByRef vTPUserCode As Object = Nothing, Optional ByRef vMemorableDate As Object = Nothing, Optional ByRef vAQuestion As Object = Nothing, Optional ByRef vTheAnswer As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vCurrInsRenewalDate As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' {* USER DEFINED CODE (Begin) *}
    '


    'If (Informations.IsNothing(vPartyCnt)) Or (Object.Equals(vPartyCnt, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Informations.IsNothing(vPassword)) Or (Object.Equals(vPassword, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '
    'RJG 09/06/2000 - User ID is mandatory


    'If (Informations.IsNothing(vUserID)) Or (Object.Equals(vUserID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '
    '
    ' {* USER DEFINED CODE (End) *}
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
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

