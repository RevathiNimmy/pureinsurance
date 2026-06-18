Option Strict Off
Option Explicit On
Imports System.Reflection
Imports System.Xml
Imports Microsoft.VisualBasic.CompilerServices
Imports SSP.Shared

Module GISActionXMLFuncsGeneric
    ' ***************************************************************** '
    ' Module Name: GISActionXMLFuncsGeneric
    '
    ' Date:  05/11/2001
    '
    ' Description: Functions to map and unmap the Method Calls and Paramaters
    '              to/from XML so that they can be sent over HTTP.
    '
    ' Edit History:
    ' RFC05112001 - Created
    '
    ' ***************************************************************** '


    Private Const ACClass As String = "GISActionXMLFuncsGeneric"

    Public Const ACXMLGenericAction As String = "GENERIC_ACTION"
    Public Const ACXMLGenericActionEndTag As String = "</GENERIC_ACTION>"

    Public Const ACXMLGenericActionReturn As String = "GENERIC_ACTION_RETURN"

    Private Const ACXMLActionClassName As String = "CLASS"
    Private Const ACXMLActionMethodName As String = "METHOD"
    Private Const ACXMLActionParamater As String = "PARAMETER"
    Private Const ACTypeAttrib As String = "T"



    ' ***************************************************************** '
    ' Name: ExecGenericActionXML
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function ExecGenericActionXML(ByVal v_sActionXML As String, ByRef r_sActionReturnXML As String) As Integer

        Dim result As Integer = 0
        Dim sNewXML As String = ""
        Dim oActionDoc As XmlDocument
        Dim oAction As XmlElement
        Dim bLoaded As Boolean
        Dim sClassName, sMethodName As String
        Dim vParam1 As Object = Nothing
        Dim vParam2 As Object = Nothing
        Dim vParam3 As Object = Nothing
        Dim vParam4 As Object = Nothing
        Dim vParam5 As Object = Nothing
        Dim vParam6 As Object = Nothing
        Dim vParam7 As Object = Nothing
        Dim vParam8 As Object = Nothing
        Dim vParam9 As Object = Nothing
        Dim vParam10 As Object = Nothing
        Dim vParam11 As Object = Nothing
        Dim vParam12 As Object = Nothing
        Dim vParam13 As Object = Nothing
        Dim vParam14 As Object = Nothing
        Dim vParam15 As Object = Nothing
        Dim vParam16 As Object = Nothing
        Dim vParam17 As Object = Nothing
        Dim vParam18 As Object = Nothing
        Dim vParam19 As Object = Nothing
        Dim vParam20 As Object = Nothing

        Dim lNumParams As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oClass As Object

        Dim lReturnValue As Integer


        result = gPMConstants.PMEReturnCode.PMTrue
        ' Create a New XML Document
        oActionDoc = New XmlDocument()

        ' Load the Action Document
        Dim temp_xml_result As Boolean

        Try
            oActionDoc.LoadXml(v_sActionXML)
            temp_xml_result = True
        Catch parseError As System.Exception
            temp_xml_result = False
        End Try
        bLoaded = temp_xml_result
        If Not bLoaded Then
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Load the Generic Action XML : " & v_sActionXML, vApp:=ACApp, vClass:=ACClass, vMethod:="ExecGenericActionXML")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        ' Get a Reference to the Action
        oAction = oActionDoc.DocumentElement.FirstChild
        ' Get the Attributes
        sClassName = CStr(oAction.GetAttribute(ACXMLActionClassName))
        sMethodName = CStr(oAction.GetAttribute(ACXMLActionMethodName))

        ' Get the number of Parameters
        lNumParams = oAction.ChildNodes.Count

        If lNumParams > 0 Then

            ' Get the Parameters

            ' Turn Off error handling for a moment as we may not get all of these params
            vParam1 = ParamValue(oAction.ChildNodes.Item(0))
            vParam2 = ParamValue(oAction.ChildNodes.Item(1))
            vParam3 = ParamValue(oAction.ChildNodes.Item(2))
            vParam4 = ParamValue(oAction.ChildNodes.Item(3))
            vParam5 = ParamValue(oAction.ChildNodes.Item(4))
            vParam6 = ParamValue(oAction.ChildNodes.Item(5))
            vParam7 = ParamValue(oAction.ChildNodes.Item(6))
            vParam8 = ParamValue(oAction.ChildNodes.Item(7))
            vParam9 = ParamValue(oAction.ChildNodes.Item(8))
            vParam10 = ParamValue(oAction.ChildNodes.Item(9))
            vParam11 = ParamValue(oAction.ChildNodes.Item(10))
            vParam12 = ParamValue(oAction.ChildNodes.Item(11))
            vParam13 = ParamValue(oAction.ChildNodes.Item(12))
            vParam14 = ParamValue(oAction.ChildNodes.Item(13))
            vParam15 = ParamValue(oAction.ChildNodes.Item(14))
            vParam16 = ParamValue(oAction.ChildNodes.Item(15))
            vParam17 = ParamValue(oAction.ChildNodes.Item(16))
            vParam18 = ParamValue(oAction.ChildNodes.Item(17))
            vParam19 = ParamValue(oAction.ChildNodes.Item(18))
            vParam20 = ParamValue(oAction.ChildNodes.Item(19))
            ' Turn on error trapping again
        End If

        oAction = Nothing
        oActionDoc = Nothing

        ' Create the Object
        oClass = gPMFunctions.CreateLateBoundObject("bGIS." & sClassName)
        If oClass Is Nothing Then
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CreateObject : " & "bGIS." & sClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="ExecGenericActionXML")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Initialise the Object

        lReturn = oClass.Initialise("", "", 1, 1, 1, 1, GISSharedConstants.GetGISLogLevel(), ACApp)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call the Method with the right number of Parameters
        Select Case lNumParams
            Case 0

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method))
            Case 1

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1))
            Case 2

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2))
            Case 3

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3))
            Case 4

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3, vParam4))
            Case 5

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3, vParam4, vParam5))
            Case 6

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3, vParam4, vParam5, vParam6))
            Case 7

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3, vParam4, vParam5, vParam6, vParam7))
            Case 8

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3, vParam4, vParam5, vParam6, vParam7, vParam8))
            Case 9

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3, vParam4, vParam5, vParam6, vParam7, vParam8, vParam9))
            Case 10

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3, vParam4, vParam5, vParam6, vParam7, vParam8, vParam9, vParam10))
            Case 11

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3, vParam4, vParam5, vParam6, vParam7, vParam8, vParam9, vParam10, vParam11))
            Case 12

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3, vParam4, vParam5, vParam6, vParam7, vParam8, vParam9, vParam10, vParam11, vParam12))
            Case 13

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3, vParam4, vParam5, vParam6, vParam7, vParam8, vParam9, vParam10, vParam11, vParam12, vParam13))
            Case 14

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3, vParam4, vParam5, vParam6, vParam7, vParam8, vParam9, vParam10, vParam11, vParam12, vParam13, vParam14))
            Case 15

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3, vParam4, vParam5, vParam6, vParam7, vParam8, vParam9, vParam10, vParam11, vParam12, vParam13, vParam14, vParam15))
            Case 16

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3, vParam4, vParam5, vParam6, vParam7, vParam8, vParam9, vParam10, vParam11, vParam12, vParam13, vParam14, vParam15, vParam16))
            Case 17

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3, vParam4, vParam5, vParam6, vParam7, vParam8, vParam9, vParam10, vParam11, vParam12, vParam13, vParam14, vParam15, vParam16, vParam17))
            Case 18

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3, vParam4, vParam5, vParam6, vParam7, vParam8, vParam9, vParam10, vParam11, vParam12, vParam13, vParam14, vParam15, vParam16, vParam17, vParam18))
            Case 19

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3, vParam4, vParam5, vParam6, vParam7, vParam8, vParam9, vParam10, vParam11, vParam12, vParam13, vParam14, vParam15, vParam16, vParam17, vParam18, vParam19))
            Case 20

                lReturnValue = CInt(Interaction.CallByName(oClass, sMethodName, CallType.Method, vParam1, vParam2, vParam3, vParam4, vParam5, vParam6, vParam7, vParam8, vParam9, vParam10, vParam11, vParam12, vParam13, vParam14, vParam15, vParam16, vParam17, vParam18, vParam19, vParam20))

        End Select

        ' We've Called the Method, so now we need to return the results
        lReturn = CType(FormatGenericActionReturnXML(sClassName, sMethodName, lReturnValue, lNumParams, r_sActionReturnXML, vParam1, vParam2, vParam3, vParam4, vParam5, vParam6, vParam7, vParam8, vParam9, vParam10, vParam11, vParam12, vParam13, vParam14, vParam15, vParam16, vParam17, vParam18, vParam19, vParam20), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to generate the Return XML", vApp:=ACApp, vClass:=ACClass, vMethod:="ExecGenericActionXML")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Return result

Err_ExecGenericActionXML:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExecGenericActionXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExecGenericActionXML", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        oAction = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: FormatGenericActionReturnXML
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function FormatGenericActionReturnXML(ByVal v_sClassName As String, ByVal v_sMethodName As String, ByVal v_lReturnValue As Integer, ByVal v_lNumOfParams As Integer, ByRef r_sActionReturnXML As String, ByVal ParamArray r_vParams() As Object) As Integer

        Dim result As Integer = 0
        Dim oElem As XmlElement
        Dim oActionReturnDoc As XmlDocument
        Dim oActionReturn, oParam As XmlElement
        Dim oCDATA As XmlCDataSection
        Dim vParam As Object
        Dim iVarType As VariantType
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a New XML Document
            oActionReturnDoc = New XmlDocument()

            ' Create the Document Element
            oActionReturn = oActionReturnDoc.CreateElement(ACXMLActionReturn)
            If Not (oActionReturnDoc.DocumentElement Is Nothing) Then
                oActionReturnDoc.RemoveChild(oActionReturnDoc.DocumentElement)
            End If
            oActionReturnDoc.AppendChild(oActionReturn)

            ' Create the Generic Action Return Element
            oElem = oActionReturnDoc.CreateElement(ACXMLGenericActionReturn)

            oActionReturn.AppendChild(oElem)

            ' Set the Class Name Element
            oElem.SetAttribute(ACXMLActionClassName, v_sClassName.Trim())

            ' Set the Method Name Element
            oElem.SetAttribute(ACXMLActionMethodName, v_sMethodName.Trim())

            ' Set the Return Value Attribute
            oElem.SetAttribute(ACXMLAttribReturnValue, v_lReturnValue)

            ' For Each Parameter that we need to Return
            For lRow As Integer = 0 To v_lNumOfParams - 1

                ' Get the Param


                vParam = r_vParams(lRow)

                ' Create the Param Value Element
                oParam = oActionReturnDoc.CreateElement(ACXMLActionParamater)

                ' Get the Variable Type
                iVarType = Informations.VarType(vParam)

                ' If the Value is a String, then use a CDATA section
                ' in case the string contains any of the following chars &<>!
                If iVarType = VariantType.String Then

                    ' Create the Character Data Section
                    oCDATA = oActionReturnDoc.CreateCDataSection("Text")

                    oCDATA.InnerText = CStr(vParam).Trim()

                    ' Append the Character Data to the Value Element

                    oParam.AppendChild(oCDATA)

                    oCDATA = Nothing

                Else

                    ' Set the Text with the value


                    Select Case iVarType
                        Case VariantType.Null
                            ' Use Empty for a Null Value as it will error otherwise
                            ' The Property Get will return this correctly as a NULL
                            oParam.InnerText = String.Empty

                        Case VariantType.Date

                            If CDate(vParam).ToString("HH:mm:ss") <> "00:00:00" Then

                                oParam.InnerText = CDate(vParam).ToString("yyyy/MM/dd HH:mm:ss")
                            Else

                                oParam.InnerText = CDate(vParam).ToString("yyyy/MM/dd")
                            End If

                        Case Else

                            If Informations.IsArray(vParam) Then

                                lReturn = CType(FormatArrayToXML(r_oDocument:=oActionReturnDoc, r_oParentElem:=oParam, v_sTagName:="ARRAY", v_vArray:=vParam), gPMConstants.PMEReturnCode)
                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Format Array Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatGenericActionXML")
                                    Return lReturn
                                End If

                            Else


                                oParam.InnerText = CStr(vParam)

                            End If


                    End Select

                End If

                ' Set the Type Attributes
                oParam.SetAttribute(ACTypeAttrib, iVarType)

                ' Append it to the Parameters Element

                oElem.AppendChild(oParam)

                oParam = Nothing

            Next

            r_sActionReturnXML = oActionReturn.InnerXml

            oElem = Nothing
            oActionReturn = Nothing
            oActionReturnDoc = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatGenericActionReturnXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatGenericActionReturnXML", excep:=excep)


            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ParamValue
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function ParamValue(ByRef oParam As XmlElement) As Object

        Dim result As Object = Nothing
        Dim vParamValue As Object
        Dim sParamType As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode



        ' If there is No Parameter then return empty

        If oParam Is Nothing Then
            Return Nothing

        Else

            ' Get the Value

            vParamValue = oParam.InnerText
            ' and the Type

            sParamType = CStr(oParam.GetAttribute(ACTypeAttrib))

            ' Note: 8204 = Variant(). I've no idea why there isn't a constant defined for it in VB.
            If sParamType = "8204" Then

                lReturn = CType(UnformatXMLtoArray(v_oParentElem:=oParam, v_sTagName:="ARRAY", r_vResultArray:=vParamValue), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to convert the Parameter back into an Array", vApp:=ACApp, vClass:=ACClass, vMethod:="ParamValue")
                End If

                Return vParamValue

            Else

                ' Convert it to type
                Return ConvertToType(sParamType, vParamValue)

            End If

        End If




        result = gPMConstants.PMEReturnCode.PMError


        ' Log Error Message
        'CONVERSION FOR LOGMESSAGEFILE OUTSIDE CATCH BLOCK
        '        gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ParamValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ParamValue", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)


        gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ParamValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ParamValue", excep:=New Exception(Informations.Err().Description))

        Return result

    End Function
End Module

