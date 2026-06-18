EXECUTE DDLDropProcedure 'spu_Is_rating_section_deleted'
GO

CREATE Procedure spu_Is_rating_section_deleted
@nRisk_cnt int,
@nRi_band_id int
AS
BEGIN
	SELECT NULL from peril p JOIN Rating_Section rs
	ON p.risk_cnt=rs.risk_cnt
	AND p.rating_section_id = rs.rating_section_id 
	WHERE p.risk_cnt=@nRisk_cnt and rs.original_flag=0
	AND p.ri_band=@nRi_band_id
END
