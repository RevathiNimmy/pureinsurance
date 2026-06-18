Option Explicit On
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.Runtime.Serialization
Imports System.Diagnostics
Imports SharedFiles
Imports SListBar.Drawing

Namespace ListBarControl

#Region "Enumerations"
    ''' <summary>
    ''' Enumeration specifying the view to use for the items within
    ''' a <see cref="ListBarGroup"/>.
    ''' </summary>
    <Description("Enumeration specifying the view to use for the items within a group.")> _
    Public Enum ListBarGroupView
        ''' <summary>
        ''' The ListBar will display using large icons, with the text underneath.
        ''' </summary>
        <Description("The ListBar will display using large icons, with the text underneath.")> _
        LargeIcons
        ''' <summary>
        ''' The ListBar will display using small icons, with text to the left.
        ''' </summary>
        <Description("The ListBar will display using small icons, with text to the left.")> _
        SmallIcons
        ''' <summary>
        ''' The ListBar will display using large icons with no text.
        ''' </summary>
        <Description("The ListBar will display large icons with no text.")> _
        LargeIconsOnly
        ''' <summary>
        ''' The ListBar will display using small icons with no text.
        ''' </summary>
        <Description("The ListBar will display small icons with no text.")> _
        SmallIconsOnly
    End Enum

    ''' <summary>
    ''' Enumeration specifying how the <see cref="ListBar"/> control will draw.
    ''' </summary>
    <Description("Enumeration specifying the ListBar control drawing style.")> _
    Public Enum ListBarDrawStyle
        ''' <summary>
        ''' The ListBar will draw using the style of the original Office
        ''' releases.
        ''' </summary>
        <Description("The ListBar will draw using the style of the original Office releases.")> _
        ListBarDrawStyleNormal
        ''' <summary>
        ''' The ListBar will draw using the Office XP style.
        ''' </summary>
        <Description("The ListBar will draw using the Office XP style.")> _
        ListBarDrawStyleOfficeXP
        ''' <summary>
        ''' The ListBar will draw using the Office 2003 style
        ''' (not implemented yet).
        ''' </summary>
        <Description("The ListBar will draw using the Office 2003 style (not implemented yet).")> _
        ListBarDrawStyleOffice2003
    End Enum
#End Region

#Region "Event argument classes"
    ''' <summary>
    ''' Provides details about an item which will undergo
    ''' an edit operation.
    ''' </summary>
    Public Class ListBarLabelEditEventArgs
        Inherits LabelEditEventArgs
        Private m_labelEditObject As Object = Nothing






        ''' <summary>
        ''' Returns the object for which label editing has
        ''' been requested.  Can either be a <see cref="ListBarItem"/> or
        ''' a <see cref="ListBarGroup"/> (or a subclass of either).
        ''' </summary>
        <Description("Gets the object for which label editing has been requested.  Either a ListBarItem or a ListBarGroup (or a subclass)")> _
        Public ReadOnly Property LabelEditObject() As Object
            Get
                Return Me.m_labelEditObject
            End Get
        End Property

        ''' <summary>
        ''' Constructs a new instance of this object
        ''' given the item, label and object.
        ''' </summary>
        ''' <param name="item">The index of the item being edited.</param>
        ''' <param name="label">The label of the item being edited.</param>
        ''' <param name="labelEditObject">The object being edited.</param>
        <Description("Constructs a new instance of this object.")> _
        Public Sub New(item As Integer, label As String, labelEditObject As Object)
            MyBase.New(item, label)
            Me.m_labelEditObject = labelEditObject
        End Sub
    End Class

    ''' <summary>
    ''' Provides event arguments for the BeforeSelectedGroupChanged event
    ''' raised by the control.  This object contains the group that
    ''' would be selected and provides the opportunity to cancel the 
    ''' group selection.
    ''' </summary>
    Public Class BeforeGroupChangedEventArgs
        Inherits EventArgs
        ''' <summary>
        ''' The ListBarGroup that would be selected.
        ''' </summary>
        Private m_group As ListBarGroup
        ''' <summary>
        ''' Whether to cancel the operation or not.
        ''' </summary>
        Private m_cancel As Boolean = False

        ''' <summary>
        ''' Gets the group that will be selected.
        ''' </summary>
        <Description("Gets the group that will be selected.")> _
        Public ReadOnly Property Group() As ListBarGroup
            Get
                Return Me.m_group
            End Get
        End Property

        ''' <summary>
        ''' Gets/sets whether the group selection should be cancelled
        ''' or not. By default the group selection is not cancelled.
        ''' </summary>
        <Description("Gets/sets whether the group selection should be cancelled.")> _
        Public Property Cancel() As Boolean
            Get
                Return Me.m_cancel
            End Get
            Set(value As Boolean)
                Me.m_cancel = value
            End Set
        End Property

        ''' <summary>
        ''' Constructs a new instance of this object.
        ''' Called
        ''' by the <see cref="ListBar"/> control before firing a 
        ''' <c>BeforeSelectedGroupChanged</c> event.
        ''' </summary>
        ''' <param name="group">The group that will be selected</param>
        <Description("Constructs a new instance of this object.")> _
        Public Sub New(group As ListBarGroup)
            Me.m_group = group
        End Sub
    End Class

    ''' <summary>
    ''' This class is used with the BeforeItemClicked event and provides
    ''' the item which is about to be clicked and the option to prevent
    ''' the item being clicked by setting the Cancel property.
    ''' </summary>
    Public Class BeforeItemClickedEventArgs
        Inherits EventArgs
        ''' <summary>
        ''' The ListBarItem which is about to be clicked.
        ''' </summary>
        Private m_item As ListBarItem = Nothing
        ''' <summary>
        ''' Whether the click should be cancelled or not.
        ''' </summary>
        Private m_cancel As Boolean = False

        ''' <summary>
        ''' Gets/sets whether the click should be cancelled or not.
        ''' </summary>
        <Description("Gets/sets whether the click should be cancelled or not.")> _
        Public Property Cancel() As Boolean
            Get
                Return Me.m_cancel
            End Get
            Set(value As Boolean)
                Me.m_cancel = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the ListBarItem that is about to be clicked.
        ''' </summary>
        <Description("Gets the ListBarItem that is about to be clicked.")> _
        Public ReadOnly Property Item() As ListBarItem
            Get
                Return Me.m_item
            End Get
        End Property


        ''' <summary>
        ''' Constructor for this object. Called
        ''' by the <see cref="ListBar"/> control before firing a 
        ''' <see cref="BeforeItemClickedEventHandler"/> event.
        ''' </summary>
        ''' <param name="item">The item that's about to be clicked.</param>
        <Description("Constructs a new instance of this object.")> _
        Public Sub New(item As ListBarItem)
            Me.m_item = item
        End Sub

    End Class

    ''' <summary>
    ''' This class is provides details of which item has been clicked
    ''' and the mouse details of the click when the <c>ItemClicked</c> event
    ''' is raised from a <c>ListBar</c>.
    ''' <seealso cref="ListBar.ItemClicked"/>
    ''' </summary>
    Public Class ItemClickedEventArgs
        Inherits ObjectClickedEventArgs
        ''' <summary>
        ''' The ListBarIem that has been clicked.
        ''' </summary>
        Private m_item As ListBarItem = Nothing

        ''' <summary>
        ''' Gets the <see cref="ListBarItem"/> that has been clicked.
        ''' </summary>
        <Description("Gets the ListBarItem that has been clicked.")> _
        Public ReadOnly Property Item() As ListBarItem
            Get
                Return Me.m_item
            End Get
        End Property


        ''' <summary>
        ''' Constructs a new instance of this object.  Called by the <see cref="ListBar"/>
        ''' control when firing an <c>ItemClicked</c> event.
        ''' </summary>
        ''' <param name="item">The item that has been clicked</param>
        ''' <param name="location">The mouse location relative to the 
        ''' control for the click.</param>
        ''' <param name="mouseButton">The mouse button used to click
        ''' the item.</param>
        <Description("Constructs a new instance of this object")> _
        Public Sub New(item As ListBarItem, location As Point, mouseButton As MouseButtons)
            MyBase.New(location, mouseButton)
            Me.m_item = item
        End Sub

    End Class

    ''' <summary>
    ''' This class is provides details of which item has been clicked
    ''' and the mouse details of the click when the <c>GroupClicked</c> event
    ''' is raised from a <see cref="ListBar" /> control.
    ''' </summary>
    Public Class GroupClickedEventArgs
        Inherits ObjectClickedEventArgs
        ''' <summary>
        ''' The ListBarGroup that has been clicked.
        ''' </summary>
        Private m_group As ListBarGroup = Nothing

        ''' <summary>
        ''' Gets the <see cref="ListBarGroup"/> that has been clicked.
        ''' </summary>
        <Description("Gets the ListBarGroup that has been clicked.")> _
        Public ReadOnly Property Group() As ListBarGroup
            Get
                Return Me.m_group
            End Get
        End Property


        ''' <summary>
        ''' Constructs a new instance of this object.  Called by the <see cref="ListBar"/>
        ''' control when firing a <c>GroupClicked</c> event.
        ''' </summary>
        ''' <param name="group">The <see cref="ListBarGroup"/> that has been clicked</param>
        ''' <param name="location">The mouse location relative to the 
        ''' control for the click.</param>
        ''' <param name="mouseButton">The mouse button used to click
        ''' the item.</param>
        <Description("Constructs a new instance of this object.")> _
        Public Sub New(group As ListBarGroup, location As Point, mouseButton As MouseButtons)
            MyBase.New(location, mouseButton)
            Me.m_group = group
        End Sub

    End Class

    ''' <summary>
    ''' An abstract class used as the bases for the <c>ItemClicked</c>
    ''' and <c>GroupClicked</c> events of the <see cref="ListBar"/> control.
    ''' This class stores details of the mouse location and button.
    ''' </summary>
    Public MustInherit Class ObjectClickedEventArgs
        Inherits EventArgs
        ''' <summary>
        ''' The location of the mouse when the item was clicked.
        ''' </summary>
        Private m_location As Point
        ''' <summary>
        ''' The mouse button that was used.
        ''' </summary>
        Private m_mouseButton As MouseButtons = MouseButtons.Left

        ''' <summary>
        ''' The Location of the mouse, relative to the control,
        ''' when the item was clicked.
        ''' </summary>
        <Description("The location of the mouse relative to the control when the item was clicked.")> _
        Public ReadOnly Property Location() As Point
            Get
                Return m_location
            End Get
        End Property


        ''' <summary>
        ''' The MouseButton used to click the item.
        ''' </summary>
        <Description("The mouse button used to click this item.")> _
        Public ReadOnly Property MouseButton() As MouseButtons
            Get
                Return Me.m_mouseButton
            End Get
        End Property

        ''' <summary>
        ''' When used in a subclass, constructs a new instance of the class with the specified
        ''' mouse location and button.
        ''' </summary>
        ''' <param name="location">The location of the mouse.</param>
        ''' <param name="mouseButton">The button which was pressed.</param>
        <Description("When used in a subclass, constructs a new instance of this class.")> _
        Public Sub New(location As Point, mouseButton As MouseButtons)
            Me.m_location = location
            Me.m_mouseButton = mouseButton
        End Sub

    End Class
#End Region

#Region "Event delegates"
    ''' <summary>
    ''' Represents the method that handles the BeforeSelectedGroupChanged event
    ''' of a ListBar control.
    ''' </summary>
    Public Delegate Sub BeforeGroupChangedEventHandler(sender As Object, e As BeforeGroupChangedEventArgs)
    ''' <summary>
    ''' Represents the method that handles the BeforeItemClicked event
    ''' of a ListBar control.
    ''' </summary>
    Public Delegate Sub BeforeItemClickedEventHandler(sender As Object, e As BeforeItemClickedEventArgs)
    ''' <summary>
    ''' Represents the method that handles the ItemClicked event of a
    ''' ListBar control.
    ''' </summary>
    Public Delegate Sub ItemClickedEventHandler(sender As Object, e As ItemClickedEventArgs)
    ''' <summary>
    ''' Represents the method that handles the GroupClicked event of a
    ''' ListBar control.
    ''' </summary>
    Public Delegate Sub GroupClickedEventHandler(sender As Object, e As GroupClickedEventArgs)

    ''' <summary>
    ''' Represents the method that handles the BeforeLabelEdit and AfterLabelEdit
    ''' events of a ListBar control.
    ''' </summary>
    Public Delegate Sub ListBarLabelEditEventHandler(sender As Object, e As ListBarLabelEditEventArgs)
#End Region

#Region "ListBar Control class"
    ''' <summary>
    ''' An implementation of a Microsoft Outlook Style ListBar control.
    ''' The control provides all the features needed to implement a replica
    ''' of the Outlook style control and is also designed to allow the same
    ''' functionality to be used in overriden controls in which the
    ''' individual sizing and appearance of each of the UI components can be
    ''' customised.
    ''' 
    ''' The <c>ListBar</c> control is modelled as an extension to
    ''' the <c>System.Windows.Forms.UserControl</c> class.  Bars
    ''' are configured using <see cref="ListBarGroup" /> objects which are
    ''' collected in the <see cref="ListBarGroupCollection" /> object
    ''' accessible through the control's accessor.
    ''' Each <see cref="ListBarGroup" /> in turn contains a 
    ''' <see cref="ListBarItemCollection" /> of <see cref="ListBarItem" /> objects 
    ''' which represent the buttons within a group.
    ''' </summary>	
    '''
    ''' <remarks>
    ''' Copyright &#169; 2003 Steve McMahon for vbAccelerator.com.
    ''' vbAccelerator is a Trade Mark of vbAccelerator Ltd.  All Rights
    ''' Reserved.  Please visit http://vbaccelerator.com/ for more
    ''' on this and other VB and .NET Framework code.  Comments to
    ''' mailto:steve@vbaccelerator.com.
    ''' </remarks>
    ''' 
    Public Class ListBar
        Inherits System.Windows.Forms.UserControl

#Region "Member Variables"
        ''' <summary>
        ''' Reference to the collection of groups contained within the ListBar control.
        ''' </summary>
        Public m_groups As ListBarGroupCollection = Nothing
        ''' <summary>
        ''' Reference to an external ToolTip object.
        ''' </summary>
        Private m_toolTip As ToolTip = Nothing
        ''' <summary>
        ''' Reference to an external Image List for drawing the large icon view.
        ''' </summary>
        Private m_largeImageList As ImageList = Nothing
        ''' <summary>
        ''' Reference to an external Image List for drawing the small icon view.
        ''' </summary>
        Private m_smallImageList As ImageList = Nothing
        Private m_selectedGroup As ListBarGroup = Nothing
        ''' <summary>
        ''' A timer for controlling scrolling when the scroll buttons are held
        ''' down.
        ''' </summary>
        Private buttonPressed As New Timer()
        ''' <summary>
        ''' Contains a reference to the active scroll button when one is pressed
        ''' and the mouse is over it.
        ''' </summary>
        Private activeButton As ListBarScrollButton = Nothing
        ''' <summary>
        ''' The last time a scroll occurred during a drag-drop operation.  Used
        ''' to control the speed of scrolling during drag-drop.
        ''' </summary>
        Private lastScrollTime As DateTime = DateTime.Now
        ''' <summary>
        ''' Drawing style fo the control.
        ''' </summary>
        Private m_drawStyle As ListBarDrawStyle = ListBarDrawStyle.ListBarDrawStyleOfficeXP
        ''' <summary>
        ''' Last width the control was drawn at.  Used to control resizing.
        ''' </summary>
        Private lastWidth As Integer = 0
        ''' <summary>
        ''' Last height the control was drawn at.  Used to control resizing.
        ''' </summary>
        Private lastHeight As Integer = 0
        ''' <summary>
        ''' Flag to control whether redrawing occurs or not
        ''' during updating:
        ''' </summary>
        Private redraw As Boolean = True
        ''' <summary>
        ''' Up scroll button reference.
        ''' </summary>
        Protected btnUp As ListBarScrollButton
        ''' <summary>
        ''' Down scroll buttons reference.
        ''' </summary>
        Protected btnDown As ListBarScrollButton
        ''' <summary>
        ''' The rectangle containing the "ListView" portion of the control.
        ''' </summary>
        Private rcListView As Rectangle
        ''' <summary>
        ''' The object that the mouse is currently over, if any.
        ''' </summary>
        Private mouseTrack As IMouseObject = Nothing
        ''' <summary>
        ''' The object that the mouse is currently down on, if any.
        ''' </summary>
        Private mouseDown As IMouseObject = Nothing
        ''' <summary>
        ''' Whether items are selected on MouseDown or
        ''' MouseUp.
        ''' </summary>
        Private m_selectOnMouseDown As Boolean = False
        ''' <summary>
        ''' Whether items can be dragged or not
        ''' </summary>
        Private m_allowDragItems As Boolean = True
        ''' <summary>
        ''' Whether groups can be dragged or not
        ''' </summary>
        Private m_allowDragGroups As Boolean = True
        ''' <summary>
        ''' During drag-drop, the insert point, if any.
        ''' </summary>
        Private dragInsertPoint As ListBarDragDropInsertPoint = Nothing
        ''' <summary>
        ''' The object that was last hovered over during
        ''' drag-drop, if any:
        ''' </summary>
        Private dragHoverOver As IMouseObject = Nothing
        ''' <summary>
        ''' The time at which hovering started over the object
        ''' which is currently being hovered over:
        ''' </summary>
        Private dragHoverOverStartTime As DateTime = DateTime.Now
        ''' <summary>
        ''' The ListBarItem currently being edited, if any
        ''' </summary>
        Private m_editItem As ListBarItem = Nothing
        ''' <summary>
        ''' The ListBarGroup currently being edited, if any
        ''' </summary>
        Private editGroup As ListBarGroup = Nothing
        ''' <summary>
        ''' Are we scrolling a new group into view or not?
        ''' </summary>
        Private scrollingGroup As Boolean = False
        ''' <summary>
        ''' The index of the group which is currently selected
        ''' when scrolling a new group into view:
        ''' </summary>
        Protected indexCurrent As Integer = -1
        ''' <summary>
        ''' The index of the newly selected group which will replace
        ''' the selected index when scrolling a new group into view:
        ''' </summary>
        Protected indexNew As Integer = -1
        ''' <summary>
        ''' The Text Box used for editing an item's caption.
        ''' </summary>
        Private txtEdit As System.Windows.Forms.TextBox
        ''' <summary>
        ''' A class to determine when the TextBox used for
        ''' editing should be cancelled:
        ''' </summary>

        Private popupCancel As PopupCancelNotifier

        ''' <summary> 
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.Container = Nothing
        ''' <summary>
        ''' Added by Shipali
        ''' Reference to an favorite caption.
        ''' </summary>
        Private m_currentGroupCaption As String = String.Empty


#End Region

