CREATE VIEW [dbo].[ReinsuranceFacultativeView]
AS
	SELECT		dbo.RI_Arrangement.ri_arrangement_id,
				dbo.RI_Arrangement_Line.type,
				CASE 
					WHEN dbo.RI_Arrangement_Line.Type = 'F' then 'Facultative - Proportional'
					WHEN dbo.RI_Arrangement_Line.Type = 'FX' then 'Facultative - Non Proportional'
				END	AS Placement,
				dbo.RI_Arrangement_Line.party_cnt,
				dbo.Party.name	AS reinsurance_company_name,
				dbo.RI_Arrangement_Line.this_share_percent,
				dbo.RI_Arrangement_Line.commission_percent,
				dbo.RI_Arrangement_Line.agreement_code,
				(dbo.RI_Arrangement_Line.this_share_percent/100) * (SELECT SUM(sum_insured) FROM dbo.ReinsurancePerilView WHERE dbo.ReinsurancePerilView.risk_cnt = dbo.RI_Arrangement.risk_cnt AND dbo.ReinsurancePerilView.ri_band =dbo.RI_Arrangement.ri_band_id) AS sum_insured,
				dbo.RI_Arrangement_Line.premium_value,
				dbo.RI_Arrangement_Line.commission_value,
				dbo.RI_Arrangement_Line.lower_limit,
				dbo.RI_Arrangement_Line.line_limit
				
	FROM		dbo.RI_Arrangement_Line
	INNER JOIN	dbo.RI_Arrangement	ON dbo.RI_Arrangement.ri_arrangement_id = dbo.RI_Arrangement_Line.ri_arrangement_id AND dbo.RI_Arrangement.original_flag = 0
	INNER JOIN	dbo.Party			ON dbo.Party.party_cnt = dbo.RI_Arrangement_Line.party_cnt
	WHERE		dbo.RI_Arrangement_Line.Type IN ('F','FX')
 


GO


