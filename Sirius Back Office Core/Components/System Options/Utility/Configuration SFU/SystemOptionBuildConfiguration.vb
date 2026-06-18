Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Module MainModule

    '*************************************
    '*************************************
    '*************************************
    ' UPDATE THIS NUMBER WHEN ADDING A NEW OPTION
    ' THIS IS THE LAST NUMBER USED
    ' NEW OPTIONS SHOULD BE THIS NUMBER + 1
    Private Const MAXOPTIONNUMBER As Integer = 5265
    '*************************************
    '*************************************
    '*************************************

    Private Const ACComment As String = "-- Regenerate all system option configuration records to ensure" & Strings.Chr(13) & Strings.Chr(10) &
                                        "-- most up-to-date versions are always present after an upgrade" & Strings.Chr(13) & Strings.Chr(10) &
                                        "-- Product:      Pure Insurance" & Strings.Chr(13) & Strings.Chr(10) &
                                        "-- Last updated: "

    Private Const ACDefaultFileName As String = "SysOptConfigS4I.sql"

    Private Const ACDeleteGroups As String = Strings.Chr(9) & "DELETE FROM system_option_configuration_group"
    Private Const ACDeleteOptions As String = Strings.Chr(9) & "DELETE FROM system_option_configuration"

    Private Const ACFillOptions As String = Strings.Chr(9) & "INSERT INTO system_options (branch_id, option_number, value, description)" & Strings.Chr(13) & Strings.Chr(10) &
                                            Strings.Chr(9) & "SELECT DISTINCT s.source_id, soc.option_number, null, Max(soc.control_caption)" & Strings.Chr(13) & Strings.Chr(10) &
                                            Strings.Chr(9) & Strings.Chr(9) & "FROM system_option_configuration soc, source s" & Strings.Chr(13) & Strings.Chr(10) &
                                            Strings.Chr(9) & Strings.Chr(9) & "WHERE soc.option_number Is Not Null" & Strings.Chr(13) & Strings.Chr(10) &
                                            Strings.Chr(9) & Strings.Chr(9) & "AND NOT EXISTS (SELECT * FROM system_options so WHERE so.branch_id = s.source_id AND so.option_number = soc.option_number)" & Strings.Chr(13) & Strings.Chr(10) &
                                            Strings.Chr(9) & Strings.Chr(9) & "GROUP BY s.source_id, soc.option_number" & Strings.Chr(13) & Strings.Chr(10) &
                                            Strings.Chr(9) & Strings.Chr(9) & "ORDER BY source_id, option_number"

    Private Const ACUpdateOptions As String = Strings.Chr(9) & "UPDATE so SET DESCRIPTION=soc.control_caption " & Strings.Chr(13) & Strings.Chr(10) &
                                            Strings.Chr(9) & "FROM System_Options so INNER JOIN system_option_configuration soc " & Strings.Chr(13) & Strings.Chr(10) &
                                            Strings.Chr(9) & Strings.Chr(9) & "ON soc.option_number=so.option_number AND control_type in ('TextBox','combobox','CheckBox','uctCompiledRule') " & Strings.Chr(13) & Strings.Chr(10) &
                                            Strings.Chr(9) & Strings.Chr(9) & "AND control_caption IS NOT NULL"

    Private Const ACInsertGroup As String = Strings.Chr(9) &
                                            "INSERT INTO system_option_configuration_group (system_option_configuration_group_id, " &
                                            "name, parent_id, display_order) VALUES ("

    Private Const ACInsertOption As String = Strings.Chr(9) &
                                             "INSERT INTO system_option_configuration (option_number, system_option_configuration_group_id, " &
                                             "control_type, control_top, control_height, control_left, control_width, control_caption, " &
                                             "command, mandatory_or_optional, tab_index, command_parameters, parent_name, control_name) VALUES ("

    Private Const ACWrapperStart As String = "IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')" & Strings.Chr(13) & Strings.Chr(10) &
                                             "BEGIN"
    Private Const ACWrapperEnd As String = "END" & Strings.Chr(13) & Strings.Chr(10) & "GO"

    'developer guide no. 107
    <ThreadStatic()>
    Public oFS As Object
    Private oStream As FileStream
    Private oStreamWriter As StreamWriter

    Public Sub Main()

        Dim sFilename As String = ""

        ' Get ommand line as filename
        sFilename = Interaction.Command()

        ' Create filesystem object
        oFS = New Object()

        ' Check for valid path
        If Directory.Exists(Path.GetDirectoryName(sFilename)) Then
            Try
                oStream = New FileStream(sFilename, FileMode.CreateNew)

            Catch

            End Try


        End If

        ' Check for succesful file
        If oStream Is Nothing Then
            ' Default filename
            sFilename = Path.Combine(My.Application.Info.DirectoryPath, ACDefaultFileName)
            If File.Exists(sFilename) Then
                File.Delete(sFilename)
            End If
            oStream = New FileStream(sFilename, FileMode.CreateNew)
        End If

        ' Add comment
        oStreamWriter = New StreamWriter(oStream)
        oStreamWriter.WriteLine(ACComment & DateTimeHelper.ToString(DateTime.Now))

        ' Add wrapper start
        oStreamWriter.WriteLine(ACWrapperStart)

        ' Delete all current values
        oStreamWriter.WriteLine(Strings.Chr(9) & "-- Delete all current options and groups")
        oStreamWriter.WriteLine(ACDeleteOptions)
        oStreamWriter.WriteLine(ACDeleteGroups)

        ProcessGroups()

        ' Write code to ensure full population of configured options
        oStreamWriter.WriteLine("")
        oStreamWriter.WriteLine(Strings.Chr(9) & "-- Ensure all configured options at least have a default record")
        oStreamWriter.WriteLine(ACFillOptions)

        oStreamWriter.WriteLine("")
        oStreamWriter.WriteLine(Strings.Chr(9) & "-- Update system options description")
        oStreamWriter.WriteLine(ACUpdateOptions)

        ' Write wrapper end
        oStreamWriter.WriteLine(ACWrapperEnd)
        oStreamWriter.WriteLine("") ' just for good measure

        ' Close file
        oStreamWriter.Close()
        oStream.Close()

        GoTo Finally_Renamed

