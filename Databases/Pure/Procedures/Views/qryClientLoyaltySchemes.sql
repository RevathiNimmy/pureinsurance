SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropView 'qryClientLoyaltySchemes'
GO

CREATE VIEW qryClientLoyaltySchemes AS 

SELECT
    pls.party_cnt 'ClientID',
    ls.code 'LoyaltySchemeCode',
    ls.description 'LoyaltySchemeDesc',
    pls.membership_number 'MembershipNumber',
    pls.other_reference 'OtherReference',
    pls.start_date 'StartDate',
    pls.end_date 'EndDate',
    pls.main_membership_number 'MainMembershipNumber',
    pls.is_active 'IsActive'
FROM party_loyalty_scheme pls
JOIN loyalty_scheme ls
    ON ls.loyalty_scheme_id = pls.loyalty_scheme_id
WHERE ls.is_deleted = 0

GO