Option Strict Off
Option Explicit On
Imports System.Xml
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Node_NET.Node")>
Public NotInheritable Class Node
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

    ' ************************************************
    ' Added to replace global variables 19/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************

    ' The Whole Dataset
    Private m_oDataset As cGISDataSetControl.Application

    ' The Root Element within the DataSet
    Private m_oRoot As XmlElement

    ' The Quote Key. Spaces if this is a Risk Node
    Private m_sQuoteKey As String = ""

    ' The Last Node Name Accessed
    ' This is needed by the Value method so that it can know
    ' the PropertyName required
    Private m_sLastNodeName As String = ""

    Private m_bStrict As Boolean

    Public Property Strict() As Boolean
        Get
            Return m_bStrict
        End Get
        Set(ByVal Value As Boolean)
            m_bStrict = Value

            If Not (m_oDataset Is Nothing) Then
                m_oDataset.Strict = Value
            End If
        End Set
    End Property

    Friend Property Root() As XmlElement
        Get
            Return m_oRoot
        End Get
        Set(ByVal Value As XmlElement)
            'Modified by Archana Tokas on 4/27/2010 9:59:07 AM no requirement of code found
            'If Microsoft.VisualBasic.Informations.IsReference(Value) And Not (TypeOf Value Is string) Then
            '	m_oRoot = Value
            'Else
            '	m_oRoot = Value
            'End If
            m_oRoot = Value

        End Set
    End Property

    Friend Property QuoteKey() As String
        Get
            Return m_sQuoteKey
        End Get
        Set(ByVal Value As String)
            m_sQuoteKey = Value
        End Set
    End Property

    Friend Property Dataset() As cGISDataSetControl.Application
        Get
            Return m_oDataset
        End Get
        Set(ByVal Value As cGISDataSetControl.Application)
            'Modified by Archana Tokas on 4/27/2010 9:59:21 AM no requirement of code found
            'If Microsoft.VisualBasic.Informations.IsReference(Value) And Not (TypeOf Value Is string) Then
            '	m_oDataset = Value
            'Else
            '	m_oDataset = Value
            'End If
            m_oDataset = Value

        End Set
    End Property

    Public ReadOnly Property Item(ByVal v_sNodename As String, Optional ByVal v_lItemNum As Integer = 1) As cGISDataSetControl.Node
        Get

            Dim result As cGISDataSetControl.Node = Nothing
            Dim oDomNodes As XmlNodeList = Nothing
            Dim oDomNode As XmlNode
            Dim oNewNode As cGISDataSetControl.Node
            Dim bGISObjectNotFound As Boolean

            oDomNode = Nothing
            If v_lItemNum < 1 Then v_lItemNum = 1
            ' RFC300300 - Uppercase the Node Name
            v_sNodename = v_sNodename.Trim().ToUpper()

            ' Is this item a GIS object or a GIS property?
            '---------------------------------------------

            ' RAW 22/01/2004 : CQ3720 : changed rules for treating as a property

            ' First check if it is for a GIS object
            bGISObjectNotFound = False

            If Not m_oRoot.HasChildNodes Then

                bGISObjectNotFound = True

            Else
                ' Get the nodes from the current root with this tag name
                oDomNodes = m_oRoot.GetElementsByTagName(v_sNodename)

                If oDomNodes Is Nothing Then
                    bGISObjectNotFound = True
                Else
                    If oDomNodes.Count < 1 Then
                        bGISObjectNotFound = True
                    End If
                End If
            End If

            If bGISObjectNotFound Then

                ' We dont know at this stage whether it is a valid object name as we only
                ' have access to instances of objects that already exist.

                ' Assume that it a GIS property

                ' It may also be an object that doesn't exist ( ie an error) but we do not know
                ' that without checking the object definition which will probably add an
                ' unacceptable processing overhead.
                ' If it is an invalid object then this node will be passed to the next call
                ' rather than a new node representing the required object.
                ' This is wrong and means that a level in the object hierarchy is missed out
                ' and can lead to eroneous references.
                ' If we had seperate properties for GisObject and GisProperty instead of using
                ' item for both then we could handle it - but we dont so for now we will have to live with it.

                result = Me
                ' Store the Last Node Name asked for, this will be used by the Value method
                m_sLastNodeName = v_sNodename

                Return result
            End If
            ' RAW 22/01/2004 : CQ3720 : end

            ' Get the 1st, 2nd etc node as specified
            Try
                v_lItemNum = If(v_lItemNum = 0, 1, v_lItemNum)
                oDomNode = oDomNodes.Item(v_lItemNum - 1)

            Catch
            End Try

            If oDomNode Is Nothing Then
                'Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + My.Application.Info.Title & ".Node.Item" + ", " + "Instance Number " & v_lItemNum & " of Node " & v_sNodename & " Not Found")
                Return result
            End If

            ' Create a new Node
            oNewNode = New cGISDataSetControl.Node()

            ' Set the New Nodes Dataset
            oNewNode.Dataset = Dataset

            ' Set the root of the new node to be this element just found
            oNewNode.Root = oDomNode

            ' Set the Quote Key
            oNewNode.QuoteKey = QuoteKey

            ' Set the New Node to have same strict level as parent.
            oNewNode.Strict = Strict

            ' Return the New Node Created
            result = oNewNode

            oNewNode = Nothing
            oDomNodes = Nothing
            oDomNode = Nothing

            Return result

