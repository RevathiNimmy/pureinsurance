Imports SharedFiles
Module MainModule
    Private Const NullToBooleanDefault As Boolean = False
    Private Const NullToDecimalDefault As Object = 0
    Private Const NullToLongDefault As Integer = 0
    Private Const NullToStringDefault As String = ""
    Private Const NullToIntegerDefault As Integer = 0
    Private Const NullToDoubleDefault As Double = 0


    Private m_oInterface As RIPortfolioTransferInterface = Nothing
    Private m_oRI2007DisabledInterface As RI2007DisabledPortfolioTransferInterface = Nothing

    Private m_oCloneInterface As RICloneTransferInterface = Nothing

#Region "Application Constants"
    Public Const ACApp As String = "RIPortfolioTransfer"

#End Region

#Region "Main Method"
    Sub Main()

            Dim m_vIsRI2007 As Object

            ' Strip command line
            Dim argc As Integer
            Dim argv As System.Collections.ObjectModel.ReadOnlyCollection(Of String)

            argv = My.Application.CommandLineArgs
            argc = argv.Count

            If bPMFunc.getProductOptionValue(v_sUsername:="", _
                          v_sPassword:="", v_iUserID:=0, _
                          v_iMainSourceID:=0, v_iLanguageID:=0, _
                          v_iCurrencyID:=0, v_iLogLevel:=0, _
                          v_sCallingAppName:="", _
                          v_vOptionNumber:=SIRHiddenOptions.SIROPTEnableRI2007, _
                          v_vBranch:=g_iSourceID, _
                          r_vUnderwriting:=m_vIsRI2007) Then
            End If

            If argc > 0 Then
                If argv(0) = "PT" Then

                    If m_vIsRI2007 = "1" Then
                        m_oInterface = New RIPortfolioTransferInterface()
                        m_oInterface.ProcessInterface()
                    Else
                        m_oRI2007DisabledInterface = New RI2007DisabledPortfolioTransferInterface()
                        m_oRI2007DisabledInterface.ProcessInterface()
                    End If

                ElseIf argv(0) = "CT" Then
                    m_oCloneInterface = New RICloneTransferInterface()
                    m_oCloneInterface.ProcessInterface()
                End If
            Else
                If m_vIsRI2007 <> "1" Then
                    m_oRI2007DisabledInterface = New RI2007DisabledPortfolioTransferInterface()
                    m_oRI2007DisabledInterface.ProcessInterface()
                End If

            End If

    End Sub
#End Region


#Region "DestroyInteropComObject"
  
#End Region

    Public Function NullToBoolean(ByVal Expression As Object) As Boolean

        ' Check for null, else convert to boolean
        If Expression Is Nothing Then
            NullToBoolean = NullToBooleanDefault
        Else
            NullToBoolean = CBool(Expression)
        End If

    End Function

    Public Function NullToDate(ByVal Expression As Object) As Date

        ' Check for null, else convert to date
        If Expression Is Nothing Then
            NullToDate = Date.MinValue
        Else
            NullToDate = CDate(Expression)
        End If

    End Function

    Public Function NullToDecimal(ByVal Expression As Object) As Object

        ' Check for null, else convert to decimal
        If Expression Is Nothing Then
            ' There is no native decimal type so we must convert
            ' the default as well
            NullToDecimal = CDec(NullToDecimalDefault)
        Else
            NullToDecimal = CDec(Expression)
        End If

    End Function

    Public Function NullToLong(ByVal Expression As Object) As Integer

        ' Check for null, else convert to Integer
        If Expression Is Nothing Then
            NullToLong = NullToLongDefault
        Else
            NullToLong = CLng(Expression)
        End If

    End Function

    Public Function NullToString(ByVal Expression As Object) As String

        ' Check for null, else convert to string
        If Expression Is Nothing Then
            NullToString = NullToStringDefault
        Else
            NullToString = CStr(Expression)
        End If

    End Function

    Public Function NullToInteger(ByVal Expression As Object) As Integer

        If Expression Is Nothing Then
            NullToInteger = NullToIntegerDefault
        Else
            NullToInteger = CInt(Expression)
        End If

    End Function

    Public Function NullToDouble(ByVal Expression As Object) As Double
        If Expression Is Nothing Then
            NullToDouble = NullToDoubleDefault
        Else
            If IsNumeric(Expression) Then
                NullToDouble = CDbl(Expression)
            Else
                NullToDouble = NullToDoubleDefault
            End If
        End If

    End Function

    ''' <summary>
    ''' It will read WCFSecurityToken from config file then return encrypted token as string
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SecuirtyToken() As String
        Dim sSecurityToken As String = String.Empty
        If System.Configuration.ConfigurationManager.AppSettings("WCFSecurityToken") IsNot Nothing Then
            sSecurityToken = System.Configuration.ConfigurationManager.AppSettings("WCFSecurityToken").ToString
        End If

        If sSecurityToken.Length > 0 Then
            Return BCrypt.Net.BCrypt.HashPassword(sSecurityToken, 6)
        Else
            Return String.Empty
        End If
    End Function


End Module
