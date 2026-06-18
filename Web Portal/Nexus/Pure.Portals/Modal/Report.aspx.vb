Imports Nexus.Utils
Imports Nexus.Library
Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Xml.Linq

Partial Class Modal_Report
    Inherits System.Web.UI.Page

    Private sFileReportName As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'set the properties for the reportsource control and then bind the viewer control to it
        Dim sReportDirName As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Reports.Location
        Dim sFolder As String = Request.QueryString("folder")
        Dim sFileName As String = Request.QueryString("reportfile")

        sourceReportSource.Report.FileName = sReportDirName & "\" & sFolder & "\" & sFileName
        viewerReportViewer.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnOK.Attributes.Add("onclick", "openEmail()")
        If Not IsPostBack Then
            viewerReportViewer.DisplayGroupTree = False
        End If
    End Sub

    Protected Sub viewerReportViewer_Error(ByVal source As Object, ByVal e As CrystalDecisions.Web.ErrorEventArgs) Handles viewerReportViewer.Error
        e.Handled = True
    End Sub


    Protected Sub btnOK_Click1(sender As Object, e As EventArgs) Handles btnOK.Click
        Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.openEmail();self.parent.tb_remove();", True)

        'sFileReportName = GenerateReport()
        'ArchiveReportToSharePoint()
    End Sub

    Public Function GenerateReport() As String
        Dim oParametersCollection As New NexusProvider.ParametersCollection
        Dim sPlaceHolderControlID As String = "plcReportForm"
        Dim sUrl As String = String.Empty
        Dim sReportsTypeControlID As String = Nothing
        Dim sSelectedReportsType As String = Nothing
        Dim sCustomValidator As String = "cusReportForm"
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim sFileName As String

        sSelectedReportsType = GetLocalResourceObject("lblReport")

        'Executed Function from Dataset function
        Try
            Dim oParameters As NexusProvider.Parameters
            oParameters = New NexusProvider.Parameters
            oParameters.ParamNameField = "user_id"
            oParameters.ParamValueField = Nothing

            'add the param into the collection
            oParametersCollection.Add(oParameters)
            sFileName = GetReportUrl(sSelectedReportsType, oParametersCollection)

        Catch ex As NexusProvider.NexusException
            'Checking  (bSIRReportPrint.Business.SendToPrint Failed : Failed : Return Value = PMNotFound) Error code , then display a message saying no record found 
            If ex.Errors(0).Code = "1000019" Then
                ex.Errors(0).Code = "88"
            End If
            Throw
        End Try
        Return sFileName
    End Function

    ''' <summary>
    ''' This method retreive the report and returns the Url to open the report
    ''' </summary>
    ''' <param name="sReportName"></param>
    ''' <param name="oParametersCollection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetReportUrl(ByVal sReportName As String, ByVal oParametersCollection As NexusProvider.ParametersCollection) As String
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim sDocumentExtractionDirectory As String = Nothing
        Dim sUniqueDirectory As String = Guid.NewGuid.ToString
        Dim url As String = String.Empty
        Dim sFileName As String = String.Empty
        Dim sLocation As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).TempFileLocation

        Dim sReportDirName As String = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork) _
              .Portals.Portal(CMS.Library.Portal.GetPortalID()).Reports.Location

        'set the extraction directory using a guid to ensure it is unique
        sDocumentExtractionDirectory = sLocation & "/" & sUniqueDirectory

        'make SAM call with request parameters, sFileName will contain the name of the file we need to display
        sFileName = oWebService.GetReport(sReportName, NexusProvider.DocumentFormatType.PDF,
            oParametersCollection, sDocumentExtractionDirectory)

        Return sFileName
    End Function

    Public Sub ArchiveReportToSharePoint()
        'Dim oQuote As NexusProvider.Quote
        'oQuote = Session(CNQuote)
        Dim sPartyKey = Request.QueryString("PartyKey")
        Dim xlJob As XElement =
      <BACKGROUND_JOB>
          <JOB jobtype="DOCUPACK">
              <PARAMETERS>
                  <PARAMETER name="destination" value="archive"/>
                  <PARAMETER name="archive" value="true"/>
                  <PARAMETER name="PartyCnt" value=<%= sPartyKey %>/>
                  <PARAMETER name="type" value="report"/> **Added parameter for Archiving Reports
              </PARAMETERS>
          </JOB>
      </BACKGROUND_JOB>

        Dim xlPath As XElement = <PARAMETER name="Path" value=<%= sFileReportName %>/>
        xlJob.Element("JOB").Element("PARAMETERS").Add(xlPath)
        'we need to specify format
        Dim sFileType As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DocumentFormat.ToLower()
        xlJob.Element("JOB").Element("PARAMETERS").Add(New XElement(<PARAMETER name="OutputFormat" value=<%= sFileType.ToUpper %>/>))
        'documents to generate so specify the document template code
        Dim sOutputFileName As String = Right(sFileReportName.ToString(), Len(sFileReportName.ToString()) - InStrRev(sFileReportName.ToString(), "\"))
        sOutputFileName = Left(sOutputFileName, sOutputFileName.LastIndexOf("."))
        Dim xlDestinationFileName As XElement = <PARAMETER name="DestinationFilename" value=<%= ReplaceSplCharacters(sOutputFileName) %>/>
        xlJob.Element("JOB").Element("PARAMETERS").Add(xlDestinationFileName)

        Dim strJob As String = xlJob.ToString 'this will be used as input to the SAM call
        Dim sDescription As String = "Archive report"
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        'call SAM to queue the docs for Archiving
        Dim iBackgroundJobID As Integer = oWebService.CreateBackgroundJob(sDescription, strJob, Now.Date)
        If Request.QueryString("PostBack") IsNot Nothing Then
            If Request.QueryString("PostBack").ToUpper = "TRUE" Then
                Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
                'refresh the parent page on postback with event argument RefreshGrid  
                Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
            End If
        End If
        'close the modal page
        Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)

    End Sub

    Private Shared Function ReplaceSplCharacters(ByRef str As String) As String
        Dim illegalChars As Char() = ":~""#%&*<>?/\{}|.".ToCharArray()
        Dim ext As String
        Dim fName As String
        If str.LastIndexOf(".") = -1 Then
            fName = str
            ext = ""
        Else
            ext = str.Substring(str.LastIndexOf("."))
            fName = str.Substring(0, str.LastIndexOf("."))
        End If


        Dim sb As New System.Text.StringBuilder

        For Each ch As Char In fName
            If Array.IndexOf(illegalChars, ch) = -1 Then
                sb.Append(ch)
            End If
        Next
        Return sb.ToString() & IIf(ext.Length > 1, ext, "")
    End Function
End Class

