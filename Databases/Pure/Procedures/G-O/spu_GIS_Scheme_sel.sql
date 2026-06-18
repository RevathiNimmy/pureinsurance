SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_Scheme_sel'
GO


CREATE PROCEDURE spu_GIS_Scheme_sel
    @gis_scheme_id int
AS
BEGIN

SELECT
    gis_scheme_id,
    gis_quote_engine_id,
    gis_business_type_id,
    gis_insurer_id,
    scheme_no,
    scheme_ver,
    scheme_status,
    start_date,
    scheme_desc,
    priority,
    agency_code,
    product_code,
    activation_level,
    printing_privileges,
    broker_group,
    commision_perc,
    quote_day_num,
    selection_day_num,
    invite_day_num,
    confirm_day_num,
    lapse_day_num,
    max_change_num,
    min_change_num,
    expiry_date,
    qm_insurer_ref,
    scheme_type_flags,
    filename,
    edi_mail_box,
    refer_email_address,
    refer_fax_number,
    scheme_type,
    scheme_variant,
    dict_ver,
    class_of_business,
    country_id,
    housekeep_day_num,
    pre_selection_day_num,
    reminder_day_num,
    renew_day_num,
    is_insurer_lead,
    TradeNbOnline,
    TradeMtaOnline,
    TradeRnlOnline,
    OnlineStartDate,
    MaximumPeriodTempMta,
    MaximumPeriodPerPolicyPeriod,
    MaximumNoOfTempMta,
    MaximumTempDrivers,
    TempVehicleLimitedMaxGroup,
    TadTavCombination,
    MtaQuoteValidity,
    IntroducerPartyCnt
    FROM GIS_Scheme
    WHERE gis_scheme_id = @gis_scheme_id
END
GO


