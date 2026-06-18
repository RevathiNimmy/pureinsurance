Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Runtime.InteropServices
Imports System.Xml
'developer guide no. 129
Imports SharedFiles
Module GISAdditionalXMLFuncs
	' ***************************************************************** '
	' Module Name: GISAdditionalXMLFuncs
	'
	' Date:  RAG201100
	'
	' Description: Functions to handle Additional Data Array <--> XML
	'
	' Edit History:
	' RFC01/08/01 - Need to convert dates in to year first format so
	' ***************************************************************** '
	
	
	Private Const ACClass As String = "GISAdditionalXMLFuncs"
	
	Private Const ACValueAttrib As String = "V"
	Private Const ACTypeAttrib As String = "T"
	Private Const ACDataElement As String = "DATA"
	Private Const ACAdditionalDataElement As String = "ADDITIONAL_DATA"
	
	
	' ***************************************************************** '
	'
	' Name: FormatArrayToXML
	'
	' Description:  Takes a array and stores it in the XML. This
	'               generic method avoids problems when the array
	'               dimensions change.
	'
	' History:  23/10/2001 DD - Created.
	'           08/11/2001 DD - Added support for single dimension arrays.
	'
	' ***************************************************************** '
	Public Function FormatArrayToXML(ByRef r_oDocument As XmlDocument, ByRef r_oParentElem As XmlElement, ByVal v_sTagName As String, ByVal v_vArray As Array) As Integer
		
		Dim result As Integer = 0
		Dim oActionArrayRow, oActionArrayCol As XmlElement
		Dim iType As VariantType
		Dim vValue As String = ""
		
		'Trap the array dimensions

		Try 
			Dim lCheckBound As Integer = v_vArray.GetLowerBound(1)
			Dim bSingleDimension As Boolean = (Information.Err().Number <> 0)
			
			Try 
				
				'Handle the version array using a generic method
				If Information.IsArray(v_vArray) Then
					If bSingleDimension Then
						'loop through a single dimension array
						oActionArrayRow = r_oDocument.CreateElement(v_sTagName)
						oActionArrayRow.SetAttribute("Dimension", 1)
						For lCol As Integer = v_vArray.GetLowerBound(0) To v_vArray.GetUpperBound(0)
							oActionArrayCol = r_oDocument.CreateElement("Col" & lCol)
							

                            vValue = CStr(v_vArray(lCol))
                            '**** Added By: AAB  -  Added On:  11-Sep-2002 13:01 ****
                            '**** Added to get over the NULL Problem.

                            If Convert.IsDBNull(vValue) Or IsNothing(vValue) Then
                                iType = VariantType.String
                            Else
                                iType = Information.VarType(vValue)
                            End If

                            'Store dates in fixed format to avoid regional problems
                            If iType = VariantType.Date Then
                                Dim TempDate3 As Date
                                If (IIf(DateTime.TryParse(vValue, TempDate3), TempDate3.ToString("HH:mm:ss"), vValue)) <> "00:00:00" Then
                                    Dim TempDate As Date
                                    vValue = IIf(DateTime.TryParse(vValue, TempDate), TempDate.ToString("yyyy/MM/dd HH:mm:ss"), vValue)
                                Else
                                    Dim TempDate2 As Date
                                    vValue = IIf(DateTime.TryParse(vValue, TempDate2), TempDate2.ToString("yyyy/MM/dd"), vValue)
                                End If
                            End If

                            oActionArrayCol.SetAttribute(ACTypeAttrib, iType)
                            '**** Added By: AAB  -  Added On:  11-Sep-2002 13:01 ****
                            '**** Added to get over the NULL Problem.
                            oActionArrayCol.InnerText = vValue & ""

                            oActionArrayRow.AppendChild(oActionArrayCol)
                        Next lCol


                        r_oParentElem.AppendChild(oActionArrayRow)
                    Else
                        'loop through a 2D array
                        For lRow As Integer = v_vArray.GetLowerBound(1) To v_vArray.GetUpperBound(1)
                            oActionArrayRow = r_oDocument.CreateElement(v_sTagName)
                            oActionArrayRow.SetAttribute("Dimension", 2)

                            For lCol As Integer = v_vArray.GetLowerBound(0) To v_vArray.GetUpperBound(0)
                                oActionArrayCol = r_oDocument.CreateElement("Col" & lCol)


                                vValue = CStr(v_vArray(lCol, lRow))
                                '**** Added By: AAB  -  Added On:  11-Sep-2002 11:29 ****
                                '**** Added the IF statement to deal with NULL values

                                If Convert.IsDBNull(vValue) Or IsNothing(vValue) Then
                                    iType = VariantType.String
                                Else
                                    iType = Information.VarType(vValue)
                                End If

                                'Store dates in fixed format to avoid regional problems
                                If iType = VariantType.Date Then
                                    Dim TempDate6 As Date
                                    If (IIf(DateTime.TryParse(vValue, TempDate6), TempDate6.ToString("HH:mm:ss"), vValue)) <> "00:00:00" Then
                                        Dim TempDate4 As Date
                                        vValue = IIf(DateTime.TryParse(vValue, TempDate4), TempDate4.ToString("yyyy/MM/dd HH:mm:ss"), vValue)
                                    Else
                                        Dim TempDate5 As Date
                                        vValue = IIf(DateTime.TryParse(vValue, TempDate5), TempDate5.ToString("yyyy/MM/dd"), vValue)
                                    End If
                                End If

                                oActionArrayCol.SetAttribute(ACTypeAttrib, iType)
                                '**** Added the blank to deal with NULL values
                                oActionArrayCol.InnerText = vValue & ""

                                oActionArrayRow.AppendChild(oActionArrayCol)
                            Next lCol


                            r_oParentElem.AppendChild(oActionArrayRow)
                        Next lRow
					End If
					
					oActionArrayCol = Nothing
					oActionArrayRow = Nothing
				End If
				
				
				Return gPMConstants.PMEReturnCode.PMTrue
			
			Catch excep As System.Exception
				
				
				
				result = gPMConstants.PMEReturnCode.PMError
				
				' Log Error Message
				 bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatArrayToXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatArrayToXML", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
				
				Return result
				
			End Try
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
	End Function
	
	
	' ***************************************************************** '
	'
	' Name: UnformatXMLtoArray
	'
	' Description:  Rebuilds an Array from one stored by the
	'               FormatArraytoXML function.
	'
	' History:  23/10/2001 DD - Created.
	'           08/11/2001 DD - Added support for single dimension arrays.
	'
	' ***************************************************************** '
	Public Function UnformatXMLtoArray(ByVal v_oParentElem As XmlElement, ByVal v_sTagName As String, ByRef r_vResultArray As Array) As Integer
		
		Dim result As Integer = 0
		Dim oMatches As XmlNodeList
		Dim oActionArrayRow, oActionArrayCol As XmlElement
		Dim lRows, lCols As Integer
		Dim vValue As Object
		Dim iType As Integer
		Dim bSingleDimension As Boolean
		
		Try 
			
			oMatches = v_oParentElem.GetElementsByTagName(v_sTagName)
			If Not (oMatches Is Nothing) Then
				
				' How Many Matches are there
				lRows = oMatches.Count
				
				' If there are matches then
				If lRows > 0 Then
					' Determine the dimensions
					oActionArrayRow = oMatches.Item(0)

					bSingleDimension = (CDbl(oActionArrayRow.GetAttribute("Dimension")) = 1)
					
					' Now work out the other dimension
					lCols = oActionArrayRow.ChildNodes.Count
					
					If bSingleDimension Then
						' Resize the Array to hold the results
						r_vResultArray = Array.CreateInstance(GetType(Object), New Integer(){lCols}, New Integer(){0})
						
						' Now move through the XML filling the array
						oActionArrayCol = oActionArrayRow.FirstChild
						For lCol As Integer = 0 To lCols - 1



							r_vResultArray(lCol) = ConvertToType(CStr(oActionArrayCol.GetAttribute(ACTypeAttrib)), oActionArrayCol.InnerText)
                            If Not (oActionArrayCol.NextSibling Is Nothing) Then
                                oActionArrayCol = oActionArrayCol.NextSibling
                            End If
						Next lCol
					Else
						
						' Resize the Array to hold the results
						r_vResultArray = Array.CreateInstance(GetType(Object), New Integer(){lCols, lRows}, New Integer(){0, 0})
						
						' Now move through the XML filling the array
						For lRow As Integer = 0 To lRows - 1
							oActionArrayRow = oMatches.Item(lRow)
							oActionArrayCol = oActionArrayRow.FirstChild
							For lCol As Integer = 0 To lCols - 1



                                r_vResultArray(lCol, lRow) = ConvertToType(CStr(oActionArrayCol.GetAttribute(ACTypeAttrib)), oActionArrayCol.InnerText)
								If Not (oActionArrayCol.NextSibling Is Nothing) Then
									oActionArrayCol = oActionArrayCol.NextSibling
								End If
							Next lCol
						Next lRow
					End If
				End If
				
				oActionArrayCol = Nothing
				oActionArrayRow = Nothing
				oMatches = Nothing
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			 bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnformatXMLtoArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnformatXMLtoArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	'
	' Name: FormatAdditionalDataXML
	'
	' Description: Takes a Name/Value array and stores it in the XML
	'
	' History: 02/10/2000 RFC - Created.
	'          20/11/2000 RAG - Moved to GISAdditionalXMLFuncs.bas
	'
	' ***************************************************************** '
	Public Function FormatAdditionalDataXML(ByRef r_oDocument As XmlDocument, ByRef r_oParentElem As XmlElement, ByVal v_vAdditionalDataArray( ,  ) As Object) As Integer
		
		Dim result As Integer = 0
		Dim lFrom, lTo As Integer
		
		Dim sName As String = ""
		Dim vValue As String = ""
		Dim iType As VariantType
		
		Dim oAddData, oData As XmlElement
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Do we Have an Array
			If Not Information.IsArray(v_vAdditionalDataArray) Then
				Return result
			End If
			
			' Get the Row From/To
			lFrom = v_vAdditionalDataArray.GetLowerBound(1)
			lTo = v_vAdditionalDataArray.GetUpperBound(1)
			
			' Create the Additional Data Element
			oAddData = r_oDocument.CreateElement(ACAdditionalDataElement)
			
			' Append the Add Data element to the Parent

			r_oParentElem.AppendChild(oAddData)
			
			' Add each Data Item
			For lRow As Integer = lFrom To lTo
				

				sName = CStr(v_vAdditionalDataArray(0, lRow))
				' RDC 02082001 vValue needs to be retrieved before it can be tested START
				'        iType = VarType(vValue)
				' RFC01/08/01 - Need to convert dates in to year first format so
				' that we do not get day/month transpostion problems with us/uk servers
				'        If (iType = vbDate) Then
				'            vValue = Format$(v_vAdditionalDataArray(1, lRow), "yyyy/mm/dd")
				'        Else
				'            vValue = v_vAdditionalDataArray(1, lRow)
				'        End If
				

				vValue = CStr(v_vAdditionalDataArray(1, lRow))
				
				iType = Information.VarType(vValue)
				
				' RFC01/08/01 - Need to convert dates in to year first format so
				' that we do not get day/month transpostion problems with us/uk servers
				If iType = VariantType.Date Then
					Dim TempDate As Date
					vValue = IIf(DateTime.TryParse(vValue, TempDate), TempDate.ToString("yyyy/MM/dd"), vValue)
				End If
				' RDC 02082001 vValue needs to be retrieved before it can be tested END
				
				' Create a Data Element
				oData = r_oDocument.CreateElement(ACDataElement)
				
				' Set text to be the Name
				oData.InnerText = sName.Trim()
				
				' Set the Type Attrib
				oData.SetAttribute(ACTypeAttrib, iType)
				
				' Set the Value Attrib
				oData.SetAttribute(ACValueAttrib, vValue)
				
				' Append it to the Add Data Element

				oAddData.AppendChild(oData)
				
				oData = Nothing
				
			Next lRow
			
			oData = Nothing
			oAddData = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatAdditionalDataXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatAdditionalDataXML", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	'
	' Name: UnFormatAdditionalDataXML
	'
	' Description: Rebuilds the Name/Value array from the XML
	'
	' History: 02/10/2000 RFC - Created.
	'          20/11/2000 RAG - Moved to GISAdditionalXMLFuncs.bas
	'
	' ***************************************************************** '
	Public Function UnFormatAdditionalDataXML(ByRef r_oDocument As XmlDocument, ByRef r_vAdditionalDataArray( ,  ) As Object) As Integer
		
		Dim result As Integer = 0
		Dim lRow, lNum As Integer
		
		Dim sName As String = ""
		Dim vValue As Object
		Dim sType As String = ""
		
		Dim oAddData, oData As XmlElement
		Dim oNodes As XmlNodeList
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Do we have any Additional Data
			oNodes = r_oDocument.GetElementsByTagName(ACAdditionalDataElement)
			
			If oNodes Is Nothing Then
				Return result
			End If
			
			
			'**** Added By: AAB  -  Added On:  04-Sep-2002 14:51 ****
			'**** Commented this line out was causing an error.
			'r_vAdditionalDataArray = ""
			
			' Get the Add Data Elem
			oAddData = oNodes.Item(0)
			
			If oAddData Is Nothing Then
				Return result
			End If
			
			' Get the Number of State Value
			lNum = oAddData.ChildNodes.Count
			
			' If there aren't any entries then exit
			If lNum < 1 Then
				Return result
			End If
			
			ReDim r_vAdditionalDataArray(1, lNum - 1)
			
			lRow = 0
			
			oData = oAddData.FirstChild
			
			Do While Not (oData Is Nothing)
				
				sName = oData.InnerText


				vValue = oData.GetAttribute(ACValueAttrib)

				sType = CStr(oData.GetAttribute(ACTypeAttrib))
				


				vValue = ConvertToType(sType, vValue)
				

				r_vAdditionalDataArray(0, lRow) = sName


				r_vAdditionalDataArray(1, lRow) = vValue
				oData = oData.NextSibling
				
				lRow += 1
				
			Loop 
			
			oData = Nothing
			oAddData = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			 bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnFormatAdditionalDataXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatAdditionalDataXML", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	'
	' Name: ConvertToType
	'
	' Description: Converts the variant from a string to its proper type.
	'
	' History: RFC210900 - Created
	'          20/11/2000 RAG - Moved to GISAdditionalXMLFuncs.bas
	'          06/11/2001 RFC - Changed to public, so it can be called by ActionXMLFuncsGeneric
	' ***************************************************************** '
	Public Function ConvertToType(ByVal v_sVarType As String, ByRef v_vValue As Object) As Object
		
		Try 
			
			' Return the Value, typed accordingly
			
			Select Case v_sVarType
				Case CStr(VariantType.String)

					Return CStr(v_vValue)
				Case CStr(VariantType.Date)

					Return CDate(v_vValue)
				Case CStr(VariantType.Decimal)

					Return CDec(v_vValue)
				Case CStr(VariantType.Decimal)

					Return CDec(v_vValue)
				Case CStr(VariantType.Boolean)

					Return CBool(v_vValue)
				Case CStr(VariantType.Short), CStr(VariantType.Integer)

					Return CInt(v_vValue)
				Case CStr(VariantType.Single)

					Return CSng(v_vValue)
				Case CStr(VariantType.Double)

					Return CDbl(v_vValue)
				Case CStr(VariantType.Byte)

					Return CByte(v_vValue)
				Case CStr(VariantType.Empty)
					Return Nothing
				Case CStr(VariantType.Null)

					Return DBNull.Value
				Case Else
					Return v_vValue
			End Select
		
		Catch 
		End Try
		
		
		
		Throw New System.Exception(Information.Err().Number.ToString() + ", " + Information.Err().Source + ", " + Information.Err().Description)
		
		Exit Function
		

		Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
		
	End Function
	
	
	' ***************************************************************** '
	' Name: ProcessActionViaHTTP
	'
	' Description: Sends the XML Data Set (including Action) via HTTP
	'              to the OIS Server. This will then be process by the
	'              OIS Server and the results sent back.
	'
	' Moved here from the many duplicates in each class 23102001 - DD
	' Added the v_oDataSet parameter to make generic.
	' ***************************************************************** '
	Public Function ProcessActionViaHTTP(ByVal v_oDataSet As cGISDataSetControl.Application, ByVal v_sActionXML As String, ByRef r_sActionReturnXML As String, Optional ByVal v_sXMLOldRisk As String = "") As Integer
		
		' RDC 17082001 object renamed in XML v3
		'Dim oHTTP As MSXML2.XMLHTTPRequest
		Dim result As Integer = 0
		Dim oHTTP As MSXML2.XMLHTTP
		Dim sXMLDataSetDef, sXMLDataSet, sReturnHTML, sReturnXML, sReturnDataSetXML As String
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim lXMLStart, lXMLEnd, lFoundAt As Integer
		Dim sURL As String = ""
		Dim bNoXML As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the Url to Post to
			lReturn = CType(iGISSharedConstants.GetHTTPPostPage(sURL), gPMConstants.PMEReturnCode)
			
			' If there is NO URL specified the do it via COM
			If sURL.Trim() = "" Then
				' When Testing Locally do this via COM
				Return ProcessActionViaCom(v_oDataSet:=v_oDataSet, v_sActionXML:=v_sActionXML, r_sActionReturnXML:=r_sActionReturnXML, v_sXMLOldRisk:=v_sXMLOldRisk)
			End If
			
			' Get the XML Data Set
			lReturn = v_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet)
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' RAG 211100 - Set a flag if no dataset is passed in,
			' If no dataset, then don't do the bit at the end ...
			bNoXML = False
			If sXMLDataSet = "" Then
				bNoXML = True
			End If
			
			' Prefix the Data Set with the Action
			sXMLDataSet = v_sActionXML & sXMLDataSet & v_sXMLOldRisk ' added v_sXMLOldRisk to fix HTTP prob! CL311000
			
			' RDC 17082001 object renamed in XML v3
			'    Set oHTTP = New MSXML2.XMLHTTPRequest
			oHTTP = New MSXML2.XMLHTTP()
			
			' Note: Url HAS to be in a variable for the following
			'sUrl = "http://devrad/GIS.asp"
			
			' Format a SYNCHRONOUS POST Command
			oHTTP.open("POST", sURL, False)
			
			' Send the XML Data Set
			oHTTP.send(sXMLDataSet)
			
			' Get the Return HTML
			' Note: The XML Data Set will be
			sReturnHTML = oHTTP.responseText
			
			' Get the XML From the HTML Body
			''''''''''''''''''''''''''''''''
			
			' Find the HTML <BODY> tag.
			lXMLStart = (sReturnHTML.IndexOf(ACHTMLBodyStartTag, StringComparison.CurrentCultureIgnoreCase) + 1)
			If lXMLStart < 1 Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			lXMLStart += ACHTMLBodyStartTag.Length
			lFoundAt = lXMLStart
			Do 
				lFoundAt = Strings.InStr(lFoundAt, sReturnHTML, ACHTMLBodyEndTag, CompareMethod.Text)
				If lFoundAt > 0 Then
					lXMLEnd = lFoundAt
					lFoundAt += 1
				End If
			Loop Until lFoundAt < 1
			
			sReturnXML = sReturnHTML.Substring(lXMLStart - 1, Math.Min(sReturnHTML.Length, lXMLEnd - lXMLStart))
			
			' RAG 211100 - don't do this is no dataset passed in.
			If bNoXML Then
				
				r_sActionReturnXML = sReturnXML
				
			Else
				
				' Split the Action from the Data Set
				lReturn = CType(SplitReturnAction(v_sReturnAndDataSetXML:=sReturnXML, r_sActionReturnXML:=r_sActionReturnXML, r_sDataSetXML:=sReturnDataSetXML), gPMConstants.PMEReturnCode)
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				' Get the XML Data Set
				lReturn = v_oDataSet.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sReturnDataSetXML)
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessActionViaHTTPFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessActionViaHTTP", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ProcessActionViaCom
	'
	' Description:
	'
	' Moved here from the many duplicates in each class 23102001 - DD
	' Added the v_oDataSet parameter to make generic.
	' ***************************************************************** '
	Public Function ProcessActionViaCom(ByVal v_oDataSet As cGISDataSetControl.Application, ByVal v_sActionXML As String, ByRef r_sActionReturnXML As String, Optional ByVal v_sXMLOldRisk As String = "") As Integer
		
		'Dim oBusiness As bGIS.ActionViaHTTP
		Dim result As Integer = 0
		Dim oBusiness As bGIS.ActionViaHTTP
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sXMLDataSetDef, sXMLDataSet As String
		Dim vXMLDataSet, sReturnDataSetXML As String
		Dim bNoXML As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			oBusiness = New bGIS.ActionViaHTTP()
			If oBusiness Is Nothing Then
				bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Create Object bGIS.ActionViaHTTP", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessActionViaCom")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Get the XML Data Set
			lReturn = v_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet)
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' RAG 211100 - Set a flag if no dataset is passed in,
			' If no dataset, then don't do the bit at the end ...
			bNoXML = False
			If sXMLDataSet = "" Then
				bNoXML = True
			End If
			
			
			
			' Prefix the Data Set with the Action
			' Note v_sXMLOldRisk is "" if not MTA call
			sXMLDataSet = v_sActionXML & sXMLDataSet & v_sXMLOldRisk
			
			
			' Convert into a Byte Array
			vXMLDataSet = StringsHelper.StrConv(sXMLDataSet, StringsHelper.VbStrConvEnum.vbFromUnicode)
			
			' Process It
			lReturn = oBusiness.ProcessAction(vXMLDataSet)
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Convert Back from a Byte Array
			sXMLDataSet = StringsHelper.StrConv(vXMLDataSet, StringsHelper.VbStrConvEnum.vbUnicode)
			
			' RAG 211100 - don't do this is no dataset passed in.
			If bNoXML Then
				
				r_sActionReturnXML = sXMLDataSet
				
			Else
				
				' Split the Action from the Data Set
				lReturn = CType(SplitReturnAction(v_sReturnAndDataSetXML:=sXMLDataSet, r_sActionReturnXML:=r_sActionReturnXML, r_sDataSetXML:=sReturnDataSetXML), gPMConstants.PMEReturnCode)
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				' Get the XML Data Set
				lReturn = v_oDataSet.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sReturnDataSetXML)
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessActionViaComFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessActionViaCom", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SplitReturnAction
	'
	' Description: Takes the incoming XML and splits the Action Return
	'              from the DataSet.
	'
	' Moved here from the many duplicates in each class 23102001 - DD
	' ***************************************************************** '
	Private Function SplitReturnAction(ByVal v_sReturnAndDataSetXML As String, ByRef r_sActionReturnXML As String, ByRef r_sDataSetXML As String) As Integer
		
		Dim result As Integer = 0
		Dim lEndOfAction As Integer
		
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Split the XML Input into an ActionReturn and a Dataset
			lEndOfAction = (v_sReturnAndDataSetXML.IndexOf(ACXMLActionReturnEndTag, StringComparison.CurrentCultureIgnoreCase) + 1)
			If lEndOfAction < 1 Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Get the ActionReturn
			r_sActionReturnXML = v_sReturnAndDataSetXML.Substring(0, lEndOfAction + ACXMLActionReturnEndTag.Length - 1)
			
			' Get the Data Set
			r_sDataSetXML = v_sReturnAndDataSetXML.Substring(lEndOfAction + ACXMLActionReturnEndTag.Length - 1)
			
			'Thinh Nguyen 08/11/2001 - make sure there is no carriage return at the beginning
			If r_sDataSetXML.Substring(0, (Strings.Chr(13) & Strings.Chr(10)).Length) = Strings.Chr(13) & Strings.Chr(10) Then
				r_sDataSetXML = Mid(r_sDataSetXML, (Strings.Chr(13) & Strings.Chr(10)).Length + 1)
			End If
			
			Return result
		
	End Function
	
	
	' ***************************************************************** '
	'
	' Name: bz
	'
	' Description: Blank to Zero function - converts a blank to a zero
	'
	' History: 09/11/2001 DD - Created.
	'
	' ***************************************************************** '
	Public Function bz(ByVal v_vValue As Byte, Optional ByVal v_vDefault As Byte = 0) As Byte

		Try 
			

			If Marshal.SizeOf(v_vValue) = 0 Then
				Return v_vDefault
			Else
				Return v_vValue
			End If
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
	End Function
End Module

