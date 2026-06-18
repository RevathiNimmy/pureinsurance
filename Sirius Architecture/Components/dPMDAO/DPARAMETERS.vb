Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
<Serializable()>
<System.Runtime.InteropServices.ProgId("Parameters_NET.Parameters")>
Public Class Parameters

    Private Const ACClass As String = "Parameters"

    ' To replace the Global Data from main module
    Private m_sSiriusUsername As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iLogLevel As Integer
    Private m_bConnectionPooling As Boolean

    Private m_oCmd As SqlCommand

    Private m_lReturn As PMConstants.PMEReturnCode

    Friend ReadOnly Property ADOCommand() As SqlCommand
        Get
            Return m_oCmd
        End Get
    End Property

    ' This is needed to remove the global variables from main module
    Friend Sub SetGlobalData(ByRef sSiriusUsername As String, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef sCallingAppName As String, ByRef iLogLevel As Integer, ByRef bConnectionPooling As Boolean)
        Try
            m_sSiriusUsername = sSiriusUsername
            m_iSourceID = iSourceID
            m_iLanguageID = iLanguageID
            m_sCallingAppName = sCallingAppName
            m_iLogLevel = iLogLevel
            m_bConnectionPooling = bConnectionPooling

        Catch excep As System.Exception
            Throw New System.Exception(excep.Source + ", " + excep.Message)

            Exit Sub
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a Parameter to the Parameter Collection
    '
    '
    ' ***************************************************************** '
    Public Function Add(ByRef sName As String, ByRef vValue As Object, ByRef iDirection As Integer, ByRef iDataType As Integer) As Integer

        Dim result As Integer = 0
        Dim sTsName As String = ""
        Dim lSize As Integer
        Dim oADOParameter As SqlParameter
        Dim sADOName As String = ""
        Dim iADODirection As ParameterDirection
        Dim iADODataType As DbType
        Dim sErrMsg, sADOType As String

        Try
            result = PMConstants.PMEReturnCode.PMTrue

            If Not vValue Is DBNull.Value Then
                If TypeOf vValue Is String Then
                    If String.IsNullOrEmpty(vValue) Then
                        vValue = Nothing
                    End If
                End If
            End If

            ' Need to add on the Parameter Prefix @ (Even for RETURN_VALUE)
            sADOName = PMConstants.PMDBParamPrefix & sName

            ' Convert the Direction
            Select Case iDirection
                Case PMConstants.PMEParameterDirection.PMParamInput
                    If (vValue Is Nothing) Then
                        If (iDataType = PMConstants.PMEDataType.PMString) Then
                            vValue = DBNull.Value.ToString
                        Else
                            vValue = DBNull.Value
                        End If
                    End If
                    iADODirection = ParameterDirection.Input
                Case PMConstants.PMEParameterDirection.PMParamInputOutput
                    iADODirection = ParameterDirection.InputOutput
                Case PMConstants.PMEParameterDirection.PMParamOutput
                    If (vValue Is Nothing) Then
                        vValue = DBNull.Value.ToString
                    End If
                    iADODirection = ParameterDirection.Output
                Case PMConstants.PMEParameterDirection.PMParamDefault
                    ' Do nothing as this parameter will default to the stored
                    ' procedure default value
                    iADODirection = ParameterDirection.Input
                Case Else
                    iADODirection = ParameterDirection.ReturnValue
            End Select

            lSize = 0

            ' Get and convert the Type
            Select Case iDataType
                ' This is a bodge so that I can recognise TableNames ans Fieldnames
                ' differently to strings and therefore avoid putting quotes around them
                ' when the substitution is done in MergeParameters.
                Case PMConstants.PMEDataType.PMTableName, PMConstants.PMEDataType.PMFieldName
                    'iADODataType = DbType.String
                    'lSize = 255
                    iADODataType = DbType.Object

                Case PMConstants.PMEDataType.PMString
                    iADODataType = DbType.String
                    ' Need to pass in a size with varchar
                    If Convert.IsDBNull(vValue) Or (vValue Is Nothing) Then
                        lSize = 255
                    ElseIf vValue.Length < 256 Then
                        lSize = 255
                    Else
                        lSize = vValue.Length
                    End If
                Case PMConstants.PMEDataType.PMLong, PMConstants.PMEDataType.PMInteger
                    iADODataType = DbType.Int32
                Case PMConstants.PMEDataType.PMDate
                    iADODataType = DbType.DateTime
                Case PMConstants.PMEDataType.PMDouble
                    iADODataType = DbType.Double
                Case PMConstants.PMEDataType.PMCurrency
                    iADODataType = DbType.Currency
                Case PMConstants.PMEDataType.PMBoolean
                    iADODataType = DbType.Boolean
                Case PMConstants.PMEDataType.PMBinary
                    iADODataType = DbType.Binary
                    lSize = 8
                Case PMConstants.PMEDataType.PMDecimal
                    iADODataType = DbType.Decimal
                Case PMConstants.PMEDataType.PMFieldName, PMConstants.PMEDataType.PMTableName
                    iADODataType = DbType.Object
                Case Else
                    iADODataType = DbType.String
            End Select

            If lSize > 0 Then
                Dim TempParameter As SqlParameter
                TempParameter = m_oCmd.CreateParameter()
                TempParameter.ParameterName = sADOName
                TempParameter.DbType = iADODataType
                TempParameter.Direction = iADODirection
                TempParameter.Size = lSize
                oADOParameter = TempParameter
            Else
                Dim TempParameter As SqlParameter
                TempParameter = m_oCmd.CreateParameter()
                TempParameter.ParameterName = sADOName
                TempParameter.DbType = iADODataType
                TempParameter.Direction = iADODirection
                oADOParameter = TempParameter
            End If

            If oADOParameter Is Nothing Then
                LogDatabaseError(m_sSiriusUsername, m_sCallingAppName, m_iSourceID, m_iLanguageID, m_bConnectionPooling, PMConstants.PMELogLevel.PMLogError, "Unable to Create Parameter : " & sName, ACApp, ACClass, "Add")
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the Value if its input
            If iADODirection = ParameterDirection.Input Or iADODirection = ParameterDirection.InputOutput Then
                If oADOParameter.DbType = DbType.DateTime And Not vValue Is DBNull.Value Then
                    oADOParameter.Value = DateTime.Parse(vValue)
                Else
                    oADOParameter.Value = vValue
                End If
            End If

            ' Append it to the Commands Parameters Collection
            m_oCmd.Parameters.Add(oADOParameter)

            Return result
        Catch excep As System.Exception
            ' Error Section.
            sErrMsg = "Error Adding Parameter, Error = " & excep.Message & Strings.ChrW(13) & StringCHR10()

            Select Case iADODataType
                Case DbType.Boolean
                    sADOType = "Boolean"
                Case DbType.Currency
                    sADOType = "Currency"
                Case DbType.String
                    sADOType = "String (char)"
                Case DbType.String
                    sADOType = "String (varchar)"
                Case DbType.Date, DbType.DateTime, DbType.Date, DbType.Time
                    sADOType = "DateTime (" & iADODataType & ")"
                Case DbType.Int32
                    sADOType = "Numeric"
                Case DbType.Int32
                    sADOType = "Integer"
                Case DbType.Decimal
                    sADOType = "Decimal"
                Case DbType.Single
                    sADOType = "Single"
                Case DbType.Int16
                    sADOType = "SmallInt"
                Case DbType.UInt16
                    sADOType = "Unsigned TinyInt"
                Case DbType.Int16
                    sADOType = "TinyInt"
                Case DbType.Double
                    sADOType = "Double"
                Case Else
                    sADOType = "Unknown (" & iADODataType & ")"
            End Select

            sErrMsg = sErrMsg & "Param Name=" & sName & " Value=" & vValue & " DataType=" & sADOType & " ADODirection=" & CStr(iDirection)

            result = PMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogDatabaseError(sSiriusUsername:=m_sSiriusUsername, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, bConnectionPooling:=m_bConnectionPooling, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrMsg, vApp:="dPMDAO", vClass:="Parameters", vMethod:="Add", vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of Parameters in the collection
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer
        Try
            If m_oCmd.Parameters Is Nothing Then
                Return 0
            Else
                Return m_oCmd.Parameters.Count
            End If
        Catch
            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:="dPMDAO", vClass:="Parameters", vMethod:="Count")
            Return 0
            Exit Function
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a Parameter from the Parameter Collection
    '
    '
    ' ***************************************************************** '
    Public Sub Delete(ByRef vKey As Byte)
        Try
            m_oCmd.Parameters.RemoveAt(vKey)
        Catch excep As System.Exception
            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="vKey=" & vKey, vApp:="dPMDAO", vClass:="Parameters", vMethod:="Delete", vErrDesc:=excep.Message)
            Exit Sub
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected Parameter from the Collection
    '
    '
    ' ***************************************************************** '
    Public Function Item(ByRef vKey As String) As SqlParameter
        Dim vADOKey As String = ""
        Try
            vADOKey = PMConstants.PMDBParamPrefix & vKey

            Return m_oCmd.Parameters.Item(vADOKey)
        Catch
            Return Nothing
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAll
    '
    ' Description: Delete all Parameters from the Parameters Collection
    '
    '
    ' ***************************************************************** '
    Public Sub DeleteAll()
        Dim iParameterCount As Integer
        Try
            ' Determine the number of Parameters in the collection
            iParameterCount = Count()

            ' Delete all the Parameters
            For i As Integer = 1 To iParameterCount
                Delete(0)
            Next i
        Catch excep As System.Exception
            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteAll Failed", vApp:="dPMDAO", vClass:="Parameters", vMethod:="DeleteAll", vErrDesc:=excep.Message)
            Exit Sub
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the Parameters Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()
        Try
            ' Set Parameters Collection to Nothing
            m_oCmd = New SqlCommand()

            m_oCmd.Connection = Nothing
            'm_oCmd.NamedParameters = True
        Catch excep As System.Exception
            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:="dPMDAO", vClass:="Parameters", vMethod:="Clear", vErrDesc:=excep.Message)
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
    Friend Function Initialise() As Integer
        Return PMConstants.PMEReturnCode.PMTrue
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
            m_oCmd = Nothing
            Return result
        Catch excep As System.Exception
            ' Error Section.
            result = PMConstants.PMEReturnCode.PMError
            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Terminate Failed", vApp:="dPMDAO", vClass:="Parameters", vMethod:="Terminate", vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Sub New()
        MyBase.New()
        Try
            Clear()
        Catch excep As System.Exception
            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the parameters class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrDesc:=excep.Message)
            Exit Sub
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        Try
            ' Class Initialise
            m_lReturn = CType(Terminate(), PMConstants.PMEReturnCode)
        Catch excep As System.Exception
            ' Log Error Message
            LogDatabaseMessage(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the parameters class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Terminate", vErrDesc:=excep.Message)
            Exit Sub
        End Try
    End Sub
    Private Function StringCHR() As String
        Return ""
    End Function
    Private Function StringCHR10() As String
        Return ""
    End Function
End Class
