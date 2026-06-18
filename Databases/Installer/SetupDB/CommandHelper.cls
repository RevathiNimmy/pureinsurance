VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CCommandHelper"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
' Class:    Thin wrapper round an ADO command object
' Shared:   PublicNotCreatable, Yes (RESTRICTED)
'
' THIS CODE MODULE IS NOW SHARED DIRECTLY WITH THE SIRIUS DATABASE
' INSTALLER PROJECT, DUE TO ITS UNIQUE REQUIREMENT FOR NO VB DLL
' REFERENCES. ALL SWIFT CODE SHOULD REFERENCE THIS DLL AS NORMAL.
'
' **********************************************************************
' This class contains two obsolete hidden public members. Please
' delete these if you are copying the class to another COM library!
' **********************************************************************
'
Option Explicit

Private m_con As CDatabase
Private m_cmd As ADODB.Command

' Error constants
Private Const knErrBadType = vbObjectError + 1
Private Const ksErrBadType = "Unrecognised ADODB.Parameter.Type %1."

Friend Sub Initialise(ByVal con As CDatabase, _
    ByVal sText As String, _
    ByVal bProcedure As Boolean, _
    Optional ByVal bPrepared As Boolean = False, _
    Optional ByVal nCommandTimeout As Long = -1)

    Set m_con = con

    If nCommandTimeout = -1 Then
        nCommandTimeout = con.Connection.CommandTimeout
    End If

    Set m_cmd = New ADODB.Command
    m_cmd.CommandText = sText
    m_cmd.CommandType = IIf(bProcedure, adCmdStoredProc, adCmdText)
    m_cmd.Prepared = bPrepared
    m_cmd.CommandTimeout = nCommandTimeout

    Set m_cmd.ActiveConnection = con.Connection

End Sub

Public Function CreateParameter(ByVal sName As String, _
    ByVal nType As ADODB.DataTypeEnum, _
    Optional ByVal nDirection As ADODB.ParameterDirectionEnum = adParamInput, _
    Optional ByVal nSize As Long = 0, _
    Optional ByVal vValue) As ADODB.Parameter

    Dim par As ADODB.Parameter

    If IsMissing(vValue) Then
        Set par = m_cmd.CreateParameter(sName, nType, nDirection, nSize)
    Else
        Set par = m_cmd.CreateParameter(sName, nType, nDirection, nSize, vValue)
    End If
    m_cmd.Parameters.Append par
    Set CreateParameter = par

End Function

Public Sub Execute()

    m_con.LastSQL = m_cmd.CommandText
    m_cmd.Execute Options:=adExecuteNoRecords

End Sub

Public Function OpenRecordset() As ADODB.Recordset

    Dim rs As ADODB.Recordset

    Set rs = New ADODB.Recordset
    rs.CursorLocation = adUseServer
    m_con.LastSQL = m_cmd.CommandText
    rs.Open Source:=m_cmd, CursorType:=adOpenForwardOnly, LockType:=adLockReadOnly
    Set OpenRecordset = rs

End Function

Public Function OpenRecordsetDisconnected() As ADODB.Recordset

    Dim rs As ADODB.Recordset

    Set rs = New ADODB.Recordset
    rs.CursorLocation = adUseClient
    m_con.LastSQL = m_cmd.CommandText
    rs.Open Source:=m_cmd, CursorType:=adOpenForwardOnly, LockType:=adLockReadOnly
    Set rs.ActiveConnection = Nothing
    Set OpenRecordsetDisconnected = rs

End Function

' **********************************************************************
' OBSOLETE - DO NOT COPY TO ANOTHER LIBRARY
' **********************************************************************
' Attributes: Hidden
Public Sub CloseRecordset()
Attribute CloseRecordset.VB_MemberFlags = "40"

End Sub

' Attributes: Default
Public Property Get Parameters() As ADODB.Parameters
Attribute Parameters.VB_UserMemId = 0

    Set Parameters = m_cmd.Parameters

End Property

' Access the command timeout property.
Public Property Get CommandTimeout() As Long

    CommandTimeout = m_cmd.CommandTimeout

End Property

' Access the command timeout property.
Public Property Let CommandTimeout(ByVal nCommandTimeout As Long)

    m_cmd.CommandTimeout = nCommandTimeout

End Property

' FOR DEBUGGING ONLY
Public Property Get WrappedCommand() As ADODB.Command

    Set WrappedCommand = m_cmd

End Property

' **********************************************************************
' OBSOLETE - DO NOT COPY TO ANOTHER LIBRARY
' **********************************************************************
' Attributes: Hidden
Public Property Get WrappedRecordset() As ADODB.Recordset
Attribute WrappedRecordset.VB_MemberFlags = "40"

End Property

