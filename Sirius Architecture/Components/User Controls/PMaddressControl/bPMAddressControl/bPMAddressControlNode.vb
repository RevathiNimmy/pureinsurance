Option Strict Off
Option Explicit On
Imports SSP.Shared
Friend NotInheritable Class PickListNode
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: PickListNode
    '
    ' Date: 12/06/1998
    '
    ' Description: Describes the PickListNode attributes.
    '
    ' Edit History: TF120698 - Created
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "PickListNode"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Class Attributes
    Private m_lNodeID As Integer
    Private m_sNodeType As String = ""
    Private m_lParentNodeID As Integer
    Private m_iLevel As Integer
    Private m_iCount As Integer
    Private m_iNoOfChildren As Integer
    Private m_sCaption As String = ""

    Private m_oAddress As bPMAddressControl.Address
    ' PRIVATE Data Members (End)


    Public ReadOnly Property AddressComponent(ByVal lAddressComponent As Integer) As String
        Get

            Select Case lAddressComponent
                Case qafields_ORGANISATION
                    Return Organisation
                Case qafields_POBOX
                    Return POBox
                Case qafields_SUBPREMISES
                    Return SubPremises
                Case qafields_BUILDINGNAME
                    Return BuildingName
                Case qafields_BUILDINGNUMBER
                    Return BuildingNumber
                Case qafields_DEPTHORO
                    Return DependentThoroughfare
                Case qafields_THORO
                    Return Thoroughfare
                Case qafields_DEPLOCAL
                    Return DependentLocality
                Case qafields_LOCAL
                    Return Locality
                Case qafields_POSTTOWN
                    Return PostTown
                Case qafields_COUNTY
                    Return County
                Case qafields_POSTCODE
                    Return PostCode
                Case 12
                    Return (BuildingNumber & " " & Thoroughfare).Trim()
                Case qafields_SURNAME
                    Return Surname
                Case qafields_FORENAME
                    Return Forename
                Case Else
                    Return ""
            End Select

        End Get
    End Property


    ' PUBLIC Property Procedures (Begin)
    Public Property NodeID() As Integer
        Get

            Return m_lNodeID

        End Get
        Set(ByVal Value As Integer)

            m_lNodeID = Value

        End Set
    End Property

    Public Property NodeType() As String
        Get

            Return m_sNodeType

        End Get
        Set(ByVal Value As String)

            m_sNodeType = Value

        End Set
    End Property

    Public Property ParentNodeID() As Integer
        Get

            Return m_lParentNodeID

        End Get
        Set(ByVal Value As Integer)

            m_lParentNodeID = Value

        End Set
    End Property

    Public Property Level() As Integer
        Get

            Return m_iLevel

        End Get
        Set(ByVal Value As Integer)

            m_iLevel = Value

        End Set
    End Property

    Public Property Count() As Integer
        Get

            Return m_iCount

        End Get
        Set(ByVal Value As Integer)

            m_iCount = Value

        End Set
    End Property

    Public Property NoOfChildren() As Integer
        Get

            Return m_iNoOfChildren

        End Get
        Set(ByVal Value As Integer)

            m_iNoOfChildren = Value

        End Set
    End Property

    Public Property Caption() As String
        Get

            Return m_sCaption

        End Get
        Set(ByVal Value As String)

            m_sCaption = Value

        End Set
    End Property

    Public Property AddressLine1() As String
        Get

            Return m_oAddress.AddressLine1

        End Get
        Set(ByVal Value As String)

            m_oAddress.AddressLine1 = Value

        End Set
    End Property

    Public Property AddressLine2() As String
        Get

            Return m_oAddress.AddressLine2

        End Get
        Set(ByVal Value As String)

            m_oAddress.AddressLine2 = Value

        End Set
    End Property

    Public Property AddressLine3() As String
        Get

            Return m_oAddress.AddressLine3

        End Get
        Set(ByVal Value As String)

            m_oAddress.AddressLine3 = Value

        End Set
    End Property

    Public Property AddressLine4() As String
        Get

            Return m_oAddress.AddressLine4

        End Get
        Set(ByVal Value As String)

            m_oAddress.AddressLine4 = Value

        End Set
    End Property

    Public Property PostCode() As String
        Get

            Return m_oAddress.PostCode

        End Get
        Set(ByVal Value As String)

            m_oAddress.PostCode = Value

        End Set
    End Property

    Public Property Organisation() As String
        Get

            Return m_oAddress.Organisation

        End Get
        Set(ByVal Value As String)

            m_oAddress.Organisation = Value

        End Set
    End Property

    Public Property POBox() As String
        Get

            Return m_oAddress.POBox

        End Get
        Set(ByVal Value As String)

            m_oAddress.POBox = Value

        End Set
    End Property

    Public Property SubPremises() As String
        Get

            Return m_oAddress.SubPremises

        End Get
        Set(ByVal Value As String)

            m_oAddress.SubPremises = Value

        End Set
    End Property

    Public Property BuildingName() As String
        Get

            Return m_oAddress.BuildingName

        End Get
        Set(ByVal Value As String)

            m_oAddress.BuildingName = Value

        End Set
    End Property

    Public Property BuildingNumber() As String
        Get

            Return m_oAddress.BuildingNumber

        End Get
        Set(ByVal Value As String)

            m_oAddress.BuildingNumber = Value

        End Set
    End Property

    Public Property DependentThoroughfare() As String
        Get

            Return m_oAddress.DependentThoroughfare

        End Get
        Set(ByVal Value As String)

            m_oAddress.DependentThoroughfare = Value

        End Set
    End Property

    Public Property Thoroughfare() As String
        Get

            Return m_oAddress.Thoroughfare

        End Get
        Set(ByVal Value As String)

            m_oAddress.Thoroughfare = Value

        End Set
    End Property

    Public Property DependentLocality() As String
        Get

            Return m_oAddress.DependentLocality

        End Get
        Set(ByVal Value As String)

            m_oAddress.DependentLocality = Value

        End Set
    End Property

    Public Property Locality() As String
        Get

            Return m_oAddress.Locality

        End Get
        Set(ByVal Value As String)

            m_oAddress.Locality = Value

        End Set
    End Property

    Public Property PostTown() As String
        Get

            Return m_oAddress.PostTown

        End Get
        Set(ByVal Value As String)

            m_oAddress.PostTown = Value

        End Set
    End Property

    Public Property County() As String
        Get

            Return m_oAddress.County

        End Get
        Set(ByVal Value As String)

            m_oAddress.County = Value

        End Set
    End Property

    'DAK140700
    Public Property CountryId() As Integer
        Get
            Return m_oAddress.CountryId
        End Get
        Set(ByVal Value As Integer)
            m_oAddress.CountryId = Value
        End Set
    End Property

    ' RDC 31072002 QAS Names additions START
    Public Property Forename() As String
        Get
            Return m_oAddress.Forename
        End Get
        Set(ByVal Value As String)
            m_oAddress.Forename = Value
        End Set
    End Property

    Public Property Surname() As String
        Get
            Return m_oAddress.Surname
        End Get
        Set(ByVal Value As String)
            m_oAddress.Surname = Value
        End Set
    End Property

    Public Property Title() As String
        Get
            Return m_oAddress.Title
        End Get
        Set(ByVal Value As String)
            m_oAddress.Title = Value
        End Set
    End Property


    Public Property Initial() As String
        Get
            Return m_oAddress.Initial
        End Get
        Set(ByVal Value As String)
            m_oAddress.Initial = Value
        End Set
    End Property
    ' RDC 31072002 QAS Names additions END

    Public Property PMAddressCnt() As Integer
        Get

            Return m_oAddress.PMAddressCnt

        End Get
        Set(ByVal Value As Integer)

            m_oAddress.PMAddressCnt = Value

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            m_oAddress = New bPMAddressControl.Address()

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
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
                m_oAddress = Nothing
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
        ' Error.
        '
        ' Log Error Message
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
