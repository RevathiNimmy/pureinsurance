
CREATE VIEW [dbo].[ReinsurancePerilView]
AS
      SELECT      risk_cnt,
            peril_type_id,
            peril_type_code,
            peril_type_description,
            annual_premium,
            this_premium,
            ri_sum_insured,
            rating_sum_insured,
            is_premium,
            is_sum_insured,
            sum_insured,
            ri_band,
            total_ri_premium,
            CASE
                  WHEN ri_sum_insured IS NULL THEN 0
                  WHEN ri_sum_insured = 0     THEN 0
                  ELSE sum_insured * ( total_ri_sum_insured/ri_sum_insured)
            END AS total_ri_sum_insured,
            Section,
            SectionTypeId,
            round(annual_premium_inclusive, 2) annual_premium_inclusive,
            round(this_premium_inclusive, 2) this_premium_inclusive,
            round(transaction_premium_inclusive, 2) transaction_premium_inclusive,
            insurance_file_cnt
  FROM dbo.ReinsurancePerilSumInsuredView

GO


