Option Strict On

'Namespace CoreImplementation
Namespace Claims

    Public Class General

        Public Function ClaimExists(ByVal claimNumber As String, ByRef baseClaimId As Integer, ByRef versionId As Integer) As Boolean
            Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
                Return ClaimExists(con, claimNumber, baseClaimId, versionId)
            End Using
        End Function

        Public Function ClaimExists(ByVal con As SiriusConnection, ByVal claimNumber As String, ByRef baseClaimId As Integer, ByRef versionId As Integer) As Boolean

            Dim dt As DataTable = Nothing

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Claim_Details_By_Claim_Number")
                cmd.Parameters.Add("claim_number", SqlDbType.VarChar, 30).Value = claimNumber
                dt = con.ExecuteDataTable(cmd)
            End Using

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows.Item(0)
                baseClaimId = Cast.ToInt32(dr.Item("base_claim_Id"), 0)
                versionId = Cast.ToInt32(dr.Item("version_Id"), 0)
                Return True
            Else
                baseClaimId = 0
                versionId = 0
                Return False
            End If

        End Function

        Public Function GetReserveDetailsForClaimPeril( _
        ByVal con As SiriusConnection, _
        ByVal claimPerilKey As Integer) As DataTable

            Dim dt As DataTable = Nothing

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_GetBaseReserveKeysForClaimPeril")
                cmd.AddInParameter("@claim_peril_id", SqlDbType.Int).Value = claimPerilKey

                dt = con.ExecuteDataTable(cmd)

            End Using

            Return dt

        End Function

        Public Function GetRecoveryDetailsForClaimPeril( _
        ByVal con As SiriusConnection, _
        ByVal claimPerilKey As Integer) As DataTable

            Dim dt As DataTable = Nothing

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_GetBaseRecoveryKeysForClaimPeril")
                cmd.AddInParameter("@claim_peril_id", SqlDbType.Int).Value = claimPerilKey

                dt = con.ExecuteDataTable(cmd)

            End Using

            Return dt

        End Function

        ''' <summary>
        ''' Use for Getting BaseClaimKey against BaseClaimPerilKey 
        ''' </summary>
        ''' <param name="con"></param>
        ''' <param name="nClaimPerilKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>

        Public Function GetBaseClaimKeyForClaimPeril(ByVal con As SiriusConnection, _
                                                 ByVal nClaimPerilKey As Integer) As Integer
            Const kACMethodName As String = "GetBaseClaimKeyForClaimPeril"
            Try
                Dim nClaimID As Integer = 0
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_GetBaseClaimKeysForClaimPeril")
                    cmd.AddInParameter("@nClaim_Peril_ID", SqlDbType.Int).Value = nClaimPerilKey
                    cmd.AddOutParameter("@nClaim_ID", SqlDbType.Int)
                    con.ExecuteNonQuery(cmd)
                    nClaimID = CType(cmd.Parameters("@nClaim_ID").Value, Integer)
                End Using
                Return nClaimID
            Catch ex As Exception
                Dim STSErrorEx As New SiriusFS.SAM.Structure.STSErrorPublisher("An error occured GetBaseClaimKeyForClaimPeril", ex)
                STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), kACMethodName, kACMethodName, True)
                Return 0
            End Try
        End Function

    End Class

End Namespace
'End Namespace

