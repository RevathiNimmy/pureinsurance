SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Dashboard'
GO
CREATE PROCEDURE spu_SAM_Dashboard  
       @lparty_cnt INT,
	   @lagent_cnt INT = NULL
AS

BEGIN
    SET NOCOUNT ON;

    DECLARE @Is_Broker INT = 0;

    IF @lagent_cnt IS NOT NULL
    BEGIN
        IF (SELECT party_agent_type_id FROM Party_Agent WHERE party_cnt = @lagent_cnt) = 1
        BEGIN
            SET @Is_Broker = 1;
        END
    END

    -- Tax Table as Temp Table
    SELECT 
        tc.insurance_file_cnt, 
        SUM(tc.value) AS total_tax 
    INTO #TaxTable
    FROM Tax_Calculation tc
    INNER JOIN Insurance_File i ON i.insurance_file_cnt = tc.insurance_file_cnt
    WHERE i.insured_cnt = @lparty_cnt
      AND i.insurance_file_type_id IN (2, 5, 6, 8, 9, 11) AND transtype in ('TTR','TTIF')
    GROUP BY tc.insurance_file_cnt;

    -- Fee Table as Temp Table
    SELECT 
        pfu.insurance_file_cnt, 
        SUM(pfu.base_fee_amount) AS total_fee
    INTO #FeeTable
    FROM policy_fee_u pfu
    INNER JOIN Insurance_File i ON i.insurance_file_cnt = pfu.insurance_file_cnt
    WHERE i.insured_cnt = @lparty_cnt
      AND i.insurance_file_type_id IN (2, 5, 6, 8, 9, 11)
    GROUP BY pfu.insurance_file_cnt;

    -- 1. Risk Type-wise Premium
    SELECT 
        rt.description AS RiskType,
        Round(SUM(  
			COALESCE(rs.this_premium, 0) +  
            COALESCE(tt.total_tax, 0) +  
            COALESCE(ft.total_fee, 0)  
            ),0)  AS Premium
    FROM insurance_file_risk_link AS ifr  
    INNER JOIN Insurance_File i ON i.insurance_file_cnt = ifr.insurance_file_cnt  
    INNER JOIN Rating_Section AS rs ON rs.risk_cnt = ifr.risk_cnt  
    INNER JOIN risk AS r ON r.risk_cnt = rs.risk_cnt  
    INNER JOIN Risk_Type AS rt ON r.risk_type_id = rt.risk_type_id  
    LEFT JOIN #TaxTable tt ON tt.insurance_file_cnt = i.insurance_file_cnt
    LEFT JOIN #FeeTable ft ON ft.insurance_file_cnt = i.insurance_file_cnt
    WHERE 
        (
            (@Is_Broker = 1 AND i.insured_cnt = @lparty_cnt AND i.lead_agent_cnt = @lagent_cnt)
            OR
            (@Is_Broker <> 1 AND i.insured_cnt = @lparty_cnt)
        )
        AND i.insurance_file_type_id IN (2, 5, 6, 8, 9, 11)
    GROUP BY rt.description;

    -- 2. Written Premium vs Claim Incurred by Year
    SELECT 
        a.fYear AS [Year],
        a.WrittenPremium,
        ISNULL(b.Claim_Incurred, 0) AS ClaimIncurred
    FROM
    (
        SELECT  
            YEAR(r.inception_date) AS fYear,
            ROUND(SUM(
                COALESCE(rs.this_premium, 0) + 
                COALESCE(tt.total_tax, 0) + 
                COALESCE(ft.total_fee, 0)
            ), 0) AS WrittenPremium
        FROM insurance_file_risk_link ifr
        INNER JOIN Insurance_File i ON i.insurance_file_cnt = ifr.insurance_file_cnt
        INNER JOIN Rating_Section rs ON rs.risk_cnt = ifr.risk_cnt
        INNER JOIN Risk r ON r.risk_cnt = rs.risk_cnt
        LEFT JOIN #TaxTable tt ON tt.insurance_file_cnt = i.insurance_file_cnt
        LEFT JOIN #FeeTable ft ON ft.insurance_file_cnt = i.insurance_file_cnt
        WHERE 
            (
                (@Is_Broker = 1 AND i.insured_cnt = @lparty_cnt AND i.lead_agent_cnt = @lagent_cnt)
                OR
                (@Is_Broker <> 1 AND i.insured_cnt = @lparty_cnt)
            )
            AND i.insurance_file_type_id IN (2, 5, 6, 8, 9, 11)
        GROUP BY YEAR(r.inception_date)
    ) a
    LEFT JOIN
    (
        SELECT  
            YEAR(c.Loss_from_date) AS fYear,
            ROUND(ISNULL(SUM(r.Initial_reserve + r.revised_reserve), 0), 0) AS Claim_Incurred
        FROM Reserve r
        INNER JOIN claim_peril cp ON r.claim_peril_id = cp.claim_peril_id
        INNER JOIN claim c ON cp.claim_id = c.claim_id
        INNER JOIN Insurance_File ifi ON ifi.insurance_file_cnt = c.Policy_id
        WHERE 
            (
                (@Is_Broker = 1 AND ifi.insured_cnt = @lparty_cnt AND ifi.lead_agent_cnt = @lagent_cnt)
                OR
                (@Is_Broker <> 1 AND ifi.insured_cnt = @lparty_cnt)
            )
            AND r.version_id = (SELECT MAX(version_id) FROM claim WHERE claim_Number = c.claim_Number AND is_dirty = 0)
        GROUP BY YEAR(c.Loss_from_date)
    ) b ON a.fYear = b.fYear
    ORDER BY a.fYear;

    -- 3. Claim Outstanding by Year
    SELECT  
        YEAR(c.Loss_from_date) AS fyear, 
        Round(ISNULL(SUM(r.Initial_reserve + r.revised_reserve - r.Paid_to_date), 0),0) AS ClaimOutstanding
    FROM Reserve r  
    JOIN claim_peril cp ON r.claim_peril_id = cp.claim_peril_id  
    JOIN claim c ON cp.claim_id = c.claim_id  
    JOIN Insurance_File ifi ON ifi.insurance_file_cnt = c.Policy_id  
    WHERE 
        (
            (@Is_Broker = 1 AND ifi.insured_cnt = @lparty_cnt AND ifi.lead_agent_cnt = @lagent_cnt)
            OR
            (@Is_Broker <> 1 AND ifi.insured_cnt = @lparty_cnt)
        )
        AND r.version_id = (SELECT MAX(version_id) FROM claim WHERE claim_Number = c.claim_Number AND is_dirty = 0)
    GROUP BY YEAR(c.Loss_from_date);

    -- 4. Premium Till Last Year
    SELECT ROUND(SUM(  
                COALESCE(rs.this_premium, 0) +  
                COALESCE(tt.total_tax, 0) +  
                COALESCE(ft.total_fee, 0)  
            ), 0) AS Premium_Till_Last_Year  
    FROM insurance_file_risk_link AS ifr  
    INNER JOIN Insurance_File i ON i.insurance_file_cnt = ifr.insurance_file_cnt  
    INNER JOIN Rating_Section AS rs ON rs.risk_cnt = ifr.risk_cnt  
    INNER JOIN risk AS r ON r.risk_cnt = rs.risk_cnt  
    LEFT JOIN #TaxTable tt ON tt.insurance_file_cnt = i.insurance_file_cnt
    LEFT JOIN #FeeTable ft ON ft.insurance_file_cnt = i.insurance_file_cnt
    WHERE 
        (
            (@Is_Broker = 1 AND i.insured_cnt = @lparty_cnt AND i.lead_agent_cnt = @lagent_cnt)
            OR
            (@Is_Broker <> 1 AND i.insured_cnt = @lparty_cnt)
        )
        AND i.insurance_file_type_id IN (2, 5, 6, 8, 9, 11)  
        AND r.inception_date < DATEADD(YEAR, -1, GETDATE());

    -- 5. Claim Incurred Till Last Year
    SELECT ROUND(SUM(r.Initial_reserve + r.revised_reserve), 0) AS Claim_Incurred_Till_Last_Year
    FROM Reserve r  
    JOIN claim_peril cp ON r.claim_peril_id = cp.claim_peril_id  
    JOIN claim c ON cp.claim_id = c.claim_id  
    JOIN Insurance_File ifi ON ifi.insurance_file_cnt = c.Policy_id  
    WHERE 
        (
            (@Is_Broker = 1 AND ifi.insured_cnt = @lparty_cnt AND ifi.lead_agent_cnt = @lagent_cnt)
            OR
            (@Is_Broker <> 1 AND ifi.insured_cnt = @lparty_cnt)
        )
        AND r.version_id = (SELECT MAX(version_id) FROM claim WHERE claim_Number = c.claim_Number AND is_dirty = 0)
        AND c.Loss_from_date < DATEADD(YEAR, -1, GETDATE());

	-- 6. Premium Till Two Year Back
	SELECT ROUND(SUM(  
                COALESCE(rs.this_premium, 0) +  
                COALESCE(tt.total_tax, 0) +  
                COALESCE(ft.total_fee, 0)  
            ), 0) AS Premium_Till_Two_Years_Back  
    FROM insurance_file_risk_link AS ifr  
    INNER JOIN Insurance_File i ON i.insurance_file_cnt = ifr.insurance_file_cnt  
    INNER JOIN Rating_Section AS rs ON rs.risk_cnt = ifr.risk_cnt  
    INNER JOIN risk AS r ON r.risk_cnt = rs.risk_cnt  
    LEFT JOIN #TaxTable tt ON tt.insurance_file_cnt = i.insurance_file_cnt
    LEFT JOIN #FeeTable ft ON ft.insurance_file_cnt = i.insurance_file_cnt
    WHERE 
        (
            (@Is_Broker = 1 AND i.insured_cnt = @lparty_cnt AND i.lead_agent_cnt = @lagent_cnt)
            OR
            (@Is_Broker <> 1 AND i.insured_cnt = @lparty_cnt)
        )
        AND i.insurance_file_type_id IN (2, 5, 6, 8, 9, 11)  
        AND r.inception_date < DATEADD(YEAR, -2, GETDATE());

	-- 7. Claim Incurred Till Year Back
	SELECT ROUND(SUM(r.Initial_reserve + r.revised_reserve), 0) AS Claim_Incurred_Till_Two_Years_Back
    FROM Reserve r  
    JOIN claim_peril cp ON r.claim_peril_id = cp.claim_peril_id  
    JOIN claim c ON cp.claim_id = c.claim_id  
    JOIN Insurance_File ifi ON ifi.insurance_file_cnt = c.Policy_id  
    WHERE 
        (
            (@Is_Broker = 1 AND ifi.insured_cnt = @lparty_cnt AND ifi.lead_agent_cnt = @lagent_cnt)
            OR
            (@Is_Broker <> 1 AND ifi.insured_cnt = @lparty_cnt)
        )
        AND r.version_id = (SELECT MAX(version_id) FROM claim WHERE claim_Number = c.claim_Number AND is_dirty = 0)
        AND c.Loss_from_date < DATEADD(YEAR, -2, GETDATE());

	-- 8. 
	SELECT
			insurance_ref AS Policy_Id,
			FORMAT(renewal_date, 'dd MMM yyyy') AS Renewal_Date,
			(CASE 
				WHEN i.insurance_file_type_id = 1 THEN 'QUOTE'
				WHEN i.insurance_file_type_id = 2 THEN 'LIVE'
				WHEN i.insurance_file_type_id = 3 THEN 'IN RENEWAL'
				WHEN i.insurance_file_type_id = 8 THEN 'CANCELLED'
				WHEN i.insurance_file_type_id = 11 THEN 'WRITTEN'
				ELSE NULL
			END) AS Status,
			p.code AS Code,
        Round(SUM(  
			COALESCE(rs.this_premium, 0) +  
            COALESCE(tt.total_tax, 0) +  
            COALESCE(ft.total_fee, 0)  
            ),0)  AS Premium
    FROM insurance_file_risk_link AS ifr  
    INNER JOIN Insurance_File i ON i.insurance_file_cnt = ifr.insurance_file_cnt  
    INNER JOIN Rating_Section AS rs ON rs.risk_cnt = ifr.risk_cnt  
    INNER JOIN risk AS r ON r.risk_cnt = rs.risk_cnt  
    INNER JOIN Risk_Type AS rt ON r.risk_type_id = rt.risk_type_id
	INNER JOIN Product AS p ON p.product_id = i.product_id
	INNER JOIN Insurance_File_Type AS ift on ift.insurance_file_type_id = i.insurance_file_type_id
    LEFT JOIN #TaxTable tt ON tt.insurance_file_cnt = i.insurance_file_cnt
    LEFT JOIN #FeeTable ft ON ft.insurance_file_cnt = i.insurance_file_cnt
	WHERE 
        (
            (@Is_Broker = 1 AND i.insured_cnt = @lparty_cnt AND i.lead_agent_cnt = @lagent_cnt)
            OR
            (@Is_Broker <> 1 AND i.insured_cnt = @lparty_cnt)
        ) AND i.insurance_file_type_id IN (1,2,3,8,11)
	GROUP BY 
	i.insurance_file_type_id,
    i.insurance_ref,
    i.renewal_date,
	p.code

