Namespace Media

    Public Class MediaCategory
        Private iCategory_id As Integer
        Private sCategory_name As String
        Private iParent_id As Integer
        Private iPortal_id As Integer
        Private iDepth As Integer
        Private iNoItems As Integer


        Public Sub New(ByVal catgeory_id As Integer, ByVal category_name As String, ByVal parent_id As Integer, ByVal portal_id As Integer, ByVal depth As Integer, ByVal no_items As Integer)
            iCategory_id = catgeory_id
            sCategory_name = category_name
            iParent_id = parent_id
            iPortal_id = portal_id
            iDepth = depth
            iNoItems = no_items
        End Sub

        Public ReadOnly Property NoItems() As Integer
            Get
                Return iNoItems
            End Get
        End Property

        Public ReadOnly Property category_id() As Integer
            Get
                Return iCategory_id
            End Get
        End Property

        Public ReadOnly Property category_name() As String
            Get
                Return sCategory_name
            End Get
        End Property

        Public ReadOnly Property parent_id() As Integer
            Get
                Return iParent_id
            End Get
        End Property

        Public ReadOnly Property portal_id() As Integer
            Get
                Return iPortal_id
            End Get
        End Property

        Public ReadOnly Property depth() As Integer
            Get
                Return iDepth
            End Get
        End Property

    End Class

End Namespace