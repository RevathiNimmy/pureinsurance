SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_select_extra_value'
GO


CREATE PROCEDURE spu_select_extra_value
    @Insurance_File_Cnt int,
    @Risk_Id int
AS


BEGIN
    /****************************************************************************/
    /* PURPOSE:  SELECTS an Extra from either Ins_File_Extra_Value or           */
    /*           Risk_Extra_Value, depending upon whether the supplied          */
    /*           Risk_Id < 1                                                    */
    /*                                                                          */
    /* DOCUMENTS:\\arp\c\windows\personal\Extras.doc                            */
    /*                                                                          */
    /* INPUTS:   @Insurance_File_Cnt                                            */
    /*           @Risk_Id                                                       */
    /*                                                                          */
    /* OUTPUTS:..the following rows using a SELECT statement:-                  */
    /*           Caption                                                        */
    /*           Calculation_Method                                             */
    /*           Percent                                                        */
    /*           Value                                                          */
    /*           Is_Taxable                                                     */
    /*           Is_Commissionable                                              */
    /*       Code                                                           */
    /*       Extra_Type_Id                                                  */
    /*                                                                          */
    /****************************************************************************/
    /****************************************************************************/
    /* Revision Description of Modification                    Date       Who   */
    /* 1.0      Original                                       14/08/1997 PH    */
    /****************************************************************************/
   /*
     Check if risk or insurance file
   */
   IF @Risk_Id < 1
      BEGIN
         /*
           Select data from Ins_File_Extra_Value
         */
         SELECT DISTINCT
                P.Caption,
                E.Calculation_Method,
                I.[Percent],
                I.Value,
                I.Is_Taxable,
                I.Is_Commissionable,
        E.Code,
        I.Extra_Type_Id
           FROM Ins_File_Extra_Value I,
                PMCaption P,
                Extra_Type E
          WHERE I.Insurance_File_Cnt = @Insurance_File_Cnt
            AND I.Extra_Type_Id = E.Extra_Type_Id
            AND E.Caption_Id = P.Caption_Id
          ORDER BY P.Caption, E.Calculation_Method
      END
   ELSE
      BEGIN
         /*
           Select data from Risk_Extra_Value
         */
         SELECT DISTINCT
                P.Caption,
                E.Calculation_Method,
                R.Percentage,
                R.Value,
                R.Is_Taxable,
                R.Is_Commissionable,
        E.Code,
        R.Extra_Type_Id
           FROM Risk_Extra_Value R,
                PMCaption P,
                Extra_Type E
--          WHERE R.Insurance_File_Cnt = @Insurance_File_Cnt
          WHERE R.Risk_Cnt = @Risk_id
            AND R.Extra_Type_Id = E.Extra_Type_Id
            AND E.Caption_Id = P.Caption_Id
          ORDER BY P.Caption, E.Calculation_Method
      END
END
GO


