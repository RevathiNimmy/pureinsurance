''' <summary>
''' The security test that you want to perform.
''' </summary>
Public Enum WSETestCaseScenario
    ''' <summary>
    ''' No security-related test is being performed.
    ''' </summary>
    None
    ''' <summary>
    ''' Test what happens when security is not set on the client.
    ''' </summary>
    MissingSecurity
    ''' <summary>
    ''' Test what happens when an invalid user name is supplied.
    ''' </summary>
    InvalidUserName
    ''' <summary>
    ''' Test what happens when a valid user name but an invalid password is supplied.
    ''' </summary>
    InvalidPassword
    ''' <summary>
    ''' Test what happens when a valid user name and password is supplied, but it does not have sufficient permissions.
    ''' </summary>
    InvalidTaskCode
End Enum
