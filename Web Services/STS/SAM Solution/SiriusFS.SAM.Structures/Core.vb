Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports System.Collections.Generic

Namespace Core
#Region " CoreBase"
    Public MustInherit Class CoreBaseIn
        Public BranchCode As String = ""
    End Class
    Public MustInherit Class CoreBaseOut
        Public STSError As STSErrortype
    End Class
#End Region

#Region " GetList"

    ' Enum used for the GetList method. Used to identify the List Type
    ' These constants have the same values as those defined in GISSharedPropertyConstants
    Public Enum STSListType As Integer
        Missing = 0
        GisList = 1
        PMLookup = 2
        UserDefinedTable = 3
    End Enum

    ' Enum used for the GetList method. Used to identify the List Type
    ' These constants have the same values as those defined in GISSharedPropertyConstants
    Public Enum GetListFilterType As Integer
        Missing = 0
        Description = 1
        Code = 2
        Id = 3
    End Enum

    ' The Following Classes Define the Input and Output Parameters for the GetList Method 
    Public Class GetListInput
        Inherits CoreBaseIn

        ' In ONLY Parameters
        Public ListType As STSListType = STSListType.Missing
        Public ListCode As String = ""
        'Start (Sriram P) - (Tech Spec GetList.doc) - (7.1.3.2.3)
        Public ExcludeDeletedRecords As Boolean = False
        Public ExcludeEffectiveDate As Boolean = False
        Public ParentFieldName As String = ""
        Public ParentFieldValue As Int32 = Nothing
        'End (Sriram P) - (Tech Spec GetList.doc) - (7.1.3.2.3)
        Public FilterType As GetListFilterType = GetListFilterType.Missing
        Public FilterValue As String = ""
        Public EffectiveDate As DateTime = Nothing
        Public Version As Integer = 0
        Public SpuICCSParameters As Dictionary(Of String, String)
        Public SpuICCSName As String = ""
        Public UseCache As Boolean = False

        'whereClause Parameters start
        Public valWhereClause As System.Collections.ObjectModel.Collection(Of BaseListFilterOptions)
        'whereClause end

    End Class

    Public Class GetListOutput
        Inherits CoreBaseOut

        ' Out ONLY Parameters
        Public ListItems As DataSet

    End Class

    Public Class ICCSParam

        Private nameField As String
        Private valueField As String

        Public Property Name() As String
            Get
                Return Me.nameField
            End Get
            Set(ByVal value As String)
                Me.nameField = value
            End Set
        End Property

        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set(ByVal value As String)
                Me.valueField = value
            End Set
        End Property

    End Class
#End Region

#Region " GetDatasetDefinition"

    Public Class GetDatasetDefinitionInput
        Inherits CoreBaseIn

        ' In ONLY Parameters
        Public DataModelCode As String = ""

    End Class

    Public Class GetDatasetDefinitionOutput
        Inherits CoreBaseOut

        ' Out ONLY Parameters
        Public XMLDatasetDefinition As String = ""

    End Class

#End Region

    Public Class BaseListFilterOptions

        Private columnNameField As String
        Private filterOperatorField As String
        Private filterValueField As String

        Public Property ColumnName() As String
            Get
                Return Me.columnNameField

            End Get
            Set(ByVal value As String)
                Me.columnNameField = value

            End Set
        End Property

        Public Property FilterOperator() As String
            Get
                Return Me.filterOperatorField

            End Get
            Set(ByVal value As String)
                Me.filterOperatorField = value
            End Set
        End Property

        Public Property FilterValue() As String
            Get
                Return Me.filterValueField

            End Get
            Set(ByVal value As String)
                Me.filterValueField = value
            End Set
        End Property

    End Class

End Namespace
