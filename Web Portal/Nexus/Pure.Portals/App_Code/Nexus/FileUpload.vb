Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports Nexus.Utils
Imports Nexus.Library
Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports System.IO
Imports System.Runtime.Serialization
Imports System.ServiceModel.Description
Imports System.ServiceModel.Web
Imports System.Web.HttpContext
Imports Nexus.Constants

<ServiceContract(Namespace:="Pure.Portals.FileUpload")> _
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)> _
Public Class FileUpload

    ''' <summary>
    ''' Handle the upload of a file from silverlight control
    ''' </summary>
    ''' <param name="fileName"></param>
    ''' <param name="inputFile"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    <WebInvoke(UriTemplate:="/UploadFiles?fileName={fileName}")>
    Public Function UploadFiles(ByVal fileName As String, ByVal inputFile As Stream) As String
        'check user permissions before processing upload
        If Nexus.UserCanDoTask("UploadDocument") Then
            'check if the file is of an allowed type. if it's in config it's ok
            Dim oFileTypes As Config.FileTypes = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                .Portals.Portal(Portal.GetPortalID()).FileTypes
            'get the extension by spliting on . and taking the last element of the resulting array
            Dim sExtension As String = Split(fileName, ".")(Split(fileName, ".").Length - 1)
            Dim bAllowed As Boolean = False
            Dim FileType As Config.FileType
            For Each FileType In oFileTypes
                If FileType.Extension.ToUpper() = sExtension.ToUpper() Then
                    bAllowed = True
                    Exit For
                End If
            Next
            If bAllowed Then
                Try
                    FileHandler.HandleFile(inputFile, fileName, FileType.DocType, True)
                Catch ex As Exception
                    If Current.Session(CNFileUploadError) Is Nothing Then
                        Current.Session(CNFileUploadError) = "FilehandlerException,"
                    Else
                        If Current.Session(CNFileUploadError).ToString().IndexOf("FilehandlerException") = -1 Then
                            Current.Session(CNFileUploadError) = Current.Session(CNFileUploadError).ToString() + "FilehandlerException,"
                        End If
                    End If
                    Throw
                End Try
            Else
                If Current.Session(CNFileUploadError) Is Nothing Then
                    Current.Session(CNFileUploadError) = sExtension + "Extension,"
                Else
                    If Current.Session(CNFileUploadError).ToString().IndexOf(sExtension) = -1 And Current.Session(CNFileUploadError).ToString().IndexOf("FilehandlerException") = -1 Then
                        Current.Session(CNFileUploadError) = Current.Session(CNFileUploadError).ToString() + sExtension + "Extension,"
                    End If
                End If
                'todo - should update silverlight control to take a message back that the upload is of a type which isn't allowed. Just ignore it for now
            End If
        Else
            'throw an exception which can be handled further up the stack
            Throw New System.Exception("File upload permission denied")
        End If
        'return the file name so that it can be removed from the queue
        Return fileName
    End Function

End Class


