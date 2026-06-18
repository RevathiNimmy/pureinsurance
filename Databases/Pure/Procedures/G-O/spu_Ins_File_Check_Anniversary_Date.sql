SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Ins_File_Check_Anniversary_Date'
GO

CREATE PROCEDURE spu_Ins_File_Check_Anniversary_Date
   @insurance_file_cnt INT
AS 

DECLARE @dtAnniversaryDate DATETIME
DECLARE @dtCoverStartDate DATETIME
DECLARE @bUpdateAnniversaryDate BIT
DECLARE @dtRenewalDate DATETIME

/*Default to not updating the anniversary date*/
SELECT @bUpdateAnniversaryDate = 0

/*Retrieve current values from policy*/
SELECT 
    @dtAnniversaryDate = anniversary_date,
    @dtRenewalDate = renewal_date,
    @dtCoverStartDate = cover_start_date
FROM insurance_file 
WHERE insurance_file_cnt = @insurance_file_cnt

/*If there is an anniversary date and if the renewal date is greater than or equal to the anniversary date then increment it and update the policy with it.*/
/*If there isn't an anniversary date then do not update it as this will cause errors for non True Monthly policies.*/
IF @dtAnniversaryDate IS NOT NULL 
BEGIN   
    
    IF YEAR(@dtRenewalDate) > YEAR(@dtAnniversaryDate)
    BEGIN
        SELECT @bUpdateAnniversaryDate = 1
    END
    ELSE
    BEGIN
        IF YEAR(@dtRenewalDate) = YEAR(@dtAnniversaryDate)
        BEGIN
            IF MONTH(@dtRenewalDate) >= MONTH(@dtAnniversaryDate)
            BEGIN
                 SELECT @bUpdateAnniversaryDate = 1
            END
        END 
    END
    
    IF @bUpdateAnniversaryDate = 1 
    BEGIN

        /*Advance the current anniversary date on by a year*/
        SELECT @dtAnniversaryDate = DATEADD(yyyy, 1, @dtAnniversaryDate)

        UPDATE insurance_file 
        SET anniversary_date = @dtAnniversaryDate 
        WHERE insurance_file_cnt = @insurance_file_cnt

    END

END



