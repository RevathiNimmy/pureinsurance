<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.ssp-worldwide.com" xmlns:MID="http://www.siriusfs.com/SFI/Export/MID_Export/20060420">
  <xsl:output omit-xml-declaration="yes"  method="text" />
  <msxsl:script language="VBScript" implements-prefix="user">
    <![CDATA[
		
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
            ElseIf iLen >  intNoOfDigits Then
                Format_Number = Right(strInput, intNoOfDigits)             
            Else
                Format_Number = strInput                   
            End If
        End function
        
        Function Format_Date(ByVal strInput)
            If Trim(strInput) = "" Then
                Format_Date = MultipleSpaces(8, " ")
            Else
                Format_Date = Year(strInput) & Format_Number(Month(strInput), 2) & Format_Number(Day(strInput), 2)
            End If
        End function

        Function Format_Date_PolicyHolder(ByVal strInput)
            If Trim(strInput) = "" Then
                Format_Date_PolicyHolder = MultipleSpaces(8, "0")
            Else
                Format_Date_PolicyHolder = Year(strInput) & Format_Number(Month(strInput), 2) & Format_Number(Day(strInput), 2)
            End If
        End function

        Function getDate()
            getDate = Now()
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
		
        Function FormatText(ByVal strInput As String, ByVal intLen As Integer, ByVal Lft As Integer)
            Dim l As Integer
            Dim i As Integer
			
            l = Len(strInput)
            If l < intLen Then
                FormatText = strInput
                For i = 1 To intLen - l
                    FormatText = FormatText & " "
                Next
            ElseIf l > intLen Then
                If Lft = 0 Then
                    FormatText = Left(strInput, intLen)
                Else
                    FormatText = Right(strInput, intLen)
                End If
            Else
                FormatText = strInput
            End If
        End Function 
		
        Function UpperCaseText(ByVal strInput As String)
            UpperCaseText = UCase(strInput)
        End function
        
        Function Format_String(ByVal strValue , ByVal intNoOfDigits ) 
            Dim iLen, i As Integer
            Format_String =  strValue
            
            iLen = Len(strValue)         
            If iLen <  intNoOfDigits Then
                For i = iLen + 1 To intNoOfDigits 
                    Format_String = Format_String & " "
                Next
            ElseIf iLen >  intNoOfDigits Then
                Format_String = Left(strValue, intNoOfDigits) 
            End If			 
        End function         
Function Print_Drivers(ByVal strDriverName As String, ByVal strDriverDOB As String, ByVal strDriverDL As String, ByVal strDriverExcludedDriver As String)  As String
            Dim aoDriverName 
            Dim aoDriverDOB 
            Dim aoDriverDL 
            Dim aoDriverExcluded_Driver 
            Dim strAllDriverDetails As String=""
            Dim nLen, nCount As Integer
			      Dim nAge As Integer
            Dim  dtDOB As Date
			            
            aoDriverName=Split(strDriverName,"~")
            aoDriverDOB=Split(strDriverDOB,"~")
            aoDriverDL=Split(strDriverDL,"~")
            aoDriverExcluded_Driver=Split(strDriverExcludedDriver,"~")
            nLen=5
            For nCount = 0 To nLen
                If nCount  <= UBound(aoDriverName) Then
                  strAllDriverDetails = strAllDriverDetails & FormatText(aoDriverName(nCount),70,0)                
                Else
                  strAllDriverDetails = strAllDriverDetails & FormatText(" ",70,0) 
                End If
                If nCount  <= UBound(aoDriverExcluded_Driver) Then
                  strAllDriverDetails = strAllDriverDetails & FormatText(aoDriverExcluded_Driver(nCount),1,0)                
                Else
                  strAllDriverDetails = strAllDriverDetails & FormatText(" ",1,0) 
                End If
               If nCount  <= UBound(aoDriverDOB) Then
                  IF(Trim(aoDriverDOB(nCount)) <> "") Then
                      strAllDriverDetails = strAllDriverDetails & Format_Date(aoDriverDOB(nCount))    
                  
				              dtDOB = CDate(aoDriverDOB(nCount))
                      nAge=DateDiff("d",dtDOB,now)
                      nAge = Left(nAge / 365,2)
				              strAllDriverDetails = strAllDriverDetails & FormatText(nAge,3,0) 
                   Else
                       strAllDriverDetails = strAllDriverDetails & FormatText(" ",8,0) 
                       strAllDriverDetails = strAllDriverDetails & FormatText(" ",3,0) 
                  End If             
                  
                Else
                  strAllDriverDetails = strAllDriverDetails & FormatText(" ",8,0) 
				  
				        ' Display Age of driver				
                strAllDriverDetails = strAllDriverDetails & FormatText(" ",3,0) 
                End If
				
                				
                ' Set 20 char of Named Driver Driving LicenceNumber 
                'If nCount  <= UBound(aoDriverDL) Then
                  'strAllDriverDetails = strAllDriverDetails & FormatText(aoDriverDL(nCount),20,0)                
                'Else
                  strAllDriverDetails = strAllDriverDetails & FormatText(" ",20,0) 
                'End If  
            Next
                       
           Return strAllDriverDetails
        End function		
      ]]>
  </msxsl:script>
  <xsl:template match="/">
    <!--HEADER  RECORD  -->
    <!--Record Type-->
    <xsl:text>H</xsl:text>
    <!--File Version Number-->
    <xsl:text>0001</xsl:text>
    <!--Supplier Type-->
    <xsl:choose>
      <xsl:when test="MID:EXPORT_HEADER/@supplier_type='Insurer'">
        <xsl:text>I</xsl:text>
      </xsl:when>
      <xsl:when test="MID:EXPORT_HEADER/@supplier_type='Delegated Authority'">
        <xsl:text>D</xsl:text>
      </xsl:when>
    </xsl:choose>
    <!--Supplier Id-->
    <xsl:value-of select="user:Format_Number(normalize-space(MID:EXPORT_HEADER/@supplier_id), 3)"/>
    <!--Site Number-->
    <xsl:value-of select="user:Format_Number(normalize-space(MID:EXPORT_HEADER/@site_number), 3)"/>
    <!--Test Indicator-->
    <xsl:value-of select="user:Format_String(normalize-space(MID:EXPORT_HEADER/@test_indicator), 1)"/>
    <!--File Sequence Number-->
    <xsl:variable name="vFileSequenceNumber" select="user:Format_Number(normalize-space(MID:EXPORT_HEADER/@file_Seq_Number), 6)"/>
    <xsl:value-of select="$vFileSequenceNumber"/>
    <!--File Production Date-->
    <xsl:value-of select="user:Format_Date(string(MID:EXPORT_HEADER/@date_exported))"/>
    <!--File Production Time-->
    <!--<xsl:text>0005</xsl:text>-->
    <xsl:value-of select="user:Format_Time(string(MID:EXPORT_HEADER/@date_exported))"/>
    <xsl:text>#</xsl:text>
    <xsl:text>&#13;&#10;</xsl:text>

    <!--    POLICY  RECORD  -->
    <xsl:for-each select="MID:EXPORT_HEADER/MID:EXPORT_POLICY">
      <!--Record Type-->
      <xsl:text>P</xsl:text>
      <!--Update Type-->
      <xsl:variable name="vUpdateType" select="user:UpperCaseText(substring(normalize-space(@update_type),1,1))"/>
      <xsl:value-of select="$vUpdateType"/>
      <!-- This is a constant value and will be supplied by Catlin when they confirm the account with MID.-->
      <!--Insurer ID-->
      <xsl:value-of select="user:Format_Number(normalize-space(@insurer_id), 3)"/>
      <!-- This is a constant value and will be supplied by Catlin when they confirm the account with MID.-->
      <!--Insurer Branch ID-->
      <xsl:value-of select="user:MultipleSpaces(10, ' ')"/>
      <!--Quoteback-->
      <xsl:value-of select="user:MultipleSpaces(35, ' ')"/>
      <!--Delegated Authority ID-->
      <xsl:value-of select="user:Format_Number(normalize-space(@delegated_authority_id), 3)"/>
      <!--DA Branch ID-->
      <xsl:value-of select="user:Format_String(normalize-space(@da_branch_id), 20)"/>
      <!--Policy Number-->
      <xsl:variable name="vPolicyNumber" select="user:FormatText(normalize-space(@insurance_ref), 20, 1)"/>
      <xsl:value-of select="$vPolicyNumber"/>
      <!--Party Policy Control Count-->
      <xsl:value-of select="user:Format_Number(normalize-space(@ppcc),4)"/>
      <!--Foreign Registration Indicator-->
      <xsl:choose>
        <xsl:when test="vUpdateType = 'D'">
          <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="@is_foreign_registration = 'true'">
              <xsl:text>F</xsl:text>
            </xsl:when>
            <xsl:when test="@is_foreign_registration = '1'">
              <xsl:text>F</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>U</xsl:text>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
      <!--Vehicle Registration Mark-->
      <xsl:value-of select="user:UpperCaseText(user:FormatText(normalize-space(@registration), 12, 0))"/>
      <!--Vehicle Identification Number-->
      <xsl:value-of select="user:UpperCaseText(user:FormatText(normalize-space(@Vehicle_VIN), 20, 0))"/>
      <!--Vehicle Make and Model-->
      <!--<xsl:choose>
        <xsl:when test="$vUpdateType = 'D'">
          <xsl:value-of select="user:MultipleSpaces(30, ' ')"/>
        </xsl:when>
        <xsl:otherwise>-->
      <xsl:value-of select="user:FormatText(normalize-space(concat(@make, @model)), 50, 0)"/>
      <!--</xsl:otherwise>
      </xsl:choose>-->
      <!--Vehicle Code-->
      <xsl:text>00000000</xsl:text>
      <!--Vehicle Cover Type-->
      <xsl:value-of select="user:Format_String(normalize-space(@Vehicle_CoverType), 2)"/>
      <!--Permitted Drivers-->
      <xsl:value-of select="user:FormatText(normalize-space(@permitted_drivers), 2, 0)"/>
      <!--Class of Use-->
      <xsl:value-of select="user:Format_String(normalize-space(@class_use), 3)"/>
      <!--Effective Start Date-->
      <xsl:variable name="vInsFileType" select="normalize-space(@insurance_file_type_code)"/>
      <xsl:variable name="vPolicyCanDel" select="$vInsFileType = 'MTACAN' or $vUpdateType = 'D'"/>
      <xsl:variable name="vUseTransDate" select="($vInsFileType = 'WRITTEN' or $vInsFileType = 'POLICY') and $vUpdateType = 'A'"/>
      <xsl:choose>
        <xsl:when test="$vUseTransDate = 'true'">
          <xsl:value-of select="user:Format_Date(string(user:getDate()))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="user:Format_Date(string(@cover_start_date))"/>
        </xsl:otherwise>
      </xsl:choose>
      <!--Effective Start Time-->
      <xsl:text>0000</xsl:text>
      <!--Date of Expiry-->
      <xsl:choose>
        <xsl:when test="normalize-space(@insurance_file_type_code) = 'MTACAN' and $vUpdateType = 'A'">
          <xsl:value-of select="user:Format_Date(string(@cover_start_date))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="user:Format_Date(string(@cover_end_date))"/>
        </xsl:otherwise>
      </xsl:choose>
      <!--Time of Expiry-->
      <xsl:text>0000</xsl:text>
      <!--Cancellation/Lapse Indicator-->
      <xsl:choose>
        <xsl:when test="normalize-space(@insurance_file_status_code) = 'LAP' and $vUpdateType = 'A'">
          <xsl:text>L</xsl:text>
        </xsl:when>
        <xsl:when test="normalize-space(@insurance_file_type_code) = 'MTACAN' and $vUpdateType = 'A'">
          <xsl:text>C</xsl:text>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
        </xsl:otherwise>
      </xsl:choose>
      <!--Company Name Indicator-->
      <!--<xsl:choose>
        <xsl:when test="$vUpdateType = 'D'">
          <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
        </xsl:when>
        <xsl:otherwise>-->
      <xsl:choose>
        <xsl:when test="normalize-space(@party_type_code) = 'PC'">
          <xsl:text>P</xsl:text>
        </xsl:when>
        <xsl:when test="normalize-space(@party_type_code) = 'CC'">
          <xsl:text>C</xsl:text>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
        </xsl:otherwise>
      </xsl:choose>
      <!--</xsl:otherwise>
      </xsl:choose>-->
      <!--Policyholder Name-->
      <xsl:value-of select="user:FormatText(normalize-space(@insured_name), 70, 0)"/>
      <!--Policyholder Date of Birth-->
      <xsl:value-of select="user:Format_Date_PolicyHolder(string(@policyholder_dob))"/>
      <!--Policyholder Age-->
      <xsl:text>000</xsl:text>
      <!--Address Line 1-->
      <xsl:value-of select="user:FormatText(normalize-space(@insured_address1), 40, 0)"/>
      <!--Address Line 2-->
      <xsl:value-of select="user:FormatText(normalize-space(@insured_address2), 40, 0)"/>
      <!--Address Line 3-->
      <xsl:value-of select="user:FormatText(normalize-space(@insured_address3), 40, 0)"/>
      <!--Address Line 4-->
      <xsl:value-of select="user:FormatText(normalize-space(@insured_address4), 40, 0)"/>
      <!--Address Line 5-->
      <xsl:value-of select="user:MultipleSpaces(40, ' ')"/>
      <!--Address Line 6-->
      <xsl:value-of select="user:MultipleSpaces(40, ' ')"/>
      <!--Postcode-->
      <xsl:value-of select="user:FormatText(normalize-space(@insured_postcode), 8, 0)"/>
      <!--Policyholder Driving Licence Number-->
      <xsl:value-of select="user:MultipleSpaces(20, ' ')"/>
      <!--Policy	Policyholder Driving Other Vehicles-->
      <xsl:value-of select="user:FormatText(normalize-space(@Policyholder_DrivingOtherVehicles), 1, 0)"/>
      <!--Non Returned Certificate-->
      <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
      <!--NCD years-->
      <xsl:value-of select="user:MultipleSpaces(2, ' ')"/>
      <!--NCD Percentage Discount-->
      <xsl:value-of select="user:MultipleSpaces(2, ' ')"/>
      <!--NCD Entitlement Reason-->
      <xsl:value-of select="user:MultipleSpaces(2, ' ')"/>
      <!--NCD Discount Type-->
      <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
      <!--Protected NCD Indicator-->
      <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
      <!--Incident Pending Flag-->
      <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
      <!--Additional Drivers Indicator-->
      <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
      <!--Number of Named Drivers-->
      <xsl:choose>
        <xsl:when test="$vUpdateType = 'D'">
          <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:text>0</xsl:text>
        </xsl:otherwise>
      </xsl:choose>
      <!--Named Driver Name,Excluded Driver,Named Driver Date of Birth,Named Driver Age and Named Driver Driving Licence Number-->
      <xsl:value-of select="user:Print_Drivers(@driverName,@driverDOB,@Driving_license_number,@excluded_Driver)"/>
      <!--Named Driver Name-->
      <!--
      <xsl:value-of select="user:FormatText(normalize-space(@driverName), 70, 0)"/>
      -->
      <!--Excluded Driver-->
      <!--
      <xsl:value-of select="user:FormatText(normalize-space(@excluded_Driver), 1, 0)"/>
      -->
      <!--Named Driver Date of Birth-->
      <!--
      <xsl:value-of select="user:Format_Date(string(@driverDOB))"/>
      -->
      <!--Named Driver Age-->
      <!--
      <xsl:value-of select="user:MultipleSpaces(3, ' ')"/>
      -->
      <!--Named Driver Driving Licence Number-->
      <!--
      <xsl:value-of select="user:FormatText(normalize-space(@Driving_license_number), 20, 0)"/>-->
      <xsl:text>#</xsl:text>
      <xsl:text>&#13;&#10;</xsl:text>
    </xsl:for-each>

    <!--    TRAILER RECORD  -->
    <!--Record Type-->
    <xsl:text>T</xsl:text>
    <!--File Sequence Number-->
    <xsl:value-of select="$vFileSequenceNumber"/>
    <!--Data Record Count-->
    <xsl:value-of select="user:Format_Number(count(//MID:EXPORT_HEADER/MID:EXPORT_POLICY) + count(//MID:EXPORT_HEADER/MID:EXPORT_POLICY/MID:EXPORT_VEHICLE) + 2, 9)"/>
    <xsl:text>#</xsl:text>
  </xsl:template>
</xsl:stylesheet>