Err_Item:

            result = Nothing

            oDomNodes = Nothing
            oDomNode = Nothing

            Throw New System.Exception(Informations.Err().Number.ToString() + ", " + Informations.Err().Source + ", " + Informations.Err().Description)

            Return result

        End Get
    End Property

    Public Property Value() As Object
        Get

            Try
                If m_oDataset.PropertyValue(m_oRoot, m_sLastNodeName) = "" Then
                    Return ""
                Else
                    Return m_oDataset.PropertyValue(m_oRoot, m_sLastNodeName)
                End If



            Catch excep As System.Exception

                Throw New System.Exception(Informations.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)

                Exit Property

            End Try
        End Get
        Set(ByVal Value As Object)

            Dim lErrorNumber As Integer
            Dim sErrorDesc As String = ""
            Dim sObjectName As String = ""

            Try

                ' RAW 22/01/2004 : CQ3720 : added
                If Not (m_oRoot Is Nothing) Then
                    sObjectName = m_oRoot.Name
                End If

                ' Set the Property Value
                m_oDataset.PropertyValueSet(r_oObjectInst:=m_oRoot, v_sObjectName:=sObjectName, v_sPropertyName:=m_sLastNodeName, v_vPropertyValue:=Value)

            Catch excep As System.Exception
                lErrorNumber = Informations.Err().Number
                sErrorDesc = excep.Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Value Let property failed for " & sObjectName & "." & m_sLastNodeName, vApp:=ACApp, vClass:=ACClass, vMethod:="Value(Let)", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
                ' RAW 22/01/2004 : CQ3720 : bpmfunc.LogMessage clears err object so use original details
                Throw New System.Exception(lErrorNumber.ToString() + ", " + ACApp & "." & ACClass & "." & "Value(Let) (" & sObjectName & "." & m_sLastNodeName & ")" + ", " + sErrorDesc)

                Exit Property

            End Try

        End Set
    End Property

    Public ReadOnly Property Count(ByVal v_sChildNodename As String) As Integer
        Get

            Dim result As Integer = 0
            Dim oDomNodes As XmlNodeList

            Try

                ' RFC300300 - Uppercase the Node Name
                v_sChildNodename = v_sChildNodename.Trim().ToUpper()

                ' Get the nodes from the current root with this tag name
                oDomNodes = m_oRoot.GetElementsByTagName(v_sChildNodename)
                If oDomNodes Is Nothing Then
                    result = 0
                End If

                ' Return the number of nodes
                result = oDomNodes.Count

                oDomNodes = Nothing

                Return result

            Catch

                Return 0
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

        Dim sParentObjectName, sParentOIKey As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sOIKey As String = ""

        Try

            ' Get the Parent Object Name
            sParentObjectName = m_oRoot.Name

            ' Get the Parent OIKey

            sParentOIKey = CStr(m_oRoot.GetAttribute(ACXMLAttribOIKey))

            ' Create the Instance
            If QuoteKey = "" Then

                lReturn = CType(m_oDataset.NewObjectInstance(v_sObjectName:=v_sObjectName, r_sOIKey:=sOIKey, v_sParentOIKey:=sParentOIKey), gPMConstants.PMEReturnCode)

            Else

                lReturn = CType(m_oDataset.NewObjectInstance(v_sObjectName:=v_sObjectName, r_sOIKey:=sOIKey, v_sParentOIKey:=sParentOIKey, v_sQuoteKey:=QuoteKey), gPMConstants.PMEReturnCode)

            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception("120, New Object, Failed to Create a New Object Instance")
            End If

        Catch excep As System.Exception

            Throw New System.Exception(Informations.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)

            Exit Sub

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

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sObjectName, sOIKey As String

        Try

            ' Get the OIKey

            sOIKey = CStr(Root.GetAttribute(ACXMLAttribOIKey))

            ' Get the Object Name
            sObjectName = Root.Name

            lReturn = CType(m_oDataset.DelObjectInstance(sObjectName, sOIKey), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception("135, Delete Object, " + "Failed to Delete Object " & sObjectName)
            End If

        Catch excep As System.Exception

            Throw New System.Exception(Informations.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)

            Exit Sub

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

        Dim result As Object = Nothing
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vChildObjects As Object = Nothing
        Dim sObjectName As String = ""

        Try

            ' Get the Object Name
            sObjectName = m_oRoot.Name

            ' Get the Child Objects for this Object
            lReturn = CType(m_oDataset.GetObjectDefDetails(v_sObjectName:=sObjectName, r_vChildObjectArray:=vChildObjects), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception("130, Child Objects, " + "Failed to get Child Objects for object " & sObjectName)
            End If

            If Informations.IsArray(vChildObjects) Then
                Return vChildObjects
            Else
                Return Nothing
            End If

        Catch
        End Try

        Throw New System.Exception(Informations.Err().Number.ToString() + ", " + Informations.Err().Source + ", " + Informations.Err().Description)

        Return result

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
    Public Function Properties() As Object

        Dim result As Object = Nothing
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vProperties As Object = Nothing
        Dim vOutProperties As Object = Nothing
        Dim sObjectName As String = ""
        Dim lFrom, lTo As Integer

        Try

            ' Get the Object Name
            sObjectName = m_oRoot.Name

            ' Get the Properties for this Object
            lReturn = CType(m_oDataset.GetObjectDefDetails(v_sObjectName:=sObjectName, r_vPropertyArray:=vProperties), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception("131, Properties, " + "Failed to get the Properties for object " & sObjectName)
            End If

            If Informations.IsArray(vProperties) Then

                ' This will return a 2d array, we want to only return the Property Names

                lFrom = vProperties.GetLowerBound(1)

                lTo = vProperties.GetUpperBound(1)

                ReDim vOutProperties(lTo)

                For lRow As Integer = lFrom To lTo

                    vOutProperties(lRow) = vProperties(0, lRow)
                Next lRow

                result = vOutProperties

            Else

                result = Nothing

            End If

            vProperties = Nothing
            vOutProperties = Nothing

            Return result

        Catch excep As System.Exception

            Throw New System.Exception(Informations.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ResetPropertyValues
    '
    ' Description:
    '
    ' History: 27/03/2000 RFC - Created.
    '
    ' ***************************************************************** '
    Public Sub ResetPropertyValues()

        Dim sObjectName, sOIKey As String
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            ' Get the Object Name
            sObjectName = m_oRoot.Name

            ' Get the OIKey

            sOIKey = CStr(m_oRoot.GetAttribute(ACXMLAttribOIKey))

            ' Reset the Property Values
            lReturn = CType(m_oDataset.ResetPropertyValues(v_sObjectName:=sObjectName, r_sOIKey:=sOIKey), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception("121, Reset Property Values, " + "Failed to Reset the Property Values for " & sObjectName)
            End If

        Catch excep As System.Exception

            Throw New System.Exception(Informations.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: GradientCalc
    '
    ' Description:
    '
    ' History: 19/04/2000 RFC - Created.
    'RFC190400 - Add Lookup Methods to GIS
    ' ***************************************************************** '
    Public Function GradientCalc(ByVal v_vLowerLevel As Object, ByVal v_vUpperLevel As Object, ByVal v_vLowerValue As Object, ByVal v_vUpperValue As Object, ByVal v_vParameterValue As Object, ByRef r_vAnswer As Object) As Integer

        Dim result As Integer = 0
        Try

            Return m_oDataset.LookupManager.GradientCalc(vLowerLevel:=CType(v_vLowerLevel, StringsHelper.FixedLengthString), vUpperLevel:=CType(v_vUpperLevel, StringsHelper.FixedLengthString), vLowerValue:=CType(v_vLowerValue, StringsHelper.FixedLengthString), vUpperValue:=CType(v_vUpperValue, StringsHelper.FixedLengthString), vParameterValue:=CStr(v_vParameterValue), vAnswer:=CType(r_vAnswer, StringsHelper.FixedLengthString))

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GradientCalc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GradientCalc", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Lookup
    '
    ' Description:
    '
    ' History: 19/04/2000 RFC - Created.
    'RFC190400 - Add Lookup Methods to GIS
    ' ***************************************************************** '
    Public Function Lookup(ByVal v_sTable As String, Optional ByVal v_vInput As Object = Nothing) As Object

        Dim result As Object = Nothing
        Try

            Return m_oDataset.LookupManager.Lookup(sTable:=v_sTable, vInput:=v_vInput)

        Catch excep As System.Exception

            result = ""

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Lookup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Lookup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LookupByDate
    '
    ' Description:
    '
    ' History: HG14072003 - Created.
    ' ***************************************************************** '
    Public Function LookupByDate(ByVal v_sTable As String, ByVal v_dEffectiveDate As Date, Optional ByVal v_vInput As Object = Nothing) As Object

        Dim result As Object = Nothing
        Try

            Return m_oDataset.LookupManager.LookupByDate(v_sTable:=v_sTable, v_dEffectiveDate:=v_dEffectiveDate, v_vInput:=v_vInput)

        Catch excep As System.Exception

            result = ""

            ' Log Error Message

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LookupByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LookupByDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

            Return result
        End Try
    End Function
    Protected Overrides Sub Finalize()
        Root = Nothing
        Dataset = Nothing
    End Sub
End Class
