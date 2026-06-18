' --------------------------------------------------------------------------------------------------------------------
' <copyright company="Dataract">
'   Copyright (c) Dataract. All rights reserved.
' </copyright>
' --------------------------------------------------------------------------------------------------------------------

#Region "Using"

Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Globalization
Imports System.Linq

Imports Dataract.FW
Imports Dataract.FW.Communication
Imports Dataract.FW.Core.Adapters
Imports Dataract.FW.Core.Search
Imports Dataract.FW.Interfaces.Services
Imports Dataract.FW.Search
Imports Dataract.FW.Services.Wcf
Imports Dataract.e5.Domain


#End Region

Namespace IntegrationProxyLayer
    ''' <summary>
    ''' Service proxy access
    ''' </summary>
    Friend NotInheritable Class ServiceProxy
#Region "Public Methods"

        ''' <summary>
        ''' Gets the Service Proxy
        ''' </summary>
        ''' <typeparam name="TService">The type of the service.</typeparam>
        ''' <typeparam name="TBoxing">The service interface to return.</typeparam>
        ''' <returns>
        ''' The service of type TService.
        ''' </returns>
        Public Function GetProxy(Of TService As IService, TBoxing As IService)() As TBoxing
            Dim proxy = New BasicWcfServiceAdapter().GetInstance(Of TService, TBoxing)()
            Enforce.ArgumentNotNull(Of IService)(proxy, [String].Format(CultureInfo.CurrentCulture, "Unable to retrieve IPersistenceService instance of {0}", GetType(TService).Name))
            Return proxy
        End Function

        ''' <summary>
        ''' Initializes a query builder for service query requests
        ''' </summary>
        ''' <typeparam name="TEntity">The type of TEntity</typeparam>
        ''' <param name="siteCollectionId">The site collection id.</param>
        ''' <param name="mode">The fetching mode</param>
        ''' <returns>
        ''' The query builder.
        ''' </returns>
        Public Function InitializeQueryBuilder(Of TEntity As e5AbstractEntity)(ByVal siteCollectionId As Guid, Optional ByVal mode As FetchingMode = FetchingMode.Eager) As QueryBuilder(Of TEntity)
            Dim q = New QueryBuilder(Of TEntity)().EqualsTo(Function(e) e.SiteCollectionId, siteCollectionId)
            q.Query.FetchMode = mode
            Return q
        End Function

        ''' <summary>
        ''' Retrieves the specified request.
        ''' </summary>
        ''' <typeparam name="TService">The type of the service.</typeparam>
        ''' <param name="request">The request.</param>
        ''' <returns></returns>
        Public Function Retrieve(Of TService As IService)(ByVal request As RetrieveRequest) As Response
            Dim proxy = Me.GetProxy(Of TService, IRetrieveOperation)()
            Dim response = proxy.Retrieve(request)
            Return response
        End Function

        ''' <summary>
        ''' Saves the specified request.
        ''' </summary>
        ''' <param name="request">The request.</param>
        ''' <returns></returns>
        Public Function Save(Of TService As IService)(ByVal request As SaveRequest) As Response
            Dim proxy = Me.GetProxy(Of TService, ISaveOperation)()
            Dim response = proxy.Save(request)
            Return response
        End Function

        ''' <summary>
        ''' Validates a service response and returns the expected service value
        ''' </summary>
        ''' <typeparam name="TEntity">The type of the entity.</typeparam>
        ''' <param name="response">The response.</param>
        ''' <exception cref="System.ApplicationException">Expected a service response object but received a null reference</exception>
        Public Function ValidateAndGetManyResponse(Of TEntity As Class)(ByVal response As Response) As ICollection(Of TEntity)
            If response Is Nothing Then
                Throw New ApplicationException("Expected a service response object but received a null reference")
            End If

            If response.Notifications.Count > 0 Then
                Throw New ApplicationException([String].Format("Expected a service response to contain no notifications but got the message: {0}", response.Notifications.First().Description))
            End If

            If response.Value Is Nothing Then
                Throw New ApplicationException("Expected a service response to contain a Value but received a null reference")
            End If

            If TryCast(response.Value, Collection(Of TEntity)) Is Nothing Then
                Throw New ApplicationException([String].Format("Expected a service response to contain a Value that conforms to type {0} but received {1}", GetType(TEntity), response.Value.[GetType]()))
            End If

            Return response.ValuesOfType(Of TEntity)()
        End Function

        ''' <summary>
        ''' Validates a service response and returns the expected service value
        ''' </summary>
        ''' <typeparam name="TEntity">The type of the entity.</typeparam>
        ''' <param name="response">The response.</param>
        ''' <exception cref="System.ApplicationException">Expected a service response object but received a null reference</exception>
        Public Function ValidateAndGetSingleResponse(Of TEntity As Class)(ByVal response As Response) As TEntity
            If response Is Nothing Then
                Throw New ApplicationException("Expected a service response object but received a null reference")
            End If

            If response.Notifications.Count > 0 Then
                Throw New ApplicationException([String].Format("Expected a service response to contain no notifications but got the message: {0}", response.Notifications.First().Description))
            End If

            If response.Value Is Nothing Then
                Throw New ApplicationException("Expected a service response to contain a Value but received a null reference")
            End If

            If TryCast(response.Value, TEntity) Is Nothing Then
                Throw New ApplicationException([String].Format("Expected a service response to contain a Value that conforms to type {0} but received {1}", GetType(TEntity), response.Value.[GetType]()))
            End If

            Return response.ValueOfType(Of TEntity)()
        End Function

#End Region
    End Class
End Namespace