---9 NoofPolicies NoofOpenClaims NoofCloseClaims
  DECLARE @NoofPolicies int,  
@NoofOpenClaims int,  
@NoofCloseClaims int  

 Declare @Claim TABLE  
 ( Claim_id INT  
 )   
  
If @Is_Broker = 0
BEGIN
	Select @NoofPolicies= count (distinct iff.insurance_folder_cnt) from insurance_folder iff
	JOIN Insurance_File ifi ON ifi.insurance_folder_cnt = iff.insurance_folder_cnt  where iff.insurance_holder_cnt=@lparty_cnt

    INSERT INTO @Claim  
	SELECT MAX(Claim_ID) FROM Claim JOIN insurance_file ON Insurance_file.Insurance_file_cnt = Claim.Policy_ID  
	WHERE Insured_Cnt = @lparty_cnt AND ISNULL(is_dirty,0)=0 GROUP BY base_claim_id 
END
ELSE
BEGIN
	Select @NoofPolicies= count (distinct iff.insurance_folder_cnt) from insurance_folder iff
	JOIN Insurance_File ifi ON ifi.insurance_folder_cnt = iff.insurance_folder_cnt  where iff.insurance_holder_cnt=@lparty_cnt AND ifi.lead_agent_cnt = @lagent_cnt

    INSERT INTO @Claim  
	SELECT MAX(Claim_ID) FROM Claim JOIN insurance_file ON Insurance_file.Insurance_file_cnt = Claim.Policy_ID  
	WHERE Insured_Cnt = @lparty_cnt and lead_agent_cnt=@lagent_cnt AND ISNULL(is_dirty,0)=0 GROUP BY base_claim_id 
END

Select @NoofCloseClaims=count(*) from claim where claim_id in  
                                                (  
                                                Select Claim_id FROM @Claim --WHERE ISNULL(is_dirty,0)=0  
                                                )  
                                                and claim_status_id=3  
  
Select @NoofOpenClaims=count(*)  from claim where claim_id in  
                                                (  
                                                Select Claim_id FROM @Claim --WHERE ISNULL(is_dirty,0)=0  
                                                )  
                                                and claim_status_id<>3 and is_dirty<>1  
  
SELECT @NoofPolicies NoofPolicies,@NoofOpenClaims NoofOpenClaims,@NoofCloseClaims NoofCloseClaims  
    -- Clean up temp tables  
	
    DROP TABLE IF EXISTS #TaxTable;
    DROP TABLE IF EXISTS #FeeTable;
END;
GO