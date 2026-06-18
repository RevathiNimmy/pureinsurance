SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_sir_lead_commission_upd'
GO


CREATE PROCEDURE [dbo].[spu_sir_lead_commission_upd]  
    @insurance_file_cnt int  
AS  
  
BEGIN  
  
/****************************************************************************************************/  
/* spu_sir_lead_commission_upd is part of the Commission Calculations        */  
/* It accumulates the lead agent entries from the agent_commission for each band    */  
/* and Add it to the lead_commission table                                                          */  
/*                                      */  
/*                                                                                                      */  
/****************************************************************************************************/  
/* Revision Description of Modification     Date        Who      */  
/* --------         ---------------------------         ----        ---     */  
/* 1.0      Created                 22/09/2000  SR  */  
/*                                      */  
/* 1.1      Added risk_type_id to lead_commission               */  
/*      table and consequently o associated             */  
/*      stored procs.               08/06/2001  RWH */  
/****************************************************************************************************/  
  
    Declare @Commission_band_id int,  
        @Commission_Percentage numeric(7,4),  
        @Premium numeric(19,4),  
        @Commission_value  numeric(19,4),  
        @risk_type_id int           -- RWH (08/06/01)  
  
    Declare Agent_Comm_Cursor Cursor Fast_Forward For  
       Select  Commission_band_id,     
                  Case sum(premium)
                  WHEN 0 THEN
                        0
                  ELSE        
					sum(commission_value)/sum(premium)
                  END,
            convert(numeric(19,4), sum(premium)) ,
            convert(numeric(19,4), sum(commission_value)),
            risk_type_id                        -- RWH (08/06/01) Added risk_type_id.
                  from    agent_commission
                  Where   is_lead_agent =0
                        And   insurance_file_cnt = @Insurance_file_cnt
                  Group By    Commission_Band_id,
                              risk_type_id
  
    -- Delete all the old record in the lead_commission table  
    Delete from Lead_Commission  
    Where Insurance_File_cnt = @insurance_file_cnt  
  
    Open Agent_Comm_Cursor  
  
    Fetch Next From Agent_Comm_Cursor into    @Commission_band_id ,  
                    @Commission_Percentage,  
                    @Premium,  
                    @Commission_value,  
                    @risk_type_id               -- RWH (08/06/01)  
  
    While @@fetch_Status = 0  
  
    Begin  
  
        -- Insert the details into the Lead commission  
  
        Insert into Lead_Commission (   Insurance_File_cnt ,  
                            Commission_Band,  
                        Premium,  
                        [Percent],  
                        Value,  
                        risk_type_id    )  
                Values      (   @Insurance_file_cnt,  
                        @Commission_band_id,  
                        @Premium,  
                        @commission_percentage,  
                        @commission_value,  
                        @risk_type_id   )       -- RWH (08/06/01)  
  
        -- Fetch the next record  
        Fetch next from Agent_Comm_Cursor into  @Commission_band_id ,  
                            @Commission_Percentage,  
                            @Premium,  
                            @Commission_value,  
                            @risk_type_id       -- RWH (08/06/01)  
  
    End  
  
    --Close and Deallocate the Cursor  
    Close Agent_Comm_Cursor  
  
    Deallocate Agent_Comm_Cursor  
  
END  
GO