#Region "Events"
        ''' <summary>
        ''' Raised before the selected group in the ListBar control is changed. Allows
        ''' the group selection to be cancelled.
        ''' </summary>
        <Description("Raised before the selected group in the ListBar control is changed.")> _
        Public Event BeforeSelectedGroupChanged As BeforeGroupChangedEventHandler
        ''' <summary>
        ''' Raised when the selected group in a ListBar control has been
        ''' changed.
        ''' </summary>
        <Description("Raised once the selected group in the ListBar control has been changed.")> _
        Public Event SelectedGroupChanged As System.EventHandler
        ''' <summary>
        ''' Raised before an item in a ListBar control is clicked.  Allows
        ''' the item selection to be cancelled.
        ''' </summary>
        <Description("Raised before an item in the ListBar control is clicked.")> _
        Public Event BeforeItemClicked As BeforeItemClickedEventHandler
        ''' <summary>
        ''' Raised when an item has been clicked in the ListBar control.
        ''' </summary>
        <Description("Raised once an item in the ListBar control has been clicked.")> _
        Public Event ItemClicked As ItemClickedEventHandler
        ''' <summary>
        ''' Raised when an item has been double clicked in the ListBar control.
        ''' </summary>
        <Description("Raised when an item has been double clicked in the ListBar control.")> _
        Public Event ItemDoubleClicked As ItemClickedEventHandler
        ''' <summary>
        ''' Raised when a group has been clicked in the ListBar control.
        ''' </summary>
        <Description("Raised when a group has been clicked in the ListBar control.")> _
        Public Event GroupClicked As GroupClickedEventHandler
        ''' <summary>
        ''' Raised before an item's label is about to be edited in the ListBar
        ''' control.  Allows the label edit to be cancelled.
        ''' </summary>
        <Description("Raised before an item's label is about to be edited in the ListBar control.")> _
        Public Event BeforeLabelEdit As ListBarLabelEditEventHandler
        ''' <summary>
        ''' Raised after an item's label has been edited in the ListBar control.
        ''' Allows the new caption to be checked and the edit cancelled.
        ''' </summary>
        <Description("Raised after an item's label has been edited but before the change is committed.")> _
        Public Event AfterLabelEdit As ListBarLabelEditEventHandler
#End Region

#Region "Constructor and Dispose/Finalise"
        ''' <summary>
        ''' Creates a new instance of a ListBar control.
        ''' </summary>
        Public Sub New()
            ' This call is required by the Windows.Forms Form Designer.
            InitializeComponent()

            ' Set up the control:
            Me.SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.DoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)

            ' Initialisation:
            m_groups = CreateListBarGroupCollection()
            btnUp = CreateListBarScrollButton(ListBarScrollButton.ListBarScrollButtonType.Up)
            btnDown = CreateListBarScrollButton(ListBarScrollButton.ListBarScrollButtonType.Down)

            ' Scroll timer:
            buttonPressed.Interval = 350
            buttonPressed.Enabled = False
            AddHandler buttonPressed.Tick, New EventHandler(AddressOf buttonPressed_Tick)

            ' Text box:
            AddHandler txtEdit.KeyDown, New KeyEventHandler(AddressOf txtEdit_KeyDown)

            popupCancel = New PopupCancelNotifier()

            AddHandler popupCancel.PopupCancel, New PopupCancelEventHandler(AddressOf popupCancel_PopupCancel)


        End Sub


        Private _SelectedRightClickItemCaption As String
        Public Property SelectedRightClickItemCaption() As String
            Get
                Return _SelectedRightClickItemCaption
            End Get
            Set(ByVal value As String)
                _SelectedRightClickItemCaption = value
            End Set
        End Property


        ''' <summary> 
        ''' Clean up any resources being used.
        ''' </summary>
        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                If components IsNot Nothing Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub
#End Region

#Region "Responding to events"
        ''' <summary>
        ''' Controls scrolling when the mouse is over and down on a scroll
        ''' bar button.
        ''' </summary>
        ''' <param name="sender">The object which raised this event.</param>
        ''' <param name="e">Arguments associated with this event.</param>
        Private Sub buttonPressed_Tick(sender As Object, e As System.EventArgs)
            ' check if the mouse is still over a scroll button
            ' that's been pressed:
            If activeButton IsNot Nothing Then
                ' shorten the interval for the next scroll down
                ' to 75ms:
                buttonPressed.Interval = 75
                ' Check if mouse in button:
                Dim pos As Point = Cursor.Position
                pos = Me.PointToClient(pos)
                If activeButton.HitTest(pos) Then
                    ' perform the scrolling:
                    Scroll(activeButton, True)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Scroll the control for the selected button.
        ''' </summary>
        ''' <param name="button">Button to scroll for.</param>
        ''' <param name="fromTimer">Whether request to scroll from a 
        ''' scroll button timer event.</param>
        Private Sub Scroll(button As ListBarScrollButton, fromTimer As Boolean)
            Dim direction As Integer = (If(button.ButtonType = ListBarScrollButton.ListBarScrollButtonType.Up, 1, -1))
            Scroll(direction, fromTimer)
        End Sub

        ''' <summary>
        ''' Scroll the control for the selected button.
        ''' </summary>
        ''' <param name="button">Button to scroll for.</param>
        Private Sub Scroll(button As ListBarScrollButton)
            Scroll(button, False)
        End Sub


        ''' <summary>
        ''' Scroll the control in the specified direction.
        ''' </summary>
        ''' <param name="direction">The direction to move in.  Note that this follows
        ''' the direction of movement of an item: +1 scrolls up, -1 scrolls down.</param>
        Private Sub Scroll(direction As Integer)
            Scroll(direction, False)
        End Sub

        ''' <summary>
        ''' Scroll the control in the specified direction.
        ''' </summary>
        ''' <param name="direction">The direction to move in.  Note that this follows
        ''' the direction of movement of an item: +1 scrolls up, -1 scrolls down.</param>
        ''' <param name="fromTimer">Whether request to scroll from a 
        ''' scroll button timer event.</param>
        Private Sub Scroll(direction As Integer, fromTimer As Boolean)
            ' get the distance we must scroll to move one entire
            ' item:
            Dim selGroup As ListBarGroup = SelectedGroup
            If selGroup.Items(0) Is Nothing Then
                Exit Sub
            End If
            Dim endScrollOffset As Integer = selGroup.ScrollOffset + (direction * selGroup.Items(0).Height)
            If endScrollOffset > 0 Then
                endScrollOffset = 0
            End If

            ' Get the invalidation rectangle:
            Dim rcInvalid As New Rectangle(New Point(1, selGroup.ButtonLocation.X + selGroup.ButtonHeight), New Size(Me.Width - 2, (If((m_groups.IndexOf(selGroup) = m_groups.Count - 1), Me.Height - (selGroup.ButtonLocation.Y + selGroup.ButtonHeight), m_groups(m_groups.IndexOf(selGroup) + 1).ButtonLocation.Y))))

            ' Starting from the current point, scroll the selected
            ' bar to the new point in ever increasing steps:
            Dim [step] As Integer = direction
            If fromTimer Then
                [step] *= selGroup.Items(0).Height \ 4
            End If
            While selGroup.ScrollOffset <> endScrollOffset
                ' determine the new scroll offset:
                Dim newOffset As Integer = selGroup.ScrollOffset + [step]
                If direction < 0 Then
                    If newOffset < endScrollOffset Then
                        newOffset = endScrollOffset
                    End If
                Else
                    If newOffset > endScrollOffset Then
                        newOffset = endScrollOffset
                    End If
                End If
                selGroup.ScrollOffset = newOffset

                ' refresh the display:
                Invalidate()
                Me.Update()

                ' Make the next step larger.
                [step] *= 2
            End While

            ' Ensure that everything is shown in the right place
            DoResize()
        End Sub

        ''' <summary>
        ''' Raises the Resize event and performs internal
        ''' sizing of the objects in the control.
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnResize(e As EventArgs)
            DoResize()
            MyBase.OnResize(e)
        End Sub

        ''' <summary>
        ''' Raises the SizeChanged event for this control
        ''' and internally sizes the display.
        ''' </summary>
        ''' <param name="e">Event arguments associated with this
        ''' event.</param>
        Protected Overrides Sub OnSizeChanged(e As EventArgs)
            DoResize()
            Me.Invalidate()

            MyBase.OnSizeChanged(e)
        End Sub

        Private Function ensureSelection() As ListBarGroup
            Dim selectedGroup__1 As ListBarGroup = SelectedGroup

            If (selectedGroup__1 Is Nothing) OrElse (Not selectedGroup__1.Visible) Then
                selectedGroup__1 = Nothing
                If m_groups.Count > 0 Then
                    For i As Integer = 0 To m_groups.Count - 1
                        If (m_groups(i).Visible) AndAlso (selectedGroup__1 Is Nothing) Then
                            m_groups(i).Selected = True
                            selectedGroup__1 = m_groups(i)
                        Else
                            If m_groups(i).Selected Then
                                m_groups(i).Selected = False
                            End If
                        End If
                    Next
                End If
            End If
            Return selectedGroup__1
        End Function

        ''' <summary>
        ''' Called by the control's internal sizing mechanism.
        ''' Returns the client size excluding the border of the
        ''' control.
        ''' </summary>
        ''' <returns>A <c>Rectangle</c> providing the area to 
        ''' draw the control into.</returns>
        Protected Overridable Function GetClientRectangleExcludingBorder() As Rectangle
            Dim rcClient As New Rectangle(Me.ClientRectangle.Left + 1, Me.ClientRectangle.Top + 1, Me.ClientRectangle.Width - 2, Me.ClientRectangle.Height - 2)
            Return rcClient
        End Function

        ''' <summary>
        ''' Called by the control's internal sizing mechanism.
        ''' Returns the rectangle for a scroll button.
        ''' </summary>
        ''' <param name="buttonType">The scroll button to
        ''' get the rectangle for.</param>
        ''' <param name="selectedGroup">The Selected Group in the control.</param>
        ''' <param name="internalGroupHeight">The internal height of the
        ''' selected group</param>
        ''' <returns>The Rectangle for the scroll button.</returns>
        Protected Overridable Function GetScrollButtonRectangle(buttonType As ListBarScrollButton.ListBarScrollButtonType, selectedGroup As ListBarGroup, internalGroupHeight As Integer) As Rectangle
            Dim buttonRect As Rectangle
            If buttonType = ListBarScrollButton.ListBarScrollButtonType.Up Then
                buttonRect = New Rectangle(New Point((If((Me.RightToLeft = RightToLeft.Yes), 2, Me.Width - 2 - btnUp.Rectangle.Width)), selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight + 2), btnUp.Rectangle.Size)
            Else
                buttonRect = New Rectangle(New Point((If((Me.RightToLeft = RightToLeft.Yes), 2, Me.Width - 2 - btnUp.Rectangle.Width)), selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight + internalGroupHeight - 2 - btnDown.Rectangle.Height), btnDown.Rectangle.Size)
            End If
            Return buttonRect

        End Function


        Public Sub DoResize()
            If Me.redraw Then
                If Me.m_groups.Count > 0 Then
                    Dim selectedGroup As ListBarGroup = ensureSelection()
                    If selectedGroup IsNot Nothing Then
                        Dim rcClient As Rectangle = GetClientRectangleExcludingBorder()
                        rcListView = New Rectangle(rcClient.Location, rcClient.Size)

                        Dim lastVisibleGroup As Integer = 0
                        Dim firstVisibleGroup As Integer = m_groups.Count - 1
                        Dim nextVisibleGroup As Integer = firstVisibleGroup

                        For i As Integer = 0 To m_groups.IndexOf(selectedGroup)
                            Dim group As ListBarGroup = m_groups(i)

                            If group.Visible Then
                                Dim buttonWidth As Integer = GetGroupButtonWidth(group)
                                group.SetLocationAndWidth(New Point(rcClient.Left, rcListView.Top), buttonWidth)
                                rcListView.Y += group.ButtonHeight
                                rcListView.Height -= group.ButtonHeight

                                If i > lastVisibleGroup Then
                                    lastVisibleGroup = i
                                End If
                                If i < firstVisibleGroup Then
                                    firstVisibleGroup = i
                                End If
                            End If
                        Next

                        Dim bottom As Integer = rcClient.Bottom
                        For i As Integer = m_groups.Count - 1 To m_groups.IndexOf(selectedGroup) + 1 Step -1
                            Dim group As ListBarGroup = m_groups(i)
                            If group.Visible Then
                                Dim buttonWidth As Integer = GetGroupButtonWidth(group)
                                bottom -= group.ButtonHeight
                                rcListView.Height -= group.ButtonHeight
                                group.SetLocationAndWidth(New Point(rcClient.Left, bottom), buttonWidth)

                                If i > lastVisibleGroup Then
                                    lastVisibleGroup = i
                                End If
                                If i < nextVisibleGroup Then
                                    nextVisibleGroup = i
                                End If
                            End If
                        Next

                        Dim size As Integer = selectedGroup.Items.Height
                        Dim height As Integer = selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight
                        If m_groups.IndexOf(selectedGroup) = lastVisibleGroup Then
                            height = Me.ClientRectangle.Height - height
                        Else
                            height = m_groups(nextVisibleGroup).ButtonLocation.Y - height
                        End If

                        Dim needUp As Boolean = False
                        Dim needDown As Boolean = False

                        needUp = (selectedGroup.ScrollOffset < 0)
                        needDown = ((size + selectedGroup.ScrollOffset) > height)

                        Dim btnUpRect As Rectangle = GetScrollButtonRectangle(ListBarScrollButton.ListBarScrollButtonType.Up, selectedGroup, height)
                        btnUp.SetRectangle(btnUpRect)
                        btnUp.Visible = needUp
                        If Not needUp Then
                            If Me.activeButton IsNot Nothing Then
                                If Me.activeButton.Equals(btnUp) Then
                                    buttonPressed.Enabled = False
                                End If
                            End If
                        End If

                        Dim btnDownRect As Rectangle = GetScrollButtonRectangle(ListBarScrollButton.ListBarScrollButtonType.Down, selectedGroup, height)
                        btnDown.SetRectangle(btnDownRect)
                        btnDown.Visible = needDown
                        If Not needDown Then
                            If Me.activeButton IsNot Nothing Then
                                If Me.activeButton.Equals(btnDown) Then
                                    buttonPressed.Enabled = False
                                End If
                            End If
                        End If
                    Else
                        btnUp.Visible = False
                        btnDown.Visible = False
                    End If

                    If Me.Width <> lastWidth Then
                        lastWidth = Me.Width
                    End If

                    If Me.Height <> lastHeight Then
                        lastHeight = Me.Height

                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Raises the Paint event and performs internal drawing of the
        ''' control.	
        ''' </summary>
        ''' <param name="e">A PaintEventArgs object with details about the 
        ''' paint event that must be performed.</param>
        Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
            If scrollingGroup Then
                RenderScrollNewGroup(e)
            Else
                Render(e)
            End If
            MyBase.OnPaint(e)
        End Sub

        ''' <summary>
        ''' Raises the double click event and performs internal double-click
        ''' processing for the control.
        ''' </summary>
        ''' <param name="e"><see cref="EventArgs"/> associated with this
        ''' double-click event.</param>
        Protected Overrides Sub OnDoubleClick(e As EventArgs)
            MyBase.OnDoubleClick(e)
            Dim pt As Point = Me.PointToClient(Cursor.Position)

            Dim obj As IMouseObject = HitTest(pt, False)
            If obj IsNot Nothing Then
                If GetType(ListBarItem).IsAssignableFrom(obj.[GetType]()) Then
                    Dim item As ListBarItem = DirectCast(obj, ListBarItem)
                    Dim button As MouseButtons = MouseButtons.Left
                    ' TODO should use GetAsyncKeyState or whatever the Framework equivalent is
                    Dim ice As New ItemClickedEventArgs(item, pt, button)
                    OnItemDoubleClicked(ice)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Raises the <see cref="ItemDoubleClicked"/> event for an item.
        ''' </summary>
        ''' <param name="e">The <see cref="ItemClickedEventArgs"/> details
        ''' associated with the double click event.</param>
        Protected Overridable Sub OnItemDoubleClicked(e As ItemClickedEventArgs)
            RaiseEvent ItemDoubleClicked(Me, e)
        End Sub

        ''' <summary>
        ''' Raises the MouseDown event and performs internal mouse-down
        ''' processing for the control.
        ''' </summary>
        ''' <param name="e">A MouseEventArgs object with details about the
        ''' mouse event that has occurred.</param>
        Protected Overrides Sub OnMouseDown(e As System.Windows.Forms.MouseEventArgs)
            MyBase.OnMouseDown(e)

            If e.Button = MouseButtons.Left Then
                If mouseTrack IsNot Nothing Then
                    mouseDown = mouseTrack
                    mouseDown.MouseDown = True
                    mouseDown.MouseDownPoint = New Point(e.X, e.Y)

                    ' Check whether a scroll button has been pressed.
                    ' If it has, then start a timer to auto-scroll
                    ' more.
                    If GetType(ListBarScrollButton).IsAssignableFrom(mouseTrack.[GetType]()) Then
                        ' Set the active scrolling button:
                        activeButton = DirectCast(mouseTrack, ListBarScrollButton)
                        ' perform the initial scroll:
                        Scroll(activeButton)
                        ' initialise the timer:
                        buttonPressed.Interval = 350
                        buttonPressed.Enabled = True
                    ElseIf GetType(ListBarItem).IsAssignableFrom(mouseTrack.[GetType]()) Then
                        If Me.m_selectOnMouseDown Then
                            MouseSelectItem(DirectCast(mouseTrack, ListBarItem), e)
                        End If
                    End If

                    ' Redraw the control:
                    Invalidate()
                End If
            End If

        End Sub

        ''' <summary>
        ''' Raises the MouseMove event and performs mouse move processing
        ''' for the control.
        ''' </summary>
        ''' <param name="e">A MouseEventArgs object describing the mouse
        ''' move event that has occurred.</param>
        Protected Overrides Sub OnMouseMove(e As System.Windows.Forms.MouseEventArgs)
            MyBase.OnMouseMove(e)

            ' no motion during item editing
            If m_editItem IsNot Nothing Then
                Return
            End If

            ' detect if the mouse is over anything:
            Dim newMouseOver As IMouseObject = HitTest(New Point(e.X, e.Y))

            If newMouseOver Is Nothing Then
                If mouseTrack IsNot Nothing Then
                    mouseTrack.MouseOver = False
                    mouseTrack = Nothing
                    Me.Cursor = Cursors.[Default]
                    Invalidate()
                End If
                If Me.m_toolTip IsNot Nothing Then
                    Me.m_toolTip.SetToolTip(Me, "")
                End If
            Else
                Dim noChange As Boolean = False
                If mouseTrack IsNot Nothing Then
                    If mouseTrack Is newMouseOver Then
                        ' We're not tracking a new item.
                        noChange = True

                        ' However, if we mouse-downed on an item, then we 
                        ' should check if the new mouse position is sufficiently
                        ' far from the original position that a drag operation
                        ' is in order:
                        If Me.m_allowDragItems Then
                            If GetType(ListBarItem).IsAssignableFrom(mouseTrack.[GetType]()) Then
                                If mouseTrack.MouseDown Then
                                    Dim hysteresis As Integer = (If(SelectedGroup.View = ListBarGroupView.LargeIcons, 4, 2))
                                    If (Math.Abs(mouseTrack.MouseDownPoint.X - e.X) > hysteresis) OrElse (Math.Abs(mouseTrack.MouseDownPoint.Y - e.Y) > hysteresis) Then
                                        ' time to start dragging:
                                        Dim dragItem As ListBarItem = DirectCast(mouseTrack, ListBarItem)
                                        Me.DoDragDrop(dragItem, DragDropEffects.Move)
                                        InternalDragDropComplete(dragItem, True)
                                        EnsureItemVisible(dragItem)
                                        Return
                                    End If
                                End If
                            End If
                        End If
                        If Me.m_allowDragGroups Then
                            If GetType(ListBarGroup).IsAssignableFrom(mouseTrack.[GetType]()) Then
                                If mouseTrack.MouseDown Then
                                    If (Math.Abs(mouseTrack.MouseDownPoint.X - e.X) > 4) OrElse (Math.Abs(mouseTrack.MouseDownPoint.Y - e.Y) > 4) Then
                                        ' time to start dragging:
                                        Dim dragGroup As ListBarGroup = DirectCast(mouseTrack, ListBarGroup)
                                        Me.DoDragDrop(dragGroup, DragDropEffects.Move)
                                        'InternalDragDropComplete(dragGroup);
                                        dragGroup.MouseOver = False
                                        dragGroup.MouseDown = False
                                        Return

                                    End If
                                End If
                            End If
                        End If
                    Else
                        mouseTrack.MouseOver = False
                    End If
                End If
                If Not noChange Then
                    mouseTrack = newMouseOver
                    If Me.m_toolTip IsNot Nothing Then
                        Me.m_toolTip.SetToolTip(Me, mouseTrack.ToolTipText)
                    End If
                    mouseTrack.MouseOver = True
                    If GetType(ListBarGroup).IsAssignableFrom(mouseTrack.[GetType]()) Then
                        Me.Cursor = Cursors.Hand
                    Else
                        Me.Cursor = Cursors.[Default]
                    End If
                    Invalidate()
                End If
            End If
        End Sub


        ''' <summary>
        ''' Raises the MouseUp event and performs mouse up processing
        ''' for the control.
        ''' </summary>
        ''' <param name="e">A MouseEventArgs object describing the mouse
        ''' move event that has occurred.</param>
        Protected Overrides Sub OnMouseUp(e As System.Windows.Forms.MouseEventArgs)
            If e.Button = MouseButtons.Right Then
                If mouseTrack IsNot Nothing Then
                    If GetType(ListBarItem).IsAssignableFrom(mouseTrack.[GetType]()) Then
                        Dim listBarItem As ListBarItem = DirectCast(mouseTrack, ListBarItem)
                        Me.SelectedRightClickItemCaption = listBarItem.Caption
                    End If
                End If
            End If

            MyBase.OnMouseUp(e)

            If e.Button = MouseButtons.Left Then
                If mouseTrack IsNot Nothing Then
                    If mouseTrack.Equals(mouseDown) Then

                        If GetType(ListBarGroup).IsAssignableFrom(mouseTrack.[GetType]()) Then
                            Dim bgc As New BeforeGroupChangedEventArgs(DirectCast(mouseTrack, ListBarGroup))
                            OnBeforeGroupChanged(bgc)
                            If Not bgc.Cancel Then
                                ' group clicked.  Select the new group:
                                SelectGroup(DirectCast(mouseTrack, ListBarGroup))
                                OnSelectedGroupChanged(New System.EventArgs())
                                Dim gce As New GroupClickedEventArgs(DirectCast(mouseTrack, ListBarGroup), New Point(e.X, e.Y), e.Button)
                                OnGroupClicked(gce)
                            End If
                            ' don't need to do anything here, except be sure
                            ' we reset the active scroll button & timer later
                        ElseIf GetType(ListBarScrollButton).IsAssignableFrom(mouseTrack.[GetType]()) Then
                        Else
                            If activeButton Is Nothing Then
                                If Not Me.m_selectOnMouseDown Then
                                    MouseSelectItem(DirectCast(mouseTrack, ListBarItem), e)
                                End If
                            End If
                        End If
                    End If
                End If

                ' no more scrolling
                activeButton = Nothing
                buttonPressed.Enabled = False

                If mouseDown IsNot Nothing Then
                    mouseDown.MouseDown = False
                    mouseDown.MouseOver = False
                End If
                If mouseTrack IsNot Nothing Then
                    mouseTrack.MouseOver = False
                End If
                Invalidate()

            ElseIf e.Button = MouseButtons.Right Then
                If mouseTrack IsNot Nothing Then
                    ' Right click?
                    If GetType(ListBarGroup).IsAssignableFrom(mouseTrack.[GetType]()) Then
                        Dim gce As New GroupClickedEventArgs(DirectCast(mouseTrack, ListBarGroup), New Point(e.X, e.Y), e.Button)
                        OnGroupClicked(gce)
                    ElseIf GetType(ListBarItem).IsAssignableFrom(mouseTrack.[GetType]()) Then
                        Dim ic As New ItemClickedEventArgs(DirectCast(mouseTrack, ListBarItem), New Point(e.X, e.Y), e.Button)
                        OnItemClicked(ic)
                        ' no action currently
                    Else
                    End If
                Else
                    ' group right click:
                    Dim gce As New GroupClickedEventArgs(SelectedGroup, New Point(e.X, e.Y), e.Button)
                    OnGroupClicked(gce)
                End If
            End If
        End Sub

        Public Sub OnMouseUpTest(e As System.Windows.Forms.MouseEventArgs, mouseTrack As ListBarGroup)
            MyBase.OnMouseUp(e)

            If e.Button = MouseButtons.Left Then
                If mouseTrack IsNot Nothing Then
                    'If mouseTrack.Equals(mouseDown) Then

                    'For Each mgroup As ListBarGroup In Me.m_groups
                    '    If mgroup.Caption = mouseTrack.Caption Then

                    '        mouseTrack.ButtonLocation.X = mgroup.ButtonLocation.X

                    '        Exit For
                    '    End If

                    'Next

                    If GetType(ListBarGroup).IsAssignableFrom(mouseTrack.[GetType]()) Then
                        Dim bgc As New BeforeGroupChangedEventArgs(DirectCast(mouseTrack, ListBarGroup))
                        OnBeforeGroupChanged(bgc)
                        If Not bgc.Cancel Then
                            ' group clicked.  Select the new group:
                            SelectGroup(DirectCast(mouseTrack, ListBarGroup))
                            OnSelectedGroupChanged(New System.EventArgs())
                            Dim gce As New GroupClickedEventArgs(DirectCast(mouseTrack, ListBarGroup), New Point(e.X, e.Y), e.Button)
                            OnGroupClicked(gce)
                        End If
                        ' don't need to do anything here, except be sure
                        ' we reset the active scroll button & timer later
                    ElseIf GetType(ListBarScrollButton).IsAssignableFrom(mouseTrack.[GetType]()) Then
                    Else
                        'If activeButton Is Nothing Then
                        '    If Not Me.m_selectOnMouseDown Then
                        '        MouseSelectItem(DirectCast(mouseTrack, ListBarItem), e)
                        '    End If
                        'End If
                    End If
                    'End If
                End If

                ' no more scrolling
                activeButton = Nothing
                buttonPressed.Enabled = False

                If mouseDown IsNot Nothing Then
                    mouseDown.MouseDown = False
                    mouseDown.MouseOver = False
                End If
                If mouseTrack IsNot Nothing Then
                    mouseTrack.MouseOver = False
                End If
                Invalidate()

            ElseIf e.Button = MouseButtons.Right Then
                'If mouseTrack IsNot Nothing Then
                '    ' Right click?
                '    If GetType(ListBarGroup).IsAssignableFrom(mouseTrack.[GetType]()) Then
                '        Dim gce As New GroupClickedEventArgs(DirectCast(mouseTrack, ListBarGroup), New Point(e.X, e.Y), e.Button)
                '        OnGroupClicked(gce)
                '    ElseIf GetType(ListBarItem).IsAssignableFrom(mouseTrack.[GetType]()) Then
                '        Dim ic As New ItemClickedEventArgs(DirectCast(mouseTrack, ListBarItem), New Point(e.X, e.Y), e.Button)
                '        OnItemClicked(ic)
                '        ' no action currently
                '    Else
                '    End If
                'Else
                '    ' group right click:
                '    Dim gce As New GroupClickedEventArgs(SelectedGroup, New Point(e.X, e.Y), e.Button)
                '    OnGroupClicked(gce)
                'End If
            End If
        End Sub

        ''' <summary>
        ''' Raises the MouseLeave event and performs internal mouse
        ''' track processing for the control.
        ''' </summary>
        ''' <param name="e">Event arguments associated with this event.</param>
        Protected Overrides Sub OnMouseLeave(e As System.EventArgs)
            MyBase.OnMouseLeave(e)
            If mouseTrack IsNot Nothing Then
                mouseTrack.MouseOver = False
                mouseTrack = Nothing
                Me.Cursor = Cursors.[Default]
                Invalidate()
            End If
        End Sub

        ''' <summary>
        ''' Raises the MouseWheel event and performs mouse wheel 
        ''' processing for the control.
        ''' </summary>
        ''' <param name="e">A MouseEventArgs object describing the mouse
        ''' move event that has occurred.</param>
        Protected Overrides Sub OnMouseWheel(e As System.Windows.Forms.MouseEventArgs)
            MyBase.OnMouseWheel(e)

            If (e.Delta > 0) AndAlso (btnUp.Visible) Then
                Scroll(1)
            ElseIf (e.Delta < 0) AndAlso (btnDown.Visible) Then
                Scroll(-1)
            End If

        End Sub

        Private Function GetBestDragDropFormat(e As DragEventArgs) As Object
            Dim ret As Object = Nothing
            Dim defaultFormat As Object = Nothing
            For Each format As String In e.Data.GetFormats()
                Dim thisFormatData As Object = e.Data.GetData(format)
                If defaultFormat Is Nothing Then
                    defaultFormat = thisFormatData
                End If

                If GetType(ListBarItem).IsAssignableFrom(thisFormatData.[GetType]()) Then
                    ret = thisFormatData
                    Exit For
                ElseIf GetType(ListBarItem).IsAssignableFrom(thisFormatData.[GetType]()) Then
                    ret = thisFormatData
                    Exit For
                End If
            Next

            If ret Is Nothing Then
                ret = defaultFormat
            End If

            Return ret
        End Function

        Private Function GetTypeOrSubClassFromData(e As DragEventArgs, dataType As Type) As Object
            Dim ret As Object = Nothing
            For Each format As String In e.Data.GetFormats()
                If dataType.IsAssignableFrom(e.Data.GetData(format).[GetType]()) Then
                    ret = e.Data.GetData(format)
                    Exit For
                End If
            Next
            Return ret
        End Function

        Private Function PerformAutoDrag(e As DragEventArgs) As Boolean
            Dim ret As Boolean = False
            If (Me.m_allowDragItems) OrElse (Me.m_allowDragGroups) Then
                For Each format As String In e.Data.GetFormats()
                    Dim dataType As Type = e.Data.GetData(format).[GetType]()
                    If GetType(ListBarItem).IsAssignableFrom(dataType) Then
                        ret = True
                        Exit For
                    ElseIf GetType(ListBarGroup).IsAssignableFrom(dataType) Then
                        ret = True
                        Exit For
                    End If
                Next
            End If
            Return ret
        End Function

        ''' <summary>
        ''' Raises the DragOver event and performs internal processing of 
        ''' drag-drop to show the insertion point and navigate through
        ''' the items in the control.
        ''' </summary>
        ''' <param name="e">A DragEventArgs object describing the drag
        ''' over being performed.</param>
        Protected Overrides Sub OnDragOver(e As DragEventArgs)
            ' perform the base operation:
            MyBase.OnDragOver(e)

            If m_groups.Count > 0 Then
                If e.Effect <> DragDropEffects.None Then
                    Me.InternalDragOverProcess(e, True)
                ElseIf Me.PerformAutoDrag(e) Then
                    Me.InternalDragOverProcess(e, False)
                End If
            End If

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnDragDrop(e As DragEventArgs)
            ' perform the base operation:
            MyBase.OnDragDrop(e)

            If m_groups.Count > 0 Then
                Dim obj As Object = GetBestDragDropFormat(e)

                If e.Effect <> DragDropEffects.None Then
                    Dim move As Boolean = (e.Effect = DragDropEffects.Move)
                    Me.InternalDragDropComplete(obj, move)
                ElseIf Me.PerformAutoDrag(e) Then
                    Me.InternalDragDropComplete(obj, True)
                End If
            End If

        End Sub

        ''' <summary>
        ''' Raises the BeforeSelectedGroupChanged event.  This event enables
        ''' the user to prevent a group selection.
        ''' </summary>
        ''' <param name="e">The BeforeGroupChangedEventArgs object associated
        ''' with this event.</param>
        Protected Overridable Sub OnBeforeGroupChanged(ByRef e As BeforeGroupChangedEventArgs)
            RaiseEvent BeforeSelectedGroupChanged(Me, e)
        End Sub
        ''' <summary>
        ''' Raises the BeforeItemClicked event.  This event enables
        ''' the user to prevent an item from being selected.
        ''' </summary>
        ''' <param name="e">The BeforeItemClickedEventArgs object associated
        ''' with this event.</param>
        Protected Overridable Sub OnBeforeItemClicked(ByRef e As BeforeItemClickedEventArgs)
            e.Cancel = (Not e.Item.Enabled)
            RaiseEvent BeforeItemClicked(Me, e)
        End Sub

        ''' <summary>
        ''' Raises the <c>ItemClicked</c> event. 
        ''' </summary>
        ''' <param name="e">The <c>ItemClickedEventArgs</c> object associated 
        ''' with this event.</param>
        Protected Overridable Sub OnItemClicked(e As ItemClickedEventArgs)
            RaiseEvent ItemClicked(Me, e)
        End Sub

        ''' <summary>
        ''' Raises the <c>GroupClicked</c> event.
        ''' </summary>
        ''' <param name="e">The <c>GroupClickedEventArgs</c> object
        ''' associated with this event.</param>
        Public Overridable Sub OnGroupClicked(e As GroupClickedEventArgs)
            RaiseEvent GroupClicked(Me, e)
        End Sub

        ''' <summary>
        ''' Raises the BeforeLabelEdit event for an item in the control.
        ''' </summary>
        ''' <param name="e">The LabelEditEventArgs describing the item
        ''' that is about to be edited and allowing the edit action
        ''' to be cancelled.</param>
        Protected Overridable Sub OnBeforeLabelEdit(e As ListBarLabelEditEventArgs)
            RaiseEvent BeforeLabelEdit(Me, e)
        End Sub

        ''' <summary>
        ''' Raises the AfterLabelEdit event for an item in the control.
        ''' </summary>
        ''' <param name="e">The AfterEditEventArgs describing the item
        ''' that has just been edited and allowing the edit action
        ''' to be cancelled or the new caption to be changed.</param>
        Protected Overridable Sub OnAfterLabelEdit(e As ListBarLabelEditEventArgs)
            RaiseEvent AfterLabelEdit(Me, e)
        End Sub

        ''' <summary>
        ''' Raises the <c>SelectedGroupChanged</c> event.
        ''' </summary>
        ''' <param name="e">An EventArgs object associated with the event.</param>
        Protected Overridable Sub OnSelectedGroupChanged(e As System.EventArgs)
            RaiseEvent SelectedGroupChanged(Me, e)
        End Sub

        Private Sub txtEdit_TextChanged(sender As Object, e As System.EventArgs)
            If m_editItem IsNot Nothing Then
                setTextBoxSize(m_editItem)
            End If
        End Sub

        Private Sub txtEdit_KeyDown(sender As Object, e As KeyEventArgs)
            Select Case e.KeyData
                Case Keys.[Return]
                    ' end editing:
                    EndTextEdit(True)
                    Exit Select

                Case Keys.Escape
                    ' cancel editing:
                    EndTextEdit(False)
                    Exit Select
            End Select
        End Sub

        Private Sub popupCancel_PopupCancel(sender As Object, e As EventArgs)
            EndTextEdit(True)
        End Sub

