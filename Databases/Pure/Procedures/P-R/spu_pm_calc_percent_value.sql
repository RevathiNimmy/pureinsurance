SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pm_calc_percent_value'
GO


CREATE PROCEDURE spu_pm_calc_percent_value
    @total_value numeric(19,4),
    @percentage numeric(19,4),
    @number_of_dp int,
    @o_percentage_value numeric(19,4) OUTPUT
AS

/* sp_pm_SIR scripts */
Begin

      -- Calculate the percentage value
      Select @o_percentage_value  = (@total_value * @percentage) / 100

      -- Round off the result to the given decimal place
      Select @o_percentage_value = Round(@o_percentage_value,@number_of_dp)

End
GO


