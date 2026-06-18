Option Strict Off
Option Explicit On
'developer guide no. 129
Imports SSP.Shared

Friend NotInheritable Class Properties
    Implements IDisposable
    Implements IEnumerable
    ' ***************************************************************** '
    ' Class Name: Properties
    '
    ' Date: 18/06/1998
    '
    ' Description: Maintains the Properties Collection.
    '
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Properties"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the Properties Collection
    Private _m_colProperties As Hashtable = Nothing
    Private Property m_colProperties() As Hashtable
        Get
            If _m_colProperties Is Nothing Then
                _m_colProperties = New Hashtable()
            End If
            Return _m_colProperties
        End Get
        Set(ByVal Value As Hashtable)
            _m_colProperties = Value
        End Set
    End Property
    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Update
    '
    ' Description: Updates a single Property into the Properties Collection
    '
    '
    ' ***************************************************************** '
    Public Function Update(ByVal v_sPropertyName As String, ByVal v_vPropertyValue As Object) As Integer

        Dim result As Integer = 0
        Dim oProperty As Property_Renamed
        Dim sKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Derive the Key
            sKey = GenerateKey(v_sPropertyName:=v_sPropertyName)

            ' Try and get this from the collection
            oProperty = Item(sKey)

            ' If we haven't already got this one
            If oProperty Is Nothing Then
                ' Add it
                Return Add(v_sPropertyName:=v_sPropertyName, v_vPropertyValue:=v_vPropertyValue)
            Else
                ' Update the Properties
                With oProperty
                    .PropertyName = v_sPropertyName



                    'developer guide no. 22
                    .PropertyValue = v_vPropertyValue
                End With
            End If

            ' Release the local reference
            oProperty = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update New Property to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single Property into the Properties Collection
    '
    '
    ' ***************************************************************** '
    Public Function Add(ByVal v_sPropertyName As String, ByVal v_vPropertyValue As Object) As Integer

        Dim result As Integer = 0
        Dim oProperty As Property_Renamed
        Dim sKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a New Local Property
            oProperty = New Property_Renamed()

            ' Set the Properties
            With oProperty
                .PropertyName = v_sPropertyName


                'developer guide no. 22
                .PropertyValue = v_vPropertyValue
            End With

            ' Derive the Key
            sKey = GenerateKey(v_sPropertyName:=v_sPropertyName)

            ' Add the supplied Key into the collection
            m_colProperties.Add(oProperty, sKey)

            ' Release the local reference
            oProperty = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add New Property to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GenerateKey
    '
    ' Description: Generates a Key for the supplied details.
    '
    '
    ' ***************************************************************** '
    Public Function GenerateKey(ByVal v_sPropertyName As String) As String

        Dim result As String = String.Empty
        Try

            ' Derive the Summary Key

            Return v_sPropertyName.Trim()

        Catch excep As System.Exception



            ' Error.
            result = ""

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GenerateKey for - " & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateKey", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of Properties in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try


            Return m_colProperties.Count

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a Key from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Delete(ByVal v_vKey As String)

        Try

            ' If the key is a string trim it.
            'If Information.VarType(v_vKey) = VariantType.String Then   // this statement doesn't with latest version of Microsoft.VisualBasic
            If TypeOf v_vKey Is String Then
                v_vKey = v_vKey.Trim()
            End If

            ' Remove from the collection based on the key value.
            m_colProperties.Remove(v_vKey)

        Catch



            ' If there was nothing to delete just return

            Exit Sub
        End Try


    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected Step from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Function Item(ByVal v_vKey As String) As Property_Renamed

        Try

            ' If the key is a string trim it.
            'If Information.VarType(v_vKey) = VariantType.String Then
            If TypeOf v_vKey Is String Then
                v_vKey = v_vKey.Trim()
            End If

            ' Return the Item based on the Key

            Return m_colProperties(v_vKey)

        Catch



            ' If not found return Nothing

            Return Nothing
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the Properties Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set Properties Collection to Nothing
            m_colProperties = Nothing

        Catch excep As System.Exception



            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: NewEnum
    '
    ' Description: Provides For Each Type functionality by allowing
    '              access to the Native Collection Enumerator object.
    '
    '              This Method must be hidded and have a Procedure ID
    '              of -4. These attributes can be set via the Procedure
    '              Attributes dialog. (On Tools Menu).
    '
    ' ***************************************************************** '

    Public Function GetEnumerator() As IEnumerator Implements System.Collections.IEnumerable.GetEnumerator

        Try


            Return m_colProperties.GetEnumerator

        Catch

            Return Nothing

            Exit Function
        End Try

    End Function

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
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
                m_colProperties = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

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
        'gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
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
