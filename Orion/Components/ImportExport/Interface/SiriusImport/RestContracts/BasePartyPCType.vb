Public Class BasePartyPCType
    Inherits BasePartyType
    Public Property Surname As String
    Public Property Forename As String
    Public Property DateOfBirth As Date
    Public Property DateOfBirthSpecified As Boolean
    Public Property Title As String
    Public Property MaritalStatusCode As MaritalStatusCodeType
    Public Property MaritalStatusCodeSpecified As Boolean
    Public Property GenderCode As String
    Public Property Initials As String
    Public Property OccupationCode As String
    Public Property EmployersBusinessCode As String
    Public Property EmploymentStatusCode As EmploymentStatusCodeType
    Public Property EmploymentStatusCodeSpecified As Boolean
    Public Property AlternativeId As String
    Public Property ClientDetail As BaseClientSharedDataType
    Public Property NationalityCode As String
    Public Property Salutation As String
    Public Property TradingName As String
    Public Property SecOccupationCode As String
    Public Property SecEmployersBusinessCode As String
    Public Property SecEmploymentStatusCode As EmploymentStatusCodeType
    Public Property SecEmploymentStatusCodeSpecified As Boolean
    Public Property AccommodationCode As String
    Public Property Lifestyle As List(Of BasePartyPCTypeLifestyle)
    Public Property TPS As Boolean
    Public Property TPSSpecified As Boolean
    Public Property MPS As Boolean
    Public Property MPSSpecified As Boolean
    Public Property EMPS As Boolean
    Public Property EMPSSpecified As Boolean
    Public Property Source As String
    Public Property PetOwner As Boolean
    Public Property PetOwnerSpecified As Boolean
End Class
