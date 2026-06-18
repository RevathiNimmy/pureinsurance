-- AK16072004: Update spu_GIS_Scheme_Add.sql
-- Updates a scheme in the Gis_Scheme table if it exists. If not, a new scheme 
-- is inserted. Updated to handle additional new fields
-- JRD29092004: Updated to include filename field
-- JRD29042005: Updated to only process SchemeGroup info if RiskGroupID passed

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_GIS_Scheme_Add'
GO

CREATE PROCEDURE spu_GIS_Scheme_Add
	@SchemeNo int,
	@SchemeDesc varchar(70),
	@SchemeVer int,
	@InsurerID int,
	@StartDate datetime,
	@ExpiryDate datetime,
	@RiskGroupID int,
	@SchemeID int,
	@AgencyCode varchar(30),
	@EDIMailBox varchar(13),
	@StatusID int,
	@CountryID int,
	@quotedayno int,
	@selectiondayno int,
	@invitedayno int,
	@confirmdayno int,
	@lapsedayno int,
	@maxchangeno int,
	@minchangeno int,
	@housekeepdayno int,
	@preselectionno int,
	@reminderdatno int,
	@renewaldayno int,
	@insurerled bit,
	@Prefix varchar(10),
	@StartNo int,
	@EndNo int,
	@Rateable bit,
	@Overridable bit,
	@polnonextno int,
	@RenewalFrequencyId int,
	@RuleFileName varchar(255),
	@OutSchemeID int OUTPUT,
	@backdated_MTA_days_allowed int,
	@quote_expiry_days int,
	@quote_guarantee_days int,
	@activation_level int,
	@gis_business_type_id int,
	@filename varchar(255),
	@MaximumPeriodTempMta int = 0,
	@MaximumPeriodPerPolicyPeriod int = 0,
	@MaximumNoOfTempMta int = 0,
	@MaximumTempDrivers int = 0,
	@TempVehicleLimitedMaxGroup tinyint = 0,
	@TadTavCombination tinyint = 0,
	@MtaQuoteValidity int = 0,
	@IntroducerPartyCnt int = -1
AS

DECLARE @ID int
DECLARE @RiskGroupCode varchar(50)
DECLARE @GroupID int
DECLARE @TempID int
DECLARE @OldStartNo int
DECLARE @OldEndNo int
DECLARE @CaptionID int

IF @activation_level = -1 SET @activation_level = NULL
IF LEN(LTRIM(RTRIM(@filename))) = 0 SET @filename = NULL

IF @SchemeID = 0 OR @SchemeID IS NULL BEGIN
    BEGIN TRANSACTION
    
	if @IntroducerPartyCnt = -1
	Begin
		select @IntroducerPartyCnt=null
	End

    INSERT INTO GIS_Scheme (gis_business_type_id, 
                scheme_no,
                scheme_desc,
                scheme_ver, 
                gis_insurer_id, 
                start_date, 
                expiry_date, 
                agency_code, 
                edi_mail_box, 
                scheme_status, 
                country_ID, 
                quote_day_num, 
                selection_day_num, 
                invite_day_num, 
                confirm_day_num, 
                lapse_day_num, 
                max_change_num, 
                min_change_num, 
                housekeep_day_num, 
                pre_selection_day_num, 
                reminder_day_num, 
                renew_day_num, 
                is_insurer_lead, 
                polnoPrefix, 
                polnoStartNo, 
                polnoEndNo, 
                Rateable, 
                Overridable, 
                polnonextno, 
                renewal_frequency_id, 
                rule_filename, 
                backdated_MTA_days_allowed, 
                quote_expiry_days, 
                quote_guarantee_days, 
                activation_level,
                filename,
                MaximumPeriodTempMta,
                MaximumPeriodPerPolicyPeriod,
                MaximumNoOfTempMta,
                MaximumTempDrivers,
                TempVehicleLimitedMaxGroup,
                TadTavCombination,
                MtaQuoteValidity,
                IntroducerPartyCnt)
         VALUES (@gis_business_type_id, 
                @SchemeNo, 
                @SchemeDesc, 
                @SchemeVer, 
                @InsurerID, 
                @StartDate, 
                @ExpiryDate, 
                @agencycode, 
                @edimailbox, 
                @statusID, 
                @countryID, 
                @quotedayno, 
                @selectiondayno, 
                @invitedayno, 
                @confirmdayno, 
                @lapsedayno,
                @maxchangeno, 
                @minchangeno, 
                @housekeepdayno, 
                @preselectionno, 
                @reminderdatno, 
                @renewaldayno, 
                @insurerled, 
                @Prefix, 
                @StartNo, 
                @EndNo, 
                @Rateable, 
                @Overridable, 
                @polnonextno, 
                @RenewalFrequencyId, 
                @RuleFileName, 
                @backdated_MTA_days_allowed, 
                @quote_expiry_days, 
                @quote_guarantee_days, 
                @activation_level,
                @filename,
                @MaximumPeriodTempMta,
                @MaximumPeriodPerPolicyPeriod,
                @MaximumNoOfTempMta,
                @MaximumTempDrivers,
                @TempVehicleLimitedMaxGroup,
                @TadTavCombination,
                @MtaQuoteValidity,
                @IntroducerPartyCnt)

    SELECT @ID = @@IDentity

	IF @RiskGroupID > 0 BEGIN
	    SELECT @RiskGroupCode = Code FROM Risk_Group WHERE risk_group_id = @RiskGroupID
	    
	    IF NOT EXISTS (SELECT Code FROM GIS_Scheme_Group WHERE Code = @RiskGroupCode) BEGIN
	        EXECUTE spu_pm_caption_id_return 1, @RiskGroupCode, @CaptionID OUTPUT
	        
	        INSERT INTO GIS_Scheme_Group (
	                    [code], 
	                    caption_id, 
	                    [description], 
	                    is_deleted, 
	                    effective_date, 
	                    gis_business_type_id)
	             VALUES (
	                    @RiskGroupCode, 
	                    @CaptionID, 
	                    RTrim(@RiskGroupCode), 
	                    0, 
	                    GetDate(), 
	                    @gis_business_type_id)
	        
	        SELECT @GroupID = @@IDENTITY
	        
	    END ELSE BEGIN
	        SELECT @GroupID = gis_scheme_group_id FROM GIS_Scheme_Group WHERE Code = @RiskGroupCode
	    END
	    
	    INSERT INTO GIS_Scheme_Group_Member(gis_scheme_id, 
	                gis_scheme_group_id)
	         VALUES (@ID, 
	                @GroupID)
	END
    
    IF @@ERROR <> 0 BEGIN 
        ROLLBACK TRANSACTION
    END ELSE BEGIN
        SELECT @OutSchemeID = @ID
        COMMIT TRANSACTION
    END
