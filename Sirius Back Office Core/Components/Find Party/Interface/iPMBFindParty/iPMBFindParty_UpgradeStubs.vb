Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Namespace UpgradeStubs
	<System.Runtime.InteropServices.ProgId("VBA_VbFileAttribute_NET.VBA_VbFileAttribute")> _
	Public NotInheritable Class VBA_VbFileAttribute 
		Public Shared Function getvbVolume() As FileAttribute

			Return CType(VBA_VbFileAttributeEnum.vbVolume, FileAttribute)
		End Function
		Public Shared Function getvbAlias() As FileAttribute

			Return CType(VBA_VbFileAttributeEnum.vbAlias, FileAttribute)
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
	<System.Runtime.InteropServices.ProgId("MSComctlLib_IListItem_NET.MSComctlLib_IListItem")> _
	Public NotInheritable Class MSComctlLib_IListItem 
		Public Shared Sub setGhosted(ByVal instance As ListViewItem, ByVal Ghosted As Boolean)

		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("VBRUN_QueryUnloadConstants_NET.VBRUN_QueryUnloadConstants")> _
	Public NotInheritable Class VBRUN_QueryUnloadConstants 
		Public Shared Function getvbFormControlMenu() As UpgradeStubs.VBRUN_QueryUnloadConstantsEnum

			Return CType(VBRUN_QueryUnloadConstantsEnum.vbFormControlMenu, UpgradeStubs.VBRUN_QueryUnloadConstantsEnum)
		End Function
		Public Shared Function getvbFormCode() As UpgradeStubs.VBRUN_QueryUnloadConstantsEnum

			Return CType(VBRUN_QueryUnloadConstantsEnum.vbFormCode, UpgradeStubs.VBRUN_QueryUnloadConstantsEnum)
		End Function
	End Class
	<System.Runtime.InteropServices.ProgId("stdole_Font_NET.stdole_Font")> _
	Public NotInheritable Class stdole_Font 
		Public Shared Sub setWeight(ByVal instance As Font, ByVal Weight As Integer)

		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("VB_PictureBox_NET.VB_PictureBox")> _
	Public NotInheritable Class VB_PictureBox 
		Public Shared Sub setScaleMode(ByVal instance As PictureBox, ByVal ScaleMode As UpgradeStubs.VBRUN_ScaleModeConstantsEnum)

		End Sub
		Public Shared Function TextHeight(ByVal instance As PictureBox, ByVal Str As String) As Single

			Return 0
		End Function
		Public Shared Sub Line(ByVal instance As PictureBox, ByVal X2 As Single, ByVal Y2 As Single, Optional ByVal X1 As Single = 0, Optional ByVal Y1 As Single = 0, Optional ByVal Color As Integer = 0, Optional ByVal DrawModifier As Object = Nothing)

		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("VBRUN_ScaleModeConstants_NET.VBRUN_ScaleModeConstants")> _
	Public NotInheritable Class VBRUN_ScaleModeConstants 
		Public Shared Function getvbTwips() As UpgradeStubs.VBRUN_ScaleModeConstantsEnum

			Return CType(VBRUN_ScaleModeConstantsEnum.vbTwips, UpgradeStubs.VBRUN_ScaleModeConstantsEnum)
		End Function
		Public Shared Function getvbPixels() As UpgradeStubs.VBRUN_ScaleModeConstantsEnum

			Return CType(VBRUN_ScaleModeConstantsEnum.vbPixels, UpgradeStubs.VBRUN_ScaleModeConstantsEnum)
		End Function
	End Class
	<System.Runtime.InteropServices.ProgId("VB_Control_NET.VB_Control")> _
	Public NotInheritable Class VB_Control 
		Public Shared Sub setAutoRedraw(ByVal instance As Control, ByVal AutoRedraw As Boolean)

		End Sub
		Public Shared Function getImage(ByVal instance As Control) As Image

			Return New Bitmap(1, 1)
		End Function
	End Class
	Public Enum VBA_VbFileAttributeEnum
		vbVolume = 8
		vbAlias = 64
	End Enum
	Public Enum VBRUN_QueryUnloadConstantsEnum
		vbFormControlMenu = 0
		vbFormCode = 1
	End Enum
	Public Enum VBRUN_ScaleModeConstantsEnum
		vbTwips = 1
		vbPixels = 3
	End Enum
End NameSpace
