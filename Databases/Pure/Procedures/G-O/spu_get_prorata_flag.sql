SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_prorata_flag'
GO


CREATE PROCEDURE spu_get_prorata_flag
    @insurance_file_cnt int
AS

--ISS1377 Also return short period rating flag.
SELECT  nb_prorata, mta_prorata, is_short_period_rated
FROM    product
JOIN    insurance_file ON insurance_file.product_id = product.product_id
WHERE   insurance_file_cnt = @insurance_file_cnt


GO