#End Region

#Region "Internal implementation"
        Private Sub EndTextEdit(commit As Boolean)
            If Me.m_editItem IsNot Nothing Then
                Dim editedItem As ListBarItem = Me.m_editItem
                Me.m_editItem = Nothing

                If (commit) AndAlso (editedItem IsNot Nothing) Then
                    Dim selectedGroup__1 As ListBarGroup = SelectedGroup

                    Dim lea As New ListBarLabelEditEventArgs(selectedGroup__1.Items.IndexOf(m_editItem), txtEdit.Text, editedItem)
                    OnAfterLabelEdit(lea)

                    If Not lea.CancelEdit Then
                        If editedItem IsNot Nothing Then
                            ' may be shutting down...
                            editedItem.Caption = lea.Label
                        End If
                    End If
                End If
            ElseIf Me.editGroup IsNot Nothing Then
                Dim editedGroup As ListBarGroup = Me.editGroup
                Me.editGroup = Nothing

                If (commit) AndAlso (editedGroup IsNot Nothing) Then
                    Dim lea As New ListBarLabelEditEventArgs(Me.Groups.IndexOf(editedGroup), txtEdit.Text, editedGroup)
                    OnAfterLabelEdit(lea)

                    If Not lea.CancelEdit Then
                        If editedGroup IsNot Nothing Then
                            editedGroup.Caption = lea.Label
                        End If
                    End If
                End If
            End If

            txtEdit.Visible = False
            Invalidate()
        End Sub

        Private Sub InternalDragDropComplete(dragItem As Object, move As Boolean)
            Dim listBarDragItem As ListBarItem = Nothing

            If GetType(ListBarItem).IsAssignableFrom(dragItem.[GetType]()) Then
                listBarDragItem = DirectCast(dragItem, ListBarItem)
                listBarDragItem.MouseOver = False
                listBarDragItem.MouseDown = False
            End If

            If dragInsertPoint IsNot Nothing Then
                Dim groupTo As ListBarGroup = SelectedGroup
                If groupTo IsNot Nothing Then
                    ' cannot happen...
                    Dim groupFrom As ListBarGroup = Nothing

                    If listBarDragItem IsNot Nothing Then
                        ' Check which bar we've come from
                        ' (it may be none, we may have come
                        ' from another control):

                        For Each group As ListBarGroup In m_groups
                            If group.Items.Contains(listBarDragItem) Then
                                groupFrom = group
                                Exit For
                            End If
                        Next
                    End If

                    If groupFrom IsNot Nothing Then
                        ' Dragged from this control
                        ' moving to a new group: 
                        If move Then
                            If dragInsertPoint.ItemAfter IsNot Nothing Then
                                If dragInsertPoint.ItemAfter.Equals(listBarDragItem) Then
                                    listBarDragItem = Nothing
                                End If
                            ElseIf dragInsertPoint.ItemBefore IsNot Nothing Then
                                If dragInsertPoint.ItemBefore.Equals(listBarDragItem) Then
                                    listBarDragItem = Nothing
                                End If
                            End If
                            If listBarDragItem IsNot Nothing Then
                                groupFrom.Items.Remove(listBarDragItem)
                            End If
                        Else
                            ' Clone a new item to add:
                            Dim newItem As New ListBarItem(listBarDragItem.Caption, listBarDragItem.IconIndex, listBarDragItem.ToolTipText, listBarDragItem.Tag)
                            listBarDragItem = newItem
                        End If
                    Else
                        ' add a new item which represents what's been dragged
                        If listBarDragItem IsNot Nothing Then
                            ' there's an issue with which image to pick here
                            listBarDragItem = New ListBarItem(listBarDragItem.Caption, listBarDragItem.IconIndex, listBarDragItem.ToolTipText, listBarDragItem.Tag)
                        Else
                            ' Create a new item
                            listBarDragItem = New ListBarItem(dragItem.ToString())
                            DirectCast(dragItem, ListBarItem).Tag = dragItem
                        End If
                    End If

                    If listBarDragItem IsNot Nothing Then
                        If dragInsertPoint.ItemAfter IsNot Nothing Then
                            groupTo.Items.InsertAfter(dragInsertPoint.ItemAfter, listBarDragItem)
                        Else
                            groupTo.Items.InsertAfter(dragInsertPoint.ItemAfter, listBarDragItem)
                        End If
                    End If
                End If
            End If

            dragInsertPoint = Nothing
            Invalidate()
        End Sub

        Public Sub SelectGroup(group As ListBarGroup)
            ' first work out the scrolling logic:
            Dim intCounter As Integer = 0
            Dim selGroup As ListBarGroup = SelectedGroup
            If selGroup IsNot group Then
                ' Which groups are we moving between?
                Me.indexNew = Me.m_groups.IndexOf(group)

                For Each mgroup As ListBarGroup In Me.m_groups
                    If mgroup.Caption = group.Caption Then
                        Me.indexNew = intCounter
                        Exit For
                    End If
                    intCounter = intCounter + 1
                Next

                Me.indexCurrent = Me.m_groups.IndexOf(selGroup)

                ' Scrolling the new group into view:
                If Me.redraw Then
                    Me.scrollingGroup = True

                    If Me.indexNew > Me.indexCurrent Then
                        ' the new index is below the current one.					
                        ' Scroll buttons from indexCurrent + 1 to indexNew
                        ' upwards
                        Dim newIndexTargetPos As Integer = selGroup.ButtonLocation.Y + selGroup.ButtonHeight
                        For i As Integer = Me.indexCurrent + 1 To Me.indexNew - 1
                            If Me.m_groups(i).Visible Then
                                newIndexTargetPos += Me.m_groups(i).ButtonHeight
                            End If
                        Next

                        Dim finished As Boolean = False
                        Dim currentPos As Integer = group.ButtonLocation.Y
                        Dim [step] As Integer = -1
                        While Not finished
                            currentPos += [step]
                            If currentPos <= newIndexTargetPos Then
                                [step] += (newIndexTargetPos - currentPos)
                                currentPos = newIndexTargetPos
                                finished = True
                            End If

                            For i As Integer = Me.indexCurrent + 1 To Me.indexNew
                                Dim workGroup As ListBarGroup = Me.m_groups(i)
                                If workGroup.Visible Then
                                    Dim newLocation As Point = workGroup.ButtonLocation
                                    newLocation.Y += [step]
                                    workGroup.SetLocationAndWidth(newLocation, workGroup.ButtonWidth)
                                End If
                            Next

                            Me.Invalidate()
                            Me.Update()

                            [step] *= 2

                        End While
                    Else
                        ' the new index is above the current one.
                        ' scroll buttons from indexNew + 1 to indexCurrent
                        ' downwards
                        Dim lastIndex As Integer = indexCurrent
                        Dim nextIndex As Integer = Me.Groups.Count - 1
                        For i As Integer = indexCurrent + 1 To Me.Groups.Count - 1
                            If i > lastIndex Then
                                lastIndex = i
                            End If
                            If i < nextIndex Then
                                nextIndex = i
                            End If
                        Next
                        Dim currentTargetPos As Integer = (If(indexCurrent = lastIndex, Me.ClientRectangle.Height, Me.m_groups(nextIndex).ButtonLocation.Y))

                        Dim finished As Boolean = False
                        Dim currentPos As Integer = selGroup.ButtonLocation.Y
                        Dim [step] As Integer = 1
                        While Not finished
                            currentPos += [step]
                            If currentPos >= currentTargetPos Then
                                [step] -= (currentPos - currentTargetPos)
                                currentPos = currentTargetPos
                                finished = True
                            End If

                            For i As Integer = indexNew + 1 To indexCurrent
                                Dim workGroup As ListBarGroup = Me.m_groups(i)
                                If workGroup.Visible Then
                                    Dim newLocation As Point = workGroup.ButtonLocation
                                    newLocation.Y += [step]
                                    workGroup.SetLocationAndWidth(newLocation, workGroup.ButtonWidth)
                                End If
                            Next

                            Me.Invalidate()
                            Me.Update()


                            [step] *= 2

                        End While
                    End If

                    Me.scrollingGroup = False
                End If

                selGroup.Selected = False
                group.Selected = True
                DoResize()
            End If

        End Sub


        ''' <summary>
        ''' Selects an item in response to a mouse event.
        ''' </summary>
        ''' <param name="item">Item to be selected.</param>
        ''' <param name="e"><see cref="System.Windows.Forms.MouseEventArgs"/> 
        ''' details associated with the mouse event.</param>
        Private Sub MouseSelectItem(item As ListBarItem, e As MouseEventArgs)
            Dim bic As New BeforeItemClickedEventArgs(DirectCast(mouseTrack, ListBarItem))
            OnBeforeItemClicked(bic)
            If Not bic.Cancel Then
                ' item clicked:
                SelectItem(DirectCast(mouseTrack, ListBarItem))
                Dim ic As New ItemClickedEventArgs(DirectCast(mouseTrack, ListBarItem), New Point(e.X, e.Y), e.Button)
                OnItemClicked(ic)
            End If
        End Sub

        ''' <summary>
        ''' Selects an item in the selected bar and makes
        ''' it visible.
        ''' </summary>
        ''' <param name="item">The item to select.</param>
        Private Sub SelectItem(item As ListBarItem)
            BeginUpdate()
            For Each otherItem As ListBarItem In SelectedGroup.Items
                otherItem.Selected = False
            Next
            item.Selected = True
            EndUpdate()
            EnsureItemVisible(item)
            Invalidate()
        End Sub

        ''' <summary>
        ''' Starts editing the specified ListBarGroup.  Note this
        ''' method is called from the StartEdit method of a ListBarGroup.
        ''' </summary>
        ''' <param name="group">The group to start editing.</param>
        Protected Friend Sub StartGroupEdit(group As ListBarGroup)
            ' Fire the BeforeLabelEdit event:
            Dim e As New ListBarLabelEditEventArgs(Me.m_groups.IndexOf(group), group.Caption, group)
            OnBeforeLabelEdit(e)

            If Not e.CancelEdit Then
                editGroup = group

                ' Focus the control:
                Me.Focus()

                ' Set the edit text:
                txtEdit.Text = group.Caption
                txtEdit.Font = (If(group.Font Is Nothing, Me.Font, group.Font))
                txtEdit.Location = group.ButtonLocation
                txtEdit.Size = New Size(group.ButtonWidth, group.ButtonHeight)
                txtEdit.Visible = True
                txtEdit.BringToFront()
                txtEdit.Focus()

                popupCancel.StartTracking(txtEdit)
            End If

        End Sub

        ''' <summary>
        ''' Starts editing the specified <c>ListBarItem</c>.  Note this
        ''' method is called from the <c>StartEdit</c> method of a 
        ''' <c>ListBarItem</c>.
        ''' <seealso cref="ListBarItem.StartEdit"/>
        ''' </summary>
        ''' <param name="item">The item to start editing.</param>
        Protected Friend Sub StartItemEdit(item As ListBarItem)

            ' Get rectangle of item relative to control:
            Dim selectedGroup__1 As ListBarGroup = SelectedGroup

            ' Check whether item is part of the selected
            ' control:
            If selectedGroup__1.Items.Contains(item) Then
                ' Fire the BeforeLabelEdit event:
                Dim e As New ListBarLabelEditEventArgs(selectedGroup__1.Items.IndexOf(item), item.Caption, item)
                OnBeforeLabelEdit(e)
                If Not e.CancelEdit Then
                    m_editItem = item

                    ' Make sure we can see it:
                    EnsureItemVisible(item)

                    ' Focus the control:
                    Me.Focus()

                    ' Set the edit text:
                    txtEdit.Text = item.Caption
                    txtEdit.Font = (If(item.Font Is Nothing, Me.Font, item.Font))
                    setTextBoxSize(m_editItem)
                    Dim top As Integer = item.TextRectangle.Top
                    txtEdit.Top = top
                    txtEdit.Visible = True
                    txtEdit.BringToFront()
                    txtEdit.Focus()

                    popupCancel.StartTracking(txtEdit)
                End If
            Else
                Throw New InvalidOperationException("Editing is only possible on items belonging to the SelectedGroup in the control.")
            End If


        End Sub

        Private Sub setTextBoxSize(editItem As ListBarItem)
            Dim selectedGroup__1 As ListBarGroup = SelectedGroup
            If selectedGroup__1 IsNot Nothing Then

                Dim text As String = txtEdit.Text
                If text.Length = 0 Then
                    text = "Xg"
                End If

                Dim maxWidth As Integer = 0
                If selectedGroup__1.View = ListBarGroupView.SmallIcons Then
                    maxWidth = Me.ClientRectangle.Width - editItem.TextRectangle.Left - 1
                Else
                    maxWidth = Me.ClientRectangle.Width - 2
                End If

                Dim gfx As Graphics = Graphics.FromHwnd(txtEdit.Handle)
                Dim fmt As New StringFormat(StringFormatFlags.LineLimit Or (If(txtEdit.RightToLeft = RightToLeft.Yes, StringFormatFlags.DirectionRightToLeft, 0)))
                fmt.Alignment = StringAlignment.Center
                Dim textSize As SizeF = gfx.MeasureString(text, txtEdit.Font, maxWidth - 6, fmt)
                fmt.Dispose()
                gfx.Dispose()

                If textSize.Width < 24 Then
                    textSize.Width = 24
                End If
                textSize.Height += 2.0F

                txtEdit.Size = New Size(gPMFunctions.ToSafeInteger(Math.Truncate(textSize.Width)) + 6, gPMFunctions.ToSafeInteger(Math.Truncate(textSize.Height)) + 4)
                If selectedGroup__1.View = ListBarGroupView.SmallIcons Then
                    txtEdit.Left = editItem.TextRectangle.Left + 1
                Else
                    txtEdit.Left = 1 + (maxWidth - gPMFunctions.ToSafeInteger(Math.Truncate(textSize.Width))) \ 2
                End If
            End If
        End Sub

        ''' <summary>
        ''' Brings the specified <c>ListBarItem</c> into view if it is not already
        ''' visible.  The <c>ListBarItem</c> must be in the selected group.
        ''' <seealso cref="ListBarItem"/>
        ''' <seealso cref="ListBar.SelectedGroup"/>
        ''' </summary>
        ''' <param name="item">Item to bring into view.</param>
        Protected Friend Sub EnsureItemVisible(item As ListBarItem)
            ' Get rectangle of item relative to control:
            Dim selectedGroup__1 As ListBarGroup = SelectedGroup

            ' Check whether item is part of the selected
            ' group:
            If selectedGroup__1.Items.Contains(item) Then
                Dim rcVisible As New Rectangle(selectedGroup__1.ButtonLocation, New Size(Me.ClientRectangle.Width, 0))

                Dim nextGroup As ListBarGroup = Nothing
                For i As Integer = Me.m_groups.IndexOf(selectedGroup__1) + 1 To Me.m_groups.Count - 1
                    If Me.m_groups(i).Visible Then
                        nextGroup = Me.m_groups(i)
                        Exit For
                    End If
                Next

                If nextGroup Is Nothing Then
                    rcVisible.Height = Me.ClientRectangle.Height - (selectedGroup__1.ButtonLocation.Y + selectedGroup__1.ButtonHeight)
                Else
                    rcVisible.Height = nextGroup.ButtonLocation.Y - rcVisible.Top
                End If

                Dim invisible As Boolean = True
                Dim notFirstTime As Boolean = False
                While invisible
                    Dim rcItem As New Rectangle(item.Location, New Size(Me.ClientRectangle.Width, item.Height))
                    rcItem.Offset(0, selectedGroup__1.ButtonLocation.Y + selectedGroup__1.ButtonHeight + selectedGroup__1.ScrollOffset)

                    ' Check if the item is too low:
                    If rcItem.Bottom > rcVisible.Bottom Then
                        ' need to scroll down until it can be seen:
                        Scroll(-1, notFirstTime)
                    ElseIf rcItem.Top < rcVisible.Top Then
                        ' need to scroll up until it can be seen:
                        Scroll(1, notFirstTime)
                    Else
                        invisible = False
                    End If
                    notFirstTime = True
                End While
            End If
        End Sub

        ''' <summary>
        ''' Checks if there is an object which interacts with
        ''' the mouse in the control under the specified point.
        ''' </summary>
        ''' <param name="pt">The point to test.</param>
        ''' <returns>If there is a mouse object under the point 
        ''' then its IMouseObject interface, otherwise null.</returns>
        Private Function HitTest(pt As Point) As IMouseObject
            Return HitTest(pt, False)
        End Function

        ''' <summary>
        ''' Checks if there is an object which interacts with
        ''' the mouse in the control under the specified point.
        ''' </summary>
        ''' <param name="pt">The point to test.</param>
        ''' <returns>If there is a mouse object under the point 
        ''' then its IMouseObject interface, otherwise null.</returns>
        ''' <param name="forDragDrop">Whether the hit testing is
        ''' being performed for a drag-drop operation or not.  During
        ''' drag-drop, the hittest rectangle is relaxed so it includes
        ''' the entire rectangle and not just the icon and text.
        ''' </param>
        Private Function HitTest(pt As Point, forDragDrop As Boolean) As IMouseObject
            ' Default return value:
            Dim mouseObject As IMouseObject = Nothing
            Dim selectedGroup__1 As ListBarGroup = SelectedGroup

            ' Over a scroll button?
            If btnUp.HitTest(pt) Then
                ' over the scroll up button:
                mouseObject = btnUp
            ElseIf btnDown.HitTest(pt) Then
                ' over the scroll down button:
                mouseObject = btnDown
            Else
                If (forDragDrop) AndAlso (selectedGroup__1 IsNot Nothing) Then
                    ' we test for any point with 6 pixels of
                    ' the scroll bars if the scroll buttons are on:
                    If btnUp.Visible Then
                        Dim scrollTest As New Rectangle(selectedGroup__1.ButtonLocation.X, selectedGroup__1.ButtonLocation.Y + selectedGroup__1.ButtonHeight, Me.ClientRectangle.Width, 6)
                        If scrollTest.Contains(pt) Then
                            mouseObject = btnUp
                        End If
                    End If
                    If btnDown.Visible Then
                        Dim nextGroup As ListBarGroup = Nothing
                        For i As Integer = Me.m_groups.IndexOf(selectedGroup__1) + 1 To Me.m_groups.Count - 1
                            If Me.m_groups(i).Visible Then
                                nextGroup = Me.m_groups(i)
                                Exit For
                            End If
                        Next
                        If nextGroup IsNot Nothing Then
                            Dim scrollTest As New Rectangle(nextGroup.ButtonLocation.X, nextGroup.ButtonLocation.Y - 6, Me.ClientRectangle.Width, 6)
                            If scrollTest.Contains(pt) Then
                                mouseObject = btnDown
                            End If
                        End If
                    End If
                End If
            End If

            ' Check whether we're over any group buttons:
            If mouseObject Is Nothing Then
                For Each group As ListBarGroup In Me.m_groups
                    If group.Visible Then
                        Dim buttonRectangle As New Rectangle(group.ButtonLocation, New Size(group.ButtonWidth, group.ButtonHeight))
                        If buttonRectangle.Contains(pt) Then
                            ' over a group:
                            mouseObject = group
                            Exit For
                        End If
                    End If
                Next
            End If

            ' Otherwise check whether we're over any list bar buttons:
            If mouseObject Is Nothing Then
                ' Is there a selected ListBar Group?
                If selectedGroup__1 IsNot Nothing Then
                    ' Check each item in this group:
                    For Each item As ListBarItem In selectedGroup__1.Items
                        Dim rcTest As Rectangle
                        If forDragDrop Then
                            ' For drag drop the entire rectangle of the item
                            ' is taken into account:
                            rcTest = New Rectangle(item.Location, New Size(item.Width, item.Height))
                            rcTest.Offset(0, selectedGroup__1.ScrollOffset + selectedGroup__1.ButtonLocation.Y + selectedGroup__1.ButtonHeight)
                            If rcTest.Contains(pt) Then
                                mouseObject = item
                                Exit For

                            End If
                        Else
                            ' Get the icon rectangle of the item within the group:
                            rcTest = item.IconRectangle
                            ' Check if the point is there:
                            If rcTest.Contains(pt) Then
                                ' We're over an item:
                                mouseObject = item
                                Exit For
                            End If
                            ' Otherwise try the text rectangle:
                            rcTest = item.TextRectangle
                            If rcTest.Contains(pt) Then
                                ' We're over an item:
                                mouseObject = item
                                Exit For
                            End If
                        End If
                    Next
                End If
            End If

            ' Return the object the mouse is over if any
            Return mouseObject
        End Function


        ''' <summary>
        ''' Internal notification from a ListBarGroup that it has 
        ''' been changed.
        ''' </summary>
        ''' <param name="group">The ListBarGroup which has been
        ''' changed, or null the group has been removed.</param>
        ''' <param name="addRemove">Whether the effect of the
        ''' change will require the control to re-measured.</param>
        Protected Friend Sub GroupChanged(group As ListBarGroup, addRemove As Boolean)
            ' if we have changed the number of groups,
            ' we need to redraw the entire control,
            ' otherwise we just redraw this group
            If addRemove Then
                If group IsNot Nothing Then
                    group.SetButtonHeight(Me.Font)
                End If
                DoResize()
                PostResizeBarChanged()
            End If
            Invalidate()
        End Sub

        ''' <summary>
        ''' Internal notification from a ListBarItem that it has been
        ''' changed.
        ''' </summary>
        ''' <param name="item">The ListBarItem which has been changed, 
        ''' or null if the item has been removed.</param>
        ''' <param name="addRemove">Whether the effect of the control
        ''' will require the bar's contents to be remeasured.</param>
        Protected Friend Sub ItemChanged(item As ListBarItem, addRemove As Boolean)
            Dim selGroup As ListBarGroup = SelectedGroup
            Dim owningGroup As ListBarGroup = Nothing
            If item IsNot Nothing Then
                ' Which bar does it belong to
                For Each group As ListBarGroup In Me.m_groups
                    If group.Items.Contains(item) Then
                        owningGroup = group
                        Exit For
                    End If
                Next

                If owningGroup IsNot Nothing Then
                    Dim imageSize As New Size(32, 32)
                    If (owningGroup.View = ListBarGroupView.LargeIcons) OrElse (owningGroup.View = ListBarGroupView.LargeIconsOnly) Then
                        If Me.m_largeImageList IsNot Nothing Then
                            imageSize = Me.m_largeImageList.ImageSize
                        End If
                    Else
                        If m_smallImageList IsNot Nothing Then
                            imageSize = Me.m_smallImageList.ImageSize
                        Else
                            imageSize = New Size(16, 16)
                        End If
                    End If

                    ' Tell the item to size itself
                    item.SetSize(owningGroup.View, MyBase.Font, imageSize)
                Else
                    selGroup.SetLocationAndWidth(selGroup.ButtonLocation, selGroup.ButtonWidth)
                End If
            End If

            If selGroup IsNot Nothing Then
                If item Is Nothing Then
                    ' need to assume it does
                    If addRemove Then
                        DoResize()
                        PostResizeBarChanged()
                    End If
                    Invalidate()
                Else
                    If selGroup.Items.Contains(item) Then
                        ' yes it does.  We need to modify the 
                        ' display:
                        If addRemove Then
                            DoResize()
                            PostResizeBarChanged()
                        End If
                        Invalidate()
                    Else
                        If owningGroup Is Nothing Then
                            If addRemove Then
                                DoResize()
                                PostResizeBarChanged()
                            End If
                            Invalidate()
                        End If
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Ensures the scroll bar isn't irrelevantly 
        ''' begin displayed.
        ''' </summary>
        Private Sub PostResizeBarChanged()
            ' if the selected bar is scrolled,then we need 
            ' to check in the new arrangement that there isn't
            ' an unused space below the last item in the bar.

            ' if there is we should check if it is possible
            ' to scroll up by one or more items whilst still
            ' ensuring the last item currently visible in the
            ' view does not become any less visible.

            Dim selectedGroup__1 As ListBarGroup = SelectedGroup

            If selectedGroup__1 IsNot Nothing Then
                If selectedGroup__1.Items.Count > 0 Then
                    If selectedGroup__1.ScrollOffset <> 0 Then
                        Dim finished As Boolean = False
                        Dim nextGroup As ListBarGroup = Nothing
                        For i As Integer = Me.m_groups.IndexOf(selectedGroup__1) + 1 To Me.m_groups.Count - 1
                            If Me.m_groups(i).Visible Then
                                nextGroup = Me.m_groups(i)
                                Exit For
                            End If
                        Next

                        While Not finished
                            Dim lastItem As ListBarItem = selectedGroup__1.Items(selectedGroup__1.Items.Count - 1)
                            Dim rcItemLast As New Rectangle(lastItem.Location, New Size(Me.ClientRectangle.Width, lastItem.Height))
                            rcItemLast.Offset(0, selectedGroup__1.ScrollOffset + selectedGroup__1.ButtonLocation.Y + selectedGroup__1.ButtonHeight)

                            Dim rcView As New Rectangle(selectedGroup__1.ButtonLocation.X, selectedGroup__1.ButtonLocation.Y + selectedGroup__1.ButtonHeight, Me.ClientRectangle.Width, 0)
                            If nextGroup Is Nothing Then
                                rcView.Height = Me.ClientRectangle.Height - rcView.Top
                            Else
                                rcView.Height = nextGroup.ButtonLocation.Y - rcView.Top
                            End If

                            If rcItemLast.Bottom < rcView.Bottom + rcItemLast.Height Then
                                ' we can scroll up:
                                Scroll(1)
                            Else
                                finished = True
                            End If

                            If selectedGroup__1.ScrollOffset = 0 Then
                                finished = True
                            End If
                        End While
                    End If
                End If
            End If
        End Sub
        Protected Friend Sub ReSetGroup()
            Dim selectedGroup__1 As ListBarGroup = SelectedGroup
            selectedGroup__1.ScrollOffset = -1
        End Sub

        Private Sub InternalDragOverProcess(e As DragEventArgs, assumeItem As Boolean)
            ' TODO: this method requires refactoring

            Dim itemBefore As ListBarItem = Nothing
            Dim itemAfter As ListBarItem = Nothing
            Dim overEmptyBar As Boolean = False
            Dim newDragHoverOver As IMouseObject = Nothing
            Dim overGroup As Boolean = False

            ' see if the drag drop data contains a ListBarItem:		
            If (GetTypeOrSubClassFromData(e, GetType(ListBarItem)) IsNot Nothing) OrElse (assumeItem AndAlso (GetTypeOrSubClassFromData(e, GetType(ListBarGroup)) Is Nothing)) Then
                ' check if we're over an item:
                Dim pt As New Point(e.X, e.Y)
                pt = Me.PointToClient(pt)
                Dim obj As IMouseObject = HitTest(pt, True)
                newDragHoverOver = obj

                If obj IsNot Nothing Then
                    ' Do scrolling checks on this bar.  Scrolling
                    ' is rate limited to once per 250ms to assist
                    ' with usability
                    Dim diff As System.TimeSpan = DateTime.Now.Subtract(lastScrollTime)
                    If diff.Milliseconds > 250 Then

                        ' Firstly, check if we're over an actual scroll button:
                        If GetType(ListBarScrollButton).IsAssignableFrom(obj.[GetType]()) Then
                            ' scroll in the appropriate direction:
                            Dim scrollButton As ListBarScrollButton = DirectCast(obj, ListBarScrollButton)
                            Scroll(scrollButton, True)
                            lastScrollTime = DateTime.Now
                        Else
                            ' Otherwise, we may be within the boundary of the
                            ' scroll buttons:
                            If btnUp.Visible Then
                                Dim rcBtnUp As Rectangle = btnUp.Rectangle
                                rcBtnUp.X = 0
                                rcBtnUp.Width = Me.ClientRectangle.Width
                                If rcBtnUp.Contains(pt) Then
                                    Scroll(1, True)
                                    lastScrollTime = DateTime.Now
                                End If
                            End If
                            If btnDown.Visible Then
                                Dim rcBtnDown As Rectangle = btnDown.Rectangle
                                rcBtnDown.X = 0
                                rcBtnDown.Width = Me.ClientRectangle.Width
                                If rcBtnDown.Contains(pt) Then
                                    Scroll(-1, True)
                                    lastScrollTime = DateTime.Now
                                End If

                            End If
                        End If
                    End If


                    ' Now check for being over an item or an empty bar:
                    If GetType(ListBarItem).IsAssignableFrom(obj.[GetType]()) Then
                        Dim item As ListBarItem = DirectCast(obj, ListBarItem)
                        Dim objDragItem As Object = GetTypeOrSubClassFromData(e, GetType(ListBarItem))
                        Dim itemEqualsDragItem As Boolean = False
                        Dim dragItem As ListBarItem = Nothing
                        If objDragItem IsNot Nothing Then
                            dragItem = DirectCast(objDragItem, ListBarItem)
                            itemEqualsDragItem = item.Equals(dragItem)
                        End If

                        If Not itemEqualsDragItem Then
                            ' we're over an item.

                            ' Get the rectangle relative to the bar:
                            Dim rc As New Rectangle(item.Location, New Size(item.Width, item.Height))
                            ' Adjust the rectangle so it's relative to the control:
                            Dim selectedGroup__1 As ListBarGroup = SelectedGroup
                            rc.Offset(0, selectedGroup__1.ButtonLocation.Y + selectedGroup__1.ButtonHeight + selectedGroup__1.ScrollOffset)

                            ' The commented section here is an 8 pixel
                            ' margin from the top or bottom of an item,
                            ' as per the Outlook bar.  I found the control
                            ' more usable with a ListView style drag-drop
                            ' approach, where the before/after decision
                            ' is made depending on where you are relative
                            ' to the centre of the item.

                            ' 
                            '								** BEGIN: Outlook style drag-drop logic
                            '							if (((pt.Y - rc.Top) > -8) && ((pt.Y - rc.Top) < 8))
                            '							{
                            '								itemBefore = item;
                            '								// we can't go before the item which follows
                            '								// the one we're dragging:
                            '								if ((selectedGroup.Items.IndexOf(itemBefore) - 1) == 
                            '									selectedGroup.Items.IndexOf(dragItem))
                            '								{
                            '									itemBefore = null;
                            '								}
                            '
                            '							}
                            '
                            '							if (((rc.Bottom - pt.Y) > -8) && ((rc.Bottom - pt.Y) < 16))
                            '							{
                            '								itemAfter = item;
                            '								// we can't go after the item which is before
                            '								// the one we're dragging:
                            '								if ((selectedGroup.Items.IndexOf(itemAfter) + 1) == 
                            '									selectedGroup.Items.IndexOf(dragItem))
                            '								{
                            '									itemAfter = null;
                            '								}
                            '							}
                            '								** END: Outlook style drag drop logic
                            '								


                            ' ListView style drag insert point logic.
                            If (selectedGroup__1.View = ListBarGroupView.SmallIconsOnly) OrElse (selectedGroup__1.View = ListBarGroupView.LargeIconsOnly) Then
                                Dim distRight As Integer = Math.Abs(rc.Right - pt.X)
                                Dim distLeft As Integer = Math.Abs(pt.X - rc.Left)
                                If distRight < distLeft Then
                                    itemAfter = item
                                Else
                                    itemBefore = item
                                End If
                            Else
                                Dim distBottom As Integer = Math.Abs(rc.Bottom - pt.Y)
                                Dim distTop As Integer = Math.Abs(pt.Y - rc.Top)
                                If distBottom < distTop Then
                                    itemAfter = item
                                Else
                                    itemBefore = item
                                End If
                            End If

                            If itemAfter IsNot Nothing Then
                                ' we can't go after the item which is before
                                ' the one we're dragging:
                                '
                                '								if ((selectedGroup.Items.IndexOf(itemAfter) + 1) == 
                                '									selectedGroup.Items.IndexOf(dragItem))
                                '								{
                                '									itemAfter = null;
                                '								}
                                '								

                                If itemAfter IsNot Nothing Then
                                    ' check there isn't an appropriate item before:
                                    If selectedGroup__1.Items.IndexOf(itemAfter) < selectedGroup__1.Items.Count - 1 Then
                                        itemBefore = selectedGroup__1.Items(selectedGroup__1.Items.IndexOf(itemAfter) + 1)
                                    End If
                                End If
                            ElseIf itemBefore IsNot Nothing Then
                                ' we can't go before the item which follows
                                ' the one we're dragging:
                                '
                                '								if ((selectedGroup.Items.IndexOf(itemBefore) - 1) == 
                                '									selectedGroup.Items.IndexOf(dragItem))
                                '								{
                                '									itemBefore = null;
                                '								}
                                '								

                                If itemBefore IsNot Nothing Then
                                    ' check there isn't an appropriate item after:
                                    If selectedGroup__1.Items.IndexOf(itemBefore) > 0 Then
                                        itemAfter = selectedGroup__1.Items(selectedGroup__1.Items.IndexOf(itemBefore) - 1)
                                    End If
                                End If
                            End If
                        End If
                    ElseIf GetType(ListBarGroup).IsAssignableFrom(obj.[GetType]()) Then
                        overGroup = True

                        ' over a group
                        If dragHoverOver IsNot Nothing Then
                            If dragHoverOver.Equals(obj) Then
                                Dim overTime As System.TimeSpan = DateTime.Now.Subtract(dragHoverOverStartTime)
                                If overTime.Milliseconds > 350 Then
                                    ' we should select this group:
                                    dragHoverOver = Nothing
                                    SelectGroup(DirectCast(obj, ListBarGroup))
                                    ' Prevent the control from scrolling for a little bit.
                                    ' TODO
                                    ' Actually what we really want to do here is to say
                                    ' that unless the mouse moves > 4 pixels from this
                                    ' spot scrolling will not occur in the new bar
                                    lastScrollTime = DateTime.Now.AddMilliseconds(500)
                                End If
                            End If
                        End If
                    End If
                Else
                    ' we may be over the bar section:
                    Dim selectedGroup__1 As ListBarGroup = SelectedGroup

                    If selectedGroup__1 IsNot Nothing Then
                        If selectedGroup__1.Items.Count > 0 Then
                            ' we're not over an item.  Check if we're 
                            ' within the bar:
                            If obj Is Nothing Then
                                ' Check if the selected group is the last group in
                                ' the control:
                                Dim nextGroup As ListBarGroup = Nothing
                                For i As Integer = Me.m_groups.IndexOf(selectedGroup__1) + 1 To Me.m_groups.Count - 1
                                    If Me.m_groups(i).Visible Then
                                        nextGroup = Me.m_groups(i)
                                        Exit For
                                    End If
                                Next

                                If nextGroup Is Nothing Then
                                    ' If so the bar area extends from the bottom
                                    ' of the button rectangle to the bottom of the 
                                    ' control:
                                    If (pt.Y > (selectedGroup__1.ButtonLocation.Y + selectedGroup__1.ButtonHeight)) AndAlso (pt.Y < Me.ClientRectangle.Height) Then
                                        itemAfter = selectedGroup__1.Items(selectedGroup__1.Items.Count - 1)
                                    End If
                                Else
                                    ' Otherwise the bar area extends from the bottom
                                    ' of the button rectangle of this control to the
                                    ' top of the button rectangle of the next control:
                                    If (pt.Y > (selectedGroup__1.ButtonLocation.Y + selectedGroup__1.ButtonHeight)) AndAlso (pt.Y < nextGroup.ButtonLocation.Y) Then
                                        itemAfter = selectedGroup__1.Items(selectedGroup__1.Items.Count - 1)
                                    End If

                                End If
                            End If
                        Else
                            overEmptyBar = True
                        End If
                    End If
                End If
            ElseIf GetTypeOrSubClassFromData(e, GetType(ListBarGroup)) IsNot Nothing Then
                itemAfter = Nothing
                itemBefore = Nothing

                ' Here we check if we should drag the list bar
                ' into a new position
                ' check if we're over an item:
                Dim pt As New Point(e.X, e.Y)
                pt = Me.PointToClient(pt)
                Dim obj As IMouseObject = HitTest(pt, True)
                newDragHoverOver = obj

                If obj IsNot Nothing Then
                    If GetType(ListBarGroup).IsAssignableFrom(obj.[GetType]()) Then
                        overGroup = True

                        Dim dragGroup As ListBarGroup = DirectCast(GetTypeOrSubClassFromData(e, GetType(ListBarGroup)), ListBarGroup)

                        Dim dragOverGroup As ListBarGroup = DirectCast(obj, ListBarGroup)
                        If Not dragOverGroup.Equals(dragGroup) Then
                            Dim reSelect As Boolean = False
                            If dragGroup.Selected Then
                                reSelect = True
                            End If
                            Dim isLastGroup As Boolean = True
                            For i As Integer = m_groups.IndexOf(dragOverGroup) + 1 To Me.m_groups.Count - 1
                                If Me.m_groups(i).Visible Then
                                    isLastGroup = False
                                End If
                            Next
                            Me.m_groups.Remove(dragGroup)
                            If isLastGroup Then
                                Me.m_groups.Add(dragGroup)
                            Else
                                Me.m_groups.Insert(m_groups.IndexOf(dragOverGroup), dragGroup)
                            End If
                            If reSelect Then
                                For i As Integer = 0 To Me.m_groups.Count - 1
                                    Me.m_groups(i).Selected = False
                                Next
                                dragGroup.Selected = True
                            End If
                            DoResize()
                            Invalidate()
                        End If
                    End If

                End If
            End If

            ' Now check if we have any drag/drop insert position:
            If (itemBefore IsNot Nothing) OrElse (itemAfter IsNot Nothing) OrElse (overEmptyBar) Then
                e.Effect = DragDropEffects.Move

                Dim newInsertPoint As New ListBarDragDropInsertPoint(itemBefore, itemAfter, overEmptyBar)

                ' do we currently have an insert point?
                If dragInsertPoint IsNot Nothing Then
                    If dragInsertPoint.CompareTo(newInsertPoint) = 0 Then
                        ' we have nothing to do
                        newInsertPoint = Nothing
                    End If
                End If

                If newInsertPoint IsNot Nothing Then
                    Trace.WriteLine("Drag Insert Point has changed")
                    dragInsertPoint = newInsertPoint
                    Invalidate()
                End If
            Else
                If overGroup Then
                    e.Effect = DragDropEffects.Move
                Else
                    e.Effect = DragDropEffects.None
                End If

                ' Clear the drag insert point if it's set:
                If dragInsertPoint IsNot Nothing Then
                    dragInsertPoint = Nothing
                    ' redraw the control:
                    Invalidate()
                End If
            End If

            If (newDragHoverOver IsNot Nothing) AndAlso (dragHoverOver IsNot Nothing) Then
                If Not newDragHoverOver.Equals(dragHoverOver) Then
                    dragHoverOver = newDragHoverOver
                    dragHoverOverStartTime = DateTime.Now
                    ' else we keep the drag hover over time.
                End If
            Else
                dragHoverOver = newDragHoverOver
                dragHoverOverStartTime = DateTime.Now
            End If

        End Sub
#End Region

#Region "API"
        ''' <summary>
        ''' Called by the control's internal sizing mechanism.
        ''' Returns the size of a <see cref="ListBarGroup"/> button
        ''' rectangle.
        ''' </summary>
        ''' <param name="group">The <see cref="ListBarGroup"/> to get the 
        ''' button width for.</param>
        ''' <returns>The width of the button.</returns>
        Protected Overridable Function GetGroupButtonWidth(group As ListBarGroup) As Integer
            Return Me.ClientRectangle.Width - 2
        End Function

        ''' <summary>
        ''' Gets/sets the default <see cref="System.Drawing.Font"/> used to 
        ''' render text in the control.
        ''' </summary>
        <Description("Gets/sets the Font used to render the text in this control.")> _
        Public Overrides Property Font() As System.Drawing.Font
            Get
                Return MyBase.Font
            End Get
            Set(value As System.Drawing.Font)
                MyBase.Font = value

                ' Force all of the ListBar items to be measured
                For Each group As ListBarGroup In m_groups
                    GroupChanged(group, True)
                Next
            End Set
        End Property
        Dim hashtableObj = New Hashtable
        Public ReadOnly Property ContainsGroupInhash(ByVal sTaskGroupKey As String, ByVal sTaskGroupCaption As String) As Boolean
            Get
                If Not hashtableObj.ContainsKey(sTaskGroupKey) Then
                    hashtableObj.Add(sTaskGroupKey, sTaskGroupCaption)
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property
        'developer guide no. 103 Modified By prashant mishra on 04-07-2014(For Refreshing the List bar on F5 Button press )
        Public ReadOnly Property ClearHashGroup() As Boolean
            Get
                hashtableObj.Clear()
                Return True
            End Get
        End Property

        ''' <summary>
        ''' Gets/sets whether items are selected on MouseDown,
        ''' rather than on MouseUp, which is the default.	
        ''' </summary>
        <Description("Gets/sets whether items are selected on MouseDown, rather than on MouseUp.")> _
        Public Property SelectOnMouseDown() As Boolean
            Get
                Return Me.m_selectOnMouseDown
            End Get
            Set(value As Boolean)
                Me.m_selectOnMouseDown = value
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets whether items will be dragged 
        ''' in the control automatically.  The default
        ''' is <c>True</c>.
        ''' </summary>
        <Description("Gets/sets whether items will be dragged in the control automatically.  The default is True.")> _
        Public Property AllowDragItems() As Boolean
            Get
                Return Me.m_allowDragItems
            End Get
            Set(value As Boolean)
                Me.m_allowDragItems = value
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets whether groups will be dragged 
        ''' in the control automatically.  The default
        ''' is <c>True</c>. (Note in MS Outlook
        ''' Groups cannot be reordered by dragging, but
        ''' they can in VS.NET).
        ''' </summary>
        <Description("Gets/sets whether groups can be dragged automatically in the control.  The default is True.")> _
        Public Property AllowDragGroups() As Boolean
            Get
                Return Me.m_allowDragGroups
            End Get
            Set(value As Boolean)
                Me.m_allowDragGroups = value
            End Set
        End Property

        ''' <summary>
        ''' Sets the groups object associated with this control
        ''' to a new group collection.
        ''' </summary>
        ''' <param name="groups">The <see cref="ListBarGroupCollection"/> object holding
        ''' the new collection of groups to associate with this control.</param>
        <Description("Sets the internal collection holding the Groups associated with this control to a new object.")> _
        Public Sub SetGroups(groups As ListBarGroupCollection)
            Me.BeginUpdate()
            groups.SetOwner(Me)
            Me.EndUpdate()
            Me.m_groups = groups
            DoResize()
            Me.Invalidate()
        End Sub

        ''' <summary>
        ''' Gets the item which is currently being edited, if any,
        ''' otherwise returns null.
        ''' </summary>
        <Description("Gets the item which is currently being edited, if any, otherwise returns null.")> _
        Public ReadOnly Property EditItem() As ListBarItem
            Get
                Return Me.m_editItem
            End Get
        End Property

        ''' <summary>
        ''' Gets the item which is currently being edited, if any,
        ''' otherwise returns null.
        ''' </summary>
        <Description("Gets the items count of group.")> _
        Public ReadOnly Property GroupCount() As Integer
            Get
                Return Me.m_groups.Count
            End Get
        End Property
        <Description("Gets the group by key.")> _
        Public ReadOnly Property GetGroupByKey(ByVal gkey As String) As ListBarGroup
            Get
                Return Me.m_groups(gkey)
            End Get
        End Property

        <Description("Gets weather group contains key or not.")> _
        Public ReadOnly Property GroupContainsKey(ByVal gkey As ListBarGroup) As Boolean
            Get
                Return Me.m_groups.Contains(gkey)
            End Get
        End Property


        ''' <summary>
        ''' Prevents the control from drawing until the 
        ''' <see cref="EndUpdate"/> method is called.
        ''' </summary>
        <Description("Prevents the control from drawing until the EndUpdate method is called.")> _
        Public Sub BeginUpdate()
            Me.redraw = False
        End Sub

        ''' <summary>
        ''' Resumes drawing of the control after drawing was suspended by the 
        ''' <see cref="BeginUpdate"/> method.		
        ''' </summary>
        <Description("Resumes drawing of the control after drawing was suspended by the BeginUpdate method.")> _
        Public Sub EndUpdate()
            Me.redraw = True
            DoResize()
            Invalidate()
        End Sub

        ''' <summary>
        ''' Renders the control when a new group is being scrolled
        ''' into view.
        ''' </summary>
        ''' <param name="pe">The arguments associated with the paint
        ''' event.</param>
        <Description("Renders the control as a new group is being scrolled into view")> _
        Protected Overridable Sub RenderScrollNewGroup(pe As PaintEventArgs)
            Dim lastBar As Integer = 0
            Dim currentNext As Integer = Me.m_groups.Count - 1
            Dim newNext As Integer = Me.m_groups.Count - 1
            For i As Integer = 0 To Me.m_groups.Count - 1
                If Me.m_groups(i).Visible Then
                    If i > lastBar Then
                        lastBar = i
                    End If
                    If (i > Me.indexCurrent) AndAlso (i < currentNext) Then
                        currentNext = i
                    End If
                    If (i > Me.indexNew) AndAlso (i < newNext) Then
                        newNext = i
                    End If
                End If
            Next

            Dim currentBar As ListBarGroup = Me.m_groups(Me.indexCurrent)
            Dim newBar As ListBarGroup = Me.m_groups(Me.indexNew)

            ' get the rectangle for currentBar:
            Dim currentBarBounds As New Rectangle(currentBar.ButtonLocation, New Size(currentBar.ButtonWidth, 0))
            ' the height of the current bar rect is the height of the control
            ' or the top of the rectangle of the next button along:
            If Me.indexCurrent = lastBar Then
                currentBarBounds.Height = Me.ClientRectangle.Height - currentBarBounds.Top
            Else
                currentBarBounds.Height = Me.m_groups(currentNext).ButtonLocation.Y - currentBarBounds.Top
            End If

            ' get the rectangle for newBar:
            Dim newBarBounds As New Rectangle(newBar.ButtonLocation, New Size(newBar.ButtonWidth, 0))
            ' the height of the new bar is the height of the control or
            ' the top of the rectangle of the next bar along:
            If Me.indexNew = lastBar Then
                newBarBounds.Height = Me.ClientRectangle.Height - newBarBounds.Top
            Else
                newBarBounds.Height = Me.m_groups(newNext).ButtonLocation.Y - newBarBounds.Top
            End If

            ' Draw the current bar contents:
            currentBar.DrawBar(pe.Graphics, currentBarBounds, (If(currentBar.View = ListBarGroupView.LargeIcons, m_largeImageList, m_smallImageList)), Me.Font, Me.m_drawStyle, Me.Enabled)

            ' Draw the new bar contents:
            newBar.DrawBar(pe.Graphics, newBarBounds, (If(newBar.View = ListBarGroupView.LargeIcons, m_largeImageList, m_smallImageList)), Me.Font, Me.m_drawStyle, Me.Enabled)

            ' Draw the buttons:
            For Each group As ListBarGroup In Me.m_groups
                group.DrawButton(pe.Graphics, Me.Font, Me.Enabled)
            Next

            ' Draw the border:
            RenderControlBorder(pe.Graphics)
        End Sub

        ''' <summary>
        ''' Renders the control given the object passed to a Paint event.
        ''' </summary>
        ''' <param name="pe">The arguments associated with the paint
        ''' event.</param>
        Protected Overridable Sub Render(pe As PaintEventArgs)
            If Me.redraw Then

                ' background - does not need to be painted
                ' as the control does it itself

                ' draw the control elements:
                If m_groups.Count > 0 Then
                    ' First draw the items in the selected group,
                    ' if any:
                    Dim selectedGroup__1 As ListBarGroup = SelectedGroup

                    If (selectedGroup__1 IsNot Nothing) AndAlso (selectedGroup__1.Visible) Then
                        ' Draw the items in the group:
                        selectedGroup__1.DrawBar(pe.Graphics, rcListView, (If((selectedGroup__1.View = ListBarGroupView.LargeIcons OrElse selectedGroup__1.View = ListBarGroupView.LargeIconsOnly), m_largeImageList, m_smallImageList)), Me.Font, Me.m_drawStyle, Me.Enabled)

                        ' Render the drag-drop insertion point, if any:
                        RenderDragInsertPoint(pe.Graphics, selectedGroup__1)
                    End If

                    ' draw the scroll buttons:
                    Dim defaultBackColor As Color = Color.FromKnownColor(KnownColor.Control)
                    If Me.m_drawStyle = ListBarDrawStyle.ListBarDrawStyleNormal Then
                        If Not Me.BackColor.Equals(Color.FromKnownColor(KnownColor.ControlDark)) Then
                            defaultBackColor = Me.BackColor
                        End If
                    ElseIf Me.DrawStyle = ListBarDrawStyle.ListBarDrawStyleOfficeXP Then
                        defaultBackColor = Me.BackColor
                    End If

                    btnUp.DrawItem(pe.Graphics, defaultBackColor, Me.Enabled)
                    btnDown.DrawItem(pe.Graphics, defaultBackColor, Me.Enabled)

                    ' Now draw the group buttons, if any:
                    For Each group As ListBarGroup In Me.m_groups
                        If group.Visible Then
                            group.DrawButton(pe.Graphics, Me.Font, Me.Enabled)
                        End If
                    Next
                End If

                ' border:
                RenderControlBorder(pe.Graphics)
            End If
        End Sub

        ''' <summary>
        ''' Draw a border around the control.  The default
        ''' implementation draws a 1 pixel inset border.
        ''' </summary>
        ''' <param name="gfx">The graphics object to drawn onto.</param>
        Protected Overridable Sub RenderControlBorder(gfx As Graphics)
            ' draw the control's border
            Dim darkPen As New Pen(CustomBorderColor.ColorDark(Me.BackColor))
            Dim lightPen As New Pen(CustomBorderColor.ColorLightLight(Me.BackColor))
            gfx.DrawLine(darkPen, Me.ClientRectangle.Left, Me.ClientRectangle.Bottom - 2, Me.ClientRectangle.Left, Me.ClientRectangle.Top)
            gfx.DrawLine(darkPen, Me.ClientRectangle.Left, Me.ClientRectangle.Top, Me.ClientRectangle.Right - 2, Me.ClientRectangle.Top)
            gfx.DrawLine(lightPen, Me.ClientRectangle.Right - 1, Me.ClientRectangle.Top, Me.ClientRectangle.Right - 1, Me.ClientRectangle.Bottom - 1)
            gfx.DrawLine(lightPen, Me.ClientRectangle.Right - 1, Me.ClientRectangle.Bottom - 1, Me.ClientRectangle.Left, Me.ClientRectangle.Bottom - 1)
            darkPen.Dispose()
            lightPen.Dispose()
        End Sub

        ''' <summary>
        ''' Draws the drag insert point, if any.  The drag insert point is
        ''' drawn using the same style as the Windows XP ListView drag
        ''' insert point.
        ''' 
        ''' Note that the Outlook ListBar draws a single pixel drag insert
        ''' point rather than a double width one.  I preferred the ListView 
        ''' XP style so went with this.  The code can be overridden to
        ''' use a single pixel border instead if desired. 
        ''' </summary>
        ''' <param name="gfx">The graphics object to draw onto.</param>
        ''' <param name="selectedGroup">The currently selected ListBarGroup.</param>
        Protected Overridable Sub RenderDragInsertPoint(gfx As Graphics, selectedGroup As ListBarGroup)
            If dragInsertPoint IsNot Nothing Then
                Dim itemAfter As ListBarItem = dragInsertPoint.ItemAfter
                Dim itemBefore As ListBarItem = dragInsertPoint.ItemBefore

                Dim offset As Integer = (If(selectedGroup.View = ListBarGroupView.LargeIcons, 2, 0))

                If itemAfter IsNot Nothing Then
                    ' Get the bounding rectangle of the item after:
                    Dim rcItem As New Rectangle(itemAfter.Location, New Size(itemAfter.Width, itemAfter.Height))
                    ' adjust the rectangle so it corresponds to the display:
                    rcItem.Offset(0, selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight + selectedGroup.ScrollOffset)

                    ' Draw the insertion point line:
                    If (selectedGroup.View = ListBarGroupView.SmallIconsOnly) OrElse (selectedGroup.View = ListBarGroupView.LargeIconsOnly) Then
                        gfx.DrawLine(SystemPens.WindowText, rcItem.Right, rcItem.Top + 2, rcItem.Right, rcItem.Bottom - 1)
                        gfx.DrawLine(SystemPens.WindowText, rcItem.Right + 1, rcItem.Top + 2, rcItem.Right + 1, rcItem.Bottom - 1)

                        ' Draw triangles:
                        If itemBefore IsNot Nothing Then
                            ' left triangles:
                            ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, New Point(rcItem.Right + 1, rcItem.Top + 2), 5, 5)
                            ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, New Point(rcItem.Right + 1, rcItem.Bottom), 5, -6)
                        End If

                        ' right triangles:
                        ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, New Point(rcItem.Right, rcItem.Top + 2), -4, 4)

                        ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, New Point(rcItem.Right, rcItem.Bottom), -5, -6)
                    Else
                        gfx.DrawLine(SystemPens.WindowText, rcItem.Left + 7, rcItem.Bottom + offset, rcItem.Right - 7, rcItem.Bottom + offset)
                        gfx.DrawLine(SystemPens.WindowText, rcItem.Left + 7, rcItem.Bottom + offset + 1, rcItem.Right - 7, rcItem.Bottom + offset + 1)

                        ' Draw the triangles:
                        If itemBefore IsNot Nothing Then
                            ' below triangles:
                            ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, New Point(rcItem.Left + 7, rcItem.Bottom + offset + 1), 10, 5)
                            ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, New Point(rcItem.Right - 6, rcItem.Bottom + offset + 1), -10, 5)
                        End If

                        ' above triangles
                        ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, New Point(rcItem.Left + 7, rcItem.Bottom + offset), 10, -5)

                        ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, New Point(rcItem.Right - 6, rcItem.Bottom + offset), -10, -5)
                    End If
                Else
                    ' before the first item:

                    ' Get the bounding rectangle of the item after:
                    Dim rcItem As Rectangle
                    If itemBefore IsNot Nothing Then
                        rcItem = New Rectangle(itemBefore.Location, New Size(Me.Width, itemBefore.Height))
                        ' adjust the rectangle so it corresponds to the display:
                        rcItem.Offset(0, selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight + selectedGroup.ScrollOffset)
                    Else
                        rcItem = New Rectangle(selectedGroup.ButtonLocation, New Size(selectedGroup.ButtonWidth, selectedGroup.ButtonHeight))
                        rcItem.Offset(0, selectedGroup.ButtonHeight)
                    End If

                    ' draw the insertion point line:
                    If (selectedGroup.View = ListBarGroupView.SmallIconsOnly) OrElse (selectedGroup.View = ListBarGroupView.LargeIconsOnly) Then
                        gfx.DrawLine(SystemPens.WindowText, rcItem.Left + 1, rcItem.Top + 2, rcItem.Left + 1, rcItem.Bottom - 1)
                        gfx.DrawLine(SystemPens.WindowText, rcItem.Left + 2, rcItem.Top + 2, rcItem.Left + 2, rcItem.Bottom - 1)

                        ' left triangles:
                        ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, New Point(rcItem.Left + 2, rcItem.Top + 2), 5, 5)
                        ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, New Point(rcItem.Left + 2, rcItem.Bottom), 5, -6)
                    Else
                        gfx.DrawLine(SystemPens.WindowText, rcItem.Left + 7, rcItem.Top + offset, rcItem.Right - 7, rcItem.Top + offset)
                        gfx.DrawLine(SystemPens.WindowText, rcItem.Left + 7, rcItem.Top + offset + 1, rcItem.Right - 7, rcItem.Top + offset + 1)

                        ' Now draw the triangles:
                        ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, New Point(rcItem.Left + 7, rcItem.Top + offset + 1), 10, 5)
                        ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, New Point(rcItem.Right - 6, rcItem.Top + offset + 1), -10, 5)
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Creates a new instance of the ListBarGroupCollection used by this
        ''' control to store the ListBarGroups.  Fired during control 
        ''' initialisation.
        ''' </summary>
        ''' <returns>A new instance of the ListBarGroupCollection to be used
        ''' by the control to store ListBarGroups.</returns>
        Protected Overridable Function CreateListBarGroupCollection() As ListBarGroupCollection
            Return New ListBarGroupCollection(Me)
        End Function

        ''' <summary>
        ''' Creates a new instance of a ListBarScrollButton used by this control
        ''' to draw the scroll buttons.  Fired during control initialisation
        ''' </summary>
        ''' <param name="buttonType">The type of scroll button (Up or Down)
        ''' to create</param>
        ''' <returns>A new ListBarScrollButton which is drawn when a ListBar
        ''' contains more items than can be displayed.</returns>
        Protected Overridable Function CreateListBarScrollButton(buttonType As ListBarScrollButton.ListBarScrollButtonType) As ListBarScrollButton
            Return New ListBarScrollButton(buttonType)
        End Function

        ''' <summary>
        ''' Gets/sets how the ListBar control will be drawn.
        ''' </summary>
        <Description("Gets/sets the style used to draw the ListBar control.")> _
        Public Property DrawStyle() As ListBarDrawStyle
            Get
                Return Me.m_drawStyle
            End Get
            Set(value As ListBarDrawStyle)
                Me.m_drawStyle = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the collection of groups in the ListBar.
        ''' </summary>
        <Description("Gets the collection of groups in the ListBar.")>
        Public ReadOnly Property Groups() As ListBarGroupCollection
            Get
                Return Me.m_groups
            End Get
        End Property

        ''' <summary>
        ''' Gets/sets the tooltip object associated with this control.
        ''' The control does not generate its own internal Tooltips
        ''' and instead relies on an external ToolTip object to
        ''' display tooltips.
        ''' </summary>
        <Description("Gets/sets the tooltip object which will be used to show tooltips for groups and items in the control.")> _
        Public Property ToolTip() As ToolTip
            Get
                Return Me.m_toolTip
            End Get
            Set(value As ToolTip)
                Me.m_toolTip = value
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets the large icon ImageList to be used for items 
        ''' with the <see cref="ListBarGroupView.LargeIcons"/> and 
        ''' <see cref="ListBarGroupView.LargeIconsOnly"/> view.
        ''' </summary>
        <Description("Gets/sets the large icon ImageList to be used for items in groups with the LargeIcons or LargeIconsOnly view.")> _
        Public Property LargeImageList() As ImageList
            Get
                Return Me.m_largeImageList
            End Get
            Set(value As ImageList)
                Me.m_largeImageList = value
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets the small icon ImageList to be used for ListBar groups
        ''' using the <see cref="ListBarGroupView.SmallIcons"/> or 
        ''' <see cref="ListBarGroupView.SmallIconsOnly "/> view.
        ''' </summary>
        <Description("Gets/sets the small icon ImageList to be used for items in groups with the SmallIcons or SmallIconsOnly view.")> _
        Public Property SmallImageList() As ImageList
            Get
                Return Me.m_smallImageList
            End Get
            Set(value As ImageList)
                Me.m_smallImageList = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the currently selected group in the ListBar control,
        ''' if any.
        ''' </summary>
        <Description("Gets the currently selected group in the ListBar control.")> _
        Public Overridable Property SelectedGroup() As ListBarGroup
            Get
                Dim selGroup As ListBarGroup = Nothing
                If Me.m_groups.Count > 0 Then
                    For Each group As ListBarGroup In Me.m_groups
                        If group.Selected Then
                            selGroup = group
                            Exit For
                        End If
                    Next
                End If

                If Not CurrentSelected Is Nothing Then
                    For Each group As ListBarGroup In Me.m_groups
                        If group.Caption = CurrentSelected.Caption Then
                            group.Selected = True
                            selGroup = group
                        Else
                            group.Selected = False
                        End If
                    Next

                End If
                Return selGroup
                'Return m_selectedGroup
            End Get
            Set(value As ListBarGroup)
                Dim selGroup As ListBarGroup = Nothing
                If Me.m_groups.Count > 0 Then
                    For Each group As ListBarGroup In Me.m_groups
                        If group.Caption = value.Caption Then
                            group.Selected = True
                            m_selectedGroup = group
                        Else
                            group.Selected = False
                        End If
                    Next

                End If
            End Set
        End Property

        Public Property CurrentSelected() As ListBarGroup
            Get
                Return m_selectedGroup
            End Get
            Set(value As ListBarGroup)
                m_selectedGroup = value
            End Set
        End Property
        ''' <summary>
        ''' Returns the favorite caption,
        ''' if any.
        ''' </summary>
        <Description("Gets the currently selected group in the ListBar control.")>
        Public Property CurrentGroupCaption() As String
            Get
                Return Me.m_currentGroupCaption
            End Get
            Set(value As String)
                Me.m_currentGroupCaption = value
            End Set
        End Property


