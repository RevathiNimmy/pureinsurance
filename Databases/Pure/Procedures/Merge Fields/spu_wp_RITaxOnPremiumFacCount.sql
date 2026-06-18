SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_RITaxOnPremiumFacCount'
GO


CREATE PROCEDURE spu_wp_RITaxOnPremiumFacCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

    SELECT 	Count(*) As how_many
    FROM 	tax_calculation tc
    JOIN    ri_arrangement_line ral 
            ON ral.ri_arrangement_line_id = tc.ri_arrangement_line_id
    JOIN    party p
            ON p.party_cnt = ral.party_cnt
            AND p.party_cnt = tc.ri_party_cnt
    JOIN    tax_band tb
            ON tb.tax_band_id = tc.tax_band_id
    JOIN    tax_type tt
            ON tt.tax_type_id = tb.tax_type_id
    WHERE 	tc.risk_cnt = @RiskId
    AND     tc.transtype = 'TTRIFP'


GO


