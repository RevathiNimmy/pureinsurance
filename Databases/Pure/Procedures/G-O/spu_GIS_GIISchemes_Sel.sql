-- AK23072004: Create spu_GIS_GIISchemes_Sel.sql
-- Retrieves list of Gemini II schemes for use in GIS Scheme Maintenance

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_GIS_GIISchemes_Sel'
GO

CREATE PROCEDURE spu_GIS_GIISchemes_Sel
    @SchemeID integer,
    @InsurerID integer,
    @RiskID integer,
    @TypeID integer
AS

DECLARE @byScheme int
DECLARE @byInsurer int
DECLARE @byRisk int

-- Set relevant flags for each search criterion provided
IF @SchemeID > 0 SELECT @byScheme = 1 ELSE SELECT @byScheme = 0
IF @InsurerID > 0 SELECT @byInsurer = 1 ELSE SELECT @byInsurer = 0
IF @RiskID > 0 SELECT @byRisk = 1 ELSE SELECT @byRisk = 0

    SELECT gs.gis_scheme_id,
           gi.gis_insurer_id,
           gs.scheme_desc,
           gs.scheme_no,
           rg.risk_group_id,
           gs.scheme_ver,
           gs.start_date,
           gs.expiry_date,
           gs.agency_code,
           gs.edi_mail_box,
           gs.scheme_status,
           gs.country_id,
           gs.filename,
           gs.quote_day_num,
           gs.selection_day_num,
           gs.invite_day_num,
           gs.confirm_day_num,
           gs.lapse_day_num,
           gs.max_change_num,
           gs.min_change_num,
           gs.housekeep_day_num,
           gs.pre_selection_day_num,
           gs.reminder_day_num,
           gs.renew_day_num,
           gs.is_insurer_lead,
           gs.polnoPrefix,
           gs.polnoStartNo,
           gs.polnoEndNo,
           gs.Rateable,
           gs.Overridable,
           gs.renewal_frequency_id,
           gs.rule_filename,
           gs.backdated_MTA_days_allowed,
           gs.quote_expiry_days,
           gs.quote_guarantee_days,
           gs.activation_level,
           gs.gis_quote_engine_id,
           gs.gis_business_type_id,
           gi.description,
           gs.priority,
           gs.product_code,
           gs.printing_privileges,
           gs.broker_group,
           gs.commision_perc,
           gs.qm_insurer_ref,
           gs.scheme_type_flags,
           gs.refer_email_address,
           gs.refer_fax_number,
           gs.scheme_type,
           gs.scheme_variant,
           gi.code,
           gi.abi_1_edi_directory,
           gi.polaris_insurer_no,
           gs.class_of_business,
           gs.TradeNbOnline,
           gs.TradeMtaOnline,
           gs.TradeRnlOnline,
           gs.OnlineStartDate,
           gs.MaximumPeriodTempMta,
           gs.MaximumPeriodPerPolicyPeriod,
           gs.MaximumNoOfTempMta,
           gs.MaximumTempDrivers,
           gs.TempVehicleLimitedMaxGroup,
           gs.TadTavCombination,
           gs.MtaQuoteValidity,
           gs.IntroducerPartyCnt
      FROM GIS_Scheme gs 
INNER JOIN GIS_Insurer gi ON gs.gis_insurer_id = gi.gis_insurer_id
 LEFT JOIN GIS_QEM_Usage gq ON gs.gis_scheme_id = gq.gis_scheme_id 
 LEFT JOIN Risk_Group rg ON gq.risk_group_id = rg.risk_group_id
     WHERE (gs.gis_business_type_id = @TypeID)
       AND ((@byScheme = 0 AND @byInsurer = 0 AND @byRisk = 0) OR -- Get all records
           -- Filter by scheme
           (@byScheme = 1 AND gs.gis_scheme_id = @SchemeID) OR
           -- Filter by insurer
           (@byInsurer = 1 AND gs.gis_insurer_id = @InsurerID AND @byRisk = 0) OR
           -- Filter by risk group
           (@byRisk = 1 AND gq.risk_group_id = @RiskID AND @byInsurer = 0) OR
           -- Filter by insurer and risk group
           (@byInsurer = 1 AND gs.gis_insurer_id = @InsurerID AND @byRisk = 1 AND 
               gq.risk_group_id = @RiskID))
  -- Order by insurer name and QM ref
  ORDER BY gi.description, gs.qm_insurer_ref

GO
