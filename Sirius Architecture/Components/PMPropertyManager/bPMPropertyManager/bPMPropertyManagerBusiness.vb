Option Strict Off
Option Explicit On
'developer guide no. 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 18/06/1998
    '
    ' Description: Contains the methods required to Manage the Properties
    '
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try


            ' Initialisation Code.

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: UpdateProperty
    '
    ' Description: Updates a given Property for a Given group.
    '              The property and/or group will be created if they
    '              do not already exist.
    ' ***************************************************************** '
    Public Function UpdateProperty(ByVal v_sGroupName As String, ByVal v_sPropertyName As String, ByVal v_vPropertyValue As Object) As Integer

        Dim result As Integer = 0
        Dim oGroup As bPMPropertyManager.Group
        Dim oProperties As bPMPropertyManager.Properties
        Dim sKey As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we have no Groups, create one
            If g_oGroups Is Nothing Then
                g_oGroups = New bPMPropertyManager.Groups()
            End If

            ' Gen the Key for this group
            sKey = g_oGroups.GenerateKey(v_sGroupName:=v_sGroupName)

            ' Get a Reference to the Property Group
            oGroup = g_oGroups.Item(sKey)

            ' If the Group does not exist, create it
            If oGroup Is Nothing Then

                ' Create a New Properties Collection
                oProperties = New bPMPropertyManager.Properties()
                'developer guide no 9.
                lReturn = CType(oProperties.Initialise(), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Add this Property to the Collection
                lReturn = CType(oProperties.Add(v_sPropertyName:=v_sPropertyName, v_vPropertyValue:=v_vPropertyValue), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Add the new Group
                lReturn = CType(g_oGroups.Add(v_sGroupName:=v_sGroupName, v_oGroupProperties:=oProperties), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                ' Group already exists, so just update the Property
                lReturn = CType(oGroup.GroupProperties.Update(v_sPropertyName:=v_sPropertyName, v_vPropertyValue:=v_vPropertyValue), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Release any Local references
            oProperties = Nothing
            oGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update the Property", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateProperty", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProperty
    '
    ' Description: Gets thevalue for a given Property for a Given group.
    ' ***************************************************************** '
    Public Function GetProperty(ByVal v_sGroupName As String, ByVal v_sPropertyName As String, ByRef r_vPropertyValue As Object) As Integer

        Dim result As Integer = 0
        Dim oGroup As bPMPropertyManager.Group
        Dim oProperty As Property_Renamed
        Dim sKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we have no Groups, create one
            If g_oGroups Is Nothing Then
                g_oGroups = New bPMPropertyManager.Groups()
            End If

            ' Gen the Key for this group
            sKey = g_oGroups.GenerateKey(v_sGroupName:=v_sGroupName)

            ' Get a Reference to the Property Group
            oGroup = g_oGroups.Item(sKey)

            ' If the Group does not exist, the Property Cannot
            If oGroup Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Generate the Key for the Property
            sKey = oGroup.GroupProperties.GenerateKey(v_sPropertyName:=v_sPropertyName)

            ' Get the Property from the collection
            oProperty = oGroup.GroupProperties.Item(sKey)

            ' Return the Property Value if it exists
            If oProperty Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                With oProperty
                    If Informations.IsReference(.PropertyValue) Then

                        r_vPropertyValue = .PropertyValue
                    Else


                        r_vPropertyValue = .PropertyValue
                    End If
                End With
            End If

            ' Release any Local references
            oProperty = Nothing
            oGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get the Property Value", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProperty", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteProperty
    '
    ' Description: Deletes a given Property for a given Group.
    ' ***************************************************************** '
    Public Function DeleteProperty(ByVal v_sGroupName As String, ByVal v_sPropertyName As String) As Integer

        Dim result As Integer = 0
        Dim oGroup As bPMPropertyManager.Group
        Dim oProperty As Property_Renamed
        Dim sKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we have no Groups, exit as we cannot have the Property
            If g_oGroups Is Nothing Then
                Return result
            End If

            ' Gen the Key for this group
            sKey = g_oGroups.GenerateKey(v_sGroupName:=v_sGroupName)

            ' Get a Reference to the Property Group
            oGroup = g_oGroups.Item(sKey)

            ' If the Group does not exist, the Property Cannot
            If oGroup Is Nothing Then
                Return result
            End If

            ' Generate the Key for the Property
            sKey = oGroup.GroupProperties.GenerateKey(v_sPropertyName:=v_sPropertyName)

            ' Get the Property from the collection
            oGroup.GroupProperties.Delete(sKey)

            ' Release any Local references
            oProperty = Nothing
            oGroup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get the Property Value", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteProperty", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteGroup
    '
    ' Description: Deletes a given Group.
    ' ***************************************************************** '
    Public Function DeleteGroup(ByVal v_sGroupName As String) As Integer

        Dim result As Integer = 0
        Dim sKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we have no Groups, exit as we cannot have the Property
            If g_oGroups Is Nothing Then
                Return result
            End If

            ' Gen the Key for this group
            sKey = g_oGroups.GenerateKey(v_sGroupName:=v_sGroupName)

            ' Delete the group
            g_oGroups.Delete(sKey)

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get the Property Value", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteGroup", excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
