Imports System.Xml
Imports System.Xml.Schema
Imports System.IO

Friend NotInheritable Class XmlDocument : Implements IEnumerable 
#Region "Fields"
    ' Shared instance of the xml schemaset
    Private Shared m_oSchemas As Xml.Schema.XmlSchemaSet = Nothing

    ' Standard fields
    Private m_sFilename As String
    Private m_sInterfaceName As String = String.Empty
    Private m_oXML As Xml.XmlDocument
    Private m_bAutoRecProcess As Boolean = False
#End Region

#Region "Properties"
    ''' <summary>
    ''' Gets the batch reference for this import file, if present
    ''' </summary>
    Public ReadOnly Property BatchReference() As String
        Get
            Return GetAttribute(m_oXML("IMPORT_HEADER"), "batch_reference")
        End Get
    End Property

    ''' <summary>
    ''' The filename relating to the xml file this object represents
    ''' </summary>
    Public ReadOnly Property Filename() As String
        Get
            Return m_sFilename
        End Get
    End Property

    ''' <summary>
    ''' Returns the header node for the import file
    ''' </summary>
    Public ReadOnly Property HeaderNode() As XmlElement
        Get
            Return m_oXML("IMPORT_HEADER")
        End Get
    End Property

    ''' <summary>
    ''' The name of the required interface for this file
    ''' </summary>
    Public ReadOnly Property InterfaceName() As String
        Get
            Return m_sInterfaceName
        End Get
    End Property

    Public Property IsAutoRecProcess() As Boolean
        Get
            Return m_bAutoRecProcess
        End Get
        Set(value As Boolean)
            m_bAutoRecProcess = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Function GetHeaderAttribute(ByVal sAttribute As String) As Object
        Return GetAttribute(m_oXML("IMPORT_HEADER"), sAttribute)
    End Function

    Private Function GetAttribute(ByVal oElement As XmlElement, ByVal sAttribute As String) As Object
        ' Check for attribute
        If oElement.HasAttribute(sAttribute) Then
            Return oElement.Attributes(sAttribute).Value
        Else
            Return Nothing
        End If
    End Function

    Private Sub SetAttribute(ByVal oElement As XmlElement, ByVal sAttribute As String, ByVal vValue As Object)
        ' Check for attribute
        If oElement.HasAttribute(sAttribute) Then
            oElement.Attributes(sAttribute).Value = vValue
        Else
            ' Create a new attribute.
            Dim newAttr As XmlAttribute = m_oXML.CreateAttribute(sAttribute)
            ' Append to element
            oElement.Attributes.Append(newAttr)
            newAttr.Value = vValue
        End If
    End Sub

    Public Sub Update(ByVal sBatchID As Integer, ByVal sBatchRef As String, ByVal dtImportDate As DateTime)

        ' Update xml doc
        SetAttribute(m_oXML("IMPORT_HEADER"), "batch_id", sBatchID)
        SetAttribute(m_oXML("IMPORT_HEADER"), "batch_reference", sBatchRef)
        SetAttribute(m_oXML("IMPORT_HEADER"), "date_imported", dtImportDate.ToString("yyyy-MM-dd'T'HH:mm:ss.fff"))

        Try
            m_oXML.Save(m_sFilename)
        Catch ex As Exception
            Throw New Exception("Unable to update import file with final batch id and import date.")
        End Try

    End Sub

    ''' <summary>
    ''' Validate the supplied file against the known schemas and determine which
    ''' interface should be used to process it
    ''' </summary>
    Public Sub Validate()
        ' Declare and populate xml reader settings
        Dim settings As XmlReaderSettings = New XmlReaderSettings()
        settings.Schemas = m_oSchemas
        settings.ValidationType = ValidationType.Schema
        settings.ValidationFlags = settings.ValidationFlags Or XmlSchemaValidationFlags.ProcessInlineSchema
        settings.ValidationFlags = settings.ValidationFlags Or XmlSchemaValidationFlags.ReportValidationWarnings
        AddHandler settings.ValidationEventHandler, AddressOf ValidationCallBack

        ' Create xml reader
        Dim reader As XmlReader = XmlReader.Create(m_sFilename, settings)

        ' Load and validate the xml
        Try
            ' Load xml
            m_oXML.Load(reader)

            Dim bAutoRecProcess As Boolean = False

            If m_oXML.DocumentElement IsNot Nothing Then
                If m_oXML.DocumentElement.Name = "n:PremiumStatementLoad" Then
                    IsAutoRecProcess = True
                    Exit Sub
                End If
            End If
            ' Check if schema validation succeeded
            If m_oXML.SchemaInfo.Validity = XmlSchemaValidity.Valid Then
                m_sInterfaceName = m_oXML("IMPORT_HEADER").Attributes("interface_name").Value
            Else
                ' Throw unrecognized exception
                Throw New Exception("Unrecognised XML format")
            End If
        Catch ex As Exception
            ' Throw validation error
            Throw New Exception("XML Validation Error", ex)
        Finally
            ' Ensure we close the reader
            If Not reader Is Nothing Then
                reader.Close()
            End If
        End Try
    End Sub
