<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.ssp-worldwide.com" xmlns:MID2="http://www.siriusfs.com/SFI/Export/MID2_Export/20060420">
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
           
      ]]>
  </msxsl:script>
  <xsl:template match="/">
    <!--    HEADER  RECORD  -->
    <!--Record Type-->
    <xsl:text>B</xsl:text>
    <!--File Version Number-->
    <xsl:text>0001</xsl:text>
    <!--Supplier Type-->
    <xsl:choose>
      <xsl:when test="MID2:EXPORT_HEADER/@supplier_type='Insurer'">
        <xsl:text>I</xsl:text>
      </xsl:when>
      <xsl:when test="MID2:EXPORT_HEADER/@supplier_type='Delegated Authority'">
        <xsl:text>D</xsl:text>
      </xsl:when>
    </xsl:choose>
    <!--Supplier Id-->
    <xsl:value-of select="user:Format_Number(normalize-space(MID2:EXPORT_HEADER/@supplier_id), 3)"/>
    <!--Site Number-->
    <xsl:value-of select="user:Format_Number(normalize-space(MID2:EXPORT_HEADER/@site_number), 3)"/>
    <!--Test Indicator-->
    <xsl:value-of select="user:Format_String(normalize-space(MID2:EXPORT_HEADER/@test_indicator), 1)"/>
    <!--File Sequence Number-->
    <xsl:variable name="vFileSequenceNumber" select="user:Format_Number(normalize-space(MID2:EXPORT_HEADER/@file_Seq_Number), 6)"/>
    <xsl:value-of select="$vFileSequenceNumber"/>
    <!--File Production Date-->
    <xsl:value-of select="user:Format_Date(string(MID2:EXPORT_HEADER/@date_exported))"/>
    <!--File Production Time-->
    <xsl:text>0000</xsl:text>
    <!--<xsl:value-of select="user:Format_Time(string(MID2:EXPORT_HEADER/@date_exported))"/>-->
    <xsl:text>#</xsl:text>
    <xsl:text>&#13;&#10;</xsl:text>

    <!--    POLICY  RECORD  -->
    <xsl:for-each select="MID2:EXPORT_HEADER/MID2:EXPORT_POLICY">
      <!--Record Type-->
      <xsl:text>F</xsl:text>
      <!--Update Type-->
      <xsl:variable name="vUpdateType" select="user:UpperCaseText(substring(normalize-space(@update_type),1,1))"/>
      <xsl:value-of select="$vUpdateType"/>
      <!--Insurer ID-->
      <xsl:value-of select="user:Format_Number(normalize-space(@insurer_id), 3)"/>
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
      <!--Policy Access-->
      <xsl:value-of select="user:MultipleSpaces(6, ' ')"/>
      <xsl:variable name="vInsFileType" select="normalize-space(@insurance_file_type_code)"/>
      <xsl:variable name="vPolicyCanDel" select="$vInsFileType = 'MTACAN' or $vUpdateType = 'D'"/>
      <!-- This is a constant value and will be supplied by Catlin when they confirm the account with MID2.-->
      <!--Report Indicator ????-->
      <xsl:text>X</xsl:text>
      <!--Renewal Indicator ????-->
      <!--<xsl:choose>
        <xsl:when test="$vPolicyCanDel = 'true'">
          <xsl:value-of select="user:MultipleSpaces(2, ' ')"/>
        </xsl:when>
        <xsl:otherwise>-->
      <xsl:text>3A</xsl:text>
      <!--</xsl:otherwise>
      </xsl:choose>-->
      <!--Vehicle Record Indicator-->
      <xsl:choose>
        <!--<xsl:when test="count(MID2:EXPORT_VEHICLE) > 0 and not($vPolicyCanDel = 'true')">-->
        <xsl:when test="count(MID2:EXPORT_VEHICLE) > 0">
          <xsl:text>Y</xsl:text>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
        </xsl:otherwise>
      </xsl:choose>
      <!--Motor Trade Policy Indicator ????-->
      <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
      <!--Policyholder Name-->
      <xsl:value-of select="user:FormatText(normalize-space(@insured_name), 70, 0)"/>
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
      <!--Policyholder Contact Details-->
      <xsl:value-of select="user:MultipleSpaces(54, ' ')"/>
      <!--Effective Start Date ????-->
      <xsl:variable name="vUseTransDate" select="($vInsFileType = 'WRITTEN' or $vInsFileType = 'POLICY') and $vUpdateType = 'A'"/>
      <xsl:choose>
        <xsl:when test="$vUseTransDate = 'true'">
          <xsl:value-of select="user:Format_Date(string(user:getDate()))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="user:Format_Date(string(@cover_start_date))"/>
        </xsl:otherwise>
      </xsl:choose>
      <!--Date of Expiry-->
      <xsl:choose>
        <xsl:when test="normalize-space(@insurance_file_type_code) = 'MTACAN' and $vUpdateType = 'A'">
          <xsl:value-of select="user:Format_Date(string(@cover_start_date))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="user:Format_Date(string(@cover_end_date))"/>
        </xsl:otherwise>
      </xsl:choose>
      <!--Permitted Drivers-->
      <xsl:value-of select="user:MultipleSpaces(2, ' ')"/>
      <!--Class of Use ???? cols added-->
      <xsl:value-of select="user:MultipleSpaces(3, ' ')"/>
      <!--Cancellation/Lapse Indicator-->
      <xsl:choose>
        <xsl:when test="normalize-space(@insurance_file_status_code) = 'LAP' and $vUpdateType = 'A'">
          <xsl:text>L</xsl:text>
        </xsl:when>
        <xsl:when test="normalize-space(@insurance_file_type_code) = 'MTACAN' and $vUpdateType = 'A'">
          <xsl:text>C</xsl:text>
        </xsl:when>
        <xsl:when test="normalize-space(@insurance_file_type_code) = 'MTAREINS' and $vUpdateType = 'A'">
          <xsl:text>R</xsl:text>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
        </xsl:otherwise>
      </xsl:choose>
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
      <!--Named Driver Name-->
      <xsl:value-of select="user:MultipleSpaces(70, ' ')"/>
      <!--Excluded Driver-->
      <xsl:value-of select="user:MultipleSpaces(6, ' ')"/>
      <!--Estimated Fleet Size-->
      <xsl:value-of select="user:MultipleSpaces(7, ' ')"/>
      <xsl:text>#</xsl:text>
      <xsl:text>&#13;&#10;</xsl:text>

      <!--    VEHICLE  RECORD  -->
      <xsl:for-each select="MID2:EXPORT_VEHICLE">
        <!--Record Type-->
        <xsl:text>V</xsl:text>
        <!--Update Type-->
        <xsl:variable name="vVehUpdateType" select="user:UpperCaseText(substring(normalize-space(@update_type),1,1))"/>
        <xsl:value-of select="$vVehUpdateType"/>
        <!-- This is a constant value and will be supplied by Catlin when they confirm the account with MID2.-->
        <!--Insurer ID-->
        <xsl:value-of select="user:Format_Number(normalize-space(@insurer_id), 3)"/>
        <!-- This is a constant value and will be supplied by Catlin when they confirm the account with MID2.-->
        <!--Insurer Branch ID-->
        <xsl:value-of select="user:MultipleSpaces(10, ' ')"/>
        <!--Quoteback-->
        <xsl:value-of select="user:MultipleSpaces(35, ' ')"/>
        <!--Delegated Authority ID-->
        <xsl:value-of select="user:Format_Number(normalize-space(@delegated_authority_id), 3)"/>
        <!--DA Branch ID-->
        <xsl:value-of select="user:Format_String(normalize-space(@da_branch_id), 20)"/>
        <!--Policy Number-->
        <xsl:value-of select="$vPolicyNumber"/>
        <!--Foreign Registration Indicator-->
        <xsl:choose>
          <xsl:when test="$vVehUpdateType = 'D'">
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
        <!--Trade Plate Indicator ????-->
        <xsl:choose>
          <xsl:when test="$vVehUpdateType = 'D'">
            <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:choose>
              <xsl:when test="@is_trade_registration = 'true'">
                <xsl:text>T</xsl:text>
              </xsl:when>
              <xsl:when test="@is_trade_registration = '1'">
                <xsl:text>T</xsl:text>
              </xsl:when>
              <xsl:otherwise>
                <xsl:text>U</xsl:text>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:otherwise>
        </xsl:choose>
        <!--Vehicle Type-->
        <xsl:value-of select="user:MultipleSpaces(20, ' ')"/>
        <!--Vehicle Make-->
        <xsl:choose>
          <xsl:when test="$vVehUpdateType = 'D'">
            <xsl:value-of select="user:MultipleSpaces(15, ' ')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="user:FormatText(normalize-space(@make), 15, 0)"/>
          </xsl:otherwise>
        </xsl:choose>
        <!--Vehicle Model-->
        <xsl:choose>
          <xsl:when test="$vVehUpdateType = 'D'">
            <xsl:value-of select="user:MultipleSpaces(15, ' ')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="user:FormatText(normalize-space(@model), 15, 0)"/>
          </xsl:otherwise>
        </xsl:choose>
        <!--Vehicle Derivative-->
        <xsl:value-of select="user:MultipleSpaces(15, ' ')"/>
        <!--Vehicle Engine Size-->
        <xsl:value-of select="user:MultipleSpaces(5, ' ')"/>
        <!--Number of Seats-->
        <xsl:value-of select="user:MultipleSpaces(3, ' ')"/>
        <!--Gross Vehicle Weight-->
        <xsl:value-of select="user:MultipleSpaces(5, ' ')"/>
        <!--Vehicle Instep Code-->
        <xsl:value-of select="user:MultipleSpaces(8, ' ')"/>
        <!--Vehicle On Date-->
        <xsl:value-of select="user:Format_Date(string(@on_date))"/>
        <!--Vehicle Off Date-->
        <xsl:value-of select="user:Format_Date(string(@off_date))"/>
        <!--Permitted Drivers-->
        <xsl:value-of select="user:FormatText(normalize-space(@permitted_drivers), 2, 0)"/>
        <!--Class of Use-->
        <xsl:value-of select="user:FormatText(normalize-space(@class_use), 3, 0)"/>
        <!--Additional Drivers Indicator-->
        <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
        <!--Number of Named Drivers-->
        <xsl:choose>
          <xsl:when test="$vVehUpdateType = 'D'">
            <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>0</xsl:text>
          </xsl:otherwise>
        </xsl:choose>
        <!--Named Driver Name-->
        <xsl:value-of select="user:MultipleSpaces(70, ' ')"/>
        <!--Excluded Driver-->
        <xsl:value-of select="user:MultipleSpaces(1, ' ')"/>
        <xsl:text>#</xsl:text>
        <xsl:text>&#13;&#10;</xsl:text>
      </xsl:for-each>
    </xsl:for-each>
    <!--    TRAILER RECORD  -->
    <!--Record Type-->
    <xsl:text>Z</xsl:text>
    <!--File Sequence Number-->
    <xsl:value-of select="$vFileSequenceNumber"/>
    <!--Data Record Count-->
    <xsl:value-of select="user:Format_Number(count(//MID2:EXPORT_HEADER/MID2:EXPORT_POLICY) + count(//MID2:EXPORT_HEADER/MID2:EXPORT_POLICY/MID2:EXPORT_VEHICLE) + 2, 9)"/>
    <xsl:text>#</xsl:text>
  </xsl:template>
</xsl:stylesheet>
