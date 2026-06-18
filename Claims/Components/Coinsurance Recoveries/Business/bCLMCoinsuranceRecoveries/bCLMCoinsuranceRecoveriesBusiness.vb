Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Globalization
'Developer Guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: {TodaysDate}
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a CLMCoinsuranceRecoveries.
    '
    ' Edit History:
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 11/12/2003
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

    ' Collection of CLMCoinsuranceRecoveriess (Private)
    Private m_oCLMCoinsuranceRecoveriess As bCLMCoinsuranceRecoveries.Recoveriess

    ' Instance of Data component
    Private m_dCLMCoinsuranceRecoveries As dCLMCoinsuranceRecoveries.Data

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As gPMConstants.PMEReturnCode

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    ' Primary Keys to work with
    Private m_lPartyID As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lClaimID As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private lPMAuthorityLevel As Integer

    Private m_sUnderwritingOrAgency As String = ""

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
                    m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse
                Case Is > m_oCLMCoinsuranceRecoveriess.Count()
                    m_lCurrentRecord = m_oCLMCoinsuranceRecoveriess.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oCLMCoinsuranceRecoveriess.Count()

        End Get
    End Property



    Public Property InsuranceFileCnt() As Integer
        Get

            Return m_lInsuranceFileCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileCnt = Value

        End Set
    End Property


    Public Property ClaimID() As Integer
        Get

            Return m_lClaimID

        End Get
        Set(ByVal Value As Integer)

            m_lClaimID = Value

        End Set
    End Property



    Public Property PartyID() As Integer
        Get

            Return m_lPartyID

        End Get
        Set(ByVal Value As Integer)

            m_lPartyID = Value

        End Set
    End Property

    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property

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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create CLMReinsuranceRecoveriess Collection
            m_oCLMCoinsuranceRecoveriess = New bCLMCoinsuranceRecoveries.Recoveriess()

            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Create object for PMDAO
            '    Set m_oDatabase = New dPMDAO.Database

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            ' Create instance of data class
            m_dCLMCoinsuranceRecoveries = New dCLMCoinsuranceRecoveries.Data()

            m_lReturn = m_dCLMCoinsuranceRecoveries.Initialise(v_sUsername:=sUsername, v_sPassword:=sPassword, v_iUserID:=iUserID, v_iSourceID:=iSourceID, v_iLanguageID:=iLanguageID, v_iCurrencyID:=iCurrencyID, v_iLogLevel:=iLogLevel, v_sCallingAppName:=sCallingAppName, v_vDatabase:=m_oDatabase)


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
                    m_oLookup = Nothing
                End If
                m_oCLMCoinsuranceRecoveriess = Nothing
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
                m_dCLMCoinsuranceRecoveries.Task = m_iTask
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
            m_dCLMCoinsuranceRecoveries.InsuranceFileCnt = m_lInsuranceFileCnt
            m_dCLMCoinsuranceRecoveries.ClaimID = m_lClaimID


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required CLMCoinsuranceRecoveriess and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vPartyID As Object = Nothing, Optional ByRef vPartyName As Object = Nothing, Optional ByRef vShare As Object = Nothing, Optional ByRef vShareValue As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oCLMCoinsuranceRecoveries As bCLMCoinsuranceRecoveries.Recoveries
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oCLMCoinsuranceRecoveriess.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord = CType(m_lCurrentRecord + 1, gPMConstants.PMEReturnCode)
            Else
                'RWH(05/03/2001) If we don't exit here then we get an extra record
                'for the last party.
                Return gPMConstants.PMEReturnCode.PMEOF
            End If

            oCLMCoinsuranceRecoveries = m_oCLMCoinsuranceRecoveriess.Item(m_lCurrentRecord)

            ' Get the CLMCoinsuranceRecoveries Property Values




            'developer guide no.98
            m_lReturn = oCLMCoinsuranceRecoveries.GetProperties(iStatus, vPartyID:=vPartyID, vPartyName:=vPartyName, vShare:=vShare, vShareValue:=vShareValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oCLMCoinsuranceRecoveries = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            For lSub As Integer = 1 To m_oCLMCoinsuranceRecoveriess.Count()
                Select Case m_oCLMCoinsuranceRecoveriess.Item(lSub).DatabaseStatus
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
        'Dim ret As Long
        Dim oCLMCoinsuranceRecoveries As bCLMCoinsuranceRecoveries.Recoveries
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection
            'ret = MsgBox(m_oCLMCoinsuranceRecoveriess.Count, vbOKCancel)
            'If ret = vbCancel Then Exit Function

            'RWH(04/09/01) Delete existing  records before doing update.
            m_lReturn = CType(DeleteCoinsurance(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            For lSub = 1 To m_oCLMCoinsuranceRecoveriess.Count()
                oCLMCoinsuranceRecoveries = m_oCLMCoinsuranceRecoveriess.Item(lSub)


                Select Case oCLMCoinsuranceRecoveries.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            Else
                                m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = CType(oCLMCoinsuranceRecoveries.AddItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            'MsgBox "Begin Trans"
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            Else
                                m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = CType(oCLMCoinsuranceRecoveries.UpdateItem(), gPMConstants.PMEReturnCode)
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
                            Else
                                m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = CType(oCLMCoinsuranceRecoveries.DeleteItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Retain the Primary Key of the CLMCoinsuranceRecoveries
            If m_oCLMCoinsuranceRecoveriess.Count() = 0 Then
                ' do nothing
            Else
                With oCLMCoinsuranceRecoveries
                    PartyID = .PartyID
                End With
            End If

            ' Release last reference
            oCLMCoinsuranceRecoveries = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    '            m_lReturn& = CommitTrans()
                    '            If (m_lReturn& <> PMTrue) Then
                    '                Update = PMFalse
                    '                Exit Function
                    '            End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oCLMCoinsuranceRecoveriess.Count()

                        ' With the item
                        With m_oCLMCoinsuranceRecoveriess.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oCLMCoinsuranceRecoveriess.Delete(lSub)

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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub




    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required CLMCoinsuranceRecoveriess and populate the Collection
    '
    ' ***************************************************************** '
    'developer guide no.101
    Public Function GetDetails(Optional ByRef vLockMode As gPMConstants.PMELockMode = 0, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCLMCoinsuranceRecoveries As bCLMCoinsuranceRecoveries.Recoveries

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oCLMCoinsuranceRecoveriess.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = gPMConstants.PMEReturnCode.PMFalse

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Information.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Check for Valid Primary Key

            Dim dbNumericTemp2 As Double

            If (Not Information.IsNothing(vClaimID)) And (Not Double.TryParse(CStr(vClaimID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vClaimID=" & vClaimID, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If

            ' Create New CLMCoinsuranceRecoveries
            oCLMCoinsuranceRecoveries = New bCLMCoinsuranceRecoveries.Recoveries()
            m_lReturn = CType(oCLMCoinsuranceRecoveries.Initialise(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            oCLMCoinsuranceRecoveries.TransactionType = m_sTransactionType

            ' Set component primary keys
            Dim vResultArray(,) As Object
            With oCLMCoinsuranceRecoveries
                .ClaimID = vClaimID
                .InsuranceFileCnt = vInsuranceFileCnt

                m_lReturn = CType(.GetDetails(vResultArray), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            If Information.IsArray(vResultArray) Then

                For lSub As Integer = 0 To vResultArray.GetUpperBound(1)
                    ' Create New
                    oCLMCoinsuranceRecoveries = New bCLMCoinsuranceRecoveries.Recoveries()
                    m_lReturn = CType(oCLMCoinsuranceRecoveries.Initialise(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                    With oCLMCoinsuranceRecoveries
                        .ClaimID = vClaimID
                        .InsuranceFileCnt = vInsuranceFileCnt

                        .PartyID = CInt(vResultArray(ACPartyCnt, lSub))


                        If CStr(vResultArray(ACSharePercent, lSub)).Trim() = "" Then

                            vResultArray(ACSharePercent, lSub) = 0
                        End If

                        If CStr(vResultArray(ACShareValue, lSub)).Trim() = "" Then

                            vResultArray(ACShareValue, lSub) = 0
                        End If


                        'developer guide no.98
                        m_lReturn = .SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyID:=vResultArray(ACPartyCnt, lSub), vPartyName:=vResultArray(ACName, lSub), vShare:=vResultArray(ACSharePercent, lSub), vShareValue:=vResultArray(ACShareValue, lSub))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End With

                    ' Add CLMCoinsuranceRecoveries to collection
                    m_lReturn = CType(m_oCLMCoinsuranceRecoveriess.Add(oNewCLMCoinsuranceRecoveries:=oCLMCoinsuranceRecoveries), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oCLMCoinsuranceRecoveries = Nothing

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
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied CLMCoinsuranceRecoveries into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vPartyName As Object = Nothing, Optional ByRef vShare As Object = Nothing, Optional ByRef vShareValue As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCLMCoinsuranceRecoveries As bCLMCoinsuranceRecoveries.Recoveries

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new CLMCoinsuranceRecoveries
            oCLMCoinsuranceRecoveries = New bCLMCoinsuranceRecoveries.Recoveries()
            m_lReturn = CType(oCLMCoinsuranceRecoveries.Initialise(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            ' Populate CLMCoinsuranceRecoveries Attributes



            'developer guide no.98
            m_lReturn = oCLMCoinsuranceRecoveries.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyID:=lRow, vPartyName:=vPartyName, vShare:=vShare, vShareValue:=vShareValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCLMCoinsuranceRecoveries = Nothing
                Return result
            End If

            With oCLMCoinsuranceRecoveries
                .PartyID = lRow
                .ClaimID = ClaimID
            End With

            ' Add CLMCoinsuranceRecoveries to collection
            m_lReturn = CType(m_oCLMCoinsuranceRecoveriess.Add(oNewCLMCoinsuranceRecoveries:=oCLMCoinsuranceRecoveries), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMCoinsuranceRecoveries = Nothing
                Return result
            End If

            oCLMCoinsuranceRecoveries = Nothing

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
    ' Description: Validates that this action is valid on the CLMCoinsuranceRecoveries
    '              specified and updates the CLMCoinsuranceRecoveries with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vPartyName As Object = Nothing, Optional ByRef vShare As Object = Nothing, Optional ByRef vShareValue As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oCLMCoinsuranceRecoveries As bCLMCoinsuranceRecoveries.Recoveries
        Dim iStatus As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            '    If (lRow& < 1) _
            ''    Or (lRow& > m_oCLMCoinsuranceRecoveriess.Count) Then
            '        EditUpdate = PMFalse
            '        Exit Function
            '    End If



            ' Get a reference to the row to Edit
            For lCount As Integer = 1 To m_oCLMCoinsuranceRecoveriess.Count()
                oCLMCoinsuranceRecoveries = m_oCLMCoinsuranceRecoveriess.Item(lCount)

                If oCLMCoinsuranceRecoveries.PartyID = lRow Then
                    ' Check the Status of the CLMCoinsuranceRecoveries

                    'If status is Add (i.e. It is not in the database),then leave status as Add
                    'or If status is Delete, report an error
                    'Otherwise set to Edit

                    Select Case oCLMCoinsuranceRecoveries.DatabaseStatus
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

                    ' Update CLMCoinsuranceRecoveries Attributes



                    'developer guide no.98
                    m_lReturn = oCLMCoinsuranceRecoveries.SetProperties(iStatus:=iStatus, vPartyName:=vPartyName, vShare:=vShare, vShareValue:=vShareValue)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        oCLMCoinsuranceRecoveries = Nothing
                        Return result
                    End If
                End If
                ' Release reference to CLMCoinsuranceRecoveries
                oCLMCoinsuranceRecoveries = Nothing
            Next lCount
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
    ' Description: Validate that the specified CLMCoinsuranceRecoveries can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oCLMCoinsuranceRecoveries As bCLMCoinsuranceRecoveries.Recoveries
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            '    If (lRow& < 1) _
            ''    Or (lRow& > m_oCLMCoinsuranceRecoveriess.Count) Then
            '        EditDelete = PMFalse
            '        Exit Function
            '    End If

            ' Get a reference to the row to delete
            For lCount As Integer = 1 To m_oCLMCoinsuranceRecoveriess.Count()
                oCLMCoinsuranceRecoveries = m_oCLMCoinsuranceRecoveriess.Item(lCount)
                If oCLMCoinsuranceRecoveries.PartyID = lRow Then
                    ' Check the Status of the CLMCoinsuranceRecoveries

                    'If status is Added (i.e. It is not in the database),
                    'then set to Dummy Delete else set to Delete
                    If oCLMCoinsuranceRecoveries.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                        oCLMCoinsuranceRecoveries.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
                    Else
                        oCLMCoinsuranceRecoveries.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
                    End If

                    ' Release reference to CLMCoinsuranceRecoveries
                End If
                oCLMCoinsuranceRecoveries = Nothing
            Next lCount
            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Class name: Business
    '
    ' Name: GetParty (Public)
    '
    ' Description:Gets the list of Party Names when the Claim iD is
    '             passed
    '
    'Author : Ranjit

    ' Date : 08 June 2000
    ' KB 14022003 PN Issue 1913: Changed v_iClaimId to be a long  - v_lClaimID
    ' ***************************************************************** '

    Public Function GetParty(ByVal v_lClaimId As Integer, ByRef r_vPartyName As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Parameters Array


            r_vPartyName = DBNull.Value

            m_dCLMCoinsuranceRecoveries.ClaimID = v_lClaimId
            m_lReturn = m_dCLMCoinsuranceRecoveries.GetParty(r_vPartyName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Class name: Business
    '
    ' Name: GetDetailsShare (Public)
    '
    ' Description: Gets the Details of Share Value and Share Percent
    '               for the details screen
    '
    '
    ' Date : 08 June 2000
    '
    ' Author: Ranjit
    ' KB 14022003 PN Issue 1913: Changed v_iClaimId to be a long  - v_lClaimID

    ' ***************************************************************** '

    Public Function GetDetailsShare(ByVal v_lClaimId As Integer, ByVal v_lPartyID As Integer, ByRef r_vShare As Object) As Integer

        '***Wirte the code for passing the parameters and collecting the resultset
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Set m_oDatabase = New dPMDAO.Database
            ' Clear the Parameters Array
            m_dCLMCoinsuranceRecoveries.ClaimID = v_lClaimId
            m_dCLMCoinsuranceRecoveries.PartyID = v_lPartyID
            m_lReturn = m_dCLMCoinsuranceRecoveries.GetDetailsShare(r_vShare)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetailsShare Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetailsShare", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Class Name : Business
    '
    ' Name: GetMainShare (Public)
    '
    ' Description: Gets the Details of Share Value and Share Percent
    '               for the main screen
    '
    ' Author : Ranjit
    '
    ' Date : 08 June 2000
    ' KB 14022003 PN Issue 1913: Changed v_iClaimId to be a long  - v_lClaimID
    '
    '' ***************************************************************** '

    Public Function GetMainShare(ByVal v_lClaimId As Integer, ByRef r_vShare(,) As Object) As Integer

        '***Wirte the code for passing the parameters and collecting the resultset
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Set m_oDatabase = New dPMDAO.Database
            ' Clear the Parameters Array
            m_dCLMCoinsuranceRecoveries.ClaimID = v_lClaimId
            m_lReturn = m_dCLMCoinsuranceRecoveries.GetMainShare(r_vShare)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMainShare Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMainShare", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Class Name : Business

    ' Name: Treatment_Lookup (Public)
    '
    ' Description:Gets the list of  Coinsurance treatment Description values
    '
    ' Author: Ranjit
    '
    ' Date : 08 June 2000
    ' ***************************************************************** '

    Public Function GetTreatment_Values(ByRef r_vArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Set m_oDatabase = New dPMDAO.Database

            m_lReturn = m_dCLMCoinsuranceRecoveries.GetTreatment_Values(r_vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTreatment_Values Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTreatment_Values", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Class Name : Business
    '
    ' Name: UpdateTreatment (Public)
    '
    ' Description: Updates the Claim Table records with the corresponding
    '              Treatment Values
    '
    ' Author : Ranjit
    '
    ' Date : 08 June 2000
    ' ***************************************************************** '

    Public Function UpdateTreatment(ByVal v_lClaimId As Integer, ByVal v_sDescription As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Parameters Array
            m_oDatabase.Parameters.Clear()

            ' Add the Paramters to the Input
            m_lReturn = m_dCLMCoinsuranceRecoveries.UpdateTreatment(v_lClaimId, v_sDescription)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateTreatment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTreatment", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Public Function GetTreatmentValue(ByVal v_lClaimId As Object, ByRef vArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' Add the Paramters to the Input

            m_lReturn = m_dCLMCoinsuranceRecoveries.GetTreatmentValue(v_lClaimId, vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTreatmentValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTreatmentValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Class Name : Business
    '
    ' Name: GetClaimNumber (Public)
    '
    ' Description: Gets the ClaimNumber from the given ClaimID
    '
    ' Author : Ranjit
    '
    ' Date : 08 June 2000
    '' ***************************************************************** '

    Public Function GetClaimNumber(ByRef r_vArray As Object) As Integer

        '***Wirte the code for passing the parameters and collecting the resultset
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Set m_oDatabase = New dPMDAO.Database
            ' Clear the Parameters Array
            m_dCLMCoinsuranceRecoveries.ClaimID = m_lClaimID
            m_lReturn = m_dCLMCoinsuranceRecoveries.GetClaimNumber(r_vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimNumber", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a OpenClaim.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Ranjit
    ' ***************************************************************** '
    Public Function GetLookupValues(ByVal iLookupType As Integer, ByRef vTableArray(,) As Object, ByVal iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vTabArray(3, 0) As Object
        Dim dtEffectiveDate As Date
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Table Array

            vTableArray = Nothing


            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "CoInsurance_Treatment"


            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""



            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

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
    '
    ' Name: GetBusinessType
    '
    ' Description:
    '
    ' History: 03/03/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetBusinessType(ByVal v_lInsFileCnt As Integer, ByRef r_vResArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_dCLMCoinsuranceRecoveries.GetBusinessType(v_lInsFileCnt, r_vResArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBusinessType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusinessType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' Created By:  'RWH(09/11/2000) For Claims Numbering.
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnderwritingOrAgencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnderwritingOrAgency", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                m_lReturn = m_oDatabase.SQLRollbackTrans()

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' SET 01082002 - Scalability
            lStatus = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("status").Value)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSystemOption
    '
    ' Description: get system option for option number
    '
    ' History: 29/05/2001 Tinny - Created
    '
    ' ***************************************************************** '
    Public Function GetSystemOption(ByVal v_lOptionNumber As Integer, ByRef r_vResult As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()


            result = m_oDatabase.Parameters.Add(sName:="option_number", vValue:=CStr(v_lOptionNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetSystemOptionSQL, sSQLName:=ACGetSystemOptionName, bStoredProcedure:=ACGetSystemOptionStored, vResultArray:=vResultArray, bKeepNulls:=True)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If



            r_vResult = vResultArray(0, 0)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemOption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim l_vResultArray(,) As Object
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

            'either PMTrue or PMNotFound
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                r_bInfoStatus = False
            Else

                r_bInfoStatus = CBool(l_vResultArray(0, 0))
            End If

            Return m_lReturn

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInfoOnlyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientAndPolicyID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    '***********************************************************************
    ' Name : GetOriginalClaimID
    '
    ' Desc : get the original claim ID from table
    '
    ' Hist : 15 June 2001 Tinny - Created
    '***********************************************************************
    Public Function GetOriginalClaimID(ByVal v_lClaimId As Integer, ByRef r_lOriginalClaimID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

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

            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lOriginalClaimID = CInt(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOriginalClaimID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOriginalClaimID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: DeleteCoinsurance
    '
    ' Description:
    '
    ' History: 04/09/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteCoinsurance() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claimid", vValue:=CStr(m_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=kDeleteCoinsuranceSQL, sSQLName:=kDeleteCoinsuranceName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteCoinsurance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteCoinsurance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         TidyUpAfterCancel
    '
    ' Parameters:   v_lClaimId      -  claim id
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

        Dim result As Integer = 0
        Const sFunctionName As String = "TidyUpAfterCancel"

        Try

            Dim oBusiness As bCLMRiskDetails.Business

            result = gPMConstants.PMEReturnCode.PMTrue


            oBusiness = New bCLMRiskDetails.Business
            m_lReturn = oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to create the business component
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lClaimId", v_lClaimId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to get an instance of bCLMRiskDetails.Business business object", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)

                Return result
            End If


            m_lReturn = oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to set process modes
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lClaimId", v_lClaimId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed when calling SetProcessModes on bCLMRiskDetails.Business business object", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)

                Return result
            End If


            m_lReturn = oBusiness.TidyUpAfterCancel(v_lClaimId:=v_lClaimId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call tidy up routine
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lClaimId", v_lClaimId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed when calling TidyUpAfterCancel on bCLMRiskDetails.Business business object", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)

                Return result
            End If

            oBusiness = Nothing

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
End Class
