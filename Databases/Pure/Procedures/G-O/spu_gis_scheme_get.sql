-- AK19072004: Update spu_GIS_Scheme_Get.sql
-- Selects a set of schemes based on the provided criteria. Updated to select 
-- additional new fields

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_GIS_Scheme_Get'
GO

CREATE PROCEDURE spu_GIS_Scheme_Get
	@SchemeID integer,
	@InsurerID integer,
	@RiskID integer
AS

DECLARE @byScheme int
DECLARE @byInsurer int
DECLARE @byRisk int

IF @SchemeID > 0 SELECT @byScheme = 1 ELSE SELECT @byScheme = 0
IF @InsurerID > 0 SELECT @byInsurer = 1 ELSE SELECT @byInsurer = 0
IF @RiskID > 0 SELECT @byRisk = 1 ELSE SELECT @byRisk = 0

    SELECT s.gis_scheme_id,
           s.gis_insurer_id,
           s.scheme_desc,
           s.scheme_no,
           r.risk_group_id,
           s.scheme_ver,
           s.start_date,
           s.expiry_date,
           s.agency_code,
           s.edi_mail_box,
           s.scheme_status,
           s.country_id,
           s.filename,
           s.quote_day_num,
           s.selection_day_num,
           s.invite_day_num,
           s.confirm_day_num,
           s.lapse_day_num,
           s.max_change_num,
           s.min_change_num,
           s.housekeep_day_num,
           s.pre_selection_day_num,
           s.reminder_day_num,
           s.renew_day_num,
           s.is_insurer_lead,
           s.polnoPrefix,
           s.polnoStartNo,
           s.polnoEndNo,
           s.Rateable,
           s.Overridable,
           s.renewal_frequency_id,
           s.rule_filename,
           s.backdated_MTA_days_allowed,
           s.quote_expiry_days,
           s.quote_guarantee_days,
           s.activation_level,
           s.TradeNbOnline,
           s.TradeMtaOnline,
           s.TradeRnlOnline,
           s.OnlineStartDate,
           s.MaximumPeriodTempMta,
           s.MaximumPeriodPerPolicyPeriod,
           s.MaximumNoOfTempMta,
           s.MaximumTempDrivers,
           s.TempVehicleLimitedMaxGroup,
           s.TadTavCombination,
           s.MtaQuoteValidity,
           s.IntroducerPartyCnt
      FROM Gis_Scheme s 
INNER JOIN Gis_Insurer i ON s.gis_insurer_id = i.gis_insurer_id
INNER JOIN GIS_QEM_Usage l ON s.gis_scheme_id=l.gis_scheme_id
INNER JOIN Risk_Group r ON r.risk_group_id=l.risk_group_id
     WHERE (s.gis_business_type_id = 4) 
       AND ((@byInsurer = 0 AND @byScheme = 0 AND @byRisk = 0) OR
           (@byInsurer = 1 AND @InsurerID = s.gis_insurer_id AND @byRisk = 0) OR
           (@byInsurer = 1 AND @InsurerID = s.gis_insurer_id AND @byRisk = 1 AND @RiskID = r.risk_group_id) OR
           (@byScheme = 1 AND @SchemeID = s.gis_scheme_id) OR
           (@byRisk = 1 AND @RiskID = r.risk_group_id AND @byInsurer = 0))

GO
