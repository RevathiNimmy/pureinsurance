Imports System.Configuration

Namespace Config

    Public Class Products : Inherits ConfigurationElementCollection

        Private cpcProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

        End Sub

        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

        Public Overrides ReadOnly Property CollectionType() As System.Configuration.ConfigurationElementCollectionType
            Get
                Return ConfigurationElementCollectionType.BasicMap
            End Get
        End Property

        Protected Overrides ReadOnly Property ElementName() As String
            Get
                Return "Product"
            End Get
        End Property

        Public ReadOnly Property GetProductByName(ByVal Name As String) As Product
            Get

                Dim i As Integer = MyBase.Count - 1
                Dim oProduct As New Product


                While (i >= 0)

                    If LCase(CType(MyBase.BaseGet(i), Product).Name) = LCase(Name) Then
                        oProduct = CType(MyBase.BaseGet(i), Product)
                    End If

                    i -= 1

                End While

                Return oProduct

            End Get
        End Property
        Public ReadOnly Property GetProductByCode(ByVal Code As String) As Product
            Get

                Dim i As Integer = MyBase.Count - 1
                Dim oProduct As New Product


                While (i >= 0)

                    If LCase(CType(MyBase.BaseGet(i), Product).ProductCode) = LCase(Code) Then
                        oProduct = CType(MyBase.BaseGet(i), Product)
                    End If

                    i -= 1

                End While

                Return oProduct

            End Get
        End Property
        '1.3
        Public ReadOnly Property GetAllowedAgentByName(ByVal Name As String) As String
            Get
                Dim i As Integer = MyBase.Count - 1
                Dim sAllowedAgent As String = String.Empty

                While (i >= 0 And sAllowedAgent = String.Empty)

                    If LCase(CType(MyBase.BaseGet(i), Product).Name) = LCase(Name) Then
                        sAllowedAgent = CType(MyBase.BaseGet(i), Product).AllowedAgent
                    End If

                    i -= 1

                End While

                Return sAllowedAgent

            End Get
        End Property


        'Public ReadOnly Property Product(ByVal DataModelCode As String) As Product
        '    Get
        '        Return MyBase.BaseGet(DataModelCode)
        '    End Get
        'End Property

        Public ReadOnly Property Product(ByVal ProductCode As String) As Product
            Get
                Return MyBase.BaseGet(ProductCode.ToUpper())
            End Get
        End Property

        Public ReadOnly Property Product(ByVal index As Integer) As Product
            Get
                Return MyBase.BaseGet(index)
            End Get
        End Property

        Public Function GetKey(ByVal index As Integer) As String
            Return CStr(MyBase.BaseGetKey(index))
        End Function

        Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
            Return New Product
        End Function

        Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
            Return CType(element, Product).ProductCode.ToUpper()
        End Function

    End Class

End Namespace
