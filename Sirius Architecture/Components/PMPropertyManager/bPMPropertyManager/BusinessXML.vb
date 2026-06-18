Option Strict Off
Option Explicit On
'developer guide no. 129
Imports SSP.Shared

<Serializable()>
<System.Runtime.InteropServices.ProgId("BusinessXML_NET.BusinessXML")>
Public NotInheritable Class BusinessXML
    Implements IDisposable
    '======================================================================================
    ' New version to persist Properties via physical XML files - RFC290102
    ' JRD 04/01/2005 PN17766 Amended to persist properties via Variant Cache object
    ' as persisting to XML files cause file locking issues in a multiuser environment.
    ' Properties are stored using a key of "GroupName PropertyName".
    ' A list of all current properties held in the Variant Cache object is also held
    ' within the Cache object.  This Cache List is used to determine which properties
    ' to delete when calling DeleteGroup.
    ' Project settings amended to create a single-instance ActiveX Exe
    '======================================================================================

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

    Private Const ACClass As String = "BusinessXML"
    Private Const ACCacheKeys As String = "CACHE_KEYS"
    Private Const ACKeyDelimiter As String = " "

    ' ************************************************

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

            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            '    m_sUsername$ = sUserName$
            '    m_sPassword$ = sPassword$
            '    m_iUserID% = iUserID%
            '    m_sCallingAppName$ = sCallingAppName$
            '    m_iLanguageID% = iLanguageID%
            '    m_iSourceID% = iSourceID%
            '    m_iCurrencyID% = iCurrencyID%
            '    m_iLogLevel% = iLogLevel%


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
        'Dim oCache As VariantCacheLib.Cache
        Dim sKey As String = ""
        Dim vCacheKeys As Object = Nothing
        Dim lArraySize As Integer
        Dim bFound As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RFC280802 - Remove Spaces from names
            v_sGroupName = v_sGroupName.Replace(" ", "").Trim().ToUpper()
            v_sPropertyName = v_sPropertyName.Replace(" ", "").Trim().ToUpper()

            'oCache = New VariantCacheLib.Cache()
            sKey = v_sGroupName & ACKeyDelimiter & v_sPropertyName
            'oCache.Remove(sKey)
            'oCache.Add(sKey, v_vPropertyValue)

            'Store property in Cache List if it is not already present


            'vCacheKeys = oCache.Item(ACCacheKeys)
            If Not Informations.IsArray(vCacheKeys) Then
                ReDim vCacheKeys(0)

                vCacheKeys(0) = sKey
            Else

                lArraySize = vCacheKeys.GetUpperBound(0)
                For lCount As Integer = 0 To lArraySize

                    If CStr(vCacheKeys(lCount)) = sKey Then
                        bFound = True
                        Exit For
                    End If
                Next lCount
                If Not bFound Then
                    lArraySize += 1
                    ReDim Preserve vCacheKeys(lArraySize)

                    vCacheKeys(lArraySize) = sKey
                End If
            End If
            'Store Cache List
            'oCache.Add(ACCacheKeys, vCacheKeys)

            'oCache = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update Property Value : " & v_sGroupName & "." & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateProperty", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProperty
    '
    ' Description: Gets the value for a given Property for a Given group.
    ' ***************************************************************** '
    Public Function GetProperty(ByVal v_sGroupName As String, ByVal v_sPropertyName As String, ByRef r_vPropertyValue As Object) As Integer

        Dim result As Integer = 0
        'developer guide no 12. 
        Dim oCache As Hashtable
        Dim sKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RFC280802 - Remove Spaces from names
            v_sGroupName = v_sGroupName.Replace(" ", "").Trim().ToUpper()
            v_sPropertyName = v_sPropertyName.Replace(" ", "").Trim().ToUpper()

            'Retrieve the property from the Cache
            'oCache = New VariantCacheLib.Cache()
            oCache = New Hashtable()
            sKey = v_sGroupName & ACKeyDelimiter & v_sPropertyName

            If Not Object.Equals(oCache.Item(sKey), Nothing) Then
                If Informations.IsReference(oCache.Item(sKey)) Then
                    r_vPropertyValue = oCache.Item(sKey)
                Else


                    r_vPropertyValue = oCache.Item(sKey)
                End If
            End If
            oCache = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get the Property Value: " & v_sGroupName & "." & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetProperty", excep:=excep)

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
        'developer guide no.12
        Dim oCache As Hashtable
        Dim sKey As String = ""
        Dim vCacheKeys As Object = Nothing
        Dim lOldArraySize As Integer
        Dim vNewCacheKeys As Object = Nothing
        Dim lNewArraySize, lRow As Integer
        Dim bFound As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RFC280802 - Remove Spaces from names
            v_sGroupName = v_sGroupName.Replace(" ", "").Trim().ToUpper()
            v_sPropertyName = v_sPropertyName.Replace(" ", "").Trim().ToUpper()

            'Remove the property from the cache
            'developer guide no.12
            oCache = New Hashtable()
            sKey = v_sGroupName & ACKeyDelimiter & v_sPropertyName
            oCache.Remove(sKey)

            'Update the Cache List


            vCacheKeys = oCache.Item(ACCacheKeys)
            If Informations.IsArray(vCacheKeys) Then
                'Is the Property to be deleted in the cache

                lOldArraySize = vCacheKeys.GetUpperBound(0)
                For lCount As Integer = 0 To lOldArraySize

                    If CStr(vCacheKeys(lCount)) = sKey Then
                        bFound = True
                        Exit For
                    End If
                Next lCount

                'Remove the property from the Cache List if it exists
                If bFound Then

                    lOldArraySize = vCacheKeys.GetUpperBound(0)

                    lNewArraySize = lOldArraySize - 1
                    If lNewArraySize > -1 Then
                        ReDim vNewCacheKeys(lNewArraySize)
                    End If

                    lRow = 0
                    For lCount As Integer = 0 To lOldArraySize
                        'Copy all other properties to the new Cache List

                        If CStr(vCacheKeys(lCount)) <> sKey Then


                            vNewCacheKeys(lRow) = vCacheKeys(lCount)
                            lRow += 1
                        End If
                    Next lCount

                    'Store the updated Cache List
                    oCache.Remove(ACCacheKeys)
                    If Informations.IsArray(vNewCacheKeys) Then
                        oCache.Add(ACCacheKeys, vNewCacheKeys)
                    End If
                End If
            End If

            oCache = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete Property Value : " & v_sGroupName & "." & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteProperty", excep:=excep)

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
        Dim lReturn As gPMConstants.PMEReturnCode
        'commented code related to caching
        'Dim oCache As VariantCacheLib.Cache
        Dim vKey As Object
        Dim sCachedGroupName, sCachedPropertyName As String
        Dim vCacheKeys As Object = Nothing
        Dim lArraySize As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RFC280802 - Remove Spaces from names
            v_sGroupName = v_sGroupName.Replace(" ", "").Trim().ToUpper()

            'Get Cache List
            'oCache = New VariantCacheLib.Cache()


            'vCacheKeys = oCache.Item(ACCacheKeys)
            'oCache = Nothing
            If Informations.IsArray(vCacheKeys) Then

                lArraySize = vCacheKeys.GetUpperBound(0)
                'Delete each property in the list for the Group
                For lCount As Integer = 0 To lArraySize


                    vKey = CStr(vCacheKeys(lCount)).Split(New String() {ACKeyDelimiter}, StringSplitOptions.None)

                    sCachedGroupName = CStr(vKey(0))

                    sCachedPropertyName = CStr(vKey(1))
                    If sCachedGroupName = v_sGroupName Then
                        'DeleteProperty will remove the property from the Cache List
                        lReturn = CType(DeleteProperty(sCachedGroupName, sCachedPropertyName), gPMConstants.PMEReturnCode)
                    End If
                Next lCount
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete Group : " & v_sGroupName, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteGroup", excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
