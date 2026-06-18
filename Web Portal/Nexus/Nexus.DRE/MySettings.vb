Imports System.Configuration
Imports Microsoft.SqlServer
Imports System.Web
Imports System.IO
Namespace My
    Partial Friend NotInheritable Class MySettings
        Private DllSettings As ClientSettingsSection
        Private DllConfigDoesNotExist As Boolean

        Default Public Overrides Property Item(ByVal propertyName As String) As Object
            Get
                Dim oValue As Object = Nothing
                Try
                    'If the .dll.config file has already been loaded, use it to obtain the value...
                    If DllSettings IsNot Nothing Then
                        oValue = DllSettings.Settings.Get(propertyName).Value.ValueXml.InnerXml
                    ElseIf Not DllConfigDoesNotExist Then
                        If Me.LoadDllConfigFile() Then
                            oValue = DllSettings.Settings.Get(propertyName).Value.ValueXml.InnerXml
                        End If
                    End If
                Catch ex As Exception
                End Try
                Try
                    If oValue Is Nothing Then
                        oValue = MyBase.Item(propertyName)
                    End If
                Catch ex As Exception
                End Try
                Return oValue
            End Get
            Set(ByVal value As Object)
                MyBase.Item(propertyName) = value
            End Set
        End Property
        Private Function LoadDllConfigFile() As Boolean
            Dim bDllConfigLoaded As Boolean = False
            Dim cfgDll As System.Configuration.Configuration
            Dim cfmDllCfg As New ExeConfigurationFileMap()

            Dim sAssemblyPath As String = Reflection.Assembly.GetExecutingAssembly().Location()

            Dim strNamespace As String = GetType(MySettings).FullName


            cfmDllCfg.ExeConfigFilename = sAssemblyPath & ".config"
            Try
                If File.Exists(cfmDllCfg.ExeConfigFilename) Then
                    strNamespace = strNamespace.Substring(0, strNamespace.IndexOf("."c))
                    cfgDll = ConfigurationManager.OpenMappedExeConfiguration(cfmDllCfg, ConfigurationUserLevel.None)

                    Dim csgApplicationSettings As ConfigurationSectionGroup = cfgDll.GetSectionGroup("applicationSettings")
                    Me.DllSettings = DirectCast(csgApplicationSettings.Sections(strNamespace), ClientSettingsSection)
                    bDllConfigLoaded = True
                Else
                    cfmDllCfg.ExeConfigFilename = HttpContext.Current.Server.MapPath("~/Bin/Nexus.Dre.dll.config")
                    cfgDll = ConfigurationManager.OpenMappedExeConfiguration(cfmDllCfg, ConfigurationUserLevel.None)

                    Dim csgApplicationSettings As ConfigurationSectionGroup = cfgDll.GetSectionGroup("applicationSettings")
                    Me.DllSettings = DirectCast(csgApplicationSettings.Sections(strNamespace), ClientSettingsSection)
                    bDllConfigLoaded = True

                End If


            Catch ex As Exception

                'bestaat niet
                DllConfigDoesNotExist = True

            End Try
            Return bDllConfigLoaded

        End Function

    End Class

End Namespace