END

IF @SchemeID > 0 BEGIN
    SELECT @OutSchemeID = @SchemeID
    SELECT @OldStartNo = (SELECT polnostartno FROM GIS_Scheme WHERE gis_scheme_id = @SchemeID)
    SELECT @OldEndNo = (SELECT polnoendno FROM gis_scheme WHERE gis_scheme_id = @SchemeID)
    
    BEGIN TRANSACTION
    
    UPDATE GIS_Scheme
       SET scheme_no=@SchemeNo,
           Scheme_desc=@SchemeDesc,
           Scheme_ver=@SchemeVer,
           gis_insurer_id=@InsurerID,
           Start_date=@StartDate,
           Expiry_Date=@ExpiryDate,
           Agency_Code=@agencycode,
           Edi_mail_box=@edimailbox,
           Scheme_Status=@statusid,
           Country_id=@countryid,
           quote_day_num=@quotedayno,
           selection_day_num=@selectiondayno,
           invite_day_num=@invitedayno,
           confirm_day_num=@confirmdayno,
           lapse_day_num=@lapsedayno,
           max_change_num=@maxchangeno,
           min_change_num=@minchangeno,
           housekeep_day_num=@housekeepdayno,
           pre_selection_day_num=@preselectionno,
           reminder_day_num=@reminderdatno,
           renew_day_num=@renewaldayno,
           is_insurer_lead=@insurerled,
           polnoPrefix=@Prefix,
           polnoStartNo=@StartNo,
           polnoEndNo=@EndNo,
           Rateable=@Rateable,
           Overridable=@Overridable,
           Renewal_Frequency_Id=@RenewalFrequencyId,
           Rule_FileName=@RuleFileName,
           backdated_MTA_days_allowed=@backdated_MTA_days_allowed,
           quote_expiry_days=@quote_expiry_days,
           quote_guarantee_days=@quote_guarantee_days, 
           activation_level=@activation_level,
           filename=@filename,
           MaximumPeriodTempMta=@MaximumPeriodTempMta,
           MaximumPeriodPerPolicyPeriod=@MaximumPeriodPerPolicyPeriod,
           MaximumNoOfTempMta=@MaximumNoOfTempMta,
           MaximumTempDrivers=@MaximumTempDrivers,
           TempVehicleLimitedMaxGroup=@TempVehicleLimitedMaxGroup,
           TadTavCombination=@TadTavCombination,
           MtaQuoteValidity=@MtaQuoteValidity
     WHERE gis_scheme_id=@SchemeID
    
    if @IntroducerPartyCnt <> -1
    Begin
	update gis_Scheme set IntroducerPartyCnt=@IntroducerPartyCnt where gis_scheme_id=@schemeid
    End

    IF (@OldStartNo <> @StartNo) OR (@OldEndNo <> @EndNo) BEGIN
        UPDATE gis_scheme
           SET polnonextno = @polnonextno
         WHERE gis_scheme_id = @SchemeID
    END
    
    IF @@ERROR <> 0 BEGIN
        ROLLBACK TRANSACTION
    END ELSE BEGIN
        COMMIT TRANSACTION
    END

	IF @RiskGroupID > 0 BEGIN
	    SELECT @RiskGroupCode = Code FROM Risk_Group WHERE risk_group_id = @RiskGroupID
	    
		IF NOT EXISTS (SELECT Code FROM GIS_Scheme_Group WHERE Code = @RiskGroupCode) BEGIN
	            
	            INSERT INTO GIS_Scheme_Group ( 
	                        [code], 
	                        caption_id, 
	                        [description], 
	                        is_deleted,
	                        effective_date, 
	                        gis_business_type_id)
	                 VALUES ( 
	                        @RiskGroupCode, 
	                        0, 
	                        @RiskGroupCode, 
	                        0, 
	                        GetDate(), 
	                        @gis_business_type_id)
	            
	            SELECT @GroupID = @@IDENTITY
	        END ELSE BEGIN
	            SELECT @GroupID = gis_scheme_group_id FROM GIS_Scheme_Group WHERE Code = @RiskGroupCode
	        END
	        
	        DELETE GIS_Scheme_Group_Member WHERE gis_scheme_id = @SchemeID
	        
	        INSERT GIS_Scheme_Group_Member (gis_scheme_id, 
	               gis_scheme_group_id)
	        VALUES (@SchemeID, 
	               @GroupID)
		END	
	END

GO
