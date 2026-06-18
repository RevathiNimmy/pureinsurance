SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
SET NOCOUNT ON
GO

EXECUTE DDLDropProcedure 'spu_terms_of_payment_sel'
GO

CREATE  PROCEDURE spu_terms_of_payment_sel(@terms_of_payment_id int)

AS

IF @terms_of_payment_id IS NULL OR @terms_of_payment_id = -1

    SELECT 
        terms_of_payment_id,
        caption_id,code,
        isnull(description,''),
        is_deleted,
        effective_date,
        number_of_days 
    FROM  terms_of_payment 
    WHERE is_deleted <> 1 
    ORDER BY number_of_days ASC

ELSE

   SELECT 	
       terms_of_payment_id,
       caption_id,code,
       isnull(description,''),
       is_deleted,
       effective_date,
       number_of_days 
   FROM  terms_of_payment
   WHERE terms_of_payment_id = @terms_of_payment_id
   AND   is_deleted <> 1
   ORDER BY number_of_days ASC 


GO
SET NOCOUNT OFF

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS ON 
GO

