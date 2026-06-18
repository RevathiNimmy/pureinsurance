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
     If intInput <> "" Then       
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
      strSecondpart=Right(strInput,intNoOfDigits-3)

        Format_hiphencode = strFirstpart & "-" & strSecondpart  
      else
        For i = 1 To intNoOfDigits + 1
          spacing = spacing & " "
        Next 
        intInput =  intInput  & spacing
      Format_hiphencode = intInput
     End If
     
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
      Format_Date = MultipleSpaces(8, " ")
    Else
      Format_Date= Format_Number(Day(strInput) ,2) & Format_Number(Month(strInput), 2) & GetRight(Year(strInput),2)
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
    
  Function CounterInc() 
    gCounter = gCounter + 1
    CounterInc = Format_Number(gCounter, 6)
  End Function
  
  Function Counter()
    Counter = Format_Number(gCounter, 6)
  End Function
  
  Function DebitCounter()
    DebitCounter = Format_Number(gCounter - 1, 6)
  End Function  
    
Function AmountAdd(Byval amount as Integer) 
    gTotalPremium = gTotalPremium +  amount
    AmountAdd = gTotalPremium
End Function
  
   Function ShowAmount()
    ShowAmount = FormatGroupedAmount(gTotalPremium/100, 0)
  End Function
  
  Function StandardAccountNo(ByVal planBankAcctNo As String)
    If Len(planBankAcctNo) > 9 Then
      StandardAccountNo = planBankAcctNo
    Else
    StandardAccountNo = planBankAcctNo
    For i = 1 To 9-Len(planBankAcctNo) 
		 StandardAccountNo =  " " & StandardAccountNo 
	  Next
    End If
  End Function
  
      ]]>
	</msxsl:script>

	<xsl:template match="/">
      <!-- PAYMENT HEADER RECORD  -->
      <!--Record Type-->
      <xsl:text>0</xsl:text>
      <!--Tape Serial Number-->
      <xsl:value-of select="user:MultipleSpaces(17, ' ')"/>
      <!--Reel Sequence Number-->
      <xsl:text>01</xsl:text>
      <!--Name of User Financial Institution-->
      <xsl:text>NAB</xsl:text>
      <!--Tape Serial Number-->
      <xsl:value-of select="user:MultipleSpaces(7, ' ')"/>
      <!--Name of User supplying file-->
      <xsl:text>NIA Underwriting Agency</xsl:text>
      <!--Tape Serial Number-->
      <xsl:value-of select="user:MultipleSpaces(3, ' ')"/>
      <!--Number of User supplying file-->
      <xsl:text>123456</xsl:text>
      <!--Description of User supplying file.-->
      <xsl:text>InsurancePrm</xsl:text>
      <!--Creation Date-->
      <xsl:value-of select="user:Format_Date(string(ACB:EXPORT_HEADER/@date_exported))"/>
      <!--Tape Serial Number-->
      <xsl:value-of select="user:MultipleSpaces(40, ' ')"/>
      <xsl:text>&#10;</xsl:text>
	
	<!-- Details RECORD -->
	<xsl:for-each select="ACB:EXPORT_HEADER/ACB:PAYMENT">
            <!--<xsl:variable name="vEachCount" select="position()" /> -->
            <!--Record Identifier-->
				    <xsl:text>1</xsl:text>
				    <!--BSB Number as Payment Branch Code-->
	          <xsl:value-of select="user:Format_hiphencode(normalize-space(@payment_branch_code),6)"/>
				    <!--Payment Account Code-->
				    <xsl:value-of select="user:StandardAccountNo(normalize-space(@payment_account_code))"/>
            <!--Tape Serial Number-->
            <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
            <!--Transaction Code-->
            <xsl:text>50</xsl:text>
            <!--Grouped Amount-->
            <xsl:value-of select="user:FormatGroupedAmount(@amount, 0)"/>
            <!--Amount Adding Loop-->
            <xsl:variable name="AmountCount" select="user:AmountAdd(@amount*100)" />
            <!--Account Code-->
            <xsl:value-of select="user:FormatText(normalize-space(@account_code),32)"/>
            <!--Policy Number-->
            <xsl:value-of select="user:FormatText(normalize-space(@policy_number),18)"/>
            <!--Bank bank code-->
            <xsl:value-of select="user:Format_hiphencode(normalize-space(@bankaccount_code),6)"/>
            <!--PAYMENT bank Number-->
            <!--<xsl:value-of select="user:Format_Right_justify_Number(normalize-space(/@scheme_bank_number),9)"/>-->
            <xsl:text>123456789</xsl:text>
            <!--Grouped Refrence tag-->
            <xsl:text>NIA-</xsl:text>
            <!--Grouped Refrence-->
            <xsl:value-of select="user:FormatText(normalize-space(@cashlistitem_id),12)"/>
            <!--Amount of withholding tax,fixed zeros-->
            <xsl:text>00000000</xsl:text>
            <!--Record Counter-->
            <xsl:variable name="Count" select="user:CounterInc()" />
            <xsl:text>&#xa;</xsl:text>
   </xsl:for-each>
    
    <!-- BALANCING ENTRY -->
    <!--Record Identifier-->
    <xsl:text>1</xsl:text>
    <!--PAYMENT bank code-->
    <xsl:value-of select="user:Format_hiphencode(normalize-space(//@bankaccount_code),6)"/>
    <!--PAYMENT bank Number-->
    <!--<xsl:value-of select="user:Format_Right_justify_Number(normalize-space(//@PAYMENT_bank_number),9)"/>-->
    <xsl:text>123456789</xsl:text>
    <!--Tape Serial Number-->
    <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
    <!--Transaction Code-->
    <xsl:text>50</xsl:text>
    <!--Amount Total Amount in cents-->
    <xsl:value-of select="user:ShowAmount() "/>
    <!--Account Title to be credited/debited-->
    <xsl:text>NIA</xsl:text>
    <!--Tape Serial Number-->
    <xsl:value-of select="user:MultipleSpaces(29, ' ')"/>
    <!--Lodgement Reference-->
    <xsl:value-of select="user:MultipleSpaces(18, ' ')"/>
    <!--BSB Number:-PAYMENT bank code-->
    <xsl:value-of select="user:Format_hiphencode(normalize-space(//@bankaccount_code),6)"/>
    <!--Account Number:-PAYMENT bank Number-->
    <!--<xsl:value-of select="user:Format_Right_justify_Number(normalize-space(//@PAYMENT_bank_number),9)"/>-->
    <xsl:text>123456789</xsl:text>
    <!--Name of Remitter-->
    <xsl:text>NIA</xsl:text>
    <!--Tape Serial Number-->
    <xsl:value-of select="user:MultipleSpaces(13, ' ')"/>
    <!--Amount of withholding tax,fixed zeros-->
    <xsl:text>00000000</xsl:text>
    <xsl:text>&#10;</xsl:text>
    
  <!-- TRAILER RECORD -->
      <!--Record Identifier-->
      <xsl:text>7</xsl:text>
      <!--BSB Number-->
      <xsl:text>999-999</xsl:text>
      <!--Tape Serial Number-->
      <xsl:value-of select="user:MultipleSpaces(12, ' ')"/>
      <!--File(User) Net Total Amount in cents-->
      <xsl:value-of select="user:ShowAmount() "/>
      <!--File(User) Credit Total Amount in cents-->
      <xsl:text>0000000000</xsl:text>
      <!--File(User) Debit Total Amount in cents-->
      <xsl:value-of select="user:ShowAmount() "/>
      <!--Tape Serial Number-->
      <xsl:value-of select="user:MultipleSpaces(24, ' ')"/>
      <!--Record Count-->
      <xsl:value-of select="user:Counter() "/>
      <!--Tape Serial Number-->
      <xsl:value-of select="user:MultipleSpaces(40, ' ')"/>
	

	</xsl:template>
</xsl:stylesheet>
