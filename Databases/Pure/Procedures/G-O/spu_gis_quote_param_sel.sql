SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_gis_quote_param_sel'
GO


CREATE PROCEDURE spu_gis_quote_param_sel
    @gis_business_type_code CHAR(10),
    @effective_date datetime,
    @gis_policy_link_id integer = NULL,
    @gis_data_model_code CHAR(10) = NULL,
    @gis_scheme_id integer = NULL,
    @quote_type integer = NULL,
    @risk_group_id integer = NULL,
    @Called_By_Sts integer = 0,
    @Real_Transaction_Type varchar(10) = null
AS
BEGIN
/********************************************************************************************************/
/* Stored Procedure spu_gis_quote_param_sel selects the Parameters/Details required to allow the GIS     */
/* to quote.                                                                                            */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision             Description of Modification                                     Date        Who */
/* --------             ---------------------------                                     ----        --- */
/* 1.0                  Original                                                        12/07/1999  RFC */
/* 1.01                 Added type & variant to selects                                 19/07/1999  CL  */
/* 1.02                 Added comm.perc, scheme id and desc to selects                  03/08/1999  CL  */
/* 1.03                 Added ins.abi_81_insurer to selects                             09/10/1999  SJ  */
/* 1.04                 Added ability to supply a specific Scheme ID. (Optional)        29/09/1999  RFC */
/* 1.05                 Added EDI details to selects                                    04/01/2000  SJ  */
/* 1.06                 Made gis_policy_link_id optional and added an optional data     11/01/2000  RFC */
/* 1.06                 model code. This is needed for Xelector where there is no db.   11/01/2000  RFC */
/* 1.06                 Added an effective_date parameter and select the appropriate    11/01/2000  RFC */
/* 1.06                 Scheme Ver dependant on this date. Also guaranteed qte date.    11/01/2000  RFC */
/* 1.07                 If guaranteed date is null then also update it with a value.    09/03/2000  CJB */
/* 1.08                 Added optional quote_type param. Only set guaranteed quote      30/03/2000  RFC */
/* 1.08                 date if the quote_type is = 2 (i.e. Guaranteed Quote)           30/03/2000  RFC */
/* 1.09                 Added Insurer Description                                       26/07/2000  RFC */
/* 1.10                 Added dict_ver column selection                                 12/09/2000  CL  */
/* 1.11                 Added a risk group for carol nash solution (product builder)    07/11/2001  BSJ */
/* 1.12                 Added scheme_type_flags column selection                        20/11/2001  MB  */
/* 1.13                 Added Insurer Code column selection                        	    08/11/2004  JRD */
/* 1.14                 If in Party Builder mode, get a default broking scheme to use   27/06/2005  CLG */
/* 1.15                 Party Builder mode removed as is now unnecessary                27/09/2005  CLG */
/********************************************************************************************************/

DECLARE @gis_business_type_id integer,
    @gis_data_model_id integer,
    @guaranteed_quote_date datetime

/* Get the Data Model ID */
IF @gis_policy_link_id IS NULL

    /* Get the Data Model ID for this Data Model Code */
    SELECT @gis_data_model_id = gis_data_model_id
    FROM gis_data_model
    WHERE code = @gis_data_model_code

ELSE

    /* Get the Data Model ID for this Policy Link */
    SELECT @gis_data_model_id = gis_data_model_id
    FROM gis_policy_link
    WHERE gis_policy_link_id = @gis_policy_link_id

/* Get the Business Type Id From the Code */
SELECT @gis_business_type_id = gis_business_type_id
FROM gis_business_type
WHERE code = @gis_business_type_code

if @gis_business_type_code = ''
Begin
	select @gis_business_type_code = null
End

/* If we have a Policy Link ID AND Quote Type = 2 (Guaranteed) */
IF (@gis_policy_link_id IS NOT NULL) AND (@quote_type = 2)
BEGIN

    /* Get the guaranteed quote date */
    SELECT @guaranteed_quote_date = guaranteed_quote_date
    FROM gis_policy_link
    WHERE gis_policy_link_id = @gis_policy_link_id

    /* IF we are NOT doing the Quote with an effective date equal to the */
    /* guaranteed quote date then update guaranteed quote date to the current datetime */
    IF @effective_date <> @guaranteed_quote_date OR @guaranteed_quote_date IS NULL

        UPDATE gis_policy_link
        SET guaranteed_quote_date = getdate()
        WHERE gis_policy_link_id = @gis_policy_link_id

