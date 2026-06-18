SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_sir_calc_premium'
GO


CREATE PROCEDURE spu_sir_calc_premium
    @rating_section_type_id int,
    @sum_insured numeric(19,4),
    @annual_rate numeric(19,4),
    @annual_premium numeric(19,4),
    @this_premium numeric(19,4)
AS


Begin

/********************************************************************************************************/
/* Stored Procedure spu_sir_calc_premium                                     */
/********************************************************************************************************/
/* Revision             Description of Modification                                     Date        Who */
/* --------             ---------------------------                                     ----        --- */
/* 1.0                  SR17082000 - Created                                */
/********************************************************************************************************/

    SET NOCOUNT ON

    --Declare variables

    DECLARE @effective_date Datetime,
        @id     int,
--          @rating_section_type_id int,
        @rate       numeric(19,4),
        @rate_type  char(2)

    --Set the effective Date
    Select @effective_date = Getdate()

    /* Get rating_section_type_id */
/*  EXEC    spu_pm_get_eff_id_from_code
            @tablename = 'Rating_Section_Type',
            @code = @rating_section_type_code,
            @effective_date = @effective_date,
            @id = @id OUTPUT

    SELECT  @rating_section_type_id = @id*/

    /* Get the rate_type and rate from the Rating section type table */
    Select @rate_type = rate_type_id,
        @rate     = rate
    From    Rating_section_type
    Where   Rating_section_type_id = @rating_section_type_id

    /* Find the annual Premium */

    Select @annual_premium =

        Case Upper(Ltrim(Rtrim(@rate_type)))

            When 'V' then    @rate

            When 'P' then    (@rate * @sum_insured) / 100

            When 'C' then    (@rate * @sum_insured) / 10000

            When 'Q' then    @rate * @sum_insured

            else   0
        End

    /* Set the This Premium */

    Select @this_premium = @annual_premium

    Select @annual_rate = @rate

--- Select @annual_rate , @annual_premium , @this_premium

End
GO


