SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_update_ri_arrangement_line'
GO


CREATE PROCEDURE spu_update_ri_arrangement_line
    @run_bal_si money OUTPUT,
    @run_bal_percent float OUTPUT,
    @ri_band_si money,
    @ri_band_premium money,
    @ri_arrangement_line_id integer,
    @this_share_percent float,
    @premium_percent float
AS


/*******************************************************************************************************/
/* Stored Procedure spu_update_ri_arrangement_line calculates the running totals and updates a         */
/* reinsurance arrangement line from the supplied values.                                              */
/*                                                                                                     */
/* Note: Refer to Reinsurance Technical Specification - Process 17.0, 18.0                             */
/*******************************************************************************************************/

/*******************************************************************************************************/
/* Revision            Description of Modification                                    Date         Who */
/* --------            ---------------------------                                    ----         --- */
/* 1.0                 Original                                                       29/04/1997   RFC */
/* 1.1                 Default Share Percent does not need to be updated &            06/06/1997   RFC */
/*                     always calculate running balance si.                                            */
/*******************************************************************************************************/

DECLARE 
    @printstring varchar(40),
    @sum_insured money,
    @premium_value money

-- Running Balance Percent
SELECT @run_bal_percent = @run_bal_percent - @this_share_percent

-- Running Balance Sum Insured 
SELECT @run_bal_si = @run_bal_percent * (@ri_band_si / 100)


-- This Sum Insured 
SELECT @sum_insured = @this_share_percent * (@ri_band_si / 100)

-- This Premium 
SELECT @premium_value = @premium_percent * (@ri_band_premium / 100)


-- Update the Reinsurance Arrangement Line (including commission)
UPDATE  ri_arrangement_line
SET     this_share_percent = @this_share_percent,
        premium_percent = @premium_percent,
        sum_insured = @sum_insured,
        premium_value = @premium_value,
        commission_value = ISNULL((@premium_value * commission_percent) / 100, 0)
WHERE   ri_arrangement_line_id = @ri_arrangement_line_id


GO



