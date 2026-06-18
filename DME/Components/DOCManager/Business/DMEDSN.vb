Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles

Friend NotInheritable Class DmeDSN 
	 ' ********************************************************************* '
	 '
	 ' Class : DmeDSN
	 '
	 ' Description : Class for handling DSNs
	
	 ' Amendments:
	 ' ===========
	 ' David Kyle    Get the Driver for the DSN.
	 ' 07/05/1999    Get the ODBC DSN type.
	 '
	 ' David Kyle    Allow for a variable number of key values on the DSN.
	 ' 14/05/1999
	 '
	 ' ********************************************************************* '
	
	 'Class
	Private Const ACClass As String = "DmeDSN"
	
	 'Maximum DSN Key entries
	Private Const ACMAXDSNKey As Integer = 9

	'Root Registry directories for DSN
	'Private Const ACDSNGroup As Integer = HKEY_LOCAL_MACHINE
	Private Const ACDSNGroup As Integer = gpmConstants.HKEY_LOCAL_MACHINE
	Private Const ACDSNRoot As String = "SOFTWARE\ODBC\odbc.ini"
	Private Const ACDSNODBC As String = "SOFTWARE\ODBC\odbc.ini\ODBC Data Sources"
	Private Const ACDSNODBCINST As String = "SOFTWARE\ODBC\ODBCINST.INI"
	
	 'The DSN Name
	Private m_sDSN As String = ""
	
	 'DSN Keys
	Private m_vDSNKeys( ,  ) As Object
	
	 'DK19990514: Number of DSN Key Values
	Private m_lDSNValueCount As Integer
	
	 'Does this DSN exist
	Private m_bExists As Boolean
	
	 'SQL Server ODBC Driver
	Private m_sDSNODBCDriver As String = ""
	
	 'DSN Type
	Private m_sDSNType As String = ""
	
	Public ReadOnly Property DSN() As String
		Get
			
			Return m_sDSN
			
		End Get
	End Property
	
	
	Public Property Exists() As Boolean
		Get
			
			Return m_bExists
			
		End Get
		Set(ByVal Value As Boolean)
			
			m_bExists = Value
			
		End Set
	End Property
	
	Public Function GetDSNDriver() As Integer
		 'DK19990507: This function looks at the DSN on the registry
		 '            and extracts the driver
		
		Dim result As Integer = 0
		Dim lPtr As Integer
		Dim vRes As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			 '    Get Driver from the Registry


			Do 


				vRes = ReadRegistryGetAll(ACDSNGroup, ACDSNODBCINST & "\" & m_sDSNType, lPtr)
				lPtr += 1
			Loop Until (CStr(vRes(1)).ToUpper() = ("Driver").ToUpper() Or CStr(vRes(2)) = "Not Found")
			

			If CStr(vRes(1)).ToUpper() = ("Driver").ToUpper() Then
				

				m_sDSNODBCDriver = CStr(vRes(2))
				result = gPMConstants.PMEReturnCode.PMTrue
				
			End If
			
			Return result
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	Public Function GetDSNType(ByRef sDSNType As String) As Integer
		 'DK19990507: This function looks at the ODBC Data Sources
		 '            on the registry and extracts the ODBC driver
		 '            type.
		
		Dim result As Integer = 0
		Dim lPtr As Integer
		Dim vRes As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			m_sDSNType = ""
			
			 'Get DSN ODBC type from the Registry


			Do 


				vRes = ReadRegistryGetAll(ACDSNGroup, ACDSNODBC, lPtr)
				lPtr += 1
			Loop Until (CStr(vRes(1)).ToUpper() = m_sDSN.ToUpper() Or CStr(vRes(2)) = "Not Found")
			

			If CStr(vRes(1)).ToUpper() = m_sDSN.ToUpper() Then

				m_sDSNType = CStr(vRes(2))
				result = gPMConstants.PMEReturnCode.PMTrue
			End If
			
			sDSNType = m_sDSNType
			
			Return result
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	Public Function GetDSN() As Integer
		
		Dim result As Integer = 0
		Dim lPtr As Integer
		Dim vRes As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			 'Make sure we have a DSN Name
			If m_sDSN = "" Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			 'Redimension the array of key
			 ' DK19990514: Allow for variable number of key values on DSN
			 '    ReDim m_vDSNKeys(0 To 1, 0 To ACMAXDSNKey) As String

			ReDim m_vDSNKeys(1, 0)
			
			 'Does not exist
			m_bExists = False
			
			 'Get first 9 Registry keys for selected DSN
			 '    For lPtr = 0 To ACMAXDSNKey
			
			 '        vRes = ReadRegistryGetAll(ACDSNGroup, ACDSNRoot & "\" & m_sDSN, lPtr)
			
			 '        If (vRes(2) <> "Not Found") Then
			 '            m_vDSNKeys(0, lPtr) = vRes(1)
			 '            m_vDSNKeys(1, lPtr) = vRes(2)
			 '            m_bExists = True
			 '        Else
			 '            Exit For
			 '        End If
			
			 '    Next lPtr
			
			lPtr = 0


			vRes = ReadRegistryGetAll(ACDSNGroup, ACDSNRoot & "\" & m_sDSN, lPtr)

			Do Until CStr(vRes(2)) = "Not Found"
				m_bExists = True

				ReDim Preserve m_vDSNKeys(1, lPtr)

				m_vDSNKeys(0, lPtr) = vRes(1)

				m_vDSNKeys(1, lPtr) = vRes(2)
				lPtr += 1


				vRes = ReadRegistryGetAll(ACDSNGroup, ACDSNRoot & "\" & m_sDSN, lPtr)
			Loop 
			
			If m_bExists Then
				m_lDSNValueCount = lPtr - 1
			End If
			
			Return result
		
		Catch 
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
	End Function
	
	 ' ********************************************************************* '
	 '
	 ' Name : CreateDSN ( Public )
	 '
	 ' Description : Creates a DSN
	 '
	 ' ********************************************************************* '
	Public Function CreateDSN(ByRef sDatabase As String, Optional ByRef vDSN As String = "") As Integer
		
		Dim result As Integer = 0
		Dim lPtr As Integer
		Dim sName, sData As String
		
		Try 
			result = gPMConstants.PMEReturnCode.PMTrue
			

			ReDim m_vDSNKeys(1, ACMAXDSNKey)
			
			m_bExists = False
			
			 'Ensure we have a SQL Server ODBC Driver
			m_sDSNType = "SQL Server"
			If GetDSNDriver() <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			 'Check if we have a DSN name

			If Not Information.IsNothing(vDSN) Then
				m_sDSN = vDSN
			End If
			
			 'Make sure we have a DSN Name
			If m_sDSN = "" Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			 'Set the Defaults
			
			m_vDSNKeys(0, 0) = "AnsiNPW"
			m_vDSNKeys(1, 0) = "Yes"
			
			m_vDSNKeys(0, 1) = "Database"
			m_vDSNKeys(1, 1) = sDatabase
			
			m_vDSNKeys(0, 2) = "Description"
			m_vDSNKeys(1, 2) = ""
			
			m_vDSNKeys(0, 3) = "Driver"
			m_vDSNKeys(1, 3) = m_sDSNODBCDriver
			
			m_vDSNKeys(0, 4) = "LastUser"
			m_vDSNKeys(1, 4) = ""
			
			m_vDSNKeys(0, 5) = "OemToAnsi"
			m_vDSNKeys(1, 5) = "No"
			
			m_vDSNKeys(0, 6) = "QoutedId"
			m_vDSNKeys(1, 6) = "Yes"
			
			m_vDSNKeys(0, 7) = "Server"
			m_vDSNKeys(1, 7) = "(local)"
			
			m_vDSNKeys(0, 8) = "Trusted_Connection"
			m_vDSNKeys(1, 8) = ""
			
			m_vDSNKeys(0, 9) = "UseProcForPrepare"
			m_vDSNKeys(1, 9) = "No"
			
			 'Create the DSN entry in the registry
			For lPtr = 0 To ACMAXDSNKey
				
				 'Get the name and data
				sName = CStr(m_vDSNKeys(0, lPtr))
				sData = CStr(m_vDSNKeys(1, lPtr))
				
				 'Write the registry value
				WriteRegistry(ACDSNGroup, ACDSNRoot & "\" & m_sDSN, CStr(m_vDSNKeys(0, lPtr)), ADVReg.InTypes.ValString, CStr(m_vDSNKeys(1, lPtr)))
				
			Next lPtr
			
			 'Write the DSN entry in the ODBC Data Sources Section
			WriteRegistry(ACDSNGroup, ACDSNODBC, m_sDSN, ADVReg.InTypes.ValString, "SQL Server")
			
			m_bExists = True
			
			m_lDSNValueCount = lPtr
			
			Return result
		
		Catch 
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	
	 ' ********************************************************************* '
	 '
	 ' Name : KeyValue ( Public )
	 '
	 ' Description : Return the KeyValue
	 '
	 ' ********************************************************************* '
	Public Function KeyValue(ByVal sName As String) As String
		
		Dim result As String = String.Empty
		
		Try 
			
			result = "MISSING"
			
			 'DK19990514: Allow for variable number of DSN key values
			 '    For lPtr = 0 To ACMAXDSNKey
			For lPtr As Integer = 0 To m_lDSNValueCount
				
				 'Check for match
				If CStr(m_vDSNKeys(0, lPtr)).ToUpper() = sName.ToUpper() Then
					result = CStr(m_vDSNKeys(1, lPtr))
					Exit For
				End If
				
			Next lPtr
			
			Return result
		
		Catch 
			
			
			
			Return CStr(gPMConstants.PMEReturnCode.PMError)
		End Try
		
	End Function
	
	Public Function Initialise(ByRef sDSN As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			 'Make sure we have a DSN
			If sDSN = "" Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_sDSN = sDSN
			
			 ' Get the DSN if it exists
			
			Return GetDSN()
		
		Catch 
			
			
			
			
			Return result
		End Try
	End Function
	
	Public Function RemoveDSN() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If m_sDSN = "" Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			 'Remove all DSN Keys in section
			DeleteSubkey(ACDSNGroup, ACDSNRoot & "\" & m_sDSN)
			
			 'Remove the current DSN setting from ODBC Sources
			DeleteValue(ACDSNGroup, ACDSNODBC, m_sDSN)
			
			m_bExists = False
			
			Return result
		
		Catch 
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
End Class