SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_is_policy_valid_for_void_transaction'
GO

CREATE PROCEDURE spu_is_policy_valid_for_void_transaction
    @user_id INT,
    @insurance_file_cnt INT
AS
BEGIN
    DECLARE @tmp_insurance_file_cnt INT;
    DECLARE @UserRight VARCHAR(30);
    DECLARE @renewal_count INT;
    DECLARE @Insurance_Folder INT;
    DECLARE @RetValue INT;
    DECLARE @PolicyVersion INT;
	DECLARE @InstalmentExists INT = 0
	DECLARE @QuoteExists INT = 0

    SET @RetValue = 1;

    SELECT @Insurance_Folder = ISNULL(ifi.Insurance_Folder_cnt, 0)
    FROM Insurance_File AS ifi
    WHERE ifi.insurance_file_cnt = @insurance_file_cnt;

    SELECT @PolicyVersion = MIN(Policy_version)
    FROM Insurance_File AS ifi
    WHERE ifi.insurance_folder_cnt = @Insurance_Folder;

    SET @PolicyVersion = ISNULL(@PolicyVersion, 0);

    SELECT @UserRight = ISNULL(UA.void_policy_version, '')
    FROM User_Authorities AS UA
    WHERE UA.user_id = @user_id;

    -- User rights checks
    IF @UserRight = 'Not Allowed'
    BEGIN
        SET @RetValue = 0;
    END

    IF @UserRight = 'Current Period Only'
    BEGIN
        SELECT @renewal_count = COUNT(*)
        FROM Insurance_File AS ifi
        LEFT JOIN insurance_file_type AS IFT
            ON IFT.insurance_file_type_id = ifi.insurance_file_type_id
        WHERE ifi.insurance_folder_cnt = @Insurance_Folder
          AND ifi.insurance_file_cnt > @insurance_file_cnt
          AND IFT.code = 'VOIDRENREP';

        SET @renewal_count = ISNULL(@renewal_count, 0);

        IF @renewal_count >= 1
        BEGIN
            SET @RetValue = 0;
        END
    END

    IF @UserRight = 'Current Period + 1'
    BEGIN
        SELECT @renewal_count = COUNT(*)
        FROM Insurance_File AS ifi
        LEFT JOIN insurance_file_type AS IFT
            ON IFT.insurance_file_type_id = ifi.insurance_file_type_id
        WHERE ifi.insurance_folder_cnt = @Insurance_Folder
          AND ifi.insurance_file_cnt > @insurance_file_cnt
          AND IFT.code = 'VOIDRENREP';

        SET @renewal_count = ISNULL(@renewal_count, 0);

        IF @renewal_count >= 2
        BEGIN
            SET @RetValue = 0;
        END
    END

    IF @UserRight = 'Unrestricted'
    BEGIN
        SET @RetValue = 1;
    END

    DECLARE @LossID INT;

    SELECT @LossID = loss_id
    FROM stats_folder
    WHERE insurance_file_cnt = @insurance_file_cnt
      AND loss_id IS NOT NULL;

    SET @LossID = ISNULL(@LossID, 0);

    IF @LossID > 0
    BEGIN
        SET @RetValue = 0;
    END

    IF @RetValue = 1
    BEGIN
        -- check for backdated version
        DECLARE @BaseInsuranceFileCnt INT;
        DECLARE @BackDatedInsFileStatus VARCHAR(50);

        SELECT @BaseInsuranceFileCnt = Base_Insurance_File_Cnt
        FROM Insurance_File AS ifi
        LEFT JOIN insurance_file_status AS ifs
            ON ifs.insurance_file_status_id = ifi.insurance_file_status_id
        WHERE ifi.insurance_file_cnt = @insurance_file_cnt;

        SET @BaseInsuranceFileCnt = ISNULL(@BaseInsuranceFileCnt, 0);

        IF @BaseInsuranceFileCnt > 0
        BEGIN
            SELECT TOP 1 @BackDatedInsFileStatus = ifs.code
            FROM Insurance_File AS ifi
            LEFT JOIN Insurance_File_Status AS ifs
                ON ifs.insurance_file_status_id = ifi.insurance_file_status_id
            WHERE Insurance_File_cnt < @BaseInsuranceFileCnt
              AND Insurance_Folder_cnt = @Insurance_Folder
            ORDER BY Insurance_File_Cnt DESC;

            IF @BackDatedInsFileStatus = 'REPBDMTA'
            BEGIN
                SET @RetValue = 0;
            END
        END
    END

    -- Policy version validation
    IF @RetValue = 1
    BEGIN
        SELECT TOP 1 @tmp_insurance_file_cnt = ISNULL(ifi.insurance_file_cnt, 0)
        FROM Insurance_File AS ifi
        LEFT JOIN insurance_file_type AS IFT
            ON IFT.insurance_file_type_id = ifi.insurance_file_type_id
        LEFT JOIN Product AS p
            ON p.product_id = ifi.product_id
        LEFT JOIN Stats_Folder AS SF
            ON SF.insurance_file_cnt = ifi.insurance_file_cnt
        WHERE ifi.insurance_folder_cnt = @Insurance_Folder
          AND IFT.code IN ('POLICY', 'MTA PERM', 'MTA TEMP', 'MTAREINS')
          AND ISNULL(p.void_policy_version, 0) = 1
          AND (ifi.insurance_file_status_id IS NULL OR ifi.insurance_file_status_id = 3)
          AND ifi.policy_version > @PolicyVersion
        ORDER BY ifi.insurance_file_cnt DESC;

        SET @tmp_insurance_file_cnt = ISNULL(@tmp_insurance_file_cnt, 0);

        IF @tmp_insurance_file_cnt <> @insurance_file_cnt
        BEGIN
            SET @RetValue = 0;
        END
    END

	IF @RetValue = 1
	BEGIN 

	select @InstalmentExists= pfprem_finance_cnt  from PFPremiumFinance where insurance_file_cnt = @insurance_file_cnt

	
	Set @InstalmentExists = ISNULL(@InstalmentExists,0)
	IF @InstalmentExists > 0 Set @InstalmentExists = 1
	  
	SELECT @QuoteExists  = count(1)    
	FROM insurance_file  IFL    
    JOIN Insurance_file_type T ON IFL.insurance_file_type_id = T.insurance_file_type_id    
	WHERE insurance_folder_cnt = @Insurance_Folder    
        AND T.Code in ( 'MTAQUOTE' ,'MTAQTETEMP' ,'RENEWAL','MTAQREINS', 'MTAQCAN')  
		AND insurance_file_cnt > @insurance_file_cnt  

		Set @QuoteExists = ISNULL(@QuoteExists,0)
		IF @QuoteExists > 0 Set @QuoteExists = 1
	END 
     
  
    SELECT @RetValue AS IsValid, @InstalmentExists  AS InstalmentExist,  @QuoteExists AS  QuoteExists


    
END
GO