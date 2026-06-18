-------------------------------------------------------------------------------
-- Author:  AMB
-- Desc:    SELECT the first monthly renewal frequency ID from the 
--          renewal_frequency table
-- Product: SFB 1.8.6
-- Date:    30-Oct-2003
-------------------------------------------------------------------------------

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_monthly_renewal_freq_id_sel'
GO

CREATE PROCEDURE spu_monthly_renewal_freq_id_sel
(
    @renewal_frequency_id    int OUTPUT
)
AS 

SELECT TOP 1
    @renewal_frequency_id = renewal_frequency_id
FROM 
    renewal_frequency
WHERE
    number_of_months = 1
AND 
    effective_date <= getdate()