#End Region

#Region "Component Designer generated code"
        ''' <summary> 
        ''' Required method for Designer support - do not modify 
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.txtEdit = New System.Windows.Forms.TextBox()
            Me.SuspendLayout()
            ' 
            ' txtEdit
            ' 
            Me.txtEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtEdit.Location = New System.Drawing.Point(60, 92)
            Me.txtEdit.Multiline = True
            Me.txtEdit.Name = "txtEdit"
            Me.txtEdit.Size = New System.Drawing.Size(80, 44)
            Me.txtEdit.TabIndex = 0
            Me.txtEdit.Text = ""
            Me.txtEdit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            Me.txtEdit.Visible = False
            AddHandler Me.txtEdit.TextChanged, New System.EventHandler(AddressOf Me.txtEdit_TextChanged)
            ' 
            ' ListBar
            ' 
            Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.txtEdit})
            Me.Name = "ListBar"
            Me.ResumeLayout(False)

        End Sub
#End Region

    End Class
#End Region

#Region "ListBarGroupCollection class"
    ''' <summary>
    ''' A class to hold the collection of groups in the ListBar control.
    ''' </summary>
    <SerializableAttribute> _
    Public Class ListBarGroupCollection
        Inherits CollectionBase
        Implements ISerializable
        ''' <summary>
        ''' The ListBar which owns this collection
        ''' </summary>
        Private owner As ListBar = Nothing

        ''' <summary>
        ''' Adds a new <see cref="ListBarGroup"/> to the control.
        ''' </summary>
        ''' <param name="group">The group to add to the control</param>
        ''' <returns>The index at which the group was added.</returns>
        <Description("Adds a new ListBarGroup to the control.")> _
        Public Overridable Function Add(group As ListBarGroup) As Integer
            Dim ret As Integer = Me.InnerList.Add(group)
            group.SetOwner(owner)
            Return ret
        End Function

        ''' <summary>
        ''' Adds a new <see cref="ListBarGroup"/> with the specified caption to
        ''' the control and returns a reference to it.
        ''' </summary>
        ''' <param name="caption">The caption for the new ListBarGroup.</param>
        ''' <returns>The ListBarGroup added to the control.</returns>
        <Description("Adds a new ListBarGroup with the specified caption to the control and returns a reference to it.")> _
        Public Overridable Function Add(caption As String) As ListBarGroup
            Dim group As New ListBarGroup(caption)
            Me.InnerList.Add(group)
            group.SetOwner(owner)
            Return group
        End Function

        ''' <summary>
        ''' Adds a series of <see cref="ListBarGroup"/> objectss based on the supplied captions.
        ''' </summary>
        ''' <param name="captions">The array of captions to use when creating
        ''' the <see cref="ListBarGroup"/> objects.</param>
        <Description("Adds a series of ListBarGroups with the specified captions to the control.")> _
        Public Overridable Sub AddRange(captions As String())
            For Each caption As String In captions
                Add(caption)
            Next
        End Sub

        ''' <summary>
        ''' Adds a range of previously defined <see cref="ListBarGroup" /> objects.
        ''' </summary>
        ''' <param name="values">The array of ListBarGroups to add
        ''' to the control.</param>
        <Description("Adds a range of previously defined ListBarGroup objects.")> _
        Public Overridable Sub AddRange(values As ListBarGroup())
            For Each group As ListBarGroup In values
                Me.InnerList.Add(group)
                group.SetOwner(owner)
            Next
        End Sub

        ''' <summary>
        ''' Determines whether a <see cref="ListBarGroup"/> element is contained within 
        ''' the control's collection of groups.
        ''' </summary>
        ''' <param name="group">The ListBarGroup to check if present.</param>
        ''' <returns>True if the ListBarGroup is contained within the control's
        ''' collection, False otherwise.</returns>
        <Description("Determins whether a ListBarGroup element is contained within the control's collection of groups.")> _
        Public Overridable Function Contains(group As ListBarGroup) As Boolean
            Return Me.InnerList.Contains(group)
        End Function

        ''' <summary>
        ''' Gets the 0-based index of the specified <see cref="ListBarGroup"/> within this
        ''' collection.
        ''' </summary>
        ''' <param name="group">The group to find the index for.</param>
        ''' <returns>The 0-based index of the group, if found, otherwise - 1.</returns>
        <Description("Gets the 0-based index of the specified ListBarGroup within this collection.")> _
        Public Overridable Function IndexOf(group As ListBarGroup) As Integer
            Return Me.InnerList.IndexOf(group)
        End Function

        ''' <summary>
        ''' Inserts a group at the specified 0-based index in the collection
        ''' of groups.
        ''' </summary>
        ''' <param name="index">The 0-based index to insert the group at.</param>
        ''' <param name="group">The ListBarGroup to add.</param>
        <Description("(Inserts a group at the specified 0-based index in the collection of groups.")> _
        Public Overridable Sub Insert(index As Integer, group As ListBarGroup)
            Me.InnerList.Insert(index, group)
            group.SetOwner(owner)
        End Sub
        ''' <summary>
        ''' Inserts a group immediately before the specified <see cref="ListBarGroup"/>.
        ''' </summary>
        ''' <param name="groupBefore">ListBarGroup to insert before.</param>
        ''' <param name="group">Group to insert.</param>
        <Description("(Inserts a group immediately before the specified ListBarGroup object.")> _
        Public Overridable Sub InsertBefore(groupBefore As ListBarGroup, group As ListBarGroup)
            Me.InnerList.Insert(Me.InnerList.IndexOf(groupBefore), group)
            group.SetOwner(owner)
        End Sub
        ''' <summary>
        ''' Inserts a <see cref="ListBarGroup"/> immediately after the specified ListBarGroup.
        ''' </summary>
        ''' <param name="groupAfter">ListBarGroup to insert after.</param>
        ''' <param name="group">Group to insert.</param>
        <Description("(Inserts a group immediately after the specified ListBarGroup object.")> _
        Public Overridable Sub InsertAfter(groupAfter As ListBarGroup, group As ListBarGroup)
            Dim index As Integer = Me.InnerList.IndexOf(groupAfter)
            If index = Me.InnerList.Count - 1 Then
                Me.Add(group)
            Else
                Me.Insert(index + 1, group)
            End If
        End Sub


        ''' <summary>
        ''' Removes the specified <see cref="ListBarGroup"/>.
        ''' </summary>
        ''' <param name="group">The group to remove.</param>
        <Description("(Removes the specified ListBarGroup object.")> _
        Public Overridable Sub Remove(group As ListBarGroup)
            Me.InnerList.Remove(group)
            NotifyOwner(group, True)
        End Sub

        ''' <summary>
        ''' Gets the <see cref="ListBarGroup"/> at the specified 0-based index.
        ''' </summary>
        <Description("(Gets the ListBarGroup at the specified 0-based index.")> _
        Default Public ReadOnly Property Item(index As Integer) As ListBarGroup
            Get
                Return DirectCast(Me.InnerList(index), ListBarGroup)
            End Get
        End Property

        ''' <summary>
        ''' Gets the <see cref="ListBarGroup"/> with the specified string key.
        ''' </summary>
        <Description("(Gets the ListBarGroup with the specified string key.")> _
        Default Public ReadOnly Property Item(key As String) As ListBarGroup
            Get
                Dim ret As ListBarGroup = Nothing
                For Each group As ListBarGroup In Me.InnerList
                    If group.Key.Equals(key) Then
                        ret = group
                        Exit For
                    End If
                Next
                Return ret
            End Get
        End Property
        <Description("(Gets the ListBarGroup item with the specified string key.")> _
        Public ReadOnly Property ItemCaption(key As String) As ListBarGroup
            Get
                Dim ret As ListBarGroup = Nothing
                For Each group As ListBarGroup In Me.InnerList
                    If group.Caption.Equals(key) Then
                        ret = group
                        Exit For
                    End If
                Next
                Return ret
            End Get
        End Property

        ''' <summary>
        ''' Notifies the owning ListBar control of any changes to a group.
        ''' </summary>
        ''' <param name="group">The Group which has changed.</param>
        ''' <param name="addRemove">Whether the control should resize
        ''' all groups associated with the ListBar.</param>
        Protected Overridable Sub NotifyOwner(group As ListBarGroup, addRemove As Boolean)
            If Me.owner IsNot Nothing Then
                Me.owner.GroupChanged(group, addRemove)
            End If
        End Sub

        ''' <summary>
        ''' Notifies the control after clearing all groups.
        ''' </summary>
        Protected Overrides Sub OnClearComplete()
            NotifyOwner(Nothing, True)
        End Sub

        ''' <summary>
        ''' Notifies the control after inserting a new ListBarGroup.
        ''' </summary>
        Protected Overrides Sub OnInsertComplete(index As System.Int32, value As System.Object)
            NotifyOwner(DirectCast(value, ListBarGroup), True)
        End Sub

        ''' <summary>
        ''' Notifies the control after removing a new ListBarGroup.
        ''' </summary>
        Protected Overrides Sub OnRemoveComplete(index As System.Int32, value As System.Object)
            NotifyOwner(Nothing, True)
        End Sub

        ''' <summary>
        ''' Notifies the control after setting a ListBarGroup to another ListBarGroup.
        ''' </summary>
        Protected Overrides Sub OnSetComplete(index As System.Int32, oldValue As System.Object, newValue As System.Object)
            NotifyOwner(DirectCast(newValue, ListBarGroup), False)
        End Sub

        ''' <summary>
        ''' 
        ''' TODO: This method has not been implemented yet.
        ''' 
        ''' Populates a System.Runtime.Serialization.SerializationInfo object with the 
        ''' data needed to serialize this object.
        ''' </summary>
        ''' <param name="info">The System.Runtime.Serialization.SerializationInfo 
        ''' to populate with data.</param>
        ''' <param name="context">The destination (see 
        ''' System.Runtime.Serialization.StreamingContext) for this serialization.</param>
        Public Overridable Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData
            ' TODO
        End Sub

        ''' <summary>
        ''' Enables a deserialized object graph to be associated with a ListBar
        ''' control.
        ''' </summary>
        ''' <param name="owner">The ListBar control which will own
        ''' this collection of items.</param>
        Public Overridable Sub SetOwner(owner As ListBar)
            Me.owner = owner
            For Each group As ListBarGroup In Me.InnerList
                group.SetOwner(owner)
            Next
        End Sub

        ''' <summary>
        ''' 
        ''' TODO: This method has not been implemented yet.
        ''' 
        ''' Constructs this object from a serialized representation.
        ''' </summary>
        ''' <param name="info">The System.Runtime.Serialization.SerializationInfo 
        ''' containing the serialized data to build this object from.</param>
        ''' <param name="context">The destination (see 
        ''' System.Runtime.Serialization.StreamingContext) for this serialization.</param>

        Public Sub New(info As SerializationInfo, context As StreamingContext)
        End Sub

        ''' <summary>
        ''' Creates a new instance of the ListBarGroup collection and associates
        ''' it with the control which owns it.
        ''' </summary>
        ''' <param name="owner">The owning ListBar control.</param>
        Public Sub New(owner As ListBar)
            Me.owner = owner
        End Sub

    End Class
#End Region

#Region "ListBarGroup class"
    ''' <summary>
    ''' A <c>ListBarGroup</c> is a bar within a <see cref="ListBar"/> control.
    ''' A bar can either contain items or it can contain a Windows
    ''' Forms control.
    ''' </summary>
    <SerializableAttribute> _
    Public Class ListBarGroup
        Implements IMouseObject
        Implements ISerializable
        ''' <summary>
        ''' The owning control.
        ''' </summary>
        Private m_owner As ListBar = Nothing
        ''' <summary>
        ''' The caption of the group.
        ''' </summary>
        Private m_caption As String = ""
        ''' <summary>
        ''' Whether the item is selected or not.
        ''' </summary>
        Private m_selected As Boolean = False
        ''' <summary>
        ''' The tooltip text for this group.
        ''' </summary>
        Private m_toolTipText As String = ""
        ''' <summary>
        ''' The string key to associate with this item.
        ''' </summary>
        Private m_key As String = ""
        ''' <summary>
        ''' User-defined data to associate with this item.
        ''' </summary>
        Private m_tag As Object = Nothing
        ''' <summary>
        ''' Temporary array to hold the subitems to add to
        ''' this group once it's owner has been assigned.
        ''' </summary>
        Private subItems As ListBarItem()
        ''' <summary>
        ''' The collection of items associated with this 
        ''' group.
        ''' </summary>
        Private m_items As ListBarItemCollection = Nothing
        ''' <summary>
        ''' A child control to display in this bar instead
        ''' of the child items.
        ''' </summary>
        Private m_childControl As Control = Nothing
        ''' <summary>
        ''' Font to render this group with.
        ''' </summary>
        Private m_font As Font = Nothing
        ''' <summary>
        ''' ForeColor to render this group with.
        ''' </summary>
        Private m_foreColor As Color = Color.FromKnownColor(KnownColor.WindowText)
        ''' <summary>
        ''' BackColor to render this group with.
        ''' </summary>
        Private m_backColor As Color = Color.FromKnownColor(KnownColor.Control)
        ''' <summary>
        ''' Bounding rectangle for this group's button.  The height
        ''' is managed by this object but the other members are typically
        ''' adjusted by the owning control through the <see cref="SetLocationAndWidth"/>
        ''' and the <see cref="SetButtonHeight"/> methods.
        ''' </summary>
        Protected rectangle As New Rectangle(0, 0, 0, 1)
        ''' <summary>
        ''' The view (LargeIcons or SmallIcons) to use when drawing the items 
        ''' in the bar.
        ''' </summary>
        Private iconSize As ListBarGroupView = ListBarGroupView.LargeIcons
        ''' <summary>
        ''' The scroll 
        ''' </summary>
        Private m_scrollOffset As Integer = 0
        ''' <summary>
        ''' Whether the mouse is over the button or not.
        ''' </summary>
        Private m_mouseOver As Boolean = False
        ''' <summary>
        ''' Whether the mouse is down on the button or not.
        ''' </summary>
        Private m_mouseDown As Boolean = False
        ''' <summary>
        ''' The point at which the mouse was clicked on the group
        ''' button.
        ''' </summary>
        Private m_mouseDownPoint As New Point(0, 0)
        ''' <summary>
        ''' Whether the group is visible or not.
        ''' </summary>
        Private m_visible As Boolean = True

        ''' <summary>
        ''' Returns a string representation of this <see cref="ListBarGroup"/>.
        ''' </summary>
        ''' <returns>A string containing the class name, caption, rectangle
        ''' and item count for this group.</returns>
        <Description("Returns a string representation of this ListBarGroup.")> _
        Public Overrides Function ToString() As String
            Return [String].Format("{0} Caption={1} Location={2} Height={3} ItemCount={4}", Me.[GetType]().FullName, Me.m_caption, Me.ButtonLocation, Me.ButtonHeight, Me.m_items.Count)
        End Function

        ''' <summary>
        ''' Returns the selected <see cref="ListBarItem"/> in this Group, if any, otherwise null.
        ''' </summary>
        <Description("Returns the selected ListBarItem in this Group, if any, otherwise null.")> _
         Public ReadOnly Property SelectedItem() As ListBarItem
            Get
                Dim ret As ListBarItem = Nothing
                For Each item As ListBarItem In Me.m_items
                    If item.Selected Then
                        ret = item
                        Exit For
                    End If
                Next
                Return ret
            End Get
        End Property

        ''' <summary>
        ''' Gets/sets a <see cref="System.Windows.Forms.Control"/>
        ''' which can be displayed within this group.
        ''' </summary>
        ''' <remarks>
        ''' Do not set the child control until this group has
        ''' been added to the control.
        ''' </remarks>
        <Description("Gets/sets a Control which is displayed in this group rather than items.")> _
        Public Property ChildControl() As Control
            Get
                Return Me.m_childControl
            End Get
            Set(value As Control)
                value.Visible = False
                Me.m_childControl = value
                Me.m_childControl.Parent = Me.m_owner
                NotifyOwner(True)
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets the point at which the mouse was clicked on the group
        ''' button.
        ''' </summary>
        <Description("Gets/sets the point at which the mouse was clicked on the group button.")> _
        Public Property MouseDownPoint() As Point Implements IMouseObject.MouseDownPoint
            Get
                Return Me.m_mouseDownPoint
            End Get
            Set(value As Point)
                Me.m_mouseDownPoint = value
            End Set
        End Property
        ''' <summary>
        ''' Gets/sets whether the mouse is over the group button.
        ''' </summary>
        <Description("Gets/sets whether the mouse is over the group button.")> _
        Public Property MouseOver() As Boolean Implements IMouseObject.MouseOver
            Get
                Return Me.m_mouseOver
            End Get
            Set(value As Boolean)
                Me.m_mouseOver = (value And Me.m_visible)
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets whether the mouse is down over the group button.
        ''' </summary>
        <Description("Gets/sets whether the mouse is down over the group button.")> _
        Public Property MouseDown() As Boolean Implements IMouseObject.MouseDown
            Get
                Return Me.m_mouseDown
            End Get
            Set(value As Boolean)
                Me.m_mouseDown = (value And Me.m_visible)
            End Set
        End Property

        ''' <summary>
        ''' Called to create a new item collection for this ListBarGroup.
        ''' </summary>
        ''' <returns>The ListBarItemCollection that will be used for this
        ''' ListBarGroup</returns>
        Protected Overridable Function CreateListBarItemCollection() As ListBarItemCollection
            Return New ListBarItemCollection(m_owner)
        End Function

        ''' <summary>
        ''' Called to create a new item collection for this ListBarGroup
        ''' when the data is being deserialized
        ''' </summary>
        ''' <returns>The ListBarItemCollection that will be used for this
        ''' ListBarGroup</returns>
        Protected Overridable Function CreateListBarItemCollection(info As SerializationInfo, context As StreamingContext) As ListBarItemCollection
            Return New ListBarItemCollection(info, context)
        End Function

        ''' <summary>
        ''' Internal member holding the negative scrolled 
        ''' offset of this bar from the top of the client area
        ''' </summary>
        Protected Friend Property ScrollOffset() As Integer
            Get
                Return Me.m_scrollOffset
            End Get
            Set(value As Integer)
                Me.m_scrollOffset = value
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets the which view to show the items within this bar.
        ''' </summary>
        <Description("Gets/sets the which view to show the items within this bar.")> _
        Public Property View() As ListBarGroupView
            Get
                Return Me.iconSize
            End Get
            Set(value As ListBarGroupView)
                If Me.iconSize <> value Then
                    Me.iconSize = value
                    SetLocationAndWidth(Me.rectangle.Location, Me.rectangle.Width)
                    NotifyOwner(True)
                End If
            End Set
        End Property

        Private Sub SetItemSize()
            If Me.m_items IsNot Nothing Then
                For Each item As ListBarItem In Me.m_items
                    Me.m_owner.ItemChanged(item, False)
                Next
                NotifyOwner(True)
            End If
        End Sub
        ''' <summary>
        ''' Called to set the height of this group's button by the owning control.
        ''' </summary>
        ''' <param name="defaultFont">The default <see cref="System.Drawing.Font"/>
        ''' to use when this item does not have a specific font set.</param>
        <Description("Called to set the height of this group's button by the owning control.")> _
        Public Overridable Sub SetButtonHeight(defaultFont As Font)
            ' Select the font we're going to use
            Dim drawFont As Font = defaultFont
            If Me.Font IsNot Nothing Then
                drawFont = Me.Font
            End If

            ' Get the string to measure to determine
            ' the item's height
            Dim measureString As String = "Xg"
            ' Measure the height of an item 
            Dim measureBitmap As New Bitmap(30, 30)
            Dim graphics__1 As Graphics = Graphics.FromImage(measureBitmap)
            Dim textSize As SizeF = graphics__1.MeasureString(measureString, drawFont)
            graphics__1.Dispose()
            measureBitmap.Dispose()

            Me.rectangle.Height = gPMFunctions.ToSafeInteger(Math.Truncate(textSize.Height)) + 2
        End Sub


        ''' <summary>
        ''' Returns the location of the button
        ''' which activates this group relative
        ''' to the owning control.
        ''' </summary>
        <Description("Returns the location of the button which activates this group relative to the owning control.")> _
        Public Overridable ReadOnly Property ButtonLocation() As Point
            Get
                Return Me.rectangle.Location
            End Get
        End Property

        ''' <summary>
        ''' Returns the width of the button
        ''' which activates this group.
        ''' </summary>
        <Description("Returns the width of the button which activates this group.")> _
        Public Overridable ReadOnly Property ButtonWidth() As Integer
            Get
                Return Me.rectangle.Width
            End Get
        End Property

        ''' <summary>
        ''' Returns the height of the button
        ''' which activates this group.
        ''' </summary>
        <Description("Returns the height of the button which activates this group.")> _
        Public Overridable ReadOnly Property ButtonHeight() As Integer
            Get
                Return Me.rectangle.Height
            End Get
        End Property

        ''' <summary>
        ''' Sets the location and width of the button which
        ''' activates this <see cref="ListBarGroup"/>.  This method
        ''' is called by internally by the <see cref="ListBar"/> 
        ''' which owns this item.
        ''' </summary>
        ''' <remarks>
        ''' This member is not intended to be called from client code.
        ''' If you do use it, it is likely that a subsequent operation
        ''' on the control or group will replace the values.  If you
        ''' need more control over placement, override this class
        ''' and build the logic into the override for this method
        ''' instead.
        ''' </remarks>
        ''' <param name="location">The new location for the item.</param>
        ''' <param name="width">The new width of the item.</param>
        <Description("Sets the location and width of the button which activates this group.  This method is called internally by the owning ListBar control.")> _
        Public Overridable Sub SetLocationAndWidth(location As Point, width As Integer)
            Me.rectangle.Location = location
            Me.rectangle.Width = width
            If Me.m_items IsNot Nothing Then
                Dim itemLocation As New Point(location.X, 0)

                Dim defaultFont As Font = Me.Font
                If defaultFont Is Nothing Then
                    If Me.m_owner Is Nothing Then
                        defaultFont = System.Windows.Forms.SystemInformation.MenuFont
                    Else
                        defaultFont = Me.m_owner.Font
                    End If
                End If

                Dim imageSize As New Size(32, 32)
                If (Me.iconSize = ListBarGroupView.LargeIcons) OrElse (Me.iconSize = ListBarGroupView.LargeIconsOnly) Then
                    If Me.m_owner IsNot Nothing Then
                        If Me.m_owner.LargeImageList IsNot Nothing Then
                            imageSize = Me.m_owner.LargeImageList.ImageSize
                        End If
                    End If
                Else
                    imageSize.Width = 16
                    imageSize.Height = 16
                    If Me.m_owner IsNot Nothing Then
                        If Me.m_owner.SmallImageList IsNot Nothing Then
                            imageSize = Me.m_owner.SmallImageList.ImageSize
                        End If
                    End If
                End If

                If (Me.View = ListBarGroupView.SmallIconsOnly) OrElse (Me.View = ListBarGroupView.LargeIconsOnly) Then
                    Dim itemWidth As Integer = imageSize.Width + 16
                    For i As Integer = 0 To Me.m_items.Count - 1
                        Dim item As ListBarItem = Me.m_items(i)
                        item.SetSize(Me.View, defaultFont, imageSize)
                        item.SetLocationAndWidth(itemLocation, itemWidth)
                        itemLocation.X += item.Width
                        If i < Me.m_items.Count - 1 Then
                            If (item.Location.X + Me.m_items(i + 1).Width) > width Then
                                itemLocation.X = location.X
                                itemLocation.Y += item.Height
                            End If
                        End If
                    Next
                Else
                    If Me.Owner IsNot Nothing Then
                        width = Me.Owner.Width
                    End If

                    For Each item As ListBarItem In Me.m_items
                        item.SetSize(Me.iconSize, defaultFont, imageSize)
                        item.SetLocationAndWidth(itemLocation, width)
                        itemLocation.Y += item.Height
                    Next
                End If
            End If
        End Sub

        ''' <summary>
        ''' Draws the items within this <see cref="ListBarGroup"/> onto the control.
        ''' </summary>
        ''' <param name="gfx">The <see cref="System.Drawing.Graphics"/> object to draw onto.</param>
        ''' <param name="bounds">The bounding <see cref="System.Drawing.Rectangle"/> within which
        ''' to draw the items.</param>
        ''' <param name="ils">The <see cref="System.Windows.Forms.ImageList"/> object to use to draw
        ''' the bar.</param>
        ''' <param name="defaultFont">The default <see cref="System.Drawing.Font"/> to use.</param>
        ''' <param name="style">The style to draw the ListBar in.</param>
        ''' <param name="enabled">Whether the ListBar control is enabled or not.</param>
        <Description("Draws the items within this group bar onto the ListBar control.  Called internally by the owning ListBar control.")> _
        Public Overridable Sub DrawBar(gfx As Graphics, bounds As Rectangle, ils As ImageList, defaultFont As Font, style As ListBarDrawStyle, enabled As Boolean)
            If Me.m_childControl IsNot Nothing Then
                Me.m_childControl.Location = bounds.Location
                Me.m_childControl.Size = bounds.Size
            Else
                Me.Items.Draw(gfx, bounds, ils, defaultFont, style, Me.View, _
                    enabled, Me.m_scrollOffset + Me.rectangle.Bottom)
            End If
        End Sub

        ''' <summary>
        ''' Draws the button for this group onto the control.
        ''' </summary>
        ''' <param name="gfx">The <see cref="System.Drawing.Graphics"/> object to draw onto.</param>
        ''' <param name="defaultFont">The default <see cref="System.Drawing.Font"/> to 
        ''' draw with.</param>
        ''' <param name="enabled">Whether this control is enabled or not.</param>
        <Description("Draws the button for this group onto the control.")> _
        Public Overridable Sub DrawButton(gfx As Graphics, defaultFont As Font, enabled As Boolean)
            If Me.m_visible Then
                ' Get the font to draw with:
                Dim drawFont As Font = Me.m_font
                If drawFont Is Nothing Then
                    drawFont = defaultFont
                End If

                ' Fill the item:
                Dim br As Brush = New SolidBrush(Me.m_backColor)
                gfx.FillRectangle(br, Me.rectangle)
                br.Dispose()

                ' Draw the text:
                Dim format As New StringFormat(StringFormatFlags.LineLimit Or StringFormatFlags.NoWrap)
                format.Trimming = StringTrimming.EllipsisCharacter
                format.Alignment = StringAlignment.Center
                format.LineAlignment = StringAlignment.Center
                format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show
                Dim rectF As New RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height)
                If enabled Then
                    br = New SolidBrush(Me.m_foreColor)
                    gfx.DrawString(Me.m_caption, drawFont, br, rectF, format)
                    br.Dispose()
                Else
                    rectF.Offset(1.0F, 1.0F)
                    br = New SolidBrush(CustomBorderColor.ColorLightLight(Me.m_backColor))
                    gfx.DrawString(Me.m_caption, drawFont, br, rectF, format)
                    br.Dispose()
                    rectF.Offset(-1.0F, -1.0F)
                    br = New SolidBrush(CustomBorderColor.ColorDark(Me.m_backColor))
                    gfx.DrawString(Me.m_caption, drawFont, br, rectF, format)
                    br.Dispose()
                End If
                format.Dispose()

                ' Draw the border:
                Dim darkDarkPen As New Pen(CustomBorderColor.ColorDarkDark(Me.BackColor))
                Dim darkPen As New Pen(CustomBorderColor.ColorDark(Me.BackColor))
                Dim lightPen As New Pen(CustomBorderColor.ColorLight(Me.BackColor))
                Dim lightLightPen As New Pen(CustomBorderColor.ColorLightLight(Me.BackColor))

                If Me.m_mouseDown AndAlso Me.m_mouseOver Then
                    gfx.DrawLine(darkDarkPen, rectangle.Left, rectangle.Bottom - 2, rectangle.Left, rectangle.Top)
                    gfx.DrawLine(darkDarkPen, rectangle.Left, rectangle.Top, rectangle.Right - 2, rectangle.Top)
                    gfx.DrawLine(darkPen, rectangle.Left + 1, rectangle.Bottom - 3, rectangle.Left + 1, rectangle.Top + 1)
                    gfx.DrawLine(darkPen, rectangle.Left + 1, rectangle.Top + 1, rectangle.Right - 3, rectangle.Top + 1)

                    gfx.DrawLine(lightLightPen, rectangle.Right - 1, rectangle.Top, rectangle.Right - 1, rectangle.Bottom - 1)
                    gfx.DrawLine(lightLightPen, rectangle.Right - 1, rectangle.Bottom - 1, rectangle.Left, rectangle.Bottom - 1)
                    gfx.DrawLine(lightPen, rectangle.Right - 2, rectangle.Top + 1, rectangle.Right - 2, rectangle.Bottom - 2)
                    gfx.DrawLine(lightPen, rectangle.Right - 2, rectangle.Bottom - 2, rectangle.Left + 1, rectangle.Bottom - 2)
                ElseIf Me.MouseOver OrElse Me.m_mouseDown Then
                    gfx.DrawLine(lightLightPen, rectangle.Left, rectangle.Bottom - 2, rectangle.Left, rectangle.Top)
                    gfx.DrawLine(lightLightPen, rectangle.Left, rectangle.Top, rectangle.Right - 2, rectangle.Top)
                    gfx.DrawLine(lightPen, rectangle.Left + 1, rectangle.Bottom - 3, rectangle.Left + 1, rectangle.Top + 1)
                    gfx.DrawLine(lightPen, rectangle.Left + 1, rectangle.Top + 1, rectangle.Right - 3, rectangle.Top + 1)

                    gfx.DrawLine(darkDarkPen, rectangle.Right - 1, rectangle.Top, rectangle.Right - 1, rectangle.Bottom - 1)
                    gfx.DrawLine(darkDarkPen, rectangle.Right - 1, rectangle.Bottom - 1, rectangle.Left, rectangle.Bottom - 1)
                    gfx.DrawLine(darkPen, rectangle.Right - 2, rectangle.Top + 1, rectangle.Right - 2, rectangle.Bottom - 2)
                    gfx.DrawLine(darkPen, rectangle.Right - 2, rectangle.Bottom - 2, rectangle.Left + 1, rectangle.Bottom - 2)
                Else
                    gfx.DrawLine(lightLightPen, rectangle.Left, rectangle.Bottom - 2, rectangle.Left, rectangle.Top + 1)
                    gfx.DrawLine(lightLightPen, rectangle.Left, rectangle.Top + 1, rectangle.Right - 2, rectangle.Top + 1)
                    gfx.DrawLine(darkPen, rectangle.Right - 1, rectangle.Top + 1, rectangle.Right - 1, rectangle.Bottom - 1)
                    gfx.DrawLine(darkPen, rectangle.Right - 1, rectangle.Bottom - 1, rectangle.Left, rectangle.Bottom - 1)
                End If

                lightLightPen.Dispose()
                lightPen.Dispose()
                darkPen.Dispose()

                darkDarkPen.Dispose()
            End If
        End Sub

        ''' <summary>
        ''' Returns the collection of items belonging to this <see cref="ListBarGroup" />.
        ''' group.
        ''' </summary>
        <Description("Returns the collection of items belonging to this ListBarGroup")> _
        Public Overridable ReadOnly Property Items() As ListBarItemCollection
            Get
                Return Me.m_items
            End Get
        End Property

        ''' <summary>
        ''' Gets/sets whether this group is visible in the control 
        ''' or not.
        ''' </summary>
        <Description("Gets/sets whether this group is visible in the control or not.")> _
        Public Overridable Property Visible() As Boolean
            Get
                Return Me.m_visible
            End Get
            Set(value As Boolean)
                Me.m_visible = value
                NotifyOwner(True)
            End Set
        End Property


        ''' <summary>
        ''' Gets/sets the <see cref="System.Drawing.Font"/> to draw the caption for this group.
        ''' </summary>
        <Description("Returns the Font used to draw the caption for this group.")> _
        Public Overridable Property Font() As Font
            Get
                Return Me.m_font
            End Get
            Set(value As Font)
                Me.m_font = value
                NotifyOwner(True)
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets the foreground colour to use when drawing
        ''' the button for this group.
        ''' </summary>
        <Description("Gets/sets the foreground colour to use when drawing the button for this group.")> _
        Public Overridable Property ForeColor() As Color
            Get
                Return Me.m_foreColor
            End Get
            Set(value As Color)
                Me.m_foreColor = value
                NotifyOwner(False)
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets the background colour to use when drawing the button for this group.
        ''' </summary>
        <Description("Gets/sets the background colour to use when drawing the button for this group.")> _
        Public Overridable Property BackColor() As Color
            Get
                Return Me.m_backColor
            End Get
            Set(value As Color)
                Me.m_backColor = value
                NotifyOwner(False)
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets the caption displayed for this group.
        ''' </summary>
        <Description("Gets/sets the caption displayed for this group.")> _
        Public Overridable Property Caption() As String
            Get
                Return Me.m_caption
            End Get
            Set(value As String)
                Me.m_caption = value
                NotifyOwner(False)
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets the string key associated with this group.
        ''' </summary>
        <Description("Gets/sets a string key associated with this group.")> _
        Public Overridable Property Key() As String
            Get
                Return Me.m_key
            End Get
            Set(value As String)
                Me.m_key = value
            End Set
        End Property
        ''' <summary>
        ''' Gets/sets the tooltip that will be displayed when the user
        ''' hovers over this group's button.
        ''' </summary>
        <Description("Gets/sets the tooltip text that will be displayed when the user hovers over this group's button.")> _
        Public Overridable Property ToolTipText() As String Implements IMouseObject.ToolTipText
            Get
                Return Me.m_toolTipText
            End Get
            Set(value As String)
                Me.m_toolTipText = value
            End Set
        End Property
        ''' <summary>
        ''' Gets/sets whether this group is selected or not.
        ''' </summary>
        <Description("Gets/sets whether this group is selected or not.")> _
        Public Overridable Property Selected() As Boolean
            Get
                Return Me.m_selected
            End Get
            Set(value As Boolean)
                If value <> Me.m_selected Then
                    Me.m_selected = value
                    If Me.m_childControl IsNot Nothing Then
                        Me.m_childControl.Visible = value
                    End If
                    NotifyOwner(False)
                End If
            End Set
        End Property
        ''' <summary>
        ''' Gets/sets a user-defined object associated with this group.
        ''' </summary>
        <Description("Gets/sets a user-defined object associated with this group.")> _
        Public Overridable Property Tag() As Object
            Get
                Return Me.m_tag
            End Get
            Set(value As Object)
                Me.m_tag = value
            End Set
        End Property

        ''' <summary>
        ''' Starts editing this item.  The <c>BeforeLabelEdit</c> event will
        ''' be fired prior to the text box being made visible.
        ''' </summary>
        ''' <exception cref="InvalidOperationException">If the item is not
        ''' part of a ListBar control.</exception>
        <Description("Starts editing this item and fires the BeforeLabelEdit event.")> _
        Public Overridable Sub StartEdit()
            If Me.m_owner IsNot Nothing Then
                m_owner.StartGroupEdit(Me)
            Else
                Throw New InvalidOperationException("Owner of this ListBarGroup has not been set.")
            End If
        End Sub
        ''' <summary>
        ''' Notifies the owning ListBar control of any changes to a group.
        ''' </summary>
        ''' <param name="addRemove">Whether the control should resize
        ''' all groups associated with the ListBar.</param>
        Protected Overridable Sub NotifyOwner(addRemove As Boolean)
            If m_owner IsNot Nothing Then
                m_owner.GroupChanged(Me, addRemove)
            End If
        End Sub

        ''' <summary>
        ''' Sets the owning control for this Group.  Called automatically
        ''' whenever a group is added to the group collection associated with
        ''' a ListBar control.
        ''' </summary>
        ''' <param name="owner">The ListBar control which owns this group.</param>
        Protected Friend Sub SetOwner(owner As ListBar)
            Me.m_owner = owner
            If Me.m_items Is Nothing Then
                Me.m_items = CreateListBarItemCollection()
            End If
            If Me.subItems IsNot Nothing Then
                Me.m_items.AddRange(subItems)
                Me.subItems = Nothing
            End If
            ' Set the size of any items which belong
            ' to this bar:
            SetItemSize()

            NotifyOwner(True)
        End Sub
        ''' <summary>
        ''' Gets the owning ListBar control for this item.
        ''' </summary>
        Protected Friend ReadOnly Property Owner() As ListBar
            Get
                Return Me.m_owner
            End Get
        End Property

        ''' <summary>
        ''' Populates a System.Runtime.Serialization.SerializationInfo object with the 
        ''' data needed to serialize this object.
        ''' </summary>
        ''' <param name="info">The System.Runtime.Serialization.SerializationInfo 
        ''' to populate with data.</param>
        ''' <param name="context">The destination (see 
        ''' System.Runtime.Serialization.StreamingContext) for this serialization.</param>
        Public Overridable Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData
            info.AddValue("Font", Me.m_font)
            info.AddValue("ToolTipText", Me.m_toolTipText)
            info.AddValue("Caption", Me.m_caption)
            info.AddValue("ForeColor", Me.m_foreColor)
            info.AddValue("BackColor", Me.m_backColor)
            info.AddValue("Tag", Me.m_tag)
            info.AddValue("Key", Me.m_key)
            info.AddValue("Rectangle", Me.rectangle)
            info.AddValue("View", gPMFunctions.ToSafeInteger(Me.iconSize))
            info.AddValue("Selected", Me.m_selected)

            info.AddValue("Items", Me.m_items)
        End Sub

        ''' <summary>
        ''' Constructs this object from a serialized representation.
        ''' </summary>
        ''' <param name="info">The System.Runtime.Serialization.SerializationInfo 
        ''' containing the serialized data to build this object from.</param>
        ''' <param name="context">The destination (see 
        ''' System.Runtime.Serialization.StreamingContext) for this serialization.</param>
        Public Sub New(info As SerializationInfo, context As StreamingContext)
            Me.m_font = DirectCast(info.GetValue("Font", GetType(Font)), Font)
            Me.m_toolTipText = info.GetString("ToolTipText")
            Me.m_caption = info.GetString("Caption")
            Me.m_foreColor = CType(info.GetValue("ForeColor", GetType(Color)), Color)
            Me.m_backColor = CType(info.GetValue("BackColor", GetType(Color)), Color)
            Me.m_tag = info.GetValue("Tag", GetType(Object))
            Me.m_key = info.GetString("Key")
            Me.rectangle = CType(info.GetValue("Rectangle", GetType(Rectangle)), Rectangle)
            Me.View = CType(info.GetInt32("View"), ListBarGroupView)
            Me.m_selected = info.GetBoolean("Selected")


            Me.m_items = CreateListBarItemCollection(info, context)
        End Sub


        ''' <summary>
        ''' Constructs a new, blank instance of a ListBarGroup. intentionally empty
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Constructs a new instance of a ListBarGroup with the specified
        ''' caption.
        ''' </summary>
        ''' <param name="caption">Caption for the group's control button.</param>
        Public Sub New(caption As String)
            Me.New()
            Me.m_caption = caption
        End Sub
        ''' <summary>
        ''' Constructs a new instance of a ListBarGroup with the specified
        ''' caption and items.
        ''' </summary>
        ''' <param name="caption">Caption for the group's control button.</param>
        ''' <param name="subItems">The array of items to add to the group's
        ''' collection of items.</param>
        Public Sub New(caption As String, subItems As ListBarItem())
            Me.New(caption)
            Me.subItems = subItems
        End Sub
        ''' <summary>
        ''' Constructs a new instance of a ListBarGroup with the specified
        ''' caption and tooltip text.
        ''' </summary>
        ''' <param name="caption">Caption for the group's control button.</param>
        ''' <param name="toolTipText">ToolTip text to show when hovering over
        ''' the group.</param>
        Public Sub New(caption As String, toolTipText As String)
            Me.New(caption)
            Me.m_toolTipText = toolTipText
        End Sub
        ''' <summary>
        ''' Constructs a new instance of a ListBarGroup with the specified
        ''' caption, tooltip text and user-defined data.
        ''' </summary>
        ''' <param name="caption">Caption for the group's control button.</param>
        ''' <param name="toolTipText">ToolTip text to show when hovering over
        ''' the group.</param>
        ''' <param name="tag">User-defined object data which is associated with
        ''' the group.</param>
        Public Sub New(caption As String, toolTipText As String, tag As Object)
            Me.New(caption, toolTipText)
            Me.m_tag = tag
        End Sub

    End Class
#End Region

#Region "ListBarItemCollection class"
    ''' <summary>
    ''' This class manages a collection of items within a ListBarGroup.
    ''' </summary>
    <SerializableAttribute> _
    Public Class ListBarItemCollection
        Inherits CollectionBase
        Implements ISerializable
        ''' <summary>
        ''' The owning ListBar control.
        ''' </summary>
        Private owner As ListBar = Nothing

        ''' <summary>
        ''' Sorts the items in this collection using the specified
        ''' comparer.
        ''' </summary>
        ''' <param name="comparer">IComparer implementation specifying
        ''' how to sort the objects.</param>
        <Description("Sorts the items in this collection using the specified comparer")> _
        Public Overridable Sub Sort(comparer As IComparer)
            Me.InnerList.Sort(comparer)
            owner.ItemChanged(Nothing, True)
        End Sub
        ''' <summary>
        ''' Sorts the items in this collection using the default comparison
        ''' operation (alphabetic).
        ''' </summary>
        <Description("Sorts the items in this collection alphabetically.")> _
        Public Overridable Sub Sort()
            Me.InnerList.Sort()
            owner.ItemChanged(Nothing, True)
        End Sub
        <Description("Refresh the items in group.")> _
        Public Overridable Sub Refresh()
            owner.ReSetGroup()
        End Sub
        ''' <summary>
        ''' Draws the items within this collection.
        ''' </summary>
        ''' <param name="gfx">The graphics object to draw onto.</param>
        ''' <param name="bounds">The bounding rectangle within which
        ''' to draw the items.</param>
        ''' <param name="ils">The ImageList to use when drawing the icons.</param>
        ''' <param name="defaultFont">The default <see cref="System.Drawing.Font"/> to use.</param>
        ''' <param name="style">The Style to draw the items using.</param>
        ''' <param name="view">The view to use when drawing the items.</param>
        ''' <param name="enabled">Whether the owning group is enabled or not.</param>
        ''' <param name="scrollOffset">The scrolled offset at which to start
        ''' drawing the items.</param>				
        Public Overridable Sub Draw(gfx As Graphics, bounds As Rectangle, ils As ImageList, defaultFont As Font, style As ListBarDrawStyle, view As ListBarGroupView, _
            enabled As Boolean, scrollOffset As Integer)
            Dim skipDraw As Boolean = False
            Dim editItem As ListBarItem = Me.owner.EditItem

            gfx.SetClip(bounds)
            For Each item As ListBarItem In Me.InnerList
                skipDraw = False
                If editItem IsNot Nothing Then
                    skipDraw = editItem.Equals(item)
                End If
                Dim itemTop As Integer = item.Location.Y
                itemTop += scrollOffset
                If ((itemTop >= bounds.Top) AndAlso (itemTop <= bounds.Bottom)) OrElse (((itemTop + item.Height) <= bounds.Bottom) AndAlso ((itemTop + item.Height) > bounds.Top)) Then
                    item.DrawButton(gfx, ils, defaultFont, style, view, scrollOffset, _
                        enabled, skipDraw)
                End If
            Next
            gfx.ResetClip()
        End Sub

        ''' <summary>
        ''' Gets the height of all the items within this collection.
        ''' </summary>
        <Description("Gets the overall height of all the items in the collection.")> _
        Public Overridable ReadOnly Property Height() As Integer
            Get
                Dim maxHeight As Integer = 0
                For Each item As ListBarItem In Me.InnerList
                    Dim itemBottom As Integer = item.Location.Y + item.Height
                    If itemBottom > maxHeight Then
                        maxHeight = itemBottom
                    End If
                Next
                Return maxHeight
            End Get
        End Property

        ''' <summary>
        ''' Adds a <see cref="ListBarItem"/> object to the group.
        ''' </summary>
        ''' <param name="item">The ListBarItem to add.</param>
        <Description("Adds a ListBarItem object to the items in the group.")> _
        Public Overridable Sub Add(item As ListBarItem)
            Me.InnerList.Add(item)
            item.SetOwner(owner)
            EnsureSingleSelection(item)
            NotifyOwner(item, True)
        End Sub

        ''' <summary>
        ''' Constructs a new <see cref="ListBarItem"/> object using the specified
        ''' caption, adds it to the bar and returns it.
        ''' </summary>
        ''' <param name="caption">The caption to use for the ListBarItem.</param>
        ''' <returns>The newly added ListBarItem object.</returns>
        <Description("Constructs a new ListBarItem object and adds it to the group.")> _
        Public Overridable Function Add(caption As String) As ListBarItem
            Dim item As New ListBarItem(caption)
            Me.InnerList.Add(item)
            item.SetOwner(owner)
            NotifyOwner(item, True)
            Return item
        End Function

        ''' <summary>
        ''' Constructs a new ListBarItem object using the specified
        ''' caption and icon, adds it to the bar and returns it.
        ''' </summary>
        ''' <param name="caption">The caption to use for the ListBarItem.</param>
        ''' <param name="iconIndex">The 0-based index of the icon for the ListBarItem
        ''' within an ImageList</param>
        ''' <returns>The newly added ListBarItem object.</returns>
        <Description("Constructs a new ListBarItem object and adds it to the group.")> _
        Public Overridable Function Add(caption As String, iconIndex As Integer) As ListBarItem
            Dim item As New ListBarItem(caption, iconIndex)
            Me.InnerList.Add(item)
            item.SetOwner(owner)
            NotifyOwner(item, True)
            Return item
        End Function

        ''' <summary>
        ''' Adds a range of <see cref="ListBarItem"/> objects to the bar from an array.
        ''' </summary>
        ''' <param name="values">The array of ListBarItem objects to
        ''' add.</param>
        <Description("Adds of range of ListBarItem objects to the bar.")> _
        Public Overridable Sub AddRange(values As ListBarItem())
            For Each item As ListBarItem In values
                Me.InnerList.Add(item)
                item.SetOwner(owner)
            Next
            EnsureSingleSelection(Me(0))
            NotifyOwner(values(0), True)
        End Sub

        ''' <summary>
        ''' Returns <c>true</c> if the specified <see cref="ListBarItem "/> is contained
        ''' within this collection, otherwise <c>false</c>.
        ''' </summary>
        ''' <param name="item">The ListBarItem to check.</param>
        ''' <returns>True if the specified ListBarItem is contained
        ''' within this collection, False otherwise.</returns>
        <Description("Returns true if the specified ListBarItem is found in this collection, otherwise false.")> _
        Public Overridable Function Contains(item As ListBarItem) As Boolean
            Return Me.InnerList.Contains(item)
        End Function

        ''' <summary>
        ''' Returns the 0-based index of the specified item in the
        ''' collection if present, -1 otherwise.
        ''' </summary>
        ''' <param name="item">The ListBarItem to check.</param>
        ''' <returns>The 0-based index of the specified item in the
        ''' collection if present, -1 otherwise.</returns>
        <Description("Returns the 0-based index of the specified item in the collection")> _
        Public Overridable Function IndexOf(item As ListBarItem) As Integer
            Return Me.InnerList.IndexOf(item)
        End Function

        ''' <summary>
        ''' Inserts a <see cref="ListBarItem"/> at the specified index in the bar.
        ''' </summary>
        ''' <param name="index">The index to insert at.</param>
        ''' <param name="item">The ListBarItem to insert.</param>
        <Description("Inserts a ListBarItem at the specified index in the bar.")> _
        Public Overridable Sub Insert(index As Integer, item As ListBarItem)
            Me.InnerList.Insert(index, item)
            item.SetOwner(Me.owner)
            EnsureSingleSelection(item)
            NotifyOwner(item, True)
        End Sub

        ''' <summary>
        ''' Inserts a <see cref="ListBarItem"/> immediately before the specified ListBarItem.
        ''' </summary>
        ''' <param name="itemBefore">ListBarItem to insert before.</param>
        ''' <param name="item">Item to insert.</param>
        <Description("Inserts a ListBarItem immediately before the specified ListBarItem.")> _
        Public Overridable Sub InsertBefore(itemBefore As ListBarItem, item As ListBarItem)
            Me.InnerList.Insert(Me.InnerList.IndexOf(itemBefore), item)
            EnsureSingleSelection(item)
            NotifyOwner(item, True)
        End Sub

        ''' <summary>
        ''' Inserts a <see cref="ListBarItem"/> immediately after the specified ListBarItem.
        ''' </summary>
        ''' <param name="itemAfter">ListBarItem to insert after.</param>
        ''' <param name="item">Item to insert.</param>
        <Description("Inserts a ListBarItem immediately after the specified ListBarItem.")> _
        Public Overridable Sub InsertAfter(itemAfter As ListBarItem, item As ListBarItem)
            Dim index As Integer = Me.InnerList.IndexOf(itemAfter)
            If index = Me.InnerList.Count - 1 Then
                Me.Add(item)
            Else
                Me.Insert(index + 1, item)
            End If
            NotifyOwner(item, True)
        End Sub

        ''' <summary>
        ''' Removes the specified <see cref="ListBarItem"/> from the collection.
        ''' </summary>
        ''' <param name="item">Item to remove.</param>
        <Description("Removes the specified ListBarItem from the collection.")> _
        Public Overridable Sub Remove(item As ListBarItem)
            Me.InnerList.Remove(item)
            NotifyOwner(item, True)
        End Sub

        ''' <summary>
        ''' Gets the <see cref="ListBarItem"/> at the specified 0-based index.
        ''' </summary>
        <Description("Gets the ListBarItem at the specified 0-based index.")> _
        Default Public ReadOnly Property Item(index As Integer) As ListBarItem
            Get
                If Me.InnerList.Count > 0 Then
                    Return DirectCast(Me.InnerList(index), ListBarItem)
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the <see cref="ListBarItem"/> with the specified key.
        ''' </summary>
        <Description("Gets the ListBarItem at the specified key.")> _
        Default Public ReadOnly Property Item(key As String) As ListBarItem
            Get
                Dim ret As ListBarItem = Nothing
                For Each item__1 As ListBarItem In Me.InnerList
                    If item__1.Key.Equals(key) Then
                        ret = item__1
                        Exit For
                    End If
                Next
                Return ret
            End Get
        End Property

        Private Sub EnsureSingleSelection(newItem As ListBarItem)
            Dim foundSelectedItem As Boolean = False
            If newItem.Selected Then
                foundSelectedItem = True
            End If

            For Each item As ListBarItem In Me.InnerList
                If Not item.Equals(newItem) Then
                    If item.Selected Then
                        If foundSelectedItem Then
                            item.Selected = False
                        Else
                            foundSelectedItem = True
                        End If
                    End If
                End If
            Next
        End Sub


        ''' <summary>
        ''' Notifies the owner control that the items have been
        ''' cleared.
        ''' </summary>
        Protected Overrides Sub OnClearComplete()
            NotifyOwner(Nothing, True)
        End Sub
        ''' <summary>
        ''' Notifies the owner control after an item has been inserted.
        ''' </summary>
        ''' <param name="index">Index of inserting item</param>
        ''' <param name="value">Item which has been inserted.</param>
        Protected Overrides Sub OnInsertComplete(index As System.Int32, value As System.Object)
            NotifyOwner(DirectCast(value, ListBarItem), True)
        End Sub
        ''' <summary>
        ''' Notifies the owner control after an item has been removed.
        ''' </summary>
        ''' <param name="index">Index of inserting item</param>
        ''' <param name="value">Item which has been inserted.</param>
        Protected Overrides Sub OnRemoveComplete(index As System.Int32, value As System.Object)
            NotifyOwner(DirectCast(value, ListBarItem), True)
        End Sub
        ''' <summary>
        ''' Notifies the owner control after an item has been changed using set.
        ''' </summary>
        ''' <param name="index">Index of inserting item</param>
        ''' <param name="oldValue">Old item which was there.</param>
        ''' <param name="newValue">New Item which has been set.</param>
        Protected Overrides Sub OnSetComplete(index As System.Int32, oldValue As System.Object, newValue As System.Object)
            NotifyOwner(DirectCast(newValue, ListBarItem), True)
        End Sub

        ''' <summary>
        ''' Notifies the owning control of a change in this item.
        ''' </summary>
        ''' <param name="addRemove">Set to true if the change
        ''' that has been made requires the size of the display
        ''' to be recalculated.</param>
        ''' <param name="item">The Item which has been changed
        ''' (or null if the item itm is invalid)</param>
        Protected Overridable Sub NotifyOwner(item As ListBarItem, addRemove As Boolean)
            If owner IsNot Nothing Then
                owner.ItemChanged(item, addRemove)
            End If
        End Sub

        ''' <summary>
        ''' 
        ''' TODO: This method has not been implemented yet.
        ''' 
        ''' Populates a System.Runtime.Serialization.SerializationInfo object with the 
        ''' data needed to serialize this object.
        ''' </summary>
        ''' <param name="info">The System.Runtime.Serialization.SerializationInfo 
        ''' to populate with data.</param>
        ''' <param name="context">The destination (see 
        ''' System.Runtime.Serialization.StreamingContext) for this serialization.</param>
        Public Overridable Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData
            '
            ' TODO: This method has not been implemented yet.
            ' 
        End Sub

        ''' <summary>
        ''' Enables a deserialized object graph to be associated with a ListBar
        ''' control.
        ''' </summary>
        ''' <param name="owner">The ListBar control which will own
        ''' this collection of items.</param>
        Public Overridable Sub SetOwner(owner As ListBar)
            Me.owner = owner
            For Each item As ListBarItem In Me.InnerList
                item.SetOwner(owner)
            Next
        End Sub

        ''' <summary>
        ''' TODO: This method has not been implemented yet.
        ''' Constructs this object from a serialized representation.
        ''' </summary>
        ''' <param name="info">The System.Runtime.Serialization.SerializationInfo containing the serialized data to build this object from.</param>
        ''' <param name="context">The destination (see System.Runtime.Serialization.StreamingContext) for this serialization.</param>
        ''' <remarks></remarks>
        Public Sub New(info As SerializationInfo, context As StreamingContext)
        End Sub

        ''' <summary>
        ''' Constructs a new instance of this collection and sets
        ''' the owner.  Typically this is performed by the owning ListBar
        ''' control.
        ''' </summary>
        ''' <param name="owner">The ListBar which owns this collection</param>
        Public Sub New(owner As ListBar)
            Me.owner = owner
        End Sub

    End Class
#End Region

#Region "ListBarItem class"
    ''' <summary>
    ''' A class containing the information describing an Item in the ListBar
    ''' control.
    ''' </summary>
    <SerializableAttribute> _
    Public Class ListBarItem
        Implements IComparable
        Implements IMouseObject
        Implements ISerializable
        Private m_owner As ListBar = Nothing
        Private m_selected As Boolean = False
        Private m_font As Font = Nothing
        Private m_toolTipText As String = ""
        Private m_caption As String = ""
        Private m_foreColor As Color = Color.FromKnownColor(KnownColor.WindowText)
        Private m_tag As Object = ""
        Private m_key As String = ""
        Private m_iconIndex As Integer
        ''' <summary>
        ''' Bounding rectangle for this item, relative to its owning
        ''' group.  The members of this are typically adjusted by the 
        ''' owning control through the <see cref="SetLocationAndWidth"/>
        ''' and the <see cref="SetSize"/> methods.
        ''' </summary>
        Protected rectangle As New Rectangle(0, 0, 0, 72)
        ''' <summary>
        ''' The rectangle containing the icon for this item.  Set this 
        ''' when overriding the standard drawing mode for an item;
        ''' the owning ListBar control uses it for hit-testing.
        ''' </summary>
        Protected m_iconRectangle As Rectangle
        ''' <summary>
        ''' The rectangle containing the text for this item.  Set this
        ''' when overriding the standard drawing mode for an item; 
        ''' the owning ListBar control uses it for hit-testing.
        ''' </summary>
        Protected m_textRectangle As Rectangle
        Private m_enabled As Boolean = True
        Private m_mouseOver As Boolean = False
        Private m_mouseDown As Boolean = False
        Private m_mouseDownPoint As New Point(0, 0)

        ''' <summary>
        ''' Returns a string representation of this <see cref="ListBarItem"/>.
        ''' </summary>
        ''' <returns>A string containing the class name, caption, icon index,
        ''' enabled state and rectangle for this item.</returns>
        <Description("Returns a string representation of this ListBarItem")> _
        Public Overrides Function ToString() As String
            Return [String].Format("{0} Caption={1} IconIndex={2} Enabled={3} Location={4} Height={5}", Me.[GetType]().FullName, Me.m_caption, Me.m_iconIndex, Me.m_enabled, Me.Location, _
                Me.Height)
        End Function

        ''' <summary>
        ''' Gets/sets the point at which the mouse was pressed
        ''' on this object.
        ''' </summary>
        <Description("Gets/sets the point at which the mouse was pressed on this object.")> _
        Public Property MouseDownPoint() As Point Implements IMouseObject.MouseDownPoint
            Get
                Return Me.m_mouseDownPoint
            End Get
            Set(value As Point)
                Me.m_mouseDownPoint = value
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets whether the mouse is over this item.
        ''' </summary>
        <Description("Gets/sets whether the mouse is over this item.")> _
        Public Property MouseOver() As Boolean Implements IMouseObject.MouseOver
            Get
                Return Me.m_mouseOver
            End Get
            Set(value As Boolean)
                Me.m_mouseOver = value
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets whether the mouse is down on this item.
        ''' </summary>
        <Description("Gets/sets whether the mouse is down on this item.")> _
        Public Property MouseDown() As Boolean Implements IMouseObject.MouseDown
            Get
                Return Me.m_mouseDown
            End Get
            Set(value As Boolean)
                Me.m_mouseDown = (value And Me.m_enabled)
            End Set
        End Property

        ''' <summary>
        ''' Draws this item into the specified graphics object.
        ''' </summary>
        ''' <param name="gfx">The <see cref="System.Drawing.Graphics"/> object to draw onto.</param>
        ''' <param name="ils">The <see cref="System.Windows.Forms.ImageList"/>to source icons from.</param>
        ''' <param name="defaultFont">The default <see cref="System.Drawing.Font"/> to use to render
        ''' the item.</param>
        ''' <param name="style">The style (Outlook version) to draw using.</param>
        ''' <param name="view">The view (large or small icons) to draw using.</param>
        ''' <param name="scrollOffset">The offset of the first item from the 
        ''' (0,0) point in the graphics object.</param>
        ''' <param name="controlEnabled">Whether the control is enabled or not.</param>
        ''' <param name="skipDrawText">Whether to skip drawing text or not
        ''' (the item is being edited)</param> 
        <Description("Draws this item into the specified graphics object")> _
        Public Overridable Sub DrawButton(gfx As Graphics, ils As ImageList, defaultFont As Font, style As ListBarDrawStyle, view As ListBarGroupView, scrollOffset As Integer, _
            controlEnabled As Boolean, skipDrawText As Boolean)

            Dim rightToLeft__1 As Boolean = False
            Dim backColor As Color = Color.FromKnownColor(KnownColor.Control)
            If Me.m_owner IsNot Nothing Then
                backColor = Me.m_owner.BackColor
                If Me.m_owner.RightToLeft = RightToLeft.Yes Then
                    rightToLeft__1 = True
                End If
            End If

            ' Work out the icon & text rectangles:			
            m_textRectangle = New Rectangle(Me.rectangle.Location, Me.rectangle.Size)
            m_textRectangle.Offset(0, scrollOffset)
            If view = ListBarGroupView.SmallIcons Then
                m_textRectangle.Y += 1
                m_textRectangle.Height -= 1
            End If
            m_iconRectangle = New Rectangle(m_textRectangle.Location, m_textRectangle.Size)

            If view = ListBarGroupView.SmallIcons Then
                If ils IsNot Nothing Then
                    If rightToLeft__1 Then
                        m_iconRectangle.X = m_iconRectangle.Right - ils.ImageSize.Width - 4
                        m_iconRectangle.Width = ils.ImageSize.Width
                    Else
                        m_iconRectangle.X += 4
                        m_iconRectangle.Width = ils.ImageSize.Width
                        m_textRectangle.X += ils.ImageSize.Width + 8
                    End If
                    m_textRectangle.Width -= (m_iconRectangle.Width + 8)
                    m_iconRectangle.Height = ils.ImageSize.Height
                    m_iconRectangle.Y += (Me.rectangle.Height - m_iconRectangle.Height) \ 2
                Else
                    m_textRectangle.Inflate(-2, -2)
                End If
            Else
                If ils IsNot Nothing Then
                    m_iconRectangle.Y += 7
                    m_iconRectangle.Height = ils.ImageSize.Height
                    m_iconRectangle.Width = ils.ImageSize.Width
                    m_iconRectangle.X = m_iconRectangle.Left + (Me.rectangle.Width - m_iconRectangle.Width) \ 2

                    m_textRectangle.Y += ils.ImageSize.Height + 11
                    m_textRectangle.Height -= (ils.ImageSize.Height + 11)
                Else
                    m_textRectangle.Inflate(-2, -2)
                End If
            End If

            ' If we're drawing using XP style and the button is
            ' hot or down then we draw the background:
            Dim rcHighlight As New Rectangle(m_iconRectangle.Location, m_iconRectangle.Size)
            rcHighlight.Inflate(2, 2)
            If style = ListBarDrawStyle.ListBarDrawStyleOfficeXP Then
                If (Me.m_enabled AndAlso controlEnabled) AndAlso (Me.MouseOver OrElse Me.m_mouseDown) Then
                    Dim highlightColor As Color
                    If Me.m_mouseDown AndAlso Me.m_mouseOver Then
                        highlightColor = ListBarUtility.BlendColor(Color.FromKnownColor(KnownColor.Highlight), Color.FromKnownColor(KnownColor.Window), 224)
                    Else
                        highlightColor = ListBarUtility.BlendColor(Color.FromKnownColor(KnownColor.Highlight), Color.FromKnownColor(KnownColor.Window), 128)
                    End If
                    Dim highlight As New SolidBrush(Color.FromArgb(128, highlightColor))
                    gfx.FillRectangle(highlight, rcHighlight)
                    highlight.Dispose()
                    gfx.DrawRectangle(SystemPens.Highlight, rcHighlight)
                End If
            End If


            ' Draw the icon if necessary:
            If ils IsNot Nothing Then
                If Me.m_iconIndex >= 0 AndAlso Me.m_iconIndex <= ils.Images.Count - 1 Then

                    Dim iconX As Integer = m_iconRectangle.X
                    Dim iconY As Integer = m_iconRectangle.Y


                    If Me.m_mouseDown AndAlso Me.m_mouseOver Then
                        iconX += 1
                        iconY += 1
                    End If
                    If Me.m_enabled AndAlso controlEnabled Then
                        ils.Draw(gfx, m_iconRectangle.X + 1, m_iconRectangle.Y + 1, Me.m_iconIndex)
                    Else
                        ControlPaint.DrawImageDisabled(gfx, ils.Images(Me.m_iconIndex), iconX, iconY, Color.FromArgb(0, 0, 0, 0))

                    End If
                Else
                    ' We don't want an exception in a paint event
                    Trace.WriteLine([String].Format("Icon {0} doesn't exist in ImageList {1}", Me.m_iconIndex, ils))
                End If
            End If
            ' We do this to make the hit testing more usable:
            m_iconRectangle.Inflate(4, 4)

            If skipDrawText Then
                Return
            End If

            If (view = ListBarGroupView.SmallIconsOnly) OrElse (view = ListBarGroupView.LargeIconsOnly) Then
                m_textRectangle = New Rectangle(0, 0, 0, 0)
            Else

                ' Draw the text:
                ' Get the font to draw with:
                Dim drawFont As Font = Me.m_font
                If drawFont Is Nothing Then
                    If Me.m_owner IsNot Nothing Then
                        drawFont = Me.m_owner.Font
                    End If
                End If
                If drawFont Is Nothing Then
                    drawFont = System.Windows.Forms.SystemInformation.MenuFont
                End If
                ' Set up format:
                Dim format As New StringFormat(StringFormatFlags.LineLimit)
                format.Trimming = StringTrimming.EllipsisCharacter
                If view = ListBarGroupView.SmallIcons Then
                    format.Alignment = StringAlignment.Near
                    format.LineAlignment = StringAlignment.Center
                    format.FormatFlags = format.FormatFlags Or StringFormatFlags.NoWrap
                Else
                    format.Alignment = StringAlignment.Center
                End If
                format.LineAlignment = StringAlignment.Near
                format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show
                ' Bounding rectangle:
                Dim rectF As New RectangleF(m_textRectangle.X, m_textRectangle.Y, m_textRectangle.Width, m_textRectangle.Height)
                If view = ListBarGroupView.SmallIcons Then
                    Dim textSize As SizeF = gfx.MeasureString(Me.m_caption, drawFont, m_textRectangle.Width, format)
                    rectF.Y += (rectF.Height - textSize.Height) / 2
                    m_textRectangle.Y += gPMFunctions.ToSafeInteger(Math.Truncate((rectF.Height - textSize.Height) / 2))
                    m_textRectangle.Height = gPMFunctions.ToSafeInteger(Math.Truncate(textSize.Height))
                End If
                ' Color:
                Dim br As New SolidBrush(Me.m_foreColor)
                ' Finally...
                If Me.m_enabled AndAlso controlEnabled Then
                    gfx.DrawString(Me.m_caption, drawFont, br, rectF, format)
                Else
                    Dim lightBrush As Brush = New SolidBrush(CustomBorderColor.ColorLightLight(backColor))
                    Dim darkBrush As Brush = New SolidBrush(CustomBorderColor.ColorDark(backColor))
                    rectF.Offset(1.0F, 1.0F)
                    gfx.DrawString(Me.m_caption, drawFont, lightBrush, rectF, format)
                    rectF.Offset(-1.0F, -1.0F)
                    gfx.DrawString(Me.m_caption, drawFont, darkBrush, rectF, format)
                    darkBrush.Dispose()
                    '	
                    '					ControlPaint.DrawStringDisabled(gfx, 
                    '						this.caption, drawFont, 
                    '						Color.FromKnownColor(KnownColor.Control), 
                    '						rectF, format);
                    '					

                    lightBrush.Dispose()
                End If
                br.Dispose()
                format.Dispose()
            End If

            ' The border around the item if required:
            If Me.m_owner.DrawStyle = ListBarDrawStyle.ListBarDrawStyleNormal Then
                If Me.m_enabled AndAlso controlEnabled Then
                    Dim penTopLeft As Pen = Nothing
                    Dim penBottomRight As Pen = Nothing
                    If (Me.m_mouseDown) AndAlso (Me.m_mouseDown) Then
                        ' inset 3d border:
                        penTopLeft = SystemPens.ControlDarkDark
                        penBottomRight = SystemPens.ControlLight
                    ElseIf (Me.m_mouseOver) OrElse (Me.m_mouseDown) Then
                        ' raised 3d border:
                        penTopLeft = SystemPens.ControlLight
                        penBottomRight = SystemPens.ControlDarkDark
                    End If
                    If penTopLeft IsNot Nothing Then
                        gfx.DrawLine(penTopLeft, rcHighlight.Left, rcHighlight.Bottom - 2, rcHighlight.Left, rcHighlight.Top)
                        gfx.DrawLine(penTopLeft, rcHighlight.Left, rcHighlight.Top, rcHighlight.Right - 2, rcHighlight.Top)
                        gfx.DrawLine(penBottomRight, rcHighlight.Right - 1, rcHighlight.Top, rcHighlight.Right - 1, rcHighlight.Bottom - 1)
                        gfx.DrawLine(penBottomRight, rcHighlight.Right - 1, rcHighlight.Bottom - 1, rcHighlight.Left, rcHighlight.Bottom - 1)
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' Gets/sets whether this item is enabled.
        ''' </summary>
        <Description("Gets/sets whether this item is enabled.")> _
        Public Property Enabled() As Boolean
            Get
                Return Me.m_enabled
            End Get
            Set(value As Boolean)
                Me.m_enabled = value
                NotifyOwner(False)
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets the foreground colour for this item.
        ''' </summary>
        <Description("Gets/sets the foreground colour for this item.")> _
        Public Property ForeColor() As Color
            Get
                Return Me.m_foreColor
            End Get
            Set(value As Color)
                Me.m_foreColor = value
                NotifyOwner(False)
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets the font used for this object.  The default
        ''' font is null which means the item renders using the
        ''' font of the parent control.
        ''' </summary>
        <Description("Gets/sets the font for this item.")> _
        Public Property Font() As Font
            Get
                Return Me.m_font
            End Get
            Set(value As Font)
                Me.m_font = value
                NotifyOwner(False)
            End Set
        End Property

        ''' <summary>
        ''' Gets the location for this item in the control.
        ''' </summary>
        ''' <remarks>
        ''' The location is relative to the group the 
        ''' item belongs to.  Therefore to find the position
        ''' relative to the control you need to add the 
        ''' bottom position of the button rectangle for the group
        ''' and the scroll offset of the item. 
        ''' </remarks>
        <Description("Gets the location of this item in the control.")> _
        Public Overridable ReadOnly Property Location() As Point
            Get
                Return Me.rectangle.Location
            End Get
        End Property

        ''' <summary>
        ''' Gets the height of this item.
        ''' </summary>
        <Description("Gets the height of this item in the control.")> _
        Public Overridable ReadOnly Property Height() As Integer
            Get
                Return Me.rectangle.Height
            End Get
        End Property

        ''' <summary>
        ''' Gets the width of this item.
        ''' </summary>
        <Description("Gets the width of this item in the control.")> _
        Public Overridable ReadOnly Property Width() As Integer
            Get
                Return Me.rectangle.Width
            End Get
        End Property

        ''' <summary>
        ''' Returns the rectangle in which the icon is drawn for
        ''' this item, relative to the control.
        ''' </summary>
        <Description("Returns the rectangle in which the icon is drawn for this item, relative to the control.")> _
        Public Overridable ReadOnly Property IconRectangle() As Rectangle
            Get
                Return Me.m_iconRectangle
            End Get
        End Property

        ''' <summary>
        ''' Returns the rectangle in which the text is drawn for
        ''' this item, relative to the control.
        ''' </summary>
        <Description("Returns the rectangle in which the text is drawn for this item, relative to the control.")> _
        Public Overridable ReadOnly Property TextRectangle() As Rectangle
            Get
                Return Me.m_textRectangle
            End Get
        End Property

        ''' <summary>
        ''' Sets the location and width of this item.  This method
        ''' is called by internally by the <see cref="ListBar"/> or
        ''' the <see cref="ListBarGroup"/> which owns this item.
        ''' </summary>
        ''' <remarks>
        ''' This member is not intended to be called from client code.
        ''' If you do use it, it is likely that a subsequent operation
        ''' on the control or group will replace the values.  If you
        ''' need more control over placement, override this class
        ''' and build the logic into the override for this method
        ''' instead.
        ''' </remarks>
        ''' <param name="location">The new location for the item.</param>
        ''' <param name="width">The new width of the item.</param>
        <Description("Sets the location and width of this item in the control. Called internally by the owning ListBar or group")> _
        Public Overridable Sub SetLocationAndWidth(location As Point, width As Integer)
            Me.rectangle.Location = location
            Me.rectangle.Width = width
        End Sub

        ''' <summary>
        ''' Called to set the height of the item by the owning control.
        ''' </summary>
        ''' <param name="view">The <see cref="ListBarGroupView"/> in which this
        ''' item is being shown.</param>
        ''' <param name="defaultFont">The default <see cref="System.Drawing.Font"/>
        ''' to use when this item does not have a specific font set.</param>
        ''' <param name="imageSize">The size of the images in the ImageList
        ''' used to render this view.</param>		
        <Description("Called to set the height of an item by the owning control.")> _
        Public Overridable Sub SetSize(view As ListBarGroupView, defaultFont As Font, imageSize As Size)
            ' Select the font we're going to use
            Dim drawFont As Font = defaultFont
            If Me.Font IsNot Nothing Then
                drawFont = Me.Font
            End If

            ' Get the string to measure to determine
            ' the item's height
            Dim measureString As String = "Xg"
            If view = ListBarGroupView.LargeIcons Then
                ' by default we allow for two lines:
                measureString += vbCr & vbLf & "Xg"
            End If

            ' Measure the height of an item 
            Dim measureBitmap As New Bitmap(30, 30)
            Dim graphics__1 As Graphics = Graphics.FromImage(measureBitmap)
            Dim textSize As SizeF = graphics__1.MeasureString(measureString, drawFont)
            graphics__1.Dispose()
            measureBitmap.Dispose()

            ' Set the height using the text size & the image size
            Dim height As Integer = imageSize.Height
            If view = ListBarGroupView.LargeIcons Then
                height += gPMFunctions.ToSafeInteger(Math.Truncate(textSize.Height))
                height += 12
            Else
                If textSize.Height > height Then
                    height = gPMFunctions.ToSafeInteger(Math.Truncate(textSize.Height))
                End If
                height += 8
            End If
            Me.rectangle.Height = height
        End Sub

        ''' <summary>
        ''' Compares this object with another object of the same type.
        ''' The implementation compares the captions of the items to
        ''' allow items to be sorted alphabetically.
        ''' </summary>
        ''' <param name="obj">Another ListBarItem object</param>
        ''' <returns>A 32-bit signed integer that indicates the relative order of the comparands.  
        ''' The return value has these meanings: 
        ''' &lt; 0: This instance is less than obj.  
        ''' 0: This instance is equal to obj.  
        ''' &gt; 0: This instance is greater than obj. </returns>
        <Description("Compares this object with another object of the same type.")> _
        Public Overridable Function CompareTo(obj As System.Object) As System.Int32 Implements IComparable.CompareTo
            Return m_caption.CompareTo(DirectCast(obj, ListBarItem).Caption)
        End Function

        ''' <summary>
        ''' Gets/sets whether this item is "selected" or not.
        ''' Only one item in the ListBar control can be selected
        ''' at a time.
        ''' </summary>
        <Description("Gets/sets whether this item is selected or not.")> _
        Public Overridable Property Selected() As Boolean
            Get
                Return Me.m_selected
            End Get
            Set(value As Boolean)
                If Me.m_selected <> value Then
                    Me.m_selected = value
                    NotifyOwner(False)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Ensures that this item can be seen in the owner
        ''' control.
        ''' </summary>
        ''' <exception cref="InvalidOperationException">If the item is not
        ''' part of a ListBarGroup.</exception>
        <Description("Ensures that this item can be seen in the owning control.")> _
        Public Overridable Sub EnsureVisible()
            If Me.m_owner IsNot Nothing Then
                m_owner.EnsureItemVisible(Me)
            Else
                Throw New InvalidOperationException("Owner of this ListBarItem has not been set.")
            End If
        End Sub

        ''' <summary>
        ''' Starts editing this item.  The <c>BeforeLabelEdit</c> event will
        ''' be fired prior to the text box being made visible.
        ''' </summary>
        ''' <exception cref="InvalidOperationException">If the item is not
        ''' part of a ListBarGroup or not part of the selected group
        ''' in the control.</exception>
        <Description("Starts editing this item.  The BeforeLabelEdit event will be fired prior to editing commencing.")> _
        Public Overridable Sub StartEdit()
            If Me.m_owner IsNot Nothing Then
                m_owner.StartItemEdit(Me)
            Else
                Throw New InvalidOperationException("Owner of this ListBarItem has not been set.")
            End If
        End Sub

        ''' <summary>
        ''' Gets/sets a user-defined string value which can be used
        ''' to look up the item in the collection which owns it.
        ''' </summary>
        <Description("Gets/sets a user-defined string value which can be used to look up the item in the collection which owns it.")> _
        Public Overridable Property Key() As String
            Get
                Return Me.m_key
            End Get
            Set(value As String)
                Me.m_key = value
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets the tooltip text that will be displayed when
        ''' the user hovers over this item.
        ''' </summary>
        <Description("Gets/sets the tooltip text that will be displayed when the user hovers over this item.")> _
        Public Overridable Property ToolTipText() As String Implements IMouseObject.ToolTipText
            Get
                Return Me.m_toolTipText
            End Get
            Set(value As String)
                Me.m_toolTipText = value
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets the caption displayed for this item.
        ''' </summary>
        <Description("Gets/sets the caption displayed for this item.")> _
        Public Overridable Property Caption() As String
            Get
                Return Me.m_caption
            End Get
            Set(value As String)
                Me.m_caption = value
                NotifyOwner(False)
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets the 0-based index of an icon in an <see cref="System.Windows.Forms.ImageList"/>
        ''' displayed with this item.
        ''' </summary>
        <Description("Gets/sets the 0-based index of an icon in an ImageList displayed with this item.")> _
        Public Overridable Property IconIndex() As Integer
            Get
                Return Me.m_iconIndex
            End Get
            Set(value As Integer)
                Me.m_iconIndex = value
                NotifyOwner(False)
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets an object which can be used to associate
        ''' user-defined data with this item.
        ''' </summary>
        <Description("Gets/sets an object which can be used to associate user-defined data with this item.")> _
        Public Overridable Property Tag() As Object
            Get
                Return Me.m_tag
            End Get
            Set(value As Object)
                Me.m_tag = value
            End Set
        End Property

        ''' <summary>
        ''' Notifies the owning control of a change in this item.
        ''' </summary>
        ''' <param name="addRemove">Set to true if the change
        ''' that has been made requires the size of the display
        ''' to be recalculated.</param>
        Protected Overridable Sub NotifyOwner(addRemove As Boolean)
            If m_owner IsNot Nothing Then
                m_owner.ItemChanged(Me, addRemove)
            End If
        End Sub

        ''' <summary>
        ''' Gets the owning ListBar control for this item.
        ''' </summary>
        Protected Friend ReadOnly Property Owner() As ListBar
            Get
                Return Me.m_owner
            End Get
        End Property

        ''' <summary>
        ''' Sets the owning ListBar control for this item.
        ''' </summary>
        ''' <param name="owner">The owning ListBar control for this item.</param>
        Protected Friend Sub SetOwner(owner As ListBar)
            Me.m_owner = owner
            NotifyOwner(True)
        End Sub

        ''' <summary>
        ''' Populates a System.Runtime.Serialization.SerializationInfo object with the 
        ''' data needed to serialize this object.
        ''' </summary>
        ''' <param name="info">The System.Runtime.Serialization.SerializationInfo 
        ''' to populate with data.</param>
        ''' <param name="context">The destination (see 
        ''' System.Runtime.Serialization.StreamingContext) for this serialization.</param>
        Public Overridable Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData
            info.AddValue("Font", Me.m_font)
            info.AddValue("ToolTipText", Me.m_toolTipText)
            info.AddValue("Caption", Me.m_caption)
            info.AddValue("ForeColor", Me.m_foreColor)
            info.AddValue("Tag", Me.m_tag)
            info.AddValue("Key", Me.m_key)
            info.AddValue("IconIndex", Me.m_iconIndex)
            info.AddValue("Rectangle", Me.rectangle)
            info.AddValue("Selected", Me.m_selected)
        End Sub

        ''' <summary>
        ''' Constructs this object from a serialized representation.
        ''' </summary>
        ''' <param name="info">The System.Runtime.Serialization.SerializationInfo 
        ''' containing the serialized data to build this object from.</param>
        ''' <param name="context">The destination (see 
        ''' System.Runtime.Serialization.StreamingContext) for this serialization.</param>
        Public Sub New(info As SerializationInfo, context As StreamingContext)
            Me.m_font = DirectCast(info.GetValue("Font", GetType(Font)), Font)
            Me.m_toolTipText = info.GetString("ToolTipText")
            Me.m_caption = info.GetString("Caption")
            Me.m_foreColor = CType(info.GetValue("ForeColor", GetType(Color)), Color)
            Me.m_tag = info.GetString("Tag")
            Me.m_key = info.GetString("Key")
            Me.m_iconIndex = info.GetInt32("IconIndex")
            Me.rectangle = CType(info.GetValue("Rectangle", GetType(Rectangle)), Rectangle)
        End Sub

        ''' <summary>
        ''' Constructs a new, empty instance of a ListBarItem.
        ''' </summary>
        Public Sub New()
        End Sub
        ''' <summary>
        '''  Constructs a new instance of a ListBarItem, specifying
        '''  the caption to display.
        ''' </summary>
        ''' <param name="caption">The caption for this item.</param>
        Public Sub New(caption As String)
            Me.New()
            Me.m_caption = caption
        End Sub
        ''' <summary>
        '''  Constructs a new instance of a ListBarItem, specifying
        '''  the caption and the index of the icon to display.
        ''' </summary>
        ''' <param name="caption">The caption for this item.</param>
        ''' <param name="iconIndex">The 0-based index of the icon
        ''' to display</param>
        Public Sub New(caption As String, iconIndex As Integer)
            Me.New(caption)
            Me.m_iconIndex = iconIndex
        End Sub
        ''' <summary>
        '''  Constructs a new instance of a ListBarItem, specifying
        '''  the caption, the index of the icon and the 
        '''  tooltip text.
        ''' </summary>
        ''' <param name="caption">The caption for this item.</param>
        ''' <param name="iconIndex">The 0-based index of the icon
        ''' to display</param>
        ''' <param name="toolTipText">The tooltip text to show
        ''' when the mouse hovers over this item.</param>
        Public Sub New(caption As String, iconIndex As Integer, toolTipText As String)
            Me.New(caption, iconIndex)
            Me.m_toolTipText = toolTipText
        End Sub
        ''' <summary>
        '''  Constructs a new instance of a ListBarItem, specifying
        '''  the caption, the index of the icon, the 
        '''  tooltip text and the tag.
        ''' </summary>
        ''' <param name="caption">The caption for this item.</param>
        ''' <param name="iconIndex">The 0-based index of the icon
        ''' to display</param>
        ''' <param name="toolTipText">The tooltip text to show
        ''' when the mouse hovers over this item.</param>
        ''' <param name="tag">An object which can be used to 
        ''' associate user-defined data with the item.</param>
        Public Sub New(caption As String, iconIndex As Integer, toolTipText As String, tag As Object)
            Me.New(caption, iconIndex, toolTipText)
            Me.m_tag = tag
        End Sub
        ''' <summary>
        '''  Constructs a new instance of a ListBarItem, specifying
        '''  the caption, the index of the icon, the 
        '''  tooltip text, the tag and the key.
        ''' </summary>
        ''' <param name="caption">The caption for this item.</param>
        ''' <param name="iconIndex">The 0-based index of the icon
        ''' to display</param>
        ''' <param name="toolTipText">The tooltip text to show
        ''' when the mouse hovers over this item.</param>
        ''' <param name="tag">An object which can be used to 
        ''' associate user-defined data with the item.</param>
        ''' <param name="key">A user-defined string which is 
        ''' associated with the item.</param>
        Public Sub New(caption As String, iconIndex As Integer, toolTipText As String, tag As Object, key As String)
            Me.New(caption, iconIndex, toolTipText, tag)
            Me.m_key = key
        End Sub

    End Class
#End Region

#Region "ListBarScrollButton class"
    ''' <summary>
    ''' A class which manages the behaviour and data associated with
    ''' a scrolling button in the ListBar control.  This class can
    ''' be overridden to provide (for example) an alternative rendering
    ''' of the button.
    ''' </summary>
    Public Class ListBarScrollButton
        Implements IMouseObject
        ''' <summary>
        ''' Enumeration of available scroll button types 
        ''' for this control.
        ''' </summary>
        Public Enum ListBarScrollButtonType
            ''' <summary>
            ''' The scroll button is an up button.
            ''' </summary>
            Up
            ''' <summary>
            ''' The scroll button is a down button.
            ''' </summary>
            Down
        End Enum

        ''' <summary>
        ''' The bounding rectangle for this button
        ''' </summary>
        Private m_rectangle As New Rectangle(0, 0, SystemInformation.VerticalScrollBarWidth, SystemInformation.HorizontalScrollBarHeight)
        ''' <summary>
        ''' Whether the mouse is down on the button or not
        ''' </summary>
        Private m_mouseDown As Boolean = False
        ''' <summary>
        ''' Whether the mouse is over this button or not
        ''' </summary>
        Private m_mouseOver As Boolean = False
        ''' <summary>
        ''' The point at which the mouse was pressed on this button.
        ''' </summary>
        Private m_mouseDownPoint As New Point(0, 0)
        ''' <summary>
        ''' Whether this button is visible or not.
        ''' </summary>
        Private m_visible As Boolean = False
        ''' <summary>
        ''' The type of scroll button.
        ''' </summary>
        Private m_buttonType As ListBarScrollButtonType = ListBarScrollButtonType.Up
        ''' <summary>
        ''' ToolTip Text to display.
        ''' </summary>
        Private m_toolTipText As String = ""

        ''' <summary>
        ''' Gets/sets the tooltip text to display for this button.
        ''' </summary>
        <Description("Gets/sets the tooltip text to display for this button.")> _
        Public Property ToolTipText() As String Implements IMouseObject.ToolTipText
            Get
                Return Me.m_toolTipText
            End Get
            Set(value As String)
                Me.m_toolTipText = value
            End Set
        End Property

        ''' <summary>
        ''' Gets/sets whether this object is visible or not.
        ''' </summary>
        <Description("Gets/sets whether this object is visible or not.")> _
        Public Property Visible() As Boolean
            Get
                Return Me.m_visible
            End Get
            Set(value As Boolean)
                Me.m_visible = value
                If Not value Then
                    Me.m_mouseDown = False
                    Me.m_mouseOver = False
                End If
            End Set
        End Property

        ''' <summary>
        ''' Determines whether the specified point is within the control.
        ''' </summary>
        ''' <param name="pt">The point to test.</param>
        ''' <returns>True if the point is over the button and the button
        ''' is visible, false otherwise.</returns>
        Public Function HitTest(pt As Point) As Boolean
            Dim hitTest__1 As Boolean = False
            If m_visible Then
                hitTest__1 = Me.m_rectangle.Contains(pt)
            End If
            Return hitTest__1
        End Function

        ''' <summary>
        ''' Gets which type of scroll button this is (Up or Down)
        ''' </summary>
        <Description("Gets which type of scroll button this is (Up or Down)")> _
        Public ReadOnly Property ButtonType() As ListBarScrollButtonType
            Get
                Return Me.m_buttonType
            End Get
        End Property

        ''' <summary>
        ''' Draws the button onto the specified <see cref="System.Drawing.Graphics" /> 
        ''' object.
        ''' </summary>
        ''' <remarks>
        ''' Note that this method is called by the owning bar even if the 
        ''' the button's <see cref="Visible"/> property is set to <c>False</c>.
        ''' In subclasses of this object this enables the button to 		
        ''' be shown disabled when it isn't needed, rather than the default
        ''' behaviour which is to remove it entirely.
        ''' </remarks>
        ''' <param name="gfx">The <see cref="System.Drawing.Graphics"/> object 
        ''' to draw on.</param>
        ''' <param name="defaultBackColor">The default background
        ''' <see cref="System.Drawing.Color"/> to use when drawing
        ''' the button.</param>
        ''' <param name="controlEnabled">Whether the owning control is enabled
        ''' or not.</param>
        Public Overridable Sub DrawItem(gfx As Graphics, defaultBackColor As Color, controlEnabled As Boolean)
            If Me.m_visible Then
                If defaultBackColor.Equals(Color.FromKnownColor(KnownColor.Control)) Then
                    ' Use the default mechanism:
                    Dim buttonState__1 As ButtonState = ButtonState.Normal
                    If controlEnabled Then
                        buttonState__1 = (If((m_mouseDown AndAlso m_mouseOver), ButtonState.Pushed, ButtonState.Normal))
                    Else
                        buttonState__1 = ButtonState.Inactive
                    End If
                    ControlPaint.DrawScrollButton(gfx, Me.m_rectangle, (If(Me.m_buttonType = ListBarScrollButtonType.Up, ScrollButton.Up, ScrollButton.Down)), buttonState__1)
                Else
                    ' Not as easy when using custom border colours:

                    ' Fill background:
                    Dim br As Brush = New SolidBrush(defaultBackColor)
                    gfx.FillRectangle(br, Me.m_rectangle)
                    br.Dispose()

                    ' Draw the glyph:
                    Dim centrePoint As New Point((Me.m_rectangle.Width \ 2), (Me.m_rectangle.Height \ 2))
                    centrePoint.Offset(Me.m_rectangle.Left + 1, Me.m_rectangle.Top)
                    If m_mouseDown AndAlso m_mouseOver Then
                        centrePoint.Offset(1, 1)
                    End If
                    Dim opposite As Integer = 0
                    If Me.ButtonType = ListBarScrollButtonType.Up Then
                        opposite = -4
                        centrePoint.Offset(0, 2)
                    Else
                        opposite = 4
                        centrePoint.Offset(0, -1)
                    End If

                    If Not controlEnabled Then
                        br = New SolidBrush(CustomBorderColor.ColorLightLight(defaultBackColor))
                        centrePoint.Offset(1, 1)
                        ListBarUtility.FillRightAngleTriangle(gfx, br, centrePoint, 4, opposite)
                        ListBarUtility.FillRightAngleTriangle(gfx, br, centrePoint, -4, opposite)
                        br.Dispose()
                        centrePoint.Offset(-1, -1)
                        br = New SolidBrush(CustomBorderColor.ColorDark(defaultBackColor))
                        ListBarUtility.FillRightAngleTriangle(gfx, br, centrePoint, 4, opposite)
                        ListBarUtility.FillRightAngleTriangle(gfx, br, centrePoint, -4, opposite)
                        br.Dispose()
                    Else
                        ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, centrePoint, 4, opposite)
                        ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, centrePoint, -4, opposite)
                    End If

                    ' Draw the border:
                    CustomBorderColor.DrawBorder(gfx, Me.m_rectangle, defaultBackColor, True, (m_mouseDown AndAlso m_mouseOver))
                End If
            End If
        End Sub

        ''' <summary>
        ''' Gets/sets whether the mouse is down on this object or not.
        ''' </summary>
        <Description("Gets/sets whether the mouse is down on this object or not.")> _
        Public Property MouseDown() As Boolean Implements IMouseObject.MouseDown
            Get
                Return Me.m_mouseDown
            End Get
            Set(value As Boolean)
                Me.m_mouseDown = value
            End Set
        End Property
        ''' <summary>
        ''' Gets/sets whether the mouse is over this object or not.
        ''' </summary>
        <Description("Gets/sets whether the mouse is over this object or not.")> _
        Public Property MouseOver() As Boolean Implements IMouseObject.MouseOver
            Get
                Return Me.m_mouseOver
            End Get
            Set(value As Boolean)
                Me.m_mouseOver = value
            End Set
        End Property
        ''' <summary>
        ''' Gets/sets the point at which the mouse was pressed on
        ''' this object.
        ''' </summary>
        <Description("Gets/sets the point at which the mouse was pressed on this object.")> _
        Public Property MouseDownPoint() As Point Implements IMouseObject.MouseDownPoint
            Get
                Return Me.m_mouseDownPoint
            End Get
            Set(value As Point)
                Me.m_mouseDownPoint = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the bounding rectangle for this button.
        ''' </summary>
        <Description("Gets the bounding rectangle for this button.")> _
        Public ReadOnly Property Rectangle() As Rectangle
            Get
                Return Me.m_rectangle
            End Get
        End Property

        ''' <summary>
        ''' Sets the bounding rectangle for this button.
        ''' </summary>
        ''' <param name="rect"></param>
        Protected Friend Overridable Sub SetRectangle(rect As Rectangle)
            Me.m_rectangle = rect
        End Sub

        ''' <summary>
        ''' Creates a new instance of this class with the specified
        ''' button type (Up or Down)
        ''' </summary>
        ''' <param name="buttonType">The scroll button type to create.</param>
        Public Sub New(buttonType As ListBarScrollButtonType)
            Me.m_buttonType = buttonType
        End Sub
    End Class
