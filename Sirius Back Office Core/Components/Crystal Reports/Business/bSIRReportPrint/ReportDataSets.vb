Imports System.Collections.Generic

Public Class ReportDataSets
    Public Property ReportDataSet As List(Of ReportDataSet)
End Class
Public Class ReportDataSet
        Public Property DataSetName As String
        Public Property ReportQueryParameters As Dictionary(Of String, Object)
        Public Property SqlCommandText As String
        Public Property SqlCommandType As String
    End Class

