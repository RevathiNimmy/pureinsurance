Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Xml
' developer guide no. 129
Imports SharedFiles
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
	'
	' Name: GisCall
	'
	' Description:  Wraps up the Format/Process/Unformat routines into
	'               one simple call - Makes a call to a bGIS method from the
	'               iGISSellerTool.
	'
	' History: 09/11/2001 DD  - Created.
	'          13/08/2002 CJB - Added code to check if any of the parameters
	'                           were an array, if so, store array bounds and
	'                           on return from actioning the commend, check
	'                           that they match. If they do not then log err
	'                           and exit to avoid potential ASP catostrophic
	'                           failure. Note that empty arrays will NOT
	'                           cause failure. Note that only 1D and 2D arrays
	'                           are catered for.
	'
	' ***************************************************************** '

	Public Function GISCall(ByVal v_sClassName As String, ByVal v_sMethodName As String, ParamArray ByVal r_vParams() As Object) As Integer
		
		Dim result As Integer = 0
		Dim v_oDataSet As cGISDataSetControl.Application
		Dim sActionXML, sActionReturnXML As String
		Dim lReturn As Integer
		Dim lReturnValue As gPMConstants.PMEReturnCode
		Dim lItem As Integer
		Dim vParams As Object
		
		Dim lCheckBound As Integer 'CJB 130802
		Dim bSingleDimension As Boolean 'CJB 130802
		Dim vArrayDimensions( ,  ) As Object 'CJB 130802
		Dim iParamPosition As Integer 'CJB 130802
		
		'Array subscript position constants CJB 130802
		Const VARDATATYPE As Integer = 0
		Const ARRAYTYPE As Integer = 1
		Const LBOUND1 As Integer = 2
		Const UBOUND1 As Integer = 3
		Const LBOUND2 As Integer = 4
		Const UBOUND2 As Integer = 5
		
		On Error GoTo Err_GisCall
		
		'In Order to pass through the ParamArray we must convert it
		'to a standard variant

		vParams = r_vParams
		
		' Prepare the Action XML for sending

		lReturnValue = CType(FormatGenericActionXML(v_sClassName, v_sMethodName, sActionXML, vParams), gPMConstants.PMEReturnCode)
		
		If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
			Return lReturnValue
		End If
		
		' CJB Start 130802 Check for any parameters that are arrays (note that only predefined arrays will
		' be detected here - empty ones (the usual case) are not identified as arrays, just empty variants
		
		' For Each Parameter
		iParamPosition = 0
        'developer guide no.  changes to be checked at run time
        For Each vParam() As String In r_vParams

            vArrayDimensions = ArraysHelper.RedimPreserve(Of Object(,))(vArrayDimensions, New Integer() {UBOUND2 - VARDATATYPE + 1, iParamPosition + 1}, New Integer() {VARDATATYPE, 0})

            ' Save the parameter datatype in an array for comparison later

            vArrayDimensions(VARDATATYPE, iParamPosition) = Information.VarType(vParam)

            If Information.IsArray(vParam) Then

                ' Check whether we have a 1D or 2D array
                On Error Resume Next
                lCheckBound = vParam.GetLowerBound(1)
                bSingleDimension = (Information.Err().Number <> 0)
                On Error GoTo Err_GisCall

                ' Get and save the array dimensions
                If bSingleDimension Then
                    ' 1D Array

                    vArrayDimensions(ARRAYTYPE, iParamPosition) = "1D"

                    vArrayDimensions(LBOUND1, iParamPosition) = vParam.GetLowerBound(0)

                    vArrayDimensions(UBOUND1, iParamPosition) = vParam.GetUpperBound(0)
                Else
                    ' 2D Array

                    vArrayDimensions(ARRAYTYPE, iParamPosition) = "2D"

                    vArrayDimensions(LBOUND1, iParamPosition) = vParam.GetLowerBound(0)

                    vArrayDimensions(UBOUND1, iParamPosition) = vParam.GetUpperBound(0)

                    vArrayDimensions(LBOUND2, iParamPosition) = vParam.GetLowerBound(1)

                    vArrayDimensions(UBOUND2, iParamPosition) = vParam.GetUpperBound(1)
                End If
            End If

            iParamPosition += 1

        Next vParam
		' CJB End 130802
		
		' Send and Process The Command
		v_oDataSet = New cGISDataSetControl.Application()
		lReturnValue = CType(ProcessActionViaHTTP(v_oDataSet:=v_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
		If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
			Return lReturnValue
		End If
		
		' Check the ActionReturn Value
		' Unformat the Action Return XML

		lReturn = UnFormatGenericActionReturnXML(v_sClassName, v_sMethodName, lReturnValue, sActionReturnXML, vParams)
		
		' Copy the data back to the ParamArray
		lItem = 0
        ''developer guide no.  changes to be checked at run time
        For Each vParam() As String In vParams

            r_vParams(lItem) = vParam
            lItem += 1

            ' CJB Start 130802 Check that any arrays passed in have the same dimensions
            ' on the way out. If they do not raise an error since ASP may have a catostrophic
            ' failure.
            If Information.IsArray(vParam) Then
                ' Check whether we have a 1D or 2D array
                On Error Resume Next
                lCheckBound = vParam.GetLowerBound(1)
                bSingleDimension = (Information.Err().Number <> 0)
                On Error GoTo Err_GisCall

                If bSingleDimension Then
                    ' 1D Array
                    ' If the return parameter datatype is an array then only attempt to check the parameter's
                    ' input array bounds if it was a predefined array on the way in!! If, as in most cases, it
                    ' was an empty array then it would not have any bounds saved for it.
                    If vArrayDimensions(VARDATATYPE, lItem - 1) = Information.VarType(vParam) Then



                        If Not (CStr(vArrayDimensions(ARRAYTYPE, lItem - 1)) = "1D" And vParam.GetLowerBound(0) = CDbl(vArrayDimensions(LBOUND1, lItem - 1)) And vParam.GetUpperBound(0) = CDbl(vArrayDimensions(UBOUND1, lItem - 1))) Then
                            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Array dimensions have changed in call to " & v_sMethodName & ". Any array parameters to a SellerTool function must be Empty on the way in or have the same dimensions as will be set on the way out to avoid catostrophic failure!", vApp:=ACApp, vClass:=ACClass, vMethod:="GisCall", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            Return gPMConstants.PMEReturnCode.PMError
                        End If
                    End If
                Else
                    ' 2D Array
                    ' If the return parameter datatype is an array then only attempt to check the parameter's
                    ' input array bounds if it was a predefined array on the way in!! If, as in most cases, it
                    ' was an empty array then it would not have any bounds saved for it.
                    If vArrayDimensions(VARDATATYPE, lItem - 1) = Information.VarType(vParam) Then





                        If Not (CStr(vArrayDimensions(ARRAYTYPE, lItem - 1)) = "2D" And vParam.GetLowerBound(0) = CDbl(vArrayDimensions(LBOUND1, lItem - 1)) And vParam.GetUpperBound(0) = CDbl(vArrayDimensions(UBOUND1, lItem - 1)) And vParam.GetLowerBound(1) = CDbl(vArrayDimensions(LBOUND2, lItem - 1)) And vParam.GetUpperBound(1) = CDbl(vArrayDimensions(UBOUND2, lItem - 1))) Then
                            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Array dimensions have changed in call to " & v_sMethodName & ". Any array parameters to a SellerTool function must be Empty on the way in or have the same dimensions as will be set on the way out to avoid catostrophic failure!", vApp:=ACApp, vClass:=ACClass, vMethod:="GisCall", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            Return gPMConstants.PMEReturnCode.PMError
                        End If
                    End If
                End If

            End If
            ' CJB End 130802

        Next vParam
		
		
		Return lReturnValue
		
Err_GisCall: 
		
		result = gPMConstants.PMEReturnCode.PMError
		
		' Log Error Message
		bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GisCall Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GisCall", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	
	
	' ***************************************************************** '
	' Name: FormatGenericActionXML
	'
	' Description:
	'   DD09112001 replaced the ParamArray with a variant so it could
	'              be called from GISCall.
	'
	' ***************************************************************** '
	Public Function FormatGenericActionXML(ByVal v_sClassName As String, ByVal v_sMethodName As String, ByRef r_sActionXML As String, ByRef r_vParams() As Object) As Integer
		'    ParamArray r_vParams() As Variant) As Long
		
		Dim result As Integer = 0
		Dim oElem As XmlElement
		Dim oAction As XmlDocument
		Dim oParam As XmlElement
		Dim oCDATA As XmlCDataSection
		Dim lRow As Integer
		Dim iVarType As VariantType
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Create a New XML Document
			oAction = New XmlDocument()
			
			' Create the Document Element
			oElem = oAction.CreateElement(ACXMLAction)
			If Not (oAction.DocumentElement Is Nothing) Then
				oAction.RemoveChild(oAction.DocumentElement)
			End If
			oAction.AppendChild(oElem)
			
			' Create the Generic Action Element
			oElem = oAction.CreateElement(ACXMLGenericAction)

			oAction.DocumentElement.AppendChild(oElem)
			
			' Set the Class Name Element
			oElem.SetAttribute(ACXMLActionClassName, v_sClassName.Trim())
			
			' Set the Method Name Element
			oElem.SetAttribute(ACXMLActionMethodName, v_sMethodName.Trim())
			
			' For Each Parameter
			For	Each vParam As String In r_vParams
				
				' Create the Param Value Element
				oParam = oAction.CreateElement(ACXMLActionParamater)
				
				' Get the Variable Type
				iVarType = Information.VarType(vParam)
				
				' If the Value is a String, then use a CDATA section
				' in case the string contains any of the following chars &<>!
				If iVarType = VariantType.String Then
					
					' Create the Character Data Section
					oCDATA = oAction.CreateCDataSection("Text")
					oCDATA.InnerText = vParam.Trim()
					
					' Append the Character Data to the Value Element

					oParam.AppendChild(oCDATA)
					
					oCDATA = Nothing
					
				Else
					
					' Set the Text with the value
					
					
					Select Case iVarType
						Case VariantType.Null, VariantType.Error, VariantType.Empty
							' Use Empty for a Null Value as it will error otherwise
							' The Property Get will return this correctly as a NULL
							oParam.InnerText = String.Empty
							
						Case VariantType.Date
							Dim TempDate3 As Date
							If (IIf(DateTime.TryParse(vParam, TempDate3), TempDate3.ToString("HH:mm:ss"), vParam)) <> "00:00:00" Then
								Dim TempDate As Date
								oParam.InnerText = IIf(DateTime.TryParse(vParam, TempDate), TempDate.ToString("yyyy/MM/dd HH:mm:ss"), vParam)
							Else
								Dim TempDate2 As Date
								oParam.InnerText = IIf(DateTime.TryParse(vParam, TempDate2), TempDate2.ToString("yyyy/MM/dd"), vParam)
							End If
							
						Case Else
							
							If Information.IsArray(vParam) Then
                                ''developer guide no.  to be checked at run time
                                lReturn = CType(FormatArrayToXML(r_oDocument:=oAction, r_oParentElem:=oParam, v_sTagName:="ARRAY", v_vArray:=vParam.ToCharArray), gPMConstants.PMEReturnCode)
								If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
									bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Format Array Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatGenericActionXML")
									Return lReturn
								End If
								
							Else
								
								oParam.InnerText = vParam
								
							End If
							
					End Select
					
				End If
				
				' Set the Type Attributes
				oParam.SetAttribute(ACTypeAttrib, iVarType)
				
				' Append it to the Parameters Element

				oElem.AppendChild(oParam)
				
				oParam = Nothing
				
			Next vParam
			
			r_sActionXML = oAction.InnerXml
			
			oAction = Nothing
			oParam = Nothing
			oElem = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatGenericActionXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatGenericActionXML", excep:=excep)
			
			
			Return result
			
		End Try
	End Function
	
	
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
		Dim lRow As Integer
		Dim iVarType As Integer
		Dim bLoaded As Boolean
		
		Dim sClassName, sMethodName As String
		Dim vParam As Object
		Dim iParamType As Integer
		
		Dim vParam1, vParam2, vParam3, vParam4, vParam5, vParam6, vParam7, vParam8, vParam9, vParam10, vParam11, vParam12, vParam13, vParam14, vParam15, vParam16, vParam17, vParam18, vParam19, vParam20 As Object
		
		Dim lNumParams As Integer
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim oClass As Object
		
		Dim lReturnValue As Integer
		

		Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_ExecGenericActionXML)")
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Create a New XML Document
		oActionDoc = New XmlDocument()
		
		' Load the Action Document

        'developer guide no. no solution 22
        'oActionDoc.validateOnParse = False
		Dim temp_xml_result As Boolean

		Try 
			oActionDoc.LoadXml(v_sActionXML)
			temp_xml_result = True
		
		Catch parseError As System.Exception
			temp_xml_result = False
		End Try
		bLoaded = temp_xml_result
		If Not bLoaded Then
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Load the Generic Action XML : " & v_sActionXML, vApp:=ACApp, vClass:=ACClass, vMethod:="ExecGenericActionXML")
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

			Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Resume Next")



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

			Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_ExecGenericActionXML)")
			
		End If
		
		oAction = Nothing
		oActionDoc = Nothing
		
		' Create the Object
		oClass = Activator.CreateInstance(System.Reflection.Assembly.GetAssembly(Type.GetType("bGIS." & sClassName + "," + ("bGIS." & sClassName).Substring(0, ("bGIS." & sClassName).LastIndexOf(".")))).FullName, "bGIS." & sClassName).Unwrap()
		If oClass Is Nothing Then
            bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CreateObject : " & "bGIS." & sClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="ExecGenericActionXML")
			Return gPMConstants.PMEReturnCode.PMFalse
		End If
		
		' Initialise the Object

		lReturn = CType(oClass, SSP.S4I.Interfaces.IBusiness).Initialise("", "", 1, 1, 1, 1, iGISSharedConstants.GetGISLogLevel(), ACApp)
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
            bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to generate the Return XML", vApp:=ACApp, vClass:=ACClass, vMethod:="ExecGenericActionXML")
			Return gPMConstants.PMEReturnCode.PMFalse
		End If
		
		
		Return result
		
