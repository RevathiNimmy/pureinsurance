SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_CardDataForAuthorisation'
GO

-- Get all required card related data prior to doing an authorisation.

CREATE PROCEDURE spu_ACT_Select_CardDataForAuthorisation 
    @insurance_file_cnt 	int
AS
    SELECT mtc.code,
	   cli.amount,
	   cli.cc_number,
	   cli.cc_name,
	   cli.cc_expiry_date,
	   cli.cc_start_date,
	   cli.cc_issue, 
	   cli.cc_pin,
	   cli.address1,
           cli.postal_code,
	   cli.cc_customer,
           cli.cashlistitem_id,
	   cli.cashlist_id,
           p.shortname,
           inf.insurance_ref, 
	   pt.description, 
           p.resolved_name
    FROM   insurance_file inf
    INNER JOIN cashlistitem        cli ON cli.cashlistitem_id        = inf.cashlistitem_id
    INNER JOIN mediatype_issuer    mti ON mti.mediatype_issuer_id    = cli.mediatype_issuer_id
    INNER JOIN mediatype_connector mtc ON mtc.mediatype_connector_id = mti.mediatype_connector_id
    INNER JOIN party               p   ON p.party_cnt                = inf.insured_cnt
    INNER JOIN party_type          pt  ON pt.party_type_id           = p.party_type_id
    WHERE  inf.insurance_file_cnt = @insurance_file_cnt
GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
