<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.ssp-worldwide.com" xmlns="http://www.siriusfs.com/SFI/Import/MID2_Import/20070716" >
  <xsl:output omit-xml-declaration="no" media-type="xml" indent="yes" />

  <msxsl:script language="VBScript" implements-prefix="user">
    <![CDATA[
		
    Function ProcessField(ByVal param , ByVal iStart, ByVal iLength) 
        Dim strMIDDoc as string
        strMIDDoc = Replace(param.AsList.Item(0).InnerXml, "&gt;", ">")
        strMIDDoc = Replace(strMIDDoc, "&lt;", "<")
        ProcessField = Mid(strMIDDoc, iStart, iLength)
    End function
		
    Function ProcessFieldL(ByVal param , ByVal iStart) 
        Dim strMIDDoc as string
        strMIDDoc = Replace(param.AsList.Item(0).InnerXml, "&gt;", ">")
        strMIDDoc = Replace(strMIDDoc, "&lt;", "<")
		iPos = iStart
		If InStr(1, Trim(Mid(strMIDDoc, iPos + 49, 35)), "THIS FILE HAS BEEN LOADED") > 0
			ProcessFieldL = 1
		Else		
			ProcessFieldL = 0
		End if
    End function
	
    Function ProcessFieldR(ByVal param , ByVal iStart) 
        Dim strMIDDoc as string
        strMIDDoc = Replace(param.AsList.Item(0).InnerXml, "&gt;", ">")
        strMIDDoc = Replace(strMIDDoc, "&lt;", "<")
        iPos = iStart
		If InStr(1, Trim(Mid(strMIDDoc, iPos + 49, 35)), "THIS FILE HAS BEEN RECEIVED") > 0
			ProcessFieldR = 1
		Else		
			ProcessFieldR = 0
		End if		
    End function
	 
	 Function ProcessFieldsFailure(ByVal param , ByVal iStart) 
        Dim strMIDDoc As string
        Dim iPos As Integer 
        Dim strTempErrorCodes As String
        Dim iTotalErrorCodes As Integer
        Dim iCnt As Integer
		
        'Initialising string variable  
        ProcessFieldsFailure = ""
		
        If param.AsList.Count > 0 Then		
            strMIDDoc = Replace(param.AsList.Item(0).InnerXml, "&gt;", ">")
            strMIDDoc = Replace(strMIDDoc, "&lt;", "<")
            iPos = iStart
            
            While Mid(strMIDDoc, iPos, 1) = "X" And Not(InStr(1, Trim(Mid(strMIDDoc, iPos + 49, 35)), "THIS FILE HAS BEEN LOADED") > 0) AND Not(InStr(1, Trim(Mid(strMIDDoc, iPos + 49, 35)), "THIS FILE HAS BEEN RECEIVED") > 0)
                'Initialising string variable
                strTempErrorCodes = ""
                
				        iPos = iPos + 1 
				        ProcessFieldsFailure = ProcessFieldsFailure & vbCrLf &"<MID2_FAILURES insurance_ref=""" & Trim(Mid(strMIDDoc, iPos, 20)) & """" 
				        iPos = iPos + 20 
				        ProcessFieldsFailure = ProcessFieldsFailure & " pppc=""" & Trim(Mid(strMIDDoc, iPos, 4)) & """" 
				        iPos = iPos + 4 
				        ProcessFieldsFailure = ProcessFieldsFailure & " expected_pppc=""" & Trim(Mid(strMIDDoc, iPos, 4)) & """" 
				        iPos = iPos + 4
				        ProcessFieldsFailure = ProcessFieldsFailure & " registration_number=""" & Trim(Mid(strMIDDoc, iPos, 12)) & """" 
				        iPos = iPos + 55
				        ProcessFieldsFailure = ProcessFieldsFailure & " error_reference=""" & Trim(Mid(strMIDDoc, iPos, 35)) & """" 
				        iPos = iPos + 36 
				
                iTotalErrorCodes = CInt((InStr(iPos, strMIDDoc, "#") - iPos) / 4)
                
                'Loop through and concatenate all error codes which may be upto 20 
                For iCnt = 1 To iTotalErrorCodes
                  If iCnt > 20 Then 
                    iPos = InStr(iPos, strMIDDoc, "#")
                    Exit For
                  End If
                  If iCnt = iTotalErrorCodes Then 
                    strTempErrorCodes = strTempErrorCodes & Mid(strMIDDoc, iPos, 4)
                    iPos = iPos + 4
                    Exit For
                  End If
                  strTempErrorCodes = strTempErrorCodes & Mid(strMIDDoc, iPos, 4)
                  iPos = iPos + 4                  
                Next iCnt
		            
                ProcessFieldsFailure = ProcessFieldsFailure & " errors=""" & Trim(strTempErrorCodes) & """/>"
				        iPos = iPos + 3 
		      End While
      End if
      ProcessFieldsFailure = ProcessFieldsFailure & vbCrLf
  End function
      ]]>
  </msxsl:script>

  <xsl:template match="/">
    <IMPORT_HEADER>
      <xsl:attribute name="interface_name" >MID2_IMPORT</xsl:attribute>
      <xsl:attribute name="batch_id">0</xsl:attribute>
      <xsl:attribute name="batch_reference">
        <xsl:value-of select="normalize-space(user:ProcessField(root, 14, 6))"/>
      </xsl:attribute>
      <xsl:attribute name="date_imported">
        <xsl:value-of select="normalize-space(user:ProcessField(root, 20, 8))"/>
      </xsl:attribute>
      <xsl:attribute name="supplier_id">
        <xsl:value-of select="normalize-space(user:ProcessField(root, 7, 3))"/>
      </xsl:attribute>
      <xsl:attribute name="site_number">
        <xsl:value-of select="normalize-space(user:ProcessField(root, 10, 3))"/>
      </xsl:attribute>
      <xsl:attribute name="is_loaded">
        <xsl:value-of select="normalize-space(user:ProcessFieldL(root, 35))"/>
      </xsl:attribute>
      <xsl:attribute name="is_received">
        <xsl:value-of select="normalize-space(user:ProcessFieldR(root, 35))"/>
      </xsl:attribute>
      <xsl:value-of select="user:ProcessFieldsFailure(root, 35)" disable-output-escaping="yes"/>
    </IMPORT_HEADER>
  </xsl:template>
</xsl:stylesheet>
