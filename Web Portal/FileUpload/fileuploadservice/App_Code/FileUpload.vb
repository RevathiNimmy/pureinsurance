Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.Web.Configuration
Imports System.IO

''' <summary>
''' Main contract for the file upload service
''' </summary>
''' <remarks></remarks>
<ServiceContract(Namespace:="Pure.Portals.FileUpload")>
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)>
Public Class FileUpload

    ''' <summary>
    ''' Handle the upload of a file from silverlight control
    ''' </summary>
    ''' <param name="file"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()>
    Public Function InsertFile(ByVal file As WebFile) As String
        FileHandler.HandleFile(file)
        'return the file name so that it can be removed from the queue
        Return file.FileName
    End Function

End Class