END

/* Are we Selecting a Specific Scheme */
IF @gis_scheme_id IS NOT NULL

    /* Yes, Get Specific Scheme Details*/
    /* We can ignore Effective Date as we know exactly which Scheme to get */
    SELECT qem.object_name,
    qem.class_name,
    sch.scheme_no,
    sch.scheme_ver,
    sch.filename,
    sch.qm_insurer_ref,
    ins.polaris_insurer_no,
    sch.scheme_type,
    sch.scheme_variant,
    sch.commision_perc,
    sch.gis_scheme_id,
    sch.scheme_desc,
    ins.abi_81_insurer,
    ins.abi_1_edi_directory,
    sch.agency_code,
    sch.edi_mail_box,
    ins.description,
    sch.dict_ver,
    sch.scheme_type_flags,
    ins.code
    FROM gis_qem_usage qu,
    gis_qem qem,
    gis_scheme sch,
    gis_insurer ins
    WHERE qu.gis_data_model_id = @gis_data_model_id
    AND qu.gis_business_type_id = @gis_business_type_id
    AND qu.gis_scheme_id = @gis_scheme_id
    AND qu.gis_qem_id = qem.gis_qem_id
    AND qu.gis_scheme_id = sch.gis_scheme_id
    AND sch.gis_insurer_id = ins.gis_insurer_id
    AND ((@Called_By_STS=0)
          or (@Called_by_Sts=1 and (sch.onlinestartdate is null or @Real_Transaction_Type is null))
          or (@Called_by_Sts=1 and sch.onlinestartdate is not null and
		((sch.TradeNBOnline=1 and @Real_Transaction_Type='NB')
			or (sch.TradeMTAOnline=1 and @Real_Transaction_Type='MTA')
			or (sch.TradeRNLOnline=1 and @Real_Transaction_Type='REN'))))
    ORDER BY
    qem.object_name ASC,
    qem.class_name ASC

