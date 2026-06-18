Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no. 129
Friend NotInheritable Class ColWrapper

    Private Const ACClass As String = "ObjectKeys"

    Private _m_cCollection As Dictionary(Of Object, Object) = Nothing
    Private Property m_cCollection() As Dictionary(Of Object, Object)
        Get
            If _m_cCollection Is Nothing Then
                _m_cCollection = New Dictionary(Of Object, Object)()
            End If
            Return _m_cCollection
        End Get
        Set(ByVal Value As Dictionary(Of Object, Object))
            _m_cCollection = Value
        End Set
    End Property


    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description:
    '
    ' History: 01/11/2000 sj - Created.
    '
    ' ***************************************************************** '
    Public Function Add(ByVal v_vItem As Object, Optional ByVal v_vKey As Object = Nothing, Optional ByRef r_vExists As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Informations.IsNothing(v_vKey) Then
                m_cCollection.Add(v_vKey, v_vItem)
            Else
                'm_cCollection.Add(v_vItem)
            End If

            Return result

        Catch excep As System.Exception



            If Informations.Err().Number = 457 Then

                If Not Informations.IsNothing(r_vExists) Then
                    r_vExists = True
                End If
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description:
    '
    ' History: 01/11/2000 sj - Created.
    '
    ' ***************************************************************** '
    Public Function Item(ByVal v_vKey As Object, Optional ByRef r_vItem As Object = Nothing, Optional ByRef r_vExists As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If (m_cCollection.ContainsKey(v_vKey)) Then
                r_vItem = m_cCollection(v_vKey)
                r_vExists = True
            Else
                r_vExists = False
            End If

            Return result

        Catch excep As System.Exception



            If Informations.Err().Number = 5 Or Informations.Err().Number = 9 Then

                If Not Informations.IsNothing(r_vExists) Then
                    r_vExists = False
                End If
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Item Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Remove
    '
    ' Description:
    '
    ' History: 01/11/2000 sj - Created.
    '
    ' ***************************************************************** '
    Public Function Remove(ByVal v_vKey As Object, Optional ByRef r_vExists As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Informations.IsNothing(r_vExists) Then
                r_vExists = True
            End If

            m_cCollection.Remove(v_vKey)

            Return result

        Catch excep As System.Exception



            If Informations.Err().Number = 5 Or Informations.Err().Number = 9 Then

                If Not Informations.IsNothing(r_vExists) Then
                    r_vExists = False
                End If
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Remove Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Remove", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description:
    '
    ' History: 01/11/2000 sj - Created.
    '
    ' ***************************************************************** '
    Public Function Count(ByRef r_lCount As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_lCount = m_cCollection.Count

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Protected Overrides Sub Finalize()
        m_cCollection = Nothing
    End Sub
End Class
