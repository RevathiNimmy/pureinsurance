Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 16092002
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMLossSchedule"
	
	'Captions
	' Interface Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTab1 As Integer = 101
	Public Const ACTab2 As Integer = 102
	Public Const ACTab3 As Integer = 103
	
	'LossScheduleType Form
	Public Const ACLossScheduleTypeTitle As Integer = 104
	Public Const ACLossTab1 As Integer = 105
	
	'Assign Form
	Public Const ACAssignTitle As Integer = 106
	Public Const ACAssignTab1 As Integer = 107
	
	'Add Form
	Public Const ACAddTitle As Integer = 108
	Public Const ACDetailsTab As Integer = 109
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACApplyButton As Integer = 202
	Public Const ACAddButton As Integer = 203
	Public Const ACEditButton As Integer = 204
	Public Const ACAssignButton As Integer = 205
	
	' Interface Controls
	Public Const ACTotals As Integer = 300
	Public Const ACPaymentAmount As Integer = 301
	Public Const ACSubTotal As Integer = 302
	Public Const ACExtras As Integer = 303
	Public Const ACTotal As Integer = 304
	Public Const ACExcess As Integer = 305
	Public Const ACViewOptions As Integer = 306
	Public Const ACNormal As Integer = 307
	Public Const ACCompressed As Integer = 308
	Public Const ACFilter As Integer = 309
	Public Const ACLossScheduleType As Integer = 310
	Public Const ACPayeeOrSupplier As Integer = 311
	Public Const ACStatus As Integer = 312
	
	'ListView Columns (general)
	Public Const AClvwDate As Integer = 313
	Public Const AClvwItemNumber As Integer = 314
	Public Const AClvwItemClaimed As Integer = 315
	Public Const AClvwItemDescription As Integer = 316
	Public Const AClvwSettlementMethod As Integer = 317
	Public Const AClvwStartingValue As Integer = 318
	Public Const AClvwAge As Integer = 319
	Public Const AClvwLife As Integer = 320
	Public Const AClvwDepreciation As Integer = 321
	Public Const AClvwGST As Integer = 322
	Public Const AClvwItemAmount As Integer = 323
	Public Const AClvwPaymentAmount As Integer = 324
	Public Const AClvwExcess As Integer = 325
	Public Const AClvwPayeeOrSupplier As Integer = 326
	Public Const AClvwItemStatus As Integer = 327
	Public Const AClvwPODate As Integer = 328
	Public Const AClvwDatePaid As Integer = 329
	Public Const AClvwSalvage As Integer = 330
	
	'Shortened Listview Columns (for compressed mode)
	Public Const AClvwCompItemNumber As Integer = 331
	Public Const AClvwCompItemClaimed As Integer = 332
	Public Const AClvwCompItemDescription As Integer = 333
	Public Const AClvwCompSettlementMethod As Integer = 334
	Public Const AClvwCompPayeeOrSupplier As Integer = 335
	
	'ListView Columns (MVPC)
	Public Const AClvwDamagedArea As Integer = 336
	Public Const AClvwRepairable As Integer = 337
	Public Const AClvwModelNo As Integer = 338
	Public Const AClvwPartsRequest As Integer = 339
	Public Const AClvwStripFit As String = "340"
	Public Const AClvwStripFitHrs As Integer = 341
	Public Const AClvwParts As String = "342"
	Public Const AClvwFreight As String = "343"
	Public Const AClvwPaint As String = "344"
	Public Const AClvwPaintHrs As Integer = 345
	Public Const AClvwPanel As String = "346"
	Public Const AClvwPanelHrs As Integer = 347
	Public Const AClvwOutwork As Integer = 348
	
	'Shortened Listview (MVPC) Columns (for compressed mode)
	Public Const AClvwCompDamaged As Integer = 349
	
	' Add Form Controls
	Public Const ACAddDateEntered As Integer = 351
	Public Const ACAddItemNumber As Integer = 352
	Public Const ACAddItemClaimed As Integer = 353
	Public Const ACAddItemDescription As Integer = 354
	Public Const ACAddSettlementMethod As Integer = 355
	Public Const ACAddStartingValue As Integer = 356
	Public Const ACAddAge As Integer = 357
	Public Const ACAddLife As Integer = 358
	Public Const ACAddDepreciationPercent As Integer = 359
	Public Const ACAddDepreciation As Integer = 360
	Public Const ACAddItemAmount As Integer = 361
	Public Const ACAddGST As Integer = 362
	Public Const ACAddPaymentAmount As Integer = 363
	Public Const ACAddExcess As Integer = 364
	Public Const ACAddPayeeOrSupplier As Integer = 365
	Public Const ACAddPODate As Integer = 366
	Public Const ACAddDatePaid As Integer = 367
	Public Const ACAddSalvage As Integer = 368
	Public Const ACAddSalvageYes As Integer = 369
	Public Const ACAddSalvageNo As Integer = 370
	Public Const ACAddStatus As Integer = 371
	
	
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	Public g_iLanguageID As Integer
	
	' windows helper function
	Public Declare Function BringWindowToTop Lib "user32" (ByVal hwnd As Integer) As Integer
End Module