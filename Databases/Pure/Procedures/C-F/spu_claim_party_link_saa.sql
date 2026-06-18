SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_claim_party_link_saa'
GO


CREATE PROCEDURE spu_claim_party_link_saa
    @claim_id int,
    @code varchar(20),
    @risk_type_id int,
    @peril_type_id int    
AS

IF @code=''
    SELECT @code=NULL
    
IF @peril_type_id > 0
BEGIN    
    
	SELECT
		cpl.party_cnt,
		p.name,
		a.address1,
		'',
		po.license_number,
		po.date_of_birth,
		po.gender,
		ds.description,
		po.contact_name,
		po.contact_telephone_number,
		pt.code,
		pt.description
	FROM
	claim_party_link cpl
	INNER JOIN party p ON p.party_cnt=cpl.party_cnt
	INNER JOIN party_other po ON po.party_cnt=p.party_cnt
	INNER JOIN party_type pt ON p.party_type_id=pt.party_type_id
	LEFT OUTER JOIN driver_status ds ON ds.driver_status_id=po.party_status
	LEFT OUTER JOIN party_address_usage pau ON pau.party_cnt=p.party_cnt
	LEFT OUTER JOIN address a ON a.address_cnt=pau.address_cnt
	LEFT OUTER JOIN address_usage_type aut ON pau.address_usage_type_id=aut.address_usage_type_id AND aut.code='3131 XCO'
	WHERE
	cpl.claim_id=@claim_id
	AND cpl.peril_type_id=@peril_type_id	
	AND pt.code=ISNULL(@code,pt.code)
	ORDER BY cpl.party_cnt ASC
END
ELSE
BEGIN
	SELECT
		cpl.party_cnt,
		p.name,
		a.address1,
		'',
		po.license_number,
		po.date_of_birth,
		po.gender,
		ds.description,
		po.contact_name,
		po.contact_telephone_number,
		pt.code,
		pt.description
	FROM
	claim_party_link cpl
	INNER JOIN party p ON p.party_cnt=cpl.party_cnt
	INNER JOIN party_other po ON po.party_cnt=p.party_cnt
	INNER JOIN party_type pt ON p.party_type_id=pt.party_type_id
	LEFT OUTER JOIN driver_status ds ON ds.driver_status_id=po.party_status
	LEFT OUTER JOIN party_address_usage pau ON pau.party_cnt=p.party_cnt
	LEFT OUTER JOIN address a ON a.address_cnt=pau.address_cnt
	LEFT OUTER JOIN address_usage_type aut ON pau.address_usage_type_id=aut.address_usage_type_id AND aut.code='3131 XCO'
	WHERE
	cpl.claim_id=@claim_id
	AND cpl.risk_type_id=@risk_type_id
	AND pt.code=ISNULL(@code,pt.code)
	ORDER BY cpl.party_cnt ASC
END

GO