' Return a SQL batch that can be used for testing the command in Query Analyzer.
Public Function TestSQL() As String

    Dim par As ADODB.Parameter
    Dim sCommandText As String
    Dim sDeclarations As CStringBuilder
    Dim sInputs As CStringBuilder
    Dim sCall1 As CStringBuilder
    Dim sCall2 As CStringBuilder
    Dim sOutputs As CStringBuilder
    Dim sSQL As CStringBuilder
    Dim sName As String

    If m_cmd.CommandType <> adCmdStoredProc Then
        TestSQL = "-- Command types other than stored procedure calls not yet implemented."
        Exit Function
    End If

    sCommandText = m_cmd.CommandText
    StartsWith sCommandText, "{ ", sCommandText
    StartsWith sCommandText, "? = ", sCommandText
    StartsWith sCommandText, "call ", sCommandText
    EndsWith sCommandText, " }", sCommandText
    sCommandText = ParseSep(sCommandText, "", "(")

    Set sDeclarations = New CStringBuilder
    Set sInputs = New CStringBuilder
    Set sCall1 = New CStringBuilder
    Set sCall2 = New CStringBuilder
    Set sOutputs = New CStringBuilder

    sDeclarations.InitialiseSeparators "declare", ","
    sInputs.InitialiseSeparators "select", ","
    sOutputs.InitialiseSeparators "select", ","
    sCall2.InitialiseSeparators " ", ", "

    sCall1 = "execute "
    sCall2 = sCommandText

    For Each par In m_cmd.Parameters
        sName = ToSQLVariable(par.Name)
        WriteDeclaration sDeclarations, par, sName
        Select Case par.Direction
        Case adParamReturnValue
            WriteOutput sOutputs, par, sName
            sCall1.Append sName
            sCall1.Append " = "
        Case adParamInput
            WriteInput sInputs, par, sName
            sCall2.AppendSeparator
            sCall2.Append sName
        Case adParamInputOutput
            WriteInput sInputs, par, sName
            WriteOutput sOutputs, par, sName
            sCall2.AppendSeparator
            sCall2.Append sName
            sCall2.Append " output"
        Case adParamOutput
            WriteOutput sOutputs, par, sName
            sCall2.AppendSeparator
            sCall2.Append sName
            sCall2.Append " output"
        End Select
    Next

    Set sSQL = New CStringBuilder

    If sDeclarations.length <> 0 Then
        sSQL.Append sDeclarations
        sSQL.Append vbCrLf
        sSQL.Append vbCrLf
    End If
    If sInputs.length <> 0 Then
        sSQL.Append sInputs
        sSQL.Append vbCrLf
        sSQL.Append vbCrLf
    End If
    sSQL.Append sCall1
    sSQL.Append sCall2
    sSQL.Append vbCrLf
    If sOutputs.length <> 0 Then
        sSQL.Append vbCrLf
        sSQL.Append sOutputs
        sSQL.Append vbCrLf
    End If

    TestSQL = sSQL

End Function

Private Sub Class_Terminate()

    Set m_con = Nothing
    Set m_cmd = Nothing

End Sub

Private Sub WriteDeclaration(ByVal sSQL As CStringBuilder, ByVal par As ADODB.Parameter, ByVal sName As String)

    sSQL.AppendSeparator
    sSQL.Append vbCrLf
    sSQL.Append "    "
    sSQL.Append sName
    sSQL.Append " "

    Select Case par.Type
    Case adBoolean
        sSQL.Append "bit"
    Case adTinyInt, adUnsignedTinyInt
        sSQL.Append "tinyint"
    Case adSmallInt
        sSQL.Append "smallint"
    Case adInteger
        sSQL.Append "integer"
    Case adBigInt
        sSQL.Append "bigint"
    Case adSingle
        sSQL.Append "real"
    Case adDouble
        sSQL.Append "float"
    Case adCurrency
        sSQL.Append "money"
    Case adNumeric
        sSQL.Append "numeric("
        sSQL.Append par.Precision
        sSQL.Append ", "
        sSQL.Append par.NumericScale
        sSQL.Append ")"
    Case adBinary
        sSQL.Append "binary("
        sSQL.Append par.Size
        sSQL.Append ")"
    Case adVarBinary
        sSQL.Append "varbinary("
        sSQL.Append par.Size
        sSQL.Append ")"
    Case adLongVarBinary
        sSQL.Append "image"
    Case adGUID
        sSQL.Append "uniqueidentifier"
    Case adChar
        sSQL.Append "char("
        sSQL.Append par.Size
        sSQL.Append ")"
    Case adVarChar
        sSQL.Append "varchar("
        sSQL.Append par.Size
        sSQL.Append ")"
    Case adLongVarChar
        sSQL.Append "varchar(8000) /* text */"
    Case adWChar
        sSQL.Append "nchar("
        sSQL.Append par.Size
        sSQL.Append ")"
    Case adVarWChar
        sSQL.Append "nvarchar("
        sSQL.Append par.Size
        sSQL.Append ")"
    Case adLongVarWChar
        sSQL.Append "nvarchar(4000) /* ntext */"
    Case adDBTimeStamp
        sSQL.Append "datetime"
    Case Else
        Err.Raise knErrBadType, "", Replace$(ksErrBadType, "%1", par.Type)
    End Select

End Sub

Private Sub WriteInput(ByVal sSQL As CStringBuilder, ByVal par As ADODB.Parameter, ByVal sName As String)

    sSQL.AppendSeparator
    sSQL.Append vbCrLf
    sSQL.Append "    "
    sSQL.Append sName
    sSQL.Append " = "
    sSQL.Append ToSQL(par.Value)

End Sub

Private Sub WriteOutput(ByVal sSQL As CStringBuilder, ByVal par As ADODB.Parameter, ByVal sName As String)

    sSQL.AppendSeparator
    sSQL.Append vbCrLf
    sSQL.Append "    "
    sSQL.Append sName
    sSQL.Append " as ["
    sSQL.Append sName
    sSQL.Append "]"

End Sub

Private Function ToSQLVariable(ByVal sName As String) As String

    If Not StartsWith(sName, "@") Then
        sName = "@" & sName
    End If
    ToSQLVariable = sName

End Function
