Attribute VB_Name = "MRecordset"
' Module:   Utility functions for handling recordsets
' Shared:   Private
'
Option Explicit

' Copy rows from one recordset to another. The existing recordset Clone() method cannot be used when
' the source recordset is from the database and the destination recordset needs to be editable.
'
' The source recordset can have any CursorLocation, CursorType, LockType, Filter and Sort values;
' the data is simply read starting at the current row and moving forward until no more rows are found.
'
' If it does not yet exist, the destination recordset is automatically created with the same
' columns as the source. If it does already exist, the destination recordset is assumed to contain
' matching columns and be editable.
'
Public Sub CopyRecordset(ByVal rsFrom As ADODB.Recordset, _
    ByRef rsTo As ADODB.Recordset, _
    Optional ByVal bMakeAllColumnsNullable As Boolean = False)

    Dim fld As ADODB.Field
    Dim nAttributes As ADODB.FieldAttributeEnum

    If rsTo Is Nothing Then
        Set rsTo = New ADODB.Recordset
        rsTo.CursorLocation = adUseClient
        rsTo.CursorType = adOpenStatic
        rsTo.LockType = adLockOptimistic
        For Each fld In rsFrom.Fields
            nAttributes = fld.Attributes
            If bMakeAllColumnsNullable Then
                nAttributes = nAttributes Or adFldIsNullable Or adFldMayBeNull
            End If
            rsTo.Fields.Append fld.Name, fld.Type, fld.DefinedSize, nAttributes
            With rsTo(fld.Name)
                .NumericScale = fld.NumericScale
                .Precision = fld.Precision
            End With
        Next
        rsTo.Open
    End If

    Do Until rsFrom.EOF
        rsTo.AddNew
        For Each fld In rsFrom.Fields
            rsTo(fld.Name).Value = fld.Value
        Next
        rsTo.Update
        rsFrom.MoveNext
    Loop

End Sub

' Safely close a recordset and free up its memory regardless of what state it's in.
' Useful in any routine's clean-up code.
'
Public Sub DisposeRecordset(ByRef rs As ADODB.Recordset)

    If Not rs Is Nothing Then
        If rs.State <> adStateClosed Then
            rs.Close
        End If
        Set rs = Nothing
    End If

End Sub
