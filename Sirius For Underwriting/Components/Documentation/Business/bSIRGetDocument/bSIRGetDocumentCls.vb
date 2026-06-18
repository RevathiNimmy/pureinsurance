Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no.129

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_lPartyCnt As Integer
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long
        Dim result As Integer = 0
        Dim ACClass As Object = Nothing
        Dim m_iCurrencyID, m_iLanguageID, m_iLogLevel, m_iSourceID, m_iUserID As Integer
        Dim m_sCallingAppName As String = ""
        Dim m_sPassword As String = ""
        Dim m_sUsername As String = ""


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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

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
    Dim ACClass As Object
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
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
        Dim ACClass As Object = Nothing
        Dim m_sTransactionType As String = ""
        Dim m_lProcessMode, m_lNavigate As Integer
        Dim m_iTask As Integer
        Dim m_dtEffectiveDate As Date
        Dim m_sUsername As String = ""

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
    '
    ' Name:        CheckIfSuppressed
    '
    ' Description: Checks if a document type has been suppressed through
    '              agent maintenance.
    '
    ' History:     PW170702
    '
    ' ***************************************************************** '
    Public Function CheckIfSuppressed(ByRef lProcessType As Integer, ByRef lInsuranceFileCnt As Integer, ByRef bSuppressed As Boolean) As Integer
        Dim result As Integer = 0
        Dim ACClass As Object = Nothing
        Dim m_sUsername As String = ""

        Dim lAgentPartyCnt As Integer
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            bSuppressed = False
            '
            ' Retrieve the insurance file record in order to get the agent's party count
            '
            With m_oDatabase

                ' add the insurance_file_cnt parameter
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACGetInsuranceFileSQL, sSQLName:=ACGetInsuranceFileName, bStoredProcedure:=ACGetInsuranceFileStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Do we have any records ?
                If .Records.Count() = 0 Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' get agent count
                ' PW020802 - scalability changes
                'Developer Guide No 111
                lAgentPartyCnt = gPMFunctions.NullToLong(.Records.Item(0).Fields()("lead_agent_cnt"))
                ' if no agent, then not suppressed
                If lAgentPartyCnt = 0 Then
                    Return result
                End If
                '
                ' Retrieve the list of suppressed documents for this agent from the
                ' agent_docs table
                '
                ' add the insurance_file_cnt parameter
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(lAgentPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACGetSuppressedDocsSQL, sSQLName:=ACGetSuppressedDocsName, bStoredProcedure:=ACGetSuppressedDocsStored, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            bSuppressed = True
            ' Check if the current process type is in the list of suppressed 'uns
            If Informations.IsArray(vResultArray) Then 'PN 70504

                For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                    If lProcessType = Val(CStr(vResultArray(0, i))) Then
                        bSuppressed = False
                        Return result
                    End If
                Next i
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckIfSuppressed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIfSuppressed", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
