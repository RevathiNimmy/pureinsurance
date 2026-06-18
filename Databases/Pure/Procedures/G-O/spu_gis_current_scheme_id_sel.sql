EXEC DDLDropProcedure 'spu_gis_current_scheme_id_sel'
GO

CREATE PROCEDURE spu_gis_current_scheme_id_sel
    @gis_scheme_id INTEGER,
    @effective_date datetime,
    @gis_business_type_code CHAR(10)
AS

BEGIN

/********************************************************************************************************/
/* Stored Procedure spu_gis_current_scheme_id_sel selects the effective scheme details for a given scheme*/
/********************************************************************************************************/

/********************************************************************************************************/
/* Revision             Description of Modification                                     Date        Who */
/* --------             ---------------------------                                     ----        --- */
/* 1.0                  Original                            				31/07/2000  RFC */
/* 1.1                  Amended to take into account of Business Type.                  12/10/2000  RFC */
/* 1.2                  Matches on gis_business_type_id removed as this doesn't                         */
/*                                           work for multi-channel sharing of schemes. 23/11/2000  RAG */
/* 1.3			GII needs the gis_business_type_id link to work correctly. Add a check for the  */
/*			business type beginning with 'GII', which should be Gemini II only. If it does  */
/*			use SQL that has the link, otherwise not.		        02/04/2003  IDP */
/* 1.4                  Gemini II MUST filter out non valid schemes or cannot do MTA's  07/06/2004  SJD */
/* 1.5  Gemini II MUST ensure country_id is maintained for inner join or cannot do MTA's  08/04/05  JRD */
/* 1.6 For Polaris schemes scheme variants must match when doing MTA 15/05/2006 SPW */
/********************************************************************************************************/
DECLARE @gis_business_type_id INTEGER

  IF LEFT(@gis_business_type_code,3) <> 'GII'
  BEGIN
	SELECT DISTINCT gso.gis_scheme_id ,
	    gso.gis_quote_engine_id ,
	    gso.gis_business_type_id ,
	    gso.gis_insurer_id ,
	    gso.filename ,
	    gso.scheme_no ,
	    gso.scheme_ver ,
	    gso.scheme_status ,
	    gso.start_date ,
	    gso.scheme_desc ,
	    gso.priority ,
	    gso.agency_code ,
	    gso.product_code ,
	    gso.activation_level ,
	    gso.printing_privileges ,
	    gso.broker_group ,
	    gso.commision_perc ,
	    gso.quote_day_num ,
	    gso.selection_day_num ,
	    gso.invite_day_num ,
	    gso.confirm_day_num ,
	    gso.lapse_day_num ,
	    gso.max_change_num ,
	    gso.min_change_num ,
	    gso.expiry_date ,
	    gso.qm_insurer_ref ,
	    gso.scheme_type_flags ,
	    gso.edi_mail_box ,
	    gso.refer_email_address ,
	    gso.refer_fax_number ,
	    gso.scheme_type ,
	    gso.scheme_variant
	FROM    gis_scheme gs
	INNER JOIN gis_scheme gso
	ON  (gs.gis_insurer_id = gso.gis_insurer_id  AND  gs.scheme_no = gso.scheme_no)
	WHERE   gs.gis_scheme_id = @gis_scheme_id
	  AND   gso.start_date <= @effective_date
	  AND   (gso.expiry_date >= @effective_date OR gso.expiry_date IS NULL)
  END
  ELSE
  BEGIN
	SELECT  @gis_business_type_id = gis_business_type_id
	FROM    gis_business_type
	WHERE   code = @gis_business_type_code

	SELECT DISTINCT gso.gis_scheme_id ,
	    gso.gis_quote_engine_id ,
	    gso.gis_business_type_id ,
	    gso.gis_insurer_id ,
	    gso.filename ,
	    gso.scheme_no ,
	    gso.scheme_ver ,
	    gso.scheme_status ,
	    gso.start_date ,
	    gso.scheme_desc ,
	    gso.priority ,
	    gso.agency_code ,
	    gso.product_code ,
	    gso.activation_level ,
	    gso.printing_privileges ,
	    gso.broker_group ,
	    gso.commision_perc ,
	    gso.quote_day_num ,
	    gso.selection_day_num ,
	    gso.invite_day_num ,
	    gso.confirm_day_num ,
	    gso.lapse_day_num ,
	    gso.max_change_num ,
	    gso.min_change_num ,
	    gso.expiry_date ,
	    gso.qm_insurer_ref ,
	    gso.scheme_type_flags ,
	    gso.edi_mail_box ,
	    gso.refer_email_address ,
	    gso.refer_fax_number ,
	    gso.scheme_type ,
	    gso.scheme_variant
	FROM    gis_scheme gs
	INNER JOIN gis_scheme gso
	ON  (gs.gis_insurer_id = gso.gis_insurer_id
	AND  gs.scheme_no = gso.scheme_no
	AND  gso.scheme_status > 0
	AND  gs.qm_insurer_ref = gso.qm_insurer_ref
        AND  ISNULL(gs.scheme_variant,0) = ISNULL(gso.scheme_variant,0)
	AND  ISNULL(gs.country_id,1) = ISNULL(gso.country_id,1))
	WHERE   gs.gis_scheme_id = @gis_scheme_id
	  AND   gs.gis_business_type_id = @gis_business_type_id
	  AND   gso.gis_business_type_id = @gis_business_type_id
	  AND   gso.start_date <= @effective_date
	  AND   (gso.expiry_date >= @effective_date OR gso.expiry_date IS NULL)
  END
END
GO


