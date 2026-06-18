SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_single_party_claim_sel'
GO


CREATE PROCEDURE spu_single_party_claim_sel
    @party_cnt int
AS


SELECT
    p.party_cnt,
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
party p
INNER JOIN party_other po ON po.party_cnt=p.party_cnt
INNER JOIN party_type pt ON p.party_type_id=pt.party_type_id
LEFT OUTER JOIN driver_status ds ON ds.driver_status_id=po.party_status
LEFT OUTER JOIN party_address_usage pau ON pau.party_cnt=p.party_cnt
LEFT OUTER JOIN address a ON a.address_cnt=pau.address_cnt
LEFT OUTER JOIN address_usage_type aut ON pau.address_usage_type_id=aut.address_usage_type_id AND aut.code='3131 XCO'
WHERE
p.party_cnt=@party_cnt

GO


