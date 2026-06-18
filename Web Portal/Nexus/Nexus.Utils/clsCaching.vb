
Public Class Cache

    Public Enum CacheLengthTypes
        CacheShort = 0
        CacheMedium = 1
        CacheLong = 2
    End Enum

    Public Shared Function CacheExpiration(ByVal CacheLength As CacheLengthTypes, _
                            Optional ByVal LiveDate As DateTime = Nothing, _
                            Optional ByVal ExpiryDate As DateTime = Nothing) As Date

        'Create a cache expiration date from current date/time plus the cache
        'interval from the web.config, 3 options, short, medium and long

        Dim dExpirationDate As Date

        Select Case CacheLength
            Case CacheLengthTypes.CacheShort
                dExpirationDate = Now.AddMinutes(funcUtils.GetRootConfigElement("ShortCacheExpire"))
            Case CacheLengthTypes.CacheMedium
                dExpirationDate = Now.AddMinutes(funcUtils.GetRootConfigElement("MediumCacheExpire"))
            Case CacheLengthTypes.CacheLong
                dExpirationDate = Now.AddMinutes(funcUtils.GetRootConfigElement("LongCacheExpire"))
        End Select

        'Use LiveDate as cache expire time, if it will occur before the standard cache timeout
        If LiveDate <> DateTime.MinValue Then
            If LiveDate < dExpirationDate And LiveDate > Now Then
                dExpirationDate = LiveDate
            End If
        End If

        'Use the ExpiryDate as cache expire, if it will occur before the standard cache timeout,
        'seems odd to check this as expire will always be after live, but the navigation cache
        'will pass to values that may be from different sitemap nodes

        If ExpiryDate <> DateTime.MinValue Then
            If ExpiryDate < dExpirationDate And ExpiryDate > Now Then
                dExpirationDate = ExpiryDate
            End If
        End If

        'The longest time that will be returned here is no more the CacheLength value
        Return dExpirationDate

    End Function


End Class

