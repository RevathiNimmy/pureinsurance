<Serializable()> Public Class Lifestyle
    Private sKey As String
    Private iLifestyleKey As Integer
    Private sName As String
    Private dtDateOfBirth As String
    Private bDateOfBirthSpecified As Boolean
    Private sCategoryCode As String
    Private sOccupationCode As String
    Private sSecOccupationCode As String
    Private bSmoker As Boolean
    Private oGenderCode As String
    Private bGenderCodeSpecified As Boolean

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()
        bSmoker = False

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("LifeStyleKey : " & iLifestyleKey & "<br />")
        sbPrint.AppendLine("Name : " & sName & "<br />")
        sbPrint.AppendLine("DateofBirth : " & dtDateOfBirth & "<br />")
        sbPrint.AppendLine("CategoryCode  : " & sCategoryCode & "<br />")
        sbPrint.AppendLine("OccupationCode : " & sOccupationCode & "<br />")
        sbPrint.AppendLine("SecOccupationCode : " & sSecOccupationCode & "<br />")
        sbPrint.AppendLine("Smoker : " & bSmoker & "<br />")
        sbPrint.AppendLine("GenderCode : " & oGenderCode.ToString() & "<br />")


        Return sbPrint.ToString

    End Function

    Public Property Key() As String
        Get
            Return Me.sKey
        End Get
        Set(ByVal value As String)
            sKey = value
        End Set
    End Property
    Public Property LifestyleKey() As Integer
        Get
            Return iLifestyleKey
        End Get
        Set(ByVal value As Integer)
            iLifestyleKey = value
        End Set
    End Property
    Public Property Name() As String
        Get
            Return sName
        End Get
        Set(ByVal value As String)
            sName = value
        End Set
    End Property
    Public Property DateOfBirth() As String
        Get
            Return dtDateOfBirth
        End Get
        Set(ByVal value As String)
            dtDateOfBirth = value
        End Set
    End Property
    Public Property DateOfBirthSpecified() As Boolean
        Get
            Return bDateOfBirthSpecified
        End Get
        Set(ByVal value As Boolean)
            bDateOfBirthSpecified = value
        End Set
    End Property
    Public Property CategoryCode() As String
        Get
            Return sCategoryCode
        End Get
        Set(ByVal value As String)
            sCategoryCode = value
        End Set
    End Property
    Public Property OccupationCode() As String
        Get
            Return sOccupationCode
        End Get
        Set(ByVal value As String)
            sOccupationCode = value
        End Set
    End Property
    Public Property SecOccupationCode() As String
        Get
            Return sSecOccupationCode
        End Get
        Set(ByVal value As String)
            sSecOccupationCode = value
        End Set
    End Property
    Public Property Smoker() As Boolean
        Get
            Return bSmoker
        End Get
        Set(ByVal value As Boolean)
            bSmoker = value
        End Set
    End Property
    Public Property GenderCode() As String
        Get
            Return oGenderCode
        End Get
        Set(ByVal value As String)
            oGenderCode = value
        End Set
    End Property
    Public Property GenderCodeSpecified() As Boolean
        Get
            Return bGenderCodeSpecified
        End Get
        Set(ByVal value As Boolean)
            bGenderCodeSpecified = value
        End Set
    End Property

    ''' <summary>
    ''' Gender
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Public Enum GenderCodeType

        ''' <summary>
        ''' Female
        ''' </summary>
        ''' <remarks>Code F in BackOffice</remarks>
        Female = 0

        ''' <summary>
        ''' Male
        ''' </summary>
        ''' <remarks>Code M in BackOffice</remarks>
        Male = 1
    End Enum


End Class


<Serializable()> Public Class LifestyleCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface to the object
    ''' </summary>
    ''' <returns>An HTML string containining data held within the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oLifestyle As Lifestyle In List
            sbPrint.AppendLine(oLifestyle.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add an Lifestyle object to the collection
    ''' </summary>
    ''' <param name="v_oLifestyle">The Lifestyle object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oLifestyle As Lifestyle) As Integer
        v_oLifestyle.Key = List.Add(v_oLifestyle)
        Return v_oLifestyle.Key
    End Function

    ''' <summary>
    ''' Remove an Lifestyle object from the collection
    ''' </summary>
    ''' <param name="v_oLifestyle">The Lifestyle object to be removed</param>
    Public Sub Remove(ByVal v_oLifestyle As Lifestyle)
        List.Remove(v_oLifestyle)
    End Sub

    ''' <summary>
    ''' Remove an Lifestyle object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Lifestyle object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Lifestyle object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Lifestyle object</param>
    ''' <value>The replacement Lifestyle object</value>
    ''' <returns>The Lifestyle object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As Lifestyle
        Get
            Return List(i)
        End Get
        Set(ByVal value As Lifestyle)
            List(i) = value
        End Set
    End Property

    Public Sub Update(ByVal v_oLifestyle As Lifestyle)
        List.Item(v_oLifestyle.Key) = v_oLifestyle
    End Sub

    Public Sub Update(ByVal v_oLifestyle As Lifestyle, ByVal index As Integer)
        List.Item(index) = v_oLifestyle
    End Sub

    '''' <summary>
    '''' Return the first Lifestyle object in the collection with the specified LifestyleType
    '''' </summary>
    '''' <param name="v_oLifestyleType">The LifestyleType of the Lifestyle object to be returned</param>
    '''' <value>The LifestyleType the Lifestyle is to be retrieved by</value>
    '''' <returns>Matching Lifestyle object, if any</returns>
    'Default Public ReadOnly Property Item(ByVal v_oLifestyleType As LifestyleType) As Lifestyle
    '    Get
    '        Return FindItemByLifestyleType(v_oLifestyleType)
    '    End Get
    'End Property

    '''' <summary>
    '''' Find the first Lifestyle object in the collection with the specified LifestyleType
    '''' </summary>
    '''' <param name="v_oLifestyleType">The LifestyleType of the Lifestyle object to be returned</param>
    '''' <returns>The matching Lifestyle object, if any</returns>
    'Public Function FindItemByLifestyleType(ByVal v_oLifestyleType As LifestyleType) As Lifestyle

    '    For Each oLifestyle As Lifestyle In List
    '        If oLifestyle.LifestyleType = v_oLifestyleType Then
    '            Return oLifestyle
    '        End If
    '    Next

    '    Return Nothing

    'End Function

End Class