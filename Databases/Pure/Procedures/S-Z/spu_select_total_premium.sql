SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_select_total_premium'
GO


CREATE PROCEDURE spu_select_total_premium
    @Insurance_File_Cnt int,
    @Risk_Id int
AS

--    @Total_This_Premium numeric(19,4) OUTPUT
DECLARE    @Total_This_Premium      numeric(19,4)
BEGIN
    /****************************************************************************/
    /* PURPOSE:  SELECTS the extra type and its associated Extra data from the  */
    /*           extra_type table.                                              */
    /*                                                                          */
    /* DOCUMENTS:\\arp\c\windows\personal\Extras.doc                            */
    /*                                                                          */
    /* INPUTS:   @risk_id                                                       */
    /*           @trans_type_basis                                              */
    /*           @effective_date                                                */
    /*                                                                          */
    /* OUTPUTS:..the following rows using a SELECT statement:-                  */
    /*           description                                                    */
    /*           calculation_method                                             */
    /*           percent                                                        */
    /*           value                                                          */
    /*           is_taxable                                                     */
    /*           is_commissionable                                              */
    /*           code                                                           */
    /*           extra_type_id                                                  */
    /*                                                                          */
    /****************************************************************************/
    /****************************************************************************/
    /* Revision Description of Modification                    Date       Who   */
    /* 1.0      Original                                       14/08/1997 PH    */
    /****************************************************************************/
   /*
     If a Risk_Id isn't supplied then...
   */
   IF @Risk_Id < 1
      /*
        ...get Total_This_Premium from Insurance_File...
      */
      BEGIN
         SELECT @Total_This_Premium = I.This_Premium
           FROM Insurance_File I
          WHERE Insurance_File_Cnt = @Insurance_File_Cnt
      END
   ELSE
      /*
        ...otherwise get Total_This_Premium from Risk.
      */
      BEGIN
         /* SELECT @Total_This_Premium = R.Total_This_Premium */
    SELECT @Total_This_Premium = R.Sum_insured_requested
           FROM Risk R
          WHERE R.Risk_cnt = @Risk_Id
      END
END
SELECT @Total_This_Premium Total_This_Premium
GO


