Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Namespace UpgradeStubs
	<System.Runtime.InteropServices.ProgId("RichTextLib_RichTextBox_NET.RichTextLib_RichTextBox")> _
	Public NotInheritable Class RichTextLib_RichTextBox 
		Public Shared Function getFileName(ByVal instance As RichTextBox) As String

			Return String.Empty
		End Function
	End Class
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
