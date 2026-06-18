Option Strict Off
Option Explicit On
Imports System

Imports SSP.Shared
<Serializable()> _
Friend NotInheritable Class PMDAOInstance 

    Private m_eFamily As gPMConstants.PMEProductFamily
    Private m_oDatabase As dPMDAO.Database


    Public Property Family() As gPMConstants.PMEProductFamily
        Get

            Return m_eFamily

        End Get
        Set(ByVal Value As gPMConstants.PMEProductFamily)

            m_eFamily = Value

        End Set
    End Property



    Public Property Database() As dPMDAO.Database
        Get

            Return m_oDatabase

        End Get
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property
    Private Shared _DefaultInstance As PMDAOInstance = Nothing
    Public Shared ReadOnly Property DefaultInstance() As PMDAOInstance
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New PMDAOInstance
            End If
            Return _DefaultInstance
        End Get
    End Property
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
