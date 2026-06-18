Option Strict On
Option Explicit On 
Imports Microsoft.ApplicationBlocks.ExceptionManagement

Namespace DataSetControl
    Public Class Node
        ' ***************************************************************** '
        ' Class Name: Node
        '
        ' Date: 01/03/2000
        '
        ' Description:
        '
        ' Edit History:
        '
        ' ***************************************************************** '


        ' The Whole Dataset
        Private m_oDataset As DataSetControl.Application

        ' The Root Element within the DataSet
        Private m_oRoot As MSXML2.IXMLDOMElement

        ' The Quote Key. Spaces if this is a Risk Node
        Private m_sQuoteKey As String

        ' The Last Node Name Accessed
        ' This is needed by the Value method so that it can know
        ' the PropertyName required
        Private m_sLastNodeName As String

        Friend Property Root() As MSXML2.IXMLDOMElement
            Get
                Root = m_oRoot
            End Get
            Set(ByVal Value As MSXML2.IXMLDOMElement)
                If IsReference(Value) And Not TypeOf Value Is String Then
                    m_oRoot = Value
                Else
                    m_oRoot = Value
                End If
            End Set
        End Property

        Friend Property QuoteKey() As String
            Get
                QuoteKey = m_sQuoteKey
            End Get
            Set(ByVal Value As String)
                m_sQuoteKey = Value
            End Set
        End Property


        Friend Property Dataset() As DataSetControl.Application
            Get
                Dataset = m_oDataset
            End Get
            Set(ByVal Value As DataSetControl.Application)
                m_oDataset = Value
            End Set
        End Property


        Public ReadOnly Property Item(ByVal v_sNodename As String, Optional ByVal v_lItemNum As Integer = 1) As DataSetControl.Node

            Get

                Dim oDomNodes As MSXML2.IXMLDOMNodeList
                Dim oDomNode As MSXML2.IXMLDOMNode
                Dim oNewNode As DataSetControl.Node

                Dim bNotFound As Boolean
                Dim lReturn As Integer
                Dim sOIKey As String
                Dim sPropertyTag As String

                Try

                    'UPGRADE_NOTE: Object oDomNode may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
                    oDomNode = Nothing

                    ' RFC300300 - Uppercase the Node Name
                    v_sNodename = UCase(Trim(v_sNodename))

                    ' Get the nodes from the current root with this tag name
                    oDomNodes = m_oRoot.getElementsByTagName(v_sNodename)
                    If (oDomNodes Is Nothing = True) Then
                        Exit Property
                    End If

                    If (oDomNodes.length < 1) Then

                        ' We are (probably) Looking for a Property of the current Object
                        ' so return the Object Reference, i.e. The current Node
                        Item = Me
                        ' Store the Last Node Name asked for, this will be used by the Value method
                        m_sLastNodeName = v_sNodename

                        Exit Property

                    End If

                    ' Get the 1st, 2nd etc node as specified
                    Try
                        oDomNode = oDomNodes.item(v_lItemNum - 1)
                    Catch
                    End Try

                    If (oDomNode Is Nothing = True) Then
                        Err.Raise(100, System.Reflection.Assembly.GetExecutingAssembly.GetName.Name & ".Node.Item", "Instance Number " & v_lItemNum & " of Node " & v_sNodename & " Not Found")
                        Exit Property
                    End If

                    ' Create a new Node
                    oNewNode = New DataSetControl.Node

                    ' Set the New Nodes Dataset
                    oNewNode.Dataset = Dataset

                    ' Set the root of the new node to be this element just found
                    oNewNode.Root = CType(oDomNode, MSXML2.IXMLDOMElement)

                    ' Set the Quote Key
                    oNewNode.QuoteKey = QuoteKey

                    ' Return the New Node Created
                    Item = oNewNode

                    'UPGRADE_NOTE: Object oNewNode may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
                    oNewNode = Nothing
                    'UPGRADE_NOTE: Object oDomNodes may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
                    oDomNodes = Nothing
                    'UPGRADE_NOTE: Object oDomNode may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
                    oDomNode = Nothing

                Catch ex As Exception

                    Dim s As String

                    Item = Nothing
                    oDomNodes = Nothing
                    oDomNode = Nothing

                    s = "STSClient.Node: Item (Get): Name=" & v_sNodename & ": Error=" & ex.Message
                    Dim ErrEx As New Exception(s, ex)
                    ExceptionManager.Publish(ErrEx)
                    Throw ErrEx

                End Try

            End Get
        End Property


        Public Property Value() As Object
            Get

                Try

                    ' Return the Value
                    'UPGRADE_WARNING: Couldn't resolve default property of object m_oDataset.PropertyValue(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object Value. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    Value = m_oDataset.PropertyValue(m_oRoot, m_sLastNodeName)

                Catch ex As Exception

                    Dim s As String

                    s = "STSClient.Node: Value (Get): Name=" & m_sLastNodeName & ": Error=" & ex.Message
                    Dim ErrEx As New Exception(s, ex)
                    ExceptionManager.Publish(ErrEx)
                    Throw ErrEx

                End Try

            End Get
            Set(ByVal Value As Object)

                Try

                    Dim sObjectName As String = ""

                    ' RAW 22/01/2004 : CQ3720 : added
                    If Not (m_oRoot Is Nothing) Then
                        sObjectName = m_oRoot.nodeName
                    End If

                    ' Set the Property Value
                    m_oDataset.PropertyValueSet(v_sObjectName:=sObjectName, r_oObjectInst:=m_oRoot, v_sPropertyName:=m_sLastNodeName, v_vPropertyValue:=Value)


                Catch ex As Exception

                    Dim s As String

                    s = "STSClient.Node: Value (Set): Name=" & m_sLastNodeName & ": Error=" & ex.Message
                    Dim ErrEx As New Exception(s, ex)
                    ExceptionManager.Publish(ErrEx)
                    Throw ErrEx

                End Try

            End Set
        End Property

        Public ReadOnly Property Count(ByVal v_sChildNodename As String) As Integer
            Get

                Dim oDomNodes As MSXML2.IXMLDOMNodeList

                Try

                    ' RFC300300 - Uppercase the Node Name
                v_sChildNodename = UCase(Trim(v_sChildNodename))

                ' Get the nodes from the current root with this tag name
                oDomNodes = m_oRoot.getElementsByTagName(v_sChildNodename)
                If (oDomNodes Is Nothing = True) Then
                    Count = 0
                End If

                ' Return the number of nodes
                Count = oDomNodes.length

                'UPGRADE_NOTE: Object oDomNodes may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
                oDomNodes = Nothing

                Catch ex As Exception

                    Dim s As String

                    s = "STSClient.Node: Value (Set): Name=" & m_sLastNodeName & ": Error=" & ex.Message
                    Dim ErrEx As New Exception(s, ex)
                    ExceptionManager.Publish(ErrEx)

                    Count = 0
                
                End Try

            End Get
        End Property

        ' ***************************************************************** '
        '
        ' Name: NewObject
        '
        ' Description:
        '
        ' History: 08/03/2000 RFC - Created.
        '
        ' ***************************************************************** '
        Public Sub NewObject(ByVal v_sObjectName As String)

            Dim sParentObjectName As String
            Dim sParentOIKey As String
            Dim lReturn As Integer
            Dim sOIKey As String
            Dim s As String

            Try

                ' Get the Parent Object Name
                sParentObjectName = m_oRoot.nodeName

                ' Get the Parent OIKey
                'UPGRADE_WARNING: Couldn't resolve default property of object m_oRoot.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                sParentOIKey = CType(m_oRoot.getAttribute(ACXMLAttribOIKey), String)

                ' Create the Instance
                If (QuoteKey = "") Then

                    lReturn = m_oDataset.NewObjectInstance(v_sObjectName:=v_sObjectName, r_sOIKey:=sOIKey, v_sParentOIKey:=sParentOIKey)

                Else

                    lReturn = m_oDataset.NewObjectInstance(v_sObjectName:=v_sObjectName, r_sOIKey:=sOIKey, v_sParentOIKey:=sParentOIKey, v_sQuoteKey:=QuoteKey)

                End If

                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                    s = "STSClient.Node: NewObject: Name=" & v_sObjectName & ""
                    Dim ErrEx As New Exception(s)
                    ExceptionManager.Publish(ErrEx)
                    Throw ErrEx

                End If

            Catch ex As Exception

                s = "STSClient.Node: NewObject: Name=" & v_sObjectName & ": Error=" & ex.Message
                Dim ErrEx As New Exception(s, ex)
                ExceptionManager.Publish(ErrEx)
                Throw ErrEx

            End Try

        End Sub


        ' ***************************************************************** '
        '
        ' Name: DeleteObject
        '
        ' Description:
        '
        ' History: 16/05/2001 RFC - Created.
        '
        ' ***************************************************************** '
        Public Sub DeleteObject()

            Dim lReturn As Integer
            Dim sObjectName As String
            Dim sOIKey As String
            Dim s As String

            Try

                ' Get the OIKey
                'UPGRADE_WARNING: Couldn't resolve default property of object Root.getAttribute(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                sOIKey = CType(Root.getAttribute(ACXMLAttribOIKey), String)

                ' Get the Object Name
                sObjectName = Root.nodeName

                lReturn = m_oDataset.DelObjectInstance(sObjectName, sOIKey)
                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                    s = "STSClient.Node: DeleteObject: Name=" & sObjectName
                    Dim ErrEx As New Exception(s)
                    ExceptionManager.Publish(ErrEx)
                    Throw ErrEx

                End If

            Catch ex As Exception

                s = "STSClient.Node: DeleteObject: Name=" & sObjectName & ": Error=" & ex.Message
                Dim ErrEx As New Exception(s, ex)
                ExceptionManager.Publish(ErrEx)
                Throw ErrEx

            End Try


        End Sub


        ' ***************************************************************** '
        '
        ' Name: ChildObjects
        '
        ' Description: Returns the List of Child Objects for this Object.
        '
        ' History: 05/04/2000 RFC - Created.
        '
        ' ***************************************************************** '
        Public Function ChildObjects() As Object

            Dim lReturn As Integer
            Dim vChildObjects() As String
            Dim sObjectName As String

            Try

                'UPGRADE_WARNING: Couldn't resolve default property of object ChildObjects. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                ChildObjects = Nothing

                ' Get the Object Name
                sObjectName = m_oRoot.nodeName

                ' Get the Child Objects for this Object
                lReturn = m_oDataset.GetObjectDefDetails( _
                    v_sObjectName:=sObjectName, _
                    r_vChildObjectArray:=vChildObjects)
                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                    Dim s As String

                    s = "STSClient.Node: ChildObjects: Failed to get child objects for " & sObjectName
                    Dim ErrEx As New Exception(s)
                    ExceptionManager.Publish(ErrEx)
                    Throw ErrEx

                End If

                If (IsArray(vChildObjects)) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object vChildObjects. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object ChildObjects. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    ChildObjects = vChildObjects
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object ChildObjects. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    ChildObjects = Nothing
                End If

            Catch ex As Exception

                Dim s As String

                s = "STSClient.Node: ChildObjects: Failed to get child objects for " & sObjectName & ": Error=" & ex.Message
                Dim ErrEx As New Exception(s, ex)
                ExceptionManager.Publish(ErrEx)
                Throw ErrEx

            End Try

        End Function


        ' ***************************************************************** '
        '
        ' Name: Properties
        '
        ' Description: Returns the List of Properties for this Object.
        '
        ' History: 05/04/2000 RFC - Created.
        '
        ' ***************************************************************** '
        Public Function Properties() As String()

            Dim lReturn As Integer
            Dim vProperties(,) As Object
            Dim vOutProperties() As String
            Dim sObjectName As String
            Dim lRow As Integer
            Dim lFrom As Integer
            Dim lTo As Integer

            Try

                'UPGRADE_WARNING: Couldn't resolve default property of object Properties. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                Properties = Nothing

                ' Get the Object Name
                sObjectName = m_oRoot.nodeName

                ' Get the Properties for this Object
                lReturn = m_oDataset.GetObjectDefDetails( _
                    v_sObjectName:=sObjectName, _
                    r_vPropertyArray:=vProperties)
                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    Err.Raise(131, "Properties", "Failed to get the Properties for object " & sObjectName)
                End If

                If (IsArray(vProperties)) Then

                    ' This will return a 2d array, we want to only return the Property Names

                    lFrom = LBound(vProperties, 2)
                    lTo = UBound(vProperties, 2)

                    ReDim vOutProperties(lTo)

                    For lRow = lFrom To lTo
                        'UPGRADE_WARNING: Couldn't resolve default property of object vProperties(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object vOutProperties(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                        vOutProperties(lRow) = CType(vProperties(0, lRow), String)
                    Next lRow

                    'UPGRADE_WARNING: Couldn't resolve default property of object vOutProperties. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object Properties. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    Properties = vOutProperties

                Else

                    'UPGRADE_WARNING: Couldn't resolve default property of object Properties. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    Properties = Nothing

                End If

                'UPGRADE_NOTE: Object vProperties may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
                vProperties = Nothing
                'UPGRADE_NOTE: Object vOutProperties may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
                vOutProperties = Nothing

            Catch ex As Exception

                Dim s As String

                s = "STSClient.Node: Properties: Failed to get properties for " & sObjectName & ": Error=" & ex.Message
                Dim ErrEx As New Exception(s, ex)
                ExceptionManager.Publish(ErrEx)
                Throw ErrEx

            End Try

        End Function


        'UPGRADE_NOTE: Class_Terminate was upgraded to Class_Terminate_Renamed. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1061"'
        Private Sub Class_Terminate_Renamed()
            'UPGRADE_NOTE: Object Root may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            Root = Nothing
            'UPGRADE_NOTE: Object Dataset may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1029"'
            Dataset = Nothing
        End Sub
        Protected Overrides Sub Finalize()
            Class_Terminate_Renamed()
            MyBase.Finalize()
        End Sub
    End Class
End Namespace