#End Region

#Region "Shared Methods"
    Private Shared Sub ValidationCallBack(ByVal sender As Object, ByVal args As ValidationEventArgs)
        If (args.Severity = XmlSeverityType.Warning) Then
            Throw New Exception("Matching schema not found. No validation occurred.", New Exception(args.Message))
        Else
            Throw New Exception(args.Message)
        End If
    End Sub
#End Region

#Region "Enumerator"
    ''' <summary>
    ''' Allows enumeration through all xml data nodes within the "IMPORT_HEADER" node
    ''' </summary>
    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return m_oXML("IMPORT_HEADER").ChildNodes.GetEnumerator
    End Function
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Creates a new instance of this class to process the specified filename
    ''' </summary>
    Public Sub New(ByVal sFilename As String)
        ' Store parameters
        m_sFilename = sFilename

        ' Create schemaset if not already
        If m_oSchemas Is Nothing Then
            m_oSchemas = New Xml.Schema.XmlSchemaSet()

            Dim sName As String = System.Reflection.Assembly.GetExecutingAssembly.GetModules()(0).FullyQualifiedName
            Dim sPath As String = System.IO.Path.GetDirectoryName(sName)
            ' Add schemas
            m_oSchemas.Add(Nothing, sPath & "\Payment_Import.xsd")
            m_oSchemas.Add(Nothing, sPath & "\Reference_Import.xsd")
            m_oSchemas.Add(Nothing, sPath & "\Receipt_Import.xsd")
            m_oSchemas.Add(Nothing, sPath & "\Instalments_Import.xsd")
            m_oSchemas.Add(Nothing, sPath & "\Bank_Reconciliation_Import.xsd")
            m_oSchemas.Add(Nothing, sPath & "\Cover_Note_Import.xsd")
            m_oSchemas.Add(Nothing, sPath & "\Exchange_Rates_Import.xsd")
            m_oSchemas.Add(Nothing, sPath & "\Mid_Import.xsd")
            m_oSchemas.Add(Nothing, sPath & "\Policy_BDX_Import.xsd")
            m_oSchemas.Add(Nothing, sPath & "\Premium_BDX_Import.xsd")
            m_oSchemas.Add(Nothing, sPath & "\Claim_BDX_Import.xsd")
            m_oSchemas.Add(Nothing, sPath & "\Cash_Allocation_Import.xsd")
            m_oSchemas.Add(Nothing, sPath & "\MID2_Import.xsd")
            m_oSchemas.Add(Nothing, sPath & "\Agent_Reconciliation_Import.xsd")
        End If

        ' Create a new xml doc and attach schemas
        m_oXML = New Xml.XmlDocument()
        m_oXML.Schemas.Add(m_oSchemas)
    End Sub
#End Region
End Class
