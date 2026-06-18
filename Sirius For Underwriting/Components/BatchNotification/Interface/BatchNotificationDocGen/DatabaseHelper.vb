Imports PureInsurance
Imports SharedFiles
Imports SSP.Shared

Module DatabaseHelper
    Public Sub AddParameterLite(
        ByVal v_oDatabase As Object,
        ByVal v_sName As String,
        ByVal v_vValue As Object,
        ByVal v_iDirection As PMEParameterDirection,
        ByVal v_iDataType As PMEDataType,
        Optional ByVal v_bClearParameters As Boolean = False)

        Dim iReturn As Long

        ' Note: No error handling.
        '   Let serious errors bubble up to calling function.
        '   If we don't it will be very difficult to locate the cause.

        ' If this is the first parameter clear the current ones
        If v_bClearParameters Then
            v_oDatabase.Parameters.Clear()
        End If

        ' Add our new parameter
        iReturn = v_oDatabase.Parameters.Add(
                sName:=v_sName,
                vValue:=v_vValue,
                iDirection:=CShort(v_iDirection),
                iDataType:=CShort(v_iDataType))

        ' Check for success
        If (iReturn <> PMEReturnCode.PMTrue) Then
            ' Raise error
            Throw New Exception(String.Format("Error {0} adding parameter '{1}' with value '{2}'", iReturn, v_sName, v_vValue))
        End If

    End Sub

    Public Sub DBConnect(ByRef oDatabase As Object)
        Dim iReturn As Integer
        oDatabase = bPMFunc.CreateLateBoundObject("dPMDAO.Database")

        ' Connect to database
        iReturn = oDatabase.OpenDatabase("sirius", 1, 1, ACApp)
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to connect to Sirius database")
        End If
    End Sub

    Public Sub DBDisconnect(ByRef oDatabase As Object)
        Dim iReturn As Integer

        ' Close database if possible
        If Not oDatabase Is Nothing Then
            iReturn = oDatabase.CloseDatabase()
        End If

    End Sub
End Module
