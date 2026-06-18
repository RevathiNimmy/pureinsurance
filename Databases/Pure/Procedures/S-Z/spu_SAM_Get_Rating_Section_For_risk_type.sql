
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE Ddldropprocedure 'spu_SAM_Get_Rating_Section_For_risk_type'
GO

CREATE PROCEDURE Spu_sam_get_rating_section_for_risk_type 
	@risk_type_id INT
AS
BEGIN
	SELECT     rst.rating_section_type_id  RatingSectionId,
			   RTRIM(code)                        RatingSectionCode ,
			   [Description],
			   RST.rate_type_id,
			   RST.Rate,
			   RST.currency_id,
			   RST.country_id,
			   RST.state_id,
			   (SELECT Earning_Pattern_id
				FROM   Earning_Pattern_Usage
				WHERE  Rating_Section_type_id = RST.rating_section_type_id
					   AND Earning_Pattern_Usage_id = (SELECT MAX(Earning_Pattern_Usage_id)
													   FROM   Earning_Pattern_Usage
													   WHERE  Rating_Section_type_id = RST.rating_section_type_id
															  AND effective_date <= Getdate())) 'Earning_Pattern_id'
	FROM       rating_section_type rst
	INNER JOIN Risk_Type_Rating_Section_Type rtrst
		ON rst.rating_section_type_id = rtrst.rating_section_type_id
	WHERE      risk_type_id = @risk_type_id
	AND 	   rst.is_deleted = 0	
END

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 
