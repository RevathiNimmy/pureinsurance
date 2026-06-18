<?xml version="1.0" encoding="UTF-8" ?>


<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.ssp-worldwide.com"
                xmlns:ACB="http://www.siriusfs.com/SFI/Export/Payment_Export/20060420">

	<xsl:output omit-xml-declaration="yes"/>

	<msxsl:script language="VBScript" implements-prefix="user">
		<![CDATA[
    
  Dim vGlobal
  Dim gPurgeDate
  Dim gCounter      
  Dim gTotalPremium
  
  Function GetRight(ByVal intInput , ByVal intNoOfDigits)

  Dim iLen, i As Integer
  Dim strInput As String

  strInput = intInput
  iLen = Len(strInput)

  If iLen <  intNoOfDigits Then
    For i = 1 To intNoOfDigits - iLen 
     GetRight = GetRight & "0"
    Next 
    GetRight = GetRight & strInput 
  ElseIf iLen >  intNoOfDigits Then
    GetRight = Right(strInput, intNoOfDigits) 
  Else
    GetRight = strInput                   
  End If

  End Function
        
  Function Format_Number(ByVal intInput , ByVal intNoOfDigits ) 
    Dim iLen, i As Integer
    Dim strInput As String

    strInput = intInput
    iLen = Len(strInput)

    If iLen <  intNoOfDigits Then
      For i = 1 To intNoOfDigits - iLen 
       Format_Number = Format_Number & "0"
      Next 
        Format_Number = Format_Number & strInput 
    Else
        Format_Number = strInput                   
    End If

  End function
        
  Function Format_hiphencode(ByVal intInput , ByVal intNoOfDigits ) 
    Dim iLen, i As Integer
    Dim strInput,spacing,strFirstpart,strSecondpart As String

    strInput = intInput
    If InStr(strInput, "-") > 0 Then
     strInput = Replace(strInput, "-", "")
    End If

    iLen = Len(strInput)

    If iLen <  intNoOfDigits Then
      For i = 1 To intNoOfDigits - iLen 
        spacing = spacing & " "
      Next 
      strInput =  strInput  & spacing
    Else
      strInput = strInput                   
    End If

    strFirstpart=Left(strInput,3)
    strSecondpart=Right(strInput,3)

    Format_hiphencode = strFirstpart & "-" & strSecondpart  
				   
  End function
        
  Function Format_Right_justify_Number(ByVal intInput , ByVal intNoOfDigits ) 
    Dim iLen, i As Integer
    Dim strInput As String

    strInput = intInput
    iLen = Len(strInput)

    If iLen <  intNoOfDigits Then
      For i = 1 To intNoOfDigits - iLen 
       Format_Right_justify_Number = " " & Format_Right_justify_Number
      Next 
      Format_Right_justify_Number = Format_Right_justify_Number & strInput 
    Else
      Format_Right_justify_Number = strInput                   
    End If
  
  End function
  Function Format_Date(ByVal strInput)

    If Trim(strInput) = "" Then
      Format_Date = MultipleSpaces(19, " ")
    Else
      Format_Date= Format_Number(Year(strInput),4) & "-"&  Format_Number(Month(strInput), 2) & "-" & Format_Number(Day(strInput), 2)& " " &Format_Number(Hour(strInput), 2) & ":" &Format_Number(Minute(strInput), 2)& ":" & Format_Number(Second(strInput), 2)
    End If
  End function
		
  Function Format_Time(ByVal strInput)

    If Mid(strInput, 12, 2) = "" Or Mid(strInput, 15, 2) = "" Then
      Format_Time = "0000"
     Else
      Format_Time = Mid(strInput, 12, 2) + Mid(strInput, 15, 2) 
    End If	

  End function
		
  Function MultipleSpaces(ByVal intCount, ByVal strChar)
    Dim i As Integer

    For i = 1 to intCount
      MultipleSpaces = MultipleSpaces + Mid(strChar, 1, 1)
    Next

  End function
		
	Function FormatText(ByVal strInput As String, ByVal intLen As Integer)
    Dim l As Integer

    l = Len(strInput)

    If l <  intLen Then
      FormatText = strInput

      For i = 1 To intLen - l 
      FormatText = FormatText & " "
      Next 
             		 
    Else
      FormatText = strInput                   
    End If
		
  End function
		
	Function FormatGroupedAmount(ByVal strInput As String, ByVal pad As Integer)
	
		Dim l As Integer		
		Dim decimalPos as Integer
		Dim decimalPart as String
		Dim result as String
		Dim i as Integer 
		
		l = Len(strInput)
		If Trim(strInput) = "" Then
			FormatGroupedAmount = "0000000000"
		Else
      strInput = FormatNumber(strInput, 2)
      If InStr(strInput, ",") > 0 Then
        strInput = Replace(strInput, ",", "")
      End If
      
			decimalPos = InStr(strInput,".")
			If l > decimalPos Then
				decimalPart = Mid(strInput,decimalPos + 1, 2)
      Else
				decimalPart = "00"			
			End If
			
      If Len(decimalPart) < 2 Then
        decimalPart = decimalPart + "0"
      End If
      
			result = Mid(strInput,1 , decimalPos -1 )
			result = StrReverse(result)
      If InStr(result, "-") > 0 Then
          result = Replace(result, "-", "")
       End If
			l  = Len(result)
			
			If l = 0 Then
			
			result = result + "00000000"
			
			ElseIf l < 8 Then 
				l = 8 - l
				For i = 1 to l					
				  result = result + "0"					
				Next 					
			End If
      
      If pad <> 0 Then
        If Len(result) + Len(decimalPart) < pad Then
          For i = pad to (Len(result) + Len(decimalPart) + 1) step -1
            result = result + "0"
          Next
        
        End If
      End If
      
			result = StrReverse(result)
			result = result + decimalPart
      FormatGroupedAmount  = result
		
		End If	
	
	End Function		
    
	Function TrimAndPadNumber(ByVal strInput As String, ByVal Pad as Integer)
	
	  Dim l as Integer
	  Dim PadString as String 
  	
	  l = Len(strInput)
	  If l = 0 Then
		  For i =1 To Pad		
			  PadString = PadString + "0"
		  Next 
	  End If
	
	  strInput = RTrim(strInput)
	  strInput = LTrim(strInput)
	  l = Len(strInput)		
  	
	  strInput = StrReverse(strInput)
	  l = Pad - l
	
		For i =1 To l		
			strInput = strInput + "0"
		Next 	
		
		strInput = StrReverse(strInput)
		TrimAndPadNumber = strInput
	
	End Function	
	
	Function SumBankAccountNumbers(ByVal Num1 as Decimal)
		Dim l as Integer
		Dim i as Integer

		vGlobal = CStr(vGlobal)
		l = Len(vGlobal)
		
		If l <= 12 Then
			vGlobal = vGlobal + Num1					
		Else
			vGlobal = Right(vGlobal,12)
			vGlobal = vGlobal + Num1		
		End If
		
		SumBankAccountNumbers = vGlobal		
	End Function
		
	Function ReturnSumBankAccountNumbers()
	
		vGlobal = vGLobal + 62276725200 			
		vGlobal = CStr(vGlobal)
		l = Len(vGlobal)
		
		vGlobal =  StrReverse(vGlobal)
		If l < 12 Then
			l = 12 - l				
			For i =1 To l		
				vGlobal = vGlobal + "0"
			Next 	
		End If 
		
		vGlobal = StrReverse(vGlobal)		
	
		ReturnSumBankAccountNumbers = vGlobal
	
	End Function   

	Function FindGreatestPurgeDate(ByVal vReturn As Boolean, ByVal vDate As Object)
	
		gPurgeDate = CDate(gPurgeDate)
		vDate = LTrim(vDate)
		vDate = Rtrim(vDate)
		
		If (vReturn = False) and Not (vDate = "") Then
			vDate = CDate(vDate)
			If (vDate > gPurgeDate) Then
				gPurgeDate = vDate		
			End If
		End If
		FindGreatestPurgeDate = gPurgeDate
	End Function
	
  Function CounterInc() 
    gCounter = gCounter + 1
    CounterInc = Format_Number(gCounter, 10)
  End Function

  Function Counter(ByVal size as Integer)
    Counter = Format_Number(gCounter, 0)
  End Function
  
  Function AmountAdd(Byval amount as integer) 
    gTotalPremium = gTotalPremium +  amount
    AmountAdd = gTotalPremium
  End Function
  
  Function ShowAmount()
    ShowAmount = FormatGroupedAmount(gTotalPremium/100, 0)
  End Function
  
  Function DebitCounter()
    DebitCounter = Format_Number(gCounter - 1, 6)
  End Function  


      ]]>
	</msxsl:script>

	<xsl:template match="/">
      <!-- INSTALLATION HEADER RECORD  -->
      <!--Record Type-0-->
      <xsl:text>0</xsl:text>
      <!--Comma separation-->
      <xsl:text>,</xsl:text>
      <!--Batch File Version-TRG01-->
      <xsl:text>TRG01</xsl:text>
      <!--Comma separation-->
      <xsl:text>,</xsl:text>
      <!--Number of User supplying file,financial institution code-->
      <xsl:text>123456</xsl:text>
      <!--Comma separation-->
      <xsl:text>,</xsl:text>
      <!--File Creation Date-->
      <xsl:value-of select="user:Format_Date(string(ACB:EXPORT_HEADER/@date_exported))"/>
      <!--Comma separation-->
      <xsl:text>,</xsl:text>
      <xsl:for-each select="ACB:EXPORT_HEADER/ACB:PAYMENT">
        <!--Record Counter-->
          <xsl:variable name="Count" select="user:CounterInc()" />
       </xsl:for-each>
      <!--Record Count-->
      <xsl:value-of select="user:Counter(20) "/>
      <xsl:text>&#10;</xsl:text>

    
	<!-- Details RECORD -->
	<xsl:for-each select="ACB:EXPORT_HEADER/ACB:PAYMENT">
        <!--Record Identifier-->
				<xsl:text>1</xsl:text>
        <!--Comma separation-->
        <xsl:text>,</xsl:text>
        <!--Number of User supplying file,financial institution code-->
        <xsl:text>123456</xsl:text>
        <!--Comma separation-->
        <xsl:text>,</xsl:text>
        <!--Transaction Type-->
        <xsl:text>TRE</xsl:text>
        <!--Comma separation-->
        <xsl:text>,</xsl:text>
        <!--Transaction Source-->
        <xsl:text>E</xsl:text>
        <!--Comma separation-->
        <xsl:text>,</xsl:text>
        <!--Customer Reference Number,policy number-->
        <xsl:value-of select="user:FormatText(normalize-space(@policy_number),0)"/>
        <!--Comma separation-->
        <xsl:text>,</xsl:text>
        <!--Customer Transaction Reference ,Plan Refrence-->
        <xsl:text>NIA-</xsl:text>
        <xsl:value-of select="user:FormatText(normalize-space(@cashlistitem_id),0)"/>
        <!--Comma separation-->
        <xsl:text>,</xsl:text>
        <!--Currency-->
        <xsl:text>AUD</xsl:text>
        <!--Comma separation-->
        <xsl:text>,</xsl:text>
        <!--Grouped Amount-->
        <xsl:value-of select="user:FormatGroupedAmount(@amount, 0)"/>
        <!--Amount Adding Loop-->
        <xsl:variable name="AmountCount" select="user:AmountAdd(@amount*100)" />
        <!--Comma separation-->
        <xsl:text>,</xsl:text>
        <!--Recurring Option-->
        <xsl:text>Y</xsl:text>
        <!--Comma separation-->
        <xsl:text>,</xsl:text>
        <!--Optional Original Bank Transaction ID--><!--
        <xsl:value-of select="user:MultipleSpaces(12, ' ')"/>-->
		    <!--Comma separation-->
        <xsl:text>,</xsl:text>
        <!--payment name-->
        <xsl:value-of select="user:FormatText(normalize-space(@payment_name),0)"/>
        <xsl:text>&#xa;</xsl:text>
		</xsl:for-each>
	
	
	 <!-- TRAILER RECORD -->
      <!--Record Identifier-->
      <xsl:text>9</xsl:text>
      <!--Comma separation-->
      <xsl:text>,</xsl:text>
      <!--Currency-->
      <xsl:text>AUD</xsl:text>
      <!--Comma separation-->
      <xsl:text>,</xsl:text>
      <!--Record Count-->
      <xsl:value-of select="user:Counter(10) "/>
      <!--Comma separation-->
      <xsl:text>,</xsl:text>
      <!--File(User) Net Total Amount in cents-->
      <xsl:text>0</xsl:text>
      <!--Comma separation-->
      <xsl:text>,</xsl:text>
      <!--Total Credit Card Refund-->
      <xsl:value-of select="user:ShowAmount() "/>
      <!--Comma separation-->
      <xsl:text>,</xsl:text>
      <!--Total Credit Card Pre-Authorisation-->
      <xsl:text>0</xsl:text>
      <!--Comma separation-->
      <xsl:text>,</xsl:text>
      <!--Total Direct Debit Amount-->
      <xsl:text>0</xsl:text>
    
	</xsl:template>
</xsl:stylesheet>
