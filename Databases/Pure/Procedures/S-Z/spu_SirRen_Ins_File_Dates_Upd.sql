EXEC DDLDropProcedure spu_SirRen_Ins_File_Dates_Upd
GO

CREATE PROCEDURE spu_SirRen_Ins_File_Dates_Upd
    @Insurance_File_Cnt INT
AS
/********************************************************************************
 Stored procedure sp_SirRen_Ins_File_Dates_Upd takes parameter          
 @InsuranceFileCnt to uniquely identify an insurance file.              
 The procedure moves all policy dates on year forward               
                                            
 Revision   Modification        
 -----------------------------------------------------------------------------
 1.0 Original 20/08/2001 IM
 2.0          06/05/2003 SJP (CMG)
                             ISS3588 Removed assumption that policy is for 1 year,
                             by using renewal frequency where found.                      
 3.0          13/01/2004 DD  Enhanced date logic for True Monthly policies
 4.0          06/08/2004 RDC Fixed the last-day-of-month date logic
 5.0          15/02/2005 CJB Also set the date_issued to be the current date.
********************************************************************************/

DECLARE @Renewal_Months_Add INT
DECLARE @Anniversary_Date DATETIME
DECLARE @Old_Expiry_Date DATETIME
DECLARE @Next_Date DATETIME
DECLARE @Months INT
DECLARE @Midnight_Renewal TINYINT

SELECT 
    @Midnight_Renewal = RG.midnight_renewal 
FROM Risk_Group RG 
JOIN Risk_Code RC 
    ON RC.risk_group_id = RG.risk_group_id
JOIN Insurance_File I 
    ON I.Risk_Code_id = RC.Risk_Code_id 
WHERE I.insurance_file_cnt = @Insurance_File_Cnt

    
SELECT 
    @Renewal_Months_Add = rfq.number_of_months,
    @Anniversary_Date = ifi.anniversary_date
FROM renewal_frequency rfq
LEFT JOIN insurance_file ifi 
    ON ifi.renewal_frequency_id = rfq.renewal_frequency_id
WHERE ifi.insurance_file_cnt = @Insurance_File_Cnt

IF @Renewal_Months_Add IS NULL
BEGIN
    SELECT 
        @Old_Expiry_Date = DATEADD(yy, 1, cover_start_date)
    FROM insurance_file
    WHERE insurance_file_cnt = @Insurance_File_Cnt

    UPDATE insurance_file
    SET cover_start_date = @Old_Expiry_Date,
        expiry_date = DATEADD(yy, 1, expiry_date),
        renewal_date = DATEADD(yy, 1, renewal_date),
        lapsed_date = DATEADD(yy, 1, lapsed_date),
        date_issued = GETDATE(),
        inception_date_tpi = @Old_Expiry_Date
    WHERE insurance_file_cnt = @Insurance_File_Cnt
END
ELSE
BEGIN
    SELECT 
        @Old_Expiry_Date = expiry_date
    FROM insurance_file
    WHERE insurance_file_cnt = @Insurance_File_Cnt

    IF @Renewal_Months_Add=1 AND @Anniversary_Date IS NOT NULL
    BEGIN

        SELECT @Months = 0
        SELECT @Next_Date = @Anniversary_Date

        WHILE month(@Old_Expiry_Date) <> month(@Next_Date)
        BEGIN
            SELECT @Months = @Months - 1
            SELECT @Next_Date = DATEADD(m, @Months, @Anniversary_Date)
        END

        SELECT @Next_Date = DATEADD(m, @Months + 1, @Anniversary_Date)

        UPDATE insurance_file
        SET cover_start_date = @Old_Expiry_Date,
            expiry_date = @Next_Date,
            renewal_date = @Next_Date,
            lapsed_date = @Next_Date,
            date_issued = GETDATE(),
            inception_date_tpi = @Old_Expiry_Date
        WHERE insurance_file_cnt = @Insurance_File_Cnt
    END
    ELSE 
    BEGIN

        IF @Midnight_Renewal = 1
        BEGIN
            SELECT @Old_Expiry_Date = DATEADD(d, 1, @Old_Expiry_Date)
        END
        
        UPDATE insurance_file
        SET cover_start_date = @Old_Expiry_Date,
            expiry_date = DATEADD(mm, @Renewal_Months_Add, expiry_date),
            renewal_date = DATEADD(mm, @Renewal_Months_Add, renewal_date),
            lapsed_date = DATEADD(mm, @Renewal_Months_Add, lapsed_date),
            date_issued = GETDATE(),
            inception_date_tpi = @Old_Expiry_Date
        WHERE insurance_file_cnt = @Insurance_File_Cnt
            
    END
END

GO
