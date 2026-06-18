'<Serializable()> Public Class Branches

'    Private sBranchCode As String
'    Private sDescription As String



'    Public Sub New()

'    End Sub

'    Public Sub New(ByVal v_sBranchCode As String, ByVal v_sDescription As String)
'        sBranchCode = v_sBranchCode
'        sDescription = v_sDescription
'    End Sub


'    ''' <summary>
'    ''' Branch Code
'    ''' </summary>
'    ''' <value>Branch Code</value>
'    ''' <returns>Branch Code</returns>
'    Public Property BranchCode() As String
'        Get
'            Return sBranchCode
'        End Get
'        Set(ByVal value As String)
'            sBranchCode = value
'        End Set
'    End Property

'    ''' <summary>
'    ''' Description
'    ''' </summary>
'    ''' <value>Description</value>
'    ''' <returns>Description</returns>
'    Public Property Description() As String
'        Get
'            Return sDescription
'        End Get
'        Set(ByVal value As String)
'            sDescription = value
'        End Set
'    End Property

'    Public Function Print() As String

'        Dim sbPrint As New Text.StringBuilder

'        sbPrint.AppendLine("Branch Code : " & sBranchCode & "<br />")
'        sbPrint.AppendLine("Description : " & sDescription & "<br />")
'        bPrint.AppendLine("<br />")

'        Return sbPrint.ToString()

'    End Function

'End Class

<Serializable()> Public Class BranchesCollection : Inherits CollectionBase

    Public Function Add(ByVal v_oBranches As Branch) As Integer
        Return List.Add(v_oBranches)
    End Function

    Public Sub Remove(ByVal v_oBranches As Branch)
        List.Remove(v_oBranches)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As Branch
        Get
            Return List(i)
        End Get
        Set(ByVal value As Branch)
            List(i) = value
        End Set
    End Property

End Class
