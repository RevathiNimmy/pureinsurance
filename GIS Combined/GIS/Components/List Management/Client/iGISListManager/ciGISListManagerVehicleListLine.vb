Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Runtime.InteropServices
Imports System.Text
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class cVehicleListLine 
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Private Structure udtVehicleRecord
        <VBFixedString(1)> _
        Public LeftBracket As String
        <VBFixedString(4)> _
        Public YearStart As String
        <VBFixedString(1)> _
        Public YearDivider As String
        <VBFixedString(4)> _
        Public YearEnd As String
        <VBFixedString(1)> _
        Public Divider1 As String
        <VBFixedString(4)> _
         Public Make As String
        <VBFixedString(1)> _
        Public Divider2 As String
        <VBFixedString(1)> _
        Public Doors As String
        <VBFixedString(1)> _
        Public StyleType As String
        <VBFixedString(1)> _
        Public Divider3 As String
        <VBFixedString(1)> _
        Public FuelType As String
        <VBFixedString(1)> _
        Public Transmission As String
        <VBFixedString(1)> _
        Public Divider4 As String
        <VBFixedString(4)> _
        Public EngineSize As String
        <VBFixedString(1)> _
        Public RightBracket As String
        <VBFixedString(1)> _
        Public Divider5 As String
        <VBFixedString(41)> _
        Public ModelName As String
        <VBFixedString(1)> _
        Public Divider6 As String
        <VBFixedString(20)> _
        Public Identifier As String
		Public Shared Function CreateInstance() As udtVehicleRecord
			Dim result As New udtVehicleRecord
            Return result
		End Function
	End Structure
	
	Private Const ACClass As String = "cVehicleListLine"
	

	Private m_udtVehicleLine As udtVehicleRecord = udtVehicleRecord.CreateInstance()
	
	Private Declare Sub CopyMemory Lib "kernel32"  Alias "RtlMoveMemory"(ByVal hpvDest As Integer, ByVal hpvSource As Integer, ByVal cbCopy As Integer)
	
	
	
	Public ReadOnly Property Style() As String
		Get
			
			Dim lNoDoors As Integer
			Dim sType As String = ""
			
			Const acTxtVan As String = "Van"
			Const acTxtSaloon As String = "Saloon"
			Const acTxtHatch As String = "Hatch"
			Const acTxtEstate As String = "Est."
			Const acTxtConvertable As String = "Conv."
			Const acTxtDoorsSuffix As String = "dr"
			
			Const acInitialEstate As String = "E"
			Const acInitialConvertable As String = "C"
			Const acInitialHatchback As String = "H"
			
            Dim sStyle As String = m_udtVehicleLine.Doors.Trim() & m_udtVehicleLine.StyleType.Trim()

            If sStyle.Length = 0 Then
                sStyle = acTxtVan
            Else
                lNoDoors = CInt(Conversion.Val(sStyle.Substring(0, 1)))
                If sStyle.Length > 1 Then
                    sType = sStyle.Substring(sStyle.Length - 1)
                End If

                If sType.Length <> 0 Then
                    Select Case sType
                        Case acInitialEstate : sType = acTxtEstate
                        Case acInitialConvertable : sType = acTxtConvertable
                        Case acInitialHatchback : sType = acTxtHatch
                    End Select
                Else
                    If lNoDoors Mod 2 = 0 Then
                        sType = acTxtSaloon
                    Else
                        sType = acTxtHatch
                    End If
                End If

                '--- Make output string
                sStyle = sType & " " & CStr(lNoDoors) & acTxtDoorsSuffix
            End If

            Return sStyle

        End Get
    End Property

    Public ReadOnly Property StyleCode() As String
        Get


            Const acVanStyleCode As String = "0"

            Dim sStyleCode As String = (m_udtVehicleLine.Doors.Trim() & m_udtVehicleLine.StyleType.Trim()).ToUpper()

            If sStyleCode.Length = 0 Then
                sStyleCode = acVanStyleCode
            End If

            Return sStyleCode

        End Get
    End Property

    Public ReadOnly Property Capacity() As String
        Get


            Dim sEngineType As String = GetEngineTypeDescription(m_udtVehicleLine.FuelType.Trim())

            Return m_udtVehicleLine.EngineSize.Trim() & " " & sEngineType

        End Get
    End Property

    Public ReadOnly Property CapacityCode() As String
        Get

            Return m_udtVehicleLine.EngineSize.Trim() & m_udtVehicleLine.FuelType.Trim()

        End Get
    End Property

    Public ReadOnly Property CC() As Integer
        Get
            Return CInt(Conversion.Val(m_udtVehicleLine.EngineSize.Trim()))
        End Get
    End Property

    Public ReadOnly Property TrimName(ByVal v_sModel As String) As String
        Get


            Const acTxtDefaultTrim As String = "(Base Model)"

            Dim sTrimName As String = m_udtVehicleLine.ModelName.Trim()

            'Remove the model name
            sTrimName = sTrimName.Replace(v_sModel, "").Trim()

            If sTrimName.Length = 0 Then
                sTrimName = acTxtDefaultTrim
            End If

            Return sTrimName

        End Get
    End Property

    Public ReadOnly Property TransmissionType() As String
        Get

            Dim result As String = String.Empty
            Select Case m_udtVehicleLine.Transmission.Trim()
                Case "A" : result = "Automatic"
                Case "M" : result = "Manual"
            End Select

            Return result
        End Get
    End Property

    Public ReadOnly Property TransmissionTypeCode() As String
        Get

            Return m_udtVehicleLine.Transmission.Trim()

        End Get
    End Property

    Public ReadOnly Property DisplayLine() As String
        Get


            Const sSpc As String = "  "
            Const acTxtDoor As String = "%door% Door"
            Const acTxtParamDoor As String = "%door%"

            Dim sModel As String = m_udtVehicleLine.ModelName.Trim()

            Dim sDoors As String = m_udtVehicleLine.Doors.Trim()

            If sDoors.Length <> 0 Then
                sDoors = acTxtDoor.Replace(acTxtParamDoor, sDoors)
            End If

            'Replace the hyphen with to
            Dim sYearRange As String = m_udtVehicleLine.YearStart & " to " & m_udtVehicleLine.YearEnd

            'Is the Starting year equal to 0000?
            If Conversion.Val(m_udtVehicleLine.YearStart) = 0 Then
                sYearRange = "No Year Range"
            Else
                'Is the vehicle a current model...if so replace the 0000 with to present
                Select Case m_udtVehicleLine.YearEnd
                    Case "0000"
                        sYearRange = m_udtVehicleLine.YearStart & " to present"
                End Select
            End If

            Dim sEngineType As String = GetEngineTypeDescription(m_udtVehicleLine.FuelType.Trim())

            Dim sGearbox As String = m_udtVehicleLine.Transmission.Trim()
            Select Case sGearbox
                Case "M" : sGearbox = "Manual"
                Case "A" : sGearbox = "Automatic"
                Case Else : sGearbox = ""
            End Select

            Dim sBody As String = m_udtVehicleLine.StyleType.Trim()
            Select Case sBody
                Case "C"
                    sBody = sSpc & "Convertible"
                Case "H"
                    sBody = sSpc & "Hatchback"
                Case "E"
                    sBody = sSpc & "Estate"
                Case Else
                    sBody = ""
            End Select

            Dim sEngineCC As String = m_udtVehicleLine.EngineSize.Trim()
            sEngineCC = Strings.FormatNumber(sEngineCC, 0, False, False, False)

            Dim sItem As String = sModel & sSpc & "-" & sSpc & sDoors & sSpc & sEngineType & sBody & sSpc & sGearbox & sSpc & sEngineCC & "cc" & sSpc & " - " & sSpc & sYearRange & "#" & m_udtVehicleLine.Identifier.Substring(0, 10).Trim()

            Return sItem

        End Get
    End Property

    Public ReadOnly Property Doors() As Integer
        Get
            Return CInt(Conversion.Val(m_udtVehicleLine.Doors))
        End Get
    End Property

    Public ReadOnly Property FuelType() As String
        Get
            Return m_udtVehicleLine.FuelType.Trim().ToUpper()
        End Get
    End Property

    Public ReadOnly Property ModelName() As String
        Get

            Dim iPos As Integer

            Dim sMake As String = m_udtVehicleLine.Make.Trim()
            Dim sModel As String = m_udtVehicleLine.ModelName.Trim()

            If sMake.ToUpper() <> sModel.ToUpper() Then
                iPos = (sModel.IndexOf(" "c) + 1)
                If iPos <> 0 Then
                    sModel = sModel.Substring(0, Math.Min(sModel.Length, iPos - 1)).Trim().ToUpper()
                End If
            End If

            Return sModel

        End Get
    End Property
	
	Public Function Load(ByVal v_strVehicleListLine As String) As Integer
		
		Dim result As Integer = 0
		Try 

            'Modified by Archana Tokas on 4/27/2010 10:30:09 AM changes to be checked at run time
            'Dim handle As GCHandle = GCHandle.Alloc(VarPtr(m_udtVehicleLine), GCHandleType.Pinned)
            'Dim handle2 As GCHandle = GCHandle.Alloc(StrPtr(v_strVehicleListLine), GCHandleType.Pinned)

            Dim handle As GCHandle = GCHandle.Alloc(m_udtVehicleLine, GCHandleType.Pinned)
            Dim handle2 As GCHandle = GCHandle.Alloc(v_strVehicleListLine, GCHandleType.Pinned)

			Try 

				Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()

				Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()

				CopyMemory(tmpPtr, tmpPtr2, Encoding.Unicode.GetByteCount(v_strVehicleListLine))
			Finally 
				handle.Free()
				handle2.Free()
			End Try
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Load Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Public Function IsInFilter(ByVal v_vStyleCode As String, Optional ByRef v_vCC As Integer = 0, Optional ByRef v_vFuelType As String = "", Optional ByRef v_vTrimName As String = "", Optional ByRef v_sModel As String = "", Optional ByRef v_vTransmissionTypeCode As String = "") As Boolean
		
		Dim result As Boolean = False
		Try 
			
			Dim oCapacityRange As iRange

			If v_vStyleCode.ToUpper() <> Me.StyleCode Then
				Return result
			End If
			
			If v_vCC <> 0 Then
				oCapacityRange = New cCapacityRange()
				With oCapacityRange
					.StartValue = v_vCC
					.EndValue = v_vCC
					If Not .Includes(Me.CC) Then
						Return result
					End If
				End With
				oCapacityRange = Nothing
			End If
			
			If v_vFuelType.Length <> 0 Then
				If v_vFuelType.ToUpper() <> Me.FuelType Then
					Return result
				End If
			End If
			
			If v_vTrimName.Length <> 0 Then
				If v_vTrimName.ToUpper() <> Me.TrimName(v_sModel).ToUpper() Then
					Return result
				End If
			End If
			
			If v_vTransmissionTypeCode.Length <> 0 Then
				If v_vTransmissionTypeCode.ToUpper() <> Me.TransmissionTypeCode Then
					Return result
				End If
			End If
			
			
			Return True
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsInFilter Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsInFilter", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Function GetEngineTypeDescription(ByVal v_sEngineType As String) As String
		
		Dim result As String = String.Empty
		 
			
			Select Case v_sEngineType
				Case "D" : v_sEngineType = "Diesel"
				Case "P" : v_sEngineType = "Petrol"
				Case Else : v_sEngineType = ""
			End Select
			
			
			Return v_sEngineType
		
	End Function
End Class

