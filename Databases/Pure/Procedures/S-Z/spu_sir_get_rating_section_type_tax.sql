SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_sir_get_rating_section_type_tax'
GO


CREATE PROCEDURE spu_sir_get_rating_section_type_tax
    @rating_section_type_id integer
AS  

	SELECT  tbr.tax_band_id,
		tbr.rate,
		ptu.allocate_percent, 
		pt.class_of_business_id 

	FROM rating_section_type rst
	
		INNER JOIN peril_type_usage ptu	ON 
			rst.peril_group_id = ptu.Peril_group_id

		INNER JOIN peril_type pt ON 
			ptu.peril_type_id = pt.peril_type_id

		INNER JOIN tax_group tg ON
			pt.tax_group = tg.tax_group_id

		INNER JOIN tax_group_tax_band tgtb ON
			tg.tax_group_id = tgtb.tax_group_id

		INNER JOIN tax_band tb ON 
			tgtb.tax_band_id = tb.tax_band_id

		INNER JOIN (Select Max(Effective_Date) as effective_date, Tax_band_id, Class_Of_Business_id from tax_band_rate
				where effective_date <=GetDAte()
				and is_deleted = 0
				group by tax_band_id, 
				class_of_business_id) tbr1 ON
			tbr1.tax_band_id = tb.tax_band_id

			INNER JOIN tax_band_rate tbr ON
				tbr1.tax_band_id = tbr.tax_band_id
			AND	tbr1.effective_date = tbr.effective_date
			AND 	tbr1.class_of_business_id = tbr.class_of_Business_id
			

	WHERE rst.rating_section_type_id = @rating_section_type_id
	AND pt.class_of_business_id = tbr.class_of_business_id	



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
