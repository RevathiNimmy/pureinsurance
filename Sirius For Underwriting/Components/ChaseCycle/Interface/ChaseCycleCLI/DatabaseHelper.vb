Module DatabaseHelper
    ' ***************************************************************** '
    ' Name: AddParameterLite
    '
    ' Description: See below
    '
    ' ***************************************************************** '
    '
    ' Adds parameters for a stored procedure call
    Public Sub AddParameterLite( _
        ByVal v_oDatabase As dPMDAO.Database, _
        ByVal v_sName As String, _
        ByVal v_vValue As Object, _
        ByVal v_iDirection As PMEParameterDirection, _
        ByVal v_iDataType As PMEDataType, _
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
        iReturn = v_oDatabase.Parameters.Add( _
                sName:=v_sName, _
                vValue:=v_vValue, _
                iDirection:=CShort(v_iDirection), _
                iDataType:=CShort(v_iDataType))

        ' Check for success
        If (iReturn <> PMEReturnCode.PMTrue) Then
            ' Raise error
            Throw New Exception(String.Format("Error {0} adding parameter '{1}' with value '{2}'", iReturn, v_sName, v_vValue))
        End If

    End Sub

    Public Sub DBConnect(ByRef oDatabase As dPMDAO.Database)
        Dim iReturn As Integer

        oDatabase = New dPMDAO.Database

        ' Connect to database
        iReturn = oDatabase.OpenDatabase("sirius", 1, 1, ACApp)
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to connect to Sirius database")
        End If
    End Sub

    Public Sub DBDisconnect(ByRef oDatabase As dPMDAO.Database)
        Dim iReturn As Integer

        ' Close database if possible
        If Not oDatabase Is Nothing Then
            iReturn = oDatabase.CloseDatabase()
        End If

    End Sub
End Module