#End Region

#Region "IMouseObject interface"
    ''' <summary>
    ''' An internal interface specifying the properties and methods which must
    ''' be supported by an object in the control which interacts with the
    ''' mouse.
    ''' TODO: think of a better name for this interface
    ''' </summary>
    Friend Interface IMouseObject
        ''' <summary>
        ''' Gets/sets the point at which the mouse button was
        ''' pressed.
        ''' </summary>
        Property MouseDownPoint() As Point
        ''' <summary>
        ''' Gets/sets the tooltip text for this object.
        ''' </summary>
        Property ToolTipText() As String
        ''' <summary>
        ''' Gets/sets whether the mouse is over the object or not.
        ''' </summary>
        Property MouseOver() As Boolean
        ''' <summary>
        ''' Gets/sets whether the mouse was pressed on the object or not.
        ''' </summary>
        Property MouseDown() As Boolean
    End Interface
#End Region

#Region "ListBarDragDropInsertPoint class"
    ''' <summary>
    ''' An internal class to manage the drag-drop insert point
    ''' within the control.
    ''' </summary>
    Friend Class ListBarDragDropInsertPoint
        Implements IComparable
        ''' <summary>
        ''' The item before the drag-drop insert point, if any
        ''' </summary>
        Private m_itemBefore As ListBarItem
        ''' <summary>
        ''' The item after the drag-drop insert point, if any 
        ''' </summary>
        Private m_itemAfter As ListBarItem
        ''' <summary>
        ''' If we're over an empty bar.
        ''' </summary>
        Private m_overEmptyBar As Boolean

        ''' <summary>
        ''' Compares this object with another object of the same type.
        ''' This implementation is only really useful for testing equality
        ''' </summary>
        ''' <param name="obj">Another ListBarDragDropInsertPoint object</param>
        ''' <returns>A 32-bit signed integer that indicates the relative order of the comparands.  
        ''' The return value has these meanings: 
        ''' &lt; 0: This instance is less than obj.  
        ''' 0: This instance is equal to obj.  
        ''' &gt; 0: This instance is greater than obj. </returns>
        Public Overridable Function CompareTo(obj As System.Object) As System.Int32 Implements IComparable.CompareTo
            Dim ret As Integer = 1
            Dim compare As ListBarDragDropInsertPoint = DirectCast(obj, ListBarDragDropInsertPoint)
            If compare.ItemBefore Is Me.ItemBefore Then
                If compare.ItemAfter Is Me.ItemAfter Then
                    If compare.OverEmptyBar = Me.OverEmptyBar Then
                        ret = 0
                    End If
                End If
            End If
            Return ret
        End Function


        ''' <summary>
        ''' Returns the item before the drag-drop point, if any.  At least one
        ''' of the properties ItemBefore or ItemAfter will return an item.
        ''' </summary>
        Public ReadOnly Property ItemBefore() As ListBarItem
            Get
                Return Me.m_itemBefore
            End Get
        End Property
        ''' <summary>
        ''' Returns the item after the drag-drop point, if any.  At least one
        ''' of the properties ItemBefore or ItemAfter will return an item.
        ''' </summary>
        Public ReadOnly Property ItemAfter() As ListBarItem
            Get
                Return Me.m_itemAfter
            End Get
        End Property

        ''' <summary>
        ''' Returns whether the drag point is over an empty bar
        ''' or not.
        ''' </summary>
        Public ReadOnly Property OverEmptyBar() As Boolean
            Get
                Return Me.m_overEmptyBar
            End Get
        End Property

        ''' <summary>
        '''  Constructs a new instance of this class, setting the items
        '''  before and after the drag-drop insertion point.
        ''' </summary>
        ''' <param name="itemBefore">Item before the drag-drop insertion
        ''' point, or null if no item before.</param>
        ''' <param name="itemAfter">Item after the drag-drop insertion
        ''' point, or null if no item after.</param>
        ''' <param name="overEmptyBar">Whether the drag-drop insertion
        ''' point should be displayed in an empty bar.</param>
        Public Sub New(itemBefore As ListBarItem, itemAfter As ListBarItem, overEmptyBar As Boolean)
            Me.m_itemBefore = itemBefore
            Me.m_itemAfter = itemAfter
            Me.m_overEmptyBar = overEmptyBar
        End Sub
    End Class
