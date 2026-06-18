Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'Developer Guide No.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 24th October 1996
    '
    ' Description: Creatable Business class which contains all the
    '              methods, business rules required to manipulate
    '              a PMUserGroup.
    '
    ' Edit History:
    ' Tom231098 - Nicked lock, stock and barrel from PMUser.
    ' DAK221199 - Check PMProductLookup table to see if we can continue
    ' DAK221299 - PMProductLookup.GetDetails - PMProductID changed to
    '             variant.
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/09/2003
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


    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer (Private)
    Private m_lCurrentRecord As Integer

    ' Current Task Record Pointer (Private)
    Private m_lTaskCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lError As gPMConstants.PMEReturnCode

    ' RDC 17102002
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Caption lookup
    Private m_oCaption As bPMCaption.Business
    'Private m_oCaption As bPMCaption.Business

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

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

        ' RDC 13062002 CompServ repalced with BAS module
        'Dim oComponentServices As PMServerBusinessCS
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

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


            ' Initialisation Code.

            '    Set oComponentServices = New PMServerBusinessCS


            If Information.IsNothing(vDatabase) Then
                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            Else

                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oComponentServices = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oCaption = New bPMCaption.Business
            lReturn = m_oCaption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            'create empty tasks collection

            ' Set Current Record to zero
            m_lTaskCurrentRecord = 0

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_oCaption IsNot Nothing Then
                    m_oCaption.Dispose()
                    m_oCaption = Nothing
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
            m_lError = m_oDatabase.SQLBeginTrans()

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            m_lError = m_oDatabase.SQLCommitTrans()

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            m_lError = m_oDatabase.SQLRollbackTrans()

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)

    Public Function UpdateOnlineAccessStatus(ByRef r_vChangeArray(,) As Object, ByRef r_bFailures As Boolean) As Integer

        Dim result As Integer = 0
        Dim dtUpdated As Date
        Dim lRecordsAffected As Integer
        Dim bTransStarted As Boolean
        Dim lContactCnt As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMFalse

            dtUpdated = DateTime.Now
            BeginTrans()
            bTransStarted = True

            For iLoop As Integer = r_vChangeArray.GetLowerBound(1) To r_vChangeArray.GetUpperBound(1)
                With m_oDatabase
                    .Parameters.Clear()


                    m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(r_vChangeArray(ACICAPartyCnt, iLoop)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If


                    'Developer Guide No.85
                    m_lReturn = .Parameters.Add(sName:="online_status", vValue:=r_vChangeArray(ACICAAccessStatus, iLoop), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If
                    'Developer Guide No.40
                    m_lReturn = .Parameters.Add(sName:="online_status_updated", vValue:=dtUpdated, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If
                    'Added the following check in case of nothing value passed.
                    If Not Information.IsNothing(lContactCnt) Then
                        m_lReturn = .Parameters.Add(sName:="contact_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    Else
                        m_lReturn = .Parameters.Add(sName:="contact_cnt", vValue:=lContactCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    ' Execute SQL Statement
                    m_lReturn = .SQLAction(sSQL:=ACUpdateOnlineAccessStatusSQL, sSQLName:=ACUpdateOnlineAccessStatusName, bStoredProcedure:=ACUpdateOnlineAccessStatusStored, lRecordsAffected:=lRecordsAffected)

                    lContactCnt = gPMFunctions.NullToLong(.Parameters.Item("contact_cnt").Value)


                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        r_vChangeArray(ACICAFailureReason, iLoop) = "Failed to run the Update Online " & _
                                                                    "Status Stored Procedure. Return Code: " & CStr(m_lReturn)
                        r_bFailures = True
                    ElseIf lContactCnt = 0 And CBool(r_vChangeArray(ACICAAccessStatus, iLoop)) Then

                        r_vChangeArray(ACICAFailureReason, iLoop) = "No email address exists for this Client"
                        r_bFailures = True
                    ElseIf lRecordsAffected <> 1 Then

                        r_vChangeArray(ACICAFailureReason, iLoop) = "Wrong number of records updated. " & _
                                                                    "Records affected: " & CStr(lRecordsAffected)
                        r_bFailures = True
                    End If

                End With
            Next

            result = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateOnlineAccessStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateOnlineAccessStatus", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            If bTransStarted Then
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    CommitTrans()
                Else
                    RollbackTrans()
                End If
            End If



        End Try

        Return result
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

End Class

