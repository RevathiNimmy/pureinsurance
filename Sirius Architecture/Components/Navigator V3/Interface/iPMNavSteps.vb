Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class Steps
    Implements IDisposable
    Implements IEnumerable
    ' ***************************************************************** '
    ' Class Name: Steps
    '
    ' Date: 01/09/1998
    '
    ' Description: Maintains the Step Collection.
    '
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Steps"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the Navigator Collection
    Private m_colSteps As Collection
    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: CreateNew (Public)
    '
    ' Description: Creates a Steps Collection using the Details
    '              supplied.
    ' ***************************************************************** '
    Public Function CreateNew(ByVal v_vMapStepsArray(,) As Object, ByVal v_vStepsSetKeyArray As Object, ByVal v_vStepsGetKeyArray As Object) As Integer

        Dim result As Integer = 0
        Dim lMapID, lStepID As Integer
        'developer guide no. 101
        Dim vComponentID As Object
        Dim vComponentType As Object
        Dim vObjectName As Object
        Dim vClassName As Object
        Dim vIsServerSide As Object
        Dim vSubMapID As Object
        Dim vTask As Object
        Dim vOkAction As Object
        Dim vCancelAction As Object
        Dim vOkNoOfSteps As Object
        Dim vCancelNoOfSteps As Object
        Dim vOkProcessID As Object
        Dim vCancelProcessID As Object
        Dim vNavigateStatus As Object
        Dim sCaption As String = ""
        Dim iIsHidden, iIsLogged As Integer

        Dim eCol As NavigatorConstants.ACEMapStepsColPos
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear any existing Keys in this Collection
            Clear()

            ' If there are no Steps in the Array then exit.
            If Not Information.IsArray(v_vMapStepsArray) Then
                Return result
            End If

            ' For each Step in the Array
            For lRow As Integer = v_vMapStepsArray.GetLowerBound(1) To v_vMapStepsArray.GetUpperBound(1)

                ' Get the Step Attributes
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSMapID

                'Developer guide no.84
                lMapID = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSStepID

                lStepID = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSComponentID

                vComponentID = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSComponentType

                vComponentType = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSObjectName

                vObjectName = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSClassName

                vClassName = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSIsServerSide

                vIsServerSide = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSSubMapID

                vSubMapID = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSTask

                vTask = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSOKAction

                vOkAction = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSCancelAction

                vCancelAction = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSOKNoOfSteps

                vOkNoOfSteps = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSCancelNoOfSteps

                vCancelNoOfSteps = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSOKProcessID

                vOkProcessID = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSCancelProcessID

                vCancelProcessID = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSNavigateStatus

                vNavigateStatus = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSStepCaption

                sCaption = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSIsHidden

                iIsHidden = v_vMapStepsArray(eCol, lRow)
                eCol = NavigatorConstants.ACEMapStepsColPos.aceMSIsLogged

                iIsLogged = v_vMapStepsArray(eCol, lRow)

                ' Add to Collection
                lReturn = CType(Add(v_lMapID:=lMapID, v_lStepID:=lStepID, v_vComponentID:=vComponentID, v_vComponentType:=CStr(vComponentType), v_vObjectName:=CStr(vObjectName), v_vClassName:=CStr(vClassName), v_vIsServerSide:=vIsServerSide, v_vSubMapID:=vSubMapID, v_vTask:=vTask, v_vOkAction:=CStr(vOkAction), v_vCancelAction:=CStr(vCancelAction), v_vOkNoOfSteps:=vOkNoOfSteps, v_vCancelNoOfSteps:=vCancelNoOfSteps, v_vOkProcessID:=vOkProcessID, v_vCancelProcessID:=vCancelProcessID, v_vNavigateStatus:=CStr(vNavigateStatus), v_sCaption:=sCaption, v_iIsHidden:=iIsHidden, v_iIsLogged:=iIsLogged, v_lItemNumber:=lRow + 1), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lRow

            ' Add the Steps Set Keys

            lReturn = CType(LoadStepsKeys(v_lMapID:=lMapID, v_vStepsKeyArray:=v_vStepsSetKeyArray, v_bSetKeys:=True), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Steps Get Keys

            lReturn = CType(LoadStepsKeys(v_lMapID:=lMapID, v_vStepsKeyArray:=v_vStepsGetKeyArray, v_bSetKeys:=False), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
    ' Name: Add
    '
    ' Description: Adds a single Navigator into the Navigators Collection
    '
    '
    ' ***************************************************************** '
    Public Function Add(ByVal v_lMapID As Integer, ByVal v_lStepID As Integer, ByVal v_vComponentID As Integer, ByVal v_vComponentType As String, ByVal v_vObjectName As String, ByVal v_vClassName As String, ByVal v_vIsServerSide As Integer, ByVal v_vSubMapID As Integer, ByVal v_vTask As Integer, ByVal v_vOkAction As String, ByVal v_vCancelAction As String, ByVal v_vOkNoOfSteps As Integer, ByVal v_vCancelNoOfSteps As Integer, ByVal v_vOkProcessID As Integer, ByVal v_vCancelProcessID As Integer, ByVal v_vNavigateStatus As String, ByVal v_sCaption As String, ByVal v_iIsHidden As Integer, ByVal v_iIsLogged As Integer, ByVal v_lItemNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim oStep As Step_Renamed
        Dim sKey As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oStep = New Step_Renamed()

            ' Initialise the Step
            'developer guide no. 9
            lReturn = oStep.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Generate the Step Key
            sKey = GenerateKey(v_lMapID:=v_lMapID, v_lStepID:=v_lStepID)

            ' Set the Step Properties
            With oStep
                .MapID = v_lMapID
                .StepID = v_lStepID
                .ComponentID = v_vComponentID
                .ComponentType = v_vComponentType
                .ObjectName = v_vObjectName
                .ClassName = v_vClassName
                .IsServerSide = v_vIsServerSide
                .SubMapID = v_vSubMapID
                .Task = v_vTask
                .OkAction = v_vOkAction
                .CancelAction = v_vCancelAction
                .OkNoOfSteps = v_vOkNoOfSteps
                .CancelNoOfSteps = v_vCancelNoOfSteps
                .OkProcessID = v_vOkProcessID
                .CancelProcessID = v_vCancelProcessID
                .NavigateStatus = v_vNavigateStatus
                .Caption = v_sCaption
                .IsHidden = v_iIsHidden
                .IsLogged = v_iIsLogged
                .StepKey = sKey
                .ItemNumber = v_lItemNumber
            End With

            ' Add the supplied step into the collection
            m_colSteps.Add(oStep, sKey)

            ' Release the Local Reference
            oStep = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Step to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function GenerateKey(ByVal v_lMapID As Integer, ByVal v_lStepID As Integer) As String

        Dim result As String = String.Empty
        Try

            ' Derive the Step Key
            result = (gPMConstants.PMMapKeyPrefix & Conversion.Str(v_lMapID).Trim()).Trim()


            Return result & _
            (gPMConstants.PMStepKeyPrefix & Conversion.Str(v_lStepID).Trim()).Trim()

        Catch excep As System.Exception



            ' Error.
            result = ""

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GenerateKey", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateKey", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of Steps in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try


            Return m_colSteps.Count

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
    ' Description: Delete a Step from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Delete(ByRef v_vKey As String)

        Try

            ' If we have a string key Trim It.
            If Information.VarType(v_vKey) = VariantType.String Then
                v_vKey = v_vKey.Trim()
            End If

            ' Remove from the collection based on the Key
            m_colSteps.Remove(v_vKey)

        Catch



            ' If there is nothing to remove just return

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
    Public Function Item(ByRef v_vKey As Object) As Step_Renamed

        Try

            ' If we have a string key Trim It.
            If Information.VarType(v_vKey) = VariantType.String Then
                v_vKey = v_vKey.Trim()
            End If

            ' Return the Item from the Collection

            Return m_colSteps(v_vKey)

        Catch



            ' If the Item is not found return Nothing

            Return Nothing
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the Navigator Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set Step Collection to Nothing
            m_colSteps = Nothing
            m_colSteps = New Collection()

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


            Return m_colSteps.GetEnumerator

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

            m_colSteps = New Collection()

            ' Class Initialise

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the Navigator class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: LoadStepsKeys (Private)
    '
    ' Description: Loads the Steps keys
    '
    ' ***************************************************************** '
    Private Function LoadStepsKeys(ByVal v_lMapID As Integer, ByVal v_vStepsKeyArray(,) As Object, ByVal v_bSetKeys As Boolean) As Integer

        Dim result As Integer = 0
        Dim oStep As Step_Renamed
        Dim eCol As NavigatorConstants.ACEMapStepsKeyColPos
        Dim sKey As String = ""

        Dim sKeyName As String = ""
        Dim vKeyValue As String = ""

        Dim lStepID, lPreviousStepID As Integer

        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        lPreviousStepID = -1

        If Not Information.IsArray(v_vStepsKeyArray) Then
            Return result
        End If

        ' Loop round all records
        For lRow As Integer = v_vStepsKeyArray.GetLowerBound(1) To v_vStepsKeyArray.GetUpperBound(1)

            eCol = NavigatorConstants.ACEMapStepsKeyColPos.aceMSKeyStepID

            lStepID = CInt(v_vStepsKeyArray(eCol, lRow))
            eCol = NavigatorConstants.ACEMapStepsKeyColPos.aceMSKeyName

            sKeyName = CStr(v_vStepsKeyArray(eCol, lRow))

            If v_bSetKeys Then
                eCol = NavigatorConstants.ACEMapStepsKeyColPos.aceMSKeyInitialValue

                vKeyValue = CStr(v_vStepsKeyArray(eCol, lRow))
            End If

            ' Has the Step ID has changed
            If lPreviousStepID <> lStepID Then
                ' Yes, so get a reference to the new step

                ' Generate the Step Key
                sKey = GenerateKey(v_lMapID:=v_lMapID, v_lStepID:=lStepID)

                oStep = Item(sKey)

                If oStep Is Nothing Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Navigator Step Not Found  - ID: " & Conversion.Str(lStepID), vApp:=ACApp, vClass:=ACClass, vMethod:="LoadStepsKeys")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            If v_bSetKeys Then
                lReturn = CType(oStep.SetKeys.Add(v_sKeyName:=sKeyName, v_vValue:=vKeyValue), gPMConstants.PMEReturnCode)
            Else
                lReturn = CType(oStep.GetKeys.Add(v_sKeyName:=sKeyName), gPMConstants.PMEReturnCode)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Store Previous Step ID for next iteration
            lPreviousStepID = lStepID

        Next lRow

        oStep = Nothing

        Return result

    End Function
End Class

