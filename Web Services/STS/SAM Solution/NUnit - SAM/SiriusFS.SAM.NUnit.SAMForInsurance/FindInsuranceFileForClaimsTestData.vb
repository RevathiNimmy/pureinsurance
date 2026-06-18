Option Strict On

Imports System

Friend Class FindInsuranceFileForClaimsTestData

    Public BranchCode As String
    Public ClaimDate As Date
    Public InsuranceRef As String

    Public Sub Load(ByVal oFindInsuranceFileForClaimsNode As XmlNode, ByVal oTestData As TestData)

        Try

            BranchCode = oTestData.CheckAttribute(oFindInsuranceFileForClaimsNode, "BranchCode", String.Empty)
            ClaimDate = oTestData.CheckAttribute(oFindInsuranceFileForClaimsNode, "ClaimDate", Date.MinValue)
            InsuranceRef = oTestData.CheckAttribute(oFindInsuranceFileForClaimsNode, "InsuranceRef", String.Empty)

        Catch ex As Exception
            Throw
        End Try
    End Sub

End Class