#End Region

#Region "Utility class (static methods)"
    ''' <summary>
    ''' An internal class holding static utility methods for the ListBar
    ''' control.
    ''' </summary>
    Friend Class ListBarUtility

        ''' <summary>
        ''' Private constructor - all methods are intended to be static so you shouldn't be able to create an instance of the class. intentionally blank
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' Fills a right-angled triangle using the specified brush.  The
        ''' origin of the triangle is taken to be the right-angle corner.
        ''' </summary>
        ''' <param name="gfx">Graphics object to draw onto.</param>
        ''' <param name="brush">Brush to fill the right-angled triangle with.</param>
        ''' <param name="origin">Location of the right-angle corner of the triangle.</param>
        ''' <param name="adjacent">The length of the adjacent side of the triangle.</param>
        ''' <param name="opposite">The length of the opposite side of the triangle.</param>
        Public Shared Sub FillRightAngleTriangle(gfx As Graphics, brush As Brush, origin As Point, adjacent As Integer, opposite As Integer)
            Dim path As New GraphicsPath()
            path.AddLine(origin.X, origin.Y, origin.X + adjacent, origin.Y)
            path.AddLine(origin.X + adjacent, origin.Y, origin.X, origin.Y + opposite)
            path.CloseFigure()
            gfx.FillPath(brush, path)
            path.Dispose()
        End Sub

        ''' <summary>
        ''' Blends two colours together using the specified alpha amount.
        ''' </summary>
        ''' <param name="colorFrom">Base colour</param>
        ''' <param name="colorTo">Colour to blend with the base colour.</param>
        ''' <param name="alpha">Alpha amount to use when blending the colours.</param>
        ''' <returns>The blended colour.</returns>
        Public Shared Function BlendColor(colorFrom As Color, colorTo As Color, alpha As Integer) As Color
            Dim retColor As Color = Color.FromArgb(((colorFrom.R * alpha) \ 255) + ((colorTo.R * (255 - alpha)) \ 255), ((colorFrom.G * alpha) \ 255) + ((colorTo.G * (255 - alpha)) \ 255), ((colorFrom.B * alpha) \ 255) + ((colorTo.B * (255 - alpha)) \ 255))
            Return retColor
        End Function
    End Class
#End Region

End Namespace
