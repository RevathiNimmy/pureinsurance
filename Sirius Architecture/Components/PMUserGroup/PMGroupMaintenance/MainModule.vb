Option Strict Off
Option Explicit On
Imports System
'developer guide no.129
Imports SharedFiles
Module MainModule

    Public Const ACApp As String = "PMGroupMaintenance"


    Public Sub Main()

        'developer guide no.244
        Dim oObject As New iPMGroupMaintenance.Interface_Renamed
        'developer guide no.9
        Dim lReturn As gPMConstants.PMEReturnCode = oObject.Initialise()

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oObject = Nothing
            Exit Sub
        End If

        lReturn = oObject.Start()

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oObject = Nothing
            Exit Sub
        End If

		oObject.Dispose()


        oObject = Nothing

    End Sub
End Module