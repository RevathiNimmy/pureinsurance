Option Strict Off
Option Explicit On
Imports System.Data
<Serializable()>
<System.Runtime.InteropServices.ProgId("Records_NET.Records")>
Public Class Records
    Private Const ACClass As String = "Records"
    Private m_oRecs As DataSet

    Private m_lCurrentRecord As Integer
    Public ReadOnly Property CurrentRecord() As Integer
        Get
            Return m_lCurrentRecord
        End Get
    End Property

    Public Function Count() As Integer
        Try
            If m_oRecs Is Nothing Then
                Return 0
            Else
                Return m_oRecs.Tables(0).Rows.Count
            End If
        Catch
            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:="dPMDAO", vClass:="Records", vMethod:="Count")
            Return 0
            Exit Function
        End Try


    End Function

    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a Record from the Records Collection
    '
    '
    ' ***************************************************************** '
    Private Sub Delete(ByRef vKey As Integer)
        Try
            m_oRecs.Tables(0).Rows(vKey).Delete()
            m_oRecs.AcceptChanges()
        Catch excep As System.Exception
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="vKey=" & vKey, vApp:="dPMDAO", vClass:="Records", vMethod:="Delete", vErrDesc:=excep.Message)
            Exit Sub
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected Record from the Records Collection
    '
    '
    ' ***************************************************************** '
    Public Function Item(ByRef vKey As Integer) As dPMDAO.Records
        Try
            If vKey <= Count() Then
                m_lCurrentRecord = vKey
            Else
                Return Nothing
            End If

            Return Me
        Catch excep As System.Exception
            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="vKey=" & vKey, vApp:="dPMDAO", vClass:="Records", vMethod:="Item", vErrDesc:=excep.Message)

            Return Nothing
        End Try
    End Function

    Public Function Fields() As DataRow
        Return m_oRecs.Tables(0).Rows(m_lCurrentRecord)
    End Function

    Public Sub Clear()
        Try
            ' Set Records Collection to Nothing
            m_oRecs = Nothing

            ' Set the CurrentRecord to zero
            m_lCurrentRecord = 0
        Catch excep As System.Exception
            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear", vApp:="dPMDAO", vClass:="Records", vMethod:="Clear", vErrDesc:=excep.Message)
            Exit Sub
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Friend Function Initialise(ByRef oRec As DataSet, ByRef bKeepNulls As Boolean) As Integer
        Dim result As Integer = 0

        Try
            result = PMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.
            m_oRecs = oRec

            ' Set the CurrentRecord to zero
            m_lCurrentRecord = 0

            Return result
        Catch excep As System.Exception
            ' Error Section.
            result = PMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:="dPMDAO", vClass:="Records", vMethod:="Initialise", vErrDesc:=excep.Message)

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
    Friend Function Terminate() As Integer
        Dim result As Integer = 0
        Try
            result = PMConstants.PMEReturnCode.PMTrue

            ' Termination Code.
            m_oRecs = Nothing

            ' Set the CurrentRecord to zero
            m_lCurrentRecord = 0

            Return result
        Catch excep As System.Exception
            result = PMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Terminate Failed", vApp:="dPMDAO", vClass:="Records", vMethod:="Terminate", vErrDesc:=excep.Message)

            Return result
        End Try
    End Function
    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()
        Dim lReturn As PMConstants.PMEReturnCode
        Try
            ' Class Terminate
            lReturn = CType(Terminate(), PMConstants.PMEReturnCode)
        Catch excep As System.Exception
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the records class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Terminate", vErrDesc:=excep.Message)
            Exit Sub
        End Try
    End Sub
End Class