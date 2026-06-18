EXECUTE DDLDropProcedure 'spu_get_mediatype_frequencies'
GO
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_get_mediatype_frequencies
AS
BEGIN

SELECT
	mediatype.mediatype_id,
	mediatype.description,
	PFRF.pffrequency_id,
	PFFrequency.description,
	is_via_third_party

FROM mediatype
LEFT JOIN PFScheme ON mediatype.mediatype_id = PFScheme.mediatype_id
LEFT JOIN PFRF ON (PFScheme.CompanyNo = PFRF.CompanyNo AND PFScheme.SchemeNo = PFRF.SchemeNo AND PFScheme.SchemeVersion = PFRF.SchemeVersion)
LEFT JOIN PFFrequency on PFRF.pffrequency_id = PFFrequency.pffrequency_id

ORDER BY mediatype.description,PFFrequency.description

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO