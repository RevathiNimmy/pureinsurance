SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_RITaxOnCommissionTreaty'
GO


CREATE PROCEDURE spu_wp_RITaxOnCommissionTreaty
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

    DECLARE reinsurance_result SCROLL CURSOR FOR
        SELECT 	t.description AS Reinsurer_Treaty,
                p.resolved_name AS Reinsurer_Name,
                tt.description AS Reinsurer_TaxType,
                tc.Premium AS Reinsurer_Premium,
                tc.percentage AS Reinsurer_TaxRate,
        		tc.value AS Reinsurer_TaxValue
        FROM 	tax_calculation tc
        JOIN    ri_arrangement_line ral 
                ON ral.ri_arrangement_line_id = tc.ri_arrangement_line_id
        JOIN	treaty t 
                ON t.treaty_id = ral.treaty_id
        JOIN    treaty_party tp 
                ON tp.treaty_id = t.treaty_id
                AND tp.party_cnt = tc.ri_party_cnt
        JOIN    party p
                ON p.party_cnt = tp.party_cnt
        JOIN    tax_band tb
                ON tb.tax_band_id = tc.tax_band_id
        JOIN    tax_type tt
                ON tt.tax_type_id = tb.tax_type_id
        WHERE 	tc.risk_cnt = @RiskId
        AND     tc.transtype = 'TTRITC'
        AND     ral.type IN ('R', 'T')

    OPEN reinsurance_result
    
    FETCH ABSOLUTE @Instance1
    FROM reinsurance_result
    
    CLOSE reinsurance_result
    DEALLOCATE reinsurance_result


GO


