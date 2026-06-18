Imports System.Xml
Imports System.Data.SqlClient
Imports System.IO

Imports Nexus.Utils

Namespace SiteMap

    Public Module PageTypes

        Function GetSchemaString(ByVal pPageTypeID As Integer) As String

            Dim command As New SqlCommand("usp_GetPageTypeSchema")
            Dim tmpSchema As String = String.Empty

            With command
                .Parameters.Add("@page_type_id", SqlDbType.Int).Value = pPageTypeID

                Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command)

                While sdrTmp.Read
                    tmpSchema = sdrTmp("xsd_schema")
                End While

                command.Dispose()
                sdrTmp.Close()

            End With

            Return tmpSchema

        End Function

    End Module

End Namespace