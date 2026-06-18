
EXECUTE DDLDropProcedure 'spu_SAM_Select_All_Product_Source'
GO
CREATE Procedure  spu_SAM_Select_All_Product_Source
	@user_id INT
AS
	SELECT RTRIM(s.code) as Code,
	RTrim(s.description) As description , P.Product_id
	FROM source s
	JOIN product_source p
	ON p.source_id = s.source_id

	--AND p.product_id = @product_id

	WHERE (p.source_id not in (SELECT source_id from PMUser_Source where user_id=@user_id) )
	ORDER BY p.product_id

