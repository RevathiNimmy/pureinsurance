SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_contact'
GO


CREATE PROCEDURE spu_wp_contact
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT
AS


SELECT
        'corr_address_line_1' = a.address1,
        'corr_address_line_2' = a.address2,
        'corr_address_line_3' = a.address3,
        'corr_address_line_4' = a.address4,
        'corr_postcode' = a.postal_code
    FROM    party_address_usage pau,
        address_usage_type aut,
        address a
    WHERE   pau.party_cnt = @PartyCnt
    AND pau.address_cnt = a.address_cnt
    AND pau.address_usage_type_id = aut.address_usage_type_id
    AND aut.code = "3131 XCO"
GO


