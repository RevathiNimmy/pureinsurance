Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Module Module1
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Public Const ACApp As String = "PMTestLogon"
	

    Public Sub Main()

        'Dim oObj As Object
        'Dim l As Integer

        ' Create an instance of the object manager.
        Dim oObjectManager As New bObjectManager.ObjectManager

        ' Call the initialise method.
        m_lReturn = oObjectManager.Initialise(sCallingAppName:="Test Logon")

        If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
            ' Set the object manager to nothing.
            oObjectManager = Nothing
            Exit Sub
        End If

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Set the object manager to nothing.
            oObjectManager = Nothing

            MessageBox.Show("Failed to Logon - Check Log File and PMMessage Table", Application.ProductName)

            Exit Sub
        End If

        If oObjectManager.LoggedOnLocally Then
            MessageBox.Show("LoggedOn Locally OK!", Application.ProductName)
        Else
            MessageBox.Show("LoggedOn Remote OK!", Application.ProductName)
        End If

        oObjectManager = Nothing

    End Sub
End Module