Err_ExecGenericActionXML: 
		
		result = gPMConstants.PMEReturnCode.PMError
		
		' Log Error Message
        bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExecGenericActionXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExecGenericActionXML", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		oAction = Nothing
		
		Return result
		
	End Function
	
	
	' ***************************************************************** '
	' Name: FormatGenericActionReturnXML
	'
	' Description:
	'
	' ***************************************************************** '
	Public Function FormatGenericActionReturnXML(ByVal v_sClassName As String, ByVal v_sMethodName As String, ByVal v_lReturnValue As Integer, ByVal v_lNumOfParams As Integer, ByRef r_sActionReturnXML As String, ParamArray ByVal r_vParams() As Object) As Integer
		
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
				iVarType = Information.VarType(vParam)
				
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
							
							If Information.IsArray(vParam) Then
								

								lReturn = CType(FormatArrayToXML(r_oDocument:=oActionReturnDoc, r_oParentElem:=oParam, v_sTagName:="ARRAY", v_vArray:=vParam), gPMConstants.PMEReturnCode)
								If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
									bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Format Array Parameter", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatGenericActionXML")
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
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatGenericActionReturnXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatGenericActionReturnXML", excep:=excep)
			
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: UnFormatGenericActionReturnXML
	'
	' Description:  Extracts the parameters from the XML returned.
	'
	' History:  DD 08/11/2001 - stopped pre-dimmed arrays from being
	'               redefined as this causes MASSIVE problems when they
	'               are returned to ASP. You can only pass "empty" variants
	'               for returning arrays.
	'           DD09112001 - replaced the ParamArray with a variant so
	'               it could be called from GISCall.
	'           CJB13082002 - Removed the change dated 08/11/2001 as this
	'               caused limitations so was checked for in GISCall.
	'
	' ***************************************************************** '
	Public Function UnFormatGenericActionReturnXML(ByRef r_sClassName As String, ByRef r_sMethodName As String, ByRef r_lReturnValue As Integer, ByVal v_sActionReturnXML As String, ByRef r_vParams() As Object) As Integer
		'    ParamArray r_vParams() As Variant) As Long
		
		Dim result As Integer = 0
		Dim oDocElem As XmlElement
		Dim oActionReturnDoc As XmlDocument
		Dim oActionReturn, oParam As XmlElement
		Dim oCDATA As XmlCDataSection
		Dim vParam As Object
		Dim iVarType As Integer
		Dim bLoaded As Boolean
		Dim lNumParams As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Create a New XML Document
			oActionReturnDoc = New XmlDocument()
			
			' Load the Document

            ' developer guide no. no solution 22
            'oActionReturnDoc.validateOnParse = False
			Dim temp_xml_result As Boolean
			Try 
				oActionReturnDoc.LoadXml(v_sActionReturnXML)
				temp_xml_result = True
			
			Catch parseError As System.Exception
				temp_xml_result = False
			End Try
			bLoaded = temp_xml_result
			If Not bLoaded Then
				bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Load the Generic Action Return XML : " & v_sActionReturnXML, vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatGenericActionReturnXML")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Get a Reference to the Action
			oActionReturn = oActionReturnDoc.DocumentElement.FirstChild
			
			' Get the Attributes

			r_sClassName = CStr(oActionReturn.GetAttribute(ACXMLActionClassName))

			r_sMethodName = CStr(oActionReturn.GetAttribute(ACXMLActionMethodName))

			r_lReturnValue = CInt(oActionReturn.GetAttribute(ACXMLAttribReturnValue))
			
			' Get the number of Parameters
			lNumParams = oActionReturn.ChildNodes.Count
			
			If lNumParams > 0 Then
				
				' For Each Parameter that we need to Return
				For lRow As Integer = 0 To lNumParams - 1
					
					' CJB130802 Do not impose these restrictions below - do checks in GISCall now...
					' Get the Param Value and Put in back in the Param Array
					' DD 08112001 - skip pre-defined arrays
					'''If (IsArray(r_vParams(lRow)) And IsEmpty(r_vParams(lRow))) Or Not IsArray(r_vParams(lRow)) Then



					r_vParams(lRow) = ParamValue(oActionReturn.ChildNodes.Item(lRow))
					'''End If
					
				Next lRow
				
			End If
			
			oActionReturn = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnFormatGenericActionReturnXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatGenericActionReturnXML", excep:=excep)
			
			
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
						bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to convert the Parameter back into an Array", vApp:=ACApp, vClass:=ACClass, vMethod:="ParamValue")
					End If
					
					Return vParamValue
					
				Else
					
					' Convert it to type
					Return ConvertToType(sParamType, vParamValue)
					
				End If
				
			End If
		
		
		
		
		result = gPMConstants.PMEReturnCode.PMError
		
		
		' Log Error Message
        gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ParamValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ParamValue", excep:=New Exception(Information.Err().Description))
		
		Return result
		
	End Function
End Module

