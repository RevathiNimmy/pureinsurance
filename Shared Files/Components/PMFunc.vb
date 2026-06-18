Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Text
<System.Runtime.InteropServices.ProgId("PMFunc_NET.PMFunc")> _
 Public Module PMFunc
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	'
	' Application general functions module. Contains all of the global
	' functions that might be useful when writing an application.
	'
	' Edit History TF311097 ConvertWildCardsForSQL Added
	'              BB161297 Decimal pounds and whole pounds added for Voyager
	' ***************************************************************** '
	
	
	' Constant for the methods to identify
	' which class this is.
	Private Const ACClass As String = "PMFunc"
	
	Private Declare Function GetComputerName Lib "kernel32"  Alias "GetComputerNameA"(ByVal lpBuffer As String, ByRef nSize As Integer) As Integer
	
	' ***************************************************************** '
	' Name: PMRoundUp
	'
	' Description: Takes a Double value :
	'              1. Rounds up by adding a rounding factor derived from
	'                 the number of decimal places requested.
	'              2. Truncates to the number of decimal places specified.
	'
	' ***************************************************************** '
	Public Function PMRoundUp(ByVal dValueIn As Double, ByVal iNumOfPlaces As Integer) As Double
		
		Dim dRoundFactor As Double
		Const dPointFourNine As Double = 0.49
		Dim dWorkValue As Double
		
		Try 
			
			' Is Number of Decimal Places Requested Invalid
			If iNumOfPlaces < 0 Then
				iNumOfPlaces = 0
			End If
			
			' Is Number of Decimal Places Requested Invalid
			If iNumOfPlaces > 8 Then
				iNumOfPlaces = 8
			End If
			
			' Derive the Rounding Factor
			dRoundFactor = dPointFourNine / (10 ^ iNumOfPlaces)
			
			' Add the rounding factor
			dWorkValue = Math.Abs(dValueIn) + dRoundFactor
			
			' Truncate to the number of decimal places required
			
			Return Math.Sign(dValueIn) * PMTruncate(dValueIn:=dWorkValue, iNumOfPlaces:=iNumOfPlaces)
		
		Catch 
			
			
			
			' Error Section.
			
			' Return the original value.
			
			Return dValueIn
		End Try
		
	End Function
	
	' ***************************************************************** '
	' Name: PMStartOfWeek
	'
	' Description: Returns the start of the week date from the given
	'              date.
	'
	' ***************************************************************** '
	Public Function PMStartOfWeek(ByVal dtDate As Date) As Date
		
		Dim iInterval As Integer
		Dim dtSOWDate As Date
		
		Try 
			
			' Check what the current day is.
			If StringsHelper.Format(dtDate, "w") = FirstDayOfWeek.Sunday Then
				' Set the interval value.
				iInterval = -6
			Else
				' Set the interval value.
				iInterval = (CDbl(StringsHelper.Format(dtDate, "w")) - 2) * -1
			End If
			
			' Calculate the start of the week.
			dtSOWDate = dtDate.AddDays(iInterval)
			
			' Return the new date.
			
			Return dtSOWDate
		
		Catch 
			
			
			
			' Error Section.
			
			
			Return dtDate
		End Try
		
	End Function
	
	' ***************************************************************** '
	' Name: PMTruncate
	'
	' Description: Takes a Double value and Truncates to the
	'              number of decimal places specified.
	'
	' ***************************************************************** '
	Public Function PMTruncate(ByVal dValueIn As Double, ByVal iNumOfPlaces As Integer) As Double
		
        Dim lMultiplier As Integer
		Dim dTemp As Double
		
		Try 
			
			' Is Number of Decimal Places Requested Invalid
			If iNumOfPlaces < 0 Then
				iNumOfPlaces = 0
			End If
			
			' Is Number of Decimal Places Requested Invalid
			If iNumOfPlaces > 8 Then
				iNumOfPlaces = 8
			End If
			
			' Derive the multiplier based on the number of decimal places
			lMultiplier = 10 ^ iNumOfPlaces
			
			'CLng rounds!
			'    lIntermediate& = CLng(dValueIn# * lMultiplier&)
			
			'Tomo22082002
			'But this goes berserk - try rounding 349.95 to 4dp, you get 349.9499
			'AND what happens when you round 10 million to 4 dp - overflow of a long
			
			'    lIntermediate& = Int(dValueIn# * lMultiplier&)
			dTemp = dValueIn * lMultiplier
			dTemp = Math.Floor(dTemp)
			
			' PMTruncate the Value
			'    PMTruncate# = (lIntermediate& / lMultiplier&)
			
			Return dTemp / lMultiplier
		
		Catch 
			
			
			
			' Error Section.
			
			' Return the original value.
			
			Return dValueIn
		End Try
		
	End Function
	
	'SD Remove dupliacte function
	'' ***************************************************************** '
	'' Name: FormatField
	''
	'' Description: Formats a field to the type specified.
	''
	'' ***************************************************************** '
	'Public Function FormatField( _
	''    iFormatType As Integer, _
	''    vFieldValue As Variant) As String
	'
	'Dim sControlResult As String
	'
	'    On Error GoTo Err_FormatField
	'
	'    ' Check for a null value
	'    If (IsNull(vFieldValue) = True) Then
	'        vFieldValue = ""
	'    End If
	'
	'    ' Determine which field type it is.
	'    Select Case (iFormatType%)
	'        Case PMFormatString
	'            ' Format value to a string.
	'            sControlResult$ = Trim$(vFieldValue)
	'
	'        Case PMFormatStringCase
	'            ' Format value to a string with proper case.
	'            sControlResult$ = Trim$(StrConv(vFieldValue, vbProperCase))
	'
	'        Case PMFormatStringUpper
	'            ' Format value to a string with uppercase.
	'            sControlResult$ = Trim$(UCase(vFieldValue))
	'
	'        Case PMFormatDateShort
	'            ' Format value to a short date
	'            If (IsDate(vFieldValue) = False) Then
	'                sControlResult$ = ""
	'            Else
	'                sControlResult$ = Trim$(Format$(vFieldValue, "short date"))
	'            End If
	'
	'        Case PMFormatDateMedium
	'            ' Format value to a medium date
	'            If (IsDate(vFieldValue) = False) Then
	'                sControlResult$ = ""
	'            Else
	'                sControlResult$ = Trim$(Format$(vFieldValue, "medium date"))
	'            End If
	'
	'        Case PMFormatDateLong
	'            ' Format value to a long date
	'            If (IsDate(vFieldValue) = False) Then
	'                sControlResult$ = ""
	'            Else
	'                sControlResult$ = Trim$(Format$(vFieldValue, "long date"))
	'            End If
	'
	'        Case PMFormatTimeShort
	'            ' Format value to a short time
	'            sControlResult$ = Trim$(Format$(vFieldValue, "short time"))
	'
	'        Case PMFormatTimeMedium
	'            ' Format value to a medium time
	'            sControlResult$ = Trim$(Format$(vFieldValue, "medium time"))
	'
	'        Case PMFormatTimeLong
	'            ' Format value to a long time
	'            sControlResult$ = Trim$(Format$(vFieldValue, "long time"))
	'
	'        Case PMFormatDateTimeShort
	'            ' Format value to a short date and time
	'            sControlResult$ = Trim$(Format$(vFieldValue, "short date")) & _
	''                " " & Trim$(Format$(vFieldValue, "short time"))
	'
	'        Case PMFormatDateTimeMedium
	'            ' Format value to a medium date and time
	'            sControlResult$ = Trim$(Format$(vFieldValue, "medium date")) & _
	''                " " & Trim$(Format$(vFieldValue, "medium time"))
	'
	'        Case PMFormatDateTimeLong
	'            ' Format value to a long date and time
	'            sControlResult$ = Trim$(Format$(vFieldValue, "long date")) & _
	''                " " & Trim$(Format$(vFieldValue, "long time"))
	'
	'        Case PMFormatDateYearOnly
	'            ' Format value to a year only date
	'            sControlResult$ = Trim$(Format$(vFieldValue, "yyyy"))
	'
	'        Case PMFormatCurrency
	'            ' Format value to a currency
	'            If (IsNumeric(vFieldValue) = False) Then
	'                sControlResult$ = ""
	'            Else
	'                sControlResult$ = Trim$(Format$(vFieldValue, "standard"))
	'            End If
	'
	'        ' BB161297 Decimal pounds sterling for Voyager
	'        Case PMFormatMoney
	'            ' Format value to decimal pounds sterling
	'            If (IsNumeric(vFieldValue) = False) Then
	'                sControlResult$ = ""
	'            Else
	'                sControlResult$ = Trim$(Format$(vFieldValue, "\?#,###.00"))
	'            End If
	'
	'        ' BB161297 Whole pounds sterling for Voyager
	'        Case PMFormatWholeMoney
	'            ' Format value to a whole pounds sterling
	'            If (IsNumeric(vFieldValue) = False) Then
	'                sControlResult$ = ""
	'            Else
	'                ' Truncate any pence
	'                vFieldValue = PMTruncate(dValueIn:=vFieldValue, iNumOfPlaces:=0)
	'                sControlResult$ = Trim$(Format$(vFieldValue, "\?#,###"))
	'            End If
	'
	'        Case PMFormatInteger
	'            ' Format value to a currency
	''            If (IsNumeric(vFieldValue) = False) Then
	''                sControlResult$ = ""
	''            Else
	'                sControlResult$ = Trim$(Format$(vFieldValue, "General Number"))
	'                'CInt(vFieldValue)
	''            End If
	'
	'        Case PMFormatBoolean
	'            ' Format value to a boolean
	'            If (CInt(vFieldValue) = 0) Then
	'                sControlResult$ = False
	'            Else
	'                sControlResult$ = True
	'            End If
	'
	'        Case PMFormatPercent
	'            ' Format value to a currency
	'            If (IsNumeric(vFieldValue) = False) Then
	'                sControlResult$ = ""
	'            Else
	'                sControlResult$ = Trim$(Format$(vFieldValue, "standard")) & "%"
	'            End If
	'    End Select
	'
	'    ' Return the field value with the new
	'    ' formatted value.
	'    FormatField = sControlResult$
	'
	'    Exit Function
	'
	'Err_FormatField:
	'
	'    ' Error Section.
	'
	'    ' Return the original value.
	'    FormatField = vFieldValue
	'
	'    Exit Function
	'

	
	' ***************************************************************** '
	' Name: UnFormatField
	'
	' Description: Unformats a field to the type specified.
	'
	' ***************************************************************** '
	Public Function UnFormatField(ByRef iFormatTypeIn As Integer, ByRef iDataTypeOut As Integer, ByRef vFieldValue As Object) As Object
		
		Dim vControlResult As Object
		
		Try 
			
			' Check for a null value

			If Convert.IsDBNull(vFieldValue) Or IsNothing(vFieldValue) Then

				vFieldValue = ""
			End If
			
			' Check the format of the in value.
			Select Case (iFormatTypeIn)
				Case gPMConstants.PMEFormatStyle.PMFormatString, gPMConstants.PMEFormatStyle.PMFormatStringCase, gPMConstants.PMEFormatStyle.PMFormatStringUpper
					If iDataTypeOut <> gPMConstants.PMEDataType.PMString Then

						If CStr(vFieldValue).Trim() = "" Then

							vFieldValue = "0"
						End If
					End If
					
				Case gPMConstants.PMEFormatStyle.PMFormatPercent

					If CStr(vFieldValue).Substring(Strings.Len(CStr(vFieldValue)) - 1) = "%" Then


						vFieldValue = CStr(vFieldValue).Substring(0, Math.Min(CStr(vFieldValue).Length, Strings.Len(CStr(vFieldValue)) - 1))
					End If
					

					Dim dbNumericTemp As Double
					If Not Double.TryParse(CStr(vFieldValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

						vFieldValue = 0
					End If
					
				Case gPMConstants.PMEFormatStyle.PMFormatDateShort, gPMConstants.PMEFormatStyle.PMFormatDateMedium, gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMConstants.PMEFormatStyle.PMFormatTimeShort, gPMConstants.PMEFormatStyle.PMFormatTimeMedium, gPMConstants.PMEFormatStyle.PMFormatTimeLong

					If CStr(vFieldValue) = "" Or Not Information.IsDate(vFieldValue) Then

						vFieldValue = #12/29/1899#
					End If
					
					'BB161297
				Case gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMConstants.PMEFormatStyle.PMFormatInteger, gPMConstants.PMEFormatStyle.PMFormatLong, gPMConstants.PMEFormatStyle.PMFormatDouble, gPMConstants.PMEFormatStyle.PMFormatBoolean, gPMConstants.PMEFormatStyle.PMFormatMoney, gPMConstants.PMEFormatStyle.PMFormatWholeMoney

					Dim dbNumericTemp2 As Double
					If CStr(vFieldValue) = "" Or Not Double.TryParse(CStr(vFieldValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

						vFieldValue = 0
					End If
			End Select
			
			' Determine which field type it is.
			Select Case (iDataTypeOut)
				Case gPMConstants.PMEDataType.PMString

					Dim dbNumericTemp3 As Double
					If (Double.TryParse(CStr(vFieldValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3)) Or (Information.IsDate(vFieldValue)) Then
						' Format value to a string.


						vControlResult = CStr(vFieldValue).Trim()
					Else


						vControlResult = vFieldValue
					End If
					
				Case gPMConstants.PMEDataType.PMDate
					' Format value to a short date


					vControlResult = CDate(vFieldValue)
					
				Case gPMConstants.PMEDataType.PMCurrency


					vControlResult = CDec(vFieldValue)
					
				Case gPMConstants.PMEDataType.PMInteger, gPMConstants.PMEDataType.PMLong


					vControlResult = CInt(vFieldValue)
					
				Case gPMConstants.PMEDataType.PMDouble


					vControlResult = CDbl(vFieldValue)
					
				Case Else


					vControlResult = vFieldValue
			End Select
			
			' Return the field value with the new
			' formatted value.
			
			Return vControlResult
		
		Catch 
			
			
			
			' Error Section.
			
			' Return the original value.
			
			Return vFieldValue
		End Try
		
	End Function


    ' ***************************************************************** '
    ' Name: GetRegSettings
    '
    ' Description: Get settings from the registry.
    '
    ' ***************************************************************** '
	Public Function GetRegSettings(ByRef sResult As String, ByRef sAppName As String, ByRef sSection As String, ByRef sKey As String, Optional ByRef vDefault As String = "") As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Check if we have the optional parameter.

			If Information.IsNothing(vDefault) Then
				' Get setting from the registry not
				' using the optional parameter.
				sResult = Interaction.GetSetting(sAppName, sSection, sKey,  )
			Else
				' Get setting from the registry
				' using the optional parameter.
				sResult = Interaction.GetSetting(sAppName, sSection, sKey, vDefault)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetRegAllSettings
	'
	' Description: Get all section settings from the registry.
	'
	' ***************************************************************** '
	Public Function GetRegAllSettings(ByRef vResult( ,  ) As Object, ByRef sAppName As String, ByRef sSection As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get section setting from the registry.
			vResult = Interaction.GetAllSettings(sAppName, sSection)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegAllSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SaveRegSettings
	'
	' Description: Save settings to the registry.
	'
	' ***************************************************************** '
	Public Function SaveRegSettings(ByRef sSetting As String, ByRef sAppName As String, ByRef sSection As String, ByRef sKey As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Save setting to the registry.
			Interaction.SaveSetting(sAppName, sSection, sKey, sSetting)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveRegSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DeleteRegSettings
	'
	' Description: Delete settings from the registry.
	'
	' ***************************************************************** '
	Public Function DeleteRegSettings(ByRef sAppName As String, ByRef sSection As String, ByRef sKey As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Delete setting from the registry.
			Interaction.DeleteSetting(sAppName, sSection, sKey)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete the registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRegSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: Encrypt
	'
	' Description: Encrypts string passed and returns the result.
	'
	' ***************************************************************** '
	Public Function Encrypt(ByRef sPassword As String, ByRef sEncryptedPassword As String) As Integer
		
		Dim result As Integer = 0
		Dim sAString As String = ""
		Dim sBString As New StringBuilder
		Dim iCntr As Integer
		Dim sChar1 As New FixedLengthString(1)
		Dim sChar2 As New FixedLengthString(1)
		Dim iSn As Integer
		Dim sCodeString As String = ""
		Dim iClen As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Encrypts the supplied string returning the encrypted
			' result. Encrypted string will always be 2 characters
			' longer than original (leave space!)
			'
			' Encrypted string contains only ASCII characters in
			' range 32-126
			
			sCodeString = "aPCXADneGgH7khIJpjKtBMzmQLrRcqSEsbUv6yuVFW9xYZ2T3fd4w5N8"
			iClen = sCodeString.Length
			
			sAString = sPassword
			iCntr = sAString.Length
			
			If iCntr < 1 Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				sEncryptedPassword = ""
				
				Return result
			End If
			
			sChar1.Value = sCodeString.Substring((Strings.Asc(sAString.Substring(0, 1)(0)) + iCntr) Mod iClen, 1)
			sChar2.Value = sCodeString.Substring(Strings.Asc(sAString.Substring(sAString.Length - 1)(0)) Mod iClen, 1)
			iSn = ((Strings.Asc(sChar1.Value(0)) + Strings.Asc(sChar2.Value(0))) Mod iClen) + 1
			sBString = New StringBuilder(sChar2.Value)
			
			For iCntr2 As Integer = 1 To iCntr
				sBString.Append(sCodeString.Substring((Strings.Asc(sAString.Substring(iCntr2 - 1, 1)(0)) + iSn + iCntr2) Mod iClen, 1))
			Next iCntr2
			
			sBString.Append(sChar1.Value)
			
			' Return the result.
			sEncryptedPassword = sBString.ToString().Trim()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			sEncryptedPassword = ""
			
			' Log Error.
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to encrypt the string", vApp:=ACApp, vClass:=ACClass, vMethod:="Encrypt", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetSystemName
	'
	' Description: Gets the system (computer) name.
	'
	' ***************************************************************** '
	Public Function GetSystemName(ByRef sSystemName As String) As Integer
		
		Dim result As Integer = 0
		Dim lResult As Integer
		Dim sBuffer As New FixedLengthString(255)
		Dim lBufferSize As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lBufferSize = 255
			
			' API Call to get computer name
			lResult = GetComputerName(sBuffer.Value, lBufferSize)
			
			' Check return code
			If lResult <> 1 Then
				' Return error
				sSystemName = ""
				result = gPMConstants.PMEReturnCode.PMFalse
			Else
				' Set System Name Parameter
				sSystemName = sBuffer.Value.Substring(0, Math.Min(sBuffer.Value.Length, lBufferSize))
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			result = gPMConstants.PMELogLevel.PMLogOnError
			sSystemName = ""
			
			' Log Error.
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get computer name", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: FindVarField
	'
	' Description: Finds the Position of a Variable Data Field within
	'              a Variable Data Block
	'
	' ***************************************************************** '
	Public Function FindVarField(ByRef sRecordName As String, ByRef sFieldName As String, ByRef vVarDataBlock( ,  ) As Object, ByRef lPosition As Integer) As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Check that Var Data Block is an Array
			If Not Information.IsArray(vVarDataBlock) Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Check that there is a record & field name to search for
			If (sFieldName.Trim() = "") Or (sRecordName.Trim() = "") Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Search for Field in Array using RecordName & FieldName
			' Convert both to Upper to avoid any case mistakes.
			For l_Row As Integer = vVarDataBlock.GetLowerBound(1) To vVarDataBlock.GetUpperBound(1)

				If (sRecordName.Trim() & sFieldName.Trim()).ToUpper() = (CStr(vVarDataBlock(gPMConstants.PMEVarDataArrayColPos.PMVarRecordName, l_Row)).Trim() & CStr(vVarDataBlock(gPMConstants.PMEVarDataArrayColPos.PMVarFieldName, l_Row)).Trim()).ToUpper() Then
					lPosition = l_Row
					Return result
				End If
			Next l_Row
			
			' Field Not Found so return error
			
			Return gPMConstants.PMEReturnCode.PMFalse
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			result = gPMConstants.PMELogLevel.PMLogOnError
			
			' Log Error.
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Find " & sFieldName & " in variable data block.", vApp:=ACApp, vClass:=ACClass, vMethod:="FindVarData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetCommandLine
	'
	' Description: Gets the command line passed and splits it down
	'              into the seporate arguments.
	'
	' ***************************************************************** '
    Public Function GetCommandLine(ByRef vArgArray() As Object, Optional ByRef vMaxArgs As Object = Nothing) As Integer

        'Declare variables.
        Dim result As Integer = 0
        Dim sChar As String = ""
        Dim sCmdLine As String = ""
        Dim iCmdLineLen As Integer
        Dim bInArg As Boolean
        Dim iMaxArgs, iNumArgs As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'See if MaxArgs was provided.

            Dim dbNumericTemp As Double

            If (Not Information.IsNothing(vMaxArgs)) And (Double.TryParse(CStr(vMaxArgs), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                iMaxArgs = CInt(vMaxArgs)
            Else
                iMaxArgs = 100
            End If

            'Make array of the correct size.
            ReDim vArgArray(iMaxArgs)

            ' Initialise
            iNumArgs = 0
            bInArg = False

            'Get command line arguments.
            sCmdLine = Interaction.Command()

            ' Get the length of the command line
            iCmdLineLen = sCmdLine.Length

            'Go thru command line one character at a time.
            For iSub As Integer = 1 To iCmdLineLen
                sChar = Mid(sCmdLine, iSub, 1)

                'Test for space or tab.
                If (sChar <> " ") And (sChar <> Strings.Chr(9)) Then
                    'Test if already in argument.
                    If Not bInArg Then
                        If iNumArgs >= iMaxArgs Then
                            Exit For
                        End If
                        iNumArgs += 1
                        bInArg = True
                    End If
                    'Concatenate character to current argument.
                    vArgArray(iNumArgs - 1) = CStr(vArgArray(iNumArgs - 1)) & sChar

                Else
                    bInArg = False

                End If

            Next iSub

            'Resize array just enough to hold arguments.
            If iNumArgs > 0 Then
                ReDim Preserve vArgArray(iNumArgs - 1)
            Else

                vArgArray = Nothing
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMELogLevel.PMLogOnError
            bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process the Command Line :- " & sCmdLine, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommandLine", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
	
	' ***************************************************************** '
	' Name: ConvertWildCardsForSQL (Public)
	'
	' Description: Converts '*' wildcards to '%' for SQL
	'
	' Edit History TF311097 Created
	' ***************************************************************** '
	Public Function ConvertWildCardsForSQL(ByRef r_sTextString As String) As Integer
		
		Dim result As Integer = 0
		Dim sSearchText As String = ""
		
		Try 
			
			sSearchText = r_sTextString
			
			' Replace * with % wildcards
			While (sSearchText.IndexOf("*"c) >= 0)
				Mid(sSearchText, sSearchText.IndexOf("*"c) + 1, 1) = "%"
			End While
			
			' Add implied wildcard to end
			If Not sSearchText.EndsWith("%") Then
				sSearchText = sSearchText & "%"
			End If
			
			r_sTextString = sSearchText
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			result = gPMConstants.PMELogLevel.PMLogOnError
			
			' Log Error.
			bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Convert Wild Cards For SQL", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertWildCardsForSQL", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Module
