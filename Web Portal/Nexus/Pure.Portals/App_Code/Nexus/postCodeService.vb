Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols

<WebService(Namespace:="http://ssp-uk.com/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class postCodeService
    Inherits System.Web.Services.WebService

    ''' <summary>
    ''' Calls the postcoder.com hosted service
    ''' </summary>
    ''' <param name="ref"></param>
    ''' <param name="sPostcode"></param>
    ''' <param name="sUserName"></param>
    ''' <param name="sPassword"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function ReturnAddressList(ByVal ref As String, ByVal sPostcode As String, ByVal sUserName As String, ByVal sPassword As String) As Postcoder.PremiseListResult
        If sPostcode <> "" And sUserName <> "" And sPassword <> "" Then
            Dim PcSoap As New Postcoder.PostCoderWebSOAP
            Dim PLResult As Postcoder.PremiseListResult

            If HttpContext.Current.Cache("PLResult") Is Nothing Then
                PLResult = PcSoap.getPremiseList(sPostcode, ref, sUserName, sPassword)
                HttpContext.Current.Cache.Insert("PLResult", PLResult, Nothing, Now.AddDays(14), TimeSpan.Zero)
            Else
                PLResult = HttpContext.Current.Cache("PLResult")
            End If
            Return PLResult
        End If
    End Function

End Class
