Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class Keys
    Implements IDisposable
    Implements IEnumerable
    ' ***************************************************************** '
    ' Class Name: Keys
    '
    ' Date: 01/09/1998
    '
    ' Description: Maintains the Key Collection.
    '
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Keys"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the Navigator Collection
    Private m_colKeys As Collection
    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: CreateNew (Public)
    '
    ' Description: Creates a new Keys Collection using the Key Array
    '              supplied.
    ' ***************************************************************** '
    Public Function CreateNew(ByVal v_vKeyArray(,) As Object, ByVal v_bSetKeys As Boolean) As Integer

        Dim result As Integer = 0
        Dim sKeyName As String
        Dim vInitialValue As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear any existing Keys in this Collection
            Clear()

            ' If there are no Keys in the Array then exit.
            If Not Information.IsArray(v_vKeyArray) Then
                Return result
            End If

            ' For each Key in the Array
            For lRow As Integer = v_vKeyArray.GetLowerBound(1) To v_vKeyArray.GetUpperBound(1)

                ' Get the Key Attributes

                sKeyName = CStr(v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow))

                ' If they are Set Keys
                If v_bSetKeys Then
                    ' Get the initial Value (If there is one)

                    vInitialValue = CStr(v_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    ' Add to the Collection
                    lReturn = CType(Add(v_sKeyName:=sKeyName, v_vValue:=vInitialValue), gPMConstants.PMEReturnCode)
                Else
                    ' Add to the Collection
                    lReturn = CType(Add(v_sKeyName:=sKeyName), gPMConstants.PMEReturnCode)
                End If

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lRow

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateNew Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateNew", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateValue
    '
    ' Description: UpdateValue updates the value for a single Key
    '              in the Keys Collection.
    '
    ' Note: The Key will be added to the Collection if it doesn't already
    '       exist.
    ' ***************************************************************** '
    Public Function UpdateValue(ByVal v_sKeyName As String, ByVal v_vValue As Object) As Integer

        Dim result As Integer = 0
        Dim oKey As iPMNavigator.Key
        Dim sKeyIndex As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Derive the Key Index
            sKeyIndex = GenerateKey(v_sKeyName:=v_sKeyName)

            ' Try and Find the Key in the Collection
            oKey = Item(sKeyIndex)

            ' Is the Key Found
            If oKey Is Nothing Then
                ' No - So Add it.
                result = Add(v_sKeyName:=v_sKeyName, v_vValue:=v_vValue)
            Else
                ' Yes - So Update it.
                oKey.Value = v_vValue
            End If

            ' Release the local reference
            oKey = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update the Key Value for - " & v_sKeyName, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single Key into the Keys Collection
    '
    '
    ' ***************************************************************** '

    Public Function Add(ByVal v_sKeyName As String, Optional ByVal v_vValue As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oKey As iPMNavigator.Key
        Dim sKeyIndex As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a Local Key reference
            oKey = New iPMNavigator.Key()

            ' Set the Key Properties
            With oKey
                .KeyName = v_sKeyName
                .Value = v_vValue
            End With

            ' Derive the Key Index
            sKeyIndex = GenerateKey(v_sKeyName:=v_sKeyName)

            ' Add the supplied Key into the collection
            m_colKeys.Add(oKey, sKeyIndex)

            ' Release the local reference
            oKey = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Key to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GenerateKey
    '
    ' Description: GenerateKeys a Key for the supplied details.
    '
    '
    ' ***************************************************************** '
    Public Function GenerateKey(ByVal v_sKeyName As String) As String

        Dim result As String = String.Empty
        Try

            ' Derive the Summary Key

            Return v_sKeyName.Trim()

        Catch excep As System.Exception



            ' Error.
            result = ""

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GenerateKey for - " & v_sKeyName, vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateKey", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of Keys in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try


            Return m_colKeys.Count

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            If Information.VarType(v_vKey) = VariantType.String Then
                v_vKey = v_vKey.Trim()
            End If

            ' Remove from the collection based on the key value.
            m_colKeys.Remove(v_vKey)

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
    Public Function Item(ByVal v_vKey As String) As iPMNavigator.Key

        Try

            ' If the key is a string trim it.
            If Information.VarType(v_vKey) = VariantType.String Then
                v_vKey = v_vKey.Trim()
            End If

            ' Return the Item based on the Key

            Return m_colKeys(v_vKey)

        Catch



            ' If not found return Nothing

            Return Nothing
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the Keys Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set Keys Collection to Nothing
            m_colKeys = Nothing
            m_colKeys = New Collection()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            Return m_colKeys.GetEnumerator

        Catch



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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            Clear()
        End If
        Me.disposedValue = True
    End Sub

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        Try

            m_colKeys = New Collection()

            ' Class Initialise

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
