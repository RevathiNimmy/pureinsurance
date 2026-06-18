Option Strict Off
Option Explicit On
Imports System.Globalization
'developer guide no. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: {TodaysDate}
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a CLMRiskDetails.
    '
    ' Edit History:
    '              SET 15052002 - Removed checks for Underwriting in
    '              AddClaimPeril function.
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 24/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)
    ' PRIVATE Data Members (Begin)

    ' Collection of CLMRiskDetailss (Private)
    Private m_oCLMRiskDetailss As bCLMRiskDetails.RiskDetailss

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
    Private m_lClaimID As Integer
    Private m_lRiskTypeID As Integer
    Private m_lRiskDataDefnID As Integer

    'RWH(13/11/2000) Claim numbering
    Private m_sUnderwritingOrAgency As String = ""

    'CMG/PB LossSchedule
    Private m_bLossSchedule As Boolean
    Private m_lPerilID As Integer
    'End CMG

    '***LookupBegin***
    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business
    Private m_oAuthorisePayments As bCLMAuthorisePayments.Business
    Private m_oOpenClaim As Object

    '***LookupEnd***
    ' PRIVATE Data Members (End)
    ' PUBLIC Property Procedures (Begin)

    'RWH(13/11/2000) Claims Numbering.

    'CMG/PB 25092002 Loss Schedule Needs to set this,otherwise it is unknown
    'when loss schedule Risk Toolbar is clicked and accesses this component
    Public Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
        Set(ByVal Value As String)

            m_sUnderwritingOrAgency = Value

        End Set
    End Property
    'End CMG


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
                    '    Case Is > m_oCLMRiskDetailss.ClaimID
                    '        m_lCurrentRecord& = m_oCLMRiskDetailss.Count
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oCLMRiskDetailss.Count()

        End Get
    End Property

    Public Property ClaimId() As Integer
        Get

            Return m_lClaimID

        End Get
        Set(ByVal Value As Integer)

            m_lClaimID = Value

        End Set
    End Property
    Public Property PerilID() As Integer
        Get

            Return m_lPerilID

        End Get
        Set(ByVal Value As Integer)

            m_lPerilID = PerilID

        End Set
    End Property
    Public Property RiskTypeID() As Integer
        Get

            Return m_lRiskTypeID

        End Get
        Set(ByVal Value As Integer)

            m_lRiskTypeID = Value

        End Set
    End Property



    Public Property RiskDataDefnID() As Integer
        Get

            Return m_lRiskDataDefnID

        End Get
        Set(ByVal Value As Integer)

            m_lRiskDataDefnID = Value

        End Set
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
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

            ' Create CLMRiskDetailss Collection
            m_oCLMRiskDetailss = New bCLMRiskDetails.RiskDetailss()


            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

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
                    m_oLookup = Nothing
                End If
                m_oCLMRiskDetailss = Nothing
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
    ' Description: Gets the Lookup values for a CLMRiskDetails.
    '
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByVal iLookupType As Integer, ByRef vTableArray(,) As Object, ByVal iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        'Dim oOpenClaim As bOpenClaim.Business

        Dim result As Integer = 0
        Dim dtEffectiveDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Release OpenClaim reference

            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=m_sUsername.Trim(), sPassword:=m_sPassword.Trim(), iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            ' Get the Lookup items

            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTableArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


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
    ' Description: Adds a single CLMRiskDetails directly into the database.
    '        Note: The CLMRiskDetails will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vProgressStatusID As Object = Nothing, Optional ByRef vClaimStatusID As Object = Nothing, Optional ByRef vClaimDescription As Object = Nothing, Optional ByRef vPrimaryCauseID As Object = Nothing, Optional ByRef vSecondaryCauseID As Object = Nothing, Optional ByRef vPerilTypeID As Object = Nothing, Optional ByRef vPerilDescription As Object = Nothing, Optional ByRef vClaimNumber As Object = Nothing, Optional ByRef vSumInsured As Object = Nothing, Optional ByRef vCurrentReserve As Object = Nothing, Optional ByRef vComments As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCLMRiskDetails As bCLMRiskDetails.RiskDetails

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new CLMRiskDetails
            oCLMRiskDetails = New bCLMRiskDetails.RiskDetails()
            m_lReturn = CType(oCLMRiskDetails.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Populate CLMRiskDetails Attributes
            'developer guide no.98
            m_lReturn = oCLMRiskDetails.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vProgressStatusID:=vProgressStatusID, vClaimStatusID:=vClaimStatusID, vClaimDescription:=vClaimDescription, vPrimaryCauseID:=vPrimaryCauseID, vSecondaryCauseID:=vSecondaryCauseID, vPerilTypeID:=vPerilTypeID, vPerilDescription:=vPerilDescription, vClaimNumber:=vClaimNumber, vSumInsured:=vSumInsured, vCurrentReserve:=vCurrentReserve, vComments:=vComments)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMRiskDetails = Nothing
                Return result
            End If

            ' Add the CLMRiskDetails to the Database
            m_lReturn = CType(oCLMRiskDetails.AddItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMRiskDetails = Nothing
                Return result
            End If

            ' Retain the Primary Key of the CLMRiskDetails Added
            With oCLMRiskDetails
                ClaimId = .ClaimId
                RiskTypeID = .RiskTypeID
                RiskDataDefnID = .RiskDataDefnID
            End With

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            oCLMRiskDetails = Nothing

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
    ' Description: Deletes a single CLMRiskDetails directly from the database.
    '        Note: The CLMRiskDetails will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vClaimID As Object = Nothing, Optional ByRef vRiskTypeID As Object = Nothing, Optional ByRef vRiskDataDefnID As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oCLMRiskDetails As bCLMRiskDetails.RiskDetails

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new CLMRiskDetails
            oCLMRiskDetails = New bCLMRiskDetails.RiskDetails()
            m_lReturn = CType(oCLMRiskDetails.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMRiskDetails = Nothing
                Return result
            End If

            ' Delete the CLMRiskDetails from the Database
            m_lReturn = CType(oCLMRiskDetails.DeleteItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMRiskDetails = Nothing
                Return result
            End If

            oCLMRiskDetails = Nothing

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
            '    m_lReturn& = m_oDatabase.SQLSelect( _
            ''        sSQl:=ACCheckIDSQL, _
            ''        sSQLName:=ACCheckIDName, _
            ''        bStoredProcedure:=ACCheckIDStored, _
            ''        lNumberRecords:=0)

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

    ' *************************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required CLMRiskDetailss and populate the Collection
    '
    ' *************************************************************************** '
    Public Function GetDetails(Optional ByRef vLockMode As gPMConstants.PMELockMode = 0, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vRiskTypeID As Object = Nothing, Optional ByRef vRiskDataDefnID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oFields As ADODB.Fields
        Dim oCLMRiskDetails As bCLMRiskDetails.RiskDetails

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oCLMRiskDetailss.Clear()

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
            ' ***GetDetailsCheckKeys***

            Dim dbNumericTemp2 As Double

            If (Not Informations.IsNothing(vClaimID)) And (Not Double.TryParse(CStr(vClaimID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vClaimID=" & CStr(vClaimID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            If Not Informations.IsNothing(vClaimID) Then
                ' Create New CLMRiskDetails
                oCLMRiskDetails = New bCLMRiskDetails.RiskDetails()
                m_lReturn = CType(oCLMRiskDetails.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                ' Set component primary keys
                With oCLMRiskDetails
                    .ClaimId = gPMFunctions.NullToLong(oFields("claim_id"))

                    m_lReturn = CType(.SelectItem(g_iClaim), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                ' Add CLMRiskDetails to collection
                If m_oCLMRiskDetailss.Count = 0 Then
                    m_oCLMRiskDetailss.Add(Nothing)
                End If
                m_lReturn = CType(m_oCLMRiskDetailss.Add(oNewCLMRiskDetails:=oCLMRiskDetails), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                oCLMRiskDetails = Nothing
            End If

            Dim dbNumericTemp3 As Double

            If (Not Informations.IsNothing(vRiskTypeID)) And (Not Double.TryParse(CStr(vRiskTypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vRiskTypeID=" & CStr(vRiskTypeID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            If Not Informations.IsNothing(vRiskTypeID) Then

                ' Create New CLMRiskDetails
                oCLMRiskDetails = New bCLMRiskDetails.RiskDetails()
                m_lReturn = CType(oCLMRiskDetails.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                ' Set component primary keys
                With oCLMRiskDetails
                    .RiskTypeID = gPMFunctions.NullToLong(oFields("risk_type_id"))

                    m_lReturn = CType(.SelectItem(g_iRiskType), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With


                ' Add CLMRiskDetails to collection
                If m_oCLMRiskDetailss.Count = 0 Then
                    m_oCLMRiskDetailss.Add(Nothing)
                End If
                m_lReturn = CType(m_oCLMRiskDetailss.Add(oNewCLMRiskDetails:=oCLMRiskDetails), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oCLMRiskDetails = Nothing

            End If


            Dim dbNumericTemp4 As Double

            If (Not Informations.IsNothing(vRiskDataDefnID)) And (Not Double.TryParse(CStr(vRiskDataDefnID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vRiskDataDefnID=" & CStr(vRiskDataDefnID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            If Not Informations.IsNothing(vRiskDataDefnID) Then

                ' Create New CLMRiskDetails
                oCLMRiskDetails = New bCLMRiskDetails.RiskDetails()
                m_lReturn = CType(oCLMRiskDetails.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                ' Set component primary keys
                With oCLMRiskDetails
                    .RiskDataDefnID = gPMFunctions.NullToLong(oFields("risk_data_Defn_id"))

                    m_lReturn = CType(.SelectItem(g_iRiskDataDefn), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With


                ' Add CLMRiskDetails to collection
                If m_oCLMRiskDetailss.Count = 0 Then
                    m_oCLMRiskDetailss.Add(Nothing)
                End If
                m_lReturn = CType(m_oCLMRiskDetailss.Add(oNewCLMRiskDetails:=oCLMRiskDetails), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oCLMRiskDetails = Nothing

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
    ' Description: Gets the required CLMRiskDetailss and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vProgressStatusID As Object = Nothing, Optional ByRef vClaimStatusID As Object = Nothing, Optional ByRef vClaimDescription As Object = Nothing, Optional ByRef vPrimaryCauseID As Object = Nothing, Optional ByRef vSecondaryCauseID As Object = Nothing, Optional ByRef vPerilTypeID As Object = Nothing, Optional ByRef vPerilDescription As Object = Nothing, Optional ByRef vClaimNumber As Object = Nothing, Optional ByRef vSumInsured As Object = Nothing, Optional ByRef vCurrentReserve As Object = Nothing, Optional ByRef vComments As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oCLMRiskDetails As bCLMRiskDetails.RiskDetails
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oCLMRiskDetailss.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oCLMRiskDetails = m_oCLMRiskDetailss.Item(m_lCurrentRecord)

            ' Get the CLMRiskDetails Property Values

            'developer guide no.98
            m_lReturn = oCLMRiskDetails.GetProperties(iStatus, vProgressStatusID:=vProgressStatusID, vClaimStatusID:=vClaimStatusID, vClaimDescription:=vClaimDescription, vPrimaryCauseID:=vPrimaryCauseID, vSecondaryCauseID:=vSecondaryCauseID, vPerilTypeID:=vPerilTypeID, vPerilDescription:=vPerilDescription, vClaimNumber:=vClaimNumber, vSumInsured:=vSumInsured, vCurrentReserve:=vCurrentReserve, vComments:=vComments)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oCLMRiskDetails = Nothing

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
    ' Description: Adds the supplied CLMRiskDetails into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vProgressStatusID As Object = Nothing, Optional ByRef vClaimStatusID As Object = Nothing, Optional ByRef vClaimDescription As Object = Nothing, Optional ByRef vPrimaryCauseID As Object = Nothing, Optional ByRef vSecondaryCauseID As Object = Nothing, Optional ByRef vPerilTypeID As Object = Nothing, Optional ByRef vPerilDescription As Object = Nothing, Optional ByRef vClaimNumber As Object = Nothing, Optional ByRef vSumInsured As Object = Nothing, Optional ByRef vCurrentReserve As Object = Nothing, Optional ByRef vComments As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCLMRiskDetails As bCLMRiskDetails.RiskDetails

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oCLMRiskDetailss.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new CLMRiskDetails
            oCLMRiskDetails = New bCLMRiskDetails.RiskDetails()
            m_lReturn = CType(oCLMRiskDetails.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Populate CLMRiskDetails Attributes

            'developer guide no.98
            m_lReturn = oCLMRiskDetails.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vProgressStatusID:=vProgressStatusID, vClaimStatusID:=vClaimStatusID, vClaimDescription:=vClaimDescription, vPrimaryCauseID:=vPrimaryCauseID, vSecondaryCauseID:=vSecondaryCauseID, vPerilTypeID:=vPerilTypeID, vPerilDescription:=vPerilDescription, vClaimNumber:=vClaimNumber, vSumInsured:=vSumInsured, vCurrentReserve:=vCurrentReserve, vComments:=vComments)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCLMRiskDetails = Nothing
                Return result
            End If

            ' Add CLMRiskDetails to collection
            If m_oCLMRiskDetailss.Count = 0 Then
                m_oCLMRiskDetailss.Add(Nothing)
            End If
            m_lReturn = CType(m_oCLMRiskDetailss.Add(oNewCLMRiskDetails:=oCLMRiskDetails), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMRiskDetails = Nothing
                Return result
            End If

            oCLMRiskDetails = Nothing

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
    ' Description: Validates that this action is valid on the CLMRiskDetails
    '              specified and updates the CLMRiskDetails with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vProgressStatusID As Object = Nothing, Optional ByRef vClaimStatusID As Object = Nothing, Optional ByRef vClaimDescription As Object = Nothing, Optional ByRef vPrimaryCauseID As Object = Nothing, Optional ByRef vSecondaryCauseID As Object = Nothing, Optional ByRef vPerilTypeID As Object = Nothing, Optional ByRef vPerilDescription As Object = Nothing, Optional ByRef vClaimNumber As Object = Nothing, Optional ByRef vSumInsured As Object = Nothing, Optional ByRef vCurrentReserve As Object = Nothing, Optional ByRef vComments As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oCLMRiskDetails As bCLMRiskDetails.RiskDetails
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCLMRiskDetailss.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oCLMRiskDetails = m_oCLMRiskDetailss.Item(lRow)

            ' Check the Status of the CLMRiskDetails

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oCLMRiskDetails.DatabaseStatus
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

            ' Update CLMRiskDetails Attributes

            'developer guide no.98
            m_lReturn = oCLMRiskDetails.SetProperties(iStatus:=iStatus, vProgressStatusID:=vProgressStatusID, vClaimStatusID:=vClaimStatusID, vClaimDescription:=vClaimDescription, vPrimaryCauseID:=vPrimaryCauseID, vSecondaryCauseID:=vSecondaryCauseID, vPerilTypeID:=vPerilTypeID, vPerilDescription:=vPerilDescription, vClaimNumber:=vClaimNumber, vSumInsured:=vSumInsured, vCurrentReserve:=vCurrentReserve, vComments:=vComments)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCLMRiskDetails = Nothing
                Return result
            End If

            ' Release reference to CLMRiskDetails
            oCLMRiskDetails = Nothing

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
    ' Description: Validate that the specified CLMRiskDetails can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oCLMRiskDetails As bCLMRiskDetails.RiskDetails

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCLMRiskDetailss.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oCLMRiskDetails = m_oCLMRiskDetailss.Item(lRow)

            ' Check the Status of the CLMRiskDetails

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oCLMRiskDetails.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oCLMRiskDetails.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oCLMRiskDetails.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to CLMRiskDetails
            oCLMRiskDetails = Nothing

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
            For lSub As Integer = 1 To m_oCLMRiskDetailss.Count()
                Select Case m_oCLMRiskDetailss.Item(lSub).DatabaseStatus
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
        Dim oCLMRiskDetails As New bCLMRiskDetails.RiskDetails
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oCLMRiskDetailss.Count()
                oCLMRiskDetails = m_oCLMRiskDetailss.Item(lSub)


                Select Case oCLMRiskDetails.DatabaseStatus
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
                        m_lReturn = CType(oCLMRiskDetails.AddItem(), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(oCLMRiskDetails.UpdateItem(), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(oCLMRiskDetails.DeleteItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Retain the Primary Key of the CLMRiskDetails
            With oCLMRiskDetails
                ClaimId = .ClaimId
                RiskTypeID = .RiskTypeID
                RiskDataDefnID = .RiskDataDefnID
            End With

            ' Release last reference
            oCLMRiskDetails = Nothing

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
                    Do While lSub <= m_oCLMRiskDetailss.Count()

                        ' With the item
                        With m_oCLMRiskDetailss.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oCLMRiskDetailss.Delete(lSub)

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

    ' ***************************************************************** '
    '
    ' Name: DeleteClaim
    '
    ' Description:
    '
    ' History: 08/05/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteClaim() As Integer

        Dim result As Integer = 0
        Dim lStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(m_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="status", vValue:=CStr(lStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteClaimSQL, sSQLName:=ACDeleteClaimName, bStoredProcedure:=ACDeleteClaimStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            lStatus = m_oDatabase.Parameters.Item("status").Value

            If lStatus <> 0 Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    ' Name: CheckMandatory (Private)
    '
    ' Description: Check Mandatory parameters have been passed.
    '
    ' ***************************************************************** '

    'Private Function CheckMandatory(Optional ByRef vProgressStatusID As Object = Nothing, Optional ByRef vClaimStatusID As Object = Nothing, Optional ByRef vClaimDescription As Object = Nothing, Optional ByRef vPrimaryCauseID As Object = Nothing, Optional ByRef vSecondaryCauseID As Object = Nothing, Optional ByRef vPerilTypeID As Object = Nothing, Optional ByRef vPerilDescription As Object = Nothing, Optional ByRef vClaimNumber As Object = Nothing, Optional ByRef vSumInsured As Object = Nothing, Optional ByRef vCurrentReserve As Object = Nothing, Optional ByRef vComments As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' {* USER DEFINED CODE (Begin) *}
    '


    'If (Informations.IsNothing(vProgressStatusID)) Or (Object.Equals(vProgressStatusID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '
    ' to add the code for the dynamic controls
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


    ' ***************************************************************** '
    ' Name: GetFieldsForRiskDataDefn (Public)
    '
    ' Description: Gets the required fields & thier values for the General Tab
    '
    ' ***************************************************************** '
    'MSS250901 - Added Optional v_iMandatory for merge

    Public Function GetFieldsForRiskDataDefn(ByVal v_lRiskTypeId As Integer, ByVal v_lClaimId As Integer, ByRef r_vResultArray As Object) As Integer
        Return GetFieldsForRiskDataDefn(v_lRiskTypeId:=v_lRiskTypeId, v_lClaimId:=v_lClaimId, r_vResultArray:=r_vResultArray, v_iMandatory:=0)
    End Function
    Public Function GetFieldsForRiskDataDefn(ByVal v_lRiskTypeId As Integer, ByVal v_lClaimId As Integer, ByRef r_vResultArray As Object, ByRef v_iMandatory As Integer) As Integer

        Dim result As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="RiskType", vValue:=CStr(v_lRiskTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'MSS250901 - Added for merge.
            'Question - if the SQL has 3 question marks, don't we _always_ need 3 parameters?
            'AK 190401 - To implement mandatory fields - Begin
            m_lReturn = m_oDatabase.Parameters.Add(sName:="mandatory", vValue:=CStr(v_iMandatory), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'AK 190401 - End
            'MSS250901 - Merge end
            r_vResultArray = Nothing
            m_lReturn = m_oDatabase.SQLSelect(ACGetFieldsForRiskDataDefnSQL, ACGetFieldsForRiskDataDefnName, ACGetFieldsForRiskDataDefnStored, , r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Do we have any records ?
            If Not Informations.IsArray(r_vResultArray) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFieldsForRiskDataDefn Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFieldsForRiskDataDefn", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDataForRiskDataDefn (Public)
    '
    ' Description: Gets the required fields & thier values for the General Tab
    '
    ' ***************************************************************** '
    Public Function GetDataForRiskDataDefn(ByVal v_lRiskDataDefn As Integer, ByVal v_lClaimId As Integer, ByRef r_vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="RiskDataDefn", vValue:=CStr(v_lRiskDataDefn), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimId", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(ACGetDataForRiskDataDefnSQL, ACGetDataForRiskDataDefnName, ACGetDataForRiskDataDefnStored, , r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(r_vResultArray) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataForRiskDataDefn Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataForRiskDataDefn", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function GetPartyTypesforRiskType(ByVal v_lRiskTypeId As Integer, ByRef r_vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="RiskTypeID", vValue:=CStr(v_lRiskTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            r_vResultArray = Nothing
            m_lReturn = m_oDatabase.SQLSelect(ACGetPartyTypesforRiskTypeSQL, ACGetPartyTypesforRiskTypeName, ACGetPartyTypesforRiskTypeStored, , r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(r_vResultArray) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyTypesforRiskType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyTypesforRiskType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' To get party details for a claim pertaining to a party type use stored procedure.
    Public Function GetPartyDetailsForClaim(ByVal v_lClaimId As Integer, ByVal v_lPartyTypeId As Integer, ByRef r_vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear all the parameters
            m_oDatabase.Parameters.Clear()

            ' Pass the Claim Id to the stored procedure.
            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimId", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Pass the Party type to the stored procedure.
            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="PartyTypeId", vValue:=CStr(v_lPartyTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure. The records returned by the stored procedure is in r_vResultArray


            r_vResultArray = Nothing
            m_lReturn = m_oDatabase.SQLSelect(ACGetPartyDetailsforClaimSQL, ACGetPartyDetailsforClaimName, ACGetPartyDetailsForClaimStored, , r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(r_vResultArray) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyDetailsForClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyDetailsForClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    'DC240402 -added description
    Public Function GetCommentsForClaim(ByVal v_lClaimId As Integer, ByVal v_lRiskTypeId As Integer, ByRef v_vDescription As String, ByRef r_vResultArray(,) As Object) As Integer


        Dim result As Integer = 0

        Try

            m_oDatabase.Parameters.Clear()
            result = gPMConstants.PMEReturnCode.PMTrue

            'DC240402 -Start
            v_vDescription = ""

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="RiskTypeID", vValue:=CStr(v_lRiskTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure. The records returned by the stored procedure is in r_vResultArray


            r_vResultArray = Nothing
            m_lReturn = m_oDatabase.SQLSelect(ACGetCommentsForClaimSQL, ACGetCommentsForClaimName, ACGetCommentsForClaimStored, , r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(r_vResultArray) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            'DC240402 -End

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCommentsForClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommentsForClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function GetBasicClaimDetails(ByVal v_lClaimId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="claimId", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure. The records returned by the stored procedure is in r_vResultArray


            r_vResultArray = Nothing

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBasicClaimDetailsSQL, sSQLName:=ACGetBasicClaimDetailsName, bStoredProcedure:=ACGetBasicClaimDetailsStored, vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(r_vResultArray) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBasicClaimDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBasicClaimDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function CheckForExistenceinClaimRisk(ByVal v_lClaimId As Integer, ByVal v_lRiskTypeId As Integer, ByRef r_bExists As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="RiskTypeId", vValue:=CStr(v_lRiskTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimExists", vValue:=CStr(r_bExists), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBinary)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure. The records returned by the stored procedure is in r_vResultArray
            m_lReturn = m_oDatabase.SQLAction(ACCheckForExistenceinClaimRiskSQL, ACCheckForExistenceinClaimRiskName, ACCheckForExistenceinClaimRiskStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_bExists = m_oDatabase.Parameters.Item(CStr(3)).Value
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForExistenceinClaimRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckForExistenceinClaimRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    Public Function AddClaimRisk(ByVal v_lClaimId As Integer, ByVal v_lRiskTypeId As Integer, ByVal v_sDescription As String, ByVal v_sComments As String) As Integer

        'DC190402 -Start
        Dim result As Integer = 0
        Dim vClaimComments(,) As Object
        Dim iTxtPointer As Integer
        Dim sTextLine As String = ""
        Dim iCount As Integer
        'DC190402 -End

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimId", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="RiskTypeId", vValue:=CStr(v_lRiskTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Description", vValue:=v_sDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC240402 -Start

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Comments", vValue:=v_sComments, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            'DC240402 -End

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(ACAddClaimRiskSQL, ACAddClaimRiskName, ACAddClaimRiskStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'S4B Claim Enhancements R&D 2005 - remove Risk level comments for Broking


            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Comment_Type", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Entity_id", vValue:=CStr(v_lRiskTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteClaimCommentsSQL, sSQLName:=ACDeleteClaimCommentsName, bStoredProcedure:=ACDeleteClaimCommentsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'New collection of text
            ReDim vClaimComments(0, 0)

            'Break up the entered comments into a collection of TextLines
            iTxtPointer = 1
            iCount = 0
            While iTxtPointer < v_sComments.Length
                sTextLine = v_sComments.Substring(iTxtPointer - 1, Math.Min(v_sComments.Length, 255))
                ReDim Preserve vClaimComments(0, iCount)

                vClaimComments(0, iCount) = sTextLine
                iTxtPointer += 255
                iCount += 1
            End While


            For iCount = vClaimComments.GetLowerBound(1) To vClaimComments.GetUpperBound(1)

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Comment_Type", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Entity_id", vValue:=CStr(v_lRiskTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Comment_Id", vValue:=CStr(iCount + 1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="Comment_Line", vValue:=CStr(vClaimComments(0, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddClaimCommentsSQL, sSQLName:=ACAddClaimCommentsName, bStoredProcedure:=ACAddClaimCommentsStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next iCount

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddClaimRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddClaimRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function AddClaimPartyClaim(ByVal v_lClaimId As Integer, ByVal v_lPartyId As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="PartyClaimId", vValue:=CStr(v_lPartyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimId", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(ACAddClaimPartyClaimSQL, ACAddClaimPartyClaimName, ACAddClaimPartyClaimStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddClaimPartyClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddClaimPartyClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Public Function GetLookupTables(ByVal v_sLookupIDs As String, ByRef r_vResultArrray As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Lookup", vValue:=v_sLookupIDs, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLSelect(ACGetLookupTablesSQL, ACGetLookupTablesName, ACGetLookupTablesStored, , r_vResultArrray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(r_vResultArrray) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupTablesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupTables", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function AddPeril(ByVal ClaimId As Integer, ByVal PerilTypeId As Integer, ByRef PerilID As Integer) As Integer

        Dim result As Integer = 0
        Try



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPeril", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPeril", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function AddGeneralDetail(ByVal v_lClaimId As Integer, ByVal v_lRiskDataDefn As Integer, ByVal v_sValue As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimId", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="RiskDataDefn", vValue:=CStr(v_lRiskDataDefn), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Value", vValue:=If(v_sValue = "", DBNull.Value, v_sValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(ACAddGeneralDetailSQL, ACAddGeneralDetailName, ACAddGeneralDetailStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddGeneralDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGeneralDetail", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function DeleteClaimPartyClaim(ByVal v_lClaimId As Integer, ByVal v_lPartyId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="PartyClaimId", vValue:=CStr(v_lPartyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimId", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(ACDeleteClaimPartyClaimSQL, ACDeleteClaimPartyClaimName, ACDeleteClaimPartyClaimStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteClaimPartyClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClaimPartyClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Public Function GetSiriusProduct(ByRef r_sSiriusProduct As String) As Integer
        Dim result As Integer = 0
        Dim oBackOffice As bBackOfficeLink.bBOLink
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            r_sSiriusProduct = ""
            oBackOffice = New bBackOfficeLink.bBOLink()

            '***********CODE MODIFIED FOR CLIENT SERVER MODEL Author: Ranjit
            m_lReturn = oBackOffice.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            '***************************************************************
            If Not (oBackOffice Is Nothing) Then
                r_sSiriusProduct = oBackOffice.Sirius_Product
                oBackOffice = Nothing
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSiriusProduct Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSiriusProduct", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function GetPerilTypeForRisk(ByVal v_lClaimId As Integer, ByVal v_lRisk As Integer, ByVal v_lPolicy As Integer, ByRef r_vResultArray(,) As Object) As Integer
        Return GetPerilTypeForRisk(v_lClaimId:=v_lClaimId, v_lRisk:=v_lRisk, v_lPolicy:=v_lPolicy, r_vResultArray:=r_vResultArray, bClaimsBuilder:=False)
    End Function

    Public Function GetPerilTypeForRisk(ByVal v_lClaimId As Integer, ByVal v_lRisk As Integer, ByVal v_lPolicy As Integer, ByRef r_vResultArray(,) As Object, ByVal bClaimsBuilder As Boolean) As Integer
        Dim result As Integer = 0
        Dim sSiriusProduct As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue


        r_vResultArray = Nothing

        m_lReturn = CType(GetSiriusProduct(sSiriusProduct), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="SiriusProduct", vValue:=sSiriusProduct, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'DC080102 changed from integer to long
        m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimId", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'DC080102 changed from integer to long
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk", vValue:=CStr(v_lRisk), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'DC080102 changed from integer to long
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Policy", vValue:=CStr(v_lPolicy), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If claimsbuilder is ON, then we want a complete list of perils, i.e. ignoring the fact that
        ' some peril types may already be in use for this claim. This means that more than one peril
        ' of the same type can be added when in "claimsbuilder" mode...
        If bClaimsBuilder Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ShowAll", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        Else
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ShowAll", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If



        r_vResultArray = Nothing
        ' Execute the stored procedure.
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPerilTypeForRiskSQL, sSQLName:=ACGetPerilTypeForRiskName, bStoredProcedure:=ACGetPerilTypeForRiskStored, vResultArray:=r_vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

        result = gPMConstants.PMEReturnCode.PMFalse
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPerilTypeForRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPerilTypeForRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result
    End Function

    'MSS250901 - Made v_lRiskID Optional for use in Broking
    Public Function AddClaimPeril(ByVal v_lClaimId As Integer, ByVal v_lPerilTypeID As Integer, ByRef r_lClaimPerilId As Integer, ByVal v_lRiskId As Integer, ByVal v_sDescription As String) As Integer
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim ACFirstRow, ACFirstColumn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimId", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="PerilTypeId", vValue:=CStr(v_lPerilTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'MSS250901 - Added for merge
            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="RiskId", vValue:=CStr(v_lRiskId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'MSS250901 - Merge end

            'CMG/PB 16092002 LossSchedule
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Description", vValue:=v_sDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACAddClaimPerilSQLU, sSQLName:=ACAddClaimPerilNameU, bStoredProcedure:=ACAddClaimPerilStoredU, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Zero based array

            r_lClaimPerilId = CInt(vResultArray(ACFirstRow, ACFirstColumn))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddClaimPeril  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddClaimPeril", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetRiskDetails(ByVal v_lRisk As Integer, ByVal v_lPolicyId As Integer, ByRef r_vDataArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim sSiriusProduct As String = ""
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetSiriusProduct(sSiriusProduct), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="SiriusProduct", vValue:=sSiriusProduct, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk", vValue:=CStr(v_lRisk), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Policy", vValue:=CStr(v_lPolicyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLSelect(ACGetRiskDetailsSQL, ACGetRiskDetailsName, ACGetRiskDetailsStored, , r_vDataArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetPerilForClaimRisk(ByVal v_lClaimId As Integer, ByVal v_lRiskTypeId As Integer, ByRef r_vDataArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim sSiriusProduct As String = ""
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetSiriusProduct(sSiriusProduct), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="SiriusProduct", vValue:=sSiriusProduct, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimId", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="RiskTypeId", vValue:=CStr(v_lRiskTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLSelect(ACGetPerilForClaimRiskSQL, ACGetPerilForClaimRiskName, ACGetPerilForClaimRiskStored, , r_vDataArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPerilForClaimRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPerilForClaimRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function CheckDeletionForPeril(ByVal v_lClaimPerilId As Integer, ByRef r_bCanDelete As Boolean) As Integer
        Dim result As Integer = 0
        Dim vDataArray(,) As Object = Nothing
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimPerilId", vValue:=CStr(v_lClaimPerilId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckDeletionForPerilSQL, sSQLName:=ACCheckDeletionForPerilName, bStoredProcedure:=ACCheckDeletionForPerilStored, vResultArray:=vDataArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_bCanDelete = CBool(vDataArray(0, 0))

            Return result

        Catch

            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Public Function DeletePeril(ByVal v_lClaimPerilId As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimPerilId", vValue:=CStr(v_lClaimPerilId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(ACDeletePerilSQL, ACDeletePerilName, ACDeletePerilStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch

            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function


    Public Function AddPerilForClaimRisk(ByVal v_lPolicyId As Integer, ByVal v_lRiskId As Integer, ByVal v_lClaimId As Object) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="PolicyID", vValue:=CStr(v_lPolicyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="RiskId", vValue:=CStr(v_lRiskId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC080102 changed from integer to long

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimId", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(ACAddPerilForClaimRiskSQL, ACAddPerilForClaimRiskName, ACAddPerilForClaimRiskStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPerilForClaimRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPerilForClaimRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function



    Public Function GetShowRiskDetails(ByVal v_lClaimId As Integer, ByRef r_vDataArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimId", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetShowRiskDetailsSQL, sSQLName:=ACGetShowRiskDetailsName, bStoredProcedure:=ACGetShowRiskDetailsStored, vResultArray:=r_vDataArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetShowRiskDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetShowRiskDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    Public Function GetPolicynumber(ByVal v_lEventCnt As Integer, ByRef r_vDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="EventCnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLSelect(ACGetPolicynumberSQL, ACGetPolicynumberName, ACGetPolicynumberStored, , r_vDataArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicynumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicynumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    'Start: Internal BugId -21 , Date- 21st Oct, Author DG :
    '                            Added a functionality. On pressing OK, in Risk Details screen,
    '                             when the Sum(CurrentReserve) = 0 then a message will come up
    '                             asking the User wether the Claim can be closed.
    '                             If the reply is YES the Claim status is set to closed.

    Public Function GetCurrentReserveRecovery(ByVal v_lClaimId As Integer, ByRef r_vDataArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLSelect(ACGetCurrentReserveRecoverySQL, ACGetCurrentReserveRecoveryName, ACGetCurrentReserveRecoveryStored, , r_vDataArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrentReserveRecovery Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentReserveRecovery", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    'End: Internal BugId -21 , Date- 21st Oct, Author DG

    'Start: Internal BugId -21 , Date- 21st Oct, Author DG
    '                            Added a functionality. On pressing OK, in Risk Details screen,
    '                             when the Sum(CurrentReserve) = 0 then a message will come up
    '                             asking the User wether the Claim can be closed.
    '                             If the reply is YES the Claim status is set to closed.
    Public Function CloseClaim(ByVal v_lClaimId As Integer) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(ACCloseClaimSQL, ACCloseClaimName, ACCloseClaimStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CloseClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CloseClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    'End: Internal BugId -21 , Date- 21st Oct, Author- DG

    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' Created By:  'RWH(13/11/2000) For Claims Numbering.
    '
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
    ' ***************************************************************** '
    Public Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Try


            Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnderwritingOrAgencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnderwritingOrAgency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*************************************************************************
    ' Name: GetRiskDetails_U
    '
    ' Desc: get revelant info to past on to iPMURisk
    '
    ' Hist : 12 April 2001 Created - Tinny
    '*************************************************************************
    Public Function GetRiskDetails_U(ByVal v_lClaimId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACSelRiskDetailSQL, sSQLName:=ACSelRiskDetailName, bStoredProcedure:=ACSelRiskDetailStored, vResultArray:=r_vResultArray)

            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskDetails_U Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDetails_U", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    '***********************************************************************
    ' Name : GetOriginalClaimID
    '
    ' Desc : get the original claim ID from  table
    '
    ' Hist : 15 June 2001 Tinny - Created
    '***********************************************************************
    Public Function GetOriginalClaimID(ByVal v_lClaimId As Integer, ByRef r_lOriginalClaimID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetOriginalClaimIDSQL, sSQLName:=ACGetOriginalClaimIDName, bStoredProcedure:=True, vResultArray:=vResultArray)


            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            r_lOriginalClaimID = gPMFunctions.ToSafeLong(vResultArray(0, 0), 0)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOriginalClaimID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOriginalClaimID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetInfoOnlyStatus
    '
    ' Description:  Find out if Claim was previously Info Only
    '
    ' Created By:  Jude Killip 25/05/2001
    '
    ' ***************************************************************** '
    Public Function GetInfoOnlyStatus(ByVal v_lClaim_Id As Integer, ByRef r_bInfoStatus As Boolean) As Integer

        Dim result As Integer = 0
        Dim l_vResultArray(,) As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(v_lClaim_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInfoOnlyStatus")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInfoOnlyStatusSQL, sSQLName:=ACGetInfoOnlyStatusName, bStoredProcedure:=ACGetInfoOnlyStatusStored, vResultArray:=l_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInfoOnlyStatus")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'DB doesn't alwas return 811 when no data is found
            'either PMTrue or PMNotFound
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                r_bInfoStatus = False
            Else
                If Informations.IsArray(l_vResultArray) Then

                    r_bInfoStatus = CBool(l_vResultArray(0, 0))
                Else
                    r_bInfoStatus = False
                End If
            End If


            Return m_lReturn

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInfoOnlyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientAndPolicyID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '' ***************************************************************** '
    '' Name: GetClaimDetail
    ''
    '' Description:  get claim default details for event
    ''
    '' Created By:  TN 24/08/2001
    ''
    '' ***************************************************************** '
    'Public Function GetClaimDetail(ByVal v_lClaimId As Long, _
    ''                                ByRef r_vResultArray As Variant)
    '
    'Dim sSQL As String
    '
    '    On Error GoTo Err_GetClaimDetail
    '
    '    GetClaimDetail = PMTrue
    '
    '    m_oDatabase.Parameters.Clear
    '
    '    GetClaimDetail = m_oDatabase.Parameters.Add(sName:="claim_id", _
    ''                                                vValue:=v_lClaimId, _
    ''                                                idirection:=PMParamInput, _
    ''                                                iDataType:=PMLong)
    '
    '    If GetClaimDetail <> PMTrue Then
    '        Exit Function
    '    End If
    '
    '    GetClaimDetail = m_oDatabase.SQLSelect(sSQL:=kGetClaimDetailSQL, _
    ''                                    sSQLName:=kGetClaimDetailName, _
    ''                                    bStoredProcedure:=True, _
    ''                                    vResultArray:=r_vResultArray)
    '
    '    If GetClaimDetail <> PMTrue Then
    '        Exit Function
    '    End If
    '
    '    If Not IsArray(r_vResultArray) Then
    '        GetClaimDetail = PMNotFound
    '        Exit Function
    '    End If
    '
    '    Exit Function
    '
    'Err_GetClaimDetail:
    '
    '    GetClaimDetail = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="GetClaimDetail Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetClaimDetail", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    '    Resume
    '
    'End Function
    ' ***************************************************************** '
    ' Name: GetClientPolicyDetails
    '
    ' Description:
    '
    ' History: 02/10/2002 sj - Created.
    ' RAM20021021 : Added Party Short Name Parameter
    '               Note : Updated spu_get_client_policy_details stored Procedure too
    '               (NRMA Changes - Sirius Process No 126)
    ' ***************************************************************** '
    Public Function GetClientPolicyDetails(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lPartyCnt As Integer) As Integer
        Return GetClientPolicyDetails(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lPartyCnt:=r_lPartyCnt, r_sPartyShortName:="", r_lInsuranceFolderCnt:=0, r_sInsuranceRef:="")
    End Function

    Public Function GetClientPolicyDetails(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lPartyCnt As Integer, ByRef r_sPartyShortName As String, ByRef r_lInsuranceFolderCnt As Integer, ByRef r_sInsuranceRef As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object = Nothing

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for insurance_file_cnt " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientPolicyDetails")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClientPolicyDetailsSQL, sSQLName:=ACGetClientPolicyDetailsName, bStoredProcedure:=ACGetClientPolicyDetailsStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLSelect Failed calling " & ACGetClientPolicyDetailsSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientPolicyDetails")
                Return result
            End If


            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lPartyCnt = CInt(vResultArray(0, 0))

            r_sPartyShortName = CStr(vResultArray(1, 0)) ' RAM20021021 : Added Party Short Name too

            r_lInsuranceFolderCnt = CInt(vResultArray(2, 0))

            r_sInsuranceRef = CStr(vResultArray(3, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientPolicyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************************
    ' Name: GetGISScreenID
    '
    ' Description: Get the GIS screen ID from the  claim table
    '
    ' History: PW140703 - Created (PS68 - Date Effective rating and rules)
    '********************************************************************************
    Public Function GetGISScreenID(ByVal lClaimID As Integer, ByRef r_lScreenID As Integer, ByVal bPerilLevel As Boolean) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISScreenID")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add the Peril Level parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="peril_level", vValue:=CStr(Math.Abs(CInt(bPerilLevel))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveGISScreenID")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetGISScreenIDSQL, sSQLName:=ACGetGISScreenIDName, bStoredProcedure:=ACGetGISScreenIDStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISScreenID")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the return value
            If Informations.IsArray(vArray) Then

                r_lScreenID = CInt(Val(CStr(vArray(0, 0))))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGISScreenID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISScreenID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************************
    ' Name: SaveGISScreenID
    '
    ' Description: Save the GIS screen ID to the  claim table
    '
    ' History: PW140703 - Created (PS68 - Date Effective rating and rules)
    '********************************************************************************
    Public Function SaveGISScreenID(ByVal lClaimID As Integer, ByVal lScreenID As Integer, ByVal bPerilLevel As Boolean) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveGISScreenID")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add the screen ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="screen_id", vValue:=CStr(lScreenID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveGISScreenID")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add the Peril Level parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="peril_level", vValue:=CStr(Math.Abs(CInt(bPerilLevel))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveGISScreenID")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSaveGISScreenIDSQL, sSQLName:=ACSaveGISScreenIDName, bStoredProcedure:=ACSaveGISScreenIDStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLAction failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveGISScreenID")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveGISScreenID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveGISScreenID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'MSS250901 - Added following functions for merge

    'AK - 190401
    'Function to return Policy Type for a given Policy

    Public Function GetPolicyType(ByVal v_lPolicyId As Integer, ByRef r_sType As String) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing


        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Risk Code Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_id", vValue:=CStr(v_lPolicyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyType")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyTypeSQL, sSQLName:=ACGetPolicyTypeName, bStoredProcedure:=ACGetPolicyTypeStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyType")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then

                r_sType = CStr(vArray(0, 0)).Trim()
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    'AK - 07082001 - Function to get the list of drivers for Gemini II Motor
    Public Function GetDrivers(ByVal v_lInsurance_File_Cnt As Integer, ByRef r_vDrivers(,) As Object) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Risk Code Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsurance_File_Cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDrivers")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDriversSQL, sSQLName:=ACGetDriversName, bStoredProcedure:=ACGetDriversStored, vResultArray:=r_vDrivers)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDrivers")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(r_vDrivers) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDrivers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDrivers", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    'DC130701 -start -to reclose a claim
    'DC130701 was iClaimId now lClaimId
    Public Function ReCloseClaim(ByVal v_lClaimId As Integer) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            'DC130701 was iClaimId now lClaimId
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(ACReCloseClaimSQL, ACReCloseClaimName, ACReCloseClaimStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReCloseClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReCloseClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    'DC130701 -end

    'DC130701 -start -to reopen a claim
    'DC130701 was iClaimId now lClaimId
    Public Function ReOpenClaim(ByVal v_lClaimId As Integer) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            'DC130701 was iClaimId now lClaimId
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(ACReOpenClaimSQL, ACReOpenClaimName, ACReOpenClaimStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReOpenClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReOpenClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    'DC130701 -end


    ' ***************************************************************** '
    ' Name: GetRiskTypeScreenID
    ' Description:
    ' History: 21/01/2003 - Alix
    ' ***************************************************************** '
    Public Function GetRiskTypeScreenID(ByVal v_lRiskTypeId As Integer, ByRef r_lScreenID As Integer) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Risk Code Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_lRiskTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTypeScreenID")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskTypeScreenIDSQL, sSQLName:=ACGetRiskTypeScreenIDName, bStoredProcedure:=ACGetRiskTypeScreenIDStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTypeScreenID")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then

                If CStr(vArray(0, 0)) <> "" Then

                    r_lScreenID = CInt(CStr(gPMFunctions.NullToLong(CStr(vArray(0, 0)))).Trim())
                Else
                    r_lScreenID = 0
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTypeScreenID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name:         GetProgressStatusCode
    '
    ' Description:  Returns the Code for a given Progress_status
    '
    ' History:      01/04/2003 Kevin Renshaw (CMG) - Created.
    ' ***************************************************************** '
    Public Function GetProgressStatusCode(ByVal iProgressStatus As Integer, ByRef sCode As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="Progress_status_id", vValue:=CStr(iProgressStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetProgressStatusCodeSQL, sSQLName:=ACGetProgressStatusCodeName, bStoredProcedure:=ACGetProgressStatusCodeStored, vResultArray:=vResultArray)


            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            sCode = CStr(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProgressStatusCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProgressStatusCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************************************
    ' Name: SetClaimStatus
    ' Desc: set claim status to v_lClaimStatusID on either _Claim or Claim table
    '********************************************************************************************
    'Public Function SetClaimStatus(ByVal v_lClaimId As Long, _
    ''                                Optional ByVal v_bIsClaim As Boolean = False, _
    ''                                Optional ByVal v_lClaimStatusID As Long = CLMClosed) As Long
    '
    'Dim sSQL As String
    'Dim sTable As String
    'Dim sMessage As String
    '
    '    On Error GoTo Catch_Error
    '    SetClaimStatus = PMTrue
    '
    '    If v_bIsClaim Then
    '        sTable = "_Claim"
    '    Else
    '        sTable = "Claim"
    '    End If
    '
    '    sSQL = "UPDATE " & sTable & vbCrLf
    '    sSQL = sSQL & "SET Claim_Status_ID = {ClaimStatusID}," & vbCrLf
    '    sSQL = sSQL & "progress_status_id = ISNULL((SELECT progress_status_id FROM Progress_Status WHERE code = 'CLOSED'),progress_status_id)" & vbCrLf
    '    sSQL = sSQL & "WHERE Claim_ID = {ClaimID}"
    '
    '    m_oDatabase.Parameters.Clear
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimStatusID", _
    ''                                        vValue:=v_lClaimStatusID, _
    ''                                        idirection:=PMParamInput, _
    ''                                        iDataType:=PMLong)
    '
    '    If m_lReturn <> PMTrue Then
    '        sMessage = "Failed to add ClaimStatusID param"
    '        GoTo Catch_Error
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", _
    ''                                        vValue:=v_lClaimId, _
    ''                                        idirection:=PMParamInput, _
    ''                                        iDataType:=PMLong)
    '
    '    If m_lReturn <> PMTrue Then
    '        sMessage = "Failed to add ClaimID param"
    '        GoTo Catch_Error
    '    End If
    '
    '    m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, _
    ''                                    sSQLName:="Update Claim Status", _
    ''                                    bStoredProcedure:=False)
    '
    '    If m_lReturn <> PMTrue Then
    '        sMessage = "Failed to set claim status to - " & v_lClaimStatusID & " For claim id - " & v_lClaimId
    '        GoTo Catch_Error
    '    End If
    '
    '    Exit Function
    '
    'Catch_Error:
    '
    '    SetClaimStatus = PMError
    '
    '    If sMessage = "" Then
    '        sMessage = "Failed to update claim status"
    '    End If
    '
    '    LogMessage m_sUsername, _
    ''         iType:=PMLogOnError, _
    ''         sMsg:=sMessage, _
    ''         vApp:=ACApp, _
    ''         vClass:=ACClass, _
    ''         vMethod:="SetClaimStatus", _
    ''         vErrNo:=Err.Number, _
    ''         vErrDesc:=Err.Description
    '
    'End Function

    ' ***************************************************************** '
    ' Name: GetRiskType
    '
    ' Parameters: n/a
    '
    ' Description: Returns the Risk Type Id for a specified  claim id
    '
    ' History:
    '           Created : MEvans : 28-05-2003 : 223
    ' ***************************************************************** '
    Public Function GetRiskType(ByRef r_vResults(,) As Object, ByVal v_lRiskId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetRiskType"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            ' Risk Id
            m_lReturn = CType(AddInputParameter(v_sName:="RiskId", v_vValue:=v_lRiskId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute Select Query
            If m_oDatabase.SQLSelect(sSQL:=ACGetRiskTypeSQL, sSQLName:=ACGetRiskTypeName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lRiskId", v_lRiskId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description), oDicParms:=oDict)
                '******************************

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lRiskId", v_lRiskId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddInputParameter"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Parameter to database object

            lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, " Failed to add parameter name:" & v_sName &
                                        ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimDataModel
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 28-05-2003 : 223
    ' ***************************************************************** '
    Function GetClaimDataModel(ByVal v_lClaimId As Integer, ByRef r_oDataSet As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetClaimDataModel"

        Dim oGIS As Object

        Dim sXMLDataSetDef As String = String.Empty
        Dim sXMLDataset As String = String.Empty
        Dim sGisDataModelCode As String = String.Empty
        Dim bLoadedEngineObject As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' initialisation
            bLoadedEngineObject = False

            ' Get an instance of bGIS.Application
            'oGIS = New bGIS.Application
            oGIS = Nothing
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=oGIS, v_sClassName:="bGIS.Application", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim r_sMessage As String = "Failed to create an instance of bGIS.Application"
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bGIS.Application", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If oGIS.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database)) = gPMConstants.PMEReturnCode.PMTrue Then

                ' load strings from claim

                If oGIS.LoadClaimFromDB(ToSafeString(sXMLDataSetDef), ToSafeString(sXMLDataset), ToSafeString(sGisDataModelCode), ToSafeInteger(v_lClaimId)) = gPMConstants.PMEReturnCode.PMTrue Then

                    ' load the dataset object

                    If r_oDataSet.LoadFromXML(ToSafeString(sXMLDataSetDef), ToSafeString(sXMLDataset)) = gPMConstants.PMEReturnCode.PMTrue Then

                        bLoadedEngineObject = True
                    End If
                Else

                    ' log error and fail
                    result = gPMConstants.PMEReturnCode.PMFalse

                    LogMsg(v_sMsg:="Failed to load claim from db for claim id:" & v_lClaimId, v_sMethod:=sFunctionName)

                End If
            Else

                ' log error and fail
                result = gPMConstants.PMEReturnCode.PMFalse

                LogMsg(v_sMsg:="Failed to create bGIS.Application", v_sMethod:=sFunctionName)

            End If

            If Not bLoadedEngineObject Then

                ' log error and fail
                result = gPMConstants.PMEReturnCode.PMFalse

                ' log failed to setup engine object
                LogMsg(v_sMsg:="Failed to setup engine object", v_sMethod:=sFunctionName)

            End If

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '******************************


        Finally
            ' destroy object reference
        End Try



        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GetClaimGisData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 18-06-2003 : 223
    ' ***************************************************************** '

    'Private Function GetClaimGisData(ByRef r_vResults(,) As Object, ByVal v_lClaimId As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Const sFunctionName As String = "GetClaimGisData"
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Clear Down Database Parameters
    'm_oDatabase.Parameters.Clear()
    '
    ' Add Required Stored Procedure Parameters
    'm_lReturn = CType(AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
    '
    ' Execute selection Query
    'If m_oDatabase.SQLSelect(sSQL:=ACGetClaimGisDataSQL, sSQLName:=ACGetClaimGisDataName, bStoredProcedure:=True, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    '******************************
    ' Log Error.
    'gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to retrieve the gis data for claim: " & CStr(v_lClaimId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
    '******************************
    '
    'End If
    '
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    '******************************
    ' Log Error.
    'gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
    '******************************
    '
    'Return result
    '
    'End Try
    'End Function
    ' ***************************************************************** '
    ' Name: DeleteGISDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 07-01-2004 : CQ3414
    ' ***************************************************************** '
    Public Function DeleteGISDetails(ByVal v_lClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "DeleteGISDetails"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_Id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=ACDeleteGISDetailsSQL, sSQLName:=ACDeleteGISDetailsName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lClaimId", v_lClaimId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to delete gis dataset and gis policy link for  claim:" & CStr(v_lClaimId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lClaimId", v_lClaimId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result

        End Try
    End Function

    Private Sub LogMsg(ByVal v_sMsg As String, ByVal v_sMethod As String)

        gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=v_sMethod & ":" & v_sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=v_sMethod)

    End Sub

    ' ***************************************************************** '
    ' Name:         TidyUpAfterCancel
    '
    ' Parameters:   v_lClaimId      -  claim id
    '               v_lClaimMode        - (optional) claim mode
    '
    ' Description:  When cancelling from a claim roadmap, the  table
    '               data needs to be deleted (for underwriting) and with
    '               claimsbuilder, additional GIS-related data will also
    '               need to be deleted...
    '
    ' History:
    '               Created : RVH   23/12/2004
    ' ***************************************************************** '

    Public Function TidyUpAfterCancel(ByVal v_lClaimId As Integer) As Integer
        Return TidyUpAfterCancel(v_lClaimId:=v_lClaimId, v_lClaimMode:=-1)
    End Function
    Public Function TidyUpAfterCancel(ByVal v_lClaimId As Integer, ByVal v_lClaimMode As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "TidyUpAfterCancel"

        Try

            Dim lOriginalClaimId As Integer
            Dim r_bClaimsBuilderEnabled As Boolean

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If claim mode not supplied, use m_iTask
            If v_lClaimMode = -1 Then
                v_lClaimMode = m_iTask
            End If

            ' Only interested in deleting  data for underwriting

            If m_sTransactionType <> "C_CO" Then
                ' Get the original claim id for the passed  claim id
                m_lReturn = CType(GetOriginalClaimID(v_lClaimId:=v_lClaimId, r_lOriginalClaimID:=lOriginalClaimId), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_lClaimId", v_lClaimId)
                    gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed - While trying to retrieve original claim id for: " & CStr(v_lClaimId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Don't bother trying to unlock if opening a claim...or claim mode set to "view" (the claim won't be locked)
                If v_lClaimMode <> gPMConstants.PMEComponentAction.PMView Then
                    m_lReturn = CType(UnlockClaim(v_lOriginalClaimId:=lOriginalClaimId), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            ' Get the claimsbuilder product option
            m_lReturn = CType(IsClaimsbuilderEnabled(r_bClaimsBuilderEnabled:=r_bClaimsBuilderEnabled), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lClaimId", v_lClaimId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed - While trying to determine if claimsbuilder is enabled", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Only want to deal with GIS data if claimsbuilder is on
            If r_bClaimsBuilderEnabled Then
                ' NB: The  GIS Details must be deleted prior to the
                '  claim being deleted
                If DeleteGISDetails(v_lClaimId:=v_lClaimId) <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' It shouldnt fail the tidy up process as  versions can
                    ' always be cleared up at a later date but is logged to help
                    ' with later clearup....
                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_lClaimId", v_lClaimId)
                    gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed - While trying to delete GIS  claim details for claim id: " & CStr(v_lClaimId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                End If
            End If

            ' Set passed  claim id as current claim id for the purposes of the delete
            m_lClaimID = v_lClaimId

            ' Delete the  claim data
            m_lReturn = DeleteClaim()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lClaimId", v_lClaimId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed - While trying to delete  claim details for claim id: " & CStr(v_lClaimId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                Return result
            End If

            '            '   RVH 17/09/2003 START : Delete  table data for event log and tasks
            '            m_lReturn = oBusiness.DeleteTaskAndEvent(m_lclaimid, 1)
            '
            '            If (m_lReturn <> PMTrue) Then
            '                TidyUpAfterCancel = PMFalse
            '                Exit Function
            '            End If
            '            '   RVH 17/09/2003 END


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lClaimId", v_lClaimId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         IsClaimsbuilderEnabled
    '
    ' Parameters:   N/A
    '
    ' Description:  Get product option for claimsbuilder
    '
    ' History:
    '               Created : RVH   23/12/2004
    ' ***************************************************************** '
    Private Function IsClaimsbuilderEnabled(ByRef r_bClaimsBuilderEnabled As Boolean) As Integer

        Dim result As Integer = 0

        Dim vResult As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue
        'developer guide no.98
        m_lReturn = CType(bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTClaimsBuilder, v_vBranch:=m_iSourceID, r_vUnderwriting:=vResult), gPMConstants.PMEReturnCode)

        r_bClaimsBuilderEnabled = gPMFunctions.ToSafeInteger(vResult) = 1

        Return result

    End Function

    ' ***************************************************************** '
    ' Name:         UnlockClaim
    '
    ' Parameters:   v_lClaimId      -  claim id
    '
    ' Description:  Unlock the claim
    '
    ' History:
    '               Created : RVH   23/12/2004
    ' ***************************************************************** '
    Private Function UnlockClaim(ByVal v_lOriginalClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "UnlockClaim"

        Dim oPMLock As bpmlock.User



        result = gPMConstants.PMEReturnCode.PMTrue

        'Get bPMLock
        oPMLock = New bpmlock.User
        m_lReturn = CType(oPMLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to create the business component
            result = gPMConstants.PMEReturnCode.PMFalse
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lOriginalClaimId", v_lOriginalClaimId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to get PMLock business object", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description), oDicParms:=oDict)

            Return result
        End If


        m_lReturn = oPMLock.UnLockKey(sKeyName:="claim_id", vKeyValue:=v_lOriginalClaimId, iUserID:=m_iUserID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unlock claim
            result = gPMConstants.PMEReturnCode.PMFalse
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lOriginalClaimId", v_lOriginalClaimId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to unlock claim for claim id: " & CStr(v_lOriginalClaimId), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description), oDicParms:=oDict)

            Return result
        End If

        oPMLock = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetGisPolicyLinkDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' ***************************************************************** '
    Public Function GetGisPolicyLinkDetails(ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetGisPolicyLinkDetails"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=ACGetGisPolicyLinkDetailsSQL, sSQLName:=ACGetGisPolicyLinkDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lClaimId", v_lClaimId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                '******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lClaimId", v_lClaimId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result

        End Try
    End Function

    Public Function BalanceClaim(ByVal v_lClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "BalanceClaim"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add parameters
            AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="user_id", v_vValue:=m_iUserID, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="user_name", v_vValue:=m_sUsername, v_iType:=gPMConstants.PMEDataType.PMString)

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(ACBalanceReserveSQL, ACBalanceReserveName, True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to execute procedure to balance reserves")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function CreateWorkTask(ByVal v_sTaskCode As String, ByVal v_sDescription As String, ByRef r_vKeyArray(,) As Object, ByVal v_lUserGroupID As Integer, ByVal v_lUserID As Object) As Integer
        Dim result As Integer = 0
        Dim oRoadmap As bSIRRoadmap.Business
        Dim lPartyCnt As Integer
        Dim iTaskDaysDue As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of bSIRRoadmap
            oRoadmap = New bSIRRoadmap.Business
            m_lReturn = CType(oRoadmap.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Default to no party
            lPartyCnt = 0

            'Default TaskDaysDue to 7
            iTaskDaysDue = 7

            ' Get the party count & TaskDaysDue
            If Informations.IsArray(r_vKeyArray) Then
                For iLoop1 As Integer = 0 To r_vKeyArray.GetUpperBound(1)

                    If CStr(r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)) = "party_cnt" Then

                        lPartyCnt = CInt(r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    ElseIf (CStr(r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)) = "TaskDaysDue") Then

                        iTaskDaysDue = CInt(r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                        'Default TaskDueDate Key to 7 for future steps

                        r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1) = "7"
                    End If
                Next iLoop1


            End If

            ' Remove any empty keys from the task
            m_lReturn = CType(RemoveBlankKeys(r_vKeyArray:=r_vKeyArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call Roadmap to create the task - Default to 7 days?

            m_lReturn = oRoadmap.CreateWorkManagerTask(v_lPartyCnt:=lPartyCnt, v_sDescription:=v_sDescription, v_sTask:=v_sTaskCode, v_lNumDays:=iTaskDaysDue, v_vKeyArray:=r_vKeyArray, v_lUserGroupID:=v_lUserGroupID, v_lUserID:=v_lUserID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear up

            oRoadmap.Dispose()


            oRoadmap = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateWorkTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateWorkTask", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    Public Function RemoveBlankKeys(ByRef r_vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vNewArray(,) As Object
        Dim iIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Do we have something to do?
            If Not Informations.IsArray(r_vKeyArray) Then
                Return result
            End If

            vNewArray = Nothing

            ' Loop the array
            For iLoop1 As Integer = 0 To r_vKeyArray.GetUpperBound(1)

                If CStr(r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)) <> "" Then

                    ' Prep the array
                    If Informations.IsArray(vNewArray) Then

                        iIndex = vNewArray.GetUpperBound(1) + 1
                        ReDim Preserve vNewArray(1, iIndex)
                    Else
                        iIndex = 0
                        ReDim vNewArray(1, iIndex)
                    End If

                    ' Store it


                    vNewArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iIndex) = r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)


                    vNewArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iIndex) = r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)

                End If
            Next iLoop1


            r_vKeyArray = vNewArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveBlankKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveBlankKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    Public Function GetClaimPerilID(ByVal v_lClaimId As Integer, ByRef r_vDataArray(,) As Object) As Integer


        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            'DC080102 changed from integer to long
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLSelect(ACGetClaimPerilIDSQL, ACGetClaimPerilIDName, True, , r_vDataArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimPerilID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimPerilID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Public Function GetClassOfBusiness(ByRef r_lId As Integer, ByRef r_sCode As String, ByVal v_lPerilTypeID As Integer, ByVal v_lClaimPerilId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClassOfBusiness"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResults(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="peril_type_id", v_vValue:=v_lPerilTypeID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            m_lReturn = CType(AddInputParameter(v_sName:="claim_peril_id", v_vValue:=v_lClaimPerilId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetClassOfBusinessSQL, sSQLName:=kGetClassOfBusinessName, bStoredProcedure:=True, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetClassOfBusinessSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            If Informations.IsArray(vResults) Then

                r_lId = CInt(vResults(0, 0))

                r_sCode = CStr(vResults(1, 0))
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: CreateEvent
    ' Desc: Adds an entry into the Event Log
    '
    ' Hist: 10/01/2006 - A.Robinson : Function Created.

    ' ***************************************************************** '
    Public Function CreateEvent(ByVal v_lEventTypeId As Integer, ByVal v_sDescription As String, Optional ByRef v_lEventCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Const AC_PROCEDURE_NAME As String = "CreateEvent"

        Dim oEvent As New bSIREvent.Business
        Dim sSQL As String = ""
        Dim lInsuranceFileCnt, lInsuranceFolderCnt, lPartyCnt As Integer
        Dim vResults(,) As Object = Nothing

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT C.policy_id FROM claim C WHERE C.claim_id = " & m_lClaimID

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Policy Id for Claim", bStoredProcedure:=False, vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(AC_PROCEDURE_NAME, "Failed to get Policy Id for Claim", gPMConstants.PMELogLevel.PMLogError)
            End If

            lInsuranceFileCnt = gPMFunctions.ToSafeLong(vResults(0, 0))

            m_lReturn = CType(GetClientPolicyDetails(v_lInsuranceFileCnt:=lInsuranceFileCnt, r_lPartyCnt:=lPartyCnt, r_sPartyShortName:="", r_lInsuranceFolderCnt:=lInsuranceFolderCnt, r_sInsuranceRef:=""), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(AC_PROCEDURE_NAME, "Failed to get Client details for Policy", gPMConstants.PMELogLevel.PMLogError)
            End If
            oEvent = New bSIREvent.Business
            m_lReturn = CType(oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(AC_PROCEDURE_NAME, "Failed to create business object bSIREvent.Business", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oEvent.DirectAdd(vPartyCnt:=lPartyCnt, vInsuranceFolderCnt:=lInsuranceFolderCnt, vInsuranceFileCnt:=lInsuranceFileCnt, vClaimCnt:=m_lClaimID, vEventType:=v_lEventTypeId, vUserId:=m_iUserID, vEventDate:=DateTime.Today, vDescription:=v_sDescription, vPerilId:=m_lPerilID, vEventCnt:=v_lEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(AC_PROCEDURE_NAME, "Failed to add event to Event Log", gPMConstants.PMELogLevel.PMLogError)
            End If


            Return result

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=AC_PROCEDURE_NAME & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=AC_PROCEDURE_NAME, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

            If Not (oEvent Is Nothing) Then

                oEvent.Dispose()
                oEvent = Nothing
            End If


        End Try
        Return result
    End Function

    Public Function CreateReserveLimitFailureTasknEvent(ByVal v_vKeyArray As Object, ByVal v_dExceededReserve As Decimal) As Integer
        Dim result As Integer = 0
        Dim sTaskDesc As String
        Dim lEventCnt As Integer
        Dim lTaskInstanceCnt As Integer
        Const kEventTypeNewClaim As Integer = 3
        Const kEventTypeMaintainClaim As Integer = 6

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oOpenClaim Is Nothing Then
                m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oOpenClaim, v_sClassName:="bOpenClaim.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(ACClass, "Failed to create business object bSIREvent.Business", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            sTaskDesc = "Claim Reserve Limit has been exceeded for this User '" & m_sUsername & "' and Reserve = " & v_dExceededReserve & "."

            m_lReturn = CreateEvent(v_lEventTypeId:=IIf(m_sTransactionType = "C_CO", kEventTypeNewClaim, kEventTypeMaintainClaim),
                                    v_sDescription:=sTaskDesc, v_lEventCnt:=lEventCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oOpenClaim.AddClaimLink(v_lClaimId:=ToSafeInteger(m_lClaimID), v_lLinkTypeId:=gPMConstants.kWorkClaimLinkTypeEvent, v_lLinkId:=ToSafeInteger(lEventCnt))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = AddWorkmanagerTask(v_sTaskDesc:=sTaskDesc, v_vKeyArray:=v_vKeyArray, v_lTaskInstanceCnt:=lTaskInstanceCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oOpenClaim.AddClaimLink(v_lClaimId:=ToSafeInteger(m_lClaimID), v_lLinkTypeId:=gPMConstants.kWorkClaimLinkTypeTask, v_lLinkId:=ToSafeInteger(lTaskInstanceCnt))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create Reserve Limit Failure Task and Event", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateUserLimitFailureTasknEvent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        Finally
            If Not (m_oOpenClaim Is Nothing) Then
                m_oOpenClaim.Dispose()
                m_oOpenClaim = Nothing
            End If
        End Try

        Return result

    End Function

    Private Function AddWorkmanagerTask(ByVal v_sTaskDesc As String, ByVal v_vKeyArray As Object, ByRef v_lTaskInstanceCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim sTaskCode As String = String.Empty
        Dim sUserGroupCode As String = String.Empty
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oAuthorisePayments Is Nothing Then
                m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oAuthorisePayments, v_sClassName:="bCLMAuthorisePayments.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(ACClass, "Failed to create business object bSIREvent.Business", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            m_lReturn = m_oAuthorisePayments.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_sTransactionType = "C_CO" Then
                sTaskCode = "OPENCLM"
            ElseIf m_sTransactionType = "C_CR" Then
                sTaskCode = "MAINCLM"
            End If

            m_lReturn = GetSupervisorUserGroup(sUserGroupCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oAuthorisePayments.AddTaskToWorkManager(r_lPMWrkTaskInstanceCnt:=v_lTaskInstanceCnt, v_sCustomer:="SYSTEM", v_sDescription:=v_sTaskDesc, v_dtTaskDueDate:=DateTime.Now,
                                                                  v_sTaskCode:=sTaskCode, v_sTaskGroupCode:="CLAIMSUPER", v_sUserGroupCode:=sUserGroupCode, v_vKeyArray:=v_vKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create Reserve Limit Failure Work Manager Task", vApp:=ACApp, vClass:=ACClass, vMethod:="AddFailureWorkmanagerTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        Finally
            If Not (m_oAuthorisePayments Is Nothing) Then
                m_oAuthorisePayments.Dispose()
                m_oAuthorisePayments = Nothing
            End If
        End Try

        Return result
    End Function

    Private Function GetSupervisorUserGroup(ByRef sGroupCode As String) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = String.Empty
        Dim vResults(,) As Object = Nothing
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT TOP 1 URL.user_group_code FROM claim_reserve_limit URL where URL.User_Name  = {user_name} order by URL.effective_date desc"

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_name", vValue:=m_sUsername, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Supervisor user group code for current user.", bStoredProcedure:=False, vResultArray:=vResults)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            sGroupCode = gPMFunctions.ToSafeString(vResults(0, 0))

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve the supervisor user group.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSupervisorUserGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        End Try
        Return result
    End Function
End Class

