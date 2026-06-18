Imports System.Configuration

Namespace Config

    Public Class CoverDate : Inherits ConfigurationElement

        Private cpStartDate As ConfigurationProperty
        Private cpTimeScale As ConfigurationProperty
        Private cpPeriod As ConfigurationProperty
        Private cpMidnightRenewal As ConfigurationProperty
        Private cpTrueMonthlyPolicy As ConfigurationProperty

        Private cpcProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpStartDate = New ConfigurationProperty("StartDate", GetType(StartDate), StartDate.Today)
            cpTimeScale = New ConfigurationProperty("TimeScale", GetType(TimeScale), TimeScale.Year)
            cpPeriod = New ConfigurationProperty("Period", GetType(Integer), 1)
            cpMidnightRenewal = New ConfigurationProperty("MidnightRenewal", GetType(String), String.Empty)
            cpTrueMonthlyPolicy = New ConfigurationProperty("TrueMonthlyPolicy", GetType(Boolean), False)

            cpcProperties.Add(cpStartDate)
            cpcProperties.Add(cpTimeScale)
            cpcProperties.Add(cpPeriod)
            cpcProperties.Add(cpMidnightRenewal)
            cpcProperties.Add(cpTrueMonthlyPolicy)

        End Sub

        Public ReadOnly Property StartDate() As StartDate
            Get
                Return CStr(MyBase.Item(cpStartDate))
            End Get
        End Property

        Public ReadOnly Property TimeScale() As TimeScale
            Get
                Return CStr(MyBase.Item(cpTimeScale))
            End Get
        End Property

        Public ReadOnly Property Period() As Integer
            Get
                Return CInt(MyBase.Item(cpPeriod))
            End Get
        End Property

        Public ReadOnly Property MidnightRenewal() As Boolean
            Get
                Return CStr(MyBase.Item(cpMidnightRenewal))
            End Get
        End Property

        Public ReadOnly Property TrueMonthlyPolicy() As Boolean
            Get
                Return CStr(MyBase.Item(cpTrueMonthlyPolicy))
            End Get
        End Property

        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

    End Class

End Namespace