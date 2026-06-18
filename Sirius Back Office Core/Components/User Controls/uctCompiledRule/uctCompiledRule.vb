Imports System.Reflection
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles

Public Class uctCompiledRule

    Dim lstDlls As New List(Of String)
    Dim m_sRulePath As String = ""
    'AutoComplete collection that will help to filter keep the records.
    Dim MySource As New AutoCompleteStringCollection()
    Dim bLoaded As Boolean = False

#Region "Public Properties"
    ''' <summary>
    ''' First method to be called.Will load the list of assemblies as the source for Intellisence
    ''' </summary>
    ''' <remarks></remarks>
    Public Shadows Sub Load()
        GetListOfPureAssemblies()
    End Sub

    ''' <summary>
    ''' Set of get the text of user control
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Property Text As String
        Get
            Return txtCompiledDll.Text
        End Get
        Set(value As String)
            txtCompiledDll.Text = value
        End Set
    End Property

    Public Property bEnterOnlyAssemblyName As Boolean
#End Region

#Region "Private Methods"

    Private Sub UserControl1_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        txtCompiledDll.Height = MyBase.Height
        txtCompiledDll.Width = MyBase.Width
    End Sub

    ''' <summary>
    ''' Get the List of all Assemblies from the Pure\Application folder.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetListOfPureAssemblies()

        Dim sListOfFiles() As String = New String() {}

        GetRulePath(m_sRulePath)

        'Get List of all dlls from Pure\Application
        If Directory.Exists(m_sRulePath) Then
            sListOfFiles = Directory.GetFiles(m_sRulePath, "*.dll")
        End If

        For Each sFilePath As String In sListOfFiles
            lstDlls.Add(Path.GetFileName(sFilePath).Replace(".dll", ""))
        Next

        'Records binded to the AutocompleteStringCollection.
        MySource.AddRange(lstDlls.ToArray)

        'this AutocompleteStringcollection binded to the textbox as custom source.
        txtCompiledDll.AutoCompleteCustomSource = MySource

        'Auto complete mode set to suggest append so that it will sugesst one
        'or more suggested completion strings it has bith ‘Suggest’ and
        '‘Append’ functionality
        txtCompiledDll.AutoCompleteMode = AutoCompleteMode.SuggestAppend

        'Set to Custom source we have filled already
        txtCompiledDll.AutoCompleteSource = AutoCompleteSource.CustomSource

    End Sub
    ''' <summary>
    ''' Get the Public Classes from the Assembly
    ''' </summary>
    ''' <param name="sAssemblyName"></param>
    ''' <remarks></remarks>
    Private Function GetTypes(sAssemblyName As String) As Integer
        Dim slibraryPath As String
        Dim DLLAssembly As [Assembly]
        Dim nNumberOfTypes As Integer = 0
        slibraryPath = Path.Combine(m_sRulePath, sAssemblyName.Replace(".", "") & ".dll")

        Dim oTypes() As System.Type
        If IO.File.Exists(slibraryPath) Then
            DLLAssembly = [Assembly].LoadFrom(slibraryPath)
            '   Return DLLAssembly.CreateInstance(ClassName, True)

            oTypes = DLLAssembly.GetExportedTypes()
            nNumberOfTypes = oTypes.Count
            For Each oType As Type In oTypes
                lstDlls.Add(oType.FullName)
            Next

            Dim iACM As Windows.Forms.AutoCompleteMode
            iACM = txtCompiledDll.AutoCompleteMode
            MySource.AddRange(lstDlls.ToArray)

        End If
        Return nNumberOfTypes
    End Function
    ''' <summary>
    ''' Hanldes the ".". when Pressed get the List of all the Public classes from the dll
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtCompiledDll_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCompiledDll.KeyDown
        Dim nNumberOfTypes As Integer = 0
        If Not bLoaded Then
            If e.KeyCode = Keys.OemPeriod Then
                If Not bEnterOnlyAssemblyName Then
                    nNumberOfTypes = GetTypes(txtCompiledDll.Text)
                    txtCompiledDll.Select(txtCompiledDll.Text.Length, 0)
                    bLoaded = True
                    SendKeys.Send(".")
                End If
            End If
        End If

        If e.KeyCode = Keys.Back Then
            bLoaded = False
        End If
    End Sub


    ''' <summary>
    ''' Get path for rule files
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetRulePath(ByRef sRulePath As String)
        Dim result As Integer = 0
        Dim sSubKey As String = ""
        sSubKey = "GIS"

        result = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", r_sSettingValue:=sRulePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)
        If result <> 1 Then
            Exit Sub
        End If

        If sRulePath <> "" Then
            If Not sRulePath.EndsWith("\") Then
                sRulePath = sRulePath & "\"
            End If
        End If
    End Sub

#End Region

End Class
