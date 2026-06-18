SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIRRen_Claims_Sel'
GO

CREATE PROCEDURE spu_SIRRen_Claims_Sel

    @Insurance_File_Cnt INT

AS

IF EXISTS
    (
        SELECT
            NULL
        FROM Insurance_File 
        WHERE insurance_file_cnt = @Insurance_File_Cnt
        AND anniversary_date IS NOT NULL
    )
BEGIN    

    /*If this is the last policy for the year then retrieve all claims that were raised this year*/
    IF EXISTS
        (
            SELECT
                NULL
            FROM Insurance_File 
            WHERE insurance_file_cnt = @Insurance_File_Cnt
            AND renewal_date >= anniversary_date
        )
    BEGIN    

        SELECT 
            C.claim_id, 
            C.loss_from_date, 
            SUBSTRING(P.code, 7, 2) 'code'
        FROM Insurance_File I
        JOIN Insurance_File IX
            ON IX.insurance_folder_cnt = I.insurance_folder_cnt
            AND IX.anniversary_date = I.anniversary_date
        JOIN Claim C
            ON C.policy_id = IX.insurance_file_cnt 
        JOIN Primary_Cause P
            ON P.primary_cause_id = C.primary_cause_id
        WHERE I.insurance_file_cnt = @Insurance_File_Cnt

    END

END
ELSE
BEGIN

    SELECT 
        C.claim_id, 
        C.loss_from_date, 
        SUBSTRING(P.code, 7, 2) 'code'
    FROM Insurance_File I
    JOIN Insurance_File IX
        ON IX.insurance_folder_cnt = I.insurance_folder_cnt
    JOIN Claim C
        ON C.policy_id = IX.insurance_file_cnt 
    JOIN Primary_Cause P
        ON P.primary_cause_id = C.primary_cause_id
    WHERE I.Insurance_File_Cnt = @Insurance_File_Cnt

END

GO