ELSE

    /* If there are Selected Schemes */
    IF @gis_policy_link_id IS NOT NULL
        AND EXISTS (SELECT *
        FROM gis_policy_schemes_sel
        WHERE gis_policy_link_id = @gis_policy_link_id)

        /* Restrict to Only those Selected */
        /* We can ignore Effective Date as we know exactly which Schemes to get */
        SELECT qem.object_name,
        qem.class_name,
        sch.scheme_no,
        sch.scheme_ver,
        sch.filename,
        sch.qm_insurer_ref,
        ins.polaris_insurer_no,
        sch.scheme_type,
        sch.scheme_variant,
        sch.commision_perc,
        sch.gis_scheme_id,
        sch.scheme_desc,
        ins.abi_81_insurer,
        ins.abi_1_edi_directory,
        sch.agency_code,
        sch.edi_mail_box,
        ins.description,
        sch.dict_ver,
        sch.scheme_type_flags,
        ins.code
        FROM gis_qem_usage qu,
        gis_qem qem,
        gis_scheme sch,
        gis_insurer ins
        WHERE qu.gis_data_model_id = @gis_data_model_id
        AND qu.gis_business_type_id = @gis_business_type_id
        AND qu.gis_qem_id = qem.gis_qem_id
        AND qu.gis_scheme_id = sch.gis_scheme_id
        AND sch.gis_insurer_id = ins.gis_insurer_id
        AND qu.gis_scheme_id IN (
        SELECT gis_scheme_id
        FROM gis_policy_schemes_sel
        WHERE gis_policy_link_id = @gis_policy_link_id)
    AND ((@Called_By_STS=0)
          or (@Called_by_Sts=1 and (sch.onlinestartdate is null or @Real_Transaction_Type is null))
          or (@Called_by_Sts=1 and sch.onlinestartdate is not null and
		((sch.TradeNBOnline=1 and @Real_Transaction_Type='NB')
			or (sch.TradeMTAOnline=1 and @Real_Transaction_Type='MTA')
			or (sch.TradeRNLOnline=1 and @Real_Transaction_Type='REN'))))
        ORDER BY
        qem.object_name ASC,
        qem.class_name ASC

    ELSE

    /* If a risk group id is NOT provided then get all schemes that apply */
    IF @risk_group_id IS NULL

        /* Otherwise, All Schemes that Apply */
        /* Use the Effective Date param to ensure that we get the */
        /* correct versions of the Schemes */
        SELECT qem.object_name,
        qem.class_name,
        sch.scheme_no,
        sch.scheme_ver,
        sch.filename,
        sch.qm_insurer_ref,
        ins.polaris_insurer_no,
   sch.scheme_type,
        sch.scheme_variant,
        sch.commision_perc,
        sch.gis_scheme_id,
        sch.scheme_desc,
        ins.abi_81_insurer,
        ins.abi_1_edi_directory,
        sch.agency_code,
        sch.edi_mail_box,
        ins.description,
        sch.dict_ver,
        sch.scheme_type_flags,
        ins.code
        FROM gis_qem_usage qu,
        gis_qem qem,
        gis_scheme sch,
        gis_insurer ins
        WHERE qu.gis_data_model_id = @gis_data_model_id
        AND qu.gis_business_type_id = @gis_business_type_id
        AND qu.gis_qem_id = qem.gis_qem_id
        AND qu.gis_scheme_id = sch.gis_scheme_id
        AND sch.gis_insurer_id = ins.gis_insurer_id
        AND sch.start_date <= @effective_date
        AND sch.expiry_date >= @effective_date
    AND ((@Called_By_STS=0)
          or (@Called_by_Sts=1 and (sch.onlinestartdate is null or @Real_Transaction_Type is null))
          or (@Called_by_Sts=1 and sch.onlinestartdate is not null and
		((sch.TradeNBOnline=1 and @Real_Transaction_Type='NB')
			or (sch.TradeMTAOnline=1 and @Real_Transaction_Type='MTA')
			or (sch.TradeRNLOnline=1 and @Real_Transaction_Type='REN'))))
        ORDER BY
        qem.object_name ASC,
        qem.class_name ASC

    /* If a risk group id IS provided then get all schemes that apply with that risk group id*/
    ELSE

        SELECT qem.object_name,
        qem.class_name,
        sch.scheme_no,
        sch.scheme_ver,
        sch.filename,
        sch.qm_insurer_ref,
        ins.polaris_insurer_no,
        sch.scheme_type,
        sch.scheme_variant,
        sch.commision_perc,
        sch.gis_scheme_id,
        sch.scheme_desc,
        ins.abi_81_insurer,
        ins.abi_1_edi_directory,
        sch.agency_code,
        sch.edi_mail_box,
        ins.description,
        sch.dict_ver,
        sch.scheme_type_flags,
        ins.code
        FROM gis_qem_usage qu,
        gis_qem qem,
        gis_scheme sch,
        gis_insurer ins
        WHERE qu.gis_data_model_id = @gis_data_model_id
        AND qu.gis_business_type_id = @gis_business_type_id
        AND qu.gis_qem_id = qem.gis_qem_id
        AND qu.gis_scheme_id = sch.gis_scheme_id
        AND sch.gis_insurer_id = ins.gis_insurer_id
        AND sch.start_date <= @effective_date
        AND sch.expiry_date >= @effective_date
        AND (
                qem.code = 'SBO' 
                OR
                qu.risk_group_id = @risk_group_id
            )
    AND ((@Called_By_STS=0)
          or (@Called_by_Sts=1 and (sch.onlinestartdate is null or @Real_Transaction_Type is null))
          or (@Called_by_Sts=1 and sch.onlinestartdate is not null and
		((sch.TradeNBOnline=1 and @Real_Transaction_Type='NB')
			or (sch.TradeMTAOnline=1 and @Real_Transaction_Type='MTA')
			or (sch.TradeRNLOnline=1 and @Real_Transaction_Type='REN'))))
        ORDER BY
        qem.object_name ASC,
        qem.class_name ASC

END
GO

