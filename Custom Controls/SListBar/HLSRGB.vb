Imports System.Drawing

Namespace Drawing
    ''' <summary>
    ''' Provides a helper class allowing bi-directional conversion
    ''' between RGB and HLS (Hue, Luminance, Saturation) colour
    ''' models.  The HLS model is helpful if you want to produce
    ''' darker or ligher shades of colours (by varying the Luminance)
    ''' or if you want to produce a brigher or more washed out 
    ''' variant (by changing the Saturation).  These operations are
    ''' typically very difficult to do in the RGB space.
    ''' </summary>
    Public Class HLSRGB
        Private m_red As Byte = 0
        Private m_green As Byte = 0
        Private m_blue As Byte = 0

        Private m_hue As Single = 0
        Private m_luminance As Single = 0
        Private m_saturation As Single = 0

        ''' <summary>
        ''' Gets or sets the Red component of the colour.
        ''' </summary>
        Public Property Red() As Byte
            Get
                Return m_red
            End Get
            Set(value As Byte)
                m_red = value
                ToHLS()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the Green component of the colour.
        ''' </summary>
        Public Property Green() As Byte
            Get
                Return m_green
            End Get
            Set(value As Byte)
                m_green = value
                ToHLS()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the Blue component of the colour.
        ''' </summary>
        Public Property Blue() As Byte
            Get
                Return m_blue
            End Get
            Set(value As Byte)
                m_blue = value
                ToHLS()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the Luminance (0.0 to 1.0) of the colour.
        ''' </summary>
        ''' <exception cref="ArgumentOutOfRangeException">If the value is out of the acceptable range for luminance (0.0 to 1.0)</exception>
        Public Property Luminance() As Single
            Get
                Return m_luminance
            End Get
            Set(value As Single)
                If (value < 0.0F) OrElse (value > 1.0F) Then
                    Throw New ArgumentOutOfRangeException("Luminance", "Luminance must be between 0.0 and 1.0")
                End If
                m_luminance = value
                ToRGB()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the Hue (0.0 to 360.0) of the colour.
        ''' </summary>
        ''' <exception cref="ArgumentOutOfRangeException">If the value is out of the acceptable range for hue (0.0 to 360.0)</exception>
        Public Property Hue() As Single
            Get
                Return m_hue
            End Get
            Set(value As Single)
                If (value < 0.0F) OrElse (value > 360.0F) Then
                    Throw New ArgumentOutOfRangeException("Hue", "Hue must be between 0.0 and 360.0")
                End If
                m_hue = value
                ToRGB()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the Saturation (0.0 to 1.0) of the colour.
        ''' </summary>
        ''' <exception cref="ArgumentOutOfRangeException">If the value is out of the acceptable range for saturation (0.0 to 1.0)</exception>
        Public Property Saturation() As Single
            Get
                Return m_saturation
            End Get
            Set(value As Single)
                If (value < 0.0F) OrElse (value > 1.0F) Then
                    Throw New ArgumentOutOfRangeException("Saturation", "Saturation must be between 0.0 and 1.0")
                End If
                m_saturation = value
                ToRGB()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the Color as a System.Drawing.Color instance
        ''' </summary>
        Public Property Color() As Color
            Get
                Dim c As Color = Color.FromArgb(m_red, m_green, m_blue)
                Return c
            End Get
            Set(value As Color)
                m_red = value.R
                m_green = value.G
                m_blue = value.B
                ToHLS()
            End Set
        End Property

        ''' <summary>
        ''' Lightens the colour by the specified amount by modifying
        ''' the luminance (for example, 0.2 would lighten the colour by 20%)
        ''' </summary>
        Public Sub LightenColor(lightenBy As Single)
            m_luminance *= (1.0F + lightenBy)
            If m_luminance > 1.0F Then
                m_luminance = 1.0F
            End If
            ToRGB()
        End Sub

        ''' <summary>
        ''' Darkens the colour by the specified amount by modifying
        ''' the luminance (for example, 0.2 would darken the colour by 20%)
        ''' </summary>
        Public Sub DarkenColor(darkenBy As Single)
            m_luminance *= darkenBy
            ToRGB()
        End Sub


        ''' <summary>
        ''' Constructs an instance of the class from the specified
        ''' System.Drawing.Color
        ''' </summary>
        ''' <param name="c">The System.Drawing.Color to use to initialise the class</param>
        Public Sub New(c As Color)
            m_red = c.R
            m_green = c.G
            m_blue = c.B
            ToHLS()
        End Sub

        ''' <summary>
        ''' Constructs an instance of the class with the specified hue, luminance and saturation values.
        ''' </summary>
        ''' <param name="hue">The Hue (between 0.0 and 360.0)</param>
        ''' <param name="luminance">The Luminance (between 0.0 and 1.0)</param>
        ''' <param name="saturation">The Saturation (between 0.0 and 1.0)</param>
        ''' <exception cref="ArgumentOutOfRangeException">If any of the H,L,S values are out of the acceptable range (0.0-360.0 for Hue and 0.0-1.0 for Luminance and Saturation)</exception>
        Public Sub New(hue As Single, luminance As Single, saturation As Single)
            If (saturation < 0.0F) OrElse (saturation > 1.0F) Then
                Throw New ArgumentOutOfRangeException("Saturation", "Saturation must be between 0.0 and 1.0")
            End If
            If (hue < 0.0F) OrElse (hue > 360.0F) Then
                Throw New ArgumentOutOfRangeException("Hue", "Hue must be between 0.0 and 360.0")
            End If
            If (luminance < 0.0F) OrElse (luminance > 1.0F) Then
                Throw New ArgumentOutOfRangeException("Luminance", "Luminance must be between 0.0 and 1.0")
            End If
            Me.m_hue = hue
            Me.m_luminance = luminance
            Me.m_saturation = saturation
            ToRGB()
        End Sub

        ''' <summary>
        ''' Constructs an instance of the class with the specified red, green and blue values.
        ''' </summary>
        ''' <param name="red">The red component.</param>
        ''' <param name="green">The green component.</param>
        ''' <param name="blue">The blue component.</param>
        Public Sub New(red As Byte, green As Byte, blue As Byte)
            Me.m_red = red
            Me.m_green = green
            Me.m_blue = blue
            ToHLS()
        End Sub

        ''' <summary>
        ''' Constructs an instance of the class using the settings of another instance.
        ''' </summary>
        ''' <param name="hlsrgb__1">The instance to clone.</param>
        ''' <remarks></remarks>
        Public Sub New(hlsrgb__1 As HLSRGB)
            Me.m_red = hlsrgb__1.Red
            Me.m_blue = hlsrgb__1.Blue
            Me.m_green = hlsrgb__1.Green
            Me.m_luminance = hlsrgb__1.Luminance
            Me.m_hue = hlsrgb__1.Hue
            Me.m_saturation = hlsrgb__1.Saturation
        End Sub

        ''' <summary>
        ''' Constructs a new instance of the class initialised to black.
        ''' </summary>
        Public Sub New()
        End Sub

        Private Sub ToHLS()
            Dim minval As Byte = Math.Min(m_red, Math.Min(m_green, m_blue))
            Dim maxval As Byte = Math.Max(m_red, Math.Max(m_green, m_blue))

            Dim mdiff As Single = CSng(maxval - minval)
            Dim msum As Single = CSng(maxval + minval)

            m_luminance = msum / 510.0F

            If maxval = minval Then
                m_saturation = 0.0F
                m_hue = 0.0F
            Else
                Dim rnorm As Single = (maxval - m_red) / mdiff
                Dim gnorm As Single = (maxval - m_green) / mdiff
                Dim bnorm As Single = (maxval - m_blue) / mdiff

                m_saturation = If((m_luminance <= 0.5F), (mdiff / msum), (mdiff / (510.0F - msum)))

                If m_red = maxval Then
                    m_hue = 60.0F * (6.0F + bnorm - gnorm)
                End If
                If m_green = maxval Then
                    m_hue = 60.0F * (2.0F + rnorm - bnorm)
                End If
                If m_blue = maxval Then
                    m_hue = 60.0F * (4.0F + gnorm - rnorm)
                End If
                If m_hue > 360.0F Then
                    m_hue = m_hue - 360.0F
                End If
            End If
        End Sub

        Private Sub ToRGB()
            If m_saturation = 0.0 Then
                m_red = CByte(Math.Truncate(m_luminance * 255.0F))
                m_green = m_red
                m_blue = m_red
            Else
                Dim rm1 As Single
                Dim rm2 As Single

                If m_luminance <= 0.5F Then
                    rm2 = m_luminance + m_luminance * m_saturation
                Else
                    rm2 = m_luminance + m_saturation - m_luminance * m_saturation
                End If
                rm1 = 2.0F * m_luminance - rm2
                m_red = ToRGB1(rm1, rm2, m_hue + 120.0F)
                m_green = ToRGB1(rm1, rm2, m_hue)
                m_blue = ToRGB1(rm1, rm2, m_hue - 120.0F)
            End If
        End Sub

        Private Function ToRGB1(rm1 As Single, rm2 As Single, rh As Single) As Byte
            If rh > 360.0F Then
                rh -= 360.0F
            ElseIf rh < 0.0F Then
                rh += 360.0F
            End If

            If rh < 60.0F Then
                rm1 = rm1 + (rm2 - rm1) * rh / 60.0F
            ElseIf rh < 180.0F Then
                rm1 = rm2
            ElseIf rh < 240.0F Then
                rm1 = rm1 + (rm2 - rm1) * (240.0F - rh) / 60.0F
            End If

            Return CByte(Math.Truncate(rm1 * 255))
        End Function

    End Class
End Namespace
