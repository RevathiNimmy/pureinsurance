Imports System.IO
Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows
Imports System.Collections.ObjectModel
Imports FileUpload.FileUploadService
Imports System.Windows.Browser
Imports System.ServiceModel
Imports System.ServiceModel.Channels

Partial Public Class MainPage
    Inherits UserControl

    Private Sub MainPage_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        'initialise the FilesToUpload collection
        Me.FilesToUpload = New List(Of FileInfo)
    End Sub

    Public Sub New()
        InitializeComponent()
    End Sub


    Dim _FilesToUpload As List(Of FileInfo)
    ''' <summary>
    ''' Holds queue of files which have been dropped on control and should be uploaded
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FilesToUpload As List(Of FileInfo)
        Get
            Return _FilesToUpload
        End Get
        Set(ByVal value As List(Of FileInfo))
            _FilesToUpload = value
        End Set
    End Property

    ''' <summary>
    ''' Handles drop events from control, checks each file exists and adds it to the queue if it does
    ''' Finally calls UploadFiles to trigger the upload process
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FileDrop(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs)
        Dim fi() As FileInfo = CType(e.Data.GetData(DataFormats.FileDrop), FileInfo())
        Dim fileService As New FileUploadService.FileUploadClient

        'Add dropped files to the upload queue
        If fi IsNot Nothing Then
            For Each fInfo As FileInfo In fi
                AddFileToUpload(fInfo)
            Next
        End If
        'Call UploadFiles to process the queue
        UploadFiles()
    End Sub

    ''' <summary>
    ''' Add files which have been dropped onto the control to the upload queue, first checking that they have not already been added
    ''' </summary>
    ''' <param name="file"></param>
    ''' <remarks></remarks>
    Private Sub AddFileToUpload(ByVal file As FileInfo)
        Dim fi_exists As FileInfo = Me.FilesToUpload.Where(Function(fi) fi.Name.Equals(file.Name)).FirstOrDefault()

        If fi_exists IsNot Nothing Then
            'the file is already in the queue so don't add it again
            Return
        End If
        Me.FilesToUpload.Add(file)
    End Sub

    Private _uploadInProgress As Boolean = False

    ''' <summary>
    ''' Upload files which have been added to the queue
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UploadFiles()
        Dim fileUploadService As FileUploadClient
        'set the endpoint address dynamically
        Dim sEndpoint = Application.Current.Host.Source.AbsoluteUri 'returns the location of the xap when deployed
        sEndpoint = sEndpoint.Substring(0, sEndpoint.LastIndexOf("/")) 'takes off the file.xap bit
        sEndpoint = sEndpoint.Substring(0, sEndpoint.LastIndexOf("/")) & "/services/FileUpload.svc" 'upload service must be in ~/services/FileUpload.svc for this to work

        Dim elements = New List(Of BindingElement)()
        elements.Add(New BinaryMessageEncodingBindingElement())

        If System.Windows.Browser.HtmlPage.Document.DocumentUri.Scheme.StartsWith("https") Then
            elements.Add(New HttpsTransportBindingElement())
        Else
            elements.Add(New HttpTransportBindingElement())
        End If

        '  elements.Add(New HttpTransportBindingElement())
        Dim binding = New CustomBinding(elements)

        fileUploadService = New FileUploadClient(binding, New EndpointAddress(sEndpoint))
        fileUploadService.Endpoint.Binding.OpenTimeout = TimeSpan.FromMinutes(10)
        fileUploadService.Endpoint.Binding.SendTimeout = TimeSpan.FromMinutes(10)


        AddHandler fileUploadService.InsertFileCompleted, AddressOf fileUploadComplete

        'pick the first file from the filestoupload collection
        Dim fi As FileInfo = Me.FilesToUpload(0)
        'check that we're not already in the middle of a file upload, if not then upload the first file
        If Not _uploadInProgress Then
            Dim oFile As WebFile = New WebFile() With {.FileName = fi.Name, .FileContent = FileInfoHelper.GetFileBytes(fi)}
            txtDrop.FontSize = "11"
            txtDrop.Text = "Uploading file " & fi.Name.ToString & "(" & Me.FilesToUpload.Count - 1 & " files remaining)"
            fileUploadService.InsertFileAsync(oFile)
            _uploadInProgress = True
        End If
    End Sub

    ''' <summary>
    ''' Handles the completion of a file upload. Updates display and 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub fileUploadComplete(ByVal sender As Object, ByVal e As InsertFileCompletedEventArgs)
        If e.Error IsNot Nothing Then
            txtDrop.Text = "Error uploading files: " & e.Error.Message
        End If
        'reset marker so that UploadFiles will process the queue when it is run again
        _uploadInProgress = False
        'remove the uploaded file from the list
        Dim fi_uploaded As FileInfo = Me.FilesToUpload.Where(Function(fi) fi.Name.Equals(CType(e.Result, String))).FirstOrDefault()
        Me.FilesToUpload.Remove(fi_uploaded)
        'Check if there is anything to upload, if there is then call UploadFiles otherwise display a message to indicate that uploading is completed
        If Me.FilesToUpload.Count > 0 Then
            'call UploadFiles to process anything that remains in the list
            UploadFiles()
        Else
            'lstDrop.Items.Add("Queue completed processing")
            'reset the display of the label - we're ready for more files
            txtDrop.Text = "Drag Files Here to Upload"
            txtDrop.FontSize = "22"
            HtmlPage.Window.Invoke("refreshFilelist", Nothing)
        End If
    End Sub
End Class