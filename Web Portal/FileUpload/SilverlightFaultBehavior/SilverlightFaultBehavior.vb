Imports System.ServiceModel.Description
Imports System.ServiceModel.Dispatcher
Imports System.ServiceModel.Channels
Imports System.ServiceModel.Configuration
Imports System.ServiceModel

Public Class SilverlightFaultBehavior : Inherits BehaviorExtensionElement : Implements IEndpointBehavior

    Public Sub ApplyDispatchBehavior(ByVal endpoint As ServiceEndpoint, ByVal endpointDispatcher As EndpointDispatcher)
        Dim inspector As New SilverlightFaultMessageInspector()
        endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector)
    End Sub

    ' The following methods are stubs and not relevant. 
    Public Sub AddBindingParameters(ByVal endpoint As ServiceEndpoint, ByVal bindingParameters As BindingParameterCollection)
    End Sub

    Public Sub ApplyClientBehavior(ByVal endpoint As ServiceEndpoint, ByVal clientRuntime As ClientRuntime)
    End Sub

    Public Sub Validate(ByVal endpoint As ServiceEndpoint)
    End Sub

    Public Overrides ReadOnly Property BehaviorType() As System.Type
        Get
            Return GetType(SilverlightFaultBehavior)
        End Get
    End Property

    Protected Overrides Function CreateBehavior() As Object
        Return New SilverlightFaultBehavior()
    End Function

    Public Class SilverlightFaultMessageInspector
        Implements IDispatchMessageInspector


        Public Sub BeforeSendReply(ByRef reply As Message, ByVal correlationState As Object)
            If reply.IsFault Then
                Dim [property] As New HttpResponseMessageProperty()
                ' Here the response code is changed to 200.
                [property].StatusCode = System.Net.HttpStatusCode.OK

                reply.Properties(HttpResponseMessageProperty.Name) = [property]
            End If
        End Sub

        Public Function AfterReceiveRequest(ByRef request As System.ServiceModel.Channels.Message, ByVal channel As System.ServiceModel.IClientChannel, ByVal instanceContext As System.ServiceModel.InstanceContext) As Object Implements System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest
            ' Do nothing to the incoming message.
            Return Nothing
        End Function

        Public Sub BeforeSendReply1(ByRef reply As System.ServiceModel.Channels.Message, ByVal correlationState As Object) Implements System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply

        End Sub
    End Class

    Public Sub AddBindingParameters1(ByVal endpoint As System.ServiceModel.Description.ServiceEndpoint, ByVal bindingParameters As System.ServiceModel.Channels.BindingParameterCollection) Implements System.ServiceModel.Description.IEndpointBehavior.AddBindingParameters

    End Sub

    Public Sub ApplyClientBehavior1(ByVal endpoint As System.ServiceModel.Description.ServiceEndpoint, ByVal clientRuntime As System.ServiceModel.Dispatcher.ClientRuntime) Implements System.ServiceModel.Description.IEndpointBehavior.ApplyClientBehavior

    End Sub

    Public Sub ApplyDispatchBehavior1(ByVal endpoint As System.ServiceModel.Description.ServiceEndpoint, ByVal endpointDispatcher As System.ServiceModel.Dispatcher.EndpointDispatcher) Implements System.ServiceModel.Description.IEndpointBehavior.ApplyDispatchBehavior

    End Sub

    Public Sub Validate1(ByVal endpoint As System.ServiceModel.Description.ServiceEndpoint) Implements System.ServiceModel.Description.IEndpointBehavior.Validate

    End Sub
End Class
