Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Namespace UpgradeStubs
	<System.Runtime.InteropServices.ProgId("VBRUN_ScaleModeConstants_NET.VBRUN_ScaleModeConstants")> _
	Public NotInheritable Class VBRUN_ScaleModeConstants 
		Public Shared Function getvbPixels() As UpgradeStubs.VBRUN_ScaleModeConstantsEnum

			Return CType(VBRUN_ScaleModeConstantsEnum.vbPixels, UpgradeStubs.VBRUN_ScaleModeConstantsEnum)
		End Function
	End Class
	<System.Runtime.InteropServices.ProgId("VB_Global_NET.VB_Global")> _
	Public NotInheritable Class VB_Global 
		Public Shared Function getLicenses() As UpgradeStubs.VB_Licenses

			Return Nothing
		End Function
	End Class
	<System.Runtime.InteropServices.ProgId("VB_Licenses_NET.VB_Licenses")> _
	Public NotInheritable Class VB_Licenses 
		Public Function Add(ByVal ProgId As String, Optional ByVal LicenseKey As String = "") As String

			Return String.Empty
		End Function
	End Class
	<System.Runtime.InteropServices.ProgId("VBRUN_LicenseInfo_NET.VBRUN_LicenseInfo")> _
	Public NotInheritable Class VBRUN_LicenseInfo 
		Public Function getProgId() As String

			Return String.Empty
		End Function
		Public Function getLicenseKey() As String

			Return String.Empty
		End Function
	End Class
	<System.Runtime.InteropServices.ProgId("VB_Control_NET.VB_Control")> _
	Public NotInheritable Class VB_Control 
		Public Shared Function getAppearance(ByVal instance As Control) As Integer

			Return 0
		End Function
	End Class
	<System.Runtime.InteropServices.ProgId("VB_ComboBox_NET.VB_ComboBox")> _
	Public NotInheritable Class VB_ComboBox 
		Public Shared Sub setLocked(ByVal instance As ComboBox, ByVal Locked As Boolean)

		End Sub
		Public Shared Function getLocked(ByVal instance As ComboBox) As Boolean

			Return False
		End Function
	End Class
	<System.Runtime.InteropServices.ProgId("VB_UserControl_NET.VB_UserControl")> _
	Public NotInheritable Class VB_UserControl 
		Public Shared Function getAmbient(ByVal instance As UserControl) As UpgradeStubs.VBRUN_AmbientProperties

			Return Nothing
		End Function
		Public Shared Function getExtender(ByVal instance As UserControl) As Object

			Return Nothing
		End Function
		Public Shared Function getMouseIcon(ByVal instance As UserControl) As Image

			Return New Bitmap(1, 1)
		End Function
		Public Shared Sub setMouseIcon(ByVal instance As UserControl, ByVal MouseIcon As Image)

		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("VBRUN_AmbientProperties_NET.VBRUN_AmbientProperties")> _
	Public NotInheritable Class VBRUN_AmbientProperties 
		Public Function getFont() As Font

			Return Nothing
		End Function
	End Class
	<System.Runtime.InteropServices.ProgId("VBRUN_PropertyBag_NET.VBRUN_PropertyBag")> _
	Public NotInheritable Class VBRUN_PropertyBag 
		Public Function ReadProperty(ByVal Name As String, Optional ByVal DefaultValue As Object = Nothing) As Object

			Return Nothing
		End Function
		Public Sub WriteProperty(ByVal Name As String, ByVal Value As Object, Optional ByVal DefaultValue As Object = Nothing)

		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("VBA_VbFileAttribute_NET.VBA_VbFileAttribute")> _
	Public NotInheritable Class VBA_VbFileAttribute 
		Public Shared Function getvbVolume() As FileAttribute

			Return CType(VBA_VbFileAttributeEnum.vbVolume, FileAttribute)
		End Function
		Public Shared Function getvbAlias() As FileAttribute

			Return CType(VBA_VbFileAttributeEnum.vbAlias, FileAttribute)
		End Function
	End Class
	Public Enum VBRUN_ScaleModeConstantsEnum
		vbPixels = 3
	End Enum
	Public Enum VBA_VbFileAttributeEnum
		vbVolume = 8
		vbAlias = 64
	End Enum
End NameSpace
