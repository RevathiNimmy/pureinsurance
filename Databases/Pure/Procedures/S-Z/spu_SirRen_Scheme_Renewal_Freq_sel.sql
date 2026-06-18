SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Scheme_Renewal_Freq_sel'
GO

CREATE PROCEDURE spu_SirRen_Scheme_Renewal_Freq_sel
    @risk_group_id INT,
    @renewal_frequency_id INT OUTPUT,
    @months_to_add INT OUTPUT,
    @midnight_renewal BIT OUTPUT

AS


SELECT    
    @renewal_frequency_id = gsc.renewal_frequency_id,
    @months_to_add = rnf.number_of_months
FROM gis_scheme gsc
JOIN gis_qem_usage gqu 
    ON gqu.gis_scheme_id = gsc.gis_scheme_id
JOIN risk_group rgp 
    ON rgp.risk_group_id = gqu.risk_group_id 
JOIN renewal_frequency rnf 
    ON rnf.renewal_frequency_id = gsc.renewal_frequency_id
WHERE rgp.risk_group_id = @risk_group_id
AND gsc.scheme_ver = 
    (
        SELECT    
            MAX(gsc.scheme_ver)
        FROM gis_scheme gsc
        JOIN gis_qem_usage gqu 
            ON gqu.gis_scheme_id = gsc.gis_scheme_id
        JOIN risk_group rgp 
            ON rgp.risk_group_id = gqu.risk_group_id 
        WHERE RGP.Risk_Group_Id = @Risk_Group_Id
    )

IF @renewal_frequency_id IS NULL
BEGIN
    SELECT 
        @renewal_frequency_id = rnf.renewal_frequency_id,
        @months_to_add = 0
    FROM renewal_frequency rnf
    WHERE UPPER(code) = 'ANNUAL'
END

SELECT    
    @midnight_renewal = rgp.midnight_renewal
FROM risk_group rgp 
WHERE rgp.risk_group_id = @risk_group_id

GO