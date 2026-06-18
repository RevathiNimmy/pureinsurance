Option Strict Off
Option Explicit On
Imports SSP.Shared
Friend NotInheritable Class Address
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Address
    '
    ' Date: 12/06/1998
    '
    ' Description: Describes the Address attributes.
    '
    ' Edit History: TF120698 - Created
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Address"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Class Attributes
    Private m_sAddressLine1 As String = ""
    Private m_sAddressLine2 As String = ""
    Private m_sAddressLine3 As String = ""
    Private m_sAddressLine4 As String = ""
    Private m_sPostCode As String = ""

    Private m_sOrganisation As String = ""
    Private m_sPOBox As String = ""
    Private m_sSubPremises As String = ""
    Private m_sBuildingName As String = ""
    Private m_sBuildingNumber As String = ""
    Private m_sDependentThoroughfare As String = ""
    Private m_sThoroughfare As String = ""
    Private m_sDependentLocality As String = ""
    Private m_sLocality As String = ""
    Private m_sPostTown As String = ""
    Private m_sCounty As String = ""
    'DAK140700
    ' CountryId
    Private m_iCountryId As Integer
    'JDW added for CNIC QASNames 20/08/01
    Private m_sTitle As String = ""
    Private m_sForename As String = ""
    Private m_sSurname As String = ""
    Private m_sInitial As String = ""

    Private m_lPMAddressCnt As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property AddressLine1() As String
        Get

            Return m_sAddressLine1

        End Get
        Set(ByVal Value As String)

            m_sAddressLine1 = Value

        End Set
    End Property

    Public Property AddressLine2() As String
        Get

            Return m_sAddressLine2

        End Get
        Set(ByVal Value As String)

            m_sAddressLine2 = Value

        End Set
    End Property

    Public Property AddressLine3() As String
        Get

            Return m_sAddressLine3

        End Get
        Set(ByVal Value As String)

            m_sAddressLine3 = Value

        End Set
    End Property

    Public Property AddressLine4() As String
        Get

            Return m_sAddressLine4

        End Get
        Set(ByVal Value As String)

            m_sAddressLine4 = Value

        End Set
    End Property

    Public Property PostCode() As String
        Get

            Return m_sPostCode

        End Get
        Set(ByVal Value As String)

            m_sPostCode = Value

        End Set
    End Property

    Public Property Organisation() As String
        Get

            Return m_sOrganisation

        End Get
        Set(ByVal Value As String)

            m_sOrganisation = Value

        End Set
    End Property

    Public Property POBox() As String
        Get

            Return m_sPOBox

        End Get
        Set(ByVal Value As String)

            m_sPOBox = Value

        End Set
    End Property

    Public Property SubPremises() As String
        Get

            Return m_sSubPremises

        End Get
        Set(ByVal Value As String)

            m_sSubPremises = Value

        End Set
    End Property

    Public Property BuildingName() As String
        Get

            Return m_sBuildingName

        End Get
        Set(ByVal Value As String)

            m_sBuildingName = Value

        End Set
    End Property

    Public Property BuildingNumber() As String
        Get

            Return m_sBuildingNumber

        End Get
        Set(ByVal Value As String)

            m_sBuildingNumber = Value

        End Set
    End Property

    Public Property DependentThoroughfare() As String
        Get

            Return m_sDependentThoroughfare

        End Get
        Set(ByVal Value As String)

            m_sDependentThoroughfare = Value

        End Set
    End Property

    Public Property Thoroughfare() As String
        Get

            Return m_sThoroughfare

        End Get
        Set(ByVal Value As String)

            m_sThoroughfare = Value

        End Set
    End Property

    Public Property DependentLocality() As String
        Get

            Return m_sDependentLocality

        End Get
        Set(ByVal Value As String)

            m_sDependentLocality = Value

        End Set
    End Property

    Public Property Locality() As String
        Get

            Return m_sLocality

        End Get
        Set(ByVal Value As String)

            m_sLocality = Value

        End Set
    End Property

    Public Property PostTown() As String
        Get

            Return m_sPostTown

        End Get
        Set(ByVal Value As String)

            m_sPostTown = Value

        End Set
    End Property

    Public Property County() As String
        Get

            Return m_sCounty

        End Get
        Set(ByVal Value As String)

            m_sCounty = Value

        End Set
    End Property

    'DAK140700
    Public Property CountryId() As Integer
        Get
            Return m_iCountryId
        End Get
        Set(ByVal Value As Integer)
            m_iCountryId = Value
        End Set
    End Property

    Public Property PMAddressCnt() As Integer
        Get

            Return m_lPMAddressCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPMAddressCnt = Value

        End Set
    End Property

    'JDW Added for CNIC 20/08/2001

    Public Property Title() As String
        Get
            Return m_sTitle
        End Get
        Set(ByVal Value As String)
            m_sTitle = Value
        End Set
    End Property


    Public Property Forename() As String
        Get
            Return m_sForename
        End Get
        Set(ByVal Value As String)
            m_sForename = Value
        End Set
    End Property


    Public Property Initial() As String
        Get
            Return m_sInitial
        End Get
        Set(ByVal Value As String)
            m_sInitial = Value
        End Set
    End Property


    Public Property Surname() As String
        Get
            Return m_sSurname
        End Get
        Set(ByVal Value As String)
            m_sSurname = Value
        End Set
    End Property
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

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



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        'bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
