SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SIRRen_HouseKeepAll'
GO

CREATE PROCEDURE spu_SIRRen_HouseKeepAll
    @DayNum INT,
    @Source_id INT = NULL,
    @insurer_mode INT = NULL
AS
/* AK  11/12/2001 - Stored procedure to housekeep all the renewal Records, after set number of days beyond Renewal_Date */
/* CJB 28/04/2004 - Filter on source_id too if not zero */
/* CJB 04/05/2004 - Filter on insurer mode too */

/* Get a list of all the insurance folders, which are supposed to be housekept */
DECLARE @Insurance_Folder_Cnt int

IF @source_id IS NULL
BEGIN
    SELECT @source_id = 0
END

IF @insurer_mode IS NULL
BEGIN
    SELECT @insurer_mode = 0
END

    -- only delete policies in renewal control with status - Renewed, Lapsed and Renewed with Alternative
DECLARE Ren_cursor CURSOR FAST_FORWARD FOR
    SELECT r.Insurance_Folder_Cnt
    FROM Renewal_Control r
    INNER JOIN insurance_folder i ON r.insurance_folder_cnt = i.insurance_folder_cnt
    INNER JOIN insurance_file inf ON r.renewal_insurance_file_cnt = inf.insurance_file_cnt
    INNER JOIN source s ON s.source_id = i.source_id
    JOIN policy_type pt
        ON pt.policy_type_id = inf.policy_type_id
        AND pt.code = 'SCHEMES'
    WHERE datediff(day, dateadd(day, @DayNum, r.Renewal_Date), getdate()) > 0
    AND Renewal_Status_Type_Id IN 
        (
            SELECT t.Renewal_Status_Type_Id
            FROM Renewal_Status_Type t
            WHERE code in ('LAPSED', 'RENEWED', 'COMPALT')
        )
    AND 
    (
        @source_id = 0
        OR
        (
            @source_id <> 0
            AND
            ISNULL(i.source_id, 0) = @source_id
        )
    )
    AND
    (
        (
            @insurer_mode = 0
            AND
            ISNULL(s.underwriting_branch_ind, 0) = 0
        )
        OR
        (
            @insurer_mode = 0
            AND
            ISNULL(s.underwriting_branch_ind, 0) = 1
            AND
            ISNULL(inf.alternate_reference, '') = ''
        )
        OR
        (
            @insurer_mode = 1 
            AND
            ISNULL(s.underwriting_branch_ind, 0) = 1
            AND
            ISNULL(inf.alternate_reference, '') <> ''
        )
    )

OPEN Ren_cursor
FETCH NEXT FROM Ren_cursor INTO @Insurance_Folder_Cnt

WHILE @@FETCH_STATUS = 0 BEGIN
    /* For each of these insurance folders, delete all the (not)required data */
    EXECUTE spu_SIRRen_HouseKeep @Insurance_Folder_Cnt

    FETCH NEXT FROM Ren_cursor INTO @Insurance_Folder_Cnt
END

CLOSE Ren_cursor
DEALLOCATE Ren_cursor

GO