Catch_Renamed:
        ' Should really log an error but we don't want to interfere with the automated process

Finally_Renamed:
        ' Release objects
        oStream = Nothing
        oFS = Nothing
    End Sub


    Private Sub ProcessControl(ByRef oControl As Control, ByRef lGroupId As Integer)

        Dim sLabel As String
        Dim sControltype As String = String.Empty

        Dim sOptionNumber As String = "NULL"
        Dim sCommand As String = "NULL"
        Dim sMorO As String = "NULL"
        Dim sTabOrder As String = "NULL"
        Dim sCommandParameters As String = "NULL"
        Dim sParentName As String = "NULL"
        Dim sControlName As String = "NULL"

        Dim vArray() As Object = Nothing

        vArray = Convert.ToString(ControlHelper.GetTag(oControl)).Split(","c)

        If Information.IsArray(vArray) Then

            If vArray.GetUpperBound(0) > -1 Then

                sOptionNumber = CStr(vArray(0)).Trim()

                If vArray.GetUpperBound(0) > 0 Then

                    sCommand = "'" & CStr(vArray(1)).Trim() & "'"
                End If
            End If


            If vArray.GetUpperBound(0) > 1 Then

                If CStr(vArray(2)).Trim().ToUpper() = "NULL" Then
                    sMorO = "Null"
                Else

                    sMorO = "'" & CStr(vArray(2)).Trim() & "'"
                End If
            End If


            If vArray.GetUpperBound(0) > 2 Then

                sCommandParameters = "'" & CStr(vArray(3)).Trim() & "'"
            End If
        End If

        If TypeOf oControl Is Label Then
            sLabel = "'" & oControl.Text.Replace("'", "''") & "'"
            sControltype = "'Label'"
            If TypeOf oControl.Parent Is GroupBox Then
                sParentName = "'" & oControl.Parent.Name & "'"
            End If
            sControlName = "'" & oControl.Name & "'"
        Else
            sLabel = "NULL"
        End If

        If TypeOf oControl Is ComboBox Then
            If oControl.AccessibleDescription IsNot Nothing Then
                sLabel = "'" & oControl.AccessibleDescription.Replace("'", "''") & "'"
            End If
            sControltype = "'ComboBox'"
            sTabOrder = CStr(oControl.TabIndex)
            If TypeOf oControl.Parent Is GroupBox Then
                sParentName = "'" & oControl.Parent.Name & "'"
            End If
            sControlName = "'" & oControl.Name & "'"
        End If

        If TypeOf oControl Is CheckBox Then
            If oControl.AccessibleDescription IsNot Nothing Then
                sLabel = "'" & oControl.AccessibleDescription.Replace("'", "''") & "'"
            Else
                sLabel = "'" & oControl.Text.Replace("'", "''") & "'"
            End If
            sControltype = "'CheckBox'"
            sTabOrder = CStr(oControl.TabIndex)
            If TypeOf oControl.Parent Is GroupBox Then
                sParentName = "'" & oControl.Parent.Name & "'"
            End If
            sControlName = "'" & oControl.Name & "'"
        End If

        If TypeOf oControl Is Button Then
            sLabel = "'" & oControl.Text.Replace("'", "''") & "'"
            sControltype = "'Command'"
            sTabOrder = CStr(oControl.TabIndex)
            If TypeOf oControl.Parent Is GroupBox Then
                sParentName = "'" & oControl.Parent.Name & "'"
            End If
            sControlName = "'" & oControl.Name & "'"
        End If

        If TypeOf oControl Is TextBox Then
            If oControl.AccessibleDescription IsNot Nothing Then
                sLabel = "'" & oControl.AccessibleDescription.Replace("'", "''") & "'"
            End If

            sControltype = "'TextBox'"
            sTabOrder = CStr(oControl.TabIndex)
            If TypeOf oControl.Parent Is GroupBox Then
                sParentName = "'" & oControl.Parent.Name & "'"
            End If
            sControlName = "'" & oControl.Name & "'"
        End If

        If TypeOf oControl Is RadioButton Then
            sLabel = "'" & oControl.Text.Replace("'", "''") & "'"
            sControltype = "'OptionButton'"
            sTabOrder = CStr(oControl.TabIndex)
            sControlName = "'" & oControl.Name & "'"
        End If

        ''added new user control in system options
        If TypeOf oControl Is uctCompiledRule.uctCompiledRule Then
            If oControl.AccessibleDescription IsNot Nothing Then
                sLabel = "'" & oControl.AccessibleDescription.Replace("'", "''") & "'"
            Else
                sLabel = "'" & oControl.Text.Replace("'", "''") & "'"
            End If
            sControltype = "'uctCompiledRule'"
            sTabOrder = CStr(oControl.TabIndex)
            sControlName = "'" & oControl.Name & "'"
        End If

        ''added new user control in system options
        If TypeOf oControl Is GroupBox Then
            sLabel = "'" & oControl.Text.Replace("'", "''") & "'"
            sControltype = "'GroupBox'"
            sTabOrder = CStr(oControl.TabIndex)
            sControlName = "'" & oControl.Name & "'"
        End If

        If sControltype = "" Then
            sControltype = "'UNKNOWN'"
        End If

        If sOptionNumber = "" Then
            sOptionNumber = "NULL"
        End If
        oStreamWriter.WriteLine(ACInsertOption & sOptionNumber & ", " & CStr(lGroupId) & ", " & sControltype & ", " &
                          VB6.PixelsToTwipsY(oControl.Top) & ", " & CStr(VB6.PixelsToTwipsY(oControl.Height)) & ", " & CStr(VB6.PixelsToTwipsX(oControl.Left)) & ", " & CStr(VB6.PixelsToTwipsX(oControl.Width)) & ", " &
                          sLabel & ", " & sCommand & ", " & sMorO & ", " & sTabOrder & ", " & sCommandParameters & "," & sParentName & "," & sControlName & ")")

    End Sub


    Private Sub ProcessGroup(ByRef oForm As Object)

        Dim oControl As Control


        Dim lGroupId As Integer = oForm.GroupId

        ' Insert group record
        oStreamWriter.WriteLine("")

        oStreamWriter.WriteLine(Strings.Chr(9) & "-- Add " & oForm.GroupName & " group")



        oStreamWriter.WriteLine(ACInsertGroup & CStr(lGroupId) & ", '" & oForm.GroupName & "', " &
                                (IIf(oForm.ParentGroupID, oForm.ParentGroupID, "null")) & ", " & oForm.DisplayOrder & ")")

        ' Process controls


        For lCount As Integer = 0 To ContainerHelper.Controls(oForm).Count - 1

            oControl = ContainerHelper.Controls(oForm)(lCount)
            ProcessControl(oControl, lGroupId)
        Next lCount

        ' Release form
        oForm.Close()
    End Sub


    Private Sub ProcessGroups()
        ' *********************************************************************
        ' Adding new tabs
        ' ---------------
        ' Simple root tabs are defined as:
        '       ProcessGroup New frmMyNewTab
        '
        ' Nested tabs are defined as:
        '       oStream.WriteLine
        '       oStream.WriteLine vbTab & "-- Add MyNewGroup root group"
        '       oStream.WriteLine ACInsertGroup & "10, 'MyNewGroup', Null, 1)"
        '       ProcessGroup New frmMyNewTab
        '
        '
        ' A new tab form should have the same standard properties as others:
        '       DisplayOrder    - Automatically calculated from GroupId
        '       GroupId         - Root tabs should always be multiples of 10
        '                       - Sub tabs should be the root tab Id + 1to9.
        '                       - i.e. Accounts = 30, Accounts\General = 31
        '       GroupName       - Descriptive title
        '       ParentGroupID   - Automatically calculated from GroupId
        '
        ' If you with to insert a tab you will need to adjust the Id's of all
        ' following groups and/or tabs.
        '
        ' Definitions are kept in id and display order for clarity.
        ' *********************************************************************

        ' Add general options - i.d. 10+
        oStreamWriter.WriteLine("")
        oStreamWriter.WriteLine(Strings.Chr(9) & "-- Add Security root group")
        oStreamWriter.WriteLine(ACInsertGroup & "80, 'Security', Null, 0)")
        ProcessGroup(New frmPasswordManagement())

        ProcessGroup(New frmGeneral())
        ' Add installation options - i.d. 20+
        ProcessGroup(New frmInstallation())

        ' Add accounts options (with dummy group header) - i.d. 30+
        oStreamWriter.WriteLine("")
        oStreamWriter.WriteLine(Strings.Chr(9) & "-- Add Accounts root group")
        oStreamWriter.WriteLine(ACInsertGroup & "40, 'Accounts', Null, 4)")
        ProcessGroup(New frmAccountsGeneral())
        ProcessGroup(New frmAccountsAge())
        ProcessGroup(New frmAccountsAgent())

        ' Add claims options (with dummy group header) - i.d. 40+
        'oStream.WriteLine
        'oStream.WriteLine vbTab & "-- Add Claims root group"
        'oStream.WriteLine ACInsertGroup & "40, 'Claims', Null, 4)"
        'ProcessGroup New frmClaimGeneral
        'ProcessGroup New frmClaimLossSchedule

        ' Add underwriting options (with dummy group header) - i.d. 50+
        oStreamWriter.WriteLine("")
        oStreamWriter.WriteLine(Strings.Chr(9) & "-- Add Underwriting root group")
        oStreamWriter.WriteLine(ACInsertGroup & "50, 'Underwriting', Null, 5)")
        ProcessGroup(New frmUWGeneral())
        ProcessGroup(New frmUWClaims())
        'ProcessGroup New frmUWClaims2
        ProcessGroup(New frmUWRenewal())
        'ProcessGroup New frmUWInsurerPaymentAllocationDefault


        ' Add installation options - i.d. 60+
        ProcessGroup(New frmImportExport())

        ' Add underwriting options (with dummy group header) - i.d. 70+
        oStreamWriter.WriteLine("")
        oStreamWriter.WriteLine(Strings.Chr(9) & "-- Add SAM root group")
        oStreamWriter.WriteLine(ACInsertGroup & "70, 'SAM', NULL, 7)")
        ProcessGroup(New frmSAM())
        ProcessGroup(New frmSAMWorkManagerTasks())

        ProcessGroup(New frmCompiledRules())
        ' Add Payment Hub configuration options - i.d. 30+
        'oStreamWriter.WriteLine("")
        'oStreamWriter.WriteLine(Strings.Chr(9) & "-- Add Payment Hub root group")
        'oStreamWriter.WriteLine(ACInsertGroup & "90, 'Payment HUB', Null, 9)")
        ProcessGroup(New frmPaymentHub())
        ProcessGroup(New frmAuthentication())
    End Sub
End Module
